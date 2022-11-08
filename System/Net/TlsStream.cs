using System;
using System.Collections;
using System.Globalization;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000558 RID: 1368
	internal class TlsStream : NetworkStream, IDisposable
	{
		// Token: 0x0600298A RID: 10634 RVA: 0x000AE2E0 File Offset: 0x000AD2E0
		public TlsStream(string destinationHost, NetworkStream networkStream, X509CertificateCollection clientCertificates, ServicePoint servicePoint, object initiatingRequest, ExecutionContext executionContext) : base(networkStream, true)
		{
			this._ExecutionContext = executionContext;
			if (this._ExecutionContext == null)
			{
				this._ExecutionContext = ExecutionContext.Capture();
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, ".ctor", "host=" + destinationHost + ", #certs=" + ((clientCertificates == null) ? "null" : clientCertificates.Count.ToString(NumberFormatInfo.InvariantInfo)));
			}
			this.m_ExceptionStatus = WebExceptionStatus.SecureChannelFailure;
			this.m_Worker = new SslState(networkStream, initiatingRequest is HttpWebRequest);
			this.m_DestinationHost = destinationHost;
			this.m_ClientCertificates = clientCertificates;
			RemoteCertValidationCallback certValidationDelegate = servicePoint.SetupHandshakeDoneProcedure(this, initiatingRequest);
			this.m_Worker.SetCertValidationDelegate(certValidationDelegate);
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x0600298B RID: 10635 RVA: 0x000AE3A2 File Offset: 0x000AD3A2
		internal WebExceptionStatus ExceptionStatus
		{
			get
			{
				return this.m_ExceptionStatus;
			}
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x000AE3AC File Offset: 0x000AD3AC
		protected override void Dispose(bool disposing)
		{
			if (Interlocked.Exchange(ref this.m_ShutDown, 1) == 1)
			{
				return;
			}
			try
			{
				if (disposing)
				{
					this.m_CachedChannelBinding = this.GetChannelBinding(ChannelBindingKind.Endpoint);
					this.m_Worker.Close();
				}
				else
				{
					this.m_Worker = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x0600298D RID: 10637 RVA: 0x000AE408 File Offset: 0x000AD408
		public override bool DataAvailable
		{
			get
			{
				return this.m_Worker.DataAvailable || base.DataAvailable;
			}
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x000AE420 File Offset: 0x000AD420
		public override int Read(byte[] buffer, int offset, int size)
		{
			if (!this.m_Worker.IsAuthenticated)
			{
				this.ProcessAuthentication(null);
			}
			int result;
			try
			{
				result = this.m_Worker.SecureStream.Read(buffer, offset, size);
			}
			catch
			{
				if (this.m_Worker.IsCertValidationFailed)
				{
					this.m_ExceptionStatus = WebExceptionStatus.TrustFailure;
				}
				else if (this.m_Worker.LastSecurityStatus != SecurityStatus.OK)
				{
					this.m_ExceptionStatus = WebExceptionStatus.SecureChannelFailure;
				}
				else
				{
					this.m_ExceptionStatus = WebExceptionStatus.ReceiveFailure;
				}
				throw;
			}
			return result;
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x000AE4A4 File Offset: 0x000AD4A4
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback asyncCallback, object asyncState)
		{
			if (!this.m_Worker.IsAuthenticated)
			{
				BufferAsyncResult result = new BufferAsyncResult(this, buffer, offset, size, false, asyncState, asyncCallback);
				if (this.ProcessAuthentication(result))
				{
					return result;
				}
			}
			IAsyncResult result2;
			try
			{
				result2 = this.m_Worker.SecureStream.BeginRead(buffer, offset, size, asyncCallback, asyncState);
			}
			catch
			{
				if (this.m_Worker.IsCertValidationFailed)
				{
					this.m_ExceptionStatus = WebExceptionStatus.TrustFailure;
				}
				else if (this.m_Worker.LastSecurityStatus != SecurityStatus.OK)
				{
					this.m_ExceptionStatus = WebExceptionStatus.SecureChannelFailure;
				}
				else
				{
					this.m_ExceptionStatus = WebExceptionStatus.ReceiveFailure;
				}
				throw;
			}
			return result2;
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x000AE53C File Offset: 0x000AD53C
		internal override IAsyncResult UnsafeBeginRead(byte[] buffer, int offset, int size, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginRead(buffer, offset, size, asyncCallback, asyncState);
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x000AE54C File Offset: 0x000AD54C
		public override int EndRead(IAsyncResult asyncResult)
		{
			int result;
			try
			{
				BufferAsyncResult bufferAsyncResult = asyncResult as BufferAsyncResult;
				if (bufferAsyncResult == null || bufferAsyncResult.AsyncObject != this)
				{
					result = this.m_Worker.SecureStream.EndRead(asyncResult);
				}
				else
				{
					bufferAsyncResult.InternalWaitForCompletion();
					Exception ex = bufferAsyncResult.Result as Exception;
					if (ex != null)
					{
						throw ex;
					}
					result = (int)bufferAsyncResult.Result;
				}
			}
			catch
			{
				if (this.m_Worker.IsCertValidationFailed)
				{
					this.m_ExceptionStatus = WebExceptionStatus.TrustFailure;
				}
				else if (this.m_Worker.LastSecurityStatus != SecurityStatus.OK)
				{
					this.m_ExceptionStatus = WebExceptionStatus.SecureChannelFailure;
				}
				else
				{
					this.m_ExceptionStatus = WebExceptionStatus.ReceiveFailure;
				}
				throw;
			}
			return result;
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x000AE5F0 File Offset: 0x000AD5F0
		public override void Write(byte[] buffer, int offset, int size)
		{
			if (!this.m_Worker.IsAuthenticated)
			{
				this.ProcessAuthentication(null);
			}
			try
			{
				this.m_Worker.SecureStream.Write(buffer, offset, size);
			}
			catch
			{
				if (this.m_Worker.IsCertValidationFailed)
				{
					this.m_ExceptionStatus = WebExceptionStatus.TrustFailure;
				}
				else if (this.m_Worker.LastSecurityStatus != SecurityStatus.OK)
				{
					this.m_ExceptionStatus = WebExceptionStatus.SecureChannelFailure;
				}
				else
				{
					this.m_ExceptionStatus = WebExceptionStatus.SendFailure;
				}
				Socket socket = base.Socket;
				if (socket != null)
				{
					socket.InternalShutdown(SocketShutdown.Both);
				}
				throw;
			}
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x000AE680 File Offset: 0x000AD680
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback asyncCallback, object asyncState)
		{
			if (!this.m_Worker.IsAuthenticated)
			{
				BufferAsyncResult result = new BufferAsyncResult(this, buffer, offset, size, true, asyncState, asyncCallback);
				if (this.ProcessAuthentication(result))
				{
					return result;
				}
			}
			IAsyncResult result2;
			try
			{
				result2 = this.m_Worker.SecureStream.BeginWrite(buffer, offset, size, asyncCallback, asyncState);
			}
			catch
			{
				if (this.m_Worker.IsCertValidationFailed)
				{
					this.m_ExceptionStatus = WebExceptionStatus.TrustFailure;
				}
				else if (this.m_Worker.LastSecurityStatus != SecurityStatus.OK)
				{
					this.m_ExceptionStatus = WebExceptionStatus.SecureChannelFailure;
				}
				else
				{
					this.m_ExceptionStatus = WebExceptionStatus.SendFailure;
				}
				throw;
			}
			return result2;
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x000AE718 File Offset: 0x000AD718
		internal override IAsyncResult UnsafeBeginWrite(byte[] buffer, int offset, int size, AsyncCallback asyncCallback, object asyncState)
		{
			return this.BeginWrite(buffer, offset, size, asyncCallback, asyncState);
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x000AE728 File Offset: 0x000AD728
		public override void EndWrite(IAsyncResult asyncResult)
		{
			try
			{
				BufferAsyncResult bufferAsyncResult = asyncResult as BufferAsyncResult;
				if (bufferAsyncResult == null || bufferAsyncResult.AsyncObject != this)
				{
					this.m_Worker.SecureStream.EndWrite(asyncResult);
				}
				else
				{
					bufferAsyncResult.InternalWaitForCompletion();
					Exception ex = bufferAsyncResult.Result as Exception;
					if (ex != null)
					{
						throw ex;
					}
				}
			}
			catch
			{
				Socket socket = base.Socket;
				if (socket != null)
				{
					socket.InternalShutdown(SocketShutdown.Both);
				}
				if (this.m_Worker.IsCertValidationFailed)
				{
					this.m_ExceptionStatus = WebExceptionStatus.TrustFailure;
				}
				else if (this.m_Worker.LastSecurityStatus != SecurityStatus.OK)
				{
					this.m_ExceptionStatus = WebExceptionStatus.SecureChannelFailure;
				}
				else
				{
					this.m_ExceptionStatus = WebExceptionStatus.SendFailure;
				}
				throw;
			}
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x000AE7D0 File Offset: 0x000AD7D0
		internal override void MultipleWrite(BufferOffsetSize[] buffers)
		{
			if (!this.m_Worker.IsAuthenticated)
			{
				this.ProcessAuthentication(null);
			}
			try
			{
				this.m_Worker.SecureStream.Write(buffers);
			}
			catch
			{
				Socket socket = base.Socket;
				if (socket != null)
				{
					socket.InternalShutdown(SocketShutdown.Both);
				}
				if (this.m_Worker.IsCertValidationFailed)
				{
					this.m_ExceptionStatus = WebExceptionStatus.TrustFailure;
				}
				else if (this.m_Worker.LastSecurityStatus != SecurityStatus.OK)
				{
					this.m_ExceptionStatus = WebExceptionStatus.SecureChannelFailure;
				}
				else
				{
					this.m_ExceptionStatus = WebExceptionStatus.SendFailure;
				}
				throw;
			}
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x000AE860 File Offset: 0x000AD860
		internal override IAsyncResult BeginMultipleWrite(BufferOffsetSize[] buffers, AsyncCallback callback, object state)
		{
			if (!this.m_Worker.IsAuthenticated)
			{
				BufferAsyncResult result = new BufferAsyncResult(this, buffers, state, callback);
				if (this.ProcessAuthentication(result))
				{
					return result;
				}
			}
			IAsyncResult result2;
			try
			{
				result2 = this.m_Worker.SecureStream.BeginWrite(buffers, callback, state);
			}
			catch
			{
				if (this.m_Worker.IsCertValidationFailed)
				{
					this.m_ExceptionStatus = WebExceptionStatus.TrustFailure;
				}
				else if (this.m_Worker.LastSecurityStatus != SecurityStatus.OK)
				{
					this.m_ExceptionStatus = WebExceptionStatus.SecureChannelFailure;
				}
				else
				{
					this.m_ExceptionStatus = WebExceptionStatus.SendFailure;
				}
				throw;
			}
			return result2;
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x000AE8F0 File Offset: 0x000AD8F0
		internal override IAsyncResult UnsafeBeginMultipleWrite(BufferOffsetSize[] buffers, AsyncCallback callback, object state)
		{
			return this.BeginMultipleWrite(buffers, callback, state);
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x000AE8FB File Offset: 0x000AD8FB
		internal override void EndMultipleWrite(IAsyncResult asyncResult)
		{
			this.EndWrite(asyncResult);
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x0600299A RID: 10650 RVA: 0x000AE904 File Offset: 0x000AD904
		public X509Certificate ClientCertificate
		{
			get
			{
				return this.m_Worker.InternalLocalCertificate;
			}
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x000AE911 File Offset: 0x000AD911
		internal ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			if (kind == ChannelBindingKind.Endpoint && this.m_CachedChannelBinding != null)
			{
				return this.m_CachedChannelBinding;
			}
			return this.m_Worker.GetChannelBinding(kind);
		}

		// Token: 0x0600299C RID: 10652 RVA: 0x000AE934 File Offset: 0x000AD934
		internal bool ProcessAuthentication(LazyAsyncResult result)
		{
			bool flag = false;
			bool flag2 = result == null;
			lock (this.m_PendingIO)
			{
				if (this.m_Worker.IsAuthenticated)
				{
					return false;
				}
				if (this.m_PendingIO.Count == 0)
				{
					flag = true;
				}
				if (flag2)
				{
					result = new LazyAsyncResult(this, null, null);
				}
				this.m_PendingIO.Add(result);
			}
			try
			{
				if (flag)
				{
					bool flag3 = true;
					LazyAsyncResult lazyAsyncResult = null;
					try
					{
						try
						{
							this.m_Worker.ValidateCreateContext(false, this.m_DestinationHost, (SslProtocols)ServicePointManager.SecurityProtocol, null, this.m_ClientCertificates, true, ServicePointManager.CheckCertificateRevocationList, ServicePointManager.CheckCertificateName);
							if (!flag2)
							{
								lazyAsyncResult = new LazyAsyncResult(this.m_Worker, null, new AsyncCallback(this.WakeupPendingIO));
							}
							if (this._ExecutionContext != null)
							{
								ExecutionContext.Run(this._ExecutionContext.CreateCopy(), new ContextCallback(this.CallProcessAuthentication), lazyAsyncResult);
							}
							else
							{
								this.m_Worker.ProcessAuthentication(lazyAsyncResult);
							}
						}
						catch
						{
							flag3 = false;
							throw;
						}
						goto IL_14C;
					}
					finally
					{
						if (flag2 || !flag3)
						{
							lock (this.m_PendingIO)
							{
								if (this.m_PendingIO.Count > 1)
								{
									ThreadPool.QueueUserWorkItem(new WaitCallback(this.StartWakeupPendingIO), null);
								}
								else
								{
									this.m_PendingIO.Clear();
								}
							}
						}
					}
				}
				if (flag2)
				{
					Exception ex = result.InternalWaitForCompletion() as Exception;
					if (ex != null)
					{
						throw ex;
					}
				}
				IL_14C:;
			}
			catch
			{
				if (this.m_Worker.IsCertValidationFailed)
				{
					this.m_ExceptionStatus = WebExceptionStatus.TrustFailure;
				}
				else if (this.m_Worker.LastSecurityStatus != SecurityStatus.OK)
				{
					this.m_ExceptionStatus = WebExceptionStatus.SecureChannelFailure;
				}
				else
				{
					this.m_ExceptionStatus = WebExceptionStatus.ReceiveFailure;
				}
				throw;
			}
			return true;
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x000AEB0C File Offset: 0x000ADB0C
		private void CallProcessAuthentication(object state)
		{
			this.m_Worker.ProcessAuthentication((LazyAsyncResult)state);
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x000AEB1F File Offset: 0x000ADB1F
		private void StartWakeupPendingIO(object nullState)
		{
			this.WakeupPendingIO(null);
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x000AEB28 File Offset: 0x000ADB28
		private void WakeupPendingIO(IAsyncResult ar)
		{
			Exception result = null;
			try
			{
				if (ar != null)
				{
					this.m_Worker.EndProcessAuthentication(ar);
				}
			}
			catch (Exception ex)
			{
				result = ex;
				if (this.m_Worker.IsCertValidationFailed)
				{
					this.m_ExceptionStatus = WebExceptionStatus.TrustFailure;
				}
				else if (this.m_Worker.LastSecurityStatus != SecurityStatus.OK)
				{
					this.m_ExceptionStatus = WebExceptionStatus.SecureChannelFailure;
				}
				else
				{
					this.m_ExceptionStatus = WebExceptionStatus.ReceiveFailure;
				}
			}
			lock (this.m_PendingIO)
			{
				while (this.m_PendingIO.Count != 0)
				{
					LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)this.m_PendingIO[this.m_PendingIO.Count - 1];
					this.m_PendingIO.RemoveAt(this.m_PendingIO.Count - 1);
					if (lazyAsyncResult is BufferAsyncResult)
					{
						if (this.m_PendingIO.Count == 0)
						{
							this.ResumeIOWorker(lazyAsyncResult);
						}
						else
						{
							ThreadPool.QueueUserWorkItem(new WaitCallback(this.ResumeIOWorker), lazyAsyncResult);
						}
					}
					else
					{
						try
						{
							lazyAsyncResult.InvokeCallback(result);
						}
						catch
						{
						}
					}
				}
			}
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x000AEC48 File Offset: 0x000ADC48
		private void ResumeIOWorker(object result)
		{
			BufferAsyncResult bufferAsyncResult = (BufferAsyncResult)result;
			try
			{
				this.ResumeIO(bufferAsyncResult);
			}
			catch (Exception ex)
			{
				if (ex is OutOfMemoryException || ex is StackOverflowException || ex is ThreadAbortException)
				{
					throw;
				}
				if (bufferAsyncResult.InternalPeekCompleted)
				{
					throw;
				}
				bufferAsyncResult.InvokeCallback(ex);
			}
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x000AECA4 File Offset: 0x000ADCA4
		private void ResumeIO(BufferAsyncResult bufferResult)
		{
			IAsyncResult asyncResult;
			if (bufferResult.IsWrite)
			{
				if (bufferResult.Buffers != null)
				{
					asyncResult = this.m_Worker.SecureStream.BeginWrite(bufferResult.Buffers, TlsStream._CompleteIOCallback, bufferResult);
				}
				else
				{
					asyncResult = this.m_Worker.SecureStream.BeginWrite(bufferResult.Buffer, bufferResult.Offset, bufferResult.Count, TlsStream._CompleteIOCallback, bufferResult);
				}
			}
			else
			{
				asyncResult = this.m_Worker.SecureStream.BeginRead(bufferResult.Buffer, bufferResult.Offset, bufferResult.Count, TlsStream._CompleteIOCallback, bufferResult);
			}
			if (asyncResult.CompletedSynchronously)
			{
				TlsStream.CompleteIO(asyncResult);
			}
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x000AED44 File Offset: 0x000ADD44
		private static void CompleteIOCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			try
			{
				TlsStream.CompleteIO(result);
			}
			catch (Exception ex)
			{
				if (ex is OutOfMemoryException || ex is StackOverflowException || ex is ThreadAbortException)
				{
					throw;
				}
				if (((LazyAsyncResult)result.AsyncState).InternalPeekCompleted)
				{
					throw;
				}
				((LazyAsyncResult)result.AsyncState).InvokeCallback(ex);
			}
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x000AEDB4 File Offset: 0x000ADDB4
		private static void CompleteIO(IAsyncResult result)
		{
			BufferAsyncResult bufferAsyncResult = (BufferAsyncResult)result.AsyncState;
			object result2 = null;
			if (bufferAsyncResult.IsWrite)
			{
				((TlsStream)bufferAsyncResult.AsyncObject).m_Worker.SecureStream.EndWrite(result);
			}
			else
			{
				result2 = ((TlsStream)bufferAsyncResult.AsyncObject).m_Worker.SecureStream.EndRead(result);
			}
			bufferAsyncResult.InvokeCallback(result2);
		}

		// Token: 0x0400286F RID: 10351
		private SslState m_Worker;

		// Token: 0x04002870 RID: 10352
		private WebExceptionStatus m_ExceptionStatus;

		// Token: 0x04002871 RID: 10353
		private string m_DestinationHost;

		// Token: 0x04002872 RID: 10354
		private X509CertificateCollection m_ClientCertificates;

		// Token: 0x04002873 RID: 10355
		private static AsyncCallback _CompleteIOCallback = new AsyncCallback(TlsStream.CompleteIOCallback);

		// Token: 0x04002874 RID: 10356
		private ExecutionContext _ExecutionContext;

		// Token: 0x04002875 RID: 10357
		private ChannelBinding m_CachedChannelBinding;

		// Token: 0x04002876 RID: 10358
		private int m_ShutDown;

		// Token: 0x04002877 RID: 10359
		private ArrayList m_PendingIO = new ArrayList();
	}
}

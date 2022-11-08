using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020004DC RID: 1244
	internal class FtpDataStream : Stream, ICloseEx
	{
		// Token: 0x060026B2 RID: 9906 RVA: 0x0009F6AC File Offset: 0x0009E6AC
		internal FtpDataStream(NetworkStream networkStream, FtpWebRequest request, TriState writeOnly)
		{
			this.m_Readable = true;
			this.m_Writeable = true;
			if (writeOnly == TriState.True)
			{
				this.m_Readable = false;
			}
			else if (writeOnly == TriState.False)
			{
				this.m_Writeable = false;
			}
			this.m_NetworkStream = networkStream;
			this.m_Request = request;
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x0009F6E8 File Offset: 0x0009E6E8
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					((ICloseEx)this).CloseEx(CloseExState.Normal);
				}
				else
				{
					((ICloseEx)this).CloseEx(CloseExState.Abort | CloseExState.Silent);
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x0009F724 File Offset: 0x0009E724
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			lock (this)
			{
				if (this.m_Closing)
				{
					return;
				}
				this.m_Closing = true;
				this.m_Writeable = false;
				this.m_Readable = false;
			}
			try
			{
				try
				{
					if ((closeState & CloseExState.Abort) == CloseExState.Normal)
					{
						this.m_NetworkStream.Close(-1);
					}
					else
					{
						this.m_NetworkStream.Close(0);
					}
				}
				finally
				{
					this.m_Request.DataStreamClosed(closeState);
				}
			}
			catch (Exception ex)
			{
				bool flag = true;
				WebException ex2 = ex as WebException;
				if (ex2 != null)
				{
					FtpWebResponse ftpWebResponse = ex2.Response as FtpWebResponse;
					if (ftpWebResponse != null && !this.m_IsFullyRead && ftpWebResponse.StatusCode == FtpStatusCode.ConnectionClosed)
					{
						flag = false;
					}
				}
				if (flag && (closeState & CloseExState.Silent) == CloseExState.Normal)
				{
					throw;
				}
			}
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x0009F800 File Offset: 0x0009E800
		private void CheckError()
		{
			if (this.m_Request.Aborted)
			{
				throw new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x060026B6 RID: 9910 RVA: 0x0009F821 File Offset: 0x0009E821
		public override bool CanRead
		{
			get
			{
				return this.m_Readable;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x060026B7 RID: 9911 RVA: 0x0009F829 File Offset: 0x0009E829
		public override bool CanSeek
		{
			get
			{
				return this.m_NetworkStream.CanSeek;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x060026B8 RID: 9912 RVA: 0x0009F836 File Offset: 0x0009E836
		public override bool CanWrite
		{
			get
			{
				return this.m_Writeable;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x060026B9 RID: 9913 RVA: 0x0009F83E File Offset: 0x0009E83E
		public override long Length
		{
			get
			{
				return this.m_NetworkStream.Length;
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x060026BA RID: 9914 RVA: 0x0009F84B File Offset: 0x0009E84B
		// (set) Token: 0x060026BB RID: 9915 RVA: 0x0009F858 File Offset: 0x0009E858
		public override long Position
		{
			get
			{
				return this.m_NetworkStream.Position;
			}
			set
			{
				this.m_NetworkStream.Position = value;
			}
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x0009F868 File Offset: 0x0009E868
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.CheckError();
			long result;
			try
			{
				result = this.m_NetworkStream.Seek(offset, origin);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			return result;
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x0009F8A8 File Offset: 0x0009E8A8
		public override int Read(byte[] buffer, int offset, int size)
		{
			this.CheckError();
			int num;
			try
			{
				num = this.m_NetworkStream.Read(buffer, offset, size);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			if (num == 0)
			{
				this.m_IsFullyRead = true;
				this.Close();
			}
			return num;
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x0009F8F8 File Offset: 0x0009E8F8
		public override void Write(byte[] buffer, int offset, int size)
		{
			this.CheckError();
			try
			{
				this.m_NetworkStream.Write(buffer, offset, size);
			}
			catch
			{
				this.CheckError();
				throw;
			}
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x0009F934 File Offset: 0x0009E934
		private void AsyncReadCallback(IAsyncResult ar)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)ar.AsyncState;
			try
			{
				try
				{
					int num = this.m_NetworkStream.EndRead(ar);
					if (num == 0)
					{
						this.m_IsFullyRead = true;
						this.Close();
					}
					lazyAsyncResult.InvokeCallback(num);
				}
				catch (Exception result)
				{
					if (!lazyAsyncResult.IsCompleted)
					{
						lazyAsyncResult.InvokeCallback(result);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x0009F9AC File Offset: 0x0009E9AC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			this.CheckError();
			LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(this, state, callback);
			try
			{
				this.m_NetworkStream.BeginRead(buffer, offset, size, new AsyncCallback(this.AsyncReadCallback), lazyAsyncResult);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			return lazyAsyncResult;
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x0009FA04 File Offset: 0x0009EA04
		public override int EndRead(IAsyncResult ar)
		{
			int result;
			try
			{
				object obj = ((LazyAsyncResult)ar).InternalWaitForCompletion();
				if (obj is Exception)
				{
					throw (Exception)obj;
				}
				result = (int)obj;
			}
			finally
			{
				this.CheckError();
			}
			return result;
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x0009FA50 File Offset: 0x0009EA50
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			this.CheckError();
			IAsyncResult result;
			try
			{
				result = this.m_NetworkStream.BeginWrite(buffer, offset, size, callback, state);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			return result;
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x0009FA94 File Offset: 0x0009EA94
		public override void EndWrite(IAsyncResult asyncResult)
		{
			try
			{
				this.m_NetworkStream.EndWrite(asyncResult);
			}
			finally
			{
				this.CheckError();
			}
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x0009FAC8 File Offset: 0x0009EAC8
		public override void Flush()
		{
			this.m_NetworkStream.Flush();
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x0009FAD5 File Offset: 0x0009EAD5
		public override void SetLength(long value)
		{
			this.m_NetworkStream.SetLength(value);
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x060026C6 RID: 9926 RVA: 0x0009FAE3 File Offset: 0x0009EAE3
		public override bool CanTimeout
		{
			get
			{
				return this.m_NetworkStream.CanTimeout;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x060026C7 RID: 9927 RVA: 0x0009FAF0 File Offset: 0x0009EAF0
		// (set) Token: 0x060026C8 RID: 9928 RVA: 0x0009FAFD File Offset: 0x0009EAFD
		public override int ReadTimeout
		{
			get
			{
				return this.m_NetworkStream.ReadTimeout;
			}
			set
			{
				this.m_NetworkStream.ReadTimeout = value;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x060026C9 RID: 9929 RVA: 0x0009FB0B File Offset: 0x0009EB0B
		// (set) Token: 0x060026CA RID: 9930 RVA: 0x0009FB18 File Offset: 0x0009EB18
		public override int WriteTimeout
		{
			get
			{
				return this.m_NetworkStream.WriteTimeout;
			}
			set
			{
				this.m_NetworkStream.WriteTimeout = value;
			}
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x0009FB26 File Offset: 0x0009EB26
		internal void SetSocketTimeoutOption(SocketShutdown mode, int timeout, bool silent)
		{
			this.m_NetworkStream.SetSocketTimeoutOption(mode, timeout, silent);
		}

		// Token: 0x0400264D RID: 9805
		private FtpWebRequest m_Request;

		// Token: 0x0400264E RID: 9806
		private NetworkStream m_NetworkStream;

		// Token: 0x0400264F RID: 9807
		private bool m_Writeable;

		// Token: 0x04002650 RID: 9808
		private bool m_Readable;

		// Token: 0x04002651 RID: 9809
		private bool m_IsFullyRead;

		// Token: 0x04002652 RID: 9810
		private bool m_Closing;
	}
}

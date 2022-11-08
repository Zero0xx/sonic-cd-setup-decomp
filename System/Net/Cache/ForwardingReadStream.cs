using System;
using System.IO;
using System.Threading;

namespace System.Net.Cache
{
	// Token: 0x0200057A RID: 1402
	internal class ForwardingReadStream : Stream, ICloseEx
	{
		// Token: 0x06002AC2 RID: 10946 RVA: 0x000B5EE0 File Offset: 0x000B4EE0
		internal ForwardingReadStream(Stream originalStream, Stream shadowStream, long bytesToSkip, bool throwOnWriteError)
		{
			if (!shadowStream.CanWrite)
			{
				throw new ArgumentException(SR.GetString("net_cache_shadowstream_not_writable"), "shadowStream");
			}
			this.m_OriginalStream = originalStream;
			this.m_ShadowStream = shadowStream;
			this.m_BytesToSkip = bytesToSkip;
			this.m_ThrowOnWriteError = throwOnWriteError;
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06002AC3 RID: 10947 RVA: 0x000B5F2D File Offset: 0x000B4F2D
		public override bool CanRead
		{
			get
			{
				return this.m_OriginalStream.CanRead;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06002AC4 RID: 10948 RVA: 0x000B5F3A File Offset: 0x000B4F3A
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06002AC5 RID: 10949 RVA: 0x000B5F3D File Offset: 0x000B4F3D
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06002AC6 RID: 10950 RVA: 0x000B5F40 File Offset: 0x000B4F40
		public override long Length
		{
			get
			{
				return this.m_OriginalStream.Length - this.m_BytesToSkip;
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06002AC7 RID: 10951 RVA: 0x000B5F54 File Offset: 0x000B4F54
		// (set) Token: 0x06002AC8 RID: 10952 RVA: 0x000B5F68 File Offset: 0x000B4F68
		public override long Position
		{
			get
			{
				return this.m_OriginalStream.Position - this.m_BytesToSkip;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x000B5F79 File Offset: 0x000B4F79
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x000B5F8A File Offset: 0x000B4F8A
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x000B5F9B File Offset: 0x000B4F9B
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x000B5FAC File Offset: 0x000B4FAC
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x000B5FBD File Offset: 0x000B4FBD
		public override void EndWrite(IAsyncResult asyncResult)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x000B5FCE File Offset: 0x000B4FCE
		public override void Flush()
		{
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x000B5FD0 File Offset: 0x000B4FD0
		public override int Read(byte[] buffer, int offset, int count)
		{
			bool flag = false;
			int num = -1;
			if (Interlocked.Increment(ref this.m_ReadNesting) != 1)
			{
				throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[]
				{
					"Read",
					"read"
				}));
			}
			int result;
			try
			{
				if (this.m_BytesToSkip != 0L)
				{
					byte[] array = new byte[4096];
					while (this.m_BytesToSkip != 0L)
					{
						int num2 = this.m_OriginalStream.Read(array, 0, (this.m_BytesToSkip < (long)array.Length) ? ((int)this.m_BytesToSkip) : array.Length);
						if (num2 == 0)
						{
							this.m_SeenReadEOF = true;
						}
						this.m_BytesToSkip -= (long)num2;
						if (!this.m_ShadowStreamIsDead)
						{
							this.m_ShadowStream.Write(array, 0, num2);
						}
					}
				}
				num = this.m_OriginalStream.Read(buffer, offset, count);
				if (num == 0)
				{
					this.m_SeenReadEOF = true;
				}
				if (this.m_ShadowStreamIsDead)
				{
					result = num;
				}
				else
				{
					flag = true;
					this.m_ShadowStream.Write(buffer, offset, num);
					result = num;
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (!this.m_ShadowStreamIsDead)
				{
					this.m_ShadowStreamIsDead = true;
					try
					{
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
					catch (Exception)
					{
						if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
						{
							throw;
						}
					}
					catch
					{
					}
				}
				if (!flag || this.m_ThrowOnWriteError)
				{
					throw;
				}
				result = num;
			}
			catch
			{
				if (!this.m_ShadowStreamIsDead)
				{
					this.m_ShadowStreamIsDead = true;
					try
					{
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
					catch (Exception exception)
					{
						if (NclUtilities.IsFatal(exception))
						{
							throw;
						}
					}
					catch
					{
					}
				}
				if (!flag || this.m_ThrowOnWriteError)
				{
					throw;
				}
				result = num;
			}
			finally
			{
				Interlocked.Decrement(ref this.m_ReadNesting);
			}
			return result;
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x000B6280 File Offset: 0x000B5280
		private void ReadCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			object asyncState = transportResult.AsyncState;
			this.ReadComplete(transportResult);
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x000B629C File Offset: 0x000B529C
		private void ReadComplete(IAsyncResult transportResult)
		{
			for (;;)
			{
				ForwardingReadStream.InnerAsyncResult innerAsyncResult = transportResult.AsyncState as ForwardingReadStream.InnerAsyncResult;
				try
				{
					if (!innerAsyncResult.IsWriteCompletion)
					{
						innerAsyncResult.Count = this.m_OriginalStream.EndRead(transportResult);
						if (innerAsyncResult.Count == 0)
						{
							this.m_SeenReadEOF = true;
						}
						if (!this.m_ShadowStreamIsDead)
						{
							innerAsyncResult.IsWriteCompletion = true;
							transportResult = this.m_ShadowStream.BeginWrite(innerAsyncResult.Buffer, innerAsyncResult.Offset, innerAsyncResult.Count, this.m_ReadCallback, innerAsyncResult);
							if (transportResult.CompletedSynchronously)
							{
								continue;
							}
							break;
						}
					}
					else
					{
						this.m_ShadowStream.EndWrite(transportResult);
						innerAsyncResult.IsWriteCompletion = false;
					}
				}
				catch (Exception result)
				{
					if (innerAsyncResult.InternalPeekCompleted)
					{
						throw;
					}
					try
					{
						this.m_ShadowStreamIsDead = true;
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
					catch (Exception)
					{
					}
					catch
					{
					}
					if (!innerAsyncResult.IsWriteCompletion || this.m_ThrowOnWriteError)
					{
						if (transportResult.CompletedSynchronously)
						{
							throw;
						}
						innerAsyncResult.InvokeCallback(result);
						break;
					}
				}
				catch
				{
					if (innerAsyncResult.InternalPeekCompleted)
					{
						throw;
					}
					try
					{
						this.m_ShadowStreamIsDead = true;
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
					catch (Exception)
					{
					}
					catch
					{
					}
					if (!innerAsyncResult.IsWriteCompletion || this.m_ThrowOnWriteError)
					{
						if (transportResult.CompletedSynchronously)
						{
							throw;
						}
						innerAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
						break;
					}
				}
				try
				{
					if (this.m_BytesToSkip != 0L)
					{
						this.m_BytesToSkip -= (long)innerAsyncResult.Count;
						innerAsyncResult.Count = ((this.m_BytesToSkip < (long)innerAsyncResult.Buffer.Length) ? ((int)this.m_BytesToSkip) : innerAsyncResult.Buffer.Length);
						if (this.m_BytesToSkip == 0L)
						{
							transportResult = innerAsyncResult;
							innerAsyncResult = (innerAsyncResult.AsyncState as ForwardingReadStream.InnerAsyncResult);
						}
						transportResult = this.m_OriginalStream.BeginRead(innerAsyncResult.Buffer, innerAsyncResult.Offset, innerAsyncResult.Count, this.m_ReadCallback, innerAsyncResult);
						if (transportResult.CompletedSynchronously)
						{
							continue;
						}
					}
					else
					{
						innerAsyncResult.InvokeCallback(innerAsyncResult.Count);
					}
				}
				catch (Exception result2)
				{
					if (innerAsyncResult.InternalPeekCompleted)
					{
						throw;
					}
					try
					{
						this.m_ShadowStreamIsDead = true;
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
					catch (Exception)
					{
					}
					catch
					{
					}
					if (transportResult.CompletedSynchronously)
					{
						throw;
					}
					innerAsyncResult.InvokeCallback(result2);
				}
				catch
				{
					if (innerAsyncResult.InternalPeekCompleted)
					{
						throw;
					}
					try
					{
						this.m_ShadowStreamIsDead = true;
						if (this.m_ShadowStream is ICloseEx)
						{
							((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
						}
						else
						{
							this.m_ShadowStream.Close();
						}
					}
					catch (Exception)
					{
					}
					catch
					{
					}
					if (transportResult.CompletedSynchronously)
					{
						throw;
					}
					innerAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
				}
				break;
			}
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x000B6624 File Offset: 0x000B5624
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (Interlocked.Increment(ref this.m_ReadNesting) != 1)
			{
				throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[]
				{
					"BeginRead",
					"read"
				}));
			}
			IAsyncResult result;
			try
			{
				if (this.m_ReadCallback == null)
				{
					this.m_ReadCallback = new AsyncCallback(this.ReadCallback);
				}
				if (this.m_ShadowStreamIsDead && this.m_BytesToSkip == 0L)
				{
					result = this.m_OriginalStream.BeginRead(buffer, offset, count, callback, state);
				}
				else
				{
					ForwardingReadStream.InnerAsyncResult innerAsyncResult = new ForwardingReadStream.InnerAsyncResult(state, callback, buffer, offset, count);
					if (this.m_BytesToSkip != 0L)
					{
						ForwardingReadStream.InnerAsyncResult userState = innerAsyncResult;
						innerAsyncResult = new ForwardingReadStream.InnerAsyncResult(userState, null, new byte[4096], 0, (this.m_BytesToSkip < (long)buffer.Length) ? ((int)this.m_BytesToSkip) : buffer.Length);
					}
					IAsyncResult asyncResult = this.m_OriginalStream.BeginRead(innerAsyncResult.Buffer, innerAsyncResult.Offset, innerAsyncResult.Count, this.m_ReadCallback, innerAsyncResult);
					if (asyncResult.CompletedSynchronously)
					{
						this.ReadComplete(asyncResult);
					}
					result = innerAsyncResult;
				}
			}
			catch
			{
				Interlocked.Decrement(ref this.m_ReadNesting);
				throw;
			}
			return result;
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x000B674C File Offset: 0x000B574C
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (Interlocked.Decrement(ref this.m_ReadNesting) != 0)
			{
				Interlocked.Increment(ref this.m_ReadNesting);
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndRead"
				}));
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			ForwardingReadStream.InnerAsyncResult innerAsyncResult = asyncResult as ForwardingReadStream.InnerAsyncResult;
			if (innerAsyncResult == null && this.m_OriginalStream.EndRead(asyncResult) == 0)
			{
				this.m_SeenReadEOF = true;
			}
			bool flag = false;
			try
			{
				innerAsyncResult.InternalWaitForCompletion();
				if (innerAsyncResult.Result is Exception)
				{
					throw (Exception)innerAsyncResult.Result;
				}
				flag = true;
			}
			finally
			{
				if (!flag && !this.m_ShadowStreamIsDead)
				{
					this.m_ShadowStreamIsDead = true;
					if (this.m_ShadowStream is ICloseEx)
					{
						((ICloseEx)this.m_ShadowStream).CloseEx(CloseExState.Abort | CloseExState.Silent);
					}
					else
					{
						this.m_ShadowStream.Close();
					}
				}
			}
			return (int)innerAsyncResult.Result;
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x000B6840 File Offset: 0x000B5840
		protected sealed override void Dispose(bool disposing)
		{
			this.Dispose(disposing, CloseExState.Normal);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x000B6850 File Offset: 0x000B5850
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			if (Interlocked.Increment(ref this._Disposed) == 1)
			{
				if (closeState == CloseExState.Silent)
				{
					try
					{
						int num = 0;
						int num2;
						while (num < ConnectStream.s_DrainingBuffer.Length && (num2 = this.Read(ConnectStream.s_DrainingBuffer, 0, ConnectStream.s_DrainingBuffer.Length)) > 0)
						{
							num += num2;
						}
					}
					catch (Exception ex)
					{
						if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
						{
							throw;
						}
					}
					catch
					{
					}
				}
				this.Dispose(true, closeState);
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x000B68E8 File Offset: 0x000B58E8
		protected virtual void Dispose(bool disposing, CloseExState closeState)
		{
			try
			{
				ICloseEx closeEx = this.m_OriginalStream as ICloseEx;
				if (closeEx != null)
				{
					closeEx.CloseEx(closeState);
				}
				else
				{
					this.m_OriginalStream.Close();
				}
			}
			finally
			{
				if (!this.m_SeenReadEOF)
				{
					closeState |= CloseExState.Abort;
				}
				if (this.m_ShadowStream is ICloseEx)
				{
					((ICloseEx)this.m_ShadowStream).CloseEx(closeState);
				}
				else
				{
					this.m_ShadowStream.Close();
				}
			}
			if (!disposing)
			{
				this.m_OriginalStream = null;
				this.m_ShadowStream = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06002AD7 RID: 10967 RVA: 0x000B697C File Offset: 0x000B597C
		public override bool CanTimeout
		{
			get
			{
				return this.m_OriginalStream.CanTimeout && this.m_ShadowStream.CanTimeout;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06002AD8 RID: 10968 RVA: 0x000B6998 File Offset: 0x000B5998
		// (set) Token: 0x06002AD9 RID: 10969 RVA: 0x000B69A8 File Offset: 0x000B59A8
		public override int ReadTimeout
		{
			get
			{
				return this.m_OriginalStream.ReadTimeout;
			}
			set
			{
				Stream originalStream = this.m_OriginalStream;
				this.m_ShadowStream.ReadTimeout = value;
				originalStream.ReadTimeout = value;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06002ADA RID: 10970 RVA: 0x000B69CF File Offset: 0x000B59CF
		// (set) Token: 0x06002ADB RID: 10971 RVA: 0x000B69DC File Offset: 0x000B59DC
		public override int WriteTimeout
		{
			get
			{
				return this.m_ShadowStream.WriteTimeout;
			}
			set
			{
				Stream originalStream = this.m_OriginalStream;
				this.m_ShadowStream.WriteTimeout = value;
				originalStream.WriteTimeout = value;
			}
		}

		// Token: 0x04002980 RID: 10624
		private Stream m_OriginalStream;

		// Token: 0x04002981 RID: 10625
		private Stream m_ShadowStream;

		// Token: 0x04002982 RID: 10626
		private int m_ReadNesting;

		// Token: 0x04002983 RID: 10627
		private bool m_ShadowStreamIsDead;

		// Token: 0x04002984 RID: 10628
		private AsyncCallback m_ReadCallback;

		// Token: 0x04002985 RID: 10629
		private long m_BytesToSkip;

		// Token: 0x04002986 RID: 10630
		private bool m_ThrowOnWriteError;

		// Token: 0x04002987 RID: 10631
		private bool m_SeenReadEOF;

		// Token: 0x04002988 RID: 10632
		private int _Disposed;

		// Token: 0x0200057B RID: 1403
		private class InnerAsyncResult : LazyAsyncResult
		{
			// Token: 0x06002ADC RID: 10972 RVA: 0x000B6A03 File Offset: 0x000B5A03
			public InnerAsyncResult(object userState, AsyncCallback userCallback, byte[] buffer, int offset, int count) : base(null, userState, userCallback)
			{
				this.Buffer = buffer;
				this.Offset = offset;
				this.Count = count;
			}

			// Token: 0x04002989 RID: 10633
			public byte[] Buffer;

			// Token: 0x0400298A RID: 10634
			public int Offset;

			// Token: 0x0400298B RID: 10635
			public int Count;

			// Token: 0x0400298C RID: 10636
			public bool IsWriteCompletion;
		}
	}
}

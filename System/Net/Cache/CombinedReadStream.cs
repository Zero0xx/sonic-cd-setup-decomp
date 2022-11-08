using System;
using System.IO;
using System.Threading;

namespace System.Net.Cache
{
	// Token: 0x02000578 RID: 1400
	internal class CombinedReadStream : Stream, ICloseEx
	{
		// Token: 0x06002AA8 RID: 10920 RVA: 0x000B5890 File Offset: 0x000B4890
		internal CombinedReadStream(Stream headStream, Stream tailStream)
		{
			this.m_HeadStream = headStream;
			this.m_TailStream = tailStream;
			this.m_HeadEOF = (headStream == Stream.Null);
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06002AA9 RID: 10921 RVA: 0x000B58B4 File Offset: 0x000B48B4
		public override bool CanRead
		{
			get
			{
				if (!this.m_HeadEOF)
				{
					return this.m_HeadStream.CanRead;
				}
				return this.m_TailStream.CanRead;
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06002AAA RID: 10922 RVA: 0x000B58D5 File Offset: 0x000B48D5
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06002AAB RID: 10923 RVA: 0x000B58D8 File Offset: 0x000B48D8
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06002AAC RID: 10924 RVA: 0x000B58DB File Offset: 0x000B48DB
		public override long Length
		{
			get
			{
				return this.m_TailStream.Length + (this.m_HeadEOF ? this.m_HeadLength : this.m_HeadStream.Length);
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06002AAD RID: 10925 RVA: 0x000B5904 File Offset: 0x000B4904
		// (set) Token: 0x06002AAE RID: 10926 RVA: 0x000B592D File Offset: 0x000B492D
		public override long Position
		{
			get
			{
				return this.m_TailStream.Position + (this.m_HeadEOF ? this.m_HeadLength : this.m_HeadStream.Position);
			}
			set
			{
				throw new NotSupportedException(SR.GetString("net_noseek"));
			}
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x000B593E File Offset: 0x000B493E
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x000B594F File Offset: 0x000B494F
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002AB1 RID: 10929 RVA: 0x000B5960 File Offset: 0x000B4960
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x000B5971 File Offset: 0x000B4971
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002AB3 RID: 10931 RVA: 0x000B5982 File Offset: 0x000B4982
		public override void EndWrite(IAsyncResult asyncResult)
		{
			throw new NotSupportedException(SR.GetString("net_noseek"));
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x000B5993 File Offset: 0x000B4993
		public override void Flush()
		{
		}

		// Token: 0x06002AB5 RID: 10933 RVA: 0x000B5998 File Offset: 0x000B4998
		public override int Read(byte[] buffer, int offset, int count)
		{
			int result;
			try
			{
				if (Interlocked.Increment(ref this.m_ReadNesting) != 1)
				{
					throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[]
					{
						"Read",
						"read"
					}));
				}
				if (this.m_HeadEOF)
				{
					result = this.m_TailStream.Read(buffer, offset, count);
				}
				else
				{
					int num = this.m_HeadStream.Read(buffer, offset, count);
					this.m_HeadLength += (long)num;
					if (num == 0 && count != 0)
					{
						this.m_HeadEOF = true;
						this.m_HeadStream.Close();
						num = this.m_TailStream.Read(buffer, offset, count);
					}
					result = num;
				}
			}
			finally
			{
				Interlocked.Decrement(ref this.m_ReadNesting);
			}
			return result;
		}

		// Token: 0x06002AB6 RID: 10934 RVA: 0x000B5A5C File Offset: 0x000B4A5C
		private void ReadCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			CombinedReadStream.InnerAsyncResult innerAsyncResult = transportResult.AsyncState as CombinedReadStream.InnerAsyncResult;
			try
			{
				int num;
				if (!this.m_HeadEOF)
				{
					num = this.m_HeadStream.EndRead(transportResult);
					this.m_HeadLength += (long)num;
				}
				else
				{
					num = this.m_TailStream.EndRead(transportResult);
				}
				if (!this.m_HeadEOF && num == 0 && innerAsyncResult.Count != 0)
				{
					this.m_HeadEOF = true;
					this.m_HeadStream.Close();
					IAsyncResult asyncResult = this.m_TailStream.BeginRead(innerAsyncResult.Buffer, innerAsyncResult.Offset, innerAsyncResult.Count, this.m_ReadCallback, innerAsyncResult);
					if (!asyncResult.CompletedSynchronously)
					{
						return;
					}
					num = this.m_TailStream.EndRead(asyncResult);
				}
				innerAsyncResult.Buffer = null;
				innerAsyncResult.InvokeCallback(num);
			}
			catch (Exception result)
			{
				if (innerAsyncResult.InternalPeekCompleted)
				{
					throw;
				}
				innerAsyncResult.InvokeCallback(result);
			}
			catch
			{
				if (innerAsyncResult.InternalPeekCompleted)
				{
					throw;
				}
				innerAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x000B5B7C File Offset: 0x000B4B7C
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			IAsyncResult result;
			try
			{
				if (Interlocked.Increment(ref this.m_ReadNesting) != 1)
				{
					throw new NotSupportedException(SR.GetString("net_io_invalidnestedcall", new object[]
					{
						"BeginRead",
						"read"
					}));
				}
				if (this.m_ReadCallback == null)
				{
					this.m_ReadCallback = new AsyncCallback(this.ReadCallback);
				}
				if (this.m_HeadEOF)
				{
					result = this.m_TailStream.BeginRead(buffer, offset, count, callback, state);
				}
				else
				{
					CombinedReadStream.InnerAsyncResult innerAsyncResult = new CombinedReadStream.InnerAsyncResult(state, callback, buffer, offset, count);
					IAsyncResult asyncResult = this.m_HeadStream.BeginRead(buffer, offset, count, this.m_ReadCallback, innerAsyncResult);
					if (!asyncResult.CompletedSynchronously)
					{
						result = innerAsyncResult;
					}
					else
					{
						int num = this.m_HeadStream.EndRead(asyncResult);
						this.m_HeadLength += (long)num;
						if (num == 0 && innerAsyncResult.Count != 0)
						{
							this.m_HeadEOF = true;
							this.m_HeadStream.Close();
							result = this.m_TailStream.BeginRead(buffer, offset, count, callback, state);
						}
						else
						{
							innerAsyncResult.Buffer = null;
							innerAsyncResult.InvokeCallback(count);
							result = innerAsyncResult;
						}
					}
				}
			}
			catch
			{
				Interlocked.Decrement(ref this.m_ReadNesting);
				throw;
			}
			return result;
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x000B5CC0 File Offset: 0x000B4CC0
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
			CombinedReadStream.InnerAsyncResult innerAsyncResult = asyncResult as CombinedReadStream.InnerAsyncResult;
			if (innerAsyncResult == null)
			{
				if (!this.m_HeadEOF)
				{
					return this.m_HeadStream.EndRead(asyncResult);
				}
				return this.m_TailStream.EndRead(asyncResult);
			}
			else
			{
				innerAsyncResult.InternalWaitForCompletion();
				if (innerAsyncResult.Result is Exception)
				{
					throw (Exception)innerAsyncResult.Result;
				}
				return (int)innerAsyncResult.Result;
			}
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x000B5D6B File Offset: 0x000B4D6B
		protected sealed override void Dispose(bool disposing)
		{
			this.Dispose(disposing, CloseExState.Normal);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x000B5D7B File Offset: 0x000B4D7B
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			this.Dispose(true, closeState);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x000B5D8C File Offset: 0x000B4D8C
		protected virtual void Dispose(bool disposing, CloseExState closeState)
		{
			try
			{
				if (!this.m_HeadEOF)
				{
					ICloseEx closeEx = this.m_HeadStream as ICloseEx;
					if (closeEx != null)
					{
						closeEx.CloseEx(closeState);
					}
					else
					{
						this.m_HeadStream.Close();
					}
				}
			}
			finally
			{
				ICloseEx closeEx2 = this.m_TailStream as ICloseEx;
				if (closeEx2 != null)
				{
					closeEx2.CloseEx(closeState);
				}
				else
				{
					this.m_TailStream.Close();
				}
			}
			if (!disposing)
			{
				this.m_HeadStream = null;
				this.m_TailStream = null;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06002ABC RID: 10940 RVA: 0x000B5E0C File Offset: 0x000B4E0C
		public override bool CanTimeout
		{
			get
			{
				return this.m_TailStream.CanTimeout && this.m_HeadStream.CanTimeout;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06002ABD RID: 10941 RVA: 0x000B5E28 File Offset: 0x000B4E28
		// (set) Token: 0x06002ABE RID: 10942 RVA: 0x000B5E4C File Offset: 0x000B4E4C
		public override int ReadTimeout
		{
			get
			{
				if (!this.m_HeadEOF)
				{
					return this.m_HeadStream.ReadTimeout;
				}
				return this.m_TailStream.ReadTimeout;
			}
			set
			{
				Stream tailStream = this.m_TailStream;
				this.m_HeadStream.ReadTimeout = value;
				tailStream.ReadTimeout = value;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06002ABF RID: 10943 RVA: 0x000B5E73 File Offset: 0x000B4E73
		// (set) Token: 0x06002AC0 RID: 10944 RVA: 0x000B5E94 File Offset: 0x000B4E94
		public override int WriteTimeout
		{
			get
			{
				if (!this.m_HeadEOF)
				{
					return this.m_HeadStream.WriteTimeout;
				}
				return this.m_TailStream.WriteTimeout;
			}
			set
			{
				Stream tailStream = this.m_TailStream;
				this.m_HeadStream.WriteTimeout = value;
				tailStream.WriteTimeout = value;
			}
		}

		// Token: 0x04002977 RID: 10615
		private Stream m_HeadStream;

		// Token: 0x04002978 RID: 10616
		private Stream m_TailStream;

		// Token: 0x04002979 RID: 10617
		private bool m_HeadEOF;

		// Token: 0x0400297A RID: 10618
		private long m_HeadLength;

		// Token: 0x0400297B RID: 10619
		private int m_ReadNesting;

		// Token: 0x0400297C RID: 10620
		private AsyncCallback m_ReadCallback;

		// Token: 0x02000579 RID: 1401
		private class InnerAsyncResult : LazyAsyncResult
		{
			// Token: 0x06002AC1 RID: 10945 RVA: 0x000B5EBB File Offset: 0x000B4EBB
			public InnerAsyncResult(object userState, AsyncCallback userCallback, byte[] buffer, int offset, int count) : base(null, userState, userCallback)
			{
				this.Buffer = buffer;
				this.Offset = offset;
				this.Count = count;
			}

			// Token: 0x0400297D RID: 10621
			public byte[] Buffer;

			// Token: 0x0400297E RID: 10622
			public int Offset;

			// Token: 0x0400297F RID: 10623
			public int Count;
		}
	}
}

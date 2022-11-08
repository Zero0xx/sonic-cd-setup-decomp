using System;
using System.IO;

namespace System.Net
{
	// Token: 0x02000681 RID: 1665
	internal class BufferedReadStream : DelegatedStream
	{
		// Token: 0x0600338F RID: 13199 RVA: 0x000D9C2A File Offset: 0x000D8C2A
		internal BufferedReadStream(Stream stream) : this(stream, false)
		{
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x000D9C34 File Offset: 0x000D8C34
		internal BufferedReadStream(Stream stream, bool readMore) : base(stream)
		{
			this.readMore = readMore;
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x06003391 RID: 13201 RVA: 0x000D9C44 File Offset: 0x000D8C44
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x06003392 RID: 13202 RVA: 0x000D9C47 File Offset: 0x000D8C47
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x000D9C4C File Offset: 0x000D8C4C
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			BufferedReadStream.ReadAsyncResult readAsyncResult = new BufferedReadStream.ReadAsyncResult(this, callback, state);
			readAsyncResult.Read(buffer, offset, count);
			return readAsyncResult;
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x000D9C70 File Offset: 0x000D8C70
		public override int EndRead(IAsyncResult asyncResult)
		{
			return BufferedReadStream.ReadAsyncResult.End(asyncResult);
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x000D9C88 File Offset: 0x000D8C88
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = 0;
			if (this.storedOffset < this.storedLength)
			{
				num = Math.Min(count, this.storedLength - this.storedOffset);
				Buffer.BlockCopy(this.storedBuffer, this.storedOffset, buffer, offset, num);
				this.storedOffset += num;
				if (num == count || !this.readMore)
				{
					return num;
				}
				offset += num;
				count -= num;
			}
			return num + base.Read(buffer, offset, count);
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x000D9D00 File Offset: 0x000D8D00
		public override int ReadByte()
		{
			if (this.storedOffset < this.storedLength)
			{
				return (int)this.storedBuffer[this.storedOffset++];
			}
			return base.ReadByte();
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x000D9D3C File Offset: 0x000D8D3C
		internal void Push(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			if (this.storedOffset == this.storedLength)
			{
				if (this.storedBuffer == null || this.storedBuffer.Length < count)
				{
					this.storedBuffer = new byte[count];
				}
				this.storedOffset = 0;
				this.storedLength = count;
			}
			else if (count <= this.storedOffset)
			{
				this.storedOffset -= count;
			}
			else if (count <= this.storedBuffer.Length - this.storedLength + this.storedOffset)
			{
				Buffer.BlockCopy(this.storedBuffer, this.storedOffset, this.storedBuffer, count, this.storedLength - this.storedOffset);
				this.storedLength += count - this.storedOffset;
				this.storedOffset = 0;
			}
			else
			{
				byte[] dst = new byte[count + this.storedLength - this.storedOffset];
				Buffer.BlockCopy(this.storedBuffer, this.storedOffset, dst, count, this.storedLength - this.storedOffset);
				this.storedLength += count - this.storedOffset;
				this.storedOffset = 0;
				this.storedBuffer = dst;
			}
			Buffer.BlockCopy(buffer, offset, this.storedBuffer, this.storedOffset, count);
		}

		// Token: 0x04002F9B RID: 12187
		private byte[] storedBuffer;

		// Token: 0x04002F9C RID: 12188
		private int storedLength;

		// Token: 0x04002F9D RID: 12189
		private int storedOffset;

		// Token: 0x04002F9E RID: 12190
		private bool readMore;

		// Token: 0x02000682 RID: 1666
		private class ReadAsyncResult : LazyAsyncResult
		{
			// Token: 0x06003398 RID: 13208 RVA: 0x000D9E71 File Offset: 0x000D8E71
			internal ReadAsyncResult(BufferedReadStream parent, AsyncCallback callback, object state) : base(null, state, callback)
			{
				this.parent = parent;
			}

			// Token: 0x06003399 RID: 13209 RVA: 0x000D9E84 File Offset: 0x000D8E84
			internal void Read(byte[] buffer, int offset, int count)
			{
				if (this.parent.storedOffset < this.parent.storedLength)
				{
					this.read = Math.Min(count, this.parent.storedLength - this.parent.storedOffset);
					Buffer.BlockCopy(this.parent.storedBuffer, this.parent.storedOffset, buffer, offset, this.read);
					this.parent.storedOffset += this.read;
					if (this.read == count || !this.parent.readMore)
					{
						base.InvokeCallback();
						return;
					}
					count -= this.read;
					offset += this.read;
				}
				IAsyncResult asyncResult = this.parent.BaseStream.BeginRead(buffer, offset, count, BufferedReadStream.ReadAsyncResult.onRead, this);
				if (asyncResult.CompletedSynchronously)
				{
					this.read += this.parent.BaseStream.EndRead(asyncResult);
					base.InvokeCallback();
				}
			}

			// Token: 0x0600339A RID: 13210 RVA: 0x000D9F84 File Offset: 0x000D8F84
			internal static int End(IAsyncResult result)
			{
				BufferedReadStream.ReadAsyncResult readAsyncResult = (BufferedReadStream.ReadAsyncResult)result;
				readAsyncResult.InternalWaitForCompletion();
				return readAsyncResult.read;
			}

			// Token: 0x0600339B RID: 13211 RVA: 0x000D9FA8 File Offset: 0x000D8FA8
			private static void OnRead(IAsyncResult result)
			{
				if (!result.CompletedSynchronously)
				{
					BufferedReadStream.ReadAsyncResult readAsyncResult = (BufferedReadStream.ReadAsyncResult)result.AsyncState;
					try
					{
						readAsyncResult.read += readAsyncResult.parent.BaseStream.EndRead(result);
						readAsyncResult.InvokeCallback();
					}
					catch (Exception result2)
					{
						if (readAsyncResult.IsCompleted)
						{
							throw;
						}
						readAsyncResult.InvokeCallback(result2);
					}
					catch
					{
						if (readAsyncResult.IsCompleted)
						{
							throw;
						}
						readAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
					}
				}
			}

			// Token: 0x04002F9F RID: 12191
			private BufferedReadStream parent;

			// Token: 0x04002FA0 RID: 12192
			private int read;

			// Token: 0x04002FA1 RID: 12193
			private static AsyncCallback onRead = new AsyncCallback(BufferedReadStream.ReadAsyncResult.OnRead);
		}
	}
}

using System;
using System.IO;

namespace System.Security.Cryptography
{
	// Token: 0x0200088E RID: 2190
	internal sealed class TailStream : Stream
	{
		// Token: 0x06004FB1 RID: 20401 RVA: 0x001154AF File Offset: 0x001144AF
		public TailStream(int bufferSize)
		{
			this._Buffer = new byte[bufferSize];
			this._BufferSize = bufferSize;
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x001154CA File Offset: 0x001144CA
		public void Clear()
		{
			this.Close();
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x001154D4 File Offset: 0x001144D4
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._Buffer != null)
					{
						Array.Clear(this._Buffer, 0, this._Buffer.Length);
					}
					this._Buffer = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06004FB4 RID: 20404 RVA: 0x00115524 File Offset: 0x00114524
		public byte[] Buffer
		{
			get
			{
				return (byte[])this._Buffer.Clone();
			}
		}

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06004FB5 RID: 20405 RVA: 0x00115536 File Offset: 0x00114536
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x06004FB6 RID: 20406 RVA: 0x00115539 File Offset: 0x00114539
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x06004FB7 RID: 20407 RVA: 0x0011553C File Offset: 0x0011453C
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x06004FB8 RID: 20408 RVA: 0x0011553F File Offset: 0x0011453F
		public override long Length
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnseekableStream"));
			}
		}

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x06004FB9 RID: 20409 RVA: 0x00115550 File Offset: 0x00114550
		// (set) Token: 0x06004FBA RID: 20410 RVA: 0x00115561 File Offset: 0x00114561
		public override long Position
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnseekableStream"));
			}
			set
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnseekableStream"));
			}
		}

		// Token: 0x06004FBB RID: 20411 RVA: 0x00115572 File Offset: 0x00114572
		public override void Flush()
		{
		}

		// Token: 0x06004FBC RID: 20412 RVA: 0x00115574 File Offset: 0x00114574
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnseekableStream"));
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x00115585 File Offset: 0x00114585
		public override void SetLength(long value)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnseekableStream"));
		}

		// Token: 0x06004FBE RID: 20414 RVA: 0x00115596 File Offset: 0x00114596
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnreadableStream"));
		}

		// Token: 0x06004FBF RID: 20415 RVA: 0x001155A8 File Offset: 0x001145A8
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			if (this._BufferFull)
			{
				if (count > this._BufferSize)
				{
					System.Buffer.InternalBlockCopy(buffer, offset + count - this._BufferSize, this._Buffer, 0, this._BufferSize);
					return;
				}
				System.Buffer.InternalBlockCopy(this._Buffer, this._BufferSize - count, this._Buffer, 0, this._BufferSize - count);
				System.Buffer.InternalBlockCopy(buffer, offset, this._Buffer, this._BufferSize - count, count);
				return;
			}
			else
			{
				if (count > this._BufferSize)
				{
					System.Buffer.InternalBlockCopy(buffer, offset + count - this._BufferSize, this._Buffer, 0, this._BufferSize);
					this._BufferFull = true;
					return;
				}
				if (count + this._BufferIndex >= this._BufferSize)
				{
					System.Buffer.InternalBlockCopy(this._Buffer, this._BufferIndex + count - this._BufferSize, this._Buffer, 0, this._BufferSize - count);
					System.Buffer.InternalBlockCopy(buffer, offset, this._Buffer, this._BufferIndex, count);
					this._BufferFull = true;
					return;
				}
				System.Buffer.InternalBlockCopy(buffer, offset, this._Buffer, this._BufferIndex, count);
				this._BufferIndex += count;
				return;
			}
		}

		// Token: 0x04002913 RID: 10515
		private byte[] _Buffer;

		// Token: 0x04002914 RID: 10516
		private int _BufferSize;

		// Token: 0x04002915 RID: 10517
		private int _BufferIndex;

		// Token: 0x04002916 RID: 10518
		private bool _BufferFull;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005D1 RID: 1489
	internal sealed class UnmanagedMemoryStreamWrapper : MemoryStream
	{
		// Token: 0x0600379A RID: 14234 RVA: 0x000BB7BF File Offset: 0x000BA7BF
		internal UnmanagedMemoryStreamWrapper(UnmanagedMemoryStream stream)
		{
			this._unmanagedStream = stream;
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x0600379B RID: 14235 RVA: 0x000BB7CE File Offset: 0x000BA7CE
		public override bool CanRead
		{
			get
			{
				return this._unmanagedStream.CanRead;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x0600379C RID: 14236 RVA: 0x000BB7DB File Offset: 0x000BA7DB
		public override bool CanSeek
		{
			get
			{
				return this._unmanagedStream.CanSeek;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x0600379D RID: 14237 RVA: 0x000BB7E8 File Offset: 0x000BA7E8
		public override bool CanWrite
		{
			get
			{
				return this._unmanagedStream.CanWrite;
			}
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x000BB7F8 File Offset: 0x000BA7F8
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this._unmanagedStream.Close();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x000BB830 File Offset: 0x000BA830
		public override void Flush()
		{
			this._unmanagedStream.Flush();
		}

		// Token: 0x060037A0 RID: 14240 RVA: 0x000BB83D File Offset: 0x000BA83D
		public override byte[] GetBuffer()
		{
			throw new UnauthorizedAccessException(Environment.GetResourceString("UnauthorizedAccess_MemStreamBuffer"));
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x060037A1 RID: 14241 RVA: 0x000BB84E File Offset: 0x000BA84E
		// (set) Token: 0x060037A2 RID: 14242 RVA: 0x000BB85C File Offset: 0x000BA85C
		public override int Capacity
		{
			get
			{
				return (int)this._unmanagedStream.Capacity;
			}
			set
			{
				throw new IOException(Environment.GetResourceString("IO.IO_FixedCapacity"));
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x060037A3 RID: 14243 RVA: 0x000BB86D File Offset: 0x000BA86D
		public override long Length
		{
			get
			{
				return this._unmanagedStream.Length;
			}
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x060037A4 RID: 14244 RVA: 0x000BB87A File Offset: 0x000BA87A
		// (set) Token: 0x060037A5 RID: 14245 RVA: 0x000BB887 File Offset: 0x000BA887
		public override long Position
		{
			get
			{
				return this._unmanagedStream.Position;
			}
			set
			{
				this._unmanagedStream.Position = value;
			}
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x000BB895 File Offset: 0x000BA895
		public override int Read([In] [Out] byte[] buffer, int offset, int count)
		{
			return this._unmanagedStream.Read(buffer, offset, count);
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x000BB8A5 File Offset: 0x000BA8A5
		public override int ReadByte()
		{
			return this._unmanagedStream.ReadByte();
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x000BB8B2 File Offset: 0x000BA8B2
		public override long Seek(long offset, SeekOrigin loc)
		{
			return this._unmanagedStream.Seek(offset, loc);
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x000BB8C4 File Offset: 0x000BA8C4
		public override byte[] ToArray()
		{
			if (!this._unmanagedStream._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if (!this._unmanagedStream.CanRead)
			{
				__Error.ReadNotSupported();
			}
			byte[] array = new byte[this._unmanagedStream.Length];
			Buffer.memcpy(this._unmanagedStream.Pointer, 0, array, 0, (int)this._unmanagedStream.Length);
			return array;
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x000BB927 File Offset: 0x000BA927
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._unmanagedStream.Write(buffer, offset, count);
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x000BB937 File Offset: 0x000BA937
		public override void WriteByte(byte value)
		{
			this._unmanagedStream.WriteByte(value);
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x000BB948 File Offset: 0x000BA948
		public override void WriteTo(Stream stream)
		{
			if (!this._unmanagedStream._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if (!this._unmanagedStream.CanRead)
			{
				__Error.ReadNotSupported();
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream", Environment.GetResourceString("ArgumentNull_Stream"));
			}
			byte[] array = this.ToArray();
			stream.Write(array, 0, array.Length);
		}

		// Token: 0x04001CDE RID: 7390
		private UnmanagedMemoryStream _unmanagedStream;
	}
}

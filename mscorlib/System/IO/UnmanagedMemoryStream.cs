using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.IO
{
	// Token: 0x020005C2 RID: 1474
	[CLSCompliant(false)]
	public class UnmanagedMemoryStream : Stream
	{
		// Token: 0x060036AD RID: 13997 RVA: 0x000B8DF4 File Offset: 0x000B7DF4
		protected UnmanagedMemoryStream()
		{
			this._mem = null;
			this._isOpen = false;
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x000B8E0B File Offset: 0x000B7E0B
		public unsafe UnmanagedMemoryStream(byte* pointer, long length)
		{
			this.Initialize(pointer, length, length, FileAccess.Read, false);
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x000B8E1E File Offset: 0x000B7E1E
		public unsafe UnmanagedMemoryStream(byte* pointer, long length, long capacity, FileAccess access)
		{
			this.Initialize(pointer, length, capacity, access, false);
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x000B8E32 File Offset: 0x000B7E32
		internal unsafe UnmanagedMemoryStream(byte* pointer, long length, long capacity, FileAccess access, bool skipSecurityCheck)
		{
			this.Initialize(pointer, length, capacity, access, skipSecurityCheck);
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x000B8E47 File Offset: 0x000B7E47
		protected unsafe void Initialize(byte* pointer, long length, long capacity, FileAccess access)
		{
			this.Initialize(pointer, length, capacity, access, false);
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x000B8E58 File Offset: 0x000B7E58
		internal unsafe void Initialize(byte* pointer, long length, long capacity, FileAccess access, bool skipSecurityCheck)
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer");
			}
			if (length < 0L || capacity < 0L)
			{
				throw new ArgumentOutOfRangeException((length < 0L) ? "length" : "capacity", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (length > capacity)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_LengthGreaterThanCapacity"));
			}
			if (pointer + capacity < pointer)
			{
				throw new ArgumentOutOfRangeException("capacity", Environment.GetResourceString("ArgumentOutOfRange_UnmanagedMemStreamWrapAround"));
			}
			if (access < FileAccess.Read || access > FileAccess.ReadWrite)
			{
				throw new ArgumentOutOfRangeException("access", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
			}
			if (this._isOpen)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CalledTwice"));
			}
			if (!skipSecurityCheck)
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			}
			this._mem = pointer;
			this._length = length;
			this._capacity = capacity;
			this._access = access;
			this._isOpen = true;
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x060036B3 RID: 14003 RVA: 0x000B8F40 File Offset: 0x000B7F40
		public override bool CanRead
		{
			get
			{
				return this._isOpen && (this._access & FileAccess.Read) != (FileAccess)0;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x060036B4 RID: 14004 RVA: 0x000B8F5A File Offset: 0x000B7F5A
		public override bool CanSeek
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x060036B5 RID: 14005 RVA: 0x000B8F62 File Offset: 0x000B7F62
		public override bool CanWrite
		{
			get
			{
				return this._isOpen && (this._access & FileAccess.Write) != (FileAccess)0;
			}
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x000B8F7C File Offset: 0x000B7F7C
		protected override void Dispose(bool disposing)
		{
			this._isOpen = false;
			base.Dispose(disposing);
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x000B8F8C File Offset: 0x000B7F8C
		public override void Flush()
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x060036B8 RID: 14008 RVA: 0x000B8F9B File Offset: 0x000B7F9B
		public override long Length
		{
			get
			{
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				return this._length;
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060036B9 RID: 14009 RVA: 0x000B8FB0 File Offset: 0x000B7FB0
		public long Capacity
		{
			get
			{
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				return this._capacity;
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060036BA RID: 14010 RVA: 0x000B8FC5 File Offset: 0x000B7FC5
		// (set) Token: 0x060036BB RID: 14011 RVA: 0x000B8FDA File Offset: 0x000B7FDA
		public override long Position
		{
			get
			{
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				return this._position;
			}
			set
			{
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				this._position = value;
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060036BC RID: 14012 RVA: 0x000B900C File Offset: 0x000B800C
		// (set) Token: 0x060036BD RID: 14013 RVA: 0x000B9054 File Offset: 0x000B8054
		public unsafe byte* PositionPointer
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				long position = this._position;
				if (position > this._capacity)
				{
					throw new IndexOutOfRangeException(Environment.GetResourceString("IndexOutOfRange_UMSPosition"));
				}
				byte* result = this._mem + position;
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				return result;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				if (new IntPtr((long)(value - this._mem)).ToInt64() > 9223372036854775807L)
				{
					throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_UnmanagedMemStreamLength"));
				}
				if (value < this._mem)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_SeekBeforeBegin"));
				}
				this._position = (long)(value - this._mem);
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060036BE RID: 14014 RVA: 0x000B90D0 File Offset: 0x000B80D0
		internal unsafe byte* Pointer
		{
			get
			{
				return this._mem;
			}
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x000B90D8 File Offset: 0x000B80D8
		public override int Read([In] [Out] byte[] buffer, int offset, int count)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if ((this._access & FileAccess.Read) == (FileAccess)0)
			{
				__Error.ReadNotSupported();
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			long position = this._position;
			long num = this._length - position;
			if (num > (long)count)
			{
				num = (long)count;
			}
			if (num <= 0L)
			{
				return 0;
			}
			int num2 = (int)num;
			if (num2 < 0)
			{
				num2 = 0;
			}
			Buffer.memcpy(this._mem + position, 0, buffer, offset, num2);
			this._position = position + num;
			return num2;
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x000B91A8 File Offset: 0x000B81A8
		public unsafe override int ReadByte()
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if ((this._access & FileAccess.Read) == (FileAccess)0)
			{
				__Error.ReadNotSupported();
			}
			long position = this._position;
			if (position >= this._length)
			{
				return -1;
			}
			this._position = position + 1L;
			return (int)this._mem[position];
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x000B91F8 File Offset: 0x000B81F8
		public override long Seek(long offset, SeekOrigin loc)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if (offset > 9223372036854775807L)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_UnmanagedMemStreamLength"));
			}
			switch (loc)
			{
			case SeekOrigin.Begin:
				if (offset < 0L)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_SeekBeforeBegin"));
				}
				this._position = offset;
				break;
			case SeekOrigin.Current:
				if (offset + this._position < 0L)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_SeekBeforeBegin"));
				}
				this._position += offset;
				break;
			case SeekOrigin.End:
				if (this._length + offset < 0L)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_SeekBeforeBegin"));
				}
				this._position = this._length + offset;
				break;
			default:
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidSeekOrigin"));
			}
			return this._position;
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x000B92D8 File Offset: 0x000B82D8
		public override void SetLength(long value)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if ((this._access & FileAccess.Write) == (FileAccess)0)
			{
				__Error.WriteNotSupported();
			}
			if (value < 0L)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (value > this._capacity)
			{
				throw new IOException(Environment.GetResourceString("IO.IO_FixedCapacity"));
			}
			long length = this._length;
			if (value > length)
			{
				Buffer.ZeroMemory(this._mem + length, value - length);
			}
			this._length = value;
			if (this._position > value)
			{
				this._position = value;
			}
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x000B9368 File Offset: 0x000B8368
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if ((this._access & FileAccess.Write) == (FileAccess)0)
			{
				__Error.WriteNotSupported();
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			long position = this._position;
			long length = this._length;
			long num = position + (long)count;
			if (num < 0L)
			{
				throw new IOException(Environment.GetResourceString("IO.IO_StreamTooLong"));
			}
			if (num > length)
			{
				if (num > this._capacity)
				{
					throw new NotSupportedException(Environment.GetResourceString("IO.IO_FixedCapacity"));
				}
				this._length = num;
			}
			if (position > length)
			{
				Buffer.ZeroMemory(this._mem + length, position - length);
			}
			Buffer.memcpy(buffer, offset, this._mem + position, 0, count);
			this._position = num;
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x000B9470 File Offset: 0x000B8470
		public unsafe override void WriteByte(byte value)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if ((this._access & FileAccess.Write) == (FileAccess)0)
			{
				__Error.WriteNotSupported();
			}
			long position = this._position;
			long length = this._length;
			long num = position + 1L;
			if (position >= length)
			{
				if (num < 0L)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_StreamTooLong"));
				}
				if (num > this._capacity)
				{
					throw new NotSupportedException(Environment.GetResourceString("IO.IO_FixedCapacity"));
				}
				this._length = num;
				if (position > length)
				{
					Buffer.ZeroMemory(this._mem + length, position - length);
				}
			}
			this._mem[position] = value;
			this._position = num;
		}

		// Token: 0x04001C9F RID: 7327
		private const long UnmanagedMemStreamMaxLength = 9223372036854775807L;

		// Token: 0x04001CA0 RID: 7328
		private unsafe byte* _mem;

		// Token: 0x04001CA1 RID: 7329
		private long _length;

		// Token: 0x04001CA2 RID: 7330
		private long _capacity;

		// Token: 0x04001CA3 RID: 7331
		private long _position;

		// Token: 0x04001CA4 RID: 7332
		private FileAccess _access;

		// Token: 0x04001CA5 RID: 7333
		internal bool _isOpen;
	}
}

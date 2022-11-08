using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO
{
	// Token: 0x020005A7 RID: 1447
	[ComVisible(true)]
	[Serializable]
	public class BinaryWriter : IDisposable
	{
		// Token: 0x06003500 RID: 13568 RVA: 0x000AF710 File Offset: 0x000AE710
		protected BinaryWriter()
		{
			this.OutStream = Stream.Null;
			this._buffer = new byte[16];
			this._encoding = new UTF8Encoding(false, true);
			this._encoder = this._encoding.GetEncoder();
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x000AF765 File Offset: 0x000AE765
		public BinaryWriter(Stream output) : this(output, new UTF8Encoding(false, true))
		{
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x000AF778 File Offset: 0x000AE778
		public BinaryWriter(Stream output, Encoding encoding)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (!output.CanWrite)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_StreamNotWritable"));
			}
			this.OutStream = output;
			this._buffer = new byte[16];
			this._encoding = encoding;
			this._encoder = this._encoding.GetEncoder();
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x000AF7F7 File Offset: 0x000AE7F7
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x000AF800 File Offset: 0x000AE800
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.OutStream.Close();
			}
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x000AF810 File Offset: 0x000AE810
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06003506 RID: 13574 RVA: 0x000AF819 File Offset: 0x000AE819
		public virtual Stream BaseStream
		{
			get
			{
				this.Flush();
				return this.OutStream;
			}
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x000AF827 File Offset: 0x000AE827
		public virtual void Flush()
		{
			this.OutStream.Flush();
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x000AF834 File Offset: 0x000AE834
		public virtual long Seek(int offset, SeekOrigin origin)
		{
			return this.OutStream.Seek((long)offset, origin);
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x000AF844 File Offset: 0x000AE844
		public virtual void Write(bool value)
		{
			this._buffer[0] = (value ? 1 : 0);
			this.OutStream.Write(this._buffer, 0, 1);
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x000AF869 File Offset: 0x000AE869
		public virtual void Write(byte value)
		{
			this.OutStream.WriteByte(value);
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x000AF877 File Offset: 0x000AE877
		[CLSCompliant(false)]
		public virtual void Write(sbyte value)
		{
			this.OutStream.WriteByte((byte)value);
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x000AF886 File Offset: 0x000AE886
		public virtual void Write(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.OutStream.Write(buffer, 0, buffer.Length);
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x000AF8A6 File Offset: 0x000AE8A6
		public virtual void Write(byte[] buffer, int index, int count)
		{
			this.OutStream.Write(buffer, index, count);
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x000AF8B8 File Offset: 0x000AE8B8
		public unsafe virtual void Write(char ch)
		{
			if (char.IsSurrogate(ch))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_SurrogatesNotAllowedAsSingleChar"));
			}
			int bytes;
			fixed (byte* buffer = this._buffer)
			{
				bytes = this._encoder.GetBytes(&ch, 1, buffer, 16, true);
			}
			this.OutStream.Write(this._buffer, 0, bytes);
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x000AF928 File Offset: 0x000AE928
		public virtual void Write(char[] chars)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			byte[] bytes = this._encoding.GetBytes(chars, 0, chars.Length);
			this.OutStream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06003510 RID: 13584 RVA: 0x000AF964 File Offset: 0x000AE964
		public virtual void Write(char[] chars, int index, int count)
		{
			byte[] bytes = this._encoding.GetBytes(chars, index, count);
			this.OutStream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06003511 RID: 13585 RVA: 0x000AF990 File Offset: 0x000AE990
		public unsafe virtual void Write(double value)
		{
			ulong num = (ulong)(*(long*)(&value));
			this._buffer[0] = (byte)num;
			this._buffer[1] = (byte)(num >> 8);
			this._buffer[2] = (byte)(num >> 16);
			this._buffer[3] = (byte)(num >> 24);
			this._buffer[4] = (byte)(num >> 32);
			this._buffer[5] = (byte)(num >> 40);
			this._buffer[6] = (byte)(num >> 48);
			this._buffer[7] = (byte)(num >> 56);
			this.OutStream.Write(this._buffer, 0, 8);
		}

		// Token: 0x06003512 RID: 13586 RVA: 0x000AFA19 File Offset: 0x000AEA19
		public virtual void Write(decimal value)
		{
			decimal.GetBytes(value, this._buffer);
			this.OutStream.Write(this._buffer, 0, 16);
		}

		// Token: 0x06003513 RID: 13587 RVA: 0x000AFA3B File Offset: 0x000AEA3B
		public virtual void Write(short value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this.OutStream.Write(this._buffer, 0, 2);
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x000AFA66 File Offset: 0x000AEA66
		[CLSCompliant(false)]
		public virtual void Write(ushort value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this.OutStream.Write(this._buffer, 0, 2);
		}

		// Token: 0x06003515 RID: 13589 RVA: 0x000AFA94 File Offset: 0x000AEA94
		public virtual void Write(int value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this._buffer[2] = (byte)(value >> 16);
			this._buffer[3] = (byte)(value >> 24);
			this.OutStream.Write(this._buffer, 0, 4);
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x000AFAE4 File Offset: 0x000AEAE4
		[CLSCompliant(false)]
		public virtual void Write(uint value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this._buffer[2] = (byte)(value >> 16);
			this._buffer[3] = (byte)(value >> 24);
			this.OutStream.Write(this._buffer, 0, 4);
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x000AFB34 File Offset: 0x000AEB34
		public virtual void Write(long value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this._buffer[2] = (byte)(value >> 16);
			this._buffer[3] = (byte)(value >> 24);
			this._buffer[4] = (byte)(value >> 32);
			this._buffer[5] = (byte)(value >> 40);
			this._buffer[6] = (byte)(value >> 48);
			this._buffer[7] = (byte)(value >> 56);
			this.OutStream.Write(this._buffer, 0, 8);
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x000AFBB8 File Offset: 0x000AEBB8
		[CLSCompliant(false)]
		public virtual void Write(ulong value)
		{
			this._buffer[0] = (byte)value;
			this._buffer[1] = (byte)(value >> 8);
			this._buffer[2] = (byte)(value >> 16);
			this._buffer[3] = (byte)(value >> 24);
			this._buffer[4] = (byte)(value >> 32);
			this._buffer[5] = (byte)(value >> 40);
			this._buffer[6] = (byte)(value >> 48);
			this._buffer[7] = (byte)(value >> 56);
			this.OutStream.Write(this._buffer, 0, 8);
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x000AFC3C File Offset: 0x000AEC3C
		public unsafe virtual void Write(float value)
		{
			uint num = *(uint*)(&value);
			this._buffer[0] = (byte)num;
			this._buffer[1] = (byte)(num >> 8);
			this._buffer[2] = (byte)(num >> 16);
			this._buffer[3] = (byte)(num >> 24);
			this.OutStream.Write(this._buffer, 0, 4);
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x000AFC94 File Offset: 0x000AEC94
		public unsafe virtual void Write(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int byteCount = this._encoding.GetByteCount(value);
			this.Write7BitEncodedInt(byteCount);
			if (this._largeByteBuffer == null)
			{
				this._largeByteBuffer = new byte[256];
				this._maxChars = 256 / this._encoding.GetMaxByteCount(1);
			}
			if (byteCount <= 256)
			{
				this._encoding.GetBytes(value, 0, value.Length, this._largeByteBuffer, 0);
				this.OutStream.Write(this._largeByteBuffer, 0, byteCount);
				return;
			}
			int num = 0;
			int num2;
			for (int i = value.Length; i > 0; i -= num2)
			{
				num2 = ((i > this._maxChars) ? this._maxChars : i);
				int bytes;
				fixed (char* ptr = value)
				{
					fixed (byte* largeByteBuffer = this._largeByteBuffer)
					{
						bytes = this._encoder.GetBytes(ptr + num, num2, largeByteBuffer, 256, num2 == i);
					}
				}
				this.OutStream.Write(this._largeByteBuffer, 0, bytes);
				num += num2;
			}
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x000AFDC8 File Offset: 0x000AEDC8
		protected void Write7BitEncodedInt(int value)
		{
			uint num;
			for (num = (uint)value; num >= 128U; num >>= 7)
			{
				this.Write((byte)(num | 128U));
			}
			this.Write((byte)num);
		}

		// Token: 0x04001C01 RID: 7169
		private const int LargeByteBufferSize = 256;

		// Token: 0x04001C02 RID: 7170
		public static readonly BinaryWriter Null = new BinaryWriter();

		// Token: 0x04001C03 RID: 7171
		protected Stream OutStream;

		// Token: 0x04001C04 RID: 7172
		private byte[] _buffer;

		// Token: 0x04001C05 RID: 7173
		private Encoding _encoding;

		// Token: 0x04001C06 RID: 7174
		private Encoder _encoder;

		// Token: 0x04001C07 RID: 7175
		private char[] _tmpOneCharBuffer = new char[1];

		// Token: 0x04001C08 RID: 7176
		private byte[] _largeByteBuffer;

		// Token: 0x04001C09 RID: 7177
		private int _maxChars;
	}
}

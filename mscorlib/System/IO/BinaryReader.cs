using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO
{
	// Token: 0x020005A6 RID: 1446
	[ComVisible(true)]
	public class BinaryReader : IDisposable
	{
		// Token: 0x060034E2 RID: 13538 RVA: 0x000AECAC File Offset: 0x000ADCAC
		public BinaryReader(Stream input) : this(input, new UTF8Encoding())
		{
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x000AECBC File Offset: 0x000ADCBC
		public BinaryReader(Stream input, Encoding encoding)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (!input.CanRead)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_StreamNotReadable"));
			}
			this.m_stream = input;
			this.m_decoder = encoding.GetDecoder();
			this.m_maxCharsSize = encoding.GetMaxCharCount(128);
			int num = encoding.GetMaxByteCount(1);
			if (num < 16)
			{
				num = 16;
			}
			this.m_buffer = new byte[num];
			this.m_charBuffer = null;
			this.m_charBytes = null;
			this.m_2BytesPerChar = (encoding is UnicodeEncoding);
			this.m_isMemoryStream = (this.m_stream.GetType() == typeof(MemoryStream));
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x060034E4 RID: 13540 RVA: 0x000AED7D File Offset: 0x000ADD7D
		public virtual Stream BaseStream
		{
			get
			{
				return this.m_stream;
			}
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x000AED85 File Offset: 0x000ADD85
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x000AED90 File Offset: 0x000ADD90
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Stream stream = this.m_stream;
				this.m_stream = null;
				if (stream != null)
				{
					stream.Close();
				}
			}
			this.m_stream = null;
			this.m_buffer = null;
			this.m_decoder = null;
			this.m_charBytes = null;
			this.m_singleChar = null;
			this.m_charBuffer = null;
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x000AEDE1 File Offset: 0x000ADDE1
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x000AEDEC File Offset: 0x000ADDEC
		public virtual int PeekChar()
		{
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			if (!this.m_stream.CanSeek)
			{
				return -1;
			}
			long position = this.m_stream.Position;
			int result = this.Read();
			this.m_stream.Position = position;
			return result;
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x000AEE35 File Offset: 0x000ADE35
		public virtual int Read()
		{
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			return this.InternalReadOneChar();
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x000AEE4A File Offset: 0x000ADE4A
		public virtual bool ReadBoolean()
		{
			this.FillBuffer(1);
			return this.m_buffer[0] != 0;
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x000AEE64 File Offset: 0x000ADE64
		public virtual byte ReadByte()
		{
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			int num = this.m_stream.ReadByte();
			if (num == -1)
			{
				__Error.EndOfFile();
			}
			return (byte)num;
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x000AEE95 File Offset: 0x000ADE95
		[CLSCompliant(false)]
		public virtual sbyte ReadSByte()
		{
			this.FillBuffer(1);
			return (sbyte)this.m_buffer[0];
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x000AEEA8 File Offset: 0x000ADEA8
		public virtual char ReadChar()
		{
			int num = this.Read();
			if (num == -1)
			{
				__Error.EndOfFile();
			}
			return (char)num;
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x000AEEC7 File Offset: 0x000ADEC7
		public virtual short ReadInt16()
		{
			this.FillBuffer(2);
			return (short)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8);
		}

		// Token: 0x060034EF RID: 13551 RVA: 0x000AEEE4 File Offset: 0x000ADEE4
		[CLSCompliant(false)]
		public virtual ushort ReadUInt16()
		{
			this.FillBuffer(2);
			return (ushort)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8);
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x000AEF04 File Offset: 0x000ADF04
		public virtual int ReadInt32()
		{
			if (this.m_isMemoryStream)
			{
				MemoryStream memoryStream = this.m_stream as MemoryStream;
				return memoryStream.InternalReadInt32();
			}
			this.FillBuffer(4);
			return (int)this.m_buffer[0] | (int)this.m_buffer[1] << 8 | (int)this.m_buffer[2] << 16 | (int)this.m_buffer[3] << 24;
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x000AEF5E File Offset: 0x000ADF5E
		[CLSCompliant(false)]
		public virtual uint ReadUInt32()
		{
			this.FillBuffer(4);
			return (uint)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8 | (int)this.m_buffer[2] << 16 | (int)this.m_buffer[3] << 24);
		}

		// Token: 0x060034F2 RID: 13554 RVA: 0x000AEF94 File Offset: 0x000ADF94
		public virtual long ReadInt64()
		{
			this.FillBuffer(8);
			uint num = (uint)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8 | (int)this.m_buffer[2] << 16 | (int)this.m_buffer[3] << 24);
			uint num2 = (uint)((int)this.m_buffer[4] | (int)this.m_buffer[5] << 8 | (int)this.m_buffer[6] << 16 | (int)this.m_buffer[7] << 24);
			return (long)((ulong)num2 << 32 | (ulong)num);
		}

		// Token: 0x060034F3 RID: 13555 RVA: 0x000AF008 File Offset: 0x000AE008
		[CLSCompliant(false)]
		public virtual ulong ReadUInt64()
		{
			this.FillBuffer(8);
			uint num = (uint)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8 | (int)this.m_buffer[2] << 16 | (int)this.m_buffer[3] << 24);
			uint num2 = (uint)((int)this.m_buffer[4] | (int)this.m_buffer[5] << 8 | (int)this.m_buffer[6] << 16 | (int)this.m_buffer[7] << 24);
			return (ulong)num2 << 32 | (ulong)num;
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x000AF07C File Offset: 0x000AE07C
		public unsafe virtual float ReadSingle()
		{
			this.FillBuffer(4);
			uint num = (uint)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8 | (int)this.m_buffer[2] << 16 | (int)this.m_buffer[3] << 24);
			return *(float*)(&num);
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x000AF0C0 File Offset: 0x000AE0C0
		public unsafe virtual double ReadDouble()
		{
			this.FillBuffer(8);
			uint num = (uint)((int)this.m_buffer[0] | (int)this.m_buffer[1] << 8 | (int)this.m_buffer[2] << 16 | (int)this.m_buffer[3] << 24);
			uint num2 = (uint)((int)this.m_buffer[4] | (int)this.m_buffer[5] << 8 | (int)this.m_buffer[6] << 16 | (int)this.m_buffer[7] << 24);
			ulong num3 = (ulong)num2 << 32 | (ulong)num;
			return *(double*)(&num3);
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x000AF139 File Offset: 0x000AE139
		public virtual decimal ReadDecimal()
		{
			this.FillBuffer(16);
			return decimal.ToDecimal(this.m_buffer);
		}

		// Token: 0x060034F7 RID: 13559 RVA: 0x000AF150 File Offset: 0x000AE150
		public virtual string ReadString()
		{
			int num = 0;
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			int num2 = this.Read7BitEncodedInt();
			if (num2 < 0)
			{
				throw new IOException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.IO_InvalidStringLen_Len"), new object[]
				{
					num2
				}));
			}
			if (num2 == 0)
			{
				return string.Empty;
			}
			if (this.m_charBytes == null)
			{
				this.m_charBytes = new byte[128];
			}
			if (this.m_charBuffer == null)
			{
				this.m_charBuffer = new char[this.m_maxCharsSize];
			}
			StringBuilder stringBuilder = null;
			int chars;
			for (;;)
			{
				int count = (num2 - num > 128) ? 128 : (num2 - num);
				int num3 = this.m_stream.Read(this.m_charBytes, 0, count);
				if (num3 == 0)
				{
					__Error.EndOfFile();
				}
				chars = this.m_decoder.GetChars(this.m_charBytes, 0, num3, this.m_charBuffer, 0);
				if (num == 0 && num3 == num2)
				{
					break;
				}
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(Math.Min(num2, 360));
				}
				stringBuilder.Append(this.m_charBuffer, 0, chars);
				num += num3;
				if (num >= num2)
				{
					goto Block_11;
				}
			}
			return new string(this.m_charBuffer, 0, chars);
			Block_11:
			return stringBuilder.ToString();
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x000AF280 File Offset: 0x000AE280
		public virtual int Read(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			return this.InternalReadChars(buffer, index, count);
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x000AF308 File Offset: 0x000AE308
		private int InternalReadChars(char[] buffer, int index, int count)
		{
			int i = count;
			if (this.m_charBytes == null)
			{
				this.m_charBytes = new byte[128];
			}
			while (i > 0)
			{
				int num = i;
				if (this.m_2BytesPerChar)
				{
					num <<= 1;
				}
				if (num > 128)
				{
					num = 128;
				}
				int chars;
				if (this.m_isMemoryStream)
				{
					MemoryStream memoryStream = this.m_stream as MemoryStream;
					int byteIndex = memoryStream.InternalGetPosition();
					num = memoryStream.InternalEmulateRead(num);
					if (num == 0)
					{
						return count - i;
					}
					chars = this.m_decoder.GetChars(memoryStream.InternalGetBuffer(), byteIndex, num, buffer, index);
				}
				else
				{
					num = this.m_stream.Read(this.m_charBytes, 0, num);
					if (num == 0)
					{
						return count - i;
					}
					chars = this.m_decoder.GetChars(this.m_charBytes, 0, num, buffer, index);
				}
				i -= chars;
				index += chars;
			}
			return count;
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x000AF3E0 File Offset: 0x000AE3E0
		private int InternalReadOneChar()
		{
			int num = 0;
			long num2 = num2 = 0L;
			if (this.m_stream.CanSeek)
			{
				num2 = this.m_stream.Position;
			}
			if (this.m_charBytes == null)
			{
				this.m_charBytes = new byte[128];
			}
			if (this.m_singleChar == null)
			{
				this.m_singleChar = new char[1];
			}
			while (num == 0)
			{
				int num3 = this.m_2BytesPerChar ? 2 : 1;
				int num4 = this.m_stream.ReadByte();
				this.m_charBytes[0] = (byte)num4;
				if (num4 == -1)
				{
					num3 = 0;
				}
				if (num3 == 2)
				{
					num4 = this.m_stream.ReadByte();
					this.m_charBytes[1] = (byte)num4;
					if (num4 == -1)
					{
						num3 = 1;
					}
				}
				if (num3 == 0)
				{
					return -1;
				}
				try
				{
					num = this.m_decoder.GetChars(this.m_charBytes, 0, num3, this.m_singleChar, 0);
				}
				catch
				{
					if (this.m_stream.CanSeek)
					{
						this.m_stream.Seek(num2 - this.m_stream.Position, SeekOrigin.Current);
					}
					throw;
				}
			}
			if (num == 0)
			{
				return -1;
			}
			return (int)this.m_singleChar[0];
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x000AF4FC File Offset: 0x000AE4FC
		public virtual char[] ReadChars(int count)
		{
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			char[] array = new char[count];
			int num = this.InternalReadChars(array, 0, count);
			if (num != count)
			{
				char[] array2 = new char[num];
				Buffer.InternalBlockCopy(array, 0, array2, 0, 2 * num);
				array = array2;
			}
			return array;
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x000AF55C File Offset: 0x000AE55C
		public virtual int Read(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			return this.m_stream.Read(buffer, index, count);
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x000AF5E8 File Offset: 0x000AE5E8
		public virtual byte[] ReadBytes(int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			byte[] array = new byte[count];
			int num = 0;
			do
			{
				int num2 = this.m_stream.Read(array, num, count);
				if (num2 == 0)
				{
					break;
				}
				num += num2;
				count -= num2;
			}
			while (count > 0);
			if (num != array.Length)
			{
				byte[] array2 = new byte[num];
				Buffer.InternalBlockCopy(array, 0, array2, 0, num);
				array = array2;
			}
			return array;
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x000AF660 File Offset: 0x000AE660
		protected virtual void FillBuffer(int numBytes)
		{
			int num = 0;
			if (this.m_stream == null)
			{
				__Error.FileNotOpen();
			}
			if (numBytes == 1)
			{
				int num2 = this.m_stream.ReadByte();
				if (num2 == -1)
				{
					__Error.EndOfFile();
				}
				this.m_buffer[0] = (byte)num2;
				return;
			}
			do
			{
				int num2 = this.m_stream.Read(this.m_buffer, num, numBytes - num);
				if (num2 == 0)
				{
					__Error.EndOfFile();
				}
				num += num2;
			}
			while (num < numBytes);
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x000AF6C8 File Offset: 0x000AE6C8
		protected internal int Read7BitEncodedInt()
		{
			int num = 0;
			int num2 = 0;
			while (num2 != 35)
			{
				byte b = this.ReadByte();
				num |= (int)(b & 127) << num2;
				num2 += 7;
				if ((b & 128) == 0)
				{
					return num;
				}
			}
			throw new FormatException(Environment.GetResourceString("Format_Bad7BitInt32"));
		}

		// Token: 0x04001BF6 RID: 7158
		private const int MaxCharBytesSize = 128;

		// Token: 0x04001BF7 RID: 7159
		private const int MaxStringBuilderCapacity = 360;

		// Token: 0x04001BF8 RID: 7160
		private Stream m_stream;

		// Token: 0x04001BF9 RID: 7161
		private byte[] m_buffer;

		// Token: 0x04001BFA RID: 7162
		private Decoder m_decoder;

		// Token: 0x04001BFB RID: 7163
		private byte[] m_charBytes;

		// Token: 0x04001BFC RID: 7164
		private char[] m_singleChar;

		// Token: 0x04001BFD RID: 7165
		private char[] m_charBuffer;

		// Token: 0x04001BFE RID: 7166
		private int m_maxCharsSize;

		// Token: 0x04001BFF RID: 7167
		private bool m_2BytesPerChar;

		// Token: 0x04001C00 RID: 7168
		private bool m_isMemoryStream;
	}
}

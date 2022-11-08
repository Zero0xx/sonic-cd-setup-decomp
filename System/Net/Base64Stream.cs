using System;
using System.IO;

namespace System.Net
{
	// Token: 0x0200067A RID: 1658
	internal class Base64Stream : DelegatedStream
	{
		// Token: 0x0600334F RID: 13135 RVA: 0x000D897A File Offset: 0x000D797A
		internal Base64Stream(Stream stream) : this(stream, Base64Stream.DefaultLineLength)
		{
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x000D8988 File Offset: 0x000D7988
		internal Base64Stream(Stream stream, int lineLength) : base(stream)
		{
			this.lineLength = lineLength;
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x000D8998 File Offset: 0x000D7998
		internal Base64Stream()
		{
			this.lineLength = Base64Stream.DefaultLineLength;
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x000D89AB File Offset: 0x000D79AB
		internal Base64Stream(int lineLength)
		{
			this.lineLength = lineLength;
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x06003353 RID: 13139 RVA: 0x000D89BA File Offset: 0x000D79BA
		public override bool CanWrite
		{
			get
			{
				return base.CanWrite;
			}
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x06003354 RID: 13140 RVA: 0x000D89C2 File Offset: 0x000D79C2
		private Base64Stream.ReadStateInfo ReadState
		{
			get
			{
				if (this.readState == null)
				{
					this.readState = new Base64Stream.ReadStateInfo();
				}
				return this.readState;
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06003355 RID: 13141 RVA: 0x000D89DD File Offset: 0x000D79DD
		internal Base64Stream.WriteStateInfo WriteState
		{
			get
			{
				if (this.writeState == null)
				{
					this.writeState = new Base64Stream.WriteStateInfo(1024);
				}
				return this.writeState;
			}
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x000D8A00 File Offset: 0x000D7A00
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			Base64Stream.ReadAsyncResult readAsyncResult = new Base64Stream.ReadAsyncResult(this, buffer, offset, count, callback, state);
			readAsyncResult.Read();
			return readAsyncResult;
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x000D8A58 File Offset: 0x000D7A58
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			Base64Stream.WriteAsyncResult writeAsyncResult = new Base64Stream.WriteAsyncResult(this, buffer, offset, count, callback, state);
			writeAsyncResult.Write();
			return writeAsyncResult;
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x000D8AB0 File Offset: 0x000D7AB0
		public override void Close()
		{
			if (this.writeState != null && this.WriteState.Length > 0)
			{
				switch (this.WriteState.Padding)
				{
				case 1:
					this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(int)this.WriteState.LastBits];
					this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[64];
					break;
				case 2:
					this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(int)this.WriteState.LastBits];
					this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[64];
					this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[64];
					break;
				}
				this.WriteState.Padding = 0;
				this.FlushInternal();
			}
			base.Close();
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x000D8BFC File Offset: 0x000D7BFC
		internal unsafe int DecodeBytes(byte[] buffer, int offset, int count)
		{
			fixed (byte* ptr = buffer)
			{
				byte* ptr2 = ptr + offset;
				byte* ptr3 = ptr2;
				byte* ptr4 = ptr2;
				byte* ptr5 = ptr2 + count;
				while (ptr3 < ptr5)
				{
					if (*ptr3 == 13 || *ptr3 == 10 || *ptr3 == 61)
					{
						ptr3++;
					}
					else
					{
						byte b = Base64Stream.base64DecodeMap[(int)(*ptr3)];
						if (b == 255)
						{
							throw new FormatException(SR.GetString("MailBase64InvalidCharacter"));
						}
						switch (this.ReadState.Pos)
						{
						case 0:
						{
							this.ReadState.Val = (byte)(b << 2);
							Base64Stream.ReadStateInfo readStateInfo = this.ReadState;
							readStateInfo.Pos += 1;
							break;
						}
						case 1:
						{
							*(ptr4++) = (byte)((int)this.ReadState.Val + (b >> 4));
							this.ReadState.Val = (byte)(b << 4);
							Base64Stream.ReadStateInfo readStateInfo2 = this.ReadState;
							readStateInfo2.Pos += 1;
							break;
						}
						case 2:
						{
							*(ptr4++) = (byte)((int)this.ReadState.Val + (b >> 2));
							this.ReadState.Val = (byte)(b << 6);
							Base64Stream.ReadStateInfo readStateInfo3 = this.ReadState;
							readStateInfo3.Pos += 1;
							break;
						}
						case 3:
							*(ptr4++) = this.ReadState.Val + b;
							this.ReadState.Pos = 0;
							break;
						}
						ptr3++;
					}
				}
				count = (int)((long)(ptr4 - ptr2));
			}
			return count;
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x000D8D80 File Offset: 0x000D7D80
		internal int EncodeBytes(byte[] buffer, int offset, int count, bool dontDeferFinalBytes)
		{
			int i = offset;
			switch (this.WriteState.Padding)
			{
			case 1:
				this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(int)this.WriteState.LastBits | (buffer[i] & 192) >> 6];
				this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(int)(buffer[i] & 63)];
				i++;
				count--;
				this.WriteState.Padding = 0;
				this.WriteState.CurrentLineLength++;
				break;
			case 2:
				this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(int)this.WriteState.LastBits | (buffer[i] & 240) >> 4];
				if (count == 1)
				{
					this.WriteState.LastBits = (byte)((buffer[i] & 15) << 2);
					this.WriteState.Padding = 1;
					return i - offset;
				}
				this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(int)(buffer[i] & 15) << 2 | (buffer[i + 1] & 192) >> 6];
				this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(int)(buffer[i + 1] & 63)];
				i += 2;
				count -= 2;
				this.WriteState.Padding = 0;
				this.WriteState.CurrentLineLength += 2;
				break;
			}
			int num = i + (count - count % 3);
			while (i < num)
			{
				if (this.lineLength != -1 && this.WriteState.CurrentLineLength + 4 > this.lineLength - 2)
				{
					this.WriteState.Buffer[this.WriteState.Length++] = 13;
					this.WriteState.Buffer[this.WriteState.Length++] = 10;
					this.WriteState.CurrentLineLength = 0;
				}
				if (this.WriteState.Length + 4 > this.WriteState.Buffer.Length)
				{
					return i - offset;
				}
				this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(buffer[i] & 252) >> 2];
				this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(int)(buffer[i] & 3) << 4 | (buffer[i + 1] & 240) >> 4];
				this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(int)(buffer[i + 1] & 15) << 2 | (buffer[i + 2] & 192) >> 6];
				this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(int)(buffer[i + 2] & 63)];
				this.WriteState.CurrentLineLength += 4;
				i += 3;
			}
			i = num;
			if (this.WriteState.Length + 4 > this.WriteState.Buffer.Length)
			{
				return i - offset;
			}
			if (this.lineLength != -1 && this.WriteState.CurrentLineLength + 4 > this.lineLength)
			{
				this.WriteState.Buffer[this.WriteState.Length++] = 13;
				this.WriteState.Buffer[this.WriteState.Length++] = 10;
				this.WriteState.CurrentLineLength = 0;
			}
			switch (count % 3)
			{
			case 1:
				this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(buffer[i] & 252) >> 2];
				if (dontDeferFinalBytes)
				{
					this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(int)((byte)((buffer[i] & 3) << 4))];
					this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[64];
					this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[64];
					this.WriteState.Padding = 0;
					this.WriteState.CurrentLineLength += 4;
				}
				else
				{
					this.WriteState.LastBits = (byte)((buffer[i] & 3) << 4);
					this.WriteState.Padding = 2;
					this.WriteState.CurrentLineLength++;
				}
				i++;
				break;
			case 2:
				this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(buffer[i] & 252) >> 2];
				this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(int)(buffer[i] & 3) << 4 | (buffer[i + 1] & 240) >> 4];
				if (dontDeferFinalBytes)
				{
					this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[(int)(buffer[i + 1] & 15) << 2];
					this.WriteState.Buffer[this.WriteState.Length++] = Base64Stream.base64EncodeMap[64];
					this.WriteState.Padding = 0;
					this.WriteState.CurrentLineLength += 4;
				}
				else
				{
					this.WriteState.LastBits = (byte)((buffer[i + 1] & 15) << 2);
					this.WriteState.Padding = 1;
					this.WriteState.CurrentLineLength += 2;
				}
				i += 2;
				break;
			}
			return i - offset;
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x000D9408 File Offset: 0x000D8408
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			return Base64Stream.ReadAsyncResult.End(asyncResult);
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x000D942B File Offset: 0x000D842B
		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			Base64Stream.WriteAsyncResult.End(asyncResult);
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x000D9441 File Offset: 0x000D8441
		public override void Flush()
		{
			if (this.writeState != null && this.WriteState.Length > 0)
			{
				this.FlushInternal();
			}
			base.Flush();
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x000D9465 File Offset: 0x000D8465
		private void FlushInternal()
		{
			base.Write(this.WriteState.Buffer, 0, this.WriteState.Length);
			this.WriteState.Length = 0;
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x000D9490 File Offset: 0x000D8490
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			for (;;)
			{
				int num = base.Read(buffer, offset, count);
				if (num == 0)
				{
					break;
				}
				num = this.DecodeBytes(buffer, offset, num);
				if (num > 0)
				{
					return num;
				}
			}
			return 0;
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x000D94F4 File Offset: 0x000D84F4
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			int num = 0;
			for (;;)
			{
				num += this.EncodeBytes(buffer, offset + num, count - num, false);
				if (num >= count)
				{
					break;
				}
				this.FlushInternal();
			}
		}

		// Token: 0x04002F80 RID: 12160
		private static int DefaultLineLength = 76;

		// Token: 0x04002F81 RID: 12161
		private static byte[] base64DecodeMap = new byte[]
		{
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			62,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			63,
			52,
			53,
			54,
			55,
			56,
			57,
			58,
			59,
			60,
			61,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			12,
			13,
			14,
			15,
			16,
			17,
			18,
			19,
			20,
			21,
			22,
			23,
			24,
			25,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			26,
			27,
			28,
			29,
			30,
			31,
			32,
			33,
			34,
			35,
			36,
			37,
			38,
			39,
			40,
			41,
			42,
			43,
			44,
			45,
			46,
			47,
			48,
			49,
			50,
			51,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue
		};

		// Token: 0x04002F82 RID: 12162
		private static byte[] base64EncodeMap = new byte[]
		{
			65,
			66,
			67,
			68,
			69,
			70,
			71,
			72,
			73,
			74,
			75,
			76,
			77,
			78,
			79,
			80,
			81,
			82,
			83,
			84,
			85,
			86,
			87,
			88,
			89,
			90,
			97,
			98,
			99,
			100,
			101,
			102,
			103,
			104,
			105,
			106,
			107,
			108,
			109,
			110,
			111,
			112,
			113,
			114,
			115,
			116,
			117,
			118,
			119,
			120,
			121,
			122,
			48,
			49,
			50,
			51,
			52,
			53,
			54,
			55,
			56,
			57,
			43,
			47,
			61
		};

		// Token: 0x04002F83 RID: 12163
		private int lineLength;

		// Token: 0x04002F84 RID: 12164
		private Base64Stream.ReadStateInfo readState;

		// Token: 0x04002F85 RID: 12165
		private Base64Stream.WriteStateInfo writeState;

		// Token: 0x0200067B RID: 1659
		private class ReadAsyncResult : LazyAsyncResult
		{
			// Token: 0x06003362 RID: 13154 RVA: 0x000D96D3 File Offset: 0x000D86D3
			internal ReadAsyncResult(Base64Stream parent, byte[] buffer, int offset, int count, AsyncCallback callback, object state) : base(null, state, callback)
			{
				this.parent = parent;
				this.buffer = buffer;
				this.offset = offset;
				this.count = count;
			}

			// Token: 0x06003363 RID: 13155 RVA: 0x000D9700 File Offset: 0x000D8700
			private bool CompleteRead(IAsyncResult result)
			{
				this.read = this.parent.BaseStream.EndRead(result);
				if (this.read == 0)
				{
					base.InvokeCallback();
					return true;
				}
				this.read = this.parent.DecodeBytes(this.buffer, this.offset, this.read);
				if (this.read > 0)
				{
					base.InvokeCallback();
					return true;
				}
				return false;
			}

			// Token: 0x06003364 RID: 13156 RVA: 0x000D976C File Offset: 0x000D876C
			internal void Read()
			{
				IAsyncResult asyncResult;
				do
				{
					asyncResult = this.parent.BaseStream.BeginRead(this.buffer, this.offset, this.count, Base64Stream.ReadAsyncResult.onRead, this);
				}
				while (asyncResult.CompletedSynchronously && !this.CompleteRead(asyncResult));
			}

			// Token: 0x06003365 RID: 13157 RVA: 0x000D97B4 File Offset: 0x000D87B4
			private static void OnRead(IAsyncResult result)
			{
				if (!result.CompletedSynchronously)
				{
					Base64Stream.ReadAsyncResult readAsyncResult = (Base64Stream.ReadAsyncResult)result.AsyncState;
					try
					{
						if (!readAsyncResult.CompleteRead(result))
						{
							readAsyncResult.Read();
						}
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

			// Token: 0x06003366 RID: 13158 RVA: 0x000D9838 File Offset: 0x000D8838
			internal static int End(IAsyncResult result)
			{
				Base64Stream.ReadAsyncResult readAsyncResult = (Base64Stream.ReadAsyncResult)result;
				readAsyncResult.InternalWaitForCompletion();
				return readAsyncResult.read;
			}

			// Token: 0x04002F86 RID: 12166
			private Base64Stream parent;

			// Token: 0x04002F87 RID: 12167
			private byte[] buffer;

			// Token: 0x04002F88 RID: 12168
			private int offset;

			// Token: 0x04002F89 RID: 12169
			private int count;

			// Token: 0x04002F8A RID: 12170
			private int read;

			// Token: 0x04002F8B RID: 12171
			private static AsyncCallback onRead = new AsyncCallback(Base64Stream.ReadAsyncResult.OnRead);
		}

		// Token: 0x0200067C RID: 1660
		private class WriteAsyncResult : LazyAsyncResult
		{
			// Token: 0x06003368 RID: 13160 RVA: 0x000D986C File Offset: 0x000D886C
			internal WriteAsyncResult(Base64Stream parent, byte[] buffer, int offset, int count, AsyncCallback callback, object state) : base(null, state, callback)
			{
				this.parent = parent;
				this.buffer = buffer;
				this.offset = offset;
				this.count = count;
			}

			// Token: 0x06003369 RID: 13161 RVA: 0x000D9898 File Offset: 0x000D8898
			internal void Write()
			{
				for (;;)
				{
					this.written += this.parent.EncodeBytes(this.buffer, this.offset + this.written, this.count - this.written, false);
					if (this.written >= this.count)
					{
						goto IL_94;
					}
					IAsyncResult asyncResult = this.parent.BaseStream.BeginWrite(this.parent.WriteState.Buffer, 0, this.parent.WriteState.Length, Base64Stream.WriteAsyncResult.onWrite, this);
					if (!asyncResult.CompletedSynchronously)
					{
						break;
					}
					this.CompleteWrite(asyncResult);
				}
				return;
				IL_94:
				base.InvokeCallback();
			}

			// Token: 0x0600336A RID: 13162 RVA: 0x000D993F File Offset: 0x000D893F
			private void CompleteWrite(IAsyncResult result)
			{
				this.parent.BaseStream.EndWrite(result);
				this.parent.WriteState.Length = 0;
			}

			// Token: 0x0600336B RID: 13163 RVA: 0x000D9964 File Offset: 0x000D8964
			private static void OnWrite(IAsyncResult result)
			{
				if (!result.CompletedSynchronously)
				{
					Base64Stream.WriteAsyncResult writeAsyncResult = (Base64Stream.WriteAsyncResult)result.AsyncState;
					try
					{
						writeAsyncResult.CompleteWrite(result);
						writeAsyncResult.Write();
					}
					catch (Exception result2)
					{
						if (writeAsyncResult.IsCompleted)
						{
							throw;
						}
						writeAsyncResult.InvokeCallback(result2);
					}
					catch
					{
						if (writeAsyncResult.IsCompleted)
						{
							throw;
						}
						writeAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
					}
				}
			}

			// Token: 0x0600336C RID: 13164 RVA: 0x000D99E8 File Offset: 0x000D89E8
			internal static void End(IAsyncResult result)
			{
				Base64Stream.WriteAsyncResult writeAsyncResult = (Base64Stream.WriteAsyncResult)result;
				writeAsyncResult.InternalWaitForCompletion();
			}

			// Token: 0x04002F8C RID: 12172
			private Base64Stream parent;

			// Token: 0x04002F8D RID: 12173
			private byte[] buffer;

			// Token: 0x04002F8E RID: 12174
			private int offset;

			// Token: 0x04002F8F RID: 12175
			private int count;

			// Token: 0x04002F90 RID: 12176
			private static AsyncCallback onWrite = new AsyncCallback(Base64Stream.WriteAsyncResult.OnWrite);

			// Token: 0x04002F91 RID: 12177
			private int written;
		}

		// Token: 0x0200067D RID: 1661
		private class ReadStateInfo
		{
			// Token: 0x17000C13 RID: 3091
			// (get) Token: 0x0600336E RID: 13166 RVA: 0x000D9A16 File Offset: 0x000D8A16
			// (set) Token: 0x0600336F RID: 13167 RVA: 0x000D9A1E File Offset: 0x000D8A1E
			internal byte Val
			{
				get
				{
					return this.val;
				}
				set
				{
					this.val = value;
				}
			}

			// Token: 0x17000C14 RID: 3092
			// (get) Token: 0x06003370 RID: 13168 RVA: 0x000D9A27 File Offset: 0x000D8A27
			// (set) Token: 0x06003371 RID: 13169 RVA: 0x000D9A2F File Offset: 0x000D8A2F
			internal byte Pos
			{
				get
				{
					return this.pos;
				}
				set
				{
					this.pos = value;
				}
			}

			// Token: 0x04002F92 RID: 12178
			private byte val;

			// Token: 0x04002F93 RID: 12179
			private byte pos;
		}

		// Token: 0x0200067E RID: 1662
		internal class WriteStateInfo
		{
			// Token: 0x06003373 RID: 13171 RVA: 0x000D9A40 File Offset: 0x000D8A40
			internal WriteStateInfo(int bufferSize)
			{
				this.outBuffer = new byte[bufferSize];
			}

			// Token: 0x17000C15 RID: 3093
			// (get) Token: 0x06003374 RID: 13172 RVA: 0x000D9A54 File Offset: 0x000D8A54
			internal byte[] Buffer
			{
				get
				{
					return this.outBuffer;
				}
			}

			// Token: 0x17000C16 RID: 3094
			// (get) Token: 0x06003375 RID: 13173 RVA: 0x000D9A5C File Offset: 0x000D8A5C
			// (set) Token: 0x06003376 RID: 13174 RVA: 0x000D9A64 File Offset: 0x000D8A64
			internal int CurrentLineLength
			{
				get
				{
					return this.currentLineLength;
				}
				set
				{
					this.currentLineLength = value;
				}
			}

			// Token: 0x17000C17 RID: 3095
			// (get) Token: 0x06003377 RID: 13175 RVA: 0x000D9A6D File Offset: 0x000D8A6D
			// (set) Token: 0x06003378 RID: 13176 RVA: 0x000D9A75 File Offset: 0x000D8A75
			internal int Length
			{
				get
				{
					return this.outLength;
				}
				set
				{
					this.outLength = value;
				}
			}

			// Token: 0x17000C18 RID: 3096
			// (get) Token: 0x06003379 RID: 13177 RVA: 0x000D9A7E File Offset: 0x000D8A7E
			// (set) Token: 0x0600337A RID: 13178 RVA: 0x000D9A86 File Offset: 0x000D8A86
			internal int Padding
			{
				get
				{
					return this.padding;
				}
				set
				{
					this.padding = value;
				}
			}

			// Token: 0x17000C19 RID: 3097
			// (get) Token: 0x0600337B RID: 13179 RVA: 0x000D9A8F File Offset: 0x000D8A8F
			// (set) Token: 0x0600337C RID: 13180 RVA: 0x000D9A97 File Offset: 0x000D8A97
			internal byte LastBits
			{
				get
				{
					return this.lastBits;
				}
				set
				{
					this.lastBits = value;
				}
			}

			// Token: 0x04002F94 RID: 12180
			private byte[] outBuffer;

			// Token: 0x04002F95 RID: 12181
			private int outLength;

			// Token: 0x04002F96 RID: 12182
			private int padding;

			// Token: 0x04002F97 RID: 12183
			private byte lastBits;

			// Token: 0x04002F98 RID: 12184
			private int currentLineLength;
		}
	}
}

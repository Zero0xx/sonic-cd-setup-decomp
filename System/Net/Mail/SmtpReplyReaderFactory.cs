using System;
using System.Collections;
using System.IO;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x020006D4 RID: 1748
	internal class SmtpReplyReaderFactory
	{
		// Token: 0x060035F4 RID: 13812 RVA: 0x000E6222 File Offset: 0x000E5222
		internal SmtpReplyReaderFactory(Stream stream)
		{
			this.bufferedStream = new BufferedReadStream(stream);
		}

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x060035F5 RID: 13813 RVA: 0x000E6236 File Offset: 0x000E5236
		internal SmtpReplyReader CurrentReader
		{
			get
			{
				return this.currentReader;
			}
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x060035F6 RID: 13814 RVA: 0x000E623E File Offset: 0x000E523E
		internal SmtpStatusCode StatusCode
		{
			get
			{
				return this.statusCode;
			}
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x000E6248 File Offset: 0x000E5248
		internal IAsyncResult BeginReadLines(SmtpReplyReader caller, AsyncCallback callback, object state)
		{
			SmtpReplyReaderFactory.ReadLinesAsyncResult readLinesAsyncResult = new SmtpReplyReaderFactory.ReadLinesAsyncResult(this, callback, state);
			readLinesAsyncResult.Read(caller);
			return readLinesAsyncResult;
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x000E6268 File Offset: 0x000E5268
		internal IAsyncResult BeginReadLine(SmtpReplyReader caller, AsyncCallback callback, object state)
		{
			SmtpReplyReaderFactory.ReadLinesAsyncResult readLinesAsyncResult = new SmtpReplyReaderFactory.ReadLinesAsyncResult(this, callback, state, true);
			readLinesAsyncResult.Read(caller);
			return readLinesAsyncResult;
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x000E6288 File Offset: 0x000E5288
		internal void Close(SmtpReplyReader caller)
		{
			if (this.currentReader == caller)
			{
				if (this.readState != SmtpReplyReaderFactory.ReadState.Done)
				{
					if (this.byteBuffer == null)
					{
						this.byteBuffer = new byte[256];
					}
					while (this.Read(caller, this.byteBuffer, 0, this.byteBuffer.Length) != 0)
					{
					}
				}
				this.currentReader = null;
			}
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x000E62DE File Offset: 0x000E52DE
		internal LineInfo[] EndReadLines(IAsyncResult result)
		{
			return SmtpReplyReaderFactory.ReadLinesAsyncResult.End(result);
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x000E62E8 File Offset: 0x000E52E8
		internal LineInfo EndReadLine(IAsyncResult result)
		{
			LineInfo[] array = SmtpReplyReaderFactory.ReadLinesAsyncResult.End(result);
			if (array != null && array.Length > 0)
			{
				return array[0];
			}
			return default(LineInfo);
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x000E631B File Offset: 0x000E531B
		internal SmtpReplyReader GetNextReplyReader()
		{
			if (this.currentReader != null)
			{
				this.currentReader.Close();
			}
			this.readState = SmtpReplyReaderFactory.ReadState.Status0;
			this.currentReader = new SmtpReplyReader(this);
			return this.currentReader;
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x000E634C File Offset: 0x000E534C
		private unsafe int ProcessRead(byte[] buffer, int offset, int read, bool readLine)
		{
			if (read == 0)
			{
				throw new IOException(SR.GetString("net_io_readfailure", new object[]
				{
					"net_io_connectionclosed"
				}));
			}
			fixed (byte* ptr = buffer)
			{
				byte* ptr2 = ptr + offset;
				byte* ptr3 = ptr2;
				byte* ptr4 = ptr3 + read;
				switch (this.readState)
				{
				case SmtpReplyReaderFactory.ReadState.Status0:
					goto IL_83;
				case SmtpReplyReaderFactory.ReadState.Status1:
					goto IL_C8;
				case SmtpReplyReaderFactory.ReadState.Status2:
					goto IL_114;
				case SmtpReplyReaderFactory.ReadState.ContinueFlag:
					goto IL_15D;
				case SmtpReplyReaderFactory.ReadState.ContinueCR:
					break;
				case SmtpReplyReaderFactory.ReadState.ContinueLF:
					goto IL_1B0;
				case SmtpReplyReaderFactory.ReadState.LastCR:
					goto IL_1FC;
				case SmtpReplyReaderFactory.ReadState.LastLF:
					goto IL_209;
				case SmtpReplyReaderFactory.ReadState.Done:
					goto IL_231;
				default:
					goto IL_247;
				}
				IL_1A0:
				while (ptr3 < ptr4)
				{
					if (*(ptr3++) == 13)
					{
						goto IL_1B0;
					}
				}
				this.readState = SmtpReplyReaderFactory.ReadState.ContinueCR;
				goto IL_247;
				IL_1FC:
				while (ptr3 < ptr4)
				{
					if (*(ptr3++) == 13)
					{
						goto IL_209;
					}
				}
				this.readState = SmtpReplyReaderFactory.ReadState.LastCR;
				goto IL_247;
				IL_83:
				if (ptr3 >= ptr4)
				{
					this.readState = SmtpReplyReaderFactory.ReadState.Status0;
					goto IL_247;
				}
				byte b = *(ptr3++);
				if (b < 48 && b > 57)
				{
					throw new FormatException(SR.GetString("SmtpInvalidResponse"));
				}
				this.statusCode = (SmtpStatusCode)(100 * (b - 48));
				IL_C8:
				if (ptr3 >= ptr4)
				{
					this.readState = SmtpReplyReaderFactory.ReadState.Status1;
					goto IL_247;
				}
				byte b2 = *(ptr3++);
				if (b2 < 48 && b2 > 57)
				{
					throw new FormatException(SR.GetString("SmtpInvalidResponse"));
				}
				this.statusCode += (int)(10 * (b2 - 48));
				IL_114:
				if (ptr3 >= ptr4)
				{
					this.readState = SmtpReplyReaderFactory.ReadState.Status2;
					goto IL_247;
				}
				byte b3 = *(ptr3++);
				if (b3 < 48 && b3 > 57)
				{
					throw new FormatException(SR.GetString("SmtpInvalidResponse"));
				}
				this.statusCode += (int)(b3 - 48);
				IL_15D:
				if (ptr3 >= ptr4)
				{
					this.readState = SmtpReplyReaderFactory.ReadState.ContinueFlag;
					goto IL_247;
				}
				byte b4 = *(ptr3++);
				if (b4 == 32)
				{
					goto IL_1FC;
				}
				if (b4 != 45)
				{
					throw new FormatException(SR.GetString("SmtpInvalidResponse"));
				}
				goto IL_1A0;
				IL_1B0:
				if (ptr3 >= ptr4)
				{
					this.readState = SmtpReplyReaderFactory.ReadState.ContinueLF;
					goto IL_247;
				}
				if (*(ptr3++) != 10)
				{
					throw new FormatException(SR.GetString("SmtpInvalidResponse"));
				}
				if (readLine)
				{
					this.readState = SmtpReplyReaderFactory.ReadState.Status0;
					return (int)((long)(ptr3 - ptr2));
				}
				goto IL_83;
				IL_209:
				if (ptr3 >= ptr4)
				{
					this.readState = SmtpReplyReaderFactory.ReadState.LastLF;
					goto IL_247;
				}
				if (*(ptr3++) != 10)
				{
					throw new FormatException(SR.GetString("SmtpInvalidResponse"));
				}
				IL_231:
				int result = (int)((long)(ptr3 - ptr2));
				this.readState = SmtpReplyReaderFactory.ReadState.Done;
				return result;
				IL_247:
				return (int)((long)(ptr3 - ptr2));
			}
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x000E65B0 File Offset: 0x000E55B0
		internal int Read(SmtpReplyReader caller, byte[] buffer, int offset, int count)
		{
			if (count == 0 || this.currentReader != caller || this.readState == SmtpReplyReaderFactory.ReadState.Done)
			{
				return 0;
			}
			int num = this.bufferedStream.Read(buffer, offset, count);
			int num2 = this.ProcessRead(buffer, offset, num, false);
			if (num2 < num)
			{
				this.bufferedStream.Push(buffer, offset + num2, num - num2);
			}
			return num2;
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x000E6608 File Offset: 0x000E5608
		internal LineInfo ReadLine(SmtpReplyReader caller)
		{
			LineInfo[] array = this.ReadLines(caller, true);
			if (array != null && array.Length > 0)
			{
				return array[0];
			}
			return default(LineInfo);
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x000E663D File Offset: 0x000E563D
		internal LineInfo[] ReadLines(SmtpReplyReader caller)
		{
			return this.ReadLines(caller, false);
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x000E6648 File Offset: 0x000E5648
		internal LineInfo[] ReadLines(SmtpReplyReader caller, bool oneLine)
		{
			if (caller != this.currentReader || this.readState == SmtpReplyReaderFactory.ReadState.Done)
			{
				return new LineInfo[0];
			}
			if (this.byteBuffer == null)
			{
				this.byteBuffer = new byte[256];
			}
			if (this.charBuffer == null)
			{
				this.charBuffer = new char[256];
			}
			StringBuilder stringBuilder = new StringBuilder();
			ArrayList arrayList = new ArrayList();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (;;)
			{
				if (num2 == num3)
				{
					num3 = this.bufferedStream.Read(this.byteBuffer, 0, this.byteBuffer.Length);
					num2 = 0;
				}
				int num4 = this.ProcessRead(this.byteBuffer, num2, num3 - num2, true);
				if (num < 4)
				{
					int num5 = Math.Min(4 - num, num4);
					num += num5;
					num2 += num5;
					num4 -= num5;
					if (num4 == 0)
					{
						continue;
					}
				}
				for (int i = num2; i < num2 + num4; i++)
				{
					this.charBuffer[i] = (char)this.byteBuffer[i];
				}
				stringBuilder.Append(this.charBuffer, num2, num4);
				num2 += num4;
				if (this.readState == SmtpReplyReaderFactory.ReadState.Status0)
				{
					num = 0;
					arrayList.Add(new LineInfo(this.statusCode, stringBuilder.ToString(0, stringBuilder.Length - 2)));
					if (oneLine)
					{
						break;
					}
					stringBuilder = new StringBuilder();
				}
				else if (this.readState == SmtpReplyReaderFactory.ReadState.Done)
				{
					goto Block_9;
				}
			}
			this.bufferedStream.Push(this.byteBuffer, num2, num3 - num2);
			return (LineInfo[])arrayList.ToArray(typeof(LineInfo));
			Block_9:
			arrayList.Add(new LineInfo(this.statusCode, stringBuilder.ToString(0, stringBuilder.Length - 2)));
			this.bufferedStream.Push(this.byteBuffer, num2, num3 - num2);
			return (LineInfo[])arrayList.ToArray(typeof(LineInfo));
		}

		// Token: 0x04003112 RID: 12562
		private const int DefaultBufferSize = 256;

		// Token: 0x04003113 RID: 12563
		private BufferedReadStream bufferedStream;

		// Token: 0x04003114 RID: 12564
		private byte[] byteBuffer;

		// Token: 0x04003115 RID: 12565
		private char[] charBuffer;

		// Token: 0x04003116 RID: 12566
		private SmtpReplyReader currentReader;

		// Token: 0x04003117 RID: 12567
		private SmtpReplyReaderFactory.ReadState readState;

		// Token: 0x04003118 RID: 12568
		private SmtpStatusCode statusCode;

		// Token: 0x020006D5 RID: 1749
		private enum ReadState
		{
			// Token: 0x0400311A RID: 12570
			Status0,
			// Token: 0x0400311B RID: 12571
			Status1,
			// Token: 0x0400311C RID: 12572
			Status2,
			// Token: 0x0400311D RID: 12573
			ContinueFlag,
			// Token: 0x0400311E RID: 12574
			ContinueCR,
			// Token: 0x0400311F RID: 12575
			ContinueLF,
			// Token: 0x04003120 RID: 12576
			LastCR,
			// Token: 0x04003121 RID: 12577
			LastLF,
			// Token: 0x04003122 RID: 12578
			Done
		}

		// Token: 0x020006D6 RID: 1750
		private class ReadLinesAsyncResult : LazyAsyncResult
		{
			// Token: 0x06003602 RID: 13826 RVA: 0x000E680E File Offset: 0x000E580E
			internal ReadLinesAsyncResult(SmtpReplyReaderFactory parent, AsyncCallback callback, object state) : base(null, state, callback)
			{
				this.parent = parent;
			}

			// Token: 0x06003603 RID: 13827 RVA: 0x000E6820 File Offset: 0x000E5820
			internal ReadLinesAsyncResult(SmtpReplyReaderFactory parent, AsyncCallback callback, object state, bool oneLine) : base(null, state, callback)
			{
				this.oneLine = oneLine;
				this.parent = parent;
			}

			// Token: 0x06003604 RID: 13828 RVA: 0x000E683C File Offset: 0x000E583C
			internal void Read(SmtpReplyReader caller)
			{
				if (this.parent.currentReader != caller || this.parent.readState == SmtpReplyReaderFactory.ReadState.Done)
				{
					base.InvokeCallback();
					return;
				}
				if (this.parent.byteBuffer == null)
				{
					this.parent.byteBuffer = new byte[256];
				}
				if (this.parent.charBuffer == null)
				{
					this.parent.charBuffer = new char[256];
				}
				this.builder = new StringBuilder();
				this.lines = new ArrayList();
				this.Read();
			}

			// Token: 0x06003605 RID: 13829 RVA: 0x000E68CC File Offset: 0x000E58CC
			internal static LineInfo[] End(IAsyncResult result)
			{
				SmtpReplyReaderFactory.ReadLinesAsyncResult readLinesAsyncResult = (SmtpReplyReaderFactory.ReadLinesAsyncResult)result;
				readLinesAsyncResult.InternalWaitForCompletion();
				return (LineInfo[])readLinesAsyncResult.lines.ToArray(typeof(LineInfo));
			}

			// Token: 0x06003606 RID: 13830 RVA: 0x000E6904 File Offset: 0x000E5904
			private void Read()
			{
				for (;;)
				{
					IAsyncResult asyncResult = this.parent.bufferedStream.BeginRead(this.parent.byteBuffer, 0, this.parent.byteBuffer.Length, SmtpReplyReaderFactory.ReadLinesAsyncResult.readCallback, this);
					if (!asyncResult.CompletedSynchronously)
					{
						break;
					}
					this.read = this.parent.bufferedStream.EndRead(asyncResult);
					if (!this.ProcessRead())
					{
						return;
					}
				}
			}

			// Token: 0x06003607 RID: 13831 RVA: 0x000E696C File Offset: 0x000E596C
			private static void ReadCallback(IAsyncResult result)
			{
				if (!result.CompletedSynchronously)
				{
					Exception ex = null;
					SmtpReplyReaderFactory.ReadLinesAsyncResult readLinesAsyncResult = (SmtpReplyReaderFactory.ReadLinesAsyncResult)result.AsyncState;
					try
					{
						readLinesAsyncResult.read = readLinesAsyncResult.parent.bufferedStream.EndRead(result);
						if (readLinesAsyncResult.ProcessRead())
						{
							readLinesAsyncResult.Read();
						}
					}
					catch (Exception ex2)
					{
						ex = ex2;
					}
					catch
					{
						ex = new Exception(SR.GetString("net_nonClsCompliantException"));
					}
					if (ex != null)
					{
						readLinesAsyncResult.InvokeCallback(ex);
					}
				}
			}

			// Token: 0x06003608 RID: 13832 RVA: 0x000E69F4 File Offset: 0x000E59F4
			private bool ProcessRead()
			{
				if (this.read == 0)
				{
					throw new IOException(SR.GetString("net_io_readfailure", new object[]
					{
						"net_io_connectionclosed"
					}));
				}
				int num = 0;
				while (num != this.read)
				{
					int num2 = this.parent.ProcessRead(this.parent.byteBuffer, num, this.read - num, true);
					if (this.statusRead < 4)
					{
						int num3 = Math.Min(4 - this.statusRead, num2);
						this.statusRead += num3;
						num += num3;
						num2 -= num3;
						if (num2 == 0)
						{
							continue;
						}
					}
					for (int i = num; i < num + num2; i++)
					{
						this.parent.charBuffer[i] = (char)this.parent.byteBuffer[i];
					}
					this.builder.Append(this.parent.charBuffer, num, num2);
					num += num2;
					if (this.parent.readState == SmtpReplyReaderFactory.ReadState.Status0)
					{
						this.lines.Add(new LineInfo(this.parent.statusCode, this.builder.ToString(0, this.builder.Length - 2)));
						this.builder = new StringBuilder();
						this.statusRead = 0;
						if (this.oneLine)
						{
							this.parent.bufferedStream.Push(this.parent.byteBuffer, num, this.read - num);
							base.InvokeCallback();
							return false;
						}
					}
					else if (this.parent.readState == SmtpReplyReaderFactory.ReadState.Done)
					{
						this.lines.Add(new LineInfo(this.parent.statusCode, this.builder.ToString(0, this.builder.Length - 2)));
						this.parent.bufferedStream.Push(this.parent.byteBuffer, num, this.read - num);
						base.InvokeCallback();
						return false;
					}
				}
				return true;
			}

			// Token: 0x04003123 RID: 12579
			private StringBuilder builder;

			// Token: 0x04003124 RID: 12580
			private ArrayList lines;

			// Token: 0x04003125 RID: 12581
			private SmtpReplyReaderFactory parent;

			// Token: 0x04003126 RID: 12582
			private static AsyncCallback readCallback = new AsyncCallback(SmtpReplyReaderFactory.ReadLinesAsyncResult.ReadCallback);

			// Token: 0x04003127 RID: 12583
			private int read;

			// Token: 0x04003128 RID: 12584
			private int statusRead;

			// Token: 0x04003129 RID: 12585
			private bool oneLine;
		}
	}
}

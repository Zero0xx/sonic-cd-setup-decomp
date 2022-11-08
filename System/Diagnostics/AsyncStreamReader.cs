using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x0200073F RID: 1855
	internal class AsyncStreamReader : IDisposable
	{
		// Token: 0x06003884 RID: 14468 RVA: 0x000EEAF0 File Offset: 0x000EDAF0
		internal AsyncStreamReader(Process process, Stream stream, UserCallBack callback, Encoding encoding) : this(process, stream, callback, encoding, 1024)
		{
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x000EEB02 File Offset: 0x000EDB02
		internal AsyncStreamReader(Process process, Stream stream, UserCallBack callback, Encoding encoding, int bufferSize)
		{
			this.Init(process, stream, callback, encoding, bufferSize);
			this.messageQueue = new Queue();
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x000EEB24 File Offset: 0x000EDB24
		private void Init(Process process, Stream stream, UserCallBack callback, Encoding encoding, int bufferSize)
		{
			this.process = process;
			this.stream = stream;
			this.encoding = encoding;
			this.userCallBack = callback;
			this.decoder = encoding.GetDecoder();
			if (bufferSize < 128)
			{
				bufferSize = 128;
			}
			this.byteBuffer = new byte[bufferSize];
			this._maxCharsPerBuffer = encoding.GetMaxCharCount(bufferSize);
			this.charBuffer = new char[this._maxCharsPerBuffer];
			this.cancelOperation = false;
			this.eofEvent = new ManualResetEvent(false);
			this.sb = null;
			this.bLastCarriageReturn = false;
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x000EEBB9 File Offset: 0x000EDBB9
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x000EEBC2 File Offset: 0x000EDBC2
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x000EEBCC File Offset: 0x000EDBCC
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.stream != null)
			{
				this.stream.Close();
			}
			if (this.stream != null)
			{
				this.stream = null;
				this.encoding = null;
				this.decoder = null;
				this.byteBuffer = null;
				this.charBuffer = null;
			}
			if (this.eofEvent != null)
			{
				this.eofEvent.Close();
				this.eofEvent = null;
			}
		}

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x0600388A RID: 14474 RVA: 0x000EEC34 File Offset: 0x000EDC34
		public virtual Encoding CurrentEncoding
		{
			get
			{
				return this.encoding;
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x0600388B RID: 14475 RVA: 0x000EEC3C File Offset: 0x000EDC3C
		public virtual Stream BaseStream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x0600388C RID: 14476 RVA: 0x000EEC44 File Offset: 0x000EDC44
		internal void BeginReadLine()
		{
			if (this.cancelOperation)
			{
				this.cancelOperation = false;
			}
			if (this.sb == null)
			{
				this.sb = new StringBuilder(1024);
				this.stream.BeginRead(this.byteBuffer, 0, this.byteBuffer.Length, new AsyncCallback(this.ReadBuffer), null);
				return;
			}
			this.FlushMessageQueue();
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x000EECA7 File Offset: 0x000EDCA7
		internal void CancelOperation()
		{
			this.cancelOperation = true;
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x000EECB0 File Offset: 0x000EDCB0
		private void ReadBuffer(IAsyncResult ar)
		{
			int num;
			try
			{
				num = this.stream.EndRead(ar);
			}
			catch (IOException)
			{
				num = 0;
			}
			catch (OperationCanceledException)
			{
				num = 0;
			}
			if (num == 0)
			{
				lock (this.messageQueue)
				{
					if (this.sb.Length != 0)
					{
						this.messageQueue.Enqueue(this.sb.ToString());
						this.sb.Length = 0;
					}
					this.messageQueue.Enqueue(null);
				}
				try
				{
					this.FlushMessageQueue();
					return;
				}
				finally
				{
					this.eofEvent.Set();
				}
			}
			int chars = this.decoder.GetChars(this.byteBuffer, 0, num, this.charBuffer, 0);
			this.sb.Append(this.charBuffer, 0, chars);
			this.GetLinesFromStringBuilder();
			this.stream.BeginRead(this.byteBuffer, 0, this.byteBuffer.Length, new AsyncCallback(this.ReadBuffer), null);
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x000EEDD0 File Offset: 0x000EDDD0
		private void GetLinesFromStringBuilder()
		{
			int i = 0;
			int num = 0;
			int length = this.sb.Length;
			if (this.bLastCarriageReturn && length > 0 && this.sb[0] == '\n')
			{
				i = 1;
				num = 1;
				this.bLastCarriageReturn = false;
			}
			while (i < length)
			{
				char c = this.sb[i];
				if (c == '\r' || c == '\n')
				{
					string obj = this.sb.ToString(num, i - num);
					num = i + 1;
					if (c == '\r' && num < length && this.sb[num] == '\n')
					{
						num++;
						i++;
					}
					lock (this.messageQueue)
					{
						this.messageQueue.Enqueue(obj);
					}
				}
				i++;
			}
			if (this.sb[length - 1] == '\r')
			{
				this.bLastCarriageReturn = true;
			}
			if (num < length)
			{
				this.sb.Remove(0, num);
			}
			else
			{
				this.sb.Length = 0;
			}
			this.FlushMessageQueue();
		}

		// Token: 0x06003890 RID: 14480 RVA: 0x000EEEE8 File Offset: 0x000EDEE8
		private void FlushMessageQueue()
		{
			while (this.messageQueue.Count > 0)
			{
				lock (this.messageQueue)
				{
					if (this.messageQueue.Count > 0)
					{
						string data = (string)this.messageQueue.Dequeue();
						if (!this.cancelOperation)
						{
							this.userCallBack(data);
						}
					}
					continue;
				}
				break;
			}
		}

		// Token: 0x06003891 RID: 14481 RVA: 0x000EEF5C File Offset: 0x000EDF5C
		internal void WaitUtilEOF()
		{
			if (this.eofEvent != null)
			{
				this.eofEvent.WaitOne();
				this.eofEvent.Close();
				this.eofEvent = null;
			}
		}

		// Token: 0x04003245 RID: 12869
		internal const int DefaultBufferSize = 1024;

		// Token: 0x04003246 RID: 12870
		private const int MinBufferSize = 128;

		// Token: 0x04003247 RID: 12871
		private Stream stream;

		// Token: 0x04003248 RID: 12872
		private Encoding encoding;

		// Token: 0x04003249 RID: 12873
		private Decoder decoder;

		// Token: 0x0400324A RID: 12874
		private byte[] byteBuffer;

		// Token: 0x0400324B RID: 12875
		private char[] charBuffer;

		// Token: 0x0400324C RID: 12876
		private int _maxCharsPerBuffer;

		// Token: 0x0400324D RID: 12877
		private Process process;

		// Token: 0x0400324E RID: 12878
		private UserCallBack userCallBack;

		// Token: 0x0400324F RID: 12879
		private bool cancelOperation;

		// Token: 0x04003250 RID: 12880
		private ManualResetEvent eofEvent;

		// Token: 0x04003251 RID: 12881
		private Queue messageQueue;

		// Token: 0x04003252 RID: 12882
		private StringBuilder sb;

		// Token: 0x04003253 RID: 12883
		private bool bLastCarriageReturn;
	}
}

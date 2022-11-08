using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace System.IO
{
	// Token: 0x020005CD RID: 1485
	[ComVisible(true)]
	[Serializable]
	public class StreamWriter : TextWriter
	{
		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06003769 RID: 14185 RVA: 0x000BAC44 File Offset: 0x000B9C44
		internal static Encoding UTF8NoBOM
		{
			get
			{
				if (StreamWriter._UTF8NoBOM == null)
				{
					UTF8Encoding utf8NoBOM = new UTF8Encoding(false, true);
					Thread.MemoryBarrier();
					StreamWriter._UTF8NoBOM = utf8NoBOM;
				}
				return StreamWriter._UTF8NoBOM;
			}
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x000BAC70 File Offset: 0x000B9C70
		internal StreamWriter() : base(null)
		{
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x000BAC79 File Offset: 0x000B9C79
		public StreamWriter(Stream stream) : this(stream, StreamWriter.UTF8NoBOM, 1024)
		{
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x000BAC8C File Offset: 0x000B9C8C
		public StreamWriter(Stream stream, Encoding encoding) : this(stream, encoding, 1024)
		{
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x000BAC9C File Offset: 0x000B9C9C
		public StreamWriter(Stream stream, Encoding encoding, int bufferSize) : base(null)
		{
			if (stream == null || encoding == null)
			{
				throw new ArgumentNullException((stream == null) ? "stream" : "encoding");
			}
			if (!stream.CanWrite)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_StreamNotWritable"));
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			this.Init(stream, encoding, bufferSize);
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x000BAD05 File Offset: 0x000B9D05
		internal StreamWriter(Stream stream, Encoding encoding, int bufferSize, bool closeable) : this(stream, encoding, bufferSize)
		{
			this.closable = closeable;
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x000BAD18 File Offset: 0x000B9D18
		public StreamWriter(string path) : this(path, false, StreamWriter.UTF8NoBOM, 1024)
		{
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x000BAD2C File Offset: 0x000B9D2C
		public StreamWriter(string path, bool append) : this(path, append, StreamWriter.UTF8NoBOM, 1024)
		{
		}

		// Token: 0x06003771 RID: 14193 RVA: 0x000BAD40 File Offset: 0x000B9D40
		public StreamWriter(string path, bool append, Encoding encoding) : this(path, append, encoding, 1024)
		{
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x000BAD50 File Offset: 0x000B9D50
		public StreamWriter(string path, bool append, Encoding encoding, int bufferSize) : base(null)
		{
			if (path == null || encoding == null)
			{
				throw new ArgumentNullException((path == null) ? "path" : "encoding");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			Stream stream = StreamWriter.CreateFile(path, append);
			this.Init(stream, encoding, bufferSize);
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x000BADAC File Offset: 0x000B9DAC
		private void Init(Stream stream, Encoding encoding, int bufferSize)
		{
			this.stream = stream;
			this.encoding = encoding;
			this.encoder = encoding.GetEncoder();
			if (bufferSize < 128)
			{
				bufferSize = 128;
			}
			this.charBuffer = new char[bufferSize];
			this.byteBuffer = new byte[encoding.GetMaxByteCount(bufferSize)];
			this.charLen = bufferSize;
			if (stream.CanSeek && stream.Position > 0L)
			{
				this.haveWrittenPreamble = true;
			}
			this.closable = true;
			if (Mda.StreamWriterBufferMDAEnabled)
			{
				string stackTrace = Environment.GetStackTrace(null, false);
				this.mdaHelper = new MdaHelper(this, stackTrace);
			}
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x000BAE44 File Offset: 0x000B9E44
		private static Stream CreateFile(string path, bool append)
		{
			FileMode mode = append ? FileMode.Append : FileMode.Create;
			return new FileStream(path, mode, FileAccess.Write, FileShare.Read, 4096, FileOptions.SequentialScan);
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x000BAE6E File Offset: 0x000B9E6E
		public override void Close()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x000BAE80 File Offset: 0x000B9E80
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (this.stream != null && (disposing || (!this.Closable && this.stream is __ConsoleStream)))
				{
					this.Flush(true, true);
					if (this.mdaHelper != null)
					{
						GC.SuppressFinalize(this.mdaHelper);
					}
				}
			}
			finally
			{
				if (this.Closable && this.stream != null)
				{
					try
					{
						if (disposing)
						{
							this.stream.Close();
						}
					}
					finally
					{
						this.stream = null;
						this.byteBuffer = null;
						this.charBuffer = null;
						this.encoding = null;
						this.encoder = null;
						this.charLen = 0;
						base.Dispose(disposing);
					}
				}
			}
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x000BAF3C File Offset: 0x000B9F3C
		public override void Flush()
		{
			this.Flush(true, true);
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x000BAF48 File Offset: 0x000B9F48
		private void Flush(bool flushStream, bool flushEncoder)
		{
			if (this.stream == null)
			{
				__Error.WriterClosed();
			}
			if (this.charPos == 0 && !flushStream && !flushEncoder)
			{
				return;
			}
			if (!this.haveWrittenPreamble)
			{
				this.haveWrittenPreamble = true;
				byte[] preamble = this.encoding.GetPreamble();
				if (preamble.Length > 0)
				{
					this.stream.Write(preamble, 0, preamble.Length);
				}
			}
			int bytes = this.encoder.GetBytes(this.charBuffer, 0, this.charPos, this.byteBuffer, 0, flushEncoder);
			this.charPos = 0;
			if (bytes > 0)
			{
				this.stream.Write(this.byteBuffer, 0, bytes);
			}
			if (flushStream)
			{
				this.stream.Flush();
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06003779 RID: 14201 RVA: 0x000BAFEF File Offset: 0x000B9FEF
		// (set) Token: 0x0600377A RID: 14202 RVA: 0x000BAFF7 File Offset: 0x000B9FF7
		public virtual bool AutoFlush
		{
			get
			{
				return this.autoFlush;
			}
			set
			{
				this.autoFlush = value;
				if (value)
				{
					this.Flush(true, false);
				}
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x0600377B RID: 14203 RVA: 0x000BB00B File Offset: 0x000BA00B
		public virtual Stream BaseStream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x0600377C RID: 14204 RVA: 0x000BB013 File Offset: 0x000BA013
		internal bool Closable
		{
			get
			{
				return this.closable;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (set) Token: 0x0600377D RID: 14205 RVA: 0x000BB01B File Offset: 0x000BA01B
		internal bool HaveWrittenPreamble
		{
			set
			{
				this.haveWrittenPreamble = value;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x0600377E RID: 14206 RVA: 0x000BB024 File Offset: 0x000BA024
		public override Encoding Encoding
		{
			get
			{
				return this.encoding;
			}
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x000BB02C File Offset: 0x000BA02C
		public override void Write(char value)
		{
			if (this.charPos == this.charLen)
			{
				this.Flush(false, false);
			}
			this.charBuffer[this.charPos] = value;
			this.charPos++;
			if (this.autoFlush)
			{
				this.Flush(true, false);
			}
		}

		// Token: 0x06003780 RID: 14208 RVA: 0x000BB07C File Offset: 0x000BA07C
		public override void Write(char[] buffer)
		{
			if (buffer == null)
			{
				return;
			}
			int num = 0;
			int num2;
			for (int i = buffer.Length; i > 0; i -= num2)
			{
				if (this.charPos == this.charLen)
				{
					this.Flush(false, false);
				}
				num2 = this.charLen - this.charPos;
				if (num2 > i)
				{
					num2 = i;
				}
				Buffer.InternalBlockCopy(buffer, num * 2, this.charBuffer, this.charPos * 2, num2 * 2);
				this.charPos += num2;
				num += num2;
			}
			if (this.autoFlush)
			{
				this.Flush(true, false);
			}
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x000BB104 File Offset: 0x000BA104
		public override void Write(char[] buffer, int index, int count)
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
			while (count > 0)
			{
				if (this.charPos == this.charLen)
				{
					this.Flush(false, false);
				}
				int num = this.charLen - this.charPos;
				if (num > count)
				{
					num = count;
				}
				Buffer.InternalBlockCopy(buffer, index * 2, this.charBuffer, this.charPos * 2, num * 2);
				this.charPos += num;
				index += num;
				count -= num;
			}
			if (this.autoFlush)
			{
				this.Flush(true, false);
			}
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x000BB1E4 File Offset: 0x000BA1E4
		public override void Write(string value)
		{
			if (value != null)
			{
				int i = value.Length;
				int num = 0;
				while (i > 0)
				{
					if (this.charPos == this.charLen)
					{
						this.Flush(false, false);
					}
					int num2 = this.charLen - this.charPos;
					if (num2 > i)
					{
						num2 = i;
					}
					value.CopyTo(num, this.charBuffer, this.charPos, num2);
					this.charPos += num2;
					num += num2;
					i -= num2;
				}
				if (this.autoFlush)
				{
					this.Flush(true, false);
				}
			}
		}

		// Token: 0x04001CC6 RID: 7366
		private const int DefaultBufferSize = 1024;

		// Token: 0x04001CC7 RID: 7367
		private const int DefaultFileStreamBufferSize = 4096;

		// Token: 0x04001CC8 RID: 7368
		private const int MinBufferSize = 128;

		// Token: 0x04001CC9 RID: 7369
		public new static readonly StreamWriter Null = new StreamWriter(Stream.Null, new UTF8Encoding(false, true), 128, false);

		// Token: 0x04001CCA RID: 7370
		internal Stream stream;

		// Token: 0x04001CCB RID: 7371
		private Encoding encoding;

		// Token: 0x04001CCC RID: 7372
		private Encoder encoder;

		// Token: 0x04001CCD RID: 7373
		internal byte[] byteBuffer;

		// Token: 0x04001CCE RID: 7374
		internal char[] charBuffer;

		// Token: 0x04001CCF RID: 7375
		internal int charPos;

		// Token: 0x04001CD0 RID: 7376
		internal int charLen;

		// Token: 0x04001CD1 RID: 7377
		internal bool autoFlush;

		// Token: 0x04001CD2 RID: 7378
		private bool haveWrittenPreamble;

		// Token: 0x04001CD3 RID: 7379
		private bool closable;

		// Token: 0x04001CD4 RID: 7380
		[NonSerialized]
		private MdaHelper mdaHelper;

		// Token: 0x04001CD5 RID: 7381
		private static Encoding _UTF8NoBOM;
	}
}

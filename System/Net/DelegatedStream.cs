using System;
using System.IO;
using System.Net.Sockets;

namespace System.Net
{
	// Token: 0x02000679 RID: 1657
	internal class DelegatedStream : Stream
	{
		// Token: 0x0600333C RID: 13116 RVA: 0x000D86C5 File Offset: 0x000D76C5
		protected DelegatedStream()
		{
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x000D86CD File Offset: 0x000D76CD
		protected DelegatedStream(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.stream = stream;
			this.netStream = (stream as NetworkStream);
		}

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x0600333E RID: 13118 RVA: 0x000D86F6 File Offset: 0x000D76F6
		protected Stream BaseStream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x0600333F RID: 13119 RVA: 0x000D86FE File Offset: 0x000D76FE
		public override bool CanRead
		{
			get
			{
				return this.stream.CanRead;
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x06003340 RID: 13120 RVA: 0x000D870B File Offset: 0x000D770B
		public override bool CanSeek
		{
			get
			{
				return this.stream.CanSeek;
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x06003341 RID: 13121 RVA: 0x000D8718 File Offset: 0x000D7718
		public override bool CanWrite
		{
			get
			{
				return this.stream.CanWrite;
			}
		}

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x06003342 RID: 13122 RVA: 0x000D8725 File Offset: 0x000D7725
		public override long Length
		{
			get
			{
				if (!this.CanSeek)
				{
					throw new NotSupportedException(SR.GetString("SeekNotSupported"));
				}
				return this.stream.Length;
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x06003343 RID: 13123 RVA: 0x000D874A File Offset: 0x000D774A
		// (set) Token: 0x06003344 RID: 13124 RVA: 0x000D876F File Offset: 0x000D776F
		public override long Position
		{
			get
			{
				if (!this.CanSeek)
				{
					throw new NotSupportedException(SR.GetString("SeekNotSupported"));
				}
				return this.stream.Position;
			}
			set
			{
				if (!this.CanSeek)
				{
					throw new NotSupportedException(SR.GetString("SeekNotSupported"));
				}
				this.stream.Position = value;
			}
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x000D8798 File Offset: 0x000D7798
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (!this.CanRead)
			{
				throw new NotSupportedException(SR.GetString("ReadNotSupported"));
			}
			IAsyncResult result;
			if (this.netStream != null)
			{
				result = this.netStream.UnsafeBeginRead(buffer, offset, count, callback, state);
			}
			else
			{
				result = this.stream.BeginRead(buffer, offset, count, callback, state);
			}
			return result;
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x000D87F0 File Offset: 0x000D77F0
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (!this.CanWrite)
			{
				throw new NotSupportedException(SR.GetString("WriteNotSupported"));
			}
			IAsyncResult result;
			if (this.netStream != null)
			{
				result = this.netStream.UnsafeBeginWrite(buffer, offset, count, callback, state);
			}
			else
			{
				result = this.stream.BeginWrite(buffer, offset, count, callback, state);
			}
			return result;
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x000D8848 File Offset: 0x000D7848
		public override void Close()
		{
			this.stream.Close();
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x000D8858 File Offset: 0x000D7858
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (!this.CanRead)
			{
				throw new NotSupportedException(SR.GetString("ReadNotSupported"));
			}
			return this.stream.EndRead(asyncResult);
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x000D888B File Offset: 0x000D788B
		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (!this.CanWrite)
			{
				throw new NotSupportedException(SR.GetString("WriteNotSupported"));
			}
			this.stream.EndWrite(asyncResult);
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x000D88B1 File Offset: 0x000D78B1
		public override void Flush()
		{
			this.stream.Flush();
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x000D88C0 File Offset: 0x000D78C0
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (!this.CanRead)
			{
				throw new NotSupportedException(SR.GetString("ReadNotSupported"));
			}
			return this.stream.Read(buffer, offset, count);
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x000D88F8 File Offset: 0x000D78F8
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (!this.CanSeek)
			{
				throw new NotSupportedException(SR.GetString("SeekNotSupported"));
			}
			return this.stream.Seek(offset, origin);
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x000D892C File Offset: 0x000D792C
		public override void SetLength(long value)
		{
			if (!this.CanSeek)
			{
				throw new NotSupportedException(SR.GetString("SeekNotSupported"));
			}
			this.stream.SetLength(value);
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x000D8952 File Offset: 0x000D7952
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (!this.CanWrite)
			{
				throw new NotSupportedException(SR.GetString("WriteNotSupported"));
			}
			this.stream.Write(buffer, offset, count);
		}

		// Token: 0x04002F7E RID: 12158
		private Stream stream;

		// Token: 0x04002F7F RID: 12159
		private NetworkStream netStream;
	}
}

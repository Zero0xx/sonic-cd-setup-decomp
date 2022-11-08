using System;
using System.IO;

namespace System.Xml
{
	// Token: 0x02000031 RID: 49
	internal class XmlRegisteredNonCachedStream : Stream
	{
		// Token: 0x06000165 RID: 357 RVA: 0x000073EC File Offset: 0x000063EC
		internal XmlRegisteredNonCachedStream(Stream stream, XmlDownloadManager downloadManager, string host)
		{
			this.stream = stream;
			this.downloadManager = downloadManager;
			this.host = host;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000740C File Offset: 0x0000640C
		~XmlRegisteredNonCachedStream()
		{
			if (this.downloadManager != null)
			{
				this.downloadManager.Remove(this.host);
			}
			this.stream = null;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00007454 File Offset: 0x00006454
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this.stream != null)
				{
					if (this.downloadManager != null)
					{
						this.downloadManager.Remove(this.host);
					}
					this.stream.Close();
				}
				this.stream = null;
				GC.SuppressFinalize(this);
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x000074B8 File Offset: 0x000064B8
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.stream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x000074CC File Offset: 0x000064CC
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000074DB File Offset: 0x000064DB
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.stream.EndRead(asyncResult);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000074E9 File Offset: 0x000064E9
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.stream.EndWrite(asyncResult);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000074F7 File Offset: 0x000064F7
		public override void Flush()
		{
			this.stream.Flush();
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007504 File Offset: 0x00006504
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.stream.Read(buffer, offset, count);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00007514 File Offset: 0x00006514
		public override int ReadByte()
		{
			return this.stream.ReadByte();
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00007521 File Offset: 0x00006521
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.stream.Seek(offset, origin);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00007530 File Offset: 0x00006530
		public override void SetLength(long value)
		{
			this.stream.SetLength(value);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000753E File Offset: 0x0000653E
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.stream.Write(buffer, offset, count);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000754E File Offset: 0x0000654E
		public override void WriteByte(byte value)
		{
			this.stream.WriteByte(value);
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000173 RID: 371 RVA: 0x0000755C File Offset: 0x0000655C
		public override bool CanRead
		{
			get
			{
				return this.stream.CanRead;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00007569 File Offset: 0x00006569
		public override bool CanSeek
		{
			get
			{
				return this.stream.CanSeek;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00007576 File Offset: 0x00006576
		public override bool CanWrite
		{
			get
			{
				return this.stream.CanWrite;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00007583 File Offset: 0x00006583
		public override long Length
		{
			get
			{
				return this.stream.Length;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00007590 File Offset: 0x00006590
		// (set) Token: 0x06000178 RID: 376 RVA: 0x0000759D File Offset: 0x0000659D
		public override long Position
		{
			get
			{
				return this.stream.Position;
			}
			set
			{
				this.stream.Position = value;
			}
		}

		// Token: 0x040004B6 RID: 1206
		protected Stream stream;

		// Token: 0x040004B7 RID: 1207
		private XmlDownloadManager downloadManager;

		// Token: 0x040004B8 RID: 1208
		private string host;
	}
}

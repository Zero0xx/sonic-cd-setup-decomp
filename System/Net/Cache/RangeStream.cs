using System;
using System.IO;

namespace System.Net.Cache
{
	// Token: 0x0200057D RID: 1405
	internal class RangeStream : Stream, ICloseEx
	{
		// Token: 0x06002AF6 RID: 10998 RVA: 0x000B6D24 File Offset: 0x000B5D24
		internal RangeStream(Stream parentStream, long offset, long size)
		{
			this.m_ParentStream = parentStream;
			this.m_Offset = offset;
			this.m_Size = size;
			if (this.m_ParentStream.CanSeek)
			{
				this.m_ParentStream.Position = offset;
				this.m_Position = offset;
				return;
			}
			throw new NotSupportedException(SR.GetString("net_cache_non_seekable_stream_not_supported"));
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06002AF7 RID: 10999 RVA: 0x000B6D7C File Offset: 0x000B5D7C
		public override bool CanRead
		{
			get
			{
				return this.m_ParentStream.CanRead;
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06002AF8 RID: 11000 RVA: 0x000B6D89 File Offset: 0x000B5D89
		public override bool CanSeek
		{
			get
			{
				return this.m_ParentStream.CanSeek;
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06002AF9 RID: 11001 RVA: 0x000B6D96 File Offset: 0x000B5D96
		public override bool CanWrite
		{
			get
			{
				return this.m_ParentStream.CanWrite;
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06002AFA RID: 11002 RVA: 0x000B6DA3 File Offset: 0x000B5DA3
		public override long Length
		{
			get
			{
				long length = this.m_ParentStream.Length;
				return this.m_Size;
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06002AFB RID: 11003 RVA: 0x000B6DB7 File Offset: 0x000B5DB7
		// (set) Token: 0x06002AFC RID: 11004 RVA: 0x000B6DCB File Offset: 0x000B5DCB
		public override long Position
		{
			get
			{
				return this.m_ParentStream.Position - this.m_Offset;
			}
			set
			{
				value += this.m_Offset;
				if (value > this.m_Offset + this.m_Size)
				{
					value = this.m_Offset + this.m_Size;
				}
				this.m_ParentStream.Position = value;
			}
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x000B6E04 File Offset: 0x000B5E04
		public override long Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
			case SeekOrigin.Begin:
				offset += this.m_Offset;
				if (offset > this.m_Offset + this.m_Size)
				{
					offset = this.m_Offset + this.m_Size;
				}
				if (offset < this.m_Offset)
				{
					offset = this.m_Offset;
					goto IL_D0;
				}
				goto IL_D0;
			case SeekOrigin.End:
				offset -= this.m_Offset + this.m_Size;
				if (offset > 0L)
				{
					offset = 0L;
				}
				if (offset < -this.m_Size)
				{
					offset = -this.m_Size;
					goto IL_D0;
				}
				goto IL_D0;
			}
			if (this.m_Position + offset > this.m_Offset + this.m_Size)
			{
				offset = this.m_Offset + this.m_Size - this.m_Position;
			}
			if (this.m_Position + offset < this.m_Offset)
			{
				offset = this.m_Offset - this.m_Position;
			}
			IL_D0:
			this.m_Position = this.m_ParentStream.Seek(offset, origin);
			return this.m_Position - this.m_Offset;
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x000B6F01 File Offset: 0x000B5F01
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_cache_unsupported_partial_stream"));
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x000B6F14 File Offset: 0x000B5F14
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.m_Position + (long)count > this.m_Offset + this.m_Size)
			{
				throw new NotSupportedException(SR.GetString("net_cache_unsupported_partial_stream"));
			}
			this.m_ParentStream.Write(buffer, offset, count);
			this.m_Position += (long)count;
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x000B6F66 File Offset: 0x000B5F66
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (this.m_Position + (long)offset > this.m_Offset + this.m_Size)
			{
				throw new NotSupportedException(SR.GetString("net_cache_unsupported_partial_stream"));
			}
			return this.m_ParentStream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x000B6FA2 File Offset: 0x000B5FA2
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.m_ParentStream.EndWrite(asyncResult);
			this.m_Position = this.m_ParentStream.Position;
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x000B6FC1 File Offset: 0x000B5FC1
		public override void Flush()
		{
			this.m_ParentStream.Flush();
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x000B6FD0 File Offset: 0x000B5FD0
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.m_Position >= this.m_Offset + this.m_Size)
			{
				return 0;
			}
			if (this.m_Position + (long)count > this.m_Offset + this.m_Size)
			{
				count = (int)(this.m_Offset + this.m_Size - this.m_Position);
			}
			int num = this.m_ParentStream.Read(buffer, offset, count);
			this.m_Position += (long)num;
			return num;
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x000B7044 File Offset: 0x000B6044
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (this.m_Position >= this.m_Offset + this.m_Size)
			{
				count = 0;
			}
			else if (this.m_Position + (long)count > this.m_Offset + this.m_Size)
			{
				count = (int)(this.m_Offset + this.m_Size - this.m_Position);
			}
			return this.m_ParentStream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x000B70AC File Offset: 0x000B60AC
		public override int EndRead(IAsyncResult asyncResult)
		{
			int num = this.m_ParentStream.EndRead(asyncResult);
			this.m_Position += (long)num;
			return num;
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x000B70D6 File Offset: 0x000B60D6
		protected sealed override void Dispose(bool disposing)
		{
			this.Dispose(disposing, CloseExState.Normal);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x000B70E6 File Offset: 0x000B60E6
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			this.Dispose(true, closeState);
			GC.SuppressFinalize(this);
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06002B08 RID: 11016 RVA: 0x000B70F6 File Offset: 0x000B60F6
		public override bool CanTimeout
		{
			get
			{
				return this.m_ParentStream.CanTimeout;
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06002B09 RID: 11017 RVA: 0x000B7103 File Offset: 0x000B6103
		// (set) Token: 0x06002B0A RID: 11018 RVA: 0x000B7110 File Offset: 0x000B6110
		public override int ReadTimeout
		{
			get
			{
				return this.m_ParentStream.ReadTimeout;
			}
			set
			{
				this.m_ParentStream.ReadTimeout = value;
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06002B0B RID: 11019 RVA: 0x000B711E File Offset: 0x000B611E
		// (set) Token: 0x06002B0C RID: 11020 RVA: 0x000B712B File Offset: 0x000B612B
		public override int WriteTimeout
		{
			get
			{
				return this.m_ParentStream.WriteTimeout;
			}
			set
			{
				this.m_ParentStream.WriteTimeout = value;
			}
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x000B713C File Offset: 0x000B613C
		protected virtual void Dispose(bool disposing, CloseExState closeState)
		{
			ICloseEx closeEx = this.m_ParentStream as ICloseEx;
			if (closeEx != null)
			{
				closeEx.CloseEx(closeState);
			}
			else
			{
				this.m_ParentStream.Close();
			}
			base.Dispose(disposing);
		}

		// Token: 0x04002999 RID: 10649
		private Stream m_ParentStream;

		// Token: 0x0400299A RID: 10650
		private long m_Offset;

		// Token: 0x0400299B RID: 10651
		private long m_Size;

		// Token: 0x0400299C RID: 10652
		private long m_Position;
	}
}

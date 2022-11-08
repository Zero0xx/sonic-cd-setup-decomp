using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020003B5 RID: 949
	[Serializable]
	public class FileWebResponse : WebResponse, ISerializable, ICloseEx
	{
		// Token: 0x06001DDA RID: 7642 RVA: 0x000712F4 File Offset: 0x000702F4
		internal FileWebResponse(FileWebRequest request, Uri uri, FileAccess access, bool asyncHint)
		{
			try
			{
				this.m_fileAccess = access;
				if (access == FileAccess.Write)
				{
					this.m_stream = Stream.Null;
				}
				else
				{
					this.m_stream = new FileWebStream(request, uri.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, asyncHint);
					this.m_contentLength = this.m_stream.Length;
				}
				this.m_headers = new WebHeaderCollection(WebHeaderCollectionType.FileWebResponse);
				this.m_headers.AddInternal("Content-Length", this.m_contentLength.ToString(NumberFormatInfo.InvariantInfo));
				this.m_headers.AddInternal("Content-Type", "application/octet-stream");
				this.m_uri = uri;
			}
			catch (Exception ex)
			{
				Exception ex2 = new WebException(ex.Message, ex, WebExceptionStatus.ConnectFailure, null);
				throw ex2;
			}
			catch
			{
				Exception ex3 = new WebException(string.Empty, new Exception(SR.GetString("net_nonClsCompliantException")), WebExceptionStatus.ConnectFailure, null);
				throw ex3;
			}
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x000713E4 File Offset: 0x000703E4
		[Obsolete("Serialization is obsoleted for this type. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected FileWebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
			this.m_headers = (WebHeaderCollection)serializationInfo.GetValue("headers", typeof(WebHeaderCollection));
			this.m_uri = (Uri)serializationInfo.GetValue("uri", typeof(Uri));
			this.m_contentLength = serializationInfo.GetInt64("contentLength");
			this.m_fileAccess = (FileAccess)serializationInfo.GetInt32("fileAccess");
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x0007145B File Offset: 0x0007045B
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x00071468 File Offset: 0x00070468
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("headers", this.m_headers, typeof(WebHeaderCollection));
			serializationInfo.AddValue("uri", this.m_uri, typeof(Uri));
			serializationInfo.AddValue("contentLength", this.m_contentLength);
			serializationInfo.AddValue("fileAccess", this.m_fileAccess);
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001DDE RID: 7646 RVA: 0x000714DA File Offset: 0x000704DA
		public override long ContentLength
		{
			get
			{
				this.CheckDisposed();
				return this.m_contentLength;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001DDF RID: 7647 RVA: 0x000714E8 File Offset: 0x000704E8
		public override string ContentType
		{
			get
			{
				this.CheckDisposed();
				return "application/octet-stream";
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001DE0 RID: 7648 RVA: 0x000714F5 File Offset: 0x000704F5
		public override WebHeaderCollection Headers
		{
			get
			{
				this.CheckDisposed();
				return this.m_headers;
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001DE1 RID: 7649 RVA: 0x00071503 File Offset: 0x00070503
		public override Uri ResponseUri
		{
			get
			{
				this.CheckDisposed();
				return this.m_uri;
			}
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x00071511 File Offset: 0x00070511
		private void CheckDisposed()
		{
			if (this.m_closed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x0007152C File Offset: 0x0007052C
		public override void Close()
		{
			((ICloseEx)this).CloseEx(CloseExState.Normal);
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x00071538 File Offset: 0x00070538
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			if (!this.m_closed)
			{
				this.m_closed = true;
				Stream stream = this.m_stream;
				if (stream != null)
				{
					if (stream is ICloseEx)
					{
						((ICloseEx)stream).CloseEx(closeState);
					}
					else
					{
						stream.Close();
					}
					this.m_stream = null;
				}
			}
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x00071581 File Offset: 0x00070581
		public override Stream GetResponseStream()
		{
			this.CheckDisposed();
			return this.m_stream;
		}

		// Token: 0x04001DA3 RID: 7587
		private const int DefaultFileStreamBufferSize = 8192;

		// Token: 0x04001DA4 RID: 7588
		private const string DefaultFileContentType = "application/octet-stream";

		// Token: 0x04001DA5 RID: 7589
		private bool m_closed;

		// Token: 0x04001DA6 RID: 7590
		private long m_contentLength;

		// Token: 0x04001DA7 RID: 7591
		private FileAccess m_fileAccess;

		// Token: 0x04001DA8 RID: 7592
		private WebHeaderCollection m_headers;

		// Token: 0x04001DA9 RID: 7593
		private Stream m_stream;

		// Token: 0x04001DAA RID: 7594
		private Uri m_uri;
	}
}

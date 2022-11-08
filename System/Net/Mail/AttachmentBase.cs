using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000674 RID: 1652
	public abstract class AttachmentBase : IDisposable
	{
		// Token: 0x060032F7 RID: 13047 RVA: 0x000D7B43 File Offset: 0x000D6B43
		internal AttachmentBase()
		{
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x000D7B56 File Offset: 0x000D6B56
		protected AttachmentBase(string fileName)
		{
			this.SetContentFromFile(fileName, string.Empty);
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x000D7B75 File Offset: 0x000D6B75
		protected AttachmentBase(string fileName, string mediaType)
		{
			this.SetContentFromFile(fileName, mediaType);
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x000D7B90 File Offset: 0x000D6B90
		protected AttachmentBase(string fileName, ContentType contentType)
		{
			this.SetContentFromFile(fileName, contentType);
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x000D7BAB File Offset: 0x000D6BAB
		protected AttachmentBase(Stream contentStream)
		{
			this.part.SetContent(contentStream);
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x000D7BCA File Offset: 0x000D6BCA
		protected AttachmentBase(Stream contentStream, string mediaType)
		{
			this.part.SetContent(contentStream, null, mediaType);
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x000D7BEB File Offset: 0x000D6BEB
		internal AttachmentBase(Stream contentStream, string name, string mediaType)
		{
			this.part.SetContent(contentStream, name, mediaType);
		}

		// Token: 0x060032FE RID: 13054 RVA: 0x000D7C0C File Offset: 0x000D6C0C
		protected AttachmentBase(Stream contentStream, ContentType contentType)
		{
			this.part.SetContent(contentStream, contentType);
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x000D7C2C File Offset: 0x000D6C2C
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x000D7C35 File Offset: 0x000D6C35
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !this.disposed)
			{
				this.disposed = true;
				this.part.Dispose();
			}
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x000D7C54 File Offset: 0x000D6C54
		internal static string ShortNameFromFile(string fileName)
		{
			int num = fileName.LastIndexOfAny(new char[]
			{
				'\\',
				':'
			}, fileName.Length - 1, fileName.Length);
			string result;
			if (num > 0)
			{
				result = fileName.Substring(num + 1, fileName.Length - num - 1);
			}
			else
			{
				result = fileName;
			}
			return result;
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x000D7CA8 File Offset: 0x000D6CA8
		internal void SetContentFromFile(string fileName, ContentType contentType)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileName == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"fileName"
				}), "fileName");
			}
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			this.part.SetContent(stream, contentType);
		}

		// Token: 0x06003303 RID: 13059 RVA: 0x000D7D0C File Offset: 0x000D6D0C
		internal void SetContentFromFile(string fileName, string mediaType)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			if (fileName == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"fileName"
				}), "fileName");
			}
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			this.part.SetContent(stream, null, mediaType);
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x000D7D74 File Offset: 0x000D6D74
		internal void SetContentFromString(string contentString, ContentType contentType)
		{
			if (contentString == null)
			{
				throw new ArgumentNullException("content");
			}
			if (this.part.Stream != null)
			{
				this.part.Stream.Close();
			}
			Encoding encoding;
			if (contentType != null && contentType.CharSet != null)
			{
				encoding = Encoding.GetEncoding(contentType.CharSet);
			}
			else if (MimeBasePart.IsAscii(contentString, false))
			{
				encoding = Encoding.ASCII;
			}
			else
			{
				encoding = Encoding.GetEncoding("utf-8");
			}
			byte[] bytes = encoding.GetBytes(contentString);
			this.part.SetContent(new MemoryStream(bytes), contentType);
			if (MimeBasePart.ShouldUseBase64Encoding(encoding))
			{
				this.part.TransferEncoding = TransferEncoding.Base64;
				return;
			}
			this.part.TransferEncoding = TransferEncoding.QuotedPrintable;
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x000D7E1C File Offset: 0x000D6E1C
		internal void SetContentFromString(string contentString, Encoding encoding, string mediaType)
		{
			if (contentString == null)
			{
				throw new ArgumentNullException("content");
			}
			if (this.part.Stream != null)
			{
				this.part.Stream.Close();
			}
			if (mediaType == null || mediaType == string.Empty)
			{
				mediaType = "text/plain";
			}
			int num = 0;
			try
			{
				string text = MailBnfHelper.ReadToken(mediaType, ref num, null);
				if (text.Length == 0 || num >= mediaType.Length || mediaType[num++] != '/')
				{
					throw new ArgumentException(SR.GetString("MediaTypeInvalid"), "mediaType");
				}
				text = MailBnfHelper.ReadToken(mediaType, ref num, null);
				if (text.Length == 0 || num < mediaType.Length)
				{
					throw new ArgumentException(SR.GetString("MediaTypeInvalid"), "mediaType");
				}
			}
			catch (FormatException)
			{
				throw new ArgumentException(SR.GetString("MediaTypeInvalid"), "mediaType");
			}
			ContentType contentType = new ContentType(mediaType);
			if (encoding == null)
			{
				if (MimeBasePart.IsAscii(contentString, false))
				{
					encoding = Encoding.ASCII;
				}
				else
				{
					encoding = Encoding.GetEncoding("utf-8");
				}
			}
			contentType.CharSet = encoding.BodyName;
			byte[] bytes = encoding.GetBytes(contentString);
			this.part.SetContent(new MemoryStream(bytes), contentType);
			if (MimeBasePart.ShouldUseBase64Encoding(encoding))
			{
				this.part.TransferEncoding = TransferEncoding.Base64;
				return;
			}
			this.part.TransferEncoding = TransferEncoding.QuotedPrintable;
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x000D7F74 File Offset: 0x000D6F74
		internal virtual void PrepareForSending()
		{
			this.part.ResetStream();
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06003307 RID: 13063 RVA: 0x000D7F81 File Offset: 0x000D6F81
		public Stream ContentStream
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				return this.part.Stream;
			}
		}

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x06003308 RID: 13064 RVA: 0x000D7FA8 File Offset: 0x000D6FA8
		// (set) Token: 0x06003309 RID: 13065 RVA: 0x000D8020 File Offset: 0x000D7020
		public string ContentId
		{
			get
			{
				string text = this.part.ContentID;
				if (string.IsNullOrEmpty(text))
				{
					text = Guid.NewGuid().ToString();
					this.ContentId = text;
					return text;
				}
				if (text.Length >= 2 && text[0] == '<' && text[text.Length - 1] == '>')
				{
					return text.Substring(1, text.Length - 2);
				}
				return text;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.part.ContentID = null;
					return;
				}
				if (value.IndexOfAny(new char[]
				{
					'<',
					'>'
				}) != -1)
				{
					throw new ArgumentException(SR.GetString("MailHeaderInvalidCID"), "value");
				}
				this.part.ContentID = "<" + value + ">";
			}
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x0600330A RID: 13066 RVA: 0x000D808D File Offset: 0x000D708D
		// (set) Token: 0x0600330B RID: 13067 RVA: 0x000D809A File Offset: 0x000D709A
		public ContentType ContentType
		{
			get
			{
				return this.part.ContentType;
			}
			set
			{
				this.part.ContentType = value;
			}
		}

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x0600330C RID: 13068 RVA: 0x000D80A8 File Offset: 0x000D70A8
		// (set) Token: 0x0600330D RID: 13069 RVA: 0x000D80B5 File Offset: 0x000D70B5
		public TransferEncoding TransferEncoding
		{
			get
			{
				return this.part.TransferEncoding;
			}
			set
			{
				this.part.TransferEncoding = value;
			}
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x0600330E RID: 13070 RVA: 0x000D80C4 File Offset: 0x000D70C4
		// (set) Token: 0x0600330F RID: 13071 RVA: 0x000D80E9 File Offset: 0x000D70E9
		internal Uri ContentLocation
		{
			get
			{
				Uri result;
				if (!Uri.TryCreate(this.part.ContentLocation, UriKind.RelativeOrAbsolute, out result))
				{
					return null;
				}
				return result;
			}
			set
			{
				this.part.ContentLocation = ((value == null) ? null : (value.IsAbsoluteUri ? value.AbsoluteUri : value.OriginalString));
			}
		}

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06003310 RID: 13072 RVA: 0x000D8118 File Offset: 0x000D7118
		internal MimePart MimePart
		{
			get
			{
				return this.part;
			}
		}

		// Token: 0x04002F77 RID: 12151
		internal bool disposed;

		// Token: 0x04002F78 RID: 12152
		private MimePart part = new MimePart();
	}
}

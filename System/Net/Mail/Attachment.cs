using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000677 RID: 1655
	public class Attachment : AttachmentBase
	{
		// Token: 0x06003325 RID: 13093 RVA: 0x000D833D File Offset: 0x000D733D
		internal Attachment()
		{
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x000D8345 File Offset: 0x000D7345
		public Attachment(string fileName) : base(fileName)
		{
			this.Name = AttachmentBase.ShortNameFromFile(fileName);
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x000D835A File Offset: 0x000D735A
		public Attachment(string fileName, string mediaType) : base(fileName, mediaType)
		{
			this.Name = AttachmentBase.ShortNameFromFile(fileName);
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x000D8370 File Offset: 0x000D7370
		public Attachment(string fileName, ContentType contentType) : base(fileName, contentType)
		{
			if (contentType.Name == null || contentType.Name == string.Empty)
			{
				this.Name = AttachmentBase.ShortNameFromFile(fileName);
				return;
			}
			this.Name = contentType.Name;
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x000D83AD File Offset: 0x000D73AD
		public Attachment(Stream contentStream, string name) : base(contentStream, null, null)
		{
			this.Name = name;
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x000D83BF File Offset: 0x000D73BF
		public Attachment(Stream contentStream, string name, string mediaType) : base(contentStream, null, mediaType)
		{
			this.Name = name;
		}

		// Token: 0x0600332B RID: 13099 RVA: 0x000D83D1 File Offset: 0x000D73D1
		public Attachment(Stream contentStream, ContentType contentType) : base(contentStream, contentType)
		{
			this.Name = contentType.Name;
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x000D83E8 File Offset: 0x000D73E8
		internal void SetContentTypeName()
		{
			if (this.name != null && this.name.Length != 0 && !MimeBasePart.IsAscii(this.name, false))
			{
				Encoding encoding = this.NameEncoding;
				if (encoding == null)
				{
					encoding = Encoding.GetEncoding("utf-8");
				}
				base.MimePart.ContentType.Name = MimeBasePart.EncodeHeaderValue(this.name, encoding, MimeBasePart.ShouldUseBase64Encoding(encoding));
				return;
			}
			base.MimePart.ContentType.Name = this.name;
		}

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x0600332D RID: 13101 RVA: 0x000D8466 File Offset: 0x000D7466
		// (set) Token: 0x0600332E RID: 13102 RVA: 0x000D8470 File Offset: 0x000D7470
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				Encoding encoding = MimeBasePart.DecodeEncoding(value);
				if (encoding != null)
				{
					this.nameEncoding = encoding;
					this.name = MimeBasePart.DecodeHeaderValue(value);
					base.MimePart.ContentType.Name = value;
					return;
				}
				this.name = value;
				this.SetContentTypeName();
			}
		}

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x0600332F RID: 13103 RVA: 0x000D84B9 File Offset: 0x000D74B9
		// (set) Token: 0x06003330 RID: 13104 RVA: 0x000D84C1 File Offset: 0x000D74C1
		public Encoding NameEncoding
		{
			get
			{
				return this.nameEncoding;
			}
			set
			{
				this.nameEncoding = value;
				if (this.name != null && this.name != string.Empty)
				{
					this.SetContentTypeName();
				}
			}
		}

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x06003331 RID: 13105 RVA: 0x000D84EC File Offset: 0x000D74EC
		public ContentDisposition ContentDisposition
		{
			get
			{
				ContentDisposition contentDisposition = base.MimePart.ContentDisposition;
				if (contentDisposition == null)
				{
					contentDisposition = new ContentDisposition();
					base.MimePart.ContentDisposition = contentDisposition;
				}
				return contentDisposition;
			}
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x000D851B File Offset: 0x000D751B
		internal override void PrepareForSending()
		{
			if (this.name != null && this.name != string.Empty)
			{
				this.SetContentTypeName();
			}
			base.PrepareForSending();
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x000D8544 File Offset: 0x000D7544
		public static Attachment CreateAttachmentFromString(string content, string name)
		{
			Attachment attachment = new Attachment();
			attachment.SetContentFromString(content, null, string.Empty);
			attachment.Name = name;
			return attachment;
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x000D856C File Offset: 0x000D756C
		public static Attachment CreateAttachmentFromString(string content, string name, Encoding contentEncoding, string mediaType)
		{
			Attachment attachment = new Attachment();
			attachment.SetContentFromString(content, contentEncoding, mediaType);
			attachment.Name = name;
			return attachment;
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x000D8590 File Offset: 0x000D7590
		public static Attachment CreateAttachmentFromString(string content, ContentType contentType)
		{
			Attachment attachment = new Attachment();
			attachment.SetContentFromString(content, contentType);
			attachment.Name = contentType.Name;
			return attachment;
		}

		// Token: 0x04002F7B RID: 12155
		private string name;

		// Token: 0x04002F7C RID: 12156
		private Encoding nameEncoding;
	}
}

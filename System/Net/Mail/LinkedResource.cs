using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000695 RID: 1685
	public class LinkedResource : AttachmentBase
	{
		// Token: 0x060033F7 RID: 13303 RVA: 0x000DB560 File Offset: 0x000DA560
		internal LinkedResource()
		{
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x000DB568 File Offset: 0x000DA568
		public LinkedResource(string fileName) : base(fileName)
		{
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x000DB571 File Offset: 0x000DA571
		public LinkedResource(string fileName, string mediaType) : base(fileName, mediaType)
		{
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x000DB57B File Offset: 0x000DA57B
		public LinkedResource(string fileName, ContentType contentType) : base(fileName, contentType)
		{
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x000DB585 File Offset: 0x000DA585
		public LinkedResource(Stream contentStream) : base(contentStream)
		{
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x000DB58E File Offset: 0x000DA58E
		public LinkedResource(Stream contentStream, string mediaType) : base(contentStream, mediaType)
		{
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x000DB598 File Offset: 0x000DA598
		public LinkedResource(Stream contentStream, ContentType contentType) : base(contentStream, contentType)
		{
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x060033FE RID: 13310 RVA: 0x000DB5A2 File Offset: 0x000DA5A2
		// (set) Token: 0x060033FF RID: 13311 RVA: 0x000DB5AA File Offset: 0x000DA5AA
		public Uri ContentLink
		{
			get
			{
				return base.ContentLocation;
			}
			set
			{
				base.ContentLocation = value;
			}
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x000DB5B4 File Offset: 0x000DA5B4
		public static LinkedResource CreateLinkedResourceFromString(string content)
		{
			LinkedResource linkedResource = new LinkedResource();
			linkedResource.SetContentFromString(content, null, string.Empty);
			return linkedResource;
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x000DB5D8 File Offset: 0x000DA5D8
		public static LinkedResource CreateLinkedResourceFromString(string content, Encoding contentEncoding, string mediaType)
		{
			LinkedResource linkedResource = new LinkedResource();
			linkedResource.SetContentFromString(content, contentEncoding, mediaType);
			return linkedResource;
		}

		// Token: 0x06003402 RID: 13314 RVA: 0x000DB5F8 File Offset: 0x000DA5F8
		public static LinkedResource CreateLinkedResourceFromString(string content, ContentType contentType)
		{
			LinkedResource linkedResource = new LinkedResource();
			linkedResource.SetContentFromString(content, contentType);
			return linkedResource;
		}
	}
}

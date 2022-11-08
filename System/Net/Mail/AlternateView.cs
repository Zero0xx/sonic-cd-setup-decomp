using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000675 RID: 1653
	public class AlternateView : AttachmentBase
	{
		// Token: 0x06003311 RID: 13073 RVA: 0x000D8120 File Offset: 0x000D7120
		internal AlternateView()
		{
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x000D8128 File Offset: 0x000D7128
		public AlternateView(string fileName) : base(fileName)
		{
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x000D8131 File Offset: 0x000D7131
		public AlternateView(string fileName, string mediaType) : base(fileName, mediaType)
		{
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x000D813B File Offset: 0x000D713B
		public AlternateView(string fileName, ContentType contentType) : base(fileName, contentType)
		{
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x000D8145 File Offset: 0x000D7145
		public AlternateView(Stream contentStream) : base(contentStream)
		{
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x000D814E File Offset: 0x000D714E
		public AlternateView(Stream contentStream, string mediaType) : base(contentStream, mediaType)
		{
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x000D8158 File Offset: 0x000D7158
		public AlternateView(Stream contentStream, ContentType contentType) : base(contentStream, contentType)
		{
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x06003318 RID: 13080 RVA: 0x000D8162 File Offset: 0x000D7162
		public LinkedResourceCollection LinkedResources
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				if (this.linkedResources == null)
				{
					this.linkedResources = new LinkedResourceCollection();
				}
				return this.linkedResources;
			}
		}

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x06003319 RID: 13081 RVA: 0x000D8196 File Offset: 0x000D7196
		// (set) Token: 0x0600331A RID: 13082 RVA: 0x000D819E File Offset: 0x000D719E
		public Uri BaseUri
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

		// Token: 0x0600331B RID: 13083 RVA: 0x000D81A8 File Offset: 0x000D71A8
		public static AlternateView CreateAlternateViewFromString(string content)
		{
			AlternateView alternateView = new AlternateView();
			alternateView.SetContentFromString(content, null, string.Empty);
			return alternateView;
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x000D81CC File Offset: 0x000D71CC
		public static AlternateView CreateAlternateViewFromString(string content, Encoding contentEncoding, string mediaType)
		{
			AlternateView alternateView = new AlternateView();
			alternateView.SetContentFromString(content, contentEncoding, mediaType);
			return alternateView;
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x000D81EC File Offset: 0x000D71EC
		public static AlternateView CreateAlternateViewFromString(string content, ContentType contentType)
		{
			AlternateView alternateView = new AlternateView();
			alternateView.SetContentFromString(content, contentType);
			return alternateView;
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x000D8208 File Offset: 0x000D7208
		protected override void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (disposing && this.linkedResources != null)
			{
				this.linkedResources.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x04002F79 RID: 12153
		private LinkedResourceCollection linkedResources;
	}
}

using System;

namespace System.Text
{
	// Token: 0x0200040D RID: 1037
	[Serializable]
	public sealed class EncodingInfo
	{
		// Token: 0x06002A84 RID: 10884 RVA: 0x00084A10 File Offset: 0x00083A10
		internal EncodingInfo(int codePage, string name, string displayName)
		{
			this.iCodePage = codePage;
			this.strEncodingName = name;
			this.strDisplayName = displayName;
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06002A85 RID: 10885 RVA: 0x00084A2D File Offset: 0x00083A2D
		public int CodePage
		{
			get
			{
				return this.iCodePage;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06002A86 RID: 10886 RVA: 0x00084A35 File Offset: 0x00083A35
		public string Name
		{
			get
			{
				return this.strEncodingName;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002A87 RID: 10887 RVA: 0x00084A3D File Offset: 0x00083A3D
		public string DisplayName
		{
			get
			{
				return this.strDisplayName;
			}
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x00084A45 File Offset: 0x00083A45
		public Encoding GetEncoding()
		{
			return Encoding.GetEncoding(this.iCodePage);
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x00084A54 File Offset: 0x00083A54
		public override bool Equals(object value)
		{
			EncodingInfo encodingInfo = value as EncodingInfo;
			return encodingInfo != null && this.CodePage == encodingInfo.CodePage;
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x00084A7B File Offset: 0x00083A7B
		public override int GetHashCode()
		{
			return this.CodePage;
		}

		// Token: 0x040014AF RID: 5295
		private int iCodePage;

		// Token: 0x040014B0 RID: 5296
		private string strEncodingName;

		// Token: 0x040014B1 RID: 5297
		private string strDisplayName;
	}
}

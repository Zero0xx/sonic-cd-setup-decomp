using System;

namespace System.Globalization
{
	// Token: 0x020003B1 RID: 945
	[Serializable]
	internal class CodePageDataItem
	{
		// Token: 0x060025B0 RID: 9648 RVA: 0x0006906C File Offset: 0x0006806C
		internal unsafe CodePageDataItem(int dataIndex)
		{
			this.m_dataIndex = dataIndex;
			this.m_codePage = 0;
			this.m_uiFamilyCodePage = EncodingTable.codePageDataPtr[dataIndex].uiFamilyCodePage;
			this.m_webName = null;
			this.m_headerName = null;
			this.m_bodyName = null;
			this.m_description = null;
			this.m_flags = EncodingTable.codePageDataPtr[dataIndex].flags;
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060025B1 RID: 9649 RVA: 0x000690DD File Offset: 0x000680DD
		public unsafe virtual string WebName
		{
			get
			{
				if (this.m_webName == null)
				{
					this.m_webName = new string(EncodingTable.codePageDataPtr[this.m_dataIndex].webName);
				}
				return this.m_webName;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060025B2 RID: 9650 RVA: 0x00069111 File Offset: 0x00068111
		public virtual int UIFamilyCodePage
		{
			get
			{
				return this.m_uiFamilyCodePage;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060025B3 RID: 9651 RVA: 0x00069119 File Offset: 0x00068119
		public unsafe virtual string HeaderName
		{
			get
			{
				if (this.m_headerName == null)
				{
					this.m_headerName = new string(EncodingTable.codePageDataPtr[this.m_dataIndex].headerName);
				}
				return this.m_headerName;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060025B4 RID: 9652 RVA: 0x0006914D File Offset: 0x0006814D
		public unsafe virtual string BodyName
		{
			get
			{
				if (this.m_bodyName == null)
				{
					this.m_bodyName = new string(EncodingTable.codePageDataPtr[this.m_dataIndex].bodyName);
				}
				return this.m_bodyName;
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x060025B5 RID: 9653 RVA: 0x00069181 File Offset: 0x00068181
		public virtual uint Flags
		{
			get
			{
				return this.m_flags;
			}
		}

		// Token: 0x04001112 RID: 4370
		internal int m_dataIndex;

		// Token: 0x04001113 RID: 4371
		internal int m_codePage;

		// Token: 0x04001114 RID: 4372
		internal int m_uiFamilyCodePage;

		// Token: 0x04001115 RID: 4373
		internal string m_webName;

		// Token: 0x04001116 RID: 4374
		internal string m_headerName;

		// Token: 0x04001117 RID: 4375
		internal string m_bodyName;

		// Token: 0x04001118 RID: 4376
		internal string m_description;

		// Token: 0x04001119 RID: 4377
		internal uint m_flags;
	}
}

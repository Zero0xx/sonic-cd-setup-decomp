using System;

namespace System.Security
{
	// Token: 0x02000612 RID: 1554
	[Serializable]
	internal sealed class SecurityDocumentElement : ISecurityElementFactory
	{
		// Token: 0x06003820 RID: 14368 RVA: 0x000BC226 File Offset: 0x000BB226
		internal SecurityDocumentElement(SecurityDocument document, int position)
		{
			this.m_document = document;
			this.m_position = position;
		}

		// Token: 0x06003821 RID: 14369 RVA: 0x000BC23C File Offset: 0x000BB23C
		SecurityElement ISecurityElementFactory.CreateSecurityElement()
		{
			return this.m_document.GetElement(this.m_position, true);
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x000BC250 File Offset: 0x000BB250
		object ISecurityElementFactory.Copy()
		{
			return new SecurityDocumentElement(this.m_document, this.m_position);
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x000BC263 File Offset: 0x000BB263
		string ISecurityElementFactory.GetTag()
		{
			return this.m_document.GetTagForElement(this.m_position);
		}

		// Token: 0x06003824 RID: 14372 RVA: 0x000BC276 File Offset: 0x000BB276
		string ISecurityElementFactory.Attribute(string attributeName)
		{
			return this.m_document.GetAttributeForElement(this.m_position, attributeName);
		}

		// Token: 0x04001D16 RID: 7446
		private int m_position;

		// Token: 0x04001D17 RID: 7447
		private SecurityDocument m_document;
	}
}

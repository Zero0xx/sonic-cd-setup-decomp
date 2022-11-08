using System;

namespace System.Reflection.Emit
{
	// Token: 0x02000803 RID: 2051
	internal class NativeVersionInfo
	{
		// Token: 0x060048B6 RID: 18614 RVA: 0x000FD4E4 File Offset: 0x000FC4E4
		internal NativeVersionInfo()
		{
			this.m_strDescription = null;
			this.m_strCompany = null;
			this.m_strTitle = null;
			this.m_strCopyright = null;
			this.m_strTrademark = null;
			this.m_strProduct = null;
			this.m_strProductVersion = null;
			this.m_strFileVersion = null;
			this.m_lcid = -1;
		}

		// Token: 0x0400257C RID: 9596
		internal string m_strDescription;

		// Token: 0x0400257D RID: 9597
		internal string m_strCompany;

		// Token: 0x0400257E RID: 9598
		internal string m_strTitle;

		// Token: 0x0400257F RID: 9599
		internal string m_strCopyright;

		// Token: 0x04002580 RID: 9600
		internal string m_strTrademark;

		// Token: 0x04002581 RID: 9601
		internal string m_strProduct;

		// Token: 0x04002582 RID: 9602
		internal string m_strProductVersion;

		// Token: 0x04002583 RID: 9603
		internal string m_strFileVersion;

		// Token: 0x04002584 RID: 9604
		internal int m_lcid;
	}
}

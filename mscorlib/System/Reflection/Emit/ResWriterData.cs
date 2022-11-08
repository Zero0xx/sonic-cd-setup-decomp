using System;
using System.IO;
using System.Resources;

namespace System.Reflection.Emit
{
	// Token: 0x02000802 RID: 2050
	internal class ResWriterData
	{
		// Token: 0x060048B5 RID: 18613 RVA: 0x000FD4A5 File Offset: 0x000FC4A5
		internal ResWriterData(ResourceWriter resWriter, Stream memoryStream, string strName, string strFileName, string strFullFileName, ResourceAttributes attribute)
		{
			this.m_resWriter = resWriter;
			this.m_memoryStream = memoryStream;
			this.m_strName = strName;
			this.m_strFileName = strFileName;
			this.m_strFullFileName = strFullFileName;
			this.m_nextResWriter = null;
			this.m_attribute = attribute;
		}

		// Token: 0x04002575 RID: 9589
		internal ResourceWriter m_resWriter;

		// Token: 0x04002576 RID: 9590
		internal string m_strName;

		// Token: 0x04002577 RID: 9591
		internal string m_strFileName;

		// Token: 0x04002578 RID: 9592
		internal string m_strFullFileName;

		// Token: 0x04002579 RID: 9593
		internal Stream m_memoryStream;

		// Token: 0x0400257A RID: 9594
		internal ResWriterData m_nextResWriter;

		// Token: 0x0400257B RID: 9595
		internal ResourceAttributes m_attribute;
	}
}

using System;
using System.Diagnostics.SymbolStore;

namespace System.Reflection.Emit
{
	// Token: 0x0200082B RID: 2091
	internal class LineNumberInfo
	{
		// Token: 0x06004A6F RID: 19055 RVA: 0x00102807 File Offset: 0x00101807
		internal LineNumberInfo()
		{
			this.m_DocumentCount = 0;
			this.m_iLastFound = 0;
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x00102820 File Offset: 0x00101820
		internal void AddLineNumberInfo(ISymbolDocumentWriter document, int iOffset, int iStartLine, int iStartColumn, int iEndLine, int iEndColumn)
		{
			int num = this.FindDocument(document);
			this.m_Documents[num].AddLineNumberInfo(document, iOffset, iStartLine, iStartColumn, iEndLine, iEndColumn);
		}

		// Token: 0x06004A71 RID: 19057 RVA: 0x0010284C File Offset: 0x0010184C
		internal int FindDocument(ISymbolDocumentWriter document)
		{
			if (this.m_iLastFound < this.m_DocumentCount && this.m_Documents[this.m_iLastFound] == document)
			{
				return this.m_iLastFound;
			}
			for (int i = 0; i < this.m_DocumentCount; i++)
			{
				if (this.m_Documents[i].m_document == document)
				{
					this.m_iLastFound = i;
					return this.m_iLastFound;
				}
			}
			this.EnsureCapacity();
			this.m_iLastFound = this.m_DocumentCount;
			this.m_Documents[this.m_DocumentCount++] = new REDocument(document);
			return this.m_iLastFound;
		}

		// Token: 0x06004A72 RID: 19058 RVA: 0x001028E4 File Offset: 0x001018E4
		internal void EnsureCapacity()
		{
			if (this.m_DocumentCount == 0)
			{
				this.m_Documents = new REDocument[16];
				return;
			}
			if (this.m_DocumentCount == this.m_Documents.Length)
			{
				REDocument[] array = new REDocument[this.m_DocumentCount * 2];
				Array.Copy(this.m_Documents, array, this.m_DocumentCount);
				this.m_Documents = array;
			}
		}

		// Token: 0x06004A73 RID: 19059 RVA: 0x00102940 File Offset: 0x00101940
		internal void EmitLineNumberInfo(ISymbolWriter symWriter)
		{
			for (int i = 0; i < this.m_DocumentCount; i++)
			{
				this.m_Documents[i].EmitLineNumberInfo(symWriter);
			}
		}

		// Token: 0x04002613 RID: 9747
		internal const int InitialSize = 16;

		// Token: 0x04002614 RID: 9748
		internal int m_DocumentCount;

		// Token: 0x04002615 RID: 9749
		internal REDocument[] m_Documents;

		// Token: 0x04002616 RID: 9750
		private int m_iLastFound;
	}
}

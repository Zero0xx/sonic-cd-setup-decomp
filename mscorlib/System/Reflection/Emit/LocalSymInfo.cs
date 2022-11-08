using System;
using System.Diagnostics.SymbolStore;

namespace System.Reflection.Emit
{
	// Token: 0x02000831 RID: 2097
	internal class LocalSymInfo
	{
		// Token: 0x06004AD7 RID: 19159 RVA: 0x00103E34 File Offset: 0x00102E34
		internal LocalSymInfo()
		{
			this.m_iLocalSymCount = 0;
			this.m_iNameSpaceCount = 0;
		}

		// Token: 0x06004AD8 RID: 19160 RVA: 0x00103E4C File Offset: 0x00102E4C
		private void EnsureCapacityNamespace()
		{
			if (this.m_iNameSpaceCount == 0)
			{
				this.m_namespace = new string[16];
				return;
			}
			if (this.m_iNameSpaceCount == this.m_namespace.Length)
			{
				string[] array = new string[this.m_iNameSpaceCount * 2];
				Array.Copy(this.m_namespace, array, this.m_iNameSpaceCount);
				this.m_namespace = array;
			}
		}

		// Token: 0x06004AD9 RID: 19161 RVA: 0x00103EA8 File Offset: 0x00102EA8
		private void EnsureCapacity()
		{
			if (this.m_iLocalSymCount == 0)
			{
				this.m_strName = new string[16];
				this.m_ubSignature = new byte[16][];
				this.m_iLocalSlot = new int[16];
				this.m_iStartOffset = new int[16];
				this.m_iEndOffset = new int[16];
				return;
			}
			if (this.m_iLocalSymCount == this.m_strName.Length)
			{
				int[] array = new int[this.m_iLocalSymCount * 2];
				Array.Copy(this.m_iLocalSlot, array, this.m_iLocalSymCount);
				this.m_iLocalSlot = array;
				array = new int[this.m_iLocalSymCount * 2];
				Array.Copy(this.m_iStartOffset, array, this.m_iLocalSymCount);
				this.m_iStartOffset = array;
				array = new int[this.m_iLocalSymCount * 2];
				Array.Copy(this.m_iEndOffset, array, this.m_iLocalSymCount);
				this.m_iEndOffset = array;
				string[] array2 = new string[this.m_iLocalSymCount * 2];
				Array.Copy(this.m_strName, array2, this.m_iLocalSymCount);
				this.m_strName = array2;
				byte[][] array3 = new byte[this.m_iLocalSymCount * 2][];
				Array.Copy(this.m_ubSignature, array3, this.m_iLocalSymCount);
				this.m_ubSignature = array3;
			}
		}

		// Token: 0x06004ADA RID: 19162 RVA: 0x00103FD8 File Offset: 0x00102FD8
		internal void AddLocalSymInfo(string strName, byte[] signature, int slot, int startOffset, int endOffset)
		{
			this.EnsureCapacity();
			this.m_iStartOffset[this.m_iLocalSymCount] = startOffset;
			this.m_iEndOffset[this.m_iLocalSymCount] = endOffset;
			this.m_iLocalSlot[this.m_iLocalSymCount] = slot;
			this.m_strName[this.m_iLocalSymCount] = strName;
			this.m_ubSignature[this.m_iLocalSymCount] = signature;
			this.m_iLocalSymCount++;
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x00104044 File Offset: 0x00103044
		internal void AddUsingNamespace(string strNamespace)
		{
			this.EnsureCapacityNamespace();
			this.m_namespace[this.m_iNameSpaceCount++] = strNamespace;
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x00104070 File Offset: 0x00103070
		internal virtual void EmitLocalSymInfo(ISymbolWriter symWriter)
		{
			for (int i = 0; i < this.m_iLocalSymCount; i++)
			{
				symWriter.DefineLocalVariable(this.m_strName[i], FieldAttributes.PrivateScope, this.m_ubSignature[i], SymAddressKind.ILOffset, this.m_iLocalSlot[i], 0, 0, this.m_iStartOffset[i], this.m_iEndOffset[i]);
			}
			for (int i = 0; i < this.m_iNameSpaceCount; i++)
			{
				symWriter.UsingNamespace(this.m_namespace[i]);
			}
		}

		// Token: 0x04002646 RID: 9798
		internal const int InitialSize = 16;

		// Token: 0x04002647 RID: 9799
		internal string[] m_strName;

		// Token: 0x04002648 RID: 9800
		internal byte[][] m_ubSignature;

		// Token: 0x04002649 RID: 9801
		internal int[] m_iLocalSlot;

		// Token: 0x0400264A RID: 9802
		internal int[] m_iStartOffset;

		// Token: 0x0400264B RID: 9803
		internal int[] m_iEndOffset;

		// Token: 0x0400264C RID: 9804
		internal int m_iLocalSymCount;

		// Token: 0x0400264D RID: 9805
		internal string[] m_namespace;

		// Token: 0x0400264E RID: 9806
		internal int m_iNameSpaceCount;
	}
}

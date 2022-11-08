using System;

namespace System.Globalization
{
	// Token: 0x02000385 RID: 901
	internal abstract class BaseInfoTable
	{
		// Token: 0x06002316 RID: 8982 RVA: 0x00058B7F File Offset: 0x00057B7F
		internal BaseInfoTable(string fileName, bool fromAssembly)
		{
			this.fileName = fileName;
			this.fromAssembly = fromAssembly;
			this.InitializeBaseInfoTablePointers(fileName, fromAssembly);
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x00058BA4 File Offset: 0x00057BA4
		internal unsafe void InitializeBaseInfoTablePointers(string fileName, bool fromAssembly)
		{
			if (fromAssembly)
			{
				this.m_pDataFileStart = GlobalizationAssembly.GetGlobalizationResourceBytePtr(typeof(BaseInfoTable).Assembly, fileName);
			}
			else
			{
				this.memoryMapFile = new AgileSafeNativeMemoryHandle(fileName);
				if (this.memoryMapFile.FileSize == 0L)
				{
					this.m_valid = false;
					return;
				}
				this.m_pDataFileStart = this.memoryMapFile.GetBytePtr();
			}
			EndianessHeader* pDataFileStart = (EndianessHeader*)this.m_pDataFileStart;
			this.m_pCultureHeader = (CultureTableHeader*)(this.m_pDataFileStart + pDataFileStart->leOffset);
			this.SetDataItemPointers();
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x00058C25 File Offset: 0x00057C25
		internal bool IsValid
		{
			get
			{
				return this.m_valid;
			}
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x00058C30 File Offset: 0x00057C30
		public override bool Equals(object value)
		{
			BaseInfoTable baseInfoTable = value as BaseInfoTable;
			return baseInfoTable != null && this.fromAssembly == baseInfoTable.fromAssembly && CultureInfo.InvariantCulture.CompareInfo.Compare(this.fileName, baseInfoTable.fileName, CompareOptions.IgnoreCase) == 0;
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x00058C78 File Offset: 0x00057C78
		public override int GetHashCode()
		{
			return this.fileName.GetHashCode();
		}

		// Token: 0x0600231B RID: 8987
		internal abstract void SetDataItemPointers();

		// Token: 0x0600231C RID: 8988 RVA: 0x00058C88 File Offset: 0x00057C88
		internal unsafe string GetStringPoolString(uint offset)
		{
			char* ptr = (char*)(this.m_pDataPool + offset);
			if (ptr[1] == '\0')
			{
				return string.Empty;
			}
			return new string(ptr + 1, 0, (int)(*ptr));
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x00058CBC File Offset: 0x00057CBC
		internal unsafe string[] GetStringArray(uint iOffset)
		{
			if (iOffset == 0U)
			{
				return new string[0];
			}
			ushort* ptr = this.m_pDataPool + iOffset;
			int num = (int)(*ptr);
			string[] array = new string[num];
			uint* ptr2 = (uint*)(ptr + 1);
			for (int i = 0; i < num; i++)
			{
				array[i] = this.GetStringPoolString(ptr2[i]);
			}
			return array;
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x00058D14 File Offset: 0x00057D14
		internal unsafe int[][] GetWordArrayArray(uint iOffset)
		{
			if (iOffset == 0U)
			{
				return new int[0][];
			}
			short* ptr = (short*)(this.m_pDataPool + iOffset);
			int num = (int)(*ptr);
			int[][] array = new int[num][];
			uint* ptr2 = (uint*)(ptr + 1);
			for (int i = 0; i < num; i++)
			{
				ptr = (short*)(this.m_pDataPool + ptr2[i]);
				int num2 = (int)(*ptr);
				ptr++;
				array[i] = new int[num2];
				for (int j = 0; j < num2; j++)
				{
					array[i][j] = (int)ptr[j];
				}
			}
			return array;
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x00058DA0 File Offset: 0x00057DA0
		internal unsafe int CompareStringToStringPoolStringBinary(string name, int offset)
		{
			int num = 0;
			char* ptr = (char*)(this.m_pDataPool + offset);
			if (ptr[1] == '\0')
			{
				if (name.Length == 0)
				{
					return 0;
				}
				return 1;
			}
			else
			{
				int num2 = 0;
				while (num2 < (int)(*ptr) && num2 < name.Length)
				{
					num = (int)(name[num2] - ((ptr[num2 + 1] <= 'Z' && ptr[num2 + 1] >= 'A') ? (ptr[num2 + 1] + 'a' - 'A') : ptr[num2 + 1]));
					if (num != 0)
					{
						break;
					}
					num2++;
				}
				if (num != 0)
				{
					return num;
				}
				return name.Length - (int)(*ptr);
			}
		}

		// Token: 0x04000EC2 RID: 3778
		internal unsafe byte* m_pDataFileStart;

		// Token: 0x04000EC3 RID: 3779
		protected AgileSafeNativeMemoryHandle memoryMapFile;

		// Token: 0x04000EC4 RID: 3780
		protected unsafe CultureTableHeader* m_pCultureHeader;

		// Token: 0x04000EC5 RID: 3781
		internal unsafe byte* m_pItemData;

		// Token: 0x04000EC6 RID: 3782
		internal uint m_numItem;

		// Token: 0x04000EC7 RID: 3783
		internal uint m_itemSize;

		// Token: 0x04000EC8 RID: 3784
		internal unsafe ushort* m_pDataPool;

		// Token: 0x04000EC9 RID: 3785
		internal bool fromAssembly;

		// Token: 0x04000ECA RID: 3786
		internal string fileName;

		// Token: 0x04000ECB RID: 3787
		protected bool m_valid = true;
	}
}

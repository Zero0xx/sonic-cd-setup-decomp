using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x02000785 RID: 1925
	public class ProcessModuleCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06003B5D RID: 15197 RVA: 0x000FD736 File Offset: 0x000FC736
		protected ProcessModuleCollection()
		{
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x000FD73E File Offset: 0x000FC73E
		public ProcessModuleCollection(ProcessModule[] processModules)
		{
			base.InnerList.AddRange(processModules);
		}

		// Token: 0x17000DE7 RID: 3559
		public ProcessModule this[int index]
		{
			get
			{
				return (ProcessModule)base.InnerList[index];
			}
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x000FD765 File Offset: 0x000FC765
		public int IndexOf(ProcessModule module)
		{
			return base.InnerList.IndexOf(module);
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x000FD773 File Offset: 0x000FC773
		public bool Contains(ProcessModule module)
		{
			return base.InnerList.Contains(module);
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x000FD781 File Offset: 0x000FC781
		public void CopyTo(ProcessModule[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}
	}
}

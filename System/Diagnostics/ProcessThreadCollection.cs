using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x0200078A RID: 1930
	public class ProcessThreadCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06003BA3 RID: 15267 RVA: 0x000FDF28 File Offset: 0x000FCF28
		protected ProcessThreadCollection()
		{
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x000FDF30 File Offset: 0x000FCF30
		public ProcessThreadCollection(ProcessThread[] processThreads)
		{
			base.InnerList.AddRange(processThreads);
		}

		// Token: 0x17000E0A RID: 3594
		public ProcessThread this[int index]
		{
			get
			{
				return (ProcessThread)base.InnerList[index];
			}
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x000FDF57 File Offset: 0x000FCF57
		public int Add(ProcessThread thread)
		{
			return base.InnerList.Add(thread);
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x000FDF65 File Offset: 0x000FCF65
		public void Insert(int index, ProcessThread thread)
		{
			base.InnerList.Insert(index, thread);
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x000FDF74 File Offset: 0x000FCF74
		public int IndexOf(ProcessThread thread)
		{
			return base.InnerList.IndexOf(thread);
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x000FDF82 File Offset: 0x000FCF82
		public bool Contains(ProcessThread thread)
		{
			return base.InnerList.Contains(thread);
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x000FDF90 File Offset: 0x000FCF90
		public void Remove(ProcessThread thread)
		{
			base.InnerList.Remove(thread);
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x000FDF9E File Offset: 0x000FCF9E
		public void CopyTo(ProcessThread[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}
	}
}

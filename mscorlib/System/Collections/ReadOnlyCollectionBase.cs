using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x02000279 RID: 633
	[ComVisible(true)]
	[Serializable]
	public abstract class ReadOnlyCollectionBase : ICollection, IEnumerable
	{
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060018CA RID: 6346 RVA: 0x000405F4 File Offset: 0x0003F5F4
		protected ArrayList InnerList
		{
			get
			{
				if (this.list == null)
				{
					this.list = new ArrayList();
				}
				return this.list;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x060018CB RID: 6347 RVA: 0x0004060F File Offset: 0x0003F60F
		public virtual int Count
		{
			get
			{
				return this.InnerList.Count;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x060018CC RID: 6348 RVA: 0x0004061C File Offset: 0x0003F61C
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InnerList.IsSynchronized;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x00040629 File Offset: 0x0003F629
		object ICollection.SyncRoot
		{
			get
			{
				return this.InnerList.SyncRoot;
			}
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x00040636 File Offset: 0x0003F636
		void ICollection.CopyTo(Array array, int index)
		{
			this.InnerList.CopyTo(array, index);
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x00040645 File Offset: 0x0003F645
		public virtual IEnumerator GetEnumerator()
		{
			return this.InnerList.GetEnumerator();
		}

		// Token: 0x040009DD RID: 2525
		private ArrayList list;
	}
}

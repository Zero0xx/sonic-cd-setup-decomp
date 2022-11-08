using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000116 RID: 278
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ListSortDescriptionCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x06000890 RID: 2192 RVA: 0x0001CDA0 File Offset: 0x0001BDA0
		public ListSortDescriptionCollection()
		{
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0001CDB4 File Offset: 0x0001BDB4
		public ListSortDescriptionCollection(ListSortDescription[] sorts)
		{
			if (sorts != null)
			{
				for (int i = 0; i < sorts.Length; i++)
				{
					this.sorts.Add(sorts[i]);
				}
			}
		}

		// Token: 0x170001C0 RID: 448
		public ListSortDescription this[int index]
		{
			get
			{
				return (ListSortDescription)this.sorts[index];
			}
			set
			{
				throw new InvalidOperationException(SR.GetString("CantModifyListSortDescriptionCollection"));
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x0001CE16 File Offset: 0x0001BE16
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x0001CE19 File Offset: 0x0001BE19
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001C3 RID: 451
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				throw new InvalidOperationException(SR.GetString("CantModifyListSortDescriptionCollection"));
			}
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0001CE36 File Offset: 0x0001BE36
		int IList.Add(object value)
		{
			throw new InvalidOperationException(SR.GetString("CantModifyListSortDescriptionCollection"));
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0001CE47 File Offset: 0x0001BE47
		void IList.Clear()
		{
			throw new InvalidOperationException(SR.GetString("CantModifyListSortDescriptionCollection"));
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0001CE58 File Offset: 0x0001BE58
		public bool Contains(object value)
		{
			return ((IList)this.sorts).Contains(value);
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0001CE66 File Offset: 0x0001BE66
		public int IndexOf(object value)
		{
			return ((IList)this.sorts).IndexOf(value);
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0001CE74 File Offset: 0x0001BE74
		void IList.Insert(int index, object value)
		{
			throw new InvalidOperationException(SR.GetString("CantModifyListSortDescriptionCollection"));
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0001CE85 File Offset: 0x0001BE85
		void IList.Remove(object value)
		{
			throw new InvalidOperationException(SR.GetString("CantModifyListSortDescriptionCollection"));
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0001CE96 File Offset: 0x0001BE96
		void IList.RemoveAt(int index)
		{
			throw new InvalidOperationException(SR.GetString("CantModifyListSortDescriptionCollection"));
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x0001CEA7 File Offset: 0x0001BEA7
		public int Count
		{
			get
			{
				return this.sorts.Count;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x0001CEB4 File Offset: 0x0001BEB4
		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0001CEB7 File Offset: 0x0001BEB7
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0001CEBA File Offset: 0x0001BEBA
		public void CopyTo(Array array, int index)
		{
			this.sorts.CopyTo(array, index);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0001CEC9 File Offset: 0x0001BEC9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.sorts.GetEnumerator();
		}

		// Token: 0x040009B2 RID: 2482
		private ArrayList sorts = new ArrayList();
	}
}

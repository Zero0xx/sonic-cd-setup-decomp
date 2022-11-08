using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000770 RID: 1904
	[Serializable]
	public class PerformanceCounterPermissionEntryCollection : CollectionBase
	{
		// Token: 0x06003A90 RID: 14992 RVA: 0x000F92EC File Offset: 0x000F82EC
		internal PerformanceCounterPermissionEntryCollection(PerformanceCounterPermission owner, ResourcePermissionBaseEntry[] entries)
		{
			this.owner = owner;
			for (int i = 0; i < entries.Length; i++)
			{
				base.InnerList.Add(new PerformanceCounterPermissionEntry(entries[i]));
			}
		}

		// Token: 0x17000DA4 RID: 3492
		public PerformanceCounterPermissionEntry this[int index]
		{
			get
			{
				return (PerformanceCounterPermissionEntry)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x000F934A File Offset: 0x000F834A
		public int Add(PerformanceCounterPermissionEntry value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06003A94 RID: 14996 RVA: 0x000F9358 File Offset: 0x000F8358
		public void AddRange(PerformanceCounterPermissionEntry[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06003A95 RID: 14997 RVA: 0x000F938C File Offset: 0x000F838C
		public void AddRange(PerformanceCounterPermissionEntryCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int count = value.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x000F93C8 File Offset: 0x000F83C8
		public bool Contains(PerformanceCounterPermissionEntry value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x000F93D6 File Offset: 0x000F83D6
		public void CopyTo(PerformanceCounterPermissionEntry[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x000F93E5 File Offset: 0x000F83E5
		public int IndexOf(PerformanceCounterPermissionEntry value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x000F93F3 File Offset: 0x000F83F3
		public void Insert(int index, PerformanceCounterPermissionEntry value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06003A9A RID: 15002 RVA: 0x000F9402 File Offset: 0x000F8402
		public void Remove(PerformanceCounterPermissionEntry value)
		{
			base.List.Remove(value);
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x000F9410 File Offset: 0x000F8410
		protected override void OnClear()
		{
			this.owner.Clear();
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x000F941D File Offset: 0x000F841D
		protected override void OnInsert(int index, object value)
		{
			this.owner.AddPermissionAccess((PerformanceCounterPermissionEntry)value);
		}

		// Token: 0x06003A9D RID: 15005 RVA: 0x000F9430 File Offset: 0x000F8430
		protected override void OnRemove(int index, object value)
		{
			this.owner.RemovePermissionAccess((PerformanceCounterPermissionEntry)value);
		}

		// Token: 0x06003A9E RID: 15006 RVA: 0x000F9443 File Offset: 0x000F8443
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			this.owner.RemovePermissionAccess((PerformanceCounterPermissionEntry)oldValue);
			this.owner.AddPermissionAccess((PerformanceCounterPermissionEntry)newValue);
		}

		// Token: 0x0400334B RID: 13131
		private PerformanceCounterPermission owner;
	}
}

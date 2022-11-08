using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000757 RID: 1879
	[Serializable]
	public class EventLogPermissionEntryCollection : CollectionBase
	{
		// Token: 0x06003985 RID: 14725 RVA: 0x000F4268 File Offset: 0x000F3268
		internal EventLogPermissionEntryCollection(EventLogPermission owner, ResourcePermissionBaseEntry[] entries)
		{
			this.owner = owner;
			for (int i = 0; i < entries.Length; i++)
			{
				base.InnerList.Add(new EventLogPermissionEntry(entries[i]));
			}
		}

		// Token: 0x17000D59 RID: 3417
		public EventLogPermissionEntry this[int index]
		{
			get
			{
				return (EventLogPermissionEntry)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x000F42C6 File Offset: 0x000F32C6
		public int Add(EventLogPermissionEntry value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06003989 RID: 14729 RVA: 0x000F42D4 File Offset: 0x000F32D4
		public void AddRange(EventLogPermissionEntry[] value)
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

		// Token: 0x0600398A RID: 14730 RVA: 0x000F4308 File Offset: 0x000F3308
		public void AddRange(EventLogPermissionEntryCollection value)
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

		// Token: 0x0600398B RID: 14731 RVA: 0x000F4344 File Offset: 0x000F3344
		public bool Contains(EventLogPermissionEntry value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x0600398C RID: 14732 RVA: 0x000F4352 File Offset: 0x000F3352
		public void CopyTo(EventLogPermissionEntry[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x000F4361 File Offset: 0x000F3361
		public int IndexOf(EventLogPermissionEntry value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x0600398E RID: 14734 RVA: 0x000F436F File Offset: 0x000F336F
		public void Insert(int index, EventLogPermissionEntry value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x000F437E File Offset: 0x000F337E
		public void Remove(EventLogPermissionEntry value)
		{
			base.List.Remove(value);
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x000F438C File Offset: 0x000F338C
		protected override void OnClear()
		{
			this.owner.Clear();
		}

		// Token: 0x06003991 RID: 14737 RVA: 0x000F4399 File Offset: 0x000F3399
		protected override void OnInsert(int index, object value)
		{
			this.owner.AddPermissionAccess((EventLogPermissionEntry)value);
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x000F43AC File Offset: 0x000F33AC
		protected override void OnRemove(int index, object value)
		{
			this.owner.RemovePermissionAccess((EventLogPermissionEntry)value);
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x000F43BF File Offset: 0x000F33BF
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			this.owner.RemovePermissionAccess((EventLogPermissionEntry)oldValue);
			this.owner.AddPermissionAccess((EventLogPermissionEntry)newValue);
		}

		// Token: 0x040032BD RID: 12989
		private EventLogPermission owner;
	}
}

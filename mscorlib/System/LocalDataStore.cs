using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System
{
	// Token: 0x020000CE RID: 206
	internal class LocalDataStore
	{
		// Token: 0x06000B90 RID: 2960 RVA: 0x00023195 File Offset: 0x00022195
		public LocalDataStore(LocalDataStoreMgr mgr, int InitialCapacity)
		{
			if (mgr == null)
			{
				throw new ArgumentNullException("mgr");
			}
			this.m_Manager = mgr;
			this.m_DataTable = new object[InitialCapacity];
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x000231C0 File Offset: 0x000221C0
		public object GetData(LocalDataStoreSlot slot)
		{
			object result = null;
			this.m_Manager.ValidateSlot(slot);
			int slot2 = slot.Slot;
			if (slot2 >= 0)
			{
				if (slot2 >= this.m_DataTable.Length)
				{
					return null;
				}
				result = this.m_DataTable[slot2];
			}
			if (!slot.IsValid())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_SlotHasBeenFreed"));
			}
			return result;
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00023218 File Offset: 0x00022218
		public void SetData(LocalDataStoreSlot slot, object data)
		{
			this.m_Manager.ValidateSlot(slot);
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(this.m_Manager, ref flag);
				if (!slot.IsValid())
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_SlotHasBeenFreed"));
				}
				this.SetDataInternal(slot.Slot, data, true);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this.m_Manager);
				}
			}
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0002328C File Offset: 0x0002228C
		internal void SetDataInternal(int slot, object data, bool bAlloc)
		{
			if (slot >= this.m_DataTable.Length)
			{
				if (!bAlloc)
				{
					return;
				}
				this.SetCapacity(this.m_Manager.GetSlotTableLength());
			}
			this.m_DataTable[slot] = data;
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x000232B8 File Offset: 0x000222B8
		private void SetCapacity(int capacity)
		{
			if (capacity < this.m_DataTable.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ALSInvalidCapacity"));
			}
			object[] array = new object[capacity];
			Array.Copy(this.m_DataTable, array, this.m_DataTable.Length);
			this.m_DataTable = array;
		}

		// Token: 0x04000421 RID: 1057
		private object[] m_DataTable;

		// Token: 0x04000422 RID: 1058
		private LocalDataStoreMgr m_Manager;

		// Token: 0x04000423 RID: 1059
		private int DONT_USE_InternalStore;
	}
}

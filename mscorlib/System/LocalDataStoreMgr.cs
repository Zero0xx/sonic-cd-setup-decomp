using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System
{
	// Token: 0x020000D1 RID: 209
	internal class LocalDataStoreMgr
	{
		// Token: 0x06000B9D RID: 2973 RVA: 0x000233CC File Offset: 0x000223CC
		public LocalDataStore CreateLocalDataStore()
		{
			LocalDataStore localDataStore = new LocalDataStore(this, this.m_SlotInfoTable.Length);
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				this.m_ManagedLocalDataStores.Add(localDataStore);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
			return localDataStore;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x00023424 File Offset: 0x00022424
		public void DeleteLocalDataStore(LocalDataStore store)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				this.m_ManagedLocalDataStores.Remove(store);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x00023468 File Offset: 0x00022468
		public LocalDataStoreSlot AllocateDataSlot()
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			LocalDataStoreSlot result;
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				int num = this.m_SlotInfoTable.Length;
				if (this.m_FirstAvailableSlot < num)
				{
					LocalDataStoreSlot localDataStoreSlot = new LocalDataStoreSlot(this, this.m_FirstAvailableSlot);
					this.m_SlotInfoTable[this.m_FirstAvailableSlot] = 1;
					int num2 = this.m_FirstAvailableSlot + 1;
					while (num2 < num && (this.m_SlotInfoTable[num2] & 1) != 0)
					{
						num2++;
					}
					this.m_FirstAvailableSlot = num2;
					result = localDataStoreSlot;
				}
				else
				{
					int num3;
					if (num < 512)
					{
						num3 = num * 2;
					}
					else
					{
						num3 = num + 128;
					}
					byte[] array = new byte[num3];
					Array.Copy(this.m_SlotInfoTable, array, num);
					this.m_SlotInfoTable = array;
					LocalDataStoreSlot localDataStoreSlot = new LocalDataStoreSlot(this, num);
					this.m_SlotInfoTable[num] = 1;
					this.m_FirstAvailableSlot = num + 1;
					result = localDataStoreSlot;
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
			return result;
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x00023550 File Offset: 0x00022550
		public LocalDataStoreSlot AllocateNamedDataSlot(string name)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			LocalDataStoreSlot result;
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				LocalDataStoreSlot localDataStoreSlot = this.AllocateDataSlot();
				this.m_KeyToSlotMap.Add(name, localDataStoreSlot);
				result = localDataStoreSlot;
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
			return result;
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x000235A0 File Offset: 0x000225A0
		public LocalDataStoreSlot GetNamedDataSlot(string name)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			LocalDataStoreSlot result;
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				LocalDataStoreSlot localDataStoreSlot = (LocalDataStoreSlot)this.m_KeyToSlotMap[name];
				if (localDataStoreSlot == null)
				{
					result = this.AllocateNamedDataSlot(name);
				}
				else
				{
					result = localDataStoreSlot;
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
			return result;
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x000235FC File Offset: 0x000225FC
		public void FreeNamedDataSlot(string name)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				this.m_KeyToSlotMap.Remove(name);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00023640 File Offset: 0x00022640
		internal void FreeDataSlot(int slot)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				for (int i = 0; i < this.m_ManagedLocalDataStores.Count; i++)
				{
					((LocalDataStore)this.m_ManagedLocalDataStores[i]).SetDataInternal(slot, null, false);
				}
				this.m_SlotInfoTable[slot] = 0;
				if (slot < this.m_FirstAvailableSlot)
				{
					this.m_FirstAvailableSlot = slot;
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x000236C0 File Offset: 0x000226C0
		public void ValidateSlot(LocalDataStoreSlot slot)
		{
			if (slot == null || slot.Manager != this)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ALSInvalidSlot"));
			}
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x000236DE File Offset: 0x000226DE
		internal int GetSlotTableLength()
		{
			return this.m_SlotInfoTable.Length;
		}

		// Token: 0x04000427 RID: 1063
		private const byte DataSlotOccupied = 1;

		// Token: 0x04000428 RID: 1064
		private const int InitialSlotTableSize = 64;

		// Token: 0x04000429 RID: 1065
		private const int SlotTableDoubleThreshold = 512;

		// Token: 0x0400042A RID: 1066
		private const int LargeSlotTableSizeIncrease = 128;

		// Token: 0x0400042B RID: 1067
		private byte[] m_SlotInfoTable = new byte[64];

		// Token: 0x0400042C RID: 1068
		private int m_FirstAvailableSlot;

		// Token: 0x0400042D RID: 1069
		private ArrayList m_ManagedLocalDataStores = new ArrayList();

		// Token: 0x0400042E RID: 1070
		private Hashtable m_KeyToSlotMap = new Hashtable();
	}
}

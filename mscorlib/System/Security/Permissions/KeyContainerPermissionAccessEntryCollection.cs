using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200065F RID: 1631
	[ComVisible(true)]
	[Serializable]
	public sealed class KeyContainerPermissionAccessEntryCollection : ICollection, IEnumerable
	{
		// Token: 0x06003AD2 RID: 15058 RVA: 0x000C6A35 File Offset: 0x000C5A35
		private KeyContainerPermissionAccessEntryCollection()
		{
		}

		// Token: 0x06003AD3 RID: 15059 RVA: 0x000C6A3D File Offset: 0x000C5A3D
		internal KeyContainerPermissionAccessEntryCollection(KeyContainerPermissionFlags globalFlags)
		{
			this.m_list = new ArrayList();
			this.m_globalFlags = globalFlags;
		}

		// Token: 0x170009E9 RID: 2537
		public KeyContainerPermissionAccessEntry this[int index]
		{
			get
			{
				if (index < 0)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
				}
				if (index >= this.m_list.Count)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				return (KeyContainerPermissionAccessEntry)this.m_list[index];
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06003AD5 RID: 15061 RVA: 0x000C6AAD File Offset: 0x000C5AAD
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x000C6ABC File Offset: 0x000C5ABC
		public int Add(KeyContainerPermissionAccessEntry accessEntry)
		{
			if (accessEntry == null)
			{
				throw new ArgumentNullException("accessEntry");
			}
			int num = this.m_list.IndexOf(accessEntry);
			if (num != -1)
			{
				((KeyContainerPermissionAccessEntry)this.m_list[num]).Flags &= accessEntry.Flags;
				return num;
			}
			if (accessEntry.Flags != this.m_globalFlags)
			{
				return this.m_list.Add(accessEntry);
			}
			return -1;
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x000C6B29 File Offset: 0x000C5B29
		public void Clear()
		{
			this.m_list.Clear();
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x000C6B36 File Offset: 0x000C5B36
		public int IndexOf(KeyContainerPermissionAccessEntry accessEntry)
		{
			return this.m_list.IndexOf(accessEntry);
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x000C6B44 File Offset: 0x000C5B44
		public void Remove(KeyContainerPermissionAccessEntry accessEntry)
		{
			if (accessEntry == null)
			{
				throw new ArgumentNullException("accessEntry");
			}
			this.m_list.Remove(accessEntry);
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x000C6B60 File Offset: 0x000C5B60
		public KeyContainerPermissionAccessEntryEnumerator GetEnumerator()
		{
			return new KeyContainerPermissionAccessEntryEnumerator(this);
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x000C6B68 File Offset: 0x000C5B68
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new KeyContainerPermissionAccessEntryEnumerator(this);
		}

		// Token: 0x06003ADC RID: 15068 RVA: 0x000C6B70 File Offset: 0x000C5B70
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RankMultiDimNotSupported"));
			}
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (index + this.Count > array.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], index);
				index++;
			}
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x000C6C0A File Offset: 0x000C5C0A
		public void CopyTo(KeyContainerPermissionAccessEntry[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06003ADE RID: 15070 RVA: 0x000C6C14 File Offset: 0x000C5C14
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06003ADF RID: 15071 RVA: 0x000C6C17 File Offset: 0x000C5C17
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x04001E7F RID: 7807
		private ArrayList m_list;

		// Token: 0x04001E80 RID: 7808
		private KeyContainerPermissionFlags m_globalFlags;
	}
}

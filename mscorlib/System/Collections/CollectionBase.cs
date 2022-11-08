using System;
using System.Runtime.InteropServices;

namespace System.Collections
{
	// Token: 0x0200025E RID: 606
	[ComVisible(true)]
	[Serializable]
	public abstract class CollectionBase : IList, ICollection, IEnumerable
	{
		// Token: 0x0600179F RID: 6047 RVA: 0x0003CAEC File Offset: 0x0003BAEC
		protected CollectionBase()
		{
			this.list = new ArrayList();
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x0003CAFF File Offset: 0x0003BAFF
		protected CollectionBase(int capacity)
		{
			this.list = new ArrayList(capacity);
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060017A1 RID: 6049 RVA: 0x0003CB13 File Offset: 0x0003BB13
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

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060017A2 RID: 6050 RVA: 0x0003CB2E File Offset: 0x0003BB2E
		protected IList List
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060017A3 RID: 6051 RVA: 0x0003CB31 File Offset: 0x0003BB31
		// (set) Token: 0x060017A4 RID: 6052 RVA: 0x0003CB3E File Offset: 0x0003BB3E
		[ComVisible(false)]
		public int Capacity
		{
			get
			{
				return this.InnerList.Capacity;
			}
			set
			{
				this.InnerList.Capacity = value;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060017A5 RID: 6053 RVA: 0x0003CB4C File Offset: 0x0003BB4C
		public int Count
		{
			get
			{
				if (this.list != null)
				{
					return this.list.Count;
				}
				return 0;
			}
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x0003CB63 File Offset: 0x0003BB63
		public void Clear()
		{
			this.OnClear();
			this.InnerList.Clear();
			this.OnClearComplete();
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x0003CB7C File Offset: 0x0003BB7C
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.InnerList.Count)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			object value = this.InnerList[index];
			this.OnValidate(value);
			this.OnRemove(index, value);
			this.InnerList.RemoveAt(index);
			try
			{
				this.OnRemoveComplete(index, value);
			}
			catch
			{
				this.InnerList.Insert(index, value);
				throw;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060017A8 RID: 6056 RVA: 0x0003CC04 File Offset: 0x0003BC04
		bool IList.IsReadOnly
		{
			get
			{
				return this.InnerList.IsReadOnly;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060017A9 RID: 6057 RVA: 0x0003CC11 File Offset: 0x0003BC11
		bool IList.IsFixedSize
		{
			get
			{
				return this.InnerList.IsFixedSize;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060017AA RID: 6058 RVA: 0x0003CC1E File Offset: 0x0003BC1E
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InnerList.IsSynchronized;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060017AB RID: 6059 RVA: 0x0003CC2B File Offset: 0x0003BC2B
		object ICollection.SyncRoot
		{
			get
			{
				return this.InnerList.SyncRoot;
			}
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x0003CC38 File Offset: 0x0003BC38
		void ICollection.CopyTo(Array array, int index)
		{
			this.InnerList.CopyTo(array, index);
		}

		// Token: 0x17000344 RID: 836
		object IList.this[int index]
		{
			get
			{
				if (index < 0 || index >= this.InnerList.Count)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				return this.InnerList[index];
			}
			set
			{
				if (index < 0 || index >= this.InnerList.Count)
				{
					throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
				}
				this.OnValidate(value);
				object obj = this.InnerList[index];
				this.OnSet(index, obj, value);
				this.InnerList[index] = value;
				try
				{
					this.OnSetComplete(index, obj, value);
				}
				catch
				{
					this.InnerList[index] = obj;
					throw;
				}
			}
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x0003CD08 File Offset: 0x0003BD08
		bool IList.Contains(object value)
		{
			return this.InnerList.Contains(value);
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x0003CD18 File Offset: 0x0003BD18
		int IList.Add(object value)
		{
			this.OnValidate(value);
			this.OnInsert(this.InnerList.Count, value);
			int num = this.InnerList.Add(value);
			try
			{
				this.OnInsertComplete(num, value);
			}
			catch
			{
				this.InnerList.RemoveAt(num);
				throw;
			}
			return num;
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x0003CD78 File Offset: 0x0003BD78
		void IList.Remove(object value)
		{
			this.OnValidate(value);
			int num = this.InnerList.IndexOf(value);
			if (num < 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_RemoveArgNotFound"));
			}
			this.OnRemove(num, value);
			this.InnerList.RemoveAt(num);
			try
			{
				this.OnRemoveComplete(num, value);
			}
			catch
			{
				this.InnerList.Insert(num, value);
				throw;
			}
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x0003CDEC File Offset: 0x0003BDEC
		int IList.IndexOf(object value)
		{
			return this.InnerList.IndexOf(value);
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x0003CDFC File Offset: 0x0003BDFC
		void IList.Insert(int index, object value)
		{
			if (index < 0 || index > this.InnerList.Count)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			this.OnValidate(value);
			this.OnInsert(index, value);
			this.InnerList.Insert(index, value);
			try
			{
				this.OnInsertComplete(index, value);
			}
			catch
			{
				this.InnerList.RemoveAt(index);
				throw;
			}
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x0003CE78 File Offset: 0x0003BE78
		public IEnumerator GetEnumerator()
		{
			return this.InnerList.GetEnumerator();
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x0003CE85 File Offset: 0x0003BE85
		protected virtual void OnSet(int index, object oldValue, object newValue)
		{
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x0003CE87 File Offset: 0x0003BE87
		protected virtual void OnInsert(int index, object value)
		{
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x0003CE89 File Offset: 0x0003BE89
		protected virtual void OnClear()
		{
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x0003CE8B File Offset: 0x0003BE8B
		protected virtual void OnRemove(int index, object value)
		{
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x0003CE8D File Offset: 0x0003BE8D
		protected virtual void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x0003CE9D File Offset: 0x0003BE9D
		protected virtual void OnSetComplete(int index, object oldValue, object newValue)
		{
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x0003CE9F File Offset: 0x0003BE9F
		protected virtual void OnInsertComplete(int index, object value)
		{
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x0003CEA1 File Offset: 0x0003BEA1
		protected virtual void OnClearComplete()
		{
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x0003CEA3 File Offset: 0x0003BEA3
		protected virtual void OnRemoveComplete(int index, object value)
		{
		}

		// Token: 0x04000989 RID: 2441
		private ArrayList list;
	}
}

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000E1 RID: 225
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class EventDescriptorCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x06000781 RID: 1921 RVA: 0x0001B384 File Offset: 0x0001A384
		public EventDescriptorCollection(EventDescriptor[] events)
		{
			this.events = events;
			if (events == null)
			{
				this.events = new EventDescriptor[0];
				this.eventCount = 0;
			}
			else
			{
				this.eventCount = this.events.Length;
			}
			this.eventsOwned = true;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0001B3D2 File Offset: 0x0001A3D2
		public EventDescriptorCollection(EventDescriptor[] events, bool readOnly) : this(events)
		{
			this.readOnly = readOnly;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0001B3E4 File Offset: 0x0001A3E4
		private EventDescriptorCollection(EventDescriptor[] events, int eventCount, string[] namedSort, IComparer comparer)
		{
			this.eventsOwned = false;
			if (namedSort != null)
			{
				this.namedSort = (string[])namedSort.Clone();
			}
			this.comparer = comparer;
			this.events = events;
			this.eventCount = eventCount;
			this.needSort = true;
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x0001B436 File Offset: 0x0001A436
		public int Count
		{
			get
			{
				return this.eventCount;
			}
		}

		// Token: 0x17000188 RID: 392
		public virtual EventDescriptor this[int index]
		{
			get
			{
				if (index >= this.eventCount)
				{
					throw new IndexOutOfRangeException();
				}
				this.EnsureEventsOwned();
				return this.events[index];
			}
		}

		// Token: 0x17000189 RID: 393
		public virtual EventDescriptor this[string name]
		{
			get
			{
				return this.Find(name, false);
			}
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0001B468 File Offset: 0x0001A468
		public int Add(EventDescriptor value)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			this.EnsureSize(this.eventCount + 1);
			this.events[this.eventCount++] = value;
			return this.eventCount - 1;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0001B4B2 File Offset: 0x0001A4B2
		public void Clear()
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			this.eventCount = 0;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0001B4C9 File Offset: 0x0001A4C9
		public bool Contains(EventDescriptor value)
		{
			return this.IndexOf(value) >= 0;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0001B4D8 File Offset: 0x0001A4D8
		void ICollection.CopyTo(Array array, int index)
		{
			this.EnsureEventsOwned();
			Array.Copy(this.events, 0, array, index, this.Count);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0001B4F4 File Offset: 0x0001A4F4
		private void EnsureEventsOwned()
		{
			if (!this.eventsOwned)
			{
				this.eventsOwned = true;
				if (this.events != null)
				{
					EventDescriptor[] destinationArray = new EventDescriptor[this.Count];
					Array.Copy(this.events, 0, destinationArray, 0, this.Count);
					this.events = destinationArray;
				}
			}
			if (this.needSort)
			{
				this.needSort = false;
				this.InternalSort(this.namedSort);
			}
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0001B55C File Offset: 0x0001A55C
		private void EnsureSize(int sizeNeeded)
		{
			if (sizeNeeded <= this.events.Length)
			{
				return;
			}
			if (this.events == null || this.events.Length == 0)
			{
				this.eventCount = 0;
				this.events = new EventDescriptor[sizeNeeded];
				return;
			}
			this.EnsureEventsOwned();
			int num = Math.Max(sizeNeeded, this.events.Length * 2);
			EventDescriptor[] destinationArray = new EventDescriptor[num];
			Array.Copy(this.events, 0, destinationArray, 0, this.eventCount);
			this.events = destinationArray;
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0001B5D4 File Offset: 0x0001A5D4
		public virtual EventDescriptor Find(string name, bool ignoreCase)
		{
			EventDescriptor result = null;
			if (ignoreCase)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (string.Equals(this.events[i].Name, name, StringComparison.OrdinalIgnoreCase))
					{
						result = this.events[i];
						break;
					}
				}
			}
			else
			{
				for (int j = 0; j < this.Count; j++)
				{
					if (string.Equals(this.events[j].Name, name, StringComparison.Ordinal))
					{
						result = this.events[j];
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0001B64D File Offset: 0x0001A64D
		public int IndexOf(EventDescriptor value)
		{
			return Array.IndexOf<EventDescriptor>(this.events, value, 0, this.eventCount);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0001B664 File Offset: 0x0001A664
		public void Insert(int index, EventDescriptor value)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			this.EnsureSize(this.eventCount + 1);
			if (index < this.eventCount)
			{
				Array.Copy(this.events, index, this.events, index + 1, this.eventCount - index);
			}
			this.events[index] = value;
			this.eventCount++;
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0001B6CC File Offset: 0x0001A6CC
		public void Remove(EventDescriptor value)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			int num = this.IndexOf(value);
			if (num != -1)
			{
				this.RemoveAt(num);
			}
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0001B6FC File Offset: 0x0001A6FC
		public void RemoveAt(int index)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			if (index < this.eventCount - 1)
			{
				Array.Copy(this.events, index + 1, this.events, index, this.eventCount - index - 1);
			}
			this.events[this.eventCount - 1] = null;
			this.eventCount--;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0001B75F File Offset: 0x0001A75F
		public IEnumerator GetEnumerator()
		{
			if (this.events.Length == this.eventCount)
			{
				return this.events.GetEnumerator();
			}
			return new ArraySubsetEnumerator(this.events, this.eventCount);
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0001B78E File Offset: 0x0001A78E
		public virtual EventDescriptorCollection Sort()
		{
			return new EventDescriptorCollection(this.events, this.eventCount, this.namedSort, this.comparer);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0001B7AD File Offset: 0x0001A7AD
		public virtual EventDescriptorCollection Sort(string[] names)
		{
			return new EventDescriptorCollection(this.events, this.eventCount, names, this.comparer);
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0001B7C7 File Offset: 0x0001A7C7
		public virtual EventDescriptorCollection Sort(string[] names, IComparer comparer)
		{
			return new EventDescriptorCollection(this.events, this.eventCount, names, comparer);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0001B7DC File Offset: 0x0001A7DC
		public virtual EventDescriptorCollection Sort(IComparer comparer)
		{
			return new EventDescriptorCollection(this.events, this.eventCount, this.namedSort, comparer);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0001B7F8 File Offset: 0x0001A7F8
		protected void InternalSort(string[] names)
		{
			if (this.events == null || this.events.Length == 0)
			{
				return;
			}
			this.InternalSort(this.comparer);
			if (names != null && names.Length > 0)
			{
				ArrayList arrayList = new ArrayList(this.events);
				int num = 0;
				int num2 = this.events.Length;
				for (int i = 0; i < names.Length; i++)
				{
					for (int j = 0; j < num2; j++)
					{
						EventDescriptor eventDescriptor = (EventDescriptor)arrayList[j];
						if (eventDescriptor != null && eventDescriptor.Name.Equals(names[i]))
						{
							this.events[num++] = eventDescriptor;
							arrayList[j] = null;
							break;
						}
					}
				}
				for (int k = 0; k < num2; k++)
				{
					if (arrayList[k] != null)
					{
						this.events[num++] = (EventDescriptor)arrayList[k];
					}
				}
			}
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0001B8D8 File Offset: 0x0001A8D8
		protected void InternalSort(IComparer sorter)
		{
			if (sorter == null)
			{
				TypeDescriptor.SortDescriptorArray(this);
				return;
			}
			Array.Sort(this.events, sorter);
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x0001B8F0 File Offset: 0x0001A8F0
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x0001B8F8 File Offset: 0x0001A8F8
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x0001B8FB File Offset: 0x0001A8FB
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0001B8FE File Offset: 0x0001A8FE
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x1700018D RID: 397
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (this.readOnly)
				{
					throw new NotSupportedException();
				}
				if (index >= this.eventCount)
				{
					throw new IndexOutOfRangeException();
				}
				this.EnsureEventsOwned();
				this.events[index] = (EventDescriptor)value;
			}
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0001B942 File Offset: 0x0001A942
		int IList.Add(object value)
		{
			return this.Add((EventDescriptor)value);
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0001B950 File Offset: 0x0001A950
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0001B958 File Offset: 0x0001A958
		bool IList.Contains(object value)
		{
			return this.Contains((EventDescriptor)value);
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0001B966 File Offset: 0x0001A966
		int IList.IndexOf(object value)
		{
			return this.IndexOf((EventDescriptor)value);
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0001B974 File Offset: 0x0001A974
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (EventDescriptor)value);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0001B983 File Offset: 0x0001A983
		void IList.Remove(object value)
		{
			this.Remove((EventDescriptor)value);
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0001B991 File Offset: 0x0001A991
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0001B99A File Offset: 0x0001A99A
		bool IList.IsReadOnly
		{
			get
			{
				return this.readOnly;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0001B9A2 File Offset: 0x0001A9A2
		bool IList.IsFixedSize
		{
			get
			{
				return this.readOnly;
			}
		}

		// Token: 0x04000969 RID: 2409
		private EventDescriptor[] events;

		// Token: 0x0400096A RID: 2410
		private string[] namedSort;

		// Token: 0x0400096B RID: 2411
		private IComparer comparer;

		// Token: 0x0400096C RID: 2412
		private bool eventsOwned = true;

		// Token: 0x0400096D RID: 2413
		private bool needSort;

		// Token: 0x0400096E RID: 2414
		private int eventCount;

		// Token: 0x0400096F RID: 2415
		private bool readOnly;

		// Token: 0x04000970 RID: 2416
		public static readonly EventDescriptorCollection Empty = new EventDescriptorCollection(null, true);
	}
}

using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000368 RID: 872
	[Serializable]
	internal class LongList
	{
		// Token: 0x0600226C RID: 8812 RVA: 0x00056DFE File Offset: 0x00055DFE
		internal LongList() : this(2)
		{
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x00056E07 File Offset: 0x00055E07
		internal LongList(int startingSize)
		{
			this.m_count = 0;
			this.m_totalItems = 0;
			this.m_values = new long[startingSize];
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x00056E2C File Offset: 0x00055E2C
		internal void Add(long value)
		{
			if (this.m_totalItems == this.m_values.Length)
			{
				this.EnlargeArray();
			}
			this.m_values[this.m_totalItems++] = value;
			this.m_count++;
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x0600226F RID: 8815 RVA: 0x00056E76 File Offset: 0x00055E76
		internal int Count
		{
			get
			{
				return this.m_count;
			}
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x00056E7E File Offset: 0x00055E7E
		internal void StartEnumeration()
		{
			this.m_currentItem = -1;
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x00056E88 File Offset: 0x00055E88
		internal bool MoveNext()
		{
			while (++this.m_currentItem < this.m_totalItems && this.m_values[this.m_currentItem] == -1L)
			{
			}
			return this.m_currentItem != this.m_totalItems;
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06002272 RID: 8818 RVA: 0x00056ED0 File Offset: 0x00055ED0
		internal long Current
		{
			get
			{
				return this.m_values[this.m_currentItem];
			}
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x00056EE0 File Offset: 0x00055EE0
		internal bool RemoveElement(long value)
		{
			int num = 0;
			while (num < this.m_totalItems && this.m_values[num] != value)
			{
				num++;
			}
			if (num == this.m_totalItems)
			{
				return false;
			}
			this.m_values[num] = -1L;
			return true;
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x00056F20 File Offset: 0x00055F20
		private void EnlargeArray()
		{
			int num = this.m_values.Length * 2;
			if (num < 0)
			{
				if (num == 2147483647)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_TooManyElements"));
				}
				num = int.MaxValue;
			}
			long[] array = new long[num];
			Array.Copy(this.m_values, array, this.m_count);
			this.m_values = array;
		}

		// Token: 0x04000E7B RID: 3707
		private const int InitialSize = 2;

		// Token: 0x04000E7C RID: 3708
		private long[] m_values;

		// Token: 0x04000E7D RID: 3709
		private int m_count;

		// Token: 0x04000E7E RID: 3710
		private int m_totalItems;

		// Token: 0x04000E7F RID: 3711
		private int m_currentItem;
	}
}

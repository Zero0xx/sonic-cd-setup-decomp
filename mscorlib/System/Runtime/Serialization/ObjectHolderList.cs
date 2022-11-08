using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000369 RID: 873
	internal class ObjectHolderList
	{
		// Token: 0x06002275 RID: 8821 RVA: 0x00056F7A File Offset: 0x00055F7A
		internal ObjectHolderList() : this(8)
		{
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x00056F83 File Offset: 0x00055F83
		internal ObjectHolderList(int startingSize)
		{
			this.m_count = 0;
			this.m_values = new ObjectHolder[startingSize];
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x00056FA0 File Offset: 0x00055FA0
		internal virtual void Add(ObjectHolder value)
		{
			if (this.m_count == this.m_values.Length)
			{
				this.EnlargeArray();
			}
			this.m_values[this.m_count++] = value;
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x00056FDC File Offset: 0x00055FDC
		internal ObjectHolderListEnumerator GetFixupEnumerator()
		{
			return new ObjectHolderListEnumerator(this, true);
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x00056FE8 File Offset: 0x00055FE8
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
			ObjectHolder[] array = new ObjectHolder[num];
			Array.Copy(this.m_values, array, this.m_count);
			this.m_values = array;
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x0600227A RID: 8826 RVA: 0x00057042 File Offset: 0x00056042
		internal int Version
		{
			get
			{
				return this.m_count;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x0600227B RID: 8827 RVA: 0x0005704A File Offset: 0x0005604A
		internal int Count
		{
			get
			{
				return this.m_count;
			}
		}

		// Token: 0x04000E80 RID: 3712
		internal const int DefaultInitialSize = 8;

		// Token: 0x04000E81 RID: 3713
		internal ObjectHolder[] m_values;

		// Token: 0x04000E82 RID: 3714
		internal int m_count;
	}
}

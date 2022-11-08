using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000367 RID: 871
	[Serializable]
	internal class FixupHolderList
	{
		// Token: 0x06002267 RID: 8807 RVA: 0x00056CED File Offset: 0x00055CED
		internal FixupHolderList() : this(2)
		{
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x00056CF6 File Offset: 0x00055CF6
		internal FixupHolderList(int startingSize)
		{
			this.m_count = 0;
			this.m_values = new FixupHolder[startingSize];
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x00056D14 File Offset: 0x00055D14
		internal virtual void Add(long id, object fixupInfo)
		{
			if (this.m_count == this.m_values.Length)
			{
				this.EnlargeArray();
			}
			this.m_values[this.m_count].m_id = id;
			this.m_values[this.m_count++].m_fixupInfo = fixupInfo;
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x00056D68 File Offset: 0x00055D68
		internal virtual void Add(FixupHolder fixup)
		{
			if (this.m_count == this.m_values.Length)
			{
				this.EnlargeArray();
			}
			this.m_values[this.m_count++] = fixup;
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x00056DA4 File Offset: 0x00055DA4
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
			FixupHolder[] array = new FixupHolder[num];
			Array.Copy(this.m_values, array, this.m_count);
			this.m_values = array;
		}

		// Token: 0x04000E78 RID: 3704
		internal const int InitialSize = 2;

		// Token: 0x04000E79 RID: 3705
		internal FixupHolder[] m_values;

		// Token: 0x04000E7A RID: 3706
		internal int m_count;
	}
}

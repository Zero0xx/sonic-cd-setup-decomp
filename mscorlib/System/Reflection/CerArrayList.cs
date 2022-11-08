using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace System.Reflection
{
	// Token: 0x02000103 RID: 259
	[Serializable]
	internal sealed class CerArrayList<V>
	{
		// Token: 0x06000EAA RID: 3754 RVA: 0x0002BD54 File Offset: 0x0002AD54
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal CerArrayList(List<V> list)
		{
			this.m_array = new V[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				this.m_array[i] = list[i];
			}
			this.m_count = list.Count;
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0002BDA8 File Offset: 0x0002ADA8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal CerArrayList(int length)
		{
			if (length < 4)
			{
				length = 4;
			}
			this.m_array = new V[length];
			this.m_count = 0;
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000EAC RID: 3756 RVA: 0x0002BDCA File Offset: 0x0002ADCA
		internal int Count
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.m_count;
			}
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0002BDD4 File Offset: 0x0002ADD4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal void Preallocate(int addition)
		{
			if (this.m_array.Length - this.m_count > addition)
			{
				return;
			}
			int num = (this.m_array.Length * 2 > this.m_array.Length + addition) ? (this.m_array.Length * 2) : (this.m_array.Length + addition);
			V[] array = new V[num];
			for (int i = 0; i < this.m_count; i++)
			{
				array[i] = this.m_array[i];
			}
			this.m_array = array;
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x0002BE53 File Offset: 0x0002AE53
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Add(V value)
		{
			this.m_array[this.m_count] = value;
			this.m_count++;
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0002BE75 File Offset: 0x0002AE75
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Replace(int index, V value)
		{
			if (index >= this.m_count)
			{
				throw new InvalidOperationException();
			}
			this.m_array[index] = value;
		}

		// Token: 0x170001D5 RID: 469
		internal V this[int index]
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.m_array[index];
			}
		}

		// Token: 0x0400052B RID: 1323
		private const int MinSize = 4;

		// Token: 0x0400052C RID: 1324
		private V[] m_array;

		// Token: 0x0400052D RID: 1325
		private int m_count;
	}
}

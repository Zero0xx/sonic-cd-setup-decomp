using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace System.Reflection
{
	// Token: 0x02000104 RID: 260
	[Serializable]
	internal sealed class CerHashtable<K, V>
	{
		// Token: 0x06000EB1 RID: 3761 RVA: 0x0002BEA1 File Offset: 0x0002AEA1
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal CerHashtable() : this(7)
		{
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0002BEAA File Offset: 0x0002AEAA
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal CerHashtable(int size)
		{
			size = HashHelpers.GetPrime(size);
			this.m_key = new K[size];
			this.m_value = new V[size];
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0002BED4 File Offset: 0x0002AED4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal void Preallocate(int count)
		{
			bool flag = false;
			bool flag2 = false;
			K[] array = null;
			V[] array2 = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(this, ref flag);
				int num = (count + this.m_count) * 2;
				if (num >= this.m_value.Length)
				{
					num = HashHelpers.GetPrime(num);
					array = new K[num];
					array2 = new V[num];
					for (int i = 0; i < this.m_key.Length; i++)
					{
						K k = this.m_key[i];
						if (k != null)
						{
							int num2 = 0;
							CerHashtable<K, V>.Insert(array, array2, ref num2, k, this.m_value[i]);
						}
					}
					flag2 = true;
				}
			}
			finally
			{
				if (flag2)
				{
					this.m_key = array;
					this.m_value = array2;
				}
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x0002BFA8 File Offset: 0x0002AFA8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private static void Insert(K[] keys, V[] values, ref int count, K key, V value)
		{
			int num = key.GetHashCode();
			if (num < 0)
			{
				num = -num;
			}
			int num2 = num % keys.Length;
			int num3 = num2;
			K k;
			for (;;)
			{
				k = keys[num3];
				if (k == null)
				{
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						return;
					}
					finally
					{
						keys[num3] = key;
						values[num3] = value;
						count++;
					}
				}
				if (k.Equals(key))
				{
					break;
				}
				num3++;
				num3 %= keys.Length;
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_AddingDuplicate__", new object[]
			{
				k,
				key
			}));
		}

		// Token: 0x170001D6 RID: 470
		internal V this[K key]
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			get
			{
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				V result;
				try
				{
					Monitor.ReliableEnter(this, ref flag);
					int num = key.GetHashCode();
					if (num < 0)
					{
						num = -num;
					}
					int num2 = num % this.m_key.Length;
					int num3 = num2;
					for (;;)
					{
						K k = this.m_key[num3];
						if (k == null)
						{
							goto IL_7E;
						}
						if (k.Equals(key))
						{
							break;
						}
						num3++;
						num3 %= this.m_key.Length;
					}
					return this.m_value[num3];
					IL_7E:
					result = default(V);
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
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			set
			{
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.ReliableEnter(this, ref flag);
					CerHashtable<K, V>.Insert(this.m_key, this.m_value, ref this.m_count, key, value);
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this);
					}
				}
			}
		}

		// Token: 0x0400052E RID: 1326
		private const int MinSize = 7;

		// Token: 0x0400052F RID: 1327
		private K[] m_key;

		// Token: 0x04000530 RID: 1328
		private V[] m_value;

		// Token: 0x04000531 RID: 1329
		private int m_count;
	}
}

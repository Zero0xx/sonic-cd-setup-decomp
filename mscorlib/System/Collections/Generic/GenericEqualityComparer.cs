using System;

namespace System.Collections.Generic
{
	// Token: 0x02000294 RID: 660
	[Serializable]
	internal class GenericEqualityComparer<T> : EqualityComparer<T> where T : IEquatable<T>
	{
		// Token: 0x06001A01 RID: 6657 RVA: 0x00043E4C File Offset: 0x00042E4C
		public override bool Equals(T x, T y)
		{
			if (x != null)
			{
				return y != null && x.Equals(y);
			}
			return y == null;
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x00043E7A File Offset: 0x00042E7A
		public override int GetHashCode(T obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetHashCode();
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x00043E94 File Offset: 0x00042E94
		internal override int IndexOf(T[] array, T value, int startIndex, int count)
		{
			int num = startIndex + count;
			if (value == null)
			{
				for (int i = startIndex; i < num; i++)
				{
					if (array[i] == null)
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = startIndex; j < num; j++)
				{
					if (array[j] != null && array[j].Equals(value))
					{
						return j;
					}
				}
			}
			return -1;
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x00043F00 File Offset: 0x00042F00
		internal override int LastIndexOf(T[] array, T value, int startIndex, int count)
		{
			int num = startIndex - count + 1;
			if (value == null)
			{
				for (int i = startIndex; i >= num; i--)
				{
					if (array[i] == null)
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = startIndex; j >= num; j--)
				{
					if (array[j] != null && array[j].Equals(value))
					{
						return j;
					}
				}
			}
			return -1;
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x00043F70 File Offset: 0x00042F70
		public override bool Equals(object obj)
		{
			GenericEqualityComparer<T> genericEqualityComparer = obj as GenericEqualityComparer<T>;
			return genericEqualityComparer != null;
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x00043F8B File Offset: 0x00042F8B
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}

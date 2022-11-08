using System;

namespace System.Collections.Generic
{
	// Token: 0x02000295 RID: 661
	[Serializable]
	internal class NullableEqualityComparer<T> : EqualityComparer<T?> where T : struct, IEquatable<T>
	{
		// Token: 0x06001A08 RID: 6664 RVA: 0x00043FA5 File Offset: 0x00042FA5
		public override bool Equals(T? x, T? y)
		{
			if (x != null)
			{
				return y != null && x.value.Equals(y.value);
			}
			return y == null;
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x00043FE1 File Offset: 0x00042FE1
		public override int GetHashCode(T? obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x00043FF0 File Offset: 0x00042FF0
		internal override int IndexOf(T?[] array, T? value, int startIndex, int count)
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
					if (array[j] != null && array[j].value.Equals(value.value))
					{
						return j;
					}
				}
			}
			return -1;
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x00044068 File Offset: 0x00043068
		internal override int LastIndexOf(T?[] array, T? value, int startIndex, int count)
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
					if (array[j] != null && array[j].value.Equals(value.value))
					{
						return j;
					}
				}
			}
			return -1;
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x000440E0 File Offset: 0x000430E0
		public override bool Equals(object obj)
		{
			NullableEqualityComparer<T> nullableEqualityComparer = obj as NullableEqualityComparer<T>;
			return nullableEqualityComparer != null;
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x000440FB File Offset: 0x000430FB
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}

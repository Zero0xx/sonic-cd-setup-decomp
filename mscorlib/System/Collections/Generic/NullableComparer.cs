using System;

namespace System.Collections.Generic
{
	// Token: 0x02000288 RID: 648
	[Serializable]
	internal class NullableComparer<T> : Comparer<T?> where T : struct, IComparable<T>
	{
		// Token: 0x0600197C RID: 6524 RVA: 0x000424C9 File Offset: 0x000414C9
		public override int Compare(T? x, T? y)
		{
			if (x != null)
			{
				if (y != null)
				{
					return x.value.CompareTo(y.value);
				}
				return 1;
			}
			else
			{
				if (y != null)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x00042508 File Offset: 0x00041508
		public override bool Equals(object obj)
		{
			NullableComparer<T> nullableComparer = obj as NullableComparer<T>;
			return nullableComparer != null;
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x00042523 File Offset: 0x00041523
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}

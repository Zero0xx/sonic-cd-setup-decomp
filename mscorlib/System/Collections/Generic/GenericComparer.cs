using System;

namespace System.Collections.Generic
{
	// Token: 0x02000287 RID: 647
	[Serializable]
	internal class GenericComparer<T> : Comparer<T> where T : IComparable<T>
	{
		// Token: 0x06001978 RID: 6520 RVA: 0x00042465 File Offset: 0x00041465
		public override int Compare(T x, T y)
		{
			if (x != null)
			{
				if (y != null)
				{
					return x.CompareTo(y);
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

		// Token: 0x06001979 RID: 6521 RVA: 0x00042494 File Offset: 0x00041494
		public override bool Equals(object obj)
		{
			GenericComparer<T> genericComparer = obj as GenericComparer<T>;
			return genericComparer != null;
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x000424AF File Offset: 0x000414AF
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}

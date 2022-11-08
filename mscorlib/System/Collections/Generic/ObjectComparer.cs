using System;

namespace System.Collections.Generic
{
	// Token: 0x02000289 RID: 649
	[Serializable]
	internal class ObjectComparer<T> : Comparer<T>
	{
		// Token: 0x06001980 RID: 6528 RVA: 0x0004253D File Offset: 0x0004153D
		public override int Compare(T x, T y)
		{
			return Comparer.Default.Compare(x, y);
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x00042558 File Offset: 0x00041558
		public override bool Equals(object obj)
		{
			ObjectComparer<T> objectComparer = obj as ObjectComparer<T>;
			return objectComparer != null;
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x00042573 File Offset: 0x00041573
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}

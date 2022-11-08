using System;

namespace System.Collections
{
	// Token: 0x02000274 RID: 628
	[Serializable]
	internal class CompatibleComparer : IEqualityComparer
	{
		// Token: 0x0600189B RID: 6299 RVA: 0x0003FA0E File Offset: 0x0003EA0E
		internal CompatibleComparer(IComparer comparer, IHashCodeProvider hashCodeProvider)
		{
			this._comparer = comparer;
			this._hcp = hashCodeProvider;
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x0003FA24 File Offset: 0x0003EA24
		public int Compare(object a, object b)
		{
			if (a == b)
			{
				return 0;
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			if (this._comparer != null)
			{
				return this._comparer.Compare(a, b);
			}
			IComparable comparable = a as IComparable;
			if (comparable != null)
			{
				return comparable.CompareTo(b);
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_ImplementIComparable"));
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x0003FA78 File Offset: 0x0003EA78
		public bool Equals(object a, object b)
		{
			return this.Compare(a, b) == 0;
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x0003FA85 File Offset: 0x0003EA85
		public int GetHashCode(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (this._hcp != null)
			{
				return this._hcp.GetHashCode(obj);
			}
			return obj.GetHashCode();
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x0600189F RID: 6303 RVA: 0x0003FAB0 File Offset: 0x0003EAB0
		internal IComparer Comparer
		{
			get
			{
				return this._comparer;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060018A0 RID: 6304 RVA: 0x0003FAB8 File Offset: 0x0003EAB8
		internal IHashCodeProvider HashCodeProvider
		{
			get
			{
				return this._hcp;
			}
		}

		// Token: 0x040009CB RID: 2507
		private IComparer _comparer;

		// Token: 0x040009CC RID: 2508
		private IHashCodeProvider _hcp;
	}
}

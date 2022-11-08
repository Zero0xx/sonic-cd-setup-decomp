using System;

namespace System.ComponentModel
{
	// Token: 0x020000AC RID: 172
	[AttributeUsage(AttributeTargets.All)]
	public sealed class BrowsableAttribute : Attribute
	{
		// Token: 0x0600063D RID: 1597 RVA: 0x0001842B File Offset: 0x0001742B
		public BrowsableAttribute(bool browsable)
		{
			this.browsable = browsable;
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x00018441 File Offset: 0x00017441
		public bool Browsable
		{
			get
			{
				return this.browsable;
			}
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0001844C File Offset: 0x0001744C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			BrowsableAttribute browsableAttribute = obj as BrowsableAttribute;
			return browsableAttribute != null && browsableAttribute.Browsable == this.browsable;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00018479 File Offset: 0x00017479
		public override int GetHashCode()
		{
			return this.browsable.GetHashCode();
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00018486 File Offset: 0x00017486
		public override bool IsDefaultAttribute()
		{
			return this.Equals(BrowsableAttribute.Default);
		}

		// Token: 0x04000907 RID: 2311
		public static readonly BrowsableAttribute Yes = new BrowsableAttribute(true);

		// Token: 0x04000908 RID: 2312
		public static readonly BrowsableAttribute No = new BrowsableAttribute(false);

		// Token: 0x04000909 RID: 2313
		public static readonly BrowsableAttribute Default = BrowsableAttribute.Yes;

		// Token: 0x0400090A RID: 2314
		private bool browsable = true;
	}
}

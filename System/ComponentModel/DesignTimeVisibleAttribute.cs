using System;

namespace System.ComponentModel
{
	// Token: 0x020000D7 RID: 215
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class DesignTimeVisibleAttribute : Attribute
	{
		// Token: 0x0600073F RID: 1855 RVA: 0x0001AA04 File Offset: 0x00019A04
		public DesignTimeVisibleAttribute(bool visible)
		{
			this.visible = visible;
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0001AA13 File Offset: 0x00019A13
		public DesignTimeVisibleAttribute()
		{
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x0001AA1B File Offset: 0x00019A1B
		public bool Visible
		{
			get
			{
				return this.visible;
			}
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0001AA24 File Offset: 0x00019A24
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DesignTimeVisibleAttribute designTimeVisibleAttribute = obj as DesignTimeVisibleAttribute;
			return designTimeVisibleAttribute != null && designTimeVisibleAttribute.Visible == this.visible;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001AA51 File Offset: 0x00019A51
		public override int GetHashCode()
		{
			return typeof(DesignTimeVisibleAttribute).GetHashCode() ^ (this.visible ? -1 : 0);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0001AA6F File Offset: 0x00019A6F
		public override bool IsDefaultAttribute()
		{
			return this.Visible == DesignTimeVisibleAttribute.Default.Visible;
		}

		// Token: 0x04000957 RID: 2391
		private bool visible;

		// Token: 0x04000958 RID: 2392
		public static readonly DesignTimeVisibleAttribute Yes = new DesignTimeVisibleAttribute(true);

		// Token: 0x04000959 RID: 2393
		public static readonly DesignTimeVisibleAttribute No = new DesignTimeVisibleAttribute(false);

		// Token: 0x0400095A RID: 2394
		public static readonly DesignTimeVisibleAttribute Default = DesignTimeVisibleAttribute.Yes;
	}
}

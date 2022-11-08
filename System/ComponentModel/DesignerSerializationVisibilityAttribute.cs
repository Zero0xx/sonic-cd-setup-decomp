using System;

namespace System.ComponentModel
{
	// Token: 0x020000D5 RID: 213
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event)]
	public sealed class DesignerSerializationVisibilityAttribute : Attribute
	{
		// Token: 0x06000733 RID: 1843 RVA: 0x0001A8F5 File Offset: 0x000198F5
		public DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility visibility)
		{
			this.visibility = visibility;
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x0001A904 File Offset: 0x00019904
		public DesignerSerializationVisibility Visibility
		{
			get
			{
				return this.visibility;
			}
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0001A90C File Offset: 0x0001990C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DesignerSerializationVisibilityAttribute designerSerializationVisibilityAttribute = obj as DesignerSerializationVisibilityAttribute;
			return designerSerializationVisibilityAttribute != null && designerSerializationVisibilityAttribute.Visibility == this.visibility;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0001A939 File Offset: 0x00019939
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0001A941 File Offset: 0x00019941
		public override bool IsDefaultAttribute()
		{
			return this.Equals(DesignerSerializationVisibilityAttribute.Default);
		}

		// Token: 0x0400094E RID: 2382
		public static readonly DesignerSerializationVisibilityAttribute Content = new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content);

		// Token: 0x0400094F RID: 2383
		public static readonly DesignerSerializationVisibilityAttribute Hidden = new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden);

		// Token: 0x04000950 RID: 2384
		public static readonly DesignerSerializationVisibilityAttribute Visible = new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible);

		// Token: 0x04000951 RID: 2385
		public static readonly DesignerSerializationVisibilityAttribute Default = DesignerSerializationVisibilityAttribute.Visible;

		// Token: 0x04000952 RID: 2386
		private DesignerSerializationVisibility visibility;
	}
}

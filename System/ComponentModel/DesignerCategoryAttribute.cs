using System;

namespace System.ComponentModel
{
	// Token: 0x020000D3 RID: 211
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class DesignerCategoryAttribute : Attribute
	{
		// Token: 0x0600072B RID: 1835 RVA: 0x0001A810 File Offset: 0x00019810
		public DesignerCategoryAttribute()
		{
			this.category = string.Empty;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0001A823 File Offset: 0x00019823
		public DesignerCategoryAttribute(string category)
		{
			this.category = category;
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x0001A832 File Offset: 0x00019832
		public string Category
		{
			get
			{
				return this.category;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x0001A83A File Offset: 0x0001983A
		public override object TypeId
		{
			get
			{
				if (this.typeId == null)
				{
					this.typeId = base.GetType().FullName + this.Category;
				}
				return this.typeId;
			}
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0001A868 File Offset: 0x00019868
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DesignerCategoryAttribute designerCategoryAttribute = obj as DesignerCategoryAttribute;
			return designerCategoryAttribute != null && designerCategoryAttribute.category == this.category;
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0001A898 File Offset: 0x00019898
		public override int GetHashCode()
		{
			return this.category.GetHashCode();
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0001A8A5 File Offset: 0x000198A5
		public override bool IsDefaultAttribute()
		{
			return this.category.Equals(DesignerCategoryAttribute.Default.Category);
		}

		// Token: 0x04000944 RID: 2372
		private string category;

		// Token: 0x04000945 RID: 2373
		private string typeId;

		// Token: 0x04000946 RID: 2374
		public static readonly DesignerCategoryAttribute Component = new DesignerCategoryAttribute("Component");

		// Token: 0x04000947 RID: 2375
		public static readonly DesignerCategoryAttribute Default = new DesignerCategoryAttribute();

		// Token: 0x04000948 RID: 2376
		public static readonly DesignerCategoryAttribute Form = new DesignerCategoryAttribute("Form");

		// Token: 0x04000949 RID: 2377
		public static readonly DesignerCategoryAttribute Generic = new DesignerCategoryAttribute("Designer");
	}
}

using System;

namespace System.ComponentModel
{
	// Token: 0x02000118 RID: 280
	[AttributeUsage(AttributeTargets.All)]
	public sealed class LocalizableAttribute : Attribute
	{
		// Token: 0x060008A4 RID: 2212 RVA: 0x0001CED6 File Offset: 0x0001BED6
		public LocalizableAttribute(bool isLocalizable)
		{
			this.isLocalizable = isLocalizable;
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x0001CEE5 File Offset: 0x0001BEE5
		public bool IsLocalizable
		{
			get
			{
				return this.isLocalizable;
			}
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0001CEED File Offset: 0x0001BEED
		public override bool IsDefaultAttribute()
		{
			return this.IsLocalizable == LocalizableAttribute.Default.IsLocalizable;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0001CF04 File Offset: 0x0001BF04
		public override bool Equals(object obj)
		{
			LocalizableAttribute localizableAttribute = obj as LocalizableAttribute;
			return localizableAttribute != null && localizableAttribute.IsLocalizable == this.isLocalizable;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0001CF2B File Offset: 0x0001BF2B
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040009B6 RID: 2486
		private bool isLocalizable;

		// Token: 0x040009B7 RID: 2487
		public static readonly LocalizableAttribute Yes = new LocalizableAttribute(true);

		// Token: 0x040009B8 RID: 2488
		public static readonly LocalizableAttribute No = new LocalizableAttribute(false);

		// Token: 0x040009B9 RID: 2489
		public static readonly LocalizableAttribute Default = LocalizableAttribute.No;
	}
}

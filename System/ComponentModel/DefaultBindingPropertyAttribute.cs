using System;

namespace System.ComponentModel
{
	// Token: 0x020000CB RID: 203
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DefaultBindingPropertyAttribute : Attribute
	{
		// Token: 0x060006ED RID: 1773 RVA: 0x0001A259 File Offset: 0x00019259
		public DefaultBindingPropertyAttribute()
		{
			this.name = null;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0001A268 File Offset: 0x00019268
		public DefaultBindingPropertyAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x0001A277 File Offset: 0x00019277
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0001A280 File Offset: 0x00019280
		public override bool Equals(object obj)
		{
			DefaultBindingPropertyAttribute defaultBindingPropertyAttribute = obj as DefaultBindingPropertyAttribute;
			return defaultBindingPropertyAttribute != null && defaultBindingPropertyAttribute.Name == this.name;
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001A2AA File Offset: 0x000192AA
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000937 RID: 2359
		private readonly string name;

		// Token: 0x04000938 RID: 2360
		public static readonly DefaultBindingPropertyAttribute Default = new DefaultBindingPropertyAttribute();
	}
}

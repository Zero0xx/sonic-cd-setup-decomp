using System;

namespace System.ComponentModel
{
	// Token: 0x02000119 RID: 281
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class LookupBindingPropertiesAttribute : Attribute
	{
		// Token: 0x060008AA RID: 2218 RVA: 0x0001CF55 File Offset: 0x0001BF55
		public LookupBindingPropertiesAttribute()
		{
			this.dataSource = null;
			this.displayMember = null;
			this.valueMember = null;
			this.lookupMember = null;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0001CF79 File Offset: 0x0001BF79
		public LookupBindingPropertiesAttribute(string dataSource, string displayMember, string valueMember, string lookupMember)
		{
			this.dataSource = dataSource;
			this.displayMember = displayMember;
			this.valueMember = valueMember;
			this.lookupMember = lookupMember;
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x0001CF9E File Offset: 0x0001BF9E
		public string DataSource
		{
			get
			{
				return this.dataSource;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x0001CFA6 File Offset: 0x0001BFA6
		public string DisplayMember
		{
			get
			{
				return this.displayMember;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x0001CFAE File Offset: 0x0001BFAE
		public string ValueMember
		{
			get
			{
				return this.valueMember;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x0001CFB6 File Offset: 0x0001BFB6
		public string LookupMember
		{
			get
			{
				return this.lookupMember;
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0001CFC0 File Offset: 0x0001BFC0
		public override bool Equals(object obj)
		{
			LookupBindingPropertiesAttribute lookupBindingPropertiesAttribute = obj as LookupBindingPropertiesAttribute;
			return lookupBindingPropertiesAttribute != null && lookupBindingPropertiesAttribute.DataSource == this.dataSource && lookupBindingPropertiesAttribute.displayMember == this.displayMember && lookupBindingPropertiesAttribute.valueMember == this.valueMember && lookupBindingPropertiesAttribute.lookupMember == this.lookupMember;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0001D023 File Offset: 0x0001C023
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040009BA RID: 2490
		private readonly string dataSource;

		// Token: 0x040009BB RID: 2491
		private readonly string displayMember;

		// Token: 0x040009BC RID: 2492
		private readonly string valueMember;

		// Token: 0x040009BD RID: 2493
		private readonly string lookupMember;

		// Token: 0x040009BE RID: 2494
		public static readonly LookupBindingPropertiesAttribute Default = new LookupBindingPropertiesAttribute();
	}
}

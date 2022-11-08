using System;

namespace System.ComponentModel
{
	// Token: 0x020000B4 RID: 180
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ComplexBindingPropertiesAttribute : Attribute
	{
		// Token: 0x0600065C RID: 1628 RVA: 0x00018638 File Offset: 0x00017638
		public ComplexBindingPropertiesAttribute()
		{
			this.dataSource = null;
			this.dataMember = null;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0001864E File Offset: 0x0001764E
		public ComplexBindingPropertiesAttribute(string dataSource)
		{
			this.dataSource = dataSource;
			this.dataMember = null;
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00018664 File Offset: 0x00017664
		public ComplexBindingPropertiesAttribute(string dataSource, string dataMember)
		{
			this.dataSource = dataSource;
			this.dataMember = dataMember;
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0001867A File Offset: 0x0001767A
		public string DataSource
		{
			get
			{
				return this.dataSource;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x00018682 File Offset: 0x00017682
		public string DataMember
		{
			get
			{
				return this.dataMember;
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0001868C File Offset: 0x0001768C
		public override bool Equals(object obj)
		{
			ComplexBindingPropertiesAttribute complexBindingPropertiesAttribute = obj as ComplexBindingPropertiesAttribute;
			return complexBindingPropertiesAttribute != null && complexBindingPropertiesAttribute.DataSource == this.dataSource && complexBindingPropertiesAttribute.DataMember == this.dataMember;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x000186C9 File Offset: 0x000176C9
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000912 RID: 2322
		private readonly string dataSource;

		// Token: 0x04000913 RID: 2323
		private readonly string dataMember;

		// Token: 0x04000914 RID: 2324
		public static readonly ComplexBindingPropertiesAttribute Default = new ComplexBindingPropertiesAttribute();
	}
}

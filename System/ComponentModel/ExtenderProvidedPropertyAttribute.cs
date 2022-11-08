using System;

namespace System.ComponentModel
{
	// Token: 0x020000E6 RID: 230
	[AttributeUsage(AttributeTargets.All)]
	public sealed class ExtenderProvidedPropertyAttribute : Attribute
	{
		// Token: 0x060007C1 RID: 1985 RVA: 0x0001BD24 File Offset: 0x0001AD24
		internal static ExtenderProvidedPropertyAttribute Create(PropertyDescriptor extenderProperty, Type receiverType, IExtenderProvider provider)
		{
			return new ExtenderProvidedPropertyAttribute
			{
				extenderProperty = extenderProperty,
				receiverType = receiverType,
				provider = provider
			};
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x0001BD55 File Offset: 0x0001AD55
		public PropertyDescriptor ExtenderProperty
		{
			get
			{
				return this.extenderProperty;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x0001BD5D File Offset: 0x0001AD5D
		public IExtenderProvider Provider
		{
			get
			{
				return this.provider;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x0001BD65 File Offset: 0x0001AD65
		public Type ReceiverType
		{
			get
			{
				return this.receiverType;
			}
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0001BD70 File Offset: 0x0001AD70
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ExtenderProvidedPropertyAttribute extenderProvidedPropertyAttribute = obj as ExtenderProvidedPropertyAttribute;
			return extenderProvidedPropertyAttribute != null && extenderProvidedPropertyAttribute.extenderProperty.Equals(this.extenderProperty) && extenderProvidedPropertyAttribute.provider.Equals(this.provider) && extenderProvidedPropertyAttribute.receiverType.Equals(this.receiverType);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0001BDC6 File Offset: 0x0001ADC6
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0001BDCE File Offset: 0x0001ADCE
		public override bool IsDefaultAttribute()
		{
			return this.receiverType == null;
		}

		// Token: 0x04000978 RID: 2424
		private PropertyDescriptor extenderProperty;

		// Token: 0x04000979 RID: 2425
		private IExtenderProvider provider;

		// Token: 0x0400097A RID: 2426
		private Type receiverType;
	}
}

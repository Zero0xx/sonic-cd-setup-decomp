using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000E5 RID: 229
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal sealed class ExtendedPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x060007B6 RID: 1974 RVA: 0x0001BB40 File Offset: 0x0001AB40
		public ExtendedPropertyDescriptor(ReflectPropertyDescriptor extenderInfo, Type receiverType, IExtenderProvider provider, Attribute[] attributes) : base(extenderInfo, attributes)
		{
			ArrayList arrayList = new ArrayList(this.AttributeArray);
			arrayList.Add(ExtenderProvidedPropertyAttribute.Create(extenderInfo, receiverType, provider));
			if (extenderInfo.IsReadOnly)
			{
				arrayList.Add(ReadOnlyAttribute.Yes);
			}
			Attribute[] array = new Attribute[arrayList.Count];
			arrayList.CopyTo(array, 0);
			this.AttributeArray = array;
			this.extenderInfo = extenderInfo;
			this.provider = provider;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001BBB0 File Offset: 0x0001ABB0
		public ExtendedPropertyDescriptor(PropertyDescriptor extender, Attribute[] attributes) : base(extender, attributes)
		{
			ExtenderProvidedPropertyAttribute extenderProvidedPropertyAttribute = extender.Attributes[typeof(ExtenderProvidedPropertyAttribute)] as ExtenderProvidedPropertyAttribute;
			ReflectPropertyDescriptor reflectPropertyDescriptor = extenderProvidedPropertyAttribute.ExtenderProperty as ReflectPropertyDescriptor;
			this.extenderInfo = reflectPropertyDescriptor;
			this.provider = extenderProvidedPropertyAttribute.Provider;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0001BBFF File Offset: 0x0001ABFF
		public override bool CanResetValue(object comp)
		{
			return this.extenderInfo.ExtenderCanResetValue(this.provider, comp);
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x0001BC13 File Offset: 0x0001AC13
		public override Type ComponentType
		{
			get
			{
				return this.extenderInfo.ComponentType;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x0001BC20 File Offset: 0x0001AC20
		public override bool IsReadOnly
		{
			get
			{
				return this.Attributes[typeof(ReadOnlyAttribute)].Equals(ReadOnlyAttribute.Yes);
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x0001BC41 File Offset: 0x0001AC41
		public override Type PropertyType
		{
			get
			{
				return this.extenderInfo.ExtenderGetType(this.provider);
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x0001BC54 File Offset: 0x0001AC54
		public override string DisplayName
		{
			get
			{
				string text = base.DisplayName;
				DisplayNameAttribute displayNameAttribute = this.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
				if (displayNameAttribute == null || displayNameAttribute.IsDefaultAttribute())
				{
					ISite site = MemberDescriptor.GetSite(this.provider);
					if (site != null)
					{
						string name = site.Name;
						if (name != null && name.Length > 0)
						{
							text = SR.GetString("MetaExtenderName", new object[]
							{
								text,
								name
							});
						}
					}
				}
				return text;
			}
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0001BCD0 File Offset: 0x0001ACD0
		public override object GetValue(object comp)
		{
			return this.extenderInfo.ExtenderGetValue(this.provider, comp);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0001BCE4 File Offset: 0x0001ACE4
		public override void ResetValue(object comp)
		{
			this.extenderInfo.ExtenderResetValue(this.provider, comp, this);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0001BCF9 File Offset: 0x0001ACF9
		public override void SetValue(object component, object value)
		{
			this.extenderInfo.ExtenderSetValue(this.provider, component, value, this);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0001BD0F File Offset: 0x0001AD0F
		public override bool ShouldSerializeValue(object comp)
		{
			return this.extenderInfo.ExtenderShouldSerializeValue(this.provider, comp);
		}

		// Token: 0x04000976 RID: 2422
		private readonly ReflectPropertyDescriptor extenderInfo;

		// Token: 0x04000977 RID: 2423
		private readonly IExtenderProvider provider;
	}
}

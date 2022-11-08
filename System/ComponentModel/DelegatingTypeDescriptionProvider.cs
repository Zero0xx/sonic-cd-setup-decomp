using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000D1 RID: 209
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal sealed class DelegatingTypeDescriptionProvider : TypeDescriptionProvider
	{
		// Token: 0x06000719 RID: 1817 RVA: 0x0001A61A File Offset: 0x0001961A
		internal DelegatingTypeDescriptionProvider(Type type)
		{
			this._type = type;
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x0001A629 File Offset: 0x00019629
		private TypeDescriptionProvider Provider
		{
			get
			{
				return TypeDescriptor.GetProviderRecursive(this._type);
			}
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0001A636 File Offset: 0x00019636
		public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
		{
			return this.Provider.CreateInstance(provider, objectType, argTypes, args);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0001A648 File Offset: 0x00019648
		public override IDictionary GetCache(object instance)
		{
			return this.Provider.GetCache(instance);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0001A656 File Offset: 0x00019656
		public override string GetFullComponentName(object component)
		{
			return this.Provider.GetFullComponentName(component);
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0001A664 File Offset: 0x00019664
		public override ICustomTypeDescriptor GetExtendedTypeDescriptor(object instance)
		{
			return this.Provider.GetExtendedTypeDescriptor(instance);
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0001A672 File Offset: 0x00019672
		public override Type GetReflectionType(Type objectType, object instance)
		{
			return this.Provider.GetReflectionType(objectType, instance);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0001A681 File Offset: 0x00019681
		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			return this.Provider.GetTypeDescriptor(objectType, instance);
		}

		// Token: 0x04000940 RID: 2368
		private Type _type;
	}
}

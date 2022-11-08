using System;
using System.Diagnostics;

namespace System.Runtime.Versioning
{
	// Token: 0x0200091A RID: 2330
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
	[Conditional("RESOURCE_ANNOTATION_WORK")]
	public sealed class ResourceConsumptionAttribute : Attribute
	{
		// Token: 0x06005473 RID: 21619 RVA: 0x00130D78 File Offset: 0x0012FD78
		public ResourceConsumptionAttribute(ResourceScope resourceScope)
		{
			this._resourceScope = resourceScope;
			this._consumptionScope = this._resourceScope;
		}

		// Token: 0x06005474 RID: 21620 RVA: 0x00130D93 File Offset: 0x0012FD93
		public ResourceConsumptionAttribute(ResourceScope resourceScope, ResourceScope consumptionScope)
		{
			this._resourceScope = resourceScope;
			this._consumptionScope = consumptionScope;
		}

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x06005475 RID: 21621 RVA: 0x00130DA9 File Offset: 0x0012FDA9
		public ResourceScope ResourceScope
		{
			get
			{
				return this._resourceScope;
			}
		}

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x06005476 RID: 21622 RVA: 0x00130DB1 File Offset: 0x0012FDB1
		public ResourceScope ConsumptionScope
		{
			get
			{
				return this._consumptionScope;
			}
		}

		// Token: 0x04002C02 RID: 11266
		private ResourceScope _consumptionScope;

		// Token: 0x04002C03 RID: 11267
		private ResourceScope _resourceScope;
	}
}

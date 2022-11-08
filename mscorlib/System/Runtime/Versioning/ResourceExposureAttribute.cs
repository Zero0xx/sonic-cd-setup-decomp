using System;
using System.Diagnostics;

namespace System.Runtime.Versioning
{
	// Token: 0x0200091B RID: 2331
	[Conditional("RESOURCE_ANNOTATION_WORK")]
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
	public sealed class ResourceExposureAttribute : Attribute
	{
		// Token: 0x06005477 RID: 21623 RVA: 0x00130DB9 File Offset: 0x0012FDB9
		public ResourceExposureAttribute(ResourceScope exposureLevel)
		{
			this._resourceExposureLevel = exposureLevel;
		}

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x06005478 RID: 21624 RVA: 0x00130DC8 File Offset: 0x0012FDC8
		public ResourceScope ResourceExposureLevel
		{
			get
			{
				return this._resourceExposureLevel;
			}
		}

		// Token: 0x04002C04 RID: 11268
		private ResourceScope _resourceExposureLevel;
	}
}

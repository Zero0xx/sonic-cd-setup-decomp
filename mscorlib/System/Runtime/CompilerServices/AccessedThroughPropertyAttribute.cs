using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005D2 RID: 1490
	[AttributeUsage(AttributeTargets.Field)]
	[ComVisible(true)]
	public sealed class AccessedThroughPropertyAttribute : Attribute
	{
		// Token: 0x060037AD RID: 14253 RVA: 0x000BB9A3 File Offset: 0x000BA9A3
		public AccessedThroughPropertyAttribute(string propertyName)
		{
			this.propertyName = propertyName;
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x060037AE RID: 14254 RVA: 0x000BB9B2 File Offset: 0x000BA9B2
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x04001CDF RID: 7391
		private readonly string propertyName;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005F0 RID: 1520
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class RequiredAttributeAttribute : Attribute
	{
		// Token: 0x060037F3 RID: 14323 RVA: 0x000BBC9C File Offset: 0x000BAC9C
		public RequiredAttributeAttribute(Type requiredContract)
		{
			this.requiredContract = requiredContract;
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x060037F4 RID: 14324 RVA: 0x000BBCAB File Offset: 0x000BACAB
		public Type RequiredContract
		{
			get
			{
				return this.requiredContract;
			}
		}

		// Token: 0x04001CFE RID: 7422
		private Type requiredContract;
	}
}

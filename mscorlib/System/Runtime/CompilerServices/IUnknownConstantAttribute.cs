using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200060D RID: 1549
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class IUnknownConstantAttribute : CustomConstantAttribute
	{
		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x0600380F RID: 14351 RVA: 0x000BBE11 File Offset: 0x000BAE11
		public override object Value
		{
			get
			{
				return new UnknownWrapper(null);
			}
		}
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200060C RID: 1548
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[Serializable]
	public sealed class IDispatchConstantAttribute : CustomConstantAttribute
	{
		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x0600380D RID: 14349 RVA: 0x000BBE01 File Offset: 0x000BAE01
		public override object Value
		{
			get
			{
				return new DispatchWrapper(null);
			}
		}
	}
}

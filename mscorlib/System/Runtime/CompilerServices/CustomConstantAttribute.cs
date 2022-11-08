using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005E0 RID: 1504
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public abstract class CustomConstantAttribute : Attribute
	{
		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x060037D8 RID: 14296
		public abstract object Value { get; }
	}
}

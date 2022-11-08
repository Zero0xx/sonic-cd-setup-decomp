using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002B6 RID: 694
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
	[Serializable]
	public sealed class DebuggerStepperBoundaryAttribute : Attribute
	{
	}
}

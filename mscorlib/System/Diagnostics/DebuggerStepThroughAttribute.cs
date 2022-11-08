using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002B5 RID: 693
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class DebuggerStepThroughAttribute : Attribute
	{
	}
}

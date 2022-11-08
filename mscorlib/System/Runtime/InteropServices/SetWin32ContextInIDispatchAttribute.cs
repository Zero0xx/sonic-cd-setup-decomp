using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000508 RID: 1288
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[Obsolete("This attribute has been deprecated.  Application Domains no longer respect Activation Context boundaries in IDispatch calls.", false)]
	[ComVisible(true)]
	public sealed class SetWin32ContextInIDispatchAttribute : Attribute
	{
	}
}

using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Serialization
{
	// Token: 0x020000A7 RID: 167
	[ComVisible(true)]
	public interface IObjectReference
	{
		// Token: 0x06000A18 RID: 2584
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		object GetRealObject(StreamingContext context);
	}
}

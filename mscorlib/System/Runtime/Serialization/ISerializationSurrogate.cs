using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Serialization
{
	// Token: 0x02000360 RID: 864
	[ComVisible(true)]
	public interface ISerializationSurrogate
	{
		// Token: 0x06002208 RID: 8712
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void GetObjectData(object obj, SerializationInfo info, StreamingContext context);

		// Token: 0x06002209 RID: 8713
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector);
	}
}

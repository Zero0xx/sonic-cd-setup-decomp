using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Serialization
{
	// Token: 0x02000361 RID: 865
	[ComVisible(true)]
	public interface ISurrogateSelector
	{
		// Token: 0x0600220A RID: 8714
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ChainSelector(ISurrogateSelector selector);

		// Token: 0x0600220B RID: 8715
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector);

		// Token: 0x0600220C RID: 8716
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		ISurrogateSelector GetNextSelector();
	}
}

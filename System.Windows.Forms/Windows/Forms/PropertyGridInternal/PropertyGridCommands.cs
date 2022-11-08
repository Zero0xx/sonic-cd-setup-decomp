using System;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007C6 RID: 1990
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class PropertyGridCommands
	{
		// Token: 0x04003DB8 RID: 15800
		protected static readonly Guid wfcMenuGroup = new Guid("{a72bd644-1979-4cbc-a620-ea4112198a66}");

		// Token: 0x04003DB9 RID: 15801
		protected static readonly Guid wfcMenuCommand = new Guid("{5a51cf82-7619-4a5d-b054-47f438425aa7}");

		// Token: 0x04003DBA RID: 15802
		public static readonly CommandID Reset = new CommandID(PropertyGridCommands.wfcMenuCommand, 12288);

		// Token: 0x04003DBB RID: 15803
		public static readonly CommandID Description = new CommandID(PropertyGridCommands.wfcMenuCommand, 12289);

		// Token: 0x04003DBC RID: 15804
		public static readonly CommandID Hide = new CommandID(PropertyGridCommands.wfcMenuCommand, 12290);

		// Token: 0x04003DBD RID: 15805
		public static readonly CommandID Commands = new CommandID(PropertyGridCommands.wfcMenuCommand, 12304);
	}
}

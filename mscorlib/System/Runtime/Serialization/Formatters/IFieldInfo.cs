using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x0200071F RID: 1823
	[ComVisible(true)]
	public interface IFieldInfo
	{
		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06004160 RID: 16736
		// (set) Token: 0x06004161 RID: 16737
		string[] FieldNames { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)] get; [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)] set; }

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06004162 RID: 16738
		// (set) Token: 0x06004163 RID: 16739
		Type[] FieldTypes { [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)] get; [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)] set; }
	}
}

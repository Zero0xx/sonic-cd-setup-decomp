using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x020007B8 RID: 1976
	[ComVisible(true)]
	[StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0x00000000000000000400000000000000", Name = "System.Runtime.Remoting")]
	public sealed class InternalRM
	{
		// Token: 0x06004670 RID: 18032 RVA: 0x000F070C File Offset: 0x000EF70C
		[Conditional("_LOGGING")]
		public static void InfoSoap(params object[] messages)
		{
		}

		// Token: 0x06004671 RID: 18033 RVA: 0x000F070E File Offset: 0x000EF70E
		public static bool SoapCheckEnabled()
		{
			return BCLDebug.CheckEnabled("SOAP");
		}
	}
}

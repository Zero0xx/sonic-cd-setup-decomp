using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x0200041E RID: 1054
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct hostent
	{
		// Token: 0x04002133 RID: 8499
		public IntPtr h_name;

		// Token: 0x04002134 RID: 8500
		public IntPtr h_aliases;

		// Token: 0x04002135 RID: 8501
		public short h_addrtype;

		// Token: 0x04002136 RID: 8502
		public short h_length;

		// Token: 0x04002137 RID: 8503
		public IntPtr h_addr_list;
	}
}

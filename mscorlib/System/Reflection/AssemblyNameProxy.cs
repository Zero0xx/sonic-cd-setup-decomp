using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002F2 RID: 754
	[ComVisible(true)]
	public class AssemblyNameProxy : MarshalByRefObject
	{
		// Token: 0x06001D52 RID: 7506 RVA: 0x0004A590 File Offset: 0x00049590
		public AssemblyName GetAssemblyName(string assemblyFile)
		{
			return AssemblyName.nGetFileInformation(assemblyFile);
		}
	}
}

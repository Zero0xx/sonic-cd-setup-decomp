using System;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000516 RID: 1302
	internal class ImporterCallback : ITypeLibImporterNotifySink
	{
		// Token: 0x060032A9 RID: 12969 RVA: 0x000AB41C File Offset: 0x000AA41C
		public void ReportEvent(ImporterEventKind EventKind, int EventCode, string EventMsg)
		{
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x000AB420 File Offset: 0x000AA420
		public Assembly ResolveRef(object TypeLib)
		{
			Assembly result;
			try
			{
				ITypeLibConverter typeLibConverter = new TypeLibConverter();
				result = typeLibConverter.ConvertTypeLibToAssembly(TypeLib, Marshal.GetTypeLibName((ITypeLib)TypeLib) + ".dll", TypeLibImporterFlags.None, new ImporterCallback(), null, null, null, null);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002E8 RID: 744
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyFileVersionAttribute : Attribute
	{
		// Token: 0x06001D0D RID: 7437 RVA: 0x00049DB3 File Offset: 0x00048DB3
		public AssemblyFileVersionAttribute(string version)
		{
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			this._version = version;
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001D0E RID: 7438 RVA: 0x00049DD0 File Offset: 0x00048DD0
		public string Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x04000AD4 RID: 2772
		private string _version;
	}
}

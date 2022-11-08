using System;
using System.Runtime.InteropServices;

namespace System.Resources
{
	// Token: 0x0200043F RID: 1087
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	[ComVisible(true)]
	public sealed class SatelliteContractVersionAttribute : Attribute
	{
		// Token: 0x06002C60 RID: 11360 RVA: 0x00096D28 File Offset: 0x00095D28
		public SatelliteContractVersionAttribute(string version)
		{
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			this._version = version;
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06002C61 RID: 11361 RVA: 0x00096D45 File Offset: 0x00095D45
		public string Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x040015B8 RID: 5560
		private string _version;
	}
}

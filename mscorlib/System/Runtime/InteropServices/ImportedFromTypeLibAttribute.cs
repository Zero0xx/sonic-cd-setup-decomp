using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E8 RID: 1256
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class ImportedFromTypeLibAttribute : Attribute
	{
		// Token: 0x06003151 RID: 12625 RVA: 0x000A90D8 File Offset: 0x000A80D8
		public ImportedFromTypeLibAttribute(string tlbFile)
		{
			this._val = tlbFile;
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06003152 RID: 12626 RVA: 0x000A90E7 File Offset: 0x000A80E7
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001901 RID: 6401
		internal string _val;
	}
}

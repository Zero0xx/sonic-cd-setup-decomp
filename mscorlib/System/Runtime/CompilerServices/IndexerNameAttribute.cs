using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005E8 RID: 1512
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Property, Inherited = true)]
	[Serializable]
	public sealed class IndexerNameAttribute : Attribute
	{
		// Token: 0x060037E7 RID: 14311 RVA: 0x000BBC05 File Offset: 0x000BAC05
		public IndexerNameAttribute(string indexerName)
		{
		}
	}
}

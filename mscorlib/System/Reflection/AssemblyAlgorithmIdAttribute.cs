using System;
using System.Configuration.Assemblies;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002ED RID: 749
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyAlgorithmIdAttribute : Attribute
	{
		// Token: 0x06001D17 RID: 7447 RVA: 0x00049E34 File Offset: 0x00048E34
		public AssemblyAlgorithmIdAttribute(AssemblyHashAlgorithm algorithmId)
		{
			this.m_algId = (uint)algorithmId;
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x00049E43 File Offset: 0x00048E43
		[CLSCompliant(false)]
		public AssemblyAlgorithmIdAttribute(uint algorithmId)
		{
			this.m_algId = algorithmId;
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x00049E52 File Offset: 0x00048E52
		[CLSCompliant(false)]
		public uint AlgorithmId
		{
			get
			{
				return this.m_algId;
			}
		}

		// Token: 0x04000AD9 RID: 2777
		private uint m_algId;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000333 RID: 819
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
	[ComVisible(true)]
	public sealed class ObfuscateAssemblyAttribute : Attribute
	{
		// Token: 0x06001F8E RID: 8078 RVA: 0x0004F673 File Offset: 0x0004E673
		public ObfuscateAssemblyAttribute(bool assemblyIsPrivate)
		{
			this.m_assemblyIsPrivate = assemblyIsPrivate;
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x0004F689 File Offset: 0x0004E689
		public bool AssemblyIsPrivate
		{
			get
			{
				return this.m_assemblyIsPrivate;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001F90 RID: 8080 RVA: 0x0004F691 File Offset: 0x0004E691
		// (set) Token: 0x06001F91 RID: 8081 RVA: 0x0004F699 File Offset: 0x0004E699
		public bool StripAfterObfuscation
		{
			get
			{
				return this.m_strip;
			}
			set
			{
				this.m_strip = value;
			}
		}

		// Token: 0x04000D84 RID: 3460
		private bool m_assemblyIsPrivate;

		// Token: 0x04000D85 RID: 3461
		private bool m_strip = true;
	}
}

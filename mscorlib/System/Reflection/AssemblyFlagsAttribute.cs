using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002EE RID: 750
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyFlagsAttribute : Attribute
	{
		// Token: 0x06001D1A RID: 7450 RVA: 0x00049E5A File Offset: 0x00048E5A
		[Obsolete("This constructor has been deprecated. Please use AssemblyFlagsAttribute(AssemblyNameFlags) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		[CLSCompliant(false)]
		public AssemblyFlagsAttribute(uint flags)
		{
			this.m_flags = (AssemblyNameFlags)flags;
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001D1B RID: 7451 RVA: 0x00049E69 File Offset: 0x00048E69
		[CLSCompliant(false)]
		[Obsolete("This property has been deprecated. Please use AssemblyFlags instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public uint Flags
		{
			get
			{
				return (uint)this.m_flags;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001D1C RID: 7452 RVA: 0x00049E71 File Offset: 0x00048E71
		public int AssemblyFlags
		{
			get
			{
				return (int)this.m_flags;
			}
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x00049E79 File Offset: 0x00048E79
		[Obsolete("This constructor has been deprecated. Please use AssemblyFlagsAttribute(AssemblyNameFlags) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public AssemblyFlagsAttribute(int assemblyFlags)
		{
			this.m_flags = (AssemblyNameFlags)assemblyFlags;
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x00049E88 File Offset: 0x00048E88
		public AssemblyFlagsAttribute(AssemblyNameFlags assemblyFlags)
		{
			this.m_flags = assemblyFlags;
		}

		// Token: 0x04000ADA RID: 2778
		private AssemblyNameFlags m_flags;
	}
}

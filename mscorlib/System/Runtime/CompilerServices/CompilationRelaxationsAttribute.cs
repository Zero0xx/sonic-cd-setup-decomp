using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005E5 RID: 1509
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Method)]
	[Serializable]
	public class CompilationRelaxationsAttribute : Attribute
	{
		// Token: 0x060037E0 RID: 14304 RVA: 0x000BBBB1 File Offset: 0x000BABB1
		public CompilationRelaxationsAttribute(int relaxations)
		{
			this.m_relaxations = relaxations;
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x000BBBC0 File Offset: 0x000BABC0
		public CompilationRelaxationsAttribute(CompilationRelaxations relaxations)
		{
			this.m_relaxations = (int)relaxations;
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x060037E2 RID: 14306 RVA: 0x000BBBCF File Offset: 0x000BABCF
		public int CompilationRelaxations
		{
			get
			{
				return this.m_relaxations;
			}
		}

		// Token: 0x04001CEA RID: 7402
		private int m_relaxations;
	}
}

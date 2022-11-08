using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002BC RID: 700
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public sealed class DebuggerBrowsableAttribute : Attribute
	{
		// Token: 0x06001B23 RID: 6947 RVA: 0x00046EFF File Offset: 0x00045EFF
		public DebuggerBrowsableAttribute(DebuggerBrowsableState state)
		{
			if (state < DebuggerBrowsableState.Never || state > DebuggerBrowsableState.RootHidden)
			{
				throw new ArgumentOutOfRangeException("state");
			}
			this.state = state;
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001B24 RID: 6948 RVA: 0x00046F21 File Offset: 0x00045F21
		public DebuggerBrowsableState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x04000A5E RID: 2654
		private DebuggerBrowsableState state;
	}
}

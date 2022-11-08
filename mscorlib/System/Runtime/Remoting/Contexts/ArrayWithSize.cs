using System;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006DA RID: 1754
	internal class ArrayWithSize
	{
		// Token: 0x06003F1B RID: 16155 RVA: 0x000D833A File Offset: 0x000D733A
		internal ArrayWithSize(IDynamicMessageSink[] sinks, int count)
		{
			this.Sinks = sinks;
			this.Count = count;
		}

		// Token: 0x04002008 RID: 8200
		internal IDynamicMessageSink[] Sinks;

		// Token: 0x04002009 RID: 8201
		internal int Count;
	}
}

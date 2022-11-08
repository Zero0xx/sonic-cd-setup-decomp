using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x0200037C RID: 892
	[ComVisible(true)]
	[Serializable]
	public struct StreamingContext
	{
		// Token: 0x060022F6 RID: 8950 RVA: 0x000584BA File Offset: 0x000574BA
		public StreamingContext(StreamingContextStates state)
		{
			this = new StreamingContext(state, null);
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x000584C4 File Offset: 0x000574C4
		public StreamingContext(StreamingContextStates state, object additional)
		{
			this.m_state = state;
			this.m_additionalContext = additional;
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x060022F8 RID: 8952 RVA: 0x000584D4 File Offset: 0x000574D4
		public object Context
		{
			get
			{
				return this.m_additionalContext;
			}
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x000584DC File Offset: 0x000574DC
		public override bool Equals(object obj)
		{
			return obj is StreamingContext && (((StreamingContext)obj).m_additionalContext == this.m_additionalContext && ((StreamingContext)obj).m_state == this.m_state);
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x00058511 File Offset: 0x00057511
		public override int GetHashCode()
		{
			return (int)this.m_state;
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x060022FB RID: 8955 RVA: 0x00058519 File Offset: 0x00057519
		public StreamingContextStates State
		{
			get
			{
				return this.m_state;
			}
		}

		// Token: 0x04000EAF RID: 3759
		internal object m_additionalContext;

		// Token: 0x04000EB0 RID: 3760
		internal StreamingContextStates m_state;
	}
}

using System;

namespace System.Runtime.Serialization
{
	// Token: 0x0200037F RID: 895
	[Serializable]
	internal class SurrogateKey
	{
		// Token: 0x06002303 RID: 8963 RVA: 0x0005875B File Offset: 0x0005775B
		internal SurrogateKey(Type type, StreamingContext context)
		{
			this.m_type = type;
			this.m_context = context;
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x00058771 File Offset: 0x00057771
		public override int GetHashCode()
		{
			return this.m_type.GetHashCode();
		}

		// Token: 0x04000EBD RID: 3773
		internal Type m_type;

		// Token: 0x04000EBE RID: 3774
		internal StreamingContext m_context;
	}
}

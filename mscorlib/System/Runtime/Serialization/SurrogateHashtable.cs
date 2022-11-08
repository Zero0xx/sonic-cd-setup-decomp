using System;
using System.Collections;

namespace System.Runtime.Serialization
{
	// Token: 0x02000380 RID: 896
	internal class SurrogateHashtable : Hashtable
	{
		// Token: 0x06002305 RID: 8965 RVA: 0x0005877E File Offset: 0x0005777E
		internal SurrogateHashtable(int size) : base(size)
		{
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x00058788 File Offset: 0x00057788
		protected override bool KeyEquals(object key, object item)
		{
			SurrogateKey surrogateKey = (SurrogateKey)item;
			SurrogateKey surrogateKey2 = (SurrogateKey)key;
			return surrogateKey2.m_type == surrogateKey.m_type && (surrogateKey2.m_context.m_state & surrogateKey.m_context.m_state) == surrogateKey.m_context.m_state && surrogateKey2.m_context.Context == surrogateKey.m_context.Context;
		}
	}
}

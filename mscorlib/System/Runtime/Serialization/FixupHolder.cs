using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000366 RID: 870
	[Serializable]
	internal class FixupHolder
	{
		// Token: 0x06002266 RID: 8806 RVA: 0x00056CD0 File Offset: 0x00055CD0
		internal FixupHolder(long id, object fixupInfo, int fixupType)
		{
			this.m_id = id;
			this.m_fixupInfo = fixupInfo;
			this.m_fixupType = fixupType;
		}

		// Token: 0x04000E72 RID: 3698
		internal const int ArrayFixup = 1;

		// Token: 0x04000E73 RID: 3699
		internal const int MemberFixup = 2;

		// Token: 0x04000E74 RID: 3700
		internal const int DelayedFixup = 4;

		// Token: 0x04000E75 RID: 3701
		internal long m_id;

		// Token: 0x04000E76 RID: 3702
		internal object m_fixupInfo;

		// Token: 0x04000E77 RID: 3703
		internal int m_fixupType;
	}
}

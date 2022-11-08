using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000338 RID: 824
	[ComVisible(true)]
	public class LocalVariableInfo
	{
		// Token: 0x06001FAB RID: 8107 RVA: 0x0004F990 File Offset: 0x0004E990
		internal LocalVariableInfo()
		{
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x0004F998 File Offset: 0x0004E998
		public override string ToString()
		{
			string text = string.Concat(new object[]
			{
				this.LocalType.ToString(),
				" (",
				this.LocalIndex,
				")"
			});
			if (this.IsPinned)
			{
				text += " (pinned)";
			}
			return text;
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001FAD RID: 8109 RVA: 0x0004F9F4 File Offset: 0x0004E9F4
		public virtual Type LocalType
		{
			get
			{
				return this.m_typeHandle.GetRuntimeType();
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001FAE RID: 8110 RVA: 0x0004FA01 File Offset: 0x0004EA01
		public virtual bool IsPinned
		{
			get
			{
				return this.m_isPinned != 0;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001FAF RID: 8111 RVA: 0x0004FA0F File Offset: 0x0004EA0F
		public virtual int LocalIndex
		{
			get
			{
				return this.m_localIndex;
			}
		}

		// Token: 0x04000D9E RID: 3486
		private int m_isPinned;

		// Token: 0x04000D9F RID: 3487
		private int m_localIndex;

		// Token: 0x04000DA0 RID: 3488
		private RuntimeTypeHandle m_typeHandle;
	}
}

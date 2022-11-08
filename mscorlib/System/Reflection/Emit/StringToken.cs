using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000849 RID: 2121
	[ComVisible(true)]
	[Serializable]
	public struct StringToken
	{
		// Token: 0x06004C67 RID: 19559 RVA: 0x0010BA98 File Offset: 0x0010AA98
		internal StringToken(int str)
		{
			this.m_string = str;
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x06004C68 RID: 19560 RVA: 0x0010BAA1 File Offset: 0x0010AAA1
		public int Token
		{
			get
			{
				return this.m_string;
			}
		}

		// Token: 0x06004C69 RID: 19561 RVA: 0x0010BAA9 File Offset: 0x0010AAA9
		public override int GetHashCode()
		{
			return this.m_string;
		}

		// Token: 0x06004C6A RID: 19562 RVA: 0x0010BAB1 File Offset: 0x0010AAB1
		public override bool Equals(object obj)
		{
			return obj is StringToken && this.Equals((StringToken)obj);
		}

		// Token: 0x06004C6B RID: 19563 RVA: 0x0010BAC9 File Offset: 0x0010AAC9
		public bool Equals(StringToken obj)
		{
			return obj.m_string == this.m_string;
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x0010BADA File Offset: 0x0010AADA
		public static bool operator ==(StringToken a, StringToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004C6D RID: 19565 RVA: 0x0010BAE4 File Offset: 0x0010AAE4
		public static bool operator !=(StringToken a, StringToken b)
		{
			return !(a == b);
		}

		// Token: 0x0400281C RID: 10268
		internal int m_string;
	}
}

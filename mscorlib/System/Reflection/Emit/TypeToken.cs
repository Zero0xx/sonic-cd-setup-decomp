using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000850 RID: 2128
	[ComVisible(true)]
	[Serializable]
	public struct TypeToken
	{
		// Token: 0x06004DEC RID: 19948 RVA: 0x0010F42B File Offset: 0x0010E42B
		internal TypeToken(int str)
		{
			this.m_class = str;
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x06004DED RID: 19949 RVA: 0x0010F434 File Offset: 0x0010E434
		public int Token
		{
			get
			{
				return this.m_class;
			}
		}

		// Token: 0x06004DEE RID: 19950 RVA: 0x0010F43C File Offset: 0x0010E43C
		public override int GetHashCode()
		{
			return this.m_class;
		}

		// Token: 0x06004DEF RID: 19951 RVA: 0x0010F444 File Offset: 0x0010E444
		public override bool Equals(object obj)
		{
			return obj is TypeToken && this.Equals((TypeToken)obj);
		}

		// Token: 0x06004DF0 RID: 19952 RVA: 0x0010F45C File Offset: 0x0010E45C
		public bool Equals(TypeToken obj)
		{
			return obj.m_class == this.m_class;
		}

		// Token: 0x06004DF1 RID: 19953 RVA: 0x0010F46D File Offset: 0x0010E46D
		public static bool operator ==(TypeToken a, TypeToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004DF2 RID: 19954 RVA: 0x0010F477 File Offset: 0x0010E477
		public static bool operator !=(TypeToken a, TypeToken b)
		{
			return !(a == b);
		}

		// Token: 0x0400284E RID: 10318
		public static readonly TypeToken Empty = default(TypeToken);

		// Token: 0x0400284F RID: 10319
		internal int m_class;
	}
}

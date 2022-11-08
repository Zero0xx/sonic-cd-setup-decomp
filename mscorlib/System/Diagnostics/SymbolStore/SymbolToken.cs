using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics.SymbolStore
{
	// Token: 0x020002D9 RID: 729
	[ComVisible(true)]
	public struct SymbolToken
	{
		// Token: 0x06001C01 RID: 7169 RVA: 0x000486BC File Offset: 0x000476BC
		public SymbolToken(int val)
		{
			this.m_token = val;
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x000486C5 File Offset: 0x000476C5
		public int GetToken()
		{
			return this.m_token;
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x000486CD File Offset: 0x000476CD
		public override int GetHashCode()
		{
			return this.m_token;
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x000486D5 File Offset: 0x000476D5
		public override bool Equals(object obj)
		{
			return obj is SymbolToken && this.Equals((SymbolToken)obj);
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x000486ED File Offset: 0x000476ED
		public bool Equals(SymbolToken obj)
		{
			return obj.m_token == this.m_token;
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x000486FE File Offset: 0x000476FE
		public static bool operator ==(SymbolToken a, SymbolToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x00048708 File Offset: 0x00047708
		public static bool operator !=(SymbolToken a, SymbolToken b)
		{
			return !(a == b);
		}

		// Token: 0x04000AC5 RID: 2757
		internal int m_token;
	}
}

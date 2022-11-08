using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000846 RID: 2118
	[ComVisible(true)]
	[Serializable]
	public struct PropertyToken
	{
		// Token: 0x06004C1E RID: 19486 RVA: 0x0010A87B File Offset: 0x0010987B
		internal PropertyToken(int str)
		{
			this.m_property = str;
		}

		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x06004C1F RID: 19487 RVA: 0x0010A884 File Offset: 0x00109884
		public int Token
		{
			get
			{
				return this.m_property;
			}
		}

		// Token: 0x06004C20 RID: 19488 RVA: 0x0010A88C File Offset: 0x0010988C
		public override int GetHashCode()
		{
			return this.m_property;
		}

		// Token: 0x06004C21 RID: 19489 RVA: 0x0010A894 File Offset: 0x00109894
		public override bool Equals(object obj)
		{
			return obj is PropertyToken && this.Equals((PropertyToken)obj);
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x0010A8AC File Offset: 0x001098AC
		public bool Equals(PropertyToken obj)
		{
			return obj.m_property == this.m_property;
		}

		// Token: 0x06004C23 RID: 19491 RVA: 0x0010A8BD File Offset: 0x001098BD
		public static bool operator ==(PropertyToken a, PropertyToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004C24 RID: 19492 RVA: 0x0010A8C7 File Offset: 0x001098C7
		public static bool operator !=(PropertyToken a, PropertyToken b)
		{
			return !(a == b);
		}

		// Token: 0x040027DA RID: 10202
		public static readonly PropertyToken Empty = default(PropertyToken);

		// Token: 0x040027DB RID: 10203
		internal int m_property;
	}
}

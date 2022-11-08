using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000839 RID: 2105
	[ComVisible(true)]
	[Serializable]
	public struct MethodToken
	{
		// Token: 0x06004B60 RID: 19296 RVA: 0x00105968 File Offset: 0x00104968
		internal MethodToken(int str)
		{
			this.m_method = str;
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06004B61 RID: 19297 RVA: 0x00105971 File Offset: 0x00104971
		public int Token
		{
			get
			{
				return this.m_method;
			}
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x00105979 File Offset: 0x00104979
		public override int GetHashCode()
		{
			return this.m_method;
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x00105981 File Offset: 0x00104981
		public override bool Equals(object obj)
		{
			return obj is MethodToken && this.Equals((MethodToken)obj);
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x00105999 File Offset: 0x00104999
		public bool Equals(MethodToken obj)
		{
			return obj.m_method == this.m_method;
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x001059AA File Offset: 0x001049AA
		public static bool operator ==(MethodToken a, MethodToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x001059B4 File Offset: 0x001049B4
		public static bool operator !=(MethodToken a, MethodToken b)
		{
			return !(a == b);
		}

		// Token: 0x04002684 RID: 9860
		public static readonly MethodToken Empty = default(MethodToken);

		// Token: 0x04002685 RID: 9861
		internal int m_method;
	}
}

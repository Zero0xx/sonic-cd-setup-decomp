using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000844 RID: 2116
	[ComVisible(true)]
	[Serializable]
	public struct ParameterToken
	{
		// Token: 0x06004BF5 RID: 19445 RVA: 0x0010A438 File Offset: 0x00109438
		internal ParameterToken(int tkParam)
		{
			this.m_tkParameter = tkParam;
		}

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x06004BF6 RID: 19446 RVA: 0x0010A441 File Offset: 0x00109441
		public int Token
		{
			get
			{
				return this.m_tkParameter;
			}
		}

		// Token: 0x06004BF7 RID: 19447 RVA: 0x0010A449 File Offset: 0x00109449
		public override int GetHashCode()
		{
			return this.m_tkParameter;
		}

		// Token: 0x06004BF8 RID: 19448 RVA: 0x0010A451 File Offset: 0x00109451
		public override bool Equals(object obj)
		{
			return obj is ParameterToken && this.Equals((ParameterToken)obj);
		}

		// Token: 0x06004BF9 RID: 19449 RVA: 0x0010A469 File Offset: 0x00109469
		public bool Equals(ParameterToken obj)
		{
			return obj.m_tkParameter == this.m_tkParameter;
		}

		// Token: 0x06004BFA RID: 19450 RVA: 0x0010A47A File Offset: 0x0010947A
		public static bool operator ==(ParameterToken a, ParameterToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x0010A484 File Offset: 0x00109484
		public static bool operator !=(ParameterToken a, ParameterToken b)
		{
			return !(a == b);
		}

		// Token: 0x040027CE RID: 10190
		public static readonly ParameterToken Empty = default(ParameterToken);

		// Token: 0x040027CF RID: 10191
		internal int m_tkParameter;
	}
}

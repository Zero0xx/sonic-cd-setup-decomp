using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000848 RID: 2120
	[ComVisible(true)]
	public struct SignatureToken
	{
		// Token: 0x06004C5F RID: 19551 RVA: 0x0010BA2C File Offset: 0x0010AA2C
		internal SignatureToken(int str, ModuleBuilder mod)
		{
			this.m_signature = str;
			this.m_moduleBuilder = mod;
		}

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x06004C60 RID: 19552 RVA: 0x0010BA3C File Offset: 0x0010AA3C
		public int Token
		{
			get
			{
				return this.m_signature;
			}
		}

		// Token: 0x06004C61 RID: 19553 RVA: 0x0010BA44 File Offset: 0x0010AA44
		public override int GetHashCode()
		{
			return this.m_signature;
		}

		// Token: 0x06004C62 RID: 19554 RVA: 0x0010BA4C File Offset: 0x0010AA4C
		public override bool Equals(object obj)
		{
			return obj is SignatureToken && this.Equals((SignatureToken)obj);
		}

		// Token: 0x06004C63 RID: 19555 RVA: 0x0010BA64 File Offset: 0x0010AA64
		public bool Equals(SignatureToken obj)
		{
			return obj.m_signature == this.m_signature;
		}

		// Token: 0x06004C64 RID: 19556 RVA: 0x0010BA75 File Offset: 0x0010AA75
		public static bool operator ==(SignatureToken a, SignatureToken b)
		{
			return a.Equals(b);
		}

		// Token: 0x06004C65 RID: 19557 RVA: 0x0010BA7F File Offset: 0x0010AA7F
		public static bool operator !=(SignatureToken a, SignatureToken b)
		{
			return !(a == b);
		}

		// Token: 0x04002819 RID: 10265
		public static readonly SignatureToken Empty = default(SignatureToken);

		// Token: 0x0400281A RID: 10266
		internal int m_signature;

		// Token: 0x0400281B RID: 10267
		internal ModuleBuilder m_moduleBuilder;
	}
}

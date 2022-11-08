using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008E2 RID: 2274
	public sealed class CryptoKeyAuditRule : AuditRule
	{
		// Token: 0x06005293 RID: 21139 RVA: 0x00129D2F File Offset: 0x00128D2F
		public CryptoKeyAuditRule(IdentityReference identity, CryptoKeyRights cryptoKeyRights, AuditFlags flags) : this(identity, CryptoKeyAuditRule.AccessMaskFromRights(cryptoKeyRights), false, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x00129D42 File Offset: 0x00128D42
		public CryptoKeyAuditRule(string identity, CryptoKeyRights cryptoKeyRights, AuditFlags flags) : this(new NTAccount(identity), CryptoKeyAuditRule.AccessMaskFromRights(cryptoKeyRights), false, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x00129D5A File Offset: 0x00128D5A
		private CryptoKeyAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06005296 RID: 21142 RVA: 0x00129D6B File Offset: 0x00128D6B
		public CryptoKeyRights CryptoKeyRights
		{
			get
			{
				return CryptoKeyAuditRule.RightsFromAccessMask(base.AccessMask);
			}
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x00129D78 File Offset: 0x00128D78
		private static int AccessMaskFromRights(CryptoKeyRights cryptoKeyRights)
		{
			return (int)cryptoKeyRights;
		}

		// Token: 0x06005298 RID: 21144 RVA: 0x00129D7B File Offset: 0x00128D7B
		internal static CryptoKeyRights RightsFromAccessMask(int accessMask)
		{
			return (CryptoKeyRights)accessMask;
		}
	}
}

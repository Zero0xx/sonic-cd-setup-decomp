using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008E1 RID: 2273
	public abstract class AuditRule : AuthorizationRule
	{
		// Token: 0x06005291 RID: 21137 RVA: 0x00129CD0 File Offset: 0x00128CD0
		protected AuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags auditFlags) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags)
		{
			if (auditFlags == AuditFlags.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "auditFlags");
			}
			if ((auditFlags & ~(AuditFlags.Success | AuditFlags.Failure)) != AuditFlags.None)
			{
				throw new ArgumentOutOfRangeException("auditFlags", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
			}
			this._flags = auditFlags;
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06005292 RID: 21138 RVA: 0x00129D27 File Offset: 0x00128D27
		public AuditFlags AuditFlags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x04002AB1 RID: 10929
		private readonly AuditFlags _flags;
	}
}

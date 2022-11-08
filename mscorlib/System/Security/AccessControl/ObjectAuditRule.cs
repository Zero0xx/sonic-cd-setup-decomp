using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000908 RID: 2312
	public abstract class ObjectAuditRule : AuditRule
	{
		// Token: 0x060053BA RID: 21434 RVA: 0x0012DED4 File Offset: 0x0012CED4
		protected ObjectAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, Guid objectType, Guid inheritedObjectType, AuditFlags auditFlags) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, auditFlags)
		{
			if (!objectType.Equals(Guid.Empty) && (accessMask & ObjectAce.AccessMaskWithObjectType) != 0)
			{
				this._objectType = objectType;
				this._objectFlags |= ObjectAceFlags.ObjectAceTypePresent;
			}
			else
			{
				this._objectType = Guid.Empty;
			}
			if (!inheritedObjectType.Equals(Guid.Empty) && (inheritanceFlags & InheritanceFlags.ContainerInherit) != InheritanceFlags.None)
			{
				this._inheritedObjectType = inheritedObjectType;
				this._objectFlags |= ObjectAceFlags.InheritedObjectAceTypePresent;
				return;
			}
			this._inheritedObjectType = Guid.Empty;
		}

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x060053BB RID: 21435 RVA: 0x0012DF60 File Offset: 0x0012CF60
		public Guid ObjectType
		{
			get
			{
				return this._objectType;
			}
		}

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x060053BC RID: 21436 RVA: 0x0012DF68 File Offset: 0x0012CF68
		public Guid InheritedObjectType
		{
			get
			{
				return this._inheritedObjectType;
			}
		}

		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x060053BD RID: 21437 RVA: 0x0012DF70 File Offset: 0x0012CF70
		public ObjectAceFlags ObjectFlags
		{
			get
			{
				return this._objectFlags;
			}
		}

		// Token: 0x04002B62 RID: 11106
		private readonly Guid _objectType;

		// Token: 0x04002B63 RID: 11107
		private readonly Guid _inheritedObjectType;

		// Token: 0x04002B64 RID: 11108
		private readonly ObjectAceFlags _objectFlags;
	}
}

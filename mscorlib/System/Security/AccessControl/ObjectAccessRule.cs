using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x02000907 RID: 2311
	public abstract class ObjectAccessRule : AccessRule
	{
		// Token: 0x060053B6 RID: 21430 RVA: 0x0012DE30 File Offset: 0x0012CE30
		protected ObjectAccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, Guid objectType, Guid inheritedObjectType, AccessControlType type) : base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, type)
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

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x060053B7 RID: 21431 RVA: 0x0012DEBC File Offset: 0x0012CEBC
		public Guid ObjectType
		{
			get
			{
				return this._objectType;
			}
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x060053B8 RID: 21432 RVA: 0x0012DEC4 File Offset: 0x0012CEC4
		public Guid InheritedObjectType
		{
			get
			{
				return this._inheritedObjectType;
			}
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x060053B9 RID: 21433 RVA: 0x0012DECC File Offset: 0x0012CECC
		public ObjectAceFlags ObjectFlags
		{
			get
			{
				return this._objectFlags;
			}
		}

		// Token: 0x04002B5F RID: 11103
		private readonly Guid _objectType;

		// Token: 0x04002B60 RID: 11104
		private readonly Guid _inheritedObjectType;

		// Token: 0x04002B61 RID: 11105
		private readonly ObjectAceFlags _objectFlags;
	}
}

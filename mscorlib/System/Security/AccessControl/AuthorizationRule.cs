using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008DE RID: 2270
	public abstract class AuthorizationRule
	{
		// Token: 0x06005283 RID: 21123 RVA: 0x00129A30 File Offset: 0x00128A30
		protected internal AuthorizationRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			if (!identity.IsValidTargetType(typeof(SecurityIdentifier)))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeIdentityReferenceType"), "identity");
			}
			if (accessMask == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ArgumentZero"), "accessMask");
			}
			if (inheritanceFlags < InheritanceFlags.None || inheritanceFlags > (InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit))
			{
				throw new ArgumentOutOfRangeException("inheritanceFlags", Environment.GetResourceString("Argument_InvalidEnumValue", new object[]
				{
					inheritanceFlags,
					"InheritanceFlags"
				}));
			}
			if (propagationFlags < PropagationFlags.None || propagationFlags > (PropagationFlags.NoPropagateInherit | PropagationFlags.InheritOnly))
			{
				throw new ArgumentOutOfRangeException("propagationFlags", Environment.GetResourceString("Argument_InvalidEnumValue", new object[]
				{
					inheritanceFlags,
					"PropagationFlags"
				}));
			}
			this._identity = identity;
			this._accessMask = accessMask;
			this._isInherited = isInherited;
			this._inheritanceFlags = inheritanceFlags;
			if (inheritanceFlags != InheritanceFlags.None)
			{
				this._propagationFlags = propagationFlags;
				return;
			}
			this._propagationFlags = PropagationFlags.None;
		}

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06005284 RID: 21124 RVA: 0x00129B39 File Offset: 0x00128B39
		public IdentityReference IdentityReference
		{
			get
			{
				return this._identity;
			}
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06005285 RID: 21125 RVA: 0x00129B41 File Offset: 0x00128B41
		protected internal int AccessMask
		{
			get
			{
				return this._accessMask;
			}
		}

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06005286 RID: 21126 RVA: 0x00129B49 File Offset: 0x00128B49
		public bool IsInherited
		{
			get
			{
				return this._isInherited;
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x06005287 RID: 21127 RVA: 0x00129B51 File Offset: 0x00128B51
		public InheritanceFlags InheritanceFlags
		{
			get
			{
				return this._inheritanceFlags;
			}
		}

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x06005288 RID: 21128 RVA: 0x00129B59 File Offset: 0x00128B59
		public PropagationFlags PropagationFlags
		{
			get
			{
				return this._propagationFlags;
			}
		}

		// Token: 0x04002AAB RID: 10923
		private readonly IdentityReference _identity;

		// Token: 0x04002AAC RID: 10924
		private readonly int _accessMask;

		// Token: 0x04002AAD RID: 10925
		private readonly bool _isInherited;

		// Token: 0x04002AAE RID: 10926
		private readonly InheritanceFlags _inheritanceFlags;

		// Token: 0x04002AAF RID: 10927
		private readonly PropagationFlags _propagationFlags;
	}
}

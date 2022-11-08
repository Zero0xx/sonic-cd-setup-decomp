using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;

namespace System.Security.AccessControl
{
	// Token: 0x020008E3 RID: 2275
	public abstract class ObjectSecurity
	{
		// Token: 0x06005299 RID: 21145 RVA: 0x00129D7E File Offset: 0x00128D7E
		private ObjectSecurity()
		{
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x00129D94 File Offset: 0x00128D94
		protected ObjectSecurity(bool isContainer, bool isDS) : this()
		{
			DiscretionaryAcl discretionaryAcl = new DiscretionaryAcl(isContainer, isDS, 5);
			this._securityDescriptor = new CommonSecurityDescriptor(isContainer, isDS, ControlFlags.None, null, null, null, discretionaryAcl);
		}

		// Token: 0x0600529B RID: 21147 RVA: 0x00129DC2 File Offset: 0x00128DC2
		internal ObjectSecurity(CommonSecurityDescriptor securityDescriptor) : this()
		{
			if (securityDescriptor == null)
			{
				throw new ArgumentNullException("securityDescriptor");
			}
			this._securityDescriptor = securityDescriptor;
		}

		// Token: 0x0600529C RID: 21148 RVA: 0x00129DE0 File Offset: 0x00128DE0
		private void UpdateWithNewSecurityDescriptor(RawSecurityDescriptor newOne, AccessControlSections includeSections)
		{
			if ((includeSections & AccessControlSections.Owner) != AccessControlSections.None)
			{
				this._ownerModified = true;
				this._securityDescriptor.Owner = newOne.Owner;
			}
			if ((includeSections & AccessControlSections.Group) != AccessControlSections.None)
			{
				this._groupModified = true;
				this._securityDescriptor.Group = newOne.Group;
			}
			if ((includeSections & AccessControlSections.Audit) != AccessControlSections.None)
			{
				this._saclModified = true;
				if (newOne.SystemAcl != null)
				{
					this._securityDescriptor.SystemAcl = new SystemAcl(this.IsContainer, this.IsDS, newOne.SystemAcl, true);
				}
				else
				{
					this._securityDescriptor.SystemAcl = null;
				}
				this._securityDescriptor.UpdateControlFlags(ObjectSecurity.SACL_CONTROL_FLAGS, newOne.ControlFlags & ObjectSecurity.SACL_CONTROL_FLAGS);
			}
			if ((includeSections & AccessControlSections.Access) != AccessControlSections.None)
			{
				this._daclModified = true;
				if (newOne.DiscretionaryAcl != null)
				{
					this._securityDescriptor.DiscretionaryAcl = new DiscretionaryAcl(this.IsContainer, this.IsDS, newOne.DiscretionaryAcl, true);
				}
				else
				{
					this._securityDescriptor.DiscretionaryAcl = null;
				}
				ControlFlags controlFlags = this._securityDescriptor.ControlFlags & ControlFlags.DiscretionaryAclPresent;
				this._securityDescriptor.UpdateControlFlags(ObjectSecurity.DACL_CONTROL_FLAGS, (newOne.ControlFlags | controlFlags) & ObjectSecurity.DACL_CONTROL_FLAGS);
			}
		}

		// Token: 0x0600529D RID: 21149 RVA: 0x00129EF9 File Offset: 0x00128EF9
		protected void ReadLock()
		{
			this._lock.AcquireReaderLock(-1);
		}

		// Token: 0x0600529E RID: 21150 RVA: 0x00129F07 File Offset: 0x00128F07
		protected void ReadUnlock()
		{
			this._lock.ReleaseReaderLock();
		}

		// Token: 0x0600529F RID: 21151 RVA: 0x00129F14 File Offset: 0x00128F14
		protected void WriteLock()
		{
			this._lock.AcquireWriterLock(-1);
		}

		// Token: 0x060052A0 RID: 21152 RVA: 0x00129F22 File Offset: 0x00128F22
		protected void WriteUnlock()
		{
			this._lock.ReleaseWriterLock();
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x060052A1 RID: 21153 RVA: 0x00129F2F File Offset: 0x00128F2F
		// (set) Token: 0x060052A2 RID: 21154 RVA: 0x00129F61 File Offset: 0x00128F61
		protected bool OwnerModified
		{
			get
			{
				if (!this._lock.IsReaderLockHeld && !this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForReadOrWrite"));
				}
				return this._ownerModified;
			}
			set
			{
				if (!this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForWrite"));
				}
				this._ownerModified = value;
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x060052A3 RID: 21155 RVA: 0x00129F87 File Offset: 0x00128F87
		// (set) Token: 0x060052A4 RID: 21156 RVA: 0x00129FB9 File Offset: 0x00128FB9
		protected bool GroupModified
		{
			get
			{
				if (!this._lock.IsReaderLockHeld && !this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForReadOrWrite"));
				}
				return this._groupModified;
			}
			set
			{
				if (!this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForWrite"));
				}
				this._groupModified = value;
			}
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x060052A5 RID: 21157 RVA: 0x00129FDF File Offset: 0x00128FDF
		// (set) Token: 0x060052A6 RID: 21158 RVA: 0x0012A011 File Offset: 0x00129011
		protected bool AuditRulesModified
		{
			get
			{
				if (!this._lock.IsReaderLockHeld && !this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForReadOrWrite"));
				}
				return this._saclModified;
			}
			set
			{
				if (!this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForWrite"));
				}
				this._saclModified = value;
			}
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x060052A7 RID: 21159 RVA: 0x0012A037 File Offset: 0x00129037
		// (set) Token: 0x060052A8 RID: 21160 RVA: 0x0012A069 File Offset: 0x00129069
		protected bool AccessRulesModified
		{
			get
			{
				if (!this._lock.IsReaderLockHeld && !this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForReadOrWrite"));
				}
				return this._daclModified;
			}
			set
			{
				if (!this._lock.IsWriterLockHeld)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustLockForWrite"));
				}
				this._daclModified = value;
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x060052A9 RID: 21161 RVA: 0x0012A08F File Offset: 0x0012908F
		protected bool IsContainer
		{
			get
			{
				return this._securityDescriptor.IsContainer;
			}
		}

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x060052AA RID: 21162 RVA: 0x0012A09C File Offset: 0x0012909C
		protected bool IsDS
		{
			get
			{
				return this._securityDescriptor.IsDS;
			}
		}

		// Token: 0x060052AB RID: 21163 RVA: 0x0012A0A9 File Offset: 0x001290A9
		protected virtual void Persist(string name, AccessControlSections includeSections)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x0012A0B0 File Offset: 0x001290B0
		protected virtual void Persist(bool enableOwnershipPrivilege, string name, AccessControlSections includeSections)
		{
			Privilege privilege = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				if (enableOwnershipPrivilege)
				{
					privilege = new Privilege("SeTakeOwnershipPrivilege");
					try
					{
						privilege.Enable();
					}
					catch (PrivilegeNotHeldException)
					{
					}
				}
				this.Persist(name, includeSections);
			}
			catch
			{
				if (privilege != null)
				{
					privilege.Revert();
				}
				throw;
			}
			finally
			{
				if (privilege != null)
				{
					privilege.Revert();
				}
			}
		}

		// Token: 0x060052AD RID: 21165 RVA: 0x0012A128 File Offset: 0x00129128
		protected virtual void Persist(SafeHandle handle, AccessControlSections includeSections)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060052AE RID: 21166 RVA: 0x0012A130 File Offset: 0x00129130
		public IdentityReference GetOwner(Type targetType)
		{
			this.ReadLock();
			IdentityReference result;
			try
			{
				if (this._securityDescriptor.Owner == null)
				{
					result = null;
				}
				else
				{
					result = this._securityDescriptor.Owner.Translate(targetType);
				}
			}
			finally
			{
				this.ReadUnlock();
			}
			return result;
		}

		// Token: 0x060052AF RID: 21167 RVA: 0x0012A188 File Offset: 0x00129188
		public void SetOwner(IdentityReference identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			this.WriteLock();
			try
			{
				this._securityDescriptor.Owner = (identity.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier);
				this._ownerModified = true;
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x060052B0 RID: 21168 RVA: 0x0012A1F0 File Offset: 0x001291F0
		public IdentityReference GetGroup(Type targetType)
		{
			this.ReadLock();
			IdentityReference result;
			try
			{
				if (this._securityDescriptor.Group == null)
				{
					result = null;
				}
				else
				{
					result = this._securityDescriptor.Group.Translate(targetType);
				}
			}
			finally
			{
				this.ReadUnlock();
			}
			return result;
		}

		// Token: 0x060052B1 RID: 21169 RVA: 0x0012A248 File Offset: 0x00129248
		public void SetGroup(IdentityReference identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			this.WriteLock();
			try
			{
				this._securityDescriptor.Group = (identity.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier);
				this._groupModified = true;
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x060052B2 RID: 21170 RVA: 0x0012A2B0 File Offset: 0x001292B0
		public virtual void PurgeAccessRules(IdentityReference identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			this.WriteLock();
			try
			{
				this._securityDescriptor.PurgeAccessControl(identity.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier);
				this._daclModified = true;
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x060052B3 RID: 21171 RVA: 0x0012A318 File Offset: 0x00129318
		public virtual void PurgeAuditRules(IdentityReference identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			this.WriteLock();
			try
			{
				this._securityDescriptor.PurgeAudit(identity.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier);
				this._saclModified = true;
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x060052B4 RID: 21172 RVA: 0x0012A380 File Offset: 0x00129380
		public bool AreAccessRulesProtected
		{
			get
			{
				this.ReadLock();
				bool result;
				try
				{
					result = ((this._securityDescriptor.ControlFlags & ControlFlags.DiscretionaryAclProtected) != ControlFlags.None);
				}
				finally
				{
					this.ReadUnlock();
				}
				return result;
			}
		}

		// Token: 0x060052B5 RID: 21173 RVA: 0x0012A3C8 File Offset: 0x001293C8
		public void SetAccessRuleProtection(bool isProtected, bool preserveInheritance)
		{
			this.WriteLock();
			try
			{
				this._securityDescriptor.SetDiscretionaryAclProtection(isProtected, preserveInheritance);
				this._daclModified = true;
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x060052B6 RID: 21174 RVA: 0x0012A408 File Offset: 0x00129408
		public bool AreAuditRulesProtected
		{
			get
			{
				this.ReadLock();
				bool result;
				try
				{
					result = ((this._securityDescriptor.ControlFlags & ControlFlags.SystemAclProtected) != ControlFlags.None);
				}
				finally
				{
					this.ReadUnlock();
				}
				return result;
			}
		}

		// Token: 0x060052B7 RID: 21175 RVA: 0x0012A450 File Offset: 0x00129450
		public void SetAuditRuleProtection(bool isProtected, bool preserveInheritance)
		{
			this.WriteLock();
			try
			{
				this._securityDescriptor.SetSystemAclProtection(isProtected, preserveInheritance);
				this._saclModified = true;
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x060052B8 RID: 21176 RVA: 0x0012A490 File Offset: 0x00129490
		public bool AreAccessRulesCanonical
		{
			get
			{
				this.ReadLock();
				bool isDiscretionaryAclCanonical;
				try
				{
					isDiscretionaryAclCanonical = this._securityDescriptor.IsDiscretionaryAclCanonical;
				}
				finally
				{
					this.ReadUnlock();
				}
				return isDiscretionaryAclCanonical;
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x060052B9 RID: 21177 RVA: 0x0012A4CC File Offset: 0x001294CC
		public bool AreAuditRulesCanonical
		{
			get
			{
				this.ReadLock();
				bool isSystemAclCanonical;
				try
				{
					isSystemAclCanonical = this._securityDescriptor.IsSystemAclCanonical;
				}
				finally
				{
					this.ReadUnlock();
				}
				return isSystemAclCanonical;
			}
		}

		// Token: 0x060052BA RID: 21178 RVA: 0x0012A508 File Offset: 0x00129508
		public static bool IsSddlConversionSupported()
		{
			return Win32.IsSddlConversionSupported();
		}

		// Token: 0x060052BB RID: 21179 RVA: 0x0012A510 File Offset: 0x00129510
		public string GetSecurityDescriptorSddlForm(AccessControlSections includeSections)
		{
			this.ReadLock();
			string sddlForm;
			try
			{
				sddlForm = this._securityDescriptor.GetSddlForm(includeSections);
			}
			finally
			{
				this.ReadUnlock();
			}
			return sddlForm;
		}

		// Token: 0x060052BC RID: 21180 RVA: 0x0012A54C File Offset: 0x0012954C
		public void SetSecurityDescriptorSddlForm(string sddlForm)
		{
			this.SetSecurityDescriptorSddlForm(sddlForm, AccessControlSections.All);
		}

		// Token: 0x060052BD RID: 21181 RVA: 0x0012A558 File Offset: 0x00129558
		public void SetSecurityDescriptorSddlForm(string sddlForm, AccessControlSections includeSections)
		{
			if (sddlForm == null)
			{
				throw new ArgumentNullException("sddlForm");
			}
			if ((includeSections & AccessControlSections.All) == AccessControlSections.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "includeSections");
			}
			this.WriteLock();
			try
			{
				this.UpdateWithNewSecurityDescriptor(new RawSecurityDescriptor(sddlForm), includeSections);
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x060052BE RID: 21182 RVA: 0x0012A5BC File Offset: 0x001295BC
		public byte[] GetSecurityDescriptorBinaryForm()
		{
			this.ReadLock();
			byte[] result;
			try
			{
				byte[] array = new byte[this._securityDescriptor.BinaryLength];
				this._securityDescriptor.GetBinaryForm(array, 0);
				result = array;
			}
			finally
			{
				this.ReadUnlock();
			}
			return result;
		}

		// Token: 0x060052BF RID: 21183 RVA: 0x0012A60C File Offset: 0x0012960C
		public void SetSecurityDescriptorBinaryForm(byte[] binaryForm)
		{
			this.SetSecurityDescriptorBinaryForm(binaryForm, AccessControlSections.All);
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x0012A618 File Offset: 0x00129618
		public void SetSecurityDescriptorBinaryForm(byte[] binaryForm, AccessControlSections includeSections)
		{
			if (binaryForm == null)
			{
				throw new ArgumentNullException("binaryForm");
			}
			if ((includeSections & AccessControlSections.All) == AccessControlSections.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "includeSections");
			}
			this.WriteLock();
			try
			{
				this.UpdateWithNewSecurityDescriptor(new RawSecurityDescriptor(binaryForm, 0), includeSections);
			}
			finally
			{
				this.WriteUnlock();
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x060052C1 RID: 21185
		public abstract Type AccessRightType { get; }

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x060052C2 RID: 21186
		public abstract Type AccessRuleType { get; }

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x060052C3 RID: 21187
		public abstract Type AuditRuleType { get; }

		// Token: 0x060052C4 RID: 21188
		protected abstract bool ModifyAccess(AccessControlModification modification, AccessRule rule, out bool modified);

		// Token: 0x060052C5 RID: 21189
		protected abstract bool ModifyAudit(AccessControlModification modification, AuditRule rule, out bool modified);

		// Token: 0x060052C6 RID: 21190 RVA: 0x0012A67C File Offset: 0x0012967C
		public virtual bool ModifyAccessRule(AccessControlModification modification, AccessRule rule, out bool modified)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			if (!this.AccessRuleType.IsAssignableFrom(rule.GetType()))
			{
				throw new ArgumentException(Environment.GetResourceString("AccessControl_InvalidAccessRuleType"), "rule");
			}
			this.WriteLock();
			bool result;
			try
			{
				result = this.ModifyAccess(modification, rule, out modified);
			}
			finally
			{
				this.WriteUnlock();
			}
			return result;
		}

		// Token: 0x060052C7 RID: 21191 RVA: 0x0012A6EC File Offset: 0x001296EC
		public virtual bool ModifyAuditRule(AccessControlModification modification, AuditRule rule, out bool modified)
		{
			if (rule == null)
			{
				throw new ArgumentNullException("rule");
			}
			if (!this.AuditRuleType.IsAssignableFrom(rule.GetType()))
			{
				throw new ArgumentException(Environment.GetResourceString("AccessControl_InvalidAuditRuleType"), "rule");
			}
			this.WriteLock();
			bool result;
			try
			{
				result = this.ModifyAudit(modification, rule, out modified);
			}
			finally
			{
				this.WriteUnlock();
			}
			return result;
		}

		// Token: 0x060052C8 RID: 21192
		public abstract AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type);

		// Token: 0x060052C9 RID: 21193
		public abstract AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags);

		// Token: 0x04002AB2 RID: 10930
		private readonly ReaderWriterLock _lock = new ReaderWriterLock();

		// Token: 0x04002AB3 RID: 10931
		internal CommonSecurityDescriptor _securityDescriptor;

		// Token: 0x04002AB4 RID: 10932
		private bool _ownerModified;

		// Token: 0x04002AB5 RID: 10933
		private bool _groupModified;

		// Token: 0x04002AB6 RID: 10934
		private bool _saclModified;

		// Token: 0x04002AB7 RID: 10935
		private bool _daclModified;

		// Token: 0x04002AB8 RID: 10936
		private static readonly ControlFlags SACL_CONTROL_FLAGS = ControlFlags.SystemAclPresent | ControlFlags.SystemAclAutoInherited | ControlFlags.SystemAclProtected;

		// Token: 0x04002AB9 RID: 10937
		private static readonly ControlFlags DACL_CONTROL_FLAGS = ControlFlags.DiscretionaryAclPresent | ControlFlags.DiscretionaryAclAutoInherited | ControlFlags.DiscretionaryAclProtected;
	}
}

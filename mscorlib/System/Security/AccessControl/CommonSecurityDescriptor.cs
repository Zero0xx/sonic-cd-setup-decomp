using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200090D RID: 2317
	public sealed class CommonSecurityDescriptor : GenericSecurityDescriptor
	{
		// Token: 0x060053E5 RID: 21477 RVA: 0x0012E618 File Offset: 0x0012D618
		private void CreateFromParts(bool isContainer, bool isDS, ControlFlags flags, SecurityIdentifier owner, SecurityIdentifier group, SystemAcl systemAcl, DiscretionaryAcl discretionaryAcl)
		{
			if (systemAcl != null && systemAcl.IsContainer != isContainer)
			{
				throw new ArgumentException(Environment.GetResourceString(isContainer ? "AccessControl_MustSpecifyContainerAcl" : "AccessControl_MustSpecifyLeafObjectAcl"), "systemAcl");
			}
			if (discretionaryAcl != null && discretionaryAcl.IsContainer != isContainer)
			{
				throw new ArgumentException(Environment.GetResourceString(isContainer ? "AccessControl_MustSpecifyContainerAcl" : "AccessControl_MustSpecifyLeafObjectAcl"), "discretionaryAcl");
			}
			this._isContainer = isContainer;
			if (systemAcl != null && systemAcl.IsDS != isDS)
			{
				throw new ArgumentException(Environment.GetResourceString(isDS ? "AccessControl_MustSpecifyDirectoryObjectAcl" : "AccessControl_MustSpecifyNonDirectoryObjectAcl"), "systemAcl");
			}
			if (discretionaryAcl != null && discretionaryAcl.IsDS != isDS)
			{
				throw new ArgumentException(Environment.GetResourceString(isDS ? "AccessControl_MustSpecifyDirectoryObjectAcl" : "AccessControl_MustSpecifyNonDirectoryObjectAcl"), "discretionaryAcl");
			}
			this._isDS = isDS;
			this._sacl = systemAcl;
			if (discretionaryAcl == null)
			{
				discretionaryAcl = DiscretionaryAcl.CreateAllowEveryoneFullAccess(this._isDS, this._isContainer);
			}
			this._dacl = discretionaryAcl;
			ControlFlags controlFlags = flags | ControlFlags.DiscretionaryAclPresent;
			if (systemAcl == null)
			{
				controlFlags &= ~ControlFlags.SystemAclPresent;
			}
			else
			{
				controlFlags |= ControlFlags.SystemAclPresent;
			}
			this._rawSd = new RawSecurityDescriptor(controlFlags, owner, group, (systemAcl == null) ? null : systemAcl.RawAcl, discretionaryAcl.RawAcl);
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x0012E747 File Offset: 0x0012D747
		public CommonSecurityDescriptor(bool isContainer, bool isDS, ControlFlags flags, SecurityIdentifier owner, SecurityIdentifier group, SystemAcl systemAcl, DiscretionaryAcl discretionaryAcl)
		{
			this.CreateFromParts(isContainer, isDS, flags, owner, group, systemAcl, discretionaryAcl);
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x0012E760 File Offset: 0x0012D760
		private CommonSecurityDescriptor(bool isContainer, bool isDS, ControlFlags flags, SecurityIdentifier owner, SecurityIdentifier group, RawAcl systemAcl, RawAcl discretionaryAcl) : this(isContainer, isDS, flags, owner, group, (systemAcl == null) ? null : new SystemAcl(isContainer, isDS, systemAcl), (discretionaryAcl == null) ? null : new DiscretionaryAcl(isContainer, isDS, discretionaryAcl))
		{
		}

		// Token: 0x060053E8 RID: 21480 RVA: 0x0012E79A File Offset: 0x0012D79A
		public CommonSecurityDescriptor(bool isContainer, bool isDS, RawSecurityDescriptor rawSecurityDescriptor) : this(isContainer, isDS, rawSecurityDescriptor, false)
		{
		}

		// Token: 0x060053E9 RID: 21481 RVA: 0x0012E7A8 File Offset: 0x0012D7A8
		internal CommonSecurityDescriptor(bool isContainer, bool isDS, RawSecurityDescriptor rawSecurityDescriptor, bool trusted)
		{
			if (rawSecurityDescriptor == null)
			{
				throw new ArgumentNullException("rawSecurityDescriptor");
			}
			this.CreateFromParts(isContainer, isDS, rawSecurityDescriptor.ControlFlags, rawSecurityDescriptor.Owner, rawSecurityDescriptor.Group, (rawSecurityDescriptor.SystemAcl == null) ? null : new SystemAcl(isContainer, isDS, rawSecurityDescriptor.SystemAcl, trusted), (rawSecurityDescriptor.DiscretionaryAcl == null) ? null : new DiscretionaryAcl(isContainer, isDS, rawSecurityDescriptor.DiscretionaryAcl, trusted));
		}

		// Token: 0x060053EA RID: 21482 RVA: 0x0012E817 File Offset: 0x0012D817
		public CommonSecurityDescriptor(bool isContainer, bool isDS, string sddlForm) : this(isContainer, isDS, new RawSecurityDescriptor(sddlForm), true)
		{
		}

		// Token: 0x060053EB RID: 21483 RVA: 0x0012E828 File Offset: 0x0012D828
		public CommonSecurityDescriptor(bool isContainer, bool isDS, byte[] binaryForm, int offset) : this(isContainer, isDS, new RawSecurityDescriptor(binaryForm, offset), true)
		{
		}

		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x060053EC RID: 21484 RVA: 0x0012E83B File Offset: 0x0012D83B
		internal sealed override GenericAcl GenericSacl
		{
			get
			{
				return this._sacl;
			}
		}

		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x060053ED RID: 21485 RVA: 0x0012E843 File Offset: 0x0012D843
		internal sealed override GenericAcl GenericDacl
		{
			get
			{
				return this._dacl;
			}
		}

		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x060053EE RID: 21486 RVA: 0x0012E84B File Offset: 0x0012D84B
		public bool IsContainer
		{
			get
			{
				return this._isContainer;
			}
		}

		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x060053EF RID: 21487 RVA: 0x0012E853 File Offset: 0x0012D853
		public bool IsDS
		{
			get
			{
				return this._isDS;
			}
		}

		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x060053F0 RID: 21488 RVA: 0x0012E85B File Offset: 0x0012D85B
		public override ControlFlags ControlFlags
		{
			get
			{
				return this._rawSd.ControlFlags;
			}
		}

		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x060053F1 RID: 21489 RVA: 0x0012E868 File Offset: 0x0012D868
		// (set) Token: 0x060053F2 RID: 21490 RVA: 0x0012E875 File Offset: 0x0012D875
		public override SecurityIdentifier Owner
		{
			get
			{
				return this._rawSd.Owner;
			}
			set
			{
				this._rawSd.Owner = value;
			}
		}

		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x060053F3 RID: 21491 RVA: 0x0012E883 File Offset: 0x0012D883
		// (set) Token: 0x060053F4 RID: 21492 RVA: 0x0012E890 File Offset: 0x0012D890
		public override SecurityIdentifier Group
		{
			get
			{
				return this._rawSd.Group;
			}
			set
			{
				this._rawSd.Group = value;
			}
		}

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x060053F5 RID: 21493 RVA: 0x0012E89E File Offset: 0x0012D89E
		// (set) Token: 0x060053F6 RID: 21494 RVA: 0x0012E8A8 File Offset: 0x0012D8A8
		public SystemAcl SystemAcl
		{
			get
			{
				return this._sacl;
			}
			set
			{
				if (value != null)
				{
					if (value.IsContainer != this.IsContainer)
					{
						throw new ArgumentException(Environment.GetResourceString(this.IsContainer ? "AccessControl_MustSpecifyContainerAcl" : "AccessControl_MustSpecifyLeafObjectAcl"), "value");
					}
					if (value.IsDS != this.IsDS)
					{
						throw new ArgumentException(Environment.GetResourceString(this.IsDS ? "AccessControl_MustSpecifyDirectoryObjectAcl" : "AccessControl_MustSpecifyNonDirectoryObjectAcl"), "value");
					}
				}
				this._sacl = value;
				if (this._sacl != null)
				{
					this._rawSd.SystemAcl = this._sacl.RawAcl;
					this.AddControlFlags(ControlFlags.SystemAclPresent);
					return;
				}
				this._rawSd.SystemAcl = null;
				this.RemoveControlFlags(ControlFlags.SystemAclPresent);
			}
		}

		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x060053F7 RID: 21495 RVA: 0x0012E95E File Offset: 0x0012D95E
		// (set) Token: 0x060053F8 RID: 21496 RVA: 0x0012E968 File Offset: 0x0012D968
		public DiscretionaryAcl DiscretionaryAcl
		{
			get
			{
				return this._dacl;
			}
			set
			{
				if (value != null)
				{
					if (value.IsContainer != this.IsContainer)
					{
						throw new ArgumentException(Environment.GetResourceString(this.IsContainer ? "AccessControl_MustSpecifyContainerAcl" : "AccessControl_MustSpecifyLeafObjectAcl"), "value");
					}
					if (value.IsDS != this.IsDS)
					{
						throw new ArgumentException(Environment.GetResourceString(this.IsDS ? "AccessControl_MustSpecifyDirectoryObjectAcl" : "AccessControl_MustSpecifyNonDirectoryObjectAcl"), "value");
					}
				}
				if (value == null)
				{
					this._dacl = DiscretionaryAcl.CreateAllowEveryoneFullAccess(this.IsDS, this.IsContainer);
				}
				else
				{
					this._dacl = value;
				}
				this._rawSd.DiscretionaryAcl = this._dacl.RawAcl;
				this.AddControlFlags(ControlFlags.DiscretionaryAclPresent);
			}
		}

		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x060053F9 RID: 21497 RVA: 0x0012EA1C File Offset: 0x0012DA1C
		public bool IsSystemAclCanonical
		{
			get
			{
				return this.SystemAcl == null || this.SystemAcl.IsCanonical;
			}
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x060053FA RID: 21498 RVA: 0x0012EA33 File Offset: 0x0012DA33
		public bool IsDiscretionaryAclCanonical
		{
			get
			{
				return this.DiscretionaryAcl == null || this.DiscretionaryAcl.IsCanonical;
			}
		}

		// Token: 0x060053FB RID: 21499 RVA: 0x0012EA4A File Offset: 0x0012DA4A
		public void SetSystemAclProtection(bool isProtected, bool preserveInheritance)
		{
			if (!isProtected)
			{
				this.RemoveControlFlags(ControlFlags.SystemAclProtected);
				return;
			}
			if (!preserveInheritance && this.SystemAcl != null)
			{
				this.SystemAcl.RemoveInheritedAces();
			}
			this.AddControlFlags(ControlFlags.SystemAclProtected);
		}

		// Token: 0x060053FC RID: 21500 RVA: 0x0012EA7C File Offset: 0x0012DA7C
		public void SetDiscretionaryAclProtection(bool isProtected, bool preserveInheritance)
		{
			if (!isProtected)
			{
				this.RemoveControlFlags(ControlFlags.DiscretionaryAclProtected);
			}
			else
			{
				if (!preserveInheritance && this.DiscretionaryAcl != null)
				{
					this.DiscretionaryAcl.RemoveInheritedAces();
				}
				this.AddControlFlags(ControlFlags.DiscretionaryAclProtected);
			}
			if (this.DiscretionaryAcl != null && this.DiscretionaryAcl.EveryOneFullAccessForNullDacl)
			{
				this.DiscretionaryAcl.EveryOneFullAccessForNullDacl = false;
			}
		}

		// Token: 0x060053FD RID: 21501 RVA: 0x0012EADB File Offset: 0x0012DADB
		public void PurgeAccessControl(SecurityIdentifier sid)
		{
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			if (this.DiscretionaryAcl != null)
			{
				this.DiscretionaryAcl.Purge(sid);
			}
		}

		// Token: 0x060053FE RID: 21502 RVA: 0x0012EB05 File Offset: 0x0012DB05
		public void PurgeAudit(SecurityIdentifier sid)
		{
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			if (this.SystemAcl != null)
			{
				this.SystemAcl.Purge(sid);
			}
		}

		// Token: 0x060053FF RID: 21503 RVA: 0x0012EB30 File Offset: 0x0012DB30
		internal void UpdateControlFlags(ControlFlags flagsToUpdate, ControlFlags newFlags)
		{
			ControlFlags flags = newFlags | (this._rawSd.ControlFlags & ~flagsToUpdate);
			this._rawSd.SetFlags(flags);
		}

		// Token: 0x06005400 RID: 21504 RVA: 0x0012EB5A File Offset: 0x0012DB5A
		internal void AddControlFlags(ControlFlags flags)
		{
			this._rawSd.SetFlags(this._rawSd.ControlFlags | flags);
		}

		// Token: 0x06005401 RID: 21505 RVA: 0x0012EB74 File Offset: 0x0012DB74
		internal void RemoveControlFlags(ControlFlags flags)
		{
			this._rawSd.SetFlags(this._rawSd.ControlFlags & ~flags);
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x06005402 RID: 21506 RVA: 0x0012EB8F File Offset: 0x0012DB8F
		internal bool IsSystemAclPresent
		{
			get
			{
				return (this._rawSd.ControlFlags & ControlFlags.SystemAclPresent) != ControlFlags.None;
			}
		}

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x06005403 RID: 21507 RVA: 0x0012EBA5 File Offset: 0x0012DBA5
		internal bool IsDiscretionaryAclPresent
		{
			get
			{
				return (this._rawSd.ControlFlags & ControlFlags.DiscretionaryAclPresent) != ControlFlags.None;
			}
		}

		// Token: 0x04002B82 RID: 11138
		private bool _isContainer;

		// Token: 0x04002B83 RID: 11139
		private bool _isDS;

		// Token: 0x04002B84 RID: 11140
		private RawSecurityDescriptor _rawSd;

		// Token: 0x04002B85 RID: 11141
		private SystemAcl _sacl;

		// Token: 0x04002B86 RID: 11142
		private DiscretionaryAcl _dacl;
	}
}

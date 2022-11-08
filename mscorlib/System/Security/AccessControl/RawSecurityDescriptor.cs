using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32;

namespace System.Security.AccessControl
{
	// Token: 0x0200090C RID: 2316
	public sealed class RawSecurityDescriptor : GenericSecurityDescriptor
	{
		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x060053D2 RID: 21458 RVA: 0x0012E307 File Offset: 0x0012D307
		internal override GenericAcl GenericSacl
		{
			get
			{
				return this._sacl;
			}
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x060053D3 RID: 21459 RVA: 0x0012E30F File Offset: 0x0012D30F
		internal override GenericAcl GenericDacl
		{
			get
			{
				return this._dacl;
			}
		}

		// Token: 0x060053D4 RID: 21460 RVA: 0x0012E317 File Offset: 0x0012D317
		private void CreateFromParts(ControlFlags flags, SecurityIdentifier owner, SecurityIdentifier group, RawAcl systemAcl, RawAcl discretionaryAcl)
		{
			this.SetFlags(flags);
			this.Owner = owner;
			this.Group = group;
			this.SystemAcl = systemAcl;
			this.DiscretionaryAcl = discretionaryAcl;
			this.ResourceManagerControl = 0;
		}

		// Token: 0x060053D5 RID: 21461 RVA: 0x0012E345 File Offset: 0x0012D345
		public RawSecurityDescriptor(ControlFlags flags, SecurityIdentifier owner, SecurityIdentifier group, RawAcl systemAcl, RawAcl discretionaryAcl)
		{
			this.CreateFromParts(flags, owner, group, systemAcl, discretionaryAcl);
		}

		// Token: 0x060053D6 RID: 21462 RVA: 0x0012E35A File Offset: 0x0012D35A
		public RawSecurityDescriptor(string sddlForm) : this(RawSecurityDescriptor.BinaryFormFromSddlForm(sddlForm), 0)
		{
		}

		// Token: 0x060053D7 RID: 21463 RVA: 0x0012E36C File Offset: 0x0012D36C
		public RawSecurityDescriptor(byte[] binaryForm, int offset)
		{
			if (binaryForm == null)
			{
				throw new ArgumentNullException("binaryForm");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (binaryForm.Length - offset < 20)
			{
				throw new ArgumentOutOfRangeException("binaryForm", Environment.GetResourceString("ArgumentOutOfRange_ArrayTooSmall"));
			}
			if (binaryForm[offset] != GenericSecurityDescriptor.Revision)
			{
				throw new ArgumentOutOfRangeException("binaryForm", Environment.GetResourceString("AccessControl_InvalidSecurityDescriptorRevision"));
			}
			byte resourceManagerControl = binaryForm[offset + 1];
			ControlFlags controlFlags = (ControlFlags)((int)binaryForm[offset + 2] + ((int)binaryForm[offset + 3] << 8));
			if ((controlFlags & ControlFlags.SelfRelative) == ControlFlags.None)
			{
				throw new ArgumentException(Environment.GetResourceString("AccessControl_InvalidSecurityDescriptorSelfRelativeForm"), "binaryForm");
			}
			int num = GenericSecurityDescriptor.UnmarshalInt(binaryForm, offset + 4);
			SecurityIdentifier owner;
			if (num != 0)
			{
				owner = new SecurityIdentifier(binaryForm, offset + num);
			}
			else
			{
				owner = null;
			}
			int num2 = GenericSecurityDescriptor.UnmarshalInt(binaryForm, offset + 8);
			SecurityIdentifier group;
			if (num2 != 0)
			{
				group = new SecurityIdentifier(binaryForm, offset + num2);
			}
			else
			{
				group = null;
			}
			int num3 = GenericSecurityDescriptor.UnmarshalInt(binaryForm, offset + 12);
			RawAcl systemAcl;
			if ((controlFlags & ControlFlags.SystemAclPresent) != ControlFlags.None && num3 != 0)
			{
				systemAcl = new RawAcl(binaryForm, offset + num3);
			}
			else
			{
				systemAcl = null;
			}
			int num4 = GenericSecurityDescriptor.UnmarshalInt(binaryForm, offset + 16);
			RawAcl discretionaryAcl;
			if ((controlFlags & ControlFlags.DiscretionaryAclPresent) != ControlFlags.None && num4 != 0)
			{
				discretionaryAcl = new RawAcl(binaryForm, offset + num4);
			}
			else
			{
				discretionaryAcl = null;
			}
			this.CreateFromParts(controlFlags, owner, group, systemAcl, discretionaryAcl);
			if ((controlFlags & ControlFlags.RMControlValid) != ControlFlags.None)
			{
				this.ResourceManagerControl = resourceManagerControl;
			}
		}

		// Token: 0x060053D8 RID: 21464 RVA: 0x0012E4BC File Offset: 0x0012D4BC
		private static byte[] BinaryFormFromSddlForm(string sddlForm)
		{
			if (!GenericSecurityDescriptor.IsSddlConversionSupported())
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			if (sddlForm == null)
			{
				throw new ArgumentNullException("sddlForm");
			}
			IntPtr zero = IntPtr.Zero;
			uint num = 0U;
			byte[] array = null;
			try
			{
				if (1 != Win32Native.ConvertStringSdToSd(sddlForm, (uint)GenericSecurityDescriptor.Revision, out zero, ref num))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error == 87 || lastWin32Error == 1336 || lastWin32Error == 1338 || lastWin32Error == 1305)
					{
						throw new ArgumentException(Environment.GetResourceString("ArgumentException_InvalidSDSddlForm"), "sddlForm");
					}
					if (lastWin32Error == 8)
					{
						throw new OutOfMemoryException();
					}
					if (lastWin32Error == 1337)
					{
						throw new ArgumentException(Environment.GetResourceString("AccessControl_InvalidSidInSDDLString"), "sddlForm");
					}
					if (lastWin32Error != 0)
					{
						throw new SystemException();
					}
				}
				array = new byte[num];
				Marshal.Copy(zero, array, 0, (int)num);
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Win32Native.LocalFree(zero);
				}
			}
			return array;
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x060053D9 RID: 21465 RVA: 0x0012E5AC File Offset: 0x0012D5AC
		public override ControlFlags ControlFlags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x060053DA RID: 21466 RVA: 0x0012E5B4 File Offset: 0x0012D5B4
		// (set) Token: 0x060053DB RID: 21467 RVA: 0x0012E5BC File Offset: 0x0012D5BC
		public override SecurityIdentifier Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				this._owner = value;
			}
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x060053DC RID: 21468 RVA: 0x0012E5C5 File Offset: 0x0012D5C5
		// (set) Token: 0x060053DD RID: 21469 RVA: 0x0012E5CD File Offset: 0x0012D5CD
		public override SecurityIdentifier Group
		{
			get
			{
				return this._group;
			}
			set
			{
				this._group = value;
			}
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x060053DE RID: 21470 RVA: 0x0012E5D6 File Offset: 0x0012D5D6
		// (set) Token: 0x060053DF RID: 21471 RVA: 0x0012E5DE File Offset: 0x0012D5DE
		public RawAcl SystemAcl
		{
			get
			{
				return this._sacl;
			}
			set
			{
				this._sacl = value;
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x060053E0 RID: 21472 RVA: 0x0012E5E7 File Offset: 0x0012D5E7
		// (set) Token: 0x060053E1 RID: 21473 RVA: 0x0012E5EF File Offset: 0x0012D5EF
		public RawAcl DiscretionaryAcl
		{
			get
			{
				return this._dacl;
			}
			set
			{
				this._dacl = value;
			}
		}

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x060053E2 RID: 21474 RVA: 0x0012E5F8 File Offset: 0x0012D5F8
		// (set) Token: 0x060053E3 RID: 21475 RVA: 0x0012E600 File Offset: 0x0012D600
		public byte ResourceManagerControl
		{
			get
			{
				return this._rmControl;
			}
			set
			{
				this._rmControl = value;
			}
		}

		// Token: 0x060053E4 RID: 21476 RVA: 0x0012E609 File Offset: 0x0012D609
		public void SetFlags(ControlFlags flags)
		{
			this._flags = (flags | ControlFlags.SelfRelative);
		}

		// Token: 0x04002B7C RID: 11132
		private SecurityIdentifier _owner;

		// Token: 0x04002B7D RID: 11133
		private SecurityIdentifier _group;

		// Token: 0x04002B7E RID: 11134
		private ControlFlags _flags;

		// Token: 0x04002B7F RID: 11135
		private RawAcl _sacl;

		// Token: 0x04002B80 RID: 11136
		private RawAcl _dacl;

		// Token: 0x04002B81 RID: 11137
		private byte _rmControl;
	}
}

using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200090B RID: 2315
	public abstract class GenericSecurityDescriptor
	{
		// Token: 0x060053C2 RID: 21442 RVA: 0x0012DFAC File Offset: 0x0012CFAC
		private static void MarshalInt(byte[] binaryForm, int offset, int number)
		{
			binaryForm[offset] = (byte)number;
			binaryForm[offset + 1] = (byte)(number >> 8);
			binaryForm[offset + 2] = (byte)(number >> 16);
			binaryForm[offset + 3] = (byte)(number >> 24);
		}

		// Token: 0x060053C3 RID: 21443 RVA: 0x0012DFD0 File Offset: 0x0012CFD0
		internal static int UnmarshalInt(byte[] binaryForm, int offset)
		{
			return (int)binaryForm[offset] + ((int)binaryForm[offset + 1] << 8) + ((int)binaryForm[offset + 2] << 16) + ((int)binaryForm[offset + 3] << 24);
		}

		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x060053C5 RID: 21445
		internal abstract GenericAcl GenericSacl { get; }

		// Token: 0x17000E93 RID: 3731
		// (get) Token: 0x060053C6 RID: 21446
		internal abstract GenericAcl GenericDacl { get; }

		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x060053C7 RID: 21447 RVA: 0x0012DFF7 File Offset: 0x0012CFF7
		private bool IsCraftedAefaDacl
		{
			get
			{
				return this.GenericDacl is DiscretionaryAcl && (this.GenericDacl as DiscretionaryAcl).EveryOneFullAccessForNullDacl;
			}
		}

		// Token: 0x060053C8 RID: 21448 RVA: 0x0012E018 File Offset: 0x0012D018
		public static bool IsSddlConversionSupported()
		{
			return Win32.IsSddlConversionSupported();
		}

		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x060053C9 RID: 21449 RVA: 0x0012E01F File Offset: 0x0012D01F
		public static byte Revision
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x060053CA RID: 21450
		public abstract ControlFlags ControlFlags { get; }

		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x060053CB RID: 21451
		// (set) Token: 0x060053CC RID: 21452
		public abstract SecurityIdentifier Owner { get; set; }

		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x060053CD RID: 21453
		// (set) Token: 0x060053CE RID: 21454
		public abstract SecurityIdentifier Group { get; set; }

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x060053CF RID: 21455 RVA: 0x0012E024 File Offset: 0x0012D024
		public int BinaryLength
		{
			get
			{
				int num = 20;
				if (this.Owner != null)
				{
					num += this.Owner.BinaryLength;
				}
				if (this.Group != null)
				{
					num += this.Group.BinaryLength;
				}
				if ((this.ControlFlags & ControlFlags.SystemAclPresent) != ControlFlags.None && this.GenericSacl != null)
				{
					num += this.GenericSacl.BinaryLength;
				}
				if ((this.ControlFlags & ControlFlags.DiscretionaryAclPresent) != ControlFlags.None && this.GenericDacl != null && !this.IsCraftedAefaDacl)
				{
					num += this.GenericDacl.BinaryLength;
				}
				return num;
			}
		}

		// Token: 0x060053D0 RID: 21456 RVA: 0x0012E0B8 File Offset: 0x0012D0B8
		public string GetSddlForm(AccessControlSections includeSections)
		{
			byte[] binaryForm = new byte[this.BinaryLength];
			this.GetBinaryForm(binaryForm, 0);
			SecurityInfos securityInfos = (SecurityInfos)0;
			if ((includeSections & AccessControlSections.Owner) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.Owner;
			}
			if ((includeSections & AccessControlSections.Group) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.Group;
			}
			if ((includeSections & AccessControlSections.Audit) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.SystemAcl;
			}
			if ((includeSections & AccessControlSections.Access) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.DiscretionaryAcl;
			}
			string result;
			int num = Win32.ConvertSdToSddl(binaryForm, 1, securityInfos, out result);
			if (num == 87 || num == 1305)
			{
				throw new InvalidOperationException();
			}
			if (num != 0)
			{
				throw new InvalidOperationException();
			}
			return result;
		}

		// Token: 0x060053D1 RID: 21457 RVA: 0x0012E128 File Offset: 0x0012D128
		public void GetBinaryForm(byte[] binaryForm, int offset)
		{
			int num = offset;
			if (binaryForm == null)
			{
				throw new ArgumentNullException("binaryForm");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (binaryForm.Length - offset < this.BinaryLength)
			{
				throw new ArgumentOutOfRangeException("binaryForm", Environment.GetResourceString("ArgumentOutOfRange_ArrayTooSmall"));
			}
			int binaryLength = this.BinaryLength;
			byte b = (this is RawSecurityDescriptor && (this.ControlFlags & ControlFlags.RMControlValid) != ControlFlags.None) ? (this as RawSecurityDescriptor).ResourceManagerControl : 0;
			int num2 = (int)this.ControlFlags;
			if (this.IsCraftedAefaDacl)
			{
				num2 &= -5;
			}
			binaryForm[offset] = GenericSecurityDescriptor.Revision;
			binaryForm[offset + 1] = b;
			binaryForm[offset + 2] = (byte)num2;
			binaryForm[offset + 3] = (byte)(num2 >> 8);
			int offset2 = offset + 4;
			int offset3 = offset + 8;
			int offset4 = offset + 12;
			int offset5 = offset + 16;
			offset += 20;
			if (this.Owner != null)
			{
				GenericSecurityDescriptor.MarshalInt(binaryForm, offset2, offset - num);
				this.Owner.GetBinaryForm(binaryForm, offset);
				offset += this.Owner.BinaryLength;
			}
			else
			{
				GenericSecurityDescriptor.MarshalInt(binaryForm, offset2, 0);
			}
			if (this.Group != null)
			{
				GenericSecurityDescriptor.MarshalInt(binaryForm, offset3, offset - num);
				this.Group.GetBinaryForm(binaryForm, offset);
				offset += this.Group.BinaryLength;
			}
			else
			{
				GenericSecurityDescriptor.MarshalInt(binaryForm, offset3, 0);
			}
			if ((this.ControlFlags & ControlFlags.SystemAclPresent) != ControlFlags.None && this.GenericSacl != null)
			{
				GenericSecurityDescriptor.MarshalInt(binaryForm, offset4, offset - num);
				this.GenericSacl.GetBinaryForm(binaryForm, offset);
				offset += this.GenericSacl.BinaryLength;
			}
			else
			{
				GenericSecurityDescriptor.MarshalInt(binaryForm, offset4, 0);
			}
			if ((this.ControlFlags & ControlFlags.DiscretionaryAclPresent) != ControlFlags.None && this.GenericDacl != null && !this.IsCraftedAefaDacl)
			{
				GenericSecurityDescriptor.MarshalInt(binaryForm, offset5, offset - num);
				this.GenericDacl.GetBinaryForm(binaryForm, offset);
				offset += this.GenericDacl.BinaryLength;
				return;
			}
			GenericSecurityDescriptor.MarshalInt(binaryForm, offset5, 0);
		}

		// Token: 0x04002B77 RID: 11127
		internal const int HeaderLength = 20;

		// Token: 0x04002B78 RID: 11128
		internal const int OwnerFoundAt = 4;

		// Token: 0x04002B79 RID: 11129
		internal const int GroupFoundAt = 8;

		// Token: 0x04002B7A RID: 11130
		internal const int SaclFoundAt = 12;

		// Token: 0x04002B7B RID: 11131
		internal const int DaclFoundAt = 16;
	}
}

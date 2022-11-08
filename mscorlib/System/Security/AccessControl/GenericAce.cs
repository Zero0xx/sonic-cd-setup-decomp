using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008CA RID: 2250
	public abstract class GenericAce
	{
		// Token: 0x060051D5 RID: 20949 RVA: 0x00126638 File Offset: 0x00125638
		internal void MarshalHeader(byte[] binaryForm, int offset)
		{
			int binaryLength = this.BinaryLength;
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
			if (binaryLength > 65535)
			{
				throw new SystemException();
			}
			binaryForm[offset] = (byte)this.AceType;
			binaryForm[offset + 1] = (byte)this.AceFlags;
			binaryForm[offset + 2] = (byte)binaryLength;
			binaryForm[offset + 3] = (byte)(binaryLength >> 8);
		}

		// Token: 0x060051D6 RID: 20950 RVA: 0x001266C7 File Offset: 0x001256C7
		internal GenericAce(AceType type, AceFlags flags)
		{
			this._type = type;
			this._flags = flags;
		}

		// Token: 0x060051D7 RID: 20951 RVA: 0x001266E0 File Offset: 0x001256E0
		internal static AceFlags AceFlagsFromAuditFlags(AuditFlags auditFlags)
		{
			AceFlags aceFlags = AceFlags.None;
			if ((auditFlags & AuditFlags.Success) != AuditFlags.None)
			{
				aceFlags |= AceFlags.SuccessfulAccess;
			}
			if ((auditFlags & AuditFlags.Failure) != AuditFlags.None)
			{
				aceFlags |= AceFlags.FailedAccess;
			}
			if (aceFlags == AceFlags.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "auditFlags");
			}
			return aceFlags;
		}

		// Token: 0x060051D8 RID: 20952 RVA: 0x00126724 File Offset: 0x00125724
		internal static AceFlags AceFlagsFromInheritanceFlags(InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			AceFlags aceFlags = AceFlags.None;
			if ((inheritanceFlags & InheritanceFlags.ContainerInherit) != InheritanceFlags.None)
			{
				aceFlags |= AceFlags.ContainerInherit;
			}
			if ((inheritanceFlags & InheritanceFlags.ObjectInherit) != InheritanceFlags.None)
			{
				aceFlags |= AceFlags.ObjectInherit;
			}
			if (aceFlags != AceFlags.None)
			{
				if ((propagationFlags & PropagationFlags.NoPropagateInherit) != PropagationFlags.None)
				{
					aceFlags |= AceFlags.NoPropagateInherit;
				}
				if ((propagationFlags & PropagationFlags.InheritOnly) != PropagationFlags.None)
				{
					aceFlags |= AceFlags.InheritOnly;
				}
			}
			return aceFlags;
		}

		// Token: 0x060051D9 RID: 20953 RVA: 0x00126760 File Offset: 0x00125760
		internal static void VerifyHeader(byte[] binaryForm, int offset)
		{
			if (binaryForm == null)
			{
				throw new ArgumentNullException("binaryForm");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (binaryForm.Length - offset < 4)
			{
				throw new ArgumentOutOfRangeException("binaryForm", Environment.GetResourceString("ArgumentOutOfRange_ArrayTooSmall"));
			}
			if (((int)binaryForm[offset + 3] << 8) + (int)binaryForm[offset + 2] > binaryForm.Length - offset)
			{
				throw new ArgumentOutOfRangeException("binaryForm", Environment.GetResourceString("ArgumentOutOfRange_ArrayTooSmall"));
			}
		}

		// Token: 0x060051DA RID: 20954 RVA: 0x001267DC File Offset: 0x001257DC
		public static GenericAce CreateFromBinaryForm(byte[] binaryForm, int offset)
		{
			GenericAce.VerifyHeader(binaryForm, offset);
			AceType aceType = (AceType)binaryForm[offset];
			GenericAce genericAce;
			if (aceType == AceType.AccessAllowed || aceType == AceType.AccessDenied || aceType == AceType.SystemAudit || aceType == AceType.SystemAlarm || aceType == AceType.AccessAllowedCallback || aceType == AceType.AccessDeniedCallback || aceType == AceType.SystemAuditCallback || aceType == AceType.SystemAlarmCallback)
			{
				AceQualifier qualifier;
				int accessMask;
				SecurityIdentifier sid;
				bool isCallback;
				byte[] opaque;
				if (!CommonAce.ParseBinaryForm(binaryForm, offset, out qualifier, out accessMask, out sid, out isCallback, out opaque))
				{
					goto IL_1A8;
				}
				AceFlags flags = (AceFlags)binaryForm[offset + 1];
				genericAce = new CommonAce(flags, qualifier, accessMask, sid, isCallback, opaque);
			}
			else if (aceType == AceType.AccessAllowedObject || aceType == AceType.AccessDeniedObject || aceType == AceType.SystemAuditObject || aceType == AceType.SystemAlarmObject || aceType == AceType.AccessAllowedCallbackObject || aceType == AceType.AccessDeniedCallbackObject || aceType == AceType.SystemAuditCallbackObject || aceType == AceType.SystemAlarmCallbackObject)
			{
				AceQualifier qualifier2;
				int accessMask2;
				SecurityIdentifier sid2;
				ObjectAceFlags flags2;
				Guid type;
				Guid inheritedType;
				bool isCallback2;
				byte[] opaque2;
				if (!ObjectAce.ParseBinaryForm(binaryForm, offset, out qualifier2, out accessMask2, out sid2, out flags2, out type, out inheritedType, out isCallback2, out opaque2))
				{
					goto IL_1A8;
				}
				AceFlags aceFlags = (AceFlags)binaryForm[offset + 1];
				genericAce = new ObjectAce(aceFlags, qualifier2, accessMask2, sid2, flags2, type, inheritedType, isCallback2, opaque2);
			}
			else if (aceType == AceType.AccessAllowedCompound)
			{
				int accessMask3;
				CompoundAceType compoundAceType;
				SecurityIdentifier sid3;
				if (!CompoundAce.ParseBinaryForm(binaryForm, offset, out accessMask3, out compoundAceType, out sid3))
				{
					goto IL_1A8;
				}
				AceFlags flags3 = (AceFlags)binaryForm[offset + 1];
				genericAce = new CompoundAce(flags3, accessMask3, compoundAceType, sid3);
			}
			else
			{
				AceFlags flags4 = (AceFlags)binaryForm[offset + 1];
				byte[] array = null;
				int num = (int)binaryForm[offset + 2] + ((int)binaryForm[offset + 3] << 8);
				if (num % 4 != 0)
				{
					goto IL_1A8;
				}
				int num2 = num - 4;
				if (num2 > 0)
				{
					array = new byte[num2];
					for (int i = 0; i < num2; i++)
					{
						array[i] = binaryForm[offset + num - num2 + i];
					}
				}
				genericAce = new CustomAce(aceType, flags4, array);
			}
			if ((genericAce is ObjectAce || (int)binaryForm[offset + 2] + ((int)binaryForm[offset + 3] << 8) == genericAce.BinaryLength) && (!(genericAce is ObjectAce) || (int)binaryForm[offset + 2] + ((int)binaryForm[offset + 3] << 8) == genericAce.BinaryLength || (int)binaryForm[offset + 2] + ((int)binaryForm[offset + 3] << 8) - 32 == genericAce.BinaryLength))
			{
				return genericAce;
			}
			IL_1A8:
			throw new ArgumentException(Environment.GetResourceString("ArgumentException_InvalidAceBinaryForm"), "binaryForm");
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x060051DB RID: 20955 RVA: 0x001269A5 File Offset: 0x001259A5
		public AceType AceType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x060051DC RID: 20956 RVA: 0x001269AD File Offset: 0x001259AD
		// (set) Token: 0x060051DD RID: 20957 RVA: 0x001269B5 File Offset: 0x001259B5
		public AceFlags AceFlags
		{
			get
			{
				return this._flags;
			}
			set
			{
				this._flags = value;
			}
		}

		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x060051DE RID: 20958 RVA: 0x001269BE File Offset: 0x001259BE
		public bool IsInherited
		{
			get
			{
				return (byte)(this.AceFlags & AceFlags.Inherited) != 0;
			}
		}

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x060051DF RID: 20959 RVA: 0x001269D0 File Offset: 0x001259D0
		public InheritanceFlags InheritanceFlags
		{
			get
			{
				InheritanceFlags inheritanceFlags = InheritanceFlags.None;
				if ((byte)(this.AceFlags & AceFlags.ContainerInherit) != 0)
				{
					inheritanceFlags |= InheritanceFlags.ContainerInherit;
				}
				if ((byte)(this.AceFlags & AceFlags.ObjectInherit) != 0)
				{
					inheritanceFlags |= InheritanceFlags.ObjectInherit;
				}
				return inheritanceFlags;
			}
		}

		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x060051E0 RID: 20960 RVA: 0x00126A00 File Offset: 0x00125A00
		public PropagationFlags PropagationFlags
		{
			get
			{
				PropagationFlags propagationFlags = PropagationFlags.None;
				if ((byte)(this.AceFlags & AceFlags.InheritOnly) != 0)
				{
					propagationFlags |= PropagationFlags.InheritOnly;
				}
				if ((byte)(this.AceFlags & AceFlags.NoPropagateInherit) != 0)
				{
					propagationFlags |= PropagationFlags.NoPropagateInherit;
				}
				return propagationFlags;
			}
		}

		// Token: 0x17000E30 RID: 3632
		// (get) Token: 0x060051E1 RID: 20961 RVA: 0x00126A30 File Offset: 0x00125A30
		public AuditFlags AuditFlags
		{
			get
			{
				AuditFlags auditFlags = AuditFlags.None;
				if ((byte)(this.AceFlags & AceFlags.SuccessfulAccess) != 0)
				{
					auditFlags |= AuditFlags.Success;
				}
				if ((byte)(this.AceFlags & AceFlags.FailedAccess) != 0)
				{
					auditFlags |= AuditFlags.Failure;
				}
				return auditFlags;
			}
		}

		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x060051E2 RID: 20962
		public abstract int BinaryLength { get; }

		// Token: 0x060051E3 RID: 20963
		public abstract void GetBinaryForm(byte[] binaryForm, int offset);

		// Token: 0x060051E4 RID: 20964 RVA: 0x00126A64 File Offset: 0x00125A64
		public GenericAce Copy()
		{
			byte[] binaryForm = new byte[this.BinaryLength];
			this.GetBinaryForm(binaryForm, 0);
			return GenericAce.CreateFromBinaryForm(binaryForm, 0);
		}

		// Token: 0x060051E5 RID: 20965 RVA: 0x00126A8C File Offset: 0x00125A8C
		public sealed override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			GenericAce genericAce = o as GenericAce;
			if (genericAce == null)
			{
				return false;
			}
			if (this.AceType != genericAce.AceType || this.AceFlags != genericAce.AceFlags)
			{
				return false;
			}
			int binaryLength = this.BinaryLength;
			int binaryLength2 = genericAce.BinaryLength;
			if (binaryLength != binaryLength2)
			{
				return false;
			}
			byte[] array = new byte[binaryLength];
			byte[] array2 = new byte[binaryLength2];
			this.GetBinaryForm(array, 0);
			genericAce.GetBinaryForm(array2, 0);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != array2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060051E6 RID: 20966 RVA: 0x00126B24 File Offset: 0x00125B24
		public sealed override int GetHashCode()
		{
			int binaryLength = this.BinaryLength;
			byte[] array = new byte[binaryLength];
			this.GetBinaryForm(array, 0);
			int num = 0;
			for (int i = 0; i < binaryLength; i += 4)
			{
				int num2 = (int)array[i] + ((int)array[i + 1] << 8) + ((int)array[i + 2] << 16) + ((int)array[i + 3] << 24);
				num ^= num2;
			}
			return num;
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x00126B7C File Offset: 0x00125B7C
		public static bool operator ==(GenericAce left, GenericAce right)
		{
			return (left == null && right == null) || (left != null && right != null && left.Equals(right));
		}

		// Token: 0x060051E8 RID: 20968 RVA: 0x00126BA4 File Offset: 0x00125BA4
		public static bool operator !=(GenericAce left, GenericAce right)
		{
			return !(left == right);
		}

		// Token: 0x04002A58 RID: 10840
		internal const int HeaderLength = 4;

		// Token: 0x04002A59 RID: 10841
		private readonly AceType _type;

		// Token: 0x04002A5A RID: 10842
		private AceFlags _flags;

		// Token: 0x04002A5B RID: 10843
		internal ushort _indexInAcl;
	}
}

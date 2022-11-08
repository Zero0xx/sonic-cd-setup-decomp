using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008D7 RID: 2263
	public abstract class CommonAcl : GenericAcl
	{
		// Token: 0x0600523A RID: 21050 RVA: 0x00127D50 File Offset: 0x00126D50
		static CommonAcl()
		{
			for (int i = 0; i < CommonAcl.AFtoPM.Length; i++)
			{
				CommonAcl.AFtoPM[i] = CommonAcl.PM.GO;
			}
			CommonAcl.AFtoPM[0] = CommonAcl.PM.F;
			CommonAcl.AFtoPM[4] = (CommonAcl.PM.F | CommonAcl.PM.CO | CommonAcl.PM.GO);
			CommonAcl.AFtoPM[5] = (CommonAcl.PM.F | CommonAcl.PM.CO);
			CommonAcl.AFtoPM[6] = (CommonAcl.PM.CO | CommonAcl.PM.GO);
			CommonAcl.AFtoPM[7] = CommonAcl.PM.CO;
			CommonAcl.AFtoPM[8] = (CommonAcl.PM.F | CommonAcl.PM.CF | CommonAcl.PM.GF);
			CommonAcl.AFtoPM[9] = (CommonAcl.PM.F | CommonAcl.PM.CF);
			CommonAcl.AFtoPM[10] = (CommonAcl.PM.CF | CommonAcl.PM.GF);
			CommonAcl.AFtoPM[11] = CommonAcl.PM.CF;
			CommonAcl.AFtoPM[12] = (CommonAcl.PM.F | CommonAcl.PM.CF | CommonAcl.PM.CO | CommonAcl.PM.GF | CommonAcl.PM.GO);
			CommonAcl.AFtoPM[13] = (CommonAcl.PM.F | CommonAcl.PM.CF | CommonAcl.PM.CO);
			CommonAcl.AFtoPM[14] = (CommonAcl.PM.CF | CommonAcl.PM.CO | CommonAcl.PM.GF | CommonAcl.PM.GO);
			CommonAcl.AFtoPM[15] = (CommonAcl.PM.CF | CommonAcl.PM.CO);
			CommonAcl.PMtoAF = new CommonAcl.AF[32];
			for (int j = 0; j < CommonAcl.PMtoAF.Length; j++)
			{
				CommonAcl.PMtoAF[j] = CommonAcl.AF.NP;
			}
			CommonAcl.PMtoAF[16] = (CommonAcl.AF)0;
			CommonAcl.PMtoAF[21] = CommonAcl.AF.OI;
			CommonAcl.PMtoAF[20] = (CommonAcl.AF.OI | CommonAcl.AF.NP);
			CommonAcl.PMtoAF[5] = (CommonAcl.AF.OI | CommonAcl.AF.IO);
			CommonAcl.PMtoAF[4] = (CommonAcl.AF.OI | CommonAcl.AF.IO | CommonAcl.AF.NP);
			CommonAcl.PMtoAF[26] = CommonAcl.AF.CI;
			CommonAcl.PMtoAF[24] = (CommonAcl.AF.CI | CommonAcl.AF.NP);
			CommonAcl.PMtoAF[10] = (CommonAcl.AF.CI | CommonAcl.AF.IO);
			CommonAcl.PMtoAF[8] = (CommonAcl.AF.CI | CommonAcl.AF.IO | CommonAcl.AF.NP);
			CommonAcl.PMtoAF[31] = (CommonAcl.AF.CI | CommonAcl.AF.OI);
			CommonAcl.PMtoAF[28] = (CommonAcl.AF.CI | CommonAcl.AF.OI | CommonAcl.AF.NP);
			CommonAcl.PMtoAF[15] = (CommonAcl.AF.CI | CommonAcl.AF.OI | CommonAcl.AF.IO);
			CommonAcl.PMtoAF[12] = (CommonAcl.AF.CI | CommonAcl.AF.OI | CommonAcl.AF.IO | CommonAcl.AF.NP);
		}

		// Token: 0x0600523B RID: 21051 RVA: 0x00127E9C File Offset: 0x00126E9C
		private static CommonAcl.AF AFFromAceFlags(AceFlags aceFlags, bool isDS)
		{
			CommonAcl.AF af = (CommonAcl.AF)0;
			if ((byte)(aceFlags & AceFlags.ContainerInherit) != 0)
			{
				af |= CommonAcl.AF.CI;
			}
			if (!isDS && (byte)(aceFlags & AceFlags.ObjectInherit) != 0)
			{
				af |= CommonAcl.AF.OI;
			}
			if ((byte)(aceFlags & AceFlags.InheritOnly) != 0)
			{
				af |= CommonAcl.AF.IO;
			}
			if ((byte)(aceFlags & AceFlags.NoPropagateInherit) != 0)
			{
				af |= CommonAcl.AF.NP;
			}
			return af;
		}

		// Token: 0x0600523C RID: 21052 RVA: 0x00127ED8 File Offset: 0x00126ED8
		private static AceFlags AceFlagsFromAF(CommonAcl.AF af, bool isDS)
		{
			AceFlags aceFlags = AceFlags.None;
			if ((af & CommonAcl.AF.CI) != (CommonAcl.AF)0)
			{
				aceFlags |= AceFlags.ContainerInherit;
			}
			if (!isDS && (af & CommonAcl.AF.OI) != (CommonAcl.AF)0)
			{
				aceFlags |= AceFlags.ObjectInherit;
			}
			if ((af & CommonAcl.AF.IO) != (CommonAcl.AF)0)
			{
				aceFlags |= AceFlags.InheritOnly;
			}
			if ((af & CommonAcl.AF.NP) != (CommonAcl.AF)0)
			{
				aceFlags |= AceFlags.NoPropagateInherit;
			}
			return aceFlags;
		}

		// Token: 0x0600523D RID: 21053 RVA: 0x00127F14 File Offset: 0x00126F14
		private static bool MergeInheritanceBits(AceFlags left, AceFlags right, bool isDS, out AceFlags result)
		{
			result = AceFlags.None;
			CommonAcl.AF af = CommonAcl.AFFromAceFlags(left, isDS);
			CommonAcl.AF af2 = CommonAcl.AFFromAceFlags(right, isDS);
			CommonAcl.PM pm = CommonAcl.AFtoPM[(int)af];
			CommonAcl.PM pm2 = CommonAcl.AFtoPM[(int)af2];
			if (pm == CommonAcl.PM.GO || pm2 == CommonAcl.PM.GO)
			{
				return false;
			}
			CommonAcl.PM pm3 = pm | pm2;
			CommonAcl.AF af3 = CommonAcl.PMtoAF[(int)pm3];
			if (af3 == CommonAcl.AF.NP)
			{
				return false;
			}
			result = CommonAcl.AceFlagsFromAF(af3, isDS);
			return true;
		}

		// Token: 0x0600523E RID: 21054 RVA: 0x00127F70 File Offset: 0x00126F70
		private static bool RemoveInheritanceBits(AceFlags existing, AceFlags remove, bool isDS, out AceFlags result, out bool total)
		{
			result = AceFlags.None;
			total = false;
			CommonAcl.AF af = CommonAcl.AFFromAceFlags(existing, isDS);
			CommonAcl.AF af2 = CommonAcl.AFFromAceFlags(remove, isDS);
			CommonAcl.PM pm = CommonAcl.AFtoPM[(int)af];
			CommonAcl.PM pm2 = CommonAcl.AFtoPM[(int)af2];
			if (pm == CommonAcl.PM.GO || pm2 == CommonAcl.PM.GO)
			{
				return false;
			}
			CommonAcl.PM pm3 = pm & ~pm2;
			if (pm3 == (CommonAcl.PM)0)
			{
				total = true;
				return true;
			}
			CommonAcl.AF af3 = CommonAcl.PMtoAF[(int)pm3];
			if (af3 == CommonAcl.AF.NP)
			{
				return false;
			}
			result = CommonAcl.AceFlagsFromAF(af3, isDS);
			return true;
		}

		// Token: 0x0600523F RID: 21055 RVA: 0x00127FDA File Offset: 0x00126FDA
		private void CanonicalizeIfNecessary()
		{
			if (this._isDirty)
			{
				this.Canonicalize(false, this is DiscretionaryAcl);
				this._isDirty = false;
			}
		}

		// Token: 0x06005240 RID: 21056 RVA: 0x00127FFC File Offset: 0x00126FFC
		private static int DaclAcePriority(GenericAce ace)
		{
			AceType aceType = ace.AceType;
			int result;
			if ((byte)(ace.AceFlags & AceFlags.Inherited) != 0)
			{
				result = 131070 + (int)ace._indexInAcl;
			}
			else if (aceType == AceType.AccessDenied || aceType == AceType.AccessDeniedCallback)
			{
				result = 0;
			}
			else if (aceType == AceType.AccessDeniedObject || aceType == AceType.AccessDeniedCallbackObject)
			{
				result = 1;
			}
			else if (aceType == AceType.AccessAllowed || aceType == AceType.AccessAllowedCallback)
			{
				result = 2;
			}
			else if (aceType == AceType.AccessAllowedObject || aceType == AceType.AccessAllowedCallbackObject)
			{
				result = 3;
			}
			else
			{
				result = (int)(ushort.MaxValue + ace._indexInAcl);
			}
			return result;
		}

		// Token: 0x06005241 RID: 21057 RVA: 0x0012806C File Offset: 0x0012706C
		private static int SaclAcePriority(GenericAce ace)
		{
			AceType aceType = ace.AceType;
			int result;
			if ((byte)(ace.AceFlags & AceFlags.Inherited) != 0)
			{
				result = 131070 + (int)ace._indexInAcl;
			}
			else if (aceType == AceType.SystemAudit || aceType == AceType.SystemAlarm || aceType == AceType.SystemAuditCallback || aceType == AceType.SystemAlarmCallback)
			{
				result = 0;
			}
			else if (aceType == AceType.SystemAuditObject || aceType == AceType.SystemAlarmObject || aceType == AceType.SystemAuditCallbackObject || aceType == AceType.SystemAlarmCallbackObject)
			{
				result = 1;
			}
			else
			{
				result = (int)(ushort.MaxValue + ace._indexInAcl);
			}
			return result;
		}

		// Token: 0x06005242 RID: 21058 RVA: 0x001280D8 File Offset: 0x001270D8
		private static CommonAcl.ComparisonResult CompareAces(GenericAce ace1, GenericAce ace2, bool isDacl)
		{
			int num = isDacl ? CommonAcl.DaclAcePriority(ace1) : CommonAcl.SaclAcePriority(ace1);
			int num2 = isDacl ? CommonAcl.DaclAcePriority(ace2) : CommonAcl.SaclAcePriority(ace2);
			if (num < num2)
			{
				return CommonAcl.ComparisonResult.LessThan;
			}
			if (num > num2)
			{
				return CommonAcl.ComparisonResult.GreaterThan;
			}
			KnownAce knownAce = ace1 as KnownAce;
			KnownAce knownAce2 = ace2 as KnownAce;
			if (knownAce != null && knownAce2 != null)
			{
				int num3 = knownAce.SecurityIdentifier.CompareTo(knownAce2.SecurityIdentifier);
				if (num3 < 0)
				{
					return CommonAcl.ComparisonResult.LessThan;
				}
				if (num3 > 0)
				{
					return CommonAcl.ComparisonResult.GreaterThan;
				}
			}
			return CommonAcl.ComparisonResult.EqualTo;
		}

		// Token: 0x06005243 RID: 21059 RVA: 0x00128158 File Offset: 0x00127158
		private void QuickSort(int left, int right, bool isDacl)
		{
			if (left >= right)
			{
				return;
			}
			int num = left;
			int num2 = right;
			GenericAce genericAce = this._acl[left];
			while (left < right)
			{
				while (CommonAcl.CompareAces(this._acl[right], genericAce, isDacl) != CommonAcl.ComparisonResult.LessThan && left < right)
				{
					right--;
				}
				if (left != right)
				{
					this._acl[left] = this._acl[right];
					left++;
				}
				while (CommonAcl.ComparisonResult.GreaterThan != CommonAcl.CompareAces(this._acl[left], genericAce, isDacl) && left < right)
				{
					left++;
				}
				if (left != right)
				{
					this._acl[right] = this._acl[left];
					right--;
				}
			}
			this._acl[left] = genericAce;
			int num3 = left;
			left = num;
			right = num2;
			if (left < num3)
			{
				this.QuickSort(left, num3 - 1, isDacl);
			}
			if (right > num3)
			{
				this.QuickSort(num3 + 1, right, isDacl);
			}
		}

		// Token: 0x06005244 RID: 21060 RVA: 0x0012823C File Offset: 0x0012723C
		private bool InspectAce(ref GenericAce ace, bool isDacl)
		{
			KnownAce knownAce = ace as KnownAce;
			if (knownAce != null && knownAce.AccessMask == 0)
			{
				return false;
			}
			if (!this.IsContainer)
			{
				if ((byte)(ace.AceFlags & AceFlags.InheritOnly) != 0)
				{
					return false;
				}
				if ((byte)(ace.AceFlags & AceFlags.InheritanceFlags) != 0)
				{
					GenericAce genericAce = ace;
					genericAce.AceFlags &= ~(AceFlags.ObjectInherit | AceFlags.ContainerInherit | AceFlags.NoPropagateInherit | AceFlags.InheritOnly);
				}
			}
			else
			{
				if ((byte)(ace.AceFlags & AceFlags.InheritOnly) != 0 && (byte)(ace.AceFlags & AceFlags.ContainerInherit) == 0 && (byte)(ace.AceFlags & AceFlags.ObjectInherit) == 0)
				{
					return false;
				}
				if ((byte)(ace.AceFlags & AceFlags.NoPropagateInherit) != 0 && (byte)(ace.AceFlags & AceFlags.ContainerInherit) == 0 && (byte)(ace.AceFlags & AceFlags.ObjectInherit) == 0)
				{
					GenericAce genericAce2 = ace;
					genericAce2.AceFlags &= ~AceFlags.NoPropagateInherit;
				}
			}
			QualifiedAce qualifiedAce = knownAce as QualifiedAce;
			if (isDacl)
			{
				GenericAce genericAce3 = ace;
				genericAce3.AceFlags &= ~(AceFlags.SuccessfulAccess | AceFlags.FailedAccess);
				if (qualifiedAce != null && qualifiedAce.AceQualifier != AceQualifier.AccessAllowed && qualifiedAce.AceQualifier != AceQualifier.AccessDenied)
				{
					return false;
				}
			}
			else
			{
				if ((byte)(ace.AceFlags & AceFlags.AuditFlags) == 0)
				{
					return false;
				}
				if (qualifiedAce != null && qualifiedAce.AceQualifier != AceQualifier.SystemAudit)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005245 RID: 21061 RVA: 0x0012835C File Offset: 0x0012735C
		private void RemoveMeaninglessAcesAndFlags(bool isDacl)
		{
			for (int i = this._acl.Count - 1; i >= 0; i--)
			{
				GenericAce genericAce = this._acl[i];
				if (!this.InspectAce(ref genericAce, isDacl))
				{
					this._acl.RemoveAce(i);
				}
			}
		}

		// Token: 0x06005246 RID: 21062 RVA: 0x001283A8 File Offset: 0x001273A8
		private void Canonicalize(bool compact, bool isDacl)
		{
			ushort num = 0;
			while ((int)num < this._acl.Count)
			{
				this._acl[(int)num]._indexInAcl = num;
				num += 1;
			}
			this.QuickSort(0, this._acl.Count - 1, isDacl);
			if (compact)
			{
				for (int i = 0; i < this.Count - 1; i++)
				{
					QualifiedAce left = this._acl[i] as QualifiedAce;
					if (!(left == null))
					{
						QualifiedAce qualifiedAce = this._acl[i + 1] as QualifiedAce;
						if (!(qualifiedAce == null) && this.MergeAces(ref left, qualifiedAce))
						{
							this._acl.RemoveAce(i + 1);
						}
					}
				}
			}
		}

		// Token: 0x06005247 RID: 21063 RVA: 0x0012845C File Offset: 0x0012745C
		private void GetObjectTypesForSplit(ObjectAce originalAce, int accessMask, AceFlags aceFlags, out ObjectAceFlags objectFlags, out Guid objectType, out Guid inheritedObjectType)
		{
			objectFlags = ObjectAceFlags.None;
			objectType = Guid.Empty;
			inheritedObjectType = Guid.Empty;
			if ((accessMask & ObjectAce.AccessMaskWithObjectType) != 0)
			{
				objectType = originalAce.ObjectAceType;
				objectFlags |= (originalAce.ObjectAceFlags & ObjectAceFlags.ObjectAceTypePresent);
			}
			if ((byte)(aceFlags & AceFlags.ContainerInherit) != 0)
			{
				inheritedObjectType = originalAce.InheritedObjectAceType;
				objectFlags |= (originalAce.ObjectAceFlags & ObjectAceFlags.InheritedObjectAceTypePresent);
			}
		}

		// Token: 0x06005248 RID: 21064 RVA: 0x001284CC File Offset: 0x001274CC
		private bool ObjectTypesMatch(QualifiedAce ace, QualifiedAce newAce)
		{
			Guid guid = (ace is ObjectAce) ? ((ObjectAce)ace).ObjectAceType : Guid.Empty;
			Guid g = (newAce is ObjectAce) ? ((ObjectAce)newAce).ObjectAceType : Guid.Empty;
			return guid.Equals(g);
		}

		// Token: 0x06005249 RID: 21065 RVA: 0x00128518 File Offset: 0x00127518
		private bool InheritedObjectTypesMatch(QualifiedAce ace, QualifiedAce newAce)
		{
			Guid guid = (ace is ObjectAce) ? ((ObjectAce)ace).InheritedObjectAceType : Guid.Empty;
			Guid g = (newAce is ObjectAce) ? ((ObjectAce)newAce).InheritedObjectAceType : Guid.Empty;
			return guid.Equals(g);
		}

		// Token: 0x0600524A RID: 21066 RVA: 0x00128564 File Offset: 0x00127564
		private bool AccessMasksAreMergeable(QualifiedAce ace, QualifiedAce newAce)
		{
			if (this.ObjectTypesMatch(ace, newAce))
			{
				return true;
			}
			ObjectAceFlags objectAceFlags = (ace is ObjectAce) ? ((ObjectAce)ace).ObjectAceFlags : ObjectAceFlags.None;
			return (ace.AccessMask & newAce.AccessMask & ObjectAce.AccessMaskWithObjectType) == (newAce.AccessMask & ObjectAce.AccessMaskWithObjectType) && (objectAceFlags & ObjectAceFlags.ObjectAceTypePresent) == ObjectAceFlags.None;
		}

		// Token: 0x0600524B RID: 21067 RVA: 0x001285C0 File Offset: 0x001275C0
		private bool AceFlagsAreMergeable(QualifiedAce ace, QualifiedAce newAce)
		{
			if (this.InheritedObjectTypesMatch(ace, newAce))
			{
				return true;
			}
			ObjectAceFlags objectAceFlags = (ace is ObjectAce) ? ((ObjectAce)ace).ObjectAceFlags : ObjectAceFlags.None;
			return (objectAceFlags & ObjectAceFlags.InheritedObjectAceTypePresent) == ObjectAceFlags.None;
		}

		// Token: 0x0600524C RID: 21068 RVA: 0x001285F8 File Offset: 0x001275F8
		private bool GetAccessMaskForRemoval(QualifiedAce ace, ObjectAceFlags objectFlags, Guid objectType, ref int accessMask)
		{
			if ((ace.AccessMask & accessMask & ObjectAce.AccessMaskWithObjectType) != 0)
			{
				if (ace is ObjectAce)
				{
					ObjectAce objectAce = ace as ObjectAce;
					if ((objectFlags & ObjectAceFlags.ObjectAceTypePresent) != ObjectAceFlags.None && (objectAce.ObjectAceFlags & ObjectAceFlags.ObjectAceTypePresent) == ObjectAceFlags.None)
					{
						return false;
					}
					if ((objectFlags & ObjectAceFlags.ObjectAceTypePresent) != ObjectAceFlags.None && !objectAce.ObjectTypesMatch(objectFlags, objectType))
					{
						accessMask &= ~ObjectAce.AccessMaskWithObjectType;
					}
				}
				else if ((objectFlags & ObjectAceFlags.ObjectAceTypePresent) != ObjectAceFlags.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600524D RID: 21069 RVA: 0x00128664 File Offset: 0x00127664
		private bool GetInheritanceFlagsForRemoval(QualifiedAce ace, ObjectAceFlags objectFlags, Guid inheritedObjectType, ref AceFlags aceFlags)
		{
			if ((byte)(ace.AceFlags & AceFlags.ContainerInherit) != 0 && (byte)(aceFlags & AceFlags.ContainerInherit) != 0)
			{
				if (ace is ObjectAce)
				{
					ObjectAce objectAce = ace as ObjectAce;
					if ((objectFlags & ObjectAceFlags.InheritedObjectAceTypePresent) != ObjectAceFlags.None && (objectAce.ObjectAceFlags & ObjectAceFlags.InheritedObjectAceTypePresent) == ObjectAceFlags.None)
					{
						return false;
					}
					if ((objectFlags & ObjectAceFlags.InheritedObjectAceTypePresent) != ObjectAceFlags.None && !objectAce.InheritedObjectTypesMatch(objectFlags, inheritedObjectType))
					{
						aceFlags &= ~(AceFlags.ObjectInherit | AceFlags.ContainerInherit | AceFlags.NoPropagateInherit | AceFlags.InheritOnly);
					}
				}
				else if ((objectFlags & ObjectAceFlags.InheritedObjectAceTypePresent) != ObjectAceFlags.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600524E RID: 21070 RVA: 0x001286D0 File Offset: 0x001276D0
		private bool MergeAces(ref QualifiedAce ace, QualifiedAce newAce)
		{
			if ((byte)(ace.AceFlags & AceFlags.Inherited) != 0)
			{
				return false;
			}
			if ((byte)(newAce.AceFlags & AceFlags.Inherited) != 0)
			{
				return false;
			}
			if (ace.AceQualifier != newAce.AceQualifier)
			{
				return false;
			}
			if (ace.SecurityIdentifier != newAce.SecurityIdentifier)
			{
				return false;
			}
			if (ace.AceFlags == newAce.AceFlags)
			{
				if (!(ace is ObjectAce) && !(newAce is ObjectAce))
				{
					ace.AccessMask |= newAce.AccessMask;
					return true;
				}
				if (this.InheritedObjectTypesMatch(ace, newAce) && this.AccessMasksAreMergeable(ace, newAce))
				{
					ace.AccessMask |= newAce.AccessMask;
					return true;
				}
			}
			if ((byte)(ace.AceFlags & AceFlags.InheritanceFlags) == (byte)(newAce.AceFlags & AceFlags.InheritanceFlags) && ace.AccessMask == newAce.AccessMask)
			{
				if (!(ace is ObjectAce) && !(newAce is ObjectAce))
				{
					QualifiedAce qualifiedAce = ace;
					qualifiedAce.AceFlags |= (newAce.AceFlags & AceFlags.AuditFlags);
					return true;
				}
				if (this.InheritedObjectTypesMatch(ace, newAce) && this.ObjectTypesMatch(ace, newAce))
				{
					QualifiedAce qualifiedAce2 = ace;
					qualifiedAce2.AceFlags |= (newAce.AceFlags & AceFlags.AuditFlags);
					return true;
				}
			}
			if ((byte)(ace.AceFlags & AceFlags.AuditFlags) == (byte)(newAce.AceFlags & AceFlags.AuditFlags) && ace.AccessMask == newAce.AccessMask)
			{
				AceFlags aceFlags;
				if (ace is ObjectAce || newAce is ObjectAce)
				{
					if (this.ObjectTypesMatch(ace, newAce) && this.AceFlagsAreMergeable(ace, newAce) && CommonAcl.MergeInheritanceBits(ace.AceFlags, newAce.AceFlags, this.IsDS, out aceFlags))
					{
						ace.AceFlags = (aceFlags | (ace.AceFlags & AceFlags.AuditFlags));
						return true;
					}
				}
				else if (CommonAcl.MergeInheritanceBits(ace.AceFlags, newAce.AceFlags, this.IsDS, out aceFlags))
				{
					ace.AceFlags = (aceFlags | (ace.AceFlags & AceFlags.AuditFlags));
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600524F RID: 21071 RVA: 0x001288D4 File Offset: 0x001278D4
		private bool CanonicalCheck(bool isDacl)
		{
			if (isDacl)
			{
				int num = 0;
				for (int i = 0; i < this._acl.Count; i++)
				{
					GenericAce genericAce = this._acl[i];
					int num2;
					if ((byte)(genericAce.AceFlags & AceFlags.Inherited) != 0)
					{
						num2 = 2;
					}
					else
					{
						QualifiedAce qualifiedAce = genericAce as QualifiedAce;
						if (qualifiedAce == null)
						{
							return false;
						}
						if (qualifiedAce.AceQualifier == AceQualifier.AccessAllowed)
						{
							num2 = 1;
						}
						else
						{
							if (qualifiedAce.AceQualifier != AceQualifier.AccessDenied)
							{
								return false;
							}
							num2 = 0;
						}
					}
					if (num2 != 3)
					{
						if (num2 > num)
						{
							num = num2;
						}
						else if (num2 < num)
						{
							return false;
						}
					}
				}
			}
			else
			{
				int num3 = 0;
				for (int j = 0; j < this._acl.Count; j++)
				{
					GenericAce genericAce2 = this._acl[j];
					if (!(genericAce2 == null))
					{
						int num4;
						if ((byte)(genericAce2.AceFlags & AceFlags.Inherited) != 0)
						{
							num4 = 1;
						}
						else
						{
							QualifiedAce qualifiedAce2 = genericAce2 as QualifiedAce;
							if (qualifiedAce2 == null)
							{
								return false;
							}
							if (qualifiedAce2.AceQualifier != AceQualifier.SystemAudit && qualifiedAce2.AceQualifier != AceQualifier.SystemAlarm)
							{
								return false;
							}
							num4 = 0;
						}
						if (num4 > num3)
						{
							num3 = num4;
						}
						else if (num4 < num3)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06005250 RID: 21072 RVA: 0x001289F6 File Offset: 0x001279F6
		private void ThrowIfNotCanonical()
		{
			if (!this._isCanonical)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ModificationOfNonCanonicalAcl"));
			}
		}

		// Token: 0x06005251 RID: 21073 RVA: 0x00128A10 File Offset: 0x00127A10
		internal CommonAcl(bool isContainer, bool isDS, byte revision, int capacity)
		{
			this._isContainer = isContainer;
			this._isDS = isDS;
			this._acl = new RawAcl(revision, capacity);
			this._isCanonical = true;
		}

		// Token: 0x06005252 RID: 21074 RVA: 0x00128A3C File Offset: 0x00127A3C
		internal CommonAcl(bool isContainer, bool isDS, RawAcl rawAcl, bool trusted, bool isDacl)
		{
			this._isContainer = isContainer;
			this._isDS = isDS;
			if (rawAcl == null)
			{
				throw new ArgumentNullException("rawAcl");
			}
			if (trusted)
			{
				this._acl = rawAcl;
				this.RemoveMeaninglessAcesAndFlags(isDacl);
			}
			else
			{
				this._acl = new RawAcl(rawAcl.Revision, rawAcl.Count);
				for (int i = 0; i < rawAcl.Count; i++)
				{
					GenericAce ace = rawAcl[i].Copy();
					if (this.InspectAce(ref ace, isDacl))
					{
						this._acl.InsertAce(this._acl.Count, ace);
					}
				}
			}
			if (this.CanonicalCheck(isDacl))
			{
				this.Canonicalize(true, isDacl);
				this._isCanonical = true;
				return;
			}
			this._isCanonical = false;
		}

		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x06005253 RID: 21075 RVA: 0x00128AFA File Offset: 0x00127AFA
		internal RawAcl RawAcl
		{
			get
			{
				return this._acl;
			}
		}

		// Token: 0x06005254 RID: 21076 RVA: 0x00128B02 File Offset: 0x00127B02
		internal void CheckAccessType(AccessControlType accessType)
		{
			if (accessType != AccessControlType.Allow && accessType != AccessControlType.Deny)
			{
				throw new ArgumentOutOfRangeException("accessType", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
			}
		}

		// Token: 0x06005255 RID: 21077 RVA: 0x00128B20 File Offset: 0x00127B20
		internal void CheckFlags(InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			if (this.IsContainer)
			{
				if (inheritanceFlags == InheritanceFlags.None && propagationFlags != PropagationFlags.None)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidAnyFlag"), "propagationFlags");
				}
			}
			else
			{
				if (inheritanceFlags != InheritanceFlags.None)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidAnyFlag"), "inheritanceFlags");
				}
				if (propagationFlags != PropagationFlags.None)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidAnyFlag"), "propagationFlags");
				}
			}
		}

		// Token: 0x06005256 RID: 21078 RVA: 0x00128B80 File Offset: 0x00127B80
		internal void AddQualifiedAce(SecurityIdentifier sid, AceQualifier qualifier, int accessMask, AceFlags flags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			this.ThrowIfNotCanonical();
			bool flag = false;
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			if (qualifier == AceQualifier.SystemAudit && (byte)(flags & AceFlags.AuditFlags) == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "flags");
			}
			if (accessMask == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ArgumentZero"), "accessMask");
			}
			GenericAce genericAce;
			if (!this.IsDS || objectFlags == ObjectAceFlags.None)
			{
				genericAce = new CommonAce(flags, qualifier, accessMask, sid, false, null);
			}
			else
			{
				genericAce = new ObjectAce(flags, qualifier, accessMask, sid, objectFlags, objectType, inheritedObjectType, false, null);
			}
			if (!this.InspectAce(ref genericAce, this is DiscretionaryAcl))
			{
				return;
			}
			for (int i = 0; i < this.Count; i++)
			{
				QualifiedAce left = this._acl[i] as QualifiedAce;
				if (!(left == null) && this.MergeAces(ref left, genericAce as QualifiedAce))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this._acl.InsertAce(this._acl.Count, genericAce);
				this._isDirty = true;
			}
			this.OnAclModificationTried();
		}

		// Token: 0x06005257 RID: 21079 RVA: 0x00128C90 File Offset: 0x00127C90
		internal void SetQualifiedAce(SecurityIdentifier sid, AceQualifier qualifier, int accessMask, AceFlags flags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			this.ThrowIfNotCanonical();
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			if (qualifier == AceQualifier.SystemAudit && (byte)(flags & AceFlags.AuditFlags) == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "flags");
			}
			if (accessMask == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ArgumentZero"), "accessMask");
			}
			GenericAce ace;
			if (!this.IsDS || objectFlags == ObjectAceFlags.None)
			{
				ace = new CommonAce(flags, qualifier, accessMask, sid, false, null);
			}
			else
			{
				ace = new ObjectAce(flags, qualifier, accessMask, sid, objectFlags, objectType, inheritedObjectType, false, null);
			}
			if (!this.InspectAce(ref ace, this is DiscretionaryAcl))
			{
				return;
			}
			for (int i = 0; i < this.Count; i++)
			{
				QualifiedAce qualifiedAce = this._acl[i] as QualifiedAce;
				if (!(qualifiedAce == null) && (byte)(qualifiedAce.AceFlags & AceFlags.Inherited) == 0 && qualifiedAce.AceQualifier == qualifier && !(qualifiedAce.SecurityIdentifier != sid))
				{
					this._acl.RemoveAce(i);
					i--;
				}
			}
			this._acl.InsertAce(this._acl.Count, ace);
			this._isDirty = true;
			this.OnAclModificationTried();
		}

		// Token: 0x06005258 RID: 21080 RVA: 0x00128DB8 File Offset: 0x00127DB8
		internal bool RemoveQualifiedAces(SecurityIdentifier sid, AceQualifier qualifier, int accessMask, AceFlags flags, bool saclSemantics, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			this.ThrowIfNotCanonical();
			if (accessMask == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ArgumentZero"), "accessMask");
			}
			if (qualifier == AceQualifier.SystemAudit && (byte)(flags & AceFlags.AuditFlags) == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "flags");
			}
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			bool flag = true;
			bool flag2 = true;
			int num = accessMask;
			AceFlags aceFlags = flags;
			byte[] binaryForm = new byte[this.BinaryLength];
			this.GetBinaryForm(binaryForm, 0);
			for (;;)
			{
				try
				{
					for (int i = 0; i < this.Count; i++)
					{
						QualifiedAce qualifiedAce = this._acl[i] as QualifiedAce;
						if (!(qualifiedAce == null) && (byte)(qualifiedAce.AceFlags & AceFlags.Inherited) == 0 && qualifiedAce.AceQualifier == qualifier && !(qualifiedAce.SecurityIdentifier != sid))
						{
							if (this.IsDS)
							{
								accessMask = num;
								bool flag3 = !this.GetAccessMaskForRemoval(qualifiedAce, objectFlags, objectType, ref accessMask);
								if ((qualifiedAce.AccessMask & accessMask) == 0)
								{
									goto IL_45A;
								}
								flags = aceFlags;
								bool flag4 = !this.GetInheritanceFlagsForRemoval(qualifiedAce, objectFlags, inheritedObjectType, ref flags);
								if (((byte)(qualifiedAce.AceFlags & AceFlags.ContainerInherit) == 0 && (byte)(flags & AceFlags.ContainerInherit) != 0 && (byte)(flags & AceFlags.InheritOnly) != 0) || ((byte)(flags & AceFlags.ContainerInherit) == 0 && (byte)(qualifiedAce.AceFlags & AceFlags.ContainerInherit) != 0 && (byte)(qualifiedAce.AceFlags & AceFlags.InheritOnly) != 0) || ((byte)(aceFlags & AceFlags.ContainerInherit) != 0 && (byte)(aceFlags & AceFlags.InheritOnly) != 0 && (byte)(flags & AceFlags.ContainerInherit) == 0))
								{
									goto IL_45A;
								}
								if (flag3 || flag4)
								{
									flag2 = false;
									break;
								}
							}
							else if ((qualifiedAce.AccessMask & accessMask) == 0)
							{
								goto IL_45A;
							}
							if (!saclSemantics || ((byte)(qualifiedAce.AceFlags & flags) & 192) != 0)
							{
								ObjectAceFlags objectAceFlags = ObjectAceFlags.None;
								Guid empty = Guid.Empty;
								Guid empty2 = Guid.Empty;
								AceFlags aceFlags2 = AceFlags.None;
								int accessMask2 = 0;
								ObjectAceFlags flags2 = ObjectAceFlags.None;
								Guid empty3 = Guid.Empty;
								Guid empty4 = Guid.Empty;
								ObjectAceFlags flags3 = ObjectAceFlags.None;
								Guid empty5 = Guid.Empty;
								Guid empty6 = Guid.Empty;
								AceFlags aceFlags3 = AceFlags.None;
								bool flag5 = false;
								AceFlags aceFlags4 = qualifiedAce.AceFlags;
								int num2 = qualifiedAce.AccessMask & ~accessMask;
								if (qualifiedAce is ObjectAce)
								{
									this.GetObjectTypesForSplit(qualifiedAce as ObjectAce, num2, aceFlags4, out objectAceFlags, out empty, out empty2);
								}
								if (saclSemantics)
								{
									aceFlags2 = (qualifiedAce.AceFlags & ~(flags & AceFlags.AuditFlags));
									accessMask2 = (qualifiedAce.AccessMask & accessMask);
									if (qualifiedAce is ObjectAce)
									{
										this.GetObjectTypesForSplit(qualifiedAce as ObjectAce, accessMask2, aceFlags2, out flags2, out empty3, out empty4);
									}
								}
								AceFlags aceFlags5 = (AceFlags)((byte)(qualifiedAce.AceFlags & AceFlags.InheritanceFlags) | ((byte)(flags & qualifiedAce.AceFlags) & 192));
								int accessMask3 = qualifiedAce.AccessMask & accessMask;
								if (!saclSemantics || (byte)(aceFlags5 & AceFlags.AuditFlags) != 0)
								{
									if (!CommonAcl.RemoveInheritanceBits(aceFlags5, flags, this.IsDS, out aceFlags3, out flag5))
									{
										flag2 = false;
										break;
									}
									if (!flag5)
									{
										aceFlags3 |= (aceFlags5 & AceFlags.AuditFlags);
										if (qualifiedAce is ObjectAce)
										{
											this.GetObjectTypesForSplit(qualifiedAce as ObjectAce, accessMask3, aceFlags3, out flags3, out empty5, out empty6);
										}
									}
								}
								if (!flag)
								{
									if (num2 != 0)
									{
										if (qualifiedAce is ObjectAce && (((ObjectAce)qualifiedAce).ObjectAceFlags & ObjectAceFlags.ObjectAceTypePresent) != ObjectAceFlags.None && (objectAceFlags & ObjectAceFlags.ObjectAceTypePresent) == ObjectAceFlags.None)
										{
											this._acl.RemoveAce(i);
											ObjectAce ace = new ObjectAce(aceFlags4, qualifier, num2, qualifiedAce.SecurityIdentifier, objectAceFlags, empty, empty2, false, null);
											this._acl.InsertAce(i, ace);
										}
										else
										{
											qualifiedAce.AceFlags = aceFlags4;
											qualifiedAce.AccessMask = num2;
											if (qualifiedAce is ObjectAce)
											{
												ObjectAce objectAce = qualifiedAce as ObjectAce;
												objectAce.ObjectAceFlags = objectAceFlags;
												objectAce.ObjectAceType = empty;
												objectAce.InheritedObjectAceType = empty2;
											}
										}
									}
									else
									{
										this._acl.RemoveAce(i);
										i--;
									}
									if (saclSemantics && (byte)(aceFlags2 & AceFlags.AuditFlags) != 0)
									{
										QualifiedAce ace2;
										if (qualifiedAce is CommonAce)
										{
											ace2 = new CommonAce(aceFlags2, qualifier, accessMask2, qualifiedAce.SecurityIdentifier, false, null);
										}
										else
										{
											ace2 = new ObjectAce(aceFlags2, qualifier, accessMask2, qualifiedAce.SecurityIdentifier, flags2, empty3, empty4, false, null);
										}
										i++;
										this._acl.InsertAce(i, ace2);
									}
									if (!flag5)
									{
										QualifiedAce ace2;
										if (qualifiedAce is CommonAce)
										{
											ace2 = new CommonAce(aceFlags3, qualifier, accessMask3, qualifiedAce.SecurityIdentifier, false, null);
										}
										else
										{
											ace2 = new ObjectAce(aceFlags3, qualifier, accessMask3, qualifiedAce.SecurityIdentifier, flags3, empty5, empty6, false, null);
										}
										i++;
										this._acl.InsertAce(i, ace2);
									}
								}
							}
						}
						IL_45A:;
					}
				}
				catch (OverflowException)
				{
					this._acl.SetBinaryForm(binaryForm, 0);
					return false;
				}
				if (!flag || !flag2)
				{
					break;
				}
				flag = false;
			}
			this.OnAclModificationTried();
			return flag2;
		}

		// Token: 0x06005259 RID: 21081 RVA: 0x0012927C File Offset: 0x0012827C
		internal void RemoveQualifiedAcesSpecific(SecurityIdentifier sid, AceQualifier qualifier, int accessMask, AceFlags flags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
		{
			this.ThrowIfNotCanonical();
			if (accessMask == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ArgumentZero"), "accessMask");
			}
			if (qualifier == AceQualifier.SystemAudit && (byte)(flags & AceFlags.AuditFlags) == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumAtLeastOneFlag"), "flags");
			}
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			for (int i = 0; i < this.Count; i++)
			{
				QualifiedAce qualifiedAce = this._acl[i] as QualifiedAce;
				if (!(qualifiedAce == null) && (byte)(qualifiedAce.AceFlags & AceFlags.Inherited) == 0 && qualifiedAce.AceQualifier == qualifier && !(qualifiedAce.SecurityIdentifier != sid) && qualifiedAce.AceFlags == flags && qualifiedAce.AccessMask == accessMask)
				{
					if (this.IsDS)
					{
						if (qualifiedAce is ObjectAce && objectFlags != ObjectAceFlags.None)
						{
							ObjectAce objectAce = qualifiedAce as ObjectAce;
							if (!objectAce.ObjectTypesMatch(objectFlags, objectType))
							{
								goto IL_102;
							}
							if (!objectAce.InheritedObjectTypesMatch(objectFlags, inheritedObjectType))
							{
								goto IL_102;
							}
						}
						else if (qualifiedAce is ObjectAce || objectFlags != ObjectAceFlags.None)
						{
							goto IL_102;
						}
					}
					this._acl.RemoveAce(i);
					i--;
				}
				IL_102:;
			}
			this.OnAclModificationTried();
		}

		// Token: 0x0600525A RID: 21082 RVA: 0x001293A1 File Offset: 0x001283A1
		internal virtual void OnAclModificationTried()
		{
		}

		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x0600525B RID: 21083 RVA: 0x001293A3 File Offset: 0x001283A3
		public sealed override byte Revision
		{
			get
			{
				return this._acl.Revision;
			}
		}

		// Token: 0x17000E51 RID: 3665
		// (get) Token: 0x0600525C RID: 21084 RVA: 0x001293B0 File Offset: 0x001283B0
		public sealed override int Count
		{
			get
			{
				this.CanonicalizeIfNecessary();
				return this._acl.Count;
			}
		}

		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x0600525D RID: 21085 RVA: 0x001293C3 File Offset: 0x001283C3
		public sealed override int BinaryLength
		{
			get
			{
				this.CanonicalizeIfNecessary();
				return this._acl.BinaryLength;
			}
		}

		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x0600525E RID: 21086 RVA: 0x001293D6 File Offset: 0x001283D6
		public bool IsCanonical
		{
			get
			{
				return this._isCanonical;
			}
		}

		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x0600525F RID: 21087 RVA: 0x001293DE File Offset: 0x001283DE
		public bool IsContainer
		{
			get
			{
				return this._isContainer;
			}
		}

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06005260 RID: 21088 RVA: 0x001293E6 File Offset: 0x001283E6
		public bool IsDS
		{
			get
			{
				return this._isDS;
			}
		}

		// Token: 0x06005261 RID: 21089 RVA: 0x001293EE File Offset: 0x001283EE
		public sealed override void GetBinaryForm(byte[] binaryForm, int offset)
		{
			this.CanonicalizeIfNecessary();
			this._acl.GetBinaryForm(binaryForm, offset);
		}

		// Token: 0x17000E56 RID: 3670
		public sealed override GenericAce this[int index]
		{
			get
			{
				this.CanonicalizeIfNecessary();
				return this._acl[index].Copy();
			}
			set
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SetMethod"));
			}
		}

		// Token: 0x06005264 RID: 21092 RVA: 0x00129430 File Offset: 0x00128430
		public void RemoveInheritedAces()
		{
			this.ThrowIfNotCanonical();
			for (int i = this._acl.Count - 1; i >= 0; i--)
			{
				GenericAce genericAce = this._acl[i];
				if ((byte)(genericAce.AceFlags & AceFlags.Inherited) != 0)
				{
					this._acl.RemoveAce(i);
				}
			}
			this.OnAclModificationTried();
		}

		// Token: 0x06005265 RID: 21093 RVA: 0x00129488 File Offset: 0x00128488
		public void Purge(SecurityIdentifier sid)
		{
			this.ThrowIfNotCanonical();
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			for (int i = this.Count - 1; i >= 0; i--)
			{
				KnownAce knownAce = this._acl[i] as KnownAce;
				if (!(knownAce == null) && (byte)(knownAce.AceFlags & AceFlags.Inherited) == 0 && knownAce.SecurityIdentifier == sid)
				{
					this._acl.RemoveAce(i);
				}
			}
			this.OnAclModificationTried();
		}

		// Token: 0x04002A80 RID: 10880
		private static CommonAcl.PM[] AFtoPM = new CommonAcl.PM[16];

		// Token: 0x04002A81 RID: 10881
		private static CommonAcl.AF[] PMtoAF;

		// Token: 0x04002A82 RID: 10882
		private RawAcl _acl;

		// Token: 0x04002A83 RID: 10883
		private bool _isDirty;

		// Token: 0x04002A84 RID: 10884
		private readonly bool _isCanonical;

		// Token: 0x04002A85 RID: 10885
		private readonly bool _isContainer;

		// Token: 0x04002A86 RID: 10886
		private readonly bool _isDS;

		// Token: 0x020008D8 RID: 2264
		[Flags]
		private enum AF
		{
			// Token: 0x04002A88 RID: 10888
			CI = 8,
			// Token: 0x04002A89 RID: 10889
			OI = 4,
			// Token: 0x04002A8A RID: 10890
			IO = 2,
			// Token: 0x04002A8B RID: 10891
			NP = 1,
			// Token: 0x04002A8C RID: 10892
			Invalid = 1
		}

		// Token: 0x020008D9 RID: 2265
		[Flags]
		private enum PM
		{
			// Token: 0x04002A8E RID: 10894
			F = 16,
			// Token: 0x04002A8F RID: 10895
			CF = 8,
			// Token: 0x04002A90 RID: 10896
			CO = 4,
			// Token: 0x04002A91 RID: 10897
			GF = 2,
			// Token: 0x04002A92 RID: 10898
			GO = 1,
			// Token: 0x04002A93 RID: 10899
			Invalid = 1
		}

		// Token: 0x020008DA RID: 2266
		private enum ComparisonResult
		{
			// Token: 0x04002A95 RID: 10901
			LessThan,
			// Token: 0x04002A96 RID: 10902
			EqualTo,
			// Token: 0x04002A97 RID: 10903
			GreaterThan
		}
	}
}

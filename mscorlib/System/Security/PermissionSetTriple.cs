using System;
using System.Security.Permissions;

namespace System.Security
{
	// Token: 0x0200067E RID: 1662
	[Serializable]
	internal sealed class PermissionSetTriple
	{
		// Token: 0x06003BFD RID: 15357 RVA: 0x000CCF0C File Offset: 0x000CBF0C
		internal PermissionSetTriple()
		{
			this.Reset();
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x000CCF1A File Offset: 0x000CBF1A
		internal PermissionSetTriple(PermissionSetTriple triple)
		{
			this.AssertSet = triple.AssertSet;
			this.GrantSet = triple.GrantSet;
			this.RefusedSet = triple.RefusedSet;
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x000CCF46 File Offset: 0x000CBF46
		internal void Reset()
		{
			this.AssertSet = null;
			this.GrantSet = null;
			this.RefusedSet = null;
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x000CCF5D File Offset: 0x000CBF5D
		internal bool IsEmpty()
		{
			return this.AssertSet == null && this.GrantSet == null && this.RefusedSet == null;
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x06003C01 RID: 15361 RVA: 0x000CCF7A File Offset: 0x000CBF7A
		private PermissionToken ZoneToken
		{
			get
			{
				if (PermissionSetTriple.s_zoneToken == null)
				{
					PermissionSetTriple.s_zoneToken = PermissionToken.GetToken(typeof(ZoneIdentityPermission));
				}
				return PermissionSetTriple.s_zoneToken;
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x06003C02 RID: 15362 RVA: 0x000CCF9C File Offset: 0x000CBF9C
		private PermissionToken UrlToken
		{
			get
			{
				if (PermissionSetTriple.s_urlToken == null)
				{
					PermissionSetTriple.s_urlToken = PermissionToken.GetToken(typeof(UrlIdentityPermission));
				}
				return PermissionSetTriple.s_urlToken;
			}
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x000CCFC0 File Offset: 0x000CBFC0
		internal bool Update(PermissionSetTriple psTriple, out PermissionSetTriple retTriple)
		{
			retTriple = null;
			retTriple = this.UpdateAssert(psTriple.AssertSet);
			if (psTriple.AssertSet != null && psTriple.AssertSet.IsUnrestricted())
			{
				return true;
			}
			this.UpdateGrant(psTriple.GrantSet);
			this.UpdateRefused(psTriple.RefusedSet);
			return false;
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x000CD010 File Offset: 0x000CC010
		internal PermissionSetTriple UpdateAssert(PermissionSet in_a)
		{
			PermissionSetTriple permissionSetTriple = null;
			if (in_a != null)
			{
				if (in_a.IsSubsetOf(this.AssertSet))
				{
					return null;
				}
				PermissionSet permissionSet;
				if (this.GrantSet != null)
				{
					permissionSet = in_a.Intersect(this.GrantSet);
				}
				else
				{
					this.GrantSet = new PermissionSet(true);
					permissionSet = in_a.Copy();
				}
				bool flag = false;
				if (this.RefusedSet != null)
				{
					permissionSet = PermissionSet.RemoveRefusedPermissionSet(permissionSet, this.RefusedSet, out flag);
				}
				if (!flag)
				{
					flag = PermissionSet.IsIntersectingAssertedPermissions(permissionSet, this.AssertSet);
				}
				if (flag)
				{
					permissionSetTriple = new PermissionSetTriple(this);
					this.Reset();
					this.GrantSet = permissionSetTriple.GrantSet.Copy();
				}
				if (this.AssertSet == null)
				{
					this.AssertSet = permissionSet;
				}
				else
				{
					this.AssertSet.InplaceUnion(permissionSet);
				}
			}
			return permissionSetTriple;
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x000CD0C8 File Offset: 0x000CC0C8
		internal void UpdateGrant(PermissionSet in_g, out ZoneIdentityPermission z, out UrlIdentityPermission u)
		{
			z = null;
			u = null;
			if (in_g != null)
			{
				if (this.GrantSet == null)
				{
					this.GrantSet = in_g.Copy();
				}
				else
				{
					this.GrantSet.InplaceIntersect(in_g);
				}
				z = (ZoneIdentityPermission)in_g.GetPermission(this.ZoneToken);
				u = (UrlIdentityPermission)in_g.GetPermission(this.UrlToken);
			}
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x000CD126 File Offset: 0x000CC126
		internal void UpdateGrant(PermissionSet in_g)
		{
			if (in_g != null)
			{
				if (this.GrantSet == null)
				{
					this.GrantSet = in_g.Copy();
					return;
				}
				this.GrantSet.InplaceIntersect(in_g);
			}
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x000CD14C File Offset: 0x000CC14C
		internal void UpdateRefused(PermissionSet in_r)
		{
			if (in_r != null)
			{
				if (this.RefusedSet == null)
				{
					this.RefusedSet = in_r.Copy();
					return;
				}
				this.RefusedSet.InplaceUnion(in_r);
			}
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x000CD174 File Offset: 0x000CC174
		private static bool CheckAssert(PermissionSet pSet, CodeAccessPermission demand, PermissionToken permToken)
		{
			if (pSet != null)
			{
				pSet.CheckDecoded(demand, permToken);
				CodeAccessPermission asserted = (CodeAccessPermission)pSet.GetPermission(demand);
				try
				{
					if ((pSet.IsUnrestricted() && demand.CanUnrestrictedOverride()) || demand.CheckAssert(asserted))
					{
						return false;
					}
				}
				catch (ArgumentException)
				{
				}
				return true;
			}
			return true;
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x000CD1D0 File Offset: 0x000CC1D0
		private static bool CheckAssert(PermissionSet assertPset, PermissionSet demandSet, out PermissionSet newDemandSet)
		{
			newDemandSet = null;
			if (assertPset != null)
			{
				assertPset.CheckDecoded(demandSet);
				if (demandSet.CheckAssertion(assertPset))
				{
					return false;
				}
				PermissionSet.RemoveAssertedPermissionSet(demandSet, assertPset, out newDemandSet);
			}
			return true;
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x000CD1F3 File Offset: 0x000CC1F3
		internal bool CheckDemand(CodeAccessPermission demand, PermissionToken permToken, RuntimeMethodHandle rmh)
		{
			if (!PermissionSetTriple.CheckAssert(this.AssertSet, demand, permToken))
			{
				return false;
			}
			CodeAccessSecurityEngine.CheckHelper(this.GrantSet, this.RefusedSet, demand, permToken, rmh, null, SecurityAction.Demand, true);
			return true;
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x000CD21F File Offset: 0x000CC21F
		internal bool CheckSetDemand(PermissionSet demandSet, out PermissionSet alteredDemandset, RuntimeMethodHandle rmh)
		{
			alteredDemandset = null;
			if (!PermissionSetTriple.CheckAssert(this.AssertSet, demandSet, out alteredDemandset))
			{
				return false;
			}
			if (alteredDemandset != null)
			{
				demandSet = alteredDemandset;
			}
			CodeAccessSecurityEngine.CheckSetHelper(this.GrantSet, this.RefusedSet, demandSet, rmh, null, SecurityAction.Demand, true);
			return true;
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x000CD255 File Offset: 0x000CC255
		internal bool CheckDemandNoThrow(CodeAccessPermission demand, PermissionToken permToken)
		{
			return CodeAccessSecurityEngine.CheckHelper(this.GrantSet, this.RefusedSet, demand, permToken, PermissionSetTriple.s_emptyRMH, null, SecurityAction.Demand, false);
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x000CD272 File Offset: 0x000CC272
		internal bool CheckSetDemandNoThrow(PermissionSet demandSet)
		{
			return CodeAccessSecurityEngine.CheckSetHelper(this.GrantSet, this.RefusedSet, demandSet, PermissionSetTriple.s_emptyRMH, null, SecurityAction.Demand, false);
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x000CD290 File Offset: 0x000CC290
		internal bool CheckFlags(ref int flags)
		{
			if (this.AssertSet != null)
			{
				int specialFlags = SecurityManager.GetSpecialFlags(this.AssertSet, null);
				if ((flags & specialFlags) != 0)
				{
					flags &= ~specialFlags;
				}
			}
			return (SecurityManager.GetSpecialFlags(this.GrantSet, this.RefusedSet) & flags) == flags;
		}

		// Token: 0x04001EFA RID: 7930
		private static RuntimeMethodHandle s_emptyRMH = new RuntimeMethodHandle(null);

		// Token: 0x04001EFB RID: 7931
		private static PermissionToken s_zoneToken;

		// Token: 0x04001EFC RID: 7932
		private static PermissionToken s_urlToken;

		// Token: 0x04001EFD RID: 7933
		internal PermissionSet AssertSet;

		// Token: 0x04001EFE RID: 7934
		internal PermissionSet GrantSet;

		// Token: 0x04001EFF RID: 7935
		internal PermissionSet RefusedSet;
	}
}

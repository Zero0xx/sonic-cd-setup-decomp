using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Security
{
	// Token: 0x0200067F RID: 1663
	[Serializable]
	internal sealed class PermissionListSet
	{
		// Token: 0x06003C10 RID: 15376 RVA: 0x000CD2E5 File Offset: 0x000CC2E5
		internal PermissionListSet()
		{
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x000CD2ED File Offset: 0x000CC2ED
		private void EnsureTriplesListCreated()
		{
			if (this.m_permSetTriples == null)
			{
				this.m_permSetTriples = new ArrayList();
				if (this.m_firstPermSetTriple != null)
				{
					this.m_permSetTriples.Add(this.m_firstPermSetTriple);
					this.m_firstPermSetTriple = null;
				}
			}
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x000CD323 File Offset: 0x000CC323
		internal void UpdateDomainPLS(PermissionListSet adPLS)
		{
			if (adPLS != null && adPLS.m_firstPermSetTriple != null)
			{
				this.UpdateDomainPLS(adPLS.m_firstPermSetTriple.GrantSet, adPLS.m_firstPermSetTriple.RefusedSet);
			}
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x000CD34C File Offset: 0x000CC34C
		internal void UpdateDomainPLS(PermissionSet grantSet, PermissionSet deniedSet)
		{
			if (this.m_firstPermSetTriple == null)
			{
				this.m_firstPermSetTriple = new PermissionSetTriple();
			}
			this.m_firstPermSetTriple.UpdateGrant(grantSet);
			this.m_firstPermSetTriple.UpdateRefused(deniedSet);
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x000CD379 File Offset: 0x000CC379
		private void Terminate(PermissionSetTriple currentTriple)
		{
			this.UpdateTripleListAndCreateNewTriple(currentTriple, null);
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x000CD383 File Offset: 0x000CC383
		private void Terminate(PermissionSetTriple currentTriple, PermissionListSet pls)
		{
			this.UpdateZoneAndOrigin(pls);
			this.UpdatePermissions(currentTriple, pls);
			this.UpdateTripleListAndCreateNewTriple(currentTriple, null);
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x000CD39D File Offset: 0x000CC39D
		private bool Update(PermissionSetTriple currentTriple, PermissionListSet pls)
		{
			this.UpdateZoneAndOrigin(pls);
			return this.UpdatePermissions(currentTriple, pls);
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x000CD3B0 File Offset: 0x000CC3B0
		private bool Update(PermissionSetTriple currentTriple, FrameSecurityDescriptor fsd)
		{
			FrameSecurityDescriptorWithResolver frameSecurityDescriptorWithResolver = fsd as FrameSecurityDescriptorWithResolver;
			if (frameSecurityDescriptorWithResolver != null)
			{
				return this.Update2(currentTriple, frameSecurityDescriptorWithResolver);
			}
			bool flag = this.Update2(currentTriple, fsd, false);
			if (!flag)
			{
				flag = this.Update2(currentTriple, fsd, true);
			}
			return flag;
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x000CD3E8 File Offset: 0x000CC3E8
		[SecurityCritical]
		private bool Update2(PermissionSetTriple currentTriple, FrameSecurityDescriptorWithResolver fsdWithResolver)
		{
			DynamicResolver resolver = fsdWithResolver.Resolver;
			CompressedStack securityContext = resolver.GetSecurityContext();
			securityContext.CompleteConstruction(null);
			return this.Update(currentTriple, securityContext.PLS);
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x000CD418 File Offset: 0x000CC418
		private bool Update2(PermissionSetTriple currentTriple, FrameSecurityDescriptor fsd, bool fDeclarative)
		{
			PermissionSet denials = fsd.GetDenials(fDeclarative);
			if (denials != null)
			{
				currentTriple.UpdateRefused(denials);
			}
			PermissionSet permitOnly = fsd.GetPermitOnly(fDeclarative);
			if (permitOnly != null)
			{
				currentTriple.UpdateGrant(permitOnly);
			}
			if (fsd.GetAssertAllPossible())
			{
				if (currentTriple.GrantSet == null)
				{
					currentTriple.GrantSet = PermissionSet.s_fullTrust;
				}
				this.UpdateTripleListAndCreateNewTriple(currentTriple, this.m_permSetTriples);
				currentTriple.GrantSet = PermissionSet.s_fullTrust;
				currentTriple.UpdateAssert(fsd.GetAssertions(fDeclarative));
				return true;
			}
			PermissionSet assertions = fsd.GetAssertions(fDeclarative);
			if (assertions != null)
			{
				if (assertions.IsUnrestricted())
				{
					if (currentTriple.GrantSet == null)
					{
						currentTriple.GrantSet = PermissionSet.s_fullTrust;
					}
					this.UpdateTripleListAndCreateNewTriple(currentTriple, this.m_permSetTriples);
					currentTriple.GrantSet = PermissionSet.s_fullTrust;
					currentTriple.UpdateAssert(assertions);
					return true;
				}
				PermissionSetTriple permissionSetTriple = currentTriple.UpdateAssert(assertions);
				if (permissionSetTriple != null)
				{
					this.EnsureTriplesListCreated();
					this.m_permSetTriples.Add(permissionSetTriple);
				}
			}
			return false;
		}

		// Token: 0x06003C1A RID: 15386 RVA: 0x000CD4F4 File Offset: 0x000CC4F4
		private void Update(PermissionSetTriple currentTriple, PermissionSet in_g, PermissionSet in_r)
		{
			ZoneIdentityPermission z;
			UrlIdentityPermission u;
			currentTriple.UpdateGrant(in_g, out z, out u);
			currentTriple.UpdateRefused(in_r);
			this.AppendZoneOrigin(z, u);
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x000CD51B File Offset: 0x000CC51B
		private void Update(PermissionSet in_g)
		{
			if (this.m_firstPermSetTriple == null)
			{
				this.m_firstPermSetTriple = new PermissionSetTriple();
			}
			this.Update(this.m_firstPermSetTriple, in_g, null);
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x000CD540 File Offset: 0x000CC540
		private void UpdateZoneAndOrigin(PermissionListSet pls)
		{
			if (pls != null)
			{
				if (this.m_zoneList == null && pls.m_zoneList != null && pls.m_zoneList.Count > 0)
				{
					this.m_zoneList = new ArrayList();
				}
				PermissionListSet.UpdateArrayList(this.m_zoneList, pls.m_zoneList);
				if (this.m_originList == null && pls.m_originList != null && pls.m_originList.Count > 0)
				{
					this.m_originList = new ArrayList();
				}
				PermissionListSet.UpdateArrayList(this.m_originList, pls.m_originList);
			}
		}

		// Token: 0x06003C1D RID: 15389 RVA: 0x000CD5C4 File Offset: 0x000CC5C4
		private bool UpdatePermissions(PermissionSetTriple currentTriple, PermissionListSet pls)
		{
			if (pls != null)
			{
				if (pls.m_permSetTriples != null)
				{
					this.UpdateTripleListAndCreateNewTriple(currentTriple, pls.m_permSetTriples);
				}
				else
				{
					PermissionSetTriple firstPermSetTriple = pls.m_firstPermSetTriple;
					PermissionSetTriple permissionSetTriple;
					if (currentTriple.Update(firstPermSetTriple, out permissionSetTriple))
					{
						return true;
					}
					if (permissionSetTriple != null)
					{
						this.EnsureTriplesListCreated();
						this.m_permSetTriples.Add(permissionSetTriple);
					}
				}
			}
			else
			{
				this.UpdateTripleListAndCreateNewTriple(currentTriple, null);
			}
			return false;
		}

		// Token: 0x06003C1E RID: 15390 RVA: 0x000CD620 File Offset: 0x000CC620
		private void UpdateTripleListAndCreateNewTriple(PermissionSetTriple currentTriple, ArrayList tripleList)
		{
			if (!currentTriple.IsEmpty())
			{
				if (this.m_firstPermSetTriple == null && this.m_permSetTriples == null)
				{
					this.m_firstPermSetTriple = new PermissionSetTriple(currentTriple);
				}
				else
				{
					this.EnsureTriplesListCreated();
					this.m_permSetTriples.Add(new PermissionSetTriple(currentTriple));
				}
				currentTriple.Reset();
			}
			if (tripleList != null)
			{
				this.EnsureTriplesListCreated();
				this.m_permSetTriples.AddRange(tripleList);
			}
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x000CD688 File Offset: 0x000CC688
		private static void UpdateArrayList(ArrayList current, ArrayList newList)
		{
			if (newList == null)
			{
				return;
			}
			for (int i = 0; i < newList.Count; i++)
			{
				if (!current.Contains(newList[i]))
				{
					current.Add(newList[i]);
				}
			}
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x000CD6C8 File Offset: 0x000CC6C8
		private void AppendZoneOrigin(ZoneIdentityPermission z, UrlIdentityPermission u)
		{
			if (z != null)
			{
				if (this.m_zoneList == null)
				{
					this.m_zoneList = new ArrayList();
				}
				this.m_zoneList.Add(z.SecurityZone);
			}
			if (u != null)
			{
				if (this.m_originList == null)
				{
					this.m_originList = new ArrayList();
				}
				this.m_originList.Add(u.Url);
			}
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x000CD72C File Offset: 0x000CC72C
		[ComVisible(true)]
		internal static PermissionListSet CreateCompressedState(CompressedStack cs, CompressedStack innerCS)
		{
			bool flag = false;
			if (cs.CompressedStackHandle == null)
			{
				return null;
			}
			PermissionListSet permissionListSet = new PermissionListSet();
			PermissionSetTriple currentTriple = new PermissionSetTriple();
			int dcscount = CompressedStack.GetDCSCount(cs.CompressedStackHandle);
			int num = dcscount - 1;
			while (num >= 0 && !flag)
			{
				DomainCompressedStack domainCompressedStack = CompressedStack.GetDomainCompressedStack(cs.CompressedStackHandle, num);
				if (domainCompressedStack != null)
				{
					if (domainCompressedStack.PLS == null)
					{
						throw new SecurityException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Security_Generic"), new object[0]));
					}
					permissionListSet.UpdateZoneAndOrigin(domainCompressedStack.PLS);
					permissionListSet.Update(currentTriple, domainCompressedStack.PLS);
					flag = domainCompressedStack.ConstructionHalted;
				}
				num--;
			}
			if (!flag)
			{
				PermissionListSet pls = null;
				if (innerCS != null)
				{
					innerCS.CompleteConstruction(null);
					pls = innerCS.PLS;
				}
				permissionListSet.Terminate(currentTriple, pls);
			}
			else
			{
				permissionListSet.Terminate(currentTriple);
			}
			return permissionListSet;
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x000CD800 File Offset: 0x000CC800
		internal static PermissionListSet CreateCompressedState(IntPtr unmanagedDCS, out bool bHaltConstruction)
		{
			PermissionListSet permissionListSet = new PermissionListSet();
			PermissionSetTriple currentTriple = new PermissionSetTriple();
			int descCount = DomainCompressedStack.GetDescCount(unmanagedDCS);
			bHaltConstruction = false;
			int num = 0;
			while (num < descCount && !bHaltConstruction)
			{
				PermissionSet in_g;
				PermissionSet in_r;
				Assembly assembly;
				FrameSecurityDescriptor fsd;
				if (DomainCompressedStack.GetDescriptorInfo(unmanagedDCS, num, out in_g, out in_r, out assembly, out fsd))
				{
					bHaltConstruction = permissionListSet.Update(currentTriple, fsd);
				}
				else
				{
					permissionListSet.Update(currentTriple, in_g, in_r);
				}
				num++;
			}
			if (!bHaltConstruction && !DomainCompressedStack.IgnoreDomain(unmanagedDCS))
			{
				PermissionSet in_g;
				PermissionSet in_r;
				DomainCompressedStack.GetDomainPermissionSets(unmanagedDCS, out in_g, out in_r);
				permissionListSet.Update(currentTriple, in_g, in_r);
			}
			permissionListSet.Terminate(currentTriple);
			return permissionListSet;
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x000CD888 File Offset: 0x000CC888
		internal static PermissionListSet CreateCompressedState_HG()
		{
			PermissionListSet permissionListSet = new PermissionListSet();
			CompressedStack.GetHomogeneousPLS(permissionListSet);
			return permissionListSet;
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x000CD8A4 File Offset: 0x000CC8A4
		internal bool CheckDemandNoThrow(CodeAccessPermission demand)
		{
			PermissionToken permToken = null;
			if (demand != null)
			{
				permToken = PermissionToken.GetToken(demand);
			}
			return this.m_firstPermSetTriple.CheckDemandNoThrow(demand, permToken);
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x000CD8CA File Offset: 0x000CC8CA
		internal bool CheckSetDemandNoThrow(PermissionSet pSet)
		{
			return this.m_firstPermSetTriple.CheckSetDemandNoThrow(pSet);
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x000CD8D8 File Offset: 0x000CC8D8
		internal bool CheckDemand(CodeAccessPermission demand, PermissionToken permToken, RuntimeMethodHandle rmh)
		{
			bool flag = true;
			if (this.m_permSetTriples != null)
			{
				for (int i = 0; i < this.m_permSetTriples.Count; i++)
				{
					if (!flag)
					{
						break;
					}
					PermissionSetTriple permissionSetTriple = (PermissionSetTriple)this.m_permSetTriples[i];
					flag = permissionSetTriple.CheckDemand(demand, permToken, rmh);
				}
			}
			else if (this.m_firstPermSetTriple != null)
			{
				flag = this.m_firstPermSetTriple.CheckDemand(demand, permToken, rmh);
			}
			return flag;
		}

		// Token: 0x06003C27 RID: 15399 RVA: 0x000CD940 File Offset: 0x000CC940
		internal bool CheckSetDemand(PermissionSet pset, RuntimeMethodHandle rmh)
		{
			PermissionSet permissionSet;
			this.CheckSetDemandWithModification(pset, out permissionSet, rmh);
			return false;
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x000CD95C File Offset: 0x000CC95C
		[SecurityCritical]
		internal bool CheckSetDemandWithModification(PermissionSet pset, out PermissionSet alteredDemandSet, RuntimeMethodHandle rmh)
		{
			bool flag = true;
			PermissionSet demandSet = pset;
			alteredDemandSet = null;
			if (this.m_permSetTriples != null)
			{
				for (int i = 0; i < this.m_permSetTriples.Count; i++)
				{
					if (!flag)
					{
						break;
					}
					PermissionSetTriple permissionSetTriple = (PermissionSetTriple)this.m_permSetTriples[i];
					flag = permissionSetTriple.CheckSetDemand(demandSet, out alteredDemandSet, rmh);
					if (alteredDemandSet != null)
					{
						demandSet = alteredDemandSet;
					}
				}
			}
			else if (this.m_firstPermSetTriple != null)
			{
				flag = this.m_firstPermSetTriple.CheckSetDemand(demandSet, out alteredDemandSet, rmh);
			}
			return flag;
		}

		// Token: 0x06003C29 RID: 15401 RVA: 0x000CD9D0 File Offset: 0x000CC9D0
		private bool CheckFlags(int flags)
		{
			bool flag = true;
			if (this.m_permSetTriples != null)
			{
				int num = 0;
				while (num < this.m_permSetTriples.Count && flag)
				{
					if (flags == 0)
					{
						break;
					}
					flag &= ((PermissionSetTriple)this.m_permSetTriples[num]).CheckFlags(ref flags);
					num++;
				}
			}
			else if (this.m_firstPermSetTriple != null)
			{
				flag = this.m_firstPermSetTriple.CheckFlags(ref flags);
			}
			return flag;
		}

		// Token: 0x06003C2A RID: 15402 RVA: 0x000CDA38 File Offset: 0x000CCA38
		internal void DemandFlagsOrGrantSet(int flags, PermissionSet grantSet)
		{
			if (this.CheckFlags(flags))
			{
				return;
			}
			this.CheckSetDemand(grantSet, default(RuntimeMethodHandle));
		}

		// Token: 0x06003C2B RID: 15403 RVA: 0x000CDA60 File Offset: 0x000CCA60
		internal void GetZoneAndOrigin(ArrayList zoneList, ArrayList originList, PermissionToken zoneToken, PermissionToken originToken)
		{
			if (this.m_zoneList != null)
			{
				zoneList.AddRange(this.m_zoneList);
			}
			if (this.m_originList != null)
			{
				originList.AddRange(this.m_originList);
			}
		}

		// Token: 0x04001F00 RID: 7936
		private PermissionSetTriple m_firstPermSetTriple;

		// Token: 0x04001F01 RID: 7937
		private ArrayList m_permSetTriples;

		// Token: 0x04001F02 RID: 7938
		private ArrayList m_zoneList;

		// Token: 0x04001F03 RID: 7939
		private ArrayList m_originList;
	}
}

using System;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Util;
using System.Text;
using System.Threading;

namespace System.Security
{
	// Token: 0x02000680 RID: 1664
	internal class PolicyManager
	{
		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x06003C2C RID: 15404 RVA: 0x000CDA8C File Offset: 0x000CCA8C
		private IList PolicyLevels
		{
			get
			{
				if (this.m_policyLevels == null)
				{
					ArrayList arrayList = new ArrayList();
					string locationFromType = PolicyLevel.GetLocationFromType(PolicyLevelType.Enterprise);
					arrayList.Add(new PolicyLevel(PolicyLevelType.Enterprise, locationFromType, ConfigId.EnterprisePolicyLevel));
					string locationFromType2 = PolicyLevel.GetLocationFromType(PolicyLevelType.Machine);
					arrayList.Add(new PolicyLevel(PolicyLevelType.Machine, locationFromType2, ConfigId.MachinePolicyLevel));
					if (Config.UserDirectory != null)
					{
						string locationFromType3 = PolicyLevel.GetLocationFromType(PolicyLevelType.User);
						arrayList.Add(new PolicyLevel(PolicyLevelType.User, locationFromType3, ConfigId.UserPolicyLevel));
					}
					Interlocked.CompareExchange(ref this.m_policyLevels, arrayList, null);
				}
				return this.m_policyLevels as ArrayList;
			}
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x000CDB09 File Offset: 0x000CCB09
		internal PolicyManager()
		{
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x000CDB11 File Offset: 0x000CCB11
		internal void AddLevel(PolicyLevel level)
		{
			this.PolicyLevels.Add(level);
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x000CDB20 File Offset: 0x000CCB20
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
		internal IEnumerator PolicyHierarchy()
		{
			return this.PolicyLevels.GetEnumerator();
		}

		// Token: 0x06003C30 RID: 15408 RVA: 0x000CDB30 File Offset: 0x000CCB30
		internal PermissionSet Resolve(Evidence evidence)
		{
			if (!PolicyManager.IsGacAssembly(evidence))
			{
				HostSecurityManager hostSecurityManager = AppDomain.CurrentDomain.HostSecurityManager;
				if ((hostSecurityManager.Flags & HostSecurityManagerOptions.HostResolvePolicy) == HostSecurityManagerOptions.HostResolvePolicy)
				{
					return hostSecurityManager.ResolvePolicy(evidence);
				}
			}
			return this.ResolveHelper(evidence);
		}

		// Token: 0x06003C31 RID: 15409 RVA: 0x000CDB6C File Offset: 0x000CCB6C
		internal PermissionSet ResolveHelper(Evidence evidence)
		{
			PermissionSet result;
			if (PolicyManager.IsGacAssembly(evidence))
			{
				result = new PermissionSet(PermissionState.Unrestricted);
			}
			else
			{
				ApplicationTrust applicationTrust = AppDomain.CurrentDomain.ApplicationTrust;
				if (applicationTrust != null)
				{
					if (PolicyManager.IsFullTrust(evidence, applicationTrust))
					{
						result = new PermissionSet(PermissionState.Unrestricted);
					}
					else
					{
						result = applicationTrust.DefaultGrantSet.PermissionSet;
					}
				}
				else
				{
					result = this.CodeGroupResolve(evidence, false);
				}
			}
			return result;
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x000CDBC4 File Offset: 0x000CCBC4
		internal PermissionSet CodeGroupResolve(Evidence evidence, bool systemPolicy)
		{
			PermissionSet permissionSet = null;
			IEnumerator enumerator = this.PolicyLevels.GetEnumerator();
			char[] serializedEvidence = PolicyManager.MakeEvidenceArray(evidence, false);
			int count = evidence.Count;
			bool flag = AppDomain.CurrentDomain.GetData("IgnoreSystemPolicy") != null;
			bool flag2 = false;
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PolicyLevel policyLevel = (PolicyLevel)obj;
				if (systemPolicy)
				{
					if (policyLevel.Type == PolicyLevelType.AppDomain)
					{
						continue;
					}
				}
				else if (flag && policyLevel.Type != PolicyLevelType.AppDomain)
				{
					continue;
				}
				PolicyStatement policyStatement = policyLevel.Resolve(evidence, count, serializedEvidence);
				if (permissionSet == null)
				{
					permissionSet = policyStatement.PermissionSet;
				}
				else
				{
					permissionSet.InplaceIntersect(policyStatement.GetPermissionSetNoCopy());
				}
				if (permissionSet == null || permissionSet.FastIsEmpty())
				{
					break;
				}
				if ((policyStatement.Attributes & PolicyStatementAttribute.LevelFinal) == PolicyStatementAttribute.LevelFinal)
				{
					if (policyLevel.Type != PolicyLevelType.AppDomain)
					{
						flag2 = true;
						break;
					}
					break;
				}
			}
			if (permissionSet != null && flag2)
			{
				PolicyLevel policyLevel2 = null;
				for (int i = this.PolicyLevels.Count - 1; i >= 0; i--)
				{
					PolicyLevel policyLevel = (PolicyLevel)this.PolicyLevels[i];
					if (policyLevel.Type == PolicyLevelType.AppDomain)
					{
						policyLevel2 = policyLevel;
						break;
					}
				}
				if (policyLevel2 != null)
				{
					PolicyStatement policyStatement = policyLevel2.Resolve(evidence, count, serializedEvidence);
					permissionSet.InplaceIntersect(policyStatement.GetPermissionSetNoCopy());
				}
			}
			if (permissionSet == null)
			{
				permissionSet = new PermissionSet(PermissionState.None);
			}
			if (!CodeAccessSecurityEngine.DoesFullTrustMeanFullTrust() || !permissionSet.IsUnrestricted())
			{
				IEnumerator hostEnumerator = evidence.GetHostEnumerator();
				while (hostEnumerator.MoveNext())
				{
					object obj2 = hostEnumerator.Current;
					IIdentityPermissionFactory identityPermissionFactory = obj2 as IIdentityPermissionFactory;
					if (identityPermissionFactory != null)
					{
						IPermission permission = identityPermissionFactory.CreateIdentityPermission(evidence);
						if (permission != null)
						{
							permissionSet.AddPermission(permission);
						}
					}
				}
			}
			permissionSet.IgnoreTypeLoadFailures = true;
			return permissionSet;
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x000CDD4A File Offset: 0x000CCD4A
		internal static bool IsGacAssembly(Evidence evidence)
		{
			return new GacMembershipCondition().Check(evidence);
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x000CDD58 File Offset: 0x000CCD58
		private static bool IsFullTrust(Evidence evidence, ApplicationTrust appTrust)
		{
			if (appTrust == null)
			{
				return false;
			}
			StrongName[] fullTrustAssemblies = appTrust.FullTrustAssemblies;
			if (fullTrustAssemblies != null)
			{
				for (int i = 0; i < fullTrustAssemblies.Length; i++)
				{
					if (fullTrustAssemblies[i] != null)
					{
						StrongNameMembershipCondition strongNameMembershipCondition = new StrongNameMembershipCondition(fullTrustAssemblies[i].PublicKey, fullTrustAssemblies[i].Name, fullTrustAssemblies[i].Version);
						object obj = null;
						if (((IReportMatchMembershipCondition)strongNameMembershipCondition).Check(evidence, out obj))
						{
							IDelayEvaluatedEvidence delayEvaluatedEvidence = obj as IDelayEvaluatedEvidence;
							if (obj != null)
							{
								delayEvaluatedEvidence.MarkUsed();
							}
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x000CDDC8 File Offset: 0x000CCDC8
		internal IEnumerator ResolveCodeGroups(Evidence evidence)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.PolicyLevels)
			{
				CodeGroup codeGroup = ((PolicyLevel)obj).ResolveMatchingCodeGroups(evidence);
				if (codeGroup != null)
				{
					arrayList.Add(codeGroup);
				}
			}
			return arrayList.GetEnumerator(0, arrayList.Count);
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x000CDE1B File Offset: 0x000CCE1B
		internal static PolicyStatement ResolveCodeGroup(CodeGroup codeGroup, Evidence evidence)
		{
			if (codeGroup.GetType().Assembly != typeof(UnionCodeGroup).Assembly)
			{
				evidence.MarkAllEvidenceAsUsed();
			}
			return codeGroup.Resolve(evidence);
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x000CDE48 File Offset: 0x000CCE48
		internal static bool CheckMembershipCondition(IMembershipCondition membershipCondition, Evidence evidence, out object usedEvidence)
		{
			IReportMatchMembershipCondition reportMatchMembershipCondition = membershipCondition as IReportMatchMembershipCondition;
			if (reportMatchMembershipCondition != null)
			{
				return reportMatchMembershipCondition.Check(evidence, out usedEvidence);
			}
			usedEvidence = null;
			evidence.MarkAllEvidenceAsUsed();
			return membershipCondition.Check(evidence);
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x000CDE78 File Offset: 0x000CCE78
		internal void Save()
		{
			this.EncodeLevel(Environment.GetResourceString("Policy_PL_Enterprise"));
			this.EncodeLevel(Environment.GetResourceString("Policy_PL_Machine"));
			this.EncodeLevel(Environment.GetResourceString("Policy_PL_User"));
		}

		// Token: 0x06003C39 RID: 15417 RVA: 0x000CDEAC File Offset: 0x000CCEAC
		private void EncodeLevel(string label)
		{
			for (int i = 0; i < this.PolicyLevels.Count; i++)
			{
				PolicyLevel policyLevel = (PolicyLevel)this.PolicyLevels[i];
				if (policyLevel.Label.Equals(label))
				{
					PolicyManager.EncodeLevel(policyLevel);
					return;
				}
			}
		}

		// Token: 0x06003C3A RID: 15418 RVA: 0x000CDEF8 File Offset: 0x000CCEF8
		internal static void EncodeLevel(PolicyLevel level)
		{
			SecurityElement securityElement = new SecurityElement("configuration");
			SecurityElement securityElement2 = new SecurityElement("mscorlib");
			SecurityElement securityElement3 = new SecurityElement("security");
			SecurityElement securityElement4 = new SecurityElement("policy");
			securityElement.AddChild(securityElement2);
			securityElement2.AddChild(securityElement3);
			securityElement3.AddChild(securityElement4);
			securityElement4.AddChild(level.ToXml());
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				Encoding utf = Encoding.UTF8;
				SecurityElement securityElement5 = new SecurityElement("xml");
				securityElement5.m_type = SecurityElementType.Format;
				securityElement5.AddAttribute("version", "1.0");
				securityElement5.AddAttribute("encoding", utf.WebName);
				stringBuilder.Append(securityElement5.ToString());
				stringBuilder.Append(securityElement.ToString());
				byte[] bytes = utf.GetBytes(stringBuilder.ToString());
				if (level.Path == null || !Config.SaveDataByte(level.Path, bytes, 0, bytes.Length))
				{
					throw new PolicyException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_UnableToSave"), new object[]
					{
						level.Label
					}));
				}
			}
			catch (Exception ex)
			{
				if (ex is PolicyException)
				{
					throw ex;
				}
				throw new PolicyException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_UnableToSave"), new object[]
				{
					level.Label
				}), ex);
			}
			catch
			{
				throw new PolicyException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_UnableToSave"), new object[]
				{
					level.Label
				}));
			}
			Config.ResetCacheData(level.ConfigId);
			if (PolicyManager.CanUseQuickCache(level.RootCodeGroup))
			{
				Config.SetQuickCache(level.ConfigId, PolicyManager.GenerateQuickCache(level));
			}
		}

		// Token: 0x06003C3B RID: 15419 RVA: 0x000CE0C4 File Offset: 0x000CD0C4
		internal static bool CanUseQuickCache(CodeGroup group)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.Add(group);
			for (int i = 0; i < arrayList.Count; i++)
			{
				group = (CodeGroup)arrayList[i];
				IUnionSemanticCodeGroup unionSemanticCodeGroup = group as IUnionSemanticCodeGroup;
				if (unionSemanticCodeGroup == null)
				{
					return false;
				}
				if (!PolicyManager.TestPolicyStatement(group.PolicyStatement))
				{
					return false;
				}
				IMembershipCondition membershipCondition = group.MembershipCondition;
				if (membershipCondition != null && !(membershipCondition is IConstantMembershipCondition))
				{
					return false;
				}
				IList children = group.Children;
				if (children != null && children.Count > 0)
				{
					foreach (object value in children)
					{
						arrayList.Add(value);
					}
				}
			}
			return true;
		}

		// Token: 0x06003C3C RID: 15420 RVA: 0x000CE169 File Offset: 0x000CD169
		private static bool TestPolicyStatement(PolicyStatement policy)
		{
			return policy == null || (policy.Attributes & PolicyStatementAttribute.Exclusive) == PolicyStatementAttribute.Nothing;
		}

		// Token: 0x06003C3D RID: 15421 RVA: 0x000CE17C File Offset: 0x000CD17C
		private static QuickCacheEntryType GenerateQuickCache(PolicyLevel level)
		{
			QuickCacheEntryType[] array = new QuickCacheEntryType[]
			{
				QuickCacheEntryType.FullTrustZoneMyComputer,
				QuickCacheEntryType.FullTrustZoneIntranet,
				QuickCacheEntryType.FullTrustZoneInternet,
				QuickCacheEntryType.FullTrustZoneTrusted,
				QuickCacheEntryType.FullTrustZoneUntrusted
			};
			QuickCacheEntryType quickCacheEntryType = (QuickCacheEntryType)0;
			Evidence evidence = new Evidence();
			try
			{
				PermissionSet permissionSet = level.Resolve(evidence).PermissionSet;
				if (permissionSet.IsUnrestricted())
				{
					quickCacheEntryType |= QuickCacheEntryType.FullTrustAll;
				}
			}
			catch (PolicyException)
			{
			}
			Array values = Enum.GetValues(typeof(SecurityZone));
			for (int i = 0; i < values.Length; i++)
			{
				if ((SecurityZone)values.GetValue(i) != SecurityZone.NoZone)
				{
					Evidence evidence2 = new Evidence();
					evidence2.AddHost(new Zone((SecurityZone)values.GetValue(i)));
					try
					{
						PermissionSet permissionSet2 = level.Resolve(evidence2).PermissionSet;
						if (permissionSet2.IsUnrestricted())
						{
							quickCacheEntryType |= array[i];
						}
					}
					catch (PolicyException)
					{
					}
				}
			}
			return quickCacheEntryType;
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x000CE28C File Offset: 0x000CD28C
		internal static char[] MakeEvidenceArray(Evidence evidence, bool verbose)
		{
			IEnumerator enumerator = evidence.GetEnumerator();
			int num = 0;
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				IBuiltInEvidence builtInEvidence = obj as IBuiltInEvidence;
				if (builtInEvidence == null)
				{
					return null;
				}
				num += builtInEvidence.GetRequiredSize(verbose);
			}
			enumerator.Reset();
			char[] array = new char[num];
			int position = 0;
			while (enumerator.MoveNext())
			{
				object obj2 = enumerator.Current;
				position = ((IBuiltInEvidence)obj2).OutputToBuffer(array, position, verbose);
			}
			return array;
		}

		// Token: 0x04001F04 RID: 7940
		private object m_policyLevels;
	}
}

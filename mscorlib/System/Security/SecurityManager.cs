using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Util;
using System.Threading;

namespace System.Security
{
	// Token: 0x02000693 RID: 1683
	[ComVisible(true)]
	public static class SecurityManager
	{
		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06003CE2 RID: 15586 RVA: 0x000D04BF File Offset: 0x000CF4BF
		internal static PolicyManager PolicyManager
		{
			get
			{
				return SecurityManager.polmgr;
			}
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x000D04C8 File Offset: 0x000CF4C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool IsGranted(IPermission perm)
		{
			if (perm == null)
			{
				return true;
			}
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			PermissionSet permissionSet;
			PermissionSet permissionSet2;
			SecurityManager._GetGrantedPermissions(out permissionSet, out permissionSet2, ref stackCrawlMark);
			return permissionSet.Contains(perm) && (permissionSet2 == null || !permissionSet2.Contains(perm));
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x000D0504 File Offset: 0x000CF504
		private static bool CheckExecution()
		{
			if (SecurityManager.checkExecution == -1)
			{
				SecurityManager.checkExecution = (((SecurityManager.GetGlobalFlags() & 256) != 0) ? 0 : 1);
			}
			if (SecurityManager.checkExecution == 1)
			{
				if (SecurityManager.securityPermissionType == null)
				{
					SecurityManager.securityPermissionType = typeof(SecurityPermission);
					SecurityManager.executionSecurityPermission = new SecurityPermission(SecurityPermissionFlag.Execution);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x000D055C File Offset: 0x000CF55C
		[StrongNameIdentityPermission(SecurityAction.LinkDemand, Name = "System.Windows.Forms", PublicKey = "0x00000000000000000400000000000000")]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GetZoneAndOrigin(out ArrayList zone, out ArrayList origin)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			if (SecurityManager._IsSecurityOn())
			{
				CodeAccessSecurityEngine.GetZoneAndOrigin(ref stackCrawlMark, out zone, out origin);
				return;
			}
			zone = null;
			origin = null;
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x000D0584 File Offset: 0x000CF584
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
		public static PolicyLevel LoadPolicyLevelFromFile(string path, PolicyLevelType type)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (!File.InternalExists(path))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_PolicyFileDoesNotExist"));
			}
			string fullPath = Path.GetFullPath(path);
			FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.None);
			fileIOPermission.AddPathList(FileIOPermissionAccess.Read, fullPath);
			fileIOPermission.AddPathList(FileIOPermissionAccess.Write, fullPath);
			fileIOPermission.Demand();
			PolicyLevel result;
			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				using (StreamReader streamReader = new StreamReader(fileStream))
				{
					result = SecurityManager.LoadPolicyLevelFromStringHelper(streamReader.ReadToEnd(), path, type);
				}
			}
			return result;
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x000D0630 File Offset: 0x000CF630
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
		public static PolicyLevel LoadPolicyLevelFromString(string str, PolicyLevelType type)
		{
			return SecurityManager.LoadPolicyLevelFromStringHelper(str, null, type);
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x000D063C File Offset: 0x000CF63C
		private static PolicyLevel LoadPolicyLevelFromStringHelper(string str, string path, PolicyLevelType type)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			PolicyLevel policyLevel = new PolicyLevel(type, path);
			Parser parser = new Parser(str);
			SecurityElement topElement = parser.GetTopElement();
			if (topElement == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_BadXml"), new object[]
				{
					"configuration"
				}));
			}
			SecurityElement securityElement = topElement.SearchForChildByTag("mscorlib");
			if (securityElement == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_BadXml"), new object[]
				{
					"mscorlib"
				}));
			}
			SecurityElement securityElement2 = securityElement.SearchForChildByTag("security");
			if (securityElement2 == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_BadXml"), new object[]
				{
					"security"
				}));
			}
			SecurityElement securityElement3 = securityElement2.SearchForChildByTag("policy");
			if (securityElement3 == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_BadXml"), new object[]
				{
					"policy"
				}));
			}
			SecurityElement securityElement4 = securityElement3.SearchForChildByTag("PolicyLevel");
			if (securityElement4 != null)
			{
				policyLevel.FromXml(securityElement4);
				return policyLevel;
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Policy_BadXml"), new object[]
			{
				"PolicyLevel"
			}));
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x000D07A0 File Offset: 0x000CF7A0
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
		public static void SavePolicyLevel(PolicyLevel level)
		{
			PolicyManager.EncodeLevel(level);
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x000D07A8 File Offset: 0x000CF7A8
		private static PermissionSet ResolvePolicy(Evidence evidence, PermissionSet reqdPset, PermissionSet optPset, PermissionSet denyPset, out PermissionSet denied, out int securitySpecialFlags, bool checkExecutionPermission)
		{
			CodeAccessPermission.AssertAllPossible();
			PermissionSet permissionSet = SecurityManager.ResolvePolicy(evidence, reqdPset, optPset, denyPset, out denied, checkExecutionPermission);
			securitySpecialFlags = SecurityManager.GetSpecialFlags(permissionSet, denied);
			return permissionSet;
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x000D07D5 File Offset: 0x000CF7D5
		public static PermissionSet ResolvePolicy(Evidence evidence, PermissionSet reqdPset, PermissionSet optPset, PermissionSet denyPset, out PermissionSet denied)
		{
			return SecurityManager.ResolvePolicy(evidence, reqdPset, optPset, denyPset, out denied, true);
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x000D07E4 File Offset: 0x000CF7E4
		private static PermissionSet ResolvePolicy(Evidence evidence, PermissionSet reqdPset, PermissionSet optPset, PermissionSet denyPset, out PermissionSet denied, bool checkExecutionPermission)
		{
			Exception exception = null;
			PermissionSet permissionSet;
			if (reqdPset == null)
			{
				permissionSet = optPset;
			}
			else
			{
				permissionSet = ((optPset == null) ? null : reqdPset.Union(optPset));
			}
			if (permissionSet != null && !permissionSet.IsUnrestricted() && SecurityManager.CheckExecution())
			{
				permissionSet.AddPermission(SecurityManager.executionSecurityPermission);
			}
			if (evidence == null)
			{
				evidence = new Evidence();
			}
			else
			{
				evidence = evidence.ShallowCopy();
			}
			evidence.AddHost(new PermissionRequestEvidence(reqdPset, optPset, denyPset));
			PermissionSet permissionSet2 = SecurityManager.polmgr.Resolve(evidence);
			if (permissionSet != null)
			{
				permissionSet2.InplaceIntersect(permissionSet);
			}
			if (checkExecutionPermission && SecurityManager.CheckExecution() && (!permissionSet2.Contains(SecurityManager.executionSecurityPermission) || (denyPset != null && denyPset.Contains(SecurityManager.executionSecurityPermission))))
			{
				throw new PolicyException(Environment.GetResourceString("Policy_NoExecutionPermission"), -2146233320, exception);
			}
			if (reqdPset != null && !reqdPset.IsSubsetOf(permissionSet2))
			{
				throw new PolicyException(Environment.GetResourceString("Policy_NoRequiredPermission"), -2146233321, exception);
			}
			if (denyPset != null)
			{
				denied = denyPset.Copy();
				permissionSet2.MergeDeniedSet(denied);
				if (denied.IsEmpty())
				{
					denied = null;
				}
			}
			else
			{
				denied = null;
			}
			permissionSet2.IgnoreTypeLoadFailures = true;
			return permissionSet2;
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x000D08F3 File Offset: 0x000CF8F3
		public static PermissionSet ResolvePolicy(Evidence evidence)
		{
			if (evidence == null)
			{
				evidence = new Evidence();
			}
			else
			{
				evidence = evidence.ShallowCopy();
			}
			evidence.AddHost(new PermissionRequestEvidence(null, null, null));
			return SecurityManager.polmgr.Resolve(evidence);
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x000D0924 File Offset: 0x000CF924
		public static PermissionSet ResolvePolicy(Evidence[] evidences)
		{
			if (evidences == null || evidences.Length == 0)
			{
				Evidence[] array = new Evidence[1];
				evidences = array;
			}
			PermissionSet permissionSet = SecurityManager.ResolvePolicy(evidences[0]);
			if (permissionSet == null)
			{
				return null;
			}
			for (int i = 1; i < evidences.Length; i++)
			{
				permissionSet = permissionSet.Intersect(SecurityManager.ResolvePolicy(evidences[i]));
				if (permissionSet == null || permissionSet.IsEmpty())
				{
					return permissionSet;
				}
			}
			return permissionSet;
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x000D097C File Offset: 0x000CF97C
		public static PermissionSet ResolveSystemPolicy(Evidence evidence)
		{
			if (PolicyManager.IsGacAssembly(evidence))
			{
				return new PermissionSet(PermissionState.Unrestricted);
			}
			return SecurityManager.polmgr.CodeGroupResolve(evidence, true);
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x000D0999 File Offset: 0x000CF999
		public static IEnumerator ResolvePolicyGroups(Evidence evidence)
		{
			return SecurityManager.polmgr.ResolveCodeGroups(evidence);
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x000D09A6 File Offset: 0x000CF9A6
		public static IEnumerator PolicyHierarchy()
		{
			return SecurityManager.polmgr.PolicyHierarchy();
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x000D09B2 File Offset: 0x000CF9B2
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPolicy)]
		public static void SavePolicy()
		{
			SecurityManager.polmgr.Save();
			SecurityManager.SaveGlobalFlags();
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06003CF3 RID: 15603 RVA: 0x000D09C3 File Offset: 0x000CF9C3
		// (set) Token: 0x06003CF4 RID: 15604 RVA: 0x000D09DA File Offset: 0x000CF9DA
		public static bool CheckExecutionRights
		{
			get
			{
				return (SecurityManager.GetGlobalFlags() & 256) != 256;
			}
			set
			{
				if (value)
				{
					SecurityManager.checkExecution = 1;
					SecurityManager.SetGlobalFlags(256, 0);
					return;
				}
				new SecurityPermission(SecurityPermissionFlag.ControlPolicy).Demand();
				SecurityManager.checkExecution = 0;
				SecurityManager.SetGlobalFlags(256, 256);
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x06003CF5 RID: 15605 RVA: 0x000D0A12 File Offset: 0x000CFA12
		// (set) Token: 0x06003CF6 RID: 15606 RVA: 0x000D0A19 File Offset: 0x000CFA19
		[Obsolete("Because security can no longer be turned off permanently, setting the SecurityEnabled property no longer has any effect. Reading the property will still indicate whether security has been turned off temporarily.")]
		public static bool SecurityEnabled
		{
			get
			{
				return SecurityManager._IsSecurityOn();
			}
			set
			{
			}
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x000D0A1C File Offset: 0x000CFA1C
		internal static int GetSpecialFlags(PermissionSet grantSet, PermissionSet deniedSet)
		{
			if (grantSet != null && grantSet.IsUnrestricted() && (deniedSet == null || deniedSet.IsEmpty()))
			{
				return -1;
			}
			SecurityPermissionFlag securityPermissionFlag = SecurityPermissionFlag.NoFlags;
			ReflectionPermissionFlag reflectionPermissionFlag = ReflectionPermissionFlag.NoFlags;
			CodeAccessPermission[] array = new CodeAccessPermission[6];
			if (grantSet != null)
			{
				if (grantSet.IsUnrestricted())
				{
					securityPermissionFlag = SecurityPermissionFlag.AllFlags;
					reflectionPermissionFlag = (ReflectionPermissionFlag.TypeInformation | ReflectionPermissionFlag.MemberAccess | ReflectionPermissionFlag.ReflectionEmit | ReflectionPermissionFlag.RestrictedMemberAccess);
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = SecurityManager.s_UnrestrictedSpecialPermissionMap[i];
					}
				}
				else
				{
					SecurityPermission securityPermission = grantSet.GetPermission(6) as SecurityPermission;
					if (securityPermission != null)
					{
						securityPermissionFlag = securityPermission.Flags;
					}
					ReflectionPermission reflectionPermission = grantSet.GetPermission(4) as ReflectionPermission;
					if (reflectionPermission != null)
					{
						reflectionPermissionFlag = reflectionPermission.Flags;
					}
					for (int j = 0; j < array.Length; j++)
					{
						array[j] = (grantSet.GetPermission(SecurityManager.s_BuiltInPermissionIndexMap[j][0]) as CodeAccessPermission);
					}
				}
			}
			if (deniedSet != null)
			{
				if (deniedSet.IsUnrestricted())
				{
					securityPermissionFlag = SecurityPermissionFlag.NoFlags;
					reflectionPermissionFlag = ReflectionPermissionFlag.NoFlags;
					for (int k = 0; k < SecurityManager.s_BuiltInPermissionIndexMap.Length; k++)
					{
						array[k] = null;
					}
				}
				else
				{
					SecurityPermission securityPermission = deniedSet.GetPermission(6) as SecurityPermission;
					if (securityPermission != null)
					{
						securityPermissionFlag &= ~securityPermission.Flags;
					}
					ReflectionPermission reflectionPermission = deniedSet.GetPermission(4) as ReflectionPermission;
					if (reflectionPermission != null)
					{
						reflectionPermissionFlag &= ~reflectionPermission.Flags;
					}
					for (int l = 0; l < SecurityManager.s_BuiltInPermissionIndexMap.Length; l++)
					{
						CodeAccessPermission codeAccessPermission = deniedSet.GetPermission(SecurityManager.s_BuiltInPermissionIndexMap[l][0]) as CodeAccessPermission;
						if (codeAccessPermission != null && !codeAccessPermission.IsSubsetOf(null))
						{
							array[l] = null;
						}
					}
				}
			}
			int num = SecurityManager.MapToSpecialFlags(securityPermissionFlag, reflectionPermissionFlag);
			if (num != -1)
			{
				for (int m = 0; m < array.Length; m++)
				{
					if (array[m] != null && ((IUnrestrictedPermission)array[m]).IsUnrestricted())
					{
						num |= 1 << SecurityManager.s_BuiltInPermissionIndexMap[m][1];
					}
				}
			}
			return num;
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x000D0BDC File Offset: 0x000CFBDC
		private static int MapToSpecialFlags(SecurityPermissionFlag securityPermissionFlags, ReflectionPermissionFlag reflectionPermissionFlags)
		{
			int num = 0;
			if ((securityPermissionFlags & SecurityPermissionFlag.UnmanagedCode) == SecurityPermissionFlag.UnmanagedCode)
			{
				num |= 1;
			}
			if ((securityPermissionFlags & SecurityPermissionFlag.SkipVerification) == SecurityPermissionFlag.SkipVerification)
			{
				num |= 2;
			}
			if ((securityPermissionFlags & SecurityPermissionFlag.Assertion) == SecurityPermissionFlag.Assertion)
			{
				num |= 8;
			}
			if ((securityPermissionFlags & SecurityPermissionFlag.SerializationFormatter) == SecurityPermissionFlag.SerializationFormatter)
			{
				num |= 32;
			}
			if ((securityPermissionFlags & SecurityPermissionFlag.BindingRedirects) == SecurityPermissionFlag.BindingRedirects)
			{
				num |= 256;
			}
			if ((securityPermissionFlags & SecurityPermissionFlag.ControlEvidence) == SecurityPermissionFlag.ControlEvidence)
			{
				num |= 65536;
			}
			if ((securityPermissionFlags & SecurityPermissionFlag.ControlPrincipal) == SecurityPermissionFlag.ControlPrincipal)
			{
				num |= 131072;
			}
			if ((reflectionPermissionFlags & ReflectionPermissionFlag.RestrictedMemberAccess) == ReflectionPermissionFlag.RestrictedMemberAccess)
			{
				num |= 64;
			}
			if ((reflectionPermissionFlags & ReflectionPermissionFlag.MemberAccess) == ReflectionPermissionFlag.MemberAccess)
			{
				num |= 16;
			}
			return num;
		}

		// Token: 0x06003CF9 RID: 15609
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _IsSameType(string strLeft, string strRight);

		// Token: 0x06003CFA RID: 15610
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _SetThreadSecurity(bool bThreadSecurity);

		// Token: 0x06003CFB RID: 15611
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _IsSecurityOn();

		// Token: 0x06003CFC RID: 15612
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetGlobalFlags();

		// Token: 0x06003CFD RID: 15613
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetGlobalFlags(int mask, int flags);

		// Token: 0x06003CFE RID: 15614
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SaveGlobalFlags();

		// Token: 0x06003CFF RID: 15615
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _GetGrantedPermissions(out PermissionSet granted, out PermissionSet denied, ref StackCrawlMark stackmark);

		// Token: 0x04001F4C RID: 8012
		private const int CheckExecutionRightsDisabledFlag = 256;

		// Token: 0x04001F4D RID: 8013
		private static Type securityPermissionType = null;

		// Token: 0x04001F4E RID: 8014
		private static SecurityPermission executionSecurityPermission = null;

		// Token: 0x04001F4F RID: 8015
		private static int checkExecution = -1;

		// Token: 0x04001F50 RID: 8016
		private static PolicyManager polmgr = new PolicyManager();

		// Token: 0x04001F51 RID: 8017
		private static int[][] s_BuiltInPermissionIndexMap = new int[][]
		{
			new int[]
			{
				0,
				10
			},
			new int[]
			{
				1,
				11
			},
			new int[]
			{
				2,
				12
			},
			new int[]
			{
				4,
				13
			},
			new int[]
			{
				6,
				14
			},
			new int[]
			{
				7,
				9
			}
		};

		// Token: 0x04001F52 RID: 8018
		private static CodeAccessPermission[] s_UnrestrictedSpecialPermissionMap = new CodeAccessPermission[]
		{
			new EnvironmentPermission(PermissionState.Unrestricted),
			new FileDialogPermission(PermissionState.Unrestricted),
			new FileIOPermission(PermissionState.Unrestricted),
			new ReflectionPermission(PermissionState.Unrestricted),
			new SecurityPermission(PermissionState.Unrestricted),
			new UIPermission(PermissionState.Unrestricted)
		};
	}
}

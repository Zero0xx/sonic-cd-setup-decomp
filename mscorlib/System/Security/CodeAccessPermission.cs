using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Util;
using System.Threading;

namespace System.Security
{
	// Token: 0x02000626 RID: 1574
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, ControlEvidence = true, ControlPolicy = true)]
	[Serializable]
	public abstract class CodeAccessPermission : IPermission, ISecurityEncodable, IStackWalk
	{
		// Token: 0x060038A8 RID: 14504 RVA: 0x000BEF1C File Offset: 0x000BDF1C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void RevertAssert()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityRuntime.RevertAssert(ref stackCrawlMark);
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x000BEF34 File Offset: 0x000BDF34
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void RevertDeny()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityRuntime.RevertDeny(ref stackCrawlMark);
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x000BEF4C File Offset: 0x000BDF4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void RevertPermitOnly()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityRuntime.RevertPermitOnly(ref stackCrawlMark);
		}

		// Token: 0x060038AB RID: 14507 RVA: 0x000BEF64 File Offset: 0x000BDF64
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void RevertAll()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityRuntime.RevertAll(ref stackCrawlMark);
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x000BEF7C File Offset: 0x000BDF7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Demand()
		{
			if (!this.CheckDemand(null))
			{
				StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCallersCaller;
				CodeAccessSecurityEngine.Check(this, ref stackCrawlMark);
			}
		}

		// Token: 0x060038AD RID: 14509 RVA: 0x000BEF9C File Offset: 0x000BDF9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void DemandInternal(PermissionType permissionType)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCallersCaller;
			CodeAccessSecurityEngine.SpecialDemand(permissionType, ref stackCrawlMark);
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x000BEFB4 File Offset: 0x000BDFB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Assert()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			CodeAccessSecurityEngine.Assert(this, ref stackCrawlMark);
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x000BEFCC File Offset: 0x000BDFCC
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static void AssertAllPossible()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			SecurityRuntime.AssertAllPossible(ref stackCrawlMark);
		}

		// Token: 0x060038B0 RID: 14512 RVA: 0x000BEFE4 File Offset: 0x000BDFE4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Deny()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			CodeAccessSecurityEngine.Deny(this, ref stackCrawlMark);
		}

		// Token: 0x060038B1 RID: 14513 RVA: 0x000BEFFC File Offset: 0x000BDFFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void PermitOnly()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			CodeAccessSecurityEngine.PermitOnly(this, ref stackCrawlMark);
		}

		// Token: 0x060038B2 RID: 14514 RVA: 0x000BF013 File Offset: 0x000BE013
		public virtual IPermission Union(IPermission other)
		{
			if (other == null)
			{
				return this.Copy();
			}
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SecurityPermissionUnion"));
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x000BF030 File Offset: 0x000BE030
		internal static SecurityElement CreatePermissionElement(IPermission perm, string permname)
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			XMLUtil.AddClassAttribute(securityElement, perm.GetType(), permname);
			securityElement.AddAttribute("version", "1");
			return securityElement;
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x000BF068 File Offset: 0x000BE068
		internal static void ValidateElement(SecurityElement elem, IPermission perm)
		{
			if (elem == null)
			{
				throw new ArgumentNullException("elem");
			}
			if (!XMLUtil.IsPermissionElement(perm, elem))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotAPermissionElement"));
			}
			string text = elem.Attribute("version");
			if (text != null && !text.Equals("1"))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidXMLBadVersion"));
			}
		}

		// Token: 0x060038B5 RID: 14517
		public abstract SecurityElement ToXml();

		// Token: 0x060038B6 RID: 14518
		public abstract void FromXml(SecurityElement elem);

		// Token: 0x060038B7 RID: 14519 RVA: 0x000BF0C8 File Offset: 0x000BE0C8
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x000BF0D5 File Offset: 0x000BE0D5
		internal bool VerifyType(IPermission perm)
		{
			return perm != null && perm.GetType() == base.GetType();
		}

		// Token: 0x060038B9 RID: 14521
		public abstract IPermission Copy();

		// Token: 0x060038BA RID: 14522
		public abstract IPermission Intersect(IPermission target);

		// Token: 0x060038BB RID: 14523
		public abstract bool IsSubsetOf(IPermission target);

		// Token: 0x060038BC RID: 14524 RVA: 0x000BF0EC File Offset: 0x000BE0EC
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			IPermission permission = obj as IPermission;
			if (obj != null && permission == null)
			{
				return false;
			}
			try
			{
				if (!this.IsSubsetOf(permission))
				{
					return false;
				}
				if (permission != null && !permission.IsSubsetOf(this))
				{
					return false;
				}
			}
			catch (ArgumentException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x000BF140 File Offset: 0x000BE140
		[ComVisible(false)]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x000BF148 File Offset: 0x000BE148
		internal bool CheckDemand(CodeAccessPermission grant)
		{
			return this.IsSubsetOf(grant);
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x000BF151 File Offset: 0x000BE151
		internal bool CheckPermitOnly(CodeAccessPermission permitted)
		{
			return this.IsSubsetOf(permitted);
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x000BF15C File Offset: 0x000BE15C
		internal bool CheckDeny(CodeAccessPermission denied)
		{
			IPermission permission = this.Intersect(denied);
			return permission == null || permission.IsSubsetOf(null);
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x000BF17D File Offset: 0x000BE17D
		internal bool CheckAssert(CodeAccessPermission asserted)
		{
			return this.IsSubsetOf(asserted);
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x000BF186 File Offset: 0x000BE186
		internal bool CanUnrestrictedOverride()
		{
			return CodeAccessPermission.CanUnrestrictedOverride(this);
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x000BF18E File Offset: 0x000BE18E
		internal static bool CanUnrestrictedOverride(IPermission ip)
		{
			return CodeAccessSecurityEngine.DoesFullTrustMeanFullTrust() || ip is IUnrestrictedPermission;
		}
	}
}

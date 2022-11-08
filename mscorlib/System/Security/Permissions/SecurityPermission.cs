using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Permissions
{
	// Token: 0x02000651 RID: 1617
	[ComVisible(true)]
	[Serializable]
	public sealed class SecurityPermission : CodeAccessPermission, IUnrestrictedPermission, IBuiltInPermission
	{
		// Token: 0x06003A3E RID: 14910 RVA: 0x000C3C65 File Offset: 0x000C2C65
		public SecurityPermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.SetUnrestricted(true);
				return;
			}
			if (state == PermissionState.None)
			{
				this.SetUnrestricted(false);
				this.Reset();
				return;
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_InvalidPermissionState"));
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x000C3C99 File Offset: 0x000C2C99
		public SecurityPermission(SecurityPermissionFlag flag)
		{
			this.VerifyAccess(flag);
			this.SetUnrestricted(false);
			this.m_flags = flag;
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x000C3CB6 File Offset: 0x000C2CB6
		private void SetUnrestricted(bool unrestricted)
		{
			if (unrestricted)
			{
				this.m_flags = SecurityPermissionFlag.AllFlags;
			}
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x000C3CC6 File Offset: 0x000C2CC6
		private void Reset()
		{
			this.m_flags = SecurityPermissionFlag.NoFlags;
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06003A43 RID: 14915 RVA: 0x000C3CDF File Offset: 0x000C2CDF
		// (set) Token: 0x06003A42 RID: 14914 RVA: 0x000C3CCF File Offset: 0x000C2CCF
		public SecurityPermissionFlag Flags
		{
			get
			{
				return this.m_flags;
			}
			set
			{
				this.VerifyAccess(value);
				this.m_flags = value;
			}
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x000C3CE8 File Offset: 0x000C2CE8
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.m_flags == SecurityPermissionFlag.NoFlags;
			}
			SecurityPermission securityPermission = target as SecurityPermission;
			if (securityPermission != null)
			{
				return (this.m_flags & ~securityPermission.m_flags) == SecurityPermissionFlag.NoFlags;
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[]
			{
				base.GetType().FullName
			}));
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x000C3D50 File Offset: 0x000C2D50
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			if (!base.VerifyType(target))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[]
				{
					base.GetType().FullName
				}));
			}
			SecurityPermission securityPermission = (SecurityPermission)target;
			if (securityPermission.IsUnrestricted() || this.IsUnrestricted())
			{
				return new SecurityPermission(PermissionState.Unrestricted);
			}
			SecurityPermissionFlag flag = this.m_flags | securityPermission.m_flags;
			return new SecurityPermission(flag);
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x000C3DD4 File Offset: 0x000C2DD4
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			if (!base.VerifyType(target))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[]
				{
					base.GetType().FullName
				}));
			}
			SecurityPermission securityPermission = (SecurityPermission)target;
			SecurityPermissionFlag securityPermissionFlag;
			if (securityPermission.IsUnrestricted())
			{
				if (this.IsUnrestricted())
				{
					return new SecurityPermission(PermissionState.Unrestricted);
				}
				securityPermissionFlag = this.m_flags;
			}
			else if (this.IsUnrestricted())
			{
				securityPermissionFlag = securityPermission.m_flags;
			}
			else
			{
				securityPermissionFlag = (this.m_flags & securityPermission.m_flags);
			}
			if (securityPermissionFlag == SecurityPermissionFlag.NoFlags)
			{
				return null;
			}
			return new SecurityPermission(securityPermissionFlag);
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x000C3E72 File Offset: 0x000C2E72
		public override IPermission Copy()
		{
			if (this.IsUnrestricted())
			{
				return new SecurityPermission(PermissionState.Unrestricted);
			}
			return new SecurityPermission(this.m_flags);
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x000C3E8E File Offset: 0x000C2E8E
		public bool IsUnrestricted()
		{
			return this.m_flags == SecurityPermissionFlag.AllFlags;
		}

		// Token: 0x06003A49 RID: 14921 RVA: 0x000C3EA0 File Offset: 0x000C2EA0
		private void VerifyAccess(SecurityPermissionFlag type)
		{
			if ((type & ~(SecurityPermissionFlag.Assertion | SecurityPermissionFlag.UnmanagedCode | SecurityPermissionFlag.SkipVerification | SecurityPermissionFlag.Execution | SecurityPermissionFlag.ControlThread | SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy | SecurityPermissionFlag.SerializationFormatter | SecurityPermissionFlag.ControlDomainPolicy | SecurityPermissionFlag.ControlPrincipal | SecurityPermissionFlag.ControlAppDomain | SecurityPermissionFlag.RemotingConfiguration | SecurityPermissionFlag.Infrastructure | SecurityPermissionFlag.BindingRedirects)) != SecurityPermissionFlag.NoFlags)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumIllegalVal"), new object[]
				{
					(int)type
				}));
			}
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x000C3EE4 File Offset: 0x000C2EE4
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = CodeAccessPermission.CreatePermissionElement(this, "System.Security.Permissions.SecurityPermission");
			if (!this.IsUnrestricted())
			{
				securityElement.AddAttribute("Flags", XMLUtil.BitFieldEnumToString(typeof(SecurityPermissionFlag), this.m_flags));
			}
			else
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		// Token: 0x06003A4B RID: 14923 RVA: 0x000C3F40 File Offset: 0x000C2F40
		public override void FromXml(SecurityElement esd)
		{
			CodeAccessPermission.ValidateElement(esd, this);
			if (XMLUtil.IsUnrestricted(esd))
			{
				this.m_flags = SecurityPermissionFlag.AllFlags;
				return;
			}
			this.Reset();
			this.SetUnrestricted(false);
			string text = esd.Attribute("Flags");
			if (text != null)
			{
				this.m_flags = (SecurityPermissionFlag)Enum.Parse(typeof(SecurityPermissionFlag), text);
			}
		}

		// Token: 0x06003A4C RID: 14924 RVA: 0x000C3F9F File Offset: 0x000C2F9F
		int IBuiltInPermission.GetTokenIndex()
		{
			return SecurityPermission.GetTokenIndex();
		}

		// Token: 0x06003A4D RID: 14925 RVA: 0x000C3FA6 File Offset: 0x000C2FA6
		internal static int GetTokenIndex()
		{
			return 6;
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x000C3FA9 File Offset: 0x000C2FA9
		[SecurityPermission(SecurityAction.LinkDemand, SkipVerification = true)]
		internal static void MethodWithSkipVerificationLinkDemand()
		{
		}

		// Token: 0x04001E44 RID: 7748
		private const string _strHeaderAssertion = "Assertion";

		// Token: 0x04001E45 RID: 7749
		private const string _strHeaderUnmanagedCode = "UnmanagedCode";

		// Token: 0x04001E46 RID: 7750
		private const string _strHeaderExecution = "Execution";

		// Token: 0x04001E47 RID: 7751
		private const string _strHeaderSkipVerification = "SkipVerification";

		// Token: 0x04001E48 RID: 7752
		private const string _strHeaderControlThread = "ControlThread";

		// Token: 0x04001E49 RID: 7753
		private const string _strHeaderControlEvidence = "ControlEvidence";

		// Token: 0x04001E4A RID: 7754
		private const string _strHeaderControlPolicy = "ControlPolicy";

		// Token: 0x04001E4B RID: 7755
		private const string _strHeaderSerializationFormatter = "SerializationFormatter";

		// Token: 0x04001E4C RID: 7756
		private const string _strHeaderControlDomainPolicy = "ControlDomainPolicy";

		// Token: 0x04001E4D RID: 7757
		private const string _strHeaderControlPrincipal = "ControlPrincipal";

		// Token: 0x04001E4E RID: 7758
		private const string _strHeaderControlAppDomain = "ControlAppDomain";

		// Token: 0x04001E4F RID: 7759
		private SecurityPermissionFlag m_flags;
	}
}

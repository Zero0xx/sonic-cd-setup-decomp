using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200065C RID: 1628
	[ComVisible(true)]
	[Serializable]
	public sealed class GacIdentityPermission : CodeAccessPermission, IBuiltInPermission
	{
		// Token: 0x06003AB4 RID: 15028 RVA: 0x000C6417 File Offset: 0x000C5417
		public GacIdentityPermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				if (!CodeAccessSecurityEngine.DoesFullTrustMeanFullTrust())
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_UnrestrictedIdentityPermission"));
				}
				return;
			}
			else
			{
				if (state == PermissionState.None)
				{
					return;
				}
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidPermissionState"));
			}
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x000C644E File Offset: 0x000C544E
		public GacIdentityPermission()
		{
		}

		// Token: 0x06003AB6 RID: 15030 RVA: 0x000C6456 File Offset: 0x000C5456
		public override IPermission Copy()
		{
			return new GacIdentityPermission();
		}

		// Token: 0x06003AB7 RID: 15031 RVA: 0x000C6460 File Offset: 0x000C5460
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return false;
			}
			if (!(target is GacIdentityPermission))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[]
				{
					base.GetType().FullName
				}));
			}
			return true;
		}

		// Token: 0x06003AB8 RID: 15032 RVA: 0x000C64AC File Offset: 0x000C54AC
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			if (!(target is GacIdentityPermission))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[]
				{
					base.GetType().FullName
				}));
			}
			return this.Copy();
		}

		// Token: 0x06003AB9 RID: 15033 RVA: 0x000C64FC File Offset: 0x000C54FC
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			if (!(target is GacIdentityPermission))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_WrongType"), new object[]
				{
					base.GetType().FullName
				}));
			}
			return this.Copy();
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x000C6554 File Offset: 0x000C5554
		public override SecurityElement ToXml()
		{
			return CodeAccessPermission.CreatePermissionElement(this, "System.Security.Permissions.GacIdentityPermission");
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x000C656E File Offset: 0x000C556E
		public override void FromXml(SecurityElement securityElement)
		{
			CodeAccessPermission.ValidateElement(securityElement, this);
		}

		// Token: 0x06003ABC RID: 15036 RVA: 0x000C6577 File Offset: 0x000C5577
		int IBuiltInPermission.GetTokenIndex()
		{
			return GacIdentityPermission.GetTokenIndex();
		}

		// Token: 0x06003ABD RID: 15037 RVA: 0x000C657E File Offset: 0x000C557E
		internal static int GetTokenIndex()
		{
			return 15;
		}
	}
}

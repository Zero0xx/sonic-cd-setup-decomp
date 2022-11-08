using System;
using System.Security;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x02000739 RID: 1849
	[Serializable]
	public sealed class AspNetHostingPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x0600384F RID: 14415 RVA: 0x000ED82C File Offset: 0x000EC82C
		internal static void VerifyAspNetHostingPermissionLevel(AspNetHostingPermissionLevel level, string arg)
		{
			if (level <= AspNetHostingPermissionLevel.Low)
			{
				if (level == AspNetHostingPermissionLevel.None || level == AspNetHostingPermissionLevel.Minimal || level == AspNetHostingPermissionLevel.Low)
				{
					return;
				}
			}
			else if (level == AspNetHostingPermissionLevel.Medium || level == AspNetHostingPermissionLevel.High || level == AspNetHostingPermissionLevel.Unrestricted)
			{
				return;
			}
			throw new ArgumentException(arg);
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x000ED87C File Offset: 0x000EC87C
		public AspNetHostingPermission(PermissionState state)
		{
			switch (state)
			{
			case PermissionState.None:
				this._level = AspNetHostingPermissionLevel.None;
				return;
			case PermissionState.Unrestricted:
				this._level = AspNetHostingPermissionLevel.Unrestricted;
				return;
			default:
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					state.ToString(),
					"state"
				}));
			}
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x000ED8E3 File Offset: 0x000EC8E3
		public AspNetHostingPermission(AspNetHostingPermissionLevel level)
		{
			AspNetHostingPermission.VerifyAspNetHostingPermissionLevel(level, "level");
			this._level = level;
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06003852 RID: 14418 RVA: 0x000ED8FD File Offset: 0x000EC8FD
		// (set) Token: 0x06003853 RID: 14419 RVA: 0x000ED905 File Offset: 0x000EC905
		public AspNetHostingPermissionLevel Level
		{
			get
			{
				return this._level;
			}
			set
			{
				AspNetHostingPermission.VerifyAspNetHostingPermissionLevel(value, "Level");
				this._level = value;
			}
		}

		// Token: 0x06003854 RID: 14420 RVA: 0x000ED919 File Offset: 0x000EC919
		public bool IsUnrestricted()
		{
			return this._level == AspNetHostingPermissionLevel.Unrestricted;
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x000ED928 File Offset: 0x000EC928
		public override IPermission Copy()
		{
			return new AspNetHostingPermission(this._level);
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x000ED938 File Offset: 0x000EC938
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			if (target.GetType() != typeof(AspNetHostingPermission))
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					(target == null) ? "null" : target.ToString(),
					"target"
				}));
			}
			AspNetHostingPermission aspNetHostingPermission = (AspNetHostingPermission)target;
			if (this.Level >= aspNetHostingPermission.Level)
			{
				return new AspNetHostingPermission(this.Level);
			}
			return new AspNetHostingPermission(aspNetHostingPermission.Level);
		}

		// Token: 0x06003857 RID: 14423 RVA: 0x000ED9C0 File Offset: 0x000EC9C0
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			if (target.GetType() != typeof(AspNetHostingPermission))
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					(target == null) ? "null" : target.ToString(),
					"target"
				}));
			}
			AspNetHostingPermission aspNetHostingPermission = (AspNetHostingPermission)target;
			if (this.Level <= aspNetHostingPermission.Level)
			{
				return new AspNetHostingPermission(this.Level);
			}
			return new AspNetHostingPermission(aspNetHostingPermission.Level);
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x000EDA44 File Offset: 0x000ECA44
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this._level == AspNetHostingPermissionLevel.None;
			}
			if (target.GetType() != typeof(AspNetHostingPermission))
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					(target == null) ? "null" : target.ToString(),
					"target"
				}));
			}
			AspNetHostingPermission aspNetHostingPermission = (AspNetHostingPermission)target;
			return this.Level <= aspNetHostingPermission.Level;
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x000EDABC File Offset: 0x000ECABC
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException(SR.GetString("AspNetHostingPermissionBadXml", new object[]
				{
					"securityElement"
				}));
			}
			if (!securityElement.Tag.Equals("IPermission"))
			{
				throw new ArgumentException(SR.GetString("AspNetHostingPermissionBadXml", new object[]
				{
					"securityElement"
				}));
			}
			string text = securityElement.Attribute("class");
			if (text == null)
			{
				throw new ArgumentException(SR.GetString("AspNetHostingPermissionBadXml", new object[]
				{
					"securityElement"
				}));
			}
			if (text.IndexOf(base.GetType().FullName, StringComparison.Ordinal) < 0)
			{
				throw new ArgumentException(SR.GetString("AspNetHostingPermissionBadXml", new object[]
				{
					"securityElement"
				}));
			}
			string strA = securityElement.Attribute("version");
			if (string.Compare(strA, "1", StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(SR.GetString("AspNetHostingPermissionBadXml", new object[]
				{
					"version"
				}));
			}
			string text2 = securityElement.Attribute("Level");
			if (text2 == null)
			{
				this._level = AspNetHostingPermissionLevel.None;
				return;
			}
			this._level = (AspNetHostingPermissionLevel)Enum.Parse(typeof(AspNetHostingPermissionLevel), text2);
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x000EDBFC File Offset: 0x000ECBFC
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().FullName + ", " + base.GetType().Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			securityElement.AddAttribute("Level", Enum.GetName(typeof(AspNetHostingPermissionLevel), this._level));
			if (this.IsUnrestricted())
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		// Token: 0x0400323B RID: 12859
		private AspNetHostingPermissionLevel _level;
	}
}

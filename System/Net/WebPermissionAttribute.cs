using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020004A6 RID: 1190
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class WebPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x0600246D RID: 9325 RVA: 0x0008F3EA File Offset: 0x0008E3EA
		public WebPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x0600246E RID: 9326 RVA: 0x0008F3F3 File Offset: 0x0008E3F3
		// (set) Token: 0x0600246F RID: 9327 RVA: 0x0008F400 File Offset: 0x0008E400
		public string Connect
		{
			get
			{
				return this.m_connect as string;
			}
			set
			{
				if (this.m_connect != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[]
					{
						"Connect",
						value
					}), "value");
				}
				this.m_connect = value;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06002470 RID: 9328 RVA: 0x0008F445 File Offset: 0x0008E445
		// (set) Token: 0x06002471 RID: 9329 RVA: 0x0008F454 File Offset: 0x0008E454
		public string Accept
		{
			get
			{
				return this.m_accept as string;
			}
			set
			{
				if (this.m_accept != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[]
					{
						"Accept",
						value
					}), "value");
				}
				this.m_accept = value;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06002472 RID: 9330 RVA: 0x0008F499 File Offset: 0x0008E499
		// (set) Token: 0x06002473 RID: 9331 RVA: 0x0008F4D8 File Offset: 0x0008E4D8
		public string ConnectPattern
		{
			get
			{
				if (this.m_connect is DelayedRegex)
				{
					return this.m_connect.ToString();
				}
				if (!(this.m_connect is bool) || !(bool)this.m_connect)
				{
					return null;
				}
				return ".*";
			}
			set
			{
				if (this.m_connect != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[]
					{
						"ConnectPatern",
						value
					}), "value");
				}
				if (value == ".*")
				{
					this.m_connect = true;
					return;
				}
				this.m_connect = new DelayedRegex(value);
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06002474 RID: 9332 RVA: 0x0008F53C File Offset: 0x0008E53C
		// (set) Token: 0x06002475 RID: 9333 RVA: 0x0008F578 File Offset: 0x0008E578
		public string AcceptPattern
		{
			get
			{
				if (this.m_accept is DelayedRegex)
				{
					return this.m_accept.ToString();
				}
				if (!(this.m_accept is bool) || !(bool)this.m_accept)
				{
					return null;
				}
				return ".*";
			}
			set
			{
				if (this.m_accept != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[]
					{
						"AcceptPattern",
						value
					}), "value");
				}
				if (value == ".*")
				{
					this.m_accept = true;
					return;
				}
				this.m_accept = new DelayedRegex(value);
			}
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x0008F5DC File Offset: 0x0008E5DC
		public override IPermission CreatePermission()
		{
			WebPermission webPermission;
			if (base.Unrestricted)
			{
				webPermission = new WebPermission(PermissionState.Unrestricted);
			}
			else
			{
				NetworkAccess networkAccess = (NetworkAccess)0;
				if (this.m_connect is bool)
				{
					if ((bool)this.m_connect)
					{
						networkAccess |= NetworkAccess.Connect;
					}
					this.m_connect = null;
				}
				if (this.m_accept is bool)
				{
					if ((bool)this.m_accept)
					{
						networkAccess |= NetworkAccess.Accept;
					}
					this.m_accept = null;
				}
				webPermission = new WebPermission(networkAccess);
				if (this.m_accept != null)
				{
					if (this.m_accept is DelayedRegex)
					{
						webPermission.AddAsPattern(NetworkAccess.Accept, (DelayedRegex)this.m_accept);
					}
					else
					{
						webPermission.AddPermission(NetworkAccess.Accept, (string)this.m_accept);
					}
				}
				if (this.m_connect != null)
				{
					if (this.m_connect is DelayedRegex)
					{
						webPermission.AddAsPattern(NetworkAccess.Connect, (DelayedRegex)this.m_connect);
					}
					else
					{
						webPermission.AddPermission(NetworkAccess.Connect, (string)this.m_connect);
					}
				}
			}
			return webPermission;
		}

		// Token: 0x040024B7 RID: 9399
		private object m_accept;

		// Token: 0x040024B8 RID: 9400
		private object m_connect;
	}
}

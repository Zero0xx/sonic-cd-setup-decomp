using System;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Net
{
	// Token: 0x0200043E RID: 1086
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class SocketPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002215 RID: 8725 RVA: 0x00086958 File Offset: 0x00085958
		public SocketPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06002216 RID: 8726 RVA: 0x00086961 File Offset: 0x00085961
		// (set) Token: 0x06002217 RID: 8727 RVA: 0x0008696C File Offset: 0x0008596C
		public string Access
		{
			get
			{
				return this.m_access;
			}
			set
			{
				if (this.m_access != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[]
					{
						"Access",
						value
					}), "value");
				}
				this.m_access = value;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06002218 RID: 8728 RVA: 0x000869B1 File Offset: 0x000859B1
		// (set) Token: 0x06002219 RID: 8729 RVA: 0x000869BC File Offset: 0x000859BC
		public string Host
		{
			get
			{
				return this.m_host;
			}
			set
			{
				if (this.m_host != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[]
					{
						"Host",
						value
					}), "value");
				}
				this.m_host = value;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x0600221A RID: 8730 RVA: 0x00086A01 File Offset: 0x00085A01
		// (set) Token: 0x0600221B RID: 8731 RVA: 0x00086A0C File Offset: 0x00085A0C
		public string Transport
		{
			get
			{
				return this.m_transport;
			}
			set
			{
				if (this.m_transport != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[]
					{
						"Transport",
						value
					}), "value");
				}
				this.m_transport = value;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x00086A51 File Offset: 0x00085A51
		// (set) Token: 0x0600221D RID: 8733 RVA: 0x00086A5C File Offset: 0x00085A5C
		public string Port
		{
			get
			{
				return this.m_port;
			}
			set
			{
				if (this.m_port != null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_multi", new object[]
					{
						"Port",
						value
					}), "value");
				}
				this.m_port = value;
			}
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x00086AA4 File Offset: 0x00085AA4
		public override IPermission CreatePermission()
		{
			SocketPermission socketPermission;
			if (base.Unrestricted)
			{
				socketPermission = new SocketPermission(PermissionState.Unrestricted);
			}
			else
			{
				socketPermission = new SocketPermission(PermissionState.None);
				if (this.m_access == null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_count", new object[]
					{
						"Access"
					}));
				}
				if (this.m_host == null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_count", new object[]
					{
						"Host"
					}));
				}
				if (this.m_transport == null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_count", new object[]
					{
						"Transport"
					}));
				}
				if (this.m_port == null)
				{
					throw new ArgumentException(SR.GetString("net_perm_attrib_count", new object[]
					{
						"Port"
					}));
				}
				this.ParseAddPermissions(socketPermission);
			}
			return socketPermission;
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x00086B7C File Offset: 0x00085B7C
		private void ParseAddPermissions(SocketPermission perm)
		{
			NetworkAccess access;
			if (string.Compare(this.m_access, "Connect", StringComparison.OrdinalIgnoreCase) == 0)
			{
				access = NetworkAccess.Connect;
			}
			else
			{
				if (string.Compare(this.m_access, "Accept", StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[]
					{
						"Access",
						this.m_access
					}));
				}
				access = NetworkAccess.Accept;
			}
			TransportType transport;
			try
			{
				transport = (TransportType)Enum.Parse(typeof(TransportType), this.m_transport, true);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[]
				{
					"Transport",
					this.m_transport
				}), ex);
			}
			catch
			{
				throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[]
				{
					"Transport",
					this.m_transport
				}), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			if (string.Compare(this.m_port, "All", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.m_port = "-1";
			}
			int num;
			try
			{
				num = int.Parse(this.m_port, NumberFormatInfo.InvariantInfo);
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[]
				{
					"Port",
					this.m_port
				}), ex2);
			}
			catch
			{
				throw new ArgumentException(SR.GetString("net_perm_invalid_val", new object[]
				{
					"Port",
					this.m_port
				}), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			if (!ValidationHelper.ValidateTcpPort(num) && num != -1)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_perm_invalid_val", new object[]
				{
					"Port",
					this.m_port
				}));
			}
			perm.AddPermission(access, transport, this.m_host, num);
		}

		// Token: 0x04002205 RID: 8709
		private const string strAccess = "Access";

		// Token: 0x04002206 RID: 8710
		private const string strConnect = "Connect";

		// Token: 0x04002207 RID: 8711
		private const string strAccept = "Accept";

		// Token: 0x04002208 RID: 8712
		private const string strHost = "Host";

		// Token: 0x04002209 RID: 8713
		private const string strTransport = "Transport";

		// Token: 0x0400220A RID: 8714
		private const string strPort = "Port";

		// Token: 0x0400220B RID: 8715
		private string m_access;

		// Token: 0x0400220C RID: 8716
		private string m_host;

		// Token: 0x0400220D RID: 8717
		private string m_port;

		// Token: 0x0400220E RID: 8718
		private string m_transport;
	}
}

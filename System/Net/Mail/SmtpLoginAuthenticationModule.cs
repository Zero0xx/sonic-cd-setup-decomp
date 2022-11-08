using System;
using System.Collections;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x020006CD RID: 1741
	internal class SmtpLoginAuthenticationModule : ISmtpAuthenticationModule
	{
		// Token: 0x060035CF RID: 13775 RVA: 0x000E5907 File Offset: 0x000E4907
		internal SmtpLoginAuthenticationModule()
		{
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x000E591C File Offset: 0x000E491C
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
		public Authorization Authenticate(string challenge, NetworkCredential credential, object sessionCookie, string spn, ChannelBinding channelBindingToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Authenticate", null);
			}
			Authorization result;
			try
			{
				lock (this.sessions)
				{
					NetworkCredential networkCredential = this.sessions[sessionCookie] as NetworkCredential;
					if (networkCredential == null)
					{
						if (credential == null || credential is SystemNetworkCredential)
						{
							result = null;
						}
						else
						{
							this.sessions[sessionCookie] = credential;
							string text = credential.UserName;
							string domain = credential.Domain;
							if (domain != null && domain.Length > 0)
							{
								text = domain + "\\" + text;
							}
							result = new Authorization(Convert.ToBase64String(Encoding.ASCII.GetBytes(text)), false);
						}
					}
					else
					{
						this.sessions.Remove(sessionCookie);
						result = new Authorization(Convert.ToBase64String(Encoding.ASCII.GetBytes(networkCredential.Password)), true);
					}
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "Authenticate", null);
				}
			}
			return result;
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x060035D1 RID: 13777 RVA: 0x000E5A2C File Offset: 0x000E4A2C
		public string AuthenticationType
		{
			get
			{
				return "login";
			}
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x000E5A33 File Offset: 0x000E4A33
		public void CloseContext(object sessionCookie)
		{
		}

		// Token: 0x04003106 RID: 12550
		private Hashtable sessions = new Hashtable();
	}
}

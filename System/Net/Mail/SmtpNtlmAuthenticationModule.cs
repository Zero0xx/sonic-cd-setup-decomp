using System;
using System.Collections;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x020006CF RID: 1743
	internal class SmtpNtlmAuthenticationModule : ISmtpAuthenticationModule
	{
		// Token: 0x060035D8 RID: 13784 RVA: 0x000E5C44 File Offset: 0x000E4C44
		internal SmtpNtlmAuthenticationModule()
		{
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x000E5C58 File Offset: 0x000E4C58
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
					NTAuthentication ntauthentication = this.sessions[sessionCookie] as NTAuthentication;
					if (ntauthentication == null)
					{
						if (credential == null)
						{
							return null;
						}
						ntauthentication = (this.sessions[sessionCookie] = new NTAuthentication(false, "Ntlm", credential, spn, ContextFlags.Connection, channelBindingToken));
					}
					string outgoingBlob = ntauthentication.GetOutgoingBlob(challenge);
					if (!ntauthentication.IsCompleted)
					{
						result = new Authorization(outgoingBlob, false);
					}
					else
					{
						this.sessions.Remove(sessionCookie);
						result = new Authorization(outgoingBlob, true);
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

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x060035DA RID: 13786 RVA: 0x000E5D38 File Offset: 0x000E4D38
		public string AuthenticationType
		{
			get
			{
				return "ntlm";
			}
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x000E5D3F File Offset: 0x000E4D3F
		public void CloseContext(object sessionCookie)
		{
		}

		// Token: 0x04003108 RID: 12552
		private Hashtable sessions = new Hashtable();
	}
}

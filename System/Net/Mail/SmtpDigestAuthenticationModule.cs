using System;
using System.Collections;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x020006C9 RID: 1737
	internal class SmtpDigestAuthenticationModule : ISmtpAuthenticationModule
	{
		// Token: 0x060035AA RID: 13738 RVA: 0x000E526B File Offset: 0x000E426B
		internal SmtpDigestAuthenticationModule()
		{
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x000E5280 File Offset: 0x000E4280
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
		public Authorization Authenticate(string challenge, NetworkCredential credential, object sessionCookie, string spn, ChannelBinding channelBindingToken)
		{
			Authorization result;
			lock (this.sessions)
			{
				NTAuthentication ntauthentication = this.sessions[sessionCookie] as NTAuthentication;
				if (ntauthentication == null)
				{
					if (credential == null)
					{
						return null;
					}
					ntauthentication = (this.sessions[sessionCookie] = new NTAuthentication(false, "WDigest", credential, spn, ContextFlags.Connection, channelBindingToken));
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
			return result;
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x060035AC RID: 13740 RVA: 0x000E5324 File Offset: 0x000E4324
		public string AuthenticationType
		{
			get
			{
				return "WDigest";
			}
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x000E532B File Offset: 0x000E432B
		public void CloseContext(object sessionCookie)
		{
		}

		// Token: 0x04003101 RID: 12545
		private Hashtable sessions = new Hashtable();
	}
}

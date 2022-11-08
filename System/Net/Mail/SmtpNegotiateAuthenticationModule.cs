using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;

namespace System.Net.Mail
{
	// Token: 0x020006CE RID: 1742
	internal class SmtpNegotiateAuthenticationModule : ISmtpAuthenticationModule
	{
		// Token: 0x060035D3 RID: 13779 RVA: 0x000E5A35 File Offset: 0x000E4A35
		internal SmtpNegotiateAuthenticationModule()
		{
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x000E5A48 File Offset: 0x000E4A48
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
						ntauthentication = (this.sessions[sessionCookie] = new NTAuthentication(false, "Negotiate", credential, spn, ContextFlags.Connection | ContextFlags.AcceptStream, channelBindingToken));
					}
					string token = null;
					if (!ntauthentication.IsCompleted)
					{
						byte[] incomingBlob = null;
						if (challenge != null)
						{
							incomingBlob = Convert.FromBase64String(challenge);
						}
						SecurityStatus securityStatus;
						byte[] outgoingBlob = ntauthentication.GetOutgoingBlob(incomingBlob, false, out securityStatus);
						if (ntauthentication.IsCompleted && outgoingBlob == null)
						{
							token = "\r\n";
						}
						if (outgoingBlob != null)
						{
							token = Convert.ToBase64String(outgoingBlob);
						}
					}
					else
					{
						token = this.GetSecurityLayerOutgoingBlob(challenge, ntauthentication);
					}
					result = new Authorization(token, ntauthentication.IsCompleted);
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

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x060035D5 RID: 13781 RVA: 0x000E5B58 File Offset: 0x000E4B58
		public string AuthenticationType
		{
			get
			{
				return "gssapi";
			}
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x000E5B60 File Offset: 0x000E4B60
		public void CloseContext(object sessionCookie)
		{
			NTAuthentication ntauthentication = null;
			lock (this.sessions)
			{
				ntauthentication = (this.sessions[sessionCookie] as NTAuthentication);
				if (ntauthentication != null)
				{
					this.sessions.Remove(sessionCookie);
				}
			}
			if (ntauthentication != null)
			{
				ntauthentication.CloseContext();
			}
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x000E5BC0 File Offset: 0x000E4BC0
		private string GetSecurityLayerOutgoingBlob(string challenge, NTAuthentication clientContext)
		{
			if (challenge == null)
			{
				return null;
			}
			byte[] array = Convert.FromBase64String(challenge);
			int num;
			try
			{
				num = clientContext.VerifySignature(array, 0, array.Length);
			}
			catch (Win32Exception)
			{
				return null;
			}
			if (num < 4 || array[0] != 1 || array[1] != 0 || array[2] != 0 || array[3] != 0)
			{
				return null;
			}
			byte[] inArray = null;
			try
			{
				num = clientContext.MakeSignature(array, 0, 4, ref inArray);
			}
			catch (Win32Exception)
			{
				return null;
			}
			return Convert.ToBase64String(inArray, 0, num);
		}

		// Token: 0x04003107 RID: 12551
		private Hashtable sessions = new Hashtable();
	}
}

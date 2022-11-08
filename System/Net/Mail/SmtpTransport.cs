using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace System.Net.Mail
{
	// Token: 0x020006DA RID: 1754
	internal class SmtpTransport
	{
		// Token: 0x0600360B RID: 13835 RVA: 0x000E6BFF File Offset: 0x000E5BFF
		internal SmtpTransport(SmtpClient client) : this(client, SmtpAuthenticationManager.GetModules())
		{
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x000E6C0D File Offset: 0x000E5C0D
		internal SmtpTransport(SmtpClient client, ISmtpAuthenticationModule[] authenticationModules)
		{
			this.client = client;
			if (authenticationModules == null)
			{
				throw new ArgumentNullException("authenticationModules");
			}
			this.authenticationModules = authenticationModules;
		}

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x0600360D RID: 13837 RVA: 0x000E6C47 File Offset: 0x000E5C47
		// (set) Token: 0x0600360E RID: 13838 RVA: 0x000E6C4F File Offset: 0x000E5C4F
		internal ICredentialsByHost Credentials
		{
			get
			{
				return this.credentials;
			}
			set
			{
				this.credentials = value;
			}
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x0600360F RID: 13839 RVA: 0x000E6C58 File Offset: 0x000E5C58
		// (set) Token: 0x06003610 RID: 13840 RVA: 0x000E6C60 File Offset: 0x000E5C60
		internal bool IdentityRequired
		{
			get
			{
				return this.m_IdentityRequired;
			}
			set
			{
				this.m_IdentityRequired = value;
			}
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x06003611 RID: 13841 RVA: 0x000E6C69 File Offset: 0x000E5C69
		internal bool IsConnected
		{
			get
			{
				return this.connection != null && this.connection.IsConnected;
			}
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x06003612 RID: 13842 RVA: 0x000E6C80 File Offset: 0x000E5C80
		// (set) Token: 0x06003613 RID: 13843 RVA: 0x000E6C88 File Offset: 0x000E5C88
		internal int Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.timeout = value;
			}
		}

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x06003614 RID: 13844 RVA: 0x000E6CA0 File Offset: 0x000E5CA0
		// (set) Token: 0x06003615 RID: 13845 RVA: 0x000E6CA8 File Offset: 0x000E5CA8
		internal bool EnableSsl
		{
			get
			{
				return this.enableSsl;
			}
			set
			{
				this.enableSsl = value;
			}
		}

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06003616 RID: 13846 RVA: 0x000E6CB1 File Offset: 0x000E5CB1
		internal X509CertificateCollection ClientCertificates
		{
			get
			{
				if (this.clientCertificates == null)
				{
					this.clientCertificates = new X509CertificateCollection();
				}
				return this.clientCertificates;
			}
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x000E6CCC File Offset: 0x000E5CCC
		internal void GetConnection(string host, int port)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (port < 0 || port > 65535)
			{
				throw new ArgumentOutOfRangeException("port");
			}
			this.connection = new SmtpConnection(this, this.client, this.credentials, this.authenticationModules);
			this.connection.Timeout = this.timeout;
			if (Logging.On)
			{
				Logging.Associate(Logging.Web, this, this.connection);
			}
			if (this.EnableSsl)
			{
				this.connection.EnableSsl = true;
				this.connection.ClientCertificates = this.ClientCertificates;
			}
			this.connection.GetConnection(host, port);
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x000E6D78 File Offset: 0x000E5D78
		internal IAsyncResult BeginGetConnection(string host, int port, ContextAwareResult outerResult, AsyncCallback callback, object state)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (port < 0 || port > 65535)
			{
				throw new ArgumentOutOfRangeException("port");
			}
			IAsyncResult result = null;
			try
			{
				this.connection = new SmtpConnection(this, this.client, this.credentials, this.authenticationModules);
				this.connection.Timeout = this.timeout;
				if (Logging.On)
				{
					Logging.Associate(Logging.Web, this, this.connection);
				}
				if (this.EnableSsl)
				{
					this.connection.EnableSsl = true;
					this.connection.ClientCertificates = this.ClientCertificates;
				}
				result = this.connection.BeginGetConnection(host, port, outerResult, callback, state);
			}
			catch (Exception innerException)
			{
				throw new SmtpException(SR.GetString("MailHostNotFound"), innerException);
			}
			catch
			{
				throw new SmtpException(SR.GetString("MailHostNotFound"), new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			return result;
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x000E6E7C File Offset: 0x000E5E7C
		internal void EndGetConnection(IAsyncResult result)
		{
			this.connection.EndGetConnection(result);
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x000E6E8C File Offset: 0x000E5E8C
		internal IAsyncResult BeginSendMail(MailAddress sender, MailAddressCollection recipients, string deliveryNotify, AsyncCallback callback, object state)
		{
			if (sender == null)
			{
				throw new ArgumentNullException("sender");
			}
			if (recipients == null)
			{
				throw new ArgumentNullException("recipients");
			}
			SendMailAsyncResult sendMailAsyncResult = new SendMailAsyncResult(this.connection, sender.SmtpAddress, recipients, this.connection.DSNEnabled ? deliveryNotify : null, callback, state);
			sendMailAsyncResult.Send();
			return sendMailAsyncResult;
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x000E6EE4 File Offset: 0x000E5EE4
		internal void ReleaseConnection()
		{
			if (this.connection != null)
			{
				this.connection.ReleaseConnection();
			}
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x000E6EF9 File Offset: 0x000E5EF9
		internal void Abort()
		{
			if (this.connection != null)
			{
				this.connection.Abort();
			}
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x000E6F10 File Offset: 0x000E5F10
		internal MailWriter EndSendMail(IAsyncResult result)
		{
			return SendMailAsyncResult.End(result);
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x000E6F28 File Offset: 0x000E5F28
		internal MailWriter SendMail(MailAddress sender, MailAddressCollection recipients, string deliveryNotify, out SmtpFailedRecipientException exception)
		{
			if (sender == null)
			{
				throw new ArgumentNullException("sender");
			}
			if (recipients == null)
			{
				throw new ArgumentNullException("recipients");
			}
			MailCommand.Send(this.connection, SmtpCommands.Mail, sender.SmtpAddress);
			this.failedRecipientExceptions.Clear();
			exception = null;
			foreach (MailAddress mailAddress in recipients)
			{
				string serverResponse;
				if (!RecipientCommand.Send(this.connection, this.connection.DSNEnabled ? (mailAddress.SmtpAddress + deliveryNotify) : mailAddress.SmtpAddress, out serverResponse))
				{
					this.failedRecipientExceptions.Add(new SmtpFailedRecipientException(this.connection.Reader.StatusCode, mailAddress.SmtpAddress, serverResponse));
				}
			}
			if (this.failedRecipientExceptions.Count > 0)
			{
				if (this.failedRecipientExceptions.Count == 1)
				{
					exception = (SmtpFailedRecipientException)this.failedRecipientExceptions[0];
				}
				else
				{
					exception = new SmtpFailedRecipientsException(this.failedRecipientExceptions, this.failedRecipientExceptions.Count == recipients.Count);
				}
				if (this.failedRecipientExceptions.Count == recipients.Count)
				{
					exception.fatal = true;
					throw exception;
				}
			}
			DataCommand.Send(this.connection);
			return new MailWriter(this.connection.GetClosableStream());
		}

		// Token: 0x0400314D RID: 12621
		internal const int DefaultPort = 25;

		// Token: 0x0400314E RID: 12622
		private ISmtpAuthenticationModule[] authenticationModules;

		// Token: 0x0400314F RID: 12623
		private SmtpConnection connection;

		// Token: 0x04003150 RID: 12624
		private SmtpClient client;

		// Token: 0x04003151 RID: 12625
		private ICredentialsByHost credentials;

		// Token: 0x04003152 RID: 12626
		private int timeout = 100000;

		// Token: 0x04003153 RID: 12627
		private ArrayList failedRecipientExceptions = new ArrayList();

		// Token: 0x04003154 RID: 12628
		private bool m_IdentityRequired;

		// Token: 0x04003155 RID: 12629
		private bool enableSsl;

		// Token: 0x04003156 RID: 12630
		private X509CertificateCollection clientCertificates;
	}
}

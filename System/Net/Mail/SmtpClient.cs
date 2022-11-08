using System;
using System.ComponentModel;
using System.IO;
using System.Net.Configuration;
using System.Net.NetworkInformation;
using System.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Threading;

namespace System.Net.Mail
{
	// Token: 0x020006B9 RID: 1721
	public class SmtpClient
	{
		// Token: 0x1400004E RID: 78
		// (add) Token: 0x06003514 RID: 13588 RVA: 0x000E1A14 File Offset: 0x000E0A14
		// (remove) Token: 0x06003515 RID: 13589 RVA: 0x000E1A2D File Offset: 0x000E0A2D
		public event SendCompletedEventHandler SendCompleted;

		// Token: 0x06003516 RID: 13590 RVA: 0x000E1A48 File Offset: 0x000E0A48
		public SmtpClient()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, "SmtpClient", ".ctor", "");
			}
			try
			{
				this.Initialize();
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, "SmtpClient", ".ctor", this);
				}
			}
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x000E1AB0 File Offset: 0x000E0AB0
		public SmtpClient(string host)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, "SmtpClient", ".ctor", "host=" + host);
			}
			try
			{
				this.host = host;
				this.Initialize();
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, "SmtpClient", ".ctor", this);
				}
			}
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x000E1B28 File Offset: 0x000E0B28
		public SmtpClient(string host, int port)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, "SmtpClient", ".ctor", string.Concat(new object[]
				{
					"host=",
					host,
					", port=",
					port
				}));
			}
			try
			{
				if (port < 0)
				{
					throw new ArgumentOutOfRangeException("port");
				}
				this.host = host;
				this.port = port;
				this.Initialize();
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, "SmtpClient", ".ctor", this);
				}
			}
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x000E1BD4 File Offset: 0x000E0BD4
		private void Initialize()
		{
			if (this.port == SmtpClient.defaultPort || this.port == 0)
			{
				new SmtpPermission(SmtpAccess.Connect).Demand();
			}
			else
			{
				new SmtpPermission(SmtpAccess.ConnectToUnrestrictedPort).Demand();
			}
			this.transport = new SmtpTransport(this);
			if (Logging.On)
			{
				Logging.Associate(Logging.Web, this, this.transport);
			}
			this.onSendCompletedDelegate = new SendOrPostCallback(this.SendCompletedWaitCallback);
			if (SmtpClient.MailConfiguration.Smtp != null)
			{
				if (SmtpClient.MailConfiguration.Smtp.Network != null)
				{
					if (this.host == null || this.host.Length == 0)
					{
						this.host = SmtpClient.MailConfiguration.Smtp.Network.Host;
					}
					if (this.port == 0)
					{
						this.port = SmtpClient.MailConfiguration.Smtp.Network.Port;
					}
					this.transport.Credentials = SmtpClient.MailConfiguration.Smtp.Network.Credential;
					this.clientDomain = SmtpClient.MailConfiguration.Smtp.Network.ClientDomain;
					if (SmtpClient.MailConfiguration.Smtp.Network.TargetName != null)
					{
						this.targetName = SmtpClient.MailConfiguration.Smtp.Network.TargetName;
					}
				}
				this.deliveryMethod = SmtpClient.MailConfiguration.Smtp.DeliveryMethod;
				if (SmtpClient.MailConfiguration.Smtp.SpecifiedPickupDirectory != null)
				{
					this.pickupDirectoryLocation = SmtpClient.MailConfiguration.Smtp.SpecifiedPickupDirectory.PickupDirectoryLocation;
				}
			}
			if (this.host != null && this.host.Length != 0)
			{
				this.host = this.host.Trim();
			}
			if (this.port == 0)
			{
				this.port = SmtpClient.defaultPort;
			}
			if (this.clientDomain == null)
			{
				this.clientDomain = IPGlobalProperties.InternalGetIPGlobalProperties().HostName;
			}
			if (this.targetName == null)
			{
				this.targetName = "SMTPSVC/" + this.host;
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x0600351A RID: 13594 RVA: 0x000E1DD0 File Offset: 0x000E0DD0
		// (set) Token: 0x0600351B RID: 13595 RVA: 0x000E1DD8 File Offset: 0x000E0DD8
		public string Host
		{
			get
			{
				return this.host;
			}
			set
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(SR.GetString("SmtpInvalidOperationDuringSend"));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value == string.Empty)
				{
					throw new ArgumentException(SR.GetString("net_emptystringset"), "value");
				}
				this.host = value.Trim();
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x0600351C RID: 13596 RVA: 0x000E1E39 File Offset: 0x000E0E39
		// (set) Token: 0x0600351D RID: 13597 RVA: 0x000E1E44 File Offset: 0x000E0E44
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(SR.GetString("SmtpInvalidOperationDuringSend"));
				}
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value != SmtpClient.defaultPort)
				{
					new SmtpPermission(SmtpAccess.ConnectToUnrestrictedPort).Demand();
				}
				this.port = value;
			}
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x0600351E RID: 13598 RVA: 0x000E1E92 File Offset: 0x000E0E92
		// (set) Token: 0x0600351F RID: 13599 RVA: 0x000E1EA9 File Offset: 0x000E0EA9
		public bool UseDefaultCredentials
		{
			get
			{
				return this.transport.Credentials is SystemNetworkCredential;
			}
			set
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(SR.GetString("SmtpInvalidOperationDuringSend"));
				}
				this.transport.Credentials = (value ? CredentialCache.DefaultNetworkCredentials : null);
			}
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x06003520 RID: 13600 RVA: 0x000E1ED9 File Offset: 0x000E0ED9
		// (set) Token: 0x06003521 RID: 13601 RVA: 0x000E1EE6 File Offset: 0x000E0EE6
		public ICredentialsByHost Credentials
		{
			get
			{
				return this.transport.Credentials;
			}
			set
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(SR.GetString("SmtpInvalidOperationDuringSend"));
				}
				this.transport.Credentials = value;
			}
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x06003522 RID: 13602 RVA: 0x000E1F0C File Offset: 0x000E0F0C
		// (set) Token: 0x06003523 RID: 13603 RVA: 0x000E1F19 File Offset: 0x000E0F19
		public int Timeout
		{
			get
			{
				return this.transport.Timeout;
			}
			set
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(SR.GetString("SmtpInvalidOperationDuringSend"));
				}
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.transport.Timeout = value;
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x06003524 RID: 13604 RVA: 0x000E1F4E File Offset: 0x000E0F4E
		public ServicePoint ServicePoint
		{
			get
			{
				this.CheckHostAndPort();
				return ServicePointManager.FindServicePoint(this.host, this.port);
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x06003525 RID: 13605 RVA: 0x000E1F67 File Offset: 0x000E0F67
		// (set) Token: 0x06003526 RID: 13606 RVA: 0x000E1F6F File Offset: 0x000E0F6F
		public SmtpDeliveryMethod DeliveryMethod
		{
			get
			{
				return this.deliveryMethod;
			}
			set
			{
				this.deliveryMethod = value;
			}
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x06003527 RID: 13607 RVA: 0x000E1F78 File Offset: 0x000E0F78
		// (set) Token: 0x06003528 RID: 13608 RVA: 0x000E1F80 File Offset: 0x000E0F80
		public string PickupDirectoryLocation
		{
			get
			{
				return this.pickupDirectoryLocation;
			}
			set
			{
				this.pickupDirectoryLocation = value;
			}
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x06003529 RID: 13609 RVA: 0x000E1F89 File Offset: 0x000E0F89
		// (set) Token: 0x0600352A RID: 13610 RVA: 0x000E1F96 File Offset: 0x000E0F96
		public bool EnableSsl
		{
			get
			{
				return this.transport.EnableSsl;
			}
			set
			{
				this.transport.EnableSsl = value;
			}
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x0600352B RID: 13611 RVA: 0x000E1FA4 File Offset: 0x000E0FA4
		public X509CertificateCollection ClientCertificates
		{
			get
			{
				return this.transport.ClientCertificates;
			}
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x0600352D RID: 13613 RVA: 0x000E1FBA File Offset: 0x000E0FBA
		// (set) Token: 0x0600352C RID: 13612 RVA: 0x000E1FB1 File Offset: 0x000E0FB1
		public string TargetName
		{
			get
			{
				return this.targetName;
			}
			set
			{
				this.targetName = value;
			}
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x000E1FC4 File Offset: 0x000E0FC4
		internal MailWriter GetFileMailWriter(string pickupDirectory)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, "SmtpClient.Send() pickupDirectory=" + pickupDirectory);
			}
			if (!Path.IsPathRooted(pickupDirectory))
			{
				throw new SmtpException(SR.GetString("SmtpNeedAbsolutePickupDirectory"));
			}
			string path2;
			do
			{
				string path = Guid.NewGuid().ToString() + ".eml";
				path2 = Path.Combine(pickupDirectory, path);
			}
			while (File.Exists(path2));
			FileStream stream = new FileStream(path2, FileMode.CreateNew);
			return new MailWriter(stream);
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x000E2041 File Offset: 0x000E1041
		protected void OnSendCompleted(AsyncCompletedEventArgs e)
		{
			if (this.SendCompleted != null)
			{
				this.SendCompleted(this, e);
			}
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x000E2058 File Offset: 0x000E1058
		private void SendCompletedWaitCallback(object operationState)
		{
			this.OnSendCompleted((AsyncCompletedEventArgs)operationState);
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x000E2068 File Offset: 0x000E1068
		public void Send(string from, string recipients, string subject, string body)
		{
			MailMessage mailMessage = new MailMessage(from, recipients, subject, body);
			this.Send(mailMessage);
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x000E2088 File Offset: 0x000E1088
		public void Send(MailMessage message)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "Send", message);
			}
			try
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.Web, this, "Send", "DeliveryMethod=" + this.DeliveryMethod.ToString());
				}
				if (Logging.On)
				{
					Logging.Associate(Logging.Web, this, message);
				}
				SmtpFailedRecipientException ex = null;
				if (this.InCall)
				{
					throw new InvalidOperationException(SR.GetString("net_inasync"));
				}
				if (message == null)
				{
					throw new ArgumentNullException("message");
				}
				if (this.DeliveryMethod == SmtpDeliveryMethod.Network)
				{
					this.CheckHostAndPort();
				}
				MailAddressCollection mailAddressCollection = new MailAddressCollection();
				if (message.From == null)
				{
					throw new InvalidOperationException(SR.GetString("SmtpFromRequired"));
				}
				if (message.To != null)
				{
					foreach (MailAddress item in message.To)
					{
						mailAddressCollection.Add(item);
					}
				}
				if (message.Bcc != null)
				{
					foreach (MailAddress item2 in message.Bcc)
					{
						mailAddressCollection.Add(item2);
					}
				}
				if (message.CC != null)
				{
					foreach (MailAddress item3 in message.CC)
					{
						mailAddressCollection.Add(item3);
					}
				}
				if (mailAddressCollection.Count == 0)
				{
					throw new InvalidOperationException(SR.GetString("SmtpRecipientRequired"));
				}
				this.transport.IdentityRequired = false;
				try
				{
					this.InCall = true;
					this.timedOut = false;
					this.timer = new Timer(new TimerCallback(this.TimeOutCallback), null, this.Timeout, this.Timeout);
					MailWriter mailWriter;
					switch (this.DeliveryMethod)
					{
					case SmtpDeliveryMethod.SpecifiedPickupDirectory:
						if (this.EnableSsl)
						{
							throw new SmtpException(SR.GetString("SmtpPickupDirectoryDoesnotSupportSsl"));
						}
						mailWriter = this.GetFileMailWriter(this.PickupDirectoryLocation);
						goto IL_25D;
					case SmtpDeliveryMethod.PickupDirectoryFromIis:
						if (this.EnableSsl)
						{
							throw new SmtpException(SR.GetString("SmtpPickupDirectoryDoesnotSupportSsl"));
						}
						mailWriter = this.GetFileMailWriter(IisPickupDirectory.GetPickupDirectory());
						goto IL_25D;
					}
					this.GetConnection();
					mailWriter = this.transport.SendMail((message.Sender != null) ? message.Sender : message.From, mailAddressCollection, message.BuildDeliveryStatusNotificationString(), out ex);
					IL_25D:
					this.message = message;
					message.Send(mailWriter, this.DeliveryMethod != SmtpDeliveryMethod.Network);
					mailWriter.Close();
					this.transport.ReleaseConnection();
					if (this.DeliveryMethod == SmtpDeliveryMethod.Network && ex != null)
					{
						throw ex;
					}
				}
				catch (Exception ex2)
				{
					if (Logging.On)
					{
						Logging.Exception(Logging.Web, this, "Send", ex2);
					}
					if (ex2 is SmtpFailedRecipientException && !((SmtpFailedRecipientException)ex2).fatal)
					{
						throw;
					}
					this.Abort();
					if (this.timedOut)
					{
						throw new SmtpException(SR.GetString("net_timeout"));
					}
					if (ex2 is SecurityException || ex2 is AuthenticationException || ex2 is SmtpException)
					{
						throw;
					}
					throw new SmtpException(SR.GetString("SmtpSendMailFailure"), ex2);
				}
				finally
				{
					this.InCall = false;
					if (this.timer != null)
					{
						this.timer.Dispose();
					}
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "Send", null);
				}
			}
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x000E2480 File Offset: 0x000E1480
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(string from, string recipients, string subject, string body, object userToken)
		{
			this.SendAsync(new MailMessage(from, recipients, subject, body), userToken);
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x000E2494 File Offset: 0x000E1494
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(MailMessage message, object userToken)
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "SendAsync", "DeliveryMethod=" + this.DeliveryMethod.ToString());
			}
			try
			{
				if (this.InCall)
				{
					throw new InvalidOperationException(SR.GetString("net_inasync"));
				}
				if (message == null)
				{
					throw new ArgumentNullException("message");
				}
				if (this.DeliveryMethod == SmtpDeliveryMethod.Network)
				{
					this.CheckHostAndPort();
				}
				this.recipients = new MailAddressCollection();
				if (message.From == null)
				{
					throw new InvalidOperationException(SR.GetString("SmtpFromRequired"));
				}
				if (message.To != null)
				{
					foreach (MailAddress item in message.To)
					{
						this.recipients.Add(item);
					}
				}
				if (message.Bcc != null)
				{
					foreach (MailAddress item2 in message.Bcc)
					{
						this.recipients.Add(item2);
					}
				}
				if (message.CC != null)
				{
					foreach (MailAddress item3 in message.CC)
					{
						this.recipients.Add(item3);
					}
				}
				if (this.recipients.Count == 0)
				{
					throw new InvalidOperationException(SR.GetString("SmtpRecipientRequired"));
				}
				try
				{
					this.InCall = true;
					this.cancelled = false;
					this.message = message;
					CredentialCache credentialCache;
					this.transport.IdentityRequired = (this.Credentials != null && ComNetOS.IsWinNt && (this.Credentials is SystemNetworkCredential || (credentialCache = (this.Credentials as CredentialCache)) == null || credentialCache.IsDefaultInCache));
					this.asyncOp = AsyncOperationManager.CreateOperation(userToken);
					switch (this.DeliveryMethod)
					{
					case SmtpDeliveryMethod.SpecifiedPickupDirectory:
					{
						if (this.EnableSsl)
						{
							throw new SmtpException(SR.GetString("SmtpPickupDirectoryDoesnotSupportSsl"));
						}
						this.writer = this.GetFileMailWriter(this.PickupDirectoryLocation);
						message.Send(this.writer, this.DeliveryMethod != SmtpDeliveryMethod.Network);
						if (this.writer != null)
						{
							this.writer.Close();
						}
						this.transport.ReleaseConnection();
						AsyncCompletedEventArgs arg = new AsyncCompletedEventArgs(null, false, this.asyncOp.UserSuppliedState);
						this.InCall = false;
						this.asyncOp.PostOperationCompleted(this.onSendCompletedDelegate, arg);
						goto IL_387;
					}
					case SmtpDeliveryMethod.PickupDirectoryFromIis:
					{
						if (this.EnableSsl)
						{
							throw new SmtpException(SR.GetString("SmtpPickupDirectoryDoesnotSupportSsl"));
						}
						this.writer = this.GetFileMailWriter(IisPickupDirectory.GetPickupDirectory());
						message.Send(this.writer, this.DeliveryMethod != SmtpDeliveryMethod.Network);
						if (this.writer != null)
						{
							this.writer.Close();
						}
						this.transport.ReleaseConnection();
						AsyncCompletedEventArgs arg2 = new AsyncCompletedEventArgs(null, false, this.asyncOp.UserSuppliedState);
						this.InCall = false;
						this.asyncOp.PostOperationCompleted(this.onSendCompletedDelegate, arg2);
						goto IL_387;
					}
					}
					this.operationCompletedResult = new ContextAwareResult(this.transport.IdentityRequired, true, null, this, SmtpClient._ContextSafeCompleteCallback);
					lock (this.operationCompletedResult.StartPostingAsyncOp())
					{
						this.transport.BeginGetConnection(this.host, this.port, this.operationCompletedResult, new AsyncCallback(this.ConnectCallback), this.operationCompletedResult);
						this.operationCompletedResult.FinishPostingAsyncOp();
					}
					IL_387:;
				}
				catch (Exception ex)
				{
					this.InCall = false;
					if (Logging.On)
					{
						Logging.Exception(Logging.Web, this, "Send", ex);
					}
					if (ex is SmtpFailedRecipientException && !((SmtpFailedRecipientException)ex).fatal)
					{
						throw;
					}
					this.Abort();
					if (this.timedOut)
					{
						throw new SmtpException(SR.GetString("net_timeout"));
					}
					if (ex is SecurityException || ex is AuthenticationException || ex is SmtpException)
					{
						throw;
					}
					throw new SmtpException(SR.GetString("SmtpSendMailFailure"), ex);
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "SendAsync", null);
				}
			}
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x000E2964 File Offset: 0x000E1964
		public void SendAsyncCancel()
		{
			if (Logging.On)
			{
				Logging.Enter(Logging.Web, this, "SendAsyncCancel", null);
			}
			try
			{
				if (this.InCall && !this.cancelled)
				{
					this.cancelled = true;
					this.Abort();
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.Web, this, "SendAsyncCancel", null);
				}
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06003536 RID: 13622 RVA: 0x000E29D4 File Offset: 0x000E19D4
		// (set) Token: 0x06003537 RID: 13623 RVA: 0x000E29DC File Offset: 0x000E19DC
		internal bool InCall
		{
			get
			{
				return this.inCall;
			}
			set
			{
				this.inCall = value;
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06003538 RID: 13624 RVA: 0x000E29E5 File Offset: 0x000E19E5
		internal static MailSettingsSectionGroupInternal MailConfiguration
		{
			get
			{
				if (SmtpClient.mailConfiguration == null)
				{
					SmtpClient.mailConfiguration = MailSettingsSectionGroupInternal.GetSection();
				}
				return SmtpClient.mailConfiguration;
			}
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x000E29FD File Offset: 0x000E19FD
		private void CheckHostAndPort()
		{
			if (this.host == null || this.host.Length == 0)
			{
				throw new InvalidOperationException(SR.GetString("UnspecifiedHost"));
			}
			if (this.port == 0)
			{
				throw new InvalidOperationException(SR.GetString("InvalidPort"));
			}
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x000E2A3C File Offset: 0x000E1A3C
		private void TimeOutCallback(object state)
		{
			if (!this.timedOut)
			{
				this.timedOut = true;
				this.Abort();
			}
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x000E2A54 File Offset: 0x000E1A54
		private void Complete(Exception exception, IAsyncResult result)
		{
			ContextAwareResult contextAwareResult = (ContextAwareResult)result.AsyncState;
			try
			{
				if (this.cancelled)
				{
					exception = null;
					this.Abort();
				}
				else if (exception != null && (!(exception is SmtpFailedRecipientException) || ((SmtpFailedRecipientException)exception).fatal))
				{
					this.Abort();
					if (!(exception is SmtpException))
					{
						exception = new SmtpException(SR.GetString("SmtpSendMailFailure"), exception);
					}
				}
				else
				{
					if (this.writer != null)
					{
						this.writer.Close();
					}
					this.transport.ReleaseConnection();
				}
			}
			finally
			{
				contextAwareResult.InvokeCallback(exception);
			}
		}

		// Token: 0x0600353C RID: 13628 RVA: 0x000E2AF4 File Offset: 0x000E1AF4
		private static void ContextSafeCompleteCallback(IAsyncResult ar)
		{
			ContextAwareResult contextAwareResult = (ContextAwareResult)ar;
			SmtpClient smtpClient = (SmtpClient)ar.AsyncState;
			Exception error = contextAwareResult.Result as Exception;
			AsyncOperation asyncOperation = smtpClient.asyncOp;
			AsyncCompletedEventArgs arg = new AsyncCompletedEventArgs(error, smtpClient.cancelled, asyncOperation.UserSuppliedState);
			smtpClient.InCall = false;
			asyncOperation.PostOperationCompleted(smtpClient.onSendCompletedDelegate, arg);
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x000E2B50 File Offset: 0x000E1B50
		private void SendMessageCallback(IAsyncResult result)
		{
			try
			{
				this.message.EndSend(result);
				this.Complete(null, result);
			}
			catch (Exception exception)
			{
				this.Complete(exception, result);
			}
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x000E2B90 File Offset: 0x000E1B90
		private void SendMailCallback(IAsyncResult result)
		{
			try
			{
				this.writer = this.transport.EndSendMail(result);
			}
			catch (Exception ex)
			{
				if (!(ex is SmtpFailedRecipientException) || ((SmtpFailedRecipientException)ex).fatal)
				{
					this.Complete(ex, result);
					return;
				}
			}
			catch
			{
				this.Complete(new Exception(SR.GetString("net_nonClsCompliantException")), result);
				return;
			}
			try
			{
				if (this.cancelled)
				{
					this.Complete(null, result);
				}
				else
				{
					this.message.BeginSend(this.writer, this.DeliveryMethod != SmtpDeliveryMethod.Network, new AsyncCallback(this.SendMessageCallback), result.AsyncState);
				}
			}
			catch (Exception exception)
			{
				this.Complete(exception, result);
			}
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x000E2C64 File Offset: 0x000E1C64
		private void ConnectCallback(IAsyncResult result)
		{
			try
			{
				this.transport.EndGetConnection(result);
				if (this.cancelled)
				{
					this.Complete(null, result);
				}
				else
				{
					this.transport.BeginSendMail((this.message.Sender != null) ? this.message.Sender : this.message.From, this.recipients, this.message.BuildDeliveryStatusNotificationString(), new AsyncCallback(this.SendMailCallback), result.AsyncState);
				}
			}
			catch (Exception exception)
			{
				this.Complete(exception, result);
			}
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x000E2D00 File Offset: 0x000E1D00
		private void GetConnection()
		{
			if (!this.transport.IsConnected)
			{
				this.transport.GetConnection(this.host, this.port);
			}
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x000E2D28 File Offset: 0x000E1D28
		private void Abort()
		{
			try
			{
				this.transport.Abort();
			}
			catch
			{
			}
		}

		// Token: 0x040030AC RID: 12460
		private string host;

		// Token: 0x040030AD RID: 12461
		private int port;

		// Token: 0x040030AE RID: 12462
		private bool inCall;

		// Token: 0x040030AF RID: 12463
		private bool cancelled;

		// Token: 0x040030B0 RID: 12464
		private bool timedOut;

		// Token: 0x040030B1 RID: 12465
		private string targetName;

		// Token: 0x040030B2 RID: 12466
		private SmtpDeliveryMethod deliveryMethod;

		// Token: 0x040030B3 RID: 12467
		private string pickupDirectoryLocation;

		// Token: 0x040030B4 RID: 12468
		private SmtpTransport transport;

		// Token: 0x040030B5 RID: 12469
		private MailMessage message;

		// Token: 0x040030B6 RID: 12470
		private MailWriter writer;

		// Token: 0x040030B7 RID: 12471
		private MailAddressCollection recipients;

		// Token: 0x040030B8 RID: 12472
		private SendOrPostCallback onSendCompletedDelegate;

		// Token: 0x040030B9 RID: 12473
		private Timer timer;

		// Token: 0x040030BA RID: 12474
		private static MailSettingsSectionGroupInternal mailConfiguration;

		// Token: 0x040030BB RID: 12475
		private ContextAwareResult operationCompletedResult;

		// Token: 0x040030BC RID: 12476
		private AsyncOperation asyncOp;

		// Token: 0x040030BD RID: 12477
		private static AsyncCallback _ContextSafeCompleteCallback = new AsyncCallback(SmtpClient.ContextSafeCompleteCallback);

		// Token: 0x040030BE RID: 12478
		private static int defaultPort = 25;

		// Token: 0x040030BF RID: 12479
		internal string clientDomain;
	}
}

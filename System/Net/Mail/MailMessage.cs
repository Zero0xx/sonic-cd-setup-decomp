using System;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x0200069E RID: 1694
	public class MailMessage : IDisposable
	{
		// Token: 0x06003443 RID: 13379 RVA: 0x000DDE84 File Offset: 0x000DCE84
		public MailMessage()
		{
			this.message = new Message();
			if (Logging.On)
			{
				Logging.Associate(Logging.Web, this, this.message);
			}
			string from = SmtpClient.MailConfiguration.Smtp.From;
			if (from != null && from.Length > 0)
			{
				this.message.From = new MailAddress(from);
			}
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x000DDEF4 File Offset: 0x000DCEF4
		public MailMessage(string from, string to)
		{
			if (from == null)
			{
				throw new ArgumentNullException("from");
			}
			if (to == null)
			{
				throw new ArgumentNullException("to");
			}
			if (from == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"from"
				}), "from");
			}
			if (to == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[]
				{
					"to"
				}), "to");
			}
			this.message = new Message(from, to);
			if (Logging.On)
			{
				Logging.Associate(Logging.Web, this, this.message);
			}
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x000DDFB7 File Offset: 0x000DCFB7
		public MailMessage(string from, string to, string subject, string body) : this(from, to)
		{
			this.Subject = subject;
			this.Body = body;
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x000DDFD0 File Offset: 0x000DCFD0
		public MailMessage(MailAddress from, MailAddress to)
		{
			if (from == null)
			{
				throw new ArgumentNullException("from");
			}
			if (to == null)
			{
				throw new ArgumentNullException("to");
			}
			this.message = new Message(from, to);
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x06003447 RID: 13383 RVA: 0x000DE00C File Offset: 0x000DD00C
		// (set) Token: 0x06003448 RID: 13384 RVA: 0x000DE019 File Offset: 0x000DD019
		public MailAddress From
		{
			get
			{
				return this.message.From;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.message.From = value;
			}
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x06003449 RID: 13385 RVA: 0x000DE035 File Offset: 0x000DD035
		// (set) Token: 0x0600344A RID: 13386 RVA: 0x000DE042 File Offset: 0x000DD042
		public MailAddress Sender
		{
			get
			{
				return this.message.Sender;
			}
			set
			{
				this.message.Sender = value;
			}
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x0600344B RID: 13387 RVA: 0x000DE050 File Offset: 0x000DD050
		// (set) Token: 0x0600344C RID: 13388 RVA: 0x000DE05D File Offset: 0x000DD05D
		public MailAddress ReplyTo
		{
			get
			{
				return this.message.ReplyTo;
			}
			set
			{
				this.message.ReplyTo = value;
			}
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x0600344D RID: 13389 RVA: 0x000DE06B File Offset: 0x000DD06B
		public MailAddressCollection To
		{
			get
			{
				return this.message.To;
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x0600344E RID: 13390 RVA: 0x000DE078 File Offset: 0x000DD078
		public MailAddressCollection Bcc
		{
			get
			{
				return this.message.Bcc;
			}
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x0600344F RID: 13391 RVA: 0x000DE085 File Offset: 0x000DD085
		public MailAddressCollection CC
		{
			get
			{
				return this.message.CC;
			}
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x06003450 RID: 13392 RVA: 0x000DE092 File Offset: 0x000DD092
		// (set) Token: 0x06003451 RID: 13393 RVA: 0x000DE09F File Offset: 0x000DD09F
		public MailPriority Priority
		{
			get
			{
				return this.message.Priority;
			}
			set
			{
				this.message.Priority = value;
			}
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x06003452 RID: 13394 RVA: 0x000DE0AD File Offset: 0x000DD0AD
		// (set) Token: 0x06003453 RID: 13395 RVA: 0x000DE0B5 File Offset: 0x000DD0B5
		public DeliveryNotificationOptions DeliveryNotificationOptions
		{
			get
			{
				return this.deliveryStatusNotification;
			}
			set
			{
				if ((DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.Delay) < value && value != DeliveryNotificationOptions.Never)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.deliveryStatusNotification = value;
			}
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x06003454 RID: 13396 RVA: 0x000DE0D5 File Offset: 0x000DD0D5
		// (set) Token: 0x06003455 RID: 13397 RVA: 0x000DE0F5 File Offset: 0x000DD0F5
		public string Subject
		{
			get
			{
				if (this.message.Subject == null)
				{
					return string.Empty;
				}
				return this.message.Subject;
			}
			set
			{
				this.message.Subject = value;
			}
		}

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x06003456 RID: 13398 RVA: 0x000DE103 File Offset: 0x000DD103
		// (set) Token: 0x06003457 RID: 13399 RVA: 0x000DE110 File Offset: 0x000DD110
		public Encoding SubjectEncoding
		{
			get
			{
				return this.message.SubjectEncoding;
			}
			set
			{
				this.message.SubjectEncoding = value;
			}
		}

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x06003458 RID: 13400 RVA: 0x000DE11E File Offset: 0x000DD11E
		public NameValueCollection Headers
		{
			get
			{
				return this.message.Headers;
			}
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06003459 RID: 13401 RVA: 0x000DE12B File Offset: 0x000DD12B
		// (set) Token: 0x0600345A RID: 13402 RVA: 0x000DE144 File Offset: 0x000DD144
		public string Body
		{
			get
			{
				if (this.body == null)
				{
					return string.Empty;
				}
				return this.body;
			}
			set
			{
				this.body = value;
				if (this.bodyEncoding == null && this.body != null)
				{
					if (MimeBasePart.IsAscii(this.body, true))
					{
						this.bodyEncoding = Encoding.ASCII;
						return;
					}
					this.bodyEncoding = Encoding.GetEncoding("utf-8");
				}
			}
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x0600345B RID: 13403 RVA: 0x000DE192 File Offset: 0x000DD192
		// (set) Token: 0x0600345C RID: 13404 RVA: 0x000DE19A File Offset: 0x000DD19A
		public Encoding BodyEncoding
		{
			get
			{
				return this.bodyEncoding;
			}
			set
			{
				this.bodyEncoding = value;
			}
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x0600345D RID: 13405 RVA: 0x000DE1A3 File Offset: 0x000DD1A3
		// (set) Token: 0x0600345E RID: 13406 RVA: 0x000DE1AB File Offset: 0x000DD1AB
		public bool IsBodyHtml
		{
			get
			{
				return this.isBodyHtml;
			}
			set
			{
				this.isBodyHtml = value;
			}
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x0600345F RID: 13407 RVA: 0x000DE1B4 File Offset: 0x000DD1B4
		public AttachmentCollection Attachments
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				if (this.attachments == null)
				{
					this.attachments = new AttachmentCollection();
				}
				return this.attachments;
			}
		}

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06003460 RID: 13408 RVA: 0x000DE1E8 File Offset: 0x000DD1E8
		public AlternateViewCollection AlternateViews
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				if (this.views == null)
				{
					this.views = new AlternateViewCollection();
				}
				return this.views;
			}
		}

		// Token: 0x06003461 RID: 13409 RVA: 0x000DE21C File Offset: 0x000DD21C
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06003462 RID: 13410 RVA: 0x000DE228 File Offset: 0x000DD228
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !this.disposed)
			{
				this.disposed = true;
				if (this.views != null)
				{
					this.views.Dispose();
				}
				if (this.attachments != null)
				{
					this.attachments.Dispose();
				}
				if (this.bodyView != null)
				{
					this.bodyView.Dispose();
				}
			}
		}

		// Token: 0x06003463 RID: 13411 RVA: 0x000DE280 File Offset: 0x000DD280
		private void SetContent()
		{
			if (this.bodyView != null)
			{
				this.bodyView.Dispose();
				this.bodyView = null;
			}
			if (this.AlternateViews.Count == 0 && this.Attachments.Count == 0)
			{
				if (this.body != null && this.body != string.Empty)
				{
					this.bodyView = AlternateView.CreateAlternateViewFromString(this.body, this.bodyEncoding, this.isBodyHtml ? "text/html" : null);
					this.message.Content = this.bodyView.MimePart;
					return;
				}
			}
			else
			{
				if (this.AlternateViews.Count == 0 && this.Attachments.Count > 0)
				{
					MimeMultiPart mimeMultiPart = new MimeMultiPart(MimeMultiPartType.Mixed);
					if (this.body != null && this.body != string.Empty)
					{
						this.bodyView = AlternateView.CreateAlternateViewFromString(this.body, this.bodyEncoding, this.isBodyHtml ? "text/html" : null);
					}
					else
					{
						this.bodyView = AlternateView.CreateAlternateViewFromString(string.Empty);
					}
					mimeMultiPart.Parts.Add(this.bodyView.MimePart);
					foreach (Attachment attachment in this.Attachments)
					{
						if (attachment != null)
						{
							attachment.PrepareForSending();
							mimeMultiPart.Parts.Add(attachment.MimePart);
						}
					}
					this.message.Content = mimeMultiPart;
					return;
				}
				MimeMultiPart mimeMultiPart2 = null;
				MimeMultiPart mimeMultiPart3 = new MimeMultiPart(MimeMultiPartType.Alternative);
				if (this.body != null && this.body != string.Empty)
				{
					this.bodyView = AlternateView.CreateAlternateViewFromString(this.body, this.bodyEncoding, null);
					mimeMultiPart3.Parts.Add(this.bodyView.MimePart);
				}
				foreach (AlternateView alternateView in this.AlternateViews)
				{
					if (alternateView != null)
					{
						alternateView.PrepareForSending();
						if (alternateView.LinkedResources.Count > 0)
						{
							MimeMultiPart mimeMultiPart4 = new MimeMultiPart(MimeMultiPartType.Related);
							mimeMultiPart4.ContentType.Parameters["type"] = alternateView.ContentType.MediaType;
							mimeMultiPart4.ContentLocation = alternateView.MimePart.ContentLocation;
							mimeMultiPart4.Parts.Add(alternateView.MimePart);
							foreach (LinkedResource linkedResource in alternateView.LinkedResources)
							{
								linkedResource.PrepareForSending();
								mimeMultiPart4.Parts.Add(linkedResource.MimePart);
							}
							mimeMultiPart3.Parts.Add(mimeMultiPart4);
						}
						else
						{
							mimeMultiPart3.Parts.Add(alternateView.MimePart);
						}
					}
				}
				if (this.Attachments.Count > 0)
				{
					mimeMultiPart2 = new MimeMultiPart(MimeMultiPartType.Mixed);
					mimeMultiPart2.Parts.Add(mimeMultiPart3);
					MimeMultiPart mimeMultiPart5 = new MimeMultiPart(MimeMultiPartType.Mixed);
					foreach (Attachment attachment2 in this.Attachments)
					{
						if (attachment2 != null)
						{
							attachment2.PrepareForSending();
							mimeMultiPart5.Parts.Add(attachment2.MimePart);
						}
					}
					mimeMultiPart2.Parts.Add(mimeMultiPart5);
					this.message.Content = mimeMultiPart2;
					return;
				}
				if (mimeMultiPart3.Parts.Count == 1 && (this.body == null || this.body == string.Empty))
				{
					this.message.Content = mimeMultiPart3.Parts[0];
					return;
				}
				this.message.Content = mimeMultiPart3;
			}
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x000DE67C File Offset: 0x000DD67C
		internal void Send(BaseWriter writer, bool sendEnvelope)
		{
			this.SetContent();
			this.message.Send(writer, sendEnvelope);
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x000DE691 File Offset: 0x000DD691
		internal IAsyncResult BeginSend(BaseWriter writer, bool sendEnvelope, AsyncCallback callback, object state)
		{
			this.SetContent();
			return this.message.BeginSend(writer, sendEnvelope, callback, state);
		}

		// Token: 0x06003466 RID: 13414 RVA: 0x000DE6A9 File Offset: 0x000DD6A9
		internal void EndSend(IAsyncResult asyncResult)
		{
			this.message.EndSend(asyncResult);
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x000DE6B8 File Offset: 0x000DD6B8
		internal string BuildDeliveryStatusNotificationString()
		{
			if (this.deliveryStatusNotification == DeliveryNotificationOptions.None)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(" NOTIFY=");
			bool flag = false;
			if (this.deliveryStatusNotification == DeliveryNotificationOptions.Never)
			{
				stringBuilder.Append("NEVER");
				return stringBuilder.ToString();
			}
			if ((this.deliveryStatusNotification & DeliveryNotificationOptions.OnSuccess) > DeliveryNotificationOptions.None)
			{
				stringBuilder.Append("SUCCESS");
				flag = true;
			}
			if ((this.deliveryStatusNotification & DeliveryNotificationOptions.OnFailure) > DeliveryNotificationOptions.None)
			{
				if (flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append("FAILURE");
				flag = true;
			}
			if ((this.deliveryStatusNotification & DeliveryNotificationOptions.Delay) > DeliveryNotificationOptions.None)
			{
				if (flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append("DELAY");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04003030 RID: 12336
		private AlternateViewCollection views;

		// Token: 0x04003031 RID: 12337
		private AttachmentCollection attachments;

		// Token: 0x04003032 RID: 12338
		private AlternateView bodyView;

		// Token: 0x04003033 RID: 12339
		private string body = string.Empty;

		// Token: 0x04003034 RID: 12340
		private Encoding bodyEncoding;

		// Token: 0x04003035 RID: 12341
		private bool isBodyHtml;

		// Token: 0x04003036 RID: 12342
		private bool disposed;

		// Token: 0x04003037 RID: 12343
		private Message message;

		// Token: 0x04003038 RID: 12344
		private DeliveryNotificationOptions deliveryStatusNotification;
	}
}

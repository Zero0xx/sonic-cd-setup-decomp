using System;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x020006A5 RID: 1701
	internal class Message
	{
		// Token: 0x06003478 RID: 13432 RVA: 0x000DEC3C File Offset: 0x000DDC3C
		internal Message()
		{
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x000DEC4C File Offset: 0x000DDC4C
		internal Message(string from, string to) : this()
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
			this.from = new MailAddress(from);
			this.to = new MailAddressCollection
			{
				to
			};
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x000DECFF File Offset: 0x000DDCFF
		internal Message(MailAddress from, MailAddress to) : this()
		{
			this.from = from;
			this.To.Add(to);
		}

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x0600347B RID: 13435 RVA: 0x000DED1A File Offset: 0x000DDD1A
		// (set) Token: 0x0600347C RID: 13436 RVA: 0x000DED2D File Offset: 0x000DDD2D
		public MailPriority Priority
		{
			get
			{
				if (this.priority != (MailPriority)(-1))
				{
					return this.priority;
				}
				return MailPriority.Normal;
			}
			set
			{
				this.priority = value;
			}
		}

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x0600347D RID: 13437 RVA: 0x000DED36 File Offset: 0x000DDD36
		// (set) Token: 0x0600347E RID: 13438 RVA: 0x000DED3E File Offset: 0x000DDD3E
		internal MailAddress From
		{
			get
			{
				return this.from;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.from = value;
			}
		}

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x0600347F RID: 13439 RVA: 0x000DED55 File Offset: 0x000DDD55
		// (set) Token: 0x06003480 RID: 13440 RVA: 0x000DED5D File Offset: 0x000DDD5D
		internal MailAddress Sender
		{
			get
			{
				return this.sender;
			}
			set
			{
				this.sender = value;
			}
		}

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06003481 RID: 13441 RVA: 0x000DED66 File Offset: 0x000DDD66
		// (set) Token: 0x06003482 RID: 13442 RVA: 0x000DED6E File Offset: 0x000DDD6E
		internal MailAddress ReplyTo
		{
			get
			{
				return this.replyTo;
			}
			set
			{
				this.replyTo = value;
			}
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06003483 RID: 13443 RVA: 0x000DED77 File Offset: 0x000DDD77
		internal MailAddressCollection To
		{
			get
			{
				if (this.to == null)
				{
					this.to = new MailAddressCollection();
				}
				return this.to;
			}
		}

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06003484 RID: 13444 RVA: 0x000DED92 File Offset: 0x000DDD92
		internal MailAddressCollection Bcc
		{
			get
			{
				if (this.bcc == null)
				{
					this.bcc = new MailAddressCollection();
				}
				return this.bcc;
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06003485 RID: 13445 RVA: 0x000DEDAD File Offset: 0x000DDDAD
		internal MailAddressCollection CC
		{
			get
			{
				if (this.cc == null)
				{
					this.cc = new MailAddressCollection();
				}
				return this.cc;
			}
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06003486 RID: 13446 RVA: 0x000DEDC8 File Offset: 0x000DDDC8
		// (set) Token: 0x06003487 RID: 13447 RVA: 0x000DEDD0 File Offset: 0x000DDDD0
		internal string Subject
		{
			get
			{
				return this.subject;
			}
			set
			{
				if (value != null && MailBnfHelper.HasCROrLF(value))
				{
					throw new ArgumentException(SR.GetString("MailSubjectInvalidFormat"));
				}
				this.subject = value;
				if (this.subject != null && this.subjectEncoding == null && !MimeBasePart.IsAscii(this.subject, false))
				{
					this.subjectEncoding = Encoding.GetEncoding("utf-8");
				}
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06003488 RID: 13448 RVA: 0x000DEE2D File Offset: 0x000DDE2D
		// (set) Token: 0x06003489 RID: 13449 RVA: 0x000DEE35 File Offset: 0x000DDE35
		internal Encoding SubjectEncoding
		{
			get
			{
				return this.subjectEncoding;
			}
			set
			{
				this.subjectEncoding = value;
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x0600348A RID: 13450 RVA: 0x000DEE3E File Offset: 0x000DDE3E
		internal NameValueCollection Headers
		{
			get
			{
				if (this.headers == null)
				{
					this.headers = new HeaderCollection();
					if (Logging.On)
					{
						Logging.Associate(Logging.Web, this, this.headers);
					}
				}
				return this.headers;
			}
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x0600348B RID: 13451 RVA: 0x000DEE71 File Offset: 0x000DDE71
		internal NameValueCollection EnvelopeHeaders
		{
			get
			{
				if (this.envelopeHeaders == null)
				{
					this.envelopeHeaders = new HeaderCollection();
					if (Logging.On)
					{
						Logging.Associate(Logging.Web, this, this.envelopeHeaders);
					}
				}
				return this.envelopeHeaders;
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x0600348C RID: 13452 RVA: 0x000DEEA4 File Offset: 0x000DDEA4
		// (set) Token: 0x0600348D RID: 13453 RVA: 0x000DEEAC File Offset: 0x000DDEAC
		internal virtual MimeBasePart Content
		{
			get
			{
				return this.content;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.content = value;
			}
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x000DEEC4 File Offset: 0x000DDEC4
		internal void EmptySendCallback(IAsyncResult result)
		{
			Exception result2 = null;
			if (result.CompletedSynchronously)
			{
				return;
			}
			Message.EmptySendContext emptySendContext = (Message.EmptySendContext)result.AsyncState;
			try
			{
				emptySendContext.writer.EndGetContentStream(result).Close();
			}
			catch (Exception ex)
			{
				result2 = ex;
			}
			catch
			{
				result2 = new Exception(SR.GetString("net_nonClsCompliantException"));
			}
			emptySendContext.result.InvokeCallback(result2);
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x000DEF3C File Offset: 0x000DDF3C
		internal virtual IAsyncResult BeginSend(BaseWriter writer, bool sendEnvelope, AsyncCallback callback, object state)
		{
			this.PrepareHeaders(sendEnvelope);
			writer.WriteHeaders(this.Headers);
			if (this.Content != null)
			{
				return this.Content.BeginSend(writer, callback, state);
			}
			LazyAsyncResult result = new LazyAsyncResult(this, state, callback);
			IAsyncResult asyncResult = writer.BeginGetContentStream(new AsyncCallback(this.EmptySendCallback), new Message.EmptySendContext(writer, result));
			if (asyncResult.CompletedSynchronously)
			{
				writer.EndGetContentStream(asyncResult).Close();
			}
			return result;
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x000DEFB0 File Offset: 0x000DDFB0
		internal virtual void EndSend(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (this.Content != null)
			{
				this.Content.EndSend(asyncResult);
				return;
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
			if (lazyAsyncResult == null || lazyAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"));
			}
			if (lazyAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[]
				{
					"EndSend"
				}));
			}
			lazyAsyncResult.InternalWaitForCompletion();
			lazyAsyncResult.EndCalled = true;
			if (lazyAsyncResult.Result is Exception)
			{
				throw (Exception)lazyAsyncResult.Result;
			}
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x000DF054 File Offset: 0x000DE054
		internal virtual void Send(BaseWriter writer, bool sendEnvelope)
		{
			if (sendEnvelope)
			{
				this.PrepareEnvelopeHeaders(sendEnvelope);
				writer.WriteHeaders(this.EnvelopeHeaders);
			}
			this.PrepareHeaders(sendEnvelope);
			writer.WriteHeaders(this.Headers);
			if (this.Content != null)
			{
				this.Content.Send(writer);
				return;
			}
			writer.GetContentStream().Close();
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x000DF0AC File Offset: 0x000DE0AC
		internal void PrepareEnvelopeHeaders(bool sendEnvelope)
		{
			this.EnvelopeHeaders[MailHeaderInfo.GetString(MailHeaderID.XSender)] = this.From.ToEncodedString();
			this.EnvelopeHeaders.Remove(MailHeaderInfo.GetString(MailHeaderID.XReceiver));
			foreach (MailAddress mailAddress in this.To)
			{
				this.EnvelopeHeaders.Add(MailHeaderInfo.GetString(MailHeaderID.XReceiver), mailAddress.ToEncodedString());
			}
			foreach (MailAddress mailAddress2 in this.CC)
			{
				this.EnvelopeHeaders.Add(MailHeaderInfo.GetString(MailHeaderID.XReceiver), mailAddress2.ToEncodedString());
			}
			foreach (MailAddress mailAddress3 in this.Bcc)
			{
				this.EnvelopeHeaders.Add(MailHeaderInfo.GetString(MailHeaderID.XReceiver), mailAddress3.ToEncodedString());
			}
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x000DF1E0 File Offset: 0x000DE1E0
		internal void PrepareHeaders(bool sendEnvelope)
		{
			this.Headers[MailHeaderInfo.GetString(MailHeaderID.MimeVersion)] = "1.0";
			this.Headers[MailHeaderInfo.GetString(MailHeaderID.From)] = this.From.ToEncodedString();
			if (this.Sender != null)
			{
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.Sender)] = this.Sender.ToEncodedString();
			}
			else
			{
				this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.Sender));
			}
			if (this.To.Count > 0)
			{
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.To)] = this.To.ToEncodedString();
			}
			else
			{
				this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.To));
			}
			if (this.CC.Count > 0)
			{
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.Cc)] = this.CC.ToEncodedString();
			}
			else
			{
				this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.Cc));
			}
			if (this.replyTo != null)
			{
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.ReplyTo)] = this.ReplyTo.ToEncodedString();
			}
			if (this.priority == MailPriority.High)
			{
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.XPriority)] = "1";
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.Priority)] = "urgent";
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.Importance)] = "high";
			}
			else if (this.priority == MailPriority.Low)
			{
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.XPriority)] = "5";
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.Priority)] = "non-urgent";
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.Importance)] = "low";
			}
			else if (this.priority != (MailPriority)(-1))
			{
				this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.XPriority));
				this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.Priority));
				this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.Importance));
			}
			this.Headers[MailHeaderInfo.GetString(MailHeaderID.Date)] = MailBnfHelper.GetDateTimeString(DateTime.Now, null);
			if (this.subject != null && this.subject != string.Empty)
			{
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.Subject)] = MimeBasePart.EncodeHeaderValue(this.subject, this.subjectEncoding, MimeBasePart.ShouldUseBase64Encoding(this.subjectEncoding));
				return;
			}
			this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.Subject));
		}

		// Token: 0x04003052 RID: 12370
		private MailAddress from;

		// Token: 0x04003053 RID: 12371
		private MailAddress sender;

		// Token: 0x04003054 RID: 12372
		private MailAddress replyTo;

		// Token: 0x04003055 RID: 12373
		private MailAddressCollection to;

		// Token: 0x04003056 RID: 12374
		private MailAddressCollection cc;

		// Token: 0x04003057 RID: 12375
		private MailAddressCollection bcc;

		// Token: 0x04003058 RID: 12376
		private MimeBasePart content;

		// Token: 0x04003059 RID: 12377
		private HeaderCollection headers;

		// Token: 0x0400305A RID: 12378
		private HeaderCollection envelopeHeaders;

		// Token: 0x0400305B RID: 12379
		private string subject;

		// Token: 0x0400305C RID: 12380
		private Encoding subjectEncoding;

		// Token: 0x0400305D RID: 12381
		private MailPriority priority = (MailPriority)(-1);

		// Token: 0x020006A6 RID: 1702
		internal class EmptySendContext
		{
			// Token: 0x06003494 RID: 13460 RVA: 0x000DF455 File Offset: 0x000DE455
			internal EmptySendContext(BaseWriter writer, LazyAsyncResult result)
			{
				this.writer = writer;
				this.result = result;
			}

			// Token: 0x0400305E RID: 12382
			internal LazyAsyncResult result;

			// Token: 0x0400305F RID: 12383
			internal BaseWriter writer;
		}
	}
}

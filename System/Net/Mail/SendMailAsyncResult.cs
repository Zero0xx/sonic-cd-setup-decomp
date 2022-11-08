using System;
using System.Collections;
using System.IO;
using System.Net.Mime;

namespace System.Net.Mail
{
	// Token: 0x020006DB RID: 1755
	internal class SendMailAsyncResult : LazyAsyncResult
	{
		// Token: 0x0600361F RID: 13855 RVA: 0x000E7090 File Offset: 0x000E6090
		internal SendMailAsyncResult(SmtpConnection connection, string from, MailAddressCollection toCollection, string deliveryNotify, AsyncCallback callback, object state) : base(null, state, callback)
		{
			this.toCollection = toCollection;
			this.connection = connection;
			this.from = from;
			this.deliveryNotify = deliveryNotify;
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x000E70C5 File Offset: 0x000E60C5
		internal void Send()
		{
			this.SendMailFrom();
		}

		// Token: 0x06003621 RID: 13857 RVA: 0x000E70D0 File Offset: 0x000E60D0
		internal static MailWriter End(IAsyncResult result)
		{
			SendMailAsyncResult sendMailAsyncResult = (SendMailAsyncResult)result;
			object obj = sendMailAsyncResult.InternalWaitForCompletion();
			if (obj is Exception)
			{
				throw (Exception)obj;
			}
			return new MailWriter(sendMailAsyncResult.stream);
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x000E7108 File Offset: 0x000E6108
		private void SendMailFrom()
		{
			IAsyncResult asyncResult = MailCommand.BeginSend(this.connection, SmtpCommands.Mail, this.from, SendMailAsyncResult.sendMailFromCompleted, this);
			if (!asyncResult.CompletedSynchronously)
			{
				return;
			}
			MailCommand.EndSend(asyncResult);
			this.SendTo();
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x000E7148 File Offset: 0x000E6148
		private static void SendMailFromCompleted(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				SendMailAsyncResult sendMailAsyncResult = (SendMailAsyncResult)result.AsyncState;
				try
				{
					MailCommand.EndSend(result);
					sendMailAsyncResult.SendTo();
				}
				catch (Exception result2)
				{
					sendMailAsyncResult.InvokeCallback(result2);
				}
				catch
				{
					sendMailAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
				}
			}
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x000E71B8 File Offset: 0x000E61B8
		private void SendTo()
		{
			if (this.to == null)
			{
				if (this.SendToCollection())
				{
					this.SendData();
				}
				return;
			}
			IAsyncResult asyncResult = RecipientCommand.BeginSend(this.connection, (this.deliveryNotify != null) ? (this.to + this.deliveryNotify) : this.to, SendMailAsyncResult.sendToCompleted, this);
			if (!asyncResult.CompletedSynchronously)
			{
				return;
			}
			string serverResponse;
			if (!RecipientCommand.EndSend(asyncResult, out serverResponse))
			{
				throw new SmtpFailedRecipientException(this.connection.Reader.StatusCode, this.to, serverResponse);
			}
			this.SendData();
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x000E7248 File Offset: 0x000E6248
		private static void SendToCompleted(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				SendMailAsyncResult sendMailAsyncResult = (SendMailAsyncResult)result.AsyncState;
				try
				{
					string serverResponse;
					if (RecipientCommand.EndSend(result, out serverResponse))
					{
						sendMailAsyncResult.SendData();
					}
					else
					{
						sendMailAsyncResult.InvokeCallback(new SmtpFailedRecipientException(sendMailAsyncResult.connection.Reader.StatusCode, sendMailAsyncResult.to, serverResponse));
					}
				}
				catch (Exception result2)
				{
					sendMailAsyncResult.InvokeCallback(result2);
				}
				catch
				{
					sendMailAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
				}
			}
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x000E72E0 File Offset: 0x000E62E0
		private bool SendToCollection()
		{
			while (this.toIndex < this.toCollection.Count)
			{
				MultiAsyncResult multiAsyncResult = (MultiAsyncResult)RecipientCommand.BeginSend(this.connection, this.toCollection[this.toIndex++].SmtpAddress + this.deliveryNotify, SendMailAsyncResult.sendToCollectionCompleted, this);
				if (!multiAsyncResult.CompletedSynchronously)
				{
					return false;
				}
				string serverResponse;
				if (!RecipientCommand.EndSend(multiAsyncResult, out serverResponse))
				{
					this.failedRecipientExceptions.Add(new SmtpFailedRecipientException(this.connection.Reader.StatusCode, this.toCollection[this.toIndex - 1].SmtpAddress, serverResponse));
				}
			}
			return true;
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x000E739C File Offset: 0x000E639C
		private static void SendToCollectionCompleted(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				SendMailAsyncResult sendMailAsyncResult = (SendMailAsyncResult)result.AsyncState;
				try
				{
					string serverResponse;
					if (!RecipientCommand.EndSend(result, out serverResponse))
					{
						sendMailAsyncResult.failedRecipientExceptions.Add(new SmtpFailedRecipientException(sendMailAsyncResult.connection.Reader.StatusCode, sendMailAsyncResult.toCollection[sendMailAsyncResult.toIndex - 1].SmtpAddress, serverResponse));
						if (sendMailAsyncResult.failedRecipientExceptions.Count == sendMailAsyncResult.toCollection.Count)
						{
							SmtpFailedRecipientException ex;
							if (sendMailAsyncResult.toCollection.Count == 1)
							{
								ex = (SmtpFailedRecipientException)sendMailAsyncResult.failedRecipientExceptions[0];
							}
							else
							{
								ex = new SmtpFailedRecipientsException(sendMailAsyncResult.failedRecipientExceptions, true);
							}
							ex.fatal = true;
							sendMailAsyncResult.InvokeCallback(ex);
							return;
						}
					}
					if (sendMailAsyncResult.SendToCollection())
					{
						sendMailAsyncResult.SendData();
					}
				}
				catch (Exception result2)
				{
					sendMailAsyncResult.InvokeCallback(result2);
				}
				catch
				{
					sendMailAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
				}
			}
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x000E74B0 File Offset: 0x000E64B0
		private void SendData()
		{
			IAsyncResult asyncResult = DataCommand.BeginSend(this.connection, SendMailAsyncResult.sendDataCompleted, this);
			if (!asyncResult.CompletedSynchronously)
			{
				return;
			}
			DataCommand.EndSend(asyncResult);
			this.stream = this.connection.GetClosableStream();
			if (this.failedRecipientExceptions.Count > 1)
			{
				base.InvokeCallback(new SmtpFailedRecipientsException(this.failedRecipientExceptions, this.failedRecipientExceptions.Count == this.toCollection.Count));
				return;
			}
			if (this.failedRecipientExceptions.Count == 1)
			{
				base.InvokeCallback(this.failedRecipientExceptions[0]);
				return;
			}
			base.InvokeCallback();
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x000E7550 File Offset: 0x000E6550
		private static void SendDataCompleted(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				SendMailAsyncResult sendMailAsyncResult = (SendMailAsyncResult)result.AsyncState;
				try
				{
					DataCommand.EndSend(result);
					sendMailAsyncResult.stream = sendMailAsyncResult.connection.GetClosableStream();
					if (sendMailAsyncResult.failedRecipientExceptions.Count > 1)
					{
						sendMailAsyncResult.InvokeCallback(new SmtpFailedRecipientsException(sendMailAsyncResult.failedRecipientExceptions, sendMailAsyncResult.failedRecipientExceptions.Count == sendMailAsyncResult.toCollection.Count));
					}
					else if (sendMailAsyncResult.failedRecipientExceptions.Count == 1)
					{
						sendMailAsyncResult.InvokeCallback(sendMailAsyncResult.failedRecipientExceptions[0]);
					}
					else
					{
						sendMailAsyncResult.InvokeCallback();
					}
				}
				catch (Exception result2)
				{
					sendMailAsyncResult.InvokeCallback(result2);
				}
				catch
				{
					sendMailAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
				}
			}
		}

		// Token: 0x04003157 RID: 12631
		private SmtpConnection connection;

		// Token: 0x04003158 RID: 12632
		private string from;

		// Token: 0x04003159 RID: 12633
		private string deliveryNotify;

		// Token: 0x0400315A RID: 12634
		private static AsyncCallback sendMailFromCompleted = new AsyncCallback(SendMailAsyncResult.SendMailFromCompleted);

		// Token: 0x0400315B RID: 12635
		private static AsyncCallback sendToCompleted = new AsyncCallback(SendMailAsyncResult.SendToCompleted);

		// Token: 0x0400315C RID: 12636
		private static AsyncCallback sendToCollectionCompleted = new AsyncCallback(SendMailAsyncResult.SendToCollectionCompleted);

		// Token: 0x0400315D RID: 12637
		private static AsyncCallback sendDataCompleted = new AsyncCallback(SendMailAsyncResult.SendDataCompleted);

		// Token: 0x0400315E RID: 12638
		private ArrayList failedRecipientExceptions = new ArrayList();

		// Token: 0x0400315F RID: 12639
		private Stream stream;

		// Token: 0x04003160 RID: 12640
		private string to;

		// Token: 0x04003161 RID: 12641
		private MailAddressCollection toCollection;

		// Token: 0x04003162 RID: 12642
		private int toIndex;
	}
}

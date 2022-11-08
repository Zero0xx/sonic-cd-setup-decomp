using System;

namespace System.Net.Mail
{
	// Token: 0x020006C3 RID: 1731
	internal static class RecipientCommand
	{
		// Token: 0x06003573 RID: 13683 RVA: 0x000E3855 File Offset: 0x000E2855
		internal static IAsyncResult BeginSend(SmtpConnection conn, string to, AsyncCallback callback, object state)
		{
			RecipientCommand.PrepareCommand(conn, to);
			return CheckCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x000E3868 File Offset: 0x000E2868
		private static bool CheckResponse(SmtpStatusCode statusCode, string response)
		{
			switch (statusCode)
			{
			case SmtpStatusCode.Ok:
			case SmtpStatusCode.UserNotLocalWillForward:
				return true;
			default:
				switch (statusCode)
				{
				case SmtpStatusCode.MailboxBusy:
				case SmtpStatusCode.InsufficientStorage:
					break;
				case SmtpStatusCode.LocalErrorInProcessing:
					goto IL_50;
				default:
					switch (statusCode)
					{
					case SmtpStatusCode.MailboxUnavailable:
					case SmtpStatusCode.UserNotLocalTryAlternatePath:
					case SmtpStatusCode.ExceededStorageAllocation:
					case SmtpStatusCode.MailboxNameNotAllowed:
						break;
					default:
						goto IL_50;
					}
					break;
				}
				return false;
				IL_50:
				if (statusCode < (SmtpStatusCode)400)
				{
					throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), response);
				}
				throw new SmtpException(statusCode, response, true);
			}
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x000E38E8 File Offset: 0x000E28E8
		internal static bool EndSend(IAsyncResult result, out string response)
		{
			SmtpStatusCode statusCode = (SmtpStatusCode)CheckCommand.EndSend(result, out response);
			return RecipientCommand.CheckResponse(statusCode, response);
		}

		// Token: 0x06003576 RID: 13686 RVA: 0x000E390C File Offset: 0x000E290C
		private static void PrepareCommand(SmtpConnection conn, string to)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.Recipient);
			conn.BufferBuilder.Append(to);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x06003577 RID: 13687 RVA: 0x000E3960 File Offset: 0x000E2960
		internal static bool Send(SmtpConnection conn, string to, out string response)
		{
			RecipientCommand.PrepareCommand(conn, to);
			SmtpStatusCode statusCode = CheckCommand.Send(conn, out response);
			return RecipientCommand.CheckResponse(statusCode, response);
		}
	}
}

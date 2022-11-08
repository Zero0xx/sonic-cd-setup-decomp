using System;

namespace System.Net.Mail
{
	// Token: 0x020006C2 RID: 1730
	internal static class MailCommand
	{
		// Token: 0x0600356E RID: 13678 RVA: 0x000E374F File Offset: 0x000E274F
		internal static IAsyncResult BeginSend(SmtpConnection conn, byte[] command, string from, AsyncCallback callback, object state)
		{
			MailCommand.PrepareCommand(conn, command, from);
			return CheckCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x000E3764 File Offset: 0x000E2764
		private static void CheckResponse(SmtpStatusCode statusCode, string response)
		{
			if (statusCode == SmtpStatusCode.Ok)
			{
				return;
			}
			switch (statusCode)
			{
			case SmtpStatusCode.LocalErrorInProcessing:
			case SmtpStatusCode.InsufficientStorage:
				break;
			default:
				if (statusCode != SmtpStatusCode.ExceededStorageAllocation)
				{
				}
				break;
			}
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), response);
			}
			throw new SmtpException(statusCode, response, true);
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x000E37BC File Offset: 0x000E27BC
		internal static void EndSend(IAsyncResult result)
		{
			string response;
			SmtpStatusCode statusCode = (SmtpStatusCode)CheckCommand.EndSend(result, out response);
			MailCommand.CheckResponse(statusCode, response);
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x000E37E0 File Offset: 0x000E27E0
		private static void PrepareCommand(SmtpConnection conn, byte[] command, string from)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(command);
			conn.BufferBuilder.Append(from);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x000E3830 File Offset: 0x000E2830
		internal static void Send(SmtpConnection conn, byte[] command, string from)
		{
			MailCommand.PrepareCommand(conn, command, from);
			string response;
			SmtpStatusCode statusCode = CheckCommand.Send(conn, out response);
			MailCommand.CheckResponse(statusCode, response);
		}
	}
}

using System;

namespace System.Net.Mail
{
	// Token: 0x020006C0 RID: 1728
	internal static class HelloCommand
	{
		// Token: 0x06003564 RID: 13668 RVA: 0x000E3591 File Offset: 0x000E2591
		internal static IAsyncResult BeginSend(SmtpConnection conn, string domain, AsyncCallback callback, object state)
		{
			HelloCommand.PrepareCommand(conn, domain);
			return CheckCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x000E35A4 File Offset: 0x000E25A4
		private static void CheckResponse(SmtpStatusCode statusCode, string serverResponse)
		{
			if (statusCode == SmtpStatusCode.Ok)
			{
				return;
			}
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), serverResponse);
			}
			throw new SmtpException(statusCode, serverResponse, true);
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x000E35E0 File Offset: 0x000E25E0
		internal static void EndSend(IAsyncResult result)
		{
			string serverResponse;
			SmtpStatusCode statusCode = (SmtpStatusCode)CheckCommand.EndSend(result, out serverResponse);
			HelloCommand.CheckResponse(statusCode, serverResponse);
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x000E3604 File Offset: 0x000E2604
		private static void PrepareCommand(SmtpConnection conn, string domain)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.Hello);
			conn.BufferBuilder.Append(domain);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x000E3658 File Offset: 0x000E2658
		internal static void Send(SmtpConnection conn, string domain)
		{
			HelloCommand.PrepareCommand(conn, domain);
			string serverResponse;
			SmtpStatusCode statusCode = CheckCommand.Send(conn, out serverResponse);
			HelloCommand.CheckResponse(statusCode, serverResponse);
		}
	}
}

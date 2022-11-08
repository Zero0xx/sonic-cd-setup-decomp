using System;

namespace System.Net.Mail
{
	// Token: 0x020006C1 RID: 1729
	internal static class StartTlsCommand
	{
		// Token: 0x06003569 RID: 13673 RVA: 0x000E367C File Offset: 0x000E267C
		internal static IAsyncResult BeginSend(SmtpConnection conn, AsyncCallback callback, object state)
		{
			StartTlsCommand.PrepareCommand(conn);
			return CheckCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x000E368C File Offset: 0x000E268C
		private static void CheckResponse(SmtpStatusCode statusCode, string response)
		{
			if (statusCode == SmtpStatusCode.ServiceReady)
			{
				return;
			}
			if (statusCode != SmtpStatusCode.ClientNotPermitted)
			{
			}
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), response);
			}
			throw new SmtpException(statusCode, response, true);
		}

		// Token: 0x0600356B RID: 13675 RVA: 0x000E36D0 File Offset: 0x000E26D0
		internal static void EndSend(IAsyncResult result)
		{
			string response;
			SmtpStatusCode statusCode = (SmtpStatusCode)CheckCommand.EndSend(result, out response);
			StartTlsCommand.CheckResponse(statusCode, response);
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x000E36F2 File Offset: 0x000E26F2
		private static void PrepareCommand(SmtpConnection conn)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.StartTls);
			conn.BufferBuilder.Append(SmtpCommands.CRLF);
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x000E372C File Offset: 0x000E272C
		internal static void Send(SmtpConnection conn)
		{
			StartTlsCommand.PrepareCommand(conn);
			string response;
			SmtpStatusCode statusCode = CheckCommand.Send(conn, out response);
			StartTlsCommand.CheckResponse(statusCode, response);
		}
	}
}

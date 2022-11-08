using System;

namespace System.Net.Mail
{
	// Token: 0x020006BE RID: 1726
	internal static class DataStopCommand
	{
		// Token: 0x0600355C RID: 13660 RVA: 0x000E33A4 File Offset: 0x000E23A4
		private static void CheckResponse(SmtpStatusCode statusCode, string serverResponse)
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
				switch (statusCode)
				{
				}
				break;
			}
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), serverResponse);
			}
			throw new SmtpException(statusCode, serverResponse, true);
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x000E340B File Offset: 0x000E240B
		private static void PrepareCommand(SmtpConnection conn)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.DataStop);
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x000E3438 File Offset: 0x000E2438
		internal static void Send(SmtpConnection conn)
		{
			DataStopCommand.PrepareCommand(conn);
			string serverResponse;
			SmtpStatusCode statusCode = CheckCommand.Send(conn, out serverResponse);
			DataStopCommand.CheckResponse(statusCode, serverResponse);
		}
	}
}

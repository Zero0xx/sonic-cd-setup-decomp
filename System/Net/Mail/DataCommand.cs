using System;

namespace System.Net.Mail
{
	// Token: 0x020006BD RID: 1725
	internal static class DataCommand
	{
		// Token: 0x06003557 RID: 13655 RVA: 0x000E32D5 File Offset: 0x000E22D5
		internal static IAsyncResult BeginSend(SmtpConnection conn, AsyncCallback callback, object state)
		{
			DataCommand.PrepareCommand(conn);
			return CheckCommand.BeginSend(conn, callback, state);
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x000E32E8 File Offset: 0x000E22E8
		private static void CheckResponse(SmtpStatusCode statusCode, string serverResponse)
		{
			if (statusCode == SmtpStatusCode.StartMailInput)
			{
				return;
			}
			if (statusCode != SmtpStatusCode.LocalErrorInProcessing && statusCode != SmtpStatusCode.TransactionFailed)
			{
			}
			if (statusCode < (SmtpStatusCode)400)
			{
				throw new SmtpException(SR.GetString("net_webstatus_ServerProtocolViolation"), serverResponse);
			}
			throw new SmtpException(statusCode, serverResponse, true);
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x000E3334 File Offset: 0x000E2334
		internal static void EndSend(IAsyncResult result)
		{
			string serverResponse;
			SmtpStatusCode statusCode = (SmtpStatusCode)CheckCommand.EndSend(result, out serverResponse);
			DataCommand.CheckResponse(statusCode, serverResponse);
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x000E3356 File Offset: 0x000E2356
		private static void PrepareCommand(SmtpConnection conn)
		{
			if (conn.IsStreamOpen)
			{
				throw new InvalidOperationException(SR.GetString("SmtpDataStreamOpen"));
			}
			conn.BufferBuilder.Append(SmtpCommands.Data);
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x000E3380 File Offset: 0x000E2380
		internal static void Send(SmtpConnection conn)
		{
			DataCommand.PrepareCommand(conn);
			string serverResponse;
			SmtpStatusCode statusCode = CheckCommand.Send(conn, out serverResponse);
			DataCommand.CheckResponse(statusCode, serverResponse);
		}
	}
}

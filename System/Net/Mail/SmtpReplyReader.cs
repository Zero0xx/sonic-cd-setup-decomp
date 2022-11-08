using System;

namespace System.Net.Mail
{
	// Token: 0x020006D3 RID: 1747
	internal class SmtpReplyReader
	{
		// Token: 0x060035EC RID: 13804 RVA: 0x000E61AD File Offset: 0x000E51AD
		internal SmtpReplyReader(SmtpReplyReaderFactory reader)
		{
			this.reader = reader;
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x000E61BC File Offset: 0x000E51BC
		internal IAsyncResult BeginReadLines(AsyncCallback callback, object state)
		{
			return this.reader.BeginReadLines(this, callback, state);
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x000E61CC File Offset: 0x000E51CC
		internal IAsyncResult BeginReadLine(AsyncCallback callback, object state)
		{
			return this.reader.BeginReadLine(this, callback, state);
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x000E61DC File Offset: 0x000E51DC
		public void Close()
		{
			this.reader.Close(this);
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x000E61EA File Offset: 0x000E51EA
		internal LineInfo[] EndReadLines(IAsyncResult result)
		{
			return this.reader.EndReadLines(result);
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x000E61F8 File Offset: 0x000E51F8
		internal LineInfo EndReadLine(IAsyncResult result)
		{
			return this.reader.EndReadLine(result);
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x000E6206 File Offset: 0x000E5206
		internal LineInfo[] ReadLines()
		{
			return this.reader.ReadLines(this);
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x000E6214 File Offset: 0x000E5214
		internal LineInfo ReadLine()
		{
			return this.reader.ReadLine(this);
		}

		// Token: 0x04003111 RID: 12561
		private SmtpReplyReaderFactory reader;
	}
}

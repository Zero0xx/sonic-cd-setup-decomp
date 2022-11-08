using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x02000499 RID: 1177
	public class DownloadProgressChangedEventArgs : ProgressChangedEventArgs
	{
		// Token: 0x060023F5 RID: 9205 RVA: 0x0008CFAE File Offset: 0x0008BFAE
		internal DownloadProgressChangedEventArgs(int progressPercentage, object userToken, long bytesReceived, long totalBytesToReceive) : base(progressPercentage, userToken)
		{
			this.m_BytesReceived = bytesReceived;
			this.m_TotalBytesToReceive = totalBytesToReceive;
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x060023F6 RID: 9206 RVA: 0x0008CFC7 File Offset: 0x0008BFC7
		public long BytesReceived
		{
			get
			{
				return this.m_BytesReceived;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x060023F7 RID: 9207 RVA: 0x0008CFCF File Offset: 0x0008BFCF
		public long TotalBytesToReceive
		{
			get
			{
				return this.m_TotalBytesToReceive;
			}
		}

		// Token: 0x0400245B RID: 9307
		private long m_BytesReceived;

		// Token: 0x0400245C RID: 9308
		private long m_TotalBytesToReceive;
	}
}

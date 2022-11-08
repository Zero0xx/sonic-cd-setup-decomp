using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x0200049B RID: 1179
	public class UploadProgressChangedEventArgs : ProgressChangedEventArgs
	{
		// Token: 0x060023FC RID: 9212 RVA: 0x0008CFD7 File Offset: 0x0008BFD7
		internal UploadProgressChangedEventArgs(int progressPercentage, object userToken, long bytesSent, long totalBytesToSend, long bytesReceived, long totalBytesToReceive) : base(progressPercentage, userToken)
		{
			this.m_BytesReceived = bytesReceived;
			this.m_TotalBytesToReceive = totalBytesToReceive;
			this.m_BytesSent = bytesSent;
			this.m_TotalBytesToSend = totalBytesToSend;
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x060023FD RID: 9213 RVA: 0x0008D000 File Offset: 0x0008C000
		public long BytesReceived
		{
			get
			{
				return this.m_BytesReceived;
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x060023FE RID: 9214 RVA: 0x0008D008 File Offset: 0x0008C008
		public long TotalBytesToReceive
		{
			get
			{
				return this.m_TotalBytesToReceive;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x060023FF RID: 9215 RVA: 0x0008D010 File Offset: 0x0008C010
		public long BytesSent
		{
			get
			{
				return this.m_BytesSent;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06002400 RID: 9216 RVA: 0x0008D018 File Offset: 0x0008C018
		public long TotalBytesToSend
		{
			get
			{
				return this.m_TotalBytesToSend;
			}
		}

		// Token: 0x0400245D RID: 9309
		private long m_BytesReceived;

		// Token: 0x0400245E RID: 9310
		private long m_TotalBytesToReceive;

		// Token: 0x0400245F RID: 9311
		private long m_BytesSent;

		// Token: 0x04002460 RID: 9312
		private long m_TotalBytesToSend;
	}
}

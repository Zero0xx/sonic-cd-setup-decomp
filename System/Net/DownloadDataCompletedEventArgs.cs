using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x0200048F RID: 1167
	public class DownloadDataCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023D7 RID: 9175 RVA: 0x0008CF09 File Offset: 0x0008BF09
		internal DownloadDataCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken) : base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x060023D8 RID: 9176 RVA: 0x0008CF1C File Offset: 0x0008BF1C
		public byte[] Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04002456 RID: 9302
		private byte[] m_Result;
	}
}

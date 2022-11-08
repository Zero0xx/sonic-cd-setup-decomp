using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x0200048D RID: 1165
	public class DownloadStringCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023D1 RID: 9169 RVA: 0x0008CEE8 File Offset: 0x0008BEE8
		internal DownloadStringCompletedEventArgs(string result, Exception exception, bool cancelled, object userToken) : base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x060023D2 RID: 9170 RVA: 0x0008CEFB File Offset: 0x0008BEFB
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04002455 RID: 9301
		private string m_Result;
	}
}

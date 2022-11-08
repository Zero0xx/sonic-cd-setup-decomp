using System;

namespace System.Windows.Forms
{
	// Token: 0x020005DC RID: 1500
	public class QuestionEventArgs : EventArgs
	{
		// Token: 0x06004E58 RID: 20056 RVA: 0x001210BD File Offset: 0x001200BD
		public QuestionEventArgs()
		{
			this.response = false;
		}

		// Token: 0x06004E59 RID: 20057 RVA: 0x001210CC File Offset: 0x001200CC
		public QuestionEventArgs(bool response)
		{
			this.response = response;
		}

		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x06004E5A RID: 20058 RVA: 0x001210DB File Offset: 0x001200DB
		// (set) Token: 0x06004E5B RID: 20059 RVA: 0x001210E3 File Offset: 0x001200E3
		public bool Response
		{
			get
			{
				return this.response;
			}
			set
			{
				this.response = value;
			}
		}

		// Token: 0x040032BA RID: 12986
		private bool response;
	}
}

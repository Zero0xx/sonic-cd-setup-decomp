using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x02000491 RID: 1169
	public class UploadStringCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023DD RID: 9181 RVA: 0x0008CF2A File Offset: 0x0008BF2A
		internal UploadStringCompletedEventArgs(string result, Exception exception, bool cancelled, object userToken) : base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x0008CF3D File Offset: 0x0008BF3D
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04002457 RID: 9303
		private string m_Result;
	}
}

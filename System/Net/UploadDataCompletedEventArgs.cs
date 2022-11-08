using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x02000493 RID: 1171
	public class UploadDataCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023E3 RID: 9187 RVA: 0x0008CF4B File Offset: 0x0008BF4B
		internal UploadDataCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken) : base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060023E4 RID: 9188 RVA: 0x0008CF5E File Offset: 0x0008BF5E
		public byte[] Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04002458 RID: 9304
		private byte[] m_Result;
	}
}

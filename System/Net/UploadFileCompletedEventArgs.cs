using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x02000495 RID: 1173
	public class UploadFileCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023E9 RID: 9193 RVA: 0x0008CF6C File Offset: 0x0008BF6C
		internal UploadFileCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken) : base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060023EA RID: 9194 RVA: 0x0008CF7F File Offset: 0x0008BF7F
		public byte[] Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04002459 RID: 9305
		private byte[] m_Result;
	}
}

using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x02000497 RID: 1175
	public class UploadValuesCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023EF RID: 9199 RVA: 0x0008CF8D File Offset: 0x0008BF8D
		internal UploadValuesCompletedEventArgs(byte[] result, Exception exception, bool cancelled, object userToken) : base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x060023F0 RID: 9200 RVA: 0x0008CFA0 File Offset: 0x0008BFA0
		public byte[] Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x0400245A RID: 9306
		private byte[] m_Result;
	}
}

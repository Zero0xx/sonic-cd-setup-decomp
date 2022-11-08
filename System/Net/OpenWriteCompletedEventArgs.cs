using System;
using System.ComponentModel;
using System.IO;

namespace System.Net
{
	// Token: 0x0200048B RID: 1163
	public class OpenWriteCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023CB RID: 9163 RVA: 0x0008CEC7 File Offset: 0x0008BEC7
		internal OpenWriteCompletedEventArgs(Stream result, Exception exception, bool cancelled, object userToken) : base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x060023CC RID: 9164 RVA: 0x0008CEDA File Offset: 0x0008BEDA
		public Stream Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04002454 RID: 9300
		private Stream m_Result;
	}
}

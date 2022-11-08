using System;
using System.ComponentModel;
using System.IO;

namespace System.Net
{
	// Token: 0x02000489 RID: 1161
	public class OpenReadCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x060023C5 RID: 9157 RVA: 0x0008CEA6 File Offset: 0x0008BEA6
		internal OpenReadCompletedEventArgs(Stream result, Exception exception, bool cancelled, object userToken) : base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x060023C6 RID: 9158 RVA: 0x0008CEB9 File Offset: 0x0008BEB9
		public Stream Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04002453 RID: 9299
		private Stream m_Result;
	}
}

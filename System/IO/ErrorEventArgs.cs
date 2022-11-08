using System;

namespace System.IO
{
	// Token: 0x02000724 RID: 1828
	public class ErrorEventArgs : EventArgs
	{
		// Token: 0x060037CB RID: 14283 RVA: 0x000EBF32 File Offset: 0x000EAF32
		public ErrorEventArgs(Exception exception)
		{
			this.exception = exception;
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x000EBF41 File Offset: 0x000EAF41
		public virtual Exception GetException()
		{
			return this.exception;
		}

		// Token: 0x040031E7 RID: 12775
		private Exception exception;
	}
}

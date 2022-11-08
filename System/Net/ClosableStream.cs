using System;
using System.IO;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000683 RID: 1667
	internal class ClosableStream : DelegatedStream
	{
		// Token: 0x0600339D RID: 13213 RVA: 0x000DA057 File Offset: 0x000D9057
		internal ClosableStream(Stream stream, EventHandler onClose) : base(stream)
		{
			this.onClose = onClose;
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x000DA067 File Offset: 0x000D9067
		public override void Close()
		{
			if (Interlocked.Increment(ref this.closed) == 1 && this.onClose != null)
			{
				this.onClose(this, new EventArgs());
			}
		}

		// Token: 0x04002FA2 RID: 12194
		private EventHandler onClose;

		// Token: 0x04002FA3 RID: 12195
		private int closed;
	}
}

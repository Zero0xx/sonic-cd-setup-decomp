using System;

namespace System.IO.Ports
{
	// Token: 0x020007AA RID: 1962
	public class SerialErrorReceivedEventArgs : EventArgs
	{
		// Token: 0x06003C2B RID: 15403 RVA: 0x001012BA File Offset: 0x001002BA
		internal SerialErrorReceivedEventArgs(SerialError eventCode)
		{
			this.errorType = eventCode;
		}

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06003C2C RID: 15404 RVA: 0x001012C9 File Offset: 0x001002C9
		public SerialError EventType
		{
			get
			{
				return this.errorType;
			}
		}

		// Token: 0x04003519 RID: 13593
		private SerialError errorType;
	}
}

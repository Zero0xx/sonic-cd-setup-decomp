using System;

namespace System.IO.Ports
{
	// Token: 0x020007B1 RID: 1969
	public class SerialDataReceivedEventArgs : EventArgs
	{
		// Token: 0x06003C89 RID: 15497 RVA: 0x00102D29 File Offset: 0x00101D29
		internal SerialDataReceivedEventArgs(SerialData eventCode)
		{
			this.receiveType = eventCode;
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x06003C8A RID: 15498 RVA: 0x00102D38 File Offset: 0x00101D38
		public SerialData EventType
		{
			get
			{
				return this.receiveType;
			}
		}

		// Token: 0x04003555 RID: 13653
		internal SerialData receiveType;
	}
}

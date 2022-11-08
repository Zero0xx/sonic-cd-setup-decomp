using System;

namespace System.IO.Ports
{
	// Token: 0x020007AD RID: 1965
	public class SerialPinChangedEventArgs : EventArgs
	{
		// Token: 0x06003C31 RID: 15409 RVA: 0x001012D1 File Offset: 0x001002D1
		internal SerialPinChangedEventArgs(SerialPinChange eventCode)
		{
			this.pinChanged = eventCode;
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x06003C32 RID: 15410 RVA: 0x001012E0 File Offset: 0x001002E0
		public SerialPinChange EventType
		{
			get
			{
				return this.pinChanged;
			}
		}

		// Token: 0x04003520 RID: 13600
		private SerialPinChange pinChanged;
	}
}

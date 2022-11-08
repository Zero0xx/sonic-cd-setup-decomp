using System;

namespace System.Net.Configuration
{
	// Token: 0x02000665 RID: 1637
	internal sealed class SmtpSpecifiedPickupDirectoryElementInternal
	{
		// Token: 0x060032AB RID: 12971 RVA: 0x000D700B File Offset: 0x000D600B
		internal SmtpSpecifiedPickupDirectoryElementInternal(SmtpSpecifiedPickupDirectoryElement element)
		{
			this.pickupDirectoryLocation = element.PickupDirectoryLocation;
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x060032AC RID: 12972 RVA: 0x000D701F File Offset: 0x000D601F
		internal string PickupDirectoryLocation
		{
			get
			{
				return this.pickupDirectoryLocation;
			}
		}

		// Token: 0x04002F5D RID: 12125
		private string pickupDirectoryLocation;
	}
}

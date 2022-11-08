using System;

namespace System.ComponentModel
{
	// Token: 0x020000F5 RID: 245
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class InitializationEventAttribute : Attribute
	{
		// Token: 0x060007FD RID: 2045 RVA: 0x0001BF74 File Offset: 0x0001AF74
		public InitializationEventAttribute(string eventName)
		{
			this.eventName = eventName;
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x0001BF83 File Offset: 0x0001AF83
		public string EventName
		{
			get
			{
				return this.eventName;
			}
		}

		// Token: 0x04000980 RID: 2432
		private string eventName;
	}
}

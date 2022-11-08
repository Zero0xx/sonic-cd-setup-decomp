using System;
using System.Diagnostics;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000B5 RID: 181
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal static class CompModSwitches
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x000186DD File Offset: 0x000176DD
		public static BooleanSwitch CommonDesignerServices
		{
			get
			{
				if (CompModSwitches.commonDesignerServices == null)
				{
					CompModSwitches.commonDesignerServices = new BooleanSwitch("CommonDesignerServices", "Assert if any common designer service is not found.");
				}
				return CompModSwitches.commonDesignerServices;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x000186FF File Offset: 0x000176FF
		public static TraceSwitch EventLog
		{
			get
			{
				if (CompModSwitches.eventLog == null)
				{
					CompModSwitches.eventLog = new TraceSwitch("EventLog", "Enable tracing for the EventLog component.");
				}
				return CompModSwitches.eventLog;
			}
		}

		// Token: 0x04000915 RID: 2325
		private static BooleanSwitch commonDesignerServices;

		// Token: 0x04000916 RID: 2326
		private static TraceSwitch eventLog;
	}
}

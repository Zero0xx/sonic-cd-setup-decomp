using System;

namespace System.Diagnostics
{
	// Token: 0x02000748 RID: 1864
	public class EntryWrittenEventArgs : EventArgs
	{
		// Token: 0x060038D0 RID: 14544 RVA: 0x000F0066 File Offset: 0x000EF066
		public EntryWrittenEventArgs()
		{
		}

		// Token: 0x060038D1 RID: 14545 RVA: 0x000F006E File Offset: 0x000EF06E
		public EntryWrittenEventArgs(EventLogEntry entry)
		{
			this.entry = entry;
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x060038D2 RID: 14546 RVA: 0x000F007D File Offset: 0x000EF07D
		public EventLogEntry Entry
		{
			get
			{
				return this.entry;
			}
		}

		// Token: 0x04003262 RID: 12898
		private EventLogEntry entry;
	}
}

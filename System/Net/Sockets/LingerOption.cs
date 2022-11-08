using System;

namespace System.Net.Sockets
{
	// Token: 0x020005A9 RID: 1449
	public class LingerOption
	{
		// Token: 0x06002CBC RID: 11452 RVA: 0x000C1957 File Offset: 0x000C0957
		public LingerOption(bool enable, int seconds)
		{
			this.Enabled = enable;
			this.LingerTime = seconds;
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06002CBD RID: 11453 RVA: 0x000C196D File Offset: 0x000C096D
		// (set) Token: 0x06002CBE RID: 11454 RVA: 0x000C1975 File Offset: 0x000C0975
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				this.enabled = value;
			}
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06002CBF RID: 11455 RVA: 0x000C197E File Offset: 0x000C097E
		// (set) Token: 0x06002CC0 RID: 11456 RVA: 0x000C1986 File Offset: 0x000C0986
		public int LingerTime
		{
			get
			{
				return this.lingerTime;
			}
			set
			{
				this.lingerTime = value;
			}
		}

		// Token: 0x04002AC2 RID: 10946
		private bool enabled;

		// Token: 0x04002AC3 RID: 10947
		private int lingerTime;
	}
}

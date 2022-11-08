using System;

namespace System.Timers
{
	// Token: 0x02000732 RID: 1842
	public class ElapsedEventArgs : EventArgs
	{
		// Token: 0x0600382C RID: 14380 RVA: 0x000ED324 File Offset: 0x000EC324
		internal ElapsedEventArgs(int low, int high)
		{
			long fileTime = (long)high << 32 | ((long)low & (long)((ulong)-1));
			this.signalTime = DateTime.FromFileTime(fileTime);
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x0600382D RID: 14381 RVA: 0x000ED34F File Offset: 0x000EC34F
		public DateTime SignalTime
		{
			get
			{
				return this.signalTime;
			}
		}

		// Token: 0x04003224 RID: 12836
		private DateTime signalTime;
	}
}

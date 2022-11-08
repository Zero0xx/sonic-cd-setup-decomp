using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000610 RID: 1552
	public class NetworkAvailabilityEventArgs : EventArgs
	{
		// Token: 0x06002FE6 RID: 12262 RVA: 0x000CF19F File Offset: 0x000CE19F
		internal NetworkAvailabilityEventArgs(bool isAvailable)
		{
			this.isAvailable = isAvailable;
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06002FE7 RID: 12263 RVA: 0x000CF1AE File Offset: 0x000CE1AE
		public bool IsAvailable
		{
			get
			{
				return this.isAvailable;
			}
		}

		// Token: 0x04002DBE RID: 11710
		private bool isAvailable;
	}
}

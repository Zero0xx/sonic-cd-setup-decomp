using System;

namespace System.Configuration
{
	// Token: 0x020006E4 RID: 1764
	public class SettingsLoadedEventArgs : EventArgs
	{
		// Token: 0x0600367B RID: 13947 RVA: 0x000E89F3 File Offset: 0x000E79F3
		public SettingsLoadedEventArgs(SettingsProvider provider)
		{
			this._provider = provider;
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x0600367C RID: 13948 RVA: 0x000E8A02 File Offset: 0x000E7A02
		public SettingsProvider Provider
		{
			get
			{
				return this._provider;
			}
		}

		// Token: 0x04003181 RID: 12673
		private SettingsProvider _provider;
	}
}

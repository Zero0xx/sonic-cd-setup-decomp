using System;
using System.ComponentModel;

namespace System.Configuration
{
	// Token: 0x020006E3 RID: 1763
	public class SettingChangingEventArgs : CancelEventArgs
	{
		// Token: 0x06003676 RID: 13942 RVA: 0x000E89AC File Offset: 0x000E79AC
		public SettingChangingEventArgs(string settingName, string settingClass, string settingKey, object newValue, bool cancel) : base(cancel)
		{
			this._settingName = settingName;
			this._settingClass = settingClass;
			this._settingKey = settingKey;
			this._newValue = newValue;
		}

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x06003677 RID: 13943 RVA: 0x000E89D3 File Offset: 0x000E79D3
		public object NewValue
		{
			get
			{
				return this._newValue;
			}
		}

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x06003678 RID: 13944 RVA: 0x000E89DB File Offset: 0x000E79DB
		public string SettingClass
		{
			get
			{
				return this._settingClass;
			}
		}

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06003679 RID: 13945 RVA: 0x000E89E3 File Offset: 0x000E79E3
		public string SettingName
		{
			get
			{
				return this._settingName;
			}
		}

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x0600367A RID: 13946 RVA: 0x000E89EB File Offset: 0x000E79EB
		public string SettingKey
		{
			get
			{
				return this._settingKey;
			}
		}

		// Token: 0x0400317D RID: 12669
		private string _settingClass;

		// Token: 0x0400317E RID: 12670
		private string _settingName;

		// Token: 0x0400317F RID: 12671
		private string _settingKey;

		// Token: 0x04003180 RID: 12672
		private object _newValue;
	}
}

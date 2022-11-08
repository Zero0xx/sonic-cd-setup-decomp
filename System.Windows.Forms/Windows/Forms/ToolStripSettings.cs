using System;
using System.Configuration;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020006DE RID: 1758
	internal partial class ToolStripSettings : ApplicationSettingsBase
	{
		// Token: 0x06005CC6 RID: 23750 RVA: 0x00150EB2 File Offset: 0x0014FEB2
		internal ToolStripSettings(string settingsKey) : base(settingsKey)
		{
		}

		// Token: 0x17001377 RID: 4983
		// (get) Token: 0x06005CC9 RID: 23753 RVA: 0x00150EE0 File Offset: 0x0014FEE0
		// (set) Token: 0x06005CCA RID: 23754 RVA: 0x00150EF2 File Offset: 0x0014FEF2
		[UserScopedSetting]
		public string ItemOrder
		{
			get
			{
				return this["ItemOrder"] as string;
			}
			set
			{
				this["ItemOrder"] = value;
			}
		}

		// Token: 0x17001378 RID: 4984
		// (get) Token: 0x06005CCB RID: 23755 RVA: 0x00150F00 File Offset: 0x0014FF00
		// (set) Token: 0x06005CCC RID: 23756 RVA: 0x00150F12 File Offset: 0x0014FF12
		[UserScopedSetting]
		public string Name
		{
			get
			{
				return this["Name"] as string;
			}
			set
			{
				this["Name"] = value;
			}
		}

		// Token: 0x1700137B RID: 4987
		// (get) Token: 0x06005CD1 RID: 23761 RVA: 0x00150F6A File Offset: 0x0014FF6A
		// (set) Token: 0x06005CD2 RID: 23762 RVA: 0x00150F7C File Offset: 0x0014FF7C
		[UserScopedSetting]
		public string ToolStripPanelName
		{
			get
			{
				return this["ToolStripPanelName"] as string;
			}
			set
			{
				this["ToolStripPanelName"] = value;
			}
		}

		// Token: 0x06005CD5 RID: 23765 RVA: 0x00150FAF File Offset: 0x0014FFAF
		public override void Save()
		{
			this.IsDefault = false;
			base.Save();
		}
	}
}

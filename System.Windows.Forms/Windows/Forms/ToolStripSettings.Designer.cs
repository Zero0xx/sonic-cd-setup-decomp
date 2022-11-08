using System;
using System.Configuration;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020006DE RID: 1758
	internal partial class ToolStripSettings : ApplicationSettingsBase
	{
		// Token: 0x17001376 RID: 4982
		// (get) Token: 0x06005CC7 RID: 23751 RVA: 0x00150EBB File Offset: 0x0014FEBB
		// (set) Token: 0x06005CC8 RID: 23752 RVA: 0x00150ECD File Offset: 0x0014FECD
		[UserScopedSetting]
		[DefaultSettingValue("true")]
		public bool IsDefault
		{
			get
			{
				return (bool)this["IsDefault"];
			}
			set
			{
				this["IsDefault"] = value;
			}
		}

		// Token: 0x17001379 RID: 4985
		// (get) Token: 0x06005CCD RID: 23757 RVA: 0x00150F20 File Offset: 0x0014FF20
		// (set) Token: 0x06005CCE RID: 23758 RVA: 0x00150F32 File Offset: 0x0014FF32
		[UserScopedSetting]
		[DefaultSettingValue("0,0")]
		public Point Location
		{
			get
			{
				return (Point)this["Location"];
			}
			set
			{
				this["Location"] = value;
			}
		}

		// Token: 0x1700137A RID: 4986
		// (get) Token: 0x06005CCF RID: 23759 RVA: 0x00150F45 File Offset: 0x0014FF45
		// (set) Token: 0x06005CD0 RID: 23760 RVA: 0x00150F57 File Offset: 0x0014FF57
		[UserScopedSetting]
		[DefaultSettingValue("0,0")]
		public Size Size
		{
			get
			{
				return (Size)this["Size"];
			}
			set
			{
				this["Size"] = value;
			}
		}

		// Token: 0x1700137C RID: 4988
		// (get) Token: 0x06005CD3 RID: 23763 RVA: 0x00150F8A File Offset: 0x0014FF8A
		// (set) Token: 0x06005CD4 RID: 23764 RVA: 0x00150F9C File Offset: 0x0014FF9C
		[UserScopedSetting]
		[DefaultSettingValue("true")]
		public bool Visible
		{
			get
			{
				return (bool)this["Visible"];
			}
			set
			{
				this["Visible"] = value;
			}
		}
	}
}

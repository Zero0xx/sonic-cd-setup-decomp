using System;

namespace System.Windows.Forms
{
	// Token: 0x020005B8 RID: 1464
	public class PowerStatus
	{
		// Token: 0x06004BE4 RID: 19428 RVA: 0x001124E4 File Offset: 0x001114E4
		internal PowerStatus()
		{
		}

		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x06004BE5 RID: 19429 RVA: 0x001124EC File Offset: 0x001114EC
		public PowerLineStatus PowerLineStatus
		{
			get
			{
				this.UpdateSystemPowerStatus();
				return (PowerLineStatus)this.systemPowerStatus.ACLineStatus;
			}
		}

		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x06004BE6 RID: 19430 RVA: 0x001124FF File Offset: 0x001114FF
		public BatteryChargeStatus BatteryChargeStatus
		{
			get
			{
				this.UpdateSystemPowerStatus();
				return (BatteryChargeStatus)this.systemPowerStatus.BatteryFlag;
			}
		}

		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x06004BE7 RID: 19431 RVA: 0x00112512 File Offset: 0x00111512
		public int BatteryFullLifetime
		{
			get
			{
				this.UpdateSystemPowerStatus();
				return this.systemPowerStatus.BatteryFullLifeTime;
			}
		}

		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x06004BE8 RID: 19432 RVA: 0x00112528 File Offset: 0x00111528
		public float BatteryLifePercent
		{
			get
			{
				this.UpdateSystemPowerStatus();
				float num = (float)this.systemPowerStatus.BatteryLifePercent / 100f;
				if (num <= 1f)
				{
					return num;
				}
				return 1f;
			}
		}

		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x06004BE9 RID: 19433 RVA: 0x0011255D File Offset: 0x0011155D
		public int BatteryLifeRemaining
		{
			get
			{
				this.UpdateSystemPowerStatus();
				return this.systemPowerStatus.BatteryLifeTime;
			}
		}

		// Token: 0x06004BEA RID: 19434 RVA: 0x00112570 File Offset: 0x00111570
		private void UpdateSystemPowerStatus()
		{
			UnsafeNativeMethods.GetSystemPowerStatus(ref this.systemPowerStatus);
		}

		// Token: 0x0400313F RID: 12607
		private NativeMethods.SYSTEM_POWER_STATUS systemPowerStatus;
	}
}

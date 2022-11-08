using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x02000740 RID: 1856
	[TypeConverter("System.Diagnostics.Design.CounterCreationDataConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Serializable]
	public class CounterCreationData
	{
		// Token: 0x06003892 RID: 14482 RVA: 0x000EEF84 File Offset: 0x000EDF84
		public CounterCreationData()
		{
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x000EEFAD File Offset: 0x000EDFAD
		public CounterCreationData(string counterName, string counterHelp, PerformanceCounterType counterType)
		{
			this.CounterType = counterType;
			this.CounterName = counterName;
			this.CounterHelp = counterHelp;
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x06003894 RID: 14484 RVA: 0x000EEFEB File Offset: 0x000EDFEB
		// (set) Token: 0x06003895 RID: 14485 RVA: 0x000EEFF3 File Offset: 0x000EDFF3
		[MonitoringDescription("CounterType")]
		[DefaultValue(PerformanceCounterType.NumberOfItems32)]
		public PerformanceCounterType CounterType
		{
			get
			{
				return this.counterType;
			}
			set
			{
				if (!Enum.IsDefined(typeof(PerformanceCounterType), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(PerformanceCounterType));
				}
				this.counterType = value;
			}
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x06003896 RID: 14486 RVA: 0x000EF029 File Offset: 0x000EE029
		// (set) Token: 0x06003897 RID: 14487 RVA: 0x000EF031 File Offset: 0x000EE031
		[DefaultValue("")]
		[MonitoringDescription("CounterName")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string CounterName
		{
			get
			{
				return this.counterName;
			}
			set
			{
				PerformanceCounterCategory.CheckValidCounter(value);
				this.counterName = value;
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x06003898 RID: 14488 RVA: 0x000EF040 File Offset: 0x000EE040
		// (set) Token: 0x06003899 RID: 14489 RVA: 0x000EF048 File Offset: 0x000EE048
		[DefaultValue("")]
		[MonitoringDescription("CounterHelp")]
		public string CounterHelp
		{
			get
			{
				return this.counterHelp;
			}
			set
			{
				PerformanceCounterCategory.CheckValidHelp(value);
				this.counterHelp = value;
			}
		}

		// Token: 0x04003254 RID: 12884
		private PerformanceCounterType counterType = PerformanceCounterType.NumberOfItems32;

		// Token: 0x04003255 RID: 12885
		private string counterName = string.Empty;

		// Token: 0x04003256 RID: 12886
		private string counterHelp = string.Empty;
	}
}

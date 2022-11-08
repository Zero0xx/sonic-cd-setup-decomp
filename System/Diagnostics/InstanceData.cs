using System;

namespace System.Diagnostics
{
	// Token: 0x0200075C RID: 1884
	public class InstanceData
	{
		// Token: 0x060039DF RID: 14815 RVA: 0x000F5100 File Offset: 0x000F4100
		public InstanceData(string instanceName, CounterSample sample)
		{
			this.instanceName = instanceName;
			this.sample = sample;
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x060039E0 RID: 14816 RVA: 0x000F5116 File Offset: 0x000F4116
		public string InstanceName
		{
			get
			{
				return this.instanceName;
			}
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x060039E1 RID: 14817 RVA: 0x000F511E File Offset: 0x000F411E
		public CounterSample Sample
		{
			get
			{
				return this.sample;
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x060039E2 RID: 14818 RVA: 0x000F5126 File Offset: 0x000F4126
		public long RawValue
		{
			get
			{
				return this.sample.RawValue;
			}
		}

		// Token: 0x040032DE RID: 13022
		private string instanceName;

		// Token: 0x040032DF RID: 13023
		private CounterSample sample;
	}
}

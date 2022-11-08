using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000ED RID: 237
	public sealed class SamplerStateManagerCollection
	{
		// Token: 0x17000258 RID: 600
		[IndexerName("SamplerState")]
		public SamplerStateManager this[int index]
		{
			get
			{
				Debug.Assert(this.samplerList != null, "There is no valid array created.");
				return this.samplerList[index];
			}
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00071B54 File Offset: 0x00070F54
		internal SamplerStateManagerCollection(Device dev)
		{
			this.samplerList = new SamplerStateManager[16];
			int num = 0;
			do
			{
				this.samplerList[num] = new SamplerStateManager(dev, num);
				num++;
			}
			while (num < 16);
		}

		// Token: 0x04001005 RID: 4101
		private SamplerStateManager[] samplerList;
	}
}

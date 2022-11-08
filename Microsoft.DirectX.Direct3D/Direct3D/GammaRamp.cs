using System;
using Microsoft.DirectX.PrivateImplementationDetails;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000B3 RID: 179
	[MiscellaneousBits(1)]
	public struct GammaRamp
	{
		// Token: 0x0600028A RID: 650 RVA: 0x0005DBCC File Offset: 0x0005CFCC
		public short[] GetRed()
		{
			short[] array = new short[256];
			array.Initialize();
			ref short int16& = ref array[0];
			ref ushort uint16& = ref this.m_Internal;
			cpblk(ref int16&, ref uint16&, 512);
			return array;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0005DC08 File Offset: 0x0005D008
		public short[] GetBlue()
		{
			short[] array = new short[256];
			array.Initialize();
			ref short int16& = ref array[0];
			ref ushort uint16& = ref this.m_Internal + 1024;
			cpblk(ref int16&, ref uint16&, 512);
			return array;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0005DC4C File Offset: 0x0005D04C
		public short[] GetGreen()
		{
			short[] array = new short[256];
			array.Initialize();
			ref short int16& = ref array[0];
			ref ushort uint16& = ref this.m_Internal + 512;
			cpblk(ref int16&, ref uint16&, 512);
			return array;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0005DC90 File Offset: 0x0005D090
		public void SetRed(short[] value)
		{
			if (value.Length != 256)
			{
				throw new ArgumentException(string.Empty, "value");
			}
			ref short int16& = ref value[0];
			ref ushort uint16& = ref this.m_Internal;
			cpblk(ref uint16&, ref int16&, 512);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0005DCD4 File Offset: 0x0005D0D4
		public void SetBlue(short[] value)
		{
			if (value.Length != 256)
			{
				throw new ArgumentException(string.Empty, "value");
			}
			ref short int16& = ref value[0];
			ref ushort uint16& = ref this.m_Internal + 1024;
			cpblk(ref uint16&, ref int16&, 512);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0005DD20 File Offset: 0x0005D120
		public void SetGreen(short[] value)
		{
			if (value.Length != 256)
			{
				throw new ArgumentException(string.Empty, "value");
			}
			ref short int16& = ref value[0];
			ref ushort uint16& = ref this.m_Internal + 512;
			cpblk(ref uint16&, ref int16&, 512);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0005DD6C File Offset: 0x0005D16C
		public GammaRamp()
		{
			initblk(ref this.m_Internal, 0, 1536);
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0005DD90 File Offset: 0x0005D190
		internal unsafe _D3DGAMMARAMP* Handle
		{
			get
			{
				return &this.m_Internal;
			}
		}

		// Token: 0x04000EFA RID: 3834
		internal _D3DGAMMARAMP m_Internal;
	}
}

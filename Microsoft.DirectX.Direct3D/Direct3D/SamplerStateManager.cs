using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000EC RID: 236
	public sealed class SamplerStateManager
	{
		// Token: 0x060004A6 RID: 1190 RVA: 0x00061E8C File Offset: 0x0006128C
		public unsafe override string ToString()
		{
			Type type = base.GetType();
			StringBuilder stringBuilder = new StringBuilder();
			PropertyInfo[] properties = type.GetProperties();
			int num = 0;
			if (0 < properties.Length)
			{
				do
				{
					MethodInfo getMethod = properties[num].GetGetMethod();
					if (getMethod != null)
					{
						object obj = getMethod.Invoke(this, null);
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$124$];
						array[0] = properties[num].Name;
						string text;
						if (obj != null)
						{
							text = obj.ToString();
						}
						else
						{
							text = string.Empty;
						}
						array[1] = text;
						stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array));
					}
					num++;
				}
				while (num < properties.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00071708 File Offset: 0x00070B08
		// (set) Token: 0x060004A8 RID: 1192 RVA: 0x0007172C File Offset: 0x00070B2C
		public TextureAddress AddressU
		{
			get
			{
				return (TextureAddress)this.pDevice.GetSamplerStageStateInt32(this.currentStageState, SamplerStageStates.AddressU);
			}
			set
			{
				this.pDevice.SetSamplerState(this.currentStageState, SamplerStageStates.AddressU, (int)value);
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x00071754 File Offset: 0x00070B54
		// (set) Token: 0x060004AA RID: 1194 RVA: 0x00071778 File Offset: 0x00070B78
		public TextureAddress AddressV
		{
			get
			{
				return (TextureAddress)this.pDevice.GetSamplerStageStateInt32(this.currentStageState, SamplerStageStates.AddressV);
			}
			set
			{
				this.pDevice.SetSamplerState(this.currentStageState, SamplerStageStates.AddressV, (int)value);
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x000717A0 File Offset: 0x00070BA0
		// (set) Token: 0x060004AC RID: 1196 RVA: 0x000717C4 File Offset: 0x00070BC4
		public TextureAddress AddressW
		{
			get
			{
				return (TextureAddress)this.pDevice.GetSamplerStageStateInt32(this.currentStageState, SamplerStageStates.AddressW);
			}
			set
			{
				this.pDevice.SetSamplerState(this.currentStageState, SamplerStageStates.AddressW, (int)value);
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x000717EC File Offset: 0x00070BEC
		// (set) Token: 0x060004AE RID: 1198 RVA: 0x0007181C File Offset: 0x00070C1C
		public Color BorderColor
		{
			get
			{
				return Color.FromArgb(this.pDevice.GetSamplerStageStateInt32(this.currentStageState, SamplerStageStates.BorderColor));
			}
			set
			{
				this.pDevice.SetSamplerState(this.currentStageState, SamplerStageStates.BorderColor, value.ToArgb());
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x00071848 File Offset: 0x00070C48
		// (set) Token: 0x060004B0 RID: 1200 RVA: 0x0007186C File Offset: 0x00070C6C
		public int BorderColorValue
		{
			get
			{
				return this.pDevice.GetSamplerStageStateInt32(this.currentStageState, SamplerStageStates.BorderColor);
			}
			set
			{
				this.pDevice.SetSamplerState(this.currentStageState, SamplerStageStates.BorderColor, value);
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00071894 File Offset: 0x00070C94
		// (set) Token: 0x060004B2 RID: 1202 RVA: 0x000718B8 File Offset: 0x00070CB8
		public TextureFilter MagFilter
		{
			get
			{
				return (TextureFilter)this.pDevice.GetSamplerStageStateInt32(this.currentStageState, SamplerStageStates.MagFilter);
			}
			set
			{
				this.pDevice.SetSamplerState(this.currentStageState, SamplerStageStates.MagFilter, (int)value);
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x000718E0 File Offset: 0x00070CE0
		// (set) Token: 0x060004B4 RID: 1204 RVA: 0x00071904 File Offset: 0x00070D04
		public TextureFilter MinFilter
		{
			get
			{
				return (TextureFilter)this.pDevice.GetSamplerStageStateInt32(this.currentStageState, SamplerStageStates.MinFilter);
			}
			set
			{
				this.pDevice.SetSamplerState(this.currentStageState, SamplerStageStates.MinFilter, (int)value);
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0007192C File Offset: 0x00070D2C
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x00071950 File Offset: 0x00070D50
		public TextureFilter MipFilter
		{
			get
			{
				return (TextureFilter)this.pDevice.GetSamplerStageStateInt32(this.currentStageState, SamplerStageStates.MipFilter);
			}
			set
			{
				this.pDevice.SetSamplerState(this.currentStageState, SamplerStageStates.MipFilter, (int)value);
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00071978 File Offset: 0x00070D78
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x0007199C File Offset: 0x00070D9C
		public float MipMapLevelOfDetailBias
		{
			get
			{
				return this.pDevice.GetSamplerStageStateSingle(this.currentStageState, SamplerStageStates.MipMapLevelOfDetailBias);
			}
			set
			{
				this.pDevice.SetSamplerState(this.currentStageState, SamplerStageStates.MipMapLevelOfDetailBias, value);
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x000719C4 File Offset: 0x00070DC4
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x000719EC File Offset: 0x00070DEC
		public int MaxMipLevel
		{
			get
			{
				return this.pDevice.GetSamplerStageStateInt32(this.currentStageState, SamplerStageStates.MaxMipLevel);
			}
			set
			{
				this.pDevice.SetSamplerState(this.currentStageState, SamplerStageStates.MaxMipLevel, value);
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x00071A14 File Offset: 0x00070E14
		// (set) Token: 0x060004BC RID: 1212 RVA: 0x00071A3C File Offset: 0x00070E3C
		public int MaxAnisotropy
		{
			get
			{
				return this.pDevice.GetSamplerStageStateInt32(this.currentStageState, SamplerStageStates.MaxAnisotropy);
			}
			set
			{
				this.pDevice.SetSamplerState(this.currentStageState, SamplerStageStates.MaxAnisotropy, value);
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00071A64 File Offset: 0x00070E64
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x00071A8C File Offset: 0x00070E8C
		public bool SrgbTexture
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetSamplerStageStateBoolean(this.currentStageState, SamplerStageStates.SrgbTexture);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetSamplerState(this.currentStageState, SamplerStageStates.SrgbTexture, value);
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x00071AB4 File Offset: 0x00070EB4
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x00071ADC File Offset: 0x00070EDC
		public int ElementIndex
		{
			get
			{
				return this.pDevice.GetSamplerStageStateInt32(this.currentStageState, SamplerStageStates.ElementIndex);
			}
			set
			{
				this.pDevice.SetSamplerState(this.currentStageState, SamplerStageStates.ElementIndex, value);
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00071B04 File Offset: 0x00070F04
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x00071B2C File Offset: 0x00070F2C
		public int DMapOffset
		{
			get
			{
				return this.pDevice.GetSamplerStageStateInt32(this.currentStageState, SamplerStageStates.DMapOffset);
			}
			set
			{
				this.pDevice.SetSamplerState(this.currentStageState, SamplerStageStates.DMapOffset, value);
			}
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x000716E0 File Offset: 0x00070AE0
		internal SamplerStateManager(Device dev, int stage)
		{
			this.pDevice = dev;
			this.currentStageState = stage;
		}

		// Token: 0x04001003 RID: 4099
		internal int currentStageState;

		// Token: 0x04001004 RID: 4100
		internal Device pDevice;
	}
}

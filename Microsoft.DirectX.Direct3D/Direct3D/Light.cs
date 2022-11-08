using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000F2 RID: 242
	public sealed class Light
	{
		// Token: 0x060004F2 RID: 1266 RVA: 0x00071CF0 File Offset: 0x000710F0
		[CLSCompliant(false)]
		public unsafe Light(_D3DLIGHT9* pLight)
		{
			this.pDevice = null;
			this.index = -1;
			cpblk(ref this.storedD3DLight, pLight, 104);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00071C5C File Offset: 0x0007105C
		public unsafe Light()
		{
			this.pDevice = null;
			this.index = -1;
			initblk(ref this.storedD3DLight, 0, 104);
			this.storedD3DLight = 3;
			*(ref this.storedD3DLight + 12) = 1f;
			*(ref this.storedD3DLight + 8) = 1f;
			*(ref this.storedD3DLight + 4) = 1f;
			*(ref this.storedD3DLight + 68) = 0f;
			*(ref this.storedD3DLight + 64) = 0f;
			*(ref this.storedD3DLight + 72) = 1f;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00071BC8 File Offset: 0x00070FC8
		internal unsafe Light(Device dev, int i)
		{
			this.pDevice = dev;
			this.index = i;
			initblk(ref this.storedD3DLight, 0, 104);
			this.storedD3DLight = 3;
			*(ref this.storedD3DLight + 12) = 1f;
			*(ref this.storedD3DLight + 8) = 1f;
			*(ref this.storedD3DLight + 4) = 1f;
			*(ref this.storedD3DLight + 68) = 0f;
			*(ref this.storedD3DLight + 64) = 0f;
			*(ref this.storedD3DLight + 72) = 1f;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00062004 File Offset: 0x00061404
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$126$];
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

		// Token: 0x060004F6 RID: 1270 RVA: 0x00071D24 File Offset: 0x00071124
		public void FromLight(Light light)
		{
			cpblk(ref this.storedD3DLight, ref light.storedD3DLight, 104);
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x00071D4C File Offset: 0x0007114C
		// (set) Token: 0x060004F8 RID: 1272 RVA: 0x00071D68 File Offset: 0x00071168
		public LightType Type
		{
			get
			{
				return this.storedD3DLight;
			}
			set
			{
				this.storedD3DLight = value;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00071D84 File Offset: 0x00071184
		// (set) Token: 0x060004FA RID: 1274 RVA: 0x00071EC8 File Offset: 0x000712C8
		public unsafe Color Diffuse
		{
			get
			{
				Microsoft.DirectX.PrivateImplementationDetails.D3DXCOLOR d3DXCOLOR = *(ref this.storedD3DLight + 4);
				*(ref d3DXCOLOR + 4) = *(ref this.storedD3DLight + 8);
				*(ref d3DXCOLOR + 8) = *(ref this.storedD3DLight + 12);
				*(ref d3DXCOLOR + 12) = *(ref this.storedD3DLight + 16);
				uint num;
				if (d3DXCOLOR >= 1f)
				{
					num = 255U;
				}
				else if (d3DXCOLOR <= 0f)
				{
					num = 0U;
				}
				else
				{
					num = (uint)(d3DXCOLOR * 255f + 0.5f);
				}
				uint num2;
				if (*(ref d3DXCOLOR + 4) >= 1f)
				{
					num2 = 255U;
				}
				else if (*(ref d3DXCOLOR + 4) <= 0f)
				{
					num2 = 0U;
				}
				else
				{
					num2 = (uint)((double)(*(ref d3DXCOLOR + 4) * 255f + 0.5f));
				}
				uint num3;
				if (*(ref d3DXCOLOR + 8) >= 1f)
				{
					num3 = 255U;
				}
				else if (*(ref d3DXCOLOR + 8) <= 0f)
				{
					num3 = 0U;
				}
				else
				{
					num3 = (uint)((double)(*(ref d3DXCOLOR + 8) * 255f + 0.5f));
				}
				uint num4;
				if (*(ref d3DXCOLOR + 12) >= 1f)
				{
					num4 = 255U;
				}
				else if (*(ref d3DXCOLOR + 12) <= 0f)
				{
					num4 = 0U;
				}
				else
				{
					num4 = (uint)((double)(*(ref d3DXCOLOR + 12) * 255f + 0.5f));
				}
				return Color.FromArgb((int)(((num4 << 8 | num) << 8 | num2) << 8 | num3));
			}
			set
			{
				*(ref this.storedD3DLight + 16) = (float)value.A * 0.003921569f;
				*(ref this.storedD3DLight + 4) = (float)value.R * 0.003921569f;
				*(ref this.storedD3DLight + 8) = (float)value.G * 0.003921569f;
				*(ref this.storedD3DLight + 12) = (float)value.B * 0.003921569f;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x00071F38 File Offset: 0x00071338
		// (set) Token: 0x060004FC RID: 1276 RVA: 0x0007207C File Offset: 0x0007147C
		public unsafe Color Specular
		{
			get
			{
				Microsoft.DirectX.PrivateImplementationDetails.D3DXCOLOR d3DXCOLOR = *(ref this.storedD3DLight + 20);
				*(ref d3DXCOLOR + 4) = *(ref this.storedD3DLight + 24);
				*(ref d3DXCOLOR + 8) = *(ref this.storedD3DLight + 28);
				*(ref d3DXCOLOR + 12) = *(ref this.storedD3DLight + 32);
				uint num;
				if (d3DXCOLOR >= 1f)
				{
					num = 255U;
				}
				else if (d3DXCOLOR <= 0f)
				{
					num = 0U;
				}
				else
				{
					num = (uint)(d3DXCOLOR * 255f + 0.5f);
				}
				uint num2;
				if (*(ref d3DXCOLOR + 4) >= 1f)
				{
					num2 = 255U;
				}
				else if (*(ref d3DXCOLOR + 4) <= 0f)
				{
					num2 = 0U;
				}
				else
				{
					num2 = (uint)((double)(*(ref d3DXCOLOR + 4) * 255f + 0.5f));
				}
				uint num3;
				if (*(ref d3DXCOLOR + 8) >= 1f)
				{
					num3 = 255U;
				}
				else if (*(ref d3DXCOLOR + 8) <= 0f)
				{
					num3 = 0U;
				}
				else
				{
					num3 = (uint)((double)(*(ref d3DXCOLOR + 8) * 255f + 0.5f));
				}
				uint num4;
				if (*(ref d3DXCOLOR + 12) >= 1f)
				{
					num4 = 255U;
				}
				else if (*(ref d3DXCOLOR + 12) <= 0f)
				{
					num4 = 0U;
				}
				else
				{
					num4 = (uint)((double)(*(ref d3DXCOLOR + 12) * 255f + 0.5f));
				}
				return Color.FromArgb((int)(((num4 << 8 | num) << 8 | num2) << 8 | num3));
			}
			set
			{
				*(ref this.storedD3DLight + 32) = (float)value.A * 0.003921569f;
				*(ref this.storedD3DLight + 20) = (float)value.R * 0.003921569f;
				*(ref this.storedD3DLight + 24) = (float)value.G * 0.003921569f;
				*(ref this.storedD3DLight + 28) = (float)value.B * 0.003921569f;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x000720F0 File Offset: 0x000714F0
		// (set) Token: 0x060004FE RID: 1278 RVA: 0x00072234 File Offset: 0x00071634
		public unsafe Color Ambient
		{
			get
			{
				Microsoft.DirectX.PrivateImplementationDetails.D3DXCOLOR d3DXCOLOR = *(ref this.storedD3DLight + 36);
				*(ref d3DXCOLOR + 4) = *(ref this.storedD3DLight + 40);
				*(ref d3DXCOLOR + 8) = *(ref this.storedD3DLight + 44);
				*(ref d3DXCOLOR + 12) = *(ref this.storedD3DLight + 48);
				uint num;
				if (d3DXCOLOR >= 1f)
				{
					num = 255U;
				}
				else if (d3DXCOLOR <= 0f)
				{
					num = 0U;
				}
				else
				{
					num = (uint)(d3DXCOLOR * 255f + 0.5f);
				}
				uint num2;
				if (*(ref d3DXCOLOR + 4) >= 1f)
				{
					num2 = 255U;
				}
				else if (*(ref d3DXCOLOR + 4) <= 0f)
				{
					num2 = 0U;
				}
				else
				{
					num2 = (uint)((double)(*(ref d3DXCOLOR + 4) * 255f + 0.5f));
				}
				uint num3;
				if (*(ref d3DXCOLOR + 8) >= 1f)
				{
					num3 = 255U;
				}
				else if (*(ref d3DXCOLOR + 8) <= 0f)
				{
					num3 = 0U;
				}
				else
				{
					num3 = (uint)((double)(*(ref d3DXCOLOR + 8) * 255f + 0.5f));
				}
				uint num4;
				if (*(ref d3DXCOLOR + 12) >= 1f)
				{
					num4 = 255U;
				}
				else if (*(ref d3DXCOLOR + 12) <= 0f)
				{
					num4 = 0U;
				}
				else
				{
					num4 = (uint)((double)(*(ref d3DXCOLOR + 12) * 255f + 0.5f));
				}
				return Color.FromArgb((int)(((num4 << 8 | num) << 8 | num2) << 8 | num3));
			}
			set
			{
				*(ref this.storedD3DLight + 48) = (float)value.A * 0.003921569f;
				*(ref this.storedD3DLight + 36) = (float)value.R * 0.003921569f;
				*(ref this.storedD3DLight + 40) = (float)value.G * 0.003921569f;
				*(ref this.storedD3DLight + 44) = (float)value.B * 0.003921569f;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x000620A4 File Offset: 0x000614A4
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x000722A8 File Offset: 0x000716A8
		public ColorValue DiffuseColor
		{
			get
			{
				ref void color = ref this.storedD3DLight + 4;
				return ColorValue.FromD3DColor(ref color);
			}
			set
			{
				ref void pColor = ref this.storedD3DLight + 4;
				value.FillD3DColor(ref pColor);
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x000620C8 File Offset: 0x000614C8
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x000722D0 File Offset: 0x000716D0
		public ColorValue SpecularColor
		{
			get
			{
				ref void color = ref this.storedD3DLight + 20;
				return ColorValue.FromD3DColor(ref color);
			}
			set
			{
				ref void pColor = ref this.storedD3DLight + 20;
				value.FillD3DColor(ref pColor);
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x000620EC File Offset: 0x000614EC
		// (set) Token: 0x06000504 RID: 1284 RVA: 0x000722F8 File Offset: 0x000716F8
		public ColorValue AmbientColor
		{
			get
			{
				ref void color = ref this.storedD3DLight + 36;
				return ColorValue.FromD3DColor(ref color);
			}
			set
			{
				ref void pColor = ref this.storedD3DLight + 36;
				value.FillD3DColor(ref pColor);
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x00072320 File Offset: 0x00071720
		// (set) Token: 0x06000506 RID: 1286 RVA: 0x0007233C File Offset: 0x0007173C
		public unsafe float Range
		{
			get
			{
				return *(ref this.storedD3DLight + 76);
			}
			set
			{
				*(ref this.storedD3DLight + 76) = value;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x0007235C File Offset: 0x0007175C
		// (set) Token: 0x06000508 RID: 1288 RVA: 0x00072378 File Offset: 0x00071778
		public unsafe float Falloff
		{
			get
			{
				return *(ref this.storedD3DLight + 80);
			}
			set
			{
				*(ref this.storedD3DLight + 80) = value;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x00072398 File Offset: 0x00071798
		// (set) Token: 0x0600050A RID: 1290 RVA: 0x000723B4 File Offset: 0x000717B4
		public unsafe float Attenuation0
		{
			get
			{
				return *(ref this.storedD3DLight + 84);
			}
			set
			{
				*(ref this.storedD3DLight + 84) = value;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x000723D4 File Offset: 0x000717D4
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x000723F0 File Offset: 0x000717F0
		public unsafe float Attenuation1
		{
			get
			{
				return *(ref this.storedD3DLight + 88);
			}
			set
			{
				*(ref this.storedD3DLight + 88) = value;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x00072410 File Offset: 0x00071810
		// (set) Token: 0x0600050E RID: 1294 RVA: 0x0007242C File Offset: 0x0007182C
		public unsafe float Attenuation2
		{
			get
			{
				return *(ref this.storedD3DLight + 92);
			}
			set
			{
				*(ref this.storedD3DLight + 92) = value;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x0007244C File Offset: 0x0007184C
		// (set) Token: 0x06000510 RID: 1296 RVA: 0x00072468 File Offset: 0x00071868
		public unsafe float InnerConeAngle
		{
			get
			{
				return *(ref this.storedD3DLight + 96);
			}
			set
			{
				*(ref this.storedD3DLight + 96) = value;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x00072488 File Offset: 0x00071888
		// (set) Token: 0x06000512 RID: 1298 RVA: 0x000724A4 File Offset: 0x000718A4
		public unsafe float OuterConeAngle
		{
			get
			{
				return *(ref this.storedD3DLight + 100);
			}
			set
			{
				*(ref this.storedD3DLight + 100) = value;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x000724C4 File Offset: 0x000718C4
		// (set) Token: 0x06000514 RID: 1300 RVA: 0x000724F8 File Offset: 0x000718F8
		public Vector3 Position
		{
			get
			{
				Vector3 result = default(Vector3);
				result = new Vector3();
				cpblk(ref result, ref this.storedD3DLight + 52, 12);
				return result;
			}
			set
			{
				cpblk(ref this.storedD3DLight + 52, ref value, 12);
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x0007251C File Offset: 0x0007191C
		// (set) Token: 0x06000516 RID: 1302 RVA: 0x00072550 File Offset: 0x00071950
		public Vector3 Direction
		{
			get
			{
				Vector3 result = default(Vector3);
				result = new Vector3();
				cpblk(ref result, ref this.storedD3DLight + 64, 12);
				return result;
			}
			set
			{
				cpblk(ref this.storedD3DLight + 64, ref value, 12);
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x00072574 File Offset: 0x00071974
		// (set) Token: 0x06000518 RID: 1304 RVA: 0x00072590 File Offset: 0x00071990
		public unsafe float XPosition
		{
			get
			{
				return *(ref this.storedD3DLight + 52);
			}
			set
			{
				*(ref this.storedD3DLight + 52) = value;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x000725B0 File Offset: 0x000719B0
		// (set) Token: 0x0600051A RID: 1306 RVA: 0x000725CC File Offset: 0x000719CC
		public unsafe float YPosition
		{
			get
			{
				return *(ref this.storedD3DLight + 56);
			}
			set
			{
				*(ref this.storedD3DLight + 56) = value;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x000725EC File Offset: 0x000719EC
		// (set) Token: 0x0600051C RID: 1308 RVA: 0x00072608 File Offset: 0x00071A08
		public unsafe float ZPosition
		{
			get
			{
				return *(ref this.storedD3DLight + 60);
			}
			set
			{
				*(ref this.storedD3DLight + 60) = value;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00072628 File Offset: 0x00071A28
		// (set) Token: 0x0600051E RID: 1310 RVA: 0x00072644 File Offset: 0x00071A44
		public unsafe float XDirection
		{
			get
			{
				return *(ref this.storedD3DLight + 64);
			}
			set
			{
				*(ref this.storedD3DLight + 64) = value;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00072664 File Offset: 0x00071A64
		// (set) Token: 0x06000520 RID: 1312 RVA: 0x00072680 File Offset: 0x00071A80
		public unsafe float YDirection
		{
			get
			{
				return *(ref this.storedD3DLight + 68);
			}
			set
			{
				*(ref this.storedD3DLight + 68) = value;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x000726A0 File Offset: 0x00071AA0
		// (set) Token: 0x06000522 RID: 1314 RVA: 0x000726BC File Offset: 0x00071ABC
		public unsafe float ZDirection
		{
			get
			{
				return *(ref this.storedD3DLight + 72);
			}
			set
			{
				*(ref this.storedD3DLight + 72) = value;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x000726DC File Offset: 0x00071ADC
		// (set) Token: 0x06000524 RID: 1316 RVA: 0x000727A4 File Offset: 0x00071BA4
		public unsafe bool Enabled
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				int num = 1;
				if (this.pDevice != null && this.index != -1)
				{
					object obj = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),Microsoft.DirectX.PrivateImplementationDetails._D3DLIGHT9 modopt(Microsoft.VisualC.IsConstModifier)*), this.pDevice.m_lpUM, this.index, ref this.storedD3DLight, *(*(int*)this.pDevice.m_lpUM + 204));
					int num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32*), this.pDevice.m_lpUM, this.index, ref num, *(*(int*)this.pDevice.m_lpUM + 216));
					if (num2 < 0)
					{
						if (!DirectXException.IsExceptionIgnored)
						{
							Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num2);
							DirectXException ex = exceptionFromResultInternal as DirectXException;
							if (ex != null)
							{
								ex.ErrorCode = num2;
								throw ex;
							}
							throw exceptionFromResultInternal;
						}
						else
						{
							<Module>.SetLastError(num2);
						}
					}
					return num != 0;
				}
				throw new InvalidOperationException();
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				if (this.pDevice != null && this.index != -1)
				{
					object obj = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),Microsoft.DirectX.PrivateImplementationDetails._D3DLIGHT9 modopt(Microsoft.VisualC.IsConstModifier)*), this.pDevice.m_lpUM, this.index, ref this.storedD3DLight, *(*(int*)this.pDevice.m_lpUM + 204));
					int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32), this.pDevice.m_lpUM, this.index, value ? 1 : 0, *(*(int*)this.pDevice.m_lpUM + 212));
					if (num < 0)
					{
						if (!DirectXException.IsExceptionIgnored)
						{
							Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num);
							DirectXException ex = exceptionFromResultInternal as DirectXException;
							if (ex != null)
							{
								ex.ErrorCode = num;
								throw ex;
							}
							throw exceptionFromResultInternal;
						}
						else
						{
							<Module>.SetLastError(num);
						}
					}
					return;
				}
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00072868 File Offset: 0x00071C68
		public unsafe void Update()
		{
			if (this.pDevice != null && this.index != -1)
			{
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),Microsoft.DirectX.PrivateImplementationDetails._D3DLIGHT9 modopt(Microsoft.VisualC.IsConstModifier)*), this.pDevice.m_lpUM, this.index, ref this.storedD3DLight, *(*(int*)this.pDevice.m_lpUM + 204));
				if (num < 0)
				{
					if (!DirectXException.IsExceptionIgnored)
					{
						Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num);
						DirectXException ex = exceptionFromResultInternal as DirectXException;
						if (ex != null)
						{
							ex.ErrorCode = num;
							throw ex;
						}
						throw exceptionFromResultInternal;
					}
					else
					{
						<Module>.SetLastError(num);
					}
				}
				return;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0400101C RID: 4124
		private _D3DLIGHT9 storedD3DLight;

		// Token: 0x0400101D RID: 4125
		private Device pDevice;

		// Token: 0x0400101E RID: 4126
		private int index;
	}
}

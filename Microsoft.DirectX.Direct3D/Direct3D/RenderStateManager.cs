using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000E7 RID: 231
	public sealed class RenderStateManager
	{
		// Token: 0x060003A6 RID: 934 RVA: 0x00061D4C File Offset: 0x0006114C
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$122$];
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

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0006F360 File Offset: 0x0006E760
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x0006F380 File Offset: 0x0006E780
		public FillMode FillMode
		{
			get
			{
				return (FillMode)this.pDevice.GetRenderStateInt32(RenderStates.FillMode);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.FillMode, (int)value);
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x0006F3A0 File Offset: 0x0006E7A0
		// (set) Token: 0x060003AA RID: 938 RVA: 0x0006F3C0 File Offset: 0x0006E7C0
		public ShadeMode ShadeMode
		{
			get
			{
				return (ShadeMode)this.pDevice.GetRenderStateInt32(RenderStates.ShadeMode);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.ShadeMode, (int)value);
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060003AB RID: 939 RVA: 0x0006F3E0 File Offset: 0x0006E7E0
		// (set) Token: 0x060003AC RID: 940 RVA: 0x0006F400 File Offset: 0x0006E800
		public bool ZBufferEnable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.ZEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.ZEnable, value);
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060003AD RID: 941 RVA: 0x0006F420 File Offset: 0x0006E820
		// (set) Token: 0x060003AE RID: 942 RVA: 0x0006F444 File Offset: 0x0006E844
		public bool UseWBuffer
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateInt32(RenderStates.ZEnable) == 2;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				if (value)
				{
					this.pDevice.SetRenderState(RenderStates.ZEnable, 2);
				}
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0006F468 File Offset: 0x0006E868
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x0006F488 File Offset: 0x0006E888
		public bool ZBufferWriteEnable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.ZBufferWriteEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.ZBufferWriteEnable, value);
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0006F4A8 File Offset: 0x0006E8A8
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x0006F4C8 File Offset: 0x0006E8C8
		public bool AlphaTestEnable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.AlphaTestEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.AlphaTestEnable, value);
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x0006F4E8 File Offset: 0x0006E8E8
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x0006F508 File Offset: 0x0006E908
		public bool LastPixel
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.LastPixel);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.LastPixel, value);
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0006F528 File Offset: 0x0006E928
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x0006F548 File Offset: 0x0006E948
		public Blend SourceBlend
		{
			get
			{
				return (Blend)this.pDevice.GetRenderStateInt32(RenderStates.SourceBlend);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.SourceBlend, (int)value);
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x0006F568 File Offset: 0x0006E968
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x0006F588 File Offset: 0x0006E988
		public Blend DestinationBlend
		{
			get
			{
				return (Blend)this.pDevice.GetRenderStateInt32(RenderStates.DestinationBlend);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.DestinationBlend, (int)value);
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x0006F5A8 File Offset: 0x0006E9A8
		// (set) Token: 0x060003BA RID: 954 RVA: 0x0006F5C8 File Offset: 0x0006E9C8
		public Cull CullMode
		{
			get
			{
				return (Cull)this.pDevice.GetRenderStateInt32(RenderStates.CullMode);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.CullMode, (int)value);
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0006F5E8 File Offset: 0x0006E9E8
		// (set) Token: 0x060003BC RID: 956 RVA: 0x0006F608 File Offset: 0x0006EA08
		public Compare ZBufferFunction
		{
			get
			{
				return (Compare)this.pDevice.GetRenderStateInt32(RenderStates.ZBufferFunction);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.ZBufferFunction, (int)value);
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0006F628 File Offset: 0x0006EA28
		// (set) Token: 0x060003BE RID: 958 RVA: 0x0006F648 File Offset: 0x0006EA48
		public int ReferenceAlpha
		{
			get
			{
				return this.pDevice.GetRenderStateInt32(RenderStates.ReferenceAlpha);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.ReferenceAlpha, value);
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060003BF RID: 959 RVA: 0x0006F668 File Offset: 0x0006EA68
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x0006F688 File Offset: 0x0006EA88
		public Compare AlphaFunction
		{
			get
			{
				return (Compare)this.pDevice.GetRenderStateInt32(RenderStates.AlphaFunction);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.AlphaFunction, (int)value);
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0006F6A8 File Offset: 0x0006EAA8
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x0006F6C8 File Offset: 0x0006EAC8
		public bool DitherEnable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.DitherEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.DitherEnable, value);
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x0006F6E8 File Offset: 0x0006EAE8
		// (set) Token: 0x060003C4 RID: 964 RVA: 0x0006F708 File Offset: 0x0006EB08
		public bool AlphaBlendEnable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.AlphaBlendEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.AlphaBlendEnable, value);
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x0006F728 File Offset: 0x0006EB28
		// (set) Token: 0x060003C6 RID: 966 RVA: 0x0006F748 File Offset: 0x0006EB48
		public bool FogEnable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.FogEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.FogEnable, value);
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0006F7B0 File Offset: 0x0006EBB0
		// (set) Token: 0x060003C8 RID: 968 RVA: 0x0006F7D0 File Offset: 0x0006EBD0
		public bool SpecularEnable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.SpecularEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.SpecularEnable, value);
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x0006F880 File Offset: 0x0006EC80
		// (set) Token: 0x060003CA RID: 970 RVA: 0x0006F8A0 File Offset: 0x0006ECA0
		public FogMode FogTableMode
		{
			get
			{
				return (FogMode)this.pDevice.GetRenderStateInt32(RenderStates.FogTableMode);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.FogTableMode, (int)value);
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060003CB RID: 971 RVA: 0x0006F8C0 File Offset: 0x0006ECC0
		// (set) Token: 0x060003CC RID: 972 RVA: 0x0006F8E0 File Offset: 0x0006ECE0
		public float FogStart
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.FogStart);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.FogStart, value);
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060003CD RID: 973 RVA: 0x0006F900 File Offset: 0x0006ED00
		// (set) Token: 0x060003CE RID: 974 RVA: 0x0006F920 File Offset: 0x0006ED20
		public float FogEnd
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.FogEnd);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.FogEnd, value);
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060003CF RID: 975 RVA: 0x0006F940 File Offset: 0x0006ED40
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x0006F960 File Offset: 0x0006ED60
		public float FogDensity
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.FogDensity);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.FogDensity, value);
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x0006F980 File Offset: 0x0006ED80
		// (set) Token: 0x060003D2 RID: 978 RVA: 0x0006F9A0 File Offset: 0x0006EDA0
		public bool RangeFogEnable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.RangeFogEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.RangeFogEnable, value);
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x0006F9C0 File Offset: 0x0006EDC0
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x0006F9E0 File Offset: 0x0006EDE0
		public bool StencilEnable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.StencilEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.StencilEnable, value);
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0006FA00 File Offset: 0x0006EE00
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x0006FA20 File Offset: 0x0006EE20
		public StencilOperation StencilFail
		{
			get
			{
				return (StencilOperation)this.pDevice.GetRenderStateInt32(RenderStates.StencilFail);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.StencilFail, (int)value);
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0006FA40 File Offset: 0x0006EE40
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x0006FA60 File Offset: 0x0006EE60
		public StencilOperation StencilZBufferFail
		{
			get
			{
				return (StencilOperation)this.pDevice.GetRenderStateInt32(RenderStates.StencilZBufferFail);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.StencilZBufferFail, (int)value);
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x0006FA80 File Offset: 0x0006EE80
		// (set) Token: 0x060003DA RID: 986 RVA: 0x0006FAA0 File Offset: 0x0006EEA0
		public StencilOperation StencilPass
		{
			get
			{
				return (StencilOperation)this.pDevice.GetRenderStateInt32(RenderStates.StencilPass);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.StencilPass, (int)value);
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060003DB RID: 987 RVA: 0x0006FAC0 File Offset: 0x0006EEC0
		// (set) Token: 0x060003DC RID: 988 RVA: 0x0006FAE0 File Offset: 0x0006EEE0
		public Compare StencilFunction
		{
			get
			{
				return (Compare)this.pDevice.GetRenderStateInt32(RenderStates.StencilFunction);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.StencilFunction, (int)value);
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060003DD RID: 989 RVA: 0x0006FB00 File Offset: 0x0006EF00
		// (set) Token: 0x060003DE RID: 990 RVA: 0x0006FB20 File Offset: 0x0006EF20
		public int ReferenceStencil
		{
			get
			{
				return this.pDevice.GetRenderStateInt32(RenderStates.ReferenceStencil);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.ReferenceStencil, value);
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0006FB40 File Offset: 0x0006EF40
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x0006FB60 File Offset: 0x0006EF60
		public int StencilMask
		{
			get
			{
				return this.pDevice.GetRenderStateInt32(RenderStates.StencilMask);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.StencilMask, value);
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0006FB80 File Offset: 0x0006EF80
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x0006FBA0 File Offset: 0x0006EFA0
		public int StencilWriteMask
		{
			get
			{
				return this.pDevice.GetRenderStateInt32(RenderStates.StencilWriteMask);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.StencilWriteMask, value);
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x0006FBC0 File Offset: 0x0006EFC0
		// (set) Token: 0x060003E4 RID: 996 RVA: 0x0006FBE0 File Offset: 0x0006EFE0
		public int TextureFactor
		{
			get
			{
				return this.pDevice.GetRenderStateInt32(RenderStates.TextureFactor);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.TextureFactor, value);
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x0006FC00 File Offset: 0x0006F000
		// (set) Token: 0x060003E6 RID: 998 RVA: 0x0006FC24 File Offset: 0x0006F024
		public WrapCoordinates Wrap0
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap0);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap0, (int)value);
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0006FC48 File Offset: 0x0006F048
		// (set) Token: 0x060003E8 RID: 1000 RVA: 0x0006FC6C File Offset: 0x0006F06C
		public WrapCoordinates Wrap1
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap1);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap1, (int)value);
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0006FC90 File Offset: 0x0006F090
		// (set) Token: 0x060003EA RID: 1002 RVA: 0x0006FCB4 File Offset: 0x0006F0B4
		public WrapCoordinates Wrap2
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap2);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap2, (int)value);
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x0006FCD8 File Offset: 0x0006F0D8
		// (set) Token: 0x060003EC RID: 1004 RVA: 0x0006FCFC File Offset: 0x0006F0FC
		public WrapCoordinates Wrap3
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap3);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap3, (int)value);
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0006FD20 File Offset: 0x0006F120
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x0006FD44 File Offset: 0x0006F144
		public WrapCoordinates Wrap4
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap4);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap4, (int)value);
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x0006FD68 File Offset: 0x0006F168
		// (set) Token: 0x060003F0 RID: 1008 RVA: 0x0006FD8C File Offset: 0x0006F18C
		public WrapCoordinates Wrap5
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap5);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap5, (int)value);
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x0006FDB0 File Offset: 0x0006F1B0
		// (set) Token: 0x060003F2 RID: 1010 RVA: 0x0006FDD4 File Offset: 0x0006F1D4
		public WrapCoordinates Wrap6
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap6);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap6, (int)value);
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0006FDF8 File Offset: 0x0006F1F8
		// (set) Token: 0x060003F4 RID: 1012 RVA: 0x0006FE1C File Offset: 0x0006F21C
		public WrapCoordinates Wrap7
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap7);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap7, (int)value);
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x0006FE40 File Offset: 0x0006F240
		// (set) Token: 0x060003F6 RID: 1014 RVA: 0x0006FE64 File Offset: 0x0006F264
		public WrapCoordinates Wrap8
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap8);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap8, (int)value);
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0006FE88 File Offset: 0x0006F288
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x0006FEAC File Offset: 0x0006F2AC
		public WrapCoordinates Wrap9
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap9);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap9, (int)value);
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x0006FED0 File Offset: 0x0006F2D0
		// (set) Token: 0x060003FA RID: 1018 RVA: 0x0006FEF4 File Offset: 0x0006F2F4
		public WrapCoordinates Wrap10
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap10);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap10, (int)value);
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x0006FF18 File Offset: 0x0006F318
		// (set) Token: 0x060003FC RID: 1020 RVA: 0x0006FF3C File Offset: 0x0006F33C
		public WrapCoordinates Wrap11
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap11);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap11, (int)value);
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x0006FF60 File Offset: 0x0006F360
		// (set) Token: 0x060003FE RID: 1022 RVA: 0x0006FF84 File Offset: 0x0006F384
		public WrapCoordinates Wrap12
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap12);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap12, (int)value);
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x0006FFA8 File Offset: 0x0006F3A8
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x0006FFCC File Offset: 0x0006F3CC
		public WrapCoordinates Wrap13
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap13);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap13, (int)value);
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0006FFF0 File Offset: 0x0006F3F0
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x00070014 File Offset: 0x0006F414
		public WrapCoordinates Wrap14
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap14);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap14, (int)value);
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x00070038 File Offset: 0x0006F438
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x0007005C File Offset: 0x0006F45C
		public WrapCoordinates Wrap15
		{
			get
			{
				return (WrapCoordinates)this.pDevice.GetRenderStateInt32(RenderStates.Wrap15);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Wrap15, (int)value);
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x00070080 File Offset: 0x0006F480
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x000700A4 File Offset: 0x0006F4A4
		public bool Clipping
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.Clipping);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.Clipping, value);
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x000700C8 File Offset: 0x0006F4C8
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x000700EC File Offset: 0x0006F4EC
		public bool Lighting
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.Lighting);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.Lighting, value);
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x000701B0 File Offset: 0x0006F5B0
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x000701D4 File Offset: 0x0006F5D4
		public FogMode FogVertexMode
		{
			get
			{
				return (FogMode)this.pDevice.GetRenderStateInt32(RenderStates.FogVertexMode);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.FogVertexMode, (int)value);
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x000701F8 File Offset: 0x0006F5F8
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x0007021C File Offset: 0x0006F61C
		public bool ColorVertex
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.ColorVertex);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.ColorVertex, value);
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x00070240 File Offset: 0x0006F640
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x00070264 File Offset: 0x0006F664
		public bool LocalViewer
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.LocalViewer);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.LocalViewer, value);
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00070288 File Offset: 0x0006F688
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x000702AC File Offset: 0x0006F6AC
		public bool NormalizeNormals
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.NormalizeNormals);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.NormalizeNormals, value);
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x000702D0 File Offset: 0x0006F6D0
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x000702F4 File Offset: 0x0006F6F4
		public ColorSource DiffuseMaterialSource
		{
			get
			{
				return (ColorSource)this.pDevice.GetRenderStateInt32(RenderStates.DiffuseMaterialSource);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.DiffuseMaterialSource, (int)value);
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00070318 File Offset: 0x0006F718
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x0007033C File Offset: 0x0006F73C
		public ColorSource SpecularMaterialSource
		{
			get
			{
				return (ColorSource)this.pDevice.GetRenderStateInt32(RenderStates.SpecularMaterialSource);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.SpecularMaterialSource, (int)value);
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00070360 File Offset: 0x0006F760
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x00070384 File Offset: 0x0006F784
		public ColorSource AmbientMaterialSource
		{
			get
			{
				return (ColorSource)this.pDevice.GetRenderStateInt32(RenderStates.AmbientMaterialSource);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.AmbientMaterialSource, (int)value);
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x000703A8 File Offset: 0x0006F7A8
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x000703CC File Offset: 0x0006F7CC
		public ColorSource EmissiveMaterialSource
		{
			get
			{
				return (ColorSource)this.pDevice.GetRenderStateInt32(RenderStates.EmissiveMaterialSource);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.EmissiveMaterialSource, (int)value);
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x000703F0 File Offset: 0x0006F7F0
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x00070414 File Offset: 0x0006F814
		public VertexBlend VertexBlend
		{
			get
			{
				return (VertexBlend)this.pDevice.GetRenderStateInt32(RenderStates.VertexBlend);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.VertexBlend, (int)value);
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00070438 File Offset: 0x0006F838
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x0007045C File Offset: 0x0006F85C
		public float PointSize
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.PointSize);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.PointSize, value);
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00070480 File Offset: 0x0006F880
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x000704A4 File Offset: 0x0006F8A4
		public float PointSizeMin
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.PointSizeMin);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.PointSizeMin, value);
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x000704C8 File Offset: 0x0006F8C8
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x000704EC File Offset: 0x0006F8EC
		public bool PointSpriteEnable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.PointSpriteEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.PointSpriteEnable, value);
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x00070510 File Offset: 0x0006F910
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x00070534 File Offset: 0x0006F934
		public bool PointScaleEnable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.PointScaleEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.PointScaleEnable, value);
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x00070558 File Offset: 0x0006F958
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x0007057C File Offset: 0x0006F97C
		public float PointScaleA
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.PointScaleA);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.PointScaleA, value);
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x000705A0 File Offset: 0x0006F9A0
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x000705C4 File Offset: 0x0006F9C4
		public float PointScaleB
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.PointScaleB);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.PointScaleB, value);
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x000705E8 File Offset: 0x0006F9E8
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x0007060C File Offset: 0x0006FA0C
		public float PointScaleC
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.PointScaleC);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.PointScaleC, value);
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x00070630 File Offset: 0x0006FA30
		// (set) Token: 0x0600042A RID: 1066 RVA: 0x00070654 File Offset: 0x0006FA54
		public bool MultiSampleAntiAlias
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.MultisampleAntiAlias);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.MultisampleAntiAlias, value);
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x00070678 File Offset: 0x0006FA78
		// (set) Token: 0x0600042C RID: 1068 RVA: 0x0007069C File Offset: 0x0006FA9C
		public int MultiSampleMask
		{
			get
			{
				return this.pDevice.GetRenderStateInt32(RenderStates.MultisampleMask);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.MultisampleMask, value);
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x000706C0 File Offset: 0x0006FAC0
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x000706E4 File Offset: 0x0006FAE4
		public PatchEdge PatchEdgeStyle
		{
			get
			{
				return (PatchEdge)this.pDevice.GetRenderStateInt32(RenderStates.PatchEdgeStyle);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.PatchEdgeStyle, (int)value);
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x00070708 File Offset: 0x0006FB08
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x0007072C File Offset: 0x0006FB2C
		public bool DebugMonitorTokenEnabled
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.DebugMonitorToken);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.DebugMonitorToken, value ? 0 : 1);
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x00070758 File Offset: 0x0006FB58
		// (set) Token: 0x06000432 RID: 1074 RVA: 0x0007077C File Offset: 0x0006FB7C
		public float PointSizeMax
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.PointSizeMax);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.PointSizeMax, value);
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x000707A0 File Offset: 0x0006FBA0
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x000707C4 File Offset: 0x0006FBC4
		public bool IndexedVertexBlendEnable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.IndexedVertexBlendEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.IndexedVertexBlendEnable, value);
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x000707E8 File Offset: 0x0006FBE8
		// (set) Token: 0x06000436 RID: 1078 RVA: 0x0007080C File Offset: 0x0006FC0C
		public ColorWriteEnable ColorWriteEnable
		{
			get
			{
				return (ColorWriteEnable)this.pDevice.GetRenderStateInt32(RenderStates.ColorWriteEnable);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.ColorWriteEnable, (int)value);
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x00070908 File Offset: 0x0006FD08
		// (set) Token: 0x06000438 RID: 1080 RVA: 0x0007092C File Offset: 0x0006FD2C
		public float TweenFactor
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.TweenFactor);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.TweenFactor, value);
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x00070950 File Offset: 0x0006FD50
		// (set) Token: 0x0600043A RID: 1082 RVA: 0x00070974 File Offset: 0x0006FD74
		public BlendOperation BlendOperation
		{
			get
			{
				return (BlendOperation)this.pDevice.GetRenderStateInt32(RenderStates.BlendOperation);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.BlendOperation, (int)value);
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x00070998 File Offset: 0x0006FD98
		// (set) Token: 0x0600043C RID: 1084 RVA: 0x000709BC File Offset: 0x0006FDBC
		public DegreeType PositionDegree
		{
			get
			{
				return (DegreeType)this.pDevice.GetRenderStateInt32(RenderStates.PositionDegree);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.PositionDegree, (int)value);
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x000709E0 File Offset: 0x0006FDE0
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x00070A04 File Offset: 0x0006FE04
		public DegreeType NormalDegree
		{
			get
			{
				return (DegreeType)this.pDevice.GetRenderStateInt32(RenderStates.NormalDegree);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.NormalDegree, (int)value);
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x00070A28 File Offset: 0x0006FE28
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x00070A4C File Offset: 0x0006FE4C
		public bool ScissorTestEnable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.ScissorTestEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.ScissorTestEnable, value);
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x00070A70 File Offset: 0x0006FE70
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x00070A94 File Offset: 0x0006FE94
		public float SlopeScaleDepthBias
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.SlopeScaleDepthBias);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.SlopeScaleDepthBias, value);
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x00070AB8 File Offset: 0x0006FEB8
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x00070ADC File Offset: 0x0006FEDC
		public float DepthBias
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.DepthBias);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.DepthBias, value);
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00070B00 File Offset: 0x0006FF00
		// (set) Token: 0x06000446 RID: 1094 RVA: 0x00070B24 File Offset: 0x0006FF24
		public bool AntiAliasedLineEnable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.AntialiasedLineEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.AntialiasedLineEnable, value);
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x00070B48 File Offset: 0x0006FF48
		// (set) Token: 0x06000448 RID: 1096 RVA: 0x00070B6C File Offset: 0x0006FF6C
		public float MaxTessellationLevel
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.MaxTessellationLevel);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.MaxTessellationLevel, value);
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x00070B90 File Offset: 0x0006FF90
		// (set) Token: 0x0600044A RID: 1098 RVA: 0x00070BB4 File Offset: 0x0006FFB4
		public float MinTessellationLevel
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.MinTessellationLevel);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.MinTessellationLevel, value);
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x00070BD8 File Offset: 0x0006FFD8
		// (set) Token: 0x0600044C RID: 1100 RVA: 0x00070BFC File Offset: 0x0006FFFC
		public float AdaptiveTessellateX
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.AdaptiveTessellateX);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.AdaptiveTessellateX, value);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x00070C20 File Offset: 0x00070020
		// (set) Token: 0x0600044E RID: 1102 RVA: 0x00070C44 File Offset: 0x00070044
		public float AdaptiveTessellateY
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.AdaptiveTessellateY);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.AdaptiveTessellateY, value);
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x00070C68 File Offset: 0x00070068
		// (set) Token: 0x06000450 RID: 1104 RVA: 0x00070C8C File Offset: 0x0007008C
		public float AdaptiveTessellateZ
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.AdaptiveTessellateZ);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.AdaptiveTessellateZ, value);
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x00070CB0 File Offset: 0x000700B0
		// (set) Token: 0x06000452 RID: 1106 RVA: 0x00070CD4 File Offset: 0x000700D4
		public float AdaptiveTessellateW
		{
			get
			{
				return this.pDevice.GetRenderStateSingle(RenderStates.AdaptiveTessellateW);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.AdaptiveTessellateW, value);
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x00070CF8 File Offset: 0x000700F8
		// (set) Token: 0x06000454 RID: 1108 RVA: 0x00070D1C File Offset: 0x0007011C
		public bool EnableAdaptiveTessellation
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.EnableAdaptiveTessellation);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.EnableAdaptiveTessellation, value);
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x00070D40 File Offset: 0x00070140
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x00070D64 File Offset: 0x00070164
		public bool TwoSidedStencilMode
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.TwoSidedStencilMode);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.TwoSidedStencilMode, value);
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00070D88 File Offset: 0x00070188
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x00070DAC File Offset: 0x000701AC
		public StencilOperation CounterClockwiseStencilFail
		{
			get
			{
				return (StencilOperation)this.pDevice.GetRenderStateInt32(RenderStates.CounterClockwiseStencilFail);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.CounterClockwiseStencilFail, (int)value);
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00070DD0 File Offset: 0x000701D0
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x00070DF4 File Offset: 0x000701F4
		public StencilOperation CounterClockwiseStencilZBufferFail
		{
			get
			{
				return (StencilOperation)this.pDevice.GetRenderStateInt32(RenderStates.CounterClockwiseStencilZBufferFail);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.CounterClockwiseStencilZBufferFail, (int)value);
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x00070E18 File Offset: 0x00070218
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x00070E3C File Offset: 0x0007023C
		public StencilOperation CounterClockwiseStencilPass
		{
			get
			{
				return (StencilOperation)this.pDevice.GetRenderStateInt32(RenderStates.CounterClockwiseStencilPass);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.CounterClockwiseStencilPass, (int)value);
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x00070E60 File Offset: 0x00070260
		// (set) Token: 0x0600045E RID: 1118 RVA: 0x00070E84 File Offset: 0x00070284
		public Compare CounterClockwiseStencilFunction
		{
			get
			{
				return (Compare)this.pDevice.GetRenderStateInt32(RenderStates.CounterClockwiseStencilFunction);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.CounterClockwiseStencilFunction, (int)value);
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x00070830 File Offset: 0x0006FC30
		// (set) Token: 0x06000460 RID: 1120 RVA: 0x00070854 File Offset: 0x0006FC54
		public ColorWriteEnable ColorWriteEnable1
		{
			get
			{
				return (ColorWriteEnable)this.pDevice.GetRenderStateInt32(RenderStates.ColorWriteEnable1);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.ColorWriteEnable1, (int)value);
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00070878 File Offset: 0x0006FC78
		// (set) Token: 0x06000462 RID: 1122 RVA: 0x0007089C File Offset: 0x0006FC9C
		public ColorWriteEnable ColorWriteEnable2
		{
			get
			{
				return (ColorWriteEnable)this.pDevice.GetRenderStateInt32(RenderStates.ColorWriteEnable2);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.ColorWriteEnable2, (int)value);
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x000708C0 File Offset: 0x0006FCC0
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x000708E4 File Offset: 0x0006FCE4
		public ColorWriteEnable ColorWriteEnable3
		{
			get
			{
				return (ColorWriteEnable)this.pDevice.GetRenderStateInt32(RenderStates.ColorWriteEnable3);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.ColorWriteEnable3, (int)value);
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00070F48 File Offset: 0x00070348
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x00070F6C File Offset: 0x0007036C
		public bool SeparateAlphaBlendEnabled
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.SeparateAlphaBlendEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.SeparateAlphaBlendEnable, value);
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x00070F90 File Offset: 0x00070390
		// (set) Token: 0x06000468 RID: 1128 RVA: 0x00070FB4 File Offset: 0x000703B4
		public Blend AlphaSourceBlend
		{
			get
			{
				return (Blend)this.pDevice.GetRenderStateInt32(RenderStates.SourceBlendAlpha);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.SourceBlendAlpha, (int)value);
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x00070FD8 File Offset: 0x000703D8
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x00070FFC File Offset: 0x000703FC
		public Blend AlphaDestinationBlend
		{
			get
			{
				return (Blend)this.pDevice.GetRenderStateInt32(RenderStates.DestinationBlendAlpha);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.DestinationBlendAlpha, (int)value);
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x00071020 File Offset: 0x00070420
		// (set) Token: 0x0600046C RID: 1132 RVA: 0x00071044 File Offset: 0x00070444
		public BlendOperation AlphaBlendOperation
		{
			get
			{
				return (BlendOperation)this.pDevice.GetRenderStateInt32(RenderStates.BlendOperationAlpha);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.BlendOperationAlpha, (int)value);
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x00070110 File Offset: 0x0006F510
		// (set) Token: 0x0600046E RID: 1134 RVA: 0x0007013C File Offset: 0x0006F53C
		public Color Ambient
		{
			get
			{
				return Color.FromArgb(this.pDevice.GetRenderStateInt32(RenderStates.Ambient));
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Ambient, value.ToArgb());
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x0006F7F0 File Offset: 0x0006EBF0
		// (set) Token: 0x06000470 RID: 1136 RVA: 0x0006F818 File Offset: 0x0006EC18
		public Color FogColor
		{
			get
			{
				return Color.FromArgb(this.pDevice.GetRenderStateInt32(RenderStates.FogColor));
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.FogColor, value.ToArgb());
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x00070EA8 File Offset: 0x000702A8
		// (set) Token: 0x06000472 RID: 1138 RVA: 0x00070ED4 File Offset: 0x000702D4
		public Color BlendFactor
		{
			get
			{
				return Color.FromArgb(this.pDevice.GetRenderStateInt32(RenderStates.BlendFactor));
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.BlendFactor, value.ToArgb());
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x0006F768 File Offset: 0x0006EB68
		// (set) Token: 0x06000474 RID: 1140 RVA: 0x0006F78C File Offset: 0x0006EB8C
		public bool SrgbWriteEnable
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.pDevice.GetRenderStateBoolean(RenderStates.SrgbWriteEnable);
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				this.pDevice.SetRenderState(RenderStates.SrgbWriteEnable, value);
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x00070168 File Offset: 0x0006F568
		// (set) Token: 0x06000476 RID: 1142 RVA: 0x0007018C File Offset: 0x0006F58C
		public int AmbientColor
		{
			get
			{
				return this.pDevice.GetRenderStateInt32(RenderStates.Ambient);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.Ambient, value);
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x0006F840 File Offset: 0x0006EC40
		// (set) Token: 0x06000478 RID: 1144 RVA: 0x0006F860 File Offset: 0x0006EC60
		public int FogColorValue
		{
			get
			{
				return this.pDevice.GetRenderStateInt32(RenderStates.FogColor);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.FogColor, value);
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x00070F00 File Offset: 0x00070300
		// (set) Token: 0x0600047A RID: 1146 RVA: 0x00070F24 File Offset: 0x00070324
		public int BlendFactorColor
		{
			get
			{
				return this.pDevice.GetRenderStateInt32(RenderStates.BlendFactor);
			}
			set
			{
				this.pDevice.SetRenderState(RenderStates.BlendFactor, value);
			}
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0006F340 File Offset: 0x0006E740
		internal RenderStateManager(Device dev)
		{
			this.pDevice = dev;
		}

		// Token: 0x04000F84 RID: 3972
		internal Device pDevice;
	}
}

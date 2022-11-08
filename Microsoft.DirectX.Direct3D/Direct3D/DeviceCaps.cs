using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000038 RID: 56
	[MiscellaneousBits(1)]
	public struct DeviceCaps
	{
		// Token: 0x06000090 RID: 144 RVA: 0x000572A0 File Offset: 0x000566A0
		internal DeviceCaps()
		{
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00057280 File Offset: 0x00056680
		internal DeviceCaps(int c, int c2)
		{
			this.caps = c;
			this.caps2 = c2;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000572B4 File Offset: 0x000566B4
		public unsafe override string ToString()
		{
			object obj = this;
			Type type = obj.GetType();
			StringBuilder stringBuilder = new StringBuilder();
			PropertyInfo[] properties = type.GetProperties();
			int num = 0;
			if (0 < properties.Length)
			{
				do
				{
					MethodInfo getMethod = properties[num].GetGetMethod();
					if (getMethod != null && !getMethod.IsStatic)
					{
						object obj2 = getMethod.Invoke(obj, null);
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$5$];
						array[0] = properties[num].Name;
						string text;
						if (obj2 != null)
						{
							text = obj2.ToString();
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
			FieldInfo[] fields = type.GetFields();
			int num2 = 0;
			if (0 < fields.Length)
			{
				do
				{
					object value = fields[num2].GetValue(obj);
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$6$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000573E0 File Offset: 0x000567E0
		public bool SupportsExecuteSystemMemory
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 4 & 1U) != 0;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00057400 File Offset: 0x00056800
		public bool SupportsExecuteVideoMemory
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 5 & 1U) != 0;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00057420 File Offset: 0x00056820
		public bool SupportsTransformedVertexSystemMemory
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 6 & 1U) != 0;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00057440 File Offset: 0x00056840
		public bool SupportsTransformedVertexVideoMemory
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 7 & 1U) != 0;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00057460 File Offset: 0x00056860
		public bool SupportsTextureSystemMemory
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 8 & 1U) != 0;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00057480 File Offset: 0x00056880
		public bool SupportsTextureVideoMemory
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 9 & 1U) != 0;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000574A0 File Offset: 0x000568A0
		public bool SupportsDrawPrimitivesTransformedVertex
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 10 & 1U) != 0;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000574C0 File Offset: 0x000568C0
		public bool CanRenderAfterFlip
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 11 & 1U) != 0;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000574E0 File Offset: 0x000568E0
		public bool SupportsTextureNonLocalVideoMemory
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 12 & 1U) != 0;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00057500 File Offset: 0x00056900
		public bool SupportsDrawPrimitives2
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 13 & 1U) != 0;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00057520 File Offset: 0x00056920
		public bool SupportsSeparateTextureMemories
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 14 & 1U) != 0;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00057540 File Offset: 0x00056940
		public bool SupportsDrawPrimitives2Ex
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 15 & 1U) != 0;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00057560 File Offset: 0x00056960
		public unsafe bool SupportsHardwareTransformAndLight
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(*(ref this.caps + 2) & 1) != 0;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00057580 File Offset: 0x00056980
		public bool CanDrawSystemToNonLocal
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 17 & 1U) != 0;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000575A0 File Offset: 0x000569A0
		public bool SupportsHardwareRasterization
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 19 & 1U) != 0;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000575C0 File Offset: 0x000569C0
		public bool SupportsPureDevice
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 20 & 1U) != 0;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000575E0 File Offset: 0x000569E0
		public bool SupportsQuinticRtPatches
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 21 & 1U) != 0;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00057600 File Offset: 0x00056A00
		public bool SupportsRtPatches
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 22 & 1U) != 0;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00057620 File Offset: 0x00056A20
		public bool SupportsRtPatchHandleZero
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps >> 23 & 1U) != 0;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00057640 File Offset: 0x00056A40
		public unsafe bool SupportsNPatches
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (*(ref this.caps + 3) & 1) != 0;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00057660 File Offset: 0x00056A60
		public bool SupportsPreSampledDMapNPatch
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps2 >> 5 & 1U) != 0;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00057680 File Offset: 0x00056A80
		public bool SupportsStreamOffset
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)(this.caps2 & 1) != 0;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0005769C File Offset: 0x00056A9C
		public bool SupportsDMapNPatch
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps2 >> 1 & 1U) != 0;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000AA RID: 170 RVA: 0x000576BC File Offset: 0x00056ABC
		public bool SupportsAdaptiveTessellateRtPatch
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps2 >> 2 & 1U) != 0;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000576DC File Offset: 0x00056ADC
		public bool SupportsAdaptiveTessellateNPatch
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps2 >> 3 & 1U) != 0;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000576FC File Offset: 0x00056AFC
		public bool CanStretchRectangleFromTextures
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps2 >> 4 & 1U) != 0;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000AD RID: 173 RVA: 0x0005771C File Offset: 0x00056B1C
		public bool VertexElementScanSharesStreamOffset
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return (byte)((uint)this.caps2 >> 6 & 1U) != 0;
			}
		}

		// Token: 0x04000D4F RID: 3407
		private int caps;

		// Token: 0x04000D50 RID: 3408
		private int caps2;
	}
}

using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.DirectX.PrivateImplementationDetails;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000B8 RID: 184
	[MiscellaneousBits(1)]
	public struct Material
	{
		// Token: 0x060002AA RID: 682 RVA: 0x0005E294 File Offset: 0x0005D694
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$76$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$77$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0005E3C0 File Offset: 0x0005D7C0
		// (set) Token: 0x060002AC RID: 684 RVA: 0x0005E500 File Offset: 0x0005D900
		public unsafe Color Diffuse
		{
			get
			{
				Microsoft.DirectX.PrivateImplementationDetails.D3DXCOLOR diffuse = this.m_Diffuse;
				*(ref diffuse + 4) = *(ref this.m_Diffuse + 4);
				*(ref diffuse + 8) = *(ref this.m_Diffuse + 8);
				*(ref diffuse + 12) = *(ref this.m_Diffuse + 12);
				uint num;
				if (diffuse >= 1f)
				{
					num = 255U;
				}
				else if (diffuse <= 0f)
				{
					num = 0U;
				}
				else
				{
					num = (uint)(diffuse * 255f + 0.5f);
				}
				uint num2;
				if (*(ref diffuse + 4) >= 1f)
				{
					num2 = 255U;
				}
				else if (*(ref diffuse + 4) <= 0f)
				{
					num2 = 0U;
				}
				else
				{
					num2 = (uint)((double)(*(ref diffuse + 4) * 255f + 0.5f));
				}
				uint num3;
				if (*(ref diffuse + 8) >= 1f)
				{
					num3 = 255U;
				}
				else if (*(ref diffuse + 8) <= 0f)
				{
					num3 = 0U;
				}
				else
				{
					num3 = (uint)((double)(*(ref diffuse + 8) * 255f + 0.5f));
				}
				uint num4;
				if (*(ref diffuse + 12) >= 1f)
				{
					num4 = 255U;
				}
				else if (*(ref diffuse + 12) <= 0f)
				{
					num4 = 0U;
				}
				else
				{
					num4 = (uint)((double)(*(ref diffuse + 12) * 255f + 0.5f));
				}
				return Color.FromArgb((int)(((num4 << 8 | num) << 8 | num2) << 8 | num3));
			}
			set
			{
				*(ref this.m_Diffuse + 12) = (float)value.A * 0.003921569f;
				this.m_Diffuse = (float)value.R * 0.003921569f;
				*(ref this.m_Diffuse + 4) = (float)value.G * 0.003921569f;
				*(ref this.m_Diffuse + 8) = (float)value.B * 0.003921569f;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0005E570 File Offset: 0x0005D970
		// (set) Token: 0x060002AE RID: 686 RVA: 0x0005E6B0 File Offset: 0x0005DAB0
		public unsafe Color Specular
		{
			get
			{
				Microsoft.DirectX.PrivateImplementationDetails.D3DXCOLOR specular = this.m_Specular;
				*(ref specular + 4) = *(ref this.m_Specular + 4);
				*(ref specular + 8) = *(ref this.m_Specular + 8);
				*(ref specular + 12) = *(ref this.m_Specular + 12);
				uint num;
				if (specular >= 1f)
				{
					num = 255U;
				}
				else if (specular <= 0f)
				{
					num = 0U;
				}
				else
				{
					num = (uint)(specular * 255f + 0.5f);
				}
				uint num2;
				if (*(ref specular + 4) >= 1f)
				{
					num2 = 255U;
				}
				else if (*(ref specular + 4) <= 0f)
				{
					num2 = 0U;
				}
				else
				{
					num2 = (uint)((double)(*(ref specular + 4) * 255f + 0.5f));
				}
				uint num3;
				if (*(ref specular + 8) >= 1f)
				{
					num3 = 255U;
				}
				else if (*(ref specular + 8) <= 0f)
				{
					num3 = 0U;
				}
				else
				{
					num3 = (uint)((double)(*(ref specular + 8) * 255f + 0.5f));
				}
				uint num4;
				if (*(ref specular + 12) >= 1f)
				{
					num4 = 255U;
				}
				else if (*(ref specular + 12) <= 0f)
				{
					num4 = 0U;
				}
				else
				{
					num4 = (uint)((double)(*(ref specular + 12) * 255f + 0.5f));
				}
				return Color.FromArgb((int)(((num4 << 8 | num) << 8 | num2) << 8 | num3));
			}
			set
			{
				*(ref this.m_Specular + 12) = (float)value.A * 0.003921569f;
				this.m_Specular = (float)value.R * 0.003921569f;
				*(ref this.m_Specular + 4) = (float)value.G * 0.003921569f;
				*(ref this.m_Specular + 8) = (float)value.B * 0.003921569f;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0005E720 File Offset: 0x0005DB20
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x0005E860 File Offset: 0x0005DC60
		public unsafe Color Ambient
		{
			get
			{
				Microsoft.DirectX.PrivateImplementationDetails.D3DXCOLOR ambient = this.m_Ambient;
				*(ref ambient + 4) = *(ref this.m_Ambient + 4);
				*(ref ambient + 8) = *(ref this.m_Ambient + 8);
				*(ref ambient + 12) = *(ref this.m_Ambient + 12);
				uint num;
				if (ambient >= 1f)
				{
					num = 255U;
				}
				else if (ambient <= 0f)
				{
					num = 0U;
				}
				else
				{
					num = (uint)(ambient * 255f + 0.5f);
				}
				uint num2;
				if (*(ref ambient + 4) >= 1f)
				{
					num2 = 255U;
				}
				else if (*(ref ambient + 4) <= 0f)
				{
					num2 = 0U;
				}
				else
				{
					num2 = (uint)((double)(*(ref ambient + 4) * 255f + 0.5f));
				}
				uint num3;
				if (*(ref ambient + 8) >= 1f)
				{
					num3 = 255U;
				}
				else if (*(ref ambient + 8) <= 0f)
				{
					num3 = 0U;
				}
				else
				{
					num3 = (uint)((double)(*(ref ambient + 8) * 255f + 0.5f));
				}
				uint num4;
				if (*(ref ambient + 12) >= 1f)
				{
					num4 = 255U;
				}
				else if (*(ref ambient + 12) <= 0f)
				{
					num4 = 0U;
				}
				else
				{
					num4 = (uint)((double)(*(ref ambient + 12) * 255f + 0.5f));
				}
				return Color.FromArgb((int)(((num4 << 8 | num) << 8 | num2) << 8 | num3));
			}
			set
			{
				*(ref this.m_Ambient + 12) = (float)value.A * 0.003921569f;
				this.m_Ambient = (float)value.R * 0.003921569f;
				*(ref this.m_Ambient + 4) = (float)value.G * 0.003921569f;
				*(ref this.m_Ambient + 8) = (float)value.B * 0.003921569f;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0005E8D0 File Offset: 0x0005DCD0
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x0005EA10 File Offset: 0x0005DE10
		public unsafe Color Emissive
		{
			get
			{
				Microsoft.DirectX.PrivateImplementationDetails.D3DXCOLOR emissive = this.m_Emissive;
				*(ref emissive + 4) = *(ref this.m_Emissive + 4);
				*(ref emissive + 8) = *(ref this.m_Emissive + 8);
				*(ref emissive + 12) = *(ref this.m_Emissive + 12);
				uint num;
				if (emissive >= 1f)
				{
					num = 255U;
				}
				else if (emissive <= 0f)
				{
					num = 0U;
				}
				else
				{
					num = (uint)(emissive * 255f + 0.5f);
				}
				uint num2;
				if (*(ref emissive + 4) >= 1f)
				{
					num2 = 255U;
				}
				else if (*(ref emissive + 4) <= 0f)
				{
					num2 = 0U;
				}
				else
				{
					num2 = (uint)((double)(*(ref emissive + 4) * 255f + 0.5f));
				}
				uint num3;
				if (*(ref emissive + 8) >= 1f)
				{
					num3 = 255U;
				}
				else if (*(ref emissive + 8) <= 0f)
				{
					num3 = 0U;
				}
				else
				{
					num3 = (uint)((double)(*(ref emissive + 8) * 255f + 0.5f));
				}
				uint num4;
				if (*(ref emissive + 12) >= 1f)
				{
					num4 = 255U;
				}
				else if (*(ref emissive + 12) <= 0f)
				{
					num4 = 0U;
				}
				else
				{
					num4 = (uint)((double)(*(ref emissive + 12) * 255f + 0.5f));
				}
				return Color.FromArgb((int)(((num4 << 8 | num) << 8 | num2) << 8 | num3));
			}
			set
			{
				*(ref this.m_Emissive + 12) = (float)value.A * 0.003921569f;
				this.m_Emissive = (float)value.R * 0.003921569f;
				*(ref this.m_Emissive + 4) = (float)value.G * 0.003921569f;
				*(ref this.m_Emissive + 8) = (float)value.B * 0.003921569f;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0005EA80 File Offset: 0x0005DE80
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x0005EAA0 File Offset: 0x0005DEA0
		public ColorValue DiffuseColor
		{
			get
			{
				ref void color = ref this.m_Diffuse;
				return ColorValue.FromD3DColor(ref color);
			}
			set
			{
				ref void pColor = ref this.m_Diffuse;
				value.FillD3DColor(ref pColor);
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0005EAC4 File Offset: 0x0005DEC4
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x0005EAE4 File Offset: 0x0005DEE4
		public ColorValue SpecularColor
		{
			get
			{
				ref void color = ref this.m_Specular;
				return ColorValue.FromD3DColor(ref color);
			}
			set
			{
				ref void pColor = ref this.m_Specular;
				value.FillD3DColor(ref pColor);
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0005EB08 File Offset: 0x0005DF08
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x0005EB28 File Offset: 0x0005DF28
		public ColorValue AmbientColor
		{
			get
			{
				ref void color = ref this.m_Ambient;
				return ColorValue.FromD3DColor(ref color);
			}
			set
			{
				ref void pColor = ref this.m_Ambient;
				value.FillD3DColor(ref pColor);
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0005EB4C File Offset: 0x0005DF4C
		// (set) Token: 0x060002BA RID: 698 RVA: 0x0005EB6C File Offset: 0x0005DF6C
		public ColorValue EmissiveColor
		{
			get
			{
				ref void color = ref this.m_Emissive;
				return ColorValue.FromD3DColor(ref color);
			}
			set
			{
				ref void pColor = ref this.m_Emissive;
				value.FillD3DColor(ref pColor);
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0005EB90 File Offset: 0x0005DF90
		// (set) Token: 0x060002BC RID: 700 RVA: 0x0005EBA8 File Offset: 0x0005DFA8
		public float SpecularSharpness
		{
			get
			{
				return this.m_Power;
			}
			set
			{
				this.m_Power = value;
			}
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0005EBC4 File Offset: 0x0005DFC4
		public Material()
		{
			ref Material material& = ref this;
			initblk(ref material&, 0, 68);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0005EBE0 File Offset: 0x0005DFE0
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			if (compare == null)
			{
				return false;
			}
			if (compare.GetType() != typeof(Material))
			{
				return false;
			}
			Material material = (Material)(compare as Material);
			ref void void& = ref this;
			ref void void&2 = ref compare;
			return <Module>.memcmp(ref void&, ref void&2, 68U) == 0;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0005EC30 File Offset: 0x0005E030
		public override int GetHashCode()
		{
			Color diffuse = this.Diffuse;
			Color ambient = this.Ambient;
			Color emissive = this.Emissive;
			return this.Specular.ToArgb() ^ emissive.ToArgb() ^ ambient.ToArgb() ^ diffuse.ToArgb();
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0005EC80 File Offset: 0x0005E080
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator ==(Material left, Material right)
		{
			ref void void& = ref left;
			ref void void&2 = ref right;
			return <Module>.memcmp(ref void&, ref void&2, 68U) == 0;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0005ECA4 File Offset: 0x0005E0A4
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator !=(Material left, Material right)
		{
			ref void void& = ref left;
			ref void void&2 = ref right;
			return <Module>.memcmp(ref void&, ref void&2, 68U) != 0;
		}

		// Token: 0x04000F02 RID: 3842
		private Microsoft.DirectX.PrivateImplementationDetails._D3DCOLORVALUE m_Diffuse;

		// Token: 0x04000F03 RID: 3843
		private Microsoft.DirectX.PrivateImplementationDetails._D3DCOLORVALUE m_Ambient;

		// Token: 0x04000F04 RID: 3844
		private Microsoft.DirectX.PrivateImplementationDetails._D3DCOLORVALUE m_Specular;

		// Token: 0x04000F05 RID: 3845
		private Microsoft.DirectX.PrivateImplementationDetails._D3DCOLORVALUE m_Emissive;

		// Token: 0x04000F06 RID: 3846
		private float m_Power;
	}
}

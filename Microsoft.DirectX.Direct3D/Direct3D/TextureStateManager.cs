using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000E9 RID: 233
	public sealed class TextureStateManager
	{
		// Token: 0x0600047C RID: 1148 RVA: 0x00061DEC File Offset: 0x000611EC
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$123$];
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

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x00071090 File Offset: 0x00070490
		// (set) Token: 0x0600047E RID: 1150 RVA: 0x000710B4 File Offset: 0x000704B4
		public TextureOperation ColorOperation
		{
			get
			{
				return (TextureOperation)this.pDevice.GetTextureStageStateInt32(this.currentStageState, TextureStageStates.ColorOperation);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.ColorOperation, (int)value);
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x0600047F RID: 1151 RVA: 0x000710DC File Offset: 0x000704DC
		// (set) Token: 0x06000480 RID: 1152 RVA: 0x00071100 File Offset: 0x00070500
		public TextureArgument ColorArgument1
		{
			get
			{
				return (TextureArgument)this.pDevice.GetTextureStageStateInt32(this.currentStageState, TextureStageStates.ColorArgument1);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.ColorArgument1, (int)value);
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x00071128 File Offset: 0x00070528
		// (set) Token: 0x06000482 RID: 1154 RVA: 0x0007114C File Offset: 0x0007054C
		public TextureArgument ColorArgument2
		{
			get
			{
				return (TextureArgument)this.pDevice.GetTextureStageStateInt32(this.currentStageState, TextureStageStates.ColorArgument2);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.ColorArgument2, (int)value);
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x00071174 File Offset: 0x00070574
		// (set) Token: 0x06000484 RID: 1156 RVA: 0x00071198 File Offset: 0x00070598
		public TextureOperation AlphaOperation
		{
			get
			{
				return (TextureOperation)this.pDevice.GetTextureStageStateInt32(this.currentStageState, TextureStageStates.AlphaOperation);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.AlphaOperation, (int)value);
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x000711C0 File Offset: 0x000705C0
		// (set) Token: 0x06000486 RID: 1158 RVA: 0x000711E4 File Offset: 0x000705E4
		public TextureArgument AlphaArgument1
		{
			get
			{
				return (TextureArgument)this.pDevice.GetTextureStageStateInt32(this.currentStageState, TextureStageStates.AlphaArgument1);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.AlphaArgument1, (int)value);
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x0007120C File Offset: 0x0007060C
		// (set) Token: 0x06000488 RID: 1160 RVA: 0x00071230 File Offset: 0x00070630
		public TextureArgument AlphaArgument2
		{
			get
			{
				return (TextureArgument)this.pDevice.GetTextureStageStateInt32(this.currentStageState, TextureStageStates.AlphaArgument2);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.AlphaArgument2, (int)value);
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x00071258 File Offset: 0x00070658
		// (set) Token: 0x0600048A RID: 1162 RVA: 0x0007127C File Offset: 0x0007067C
		public float BumpEnvironmentMaterial00
		{
			get
			{
				return this.pDevice.GetTextureStageStateSingle(this.currentStageState, TextureStageStates.BumpEnvironmentMaterial00);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.BumpEnvironmentMaterial00, value);
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x000712A4 File Offset: 0x000706A4
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x000712C8 File Offset: 0x000706C8
		public float BumpEnvironmentMaterial01
		{
			get
			{
				return this.pDevice.GetTextureStageStateSingle(this.currentStageState, TextureStageStates.BumpEnvironmentMaterial01);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.BumpEnvironmentMaterial01, value);
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x000712F0 File Offset: 0x000706F0
		// (set) Token: 0x0600048E RID: 1166 RVA: 0x00071318 File Offset: 0x00070718
		public float BumpEnvironmentMaterial10
		{
			get
			{
				return this.pDevice.GetTextureStageStateSingle(this.currentStageState, TextureStageStates.BumpEnvironmentMaterial10);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.BumpEnvironmentMaterial10, value);
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x00071340 File Offset: 0x00070740
		// (set) Token: 0x06000490 RID: 1168 RVA: 0x00071368 File Offset: 0x00070768
		public float BumpEnvironmentMaterial11
		{
			get
			{
				return this.pDevice.GetTextureStageStateSingle(this.currentStageState, TextureStageStates.BumpEnvironmentMaterial11);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.BumpEnvironmentMaterial11, value);
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x00071390 File Offset: 0x00070790
		// (set) Token: 0x06000492 RID: 1170 RVA: 0x000713B8 File Offset: 0x000707B8
		public int TextureCoordinateIndex
		{
			get
			{
				return this.pDevice.GetTextureStageStateInt32(this.currentStageState, TextureStageStates.TextureCoordinateIndex);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.TextureCoordinateIndex, value);
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x000713E0 File Offset: 0x000707E0
		// (set) Token: 0x06000494 RID: 1172 RVA: 0x00071408 File Offset: 0x00070808
		public float BumpEnvironmentLuminanceScale
		{
			get
			{
				return this.pDevice.GetTextureStageStateSingle(this.currentStageState, TextureStageStates.BumpEnvironmentLuminanceScale);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.BumpEnvironmentLuminanceScale, value);
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x00071430 File Offset: 0x00070830
		// (set) Token: 0x06000496 RID: 1174 RVA: 0x00071458 File Offset: 0x00070858
		public float BumpEnvironmentLuminanceOffset
		{
			get
			{
				return this.pDevice.GetTextureStageStateSingle(this.currentStageState, TextureStageStates.BumpEnvironmentLuminanceOffset);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.BumpEnvironmentLuminanceOffset, value);
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x00071480 File Offset: 0x00070880
		// (set) Token: 0x06000498 RID: 1176 RVA: 0x000714A8 File Offset: 0x000708A8
		public TextureTransform TextureTransform
		{
			get
			{
				return (TextureTransform)this.pDevice.GetTextureStageStateInt32(this.currentStageState, TextureStageStates.TextureTransform);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.TextureTransform, (int)value);
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x000714D0 File Offset: 0x000708D0
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x000714F8 File Offset: 0x000708F8
		public TextureArgument ColorArgument0
		{
			get
			{
				return (TextureArgument)this.pDevice.GetTextureStageStateInt32(this.currentStageState, TextureStageStates.ColorArgument0);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.ColorArgument0, (int)value);
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x00071520 File Offset: 0x00070920
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x00071548 File Offset: 0x00070948
		public TextureArgument AlphaArgument0
		{
			get
			{
				return (TextureArgument)this.pDevice.GetTextureStageStateInt32(this.currentStageState, TextureStageStates.AlphaArgument0);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.AlphaArgument0, (int)value);
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x00071570 File Offset: 0x00070970
		// (set) Token: 0x0600049E RID: 1182 RVA: 0x00071598 File Offset: 0x00070998
		public TextureArgument ResultArgument
		{
			get
			{
				return (TextureArgument)this.pDevice.GetTextureStageStateInt32(this.currentStageState, TextureStageStates.ResultArgument);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.ResultArgument, (int)value);
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x000715C0 File Offset: 0x000709C0
		// (set) Token: 0x060004A0 RID: 1184 RVA: 0x000715F0 File Offset: 0x000709F0
		public Color ConstantColor
		{
			get
			{
				return Color.FromArgb(this.pDevice.GetTextureStageStateInt32(this.currentStageState, TextureStageStates.Constant));
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.Constant, value.ToArgb());
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0007161C File Offset: 0x00070A1C
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x00071644 File Offset: 0x00070A44
		public int ConstantColorValue
		{
			get
			{
				return this.pDevice.GetTextureStageStateInt32(this.currentStageState, TextureStageStates.Constant);
			}
			set
			{
				this.pDevice.SetTextureStageState(this.currentStageState, TextureStageStates.Constant, value);
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00071068 File Offset: 0x00070468
		internal TextureStateManager(Device dev, int stage)
		{
			this.pDevice = dev;
			this.currentStageState = stage;
		}

		// Token: 0x04000FED RID: 4077
		internal int currentStageState;

		// Token: 0x04000FEE RID: 4078
		internal Device pDevice;
	}
}

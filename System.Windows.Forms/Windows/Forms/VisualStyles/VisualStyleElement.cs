using System;

namespace System.Windows.Forms.VisualStyles
{
	// Token: 0x020007D0 RID: 2000
	public class VisualStyleElement
	{
		// Token: 0x06006A42 RID: 27202 RVA: 0x0018AB62 File Offset: 0x00189B62
		private VisualStyleElement(string className, int part, int state)
		{
			this.className = className;
			this.part = part;
			this.state = state;
		}

		// Token: 0x06006A43 RID: 27203 RVA: 0x0018AB7F File Offset: 0x00189B7F
		public static VisualStyleElement CreateElement(string className, int part, int state)
		{
			return new VisualStyleElement(className, part, state);
		}

		// Token: 0x17001692 RID: 5778
		// (get) Token: 0x06006A44 RID: 27204 RVA: 0x0018AB89 File Offset: 0x00189B89
		public string ClassName
		{
			get
			{
				return this.className;
			}
		}

		// Token: 0x17001693 RID: 5779
		// (get) Token: 0x06006A45 RID: 27205 RVA: 0x0018AB91 File Offset: 0x00189B91
		public int Part
		{
			get
			{
				return this.part;
			}
		}

		// Token: 0x17001694 RID: 5780
		// (get) Token: 0x06006A46 RID: 27206 RVA: 0x0018AB99 File Offset: 0x00189B99
		public int State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x04003E34 RID: 15924
		internal static readonly int Count = 25;

		// Token: 0x04003E35 RID: 15925
		private string className;

		// Token: 0x04003E36 RID: 15926
		private int part;

		// Token: 0x04003E37 RID: 15927
		private int state;

		// Token: 0x020007D1 RID: 2001
		public static class Button
		{
			// Token: 0x04003E38 RID: 15928
			private static readonly string className = "BUTTON";

			// Token: 0x020007D2 RID: 2002
			public static class PushButton
			{
				// Token: 0x17001695 RID: 5781
				// (get) Token: 0x06006A49 RID: 27209 RVA: 0x0018ABB6 File Offset: 0x00189BB6
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Button.PushButton.normal == null)
						{
							VisualStyleElement.Button.PushButton.normal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.PushButton.part, 1);
						}
						return VisualStyleElement.Button.PushButton.normal;
					}
				}

				// Token: 0x17001696 RID: 5782
				// (get) Token: 0x06006A4A RID: 27210 RVA: 0x0018ABD9 File Offset: 0x00189BD9
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Button.PushButton.hot == null)
						{
							VisualStyleElement.Button.PushButton.hot = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.PushButton.part, 2);
						}
						return VisualStyleElement.Button.PushButton.hot;
					}
				}

				// Token: 0x17001697 RID: 5783
				// (get) Token: 0x06006A4B RID: 27211 RVA: 0x0018ABFC File Offset: 0x00189BFC
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Button.PushButton.pressed == null)
						{
							VisualStyleElement.Button.PushButton.pressed = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.PushButton.part, 3);
						}
						return VisualStyleElement.Button.PushButton.pressed;
					}
				}

				// Token: 0x17001698 RID: 5784
				// (get) Token: 0x06006A4C RID: 27212 RVA: 0x0018AC1F File Offset: 0x00189C1F
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Button.PushButton.disabled == null)
						{
							VisualStyleElement.Button.PushButton.disabled = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.PushButton.part, 4);
						}
						return VisualStyleElement.Button.PushButton.disabled;
					}
				}

				// Token: 0x17001699 RID: 5785
				// (get) Token: 0x06006A4D RID: 27213 RVA: 0x0018AC42 File Offset: 0x00189C42
				public static VisualStyleElement Default
				{
					get
					{
						if (VisualStyleElement.Button.PushButton._default == null)
						{
							VisualStyleElement.Button.PushButton._default = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.PushButton.part, 5);
						}
						return VisualStyleElement.Button.PushButton._default;
					}
				}

				// Token: 0x04003E39 RID: 15929
				private static readonly int part = 1;

				// Token: 0x04003E3A RID: 15930
				private static VisualStyleElement normal;

				// Token: 0x04003E3B RID: 15931
				private static VisualStyleElement hot;

				// Token: 0x04003E3C RID: 15932
				private static VisualStyleElement pressed;

				// Token: 0x04003E3D RID: 15933
				private static VisualStyleElement disabled;

				// Token: 0x04003E3E RID: 15934
				private static VisualStyleElement _default;
			}

			// Token: 0x020007D3 RID: 2003
			public static class RadioButton
			{
				// Token: 0x1700169A RID: 5786
				// (get) Token: 0x06006A4F RID: 27215 RVA: 0x0018AC6D File Offset: 0x00189C6D
				public static VisualStyleElement UncheckedNormal
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.uncheckednormal == null)
						{
							VisualStyleElement.Button.RadioButton.uncheckednormal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 1);
						}
						return VisualStyleElement.Button.RadioButton.uncheckednormal;
					}
				}

				// Token: 0x1700169B RID: 5787
				// (get) Token: 0x06006A50 RID: 27216 RVA: 0x0018AC90 File Offset: 0x00189C90
				public static VisualStyleElement UncheckedHot
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.uncheckedhot == null)
						{
							VisualStyleElement.Button.RadioButton.uncheckedhot = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 2);
						}
						return VisualStyleElement.Button.RadioButton.uncheckedhot;
					}
				}

				// Token: 0x1700169C RID: 5788
				// (get) Token: 0x06006A51 RID: 27217 RVA: 0x0018ACB3 File Offset: 0x00189CB3
				public static VisualStyleElement UncheckedPressed
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.uncheckedpressed == null)
						{
							VisualStyleElement.Button.RadioButton.uncheckedpressed = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 3);
						}
						return VisualStyleElement.Button.RadioButton.uncheckedpressed;
					}
				}

				// Token: 0x1700169D RID: 5789
				// (get) Token: 0x06006A52 RID: 27218 RVA: 0x0018ACD6 File Offset: 0x00189CD6
				public static VisualStyleElement UncheckedDisabled
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.uncheckeddisabled == null)
						{
							VisualStyleElement.Button.RadioButton.uncheckeddisabled = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 4);
						}
						return VisualStyleElement.Button.RadioButton.uncheckeddisabled;
					}
				}

				// Token: 0x1700169E RID: 5790
				// (get) Token: 0x06006A53 RID: 27219 RVA: 0x0018ACF9 File Offset: 0x00189CF9
				public static VisualStyleElement CheckedNormal
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.checkednormal == null)
						{
							VisualStyleElement.Button.RadioButton.checkednormal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 5);
						}
						return VisualStyleElement.Button.RadioButton.checkednormal;
					}
				}

				// Token: 0x1700169F RID: 5791
				// (get) Token: 0x06006A54 RID: 27220 RVA: 0x0018AD1C File Offset: 0x00189D1C
				public static VisualStyleElement CheckedHot
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.checkedhot == null)
						{
							VisualStyleElement.Button.RadioButton.checkedhot = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 6);
						}
						return VisualStyleElement.Button.RadioButton.checkedhot;
					}
				}

				// Token: 0x170016A0 RID: 5792
				// (get) Token: 0x06006A55 RID: 27221 RVA: 0x0018AD3F File Offset: 0x00189D3F
				public static VisualStyleElement CheckedPressed
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.checkedpressed == null)
						{
							VisualStyleElement.Button.RadioButton.checkedpressed = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 7);
						}
						return VisualStyleElement.Button.RadioButton.checkedpressed;
					}
				}

				// Token: 0x170016A1 RID: 5793
				// (get) Token: 0x06006A56 RID: 27222 RVA: 0x0018AD62 File Offset: 0x00189D62
				public static VisualStyleElement CheckedDisabled
				{
					get
					{
						if (VisualStyleElement.Button.RadioButton.checkeddisabled == null)
						{
							VisualStyleElement.Button.RadioButton.checkeddisabled = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.RadioButton.part, 8);
						}
						return VisualStyleElement.Button.RadioButton.checkeddisabled;
					}
				}

				// Token: 0x04003E3F RID: 15935
				private static readonly int part = 2;

				// Token: 0x04003E40 RID: 15936
				private static VisualStyleElement uncheckednormal;

				// Token: 0x04003E41 RID: 15937
				private static VisualStyleElement uncheckedhot;

				// Token: 0x04003E42 RID: 15938
				private static VisualStyleElement uncheckedpressed;

				// Token: 0x04003E43 RID: 15939
				private static VisualStyleElement uncheckeddisabled;

				// Token: 0x04003E44 RID: 15940
				private static VisualStyleElement checkednormal;

				// Token: 0x04003E45 RID: 15941
				private static VisualStyleElement checkedhot;

				// Token: 0x04003E46 RID: 15942
				private static VisualStyleElement checkedpressed;

				// Token: 0x04003E47 RID: 15943
				private static VisualStyleElement checkeddisabled;
			}

			// Token: 0x020007D4 RID: 2004
			public static class CheckBox
			{
				// Token: 0x170016A2 RID: 5794
				// (get) Token: 0x06006A58 RID: 27224 RVA: 0x0018AD8D File Offset: 0x00189D8D
				public static VisualStyleElement UncheckedNormal
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.uncheckednormal == null)
						{
							VisualStyleElement.Button.CheckBox.uncheckednormal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 1);
						}
						return VisualStyleElement.Button.CheckBox.uncheckednormal;
					}
				}

				// Token: 0x170016A3 RID: 5795
				// (get) Token: 0x06006A59 RID: 27225 RVA: 0x0018ADB0 File Offset: 0x00189DB0
				public static VisualStyleElement UncheckedHot
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.uncheckedhot == null)
						{
							VisualStyleElement.Button.CheckBox.uncheckedhot = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 2);
						}
						return VisualStyleElement.Button.CheckBox.uncheckedhot;
					}
				}

				// Token: 0x170016A4 RID: 5796
				// (get) Token: 0x06006A5A RID: 27226 RVA: 0x0018ADD3 File Offset: 0x00189DD3
				public static VisualStyleElement UncheckedPressed
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.uncheckedpressed == null)
						{
							VisualStyleElement.Button.CheckBox.uncheckedpressed = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 3);
						}
						return VisualStyleElement.Button.CheckBox.uncheckedpressed;
					}
				}

				// Token: 0x170016A5 RID: 5797
				// (get) Token: 0x06006A5B RID: 27227 RVA: 0x0018ADF6 File Offset: 0x00189DF6
				public static VisualStyleElement UncheckedDisabled
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.uncheckeddisabled == null)
						{
							VisualStyleElement.Button.CheckBox.uncheckeddisabled = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 4);
						}
						return VisualStyleElement.Button.CheckBox.uncheckeddisabled;
					}
				}

				// Token: 0x170016A6 RID: 5798
				// (get) Token: 0x06006A5C RID: 27228 RVA: 0x0018AE19 File Offset: 0x00189E19
				public static VisualStyleElement CheckedNormal
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.checkednormal == null)
						{
							VisualStyleElement.Button.CheckBox.checkednormal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 5);
						}
						return VisualStyleElement.Button.CheckBox.checkednormal;
					}
				}

				// Token: 0x170016A7 RID: 5799
				// (get) Token: 0x06006A5D RID: 27229 RVA: 0x0018AE3C File Offset: 0x00189E3C
				public static VisualStyleElement CheckedHot
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.checkedhot == null)
						{
							VisualStyleElement.Button.CheckBox.checkedhot = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 6);
						}
						return VisualStyleElement.Button.CheckBox.checkedhot;
					}
				}

				// Token: 0x170016A8 RID: 5800
				// (get) Token: 0x06006A5E RID: 27230 RVA: 0x0018AE5F File Offset: 0x00189E5F
				public static VisualStyleElement CheckedPressed
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.checkedpressed == null)
						{
							VisualStyleElement.Button.CheckBox.checkedpressed = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 7);
						}
						return VisualStyleElement.Button.CheckBox.checkedpressed;
					}
				}

				// Token: 0x170016A9 RID: 5801
				// (get) Token: 0x06006A5F RID: 27231 RVA: 0x0018AE82 File Offset: 0x00189E82
				public static VisualStyleElement CheckedDisabled
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.checkeddisabled == null)
						{
							VisualStyleElement.Button.CheckBox.checkeddisabled = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 8);
						}
						return VisualStyleElement.Button.CheckBox.checkeddisabled;
					}
				}

				// Token: 0x170016AA RID: 5802
				// (get) Token: 0x06006A60 RID: 27232 RVA: 0x0018AEA5 File Offset: 0x00189EA5
				public static VisualStyleElement MixedNormal
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.mixednormal == null)
						{
							VisualStyleElement.Button.CheckBox.mixednormal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 9);
						}
						return VisualStyleElement.Button.CheckBox.mixednormal;
					}
				}

				// Token: 0x170016AB RID: 5803
				// (get) Token: 0x06006A61 RID: 27233 RVA: 0x0018AEC9 File Offset: 0x00189EC9
				public static VisualStyleElement MixedHot
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.mixedhot == null)
						{
							VisualStyleElement.Button.CheckBox.mixedhot = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 10);
						}
						return VisualStyleElement.Button.CheckBox.mixedhot;
					}
				}

				// Token: 0x170016AC RID: 5804
				// (get) Token: 0x06006A62 RID: 27234 RVA: 0x0018AEED File Offset: 0x00189EED
				public static VisualStyleElement MixedPressed
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.mixedpressed == null)
						{
							VisualStyleElement.Button.CheckBox.mixedpressed = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 11);
						}
						return VisualStyleElement.Button.CheckBox.mixedpressed;
					}
				}

				// Token: 0x170016AD RID: 5805
				// (get) Token: 0x06006A63 RID: 27235 RVA: 0x0018AF11 File Offset: 0x00189F11
				public static VisualStyleElement MixedDisabled
				{
					get
					{
						if (VisualStyleElement.Button.CheckBox.mixeddisabled == null)
						{
							VisualStyleElement.Button.CheckBox.mixeddisabled = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.CheckBox.part, 12);
						}
						return VisualStyleElement.Button.CheckBox.mixeddisabled;
					}
				}

				// Token: 0x04003E48 RID: 15944
				private static readonly int part = 3;

				// Token: 0x04003E49 RID: 15945
				private static VisualStyleElement uncheckednormal;

				// Token: 0x04003E4A RID: 15946
				private static VisualStyleElement uncheckedhot;

				// Token: 0x04003E4B RID: 15947
				private static VisualStyleElement uncheckedpressed;

				// Token: 0x04003E4C RID: 15948
				private static VisualStyleElement uncheckeddisabled;

				// Token: 0x04003E4D RID: 15949
				private static VisualStyleElement checkednormal;

				// Token: 0x04003E4E RID: 15950
				private static VisualStyleElement checkedhot;

				// Token: 0x04003E4F RID: 15951
				private static VisualStyleElement checkedpressed;

				// Token: 0x04003E50 RID: 15952
				private static VisualStyleElement checkeddisabled;

				// Token: 0x04003E51 RID: 15953
				private static VisualStyleElement mixednormal;

				// Token: 0x04003E52 RID: 15954
				private static VisualStyleElement mixedhot;

				// Token: 0x04003E53 RID: 15955
				private static VisualStyleElement mixedpressed;

				// Token: 0x04003E54 RID: 15956
				private static VisualStyleElement mixeddisabled;
			}

			// Token: 0x020007D5 RID: 2005
			public static class GroupBox
			{
				// Token: 0x170016AE RID: 5806
				// (get) Token: 0x06006A65 RID: 27237 RVA: 0x0018AF3D File Offset: 0x00189F3D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Button.GroupBox.normal == null)
						{
							VisualStyleElement.Button.GroupBox.normal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.GroupBox.part, 1);
						}
						return VisualStyleElement.Button.GroupBox.normal;
					}
				}

				// Token: 0x170016AF RID: 5807
				// (get) Token: 0x06006A66 RID: 27238 RVA: 0x0018AF60 File Offset: 0x00189F60
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Button.GroupBox.disabled == null)
						{
							VisualStyleElement.Button.GroupBox.disabled = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.GroupBox.part, 2);
						}
						return VisualStyleElement.Button.GroupBox.disabled;
					}
				}

				// Token: 0x04003E55 RID: 15957
				private static readonly int part = 4;

				// Token: 0x04003E56 RID: 15958
				private static VisualStyleElement normal;

				// Token: 0x04003E57 RID: 15959
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007D6 RID: 2006
			public static class UserButton
			{
				// Token: 0x170016B0 RID: 5808
				// (get) Token: 0x06006A68 RID: 27240 RVA: 0x0018AF8B File Offset: 0x00189F8B
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Button.UserButton.normal == null)
						{
							VisualStyleElement.Button.UserButton.normal = new VisualStyleElement(VisualStyleElement.Button.className, VisualStyleElement.Button.UserButton.part, 0);
						}
						return VisualStyleElement.Button.UserButton.normal;
					}
				}

				// Token: 0x04003E58 RID: 15960
				private static readonly int part = 5;

				// Token: 0x04003E59 RID: 15961
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x020007D7 RID: 2007
		public static class ComboBox
		{
			// Token: 0x04003E5A RID: 15962
			private static readonly string className = "COMBOBOX";

			// Token: 0x020007D8 RID: 2008
			public static class DropDownButton
			{
				// Token: 0x170016B1 RID: 5809
				// (get) Token: 0x06006A6B RID: 27243 RVA: 0x0018AFC2 File Offset: 0x00189FC2
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ComboBox.DropDownButton.normal == null)
						{
							VisualStyleElement.ComboBox.DropDownButton.normal = new VisualStyleElement(VisualStyleElement.ComboBox.className, VisualStyleElement.ComboBox.DropDownButton.part, 1);
						}
						return VisualStyleElement.ComboBox.DropDownButton.normal;
					}
				}

				// Token: 0x170016B2 RID: 5810
				// (get) Token: 0x06006A6C RID: 27244 RVA: 0x0018AFE5 File Offset: 0x00189FE5
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ComboBox.DropDownButton.hot == null)
						{
							VisualStyleElement.ComboBox.DropDownButton.hot = new VisualStyleElement(VisualStyleElement.ComboBox.className, VisualStyleElement.ComboBox.DropDownButton.part, 2);
						}
						return VisualStyleElement.ComboBox.DropDownButton.hot;
					}
				}

				// Token: 0x170016B3 RID: 5811
				// (get) Token: 0x06006A6D RID: 27245 RVA: 0x0018B008 File Offset: 0x0018A008
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ComboBox.DropDownButton.pressed == null)
						{
							VisualStyleElement.ComboBox.DropDownButton.pressed = new VisualStyleElement(VisualStyleElement.ComboBox.className, VisualStyleElement.ComboBox.DropDownButton.part, 3);
						}
						return VisualStyleElement.ComboBox.DropDownButton.pressed;
					}
				}

				// Token: 0x170016B4 RID: 5812
				// (get) Token: 0x06006A6E RID: 27246 RVA: 0x0018B02B File Offset: 0x0018A02B
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ComboBox.DropDownButton.disabled == null)
						{
							VisualStyleElement.ComboBox.DropDownButton.disabled = new VisualStyleElement(VisualStyleElement.ComboBox.className, VisualStyleElement.ComboBox.DropDownButton.part, 4);
						}
						return VisualStyleElement.ComboBox.DropDownButton.disabled;
					}
				}

				// Token: 0x04003E5B RID: 15963
				private static readonly int part = 1;

				// Token: 0x04003E5C RID: 15964
				private static VisualStyleElement normal;

				// Token: 0x04003E5D RID: 15965
				private static VisualStyleElement hot;

				// Token: 0x04003E5E RID: 15966
				private static VisualStyleElement pressed;

				// Token: 0x04003E5F RID: 15967
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007D9 RID: 2009
			internal static class Border
			{
				// Token: 0x170016B5 RID: 5813
				// (get) Token: 0x06006A70 RID: 27248 RVA: 0x0018B056 File Offset: 0x0018A056
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ComboBox.Border.normal == null)
						{
							VisualStyleElement.ComboBox.Border.normal = new VisualStyleElement(VisualStyleElement.ComboBox.className, 4, 3);
						}
						return VisualStyleElement.ComboBox.Border.normal;
					}
				}

				// Token: 0x04003E60 RID: 15968
				private const int part = 4;

				// Token: 0x04003E61 RID: 15969
				private static VisualStyleElement normal;
			}

			// Token: 0x020007DA RID: 2010
			internal static class ReadOnlyButton
			{
				// Token: 0x170016B6 RID: 5814
				// (get) Token: 0x06006A71 RID: 27249 RVA: 0x0018B075 File Offset: 0x0018A075
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ComboBox.ReadOnlyButton.normal == null)
						{
							VisualStyleElement.ComboBox.ReadOnlyButton.normal = new VisualStyleElement(VisualStyleElement.ComboBox.className, 5, 2);
						}
						return VisualStyleElement.ComboBox.ReadOnlyButton.normal;
					}
				}

				// Token: 0x04003E62 RID: 15970
				private const int part = 5;

				// Token: 0x04003E63 RID: 15971
				private static VisualStyleElement normal;
			}

			// Token: 0x020007DB RID: 2011
			internal static class DropDownButtonRight
			{
				// Token: 0x170016B7 RID: 5815
				// (get) Token: 0x06006A72 RID: 27250 RVA: 0x0018B094 File Offset: 0x0018A094
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ComboBox.DropDownButtonRight.normal == null)
						{
							VisualStyleElement.ComboBox.DropDownButtonRight.normal = new VisualStyleElement(VisualStyleElement.ComboBox.className, 6, 1);
						}
						return VisualStyleElement.ComboBox.DropDownButtonRight.normal;
					}
				}

				// Token: 0x04003E64 RID: 15972
				private const int part = 6;

				// Token: 0x04003E65 RID: 15973
				private static VisualStyleElement normal;
			}

			// Token: 0x020007DC RID: 2012
			internal static class DropDownButtonLeft
			{
				// Token: 0x170016B8 RID: 5816
				// (get) Token: 0x06006A73 RID: 27251 RVA: 0x0018B0B3 File Offset: 0x0018A0B3
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ComboBox.DropDownButtonLeft.normal == null)
						{
							VisualStyleElement.ComboBox.DropDownButtonLeft.normal = new VisualStyleElement(VisualStyleElement.ComboBox.className, 7, 2);
						}
						return VisualStyleElement.ComboBox.DropDownButtonLeft.normal;
					}
				}

				// Token: 0x04003E66 RID: 15974
				private const int part = 7;

				// Token: 0x04003E67 RID: 15975
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x020007DD RID: 2013
		public static class Page
		{
			// Token: 0x04003E68 RID: 15976
			private static readonly string className = "PAGE";

			// Token: 0x020007DE RID: 2014
			public static class Up
			{
				// Token: 0x170016B9 RID: 5817
				// (get) Token: 0x06006A75 RID: 27253 RVA: 0x0018B0DE File Offset: 0x0018A0DE
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Page.Up.normal == null)
						{
							VisualStyleElement.Page.Up.normal = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Up.part, 1);
						}
						return VisualStyleElement.Page.Up.normal;
					}
				}

				// Token: 0x170016BA RID: 5818
				// (get) Token: 0x06006A76 RID: 27254 RVA: 0x0018B101 File Offset: 0x0018A101
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Page.Up.hot == null)
						{
							VisualStyleElement.Page.Up.hot = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Up.part, 2);
						}
						return VisualStyleElement.Page.Up.hot;
					}
				}

				// Token: 0x170016BB RID: 5819
				// (get) Token: 0x06006A77 RID: 27255 RVA: 0x0018B124 File Offset: 0x0018A124
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Page.Up.pressed == null)
						{
							VisualStyleElement.Page.Up.pressed = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Up.part, 3);
						}
						return VisualStyleElement.Page.Up.pressed;
					}
				}

				// Token: 0x170016BC RID: 5820
				// (get) Token: 0x06006A78 RID: 27256 RVA: 0x0018B147 File Offset: 0x0018A147
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Page.Up.disabled == null)
						{
							VisualStyleElement.Page.Up.disabled = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Up.part, 4);
						}
						return VisualStyleElement.Page.Up.disabled;
					}
				}

				// Token: 0x04003E69 RID: 15977
				private static readonly int part = 1;

				// Token: 0x04003E6A RID: 15978
				private static VisualStyleElement normal;

				// Token: 0x04003E6B RID: 15979
				private static VisualStyleElement hot;

				// Token: 0x04003E6C RID: 15980
				private static VisualStyleElement pressed;

				// Token: 0x04003E6D RID: 15981
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007DF RID: 2015
			public static class Down
			{
				// Token: 0x170016BD RID: 5821
				// (get) Token: 0x06006A7A RID: 27258 RVA: 0x0018B172 File Offset: 0x0018A172
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Page.Down.normal == null)
						{
							VisualStyleElement.Page.Down.normal = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Down.part, 1);
						}
						return VisualStyleElement.Page.Down.normal;
					}
				}

				// Token: 0x170016BE RID: 5822
				// (get) Token: 0x06006A7B RID: 27259 RVA: 0x0018B195 File Offset: 0x0018A195
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Page.Down.hot == null)
						{
							VisualStyleElement.Page.Down.hot = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Down.part, 2);
						}
						return VisualStyleElement.Page.Down.hot;
					}
				}

				// Token: 0x170016BF RID: 5823
				// (get) Token: 0x06006A7C RID: 27260 RVA: 0x0018B1B8 File Offset: 0x0018A1B8
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Page.Down.pressed == null)
						{
							VisualStyleElement.Page.Down.pressed = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Down.part, 3);
						}
						return VisualStyleElement.Page.Down.pressed;
					}
				}

				// Token: 0x170016C0 RID: 5824
				// (get) Token: 0x06006A7D RID: 27261 RVA: 0x0018B1DB File Offset: 0x0018A1DB
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Page.Down.disabled == null)
						{
							VisualStyleElement.Page.Down.disabled = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.Down.part, 4);
						}
						return VisualStyleElement.Page.Down.disabled;
					}
				}

				// Token: 0x04003E6E RID: 15982
				private static readonly int part = 2;

				// Token: 0x04003E6F RID: 15983
				private static VisualStyleElement normal;

				// Token: 0x04003E70 RID: 15984
				private static VisualStyleElement hot;

				// Token: 0x04003E71 RID: 15985
				private static VisualStyleElement pressed;

				// Token: 0x04003E72 RID: 15986
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007E0 RID: 2016
			public static class UpHorizontal
			{
				// Token: 0x170016C1 RID: 5825
				// (get) Token: 0x06006A7F RID: 27263 RVA: 0x0018B206 File Offset: 0x0018A206
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Page.UpHorizontal.normal == null)
						{
							VisualStyleElement.Page.UpHorizontal.normal = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.UpHorizontal.part, 1);
						}
						return VisualStyleElement.Page.UpHorizontal.normal;
					}
				}

				// Token: 0x170016C2 RID: 5826
				// (get) Token: 0x06006A80 RID: 27264 RVA: 0x0018B229 File Offset: 0x0018A229
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Page.UpHorizontal.hot == null)
						{
							VisualStyleElement.Page.UpHorizontal.hot = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.UpHorizontal.part, 2);
						}
						return VisualStyleElement.Page.UpHorizontal.hot;
					}
				}

				// Token: 0x170016C3 RID: 5827
				// (get) Token: 0x06006A81 RID: 27265 RVA: 0x0018B24C File Offset: 0x0018A24C
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Page.UpHorizontal.pressed == null)
						{
							VisualStyleElement.Page.UpHorizontal.pressed = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.UpHorizontal.part, 3);
						}
						return VisualStyleElement.Page.UpHorizontal.pressed;
					}
				}

				// Token: 0x170016C4 RID: 5828
				// (get) Token: 0x06006A82 RID: 27266 RVA: 0x0018B26F File Offset: 0x0018A26F
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Page.UpHorizontal.disabled == null)
						{
							VisualStyleElement.Page.UpHorizontal.disabled = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.UpHorizontal.part, 4);
						}
						return VisualStyleElement.Page.UpHorizontal.disabled;
					}
				}

				// Token: 0x04003E73 RID: 15987
				private static readonly int part = 3;

				// Token: 0x04003E74 RID: 15988
				private static VisualStyleElement normal;

				// Token: 0x04003E75 RID: 15989
				private static VisualStyleElement hot;

				// Token: 0x04003E76 RID: 15990
				private static VisualStyleElement pressed;

				// Token: 0x04003E77 RID: 15991
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007E1 RID: 2017
			public static class DownHorizontal
			{
				// Token: 0x170016C5 RID: 5829
				// (get) Token: 0x06006A84 RID: 27268 RVA: 0x0018B29A File Offset: 0x0018A29A
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Page.DownHorizontal.normal == null)
						{
							VisualStyleElement.Page.DownHorizontal.normal = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.DownHorizontal.part, 1);
						}
						return VisualStyleElement.Page.DownHorizontal.normal;
					}
				}

				// Token: 0x170016C6 RID: 5830
				// (get) Token: 0x06006A85 RID: 27269 RVA: 0x0018B2BD File Offset: 0x0018A2BD
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Page.DownHorizontal.hot == null)
						{
							VisualStyleElement.Page.DownHorizontal.hot = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.DownHorizontal.part, 2);
						}
						return VisualStyleElement.Page.DownHorizontal.hot;
					}
				}

				// Token: 0x170016C7 RID: 5831
				// (get) Token: 0x06006A86 RID: 27270 RVA: 0x0018B2E0 File Offset: 0x0018A2E0
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Page.DownHorizontal.pressed == null)
						{
							VisualStyleElement.Page.DownHorizontal.pressed = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.DownHorizontal.part, 3);
						}
						return VisualStyleElement.Page.DownHorizontal.pressed;
					}
				}

				// Token: 0x170016C8 RID: 5832
				// (get) Token: 0x06006A87 RID: 27271 RVA: 0x0018B303 File Offset: 0x0018A303
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Page.DownHorizontal.disabled == null)
						{
							VisualStyleElement.Page.DownHorizontal.disabled = new VisualStyleElement(VisualStyleElement.Page.className, VisualStyleElement.Page.DownHorizontal.part, 4);
						}
						return VisualStyleElement.Page.DownHorizontal.disabled;
					}
				}

				// Token: 0x04003E78 RID: 15992
				private static readonly int part = 4;

				// Token: 0x04003E79 RID: 15993
				private static VisualStyleElement normal;

				// Token: 0x04003E7A RID: 15994
				private static VisualStyleElement hot;

				// Token: 0x04003E7B RID: 15995
				private static VisualStyleElement pressed;

				// Token: 0x04003E7C RID: 15996
				private static VisualStyleElement disabled;
			}
		}

		// Token: 0x020007E2 RID: 2018
		public static class Spin
		{
			// Token: 0x04003E7D RID: 15997
			private static readonly string className = "SPIN";

			// Token: 0x020007E3 RID: 2019
			public static class Up
			{
				// Token: 0x170016C9 RID: 5833
				// (get) Token: 0x06006A8A RID: 27274 RVA: 0x0018B33A File Offset: 0x0018A33A
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Spin.Up.normal == null)
						{
							VisualStyleElement.Spin.Up.normal = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Up.part, 1);
						}
						return VisualStyleElement.Spin.Up.normal;
					}
				}

				// Token: 0x170016CA RID: 5834
				// (get) Token: 0x06006A8B RID: 27275 RVA: 0x0018B35D File Offset: 0x0018A35D
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Spin.Up.hot == null)
						{
							VisualStyleElement.Spin.Up.hot = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Up.part, 2);
						}
						return VisualStyleElement.Spin.Up.hot;
					}
				}

				// Token: 0x170016CB RID: 5835
				// (get) Token: 0x06006A8C RID: 27276 RVA: 0x0018B380 File Offset: 0x0018A380
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Spin.Up.pressed == null)
						{
							VisualStyleElement.Spin.Up.pressed = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Up.part, 3);
						}
						return VisualStyleElement.Spin.Up.pressed;
					}
				}

				// Token: 0x170016CC RID: 5836
				// (get) Token: 0x06006A8D RID: 27277 RVA: 0x0018B3A3 File Offset: 0x0018A3A3
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Spin.Up.disabled == null)
						{
							VisualStyleElement.Spin.Up.disabled = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Up.part, 4);
						}
						return VisualStyleElement.Spin.Up.disabled;
					}
				}

				// Token: 0x04003E7E RID: 15998
				private static readonly int part = 1;

				// Token: 0x04003E7F RID: 15999
				private static VisualStyleElement normal;

				// Token: 0x04003E80 RID: 16000
				private static VisualStyleElement hot;

				// Token: 0x04003E81 RID: 16001
				private static VisualStyleElement pressed;

				// Token: 0x04003E82 RID: 16002
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007E4 RID: 2020
			public static class Down
			{
				// Token: 0x170016CD RID: 5837
				// (get) Token: 0x06006A8F RID: 27279 RVA: 0x0018B3CE File Offset: 0x0018A3CE
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Spin.Down.normal == null)
						{
							VisualStyleElement.Spin.Down.normal = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Down.part, 1);
						}
						return VisualStyleElement.Spin.Down.normal;
					}
				}

				// Token: 0x170016CE RID: 5838
				// (get) Token: 0x06006A90 RID: 27280 RVA: 0x0018B3F1 File Offset: 0x0018A3F1
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Spin.Down.hot == null)
						{
							VisualStyleElement.Spin.Down.hot = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Down.part, 2);
						}
						return VisualStyleElement.Spin.Down.hot;
					}
				}

				// Token: 0x170016CF RID: 5839
				// (get) Token: 0x06006A91 RID: 27281 RVA: 0x0018B414 File Offset: 0x0018A414
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Spin.Down.pressed == null)
						{
							VisualStyleElement.Spin.Down.pressed = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Down.part, 3);
						}
						return VisualStyleElement.Spin.Down.pressed;
					}
				}

				// Token: 0x170016D0 RID: 5840
				// (get) Token: 0x06006A92 RID: 27282 RVA: 0x0018B437 File Offset: 0x0018A437
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Spin.Down.disabled == null)
						{
							VisualStyleElement.Spin.Down.disabled = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.Down.part, 4);
						}
						return VisualStyleElement.Spin.Down.disabled;
					}
				}

				// Token: 0x04003E83 RID: 16003
				private static readonly int part = 2;

				// Token: 0x04003E84 RID: 16004
				private static VisualStyleElement normal;

				// Token: 0x04003E85 RID: 16005
				private static VisualStyleElement hot;

				// Token: 0x04003E86 RID: 16006
				private static VisualStyleElement pressed;

				// Token: 0x04003E87 RID: 16007
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007E5 RID: 2021
			public static class UpHorizontal
			{
				// Token: 0x170016D1 RID: 5841
				// (get) Token: 0x06006A94 RID: 27284 RVA: 0x0018B462 File Offset: 0x0018A462
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Spin.UpHorizontal.normal == null)
						{
							VisualStyleElement.Spin.UpHorizontal.normal = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.UpHorizontal.part, 1);
						}
						return VisualStyleElement.Spin.UpHorizontal.normal;
					}
				}

				// Token: 0x170016D2 RID: 5842
				// (get) Token: 0x06006A95 RID: 27285 RVA: 0x0018B485 File Offset: 0x0018A485
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Spin.UpHorizontal.hot == null)
						{
							VisualStyleElement.Spin.UpHorizontal.hot = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.UpHorizontal.part, 2);
						}
						return VisualStyleElement.Spin.UpHorizontal.hot;
					}
				}

				// Token: 0x170016D3 RID: 5843
				// (get) Token: 0x06006A96 RID: 27286 RVA: 0x0018B4A8 File Offset: 0x0018A4A8
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Spin.UpHorizontal.pressed == null)
						{
							VisualStyleElement.Spin.UpHorizontal.pressed = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.UpHorizontal.part, 3);
						}
						return VisualStyleElement.Spin.UpHorizontal.pressed;
					}
				}

				// Token: 0x170016D4 RID: 5844
				// (get) Token: 0x06006A97 RID: 27287 RVA: 0x0018B4CB File Offset: 0x0018A4CB
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Spin.UpHorizontal.disabled == null)
						{
							VisualStyleElement.Spin.UpHorizontal.disabled = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.UpHorizontal.part, 4);
						}
						return VisualStyleElement.Spin.UpHorizontal.disabled;
					}
				}

				// Token: 0x04003E88 RID: 16008
				private static readonly int part = 3;

				// Token: 0x04003E89 RID: 16009
				private static VisualStyleElement normal;

				// Token: 0x04003E8A RID: 16010
				private static VisualStyleElement hot;

				// Token: 0x04003E8B RID: 16011
				private static VisualStyleElement pressed;

				// Token: 0x04003E8C RID: 16012
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007E6 RID: 2022
			public static class DownHorizontal
			{
				// Token: 0x170016D5 RID: 5845
				// (get) Token: 0x06006A99 RID: 27289 RVA: 0x0018B4F6 File Offset: 0x0018A4F6
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Spin.DownHorizontal.normal == null)
						{
							VisualStyleElement.Spin.DownHorizontal.normal = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.DownHorizontal.part, 1);
						}
						return VisualStyleElement.Spin.DownHorizontal.normal;
					}
				}

				// Token: 0x170016D6 RID: 5846
				// (get) Token: 0x06006A9A RID: 27290 RVA: 0x0018B519 File Offset: 0x0018A519
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Spin.DownHorizontal.hot == null)
						{
							VisualStyleElement.Spin.DownHorizontal.hot = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.DownHorizontal.part, 2);
						}
						return VisualStyleElement.Spin.DownHorizontal.hot;
					}
				}

				// Token: 0x170016D7 RID: 5847
				// (get) Token: 0x06006A9B RID: 27291 RVA: 0x0018B53C File Offset: 0x0018A53C
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Spin.DownHorizontal.pressed == null)
						{
							VisualStyleElement.Spin.DownHorizontal.pressed = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.DownHorizontal.part, 3);
						}
						return VisualStyleElement.Spin.DownHorizontal.pressed;
					}
				}

				// Token: 0x170016D8 RID: 5848
				// (get) Token: 0x06006A9C RID: 27292 RVA: 0x0018B55F File Offset: 0x0018A55F
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Spin.DownHorizontal.disabled == null)
						{
							VisualStyleElement.Spin.DownHorizontal.disabled = new VisualStyleElement(VisualStyleElement.Spin.className, VisualStyleElement.Spin.DownHorizontal.part, 4);
						}
						return VisualStyleElement.Spin.DownHorizontal.disabled;
					}
				}

				// Token: 0x04003E8D RID: 16013
				private static readonly int part = 4;

				// Token: 0x04003E8E RID: 16014
				private static VisualStyleElement normal;

				// Token: 0x04003E8F RID: 16015
				private static VisualStyleElement hot;

				// Token: 0x04003E90 RID: 16016
				private static VisualStyleElement pressed;

				// Token: 0x04003E91 RID: 16017
				private static VisualStyleElement disabled;
			}
		}

		// Token: 0x020007E7 RID: 2023
		public static class ScrollBar
		{
			// Token: 0x04003E92 RID: 16018
			private static readonly string className = "SCROLLBAR";

			// Token: 0x020007E8 RID: 2024
			public static class ArrowButton
			{
				// Token: 0x170016D9 RID: 5849
				// (get) Token: 0x06006A9F RID: 27295 RVA: 0x0018B596 File Offset: 0x0018A596
				public static VisualStyleElement UpNormal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.upnormal == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.upnormal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 1);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.upnormal;
					}
				}

				// Token: 0x170016DA RID: 5850
				// (get) Token: 0x06006AA0 RID: 27296 RVA: 0x0018B5B9 File Offset: 0x0018A5B9
				public static VisualStyleElement UpHot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.uphot == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.uphot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 2);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.uphot;
					}
				}

				// Token: 0x170016DB RID: 5851
				// (get) Token: 0x06006AA1 RID: 27297 RVA: 0x0018B5DC File Offset: 0x0018A5DC
				public static VisualStyleElement UpPressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.uppressed == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.uppressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 3);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.uppressed;
					}
				}

				// Token: 0x170016DC RID: 5852
				// (get) Token: 0x06006AA2 RID: 27298 RVA: 0x0018B5FF File Offset: 0x0018A5FF
				public static VisualStyleElement UpDisabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.updisabled == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.updisabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 4);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.updisabled;
					}
				}

				// Token: 0x170016DD RID: 5853
				// (get) Token: 0x06006AA3 RID: 27299 RVA: 0x0018B622 File Offset: 0x0018A622
				public static VisualStyleElement DownNormal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.downnormal == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.downnormal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 5);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.downnormal;
					}
				}

				// Token: 0x170016DE RID: 5854
				// (get) Token: 0x06006AA4 RID: 27300 RVA: 0x0018B645 File Offset: 0x0018A645
				public static VisualStyleElement DownHot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.downhot == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.downhot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 6);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.downhot;
					}
				}

				// Token: 0x170016DF RID: 5855
				// (get) Token: 0x06006AA5 RID: 27301 RVA: 0x0018B668 File Offset: 0x0018A668
				public static VisualStyleElement DownPressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.downpressed == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.downpressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 7);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.downpressed;
					}
				}

				// Token: 0x170016E0 RID: 5856
				// (get) Token: 0x06006AA6 RID: 27302 RVA: 0x0018B68B File Offset: 0x0018A68B
				public static VisualStyleElement DownDisabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.downdisabled == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.downdisabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 8);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.downdisabled;
					}
				}

				// Token: 0x170016E1 RID: 5857
				// (get) Token: 0x06006AA7 RID: 27303 RVA: 0x0018B6AE File Offset: 0x0018A6AE
				public static VisualStyleElement LeftNormal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.leftnormal == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.leftnormal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 9);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.leftnormal;
					}
				}

				// Token: 0x170016E2 RID: 5858
				// (get) Token: 0x06006AA8 RID: 27304 RVA: 0x0018B6D2 File Offset: 0x0018A6D2
				public static VisualStyleElement LeftHot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.lefthot == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.lefthot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 10);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.lefthot;
					}
				}

				// Token: 0x170016E3 RID: 5859
				// (get) Token: 0x06006AA9 RID: 27305 RVA: 0x0018B6F6 File Offset: 0x0018A6F6
				public static VisualStyleElement LeftPressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.leftpressed == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.leftpressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 11);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.leftpressed;
					}
				}

				// Token: 0x170016E4 RID: 5860
				// (get) Token: 0x06006AAA RID: 27306 RVA: 0x0018B71A File Offset: 0x0018A71A
				public static VisualStyleElement LeftDisabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.leftdisabled == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.leftdisabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 12);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.leftdisabled;
					}
				}

				// Token: 0x170016E5 RID: 5861
				// (get) Token: 0x06006AAB RID: 27307 RVA: 0x0018B73E File Offset: 0x0018A73E
				public static VisualStyleElement RightNormal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.rightnormal == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.rightnormal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 13);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.rightnormal;
					}
				}

				// Token: 0x170016E6 RID: 5862
				// (get) Token: 0x06006AAC RID: 27308 RVA: 0x0018B762 File Offset: 0x0018A762
				public static VisualStyleElement RightHot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.righthot == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.righthot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 14);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.righthot;
					}
				}

				// Token: 0x170016E7 RID: 5863
				// (get) Token: 0x06006AAD RID: 27309 RVA: 0x0018B786 File Offset: 0x0018A786
				public static VisualStyleElement RightPressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.rightpressed == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.rightpressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 15);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.rightpressed;
					}
				}

				// Token: 0x170016E8 RID: 5864
				// (get) Token: 0x06006AAE RID: 27310 RVA: 0x0018B7AA File Offset: 0x0018A7AA
				public static VisualStyleElement RightDisabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ArrowButton.rightdisabled == null)
						{
							VisualStyleElement.ScrollBar.ArrowButton.rightdisabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ArrowButton.part, 16);
						}
						return VisualStyleElement.ScrollBar.ArrowButton.rightdisabled;
					}
				}

				// Token: 0x04003E93 RID: 16019
				private static readonly int part = 1;

				// Token: 0x04003E94 RID: 16020
				private static VisualStyleElement upnormal;

				// Token: 0x04003E95 RID: 16021
				private static VisualStyleElement uphot;

				// Token: 0x04003E96 RID: 16022
				private static VisualStyleElement uppressed;

				// Token: 0x04003E97 RID: 16023
				private static VisualStyleElement updisabled;

				// Token: 0x04003E98 RID: 16024
				private static VisualStyleElement downnormal;

				// Token: 0x04003E99 RID: 16025
				private static VisualStyleElement downhot;

				// Token: 0x04003E9A RID: 16026
				private static VisualStyleElement downpressed;

				// Token: 0x04003E9B RID: 16027
				private static VisualStyleElement downdisabled;

				// Token: 0x04003E9C RID: 16028
				private static VisualStyleElement leftnormal;

				// Token: 0x04003E9D RID: 16029
				private static VisualStyleElement lefthot;

				// Token: 0x04003E9E RID: 16030
				private static VisualStyleElement leftpressed;

				// Token: 0x04003E9F RID: 16031
				private static VisualStyleElement leftdisabled;

				// Token: 0x04003EA0 RID: 16032
				private static VisualStyleElement rightnormal;

				// Token: 0x04003EA1 RID: 16033
				private static VisualStyleElement righthot;

				// Token: 0x04003EA2 RID: 16034
				private static VisualStyleElement rightpressed;

				// Token: 0x04003EA3 RID: 16035
				private static VisualStyleElement rightdisabled;
			}

			// Token: 0x020007E9 RID: 2025
			public static class ThumbButtonHorizontal
			{
				// Token: 0x170016E9 RID: 5865
				// (get) Token: 0x06006AB0 RID: 27312 RVA: 0x0018B7D6 File Offset: 0x0018A7D6
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonHorizontal.normal == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonHorizontal.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonHorizontal.part, 1);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonHorizontal.normal;
					}
				}

				// Token: 0x170016EA RID: 5866
				// (get) Token: 0x06006AB1 RID: 27313 RVA: 0x0018B7F9 File Offset: 0x0018A7F9
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonHorizontal.hot == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonHorizontal.hot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonHorizontal.part, 2);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonHorizontal.hot;
					}
				}

				// Token: 0x170016EB RID: 5867
				// (get) Token: 0x06006AB2 RID: 27314 RVA: 0x0018B81C File Offset: 0x0018A81C
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonHorizontal.pressed == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonHorizontal.pressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonHorizontal.part, 3);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonHorizontal.pressed;
					}
				}

				// Token: 0x170016EC RID: 5868
				// (get) Token: 0x06006AB3 RID: 27315 RVA: 0x0018B83F File Offset: 0x0018A83F
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonHorizontal.disabled == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonHorizontal.disabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonHorizontal.part, 4);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonHorizontal.disabled;
					}
				}

				// Token: 0x04003EA4 RID: 16036
				private static readonly int part = 2;

				// Token: 0x04003EA5 RID: 16037
				private static VisualStyleElement normal;

				// Token: 0x04003EA6 RID: 16038
				private static VisualStyleElement hot;

				// Token: 0x04003EA7 RID: 16039
				private static VisualStyleElement pressed;

				// Token: 0x04003EA8 RID: 16040
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007EA RID: 2026
			public static class ThumbButtonVertical
			{
				// Token: 0x170016ED RID: 5869
				// (get) Token: 0x06006AB5 RID: 27317 RVA: 0x0018B86A File Offset: 0x0018A86A
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonVertical.normal == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonVertical.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonVertical.part, 1);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonVertical.normal;
					}
				}

				// Token: 0x170016EE RID: 5870
				// (get) Token: 0x06006AB6 RID: 27318 RVA: 0x0018B88D File Offset: 0x0018A88D
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonVertical.hot == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonVertical.hot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonVertical.part, 2);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonVertical.hot;
					}
				}

				// Token: 0x170016EF RID: 5871
				// (get) Token: 0x06006AB7 RID: 27319 RVA: 0x0018B8B0 File Offset: 0x0018A8B0
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonVertical.pressed == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonVertical.pressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonVertical.part, 3);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonVertical.pressed;
					}
				}

				// Token: 0x170016F0 RID: 5872
				// (get) Token: 0x06006AB8 RID: 27320 RVA: 0x0018B8D3 File Offset: 0x0018A8D3
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.ThumbButtonVertical.disabled == null)
						{
							VisualStyleElement.ScrollBar.ThumbButtonVertical.disabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.ThumbButtonVertical.part, 4);
						}
						return VisualStyleElement.ScrollBar.ThumbButtonVertical.disabled;
					}
				}

				// Token: 0x04003EA9 RID: 16041
				private static readonly int part = 3;

				// Token: 0x04003EAA RID: 16042
				private static VisualStyleElement normal;

				// Token: 0x04003EAB RID: 16043
				private static VisualStyleElement hot;

				// Token: 0x04003EAC RID: 16044
				private static VisualStyleElement pressed;

				// Token: 0x04003EAD RID: 16045
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007EB RID: 2027
			public static class RightTrackHorizontal
			{
				// Token: 0x170016F1 RID: 5873
				// (get) Token: 0x06006ABA RID: 27322 RVA: 0x0018B8FE File Offset: 0x0018A8FE
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.RightTrackHorizontal.normal == null)
						{
							VisualStyleElement.ScrollBar.RightTrackHorizontal.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.RightTrackHorizontal.part, 1);
						}
						return VisualStyleElement.ScrollBar.RightTrackHorizontal.normal;
					}
				}

				// Token: 0x170016F2 RID: 5874
				// (get) Token: 0x06006ABB RID: 27323 RVA: 0x0018B921 File Offset: 0x0018A921
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.RightTrackHorizontal.hot == null)
						{
							VisualStyleElement.ScrollBar.RightTrackHorizontal.hot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.RightTrackHorizontal.part, 2);
						}
						return VisualStyleElement.ScrollBar.RightTrackHorizontal.hot;
					}
				}

				// Token: 0x170016F3 RID: 5875
				// (get) Token: 0x06006ABC RID: 27324 RVA: 0x0018B944 File Offset: 0x0018A944
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.RightTrackHorizontal.pressed == null)
						{
							VisualStyleElement.ScrollBar.RightTrackHorizontal.pressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.RightTrackHorizontal.part, 3);
						}
						return VisualStyleElement.ScrollBar.RightTrackHorizontal.pressed;
					}
				}

				// Token: 0x170016F4 RID: 5876
				// (get) Token: 0x06006ABD RID: 27325 RVA: 0x0018B967 File Offset: 0x0018A967
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.RightTrackHorizontal.disabled == null)
						{
							VisualStyleElement.ScrollBar.RightTrackHorizontal.disabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.RightTrackHorizontal.part, 4);
						}
						return VisualStyleElement.ScrollBar.RightTrackHorizontal.disabled;
					}
				}

				// Token: 0x04003EAE RID: 16046
				private static readonly int part = 4;

				// Token: 0x04003EAF RID: 16047
				private static VisualStyleElement normal;

				// Token: 0x04003EB0 RID: 16048
				private static VisualStyleElement hot;

				// Token: 0x04003EB1 RID: 16049
				private static VisualStyleElement pressed;

				// Token: 0x04003EB2 RID: 16050
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007EC RID: 2028
			public static class LeftTrackHorizontal
			{
				// Token: 0x170016F5 RID: 5877
				// (get) Token: 0x06006ABF RID: 27327 RVA: 0x0018B992 File Offset: 0x0018A992
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LeftTrackHorizontal.normal == null)
						{
							VisualStyleElement.ScrollBar.LeftTrackHorizontal.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LeftTrackHorizontal.part, 1);
						}
						return VisualStyleElement.ScrollBar.LeftTrackHorizontal.normal;
					}
				}

				// Token: 0x170016F6 RID: 5878
				// (get) Token: 0x06006AC0 RID: 27328 RVA: 0x0018B9B5 File Offset: 0x0018A9B5
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LeftTrackHorizontal.hot == null)
						{
							VisualStyleElement.ScrollBar.LeftTrackHorizontal.hot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LeftTrackHorizontal.part, 2);
						}
						return VisualStyleElement.ScrollBar.LeftTrackHorizontal.hot;
					}
				}

				// Token: 0x170016F7 RID: 5879
				// (get) Token: 0x06006AC1 RID: 27329 RVA: 0x0018B9D8 File Offset: 0x0018A9D8
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LeftTrackHorizontal.pressed == null)
						{
							VisualStyleElement.ScrollBar.LeftTrackHorizontal.pressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LeftTrackHorizontal.part, 3);
						}
						return VisualStyleElement.ScrollBar.LeftTrackHorizontal.pressed;
					}
				}

				// Token: 0x170016F8 RID: 5880
				// (get) Token: 0x06006AC2 RID: 27330 RVA: 0x0018B9FB File Offset: 0x0018A9FB
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LeftTrackHorizontal.disabled == null)
						{
							VisualStyleElement.ScrollBar.LeftTrackHorizontal.disabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LeftTrackHorizontal.part, 4);
						}
						return VisualStyleElement.ScrollBar.LeftTrackHorizontal.disabled;
					}
				}

				// Token: 0x04003EB3 RID: 16051
				private static readonly int part = 5;

				// Token: 0x04003EB4 RID: 16052
				private static VisualStyleElement normal;

				// Token: 0x04003EB5 RID: 16053
				private static VisualStyleElement hot;

				// Token: 0x04003EB6 RID: 16054
				private static VisualStyleElement pressed;

				// Token: 0x04003EB7 RID: 16055
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007ED RID: 2029
			public static class LowerTrackVertical
			{
				// Token: 0x170016F9 RID: 5881
				// (get) Token: 0x06006AC4 RID: 27332 RVA: 0x0018BA26 File Offset: 0x0018AA26
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LowerTrackVertical.normal == null)
						{
							VisualStyleElement.ScrollBar.LowerTrackVertical.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LowerTrackVertical.part, 1);
						}
						return VisualStyleElement.ScrollBar.LowerTrackVertical.normal;
					}
				}

				// Token: 0x170016FA RID: 5882
				// (get) Token: 0x06006AC5 RID: 27333 RVA: 0x0018BA49 File Offset: 0x0018AA49
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LowerTrackVertical.hot == null)
						{
							VisualStyleElement.ScrollBar.LowerTrackVertical.hot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LowerTrackVertical.part, 2);
						}
						return VisualStyleElement.ScrollBar.LowerTrackVertical.hot;
					}
				}

				// Token: 0x170016FB RID: 5883
				// (get) Token: 0x06006AC6 RID: 27334 RVA: 0x0018BA6C File Offset: 0x0018AA6C
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LowerTrackVertical.pressed == null)
						{
							VisualStyleElement.ScrollBar.LowerTrackVertical.pressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LowerTrackVertical.part, 3);
						}
						return VisualStyleElement.ScrollBar.LowerTrackVertical.pressed;
					}
				}

				// Token: 0x170016FC RID: 5884
				// (get) Token: 0x06006AC7 RID: 27335 RVA: 0x0018BA8F File Offset: 0x0018AA8F
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.LowerTrackVertical.disabled == null)
						{
							VisualStyleElement.ScrollBar.LowerTrackVertical.disabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.LowerTrackVertical.part, 4);
						}
						return VisualStyleElement.ScrollBar.LowerTrackVertical.disabled;
					}
				}

				// Token: 0x04003EB8 RID: 16056
				private static readonly int part = 6;

				// Token: 0x04003EB9 RID: 16057
				private static VisualStyleElement normal;

				// Token: 0x04003EBA RID: 16058
				private static VisualStyleElement hot;

				// Token: 0x04003EBB RID: 16059
				private static VisualStyleElement pressed;

				// Token: 0x04003EBC RID: 16060
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007EE RID: 2030
			public static class UpperTrackVertical
			{
				// Token: 0x170016FD RID: 5885
				// (get) Token: 0x06006AC9 RID: 27337 RVA: 0x0018BABA File Offset: 0x0018AABA
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.UpperTrackVertical.normal == null)
						{
							VisualStyleElement.ScrollBar.UpperTrackVertical.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.UpperTrackVertical.part, 1);
						}
						return VisualStyleElement.ScrollBar.UpperTrackVertical.normal;
					}
				}

				// Token: 0x170016FE RID: 5886
				// (get) Token: 0x06006ACA RID: 27338 RVA: 0x0018BADD File Offset: 0x0018AADD
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ScrollBar.UpperTrackVertical.hot == null)
						{
							VisualStyleElement.ScrollBar.UpperTrackVertical.hot = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.UpperTrackVertical.part, 2);
						}
						return VisualStyleElement.ScrollBar.UpperTrackVertical.hot;
					}
				}

				// Token: 0x170016FF RID: 5887
				// (get) Token: 0x06006ACB RID: 27339 RVA: 0x0018BB00 File Offset: 0x0018AB00
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ScrollBar.UpperTrackVertical.pressed == null)
						{
							VisualStyleElement.ScrollBar.UpperTrackVertical.pressed = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.UpperTrackVertical.part, 3);
						}
						return VisualStyleElement.ScrollBar.UpperTrackVertical.pressed;
					}
				}

				// Token: 0x17001700 RID: 5888
				// (get) Token: 0x06006ACC RID: 27340 RVA: 0x0018BB23 File Offset: 0x0018AB23
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ScrollBar.UpperTrackVertical.disabled == null)
						{
							VisualStyleElement.ScrollBar.UpperTrackVertical.disabled = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.UpperTrackVertical.part, 4);
						}
						return VisualStyleElement.ScrollBar.UpperTrackVertical.disabled;
					}
				}

				// Token: 0x04003EBD RID: 16061
				private static readonly int part = 7;

				// Token: 0x04003EBE RID: 16062
				private static VisualStyleElement normal;

				// Token: 0x04003EBF RID: 16063
				private static VisualStyleElement hot;

				// Token: 0x04003EC0 RID: 16064
				private static VisualStyleElement pressed;

				// Token: 0x04003EC1 RID: 16065
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007EF RID: 2031
			public static class GripperHorizontal
			{
				// Token: 0x17001701 RID: 5889
				// (get) Token: 0x06006ACE RID: 27342 RVA: 0x0018BB4E File Offset: 0x0018AB4E
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.GripperHorizontal.normal == null)
						{
							VisualStyleElement.ScrollBar.GripperHorizontal.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.GripperHorizontal.part, 0);
						}
						return VisualStyleElement.ScrollBar.GripperHorizontal.normal;
					}
				}

				// Token: 0x04003EC2 RID: 16066
				private static readonly int part = 8;

				// Token: 0x04003EC3 RID: 16067
				private static VisualStyleElement normal;
			}

			// Token: 0x020007F0 RID: 2032
			public static class GripperVertical
			{
				// Token: 0x17001702 RID: 5890
				// (get) Token: 0x06006AD0 RID: 27344 RVA: 0x0018BB79 File Offset: 0x0018AB79
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ScrollBar.GripperVertical.normal == null)
						{
							VisualStyleElement.ScrollBar.GripperVertical.normal = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.GripperVertical.part, 0);
						}
						return VisualStyleElement.ScrollBar.GripperVertical.normal;
					}
				}

				// Token: 0x04003EC4 RID: 16068
				private static readonly int part = 9;

				// Token: 0x04003EC5 RID: 16069
				private static VisualStyleElement normal;
			}

			// Token: 0x020007F1 RID: 2033
			public static class SizeBox
			{
				// Token: 0x17001703 RID: 5891
				// (get) Token: 0x06006AD2 RID: 27346 RVA: 0x0018BBA5 File Offset: 0x0018ABA5
				public static VisualStyleElement RightAlign
				{
					get
					{
						if (VisualStyleElement.ScrollBar.SizeBox.rightalign == null)
						{
							VisualStyleElement.ScrollBar.SizeBox.rightalign = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.SizeBox.part, 1);
						}
						return VisualStyleElement.ScrollBar.SizeBox.rightalign;
					}
				}

				// Token: 0x17001704 RID: 5892
				// (get) Token: 0x06006AD3 RID: 27347 RVA: 0x0018BBC8 File Offset: 0x0018ABC8
				public static VisualStyleElement LeftAlign
				{
					get
					{
						if (VisualStyleElement.ScrollBar.SizeBox.leftalign == null)
						{
							VisualStyleElement.ScrollBar.SizeBox.leftalign = new VisualStyleElement(VisualStyleElement.ScrollBar.className, VisualStyleElement.ScrollBar.SizeBox.part, 2);
						}
						return VisualStyleElement.ScrollBar.SizeBox.leftalign;
					}
				}

				// Token: 0x04003EC6 RID: 16070
				private static readonly int part = 10;

				// Token: 0x04003EC7 RID: 16071
				private static VisualStyleElement rightalign;

				// Token: 0x04003EC8 RID: 16072
				private static VisualStyleElement leftalign;
			}
		}

		// Token: 0x020007F2 RID: 2034
		public static class Tab
		{
			// Token: 0x04003EC9 RID: 16073
			private static readonly string className = "TAB";

			// Token: 0x020007F3 RID: 2035
			public static class TabItem
			{
				// Token: 0x17001705 RID: 5893
				// (get) Token: 0x06006AD6 RID: 27350 RVA: 0x0018BC00 File Offset: 0x0018AC00
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TabItem.normal == null)
						{
							VisualStyleElement.Tab.TabItem.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItem.part, 1);
						}
						return VisualStyleElement.Tab.TabItem.normal;
					}
				}

				// Token: 0x17001706 RID: 5894
				// (get) Token: 0x06006AD7 RID: 27351 RVA: 0x0018BC23 File Offset: 0x0018AC23
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Tab.TabItem.hot == null)
						{
							VisualStyleElement.Tab.TabItem.hot = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItem.part, 2);
						}
						return VisualStyleElement.Tab.TabItem.hot;
					}
				}

				// Token: 0x17001707 RID: 5895
				// (get) Token: 0x06006AD8 RID: 27352 RVA: 0x0018BC46 File Offset: 0x0018AC46
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Tab.TabItem.pressed == null)
						{
							VisualStyleElement.Tab.TabItem.pressed = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItem.part, 3);
						}
						return VisualStyleElement.Tab.TabItem.pressed;
					}
				}

				// Token: 0x17001708 RID: 5896
				// (get) Token: 0x06006AD9 RID: 27353 RVA: 0x0018BC69 File Offset: 0x0018AC69
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Tab.TabItem.disabled == null)
						{
							VisualStyleElement.Tab.TabItem.disabled = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItem.part, 4);
						}
						return VisualStyleElement.Tab.TabItem.disabled;
					}
				}

				// Token: 0x04003ECA RID: 16074
				private static readonly int part = 1;

				// Token: 0x04003ECB RID: 16075
				private static VisualStyleElement normal;

				// Token: 0x04003ECC RID: 16076
				private static VisualStyleElement hot;

				// Token: 0x04003ECD RID: 16077
				private static VisualStyleElement pressed;

				// Token: 0x04003ECE RID: 16078
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007F4 RID: 2036
			public static class TabItemLeftEdge
			{
				// Token: 0x17001709 RID: 5897
				// (get) Token: 0x06006ADB RID: 27355 RVA: 0x0018BC94 File Offset: 0x0018AC94
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemLeftEdge.normal == null)
						{
							VisualStyleElement.Tab.TabItemLeftEdge.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemLeftEdge.part, 1);
						}
						return VisualStyleElement.Tab.TabItemLeftEdge.normal;
					}
				}

				// Token: 0x1700170A RID: 5898
				// (get) Token: 0x06006ADC RID: 27356 RVA: 0x0018BCB7 File Offset: 0x0018ACB7
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemLeftEdge.hot == null)
						{
							VisualStyleElement.Tab.TabItemLeftEdge.hot = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemLeftEdge.part, 2);
						}
						return VisualStyleElement.Tab.TabItemLeftEdge.hot;
					}
				}

				// Token: 0x1700170B RID: 5899
				// (get) Token: 0x06006ADD RID: 27357 RVA: 0x0018BCDA File Offset: 0x0018ACDA
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemLeftEdge.pressed == null)
						{
							VisualStyleElement.Tab.TabItemLeftEdge.pressed = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemLeftEdge.part, 3);
						}
						return VisualStyleElement.Tab.TabItemLeftEdge.pressed;
					}
				}

				// Token: 0x1700170C RID: 5900
				// (get) Token: 0x06006ADE RID: 27358 RVA: 0x0018BCFD File Offset: 0x0018ACFD
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemLeftEdge.disabled == null)
						{
							VisualStyleElement.Tab.TabItemLeftEdge.disabled = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemLeftEdge.part, 4);
						}
						return VisualStyleElement.Tab.TabItemLeftEdge.disabled;
					}
				}

				// Token: 0x04003ECF RID: 16079
				private static readonly int part = 2;

				// Token: 0x04003ED0 RID: 16080
				private static VisualStyleElement normal;

				// Token: 0x04003ED1 RID: 16081
				private static VisualStyleElement hot;

				// Token: 0x04003ED2 RID: 16082
				private static VisualStyleElement pressed;

				// Token: 0x04003ED3 RID: 16083
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007F5 RID: 2037
			public static class TabItemRightEdge
			{
				// Token: 0x1700170D RID: 5901
				// (get) Token: 0x06006AE0 RID: 27360 RVA: 0x0018BD28 File Offset: 0x0018AD28
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemRightEdge.normal == null)
						{
							VisualStyleElement.Tab.TabItemRightEdge.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemRightEdge.part, 1);
						}
						return VisualStyleElement.Tab.TabItemRightEdge.normal;
					}
				}

				// Token: 0x1700170E RID: 5902
				// (get) Token: 0x06006AE1 RID: 27361 RVA: 0x0018BD4B File Offset: 0x0018AD4B
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemRightEdge.hot == null)
						{
							VisualStyleElement.Tab.TabItemRightEdge.hot = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemRightEdge.part, 2);
						}
						return VisualStyleElement.Tab.TabItemRightEdge.hot;
					}
				}

				// Token: 0x1700170F RID: 5903
				// (get) Token: 0x06006AE2 RID: 27362 RVA: 0x0018BD6E File Offset: 0x0018AD6E
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemRightEdge.pressed == null)
						{
							VisualStyleElement.Tab.TabItemRightEdge.pressed = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemRightEdge.part, 3);
						}
						return VisualStyleElement.Tab.TabItemRightEdge.pressed;
					}
				}

				// Token: 0x17001710 RID: 5904
				// (get) Token: 0x06006AE3 RID: 27363 RVA: 0x0018BD91 File Offset: 0x0018AD91
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemRightEdge.disabled == null)
						{
							VisualStyleElement.Tab.TabItemRightEdge.disabled = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemRightEdge.part, 4);
						}
						return VisualStyleElement.Tab.TabItemRightEdge.disabled;
					}
				}

				// Token: 0x04003ED4 RID: 16084
				private static readonly int part = 3;

				// Token: 0x04003ED5 RID: 16085
				private static VisualStyleElement normal;

				// Token: 0x04003ED6 RID: 16086
				private static VisualStyleElement hot;

				// Token: 0x04003ED7 RID: 16087
				private static VisualStyleElement pressed;

				// Token: 0x04003ED8 RID: 16088
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007F6 RID: 2038
			public static class TabItemBothEdges
			{
				// Token: 0x17001711 RID: 5905
				// (get) Token: 0x06006AE5 RID: 27365 RVA: 0x0018BDBC File Offset: 0x0018ADBC
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TabItemBothEdges.normal == null)
						{
							VisualStyleElement.Tab.TabItemBothEdges.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TabItemBothEdges.part, 0);
						}
						return VisualStyleElement.Tab.TabItemBothEdges.normal;
					}
				}

				// Token: 0x04003ED9 RID: 16089
				private static readonly int part = 4;

				// Token: 0x04003EDA RID: 16090
				private static VisualStyleElement normal;
			}

			// Token: 0x020007F7 RID: 2039
			public static class TopTabItem
			{
				// Token: 0x17001712 RID: 5906
				// (get) Token: 0x06006AE7 RID: 27367 RVA: 0x0018BDE7 File Offset: 0x0018ADE7
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItem.normal == null)
						{
							VisualStyleElement.Tab.TopTabItem.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItem.part, 1);
						}
						return VisualStyleElement.Tab.TopTabItem.normal;
					}
				}

				// Token: 0x17001713 RID: 5907
				// (get) Token: 0x06006AE8 RID: 27368 RVA: 0x0018BE0A File Offset: 0x0018AE0A
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItem.hot == null)
						{
							VisualStyleElement.Tab.TopTabItem.hot = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItem.part, 2);
						}
						return VisualStyleElement.Tab.TopTabItem.hot;
					}
				}

				// Token: 0x17001714 RID: 5908
				// (get) Token: 0x06006AE9 RID: 27369 RVA: 0x0018BE2D File Offset: 0x0018AE2D
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItem.pressed == null)
						{
							VisualStyleElement.Tab.TopTabItem.pressed = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItem.part, 3);
						}
						return VisualStyleElement.Tab.TopTabItem.pressed;
					}
				}

				// Token: 0x17001715 RID: 5909
				// (get) Token: 0x06006AEA RID: 27370 RVA: 0x0018BE50 File Offset: 0x0018AE50
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItem.disabled == null)
						{
							VisualStyleElement.Tab.TopTabItem.disabled = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItem.part, 4);
						}
						return VisualStyleElement.Tab.TopTabItem.disabled;
					}
				}

				// Token: 0x04003EDB RID: 16091
				private static readonly int part = 5;

				// Token: 0x04003EDC RID: 16092
				private static VisualStyleElement normal;

				// Token: 0x04003EDD RID: 16093
				private static VisualStyleElement hot;

				// Token: 0x04003EDE RID: 16094
				private static VisualStyleElement pressed;

				// Token: 0x04003EDF RID: 16095
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007F8 RID: 2040
			public static class TopTabItemLeftEdge
			{
				// Token: 0x17001716 RID: 5910
				// (get) Token: 0x06006AEC RID: 27372 RVA: 0x0018BE7B File Offset: 0x0018AE7B
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemLeftEdge.normal == null)
						{
							VisualStyleElement.Tab.TopTabItemLeftEdge.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemLeftEdge.part, 1);
						}
						return VisualStyleElement.Tab.TopTabItemLeftEdge.normal;
					}
				}

				// Token: 0x17001717 RID: 5911
				// (get) Token: 0x06006AED RID: 27373 RVA: 0x0018BE9E File Offset: 0x0018AE9E
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemLeftEdge.hot == null)
						{
							VisualStyleElement.Tab.TopTabItemLeftEdge.hot = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemLeftEdge.part, 2);
						}
						return VisualStyleElement.Tab.TopTabItemLeftEdge.hot;
					}
				}

				// Token: 0x17001718 RID: 5912
				// (get) Token: 0x06006AEE RID: 27374 RVA: 0x0018BEC1 File Offset: 0x0018AEC1
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemLeftEdge.pressed == null)
						{
							VisualStyleElement.Tab.TopTabItemLeftEdge.pressed = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemLeftEdge.part, 3);
						}
						return VisualStyleElement.Tab.TopTabItemLeftEdge.pressed;
					}
				}

				// Token: 0x17001719 RID: 5913
				// (get) Token: 0x06006AEF RID: 27375 RVA: 0x0018BEE4 File Offset: 0x0018AEE4
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemLeftEdge.disabled == null)
						{
							VisualStyleElement.Tab.TopTabItemLeftEdge.disabled = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemLeftEdge.part, 4);
						}
						return VisualStyleElement.Tab.TopTabItemLeftEdge.disabled;
					}
				}

				// Token: 0x04003EE0 RID: 16096
				private static readonly int part = 6;

				// Token: 0x04003EE1 RID: 16097
				private static VisualStyleElement normal;

				// Token: 0x04003EE2 RID: 16098
				private static VisualStyleElement hot;

				// Token: 0x04003EE3 RID: 16099
				private static VisualStyleElement pressed;

				// Token: 0x04003EE4 RID: 16100
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007F9 RID: 2041
			public static class TopTabItemRightEdge
			{
				// Token: 0x1700171A RID: 5914
				// (get) Token: 0x06006AF1 RID: 27377 RVA: 0x0018BF0F File Offset: 0x0018AF0F
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemRightEdge.normal == null)
						{
							VisualStyleElement.Tab.TopTabItemRightEdge.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemRightEdge.part, 1);
						}
						return VisualStyleElement.Tab.TopTabItemRightEdge.normal;
					}
				}

				// Token: 0x1700171B RID: 5915
				// (get) Token: 0x06006AF2 RID: 27378 RVA: 0x0018BF32 File Offset: 0x0018AF32
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemRightEdge.hot == null)
						{
							VisualStyleElement.Tab.TopTabItemRightEdge.hot = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemRightEdge.part, 2);
						}
						return VisualStyleElement.Tab.TopTabItemRightEdge.hot;
					}
				}

				// Token: 0x1700171C RID: 5916
				// (get) Token: 0x06006AF3 RID: 27379 RVA: 0x0018BF55 File Offset: 0x0018AF55
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemRightEdge.pressed == null)
						{
							VisualStyleElement.Tab.TopTabItemRightEdge.pressed = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemRightEdge.part, 3);
						}
						return VisualStyleElement.Tab.TopTabItemRightEdge.pressed;
					}
				}

				// Token: 0x1700171D RID: 5917
				// (get) Token: 0x06006AF4 RID: 27380 RVA: 0x0018BF78 File Offset: 0x0018AF78
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemRightEdge.disabled == null)
						{
							VisualStyleElement.Tab.TopTabItemRightEdge.disabled = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemRightEdge.part, 4);
						}
						return VisualStyleElement.Tab.TopTabItemRightEdge.disabled;
					}
				}

				// Token: 0x04003EE5 RID: 16101
				private static readonly int part = 7;

				// Token: 0x04003EE6 RID: 16102
				private static VisualStyleElement normal;

				// Token: 0x04003EE7 RID: 16103
				private static VisualStyleElement hot;

				// Token: 0x04003EE8 RID: 16104
				private static VisualStyleElement pressed;

				// Token: 0x04003EE9 RID: 16105
				private static VisualStyleElement disabled;
			}

			// Token: 0x020007FA RID: 2042
			public static class TopTabItemBothEdges
			{
				// Token: 0x1700171E RID: 5918
				// (get) Token: 0x06006AF6 RID: 27382 RVA: 0x0018BFA3 File Offset: 0x0018AFA3
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.TopTabItemBothEdges.normal == null)
						{
							VisualStyleElement.Tab.TopTabItemBothEdges.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.TopTabItemBothEdges.part, 0);
						}
						return VisualStyleElement.Tab.TopTabItemBothEdges.normal;
					}
				}

				// Token: 0x04003EEA RID: 16106
				private static readonly int part = 8;

				// Token: 0x04003EEB RID: 16107
				private static VisualStyleElement normal;
			}

			// Token: 0x020007FB RID: 2043
			public static class Pane
			{
				// Token: 0x1700171F RID: 5919
				// (get) Token: 0x06006AF8 RID: 27384 RVA: 0x0018BFCE File Offset: 0x0018AFCE
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.Pane.normal == null)
						{
							VisualStyleElement.Tab.Pane.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.Pane.part, 0);
						}
						return VisualStyleElement.Tab.Pane.normal;
					}
				}

				// Token: 0x04003EEC RID: 16108
				private static readonly int part = 9;

				// Token: 0x04003EED RID: 16109
				private static VisualStyleElement normal;
			}

			// Token: 0x020007FC RID: 2044
			public static class Body
			{
				// Token: 0x17001720 RID: 5920
				// (get) Token: 0x06006AFA RID: 27386 RVA: 0x0018BFFA File Offset: 0x0018AFFA
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Tab.Body.normal == null)
						{
							VisualStyleElement.Tab.Body.normal = new VisualStyleElement(VisualStyleElement.Tab.className, VisualStyleElement.Tab.Body.part, 0);
						}
						return VisualStyleElement.Tab.Body.normal;
					}
				}

				// Token: 0x04003EEE RID: 16110
				private static readonly int part = 10;

				// Token: 0x04003EEF RID: 16111
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x020007FD RID: 2045
		public static class ExplorerBar
		{
			// Token: 0x04003EF0 RID: 16112
			private static readonly string className = "EXPLORERBAR";

			// Token: 0x020007FE RID: 2046
			public static class HeaderBackground
			{
				// Token: 0x17001721 RID: 5921
				// (get) Token: 0x06006AFD RID: 27389 RVA: 0x0018C032 File Offset: 0x0018B032
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderBackground.normal == null)
						{
							VisualStyleElement.ExplorerBar.HeaderBackground.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderBackground.part, 0);
						}
						return VisualStyleElement.ExplorerBar.HeaderBackground.normal;
					}
				}

				// Token: 0x04003EF1 RID: 16113
				private static readonly int part = 1;

				// Token: 0x04003EF2 RID: 16114
				private static VisualStyleElement normal;
			}

			// Token: 0x020007FF RID: 2047
			public static class HeaderClose
			{
				// Token: 0x17001722 RID: 5922
				// (get) Token: 0x06006AFF RID: 27391 RVA: 0x0018C05D File Offset: 0x0018B05D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderClose.normal == null)
						{
							VisualStyleElement.ExplorerBar.HeaderClose.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderClose.part, 1);
						}
						return VisualStyleElement.ExplorerBar.HeaderClose.normal;
					}
				}

				// Token: 0x17001723 RID: 5923
				// (get) Token: 0x06006B00 RID: 27392 RVA: 0x0018C080 File Offset: 0x0018B080
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderClose.hot == null)
						{
							VisualStyleElement.ExplorerBar.HeaderClose.hot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderClose.part, 2);
						}
						return VisualStyleElement.ExplorerBar.HeaderClose.hot;
					}
				}

				// Token: 0x17001724 RID: 5924
				// (get) Token: 0x06006B01 RID: 27393 RVA: 0x0018C0A3 File Offset: 0x0018B0A3
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderClose.pressed == null)
						{
							VisualStyleElement.ExplorerBar.HeaderClose.pressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderClose.part, 3);
						}
						return VisualStyleElement.ExplorerBar.HeaderClose.pressed;
					}
				}

				// Token: 0x04003EF3 RID: 16115
				private static readonly int part = 2;

				// Token: 0x04003EF4 RID: 16116
				private static VisualStyleElement normal;

				// Token: 0x04003EF5 RID: 16117
				private static VisualStyleElement hot;

				// Token: 0x04003EF6 RID: 16118
				private static VisualStyleElement pressed;
			}

			// Token: 0x02000800 RID: 2048
			public static class HeaderPin
			{
				// Token: 0x17001725 RID: 5925
				// (get) Token: 0x06006B03 RID: 27395 RVA: 0x0018C0CE File Offset: 0x0018B0CE
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderPin.normal == null)
						{
							VisualStyleElement.ExplorerBar.HeaderPin.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderPin.part, 1);
						}
						return VisualStyleElement.ExplorerBar.HeaderPin.normal;
					}
				}

				// Token: 0x17001726 RID: 5926
				// (get) Token: 0x06006B04 RID: 27396 RVA: 0x0018C0F1 File Offset: 0x0018B0F1
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderPin.hot == null)
						{
							VisualStyleElement.ExplorerBar.HeaderPin.hot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderPin.part, 2);
						}
						return VisualStyleElement.ExplorerBar.HeaderPin.hot;
					}
				}

				// Token: 0x17001727 RID: 5927
				// (get) Token: 0x06006B05 RID: 27397 RVA: 0x0018C114 File Offset: 0x0018B114
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderPin.pressed == null)
						{
							VisualStyleElement.ExplorerBar.HeaderPin.pressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderPin.part, 3);
						}
						return VisualStyleElement.ExplorerBar.HeaderPin.pressed;
					}
				}

				// Token: 0x17001728 RID: 5928
				// (get) Token: 0x06006B06 RID: 27398 RVA: 0x0018C137 File Offset: 0x0018B137
				public static VisualStyleElement SelectedNormal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderPin.selectednormal == null)
						{
							VisualStyleElement.ExplorerBar.HeaderPin.selectednormal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderPin.part, 4);
						}
						return VisualStyleElement.ExplorerBar.HeaderPin.selectednormal;
					}
				}

				// Token: 0x17001729 RID: 5929
				// (get) Token: 0x06006B07 RID: 27399 RVA: 0x0018C15A File Offset: 0x0018B15A
				public static VisualStyleElement SelectedHot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderPin.selectedhot == null)
						{
							VisualStyleElement.ExplorerBar.HeaderPin.selectedhot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderPin.part, 5);
						}
						return VisualStyleElement.ExplorerBar.HeaderPin.selectedhot;
					}
				}

				// Token: 0x1700172A RID: 5930
				// (get) Token: 0x06006B08 RID: 27400 RVA: 0x0018C17D File Offset: 0x0018B17D
				public static VisualStyleElement SelectedPressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.HeaderPin.selectedpressed == null)
						{
							VisualStyleElement.ExplorerBar.HeaderPin.selectedpressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.HeaderPin.part, 6);
						}
						return VisualStyleElement.ExplorerBar.HeaderPin.selectedpressed;
					}
				}

				// Token: 0x04003EF7 RID: 16119
				private static readonly int part = 3;

				// Token: 0x04003EF8 RID: 16120
				private static VisualStyleElement normal;

				// Token: 0x04003EF9 RID: 16121
				private static VisualStyleElement hot;

				// Token: 0x04003EFA RID: 16122
				private static VisualStyleElement pressed;

				// Token: 0x04003EFB RID: 16123
				private static VisualStyleElement selectednormal;

				// Token: 0x04003EFC RID: 16124
				private static VisualStyleElement selectedhot;

				// Token: 0x04003EFD RID: 16125
				private static VisualStyleElement selectedpressed;
			}

			// Token: 0x02000801 RID: 2049
			public static class IEBarMenu
			{
				// Token: 0x1700172B RID: 5931
				// (get) Token: 0x06006B0A RID: 27402 RVA: 0x0018C1A8 File Offset: 0x0018B1A8
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.IEBarMenu.normal == null)
						{
							VisualStyleElement.ExplorerBar.IEBarMenu.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.IEBarMenu.part, 1);
						}
						return VisualStyleElement.ExplorerBar.IEBarMenu.normal;
					}
				}

				// Token: 0x1700172C RID: 5932
				// (get) Token: 0x06006B0B RID: 27403 RVA: 0x0018C1CB File Offset: 0x0018B1CB
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.IEBarMenu.hot == null)
						{
							VisualStyleElement.ExplorerBar.IEBarMenu.hot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.IEBarMenu.part, 2);
						}
						return VisualStyleElement.ExplorerBar.IEBarMenu.hot;
					}
				}

				// Token: 0x1700172D RID: 5933
				// (get) Token: 0x06006B0C RID: 27404 RVA: 0x0018C1EE File Offset: 0x0018B1EE
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.IEBarMenu.pressed == null)
						{
							VisualStyleElement.ExplorerBar.IEBarMenu.pressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.IEBarMenu.part, 3);
						}
						return VisualStyleElement.ExplorerBar.IEBarMenu.pressed;
					}
				}

				// Token: 0x04003EFE RID: 16126
				private static readonly int part = 4;

				// Token: 0x04003EFF RID: 16127
				private static VisualStyleElement normal;

				// Token: 0x04003F00 RID: 16128
				private static VisualStyleElement hot;

				// Token: 0x04003F01 RID: 16129
				private static VisualStyleElement pressed;
			}

			// Token: 0x02000802 RID: 2050
			public static class NormalGroupBackground
			{
				// Token: 0x1700172E RID: 5934
				// (get) Token: 0x06006B0E RID: 27406 RVA: 0x0018C219 File Offset: 0x0018B219
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupBackground.normal == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupBackground.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupBackground.part, 0);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupBackground.normal;
					}
				}

				// Token: 0x04003F02 RID: 16130
				private static readonly int part = 5;

				// Token: 0x04003F03 RID: 16131
				private static VisualStyleElement normal;
			}

			// Token: 0x02000803 RID: 2051
			public static class NormalGroupCollapse
			{
				// Token: 0x1700172F RID: 5935
				// (get) Token: 0x06006B10 RID: 27408 RVA: 0x0018C244 File Offset: 0x0018B244
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupCollapse.normal == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupCollapse.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupCollapse.part, 1);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupCollapse.normal;
					}
				}

				// Token: 0x17001730 RID: 5936
				// (get) Token: 0x06006B11 RID: 27409 RVA: 0x0018C267 File Offset: 0x0018B267
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupCollapse.hot == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupCollapse.hot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupCollapse.part, 2);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupCollapse.hot;
					}
				}

				// Token: 0x17001731 RID: 5937
				// (get) Token: 0x06006B12 RID: 27410 RVA: 0x0018C28A File Offset: 0x0018B28A
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupCollapse.pressed == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupCollapse.pressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupCollapse.part, 3);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupCollapse.pressed;
					}
				}

				// Token: 0x04003F04 RID: 16132
				private static readonly int part = 6;

				// Token: 0x04003F05 RID: 16133
				private static VisualStyleElement normal;

				// Token: 0x04003F06 RID: 16134
				private static VisualStyleElement hot;

				// Token: 0x04003F07 RID: 16135
				private static VisualStyleElement pressed;
			}

			// Token: 0x02000804 RID: 2052
			public static class NormalGroupExpand
			{
				// Token: 0x17001732 RID: 5938
				// (get) Token: 0x06006B14 RID: 27412 RVA: 0x0018C2B5 File Offset: 0x0018B2B5
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupExpand.normal == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupExpand.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupExpand.part, 1);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupExpand.normal;
					}
				}

				// Token: 0x17001733 RID: 5939
				// (get) Token: 0x06006B15 RID: 27413 RVA: 0x0018C2D8 File Offset: 0x0018B2D8
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupExpand.hot == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupExpand.hot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupExpand.part, 2);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupExpand.hot;
					}
				}

				// Token: 0x17001734 RID: 5940
				// (get) Token: 0x06006B16 RID: 27414 RVA: 0x0018C2FB File Offset: 0x0018B2FB
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupExpand.pressed == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupExpand.pressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupExpand.part, 3);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupExpand.pressed;
					}
				}

				// Token: 0x04003F08 RID: 16136
				private static readonly int part = 7;

				// Token: 0x04003F09 RID: 16137
				private static VisualStyleElement normal;

				// Token: 0x04003F0A RID: 16138
				private static VisualStyleElement hot;

				// Token: 0x04003F0B RID: 16139
				private static VisualStyleElement pressed;
			}

			// Token: 0x02000805 RID: 2053
			public static class NormalGroupHead
			{
				// Token: 0x17001735 RID: 5941
				// (get) Token: 0x06006B18 RID: 27416 RVA: 0x0018C326 File Offset: 0x0018B326
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.NormalGroupHead.normal == null)
						{
							VisualStyleElement.ExplorerBar.NormalGroupHead.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.NormalGroupHead.part, 0);
						}
						return VisualStyleElement.ExplorerBar.NormalGroupHead.normal;
					}
				}

				// Token: 0x04003F0C RID: 16140
				private static readonly int part = 8;

				// Token: 0x04003F0D RID: 16141
				private static VisualStyleElement normal;
			}

			// Token: 0x02000806 RID: 2054
			public static class SpecialGroupBackground
			{
				// Token: 0x17001736 RID: 5942
				// (get) Token: 0x06006B1A RID: 27418 RVA: 0x0018C351 File Offset: 0x0018B351
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupBackground.normal == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupBackground.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupBackground.part, 0);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupBackground.normal;
					}
				}

				// Token: 0x04003F0E RID: 16142
				private static readonly int part = 9;

				// Token: 0x04003F0F RID: 16143
				private static VisualStyleElement normal;
			}

			// Token: 0x02000807 RID: 2055
			public static class SpecialGroupCollapse
			{
				// Token: 0x17001737 RID: 5943
				// (get) Token: 0x06006B1C RID: 27420 RVA: 0x0018C37D File Offset: 0x0018B37D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupCollapse.normal == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupCollapse.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupCollapse.part, 1);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupCollapse.normal;
					}
				}

				// Token: 0x17001738 RID: 5944
				// (get) Token: 0x06006B1D RID: 27421 RVA: 0x0018C3A0 File Offset: 0x0018B3A0
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupCollapse.hot == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupCollapse.hot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupCollapse.part, 2);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupCollapse.hot;
					}
				}

				// Token: 0x17001739 RID: 5945
				// (get) Token: 0x06006B1E RID: 27422 RVA: 0x0018C3C3 File Offset: 0x0018B3C3
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupCollapse.pressed == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupCollapse.pressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupCollapse.part, 3);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupCollapse.pressed;
					}
				}

				// Token: 0x04003F10 RID: 16144
				private static readonly int part = 10;

				// Token: 0x04003F11 RID: 16145
				private static VisualStyleElement normal;

				// Token: 0x04003F12 RID: 16146
				private static VisualStyleElement hot;

				// Token: 0x04003F13 RID: 16147
				private static VisualStyleElement pressed;
			}

			// Token: 0x02000808 RID: 2056
			public static class SpecialGroupExpand
			{
				// Token: 0x1700173A RID: 5946
				// (get) Token: 0x06006B20 RID: 27424 RVA: 0x0018C3EF File Offset: 0x0018B3EF
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupExpand.normal == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupExpand.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupExpand.part, 1);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupExpand.normal;
					}
				}

				// Token: 0x1700173B RID: 5947
				// (get) Token: 0x06006B21 RID: 27425 RVA: 0x0018C412 File Offset: 0x0018B412
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupExpand.hot == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupExpand.hot = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupExpand.part, 2);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupExpand.hot;
					}
				}

				// Token: 0x1700173C RID: 5948
				// (get) Token: 0x06006B22 RID: 27426 RVA: 0x0018C435 File Offset: 0x0018B435
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupExpand.pressed == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupExpand.pressed = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupExpand.part, 3);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupExpand.pressed;
					}
				}

				// Token: 0x04003F14 RID: 16148
				private static readonly int part = 11;

				// Token: 0x04003F15 RID: 16149
				private static VisualStyleElement normal;

				// Token: 0x04003F16 RID: 16150
				private static VisualStyleElement hot;

				// Token: 0x04003F17 RID: 16151
				private static VisualStyleElement pressed;
			}

			// Token: 0x02000809 RID: 2057
			public static class SpecialGroupHead
			{
				// Token: 0x1700173D RID: 5949
				// (get) Token: 0x06006B24 RID: 27428 RVA: 0x0018C461 File Offset: 0x0018B461
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ExplorerBar.SpecialGroupHead.normal == null)
						{
							VisualStyleElement.ExplorerBar.SpecialGroupHead.normal = new VisualStyleElement(VisualStyleElement.ExplorerBar.className, VisualStyleElement.ExplorerBar.SpecialGroupHead.part, 0);
						}
						return VisualStyleElement.ExplorerBar.SpecialGroupHead.normal;
					}
				}

				// Token: 0x04003F18 RID: 16152
				private static readonly int part = 12;

				// Token: 0x04003F19 RID: 16153
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x0200080A RID: 2058
		public static class Header
		{
			// Token: 0x04003F1A RID: 16154
			private static readonly string className = "HEADER";

			// Token: 0x0200080B RID: 2059
			public static class Item
			{
				// Token: 0x1700173E RID: 5950
				// (get) Token: 0x06006B27 RID: 27431 RVA: 0x0018C499 File Offset: 0x0018B499
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Header.Item.normal == null)
						{
							VisualStyleElement.Header.Item.normal = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.Item.part, 1);
						}
						return VisualStyleElement.Header.Item.normal;
					}
				}

				// Token: 0x1700173F RID: 5951
				// (get) Token: 0x06006B28 RID: 27432 RVA: 0x0018C4BC File Offset: 0x0018B4BC
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Header.Item.hot == null)
						{
							VisualStyleElement.Header.Item.hot = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.Item.part, 2);
						}
						return VisualStyleElement.Header.Item.hot;
					}
				}

				// Token: 0x17001740 RID: 5952
				// (get) Token: 0x06006B29 RID: 27433 RVA: 0x0018C4DF File Offset: 0x0018B4DF
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Header.Item.pressed == null)
						{
							VisualStyleElement.Header.Item.pressed = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.Item.part, 3);
						}
						return VisualStyleElement.Header.Item.pressed;
					}
				}

				// Token: 0x04003F1B RID: 16155
				private static readonly int part = 1;

				// Token: 0x04003F1C RID: 16156
				private static VisualStyleElement normal;

				// Token: 0x04003F1D RID: 16157
				private static VisualStyleElement hot;

				// Token: 0x04003F1E RID: 16158
				private static VisualStyleElement pressed;
			}

			// Token: 0x0200080C RID: 2060
			public static class ItemLeft
			{
				// Token: 0x17001741 RID: 5953
				// (get) Token: 0x06006B2B RID: 27435 RVA: 0x0018C50A File Offset: 0x0018B50A
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Header.ItemLeft.normal == null)
						{
							VisualStyleElement.Header.ItemLeft.normal = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.ItemLeft.part, 1);
						}
						return VisualStyleElement.Header.ItemLeft.normal;
					}
				}

				// Token: 0x17001742 RID: 5954
				// (get) Token: 0x06006B2C RID: 27436 RVA: 0x0018C52D File Offset: 0x0018B52D
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Header.ItemLeft.hot == null)
						{
							VisualStyleElement.Header.ItemLeft.hot = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.ItemLeft.part, 2);
						}
						return VisualStyleElement.Header.ItemLeft.hot;
					}
				}

				// Token: 0x17001743 RID: 5955
				// (get) Token: 0x06006B2D RID: 27437 RVA: 0x0018C550 File Offset: 0x0018B550
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Header.ItemLeft.pressed == null)
						{
							VisualStyleElement.Header.ItemLeft.pressed = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.ItemLeft.part, 3);
						}
						return VisualStyleElement.Header.ItemLeft.pressed;
					}
				}

				// Token: 0x04003F1F RID: 16159
				private static readonly int part = 2;

				// Token: 0x04003F20 RID: 16160
				private static VisualStyleElement normal;

				// Token: 0x04003F21 RID: 16161
				private static VisualStyleElement hot;

				// Token: 0x04003F22 RID: 16162
				private static VisualStyleElement pressed;
			}

			// Token: 0x0200080D RID: 2061
			public static class ItemRight
			{
				// Token: 0x17001744 RID: 5956
				// (get) Token: 0x06006B2F RID: 27439 RVA: 0x0018C57B File Offset: 0x0018B57B
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Header.ItemRight.normal == null)
						{
							VisualStyleElement.Header.ItemRight.normal = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.ItemRight.part, 1);
						}
						return VisualStyleElement.Header.ItemRight.normal;
					}
				}

				// Token: 0x17001745 RID: 5957
				// (get) Token: 0x06006B30 RID: 27440 RVA: 0x0018C59E File Offset: 0x0018B59E
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Header.ItemRight.hot == null)
						{
							VisualStyleElement.Header.ItemRight.hot = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.ItemRight.part, 2);
						}
						return VisualStyleElement.Header.ItemRight.hot;
					}
				}

				// Token: 0x17001746 RID: 5958
				// (get) Token: 0x06006B31 RID: 27441 RVA: 0x0018C5C1 File Offset: 0x0018B5C1
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Header.ItemRight.pressed == null)
						{
							VisualStyleElement.Header.ItemRight.pressed = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.ItemRight.part, 3);
						}
						return VisualStyleElement.Header.ItemRight.pressed;
					}
				}

				// Token: 0x04003F23 RID: 16163
				private static readonly int part = 3;

				// Token: 0x04003F24 RID: 16164
				private static VisualStyleElement normal;

				// Token: 0x04003F25 RID: 16165
				private static VisualStyleElement hot;

				// Token: 0x04003F26 RID: 16166
				private static VisualStyleElement pressed;
			}

			// Token: 0x0200080E RID: 2062
			public static class SortArrow
			{
				// Token: 0x17001747 RID: 5959
				// (get) Token: 0x06006B33 RID: 27443 RVA: 0x0018C5EC File Offset: 0x0018B5EC
				public static VisualStyleElement SortedUp
				{
					get
					{
						if (VisualStyleElement.Header.SortArrow.sortedup == null)
						{
							VisualStyleElement.Header.SortArrow.sortedup = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.SortArrow.part, 1);
						}
						return VisualStyleElement.Header.SortArrow.sortedup;
					}
				}

				// Token: 0x17001748 RID: 5960
				// (get) Token: 0x06006B34 RID: 27444 RVA: 0x0018C60F File Offset: 0x0018B60F
				public static VisualStyleElement SortedDown
				{
					get
					{
						if (VisualStyleElement.Header.SortArrow.sorteddown == null)
						{
							VisualStyleElement.Header.SortArrow.sorteddown = new VisualStyleElement(VisualStyleElement.Header.className, VisualStyleElement.Header.SortArrow.part, 2);
						}
						return VisualStyleElement.Header.SortArrow.sorteddown;
					}
				}

				// Token: 0x04003F27 RID: 16167
				private static readonly int part = 4;

				// Token: 0x04003F28 RID: 16168
				private static VisualStyleElement sortedup;

				// Token: 0x04003F29 RID: 16169
				private static VisualStyleElement sorteddown;
			}
		}

		// Token: 0x0200080F RID: 2063
		public static class ListView
		{
			// Token: 0x04003F2A RID: 16170
			private static readonly string className = "LISTVIEW";

			// Token: 0x02000810 RID: 2064
			public static class Item
			{
				// Token: 0x17001749 RID: 5961
				// (get) Token: 0x06006B37 RID: 27447 RVA: 0x0018C646 File Offset: 0x0018B646
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ListView.Item.normal == null)
						{
							VisualStyleElement.ListView.Item.normal = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.Item.part, 1);
						}
						return VisualStyleElement.ListView.Item.normal;
					}
				}

				// Token: 0x1700174A RID: 5962
				// (get) Token: 0x06006B38 RID: 27448 RVA: 0x0018C669 File Offset: 0x0018B669
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ListView.Item.hot == null)
						{
							VisualStyleElement.ListView.Item.hot = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.Item.part, 2);
						}
						return VisualStyleElement.ListView.Item.hot;
					}
				}

				// Token: 0x1700174B RID: 5963
				// (get) Token: 0x06006B39 RID: 27449 RVA: 0x0018C68C File Offset: 0x0018B68C
				public static VisualStyleElement Selected
				{
					get
					{
						if (VisualStyleElement.ListView.Item.selected == null)
						{
							VisualStyleElement.ListView.Item.selected = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.Item.part, 3);
						}
						return VisualStyleElement.ListView.Item.selected;
					}
				}

				// Token: 0x1700174C RID: 5964
				// (get) Token: 0x06006B3A RID: 27450 RVA: 0x0018C6AF File Offset: 0x0018B6AF
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ListView.Item.disabled == null)
						{
							VisualStyleElement.ListView.Item.disabled = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.Item.part, 4);
						}
						return VisualStyleElement.ListView.Item.disabled;
					}
				}

				// Token: 0x1700174D RID: 5965
				// (get) Token: 0x06006B3B RID: 27451 RVA: 0x0018C6D2 File Offset: 0x0018B6D2
				public static VisualStyleElement SelectedNotFocus
				{
					get
					{
						if (VisualStyleElement.ListView.Item.selectednotfocus == null)
						{
							VisualStyleElement.ListView.Item.selectednotfocus = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.Item.part, 5);
						}
						return VisualStyleElement.ListView.Item.selectednotfocus;
					}
				}

				// Token: 0x04003F2B RID: 16171
				private static readonly int part = 1;

				// Token: 0x04003F2C RID: 16172
				private static VisualStyleElement normal;

				// Token: 0x04003F2D RID: 16173
				private static VisualStyleElement hot;

				// Token: 0x04003F2E RID: 16174
				private static VisualStyleElement selected;

				// Token: 0x04003F2F RID: 16175
				private static VisualStyleElement disabled;

				// Token: 0x04003F30 RID: 16176
				private static VisualStyleElement selectednotfocus;
			}

			// Token: 0x02000811 RID: 2065
			public static class Group
			{
				// Token: 0x1700174E RID: 5966
				// (get) Token: 0x06006B3D RID: 27453 RVA: 0x0018C6FD File Offset: 0x0018B6FD
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ListView.Group.normal == null)
						{
							VisualStyleElement.ListView.Group.normal = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.Group.part, 0);
						}
						return VisualStyleElement.ListView.Group.normal;
					}
				}

				// Token: 0x04003F31 RID: 16177
				private static readonly int part = 2;

				// Token: 0x04003F32 RID: 16178
				private static VisualStyleElement normal;
			}

			// Token: 0x02000812 RID: 2066
			public static class Detail
			{
				// Token: 0x1700174F RID: 5967
				// (get) Token: 0x06006B3F RID: 27455 RVA: 0x0018C728 File Offset: 0x0018B728
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ListView.Detail.normal == null)
						{
							VisualStyleElement.ListView.Detail.normal = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.Detail.part, 0);
						}
						return VisualStyleElement.ListView.Detail.normal;
					}
				}

				// Token: 0x04003F33 RID: 16179
				private static readonly int part = 3;

				// Token: 0x04003F34 RID: 16180
				private static VisualStyleElement normal;
			}

			// Token: 0x02000813 RID: 2067
			public static class SortedDetail
			{
				// Token: 0x17001750 RID: 5968
				// (get) Token: 0x06006B41 RID: 27457 RVA: 0x0018C753 File Offset: 0x0018B753
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ListView.SortedDetail.normal == null)
						{
							VisualStyleElement.ListView.SortedDetail.normal = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.SortedDetail.part, 0);
						}
						return VisualStyleElement.ListView.SortedDetail.normal;
					}
				}

				// Token: 0x04003F35 RID: 16181
				private static readonly int part = 4;

				// Token: 0x04003F36 RID: 16182
				private static VisualStyleElement normal;
			}

			// Token: 0x02000814 RID: 2068
			public static class EmptyText
			{
				// Token: 0x17001751 RID: 5969
				// (get) Token: 0x06006B43 RID: 27459 RVA: 0x0018C77E File Offset: 0x0018B77E
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ListView.EmptyText.normal == null)
						{
							VisualStyleElement.ListView.EmptyText.normal = new VisualStyleElement(VisualStyleElement.ListView.className, VisualStyleElement.ListView.EmptyText.part, 0);
						}
						return VisualStyleElement.ListView.EmptyText.normal;
					}
				}

				// Token: 0x04003F37 RID: 16183
				private static readonly int part = 5;

				// Token: 0x04003F38 RID: 16184
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x02000815 RID: 2069
		public static class MenuBand
		{
			// Token: 0x04003F39 RID: 16185
			private static readonly string className = "MENUBAND";

			// Token: 0x02000816 RID: 2070
			public static class NewApplicationButton
			{
				// Token: 0x17001752 RID: 5970
				// (get) Token: 0x06006B46 RID: 27462 RVA: 0x0018C7B5 File Offset: 0x0018B7B5
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.MenuBand.NewApplicationButton.normal == null)
						{
							VisualStyleElement.MenuBand.NewApplicationButton.normal = new VisualStyleElement(VisualStyleElement.MenuBand.className, VisualStyleElement.MenuBand.NewApplicationButton.part, 1);
						}
						return VisualStyleElement.MenuBand.NewApplicationButton.normal;
					}
				}

				// Token: 0x17001753 RID: 5971
				// (get) Token: 0x06006B47 RID: 27463 RVA: 0x0018C7D8 File Offset: 0x0018B7D8
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.MenuBand.NewApplicationButton.hot == null)
						{
							VisualStyleElement.MenuBand.NewApplicationButton.hot = new VisualStyleElement(VisualStyleElement.MenuBand.className, VisualStyleElement.MenuBand.NewApplicationButton.part, 2);
						}
						return VisualStyleElement.MenuBand.NewApplicationButton.hot;
					}
				}

				// Token: 0x17001754 RID: 5972
				// (get) Token: 0x06006B48 RID: 27464 RVA: 0x0018C7FB File Offset: 0x0018B7FB
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.MenuBand.NewApplicationButton.pressed == null)
						{
							VisualStyleElement.MenuBand.NewApplicationButton.pressed = new VisualStyleElement(VisualStyleElement.MenuBand.className, VisualStyleElement.MenuBand.NewApplicationButton.part, 3);
						}
						return VisualStyleElement.MenuBand.NewApplicationButton.pressed;
					}
				}

				// Token: 0x17001755 RID: 5973
				// (get) Token: 0x06006B49 RID: 27465 RVA: 0x0018C81E File Offset: 0x0018B81E
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.MenuBand.NewApplicationButton.disabled == null)
						{
							VisualStyleElement.MenuBand.NewApplicationButton.disabled = new VisualStyleElement(VisualStyleElement.MenuBand.className, VisualStyleElement.MenuBand.NewApplicationButton.part, 4);
						}
						return VisualStyleElement.MenuBand.NewApplicationButton.disabled;
					}
				}

				// Token: 0x17001756 RID: 5974
				// (get) Token: 0x06006B4A RID: 27466 RVA: 0x0018C841 File Offset: 0x0018B841
				public static VisualStyleElement Checked
				{
					get
					{
						if (VisualStyleElement.MenuBand.NewApplicationButton._checked == null)
						{
							VisualStyleElement.MenuBand.NewApplicationButton._checked = new VisualStyleElement(VisualStyleElement.MenuBand.className, VisualStyleElement.MenuBand.NewApplicationButton.part, 5);
						}
						return VisualStyleElement.MenuBand.NewApplicationButton._checked;
					}
				}

				// Token: 0x17001757 RID: 5975
				// (get) Token: 0x06006B4B RID: 27467 RVA: 0x0018C864 File Offset: 0x0018B864
				public static VisualStyleElement HotChecked
				{
					get
					{
						if (VisualStyleElement.MenuBand.NewApplicationButton.hotchecked == null)
						{
							VisualStyleElement.MenuBand.NewApplicationButton.hotchecked = new VisualStyleElement(VisualStyleElement.MenuBand.className, VisualStyleElement.MenuBand.NewApplicationButton.part, 6);
						}
						return VisualStyleElement.MenuBand.NewApplicationButton.hotchecked;
					}
				}

				// Token: 0x04003F3A RID: 16186
				private static readonly int part = 1;

				// Token: 0x04003F3B RID: 16187
				private static VisualStyleElement normal;

				// Token: 0x04003F3C RID: 16188
				private static VisualStyleElement hot;

				// Token: 0x04003F3D RID: 16189
				private static VisualStyleElement pressed;

				// Token: 0x04003F3E RID: 16190
				private static VisualStyleElement disabled;

				// Token: 0x04003F3F RID: 16191
				private static VisualStyleElement _checked;

				// Token: 0x04003F40 RID: 16192
				private static VisualStyleElement hotchecked;
			}

			// Token: 0x02000817 RID: 2071
			public static class Separator
			{
				// Token: 0x17001758 RID: 5976
				// (get) Token: 0x06006B4D RID: 27469 RVA: 0x0018C88F File Offset: 0x0018B88F
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.MenuBand.Separator.normal == null)
						{
							VisualStyleElement.MenuBand.Separator.normal = new VisualStyleElement(VisualStyleElement.MenuBand.className, VisualStyleElement.MenuBand.Separator.part, 0);
						}
						return VisualStyleElement.MenuBand.Separator.normal;
					}
				}

				// Token: 0x04003F41 RID: 16193
				private static readonly int part = 2;

				// Token: 0x04003F42 RID: 16194
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x02000818 RID: 2072
		public static class Menu
		{
			// Token: 0x04003F43 RID: 16195
			private static readonly string className = "MENU";

			// Token: 0x02000819 RID: 2073
			public static class Item
			{
				// Token: 0x17001759 RID: 5977
				// (get) Token: 0x06006B50 RID: 27472 RVA: 0x0018C8C6 File Offset: 0x0018B8C6
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Menu.Item.normal == null)
						{
							VisualStyleElement.Menu.Item.normal = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.Item.part, 1);
						}
						return VisualStyleElement.Menu.Item.normal;
					}
				}

				// Token: 0x1700175A RID: 5978
				// (get) Token: 0x06006B51 RID: 27473 RVA: 0x0018C8E9 File Offset: 0x0018B8E9
				public static VisualStyleElement Selected
				{
					get
					{
						if (VisualStyleElement.Menu.Item.selected == null)
						{
							VisualStyleElement.Menu.Item.selected = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.Item.part, 2);
						}
						return VisualStyleElement.Menu.Item.selected;
					}
				}

				// Token: 0x1700175B RID: 5979
				// (get) Token: 0x06006B52 RID: 27474 RVA: 0x0018C90C File Offset: 0x0018B90C
				public static VisualStyleElement Demoted
				{
					get
					{
						if (VisualStyleElement.Menu.Item.demoted == null)
						{
							VisualStyleElement.Menu.Item.demoted = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.Item.part, 3);
						}
						return VisualStyleElement.Menu.Item.demoted;
					}
				}

				// Token: 0x04003F44 RID: 16196
				private static readonly int part = 1;

				// Token: 0x04003F45 RID: 16197
				private static VisualStyleElement normal;

				// Token: 0x04003F46 RID: 16198
				private static VisualStyleElement selected;

				// Token: 0x04003F47 RID: 16199
				private static VisualStyleElement demoted;
			}

			// Token: 0x0200081A RID: 2074
			public static class DropDown
			{
				// Token: 0x1700175C RID: 5980
				// (get) Token: 0x06006B54 RID: 27476 RVA: 0x0018C937 File Offset: 0x0018B937
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Menu.DropDown.normal == null)
						{
							VisualStyleElement.Menu.DropDown.normal = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.DropDown.part, 0);
						}
						return VisualStyleElement.Menu.DropDown.normal;
					}
				}

				// Token: 0x04003F48 RID: 16200
				private static readonly int part = 2;

				// Token: 0x04003F49 RID: 16201
				private static VisualStyleElement normal;
			}

			// Token: 0x0200081B RID: 2075
			public static class BarItem
			{
				// Token: 0x1700175D RID: 5981
				// (get) Token: 0x06006B56 RID: 27478 RVA: 0x0018C962 File Offset: 0x0018B962
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Menu.BarItem.normal == null)
						{
							VisualStyleElement.Menu.BarItem.normal = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.BarItem.part, 0);
						}
						return VisualStyleElement.Menu.BarItem.normal;
					}
				}

				// Token: 0x04003F4A RID: 16202
				private static readonly int part = 3;

				// Token: 0x04003F4B RID: 16203
				private static VisualStyleElement normal;
			}

			// Token: 0x0200081C RID: 2076
			public static class BarDropDown
			{
				// Token: 0x1700175E RID: 5982
				// (get) Token: 0x06006B58 RID: 27480 RVA: 0x0018C98D File Offset: 0x0018B98D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Menu.BarDropDown.normal == null)
						{
							VisualStyleElement.Menu.BarDropDown.normal = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.BarDropDown.part, 0);
						}
						return VisualStyleElement.Menu.BarDropDown.normal;
					}
				}

				// Token: 0x04003F4C RID: 16204
				private static readonly int part = 4;

				// Token: 0x04003F4D RID: 16205
				private static VisualStyleElement normal;
			}

			// Token: 0x0200081D RID: 2077
			public static class Chevron
			{
				// Token: 0x1700175F RID: 5983
				// (get) Token: 0x06006B5A RID: 27482 RVA: 0x0018C9B8 File Offset: 0x0018B9B8
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Menu.Chevron.normal == null)
						{
							VisualStyleElement.Menu.Chevron.normal = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.Chevron.part, 0);
						}
						return VisualStyleElement.Menu.Chevron.normal;
					}
				}

				// Token: 0x04003F4E RID: 16206
				private static readonly int part = 5;

				// Token: 0x04003F4F RID: 16207
				private static VisualStyleElement normal;
			}

			// Token: 0x0200081E RID: 2078
			public static class Separator
			{
				// Token: 0x17001760 RID: 5984
				// (get) Token: 0x06006B5C RID: 27484 RVA: 0x0018C9E3 File Offset: 0x0018B9E3
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Menu.Separator.normal == null)
						{
							VisualStyleElement.Menu.Separator.normal = new VisualStyleElement(VisualStyleElement.Menu.className, VisualStyleElement.Menu.Separator.part, 0);
						}
						return VisualStyleElement.Menu.Separator.normal;
					}
				}

				// Token: 0x04003F50 RID: 16208
				private static readonly int part = 6;

				// Token: 0x04003F51 RID: 16209
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x0200081F RID: 2079
		public static class ProgressBar
		{
			// Token: 0x04003F52 RID: 16210
			private static readonly string className = "PROGRESS";

			// Token: 0x02000820 RID: 2080
			public static class Bar
			{
				// Token: 0x17001761 RID: 5985
				// (get) Token: 0x06006B5F RID: 27487 RVA: 0x0018CA1A File Offset: 0x0018BA1A
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ProgressBar.Bar.normal == null)
						{
							VisualStyleElement.ProgressBar.Bar.normal = new VisualStyleElement(VisualStyleElement.ProgressBar.className, VisualStyleElement.ProgressBar.Bar.part, 0);
						}
						return VisualStyleElement.ProgressBar.Bar.normal;
					}
				}

				// Token: 0x04003F53 RID: 16211
				private static readonly int part = 1;

				// Token: 0x04003F54 RID: 16212
				private static VisualStyleElement normal;
			}

			// Token: 0x02000821 RID: 2081
			public static class BarVertical
			{
				// Token: 0x17001762 RID: 5986
				// (get) Token: 0x06006B61 RID: 27489 RVA: 0x0018CA45 File Offset: 0x0018BA45
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ProgressBar.BarVertical.normal == null)
						{
							VisualStyleElement.ProgressBar.BarVertical.normal = new VisualStyleElement(VisualStyleElement.ProgressBar.className, VisualStyleElement.ProgressBar.BarVertical.part, 0);
						}
						return VisualStyleElement.ProgressBar.BarVertical.normal;
					}
				}

				// Token: 0x04003F55 RID: 16213
				private static readonly int part = 2;

				// Token: 0x04003F56 RID: 16214
				private static VisualStyleElement normal;
			}

			// Token: 0x02000822 RID: 2082
			public static class Chunk
			{
				// Token: 0x17001763 RID: 5987
				// (get) Token: 0x06006B63 RID: 27491 RVA: 0x0018CA70 File Offset: 0x0018BA70
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ProgressBar.Chunk.normal == null)
						{
							VisualStyleElement.ProgressBar.Chunk.normal = new VisualStyleElement(VisualStyleElement.ProgressBar.className, VisualStyleElement.ProgressBar.Chunk.part, 0);
						}
						return VisualStyleElement.ProgressBar.Chunk.normal;
					}
				}

				// Token: 0x04003F57 RID: 16215
				private static readonly int part = 3;

				// Token: 0x04003F58 RID: 16216
				private static VisualStyleElement normal;
			}

			// Token: 0x02000823 RID: 2083
			public static class ChunkVertical
			{
				// Token: 0x17001764 RID: 5988
				// (get) Token: 0x06006B65 RID: 27493 RVA: 0x0018CA9B File Offset: 0x0018BA9B
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ProgressBar.ChunkVertical.normal == null)
						{
							VisualStyleElement.ProgressBar.ChunkVertical.normal = new VisualStyleElement(VisualStyleElement.ProgressBar.className, VisualStyleElement.ProgressBar.ChunkVertical.part, 0);
						}
						return VisualStyleElement.ProgressBar.ChunkVertical.normal;
					}
				}

				// Token: 0x04003F59 RID: 16217
				private static readonly int part = 4;

				// Token: 0x04003F5A RID: 16218
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x02000824 RID: 2084
		public static class Rebar
		{
			// Token: 0x04003F5B RID: 16219
			private static readonly string className = "REBAR";

			// Token: 0x02000825 RID: 2085
			public static class Gripper
			{
				// Token: 0x17001765 RID: 5989
				// (get) Token: 0x06006B68 RID: 27496 RVA: 0x0018CAD2 File Offset: 0x0018BAD2
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Rebar.Gripper.normal == null)
						{
							VisualStyleElement.Rebar.Gripper.normal = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.Gripper.part, 0);
						}
						return VisualStyleElement.Rebar.Gripper.normal;
					}
				}

				// Token: 0x04003F5C RID: 16220
				private static readonly int part = 1;

				// Token: 0x04003F5D RID: 16221
				private static VisualStyleElement normal;
			}

			// Token: 0x02000826 RID: 2086
			public static class GripperVertical
			{
				// Token: 0x17001766 RID: 5990
				// (get) Token: 0x06006B6A RID: 27498 RVA: 0x0018CAFD File Offset: 0x0018BAFD
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Rebar.GripperVertical.normal == null)
						{
							VisualStyleElement.Rebar.GripperVertical.normal = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.GripperVertical.part, 0);
						}
						return VisualStyleElement.Rebar.GripperVertical.normal;
					}
				}

				// Token: 0x04003F5E RID: 16222
				private static readonly int part = 2;

				// Token: 0x04003F5F RID: 16223
				private static VisualStyleElement normal;
			}

			// Token: 0x02000827 RID: 2087
			public static class Band
			{
				// Token: 0x17001767 RID: 5991
				// (get) Token: 0x06006B6C RID: 27500 RVA: 0x0018CB28 File Offset: 0x0018BB28
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Rebar.Band.normal == null)
						{
							VisualStyleElement.Rebar.Band.normal = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.Band.part, 0);
						}
						return VisualStyleElement.Rebar.Band.normal;
					}
				}

				// Token: 0x04003F60 RID: 16224
				private static readonly int part = 3;

				// Token: 0x04003F61 RID: 16225
				private static VisualStyleElement normal;
			}

			// Token: 0x02000828 RID: 2088
			public static class Chevron
			{
				// Token: 0x17001768 RID: 5992
				// (get) Token: 0x06006B6E RID: 27502 RVA: 0x0018CB53 File Offset: 0x0018BB53
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Rebar.Chevron.normal == null)
						{
							VisualStyleElement.Rebar.Chevron.normal = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.Chevron.part, 1);
						}
						return VisualStyleElement.Rebar.Chevron.normal;
					}
				}

				// Token: 0x17001769 RID: 5993
				// (get) Token: 0x06006B6F RID: 27503 RVA: 0x0018CB76 File Offset: 0x0018BB76
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Rebar.Chevron.hot == null)
						{
							VisualStyleElement.Rebar.Chevron.hot = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.Chevron.part, 2);
						}
						return VisualStyleElement.Rebar.Chevron.hot;
					}
				}

				// Token: 0x1700176A RID: 5994
				// (get) Token: 0x06006B70 RID: 27504 RVA: 0x0018CB99 File Offset: 0x0018BB99
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Rebar.Chevron.pressed == null)
						{
							VisualStyleElement.Rebar.Chevron.pressed = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.Chevron.part, 3);
						}
						return VisualStyleElement.Rebar.Chevron.pressed;
					}
				}

				// Token: 0x04003F62 RID: 16226
				private static readonly int part = 4;

				// Token: 0x04003F63 RID: 16227
				private static VisualStyleElement normal;

				// Token: 0x04003F64 RID: 16228
				private static VisualStyleElement hot;

				// Token: 0x04003F65 RID: 16229
				private static VisualStyleElement pressed;
			}

			// Token: 0x02000829 RID: 2089
			public static class ChevronVertical
			{
				// Token: 0x1700176B RID: 5995
				// (get) Token: 0x06006B72 RID: 27506 RVA: 0x0018CBC4 File Offset: 0x0018BBC4
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Rebar.ChevronVertical.normal == null)
						{
							VisualStyleElement.Rebar.ChevronVertical.normal = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.ChevronVertical.part, 1);
						}
						return VisualStyleElement.Rebar.ChevronVertical.normal;
					}
				}

				// Token: 0x1700176C RID: 5996
				// (get) Token: 0x06006B73 RID: 27507 RVA: 0x0018CBE7 File Offset: 0x0018BBE7
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Rebar.ChevronVertical.hot == null)
						{
							VisualStyleElement.Rebar.ChevronVertical.hot = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.ChevronVertical.part, 2);
						}
						return VisualStyleElement.Rebar.ChevronVertical.hot;
					}
				}

				// Token: 0x1700176D RID: 5997
				// (get) Token: 0x06006B74 RID: 27508 RVA: 0x0018CC0A File Offset: 0x0018BC0A
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Rebar.ChevronVertical.pressed == null)
						{
							VisualStyleElement.Rebar.ChevronVertical.pressed = new VisualStyleElement(VisualStyleElement.Rebar.className, VisualStyleElement.Rebar.ChevronVertical.part, 3);
						}
						return VisualStyleElement.Rebar.ChevronVertical.pressed;
					}
				}

				// Token: 0x04003F66 RID: 16230
				private static readonly int part = 5;

				// Token: 0x04003F67 RID: 16231
				private static VisualStyleElement normal;

				// Token: 0x04003F68 RID: 16232
				private static VisualStyleElement hot;

				// Token: 0x04003F69 RID: 16233
				private static VisualStyleElement pressed;
			}
		}

		// Token: 0x0200082A RID: 2090
		public static class StartPanel
		{
			// Token: 0x04003F6A RID: 16234
			private static readonly string className = "STARTPANEL";

			// Token: 0x0200082B RID: 2091
			public static class UserPane
			{
				// Token: 0x1700176E RID: 5998
				// (get) Token: 0x06006B77 RID: 27511 RVA: 0x0018CC41 File Offset: 0x0018BC41
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.UserPane.normal == null)
						{
							VisualStyleElement.StartPanel.UserPane.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.UserPane.part, 0);
						}
						return VisualStyleElement.StartPanel.UserPane.normal;
					}
				}

				// Token: 0x04003F6B RID: 16235
				private static readonly int part = 1;

				// Token: 0x04003F6C RID: 16236
				private static VisualStyleElement normal;
			}

			// Token: 0x0200082C RID: 2092
			public static class MorePrograms
			{
				// Token: 0x1700176F RID: 5999
				// (get) Token: 0x06006B79 RID: 27513 RVA: 0x0018CC6C File Offset: 0x0018BC6C
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.MorePrograms.normal == null)
						{
							VisualStyleElement.StartPanel.MorePrograms.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.MorePrograms.part, 0);
						}
						return VisualStyleElement.StartPanel.MorePrograms.normal;
					}
				}

				// Token: 0x04003F6D RID: 16237
				private static readonly int part = 2;

				// Token: 0x04003F6E RID: 16238
				private static VisualStyleElement normal;
			}

			// Token: 0x0200082D RID: 2093
			public static class MoreProgramsArrow
			{
				// Token: 0x17001770 RID: 6000
				// (get) Token: 0x06006B7B RID: 27515 RVA: 0x0018CC97 File Offset: 0x0018BC97
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.MoreProgramsArrow.normal == null)
						{
							VisualStyleElement.StartPanel.MoreProgramsArrow.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.MoreProgramsArrow.part, 1);
						}
						return VisualStyleElement.StartPanel.MoreProgramsArrow.normal;
					}
				}

				// Token: 0x17001771 RID: 6001
				// (get) Token: 0x06006B7C RID: 27516 RVA: 0x0018CCBA File Offset: 0x0018BCBA
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.StartPanel.MoreProgramsArrow.hot == null)
						{
							VisualStyleElement.StartPanel.MoreProgramsArrow.hot = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.MoreProgramsArrow.part, 2);
						}
						return VisualStyleElement.StartPanel.MoreProgramsArrow.hot;
					}
				}

				// Token: 0x17001772 RID: 6002
				// (get) Token: 0x06006B7D RID: 27517 RVA: 0x0018CCDD File Offset: 0x0018BCDD
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.StartPanel.MoreProgramsArrow.pressed == null)
						{
							VisualStyleElement.StartPanel.MoreProgramsArrow.pressed = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.MoreProgramsArrow.part, 3);
						}
						return VisualStyleElement.StartPanel.MoreProgramsArrow.pressed;
					}
				}

				// Token: 0x04003F6F RID: 16239
				private static readonly int part = 3;

				// Token: 0x04003F70 RID: 16240
				private static VisualStyleElement normal;

				// Token: 0x04003F71 RID: 16241
				private static VisualStyleElement hot;

				// Token: 0x04003F72 RID: 16242
				private static VisualStyleElement pressed;
			}

			// Token: 0x0200082E RID: 2094
			public static class ProgList
			{
				// Token: 0x17001773 RID: 6003
				// (get) Token: 0x06006B7F RID: 27519 RVA: 0x0018CD08 File Offset: 0x0018BD08
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.ProgList.normal == null)
						{
							VisualStyleElement.StartPanel.ProgList.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.ProgList.part, 0);
						}
						return VisualStyleElement.StartPanel.ProgList.normal;
					}
				}

				// Token: 0x04003F73 RID: 16243
				private static readonly int part = 4;

				// Token: 0x04003F74 RID: 16244
				private static VisualStyleElement normal;
			}

			// Token: 0x0200082F RID: 2095
			public static class ProgListSeparator
			{
				// Token: 0x17001774 RID: 6004
				// (get) Token: 0x06006B81 RID: 27521 RVA: 0x0018CD33 File Offset: 0x0018BD33
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.ProgListSeparator.normal == null)
						{
							VisualStyleElement.StartPanel.ProgListSeparator.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.ProgListSeparator.part, 0);
						}
						return VisualStyleElement.StartPanel.ProgListSeparator.normal;
					}
				}

				// Token: 0x04003F75 RID: 16245
				private static readonly int part = 5;

				// Token: 0x04003F76 RID: 16246
				private static VisualStyleElement normal;
			}

			// Token: 0x02000830 RID: 2096
			public static class PlaceList
			{
				// Token: 0x17001775 RID: 6005
				// (get) Token: 0x06006B83 RID: 27523 RVA: 0x0018CD5E File Offset: 0x0018BD5E
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.PlaceList.normal == null)
						{
							VisualStyleElement.StartPanel.PlaceList.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.PlaceList.part, 0);
						}
						return VisualStyleElement.StartPanel.PlaceList.normal;
					}
				}

				// Token: 0x04003F77 RID: 16247
				private static readonly int part = 6;

				// Token: 0x04003F78 RID: 16248
				private static VisualStyleElement normal;
			}

			// Token: 0x02000831 RID: 2097
			public static class PlaceListSeparator
			{
				// Token: 0x17001776 RID: 6006
				// (get) Token: 0x06006B85 RID: 27525 RVA: 0x0018CD89 File Offset: 0x0018BD89
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.PlaceListSeparator.normal == null)
						{
							VisualStyleElement.StartPanel.PlaceListSeparator.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.PlaceListSeparator.part, 0);
						}
						return VisualStyleElement.StartPanel.PlaceListSeparator.normal;
					}
				}

				// Token: 0x04003F79 RID: 16249
				private static readonly int part = 7;

				// Token: 0x04003F7A RID: 16250
				private static VisualStyleElement normal;
			}

			// Token: 0x02000832 RID: 2098
			public static class LogOff
			{
				// Token: 0x17001777 RID: 6007
				// (get) Token: 0x06006B87 RID: 27527 RVA: 0x0018CDB4 File Offset: 0x0018BDB4
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.LogOff.normal == null)
						{
							VisualStyleElement.StartPanel.LogOff.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.LogOff.part, 0);
						}
						return VisualStyleElement.StartPanel.LogOff.normal;
					}
				}

				// Token: 0x04003F7B RID: 16251
				private static readonly int part = 8;

				// Token: 0x04003F7C RID: 16252
				private static VisualStyleElement normal;
			}

			// Token: 0x02000833 RID: 2099
			public static class LogOffButtons
			{
				// Token: 0x17001778 RID: 6008
				// (get) Token: 0x06006B89 RID: 27529 RVA: 0x0018CDDF File Offset: 0x0018BDDF
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.LogOffButtons.normal == null)
						{
							VisualStyleElement.StartPanel.LogOffButtons.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.LogOffButtons.part, 1);
						}
						return VisualStyleElement.StartPanel.LogOffButtons.normal;
					}
				}

				// Token: 0x17001779 RID: 6009
				// (get) Token: 0x06006B8A RID: 27530 RVA: 0x0018CE02 File Offset: 0x0018BE02
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.StartPanel.LogOffButtons.hot == null)
						{
							VisualStyleElement.StartPanel.LogOffButtons.hot = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.LogOffButtons.part, 2);
						}
						return VisualStyleElement.StartPanel.LogOffButtons.hot;
					}
				}

				// Token: 0x1700177A RID: 6010
				// (get) Token: 0x06006B8B RID: 27531 RVA: 0x0018CE25 File Offset: 0x0018BE25
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.StartPanel.LogOffButtons.pressed == null)
						{
							VisualStyleElement.StartPanel.LogOffButtons.pressed = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.LogOffButtons.part, 3);
						}
						return VisualStyleElement.StartPanel.LogOffButtons.pressed;
					}
				}

				// Token: 0x04003F7D RID: 16253
				private static readonly int part = 9;

				// Token: 0x04003F7E RID: 16254
				private static VisualStyleElement normal;

				// Token: 0x04003F7F RID: 16255
				private static VisualStyleElement hot;

				// Token: 0x04003F80 RID: 16256
				private static VisualStyleElement pressed;
			}

			// Token: 0x02000834 RID: 2100
			public static class UserPicture
			{
				// Token: 0x1700177B RID: 6011
				// (get) Token: 0x06006B8D RID: 27533 RVA: 0x0018CE51 File Offset: 0x0018BE51
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.UserPicture.normal == null)
						{
							VisualStyleElement.StartPanel.UserPicture.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.UserPicture.part, 0);
						}
						return VisualStyleElement.StartPanel.UserPicture.normal;
					}
				}

				// Token: 0x04003F81 RID: 16257
				private static readonly int part = 10;

				// Token: 0x04003F82 RID: 16258
				private static VisualStyleElement normal;
			}

			// Token: 0x02000835 RID: 2101
			public static class Preview
			{
				// Token: 0x1700177C RID: 6012
				// (get) Token: 0x06006B8F RID: 27535 RVA: 0x0018CE7D File Offset: 0x0018BE7D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.StartPanel.Preview.normal == null)
						{
							VisualStyleElement.StartPanel.Preview.normal = new VisualStyleElement(VisualStyleElement.StartPanel.className, VisualStyleElement.StartPanel.Preview.part, 0);
						}
						return VisualStyleElement.StartPanel.Preview.normal;
					}
				}

				// Token: 0x04003F83 RID: 16259
				private static readonly int part = 11;

				// Token: 0x04003F84 RID: 16260
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x02000836 RID: 2102
		public static class Status
		{
			// Token: 0x04003F85 RID: 16261
			private static readonly string className = "STATUS";

			// Token: 0x02000837 RID: 2103
			public static class Bar
			{
				// Token: 0x1700177D RID: 6013
				// (get) Token: 0x06006B92 RID: 27538 RVA: 0x0018CEB5 File Offset: 0x0018BEB5
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Status.Bar.normal == null)
						{
							VisualStyleElement.Status.Bar.normal = new VisualStyleElement(VisualStyleElement.Status.className, VisualStyleElement.Status.Bar.part, 0);
						}
						return VisualStyleElement.Status.Bar.normal;
					}
				}

				// Token: 0x04003F86 RID: 16262
				private static readonly int part;

				// Token: 0x04003F87 RID: 16263
				private static VisualStyleElement normal;
			}

			// Token: 0x02000838 RID: 2104
			public static class Pane
			{
				// Token: 0x1700177E RID: 6014
				// (get) Token: 0x06006B93 RID: 27539 RVA: 0x0018CED8 File Offset: 0x0018BED8
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Status.Pane.normal == null)
						{
							VisualStyleElement.Status.Pane.normal = new VisualStyleElement(VisualStyleElement.Status.className, VisualStyleElement.Status.Pane.part, 0);
						}
						return VisualStyleElement.Status.Pane.normal;
					}
				}

				// Token: 0x04003F88 RID: 16264
				private static readonly int part = 1;

				// Token: 0x04003F89 RID: 16265
				private static VisualStyleElement normal;
			}

			// Token: 0x02000839 RID: 2105
			public static class GripperPane
			{
				// Token: 0x1700177F RID: 6015
				// (get) Token: 0x06006B95 RID: 27541 RVA: 0x0018CF03 File Offset: 0x0018BF03
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Status.GripperPane.normal == null)
						{
							VisualStyleElement.Status.GripperPane.normal = new VisualStyleElement(VisualStyleElement.Status.className, VisualStyleElement.Status.GripperPane.part, 0);
						}
						return VisualStyleElement.Status.GripperPane.normal;
					}
				}

				// Token: 0x04003F8A RID: 16266
				private static readonly int part = 2;

				// Token: 0x04003F8B RID: 16267
				private static VisualStyleElement normal;
			}

			// Token: 0x0200083A RID: 2106
			public static class Gripper
			{
				// Token: 0x17001780 RID: 6016
				// (get) Token: 0x06006B97 RID: 27543 RVA: 0x0018CF2E File Offset: 0x0018BF2E
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Status.Gripper.normal == null)
						{
							VisualStyleElement.Status.Gripper.normal = new VisualStyleElement(VisualStyleElement.Status.className, VisualStyleElement.Status.Gripper.part, 0);
						}
						return VisualStyleElement.Status.Gripper.normal;
					}
				}

				// Token: 0x04003F8C RID: 16268
				private static readonly int part = 3;

				// Token: 0x04003F8D RID: 16269
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x0200083B RID: 2107
		public static class TaskBand
		{
			// Token: 0x04003F8E RID: 16270
			private static readonly string className = "TASKBAND";

			// Token: 0x0200083C RID: 2108
			public static class GroupCount
			{
				// Token: 0x17001781 RID: 6017
				// (get) Token: 0x06006B9A RID: 27546 RVA: 0x0018CF65 File Offset: 0x0018BF65
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TaskBand.GroupCount.normal == null)
						{
							VisualStyleElement.TaskBand.GroupCount.normal = new VisualStyleElement(VisualStyleElement.TaskBand.className, VisualStyleElement.TaskBand.GroupCount.part, 0);
						}
						return VisualStyleElement.TaskBand.GroupCount.normal;
					}
				}

				// Token: 0x04003F8F RID: 16271
				private static readonly int part = 1;

				// Token: 0x04003F90 RID: 16272
				private static VisualStyleElement normal;
			}

			// Token: 0x0200083D RID: 2109
			public static class FlashButton
			{
				// Token: 0x17001782 RID: 6018
				// (get) Token: 0x06006B9C RID: 27548 RVA: 0x0018CF90 File Offset: 0x0018BF90
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TaskBand.FlashButton.normal == null)
						{
							VisualStyleElement.TaskBand.FlashButton.normal = new VisualStyleElement(VisualStyleElement.TaskBand.className, VisualStyleElement.TaskBand.FlashButton.part, 0);
						}
						return VisualStyleElement.TaskBand.FlashButton.normal;
					}
				}

				// Token: 0x04003F91 RID: 16273
				private static readonly int part = 2;

				// Token: 0x04003F92 RID: 16274
				private static VisualStyleElement normal;
			}

			// Token: 0x0200083E RID: 2110
			public static class FlashButtonGroupMenu
			{
				// Token: 0x17001783 RID: 6019
				// (get) Token: 0x06006B9E RID: 27550 RVA: 0x0018CFBB File Offset: 0x0018BFBB
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TaskBand.FlashButtonGroupMenu.normal == null)
						{
							VisualStyleElement.TaskBand.FlashButtonGroupMenu.normal = new VisualStyleElement(VisualStyleElement.TaskBand.className, VisualStyleElement.TaskBand.FlashButtonGroupMenu.part, 0);
						}
						return VisualStyleElement.TaskBand.FlashButtonGroupMenu.normal;
					}
				}

				// Token: 0x04003F93 RID: 16275
				private static readonly int part = 3;

				// Token: 0x04003F94 RID: 16276
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x0200083F RID: 2111
		public static class TaskbarClock
		{
			// Token: 0x04003F95 RID: 16277
			private static readonly string className = "CLOCK";

			// Token: 0x02000840 RID: 2112
			public static class Time
			{
				// Token: 0x17001784 RID: 6020
				// (get) Token: 0x06006BA1 RID: 27553 RVA: 0x0018CFF2 File Offset: 0x0018BFF2
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TaskbarClock.Time.normal == null)
						{
							VisualStyleElement.TaskbarClock.Time.normal = new VisualStyleElement(VisualStyleElement.TaskbarClock.className, VisualStyleElement.TaskbarClock.Time.part, 1);
						}
						return VisualStyleElement.TaskbarClock.Time.normal;
					}
				}

				// Token: 0x04003F96 RID: 16278
				private static readonly int part = 1;

				// Token: 0x04003F97 RID: 16279
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x02000841 RID: 2113
		public static class Taskbar
		{
			// Token: 0x04003F98 RID: 16280
			private static readonly string className = "TASKBAR";

			// Token: 0x02000842 RID: 2114
			public static class BackgroundBottom
			{
				// Token: 0x17001785 RID: 6021
				// (get) Token: 0x06006BA4 RID: 27556 RVA: 0x0018D029 File Offset: 0x0018C029
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.BackgroundBottom.normal == null)
						{
							VisualStyleElement.Taskbar.BackgroundBottom.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.BackgroundBottom.part, 0);
						}
						return VisualStyleElement.Taskbar.BackgroundBottom.normal;
					}
				}

				// Token: 0x04003F99 RID: 16281
				private static readonly int part = 1;

				// Token: 0x04003F9A RID: 16282
				private static VisualStyleElement normal;
			}

			// Token: 0x02000843 RID: 2115
			public static class BackgroundRight
			{
				// Token: 0x17001786 RID: 6022
				// (get) Token: 0x06006BA6 RID: 27558 RVA: 0x0018D054 File Offset: 0x0018C054
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.BackgroundRight.normal == null)
						{
							VisualStyleElement.Taskbar.BackgroundRight.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.BackgroundRight.part, 0);
						}
						return VisualStyleElement.Taskbar.BackgroundRight.normal;
					}
				}

				// Token: 0x04003F9B RID: 16283
				private static readonly int part = 2;

				// Token: 0x04003F9C RID: 16284
				private static VisualStyleElement normal;
			}

			// Token: 0x02000844 RID: 2116
			public static class BackgroundTop
			{
				// Token: 0x17001787 RID: 6023
				// (get) Token: 0x06006BA8 RID: 27560 RVA: 0x0018D07F File Offset: 0x0018C07F
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.BackgroundTop.normal == null)
						{
							VisualStyleElement.Taskbar.BackgroundTop.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.BackgroundTop.part, 0);
						}
						return VisualStyleElement.Taskbar.BackgroundTop.normal;
					}
				}

				// Token: 0x04003F9D RID: 16285
				private static readonly int part = 3;

				// Token: 0x04003F9E RID: 16286
				private static VisualStyleElement normal;
			}

			// Token: 0x02000845 RID: 2117
			public static class BackgroundLeft
			{
				// Token: 0x17001788 RID: 6024
				// (get) Token: 0x06006BAA RID: 27562 RVA: 0x0018D0AA File Offset: 0x0018C0AA
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.BackgroundLeft.normal == null)
						{
							VisualStyleElement.Taskbar.BackgroundLeft.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.BackgroundLeft.part, 0);
						}
						return VisualStyleElement.Taskbar.BackgroundLeft.normal;
					}
				}

				// Token: 0x04003F9F RID: 16287
				private static readonly int part = 4;

				// Token: 0x04003FA0 RID: 16288
				private static VisualStyleElement normal;
			}

			// Token: 0x02000846 RID: 2118
			public static class SizingBarBottom
			{
				// Token: 0x17001789 RID: 6025
				// (get) Token: 0x06006BAC RID: 27564 RVA: 0x0018D0D5 File Offset: 0x0018C0D5
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.SizingBarBottom.normal == null)
						{
							VisualStyleElement.Taskbar.SizingBarBottom.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.SizingBarBottom.part, 0);
						}
						return VisualStyleElement.Taskbar.SizingBarBottom.normal;
					}
				}

				// Token: 0x04003FA1 RID: 16289
				private static readonly int part = 5;

				// Token: 0x04003FA2 RID: 16290
				private static VisualStyleElement normal;
			}

			// Token: 0x02000847 RID: 2119
			public static class SizingBarRight
			{
				// Token: 0x1700178A RID: 6026
				// (get) Token: 0x06006BAE RID: 27566 RVA: 0x0018D100 File Offset: 0x0018C100
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.SizingBarRight.normal == null)
						{
							VisualStyleElement.Taskbar.SizingBarRight.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.SizingBarRight.part, 0);
						}
						return VisualStyleElement.Taskbar.SizingBarRight.normal;
					}
				}

				// Token: 0x04003FA3 RID: 16291
				private static readonly int part = 6;

				// Token: 0x04003FA4 RID: 16292
				private static VisualStyleElement normal;
			}

			// Token: 0x02000848 RID: 2120
			public static class SizingBarTop
			{
				// Token: 0x1700178B RID: 6027
				// (get) Token: 0x06006BB0 RID: 27568 RVA: 0x0018D12B File Offset: 0x0018C12B
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.SizingBarTop.normal == null)
						{
							VisualStyleElement.Taskbar.SizingBarTop.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.SizingBarTop.part, 0);
						}
						return VisualStyleElement.Taskbar.SizingBarTop.normal;
					}
				}

				// Token: 0x04003FA5 RID: 16293
				private static readonly int part = 7;

				// Token: 0x04003FA6 RID: 16294
				private static VisualStyleElement normal;
			}

			// Token: 0x02000849 RID: 2121
			public static class SizingBarLeft
			{
				// Token: 0x1700178C RID: 6028
				// (get) Token: 0x06006BB2 RID: 27570 RVA: 0x0018D156 File Offset: 0x0018C156
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Taskbar.SizingBarLeft.normal == null)
						{
							VisualStyleElement.Taskbar.SizingBarLeft.normal = new VisualStyleElement(VisualStyleElement.Taskbar.className, VisualStyleElement.Taskbar.SizingBarLeft.part, 0);
						}
						return VisualStyleElement.Taskbar.SizingBarLeft.normal;
					}
				}

				// Token: 0x04003FA7 RID: 16295
				private static readonly int part = 8;

				// Token: 0x04003FA8 RID: 16296
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x0200084A RID: 2122
		public static class ToolBar
		{
			// Token: 0x04003FA9 RID: 16297
			private static readonly string className = "TOOLBAR";

			// Token: 0x0200084B RID: 2123
			internal static class Bar
			{
				// Token: 0x1700178D RID: 6029
				// (get) Token: 0x06006BB5 RID: 27573 RVA: 0x0018D18D File Offset: 0x0018C18D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolBar.Bar.normal == null)
						{
							VisualStyleElement.ToolBar.Bar.normal = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.Bar.part, 0);
						}
						return VisualStyleElement.ToolBar.Bar.normal;
					}
				}

				// Token: 0x04003FAA RID: 16298
				private static readonly int part;

				// Token: 0x04003FAB RID: 16299
				private static VisualStyleElement normal;
			}

			// Token: 0x0200084C RID: 2124
			public static class Button
			{
				// Token: 0x1700178E RID: 6030
				// (get) Token: 0x06006BB6 RID: 27574 RVA: 0x0018D1B0 File Offset: 0x0018C1B0
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolBar.Button.normal == null)
						{
							VisualStyleElement.ToolBar.Button.normal = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.Button.part, 1);
						}
						return VisualStyleElement.ToolBar.Button.normal;
					}
				}

				// Token: 0x1700178F RID: 6031
				// (get) Token: 0x06006BB7 RID: 27575 RVA: 0x0018D1D3 File Offset: 0x0018C1D3
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ToolBar.Button.hot == null)
						{
							VisualStyleElement.ToolBar.Button.hot = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.Button.part, 2);
						}
						return VisualStyleElement.ToolBar.Button.hot;
					}
				}

				// Token: 0x17001790 RID: 6032
				// (get) Token: 0x06006BB8 RID: 27576 RVA: 0x0018D1F6 File Offset: 0x0018C1F6
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ToolBar.Button.pressed == null)
						{
							VisualStyleElement.ToolBar.Button.pressed = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.Button.part, 3);
						}
						return VisualStyleElement.ToolBar.Button.pressed;
					}
				}

				// Token: 0x17001791 RID: 6033
				// (get) Token: 0x06006BB9 RID: 27577 RVA: 0x0018D219 File Offset: 0x0018C219
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ToolBar.Button.disabled == null)
						{
							VisualStyleElement.ToolBar.Button.disabled = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.Button.part, 4);
						}
						return VisualStyleElement.ToolBar.Button.disabled;
					}
				}

				// Token: 0x17001792 RID: 6034
				// (get) Token: 0x06006BBA RID: 27578 RVA: 0x0018D23C File Offset: 0x0018C23C
				public static VisualStyleElement Checked
				{
					get
					{
						if (VisualStyleElement.ToolBar.Button._checked == null)
						{
							VisualStyleElement.ToolBar.Button._checked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.Button.part, 5);
						}
						return VisualStyleElement.ToolBar.Button._checked;
					}
				}

				// Token: 0x17001793 RID: 6035
				// (get) Token: 0x06006BBB RID: 27579 RVA: 0x0018D25F File Offset: 0x0018C25F
				public static VisualStyleElement HotChecked
				{
					get
					{
						if (VisualStyleElement.ToolBar.Button.hotchecked == null)
						{
							VisualStyleElement.ToolBar.Button.hotchecked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.Button.part, 6);
						}
						return VisualStyleElement.ToolBar.Button.hotchecked;
					}
				}

				// Token: 0x04003FAC RID: 16300
				private static readonly int part = 1;

				// Token: 0x04003FAD RID: 16301
				private static VisualStyleElement normal;

				// Token: 0x04003FAE RID: 16302
				private static VisualStyleElement hot;

				// Token: 0x04003FAF RID: 16303
				private static VisualStyleElement pressed;

				// Token: 0x04003FB0 RID: 16304
				private static VisualStyleElement disabled;

				// Token: 0x04003FB1 RID: 16305
				private static VisualStyleElement _checked;

				// Token: 0x04003FB2 RID: 16306
				private static VisualStyleElement hotchecked;
			}

			// Token: 0x0200084D RID: 2125
			public static class DropDownButton
			{
				// Token: 0x17001794 RID: 6036
				// (get) Token: 0x06006BBD RID: 27581 RVA: 0x0018D28A File Offset: 0x0018C28A
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolBar.DropDownButton.normal == null)
						{
							VisualStyleElement.ToolBar.DropDownButton.normal = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.DropDownButton.part, 1);
						}
						return VisualStyleElement.ToolBar.DropDownButton.normal;
					}
				}

				// Token: 0x17001795 RID: 6037
				// (get) Token: 0x06006BBE RID: 27582 RVA: 0x0018D2AD File Offset: 0x0018C2AD
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ToolBar.DropDownButton.hot == null)
						{
							VisualStyleElement.ToolBar.DropDownButton.hot = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.DropDownButton.part, 2);
						}
						return VisualStyleElement.ToolBar.DropDownButton.hot;
					}
				}

				// Token: 0x17001796 RID: 6038
				// (get) Token: 0x06006BBF RID: 27583 RVA: 0x0018D2D0 File Offset: 0x0018C2D0
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ToolBar.DropDownButton.pressed == null)
						{
							VisualStyleElement.ToolBar.DropDownButton.pressed = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.DropDownButton.part, 3);
						}
						return VisualStyleElement.ToolBar.DropDownButton.pressed;
					}
				}

				// Token: 0x17001797 RID: 6039
				// (get) Token: 0x06006BC0 RID: 27584 RVA: 0x0018D2F3 File Offset: 0x0018C2F3
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ToolBar.DropDownButton.disabled == null)
						{
							VisualStyleElement.ToolBar.DropDownButton.disabled = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.DropDownButton.part, 4);
						}
						return VisualStyleElement.ToolBar.DropDownButton.disabled;
					}
				}

				// Token: 0x17001798 RID: 6040
				// (get) Token: 0x06006BC1 RID: 27585 RVA: 0x0018D316 File Offset: 0x0018C316
				public static VisualStyleElement Checked
				{
					get
					{
						if (VisualStyleElement.ToolBar.DropDownButton._checked == null)
						{
							VisualStyleElement.ToolBar.DropDownButton._checked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.DropDownButton.part, 5);
						}
						return VisualStyleElement.ToolBar.DropDownButton._checked;
					}
				}

				// Token: 0x17001799 RID: 6041
				// (get) Token: 0x06006BC2 RID: 27586 RVA: 0x0018D339 File Offset: 0x0018C339
				public static VisualStyleElement HotChecked
				{
					get
					{
						if (VisualStyleElement.ToolBar.DropDownButton.hotchecked == null)
						{
							VisualStyleElement.ToolBar.DropDownButton.hotchecked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.DropDownButton.part, 6);
						}
						return VisualStyleElement.ToolBar.DropDownButton.hotchecked;
					}
				}

				// Token: 0x04003FB3 RID: 16307
				private static readonly int part = 2;

				// Token: 0x04003FB4 RID: 16308
				private static VisualStyleElement normal;

				// Token: 0x04003FB5 RID: 16309
				private static VisualStyleElement hot;

				// Token: 0x04003FB6 RID: 16310
				private static VisualStyleElement pressed;

				// Token: 0x04003FB7 RID: 16311
				private static VisualStyleElement disabled;

				// Token: 0x04003FB8 RID: 16312
				private static VisualStyleElement _checked;

				// Token: 0x04003FB9 RID: 16313
				private static VisualStyleElement hotchecked;
			}

			// Token: 0x0200084E RID: 2126
			public static class SplitButton
			{
				// Token: 0x1700179A RID: 6042
				// (get) Token: 0x06006BC4 RID: 27588 RVA: 0x0018D364 File Offset: 0x0018C364
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButton.normal == null)
						{
							VisualStyleElement.ToolBar.SplitButton.normal = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButton.part, 1);
						}
						return VisualStyleElement.ToolBar.SplitButton.normal;
					}
				}

				// Token: 0x1700179B RID: 6043
				// (get) Token: 0x06006BC5 RID: 27589 RVA: 0x0018D387 File Offset: 0x0018C387
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButton.hot == null)
						{
							VisualStyleElement.ToolBar.SplitButton.hot = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButton.part, 2);
						}
						return VisualStyleElement.ToolBar.SplitButton.hot;
					}
				}

				// Token: 0x1700179C RID: 6044
				// (get) Token: 0x06006BC6 RID: 27590 RVA: 0x0018D3AA File Offset: 0x0018C3AA
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButton.pressed == null)
						{
							VisualStyleElement.ToolBar.SplitButton.pressed = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButton.part, 3);
						}
						return VisualStyleElement.ToolBar.SplitButton.pressed;
					}
				}

				// Token: 0x1700179D RID: 6045
				// (get) Token: 0x06006BC7 RID: 27591 RVA: 0x0018D3CD File Offset: 0x0018C3CD
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButton.disabled == null)
						{
							VisualStyleElement.ToolBar.SplitButton.disabled = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButton.part, 4);
						}
						return VisualStyleElement.ToolBar.SplitButton.disabled;
					}
				}

				// Token: 0x1700179E RID: 6046
				// (get) Token: 0x06006BC8 RID: 27592 RVA: 0x0018D3F0 File Offset: 0x0018C3F0
				public static VisualStyleElement Checked
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButton._checked == null)
						{
							VisualStyleElement.ToolBar.SplitButton._checked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButton.part, 5);
						}
						return VisualStyleElement.ToolBar.SplitButton._checked;
					}
				}

				// Token: 0x1700179F RID: 6047
				// (get) Token: 0x06006BC9 RID: 27593 RVA: 0x0018D413 File Offset: 0x0018C413
				public static VisualStyleElement HotChecked
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButton.hotchecked == null)
						{
							VisualStyleElement.ToolBar.SplitButton.hotchecked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButton.part, 6);
						}
						return VisualStyleElement.ToolBar.SplitButton.hotchecked;
					}
				}

				// Token: 0x04003FBA RID: 16314
				private static readonly int part = 3;

				// Token: 0x04003FBB RID: 16315
				private static VisualStyleElement normal;

				// Token: 0x04003FBC RID: 16316
				private static VisualStyleElement hot;

				// Token: 0x04003FBD RID: 16317
				private static VisualStyleElement pressed;

				// Token: 0x04003FBE RID: 16318
				private static VisualStyleElement disabled;

				// Token: 0x04003FBF RID: 16319
				private static VisualStyleElement _checked;

				// Token: 0x04003FC0 RID: 16320
				private static VisualStyleElement hotchecked;
			}

			// Token: 0x0200084F RID: 2127
			public static class SplitButtonDropDown
			{
				// Token: 0x170017A0 RID: 6048
				// (get) Token: 0x06006BCB RID: 27595 RVA: 0x0018D43E File Offset: 0x0018C43E
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButtonDropDown.normal == null)
						{
							VisualStyleElement.ToolBar.SplitButtonDropDown.normal = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButtonDropDown.part, 1);
						}
						return VisualStyleElement.ToolBar.SplitButtonDropDown.normal;
					}
				}

				// Token: 0x170017A1 RID: 6049
				// (get) Token: 0x06006BCC RID: 27596 RVA: 0x0018D461 File Offset: 0x0018C461
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButtonDropDown.hot == null)
						{
							VisualStyleElement.ToolBar.SplitButtonDropDown.hot = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButtonDropDown.part, 2);
						}
						return VisualStyleElement.ToolBar.SplitButtonDropDown.hot;
					}
				}

				// Token: 0x170017A2 RID: 6050
				// (get) Token: 0x06006BCD RID: 27597 RVA: 0x0018D484 File Offset: 0x0018C484
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButtonDropDown.pressed == null)
						{
							VisualStyleElement.ToolBar.SplitButtonDropDown.pressed = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButtonDropDown.part, 3);
						}
						return VisualStyleElement.ToolBar.SplitButtonDropDown.pressed;
					}
				}

				// Token: 0x170017A3 RID: 6051
				// (get) Token: 0x06006BCE RID: 27598 RVA: 0x0018D4A7 File Offset: 0x0018C4A7
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButtonDropDown.disabled == null)
						{
							VisualStyleElement.ToolBar.SplitButtonDropDown.disabled = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButtonDropDown.part, 4);
						}
						return VisualStyleElement.ToolBar.SplitButtonDropDown.disabled;
					}
				}

				// Token: 0x170017A4 RID: 6052
				// (get) Token: 0x06006BCF RID: 27599 RVA: 0x0018D4CA File Offset: 0x0018C4CA
				public static VisualStyleElement Checked
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButtonDropDown._checked == null)
						{
							VisualStyleElement.ToolBar.SplitButtonDropDown._checked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButtonDropDown.part, 5);
						}
						return VisualStyleElement.ToolBar.SplitButtonDropDown._checked;
					}
				}

				// Token: 0x170017A5 RID: 6053
				// (get) Token: 0x06006BD0 RID: 27600 RVA: 0x0018D4ED File Offset: 0x0018C4ED
				public static VisualStyleElement HotChecked
				{
					get
					{
						if (VisualStyleElement.ToolBar.SplitButtonDropDown.hotchecked == null)
						{
							VisualStyleElement.ToolBar.SplitButtonDropDown.hotchecked = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SplitButtonDropDown.part, 6);
						}
						return VisualStyleElement.ToolBar.SplitButtonDropDown.hotchecked;
					}
				}

				// Token: 0x04003FC1 RID: 16321
				private static readonly int part = 4;

				// Token: 0x04003FC2 RID: 16322
				private static VisualStyleElement normal;

				// Token: 0x04003FC3 RID: 16323
				private static VisualStyleElement hot;

				// Token: 0x04003FC4 RID: 16324
				private static VisualStyleElement pressed;

				// Token: 0x04003FC5 RID: 16325
				private static VisualStyleElement disabled;

				// Token: 0x04003FC6 RID: 16326
				private static VisualStyleElement _checked;

				// Token: 0x04003FC7 RID: 16327
				private static VisualStyleElement hotchecked;
			}

			// Token: 0x02000850 RID: 2128
			public static class SeparatorHorizontal
			{
				// Token: 0x170017A6 RID: 6054
				// (get) Token: 0x06006BD2 RID: 27602 RVA: 0x0018D518 File Offset: 0x0018C518
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolBar.SeparatorHorizontal.normal == null)
						{
							VisualStyleElement.ToolBar.SeparatorHorizontal.normal = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SeparatorHorizontal.part, 0);
						}
						return VisualStyleElement.ToolBar.SeparatorHorizontal.normal;
					}
				}

				// Token: 0x04003FC8 RID: 16328
				private static readonly int part = 5;

				// Token: 0x04003FC9 RID: 16329
				private static VisualStyleElement normal;
			}

			// Token: 0x02000851 RID: 2129
			public static class SeparatorVertical
			{
				// Token: 0x170017A7 RID: 6055
				// (get) Token: 0x06006BD4 RID: 27604 RVA: 0x0018D543 File Offset: 0x0018C543
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolBar.SeparatorVertical.normal == null)
						{
							VisualStyleElement.ToolBar.SeparatorVertical.normal = new VisualStyleElement(VisualStyleElement.ToolBar.className, VisualStyleElement.ToolBar.SeparatorVertical.part, 0);
						}
						return VisualStyleElement.ToolBar.SeparatorVertical.normal;
					}
				}

				// Token: 0x04003FCA RID: 16330
				private static readonly int part = 6;

				// Token: 0x04003FCB RID: 16331
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x02000852 RID: 2130
		public static class ToolTip
		{
			// Token: 0x04003FCC RID: 16332
			private static readonly string className = "TOOLTIP";

			// Token: 0x02000853 RID: 2131
			public static class Standard
			{
				// Token: 0x170017A8 RID: 6056
				// (get) Token: 0x06006BD7 RID: 27607 RVA: 0x0018D57A File Offset: 0x0018C57A
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolTip.Standard.normal == null)
						{
							VisualStyleElement.ToolTip.Standard.normal = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.Standard.part, 1);
						}
						return VisualStyleElement.ToolTip.Standard.normal;
					}
				}

				// Token: 0x170017A9 RID: 6057
				// (get) Token: 0x06006BD8 RID: 27608 RVA: 0x0018D59D File Offset: 0x0018C59D
				public static VisualStyleElement Link
				{
					get
					{
						if (VisualStyleElement.ToolTip.Standard.link == null)
						{
							VisualStyleElement.ToolTip.Standard.link = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.Standard.part, 2);
						}
						return VisualStyleElement.ToolTip.Standard.link;
					}
				}

				// Token: 0x04003FCD RID: 16333
				private static readonly int part = 1;

				// Token: 0x04003FCE RID: 16334
				private static VisualStyleElement normal;

				// Token: 0x04003FCF RID: 16335
				private static VisualStyleElement link;
			}

			// Token: 0x02000854 RID: 2132
			public static class StandardTitle
			{
				// Token: 0x170017AA RID: 6058
				// (get) Token: 0x06006BDA RID: 27610 RVA: 0x0018D5C8 File Offset: 0x0018C5C8
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolTip.StandardTitle.normal == null)
						{
							VisualStyleElement.ToolTip.StandardTitle.normal = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.StandardTitle.part, 0);
						}
						return VisualStyleElement.ToolTip.StandardTitle.normal;
					}
				}

				// Token: 0x04003FD0 RID: 16336
				private static readonly int part = 2;

				// Token: 0x04003FD1 RID: 16337
				private static VisualStyleElement normal;
			}

			// Token: 0x02000855 RID: 2133
			public static class Balloon
			{
				// Token: 0x170017AB RID: 6059
				// (get) Token: 0x06006BDC RID: 27612 RVA: 0x0018D5F3 File Offset: 0x0018C5F3
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolTip.Balloon.normal == null)
						{
							VisualStyleElement.ToolTip.Balloon.normal = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.Balloon.part, 1);
						}
						return VisualStyleElement.ToolTip.Balloon.normal;
					}
				}

				// Token: 0x170017AC RID: 6060
				// (get) Token: 0x06006BDD RID: 27613 RVA: 0x0018D616 File Offset: 0x0018C616
				public static VisualStyleElement Link
				{
					get
					{
						if (VisualStyleElement.ToolTip.Balloon.link == null)
						{
							VisualStyleElement.ToolTip.Balloon.link = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.Balloon.part, 2);
						}
						return VisualStyleElement.ToolTip.Balloon.link;
					}
				}

				// Token: 0x04003FD2 RID: 16338
				private static readonly int part = 3;

				// Token: 0x04003FD3 RID: 16339
				private static VisualStyleElement normal;

				// Token: 0x04003FD4 RID: 16340
				private static VisualStyleElement link;
			}

			// Token: 0x02000856 RID: 2134
			public static class BalloonTitle
			{
				// Token: 0x170017AD RID: 6061
				// (get) Token: 0x06006BDF RID: 27615 RVA: 0x0018D641 File Offset: 0x0018C641
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolTip.BalloonTitle.normal == null)
						{
							VisualStyleElement.ToolTip.BalloonTitle.normal = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.BalloonTitle.part, 0);
						}
						return VisualStyleElement.ToolTip.BalloonTitle.normal;
					}
				}

				// Token: 0x04003FD5 RID: 16341
				private static readonly int part = 4;

				// Token: 0x04003FD6 RID: 16342
				private static VisualStyleElement normal;
			}

			// Token: 0x02000857 RID: 2135
			public static class Close
			{
				// Token: 0x170017AE RID: 6062
				// (get) Token: 0x06006BE1 RID: 27617 RVA: 0x0018D66C File Offset: 0x0018C66C
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.ToolTip.Close.normal == null)
						{
							VisualStyleElement.ToolTip.Close.normal = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.Close.part, 1);
						}
						return VisualStyleElement.ToolTip.Close.normal;
					}
				}

				// Token: 0x170017AF RID: 6063
				// (get) Token: 0x06006BE2 RID: 27618 RVA: 0x0018D68F File Offset: 0x0018C68F
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.ToolTip.Close.hot == null)
						{
							VisualStyleElement.ToolTip.Close.hot = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.Close.part, 2);
						}
						return VisualStyleElement.ToolTip.Close.hot;
					}
				}

				// Token: 0x170017B0 RID: 6064
				// (get) Token: 0x06006BE3 RID: 27619 RVA: 0x0018D6B2 File Offset: 0x0018C6B2
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.ToolTip.Close.pressed == null)
						{
							VisualStyleElement.ToolTip.Close.pressed = new VisualStyleElement(VisualStyleElement.ToolTip.className, VisualStyleElement.ToolTip.Close.part, 3);
						}
						return VisualStyleElement.ToolTip.Close.pressed;
					}
				}

				// Token: 0x04003FD7 RID: 16343
				private static readonly int part = 5;

				// Token: 0x04003FD8 RID: 16344
				private static VisualStyleElement normal;

				// Token: 0x04003FD9 RID: 16345
				private static VisualStyleElement hot;

				// Token: 0x04003FDA RID: 16346
				private static VisualStyleElement pressed;
			}
		}

		// Token: 0x02000858 RID: 2136
		public static class TrackBar
		{
			// Token: 0x04003FDB RID: 16347
			private static readonly string className = "TRACKBAR";

			// Token: 0x02000859 RID: 2137
			public static class Track
			{
				// Token: 0x170017B1 RID: 6065
				// (get) Token: 0x06006BE6 RID: 27622 RVA: 0x0018D6E9 File Offset: 0x0018C6E9
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.Track.normal == null)
						{
							VisualStyleElement.TrackBar.Track.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.Track.part, 1);
						}
						return VisualStyleElement.TrackBar.Track.normal;
					}
				}

				// Token: 0x04003FDC RID: 16348
				private static readonly int part = 1;

				// Token: 0x04003FDD RID: 16349
				private static VisualStyleElement normal;
			}

			// Token: 0x0200085A RID: 2138
			public static class TrackVertical
			{
				// Token: 0x170017B2 RID: 6066
				// (get) Token: 0x06006BE8 RID: 27624 RVA: 0x0018D714 File Offset: 0x0018C714
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.TrackVertical.normal == null)
						{
							VisualStyleElement.TrackBar.TrackVertical.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.TrackVertical.part, 1);
						}
						return VisualStyleElement.TrackBar.TrackVertical.normal;
					}
				}

				// Token: 0x04003FDE RID: 16350
				private static readonly int part = 2;

				// Token: 0x04003FDF RID: 16351
				private static VisualStyleElement normal;
			}

			// Token: 0x0200085B RID: 2139
			public static class Thumb
			{
				// Token: 0x170017B3 RID: 6067
				// (get) Token: 0x06006BEA RID: 27626 RVA: 0x0018D73F File Offset: 0x0018C73F
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.Thumb.normal == null)
						{
							VisualStyleElement.TrackBar.Thumb.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.Thumb.part, 1);
						}
						return VisualStyleElement.TrackBar.Thumb.normal;
					}
				}

				// Token: 0x170017B4 RID: 6068
				// (get) Token: 0x06006BEB RID: 27627 RVA: 0x0018D762 File Offset: 0x0018C762
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TrackBar.Thumb.hot == null)
						{
							VisualStyleElement.TrackBar.Thumb.hot = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.Thumb.part, 2);
						}
						return VisualStyleElement.TrackBar.Thumb.hot;
					}
				}

				// Token: 0x170017B5 RID: 6069
				// (get) Token: 0x06006BEC RID: 27628 RVA: 0x0018D785 File Offset: 0x0018C785
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.TrackBar.Thumb.pressed == null)
						{
							VisualStyleElement.TrackBar.Thumb.pressed = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.Thumb.part, 3);
						}
						return VisualStyleElement.TrackBar.Thumb.pressed;
					}
				}

				// Token: 0x170017B6 RID: 6070
				// (get) Token: 0x06006BED RID: 27629 RVA: 0x0018D7A8 File Offset: 0x0018C7A8
				public static VisualStyleElement Focused
				{
					get
					{
						if (VisualStyleElement.TrackBar.Thumb.focused == null)
						{
							VisualStyleElement.TrackBar.Thumb.focused = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.Thumb.part, 4);
						}
						return VisualStyleElement.TrackBar.Thumb.focused;
					}
				}

				// Token: 0x170017B7 RID: 6071
				// (get) Token: 0x06006BEE RID: 27630 RVA: 0x0018D7CB File Offset: 0x0018C7CB
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TrackBar.Thumb.disabled == null)
						{
							VisualStyleElement.TrackBar.Thumb.disabled = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.Thumb.part, 5);
						}
						return VisualStyleElement.TrackBar.Thumb.disabled;
					}
				}

				// Token: 0x04003FE0 RID: 16352
				private static readonly int part = 3;

				// Token: 0x04003FE1 RID: 16353
				private static VisualStyleElement normal;

				// Token: 0x04003FE2 RID: 16354
				private static VisualStyleElement hot;

				// Token: 0x04003FE3 RID: 16355
				private static VisualStyleElement pressed;

				// Token: 0x04003FE4 RID: 16356
				private static VisualStyleElement focused;

				// Token: 0x04003FE5 RID: 16357
				private static VisualStyleElement disabled;
			}

			// Token: 0x0200085C RID: 2140
			public static class ThumbBottom
			{
				// Token: 0x170017B8 RID: 6072
				// (get) Token: 0x06006BF0 RID: 27632 RVA: 0x0018D7F6 File Offset: 0x0018C7F6
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbBottom.normal == null)
						{
							VisualStyleElement.TrackBar.ThumbBottom.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbBottom.part, 1);
						}
						return VisualStyleElement.TrackBar.ThumbBottom.normal;
					}
				}

				// Token: 0x170017B9 RID: 6073
				// (get) Token: 0x06006BF1 RID: 27633 RVA: 0x0018D819 File Offset: 0x0018C819
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbBottom.hot == null)
						{
							VisualStyleElement.TrackBar.ThumbBottom.hot = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbBottom.part, 2);
						}
						return VisualStyleElement.TrackBar.ThumbBottom.hot;
					}
				}

				// Token: 0x170017BA RID: 6074
				// (get) Token: 0x06006BF2 RID: 27634 RVA: 0x0018D83C File Offset: 0x0018C83C
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbBottom.pressed == null)
						{
							VisualStyleElement.TrackBar.ThumbBottom.pressed = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbBottom.part, 3);
						}
						return VisualStyleElement.TrackBar.ThumbBottom.pressed;
					}
				}

				// Token: 0x170017BB RID: 6075
				// (get) Token: 0x06006BF3 RID: 27635 RVA: 0x0018D85F File Offset: 0x0018C85F
				public static VisualStyleElement Focused
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbBottom.focused == null)
						{
							VisualStyleElement.TrackBar.ThumbBottom.focused = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbBottom.part, 4);
						}
						return VisualStyleElement.TrackBar.ThumbBottom.focused;
					}
				}

				// Token: 0x170017BC RID: 6076
				// (get) Token: 0x06006BF4 RID: 27636 RVA: 0x0018D882 File Offset: 0x0018C882
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbBottom.disabled == null)
						{
							VisualStyleElement.TrackBar.ThumbBottom.disabled = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbBottom.part, 5);
						}
						return VisualStyleElement.TrackBar.ThumbBottom.disabled;
					}
				}

				// Token: 0x04003FE6 RID: 16358
				private static readonly int part = 4;

				// Token: 0x04003FE7 RID: 16359
				private static VisualStyleElement normal;

				// Token: 0x04003FE8 RID: 16360
				private static VisualStyleElement hot;

				// Token: 0x04003FE9 RID: 16361
				private static VisualStyleElement pressed;

				// Token: 0x04003FEA RID: 16362
				private static VisualStyleElement focused;

				// Token: 0x04003FEB RID: 16363
				private static VisualStyleElement disabled;
			}

			// Token: 0x0200085D RID: 2141
			public static class ThumbTop
			{
				// Token: 0x170017BD RID: 6077
				// (get) Token: 0x06006BF6 RID: 27638 RVA: 0x0018D8AD File Offset: 0x0018C8AD
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbTop.normal == null)
						{
							VisualStyleElement.TrackBar.ThumbTop.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbTop.part, 1);
						}
						return VisualStyleElement.TrackBar.ThumbTop.normal;
					}
				}

				// Token: 0x170017BE RID: 6078
				// (get) Token: 0x06006BF7 RID: 27639 RVA: 0x0018D8D0 File Offset: 0x0018C8D0
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbTop.hot == null)
						{
							VisualStyleElement.TrackBar.ThumbTop.hot = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbTop.part, 2);
						}
						return VisualStyleElement.TrackBar.ThumbTop.hot;
					}
				}

				// Token: 0x170017BF RID: 6079
				// (get) Token: 0x06006BF8 RID: 27640 RVA: 0x0018D8F3 File Offset: 0x0018C8F3
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbTop.pressed == null)
						{
							VisualStyleElement.TrackBar.ThumbTop.pressed = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbTop.part, 3);
						}
						return VisualStyleElement.TrackBar.ThumbTop.pressed;
					}
				}

				// Token: 0x170017C0 RID: 6080
				// (get) Token: 0x06006BF9 RID: 27641 RVA: 0x0018D916 File Offset: 0x0018C916
				public static VisualStyleElement Focused
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbTop.focused == null)
						{
							VisualStyleElement.TrackBar.ThumbTop.focused = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbTop.part, 4);
						}
						return VisualStyleElement.TrackBar.ThumbTop.focused;
					}
				}

				// Token: 0x170017C1 RID: 6081
				// (get) Token: 0x06006BFA RID: 27642 RVA: 0x0018D939 File Offset: 0x0018C939
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbTop.disabled == null)
						{
							VisualStyleElement.TrackBar.ThumbTop.disabled = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbTop.part, 5);
						}
						return VisualStyleElement.TrackBar.ThumbTop.disabled;
					}
				}

				// Token: 0x04003FEC RID: 16364
				private static readonly int part = 5;

				// Token: 0x04003FED RID: 16365
				private static VisualStyleElement normal;

				// Token: 0x04003FEE RID: 16366
				private static VisualStyleElement hot;

				// Token: 0x04003FEF RID: 16367
				private static VisualStyleElement pressed;

				// Token: 0x04003FF0 RID: 16368
				private static VisualStyleElement focused;

				// Token: 0x04003FF1 RID: 16369
				private static VisualStyleElement disabled;
			}

			// Token: 0x0200085E RID: 2142
			public static class ThumbVertical
			{
				// Token: 0x170017C2 RID: 6082
				// (get) Token: 0x06006BFC RID: 27644 RVA: 0x0018D964 File Offset: 0x0018C964
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbVertical.normal == null)
						{
							VisualStyleElement.TrackBar.ThumbVertical.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbVertical.part, 1);
						}
						return VisualStyleElement.TrackBar.ThumbVertical.normal;
					}
				}

				// Token: 0x170017C3 RID: 6083
				// (get) Token: 0x06006BFD RID: 27645 RVA: 0x0018D987 File Offset: 0x0018C987
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbVertical.hot == null)
						{
							VisualStyleElement.TrackBar.ThumbVertical.hot = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbVertical.part, 2);
						}
						return VisualStyleElement.TrackBar.ThumbVertical.hot;
					}
				}

				// Token: 0x170017C4 RID: 6084
				// (get) Token: 0x06006BFE RID: 27646 RVA: 0x0018D9AA File Offset: 0x0018C9AA
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbVertical.pressed == null)
						{
							VisualStyleElement.TrackBar.ThumbVertical.pressed = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbVertical.part, 3);
						}
						return VisualStyleElement.TrackBar.ThumbVertical.pressed;
					}
				}

				// Token: 0x170017C5 RID: 6085
				// (get) Token: 0x06006BFF RID: 27647 RVA: 0x0018D9CD File Offset: 0x0018C9CD
				public static VisualStyleElement Focused
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbVertical.focused == null)
						{
							VisualStyleElement.TrackBar.ThumbVertical.focused = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbVertical.part, 4);
						}
						return VisualStyleElement.TrackBar.ThumbVertical.focused;
					}
				}

				// Token: 0x170017C6 RID: 6086
				// (get) Token: 0x06006C00 RID: 27648 RVA: 0x0018D9F0 File Offset: 0x0018C9F0
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbVertical.disabled == null)
						{
							VisualStyleElement.TrackBar.ThumbVertical.disabled = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbVertical.part, 5);
						}
						return VisualStyleElement.TrackBar.ThumbVertical.disabled;
					}
				}

				// Token: 0x04003FF2 RID: 16370
				private static readonly int part = 6;

				// Token: 0x04003FF3 RID: 16371
				private static VisualStyleElement normal;

				// Token: 0x04003FF4 RID: 16372
				private static VisualStyleElement hot;

				// Token: 0x04003FF5 RID: 16373
				private static VisualStyleElement pressed;

				// Token: 0x04003FF6 RID: 16374
				private static VisualStyleElement focused;

				// Token: 0x04003FF7 RID: 16375
				private static VisualStyleElement disabled;
			}

			// Token: 0x0200085F RID: 2143
			public static class ThumbLeft
			{
				// Token: 0x170017C7 RID: 6087
				// (get) Token: 0x06006C02 RID: 27650 RVA: 0x0018DA1B File Offset: 0x0018CA1B
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbLeft.normal == null)
						{
							VisualStyleElement.TrackBar.ThumbLeft.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbLeft.part, 1);
						}
						return VisualStyleElement.TrackBar.ThumbLeft.normal;
					}
				}

				// Token: 0x170017C8 RID: 6088
				// (get) Token: 0x06006C03 RID: 27651 RVA: 0x0018DA3E File Offset: 0x0018CA3E
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbLeft.hot == null)
						{
							VisualStyleElement.TrackBar.ThumbLeft.hot = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbLeft.part, 2);
						}
						return VisualStyleElement.TrackBar.ThumbLeft.hot;
					}
				}

				// Token: 0x170017C9 RID: 6089
				// (get) Token: 0x06006C04 RID: 27652 RVA: 0x0018DA61 File Offset: 0x0018CA61
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbLeft.pressed == null)
						{
							VisualStyleElement.TrackBar.ThumbLeft.pressed = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbLeft.part, 3);
						}
						return VisualStyleElement.TrackBar.ThumbLeft.pressed;
					}
				}

				// Token: 0x170017CA RID: 6090
				// (get) Token: 0x06006C05 RID: 27653 RVA: 0x0018DA84 File Offset: 0x0018CA84
				public static VisualStyleElement Focused
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbLeft.focused == null)
						{
							VisualStyleElement.TrackBar.ThumbLeft.focused = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbLeft.part, 4);
						}
						return VisualStyleElement.TrackBar.ThumbLeft.focused;
					}
				}

				// Token: 0x170017CB RID: 6091
				// (get) Token: 0x06006C06 RID: 27654 RVA: 0x0018DAA7 File Offset: 0x0018CAA7
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbLeft.disabled == null)
						{
							VisualStyleElement.TrackBar.ThumbLeft.disabled = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbLeft.part, 5);
						}
						return VisualStyleElement.TrackBar.ThumbLeft.disabled;
					}
				}

				// Token: 0x04003FF8 RID: 16376
				private static readonly int part = 7;

				// Token: 0x04003FF9 RID: 16377
				private static VisualStyleElement normal;

				// Token: 0x04003FFA RID: 16378
				private static VisualStyleElement hot;

				// Token: 0x04003FFB RID: 16379
				private static VisualStyleElement pressed;

				// Token: 0x04003FFC RID: 16380
				private static VisualStyleElement focused;

				// Token: 0x04003FFD RID: 16381
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000860 RID: 2144
			public static class ThumbRight
			{
				// Token: 0x170017CC RID: 6092
				// (get) Token: 0x06006C08 RID: 27656 RVA: 0x0018DAD2 File Offset: 0x0018CAD2
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbRight.normal == null)
						{
							VisualStyleElement.TrackBar.ThumbRight.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbRight.part, 1);
						}
						return VisualStyleElement.TrackBar.ThumbRight.normal;
					}
				}

				// Token: 0x170017CD RID: 6093
				// (get) Token: 0x06006C09 RID: 27657 RVA: 0x0018DAF5 File Offset: 0x0018CAF5
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbRight.hot == null)
						{
							VisualStyleElement.TrackBar.ThumbRight.hot = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbRight.part, 2);
						}
						return VisualStyleElement.TrackBar.ThumbRight.hot;
					}
				}

				// Token: 0x170017CE RID: 6094
				// (get) Token: 0x06006C0A RID: 27658 RVA: 0x0018DB18 File Offset: 0x0018CB18
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbRight.pressed == null)
						{
							VisualStyleElement.TrackBar.ThumbRight.pressed = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbRight.part, 3);
						}
						return VisualStyleElement.TrackBar.ThumbRight.pressed;
					}
				}

				// Token: 0x170017CF RID: 6095
				// (get) Token: 0x06006C0B RID: 27659 RVA: 0x0018DB3B File Offset: 0x0018CB3B
				public static VisualStyleElement Focused
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbRight.focused == null)
						{
							VisualStyleElement.TrackBar.ThumbRight.focused = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbRight.part, 4);
						}
						return VisualStyleElement.TrackBar.ThumbRight.focused;
					}
				}

				// Token: 0x170017D0 RID: 6096
				// (get) Token: 0x06006C0C RID: 27660 RVA: 0x0018DB5E File Offset: 0x0018CB5E
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TrackBar.ThumbRight.disabled == null)
						{
							VisualStyleElement.TrackBar.ThumbRight.disabled = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.ThumbRight.part, 5);
						}
						return VisualStyleElement.TrackBar.ThumbRight.disabled;
					}
				}

				// Token: 0x04003FFE RID: 16382
				private static readonly int part = 8;

				// Token: 0x04003FFF RID: 16383
				private static VisualStyleElement normal;

				// Token: 0x04004000 RID: 16384
				private static VisualStyleElement hot;

				// Token: 0x04004001 RID: 16385
				private static VisualStyleElement pressed;

				// Token: 0x04004002 RID: 16386
				private static VisualStyleElement focused;

				// Token: 0x04004003 RID: 16387
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000861 RID: 2145
			public static class Ticks
			{
				// Token: 0x170017D1 RID: 6097
				// (get) Token: 0x06006C0E RID: 27662 RVA: 0x0018DB89 File Offset: 0x0018CB89
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.Ticks.normal == null)
						{
							VisualStyleElement.TrackBar.Ticks.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.Ticks.part, 1);
						}
						return VisualStyleElement.TrackBar.Ticks.normal;
					}
				}

				// Token: 0x04004004 RID: 16388
				private static readonly int part = 9;

				// Token: 0x04004005 RID: 16389
				private static VisualStyleElement normal;
			}

			// Token: 0x02000862 RID: 2146
			public static class TicksVertical
			{
				// Token: 0x170017D2 RID: 6098
				// (get) Token: 0x06006C10 RID: 27664 RVA: 0x0018DBB5 File Offset: 0x0018CBB5
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrackBar.TicksVertical.normal == null)
						{
							VisualStyleElement.TrackBar.TicksVertical.normal = new VisualStyleElement(VisualStyleElement.TrackBar.className, VisualStyleElement.TrackBar.TicksVertical.part, 1);
						}
						return VisualStyleElement.TrackBar.TicksVertical.normal;
					}
				}

				// Token: 0x04004006 RID: 16390
				private static readonly int part = 10;

				// Token: 0x04004007 RID: 16391
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x02000863 RID: 2147
		public static class TreeView
		{
			// Token: 0x04004008 RID: 16392
			private static readonly string className = "TREEVIEW";

			// Token: 0x02000864 RID: 2148
			public static class Item
			{
				// Token: 0x170017D3 RID: 6099
				// (get) Token: 0x06006C13 RID: 27667 RVA: 0x0018DBED File Offset: 0x0018CBED
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TreeView.Item.normal == null)
						{
							VisualStyleElement.TreeView.Item.normal = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Item.part, 1);
						}
						return VisualStyleElement.TreeView.Item.normal;
					}
				}

				// Token: 0x170017D4 RID: 6100
				// (get) Token: 0x06006C14 RID: 27668 RVA: 0x0018DC10 File Offset: 0x0018CC10
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TreeView.Item.hot == null)
						{
							VisualStyleElement.TreeView.Item.hot = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Item.part, 2);
						}
						return VisualStyleElement.TreeView.Item.hot;
					}
				}

				// Token: 0x170017D5 RID: 6101
				// (get) Token: 0x06006C15 RID: 27669 RVA: 0x0018DC33 File Offset: 0x0018CC33
				public static VisualStyleElement Selected
				{
					get
					{
						if (VisualStyleElement.TreeView.Item.selected == null)
						{
							VisualStyleElement.TreeView.Item.selected = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Item.part, 3);
						}
						return VisualStyleElement.TreeView.Item.selected;
					}
				}

				// Token: 0x170017D6 RID: 6102
				// (get) Token: 0x06006C16 RID: 27670 RVA: 0x0018DC56 File Offset: 0x0018CC56
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TreeView.Item.disabled == null)
						{
							VisualStyleElement.TreeView.Item.disabled = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Item.part, 4);
						}
						return VisualStyleElement.TreeView.Item.disabled;
					}
				}

				// Token: 0x170017D7 RID: 6103
				// (get) Token: 0x06006C17 RID: 27671 RVA: 0x0018DC79 File Offset: 0x0018CC79
				public static VisualStyleElement SelectedNotFocus
				{
					get
					{
						if (VisualStyleElement.TreeView.Item.selectednotfocus == null)
						{
							VisualStyleElement.TreeView.Item.selectednotfocus = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Item.part, 5);
						}
						return VisualStyleElement.TreeView.Item.selectednotfocus;
					}
				}

				// Token: 0x04004009 RID: 16393
				private static readonly int part = 1;

				// Token: 0x0400400A RID: 16394
				private static VisualStyleElement normal;

				// Token: 0x0400400B RID: 16395
				private static VisualStyleElement hot;

				// Token: 0x0400400C RID: 16396
				private static VisualStyleElement selected;

				// Token: 0x0400400D RID: 16397
				private static VisualStyleElement disabled;

				// Token: 0x0400400E RID: 16398
				private static VisualStyleElement selectednotfocus;
			}

			// Token: 0x02000865 RID: 2149
			public static class Glyph
			{
				// Token: 0x170017D8 RID: 6104
				// (get) Token: 0x06006C19 RID: 27673 RVA: 0x0018DCA4 File Offset: 0x0018CCA4
				public static VisualStyleElement Closed
				{
					get
					{
						if (VisualStyleElement.TreeView.Glyph.closed == null)
						{
							VisualStyleElement.TreeView.Glyph.closed = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Glyph.part, 1);
						}
						return VisualStyleElement.TreeView.Glyph.closed;
					}
				}

				// Token: 0x170017D9 RID: 6105
				// (get) Token: 0x06006C1A RID: 27674 RVA: 0x0018DCC7 File Offset: 0x0018CCC7
				public static VisualStyleElement Opened
				{
					get
					{
						if (VisualStyleElement.TreeView.Glyph.opened == null)
						{
							VisualStyleElement.TreeView.Glyph.opened = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Glyph.part, 2);
						}
						return VisualStyleElement.TreeView.Glyph.opened;
					}
				}

				// Token: 0x0400400F RID: 16399
				private static readonly int part = 2;

				// Token: 0x04004010 RID: 16400
				private static VisualStyleElement closed;

				// Token: 0x04004011 RID: 16401
				private static VisualStyleElement opened;
			}

			// Token: 0x02000866 RID: 2150
			public static class Branch
			{
				// Token: 0x170017DA RID: 6106
				// (get) Token: 0x06006C1C RID: 27676 RVA: 0x0018DCF2 File Offset: 0x0018CCF2
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TreeView.Branch.normal == null)
						{
							VisualStyleElement.TreeView.Branch.normal = new VisualStyleElement(VisualStyleElement.TreeView.className, VisualStyleElement.TreeView.Branch.part, 0);
						}
						return VisualStyleElement.TreeView.Branch.normal;
					}
				}

				// Token: 0x04004012 RID: 16402
				private static readonly int part = 3;

				// Token: 0x04004013 RID: 16403
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x02000867 RID: 2151
		public static class TextBox
		{
			// Token: 0x04004014 RID: 16404
			private static readonly string className = "EDIT";

			// Token: 0x02000868 RID: 2152
			public static class TextEdit
			{
				// Token: 0x170017DB RID: 6107
				// (get) Token: 0x06006C1F RID: 27679 RVA: 0x0018DD29 File Offset: 0x0018CD29
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TextBox.TextEdit.normal == null)
						{
							VisualStyleElement.TextBox.TextEdit.normal = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.TextEdit.part, 1);
						}
						return VisualStyleElement.TextBox.TextEdit.normal;
					}
				}

				// Token: 0x170017DC RID: 6108
				// (get) Token: 0x06006C20 RID: 27680 RVA: 0x0018DD4C File Offset: 0x0018CD4C
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.TextBox.TextEdit.hot == null)
						{
							VisualStyleElement.TextBox.TextEdit.hot = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.TextEdit.part, 2);
						}
						return VisualStyleElement.TextBox.TextEdit.hot;
					}
				}

				// Token: 0x170017DD RID: 6109
				// (get) Token: 0x06006C21 RID: 27681 RVA: 0x0018DD6F File Offset: 0x0018CD6F
				public static VisualStyleElement Selected
				{
					get
					{
						if (VisualStyleElement.TextBox.TextEdit.selected == null)
						{
							VisualStyleElement.TextBox.TextEdit.selected = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.TextEdit.part, 3);
						}
						return VisualStyleElement.TextBox.TextEdit.selected;
					}
				}

				// Token: 0x170017DE RID: 6110
				// (get) Token: 0x06006C22 RID: 27682 RVA: 0x0018DD92 File Offset: 0x0018CD92
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.TextBox.TextEdit.disabled == null)
						{
							VisualStyleElement.TextBox.TextEdit.disabled = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.TextEdit.part, 4);
						}
						return VisualStyleElement.TextBox.TextEdit.disabled;
					}
				}

				// Token: 0x170017DF RID: 6111
				// (get) Token: 0x06006C23 RID: 27683 RVA: 0x0018DDB5 File Offset: 0x0018CDB5
				public static VisualStyleElement Focused
				{
					get
					{
						if (VisualStyleElement.TextBox.TextEdit.focused == null)
						{
							VisualStyleElement.TextBox.TextEdit.focused = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.TextEdit.part, 5);
						}
						return VisualStyleElement.TextBox.TextEdit.focused;
					}
				}

				// Token: 0x170017E0 RID: 6112
				// (get) Token: 0x06006C24 RID: 27684 RVA: 0x0018DDD8 File Offset: 0x0018CDD8
				public static VisualStyleElement ReadOnly
				{
					get
					{
						if (VisualStyleElement.TextBox.TextEdit._readonly == null)
						{
							VisualStyleElement.TextBox.TextEdit._readonly = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.TextEdit.part, 6);
						}
						return VisualStyleElement.TextBox.TextEdit._readonly;
					}
				}

				// Token: 0x170017E1 RID: 6113
				// (get) Token: 0x06006C25 RID: 27685 RVA: 0x0018DDFB File Offset: 0x0018CDFB
				public static VisualStyleElement Assist
				{
					get
					{
						if (VisualStyleElement.TextBox.TextEdit.assist == null)
						{
							VisualStyleElement.TextBox.TextEdit.assist = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.TextEdit.part, 7);
						}
						return VisualStyleElement.TextBox.TextEdit.assist;
					}
				}

				// Token: 0x04004015 RID: 16405
				private static readonly int part = 1;

				// Token: 0x04004016 RID: 16406
				private static VisualStyleElement normal;

				// Token: 0x04004017 RID: 16407
				private static VisualStyleElement hot;

				// Token: 0x04004018 RID: 16408
				private static VisualStyleElement selected;

				// Token: 0x04004019 RID: 16409
				private static VisualStyleElement disabled;

				// Token: 0x0400401A RID: 16410
				private static VisualStyleElement focused;

				// Token: 0x0400401B RID: 16411
				private static VisualStyleElement _readonly;

				// Token: 0x0400401C RID: 16412
				private static VisualStyleElement assist;
			}

			// Token: 0x02000869 RID: 2153
			public static class Caret
			{
				// Token: 0x170017E2 RID: 6114
				// (get) Token: 0x06006C27 RID: 27687 RVA: 0x0018DE26 File Offset: 0x0018CE26
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TextBox.Caret.normal == null)
						{
							VisualStyleElement.TextBox.Caret.normal = new VisualStyleElement(VisualStyleElement.TextBox.className, VisualStyleElement.TextBox.Caret.part, 0);
						}
						return VisualStyleElement.TextBox.Caret.normal;
					}
				}

				// Token: 0x0400401D RID: 16413
				private static readonly int part = 2;

				// Token: 0x0400401E RID: 16414
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x0200086A RID: 2154
		public static class TrayNotify
		{
			// Token: 0x0400401F RID: 16415
			private static readonly string className = "TRAYNOTIFY";

			// Token: 0x0200086B RID: 2155
			public static class Background
			{
				// Token: 0x170017E3 RID: 6115
				// (get) Token: 0x06006C2A RID: 27690 RVA: 0x0018DE5D File Offset: 0x0018CE5D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrayNotify.Background.normal == null)
						{
							VisualStyleElement.TrayNotify.Background.normal = new VisualStyleElement(VisualStyleElement.TrayNotify.className, VisualStyleElement.TrayNotify.Background.part, 0);
						}
						return VisualStyleElement.TrayNotify.Background.normal;
					}
				}

				// Token: 0x04004020 RID: 16416
				private static readonly int part = 1;

				// Token: 0x04004021 RID: 16417
				private static VisualStyleElement normal;
			}

			// Token: 0x0200086C RID: 2156
			public static class AnimateBackground
			{
				// Token: 0x170017E4 RID: 6116
				// (get) Token: 0x06006C2C RID: 27692 RVA: 0x0018DE88 File Offset: 0x0018CE88
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.TrayNotify.AnimateBackground.normal == null)
						{
							VisualStyleElement.TrayNotify.AnimateBackground.normal = new VisualStyleElement(VisualStyleElement.TrayNotify.className, VisualStyleElement.TrayNotify.AnimateBackground.part, 0);
						}
						return VisualStyleElement.TrayNotify.AnimateBackground.normal;
					}
				}

				// Token: 0x04004022 RID: 16418
				private static readonly int part = 2;

				// Token: 0x04004023 RID: 16419
				private static VisualStyleElement normal;
			}
		}

		// Token: 0x0200086D RID: 2157
		public static class Window
		{
			// Token: 0x04004024 RID: 16420
			private static readonly string className = "WINDOW";

			// Token: 0x0200086E RID: 2158
			public static class Caption
			{
				// Token: 0x170017E5 RID: 6117
				// (get) Token: 0x06006C2F RID: 27695 RVA: 0x0018DEBF File Offset: 0x0018CEBF
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.Caption.active == null)
						{
							VisualStyleElement.Window.Caption.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.Caption.part, 1);
						}
						return VisualStyleElement.Window.Caption.active;
					}
				}

				// Token: 0x170017E6 RID: 6118
				// (get) Token: 0x06006C30 RID: 27696 RVA: 0x0018DEE2 File Offset: 0x0018CEE2
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.Caption.inactive == null)
						{
							VisualStyleElement.Window.Caption.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.Caption.part, 2);
						}
						return VisualStyleElement.Window.Caption.inactive;
					}
				}

				// Token: 0x170017E7 RID: 6119
				// (get) Token: 0x06006C31 RID: 27697 RVA: 0x0018DF05 File Offset: 0x0018CF05
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.Caption.disabled == null)
						{
							VisualStyleElement.Window.Caption.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.Caption.part, 3);
						}
						return VisualStyleElement.Window.Caption.disabled;
					}
				}

				// Token: 0x04004025 RID: 16421
				private static readonly int part = 1;

				// Token: 0x04004026 RID: 16422
				private static VisualStyleElement active;

				// Token: 0x04004027 RID: 16423
				private static VisualStyleElement inactive;

				// Token: 0x04004028 RID: 16424
				private static VisualStyleElement disabled;
			}

			// Token: 0x0200086F RID: 2159
			public static class SmallCaption
			{
				// Token: 0x170017E8 RID: 6120
				// (get) Token: 0x06006C33 RID: 27699 RVA: 0x0018DF30 File Offset: 0x0018CF30
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.SmallCaption.active == null)
						{
							VisualStyleElement.Window.SmallCaption.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCaption.part, 1);
						}
						return VisualStyleElement.Window.SmallCaption.active;
					}
				}

				// Token: 0x170017E9 RID: 6121
				// (get) Token: 0x06006C34 RID: 27700 RVA: 0x0018DF53 File Offset: 0x0018CF53
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.SmallCaption.inactive == null)
						{
							VisualStyleElement.Window.SmallCaption.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCaption.part, 2);
						}
						return VisualStyleElement.Window.SmallCaption.inactive;
					}
				}

				// Token: 0x170017EA RID: 6122
				// (get) Token: 0x06006C35 RID: 27701 RVA: 0x0018DF76 File Offset: 0x0018CF76
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.SmallCaption.disabled == null)
						{
							VisualStyleElement.Window.SmallCaption.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCaption.part, 3);
						}
						return VisualStyleElement.Window.SmallCaption.disabled;
					}
				}

				// Token: 0x04004029 RID: 16425
				private static readonly int part = 2;

				// Token: 0x0400402A RID: 16426
				private static VisualStyleElement active;

				// Token: 0x0400402B RID: 16427
				private static VisualStyleElement inactive;

				// Token: 0x0400402C RID: 16428
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000870 RID: 2160
			public static class MinCaption
			{
				// Token: 0x170017EB RID: 6123
				// (get) Token: 0x06006C37 RID: 27703 RVA: 0x0018DFA1 File Offset: 0x0018CFA1
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.MinCaption.active == null)
						{
							VisualStyleElement.Window.MinCaption.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MinCaption.part, 1);
						}
						return VisualStyleElement.Window.MinCaption.active;
					}
				}

				// Token: 0x170017EC RID: 6124
				// (get) Token: 0x06006C38 RID: 27704 RVA: 0x0018DFC4 File Offset: 0x0018CFC4
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.MinCaption.inactive == null)
						{
							VisualStyleElement.Window.MinCaption.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MinCaption.part, 2);
						}
						return VisualStyleElement.Window.MinCaption.inactive;
					}
				}

				// Token: 0x170017ED RID: 6125
				// (get) Token: 0x06006C39 RID: 27705 RVA: 0x0018DFE7 File Offset: 0x0018CFE7
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MinCaption.disabled == null)
						{
							VisualStyleElement.Window.MinCaption.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MinCaption.part, 3);
						}
						return VisualStyleElement.Window.MinCaption.disabled;
					}
				}

				// Token: 0x0400402D RID: 16429
				private static readonly int part = 3;

				// Token: 0x0400402E RID: 16430
				private static VisualStyleElement active;

				// Token: 0x0400402F RID: 16431
				private static VisualStyleElement inactive;

				// Token: 0x04004030 RID: 16432
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000871 RID: 2161
			public static class SmallMinCaption
			{
				// Token: 0x170017EE RID: 6126
				// (get) Token: 0x06006C3B RID: 27707 RVA: 0x0018E012 File Offset: 0x0018D012
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.SmallMinCaption.active == null)
						{
							VisualStyleElement.Window.SmallMinCaption.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallMinCaption.part, 1);
						}
						return VisualStyleElement.Window.SmallMinCaption.active;
					}
				}

				// Token: 0x170017EF RID: 6127
				// (get) Token: 0x06006C3C RID: 27708 RVA: 0x0018E035 File Offset: 0x0018D035
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.SmallMinCaption.inactive == null)
						{
							VisualStyleElement.Window.SmallMinCaption.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallMinCaption.part, 2);
						}
						return VisualStyleElement.Window.SmallMinCaption.inactive;
					}
				}

				// Token: 0x170017F0 RID: 6128
				// (get) Token: 0x06006C3D RID: 27709 RVA: 0x0018E058 File Offset: 0x0018D058
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.SmallMinCaption.disabled == null)
						{
							VisualStyleElement.Window.SmallMinCaption.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallMinCaption.part, 3);
						}
						return VisualStyleElement.Window.SmallMinCaption.disabled;
					}
				}

				// Token: 0x04004031 RID: 16433
				private static readonly int part = 4;

				// Token: 0x04004032 RID: 16434
				private static VisualStyleElement active;

				// Token: 0x04004033 RID: 16435
				private static VisualStyleElement inactive;

				// Token: 0x04004034 RID: 16436
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000872 RID: 2162
			public static class MaxCaption
			{
				// Token: 0x170017F1 RID: 6129
				// (get) Token: 0x06006C3F RID: 27711 RVA: 0x0018E083 File Offset: 0x0018D083
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.MaxCaption.active == null)
						{
							VisualStyleElement.Window.MaxCaption.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MaxCaption.part, 1);
						}
						return VisualStyleElement.Window.MaxCaption.active;
					}
				}

				// Token: 0x170017F2 RID: 6130
				// (get) Token: 0x06006C40 RID: 27712 RVA: 0x0018E0A6 File Offset: 0x0018D0A6
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.MaxCaption.inactive == null)
						{
							VisualStyleElement.Window.MaxCaption.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MaxCaption.part, 2);
						}
						return VisualStyleElement.Window.MaxCaption.inactive;
					}
				}

				// Token: 0x170017F3 RID: 6131
				// (get) Token: 0x06006C41 RID: 27713 RVA: 0x0018E0C9 File Offset: 0x0018D0C9
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MaxCaption.disabled == null)
						{
							VisualStyleElement.Window.MaxCaption.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MaxCaption.part, 3);
						}
						return VisualStyleElement.Window.MaxCaption.disabled;
					}
				}

				// Token: 0x04004035 RID: 16437
				private static readonly int part = 5;

				// Token: 0x04004036 RID: 16438
				private static VisualStyleElement active;

				// Token: 0x04004037 RID: 16439
				private static VisualStyleElement inactive;

				// Token: 0x04004038 RID: 16440
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000873 RID: 2163
			public static class SmallMaxCaption
			{
				// Token: 0x170017F4 RID: 6132
				// (get) Token: 0x06006C43 RID: 27715 RVA: 0x0018E0F4 File Offset: 0x0018D0F4
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.SmallMaxCaption.active == null)
						{
							VisualStyleElement.Window.SmallMaxCaption.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallMaxCaption.part, 1);
						}
						return VisualStyleElement.Window.SmallMaxCaption.active;
					}
				}

				// Token: 0x170017F5 RID: 6133
				// (get) Token: 0x06006C44 RID: 27716 RVA: 0x0018E117 File Offset: 0x0018D117
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.SmallMaxCaption.inactive == null)
						{
							VisualStyleElement.Window.SmallMaxCaption.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallMaxCaption.part, 2);
						}
						return VisualStyleElement.Window.SmallMaxCaption.inactive;
					}
				}

				// Token: 0x170017F6 RID: 6134
				// (get) Token: 0x06006C45 RID: 27717 RVA: 0x0018E13A File Offset: 0x0018D13A
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.SmallMaxCaption.disabled == null)
						{
							VisualStyleElement.Window.SmallMaxCaption.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallMaxCaption.part, 3);
						}
						return VisualStyleElement.Window.SmallMaxCaption.disabled;
					}
				}

				// Token: 0x04004039 RID: 16441
				private static readonly int part = 6;

				// Token: 0x0400403A RID: 16442
				private static VisualStyleElement active;

				// Token: 0x0400403B RID: 16443
				private static VisualStyleElement inactive;

				// Token: 0x0400403C RID: 16444
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000874 RID: 2164
			public static class FrameLeft
			{
				// Token: 0x170017F7 RID: 6135
				// (get) Token: 0x06006C47 RID: 27719 RVA: 0x0018E165 File Offset: 0x0018D165
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.FrameLeft.active == null)
						{
							VisualStyleElement.Window.FrameLeft.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameLeft.part, 1);
						}
						return VisualStyleElement.Window.FrameLeft.active;
					}
				}

				// Token: 0x170017F8 RID: 6136
				// (get) Token: 0x06006C48 RID: 27720 RVA: 0x0018E188 File Offset: 0x0018D188
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.FrameLeft.inactive == null)
						{
							VisualStyleElement.Window.FrameLeft.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameLeft.part, 2);
						}
						return VisualStyleElement.Window.FrameLeft.inactive;
					}
				}

				// Token: 0x0400403D RID: 16445
				private static readonly int part = 7;

				// Token: 0x0400403E RID: 16446
				private static VisualStyleElement active;

				// Token: 0x0400403F RID: 16447
				private static VisualStyleElement inactive;
			}

			// Token: 0x02000875 RID: 2165
			public static class FrameRight
			{
				// Token: 0x170017F9 RID: 6137
				// (get) Token: 0x06006C4A RID: 27722 RVA: 0x0018E1B3 File Offset: 0x0018D1B3
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.FrameRight.active == null)
						{
							VisualStyleElement.Window.FrameRight.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameRight.part, 1);
						}
						return VisualStyleElement.Window.FrameRight.active;
					}
				}

				// Token: 0x170017FA RID: 6138
				// (get) Token: 0x06006C4B RID: 27723 RVA: 0x0018E1D6 File Offset: 0x0018D1D6
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.FrameRight.inactive == null)
						{
							VisualStyleElement.Window.FrameRight.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameRight.part, 2);
						}
						return VisualStyleElement.Window.FrameRight.inactive;
					}
				}

				// Token: 0x04004040 RID: 16448
				private static readonly int part = 8;

				// Token: 0x04004041 RID: 16449
				private static VisualStyleElement active;

				// Token: 0x04004042 RID: 16450
				private static VisualStyleElement inactive;
			}

			// Token: 0x02000876 RID: 2166
			public static class FrameBottom
			{
				// Token: 0x170017FB RID: 6139
				// (get) Token: 0x06006C4D RID: 27725 RVA: 0x0018E201 File Offset: 0x0018D201
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.FrameBottom.active == null)
						{
							VisualStyleElement.Window.FrameBottom.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameBottom.part, 1);
						}
						return VisualStyleElement.Window.FrameBottom.active;
					}
				}

				// Token: 0x170017FC RID: 6140
				// (get) Token: 0x06006C4E RID: 27726 RVA: 0x0018E224 File Offset: 0x0018D224
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.FrameBottom.inactive == null)
						{
							VisualStyleElement.Window.FrameBottom.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameBottom.part, 2);
						}
						return VisualStyleElement.Window.FrameBottom.inactive;
					}
				}

				// Token: 0x04004043 RID: 16451
				private static readonly int part = 9;

				// Token: 0x04004044 RID: 16452
				private static VisualStyleElement active;

				// Token: 0x04004045 RID: 16453
				private static VisualStyleElement inactive;
			}

			// Token: 0x02000877 RID: 2167
			public static class SmallFrameLeft
			{
				// Token: 0x170017FD RID: 6141
				// (get) Token: 0x06006C50 RID: 27728 RVA: 0x0018E250 File Offset: 0x0018D250
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameLeft.active == null)
						{
							VisualStyleElement.Window.SmallFrameLeft.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameLeft.part, 1);
						}
						return VisualStyleElement.Window.SmallFrameLeft.active;
					}
				}

				// Token: 0x170017FE RID: 6142
				// (get) Token: 0x06006C51 RID: 27729 RVA: 0x0018E273 File Offset: 0x0018D273
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameLeft.inactive == null)
						{
							VisualStyleElement.Window.SmallFrameLeft.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameLeft.part, 2);
						}
						return VisualStyleElement.Window.SmallFrameLeft.inactive;
					}
				}

				// Token: 0x04004046 RID: 16454
				private static readonly int part = 10;

				// Token: 0x04004047 RID: 16455
				private static VisualStyleElement active;

				// Token: 0x04004048 RID: 16456
				private static VisualStyleElement inactive;
			}

			// Token: 0x02000878 RID: 2168
			public static class SmallFrameRight
			{
				// Token: 0x170017FF RID: 6143
				// (get) Token: 0x06006C53 RID: 27731 RVA: 0x0018E29F File Offset: 0x0018D29F
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameRight.active == null)
						{
							VisualStyleElement.Window.SmallFrameRight.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameRight.part, 1);
						}
						return VisualStyleElement.Window.SmallFrameRight.active;
					}
				}

				// Token: 0x17001800 RID: 6144
				// (get) Token: 0x06006C54 RID: 27732 RVA: 0x0018E2C2 File Offset: 0x0018D2C2
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameRight.inactive == null)
						{
							VisualStyleElement.Window.SmallFrameRight.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameRight.part, 2);
						}
						return VisualStyleElement.Window.SmallFrameRight.inactive;
					}
				}

				// Token: 0x04004049 RID: 16457
				private static readonly int part = 11;

				// Token: 0x0400404A RID: 16458
				private static VisualStyleElement active;

				// Token: 0x0400404B RID: 16459
				private static VisualStyleElement inactive;
			}

			// Token: 0x02000879 RID: 2169
			public static class SmallFrameBottom
			{
				// Token: 0x17001801 RID: 6145
				// (get) Token: 0x06006C56 RID: 27734 RVA: 0x0018E2EE File Offset: 0x0018D2EE
				public static VisualStyleElement Active
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameBottom.active == null)
						{
							VisualStyleElement.Window.SmallFrameBottom.active = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameBottom.part, 1);
						}
						return VisualStyleElement.Window.SmallFrameBottom.active;
					}
				}

				// Token: 0x17001802 RID: 6146
				// (get) Token: 0x06006C57 RID: 27735 RVA: 0x0018E311 File Offset: 0x0018D311
				public static VisualStyleElement Inactive
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameBottom.inactive == null)
						{
							VisualStyleElement.Window.SmallFrameBottom.inactive = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameBottom.part, 2);
						}
						return VisualStyleElement.Window.SmallFrameBottom.inactive;
					}
				}

				// Token: 0x0400404C RID: 16460
				private static readonly int part = 12;

				// Token: 0x0400404D RID: 16461
				private static VisualStyleElement active;

				// Token: 0x0400404E RID: 16462
				private static VisualStyleElement inactive;
			}

			// Token: 0x0200087A RID: 2170
			public static class SysButton
			{
				// Token: 0x17001803 RID: 6147
				// (get) Token: 0x06006C59 RID: 27737 RVA: 0x0018E33D File Offset: 0x0018D33D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.SysButton.normal == null)
						{
							VisualStyleElement.Window.SysButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SysButton.part, 1);
						}
						return VisualStyleElement.Window.SysButton.normal;
					}
				}

				// Token: 0x17001804 RID: 6148
				// (get) Token: 0x06006C5A RID: 27738 RVA: 0x0018E360 File Offset: 0x0018D360
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.SysButton.hot == null)
						{
							VisualStyleElement.Window.SysButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SysButton.part, 2);
						}
						return VisualStyleElement.Window.SysButton.hot;
					}
				}

				// Token: 0x17001805 RID: 6149
				// (get) Token: 0x06006C5B RID: 27739 RVA: 0x0018E383 File Offset: 0x0018D383
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.SysButton.pressed == null)
						{
							VisualStyleElement.Window.SysButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SysButton.part, 3);
						}
						return VisualStyleElement.Window.SysButton.pressed;
					}
				}

				// Token: 0x17001806 RID: 6150
				// (get) Token: 0x06006C5C RID: 27740 RVA: 0x0018E3A6 File Offset: 0x0018D3A6
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.SysButton.disabled == null)
						{
							VisualStyleElement.Window.SysButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SysButton.part, 4);
						}
						return VisualStyleElement.Window.SysButton.disabled;
					}
				}

				// Token: 0x0400404F RID: 16463
				private static readonly int part = 13;

				// Token: 0x04004050 RID: 16464
				private static VisualStyleElement normal;

				// Token: 0x04004051 RID: 16465
				private static VisualStyleElement hot;

				// Token: 0x04004052 RID: 16466
				private static VisualStyleElement pressed;

				// Token: 0x04004053 RID: 16467
				private static VisualStyleElement disabled;
			}

			// Token: 0x0200087B RID: 2171
			public static class MdiSysButton
			{
				// Token: 0x17001807 RID: 6151
				// (get) Token: 0x06006C5E RID: 27742 RVA: 0x0018E3D2 File Offset: 0x0018D3D2
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.MdiSysButton.normal == null)
						{
							VisualStyleElement.Window.MdiSysButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiSysButton.part, 1);
						}
						return VisualStyleElement.Window.MdiSysButton.normal;
					}
				}

				// Token: 0x17001808 RID: 6152
				// (get) Token: 0x06006C5F RID: 27743 RVA: 0x0018E3F5 File Offset: 0x0018D3F5
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.MdiSysButton.hot == null)
						{
							VisualStyleElement.Window.MdiSysButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiSysButton.part, 2);
						}
						return VisualStyleElement.Window.MdiSysButton.hot;
					}
				}

				// Token: 0x17001809 RID: 6153
				// (get) Token: 0x06006C60 RID: 27744 RVA: 0x0018E418 File Offset: 0x0018D418
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.MdiSysButton.pressed == null)
						{
							VisualStyleElement.Window.MdiSysButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiSysButton.part, 3);
						}
						return VisualStyleElement.Window.MdiSysButton.pressed;
					}
				}

				// Token: 0x1700180A RID: 6154
				// (get) Token: 0x06006C61 RID: 27745 RVA: 0x0018E43B File Offset: 0x0018D43B
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MdiSysButton.disabled == null)
						{
							VisualStyleElement.Window.MdiSysButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiSysButton.part, 4);
						}
						return VisualStyleElement.Window.MdiSysButton.disabled;
					}
				}

				// Token: 0x04004054 RID: 16468
				private static readonly int part = 14;

				// Token: 0x04004055 RID: 16469
				private static VisualStyleElement normal;

				// Token: 0x04004056 RID: 16470
				private static VisualStyleElement hot;

				// Token: 0x04004057 RID: 16471
				private static VisualStyleElement pressed;

				// Token: 0x04004058 RID: 16472
				private static VisualStyleElement disabled;
			}

			// Token: 0x0200087C RID: 2172
			public static class MinButton
			{
				// Token: 0x1700180B RID: 6155
				// (get) Token: 0x06006C63 RID: 27747 RVA: 0x0018E467 File Offset: 0x0018D467
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.MinButton.normal == null)
						{
							VisualStyleElement.Window.MinButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MinButton.part, 1);
						}
						return VisualStyleElement.Window.MinButton.normal;
					}
				}

				// Token: 0x1700180C RID: 6156
				// (get) Token: 0x06006C64 RID: 27748 RVA: 0x0018E48A File Offset: 0x0018D48A
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.MinButton.hot == null)
						{
							VisualStyleElement.Window.MinButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MinButton.part, 2);
						}
						return VisualStyleElement.Window.MinButton.hot;
					}
				}

				// Token: 0x1700180D RID: 6157
				// (get) Token: 0x06006C65 RID: 27749 RVA: 0x0018E4AD File Offset: 0x0018D4AD
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.MinButton.pressed == null)
						{
							VisualStyleElement.Window.MinButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MinButton.part, 3);
						}
						return VisualStyleElement.Window.MinButton.pressed;
					}
				}

				// Token: 0x1700180E RID: 6158
				// (get) Token: 0x06006C66 RID: 27750 RVA: 0x0018E4D0 File Offset: 0x0018D4D0
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MinButton.disabled == null)
						{
							VisualStyleElement.Window.MinButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MinButton.part, 4);
						}
						return VisualStyleElement.Window.MinButton.disabled;
					}
				}

				// Token: 0x04004059 RID: 16473
				private static readonly int part = 15;

				// Token: 0x0400405A RID: 16474
				private static VisualStyleElement normal;

				// Token: 0x0400405B RID: 16475
				private static VisualStyleElement hot;

				// Token: 0x0400405C RID: 16476
				private static VisualStyleElement pressed;

				// Token: 0x0400405D RID: 16477
				private static VisualStyleElement disabled;
			}

			// Token: 0x0200087D RID: 2173
			public static class MdiMinButton
			{
				// Token: 0x1700180F RID: 6159
				// (get) Token: 0x06006C68 RID: 27752 RVA: 0x0018E4FC File Offset: 0x0018D4FC
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.MdiMinButton.normal == null)
						{
							VisualStyleElement.Window.MdiMinButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiMinButton.part, 1);
						}
						return VisualStyleElement.Window.MdiMinButton.normal;
					}
				}

				// Token: 0x17001810 RID: 6160
				// (get) Token: 0x06006C69 RID: 27753 RVA: 0x0018E51F File Offset: 0x0018D51F
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.MdiMinButton.hot == null)
						{
							VisualStyleElement.Window.MdiMinButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiMinButton.part, 2);
						}
						return VisualStyleElement.Window.MdiMinButton.hot;
					}
				}

				// Token: 0x17001811 RID: 6161
				// (get) Token: 0x06006C6A RID: 27754 RVA: 0x0018E542 File Offset: 0x0018D542
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.MdiMinButton.pressed == null)
						{
							VisualStyleElement.Window.MdiMinButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiMinButton.part, 3);
						}
						return VisualStyleElement.Window.MdiMinButton.pressed;
					}
				}

				// Token: 0x17001812 RID: 6162
				// (get) Token: 0x06006C6B RID: 27755 RVA: 0x0018E565 File Offset: 0x0018D565
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MdiMinButton.disabled == null)
						{
							VisualStyleElement.Window.MdiMinButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiMinButton.part, 4);
						}
						return VisualStyleElement.Window.MdiMinButton.disabled;
					}
				}

				// Token: 0x0400405E RID: 16478
				private static readonly int part = 16;

				// Token: 0x0400405F RID: 16479
				private static VisualStyleElement normal;

				// Token: 0x04004060 RID: 16480
				private static VisualStyleElement hot;

				// Token: 0x04004061 RID: 16481
				private static VisualStyleElement pressed;

				// Token: 0x04004062 RID: 16482
				private static VisualStyleElement disabled;
			}

			// Token: 0x0200087E RID: 2174
			public static class MaxButton
			{
				// Token: 0x17001813 RID: 6163
				// (get) Token: 0x06006C6D RID: 27757 RVA: 0x0018E591 File Offset: 0x0018D591
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.MaxButton.normal == null)
						{
							VisualStyleElement.Window.MaxButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MaxButton.part, 1);
						}
						return VisualStyleElement.Window.MaxButton.normal;
					}
				}

				// Token: 0x17001814 RID: 6164
				// (get) Token: 0x06006C6E RID: 27758 RVA: 0x0018E5B4 File Offset: 0x0018D5B4
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.MaxButton.hot == null)
						{
							VisualStyleElement.Window.MaxButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MaxButton.part, 2);
						}
						return VisualStyleElement.Window.MaxButton.hot;
					}
				}

				// Token: 0x17001815 RID: 6165
				// (get) Token: 0x06006C6F RID: 27759 RVA: 0x0018E5D7 File Offset: 0x0018D5D7
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.MaxButton.pressed == null)
						{
							VisualStyleElement.Window.MaxButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MaxButton.part, 3);
						}
						return VisualStyleElement.Window.MaxButton.pressed;
					}
				}

				// Token: 0x17001816 RID: 6166
				// (get) Token: 0x06006C70 RID: 27760 RVA: 0x0018E5FA File Offset: 0x0018D5FA
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MaxButton.disabled == null)
						{
							VisualStyleElement.Window.MaxButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MaxButton.part, 4);
						}
						return VisualStyleElement.Window.MaxButton.disabled;
					}
				}

				// Token: 0x04004063 RID: 16483
				private static readonly int part = 17;

				// Token: 0x04004064 RID: 16484
				private static VisualStyleElement normal;

				// Token: 0x04004065 RID: 16485
				private static VisualStyleElement hot;

				// Token: 0x04004066 RID: 16486
				private static VisualStyleElement pressed;

				// Token: 0x04004067 RID: 16487
				private static VisualStyleElement disabled;
			}

			// Token: 0x0200087F RID: 2175
			public static class CloseButton
			{
				// Token: 0x17001817 RID: 6167
				// (get) Token: 0x06006C72 RID: 27762 RVA: 0x0018E626 File Offset: 0x0018D626
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.CloseButton.normal == null)
						{
							VisualStyleElement.Window.CloseButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.CloseButton.part, 1);
						}
						return VisualStyleElement.Window.CloseButton.normal;
					}
				}

				// Token: 0x17001818 RID: 6168
				// (get) Token: 0x06006C73 RID: 27763 RVA: 0x0018E649 File Offset: 0x0018D649
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.CloseButton.hot == null)
						{
							VisualStyleElement.Window.CloseButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.CloseButton.part, 2);
						}
						return VisualStyleElement.Window.CloseButton.hot;
					}
				}

				// Token: 0x17001819 RID: 6169
				// (get) Token: 0x06006C74 RID: 27764 RVA: 0x0018E66C File Offset: 0x0018D66C
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.CloseButton.pressed == null)
						{
							VisualStyleElement.Window.CloseButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.CloseButton.part, 3);
						}
						return VisualStyleElement.Window.CloseButton.pressed;
					}
				}

				// Token: 0x1700181A RID: 6170
				// (get) Token: 0x06006C75 RID: 27765 RVA: 0x0018E68F File Offset: 0x0018D68F
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.CloseButton.disabled == null)
						{
							VisualStyleElement.Window.CloseButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.CloseButton.part, 4);
						}
						return VisualStyleElement.Window.CloseButton.disabled;
					}
				}

				// Token: 0x04004068 RID: 16488
				private static readonly int part = 18;

				// Token: 0x04004069 RID: 16489
				private static VisualStyleElement normal;

				// Token: 0x0400406A RID: 16490
				private static VisualStyleElement hot;

				// Token: 0x0400406B RID: 16491
				private static VisualStyleElement pressed;

				// Token: 0x0400406C RID: 16492
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000880 RID: 2176
			public static class SmallCloseButton
			{
				// Token: 0x1700181B RID: 6171
				// (get) Token: 0x06006C77 RID: 27767 RVA: 0x0018E6BB File Offset: 0x0018D6BB
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.SmallCloseButton.normal == null)
						{
							VisualStyleElement.Window.SmallCloseButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCloseButton.part, 1);
						}
						return VisualStyleElement.Window.SmallCloseButton.normal;
					}
				}

				// Token: 0x1700181C RID: 6172
				// (get) Token: 0x06006C78 RID: 27768 RVA: 0x0018E6DE File Offset: 0x0018D6DE
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.SmallCloseButton.hot == null)
						{
							VisualStyleElement.Window.SmallCloseButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCloseButton.part, 2);
						}
						return VisualStyleElement.Window.SmallCloseButton.hot;
					}
				}

				// Token: 0x1700181D RID: 6173
				// (get) Token: 0x06006C79 RID: 27769 RVA: 0x0018E701 File Offset: 0x0018D701
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.SmallCloseButton.pressed == null)
						{
							VisualStyleElement.Window.SmallCloseButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCloseButton.part, 3);
						}
						return VisualStyleElement.Window.SmallCloseButton.pressed;
					}
				}

				// Token: 0x1700181E RID: 6174
				// (get) Token: 0x06006C7A RID: 27770 RVA: 0x0018E724 File Offset: 0x0018D724
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.SmallCloseButton.disabled == null)
						{
							VisualStyleElement.Window.SmallCloseButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCloseButton.part, 4);
						}
						return VisualStyleElement.Window.SmallCloseButton.disabled;
					}
				}

				// Token: 0x0400406D RID: 16493
				private static readonly int part = 19;

				// Token: 0x0400406E RID: 16494
				private static VisualStyleElement normal;

				// Token: 0x0400406F RID: 16495
				private static VisualStyleElement hot;

				// Token: 0x04004070 RID: 16496
				private static VisualStyleElement pressed;

				// Token: 0x04004071 RID: 16497
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000881 RID: 2177
			public static class MdiCloseButton
			{
				// Token: 0x1700181F RID: 6175
				// (get) Token: 0x06006C7C RID: 27772 RVA: 0x0018E750 File Offset: 0x0018D750
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.MdiCloseButton.normal == null)
						{
							VisualStyleElement.Window.MdiCloseButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiCloseButton.part, 1);
						}
						return VisualStyleElement.Window.MdiCloseButton.normal;
					}
				}

				// Token: 0x17001820 RID: 6176
				// (get) Token: 0x06006C7D RID: 27773 RVA: 0x0018E773 File Offset: 0x0018D773
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.MdiCloseButton.hot == null)
						{
							VisualStyleElement.Window.MdiCloseButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiCloseButton.part, 2);
						}
						return VisualStyleElement.Window.MdiCloseButton.hot;
					}
				}

				// Token: 0x17001821 RID: 6177
				// (get) Token: 0x06006C7E RID: 27774 RVA: 0x0018E796 File Offset: 0x0018D796
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.MdiCloseButton.pressed == null)
						{
							VisualStyleElement.Window.MdiCloseButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiCloseButton.part, 3);
						}
						return VisualStyleElement.Window.MdiCloseButton.pressed;
					}
				}

				// Token: 0x17001822 RID: 6178
				// (get) Token: 0x06006C7F RID: 27775 RVA: 0x0018E7B9 File Offset: 0x0018D7B9
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MdiCloseButton.disabled == null)
						{
							VisualStyleElement.Window.MdiCloseButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiCloseButton.part, 4);
						}
						return VisualStyleElement.Window.MdiCloseButton.disabled;
					}
				}

				// Token: 0x04004072 RID: 16498
				private static readonly int part = 20;

				// Token: 0x04004073 RID: 16499
				private static VisualStyleElement normal;

				// Token: 0x04004074 RID: 16500
				private static VisualStyleElement hot;

				// Token: 0x04004075 RID: 16501
				private static VisualStyleElement pressed;

				// Token: 0x04004076 RID: 16502
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000882 RID: 2178
			public static class RestoreButton
			{
				// Token: 0x17001823 RID: 6179
				// (get) Token: 0x06006C81 RID: 27777 RVA: 0x0018E7E5 File Offset: 0x0018D7E5
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.RestoreButton.normal == null)
						{
							VisualStyleElement.Window.RestoreButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.RestoreButton.part, 1);
						}
						return VisualStyleElement.Window.RestoreButton.normal;
					}
				}

				// Token: 0x17001824 RID: 6180
				// (get) Token: 0x06006C82 RID: 27778 RVA: 0x0018E808 File Offset: 0x0018D808
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.RestoreButton.hot == null)
						{
							VisualStyleElement.Window.RestoreButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.RestoreButton.part, 2);
						}
						return VisualStyleElement.Window.RestoreButton.hot;
					}
				}

				// Token: 0x17001825 RID: 6181
				// (get) Token: 0x06006C83 RID: 27779 RVA: 0x0018E82B File Offset: 0x0018D82B
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.RestoreButton.pressed == null)
						{
							VisualStyleElement.Window.RestoreButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.RestoreButton.part, 3);
						}
						return VisualStyleElement.Window.RestoreButton.pressed;
					}
				}

				// Token: 0x17001826 RID: 6182
				// (get) Token: 0x06006C84 RID: 27780 RVA: 0x0018E84E File Offset: 0x0018D84E
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.RestoreButton.disabled == null)
						{
							VisualStyleElement.Window.RestoreButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.RestoreButton.part, 4);
						}
						return VisualStyleElement.Window.RestoreButton.disabled;
					}
				}

				// Token: 0x04004077 RID: 16503
				private static readonly int part = 21;

				// Token: 0x04004078 RID: 16504
				private static VisualStyleElement normal;

				// Token: 0x04004079 RID: 16505
				private static VisualStyleElement hot;

				// Token: 0x0400407A RID: 16506
				private static VisualStyleElement pressed;

				// Token: 0x0400407B RID: 16507
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000883 RID: 2179
			public static class MdiRestoreButton
			{
				// Token: 0x17001827 RID: 6183
				// (get) Token: 0x06006C86 RID: 27782 RVA: 0x0018E87A File Offset: 0x0018D87A
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.MdiRestoreButton.normal == null)
						{
							VisualStyleElement.Window.MdiRestoreButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiRestoreButton.part, 1);
						}
						return VisualStyleElement.Window.MdiRestoreButton.normal;
					}
				}

				// Token: 0x17001828 RID: 6184
				// (get) Token: 0x06006C87 RID: 27783 RVA: 0x0018E89D File Offset: 0x0018D89D
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.MdiRestoreButton.hot == null)
						{
							VisualStyleElement.Window.MdiRestoreButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiRestoreButton.part, 2);
						}
						return VisualStyleElement.Window.MdiRestoreButton.hot;
					}
				}

				// Token: 0x17001829 RID: 6185
				// (get) Token: 0x06006C88 RID: 27784 RVA: 0x0018E8C0 File Offset: 0x0018D8C0
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.MdiRestoreButton.pressed == null)
						{
							VisualStyleElement.Window.MdiRestoreButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiRestoreButton.part, 3);
						}
						return VisualStyleElement.Window.MdiRestoreButton.pressed;
					}
				}

				// Token: 0x1700182A RID: 6186
				// (get) Token: 0x06006C89 RID: 27785 RVA: 0x0018E8E3 File Offset: 0x0018D8E3
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MdiRestoreButton.disabled == null)
						{
							VisualStyleElement.Window.MdiRestoreButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiRestoreButton.part, 4);
						}
						return VisualStyleElement.Window.MdiRestoreButton.disabled;
					}
				}

				// Token: 0x0400407C RID: 16508
				private static readonly int part = 22;

				// Token: 0x0400407D RID: 16509
				private static VisualStyleElement normal;

				// Token: 0x0400407E RID: 16510
				private static VisualStyleElement hot;

				// Token: 0x0400407F RID: 16511
				private static VisualStyleElement pressed;

				// Token: 0x04004080 RID: 16512
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000884 RID: 2180
			public static class HelpButton
			{
				// Token: 0x1700182B RID: 6187
				// (get) Token: 0x06006C8B RID: 27787 RVA: 0x0018E90F File Offset: 0x0018D90F
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.HelpButton.normal == null)
						{
							VisualStyleElement.Window.HelpButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HelpButton.part, 1);
						}
						return VisualStyleElement.Window.HelpButton.normal;
					}
				}

				// Token: 0x1700182C RID: 6188
				// (get) Token: 0x06006C8C RID: 27788 RVA: 0x0018E932 File Offset: 0x0018D932
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.HelpButton.hot == null)
						{
							VisualStyleElement.Window.HelpButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HelpButton.part, 2);
						}
						return VisualStyleElement.Window.HelpButton.hot;
					}
				}

				// Token: 0x1700182D RID: 6189
				// (get) Token: 0x06006C8D RID: 27789 RVA: 0x0018E955 File Offset: 0x0018D955
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.HelpButton.pressed == null)
						{
							VisualStyleElement.Window.HelpButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HelpButton.part, 3);
						}
						return VisualStyleElement.Window.HelpButton.pressed;
					}
				}

				// Token: 0x1700182E RID: 6190
				// (get) Token: 0x06006C8E RID: 27790 RVA: 0x0018E978 File Offset: 0x0018D978
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.HelpButton.disabled == null)
						{
							VisualStyleElement.Window.HelpButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HelpButton.part, 4);
						}
						return VisualStyleElement.Window.HelpButton.disabled;
					}
				}

				// Token: 0x04004081 RID: 16513
				private static readonly int part = 23;

				// Token: 0x04004082 RID: 16514
				private static VisualStyleElement normal;

				// Token: 0x04004083 RID: 16515
				private static VisualStyleElement hot;

				// Token: 0x04004084 RID: 16516
				private static VisualStyleElement pressed;

				// Token: 0x04004085 RID: 16517
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000885 RID: 2181
			public static class MdiHelpButton
			{
				// Token: 0x1700182F RID: 6191
				// (get) Token: 0x06006C90 RID: 27792 RVA: 0x0018E9A4 File Offset: 0x0018D9A4
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.MdiHelpButton.normal == null)
						{
							VisualStyleElement.Window.MdiHelpButton.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiHelpButton.part, 1);
						}
						return VisualStyleElement.Window.MdiHelpButton.normal;
					}
				}

				// Token: 0x17001830 RID: 6192
				// (get) Token: 0x06006C91 RID: 27793 RVA: 0x0018E9C7 File Offset: 0x0018D9C7
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.MdiHelpButton.hot == null)
						{
							VisualStyleElement.Window.MdiHelpButton.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiHelpButton.part, 2);
						}
						return VisualStyleElement.Window.MdiHelpButton.hot;
					}
				}

				// Token: 0x17001831 RID: 6193
				// (get) Token: 0x06006C92 RID: 27794 RVA: 0x0018E9EA File Offset: 0x0018D9EA
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.MdiHelpButton.pressed == null)
						{
							VisualStyleElement.Window.MdiHelpButton.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiHelpButton.part, 3);
						}
						return VisualStyleElement.Window.MdiHelpButton.pressed;
					}
				}

				// Token: 0x17001832 RID: 6194
				// (get) Token: 0x06006C93 RID: 27795 RVA: 0x0018EA0D File Offset: 0x0018DA0D
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.MdiHelpButton.disabled == null)
						{
							VisualStyleElement.Window.MdiHelpButton.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.MdiHelpButton.part, 4);
						}
						return VisualStyleElement.Window.MdiHelpButton.disabled;
					}
				}

				// Token: 0x04004086 RID: 16518
				private static readonly int part = 24;

				// Token: 0x04004087 RID: 16519
				private static VisualStyleElement normal;

				// Token: 0x04004088 RID: 16520
				private static VisualStyleElement hot;

				// Token: 0x04004089 RID: 16521
				private static VisualStyleElement pressed;

				// Token: 0x0400408A RID: 16522
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000886 RID: 2182
			public static class HorizontalScroll
			{
				// Token: 0x17001833 RID: 6195
				// (get) Token: 0x06006C95 RID: 27797 RVA: 0x0018EA39 File Offset: 0x0018DA39
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalScroll.normal == null)
						{
							VisualStyleElement.Window.HorizontalScroll.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalScroll.part, 1);
						}
						return VisualStyleElement.Window.HorizontalScroll.normal;
					}
				}

				// Token: 0x17001834 RID: 6196
				// (get) Token: 0x06006C96 RID: 27798 RVA: 0x0018EA5C File Offset: 0x0018DA5C
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalScroll.hot == null)
						{
							VisualStyleElement.Window.HorizontalScroll.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalScroll.part, 2);
						}
						return VisualStyleElement.Window.HorizontalScroll.hot;
					}
				}

				// Token: 0x17001835 RID: 6197
				// (get) Token: 0x06006C97 RID: 27799 RVA: 0x0018EA7F File Offset: 0x0018DA7F
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalScroll.pressed == null)
						{
							VisualStyleElement.Window.HorizontalScroll.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalScroll.part, 3);
						}
						return VisualStyleElement.Window.HorizontalScroll.pressed;
					}
				}

				// Token: 0x17001836 RID: 6198
				// (get) Token: 0x06006C98 RID: 27800 RVA: 0x0018EAA2 File Offset: 0x0018DAA2
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalScroll.disabled == null)
						{
							VisualStyleElement.Window.HorizontalScroll.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalScroll.part, 4);
						}
						return VisualStyleElement.Window.HorizontalScroll.disabled;
					}
				}

				// Token: 0x0400408B RID: 16523
				private static readonly int part = 25;

				// Token: 0x0400408C RID: 16524
				private static VisualStyleElement normal;

				// Token: 0x0400408D RID: 16525
				private static VisualStyleElement hot;

				// Token: 0x0400408E RID: 16526
				private static VisualStyleElement pressed;

				// Token: 0x0400408F RID: 16527
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000887 RID: 2183
			public static class HorizontalThumb
			{
				// Token: 0x17001837 RID: 6199
				// (get) Token: 0x06006C9A RID: 27802 RVA: 0x0018EACE File Offset: 0x0018DACE
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalThumb.normal == null)
						{
							VisualStyleElement.Window.HorizontalThumb.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalThumb.part, 1);
						}
						return VisualStyleElement.Window.HorizontalThumb.normal;
					}
				}

				// Token: 0x17001838 RID: 6200
				// (get) Token: 0x06006C9B RID: 27803 RVA: 0x0018EAF1 File Offset: 0x0018DAF1
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalThumb.hot == null)
						{
							VisualStyleElement.Window.HorizontalThumb.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalThumb.part, 2);
						}
						return VisualStyleElement.Window.HorizontalThumb.hot;
					}
				}

				// Token: 0x17001839 RID: 6201
				// (get) Token: 0x06006C9C RID: 27804 RVA: 0x0018EB14 File Offset: 0x0018DB14
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalThumb.pressed == null)
						{
							VisualStyleElement.Window.HorizontalThumb.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalThumb.part, 3);
						}
						return VisualStyleElement.Window.HorizontalThumb.pressed;
					}
				}

				// Token: 0x1700183A RID: 6202
				// (get) Token: 0x06006C9D RID: 27805 RVA: 0x0018EB37 File Offset: 0x0018DB37
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.HorizontalThumb.disabled == null)
						{
							VisualStyleElement.Window.HorizontalThumb.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.HorizontalThumb.part, 4);
						}
						return VisualStyleElement.Window.HorizontalThumb.disabled;
					}
				}

				// Token: 0x04004090 RID: 16528
				private static readonly int part = 26;

				// Token: 0x04004091 RID: 16529
				private static VisualStyleElement normal;

				// Token: 0x04004092 RID: 16530
				private static VisualStyleElement hot;

				// Token: 0x04004093 RID: 16531
				private static VisualStyleElement pressed;

				// Token: 0x04004094 RID: 16532
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000888 RID: 2184
			public static class VerticalScroll
			{
				// Token: 0x1700183B RID: 6203
				// (get) Token: 0x06006C9F RID: 27807 RVA: 0x0018EB63 File Offset: 0x0018DB63
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.VerticalScroll.normal == null)
						{
							VisualStyleElement.Window.VerticalScroll.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalScroll.part, 1);
						}
						return VisualStyleElement.Window.VerticalScroll.normal;
					}
				}

				// Token: 0x1700183C RID: 6204
				// (get) Token: 0x06006CA0 RID: 27808 RVA: 0x0018EB86 File Offset: 0x0018DB86
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.VerticalScroll.hot == null)
						{
							VisualStyleElement.Window.VerticalScroll.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalScroll.part, 2);
						}
						return VisualStyleElement.Window.VerticalScroll.hot;
					}
				}

				// Token: 0x1700183D RID: 6205
				// (get) Token: 0x06006CA1 RID: 27809 RVA: 0x0018EBA9 File Offset: 0x0018DBA9
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.VerticalScroll.pressed == null)
						{
							VisualStyleElement.Window.VerticalScroll.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalScroll.part, 3);
						}
						return VisualStyleElement.Window.VerticalScroll.pressed;
					}
				}

				// Token: 0x1700183E RID: 6206
				// (get) Token: 0x06006CA2 RID: 27810 RVA: 0x0018EBCC File Offset: 0x0018DBCC
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.VerticalScroll.disabled == null)
						{
							VisualStyleElement.Window.VerticalScroll.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalScroll.part, 4);
						}
						return VisualStyleElement.Window.VerticalScroll.disabled;
					}
				}

				// Token: 0x04004095 RID: 16533
				private static readonly int part = 27;

				// Token: 0x04004096 RID: 16534
				private static VisualStyleElement normal;

				// Token: 0x04004097 RID: 16535
				private static VisualStyleElement hot;

				// Token: 0x04004098 RID: 16536
				private static VisualStyleElement pressed;

				// Token: 0x04004099 RID: 16537
				private static VisualStyleElement disabled;
			}

			// Token: 0x02000889 RID: 2185
			public static class VerticalThumb
			{
				// Token: 0x1700183F RID: 6207
				// (get) Token: 0x06006CA4 RID: 27812 RVA: 0x0018EBF8 File Offset: 0x0018DBF8
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.VerticalThumb.normal == null)
						{
							VisualStyleElement.Window.VerticalThumb.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalThumb.part, 1);
						}
						return VisualStyleElement.Window.VerticalThumb.normal;
					}
				}

				// Token: 0x17001840 RID: 6208
				// (get) Token: 0x06006CA5 RID: 27813 RVA: 0x0018EC1B File Offset: 0x0018DC1B
				public static VisualStyleElement Hot
				{
					get
					{
						if (VisualStyleElement.Window.VerticalThumb.hot == null)
						{
							VisualStyleElement.Window.VerticalThumb.hot = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalThumb.part, 2);
						}
						return VisualStyleElement.Window.VerticalThumb.hot;
					}
				}

				// Token: 0x17001841 RID: 6209
				// (get) Token: 0x06006CA6 RID: 27814 RVA: 0x0018EC3E File Offset: 0x0018DC3E
				public static VisualStyleElement Pressed
				{
					get
					{
						if (VisualStyleElement.Window.VerticalThumb.pressed == null)
						{
							VisualStyleElement.Window.VerticalThumb.pressed = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalThumb.part, 3);
						}
						return VisualStyleElement.Window.VerticalThumb.pressed;
					}
				}

				// Token: 0x17001842 RID: 6210
				// (get) Token: 0x06006CA7 RID: 27815 RVA: 0x0018EC61 File Offset: 0x0018DC61
				public static VisualStyleElement Disabled
				{
					get
					{
						if (VisualStyleElement.Window.VerticalThumb.disabled == null)
						{
							VisualStyleElement.Window.VerticalThumb.disabled = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.VerticalThumb.part, 4);
						}
						return VisualStyleElement.Window.VerticalThumb.disabled;
					}
				}

				// Token: 0x0400409A RID: 16538
				private static readonly int part = 28;

				// Token: 0x0400409B RID: 16539
				private static VisualStyleElement normal;

				// Token: 0x0400409C RID: 16540
				private static VisualStyleElement hot;

				// Token: 0x0400409D RID: 16541
				private static VisualStyleElement pressed;

				// Token: 0x0400409E RID: 16542
				private static VisualStyleElement disabled;
			}

			// Token: 0x0200088A RID: 2186
			public static class Dialog
			{
				// Token: 0x17001843 RID: 6211
				// (get) Token: 0x06006CA9 RID: 27817 RVA: 0x0018EC8D File Offset: 0x0018DC8D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.Dialog.normal == null)
						{
							VisualStyleElement.Window.Dialog.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.Dialog.part, 0);
						}
						return VisualStyleElement.Window.Dialog.normal;
					}
				}

				// Token: 0x0400409F RID: 16543
				private static readonly int part = 29;

				// Token: 0x040040A0 RID: 16544
				private static VisualStyleElement normal;
			}

			// Token: 0x0200088B RID: 2187
			public static class CaptionSizingTemplate
			{
				// Token: 0x17001844 RID: 6212
				// (get) Token: 0x06006CAB RID: 27819 RVA: 0x0018ECB9 File Offset: 0x0018DCB9
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.CaptionSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.CaptionSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.CaptionSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.CaptionSizingTemplate.normal;
					}
				}

				// Token: 0x040040A1 RID: 16545
				private static readonly int part = 30;

				// Token: 0x040040A2 RID: 16546
				private static VisualStyleElement normal;
			}

			// Token: 0x0200088C RID: 2188
			public static class SmallCaptionSizingTemplate
			{
				// Token: 0x17001845 RID: 6213
				// (get) Token: 0x06006CAD RID: 27821 RVA: 0x0018ECE5 File Offset: 0x0018DCE5
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.SmallCaptionSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.SmallCaptionSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallCaptionSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.SmallCaptionSizingTemplate.normal;
					}
				}

				// Token: 0x040040A3 RID: 16547
				private static readonly int part = 31;

				// Token: 0x040040A4 RID: 16548
				private static VisualStyleElement normal;
			}

			// Token: 0x0200088D RID: 2189
			public static class FrameLeftSizingTemplate
			{
				// Token: 0x17001846 RID: 6214
				// (get) Token: 0x06006CAF RID: 27823 RVA: 0x0018ED11 File Offset: 0x0018DD11
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.FrameLeftSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.FrameLeftSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameLeftSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.FrameLeftSizingTemplate.normal;
					}
				}

				// Token: 0x040040A5 RID: 16549
				private static readonly int part = 32;

				// Token: 0x040040A6 RID: 16550
				private static VisualStyleElement normal;
			}

			// Token: 0x0200088E RID: 2190
			public static class SmallFrameLeftSizingTemplate
			{
				// Token: 0x17001847 RID: 6215
				// (get) Token: 0x06006CB1 RID: 27825 RVA: 0x0018ED3D File Offset: 0x0018DD3D
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameLeftSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.SmallFrameLeftSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameLeftSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.SmallFrameLeftSizingTemplate.normal;
					}
				}

				// Token: 0x040040A7 RID: 16551
				private static readonly int part = 33;

				// Token: 0x040040A8 RID: 16552
				private static VisualStyleElement normal;
			}

			// Token: 0x0200088F RID: 2191
			public static class FrameRightSizingTemplate
			{
				// Token: 0x17001848 RID: 6216
				// (get) Token: 0x06006CB3 RID: 27827 RVA: 0x0018ED69 File Offset: 0x0018DD69
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.FrameRightSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.FrameRightSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameRightSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.FrameRightSizingTemplate.normal;
					}
				}

				// Token: 0x040040A9 RID: 16553
				private static readonly int part = 34;

				// Token: 0x040040AA RID: 16554
				private static VisualStyleElement normal;
			}

			// Token: 0x02000890 RID: 2192
			public static class SmallFrameRightSizingTemplate
			{
				// Token: 0x17001849 RID: 6217
				// (get) Token: 0x06006CB5 RID: 27829 RVA: 0x0018ED95 File Offset: 0x0018DD95
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameRightSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.SmallFrameRightSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameRightSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.SmallFrameRightSizingTemplate.normal;
					}
				}

				// Token: 0x040040AB RID: 16555
				private static readonly int part = 35;

				// Token: 0x040040AC RID: 16556
				private static VisualStyleElement normal;
			}

			// Token: 0x02000891 RID: 2193
			public static class FrameBottomSizingTemplate
			{
				// Token: 0x1700184A RID: 6218
				// (get) Token: 0x06006CB7 RID: 27831 RVA: 0x0018EDC1 File Offset: 0x0018DDC1
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.FrameBottomSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.FrameBottomSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.FrameBottomSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.FrameBottomSizingTemplate.normal;
					}
				}

				// Token: 0x040040AD RID: 16557
				private static readonly int part = 36;

				// Token: 0x040040AE RID: 16558
				private static VisualStyleElement normal;
			}

			// Token: 0x02000892 RID: 2194
			public static class SmallFrameBottomSizingTemplate
			{
				// Token: 0x1700184B RID: 6219
				// (get) Token: 0x06006CB9 RID: 27833 RVA: 0x0018EDED File Offset: 0x0018DDED
				public static VisualStyleElement Normal
				{
					get
					{
						if (VisualStyleElement.Window.SmallFrameBottomSizingTemplate.normal == null)
						{
							VisualStyleElement.Window.SmallFrameBottomSizingTemplate.normal = new VisualStyleElement(VisualStyleElement.Window.className, VisualStyleElement.Window.SmallFrameBottomSizingTemplate.part, 0);
						}
						return VisualStyleElement.Window.SmallFrameBottomSizingTemplate.normal;
					}
				}

				// Token: 0x040040AF RID: 16559
				private static readonly int part = 37;

				// Token: 0x040040B0 RID: 16560
				private static VisualStyleElement normal;
			}
		}
	}
}

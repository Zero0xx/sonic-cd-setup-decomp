using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x020005BD RID: 1469
	public class ProfessionalColorTable
	{
		// Token: 0x17000F45 RID: 3909
		// (get) Token: 0x06004C39 RID: 19513 RVA: 0x00112964 File Offset: 0x00111964
		private Dictionary<ProfessionalColorTable.KnownColors, Color> ColorTable
		{
			get
			{
				if (this.UseSystemColors)
				{
					if (!this.usingSystemColors || this.professionalRGB == null)
					{
						if (this.professionalRGB == null)
						{
							this.professionalRGB = new Dictionary<ProfessionalColorTable.KnownColors, Color>(212);
						}
						this.InitSystemColors(ref this.professionalRGB);
					}
				}
				else if (ToolStripManager.VisualStylesEnabled)
				{
					if (this.usingSystemColors || this.professionalRGB == null)
					{
						if (this.professionalRGB == null)
						{
							this.professionalRGB = new Dictionary<ProfessionalColorTable.KnownColors, Color>(212);
						}
						this.InitThemedColors(ref this.professionalRGB);
					}
				}
				else if (!this.usingSystemColors || this.professionalRGB == null)
				{
					if (this.professionalRGB == null)
					{
						this.professionalRGB = new Dictionary<ProfessionalColorTable.KnownColors, Color>(212);
					}
					this.InitSystemColors(ref this.professionalRGB);
				}
				return this.professionalRGB;
			}
		}

		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x06004C3A RID: 19514 RVA: 0x00112A29 File Offset: 0x00111A29
		// (set) Token: 0x06004C3B RID: 19515 RVA: 0x00112A31 File Offset: 0x00111A31
		public bool UseSystemColors
		{
			get
			{
				return this.useSystemColors;
			}
			set
			{
				if (this.useSystemColors != value)
				{
					this.useSystemColors = value;
					this.ResetRGBTable();
				}
			}
		}

		// Token: 0x06004C3C RID: 19516 RVA: 0x00112A4C File Offset: 0x00111A4C
		internal Color FromKnownColor(ProfessionalColorTable.KnownColors color)
		{
			if (ProfessionalColors.ColorFreshnessKey != this.colorFreshnessKey || ProfessionalColors.ColorScheme != this.lastKnownColorScheme)
			{
				this.ResetRGBTable();
			}
			this.colorFreshnessKey = ProfessionalColors.ColorFreshnessKey;
			this.lastKnownColorScheme = ProfessionalColors.ColorScheme;
			return this.ColorTable[color];
		}

		// Token: 0x06004C3D RID: 19517 RVA: 0x00112AA0 File Offset: 0x00111AA0
		private void ResetRGBTable()
		{
			if (this.professionalRGB != null)
			{
				this.professionalRGB.Clear();
			}
			this.professionalRGB = null;
		}

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x06004C3E RID: 19518 RVA: 0x00112ABC File Offset: 0x00111ABC
		[SRDescription("ProfessionalColorsButtonSelectedHighlightDescr")]
		public virtual Color ButtonSelectedHighlight
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.ButtonSelectedHighlight);
			}
		}

		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x06004C3F RID: 19519 RVA: 0x00112AC9 File Offset: 0x00111AC9
		[SRDescription("ProfessionalColorsButtonSelectedHighlightBorderDescr")]
		public virtual Color ButtonSelectedHighlightBorder
		{
			get
			{
				return this.ButtonPressedBorder;
			}
		}

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x06004C40 RID: 19520 RVA: 0x00112AD1 File Offset: 0x00111AD1
		[SRDescription("ProfessionalColorsButtonPressedHighlightDescr")]
		public virtual Color ButtonPressedHighlight
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.ButtonPressedHighlight);
			}
		}

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x06004C41 RID: 19521 RVA: 0x00112ADE File Offset: 0x00111ADE
		[SRDescription("ProfessionalColorsButtonPressedHighlightBorderDescr")]
		public virtual Color ButtonPressedHighlightBorder
		{
			get
			{
				return SystemColors.Highlight;
			}
		}

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x06004C42 RID: 19522 RVA: 0x00112AE5 File Offset: 0x00111AE5
		[SRDescription("ProfessionalColorsButtonCheckedHighlightDescr")]
		public virtual Color ButtonCheckedHighlight
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.ButtonCheckedHighlight);
			}
		}

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x06004C43 RID: 19523 RVA: 0x00112AF2 File Offset: 0x00111AF2
		[SRDescription("ProfessionalColorsButtonCheckedHighlightBorderDescr")]
		public virtual Color ButtonCheckedHighlightBorder
		{
			get
			{
				return SystemColors.Highlight;
			}
		}

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x06004C44 RID: 19524 RVA: 0x00112AF9 File Offset: 0x00111AF9
		[SRDescription("ProfessionalColorsButtonPressedBorderDescr")]
		public virtual Color ButtonPressedBorder
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver);
			}
		}

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x06004C45 RID: 19525 RVA: 0x00112B02 File Offset: 0x00111B02
		[SRDescription("ProfessionalColorsButtonSelectedBorderDescr")]
		public virtual Color ButtonSelectedBorder
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver);
			}
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x06004C46 RID: 19526 RVA: 0x00112B0B File Offset: 0x00111B0B
		[SRDescription("ProfessionalColorsButtonCheckedGradientBeginDescr")]
		public virtual Color ButtonCheckedGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedBegin);
			}
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x06004C47 RID: 19527 RVA: 0x00112B15 File Offset: 0x00111B15
		[SRDescription("ProfessionalColorsButtonCheckedGradientMiddleDescr")]
		public virtual Color ButtonCheckedGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedMiddle);
			}
		}

		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x06004C48 RID: 19528 RVA: 0x00112B1F File Offset: 0x00111B1F
		[SRDescription("ProfessionalColorsButtonCheckedGradientEndDescr")]
		public virtual Color ButtonCheckedGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedEnd);
			}
		}

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x06004C49 RID: 19529 RVA: 0x00112B29 File Offset: 0x00111B29
		[SRDescription("ProfessionalColorsButtonSelectedGradientBeginDescr")]
		public virtual Color ButtonSelectedGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverBegin);
			}
		}

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x06004C4A RID: 19530 RVA: 0x00112B33 File Offset: 0x00111B33
		[SRDescription("ProfessionalColorsButtonSelectedGradientMiddleDescr")]
		public virtual Color ButtonSelectedGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle);
			}
		}

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x06004C4B RID: 19531 RVA: 0x00112B3D File Offset: 0x00111B3D
		[SRDescription("ProfessionalColorsButtonSelectedGradientEndDescr")]
		public virtual Color ButtonSelectedGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverEnd);
			}
		}

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x06004C4C RID: 19532 RVA: 0x00112B47 File Offset: 0x00111B47
		[SRDescription("ProfessionalColorsButtonPressedGradientBeginDescr")]
		public virtual Color ButtonPressedGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownBegin);
			}
		}

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x06004C4D RID: 19533 RVA: 0x00112B51 File Offset: 0x00111B51
		[SRDescription("ProfessionalColorsButtonPressedGradientMiddleDescr")]
		public virtual Color ButtonPressedGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle);
			}
		}

		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x06004C4E RID: 19534 RVA: 0x00112B5B File Offset: 0x00111B5B
		[SRDescription("ProfessionalColorsButtonPressedGradientEndDescr")]
		public virtual Color ButtonPressedGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownEnd);
			}
		}

		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x06004C4F RID: 19535 RVA: 0x00112B65 File Offset: 0x00111B65
		[SRDescription("ProfessionalColorsCheckBackgroundDescr")]
		public virtual Color CheckBackground
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelected);
			}
		}

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x06004C50 RID: 19536 RVA: 0x00112B6F File Offset: 0x00111B6F
		[SRDescription("ProfessionalColorsCheckSelectedBackgroundDescr")]
		public virtual Color CheckSelectedBackground
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver);
			}
		}

		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x06004C51 RID: 19537 RVA: 0x00112B79 File Offset: 0x00111B79
		[SRDescription("ProfessionalColorsCheckPressedBackgroundDescr")]
		public virtual Color CheckPressedBackground
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver);
			}
		}

		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x06004C52 RID: 19538 RVA: 0x00112B83 File Offset: 0x00111B83
		[SRDescription("ProfessionalColorsGripDarkDescr")]
		public virtual Color GripDark
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBDragHandle);
			}
		}

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x06004C53 RID: 19539 RVA: 0x00112B8D File Offset: 0x00111B8D
		[SRDescription("ProfessionalColorsGripLightDescr")]
		public virtual Color GripLight
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBDragHandleShadow);
			}
		}

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x06004C54 RID: 19540 RVA: 0x00112B97 File Offset: 0x00111B97
		[SRDescription("ProfessionalColorsImageMarginGradientBeginDescr")]
		public virtual Color ImageMarginGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradVertBegin);
			}
		}

		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x06004C55 RID: 19541 RVA: 0x00112BA1 File Offset: 0x00111BA1
		[SRDescription("ProfessionalColorsImageMarginGradientMiddleDescr")]
		public virtual Color ImageMarginGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradVertMiddle);
			}
		}

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x06004C56 RID: 19542 RVA: 0x00112BAB File Offset: 0x00111BAB
		[SRDescription("ProfessionalColorsImageMarginGradientEndDescr")]
		public virtual Color ImageMarginGradientEnd
		{
			get
			{
				if (!this.usingSystemColors)
				{
					return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradVertEnd);
				}
				return SystemColors.Control;
			}
		}

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x06004C57 RID: 19543 RVA: 0x00112BC3 File Offset: 0x00111BC3
		[SRDescription("ProfessionalColorsImageMarginRevealedGradientBeginDescr")]
		public virtual Color ImageMarginRevealedGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin);
			}
		}

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x06004C58 RID: 19544 RVA: 0x00112BCD File Offset: 0x00111BCD
		[SRDescription("ProfessionalColorsImageMarginRevealedGradientMiddleDescr")]
		public virtual Color ImageMarginRevealedGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle);
			}
		}

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06004C59 RID: 19545 RVA: 0x00112BD7 File Offset: 0x00111BD7
		[SRDescription("ProfessionalColorsImageMarginRevealedGradientEndDescr")]
		public virtual Color ImageMarginRevealedGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd);
			}
		}

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06004C5A RID: 19546 RVA: 0x00112BE1 File Offset: 0x00111BE1
		[SRDescription("ProfessionalColorsMenuStripGradientBeginDescr")]
		public virtual Color MenuStripGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin);
			}
		}

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x06004C5B RID: 19547 RVA: 0x00112BEB File Offset: 0x00111BEB
		[SRDescription("ProfessionalColorsMenuStripGradientEndDescr")]
		public virtual Color MenuStripGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd);
			}
		}

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x06004C5C RID: 19548 RVA: 0x00112BF5 File Offset: 0x00111BF5
		[SRDescription("ProfessionalColorsMenuItemSelectedDescr")]
		public virtual Color MenuItemSelected
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver);
			}
		}

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x06004C5D RID: 19549 RVA: 0x00112BFF File Offset: 0x00111BFF
		[SRDescription("ProfessionalColorsMenuItemBorderDescr")]
		public virtual Color MenuItemBorder
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelected);
			}
		}

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x06004C5E RID: 19550 RVA: 0x00112C08 File Offset: 0x00111C08
		[SRDescription("ProfessionalColorsMenuBorderDescr")]
		public virtual Color MenuBorder
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBMenuBdrOuter);
			}
		}

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x06004C5F RID: 19551 RVA: 0x00112C12 File Offset: 0x00111C12
		[SRDescription("ProfessionalColorsMenuItemSelectedGradientBeginDescr")]
		public virtual Color MenuItemSelectedGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverBegin);
			}
		}

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x06004C60 RID: 19552 RVA: 0x00112C1C File Offset: 0x00111C1C
		[SRDescription("ProfessionalColorsMenuItemSelectedGradientEndDescr")]
		public virtual Color MenuItemSelectedGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverEnd);
			}
		}

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06004C61 RID: 19553 RVA: 0x00112C26 File Offset: 0x00111C26
		[SRDescription("ProfessionalColorsMenuItemPressedGradientBeginDescr")]
		public virtual Color MenuItemPressedGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin);
			}
		}

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06004C62 RID: 19554 RVA: 0x00112C30 File Offset: 0x00111C30
		[SRDescription("ProfessionalColorsMenuItemPressedGradientMiddleDescr")]
		public virtual Color MenuItemPressedGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle);
			}
		}

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x06004C63 RID: 19555 RVA: 0x00112C3A File Offset: 0x00111C3A
		[SRDescription("ProfessionalColorsMenuItemPressedGradientEndDescr")]
		public virtual Color MenuItemPressedGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd);
			}
		}

		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x06004C64 RID: 19556 RVA: 0x00112C44 File Offset: 0x00111C44
		[SRDescription("ProfessionalColorsRaftingContainerGradientBeginDescr")]
		public virtual Color RaftingContainerGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin);
			}
		}

		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x06004C65 RID: 19557 RVA: 0x00112C4E File Offset: 0x00111C4E
		[SRDescription("ProfessionalColorsRaftingContainerGradientEndDescr")]
		public virtual Color RaftingContainerGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd);
			}
		}

		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x06004C66 RID: 19558 RVA: 0x00112C58 File Offset: 0x00111C58
		[SRDescription("ProfessionalColorsSeparatorDarkDescr")]
		public virtual Color SeparatorDark
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLine);
			}
		}

		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x06004C67 RID: 19559 RVA: 0x00112C62 File Offset: 0x00111C62
		[SRDescription("ProfessionalColorsSeparatorLightDescr")]
		public virtual Color SeparatorLight
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLineLight);
			}
		}

		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x06004C68 RID: 19560 RVA: 0x00112C6C File Offset: 0x00111C6C
		[SRDescription("ProfessionalColorsStatusStripGradientBeginDescr")]
		public virtual Color StatusStripGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin);
			}
		}

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x06004C69 RID: 19561 RVA: 0x00112C76 File Offset: 0x00111C76
		[SRDescription("ProfessionalColorsStatusStripGradientEndDescr")]
		public virtual Color StatusStripGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd);
			}
		}

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x06004C6A RID: 19562 RVA: 0x00112C80 File Offset: 0x00111C80
		[SRDescription("ProfessionalColorsToolStripBorderDescr")]
		public virtual Color ToolStripBorder
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBShadow);
			}
		}

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x06004C6B RID: 19563 RVA: 0x00112C8A File Offset: 0x00111C8A
		[SRDescription("ProfessionalColorsToolStripDropDownBackgroundDescr")]
		public virtual Color ToolStripDropDownBackground
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBMenuBkgd);
			}
		}

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x06004C6C RID: 19564 RVA: 0x00112C94 File Offset: 0x00111C94
		[SRDescription("ProfessionalColorsToolStripGradientBeginDescr")]
		public virtual Color ToolStripGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradVertBegin);
			}
		}

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x06004C6D RID: 19565 RVA: 0x00112C9E File Offset: 0x00111C9E
		[SRDescription("ProfessionalColorsToolStripGradientMiddleDescr")]
		public virtual Color ToolStripGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradVertMiddle);
			}
		}

		// Token: 0x17000F77 RID: 3959
		// (get) Token: 0x06004C6E RID: 19566 RVA: 0x00112CA8 File Offset: 0x00111CA8
		[SRDescription("ProfessionalColorsToolStripGradientEndDescr")]
		public virtual Color ToolStripGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradVertEnd);
			}
		}

		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x06004C6F RID: 19567 RVA: 0x00112CB2 File Offset: 0x00111CB2
		[SRDescription("ProfessionalColorsToolStripContentPanelGradientBeginDescr")]
		public virtual Color ToolStripContentPanelGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin);
			}
		}

		// Token: 0x17000F79 RID: 3961
		// (get) Token: 0x06004C70 RID: 19568 RVA: 0x00112CBC File Offset: 0x00111CBC
		[SRDescription("ProfessionalColorsToolStripContentPanelGradientEndDescr")]
		public virtual Color ToolStripContentPanelGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd);
			}
		}

		// Token: 0x17000F7A RID: 3962
		// (get) Token: 0x06004C71 RID: 19569 RVA: 0x00112CC6 File Offset: 0x00111CC6
		[SRDescription("ProfessionalColorsToolStripPanelGradientBeginDescr")]
		public virtual Color ToolStripPanelGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin);
			}
		}

		// Token: 0x17000F7B RID: 3963
		// (get) Token: 0x06004C72 RID: 19570 RVA: 0x00112CD0 File Offset: 0x00111CD0
		[SRDescription("ProfessionalColorsToolStripPanelGradientEndDescr")]
		public virtual Color ToolStripPanelGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd);
			}
		}

		// Token: 0x17000F7C RID: 3964
		// (get) Token: 0x06004C73 RID: 19571 RVA: 0x00112CDA File Offset: 0x00111CDA
		[SRDescription("ProfessionalColorsOverflowButtonGradientBeginDescr")]
		public virtual Color OverflowButtonGradientBegin
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsBegin);
			}
		}

		// Token: 0x17000F7D RID: 3965
		// (get) Token: 0x06004C74 RID: 19572 RVA: 0x00112CE4 File Offset: 0x00111CE4
		[SRDescription("ProfessionalColorsOverflowButtonGradientMiddleDescr")]
		public virtual Color OverflowButtonGradientMiddle
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMiddle);
			}
		}

		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x06004C75 RID: 19573 RVA: 0x00112CEE File Offset: 0x00111CEE
		[SRDescription("ProfessionalColorsOverflowButtonGradientEndDescr")]
		public virtual Color OverflowButtonGradientEnd
		{
			get
			{
				return this.FromKnownColor(ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsEnd);
			}
		}

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x06004C76 RID: 19574 RVA: 0x00112CF8 File Offset: 0x00111CF8
		internal Color ComboBoxButtonGradientBegin
		{
			get
			{
				return this.MenuItemPressedGradientBegin;
			}
		}

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x06004C77 RID: 19575 RVA: 0x00112D00 File Offset: 0x00111D00
		internal Color ComboBoxButtonGradientEnd
		{
			get
			{
				return this.MenuItemPressedGradientEnd;
			}
		}

		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x06004C78 RID: 19576 RVA: 0x00112D08 File Offset: 0x00111D08
		internal Color ComboBoxButtonSelectedGradientBegin
		{
			get
			{
				return this.MenuItemSelectedGradientBegin;
			}
		}

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x06004C79 RID: 19577 RVA: 0x00112D10 File Offset: 0x00111D10
		internal Color ComboBoxButtonSelectedGradientEnd
		{
			get
			{
				return this.MenuItemSelectedGradientEnd;
			}
		}

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x06004C7A RID: 19578 RVA: 0x00112D18 File Offset: 0x00111D18
		internal Color ComboBoxButtonPressedGradientBegin
		{
			get
			{
				return this.ButtonPressedGradientBegin;
			}
		}

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x06004C7B RID: 19579 RVA: 0x00112D20 File Offset: 0x00111D20
		internal Color ComboBoxButtonPressedGradientEnd
		{
			get
			{
				return this.ButtonPressedGradientEnd;
			}
		}

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x06004C7C RID: 19580 RVA: 0x00112D28 File Offset: 0x00111D28
		internal Color ComboBoxButtonOnOverflow
		{
			get
			{
				return this.ToolStripDropDownBackground;
			}
		}

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x06004C7D RID: 19581 RVA: 0x00112D30 File Offset: 0x00111D30
		internal Color ComboBoxBorder
		{
			get
			{
				return this.ButtonSelectedHighlightBorder;
			}
		}

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x06004C7E RID: 19582 RVA: 0x00112D38 File Offset: 0x00111D38
		internal Color TextBoxBorder
		{
			get
			{
				return this.ButtonSelectedHighlightBorder;
			}
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x00112D40 File Offset: 0x00111D40
		private static Color GetAlphaBlendedColor(Graphics g, Color src, Color dest, int alpha)
		{
			int red = ((int)src.R * alpha + (255 - alpha) * (int)dest.R) / 255;
			int green = ((int)src.G * alpha + (255 - alpha) * (int)dest.G) / 255;
			int blue = ((int)src.B * alpha + (255 - alpha) * (int)dest.B) / 255;
			int alpha2 = ((int)src.A * alpha + (255 - alpha) * (int)dest.A) / 255;
			if (g == null)
			{
				return Color.FromArgb(alpha2, red, green, blue);
			}
			return g.GetNearestColor(Color.FromArgb(alpha2, red, green, blue));
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x00112DEC File Offset: 0x00111DEC
		private static Color GetAlphaBlendedColorHighRes(Graphics graphics, Color src, Color dest, int alpha)
		{
			int num;
			int num2;
			if (alpha < 100)
			{
				num = 100 - alpha;
				num2 = 100;
			}
			else
			{
				num = 1000 - alpha;
				num2 = 1000;
			}
			int red = (alpha * (int)src.R + num * (int)dest.R + num2 / 2) / num2;
			int green = (alpha * (int)src.G + num * (int)dest.G + num2 / 2) / num2;
			int blue = (alpha * (int)src.B + num * (int)dest.B + num2 / 2) / num2;
			if (graphics == null)
			{
				return Color.FromArgb(red, green, blue);
			}
			return graphics.GetNearestColor(Color.FromArgb(red, green, blue));
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x00112E8C File Offset: 0x00111E8C
		private void InitCommonColors(ref Dictionary<ProfessionalColorTable.KnownColors, Color> rgbTable)
		{
			if (!DisplayInformation.LowResolution)
			{
				using (Graphics graphics = WindowsFormsUtils.CreateMeasurementGraphics())
				{
					rgbTable[ProfessionalColorTable.KnownColors.ButtonPressedHighlight] = ProfessionalColorTable.GetAlphaBlendedColor(graphics, SystemColors.Window, ProfessionalColorTable.GetAlphaBlendedColor(graphics, SystemColors.Highlight, SystemColors.Window, 160), 50);
					rgbTable[ProfessionalColorTable.KnownColors.ButtonCheckedHighlight] = ProfessionalColorTable.GetAlphaBlendedColor(graphics, SystemColors.Window, ProfessionalColorTable.GetAlphaBlendedColor(graphics, SystemColors.Highlight, SystemColors.Window, 80), 20);
					rgbTable[ProfessionalColorTable.KnownColors.ButtonSelectedHighlight] = rgbTable[ProfessionalColorTable.KnownColors.ButtonCheckedHighlight];
					return;
				}
			}
			rgbTable[ProfessionalColorTable.KnownColors.ButtonPressedHighlight] = SystemColors.Highlight;
			rgbTable[ProfessionalColorTable.KnownColors.ButtonCheckedHighlight] = SystemColors.ControlLight;
			rgbTable[ProfessionalColorTable.KnownColors.ButtonSelectedHighlight] = SystemColors.ControlLight;
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x00112F6C File Offset: 0x00111F6C
		internal void InitSystemColors(ref Dictionary<ProfessionalColorTable.KnownColors, Color> rgbTable)
		{
			this.usingSystemColors = true;
			this.InitCommonColors(ref rgbTable);
			Color buttonFace = SystemColors.ButtonFace;
			Color buttonShadow = SystemColors.ButtonShadow;
			Color highlight = SystemColors.Highlight;
			Color window = SystemColors.Window;
			Color empty = Color.Empty;
			Color controlText = SystemColors.ControlText;
			Color buttonHighlight = SystemColors.ButtonHighlight;
			Color grayText = SystemColors.GrayText;
			Color highlightText = SystemColors.HighlightText;
			Color windowText = SystemColors.WindowText;
			Color value = buttonFace;
			Color value2 = buttonFace;
			Color value3 = buttonFace;
			Color value4 = highlight;
			Color value5 = highlight;
			bool lowResolution = DisplayInformation.LowResolution;
			bool highContrast = DisplayInformation.HighContrast;
			if (lowResolution)
			{
				value4 = window;
			}
			else if (!highContrast)
			{
				value = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 23);
				value2 = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 50);
				value3 = SystemColors.ButtonFace;
				value4 = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, highlight, window, 30);
				value5 = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, highlight, window, 50);
			}
			if (lowResolution || highContrast)
			{
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBkgd] = buttonFace;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = SystemColors.ControlLight;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandle] = controlText;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = buttonFace;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = buttonShadow;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = buttonShadow;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = buttonShadow;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = buttonShadow;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = buttonShadow;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = controlText;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBkgd] = window;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLine] = buttonShadow;
			}
			else
			{
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBkgd] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, window, buttonFace, 165);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, highlight, window, 50);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandle] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonShadow, window, 75);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 205);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 70);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 90);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 40);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 70);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 90);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, controlText, buttonShadow, 20);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBkgd] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonFace, window, 143);
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLine] = ProfessionalColorTable.GetAlphaBlendedColorHighRes(null, buttonShadow, window, 70);
			}
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = (lowResolution ? SystemColors.ControlLight : highlight);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterFloating] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseDown] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelected] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgd] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdLight] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseDown] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextDisabled] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextLight] = grayText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseDown] = highlightText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDockSeparatorLine] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandleShadow] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDropDownArrow] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverEnd] = value4;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverBegin] = value4;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle] = value4;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsEnd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedBegin] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedEnd] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedBegin] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedEnd] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedMiddle] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertBegin] = value;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertMiddle] = value2;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertEnd] = value3;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownBegin] = value5;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle] = value5;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownEnd] = value5;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = value;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = value2;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledDark] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledLight] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLowColorIconDisabled] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMainMenuBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlTextDisabled] = grayText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgd] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuShadow] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuSplitArrow] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBOptionsButtonShadow] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBShadow] = rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBkgd];
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLineLight] = buttonHighlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandle] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandleMouseOver] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleText] = buttonHighlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledFocuslessHighlightedText] = grayText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledHighlightedText] = grayText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDlgGroupBoxText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDark] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseDown] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = SystemColors.MenuText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLight] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseDown] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = SystemColors.MenuText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseDown] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = SystemColors.MenuText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrSelected] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseDown] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdSelected] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseDown] = highlightText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextSelected] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = buttonHighlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseDown] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseOver] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseDown] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseOver] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = SystemColors.InactiveCaption;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = SystemColors.InactiveCaptionText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBdr] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgdSelected] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderSeeThroughSelection] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentDarkBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentLightBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentTextDisabled] = grayText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPHyperlink] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPLightBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlink] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlinkFollowed] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradEnd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrListHeaderArrow] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrNetLookBkgnd] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOABBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdrContrast] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerActiveBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBdr] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerInactiveBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = buttonHighlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabStopTicks] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGWorkspaceBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFlagNone] = buttonHighlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarDark] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarLight] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarText] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGridlines] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupLine] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupNested] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupShaded] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupText] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKIconBar] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKPreviewPaneLabelText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorDark] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorLight] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBActionDividerLine] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonDark] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = buttonHighlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBDarkOutline] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBFoldersBackground] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonDark] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonLight] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBLabelText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonDark] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonLight] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonDark] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonLight] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterDark] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPlacesBarBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelected] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrInactiveSelected] = grayText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrMouseOver] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubWebDocScratchPageBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrSBBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrScrollbarBkgd] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradBegin] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradEnd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrInnerDocked] = empty;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterDocked] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterFloating] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBkgd] = window;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdr] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDisabled] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgdDisabled] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextDisabled] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextMouseDown] = highlightText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPGroupline] = buttonShadow;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipBkgd] = SystemColors.Info;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipText] = SystemColors.InfoText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPNavBarBkgnd] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = windowText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTextDisabled] = grayText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdActive] = highlight;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdInactive] = buttonFace;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextActive] = highlightText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextInactive] = controlText;
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrXLFormulaBarBkgd] = buttonFace;
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x00113B94 File Offset: 0x00112B94
		internal void InitOliveLunaColors(ref Dictionary<ProfessionalColorTable.KnownColors, Color> rgbTable)
		{
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(81, 94, 51);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(81, 94, 51);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterFloating] = Color.FromArgb(116, 134, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBkgd] = Color.FromArgb(209, 222, 173);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseDown] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelected] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgd] = Color.FromArgb(209, 222, 173);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = Color.FromArgb(255, 192, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextDisabled] = Color.FromArgb(141, 141, 141);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextLight] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDockSeparatorLine] = Color.FromArgb(96, 119, 66);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandle] = Color.FromArgb(81, 94, 51);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandleShadow] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDropDownArrow] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin] = Color.FromArgb(217, 217, 167);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = Color.FromArgb(242, 241, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = Color.FromArgb(230, 230, 209);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = Color.FromArgb(160, 177, 116);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = Color.FromArgb(186, 201, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = Color.FromArgb(237, 240, 214);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = Color.FromArgb(181, 196, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownBegin] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownEnd] = Color.FromArgb(255, 223, 154);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle] = Color.FromArgb(255, 177, 109);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverBegin] = Color.FromArgb(255, 255, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverEnd] = Color.FromArgb(255, 203, 136);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle] = Color.FromArgb(255, 225, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = Color.FromArgb(186, 204, 150);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsEnd] = Color.FromArgb(96, 119, 107);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = Color.FromArgb(141, 160, 107);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = Color.FromArgb(255, 255, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = Color.FromArgb(255, 193, 118);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = Color.FromArgb(255, 225, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedBegin] = Color.FromArgb(254, 140, 73);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedEnd] = Color.FromArgb(255, 221, 152);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = Color.FromArgb(255, 184, 116);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedBegin] = Color.FromArgb(255, 223, 154);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedEnd] = Color.FromArgb(255, 166, 76);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedMiddle] = Color.FromArgb(255, 195, 116);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertBegin] = Color.FromArgb(255, 255, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertEnd] = Color.FromArgb(181, 196, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertMiddle] = Color.FromArgb(206, 220, 167);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledDark] = Color.FromArgb(131, 144, 113);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledLight] = Color.FromArgb(243, 244, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(218, 227, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(218, 227, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLowColorIconDisabled] = Color.FromArgb(159, 174, 122);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMainMenuBkgd] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = Color.FromArgb(117, 141, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBkgd] = Color.FromArgb(244, 244, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlTextDisabled] = Color.FromArgb(141, 141, 141);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgd] = Color.FromArgb(216, 227, 182);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(173, 181, 157);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(173, 181, 157);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuShadow] = Color.FromArgb(134, 148, 108);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuSplitArrow] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBOptionsButtonShadow] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBShadow] = Color.FromArgb(96, 128, 88);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLine] = Color.FromArgb(96, 128, 88);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLineLight] = Color.FromArgb(244, 247, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandle] = Color.FromArgb(197, 212, 159);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandleMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleBkgd] = Color.FromArgb(116, 134, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledFocuslessHighlightedText] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledHighlightedText] = Color.FromArgb(220, 224, 208);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDlgGroupBoxText] = Color.FromArgb(153, 84, 10);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdr] = Color.FromArgb(96, 119, 107);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDark] = Color.FromArgb(176, 194, 140);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseDown] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseDown] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseDown] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(63, 93, 56);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrSelected] = Color.FromArgb(96, 128, 88);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgd] = Color.FromArgb(218, 227, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdSelected] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextSelected] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(218, 227, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(218, 227, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(183, 198, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(183, 198, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBdr] = Color.FromArgb(191, 191, 223);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBkgd] = Color.FromArgb(239, 235, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBdr] = Color.FromArgb(126, 125, 104);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgd] = Color.FromArgb(239, 235, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgdSelected] = Color.FromArgb(255, 192, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderSeeThroughSelection] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(159, 171, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(159, 171, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentDarkBkgd] = Color.FromArgb(217, 227, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentLightBkgd] = Color.FromArgb(230, 234, 208);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentTextDisabled] = Color.FromArgb(150, 145, 133);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = Color.FromArgb(161, 176, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = Color.FromArgb(210, 223, 174);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(90, 107, 70);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(90, 107, 70);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPHyperlink] = Color.FromArgb(0, 61, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPLightBkgd] = Color.FromArgb(243, 242, 231);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlink] = Color.FromArgb(0, 61, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlinkFollowed] = Color.FromArgb(170, 0, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(96, 128, 88);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(96, 128, 88);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(217, 217, 167);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(217, 217, 167);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradEnd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(242, 241, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(242, 241, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrListHeaderArrow] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrNetLookBkgnd] = Color.FromArgb(255, 255, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOABBkgd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdr] = Color.FromArgb(211, 211, 211);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdrContrast] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = Color.FromArgb(151, 160, 123);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerActiveBkgd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBdr] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBkgd] = Color.FromArgb(226, 231, 191);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerInactiveBkgd] = Color.FromArgb(171, 192, 138);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdr] = Color.FromArgb(117, 141, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabStopTicks] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = Color.FromArgb(218, 227, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGWorkspaceBkgd] = Color.FromArgb(151, 160, 123);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFlagNone] = Color.FromArgb(242, 240, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarDark] = Color.FromArgb(96, 119, 66);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarLight] = Color.FromArgb(175, 192, 130);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGridlines] = Color.FromArgb(234, 233, 225);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupLine] = Color.FromArgb(181, 196, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupNested] = Color.FromArgb(253, 238, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupShaded] = Color.FromArgb(175, 186, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupText] = Color.FromArgb(115, 137, 84);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKIconBar] = Color.FromArgb(253, 247, 233);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarBkgd] = Color.FromArgb(151, 160, 123);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKPreviewPaneLabelText] = Color.FromArgb(151, 160, 123);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorDark] = Color.FromArgb(187, 85, 3);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorLight] = Color.FromArgb(251, 200, 79);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBActionDividerLine] = Color.FromArgb(200, 212, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonDark] = Color.FromArgb(176, 191, 138);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(234, 240, 207);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(234, 240, 207);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBDarkOutline] = Color.FromArgb(96, 128, 88);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBFoldersBackground] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonDark] = Color.FromArgb(247, 190, 87);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonLight] = Color.FromArgb(255, 255, 220);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBLabelText] = Color.FromArgb(50, 69, 105);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonDark] = Color.FromArgb(248, 222, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonLight] = Color.FromArgb(232, 127, 8);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonDark] = Color.FromArgb(238, 147, 17);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonLight] = Color.FromArgb(251, 230, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterDark] = Color.FromArgb(64, 81, 59);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(120, 142, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(120, 142, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPlacesBarBkgd] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = Color.FromArgb(242, 240, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = Color.FromArgb(96, 128, 88);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = Color.FromArgb(206, 220, 167);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelected] = Color.FromArgb(107, 129, 107);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = Color.FromArgb(107, 129, 107);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrInactiveSelected] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrMouseOver] = Color.FromArgb(107, 129, 107);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = Color.FromArgb(151, 160, 123);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubWebDocScratchPageBkgd] = Color.FromArgb(193, 198, 176);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrSBBdr] = Color.FromArgb(211, 211, 211);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrScrollbarBkgd] = Color.FromArgb(249, 249, 247);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradBegin] = Color.FromArgb(237, 242, 212);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradEnd] = Color.FromArgb(191, 206, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrInnerDocked] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterDocked] = Color.FromArgb(242, 241, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterFloating] = Color.FromArgb(116, 134, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBkgd] = Color.FromArgb(243, 242, 231);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdr] = Color.FromArgb(164, 185, 127);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDisabled] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgd] = Color.FromArgb(197, 212, 159);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgdDisabled] = Color.FromArgb(222, 222, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextDisabled] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPGroupline] = Color.FromArgb(188, 187, 177);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipBkgd] = Color.FromArgb(255, 255, 204);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPNavBarBkgnd] = Color.FromArgb(116, 134, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTextDisabled] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdActive] = Color.FromArgb(216, 227, 182);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdInactive] = Color.FromArgb(188, 205, 131);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextActive] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextInactive] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrXLFormulaBarBkgd] = Color.FromArgb(217, 217, 167);
		}

		// Token: 0x06004C84 RID: 19588 RVA: 0x00115494 File Offset: 0x00114494
		internal void InitSilverLunaColors(ref Dictionary<ProfessionalColorTable.KnownColors, Color> rgbTable)
		{
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(173, 174, 193);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterFloating] = Color.FromArgb(122, 121, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBkgd] = Color.FromArgb(219, 218, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseDown] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelected] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgd] = Color.FromArgb(219, 218, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = Color.FromArgb(255, 192, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextDisabled] = Color.FromArgb(141, 141, 141);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextLight] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDockSeparatorLine] = Color.FromArgb(110, 109, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandle] = Color.FromArgb(84, 84, 117);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandleShadow] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDropDownArrow] = Color.FromArgb(224, 223, 227);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin] = Color.FromArgb(215, 215, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = Color.FromArgb(243, 243, 247);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = Color.FromArgb(215, 215, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = Color.FromArgb(118, 116, 151);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = Color.FromArgb(184, 185, 202);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = Color.FromArgb(232, 233, 242);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = Color.FromArgb(172, 170, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownBegin] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownEnd] = Color.FromArgb(255, 223, 154);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle] = Color.FromArgb(255, 177, 109);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverBegin] = Color.FromArgb(255, 255, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverEnd] = Color.FromArgb(255, 203, 136);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle] = Color.FromArgb(255, 225, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = Color.FromArgb(186, 185, 206);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsEnd] = Color.FromArgb(118, 116, 146);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = Color.FromArgb(156, 155, 180);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = Color.FromArgb(255, 255, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = Color.FromArgb(255, 193, 118);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = Color.FromArgb(255, 225, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedBegin] = Color.FromArgb(254, 140, 73);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedEnd] = Color.FromArgb(255, 221, 152);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = Color.FromArgb(255, 184, 116);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedBegin] = Color.FromArgb(255, 223, 154);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedEnd] = Color.FromArgb(255, 166, 76);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedMiddle] = Color.FromArgb(255, 195, 116);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertBegin] = Color.FromArgb(249, 249, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertEnd] = Color.FromArgb(147, 145, 176);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertMiddle] = Color.FromArgb(225, 226, 236);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledDark] = Color.FromArgb(122, 121, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledLight] = Color.FromArgb(247, 245, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(212, 212, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(212, 212, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLowColorIconDisabled] = Color.FromArgb(168, 167, 190);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMainMenuBkgd] = Color.FromArgb(198, 200, 215);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBkgd] = Color.FromArgb(253, 250, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlTextDisabled] = Color.FromArgb(141, 141, 141);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgd] = Color.FromArgb(214, 211, 231);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(185, 187, 200);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(185, 187, 200);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuShadow] = Color.FromArgb(154, 140, 176);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuSplitArrow] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBOptionsButtonShadow] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBShadow] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLine] = Color.FromArgb(110, 109, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLineLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandle] = Color.FromArgb(192, 192, 211);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandleMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleBkgd] = Color.FromArgb(122, 121, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledFocuslessHighlightedText] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledHighlightedText] = Color.FromArgb(59, 59, 63);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDlgGroupBoxText] = Color.FromArgb(7, 70, 213);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdr] = Color.FromArgb(118, 116, 146);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDark] = Color.FromArgb(186, 185, 206);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseDown] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseDown] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseDown] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrSelected] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgd] = Color.FromArgb(212, 212, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdSelected] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextSelected] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(212, 212, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(212, 212, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(148, 148, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(148, 148, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(171, 169, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(171, 169, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(224, 223, 227);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(224, 223, 227);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBdr] = Color.FromArgb(191, 191, 223);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBkgd] = Color.FromArgb(239, 235, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBdr] = Color.FromArgb(126, 125, 104);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgd] = Color.FromArgb(223, 223, 234);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgdSelected] = Color.FromArgb(255, 192, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderSeeThroughSelection] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(162, 162, 181);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(162, 162, 181);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentDarkBkgd] = Color.FromArgb(212, 213, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentLightBkgd] = Color.FromArgb(227, 227, 236);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentTextDisabled] = Color.FromArgb(150, 145, 133);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = Color.FromArgb(169, 168, 191);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = Color.FromArgb(208, 208, 223);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(92, 91, 121);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(92, 91, 121);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPHyperlink] = Color.FromArgb(0, 61, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPLightBkgd] = Color.FromArgb(238, 238, 244);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlink] = Color.FromArgb(0, 61, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlinkFollowed] = Color.FromArgb(170, 0, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(215, 215, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(215, 215, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradEnd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(243, 243, 247);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(243, 243, 247);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrListHeaderArrow] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrNetLookBkgnd] = Color.FromArgb(249, 249, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOABBkgd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdr] = Color.FromArgb(211, 211, 211);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdrContrast] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = Color.FromArgb(155, 154, 179);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerActiveBkgd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBdr] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBkgd] = Color.FromArgb(223, 223, 234);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerInactiveBkgd] = Color.FromArgb(177, 176, 195);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdr] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabStopTicks] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = Color.FromArgb(212, 212, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGWorkspaceBkgd] = Color.FromArgb(155, 154, 179);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFlagNone] = Color.FromArgb(239, 239, 244);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarDark] = Color.FromArgb(110, 109, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarLight] = Color.FromArgb(168, 167, 191);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGridlines] = Color.FromArgb(234, 233, 225);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupLine] = Color.FromArgb(165, 164, 189);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupNested] = Color.FromArgb(253, 238, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupShaded] = Color.FromArgb(229, 229, 235);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupText] = Color.FromArgb(112, 111, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKIconBar] = Color.FromArgb(253, 247, 233);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarBkgd] = Color.FromArgb(155, 154, 179);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKPreviewPaneLabelText] = Color.FromArgb(155, 154, 179);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorDark] = Color.FromArgb(187, 85, 3);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorLight] = Color.FromArgb(251, 200, 79);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBActionDividerLine] = Color.FromArgb(204, 206, 219);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonDark] = Color.FromArgb(147, 145, 176);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(225, 226, 236);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(225, 226, 236);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBDarkOutline] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBFoldersBackground] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonDark] = Color.FromArgb(247, 190, 87);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonLight] = Color.FromArgb(255, 255, 220);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBLabelText] = Color.FromArgb(50, 69, 105);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonDark] = Color.FromArgb(248, 222, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonLight] = Color.FromArgb(232, 127, 8);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonDark] = Color.FromArgb(238, 147, 17);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonLight] = Color.FromArgb(251, 230, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterDark] = Color.FromArgb(110, 109, 143);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(168, 167, 191);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(168, 167, 191);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPlacesBarBkgd] = Color.FromArgb(224, 223, 227);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = Color.FromArgb(243, 243, 247);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = Color.FromArgb(215, 215, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelected] = Color.FromArgb(142, 142, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = Color.FromArgb(142, 142, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrInactiveSelected] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrMouseOver] = Color.FromArgb(142, 142, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = Color.FromArgb(155, 154, 179);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubWebDocScratchPageBkgd] = Color.FromArgb(195, 195, 210);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrSBBdr] = Color.FromArgb(236, 234, 218);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrScrollbarBkgd] = Color.FromArgb(247, 247, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradBegin] = Color.FromArgb(239, 239, 247);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradEnd] = Color.FromArgb(179, 178, 204);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrInnerDocked] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterDocked] = Color.FromArgb(243, 243, 247);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterFloating] = Color.FromArgb(122, 121, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBkgd] = Color.FromArgb(238, 238, 244);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdr] = Color.FromArgb(165, 172, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDisabled] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgd] = Color.FromArgb(192, 192, 211);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgdDisabled] = Color.FromArgb(222, 222, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextDisabled] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPGroupline] = Color.FromArgb(161, 160, 187);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipBkgd] = Color.FromArgb(255, 255, 204);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPNavBarBkgnd] = Color.FromArgb(122, 121, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTextDisabled] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdActive] = Color.FromArgb(184, 188, 234);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdInactive] = Color.FromArgb(198, 198, 217);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextActive] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextInactive] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrXLFormulaBarBkgd] = Color.FromArgb(215, 215, 229);
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x00116D84 File Offset: 0x00115D84
		private void InitRoyaleColors(ref Dictionary<ProfessionalColorTable.KnownColors, Color> rgbTable)
		{
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBkgd] = Color.FromArgb(238, 237, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandle] = Color.FromArgb(189, 188, 191);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLine] = Color.FromArgb(193, 193, 196);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleBkgd] = Color.FromArgb(167, 166, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterFloating] = Color.FromArgb(142, 141, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandle] = Color.FromArgb(238, 237, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandleMouseOver] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgd] = Color.FromArgb(238, 237, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextDisabled] = Color.FromArgb(176, 175, 179);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver] = Color.FromArgb(51, 94, 168);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseDown] = Color.FromArgb(153, 175, 212);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseDown] = Color.FromArgb(51, 94, 168);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseDown] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = Color.FromArgb(226, 229, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelected] = Color.FromArgb(51, 94, 168);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = Color.FromArgb(51, 94, 168);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = Color.FromArgb(51, 94, 168);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextLight] = Color.FromArgb(167, 166, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMainMenuBkgd] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBkgd] = Color.FromArgb(252, 252, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlTextDisabled] = Color.FromArgb(193, 193, 196);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = Color.FromArgb(134, 133, 136);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgd] = Color.FromArgb(238, 237, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(228, 226, 230);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuSplitArrow] = Color.FromArgb(167, 166, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBkgd] = Color.FromArgb(245, 244, 246);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdActive] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdInactive] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextActive] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextInactive] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterFloating] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterDocked] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDisabled] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextDisabled] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgdDisabled] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPGroupline] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrSBBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdrContrast] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOABBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderSeeThroughSelection] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgdSelected] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLineLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBShadow] = Color.FromArgb(238, 237, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBOptionsButtonShadow] = Color.FromArgb(245, 244, 246);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPNavBarBkgnd] = Color.FromArgb(193, 193, 196);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrInnerDocked] = Color.FromArgb(245, 244, 246);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledLight] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledDark] = Color.FromArgb(167, 166, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLowColorIconDisabled] = Color.FromArgb(176, 175, 179);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = Color.FromArgb(251, 250, 251);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertBegin] = Color.FromArgb(252, 252, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertMiddle] = Color.FromArgb(245, 244, 246);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertEnd] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = Color.FromArgb(242, 242, 242);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = Color.FromArgb(224, 224, 225);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsEnd] = Color.FromArgb(167, 166, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = Color.FromArgb(252, 252, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = Color.FromArgb(245, 244, 246);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = Color.FromArgb(247, 246, 248);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = Color.FromArgb(241, 240, 242);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = Color.FromArgb(228, 226, 230);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedBegin] = Color.FromArgb(226, 229, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = Color.FromArgb(226, 229, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedEnd] = Color.FromArgb(226, 229, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedBegin] = Color.FromArgb(226, 229, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedMiddle] = Color.FromArgb(226, 229, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedEnd] = Color.FromArgb(226, 229, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverBegin] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverEnd] = Color.FromArgb(194, 207, 229);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownBegin] = Color.FromArgb(153, 175, 212);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle] = Color.FromArgb(153, 175, 212);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownEnd] = Color.FromArgb(153, 175, 212);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrNetLookBkgnd] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuShadow] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDockSeparatorLine] = Color.FromArgb(51, 94, 168);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDropDownArrow] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGridlines] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupLine] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupShaded] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupNested] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKIconBar] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFlagNone] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarLight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarDark] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonDark] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonLight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonDark] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonLight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonDark] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonLight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonDark] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBDarkOutline] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterDark] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBActionDividerLine] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBLabelText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBFoldersBackground] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorLight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorDark] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKPreviewPaneLabelText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlink] = Color.FromArgb(0, 61, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlinkFollowed] = Color.FromArgb(170, 0, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGWorkspaceBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerActiveBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerInactiveBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabStopTicks] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrXLFormulaBarBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandleShadow] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelected] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrInactiveSelected] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrMouseOver] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDlgGroupBoxText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrScrollbarBkgd] = Color.FromArgb(237, 235, 239);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrListHeaderArrow] = Color.FromArgb(155, 154, 156);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledHighlightedText] = Color.FromArgb(188, 202, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(235, 233, 237);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledFocuslessHighlightedText] = Color.FromArgb(167, 166, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextMouseDown] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTextDisabled] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseOver] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseOver] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseDown] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseDown] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPLightBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentLightBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentDarkBkgd] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentText] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentTextDisabled] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPHyperlink] = Color.FromArgb(255, 51, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgd] = Color.FromArgb(212, 212, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdr] = Color.FromArgb(118, 116, 146);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDark] = Color.FromArgb(186, 185, 206);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdSelected] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextSelected] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrSelected] = Color.FromArgb(124, 124, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(193, 210, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(49, 106, 197);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(49, 106, 197);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(49, 106, 197);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(49, 106, 197);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseDown] = Color.FromArgb(154, 183, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseDown] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseDown] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseDown] = Color.FromArgb(75, 75, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradBegin] = Color.FromArgb(246, 244, 236);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradEnd] = Color.FromArgb(179, 178, 204);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradEnd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPlacesBarBkgd] = Color.FromArgb(224, 223, 227);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = Color.FromArgb(152, 181, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubWebDocScratchPageBkgd] = Color.FromArgb(193, 210, 238);
		}

		// Token: 0x06004C86 RID: 19590 RVA: 0x0011843C File Offset: 0x0011743C
		internal void InitThemedColors(ref Dictionary<ProfessionalColorTable.KnownColors, Color> rgbTable)
		{
			string colorScheme = VisualStyleInformation.ColorScheme;
			string fileName = Path.GetFileName(VisualStyleInformation.ThemeFilename);
			bool flag = false;
			if (string.Equals("luna.msstyles", fileName, StringComparison.OrdinalIgnoreCase))
			{
				if (colorScheme == "NormalColor")
				{
					this.InitBlueLunaColors(ref rgbTable);
					this.usingSystemColors = false;
					flag = true;
				}
				else if (colorScheme == "HomeStead")
				{
					this.InitOliveLunaColors(ref rgbTable);
					this.usingSystemColors = false;
					flag = true;
				}
				else if (colorScheme == "Metallic")
				{
					this.InitSilverLunaColors(ref rgbTable);
					this.usingSystemColors = false;
					flag = true;
				}
			}
			else if (string.Equals("aero.msstyles", fileName, StringComparison.OrdinalIgnoreCase))
			{
				this.InitSystemColors(ref rgbTable);
				this.usingSystemColors = true;
				flag = true;
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = rgbTable[ProfessionalColorTable.KnownColors.ButtonSelectedHighlight];
				rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver];
			}
			else if (string.Equals("royale.msstyles", fileName, StringComparison.OrdinalIgnoreCase) && (colorScheme == "NormalColor" || colorScheme == "Royale"))
			{
				this.InitRoyaleColors(ref rgbTable);
				this.usingSystemColors = false;
				flag = true;
			}
			if (!flag)
			{
				this.InitSystemColors(ref rgbTable);
				this.usingSystemColors = true;
			}
			this.InitCommonColors(ref rgbTable);
		}

		// Token: 0x06004C87 RID: 19591 RVA: 0x00118568 File Offset: 0x00117568
		internal void InitBlueLunaColors(ref Dictionary<ProfessionalColorTable.KnownColors, Color> rgbTable)
		{
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(196, 205, 218);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterDocked] = Color.FromArgb(196, 205, 218);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBdrOuterFloating] = Color.FromArgb(42, 102, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBBkgd] = Color.FromArgb(196, 219, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseDown] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelected] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgd] = Color.FromArgb(196, 219, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelected] = Color.FromArgb(255, 192, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextDisabled] = Color.FromArgb(141, 141, 141);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextLight] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBCtlTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDockSeparatorLine] = Color.FromArgb(0, 53, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandle] = Color.FromArgb(39, 65, 118);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDragHandleShadow] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBDropDownArrow] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzBegin] = Color.FromArgb(158, 190, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMainMenuHorzEnd] = Color.FromArgb(196, 218, 250);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = Color.FromArgb(203, 221, 246);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = Color.FromArgb(114, 155, 215);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = Color.FromArgb(161, 197, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = Color.FromArgb(227, 239, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = Color.FromArgb(123, 164, 224);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownBegin] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownEnd] = Color.FromArgb(255, 223, 154);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseDownMiddle] = Color.FromArgb(255, 177, 109);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverBegin] = Color.FromArgb(255, 255, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverEnd] = Color.FromArgb(255, 203, 136);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradMouseOverMiddle] = Color.FromArgb(255, 225, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsBegin] = Color.FromArgb(127, 177, 250);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsEnd] = Color.FromArgb(0, 53, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMiddle] = Color.FromArgb(82, 127, 208);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = Color.FromArgb(255, 255, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = Color.FromArgb(255, 193, 118);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = Color.FromArgb(255, 225, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedBegin] = Color.FromArgb(254, 140, 73);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedEnd] = Color.FromArgb(255, 221, 152);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = Color.FromArgb(255, 184, 116);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedBegin] = Color.FromArgb(255, 223, 154);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedEnd] = Color.FromArgb(255, 166, 76);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradSelectedMiddle] = Color.FromArgb(255, 195, 116);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertBegin] = Color.FromArgb(227, 239, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertEnd] = Color.FromArgb(123, 164, 224);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBGradVertMiddle] = Color.FromArgb(203, 225, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledDark] = Color.FromArgb(97, 122, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBIconDisabledLight] = Color.FromArgb(233, 236, 242);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(186, 211, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLabelBkgnd] = Color.FromArgb(186, 211, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBLowColorIconDisabled] = Color.FromArgb(109, 150, 208);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMainMenuBkgd] = Color.FromArgb(153, 204, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBdrOuter] = Color.FromArgb(0, 45, 150);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuBkgd] = Color.FromArgb(246, 246, 246);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuCtlTextDisabled] = Color.FromArgb(141, 141, 141);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgd] = Color.FromArgb(203, 225, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(172, 183, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuIconBkgdDropped] = Color.FromArgb(172, 183, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuShadow] = Color.FromArgb(95, 130, 234);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBMenuSplitArrow] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBOptionsButtonShadow] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBShadow] = Color.FromArgb(59, 97, 156);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLine] = Color.FromArgb(106, 140, 203);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBSplitterLineLight] = Color.FromArgb(241, 249, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandle] = Color.FromArgb(169, 199, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTearOffHandleMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleBkgd] = Color.FromArgb(42, 102, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrCBTitleText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledFocuslessHighlightedText] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDisabledHighlightedText] = Color.FromArgb(187, 206, 236);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDlgGroupBoxText] = Color.FromArgb(0, 70, 213);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdr] = Color.FromArgb(0, 53, 154);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDark] = Color.FromArgb(117, 166, 241);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseDown] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrDarkMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseDown] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrLightMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseDown] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrMouseOver] = Color.FromArgb(0, 0, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBdrSelected] = Color.FromArgb(59, 97, 156);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgd] = Color.FromArgb(186, 211, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabBkgdSelected] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDocTabTextSelected] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(186, 211, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabBkgd] = Color.FromArgb(186, 211, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(94, 94, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWActiveTabTextDisabled] = Color.FromArgb(94, 94, 94);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(129, 169, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabBkgd] = Color.FromArgb(129, 169, 226);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWInactiveTabText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseDown] = Color.FromArgb(254, 128, 62);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabBkgdMouseOver] = Color.FromArgb(255, 238, 194);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrDWTabTextMouseOver] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedBkgd] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrFocuslessHighlightedText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBdr] = Color.FromArgb(89, 89, 172);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderBkgd] = Color.FromArgb(239, 235, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBdr] = Color.FromArgb(126, 125, 104);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgd] = Color.FromArgb(239, 235, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderCellBkgdSelected] = Color.FromArgb(255, 192, 111);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGDHeaderSeeThroughSelection] = Color.FromArgb(191, 191, 223);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(74, 122, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPDarkBkgd] = Color.FromArgb(74, 122, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentDarkBkgd] = Color.FromArgb(185, 208, 241);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentLightBkgd] = Color.FromArgb(221, 236, 254);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupContentTextDisabled] = Color.FromArgb(150, 145, 133);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = Color.FromArgb(101, 143, 224);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = Color.FromArgb(196, 219, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(0, 45, 134);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupHeaderText] = Color.FromArgb(0, 45, 134);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPGroupline] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPHyperlink] = Color.FromArgb(0, 61, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrGSPLightBkgd] = Color.FromArgb(221, 236, 254);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlink] = Color.FromArgb(0, 61, 178);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrHyperlinkFollowed] = Color.FromArgb(170, 0, 170);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(59, 97, 156);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIBdr] = Color.FromArgb(59, 97, 156);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(158, 190, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradBegin] = Color.FromArgb(158, 190, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradEnd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(196, 218, 250);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIGradMiddle] = Color.FromArgb(196, 218, 250);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrJotNavUIText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrListHeaderArrow] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrNetLookBkgnd] = Color.FromArgb(227, 239, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOABBkgd] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdr] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOBBkgdBdrContrast] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = Color.FromArgb(144, 153, 174);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerActiveBkgd] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBdr] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerBkgd] = Color.FromArgb(216, 231, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerInactiveBkgd] = Color.FromArgb(158, 190, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdr] = Color.FromArgb(75, 120, 202);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerTabStopTicks] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGRulerText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = Color.FromArgb(186, 211, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOGWorkspaceBkgd] = Color.FromArgb(144, 153, 174);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFlagNone] = Color.FromArgb(242, 240, 228);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarDark] = Color.FromArgb(0, 53, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarLight] = Color.FromArgb(89, 135, 214);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKFolderbarText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGridlines] = Color.FromArgb(234, 233, 225);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupLine] = Color.FromArgb(123, 164, 224);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupNested] = Color.FromArgb(253, 238, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupShaded] = Color.FromArgb(190, 218, 251);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKGroupText] = Color.FromArgb(55, 104, 185);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKIconBar] = Color.FromArgb(253, 247, 233);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarBkgd] = Color.FromArgb(144, 153, 174);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKInfoBarText] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKPreviewPaneLabelText] = Color.FromArgb(144, 153, 174);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorDark] = Color.FromArgb(187, 85, 3);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKTodayIndicatorLight] = Color.FromArgb(251, 200, 79);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBActionDividerLine] = Color.FromArgb(215, 228, 251);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonDark] = Color.FromArgb(123, 164, 224);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(203, 225, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBButtonLight] = Color.FromArgb(203, 225, 252);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBDarkOutline] = Color.FromArgb(0, 45, 150);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBFoldersBackground] = Color.FromArgb(255, 255, 255);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonDark] = Color.FromArgb(247, 190, 87);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBHoverButtonLight] = Color.FromArgb(255, 255, 220);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBLabelText] = Color.FromArgb(50, 69, 105);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonDark] = Color.FromArgb(248, 222, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBPressedButtonLight] = Color.FromArgb(232, 127, 8);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonDark] = Color.FromArgb(238, 147, 17);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSelectedButtonLight] = Color.FromArgb(251, 230, 148);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterDark] = Color.FromArgb(0, 53, 145);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(89, 135, 214);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrOLKWBSplitterLight] = Color.FromArgb(89, 135, 214);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPlacesBarBkgd] = Color.FromArgb(236, 233, 216);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = Color.FromArgb(195, 218, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = Color.FromArgb(59, 97, 156);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = Color.FromArgb(158, 190, 245);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelected] = Color.FromArgb(61, 108, 192);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = Color.FromArgb(61, 108, 192);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrInactiveSelected] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPPSlideBdrMouseOver] = Color.FromArgb(61, 108, 192);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = Color.FromArgb(144, 153, 174);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrPubWebDocScratchPageBkgd] = Color.FromArgb(189, 194, 207);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrSBBdr] = Color.FromArgb(211, 211, 211);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrScrollbarBkgd] = Color.FromArgb(251, 251, 248);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradBegin] = Color.FromArgb(220, 236, 254);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrToastGradEnd] = Color.FromArgb(167, 197, 238);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrInnerDocked] = Color.FromArgb(185, 212, 249);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterDocked] = Color.FromArgb(196, 218, 250);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBdrOuterFloating] = Color.FromArgb(42, 102, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPBkgd] = Color.FromArgb(221, 236, 254);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdr] = Color.FromArgb(127, 157, 185);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDefault] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBdrDisabled] = Color.FromArgb(128, 128, 128);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgd] = Color.FromArgb(169, 199, 240);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlBkgdDisabled] = Color.FromArgb(222, 222, 222);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextDisabled] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPCtlTextMouseDown] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPGroupline] = Color.FromArgb(123, 164, 224);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipBkgd] = Color.FromArgb(255, 255, 204);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPInfoTipText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPNavBarBkgnd] = Color.FromArgb(74, 122, 201);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPText] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTextDisabled] = Color.FromArgb(172, 168, 153);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdActive] = Color.FromArgb(123, 164, 224);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleBkgdInactive] = Color.FromArgb(148, 187, 239);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextActive] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrWPTitleTextInactive] = Color.FromArgb(0, 0, 0);
			rgbTable[ProfessionalColorTable.KnownColors.msocbvcrXLFormulaBarBkgd] = Color.FromArgb(158, 190, 245);
		}

		// Token: 0x04003149 RID: 12617
		private const string oliveColorScheme = "HomeStead";

		// Token: 0x0400314A RID: 12618
		private const string normalColorScheme = "NormalColor";

		// Token: 0x0400314B RID: 12619
		private const string silverColorScheme = "Metallic";

		// Token: 0x0400314C RID: 12620
		private const string royaleColorScheme = "Royale";

		// Token: 0x0400314D RID: 12621
		private const string lunaFileName = "luna.msstyles";

		// Token: 0x0400314E RID: 12622
		private const string royaleFileName = "royale.msstyles";

		// Token: 0x0400314F RID: 12623
		private const string aeroFileName = "aero.msstyles";

		// Token: 0x04003150 RID: 12624
		private Dictionary<ProfessionalColorTable.KnownColors, Color> professionalRGB;

		// Token: 0x04003151 RID: 12625
		private bool usingSystemColors;

		// Token: 0x04003152 RID: 12626
		private bool useSystemColors;

		// Token: 0x04003153 RID: 12627
		private string lastKnownColorScheme = string.Empty;

		// Token: 0x04003154 RID: 12628
		private object colorFreshnessKey;

		// Token: 0x020005BE RID: 1470
		internal enum KnownColors
		{
			// Token: 0x04003156 RID: 12630
			msocbvcrCBBdrOuterDocked,
			// Token: 0x04003157 RID: 12631
			msocbvcrCBBdrOuterFloating,
			// Token: 0x04003158 RID: 12632
			msocbvcrCBBkgd,
			// Token: 0x04003159 RID: 12633
			msocbvcrCBCtlBdrMouseDown,
			// Token: 0x0400315A RID: 12634
			msocbvcrCBCtlBdrMouseOver,
			// Token: 0x0400315B RID: 12635
			msocbvcrCBCtlBdrSelected,
			// Token: 0x0400315C RID: 12636
			msocbvcrCBCtlBdrSelectedMouseOver,
			// Token: 0x0400315D RID: 12637
			msocbvcrCBCtlBkgd,
			// Token: 0x0400315E RID: 12638
			msocbvcrCBCtlBkgdLight,
			// Token: 0x0400315F RID: 12639
			msocbvcrCBCtlBkgdMouseDown,
			// Token: 0x04003160 RID: 12640
			msocbvcrCBCtlBkgdMouseOver,
			// Token: 0x04003161 RID: 12641
			msocbvcrCBCtlBkgdSelected,
			// Token: 0x04003162 RID: 12642
			msocbvcrCBCtlBkgdSelectedMouseOver,
			// Token: 0x04003163 RID: 12643
			msocbvcrCBCtlText,
			// Token: 0x04003164 RID: 12644
			msocbvcrCBCtlTextDisabled,
			// Token: 0x04003165 RID: 12645
			msocbvcrCBCtlTextLight,
			// Token: 0x04003166 RID: 12646
			msocbvcrCBCtlTextMouseDown,
			// Token: 0x04003167 RID: 12647
			msocbvcrCBCtlTextMouseOver,
			// Token: 0x04003168 RID: 12648
			msocbvcrCBDockSeparatorLine,
			// Token: 0x04003169 RID: 12649
			msocbvcrCBDragHandle,
			// Token: 0x0400316A RID: 12650
			msocbvcrCBDragHandleShadow,
			// Token: 0x0400316B RID: 12651
			msocbvcrCBDropDownArrow,
			// Token: 0x0400316C RID: 12652
			msocbvcrCBGradMainMenuHorzBegin,
			// Token: 0x0400316D RID: 12653
			msocbvcrCBGradMainMenuHorzEnd,
			// Token: 0x0400316E RID: 12654
			msocbvcrCBGradMenuIconBkgdDroppedBegin,
			// Token: 0x0400316F RID: 12655
			msocbvcrCBGradMenuIconBkgdDroppedEnd,
			// Token: 0x04003170 RID: 12656
			msocbvcrCBGradMenuIconBkgdDroppedMiddle,
			// Token: 0x04003171 RID: 12657
			msocbvcrCBGradMenuTitleBkgdBegin,
			// Token: 0x04003172 RID: 12658
			msocbvcrCBGradMenuTitleBkgdEnd,
			// Token: 0x04003173 RID: 12659
			msocbvcrCBGradMouseDownBegin,
			// Token: 0x04003174 RID: 12660
			msocbvcrCBGradMouseDownEnd,
			// Token: 0x04003175 RID: 12661
			msocbvcrCBGradMouseDownMiddle,
			// Token: 0x04003176 RID: 12662
			msocbvcrCBGradMouseOverBegin,
			// Token: 0x04003177 RID: 12663
			msocbvcrCBGradMouseOverEnd,
			// Token: 0x04003178 RID: 12664
			msocbvcrCBGradMouseOverMiddle,
			// Token: 0x04003179 RID: 12665
			msocbvcrCBGradOptionsBegin,
			// Token: 0x0400317A RID: 12666
			msocbvcrCBGradOptionsEnd,
			// Token: 0x0400317B RID: 12667
			msocbvcrCBGradOptionsMiddle,
			// Token: 0x0400317C RID: 12668
			msocbvcrCBGradOptionsMouseOverBegin,
			// Token: 0x0400317D RID: 12669
			msocbvcrCBGradOptionsMouseOverEnd,
			// Token: 0x0400317E RID: 12670
			msocbvcrCBGradOptionsMouseOverMiddle,
			// Token: 0x0400317F RID: 12671
			msocbvcrCBGradOptionsSelectedBegin,
			// Token: 0x04003180 RID: 12672
			msocbvcrCBGradOptionsSelectedEnd,
			// Token: 0x04003181 RID: 12673
			msocbvcrCBGradOptionsSelectedMiddle,
			// Token: 0x04003182 RID: 12674
			msocbvcrCBGradSelectedBegin,
			// Token: 0x04003183 RID: 12675
			msocbvcrCBGradSelectedEnd,
			// Token: 0x04003184 RID: 12676
			msocbvcrCBGradSelectedMiddle,
			// Token: 0x04003185 RID: 12677
			msocbvcrCBGradVertBegin,
			// Token: 0x04003186 RID: 12678
			msocbvcrCBGradVertEnd,
			// Token: 0x04003187 RID: 12679
			msocbvcrCBGradVertMiddle,
			// Token: 0x04003188 RID: 12680
			msocbvcrCBIconDisabledDark,
			// Token: 0x04003189 RID: 12681
			msocbvcrCBIconDisabledLight,
			// Token: 0x0400318A RID: 12682
			msocbvcrCBLabelBkgnd,
			// Token: 0x0400318B RID: 12683
			msocbvcrCBLowColorIconDisabled,
			// Token: 0x0400318C RID: 12684
			msocbvcrCBMainMenuBkgd,
			// Token: 0x0400318D RID: 12685
			msocbvcrCBMenuBdrOuter,
			// Token: 0x0400318E RID: 12686
			msocbvcrCBMenuBkgd,
			// Token: 0x0400318F RID: 12687
			msocbvcrCBMenuCtlText,
			// Token: 0x04003190 RID: 12688
			msocbvcrCBMenuCtlTextDisabled,
			// Token: 0x04003191 RID: 12689
			msocbvcrCBMenuIconBkgd,
			// Token: 0x04003192 RID: 12690
			msocbvcrCBMenuIconBkgdDropped,
			// Token: 0x04003193 RID: 12691
			msocbvcrCBMenuShadow,
			// Token: 0x04003194 RID: 12692
			msocbvcrCBMenuSplitArrow,
			// Token: 0x04003195 RID: 12693
			msocbvcrCBOptionsButtonShadow,
			// Token: 0x04003196 RID: 12694
			msocbvcrCBShadow,
			// Token: 0x04003197 RID: 12695
			msocbvcrCBSplitterLine,
			// Token: 0x04003198 RID: 12696
			msocbvcrCBSplitterLineLight,
			// Token: 0x04003199 RID: 12697
			msocbvcrCBTearOffHandle,
			// Token: 0x0400319A RID: 12698
			msocbvcrCBTearOffHandleMouseOver,
			// Token: 0x0400319B RID: 12699
			msocbvcrCBTitleBkgd,
			// Token: 0x0400319C RID: 12700
			msocbvcrCBTitleText,
			// Token: 0x0400319D RID: 12701
			msocbvcrDisabledFocuslessHighlightedText,
			// Token: 0x0400319E RID: 12702
			msocbvcrDisabledHighlightedText,
			// Token: 0x0400319F RID: 12703
			msocbvcrDlgGroupBoxText,
			// Token: 0x040031A0 RID: 12704
			msocbvcrDocTabBdr,
			// Token: 0x040031A1 RID: 12705
			msocbvcrDocTabBdrDark,
			// Token: 0x040031A2 RID: 12706
			msocbvcrDocTabBdrDarkMouseDown,
			// Token: 0x040031A3 RID: 12707
			msocbvcrDocTabBdrDarkMouseOver,
			// Token: 0x040031A4 RID: 12708
			msocbvcrDocTabBdrLight,
			// Token: 0x040031A5 RID: 12709
			msocbvcrDocTabBdrLightMouseDown,
			// Token: 0x040031A6 RID: 12710
			msocbvcrDocTabBdrLightMouseOver,
			// Token: 0x040031A7 RID: 12711
			msocbvcrDocTabBdrMouseDown,
			// Token: 0x040031A8 RID: 12712
			msocbvcrDocTabBdrMouseOver,
			// Token: 0x040031A9 RID: 12713
			msocbvcrDocTabBdrSelected,
			// Token: 0x040031AA RID: 12714
			msocbvcrDocTabBkgd,
			// Token: 0x040031AB RID: 12715
			msocbvcrDocTabBkgdMouseDown,
			// Token: 0x040031AC RID: 12716
			msocbvcrDocTabBkgdMouseOver,
			// Token: 0x040031AD RID: 12717
			msocbvcrDocTabBkgdSelected,
			// Token: 0x040031AE RID: 12718
			msocbvcrDocTabText,
			// Token: 0x040031AF RID: 12719
			msocbvcrDocTabTextMouseDown,
			// Token: 0x040031B0 RID: 12720
			msocbvcrDocTabTextMouseOver,
			// Token: 0x040031B1 RID: 12721
			msocbvcrDocTabTextSelected,
			// Token: 0x040031B2 RID: 12722
			msocbvcrDWActiveTabBkgd,
			// Token: 0x040031B3 RID: 12723
			msocbvcrDWActiveTabText,
			// Token: 0x040031B4 RID: 12724
			msocbvcrDWActiveTabTextDisabled,
			// Token: 0x040031B5 RID: 12725
			msocbvcrDWInactiveTabBkgd,
			// Token: 0x040031B6 RID: 12726
			msocbvcrDWInactiveTabText,
			// Token: 0x040031B7 RID: 12727
			msocbvcrDWTabBkgdMouseDown,
			// Token: 0x040031B8 RID: 12728
			msocbvcrDWTabBkgdMouseOver,
			// Token: 0x040031B9 RID: 12729
			msocbvcrDWTabTextMouseDown,
			// Token: 0x040031BA RID: 12730
			msocbvcrDWTabTextMouseOver,
			// Token: 0x040031BB RID: 12731
			msocbvcrFocuslessHighlightedBkgd,
			// Token: 0x040031BC RID: 12732
			msocbvcrFocuslessHighlightedText,
			// Token: 0x040031BD RID: 12733
			msocbvcrGDHeaderBdr,
			// Token: 0x040031BE RID: 12734
			msocbvcrGDHeaderBkgd,
			// Token: 0x040031BF RID: 12735
			msocbvcrGDHeaderCellBdr,
			// Token: 0x040031C0 RID: 12736
			msocbvcrGDHeaderCellBkgd,
			// Token: 0x040031C1 RID: 12737
			msocbvcrGDHeaderCellBkgdSelected,
			// Token: 0x040031C2 RID: 12738
			msocbvcrGDHeaderSeeThroughSelection,
			// Token: 0x040031C3 RID: 12739
			msocbvcrGSPDarkBkgd,
			// Token: 0x040031C4 RID: 12740
			msocbvcrGSPGroupContentDarkBkgd,
			// Token: 0x040031C5 RID: 12741
			msocbvcrGSPGroupContentLightBkgd,
			// Token: 0x040031C6 RID: 12742
			msocbvcrGSPGroupContentText,
			// Token: 0x040031C7 RID: 12743
			msocbvcrGSPGroupContentTextDisabled,
			// Token: 0x040031C8 RID: 12744
			msocbvcrGSPGroupHeaderDarkBkgd,
			// Token: 0x040031C9 RID: 12745
			msocbvcrGSPGroupHeaderLightBkgd,
			// Token: 0x040031CA RID: 12746
			msocbvcrGSPGroupHeaderText,
			// Token: 0x040031CB RID: 12747
			msocbvcrGSPGroupline,
			// Token: 0x040031CC RID: 12748
			msocbvcrGSPHyperlink,
			// Token: 0x040031CD RID: 12749
			msocbvcrGSPLightBkgd,
			// Token: 0x040031CE RID: 12750
			msocbvcrHyperlink,
			// Token: 0x040031CF RID: 12751
			msocbvcrHyperlinkFollowed,
			// Token: 0x040031D0 RID: 12752
			msocbvcrJotNavUIBdr,
			// Token: 0x040031D1 RID: 12753
			msocbvcrJotNavUIGradBegin,
			// Token: 0x040031D2 RID: 12754
			msocbvcrJotNavUIGradEnd,
			// Token: 0x040031D3 RID: 12755
			msocbvcrJotNavUIGradMiddle,
			// Token: 0x040031D4 RID: 12756
			msocbvcrJotNavUIText,
			// Token: 0x040031D5 RID: 12757
			msocbvcrListHeaderArrow,
			// Token: 0x040031D6 RID: 12758
			msocbvcrNetLookBkgnd,
			// Token: 0x040031D7 RID: 12759
			msocbvcrOABBkgd,
			// Token: 0x040031D8 RID: 12760
			msocbvcrOBBkgdBdr,
			// Token: 0x040031D9 RID: 12761
			msocbvcrOBBkgdBdrContrast,
			// Token: 0x040031DA RID: 12762
			msocbvcrOGMDIParentWorkspaceBkgd,
			// Token: 0x040031DB RID: 12763
			msocbvcrOGRulerActiveBkgd,
			// Token: 0x040031DC RID: 12764
			msocbvcrOGRulerBdr,
			// Token: 0x040031DD RID: 12765
			msocbvcrOGRulerBkgd,
			// Token: 0x040031DE RID: 12766
			msocbvcrOGRulerInactiveBkgd,
			// Token: 0x040031DF RID: 12767
			msocbvcrOGRulerTabBoxBdr,
			// Token: 0x040031E0 RID: 12768
			msocbvcrOGRulerTabBoxBdrHighlight,
			// Token: 0x040031E1 RID: 12769
			msocbvcrOGRulerTabStopTicks,
			// Token: 0x040031E2 RID: 12770
			msocbvcrOGRulerText,
			// Token: 0x040031E3 RID: 12771
			msocbvcrOGTaskPaneGroupBoxHeaderBkgd,
			// Token: 0x040031E4 RID: 12772
			msocbvcrOGWorkspaceBkgd,
			// Token: 0x040031E5 RID: 12773
			msocbvcrOLKFlagNone,
			// Token: 0x040031E6 RID: 12774
			msocbvcrOLKFolderbarDark,
			// Token: 0x040031E7 RID: 12775
			msocbvcrOLKFolderbarLight,
			// Token: 0x040031E8 RID: 12776
			msocbvcrOLKFolderbarText,
			// Token: 0x040031E9 RID: 12777
			msocbvcrOLKGridlines,
			// Token: 0x040031EA RID: 12778
			msocbvcrOLKGroupLine,
			// Token: 0x040031EB RID: 12779
			msocbvcrOLKGroupNested,
			// Token: 0x040031EC RID: 12780
			msocbvcrOLKGroupShaded,
			// Token: 0x040031ED RID: 12781
			msocbvcrOLKGroupText,
			// Token: 0x040031EE RID: 12782
			msocbvcrOLKIconBar,
			// Token: 0x040031EF RID: 12783
			msocbvcrOLKInfoBarBkgd,
			// Token: 0x040031F0 RID: 12784
			msocbvcrOLKInfoBarText,
			// Token: 0x040031F1 RID: 12785
			msocbvcrOLKPreviewPaneLabelText,
			// Token: 0x040031F2 RID: 12786
			msocbvcrOLKTodayIndicatorDark,
			// Token: 0x040031F3 RID: 12787
			msocbvcrOLKTodayIndicatorLight,
			// Token: 0x040031F4 RID: 12788
			msocbvcrOLKWBActionDividerLine,
			// Token: 0x040031F5 RID: 12789
			msocbvcrOLKWBButtonDark,
			// Token: 0x040031F6 RID: 12790
			msocbvcrOLKWBButtonLight,
			// Token: 0x040031F7 RID: 12791
			msocbvcrOLKWBDarkOutline,
			// Token: 0x040031F8 RID: 12792
			msocbvcrOLKWBFoldersBackground,
			// Token: 0x040031F9 RID: 12793
			msocbvcrOLKWBHoverButtonDark,
			// Token: 0x040031FA RID: 12794
			msocbvcrOLKWBHoverButtonLight,
			// Token: 0x040031FB RID: 12795
			msocbvcrOLKWBLabelText,
			// Token: 0x040031FC RID: 12796
			msocbvcrOLKWBPressedButtonDark,
			// Token: 0x040031FD RID: 12797
			msocbvcrOLKWBPressedButtonLight,
			// Token: 0x040031FE RID: 12798
			msocbvcrOLKWBSelectedButtonDark,
			// Token: 0x040031FF RID: 12799
			msocbvcrOLKWBSelectedButtonLight,
			// Token: 0x04003200 RID: 12800
			msocbvcrOLKWBSplitterDark,
			// Token: 0x04003201 RID: 12801
			msocbvcrOLKWBSplitterLight,
			// Token: 0x04003202 RID: 12802
			msocbvcrPlacesBarBkgd,
			// Token: 0x04003203 RID: 12803
			msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd,
			// Token: 0x04003204 RID: 12804
			msocbvcrPPOutlineThumbnailsPaneTabBdr,
			// Token: 0x04003205 RID: 12805
			msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd,
			// Token: 0x04003206 RID: 12806
			msocbvcrPPOutlineThumbnailsPaneTabText,
			// Token: 0x04003207 RID: 12807
			msocbvcrPPSlideBdrActiveSelected,
			// Token: 0x04003208 RID: 12808
			msocbvcrPPSlideBdrActiveSelectedMouseOver,
			// Token: 0x04003209 RID: 12809
			msocbvcrPPSlideBdrInactiveSelected,
			// Token: 0x0400320A RID: 12810
			msocbvcrPPSlideBdrMouseOver,
			// Token: 0x0400320B RID: 12811
			msocbvcrPubPrintDocScratchPageBkgd,
			// Token: 0x0400320C RID: 12812
			msocbvcrPubWebDocScratchPageBkgd,
			// Token: 0x0400320D RID: 12813
			msocbvcrSBBdr,
			// Token: 0x0400320E RID: 12814
			msocbvcrScrollbarBkgd,
			// Token: 0x0400320F RID: 12815
			msocbvcrToastGradBegin,
			// Token: 0x04003210 RID: 12816
			msocbvcrToastGradEnd,
			// Token: 0x04003211 RID: 12817
			msocbvcrWPBdrInnerDocked,
			// Token: 0x04003212 RID: 12818
			msocbvcrWPBdrOuterDocked,
			// Token: 0x04003213 RID: 12819
			msocbvcrWPBdrOuterFloating,
			// Token: 0x04003214 RID: 12820
			msocbvcrWPBkgd,
			// Token: 0x04003215 RID: 12821
			msocbvcrWPCtlBdr,
			// Token: 0x04003216 RID: 12822
			msocbvcrWPCtlBdrDefault,
			// Token: 0x04003217 RID: 12823
			msocbvcrWPCtlBdrDisabled,
			// Token: 0x04003218 RID: 12824
			msocbvcrWPCtlBkgd,
			// Token: 0x04003219 RID: 12825
			msocbvcrWPCtlBkgdDisabled,
			// Token: 0x0400321A RID: 12826
			msocbvcrWPCtlText,
			// Token: 0x0400321B RID: 12827
			msocbvcrWPCtlTextDisabled,
			// Token: 0x0400321C RID: 12828
			msocbvcrWPCtlTextMouseDown,
			// Token: 0x0400321D RID: 12829
			msocbvcrWPGroupline,
			// Token: 0x0400321E RID: 12830
			msocbvcrWPInfoTipBkgd,
			// Token: 0x0400321F RID: 12831
			msocbvcrWPInfoTipText,
			// Token: 0x04003220 RID: 12832
			msocbvcrWPNavBarBkgnd,
			// Token: 0x04003221 RID: 12833
			msocbvcrWPText,
			// Token: 0x04003222 RID: 12834
			msocbvcrWPTextDisabled,
			// Token: 0x04003223 RID: 12835
			msocbvcrWPTitleBkgdActive,
			// Token: 0x04003224 RID: 12836
			msocbvcrWPTitleBkgdInactive,
			// Token: 0x04003225 RID: 12837
			msocbvcrWPTitleTextActive,
			// Token: 0x04003226 RID: 12838
			msocbvcrWPTitleTextInactive,
			// Token: 0x04003227 RID: 12839
			msocbvcrXLFormulaBarBkgd,
			// Token: 0x04003228 RID: 12840
			ButtonSelectedHighlight,
			// Token: 0x04003229 RID: 12841
			ButtonPressedHighlight,
			// Token: 0x0400322A RID: 12842
			ButtonCheckedHighlight,
			// Token: 0x0400322B RID: 12843
			lastKnownColor = 212
		}
	}
}

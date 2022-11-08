using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x020005BC RID: 1468
	public sealed class ProfessionalColors
	{
		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x06004BF9 RID: 19449 RVA: 0x00112636 File Offset: 0x00111636
		internal static ProfessionalColorTable ColorTable
		{
			get
			{
				if (ProfessionalColors.professionalColorTable == null)
				{
					ProfessionalColors.professionalColorTable = new ProfessionalColorTable();
				}
				return ProfessionalColors.professionalColorTable;
			}
		}

		// Token: 0x06004BFA RID: 19450 RVA: 0x0011264E File Offset: 0x0011164E
		static ProfessionalColors()
		{
			SystemEvents.UserPreferenceChanged += ProfessionalColors.OnUserPreferenceChanged;
			ProfessionalColors.SetScheme();
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x00112666 File Offset: 0x00111666
		private ProfessionalColors()
		{
		}

		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x06004BFC RID: 19452 RVA: 0x0011266E File Offset: 0x0011166E
		internal static string ColorScheme
		{
			get
			{
				return ProfessionalColors.colorScheme;
			}
		}

		// Token: 0x17000F0C RID: 3852
		// (get) Token: 0x06004BFD RID: 19453 RVA: 0x00112675 File Offset: 0x00111675
		internal static object ColorFreshnessKey
		{
			get
			{
				return ProfessionalColors.colorFreshnessKey;
			}
		}

		// Token: 0x17000F0D RID: 3853
		// (get) Token: 0x06004BFE RID: 19454 RVA: 0x0011267C File Offset: 0x0011167C
		[SRDescription("ProfessionalColorsButtonSelectedHighlightDescr")]
		public static Color ButtonSelectedHighlight
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonSelectedHighlight;
			}
		}

		// Token: 0x17000F0E RID: 3854
		// (get) Token: 0x06004BFF RID: 19455 RVA: 0x00112688 File Offset: 0x00111688
		[SRDescription("ProfessionalColorsButtonSelectedHighlightBorderDescr")]
		public static Color ButtonSelectedHighlightBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonSelectedHighlightBorder;
			}
		}

		// Token: 0x17000F0F RID: 3855
		// (get) Token: 0x06004C00 RID: 19456 RVA: 0x00112694 File Offset: 0x00111694
		[SRDescription("ProfessionalColorsButtonPressedHighlightDescr")]
		public static Color ButtonPressedHighlight
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonPressedHighlight;
			}
		}

		// Token: 0x17000F10 RID: 3856
		// (get) Token: 0x06004C01 RID: 19457 RVA: 0x001126A0 File Offset: 0x001116A0
		[SRDescription("ProfessionalColorsButtonPressedHighlightBorderDescr")]
		public static Color ButtonPressedHighlightBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonPressedHighlightBorder;
			}
		}

		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x06004C02 RID: 19458 RVA: 0x001126AC File Offset: 0x001116AC
		[SRDescription("ProfessionalColorsButtonCheckedHighlightDescr")]
		public static Color ButtonCheckedHighlight
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonCheckedHighlight;
			}
		}

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x06004C03 RID: 19459 RVA: 0x001126B8 File Offset: 0x001116B8
		[SRDescription("ProfessionalColorsButtonCheckedHighlightBorderDescr")]
		public static Color ButtonCheckedHighlightBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonCheckedHighlightBorder;
			}
		}

		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x06004C04 RID: 19460 RVA: 0x001126C4 File Offset: 0x001116C4
		[SRDescription("ProfessionalColorsButtonPressedBorderDescr")]
		public static Color ButtonPressedBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonPressedBorder;
			}
		}

		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x06004C05 RID: 19461 RVA: 0x001126D0 File Offset: 0x001116D0
		[SRDescription("ProfessionalColorsButtonSelectedBorderDescr")]
		public static Color ButtonSelectedBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonCheckedGradientBegin;
			}
		}

		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x06004C06 RID: 19462 RVA: 0x001126DC File Offset: 0x001116DC
		[SRDescription("ProfessionalColorsButtonCheckedGradientBeginDescr")]
		public static Color ButtonCheckedGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonCheckedGradientBegin;
			}
		}

		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x06004C07 RID: 19463 RVA: 0x001126E8 File Offset: 0x001116E8
		[SRDescription("ProfessionalColorsButtonCheckedGradientMiddleDescr")]
		public static Color ButtonCheckedGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonCheckedGradientMiddle;
			}
		}

		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x06004C08 RID: 19464 RVA: 0x001126F4 File Offset: 0x001116F4
		[SRDescription("ProfessionalColorsButtonCheckedGradientEndDescr")]
		public static Color ButtonCheckedGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonCheckedGradientEnd;
			}
		}

		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x06004C09 RID: 19465 RVA: 0x00112700 File Offset: 0x00111700
		[SRDescription("ProfessionalColorsButtonSelectedGradientBeginDescr")]
		public static Color ButtonSelectedGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonSelectedGradientBegin;
			}
		}

		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x06004C0A RID: 19466 RVA: 0x0011270C File Offset: 0x0011170C
		[SRDescription("ProfessionalColorsButtonSelectedGradientMiddleDescr")]
		public static Color ButtonSelectedGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonSelectedGradientMiddle;
			}
		}

		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x06004C0B RID: 19467 RVA: 0x00112718 File Offset: 0x00111718
		[SRDescription("ProfessionalColorsButtonSelectedGradientEndDescr")]
		public static Color ButtonSelectedGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonSelectedGradientEnd;
			}
		}

		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x06004C0C RID: 19468 RVA: 0x00112724 File Offset: 0x00111724
		[SRDescription("ProfessionalColorsButtonPressedGradientBeginDescr")]
		public static Color ButtonPressedGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonPressedGradientBegin;
			}
		}

		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x06004C0D RID: 19469 RVA: 0x00112730 File Offset: 0x00111730
		[SRDescription("ProfessionalColorsButtonPressedGradientMiddleDescr")]
		public static Color ButtonPressedGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonPressedGradientMiddle;
			}
		}

		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x06004C0E RID: 19470 RVA: 0x0011273C File Offset: 0x0011173C
		[SRDescription("ProfessionalColorsButtonPressedGradientEndDescr")]
		public static Color ButtonPressedGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ButtonPressedGradientEnd;
			}
		}

		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x06004C0F RID: 19471 RVA: 0x00112748 File Offset: 0x00111748
		[SRDescription("ProfessionalColorsCheckBackgroundDescr")]
		public static Color CheckBackground
		{
			get
			{
				return ProfessionalColors.ColorTable.CheckBackground;
			}
		}

		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x06004C10 RID: 19472 RVA: 0x00112754 File Offset: 0x00111754
		[SRDescription("ProfessionalColorsCheckSelectedBackgroundDescr")]
		public static Color CheckSelectedBackground
		{
			get
			{
				return ProfessionalColors.ColorTable.CheckSelectedBackground;
			}
		}

		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x06004C11 RID: 19473 RVA: 0x00112760 File Offset: 0x00111760
		[SRDescription("ProfessionalColorsCheckPressedBackgroundDescr")]
		public static Color CheckPressedBackground
		{
			get
			{
				return ProfessionalColors.ColorTable.CheckPressedBackground;
			}
		}

		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x06004C12 RID: 19474 RVA: 0x0011276C File Offset: 0x0011176C
		[SRDescription("ProfessionalColorsGripDarkDescr")]
		public static Color GripDark
		{
			get
			{
				return ProfessionalColors.ColorTable.GripDark;
			}
		}

		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x06004C13 RID: 19475 RVA: 0x00112778 File Offset: 0x00111778
		[SRDescription("ProfessionalColorsGripLightDescr")]
		public static Color GripLight
		{
			get
			{
				return ProfessionalColors.ColorTable.GripLight;
			}
		}

		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x06004C14 RID: 19476 RVA: 0x00112784 File Offset: 0x00111784
		[SRDescription("ProfessionalColorsImageMarginGradientBeginDescr")]
		public static Color ImageMarginGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ImageMarginGradientBegin;
			}
		}

		// Token: 0x17000F24 RID: 3876
		// (get) Token: 0x06004C15 RID: 19477 RVA: 0x00112790 File Offset: 0x00111790
		[SRDescription("ProfessionalColorsImageMarginGradientMiddleDescr")]
		public static Color ImageMarginGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.ImageMarginGradientMiddle;
			}
		}

		// Token: 0x17000F25 RID: 3877
		// (get) Token: 0x06004C16 RID: 19478 RVA: 0x0011279C File Offset: 0x0011179C
		[SRDescription("ProfessionalColorsImageMarginGradientEndDescr")]
		public static Color ImageMarginGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ImageMarginGradientEnd;
			}
		}

		// Token: 0x17000F26 RID: 3878
		// (get) Token: 0x06004C17 RID: 19479 RVA: 0x001127A8 File Offset: 0x001117A8
		[SRDescription("ProfessionalColorsImageMarginRevealedGradientBeginDescr")]
		public static Color ImageMarginRevealedGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ImageMarginRevealedGradientBegin;
			}
		}

		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x06004C18 RID: 19480 RVA: 0x001127B4 File Offset: 0x001117B4
		[SRDescription("ProfessionalColorsImageMarginRevealedGradientMiddleDescr")]
		public static Color ImageMarginRevealedGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.ImageMarginRevealedGradientMiddle;
			}
		}

		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x06004C19 RID: 19481 RVA: 0x001127C0 File Offset: 0x001117C0
		[SRDescription("ProfessionalColorsImageMarginRevealedGradientEndDescr")]
		public static Color ImageMarginRevealedGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ImageMarginRevealedGradientEnd;
			}
		}

		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x06004C1A RID: 19482 RVA: 0x001127CC File Offset: 0x001117CC
		[SRDescription("ProfessionalColorsMenuStripGradientBeginDescr")]
		public static Color MenuStripGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuStripGradientBegin;
			}
		}

		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x06004C1B RID: 19483 RVA: 0x001127D8 File Offset: 0x001117D8
		[SRDescription("ProfessionalColorsMenuStripGradientEndDescr")]
		public static Color MenuStripGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuStripGradientEnd;
			}
		}

		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x06004C1C RID: 19484 RVA: 0x001127E4 File Offset: 0x001117E4
		[SRDescription("ProfessionalColorsMenuBorderDescr")]
		public static Color MenuBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuBorder;
			}
		}

		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x06004C1D RID: 19485 RVA: 0x001127F0 File Offset: 0x001117F0
		[SRDescription("ProfessionalColorsMenuItemSelectedDescr")]
		public static Color MenuItemSelected
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuItemBorder;
			}
		}

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x06004C1E RID: 19486 RVA: 0x001127FC File Offset: 0x001117FC
		[SRDescription("ProfessionalColorsMenuItemBorderDescr")]
		public static Color MenuItemBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuItemBorder;
			}
		}

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x06004C1F RID: 19487 RVA: 0x00112808 File Offset: 0x00111808
		[SRDescription("ProfessionalColorsMenuItemSelectedGradientBeginDescr")]
		public static Color MenuItemSelectedGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuItemSelectedGradientBegin;
			}
		}

		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x06004C20 RID: 19488 RVA: 0x00112814 File Offset: 0x00111814
		[SRDescription("ProfessionalColorsMenuItemSelectedGradientEndDescr")]
		public static Color MenuItemSelectedGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuItemSelectedGradientEnd;
			}
		}

		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x06004C21 RID: 19489 RVA: 0x00112820 File Offset: 0x00111820
		[SRDescription("ProfessionalColorsMenuItemPressedGradientBeginDescr")]
		public static Color MenuItemPressedGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuItemPressedGradientBegin;
			}
		}

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x06004C22 RID: 19490 RVA: 0x0011282C File Offset: 0x0011182C
		[SRDescription("ProfessionalColorsMenuItemPressedGradientMiddleDescr")]
		public static Color MenuItemPressedGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuItemPressedGradientMiddle;
			}
		}

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x06004C23 RID: 19491 RVA: 0x00112838 File Offset: 0x00111838
		[SRDescription("ProfessionalColorsMenuItemPressedGradientEndDescr")]
		public static Color MenuItemPressedGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.MenuItemPressedGradientEnd;
			}
		}

		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x06004C24 RID: 19492 RVA: 0x00112844 File Offset: 0x00111844
		[SRDescription("ProfessionalColorsRaftingContainerGradientBeginDescr")]
		public static Color RaftingContainerGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.RaftingContainerGradientBegin;
			}
		}

		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x06004C25 RID: 19493 RVA: 0x00112850 File Offset: 0x00111850
		[SRDescription("ProfessionalColorsRaftingContainerGradientEndDescr")]
		public static Color RaftingContainerGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.RaftingContainerGradientEnd;
			}
		}

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x06004C26 RID: 19494 RVA: 0x0011285C File Offset: 0x0011185C
		[SRDescription("ProfessionalColorsSeparatorDarkDescr")]
		public static Color SeparatorDark
		{
			get
			{
				return ProfessionalColors.ColorTable.SeparatorDark;
			}
		}

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x06004C27 RID: 19495 RVA: 0x00112868 File Offset: 0x00111868
		[SRDescription("ProfessionalColorsSeparatorLightDescr")]
		public static Color SeparatorLight
		{
			get
			{
				return ProfessionalColors.ColorTable.SeparatorLight;
			}
		}

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x06004C28 RID: 19496 RVA: 0x00112874 File Offset: 0x00111874
		[SRDescription("ProfessionalColorsStatusStripGradientBeginDescr")]
		public static Color StatusStripGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.StatusStripGradientBegin;
			}
		}

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x06004C29 RID: 19497 RVA: 0x00112880 File Offset: 0x00111880
		[SRDescription("ProfessionalColorsStatusStripGradientEndDescr")]
		public static Color StatusStripGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.StatusStripGradientEnd;
			}
		}

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x06004C2A RID: 19498 RVA: 0x0011288C File Offset: 0x0011188C
		[SRDescription("ProfessionalColorsToolStripBorderDescr")]
		public static Color ToolStripBorder
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripBorder;
			}
		}

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x06004C2B RID: 19499 RVA: 0x00112898 File Offset: 0x00111898
		[SRDescription("ProfessionalColorsToolStripDropDownBackgroundDescr")]
		public static Color ToolStripDropDownBackground
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripDropDownBackground;
			}
		}

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x06004C2C RID: 19500 RVA: 0x001128A4 File Offset: 0x001118A4
		[SRDescription("ProfessionalColorsToolStripGradientBeginDescr")]
		public static Color ToolStripGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripGradientBegin;
			}
		}

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x06004C2D RID: 19501 RVA: 0x001128B0 File Offset: 0x001118B0
		[SRDescription("ProfessionalColorsToolStripGradientMiddleDescr")]
		public static Color ToolStripGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripGradientMiddle;
			}
		}

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x06004C2E RID: 19502 RVA: 0x001128BC File Offset: 0x001118BC
		[SRDescription("ProfessionalColorsToolStripGradientEndDescr")]
		public static Color ToolStripGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripGradientEnd;
			}
		}

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x06004C2F RID: 19503 RVA: 0x001128C8 File Offset: 0x001118C8
		[SRDescription("ProfessionalColorsToolStripContentPanelGradientBeginDescr")]
		public static Color ToolStripContentPanelGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripContentPanelGradientBegin;
			}
		}

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x06004C30 RID: 19504 RVA: 0x001128D4 File Offset: 0x001118D4
		[SRDescription("ProfessionalColorsToolStripContentPanelGradientEndDescr")]
		public static Color ToolStripContentPanelGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripContentPanelGradientEnd;
			}
		}

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x06004C31 RID: 19505 RVA: 0x001128E0 File Offset: 0x001118E0
		[SRDescription("ProfessionalColorsToolStripPanelGradientBeginDescr")]
		public static Color ToolStripPanelGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripPanelGradientBegin;
			}
		}

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x06004C32 RID: 19506 RVA: 0x001128EC File Offset: 0x001118EC
		[SRDescription("ProfessionalColorsToolStripPanelGradientEndDescr")]
		public static Color ToolStripPanelGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.ToolStripPanelGradientEnd;
			}
		}

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x06004C33 RID: 19507 RVA: 0x001128F8 File Offset: 0x001118F8
		[SRDescription("ProfessionalColorsOverflowButtonGradientBeginDescr")]
		public static Color OverflowButtonGradientBegin
		{
			get
			{
				return ProfessionalColors.ColorTable.OverflowButtonGradientBegin;
			}
		}

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06004C34 RID: 19508 RVA: 0x00112904 File Offset: 0x00111904
		[SRDescription("ProfessionalColorsOverflowButtonGradientMiddleDescr")]
		public static Color OverflowButtonGradientMiddle
		{
			get
			{
				return ProfessionalColors.ColorTable.OverflowButtonGradientMiddle;
			}
		}

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06004C35 RID: 19509 RVA: 0x00112910 File Offset: 0x00111910
		[SRDescription("ProfessionalColorsOverflowButtonGradientEndDescr")]
		public static Color OverflowButtonGradientEnd
		{
			get
			{
				return ProfessionalColors.ColorTable.OverflowButtonGradientEnd;
			}
		}

		// Token: 0x06004C36 RID: 19510 RVA: 0x0011291C File Offset: 0x0011191C
		private static void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			ProfessionalColors.SetScheme();
			if (e.Category == UserPreferenceCategory.Color)
			{
				ProfessionalColors.colorFreshnessKey = new object();
			}
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x00112936 File Offset: 0x00111936
		private static void SetScheme()
		{
			if (VisualStyleRenderer.IsSupported)
			{
				ProfessionalColors.colorScheme = VisualStyleInformation.ColorScheme;
				return;
			}
			ProfessionalColors.colorScheme = null;
		}

		// Token: 0x04003146 RID: 12614
		[ThreadStatic]
		private static ProfessionalColorTable professionalColorTable;

		// Token: 0x04003147 RID: 12615
		[ThreadStatic]
		private static string colorScheme;

		// Token: 0x04003148 RID: 12616
		[ThreadStatic]
		private static object colorFreshnessKey;
	}
}

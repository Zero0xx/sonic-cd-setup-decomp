using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x020005FB RID: 1531
	public sealed class ScrollBarRenderer
	{
		// Token: 0x0600509F RID: 20639 RVA: 0x00126FBE File Offset: 0x00125FBE
		private ScrollBarRenderer()
		{
		}

		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x060050A0 RID: 20640 RVA: 0x00126FC6 File Offset: 0x00125FC6
		public static bool IsSupported
		{
			get
			{
				return VisualStyleRenderer.IsSupported;
			}
		}

		// Token: 0x060050A1 RID: 20641 RVA: 0x00126FCD File Offset: 0x00125FCD
		public static void DrawArrowButton(Graphics g, Rectangle bounds, ScrollBarArrowButtonState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.ArrowButton.LeftNormal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x060050A2 RID: 20642 RVA: 0x00126FE6 File Offset: 0x00125FE6
		public static void DrawHorizontalThumb(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.ThumbButtonHorizontal.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x060050A3 RID: 20643 RVA: 0x00126FFF File Offset: 0x00125FFF
		public static void DrawVerticalThumb(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.ThumbButtonVertical.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x060050A4 RID: 20644 RVA: 0x00127018 File Offset: 0x00126018
		public static void DrawHorizontalThumbGrip(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.GripperHorizontal.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x00127031 File Offset: 0x00126031
		public static void DrawVerticalThumbGrip(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.GripperVertical.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x060050A6 RID: 20646 RVA: 0x0012704A File Offset: 0x0012604A
		public static void DrawRightHorizontalTrack(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.RightTrackHorizontal.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x060050A7 RID: 20647 RVA: 0x00127063 File Offset: 0x00126063
		public static void DrawLeftHorizontalTrack(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.LeftTrackHorizontal.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x060050A8 RID: 20648 RVA: 0x0012707C File Offset: 0x0012607C
		public static void DrawUpperVerticalTrack(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.UpperTrackVertical.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x060050A9 RID: 20649 RVA: 0x00127095 File Offset: 0x00126095
		public static void DrawLowerVerticalTrack(Graphics g, Rectangle bounds, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.LowerTrackVertical.Normal, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x060050AA RID: 20650 RVA: 0x001270AE File Offset: 0x001260AE
		public static void DrawSizeBox(Graphics g, Rectangle bounds, ScrollBarSizeBoxState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.SizeBox.LeftAlign, (int)state);
			ScrollBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x060050AB RID: 20651 RVA: 0x001270C7 File Offset: 0x001260C7
		public static Size GetThumbGripSize(Graphics g, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.GripperHorizontal.Normal, (int)state);
			return ScrollBarRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.True);
		}

		// Token: 0x060050AC RID: 20652 RVA: 0x001270E0 File Offset: 0x001260E0
		public static Size GetSizeBoxSize(Graphics g, ScrollBarState state)
		{
			ScrollBarRenderer.InitializeRenderer(VisualStyleElement.ScrollBar.SizeBox.LeftAlign, (int)state);
			return ScrollBarRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.True);
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x001270F9 File Offset: 0x001260F9
		private static void InitializeRenderer(VisualStyleElement element, int state)
		{
			if (ScrollBarRenderer.visualStyleRenderer == null)
			{
				ScrollBarRenderer.visualStyleRenderer = new VisualStyleRenderer(element.ClassName, element.Part, state);
				return;
			}
			ScrollBarRenderer.visualStyleRenderer.SetParameters(element.ClassName, element.Part, state);
		}

		// Token: 0x040034CD RID: 13517
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer;
	}
}

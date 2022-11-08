using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x020005E0 RID: 1504
	public sealed class RadioButtonRenderer
	{
		// Token: 0x06004E8F RID: 20111 RVA: 0x00121863 File Offset: 0x00120863
		private RadioButtonRenderer()
		{
		}

		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x06004E90 RID: 20112 RVA: 0x0012186B File Offset: 0x0012086B
		// (set) Token: 0x06004E91 RID: 20113 RVA: 0x00121872 File Offset: 0x00120872
		public static bool RenderMatchingApplicationState
		{
			get
			{
				return RadioButtonRenderer.renderMatchingApplicationState;
			}
			set
			{
				RadioButtonRenderer.renderMatchingApplicationState = value;
			}
		}

		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x06004E92 RID: 20114 RVA: 0x0012187A File Offset: 0x0012087A
		private static bool RenderWithVisualStyles
		{
			get
			{
				return !RadioButtonRenderer.renderMatchingApplicationState || Application.RenderWithVisualStyles;
			}
		}

		// Token: 0x06004E93 RID: 20115 RVA: 0x0012188A File Offset: 0x0012088A
		public static bool IsBackgroundPartiallyTransparent(RadioButtonState state)
		{
			if (RadioButtonRenderer.RenderWithVisualStyles)
			{
				RadioButtonRenderer.InitializeRenderer((int)state);
				return RadioButtonRenderer.visualStyleRenderer.IsBackgroundPartiallyTransparent();
			}
			return false;
		}

		// Token: 0x06004E94 RID: 20116 RVA: 0x001218A5 File Offset: 0x001208A5
		public static void DrawParentBackground(Graphics g, Rectangle bounds, Control childControl)
		{
			if (RadioButtonRenderer.RenderWithVisualStyles)
			{
				RadioButtonRenderer.InitializeRenderer(0);
				RadioButtonRenderer.visualStyleRenderer.DrawParentBackground(g, bounds, childControl);
			}
		}

		// Token: 0x06004E95 RID: 20117 RVA: 0x001218C4 File Offset: 0x001208C4
		public static void DrawRadioButton(Graphics g, Point glyphLocation, RadioButtonState state)
		{
			Rectangle rectangle = new Rectangle(glyphLocation, RadioButtonRenderer.GetGlyphSize(g, state));
			if (RadioButtonRenderer.RenderWithVisualStyles)
			{
				RadioButtonRenderer.InitializeRenderer((int)state);
				RadioButtonRenderer.visualStyleRenderer.DrawBackground(g, rectangle);
				return;
			}
			ControlPaint.DrawRadioButton(g, rectangle, RadioButtonRenderer.ConvertToButtonState(state));
		}

		// Token: 0x06004E96 RID: 20118 RVA: 0x00121907 File Offset: 0x00120907
		public static void DrawRadioButton(Graphics g, Point glyphLocation, Rectangle textBounds, string radioButtonText, Font font, bool focused, RadioButtonState state)
		{
			RadioButtonRenderer.DrawRadioButton(g, glyphLocation, textBounds, radioButtonText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, focused, state);
		}

		// Token: 0x06004E97 RID: 20119 RVA: 0x0012191C File Offset: 0x0012091C
		public static void DrawRadioButton(Graphics g, Point glyphLocation, Rectangle textBounds, string radioButtonText, Font font, TextFormatFlags flags, bool focused, RadioButtonState state)
		{
			Rectangle rectangle = new Rectangle(glyphLocation, RadioButtonRenderer.GetGlyphSize(g, state));
			Color foreColor;
			if (RadioButtonRenderer.RenderWithVisualStyles)
			{
				RadioButtonRenderer.InitializeRenderer((int)state);
				RadioButtonRenderer.visualStyleRenderer.DrawBackground(g, rectangle);
				foreColor = RadioButtonRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			}
			else
			{
				ControlPaint.DrawRadioButton(g, rectangle, RadioButtonRenderer.ConvertToButtonState(state));
				foreColor = SystemColors.ControlText;
			}
			TextRenderer.DrawText(g, radioButtonText, font, textBounds, foreColor, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, textBounds);
			}
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x00121994 File Offset: 0x00120994
		public static void DrawRadioButton(Graphics g, Point glyphLocation, Rectangle textBounds, string radioButtonText, Font font, Image image, Rectangle imageBounds, bool focused, RadioButtonState state)
		{
			RadioButtonRenderer.DrawRadioButton(g, glyphLocation, textBounds, radioButtonText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, image, imageBounds, focused, state);
		}

		// Token: 0x06004E99 RID: 20121 RVA: 0x001219B8 File Offset: 0x001209B8
		public static void DrawRadioButton(Graphics g, Point glyphLocation, Rectangle textBounds, string radioButtonText, Font font, TextFormatFlags flags, Image image, Rectangle imageBounds, bool focused, RadioButtonState state)
		{
			Rectangle rectangle = new Rectangle(glyphLocation, RadioButtonRenderer.GetGlyphSize(g, state));
			Color foreColor;
			if (RadioButtonRenderer.RenderWithVisualStyles)
			{
				RadioButtonRenderer.InitializeRenderer((int)state);
				RadioButtonRenderer.visualStyleRenderer.DrawImage(g, imageBounds, image);
				RadioButtonRenderer.visualStyleRenderer.DrawBackground(g, rectangle);
				foreColor = RadioButtonRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			}
			else
			{
				g.DrawImage(image, imageBounds);
				ControlPaint.DrawRadioButton(g, rectangle, RadioButtonRenderer.ConvertToButtonState(state));
				foreColor = SystemColors.ControlText;
			}
			TextRenderer.DrawText(g, radioButtonText, font, textBounds, foreColor, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, textBounds);
			}
		}

		// Token: 0x06004E9A RID: 20122 RVA: 0x00121A46 File Offset: 0x00120A46
		public static Size GetGlyphSize(Graphics g, RadioButtonState state)
		{
			if (RadioButtonRenderer.RenderWithVisualStyles)
			{
				RadioButtonRenderer.InitializeRenderer((int)state);
				return RadioButtonRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.Draw);
			}
			return new Size(13, 13);
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x00121A6C File Offset: 0x00120A6C
		internal static ButtonState ConvertToButtonState(RadioButtonState state)
		{
			switch (state)
			{
			case RadioButtonState.UncheckedPressed:
				return ButtonState.Pushed;
			case RadioButtonState.UncheckedDisabled:
				return ButtonState.Inactive;
			case RadioButtonState.CheckedNormal:
			case RadioButtonState.CheckedHot:
				return ButtonState.Checked;
			case RadioButtonState.CheckedPressed:
				return ButtonState.Checked | ButtonState.Pushed;
			case RadioButtonState.CheckedDisabled:
				return ButtonState.Checked | ButtonState.Inactive;
			default:
				return ButtonState.Normal;
			}
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x00121ABC File Offset: 0x00120ABC
		internal static RadioButtonState ConvertFromButtonState(ButtonState state, bool isHot)
		{
			if ((state & ButtonState.Checked) == ButtonState.Checked)
			{
				if ((state & ButtonState.Pushed) == ButtonState.Pushed)
				{
					return RadioButtonState.CheckedPressed;
				}
				if ((state & ButtonState.Inactive) == ButtonState.Inactive)
				{
					return RadioButtonState.CheckedDisabled;
				}
				if (isHot)
				{
					return RadioButtonState.CheckedHot;
				}
				return RadioButtonState.CheckedNormal;
			}
			else
			{
				if ((state & ButtonState.Pushed) == ButtonState.Pushed)
				{
					return RadioButtonState.UncheckedPressed;
				}
				if ((state & ButtonState.Inactive) == ButtonState.Inactive)
				{
					return RadioButtonState.UncheckedDisabled;
				}
				if (isHot)
				{
					return RadioButtonState.UncheckedHot;
				}
				return RadioButtonState.UncheckedNormal;
			}
		}

		// Token: 0x06004E9D RID: 20125 RVA: 0x00121B24 File Offset: 0x00120B24
		private static void InitializeRenderer(int state)
		{
			if (RadioButtonRenderer.visualStyleRenderer == null)
			{
				RadioButtonRenderer.visualStyleRenderer = new VisualStyleRenderer(RadioButtonRenderer.RadioElement.ClassName, RadioButtonRenderer.RadioElement.Part, state);
				return;
			}
			RadioButtonRenderer.visualStyleRenderer.SetParameters(RadioButtonRenderer.RadioElement.ClassName, RadioButtonRenderer.RadioElement.Part, state);
		}

		// Token: 0x040032C3 RID: 12995
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer = null;

		// Token: 0x040032C4 RID: 12996
		private static readonly VisualStyleElement RadioElement = VisualStyleElement.Button.RadioButton.UncheckedNormal;

		// Token: 0x040032C5 RID: 12997
		private static bool renderMatchingApplicationState = true;
	}
}

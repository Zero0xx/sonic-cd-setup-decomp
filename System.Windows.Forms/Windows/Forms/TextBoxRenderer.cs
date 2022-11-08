using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x0200065F RID: 1631
	public sealed class TextBoxRenderer
	{
		// Token: 0x060055A5 RID: 21925 RVA: 0x0013749A File Offset: 0x0013649A
		private TextBoxRenderer()
		{
		}

		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x060055A6 RID: 21926 RVA: 0x001374A2 File Offset: 0x001364A2
		public static bool IsSupported
		{
			get
			{
				return VisualStyleRenderer.IsSupported;
			}
		}

		// Token: 0x060055A7 RID: 21927 RVA: 0x001374AC File Offset: 0x001364AC
		private static void DrawBackground(Graphics g, Rectangle bounds, TextBoxState state)
		{
			TextBoxRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			if (state != TextBoxState.Disabled)
			{
				Color color = TextBoxRenderer.visualStyleRenderer.GetColor(ColorProperty.FillColor);
				if (color != SystemColors.Window)
				{
					Rectangle backgroundContentRectangle = TextBoxRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
					g.FillRectangle(new SolidBrush(SystemColors.Window), backgroundContentRectangle);
				}
			}
		}

		// Token: 0x060055A8 RID: 21928 RVA: 0x00137504 File Offset: 0x00136504
		public static void DrawTextBox(Graphics g, Rectangle bounds, TextBoxState state)
		{
			TextBoxRenderer.InitializeRenderer((int)state);
			TextBoxRenderer.DrawBackground(g, bounds, state);
		}

		// Token: 0x060055A9 RID: 21929 RVA: 0x00137514 File Offset: 0x00136514
		public static void DrawTextBox(Graphics g, Rectangle bounds, string textBoxText, Font font, TextBoxState state)
		{
			TextBoxRenderer.DrawTextBox(g, bounds, textBoxText, font, TextFormatFlags.TextBoxControl, state);
		}

		// Token: 0x060055AA RID: 21930 RVA: 0x00137526 File Offset: 0x00136526
		public static void DrawTextBox(Graphics g, Rectangle bounds, string textBoxText, Font font, Rectangle textBounds, TextBoxState state)
		{
			TextBoxRenderer.DrawTextBox(g, bounds, textBoxText, font, textBounds, TextFormatFlags.TextBoxControl, state);
		}

		// Token: 0x060055AB RID: 21931 RVA: 0x0013753C File Offset: 0x0013653C
		public static void DrawTextBox(Graphics g, Rectangle bounds, string textBoxText, Font font, TextFormatFlags flags, TextBoxState state)
		{
			TextBoxRenderer.InitializeRenderer((int)state);
			Rectangle backgroundContentRectangle = TextBoxRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
			backgroundContentRectangle.Inflate(-2, -2);
			TextBoxRenderer.DrawTextBox(g, bounds, textBoxText, font, backgroundContentRectangle, flags, state);
		}

		// Token: 0x060055AC RID: 21932 RVA: 0x00137578 File Offset: 0x00136578
		public static void DrawTextBox(Graphics g, Rectangle bounds, string textBoxText, Font font, Rectangle textBounds, TextFormatFlags flags, TextBoxState state)
		{
			TextBoxRenderer.InitializeRenderer((int)state);
			TextBoxRenderer.DrawBackground(g, bounds, state);
			Color color = TextBoxRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			TextRenderer.DrawText(g, textBoxText, font, textBounds, color, flags);
		}

		// Token: 0x060055AD RID: 21933 RVA: 0x001375B4 File Offset: 0x001365B4
		private static void InitializeRenderer(int state)
		{
			if (TextBoxRenderer.visualStyleRenderer == null)
			{
				TextBoxRenderer.visualStyleRenderer = new VisualStyleRenderer(TextBoxRenderer.TextBoxElement.ClassName, TextBoxRenderer.TextBoxElement.Part, state);
				return;
			}
			TextBoxRenderer.visualStyleRenderer.SetParameters(TextBoxRenderer.TextBoxElement.ClassName, TextBoxRenderer.TextBoxElement.Part, state);
		}

		// Token: 0x0400370E RID: 14094
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer = null;

		// Token: 0x0400370F RID: 14095
		private static readonly VisualStyleElement TextBoxElement = VisualStyleElement.TextBox.TextEdit.Normal;
	}
}

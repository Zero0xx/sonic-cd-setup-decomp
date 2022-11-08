using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x02000299 RID: 665
	public sealed class ComboBoxRenderer
	{
		// Token: 0x060023E5 RID: 9189 RVA: 0x000521F4 File Offset: 0x000511F4
		private ComboBoxRenderer()
		{
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x060023E6 RID: 9190 RVA: 0x000521FC File Offset: 0x000511FC
		public static bool IsSupported
		{
			get
			{
				return VisualStyleRenderer.IsSupported;
			}
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x00052204 File Offset: 0x00051204
		private static void DrawBackground(Graphics g, Rectangle bounds, ComboBoxState state)
		{
			ComboBoxRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			if (state != ComboBoxState.Disabled)
			{
				Color color = ComboBoxRenderer.visualStyleRenderer.GetColor(ColorProperty.FillColor);
				if (color != SystemColors.Window)
				{
					Rectangle backgroundContentRectangle = ComboBoxRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
					backgroundContentRectangle.Inflate(-2, -2);
					g.FillRectangle(SystemBrushes.Window, backgroundContentRectangle);
				}
			}
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x00052264 File Offset: 0x00051264
		public static void DrawTextBox(Graphics g, Rectangle bounds, ComboBoxState state)
		{
			if (ComboBoxRenderer.visualStyleRenderer == null)
			{
				ComboBoxRenderer.visualStyleRenderer = new VisualStyleRenderer(ComboBoxRenderer.TextBoxElement.ClassName, ComboBoxRenderer.TextBoxElement.Part, (int)state);
			}
			else
			{
				ComboBoxRenderer.visualStyleRenderer.SetParameters(ComboBoxRenderer.TextBoxElement.ClassName, ComboBoxRenderer.TextBoxElement.Part, (int)state);
			}
			ComboBoxRenderer.DrawBackground(g, bounds, state);
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x000522C0 File Offset: 0x000512C0
		public static void DrawTextBox(Graphics g, Rectangle bounds, string comboBoxText, Font font, ComboBoxState state)
		{
			ComboBoxRenderer.DrawTextBox(g, bounds, comboBoxText, font, TextFormatFlags.TextBoxControl, state);
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x000522D2 File Offset: 0x000512D2
		public static void DrawTextBox(Graphics g, Rectangle bounds, string comboBoxText, Font font, Rectangle textBounds, ComboBoxState state)
		{
			ComboBoxRenderer.DrawTextBox(g, bounds, comboBoxText, font, textBounds, TextFormatFlags.TextBoxControl, state);
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x000522E8 File Offset: 0x000512E8
		public static void DrawTextBox(Graphics g, Rectangle bounds, string comboBoxText, Font font, TextFormatFlags flags, ComboBoxState state)
		{
			if (ComboBoxRenderer.visualStyleRenderer == null)
			{
				ComboBoxRenderer.visualStyleRenderer = new VisualStyleRenderer(ComboBoxRenderer.TextBoxElement.ClassName, ComboBoxRenderer.TextBoxElement.Part, (int)state);
			}
			else
			{
				ComboBoxRenderer.visualStyleRenderer.SetParameters(ComboBoxRenderer.TextBoxElement.ClassName, ComboBoxRenderer.TextBoxElement.Part, (int)state);
			}
			Rectangle backgroundContentRectangle = ComboBoxRenderer.visualStyleRenderer.GetBackgroundContentRectangle(g, bounds);
			backgroundContentRectangle.Inflate(-2, -2);
			ComboBoxRenderer.DrawTextBox(g, bounds, comboBoxText, font, backgroundContentRectangle, flags, state);
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x00052364 File Offset: 0x00051364
		public static void DrawTextBox(Graphics g, Rectangle bounds, string comboBoxText, Font font, Rectangle textBounds, TextFormatFlags flags, ComboBoxState state)
		{
			if (ComboBoxRenderer.visualStyleRenderer == null)
			{
				ComboBoxRenderer.visualStyleRenderer = new VisualStyleRenderer(ComboBoxRenderer.TextBoxElement.ClassName, ComboBoxRenderer.TextBoxElement.Part, (int)state);
			}
			else
			{
				ComboBoxRenderer.visualStyleRenderer.SetParameters(ComboBoxRenderer.TextBoxElement.ClassName, ComboBoxRenderer.TextBoxElement.Part, (int)state);
			}
			ComboBoxRenderer.DrawBackground(g, bounds, state);
			Color color = ComboBoxRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			TextRenderer.DrawText(g, comboBoxText, font, textBounds, color, flags);
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x000523E0 File Offset: 0x000513E0
		public static void DrawDropDownButton(Graphics g, Rectangle bounds, ComboBoxState state)
		{
			if (ComboBoxRenderer.visualStyleRenderer == null)
			{
				ComboBoxRenderer.visualStyleRenderer = new VisualStyleRenderer(ComboBoxRenderer.ComboBoxElement.ClassName, ComboBoxRenderer.ComboBoxElement.Part, (int)state);
			}
			else
			{
				ComboBoxRenderer.visualStyleRenderer.SetParameters(ComboBoxRenderer.ComboBoxElement.ClassName, ComboBoxRenderer.ComboBoxElement.Part, (int)state);
			}
			ComboBoxRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x04001587 RID: 5511
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer = null;

		// Token: 0x04001588 RID: 5512
		private static readonly VisualStyleElement ComboBoxElement = VisualStyleElement.ComboBox.DropDownButton.Normal;

		// Token: 0x04001589 RID: 5513
		private static readonly VisualStyleElement TextBoxElement = VisualStyleElement.TextBox.TextEdit.Normal;
	}
}

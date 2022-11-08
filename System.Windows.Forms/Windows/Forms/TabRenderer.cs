using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x0200065C RID: 1628
	public sealed class TabRenderer
	{
		// Token: 0x06005597 RID: 21911 RVA: 0x00137239 File Offset: 0x00136239
		private TabRenderer()
		{
		}

		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x06005598 RID: 21912 RVA: 0x00137241 File Offset: 0x00136241
		public static bool IsSupported
		{
			get
			{
				return VisualStyleRenderer.IsSupported;
			}
		}

		// Token: 0x06005599 RID: 21913 RVA: 0x00137248 File Offset: 0x00136248
		public static void DrawTabItem(Graphics g, Rectangle bounds, TabItemState state)
		{
			TabRenderer.InitializeRenderer(VisualStyleElement.Tab.TabItem.Normal, (int)state);
			TabRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x0600559A RID: 21914 RVA: 0x00137264 File Offset: 0x00136264
		public static void DrawTabItem(Graphics g, Rectangle bounds, bool focused, TabItemState state)
		{
			TabRenderer.InitializeRenderer(VisualStyleElement.Tab.TabItem.Normal, (int)state);
			TabRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			Rectangle rectangle = Rectangle.Inflate(bounds, -3, -3);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		// Token: 0x0600559B RID: 21915 RVA: 0x0013729D File Offset: 0x0013629D
		public static void DrawTabItem(Graphics g, Rectangle bounds, string tabItemText, Font font, TabItemState state)
		{
			TabRenderer.DrawTabItem(g, bounds, tabItemText, font, false, state);
		}

		// Token: 0x0600559C RID: 21916 RVA: 0x001372AB File Offset: 0x001362AB
		public static void DrawTabItem(Graphics g, Rectangle bounds, string tabItemText, Font font, bool focused, TabItemState state)
		{
			TabRenderer.DrawTabItem(g, bounds, tabItemText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, focused, state);
		}

		// Token: 0x0600559D RID: 21917 RVA: 0x001372BC File Offset: 0x001362BC
		public static void DrawTabItem(Graphics g, Rectangle bounds, string tabItemText, Font font, TextFormatFlags flags, bool focused, TabItemState state)
		{
			TabRenderer.InitializeRenderer(VisualStyleElement.Tab.TabItem.Normal, (int)state);
			TabRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			Rectangle rectangle = Rectangle.Inflate(bounds, -3, -3);
			Color color = TabRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			TextRenderer.DrawText(g, tabItemText, font, rectangle, color, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		// Token: 0x0600559E RID: 21918 RVA: 0x00137314 File Offset: 0x00136314
		public static void DrawTabItem(Graphics g, Rectangle bounds, Image image, Rectangle imageRectangle, bool focused, TabItemState state)
		{
			TabRenderer.InitializeRenderer(VisualStyleElement.Tab.TabItem.Normal, (int)state);
			TabRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			Rectangle rectangle = Rectangle.Inflate(bounds, -3, -3);
			TabRenderer.visualStyleRenderer.DrawImage(g, imageRectangle, image);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		// Token: 0x0600559F RID: 21919 RVA: 0x0013735C File Offset: 0x0013635C
		public static void DrawTabItem(Graphics g, Rectangle bounds, string tabItemText, Font font, Image image, Rectangle imageRectangle, bool focused, TabItemState state)
		{
			TabRenderer.DrawTabItem(g, bounds, tabItemText, font, TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter, image, imageRectangle, focused, state);
		}

		// Token: 0x060055A0 RID: 21920 RVA: 0x0013737C File Offset: 0x0013637C
		public static void DrawTabItem(Graphics g, Rectangle bounds, string tabItemText, Font font, TextFormatFlags flags, Image image, Rectangle imageRectangle, bool focused, TabItemState state)
		{
			TabRenderer.InitializeRenderer(VisualStyleElement.Tab.TabItem.Normal, (int)state);
			TabRenderer.visualStyleRenderer.DrawBackground(g, bounds);
			Rectangle rectangle = Rectangle.Inflate(bounds, -3, -3);
			TabRenderer.visualStyleRenderer.DrawImage(g, imageRectangle, image);
			Color color = TabRenderer.visualStyleRenderer.GetColor(ColorProperty.TextColor);
			TextRenderer.DrawText(g, tabItemText, font, rectangle, color, flags);
			if (focused)
			{
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}

		// Token: 0x060055A1 RID: 21921 RVA: 0x001373E2 File Offset: 0x001363E2
		public static void DrawTabPage(Graphics g, Rectangle bounds)
		{
			TabRenderer.InitializeRenderer(VisualStyleElement.Tab.Pane.Normal, 0);
			TabRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x060055A2 RID: 21922 RVA: 0x001373FB File Offset: 0x001363FB
		private static void InitializeRenderer(VisualStyleElement element, int state)
		{
			if (TabRenderer.visualStyleRenderer == null)
			{
				TabRenderer.visualStyleRenderer = new VisualStyleRenderer(element.ClassName, element.Part, state);
				return;
			}
			TabRenderer.visualStyleRenderer.SetParameters(element.ClassName, element.Part, state);
		}

		// Token: 0x04003709 RID: 14089
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer;
	}
}

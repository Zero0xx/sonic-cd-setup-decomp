using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x020006F5 RID: 1781
	public sealed class TrackBarRenderer
	{
		// Token: 0x06005EAA RID: 24234 RVA: 0x00157FC9 File Offset: 0x00156FC9
		private TrackBarRenderer()
		{
		}

		// Token: 0x170013F2 RID: 5106
		// (get) Token: 0x06005EAB RID: 24235 RVA: 0x00157FD1 File Offset: 0x00156FD1
		public static bool IsSupported
		{
			get
			{
				return VisualStyleRenderer.IsSupported;
			}
		}

		// Token: 0x06005EAC RID: 24236 RVA: 0x00157FD8 File Offset: 0x00156FD8
		public static void DrawHorizontalTrack(Graphics g, Rectangle bounds)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.Track.Normal, 1);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x06005EAD RID: 24237 RVA: 0x00157FF1 File Offset: 0x00156FF1
		public static void DrawVerticalTrack(Graphics g, Rectangle bounds)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.TrackVertical.Normal, 1);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x06005EAE RID: 24238 RVA: 0x0015800A File Offset: 0x0015700A
		public static void DrawHorizontalThumb(Graphics g, Rectangle bounds, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.Thumb.Normal, (int)state);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x06005EAF RID: 24239 RVA: 0x00158023 File Offset: 0x00157023
		public static void DrawVerticalThumb(Graphics g, Rectangle bounds, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbVertical.Normal, (int)state);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x06005EB0 RID: 24240 RVA: 0x0015803C File Offset: 0x0015703C
		public static void DrawLeftPointingThumb(Graphics g, Rectangle bounds, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbLeft.Normal, (int)state);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x06005EB1 RID: 24241 RVA: 0x00158055 File Offset: 0x00157055
		public static void DrawRightPointingThumb(Graphics g, Rectangle bounds, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbRight.Normal, (int)state);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x06005EB2 RID: 24242 RVA: 0x0015806E File Offset: 0x0015706E
		public static void DrawTopPointingThumb(Graphics g, Rectangle bounds, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbTop.Normal, (int)state);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x06005EB3 RID: 24243 RVA: 0x00158087 File Offset: 0x00157087
		public static void DrawBottomPointingThumb(Graphics g, Rectangle bounds, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbBottom.Normal, (int)state);
			TrackBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x06005EB4 RID: 24244 RVA: 0x001580A0 File Offset: 0x001570A0
		public static void DrawHorizontalTicks(Graphics g, Rectangle bounds, int numTicks, EdgeStyle edgeStyle)
		{
			if (numTicks <= 0 || bounds.Height <= 0 || bounds.Width <= 0 || g == null)
			{
				return;
			}
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.Ticks.Normal, 1);
			if (numTicks == 1)
			{
				TrackBarRenderer.visualStyleRenderer.DrawEdge(g, new Rectangle(bounds.X, bounds.Y, 2, bounds.Height), Edges.Left, edgeStyle, EdgeEffects.None);
				return;
			}
			float num = ((float)bounds.Width - 2f) / ((float)numTicks - 1f);
			while (numTicks > 0)
			{
				float num2 = (float)bounds.X + (float)(numTicks - 1) * num;
				TrackBarRenderer.visualStyleRenderer.DrawEdge(g, new Rectangle((int)Math.Round((double)num2), bounds.Y, 2, bounds.Height), Edges.Left, edgeStyle, EdgeEffects.None);
				numTicks--;
			}
		}

		// Token: 0x06005EB5 RID: 24245 RVA: 0x00158164 File Offset: 0x00157164
		public static void DrawVerticalTicks(Graphics g, Rectangle bounds, int numTicks, EdgeStyle edgeStyle)
		{
			if (numTicks <= 0 || bounds.Height <= 0 || bounds.Width <= 0 || g == null)
			{
				return;
			}
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.TicksVertical.Normal, 1);
			if (numTicks == 1)
			{
				TrackBarRenderer.visualStyleRenderer.DrawEdge(g, new Rectangle(bounds.X, bounds.Y, bounds.Width, 2), Edges.Top, edgeStyle, EdgeEffects.None);
				return;
			}
			float num = ((float)bounds.Height - 2f) / ((float)numTicks - 1f);
			while (numTicks > 0)
			{
				float num2 = (float)bounds.Y + (float)(numTicks - 1) * num;
				TrackBarRenderer.visualStyleRenderer.DrawEdge(g, new Rectangle(bounds.X, (int)Math.Round((double)num2), bounds.Width, 2), Edges.Top, edgeStyle, EdgeEffects.None);
				numTicks--;
			}
		}

		// Token: 0x06005EB6 RID: 24246 RVA: 0x00158225 File Offset: 0x00157225
		public static Size GetLeftPointingThumbSize(Graphics g, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbLeft.Normal, (int)state);
			return TrackBarRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.True);
		}

		// Token: 0x06005EB7 RID: 24247 RVA: 0x0015823E File Offset: 0x0015723E
		public static Size GetRightPointingThumbSize(Graphics g, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbRight.Normal, (int)state);
			return TrackBarRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.True);
		}

		// Token: 0x06005EB8 RID: 24248 RVA: 0x00158257 File Offset: 0x00157257
		public static Size GetTopPointingThumbSize(Graphics g, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbTop.Normal, (int)state);
			return TrackBarRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.True);
		}

		// Token: 0x06005EB9 RID: 24249 RVA: 0x00158270 File Offset: 0x00157270
		public static Size GetBottomPointingThumbSize(Graphics g, TrackBarThumbState state)
		{
			TrackBarRenderer.InitializeRenderer(VisualStyleElement.TrackBar.ThumbBottom.Normal, (int)state);
			return TrackBarRenderer.visualStyleRenderer.GetPartSize(g, ThemeSizeType.True);
		}

		// Token: 0x06005EBA RID: 24250 RVA: 0x00158289 File Offset: 0x00157289
		private static void InitializeRenderer(VisualStyleElement element, int state)
		{
			if (TrackBarRenderer.visualStyleRenderer == null)
			{
				TrackBarRenderer.visualStyleRenderer = new VisualStyleRenderer(element.ClassName, element.Part, state);
				return;
			}
			TrackBarRenderer.visualStyleRenderer.SetParameters(element.ClassName, element.Part, state);
		}

		// Token: 0x040039A3 RID: 14755
		private const int lineWidth = 2;

		// Token: 0x040039A4 RID: 14756
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer;
	}
}

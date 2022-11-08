using System;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x020005C0 RID: 1472
	public sealed class ProgressBarRenderer
	{
		// Token: 0x06004CDE RID: 19678 RVA: 0x0011A833 File Offset: 0x00119833
		private ProgressBarRenderer()
		{
		}

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x06004CDF RID: 19679 RVA: 0x0011A83B File Offset: 0x0011983B
		public static bool IsSupported
		{
			get
			{
				return VisualStyleRenderer.IsSupported;
			}
		}

		// Token: 0x06004CE0 RID: 19680 RVA: 0x0011A842 File Offset: 0x00119842
		public static void DrawHorizontalBar(Graphics g, Rectangle bounds)
		{
			ProgressBarRenderer.InitializeRenderer(VisualStyleElement.ProgressBar.Bar.Normal);
			ProgressBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x06004CE1 RID: 19681 RVA: 0x0011A85A File Offset: 0x0011985A
		public static void DrawVerticalBar(Graphics g, Rectangle bounds)
		{
			ProgressBarRenderer.InitializeRenderer(VisualStyleElement.ProgressBar.BarVertical.Normal);
			ProgressBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x06004CE2 RID: 19682 RVA: 0x0011A872 File Offset: 0x00119872
		public static void DrawHorizontalChunks(Graphics g, Rectangle bounds)
		{
			ProgressBarRenderer.InitializeRenderer(VisualStyleElement.ProgressBar.Chunk.Normal);
			ProgressBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x06004CE3 RID: 19683 RVA: 0x0011A88A File Offset: 0x0011988A
		public static void DrawVerticalChunks(Graphics g, Rectangle bounds)
		{
			ProgressBarRenderer.InitializeRenderer(VisualStyleElement.ProgressBar.ChunkVertical.Normal);
			ProgressBarRenderer.visualStyleRenderer.DrawBackground(g, bounds);
		}

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x06004CE4 RID: 19684 RVA: 0x0011A8A2 File Offset: 0x001198A2
		public static int ChunkThickness
		{
			get
			{
				ProgressBarRenderer.InitializeRenderer(VisualStyleElement.ProgressBar.Chunk.Normal);
				return ProgressBarRenderer.visualStyleRenderer.GetInteger(IntegerProperty.ProgressChunkSize);
			}
		}

		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x06004CE5 RID: 19685 RVA: 0x0011A8BD File Offset: 0x001198BD
		public static int ChunkSpaceThickness
		{
			get
			{
				ProgressBarRenderer.InitializeRenderer(VisualStyleElement.ProgressBar.Chunk.Normal);
				return ProgressBarRenderer.visualStyleRenderer.GetInteger(IntegerProperty.ProgressSpaceSize);
			}
		}

		// Token: 0x06004CE6 RID: 19686 RVA: 0x0011A8D8 File Offset: 0x001198D8
		private static void InitializeRenderer(VisualStyleElement element)
		{
			if (ProgressBarRenderer.visualStyleRenderer == null)
			{
				ProgressBarRenderer.visualStyleRenderer = new VisualStyleRenderer(element);
				return;
			}
			ProgressBarRenderer.visualStyleRenderer.SetParameters(element);
		}

		// Token: 0x04003235 RID: 12853
		[ThreadStatic]
		private static VisualStyleRenderer visualStyleRenderer;
	}
}

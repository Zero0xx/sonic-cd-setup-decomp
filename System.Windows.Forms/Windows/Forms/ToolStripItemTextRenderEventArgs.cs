using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020006A9 RID: 1705
	public class ToolStripItemTextRenderEventArgs : ToolStripItemRenderEventArgs
	{
		// Token: 0x06005957 RID: 22871 RVA: 0x0014479C File Offset: 0x0014379C
		public ToolStripItemTextRenderEventArgs(Graphics g, ToolStripItem item, string text, Rectangle textRectangle, Color textColor, Font textFont, TextFormatFlags format) : base(g, item)
		{
			this.text = text;
			this.textRectangle = textRectangle;
			this.defaultTextColor = textColor;
			this.textFont = textFont;
			this.textAlignment = item.TextAlign;
			this.textFormat = format;
			this.textDirection = item.TextDirection;
		}

		// Token: 0x06005958 RID: 22872 RVA: 0x00144818 File Offset: 0x00143818
		public ToolStripItemTextRenderEventArgs(Graphics g, ToolStripItem item, string text, Rectangle textRectangle, Color textColor, Font textFont, ContentAlignment textAlign) : base(g, item)
		{
			this.text = text;
			this.textRectangle = textRectangle;
			this.defaultTextColor = textColor;
			this.textFont = textFont;
			this.textFormat = ToolStripItemInternalLayout.ContentAlignToTextFormat(textAlign, item.RightToLeft == RightToLeft.Yes);
			this.textFormat = (item.ShowKeyboardCues ? this.textFormat : (this.textFormat | TextFormatFlags.HidePrefix));
			this.textDirection = item.TextDirection;
		}

		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x06005959 RID: 22873 RVA: 0x001448B8 File Offset: 0x001438B8
		// (set) Token: 0x0600595A RID: 22874 RVA: 0x001448C0 File Offset: 0x001438C0
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x0600595B RID: 22875 RVA: 0x001448C9 File Offset: 0x001438C9
		// (set) Token: 0x0600595C RID: 22876 RVA: 0x001448E0 File Offset: 0x001438E0
		public Color TextColor
		{
			get
			{
				if (this.textColorChanged)
				{
					return this.textColor;
				}
				return this.DefaultTextColor;
			}
			set
			{
				this.textColor = value;
				this.textColorChanged = true;
			}
		}

		// Token: 0x17001281 RID: 4737
		// (get) Token: 0x0600595D RID: 22877 RVA: 0x001448F0 File Offset: 0x001438F0
		// (set) Token: 0x0600595E RID: 22878 RVA: 0x001448F8 File Offset: 0x001438F8
		internal Color DefaultTextColor
		{
			get
			{
				return this.defaultTextColor;
			}
			set
			{
				this.defaultTextColor = value;
			}
		}

		// Token: 0x17001282 RID: 4738
		// (get) Token: 0x0600595F RID: 22879 RVA: 0x00144901 File Offset: 0x00143901
		// (set) Token: 0x06005960 RID: 22880 RVA: 0x00144909 File Offset: 0x00143909
		public Font TextFont
		{
			get
			{
				return this.textFont;
			}
			set
			{
				this.textFont = value;
			}
		}

		// Token: 0x17001283 RID: 4739
		// (get) Token: 0x06005961 RID: 22881 RVA: 0x00144912 File Offset: 0x00143912
		// (set) Token: 0x06005962 RID: 22882 RVA: 0x0014491A File Offset: 0x0014391A
		public Rectangle TextRectangle
		{
			get
			{
				return this.textRectangle;
			}
			set
			{
				this.textRectangle = value;
			}
		}

		// Token: 0x17001284 RID: 4740
		// (get) Token: 0x06005963 RID: 22883 RVA: 0x00144923 File Offset: 0x00143923
		// (set) Token: 0x06005964 RID: 22884 RVA: 0x0014492B File Offset: 0x0014392B
		public TextFormatFlags TextFormat
		{
			get
			{
				return this.textFormat;
			}
			set
			{
				this.textFormat = value;
			}
		}

		// Token: 0x17001285 RID: 4741
		// (get) Token: 0x06005965 RID: 22885 RVA: 0x00144934 File Offset: 0x00143934
		// (set) Token: 0x06005966 RID: 22886 RVA: 0x0014493C File Offset: 0x0014393C
		public ToolStripTextDirection TextDirection
		{
			get
			{
				return this.textDirection;
			}
			set
			{
				this.textDirection = value;
			}
		}

		// Token: 0x04003867 RID: 14439
		private string text;

		// Token: 0x04003868 RID: 14440
		private Rectangle textRectangle = Rectangle.Empty;

		// Token: 0x04003869 RID: 14441
		private Color textColor = SystemColors.ControlText;

		// Token: 0x0400386A RID: 14442
		private Font textFont;

		// Token: 0x0400386B RID: 14443
		private ContentAlignment textAlignment;

		// Token: 0x0400386C RID: 14444
		private ToolStripTextDirection textDirection = ToolStripTextDirection.Horizontal;

		// Token: 0x0400386D RID: 14445
		private TextFormatFlags textFormat;

		// Token: 0x0400386E RID: 14446
		private Color defaultTextColor = SystemColors.ControlText;

		// Token: 0x0400386F RID: 14447
		private bool textColorChanged;
	}
}

using System;
using System.Drawing;
using System.Windows.Forms.ButtonInternal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000683 RID: 1667
	internal class ToolStripItemInternalLayout
	{
		// Token: 0x060057E7 RID: 22503 RVA: 0x0013D9AD File Offset: 0x0013C9AD
		public ToolStripItemInternalLayout(ToolStripItem ownerItem)
		{
			if (ownerItem == null)
			{
				throw new ArgumentNullException("ownerItem");
			}
			this.ownerItem = ownerItem;
		}

		// Token: 0x17001248 RID: 4680
		// (get) Token: 0x060057E8 RID: 22504 RVA: 0x0013D9D5 File Offset: 0x0013C9D5
		protected virtual ToolStripItem Owner
		{
			get
			{
				return this.ownerItem;
			}
		}

		// Token: 0x17001249 RID: 4681
		// (get) Token: 0x060057E9 RID: 22505 RVA: 0x0013D9E0 File Offset: 0x0013C9E0
		public virtual Rectangle ImageRectangle
		{
			get
			{
				Rectangle imageBounds = this.LayoutData.imageBounds;
				imageBounds.Intersect(this.layoutData.field);
				return imageBounds;
			}
		}

		// Token: 0x1700124A RID: 4682
		// (get) Token: 0x060057EA RID: 22506 RVA: 0x0013DA0C File Offset: 0x0013CA0C
		internal ButtonBaseAdapter.LayoutData LayoutData
		{
			get
			{
				this.EnsureLayout();
				return this.layoutData;
			}
		}

		// Token: 0x1700124B RID: 4683
		// (get) Token: 0x060057EB RID: 22507 RVA: 0x0013DA1B File Offset: 0x0013CA1B
		public Size PreferredImageSize
		{
			get
			{
				return this.Owner.PreferredImageSize;
			}
		}

		// Token: 0x1700124C RID: 4684
		// (get) Token: 0x060057EC RID: 22508 RVA: 0x0013DA28 File Offset: 0x0013CA28
		protected virtual ToolStrip ParentInternal
		{
			get
			{
				if (this.ownerItem == null)
				{
					return null;
				}
				return this.ownerItem.ParentInternal;
			}
		}

		// Token: 0x1700124D RID: 4685
		// (get) Token: 0x060057ED RID: 22509 RVA: 0x0013DA40 File Offset: 0x0013CA40
		public virtual Rectangle TextRectangle
		{
			get
			{
				Rectangle textBounds = this.LayoutData.textBounds;
				textBounds.Intersect(this.layoutData.field);
				return textBounds;
			}
		}

		// Token: 0x1700124E RID: 4686
		// (get) Token: 0x060057EE RID: 22510 RVA: 0x0013DA6C File Offset: 0x0013CA6C
		public virtual Rectangle ContentRectangle
		{
			get
			{
				return this.LayoutData.field;
			}
		}

		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x060057EF RID: 22511 RVA: 0x0013DA79 File Offset: 0x0013CA79
		public virtual TextFormatFlags TextFormat
		{
			get
			{
				if (this.currentLayoutOptions != null)
				{
					return this.currentLayoutOptions.gdiTextFormatFlags;
				}
				return this.CommonLayoutOptions().gdiTextFormatFlags;
			}
		}

		// Token: 0x060057F0 RID: 22512 RVA: 0x0013DA9C File Offset: 0x0013CA9C
		internal static TextFormatFlags ContentAlignToTextFormat(ContentAlignment alignment, bool rightToLeft)
		{
			TextFormatFlags textFormatFlags = TextFormatFlags.Default;
			if (rightToLeft)
			{
				textFormatFlags |= TextFormatFlags.RightToLeft;
			}
			textFormatFlags |= ControlPaint.TranslateAlignmentForGDI(alignment);
			return textFormatFlags | ControlPaint.TranslateLineAlignmentForGDI(alignment);
		}

		// Token: 0x060057F1 RID: 22513 RVA: 0x0013DACC File Offset: 0x0013CACC
		protected virtual ToolStripItemInternalLayout.ToolStripItemLayoutOptions CommonLayoutOptions()
		{
			ToolStripItemInternalLayout.ToolStripItemLayoutOptions toolStripItemLayoutOptions = new ToolStripItemInternalLayout.ToolStripItemLayoutOptions();
			Rectangle client = new Rectangle(Point.Empty, this.ownerItem.Size);
			toolStripItemLayoutOptions.client = client;
			toolStripItemLayoutOptions.growBorderBy1PxWhenDefault = false;
			toolStripItemLayoutOptions.borderSize = 2;
			toolStripItemLayoutOptions.paddingSize = 0;
			toolStripItemLayoutOptions.maxFocus = true;
			toolStripItemLayoutOptions.focusOddEvenFixup = false;
			toolStripItemLayoutOptions.font = this.ownerItem.Font;
			toolStripItemLayoutOptions.text = (((this.Owner.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text) ? this.Owner.Text : string.Empty);
			toolStripItemLayoutOptions.imageSize = this.PreferredImageSize;
			toolStripItemLayoutOptions.checkSize = 0;
			toolStripItemLayoutOptions.checkPaddingSize = 0;
			toolStripItemLayoutOptions.checkAlign = ContentAlignment.TopLeft;
			toolStripItemLayoutOptions.imageAlign = this.Owner.ImageAlign;
			toolStripItemLayoutOptions.textAlign = this.Owner.TextAlign;
			toolStripItemLayoutOptions.hintTextUp = false;
			toolStripItemLayoutOptions.shadowedText = !this.ownerItem.Enabled;
			toolStripItemLayoutOptions.layoutRTL = (RightToLeft.Yes == this.Owner.RightToLeft);
			toolStripItemLayoutOptions.textImageRelation = this.Owner.TextImageRelation;
			toolStripItemLayoutOptions.textImageInset = 0;
			toolStripItemLayoutOptions.everettButtonCompat = false;
			toolStripItemLayoutOptions.gdiTextFormatFlags = ToolStripItemInternalLayout.ContentAlignToTextFormat(this.Owner.TextAlign, this.Owner.RightToLeft == RightToLeft.Yes);
			toolStripItemLayoutOptions.gdiTextFormatFlags = (this.Owner.ShowKeyboardCues ? toolStripItemLayoutOptions.gdiTextFormatFlags : (toolStripItemLayoutOptions.gdiTextFormatFlags | TextFormatFlags.HidePrefix));
			return toolStripItemLayoutOptions;
		}

		// Token: 0x060057F2 RID: 22514 RVA: 0x0013DC36 File Offset: 0x0013CC36
		private bool EnsureLayout()
		{
			if (this.layoutData == null || this.parentLayoutData == null || !this.parentLayoutData.IsCurrent(this.ParentInternal))
			{
				this.PerformLayout();
				return true;
			}
			return false;
		}

		// Token: 0x060057F3 RID: 22515 RVA: 0x0013DC64 File Offset: 0x0013CC64
		private ButtonBaseAdapter.LayoutData GetLayoutData()
		{
			this.currentLayoutOptions = this.CommonLayoutOptions();
			if (this.Owner.TextDirection != ToolStripTextDirection.Horizontal)
			{
				this.currentLayoutOptions.verticalText = true;
			}
			return this.currentLayoutOptions.Layout();
		}

		// Token: 0x060057F4 RID: 22516 RVA: 0x0013DCA4 File Offset: 0x0013CCA4
		public virtual Size GetPreferredSize(Size constrainingSize)
		{
			Size empty = Size.Empty;
			this.EnsureLayout();
			if (this.ownerItem != null)
			{
				this.lastPreferredSize = this.currentLayoutOptions.GetPreferredSizeCore(constrainingSize);
				return this.lastPreferredSize;
			}
			return Size.Empty;
		}

		// Token: 0x060057F5 RID: 22517 RVA: 0x0013DCDC File Offset: 0x0013CCDC
		internal void PerformLayout()
		{
			this.layoutData = this.GetLayoutData();
			ToolStrip parentInternal = this.ParentInternal;
			if (parentInternal != null)
			{
				this.parentLayoutData = new ToolStripItemInternalLayout.ToolStripLayoutData(parentInternal);
				return;
			}
			this.parentLayoutData = null;
		}

		// Token: 0x040037AD RID: 14253
		private const int BORDER_WIDTH = 2;

		// Token: 0x040037AE RID: 14254
		private const int BORDER_HEIGHT = 3;

		// Token: 0x040037AF RID: 14255
		private ToolStripItemInternalLayout.ToolStripItemLayoutOptions currentLayoutOptions;

		// Token: 0x040037B0 RID: 14256
		private ToolStripItem ownerItem;

		// Token: 0x040037B1 RID: 14257
		private ButtonBaseAdapter.LayoutData layoutData;

		// Token: 0x040037B2 RID: 14258
		private static readonly Size INVALID_SIZE = new Size(int.MinValue, int.MinValue);

		// Token: 0x040037B3 RID: 14259
		private Size lastPreferredSize = ToolStripItemInternalLayout.INVALID_SIZE;

		// Token: 0x040037B4 RID: 14260
		private ToolStripItemInternalLayout.ToolStripLayoutData parentLayoutData;

		// Token: 0x0200068A RID: 1674
		internal class ToolStripItemLayoutOptions : ButtonBaseAdapter.LayoutOptions
		{
			// Token: 0x0600583B RID: 22587 RVA: 0x001405D4 File Offset: 0x0013F5D4
			protected override Size GetTextSize(Size proposedConstraints)
			{
				if (this.cachedSize != LayoutUtils.InvalidSize && (this.cachedProposedConstraints == proposedConstraints || this.cachedSize.Width <= proposedConstraints.Width))
				{
					return this.cachedSize;
				}
				this.cachedSize = base.GetTextSize(proposedConstraints);
				this.cachedProposedConstraints = proposedConstraints;
				return this.cachedSize;
			}

			// Token: 0x040037F9 RID: 14329
			private Size cachedSize = LayoutUtils.InvalidSize;

			// Token: 0x040037FA RID: 14330
			private Size cachedProposedConstraints = LayoutUtils.InvalidSize;
		}

		// Token: 0x0200068B RID: 1675
		private class ToolStripLayoutData
		{
			// Token: 0x0600583D RID: 22589 RVA: 0x00140654 File Offset: 0x0013F654
			public ToolStripLayoutData(ToolStrip toolStrip)
			{
				this.layoutStyle = toolStrip.LayoutStyle;
				this.autoSize = toolStrip.AutoSize;
				this.size = toolStrip.Size;
			}

			// Token: 0x0600583E RID: 22590 RVA: 0x00140680 File Offset: 0x0013F680
			public bool IsCurrent(ToolStrip toolStrip)
			{
				return toolStrip != null && (toolStrip.Size == this.size && toolStrip.LayoutStyle == this.layoutStyle) && toolStrip.AutoSize == this.autoSize;
			}

			// Token: 0x040037FB RID: 14331
			private ToolStripLayoutStyle layoutStyle;

			// Token: 0x040037FC RID: 14332
			private bool autoSize;

			// Token: 0x040037FD RID: 14333
			private Size size;
		}
	}
}

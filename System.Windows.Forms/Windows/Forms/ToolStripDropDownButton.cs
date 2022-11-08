using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000682 RID: 1666
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
	public class ToolStripDropDownButton : ToolStripDropDownItem
	{
		// Token: 0x060057D3 RID: 22483 RVA: 0x0013D6CE File Offset: 0x0013C6CE
		public ToolStripDropDownButton()
		{
			this.Initialize();
		}

		// Token: 0x060057D4 RID: 22484 RVA: 0x0013D6E3 File Offset: 0x0013C6E3
		public ToolStripDropDownButton(string text) : base(text, null, null)
		{
			this.Initialize();
		}

		// Token: 0x060057D5 RID: 22485 RVA: 0x0013D6FB File Offset: 0x0013C6FB
		public ToolStripDropDownButton(Image image) : base(null, image, null)
		{
			this.Initialize();
		}

		// Token: 0x060057D6 RID: 22486 RVA: 0x0013D713 File Offset: 0x0013C713
		public ToolStripDropDownButton(string text, Image image) : base(text, image, null)
		{
			this.Initialize();
		}

		// Token: 0x060057D7 RID: 22487 RVA: 0x0013D72B File Offset: 0x0013C72B
		public ToolStripDropDownButton(string text, Image image, EventHandler onClick) : base(text, image, onClick)
		{
			this.Initialize();
		}

		// Token: 0x060057D8 RID: 22488 RVA: 0x0013D743 File Offset: 0x0013C743
		public ToolStripDropDownButton(string text, Image image, EventHandler onClick, string name) : base(text, image, onClick, name)
		{
			this.Initialize();
		}

		// Token: 0x060057D9 RID: 22489 RVA: 0x0013D75D File Offset: 0x0013C75D
		public ToolStripDropDownButton(string text, Image image, params ToolStripItem[] dropDownItems) : base(text, image, dropDownItems)
		{
			this.Initialize();
		}

		// Token: 0x17001245 RID: 4677
		// (get) Token: 0x060057DA RID: 22490 RVA: 0x0013D775 File Offset: 0x0013C775
		// (set) Token: 0x060057DB RID: 22491 RVA: 0x0013D77D File Offset: 0x0013C77D
		[DefaultValue(true)]
		public new bool AutoToolTip
		{
			get
			{
				return base.AutoToolTip;
			}
			set
			{
				base.AutoToolTip = value;
			}
		}

		// Token: 0x17001246 RID: 4678
		// (get) Token: 0x060057DC RID: 22492 RVA: 0x0013D786 File Offset: 0x0013C786
		protected override bool DefaultAutoToolTip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001247 RID: 4679
		// (get) Token: 0x060057DD RID: 22493 RVA: 0x0013D789 File Offset: 0x0013C789
		// (set) Token: 0x060057DE RID: 22494 RVA: 0x0013D791 File Offset: 0x0013C791
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripDropDownButtonShowDropDownArrowDescr")]
		[DefaultValue(true)]
		public bool ShowDropDownArrow
		{
			get
			{
				return this.showDropDownArrow;
			}
			set
			{
				if (this.showDropDownArrow != value)
				{
					this.showDropDownArrow = value;
					base.InvalidateItemLayout(PropertyNames.ShowDropDownArrow);
				}
			}
		}

		// Token: 0x060057DF RID: 22495 RVA: 0x0013D7AE File Offset: 0x0013C7AE
		internal override ToolStripItemInternalLayout CreateInternalLayout()
		{
			return new ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout(this);
		}

		// Token: 0x060057E0 RID: 22496 RVA: 0x0013D7B6 File Offset: 0x0013C7B6
		protected override ToolStripDropDown CreateDefaultDropDown()
		{
			return new ToolStripDropDownMenu(this, true);
		}

		// Token: 0x060057E1 RID: 22497 RVA: 0x0013D7BF File Offset: 0x0013C7BF
		private void Initialize()
		{
			base.SupportsSpaceKey = true;
		}

		// Token: 0x060057E2 RID: 22498 RVA: 0x0013D7C8 File Offset: 0x0013C7C8
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (Control.ModifierKeys != Keys.Alt && e.Button == MouseButtons.Left)
			{
				if (base.DropDown.Visible)
				{
					ToolStripManager.ModalMenuFilter.CloseActiveDropDown(base.DropDown, ToolStripDropDownCloseReason.AppClicked);
				}
				else
				{
					this.openMouseId = ((base.ParentInternal == null) ? 0 : base.ParentInternal.GetMouseId());
					base.ShowDropDown(true);
				}
			}
			base.OnMouseDown(e);
		}

		// Token: 0x060057E3 RID: 22499 RVA: 0x0013D834 File Offset: 0x0013C834
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (Control.ModifierKeys != Keys.Alt && e.Button == MouseButtons.Left)
			{
				byte b = (base.ParentInternal == null) ? 0 : base.ParentInternal.GetMouseId();
				if (b != this.openMouseId)
				{
					this.openMouseId = 0;
					ToolStripManager.ModalMenuFilter.CloseActiveDropDown(base.DropDown, ToolStripDropDownCloseReason.AppClicked);
					base.Select();
				}
			}
			base.OnMouseUp(e);
		}

		// Token: 0x060057E4 RID: 22500 RVA: 0x0013D89A File Offset: 0x0013C89A
		protected override void OnMouseLeave(EventArgs e)
		{
			this.openMouseId = 0;
			base.OnMouseLeave(e);
		}

		// Token: 0x060057E5 RID: 22501 RVA: 0x0013D8AC File Offset: 0x0013C8AC
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.Owner != null)
			{
				ToolStripRenderer renderer = base.Renderer;
				Graphics graphics = e.Graphics;
				renderer.DrawDropDownButtonBackground(new ToolStripItemRenderEventArgs(e.Graphics, this));
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image)
				{
					renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(graphics, this, base.InternalLayout.ImageRectangle));
				}
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
				{
					renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(graphics, this, this.Text, base.InternalLayout.TextRectangle, this.ForeColor, this.Font, base.InternalLayout.TextFormat));
				}
				if (this.ShowDropDownArrow)
				{
					ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout toolStripDropDownButtonInternalLayout = base.InternalLayout as ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout;
					Rectangle arrowRectangle = (toolStripDropDownButtonInternalLayout != null) ? toolStripDropDownButtonInternalLayout.DropDownArrowRect : Rectangle.Empty;
					Color arrowColor = this.Enabled ? SystemColors.ControlText : SystemColors.ControlDark;
					renderer.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, this, arrowRectangle, arrowColor, ArrowDirection.Down));
				}
			}
		}

		// Token: 0x060057E6 RID: 22502 RVA: 0x0013D994 File Offset: 0x0013C994
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (this.HasDropDownItems)
			{
				base.Select();
				base.ShowDropDown();
				return true;
			}
			return false;
		}

		// Token: 0x040037AB RID: 14251
		private bool showDropDownArrow = true;

		// Token: 0x040037AC RID: 14252
		private byte openMouseId;

		// Token: 0x0200068C RID: 1676
		internal class ToolStripDropDownButtonInternalLayout : ToolStripItemInternalLayout
		{
			// Token: 0x0600583F RID: 22591 RVA: 0x001406B8 File Offset: 0x0013F6B8
			public ToolStripDropDownButtonInternalLayout(ToolStripDropDownButton ownerItem) : base(ownerItem)
			{
				this.ownerItem = ownerItem;
			}

			// Token: 0x06005840 RID: 22592 RVA: 0x001406D4 File Offset: 0x0013F6D4
			public override Size GetPreferredSize(Size constrainingSize)
			{
				Size preferredSize = base.GetPreferredSize(constrainingSize);
				if (this.ownerItem.ShowDropDownArrow)
				{
					if (this.ownerItem.TextDirection == ToolStripTextDirection.Horizontal)
					{
						preferredSize.Width += this.DropDownArrowRect.Width + ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowPadding.Horizontal;
					}
					else
					{
						preferredSize.Height += this.DropDownArrowRect.Height + ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowPadding.Vertical;
					}
				}
				return preferredSize;
			}

			// Token: 0x06005841 RID: 22593 RVA: 0x00140758 File Offset: 0x0013F758
			protected override ToolStripItemInternalLayout.ToolStripItemLayoutOptions CommonLayoutOptions()
			{
				ToolStripItemInternalLayout.ToolStripItemLayoutOptions toolStripItemLayoutOptions = base.CommonLayoutOptions();
				if (this.ownerItem.ShowDropDownArrow)
				{
					if (this.ownerItem.TextDirection == ToolStripTextDirection.Horizontal)
					{
						int num = ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowSize.Width + ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowPadding.Horizontal;
						ToolStripItemInternalLayout.ToolStripItemLayoutOptions toolStripItemLayoutOptions2 = toolStripItemLayoutOptions;
						toolStripItemLayoutOptions2.client.Width = toolStripItemLayoutOptions2.client.Width - num;
						if (this.ownerItem.RightToLeft == RightToLeft.Yes)
						{
							toolStripItemLayoutOptions.client.Offset(num, 0);
							this.dropDownArrowRect = new Rectangle(ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowPadding.Left, 0, ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowSize.Width, this.ownerItem.Bounds.Height);
						}
						else
						{
							this.dropDownArrowRect = new Rectangle(toolStripItemLayoutOptions.client.Right, 0, ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowSize.Width, this.ownerItem.Bounds.Height);
						}
					}
					else
					{
						int num2 = ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowSize.Height + ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowPadding.Vertical;
						ToolStripItemInternalLayout.ToolStripItemLayoutOptions toolStripItemLayoutOptions3 = toolStripItemLayoutOptions;
						toolStripItemLayoutOptions3.client.Height = toolStripItemLayoutOptions3.client.Height - num2;
						this.dropDownArrowRect = new Rectangle(0, toolStripItemLayoutOptions.client.Bottom + ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowPadding.Top, this.ownerItem.Bounds.Width - 1, ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout.dropDownArrowSize.Height);
					}
				}
				return toolStripItemLayoutOptions;
			}

			// Token: 0x17001256 RID: 4694
			// (get) Token: 0x06005842 RID: 22594 RVA: 0x001408AC File Offset: 0x0013F8AC
			public Rectangle DropDownArrowRect
			{
				get
				{
					return this.dropDownArrowRect;
				}
			}

			// Token: 0x040037FE RID: 14334
			private ToolStripDropDownButton ownerItem;

			// Token: 0x040037FF RID: 14335
			private static Size dropDownArrowSize = new Size(5, 3);

			// Token: 0x04003800 RID: 14336
			private static Padding dropDownArrowPadding = new Padding(2);

			// Token: 0x04003801 RID: 14337
			private Rectangle dropDownArrowRect = Rectangle.Empty;
		}
	}
}

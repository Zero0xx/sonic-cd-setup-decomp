using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x0200049E RID: 1182
	[ToolboxItemFilter("System.Windows.Forms.MainMenu")]
	public class MainMenu : Menu
	{
		// Token: 0x06004682 RID: 18050 RVA: 0x000FFDB4 File Offset: 0x000FEDB4
		public MainMenu() : base(null)
		{
		}

		// Token: 0x06004683 RID: 18051 RVA: 0x000FFDC4 File Offset: 0x000FEDC4
		public MainMenu(IContainer container) : this()
		{
			container.Add(this);
		}

		// Token: 0x06004684 RID: 18052 RVA: 0x000FFDD3 File Offset: 0x000FEDD3
		public MainMenu(MenuItem[] items) : base(items)
		{
		}

		// Token: 0x14000279 RID: 633
		// (add) Token: 0x06004685 RID: 18053 RVA: 0x000FFDE3 File Offset: 0x000FEDE3
		// (remove) Token: 0x06004686 RID: 18054 RVA: 0x000FFDFC File Offset: 0x000FEDFC
		[SRDescription("MainMenuCollapseDescr")]
		public event EventHandler Collapse
		{
			add
			{
				this.onCollapse = (EventHandler)Delegate.Combine(this.onCollapse, value);
			}
			remove
			{
				this.onCollapse = (EventHandler)Delegate.Remove(this.onCollapse, value);
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06004687 RID: 18055 RVA: 0x000FFE15 File Offset: 0x000FEE15
		// (set) Token: 0x06004688 RID: 18056 RVA: 0x000FFE3C File Offset: 0x000FEE3C
		[Localizable(true)]
		[AmbientValue(RightToLeft.Inherit)]
		[SRDescription("MenuRightToLeftDescr")]
		public virtual RightToLeft RightToLeft
		{
			get
			{
				if (RightToLeft.Inherit != this.rightToLeft)
				{
					return this.rightToLeft;
				}
				if (this.form != null)
				{
					return this.form.RightToLeft;
				}
				return RightToLeft.Inherit;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("RightToLeft", (int)value, typeof(RightToLeft));
				}
				if (this.rightToLeft != value)
				{
					this.rightToLeft = value;
					base.UpdateRtl(value == RightToLeft.Yes);
				}
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06004689 RID: 18057 RVA: 0x000FFE89 File Offset: 0x000FEE89
		internal override bool RenderIsRightToLeft
		{
			get
			{
				return this.RightToLeft == RightToLeft.Yes && (this.form == null || !this.form.IsMirrored);
			}
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x000FFEB0 File Offset: 0x000FEEB0
		public virtual MainMenu CloneMenu()
		{
			MainMenu mainMenu = new MainMenu();
			mainMenu.CloneMenu(this);
			return mainMenu;
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x000FFECB File Offset: 0x000FEECB
		protected override IntPtr CreateMenuHandle()
		{
			return UnsafeNativeMethods.CreateMenu();
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x000FFED2 File Offset: 0x000FEED2
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.form != null && (this.ownerForm == null || this.form == this.ownerForm))
			{
				this.form.Menu = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x000FFF08 File Offset: 0x000FEF08
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public Form GetForm()
		{
			return this.form;
		}

		// Token: 0x0600468E RID: 18062 RVA: 0x000FFF10 File Offset: 0x000FEF10
		internal Form GetFormUnsafe()
		{
			return this.form;
		}

		// Token: 0x0600468F RID: 18063 RVA: 0x000FFF18 File Offset: 0x000FEF18
		internal override void ItemsChanged(int change)
		{
			base.ItemsChanged(change);
			if (this.form != null)
			{
				this.form.MenuChanged(change, this);
			}
		}

		// Token: 0x06004690 RID: 18064 RVA: 0x000FFF36 File Offset: 0x000FEF36
		internal virtual void ItemsChanged(int change, Menu menu)
		{
			if (this.form != null)
			{
				this.form.MenuChanged(change, menu);
			}
		}

		// Token: 0x06004691 RID: 18065 RVA: 0x000FFF4D File Offset: 0x000FEF4D
		protected internal virtual void OnCollapse(EventArgs e)
		{
			if (this.onCollapse != null)
			{
				this.onCollapse(this, e);
			}
		}

		// Token: 0x06004692 RID: 18066 RVA: 0x000FFF64 File Offset: 0x000FEF64
		internal virtual bool ShouldSerializeRightToLeft()
		{
			return RightToLeft.Inherit != this.RightToLeft;
		}

		// Token: 0x06004693 RID: 18067 RVA: 0x000FFF72 File Offset: 0x000FEF72
		public override string ToString()
		{
			return base.ToString();
		}

		// Token: 0x040021A9 RID: 8617
		internal Form form;

		// Token: 0x040021AA RID: 8618
		internal Form ownerForm;

		// Token: 0x040021AB RID: 8619
		private RightToLeft rightToLeft = RightToLeft.Inherit;

		// Token: 0x040021AC RID: 8620
		private EventHandler onCollapse;
	}
}

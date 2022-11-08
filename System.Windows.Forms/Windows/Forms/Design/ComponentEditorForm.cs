using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200077B RID: 1915
	[ToolboxItem(false)]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public partial class ComponentEditorForm : Form
	{
		// Token: 0x0600649A RID: 25754 RVA: 0x00170060 File Offset: 0x0016F060
		public ComponentEditorForm(object component, Type[] pageTypes)
		{
			if (!(component is IComponent))
			{
				throw new ArgumentException(SR.GetString("ComponentEditorFormBadComponent"), "component");
			}
			this.component = (IComponent)component;
			this.pageTypes = pageTypes;
			this.dirty = false;
			this.firstActivate = true;
			this.activePage = -1;
			this.initialActivePage = 0;
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MinimizeBox = false;
			base.MaximizeBox = false;
			base.ShowInTaskbar = false;
			base.Icon = null;
			base.StartPosition = FormStartPosition.CenterParent;
			this.OnNewObjects();
			this.OnConfigureUI();
		}

		// Token: 0x0600649B RID: 25755 RVA: 0x0017010C File Offset: 0x0016F10C
		internal virtual void ApplyChanges(bool lastApply)
		{
			if (this.dirty)
			{
				IComponentChangeService componentChangeService = null;
				if (this.component.Site != null)
				{
					componentChangeService = (IComponentChangeService)this.component.Site.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						try
						{
							componentChangeService.OnComponentChanging(this.component, null);
						}
						catch (CheckoutException ex)
						{
							if (ex == CheckoutException.Canceled)
							{
								return;
							}
							throw ex;
						}
					}
				}
				for (int i = 0; i < this.pageSites.Length; i++)
				{
					if (this.pageSites[i].Dirty)
					{
						this.pageSites[i].GetPageControl().ApplyChanges();
						this.pageSites[i].Dirty = false;
					}
				}
				if (componentChangeService != null)
				{
					componentChangeService.OnComponentChanged(this.component, null, null, null);
				}
				this.applyButton.Enabled = false;
				this.cancelButton.Text = SR.GetString("CloseCaption");
				this.dirty = false;
				if (!lastApply)
				{
					for (int j = 0; j < this.pageSites.Length; j++)
					{
						this.pageSites[j].GetPageControl().OnApplyComplete();
					}
				}
			}
		}

		// Token: 0x17001530 RID: 5424
		// (get) Token: 0x0600649C RID: 25756 RVA: 0x0017022C File Offset: 0x0016F22C
		// (set) Token: 0x0600649D RID: 25757 RVA: 0x00170234 File Offset: 0x0016F234
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		// Token: 0x140003F5 RID: 1013
		// (add) Token: 0x0600649E RID: 25758 RVA: 0x0017023D File Offset: 0x0016F23D
		// (remove) Token: 0x0600649F RID: 25759 RVA: 0x00170246 File Offset: 0x0016F246
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		// Token: 0x060064A0 RID: 25760 RVA: 0x00170250 File Offset: 0x0016F250
		private void OnButtonClick(object sender, EventArgs e)
		{
			if (sender == this.okButton)
			{
				this.ApplyChanges(true);
				base.DialogResult = DialogResult.OK;
				return;
			}
			if (sender == this.cancelButton)
			{
				base.DialogResult = DialogResult.Cancel;
				return;
			}
			if (sender == this.applyButton)
			{
				this.ApplyChanges(false);
				return;
			}
			if (sender == this.helpButton)
			{
				this.ShowPageHelp();
			}
		}

		// Token: 0x060064A1 RID: 25761 RVA: 0x001702A8 File Offset: 0x0016F2A8
		private void OnConfigureUI()
		{
			Font font = Control.DefaultFont;
			if (this.component.Site != null)
			{
				IUIService iuiservice = (IUIService)this.component.Site.GetService(typeof(IUIService));
				if (iuiservice != null)
				{
					font = (Font)iuiservice.Styles["DialogFont"];
				}
			}
			this.Font = font;
			this.okButton = new Button();
			this.cancelButton = new Button();
			this.applyButton = new Button();
			this.helpButton = new Button();
			this.selectorImageList = new ImageList();
			this.selectorImageList.ImageSize = new Size(16, 16);
			this.selector = new ComponentEditorForm.PageSelector();
			this.selector.ImageList = this.selectorImageList;
			this.selector.AfterSelect += this.OnSelChangeSelector;
			Label label = new Label();
			label.BackColor = SystemColors.ControlDark;
			int num = 90;
			if (this.pageSites != null)
			{
				for (int i = 0; i < this.pageSites.Length; i++)
				{
					ComponentEditorPage pageControl = this.pageSites[i].GetPageControl();
					string title = pageControl.Title;
					Graphics graphics = base.CreateGraphicsInternal();
					int num2 = (int)graphics.MeasureString(title, this.Font).Width;
					graphics.Dispose();
					this.selectorImageList.Images.Add(pageControl.Icon.ToBitmap());
					this.selector.Nodes.Add(new TreeNode(title, i, i));
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			num += 10;
			string text = string.Empty;
			ISite site = this.component.Site;
			if (site != null)
			{
				text = SR.GetString("ComponentEditorFormProperties", new object[]
				{
					site.Name
				});
			}
			else
			{
				text = SR.GetString("ComponentEditorFormPropertiesNoName");
			}
			this.Text = text;
			Rectangle rectangle = new Rectangle(12 + num, 16, this.maxSize.Width, this.maxSize.Height);
			this.pageHost.Bounds = rectangle;
			label.Bounds = new Rectangle(rectangle.X, 6, rectangle.Width, 4);
			if (this.pageSites != null)
			{
				Rectangle bounds = new Rectangle(0, 0, rectangle.Width, rectangle.Height);
				for (int j = 0; j < this.pageSites.Length; j++)
				{
					ComponentEditorPage pageControl2 = this.pageSites[j].GetPageControl();
					pageControl2.GetControl().Bounds = bounds;
				}
			}
			int width = SystemInformation.FixedFrameBorderSize.Width;
			Rectangle bounds2 = rectangle;
			Size size = new Size(bounds2.Width + 3 * (6 + width) + num, bounds2.Height + 4 + 24 + 23 + 2 * width + SystemInformation.CaptionHeight);
			base.Size = size;
			this.selector.Bounds = new Rectangle(6, 6, num, bounds2.Height + 4 + 12 + 23);
			bounds2.X = bounds2.Width + bounds2.X - 80;
			bounds2.Y = bounds2.Height + bounds2.Y + 6;
			bounds2.Width = 80;
			bounds2.Height = 23;
			this.helpButton.Bounds = bounds2;
			this.helpButton.Text = SR.GetString("HelpCaption");
			this.helpButton.Click += this.OnButtonClick;
			this.helpButton.Enabled = false;
			this.helpButton.FlatStyle = FlatStyle.System;
			bounds2.X -= 86;
			this.applyButton.Bounds = bounds2;
			this.applyButton.Text = SR.GetString("ApplyCaption");
			this.applyButton.Click += this.OnButtonClick;
			this.applyButton.Enabled = false;
			this.applyButton.FlatStyle = FlatStyle.System;
			bounds2.X -= 86;
			this.cancelButton.Bounds = bounds2;
			this.cancelButton.Text = SR.GetString("CancelCaption");
			this.cancelButton.Click += this.OnButtonClick;
			this.cancelButton.FlatStyle = FlatStyle.System;
			base.CancelButton = this.cancelButton;
			bounds2.X -= 86;
			this.okButton.Bounds = bounds2;
			this.okButton.Text = SR.GetString("OKCaption");
			this.okButton.Click += this.OnButtonClick;
			this.okButton.FlatStyle = FlatStyle.System;
			base.AcceptButton = this.okButton;
			base.Controls.Clear();
			base.Controls.AddRange(new Control[]
			{
				this.selector,
				label,
				this.pageHost,
				this.okButton,
				this.cancelButton,
				this.applyButton,
				this.helpButton
			});
			this.AutoScaleBaseSize = new Size(5, 14);
			base.ApplyAutoScaling();
		}

		// Token: 0x060064A2 RID: 25762 RVA: 0x001707D4 File Offset: 0x0016F7D4
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if (this.firstActivate)
			{
				this.firstActivate = false;
				this.selector.SelectedNode = this.selector.Nodes[this.initialActivePage];
				this.pageSites[this.initialActivePage].Active = true;
				this.activePage = this.initialActivePage;
				this.helpButton.Enabled = this.pageSites[this.activePage].GetPageControl().SupportsHelp();
			}
		}

		// Token: 0x060064A3 RID: 25763 RVA: 0x00170859 File Offset: 0x0016F859
		protected override void OnHelpRequested(HelpEventArgs e)
		{
			base.OnHelpRequested(e);
			this.ShowPageHelp();
		}

		// Token: 0x060064A4 RID: 25764 RVA: 0x00170868 File Offset: 0x0016F868
		private void OnNewObjects()
		{
			this.pageSites = null;
			this.maxSize = new Size(258, 24 * this.pageTypes.Length);
			this.pageSites = new ComponentEditorForm.ComponentEditorPageSite[this.pageTypes.Length];
			for (int i = 0; i < this.pageTypes.Length; i++)
			{
				this.pageSites[i] = new ComponentEditorForm.ComponentEditorPageSite(this.pageHost, this.pageTypes[i], this.component, this);
				ComponentEditorPage pageControl = this.pageSites[i].GetPageControl();
				Size size = pageControl.Size;
				if (size.Width > this.maxSize.Width)
				{
					this.maxSize.Width = size.Width;
				}
				if (size.Height > this.maxSize.Height)
				{
					this.maxSize.Height = size.Height;
				}
			}
			for (int j = 0; j < this.pageSites.Length; j++)
			{
				this.pageSites[j].GetPageControl().Size = this.maxSize;
			}
		}

		// Token: 0x060064A5 RID: 25765 RVA: 0x00170974 File Offset: 0x0016F974
		protected virtual void OnSelChangeSelector(object source, TreeViewEventArgs e)
		{
			if (this.firstActivate)
			{
				return;
			}
			int index = this.selector.SelectedNode.Index;
			if (index == this.activePage)
			{
				return;
			}
			if (this.activePage != -1)
			{
				if (this.pageSites[this.activePage].AutoCommit)
				{
					this.ApplyChanges(false);
				}
				this.pageSites[this.activePage].Active = false;
			}
			this.activePage = index;
			this.pageSites[this.activePage].Active = true;
			this.helpButton.Enabled = this.pageSites[this.activePage].GetPageControl().SupportsHelp();
		}

		// Token: 0x060064A6 RID: 25766 RVA: 0x00170A18 File Offset: 0x0016FA18
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public override bool PreProcessMessage(ref Message msg)
		{
			return (this.pageSites != null && this.pageSites[this.activePage].GetPageControl().IsPageMessage(ref msg)) || base.PreProcessMessage(ref msg);
		}

		// Token: 0x060064A7 RID: 25767 RVA: 0x00170A45 File Offset: 0x0016FA45
		internal virtual void SetDirty()
		{
			this.dirty = true;
			this.applyButton.Enabled = true;
			this.cancelButton.Text = SR.GetString("CancelCaption");
		}

		// Token: 0x060064A8 RID: 25768 RVA: 0x00170A6F File Offset: 0x0016FA6F
		public virtual DialogResult ShowForm()
		{
			return this.ShowForm(null, 0);
		}

		// Token: 0x060064A9 RID: 25769 RVA: 0x00170A79 File Offset: 0x0016FA79
		public virtual DialogResult ShowForm(int page)
		{
			return this.ShowForm(null, page);
		}

		// Token: 0x060064AA RID: 25770 RVA: 0x00170A83 File Offset: 0x0016FA83
		public virtual DialogResult ShowForm(IWin32Window owner)
		{
			return this.ShowForm(owner, 0);
		}

		// Token: 0x060064AB RID: 25771 RVA: 0x00170A8D File Offset: 0x0016FA8D
		public virtual DialogResult ShowForm(IWin32Window owner, int page)
		{
			this.initialActivePage = page;
			base.ShowDialog(owner);
			return base.DialogResult;
		}

		// Token: 0x060064AC RID: 25772 RVA: 0x00170AA4 File Offset: 0x0016FAA4
		private void ShowPageHelp()
		{
			if (this.pageSites[this.activePage].GetPageControl().SupportsHelp())
			{
				this.pageSites[this.activePage].GetPageControl().ShowHelp();
			}
		}

		// Token: 0x04003BD9 RID: 15321
		private const int BUTTON_WIDTH = 80;

		// Token: 0x04003BDA RID: 15322
		private const int BUTTON_HEIGHT = 23;

		// Token: 0x04003BDB RID: 15323
		private const int BUTTON_PAD = 6;

		// Token: 0x04003BDC RID: 15324
		private const int MIN_SELECTOR_WIDTH = 90;

		// Token: 0x04003BDD RID: 15325
		private const int SELECTOR_PADDING = 10;

		// Token: 0x04003BDE RID: 15326
		private const int STRIP_HEIGHT = 4;

		// Token: 0x04003BDF RID: 15327
		private IComponent component;

		// Token: 0x04003BE0 RID: 15328
		private Type[] pageTypes;

		// Token: 0x04003BE1 RID: 15329
		private ComponentEditorForm.ComponentEditorPageSite[] pageSites;

		// Token: 0x04003BE2 RID: 15330
		private Size maxSize = Size.Empty;

		// Token: 0x04003BE3 RID: 15331
		private int initialActivePage;

		// Token: 0x04003BE4 RID: 15332
		private int activePage;

		// Token: 0x04003BE5 RID: 15333
		private bool dirty;

		// Token: 0x04003BE6 RID: 15334
		private bool firstActivate;

		// Token: 0x04003BE7 RID: 15335
		private Panel pageHost = new Panel();

		// Token: 0x04003BE8 RID: 15336
		private ComponentEditorForm.PageSelector selector;

		// Token: 0x04003BE9 RID: 15337
		private ImageList selectorImageList;

		// Token: 0x04003BEA RID: 15338
		private Button okButton;

		// Token: 0x04003BEB RID: 15339
		private Button cancelButton;

		// Token: 0x04003BEC RID: 15340
		private Button applyButton;

		// Token: 0x04003BED RID: 15341
		private Button helpButton;

		// Token: 0x0200077C RID: 1916
		private sealed class ComponentEditorPageSite : IComponentEditorPageSite
		{
			// Token: 0x060064AD RID: 25773 RVA: 0x00170AD8 File Offset: 0x0016FAD8
			internal ComponentEditorPageSite(Control parent, Type pageClass, IComponent component, ComponentEditorForm form)
			{
				this.component = component;
				this.parent = parent;
				this.isActive = false;
				this.isDirty = false;
				if (form == null)
				{
					throw new ArgumentNullException("form");
				}
				this.form = form;
				try
				{
					this.pageControl = (ComponentEditorPage)SecurityUtils.SecureCreateInstance(pageClass);
				}
				catch (TargetInvocationException ex)
				{
					throw new TargetInvocationException(SR.GetString("ExceptionCreatingCompEditorControl", new object[]
					{
						ex.ToString()
					}), ex.InnerException);
				}
				this.pageControl.SetSite(this);
				this.pageControl.SetComponent(component);
			}

			// Token: 0x17001531 RID: 5425
			// (set) Token: 0x060064AE RID: 25774 RVA: 0x00170B84 File Offset: 0x0016FB84
			internal bool Active
			{
				set
				{
					if (value)
					{
						this.pageControl.CreateControl();
						this.pageControl.Activate();
					}
					else
					{
						this.pageControl.Deactivate();
					}
					this.isActive = value;
				}
			}

			// Token: 0x17001532 RID: 5426
			// (get) Token: 0x060064AF RID: 25775 RVA: 0x00170BB3 File Offset: 0x0016FBB3
			internal bool AutoCommit
			{
				get
				{
					return this.pageControl.CommitOnDeactivate;
				}
			}

			// Token: 0x17001533 RID: 5427
			// (get) Token: 0x060064B0 RID: 25776 RVA: 0x00170BC0 File Offset: 0x0016FBC0
			// (set) Token: 0x060064B1 RID: 25777 RVA: 0x00170BC8 File Offset: 0x0016FBC8
			internal bool Dirty
			{
				get
				{
					return this.isDirty;
				}
				set
				{
					this.isDirty = value;
				}
			}

			// Token: 0x060064B2 RID: 25778 RVA: 0x00170BD1 File Offset: 0x0016FBD1
			public Control GetControl()
			{
				return this.parent;
			}

			// Token: 0x060064B3 RID: 25779 RVA: 0x00170BD9 File Offset: 0x0016FBD9
			internal ComponentEditorPage GetPageControl()
			{
				return this.pageControl;
			}

			// Token: 0x060064B4 RID: 25780 RVA: 0x00170BE1 File Offset: 0x0016FBE1
			public void SetDirty()
			{
				if (this.isActive)
				{
					this.Dirty = true;
				}
				this.form.SetDirty();
			}

			// Token: 0x04003BEE RID: 15342
			internal IComponent component;

			// Token: 0x04003BEF RID: 15343
			internal ComponentEditorPage pageControl;

			// Token: 0x04003BF0 RID: 15344
			internal Control parent;

			// Token: 0x04003BF1 RID: 15345
			internal bool isActive;

			// Token: 0x04003BF2 RID: 15346
			internal bool isDirty;

			// Token: 0x04003BF3 RID: 15347
			private ComponentEditorForm form;
		}

		// Token: 0x0200077D RID: 1917
		internal sealed class PageSelector : TreeView
		{
			// Token: 0x060064B5 RID: 25781 RVA: 0x00170C00 File Offset: 0x0016FC00
			public PageSelector()
			{
				base.HotTracking = true;
				base.HideSelection = false;
				this.BackColor = SystemColors.Control;
				base.Indent = 0;
				base.LabelEdit = false;
				base.Scrollable = false;
				base.ShowLines = false;
				base.ShowPlusMinus = false;
				base.ShowRootLines = false;
				base.BorderStyle = BorderStyle.None;
				base.Indent = 0;
				base.FullRowSelect = true;
			}

			// Token: 0x17001534 RID: 5428
			// (get) Token: 0x060064B6 RID: 25782 RVA: 0x00170C6C File Offset: 0x0016FC6C
			protected override CreateParams CreateParams
			{
				[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					CreateParams createParams = base.CreateParams;
					createParams.ExStyle |= 131072;
					return createParams;
				}
			}

			// Token: 0x060064B7 RID: 25783 RVA: 0x00170CA8 File Offset: 0x0016FCA8
			private void CreateDitherBrush()
			{
				short[] lpvBits = new short[]
				{
					-21846,
					21845,
					-21846,
					21845,
					-21846,
					21845,
					-21846,
					21845
				};
				IntPtr intPtr = SafeNativeMethods.CreateBitmap(8, 8, 1, 1, lpvBits);
				if (intPtr != IntPtr.Zero)
				{
					this.hbrushDither = SafeNativeMethods.CreatePatternBrush(new HandleRef(null, intPtr));
					SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
				}
			}

			// Token: 0x060064B8 RID: 25784 RVA: 0x00170D00 File Offset: 0x0016FD00
			private void DrawTreeItem(string itemText, int imageIndex, IntPtr dc, NativeMethods.RECT rcIn, int state, int backColor, int textColor)
			{
				IntNativeMethods.SIZE size = new IntNativeMethods.SIZE();
				IntNativeMethods.RECT rect = default(IntNativeMethods.RECT);
				IntNativeMethods.RECT rect2 = new IntNativeMethods.RECT(rcIn.left, rcIn.top, rcIn.right, rcIn.bottom);
				ImageList imageList = base.ImageList;
				IntPtr intPtr = IntPtr.Zero;
				if ((state & 2) != 0)
				{
					intPtr = SafeNativeMethods.SelectObject(new HandleRef(null, dc), new HandleRef(base.Parent, base.Parent.FontHandle));
				}
				if ((state & 1) != 0 && this.hbrushDither != IntPtr.Zero)
				{
					this.FillRectDither(dc, rcIn);
					SafeNativeMethods.SetBkMode(new HandleRef(null, dc), 1);
				}
				else
				{
					SafeNativeMethods.SetBkColor(new HandleRef(null, dc), backColor);
					IntUnsafeNativeMethods.ExtTextOut(new HandleRef(null, dc), 0, 0, 6, ref rect2, null, 0, null);
				}
				IntUnsafeNativeMethods.GetTextExtentPoint32(new HandleRef(null, dc), itemText, size);
				rect.left = rect2.left + 16 + 8;
				rect.top = rect2.top + (rect2.bottom - rect2.top - size.cy >> 1);
				rect.bottom = rect.top + size.cy;
				rect.right = rect2.right;
				SafeNativeMethods.SetTextColor(new HandleRef(null, dc), textColor);
				IntUnsafeNativeMethods.DrawText(new HandleRef(null, dc), itemText, ref rect, 34820);
				SafeNativeMethods.ImageList_Draw(new HandleRef(imageList, imageList.Handle), imageIndex, new HandleRef(null, dc), 4, rect2.top + (rect2.bottom - rect2.top - 16 >> 1), 1);
				if ((state & 2) != 0)
				{
					int clr = SafeNativeMethods.SetBkColor(new HandleRef(null, dc), ColorTranslator.ToWin32(SystemColors.ControlLightLight));
					rect.left = rect2.left;
					rect.top = rect2.top;
					rect.bottom = rect2.top + 1;
					rect.right = rect2.right;
					IntUnsafeNativeMethods.ExtTextOut(new HandleRef(null, dc), 0, 0, 2, ref rect, null, 0, null);
					rect.bottom = rect2.bottom;
					rect.right = rect2.left + 1;
					IntUnsafeNativeMethods.ExtTextOut(new HandleRef(null, dc), 0, 0, 2, ref rect, null, 0, null);
					SafeNativeMethods.SetBkColor(new HandleRef(null, dc), ColorTranslator.ToWin32(SystemColors.ControlDark));
					rect.left = rect2.left;
					rect.right = rect2.right;
					rect.top = rect2.bottom - 1;
					rect.bottom = rect2.bottom;
					IntUnsafeNativeMethods.ExtTextOut(new HandleRef(null, dc), 0, 0, 2, ref rect, null, 0, null);
					rect.left = rect2.right - 1;
					rect.top = rect2.top;
					IntUnsafeNativeMethods.ExtTextOut(new HandleRef(null, dc), 0, 0, 2, ref rect, null, 0, null);
					SafeNativeMethods.SetBkColor(new HandleRef(null, dc), clr);
				}
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.SelectObject(new HandleRef(null, dc), new HandleRef(null, intPtr));
				}
			}

			// Token: 0x060064B9 RID: 25785 RVA: 0x00171008 File Offset: 0x00170008
			protected override void OnHandleCreated(EventArgs e)
			{
				base.OnHandleCreated(e);
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4380, 0, 0);
				num += 6;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 4379, num, 0);
				if (this.hbrushDither == IntPtr.Zero)
				{
					this.CreateDitherBrush();
				}
			}

			// Token: 0x060064BA RID: 25786 RVA: 0x00171070 File Offset: 0x00170070
			private void OnCustomDraw(ref Message m)
			{
				NativeMethods.NMTVCUSTOMDRAW nmtvcustomdraw = (NativeMethods.NMTVCUSTOMDRAW)m.GetLParam(typeof(NativeMethods.NMTVCUSTOMDRAW));
				int dwDrawStage = nmtvcustomdraw.nmcd.dwDrawStage;
				switch (dwDrawStage)
				{
				case 1:
					m.Result = (IntPtr)48;
					return;
				case 2:
					m.Result = (IntPtr)4;
					return;
				default:
				{
					if (dwDrawStage != 65537)
					{
						m.Result = (IntPtr)0;
						return;
					}
					TreeNode treeNode = TreeNode.FromHandle(this, nmtvcustomdraw.nmcd.dwItemSpec);
					if (treeNode != null)
					{
						int num = 0;
						int uItemState = nmtvcustomdraw.nmcd.uItemState;
						if ((uItemState & 64) != 0 || (uItemState & 16) != 0)
						{
							num |= 2;
						}
						if ((uItemState & 1) != 0)
						{
							num |= 1;
						}
						this.DrawTreeItem(treeNode.Text, treeNode.ImageIndex, nmtvcustomdraw.nmcd.hdc, nmtvcustomdraw.nmcd.rc, num, ColorTranslator.ToWin32(SystemColors.Control), ColorTranslator.ToWin32(SystemColors.ControlText));
					}
					m.Result = (IntPtr)4;
					return;
				}
				}
			}

			// Token: 0x060064BB RID: 25787 RVA: 0x0017116C File Offset: 0x0017016C
			protected override void OnHandleDestroyed(EventArgs e)
			{
				base.OnHandleDestroyed(e);
				if (!base.RecreatingHandle && this.hbrushDither != IntPtr.Zero)
				{
					SafeNativeMethods.DeleteObject(new HandleRef(this, this.hbrushDither));
					this.hbrushDither = IntPtr.Zero;
				}
			}

			// Token: 0x060064BC RID: 25788 RVA: 0x001711AC File Offset: 0x001701AC
			private void FillRectDither(IntPtr dc, NativeMethods.RECT rc)
			{
				IntPtr value = SafeNativeMethods.SelectObject(new HandleRef(null, dc), new HandleRef(this, this.hbrushDither));
				if (value != IntPtr.Zero)
				{
					int crColor = SafeNativeMethods.SetTextColor(new HandleRef(null, dc), ColorTranslator.ToWin32(SystemColors.ControlLightLight));
					int clr = SafeNativeMethods.SetBkColor(new HandleRef(null, dc), ColorTranslator.ToWin32(SystemColors.Control));
					SafeNativeMethods.PatBlt(new HandleRef(null, dc), rc.left, rc.top, rc.right - rc.left, rc.bottom - rc.top, 15728673);
					SafeNativeMethods.SetTextColor(new HandleRef(null, dc), crColor);
					SafeNativeMethods.SetBkColor(new HandleRef(null, dc), clr);
				}
			}

			// Token: 0x060064BD RID: 25789 RVA: 0x0017126C File Offset: 0x0017026C
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == 8270 && ((NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR))).code == -12)
				{
					this.OnCustomDraw(ref m);
					return;
				}
				base.WndProc(ref m);
			}

			// Token: 0x04003BF4 RID: 15348
			private const int PADDING_VERT = 3;

			// Token: 0x04003BF5 RID: 15349
			private const int PADDING_HORZ = 4;

			// Token: 0x04003BF6 RID: 15350
			private const int SIZE_ICON_X = 16;

			// Token: 0x04003BF7 RID: 15351
			private const int SIZE_ICON_Y = 16;

			// Token: 0x04003BF8 RID: 15352
			private const int STATE_NORMAL = 0;

			// Token: 0x04003BF9 RID: 15353
			private const int STATE_SELECTED = 1;

			// Token: 0x04003BFA RID: 15354
			private const int STATE_HOT = 2;

			// Token: 0x04003BFB RID: 15355
			private IntPtr hbrushDither;
		}
	}
}

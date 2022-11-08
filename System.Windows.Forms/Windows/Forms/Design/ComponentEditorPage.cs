using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200077E RID: 1918
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class ComponentEditorPage : Panel
	{
		// Token: 0x060064BE RID: 25790 RVA: 0x001712B6 File Offset: 0x001702B6
		public ComponentEditorPage()
		{
			this.commitOnDeactivate = false;
			this.firstActivate = true;
			this.loadRequired = false;
			this.loading = 0;
			base.Visible = false;
		}

		// Token: 0x17001535 RID: 5429
		// (get) Token: 0x060064BF RID: 25791 RVA: 0x001712E1 File Offset: 0x001702E1
		// (set) Token: 0x060064C0 RID: 25792 RVA: 0x001712E9 File Offset: 0x001702E9
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

		// Token: 0x140003F6 RID: 1014
		// (add) Token: 0x060064C1 RID: 25793 RVA: 0x001712F2 File Offset: 0x001702F2
		// (remove) Token: 0x060064C2 RID: 25794 RVA: 0x001712FB File Offset: 0x001702FB
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

		// Token: 0x17001536 RID: 5430
		// (get) Token: 0x060064C3 RID: 25795 RVA: 0x00171304 File Offset: 0x00170304
		// (set) Token: 0x060064C4 RID: 25796 RVA: 0x0017130C File Offset: 0x0017030C
		protected IComponentEditorPageSite PageSite
		{
			get
			{
				return this.pageSite;
			}
			set
			{
				this.pageSite = value;
			}
		}

		// Token: 0x17001537 RID: 5431
		// (get) Token: 0x060064C5 RID: 25797 RVA: 0x00171315 File Offset: 0x00170315
		// (set) Token: 0x060064C6 RID: 25798 RVA: 0x0017131D File Offset: 0x0017031D
		protected IComponent Component
		{
			get
			{
				return this.component;
			}
			set
			{
				this.component = value;
			}
		}

		// Token: 0x17001538 RID: 5432
		// (get) Token: 0x060064C7 RID: 25799 RVA: 0x00171326 File Offset: 0x00170326
		// (set) Token: 0x060064C8 RID: 25800 RVA: 0x0017132E File Offset: 0x0017032E
		protected bool FirstActivate
		{
			get
			{
				return this.firstActivate;
			}
			set
			{
				this.firstActivate = value;
			}
		}

		// Token: 0x17001539 RID: 5433
		// (get) Token: 0x060064C9 RID: 25801 RVA: 0x00171337 File Offset: 0x00170337
		// (set) Token: 0x060064CA RID: 25802 RVA: 0x0017133F File Offset: 0x0017033F
		protected bool LoadRequired
		{
			get
			{
				return this.loadRequired;
			}
			set
			{
				this.loadRequired = value;
			}
		}

		// Token: 0x1700153A RID: 5434
		// (get) Token: 0x060064CB RID: 25803 RVA: 0x00171348 File Offset: 0x00170348
		// (set) Token: 0x060064CC RID: 25804 RVA: 0x00171350 File Offset: 0x00170350
		protected int Loading
		{
			get
			{
				return this.loading;
			}
			set
			{
				this.loading = value;
			}
		}

		// Token: 0x1700153B RID: 5435
		// (get) Token: 0x060064CD RID: 25805 RVA: 0x00171359 File Offset: 0x00170359
		// (set) Token: 0x060064CE RID: 25806 RVA: 0x00171361 File Offset: 0x00170361
		public bool CommitOnDeactivate
		{
			get
			{
				return this.commitOnDeactivate;
			}
			set
			{
				this.commitOnDeactivate = value;
			}
		}

		// Token: 0x1700153C RID: 5436
		// (get) Token: 0x060064CF RID: 25807 RVA: 0x0017136C File Offset: 0x0017036C
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style &= -12582913;
				return createParams;
			}
		}

		// Token: 0x1700153D RID: 5437
		// (get) Token: 0x060064D0 RID: 25808 RVA: 0x00171393 File Offset: 0x00170393
		// (set) Token: 0x060064D1 RID: 25809 RVA: 0x001713BD File Offset: 0x001703BD
		public Icon Icon
		{
			get
			{
				if (this.icon == null)
				{
					this.icon = new Icon(typeof(ComponentEditorPage), "ComponentEditorPage.ico");
				}
				return this.icon;
			}
			set
			{
				this.icon = value;
			}
		}

		// Token: 0x1700153E RID: 5438
		// (get) Token: 0x060064D2 RID: 25810 RVA: 0x001713C6 File Offset: 0x001703C6
		public virtual string Title
		{
			get
			{
				return base.Text;
			}
		}

		// Token: 0x060064D3 RID: 25811 RVA: 0x001713CE File Offset: 0x001703CE
		public virtual void Activate()
		{
			if (this.loadRequired)
			{
				this.EnterLoadingMode();
				this.LoadComponent();
				this.ExitLoadingMode();
				this.loadRequired = false;
			}
			base.Visible = true;
			this.firstActivate = false;
		}

		// Token: 0x060064D4 RID: 25812 RVA: 0x001713FF File Offset: 0x001703FF
		public virtual void ApplyChanges()
		{
			this.SaveComponent();
		}

		// Token: 0x060064D5 RID: 25813 RVA: 0x00171407 File Offset: 0x00170407
		public virtual void Deactivate()
		{
			base.Visible = false;
		}

		// Token: 0x060064D6 RID: 25814 RVA: 0x00171410 File Offset: 0x00170410
		protected void EnterLoadingMode()
		{
			this.loading++;
		}

		// Token: 0x060064D7 RID: 25815 RVA: 0x00171420 File Offset: 0x00170420
		protected void ExitLoadingMode()
		{
			this.loading--;
		}

		// Token: 0x060064D8 RID: 25816 RVA: 0x00171430 File Offset: 0x00170430
		public virtual Control GetControl()
		{
			return this;
		}

		// Token: 0x060064D9 RID: 25817 RVA: 0x00171433 File Offset: 0x00170433
		protected IComponent GetSelectedComponent()
		{
			return this.component;
		}

		// Token: 0x060064DA RID: 25818 RVA: 0x0017143B File Offset: 0x0017043B
		public virtual bool IsPageMessage(ref Message msg)
		{
			return this.PreProcessMessage(ref msg);
		}

		// Token: 0x060064DB RID: 25819 RVA: 0x00171444 File Offset: 0x00170444
		protected bool IsFirstActivate()
		{
			return this.firstActivate;
		}

		// Token: 0x060064DC RID: 25820 RVA: 0x0017144C File Offset: 0x0017044C
		protected bool IsLoading()
		{
			return this.loading != 0;
		}

		// Token: 0x060064DD RID: 25821
		protected abstract void LoadComponent();

		// Token: 0x060064DE RID: 25822 RVA: 0x0017145A File Offset: 0x0017045A
		public virtual void OnApplyComplete()
		{
			this.ReloadComponent();
		}

		// Token: 0x060064DF RID: 25823 RVA: 0x00171462 File Offset: 0x00170462
		protected virtual void ReloadComponent()
		{
			if (!base.Visible)
			{
				this.loadRequired = true;
			}
		}

		// Token: 0x060064E0 RID: 25824
		protected abstract void SaveComponent();

		// Token: 0x060064E1 RID: 25825 RVA: 0x00171473 File Offset: 0x00170473
		protected virtual void SetDirty()
		{
			if (!this.IsLoading())
			{
				this.pageSite.SetDirty();
			}
		}

		// Token: 0x060064E2 RID: 25826 RVA: 0x00171488 File Offset: 0x00170488
		public virtual void SetComponent(IComponent component)
		{
			this.component = component;
			this.loadRequired = true;
		}

		// Token: 0x060064E3 RID: 25827 RVA: 0x00171498 File Offset: 0x00170498
		public virtual void SetSite(IComponentEditorPageSite site)
		{
			this.pageSite = site;
			this.pageSite.GetControl().Controls.Add(this);
		}

		// Token: 0x060064E4 RID: 25828 RVA: 0x001714B7 File Offset: 0x001704B7
		public virtual void ShowHelp()
		{
		}

		// Token: 0x060064E5 RID: 25829 RVA: 0x001714B9 File Offset: 0x001704B9
		public virtual bool SupportsHelp()
		{
			return false;
		}

		// Token: 0x04003BFC RID: 15356
		private IComponentEditorPageSite pageSite;

		// Token: 0x04003BFD RID: 15357
		private IComponent component;

		// Token: 0x04003BFE RID: 15358
		private bool firstActivate;

		// Token: 0x04003BFF RID: 15359
		private bool loadRequired;

		// Token: 0x04003C00 RID: 15360
		private int loading;

		// Token: 0x04003C01 RID: 15361
		private Icon icon;

		// Token: 0x04003C02 RID: 15362
		private bool commitOnDeactivate;
	}
}

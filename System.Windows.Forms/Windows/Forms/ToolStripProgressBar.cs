using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020006D4 RID: 1748
	[DefaultProperty("Value")]
	public class ToolStripProgressBar : ToolStripControlHost
	{
		// Token: 0x06005C2B RID: 23595 RVA: 0x00150194 File Offset: 0x0014F194
		public ToolStripProgressBar() : base(ToolStripProgressBar.CreateControlInstance())
		{
		}

		// Token: 0x06005C2C RID: 23596 RVA: 0x001501A1 File Offset: 0x0014F1A1
		public ToolStripProgressBar(string name) : this()
		{
			base.Name = name;
		}

		// Token: 0x17001347 RID: 4935
		// (get) Token: 0x06005C2D RID: 23597 RVA: 0x001501B0 File Offset: 0x0014F1B0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ProgressBar ProgressBar
		{
			get
			{
				return base.Control as ProgressBar;
			}
		}

		// Token: 0x17001348 RID: 4936
		// (get) Token: 0x06005C2E RID: 23598 RVA: 0x001501BD File Offset: 0x0014F1BD
		// (set) Token: 0x06005C2F RID: 23599 RVA: 0x001501C5 File Offset: 0x0014F1C5
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		// Token: 0x17001349 RID: 4937
		// (get) Token: 0x06005C30 RID: 23600 RVA: 0x001501CE File Offset: 0x0014F1CE
		// (set) Token: 0x06005C31 RID: 23601 RVA: 0x001501D6 File Offset: 0x0014F1D6
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		// Token: 0x1700134A RID: 4938
		// (get) Token: 0x06005C32 RID: 23602 RVA: 0x001501DF File Offset: 0x0014F1DF
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 15);
			}
		}

		// Token: 0x1700134B RID: 4939
		// (get) Token: 0x06005C33 RID: 23603 RVA: 0x001501EA File Offset: 0x0014F1EA
		protected internal override Padding DefaultMargin
		{
			get
			{
				if (base.Owner != null && base.Owner is StatusStrip)
				{
					return new Padding(1, 3, 1, 3);
				}
				return new Padding(1, 2, 1, 1);
			}
		}

		// Token: 0x1700134C RID: 4940
		// (get) Token: 0x06005C34 RID: 23604 RVA: 0x00150214 File Offset: 0x0014F214
		// (set) Token: 0x06005C35 RID: 23605 RVA: 0x00150221 File Offset: 0x0014F221
		[SRDescription("ProgressBarMarqueeAnimationSpeed")]
		[SRCategory("CatBehavior")]
		[DefaultValue(100)]
		public int MarqueeAnimationSpeed
		{
			get
			{
				return this.ProgressBar.MarqueeAnimationSpeed;
			}
			set
			{
				this.ProgressBar.MarqueeAnimationSpeed = value;
			}
		}

		// Token: 0x1700134D RID: 4941
		// (get) Token: 0x06005C36 RID: 23606 RVA: 0x0015022F File Offset: 0x0014F22F
		// (set) Token: 0x06005C37 RID: 23607 RVA: 0x0015023C File Offset: 0x0014F23C
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(100)]
		[SRDescription("ProgressBarMaximumDescr")]
		[SRCategory("CatBehavior")]
		public int Maximum
		{
			get
			{
				return this.ProgressBar.Maximum;
			}
			set
			{
				this.ProgressBar.Maximum = value;
			}
		}

		// Token: 0x1700134E RID: 4942
		// (get) Token: 0x06005C38 RID: 23608 RVA: 0x0015024A File Offset: 0x0014F24A
		// (set) Token: 0x06005C39 RID: 23609 RVA: 0x00150257 File Offset: 0x0014F257
		[SRDescription("ProgressBarMinimumDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(0)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Minimum
		{
			get
			{
				return this.ProgressBar.Minimum;
			}
			set
			{
				this.ProgressBar.Minimum = value;
			}
		}

		// Token: 0x1700134F RID: 4943
		// (get) Token: 0x06005C3A RID: 23610 RVA: 0x00150265 File Offset: 0x0014F265
		// (set) Token: 0x06005C3B RID: 23611 RVA: 0x00150272 File Offset: 0x0014F272
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("ControlRightToLeftLayoutDescr")]
		public virtual bool RightToLeftLayout
		{
			get
			{
				return this.ProgressBar.RightToLeftLayout;
			}
			set
			{
				this.ProgressBar.RightToLeftLayout = value;
			}
		}

		// Token: 0x17001350 RID: 4944
		// (get) Token: 0x06005C3C RID: 23612 RVA: 0x00150280 File Offset: 0x0014F280
		// (set) Token: 0x06005C3D RID: 23613 RVA: 0x0015028D File Offset: 0x0014F28D
		[SRCategory("CatBehavior")]
		[DefaultValue(10)]
		[SRDescription("ProgressBarStepDescr")]
		public int Step
		{
			get
			{
				return this.ProgressBar.Step;
			}
			set
			{
				this.ProgressBar.Step = value;
			}
		}

		// Token: 0x17001351 RID: 4945
		// (get) Token: 0x06005C3E RID: 23614 RVA: 0x0015029B File Offset: 0x0014F29B
		// (set) Token: 0x06005C3F RID: 23615 RVA: 0x001502A8 File Offset: 0x0014F2A8
		[SRCategory("CatBehavior")]
		[DefaultValue(ProgressBarStyle.Blocks)]
		[SRDescription("ProgressBarStyleDescr")]
		public ProgressBarStyle Style
		{
			get
			{
				return this.ProgressBar.Style;
			}
			set
			{
				this.ProgressBar.Style = value;
			}
		}

		// Token: 0x17001352 RID: 4946
		// (get) Token: 0x06005C40 RID: 23616 RVA: 0x001502B6 File Offset: 0x0014F2B6
		// (set) Token: 0x06005C41 RID: 23617 RVA: 0x001502C3 File Offset: 0x0014F2C3
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override string Text
		{
			get
			{
				return base.Control.Text;
			}
			set
			{
				base.Control.Text = value;
			}
		}

		// Token: 0x17001353 RID: 4947
		// (get) Token: 0x06005C42 RID: 23618 RVA: 0x001502D1 File Offset: 0x0014F2D1
		// (set) Token: 0x06005C43 RID: 23619 RVA: 0x001502DE File Offset: 0x0014F2DE
		[DefaultValue(0)]
		[SRCategory("CatBehavior")]
		[Bindable(true)]
		[SRDescription("ProgressBarValueDescr")]
		public int Value
		{
			get
			{
				return this.ProgressBar.Value;
			}
			set
			{
				this.ProgressBar.Value = value;
			}
		}

		// Token: 0x06005C44 RID: 23620 RVA: 0x001502EC File Offset: 0x0014F2EC
		private static Control CreateControlInstance()
		{
			return new ProgressBar
			{
				Size = new Size(100, 15)
			};
		}

		// Token: 0x06005C45 RID: 23621 RVA: 0x0015030F File Offset: 0x0014F30F
		private void HandleRightToLeftLayoutChanged(object sender, EventArgs e)
		{
			this.OnRightToLeftLayoutChanged(e);
		}

		// Token: 0x06005C46 RID: 23622 RVA: 0x00150318 File Offset: 0x0014F318
		protected virtual void OnRightToLeftLayoutChanged(EventArgs e)
		{
			base.RaiseEvent(ToolStripProgressBar.EventRightToLeftLayoutChanged, e);
		}

		// Token: 0x06005C47 RID: 23623 RVA: 0x00150328 File Offset: 0x0014F328
		protected override void OnSubscribeControlEvents(Control control)
		{
			ProgressBar progressBar = control as ProgressBar;
			if (progressBar != null)
			{
				progressBar.RightToLeftLayoutChanged += this.HandleRightToLeftLayoutChanged;
			}
			base.OnSubscribeControlEvents(control);
		}

		// Token: 0x06005C48 RID: 23624 RVA: 0x00150358 File Offset: 0x0014F358
		protected override void OnUnsubscribeControlEvents(Control control)
		{
			ProgressBar progressBar = control as ProgressBar;
			if (progressBar != null)
			{
				progressBar.RightToLeftLayoutChanged -= this.HandleRightToLeftLayoutChanged;
			}
			base.OnUnsubscribeControlEvents(control);
		}

		// Token: 0x14000374 RID: 884
		// (add) Token: 0x06005C49 RID: 23625 RVA: 0x00150388 File Offset: 0x0014F388
		// (remove) Token: 0x06005C4A RID: 23626 RVA: 0x00150391 File Offset: 0x0014F391
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyDown
		{
			add
			{
				base.KeyDown += value;
			}
			remove
			{
				base.KeyDown -= value;
			}
		}

		// Token: 0x14000375 RID: 885
		// (add) Token: 0x06005C4B RID: 23627 RVA: 0x0015039A File Offset: 0x0014F39A
		// (remove) Token: 0x06005C4C RID: 23628 RVA: 0x001503A3 File Offset: 0x0014F3A3
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				base.KeyPress += value;
			}
			remove
			{
				base.KeyPress -= value;
			}
		}

		// Token: 0x14000376 RID: 886
		// (add) Token: 0x06005C4D RID: 23629 RVA: 0x001503AC File Offset: 0x0014F3AC
		// (remove) Token: 0x06005C4E RID: 23630 RVA: 0x001503B5 File Offset: 0x0014F3B5
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event KeyEventHandler KeyUp
		{
			add
			{
				base.KeyUp += value;
			}
			remove
			{
				base.KeyUp -= value;
			}
		}

		// Token: 0x14000377 RID: 887
		// (add) Token: 0x06005C4F RID: 23631 RVA: 0x001503BE File Offset: 0x0014F3BE
		// (remove) Token: 0x06005C50 RID: 23632 RVA: 0x001503C7 File Offset: 0x0014F3C7
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler LocationChanged
		{
			add
			{
				base.LocationChanged += value;
			}
			remove
			{
				base.LocationChanged -= value;
			}
		}

		// Token: 0x14000378 RID: 888
		// (add) Token: 0x06005C51 RID: 23633 RVA: 0x001503D0 File Offset: 0x0014F3D0
		// (remove) Token: 0x06005C52 RID: 23634 RVA: 0x001503D9 File Offset: 0x0014F3D9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler OwnerChanged
		{
			add
			{
				base.OwnerChanged += value;
			}
			remove
			{
				base.OwnerChanged -= value;
			}
		}

		// Token: 0x14000379 RID: 889
		// (add) Token: 0x06005C53 RID: 23635 RVA: 0x001503E2 File Offset: 0x0014F3E2
		// (remove) Token: 0x06005C54 RID: 23636 RVA: 0x001503F5 File Offset: 0x0014F3F5
		[SRDescription("ControlOnRightToLeftLayoutChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripProgressBar.EventRightToLeftLayoutChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripProgressBar.EventRightToLeftLayoutChanged, value);
			}
		}

		// Token: 0x1400037A RID: 890
		// (add) Token: 0x06005C55 RID: 23637 RVA: 0x00150408 File Offset: 0x0014F408
		// (remove) Token: 0x06005C56 RID: 23638 RVA: 0x00150411 File Offset: 0x0014F411
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		// Token: 0x1400037B RID: 891
		// (add) Token: 0x06005C57 RID: 23639 RVA: 0x0015041A File Offset: 0x0014F41A
		// (remove) Token: 0x06005C58 RID: 23640 RVA: 0x00150423 File Offset: 0x0014F423
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler Validated
		{
			add
			{
				base.Validated += value;
			}
			remove
			{
				base.Validated -= value;
			}
		}

		// Token: 0x1400037C RID: 892
		// (add) Token: 0x06005C59 RID: 23641 RVA: 0x0015042C File Offset: 0x0014F42C
		// (remove) Token: 0x06005C5A RID: 23642 RVA: 0x00150435 File Offset: 0x0014F435
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event CancelEventHandler Validating
		{
			add
			{
				base.Validating += value;
			}
			remove
			{
				base.Validating -= value;
			}
		}

		// Token: 0x06005C5B RID: 23643 RVA: 0x0015043E File Offset: 0x0014F43E
		public void Increment(int value)
		{
			this.ProgressBar.Increment(value);
		}

		// Token: 0x06005C5C RID: 23644 RVA: 0x0015044C File Offset: 0x0014F44C
		public void PerformStep()
		{
			this.ProgressBar.PerformStep();
		}

		// Token: 0x0400390E RID: 14606
		internal static readonly object EventRightToLeftLayoutChanged = new object();
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000677 RID: 1655
	public class ToolStripControlHost : ToolStripItem
	{
		// Token: 0x060056CF RID: 22223 RVA: 0x0013BBBC File Offset: 0x0013ABBC
		public ToolStripControlHost(Control c)
		{
			if (c == null)
			{
				throw new ArgumentNullException("c", "ControlCannotBeNull");
			}
			this.control = c;
			this.SyncControlParent();
			c.Visible = true;
			this.SetBounds(c.Bounds);
			Rectangle bounds = this.Bounds;
			CommonProperties.UpdateSpecifiedBounds(c, bounds.X, bounds.Y, bounds.Width, bounds.Height);
			this.OnSubscribeControlEvents(c);
		}

		// Token: 0x060056D0 RID: 22224 RVA: 0x0013BC3A File Offset: 0x0013AC3A
		public ToolStripControlHost(Control c, string name) : this(c)
		{
			base.Name = name;
		}

		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x060056D1 RID: 22225 RVA: 0x0013BC4A File Offset: 0x0013AC4A
		// (set) Token: 0x060056D2 RID: 22226 RVA: 0x0013BC57 File Offset: 0x0013AC57
		public override Color BackColor
		{
			get
			{
				return this.Control.BackColor;
			}
			set
			{
				this.Control.BackColor = value;
			}
		}

		// Token: 0x1700120C RID: 4620
		// (get) Token: 0x060056D3 RID: 22227 RVA: 0x0013BC65 File Offset: 0x0013AC65
		// (set) Token: 0x060056D4 RID: 22228 RVA: 0x0013BC72 File Offset: 0x0013AC72
		[Localizable(true)]
		[SRDescription("ToolStripItemImageDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(null)]
		public override Image BackgroundImage
		{
			get
			{
				return this.Control.BackgroundImage;
			}
			set
			{
				this.Control.BackgroundImage = value;
			}
		}

		// Token: 0x1700120D RID: 4621
		// (get) Token: 0x060056D5 RID: 22229 RVA: 0x0013BC80 File Offset: 0x0013AC80
		// (set) Token: 0x060056D6 RID: 22230 RVA: 0x0013BC8D File Offset: 0x0013AC8D
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("ControlBackgroundImageLayoutDescr")]
		[DefaultValue(ImageLayout.Tile)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return this.Control.BackgroundImageLayout;
			}
			set
			{
				this.Control.BackgroundImageLayout = value;
			}
		}

		// Token: 0x1700120E RID: 4622
		// (get) Token: 0x060056D7 RID: 22231 RVA: 0x0013BC9B File Offset: 0x0013AC9B
		public override bool CanSelect
		{
			get
			{
				return this.control != null && (base.DesignMode || this.Control.CanSelect);
			}
		}

		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x060056D8 RID: 22232 RVA: 0x0013BCBC File Offset: 0x0013ACBC
		// (set) Token: 0x060056D9 RID: 22233 RVA: 0x0013BCC9 File Offset: 0x0013ACC9
		[DefaultValue(true)]
		[SRDescription("ControlCausesValidationDescr")]
		[SRCategory("CatFocus")]
		public bool CausesValidation
		{
			get
			{
				return this.Control.CausesValidation;
			}
			set
			{
				this.Control.CausesValidation = value;
			}
		}

		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x060056DA RID: 22234 RVA: 0x0013BCD7 File Offset: 0x0013ACD7
		// (set) Token: 0x060056DB RID: 22235 RVA: 0x0013BCDF File Offset: 0x0013ACDF
		[Browsable(false)]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		public ContentAlignment ControlAlign
		{
			get
			{
				return this.controlAlign;
			}
			set
			{
				if (!WindowsFormsUtils.EnumValidator.IsValidContentAlignment(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
				}
				if (this.controlAlign != value)
				{
					this.controlAlign = value;
					this.OnBoundsChanged();
				}
			}
		}

		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x060056DC RID: 22236 RVA: 0x0013BD15 File Offset: 0x0013AD15
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Control Control
		{
			get
			{
				return this.control;
			}
		}

		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x060056DD RID: 22237 RVA: 0x0013BD1D File Offset: 0x0013AD1D
		protected override Size DefaultSize
		{
			get
			{
				if (this.Control != null)
				{
					return this.Control.Size;
				}
				return base.DefaultSize;
			}
		}

		// Token: 0x17001213 RID: 4627
		// (get) Token: 0x060056DE RID: 22238 RVA: 0x0013BD39 File Offset: 0x0013AD39
		// (set) Token: 0x060056DF RID: 22239 RVA: 0x0013BD41 File Offset: 0x0013AD41
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new ToolStripItemDisplayStyle DisplayStyle
		{
			get
			{
				return base.DisplayStyle;
			}
			set
			{
				base.DisplayStyle = value;
			}
		}

		// Token: 0x1400033B RID: 827
		// (add) Token: 0x060056E0 RID: 22240 RVA: 0x0013BD4A File Offset: 0x0013AD4A
		// (remove) Token: 0x060056E1 RID: 22241 RVA: 0x0013BD5D File Offset: 0x0013AD5D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler DisplayStyleChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripItem.EventDisplayStyleChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripItem.EventDisplayStyleChanged, value);
			}
		}

		// Token: 0x17001214 RID: 4628
		// (get) Token: 0x060056E2 RID: 22242 RVA: 0x0013BD70 File Offset: 0x0013AD70
		// (set) Token: 0x060056E3 RID: 22243 RVA: 0x0013BD78 File Offset: 0x0013AD78
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DefaultValue(false)]
		public new bool DoubleClickEnabled
		{
			get
			{
				return base.DoubleClickEnabled;
			}
			set
			{
				base.DoubleClickEnabled = value;
			}
		}

		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x060056E4 RID: 22244 RVA: 0x0013BD81 File Offset: 0x0013AD81
		// (set) Token: 0x060056E5 RID: 22245 RVA: 0x0013BD8E File Offset: 0x0013AD8E
		public override Font Font
		{
			get
			{
				return this.Control.Font;
			}
			set
			{
				this.Control.Font = value;
			}
		}

		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x060056E6 RID: 22246 RVA: 0x0013BD9C File Offset: 0x0013AD9C
		// (set) Token: 0x060056E7 RID: 22247 RVA: 0x0013BDA9 File Offset: 0x0013ADA9
		public override bool Enabled
		{
			get
			{
				return this.Control.Enabled;
			}
			set
			{
				this.Control.Enabled = value;
			}
		}

		// Token: 0x1400033C RID: 828
		// (add) Token: 0x060056E8 RID: 22248 RVA: 0x0013BDB7 File Offset: 0x0013ADB7
		// (remove) Token: 0x060056E9 RID: 22249 RVA: 0x0013BDCA File Offset: 0x0013ADCA
		[SRDescription("ControlOnEnterDescr")]
		[SRCategory("CatFocus")]
		public event EventHandler Enter
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventEnter, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventEnter, value);
			}
		}

		// Token: 0x17001217 RID: 4631
		// (get) Token: 0x060056EA RID: 22250 RVA: 0x0013BDDD File Offset: 0x0013ADDD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public virtual bool Focused
		{
			get
			{
				return this.Control.Focused;
			}
		}

		// Token: 0x17001218 RID: 4632
		// (get) Token: 0x060056EB RID: 22251 RVA: 0x0013BDEA File Offset: 0x0013ADEA
		// (set) Token: 0x060056EC RID: 22252 RVA: 0x0013BDF7 File Offset: 0x0013ADF7
		public override Color ForeColor
		{
			get
			{
				return this.Control.ForeColor;
			}
			set
			{
				this.Control.ForeColor = value;
			}
		}

		// Token: 0x1400033D RID: 829
		// (add) Token: 0x060056ED RID: 22253 RVA: 0x0013BE05 File Offset: 0x0013AE05
		// (remove) Token: 0x060056EE RID: 22254 RVA: 0x0013BE18 File Offset: 0x0013AE18
		[SRCategory("CatFocus")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[SRDescription("ToolStripItemOnGotFocusDescr")]
		public event EventHandler GotFocus
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventGotFocus, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventGotFocus, value);
			}
		}

		// Token: 0x17001219 RID: 4633
		// (get) Token: 0x060056EF RID: 22255 RVA: 0x0013BE2B File Offset: 0x0013AE2B
		// (set) Token: 0x060056F0 RID: 22256 RVA: 0x0013BE33 File Offset: 0x0013AE33
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Image Image
		{
			get
			{
				return base.Image;
			}
			set
			{
				base.Image = value;
			}
		}

		// Token: 0x1700121A RID: 4634
		// (get) Token: 0x060056F1 RID: 22257 RVA: 0x0013BE3C File Offset: 0x0013AE3C
		// (set) Token: 0x060056F2 RID: 22258 RVA: 0x0013BE44 File Offset: 0x0013AE44
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ToolStripItemImageScaling ImageScaling
		{
			get
			{
				return base.ImageScaling;
			}
			set
			{
				base.ImageScaling = value;
			}
		}

		// Token: 0x1700121B RID: 4635
		// (get) Token: 0x060056F3 RID: 22259 RVA: 0x0013BE4D File Offset: 0x0013AE4D
		// (set) Token: 0x060056F4 RID: 22260 RVA: 0x0013BE55 File Offset: 0x0013AE55
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Color ImageTransparentColor
		{
			get
			{
				return base.ImageTransparentColor;
			}
			set
			{
				base.ImageTransparentColor = value;
			}
		}

		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x060056F5 RID: 22261 RVA: 0x0013BE5E File Offset: 0x0013AE5E
		// (set) Token: 0x060056F6 RID: 22262 RVA: 0x0013BE66 File Offset: 0x0013AE66
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new ContentAlignment ImageAlign
		{
			get
			{
				return base.ImageAlign;
			}
			set
			{
				base.ImageAlign = value;
			}
		}

		// Token: 0x1400033E RID: 830
		// (add) Token: 0x060056F7 RID: 22263 RVA: 0x0013BE6F File Offset: 0x0013AE6F
		// (remove) Token: 0x060056F8 RID: 22264 RVA: 0x0013BE82 File Offset: 0x0013AE82
		[SRDescription("ControlOnLeaveDescr")]
		[SRCategory("CatFocus")]
		public event EventHandler Leave
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventLeave, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventLeave, value);
			}
		}

		// Token: 0x1400033F RID: 831
		// (add) Token: 0x060056F9 RID: 22265 RVA: 0x0013BE95 File Offset: 0x0013AE95
		// (remove) Token: 0x060056FA RID: 22266 RVA: 0x0013BEA8 File Offset: 0x0013AEA8
		[SRCategory("CatFocus")]
		[SRDescription("ToolStripItemOnLostFocusDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler LostFocus
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventLostFocus, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventLostFocus, value);
			}
		}

		// Token: 0x14000340 RID: 832
		// (add) Token: 0x060056FB RID: 22267 RVA: 0x0013BEBB File Offset: 0x0013AEBB
		// (remove) Token: 0x060056FC RID: 22268 RVA: 0x0013BECE File Offset: 0x0013AECE
		[SRDescription("ControlOnKeyDownDescr")]
		[SRCategory("CatKey")]
		public event KeyEventHandler KeyDown
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventKeyDown, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventKeyDown, value);
			}
		}

		// Token: 0x14000341 RID: 833
		// (add) Token: 0x060056FD RID: 22269 RVA: 0x0013BEE1 File Offset: 0x0013AEE1
		// (remove) Token: 0x060056FE RID: 22270 RVA: 0x0013BEF4 File Offset: 0x0013AEF4
		[SRDescription("ControlOnKeyPressDescr")]
		[SRCategory("CatKey")]
		public event KeyPressEventHandler KeyPress
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventKeyPress, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventKeyPress, value);
			}
		}

		// Token: 0x14000342 RID: 834
		// (add) Token: 0x060056FF RID: 22271 RVA: 0x0013BF07 File Offset: 0x0013AF07
		// (remove) Token: 0x06005700 RID: 22272 RVA: 0x0013BF1A File Offset: 0x0013AF1A
		[SRCategory("CatKey")]
		[SRDescription("ControlOnKeyUpDescr")]
		public event KeyEventHandler KeyUp
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventKeyUp, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventKeyUp, value);
			}
		}

		// Token: 0x1700121D RID: 4637
		// (get) Token: 0x06005701 RID: 22273 RVA: 0x0013BF2D File Offset: 0x0013AF2D
		// (set) Token: 0x06005702 RID: 22274 RVA: 0x0013BF49 File Offset: 0x0013AF49
		public override RightToLeft RightToLeft
		{
			get
			{
				if (this.control != null)
				{
					return this.control.RightToLeft;
				}
				return base.RightToLeft;
			}
			set
			{
				if (this.control != null)
				{
					this.control.RightToLeft = value;
				}
			}
		}

		// Token: 0x1700121E RID: 4638
		// (get) Token: 0x06005703 RID: 22275 RVA: 0x0013BF5F File Offset: 0x0013AF5F
		// (set) Token: 0x06005704 RID: 22276 RVA: 0x0013BF67 File Offset: 0x0013AF67
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new bool RightToLeftAutoMirrorImage
		{
			get
			{
				return base.RightToLeftAutoMirrorImage;
			}
			set
			{
				base.RightToLeftAutoMirrorImage = value;
			}
		}

		// Token: 0x1700121F RID: 4639
		// (get) Token: 0x06005705 RID: 22277 RVA: 0x0013BF70 File Offset: 0x0013AF70
		public override bool Selected
		{
			get
			{
				return this.Control != null && this.Control.Focused;
			}
		}

		// Token: 0x17001220 RID: 4640
		// (get) Token: 0x06005706 RID: 22278 RVA: 0x0013BF87 File Offset: 0x0013AF87
		// (set) Token: 0x06005707 RID: 22279 RVA: 0x0013BF90 File Offset: 0x0013AF90
		public override Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				Rectangle right = Rectangle.Empty;
				if (this.control != null)
				{
					right = this.control.Bounds;
					right.Size = value;
					CommonProperties.UpdateSpecifiedBounds(this.control, right.X, right.Y, right.Width, right.Height);
				}
				base.Size = value;
				if (this.control != null)
				{
					Rectangle bounds = this.control.Bounds;
					if (bounds != right)
					{
						CommonProperties.UpdateSpecifiedBounds(this.control, bounds.X, bounds.Y, bounds.Width, bounds.Height);
					}
				}
			}
		}

		// Token: 0x17001221 RID: 4641
		// (get) Token: 0x06005708 RID: 22280 RVA: 0x0013C031 File Offset: 0x0013B031
		// (set) Token: 0x06005709 RID: 22281 RVA: 0x0013C039 File Offset: 0x0013B039
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				base.Site = value;
				if (value != null)
				{
					this.Control.Site = new ToolStripControlHost.StubSite(this.Control, this);
					return;
				}
				this.Control.Site = null;
			}
		}

		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x0600570A RID: 22282 RVA: 0x0013C069 File Offset: 0x0013B069
		// (set) Token: 0x0600570B RID: 22283 RVA: 0x0013C076 File Offset: 0x0013B076
		[DefaultValue("")]
		public override string Text
		{
			get
			{
				return this.Control.Text;
			}
			set
			{
				this.Control.Text = value;
			}
		}

		// Token: 0x17001223 RID: 4643
		// (get) Token: 0x0600570C RID: 22284 RVA: 0x0013C084 File Offset: 0x0013B084
		// (set) Token: 0x0600570D RID: 22285 RVA: 0x0013C08C File Offset: 0x0013B08C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new ContentAlignment TextAlign
		{
			get
			{
				return base.TextAlign;
			}
			set
			{
				base.TextAlign = value;
			}
		}

		// Token: 0x17001224 RID: 4644
		// (get) Token: 0x0600570E RID: 22286 RVA: 0x0013C095 File Offset: 0x0013B095
		// (set) Token: 0x0600570F RID: 22287 RVA: 0x0013C09D File Offset: 0x0013B09D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(ToolStripTextDirection.Horizontal)]
		public override ToolStripTextDirection TextDirection
		{
			get
			{
				return base.TextDirection;
			}
			set
			{
				base.TextDirection = value;
			}
		}

		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x06005710 RID: 22288 RVA: 0x0013C0A6 File Offset: 0x0013B0A6
		// (set) Token: 0x06005711 RID: 22289 RVA: 0x0013C0AE File Offset: 0x0013B0AE
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new TextImageRelation TextImageRelation
		{
			get
			{
				return base.TextImageRelation;
			}
			set
			{
				base.TextImageRelation = value;
			}
		}

		// Token: 0x14000343 RID: 835
		// (add) Token: 0x06005712 RID: 22290 RVA: 0x0013C0B7 File Offset: 0x0013B0B7
		// (remove) Token: 0x06005713 RID: 22291 RVA: 0x0013C0CA File Offset: 0x0013B0CA
		[SRDescription("ControlOnValidatingDescr")]
		[SRCategory("CatFocus")]
		public event CancelEventHandler Validating
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventValidating, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventValidating, value);
			}
		}

		// Token: 0x14000344 RID: 836
		// (add) Token: 0x06005714 RID: 22292 RVA: 0x0013C0DD File Offset: 0x0013B0DD
		// (remove) Token: 0x06005715 RID: 22293 RVA: 0x0013C0F0 File Offset: 0x0013B0F0
		[SRCategory("CatFocus")]
		[SRDescription("ControlOnValidatedDescr")]
		public event EventHandler Validated
		{
			add
			{
				base.Events.AddHandler(ToolStripControlHost.EventValidated, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripControlHost.EventValidated, value);
			}
		}

		// Token: 0x06005716 RID: 22294 RVA: 0x0013C103 File Offset: 0x0013B103
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return this.Control.AccessibilityObject;
		}

		// Token: 0x06005717 RID: 22295 RVA: 0x0013C110 File Offset: 0x0013B110
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing && this.Control != null)
			{
				this.OnUnsubscribeControlEvents(this.Control);
				this.Control.Dispose();
				this.control = null;
			}
		}

		// Token: 0x06005718 RID: 22296 RVA: 0x0013C142 File Offset: 0x0013B142
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void Focus()
		{
			this.Control.Focus();
		}

		// Token: 0x06005719 RID: 22297 RVA: 0x0013C150 File Offset: 0x0013B150
		public override Size GetPreferredSize(Size constrainingSize)
		{
			if (this.control != null)
			{
				return this.Control.GetPreferredSize(constrainingSize - this.Padding.Size) + this.Padding.Size;
			}
			return base.GetPreferredSize(constrainingSize);
		}

		// Token: 0x0600571A RID: 22298 RVA: 0x0013C19F File Offset: 0x0013B19F
		private void HandleClick(object sender, EventArgs e)
		{
			this.OnClick(e);
		}

		// Token: 0x0600571B RID: 22299 RVA: 0x0013C1A8 File Offset: 0x0013B1A8
		private void HandleBackColorChanged(object sender, EventArgs e)
		{
			this.OnBackColorChanged(e);
		}

		// Token: 0x0600571C RID: 22300 RVA: 0x0013C1B1 File Offset: 0x0013B1B1
		private void HandleDoubleClick(object sender, EventArgs e)
		{
			this.OnDoubleClick(e);
		}

		// Token: 0x0600571D RID: 22301 RVA: 0x0013C1BA File Offset: 0x0013B1BA
		private void HandleDragDrop(object sender, DragEventArgs e)
		{
			this.OnDragDrop(e);
		}

		// Token: 0x0600571E RID: 22302 RVA: 0x0013C1C3 File Offset: 0x0013B1C3
		private void HandleDragEnter(object sender, DragEventArgs e)
		{
			this.OnDragEnter(e);
		}

		// Token: 0x0600571F RID: 22303 RVA: 0x0013C1CC File Offset: 0x0013B1CC
		private void HandleDragLeave(object sender, EventArgs e)
		{
			this.OnDragLeave(e);
		}

		// Token: 0x06005720 RID: 22304 RVA: 0x0013C1D5 File Offset: 0x0013B1D5
		private void HandleDragOver(object sender, DragEventArgs e)
		{
			this.OnDragOver(e);
		}

		// Token: 0x06005721 RID: 22305 RVA: 0x0013C1DE File Offset: 0x0013B1DE
		private void HandleEnter(object sender, EventArgs e)
		{
			this.OnEnter(e);
		}

		// Token: 0x06005722 RID: 22306 RVA: 0x0013C1E7 File Offset: 0x0013B1E7
		private void HandleEnabledChanged(object sender, EventArgs e)
		{
			this.OnEnabledChanged(e);
		}

		// Token: 0x06005723 RID: 22307 RVA: 0x0013C1F0 File Offset: 0x0013B1F0
		private void HandleForeColorChanged(object sender, EventArgs e)
		{
			this.OnForeColorChanged(e);
		}

		// Token: 0x06005724 RID: 22308 RVA: 0x0013C1F9 File Offset: 0x0013B1F9
		private void HandleGiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			this.OnGiveFeedback(e);
		}

		// Token: 0x06005725 RID: 22309 RVA: 0x0013C202 File Offset: 0x0013B202
		private void HandleGotFocus(object sender, EventArgs e)
		{
			this.OnGotFocus(e);
		}

		// Token: 0x06005726 RID: 22310 RVA: 0x0013C20B File Offset: 0x0013B20B
		private void HandleLocationChanged(object sender, EventArgs e)
		{
			this.OnLocationChanged(e);
		}

		// Token: 0x06005727 RID: 22311 RVA: 0x0013C214 File Offset: 0x0013B214
		private void HandleLostFocus(object sender, EventArgs e)
		{
			this.OnLostFocus(e);
		}

		// Token: 0x06005728 RID: 22312 RVA: 0x0013C21D File Offset: 0x0013B21D
		private void HandleKeyDown(object sender, KeyEventArgs e)
		{
			this.OnKeyDown(e);
		}

		// Token: 0x06005729 RID: 22313 RVA: 0x0013C226 File Offset: 0x0013B226
		private void HandleKeyPress(object sender, KeyPressEventArgs e)
		{
			this.OnKeyPress(e);
		}

		// Token: 0x0600572A RID: 22314 RVA: 0x0013C22F File Offset: 0x0013B22F
		private void HandleKeyUp(object sender, KeyEventArgs e)
		{
			this.OnKeyUp(e);
		}

		// Token: 0x0600572B RID: 22315 RVA: 0x0013C238 File Offset: 0x0013B238
		private void HandleLeave(object sender, EventArgs e)
		{
			this.OnLeave(e);
		}

		// Token: 0x0600572C RID: 22316 RVA: 0x0013C241 File Offset: 0x0013B241
		private void HandleMouseDown(object sender, MouseEventArgs e)
		{
			this.OnMouseDown(e);
			base.RaiseMouseEvent(ToolStripItem.EventMouseDown, e);
		}

		// Token: 0x0600572D RID: 22317 RVA: 0x0013C256 File Offset: 0x0013B256
		private void HandleMouseEnter(object sender, EventArgs e)
		{
			this.OnMouseEnter(e);
			base.RaiseEvent(ToolStripItem.EventMouseEnter, e);
		}

		// Token: 0x0600572E RID: 22318 RVA: 0x0013C26B File Offset: 0x0013B26B
		private void HandleMouseLeave(object sender, EventArgs e)
		{
			this.OnMouseLeave(e);
			base.RaiseEvent(ToolStripItem.EventMouseLeave, e);
		}

		// Token: 0x0600572F RID: 22319 RVA: 0x0013C280 File Offset: 0x0013B280
		private void HandleMouseHover(object sender, EventArgs e)
		{
			this.OnMouseHover(e);
			base.RaiseEvent(ToolStripItem.EventMouseHover, e);
		}

		// Token: 0x06005730 RID: 22320 RVA: 0x0013C295 File Offset: 0x0013B295
		private void HandleMouseMove(object sender, MouseEventArgs e)
		{
			this.OnMouseMove(e);
			base.RaiseMouseEvent(ToolStripItem.EventMouseMove, e);
		}

		// Token: 0x06005731 RID: 22321 RVA: 0x0013C2AA File Offset: 0x0013B2AA
		private void HandleMouseUp(object sender, MouseEventArgs e)
		{
			this.OnMouseUp(e);
			base.RaiseMouseEvent(ToolStripItem.EventMouseUp, e);
		}

		// Token: 0x06005732 RID: 22322 RVA: 0x0013C2BF File Offset: 0x0013B2BF
		private void HandlePaint(object sender, PaintEventArgs e)
		{
			this.OnPaint(e);
			base.RaisePaintEvent(ToolStripItem.EventPaint, e);
		}

		// Token: 0x06005733 RID: 22323 RVA: 0x0013C2D4 File Offset: 0x0013B2D4
		private void HandleQueryAccessibilityHelp(object sender, QueryAccessibilityHelpEventArgs e)
		{
			QueryAccessibilityHelpEventHandler queryAccessibilityHelpEventHandler = (QueryAccessibilityHelpEventHandler)base.Events[ToolStripItem.EventQueryAccessibilityHelp];
			if (queryAccessibilityHelpEventHandler != null)
			{
				queryAccessibilityHelpEventHandler(this, e);
			}
		}

		// Token: 0x06005734 RID: 22324 RVA: 0x0013C302 File Offset: 0x0013B302
		private void HandleQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			this.OnQueryContinueDrag(e);
		}

		// Token: 0x06005735 RID: 22325 RVA: 0x0013C30B File Offset: 0x0013B30B
		private void HandleRightToLeftChanged(object sender, EventArgs e)
		{
			this.OnRightToLeftChanged(e);
		}

		// Token: 0x06005736 RID: 22326 RVA: 0x0013C314 File Offset: 0x0013B314
		private void HandleResize(object sender, EventArgs e)
		{
			if (this.suspendSyncSizeCount == 0)
			{
				this.OnHostedControlResize(e);
			}
		}

		// Token: 0x06005737 RID: 22327 RVA: 0x0013C325 File Offset: 0x0013B325
		private void HandleTextChanged(object sender, EventArgs e)
		{
			this.OnTextChanged(e);
		}

		// Token: 0x06005738 RID: 22328 RVA: 0x0013C330 File Offset: 0x0013B330
		private void HandleControlVisibleChanged(object sender, EventArgs e)
		{
			bool participatesInLayout = ((IArrangedElement)this.Control).ParticipatesInLayout;
			bool participatesInLayout2 = ((IArrangedElement)this).ParticipatesInLayout;
			if (participatesInLayout2 != participatesInLayout)
			{
				base.Visible = this.Control.Visible;
			}
		}

		// Token: 0x06005739 RID: 22329 RVA: 0x0013C365 File Offset: 0x0013B365
		private void HandleValidating(object sender, CancelEventArgs e)
		{
			this.OnValidating(e);
		}

		// Token: 0x0600573A RID: 22330 RVA: 0x0013C36E File Offset: 0x0013B36E
		private void HandleValidated(object sender, EventArgs e)
		{
			this.OnValidated(e);
		}

		// Token: 0x0600573B RID: 22331 RVA: 0x0013C377 File Offset: 0x0013B377
		internal override void OnAccessibleDescriptionChanged(EventArgs e)
		{
			this.Control.AccessibleDescription = base.AccessibleDescription;
		}

		// Token: 0x0600573C RID: 22332 RVA: 0x0013C38A File Offset: 0x0013B38A
		internal override void OnAccessibleNameChanged(EventArgs e)
		{
			this.Control.AccessibleName = base.AccessibleName;
		}

		// Token: 0x0600573D RID: 22333 RVA: 0x0013C39D File Offset: 0x0013B39D
		internal override void OnAccessibleDefaultActionDescriptionChanged(EventArgs e)
		{
			this.Control.AccessibleDefaultActionDescription = base.AccessibleDefaultActionDescription;
		}

		// Token: 0x0600573E RID: 22334 RVA: 0x0013C3B0 File Offset: 0x0013B3B0
		internal override void OnAccessibleRoleChanged(EventArgs e)
		{
			this.Control.AccessibleRole = base.AccessibleRole;
		}

		// Token: 0x0600573F RID: 22335 RVA: 0x0013C3C3 File Offset: 0x0013B3C3
		protected virtual void OnEnter(EventArgs e)
		{
			base.RaiseEvent(ToolStripControlHost.EventEnter, e);
		}

		// Token: 0x06005740 RID: 22336 RVA: 0x0013C3D1 File Offset: 0x0013B3D1
		protected virtual void OnGotFocus(EventArgs e)
		{
			base.RaiseEvent(ToolStripControlHost.EventGotFocus, e);
		}

		// Token: 0x06005741 RID: 22337 RVA: 0x0013C3DF File Offset: 0x0013B3DF
		protected virtual void OnLeave(EventArgs e)
		{
			base.RaiseEvent(ToolStripControlHost.EventLeave, e);
		}

		// Token: 0x06005742 RID: 22338 RVA: 0x0013C3ED File Offset: 0x0013B3ED
		protected virtual void OnLostFocus(EventArgs e)
		{
			base.RaiseEvent(ToolStripControlHost.EventLostFocus, e);
		}

		// Token: 0x06005743 RID: 22339 RVA: 0x0013C3FB File Offset: 0x0013B3FB
		protected virtual void OnKeyDown(KeyEventArgs e)
		{
			base.RaiseKeyEvent(ToolStripControlHost.EventKeyDown, e);
		}

		// Token: 0x06005744 RID: 22340 RVA: 0x0013C409 File Offset: 0x0013B409
		protected virtual void OnKeyPress(KeyPressEventArgs e)
		{
			base.RaiseKeyPressEvent(ToolStripControlHost.EventKeyPress, e);
		}

		// Token: 0x06005745 RID: 22341 RVA: 0x0013C417 File Offset: 0x0013B417
		protected virtual void OnKeyUp(KeyEventArgs e)
		{
			base.RaiseKeyEvent(ToolStripControlHost.EventKeyUp, e);
		}

		// Token: 0x06005746 RID: 22342 RVA: 0x0013C428 File Offset: 0x0013B428
		protected override void OnBoundsChanged()
		{
			if (this.control != null)
			{
				this.SuspendSizeSync();
				IArrangedElement arrangedElement = this.control;
				if (arrangedElement == null)
				{
					return;
				}
				Size size = LayoutUtils.DeflateRect(this.Bounds, this.Padding).Size;
				Rectangle rectangle = LayoutUtils.Align(size, this.Bounds, this.ControlAlign);
				arrangedElement.SetBounds(rectangle, BoundsSpecified.None);
				if (rectangle != this.control.Bounds)
				{
					rectangle = LayoutUtils.Align(this.control.Size, this.Bounds, this.ControlAlign);
					arrangedElement.SetBounds(rectangle, BoundsSpecified.None);
				}
				this.ResumeSizeSync();
			}
		}

		// Token: 0x06005747 RID: 22343 RVA: 0x0013C4C4 File Offset: 0x0013B4C4
		protected override void OnPaint(PaintEventArgs e)
		{
		}

		// Token: 0x06005748 RID: 22344 RVA: 0x0013C4C6 File Offset: 0x0013B4C6
		protected internal override void OnLayout(LayoutEventArgs e)
		{
		}

		// Token: 0x06005749 RID: 22345 RVA: 0x0013C4C8 File Offset: 0x0013B4C8
		protected override void OnParentChanged(ToolStrip oldParent, ToolStrip newParent)
		{
			if (oldParent != null && base.Owner == null && newParent == null && this.Control != null)
			{
				WindowsFormsUtils.ReadOnlyControlCollection controlCollection = ToolStripControlHost.GetControlCollection(this.Control.ParentInternal as ToolStrip);
				if (controlCollection != null)
				{
					controlCollection.RemoveInternal(this.Control);
				}
			}
			else
			{
				this.SyncControlParent();
			}
			base.OnParentChanged(oldParent, newParent);
		}

		// Token: 0x0600574A RID: 22346 RVA: 0x0013C520 File Offset: 0x0013B520
		protected virtual void OnSubscribeControlEvents(Control control)
		{
			if (control != null)
			{
				control.Click += this.HandleClick;
				control.BackColorChanged += this.HandleBackColorChanged;
				control.DoubleClick += this.HandleDoubleClick;
				control.DragDrop += this.HandleDragDrop;
				control.DragEnter += this.HandleDragEnter;
				control.DragLeave += this.HandleDragLeave;
				control.DragOver += this.HandleDragOver;
				control.Enter += this.HandleEnter;
				control.EnabledChanged += this.HandleEnabledChanged;
				control.ForeColorChanged += this.HandleForeColorChanged;
				control.GiveFeedback += this.HandleGiveFeedback;
				control.GotFocus += this.HandleGotFocus;
				control.Leave += this.HandleLeave;
				control.LocationChanged += this.HandleLocationChanged;
				control.LostFocus += this.HandleLostFocus;
				control.KeyDown += this.HandleKeyDown;
				control.KeyPress += this.HandleKeyPress;
				control.KeyUp += this.HandleKeyUp;
				control.MouseDown += this.HandleMouseDown;
				control.MouseEnter += this.HandleMouseEnter;
				control.MouseHover += this.HandleMouseHover;
				control.MouseLeave += this.HandleMouseLeave;
				control.MouseMove += this.HandleMouseMove;
				control.MouseUp += this.HandleMouseUp;
				control.Paint += this.HandlePaint;
				control.QueryAccessibilityHelp += this.HandleQueryAccessibilityHelp;
				control.QueryContinueDrag += this.HandleQueryContinueDrag;
				control.Resize += this.HandleResize;
				control.RightToLeftChanged += this.HandleRightToLeftChanged;
				control.TextChanged += this.HandleTextChanged;
				control.VisibleChanged += this.HandleControlVisibleChanged;
				control.Validating += this.HandleValidating;
				control.Validated += this.HandleValidated;
			}
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x0013C788 File Offset: 0x0013B788
		protected virtual void OnUnsubscribeControlEvents(Control control)
		{
			if (control != null)
			{
				control.Click -= this.HandleClick;
				control.BackColorChanged -= this.HandleBackColorChanged;
				control.DoubleClick -= this.HandleDoubleClick;
				control.DragDrop -= this.HandleDragDrop;
				control.DragEnter -= this.HandleDragEnter;
				control.DragLeave -= this.HandleDragLeave;
				control.DragOver -= this.HandleDragOver;
				control.Enter -= this.HandleEnter;
				control.EnabledChanged -= this.HandleEnabledChanged;
				control.ForeColorChanged -= this.HandleForeColorChanged;
				control.GiveFeedback -= this.HandleGiveFeedback;
				control.GotFocus -= this.HandleGotFocus;
				control.Leave -= this.HandleLeave;
				control.LocationChanged -= this.HandleLocationChanged;
				control.LostFocus -= this.HandleLostFocus;
				control.KeyDown -= this.HandleKeyDown;
				control.KeyPress -= this.HandleKeyPress;
				control.KeyUp -= this.HandleKeyUp;
				control.MouseDown -= this.HandleMouseDown;
				control.MouseEnter -= this.HandleMouseEnter;
				control.MouseHover -= this.HandleMouseHover;
				control.MouseLeave -= this.HandleMouseLeave;
				control.MouseMove -= this.HandleMouseMove;
				control.MouseUp -= this.HandleMouseUp;
				control.Paint -= this.HandlePaint;
				control.QueryAccessibilityHelp -= this.HandleQueryAccessibilityHelp;
				control.QueryContinueDrag -= this.HandleQueryContinueDrag;
				control.Resize -= this.HandleResize;
				control.RightToLeftChanged -= this.HandleRightToLeftChanged;
				control.TextChanged -= this.HandleTextChanged;
				control.VisibleChanged -= this.HandleControlVisibleChanged;
				control.Validating -= this.HandleValidating;
				control.Validated -= this.HandleValidated;
			}
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x0013C9ED File Offset: 0x0013B9ED
		protected virtual void OnValidating(CancelEventArgs e)
		{
			base.RaiseCancelEvent(ToolStripControlHost.EventValidating, e);
		}

		// Token: 0x0600574D RID: 22349 RVA: 0x0013C9FB File Offset: 0x0013B9FB
		protected virtual void OnValidated(EventArgs e)
		{
			base.RaiseEvent(ToolStripControlHost.EventValidated, e);
		}

		// Token: 0x0600574E RID: 22350 RVA: 0x0013CA0C File Offset: 0x0013BA0C
		private static WindowsFormsUtils.ReadOnlyControlCollection GetControlCollection(ToolStrip toolStrip)
		{
			return (toolStrip != null) ? ((WindowsFormsUtils.ReadOnlyControlCollection)toolStrip.Controls) : null;
		}

		// Token: 0x0600574F RID: 22351 RVA: 0x0013CA2C File Offset: 0x0013BA2C
		private void SyncControlParent()
		{
			WindowsFormsUtils.ReadOnlyControlCollection controlCollection = ToolStripControlHost.GetControlCollection(base.ParentInternal);
			if (controlCollection != null)
			{
				controlCollection.AddInternal(this.Control);
			}
		}

		// Token: 0x06005750 RID: 22352 RVA: 0x0013CA54 File Offset: 0x0013BA54
		protected virtual void OnHostedControlResize(EventArgs e)
		{
			this.Size = this.Control.Size;
		}

		// Token: 0x06005751 RID: 22353 RVA: 0x0013CA67 File Offset: 0x0013BA67
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			return false;
		}

		// Token: 0x06005752 RID: 22354 RVA: 0x0013CA6A File Offset: 0x0013BA6A
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessDialogKey(Keys keyData)
		{
			return false;
		}

		// Token: 0x06005753 RID: 22355 RVA: 0x0013CA70 File Offset: 0x0013BA70
		protected override void SetVisibleCore(bool visible)
		{
			if (this.inSetVisibleCore)
			{
				return;
			}
			this.inSetVisibleCore = true;
			this.Control.SuspendLayout();
			try
			{
				this.Control.Visible = visible;
			}
			finally
			{
				this.Control.ResumeLayout(false);
				base.SetVisibleCore(visible);
				this.inSetVisibleCore = false;
			}
		}

		// Token: 0x06005754 RID: 22356 RVA: 0x0013CAD4 File Offset: 0x0013BAD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void ResetBackColor()
		{
			this.Control.ResetBackColor();
		}

		// Token: 0x06005755 RID: 22357 RVA: 0x0013CAE1 File Offset: 0x0013BAE1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void ResetForeColor()
		{
			this.Control.ResetForeColor();
		}

		// Token: 0x06005756 RID: 22358 RVA: 0x0013CAEE File Offset: 0x0013BAEE
		private void SuspendSizeSync()
		{
			this.suspendSyncSizeCount++;
		}

		// Token: 0x06005757 RID: 22359 RVA: 0x0013CAFE File Offset: 0x0013BAFE
		private void ResumeSizeSync()
		{
			this.suspendSyncSizeCount--;
		}

		// Token: 0x06005758 RID: 22360 RVA: 0x0013CB0E File Offset: 0x0013BB0E
		internal override bool ShouldSerializeBackColor()
		{
			if (this.control != null)
			{
				return this.control.ShouldSerializeBackColor();
			}
			return base.ShouldSerializeBackColor();
		}

		// Token: 0x06005759 RID: 22361 RVA: 0x0013CB2A File Offset: 0x0013BB2A
		internal override bool ShouldSerializeForeColor()
		{
			if (this.control != null)
			{
				return this.control.ShouldSerializeForeColor();
			}
			return base.ShouldSerializeForeColor();
		}

		// Token: 0x0600575A RID: 22362 RVA: 0x0013CB46 File Offset: 0x0013BB46
		internal override bool ShouldSerializeFont()
		{
			if (this.control != null)
			{
				return this.control.ShouldSerializeFont();
			}
			return base.ShouldSerializeFont();
		}

		// Token: 0x0600575B RID: 22363 RVA: 0x0013CB62 File Offset: 0x0013BB62
		internal override bool ShouldSerializeRightToLeft()
		{
			if (this.control != null)
			{
				return this.control.ShouldSerializeRightToLeft();
			}
			return base.ShouldSerializeRightToLeft();
		}

		// Token: 0x04003785 RID: 14213
		private Control control;

		// Token: 0x04003786 RID: 14214
		private int suspendSyncSizeCount;

		// Token: 0x04003787 RID: 14215
		private ContentAlignment controlAlign = ContentAlignment.MiddleCenter;

		// Token: 0x04003788 RID: 14216
		private bool inSetVisibleCore;

		// Token: 0x04003789 RID: 14217
		internal static readonly object EventGotFocus = new object();

		// Token: 0x0400378A RID: 14218
		internal static readonly object EventLostFocus = new object();

		// Token: 0x0400378B RID: 14219
		internal static readonly object EventKeyDown = new object();

		// Token: 0x0400378C RID: 14220
		internal static readonly object EventKeyPress = new object();

		// Token: 0x0400378D RID: 14221
		internal static readonly object EventKeyUp = new object();

		// Token: 0x0400378E RID: 14222
		internal static readonly object EventEnter = new object();

		// Token: 0x0400378F RID: 14223
		internal static readonly object EventLeave = new object();

		// Token: 0x04003790 RID: 14224
		internal static readonly object EventValidated = new object();

		// Token: 0x04003791 RID: 14225
		internal static readonly object EventValidating = new object();

		// Token: 0x02000678 RID: 1656
		private class StubSite : ISite, IServiceProvider
		{
			// Token: 0x0600575D RID: 22365 RVA: 0x0013CBE7 File Offset: 0x0013BBE7
			public StubSite(Component control, Component host)
			{
				this.comp = control;
				this.owner = host;
			}

			// Token: 0x17001226 RID: 4646
			// (get) Token: 0x0600575E RID: 22366 RVA: 0x0013CBFD File Offset: 0x0013BBFD
			IComponent ISite.Component
			{
				get
				{
					return this.comp;
				}
			}

			// Token: 0x17001227 RID: 4647
			// (get) Token: 0x0600575F RID: 22367 RVA: 0x0013CC05 File Offset: 0x0013BC05
			IContainer ISite.Container
			{
				get
				{
					return this.owner.Site.Container;
				}
			}

			// Token: 0x17001228 RID: 4648
			// (get) Token: 0x06005760 RID: 22368 RVA: 0x0013CC17 File Offset: 0x0013BC17
			bool ISite.DesignMode
			{
				get
				{
					return this.owner.Site.DesignMode;
				}
			}

			// Token: 0x17001229 RID: 4649
			// (get) Token: 0x06005761 RID: 22369 RVA: 0x0013CC29 File Offset: 0x0013BC29
			// (set) Token: 0x06005762 RID: 22370 RVA: 0x0013CC3B File Offset: 0x0013BC3B
			string ISite.Name
			{
				get
				{
					return this.owner.Site.Name;
				}
				set
				{
					this.owner.Site.Name = value;
				}
			}

			// Token: 0x06005763 RID: 22371 RVA: 0x0013CC4E File Offset: 0x0013BC4E
			object IServiceProvider.GetService(Type service)
			{
				if (service == null)
				{
					throw new ArgumentNullException("service");
				}
				if (this.owner.Site != null)
				{
					return this.owner.Site.GetService(service);
				}
				return null;
			}

			// Token: 0x04003792 RID: 14226
			private IComponent comp;

			// Token: 0x04003793 RID: 14227
			private IComponent owner;
		}
	}
}

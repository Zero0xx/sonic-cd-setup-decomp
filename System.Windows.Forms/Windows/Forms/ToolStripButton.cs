using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
	// Token: 0x02000675 RID: 1653
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
	public class ToolStripButton : ToolStripItem
	{
		// Token: 0x060056B1 RID: 22193 RVA: 0x0013B887 File Offset: 0x0013A887
		public ToolStripButton()
		{
			this.Initialize();
		}

		// Token: 0x060056B2 RID: 22194 RVA: 0x0013B895 File Offset: 0x0013A895
		public ToolStripButton(string text) : base(text, null, null)
		{
			this.Initialize();
		}

		// Token: 0x060056B3 RID: 22195 RVA: 0x0013B8A6 File Offset: 0x0013A8A6
		public ToolStripButton(Image image) : base(null, image, null)
		{
			this.Initialize();
		}

		// Token: 0x060056B4 RID: 22196 RVA: 0x0013B8B7 File Offset: 0x0013A8B7
		public ToolStripButton(string text, Image image) : base(text, image, null)
		{
			this.Initialize();
		}

		// Token: 0x060056B5 RID: 22197 RVA: 0x0013B8C8 File Offset: 0x0013A8C8
		public ToolStripButton(string text, Image image, EventHandler onClick) : base(text, image, onClick)
		{
			this.Initialize();
		}

		// Token: 0x060056B6 RID: 22198 RVA: 0x0013B8D9 File Offset: 0x0013A8D9
		public ToolStripButton(string text, Image image, EventHandler onClick, string name) : base(text, image, onClick, name)
		{
			this.Initialize();
		}

		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x060056B7 RID: 22199 RVA: 0x0013B8EC File Offset: 0x0013A8EC
		// (set) Token: 0x060056B8 RID: 22200 RVA: 0x0013B8F4 File Offset: 0x0013A8F4
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

		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x060056B9 RID: 22201 RVA: 0x0013B8FD File Offset: 0x0013A8FD
		public override bool CanSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x060056BA RID: 22202 RVA: 0x0013B900 File Offset: 0x0013A900
		// (set) Token: 0x060056BB RID: 22203 RVA: 0x0013B908 File Offset: 0x0013A908
		[SRDescription("ToolStripButtonCheckOnClickDescr")]
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		public bool CheckOnClick
		{
			get
			{
				return this.checkOnClick;
			}
			set
			{
				this.checkOnClick = value;
			}
		}

		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x060056BC RID: 22204 RVA: 0x0013B911 File Offset: 0x0013A911
		// (set) Token: 0x060056BD RID: 22205 RVA: 0x0013B91F File Offset: 0x0013A91F
		[SRDescription("ToolStripButtonCheckedDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		public bool Checked
		{
			get
			{
				return this.checkState != CheckState.Unchecked;
			}
			set
			{
				if (value != this.Checked)
				{
					this.CheckState = (value ? CheckState.Checked : CheckState.Unchecked);
					base.InvokePaint();
				}
			}
		}

		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x060056BE RID: 22206 RVA: 0x0013B93D File Offset: 0x0013A93D
		// (set) Token: 0x060056BF RID: 22207 RVA: 0x0013B948 File Offset: 0x0013A948
		[SRDescription("CheckBoxCheckStateDescr")]
		[DefaultValue(CheckState.Unchecked)]
		[SRCategory("CatAppearance")]
		public CheckState CheckState
		{
			get
			{
				return this.checkState;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(CheckState));
				}
				if (value != this.checkState)
				{
					this.checkState = value;
					base.Invalidate();
					this.OnCheckedChanged(EventArgs.Empty);
					this.OnCheckStateChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000339 RID: 825
		// (add) Token: 0x060056C0 RID: 22208 RVA: 0x0013B9A7 File Offset: 0x0013A9A7
		// (remove) Token: 0x060056C1 RID: 22209 RVA: 0x0013B9BA File Offset: 0x0013A9BA
		[SRDescription("CheckBoxOnCheckedChangedDescr")]
		public event EventHandler CheckedChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripButton.EventCheckedChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripButton.EventCheckedChanged, value);
			}
		}

		// Token: 0x1400033A RID: 826
		// (add) Token: 0x060056C2 RID: 22210 RVA: 0x0013B9CD File Offset: 0x0013A9CD
		// (remove) Token: 0x060056C3 RID: 22211 RVA: 0x0013B9E0 File Offset: 0x0013A9E0
		[SRDescription("CheckBoxOnCheckStateChangedDescr")]
		public event EventHandler CheckStateChanged
		{
			add
			{
				base.Events.AddHandler(ToolStripButton.EventCheckStateChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(ToolStripButton.EventCheckStateChanged, value);
			}
		}

		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x060056C4 RID: 22212 RVA: 0x0013B9F3 File Offset: 0x0013A9F3
		protected override bool DefaultAutoToolTip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060056C5 RID: 22213 RVA: 0x0013B9F6 File Offset: 0x0013A9F6
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripButton.ToolStripButtonAccessibleObject(this);
		}

		// Token: 0x060056C6 RID: 22214 RVA: 0x0013BA00 File Offset: 0x0013AA00
		public override Size GetPreferredSize(Size constrainingSize)
		{
			Size preferredSize = base.GetPreferredSize(constrainingSize);
			preferredSize.Width = Math.Max(preferredSize.Width, 23);
			return preferredSize;
		}

		// Token: 0x060056C7 RID: 22215 RVA: 0x0013BA2B File Offset: 0x0013AA2B
		private void Initialize()
		{
			base.SupportsSpaceKey = true;
		}

		// Token: 0x060056C8 RID: 22216 RVA: 0x0013BA34 File Offset: 0x0013AA34
		protected virtual void OnCheckedChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripButton.EventCheckedChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060056C9 RID: 22217 RVA: 0x0013BA64 File Offset: 0x0013AA64
		protected virtual void OnCheckStateChanged(EventArgs e)
		{
			base.AccessibilityNotifyClients(AccessibleEvents.StateChange);
			EventHandler eventHandler = (EventHandler)base.Events[ToolStripButton.EventCheckStateChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x060056CA RID: 22218 RVA: 0x0013BAA0 File Offset: 0x0013AAA0
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.Owner != null)
			{
				ToolStripRenderer renderer = base.Renderer;
				renderer.DrawButtonBackground(new ToolStripItemRenderEventArgs(e.Graphics, this));
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image)
				{
					renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(e.Graphics, this, base.InternalLayout.ImageRectangle)
					{
						ShiftOnPress = true
					});
				}
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
				{
					renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(e.Graphics, this, this.Text, base.InternalLayout.TextRectangle, this.ForeColor, this.Font, base.InternalLayout.TextFormat));
				}
			}
		}

		// Token: 0x060056CB RID: 22219 RVA: 0x0013BB47 File Offset: 0x0013AB47
		protected override void OnClick(EventArgs e)
		{
			if (this.checkOnClick)
			{
				this.Checked = !this.Checked;
			}
			base.OnClick(e);
		}

		// Token: 0x0400377F RID: 14207
		private const int StandardButtonWidth = 23;

		// Token: 0x04003780 RID: 14208
		private CheckState checkState;

		// Token: 0x04003781 RID: 14209
		private bool checkOnClick;

		// Token: 0x04003782 RID: 14210
		private static readonly object EventCheckedChanged = new object();

		// Token: 0x04003783 RID: 14211
		private static readonly object EventCheckStateChanged = new object();

		// Token: 0x02000676 RID: 1654
		[ComVisible(true)]
		internal class ToolStripButtonAccessibleObject : ToolStripItem.ToolStripItemAccessibleObject
		{
			// Token: 0x060056CD RID: 22221 RVA: 0x0013BB7D File Offset: 0x0013AB7D
			public ToolStripButtonAccessibleObject(ToolStripButton ownerItem) : base(ownerItem)
			{
				this.ownerItem = ownerItem;
			}

			// Token: 0x1700120A RID: 4618
			// (get) Token: 0x060056CE RID: 22222 RVA: 0x0013BB8D File Offset: 0x0013AB8D
			public override AccessibleStates State
			{
				get
				{
					if (this.ownerItem.Enabled && this.ownerItem.Checked)
					{
						return base.State | AccessibleStates.Checked;
					}
					return base.State;
				}
			}

			// Token: 0x04003784 RID: 14212
			private ToolStripButton ownerItem;
		}
	}
}

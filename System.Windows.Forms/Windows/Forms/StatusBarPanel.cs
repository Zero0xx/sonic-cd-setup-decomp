using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200062D RID: 1581
	[DesignTimeVisible(false)]
	[ToolboxItem(false)]
	[DefaultProperty("Text")]
	public class StatusBarPanel : Component, ISupportInitialize
	{
		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x060052D9 RID: 21209 RVA: 0x0012F23D File Offset: 0x0012E23D
		// (set) Token: 0x060052DA RID: 21210 RVA: 0x0012F245 File Offset: 0x0012E245
		[SRDescription("StatusBarPanelAlignmentDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(HorizontalAlignment.Left)]
		[Localizable(true)]
		public HorizontalAlignment Alignment
		{
			get
			{
				return this.alignment;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(HorizontalAlignment));
				}
				if (this.alignment != value)
				{
					this.alignment = value;
					this.Realize();
				}
			}
		}

		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x060052DB RID: 21211 RVA: 0x0012F283 File Offset: 0x0012E283
		// (set) Token: 0x060052DC RID: 21212 RVA: 0x0012F28B File Offset: 0x0012E28B
		[SRDescription("StatusBarPanelAutoSizeDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(StatusBarPanelAutoSize.None)]
		[RefreshProperties(RefreshProperties.All)]
		public StatusBarPanelAutoSize AutoSize
		{
			get
			{
				return this.autoSize;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 1, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(StatusBarPanelAutoSize));
				}
				if (this.autoSize != value)
				{
					this.autoSize = value;
					this.UpdateSize();
				}
			}
		}

		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x060052DD RID: 21213 RVA: 0x0012F2C9 File Offset: 0x0012E2C9
		// (set) Token: 0x060052DE RID: 21214 RVA: 0x0012F2D4 File Offset: 0x0012E2D4
		[SRDescription("StatusBarPanelBorderStyleDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(StatusBarPanelBorderStyle.Sunken)]
		[DispId(-504)]
		public StatusBarPanelBorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 1, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(StatusBarPanelBorderStyle));
				}
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					this.Realize();
					if (this.Created)
					{
						this.parent.Invalidate();
					}
				}
			}
		}

		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x060052DF RID: 21215 RVA: 0x0012F330 File Offset: 0x0012E330
		internal bool Created
		{
			get
			{
				return this.parent != null && this.parent.ArePanelsRealized();
			}
		}

		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x060052E0 RID: 21216 RVA: 0x0012F347 File Offset: 0x0012E347
		// (set) Token: 0x060052E1 RID: 21217 RVA: 0x0012F350 File Offset: 0x0012E350
		[SRDescription("StatusBarPanelIconDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(null)]
		[Localizable(true)]
		public Icon Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				if (value != null && (value.Height > SystemInformation.SmallIconSize.Height || value.Width > SystemInformation.SmallIconSize.Width))
				{
					this.icon = new Icon(value, SystemInformation.SmallIconSize);
				}
				else
				{
					this.icon = value;
				}
				if (this.Created)
				{
					IntPtr lparam = (this.icon == null) ? IntPtr.Zero : this.icon.Handle;
					this.parent.SendMessage(1039, (IntPtr)this.GetIndex(), lparam);
				}
				this.UpdateSize();
				if (this.Created)
				{
					this.parent.Invalidate();
				}
			}
		}

		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x060052E2 RID: 21218 RVA: 0x0012F3FD File Offset: 0x0012E3FD
		// (set) Token: 0x060052E3 RID: 21219 RVA: 0x0012F405 File Offset: 0x0012E405
		internal int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x060052E4 RID: 21220 RVA: 0x0012F40E File Offset: 0x0012E40E
		// (set) Token: 0x060052E5 RID: 21221 RVA: 0x0012F418 File Offset: 0x0012E418
		[RefreshProperties(RefreshProperties.All)]
		[SRCategory("CatBehavior")]
		[DefaultValue(10)]
		[Localizable(true)]
		[SRDescription("StatusBarPanelMinWidthDescr")]
		public int MinWidth
		{
			get
			{
				return this.minWidth;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("MinWidth", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"MinWidth",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (value != this.minWidth)
				{
					this.minWidth = value;
					this.UpdateSize();
					if (this.minWidth > this.Width)
					{
						this.Width = value;
					}
				}
			}
		}

		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x060052E6 RID: 21222 RVA: 0x0012F499 File Offset: 0x0012E499
		// (set) Token: 0x060052E7 RID: 21223 RVA: 0x0012F4A7 File Offset: 0x0012E4A7
		[SRCategory("CatAppearance")]
		[SRDescription("StatusBarPanelNameDescr")]
		[Localizable(true)]
		public string Name
		{
			get
			{
				return WindowsFormsUtils.GetComponentName(this, this.name);
			}
			set
			{
				this.name = value;
				if (this.Site != null)
				{
					this.Site.Name = this.name;
				}
			}
		}

		// Token: 0x170010D1 RID: 4305
		// (get) Token: 0x060052E8 RID: 21224 RVA: 0x0012F4C9 File Offset: 0x0012E4C9
		[Browsable(false)]
		public StatusBar Parent
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170010D2 RID: 4306
		// (set) Token: 0x060052E9 RID: 21225 RVA: 0x0012F4D1 File Offset: 0x0012E4D1
		internal StatusBar ParentInternal
		{
			set
			{
				this.parent = value;
			}
		}

		// Token: 0x170010D3 RID: 4307
		// (get) Token: 0x060052EA RID: 21226 RVA: 0x0012F4DA File Offset: 0x0012E4DA
		// (set) Token: 0x060052EB RID: 21227 RVA: 0x0012F4E2 File Offset: 0x0012E4E2
		internal int Right
		{
			get
			{
				return this.right;
			}
			set
			{
				this.right = value;
			}
		}

		// Token: 0x170010D4 RID: 4308
		// (get) Token: 0x060052EC RID: 21228 RVA: 0x0012F4EB File Offset: 0x0012E4EB
		// (set) Token: 0x060052ED RID: 21229 RVA: 0x0012F4F4 File Offset: 0x0012E4F4
		[SRDescription("StatusBarPanelStyleDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(StatusBarPanelStyle.Text)]
		public StatusBarPanelStyle Style
		{
			get
			{
				return this.style;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 1, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(StatusBarPanelStyle));
				}
				if (this.style != value)
				{
					this.style = value;
					this.Realize();
					if (this.Created)
					{
						this.parent.Invalidate();
					}
				}
			}
		}

		// Token: 0x170010D5 RID: 4309
		// (get) Token: 0x060052EE RID: 21230 RVA: 0x0012F550 File Offset: 0x0012E550
		// (set) Token: 0x060052EF RID: 21231 RVA: 0x0012F558 File Offset: 0x0012E558
		[Localizable(false)]
		[SRCategory("CatData")]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		// Token: 0x170010D6 RID: 4310
		// (get) Token: 0x060052F0 RID: 21232 RVA: 0x0012F561 File Offset: 0x0012E561
		// (set) Token: 0x060052F1 RID: 21233 RVA: 0x0012F577 File Offset: 0x0012E577
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue("")]
		[SRDescription("StatusBarPanelTextDescr")]
		public string Text
		{
			get
			{
				if (this.text == null)
				{
					return "";
				}
				return this.text;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (!this.Text.Equals(value))
				{
					if (value.Length == 0)
					{
						this.text = null;
					}
					else
					{
						this.text = value;
					}
					this.Realize();
					this.UpdateSize();
				}
			}
		}

		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x060052F2 RID: 21234 RVA: 0x0012F5B5 File Offset: 0x0012E5B5
		// (set) Token: 0x060052F3 RID: 21235 RVA: 0x0012F5CC File Offset: 0x0012E5CC
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue("")]
		[SRDescription("StatusBarPanelToolTipTextDescr")]
		public string ToolTipText
		{
			get
			{
				if (this.toolTipText == null)
				{
					return "";
				}
				return this.toolTipText;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (!this.ToolTipText.Equals(value))
				{
					if (value.Length == 0)
					{
						this.toolTipText = null;
					}
					else
					{
						this.toolTipText = value;
					}
					if (this.Created)
					{
						this.parent.UpdateTooltip(this);
					}
				}
			}
		}

		// Token: 0x170010D8 RID: 4312
		// (get) Token: 0x060052F4 RID: 21236 RVA: 0x0012F61D File Offset: 0x0012E61D
		// (set) Token: 0x060052F5 RID: 21237 RVA: 0x0012F625 File Offset: 0x0012E625
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue(100)]
		[SRDescription("StatusBarPanelWidthDescr")]
		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				if (!this.initializing && value < this.minWidth)
				{
					throw new ArgumentOutOfRangeException("Width", SR.GetString("WidthGreaterThanMinWidth"));
				}
				this.width = value;
				this.UpdateSize();
			}
		}

		// Token: 0x060052F6 RID: 21238 RVA: 0x0012F65A File Offset: 0x0012E65A
		public void BeginInit()
		{
			this.initializing = true;
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x0012F664 File Offset: 0x0012E664
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.parent != null)
			{
				int num = this.GetIndex();
				if (num != -1)
				{
					this.parent.Panels.RemoveAt(num);
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x060052F8 RID: 21240 RVA: 0x0012F69F File Offset: 0x0012E69F
		public void EndInit()
		{
			this.initializing = false;
			if (this.Width < this.MinWidth)
			{
				this.Width = this.MinWidth;
			}
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x0012F6C4 File Offset: 0x0012E6C4
		internal int GetContentsWidth(bool newPanel)
		{
			string text;
			if (newPanel)
			{
				if (this.text == null)
				{
					text = "";
				}
				else
				{
					text = this.text;
				}
			}
			else
			{
				text = this.Text;
			}
			Graphics graphics = this.parent.CreateGraphicsInternal();
			Size size = Size.Ceiling(graphics.MeasureString(text, this.parent.Font));
			if (this.icon != null)
			{
				size.Width += this.icon.Size.Width + 5;
			}
			graphics.Dispose();
			int val = size.Width + SystemInformation.BorderSize.Width * 2 + 6 + 2;
			return Math.Max(val, this.minWidth);
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x0012F773 File Offset: 0x0012E773
		private int GetIndex()
		{
			return this.index;
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x0012F77C File Offset: 0x0012E77C
		internal void Realize()
		{
			if (this.Created)
			{
				int num = 0;
				string text;
				if (this.text == null)
				{
					text = "";
				}
				else
				{
					text = this.text;
				}
				HorizontalAlignment horizontalAlignment = this.alignment;
				if (this.parent.RightToLeft == RightToLeft.Yes)
				{
					switch (horizontalAlignment)
					{
					case HorizontalAlignment.Left:
						horizontalAlignment = HorizontalAlignment.Right;
						break;
					case HorizontalAlignment.Right:
						horizontalAlignment = HorizontalAlignment.Left;
						break;
					}
				}
				string lParam;
				switch (horizontalAlignment)
				{
				case HorizontalAlignment.Right:
					lParam = "\t\t" + text;
					break;
				case HorizontalAlignment.Center:
					lParam = "\t" + text;
					break;
				default:
					lParam = text;
					break;
				}
				switch (this.borderStyle)
				{
				case StatusBarPanelBorderStyle.None:
					num |= 256;
					break;
				case StatusBarPanelBorderStyle.Raised:
					num |= 512;
					break;
				}
				switch (this.style)
				{
				case StatusBarPanelStyle.OwnerDraw:
					num |= 4096;
					break;
				}
				int num2 = this.GetIndex() | num;
				if (this.parent.RightToLeft == RightToLeft.Yes)
				{
					num2 |= 1024;
				}
				if ((int)UnsafeNativeMethods.SendMessage(new HandleRef(this.parent, this.parent.Handle), NativeMethods.SB_SETTEXT, (IntPtr)num2, lParam) == 0)
				{
					throw new InvalidOperationException(SR.GetString("UnableToSetPanelText"));
				}
				if (this.icon != null && this.style != StatusBarPanelStyle.OwnerDraw)
				{
					this.parent.SendMessage(1039, (IntPtr)this.GetIndex(), this.icon.Handle);
				}
				else
				{
					this.parent.SendMessage(1039, (IntPtr)this.GetIndex(), IntPtr.Zero);
				}
				if (this.style == StatusBarPanelStyle.OwnerDraw)
				{
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					int num3 = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this.parent, this.parent.Handle), 1034, (IntPtr)this.GetIndex(), ref rect);
					if (num3 != 0)
					{
						this.parent.Invalidate(Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom));
					}
				}
			}
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x0012F999 File Offset: 0x0012E999
		private void UpdateSize()
		{
			if (this.autoSize == StatusBarPanelAutoSize.Contents)
			{
				this.ApplyContentSizing();
				return;
			}
			if (this.Created)
			{
				this.parent.DirtyLayout();
				this.parent.PerformLayout();
			}
		}

		// Token: 0x060052FD RID: 21245 RVA: 0x0012F9CC File Offset: 0x0012E9CC
		private void ApplyContentSizing()
		{
			if (this.autoSize == StatusBarPanelAutoSize.Contents && this.parent != null)
			{
				int contentsWidth = this.GetContentsWidth(false);
				if (contentsWidth != this.Width)
				{
					this.Width = contentsWidth;
					if (this.Created)
					{
						this.parent.DirtyLayout();
						this.parent.PerformLayout();
					}
				}
			}
		}

		// Token: 0x060052FE RID: 21246 RVA: 0x0012FA20 File Offset: 0x0012EA20
		public override string ToString()
		{
			return "StatusBarPanel: {" + this.Text + "}";
		}

		// Token: 0x04003644 RID: 13892
		private const int DEFAULTWIDTH = 100;

		// Token: 0x04003645 RID: 13893
		private const int DEFAULTMINWIDTH = 10;

		// Token: 0x04003646 RID: 13894
		private const int PANELTEXTINSET = 3;

		// Token: 0x04003647 RID: 13895
		private const int PANELGAP = 2;

		// Token: 0x04003648 RID: 13896
		private string text = "";

		// Token: 0x04003649 RID: 13897
		private string name = "";

		// Token: 0x0400364A RID: 13898
		private string toolTipText = "";

		// Token: 0x0400364B RID: 13899
		private Icon icon;

		// Token: 0x0400364C RID: 13900
		private HorizontalAlignment alignment;

		// Token: 0x0400364D RID: 13901
		private StatusBarPanelBorderStyle borderStyle = StatusBarPanelBorderStyle.Sunken;

		// Token: 0x0400364E RID: 13902
		private StatusBarPanelStyle style = StatusBarPanelStyle.Text;

		// Token: 0x0400364F RID: 13903
		private StatusBar parent;

		// Token: 0x04003650 RID: 13904
		private int width = 100;

		// Token: 0x04003651 RID: 13905
		private int right;

		// Token: 0x04003652 RID: 13906
		private int minWidth = 10;

		// Token: 0x04003653 RID: 13907
		private int index;

		// Token: 0x04003654 RID: 13908
		private StatusBarPanelAutoSize autoSize = StatusBarPanelAutoSize.None;

		// Token: 0x04003655 RID: 13909
		private bool initializing;

		// Token: 0x04003656 RID: 13910
		private object userData;
	}
}

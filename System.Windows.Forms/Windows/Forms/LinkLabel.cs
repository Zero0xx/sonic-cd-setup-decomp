using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x0200046F RID: 1135
	[SRDescription("DescriptionLinkLabel")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("LinkClicked")]
	[ToolboxItem("System.Windows.Forms.Design.AutoSizeToolboxItem,System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class LinkLabel : Label, IButtonControl
	{
		// Token: 0x0600428E RID: 17038 RVA: 0x000EDB5C File Offset: 0x000ECB5C
		public LinkLabel()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.StandardClick | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.ResetLinkArea();
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x0600428F RID: 17039 RVA: 0x000EDBB9 File Offset: 0x000ECBB9
		// (set) Token: 0x06004290 RID: 17040 RVA: 0x000EDBD5 File Offset: 0x000ECBD5
		[SRCategory("CatAppearance")]
		[SRDescription("LinkLabelActiveLinkColorDescr")]
		public Color ActiveLinkColor
		{
			get
			{
				if (this.activeLinkColor.IsEmpty)
				{
					return this.IEActiveLinkColor;
				}
				return this.activeLinkColor;
			}
			set
			{
				if (this.activeLinkColor != value)
				{
					this.activeLinkColor = value;
					this.InvalidateLink(null);
				}
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06004291 RID: 17041 RVA: 0x000EDBF3 File Offset: 0x000ECBF3
		// (set) Token: 0x06004292 RID: 17042 RVA: 0x000EDC0F File Offset: 0x000ECC0F
		[SRDescription("LinkLabelDisabledLinkColorDescr")]
		[SRCategory("CatAppearance")]
		public Color DisabledLinkColor
		{
			get
			{
				if (this.disabledLinkColor.IsEmpty)
				{
					return this.IEDisabledLinkColor;
				}
				return this.disabledLinkColor;
			}
			set
			{
				if (this.disabledLinkColor != value)
				{
					this.disabledLinkColor = value;
					this.InvalidateLink(null);
				}
			}
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06004293 RID: 17043 RVA: 0x000EDC2D File Offset: 0x000ECC2D
		// (set) Token: 0x06004294 RID: 17044 RVA: 0x000EDC38 File Offset: 0x000ECC38
		private LinkLabel.Link FocusLink
		{
			get
			{
				return this.focusLink;
			}
			set
			{
				if (this.focusLink != value)
				{
					if (this.focusLink != null)
					{
						this.InvalidateLink(this.focusLink);
					}
					this.focusLink = value;
					if (this.focusLink != null)
					{
						this.InvalidateLink(this.focusLink);
						this.UpdateAccessibilityLink(this.focusLink);
					}
				}
			}
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06004295 RID: 17045 RVA: 0x000EDC89 File Offset: 0x000ECC89
		private Color IELinkColor
		{
			get
			{
				return LinkUtilities.IELinkColor;
			}
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06004296 RID: 17046 RVA: 0x000EDC90 File Offset: 0x000ECC90
		private Color IEActiveLinkColor
		{
			get
			{
				return LinkUtilities.IEActiveLinkColor;
			}
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06004297 RID: 17047 RVA: 0x000EDC97 File Offset: 0x000ECC97
		private Color IEVisitedLinkColor
		{
			get
			{
				return LinkUtilities.IEVisitedLinkColor;
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06004298 RID: 17048 RVA: 0x000EDC9E File Offset: 0x000ECC9E
		private Color IEDisabledLinkColor
		{
			get
			{
				if (LinkLabel.iedisabledLinkColor.IsEmpty)
				{
					LinkLabel.iedisabledLinkColor = ControlPaint.Dark(base.DisabledColor);
				}
				return LinkLabel.iedisabledLinkColor;
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06004299 RID: 17049 RVA: 0x000EDCC1 File Offset: 0x000ECCC1
		private Rectangle ClientRectWithPadding
		{
			get
			{
				return LayoutUtils.DeflateRect(base.ClientRectangle, this.Padding);
			}
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x0600429A RID: 17050 RVA: 0x000EDCD4 File Offset: 0x000ECCD4
		// (set) Token: 0x0600429B RID: 17051 RVA: 0x000EDCDC File Offset: 0x000ECCDC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new FlatStyle FlatStyle
		{
			get
			{
				return base.FlatStyle;
			}
			set
			{
				base.FlatStyle = value;
			}
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x0600429C RID: 17052 RVA: 0x000EDCE8 File Offset: 0x000ECCE8
		// (set) Token: 0x0600429D RID: 17053 RVA: 0x000EDD3C File Offset: 0x000ECD3C
		[SRDescription("LinkLabelLinkAreaDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		[Editor("System.Windows.Forms.Design.LinkAreaEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		public LinkArea LinkArea
		{
			get
			{
				if (this.links.Count == 0)
				{
					return new LinkArea(0, 0);
				}
				return new LinkArea(((LinkLabel.Link)this.links[0]).Start, ((LinkLabel.Link)this.links[0]).Length);
			}
			set
			{
				LinkArea linkArea = this.LinkArea;
				this.links.Clear();
				if (!value.IsEmpty)
				{
					if (value.Start < 0)
					{
						throw new ArgumentOutOfRangeException("LinkArea", value, SR.GetString("LinkLabelAreaStart"));
					}
					if (value.Length < -1)
					{
						throw new ArgumentOutOfRangeException("LinkArea", value, SR.GetString("LinkLabelAreaLength"));
					}
					if (value.Start != 0 || value.Length != 0)
					{
						this.Links.Add(new LinkLabel.Link(this));
						((LinkLabel.Link)this.links[0]).Start = value.Start;
						((LinkLabel.Link)this.links[0]).Length = value.Length;
					}
				}
				this.UpdateSelectability();
				if (!linkArea.Equals(this.LinkArea))
				{
					this.InvalidateTextLayout();
					LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.LinkArea);
					base.AdjustSize();
					base.Invalidate();
				}
			}
		}

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x0600429E RID: 17054 RVA: 0x000EDE52 File Offset: 0x000ECE52
		// (set) Token: 0x0600429F RID: 17055 RVA: 0x000EDE5C File Offset: 0x000ECE5C
		[SRCategory("CatBehavior")]
		[DefaultValue(LinkBehavior.SystemDefault)]
		[SRDescription("LinkLabelLinkBehaviorDescr")]
		public LinkBehavior LinkBehavior
		{
			get
			{
				return this.linkBehavior;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("LinkBehavior", (int)value, typeof(LinkBehavior));
				}
				if (value != this.linkBehavior)
				{
					this.linkBehavior = value;
					this.InvalidateLinkFonts();
					this.InvalidateLink(null);
				}
			}
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x060042A0 RID: 17056 RVA: 0x000EDEAC File Offset: 0x000ECEAC
		// (set) Token: 0x060042A1 RID: 17057 RVA: 0x000EDED5 File Offset: 0x000ECED5
		[SRDescription("LinkLabelLinkColorDescr")]
		[SRCategory("CatAppearance")]
		public Color LinkColor
		{
			get
			{
				if (!this.linkColor.IsEmpty)
				{
					return this.linkColor;
				}
				if (SystemInformation.HighContrast)
				{
					return SystemColors.HotTrack;
				}
				return this.IELinkColor;
			}
			set
			{
				if (this.linkColor != value)
				{
					this.linkColor = value;
					this.InvalidateLink(null);
				}
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x060042A2 RID: 17058 RVA: 0x000EDEF3 File Offset: 0x000ECEF3
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public LinkLabel.LinkCollection Links
		{
			get
			{
				if (this.linkCollection == null)
				{
					this.linkCollection = new LinkLabel.LinkCollection(this);
				}
				return this.linkCollection;
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x060042A3 RID: 17059 RVA: 0x000EDF0F File Offset: 0x000ECF0F
		// (set) Token: 0x060042A4 RID: 17060 RVA: 0x000EDF38 File Offset: 0x000ECF38
		[SRCategory("CatAppearance")]
		[SRDescription("LinkLabelLinkVisitedDescr")]
		[DefaultValue(false)]
		public bool LinkVisited
		{
			get
			{
				return this.links.Count != 0 && ((LinkLabel.Link)this.links[0]).Visited;
			}
			set
			{
				if (value != this.LinkVisited)
				{
					if (this.links.Count == 0)
					{
						this.Links.Add(new LinkLabel.Link(this));
					}
					((LinkLabel.Link)this.links[0]).Visited = value;
				}
			}
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x060042A5 RID: 17061 RVA: 0x000EDF84 File Offset: 0x000ECF84
		internal override bool OwnerDraw
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x060042A6 RID: 17062 RVA: 0x000EDF87 File Offset: 0x000ECF87
		// (set) Token: 0x060042A7 RID: 17063 RVA: 0x000EDF90 File Offset: 0x000ECF90
		protected Cursor OverrideCursor
		{
			get
			{
				return this.overrideCursor;
			}
			set
			{
				if (this.overrideCursor != value)
				{
					this.overrideCursor = value;
					if (base.IsHandleCreated)
					{
						NativeMethods.POINT point = new NativeMethods.POINT();
						NativeMethods.RECT rect = default(NativeMethods.RECT);
						UnsafeNativeMethods.GetCursorPos(point);
						UnsafeNativeMethods.GetWindowRect(new HandleRef(this, base.Handle), ref rect);
						if ((rect.left <= point.x && point.x < rect.right && rect.top <= point.y && point.y < rect.bottom) || UnsafeNativeMethods.GetCapture() == base.Handle)
						{
							base.SendMessage(32, base.Handle, 1);
						}
					}
				}
			}
		}

		// Token: 0x1400025E RID: 606
		// (add) Token: 0x060042A8 RID: 17064 RVA: 0x000EE047 File Offset: 0x000ED047
		// (remove) Token: 0x060042A9 RID: 17065 RVA: 0x000EE050 File Offset: 0x000ED050
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public new event EventHandler TabStopChanged
		{
			add
			{
				base.TabStopChanged += value;
			}
			remove
			{
				base.TabStopChanged -= value;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x060042AA RID: 17066 RVA: 0x000EE059 File Offset: 0x000ED059
		// (set) Token: 0x060042AB RID: 17067 RVA: 0x000EE061 File Offset: 0x000ED061
		[RefreshProperties(RefreshProperties.Repaint)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x060042AC RID: 17068 RVA: 0x000EE06A File Offset: 0x000ED06A
		// (set) Token: 0x060042AD RID: 17069 RVA: 0x000EE072 File Offset: 0x000ED072
		[RefreshProperties(RefreshProperties.Repaint)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x060042AE RID: 17070 RVA: 0x000EE07C File Offset: 0x000ED07C
		// (set) Token: 0x060042AF RID: 17071 RVA: 0x000EE105 File Offset: 0x000ED105
		[SRCategory("CatAppearance")]
		[SRDescription("LinkLabelVisitedLinkColorDescr")]
		public Color VisitedLinkColor
		{
			get
			{
				if (!this.visitedLinkColor.IsEmpty)
				{
					return this.visitedLinkColor;
				}
				if (SystemInformation.HighContrast)
				{
					int red = (int)((SystemColors.Window.R + SystemColors.WindowText.R + 1) / 2);
					int g = (int)SystemColors.WindowText.G;
					int blue = (int)((SystemColors.Window.B + SystemColors.WindowText.B + 1) / 2);
					return Color.FromArgb(red, g, blue);
				}
				return this.IEVisitedLinkColor;
			}
			set
			{
				if (this.visitedLinkColor != value)
				{
					this.visitedLinkColor = value;
					this.InvalidateLink(null);
				}
			}
		}

		// Token: 0x1400025F RID: 607
		// (add) Token: 0x060042B0 RID: 17072 RVA: 0x000EE123 File Offset: 0x000ED123
		// (remove) Token: 0x060042B1 RID: 17073 RVA: 0x000EE136 File Offset: 0x000ED136
		[SRDescription("LinkLabelLinkClickedDescr")]
		[WinCategory("Action")]
		public event LinkLabelLinkClickedEventHandler LinkClicked
		{
			add
			{
				base.Events.AddHandler(LinkLabel.EventLinkClicked, value);
			}
			remove
			{
				base.Events.RemoveHandler(LinkLabel.EventLinkClicked, value);
			}
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x000EE14C File Offset: 0x000ED14C
		internal static Rectangle CalcTextRenderBounds(Rectangle textRect, Rectangle clientRect, ContentAlignment align)
		{
			int x;
			if ((align & WindowsFormsUtils.AnyRightAlign) != (ContentAlignment)0)
			{
				x = clientRect.Right - textRect.Width;
			}
			else if ((align & WindowsFormsUtils.AnyCenterAlign) != (ContentAlignment)0)
			{
				x = (clientRect.Width - textRect.Width) / 2;
			}
			else
			{
				x = clientRect.X;
			}
			int y;
			if ((align & WindowsFormsUtils.AnyBottomAlign) != (ContentAlignment)0)
			{
				y = clientRect.Bottom - textRect.Height;
			}
			else if ((align & WindowsFormsUtils.AnyMiddleAlign) != (ContentAlignment)0)
			{
				y = (clientRect.Height - textRect.Height) / 2;
			}
			else
			{
				y = clientRect.Y;
			}
			int width;
			if (textRect.Width > clientRect.Width)
			{
				x = clientRect.X;
				width = clientRect.Width;
			}
			else
			{
				width = textRect.Width;
			}
			int height;
			if (textRect.Height > clientRect.Height)
			{
				y = clientRect.Y;
				height = clientRect.Height;
			}
			else
			{
				height = textRect.Height;
			}
			return new Rectangle(x, y, width, height);
		}

		// Token: 0x060042B3 RID: 17075 RVA: 0x000EE236 File Offset: 0x000ED236
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new LinkLabel.LinkLabelAccessibleObject(this);
		}

		// Token: 0x060042B4 RID: 17076 RVA: 0x000EE23E File Offset: 0x000ED23E
		protected override void CreateHandle()
		{
			base.CreateHandle();
			this.InvalidateTextLayout();
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x060042B5 RID: 17077 RVA: 0x000EE24C File Offset: 0x000ED24C
		internal override bool CanUseTextRenderer
		{
			get
			{
				StringInfo stringInfo = new StringInfo(this.Text);
				return this.LinkArea.Start == 0 && (this.LinkArea.Length == 0 || this.LinkArea.Length == stringInfo.LengthInTextElements);
			}
		}

		// Token: 0x060042B6 RID: 17078 RVA: 0x000EE29F File Offset: 0x000ED29F
		internal override bool UseGDIMeasuring()
		{
			return !this.UseCompatibleTextRendering;
		}

		// Token: 0x060042B7 RID: 17079 RVA: 0x000EE2AC File Offset: 0x000ED2AC
		private static int ConvertToCharIndex(int index, string text)
		{
			if (index <= 0)
			{
				return 0;
			}
			if (string.IsNullOrEmpty(text))
			{
				return index;
			}
			StringInfo stringInfo = new StringInfo(text);
			int lengthInTextElements = stringInfo.LengthInTextElements;
			if (index > lengthInTextElements)
			{
				return index - lengthInTextElements + text.Length;
			}
			string text2 = stringInfo.SubstringByTextElements(0, index);
			return text2.Length;
		}

		// Token: 0x060042B8 RID: 17080 RVA: 0x000EE2F8 File Offset: 0x000ED2F8
		private void EnsureRun(Graphics g)
		{
			if (this.textLayoutValid)
			{
				return;
			}
			if (this.textRegion != null)
			{
				this.textRegion.Dispose();
				this.textRegion = null;
			}
			if (this.Text.Length == 0)
			{
				this.Links.Clear();
				this.Links.Add(new LinkLabel.Link(0, -1));
				this.textLayoutValid = true;
				return;
			}
			StringFormat stringFormat = this.CreateStringFormat();
			string text = this.Text;
			try
			{
				Font font = new Font(this.Font, this.Font.Style | FontStyle.Underline);
				Graphics graphics = null;
				try
				{
					if (g == null)
					{
						graphics = (g = base.CreateGraphicsInternal());
					}
					if (this.UseCompatibleTextRendering)
					{
						Region[] array = g.MeasureCharacterRanges(text, font, this.ClientRectWithPadding, stringFormat);
						int num = 0;
						for (int i = 0; i < this.Links.Count; i++)
						{
							LinkLabel.Link link = this.Links[i];
							int num2 = LinkLabel.ConvertToCharIndex(link.Start, text);
							int num3 = LinkLabel.ConvertToCharIndex(link.Start + link.Length, text);
							if (this.LinkInText(num2, num3 - num2))
							{
								this.Links[i].VisualRegion = array[num];
								num++;
							}
						}
						this.textRegion = array[array.Length - 1];
					}
					else
					{
						Rectangle clientRectWithPadding = this.ClientRectWithPadding;
						Size size = new Size(clientRectWithPadding.Width, clientRectWithPadding.Height);
						TextFormatFlags textFormatFlags = this.CreateTextFormatFlags(size);
						Size size2 = TextRenderer.MeasureText(text, font, size, textFormatFlags);
						int iLeftMargin;
						int iRightMargin;
						using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(g))
						{
							if ((textFormatFlags & TextFormatFlags.NoPadding) == TextFormatFlags.NoPadding)
							{
								windowsGraphics.TextPadding = TextPaddingOptions.NoPadding;
							}
							else if ((textFormatFlags & TextFormatFlags.LeftAndRightPadding) == TextFormatFlags.LeftAndRightPadding)
							{
								windowsGraphics.TextPadding = TextPaddingOptions.LeftAndRightPadding;
							}
							using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(this.Font))
							{
								IntNativeMethods.DRAWTEXTPARAMS textMargins = windowsGraphics.GetTextMargins(windowsFont);
								iLeftMargin = textMargins.iLeftMargin;
								iRightMargin = textMargins.iRightMargin;
							}
						}
						Rectangle rectangle = new Rectangle(clientRectWithPadding.X + iLeftMargin, clientRectWithPadding.Y, size2.Width - iRightMargin - iLeftMargin, size2.Height);
						rectangle = LinkLabel.CalcTextRenderBounds(rectangle, clientRectWithPadding, base.RtlTranslateContent(this.TextAlign));
						Region visualRegion = new Region(rectangle);
						if (this.links != null && this.links.Count == 1)
						{
							this.Links[0].VisualRegion = visualRegion;
						}
						this.textRegion = visualRegion;
					}
				}
				finally
				{
					font.Dispose();
					font = null;
					if (graphics != null)
					{
						graphics.Dispose();
						graphics = null;
					}
				}
				this.textLayoutValid = true;
			}
			finally
			{
				stringFormat.Dispose();
			}
		}

		// Token: 0x060042B9 RID: 17081 RVA: 0x000EE600 File Offset: 0x000ED600
		internal override StringFormat CreateStringFormat()
		{
			StringFormat stringFormat = base.CreateStringFormat();
			if (string.IsNullOrEmpty(this.Text))
			{
				return stringFormat;
			}
			CharacterRange[] measurableCharacterRanges = this.AdjustCharacterRangesForSurrogateChars();
			stringFormat.SetMeasurableCharacterRanges(measurableCharacterRanges);
			return stringFormat;
		}

		// Token: 0x060042BA RID: 17082 RVA: 0x000EE634 File Offset: 0x000ED634
		private CharacterRange[] AdjustCharacterRangesForSurrogateChars()
		{
			string text = this.Text;
			if (string.IsNullOrEmpty(text))
			{
				return new CharacterRange[0];
			}
			StringInfo stringInfo = new StringInfo(text);
			int lengthInTextElements = stringInfo.LengthInTextElements;
			ArrayList arrayList = new ArrayList(this.Links.Count);
			foreach (object obj in this.Links)
			{
				LinkLabel.Link link = (LinkLabel.Link)obj;
				int num = LinkLabel.ConvertToCharIndex(link.Start, text);
				int num2 = LinkLabel.ConvertToCharIndex(link.Start + link.Length, text);
				if (this.LinkInText(num, num2 - num))
				{
					int num3 = Math.Min(link.Length, lengthInTextElements - link.Start);
					arrayList.Add(new CharacterRange(num, LinkLabel.ConvertToCharIndex(link.Start + num3, text) - num));
				}
			}
			CharacterRange[] array = new CharacterRange[arrayList.Count + 1];
			arrayList.CopyTo(array, 0);
			array[array.Length - 1] = new CharacterRange(0, text.Length);
			return array;
		}

		// Token: 0x060042BB RID: 17083 RVA: 0x000EE774 File Offset: 0x000ED774
		private bool IsOneLink()
		{
			if (this.links == null || this.links.Count != 1 || this.Text == null)
			{
				return false;
			}
			StringInfo stringInfo = new StringInfo(this.Text);
			return this.LinkArea.Start == 0 && this.LinkArea.Length == stringInfo.LengthInTextElements;
		}

		// Token: 0x060042BC RID: 17084 RVA: 0x000EE7D8 File Offset: 0x000ED7D8
		protected LinkLabel.Link PointInLink(int x, int y)
		{
			Graphics graphics = base.CreateGraphicsInternal();
			LinkLabel.Link result = null;
			try
			{
				this.EnsureRun(graphics);
				foreach (object obj in this.links)
				{
					LinkLabel.Link link = (LinkLabel.Link)obj;
					if (link.VisualRegion != null && link.VisualRegion.IsVisible(x, y, graphics))
					{
						result = link;
						break;
					}
				}
			}
			finally
			{
				graphics.Dispose();
				graphics = null;
			}
			return result;
		}

		// Token: 0x060042BD RID: 17085 RVA: 0x000EE874 File Offset: 0x000ED874
		private void InvalidateLink(LinkLabel.Link link)
		{
			if (base.IsHandleCreated)
			{
				if (link == null || link.VisualRegion == null || this.IsOneLink())
				{
					base.Invalidate();
					return;
				}
				base.Invalidate(link.VisualRegion);
			}
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x000EE8A4 File Offset: 0x000ED8A4
		private void InvalidateLinkFonts()
		{
			if (this.linkFont != null)
			{
				this.linkFont.Dispose();
			}
			if (this.hoverLinkFont != null && this.hoverLinkFont != this.linkFont)
			{
				this.hoverLinkFont.Dispose();
			}
			this.linkFont = null;
			this.hoverLinkFont = null;
		}

		// Token: 0x060042BF RID: 17087 RVA: 0x000EE8F3 File Offset: 0x000ED8F3
		private void InvalidateTextLayout()
		{
			this.textLayoutValid = false;
		}

		// Token: 0x060042C0 RID: 17088 RVA: 0x000EE8FC File Offset: 0x000ED8FC
		private bool LinkInText(int start, int length)
		{
			return 0 <= start && start < this.Text.Length && 0 < length;
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x060042C1 RID: 17089 RVA: 0x000EE916 File Offset: 0x000ED916
		// (set) Token: 0x060042C2 RID: 17090 RVA: 0x000EE91E File Offset: 0x000ED91E
		DialogResult IButtonControl.DialogResult
		{
			get
			{
				return this.dialogResult;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DialogResult));
				}
				this.dialogResult = value;
			}
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x000EE94D File Offset: 0x000ED94D
		void IButtonControl.NotifyDefault(bool value)
		{
		}

		// Token: 0x060042C4 RID: 17092 RVA: 0x000EE950 File Offset: 0x000ED950
		protected override void OnGotFocus(EventArgs e)
		{
			if (!this.processingOnGotFocus)
			{
				base.OnGotFocus(e);
				this.processingOnGotFocus = true;
			}
			try
			{
				LinkLabel.Link link = this.FocusLink;
				if (link == null)
				{
					IntSecurity.ModifyFocus.Assert();
					this.Select(true, true);
				}
				else
				{
					this.InvalidateLink(link);
					this.UpdateAccessibilityLink(link);
				}
			}
			finally
			{
				if (this.processingOnGotFocus)
				{
					this.processingOnGotFocus = false;
				}
			}
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x000EE9C4 File Offset: 0x000ED9C4
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if (this.FocusLink != null)
			{
				this.InvalidateLink(this.FocusLink);
			}
		}

		// Token: 0x060042C6 RID: 17094 RVA: 0x000EE9E1 File Offset: 0x000ED9E1
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.KeyCode == Keys.Return && this.FocusLink != null && this.FocusLink.Enabled)
			{
				this.OnLinkClicked(new LinkLabelLinkClickedEventArgs(this.FocusLink));
			}
		}

		// Token: 0x060042C7 RID: 17095 RVA: 0x000EEA1C File Offset: 0x000EDA1C
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (!base.Enabled)
			{
				return;
			}
			foreach (object obj in this.links)
			{
				LinkLabel.Link link = (LinkLabel.Link)obj;
				if ((link.State & LinkState.Hover) == LinkState.Hover || (link.State & LinkState.Active) == LinkState.Active)
				{
					bool flag = (link.State & LinkState.Active) == LinkState.Active;
					link.State &= (LinkState)(-4);
					if (flag || this.hoverLinkFont != this.linkFont)
					{
						this.InvalidateLink(link);
					}
					this.OverrideCursor = null;
				}
			}
		}

		// Token: 0x060042C8 RID: 17096 RVA: 0x000EEAD0 File Offset: 0x000EDAD0
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!base.Enabled || e.Clicks > 1)
			{
				this.receivedDoubleClick = true;
				return;
			}
			for (int i = 0; i < this.links.Count; i++)
			{
				if ((((LinkLabel.Link)this.links[i]).State & LinkState.Hover) == LinkState.Hover)
				{
					((LinkLabel.Link)this.links[i]).State |= LinkState.Active;
					this.FocusInternal();
					if (((LinkLabel.Link)this.links[i]).Enabled)
					{
						this.FocusLink = (LinkLabel.Link)this.links[i];
						this.InvalidateLink(this.FocusLink);
					}
					base.CaptureInternal = true;
					return;
				}
			}
		}

		// Token: 0x060042C9 RID: 17097 RVA: 0x000EEB9C File Offset: 0x000EDB9C
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (base.Disposing || base.IsDisposed)
			{
				return;
			}
			if (!base.Enabled || e.Clicks > 1 || this.receivedDoubleClick)
			{
				this.receivedDoubleClick = false;
				return;
			}
			for (int i = 0; i < this.links.Count; i++)
			{
				if ((((LinkLabel.Link)this.links[i]).State & LinkState.Active) == LinkState.Active)
				{
					((LinkLabel.Link)this.links[i]).State &= (LinkState)(-3);
					this.InvalidateLink((LinkLabel.Link)this.links[i]);
					base.CaptureInternal = false;
					LinkLabel.Link link = this.PointInLink(e.X, e.Y);
					if (link != null && link == this.FocusLink && link.Enabled)
					{
						this.OnLinkClicked(new LinkLabelLinkClickedEventArgs(link, e.Button));
					}
				}
			}
		}

		// Token: 0x060042CA RID: 17098 RVA: 0x000EEC90 File Offset: 0x000EDC90
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!base.Enabled)
			{
				return;
			}
			LinkLabel.Link link = null;
			foreach (object obj in this.links)
			{
				LinkLabel.Link link2 = (LinkLabel.Link)obj;
				if ((link2.State & LinkState.Hover) == LinkState.Hover)
				{
					link = link2;
					break;
				}
			}
			LinkLabel.Link link3 = this.PointInLink(e.X, e.Y);
			if (link3 != link)
			{
				if (link != null)
				{
					link.State &= (LinkState)(-2);
				}
				if (link3 != null)
				{
					link3.State |= LinkState.Hover;
					if (link3.Enabled)
					{
						this.OverrideCursor = Cursors.Hand;
					}
				}
				else
				{
					this.OverrideCursor = null;
				}
				if (this.hoverLinkFont != this.linkFont)
				{
					if (link != null)
					{
						this.InvalidateLink(link);
					}
					if (link3 != null)
					{
						this.InvalidateLink(link3);
					}
				}
			}
		}

		// Token: 0x060042CB RID: 17099 RVA: 0x000EED80 File Offset: 0x000EDD80
		protected virtual void OnLinkClicked(LinkLabelLinkClickedEventArgs e)
		{
			LinkLabelLinkClickedEventHandler linkLabelLinkClickedEventHandler = (LinkLabelLinkClickedEventHandler)base.Events[LinkLabel.EventLinkClicked];
			if (linkLabelLinkClickedEventHandler != null)
			{
				linkLabelLinkClickedEventHandler(this, e);
			}
		}

		// Token: 0x060042CC RID: 17100 RVA: 0x000EEDAE File Offset: 0x000EDDAE
		protected override void OnPaddingChanged(EventArgs e)
		{
			base.OnPaddingChanged(e);
			this.InvalidateTextLayout();
		}

		// Token: 0x060042CD RID: 17101 RVA: 0x000EEDC0 File Offset: 0x000EDDC0
		protected override void OnPaint(PaintEventArgs e)
		{
			RectangleF rectangleF = RectangleF.Empty;
			base.Animate();
			ImageAnimator.UpdateFrames(base.Image);
			this.EnsureRun(e.Graphics);
			if (this.Text.Length == 0)
			{
				this.PaintLinkBackground(e.Graphics);
			}
			else
			{
				if (base.AutoEllipsis)
				{
					Rectangle clientRectWithPadding = this.ClientRectWithPadding;
					Size preferredSize = this.GetPreferredSize(new Size(clientRectWithPadding.Width, clientRectWithPadding.Height));
					this.showToolTip = (clientRectWithPadding.Width < preferredSize.Width || clientRectWithPadding.Height < preferredSize.Height);
				}
				else
				{
					this.showToolTip = false;
				}
				if (base.Enabled)
				{
					bool flag = !base.GetStyle(ControlStyles.OptimizedDoubleBuffer);
					SolidBrush solidBrush = new SolidBrush(this.ForeColor);
					SolidBrush solidBrush2 = new SolidBrush(this.LinkColor);
					try
					{
						if (!flag)
						{
							this.PaintLinkBackground(e.Graphics);
						}
						LinkUtilities.EnsureLinkFonts(this.Font, this.LinkBehavior, ref this.linkFont, ref this.hoverLinkFont);
						Region clip = e.Graphics.Clip;
						try
						{
							if (this.IsOneLink())
							{
								e.Graphics.Clip = clip;
								RectangleF[] regionScans = ((LinkLabel.Link)this.links[0]).VisualRegion.GetRegionScans(e.Graphics.Transform);
								if (regionScans != null && regionScans.Length > 0)
								{
									if (this.UseCompatibleTextRendering)
									{
										rectangleF = new RectangleF(regionScans[0].Location, SizeF.Empty);
										foreach (RectangleF b in regionScans)
										{
											rectangleF = RectangleF.Union(rectangleF, b);
										}
									}
									else
									{
										rectangleF = this.ClientRectWithPadding;
										Size size = rectangleF.Size.ToSize();
										Size textSize = base.MeasureTextCache.GetTextSize(this.Text, this.Font, size, this.CreateTextFormatFlags(size));
										rectangleF.Width = (float)textSize.Width;
										if ((float)textSize.Height < rectangleF.Height)
										{
											rectangleF.Height = (float)textSize.Height;
										}
										rectangleF = LinkLabel.CalcTextRenderBounds(Rectangle.Round(rectangleF), this.ClientRectWithPadding, base.RtlTranslateContent(this.TextAlign));
									}
									e.Graphics.ExcludeClip(new Region(rectangleF));
								}
							}
							else
							{
								foreach (object obj in this.links)
								{
									LinkLabel.Link link = (LinkLabel.Link)obj;
									if (link.VisualRegion != null)
									{
										e.Graphics.ExcludeClip(link.VisualRegion);
									}
								}
							}
							if (!this.IsOneLink())
							{
								this.PaintLink(e.Graphics, null, solidBrush, solidBrush2, flag, rectangleF);
							}
							foreach (object obj2 in this.links)
							{
								LinkLabel.Link link2 = (LinkLabel.Link)obj2;
								this.PaintLink(e.Graphics, link2, solidBrush, solidBrush2, flag, rectangleF);
							}
							if (flag)
							{
								e.Graphics.Clip = clip;
								e.Graphics.ExcludeClip(this.textRegion);
								this.PaintLinkBackground(e.Graphics);
							}
						}
						finally
						{
							e.Graphics.Clip = clip;
						}
						goto IL_451;
					}
					finally
					{
						solidBrush.Dispose();
						solidBrush2.Dispose();
					}
				}
				Region clip2 = e.Graphics.Clip;
				try
				{
					this.PaintLinkBackground(e.Graphics);
					e.Graphics.IntersectClip(this.textRegion);
					if (this.UseCompatibleTextRendering)
					{
						StringFormat format = this.CreateStringFormat();
						ControlPaint.DrawStringDisabled(e.Graphics, this.Text, this.Font, base.DisabledColor, this.ClientRectWithPadding, format);
					}
					else
					{
						IntPtr hdc = e.Graphics.GetHdc();
						Color nearestColor;
						try
						{
							using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
							{
								nearestColor = windowsGraphics.GetNearestColor(base.DisabledColor);
							}
						}
						finally
						{
							e.Graphics.ReleaseHdc();
						}
						Rectangle clientRectWithPadding2 = this.ClientRectWithPadding;
						ControlPaint.DrawStringDisabled(e.Graphics, this.Text, this.Font, nearestColor, clientRectWithPadding2, this.CreateTextFormatFlags(clientRectWithPadding2.Size));
					}
				}
				finally
				{
					e.Graphics.Clip = clip2;
				}
			}
			IL_451:
			base.RaisePaintEvent(this, e);
		}

		// Token: 0x060042CE RID: 17102 RVA: 0x000EF2D4 File Offset: 0x000EE2D4
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			Image image = base.Image;
			if (image != null)
			{
				Region clip = e.Graphics.Clip;
				Rectangle rect = base.CalcImageRenderBounds(image, base.ClientRectangle, base.RtlTranslateAlignment(base.ImageAlign));
				e.Graphics.ExcludeClip(rect);
				try
				{
					base.OnPaintBackground(e);
				}
				finally
				{
					e.Graphics.Clip = clip;
				}
				e.Graphics.IntersectClip(rect);
				try
				{
					base.OnPaintBackground(e);
					base.DrawImage(e.Graphics, image, base.ClientRectangle, base.RtlTranslateAlignment(base.ImageAlign));
					return;
				}
				finally
				{
					e.Graphics.Clip = clip;
				}
			}
			base.OnPaintBackground(e);
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x000EF39C File Offset: 0x000EE39C
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.InvalidateTextLayout();
			this.InvalidateLinkFonts();
			base.Invalidate();
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x000EF3B7 File Offset: 0x000EE3B7
		protected override void OnAutoSizeChanged(EventArgs e)
		{
			base.OnAutoSizeChanged(e);
			this.InvalidateTextLayout();
		}

		// Token: 0x060042D1 RID: 17105 RVA: 0x000EF3C6 File Offset: 0x000EE3C6
		internal override void OnAutoEllipsisChanged()
		{
			base.OnAutoEllipsisChanged();
			this.InvalidateTextLayout();
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x000EF3D4 File Offset: 0x000EE3D4
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			if (!base.Enabled)
			{
				for (int i = 0; i < this.links.Count; i++)
				{
					((LinkLabel.Link)this.links[i]).State &= (LinkState)(-4);
				}
				this.OverrideCursor = null;
			}
			this.InvalidateTextLayout();
			base.Invalidate();
		}

		// Token: 0x060042D3 RID: 17107 RVA: 0x000EF438 File Offset: 0x000EE438
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			this.InvalidateTextLayout();
			this.UpdateSelectability();
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x000EF44D File Offset: 0x000EE44D
		protected override void OnTextAlignChanged(EventArgs e)
		{
			base.OnTextAlignChanged(e);
			this.InvalidateTextLayout();
			this.UpdateSelectability();
		}

		// Token: 0x060042D5 RID: 17109 RVA: 0x000EF464 File Offset: 0x000EE464
		private void PaintLink(Graphics g, LinkLabel.Link link, SolidBrush foreBrush, SolidBrush linkBrush, bool optimizeBackgroundRendering, RectangleF finalrect)
		{
			Font font = this.Font;
			if (link != null)
			{
				if (link.VisualRegion != null)
				{
					Color color = Color.Empty;
					LinkState state = link.State;
					if ((state & LinkState.Hover) == LinkState.Hover)
					{
						font = this.hoverLinkFont;
					}
					else
					{
						font = this.linkFont;
					}
					if (link.Enabled)
					{
						if ((state & LinkState.Active) == LinkState.Active)
						{
							color = this.ActiveLinkColor;
						}
						else if ((state & LinkState.Visited) == LinkState.Visited)
						{
							color = this.VisitedLinkColor;
						}
					}
					else
					{
						color = this.DisabledLinkColor;
					}
					if (this.IsOneLink())
					{
						g.Clip = new Region(finalrect);
					}
					else
					{
						g.Clip = link.VisualRegion;
					}
					if (optimizeBackgroundRendering)
					{
						this.PaintLinkBackground(g);
					}
					if (this.UseCompatibleTextRendering)
					{
						SolidBrush solidBrush = (color == Color.Empty) ? linkBrush : new SolidBrush(color);
						StringFormat format = this.CreateStringFormat();
						g.DrawString(this.Text, font, solidBrush, this.ClientRectWithPadding, format);
						if (solidBrush != linkBrush)
						{
							solidBrush.Dispose();
						}
					}
					else
					{
						if (color == Color.Empty)
						{
							color = linkBrush.Color;
						}
						IntPtr hdc = g.GetHdc();
						try
						{
							using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
							{
								color = windowsGraphics.GetNearestColor(color);
							}
						}
						finally
						{
							g.ReleaseHdc();
						}
						Rectangle clientRectWithPadding = this.ClientRectWithPadding;
						TextRenderer.DrawText(g, this.Text, font, clientRectWithPadding, color, this.CreateTextFormatFlags(clientRectWithPadding.Size));
					}
					if (this.Focused && this.ShowFocusCues && this.FocusLink == link)
					{
						RectangleF[] regionScans = link.VisualRegion.GetRegionScans(g.Transform);
						if (regionScans != null && regionScans.Length > 0)
						{
							if (this.IsOneLink())
							{
								Rectangle rectangle = Rectangle.Ceiling(finalrect);
								ControlPaint.DrawFocusRectangle(g, rectangle, this.ForeColor, this.BackColor);
								return;
							}
							foreach (RectangleF value in regionScans)
							{
								ControlPaint.DrawFocusRectangle(g, Rectangle.Ceiling(value), this.ForeColor, this.BackColor);
							}
							return;
						}
					}
				}
			}
			else
			{
				g.IntersectClip(this.textRegion);
				if (optimizeBackgroundRendering)
				{
					this.PaintLinkBackground(g);
				}
				if (this.UseCompatibleTextRendering)
				{
					StringFormat format2 = this.CreateStringFormat();
					g.DrawString(this.Text, font, foreBrush, this.ClientRectWithPadding, format2);
					return;
				}
				IntPtr hdc2 = g.GetHdc();
				Color nearestColor;
				try
				{
					using (WindowsGraphics windowsGraphics2 = WindowsGraphics.FromHdc(hdc2))
					{
						nearestColor = windowsGraphics2.GetNearestColor(foreBrush.Color);
					}
				}
				finally
				{
					g.ReleaseHdc();
				}
				Rectangle clientRectWithPadding2 = this.ClientRectWithPadding;
				TextRenderer.DrawText(g, this.Text, font, clientRectWithPadding2, nearestColor, this.CreateTextFormatFlags(clientRectWithPadding2.Size));
			}
		}

		// Token: 0x060042D6 RID: 17110 RVA: 0x000EF744 File Offset: 0x000EE744
		private void PaintLinkBackground(Graphics g)
		{
			using (PaintEventArgs paintEventArgs = new PaintEventArgs(g, base.ClientRectangle))
			{
				base.InvokePaintBackground(this, paintEventArgs);
			}
		}

		// Token: 0x060042D7 RID: 17111 RVA: 0x000EF784 File Offset: 0x000EE784
		void IButtonControl.PerformClick()
		{
			if (this.FocusLink == null && this.Links.Count > 0)
			{
				string text = this.Text;
				foreach (object obj in this.Links)
				{
					LinkLabel.Link link = (LinkLabel.Link)obj;
					int num = LinkLabel.ConvertToCharIndex(link.Start, text);
					int num2 = LinkLabel.ConvertToCharIndex(link.Start + link.Length, text);
					if (link.Enabled && this.LinkInText(num, num2 - num))
					{
						this.FocusLink = link;
						break;
					}
				}
			}
			if (this.FocusLink != null)
			{
				this.OnLinkClicked(new LinkLabelLinkClickedEventArgs(this.FocusLink));
			}
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x000EF858 File Offset: 0x000EE858
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & (Keys.Control | Keys.Alt)) != Keys.Alt)
			{
				Keys keys = keyData & Keys.KeyCode;
				Keys keys2 = keys;
				if (keys2 != Keys.Tab)
				{
					switch (keys2)
					{
					case Keys.Left:
					case Keys.Up:
						if (this.FocusNextLink(false))
						{
							return true;
						}
						break;
					case Keys.Right:
					case Keys.Down:
						if (this.FocusNextLink(true))
						{
							return true;
						}
						break;
					}
				}
				else if (base.TabStop)
				{
					bool forward = (keyData & Keys.Shift) != Keys.Shift;
					if (this.FocusNextLink(forward))
					{
						return true;
					}
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x000EF8E0 File Offset: 0x000EE8E0
		private bool FocusNextLink(bool forward)
		{
			int num = -1;
			if (this.focusLink != null)
			{
				for (int i = 0; i < this.links.Count; i++)
				{
					if (this.links[i] == this.focusLink)
					{
						num = i;
						break;
					}
				}
			}
			num = this.GetNextLinkIndex(num, forward);
			if (num != -1)
			{
				this.FocusLink = this.Links[num];
				return true;
			}
			this.FocusLink = null;
			return false;
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x000EF950 File Offset: 0x000EE950
		private int GetNextLinkIndex(int focusIndex, bool forward)
		{
			string text = this.Text;
			int num = 0;
			int num2 = 0;
			if (forward)
			{
				do
				{
					focusIndex++;
					LinkLabel.Link link;
					if (focusIndex < this.Links.Count)
					{
						link = this.Links[focusIndex];
						num = LinkLabel.ConvertToCharIndex(link.Start, text);
						num2 = LinkLabel.ConvertToCharIndex(link.Start + link.Length, text);
					}
					else
					{
						link = null;
					}
					if (link == null || link.Enabled)
					{
						break;
					}
				}
				while (this.LinkInText(num, num2 - num));
			}
			else
			{
				LinkLabel.Link link;
				do
				{
					focusIndex--;
					if (focusIndex >= 0)
					{
						link = this.Links[focusIndex];
						num = LinkLabel.ConvertToCharIndex(link.Start, text);
						num2 = LinkLabel.ConvertToCharIndex(link.Start + link.Length, text);
					}
					else
					{
						link = null;
					}
				}
				while (link != null && !link.Enabled && this.LinkInText(num, num2 - num));
			}
			if (focusIndex < 0 || focusIndex >= this.links.Count)
			{
				return -1;
			}
			return focusIndex;
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x000EFA30 File Offset: 0x000EEA30
		private void ResetLinkArea()
		{
			this.LinkArea = new LinkArea(0, -1);
		}

		// Token: 0x060042DC RID: 17116 RVA: 0x000EFA3F File Offset: 0x000EEA3F
		internal void ResetActiveLinkColor()
		{
			this.activeLinkColor = Color.Empty;
		}

		// Token: 0x060042DD RID: 17117 RVA: 0x000EFA4C File Offset: 0x000EEA4C
		internal void ResetDisabledLinkColor()
		{
			this.disabledLinkColor = Color.Empty;
		}

		// Token: 0x060042DE RID: 17118 RVA: 0x000EFA59 File Offset: 0x000EEA59
		internal void ResetLinkColor()
		{
			this.linkColor = Color.Empty;
			this.InvalidateLink(null);
		}

		// Token: 0x060042DF RID: 17119 RVA: 0x000EFA6D File Offset: 0x000EEA6D
		private void ResetVisitedLinkColor()
		{
			this.visitedLinkColor = Color.Empty;
		}

		// Token: 0x060042E0 RID: 17120 RVA: 0x000EFA7A File Offset: 0x000EEA7A
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			this.InvalidateTextLayout();
			base.Invalidate();
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x060042E1 RID: 17121 RVA: 0x000EFA98 File Offset: 0x000EEA98
		protected override void Select(bool directed, bool forward)
		{
			if (directed && this.links.Count > 0)
			{
				int focusIndex = -1;
				if (this.FocusLink != null)
				{
					focusIndex = this.links.IndexOf(this.FocusLink);
				}
				this.FocusLink = null;
				int nextLinkIndex = this.GetNextLinkIndex(focusIndex, forward);
				if (nextLinkIndex == -1)
				{
					if (forward)
					{
						nextLinkIndex = this.GetNextLinkIndex(-1, forward);
					}
					else
					{
						nextLinkIndex = this.GetNextLinkIndex(this.links.Count, forward);
					}
				}
				if (nextLinkIndex != -1)
				{
					this.FocusLink = (LinkLabel.Link)this.links[nextLinkIndex];
				}
			}
			base.Select(directed, forward);
		}

		// Token: 0x060042E2 RID: 17122 RVA: 0x000EFB2A File Offset: 0x000EEB2A
		internal bool ShouldSerializeActiveLinkColor()
		{
			return !this.activeLinkColor.IsEmpty;
		}

		// Token: 0x060042E3 RID: 17123 RVA: 0x000EFB3A File Offset: 0x000EEB3A
		internal bool ShouldSerializeDisabledLinkColor()
		{
			return !this.disabledLinkColor.IsEmpty;
		}

		// Token: 0x060042E4 RID: 17124 RVA: 0x000EFB4A File Offset: 0x000EEB4A
		private bool ShouldSerializeLinkArea()
		{
			return this.links.Count != 1 || this.Links[0].Start != 0 || this.Links[0].length != -1;
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x000EFB88 File Offset: 0x000EEB88
		internal bool ShouldSerializeLinkColor()
		{
			return !this.linkColor.IsEmpty;
		}

		// Token: 0x060042E6 RID: 17126 RVA: 0x000EFB98 File Offset: 0x000EEB98
		private bool ShouldSerializeUseCompatibleTextRendering()
		{
			return !this.CanUseTextRenderer || this.UseCompatibleTextRendering != Control.UseCompatibleTextRenderingDefault;
		}

		// Token: 0x060042E7 RID: 17127 RVA: 0x000EFBB4 File Offset: 0x000EEBB4
		private bool ShouldSerializeVisitedLinkColor()
		{
			return !this.visitedLinkColor.IsEmpty;
		}

		// Token: 0x060042E8 RID: 17128 RVA: 0x000EFBC4 File Offset: 0x000EEBC4
		private void UpdateAccessibilityLink(LinkLabel.Link focusLink)
		{
			if (!base.IsHandleCreated)
			{
				return;
			}
			int childID = -1;
			for (int i = 0; i < this.links.Count; i++)
			{
				if (this.links[i] == focusLink)
				{
					childID = i;
				}
			}
			base.AccessibilityNotifyClients(AccessibleEvents.Focus, childID);
		}

		// Token: 0x060042E9 RID: 17129 RVA: 0x000EFC10 File Offset: 0x000EEC10
		private void ValidateNoOverlappingLinks()
		{
			for (int i = 0; i < this.links.Count; i++)
			{
				LinkLabel.Link link = (LinkLabel.Link)this.links[i];
				if (link.Length < 0)
				{
					throw new InvalidOperationException(SR.GetString("LinkLabelOverlap"));
				}
				for (int j = i; j < this.links.Count; j++)
				{
					if (i != j)
					{
						LinkLabel.Link link2 = (LinkLabel.Link)this.links[j];
						int num = Math.Max(link.Start, link2.Start);
						int num2 = Math.Min(link.Start + link.Length, link2.Start + link2.Length);
						if (num < num2)
						{
							throw new InvalidOperationException(SR.GetString("LinkLabelOverlap"));
						}
					}
				}
			}
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x000EFCDC File Offset: 0x000EECDC
		private void UpdateSelectability()
		{
			LinkArea linkArea = this.LinkArea;
			bool flag = false;
			string text = this.Text;
			int num = LinkLabel.ConvertToCharIndex(linkArea.Start, text);
			int num2 = LinkLabel.ConvertToCharIndex(linkArea.Start + linkArea.Length, text);
			if (this.LinkInText(num, num2 - num))
			{
				flag = true;
			}
			else if (this.FocusLink != null)
			{
				this.FocusLink = null;
			}
			this.OverrideCursor = null;
			base.TabStop = flag;
			base.SetStyle(ControlStyles.Selectable, flag);
		}

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x060042EB RID: 17131 RVA: 0x000EFD58 File Offset: 0x000EED58
		// (set) Token: 0x060042EC RID: 17132 RVA: 0x000EFD60 File Offset: 0x000EED60
		[SRCategory("CatBehavior")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("UseCompatibleTextRenderingDescr")]
		public new bool UseCompatibleTextRendering
		{
			get
			{
				return base.UseCompatibleTextRendering;
			}
			set
			{
				if (base.UseCompatibleTextRendering != value)
				{
					base.UseCompatibleTextRendering = value;
					this.InvalidateTextLayout();
				}
			}
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x000EFD78 File Offset: 0x000EED78
		private void WmSetCursor(ref Message m)
		{
			if (!(m.WParam == base.InternalHandle) || NativeMethods.Util.LOWORD(m.LParam) != 1)
			{
				this.DefWndProc(ref m);
				return;
			}
			if (this.OverrideCursor != null)
			{
				Cursor.CurrentInternal = this.OverrideCursor;
				return;
			}
			Cursor.CurrentInternal = this.Cursor;
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x000EFDD4 File Offset: 0x000EEDD4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message msg)
		{
			int msg2 = msg.Msg;
			if (msg2 == 32)
			{
				this.WmSetCursor(ref msg);
				return;
			}
			base.WndProc(ref msg);
		}

		// Token: 0x040020A8 RID: 8360
		private static readonly object EventLinkClicked = new object();

		// Token: 0x040020A9 RID: 8361
		private static Color iedisabledLinkColor = Color.Empty;

		// Token: 0x040020AA RID: 8362
		private static LinkLabel.LinkComparer linkComparer = new LinkLabel.LinkComparer();

		// Token: 0x040020AB RID: 8363
		private DialogResult dialogResult;

		// Token: 0x040020AC RID: 8364
		private Color linkColor = Color.Empty;

		// Token: 0x040020AD RID: 8365
		private Color activeLinkColor = Color.Empty;

		// Token: 0x040020AE RID: 8366
		private Color visitedLinkColor = Color.Empty;

		// Token: 0x040020AF RID: 8367
		private Color disabledLinkColor = Color.Empty;

		// Token: 0x040020B0 RID: 8368
		private Font linkFont;

		// Token: 0x040020B1 RID: 8369
		private Font hoverLinkFont;

		// Token: 0x040020B2 RID: 8370
		private bool textLayoutValid;

		// Token: 0x040020B3 RID: 8371
		private bool receivedDoubleClick;

		// Token: 0x040020B4 RID: 8372
		private ArrayList links = new ArrayList(2);

		// Token: 0x040020B5 RID: 8373
		private LinkLabel.Link focusLink;

		// Token: 0x040020B6 RID: 8374
		private LinkLabel.LinkCollection linkCollection;

		// Token: 0x040020B7 RID: 8375
		private Region textRegion;

		// Token: 0x040020B8 RID: 8376
		private Cursor overrideCursor;

		// Token: 0x040020B9 RID: 8377
		private bool processingOnGotFocus;

		// Token: 0x040020BA RID: 8378
		private LinkBehavior linkBehavior;

		// Token: 0x02000470 RID: 1136
		public class LinkCollection : IList, ICollection, IEnumerable
		{
			// Token: 0x060042F0 RID: 17136 RVA: 0x000EFE1C File Offset: 0x000EEE1C
			public LinkCollection(LinkLabel owner)
			{
				if (owner == null)
				{
					throw new ArgumentNullException("owner");
				}
				this.owner = owner;
			}

			// Token: 0x17000D13 RID: 3347
			public virtual LinkLabel.Link this[int index]
			{
				get
				{
					return (LinkLabel.Link)this.owner.links[index];
				}
				set
				{
					this.owner.links[index] = value;
					this.owner.links.Sort(LinkLabel.linkComparer);
					this.owner.InvalidateTextLayout();
					this.owner.Invalidate();
				}
			}

			// Token: 0x17000D14 RID: 3348
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (value is LinkLabel.Link)
					{
						this[index] = (LinkLabel.Link)value;
						return;
					}
					throw new ArgumentException(SR.GetString("LinkLabelBadLink"), "value");
				}
			}

			// Token: 0x17000D15 RID: 3349
			public virtual LinkLabel.Link this[string key]
			{
				get
				{
					if (string.IsNullOrEmpty(key))
					{
						return null;
					}
					int index = this.IndexOfKey(key);
					if (this.IsValidIndex(index))
					{
						return this[index];
					}
					return null;
				}
			}

			// Token: 0x17000D16 RID: 3350
			// (get) Token: 0x060042F6 RID: 17142 RVA: 0x000EFEFD File Offset: 0x000EEEFD
			[Browsable(false)]
			public int Count
			{
				get
				{
					return this.owner.links.Count;
				}
			}

			// Token: 0x17000D17 RID: 3351
			// (get) Token: 0x060042F7 RID: 17143 RVA: 0x000EFF0F File Offset: 0x000EEF0F
			public bool LinksAdded
			{
				get
				{
					return this.linksAdded;
				}
			}

			// Token: 0x17000D18 RID: 3352
			// (get) Token: 0x060042F8 RID: 17144 RVA: 0x000EFF17 File Offset: 0x000EEF17
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000D19 RID: 3353
			// (get) Token: 0x060042F9 RID: 17145 RVA: 0x000EFF1A File Offset: 0x000EEF1A
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000D1A RID: 3354
			// (get) Token: 0x060042FA RID: 17146 RVA: 0x000EFF1D File Offset: 0x000EEF1D
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000D1B RID: 3355
			// (get) Token: 0x060042FB RID: 17147 RVA: 0x000EFF20 File Offset: 0x000EEF20
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060042FC RID: 17148 RVA: 0x000EFF23 File Offset: 0x000EEF23
			public LinkLabel.Link Add(int start, int length)
			{
				if (length != 0)
				{
					this.linksAdded = true;
				}
				return this.Add(start, length, null);
			}

			// Token: 0x060042FD RID: 17149 RVA: 0x000EFF38 File Offset: 0x000EEF38
			public LinkLabel.Link Add(int start, int length, object linkData)
			{
				if (length != 0)
				{
					this.linksAdded = true;
				}
				if (this.owner.links.Count == 1 && this[0].Start == 0 && this[0].length == -1)
				{
					this.owner.links.Clear();
				}
				LinkLabel.Link link = new LinkLabel.Link(this.owner);
				link.Start = start;
				link.Length = length;
				link.LinkData = linkData;
				this.Add(link);
				return link;
			}

			// Token: 0x060042FE RID: 17150 RVA: 0x000EFFBC File Offset: 0x000EEFBC
			public int Add(LinkLabel.Link value)
			{
				if (value != null && value.Length != 0)
				{
					this.linksAdded = true;
				}
				if (this.owner.links.Count == 1 && this[0].Start == 0 && this[0].length == -1)
				{
					this.owner.links.Clear();
				}
				value.Owner = this.owner;
				this.owner.links.Add(value);
				if (this.owner.AutoSize)
				{
					LayoutTransaction.DoLayout(this.owner.ParentInternal, this.owner, PropertyNames.Links);
					this.owner.AdjustSize();
					this.owner.Invalidate();
				}
				if (this.owner.Links.Count > 1)
				{
					this.owner.links.Sort(LinkLabel.linkComparer);
				}
				this.owner.ValidateNoOverlappingLinks();
				this.owner.UpdateSelectability();
				this.owner.InvalidateTextLayout();
				this.owner.Invalidate();
				if (this.owner.Links.Count > 1)
				{
					return this.IndexOf(value);
				}
				return 0;
			}

			// Token: 0x060042FF RID: 17151 RVA: 0x000F00E7 File Offset: 0x000EF0E7
			int IList.Add(object value)
			{
				if (value is LinkLabel.Link)
				{
					return this.Add((LinkLabel.Link)value);
				}
				throw new ArgumentException(SR.GetString("LinkLabelBadLink"), "value");
			}

			// Token: 0x06004300 RID: 17152 RVA: 0x000F0112 File Offset: 0x000EF112
			void IList.Insert(int index, object value)
			{
				if (value is LinkLabel.Link)
				{
					this.Add((LinkLabel.Link)value);
					return;
				}
				throw new ArgumentException(SR.GetString("LinkLabelBadLink"), "value");
			}

			// Token: 0x06004301 RID: 17153 RVA: 0x000F013E File Offset: 0x000EF13E
			public bool Contains(LinkLabel.Link link)
			{
				return this.owner.links.Contains(link);
			}

			// Token: 0x06004302 RID: 17154 RVA: 0x000F0151 File Offset: 0x000EF151
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			// Token: 0x06004303 RID: 17155 RVA: 0x000F0160 File Offset: 0x000EF160
			bool IList.Contains(object link)
			{
				return link is LinkLabel.Link && this.Contains((LinkLabel.Link)link);
			}

			// Token: 0x06004304 RID: 17156 RVA: 0x000F0178 File Offset: 0x000EF178
			public int IndexOf(LinkLabel.Link link)
			{
				return this.owner.links.IndexOf(link);
			}

			// Token: 0x06004305 RID: 17157 RVA: 0x000F018B File Offset: 0x000EF18B
			int IList.IndexOf(object link)
			{
				if (link is LinkLabel.Link)
				{
					return this.IndexOf((LinkLabel.Link)link);
				}
				return -1;
			}

			// Token: 0x06004306 RID: 17158 RVA: 0x000F01A4 File Offset: 0x000EF1A4
			public virtual int IndexOfKey(string key)
			{
				if (string.IsNullOrEmpty(key))
				{
					return -1;
				}
				if (this.IsValidIndex(this.lastAccessedIndex) && WindowsFormsUtils.SafeCompareStrings(this[this.lastAccessedIndex].Name, key, true))
				{
					return this.lastAccessedIndex;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, true))
					{
						this.lastAccessedIndex = i;
						return i;
					}
				}
				this.lastAccessedIndex = -1;
				return -1;
			}

			// Token: 0x06004307 RID: 17159 RVA: 0x000F0221 File Offset: 0x000EF221
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			// Token: 0x06004308 RID: 17160 RVA: 0x000F0234 File Offset: 0x000EF234
			public virtual void Clear()
			{
				bool flag = this.owner.links.Count > 0 && this.owner.AutoSize;
				this.owner.links.Clear();
				if (flag)
				{
					LayoutTransaction.DoLayout(this.owner.ParentInternal, this.owner, PropertyNames.Links);
					this.owner.AdjustSize();
					this.owner.Invalidate();
				}
				this.owner.UpdateSelectability();
				this.owner.InvalidateTextLayout();
				this.owner.Invalidate();
			}

			// Token: 0x06004309 RID: 17161 RVA: 0x000F02C8 File Offset: 0x000EF2C8
			void ICollection.CopyTo(Array dest, int index)
			{
				this.owner.links.CopyTo(dest, index);
			}

			// Token: 0x0600430A RID: 17162 RVA: 0x000F02DC File Offset: 0x000EF2DC
			public IEnumerator GetEnumerator()
			{
				if (this.owner.links != null)
				{
					return this.owner.links.GetEnumerator();
				}
				return new LinkLabel.Link[0].GetEnumerator();
			}

			// Token: 0x0600430B RID: 17163 RVA: 0x000F0308 File Offset: 0x000EF308
			public void Remove(LinkLabel.Link value)
			{
				if (value.Owner != this.owner)
				{
					return;
				}
				this.owner.links.Remove(value);
				if (this.owner.AutoSize)
				{
					LayoutTransaction.DoLayout(this.owner.ParentInternal, this.owner, PropertyNames.Links);
					this.owner.AdjustSize();
					this.owner.Invalidate();
				}
				this.owner.links.Sort(LinkLabel.linkComparer);
				this.owner.ValidateNoOverlappingLinks();
				this.owner.UpdateSelectability();
				this.owner.InvalidateTextLayout();
				this.owner.Invalidate();
				if (this.owner.FocusLink == null && this.owner.links.Count > 0)
				{
					this.owner.FocusLink = (LinkLabel.Link)this.owner.links[0];
				}
			}

			// Token: 0x0600430C RID: 17164 RVA: 0x000F03F5 File Offset: 0x000EF3F5
			public void RemoveAt(int index)
			{
				this.Remove(this[index]);
			}

			// Token: 0x0600430D RID: 17165 RVA: 0x000F0404 File Offset: 0x000EF404
			public virtual void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			// Token: 0x0600430E RID: 17166 RVA: 0x000F0429 File Offset: 0x000EF429
			void IList.Remove(object value)
			{
				if (value is LinkLabel.Link)
				{
					this.Remove((LinkLabel.Link)value);
				}
			}

			// Token: 0x040020BB RID: 8379
			private LinkLabel owner;

			// Token: 0x040020BC RID: 8380
			private bool linksAdded;

			// Token: 0x040020BD RID: 8381
			private int lastAccessedIndex = -1;
		}

		// Token: 0x02000471 RID: 1137
		[TypeConverter(typeof(LinkConverter))]
		public class Link
		{
			// Token: 0x0600430F RID: 17167 RVA: 0x000F043F File Offset: 0x000EF43F
			public Link()
			{
			}

			// Token: 0x06004310 RID: 17168 RVA: 0x000F044E File Offset: 0x000EF44E
			public Link(int start, int length)
			{
				this.start = start;
				this.length = length;
			}

			// Token: 0x06004311 RID: 17169 RVA: 0x000F046B File Offset: 0x000EF46B
			public Link(int start, int length, object linkData)
			{
				this.start = start;
				this.length = length;
				this.linkData = linkData;
			}

			// Token: 0x06004312 RID: 17170 RVA: 0x000F048F File Offset: 0x000EF48F
			internal Link(LinkLabel owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000D1C RID: 3356
			// (get) Token: 0x06004313 RID: 17171 RVA: 0x000F04A5 File Offset: 0x000EF4A5
			// (set) Token: 0x06004314 RID: 17172 RVA: 0x000F04AD File Offset: 0x000EF4AD
			public string Description
			{
				get
				{
					return this.description;
				}
				set
				{
					this.description = value;
				}
			}

			// Token: 0x17000D1D RID: 3357
			// (get) Token: 0x06004315 RID: 17173 RVA: 0x000F04B6 File Offset: 0x000EF4B6
			// (set) Token: 0x06004316 RID: 17174 RVA: 0x000F04C0 File Offset: 0x000EF4C0
			[DefaultValue(true)]
			public bool Enabled
			{
				get
				{
					return this.enabled;
				}
				set
				{
					if (this.enabled != value)
					{
						this.enabled = value;
						if ((this.state & (LinkState)3) != LinkState.Normal)
						{
							this.state &= (LinkState)(-4);
							if (this.owner != null)
							{
								this.owner.OverrideCursor = null;
							}
						}
						if (this.owner != null)
						{
							this.owner.InvalidateLink(this);
						}
					}
				}
			}

			// Token: 0x17000D1E RID: 3358
			// (get) Token: 0x06004317 RID: 17175 RVA: 0x000F0520 File Offset: 0x000EF520
			// (set) Token: 0x06004318 RID: 17176 RVA: 0x000F0577 File Offset: 0x000EF577
			public int Length
			{
				get
				{
					if (this.length != -1)
					{
						return this.length;
					}
					if (this.owner != null && !string.IsNullOrEmpty(this.owner.Text))
					{
						StringInfo stringInfo = new StringInfo(this.owner.Text);
						return stringInfo.LengthInTextElements - this.Start;
					}
					return 0;
				}
				set
				{
					if (this.length != value)
					{
						this.length = value;
						if (this.owner != null)
						{
							this.owner.InvalidateTextLayout();
							this.owner.Invalidate();
						}
					}
				}
			}

			// Token: 0x17000D1F RID: 3359
			// (get) Token: 0x06004319 RID: 17177 RVA: 0x000F05A7 File Offset: 0x000EF5A7
			// (set) Token: 0x0600431A RID: 17178 RVA: 0x000F05AF File Offset: 0x000EF5AF
			[DefaultValue(null)]
			public object LinkData
			{
				get
				{
					return this.linkData;
				}
				set
				{
					this.linkData = value;
				}
			}

			// Token: 0x17000D20 RID: 3360
			// (get) Token: 0x0600431B RID: 17179 RVA: 0x000F05B8 File Offset: 0x000EF5B8
			// (set) Token: 0x0600431C RID: 17180 RVA: 0x000F05C0 File Offset: 0x000EF5C0
			internal LinkLabel Owner
			{
				get
				{
					return this.owner;
				}
				set
				{
					this.owner = value;
				}
			}

			// Token: 0x17000D21 RID: 3361
			// (get) Token: 0x0600431D RID: 17181 RVA: 0x000F05C9 File Offset: 0x000EF5C9
			// (set) Token: 0x0600431E RID: 17182 RVA: 0x000F05D1 File Offset: 0x000EF5D1
			internal LinkState State
			{
				get
				{
					return this.state;
				}
				set
				{
					this.state = value;
				}
			}

			// Token: 0x17000D22 RID: 3362
			// (get) Token: 0x0600431F RID: 17183 RVA: 0x000F05DA File Offset: 0x000EF5DA
			// (set) Token: 0x06004320 RID: 17184 RVA: 0x000F05F0 File Offset: 0x000EF5F0
			[SRDescription("TreeNodeNodeNameDescr")]
			[SRCategory("CatAppearance")]
			[DefaultValue("")]
			public string Name
			{
				get
				{
					if (this.name != null)
					{
						return this.name;
					}
					return "";
				}
				set
				{
					this.name = value;
				}
			}

			// Token: 0x17000D23 RID: 3363
			// (get) Token: 0x06004321 RID: 17185 RVA: 0x000F05F9 File Offset: 0x000EF5F9
			// (set) Token: 0x06004322 RID: 17186 RVA: 0x000F0604 File Offset: 0x000EF604
			public int Start
			{
				get
				{
					return this.start;
				}
				set
				{
					if (this.start != value)
					{
						this.start = value;
						if (this.owner != null)
						{
							this.owner.links.Sort(LinkLabel.linkComparer);
							this.owner.InvalidateTextLayout();
							this.owner.Invalidate();
						}
					}
				}
			}

			// Token: 0x17000D24 RID: 3364
			// (get) Token: 0x06004323 RID: 17187 RVA: 0x000F0654 File Offset: 0x000EF654
			// (set) Token: 0x06004324 RID: 17188 RVA: 0x000F065C File Offset: 0x000EF65C
			[SRCategory("CatData")]
			[Localizable(false)]
			[TypeConverter(typeof(StringConverter))]
			[Bindable(true)]
			[SRDescription("ControlTagDescr")]
			[DefaultValue(null)]
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

			// Token: 0x17000D25 RID: 3365
			// (get) Token: 0x06004325 RID: 17189 RVA: 0x000F0665 File Offset: 0x000EF665
			// (set) Token: 0x06004326 RID: 17190 RVA: 0x000F0674 File Offset: 0x000EF674
			[DefaultValue(false)]
			public bool Visited
			{
				get
				{
					return (this.State & LinkState.Visited) == LinkState.Visited;
				}
				set
				{
					bool visited = this.Visited;
					if (value)
					{
						this.State |= LinkState.Visited;
					}
					else
					{
						this.State &= (LinkState)(-5);
					}
					if (visited != this.Visited && this.owner != null)
					{
						this.owner.InvalidateLink(this);
					}
				}
			}

			// Token: 0x17000D26 RID: 3366
			// (get) Token: 0x06004327 RID: 17191 RVA: 0x000F06C7 File Offset: 0x000EF6C7
			// (set) Token: 0x06004328 RID: 17192 RVA: 0x000F06CF File Offset: 0x000EF6CF
			internal Region VisualRegion
			{
				get
				{
					return this.visualRegion;
				}
				set
				{
					this.visualRegion = value;
				}
			}

			// Token: 0x040020BE RID: 8382
			private int start;

			// Token: 0x040020BF RID: 8383
			private object linkData;

			// Token: 0x040020C0 RID: 8384
			private LinkState state;

			// Token: 0x040020C1 RID: 8385
			private bool enabled = true;

			// Token: 0x040020C2 RID: 8386
			private Region visualRegion;

			// Token: 0x040020C3 RID: 8387
			internal int length;

			// Token: 0x040020C4 RID: 8388
			private LinkLabel owner;

			// Token: 0x040020C5 RID: 8389
			private string name;

			// Token: 0x040020C6 RID: 8390
			private string description;

			// Token: 0x040020C7 RID: 8391
			private object userData;
		}

		// Token: 0x02000472 RID: 1138
		private class LinkComparer : IComparer
		{
			// Token: 0x06004329 RID: 17193 RVA: 0x000F06D8 File Offset: 0x000EF6D8
			int IComparer.Compare(object link1, object link2)
			{
				int start = ((LinkLabel.Link)link1).Start;
				int start2 = ((LinkLabel.Link)link2).Start;
				return start - start2;
			}
		}

		// Token: 0x02000473 RID: 1139
		[ComVisible(true)]
		internal class LinkLabelAccessibleObject : Label.LabelAccessibleObject
		{
			// Token: 0x0600432B RID: 17195 RVA: 0x000F0708 File Offset: 0x000EF708
			public LinkLabelAccessibleObject(LinkLabel owner) : base(owner)
			{
			}

			// Token: 0x0600432C RID: 17196 RVA: 0x000F0711 File Offset: 0x000EF711
			public override AccessibleObject GetChild(int index)
			{
				if (index >= 0 && index < ((LinkLabel)base.Owner).Links.Count)
				{
					return new LinkLabel.LinkAccessibleObject(((LinkLabel)base.Owner).Links[index]);
				}
				return null;
			}

			// Token: 0x0600432D RID: 17197 RVA: 0x000F074C File Offset: 0x000EF74C
			public override AccessibleObject HitTest(int x, int y)
			{
				Point point = base.Owner.PointToClient(new Point(x, y));
				LinkLabel.Link link = ((LinkLabel)base.Owner).PointInLink(point.X, point.Y);
				if (link != null)
				{
					return new LinkLabel.LinkAccessibleObject(link);
				}
				if (this.Bounds.Contains(x, y))
				{
					return this;
				}
				return null;
			}

			// Token: 0x0600432E RID: 17198 RVA: 0x000F07AA File Offset: 0x000EF7AA
			public override int GetChildCount()
			{
				return ((LinkLabel)base.Owner).Links.Count;
			}
		}

		// Token: 0x02000474 RID: 1140
		[ComVisible(true)]
		internal class LinkAccessibleObject : AccessibleObject
		{
			// Token: 0x0600432F RID: 17199 RVA: 0x000F07C1 File Offset: 0x000EF7C1
			public LinkAccessibleObject(LinkLabel.Link link)
			{
				this.link = link;
			}

			// Token: 0x17000D27 RID: 3367
			// (get) Token: 0x06004330 RID: 17200 RVA: 0x000F07D0 File Offset: 0x000EF7D0
			public override Rectangle Bounds
			{
				get
				{
					Region visualRegion = this.link.VisualRegion;
					Graphics graphics = null;
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						graphics = Graphics.FromHwnd(this.link.Owner.Handle);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (visualRegion == null)
					{
						this.link.Owner.EnsureRun(graphics);
						visualRegion = this.link.VisualRegion;
						if (visualRegion == null)
						{
							graphics.Dispose();
							return Rectangle.Empty;
						}
					}
					Rectangle r;
					try
					{
						r = Rectangle.Ceiling(visualRegion.GetBounds(graphics));
					}
					finally
					{
						graphics.Dispose();
					}
					return this.link.Owner.RectangleToScreen(r);
				}
			}

			// Token: 0x17000D28 RID: 3368
			// (get) Token: 0x06004331 RID: 17201 RVA: 0x000F0888 File Offset: 0x000EF888
			public override string DefaultAction
			{
				get
				{
					return SR.GetString("AccessibleActionClick");
				}
			}

			// Token: 0x17000D29 RID: 3369
			// (get) Token: 0x06004332 RID: 17202 RVA: 0x000F0894 File Offset: 0x000EF894
			public override string Description
			{
				get
				{
					return this.link.Description;
				}
			}

			// Token: 0x17000D2A RID: 3370
			// (get) Token: 0x06004333 RID: 17203 RVA: 0x000F08A4 File Offset: 0x000EF8A4
			// (set) Token: 0x06004334 RID: 17204 RVA: 0x000F08FC File Offset: 0x000EF8FC
			public override string Name
			{
				get
				{
					string text = this.link.Owner.Text;
					int num = LinkLabel.ConvertToCharIndex(this.link.Start, text);
					int num2 = LinkLabel.ConvertToCharIndex(this.link.Start + this.link.Length, text);
					return text.Substring(num, num2 - num);
				}
				set
				{
					base.Name = value;
				}
			}

			// Token: 0x17000D2B RID: 3371
			// (get) Token: 0x06004335 RID: 17205 RVA: 0x000F0905 File Offset: 0x000EF905
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.link.Owner.AccessibilityObject;
				}
			}

			// Token: 0x17000D2C RID: 3372
			// (get) Token: 0x06004336 RID: 17206 RVA: 0x000F0917 File Offset: 0x000EF917
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.Link;
				}
			}

			// Token: 0x17000D2D RID: 3373
			// (get) Token: 0x06004337 RID: 17207 RVA: 0x000F091C File Offset: 0x000EF91C
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.Focusable;
					if (this.link.Owner.FocusLink == this.link)
					{
						accessibleStates |= AccessibleStates.Focused;
					}
					return accessibleStates;
				}
			}

			// Token: 0x17000D2E RID: 3374
			// (get) Token: 0x06004338 RID: 17208 RVA: 0x000F094C File Offset: 0x000EF94C
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.Name;
				}
			}

			// Token: 0x06004339 RID: 17209 RVA: 0x000F0954 File Offset: 0x000EF954
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				this.link.Owner.OnLinkClicked(new LinkLabelLinkClickedEventArgs(this.link));
			}

			// Token: 0x040020C8 RID: 8392
			private LinkLabel.Link link;
		}
	}
}

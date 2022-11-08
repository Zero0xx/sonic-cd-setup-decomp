using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x02000669 RID: 1641
	[ToolboxItem(false)]
	[DefaultProperty("Text")]
	[Designer("System.Windows.Forms.Design.ToolBarButtonDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DesignTimeVisible(false)]
	public class ToolBarButton : Component
	{
		// Token: 0x0600565A RID: 22106 RVA: 0x0013A3C7 File Offset: 0x001393C7
		public ToolBarButton()
		{
		}

		// Token: 0x0600565B RID: 22107 RVA: 0x0013A3F7 File Offset: 0x001393F7
		public ToolBarButton(string text)
		{
			this.Text = text;
		}

		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x0600565C RID: 22108 RVA: 0x0013A42E File Offset: 0x0013942E
		internal ToolBarButton.ToolBarButtonImageIndexer ImageIndexer
		{
			get
			{
				if (this.imageIndexer == null)
				{
					this.imageIndexer = new ToolBarButton.ToolBarButtonImageIndexer(this);
				}
				return this.imageIndexer;
			}
		}

		// Token: 0x170011ED RID: 4589
		// (get) Token: 0x0600565D RID: 22109 RVA: 0x0013A44A File Offset: 0x0013944A
		// (set) Token: 0x0600565E RID: 22110 RVA: 0x0013A452 File Offset: 0x00139452
		[DefaultValue(null)]
		[SRDescription("ToolBarButtonMenuDescr")]
		[TypeConverter(typeof(ReferenceConverter))]
		public Menu DropDownMenu
		{
			get
			{
				return this.dropDownMenu;
			}
			set
			{
				if (value != null && !(value is ContextMenu))
				{
					throw new ArgumentException(SR.GetString("ToolBarButtonInvalidDropDownMenuType"));
				}
				this.dropDownMenu = value;
			}
		}

		// Token: 0x170011EE RID: 4590
		// (get) Token: 0x0600565F RID: 22111 RVA: 0x0013A476 File Offset: 0x00139476
		// (set) Token: 0x06005660 RID: 22112 RVA: 0x0013A480 File Offset: 0x00139480
		[DefaultValue(true)]
		[Localizable(true)]
		[SRDescription("ToolBarButtonEnabledDescr")]
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
					if (this.parent != null && this.parent.IsHandleCreated)
					{
						this.parent.SendMessage(1025, this.FindButtonIndex(), this.enabled ? 1 : 0);
					}
				}
			}
		}

		// Token: 0x170011EF RID: 4591
		// (get) Token: 0x06005661 RID: 22113 RVA: 0x0013A4D5 File Offset: 0x001394D5
		// (set) Token: 0x06005662 RID: 22114 RVA: 0x0013A4E4 File Offset: 0x001394E4
		[SRDescription("ToolBarButtonImageIndexDescr")]
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue(-1)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Localizable(true)]
		public int ImageIndex
		{
			get
			{
				return this.ImageIndexer.Index;
			}
			set
			{
				if (this.ImageIndexer.Index != value)
				{
					if (value < -1)
					{
						throw new ArgumentOutOfRangeException("ImageIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"ImageIndex",
							value.ToString(CultureInfo.CurrentCulture),
							-1
						}));
					}
					this.ImageIndexer.Index = value;
					this.UpdateButton(false);
				}
			}
		}

		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x06005663 RID: 22115 RVA: 0x0013A553 File Offset: 0x00139553
		// (set) Token: 0x06005664 RID: 22116 RVA: 0x0013A560 File Offset: 0x00139560
		[TypeConverter(typeof(ImageKeyConverter))]
		[Localizable(true)]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ToolBarButtonImageIndexDescr")]
		[DefaultValue("")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public string ImageKey
		{
			get
			{
				return this.ImageIndexer.Key;
			}
			set
			{
				if (this.ImageIndexer.Key != value)
				{
					this.ImageIndexer.Key = value;
					this.UpdateButton(false);
				}
			}
		}

		// Token: 0x170011F1 RID: 4593
		// (get) Token: 0x06005665 RID: 22117 RVA: 0x0013A588 File Offset: 0x00139588
		// (set) Token: 0x06005666 RID: 22118 RVA: 0x0013A596 File Offset: 0x00139596
		[Browsable(false)]
		public string Name
		{
			get
			{
				return WindowsFormsUtils.GetComponentName(this, this.name);
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					this.name = null;
				}
				else
				{
					this.name = value;
				}
				if (this.Site != null)
				{
					this.Site.Name = this.name;
				}
			}
		}

		// Token: 0x170011F2 RID: 4594
		// (get) Token: 0x06005667 RID: 22119 RVA: 0x0013A5CC File Offset: 0x001395CC
		[Browsable(false)]
		public ToolBar Parent
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170011F3 RID: 4595
		// (get) Token: 0x06005668 RID: 22120 RVA: 0x0013A5D4 File Offset: 0x001395D4
		// (set) Token: 0x06005669 RID: 22121 RVA: 0x0013A631 File Offset: 0x00139631
		[SRDescription("ToolBarButtonPartialPushDescr")]
		[DefaultValue(false)]
		public bool PartialPush
		{
			get
			{
				if (this.parent == null || !this.parent.IsHandleCreated)
				{
					return this.partialPush;
				}
				if ((int)this.parent.SendMessage(1037, this.FindButtonIndex(), 0) != 0)
				{
					this.partialPush = true;
				}
				else
				{
					this.partialPush = false;
				}
				return this.partialPush;
			}
			set
			{
				if (this.partialPush != value)
				{
					this.partialPush = value;
					this.UpdateButton(false);
				}
			}
		}

		// Token: 0x170011F4 RID: 4596
		// (get) Token: 0x0600566A RID: 22122 RVA: 0x0013A64A File Offset: 0x0013964A
		// (set) Token: 0x0600566B RID: 22123 RVA: 0x0013A66E File Offset: 0x0013966E
		[SRDescription("ToolBarButtonPushedDescr")]
		[DefaultValue(false)]
		public bool Pushed
		{
			get
			{
				if (this.parent == null || !this.parent.IsHandleCreated)
				{
					return this.pushed;
				}
				return this.GetPushedState();
			}
			set
			{
				if (value != this.Pushed)
				{
					this.pushed = value;
					this.UpdateButton(false, false, false);
				}
			}
		}

		// Token: 0x170011F5 RID: 4597
		// (get) Token: 0x0600566C RID: 22124 RVA: 0x0013A68C File Offset: 0x0013968C
		public Rectangle Rectangle
		{
			get
			{
				if (this.parent != null)
				{
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					UnsafeNativeMethods.SendMessage(new HandleRef(this.parent, this.parent.Handle), 1075, this.FindButtonIndex(), ref rect);
					return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
				}
				return Rectangle.Empty;
			}
		}

		// Token: 0x170011F6 RID: 4598
		// (get) Token: 0x0600566D RID: 22125 RVA: 0x0013A6F9 File Offset: 0x001396F9
		// (set) Token: 0x0600566E RID: 22126 RVA: 0x0013A701 File Offset: 0x00139701
		[DefaultValue(ToolBarButtonStyle.PushButton)]
		[SRDescription("ToolBarButtonStyleDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public ToolBarButtonStyle Style
		{
			get
			{
				return this.style;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 1, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolBarButtonStyle));
				}
				if (this.style == value)
				{
					return;
				}
				this.style = value;
				this.UpdateButton(true);
			}
		}

		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x0600566F RID: 22127 RVA: 0x0013A741 File Offset: 0x00139741
		// (set) Token: 0x06005670 RID: 22128 RVA: 0x0013A749 File Offset: 0x00139749
		[Bindable(true)]
		[SRCategory("CatData")]
		[TypeConverter(typeof(StringConverter))]
		[Localizable(false)]
		[DefaultValue(null)]
		[SRDescription("ControlTagDescr")]
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

		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x06005671 RID: 22129 RVA: 0x0013A752 File Offset: 0x00139752
		// (set) Token: 0x06005672 RID: 22130 RVA: 0x0013A768 File Offset: 0x00139768
		[SRDescription("ToolBarButtonTextDescr")]
		[Localizable(true)]
		[DefaultValue("")]
		public string Text
		{
			get
			{
				if (this.text != null)
				{
					return this.text;
				}
				return "";
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					value = null;
				}
				if ((value == null && this.text != null) || (value != null && (this.text == null || !this.text.Equals(value))))
				{
					this.text = value;
					this.UpdateButton(WindowsFormsUtils.ContainsMnemonic(this.text), true, true);
				}
			}
		}

		// Token: 0x170011F9 RID: 4601
		// (get) Token: 0x06005673 RID: 22131 RVA: 0x0013A7BE File Offset: 0x001397BE
		// (set) Token: 0x06005674 RID: 22132 RVA: 0x0013A7D4 File Offset: 0x001397D4
		[SRDescription("ToolBarButtonToolTipTextDescr")]
		[Localizable(true)]
		[DefaultValue("")]
		public string ToolTipText
		{
			get
			{
				if (this.tooltipText != null)
				{
					return this.tooltipText;
				}
				return "";
			}
			set
			{
				this.tooltipText = value;
			}
		}

		// Token: 0x170011FA RID: 4602
		// (get) Token: 0x06005675 RID: 22133 RVA: 0x0013A7DD File Offset: 0x001397DD
		// (set) Token: 0x06005676 RID: 22134 RVA: 0x0013A7E5 File Offset: 0x001397E5
		[Localizable(true)]
		[SRDescription("ToolBarButtonVisibleDescr")]
		[DefaultValue(true)]
		public bool Visible
		{
			get
			{
				return this.visible;
			}
			set
			{
				if (this.visible != value)
				{
					this.visible = value;
					this.UpdateButton(false);
				}
			}
		}

		// Token: 0x170011FB RID: 4603
		// (get) Token: 0x06005677 RID: 22135 RVA: 0x0013A800 File Offset: 0x00139800
		internal short Width
		{
			get
			{
				int num = 0;
				ToolBarButtonStyle toolBarButtonStyle = this.Style;
				Size border3DSize = SystemInformation.Border3DSize;
				if (toolBarButtonStyle != ToolBarButtonStyle.Separator)
				{
					using (Graphics graphics = this.parent.CreateGraphicsInternal())
					{
						Size buttonSize = this.parent.buttonSize;
						if (!buttonSize.IsEmpty)
						{
							num = buttonSize.Width;
						}
						else if (this.parent.ImageList != null || !string.IsNullOrEmpty(this.Text))
						{
							Size imageSize = this.parent.ImageSize;
							Size size = Size.Ceiling(graphics.MeasureString(this.Text, this.parent.Font));
							if (this.parent.TextAlign == ToolBarTextAlign.Right)
							{
								if (size.Width == 0)
								{
									num = imageSize.Width + border3DSize.Width * 4;
								}
								else
								{
									num = imageSize.Width + size.Width + border3DSize.Width * 6;
								}
							}
							else if (imageSize.Width > size.Width)
							{
								num = imageSize.Width + border3DSize.Width * 4;
							}
							else
							{
								num = size.Width + border3DSize.Width * 4;
							}
							if (toolBarButtonStyle == ToolBarButtonStyle.DropDownButton && this.parent.DropDownArrows)
							{
								num += 15;
							}
						}
						else
						{
							num = this.parent.ButtonSize.Width;
						}
						goto IL_14D;
					}
				}
				num = border3DSize.Width * 2;
				IL_14D:
				return (short)num;
			}
		}

		// Token: 0x06005678 RID: 22136 RVA: 0x0013A978 File Offset: 0x00139978
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.parent != null)
			{
				int num = this.FindButtonIndex();
				if (num != -1)
				{
					this.parent.Buttons.RemoveAt(num);
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06005679 RID: 22137 RVA: 0x0013A9B4 File Offset: 0x001399B4
		private int FindButtonIndex()
		{
			for (int i = 0; i < this.parent.Buttons.Count; i++)
			{
				if (this.parent.Buttons[i] == this)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600567A RID: 22138 RVA: 0x0013A9F4 File Offset: 0x001399F4
		internal int GetButtonWidth()
		{
			int result = this.Parent.ButtonSize.Width;
			NativeMethods.TBBUTTONINFO tbbuttoninfo = default(NativeMethods.TBBUTTONINFO);
			tbbuttoninfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.TBBUTTONINFO));
			tbbuttoninfo.dwMask = 64;
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this.Parent, this.Parent.Handle), NativeMethods.TB_GETBUTTONINFO, this.commandId, ref tbbuttoninfo);
			if (num != -1)
			{
				result = (int)tbbuttoninfo.cx;
			}
			return result;
		}

		// Token: 0x0600567B RID: 22139 RVA: 0x0013AA77 File Offset: 0x00139A77
		private bool GetPushedState()
		{
			if ((int)this.parent.SendMessage(1034, this.FindButtonIndex(), 0) != 0)
			{
				this.pushed = true;
			}
			else
			{
				this.pushed = false;
			}
			return this.pushed;
		}

		// Token: 0x0600567C RID: 22140 RVA: 0x0013AAB0 File Offset: 0x00139AB0
		internal NativeMethods.TBBUTTON GetTBBUTTON(int commandId)
		{
			NativeMethods.TBBUTTON result = default(NativeMethods.TBBUTTON);
			result.iBitmap = this.ImageIndexer.ActualIndex;
			result.fsState = 0;
			if (this.enabled)
			{
				result.fsState |= 4;
			}
			if (this.partialPush && this.style == ToolBarButtonStyle.ToggleButton)
			{
				result.fsState |= 16;
			}
			if (this.pushed)
			{
				result.fsState |= 1;
			}
			if (!this.visible)
			{
				result.fsState |= 8;
			}
			switch (this.style)
			{
			case ToolBarButtonStyle.PushButton:
				result.fsStyle = 0;
				break;
			case ToolBarButtonStyle.ToggleButton:
				result.fsStyle = 2;
				break;
			case ToolBarButtonStyle.Separator:
				result.fsStyle = 1;
				break;
			case ToolBarButtonStyle.DropDownButton:
				result.fsStyle = 8;
				break;
			}
			result.dwData = (IntPtr)0;
			result.iString = this.stringIndex;
			this.commandId = commandId;
			result.idCommand = commandId;
			return result;
		}

		// Token: 0x0600567D RID: 22141 RVA: 0x0013ABBC File Offset: 0x00139BBC
		internal NativeMethods.TBBUTTONINFO GetTBBUTTONINFO(bool updateText, int newCommandId)
		{
			NativeMethods.TBBUTTONINFO result = default(NativeMethods.TBBUTTONINFO);
			result.cbSize = Marshal.SizeOf(typeof(NativeMethods.TBBUTTONINFO));
			result.dwMask = 13;
			if (updateText)
			{
				result.dwMask |= 2;
			}
			result.iImage = this.ImageIndexer.ActualIndex;
			if (newCommandId != this.commandId)
			{
				this.commandId = newCommandId;
				result.idCommand = newCommandId;
				result.dwMask |= 32;
			}
			result.fsState = 0;
			if (this.enabled)
			{
				result.fsState |= 4;
			}
			if (this.partialPush && this.style == ToolBarButtonStyle.ToggleButton)
			{
				result.fsState |= 16;
			}
			if (this.pushed)
			{
				result.fsState |= 1;
			}
			if (!this.visible)
			{
				result.fsState |= 8;
			}
			switch (this.style)
			{
			case ToolBarButtonStyle.PushButton:
				result.fsStyle = 0;
				break;
			case ToolBarButtonStyle.ToggleButton:
				result.fsStyle = 2;
				break;
			case ToolBarButtonStyle.Separator:
				result.fsStyle = 1;
				break;
			}
			if (this.text == null)
			{
				result.pszText = Marshal.StringToHGlobalAuto("\0\0");
			}
			else
			{
				string s = this.text;
				this.PrefixAmpersands(ref s);
				result.pszText = Marshal.StringToHGlobalAuto(s);
			}
			return result;
		}

		// Token: 0x0600567E RID: 22142 RVA: 0x0013AD20 File Offset: 0x00139D20
		private void PrefixAmpersands(ref string value)
		{
			if (value == null || value.Length == 0)
			{
				return;
			}
			if (value.IndexOf('&') < 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] == '&')
				{
					if (i < value.Length - 1 && value[i + 1] == '&')
					{
						i++;
					}
					stringBuilder.Append("&&");
				}
				else
				{
					stringBuilder.Append(value[i]);
				}
			}
			value = stringBuilder.ToString();
		}

		// Token: 0x0600567F RID: 22143 RVA: 0x0013ADAF File Offset: 0x00139DAF
		public override string ToString()
		{
			return "ToolBarButton: " + this.Text + ", Style: " + this.Style.ToString("G");
		}

		// Token: 0x06005680 RID: 22144 RVA: 0x0013ADDB File Offset: 0x00139DDB
		internal void UpdateButton(bool recreate)
		{
			this.UpdateButton(recreate, false, true);
		}

		// Token: 0x06005681 RID: 22145 RVA: 0x0013ADE8 File Offset: 0x00139DE8
		private void UpdateButton(bool recreate, bool updateText, bool updatePushedState)
		{
			if (this.style == ToolBarButtonStyle.DropDownButton && this.parent != null && this.parent.DropDownArrows)
			{
				recreate = true;
			}
			if (updatePushedState && this.parent != null && this.parent.IsHandleCreated)
			{
				this.GetPushedState();
			}
			if (this.parent != null)
			{
				int num = this.FindButtonIndex();
				if (num != -1)
				{
					this.parent.InternalSetButton(num, this, recreate, updateText);
				}
			}
		}

		// Token: 0x04003754 RID: 14164
		private string text;

		// Token: 0x04003755 RID: 14165
		private string name;

		// Token: 0x04003756 RID: 14166
		private string tooltipText;

		// Token: 0x04003757 RID: 14167
		private bool enabled = true;

		// Token: 0x04003758 RID: 14168
		private bool visible = true;

		// Token: 0x04003759 RID: 14169
		private bool pushed;

		// Token: 0x0400375A RID: 14170
		private bool partialPush;

		// Token: 0x0400375B RID: 14171
		private int commandId = -1;

		// Token: 0x0400375C RID: 14172
		private ToolBarButton.ToolBarButtonImageIndexer imageIndexer;

		// Token: 0x0400375D RID: 14173
		private ToolBarButtonStyle style = ToolBarButtonStyle.PushButton;

		// Token: 0x0400375E RID: 14174
		private object userData;

		// Token: 0x0400375F RID: 14175
		internal IntPtr stringIndex = (IntPtr)(-1);

		// Token: 0x04003760 RID: 14176
		internal ToolBar parent;

		// Token: 0x04003761 RID: 14177
		internal Menu dropDownMenu;

		// Token: 0x0200066A RID: 1642
		internal class ToolBarButtonImageIndexer : ImageList.Indexer
		{
			// Token: 0x06005682 RID: 22146 RVA: 0x0013AE57 File Offset: 0x00139E57
			public ToolBarButtonImageIndexer(ToolBarButton button)
			{
				this.owner = button;
			}

			// Token: 0x170011FC RID: 4604
			// (get) Token: 0x06005683 RID: 22147 RVA: 0x0013AE66 File Offset: 0x00139E66
			// (set) Token: 0x06005684 RID: 22148 RVA: 0x0013AE8F File Offset: 0x00139E8F
			public override ImageList ImageList
			{
				get
				{
					if (this.owner != null && this.owner.parent != null)
					{
						return this.owner.parent.ImageList;
					}
					return null;
				}
				set
				{
				}
			}

			// Token: 0x04003762 RID: 14178
			private ToolBarButton owner;
		}
	}
}

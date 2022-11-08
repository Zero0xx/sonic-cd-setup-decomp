using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.Layout;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x020005E8 RID: 1512
	[Docking(DockingBehavior.Ask)]
	[SRDescription("DescriptionRichTextBox")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.RichTextBoxDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ComVisible(true)]
	public class RichTextBox : TextBoxBase
	{
		// Token: 0x17000FFB RID: 4091
		// (get) Token: 0x06004EC4 RID: 20164 RVA: 0x00122082 File Offset: 0x00121082
		private static TraceSwitch RichTextDbg
		{
			get
			{
				if (RichTextBox.richTextDbg == null)
				{
					RichTextBox.richTextDbg = new TraceSwitch("RichTextDbg", "Debug info about RichTextBox");
				}
				return RichTextBox.richTextDbg;
			}
		}

		// Token: 0x06004EC5 RID: 20165 RVA: 0x001220A4 File Offset: 0x001210A4
		public RichTextBox()
		{
			this.InConstructor = true;
			this.richTextBoxFlags[RichTextBox.autoWordSelectionSection] = 0;
			this.DetectUrls = true;
			this.ScrollBars = RichTextBoxScrollBars.Both;
			this.RichTextShortcutsEnabled = true;
			this.MaxLength = int.MaxValue;
			this.Multiline = true;
			this.AutoSize = false;
			this.curSelStart = (this.curSelEnd = (int)(this.curSelType = -1));
			this.InConstructor = false;
		}

		// Token: 0x17000FFC RID: 4092
		// (get) Token: 0x06004EC6 RID: 20166 RVA: 0x0012213F File Offset: 0x0012113F
		// (set) Token: 0x06004EC7 RID: 20167 RVA: 0x00122158 File Offset: 0x00121158
		[Browsable(false)]
		public override bool AllowDrop
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.allowOleDropSection] != 0;
			}
			set
			{
				if (value)
				{
					try
					{
						IntSecurity.ClipboardRead.Demand();
					}
					catch (Exception innerException)
					{
						throw new InvalidOperationException(SR.GetString("DragDropRegFailed"), innerException);
					}
				}
				this.richTextBoxFlags[RichTextBox.allowOleDropSection] = (value ? 1 : 0);
				this.UpdateOleCallback();
			}
		}

		// Token: 0x17000FFD RID: 4093
		// (get) Token: 0x06004EC8 RID: 20168 RVA: 0x001221B4 File Offset: 0x001211B4
		// (set) Token: 0x06004EC9 RID: 20169 RVA: 0x001221CC File Offset: 0x001211CC
		internal bool AllowOleObjects
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.allowOleObjectsSection] != 0;
			}
			set
			{
				this.richTextBoxFlags[RichTextBox.allowOleObjectsSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000FFE RID: 4094
		// (get) Token: 0x06004ECA RID: 20170 RVA: 0x001221E5 File Offset: 0x001211E5
		// (set) Token: 0x06004ECB RID: 20171 RVA: 0x001221ED File Offset: 0x001211ED
		[DefaultValue(false)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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

		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x06004ECC RID: 20172 RVA: 0x001221F6 File Offset: 0x001211F6
		// (set) Token: 0x06004ECD RID: 20173 RVA: 0x0012220E File Offset: 0x0012120E
		[SRDescription("RichTextBoxAutoWordSelection")]
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		public bool AutoWordSelection
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.autoWordSelectionSection] != 0;
			}
			set
			{
				this.richTextBoxFlags[RichTextBox.autoWordSelectionSection] = (value ? 1 : 0);
				if (base.IsHandleCreated)
				{
					base.SendMessage(1101, value ? 2 : 4, 1);
				}
			}
		}

		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x06004ECE RID: 20174 RVA: 0x00122243 File Offset: 0x00121243
		// (set) Token: 0x06004ECF RID: 20175 RVA: 0x0012224B File Offset: 0x0012124B
		[Browsable(false)]
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

		// Token: 0x140002E1 RID: 737
		// (add) Token: 0x06004ED0 RID: 20176 RVA: 0x00122254 File Offset: 0x00121254
		// (remove) Token: 0x06004ED1 RID: 20177 RVA: 0x0012225D File Offset: 0x0012125D
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				base.BackgroundImageChanged += value;
			}
			remove
			{
				base.BackgroundImageChanged -= value;
			}
		}

		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x06004ED2 RID: 20178 RVA: 0x00122266 File Offset: 0x00121266
		// (set) Token: 0x06004ED3 RID: 20179 RVA: 0x0012226E File Offset: 0x0012126E
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
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

		// Token: 0x140002E2 RID: 738
		// (add) Token: 0x06004ED4 RID: 20180 RVA: 0x00122277 File Offset: 0x00121277
		// (remove) Token: 0x06004ED5 RID: 20181 RVA: 0x00122280 File Offset: 0x00121280
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x06004ED6 RID: 20182 RVA: 0x00122289 File Offset: 0x00121289
		// (set) Token: 0x06004ED7 RID: 20183 RVA: 0x00122294 File Offset: 0x00121294
		[SRCategory("CatBehavior")]
		[SRDescription("RichTextBoxBulletIndent")]
		[DefaultValue(0)]
		[Localizable(true)]
		public int BulletIndent
		{
			get
			{
				return this.bulletIndent;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("BulletIndent", SR.GetString("InvalidArgument", new object[]
					{
						"BulletIndent",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.bulletIndent = value;
				if (base.IsHandleCreated && this.SelectionBullet)
				{
					this.SelectionBullet = true;
				}
			}
		}

		// Token: 0x17001003 RID: 4099
		// (get) Token: 0x06004ED8 RID: 20184 RVA: 0x001222F7 File Offset: 0x001212F7
		// (set) Token: 0x06004ED9 RID: 20185 RVA: 0x0012230F File Offset: 0x0012130F
		private bool CallOnContentsResized
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.callOnContentsResizedSection] != 0;
			}
			set
			{
				this.richTextBoxFlags[RichTextBox.callOnContentsResizedSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17001004 RID: 4100
		// (get) Token: 0x06004EDA RID: 20186 RVA: 0x00122328 File Offset: 0x00121328
		internal override bool CanRaiseTextChangedEvent
		{
			get
			{
				return !this.SuppressTextChangedEvent;
			}
		}

		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x06004EDB RID: 20187 RVA: 0x00122334 File Offset: 0x00121334
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("RichTextBoxCanRedoDescr")]
		public bool CanRedo
		{
			get
			{
				return base.IsHandleCreated && (int)base.SendMessage(1109, 0, 0) != 0;
			}
		}

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x06004EDC RID: 20188 RVA: 0x00122368 File Offset: 0x00121368
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				if (RichTextBox.moduleHandle == IntPtr.Zero)
				{
					RichTextBox.moduleHandle = UnsafeNativeMethods.LoadLibrary("RichEd20.DLL");
					int lastWin32Error = Marshal.GetLastWin32Error();
					if ((long)RichTextBox.moduleHandle < 32L)
					{
						throw new Win32Exception(lastWin32Error, SR.GetString("LoadDLLError", new object[]
						{
							"RichEd20.DLL"
						}));
					}
					StringBuilder stringBuilder = new StringBuilder(260);
					UnsafeNativeMethods.GetModuleFileName(new HandleRef(null, RichTextBox.moduleHandle), stringBuilder, stringBuilder.Capacity);
					string text = stringBuilder.ToString();
					new FileIOPermission(FileIOPermissionAccess.Read, text).Assert();
					FileVersionInfo versionInfo;
					try
					{
						versionInfo = FileVersionInfo.GetVersionInfo(text);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					int num;
					if (versionInfo != null && !string.IsNullOrEmpty(versionInfo.ProductVersion) && int.TryParse(versionInfo.ProductVersion[0].ToString(), out num))
					{
						RichTextBox.richEditMajorVersion = num;
					}
				}
				CreateParams createParams = base.CreateParams;
				if (Marshal.SystemDefaultCharSize == 1)
				{
					createParams.ClassName = "RichEdit20A";
				}
				else
				{
					createParams.ClassName = "RichEdit20W";
				}
				if (this.Multiline)
				{
					if ((this.ScrollBars & RichTextBoxScrollBars.Horizontal) != RichTextBoxScrollBars.None && !base.WordWrap)
					{
						createParams.Style |= 1048576;
						if ((this.ScrollBars & (RichTextBoxScrollBars)16) != RichTextBoxScrollBars.None)
						{
							createParams.Style |= 8192;
						}
					}
					if ((this.ScrollBars & RichTextBoxScrollBars.Vertical) != RichTextBoxScrollBars.None)
					{
						createParams.Style |= 2097152;
						if ((this.ScrollBars & (RichTextBoxScrollBars)16) != RichTextBoxScrollBars.None)
						{
							createParams.Style |= 8192;
						}
					}
				}
				if (BorderStyle.FixedSingle == base.BorderStyle && (createParams.Style & 8388608) != 0)
				{
					createParams.Style &= -8388609;
					createParams.ExStyle |= 512;
				}
				return createParams;
			}
		}

		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x06004EDD RID: 20189 RVA: 0x0012254C File Offset: 0x0012154C
		// (set) Token: 0x06004EDE RID: 20190 RVA: 0x00122564 File Offset: 0x00121564
		[DefaultValue(true)]
		[SRDescription("RichTextBoxDetectURLs")]
		[SRCategory("CatBehavior")]
		public bool DetectUrls
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.autoUrlDetectSection] != 0;
			}
			set
			{
				if (value != this.DetectUrls)
				{
					this.richTextBoxFlags[RichTextBox.autoUrlDetectSection] = (value ? 1 : 0);
					if (base.IsHandleCreated)
					{
						base.SendMessage(1115, value ? 1 : 0, 0);
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x06004EDF RID: 20191 RVA: 0x001225B3 File Offset: 0x001215B3
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 96);
			}
		}

		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x06004EE0 RID: 20192 RVA: 0x001225BE File Offset: 0x001215BE
		// (set) Token: 0x06004EE1 RID: 20193 RVA: 0x001225D8 File Offset: 0x001215D8
		[DefaultValue(false)]
		[SRDescription("RichTextBoxEnableAutoDragDrop")]
		[SRCategory("CatBehavior")]
		public bool EnableAutoDragDrop
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.enableAutoDragDropSection] != 0;
			}
			set
			{
				if (value)
				{
					try
					{
						IntSecurity.ClipboardRead.Demand();
					}
					catch (Exception innerException)
					{
						throw new InvalidOperationException(SR.GetString("DragDropRegFailed"), innerException);
					}
				}
				this.richTextBoxFlags[RichTextBox.enableAutoDragDropSection] = (value ? 1 : 0);
				this.UpdateOleCallback();
			}
		}

		// Token: 0x1700100A RID: 4106
		// (get) Token: 0x06004EE2 RID: 20194 RVA: 0x00122634 File Offset: 0x00121634
		// (set) Token: 0x06004EE3 RID: 20195 RVA: 0x0012263C File Offset: 0x0012163C
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				if (base.IsHandleCreated)
				{
					if (this.InternalSetForeColor(value))
					{
						base.ForeColor = value;
						return;
					}
				}
				else
				{
					base.ForeColor = value;
				}
			}
		}

		// Token: 0x1700100B RID: 4107
		// (get) Token: 0x06004EE4 RID: 20196 RVA: 0x0012265E File Offset: 0x0012165E
		// (set) Token: 0x06004EE5 RID: 20197 RVA: 0x00122668 File Offset: 0x00121668
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				if (base.IsHandleCreated)
				{
					if (SafeNativeMethods.GetWindowTextLength(new HandleRef(this, base.Handle)) > 0)
					{
						if (value == null)
						{
							base.Font = null;
							this.SetCharFormatFont(false, this.Font);
							return;
						}
						try
						{
							Font charFormatFont = this.GetCharFormatFont(false);
							if (charFormatFont == null || !charFormatFont.Equals(value))
							{
								this.SetCharFormatFont(false, value);
								this.CallOnContentsResized = true;
								base.Font = this.GetCharFormatFont(false);
							}
							return;
						}
						finally
						{
							this.CallOnContentsResized = false;
						}
					}
					base.Font = value;
					return;
				}
				base.Font = value;
			}
		}

		// Token: 0x06004EE6 RID: 20198 RVA: 0x00122704 File Offset: 0x00121704
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			Size empty = Size.Empty;
			if (!base.WordWrap && this.Multiline && (this.ScrollBars & RichTextBoxScrollBars.Horizontal) != RichTextBoxScrollBars.None)
			{
				empty.Height += SystemInformation.HorizontalScrollBarHeight;
			}
			if (this.Multiline && (this.ScrollBars & RichTextBoxScrollBars.Vertical) != RichTextBoxScrollBars.None)
			{
				empty.Width += SystemInformation.VerticalScrollBarWidth;
			}
			proposedConstraints -= empty;
			Size preferredSizeCore = base.GetPreferredSizeCore(proposedConstraints);
			return preferredSizeCore + empty;
		}

		// Token: 0x1700100C RID: 4108
		// (get) Token: 0x06004EE7 RID: 20199 RVA: 0x00122781 File Offset: 0x00121781
		// (set) Token: 0x06004EE8 RID: 20200 RVA: 0x00122799 File Offset: 0x00121799
		private bool InConstructor
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.fInCtorSection] != 0;
			}
			set
			{
				this.richTextBoxFlags[RichTextBox.fInCtorSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x1700100D RID: 4109
		// (get) Token: 0x06004EE9 RID: 20201 RVA: 0x001227B4 File Offset: 0x001217B4
		// (set) Token: 0x06004EEA RID: 20202 RVA: 0x001227F1 File Offset: 0x001217F1
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public RichTextBoxLanguageOptions LanguageOption
		{
			get
			{
				RichTextBoxLanguageOptions result;
				if (base.IsHandleCreated)
				{
					result = (RichTextBoxLanguageOptions)((int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1145, 0, 0));
				}
				else
				{
					result = this.languageOption;
				}
				return result;
			}
			set
			{
				if (this.LanguageOption != value)
				{
					this.languageOption = value;
					if (base.IsHandleCreated)
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1144, 0, (int)value);
					}
				}
			}
		}

		// Token: 0x1700100E RID: 4110
		// (get) Token: 0x06004EEB RID: 20203 RVA: 0x00122824 File Offset: 0x00121824
		// (set) Token: 0x06004EEC RID: 20204 RVA: 0x0012283C File Offset: 0x0012183C
		private bool LinkCursor
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.linkcursorSection] != 0;
			}
			set
			{
				this.richTextBoxFlags[RichTextBox.linkcursorSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x1700100F RID: 4111
		// (get) Token: 0x06004EED RID: 20205 RVA: 0x00122855 File Offset: 0x00121855
		// (set) Token: 0x06004EEE RID: 20206 RVA: 0x0012285D File Offset: 0x0012185D
		[DefaultValue(2147483647)]
		public override int MaxLength
		{
			get
			{
				return base.MaxLength;
			}
			set
			{
				base.MaxLength = value;
			}
		}

		// Token: 0x17001010 RID: 4112
		// (get) Token: 0x06004EEF RID: 20207 RVA: 0x00122866 File Offset: 0x00121866
		// (set) Token: 0x06004EF0 RID: 20208 RVA: 0x0012286E File Offset: 0x0012186E
		[DefaultValue(true)]
		public override bool Multiline
		{
			get
			{
				return base.Multiline;
			}
			set
			{
				base.Multiline = value;
			}
		}

		// Token: 0x17001011 RID: 4113
		// (get) Token: 0x06004EF1 RID: 20209 RVA: 0x00122877 File Offset: 0x00121877
		// (set) Token: 0x06004EF2 RID: 20210 RVA: 0x0012288F File Offset: 0x0012188F
		private bool ProtectedError
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.protectedErrorSection] != 0;
			}
			set
			{
				this.richTextBoxFlags[RichTextBox.protectedErrorSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17001012 RID: 4114
		// (get) Token: 0x06004EF3 RID: 20211 RVA: 0x001228A8 File Offset: 0x001218A8
		[SRDescription("RichTextBoxRedoActionNameDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		public string RedoActionName
		{
			get
			{
				if (!this.CanRedo)
				{
					return "";
				}
				int actionID = (int)base.SendMessage(1111, 0, 0);
				return this.GetEditorActionName(actionID);
			}
		}

		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x06004EF4 RID: 20212 RVA: 0x001228DD File Offset: 0x001218DD
		// (set) Token: 0x06004EF5 RID: 20213 RVA: 0x00122908 File Offset: 0x00121908
		[DefaultValue(true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public bool RichTextShortcutsEnabled
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.richTextShortcutsEnabledSection] != 0;
			}
			set
			{
				if (RichTextBox.shortcutsToDisable == null)
				{
					RichTextBox.shortcutsToDisable = new int[]
					{
						131148,
						131154,
						131141,
						131146
					};
				}
				this.richTextBoxFlags[RichTextBox.richTextShortcutsEnabledSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x06004EF6 RID: 20214 RVA: 0x0012293E File Offset: 0x0012193E
		// (set) Token: 0x06004EF7 RID: 20215 RVA: 0x00122948 File Offset: 0x00121948
		[SRCategory("CatBehavior")]
		[SRDescription("RichTextBoxRightMargin")]
		[DefaultValue(0)]
		[Localizable(true)]
		public int RightMargin
		{
			get
			{
				return this.rightMargin;
			}
			set
			{
				if (this.rightMargin != value)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("RightMargin", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"RightMargin",
							value.ToString(CultureInfo.CurrentCulture),
							0.ToString(CultureInfo.CurrentCulture)
						}));
					}
					this.rightMargin = value;
					if (value == 0)
					{
						base.RecreateHandle();
						return;
					}
					if (base.IsHandleCreated)
					{
						IntPtr intPtr = UnsafeNativeMethods.CreateIC("DISPLAY", null, null, new HandleRef(null, IntPtr.Zero));
						try
						{
							base.SendMessage(1096, intPtr, (IntPtr)RichTextBox.Pixel2Twip(intPtr, value, true));
						}
						finally
						{
							if (intPtr != IntPtr.Zero)
							{
								UnsafeNativeMethods.DeleteDC(new HandleRef(null, intPtr));
							}
						}
					}
				}
			}
		}

		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x06004EF8 RID: 20216 RVA: 0x00122A24 File Offset: 0x00121A24
		// (set) Token: 0x06004EF9 RID: 20217 RVA: 0x00122A54 File Offset: 0x00121A54
		[RefreshProperties(RefreshProperties.All)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxRTF")]
		public string Rtf
		{
			get
			{
				if (base.IsHandleCreated)
				{
					return this.StreamOut(2);
				}
				if (this.textPlain != null)
				{
					this.ForceHandleCreate();
					return this.StreamOut(2);
				}
				return this.textRtf;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (value.Equals(this.Rtf))
				{
					return;
				}
				this.ForceHandleCreate();
				this.textRtf = value;
				this.StreamIn(value, 2);
				if (this.CanRaiseTextChangedEvent)
				{
					this.OnTextChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x06004EFA RID: 20218 RVA: 0x00122AA2 File Offset: 0x00121AA2
		// (set) Token: 0x06004EFB RID: 20219 RVA: 0x00122AB4 File Offset: 0x00121AB4
		[SRCategory("CatAppearance")]
		[DefaultValue(RichTextBoxScrollBars.Both)]
		[Localizable(true)]
		[SRDescription("RichTextBoxScrollBars")]
		public RichTextBoxScrollBars ScrollBars
		{
			get
			{
				return (RichTextBoxScrollBars)this.richTextBoxFlags[RichTextBox.scrollBarsSection];
			}
			set
			{
				if (!ClientUtils.IsEnumValid_NotSequential(value, (int)value, new int[]
				{
					3,
					0,
					1,
					2,
					17,
					18,
					19
				}))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(RichTextBoxScrollBars));
				}
				if (value != this.ScrollBars)
				{
					using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.ScrollBars))
					{
						this.richTextBoxFlags[RichTextBox.scrollBarsSection] = (int)value;
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x06004EFC RID: 20220 RVA: 0x00122B5C File Offset: 0x00121B5C
		// (set) Token: 0x06004EFD RID: 20221 RVA: 0x00122BD0 File Offset: 0x00121BD0
		[Browsable(false)]
		[DefaultValue(HorizontalAlignment.Left)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelAlignment")]
		public HorizontalAlignment SelectionAlignment
		{
			get
			{
				HorizontalAlignment result = HorizontalAlignment.Left;
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.rgxTabs = new int[32];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1085, 0, paraformat);
				if ((8 & paraformat.dwMask) != 0)
				{
					switch (paraformat.wAlignment)
					{
					case 1:
						result = HorizontalAlignment.Left;
						break;
					case 2:
						result = HorizontalAlignment.Right;
						break;
					case 3:
						result = HorizontalAlignment.Center;
						break;
					}
				}
				return result;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(HorizontalAlignment));
				}
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.dwMask = 8;
				switch (value)
				{
				case HorizontalAlignment.Left:
					paraformat.wAlignment = 1;
					break;
				case HorizontalAlignment.Right:
					paraformat.wAlignment = 2;
					break;
				case HorizontalAlignment.Center:
					paraformat.wAlignment = 3;
					break;
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1095, 0, paraformat);
			}
		}

		// Token: 0x17001018 RID: 4120
		// (get) Token: 0x06004EFE RID: 20222 RVA: 0x00122C60 File Offset: 0x00121C60
		// (set) Token: 0x06004EFF RID: 20223 RVA: 0x00122CC0 File Offset: 0x00121CC0
		[Browsable(false)]
		[DefaultValue(false)]
		[SRDescription("RichTextBoxSelBullet")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool SelectionBullet
		{
			get
			{
				RichTextBoxSelectionAttribute richTextBoxSelectionAttribute = RichTextBoxSelectionAttribute.None;
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.rgxTabs = new int[32];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1085, 0, paraformat);
				if ((32 & paraformat.dwMask) != 0)
				{
					if (1 == paraformat.wNumbering)
					{
						richTextBoxSelectionAttribute = RichTextBoxSelectionAttribute.All;
					}
					return richTextBoxSelectionAttribute == RichTextBoxSelectionAttribute.All;
				}
				return false;
			}
			set
			{
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.dwMask = 36;
				if (!value)
				{
					paraformat.wNumbering = 0;
					paraformat.dxOffset = 0;
				}
				else
				{
					paraformat.wNumbering = 1;
					paraformat.dxOffset = RichTextBox.Pixel2Twip(IntPtr.Zero, this.bulletIndent, true);
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1095, 0, paraformat);
			}
		}

		// Token: 0x17001019 RID: 4121
		// (get) Token: 0x06004F00 RID: 20224 RVA: 0x00122D2C File Offset: 0x00121D2C
		// (set) Token: 0x06004F01 RID: 20225 RVA: 0x00122D74 File Offset: 0x00121D74
		[DefaultValue(0)]
		[SRDescription("RichTextBoxSelCharOffset")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectionCharOffset
		{
			get
			{
				this.ForceHandleCreate();
				NativeMethods.CHARFORMATA charFormat = this.GetCharFormat(true);
				int yOffset;
				if ((charFormat.dwMask & 268435456) != 0)
				{
					yOffset = charFormat.yOffset;
				}
				else
				{
					yOffset = charFormat.yOffset;
				}
				return RichTextBox.Twip2Pixel(IntPtr.Zero, yOffset, false);
			}
			set
			{
				if (value > 2000 || value < -2000)
				{
					throw new ArgumentOutOfRangeException("SelectionCharOffset", SR.GetString("InvalidBoundArgument", new object[]
					{
						"SelectionCharOffset",
						value,
						-2000,
						2000
					}));
				}
				this.ForceHandleCreate();
				NativeMethods.CHARFORMATA charformata = new NativeMethods.CHARFORMATA();
				charformata.dwMask = 268435456;
				charformata.yOffset = RichTextBox.Pixel2Twip(IntPtr.Zero, value, false);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1092, 1, charformata);
			}
		}

		// Token: 0x1700101A RID: 4122
		// (get) Token: 0x06004F02 RID: 20226 RVA: 0x00122E1C File Offset: 0x00121E1C
		// (set) Token: 0x06004F03 RID: 20227 RVA: 0x00122E58 File Offset: 0x00121E58
		[SRDescription("RichTextBoxSelColor")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color SelectionColor
		{
			get
			{
				Color result = Color.Empty;
				this.ForceHandleCreate();
				NativeMethods.CHARFORMATA charFormat = this.GetCharFormat(true);
				if ((charFormat.dwMask & 1073741824) != 0)
				{
					result = ColorTranslator.FromOle(charFormat.crTextColor);
				}
				return result;
			}
			set
			{
				this.ForceHandleCreate();
				NativeMethods.CHARFORMATA charFormat = this.GetCharFormat(true);
				charFormat.dwMask = 1073741824;
				charFormat.dwEffects = 0;
				charFormat.crTextColor = ColorTranslator.ToWin32(value);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1092, 1, charFormat);
			}
		}

		// Token: 0x1700101B RID: 4123
		// (get) Token: 0x06004F04 RID: 20228 RVA: 0x00122EAC File Offset: 0x00121EAC
		// (set) Token: 0x06004F05 RID: 20229 RVA: 0x00122F0C File Offset: 0x00121F0C
		[SRDescription("RichTextBoxSelBackColor")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color SelectionBackColor
		{
			get
			{
				Color result = Color.Empty;
				if (base.IsHandleCreated)
				{
					NativeMethods.CHARFORMAT2A charFormat = this.GetCharFormat2(true);
					if ((charFormat.dwEffects & 67108864) != 0)
					{
						result = this.BackColor;
					}
					else if ((charFormat.dwMask & 67108864) != 0)
					{
						result = ColorTranslator.FromOle(charFormat.crBackColor);
					}
				}
				else
				{
					result = this.selectionBackColorToSetOnHandleCreated;
				}
				return result;
			}
			set
			{
				this.selectionBackColorToSetOnHandleCreated = value;
				if (base.IsHandleCreated)
				{
					NativeMethods.CHARFORMAT2A charformat2A = new NativeMethods.CHARFORMAT2A();
					if (value == Color.Empty)
					{
						charformat2A.dwEffects = 67108864;
					}
					else
					{
						charformat2A.dwMask = 67108864;
						charformat2A.crBackColor = ColorTranslator.ToWin32(value);
					}
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1092, 1, charformat2A);
				}
			}
		}

		// Token: 0x1700101C RID: 4124
		// (get) Token: 0x06004F06 RID: 20230 RVA: 0x00122F78 File Offset: 0x00121F78
		// (set) Token: 0x06004F07 RID: 20231 RVA: 0x00122F81 File Offset: 0x00121F81
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("RichTextBoxSelFont")]
		public Font SelectionFont
		{
			get
			{
				return this.GetCharFormatFont(true);
			}
			set
			{
				this.SetCharFormatFont(true, value);
			}
		}

		// Token: 0x1700101D RID: 4125
		// (get) Token: 0x06004F08 RID: 20232 RVA: 0x00122F8C File Offset: 0x00121F8C
		// (set) Token: 0x06004F09 RID: 20233 RVA: 0x00122FEC File Offset: 0x00121FEC
		[SRDescription("RichTextBoxSelHangingIndent")]
		[DefaultValue(0)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectionHangingIndent
		{
			get
			{
				int v = 0;
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.rgxTabs = new int[32];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1085, 0, paraformat);
				if ((4 & paraformat.dwMask) != 0)
				{
					v = paraformat.dxOffset;
				}
				return RichTextBox.Twip2Pixel(IntPtr.Zero, v, true);
			}
			set
			{
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.dwMask = 4;
				paraformat.dxOffset = RichTextBox.Pixel2Twip(IntPtr.Zero, value, true);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1095, 0, paraformat);
			}
		}

		// Token: 0x1700101E RID: 4126
		// (get) Token: 0x06004F0A RID: 20234 RVA: 0x00123038 File Offset: 0x00122038
		// (set) Token: 0x06004F0B RID: 20235 RVA: 0x00123098 File Offset: 0x00122098
		[DefaultValue(0)]
		[SRDescription("RichTextBoxSelIndent")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectionIndent
		{
			get
			{
				int v = 0;
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.rgxTabs = new int[32];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1085, 0, paraformat);
				if ((1 & paraformat.dwMask) != 0)
				{
					v = paraformat.dxStartIndent;
				}
				return RichTextBox.Twip2Pixel(IntPtr.Zero, v, true);
			}
			set
			{
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.dwMask = 1;
				paraformat.dxStartIndent = RichTextBox.Pixel2Twip(IntPtr.Zero, value, true);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1095, 0, paraformat);
			}
		}

		// Token: 0x1700101F RID: 4127
		// (get) Token: 0x06004F0C RID: 20236 RVA: 0x001230E3 File Offset: 0x001220E3
		// (set) Token: 0x06004F0D RID: 20237 RVA: 0x001230FF File Offset: 0x001220FF
		[SRDescription("TextBoxSelectionLengthDescr")]
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override int SelectionLength
		{
			get
			{
				if (!base.IsHandleCreated)
				{
					return base.SelectionLength;
				}
				return this.SelectedText.Length;
			}
			set
			{
				base.SelectionLength = value;
			}
		}

		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x06004F0E RID: 20238 RVA: 0x00123108 File Offset: 0x00122108
		// (set) Token: 0x06004F0F RID: 20239 RVA: 0x0012311D File Offset: 0x0012211D
		[SRDescription("RichTextBoxSelProtected")]
		[DefaultValue(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool SelectionProtected
		{
			get
			{
				this.ForceHandleCreate();
				return this.GetCharFormat(16, 16) == RichTextBoxSelectionAttribute.All;
			}
			set
			{
				this.ForceHandleCreate();
				this.SetCharFormat(16, value ? 16 : 0, RichTextBoxSelectionAttribute.All);
			}
		}

		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x06004F10 RID: 20240 RVA: 0x00123137 File Offset: 0x00122137
		internal override bool SelectionUsesDbcsOffsetsInWin9x
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x06004F11 RID: 20241 RVA: 0x0012313A File Offset: 0x0012213A
		// (set) Token: 0x06004F12 RID: 20242 RVA: 0x0012314D File Offset: 0x0012214D
		[DefaultValue("")]
		[SRDescription("RichTextBoxSelRTF")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SelectedRtf
		{
			get
			{
				this.ForceHandleCreate();
				return this.StreamOut(32770);
			}
			set
			{
				this.ForceHandleCreate();
				if (value == null)
				{
					value = "";
				}
				this.StreamIn(value, 32770);
			}
		}

		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x06004F13 RID: 20243 RVA: 0x0012316C File Offset: 0x0012216C
		// (set) Token: 0x06004F14 RID: 20244 RVA: 0x001231CC File Offset: 0x001221CC
		[Browsable(false)]
		[DefaultValue(0)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelRightIndent")]
		public int SelectionRightIndent
		{
			get
			{
				int v = 0;
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.rgxTabs = new int[32];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1085, 0, paraformat);
				if ((2 & paraformat.dwMask) != 0)
				{
					v = paraformat.dxRightIndent;
				}
				return RichTextBox.Twip2Pixel(IntPtr.Zero, v, true);
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("SelectionRightIndent", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"SelectionRightIndent",
						value,
						0
					}));
				}
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.dwMask = 2;
				paraformat.dxRightIndent = RichTextBox.Pixel2Twip(IntPtr.Zero, value, true);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1095, 0, paraformat);
			}
		}

		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x06004F15 RID: 20245 RVA: 0x00123254 File Offset: 0x00122254
		// (set) Token: 0x06004F16 RID: 20246 RVA: 0x001232DC File Offset: 0x001222DC
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelTabs")]
		public int[] SelectionTabs
		{
			get
			{
				int[] array = new int[0];
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.rgxTabs = new int[32];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1085, 0, paraformat);
				if ((16 & paraformat.dwMask) != 0)
				{
					array = new int[(int)paraformat.cTabCount];
					for (int i = 0; i < (int)paraformat.cTabCount; i++)
					{
						array[i] = RichTextBox.Twip2Pixel(IntPtr.Zero, paraformat.rgxTabs[i], true);
					}
				}
				return array;
			}
			set
			{
				if (value != null && value.Length > 32)
				{
					throw new ArgumentOutOfRangeException("SelectionTabs", SR.GetString("SelTabCountRange"));
				}
				this.ForceHandleCreate();
				NativeMethods.PARAFORMAT paraformat = new NativeMethods.PARAFORMAT();
				paraformat.rgxTabs = new int[32];
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1085, 0, paraformat);
				paraformat.cTabCount = (short)((value == null) ? 0 : value.Length);
				paraformat.dwMask = 16;
				for (int i = 0; i < (int)paraformat.cTabCount; i++)
				{
					paraformat.rgxTabs[i] = RichTextBox.Pixel2Twip(IntPtr.Zero, value[i], true);
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1095, 0, paraformat);
			}
		}

		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x06004F17 RID: 20247 RVA: 0x00123394 File Offset: 0x00122394
		// (set) Token: 0x06004F18 RID: 20248 RVA: 0x001233B4 File Offset: 0x001223B4
		[DefaultValue("")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelText")]
		public override string SelectedText
		{
			get
			{
				this.ForceHandleCreate();
				return this.StreamOut(32785);
			}
			set
			{
				this.ForceHandleCreate();
				this.StreamIn(value, 32785);
			}
		}

		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x06004F19 RID: 20249 RVA: 0x001233C8 File Offset: 0x001223C8
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("RichTextBoxSelTypeDescr")]
		public RichTextBoxSelectionTypes SelectionType
		{
			get
			{
				this.ForceHandleCreate();
				if (this.SelectionLength > 0)
				{
					return (RichTextBoxSelectionTypes)((int)base.SendMessage(1090, 0, 0));
				}
				return RichTextBoxSelectionTypes.Empty;
			}
		}

		// Token: 0x17001027 RID: 4135
		// (get) Token: 0x06004F1A RID: 20250 RVA: 0x001233FA File Offset: 0x001223FA
		// (set) Token: 0x06004F1B RID: 20251 RVA: 0x00123414 File Offset: 0x00122414
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("RichTextBoxSelMargin")]
		public bool ShowSelectionMargin
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.showSelBarSection] != 0;
			}
			set
			{
				if (value != this.ShowSelectionMargin)
				{
					this.richTextBoxFlags[RichTextBox.showSelBarSection] = (value ? 1 : 0);
					if (base.IsHandleCreated)
					{
						base.SendMessage(1101, value ? 2 : 4, 16777216);
					}
				}
			}
		}

		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x06004F1C RID: 20252 RVA: 0x00123464 File Offset: 0x00122464
		// (set) Token: 0x06004F1D RID: 20253 RVA: 0x001234CC File Offset: 0x001224CC
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.All)]
		public override string Text
		{
			get
			{
				if (base.IsDisposed)
				{
					return base.Text;
				}
				if (base.RecreatingHandle || base.GetAnyDisposingInHierarchy())
				{
					return "";
				}
				if (base.IsHandleCreated || this.textRtf != null)
				{
					this.ForceHandleCreate();
					return this.StreamOut(17);
				}
				if (this.textPlain != null)
				{
					return this.textPlain;
				}
				return base.Text;
			}
			set
			{
				using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Text))
				{
					this.textRtf = null;
					if (!base.IsHandleCreated)
					{
						this.textPlain = value;
					}
					else
					{
						this.textPlain = null;
						if (value == null)
						{
							value = "";
						}
						this.StreamIn(value, 17);
						base.SendMessage(185, 0, 0);
					}
				}
			}
		}

		// Token: 0x17001029 RID: 4137
		// (get) Token: 0x06004F1E RID: 20254 RVA: 0x00123550 File Offset: 0x00122550
		// (set) Token: 0x06004F1F RID: 20255 RVA: 0x00123568 File Offset: 0x00122568
		private bool SuppressTextChangedEvent
		{
			get
			{
				return this.richTextBoxFlags[RichTextBox.suppressTextChangedEventSection] != 0;
			}
			set
			{
				bool suppressTextChangedEvent = this.SuppressTextChangedEvent;
				if (value != suppressTextChangedEvent)
				{
					this.richTextBoxFlags[RichTextBox.suppressTextChangedEventSection] = (value ? 1 : 0);
					CommonProperties.xClearPreferredSizeCache(this);
				}
			}
		}

		// Token: 0x1700102A RID: 4138
		// (get) Token: 0x06004F20 RID: 20256 RVA: 0x001235A0 File Offset: 0x001225A0
		[Browsable(false)]
		public override int TextLength
		{
			get
			{
				NativeMethods.GETTEXTLENGTHEX gettextlengthex = new NativeMethods.GETTEXTLENGTHEX();
				gettextlengthex.flags = 8U;
				if (Marshal.SystemDefaultCharSize == 1)
				{
					gettextlengthex.codepage = 0U;
				}
				else
				{
					gettextlengthex.codepage = 1200U;
				}
				return (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1119, gettextlengthex, 0);
			}
		}

		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x06004F21 RID: 20257 RVA: 0x001235F4 File Offset: 0x001225F4
		[SRDescription("RichTextBoxUndoActionNameDescr")]
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string UndoActionName
		{
			get
			{
				if (!base.CanUndo)
				{
					return "";
				}
				int actionID = (int)base.SendMessage(1110, 0, 0);
				return this.GetEditorActionName(actionID);
			}
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x0012362C File Offset: 0x0012262C
		private string GetEditorActionName(int actionID)
		{
			switch (actionID)
			{
			default:
				return SR.GetString("RichTextBox_IDUnknown");
			case 1:
				return SR.GetString("RichTextBox_IDTyping");
			case 2:
				return SR.GetString("RichTextBox_IDDelete");
			case 3:
				return SR.GetString("RichTextBox_IDDragDrop");
			case 4:
				return SR.GetString("RichTextBox_IDCut");
			case 5:
				return SR.GetString("RichTextBox_IDPaste");
			}
		}

		// Token: 0x1700102C RID: 4140
		// (get) Token: 0x06004F23 RID: 20259 RVA: 0x0012369C File Offset: 0x0012269C
		// (set) Token: 0x06004F24 RID: 20260 RVA: 0x001236F0 File Offset: 0x001226F0
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[DefaultValue(1f)]
		[SRDescription("RichTextBoxZoomFactor")]
		public float ZoomFactor
		{
			get
			{
				if (base.IsHandleCreated)
				{
					int num = 0;
					int num2 = 0;
					base.SendMessage(1248, ref num, ref num2);
					if (num != 0 && num2 != 0)
					{
						this.zoomMultiplier = (float)num / (float)num2;
					}
					else
					{
						this.zoomMultiplier = 1f;
					}
					return this.zoomMultiplier;
				}
				return this.zoomMultiplier;
			}
			set
			{
				if (this.zoomMultiplier == value)
				{
					return;
				}
				if (value <= 0.015625f || value >= 64f)
				{
					throw new ArgumentOutOfRangeException("ZoomFactor", SR.GetString("InvalidExBoundArgument", new object[]
					{
						"ZoomFactor",
						value.ToString(CultureInfo.CurrentCulture),
						0.015625f.ToString(CultureInfo.CurrentCulture),
						64f.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.SendZoomFactor(value);
			}
		}

		// Token: 0x140002E3 RID: 739
		// (add) Token: 0x06004F25 RID: 20261 RVA: 0x0012377C File Offset: 0x0012277C
		// (remove) Token: 0x06004F26 RID: 20262 RVA: 0x0012378F File Offset: 0x0012278F
		[SRDescription("RichTextBoxContentsResized")]
		[SRCategory("CatBehavior")]
		public event ContentsResizedEventHandler ContentsResized
		{
			add
			{
				base.Events.AddHandler(RichTextBox.EVENT_REQUESTRESIZE, value);
			}
			remove
			{
				base.Events.RemoveHandler(RichTextBox.EVENT_REQUESTRESIZE, value);
			}
		}

		// Token: 0x140002E4 RID: 740
		// (add) Token: 0x06004F27 RID: 20263 RVA: 0x001237A2 File Offset: 0x001227A2
		// (remove) Token: 0x06004F28 RID: 20264 RVA: 0x001237AB File Offset: 0x001227AB
		[Browsable(false)]
		public new event DragEventHandler DragDrop
		{
			add
			{
				base.DragDrop += value;
			}
			remove
			{
				base.DragDrop -= value;
			}
		}

		// Token: 0x140002E5 RID: 741
		// (add) Token: 0x06004F29 RID: 20265 RVA: 0x001237B4 File Offset: 0x001227B4
		// (remove) Token: 0x06004F2A RID: 20266 RVA: 0x001237BD File Offset: 0x001227BD
		[Browsable(false)]
		public new event DragEventHandler DragEnter
		{
			add
			{
				base.DragEnter += value;
			}
			remove
			{
				base.DragEnter -= value;
			}
		}

		// Token: 0x140002E6 RID: 742
		// (add) Token: 0x06004F2B RID: 20267 RVA: 0x001237C6 File Offset: 0x001227C6
		// (remove) Token: 0x06004F2C RID: 20268 RVA: 0x001237CF File Offset: 0x001227CF
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler DragLeave
		{
			add
			{
				base.DragLeave += value;
			}
			remove
			{
				base.DragLeave -= value;
			}
		}

		// Token: 0x140002E7 RID: 743
		// (add) Token: 0x06004F2D RID: 20269 RVA: 0x001237D8 File Offset: 0x001227D8
		// (remove) Token: 0x06004F2E RID: 20270 RVA: 0x001237E1 File Offset: 0x001227E1
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event DragEventHandler DragOver
		{
			add
			{
				base.DragOver += value;
			}
			remove
			{
				base.DragOver -= value;
			}
		}

		// Token: 0x140002E8 RID: 744
		// (add) Token: 0x06004F2F RID: 20271 RVA: 0x001237EA File Offset: 0x001227EA
		// (remove) Token: 0x06004F30 RID: 20272 RVA: 0x001237F3 File Offset: 0x001227F3
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event GiveFeedbackEventHandler GiveFeedback
		{
			add
			{
				base.GiveFeedback += value;
			}
			remove
			{
				base.GiveFeedback -= value;
			}
		}

		// Token: 0x140002E9 RID: 745
		// (add) Token: 0x06004F31 RID: 20273 RVA: 0x001237FC File Offset: 0x001227FC
		// (remove) Token: 0x06004F32 RID: 20274 RVA: 0x00123805 File Offset: 0x00122805
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event QueryContinueDragEventHandler QueryContinueDrag
		{
			add
			{
				base.QueryContinueDrag += value;
			}
			remove
			{
				base.QueryContinueDrag -= value;
			}
		}

		// Token: 0x140002EA RID: 746
		// (add) Token: 0x06004F33 RID: 20275 RVA: 0x0012380E File Offset: 0x0012280E
		// (remove) Token: 0x06004F34 RID: 20276 RVA: 0x00123821 File Offset: 0x00122821
		[SRDescription("RichTextBoxHScroll")]
		[SRCategory("CatBehavior")]
		public event EventHandler HScroll
		{
			add
			{
				base.Events.AddHandler(RichTextBox.EVENT_HSCROLL, value);
			}
			remove
			{
				base.Events.RemoveHandler(RichTextBox.EVENT_HSCROLL, value);
			}
		}

		// Token: 0x140002EB RID: 747
		// (add) Token: 0x06004F35 RID: 20277 RVA: 0x00123834 File Offset: 0x00122834
		// (remove) Token: 0x06004F36 RID: 20278 RVA: 0x00123847 File Offset: 0x00122847
		[SRCategory("CatBehavior")]
		[SRDescription("RichTextBoxLinkClick")]
		public event LinkClickedEventHandler LinkClicked
		{
			add
			{
				base.Events.AddHandler(RichTextBox.EVENT_LINKACTIVATE, value);
			}
			remove
			{
				base.Events.RemoveHandler(RichTextBox.EVENT_LINKACTIVATE, value);
			}
		}

		// Token: 0x140002EC RID: 748
		// (add) Token: 0x06004F37 RID: 20279 RVA: 0x0012385A File Offset: 0x0012285A
		// (remove) Token: 0x06004F38 RID: 20280 RVA: 0x0012386D File Offset: 0x0012286D
		[SRDescription("RichTextBoxIMEChange")]
		[SRCategory("CatBehavior")]
		public event EventHandler ImeChange
		{
			add
			{
				base.Events.AddHandler(RichTextBox.EVENT_IMECHANGE, value);
			}
			remove
			{
				base.Events.RemoveHandler(RichTextBox.EVENT_IMECHANGE, value);
			}
		}

		// Token: 0x140002ED RID: 749
		// (add) Token: 0x06004F39 RID: 20281 RVA: 0x00123880 File Offset: 0x00122880
		// (remove) Token: 0x06004F3A RID: 20282 RVA: 0x00123893 File Offset: 0x00122893
		[SRCategory("CatBehavior")]
		[SRDescription("RichTextBoxProtected")]
		public event EventHandler Protected
		{
			add
			{
				base.Events.AddHandler(RichTextBox.EVENT_PROTECTED, value);
			}
			remove
			{
				base.Events.RemoveHandler(RichTextBox.EVENT_PROTECTED, value);
			}
		}

		// Token: 0x140002EE RID: 750
		// (add) Token: 0x06004F3B RID: 20283 RVA: 0x001238A6 File Offset: 0x001228A6
		// (remove) Token: 0x06004F3C RID: 20284 RVA: 0x001238B9 File Offset: 0x001228B9
		[SRDescription("RichTextBoxSelChange")]
		[SRCategory("CatBehavior")]
		public event EventHandler SelectionChanged
		{
			add
			{
				base.Events.AddHandler(RichTextBox.EVENT_SELCHANGE, value);
			}
			remove
			{
				base.Events.RemoveHandler(RichTextBox.EVENT_SELCHANGE, value);
			}
		}

		// Token: 0x140002EF RID: 751
		// (add) Token: 0x06004F3D RID: 20285 RVA: 0x001238CC File Offset: 0x001228CC
		// (remove) Token: 0x06004F3E RID: 20286 RVA: 0x001238DF File Offset: 0x001228DF
		[SRDescription("RichTextBoxVScroll")]
		[SRCategory("CatBehavior")]
		public event EventHandler VScroll
		{
			add
			{
				base.Events.AddHandler(RichTextBox.EVENT_VSCROLL, value);
			}
			remove
			{
				base.Events.RemoveHandler(RichTextBox.EVENT_VSCROLL, value);
			}
		}

		// Token: 0x06004F3F RID: 20287 RVA: 0x001238F4 File Offset: 0x001228F4
		public bool CanPaste(DataFormats.Format clipFormat)
		{
			return (int)base.SendMessage(1074, clipFormat.Id, 0) != 0;
		}

		// Token: 0x06004F40 RID: 20288 RVA: 0x00123922 File Offset: 0x00122922
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds)
		{
			base.DrawToBitmap(bitmap, targetBounds);
		}

		// Token: 0x06004F41 RID: 20289 RVA: 0x0012392C File Offset: 0x0012292C
		private unsafe int EditStreamProc(IntPtr dwCookie, IntPtr buf, int cb, out int transferred)
		{
			int result = 0;
			byte[] array = new byte[cb];
			int num = (int)dwCookie;
			transferred = 0;
			try
			{
				switch (num & 3)
				{
				case 1:
					if (this.editStream != null)
					{
						transferred = this.editStream.Read(array, 0, cb);
						Marshal.Copy(array, 0, buf, transferred);
						if (transferred < 0)
						{
							transferred = 0;
						}
					}
					else
					{
						transferred = 0;
					}
					break;
				case 2:
				{
					if (this.editStream == null)
					{
						this.editStream = new MemoryStream();
					}
					int num2 = num & 112;
					if (num2 != 16)
					{
						if (num2 == 32 || num2 == 64)
						{
							Marshal.Copy(buf, array, 0, cb);
							this.editStream.Write(array, 0, cb);
						}
					}
					else if ((num & 8) != 0)
					{
						int num3 = cb / 2;
						int num4 = 0;
						try
						{
							fixed (byte* ptr = array)
							{
								char* ptr2 = (char*)ptr;
								char* ptr3 = (long)buf;
								for (int i = 0; i < num3; i++)
								{
									if (*ptr3 == '\r')
									{
										ptr3++;
									}
									else
									{
										*ptr2 = *ptr3;
										ptr2++;
										ptr3++;
										num4++;
									}
								}
							}
						}
						finally
						{
							byte* ptr = null;
						}
						this.editStream.Write(array, 0, num4 * 2);
					}
					else
					{
						int num5 = 0;
						try
						{
							fixed (byte* ptr4 = array)
							{
								byte* ptr5 = ptr4;
								byte* ptr6 = (long)buf;
								for (int j = 0; j < cb; j++)
								{
									if (*ptr6 == 13)
									{
										ptr6++;
									}
									else
									{
										*ptr5 = *ptr6;
										ptr5++;
										ptr6++;
										num5++;
									}
								}
							}
						}
						finally
						{
							byte* ptr4 = null;
						}
						this.editStream.Write(array, 0, num5);
					}
					transferred = cb;
					break;
				}
				}
			}
			catch (IOException)
			{
				transferred = 0;
				result = 1;
			}
			return result;
		}

		// Token: 0x06004F42 RID: 20290 RVA: 0x00123B58 File Offset: 0x00122B58
		public int Find(string str)
		{
			return this.Find(str, 0, 0, RichTextBoxFinds.None);
		}

		// Token: 0x06004F43 RID: 20291 RVA: 0x00123B64 File Offset: 0x00122B64
		public int Find(string str, RichTextBoxFinds options)
		{
			return this.Find(str, 0, 0, options);
		}

		// Token: 0x06004F44 RID: 20292 RVA: 0x00123B70 File Offset: 0x00122B70
		public int Find(string str, int start, RichTextBoxFinds options)
		{
			return this.Find(str, start, -1, options);
		}

		// Token: 0x06004F45 RID: 20293 RVA: 0x00123B7C File Offset: 0x00122B7C
		public int Find(string str, int start, int end, RichTextBoxFinds options)
		{
			int textLength = this.TextLength;
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			if (start < 0 || start > textLength)
			{
				throw new ArgumentOutOfRangeException("start", SR.GetString("InvalidBoundArgument", new object[]
				{
					"start",
					start,
					0,
					textLength
				}));
			}
			if (end < -1)
			{
				throw new ArgumentOutOfRangeException("end", SR.GetString("RichTextFindEndInvalid", new object[]
				{
					end
				}));
			}
			bool flag = true;
			NativeMethods.FINDTEXT findtext = new NativeMethods.FINDTEXT();
			findtext.chrg = new NativeMethods.CHARRANGE();
			findtext.lpstrText = str;
			if (end == -1)
			{
				end = textLength;
			}
			if (start > end)
			{
				throw new ArgumentException(SR.GetString("RichTextFindEndInvalid", new object[]
				{
					end
				}));
			}
			if ((options & RichTextBoxFinds.Reverse) != RichTextBoxFinds.Reverse)
			{
				findtext.chrg.cpMin = start;
				findtext.chrg.cpMax = end;
			}
			else
			{
				findtext.chrg.cpMin = end;
				findtext.chrg.cpMax = start;
			}
			if (findtext.chrg.cpMin == findtext.chrg.cpMax)
			{
				if ((options & RichTextBoxFinds.Reverse) != RichTextBoxFinds.Reverse)
				{
					findtext.chrg.cpMin = 0;
					findtext.chrg.cpMax = -1;
				}
				else
				{
					findtext.chrg.cpMin = textLength;
					findtext.chrg.cpMax = 0;
				}
			}
			int num = 0;
			if ((options & RichTextBoxFinds.WholeWord) == RichTextBoxFinds.WholeWord)
			{
				num |= 2;
			}
			if ((options & RichTextBoxFinds.MatchCase) == RichTextBoxFinds.MatchCase)
			{
				num |= 4;
			}
			if ((options & RichTextBoxFinds.NoHighlight) == RichTextBoxFinds.NoHighlight)
			{
				flag = false;
			}
			if ((options & RichTextBoxFinds.Reverse) != RichTextBoxFinds.Reverse)
			{
				num |= 1;
			}
			int num2 = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1080, num, findtext);
			if (num2 != -1 && flag)
			{
				NativeMethods.CHARRANGE charrange = new NativeMethods.CHARRANGE();
				charrange.cpMin = num2;
				char c = 'ـ';
				string text = this.Text;
				string text2 = text.Substring(num2, str.Length);
				int num3 = text2.IndexOf(c);
				if (num3 == -1)
				{
					charrange.cpMax = num2 + str.Length;
				}
				else
				{
					int i = num3;
					int num4 = num2 + num3;
					while (i < str.Length)
					{
						while (text[num4] == c && str[i] != c)
						{
							num4++;
						}
						i++;
						num4++;
					}
					charrange.cpMax = num4;
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1079, 0, charrange);
				base.SendMessage(183, 0, 0);
			}
			return num2;
		}

		// Token: 0x06004F46 RID: 20294 RVA: 0x00123E12 File Offset: 0x00122E12
		public int Find(char[] characterSet)
		{
			return this.Find(characterSet, 0, -1);
		}

		// Token: 0x06004F47 RID: 20295 RVA: 0x00123E1D File Offset: 0x00122E1D
		public int Find(char[] characterSet, int start)
		{
			return this.Find(characterSet, start, -1);
		}

		// Token: 0x06004F48 RID: 20296 RVA: 0x00123E28 File Offset: 0x00122E28
		public int Find(char[] characterSet, int start, int end)
		{
			bool flag = true;
			bool negate = false;
			int textLength = this.TextLength;
			if (characterSet == null)
			{
				throw new ArgumentNullException("characterSet");
			}
			if (start < 0 || start > textLength)
			{
				throw new ArgumentOutOfRangeException("start", SR.GetString("InvalidBoundArgument", new object[]
				{
					"start",
					start,
					0,
					textLength
				}));
			}
			if (end < start && end != -1)
			{
				throw new ArgumentOutOfRangeException("end", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"end",
					end,
					"start"
				}));
			}
			if (characterSet.Length == 0)
			{
				return -1;
			}
			int windowTextLength = SafeNativeMethods.GetWindowTextLength(new HandleRef(this, base.Handle));
			if (start == end)
			{
				start = 0;
				end = windowTextLength;
			}
			if (end == -1)
			{
				end = windowTextLength;
			}
			NativeMethods.CHARRANGE charrange = new NativeMethods.CHARRANGE();
			charrange.cpMax = (charrange.cpMin = start);
			NativeMethods.TEXTRANGE textrange = new NativeMethods.TEXTRANGE();
			textrange.chrg = new NativeMethods.CHARRANGE();
			textrange.chrg.cpMin = charrange.cpMin;
			textrange.chrg.cpMax = charrange.cpMax;
			UnsafeNativeMethods.CharBuffer charBuffer = UnsafeNativeMethods.CharBuffer.CreateBuffer(513);
			textrange.lpstrText = charBuffer.AllocCoTaskMem();
			if (textrange.lpstrText == IntPtr.Zero)
			{
				throw new OutOfMemoryException();
			}
			try
			{
				bool flag2 = false;
				while (!flag2)
				{
					if (flag)
					{
						textrange.chrg.cpMin = charrange.cpMax;
						textrange.chrg.cpMax += 512;
					}
					else
					{
						textrange.chrg.cpMax = charrange.cpMin;
						textrange.chrg.cpMin -= 512;
						if (textrange.chrg.cpMin < 0)
						{
							textrange.chrg.cpMin = 0;
						}
					}
					if (end != -1)
					{
						textrange.chrg.cpMax = Math.Min(textrange.chrg.cpMax, end);
					}
					int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1099, 0, textrange);
					if (num == 0)
					{
						charrange.cpMax = (charrange.cpMin = -1);
						break;
					}
					charBuffer.PutCoTaskMem(textrange.lpstrText);
					string @string = charBuffer.GetString();
					if (flag)
					{
						for (int i = 0; i < num; i++)
						{
							bool charInCharSet = this.GetCharInCharSet(@string[i], characterSet, negate);
							if (charInCharSet)
							{
								flag2 = true;
								break;
							}
							charrange.cpMax++;
						}
					}
					else
					{
						int index = num;
						while (index-- != 0)
						{
							bool charInCharSet2 = this.GetCharInCharSet(@string[index], characterSet, negate);
							if (charInCharSet2)
							{
								flag2 = true;
								break;
							}
							charrange.cpMin--;
						}
					}
				}
			}
			finally
			{
				if (textrange.lpstrText != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(textrange.lpstrText);
				}
			}
			return flag ? charrange.cpMax : charrange.cpMin;
		}

		// Token: 0x06004F49 RID: 20297 RVA: 0x0012415C File Offset: 0x0012315C
		private void ForceHandleCreate()
		{
			if (!base.IsHandleCreated)
			{
				this.CreateHandle();
			}
		}

		// Token: 0x06004F4A RID: 20298 RVA: 0x0012416C File Offset: 0x0012316C
		private bool InternalSetForeColor(Color value)
		{
			NativeMethods.CHARFORMATA charFormat = this.GetCharFormat(false);
			if ((charFormat.dwMask & 1073741824) != 0 && ColorTranslator.ToWin32(value) == charFormat.crTextColor)
			{
				return true;
			}
			charFormat.dwMask = 1073741824;
			charFormat.dwEffects = 0;
			charFormat.crTextColor = ColorTranslator.ToWin32(value);
			return this.SetCharFormat(4, charFormat);
		}

		// Token: 0x06004F4B RID: 20299 RVA: 0x001241C8 File Offset: 0x001231C8
		private NativeMethods.CHARFORMATA GetCharFormat(bool fSelection)
		{
			NativeMethods.CHARFORMATA charformata = new NativeMethods.CHARFORMATA();
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1082, fSelection ? 1 : 0, charformata);
			return charformata;
		}

		// Token: 0x06004F4C RID: 20300 RVA: 0x001241FC File Offset: 0x001231FC
		private NativeMethods.CHARFORMAT2A GetCharFormat2(bool fSelection)
		{
			NativeMethods.CHARFORMAT2A charformat2A = new NativeMethods.CHARFORMAT2A();
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1082, fSelection ? 1 : 0, charformat2A);
			return charformat2A;
		}

		// Token: 0x06004F4D RID: 20301 RVA: 0x00124230 File Offset: 0x00123230
		private RichTextBoxSelectionAttribute GetCharFormat(int mask, int effect)
		{
			RichTextBoxSelectionAttribute result = RichTextBoxSelectionAttribute.None;
			if (base.IsHandleCreated)
			{
				NativeMethods.CHARFORMATA charFormat = this.GetCharFormat(true);
				if ((charFormat.dwMask & mask) != 0 && (charFormat.dwEffects & effect) != 0)
				{
					result = RichTextBoxSelectionAttribute.All;
				}
			}
			return result;
		}

		// Token: 0x06004F4E RID: 20302 RVA: 0x00124268 File Offset: 0x00123268
		private Font GetCharFormatFont(bool selectionOnly)
		{
			this.ForceHandleCreate();
			NativeMethods.CHARFORMATA charFormat = this.GetCharFormat(selectionOnly);
			if ((charFormat.dwMask & 536870912) == 0)
			{
				return null;
			}
			string text = Encoding.Default.GetString(charFormat.szFaceName);
			int num = text.IndexOf('\0');
			if (num != -1)
			{
				text = text.Substring(0, num);
			}
			float num2 = 13f;
			if ((charFormat.dwMask & -2147483648) != 0)
			{
				num2 = (float)charFormat.yHeight / 20f;
				if (num2 == 0f && charFormat.yHeight > 0)
				{
					num2 = 1f;
				}
			}
			FontStyle fontStyle = FontStyle.Regular;
			if ((charFormat.dwMask & 1) != 0 && (charFormat.dwEffects & 1) != 0)
			{
				fontStyle |= FontStyle.Bold;
			}
			if ((charFormat.dwMask & 2) != 0 && (charFormat.dwEffects & 2) != 0)
			{
				fontStyle |= FontStyle.Italic;
			}
			if ((charFormat.dwMask & 8) != 0 && (charFormat.dwEffects & 8) != 0)
			{
				fontStyle |= FontStyle.Strikeout;
			}
			if ((charFormat.dwMask & 4) != 0 && (charFormat.dwEffects & 4) != 0)
			{
				fontStyle |= FontStyle.Underline;
			}
			try
			{
				return new Font(text, num2, fontStyle, GraphicsUnit.Point, charFormat.bCharSet);
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06004F4F RID: 20303 RVA: 0x00124388 File Offset: 0x00123388
		public override int GetCharIndexFromPosition(Point pt)
		{
			NativeMethods.POINT lParam = new NativeMethods.POINT(pt.X, pt.Y);
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 215, 0, lParam);
			string text = this.Text;
			if (num >= text.Length)
			{
				num = Math.Max(text.Length - 1, 0);
			}
			return num;
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x001243E8 File Offset: 0x001233E8
		private bool GetCharInCharSet(char c, char[] charSet, bool negate)
		{
			bool flag = false;
			int num = charSet.Length;
			int num2 = 0;
			while (!flag && num2 < num)
			{
				flag = (c == charSet[num2]);
				num2++;
			}
			if (!negate)
			{
				return flag;
			}
			return !flag;
		}

		// Token: 0x06004F51 RID: 20305 RVA: 0x0012441A File Offset: 0x0012341A
		public override int GetLineFromCharIndex(int index)
		{
			return (int)base.SendMessage(1078, 0, index);
		}

		// Token: 0x06004F52 RID: 20306 RVA: 0x00124430 File Offset: 0x00123430
		public override Point GetPositionFromCharIndex(int index)
		{
			if (RichTextBox.richEditMajorVersion == 2)
			{
				return base.GetPositionFromCharIndex(index);
			}
			if (index < 0 || index > this.Text.Length)
			{
				return Point.Empty;
			}
			NativeMethods.POINT point = new NativeMethods.POINT();
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 214, point, index);
			return new Point(point.x, point.y);
		}

		// Token: 0x06004F53 RID: 20307 RVA: 0x00124495 File Offset: 0x00123495
		private bool GetProtectedError()
		{
			if (this.ProtectedError)
			{
				this.ProtectedError = false;
				return true;
			}
			return false;
		}

		// Token: 0x06004F54 RID: 20308 RVA: 0x001244A9 File Offset: 0x001234A9
		public void LoadFile(string path)
		{
			this.LoadFile(path, RichTextBoxStreamType.RichText);
		}

		// Token: 0x06004F55 RID: 20309 RVA: 0x001244B4 File Offset: 0x001234B4
		public void LoadFile(string path, RichTextBoxStreamType fileType)
		{
			if (!ClientUtils.IsEnumValid(fileType, (int)fileType, 0, 4))
			{
				throw new InvalidEnumArgumentException("fileType", (int)fileType, typeof(RichTextBoxStreamType));
			}
			Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			try
			{
				this.LoadFile(stream, fileType);
			}
			finally
			{
				stream.Close();
			}
		}

		// Token: 0x06004F56 RID: 20310 RVA: 0x00124514 File Offset: 0x00123514
		public void LoadFile(Stream data, RichTextBoxStreamType fileType)
		{
			if (!ClientUtils.IsEnumValid(fileType, (int)fileType, 0, 4))
			{
				throw new InvalidEnumArgumentException("fileType", (int)fileType, typeof(RichTextBoxStreamType));
			}
			int flags;
			switch (fileType)
			{
			case RichTextBoxStreamType.RichText:
				flags = 2;
				goto IL_6C;
			case RichTextBoxStreamType.PlainText:
				this.Rtf = "";
				flags = 1;
				goto IL_6C;
			case RichTextBoxStreamType.UnicodePlainText:
				flags = 17;
				goto IL_6C;
			}
			throw new ArgumentException(SR.GetString("InvalidFileType"));
			IL_6C:
			this.StreamIn(data, flags);
		}

		// Token: 0x06004F57 RID: 20311 RVA: 0x00124595 File Offset: 0x00123595
		protected override void OnBackColorChanged(EventArgs e)
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(1091, 0, ColorTranslator.ToWin32(this.BackColor));
			}
			base.OnBackColorChanged(e);
		}

		// Token: 0x06004F58 RID: 20312 RVA: 0x001245BE File Offset: 0x001235BE
		protected override void OnContextMenuChanged(EventArgs e)
		{
			base.OnContextMenuChanged(e);
			this.UpdateOleCallback();
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x001245D0 File Offset: 0x001235D0
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			string windowText = this.WindowText;
			base.ForceWindowText(null);
			base.ForceWindowText(windowText);
		}

		// Token: 0x06004F5A RID: 20314 RVA: 0x001245FC File Offset: 0x001235FC
		protected virtual void OnContentsResized(ContentsResizedEventArgs e)
		{
			ContentsResizedEventHandler contentsResizedEventHandler = (ContentsResizedEventHandler)base.Events[RichTextBox.EVENT_REQUESTRESIZE];
			if (contentsResizedEventHandler != null)
			{
				contentsResizedEventHandler(this, e);
			}
		}

		// Token: 0x06004F5B RID: 20315 RVA: 0x0012462C File Offset: 0x0012362C
		protected override void OnHandleCreated(EventArgs e)
		{
			this.curSelStart = (this.curSelEnd = (int)(this.curSelType = -1));
			this.UpdateMaxLength();
			base.SendMessage(1093, 0, 79626255);
			int num = this.rightMargin;
			this.rightMargin = 0;
			this.RightMargin = num;
			base.SendMessage(1115, this.DetectUrls ? 1 : 0, 0);
			if (this.selectionBackColorToSetOnHandleCreated != Color.Empty)
			{
				this.SelectionBackColor = this.selectionBackColorToSetOnHandleCreated;
			}
			this.AutoWordSelection = this.AutoWordSelection;
			base.SendMessage(1091, 0, ColorTranslator.ToWin32(this.BackColor));
			this.InternalSetForeColor(this.ForeColor);
			base.OnHandleCreated(e);
			this.UpdateOleCallback();
			try
			{
				this.SuppressTextChangedEvent = true;
				if (this.textRtf != null)
				{
					string rtf = this.textRtf;
					this.textRtf = null;
					this.Rtf = rtf;
				}
				else if (this.textPlain != null)
				{
					string text = this.textPlain;
					this.textPlain = null;
					this.Text = text;
				}
			}
			finally
			{
				this.SuppressTextChangedEvent = false;
			}
			base.SetSelectionOnHandle();
			if (this.ShowSelectionMargin)
			{
				UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 1101, (IntPtr)2, (IntPtr)16777216);
			}
			if (this.languageOption != this.LanguageOption)
			{
				this.LanguageOption = this.languageOption;
			}
			base.ClearUndo();
			this.SendZoomFactor(this.zoomMultiplier);
			SystemEvents.UserPreferenceChanged += this.UserPreferenceChangedHandler;
		}

		// Token: 0x06004F5C RID: 20316 RVA: 0x001247C8 File Offset: 0x001237C8
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			if (!this.InConstructor)
			{
				this.textRtf = this.Rtf;
				if (this.textRtf.Length == 0)
				{
					this.textRtf = null;
				}
			}
			this.oleCallback = null;
			SystemEvents.UserPreferenceChanged -= this.UserPreferenceChangedHandler;
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x0012481C File Offset: 0x0012381C
		protected virtual void OnHScroll(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[RichTextBox.EVENT_HSCROLL];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004F5E RID: 20318 RVA: 0x0012484C File Offset: 0x0012384C
		protected virtual void OnLinkClicked(LinkClickedEventArgs e)
		{
			LinkClickedEventHandler linkClickedEventHandler = (LinkClickedEventHandler)base.Events[RichTextBox.EVENT_LINKACTIVATE];
			if (linkClickedEventHandler != null)
			{
				linkClickedEventHandler(this, e);
			}
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x0012487C File Offset: 0x0012387C
		protected virtual void OnImeChange(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[RichTextBox.EVENT_IMECHANGE];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x001248AC File Offset: 0x001238AC
		protected virtual void OnProtected(EventArgs e)
		{
			this.ProtectedError = true;
			EventHandler eventHandler = (EventHandler)base.Events[RichTextBox.EVENT_PROTECTED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004F61 RID: 20321 RVA: 0x001248E4 File Offset: 0x001238E4
		protected virtual void OnSelectionChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[RichTextBox.EVENT_SELCHANGE];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004F62 RID: 20322 RVA: 0x00124914 File Offset: 0x00123914
		protected virtual void OnVScroll(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[RichTextBox.EVENT_VSCROLL];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x00124942 File Offset: 0x00123942
		public void Paste(DataFormats.Format clipFormat)
		{
			IntSecurity.ClipboardRead.Demand();
			this.PasteUnsafe(clipFormat, 0);
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x00124958 File Offset: 0x00123958
		private void PasteUnsafe(DataFormats.Format clipFormat, int hIcon)
		{
			NativeMethods.REPASTESPECIAL repastespecial = null;
			if (hIcon != 0)
			{
				repastespecial = new NativeMethods.REPASTESPECIAL();
				repastespecial.dwAspect = 4;
				repastespecial.dwParam = hIcon;
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1088, clipFormat.Id, repastespecial);
		}

		// Token: 0x06004F65 RID: 20325 RVA: 0x0012499C File Offset: 0x0012399C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if (!this.RichTextShortcutsEnabled)
			{
				foreach (int num in RichTextBox.shortcutsToDisable)
				{
					if (keyData == (Keys)num)
					{
						return true;
					}
				}
			}
			return base.ProcessCmdKey(ref m, keyData);
		}

		// Token: 0x06004F66 RID: 20326 RVA: 0x001249DB File Offset: 0x001239DB
		public void Redo()
		{
			base.SendMessage(1108, 0, 0);
		}

		// Token: 0x06004F67 RID: 20327 RVA: 0x001249EB File Offset: 0x001239EB
		public void SaveFile(string path)
		{
			this.SaveFile(path, RichTextBoxStreamType.RichText);
		}

		// Token: 0x06004F68 RID: 20328 RVA: 0x001249F8 File Offset: 0x001239F8
		public void SaveFile(string path, RichTextBoxStreamType fileType)
		{
			if (!ClientUtils.IsEnumValid(fileType, (int)fileType, 0, 4))
			{
				throw new InvalidEnumArgumentException("fileType", (int)fileType, typeof(RichTextBoxStreamType));
			}
			Stream stream = File.Create(path);
			try
			{
				this.SaveFile(stream, fileType);
			}
			finally
			{
				stream.Close();
			}
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x00124A54 File Offset: 0x00123A54
		public void SaveFile(Stream data, RichTextBoxStreamType fileType)
		{
			int flags;
			switch (fileType)
			{
			case RichTextBoxStreamType.RichText:
				flags = 2;
				break;
			case RichTextBoxStreamType.PlainText:
				flags = 1;
				break;
			case RichTextBoxStreamType.RichNoOleObjs:
				flags = 3;
				break;
			case RichTextBoxStreamType.TextTextOleObjs:
				flags = 4;
				break;
			case RichTextBoxStreamType.UnicodePlainText:
				flags = 17;
				break;
			default:
				throw new InvalidEnumArgumentException("fileType", (int)fileType, typeof(RichTextBoxStreamType));
			}
			this.StreamOut(data, flags, true);
		}

		// Token: 0x06004F6A RID: 20330 RVA: 0x00124AB4 File Offset: 0x00123AB4
		private void SendZoomFactor(float zoom)
		{
			int num;
			int num2;
			if (zoom == 1f)
			{
				num = 0;
				num2 = 0;
			}
			else
			{
				num = 1000;
				float num3 = 1000f * zoom;
				num2 = (int)Math.Ceiling((double)num3);
				if (num2 >= 64000)
				{
					num2 = (int)Math.Floor((double)num3);
				}
			}
			if (base.IsHandleCreated)
			{
				base.SendMessage(1249, num2, num);
			}
			if (num2 != 0)
			{
				this.zoomMultiplier = (float)num2 / (float)num;
				return;
			}
			this.zoomMultiplier = 1f;
		}

		// Token: 0x06004F6B RID: 20331 RVA: 0x00124B28 File Offset: 0x00123B28
		private bool SetCharFormat(int mask, int effect, RichTextBoxSelectionAttribute charFormat)
		{
			if (base.IsHandleCreated)
			{
				NativeMethods.CHARFORMATA charformata = new NativeMethods.CHARFORMATA();
				charformata.dwMask = mask;
				switch (charFormat)
				{
				case RichTextBoxSelectionAttribute.None:
					charformata.dwEffects = 0;
					break;
				case RichTextBoxSelectionAttribute.All:
					charformata.dwEffects = effect;
					break;
				default:
					throw new ArgumentException(SR.GetString("UnknownAttr"));
				}
				return IntPtr.Zero != UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1092, 1, charformata);
			}
			return false;
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x00124BA2 File Offset: 0x00123BA2
		private bool SetCharFormat(int charRange, NativeMethods.CHARFORMATA cf)
		{
			return IntPtr.Zero != UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1092, charRange, cf);
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x00124BC8 File Offset: 0x00123BC8
		private void SetCharFormatFont(bool selectionOnly, Font value)
		{
			this.ForceHandleCreate();
			NativeMethods.LOGFONT logfont = new NativeMethods.LOGFONT();
			RichTextBox.FontToLogFont(value, logfont);
			int dwMask = -1476394993;
			int num = 0;
			if (value.Bold)
			{
				num |= 1;
			}
			if (value.Italic)
			{
				num |= 2;
			}
			if (value.Strikeout)
			{
				num |= 8;
			}
			if (value.Underline)
			{
				num |= 4;
			}
			byte[] bytes;
			if (Marshal.SystemDefaultCharSize == 1)
			{
				bytes = Encoding.Default.GetBytes(logfont.lfFaceName);
				NativeMethods.CHARFORMATA charformata = new NativeMethods.CHARFORMATA();
				for (int i = 0; i < bytes.Length; i++)
				{
					charformata.szFaceName[i] = bytes[i];
				}
				charformata.dwMask = dwMask;
				charformata.dwEffects = num;
				charformata.yHeight = (int)(value.SizeInPoints * 20f);
				charformata.bCharSet = logfont.lfCharSet;
				charformata.bPitchAndFamily = logfont.lfPitchAndFamily;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1092, selectionOnly ? 1 : 4, charformata);
				return;
			}
			bytes = Encoding.Unicode.GetBytes(logfont.lfFaceName);
			NativeMethods.CHARFORMATW charformatw = new NativeMethods.CHARFORMATW();
			for (int j = 0; j < bytes.Length; j++)
			{
				charformatw.szFaceName[j] = bytes[j];
			}
			charformatw.dwMask = dwMask;
			charformatw.dwEffects = num;
			charformatw.yHeight = (int)(value.SizeInPoints * 20f);
			charformatw.bCharSet = logfont.lfCharSet;
			charformatw.bPitchAndFamily = logfont.lfPitchAndFamily;
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1092, selectionOnly ? 1 : 4, charformatw);
		}

		// Token: 0x06004F6E RID: 20334 RVA: 0x00124D58 File Offset: 0x00123D58
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private static void FontToLogFont(Font value, NativeMethods.LOGFONT logfont)
		{
			value.ToLogFont(logfont);
		}

		// Token: 0x06004F6F RID: 20335 RVA: 0x00124D64 File Offset: 0x00123D64
		private static void SetupLogPixels(IntPtr hDC)
		{
			bool flag = false;
			if (hDC == IntPtr.Zero)
			{
				hDC = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
				flag = true;
			}
			if (hDC == IntPtr.Zero)
			{
				return;
			}
			RichTextBox.logPixelsX = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, hDC), 88);
			RichTextBox.logPixelsY = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, hDC), 90);
			if (flag)
			{
				UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, hDC));
			}
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x00124DD8 File Offset: 0x00123DD8
		private static int Pixel2Twip(IntPtr hDC, int v, bool xDirection)
		{
			RichTextBox.SetupLogPixels(hDC);
			int num = xDirection ? RichTextBox.logPixelsX : RichTextBox.logPixelsY;
			return (int)((double)v / (double)num * 72.0 * 20.0);
		}

		// Token: 0x06004F71 RID: 20337 RVA: 0x00124E18 File Offset: 0x00123E18
		private static int Twip2Pixel(IntPtr hDC, int v, bool xDirection)
		{
			RichTextBox.SetupLogPixels(hDC);
			int num = xDirection ? RichTextBox.logPixelsX : RichTextBox.logPixelsY;
			return (int)((double)v / 20.0 / 72.0 * (double)num);
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x00124E58 File Offset: 0x00123E58
		private void StreamIn(string str, int flags)
		{
			if (str.Length != 0)
			{
				int num = str.IndexOf('\0');
				if (num != -1)
				{
					str = str.Substring(0, num);
				}
				byte[] bytes;
				if ((flags & 16) != 0)
				{
					bytes = Encoding.Unicode.GetBytes(str);
				}
				else
				{
					bytes = Encoding.Default.GetBytes(str);
				}
				this.editStream = new MemoryStream(bytes.Length);
				this.editStream.Write(bytes, 0, bytes.Length);
				this.editStream.Position = 0L;
				this.StreamIn(this.editStream, flags);
				return;
			}
			if ((32768 & flags) != 0)
			{
				base.SendMessage(771, 0, 0);
				this.ProtectedError = false;
				return;
			}
			base.SendMessage(12, 0, "");
		}

		// Token: 0x06004F73 RID: 20339 RVA: 0x00124F0C File Offset: 0x00123F0C
		private void StreamIn(Stream data, int flags)
		{
			if ((flags & 32768) == 0)
			{
				NativeMethods.CHARRANGE lParam = new NativeMethods.CHARRANGE();
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1079, 0, lParam);
			}
			try
			{
				this.editStream = data;
				if ((flags & 2) != 0)
				{
					long position = this.editStream.Position;
					byte[] array = new byte[RichTextBox.SZ_RTF_TAG.Length];
					this.editStream.Read(array, (int)position, RichTextBox.SZ_RTF_TAG.Length);
					string @string = Encoding.Default.GetString(array);
					if (!RichTextBox.SZ_RTF_TAG.Equals(@string))
					{
						throw new ArgumentException(SR.GetString("InvalidFileFormat"));
					}
					this.editStream.Position = position;
				}
				NativeMethods.EDITSTREAM editstream = new NativeMethods.EDITSTREAM();
				int num;
				if ((flags & 16) != 0)
				{
					num = 9;
				}
				else
				{
					num = 5;
				}
				if ((flags & 2) != 0)
				{
					num |= 64;
				}
				else
				{
					num |= 16;
				}
				editstream.dwCookie = (IntPtr)num;
				editstream.pfnCallback = new NativeMethods.EditStreamCallback(this.EditStreamProc);
				base.SendMessage(1077, 0, int.MaxValue);
				if (IntPtr.Size == 8)
				{
					NativeMethods.EDITSTREAM64 editstream2 = this.ConvertToEDITSTREAM64(editstream);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1097, flags, editstream2);
					editstream.dwError = this.GetErrorValue64(editstream2);
				}
				else
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1097, flags, editstream);
				}
				this.UpdateMaxLength();
				if (!this.GetProtectedError())
				{
					if (editstream.dwError != 0)
					{
						throw new InvalidOperationException(SR.GetString("LoadTextError"));
					}
					base.SendMessage(185, -1, 0);
					base.SendMessage(186, 0, 0);
				}
			}
			finally
			{
				this.editStream = null;
			}
		}

		// Token: 0x06004F74 RID: 20340 RVA: 0x001250DC File Offset: 0x001240DC
		private string StreamOut(int flags)
		{
			Stream stream = new MemoryStream();
			this.StreamOut(stream, flags, false);
			stream.Position = 0L;
			int num = (int)stream.Length;
			string text = string.Empty;
			if (num > 0)
			{
				byte[] array = new byte[num];
				stream.Read(array, 0, num);
				if ((flags & 16) != 0)
				{
					text = Encoding.Unicode.GetString(array, 0, array.Length);
				}
				else
				{
					text = Encoding.Default.GetString(array, 0, array.Length);
				}
				if (!string.IsNullOrEmpty(text) && text[text.Length - 1] == '\0')
				{
					text = text.Substring(0, text.Length - 1);
				}
			}
			return text;
		}

		// Token: 0x06004F75 RID: 20341 RVA: 0x00125174 File Offset: 0x00124174
		private void StreamOut(Stream data, int flags, bool includeCrLfs)
		{
			this.editStream = data;
			try
			{
				NativeMethods.EDITSTREAM editstream = new NativeMethods.EDITSTREAM();
				int num;
				if ((flags & 16) != 0)
				{
					num = 10;
				}
				else
				{
					num = 6;
				}
				if ((flags & 2) != 0)
				{
					num |= 64;
				}
				else if (includeCrLfs)
				{
					num |= 32;
				}
				else
				{
					num |= 16;
				}
				editstream.dwCookie = (IntPtr)num;
				editstream.pfnCallback = new NativeMethods.EditStreamCallback(this.EditStreamProc);
				if (IntPtr.Size == 8)
				{
					NativeMethods.EDITSTREAM64 editstream2 = this.ConvertToEDITSTREAM64(editstream);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1098, flags, editstream2);
					editstream.dwError = this.GetErrorValue64(editstream2);
				}
				else
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1098, flags, editstream);
				}
				if (editstream.dwError != 0)
				{
					throw new InvalidOperationException(SR.GetString("SaveTextError"));
				}
			}
			finally
			{
				this.editStream = null;
			}
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x0012525C File Offset: 0x0012425C
		private unsafe NativeMethods.EDITSTREAM64 ConvertToEDITSTREAM64(NativeMethods.EDITSTREAM es)
		{
			NativeMethods.EDITSTREAM64 editstream = new NativeMethods.EDITSTREAM64();
			fixed (byte* ptr = &editstream.contents[0])
			{
				*(long*)ptr = (long)es.dwCookie;
				((int*)ptr)[2] = es.dwError;
				long num = (long)Marshal.GetFunctionPointerForDelegate(es.pfnCallback);
				byte* ptr2 = (byte*)(&num);
				for (int i = 0; i < 8; i++)
				{
					editstream.contents[i + 12] = ptr2[i];
				}
			}
			return editstream;
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x001252D0 File Offset: 0x001242D0
		private unsafe int GetErrorValue64(NativeMethods.EDITSTREAM64 es64)
		{
			int result;
			fixed (byte* ptr = &es64.contents[0])
			{
				result = ((int*)ptr)[2];
			}
			return result;
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x001252F8 File Offset: 0x001242F8
		private void UpdateOleCallback()
		{
			if (base.IsHandleCreated)
			{
				if (this.oleCallback == null)
				{
					bool flag = false;
					try
					{
						IntSecurity.UnmanagedCode.Demand();
						flag = true;
					}
					catch (SecurityException)
					{
						flag = false;
					}
					if (flag)
					{
						this.AllowOleObjects = true;
					}
					else
					{
						this.AllowOleObjects = (0 != (int)base.SendMessage(1294, 0, 1));
					}
					this.oleCallback = this.CreateRichEditOleCallback();
					IntPtr iunknownForObject = Marshal.GetIUnknownForObject(this.oleCallback);
					try
					{
						Guid guid = typeof(UnsafeNativeMethods.IRichEditOleCallback).GUID;
						IntPtr intPtr;
						Marshal.QueryInterface(iunknownForObject, ref guid, out intPtr);
						try
						{
							UnsafeNativeMethods.SendCallbackMessage(new HandleRef(this, base.Handle), 1094, IntPtr.Zero, intPtr);
						}
						finally
						{
							Marshal.Release(intPtr);
						}
					}
					finally
					{
						Marshal.Release(iunknownForObject);
					}
				}
				UnsafeNativeMethods.DragAcceptFiles(new HandleRef(this, base.Handle), false);
			}
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x001253F8 File Offset: 0x001243F8
		private void UserPreferenceChangedHandler(object o, UserPreferenceChangedEventArgs e)
		{
			if (base.IsHandleCreated)
			{
				if (this.BackColor.IsSystemColor)
				{
					base.SendMessage(1091, 0, ColorTranslator.ToWin32(this.BackColor));
				}
				if (this.ForeColor.IsSystemColor)
				{
					this.InternalSetForeColor(this.ForeColor);
				}
			}
		}

		// Token: 0x06004F7A RID: 20346 RVA: 0x00125452 File Offset: 0x00124452
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected virtual object CreateRichEditOleCallback()
		{
			return new RichTextBox.OleCallback(this);
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x0012545C File Offset: 0x0012445C
		private void EnLinkMsgHandler(ref Message m)
		{
			NativeMethods.ENLINK enlink;
			if (IntPtr.Size == 8)
			{
				enlink = RichTextBox.ConvertFromENLINK64((NativeMethods.ENLINK64)m.GetLParam(typeof(NativeMethods.ENLINK64)));
			}
			else
			{
				enlink = (NativeMethods.ENLINK)m.GetLParam(typeof(NativeMethods.ENLINK));
			}
			int msg = enlink.msg;
			if (msg == 32)
			{
				this.LinkCursor = true;
				m.Result = (IntPtr)1;
				return;
			}
			if (msg != 513)
			{
				m.Result = IntPtr.Zero;
				return;
			}
			string text = this.CharRangeToString(enlink.charrange);
			if (!string.IsNullOrEmpty(text))
			{
				this.OnLinkClicked(new LinkClickedEventArgs(text));
			}
			m.Result = (IntPtr)1;
		}

		// Token: 0x06004F7C RID: 20348 RVA: 0x00125508 File Offset: 0x00124508
		private string CharRangeToString(NativeMethods.CHARRANGE c)
		{
			NativeMethods.TEXTRANGE textrange = new NativeMethods.TEXTRANGE();
			textrange.chrg = c;
			if (c.cpMax > this.Text.Length || c.cpMax - c.cpMin <= 0)
			{
				return string.Empty;
			}
			int size = c.cpMax - c.cpMin + 1;
			UnsafeNativeMethods.CharBuffer charBuffer = UnsafeNativeMethods.CharBuffer.CreateBuffer(size);
			IntPtr intPtr = charBuffer.AllocCoTaskMem();
			if (intPtr == IntPtr.Zero)
			{
				throw new OutOfMemoryException(SR.GetString("OutOfMemory"));
			}
			textrange.lpstrText = intPtr;
			(int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1099, 0, textrange);
			charBuffer.PutCoTaskMem(intPtr);
			if (textrange.lpstrText != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(intPtr);
			}
			return charBuffer.GetString();
		}

		// Token: 0x06004F7D RID: 20349 RVA: 0x001255D4 File Offset: 0x001245D4
		internal override void UpdateMaxLength()
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(1077, 0, this.MaxLength);
			}
		}

		// Token: 0x06004F7E RID: 20350 RVA: 0x001255F4 File Offset: 0x001245F4
		private void WmReflectCommand(ref Message m)
		{
			if (!(m.LParam == base.Handle) || base.GetState(262144))
			{
				base.WndProc(ref m);
				return;
			}
			switch (NativeMethods.Util.HIWORD(m.WParam))
			{
			case 1537:
				this.OnHScroll(EventArgs.Empty);
				return;
			case 1538:
				this.OnVScroll(EventArgs.Empty);
				return;
			default:
				base.WndProc(ref m);
				return;
			}
		}

		// Token: 0x06004F7F RID: 20351 RVA: 0x0012566C File Offset: 0x0012466C
		internal void WmReflectNotify(ref Message m)
		{
			if (m.HWnd == base.Handle)
			{
				int code = ((NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR))).code;
				switch (code)
				{
				case 1793:
					if (!this.CallOnContentsResized)
					{
						NativeMethods.REQRESIZE reqresize = (NativeMethods.REQRESIZE)m.GetLParam(typeof(NativeMethods.REQRESIZE));
						if (base.BorderStyle == BorderStyle.Fixed3D)
						{
							NativeMethods.REQRESIZE reqresize2 = reqresize;
							reqresize2.rc.bottom = reqresize2.rc.bottom + 1;
						}
						this.OnContentsResized(new ContentsResizedEventArgs(Rectangle.FromLTRB(reqresize.rc.left, reqresize.rc.top, reqresize.rc.right, reqresize.rc.bottom)));
						return;
					}
					break;
				case 1794:
				{
					NativeMethods.SELCHANGE selChange = (NativeMethods.SELCHANGE)m.GetLParam(typeof(NativeMethods.SELCHANGE));
					this.WmSelectionChange(selChange);
					return;
				}
				case 1795:
				{
					NativeMethods.ENDROPFILES endropfiles = (NativeMethods.ENDROPFILES)m.GetLParam(typeof(NativeMethods.ENDROPFILES));
					StringBuilder stringBuilder = new StringBuilder(260);
					UnsafeNativeMethods.DragQueryFile(new HandleRef(endropfiles, endropfiles.hDrop), 0, stringBuilder, 260);
					try
					{
						this.LoadFile(stringBuilder.ToString(), RichTextBoxStreamType.RichText);
					}
					catch
					{
						try
						{
							this.LoadFile(stringBuilder.ToString(), RichTextBoxStreamType.PlainText);
						}
						catch
						{
						}
					}
					m.Result = (IntPtr)1;
					return;
				}
				case 1796:
				{
					NativeMethods.ENPROTECTED enprotected;
					if (IntPtr.Size == 8)
					{
						enprotected = this.ConvertFromENPROTECTED64((NativeMethods.ENPROTECTED64)m.GetLParam(typeof(NativeMethods.ENPROTECTED64)));
					}
					else
					{
						enprotected = (NativeMethods.ENPROTECTED)m.GetLParam(typeof(NativeMethods.ENPROTECTED));
					}
					int msg = enprotected.msg;
					if (msg <= 769)
					{
						if (msg != 12)
						{
							if (msg == 194)
							{
								goto IL_276;
							}
							if (msg != 769)
							{
								goto IL_26F;
							}
						}
					}
					else if (msg != 1077)
					{
						if (msg != 1092)
						{
							switch (msg)
							{
							case 1095:
								goto IL_276;
							case 1096:
								goto IL_26F;
							case 1097:
								if (((int)enprotected.wParam & 32768) == 0)
								{
									m.Result = IntPtr.Zero;
									return;
								}
								goto IL_276;
							default:
								goto IL_26F;
							}
						}
						else
						{
							NativeMethods.CHARFORMATA charformata = (NativeMethods.CHARFORMATA)UnsafeNativeMethods.PtrToStructure(enprotected.lParam, typeof(NativeMethods.CHARFORMATA));
							if ((charformata.dwMask & 16) != 0)
							{
								m.Result = IntPtr.Zero;
								return;
							}
							goto IL_276;
						}
					}
					m.Result = IntPtr.Zero;
					return;
					IL_26F:
					SafeNativeMethods.MessageBeep(0);
					IL_276:
					this.OnProtected(EventArgs.Empty);
					m.Result = (IntPtr)1;
					return;
				}
				default:
					if (code == 1803)
					{
						this.EnLinkMsgHandler(ref m);
						return;
					}
					base.WndProc(ref m);
					return;
				}
			}
			else
			{
				base.WndProc(ref m);
			}
		}

		// Token: 0x06004F80 RID: 20352 RVA: 0x00125934 File Offset: 0x00124934
		private unsafe NativeMethods.ENPROTECTED ConvertFromENPROTECTED64(NativeMethods.ENPROTECTED64 es64)
		{
			NativeMethods.ENPROTECTED enprotected = new NativeMethods.ENPROTECTED();
			fixed (byte* ptr = &es64.contents[0])
			{
				enprotected.nmhdr = default(NativeMethods.NMHDR);
				enprotected.chrg = new NativeMethods.CHARRANGE();
				enprotected.nmhdr.hwndFrom = Marshal.ReadIntPtr((IntPtr)((void*)ptr));
				enprotected.nmhdr.idFrom = Marshal.ReadIntPtr((IntPtr)((void*)((byte*)ptr + 8)));
				enprotected.nmhdr.code = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 16)));
				enprotected.msg = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 24)));
				enprotected.wParam = Marshal.ReadIntPtr((IntPtr)((void*)((byte*)ptr + 28)));
				enprotected.lParam = Marshal.ReadIntPtr((IntPtr)((void*)((byte*)ptr + 36)));
				enprotected.chrg.cpMin = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 44)));
				enprotected.chrg.cpMax = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 48)));
			}
			return enprotected;
		}

		// Token: 0x06004F81 RID: 20353 RVA: 0x00125A34 File Offset: 0x00124A34
		private unsafe static NativeMethods.ENLINK ConvertFromENLINK64(NativeMethods.ENLINK64 es64)
		{
			NativeMethods.ENLINK enlink = new NativeMethods.ENLINK();
			fixed (byte* ptr = &es64.contents[0])
			{
				enlink.nmhdr = default(NativeMethods.NMHDR);
				enlink.charrange = new NativeMethods.CHARRANGE();
				enlink.nmhdr.hwndFrom = Marshal.ReadIntPtr((IntPtr)((void*)ptr));
				enlink.nmhdr.idFrom = Marshal.ReadIntPtr((IntPtr)((void*)((byte*)ptr + 8)));
				enlink.nmhdr.code = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 16)));
				enlink.msg = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 24)));
				enlink.wParam = Marshal.ReadIntPtr((IntPtr)((void*)((byte*)ptr + 28)));
				enlink.lParam = Marshal.ReadIntPtr((IntPtr)((void*)((byte*)ptr + 36)));
				enlink.charrange.cpMin = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 44)));
				enlink.charrange.cpMax = Marshal.ReadInt32((IntPtr)((void*)((byte*)ptr + 48)));
			}
			return enlink;
		}

		// Token: 0x06004F82 RID: 20354 RVA: 0x00125B34 File Offset: 0x00124B34
		private void WmSelectionChange(NativeMethods.SELCHANGE selChange)
		{
			int cpMin = selChange.chrg.cpMin;
			int cpMax = selChange.chrg.cpMax;
			short num = (short)selChange.seltyp;
			if (base.ImeMode == ImeMode.Hangul || base.ImeMode == ImeMode.HangulFull)
			{
				int num2 = (int)base.SendMessage(1146, 0, 0);
				if (num2 != 0)
				{
					int windowTextLength = SafeNativeMethods.GetWindowTextLength(new HandleRef(this, base.Handle));
					if (cpMin == cpMax && windowTextLength == this.MaxLength)
					{
						base.SendMessage(8, 0, 0);
						base.SendMessage(7, 0, 0);
						UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 177, cpMax - 1, cpMax);
					}
				}
			}
			if (cpMin != this.curSelStart || cpMax != this.curSelEnd || num != this.curSelType)
			{
				this.curSelStart = cpMin;
				this.curSelEnd = cpMax;
				this.curSelType = num;
				this.OnSelectionChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x00125C18 File Offset: 0x00124C18
		private void WmSetFont(ref Message m)
		{
			try
			{
				this.SuppressTextChangedEvent = true;
				base.WndProc(ref m);
			}
			finally
			{
				this.SuppressTextChangedEvent = false;
			}
			this.InternalSetForeColor(this.ForeColor);
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x00125C5C File Offset: 0x00124C5C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 135)
			{
				if (msg <= 48)
				{
					if (msg != 32)
					{
						if (msg == 48)
						{
							this.WmSetFont(ref m);
							return;
						}
					}
					else
					{
						this.LinkCursor = false;
						this.DefWndProc(ref m);
						if (this.LinkCursor && !this.Cursor.Equals(Cursors.WaitCursor))
						{
							UnsafeNativeMethods.SetCursor(new HandleRef(Cursors.Hand, Cursors.Hand.Handle));
							m.Result = (IntPtr)1;
							return;
						}
						base.WndProc(ref m);
						return;
					}
				}
				else if (msg != 61)
				{
					if (msg == 135)
					{
						base.WndProc(ref m);
						m.Result = (IntPtr)(base.AcceptsTab ? ((int)m.Result | 2) : ((int)m.Result & -3));
						return;
					}
				}
				else
				{
					base.WndProc(ref m);
					if ((int)((long)m.LParam) == -12)
					{
						m.Result = (IntPtr)((Marshal.SystemDefaultCharSize == 1) ? 65565 : 65566);
						return;
					}
					return;
				}
			}
			else if (msg <= 517)
			{
				switch (msg)
				{
				case 276:
				{
					base.WndProc(ref m);
					int num = NativeMethods.Util.LOWORD(m.WParam);
					if (num == 5)
					{
						this.OnHScroll(EventArgs.Empty);
					}
					if (num == 4)
					{
						this.OnHScroll(EventArgs.Empty);
						return;
					}
					return;
				}
				case 277:
				{
					base.WndProc(ref m);
					int num = NativeMethods.Util.LOWORD(m.WParam);
					if (num == 5)
					{
						this.OnVScroll(EventArgs.Empty);
						return;
					}
					if (num == 4)
					{
						this.OnVScroll(EventArgs.Empty);
						return;
					}
					return;
				}
				default:
					if (msg == 517)
					{
						bool style = base.GetStyle(ControlStyles.UserMouse);
						base.SetStyle(ControlStyles.UserMouse, true);
						base.WndProc(ref m);
						base.SetStyle(ControlStyles.UserMouse, style);
						return;
					}
					break;
				}
			}
			else
			{
				if (msg == 642)
				{
					this.OnImeChange(EventArgs.Empty);
					base.WndProc(ref m);
					return;
				}
				if (msg == 8270)
				{
					this.WmReflectNotify(ref m);
					return;
				}
				if (msg == 8465)
				{
					this.WmReflectCommand(ref m);
					return;
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x040032D1 RID: 13009
		private const int DV_E_DVASPECT = -2147221397;

		// Token: 0x040032D2 RID: 13010
		private const int DVASPECT_CONTENT = 1;

		// Token: 0x040032D3 RID: 13011
		private const int DVASPECT_THUMBNAIL = 2;

		// Token: 0x040032D4 RID: 13012
		private const int DVASPECT_ICON = 4;

		// Token: 0x040032D5 RID: 13013
		private const int DVASPECT_DOCPRINT = 8;

		// Token: 0x040032D6 RID: 13014
		internal const int INPUT = 1;

		// Token: 0x040032D7 RID: 13015
		internal const int OUTPUT = 2;

		// Token: 0x040032D8 RID: 13016
		internal const int DIRECTIONMASK = 3;

		// Token: 0x040032D9 RID: 13017
		internal const int ANSI = 4;

		// Token: 0x040032DA RID: 13018
		internal const int UNICODE = 8;

		// Token: 0x040032DB RID: 13019
		internal const int FORMATMASK = 12;

		// Token: 0x040032DC RID: 13020
		internal const int TEXTLF = 16;

		// Token: 0x040032DD RID: 13021
		internal const int TEXTCRLF = 32;

		// Token: 0x040032DE RID: 13022
		internal const int RTF = 64;

		// Token: 0x040032DF RID: 13023
		internal const int KINDMASK = 112;

		// Token: 0x040032E0 RID: 13024
		private const int CHAR_BUFFER_LEN = 512;

		// Token: 0x040032E1 RID: 13025
		private static TraceSwitch richTextDbg;

		// Token: 0x040032E2 RID: 13026
		private static IntPtr moduleHandle;

		// Token: 0x040032E3 RID: 13027
		private static readonly string SZ_RTF_TAG = "{\\rtf";

		// Token: 0x040032E4 RID: 13028
		private static readonly object EVENT_HSCROLL = new object();

		// Token: 0x040032E5 RID: 13029
		private static readonly object EVENT_LINKACTIVATE = new object();

		// Token: 0x040032E6 RID: 13030
		private static readonly object EVENT_IMECHANGE = new object();

		// Token: 0x040032E7 RID: 13031
		private static readonly object EVENT_PROTECTED = new object();

		// Token: 0x040032E8 RID: 13032
		private static readonly object EVENT_REQUESTRESIZE = new object();

		// Token: 0x040032E9 RID: 13033
		private static readonly object EVENT_SELCHANGE = new object();

		// Token: 0x040032EA RID: 13034
		private static readonly object EVENT_VSCROLL = new object();

		// Token: 0x040032EB RID: 13035
		private int bulletIndent;

		// Token: 0x040032EC RID: 13036
		private int rightMargin;

		// Token: 0x040032ED RID: 13037
		private string textRtf;

		// Token: 0x040032EE RID: 13038
		private string textPlain;

		// Token: 0x040032EF RID: 13039
		private Color selectionBackColorToSetOnHandleCreated;

		// Token: 0x040032F0 RID: 13040
		private RichTextBoxLanguageOptions languageOption = RichTextBoxLanguageOptions.AutoFont | RichTextBoxLanguageOptions.DualFont;

		// Token: 0x040032F1 RID: 13041
		private static int logPixelsX;

		// Token: 0x040032F2 RID: 13042
		private static int logPixelsY;

		// Token: 0x040032F3 RID: 13043
		private Stream editStream;

		// Token: 0x040032F4 RID: 13044
		private float zoomMultiplier = 1f;

		// Token: 0x040032F5 RID: 13045
		private int curSelStart;

		// Token: 0x040032F6 RID: 13046
		private int curSelEnd;

		// Token: 0x040032F7 RID: 13047
		private short curSelType;

		// Token: 0x040032F8 RID: 13048
		private object oleCallback;

		// Token: 0x040032F9 RID: 13049
		private static int[] shortcutsToDisable;

		// Token: 0x040032FA RID: 13050
		private static int richEditMajorVersion = 3;

		// Token: 0x040032FB RID: 13051
		private BitVector32 richTextBoxFlags = default(BitVector32);

		// Token: 0x040032FC RID: 13052
		private static readonly BitVector32.Section autoWordSelectionSection = BitVector32.CreateSection(1);

		// Token: 0x040032FD RID: 13053
		private static readonly BitVector32.Section showSelBarSection = BitVector32.CreateSection(1, RichTextBox.autoWordSelectionSection);

		// Token: 0x040032FE RID: 13054
		private static readonly BitVector32.Section autoUrlDetectSection = BitVector32.CreateSection(1, RichTextBox.showSelBarSection);

		// Token: 0x040032FF RID: 13055
		private static readonly BitVector32.Section fInCtorSection = BitVector32.CreateSection(1, RichTextBox.autoUrlDetectSection);

		// Token: 0x04003300 RID: 13056
		private static readonly BitVector32.Section protectedErrorSection = BitVector32.CreateSection(1, RichTextBox.fInCtorSection);

		// Token: 0x04003301 RID: 13057
		private static readonly BitVector32.Section linkcursorSection = BitVector32.CreateSection(1, RichTextBox.protectedErrorSection);

		// Token: 0x04003302 RID: 13058
		private static readonly BitVector32.Section allowOleDropSection = BitVector32.CreateSection(1, RichTextBox.linkcursorSection);

		// Token: 0x04003303 RID: 13059
		private static readonly BitVector32.Section suppressTextChangedEventSection = BitVector32.CreateSection(1, RichTextBox.allowOleDropSection);

		// Token: 0x04003304 RID: 13060
		private static readonly BitVector32.Section callOnContentsResizedSection = BitVector32.CreateSection(1, RichTextBox.suppressTextChangedEventSection);

		// Token: 0x04003305 RID: 13061
		private static readonly BitVector32.Section richTextShortcutsEnabledSection = BitVector32.CreateSection(1, RichTextBox.callOnContentsResizedSection);

		// Token: 0x04003306 RID: 13062
		private static readonly BitVector32.Section allowOleObjectsSection = BitVector32.CreateSection(1, RichTextBox.richTextShortcutsEnabledSection);

		// Token: 0x04003307 RID: 13063
		private static readonly BitVector32.Section scrollBarsSection = BitVector32.CreateSection(19, RichTextBox.allowOleObjectsSection);

		// Token: 0x04003308 RID: 13064
		private static readonly BitVector32.Section enableAutoDragDropSection = BitVector32.CreateSection(1, RichTextBox.scrollBarsSection);

		// Token: 0x020005E9 RID: 1513
		private class OleCallback : UnsafeNativeMethods.IRichEditOleCallback
		{
			// Token: 0x06004F86 RID: 20358 RVA: 0x00125FB3 File Offset: 0x00124FB3
			internal OleCallback(RichTextBox owner)
			{
				this.owner = owner;
			}

			// Token: 0x06004F87 RID: 20359 RVA: 0x00125FC4 File Offset: 0x00124FC4
			public int GetNewStorage(out UnsafeNativeMethods.IStorage storage)
			{
				if (!this.owner.AllowOleObjects)
				{
					storage = null;
					return -2147467259;
				}
				UnsafeNativeMethods.ILockBytes iLockBytes = UnsafeNativeMethods.CreateILockBytesOnHGlobal(NativeMethods.NullHandleRef, true);
				storage = UnsafeNativeMethods.StgCreateDocfileOnILockBytes(iLockBytes, 4114, 0);
				return 0;
			}

			// Token: 0x06004F88 RID: 20360 RVA: 0x00126002 File Offset: 0x00125002
			public int GetInPlaceContext(IntPtr lplpFrame, IntPtr lplpDoc, IntPtr lpFrameInfo)
			{
				return -2147467263;
			}

			// Token: 0x06004F89 RID: 20361 RVA: 0x00126009 File Offset: 0x00125009
			public int ShowContainerUI(int fShow)
			{
				return 0;
			}

			// Token: 0x06004F8A RID: 20362 RVA: 0x0012600C File Offset: 0x0012500C
			public int QueryInsertObject(ref Guid lpclsid, IntPtr lpstg, int cp)
			{
				try
				{
					IntSecurity.UnmanagedCode.Demand();
					return 0;
				}
				catch (SecurityException)
				{
				}
				Guid a = default(Guid);
				int hr = UnsafeNativeMethods.ReadClassStg(new HandleRef(null, lpstg), ref a);
				if (!NativeMethods.Succeeded(hr))
				{
					return 1;
				}
				if (a == Guid.Empty)
				{
					a = lpclsid;
				}
				string a2;
				if ((a2 = a.ToString().ToUpper(CultureInfo.InvariantCulture)) != null && (a2 == "00000315-0000-0000-C000-000000000046" || a2 == "00000316-0000-0000-C000-000000000046" || a2 == "00000319-0000-0000-C000-000000000046" || a2 == "0003000A-0000-0000-C000-000000000046"))
				{
					return 0;
				}
				return 1;
			}

			// Token: 0x06004F8B RID: 20363 RVA: 0x001260C8 File Offset: 0x001250C8
			public int DeleteObject(IntPtr lpoleobj)
			{
				return 0;
			}

			// Token: 0x06004F8C RID: 20364 RVA: 0x001260CC File Offset: 0x001250CC
			public int QueryAcceptData(IDataObject lpdataobj, IntPtr lpcfFormat, int reco, int fReally, IntPtr hMetaPict)
			{
				if (reco != 1)
				{
					return -2147467263;
				}
				if (!this.owner.AllowDrop && !this.owner.EnableAutoDragDrop)
				{
					this.lastDataObject = null;
					return -2147467259;
				}
				MouseButtons mouseButtons = Control.MouseButtons;
				Keys modifierKeys = Control.ModifierKeys;
				int num = 0;
				if ((mouseButtons & MouseButtons.Left) == MouseButtons.Left)
				{
					num |= 1;
				}
				if ((mouseButtons & MouseButtons.Right) == MouseButtons.Right)
				{
					num |= 2;
				}
				if ((mouseButtons & MouseButtons.Middle) == MouseButtons.Middle)
				{
					num |= 16;
				}
				if ((modifierKeys & Keys.Control) == Keys.Control)
				{
					num |= 8;
				}
				if ((modifierKeys & Keys.Shift) == Keys.Shift)
				{
					num |= 4;
				}
				this.lastDataObject = new DataObject(lpdataobj);
				if (!this.owner.EnableAutoDragDrop)
				{
					this.lastEffect = DragDropEffects.None;
				}
				DragEventArgs dragEventArgs = new DragEventArgs(this.lastDataObject, num, Control.MousePosition.X, Control.MousePosition.Y, DragDropEffects.All, this.lastEffect);
				if (fReally == 0)
				{
					dragEventArgs.Effect = (((num & 8) == 8) ? DragDropEffects.Copy : DragDropEffects.Move);
					this.owner.OnDragEnter(dragEventArgs);
				}
				else
				{
					this.owner.OnDragDrop(dragEventArgs);
					this.lastDataObject = null;
				}
				this.lastEffect = dragEventArgs.Effect;
				if (dragEventArgs.Effect == DragDropEffects.None)
				{
					return -2147467259;
				}
				return 0;
			}

			// Token: 0x06004F8D RID: 20365 RVA: 0x0012621D File Offset: 0x0012521D
			public int ContextSensitiveHelp(int fEnterMode)
			{
				return -2147467263;
			}

			// Token: 0x06004F8E RID: 20366 RVA: 0x00126224 File Offset: 0x00125224
			public int GetClipboardData(NativeMethods.CHARRANGE lpchrg, int reco, IntPtr lplpdataobj)
			{
				return -2147467263;
			}

			// Token: 0x06004F8F RID: 20367 RVA: 0x0012622C File Offset: 0x0012522C
			public int GetDragDropEffect(bool fDrag, int grfKeyState, ref int pdwEffect)
			{
				if (this.owner.AllowDrop || this.owner.EnableAutoDragDrop)
				{
					if (fDrag && grfKeyState == 0)
					{
						if (this.owner.EnableAutoDragDrop)
						{
							this.lastEffect = DragDropEffects.All;
						}
						else
						{
							this.lastEffect = DragDropEffects.None;
						}
					}
					else if (!fDrag && this.lastDataObject != null && grfKeyState != 0)
					{
						DragEventArgs dragEventArgs = new DragEventArgs(this.lastDataObject, grfKeyState, Control.MousePosition.X, Control.MousePosition.Y, DragDropEffects.All, this.lastEffect);
						if (this.lastEffect != DragDropEffects.None)
						{
							dragEventArgs.Effect = (((grfKeyState & 8) == 8) ? DragDropEffects.Copy : DragDropEffects.Move);
						}
						this.owner.OnDragOver(dragEventArgs);
						this.lastEffect = dragEventArgs.Effect;
					}
					pdwEffect = (int)this.lastEffect;
				}
				else
				{
					pdwEffect = 0;
				}
				return 0;
			}

			// Token: 0x06004F90 RID: 20368 RVA: 0x00126300 File Offset: 0x00125300
			public int GetContextMenu(short seltype, IntPtr lpoleobj, NativeMethods.CHARRANGE lpchrg, out IntPtr hmenu)
			{
				ContextMenu contextMenu = this.owner.ContextMenu;
				if (contextMenu == null || !this.owner.ShortcutsEnabled)
				{
					hmenu = IntPtr.Zero;
				}
				else
				{
					contextMenu.sourceControl = this.owner;
					contextMenu.OnPopup(EventArgs.Empty);
					IntPtr handle = contextMenu.Handle;
					Menu menu = contextMenu;
					for (;;)
					{
						int i = 0;
						int itemCount = menu.ItemCount;
						while (i < itemCount)
						{
							if (menu.items[i].handle != IntPtr.Zero)
							{
								menu = menu.items[i];
								break;
							}
							i++;
						}
						if (i == itemCount)
						{
							menu.handle = IntPtr.Zero;
							menu.created = false;
							if (menu == contextMenu)
							{
								break;
							}
							menu = ((MenuItem)menu).Menu;
						}
					}
					hmenu = handle;
				}
				return 0;
			}

			// Token: 0x04003309 RID: 13065
			private RichTextBox owner;

			// Token: 0x0400330A RID: 13066
			private IDataObject lastDataObject;

			// Token: 0x0400330B RID: 13067
			private DragDropEffects lastEffect;
		}
	}
}

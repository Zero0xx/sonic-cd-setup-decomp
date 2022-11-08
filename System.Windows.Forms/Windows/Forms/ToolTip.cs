using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020006EE RID: 1774
	[DefaultEvent("Popup")]
	[SRDescription("DescriptionToolTip")]
	[ProvideProperty("ToolTip", typeof(Control))]
	[ToolboxItemFilter("System.Windows.Forms")]
	public class ToolTip : Component, IExtenderProvider
	{
		// Token: 0x06005DDB RID: 24027 RVA: 0x001541C6 File Offset: 0x001531C6
		public ToolTip(IContainer cont) : this()
		{
			cont.Add(this);
		}

		// Token: 0x06005DDC RID: 24028 RVA: 0x001541D8 File Offset: 0x001531D8
		public ToolTip()
		{
			this.window = new ToolTip.ToolTipNativeWindow(this);
			this.auto = true;
			this.delayTimes[0] = 500;
			this.AdjustBaseFromAuto();
		}

		// Token: 0x170013C6 RID: 5062
		// (get) Token: 0x06005DDD RID: 24029 RVA: 0x0015427B File Offset: 0x0015327B
		// (set) Token: 0x06005DDE RID: 24030 RVA: 0x00154284 File Offset: 0x00153284
		[DefaultValue(true)]
		[SRDescription("ToolTipActiveDescr")]
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				if (this.active != value)
				{
					this.active = value;
					if (!base.DesignMode && this.GetHandleCreated())
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1025, value ? 1 : 0, 0);
					}
				}
			}
		}

		// Token: 0x170013C7 RID: 5063
		// (get) Token: 0x06005DDF RID: 24031 RVA: 0x001542D0 File Offset: 0x001532D0
		// (set) Token: 0x06005DE0 RID: 24032 RVA: 0x001542DC File Offset: 0x001532DC
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("ToolTipAutomaticDelayDescr")]
		[DefaultValue(500)]
		public int AutomaticDelay
		{
			get
			{
				return this.delayTimes[0];
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("AutomaticDelay", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"AutomaticDelay",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.SetDelayTime(0, value);
			}
		}

		// Token: 0x170013C8 RID: 5064
		// (get) Token: 0x06005DE1 RID: 24033 RVA: 0x0015433A File Offset: 0x0015333A
		// (set) Token: 0x06005DE2 RID: 24034 RVA: 0x00154344 File Offset: 0x00153344
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("ToolTipAutoPopDelayDescr")]
		public int AutoPopDelay
		{
			get
			{
				return this.delayTimes[2];
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("AutoPopDelay", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"AutoPopDelay",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.SetDelayTime(2, value);
			}
		}

		// Token: 0x170013C9 RID: 5065
		// (get) Token: 0x06005DE3 RID: 24035 RVA: 0x001543A2 File Offset: 0x001533A2
		// (set) Token: 0x06005DE4 RID: 24036 RVA: 0x001543AA File Offset: 0x001533AA
		[DefaultValue(typeof(Color), "Info")]
		[SRDescription("ToolTipBackColorDescr")]
		public Color BackColor
		{
			get
			{
				return this.backColor;
			}
			set
			{
				this.backColor = value;
				if (this.GetHandleCreated())
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1043, ColorTranslator.ToWin32(this.backColor), 0);
				}
			}
		}

		// Token: 0x170013CA RID: 5066
		// (get) Token: 0x06005DE5 RID: 24037 RVA: 0x001543E0 File Offset: 0x001533E0
		protected virtual CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = new CreateParams();
				if (this.TopLevelControl != null)
				{
					createParams.Parent = this.TopLevelControl.Handle;
				}
				createParams.ClassName = "tooltips_class32";
				if (this.showAlways)
				{
					createParams.Style = 1;
				}
				if (this.isBalloon)
				{
					createParams.Style |= 64;
				}
				if (!this.stripAmpersands)
				{
					createParams.Style |= 2;
				}
				if (!this.useAnimation)
				{
					createParams.Style |= 16;
				}
				if (!this.useFading)
				{
					createParams.Style |= 32;
				}
				createParams.ExStyle = 0;
				createParams.Caption = null;
				return createParams;
			}
		}

		// Token: 0x170013CB RID: 5067
		// (get) Token: 0x06005DE6 RID: 24038 RVA: 0x00154490 File Offset: 0x00153490
		// (set) Token: 0x06005DE7 RID: 24039 RVA: 0x00154498 File Offset: 0x00153498
		[SRDescription("ToolTipForeColorDescr")]
		[DefaultValue(typeof(Color), "InfoText")]
		public Color ForeColor
		{
			get
			{
				return this.foreColor;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("ToolTipEmptyColor", new object[]
					{
						"ForeColor"
					}));
				}
				this.foreColor = value;
				if (this.GetHandleCreated())
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1044, ColorTranslator.ToWin32(this.foreColor), 0);
				}
			}
		}

		// Token: 0x170013CC RID: 5068
		// (get) Token: 0x06005DE8 RID: 24040 RVA: 0x00154500 File Offset: 0x00153500
		internal IntPtr Handle
		{
			get
			{
				if (!this.GetHandleCreated())
				{
					this.CreateHandle();
				}
				return this.window.Handle;
			}
		}

		// Token: 0x170013CD RID: 5069
		// (get) Token: 0x06005DE9 RID: 24041 RVA: 0x0015451C File Offset: 0x0015351C
		private bool HasAllWindowsPermission
		{
			get
			{
				try
				{
					IntSecurity.AllWindows.Demand();
					return true;
				}
				catch (SecurityException)
				{
				}
				return false;
			}
		}

		// Token: 0x170013CE RID: 5070
		// (get) Token: 0x06005DEA RID: 24042 RVA: 0x00154550 File Offset: 0x00153550
		// (set) Token: 0x06005DEB RID: 24043 RVA: 0x00154558 File Offset: 0x00153558
		[DefaultValue(false)]
		[SRDescription("ToolTipIsBalloonDescr")]
		public bool IsBalloon
		{
			get
			{
				return this.isBalloon;
			}
			set
			{
				if (this.isBalloon != value)
				{
					this.isBalloon = value;
					if (this.GetHandleCreated())
					{
						this.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x06005DEC RID: 24044 RVA: 0x00154578 File Offset: 0x00153578
		private bool IsWindowActive(IWin32Window window)
		{
			Control control = window as Control;
			if ((control.ShowParams & 15) != 4)
			{
				IntPtr activeWindow = UnsafeNativeMethods.GetActiveWindow();
				IntPtr ancestor = UnsafeNativeMethods.GetAncestor(new HandleRef(window, window.Handle), 2);
				if (activeWindow != ancestor)
				{
					ToolTip.TipInfo tipInfo = (ToolTip.TipInfo)this.tools[control];
					if (tipInfo != null && (tipInfo.TipType & ToolTip.TipInfo.Type.SemiAbsolute) != ToolTip.TipInfo.Type.None)
					{
						this.tools.Remove(control);
						this.DestroyRegion(control);
					}
					return false;
				}
			}
			return true;
		}

		// Token: 0x170013CF RID: 5071
		// (get) Token: 0x06005DED RID: 24045 RVA: 0x001545EF File Offset: 0x001535EF
		// (set) Token: 0x06005DEE RID: 24046 RVA: 0x001545FC File Offset: 0x001535FC
		[SRDescription("ToolTipInitialDelayDescr")]
		[RefreshProperties(RefreshProperties.All)]
		public int InitialDelay
		{
			get
			{
				return this.delayTimes[3];
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("InitialDelay", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"InitialDelay",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.SetDelayTime(3, value);
			}
		}

		// Token: 0x170013D0 RID: 5072
		// (get) Token: 0x06005DEF RID: 24047 RVA: 0x0015465A File Offset: 0x0015365A
		// (set) Token: 0x06005DF0 RID: 24048 RVA: 0x00154662 File Offset: 0x00153662
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("ToolTipOwnerDrawDescr")]
		public bool OwnerDraw
		{
			get
			{
				return this.ownerDraw;
			}
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			set
			{
				this.ownerDraw = value;
			}
		}

		// Token: 0x170013D1 RID: 5073
		// (get) Token: 0x06005DF1 RID: 24049 RVA: 0x0015466B File Offset: 0x0015366B
		// (set) Token: 0x06005DF2 RID: 24050 RVA: 0x00154678 File Offset: 0x00153678
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("ToolTipReshowDelayDescr")]
		public int ReshowDelay
		{
			get
			{
				return this.delayTimes[1];
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("ReshowDelay", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"ReshowDelay",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.SetDelayTime(1, value);
			}
		}

		// Token: 0x170013D2 RID: 5074
		// (get) Token: 0x06005DF3 RID: 24051 RVA: 0x001546D6 File Offset: 0x001536D6
		// (set) Token: 0x06005DF4 RID: 24052 RVA: 0x001546DE File Offset: 0x001536DE
		[SRDescription("ToolTipShowAlwaysDescr")]
		[DefaultValue(false)]
		public bool ShowAlways
		{
			get
			{
				return this.showAlways;
			}
			set
			{
				if (this.showAlways != value)
				{
					this.showAlways = value;
					if (this.GetHandleCreated())
					{
						this.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x170013D3 RID: 5075
		// (get) Token: 0x06005DF5 RID: 24053 RVA: 0x001546FE File Offset: 0x001536FE
		// (set) Token: 0x06005DF6 RID: 24054 RVA: 0x00154706 File Offset: 0x00153706
		[SRDescription("ToolTipStripAmpersandsDescr")]
		[Browsable(true)]
		[DefaultValue(false)]
		public bool StripAmpersands
		{
			get
			{
				return this.stripAmpersands;
			}
			set
			{
				if (this.stripAmpersands != value)
				{
					this.stripAmpersands = value;
					if (this.GetHandleCreated())
					{
						this.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x170013D4 RID: 5076
		// (get) Token: 0x06005DF7 RID: 24055 RVA: 0x00154726 File Offset: 0x00153726
		// (set) Token: 0x06005DF8 RID: 24056 RVA: 0x0015472E File Offset: 0x0015372E
		[SRDescription("ControlTagDescr")]
		[Localizable(false)]
		[Bindable(true)]
		[SRCategory("CatData")]
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

		// Token: 0x170013D5 RID: 5077
		// (get) Token: 0x06005DF9 RID: 24057 RVA: 0x00154737 File Offset: 0x00153737
		// (set) Token: 0x06005DFA RID: 24058 RVA: 0x00154740 File Offset: 0x00153740
		[DefaultValue(ToolTipIcon.None)]
		[SRDescription("ToolTipToolTipIconDescr")]
		public ToolTipIcon ToolTipIcon
		{
			get
			{
				return this.toolTipIcon;
			}
			set
			{
				if (this.toolTipIcon != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolTipIcon));
					}
					this.toolTipIcon = value;
					if (this.toolTipIcon > ToolTipIcon.None && this.GetHandleCreated())
					{
						string lParam = (!string.IsNullOrEmpty(this.toolTipTitle)) ? this.toolTipTitle : " ";
						UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_SETTITLE, (int)this.toolTipIcon, lParam);
						UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1053, 0, 0);
					}
				}
			}
		}

		// Token: 0x170013D6 RID: 5078
		// (get) Token: 0x06005DFB RID: 24059 RVA: 0x001547E9 File Offset: 0x001537E9
		// (set) Token: 0x06005DFC RID: 24060 RVA: 0x001547F4 File Offset: 0x001537F4
		[DefaultValue("")]
		[SRDescription("ToolTipTitleDescr")]
		public string ToolTipTitle
		{
			get
			{
				return this.toolTipTitle;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (this.toolTipTitle != value)
				{
					this.toolTipTitle = value;
					if (this.GetHandleCreated())
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_SETTITLE, (int)this.toolTipIcon, this.toolTipTitle);
						UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1053, 0, 0);
					}
				}
			}
		}

		// Token: 0x170013D7 RID: 5079
		// (get) Token: 0x06005DFD RID: 24061 RVA: 0x00154864 File Offset: 0x00153864
		private Control TopLevelControl
		{
			get
			{
				Control control = null;
				if (this.topLevelControl == null)
				{
					Control[] array = new Control[this.tools.Keys.Count];
					this.tools.Keys.CopyTo(array, 0);
					if (array != null && array.Length > 0)
					{
						foreach (Control control2 in array)
						{
							control = control2.TopLevelControlInternal;
							if (control != null)
							{
								break;
							}
							if (control2.IsActiveX)
							{
								control = control2;
								break;
							}
							if (control == null && control2 != null && control2.ParentInternal != null)
							{
								while (control2.ParentInternal != null)
								{
									control2 = control2.ParentInternal;
								}
								control = control2;
								if (control != null)
								{
									break;
								}
							}
						}
					}
					this.topLevelControl = control;
					if (control != null)
					{
						control.HandleCreated += this.TopLevelCreated;
						control.HandleDestroyed += this.TopLevelDestroyed;
						if (control.IsHandleCreated)
						{
							this.TopLevelCreated(control, EventArgs.Empty);
						}
						control.ParentChanged += this.OnTopLevelPropertyChanged;
					}
				}
				else
				{
					control = this.topLevelControl;
				}
				return control;
			}
		}

		// Token: 0x170013D8 RID: 5080
		// (get) Token: 0x06005DFE RID: 24062 RVA: 0x00154959 File Offset: 0x00153959
		// (set) Token: 0x06005DFF RID: 24063 RVA: 0x00154961 File Offset: 0x00153961
		[DefaultValue(true)]
		[Browsable(true)]
		[SRDescription("ToolTipUseAnimationDescr")]
		public bool UseAnimation
		{
			get
			{
				return this.useAnimation;
			}
			set
			{
				if (this.useAnimation != value)
				{
					this.useAnimation = value;
					if (this.GetHandleCreated())
					{
						this.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x170013D9 RID: 5081
		// (get) Token: 0x06005E00 RID: 24064 RVA: 0x00154981 File Offset: 0x00153981
		// (set) Token: 0x06005E01 RID: 24065 RVA: 0x00154989 File Offset: 0x00153989
		[SRDescription("ToolTipUseFadingDescr")]
		[Browsable(true)]
		[DefaultValue(true)]
		public bool UseFading
		{
			get
			{
				return this.useFading;
			}
			set
			{
				if (this.useFading != value)
				{
					this.useFading = value;
					if (this.GetHandleCreated())
					{
						this.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x1400038B RID: 907
		// (add) Token: 0x06005E02 RID: 24066 RVA: 0x001549A9 File Offset: 0x001539A9
		// (remove) Token: 0x06005E03 RID: 24067 RVA: 0x001549C2 File Offset: 0x001539C2
		[SRDescription("ToolTipDrawEventDescr")]
		[SRCategory("CatBehavior")]
		public event DrawToolTipEventHandler Draw
		{
			add
			{
				this.onDraw = (DrawToolTipEventHandler)Delegate.Combine(this.onDraw, value);
			}
			remove
			{
				this.onDraw = (DrawToolTipEventHandler)Delegate.Remove(this.onDraw, value);
			}
		}

		// Token: 0x1400038C RID: 908
		// (add) Token: 0x06005E04 RID: 24068 RVA: 0x001549DB File Offset: 0x001539DB
		// (remove) Token: 0x06005E05 RID: 24069 RVA: 0x001549F4 File Offset: 0x001539F4
		[SRDescription("ToolTipPopupEventDescr")]
		[SRCategory("CatBehavior")]
		public event PopupEventHandler Popup
		{
			add
			{
				this.onPopup = (PopupEventHandler)Delegate.Combine(this.onPopup, value);
			}
			remove
			{
				this.onPopup = (PopupEventHandler)Delegate.Remove(this.onPopup, value);
			}
		}

		// Token: 0x06005E06 RID: 24070 RVA: 0x00154A0D File Offset: 0x00153A0D
		private void AdjustBaseFromAuto()
		{
			this.delayTimes[1] = this.delayTimes[0] / 5;
			this.delayTimes[2] = this.delayTimes[0] * 10;
			this.delayTimes[3] = this.delayTimes[0];
		}

		// Token: 0x06005E07 RID: 24071 RVA: 0x00154A44 File Offset: 0x00153A44
		private void HandleCreated(object sender, EventArgs eventargs)
		{
			this.ClearTopLevelControlEvents();
			this.topLevelControl = null;
			this.CreateRegion((Control)sender);
			this.CheckNativeToolTip((Control)sender);
			this.CheckCompositeControls((Control)sender);
		}

		// Token: 0x06005E08 RID: 24072 RVA: 0x00154A78 File Offset: 0x00153A78
		private void CheckNativeToolTip(Control associatedControl)
		{
			if (!this.GetHandleCreated())
			{
				return;
			}
			TreeView treeView = associatedControl as TreeView;
			if (treeView != null && treeView.ShowNodeToolTips)
			{
				treeView.SetToolTip(this, this.GetToolTip(associatedControl));
			}
			if (associatedControl is ToolBar)
			{
				((ToolBar)associatedControl).SetToolTip(this);
			}
			TabControl tabControl = associatedControl as TabControl;
			if (tabControl != null && tabControl.ShowToolTips)
			{
				tabControl.SetToolTip(this, this.GetToolTip(associatedControl));
			}
			if (associatedControl is ListView)
			{
				((ListView)associatedControl).SetToolTip(this, this.GetToolTip(associatedControl));
			}
			if (associatedControl is StatusBar)
			{
				((StatusBar)associatedControl).SetToolTip(this);
			}
			if (associatedControl is Label)
			{
				((Label)associatedControl).SetToolTip(this);
			}
		}

		// Token: 0x06005E09 RID: 24073 RVA: 0x00154B25 File Offset: 0x00153B25
		private void CheckCompositeControls(Control associatedControl)
		{
			if (associatedControl is UpDownBase)
			{
				((UpDownBase)associatedControl).SetToolTip(this, this.GetToolTip(associatedControl));
			}
		}

		// Token: 0x06005E0A RID: 24074 RVA: 0x00154B42 File Offset: 0x00153B42
		private void HandleDestroyed(object sender, EventArgs eventargs)
		{
			this.DestroyRegion((Control)sender);
		}

		// Token: 0x06005E0B RID: 24075 RVA: 0x00154B50 File Offset: 0x00153B50
		private void OnDraw(DrawToolTipEventArgs e)
		{
			if (this.onDraw != null)
			{
				this.onDraw(this, e);
			}
		}

		// Token: 0x06005E0C RID: 24076 RVA: 0x00154B67 File Offset: 0x00153B67
		private void OnPopup(PopupEventArgs e)
		{
			if (this.onPopup != null)
			{
				this.onPopup(this, e);
			}
		}

		// Token: 0x06005E0D RID: 24077 RVA: 0x00154B7E File Offset: 0x00153B7E
		private void TopLevelCreated(object sender, EventArgs eventargs)
		{
			this.CreateHandle();
			this.CreateAllRegions();
		}

		// Token: 0x06005E0E RID: 24078 RVA: 0x00154B8C File Offset: 0x00153B8C
		private void TopLevelDestroyed(object sender, EventArgs eventargs)
		{
			this.DestoyAllRegions();
			this.DestroyHandle();
		}

		// Token: 0x06005E0F RID: 24079 RVA: 0x00154B9A File Offset: 0x00153B9A
		public bool CanExtend(object target)
		{
			return target is Control && !(target is ToolTip);
		}

		// Token: 0x06005E10 RID: 24080 RVA: 0x00154BB0 File Offset: 0x00153BB0
		private void ClearTopLevelControlEvents()
		{
			if (this.topLevelControl != null)
			{
				this.topLevelControl.ParentChanged -= this.OnTopLevelPropertyChanged;
				this.topLevelControl.HandleCreated -= this.TopLevelCreated;
				this.topLevelControl.HandleDestroyed -= this.TopLevelDestroyed;
			}
		}

		// Token: 0x06005E11 RID: 24081 RVA: 0x00154C0C File Offset: 0x00153C0C
		private void CreateHandle()
		{
			if (this.GetHandleCreated())
			{
				return;
			}
			IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
			try
			{
				SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
				{
					dwICC = 8
				});
				CreateParams createParams = this.CreateParams;
				if (this.GetHandleCreated())
				{
					return;
				}
				this.window.CreateHandle(createParams);
			}
			finally
			{
				UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
			}
			if (this.ownerDraw)
			{
				int num = (int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this.Handle), -16));
				num &= -8388609;
				UnsafeNativeMethods.SetWindowLong(new HandleRef(this, this.Handle), -16, new HandleRef(null, (IntPtr)num));
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
			if (this.auto)
			{
				this.SetDelayTime(0, this.delayTimes[0]);
				this.delayTimes[2] = this.GetDelayTime(2);
				this.delayTimes[3] = this.GetDelayTime(3);
				this.delayTimes[1] = this.GetDelayTime(1);
			}
			else
			{
				for (int i = 1; i < this.delayTimes.Length; i++)
				{
					if (this.delayTimes[i] >= 1)
					{
						this.SetDelayTime(i, this.delayTimes[i]);
					}
				}
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1025, this.active ? 1 : 0, 0);
			if (this.BackColor != SystemColors.Info)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1043, ColorTranslator.ToWin32(this.BackColor), 0);
			}
			if (this.ForeColor != SystemColors.ControlText)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1044, ColorTranslator.ToWin32(this.ForeColor), 0);
			}
			if (this.toolTipIcon > ToolTipIcon.None || !string.IsNullOrEmpty(this.toolTipTitle))
			{
				string lParam = (!string.IsNullOrEmpty(this.toolTipTitle)) ? this.toolTipTitle : " ";
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_SETTITLE, (int)this.toolTipIcon, lParam);
			}
		}

		// Token: 0x06005E12 RID: 24082 RVA: 0x00154E40 File Offset: 0x00153E40
		private void CreateAllRegions()
		{
			Control[] array = new Control[this.tools.Keys.Count];
			this.tools.Keys.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] is DataGridView)
				{
					return;
				}
				this.CreateRegion(array[i]);
			}
		}

		// Token: 0x06005E13 RID: 24083 RVA: 0x00154E98 File Offset: 0x00153E98
		private void DestoyAllRegions()
		{
			Control[] array = new Control[this.tools.Keys.Count];
			this.tools.Keys.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] is DataGridView)
				{
					return;
				}
				this.DestroyRegion(array[i]);
			}
		}

		// Token: 0x06005E14 RID: 24084 RVA: 0x00154EF0 File Offset: 0x00153EF0
		private void SetToolInfo(Control ctl, string caption)
		{
			bool flag;
			NativeMethods.TOOLINFO_TOOLTIP toolinfo = this.GetTOOLINFO(ctl, caption, out flag);
			try
			{
				int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_ADDTOOL, 0, toolinfo);
				if (ctl is TreeView || ctl is ListView)
				{
					TreeView treeView = ctl as TreeView;
					if (treeView != null && treeView.ShowNodeToolTips)
					{
						return;
					}
					ListView listView = ctl as ListView;
					if (listView != null && listView.ShowItemToolTips)
					{
						return;
					}
				}
				if (num == 0)
				{
					throw new InvalidOperationException(SR.GetString("ToolTipAddFailed"));
				}
			}
			finally
			{
				if (flag && IntPtr.Zero != toolinfo.lpszText)
				{
					Marshal.FreeHGlobal(toolinfo.lpszText);
				}
			}
		}

		// Token: 0x06005E15 RID: 24085 RVA: 0x00154FA8 File Offset: 0x00153FA8
		private void CreateRegion(Control ctl)
		{
			string toolTip = this.GetToolTip(ctl);
			bool flag = toolTip != null && toolTip.Length > 0;
			bool flag2 = ctl.IsHandleCreated && this.TopLevelControl != null && this.TopLevelControl.IsHandleCreated;
			if (!this.created.ContainsKey(ctl) && flag && flag2 && !base.DesignMode)
			{
				this.SetToolInfo(ctl, toolTip);
				this.created[ctl] = ctl;
			}
			if (ctl.IsHandleCreated && this.topLevelControl == null)
			{
				ctl.MouseMove -= this.MouseMove;
				ctl.MouseMove += this.MouseMove;
			}
		}

		// Token: 0x06005E16 RID: 24086 RVA: 0x00155054 File Offset: 0x00154054
		private void MouseMove(object sender, MouseEventArgs me)
		{
			Control control = (Control)sender;
			if (!this.created.ContainsKey(control) && control.IsHandleCreated && this.TopLevelControl != null)
			{
				this.CreateRegion(control);
			}
			if (this.created.ContainsKey(control))
			{
				control.MouseMove -= this.MouseMove;
			}
		}

		// Token: 0x06005E17 RID: 24087 RVA: 0x001550AD File Offset: 0x001540AD
		internal void DestroyHandle()
		{
			if (this.GetHandleCreated())
			{
				this.window.DestroyHandle();
			}
		}

		// Token: 0x06005E18 RID: 24088 RVA: 0x001550C4 File Offset: 0x001540C4
		private void DestroyRegion(Control ctl)
		{
			bool flag = ctl.IsHandleCreated && this.topLevelControl != null && this.topLevelControl.IsHandleCreated && !this.isDisposing;
			Form form = this.topLevelControl as Form;
			if (form == null || (form != null && !form.Modal))
			{
				flag = (flag && this.GetHandleCreated());
			}
			if (this.created.ContainsKey(ctl) && flag && !base.DesignMode)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_DELTOOL, 0, this.GetMinTOOLINFO(ctl));
				this.created.Remove(ctl);
			}
		}

		// Token: 0x06005E19 RID: 24089 RVA: 0x00155168 File Offset: 0x00154168
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.isDisposing = true;
				try
				{
					this.ClearTopLevelControlEvents();
					this.StopTimer();
					this.DestroyHandle();
					this.RemoveAll();
					this.window = null;
					Form form = this.TopLevelControl as Form;
					if (form != null)
					{
						form.Deactivate -= this.BaseFormDeactivate;
					}
				}
				finally
				{
					this.isDisposing = false;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06005E1A RID: 24090 RVA: 0x001551E0 File Offset: 0x001541E0
		private int GetDelayTime(int type)
		{
			if (this.GetHandleCreated())
			{
				return (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1045, type, 0);
			}
			return this.delayTimes[type];
		}

		// Token: 0x06005E1B RID: 24091 RVA: 0x00155210 File Offset: 0x00154210
		internal bool GetHandleCreated()
		{
			return this.window != null && this.window.Handle != IntPtr.Zero;
		}

		// Token: 0x06005E1C RID: 24092 RVA: 0x00155234 File Offset: 0x00154234
		private NativeMethods.TOOLINFO_TOOLTIP GetMinTOOLINFO(Control ctl)
		{
			NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
			toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			toolinfo_TOOLTIP.hwnd = ctl.Handle;
			toolinfo_TOOLTIP.uFlags |= 1;
			toolinfo_TOOLTIP.uId = ctl.Handle;
			return toolinfo_TOOLTIP;
		}

		// Token: 0x06005E1D RID: 24093 RVA: 0x00155284 File Offset: 0x00154284
		private NativeMethods.TOOLINFO_TOOLTIP GetTOOLINFO(Control ctl, string caption, out bool allocatedString)
		{
			allocatedString = false;
			NativeMethods.TOOLINFO_TOOLTIP minTOOLINFO = this.GetMinTOOLINFO(ctl);
			minTOOLINFO.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			minTOOLINFO.uFlags |= 272;
			Control control = this.TopLevelControl;
			if (control != null && control.RightToLeft == RightToLeft.Yes && !ctl.IsMirrored)
			{
				minTOOLINFO.uFlags |= 4;
			}
			if (ctl is TreeView || ctl is ListView)
			{
				TreeView treeView = ctl as TreeView;
				if (treeView != null && treeView.ShowNodeToolTips)
				{
					minTOOLINFO.lpszText = NativeMethods.InvalidIntPtr;
				}
				else
				{
					ListView listView = ctl as ListView;
					if (listView != null && listView.ShowItemToolTips)
					{
						minTOOLINFO.lpszText = NativeMethods.InvalidIntPtr;
					}
					else
					{
						minTOOLINFO.lpszText = Marshal.StringToHGlobalAuto(caption);
						allocatedString = true;
					}
				}
			}
			else
			{
				minTOOLINFO.lpszText = Marshal.StringToHGlobalAuto(caption);
				allocatedString = true;
			}
			return minTOOLINFO;
		}

		// Token: 0x06005E1E RID: 24094 RVA: 0x0015535C File Offset: 0x0015435C
		private NativeMethods.TOOLINFO_TOOLTIP GetWinTOOLINFO(IntPtr hWnd)
		{
			NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
			toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			toolinfo_TOOLTIP.hwnd = hWnd;
			toolinfo_TOOLTIP.uFlags |= 273;
			Control control = this.TopLevelControl;
			if (control != null && control.RightToLeft == RightToLeft.Yes && ((int)((long)UnsafeNativeMethods.GetWindowLong(new HandleRef(this, hWnd), -16)) & 4194304) != 4194304)
			{
				toolinfo_TOOLTIP.uFlags |= 4;
			}
			toolinfo_TOOLTIP.uId = toolinfo_TOOLTIP.hwnd;
			return toolinfo_TOOLTIP;
		}

		// Token: 0x06005E1F RID: 24095 RVA: 0x001553F0 File Offset: 0x001543F0
		[DefaultValue("")]
		[Localizable(true)]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("ToolTipToolTipDescr")]
		public string GetToolTip(Control control)
		{
			if (control == null)
			{
				return string.Empty;
			}
			ToolTip.TipInfo tipInfo = (ToolTip.TipInfo)this.tools[control];
			if (tipInfo == null || tipInfo.Caption == null)
			{
				return "";
			}
			return tipInfo.Caption;
		}

		// Token: 0x06005E20 RID: 24096 RVA: 0x00155430 File Offset: 0x00154430
		private IntPtr GetWindowFromPoint(Point screenCoords, ref bool success)
		{
			Control control = this.TopLevelControl;
			if (control != null && control.IsActiveX)
			{
				IntPtr intPtr = UnsafeNativeMethods.WindowFromPoint(screenCoords.X, screenCoords.Y);
				if (intPtr != IntPtr.Zero)
				{
					Control control2 = Control.FromHandleInternal(intPtr);
					if (control2 != null && this.tools != null && this.tools.ContainsKey(control2))
					{
						return intPtr;
					}
				}
				return IntPtr.Zero;
			}
			IntPtr intPtr2 = IntPtr.Zero;
			if (control != null)
			{
				intPtr2 = control.Handle;
			}
			IntPtr intPtr3 = IntPtr.Zero;
			bool flag = false;
			while (!flag)
			{
				Point point = screenCoords;
				if (control != null)
				{
					point = control.PointToClientInternal(screenCoords);
				}
				IntPtr intPtr4 = UnsafeNativeMethods.ChildWindowFromPointEx(new HandleRef(null, intPtr2), point.X, point.Y, 1);
				if (intPtr4 == intPtr2)
				{
					intPtr3 = intPtr4;
					flag = true;
				}
				else if (intPtr4 == IntPtr.Zero)
				{
					flag = true;
				}
				else
				{
					control = Control.FromHandleInternal(intPtr4);
					if (control == null)
					{
						control = Control.FromChildHandleInternal(intPtr4);
						if (control != null)
						{
							intPtr3 = control.Handle;
						}
						flag = true;
					}
					else
					{
						intPtr2 = control.Handle;
					}
				}
			}
			if (intPtr3 != IntPtr.Zero)
			{
				Control control3 = Control.FromHandleInternal(intPtr3);
				if (control3 != null)
				{
					Control control4 = control3;
					while (control4 != null && control4.Visible)
					{
						control4 = control4.ParentInternal;
					}
					if (control4 != null)
					{
						intPtr3 = IntPtr.Zero;
					}
					success = true;
				}
			}
			return intPtr3;
		}

		// Token: 0x06005E21 RID: 24097 RVA: 0x0015557B File Offset: 0x0015457B
		private void OnTopLevelPropertyChanged(object s, EventArgs e)
		{
			this.ClearTopLevelControlEvents();
			this.topLevelControl = null;
			this.topLevelControl = this.TopLevelControl;
		}

		// Token: 0x06005E22 RID: 24098 RVA: 0x00155596 File Offset: 0x00154596
		private void RecreateHandle()
		{
			if (!base.DesignMode)
			{
				if (this.GetHandleCreated())
				{
					this.DestroyHandle();
				}
				this.created.Clear();
				this.CreateHandle();
				this.CreateAllRegions();
			}
		}

		// Token: 0x06005E23 RID: 24099 RVA: 0x001555C8 File Offset: 0x001545C8
		public void RemoveAll()
		{
			Control[] array = new Control[this.tools.Keys.Count];
			this.tools.Keys.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].IsHandleCreated)
				{
					this.DestroyRegion(array[i]);
				}
				array[i].HandleCreated -= this.HandleCreated;
				array[i].HandleDestroyed -= this.HandleDestroyed;
			}
			this.created.Clear();
			this.tools.Clear();
			this.ClearTopLevelControlEvents();
			this.topLevelControl = null;
		}

		// Token: 0x06005E24 RID: 24100 RVA: 0x0015566C File Offset: 0x0015466C
		private void SetDelayTime(int type, int time)
		{
			if (type == 0)
			{
				this.auto = true;
			}
			else
			{
				this.auto = false;
			}
			this.delayTimes[type] = time;
			if (this.GetHandleCreated() && time >= 0)
			{
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1027, type, time);
				if (this.auto)
				{
					this.delayTimes[2] = this.GetDelayTime(2);
					this.delayTimes[3] = this.GetDelayTime(3);
					this.delayTimes[1] = this.GetDelayTime(1);
					return;
				}
			}
			else if (this.auto)
			{
				this.AdjustBaseFromAuto();
			}
		}

		// Token: 0x06005E25 RID: 24101 RVA: 0x00155700 File Offset: 0x00154700
		public void SetToolTip(Control control, string caption)
		{
			ToolTip.TipInfo info = new ToolTip.TipInfo(caption, ToolTip.TipInfo.Type.Auto);
			this.SetToolTipInternal(control, info);
		}

		// Token: 0x06005E26 RID: 24102 RVA: 0x00155720 File Offset: 0x00154720
		private void SetToolTipInternal(Control control, ToolTip.TipInfo info)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			bool flag = false;
			bool flag2 = false;
			if (this.tools.ContainsKey(control))
			{
				flag = true;
			}
			if (info == null || string.IsNullOrEmpty(info.Caption))
			{
				flag2 = true;
			}
			if (flag && flag2)
			{
				this.tools.Remove(control);
			}
			else if (!flag2)
			{
				this.tools[control] = info;
			}
			if (!flag2 && !flag)
			{
				control.HandleCreated += this.HandleCreated;
				control.HandleDestroyed += this.HandleDestroyed;
				if (control.IsHandleCreated)
				{
					this.HandleCreated(control, EventArgs.Empty);
					return;
				}
			}
			else
			{
				bool flag3 = control.IsHandleCreated && this.TopLevelControl != null && this.TopLevelControl.IsHandleCreated;
				if (flag && !flag2 && flag3 && !base.DesignMode)
				{
					bool flag4;
					NativeMethods.TOOLINFO_TOOLTIP toolinfo = this.GetTOOLINFO(control, info.Caption, out flag4);
					try
					{
						UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_SETTOOLINFO, 0, toolinfo);
					}
					finally
					{
						if (flag4 && IntPtr.Zero != toolinfo.lpszText)
						{
							Marshal.FreeHGlobal(toolinfo.lpszText);
						}
					}
					this.CheckNativeToolTip(control);
					this.CheckCompositeControls(control);
					return;
				}
				if (flag2 && flag && !base.DesignMode)
				{
					control.HandleCreated -= this.HandleCreated;
					control.HandleDestroyed -= this.HandleDestroyed;
					if (control.IsHandleCreated)
					{
						this.HandleDestroyed(control, EventArgs.Empty);
					}
					this.created.Remove(control);
				}
			}
		}

		// Token: 0x06005E27 RID: 24103 RVA: 0x001558B8 File Offset: 0x001548B8
		private bool ShouldSerializeAutomaticDelay()
		{
			return this.auto && this.AutomaticDelay != 500;
		}

		// Token: 0x06005E28 RID: 24104 RVA: 0x001558D2 File Offset: 0x001548D2
		private bool ShouldSerializeAutoPopDelay()
		{
			return !this.auto;
		}

		// Token: 0x06005E29 RID: 24105 RVA: 0x001558DD File Offset: 0x001548DD
		private bool ShouldSerializeInitialDelay()
		{
			return !this.auto;
		}

		// Token: 0x06005E2A RID: 24106 RVA: 0x001558E8 File Offset: 0x001548E8
		private bool ShouldSerializeReshowDelay()
		{
			return !this.auto;
		}

		// Token: 0x06005E2B RID: 24107 RVA: 0x001558F4 File Offset: 0x001548F4
		private void ShowTooltip(string text, IWin32Window win, int duration)
		{
			if (win == null)
			{
				throw new ArgumentNullException("win");
			}
			Control control = win as Control;
			if (control != null)
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.GetWindowRect(new HandleRef(control, control.Handle), ref rect);
				Cursor currentInternal = Cursor.CurrentInternal;
				Point position = Cursor.Position;
				Point point = position;
				Screen screen = Screen.FromPoint(position);
				if (position.X < rect.left || position.X > rect.right || position.Y < rect.top || position.Y > rect.bottom)
				{
					NativeMethods.RECT rect2 = default(NativeMethods.RECT);
					rect2.left = ((rect.left < screen.WorkingArea.Left) ? screen.WorkingArea.Left : rect.left);
					rect2.top = ((rect.top < screen.WorkingArea.Top) ? screen.WorkingArea.Top : rect.top);
					rect2.right = ((rect.right > screen.WorkingArea.Right) ? screen.WorkingArea.Right : rect.right);
					rect2.bottom = ((rect.bottom > screen.WorkingArea.Bottom) ? screen.WorkingArea.Bottom : rect.bottom);
					point.X = rect2.left + (rect2.right - rect2.left) / 2;
					point.Y = rect2.top + (rect2.bottom - rect2.top) / 2;
					control.PointToClientInternal(point);
					this.SetTrackPosition(point.X, point.Y);
					this.SetTool(win, text, ToolTip.TipInfo.Type.SemiAbsolute, point);
					if (duration > 0)
					{
						this.StartTimer(this.window, duration);
						return;
					}
				}
				else
				{
					ToolTip.TipInfo tipInfo = (ToolTip.TipInfo)this.tools[control];
					if (tipInfo == null)
					{
						tipInfo = new ToolTip.TipInfo(text, ToolTip.TipInfo.Type.SemiAbsolute);
					}
					else
					{
						tipInfo.TipType |= ToolTip.TipInfo.Type.SemiAbsolute;
						tipInfo.Caption = text;
					}
					tipInfo.Position = point;
					if (duration > 0)
					{
						if (this.originalPopupDelay == 0)
						{
							this.originalPopupDelay = this.AutoPopDelay;
						}
						this.AutoPopDelay = duration;
					}
					this.SetToolTipInternal(control, tipInfo);
				}
			}
		}

		// Token: 0x06005E2C RID: 24108 RVA: 0x00155B5E File Offset: 0x00154B5E
		public void Show(string text, IWin32Window window)
		{
			if (this.HasAllWindowsPermission && this.IsWindowActive(window))
			{
				this.ShowTooltip(text, window, 0);
			}
		}

		// Token: 0x06005E2D RID: 24109 RVA: 0x00155B7C File Offset: 0x00154B7C
		public void Show(string text, IWin32Window window, int duration)
		{
			if (duration < 0)
			{
				throw new ArgumentOutOfRangeException("duration", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"duration",
					duration.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (this.HasAllWindowsPermission && this.IsWindowActive(window))
			{
				this.ShowTooltip(text, window, duration);
			}
		}

		// Token: 0x06005E2E RID: 24110 RVA: 0x00155BEC File Offset: 0x00154BEC
		public void Show(string text, IWin32Window window, Point point)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			if (this.HasAllWindowsPermission && this.IsWindowActive(window))
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.GetWindowRect(new HandleRef(window, Control.GetSafeHandle(window)), ref rect);
				int num = rect.left + point.X;
				int num2 = rect.top + point.Y;
				this.SetTrackPosition(num, num2);
				this.SetTool(window, text, ToolTip.TipInfo.Type.Absolute, new Point(num, num2));
			}
		}

		// Token: 0x06005E2F RID: 24111 RVA: 0x00155C6C File Offset: 0x00154C6C
		public void Show(string text, IWin32Window window, Point point, int duration)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			if (duration < 0)
			{
				throw new ArgumentOutOfRangeException("duration", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"duration",
					duration.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (this.HasAllWindowsPermission && this.IsWindowActive(window))
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.GetWindowRect(new HandleRef(window, Control.GetSafeHandle(window)), ref rect);
				int num = rect.left + point.X;
				int num2 = rect.top + point.Y;
				this.SetTrackPosition(num, num2);
				this.SetTool(window, text, ToolTip.TipInfo.Type.Absolute, new Point(num, num2));
				this.StartTimer(window, duration);
			}
		}

		// Token: 0x06005E30 RID: 24112 RVA: 0x00155D40 File Offset: 0x00154D40
		public void Show(string text, IWin32Window window, int x, int y)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			if (this.HasAllWindowsPermission && this.IsWindowActive(window))
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.GetWindowRect(new HandleRef(window, Control.GetSafeHandle(window)), ref rect);
				int num = rect.left + x;
				int num2 = rect.top + y;
				this.SetTrackPosition(num, num2);
				this.SetTool(window, text, ToolTip.TipInfo.Type.Absolute, new Point(num, num2));
			}
		}

		// Token: 0x06005E31 RID: 24113 RVA: 0x00155DB8 File Offset: 0x00154DB8
		public void Show(string text, IWin32Window window, int x, int y, int duration)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			if (duration < 0)
			{
				throw new ArgumentOutOfRangeException("duration", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"duration",
					duration.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (this.HasAllWindowsPermission && this.IsWindowActive(window))
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.GetWindowRect(new HandleRef(window, Control.GetSafeHandle(window)), ref rect);
				int num = rect.left + x;
				int num2 = rect.top + y;
				this.SetTrackPosition(num, num2);
				this.SetTool(window, text, ToolTip.TipInfo.Type.Absolute, new Point(num, num2));
				this.StartTimer(window, duration);
			}
		}

		// Token: 0x06005E32 RID: 24114 RVA: 0x00155E84 File Offset: 0x00154E84
		private void SetTrackPosition(int pointX, int pointY)
		{
			try
			{
				this.trackPosition = true;
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1042, 0, NativeMethods.Util.MAKELONG(pointX, pointY));
			}
			finally
			{
				this.trackPosition = false;
			}
		}

		// Token: 0x06005E33 RID: 24115 RVA: 0x00155ED4 File Offset: 0x00154ED4
		public void Hide(IWin32Window win)
		{
			if (win == null)
			{
				throw new ArgumentNullException("win");
			}
			if (this.HasAllWindowsPermission)
			{
				if (this.window == null)
				{
					return;
				}
				if (this.GetHandleCreated())
				{
					IntPtr safeHandle = Control.GetSafeHandle(win);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1041, 0, this.GetWinTOOLINFO(safeHandle));
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_DELTOOL, 0, this.GetWinTOOLINFO(safeHandle));
				}
				this.StopTimer();
				Control control = win as Control;
				if (control == null)
				{
					this.owners.Remove(win.Handle);
				}
				else
				{
					if (this.tools.ContainsKey(control))
					{
						this.SetToolInfo(control, this.GetToolTip(control));
					}
					else
					{
						this.owners.Remove(win.Handle);
					}
					Form form = control.FindFormInternal();
					if (form != null)
					{
						form.Deactivate -= this.BaseFormDeactivate;
					}
				}
				this.ClearTopLevelControlEvents();
				this.topLevelControl = null;
			}
		}

		// Token: 0x06005E34 RID: 24116 RVA: 0x00155FD5 File Offset: 0x00154FD5
		private void BaseFormDeactivate(object sender, EventArgs e)
		{
			this.HideAllToolTips();
		}

		// Token: 0x06005E35 RID: 24117 RVA: 0x00155FE0 File Offset: 0x00154FE0
		private void HideAllToolTips()
		{
			Control[] array = new Control[this.owners.Values.Count];
			this.owners.Values.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				this.Hide(array[i]);
			}
		}

		// Token: 0x06005E36 RID: 24118 RVA: 0x0015602C File Offset: 0x0015502C
		private void SetTool(IWin32Window win, string text, ToolTip.TipInfo.Type type, Point position)
		{
			Control control = win as Control;
			if (control != null && this.tools.ContainsKey(control))
			{
				bool flag = false;
				NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
				try
				{
					toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
					toolinfo_TOOLTIP.hwnd = control.Handle;
					toolinfo_TOOLTIP.uId = control.Handle;
					int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_GETTOOLINFO, 0, toolinfo_TOOLTIP);
					if (num != 0)
					{
						toolinfo_TOOLTIP.uFlags |= 32;
						if (type == ToolTip.TipInfo.Type.Absolute || type == ToolTip.TipInfo.Type.SemiAbsolute)
						{
							toolinfo_TOOLTIP.uFlags |= 128;
						}
						toolinfo_TOOLTIP.lpszText = Marshal.StringToHGlobalAuto(text);
						flag = true;
					}
					ToolTip.TipInfo tipInfo = (ToolTip.TipInfo)this.tools[control];
					if (tipInfo == null)
					{
						tipInfo = new ToolTip.TipInfo(text, type);
					}
					else
					{
						tipInfo.TipType |= type;
						tipInfo.Caption = text;
					}
					tipInfo.Position = position;
					this.tools[control] = tipInfo;
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_SETTOOLINFO, 0, toolinfo_TOOLTIP);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1041, 1, toolinfo_TOOLTIP);
					goto IL_25D;
				}
				finally
				{
					if (flag && IntPtr.Zero != toolinfo_TOOLTIP.lpszText)
					{
						Marshal.FreeHGlobal(toolinfo_TOOLTIP.lpszText);
					}
				}
			}
			this.Hide(win);
			ToolTip.TipInfo tipInfo2 = (ToolTip.TipInfo)this.tools[control];
			if (tipInfo2 == null)
			{
				tipInfo2 = new ToolTip.TipInfo(text, type);
			}
			else
			{
				tipInfo2.TipType |= type;
				tipInfo2.Caption = text;
			}
			tipInfo2.Position = position;
			this.tools[control] = tipInfo2;
			IntPtr safeHandle = Control.GetSafeHandle(win);
			this.owners[safeHandle] = win;
			NativeMethods.TOOLINFO_TOOLTIP winTOOLINFO = this.GetWinTOOLINFO(safeHandle);
			winTOOLINFO.uFlags |= 32;
			if (type == ToolTip.TipInfo.Type.Absolute || type == ToolTip.TipInfo.Type.SemiAbsolute)
			{
				winTOOLINFO.uFlags |= 128;
			}
			try
			{
				winTOOLINFO.lpszText = Marshal.StringToHGlobalAuto(text);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_ADDTOOL, 0, winTOOLINFO);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1041, 1, winTOOLINFO);
			}
			finally
			{
				if (IntPtr.Zero != winTOOLINFO.lpszText)
				{
					Marshal.FreeHGlobal(winTOOLINFO.lpszText);
				}
			}
			IL_25D:
			if (control != null)
			{
				Form form = control.FindFormInternal();
				if (form != null)
				{
					form.Deactivate += this.BaseFormDeactivate;
				}
			}
		}

		// Token: 0x06005E37 RID: 24119 RVA: 0x001562EC File Offset: 0x001552EC
		private void StartTimer(IWin32Window owner, int interval)
		{
			if (this.timer == null)
			{
				this.timer = new ToolTip.ToolTipTimer(owner);
				this.timer.Tick += this.TimerHandler;
			}
			this.timer.Interval = interval;
			this.timer.Start();
		}

		// Token: 0x06005E38 RID: 24120 RVA: 0x0015633C File Offset: 0x0015533C
		protected void StopTimer()
		{
			ToolTip.ToolTipTimer toolTipTimer = this.timer;
			if (toolTipTimer != null)
			{
				toolTipTimer.Stop();
				toolTipTimer.Dispose();
				this.timer = null;
			}
		}

		// Token: 0x06005E39 RID: 24121 RVA: 0x00156366 File Offset: 0x00155366
		private void TimerHandler(object source, EventArgs args)
		{
			this.Hide(((ToolTip.ToolTipTimer)source).Host);
		}

		// Token: 0x06005E3A RID: 24122 RVA: 0x0015637C File Offset: 0x0015537C
		~ToolTip()
		{
			this.DestroyHandle();
		}

		// Token: 0x06005E3B RID: 24123 RVA: 0x001563A8 File Offset: 0x001553A8
		public override string ToString()
		{
			string text = base.ToString();
			return string.Concat(new string[]
			{
				text,
				" InitialDelay: ",
				this.InitialDelay.ToString(CultureInfo.CurrentCulture),
				", ShowAlways: ",
				this.ShowAlways.ToString(CultureInfo.CurrentCulture)
			});
		}

		// Token: 0x06005E3C RID: 24124 RVA: 0x0015640C File Offset: 0x0015540C
		private void Reposition(Point tipPosition, Size tipSize)
		{
			Point point = tipPosition;
			Screen screen = Screen.FromPoint(point);
			if (point.X + tipSize.Width > screen.WorkingArea.Right)
			{
				point.X = screen.WorkingArea.Right - tipSize.Width;
			}
			if (point.Y + tipSize.Height > screen.WorkingArea.Bottom)
			{
				point.Y = screen.WorkingArea.Bottom - tipSize.Height;
			}
			SafeNativeMethods.SetWindowPos(new HandleRef(this, this.Handle), NativeMethods.HWND_TOPMOST, point.X, point.Y, tipSize.Width, tipSize.Height, 529);
		}

		// Token: 0x06005E3D RID: 24125 RVA: 0x001564D4 File Offset: 0x001554D4
		private void WmMove()
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetWindowRect(new HandleRef(this, this.Handle), ref rect);
			NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
			toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_GETCURRENTTOOL, 0, toolinfo_TOOLTIP);
			if (num != 0)
			{
				IWin32Window win32Window = (IWin32Window)this.owners[toolinfo_TOOLTIP.hwnd];
				if (win32Window == null)
				{
					win32Window = Control.FromHandleInternal(toolinfo_TOOLTIP.hwnd);
				}
				if (win32Window == null)
				{
					return;
				}
				ToolTip.TipInfo tipInfo = (ToolTip.TipInfo)this.tools[win32Window];
				if (win32Window == null || tipInfo == null)
				{
					return;
				}
				TreeView treeView = win32Window as TreeView;
				if (treeView != null && treeView.ShowNodeToolTips)
				{
					return;
				}
				if (tipInfo.Position != Point.Empty)
				{
					this.Reposition(tipInfo.Position, rect.Size);
				}
			}
		}

		// Token: 0x06005E3E RID: 24126 RVA: 0x001565C4 File Offset: 0x001555C4
		private void WmMouseActivate(ref Message msg)
		{
			NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
			toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_GETCURRENTTOOL, 0, toolinfo_TOOLTIP);
			if (num != 0)
			{
				IWin32Window win32Window = (IWin32Window)this.owners[toolinfo_TOOLTIP.hwnd];
				if (win32Window == null)
				{
					win32Window = Control.FromHandleInternal(toolinfo_TOOLTIP.hwnd);
				}
				if (win32Window == null)
				{
					return;
				}
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				UnsafeNativeMethods.GetWindowRect(new HandleRef(win32Window, Control.GetSafeHandle(win32Window)), ref rect);
				Point position = Cursor.Position;
				if (position.X >= rect.left && position.X <= rect.right && position.Y >= rect.top && position.Y <= rect.bottom)
				{
					msg.Result = (IntPtr)3;
				}
			}
		}

		// Token: 0x06005E3F RID: 24127 RVA: 0x001566B0 File Offset: 0x001556B0
		private void WmWindowFromPoint(ref Message msg)
		{
			NativeMethods.POINT point = (NativeMethods.POINT)msg.GetLParam(typeof(NativeMethods.POINT));
			Point screenCoords = new Point(point.x, point.y);
			bool flag = false;
			msg.Result = this.GetWindowFromPoint(screenCoords, ref flag);
		}

		// Token: 0x06005E40 RID: 24128 RVA: 0x001566F8 File Offset: 0x001556F8
		private void WmShow()
		{
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			UnsafeNativeMethods.GetWindowRect(new HandleRef(this, this.Handle), ref rect);
			NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
			toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_GETCURRENTTOOL, 0, toolinfo_TOOLTIP);
			if (num != 0)
			{
				IWin32Window win32Window = (IWin32Window)this.owners[toolinfo_TOOLTIP.hwnd];
				if (win32Window == null)
				{
					win32Window = Control.FromHandleInternal(toolinfo_TOOLTIP.hwnd);
				}
				if (win32Window == null)
				{
					return;
				}
				Control control = win32Window as Control;
				Size size = rect.Size;
				PopupEventArgs popupEventArgs = new PopupEventArgs(win32Window, control, this.IsBalloon, size);
				this.OnPopup(popupEventArgs);
				DataGridView dataGridView = control as DataGridView;
				if (dataGridView != null && dataGridView.CancelToolTipPopup(this))
				{
					popupEventArgs.Cancel = true;
				}
				UnsafeNativeMethods.GetWindowRect(new HandleRef(this, this.Handle), ref rect);
				size = ((popupEventArgs.ToolTipSize == size) ? rect.Size : popupEventArgs.ToolTipSize);
				if (this.IsBalloon)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1055, 1, ref rect);
					if (rect.Size.Height > size.Height)
					{
						size.Height = rect.Size.Height;
					}
				}
				if (size != rect.Size)
				{
					Screen screen = Screen.FromPoint(Cursor.Position);
					int lParam = this.IsBalloon ? Math.Min(size.Width - 20, screen.WorkingArea.Width) : Math.Min(size.Width, screen.WorkingArea.Width);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1048, 0, lParam);
				}
				if (popupEventArgs.Cancel)
				{
					this.cancelled = true;
					SafeNativeMethods.SetWindowPos(new HandleRef(this, this.Handle), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, 528);
					return;
				}
				this.cancelled = false;
				SafeNativeMethods.SetWindowPos(new HandleRef(this, this.Handle), NativeMethods.HWND_TOPMOST, rect.left, rect.top, size.Width, size.Height, 528);
			}
		}

		// Token: 0x06005E41 RID: 24129 RVA: 0x0015694E File Offset: 0x0015594E
		private bool WmWindowPosChanged()
		{
			if (this.cancelled)
			{
				SafeNativeMethods.ShowWindow(new HandleRef(this, this.Handle), 0);
				return true;
			}
			return false;
		}

		// Token: 0x06005E42 RID: 24130 RVA: 0x00156970 File Offset: 0x00155970
		private unsafe void WmWindowPosChanging(ref Message m)
		{
			if (this.cancelled)
			{
				return;
			}
			NativeMethods.WINDOWPOS* ptr = (NativeMethods.WINDOWPOS*)((void*)m.LParam);
			Cursor currentInternal = Cursor.CurrentInternal;
			Point position = Cursor.Position;
			NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
			toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_GETCURRENTTOOL, 0, toolinfo_TOOLTIP);
			if (num != 0)
			{
				IWin32Window win32Window = (IWin32Window)this.owners[toolinfo_TOOLTIP.hwnd];
				if (win32Window == null)
				{
					win32Window = Control.FromHandleInternal(toolinfo_TOOLTIP.hwnd);
				}
				if (win32Window == null || !this.IsWindowActive(win32Window))
				{
					return;
				}
				ToolTip.TipInfo tipInfo = null;
				if (win32Window != null)
				{
					tipInfo = (ToolTip.TipInfo)this.tools[win32Window];
					if (tipInfo == null)
					{
						return;
					}
					TreeView treeView = win32Window as TreeView;
					if (treeView != null && treeView.ShowNodeToolTips)
					{
						return;
					}
				}
				if (this.IsBalloon)
				{
					ptr->cx += 20;
					return;
				}
				if ((tipInfo.TipType & ToolTip.TipInfo.Type.Auto) != ToolTip.TipInfo.Type.None)
				{
					this.window.DefWndProc(ref m);
					return;
				}
				if ((tipInfo.TipType & ToolTip.TipInfo.Type.SemiAbsolute) != ToolTip.TipInfo.Type.None && tipInfo.Position == Point.Empty)
				{
					Screen screen = Screen.FromPoint(position);
					if (currentInternal != null)
					{
						ptr->x = position.X;
						try
						{
							IntSecurity.ObjectFromWin32Handle.Assert();
							ptr->y = position.Y;
							if (ptr->y + ptr->cy + currentInternal.Size.Height - currentInternal.HotSpot.Y > screen.WorkingArea.Bottom)
							{
								ptr->y = position.Y - ptr->cy;
							}
							else
							{
								ptr->y = position.Y + currentInternal.Size.Height - currentInternal.HotSpot.Y;
							}
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					if (ptr->x + ptr->cx > screen.WorkingArea.Right)
					{
						ptr->x = screen.WorkingArea.Right - ptr->cx;
					}
				}
				else if ((tipInfo.TipType & ToolTip.TipInfo.Type.SemiAbsolute) != ToolTip.TipInfo.Type.None && tipInfo.Position != Point.Empty)
				{
					Screen screen2 = Screen.FromPoint(tipInfo.Position);
					ptr->x = tipInfo.Position.X;
					if (ptr->x + ptr->cx > screen2.WorkingArea.Right)
					{
						ptr->x = screen2.WorkingArea.Right - ptr->cx;
					}
					ptr->y = tipInfo.Position.Y;
					if (ptr->y + ptr->cy > screen2.WorkingArea.Bottom)
					{
						ptr->y = screen2.WorkingArea.Bottom - ptr->cy;
					}
				}
			}
			m.Result = IntPtr.Zero;
		}

		// Token: 0x06005E43 RID: 24131 RVA: 0x00156C98 File Offset: 0x00155C98
		private void WmPop()
		{
			NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
			toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_GETCURRENTTOOL, 0, toolinfo_TOOLTIP);
			if (num != 0)
			{
				IWin32Window win32Window = (IWin32Window)this.owners[toolinfo_TOOLTIP.hwnd];
				if (win32Window == null)
				{
					win32Window = Control.FromHandleInternal(toolinfo_TOOLTIP.hwnd);
				}
				if (win32Window == null)
				{
					return;
				}
				Control control = win32Window as Control;
				ToolTip.TipInfo tipInfo = (ToolTip.TipInfo)this.tools[win32Window];
				if (tipInfo == null)
				{
					return;
				}
				if ((tipInfo.TipType & ToolTip.TipInfo.Type.Auto) != ToolTip.TipInfo.Type.None || (tipInfo.TipType & ToolTip.TipInfo.Type.SemiAbsolute) != ToolTip.TipInfo.Type.None)
				{
					Screen screen = Screen.FromPoint(Cursor.Position);
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1048, 0, screen.WorkingArea.Width);
				}
				if ((tipInfo.TipType & ToolTip.TipInfo.Type.Auto) == ToolTip.TipInfo.Type.None)
				{
					this.tools.Remove(control);
					this.owners.Remove(win32Window.Handle);
					control.HandleCreated -= this.HandleCreated;
					control.HandleDestroyed -= this.HandleDestroyed;
					this.created.Remove(control);
					if (this.originalPopupDelay != 0)
					{
						this.AutoPopDelay = this.originalPopupDelay;
						this.originalPopupDelay = 0;
						return;
					}
				}
				else
				{
					tipInfo.TipType = ToolTip.TipInfo.Type.Auto;
					tipInfo.Position = Point.Empty;
					this.tools[control] = tipInfo;
				}
			}
		}

		// Token: 0x06005E44 RID: 24132 RVA: 0x00156E18 File Offset: 0x00155E18
		private void WndProc(ref Message msg)
		{
			int msg2 = msg.Msg;
			if (msg2 <= 33)
			{
				if (msg2 == 3)
				{
					this.WmMove();
					return;
				}
				if (msg2 != 15)
				{
					if (msg2 != 33)
					{
						goto IL_282;
					}
					this.WmMouseActivate(ref msg);
					return;
				}
			}
			else if (msg2 <= 792)
			{
				switch (msg2)
				{
				case 70:
					this.WmWindowPosChanging(ref msg);
					return;
				case 71:
					if (!this.WmWindowPosChanged())
					{
						this.window.DefWndProc(ref msg);
						return;
					}
					return;
				default:
					if (msg2 != 792)
					{
						goto IL_282;
					}
					break;
				}
			}
			else
			{
				if (msg2 == 1040)
				{
					this.WmWindowFromPoint(ref msg);
					return;
				}
				if (msg2 != 8270)
				{
					goto IL_282;
				}
				NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)msg.GetLParam(typeof(NativeMethods.NMHDR));
				if (nmhdr.code == -521 && !this.trackPosition)
				{
					this.WmShow();
					return;
				}
				if (nmhdr.code == -522)
				{
					this.WmPop();
					this.window.DefWndProc(ref msg);
					return;
				}
				return;
			}
			if (this.ownerDraw && !this.isBalloon && !this.trackPosition)
			{
				NativeMethods.PAINTSTRUCT paintstruct = default(NativeMethods.PAINTSTRUCT);
				IntPtr hdc = UnsafeNativeMethods.BeginPaint(new HandleRef(this, this.Handle), ref paintstruct);
				Graphics graphics = Graphics.FromHdcInternal(hdc);
				try
				{
					Rectangle rectangle = new Rectangle(paintstruct.rcPaint_left, paintstruct.rcPaint_top, paintstruct.rcPaint_right - paintstruct.rcPaint_left, paintstruct.rcPaint_bottom - paintstruct.rcPaint_top);
					if (rectangle == Rectangle.Empty)
					{
						return;
					}
					NativeMethods.TOOLINFO_TOOLTIP toolinfo_TOOLTIP = new NativeMethods.TOOLINFO_TOOLTIP();
					toolinfo_TOOLTIP.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_TOOLTIP));
					int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_GETCURRENTTOOL, 0, toolinfo_TOOLTIP);
					if (num != 0)
					{
						IWin32Window win32Window = (IWin32Window)this.owners[toolinfo_TOOLTIP.hwnd];
						Control control = Control.FromHandleInternal(toolinfo_TOOLTIP.hwnd);
						if (win32Window == null)
						{
							win32Window = control;
						}
						IntSecurity.ObjectFromWin32Handle.Assert();
						Font font;
						try
						{
							font = Font.FromHfont(UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 49, 0, 0));
						}
						catch (ArgumentException)
						{
							font = Control.DefaultFont;
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						this.OnDraw(new DrawToolTipEventArgs(graphics, win32Window, control, rectangle, this.GetToolTip(control), this.BackColor, this.ForeColor, font));
						return;
					}
				}
				finally
				{
					graphics.Dispose();
					UnsafeNativeMethods.EndPaint(new HandleRef(this, this.Handle), ref paintstruct);
				}
			}
			IL_282:
			this.window.DefWndProc(ref msg);
		}

		// Token: 0x04003964 RID: 14692
		private const int DEFAULT_DELAY = 500;

		// Token: 0x04003965 RID: 14693
		private const int RESHOW_RATIO = 5;

		// Token: 0x04003966 RID: 14694
		private const int AUTOPOP_RATIO = 10;

		// Token: 0x04003967 RID: 14695
		private const int XBALLOONOFFSET = 10;

		// Token: 0x04003968 RID: 14696
		private const int YBALLOONOFFSET = 8;

		// Token: 0x04003969 RID: 14697
		private Hashtable tools = new Hashtable();

		// Token: 0x0400396A RID: 14698
		private int[] delayTimes = new int[4];

		// Token: 0x0400396B RID: 14699
		private bool auto = true;

		// Token: 0x0400396C RID: 14700
		private bool showAlways;

		// Token: 0x0400396D RID: 14701
		private ToolTip.ToolTipNativeWindow window;

		// Token: 0x0400396E RID: 14702
		private Control topLevelControl;

		// Token: 0x0400396F RID: 14703
		private bool active = true;

		// Token: 0x04003970 RID: 14704
		private bool ownerDraw;

		// Token: 0x04003971 RID: 14705
		private object userData;

		// Token: 0x04003972 RID: 14706
		private Color backColor = SystemColors.Info;

		// Token: 0x04003973 RID: 14707
		private Color foreColor = SystemColors.InfoText;

		// Token: 0x04003974 RID: 14708
		private bool isBalloon;

		// Token: 0x04003975 RID: 14709
		private bool isDisposing;

		// Token: 0x04003976 RID: 14710
		private string toolTipTitle = string.Empty;

		// Token: 0x04003977 RID: 14711
		private ToolTipIcon toolTipIcon;

		// Token: 0x04003978 RID: 14712
		private ToolTip.ToolTipTimer timer;

		// Token: 0x04003979 RID: 14713
		private Hashtable owners = new Hashtable();

		// Token: 0x0400397A RID: 14714
		private bool stripAmpersands;

		// Token: 0x0400397B RID: 14715
		private bool useAnimation = true;

		// Token: 0x0400397C RID: 14716
		private bool useFading = true;

		// Token: 0x0400397D RID: 14717
		private int originalPopupDelay;

		// Token: 0x0400397E RID: 14718
		private bool trackPosition;

		// Token: 0x0400397F RID: 14719
		private PopupEventHandler onPopup;

		// Token: 0x04003980 RID: 14720
		private DrawToolTipEventHandler onDraw;

		// Token: 0x04003981 RID: 14721
		private Hashtable created = new Hashtable();

		// Token: 0x04003982 RID: 14722
		private bool cancelled;

		// Token: 0x020006EF RID: 1775
		private class ToolTipNativeWindow : NativeWindow
		{
			// Token: 0x06005E45 RID: 24133 RVA: 0x00157100 File Offset: 0x00156100
			internal ToolTipNativeWindow(ToolTip control)
			{
				this.control = control;
			}

			// Token: 0x06005E46 RID: 24134 RVA: 0x0015710F File Offset: 0x0015610F
			protected override void WndProc(ref Message m)
			{
				if (this.control != null)
				{
					this.control.WndProc(ref m);
				}
			}

			// Token: 0x04003983 RID: 14723
			private ToolTip control;
		}

		// Token: 0x020006F0 RID: 1776
		private class ToolTipTimer : Timer
		{
			// Token: 0x06005E47 RID: 24135 RVA: 0x00157125 File Offset: 0x00156125
			public ToolTipTimer(IWin32Window owner)
			{
				this.host = owner;
			}

			// Token: 0x170013DA RID: 5082
			// (get) Token: 0x06005E48 RID: 24136 RVA: 0x00157134 File Offset: 0x00156134
			public IWin32Window Host
			{
				get
				{
					return this.host;
				}
			}

			// Token: 0x04003984 RID: 14724
			private IWin32Window host;
		}

		// Token: 0x020006F1 RID: 1777
		private class TipInfo
		{
			// Token: 0x06005E49 RID: 24137 RVA: 0x0015713C File Offset: 0x0015613C
			public TipInfo(string caption, ToolTip.TipInfo.Type type)
			{
				this.caption = caption;
				this.TipType = type;
				if (type == ToolTip.TipInfo.Type.Auto)
				{
					this.designerText = caption;
				}
			}

			// Token: 0x170013DB RID: 5083
			// (get) Token: 0x06005E4A RID: 24138 RVA: 0x0015716F File Offset: 0x0015616F
			// (set) Token: 0x06005E4B RID: 24139 RVA: 0x00157188 File Offset: 0x00156188
			public string Caption
			{
				get
				{
					if ((this.TipType & (ToolTip.TipInfo.Type.Absolute | ToolTip.TipInfo.Type.SemiAbsolute)) == ToolTip.TipInfo.Type.None)
					{
						return this.designerText;
					}
					return this.caption;
				}
				set
				{
					this.caption = value;
				}
			}

			// Token: 0x04003985 RID: 14725
			public ToolTip.TipInfo.Type TipType = ToolTip.TipInfo.Type.Auto;

			// Token: 0x04003986 RID: 14726
			private string caption;

			// Token: 0x04003987 RID: 14727
			private string designerText;

			// Token: 0x04003988 RID: 14728
			public Point Position = Point.Empty;

			// Token: 0x020006F2 RID: 1778
			[Flags]
			public enum Type
			{
				// Token: 0x0400398A RID: 14730
				None = 0,
				// Token: 0x0400398B RID: 14731
				Auto = 1,
				// Token: 0x0400398C RID: 14732
				Absolute = 2,
				// Token: 0x0400398D RID: 14733
				SemiAbsolute = 4
			}
		}
	}
}

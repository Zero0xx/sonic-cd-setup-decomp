using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Windows.Forms
{
	// Token: 0x02000724 RID: 1828
	[ComVisible(true)]
	[Designer("System.Windows.Forms.Design.AxDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Name")]
	[DefaultEvent("Enter")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class WebBrowserBase : Control
	{
		// Token: 0x060060D5 RID: 24789 RVA: 0x001627E8 File Offset: 0x001617E8
		internal WebBrowserBase(string clsidString)
		{
			if (Application.OleRequired() != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.GetString("AXMTAThread", new object[]
				{
					clsidString
				}));
			}
			base.SetStyle(ControlStyles.UserPaint, false);
			this.clsid = new Guid(clsidString);
			this.webBrowserBaseChangingSize.Width = -1;
			this.SetAXHostState(WebBrowserHelper.isMaskEdit, this.clsid.Equals(WebBrowserHelper.maskEdit_Clsid));
		}

		// Token: 0x1700147E RID: 5246
		// (get) Token: 0x060060D6 RID: 24790 RVA: 0x0016287B File Offset: 0x0016187B
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object ActiveXInstance
		{
			get
			{
				return this.activeXInstance;
			}
		}

		// Token: 0x060060D7 RID: 24791 RVA: 0x00162883 File Offset: 0x00161883
		protected virtual WebBrowserSiteBase CreateWebBrowserSiteBase()
		{
			return new WebBrowserSiteBase(this);
		}

		// Token: 0x060060D8 RID: 24792 RVA: 0x0016288B File Offset: 0x0016188B
		protected virtual void AttachInterfaces(object nativeActiveXObject)
		{
		}

		// Token: 0x060060D9 RID: 24793 RVA: 0x0016288D File Offset: 0x0016188D
		protected virtual void DetachInterfaces()
		{
		}

		// Token: 0x060060DA RID: 24794 RVA: 0x0016288F File Offset: 0x0016188F
		protected virtual void CreateSink()
		{
		}

		// Token: 0x060060DB RID: 24795 RVA: 0x00162891 File Offset: 0x00161891
		protected virtual void DetachSink()
		{
		}

		// Token: 0x060060DC RID: 24796 RVA: 0x00162893 File Offset: 0x00161893
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds)
		{
			base.DrawToBitmap(bitmap, targetBounds);
		}

		// Token: 0x1700147F RID: 5247
		// (set) Token: 0x060060DD RID: 24797 RVA: 0x001628A0 File Offset: 0x001618A0
		public override ISite Site
		{
			set
			{
				bool flag = this.RemoveSelectionHandler();
				base.Site = value;
				if (flag)
				{
					this.AddSelectionHandler();
				}
			}
		}

		// Token: 0x060060DE RID: 24798 RVA: 0x001628C4 File Offset: 0x001618C4
		internal override void OnBoundsUpdate(int x, int y, int width, int height)
		{
			if (this.ActiveXState >= WebBrowserHelper.AXState.InPlaceActive)
			{
				try
				{
					this.webBrowserBaseChangingSize.Width = width;
					this.webBrowserBaseChangingSize.Height = height;
					this.AXInPlaceObject.SetObjectRects(new NativeMethods.COMRECT(new Rectangle(0, 0, width, height)), WebBrowserHelper.GetClipRect());
				}
				finally
				{
					this.webBrowserBaseChangingSize.Width = -1;
				}
			}
			base.OnBoundsUpdate(x, y, width, height);
		}

		// Token: 0x060060DF RID: 24799 RVA: 0x0016293C File Offset: 0x0016193C
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			return !this.ignoreDialogKeys && base.ProcessDialogKey(keyData);
		}

		// Token: 0x060060E0 RID: 24800 RVA: 0x00162950 File Offset: 0x00161950
		public override bool PreProcessMessage(ref Message msg)
		{
			if (this.IsUserMode)
			{
				if (this.GetAXHostState(WebBrowserHelper.siteProcessedInputKey))
				{
					return base.PreProcessMessage(ref msg);
				}
				NativeMethods.MSG msg2 = default(NativeMethods.MSG);
				msg2.message = msg.Msg;
				msg2.wParam = msg.WParam;
				msg2.lParam = msg.LParam;
				msg2.hwnd = msg.HWnd;
				this.SetAXHostState(WebBrowserHelper.siteProcessedInputKey, false);
				try
				{
					if (this.axOleInPlaceObject != null)
					{
						int num = this.axOleInPlaceActiveObject.TranslateAccelerator(ref msg2);
						if (num == 0)
						{
							return true;
						}
						msg.Msg = msg2.message;
						msg.WParam = msg2.wParam;
						msg.LParam = msg2.lParam;
						msg.HWnd = msg2.hwnd;
						if (num == 1)
						{
							bool result = false;
							this.ignoreDialogKeys = true;
							try
							{
								result = base.PreProcessMessage(ref msg);
							}
							finally
							{
								this.ignoreDialogKeys = false;
							}
							return result;
						}
						if (this.GetAXHostState(WebBrowserHelper.siteProcessedInputKey))
						{
							return base.PreProcessMessage(ref msg);
						}
						return false;
					}
				}
				finally
				{
					this.SetAXHostState(WebBrowserHelper.siteProcessedInputKey, false);
				}
				return false;
			}
			return false;
		}

		// Token: 0x060060E1 RID: 24801 RVA: 0x00162A88 File Offset: 0x00161A88
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			bool result = false;
			if (base.CanSelect)
			{
				try
				{
					NativeMethods.tagCONTROLINFO tagCONTROLINFO = new NativeMethods.tagCONTROLINFO();
					int controlInfo = this.axOleControl.GetControlInfo(tagCONTROLINFO);
					if (NativeMethods.Succeeded(controlInfo))
					{
						NativeMethods.MSG msg = default(NativeMethods.MSG);
						msg.hwnd = IntPtr.Zero;
						msg.message = 260;
						msg.wParam = (IntPtr)((int)char.ToUpper(charCode, CultureInfo.CurrentCulture));
						msg.lParam = (IntPtr)538443777;
						msg.time = SafeNativeMethods.GetTickCount();
						NativeMethods.POINT point = new NativeMethods.POINT();
						UnsafeNativeMethods.GetCursorPos(point);
						msg.pt_x = point.x;
						msg.pt_y = point.y;
						if (SafeNativeMethods.IsAccelerator(new HandleRef(tagCONTROLINFO, tagCONTROLINFO.hAccel), (int)tagCONTROLINFO.cAccel, ref msg, null))
						{
							this.axOleControl.OnMnemonic(ref msg);
							this.FocusInternal();
							result = true;
						}
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
				}
			}
			return result;
		}

		// Token: 0x060060E2 RID: 24802 RVA: 0x00162B94 File Offset: 0x00161B94
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 43)
			{
				if (msg <= 8)
				{
					if (msg != 2)
					{
						if (msg != 8)
						{
							goto IL_18F;
						}
						this.hwndFocus = m.WParam;
						try
						{
							base.WndProc(ref m);
							return;
						}
						finally
						{
							this.hwndFocus = IntPtr.Zero;
						}
					}
					IntPtr handle;
					if (this.ActiveXState >= WebBrowserHelper.AXState.InPlaceActive && NativeMethods.Succeeded(this.AXInPlaceObject.GetWindow(out handle)))
					{
						Application.ParkHandle(new HandleRef(this.AXInPlaceObject, handle));
					}
					if (base.RecreatingHandle)
					{
						this.axReloadingState = this.axState;
					}
					this.TransitionDownTo(WebBrowserHelper.AXState.Running);
					if (this.axWindow != null)
					{
						this.axWindow.ReleaseHandle();
					}
					this.OnHandleDestroyed(EventArgs.Empty);
					return;
				}
				switch (msg)
				{
				case 20:
				case 21:
					break;
				default:
					switch (msg)
					{
					case 32:
						break;
					case 33:
						goto IL_D6;
					default:
						if (msg != 43)
						{
							goto IL_18F;
						}
						break;
					}
					break;
				}
			}
			else if (msg <= 123)
			{
				if (msg == 83)
				{
					base.WndProc(ref m);
					this.DefWndProc(ref m);
					return;
				}
				if (msg != 123)
				{
					goto IL_18F;
				}
			}
			else if (msg != 273)
			{
				switch (msg)
				{
				case 513:
				case 516:
				case 519:
					goto IL_D6;
				case 514:
				case 515:
				case 517:
				case 518:
				case 520:
				case 521:
					break;
				default:
					if (msg != 8277)
					{
						goto IL_18F;
					}
					break;
				}
			}
			else
			{
				if (!Control.ReflectMessageInternal(m.LParam, ref m))
				{
					this.DefWndProc(ref m);
					return;
				}
				return;
			}
			this.DefWndProc(ref m);
			return;
			IL_D6:
			if (!base.DesignMode && this.containingControl != null && this.containingControl.ActiveControl != this)
			{
				this.FocusInternal();
			}
			this.DefWndProc(ref m);
			return;
			IL_18F:
			if (m.Msg == WebBrowserHelper.REGMSG_MSG)
			{
				m.Result = (IntPtr)123;
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x060060E3 RID: 24803 RVA: 0x00162D64 File Offset: 0x00161D64
		protected override void OnParentChanged(EventArgs e)
		{
			Control parentInternal = this.ParentInternal;
			if ((base.Visible && parentInternal != null && parentInternal.Visible) || base.IsHandleCreated)
			{
				this.TransitionUpTo(WebBrowserHelper.AXState.InPlaceActive);
			}
			base.OnParentChanged(e);
		}

		// Token: 0x060060E4 RID: 24804 RVA: 0x00162DA1 File Offset: 0x00161DA1
		protected override void OnVisibleChanged(EventArgs e)
		{
			if (base.Visible && !base.Disposing && !base.IsDisposed)
			{
				this.TransitionUpTo(WebBrowserHelper.AXState.InPlaceActive);
			}
			base.OnVisibleChanged(e);
		}

		// Token: 0x060060E5 RID: 24805 RVA: 0x00162DC9 File Offset: 0x00161DC9
		protected override void OnGotFocus(EventArgs e)
		{
			if (this.ActiveXState < WebBrowserHelper.AXState.UIActive)
			{
				this.TransitionUpTo(WebBrowserHelper.AXState.UIActive);
			}
			base.OnGotFocus(e);
		}

		// Token: 0x060060E6 RID: 24806 RVA: 0x00162DE2 File Offset: 0x00161DE2
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if (!base.ContainsFocus)
			{
				this.TransitionDownTo(WebBrowserHelper.AXState.InPlaceActive);
			}
		}

		// Token: 0x060060E7 RID: 24807 RVA: 0x00162DFA File Offset: 0x00161DFA
		protected override void OnRightToLeftChanged(EventArgs e)
		{
		}

		// Token: 0x060060E8 RID: 24808 RVA: 0x00162DFC File Offset: 0x00161DFC
		internal override bool CanSelectCore()
		{
			return this.ActiveXState >= WebBrowserHelper.AXState.InPlaceActive && base.CanSelectCore();
		}

		// Token: 0x060060E9 RID: 24809 RVA: 0x00162E0F File Offset: 0x00161E0F
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.AmbientChanged(-703);
		}

		// Token: 0x060060EA RID: 24810 RVA: 0x00162E23 File Offset: 0x00161E23
		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			this.AmbientChanged(-704);
		}

		// Token: 0x060060EB RID: 24811 RVA: 0x00162E37 File Offset: 0x00161E37
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			this.AmbientChanged(-701);
		}

		// Token: 0x060060EC RID: 24812 RVA: 0x00162E4B File Offset: 0x00161E4B
		internal override void RecreateHandleCore()
		{
			if (!this.inRtlRecreate)
			{
				base.RecreateHandleCore();
			}
		}

		// Token: 0x060060ED RID: 24813 RVA: 0x00162E5B File Offset: 0x00161E5B
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.TransitionDownTo(WebBrowserHelper.AXState.Passive);
			}
			base.Dispose(disposing);
		}

		// Token: 0x17001480 RID: 5248
		// (get) Token: 0x060060EE RID: 24814 RVA: 0x00162E6E File Offset: 0x00161E6E
		// (set) Token: 0x060060EF RID: 24815 RVA: 0x00162E76 File Offset: 0x00161E76
		internal WebBrowserHelper.AXState ActiveXState
		{
			get
			{
				return this.axState;
			}
			set
			{
				this.axState = value;
			}
		}

		// Token: 0x060060F0 RID: 24816 RVA: 0x00162E7F File Offset: 0x00161E7F
		internal bool GetAXHostState(int mask)
		{
			return this.axHostState[mask];
		}

		// Token: 0x060060F1 RID: 24817 RVA: 0x00162E8D File Offset: 0x00161E8D
		internal void SetAXHostState(int mask, bool value)
		{
			this.axHostState[mask] = value;
		}

		// Token: 0x060060F2 RID: 24818 RVA: 0x00162E9C File Offset: 0x00161E9C
		internal IntPtr GetHandleNoCreate()
		{
			if (!base.IsHandleCreated)
			{
				return IntPtr.Zero;
			}
			return base.Handle;
		}

		// Token: 0x060060F3 RID: 24819 RVA: 0x00162EB4 File Offset: 0x00161EB4
		internal void TransitionUpTo(WebBrowserHelper.AXState state)
		{
			if (!this.GetAXHostState(WebBrowserHelper.inTransition))
			{
				this.SetAXHostState(WebBrowserHelper.inTransition, true);
				try
				{
					while (state > this.ActiveXState)
					{
						switch (this.ActiveXState)
						{
						case WebBrowserHelper.AXState.Passive:
							this.TransitionFromPassiveToLoaded();
							continue;
						case WebBrowserHelper.AXState.Loaded:
							this.TransitionFromLoadedToRunning();
							continue;
						case WebBrowserHelper.AXState.Running:
							this.TransitionFromRunningToInPlaceActive();
							continue;
						case WebBrowserHelper.AXState.InPlaceActive:
							this.TransitionFromInPlaceActiveToUIActive();
							continue;
						}
						this.ActiveXState++;
					}
				}
				finally
				{
					this.SetAXHostState(WebBrowserHelper.inTransition, false);
				}
			}
		}

		// Token: 0x060060F4 RID: 24820 RVA: 0x00162F58 File Offset: 0x00161F58
		internal void TransitionDownTo(WebBrowserHelper.AXState state)
		{
			if (!this.GetAXHostState(WebBrowserHelper.inTransition))
			{
				this.SetAXHostState(WebBrowserHelper.inTransition, true);
				try
				{
					while (state < this.ActiveXState)
					{
						WebBrowserHelper.AXState activeXState = this.ActiveXState;
						switch (activeXState)
						{
						case WebBrowserHelper.AXState.Loaded:
							this.TransitionFromLoadedToPassive();
							continue;
						case WebBrowserHelper.AXState.Running:
							this.TransitionFromRunningToLoaded();
							continue;
						case (WebBrowserHelper.AXState)3:
							break;
						case WebBrowserHelper.AXState.InPlaceActive:
							this.TransitionFromInPlaceActiveToRunning();
							continue;
						default:
							if (activeXState == WebBrowserHelper.AXState.UIActive)
							{
								this.TransitionFromUIActiveToInPlaceActive();
								continue;
							}
							break;
						}
						this.ActiveXState--;
					}
				}
				finally
				{
					this.SetAXHostState(WebBrowserHelper.inTransition, false);
				}
			}
		}

		// Token: 0x060060F5 RID: 24821 RVA: 0x00162FFC File Offset: 0x00161FFC
		internal bool DoVerb(int verb)
		{
			int num = this.axOleObject.DoVerb(verb, IntPtr.Zero, this.ActiveXSite, 0, base.Handle, new NativeMethods.COMRECT(base.Bounds));
			return num == 0;
		}

		// Token: 0x17001481 RID: 5249
		// (get) Token: 0x060060F6 RID: 24822 RVA: 0x00163037 File Offset: 0x00162037
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		internal ContainerControl ContainingControl
		{
			get
			{
				if (this.containingControl == null || this.GetAXHostState(WebBrowserHelper.recomputeContainingControl))
				{
					this.containingControl = this.FindContainerControlInternal();
				}
				return this.containingControl;
			}
		}

		// Token: 0x060060F7 RID: 24823 RVA: 0x00163060 File Offset: 0x00162060
		internal WebBrowserContainer CreateWebBrowserContainer()
		{
			if (this.wbContainer == null)
			{
				this.wbContainer = new WebBrowserContainer(this);
			}
			return this.wbContainer;
		}

		// Token: 0x060060F8 RID: 24824 RVA: 0x0016307C File Offset: 0x0016207C
		internal WebBrowserContainer GetParentContainer()
		{
			if (this.container == null)
			{
				this.container = WebBrowserContainer.FindContainerForControl(this);
			}
			if (this.container == null)
			{
				this.container = this.CreateWebBrowserContainer();
				this.container.AddControl(this);
			}
			return this.container;
		}

		// Token: 0x060060F9 RID: 24825 RVA: 0x001630B8 File Offset: 0x001620B8
		internal void SetEditMode(WebBrowserHelper.AXEditMode em)
		{
			this.axEditMode = em;
		}

		// Token: 0x060060FA RID: 24826 RVA: 0x001630C4 File Offset: 0x001620C4
		internal void SetSelectionStyle(WebBrowserHelper.SelectionStyle selectionStyle)
		{
			if (base.DesignMode)
			{
				ISelectionService selectionService = WebBrowserHelper.GetSelectionService(this);
				this.selectionStyle = selectionStyle;
				if (selectionService != null && selectionService.GetComponentSelected(this))
				{
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)["SelectionStyle"];
					if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(int))
					{
						propertyDescriptor.SetValue(this, (int)selectionStyle);
					}
				}
			}
		}

		// Token: 0x060060FB RID: 24827 RVA: 0x00163128 File Offset: 0x00162128
		internal void AddSelectionHandler()
		{
			if (!this.GetAXHostState(WebBrowserHelper.addedSelectionHandler))
			{
				this.SetAXHostState(WebBrowserHelper.addedSelectionHandler, true);
				ISelectionService selectionService = WebBrowserHelper.GetSelectionService(this);
				if (selectionService != null)
				{
					selectionService.SelectionChanging += this.SelectionChangeHandler;
				}
			}
		}

		// Token: 0x060060FC RID: 24828 RVA: 0x00163164 File Offset: 0x00162164
		internal bool RemoveSelectionHandler()
		{
			bool axhostState = this.GetAXHostState(WebBrowserHelper.addedSelectionHandler);
			if (axhostState)
			{
				this.SetAXHostState(WebBrowserHelper.addedSelectionHandler, false);
				ISelectionService selectionService = WebBrowserHelper.GetSelectionService(this);
				if (selectionService != null)
				{
					selectionService.SelectionChanging -= this.SelectionChangeHandler;
				}
			}
			return axhostState;
		}

		// Token: 0x060060FD RID: 24829 RVA: 0x001631A4 File Offset: 0x001621A4
		internal void AttachWindow(IntPtr hwnd)
		{
			UnsafeNativeMethods.SetParent(new HandleRef(null, hwnd), new HandleRef(this, base.Handle));
			if (this.axWindow != null)
			{
				this.axWindow.ReleaseHandle();
			}
			this.axWindow = new WebBrowserBase.WebBrowserBaseNativeWindow(this);
			this.axWindow.AssignHandle(hwnd, false);
			base.UpdateZOrder();
			base.UpdateBounds();
			Size size = base.Size;
			size = this.SetExtent(size.Width, size.Height);
			Point location = base.Location;
			base.Bounds = new Rectangle(location.X, location.Y, size.Width, size.Height);
		}

		// Token: 0x17001482 RID: 5250
		// (get) Token: 0x060060FE RID: 24830 RVA: 0x0016324C File Offset: 0x0016224C
		internal bool IsUserMode
		{
			get
			{
				return this.Site == null || !base.DesignMode;
			}
		}

		// Token: 0x060060FF RID: 24831 RVA: 0x00163264 File Offset: 0x00162264
		internal void MakeDirty()
		{
			ISite site = this.Site;
			if (site != null)
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
				if (componentChangeService != null)
				{
					componentChangeService.OnComponentChanging(this, null);
					componentChangeService.OnComponentChanged(this, null, null, null);
				}
			}
		}

		// Token: 0x17001483 RID: 5251
		// (get) Token: 0x06006100 RID: 24832 RVA: 0x001632A6 File Offset: 0x001622A6
		// (set) Token: 0x06006101 RID: 24833 RVA: 0x001632AE File Offset: 0x001622AE
		internal int NoComponentChangeEvents
		{
			get
			{
				return this.noComponentChange;
			}
			set
			{
				this.noComponentChange = value;
			}
		}

		// Token: 0x06006102 RID: 24834 RVA: 0x001632B7 File Offset: 0x001622B7
		private void StartEvents()
		{
			if (!this.GetAXHostState(WebBrowserHelper.sinkAttached))
			{
				this.SetAXHostState(WebBrowserHelper.sinkAttached, true);
				this.CreateSink();
			}
			this.ActiveXSite.StartEvents();
		}

		// Token: 0x06006103 RID: 24835 RVA: 0x001632E3 File Offset: 0x001622E3
		private void StopEvents()
		{
			if (this.GetAXHostState(WebBrowserHelper.sinkAttached))
			{
				this.SetAXHostState(WebBrowserHelper.sinkAttached, false);
				this.DetachSink();
			}
			this.ActiveXSite.StopEvents();
		}

		// Token: 0x06006104 RID: 24836 RVA: 0x0016330F File Offset: 0x0016230F
		private void TransitionFromPassiveToLoaded()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.Passive)
			{
				this.activeXInstance = UnsafeNativeMethods.CoCreateInstance(ref this.clsid, null, 1, ref NativeMethods.ActiveX.IID_IUnknown);
				this.ActiveXState = WebBrowserHelper.AXState.Loaded;
				this.AttachInterfacesInternal();
			}
		}

		// Token: 0x06006105 RID: 24837 RVA: 0x00163340 File Offset: 0x00162340
		private void TransitionFromLoadedToPassive()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.Loaded)
			{
				this.NoComponentChangeEvents++;
				try
				{
					if (this.activeXInstance != null)
					{
						this.DetachInterfacesInternal();
						Marshal.FinalReleaseComObject(this.activeXInstance);
						this.activeXInstance = null;
					}
				}
				finally
				{
					this.NoComponentChangeEvents--;
				}
				this.ActiveXState = WebBrowserHelper.AXState.Passive;
			}
		}

		// Token: 0x06006106 RID: 24838 RVA: 0x001633B0 File Offset: 0x001623B0
		private void TransitionFromLoadedToRunning()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.Loaded)
			{
				int num = 0;
				int miscStatus = this.axOleObject.GetMiscStatus(1, out num);
				if (NativeMethods.Succeeded(miscStatus) && (num & 131072) != 0)
				{
					this.axOleObject.SetClientSite(this.ActiveXSite);
				}
				if (!base.DesignMode)
				{
					this.StartEvents();
				}
				this.ActiveXState = WebBrowserHelper.AXState.Running;
			}
		}

		// Token: 0x06006107 RID: 24839 RVA: 0x00163410 File Offset: 0x00162410
		private void TransitionFromRunningToLoaded()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.Running)
			{
				this.StopEvents();
				WebBrowserContainer parentContainer = this.GetParentContainer();
				if (parentContainer != null)
				{
					parentContainer.RemoveControl(this);
				}
				this.axOleObject.SetClientSite(null);
				this.ActiveXState = WebBrowserHelper.AXState.Loaded;
			}
		}

		// Token: 0x06006108 RID: 24840 RVA: 0x00163454 File Offset: 0x00162454
		private void TransitionFromRunningToInPlaceActive()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.Running)
			{
				try
				{
					this.DoVerb(-5);
				}
				catch (Exception inner)
				{
					throw new TargetInvocationException(SR.GetString("AXNohWnd", new object[]
					{
						base.GetType().Name
					}), inner);
				}
				base.CreateControl(true);
				this.ActiveXState = WebBrowserHelper.AXState.InPlaceActive;
			}
		}

		// Token: 0x06006109 RID: 24841 RVA: 0x001634BC File Offset: 0x001624BC
		private void TransitionFromInPlaceActiveToRunning()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.InPlaceActive)
			{
				ContainerControl containerControl = this.ContainingControl;
				if (containerControl != null && containerControl.ActiveControl == this)
				{
					containerControl.SetActiveControlInternal(null);
				}
				this.AXInPlaceObject.InPlaceDeactivate();
				this.ActiveXState = WebBrowserHelper.AXState.Running;
			}
		}

		// Token: 0x0600610A RID: 24842 RVA: 0x00163500 File Offset: 0x00162500
		private void TransitionFromInPlaceActiveToUIActive()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.InPlaceActive)
			{
				try
				{
					this.DoVerb(-4);
				}
				catch (Exception inner)
				{
					throw new TargetInvocationException(SR.GetString("AXNohWnd", new object[]
					{
						base.GetType().Name
					}), inner);
				}
				this.ActiveXState = WebBrowserHelper.AXState.UIActive;
			}
		}

		// Token: 0x0600610B RID: 24843 RVA: 0x00163560 File Offset: 0x00162560
		private void TransitionFromUIActiveToInPlaceActive()
		{
			if (this.ActiveXState == WebBrowserHelper.AXState.UIActive)
			{
				this.AXInPlaceObject.UIDeactivate();
				this.ActiveXState = WebBrowserHelper.AXState.InPlaceActive;
			}
		}

		// Token: 0x17001484 RID: 5252
		// (get) Token: 0x0600610C RID: 24844 RVA: 0x0016357E File Offset: 0x0016257E
		internal WebBrowserSiteBase ActiveXSite
		{
			get
			{
				if (this.axSite == null)
				{
					this.axSite = this.CreateWebBrowserSiteBase();
				}
				return this.axSite;
			}
		}

		// Token: 0x0600610D RID: 24845 RVA: 0x0016359C File Offset: 0x0016259C
		private void AttachInterfacesInternal()
		{
			this.axOleObject = (UnsafeNativeMethods.IOleObject)this.activeXInstance;
			this.axOleInPlaceObject = (UnsafeNativeMethods.IOleInPlaceObject)this.activeXInstance;
			this.axOleInPlaceActiveObject = (UnsafeNativeMethods.IOleInPlaceActiveObject)this.activeXInstance;
			this.axOleControl = (UnsafeNativeMethods.IOleControl)this.activeXInstance;
			this.AttachInterfaces(this.activeXInstance);
		}

		// Token: 0x0600610E RID: 24846 RVA: 0x001635F9 File Offset: 0x001625F9
		private void DetachInterfacesInternal()
		{
			this.axOleObject = null;
			this.axOleInPlaceObject = null;
			this.axOleInPlaceActiveObject = null;
			this.axOleControl = null;
			this.DetachInterfaces();
		}

		// Token: 0x17001485 RID: 5253
		// (get) Token: 0x0600610F RID: 24847 RVA: 0x0016361D File Offset: 0x0016261D
		private EventHandler SelectionChangeHandler
		{
			get
			{
				if (this.selectionChangeHandler == null)
				{
					this.selectionChangeHandler = new EventHandler(this.OnNewSelection);
				}
				return this.selectionChangeHandler;
			}
		}

		// Token: 0x06006110 RID: 24848 RVA: 0x00163640 File Offset: 0x00162640
		private void OnNewSelection(object sender, EventArgs e)
		{
			if (base.DesignMode)
			{
				ISelectionService selectionService = WebBrowserHelper.GetSelectionService(this);
				if (selectionService != null)
				{
					if (!selectionService.GetComponentSelected(this))
					{
						if (this.EditMode)
						{
							this.GetParentContainer().OnExitEditMode(this);
							this.SetEditMode(WebBrowserHelper.AXEditMode.None);
						}
						this.SetSelectionStyle(WebBrowserHelper.SelectionStyle.Selected);
						this.RemoveSelectionHandler();
						return;
					}
					PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(this)["SelectionStyle"];
					if (propertyDescriptor != null && propertyDescriptor.PropertyType == typeof(int))
					{
						int num = (int)propertyDescriptor.GetValue(this);
						if (num != (int)this.selectionStyle)
						{
							propertyDescriptor.SetValue(this, this.selectionStyle);
						}
					}
				}
			}
		}

		// Token: 0x06006111 RID: 24849 RVA: 0x001636E8 File Offset: 0x001626E8
		private Size SetExtent(int width, int height)
		{
			NativeMethods.tagSIZEL tagSIZEL = new NativeMethods.tagSIZEL();
			tagSIZEL.cx = width;
			tagSIZEL.cy = height;
			bool flag = base.DesignMode;
			try
			{
				this.Pixel2hiMetric(tagSIZEL, tagSIZEL);
				this.axOleObject.SetExtent(1, tagSIZEL);
			}
			catch (COMException)
			{
				flag = true;
			}
			if (flag)
			{
				this.axOleObject.GetExtent(1, tagSIZEL);
				try
				{
					this.axOleObject.SetExtent(1, tagSIZEL);
				}
				catch (COMException)
				{
				}
			}
			return this.GetExtent();
		}

		// Token: 0x06006112 RID: 24850 RVA: 0x00163774 File Offset: 0x00162774
		private Size GetExtent()
		{
			NativeMethods.tagSIZEL tagSIZEL = new NativeMethods.tagSIZEL();
			this.axOleObject.GetExtent(1, tagSIZEL);
			this.HiMetric2Pixel(tagSIZEL, tagSIZEL);
			return new Size(tagSIZEL.cx, tagSIZEL.cy);
		}

		// Token: 0x06006113 RID: 24851 RVA: 0x001637B0 File Offset: 0x001627B0
		private void HiMetric2Pixel(NativeMethods.tagSIZEL sz, NativeMethods.tagSIZEL szout)
		{
			NativeMethods._POINTL pointl = new NativeMethods._POINTL();
			pointl.x = sz.cx;
			pointl.y = sz.cy;
			NativeMethods.tagPOINTF tagPOINTF = new NativeMethods.tagPOINTF();
			((UnsafeNativeMethods.IOleControlSite)this.ActiveXSite).TransformCoords(pointl, tagPOINTF, 6);
			szout.cx = (int)tagPOINTF.x;
			szout.cy = (int)tagPOINTF.y;
		}

		// Token: 0x06006114 RID: 24852 RVA: 0x0016380C File Offset: 0x0016280C
		private void Pixel2hiMetric(NativeMethods.tagSIZEL sz, NativeMethods.tagSIZEL szout)
		{
			NativeMethods.tagPOINTF tagPOINTF = new NativeMethods.tagPOINTF();
			tagPOINTF.x = (float)sz.cx;
			tagPOINTF.y = (float)sz.cy;
			NativeMethods._POINTL pointl = new NativeMethods._POINTL();
			((UnsafeNativeMethods.IOleControlSite)this.ActiveXSite).TransformCoords(pointl, tagPOINTF, 10);
			szout.cx = pointl.x;
			szout.cy = pointl.y;
		}

		// Token: 0x17001486 RID: 5254
		// (get) Token: 0x06006115 RID: 24853 RVA: 0x00163867 File Offset: 0x00162867
		private bool EditMode
		{
			get
			{
				return this.axEditMode != WebBrowserHelper.AXEditMode.None;
			}
		}

		// Token: 0x06006116 RID: 24854 RVA: 0x00163878 File Offset: 0x00162878
		internal ContainerControl FindContainerControlInternal()
		{
			if (this.Site != null)
			{
				IDesignerHost designerHost = (IDesignerHost)this.Site.GetService(typeof(IDesignerHost));
				if (designerHost != null)
				{
					IComponent rootComponent = designerHost.RootComponent;
					if (rootComponent != null && rootComponent is ContainerControl)
					{
						return (ContainerControl)rootComponent;
					}
				}
			}
			ContainerControl containerControl = null;
			for (Control control = this; control != null; control = control.ParentInternal)
			{
				ContainerControl containerControl2 = control as ContainerControl;
				if (containerControl2 != null)
				{
					containerControl = containerControl2;
				}
			}
			if (containerControl == null)
			{
				containerControl = (Control.FromHandle(UnsafeNativeMethods.GetParent(new HandleRef(this, base.Handle))) as ContainerControl);
			}
			if (containerControl is Application.ParkingWindow)
			{
				containerControl = null;
			}
			this.SetAXHostState(WebBrowserHelper.recomputeContainingControl, containerControl == null);
			return containerControl;
		}

		// Token: 0x06006117 RID: 24855 RVA: 0x0016391C File Offset: 0x0016291C
		private void AmbientChanged(int dispid)
		{
			if (this.activeXInstance != null)
			{
				try
				{
					base.Invalidate();
					this.axOleControl.OnAmbientPropertyChange(dispid);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
				}
			}
		}

		// Token: 0x17001487 RID: 5255
		// (get) Token: 0x06006118 RID: 24856 RVA: 0x00163964 File Offset: 0x00162964
		internal UnsafeNativeMethods.IOleInPlaceObject AXInPlaceObject
		{
			get
			{
				return this.axOleInPlaceObject;
			}
		}

		// Token: 0x17001488 RID: 5256
		// (get) Token: 0x06006119 RID: 24857 RVA: 0x0016396C File Offset: 0x0016296C
		protected override Size DefaultSize
		{
			get
			{
				return new Size(75, 23);
			}
		}

		// Token: 0x0600611A RID: 24858 RVA: 0x00163977 File Offset: 0x00162977
		protected override bool IsInputChar(char charCode)
		{
			return true;
		}

		// Token: 0x0600611B RID: 24859 RVA: 0x0016397C File Offset: 0x0016297C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnHandleCreated(EventArgs e)
		{
			if (Application.OleRequired() != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
			}
			base.OnHandleCreated(e);
			if (this.axReloadingState != WebBrowserHelper.AXState.Passive && this.axReloadingState != this.axState)
			{
				if (this.axState < this.axReloadingState)
				{
					this.TransitionUpTo(this.axReloadingState);
				}
				else
				{
					this.TransitionDownTo(this.axReloadingState);
				}
				this.axReloadingState = WebBrowserHelper.AXState.Passive;
			}
		}

		// Token: 0x17001489 RID: 5257
		// (get) Token: 0x0600611C RID: 24860 RVA: 0x001639EC File Offset: 0x001629EC
		// (set) Token: 0x0600611D RID: 24861 RVA: 0x001639F4 File Offset: 0x001629F4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		// Token: 0x1700148A RID: 5258
		// (get) Token: 0x0600611E RID: 24862 RVA: 0x001639FD File Offset: 0x001629FD
		// (set) Token: 0x0600611F RID: 24863 RVA: 0x00163A05 File Offset: 0x00162A05
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		// Token: 0x1700148B RID: 5259
		// (get) Token: 0x06006120 RID: 24864 RVA: 0x00163A0E File Offset: 0x00162A0E
		// (set) Token: 0x06006121 RID: 24865 RVA: 0x00163A16 File Offset: 0x00162A16
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		// Token: 0x1700148C RID: 5260
		// (get) Token: 0x06006122 RID: 24866 RVA: 0x00163A1F File Offset: 0x00162A1F
		// (set) Token: 0x06006123 RID: 24867 RVA: 0x00163A27 File Offset: 0x00162A27
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new ImeMode ImeMode
		{
			get
			{
				return base.ImeMode;
			}
			set
			{
				base.ImeMode = value;
			}
		}

		// Token: 0x1700148D RID: 5261
		// (get) Token: 0x06006124 RID: 24868 RVA: 0x00163A30 File Offset: 0x00162A30
		// (set) Token: 0x06006125 RID: 24869 RVA: 0x00163A38 File Offset: 0x00162A38
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool AllowDrop
		{
			get
			{
				return base.AllowDrop;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserAllowDropNotSupported"));
			}
		}

		// Token: 0x1700148E RID: 5262
		// (get) Token: 0x06006126 RID: 24870 RVA: 0x00163A49 File Offset: 0x00162A49
		// (set) Token: 0x06006127 RID: 24871 RVA: 0x00163A51 File Offset: 0x00162A51
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserBackgroundImageNotSupported"));
			}
		}

		// Token: 0x1700148F RID: 5263
		// (get) Token: 0x06006128 RID: 24872 RVA: 0x00163A62 File Offset: 0x00162A62
		// (set) Token: 0x06006129 RID: 24873 RVA: 0x00163A6A File Offset: 0x00162A6A
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserBackgroundImageLayoutNotSupported"));
			}
		}

		// Token: 0x17001490 RID: 5264
		// (get) Token: 0x0600612A RID: 24874 RVA: 0x00163A7B File Offset: 0x00162A7B
		// (set) Token: 0x0600612B RID: 24875 RVA: 0x00163A83 File Offset: 0x00162A83
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Cursor Cursor
		{
			get
			{
				return base.Cursor;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserCursorNotSupported"));
			}
		}

		// Token: 0x17001491 RID: 5265
		// (get) Token: 0x0600612C RID: 24876 RVA: 0x00163A94 File Offset: 0x00162A94
		// (set) Token: 0x0600612D RID: 24877 RVA: 0x00163A9C File Offset: 0x00162A9C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserEnabledNotSupported"));
			}
		}

		// Token: 0x17001492 RID: 5266
		// (get) Token: 0x0600612E RID: 24878 RVA: 0x00163AAD File Offset: 0x00162AAD
		// (set) Token: 0x0600612F RID: 24879 RVA: 0x00163AB0 File Offset: 0x00162AB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Localizable(false)]
		[Browsable(false)]
		public override RightToLeft RightToLeft
		{
			get
			{
				return RightToLeft.No;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserRightToLeftNotSupported"));
			}
		}

		// Token: 0x17001493 RID: 5267
		// (get) Token: 0x06006130 RID: 24880 RVA: 0x00163AC1 File Offset: 0x00162AC1
		// (set) Token: 0x06006131 RID: 24881 RVA: 0x00163AC8 File Offset: 0x00162AC8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string Text
		{
			get
			{
				return "";
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserTextNotSupported"));
			}
		}

		// Token: 0x17001494 RID: 5268
		// (get) Token: 0x06006132 RID: 24882 RVA: 0x00163AD9 File Offset: 0x00162AD9
		// (set) Token: 0x06006133 RID: 24883 RVA: 0x00163AE1 File Offset: 0x00162AE1
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool UseWaitCursor
		{
			get
			{
				return base.UseWaitCursor;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("WebBrowserUseWaitCursorNotSupported"));
			}
		}

		// Token: 0x140003B7 RID: 951
		// (add) Token: 0x06006134 RID: 24884 RVA: 0x00163AF4 File Offset: 0x00162AF4
		// (remove) Token: 0x06006135 RID: 24885 RVA: 0x00163B20 File Offset: 0x00162B20
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"BackgroundImageLayoutChanged"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003B8 RID: 952
		// (add) Token: 0x06006136 RID: 24886 RVA: 0x00163B24 File Offset: 0x00162B24
		// (remove) Token: 0x06006137 RID: 24887 RVA: 0x00163B50 File Offset: 0x00162B50
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Enter
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"Enter"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003B9 RID: 953
		// (add) Token: 0x06006138 RID: 24888 RVA: 0x00163B54 File Offset: 0x00162B54
		// (remove) Token: 0x06006139 RID: 24889 RVA: 0x00163B80 File Offset: 0x00162B80
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Leave
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"Leave"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003BA RID: 954
		// (add) Token: 0x0600613A RID: 24890 RVA: 0x00163B84 File Offset: 0x00162B84
		// (remove) Token: 0x0600613B RID: 24891 RVA: 0x00163BB0 File Offset: 0x00162BB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler MouseCaptureChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseCaptureChanged"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003BB RID: 955
		// (add) Token: 0x0600613C RID: 24892 RVA: 0x00163BB4 File Offset: 0x00162BB4
		// (remove) Token: 0x0600613D RID: 24893 RVA: 0x00163BE0 File Offset: 0x00162BE0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event MouseEventHandler MouseClick
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseClick"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003BC RID: 956
		// (add) Token: 0x0600613E RID: 24894 RVA: 0x00163BE4 File Offset: 0x00162BE4
		// (remove) Token: 0x0600613F RID: 24895 RVA: 0x00163C10 File Offset: 0x00162C10
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseDoubleClick
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseDoubleClick"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003BD RID: 957
		// (add) Token: 0x06006140 RID: 24896 RVA: 0x00163C14 File Offset: 0x00162C14
		// (remove) Token: 0x06006141 RID: 24897 RVA: 0x00163C40 File Offset: 0x00162C40
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler BackColorChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"BackColorChanged"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003BE RID: 958
		// (add) Token: 0x06006142 RID: 24898 RVA: 0x00163C44 File Offset: 0x00162C44
		// (remove) Token: 0x06006143 RID: 24899 RVA: 0x00163C70 File Offset: 0x00162C70
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"BackgroundImageChanged"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003BF RID: 959
		// (add) Token: 0x06006144 RID: 24900 RVA: 0x00163C74 File Offset: 0x00162C74
		// (remove) Token: 0x06006145 RID: 24901 RVA: 0x00163CA0 File Offset: 0x00162CA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler BindingContextChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"BindingContextChanged"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003C0 RID: 960
		// (add) Token: 0x06006146 RID: 24902 RVA: 0x00163CA4 File Offset: 0x00162CA4
		// (remove) Token: 0x06006147 RID: 24903 RVA: 0x00163CD0 File Offset: 0x00162CD0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler CursorChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"CursorChanged"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003C1 RID: 961
		// (add) Token: 0x06006148 RID: 24904 RVA: 0x00163CD4 File Offset: 0x00162CD4
		// (remove) Token: 0x06006149 RID: 24905 RVA: 0x00163D00 File Offset: 0x00162D00
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler EnabledChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"EnabledChanged"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003C2 RID: 962
		// (add) Token: 0x0600614A RID: 24906 RVA: 0x00163D04 File Offset: 0x00162D04
		// (remove) Token: 0x0600614B RID: 24907 RVA: 0x00163D30 File Offset: 0x00162D30
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler FontChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"FontChanged"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003C3 RID: 963
		// (add) Token: 0x0600614C RID: 24908 RVA: 0x00163D34 File Offset: 0x00162D34
		// (remove) Token: 0x0600614D RID: 24909 RVA: 0x00163D60 File Offset: 0x00162D60
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ForeColorChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"ForeColorChanged"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003C4 RID: 964
		// (add) Token: 0x0600614E RID: 24910 RVA: 0x00163D64 File Offset: 0x00162D64
		// (remove) Token: 0x0600614F RID: 24911 RVA: 0x00163D90 File Offset: 0x00162D90
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler RightToLeftChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"RightToLeftChanged"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003C5 RID: 965
		// (add) Token: 0x06006150 RID: 24912 RVA: 0x00163D94 File Offset: 0x00162D94
		// (remove) Token: 0x06006151 RID: 24913 RVA: 0x00163DC0 File Offset: 0x00162DC0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TextChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"TextChanged"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003C6 RID: 966
		// (add) Token: 0x06006152 RID: 24914 RVA: 0x00163DC4 File Offset: 0x00162DC4
		// (remove) Token: 0x06006153 RID: 24915 RVA: 0x00163DF0 File Offset: 0x00162DF0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler Click
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"Click"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003C7 RID: 967
		// (add) Token: 0x06006154 RID: 24916 RVA: 0x00163DF4 File Offset: 0x00162DF4
		// (remove) Token: 0x06006155 RID: 24917 RVA: 0x00163E20 File Offset: 0x00162E20
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event DragEventHandler DragDrop
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"DragDrop"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003C8 RID: 968
		// (add) Token: 0x06006156 RID: 24918 RVA: 0x00163E24 File Offset: 0x00162E24
		// (remove) Token: 0x06006157 RID: 24919 RVA: 0x00163E50 File Offset: 0x00162E50
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event DragEventHandler DragEnter
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"DragEnter"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003C9 RID: 969
		// (add) Token: 0x06006158 RID: 24920 RVA: 0x00163E54 File Offset: 0x00162E54
		// (remove) Token: 0x06006159 RID: 24921 RVA: 0x00163E80 File Offset: 0x00162E80
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event DragEventHandler DragOver
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"DragOver"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003CA RID: 970
		// (add) Token: 0x0600615A RID: 24922 RVA: 0x00163E84 File Offset: 0x00162E84
		// (remove) Token: 0x0600615B RID: 24923 RVA: 0x00163EB0 File Offset: 0x00162EB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler DragLeave
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"DragLeave"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003CB RID: 971
		// (add) Token: 0x0600615C RID: 24924 RVA: 0x00163EB4 File Offset: 0x00162EB4
		// (remove) Token: 0x0600615D RID: 24925 RVA: 0x00163EE0 File Offset: 0x00162EE0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event GiveFeedbackEventHandler GiveFeedback
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"GiveFeedback"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003CC RID: 972
		// (add) Token: 0x0600615E RID: 24926 RVA: 0x00163EE4 File Offset: 0x00162EE4
		// (remove) Token: 0x0600615F RID: 24927 RVA: 0x00163F10 File Offset: 0x00162F10
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event HelpEventHandler HelpRequested
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"HelpRequested"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003CD RID: 973
		// (add) Token: 0x06006160 RID: 24928 RVA: 0x00163F14 File Offset: 0x00162F14
		// (remove) Token: 0x06006161 RID: 24929 RVA: 0x00163F40 File Offset: 0x00162F40
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event PaintEventHandler Paint
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"Paint"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003CE RID: 974
		// (add) Token: 0x06006162 RID: 24930 RVA: 0x00163F44 File Offset: 0x00162F44
		// (remove) Token: 0x06006163 RID: 24931 RVA: 0x00163F70 File Offset: 0x00162F70
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event QueryContinueDragEventHandler QueryContinueDrag
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"QueryContinueDrag"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003CF RID: 975
		// (add) Token: 0x06006164 RID: 24932 RVA: 0x00163F74 File Offset: 0x00162F74
		// (remove) Token: 0x06006165 RID: 24933 RVA: 0x00163FA0 File Offset: 0x00162FA0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"QueryAccessibilityHelp"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003D0 RID: 976
		// (add) Token: 0x06006166 RID: 24934 RVA: 0x00163FA4 File Offset: 0x00162FA4
		// (remove) Token: 0x06006167 RID: 24935 RVA: 0x00163FD0 File Offset: 0x00162FD0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler DoubleClick
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"DoubleClick"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003D1 RID: 977
		// (add) Token: 0x06006168 RID: 24936 RVA: 0x00163FD4 File Offset: 0x00162FD4
		// (remove) Token: 0x06006169 RID: 24937 RVA: 0x00164000 File Offset: 0x00163000
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler ImeModeChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"ImeModeChanged"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003D2 RID: 978
		// (add) Token: 0x0600616A RID: 24938 RVA: 0x00164004 File Offset: 0x00163004
		// (remove) Token: 0x0600616B RID: 24939 RVA: 0x00164030 File Offset: 0x00163030
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event KeyEventHandler KeyDown
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"KeyDown"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003D3 RID: 979
		// (add) Token: 0x0600616C RID: 24940 RVA: 0x00164034 File Offset: 0x00163034
		// (remove) Token: 0x0600616D RID: 24941 RVA: 0x00164060 File Offset: 0x00163060
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"KeyPress"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003D4 RID: 980
		// (add) Token: 0x0600616E RID: 24942 RVA: 0x00164064 File Offset: 0x00163064
		// (remove) Token: 0x0600616F RID: 24943 RVA: 0x00164090 File Offset: 0x00163090
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyUp
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"KeyUp"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003D5 RID: 981
		// (add) Token: 0x06006170 RID: 24944 RVA: 0x00164094 File Offset: 0x00163094
		// (remove) Token: 0x06006171 RID: 24945 RVA: 0x001640C0 File Offset: 0x001630C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event LayoutEventHandler Layout
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"Layout"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003D6 RID: 982
		// (add) Token: 0x06006172 RID: 24946 RVA: 0x001640C4 File Offset: 0x001630C4
		// (remove) Token: 0x06006173 RID: 24947 RVA: 0x001640F0 File Offset: 0x001630F0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseDown
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseDown"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003D7 RID: 983
		// (add) Token: 0x06006174 RID: 24948 RVA: 0x001640F4 File Offset: 0x001630F4
		// (remove) Token: 0x06006175 RID: 24949 RVA: 0x00164120 File Offset: 0x00163120
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler MouseEnter
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseEnter"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003D8 RID: 984
		// (add) Token: 0x06006176 RID: 24950 RVA: 0x00164124 File Offset: 0x00163124
		// (remove) Token: 0x06006177 RID: 24951 RVA: 0x00164150 File Offset: 0x00163150
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler MouseLeave
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseLeave"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003D9 RID: 985
		// (add) Token: 0x06006178 RID: 24952 RVA: 0x00164154 File Offset: 0x00163154
		// (remove) Token: 0x06006179 RID: 24953 RVA: 0x00164180 File Offset: 0x00163180
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler MouseHover
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseHover"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003DA RID: 986
		// (add) Token: 0x0600617A RID: 24954 RVA: 0x00164184 File Offset: 0x00163184
		// (remove) Token: 0x0600617B RID: 24955 RVA: 0x001641B0 File Offset: 0x001631B0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event MouseEventHandler MouseMove
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseMove"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003DB RID: 987
		// (add) Token: 0x0600617C RID: 24956 RVA: 0x001641B4 File Offset: 0x001631B4
		// (remove) Token: 0x0600617D RID: 24957 RVA: 0x001641E0 File Offset: 0x001631E0
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event MouseEventHandler MouseUp
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseUp"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003DC RID: 988
		// (add) Token: 0x0600617E RID: 24958 RVA: 0x001641E4 File Offset: 0x001631E4
		// (remove) Token: 0x0600617F RID: 24959 RVA: 0x00164210 File Offset: 0x00163210
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event MouseEventHandler MouseWheel
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"MouseWheel"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003DD RID: 989
		// (add) Token: 0x06006180 RID: 24960 RVA: 0x00164214 File Offset: 0x00163214
		// (remove) Token: 0x06006181 RID: 24961 RVA: 0x00164240 File Offset: 0x00163240
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event UICuesEventHandler ChangeUICues
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"ChangeUICues"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x140003DE RID: 990
		// (add) Token: 0x06006182 RID: 24962 RVA: 0x00164244 File Offset: 0x00163244
		// (remove) Token: 0x06006183 RID: 24963 RVA: 0x00164270 File Offset: 0x00163270
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler StyleChanged
		{
			add
			{
				throw new NotSupportedException(SR.GetString("AXAddInvalidEvent", new object[]
				{
					"StyleChanged"
				}));
			}
			remove
			{
			}
		}

		// Token: 0x04003AAA RID: 15018
		private WebBrowserHelper.AXState axState;

		// Token: 0x04003AAB RID: 15019
		private WebBrowserHelper.AXState axReloadingState;

		// Token: 0x04003AAC RID: 15020
		private WebBrowserHelper.AXEditMode axEditMode;

		// Token: 0x04003AAD RID: 15021
		private bool inRtlRecreate;

		// Token: 0x04003AAE RID: 15022
		private BitVector32 axHostState = default(BitVector32);

		// Token: 0x04003AAF RID: 15023
		private WebBrowserHelper.SelectionStyle selectionStyle;

		// Token: 0x04003AB0 RID: 15024
		private int noComponentChange;

		// Token: 0x04003AB1 RID: 15025
		private WebBrowserSiteBase axSite;

		// Token: 0x04003AB2 RID: 15026
		private ContainerControl containingControl;

		// Token: 0x04003AB3 RID: 15027
		private IntPtr hwndFocus = IntPtr.Zero;

		// Token: 0x04003AB4 RID: 15028
		private EventHandler selectionChangeHandler;

		// Token: 0x04003AB5 RID: 15029
		private Guid clsid;

		// Token: 0x04003AB6 RID: 15030
		private UnsafeNativeMethods.IOleObject axOleObject;

		// Token: 0x04003AB7 RID: 15031
		private UnsafeNativeMethods.IOleInPlaceObject axOleInPlaceObject;

		// Token: 0x04003AB8 RID: 15032
		private UnsafeNativeMethods.IOleInPlaceActiveObject axOleInPlaceActiveObject;

		// Token: 0x04003AB9 RID: 15033
		private UnsafeNativeMethods.IOleControl axOleControl;

		// Token: 0x04003ABA RID: 15034
		private WebBrowserBase.WebBrowserBaseNativeWindow axWindow;

		// Token: 0x04003ABB RID: 15035
		private Size webBrowserBaseChangingSize = Size.Empty;

		// Token: 0x04003ABC RID: 15036
		private WebBrowserContainer wbContainer;

		// Token: 0x04003ABD RID: 15037
		private bool ignoreDialogKeys;

		// Token: 0x04003ABE RID: 15038
		internal WebBrowserContainer container;

		// Token: 0x04003ABF RID: 15039
		internal object activeXInstance;

		// Token: 0x02000725 RID: 1829
		private class WebBrowserBaseNativeWindow : NativeWindow
		{
			// Token: 0x06006184 RID: 24964 RVA: 0x00164272 File Offset: 0x00163272
			public WebBrowserBaseNativeWindow(WebBrowserBase ax)
			{
				this.WebBrowserBase = ax;
			}

			// Token: 0x06006185 RID: 24965 RVA: 0x00164284 File Offset: 0x00163284
			protected override void WndProc(ref Message m)
			{
				int msg = m.Msg;
				if (msg == 70)
				{
					this.WmWindowPosChanging(ref m);
					return;
				}
				base.WndProc(ref m);
			}

			// Token: 0x06006186 RID: 24966 RVA: 0x001642AC File Offset: 0x001632AC
			private unsafe void WmWindowPosChanging(ref Message m)
			{
				NativeMethods.WINDOWPOS* ptr = (NativeMethods.WINDOWPOS*)((void*)m.LParam);
				ptr->x = 0;
				ptr->y = 0;
				Size webBrowserBaseChangingSize = this.WebBrowserBase.webBrowserBaseChangingSize;
				if (webBrowserBaseChangingSize.Width == -1)
				{
					ptr->cx = this.WebBrowserBase.Width;
					ptr->cy = this.WebBrowserBase.Height;
				}
				else
				{
					ptr->cx = webBrowserBaseChangingSize.Width;
					ptr->cy = webBrowserBaseChangingSize.Height;
				}
				m.Result = (IntPtr)0;
			}

			// Token: 0x04003AC0 RID: 15040
			private WebBrowserBase WebBrowserBase;
		}
	}
}

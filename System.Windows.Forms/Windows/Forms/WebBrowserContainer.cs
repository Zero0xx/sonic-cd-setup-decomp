using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200072D RID: 1837
	internal class WebBrowserContainer : UnsafeNativeMethods.IOleContainer, UnsafeNativeMethods.IOleInPlaceFrame
	{
		// Token: 0x06006255 RID: 25173 RVA: 0x00165F11 File Offset: 0x00164F11
		internal WebBrowserContainer(WebBrowserBase parent)
		{
			this.parent = parent;
		}

		// Token: 0x06006256 RID: 25174 RVA: 0x00165F2B File Offset: 0x00164F2B
		int UnsafeNativeMethods.IOleContainer.ParseDisplayName(object pbc, string pszDisplayName, int[] pchEaten, object[] ppmkOut)
		{
			if (ppmkOut != null)
			{
				ppmkOut[0] = null;
			}
			return -2147467263;
		}

		// Token: 0x06006257 RID: 25175 RVA: 0x00165F3C File Offset: 0x00164F3C
		int UnsafeNativeMethods.IOleContainer.EnumObjects(int grfFlags, out UnsafeNativeMethods.IEnumUnknown ppenum)
		{
			ppenum = null;
			if ((grfFlags & 1) != 0)
			{
				ArrayList arrayList = new ArrayList();
				this.ListAXControls(arrayList, true);
				if (arrayList.Count > 0)
				{
					object[] array = new object[arrayList.Count];
					arrayList.CopyTo(array, 0);
					ppenum = new AxHost.EnumUnknown(array);
					return 0;
				}
			}
			ppenum = new AxHost.EnumUnknown(null);
			return 0;
		}

		// Token: 0x06006258 RID: 25176 RVA: 0x00165F8F File Offset: 0x00164F8F
		int UnsafeNativeMethods.IOleContainer.LockContainer(bool fLock)
		{
			return -2147467263;
		}

		// Token: 0x06006259 RID: 25177 RVA: 0x00165F96 File Offset: 0x00164F96
		IntPtr UnsafeNativeMethods.IOleInPlaceFrame.GetWindow()
		{
			return this.parent.Handle;
		}

		// Token: 0x0600625A RID: 25178 RVA: 0x00165FA3 File Offset: 0x00164FA3
		int UnsafeNativeMethods.IOleInPlaceFrame.ContextSensitiveHelp(int fEnterMode)
		{
			return 0;
		}

		// Token: 0x0600625B RID: 25179 RVA: 0x00165FA6 File Offset: 0x00164FA6
		int UnsafeNativeMethods.IOleInPlaceFrame.GetBorder(NativeMethods.COMRECT lprectBorder)
		{
			return -2147467263;
		}

		// Token: 0x0600625C RID: 25180 RVA: 0x00165FAD File Offset: 0x00164FAD
		int UnsafeNativeMethods.IOleInPlaceFrame.RequestBorderSpace(NativeMethods.COMRECT pborderwidths)
		{
			return -2147467263;
		}

		// Token: 0x0600625D RID: 25181 RVA: 0x00165FB4 File Offset: 0x00164FB4
		int UnsafeNativeMethods.IOleInPlaceFrame.SetBorderSpace(NativeMethods.COMRECT pborderwidths)
		{
			return -2147467263;
		}

		// Token: 0x0600625E RID: 25182 RVA: 0x00165FBC File Offset: 0x00164FBC
		int UnsafeNativeMethods.IOleInPlaceFrame.SetActiveObject(UnsafeNativeMethods.IOleInPlaceActiveObject pActiveObject, string pszObjName)
		{
			if (pActiveObject == null)
			{
				if (this.ctlInEditMode != null)
				{
					this.ctlInEditMode.SetEditMode(WebBrowserHelper.AXEditMode.None);
					this.ctlInEditMode = null;
				}
				return 0;
			}
			WebBrowserBase webBrowserBase = null;
			UnsafeNativeMethods.IOleObject oleObject = pActiveObject as UnsafeNativeMethods.IOleObject;
			if (oleObject != null)
			{
				try
				{
					UnsafeNativeMethods.IOleClientSite clientSite = oleObject.GetClientSite();
					WebBrowserSiteBase webBrowserSiteBase = clientSite as WebBrowserSiteBase;
					if (webBrowserSiteBase != null)
					{
						webBrowserBase = webBrowserSiteBase.GetAXHost();
					}
				}
				catch (COMException)
				{
				}
				if (this.ctlInEditMode != null)
				{
					this.ctlInEditMode.SetSelectionStyle(WebBrowserHelper.SelectionStyle.Selected);
					this.ctlInEditMode.SetEditMode(WebBrowserHelper.AXEditMode.None);
				}
				if (webBrowserBase == null)
				{
					this.ctlInEditMode = null;
				}
				else if (!webBrowserBase.IsUserMode)
				{
					this.ctlInEditMode = webBrowserBase;
					webBrowserBase.SetEditMode(WebBrowserHelper.AXEditMode.Object);
					webBrowserBase.AddSelectionHandler();
					webBrowserBase.SetSelectionStyle(WebBrowserHelper.SelectionStyle.Active);
				}
			}
			return 0;
		}

		// Token: 0x0600625F RID: 25183 RVA: 0x00166074 File Offset: 0x00165074
		int UnsafeNativeMethods.IOleInPlaceFrame.InsertMenus(IntPtr hmenuShared, NativeMethods.tagOleMenuGroupWidths lpMenuWidths)
		{
			return 0;
		}

		// Token: 0x06006260 RID: 25184 RVA: 0x00166077 File Offset: 0x00165077
		int UnsafeNativeMethods.IOleInPlaceFrame.SetMenu(IntPtr hmenuShared, IntPtr holemenu, IntPtr hwndActiveObject)
		{
			return -2147467263;
		}

		// Token: 0x06006261 RID: 25185 RVA: 0x0016607E File Offset: 0x0016507E
		int UnsafeNativeMethods.IOleInPlaceFrame.RemoveMenus(IntPtr hmenuShared)
		{
			return -2147467263;
		}

		// Token: 0x06006262 RID: 25186 RVA: 0x00166085 File Offset: 0x00165085
		int UnsafeNativeMethods.IOleInPlaceFrame.SetStatusText(string pszStatusText)
		{
			return -2147467263;
		}

		// Token: 0x06006263 RID: 25187 RVA: 0x0016608C File Offset: 0x0016508C
		int UnsafeNativeMethods.IOleInPlaceFrame.EnableModeless(bool fEnable)
		{
			return -2147467263;
		}

		// Token: 0x06006264 RID: 25188 RVA: 0x00166093 File Offset: 0x00165093
		int UnsafeNativeMethods.IOleInPlaceFrame.TranslateAccelerator(ref NativeMethods.MSG lpmsg, short wID)
		{
			return 1;
		}

		// Token: 0x06006265 RID: 25189 RVA: 0x00166098 File Offset: 0x00165098
		private void ListAXControls(ArrayList list, bool fuseOcx)
		{
			Hashtable hashtable = this.GetComponents();
			if (hashtable == null)
			{
				return;
			}
			Control[] array = new Control[hashtable.Keys.Count];
			hashtable.Keys.CopyTo(array, 0);
			if (array != null)
			{
				foreach (Control control in array)
				{
					WebBrowserBase webBrowserBase = control as WebBrowserBase;
					if (webBrowserBase != null)
					{
						if (fuseOcx)
						{
							object activeXInstance = webBrowserBase.activeXInstance;
							if (activeXInstance != null)
							{
								list.Add(activeXInstance);
							}
						}
						else
						{
							list.Add(control);
						}
					}
				}
			}
		}

		// Token: 0x06006266 RID: 25190 RVA: 0x00166112 File Offset: 0x00165112
		private Hashtable GetComponents()
		{
			return this.GetComponents(this.GetParentsContainer());
		}

		// Token: 0x06006267 RID: 25191 RVA: 0x00166120 File Offset: 0x00165120
		private IContainer GetParentsContainer()
		{
			IContainer parentIContainer = this.GetParentIContainer();
			if (parentIContainer != null)
			{
				return parentIContainer;
			}
			return this.assocContainer;
		}

		// Token: 0x06006268 RID: 25192 RVA: 0x00166140 File Offset: 0x00165140
		private IContainer GetParentIContainer()
		{
			ISite site = this.parent.Site;
			if (site != null && site.DesignMode)
			{
				return site.Container;
			}
			return null;
		}

		// Token: 0x06006269 RID: 25193 RVA: 0x0016616C File Offset: 0x0016516C
		private Hashtable GetComponents(IContainer cont)
		{
			this.FillComponentsTable(cont);
			return this.components;
		}

		// Token: 0x0600626A RID: 25194 RVA: 0x0016617C File Offset: 0x0016517C
		private void FillComponentsTable(IContainer container)
		{
			if (container != null)
			{
				ComponentCollection componentCollection = container.Components;
				if (componentCollection != null)
				{
					this.components = new Hashtable();
					foreach (object obj in componentCollection)
					{
						IComponent component = (IComponent)obj;
						if (component is Control && component != this.parent && component.Site != null)
						{
							this.components.Add(component, component);
						}
					}
					return;
				}
			}
			bool flag = true;
			Control[] array = new Control[this.containerCache.Values.Count];
			this.containerCache.Values.CopyTo(array, 0);
			if (array != null)
			{
				if (array.Length > 0 && this.components == null)
				{
					this.components = new Hashtable();
					flag = false;
				}
				for (int i = 0; i < array.Length; i++)
				{
					if (flag && !this.components.Contains(array[i]))
					{
						this.components.Add(array[i], array[i]);
					}
				}
			}
			this.GetAllChildren(this.parent);
		}

		// Token: 0x0600626B RID: 25195 RVA: 0x0016629C File Offset: 0x0016529C
		private void GetAllChildren(Control ctl)
		{
			if (ctl == null)
			{
				return;
			}
			if (this.components == null)
			{
				this.components = new Hashtable();
			}
			if (ctl != this.parent && !this.components.Contains(ctl))
			{
				this.components.Add(ctl, ctl);
			}
			foreach (object obj in ctl.Controls)
			{
				Control ctl2 = (Control)obj;
				this.GetAllChildren(ctl2);
			}
		}

		// Token: 0x0600626C RID: 25196 RVA: 0x00166330 File Offset: 0x00165330
		private bool RegisterControl(WebBrowserBase ctl)
		{
			ISite site = ctl.Site;
			if (site != null)
			{
				IContainer container = site.Container;
				if (container != null)
				{
					if (this.assocContainer != null)
					{
						return container == this.assocContainer;
					}
					this.assocContainer = container;
					IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						componentChangeService.ComponentRemoved += this.OnComponentRemoved;
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600626D RID: 25197 RVA: 0x00166398 File Offset: 0x00165398
		private void OnComponentRemoved(object sender, ComponentEventArgs e)
		{
			Control control = e.Component as Control;
			if (sender == this.assocContainer && control != null)
			{
				this.RemoveControl(control);
			}
		}

		// Token: 0x0600626E RID: 25198 RVA: 0x001663C4 File Offset: 0x001653C4
		internal void AddControl(Control ctl)
		{
			if (this.containerCache.Contains(ctl))
			{
				throw new ArgumentException(SR.GetString("AXDuplicateControl", new object[]
				{
					this.GetNameForControl(ctl)
				}), "ctl");
			}
			this.containerCache.Add(ctl, ctl);
			if (this.assocContainer == null)
			{
				ISite site = ctl.Site;
				if (site != null)
				{
					this.assocContainer = site.Container;
					IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
					if (componentChangeService != null)
					{
						componentChangeService.ComponentRemoved += this.OnComponentRemoved;
					}
				}
			}
		}

		// Token: 0x0600626F RID: 25199 RVA: 0x0016645C File Offset: 0x0016545C
		internal void RemoveControl(Control ctl)
		{
			this.containerCache.Remove(ctl);
		}

		// Token: 0x06006270 RID: 25200 RVA: 0x0016646C File Offset: 0x0016546C
		internal static WebBrowserContainer FindContainerForControl(WebBrowserBase ctl)
		{
			if (ctl != null)
			{
				if (ctl.container != null)
				{
					return ctl.container;
				}
				ScrollableControl containingControl = ctl.ContainingControl;
				if (containingControl != null)
				{
					WebBrowserContainer webBrowserContainer = ctl.CreateWebBrowserContainer();
					if (webBrowserContainer.RegisterControl(ctl))
					{
						webBrowserContainer.AddControl(ctl);
						return webBrowserContainer;
					}
				}
			}
			return null;
		}

		// Token: 0x06006271 RID: 25201 RVA: 0x001664B0 File Offset: 0x001654B0
		internal string GetNameForControl(Control ctl)
		{
			string text = (ctl.Site != null) ? ctl.Site.Name : ctl.Name;
			return text ?? "";
		}

		// Token: 0x06006272 RID: 25202 RVA: 0x001664E4 File Offset: 0x001654E4
		internal void OnUIActivate(WebBrowserBase site)
		{
			if (this.siteUIActive == site)
			{
				return;
			}
			if (this.siteUIActive != null && this.siteUIActive != site)
			{
				WebBrowserBase webBrowserBase = this.siteUIActive;
				webBrowserBase.AXInPlaceObject.UIDeactivate();
			}
			site.AddSelectionHandler();
			this.siteUIActive = site;
			ContainerControl containingControl = site.ContainingControl;
			if (containingControl != null && containingControl.Contains(site))
			{
				containingControl.SetActiveControlInternal(site);
			}
		}

		// Token: 0x06006273 RID: 25203 RVA: 0x00166546 File Offset: 0x00165546
		internal void OnUIDeactivate(WebBrowserBase site)
		{
			this.siteUIActive = null;
			site.RemoveSelectionHandler();
			site.SetSelectionStyle(WebBrowserHelper.SelectionStyle.Selected);
			site.SetEditMode(WebBrowserHelper.AXEditMode.None);
		}

		// Token: 0x06006274 RID: 25204 RVA: 0x00166564 File Offset: 0x00165564
		internal void OnInPlaceDeactivate(WebBrowserBase site)
		{
			if (this.siteActive == site)
			{
				this.siteActive = null;
				ContainerControl containerControl = this.parent.FindContainerControlInternal();
				if (containerControl != null)
				{
					containerControl.SetActiveControlInternal(null);
				}
			}
		}

		// Token: 0x06006275 RID: 25205 RVA: 0x00166597 File Offset: 0x00165597
		internal void OnExitEditMode(WebBrowserBase ctl)
		{
			if (this.ctlInEditMode == ctl)
			{
				this.ctlInEditMode = null;
			}
		}

		// Token: 0x04003AF5 RID: 15093
		private WebBrowserBase parent;

		// Token: 0x04003AF6 RID: 15094
		private IContainer assocContainer;

		// Token: 0x04003AF7 RID: 15095
		private WebBrowserBase siteUIActive;

		// Token: 0x04003AF8 RID: 15096
		private WebBrowserBase siteActive;

		// Token: 0x04003AF9 RID: 15097
		private Hashtable containerCache = new Hashtable();

		// Token: 0x04003AFA RID: 15098
		private Hashtable components;

		// Token: 0x04003AFB RID: 15099
		private WebBrowserBase ctlInEditMode;
	}
}

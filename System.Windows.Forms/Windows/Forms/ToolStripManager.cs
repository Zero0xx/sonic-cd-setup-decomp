using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x020006AF RID: 1711
	public sealed class ToolStripManager
	{
		// Token: 0x06005998 RID: 22936 RVA: 0x00144F10 File Offset: 0x00143F10
		private static void InitalizeThread()
		{
			if (!ToolStripManager.initialized)
			{
				ToolStripManager.initialized = true;
				ToolStripManager.currentRendererType = ToolStripManager.ProfessionalRendererType;
			}
		}

		// Token: 0x06005999 RID: 22937 RVA: 0x00144F29 File Offset: 0x00143F29
		private ToolStripManager()
		{
		}

		// Token: 0x0600599A RID: 22938 RVA: 0x00144F34 File Offset: 0x00143F34
		static ToolStripManager()
		{
			SystemEvents.UserPreferenceChanging += ToolStripManager.OnUserPreferenceChanging;
		}

		// Token: 0x17001293 RID: 4755
		// (get) Token: 0x0600599B RID: 22939 RVA: 0x00144F80 File Offset: 0x00143F80
		internal static Font DefaultFont
		{
			get
			{
				Font font = ToolStripManager.defaultFont;
				if (font == null)
				{
					lock (ToolStripManager.internalSyncObject)
					{
						font = ToolStripManager.defaultFont;
						if (font == null)
						{
							Font menuFont = SystemFonts.MenuFont;
							if (menuFont == null)
							{
								menuFont = Control.DefaultFont;
							}
							if (menuFont != null)
							{
								if (menuFont.Unit != GraphicsUnit.Point)
								{
									ToolStripManager.defaultFont = ControlPaint.FontInPoints(menuFont);
									font = ToolStripManager.defaultFont;
									menuFont.Dispose();
								}
								else
								{
									ToolStripManager.defaultFont = menuFont;
									font = ToolStripManager.defaultFont;
								}
							}
							return font;
						}
					}
					return font;
				}
				return font;
			}
		}

		// Token: 0x17001294 RID: 4756
		// (get) Token: 0x0600599C RID: 22940 RVA: 0x00145010 File Offset: 0x00144010
		internal static ClientUtils.WeakRefCollection ToolStrips
		{
			get
			{
				if (ToolStripManager.toolStripWeakArrayList == null)
				{
					ToolStripManager.toolStripWeakArrayList = new ClientUtils.WeakRefCollection();
				}
				return ToolStripManager.toolStripWeakArrayList;
			}
		}

		// Token: 0x0600599D RID: 22941 RVA: 0x00145028 File Offset: 0x00144028
		private static void AddEventHandler(int key, Delegate value)
		{
			lock (ToolStripManager.internalSyncObject)
			{
				if (ToolStripManager.staticEventHandlers == null)
				{
					ToolStripManager.staticEventHandlers = new Delegate[1];
				}
				ToolStripManager.staticEventHandlers[key] = Delegate.Combine(ToolStripManager.staticEventHandlers[key], value);
			}
		}

		// Token: 0x0600599E RID: 22942 RVA: 0x00145080 File Offset: 0x00144080
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public static ToolStrip FindToolStrip(string toolStripName)
		{
			ToolStrip result = null;
			for (int i = 0; i < ToolStripManager.ToolStrips.Count; i++)
			{
				if (ToolStripManager.ToolStrips[i] != null && string.Equals(((ToolStrip)ToolStripManager.ToolStrips[i]).Name, toolStripName, StringComparison.Ordinal))
				{
					result = (ToolStrip)ToolStripManager.ToolStrips[i];
					break;
				}
			}
			return result;
		}

		// Token: 0x0600599F RID: 22943 RVA: 0x001450E4 File Offset: 0x001440E4
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		internal static ToolStrip FindToolStrip(Form owningForm, string toolStripName)
		{
			ToolStrip toolStrip = null;
			for (int i = 0; i < ToolStripManager.ToolStrips.Count; i++)
			{
				if (ToolStripManager.ToolStrips[i] != null && string.Equals(((ToolStrip)ToolStripManager.ToolStrips[i]).Name, toolStripName, StringComparison.Ordinal))
				{
					toolStrip = (ToolStrip)ToolStripManager.ToolStrips[i];
					if (toolStrip.FindForm() == owningForm)
					{
						break;
					}
				}
			}
			return toolStrip;
		}

		// Token: 0x060059A0 RID: 22944 RVA: 0x00145150 File Offset: 0x00144150
		private static bool CanChangeSelection(ToolStrip start, ToolStrip toolStrip)
		{
			if (toolStrip == null)
			{
				return false;
			}
			bool flag = !toolStrip.TabStop && toolStrip.Enabled && toolStrip.Visible && !toolStrip.IsDisposed && !toolStrip.Disposing && !toolStrip.IsDropDown && ToolStripManager.IsOnSameWindow(start, toolStrip);
			if (flag)
			{
				foreach (object obj in toolStrip.Items)
				{
					ToolStripItem toolStripItem = (ToolStripItem)obj;
					if (toolStripItem.CanSelect)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060059A1 RID: 22945 RVA: 0x001451F8 File Offset: 0x001441F8
		private static bool ChangeSelection(ToolStrip start, ToolStrip toolStrip)
		{
			if (toolStrip == null || start == null)
			{
				return false;
			}
			if (start == toolStrip)
			{
				return false;
			}
			if (ToolStripManager.ModalMenuFilter.InMenuMode)
			{
				if (ToolStripManager.ModalMenuFilter.GetActiveToolStrip() == start)
				{
					ToolStripManager.ModalMenuFilter.RemoveActiveToolStrip(start);
					start.NotifySelectionChange(null);
				}
				ToolStripManager.ModalMenuFilter.SetActiveToolStrip(toolStrip);
			}
			else
			{
				toolStrip.FocusInternal();
			}
			start.SnapFocusChange(toolStrip);
			toolStrip.SelectNextToolStripItem(null, toolStrip.RightToLeft != RightToLeft.Yes);
			return true;
		}

		// Token: 0x060059A2 RID: 22946 RVA: 0x0014525C File Offset: 0x0014425C
		private static Delegate GetEventHandler(int key)
		{
			Delegate result;
			lock (ToolStripManager.internalSyncObject)
			{
				if (ToolStripManager.staticEventHandlers == null)
				{
					result = null;
				}
				else
				{
					result = ToolStripManager.staticEventHandlers[key];
				}
			}
			return result;
		}

		// Token: 0x060059A3 RID: 22947 RVA: 0x001452A4 File Offset: 0x001442A4
		private static bool IsOnSameWindow(Control control1, Control control2)
		{
			return WindowsFormsUtils.GetRootHWnd(control1).Handle == WindowsFormsUtils.GetRootHWnd(control2).Handle;
		}

		// Token: 0x060059A4 RID: 22948 RVA: 0x001452D2 File Offset: 0x001442D2
		internal static bool IsThreadUsingToolStrips()
		{
			return ToolStripManager.toolStripWeakArrayList != null && ToolStripManager.toolStripWeakArrayList.Count > 0;
		}

		// Token: 0x060059A5 RID: 22949 RVA: 0x001452EC File Offset: 0x001442EC
		private static void OnUserPreferenceChanging(object sender, UserPreferenceChangingEventArgs e)
		{
			if (e.Category == UserPreferenceCategory.Window)
			{
				lock (ToolStripManager.internalSyncObject)
				{
					ToolStripManager.defaultFont = null;
				}
			}
		}

		// Token: 0x060059A6 RID: 22950 RVA: 0x00145330 File Offset: 0x00144330
		internal static void NotifyMenuModeChange(bool invalidateText, bool activationChange)
		{
			bool flag = false;
			for (int i = 0; i < ToolStripManager.ToolStrips.Count; i++)
			{
				ToolStrip toolStrip = ToolStripManager.ToolStrips[i] as ToolStrip;
				if (toolStrip == null)
				{
					flag = true;
				}
				else
				{
					if (invalidateText)
					{
						toolStrip.InvalidateTextItems();
					}
					if (activationChange)
					{
						toolStrip.KeyboardActive = false;
					}
				}
			}
			if (flag)
			{
				ToolStripManager.PruneToolStripList();
			}
		}

		// Token: 0x060059A7 RID: 22951 RVA: 0x00145388 File Offset: 0x00144388
		internal static void PruneToolStripList()
		{
			if (ToolStripManager.toolStripWeakArrayList != null && ToolStripManager.toolStripWeakArrayList.Count > 0)
			{
				for (int i = ToolStripManager.toolStripWeakArrayList.Count - 1; i >= 0; i--)
				{
					if (ToolStripManager.toolStripWeakArrayList[i] == null)
					{
						ToolStripManager.toolStripWeakArrayList.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x060059A8 RID: 22952 RVA: 0x001453D8 File Offset: 0x001443D8
		private static void RemoveEventHandler(int key, Delegate value)
		{
			lock (ToolStripManager.internalSyncObject)
			{
				if (ToolStripManager.staticEventHandlers != null)
				{
					ToolStripManager.staticEventHandlers[key] = Delegate.Remove(ToolStripManager.staticEventHandlers[key], value);
				}
			}
		}

		// Token: 0x060059A9 RID: 22953 RVA: 0x00145428 File Offset: 0x00144428
		internal static bool SelectNextToolStrip(ToolStrip start, bool forward)
		{
			if (start == null || start.ParentInternal == null)
			{
				return false;
			}
			ToolStrip toolStrip = null;
			ToolStrip toolStrip2 = null;
			int tabIndex = start.TabIndex;
			int num = ToolStripManager.ToolStrips.IndexOf(start);
			int count = ToolStripManager.ToolStrips.Count;
			for (int i = 0; i < count; i++)
			{
				num = (forward ? ((num + 1) % count) : ((num + count - 1) % count));
				ToolStrip toolStrip3 = ToolStripManager.ToolStrips[num] as ToolStrip;
				if (toolStrip3 != null && toolStrip3 != start)
				{
					int tabIndex2 = toolStrip3.TabIndex;
					if (forward)
					{
						if (tabIndex2 >= tabIndex && ToolStripManager.CanChangeSelection(start, toolStrip3))
						{
							if (toolStrip2 == null)
							{
								toolStrip2 = toolStrip3;
							}
							else if (toolStrip3.TabIndex < toolStrip2.TabIndex)
							{
								toolStrip2 = toolStrip3;
							}
						}
						else if ((toolStrip == null || toolStrip3.TabIndex < toolStrip.TabIndex) && ToolStripManager.CanChangeSelection(start, toolStrip3))
						{
							toolStrip = toolStrip3;
						}
					}
					else if (tabIndex2 <= tabIndex && ToolStripManager.CanChangeSelection(start, toolStrip3))
					{
						if (toolStrip2 == null)
						{
							toolStrip2 = toolStrip3;
						}
						else if (toolStrip3.TabIndex > toolStrip2.TabIndex)
						{
							toolStrip2 = toolStrip3;
						}
					}
					else if ((toolStrip == null || toolStrip3.TabIndex > toolStrip.TabIndex) && ToolStripManager.CanChangeSelection(start, toolStrip3))
					{
						toolStrip = toolStrip3;
					}
					if (toolStrip2 != null && Math.Abs(toolStrip2.TabIndex - tabIndex) <= 1)
					{
						break;
					}
				}
			}
			if (toolStrip2 != null)
			{
				return ToolStripManager.ChangeSelection(start, toolStrip2);
			}
			return toolStrip != null && ToolStripManager.ChangeSelection(start, toolStrip);
		}

		// Token: 0x17001295 RID: 4757
		// (get) Token: 0x060059AA RID: 22954 RVA: 0x0014557E File Offset: 0x0014457E
		// (set) Token: 0x060059AB RID: 22955 RVA: 0x0014558A File Offset: 0x0014458A
		private static Type CurrentRendererType
		{
			get
			{
				ToolStripManager.InitalizeThread();
				return ToolStripManager.currentRendererType;
			}
			set
			{
				ToolStripManager.currentRendererType = value;
			}
		}

		// Token: 0x17001296 RID: 4758
		// (get) Token: 0x060059AC RID: 22956 RVA: 0x00145592 File Offset: 0x00144592
		private static Type DefaultRendererType
		{
			get
			{
				return ToolStripManager.ProfessionalRendererType;
			}
		}

		// Token: 0x17001297 RID: 4759
		// (get) Token: 0x060059AD RID: 22957 RVA: 0x00145599 File Offset: 0x00144599
		// (set) Token: 0x060059AE RID: 22958 RVA: 0x001455B8 File Offset: 0x001445B8
		public static ToolStripRenderer Renderer
		{
			get
			{
				if (ToolStripManager.defaultRenderer == null)
				{
					ToolStripManager.defaultRenderer = ToolStripManager.CreateRenderer(ToolStripManager.RenderMode);
				}
				return ToolStripManager.defaultRenderer;
			}
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			set
			{
				if (ToolStripManager.defaultRenderer != value)
				{
					ToolStripManager.CurrentRendererType = ((value == null) ? ToolStripManager.DefaultRendererType : value.GetType());
					ToolStripManager.defaultRenderer = value;
					EventHandler eventHandler = (EventHandler)ToolStripManager.GetEventHandler(0);
					if (eventHandler != null)
					{
						eventHandler(null, EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x1400035F RID: 863
		// (add) Token: 0x060059AF RID: 22959 RVA: 0x00145603 File Offset: 0x00144603
		// (remove) Token: 0x060059B0 RID: 22960 RVA: 0x0014560C File Offset: 0x0014460C
		public static event EventHandler RendererChanged
		{
			add
			{
				ToolStripManager.AddEventHandler(0, value);
			}
			remove
			{
				ToolStripManager.RemoveEventHandler(0, value);
			}
		}

		// Token: 0x17001298 RID: 4760
		// (get) Token: 0x060059B1 RID: 22961 RVA: 0x00145618 File Offset: 0x00144618
		// (set) Token: 0x060059B2 RID: 22962 RVA: 0x00145658 File Offset: 0x00144658
		public static ToolStripManagerRenderMode RenderMode
		{
			get
			{
				Type type = ToolStripManager.CurrentRendererType;
				if (ToolStripManager.defaultRenderer != null && !ToolStripManager.defaultRenderer.IsAutoGenerated)
				{
					return ToolStripManagerRenderMode.Custom;
				}
				if (type == ToolStripManager.ProfessionalRendererType)
				{
					return ToolStripManagerRenderMode.Professional;
				}
				if (type == ToolStripManager.SystemRendererType)
				{
					return ToolStripManagerRenderMode.System;
				}
				return ToolStripManagerRenderMode.Custom;
			}
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolStripManagerRenderMode));
				}
				switch (value)
				{
				case ToolStripManagerRenderMode.Custom:
					throw new NotSupportedException(SR.GetString("ToolStripRenderModeUseRendererPropertyInstead"));
				case ToolStripManagerRenderMode.System:
				case ToolStripManagerRenderMode.Professional:
					ToolStripManager.Renderer = ToolStripManager.CreateRenderer(value);
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x060059B3 RID: 22963 RVA: 0x001456BB File Offset: 0x001446BB
		// (set) Token: 0x060059B4 RID: 22964 RVA: 0x001456CC File Offset: 0x001446CC
		public static bool VisualStylesEnabled
		{
			get
			{
				return ToolStripManager.visualStylesEnabledIfPossible && Application.RenderWithVisualStyles;
			}
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			set
			{
				bool visualStylesEnabled = ToolStripManager.VisualStylesEnabled;
				ToolStripManager.visualStylesEnabledIfPossible = value;
				if (visualStylesEnabled != ToolStripManager.VisualStylesEnabled)
				{
					EventHandler eventHandler = (EventHandler)ToolStripManager.GetEventHandler(0);
					if (eventHandler != null)
					{
						eventHandler(null, EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x060059B5 RID: 22965 RVA: 0x00145708 File Offset: 0x00144708
		internal static ToolStripRenderer CreateRenderer(ToolStripManagerRenderMode renderMode)
		{
			switch (renderMode)
			{
			case ToolStripManagerRenderMode.System:
				return new ToolStripSystemRenderer(true);
			case ToolStripManagerRenderMode.Professional:
				return new ToolStripProfessionalRenderer(true);
			}
			return new ToolStripSystemRenderer(true);
		}

		// Token: 0x060059B6 RID: 22966 RVA: 0x00145740 File Offset: 0x00144740
		internal static ToolStripRenderer CreateRenderer(ToolStripRenderMode renderMode)
		{
			switch (renderMode)
			{
			case ToolStripRenderMode.System:
				return new ToolStripSystemRenderer(true);
			case ToolStripRenderMode.Professional:
				return new ToolStripProfessionalRenderer(true);
			}
			return new ToolStripSystemRenderer(true);
		}

		// Token: 0x1700129A RID: 4762
		// (get) Token: 0x060059B7 RID: 22967 RVA: 0x00145777 File Offset: 0x00144777
		internal static ClientUtils.WeakRefCollection ToolStripPanels
		{
			get
			{
				if (ToolStripManager.toolStripPanelWeakArrayList == null)
				{
					ToolStripManager.toolStripPanelWeakArrayList = new ClientUtils.WeakRefCollection();
				}
				return ToolStripManager.toolStripPanelWeakArrayList;
			}
		}

		// Token: 0x060059B8 RID: 22968 RVA: 0x00145790 File Offset: 0x00144790
		internal static ToolStripPanel ToolStripPanelFromPoint(Control draggedControl, Point screenLocation)
		{
			if (ToolStripManager.toolStripPanelWeakArrayList != null)
			{
				ISupportToolStripPanel supportToolStripPanel = draggedControl as ISupportToolStripPanel;
				bool isCurrentlyDragging = supportToolStripPanel.IsCurrentlyDragging;
				for (int i = 0; i < ToolStripManager.toolStripPanelWeakArrayList.Count; i++)
				{
					ToolStripPanel toolStripPanel = ToolStripManager.toolStripPanelWeakArrayList[i] as ToolStripPanel;
					if (toolStripPanel != null && toolStripPanel.IsHandleCreated && toolStripPanel.Visible && toolStripPanel.DragBounds.Contains(toolStripPanel.PointToClient(screenLocation)))
					{
						if (!isCurrentlyDragging)
						{
							return toolStripPanel;
						}
						if (ToolStripManager.IsOnSameWindow(draggedControl, toolStripPanel))
						{
							return toolStripPanel;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060059B9 RID: 22969 RVA: 0x00145814 File Offset: 0x00144814
		public static void LoadSettings(Form targetForm)
		{
			if (targetForm == null)
			{
				throw new ArgumentNullException("targetForm");
			}
			ToolStripManager.LoadSettings(targetForm, targetForm.GetType().FullName);
		}

		// Token: 0x060059BA RID: 22970 RVA: 0x00145838 File Offset: 0x00144838
		public static void LoadSettings(Form targetForm, string key)
		{
			if (targetForm == null)
			{
				throw new ArgumentNullException("targetForm");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			ToolStripSettingsManager toolStripSettingsManager = new ToolStripSettingsManager(targetForm, key);
			toolStripSettingsManager.Load();
		}

		// Token: 0x060059BB RID: 22971 RVA: 0x00145874 File Offset: 0x00144874
		public static void SaveSettings(Form sourceForm)
		{
			if (sourceForm == null)
			{
				throw new ArgumentNullException("sourceForm");
			}
			ToolStripManager.SaveSettings(sourceForm, sourceForm.GetType().FullName);
		}

		// Token: 0x060059BC RID: 22972 RVA: 0x00145898 File Offset: 0x00144898
		public static void SaveSettings(Form sourceForm, string key)
		{
			if (sourceForm == null)
			{
				throw new ArgumentNullException("sourceForm");
			}
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			ToolStripSettingsManager toolStripSettingsManager = new ToolStripSettingsManager(sourceForm, key);
			toolStripSettingsManager.Save();
		}

		// Token: 0x1700129B RID: 4763
		// (get) Token: 0x060059BD RID: 22973 RVA: 0x001458D4 File Offset: 0x001448D4
		internal static bool ShowMenuFocusCues
		{
			get
			{
				return DisplayInformation.MenuAccessKeysUnderlined || ToolStripManager.ModalMenuFilter.Instance.ShowUnderlines;
			}
		}

		// Token: 0x060059BE RID: 22974 RVA: 0x001458EC File Offset: 0x001448EC
		public static bool IsValidShortcut(Keys shortcut)
		{
			Keys keys = shortcut & Keys.KeyCode;
			Keys keys2 = shortcut & Keys.Modifiers;
			if (shortcut == Keys.None)
			{
				return false;
			}
			if (keys == Keys.Delete || keys == Keys.Insert)
			{
				return true;
			}
			if (keys >= Keys.F1 && keys <= Keys.F24)
			{
				return true;
			}
			if (keys == Keys.None || keys2 == Keys.None)
			{
				return false;
			}
			switch (keys)
			{
			case Keys.ShiftKey:
			case Keys.ControlKey:
			case Keys.Menu:
				return false;
			default:
				return keys2 != Keys.Shift;
			}
		}

		// Token: 0x060059BF RID: 22975 RVA: 0x00145958 File Offset: 0x00144958
		internal static bool IsMenuKey(Keys keyData)
		{
			Keys keys = keyData & Keys.KeyCode;
			return Keys.Menu == keys || Keys.F10 == keys;
		}

		// Token: 0x060059C0 RID: 22976 RVA: 0x0014597C File Offset: 0x0014497C
		public static bool IsShortcutDefined(Keys shortcut)
		{
			for (int i = 0; i < ToolStripManager.ToolStrips.Count; i++)
			{
				ToolStrip toolStrip = ToolStripManager.ToolStrips[i] as ToolStrip;
				if (toolStrip != null && toolStrip.Shortcuts.Contains(shortcut))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060059C1 RID: 22977 RVA: 0x001459C8 File Offset: 0x001449C8
		internal static bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if (ToolStripManager.IsValidShortcut(keyData))
			{
				return ToolStripManager.ProcessShortcut(ref m, keyData);
			}
			if (m.Msg == 260)
			{
				ToolStripManager.ModalMenuFilter.ProcessMenuKeyDown(ref m);
			}
			return false;
		}

		// Token: 0x060059C2 RID: 22978 RVA: 0x001459F0 File Offset: 0x001449F0
		internal static bool ProcessShortcut(ref Message m, Keys shortcut)
		{
			if (!ToolStripManager.IsThreadUsingToolStrips())
			{
				return false;
			}
			Control control = Control.FromChildHandleInternal(m.HWnd);
			Control control2 = control;
			if (control2 != null && ToolStripManager.IsValidShortcut(shortcut))
			{
				for (;;)
				{
					if (control2.ContextMenuStrip != null && control2.ContextMenuStrip.Shortcuts.ContainsKey(shortcut))
					{
						ToolStripMenuItem toolStripMenuItem = control2.ContextMenuStrip.Shortcuts[shortcut] as ToolStripMenuItem;
						if (toolStripMenuItem.ProcessCmdKey(ref m, shortcut))
						{
							break;
						}
					}
					control2 = control2.ParentInternal;
					if (control2 == null)
					{
						goto Block_6;
					}
				}
				return true;
				Block_6:
				if (control2 != null)
				{
					control = control2;
				}
				bool result = false;
				bool flag = false;
				for (int i = 0; i < ToolStripManager.ToolStrips.Count; i++)
				{
					ToolStrip toolStrip = ToolStripManager.ToolStrips[i] as ToolStrip;
					bool flag2 = false;
					bool flag3 = false;
					if (toolStrip == null)
					{
						flag = true;
					}
					else if ((control == null || toolStrip != control.ContextMenuStrip) && toolStrip.Shortcuts.ContainsKey(shortcut))
					{
						if (toolStrip.IsDropDown)
						{
							ToolStripDropDown toolStripDropDown = toolStrip as ToolStripDropDown;
							ContextMenuStrip contextMenuStrip = toolStripDropDown.GetFirstDropDown() as ContextMenuStrip;
							if (contextMenuStrip != null)
							{
								flag3 = contextMenuStrip.IsAssignedToDropDownItem;
								if (!flag3)
								{
									if (contextMenuStrip != control.ContextMenuStrip)
									{
										goto IL_1D2;
									}
									flag2 = true;
								}
							}
						}
						bool flag4 = false;
						if (!flag2)
						{
							ToolStrip toplevelOwnerToolStrip = toolStrip.GetToplevelOwnerToolStrip();
							if (toplevelOwnerToolStrip != null && control != null)
							{
								HandleRef rootHWnd = WindowsFormsUtils.GetRootHWnd(toplevelOwnerToolStrip);
								HandleRef rootHWnd2 = WindowsFormsUtils.GetRootHWnd(control);
								flag4 = (rootHWnd.Handle == rootHWnd2.Handle);
								if (flag4)
								{
									Form form = Control.FromHandleInternal(rootHWnd2.Handle) as Form;
									if (form != null && form.IsMdiContainer)
									{
										Form form2 = toplevelOwnerToolStrip.FindFormInternal();
										if (form2 != form && form2 != null)
										{
											flag4 = (form2 == form.ActiveMdiChildInternal);
										}
									}
								}
							}
						}
						if (flag2 || flag4 || flag3)
						{
							ToolStripMenuItem toolStripMenuItem2 = toolStrip.Shortcuts[shortcut] as ToolStripMenuItem;
							if (toolStripMenuItem2 != null && toolStripMenuItem2.ProcessCmdKey(ref m, shortcut))
							{
								result = true;
								break;
							}
						}
					}
					IL_1D2:;
				}
				if (flag)
				{
					ToolStripManager.PruneToolStripList();
				}
				return result;
			}
			return false;
		}

		// Token: 0x060059C3 RID: 22979 RVA: 0x00145BF4 File Offset: 0x00144BF4
		internal static bool ProcessMenuKey(ref Message m)
		{
			if (!ToolStripManager.IsThreadUsingToolStrips())
			{
				return false;
			}
			Keys keys = (Keys)((int)m.LParam);
			Control control = Control.FromHandleInternal(m.HWnd);
			Control control2 = null;
			MenuStrip menuStrip = null;
			if (control != null)
			{
				control2 = control.TopLevelControlInternal;
				if (control2 != null)
				{
					IntPtr menu = UnsafeNativeMethods.GetMenu(new HandleRef(control2, control2.Handle));
					if (menu == IntPtr.Zero)
					{
						menuStrip = ToolStripManager.GetMainMenuStrip(control2);
					}
				}
			}
			if ((ushort)keys == 32)
			{
				ToolStripManager.ModalMenuFilter.MenuKeyToggle = false;
			}
			else if ((ushort)keys == 45)
			{
				Form form = control2 as Form;
				if (form != null && form.IsMdiChild && form.WindowState == FormWindowState.Maximized)
				{
					ToolStripManager.ModalMenuFilter.MenuKeyToggle = false;
				}
			}
			else
			{
				if (UnsafeNativeMethods.GetKeyState(16) < 0 && keys == Keys.None)
				{
					return ToolStripManager.ModalMenuFilter.InMenuMode;
				}
				if (menuStrip != null && !ToolStripManager.ModalMenuFilter.MenuKeyToggle)
				{
					HandleRef rootHWnd = WindowsFormsUtils.GetRootHWnd(menuStrip);
					IntPtr foregroundWindow = UnsafeNativeMethods.GetForegroundWindow();
					if (rootHWnd.Handle == foregroundWindow)
					{
						return menuStrip.OnMenuKey();
					}
				}
				else if (menuStrip != null)
				{
					ToolStripManager.ModalMenuFilter.MenuKeyToggle = false;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060059C4 RID: 22980 RVA: 0x00145CE8 File Offset: 0x00144CE8
		internal static MenuStrip GetMainMenuStrip(Control control)
		{
			if (control == null)
			{
				return null;
			}
			Form form = control.FindFormInternal();
			if (form != null && form.MainMenuStrip != null)
			{
				return form.MainMenuStrip;
			}
			return ToolStripManager.GetFirstMenuStripRecursive(control.Controls);
		}

		// Token: 0x060059C5 RID: 22981 RVA: 0x00145D20 File Offset: 0x00144D20
		private static MenuStrip GetFirstMenuStripRecursive(Control.ControlCollection controlsToLookIn)
		{
			try
			{
				for (int i = 0; i < controlsToLookIn.Count; i++)
				{
					if (controlsToLookIn[i] != null && controlsToLookIn[i] is MenuStrip)
					{
						return controlsToLookIn[i] as MenuStrip;
					}
				}
				for (int j = 0; j < controlsToLookIn.Count; j++)
				{
					if (controlsToLookIn[j] != null && controlsToLookIn[j].Controls != null && controlsToLookIn[j].Controls.Count > 0)
					{
						MenuStrip firstMenuStripRecursive = ToolStripManager.GetFirstMenuStripRecursive(controlsToLookIn[j].Controls);
						if (firstMenuStripRecursive != null)
						{
							return firstMenuStripRecursive;
						}
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
			return null;
		}

		// Token: 0x060059C6 RID: 22982 RVA: 0x00145DE0 File Offset: 0x00144DE0
		private static ToolStripItem FindMatch(ToolStripItem source, ToolStripItemCollection destinationItems)
		{
			ToolStripItem toolStripItem = null;
			if (source != null)
			{
				for (int i = 0; i < destinationItems.Count; i++)
				{
					ToolStripItem toolStripItem2 = destinationItems[i];
					if (WindowsFormsUtils.SafeCompareStrings(source.Text, toolStripItem2.Text, true))
					{
						toolStripItem = toolStripItem2;
						break;
					}
				}
				if (toolStripItem == null && source.MergeIndex > -1 && source.MergeIndex < destinationItems.Count)
				{
					toolStripItem = destinationItems[source.MergeIndex];
				}
			}
			return toolStripItem;
		}

		// Token: 0x060059C7 RID: 22983 RVA: 0x00145E4C File Offset: 0x00144E4C
		internal static ArrayList FindMergeableToolStrips(ContainerControl container)
		{
			ArrayList arrayList = new ArrayList();
			if (container != null)
			{
				for (int i = 0; i < ToolStripManager.ToolStrips.Count; i++)
				{
					ToolStrip toolStrip = (ToolStrip)ToolStripManager.ToolStrips[i];
					if (toolStrip != null && toolStrip.AllowMerge && container == toolStrip.FindFormInternal())
					{
						arrayList.Add(toolStrip);
					}
				}
			}
			arrayList.Sort(new ToolStripCustomIComparer());
			return arrayList;
		}

		// Token: 0x060059C8 RID: 22984 RVA: 0x00145EB0 File Offset: 0x00144EB0
		private static bool IsSpecialMDIStrip(ToolStrip toolStrip)
		{
			return toolStrip is MdiControlStrip || toolStrip is MdiWindowListStrip;
		}

		// Token: 0x060059C9 RID: 22985 RVA: 0x00145EC8 File Offset: 0x00144EC8
		public static bool Merge(ToolStrip sourceToolStrip, ToolStrip targetToolStrip)
		{
			if (sourceToolStrip == null)
			{
				throw new ArgumentNullException("sourceToolStrip");
			}
			if (targetToolStrip == null)
			{
				throw new ArgumentNullException("targetToolStrip");
			}
			if (targetToolStrip == sourceToolStrip)
			{
				throw new ArgumentException(SR.GetString("ToolStripMergeImpossibleIdentical"));
			}
			bool flag = ToolStripManager.IsSpecialMDIStrip(sourceToolStrip) || (sourceToolStrip.AllowMerge && targetToolStrip.AllowMerge && (sourceToolStrip.GetType().IsAssignableFrom(targetToolStrip.GetType()) || targetToolStrip.GetType().IsAssignableFrom(sourceToolStrip.GetType())));
			MergeHistory mergeHistory = null;
			if (flag)
			{
				mergeHistory = new MergeHistory(sourceToolStrip);
				int count = sourceToolStrip.Items.Count;
				if (count > 0)
				{
					sourceToolStrip.SuspendLayout();
					targetToolStrip.SuspendLayout();
					try
					{
						int num = count;
						int i = 0;
						int num2 = 0;
						while (i < count)
						{
							ToolStripItem source = sourceToolStrip.Items[num2];
							ToolStripManager.MergeRecursive(source, targetToolStrip.Items, mergeHistory.MergeHistoryItemsStack);
							int num3 = num - sourceToolStrip.Items.Count;
							num2 = ((num3 > 0) ? num2 : (num2 + 1));
							num = sourceToolStrip.Items.Count;
							i++;
						}
					}
					finally
					{
						sourceToolStrip.ResumeLayout();
						targetToolStrip.ResumeLayout();
					}
					if (mergeHistory.MergeHistoryItemsStack.Count > 0)
					{
						targetToolStrip.MergeHistoryStack.Push(mergeHistory);
					}
				}
			}
			bool result = false;
			if (mergeHistory != null && mergeHistory.MergeHistoryItemsStack.Count > 0)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060059CA RID: 22986 RVA: 0x00146030 File Offset: 0x00145030
		private static void MergeRecursive(ToolStripItem source, ToolStripItemCollection destinationItems, Stack<MergeHistoryItem> history)
		{
			switch (source.MergeAction)
			{
			case MergeAction.Append:
			{
				MergeHistoryItem mergeHistoryItem = new MergeHistoryItem(MergeAction.Remove);
				mergeHistoryItem.PreviousIndexCollection = source.Owner.Items;
				mergeHistoryItem.PreviousIndex = mergeHistoryItem.PreviousIndexCollection.IndexOf(source);
				mergeHistoryItem.TargetItem = source;
				int index = destinationItems.Add(source);
				mergeHistoryItem.Index = index;
				mergeHistoryItem.IndexCollection = destinationItems;
				history.Push(mergeHistoryItem);
				break;
			}
			case MergeAction.Insert:
				if (source.MergeIndex > -1)
				{
					MergeHistoryItem mergeHistoryItem = new MergeHistoryItem(MergeAction.Remove);
					mergeHistoryItem.PreviousIndexCollection = source.Owner.Items;
					mergeHistoryItem.PreviousIndex = mergeHistoryItem.PreviousIndexCollection.IndexOf(source);
					mergeHistoryItem.TargetItem = source;
					int index2 = Math.Min(destinationItems.Count, source.MergeIndex);
					destinationItems.Insert(index2, source);
					mergeHistoryItem.IndexCollection = destinationItems;
					mergeHistoryItem.Index = index2;
					history.Push(mergeHistoryItem);
					return;
				}
				break;
			case MergeAction.Replace:
			case MergeAction.Remove:
			case MergeAction.MatchOnly:
			{
				ToolStripItem toolStripItem = ToolStripManager.FindMatch(source, destinationItems);
				if (toolStripItem != null)
				{
					switch (source.MergeAction)
					{
					case MergeAction.Replace:
					case MergeAction.Remove:
						break;
					case MergeAction.MatchOnly:
					{
						ToolStripDropDownItem toolStripDropDownItem = toolStripItem as ToolStripDropDownItem;
						ToolStripDropDownItem toolStripDropDownItem2 = source as ToolStripDropDownItem;
						if (toolStripDropDownItem == null || toolStripDropDownItem2 == null || toolStripDropDownItem2.DropDownItems.Count == 0)
						{
							return;
						}
						int count = toolStripDropDownItem2.DropDownItems.Count;
						if (count <= 0)
						{
							return;
						}
						int num = count;
						toolStripDropDownItem2.DropDown.SuspendLayout();
						try
						{
							int i = 0;
							int num2 = 0;
							while (i < count)
							{
								ToolStripManager.MergeRecursive(toolStripDropDownItem2.DropDownItems[num2], toolStripDropDownItem.DropDownItems, history);
								int num3 = num - toolStripDropDownItem2.DropDownItems.Count;
								num2 = ((num3 > 0) ? num2 : (num2 + 1));
								num = toolStripDropDownItem2.DropDownItems.Count;
								i++;
							}
							return;
						}
						finally
						{
							toolStripDropDownItem2.DropDown.ResumeLayout();
						}
						break;
					}
					default:
						return;
					}
					MergeHistoryItem mergeHistoryItem = new MergeHistoryItem(MergeAction.Insert);
					mergeHistoryItem.TargetItem = toolStripItem;
					int index3 = destinationItems.IndexOf(toolStripItem);
					destinationItems.RemoveAt(index3);
					mergeHistoryItem.Index = index3;
					mergeHistoryItem.IndexCollection = destinationItems;
					mergeHistoryItem.TargetItem = toolStripItem;
					history.Push(mergeHistoryItem);
					if (source.MergeAction == MergeAction.Replace)
					{
						mergeHistoryItem = new MergeHistoryItem(MergeAction.Remove);
						mergeHistoryItem.PreviousIndexCollection = source.Owner.Items;
						mergeHistoryItem.PreviousIndex = mergeHistoryItem.PreviousIndexCollection.IndexOf(source);
						mergeHistoryItem.TargetItem = source;
						destinationItems.Insert(index3, source);
						mergeHistoryItem.Index = index3;
						mergeHistoryItem.IndexCollection = destinationItems;
						history.Push(mergeHistoryItem);
						return;
					}
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x060059CB RID: 22987 RVA: 0x001462B0 File Offset: 0x001452B0
		public static bool Merge(ToolStrip sourceToolStrip, string targetName)
		{
			if (sourceToolStrip == null)
			{
				throw new ArgumentNullException("sourceToolStrip");
			}
			if (targetName == null)
			{
				throw new ArgumentNullException("targetName");
			}
			ToolStrip toolStrip = ToolStripManager.FindToolStrip(targetName);
			return toolStrip != null && ToolStripManager.Merge(sourceToolStrip, toolStrip);
		}

		// Token: 0x060059CC RID: 22988 RVA: 0x001462EC File Offset: 0x001452EC
		internal static bool RevertMergeInternal(ToolStrip targetToolStrip, ToolStrip sourceToolStrip, bool revertMDIControls)
		{
			bool result = false;
			if (targetToolStrip == null)
			{
				throw new ArgumentNullException("targetToolStrip");
			}
			if (targetToolStrip == sourceToolStrip)
			{
				throw new ArgumentException(SR.GetString("ToolStripMergeImpossibleIdentical"));
			}
			bool flag = false;
			if (sourceToolStrip != null)
			{
				foreach (MergeHistory mergeHistory in targetToolStrip.MergeHistoryStack)
				{
					flag = (mergeHistory.MergedToolStrip == sourceToolStrip);
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (sourceToolStrip != null)
			{
				sourceToolStrip.SuspendLayout();
			}
			targetToolStrip.SuspendLayout();
			try
			{
				Stack<ToolStrip> stack = new Stack<ToolStrip>();
				flag = false;
				while (targetToolStrip.MergeHistoryStack.Count > 0)
				{
					if (flag)
					{
						break;
					}
					result = true;
					MergeHistory mergeHistory2 = targetToolStrip.MergeHistoryStack.Pop();
					if (mergeHistory2.MergedToolStrip == sourceToolStrip)
					{
						flag = true;
					}
					else if (!revertMDIControls && sourceToolStrip == null)
					{
						if (ToolStripManager.IsSpecialMDIStrip(mergeHistory2.MergedToolStrip))
						{
							stack.Push(mergeHistory2.MergedToolStrip);
						}
					}
					else
					{
						stack.Push(mergeHistory2.MergedToolStrip);
					}
					while (mergeHistory2.MergeHistoryItemsStack.Count > 0)
					{
						MergeHistoryItem mergeHistoryItem = mergeHistory2.MergeHistoryItemsStack.Pop();
						switch (mergeHistoryItem.MergeAction)
						{
						case MergeAction.Insert:
							mergeHistoryItem.IndexCollection.Insert(Math.Min(mergeHistoryItem.Index, mergeHistoryItem.IndexCollection.Count), mergeHistoryItem.TargetItem);
							break;
						case MergeAction.Remove:
							mergeHistoryItem.IndexCollection.Remove(mergeHistoryItem.TargetItem);
							mergeHistoryItem.PreviousIndexCollection.Insert(Math.Min(mergeHistoryItem.PreviousIndex, mergeHistoryItem.PreviousIndexCollection.Count), mergeHistoryItem.TargetItem);
							break;
						}
					}
				}
				while (stack.Count > 0)
				{
					ToolStrip sourceToolStrip2 = stack.Pop();
					ToolStripManager.Merge(sourceToolStrip2, targetToolStrip);
				}
			}
			finally
			{
				if (sourceToolStrip != null)
				{
					sourceToolStrip.ResumeLayout();
				}
				targetToolStrip.ResumeLayout();
			}
			return result;
		}

		// Token: 0x060059CD RID: 22989 RVA: 0x00146500 File Offset: 0x00145500
		public static bool RevertMerge(ToolStrip targetToolStrip)
		{
			return ToolStripManager.RevertMergeInternal(targetToolStrip, null, false);
		}

		// Token: 0x060059CE RID: 22990 RVA: 0x0014650A File Offset: 0x0014550A
		public static bool RevertMerge(ToolStrip targetToolStrip, ToolStrip sourceToolStrip)
		{
			if (sourceToolStrip == null)
			{
				throw new ArgumentNullException("sourceToolStrip");
			}
			return ToolStripManager.RevertMergeInternal(targetToolStrip, sourceToolStrip, false);
		}

		// Token: 0x060059CF RID: 22991 RVA: 0x00146524 File Offset: 0x00145524
		public static bool RevertMerge(string targetName)
		{
			ToolStrip toolStrip = ToolStripManager.FindToolStrip(targetName);
			return toolStrip != null && ToolStripManager.RevertMerge(toolStrip);
		}

		// Token: 0x04003881 RID: 14465
		private const int staticEventDefaultRendererChanged = 0;

		// Token: 0x04003882 RID: 14466
		private const int staticEventCount = 1;

		// Token: 0x04003883 RID: 14467
		[ThreadStatic]
		private static ClientUtils.WeakRefCollection toolStripWeakArrayList;

		// Token: 0x04003884 RID: 14468
		[ThreadStatic]
		private static ClientUtils.WeakRefCollection toolStripPanelWeakArrayList;

		// Token: 0x04003885 RID: 14469
		[ThreadStatic]
		private static bool initialized;

		// Token: 0x04003886 RID: 14470
		private static Font defaultFont;

		// Token: 0x04003887 RID: 14471
		[ThreadStatic]
		private static Delegate[] staticEventHandlers;

		// Token: 0x04003888 RID: 14472
		private static object internalSyncObject = new object();

		// Token: 0x04003889 RID: 14473
		[ThreadStatic]
		private static ToolStripRenderer defaultRenderer;

		// Token: 0x0400388A RID: 14474
		internal static Type SystemRendererType = typeof(ToolStripSystemRenderer);

		// Token: 0x0400388B RID: 14475
		internal static Type ProfessionalRendererType = typeof(ToolStripProfessionalRenderer);

		// Token: 0x0400388C RID: 14476
		private static bool visualStylesEnabledIfPossible = true;

		// Token: 0x0400388D RID: 14477
		[ThreadStatic]
		private static Type currentRendererType;

		// Token: 0x020006B0 RID: 1712
		internal class ModalMenuFilter : IMessageModifyAndFilter, IMessageFilter
		{
			// Token: 0x1700129C RID: 4764
			// (get) Token: 0x060059D0 RID: 22992 RVA: 0x00146543 File Offset: 0x00145543
			internal static ToolStripManager.ModalMenuFilter Instance
			{
				get
				{
					if (ToolStripManager.ModalMenuFilter._instance == null)
					{
						ToolStripManager.ModalMenuFilter._instance = new ToolStripManager.ModalMenuFilter();
					}
					return ToolStripManager.ModalMenuFilter._instance;
				}
			}

			// Token: 0x060059D1 RID: 22993 RVA: 0x0014655B File Offset: 0x0014555B
			private ModalMenuFilter()
			{
			}

			// Token: 0x1700129D RID: 4765
			// (get) Token: 0x060059D2 RID: 22994 RVA: 0x00146579 File Offset: 0x00145579
			internal static HandleRef ActiveHwnd
			{
				get
				{
					return ToolStripManager.ModalMenuFilter.Instance.ActiveHwndInternal;
				}
			}

			// Token: 0x1700129E RID: 4766
			// (get) Token: 0x060059D3 RID: 22995 RVA: 0x00146585 File Offset: 0x00145585
			// (set) Token: 0x060059D4 RID: 22996 RVA: 0x0014658D File Offset: 0x0014558D
			public bool ShowUnderlines
			{
				get
				{
					return this._showUnderlines;
				}
				set
				{
					if (this._showUnderlines != value)
					{
						this._showUnderlines = value;
						ToolStripManager.NotifyMenuModeChange(true, false);
					}
				}
			}

			// Token: 0x1700129F RID: 4767
			// (get) Token: 0x060059D5 RID: 22997 RVA: 0x001465A6 File Offset: 0x001455A6
			// (set) Token: 0x060059D6 RID: 22998 RVA: 0x001465B0 File Offset: 0x001455B0
			private HandleRef ActiveHwndInternal
			{
				get
				{
					return this._activeHwnd;
				}
				set
				{
					if (this._activeHwnd.Handle != value.Handle)
					{
						Control control;
						if (this._activeHwnd.Handle != IntPtr.Zero)
						{
							control = Control.FromHandleInternal(this._activeHwnd.Handle);
							if (control != null)
							{
								control.HandleCreated -= this.OnActiveHwndHandleCreated;
							}
						}
						this._activeHwnd = value;
						control = Control.FromHandleInternal(this._activeHwnd.Handle);
						if (control != null)
						{
							control.HandleCreated += this.OnActiveHwndHandleCreated;
						}
					}
				}
			}

			// Token: 0x170012A0 RID: 4768
			// (get) Token: 0x060059D7 RID: 22999 RVA: 0x00146642 File Offset: 0x00145642
			internal static bool InMenuMode
			{
				get
				{
					return ToolStripManager.ModalMenuFilter.Instance._inMenuMode;
				}
			}

			// Token: 0x170012A1 RID: 4769
			// (get) Token: 0x060059D8 RID: 23000 RVA: 0x0014664E File Offset: 0x0014564E
			// (set) Token: 0x060059D9 RID: 23001 RVA: 0x0014665A File Offset: 0x0014565A
			internal static bool MenuKeyToggle
			{
				get
				{
					return ToolStripManager.ModalMenuFilter.Instance.menuKeyToggle;
				}
				set
				{
					if (ToolStripManager.ModalMenuFilter.Instance.menuKeyToggle != value)
					{
						ToolStripManager.ModalMenuFilter.Instance.menuKeyToggle = value;
					}
				}
			}

			// Token: 0x170012A2 RID: 4770
			// (get) Token: 0x060059DA RID: 23002 RVA: 0x00146674 File Offset: 0x00145674
			private ToolStripManager.ModalMenuFilter.HostedWindowsFormsMessageHook MessageHook
			{
				get
				{
					if (this.messageHook == null)
					{
						this.messageHook = new ToolStripManager.ModalMenuFilter.HostedWindowsFormsMessageHook();
					}
					return this.messageHook;
				}
			}

			// Token: 0x060059DB RID: 23003 RVA: 0x00146690 File Offset: 0x00145690
			private void EnterMenuModeCore()
			{
				if (!ToolStripManager.ModalMenuFilter.InMenuMode)
				{
					IntPtr activeWindow = UnsafeNativeMethods.GetActiveWindow();
					if (activeWindow != IntPtr.Zero)
					{
						this.ActiveHwndInternal = new HandleRef(this, activeWindow);
					}
					Application.ThreadContext.FromCurrent().AddMessageFilter(this);
					Application.ThreadContext.FromCurrent().TrackInput(true);
					if (!Application.ThreadContext.FromCurrent().GetMessageLoop(true))
					{
						this.MessageHook.HookMessages = true;
					}
					this._inMenuMode = true;
					this.ProcessMessages(true);
				}
			}

			// Token: 0x060059DC RID: 23004 RVA: 0x00146701 File Offset: 0x00145701
			internal static void ExitMenuMode()
			{
				ToolStripManager.ModalMenuFilter.Instance.ExitMenuModeCore();
			}

			// Token: 0x060059DD RID: 23005 RVA: 0x00146710 File Offset: 0x00145710
			private void ExitMenuModeCore()
			{
				this.ProcessMessages(false);
				if (ToolStripManager.ModalMenuFilter.InMenuMode)
				{
					try
					{
						if (this.messageHook != null)
						{
							this.messageHook.HookMessages = false;
						}
						Application.ThreadContext.FromCurrent().RemoveMessageFilter(this);
						Application.ThreadContext.FromCurrent().TrackInput(false);
						if (ToolStripManager.ModalMenuFilter.ActiveHwnd.Handle != IntPtr.Zero)
						{
							Control control = Control.FromHandleInternal(ToolStripManager.ModalMenuFilter.ActiveHwnd.Handle);
							if (control != null)
							{
								control.HandleCreated -= this.OnActiveHwndHandleCreated;
							}
							this.ActiveHwndInternal = NativeMethods.NullHandleRef;
						}
						if (this._inputFilterQueue != null)
						{
							this._inputFilterQueue.Clear();
						}
						if (this._caretHidden)
						{
							this._caretHidden = false;
							SafeNativeMethods.ShowCaret(NativeMethods.NullHandleRef);
						}
					}
					finally
					{
						this._inMenuMode = false;
						bool showUnderlines = this._showUnderlines;
						this._showUnderlines = false;
						ToolStripManager.NotifyMenuModeChange(showUnderlines, true);
					}
				}
			}

			// Token: 0x060059DE RID: 23006 RVA: 0x00146800 File Offset: 0x00145800
			internal static ToolStrip GetActiveToolStrip()
			{
				return ToolStripManager.ModalMenuFilter.Instance.GetActiveToolStripInternal();
			}

			// Token: 0x060059DF RID: 23007 RVA: 0x0014680C File Offset: 0x0014580C
			internal ToolStrip GetActiveToolStripInternal()
			{
				if (this._inputFilterQueue != null && this._inputFilterQueue.Count > 0)
				{
					return this._inputFilterQueue[this._inputFilterQueue.Count - 1];
				}
				return null;
			}

			// Token: 0x060059E0 RID: 23008 RVA: 0x00146840 File Offset: 0x00145840
			private ToolStrip GetCurrentToplevelToolStrip()
			{
				if (this._toplevelToolStrip == null)
				{
					ToolStrip activeToolStripInternal = this.GetActiveToolStripInternal();
					if (activeToolStripInternal != null)
					{
						this._toplevelToolStrip = activeToolStripInternal.GetToplevelOwnerToolStrip();
					}
				}
				return this._toplevelToolStrip;
			}

			// Token: 0x060059E1 RID: 23009 RVA: 0x00146874 File Offset: 0x00145874
			private void OnActiveHwndHandleCreated(object sender, EventArgs e)
			{
				Control control = sender as Control;
				this.ActiveHwndInternal = new HandleRef(this, control.Handle);
			}

			// Token: 0x060059E2 RID: 23010 RVA: 0x0014689C File Offset: 0x0014589C
			internal static void ProcessMenuKeyDown(ref Message m)
			{
				Keys keyData = (Keys)((int)m.WParam);
				ToolStrip toolStrip = Control.FromHandleInternal(m.HWnd) as ToolStrip;
				if (toolStrip != null && !toolStrip.IsDropDown)
				{
					return;
				}
				if (ToolStripManager.IsMenuKey(keyData))
				{
					if (!ToolStripManager.ModalMenuFilter.InMenuMode && ToolStripManager.ModalMenuFilter.MenuKeyToggle)
					{
						ToolStripManager.ModalMenuFilter.MenuKeyToggle = false;
						return;
					}
					if (!ToolStripManager.ModalMenuFilter.MenuKeyToggle)
					{
						ToolStripManager.ModalMenuFilter.Instance.ShowUnderlines = true;
					}
				}
			}

			// Token: 0x060059E3 RID: 23011 RVA: 0x00146901 File Offset: 0x00145901
			internal static void CloseActiveDropDown(ToolStripDropDown activeToolStripDropDown, ToolStripDropDownCloseReason reason)
			{
				activeToolStripDropDown.SetCloseReason(reason);
				activeToolStripDropDown.Visible = false;
				if (ToolStripManager.ModalMenuFilter.GetActiveToolStrip() == null)
				{
					ToolStripManager.ModalMenuFilter.ExitMenuMode();
					if (activeToolStripDropDown.OwnerItem != null)
					{
						activeToolStripDropDown.OwnerItem.Unselect();
					}
				}
			}

			// Token: 0x060059E4 RID: 23012 RVA: 0x00146930 File Offset: 0x00145930
			private void ProcessMessages(bool process)
			{
				if (process)
				{
					if (this._ensureMessageProcessingTimer == null)
					{
						this._ensureMessageProcessingTimer = new Timer();
					}
					this._ensureMessageProcessingTimer.Interval = 500;
					this._ensureMessageProcessingTimer.Enabled = true;
					return;
				}
				if (this._ensureMessageProcessingTimer != null)
				{
					this._ensureMessageProcessingTimer.Enabled = false;
					this._ensureMessageProcessingTimer.Dispose();
					this._ensureMessageProcessingTimer = null;
				}
			}

			// Token: 0x060059E5 RID: 23013 RVA: 0x00146998 File Offset: 0x00145998
			private void ProcessMouseButtonPressed(IntPtr hwndMouseMessageIsFrom, int x, int y)
			{
				int count = this._inputFilterQueue.Count;
				for (int i = 0; i < count; i++)
				{
					ToolStrip activeToolStripInternal = this.GetActiveToolStripInternal();
					if (activeToolStripInternal == null)
					{
						break;
					}
					NativeMethods.POINT point = new NativeMethods.POINT();
					point.x = x;
					point.y = y;
					UnsafeNativeMethods.MapWindowPoints(new HandleRef(activeToolStripInternal, hwndMouseMessageIsFrom), new HandleRef(activeToolStripInternal, activeToolStripInternal.Handle), point, 1);
					if (activeToolStripInternal.ClientRectangle.Contains(point.x, point.y))
					{
						break;
					}
					ToolStripDropDown toolStripDropDown = activeToolStripInternal as ToolStripDropDown;
					if (toolStripDropDown != null)
					{
						if (toolStripDropDown.OwnerToolStrip == null || !(toolStripDropDown.OwnerToolStrip.Handle == hwndMouseMessageIsFrom) || toolStripDropDown.OwnerDropDownItem == null || !toolStripDropDown.OwnerDropDownItem.DropDownButtonArea.Contains(x, y))
						{
							ToolStripManager.ModalMenuFilter.CloseActiveDropDown(toolStripDropDown, ToolStripDropDownCloseReason.AppClicked);
						}
					}
					else
					{
						activeToolStripInternal.NotifySelectionChange(null);
						this.ExitMenuModeCore();
					}
				}
			}

			// Token: 0x060059E6 RID: 23014 RVA: 0x00146A80 File Offset: 0x00145A80
			private bool ProcessActivationChange()
			{
				int count = this._inputFilterQueue.Count;
				for (int i = 0; i < count; i++)
				{
					ToolStripDropDown toolStripDropDown = this.GetActiveToolStripInternal() as ToolStripDropDown;
					if (toolStripDropDown != null && toolStripDropDown.AutoClose)
					{
						toolStripDropDown.Visible = false;
					}
				}
				this.ExitMenuModeCore();
				return true;
			}

			// Token: 0x060059E7 RID: 23015 RVA: 0x00146ACA File Offset: 0x00145ACA
			internal static void SetActiveToolStrip(ToolStrip toolStrip, bool menuKeyPressed)
			{
				if (!ToolStripManager.ModalMenuFilter.InMenuMode && menuKeyPressed)
				{
					ToolStripManager.ModalMenuFilter.Instance.ShowUnderlines = true;
				}
				ToolStripManager.ModalMenuFilter.Instance.SetActiveToolStripCore(toolStrip);
			}

			// Token: 0x060059E8 RID: 23016 RVA: 0x00146AEC File Offset: 0x00145AEC
			internal static void SetActiveToolStrip(ToolStrip toolStrip)
			{
				ToolStripManager.ModalMenuFilter.Instance.SetActiveToolStripCore(toolStrip);
			}

			// Token: 0x060059E9 RID: 23017 RVA: 0x00146AFC File Offset: 0x00145AFC
			private void SetActiveToolStripCore(ToolStrip toolStrip)
			{
				if (toolStrip == null)
				{
					return;
				}
				if (toolStrip.IsDropDown)
				{
					ToolStripDropDown toolStripDropDown = toolStrip as ToolStripDropDown;
					if (!toolStripDropDown.AutoClose)
					{
						IntPtr activeWindow = UnsafeNativeMethods.GetActiveWindow();
						if (activeWindow != IntPtr.Zero)
						{
							this.ActiveHwndInternal = new HandleRef(this, activeWindow);
						}
						return;
					}
				}
				toolStrip.KeyboardActive = true;
				if (this._inputFilterQueue == null)
				{
					this._inputFilterQueue = new List<ToolStrip>();
				}
				else
				{
					ToolStrip activeToolStripInternal = this.GetActiveToolStripInternal();
					if (activeToolStripInternal != null)
					{
						if (!activeToolStripInternal.IsDropDown)
						{
							this._inputFilterQueue.Remove(activeToolStripInternal);
						}
						else if (toolStrip.IsDropDown && ToolStripDropDown.GetFirstDropDown(toolStrip) != ToolStripDropDown.GetFirstDropDown(activeToolStripInternal))
						{
							this._inputFilterQueue.Remove(activeToolStripInternal);
							ToolStripDropDown toolStripDropDown2 = activeToolStripInternal as ToolStripDropDown;
							toolStripDropDown2.DismissAll();
						}
					}
				}
				this._toplevelToolStrip = null;
				if (!this._inputFilterQueue.Contains(toolStrip))
				{
					this._inputFilterQueue.Add(toolStrip);
				}
				if (!ToolStripManager.ModalMenuFilter.InMenuMode && this._inputFilterQueue.Count > 0)
				{
					this.EnterMenuModeCore();
				}
				if (!this._caretHidden && toolStrip.IsDropDown && ToolStripManager.ModalMenuFilter.InMenuMode)
				{
					this._caretHidden = true;
					SafeNativeMethods.HideCaret(NativeMethods.NullHandleRef);
				}
			}

			// Token: 0x060059EA RID: 23018 RVA: 0x00146C17 File Offset: 0x00145C17
			internal static void SuspendMenuMode()
			{
				ToolStripManager.ModalMenuFilter.Instance._suspendMenuMode = true;
			}

			// Token: 0x060059EB RID: 23019 RVA: 0x00146C24 File Offset: 0x00145C24
			internal static void ResumeMenuMode()
			{
				ToolStripManager.ModalMenuFilter.Instance._suspendMenuMode = false;
			}

			// Token: 0x060059EC RID: 23020 RVA: 0x00146C31 File Offset: 0x00145C31
			internal static void RemoveActiveToolStrip(ToolStrip toolStrip)
			{
				ToolStripManager.ModalMenuFilter.Instance.RemoveActiveToolStripCore(toolStrip);
			}

			// Token: 0x060059ED RID: 23021 RVA: 0x00146C3E File Offset: 0x00145C3E
			private void RemoveActiveToolStripCore(ToolStrip toolStrip)
			{
				this._toplevelToolStrip = null;
				if (this._inputFilterQueue != null)
				{
					this._inputFilterQueue.Remove(toolStrip);
				}
			}

			// Token: 0x060059EE RID: 23022 RVA: 0x00146C5C File Offset: 0x00145C5C
			private static bool IsChildOrSameWindow(HandleRef hwndParent, HandleRef hwndChild)
			{
				return hwndParent.Handle == hwndChild.Handle || UnsafeNativeMethods.IsChild(hwndParent, hwndChild);
			}

			// Token: 0x060059EF RID: 23023 RVA: 0x00146C84 File Offset: 0x00145C84
			private static bool IsKeyOrMouseMessage(Message m)
			{
				bool result = false;
				if (m.Msg >= 512 && m.Msg <= 522)
				{
					result = true;
				}
				else if (m.Msg >= 161 && m.Msg <= 169)
				{
					result = true;
				}
				else if (m.Msg >= 256 && m.Msg <= 264)
				{
					result = true;
				}
				return result;
			}

			// Token: 0x060059F0 RID: 23024 RVA: 0x00146CF4 File Offset: 0x00145CF4
			public bool PreFilterMessage(ref Message m)
			{
				if (this._suspendMenuMode)
				{
					return false;
				}
				ToolStrip activeToolStrip = ToolStripManager.ModalMenuFilter.GetActiveToolStrip();
				if (activeToolStrip == null)
				{
					return false;
				}
				if (activeToolStrip.IsDisposed)
				{
					this.RemoveActiveToolStripCore(activeToolStrip);
					return false;
				}
				HandleRef handleRef = new HandleRef(activeToolStrip, activeToolStrip.Handle);
				HandleRef handleRef2 = new HandleRef(null, UnsafeNativeMethods.GetActiveWindow());
				if (handleRef2.Handle != this._lastActiveWindow.Handle)
				{
					if (handleRef2.Handle == IntPtr.Zero)
					{
						this.ProcessActivationChange();
					}
					else if (!(Control.FromChildHandleInternal(handleRef2.Handle) is ToolStripDropDown) && !ToolStripManager.ModalMenuFilter.IsChildOrSameWindow(handleRef2, handleRef) && !ToolStripManager.ModalMenuFilter.IsChildOrSameWindow(handleRef2, ToolStripManager.ModalMenuFilter.ActiveHwnd))
					{
						this.ProcessActivationChange();
					}
				}
				this._lastActiveWindow = handleRef2;
				if (!ToolStripManager.ModalMenuFilter.IsKeyOrMouseMessage(m))
				{
					return false;
				}
				int msg = m.Msg;
				if (msg <= 167)
				{
					switch (msg)
					{
					case 160:
						goto IL_15E;
					case 161:
					case 164:
						break;
					case 162:
					case 163:
						return false;
					default:
						if (msg != 167)
						{
							return false;
						}
						break;
					}
					this.ProcessMouseButtonPressed(IntPtr.Zero, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam));
					return false;
				}
				switch (msg)
				{
				case 256:
				case 257:
				case 258:
				case 259:
				case 260:
				case 261:
				case 262:
				case 263:
					if (!activeToolStrip.ContainsFocus)
					{
						m.HWnd = activeToolStrip.Handle;
						return false;
					}
					return false;
				default:
					switch (msg)
					{
					case 512:
						goto IL_15E;
					case 513:
					case 516:
						break;
					case 514:
					case 515:
						return false;
					default:
						if (msg != 519)
						{
							return false;
						}
						break;
					}
					this.ProcessMouseButtonPressed(m.HWnd, NativeMethods.Util.SignedLOWORD(m.LParam), NativeMethods.Util.SignedHIWORD(m.LParam));
					return false;
				}
				IL_15E:
				Control control = Control.FromChildHandleInternal(m.HWnd);
				if ((control == null || !(control.TopLevelControlInternal is ToolStripDropDown)) && !ToolStripManager.ModalMenuFilter.IsChildOrSameWindow(handleRef, new HandleRef(null, m.HWnd)))
				{
					ToolStrip currentToplevelToolStrip = this.GetCurrentToplevelToolStrip();
					return (currentToplevelToolStrip == null || !ToolStripManager.ModalMenuFilter.IsChildOrSameWindow(new HandleRef(currentToplevelToolStrip, currentToplevelToolStrip.Handle), new HandleRef(null, m.HWnd))) && ToolStripManager.ModalMenuFilter.IsChildOrSameWindow(ToolStripManager.ModalMenuFilter.ActiveHwnd, new HandleRef(null, m.HWnd));
				}
				return false;
			}

			// Token: 0x0400388E RID: 14478
			private const int MESSAGE_PROCESSING_INTERVAL = 500;

			// Token: 0x0400388F RID: 14479
			private HandleRef _activeHwnd = NativeMethods.NullHandleRef;

			// Token: 0x04003890 RID: 14480
			private HandleRef _lastActiveWindow = NativeMethods.NullHandleRef;

			// Token: 0x04003891 RID: 14481
			private List<ToolStrip> _inputFilterQueue;

			// Token: 0x04003892 RID: 14482
			private bool _inMenuMode;

			// Token: 0x04003893 RID: 14483
			private bool _caretHidden;

			// Token: 0x04003894 RID: 14484
			private bool _showUnderlines;

			// Token: 0x04003895 RID: 14485
			private bool menuKeyToggle;

			// Token: 0x04003896 RID: 14486
			private bool _suspendMenuMode;

			// Token: 0x04003897 RID: 14487
			private ToolStripManager.ModalMenuFilter.HostedWindowsFormsMessageHook messageHook;

			// Token: 0x04003898 RID: 14488
			private Timer _ensureMessageProcessingTimer;

			// Token: 0x04003899 RID: 14489
			private ToolStrip _toplevelToolStrip;

			// Token: 0x0400389A RID: 14490
			[ThreadStatic]
			private static ToolStripManager.ModalMenuFilter _instance;

			// Token: 0x020006B1 RID: 1713
			private class HostedWindowsFormsMessageHook
			{
				// Token: 0x170012A3 RID: 4771
				// (get) Token: 0x060059F2 RID: 23026 RVA: 0x00146F4F File Offset: 0x00145F4F
				// (set) Token: 0x060059F3 RID: 23027 RVA: 0x00146F61 File Offset: 0x00145F61
				public bool HookMessages
				{
					get
					{
						return this.messageHookHandle != IntPtr.Zero;
					}
					set
					{
						if (value)
						{
							this.InstallMessageHook();
							return;
						}
						this.UninstallMessageHook();
					}
				}

				// Token: 0x060059F4 RID: 23028 RVA: 0x00146F74 File Offset: 0x00145F74
				private void InstallMessageHook()
				{
					lock (this)
					{
						if (!(this.messageHookHandle != IntPtr.Zero))
						{
							this.hookProc = new NativeMethods.HookProc(this.MessageHookProc);
							this.messageHookHandle = UnsafeNativeMethods.SetWindowsHookEx(3, this.hookProc, new HandleRef(null, IntPtr.Zero), SafeNativeMethods.GetCurrentThreadId());
							if (this.messageHookHandle != IntPtr.Zero)
							{
								this.isHooked = true;
							}
						}
					}
				}

				// Token: 0x060059F5 RID: 23029 RVA: 0x00147004 File Offset: 0x00146004
				private unsafe IntPtr MessageHookProc(int nCode, IntPtr wparam, IntPtr lparam)
				{
					if (nCode == 0 && this.isHooked && (int)wparam == 1)
					{
						NativeMethods.MSG* ptr = (NativeMethods.MSG*)((void*)lparam);
						if (ptr != null && Application.ThreadContext.FromCurrent().PreTranslateMessage(ref *ptr))
						{
							ptr->message = 0;
						}
					}
					return UnsafeNativeMethods.CallNextHookEx(new HandleRef(this, this.messageHookHandle), nCode, wparam, lparam);
				}

				// Token: 0x060059F6 RID: 23030 RVA: 0x0014705C File Offset: 0x0014605C
				private void UninstallMessageHook()
				{
					lock (this)
					{
						if (this.messageHookHandle != IntPtr.Zero)
						{
							UnsafeNativeMethods.UnhookWindowsHookEx(new HandleRef(this, this.messageHookHandle));
							this.hookProc = null;
							this.messageHookHandle = IntPtr.Zero;
							this.isHooked = false;
						}
					}
				}

				// Token: 0x0400389B RID: 14491
				private IntPtr messageHookHandle = IntPtr.Zero;

				// Token: 0x0400389C RID: 14492
				private bool isHooked;

				// Token: 0x0400389D RID: 14493
				private NativeMethods.HookProc hookProc;
			}
		}
	}
}

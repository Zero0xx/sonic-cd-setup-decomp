using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.ComponentModel.Com2Interop;
using System.Windows.Forms.Design;
using System.Windows.Forms.PropertyGridInternal;
using Microsoft.Win32;

namespace System.Windows.Forms
{
	// Token: 0x020005C3 RID: 1475
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[SRDescription("DescriptionPropertyGrid")]
	[Designer("System.Windows.Forms.Design.PropertyGridDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class PropertyGrid : ContainerControl, IComPropertyBrowser, UnsafeNativeMethods.IPropertyNotifySink
	{
		// Token: 0x06004CEF RID: 19695 RVA: 0x0011A8F8 File Offset: 0x001198F8
		private bool GetFlag(ushort flag)
		{
			return (this.flags & flag) != 0;
		}

		// Token: 0x06004CF0 RID: 19696 RVA: 0x0011A908 File Offset: 0x00119908
		private void SetFlag(ushort flag, bool value)
		{
			if (value)
			{
				this.flags |= flag;
				return;
			}
			this.flags &= ~flag;
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x0011A930 File Offset: 0x00119930
		public PropertyGrid()
		{
			this.onComponentAdd = new ComponentEventHandler(this.OnComponentAdd);
			this.onComponentRemove = new ComponentEventHandler(this.OnComponentRemove);
			this.onComponentChanged = new ComponentChangedEventHandler(this.OnComponentChanged);
			base.SuspendLayout();
			base.AutoScaleMode = AutoScaleMode.None;
			try
			{
				this.gridView = this.CreateGridView(null);
				this.gridView.TabStop = true;
				this.gridView.MouseMove += this.OnChildMouseMove;
				this.gridView.MouseDown += this.OnChildMouseDown;
				this.gridView.TabIndex = 2;
				this.separator1 = this.CreateSeparatorButton();
				this.separator2 = this.CreateSeparatorButton();
				this.toolStrip = new ToolStrip();
				this.toolStrip.SuspendLayout();
				this.toolStrip.ShowItemToolTips = true;
				this.toolStrip.AccessibleName = SR.GetString("PropertyGridToolbarAccessibleName");
				this.toolStrip.AccessibleRole = AccessibleRole.ToolBar;
				this.toolStrip.TabStop = true;
				this.toolStrip.AllowMerge = false;
				this.toolStrip.Text = "PropertyGridToolBar";
				this.toolStrip.Dock = DockStyle.None;
				this.toolStrip.AutoSize = false;
				this.toolStrip.TabIndex = 1;
				this.toolStrip.CanOverflow = false;
				this.toolStrip.GripStyle = ToolStripGripStyle.Hidden;
				Padding padding = this.toolStrip.Padding;
				padding.Left = 2;
				this.toolStrip.Padding = padding;
				this.SetToolStripRenderer();
				this.AddRefTab(this.DefaultTabType, null, PropertyTabScope.Static, true);
				this.doccomment = new DocComment(this);
				this.doccomment.SuspendLayout();
				this.doccomment.TabStop = false;
				this.doccomment.Dock = DockStyle.None;
				this.doccomment.BackColor = SystemColors.Control;
				this.doccomment.ForeColor = SystemColors.ControlText;
				this.doccomment.MouseMove += this.OnChildMouseMove;
				this.doccomment.MouseDown += this.OnChildMouseDown;
				this.hotcommands = new HotCommands(this);
				this.hotcommands.SuspendLayout();
				this.hotcommands.TabIndex = 3;
				this.hotcommands.Dock = DockStyle.None;
				this.SetHotCommandColors(false);
				this.hotcommands.Visible = false;
				this.hotcommands.MouseMove += this.OnChildMouseMove;
				this.hotcommands.MouseDown += this.OnChildMouseDown;
				this.Controls.AddRange(new Control[]
				{
					this.doccomment,
					this.hotcommands,
					this.gridView,
					this.toolStrip
				});
				base.SetActiveControlInternal(this.gridView);
				this.toolStrip.ResumeLayout(false);
				this.SetupToolbar();
				this.PropertySort = PropertySort.CategorizedAlphabetical;
				this.Text = "PropertyGrid";
				this.SetSelectState(0);
			}
			catch (Exception)
			{
			}
			finally
			{
				if (this.doccomment != null)
				{
					this.doccomment.ResumeLayout(false);
				}
				if (this.hotcommands != null)
				{
					this.hotcommands.ResumeLayout(false);
				}
				base.ResumeLayout(true);
			}
		}

		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x06004CF2 RID: 19698 RVA: 0x0011ACF8 File Offset: 0x00119CF8
		// (set) Token: 0x06004CF3 RID: 19699 RVA: 0x0011AD24 File Offset: 0x00119D24
		internal IDesignerHost ActiveDesigner
		{
			get
			{
				if (this.designerHost == null)
				{
					this.designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
				}
				return this.designerHost;
			}
			set
			{
				if (value != this.designerHost)
				{
					this.SetFlag(32, true);
					if (this.designerHost != null)
					{
						IComponentChangeService componentChangeService = (IComponentChangeService)this.designerHost.GetService(typeof(IComponentChangeService));
						if (componentChangeService != null)
						{
							componentChangeService.ComponentAdded -= this.onComponentAdd;
							componentChangeService.ComponentRemoved -= this.onComponentRemove;
							componentChangeService.ComponentChanged -= this.onComponentChanged;
						}
						IPropertyValueUIService propertyValueUIService = (IPropertyValueUIService)this.designerHost.GetService(typeof(IPropertyValueUIService));
						if (propertyValueUIService != null)
						{
							propertyValueUIService.PropertyUIValueItemsChanged -= this.OnNotifyPropertyValueUIItemsChanged;
						}
						this.designerHost.TransactionOpened -= this.OnTransactionOpened;
						this.designerHost.TransactionClosed -= this.OnTransactionClosed;
						this.SetFlag(16, false);
						this.RemoveTabs(PropertyTabScope.Document, true);
						this.designerHost = null;
					}
					if (value != null)
					{
						IComponentChangeService componentChangeService2 = (IComponentChangeService)value.GetService(typeof(IComponentChangeService));
						if (componentChangeService2 != null)
						{
							componentChangeService2.ComponentAdded += this.onComponentAdd;
							componentChangeService2.ComponentRemoved += this.onComponentRemove;
							componentChangeService2.ComponentChanged += this.onComponentChanged;
						}
						value.TransactionOpened += this.OnTransactionOpened;
						value.TransactionClosed += this.OnTransactionClosed;
						this.SetFlag(16, false);
						IPropertyValueUIService propertyValueUIService2 = (IPropertyValueUIService)value.GetService(typeof(IPropertyValueUIService));
						if (propertyValueUIService2 != null)
						{
							propertyValueUIService2.PropertyUIValueItemsChanged += this.OnNotifyPropertyValueUIItemsChanged;
						}
					}
					this.designerHost = value;
					if (this.peMain != null)
					{
						this.peMain.DesignerHost = value;
					}
					this.RefreshTabs(PropertyTabScope.Document);
				}
			}
		}

		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x06004CF4 RID: 19700 RVA: 0x0011AEC6 File Offset: 0x00119EC6
		// (set) Token: 0x06004CF5 RID: 19701 RVA: 0x0011AECE File Offset: 0x00119ECE
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				base.AutoScroll = value;
			}
		}

		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x06004CF6 RID: 19702 RVA: 0x0011AED7 File Offset: 0x00119ED7
		// (set) Token: 0x06004CF7 RID: 19703 RVA: 0x0011AEDF File Offset: 0x00119EDF
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				this.toolStrip.BackColor = value;
				this.toolStrip.Invalidate(true);
			}
		}

		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x06004CF8 RID: 19704 RVA: 0x0011AF00 File Offset: 0x00119F00
		// (set) Token: 0x06004CF9 RID: 19705 RVA: 0x0011AF08 File Offset: 0x00119F08
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
				base.BackgroundImage = value;
			}
		}

		// Token: 0x140002CA RID: 714
		// (add) Token: 0x06004CFA RID: 19706 RVA: 0x0011AF11 File Offset: 0x00119F11
		// (remove) Token: 0x06004CFB RID: 19707 RVA: 0x0011AF1A File Offset: 0x00119F1A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x06004CFC RID: 19708 RVA: 0x0011AF23 File Offset: 0x00119F23
		// (set) Token: 0x06004CFD RID: 19709 RVA: 0x0011AF2B File Offset: 0x00119F2B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x140002CB RID: 715
		// (add) Token: 0x06004CFE RID: 19710 RVA: 0x0011AF34 File Offset: 0x00119F34
		// (remove) Token: 0x06004CFF RID: 19711 RVA: 0x0011AF3D File Offset: 0x00119F3D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x06004D01 RID: 19713 RVA: 0x0011AFD0 File Offset: 0x00119FD0
		// (set) Token: 0x06004D00 RID: 19712 RVA: 0x0011AF48 File Offset: 0x00119F48
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public AttributeCollection BrowsableAttributes
		{
			get
			{
				if (this.browsableAttributes == null)
				{
					this.browsableAttributes = new AttributeCollection(new Attribute[]
					{
						new BrowsableAttribute(true)
					});
				}
				return this.browsableAttributes;
			}
			set
			{
				if (value == null || value == AttributeCollection.Empty)
				{
					this.browsableAttributes = new AttributeCollection(new Attribute[]
					{
						BrowsableAttribute.Yes
					});
				}
				else
				{
					Attribute[] array = new Attribute[value.Count];
					value.CopyTo(array, 0);
					this.browsableAttributes = new AttributeCollection(array);
				}
				if (this.currentObjects != null && this.currentObjects.Length > 0 && this.peMain != null)
				{
					this.peMain.BrowsableAttributes = this.BrowsableAttributes;
					this.Refresh(true);
				}
			}
		}

		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x06004D02 RID: 19714 RVA: 0x0011B007 File Offset: 0x0011A007
		private bool CanCopy
		{
			get
			{
				return this.gridView.CanCopy;
			}
		}

		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x06004D03 RID: 19715 RVA: 0x0011B014 File Offset: 0x0011A014
		private bool CanCut
		{
			get
			{
				return this.gridView.CanCut;
			}
		}

		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x06004D04 RID: 19716 RVA: 0x0011B021 File Offset: 0x0011A021
		private bool CanPaste
		{
			get
			{
				return this.gridView.CanPaste;
			}
		}

		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x06004D05 RID: 19717 RVA: 0x0011B02E File Offset: 0x0011A02E
		private bool CanUndo
		{
			get
			{
				return this.gridView.CanUndo;
			}
		}

		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x06004D06 RID: 19718 RVA: 0x0011B03B File Offset: 0x0011A03B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[SRDescription("PropertyGridCanShowCommandsDesc")]
		public virtual bool CanShowCommands
		{
			get
			{
				return this.hotcommands.WouldBeVisible;
			}
		}

		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x06004D07 RID: 19719 RVA: 0x0011B048 File Offset: 0x0011A048
		// (set) Token: 0x06004D08 RID: 19720 RVA: 0x0011B050 File Offset: 0x0011A050
		[DefaultValue(typeof(Color), "ControlText")]
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridCategoryForeColorDesc")]
		public Color CategoryForeColor
		{
			get
			{
				return this.categoryForeColor;
			}
			set
			{
				if (this.categoryForeColor != value)
				{
					this.categoryForeColor = value;
					this.gridView.Invalidate();
				}
			}
		}

		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x06004D09 RID: 19721 RVA: 0x0011B072 File Offset: 0x0011A072
		// (set) Token: 0x06004D0A RID: 19722 RVA: 0x0011B07F File Offset: 0x0011A07F
		[SRDescription("PropertyGridCommandsBackColorDesc")]
		[SRCategory("CatAppearance")]
		public Color CommandsBackColor
		{
			get
			{
				return this.hotcommands.BackColor;
			}
			set
			{
				this.hotcommands.BackColor = value;
			}
		}

		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x06004D0B RID: 19723 RVA: 0x0011B08D File Offset: 0x0011A08D
		// (set) Token: 0x06004D0C RID: 19724 RVA: 0x0011B09A File Offset: 0x0011A09A
		[SRDescription("PropertyGridCommandsForeColorDesc")]
		[SRCategory("CatAppearance")]
		public Color CommandsForeColor
		{
			get
			{
				return this.hotcommands.ForeColor;
			}
			set
			{
				this.hotcommands.ForeColor = value;
			}
		}

		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x06004D0D RID: 19725 RVA: 0x0011B0A8 File Offset: 0x0011A0A8
		// (set) Token: 0x06004D0E RID: 19726 RVA: 0x0011B0BA File Offset: 0x0011A0BA
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridCommandsLinkColorDesc")]
		public Color CommandsLinkColor
		{
			get
			{
				return this.hotcommands.Label.LinkColor;
			}
			set
			{
				this.hotcommands.Label.LinkColor = value;
			}
		}

		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x06004D0F RID: 19727 RVA: 0x0011B0CD File Offset: 0x0011A0CD
		// (set) Token: 0x06004D10 RID: 19728 RVA: 0x0011B0DF File Offset: 0x0011A0DF
		[SRDescription("PropertyGridCommandsActiveLinkColorDesc")]
		[SRCategory("CatAppearance")]
		public Color CommandsActiveLinkColor
		{
			get
			{
				return this.hotcommands.Label.ActiveLinkColor;
			}
			set
			{
				this.hotcommands.Label.ActiveLinkColor = value;
			}
		}

		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x06004D11 RID: 19729 RVA: 0x0011B0F2 File Offset: 0x0011A0F2
		// (set) Token: 0x06004D12 RID: 19730 RVA: 0x0011B104 File Offset: 0x0011A104
		[SRDescription("PropertyGridCommandsDisabledLinkColorDesc")]
		[SRCategory("CatAppearance")]
		public Color CommandsDisabledLinkColor
		{
			get
			{
				return this.hotcommands.Label.DisabledLinkColor;
			}
			set
			{
				this.hotcommands.Label.DisabledLinkColor = value;
			}
		}

		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x06004D13 RID: 19731 RVA: 0x0011B117 File Offset: 0x0011A117
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public virtual bool CommandsVisible
		{
			get
			{
				return this.hotcommands.Visible;
			}
		}

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x06004D14 RID: 19732 RVA: 0x0011B124 File Offset: 0x0011A124
		// (set) Token: 0x06004D15 RID: 19733 RVA: 0x0011B134 File Offset: 0x0011A134
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridCommandsVisibleIfAvailable")]
		[DefaultValue(true)]
		public virtual bool CommandsVisibleIfAvailable
		{
			get
			{
				return this.hotcommands.AllowVisible;
			}
			set
			{
				bool visible = this.hotcommands.Visible;
				this.hotcommands.AllowVisible = value;
				if (visible != this.hotcommands.Visible)
				{
					this.OnLayoutInternal(false);
					this.hotcommands.Invalidate();
				}
			}
		}

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x06004D16 RID: 19734 RVA: 0x0011B179 File Offset: 0x0011A179
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public Point ContextMenuDefaultLocation
		{
			get
			{
				return this.GetPropertyGridView().ContextMenuDefaultLocation;
			}
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x06004D17 RID: 19735 RVA: 0x0011B186 File Offset: 0x0011A186
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Control.ControlCollection Controls
		{
			get
			{
				return base.Controls;
			}
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x06004D18 RID: 19736 RVA: 0x0011B18E File Offset: 0x0011A18E
		protected override Size DefaultSize
		{
			get
			{
				return new Size(130, 130);
			}
		}

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x06004D19 RID: 19737 RVA: 0x0011B19F File Offset: 0x0011A19F
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual Type DefaultTabType
		{
			get
			{
				return typeof(PropertiesTab);
			}
		}

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x06004D1A RID: 19738 RVA: 0x0011B1AB File Offset: 0x0011A1AB
		// (set) Token: 0x06004D1B RID: 19739 RVA: 0x0011B1B3 File Offset: 0x0011A1B3
		protected bool DrawFlatToolbar
		{
			get
			{
				return this.drawFlatToolBar;
			}
			set
			{
				if (this.drawFlatToolBar != value)
				{
					this.drawFlatToolBar = value;
					this.SetToolStripRenderer();
				}
				this.SetHotCommandColors(value);
			}
		}

		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x06004D1C RID: 19740 RVA: 0x0011B1D2 File Offset: 0x0011A1D2
		// (set) Token: 0x06004D1D RID: 19741 RVA: 0x0011B1DA File Offset: 0x0011A1DA
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

		// Token: 0x140002CC RID: 716
		// (add) Token: 0x06004D1E RID: 19742 RVA: 0x0011B1E3 File Offset: 0x0011A1E3
		// (remove) Token: 0x06004D1F RID: 19743 RVA: 0x0011B1EC File Offset: 0x0011A1EC
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler ForeColorChanged
		{
			add
			{
				base.ForeColorChanged += value;
			}
			remove
			{
				base.ForeColorChanged -= value;
			}
		}

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x06004D20 RID: 19744 RVA: 0x0011B1F5 File Offset: 0x0011A1F5
		// (set) Token: 0x06004D21 RID: 19745 RVA: 0x0011B200 File Offset: 0x0011A200
		private bool FreezePainting
		{
			get
			{
				return this.paintFrozen > 0;
			}
			set
			{
				if (value && base.IsHandleCreated && base.Visible && this.paintFrozen++ == 0)
				{
					base.SendMessage(11, 0, 0);
				}
				if (!value)
				{
					if (this.paintFrozen == 0)
					{
						return;
					}
					if (--this.paintFrozen == 0)
					{
						base.SendMessage(11, 1, 0);
						base.Invalidate(true);
					}
				}
			}
		}

		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x06004D22 RID: 19746 RVA: 0x0011B26F File Offset: 0x0011A26F
		// (set) Token: 0x06004D23 RID: 19747 RVA: 0x0011B27C File Offset: 0x0011A27C
		[SRDescription("PropertyGridHelpBackColorDesc")]
		[DefaultValue(typeof(Color), "Control")]
		[SRCategory("CatAppearance")]
		public Color HelpBackColor
		{
			get
			{
				return this.doccomment.BackColor;
			}
			set
			{
				this.doccomment.BackColor = value;
			}
		}

		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x06004D24 RID: 19748 RVA: 0x0011B28A File Offset: 0x0011A28A
		// (set) Token: 0x06004D25 RID: 19749 RVA: 0x0011B297 File Offset: 0x0011A297
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridHelpForeColorDesc")]
		[DefaultValue(typeof(Color), "ControlText")]
		public Color HelpForeColor
		{
			get
			{
				return this.doccomment.ForeColor;
			}
			set
			{
				this.doccomment.ForeColor = value;
			}
		}

		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x06004D26 RID: 19750 RVA: 0x0011B2A5 File Offset: 0x0011A2A5
		// (set) Token: 0x06004D27 RID: 19751 RVA: 0x0011B2AD File Offset: 0x0011A2AD
		[DefaultValue(true)]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridHelpVisibleDesc")]
		public virtual bool HelpVisible
		{
			get
			{
				return this.helpVisible;
			}
			set
			{
				this.helpVisible = value;
				this.doccomment.Visible = value;
				this.OnLayoutInternal(false);
				base.Invalidate();
				this.doccomment.Invalidate();
			}
		}

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x06004D28 RID: 19752 RVA: 0x0011B2DA File Offset: 0x0011A2DA
		bool IComPropertyBrowser.InPropertySet
		{
			get
			{
				return this.GetPropertyGridView().GetInPropertySet();
			}
		}

		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x06004D29 RID: 19753 RVA: 0x0011B2E7 File Offset: 0x0011A2E7
		// (set) Token: 0x06004D2A RID: 19754 RVA: 0x0011B2EF File Offset: 0x0011A2EF
		[DefaultValue(typeof(Color), "InactiveBorder")]
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridLineColorDesc")]
		public Color LineColor
		{
			get
			{
				return this.lineColor;
			}
			set
			{
				if (this.lineColor != value)
				{
					this.lineColor = value;
					if (this.lineBrush != null)
					{
						this.lineBrush.Dispose();
						this.lineBrush = null;
					}
					this.gridView.Invalidate();
				}
			}
		}

		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x06004D2B RID: 19755 RVA: 0x0011B32B File Offset: 0x0011A32B
		// (set) Token: 0x06004D2C RID: 19756 RVA: 0x0011B333 File Offset: 0x0011A333
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x140002CD RID: 717
		// (add) Token: 0x06004D2D RID: 19757 RVA: 0x0011B33C File Offset: 0x0011A33C
		// (remove) Token: 0x06004D2E RID: 19758 RVA: 0x0011B345 File Offset: 0x0011A345
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x06004D2F RID: 19759 RVA: 0x0011B34E File Offset: 0x0011A34E
		// (set) Token: 0x06004D30 RID: 19760 RVA: 0x0011B358 File Offset: 0x0011A358
		[DefaultValue(PropertySort.CategorizedAlphabetical)]
		[SRDescription("PropertyGridPropertySortDesc")]
		[SRCategory("CatAppearance")]
		public PropertySort PropertySort
		{
			get
			{
				return this.propertySortValue;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(PropertySort));
				}
				ToolStripButton sender;
				if ((value & PropertySort.Categorized) != PropertySort.NoSort)
				{
					sender = this.viewSortButtons[0];
				}
				else if ((value & PropertySort.Alphabetical) != PropertySort.NoSort)
				{
					sender = this.viewSortButtons[1];
				}
				else
				{
					sender = this.viewSortButtons[2];
				}
				GridItem selectedGridItem = this.SelectedGridItem;
				this.OnViewSortButtonClick(sender, EventArgs.Empty);
				this.propertySortValue = value;
				if (selectedGridItem != null)
				{
					try
					{
						this.SelectedGridItem = selectedGridItem;
					}
					catch (ArgumentException)
					{
					}
				}
			}
		}

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x06004D31 RID: 19761 RVA: 0x0011B3F0 File Offset: 0x0011A3F0
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PropertyGrid.PropertyTabCollection PropertyTabs
		{
			get
			{
				return new PropertyGrid.PropertyTabCollection(this);
			}
		}

		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x06004D32 RID: 19762 RVA: 0x0011B3F8 File Offset: 0x0011A3F8
		// (set) Token: 0x06004D33 RID: 19763 RVA: 0x0011B418 File Offset: 0x0011A418
		[SRCategory("CatBehavior")]
		[TypeConverter(typeof(PropertyGrid.SelectedObjectConverter))]
		[DefaultValue(null)]
		[SRDescription("PropertyGridSelectedObjectDesc")]
		public object SelectedObject
		{
			get
			{
				if (this.currentObjects == null || this.currentObjects.Length == 0)
				{
					return null;
				}
				return this.currentObjects[0];
			}
			set
			{
				if (value == null)
				{
					this.SelectedObjects = new object[0];
					return;
				}
				this.SelectedObjects = new object[]
				{
					value
				};
			}
		}

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x06004D35 RID: 19765 RVA: 0x0011B974 File Offset: 0x0011A974
		// (set) Token: 0x06004D34 RID: 19764 RVA: 0x0011B448 File Offset: 0x0011A448
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object[] SelectedObjects
		{
			get
			{
				if (this.currentObjects == null)
				{
					return new object[0];
				}
				return (object[])this.currentObjects.Clone();
			}
			set
			{
				try
				{
					this.FreezePainting = true;
					this.SetFlag(128, false);
					if (this.GetFlag(16))
					{
						this.SetFlag(256, false);
					}
					this.gridView.EnsurePendingChangesCommitted();
					bool flag = false;
					bool flag2 = false;
					bool flag3 = true;
					if (value != null && value.Length > 0)
					{
						for (int i = 0; i < value.Length; i++)
						{
							if (value[i] == null)
							{
								throw new ArgumentException(SR.GetString("PropertyGridSetNull", new object[]
								{
									i.ToString(CultureInfo.CurrentCulture),
									value.Length.ToString(CultureInfo.CurrentCulture)
								}));
							}
							if (value[i] is PropertyGrid.IUnimplemented)
							{
								throw new NotSupportedException(SR.GetString("PropertyGridRemotedObject", new object[]
								{
									value[i].GetType().FullName
								}));
							}
						}
					}
					else
					{
						flag3 = false;
					}
					if (this.currentObjects != null && value != null && this.currentObjects.Length == value.Length)
					{
						flag = true;
						flag2 = true;
						int num = 0;
						while (num < value.Length && (flag || flag2))
						{
							if (flag && this.currentObjects[num] != value[num])
							{
								flag = false;
							}
							Type type = this.GetUnwrappedObject(num).GetType();
							object obj = value[num];
							if (obj is ICustomTypeDescriptor)
							{
								obj = ((ICustomTypeDescriptor)obj).GetPropertyOwner(null);
							}
							Type type2 = obj.GetType();
							if (flag2 && (type != type2 || (type.IsCOMObject && type2.IsCOMObject)))
							{
								flag2 = false;
							}
							num++;
						}
					}
					if (!flag)
					{
						this.EnsureDesignerEventService();
						flag3 = (flag3 && this.GetFlag(2));
						this.SetStatusBox("", "");
						this.ClearCachedProps();
						if (value == null)
						{
							this.currentObjects = new object[0];
						}
						else
						{
							this.currentObjects = (object[])value.Clone();
						}
						this.SinkPropertyNotifyEvents();
						this.SetFlag(1, true);
						if (this.gridView != null)
						{
							this.gridView.RemoveSelectedEntryHelpAttributes();
						}
						if (this.peMain != null)
						{
							this.peMain.Dispose();
						}
						if (!flag2 && !this.GetFlag(8) && this.selectedViewTab < this.viewTabButtons.Length)
						{
							Type type3 = (this.selectedViewTab == -1) ? null : this.viewTabs[this.selectedViewTab].GetType();
							ToolStripButton button = null;
							this.RefreshTabs(PropertyTabScope.Component);
							this.EnableTabs();
							if (type3 != null)
							{
								for (int j = 0; j < this.viewTabs.Length; j++)
								{
									if (this.viewTabs[j].GetType() == type3 && this.viewTabButtons[j].Visible)
									{
										button = this.viewTabButtons[j];
										break;
									}
								}
							}
							this.SelectViewTabButtonDefault(button);
						}
						if (flag3 && this.viewTabs != null && this.viewTabs.Length > 1 && this.viewTabs[1] is EventsTab)
						{
							flag3 = this.viewTabButtons[1].Visible;
							Attribute[] array = new Attribute[this.BrowsableAttributes.Count];
							this.BrowsableAttributes.CopyTo(array, 0);
							Hashtable hashtable = null;
							if (this.currentObjects.Length > 10)
							{
								hashtable = new Hashtable();
							}
							int num2 = 0;
							while (num2 < this.currentObjects.Length && flag3)
							{
								object obj2 = this.currentObjects[num2];
								if (obj2 is ICustomTypeDescriptor)
								{
									obj2 = ((ICustomTypeDescriptor)obj2).GetPropertyOwner(null);
								}
								Type type4 = obj2.GetType();
								if (hashtable == null || !hashtable.Contains(type4))
								{
									flag3 = (flag3 && obj2 is IComponent && ((IComponent)obj2).Site != null);
									PropertyDescriptorCollection properties = ((EventsTab)this.viewTabs[1]).GetProperties(obj2, array);
									flag3 = (flag3 && properties != null && properties.Count > 0);
									if (flag3 && hashtable != null)
									{
										hashtable[type4] = type4;
									}
								}
								num2++;
							}
						}
						this.ShowEventsButton(flag3 && this.currentObjects.Length > 0);
						this.DisplayHotCommands();
						if (this.currentObjects.Length == 1)
						{
							this.EnablePropPageButton(this.currentObjects[0]);
						}
						else
						{
							this.EnablePropPageButton(null);
						}
						this.OnSelectedObjectsChanged(EventArgs.Empty);
					}
					if (!this.GetFlag(8))
					{
						if (this.currentObjects.Length > 0 && this.GetFlag(32))
						{
							object activeDesigner = this.ActiveDesigner;
							if (activeDesigner != null && this.designerSelections != null && this.designerSelections.ContainsKey(activeDesigner.GetHashCode()))
							{
								int num3 = (int)this.designerSelections[activeDesigner.GetHashCode()];
								if (num3 < this.viewTabs.Length && (num3 == 0 || this.viewTabButtons[num3].Visible))
								{
									this.SelectViewTabButton(this.viewTabButtons[num3], true);
								}
							}
							else
							{
								this.Refresh(false);
							}
							this.SetFlag(32, false);
						}
						else
						{
							this.Refresh(true);
						}
						if (this.currentObjects.Length > 0)
						{
							this.SaveTabSelection();
						}
					}
				}
				finally
				{
					this.FreezePainting = false;
				}
			}
		}

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x06004D36 RID: 19766 RVA: 0x0011B995 File Offset: 0x0011A995
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PropertyTab SelectedTab
		{
			get
			{
				return this.viewTabs[this.selectedViewTab];
			}
		}

		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x06004D37 RID: 19767 RVA: 0x0011B9A4 File Offset: 0x0011A9A4
		// (set) Token: 0x06004D38 RID: 19768 RVA: 0x0011B9C8 File Offset: 0x0011A9C8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public GridItem SelectedGridItem
		{
			get
			{
				GridItem selectedGridEntry = this.gridView.SelectedGridEntry;
				if (selectedGridEntry == null)
				{
					return this.peMain;
				}
				return selectedGridEntry;
			}
			set
			{
				this.gridView.SelectedGridEntry = (GridEntry)value;
			}
		}

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x06004D39 RID: 19769 RVA: 0x0011B9DB File Offset: 0x0011A9DB
		protected internal override bool ShowFocusCues
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x06004D3A RID: 19770 RVA: 0x0011B9DE File Offset: 0x0011A9DE
		// (set) Token: 0x06004D3B RID: 19771 RVA: 0x0011B9E8 File Offset: 0x0011A9E8
		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				base.SuspendAllLayout(this);
				base.Site = value;
				this.gridView.ServiceProvider = value;
				if (value == null)
				{
					this.ActiveDesigner = null;
				}
				else
				{
					this.ActiveDesigner = (IDesignerHost)value.GetService(typeof(IDesignerHost));
				}
				base.ResumeAllLayout(this, true);
			}
		}

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x06004D3C RID: 19772 RVA: 0x0011BA3E File Offset: 0x0011AA3E
		// (set) Token: 0x06004D3D RID: 19773 RVA: 0x0011BA46 File Offset: 0x0011AA46
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		// Token: 0x140002CE RID: 718
		// (add) Token: 0x06004D3E RID: 19774 RVA: 0x0011BA4F File Offset: 0x0011AA4F
		// (remove) Token: 0x06004D3F RID: 19775 RVA: 0x0011BA58 File Offset: 0x0011AA58
		[Browsable(false)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x06004D40 RID: 19776 RVA: 0x0011BA61 File Offset: 0x0011AA61
		// (set) Token: 0x06004D41 RID: 19777 RVA: 0x0011BA6C File Offset: 0x0011AA6C
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		[SRDescription("PropertyGridLargeButtonsDesc")]
		public bool LargeButtons
		{
			get
			{
				return this.buttonType == 1;
			}
			set
			{
				if (value == (this.buttonType == 1))
				{
					return;
				}
				this.buttonType = (value ? 1 : 0);
				if (value)
				{
					this.EnsureLargeButtons();
					if (this.imageList != null && this.imageList[1] != null)
					{
						this.toolStrip.ImageScalingSize = this.imageList[1].ImageSize;
					}
				}
				else if (this.imageList != null && this.imageList[0] != null)
				{
					this.toolStrip.ImageScalingSize = this.imageList[0].ImageSize;
				}
				this.toolStrip.ImageList = this.imageList[this.buttonType];
				this.OnLayoutInternal(false);
				base.Invalidate();
				this.toolStrip.Invalidate();
			}
		}

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x06004D42 RID: 19778 RVA: 0x0011BB22 File Offset: 0x0011AB22
		// (set) Token: 0x06004D43 RID: 19779 RVA: 0x0011BB2A File Offset: 0x0011AB2A
		[SRDescription("PropertyGridToolbarVisibleDesc")]
		[SRCategory("CatAppearance")]
		[DefaultValue(true)]
		public virtual bool ToolbarVisible
		{
			get
			{
				return this.toolbarVisible;
			}
			set
			{
				this.toolbarVisible = value;
				this.toolStrip.Visible = value;
				this.OnLayoutInternal(false);
				if (value)
				{
					this.SetupToolbar(this.viewTabsDirty);
				}
				base.Invalidate();
				this.toolStrip.Invalidate();
			}
		}

		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x06004D44 RID: 19780 RVA: 0x0011BB66 File Offset: 0x0011AB66
		// (set) Token: 0x06004D45 RID: 19781 RVA: 0x0011BB7D File Offset: 0x0011AB7D
		protected ToolStripRenderer ToolStripRenderer
		{
			get
			{
				if (this.toolStrip != null)
				{
					return this.toolStrip.Renderer;
				}
				return null;
			}
			set
			{
				if (this.toolStrip != null)
				{
					this.toolStrip.Renderer = value;
				}
			}
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x06004D46 RID: 19782 RVA: 0x0011BB93 File Offset: 0x0011AB93
		// (set) Token: 0x06004D47 RID: 19783 RVA: 0x0011BBA0 File Offset: 0x0011ABA0
		[DefaultValue(typeof(Color), "Window")]
		[SRCategory("CatAppearance")]
		[SRDescription("PropertyGridViewBackColorDesc")]
		public Color ViewBackColor
		{
			get
			{
				return this.gridView.BackColor;
			}
			set
			{
				this.gridView.BackColor = value;
				this.gridView.Invalidate();
			}
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x06004D48 RID: 19784 RVA: 0x0011BBB9 File Offset: 0x0011ABB9
		// (set) Token: 0x06004D49 RID: 19785 RVA: 0x0011BBC6 File Offset: 0x0011ABC6
		[SRDescription("PropertyGridViewForeColorDesc")]
		[SRCategory("CatAppearance")]
		[DefaultValue(typeof(Color), "WindowText")]
		public Color ViewForeColor
		{
			get
			{
				return this.gridView.ForeColor;
			}
			set
			{
				this.gridView.ForeColor = value;
				this.gridView.Invalidate();
			}
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x0011BBE0 File Offset: 0x0011ABE0
		private int AddImage(Bitmap image)
		{
			image.MakeTransparent();
			int count = this.imageList[0].Images.Count;
			this.imageList[0].Images.Add(image);
			return count;
		}

		// Token: 0x140002CF RID: 719
		// (add) Token: 0x06004D4B RID: 19787 RVA: 0x0011BC1A File Offset: 0x0011AC1A
		// (remove) Token: 0x06004D4C RID: 19788 RVA: 0x0011BC23 File Offset: 0x0011AC23
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event KeyEventHandler KeyDown
		{
			add
			{
				base.KeyDown += value;
			}
			remove
			{
				base.KeyDown -= value;
			}
		}

		// Token: 0x140002D0 RID: 720
		// (add) Token: 0x06004D4D RID: 19789 RVA: 0x0011BC2C File Offset: 0x0011AC2C
		// (remove) Token: 0x06004D4E RID: 19790 RVA: 0x0011BC35 File Offset: 0x0011AC35
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				base.KeyPress += value;
			}
			remove
			{
				base.KeyPress -= value;
			}
		}

		// Token: 0x140002D1 RID: 721
		// (add) Token: 0x06004D4F RID: 19791 RVA: 0x0011BC3E File Offset: 0x0011AC3E
		// (remove) Token: 0x06004D50 RID: 19792 RVA: 0x0011BC47 File Offset: 0x0011AC47
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public new event KeyEventHandler KeyUp
		{
			add
			{
				base.KeyUp += value;
			}
			remove
			{
				base.KeyUp -= value;
			}
		}

		// Token: 0x140002D2 RID: 722
		// (add) Token: 0x06004D51 RID: 19793 RVA: 0x0011BC50 File Offset: 0x0011AC50
		// (remove) Token: 0x06004D52 RID: 19794 RVA: 0x0011BC59 File Offset: 0x0011AC59
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event MouseEventHandler MouseDown
		{
			add
			{
				base.MouseDown += value;
			}
			remove
			{
				base.MouseDown -= value;
			}
		}

		// Token: 0x140002D3 RID: 723
		// (add) Token: 0x06004D53 RID: 19795 RVA: 0x0011BC62 File Offset: 0x0011AC62
		// (remove) Token: 0x06004D54 RID: 19796 RVA: 0x0011BC6B File Offset: 0x0011AC6B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public new event MouseEventHandler MouseUp
		{
			add
			{
				base.MouseUp += value;
			}
			remove
			{
				base.MouseUp -= value;
			}
		}

		// Token: 0x140002D4 RID: 724
		// (add) Token: 0x06004D55 RID: 19797 RVA: 0x0011BC74 File Offset: 0x0011AC74
		// (remove) Token: 0x06004D56 RID: 19798 RVA: 0x0011BC7D File Offset: 0x0011AC7D
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public new event MouseEventHandler MouseMove
		{
			add
			{
				base.MouseMove += value;
			}
			remove
			{
				base.MouseMove -= value;
			}
		}

		// Token: 0x140002D5 RID: 725
		// (add) Token: 0x06004D57 RID: 19799 RVA: 0x0011BC86 File Offset: 0x0011AC86
		// (remove) Token: 0x06004D58 RID: 19800 RVA: 0x0011BC8F File Offset: 0x0011AC8F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new event EventHandler MouseEnter
		{
			add
			{
				base.MouseEnter += value;
			}
			remove
			{
				base.MouseEnter -= value;
			}
		}

		// Token: 0x140002D6 RID: 726
		// (add) Token: 0x06004D59 RID: 19801 RVA: 0x0011BC98 File Offset: 0x0011AC98
		// (remove) Token: 0x06004D5A RID: 19802 RVA: 0x0011BCA1 File Offset: 0x0011ACA1
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public new event EventHandler MouseLeave
		{
			add
			{
				base.MouseLeave += value;
			}
			remove
			{
				base.MouseLeave -= value;
			}
		}

		// Token: 0x140002D7 RID: 727
		// (add) Token: 0x06004D5B RID: 19803 RVA: 0x0011BCAA File Offset: 0x0011ACAA
		// (remove) Token: 0x06004D5C RID: 19804 RVA: 0x0011BCBD File Offset: 0x0011ACBD
		[SRDescription("PropertyGridPropertyValueChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event PropertyValueChangedEventHandler PropertyValueChanged
		{
			add
			{
				base.Events.AddHandler(PropertyGrid.EventPropertyValueChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(PropertyGrid.EventPropertyValueChanged, value);
			}
		}

		// Token: 0x140002D8 RID: 728
		// (add) Token: 0x06004D5D RID: 19805 RVA: 0x0011BCD0 File Offset: 0x0011ACD0
		// (remove) Token: 0x06004D5E RID: 19806 RVA: 0x0011BCE3 File Offset: 0x0011ACE3
		event ComponentRenameEventHandler IComPropertyBrowser.ComComponentNameChanged
		{
			add
			{
				base.Events.AddHandler(PropertyGrid.EventComComponentNameChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(PropertyGrid.EventComComponentNameChanged, value);
			}
		}

		// Token: 0x140002D9 RID: 729
		// (add) Token: 0x06004D5F RID: 19807 RVA: 0x0011BCF6 File Offset: 0x0011ACF6
		// (remove) Token: 0x06004D60 RID: 19808 RVA: 0x0011BD09 File Offset: 0x0011AD09
		[SRCategory("CatPropertyChanged")]
		[SRDescription("PropertyGridPropertyTabchangedDescr")]
		public event PropertyTabChangedEventHandler PropertyTabChanged
		{
			add
			{
				base.Events.AddHandler(PropertyGrid.EventPropertyTabChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(PropertyGrid.EventPropertyTabChanged, value);
			}
		}

		// Token: 0x140002DA RID: 730
		// (add) Token: 0x06004D61 RID: 19809 RVA: 0x0011BD1C File Offset: 0x0011AD1C
		// (remove) Token: 0x06004D62 RID: 19810 RVA: 0x0011BD2F File Offset: 0x0011AD2F
		[SRDescription("PropertyGridPropertySortChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event EventHandler PropertySortChanged
		{
			add
			{
				base.Events.AddHandler(PropertyGrid.EventPropertySortChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(PropertyGrid.EventPropertySortChanged, value);
			}
		}

		// Token: 0x140002DB RID: 731
		// (add) Token: 0x06004D63 RID: 19811 RVA: 0x0011BD42 File Offset: 0x0011AD42
		// (remove) Token: 0x06004D64 RID: 19812 RVA: 0x0011BD55 File Offset: 0x0011AD55
		[SRDescription("PropertyGridSelectedGridItemChangedDescr")]
		[SRCategory("CatPropertyChanged")]
		public event SelectedGridItemChangedEventHandler SelectedGridItemChanged
		{
			add
			{
				base.Events.AddHandler(PropertyGrid.EventSelectedGridItemChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(PropertyGrid.EventSelectedGridItemChanged, value);
			}
		}

		// Token: 0x140002DC RID: 732
		// (add) Token: 0x06004D65 RID: 19813 RVA: 0x0011BD68 File Offset: 0x0011AD68
		// (remove) Token: 0x06004D66 RID: 19814 RVA: 0x0011BD7B File Offset: 0x0011AD7B
		[SRCategory("CatPropertyChanged")]
		[SRDescription("PropertyGridSelectedObjectsChangedDescr")]
		public event EventHandler SelectedObjectsChanged
		{
			add
			{
				base.Events.AddHandler(PropertyGrid.EventSelectedObjectsChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(PropertyGrid.EventSelectedObjectsChanged, value);
			}
		}

		// Token: 0x06004D67 RID: 19815 RVA: 0x0011BD8E File Offset: 0x0011AD8E
		internal void AddTab(Type tabType, PropertyTabScope scope)
		{
			this.AddRefTab(tabType, null, scope, true);
		}

		// Token: 0x06004D68 RID: 19816 RVA: 0x0011BD9C File Offset: 0x0011AD9C
		internal void AddRefTab(Type tabType, object component, PropertyTabScope type, bool setupToolbar)
		{
			PropertyTab propertyTab = null;
			int num = -1;
			if (this.viewTabs != null)
			{
				for (int i = 0; i < this.viewTabs.Length; i++)
				{
					if (tabType == this.viewTabs[i].GetType())
					{
						propertyTab = this.viewTabs[i];
						num = i;
						break;
					}
				}
			}
			else
			{
				num = 0;
			}
			if (propertyTab == null)
			{
				IDesignerHost host = null;
				if (component != null && component is IComponent && ((IComponent)component).Site != null)
				{
					host = (IDesignerHost)((IComponent)component).Site.GetService(typeof(IDesignerHost));
				}
				try
				{
					propertyTab = this.CreateTab(tabType, host);
				}
				catch (Exception)
				{
					return;
				}
				if (this.viewTabs != null)
				{
					num = this.viewTabs.Length;
					if (tabType == this.DefaultTabType)
					{
						num = 0;
					}
					else if (typeof(EventsTab).IsAssignableFrom(tabType))
					{
						num = 1;
					}
					else
					{
						for (int j = 1; j < this.viewTabs.Length; j++)
						{
							if (!(this.viewTabs[j] is EventsTab) && string.Compare(propertyTab.TabName, this.viewTabs[j].TabName, false, CultureInfo.InvariantCulture) < 0)
							{
								num = j;
								break;
							}
						}
					}
				}
				PropertyTab[] array = new PropertyTab[this.viewTabs.Length + 1];
				Array.Copy(this.viewTabs, 0, array, 0, num);
				Array.Copy(this.viewTabs, num, array, num + 1, this.viewTabs.Length - num);
				array[num] = propertyTab;
				this.viewTabs = array;
				this.viewTabsDirty = true;
				PropertyTabScope[] array2 = new PropertyTabScope[this.viewTabScopes.Length + 1];
				Array.Copy(this.viewTabScopes, 0, array2, 0, num);
				Array.Copy(this.viewTabScopes, num, array2, num + 1, this.viewTabScopes.Length - num);
				array2[num] = type;
				this.viewTabScopes = array2;
			}
			if (propertyTab != null && component != null)
			{
				try
				{
					object[] components = propertyTab.Components;
					int num2 = (components == null) ? 0 : components.Length;
					object[] array3 = new object[num2 + 1];
					if (num2 > 0)
					{
						Array.Copy(components, array3, num2);
					}
					array3[num2] = component;
					propertyTab.Components = array3;
				}
				catch (Exception)
				{
					this.RemoveTab(num, false);
				}
			}
			if (setupToolbar)
			{
				this.SetupToolbar();
				this.ShowEventsButton(false);
			}
		}

		// Token: 0x06004D69 RID: 19817 RVA: 0x0011BFD4 File Offset: 0x0011AFD4
		public void CollapseAllGridItems()
		{
			this.gridView.RecursivelyExpand(this.peMain, false, false, -1);
		}

		// Token: 0x06004D6A RID: 19818 RVA: 0x0011BFEA File Offset: 0x0011AFEA
		private void ClearCachedProps()
		{
			if (this.viewTabProps != null)
			{
				this.viewTabProps.Clear();
			}
		}

		// Token: 0x06004D6B RID: 19819 RVA: 0x0011BFFF File Offset: 0x0011AFFF
		internal void ClearValueCaches()
		{
			if (this.peMain != null)
			{
				this.peMain.ClearCachedValues();
			}
		}

		// Token: 0x06004D6C RID: 19820 RVA: 0x0011C014 File Offset: 0x0011B014
		internal void ClearTabs(PropertyTabScope tabScope)
		{
			if (tabScope < PropertyTabScope.Document)
			{
				throw new ArgumentException(SR.GetString("PropertyGridTabScope"));
			}
			this.RemoveTabs(tabScope, true);
		}

		// Token: 0x06004D6D RID: 19821 RVA: 0x0011C032 File Offset: 0x0011B032
		private PropertyGridView CreateGridView(IServiceProvider sp)
		{
			return new PropertyGridView(sp, this);
		}

		// Token: 0x06004D6E RID: 19822 RVA: 0x0011C03C File Offset: 0x0011B03C
		private ToolStripSeparator CreateSeparatorButton()
		{
			return new ToolStripSeparator();
		}

		// Token: 0x06004D6F RID: 19823 RVA: 0x0011C050 File Offset: 0x0011B050
		protected virtual PropertyTab CreatePropertyTab(Type tabType)
		{
			return null;
		}

		// Token: 0x06004D70 RID: 19824 RVA: 0x0011C054 File Offset: 0x0011B054
		private PropertyTab CreateTab(Type tabType, IDesignerHost host)
		{
			PropertyTab propertyTab = this.CreatePropertyTab(tabType);
			if (propertyTab == null)
			{
				ConstructorInfo constructor = tabType.GetConstructor(new Type[]
				{
					typeof(IServiceProvider)
				});
				object obj = null;
				if (constructor == null)
				{
					constructor = tabType.GetConstructor(new Type[]
					{
						typeof(IDesignerHost)
					});
					if (constructor != null)
					{
						obj = host;
					}
				}
				else
				{
					obj = this.Site;
				}
				if (obj != null && constructor != null)
				{
					propertyTab = (PropertyTab)constructor.Invoke(new object[]
					{
						obj
					});
				}
				else
				{
					propertyTab = (PropertyTab)Activator.CreateInstance(tabType);
				}
			}
			if (propertyTab != null)
			{
				Bitmap bitmap = propertyTab.Bitmap;
				if (bitmap == null)
				{
					throw new ArgumentException(SR.GetString("PropertyGridNoBitmap", new object[]
					{
						propertyTab.GetType().FullName
					}));
				}
				Size size = bitmap.Size;
				if (size.Width != 16 || size.Height != 16)
				{
					bitmap = new Bitmap(bitmap, new Size(16, 16));
				}
				string tabName = propertyTab.TabName;
				if (tabName == null || tabName.Length == 0)
				{
					throw new ArgumentException(SR.GetString("PropertyGridTabName", new object[]
					{
						propertyTab.GetType().FullName
					}));
				}
			}
			return propertyTab;
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x0011C198 File Offset: 0x0011B198
		private ToolStripButton CreatePushButton(string toolTipText, int imageIndex, EventHandler eventHandler)
		{
			ToolStripButton toolStripButton = new ToolStripButton();
			toolStripButton.Text = toolTipText;
			toolStripButton.AutoToolTip = true;
			toolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			toolStripButton.ImageIndex = imageIndex;
			toolStripButton.Click += eventHandler;
			toolStripButton.ImageScaling = ToolStripItemImageScaling.SizeToFit;
			return toolStripButton;
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x0011C1D6 File Offset: 0x0011B1D6
		internal void DumpPropsToConsole()
		{
			this.gridView.DumpPropsToConsole(this.peMain, "");
		}

		// Token: 0x06004D73 RID: 19827 RVA: 0x0011C1F0 File Offset: 0x0011B1F0
		private void DisplayHotCommands()
		{
			bool visible = this.hotcommands.Visible;
			IComponent component = null;
			DesignerVerb[] array = null;
			if (this.currentObjects != null && this.currentObjects.Length > 0)
			{
				for (int i = 0; i < this.currentObjects.Length; i++)
				{
					object unwrappedObject = this.GetUnwrappedObject(i);
					if (unwrappedObject is IComponent)
					{
						component = (IComponent)unwrappedObject;
						break;
					}
				}
				if (component != null)
				{
					ISite site = component.Site;
					if (site != null)
					{
						IMenuCommandService menuCommandService = (IMenuCommandService)site.GetService(typeof(IMenuCommandService));
						if (menuCommandService != null)
						{
							array = new DesignerVerb[menuCommandService.Verbs.Count];
							menuCommandService.Verbs.CopyTo(array, 0);
						}
						else if (this.currentObjects.Length == 1 && this.GetUnwrappedObject(0) is IComponent)
						{
							IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
							if (designerHost != null)
							{
								IDesigner designer = designerHost.GetDesigner(component);
								if (designer != null)
								{
									array = new DesignerVerb[designer.Verbs.Count];
									designer.Verbs.CopyTo(array, 0);
								}
							}
						}
					}
				}
			}
			if (!base.DesignMode)
			{
				if (array != null && array.Length > 0)
				{
					this.hotcommands.SetVerbs(component, array);
				}
				else
				{
					this.hotcommands.SetVerbs(null, null);
				}
				if (visible != this.hotcommands.Visible)
				{
					this.OnLayoutInternal(false);
				}
			}
		}

		// Token: 0x06004D74 RID: 19828 RVA: 0x0011C350 File Offset: 0x0011B350
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.GetFlag(2))
				{
					if (this.designerEventService != null)
					{
						this.designerEventService.ActiveDesignerChanged -= this.OnActiveDesignerChanged;
					}
					this.designerEventService = null;
					this.SetFlag(2, false);
				}
				this.ActiveDesigner = null;
				if (this.viewTabs != null)
				{
					for (int i = 0; i < this.viewTabs.Length; i++)
					{
						this.viewTabs[i].Dispose();
					}
					this.viewTabs = null;
				}
				if (this.imageList != null)
				{
					for (int j = 0; j < this.imageList.Length; j++)
					{
						if (this.imageList[j] != null)
						{
							this.imageList[j].Dispose();
						}
					}
					this.imageList = null;
				}
				if (this.bmpAlpha != null)
				{
					this.bmpAlpha.Dispose();
					this.bmpAlpha = null;
				}
				if (this.bmpCategory != null)
				{
					this.bmpCategory.Dispose();
					this.bmpCategory = null;
				}
				if (this.bmpPropPage != null)
				{
					this.bmpPropPage.Dispose();
					this.bmpPropPage = null;
				}
				if (this.lineBrush != null)
				{
					this.lineBrush.Dispose();
					this.lineBrush = null;
				}
				if (this.peMain != null)
				{
					this.peMain.Dispose();
					this.peMain = null;
				}
				if (this.currentObjects != null)
				{
					this.currentObjects = null;
					this.SinkPropertyNotifyEvents();
				}
				this.ClearCachedProps();
				this.currentPropEntries = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06004D75 RID: 19829 RVA: 0x0011C4B4 File Offset: 0x0011B4B4
		private void DividerDraw(int y)
		{
			if (y == -1)
			{
				return;
			}
			Rectangle bounds = this.gridView.Bounds;
			bounds.Y = y - 3;
			bounds.Height = 3;
			PropertyGrid.DrawXorBar(this, bounds);
		}

		// Token: 0x06004D76 RID: 19830 RVA: 0x0011C4EC File Offset: 0x0011B4EC
		private PropertyGrid.SnappableControl DividerInside(int x, int y)
		{
			int num = -1;
			if (this.hotcommands.Visible)
			{
				Point location = this.hotcommands.Location;
				if (y >= location.Y - 3 && y <= location.Y + 1)
				{
					return this.hotcommands;
				}
				num = 0;
			}
			if (this.doccomment.Visible)
			{
				Point location2 = this.doccomment.Location;
				if (y >= location2.Y - 3 && y <= location2.Y + 1)
				{
					return this.doccomment;
				}
				if (num == -1)
				{
					num = 1;
				}
			}
			if (num != -1)
			{
				int y2 = this.gridView.Location.Y;
				int num2 = y2 + this.gridView.Size.Height;
				if (Math.Abs(num2 - y) <= 1 && y > y2)
				{
					switch (num)
					{
					case 0:
						return this.hotcommands;
					case 1:
						return this.doccomment;
					}
				}
			}
			return null;
		}

		// Token: 0x06004D77 RID: 19831 RVA: 0x0011C5D8 File Offset: 0x0011B5D8
		private int DividerLimitHigh(PropertyGrid.SnappableControl target)
		{
			int num = this.gridView.Location.Y + 20;
			if (target == this.doccomment && this.hotcommands.Visible)
			{
				num += this.hotcommands.Size.Height + 2;
			}
			return num;
		}

		// Token: 0x06004D78 RID: 19832 RVA: 0x0011C62C File Offset: 0x0011B62C
		private int DividerLimitMove(PropertyGrid.SnappableControl target, int y)
		{
			Rectangle bounds = target.Bounds;
			int val = Math.Min(bounds.Y + bounds.Height - 15, y);
			return Math.Max(this.DividerLimitHigh(target), val);
		}

		// Token: 0x06004D79 RID: 19833 RVA: 0x0011C66C File Offset: 0x0011B66C
		private static void DrawXorBar(Control ctlDrawTo, Rectangle rcFrame)
		{
			Rectangle rectangle = ctlDrawTo.RectangleToScreen(rcFrame);
			if (rectangle.Width < rectangle.Height)
			{
				for (int i = 0; i < rectangle.Width; i++)
				{
					ControlPaint.DrawReversibleLine(new Point(rectangle.X + i, rectangle.Y), new Point(rectangle.X + i, rectangle.Y + rectangle.Height), ctlDrawTo.BackColor);
				}
				return;
			}
			for (int j = 0; j < rectangle.Height; j++)
			{
				ControlPaint.DrawReversibleLine(new Point(rectangle.X, rectangle.Y + j), new Point(rectangle.X + rectangle.Width, rectangle.Y + j), ctlDrawTo.BackColor);
			}
		}

		// Token: 0x06004D7A RID: 19834 RVA: 0x0011C730 File Offset: 0x0011B730
		void IComPropertyBrowser.DropDownDone()
		{
			this.GetPropertyGridView().DropDownDone();
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x0011C740 File Offset: 0x0011B740
		private bool EnablePropPageButton(object obj)
		{
			if (obj == null)
			{
				this.btnViewPropertyPages.Enabled = false;
				return false;
			}
			IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
			bool flag;
			if (iuiservice != null)
			{
				flag = iuiservice.CanShowComponentEditor(obj);
			}
			else
			{
				flag = (TypeDescriptor.GetEditor(obj, typeof(ComponentEditor)) != null);
			}
			this.btnViewPropertyPages.Enabled = flag;
			return flag;
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x0011C7A8 File Offset: 0x0011B7A8
		private void EnableTabs()
		{
			if (this.currentObjects != null)
			{
				this.SetupToolbar();
				for (int i = 1; i < this.viewTabs.Length; i++)
				{
					bool flag = true;
					for (int j = 0; j < this.currentObjects.Length; j++)
					{
						try
						{
							if (!this.viewTabs[i].CanExtend(this.GetUnwrappedObject(j)))
							{
								flag = false;
								break;
							}
						}
						catch (Exception)
						{
							flag = false;
							break;
						}
					}
					if (flag != this.viewTabButtons[i].Visible)
					{
						this.viewTabButtons[i].Visible = flag;
						if (!flag && i == this.selectedViewTab)
						{
							this.SelectViewTabButton(this.viewTabButtons[0], true);
						}
					}
				}
			}
		}

		// Token: 0x06004D7D RID: 19837 RVA: 0x0011C858 File Offset: 0x0011B858
		private void EnsureDesignerEventService()
		{
			if (this.GetFlag(2))
			{
				return;
			}
			this.designerEventService = (IDesignerEventService)this.GetService(typeof(IDesignerEventService));
			if (this.designerEventService != null)
			{
				this.SetFlag(2, true);
				this.designerEventService.ActiveDesignerChanged += this.OnActiveDesignerChanged;
				this.OnActiveDesignerChanged(null, new ActiveDesignerEventArgs(null, this.designerEventService.ActiveDesigner));
			}
		}

		// Token: 0x06004D7E RID: 19838 RVA: 0x0011C8CC File Offset: 0x0011B8CC
		private void EnsureLargeButtons()
		{
			if (this.imageList[1] == null)
			{
				this.imageList[1] = new ImageList();
				this.imageList[1].ImageSize = new Size(32, 32);
				ImageList.ImageCollection images = this.imageList[0].Images;
				for (int i = 0; i < images.Count; i++)
				{
					if (images[i] is Bitmap)
					{
						this.imageList[1].Images.Add(new Bitmap((Bitmap)images[i], 32, 32));
					}
				}
			}
		}

		// Token: 0x06004D7F RID: 19839 RVA: 0x0011C95C File Offset: 0x0011B95C
		bool IComPropertyBrowser.EnsurePendingChangesCommitted()
		{
			bool result;
			try
			{
				if (this.designerHost != null)
				{
					this.designerHost.TransactionOpened -= this.OnTransactionOpened;
					this.designerHost.TransactionClosed -= this.OnTransactionClosed;
				}
				result = this.GetPropertyGridView().EnsurePendingChangesCommitted();
			}
			finally
			{
				if (this.designerHost != null)
				{
					this.designerHost.TransactionOpened += this.OnTransactionOpened;
					this.designerHost.TransactionClosed += this.OnTransactionClosed;
				}
			}
			return result;
		}

		// Token: 0x06004D80 RID: 19840 RVA: 0x0011C9F8 File Offset: 0x0011B9F8
		public void ExpandAllGridItems()
		{
			this.gridView.RecursivelyExpand(this.peMain, false, true, 10);
		}

		// Token: 0x06004D81 RID: 19841 RVA: 0x0011CA10 File Offset: 0x0011BA10
		private static Type[] GetCommonTabs(object[] objs, PropertyTabScope tabScope)
		{
			if (objs == null || objs.Length == 0)
			{
				return new Type[0];
			}
			Type[] array = new Type[5];
			int num = 0;
			PropertyTabAttribute propertyTabAttribute = (PropertyTabAttribute)TypeDescriptor.GetAttributes(objs[0])[typeof(PropertyTabAttribute)];
			if (propertyTabAttribute == null)
			{
				return new Type[0];
			}
			int i;
			for (i = 0; i < propertyTabAttribute.TabScopes.Length; i++)
			{
				PropertyTabScope propertyTabScope = propertyTabAttribute.TabScopes[i];
				if (propertyTabScope == tabScope)
				{
					if (num == array.Length)
					{
						Type[] array2 = new Type[num * 2];
						Array.Copy(array, 0, array2, 0, num);
						array = array2;
					}
					array[num++] = propertyTabAttribute.TabClasses[i];
				}
			}
			if (num == 0)
			{
				return new Type[0];
			}
			i = 1;
			while (i < objs.Length && num > 0)
			{
				propertyTabAttribute = (PropertyTabAttribute)TypeDescriptor.GetAttributes(objs[i])[typeof(PropertyTabAttribute)];
				if (propertyTabAttribute == null)
				{
					return new Type[0];
				}
				for (int j = 0; j < num; j++)
				{
					bool flag = false;
					for (int k = 0; k < propertyTabAttribute.TabClasses.Length; k++)
					{
						if (propertyTabAttribute.TabClasses[k] == array[j])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						array[j] = array[num - 1];
						array[num - 1] = null;
						num--;
						j--;
					}
				}
				i++;
			}
			Type[] array3 = new Type[num];
			if (num > 0)
			{
				Array.Copy(array, 0, array3, 0, num);
			}
			return array3;
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x0011CB69 File Offset: 0x0011BB69
		internal GridEntry GetDefaultGridEntry()
		{
			if (this.peDefault == null && this.currentPropEntries != null)
			{
				this.peDefault = (GridEntry)this.currentPropEntries[0];
			}
			return this.peDefault;
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x0011CB98 File Offset: 0x0011BB98
		private object GetUnwrappedObject(int index)
		{
			if (this.currentObjects == null || index < 0 || index > this.currentObjects.Length)
			{
				return null;
			}
			object obj = this.currentObjects[index];
			if (obj is ICustomTypeDescriptor)
			{
				obj = ((ICustomTypeDescriptor)obj).GetPropertyOwner(null);
			}
			return obj;
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x0011CBDD File Offset: 0x0011BBDD
		internal GridEntryCollection GetPropEntries()
		{
			if (this.currentPropEntries == null)
			{
				this.UpdateSelection();
			}
			this.SetFlag(1, false);
			return this.currentPropEntries;
		}

		// Token: 0x06004D85 RID: 19845 RVA: 0x0011CBFB File Offset: 0x0011BBFB
		private PropertyGridView GetPropertyGridView()
		{
			return this.gridView;
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x0011CC03 File Offset: 0x0011BC03
		void IComPropertyBrowser.HandleF4()
		{
			if (this.gridView.ContainsFocus)
			{
				return;
			}
			if (base.ActiveControl != this.gridView)
			{
				base.SetActiveControlInternal(this.gridView);
			}
			this.gridView.FocusInternal();
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x0011CC39 File Offset: 0x0011BC39
		internal bool HavePropEntriesChanged()
		{
			return this.GetFlag(1);
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x0011CC44 File Offset: 0x0011BC44
		void IComPropertyBrowser.LoadState(RegistryKey optRoot)
		{
			if (optRoot != null)
			{
				object value = optRoot.GetValue("PbrsAlpha", "0");
				if (value != null && value.ToString().Equals("1"))
				{
					this.PropertySort = PropertySort.Alphabetical;
				}
				else
				{
					this.PropertySort = PropertySort.CategorizedAlphabetical;
				}
				value = optRoot.GetValue("PbrsShowDesc", "1");
				this.HelpVisible = (value != null && value.ToString().Equals("1"));
				value = optRoot.GetValue("PbrsShowCommands", "0");
				this.CommandsVisibleIfAvailable = (value != null && value.ToString().Equals("1"));
				value = optRoot.GetValue("PbrsDescHeightRatio", "-1");
				bool flag = false;
				if (value is string)
				{
					int num = int.Parse((string)value, CultureInfo.InvariantCulture);
					if (num > 0)
					{
						this.dcSizeRatio = num;
						flag = true;
					}
				}
				value = optRoot.GetValue("PbrsHotCommandHeightRatio", "-1");
				if (value is string)
				{
					int num2 = int.Parse((string)value, CultureInfo.InvariantCulture);
					if (num2 > 0)
					{
						this.dcSizeRatio = num2;
						flag = true;
					}
				}
				if (flag)
				{
					this.OnLayoutInternal(false);
					return;
				}
			}
			else
			{
				this.PropertySort = PropertySort.CategorizedAlphabetical;
				this.HelpVisible = true;
				this.CommandsVisibleIfAvailable = false;
			}
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x0011CD78 File Offset: 0x0011BD78
		private void OnActiveDesignerChanged(object sender, ActiveDesignerEventArgs e)
		{
			if (e.OldDesigner != null && e.OldDesigner == this.designerHost)
			{
				this.ActiveDesigner = null;
			}
			if (e.NewDesigner != null && e.NewDesigner != this.designerHost)
			{
				this.ActiveDesigner = e.NewDesigner;
			}
		}

		// Token: 0x06004D8A RID: 19850 RVA: 0x0011CDC4 File Offset: 0x0011BDC4
		void UnsafeNativeMethods.IPropertyNotifySink.OnChanged(int dispID)
		{
			bool flag = false;
			PropertyDescriptorGridEntry propertyDescriptorGridEntry = this.gridView.SelectedGridEntry as PropertyDescriptorGridEntry;
			if (propertyDescriptorGridEntry != null && propertyDescriptorGridEntry.PropertyDescriptor != null && propertyDescriptorGridEntry.PropertyDescriptor.Attributes != null)
			{
				DispIdAttribute dispIdAttribute = (DispIdAttribute)propertyDescriptorGridEntry.PropertyDescriptor.Attributes[typeof(DispIdAttribute)];
				if (dispIdAttribute != null && !dispIdAttribute.IsDefaultAttribute())
				{
					flag = (dispID != dispIdAttribute.Value);
				}
			}
			if (!this.GetFlag(512))
			{
				if (!this.gridView.GetInPropertySet() || flag)
				{
					this.Refresh(flag);
				}
				object unwrappedObject = this.GetUnwrappedObject(0);
				if (ComNativeDescriptor.Instance.IsNameDispId(unwrappedObject, dispID) || dispID == -800)
				{
					this.OnComComponentNameChanged(new ComponentRenameEventArgs(unwrappedObject, null, TypeDescriptor.GetClassName(unwrappedObject)));
				}
			}
		}

		// Token: 0x06004D8B RID: 19851 RVA: 0x0011CE8C File Offset: 0x0011BE8C
		private void OnChildMouseMove(object sender, MouseEventArgs me)
		{
			Point empty = Point.Empty;
			if (this.ShouldForwardChildMouseMessage((Control)sender, me, ref empty))
			{
				this.OnMouseMove(new MouseEventArgs(me.Button, me.Clicks, empty.X, empty.Y, me.Delta));
			}
		}

		// Token: 0x06004D8C RID: 19852 RVA: 0x0011CEDC File Offset: 0x0011BEDC
		private void OnChildMouseDown(object sender, MouseEventArgs me)
		{
			Point empty = Point.Empty;
			if (this.ShouldForwardChildMouseMessage((Control)sender, me, ref empty))
			{
				this.OnMouseDown(new MouseEventArgs(me.Button, me.Clicks, empty.X, empty.Y, me.Delta));
			}
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x0011CF2C File Offset: 0x0011BF2C
		private void OnComponentAdd(object sender, ComponentEventArgs e)
		{
			PropertyTabAttribute propertyTabAttribute = (PropertyTabAttribute)TypeDescriptor.GetAttributes(e.Component.GetType())[typeof(PropertyTabAttribute)];
			if (propertyTabAttribute == null)
			{
				return;
			}
			for (int i = 0; i < propertyTabAttribute.TabClasses.Length; i++)
			{
				if (propertyTabAttribute.TabScopes[i] == PropertyTabScope.Document)
				{
					this.AddRefTab(propertyTabAttribute.TabClasses[i], e.Component, PropertyTabScope.Document, true);
				}
			}
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x0011CF98 File Offset: 0x0011BF98
		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			bool flag = this.GetFlag(16);
			if (flag || this.GetFlag(4) || this.gridView.GetInPropertySet() || this.currentObjects == null || this.currentObjects.Length == 0)
			{
				if (flag && !this.gridView.GetInPropertySet())
				{
					this.SetFlag(256, true);
				}
				return;
			}
			int num = this.currentObjects.Length;
			for (int i = 0; i < num; i++)
			{
				if (this.currentObjects[i] == e.Component)
				{
					this.Refresh(false);
					return;
				}
			}
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x0011D024 File Offset: 0x0011C024
		private void OnComponentRemove(object sender, ComponentEventArgs e)
		{
			PropertyTabAttribute propertyTabAttribute = (PropertyTabAttribute)TypeDescriptor.GetAttributes(e.Component.GetType())[typeof(PropertyTabAttribute)];
			if (propertyTabAttribute == null)
			{
				return;
			}
			for (int i = 0; i < propertyTabAttribute.TabClasses.Length; i++)
			{
				if (propertyTabAttribute.TabScopes[i] == PropertyTabScope.Document)
				{
					this.ReleaseTab(propertyTabAttribute.TabClasses[i], e.Component);
				}
			}
			for (int j = 0; j < this.currentObjects.Length; j++)
			{
				if (e.Component == this.currentObjects[j])
				{
					object[] array = new object[this.currentObjects.Length - 1];
					Array.Copy(this.currentObjects, 0, array, 0, j);
					if (j < array.Length)
					{
						Array.Copy(this.currentObjects, 0, array, j + j, array.Length - j);
					}
					if (!this.GetFlag(16))
					{
						this.SelectedObjects = array;
					}
					else
					{
						this.gridView.ClearProps();
						this.currentObjects = array;
						this.SetFlag(128, true);
					}
				}
			}
			this.SetupToolbar();
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x0011D123 File Offset: 0x0011C123
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			this.Refresh();
		}

		// Token: 0x06004D91 RID: 19857 RVA: 0x0011D132 File Offset: 0x0011C132
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.Refresh();
		}

		// Token: 0x06004D92 RID: 19858 RVA: 0x0011D141 File Offset: 0x0011C141
		internal void OnGridViewMouseWheel(MouseEventArgs e)
		{
			this.OnMouseWheel(e);
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x0011D14A File Offset: 0x0011C14A
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			this.OnLayoutInternal(false);
			TypeDescriptor.Refreshed += this.OnTypeDescriptorRefreshed;
			if (this.currentObjects != null && this.currentObjects.Length > 0)
			{
				this.Refresh(true);
			}
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x0011D185 File Offset: 0x0011C185
		protected override void OnHandleDestroyed(EventArgs e)
		{
			TypeDescriptor.Refreshed -= this.OnTypeDescriptorRefreshed;
			base.OnHandleDestroyed(e);
		}

		// Token: 0x06004D95 RID: 19861 RVA: 0x0011D19F File Offset: 0x0011C19F
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if (base.ActiveControl == null)
			{
				base.SetActiveControlInternal(this.gridView);
				return;
			}
			if (!base.ActiveControl.FocusInternal())
			{
				base.SetActiveControlInternal(this.gridView);
			}
		}

		// Token: 0x06004D96 RID: 19862 RVA: 0x0011D1D8 File Offset: 0x0011C1D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void ScaleCore(float dx, float dy)
		{
			int num = (int)Math.Round((double)((float)base.Left * dx));
			int num2 = (int)Math.Round((double)((float)base.Top * dy));
			int width = base.Width;
			width = (int)Math.Round((double)((float)(base.Left + base.Width) * dx - (float)num));
			int height = base.Height;
			height = (int)Math.Round((double)((float)(base.Top + base.Height) * dy - (float)num2));
			base.SetBounds(num, num2, width, height, BoundsSpecified.All);
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x0011D258 File Offset: 0x0011C258
		private void OnLayoutInternal(bool dividerOnly)
		{
			if (!base.IsHandleCreated || !base.Visible)
			{
				return;
			}
			try
			{
				this.FreezePainting = true;
				if (!dividerOnly)
				{
					if (!this.toolStrip.Visible && !this.doccomment.Visible && !this.hotcommands.Visible)
					{
						this.gridView.Location = new Point(0, 0);
						this.gridView.Size = base.Size;
						return;
					}
					if (this.toolStrip.Visible)
					{
						int width = base.Width;
						int height = this.LargeButtons ? 41 : 25;
						Rectangle bounds = new Rectangle(0, 1, width, height);
						this.toolStrip.Bounds = bounds;
						int y = this.gridView.Location.Y;
						this.gridView.Location = new Point(0, this.toolStrip.Height + this.toolStrip.Top);
					}
					else
					{
						this.gridView.Location = new Point(0, 0);
					}
				}
				int num = base.Size.Height;
				if (num >= 20)
				{
					int num2 = num - (this.gridView.Location.Y + 20);
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					if (dividerOnly)
					{
						num3 = (this.doccomment.Visible ? this.doccomment.Size.Height : 0);
						num4 = (this.hotcommands.Visible ? this.hotcommands.Size.Height : 0);
					}
					else
					{
						if (this.doccomment.Visible)
						{
							num5 = this.doccomment.GetOptimalHeight(base.Size.Width - 3);
							if (this.doccomment.userSized)
							{
								num3 = this.doccomment.Size.Height;
							}
							else if (this.dcSizeRatio != -1)
							{
								num3 = base.Height * this.dcSizeRatio / 100;
							}
							else
							{
								num3 = num5;
							}
						}
						if (this.hotcommands.Visible)
						{
							num6 = this.hotcommands.GetOptimalHeight(base.Size.Width - 3);
							if (this.hotcommands.userSized)
							{
								num4 = this.hotcommands.Size.Height;
							}
							else if (this.hcSizeRatio != -1)
							{
								num4 = base.Height * this.hcSizeRatio / 100;
							}
							else
							{
								num4 = num6;
							}
						}
					}
					if (num3 > 0)
					{
						num2 -= 3;
						int num7;
						if (num4 == 0 || num3 + num4 < num2)
						{
							num7 = Math.Min(num3, num2);
						}
						else if (num4 > 0 && num4 < num2)
						{
							num7 = num2 - num4;
						}
						else
						{
							num7 = Math.Min(num3, num2 / 2 - 1);
						}
						num7 = Math.Max(num7, 6);
						this.doccomment.SetBounds(0, num - num7, base.Size.Width, num7);
						if (num7 <= num5 && num7 < num3)
						{
							this.doccomment.userSized = false;
						}
						else if (this.dcSizeRatio != -1 || this.doccomment.userSized)
						{
							this.dcSizeRatio = this.doccomment.Height * 100 / base.Height;
						}
						this.doccomment.Invalidate();
						num = this.doccomment.Location.Y - 3;
						num2 -= num7;
					}
					if (num4 > 0)
					{
						num2 -= 3;
						int num7;
						if (num2 > num4)
						{
							num7 = Math.Min(num4, num2);
						}
						else
						{
							num7 = num2;
						}
						num7 = Math.Max(num7, 6);
						if (num7 <= num6 && num7 < num4)
						{
							this.hotcommands.userSized = false;
						}
						else if (this.hcSizeRatio != -1 || this.hotcommands.userSized)
						{
							this.hcSizeRatio = this.hotcommands.Height * 100 / base.Height;
						}
						this.hotcommands.SetBounds(0, num - num7, base.Size.Width, num7);
						this.hotcommands.Invalidate();
						num = this.hotcommands.Location.Y - 3;
					}
					this.gridView.Size = new Size(base.Size.Width, num - this.gridView.Location.Y);
				}
			}
			finally
			{
				this.FreezePainting = false;
			}
		}

		// Token: 0x06004D98 RID: 19864 RVA: 0x0011D6D8 File Offset: 0x0011C6D8
		protected override void OnMouseDown(MouseEventArgs me)
		{
			PropertyGrid.SnappableControl snappableControl = this.DividerInside(me.X, me.Y);
			if (snappableControl != null && me.Button == MouseButtons.Left)
			{
				base.CaptureInternal = true;
				this.targetMove = snappableControl;
				this.dividerMoveY = me.Y;
				this.DividerDraw(this.dividerMoveY);
			}
			base.OnMouseDown(me);
		}

		// Token: 0x06004D99 RID: 19865 RVA: 0x0011D738 File Offset: 0x0011C738
		protected override void OnMouseMove(MouseEventArgs me)
		{
			if (this.dividerMoveY != -1)
			{
				int num = this.DividerLimitMove(this.targetMove, me.Y);
				if (num != this.dividerMoveY)
				{
					this.DividerDraw(this.dividerMoveY);
					this.dividerMoveY = num;
					this.DividerDraw(this.dividerMoveY);
				}
				base.OnMouseMove(me);
				return;
			}
			if (this.DividerInside(me.X, me.Y) != null)
			{
				this.Cursor = Cursors.HSplit;
				return;
			}
			this.Cursor = null;
		}

		// Token: 0x06004D9A RID: 19866 RVA: 0x0011D7B8 File Offset: 0x0011C7B8
		protected override void OnMouseUp(MouseEventArgs me)
		{
			if (this.dividerMoveY == -1)
			{
				return;
			}
			this.Cursor = null;
			this.DividerDraw(this.dividerMoveY);
			this.dividerMoveY = this.DividerLimitMove(this.targetMove, me.Y);
			Rectangle bounds = this.targetMove.Bounds;
			if (this.dividerMoveY != bounds.Y)
			{
				int val = bounds.Height + bounds.Y - this.dividerMoveY - 1;
				Size size = this.targetMove.Size;
				size.Height = Math.Max(0, val);
				this.targetMove.Size = size;
				this.targetMove.userSized = true;
				this.OnLayoutInternal(true);
				base.Invalidate(new Rectangle(0, me.Y - 3, base.Size.Width, me.Y + 3));
				this.gridView.Invalidate(new Rectangle(0, this.gridView.Size.Height - 3, base.Size.Width, 3));
			}
			base.CaptureInternal = false;
			this.dividerMoveY = -1;
			this.targetMove = null;
			base.OnMouseUp(me);
		}

		// Token: 0x06004D9B RID: 19867 RVA: 0x0011D8E7 File Offset: 0x0011C8E7
		int UnsafeNativeMethods.IPropertyNotifySink.OnRequestEdit(int dispID)
		{
			return 0;
		}

		// Token: 0x06004D9C RID: 19868 RVA: 0x0011D8EA File Offset: 0x0011C8EA
		protected override void OnResize(EventArgs e)
		{
			if (base.IsHandleCreated && base.Visible)
			{
				this.OnLayoutInternal(false);
			}
			base.OnResize(e);
		}

		// Token: 0x06004D9D RID: 19869 RVA: 0x0011D90A File Offset: 0x0011C90A
		private void OnButtonClick(object sender, EventArgs e)
		{
			if (sender != this.btnViewPropertyPages)
			{
				this.gridView.FocusInternal();
			}
		}

		// Token: 0x06004D9E RID: 19870 RVA: 0x0011D924 File Offset: 0x0011C924
		protected void OnComComponentNameChanged(ComponentRenameEventArgs e)
		{
			ComponentRenameEventHandler componentRenameEventHandler = (ComponentRenameEventHandler)base.Events[PropertyGrid.EventComComponentNameChanged];
			if (componentRenameEventHandler != null)
			{
				componentRenameEventHandler(this, e);
			}
		}

		// Token: 0x06004D9F RID: 19871 RVA: 0x0011D952 File Offset: 0x0011C952
		protected void OnNotifyPropertyValueUIItemsChanged(object sender, EventArgs e)
		{
			this.gridView.LabelPaintMargin = 0;
			this.gridView.Invalidate(true);
		}

		// Token: 0x06004DA0 RID: 19872 RVA: 0x0011D96C File Offset: 0x0011C96C
		protected override void OnPaint(PaintEventArgs pevent)
		{
			Point location = this.gridView.Location;
			int width = base.Size.Width;
			Brush brush;
			if (this.BackColor.IsSystemColor)
			{
				brush = SystemBrushes.FromSystemColor(this.BackColor);
			}
			else
			{
				brush = new SolidBrush(this.BackColor);
			}
			pevent.Graphics.FillRectangle(brush, new Rectangle(0, 0, width, location.Y));
			int num = location.Y + this.gridView.Size.Height;
			if (this.hotcommands.Visible)
			{
				pevent.Graphics.FillRectangle(brush, new Rectangle(0, num, width, this.hotcommands.Location.Y - num));
				num += this.hotcommands.Size.Height;
			}
			if (this.doccomment.Visible)
			{
				pevent.Graphics.FillRectangle(brush, new Rectangle(0, num, width, this.doccomment.Location.Y - num));
				num += this.doccomment.Size.Height;
			}
			pevent.Graphics.FillRectangle(brush, new Rectangle(0, num, width, base.Size.Height - num));
			if (!this.BackColor.IsSystemColor)
			{
				brush.Dispose();
			}
			base.OnPaint(pevent);
			if (this.lineBrush != null)
			{
				this.lineBrush.Dispose();
				this.lineBrush = null;
			}
		}

		// Token: 0x06004DA1 RID: 19873 RVA: 0x0011DAF4 File Offset: 0x0011CAF4
		protected virtual void OnPropertySortChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[PropertyGrid.EventPropertySortChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004DA2 RID: 19874 RVA: 0x0011DB24 File Offset: 0x0011CB24
		protected virtual void OnPropertyTabChanged(PropertyTabChangedEventArgs e)
		{
			PropertyTabChangedEventHandler propertyTabChangedEventHandler = (PropertyTabChangedEventHandler)base.Events[PropertyGrid.EventPropertyTabChanged];
			if (propertyTabChangedEventHandler != null)
			{
				propertyTabChangedEventHandler(this, e);
			}
		}

		// Token: 0x06004DA3 RID: 19875 RVA: 0x0011DB54 File Offset: 0x0011CB54
		protected virtual void OnPropertyValueChanged(PropertyValueChangedEventArgs e)
		{
			PropertyValueChangedEventHandler propertyValueChangedEventHandler = (PropertyValueChangedEventHandler)base.Events[PropertyGrid.EventPropertyValueChanged];
			if (propertyValueChangedEventHandler != null)
			{
				propertyValueChangedEventHandler(this, e);
			}
		}

		// Token: 0x06004DA4 RID: 19876 RVA: 0x0011DB82 File Offset: 0x0011CB82
		internal void OnPropertyValueSet(GridItem changedItem, object oldValue)
		{
			this.OnPropertyValueChanged(new PropertyValueChangedEventArgs(changedItem, oldValue));
		}

		// Token: 0x06004DA5 RID: 19877 RVA: 0x0011DB91 File Offset: 0x0011CB91
		internal void OnSelectedGridItemChanged(GridEntry oldEntry, GridEntry newEntry)
		{
			this.OnSelectedGridItemChanged(new SelectedGridItemChangedEventArgs(oldEntry, newEntry));
		}

		// Token: 0x06004DA6 RID: 19878 RVA: 0x0011DBA0 File Offset: 0x0011CBA0
		protected virtual void OnSelectedGridItemChanged(SelectedGridItemChangedEventArgs e)
		{
			SelectedGridItemChangedEventHandler selectedGridItemChangedEventHandler = (SelectedGridItemChangedEventHandler)base.Events[PropertyGrid.EventSelectedGridItemChanged];
			if (selectedGridItemChangedEventHandler != null)
			{
				selectedGridItemChangedEventHandler(this, e);
			}
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x0011DBD0 File Offset: 0x0011CBD0
		protected virtual void OnSelectedObjectsChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[PropertyGrid.EventSelectedObjectsChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004DA8 RID: 19880 RVA: 0x0011DC00 File Offset: 0x0011CC00
		private void OnTransactionClosed(object sender, DesignerTransactionCloseEventArgs e)
		{
			if (e.LastTransaction)
			{
				IComponent component = this.SelectedObject as IComponent;
				if (component != null && component.Site == null)
				{
					this.SelectedObject = null;
					return;
				}
				this.SetFlag(16, false);
				if (this.GetFlag(128))
				{
					this.SelectedObjects = this.currentObjects;
					this.SetFlag(128, false);
				}
				else if (this.GetFlag(256))
				{
					this.Refresh(false);
				}
				this.SetFlag(256, false);
			}
		}

		// Token: 0x06004DA9 RID: 19881 RVA: 0x0011DC84 File Offset: 0x0011CC84
		private void OnTransactionOpened(object sender, EventArgs e)
		{
			this.SetFlag(16, true);
		}

		// Token: 0x06004DAA RID: 19882 RVA: 0x0011DC90 File Offset: 0x0011CC90
		private void OnTypeDescriptorRefreshed(RefreshEventArgs e)
		{
			if (base.InvokeRequired)
			{
				base.BeginInvoke(new RefreshEventHandler(this.OnTypeDescriptorRefreshedInvoke), new object[]
				{
					e
				});
				return;
			}
			this.OnTypeDescriptorRefreshedInvoke(e);
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x0011DCCC File Offset: 0x0011CCCC
		private void OnTypeDescriptorRefreshedInvoke(RefreshEventArgs e)
		{
			if (this.currentObjects != null)
			{
				for (int i = 0; i < this.currentObjects.Length; i++)
				{
					Type typeChanged = e.TypeChanged;
					if (this.currentObjects[i] == e.ComponentChanged || (typeChanged != null && typeChanged.IsAssignableFrom(this.currentObjects[i].GetType())))
					{
						this.ClearCachedProps();
						this.Refresh(true);
						return;
					}
				}
			}
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x0011DD34 File Offset: 0x0011CD34
		private void OnViewSortButtonClick(object sender, EventArgs e)
		{
			try
			{
				this.FreezePainting = true;
				if (sender == this.viewSortButtons[this.selectedViewSort])
				{
					this.viewSortButtons[this.selectedViewSort].Checked = true;
					return;
				}
				this.viewSortButtons[this.selectedViewSort].Checked = false;
				int num = 0;
				while (num < this.viewSortButtons.Length && this.viewSortButtons[num] != sender)
				{
					num++;
				}
				this.selectedViewSort = num;
				this.viewSortButtons[this.selectedViewSort].Checked = true;
				switch (this.selectedViewSort)
				{
				case 0:
					this.propertySortValue = PropertySort.CategorizedAlphabetical;
					break;
				case 1:
					this.propertySortValue = PropertySort.Alphabetical;
					break;
				case 2:
					this.propertySortValue = PropertySort.NoSort;
					break;
				}
				this.OnPropertySortChanged(EventArgs.Empty);
				this.Refresh(false);
				this.OnLayoutInternal(false);
			}
			finally
			{
				this.FreezePainting = false;
			}
			this.OnButtonClick(sender, e);
		}

		// Token: 0x06004DAD RID: 19885 RVA: 0x0011DE2C File Offset: 0x0011CE2C
		private void OnViewTabButtonClick(object sender, EventArgs e)
		{
			try
			{
				this.FreezePainting = true;
				this.SelectViewTabButton((ToolStripButton)sender, true);
				this.OnLayoutInternal(false);
				this.SaveTabSelection();
			}
			finally
			{
				this.FreezePainting = false;
			}
			this.OnButtonClick(sender, e);
		}

		// Token: 0x06004DAE RID: 19886 RVA: 0x0011DE7C File Offset: 0x0011CE7C
		private void OnViewButtonClickPP(object sender, EventArgs e)
		{
			if (this.btnViewPropertyPages.Enabled && this.currentObjects != null && this.currentObjects.Length > 0)
			{
				object obj = this.currentObjects[0];
				object component = obj;
				bool flag = false;
				IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
				try
				{
					if (iuiservice != null)
					{
						flag = iuiservice.ShowComponentEditor(component, this);
					}
					else
					{
						try
						{
							ComponentEditor componentEditor = (ComponentEditor)TypeDescriptor.GetEditor(component, typeof(ComponentEditor));
							if (componentEditor != null)
							{
								if (componentEditor is WindowsFormsComponentEditor)
								{
									flag = ((WindowsFormsComponentEditor)componentEditor).EditComponent(null, component, this);
								}
								else
								{
									flag = componentEditor.EditComponent(component);
								}
							}
						}
						catch
						{
						}
					}
					if (flag)
					{
						if (obj is IComponent && this.connectionPointCookies[0] == null)
						{
							ISite site = ((IComponent)obj).Site;
							if (site != null)
							{
								IComponentChangeService componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
								if (componentChangeService != null)
								{
									try
									{
										componentChangeService.OnComponentChanging(obj, null);
									}
									catch (CheckoutException ex)
									{
										if (ex == CheckoutException.Canceled)
										{
											return;
										}
										throw ex;
									}
									try
									{
										this.SetFlag(4, true);
										componentChangeService.OnComponentChanged(obj, null, null, null);
									}
									finally
									{
										this.SetFlag(4, false);
									}
								}
							}
						}
						this.gridView.Refresh();
					}
				}
				catch (Exception ex2)
				{
					string @string = SR.GetString("ErrorPropertyPageFailed");
					if (iuiservice != null)
					{
						iuiservice.ShowError(ex2, @string);
					}
					else
					{
						RTLAwareMessageBox.Show(null, @string, SR.GetString("PropertyGridTitle"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
					}
				}
			}
			this.OnButtonClick(sender, e);
		}

		// Token: 0x06004DAF RID: 19887 RVA: 0x0011E028 File Offset: 0x0011D028
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (base.Visible && base.IsHandleCreated)
			{
				this.OnLayoutInternal(false);
				this.SetupToolbar();
			}
		}

		// Token: 0x06004DB0 RID: 19888 RVA: 0x0011E050 File Offset: 0x0011D050
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			Keys keys = keyData & Keys.KeyCode;
			if (keys != Keys.Tab || (keyData & Keys.Control) != Keys.None || (keyData & Keys.Alt) != Keys.None)
			{
				return base.ProcessDialogKey(keyData);
			}
			if ((keyData & Keys.Shift) != Keys.None)
			{
				if (this.hotcommands.Visible && this.hotcommands.ContainsFocus)
				{
					this.gridView.ReverseFocus();
				}
				else if (this.gridView.FocusInside)
				{
					if (!this.toolStrip.Visible)
					{
						return base.ProcessDialogKey(keyData);
					}
					this.toolStrip.FocusInternal();
				}
				else
				{
					if (this.toolStrip.Focused || !this.toolStrip.Visible)
					{
						return base.ProcessDialogKey(keyData);
					}
					if (this.hotcommands.Visible)
					{
						this.hotcommands.Select(false);
					}
					else if (this.peMain != null)
					{
						this.gridView.ReverseFocus();
					}
					else
					{
						if (!this.toolStrip.Visible)
						{
							return base.ProcessDialogKey(keyData);
						}
						this.toolStrip.FocusInternal();
					}
				}
				return true;
			}
			bool flag = false;
			if (this.toolStrip.Focused)
			{
				if (this.peMain != null)
				{
					this.gridView.FocusInternal();
				}
				else
				{
					base.ProcessDialogKey(keyData);
				}
				return true;
			}
			if (this.gridView.FocusInside)
			{
				if (this.hotcommands.Visible)
				{
					this.hotcommands.Select(true);
					return true;
				}
				flag = true;
			}
			else if (this.hotcommands.ContainsFocus)
			{
				flag = true;
			}
			else if (this.toolStrip.Visible)
			{
				this.toolStrip.FocusInternal();
			}
			else
			{
				this.gridView.FocusInternal();
			}
			if (flag)
			{
				bool flag2 = base.ProcessDialogKey(keyData);
				if (!flag2 && base.Parent == null)
				{
					IntPtr parent = UnsafeNativeMethods.GetParent(new HandleRef(this, base.Handle));
					if (parent != IntPtr.Zero)
					{
						UnsafeNativeMethods.SetFocus(new HandleRef(null, parent));
					}
				}
				return flag2;
			}
			return true;
		}

		// Token: 0x06004DB1 RID: 19889 RVA: 0x0011E242 File Offset: 0x0011D242
		public override void Refresh()
		{
			if (this.GetFlag(512))
			{
				return;
			}
			this.Refresh(true);
			base.Refresh();
		}

		// Token: 0x06004DB2 RID: 19890 RVA: 0x0011E260 File Offset: 0x0011D260
		private void Refresh(bool clearCached)
		{
			if (base.Disposing)
			{
				return;
			}
			if (this.GetFlag(512))
			{
				return;
			}
			try
			{
				this.FreezePainting = true;
				this.SetFlag(512, true);
				if (clearCached)
				{
					this.ClearCachedProps();
				}
				this.RefreshProperties(clearCached);
				this.gridView.Refresh();
				this.DisplayHotCommands();
			}
			finally
			{
				this.FreezePainting = false;
				this.SetFlag(512, false);
			}
		}

		// Token: 0x06004DB3 RID: 19891 RVA: 0x0011E2E0 File Offset: 0x0011D2E0
		internal void RefreshProperties(bool clearCached)
		{
			if (clearCached && this.selectedViewTab != -1 && this.viewTabs != null)
			{
				PropertyTab propertyTab = this.viewTabs[this.selectedViewTab];
				if (propertyTab != null && this.viewTabProps != null)
				{
					string key = propertyTab.TabName + this.propertySortValue.ToString();
					this.viewTabProps.Remove(key);
				}
			}
			this.SetFlag(1, true);
			this.UpdateSelection();
		}

		// Token: 0x06004DB4 RID: 19892 RVA: 0x0011E350 File Offset: 0x0011D350
		public void RefreshTabs(PropertyTabScope tabScope)
		{
			if (tabScope < PropertyTabScope.Document)
			{
				throw new ArgumentException(SR.GetString("PropertyGridTabScope"));
			}
			this.RemoveTabs(tabScope, false);
			if (tabScope <= PropertyTabScope.Component && this.currentObjects != null && this.currentObjects.Length > 0)
			{
				Type[] commonTabs = PropertyGrid.GetCommonTabs(this.currentObjects, PropertyTabScope.Component);
				for (int i = 0; i < commonTabs.Length; i++)
				{
					for (int j = 0; j < this.currentObjects.Length; j++)
					{
						this.AddRefTab(commonTabs[i], this.currentObjects[j], PropertyTabScope.Component, false);
					}
				}
			}
			if (tabScope <= PropertyTabScope.Document && this.designerHost != null)
			{
				IContainer container = this.designerHost.Container;
				if (container != null)
				{
					ComponentCollection components = container.Components;
					if (components != null)
					{
						foreach (object obj in components)
						{
							IComponent component = (IComponent)obj;
							PropertyTabAttribute propertyTabAttribute = (PropertyTabAttribute)TypeDescriptor.GetAttributes(component.GetType())[typeof(PropertyTabAttribute)];
							if (propertyTabAttribute != null)
							{
								for (int k = 0; k < propertyTabAttribute.TabClasses.Length; k++)
								{
									if (propertyTabAttribute.TabScopes[k] == PropertyTabScope.Document)
									{
										this.AddRefTab(propertyTabAttribute.TabClasses[k], component, PropertyTabScope.Document, false);
									}
								}
							}
						}
					}
				}
			}
			this.SetupToolbar();
		}

		// Token: 0x06004DB5 RID: 19893 RVA: 0x0011E4B4 File Offset: 0x0011D4B4
		internal void ReleaseTab(Type tabType, object component)
		{
			PropertyTab propertyTab = null;
			int num = -1;
			for (int i = 0; i < this.viewTabs.Length; i++)
			{
				if (tabType == this.viewTabs[i].GetType())
				{
					propertyTab = this.viewTabs[i];
					num = i;
					break;
				}
			}
			if (propertyTab == null)
			{
				return;
			}
			object[] array = propertyTab.Components;
			bool flag = false;
			try
			{
				int num2 = -1;
				if (array != null)
				{
					num2 = Array.IndexOf<object>(array, component);
				}
				if (num2 >= 0)
				{
					object[] array2 = new object[array.Length - 1];
					Array.Copy(array, 0, array2, 0, num2);
					Array.Copy(array, num2 + 1, array2, num2, array.Length - num2 - 1);
					array = array2;
					propertyTab.Components = array;
				}
				flag = (array.Length == 0);
			}
			catch (Exception)
			{
				flag = true;
			}
			if (flag && this.viewTabScopes[num] > PropertyTabScope.Global)
			{
				this.RemoveTab(num, false);
			}
		}

		// Token: 0x06004DB6 RID: 19894 RVA: 0x0011E588 File Offset: 0x0011D588
		private void RemoveImage(int index)
		{
			this.imageList[0].Images.RemoveAt(index);
			if (this.imageList[1] != null)
			{
				this.imageList[1].Images.RemoveAt(index);
			}
		}

		// Token: 0x06004DB7 RID: 19895 RVA: 0x0011E5BC File Offset: 0x0011D5BC
		internal void RemoveTabs(PropertyTabScope classification, bool setupToolbar)
		{
			if (classification == PropertyTabScope.Static)
			{
				throw new ArgumentException(SR.GetString("PropertyGridRemoveStaticTabs"));
			}
			if (this.viewTabButtons == null || this.viewTabs == null || this.viewTabScopes == null)
			{
				return;
			}
			ToolStripButton button = (this.selectedViewTab >= 0 && this.selectedViewTab < this.viewTabButtons.Length) ? this.viewTabButtons[this.selectedViewTab] : null;
			for (int i = this.viewTabs.Length - 1; i >= 0; i--)
			{
				if (this.viewTabScopes[i] >= classification)
				{
					if (this.selectedViewTab == i)
					{
						this.selectedViewTab = -1;
					}
					else if (this.selectedViewTab > i)
					{
						this.selectedViewTab--;
					}
					PropertyTab[] destinationArray = new PropertyTab[this.viewTabs.Length - 1];
					Array.Copy(this.viewTabs, 0, destinationArray, 0, i);
					Array.Copy(this.viewTabs, i + 1, destinationArray, i, this.viewTabs.Length - i - 1);
					this.viewTabs = destinationArray;
					PropertyTabScope[] destinationArray2 = new PropertyTabScope[this.viewTabScopes.Length - 1];
					Array.Copy(this.viewTabScopes, 0, destinationArray2, 0, i);
					Array.Copy(this.viewTabScopes, i + 1, destinationArray2, i, this.viewTabScopes.Length - i - 1);
					this.viewTabScopes = destinationArray2;
					this.viewTabsDirty = true;
				}
			}
			if (setupToolbar && this.viewTabsDirty)
			{
				this.SetupToolbar();
				this.selectedViewTab = -1;
				this.SelectViewTabButtonDefault(button);
				for (int j = 0; j < this.viewTabs.Length; j++)
				{
					this.viewTabs[j].Components = new object[0];
				}
			}
		}

		// Token: 0x06004DB8 RID: 19896 RVA: 0x0011E748 File Offset: 0x0011D748
		internal void RemoveTab(int tabIndex, bool setupToolbar)
		{
			if (tabIndex >= this.viewTabs.Length || tabIndex < 0)
			{
				throw new ArgumentOutOfRangeException("tabIndex", SR.GetString("PropertyGridBadTabIndex"));
			}
			if (this.viewTabScopes[tabIndex] == PropertyTabScope.Static)
			{
				throw new ArgumentException(SR.GetString("PropertyGridRemoveStaticTabs"));
			}
			if (this.selectedViewTab == tabIndex)
			{
				this.selectedViewTab = 0;
			}
			if (!this.GetFlag(32) && this.ActiveDesigner != null)
			{
				int hashCode = this.ActiveDesigner.GetHashCode();
				if (this.designerSelections != null && this.designerSelections.ContainsKey(hashCode) && (int)this.designerSelections[hashCode] == tabIndex)
				{
					this.designerSelections.Remove(hashCode);
				}
			}
			ToolStripButton button = this.viewTabButtons[this.selectedViewTab];
			PropertyTab[] destinationArray = new PropertyTab[this.viewTabs.Length - 1];
			Array.Copy(this.viewTabs, 0, destinationArray, 0, tabIndex);
			Array.Copy(this.viewTabs, tabIndex + 1, destinationArray, tabIndex, this.viewTabs.Length - tabIndex - 1);
			this.viewTabs = destinationArray;
			PropertyTabScope[] destinationArray2 = new PropertyTabScope[this.viewTabScopes.Length - 1];
			Array.Copy(this.viewTabScopes, 0, destinationArray2, 0, tabIndex);
			Array.Copy(this.viewTabScopes, tabIndex + 1, destinationArray2, tabIndex, this.viewTabScopes.Length - tabIndex - 1);
			this.viewTabScopes = destinationArray2;
			this.viewTabsDirty = true;
			if (setupToolbar)
			{
				this.SetupToolbar();
				this.selectedViewTab = -1;
				this.SelectViewTabButtonDefault(button);
			}
		}

		// Token: 0x06004DB9 RID: 19897 RVA: 0x0011E8B8 File Offset: 0x0011D8B8
		internal void RemoveTab(Type tabType)
		{
			int num = -1;
			for (int i = 0; i < this.viewTabs.Length; i++)
			{
				if (tabType == this.viewTabs[i].GetType())
				{
					PropertyTab propertyTab = this.viewTabs[i];
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return;
			}
			PropertyTab[] destinationArray = new PropertyTab[this.viewTabs.Length - 1];
			Array.Copy(this.viewTabs, 0, destinationArray, 0, num);
			Array.Copy(this.viewTabs, num + 1, destinationArray, num, this.viewTabs.Length - num - 1);
			this.viewTabs = destinationArray;
			PropertyTabScope[] destinationArray2 = new PropertyTabScope[this.viewTabScopes.Length - 1];
			Array.Copy(this.viewTabScopes, 0, destinationArray2, 0, num);
			Array.Copy(this.viewTabScopes, num + 1, destinationArray2, num, this.viewTabScopes.Length - num - 1);
			this.viewTabScopes = destinationArray2;
			this.viewTabsDirty = true;
			this.SetupToolbar();
		}

		// Token: 0x06004DBA RID: 19898 RVA: 0x0011E98D File Offset: 0x0011D98D
		private void ResetCommandsBackColor()
		{
			this.hotcommands.ResetBackColor();
		}

		// Token: 0x06004DBB RID: 19899 RVA: 0x0011E99A File Offset: 0x0011D99A
		private void ResetCommandsForeColor()
		{
			this.hotcommands.ResetForeColor();
		}

		// Token: 0x06004DBC RID: 19900 RVA: 0x0011E9A7 File Offset: 0x0011D9A7
		private void ResetCommandsLinkColor()
		{
			this.hotcommands.Label.ResetLinkColor();
		}

		// Token: 0x06004DBD RID: 19901 RVA: 0x0011E9B9 File Offset: 0x0011D9B9
		private void ResetCommandsActiveLinkColor()
		{
			this.hotcommands.Label.ResetActiveLinkColor();
		}

		// Token: 0x06004DBE RID: 19902 RVA: 0x0011E9CB File Offset: 0x0011D9CB
		private void ResetCommandsDisabledLinkColor()
		{
			this.hotcommands.Label.ResetDisabledLinkColor();
		}

		// Token: 0x06004DBF RID: 19903 RVA: 0x0011E9DD File Offset: 0x0011D9DD
		private void ResetHelpBackColor()
		{
			this.doccomment.ResetBackColor();
		}

		// Token: 0x06004DC0 RID: 19904 RVA: 0x0011E9EA File Offset: 0x0011D9EA
		private void ResetHelpForeColor()
		{
			this.doccomment.ResetBackColor();
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x0011E9F8 File Offset: 0x0011D9F8
		internal void ReplaceSelectedObject(object oldObject, object newObject)
		{
			for (int i = 0; i < this.currentObjects.Length; i++)
			{
				if (this.currentObjects[i] == oldObject)
				{
					this.currentObjects[i] = newObject;
					this.Refresh(true);
					return;
				}
			}
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x0011EA34 File Offset: 0x0011DA34
		public void ResetSelectedProperty()
		{
			this.GetPropertyGridView().Reset();
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x0011EA44 File Offset: 0x0011DA44
		private void SaveTabSelection()
		{
			if (this.designerHost != null)
			{
				if (this.designerSelections == null)
				{
					this.designerSelections = new Hashtable();
				}
				this.designerSelections[this.designerHost.GetHashCode()] = this.selectedViewTab;
			}
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x0011EA94 File Offset: 0x0011DA94
		void IComPropertyBrowser.SaveState(RegistryKey optRoot)
		{
			if (optRoot == null)
			{
				return;
			}
			optRoot.SetValue("PbrsAlpha", (this.PropertySort == PropertySort.Alphabetical) ? "1" : "0");
			optRoot.SetValue("PbrsShowDesc", this.HelpVisible ? "1" : "0");
			optRoot.SetValue("PbrsShowCommands", this.CommandsVisibleIfAvailable ? "1" : "0");
			optRoot.SetValue("PbrsDescHeightRatio", this.dcSizeRatio.ToString(CultureInfo.InvariantCulture));
			optRoot.SetValue("PbrsHotCommandHeightRatio", this.hcSizeRatio.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x0011EB3C File Offset: 0x0011DB3C
		private void SetHotCommandColors(bool vscompat)
		{
			if (vscompat)
			{
				this.hotcommands.SetColors(SystemColors.Control, SystemColors.ControlText, SystemColors.ActiveCaption, SystemColors.ActiveCaption, SystemColors.ActiveCaption, SystemColors.ControlDark);
				return;
			}
			this.hotcommands.SetColors(SystemColors.Control, SystemColors.ControlText, Color.Empty, Color.Empty, Color.Empty, Color.Empty);
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x0011EB9F File Offset: 0x0011DB9F
		internal void SetStatusBox(string title, string desc)
		{
			this.doccomment.SetComment(title, desc);
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x0011EBAE File Offset: 0x0011DBAE
		private void SelectViewTabButton(ToolStripButton button, bool updateSelection)
		{
			this.SelectViewTabButtonDefault(button);
			if (updateSelection)
			{
				this.Refresh(false);
			}
		}

		// Token: 0x06004DC8 RID: 19912 RVA: 0x0011EBC4 File Offset: 0x0011DBC4
		private bool SelectViewTabButtonDefault(ToolStripButton button)
		{
			if (this.selectedViewTab >= 0 && this.selectedViewTab >= this.viewTabButtons.Length)
			{
				this.selectedViewTab = -1;
			}
			if (this.selectedViewTab >= 0 && this.selectedViewTab < this.viewTabButtons.Length && button == this.viewTabButtons[this.selectedViewTab])
			{
				this.viewTabButtons[this.selectedViewTab].Checked = true;
				return true;
			}
			PropertyTab oldTab = null;
			if (this.selectedViewTab != -1)
			{
				this.viewTabButtons[this.selectedViewTab].Checked = false;
				oldTab = this.viewTabs[this.selectedViewTab];
			}
			for (int i = 0; i < this.viewTabButtons.Length; i++)
			{
				if (this.viewTabButtons[i] == button)
				{
					this.selectedViewTab = i;
					this.viewTabButtons[i].Checked = true;
					try
					{
						this.SetFlag(8, true);
						this.OnPropertyTabChanged(new PropertyTabChangedEventArgs(oldTab, this.viewTabs[i]));
					}
					finally
					{
						this.SetFlag(8, false);
					}
					return true;
				}
			}
			this.selectedViewTab = 0;
			this.SelectViewTabButton(this.viewTabButtons[0], false);
			return false;
		}

		// Token: 0x06004DC9 RID: 19913 RVA: 0x0011ECE0 File Offset: 0x0011DCE0
		private void SetSelectState(int state)
		{
			if (state >= this.viewTabs.Length * this.viewSortButtons.Length)
			{
				state = 0;
			}
			else if (state < 0)
			{
				state = this.viewTabs.Length * this.viewSortButtons.Length - 1;
			}
			int num = this.viewSortButtons.Length;
			if (num > 0)
			{
				int num2 = state / num;
				int num3 = state % num;
				this.OnViewTabButtonClick(this.viewTabButtons[num2], EventArgs.Empty);
				this.OnViewSortButtonClick(this.viewSortButtons[num3], EventArgs.Empty);
			}
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x0011ED5C File Offset: 0x0011DD5C
		private void SetToolStripRenderer()
		{
			if (this.DrawFlatToolbar)
			{
				this.ToolStripRenderer = new ToolStripProfessionalRenderer(new ProfessionalColorTable
				{
					UseSystemColors = true
				});
				return;
			}
			this.ToolStripRenderer = new ToolStripSystemRenderer();
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x0011ED96 File Offset: 0x0011DD96
		private void SetupToolbar()
		{
			this.SetupToolbar(false);
		}

		// Token: 0x06004DCC RID: 19916 RVA: 0x0011EDA0 File Offset: 0x0011DDA0
		private void SetupToolbar(bool fullRebuild)
		{
			if (!this.viewTabsDirty && !fullRebuild)
			{
				return;
			}
			try
			{
				this.FreezePainting = true;
				if (this.imageList[0] == null || fullRebuild)
				{
					this.imageList[0] = new ImageList();
				}
				EventHandler eventHandler = new EventHandler(this.OnViewTabButtonClick);
				EventHandler eventHandler2 = new EventHandler(this.OnViewSortButtonClick);
				EventHandler eventHandler3 = new EventHandler(this.OnViewButtonClickPP);
				ArrayList arrayList;
				if (fullRebuild)
				{
					arrayList = new ArrayList();
				}
				else
				{
					arrayList = new ArrayList(this.toolStrip.Items);
				}
				if (this.viewSortButtons == null || fullRebuild)
				{
					this.viewSortButtons = new ToolStripButton[3];
					int imageIndex = -1;
					int imageIndex2 = -1;
					try
					{
						if (this.bmpAlpha == null)
						{
							this.bmpAlpha = new Bitmap(typeof(PropertyGrid), "PBAlpha.bmp");
						}
						imageIndex = this.AddImage(this.bmpAlpha);
					}
					catch (Exception)
					{
					}
					try
					{
						if (this.bmpCategory == null)
						{
							this.bmpCategory = new Bitmap(typeof(PropertyGrid), "PBCatego.bmp");
						}
						imageIndex2 = this.AddImage(this.bmpCategory);
					}
					catch (Exception)
					{
					}
					this.viewSortButtons[1] = this.CreatePushButton(SR.GetString("PBRSToolTipAlphabetic"), imageIndex, eventHandler2);
					this.viewSortButtons[0] = this.CreatePushButton(SR.GetString("PBRSToolTipCategorized"), imageIndex2, eventHandler2);
					this.viewSortButtons[2] = this.CreatePushButton("", 0, eventHandler2);
					this.viewSortButtons[2].Visible = false;
					for (int i = 0; i < this.viewSortButtons.Length; i++)
					{
						arrayList.Add(this.viewSortButtons[i]);
					}
				}
				else
				{
					int count = arrayList.Count;
					for (int i = count - 1; i >= 2; i--)
					{
						arrayList.RemoveAt(i);
					}
					count = this.imageList[0].Images.Count;
					for (int i = count - 1; i >= 2; i--)
					{
						this.RemoveImage(i);
					}
				}
				arrayList.Add(this.separator1);
				this.viewTabButtons = new ToolStripButton[this.viewTabs.Length];
				bool flag = this.viewTabs.Length > 1;
				for (int i = 0; i < this.viewTabs.Length; i++)
				{
					try
					{
						Bitmap bitmap = this.viewTabs[i].Bitmap;
						this.viewTabButtons[i] = this.CreatePushButton(this.viewTabs[i].TabName, this.AddImage(bitmap), eventHandler);
						if (flag)
						{
							arrayList.Add(this.viewTabButtons[i]);
						}
					}
					catch (Exception)
					{
					}
				}
				if (flag)
				{
					arrayList.Add(this.separator2);
				}
				int imageIndex3 = 0;
				try
				{
					if (this.bmpPropPage == null)
					{
						this.bmpPropPage = new Bitmap(typeof(PropertyGrid), "PBPPage.bmp");
					}
					imageIndex3 = this.AddImage(this.bmpPropPage);
				}
				catch (Exception)
				{
				}
				this.btnViewPropertyPages = this.CreatePushButton(SR.GetString("PBRSToolTipPropertyPages"), imageIndex3, eventHandler3);
				this.btnViewPropertyPages.Enabled = false;
				arrayList.Add(this.btnViewPropertyPages);
				if (this.imageList[1] != null)
				{
					this.imageList[1].Dispose();
					this.imageList[1] = null;
				}
				if (this.buttonType != 0)
				{
					this.EnsureLargeButtons();
				}
				this.toolStrip.ImageList = this.imageList[this.buttonType];
				this.toolStrip.SuspendLayout();
				this.toolStrip.Items.Clear();
				for (int j = 0; j < arrayList.Count; j++)
				{
					this.toolStrip.Items.Add(arrayList[j] as ToolStripItem);
				}
				this.toolStrip.ResumeLayout();
				if (this.viewTabsDirty)
				{
					this.OnLayoutInternal(false);
				}
				this.viewTabsDirty = false;
			}
			finally
			{
				this.FreezePainting = false;
			}
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x0011F1D0 File Offset: 0x0011E1D0
		protected void ShowEventsButton(bool value)
		{
			if (this.viewTabs != null && this.viewTabs.Length > 1 && this.viewTabs[1] is EventsTab)
			{
				this.viewTabButtons[1].Visible = value;
				if (!value && this.selectedViewTab == 1)
				{
					this.SelectViewTabButton(this.viewTabButtons[0], true);
				}
			}
			this.UpdatePropertiesViewTabVisibility();
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x0011F22E File Offset: 0x0011E22E
		private bool ShouldSerializeCommandsBackColor()
		{
			return this.hotcommands.ShouldSerializeBackColor();
		}

		// Token: 0x06004DCF RID: 19919 RVA: 0x0011F23B File Offset: 0x0011E23B
		private bool ShouldSerializeCommandsForeColor()
		{
			return this.hotcommands.ShouldSerializeForeColor();
		}

		// Token: 0x06004DD0 RID: 19920 RVA: 0x0011F248 File Offset: 0x0011E248
		private bool ShouldSerializeCommandsLinkColor()
		{
			return this.hotcommands.Label.ShouldSerializeLinkColor();
		}

		// Token: 0x06004DD1 RID: 19921 RVA: 0x0011F25A File Offset: 0x0011E25A
		private bool ShouldSerializeCommandsActiveLinkColor()
		{
			return this.hotcommands.Label.ShouldSerializeActiveLinkColor();
		}

		// Token: 0x06004DD2 RID: 19922 RVA: 0x0011F26C File Offset: 0x0011E26C
		private bool ShouldSerializeCommandsDisabledLinkColor()
		{
			return this.hotcommands.Label.ShouldSerializeDisabledLinkColor();
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x0011F280 File Offset: 0x0011E280
		private void SinkPropertyNotifyEvents()
		{
			int num = 0;
			while (this.connectionPointCookies != null && num < this.connectionPointCookies.Length)
			{
				if (this.connectionPointCookies[num] != null)
				{
					this.connectionPointCookies[num].Disconnect();
					this.connectionPointCookies[num] = null;
				}
				num++;
			}
			if (this.currentObjects == null || this.currentObjects.Length == 0)
			{
				this.connectionPointCookies = null;
				return;
			}
			if (this.connectionPointCookies == null || this.currentObjects.Length > this.connectionPointCookies.Length)
			{
				this.connectionPointCookies = new AxHost.ConnectionPointCookie[this.currentObjects.Length];
			}
			for (int i = 0; i < this.currentObjects.Length; i++)
			{
				try
				{
					object unwrappedObject = this.GetUnwrappedObject(i);
					if (Marshal.IsComObject(unwrappedObject))
					{
						this.connectionPointCookies[i] = new AxHost.ConnectionPointCookie(unwrappedObject, this, typeof(UnsafeNativeMethods.IPropertyNotifySink), false);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x06004DD4 RID: 19924 RVA: 0x0011F364 File Offset: 0x0011E364
		private bool ShouldForwardChildMouseMessage(Control child, MouseEventArgs me, ref Point pt)
		{
			Size size = child.Size;
			if (me.Y <= 1 || size.Height - me.Y <= 1)
			{
				NativeMethods.POINT point = new NativeMethods.POINT();
				point.x = me.X;
				point.y = me.Y;
				UnsafeNativeMethods.MapWindowPoints(new HandleRef(child, child.Handle), new HandleRef(this, base.Handle), point, 1);
				pt.X = point.x;
				pt.Y = point.y;
				return true;
			}
			return false;
		}

		// Token: 0x06004DD5 RID: 19925 RVA: 0x0011F3EC File Offset: 0x0011E3EC
		private void UpdatePropertiesViewTabVisibility()
		{
			if (this.viewTabButtons != null)
			{
				int num = 0;
				for (int i = 1; i < this.viewTabButtons.Length; i++)
				{
					if (this.viewTabButtons[i].Visible)
					{
						num++;
					}
				}
				if (num > 0)
				{
					this.viewTabButtons[0].Visible = true;
					this.separator2.Visible = true;
					return;
				}
				this.viewTabButtons[0].Visible = false;
				this.separator2.Visible = false;
			}
		}

		// Token: 0x06004DD6 RID: 19926 RVA: 0x0011F464 File Offset: 0x0011E464
		internal void UpdateSelection()
		{
			if (!this.GetFlag(1))
			{
				return;
			}
			if (this.viewTabs == null)
			{
				return;
			}
			string key = this.viewTabs[this.selectedViewTab].TabName + this.propertySortValue.ToString();
			if (this.viewTabProps != null && this.viewTabProps.ContainsKey(key))
			{
				this.peMain = (GridEntry)this.viewTabProps[key];
				if (this.peMain != null)
				{
					this.peMain.Refresh();
				}
			}
			else
			{
				if (this.currentObjects != null && this.currentObjects.Length > 0)
				{
					this.peMain = (GridEntry)GridEntry.Create(this.gridView, this.currentObjects, new PropertyGrid.PropertyGridServiceProvider(this), this.designerHost, this.SelectedTab, this.propertySortValue);
				}
				else
				{
					this.peMain = null;
				}
				if (this.peMain == null)
				{
					this.currentPropEntries = new GridEntryCollection(null, new GridEntry[0]);
					this.gridView.ClearProps();
					return;
				}
				if (this.BrowsableAttributes != null)
				{
					this.peMain.BrowsableAttributes = this.BrowsableAttributes;
				}
				if (this.viewTabProps == null)
				{
					this.viewTabProps = new Hashtable();
				}
				this.viewTabProps[key] = this.peMain;
			}
			this.currentPropEntries = this.peMain.Children;
			this.peDefault = this.peMain.DefaultChild;
			this.gridView.Invalidate();
		}

		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x06004DD7 RID: 19927 RVA: 0x0011F5D5 File Offset: 0x0011E5D5
		// (set) Token: 0x06004DD8 RID: 19928 RVA: 0x0011F5DD File Offset: 0x0011E5DD
		[SRDescription("UseCompatibleTextRenderingDescr")]
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		public bool UseCompatibleTextRendering
		{
			get
			{
				return base.UseCompatibleTextRenderingInt;
			}
			set
			{
				base.UseCompatibleTextRenderingInt = value;
				this.doccomment.UpdateTextRenderingEngine();
				this.gridView.Invalidate();
			}
		}

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x06004DD9 RID: 19929 RVA: 0x0011F5FC File Offset: 0x0011E5FC
		internal override bool SupportsUseCompatibleTextRendering
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004DDA RID: 19930 RVA: 0x0011F5FF File Offset: 0x0011E5FF
		internal bool WantsTab(bool forward)
		{
			if (forward)
			{
				return this.toolStrip.Visible && this.toolStrip.Focused;
			}
			return this.gridView.ContainsFocus && this.toolStrip.Visible;
		}

		// Token: 0x06004DDB RID: 19931 RVA: 0x0011F63C File Offset: 0x0011E63C
		private void GetDataFromCopyData(IntPtr lparam)
		{
			NativeMethods.COPYDATASTRUCT copydatastruct = (NativeMethods.COPYDATASTRUCT)UnsafeNativeMethods.PtrToStructure(lparam, typeof(NativeMethods.COPYDATASTRUCT));
			if (copydatastruct != null && copydatastruct.lpData != IntPtr.Zero)
			{
				this.propName = Marshal.PtrToStringAuto(copydatastruct.lpData);
				this.dwMsg = copydatastruct.dwData;
			}
		}

		// Token: 0x06004DDC RID: 19932 RVA: 0x0011F691 File Offset: 0x0011E691
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			this.SetupToolbar(true);
			if (!this.GetFlag(64))
			{
				this.SetupToolbar(true);
				this.SetFlag(64, true);
			}
			base.OnSystemColorsChanged(e);
		}

		// Token: 0x06004DDD RID: 19933 RVA: 0x0011F6BC File Offset: 0x0011E6BC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg != 74)
			{
				switch (msg)
				{
				case 768:
					if ((long)m.LParam == 0L)
					{
						this.gridView.DoCutCommand();
						return;
					}
					m.Result = (this.CanCut ? ((IntPtr)1) : ((IntPtr)0));
					return;
				case 769:
					if ((long)m.LParam == 0L)
					{
						this.gridView.DoCopyCommand();
						return;
					}
					m.Result = (this.CanCopy ? ((IntPtr)1) : ((IntPtr)0));
					return;
				case 770:
					if ((long)m.LParam == 0L)
					{
						this.gridView.DoPasteCommand();
						return;
					}
					m.Result = (this.CanPaste ? ((IntPtr)1) : ((IntPtr)0));
					return;
				case 771:
					break;
				case 772:
					if ((long)m.LParam == 0L)
					{
						this.gridView.DoUndoCommand();
						return;
					}
					m.Result = (this.CanUndo ? ((IntPtr)1) : ((IntPtr)0));
					return;
				default:
					switch (msg)
					{
					case 1104:
						if (this.toolStrip != null)
						{
							m.Result = (IntPtr)this.toolStrip.Items.Count;
							return;
						}
						break;
					case 1105:
						if (this.toolStrip != null)
						{
							int num = (int)((long)m.WParam);
							if (num >= 0 && num < this.toolStrip.Items.Count)
							{
								ToolStripButton toolStripButton = this.toolStrip.Items[num] as ToolStripButton;
								if (toolStripButton != null)
								{
									toolStripButton.Checked = !toolStripButton.Checked;
									if (toolStripButton == this.btnViewPropertyPages)
									{
										this.OnViewButtonClickPP(toolStripButton, EventArgs.Empty);
										return;
									}
									switch ((int)((long)m.WParam))
									{
									case 0:
									case 1:
										this.OnViewSortButtonClick(toolStripButton, EventArgs.Empty);
										return;
									default:
										this.SelectViewTabButton(toolStripButton, true);
										break;
									}
								}
							}
							return;
						}
						break;
					case 1106:
						if (this.toolStrip != null)
						{
							int num2 = (int)((long)m.WParam);
							if (num2 >= 0 && num2 < this.toolStrip.Items.Count)
							{
								ToolStripButton toolStripButton2 = this.toolStrip.Items[num2] as ToolStripButton;
								if (toolStripButton2 != null)
								{
									m.Result = (IntPtr)(toolStripButton2.Checked ? 1 : 0);
									return;
								}
								m.Result = IntPtr.Zero;
							}
							return;
						}
						break;
					case 1107:
					case 1108:
						if (this.toolStrip != null)
						{
							int num3 = (int)((long)m.WParam);
							if (num3 >= 0 && num3 < this.toolStrip.Items.Count)
							{
								string text;
								if (m.Msg == 1107)
								{
									text = this.toolStrip.Items[num3].Text;
								}
								else
								{
									text = this.toolStrip.Items[num3].ToolTipText;
								}
								m.Result = AutomationMessages.WriteAutomationText(text);
							}
							return;
						}
						break;
					case 1109:
						if (m.Msg == this.dwMsg)
						{
							m.Result = (IntPtr)this.gridView.GetPropertyLocation(this.propName, m.LParam == IntPtr.Zero, m.WParam == IntPtr.Zero);
							return;
						}
						break;
					case 1110:
					case 1111:
						m.Result = this.gridView.SendMessage(m.Msg, m.WParam, m.LParam);
						return;
					case 1112:
						if (m.LParam != IntPtr.Zero)
						{
							string b = AutomationMessages.ReadAutomationText(m.LParam);
							for (int i = 0; i < this.viewTabs.Length; i++)
							{
								if (this.viewTabs[i].GetType().FullName == b && this.viewTabButtons[i].Visible)
								{
									this.SelectViewTabButtonDefault(this.viewTabButtons[i]);
									m.Result = (IntPtr)1;
									break;
								}
							}
						}
						m.Result = (IntPtr)0;
						return;
					case 1113:
					{
						string testingInfo = this.gridView.GetTestingInfo((int)((long)m.WParam));
						m.Result = AutomationMessages.WriteAutomationText(testingInfo);
						return;
					}
					}
					break;
				}
				base.WndProc(ref m);
				return;
			}
			this.GetDataFromCopyData(m.LParam);
			m.Result = (IntPtr)1;
		}

		// Token: 0x0400323A RID: 12858
		private const int CYDIVIDER = 3;

		// Token: 0x0400323B RID: 12859
		private const int CXINDENT = 0;

		// Token: 0x0400323C RID: 12860
		private const int CYINDENT = 2;

		// Token: 0x0400323D RID: 12861
		private const int MIN_GRID_HEIGHT = 20;

		// Token: 0x0400323E RID: 12862
		private const int PROPERTIES = 0;

		// Token: 0x0400323F RID: 12863
		private const int EVENTS = 1;

		// Token: 0x04003240 RID: 12864
		private const int ALPHA = 1;

		// Token: 0x04003241 RID: 12865
		private const int CATEGORIES = 0;

		// Token: 0x04003242 RID: 12866
		private const int NO_SORT = 2;

		// Token: 0x04003243 RID: 12867
		private const int NORMAL_BUTTONS = 0;

		// Token: 0x04003244 RID: 12868
		private const int LARGE_BUTTONS = 1;

		// Token: 0x04003245 RID: 12869
		private const ushort PropertiesChanged = 1;

		// Token: 0x04003246 RID: 12870
		private const ushort GotDesignerEventService = 2;

		// Token: 0x04003247 RID: 12871
		private const ushort InternalChange = 4;

		// Token: 0x04003248 RID: 12872
		private const ushort TabsChanging = 8;

		// Token: 0x04003249 RID: 12873
		private const ushort BatchMode = 16;

		// Token: 0x0400324A RID: 12874
		private const ushort ReInitTab = 32;

		// Token: 0x0400324B RID: 12875
		private const ushort SysColorChangeRefresh = 64;

		// Token: 0x0400324C RID: 12876
		private const ushort FullRefreshAfterBatch = 128;

		// Token: 0x0400324D RID: 12877
		private const ushort BatchModeChange = 256;

		// Token: 0x0400324E RID: 12878
		private const ushort RefreshingProperties = 512;

		// Token: 0x0400324F RID: 12879
		private DocComment doccomment;

		// Token: 0x04003250 RID: 12880
		private int dcSizeRatio = -1;

		// Token: 0x04003251 RID: 12881
		private int hcSizeRatio = -1;

		// Token: 0x04003252 RID: 12882
		private HotCommands hotcommands;

		// Token: 0x04003253 RID: 12883
		private ToolStrip toolStrip;

		// Token: 0x04003254 RID: 12884
		private bool helpVisible = true;

		// Token: 0x04003255 RID: 12885
		private bool toolbarVisible = true;

		// Token: 0x04003256 RID: 12886
		private ImageList[] imageList = new ImageList[2];

		// Token: 0x04003257 RID: 12887
		private Bitmap bmpAlpha;

		// Token: 0x04003258 RID: 12888
		private Bitmap bmpCategory;

		// Token: 0x04003259 RID: 12889
		private Bitmap bmpPropPage;

		// Token: 0x0400325A RID: 12890
		private bool viewTabsDirty = true;

		// Token: 0x0400325B RID: 12891
		private bool drawFlatToolBar;

		// Token: 0x0400325C RID: 12892
		private PropertyTab[] viewTabs = new PropertyTab[0];

		// Token: 0x0400325D RID: 12893
		private PropertyTabScope[] viewTabScopes = new PropertyTabScope[0];

		// Token: 0x0400325E RID: 12894
		private Hashtable viewTabProps;

		// Token: 0x0400325F RID: 12895
		private ToolStripButton[] viewTabButtons;

		// Token: 0x04003260 RID: 12896
		private int selectedViewTab;

		// Token: 0x04003261 RID: 12897
		private ToolStripButton[] viewSortButtons;

		// Token: 0x04003262 RID: 12898
		private int selectedViewSort;

		// Token: 0x04003263 RID: 12899
		private PropertySort propertySortValue;

		// Token: 0x04003264 RID: 12900
		private ToolStripButton btnViewPropertyPages;

		// Token: 0x04003265 RID: 12901
		private ToolStripSeparator separator1;

		// Token: 0x04003266 RID: 12902
		private ToolStripSeparator separator2;

		// Token: 0x04003267 RID: 12903
		private int buttonType;

		// Token: 0x04003268 RID: 12904
		private PropertyGridView gridView;

		// Token: 0x04003269 RID: 12905
		private IDesignerHost designerHost;

		// Token: 0x0400326A RID: 12906
		private IDesignerEventService designerEventService;

		// Token: 0x0400326B RID: 12907
		private Hashtable designerSelections;

		// Token: 0x0400326C RID: 12908
		private GridEntry peDefault;

		// Token: 0x0400326D RID: 12909
		private GridEntry peMain;

		// Token: 0x0400326E RID: 12910
		private GridEntryCollection currentPropEntries;

		// Token: 0x0400326F RID: 12911
		private object[] currentObjects;

		// Token: 0x04003270 RID: 12912
		private int paintFrozen;

		// Token: 0x04003271 RID: 12913
		private Color lineColor = SystemColors.InactiveBorder;

		// Token: 0x04003272 RID: 12914
		private Color categoryForeColor = SystemColors.ControlText;

		// Token: 0x04003273 RID: 12915
		internal Brush lineBrush;

		// Token: 0x04003274 RID: 12916
		private AttributeCollection browsableAttributes;

		// Token: 0x04003275 RID: 12917
		private PropertyGrid.SnappableControl targetMove;

		// Token: 0x04003276 RID: 12918
		private int dividerMoveY = -1;

		// Token: 0x04003277 RID: 12919
		private ushort flags;

		// Token: 0x04003278 RID: 12920
		private readonly ComponentEventHandler onComponentAdd;

		// Token: 0x04003279 RID: 12921
		private readonly ComponentEventHandler onComponentRemove;

		// Token: 0x0400327A RID: 12922
		private readonly ComponentChangedEventHandler onComponentChanged;

		// Token: 0x0400327B RID: 12923
		private AxHost.ConnectionPointCookie[] connectionPointCookies;

		// Token: 0x0400327C RID: 12924
		private static object EventPropertyValueChanged = new object();

		// Token: 0x0400327D RID: 12925
		private static object EventComComponentNameChanged = new object();

		// Token: 0x0400327E RID: 12926
		private static object EventPropertyTabChanged = new object();

		// Token: 0x0400327F RID: 12927
		private static object EventSelectedGridItemChanged = new object();

		// Token: 0x04003280 RID: 12928
		private static object EventPropertySortChanged = new object();

		// Token: 0x04003281 RID: 12929
		private static object EventSelectedObjectsChanged = new object();

		// Token: 0x04003282 RID: 12930
		private string propName;

		// Token: 0x04003283 RID: 12931
		private int dwMsg;

		// Token: 0x020005C4 RID: 1476
		internal abstract class SnappableControl : Control
		{
			// Token: 0x06004DDF RID: 19935
			public abstract int GetOptimalHeight(int width);

			// Token: 0x06004DE0 RID: 19936
			public abstract int SnapHeightRequest(int request);

			// Token: 0x06004DE1 RID: 19937 RVA: 0x0011FB5B File Offset: 0x0011EB5B
			public SnappableControl(PropertyGrid ownerGrid)
			{
				this.ownerGrid = ownerGrid;
				base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			}

			// Token: 0x17000FD0 RID: 4048
			// (get) Token: 0x06004DE2 RID: 19938 RVA: 0x0011FB76 File Offset: 0x0011EB76
			// (set) Token: 0x06004DE3 RID: 19939 RVA: 0x0011FB7D File Offset: 0x0011EB7D
			public override Cursor Cursor
			{
				get
				{
					return Cursors.Default;
				}
				set
				{
					base.Cursor = value;
				}
			}

			// Token: 0x06004DE4 RID: 19940 RVA: 0x0011FB86 File Offset: 0x0011EB86
			protected override void OnControlAdded(ControlEventArgs ce)
			{
			}

			// Token: 0x06004DE5 RID: 19941 RVA: 0x0011FB88 File Offset: 0x0011EB88
			protected override void OnPaint(PaintEventArgs e)
			{
				base.OnPaint(e);
				Rectangle clientRectangle = base.ClientRectangle;
				clientRectangle.Width--;
				clientRectangle.Height--;
				e.Graphics.DrawRectangle(SystemPens.ControlDark, clientRectangle);
			}

			// Token: 0x04003284 RID: 12932
			protected PropertyGrid ownerGrid;

			// Token: 0x04003285 RID: 12933
			internal bool userSized;
		}

		// Token: 0x020005C5 RID: 1477
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public class PropertyTabCollection : ICollection, IEnumerable
		{
			// Token: 0x06004DE6 RID: 19942 RVA: 0x0011FBD2 File Offset: 0x0011EBD2
			internal PropertyTabCollection(PropertyGrid owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000FD1 RID: 4049
			// (get) Token: 0x06004DE7 RID: 19943 RVA: 0x0011FBE1 File Offset: 0x0011EBE1
			public int Count
			{
				get
				{
					if (this.owner == null)
					{
						return 0;
					}
					return this.owner.viewTabs.Length;
				}
			}

			// Token: 0x17000FD2 RID: 4050
			// (get) Token: 0x06004DE8 RID: 19944 RVA: 0x0011FBFA File Offset: 0x0011EBFA
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17000FD3 RID: 4051
			// (get) Token: 0x06004DE9 RID: 19945 RVA: 0x0011FBFD File Offset: 0x0011EBFD
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000FD4 RID: 4052
			public PropertyTab this[int index]
			{
				get
				{
					if (this.owner == null)
					{
						throw new InvalidOperationException(SR.GetString("PropertyGridPropertyTabCollectionReadOnly"));
					}
					return this.owner.viewTabs[index];
				}
			}

			// Token: 0x06004DEB RID: 19947 RVA: 0x0011FC27 File Offset: 0x0011EC27
			public void AddTabType(Type propertyTabType)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("PropertyGridPropertyTabCollectionReadOnly"));
				}
				this.owner.AddTab(propertyTabType, PropertyTabScope.Global);
			}

			// Token: 0x06004DEC RID: 19948 RVA: 0x0011FC4E File Offset: 0x0011EC4E
			public void AddTabType(Type propertyTabType, PropertyTabScope tabScope)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("PropertyGridPropertyTabCollectionReadOnly"));
				}
				this.owner.AddTab(propertyTabType, tabScope);
			}

			// Token: 0x06004DED RID: 19949 RVA: 0x0011FC75 File Offset: 0x0011EC75
			public void Clear(PropertyTabScope tabScope)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("PropertyGridPropertyTabCollectionReadOnly"));
				}
				this.owner.ClearTabs(tabScope);
			}

			// Token: 0x06004DEE RID: 19950 RVA: 0x0011FC9B File Offset: 0x0011EC9B
			void ICollection.CopyTo(Array dest, int index)
			{
				if (this.owner == null)
				{
					return;
				}
				if (this.owner.viewTabs.Length > 0)
				{
					Array.Copy(this.owner.viewTabs, 0, dest, index, this.owner.viewTabs.Length);
				}
			}

			// Token: 0x06004DEF RID: 19951 RVA: 0x0011FCD6 File Offset: 0x0011ECD6
			public IEnumerator GetEnumerator()
			{
				if (this.owner == null)
				{
					return new PropertyTab[0].GetEnumerator();
				}
				return this.owner.viewTabs.GetEnumerator();
			}

			// Token: 0x06004DF0 RID: 19952 RVA: 0x0011FCFC File Offset: 0x0011ECFC
			public void RemoveTabType(Type propertyTabType)
			{
				if (this.owner == null)
				{
					throw new InvalidOperationException(SR.GetString("PropertyGridPropertyTabCollectionReadOnly"));
				}
				this.owner.RemoveTab(propertyTabType);
			}

			// Token: 0x04003286 RID: 12934
			internal static PropertyGrid.PropertyTabCollection Empty = new PropertyGrid.PropertyTabCollection(null);

			// Token: 0x04003287 RID: 12935
			private PropertyGrid owner;
		}

		// Token: 0x020005C6 RID: 1478
		private interface IUnimplemented
		{
		}

		// Token: 0x020005C7 RID: 1479
		internal class SelectedObjectConverter : ReferenceConverter
		{
			// Token: 0x06004DF2 RID: 19954 RVA: 0x0011FD2F File Offset: 0x0011ED2F
			public SelectedObjectConverter() : base(typeof(IComponent))
			{
			}
		}

		// Token: 0x020005C8 RID: 1480
		private class PropertyGridServiceProvider : IServiceProvider
		{
			// Token: 0x06004DF3 RID: 19955 RVA: 0x0011FD41 File Offset: 0x0011ED41
			public PropertyGridServiceProvider(PropertyGrid owner)
			{
				this.owner = owner;
			}

			// Token: 0x06004DF4 RID: 19956 RVA: 0x0011FD50 File Offset: 0x0011ED50
			public object GetService(Type serviceType)
			{
				object obj = null;
				if (this.owner.ActiveDesigner != null)
				{
					obj = this.owner.ActiveDesigner.GetService(serviceType);
				}
				if (obj == null)
				{
					obj = this.owner.gridView.GetService(serviceType);
				}
				if (obj == null && this.owner.Site != null)
				{
					obj = this.owner.Site.GetService(serviceType);
				}
				return obj;
			}

			// Token: 0x04003288 RID: 12936
			private PropertyGrid owner;
		}

		// Token: 0x020005C9 RID: 1481
		internal static class MeasureTextHelper
		{
			// Token: 0x06004DF5 RID: 19957 RVA: 0x0011FDB6 File Offset: 0x0011EDB6
			public static SizeF MeasureText(PropertyGrid owner, Graphics g, string text, Font font)
			{
				return PropertyGrid.MeasureTextHelper.MeasureTextSimple(owner, g, text, font, new SizeF(0f, 0f));
			}

			// Token: 0x06004DF6 RID: 19958 RVA: 0x0011FDD0 File Offset: 0x0011EDD0
			public static SizeF MeasureText(PropertyGrid owner, Graphics g, string text, Font font, int width)
			{
				return PropertyGrid.MeasureTextHelper.MeasureText(owner, g, text, font, new SizeF((float)width, 999999f));
			}

			// Token: 0x06004DF7 RID: 19959 RVA: 0x0011FDE8 File Offset: 0x0011EDE8
			public static SizeF MeasureTextSimple(PropertyGrid owner, Graphics g, string text, Font font, SizeF size)
			{
				SizeF result;
				if (owner.UseCompatibleTextRendering)
				{
					result = g.MeasureString(text, font, size);
				}
				else
				{
					result = TextRenderer.MeasureText(g, text, font, Size.Ceiling(size), PropertyGrid.MeasureTextHelper.GetTextRendererFlags());
				}
				return result;
			}

			// Token: 0x06004DF8 RID: 19960 RVA: 0x0011FE28 File Offset: 0x0011EE28
			public static SizeF MeasureText(PropertyGrid owner, Graphics g, string text, Font font, SizeF size)
			{
				SizeF result;
				if (owner.UseCompatibleTextRendering)
				{
					result = g.MeasureString(text, font, size);
				}
				else
				{
					TextFormatFlags flags = PropertyGrid.MeasureTextHelper.GetTextRendererFlags() | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.WordBreak | TextFormatFlags.NoFullWidthCharacterBreak;
					result = TextRenderer.MeasureText(g, text, font, Size.Ceiling(size), flags);
				}
				return result;
			}

			// Token: 0x06004DF9 RID: 19961 RVA: 0x0011FE76 File Offset: 0x0011EE76
			public static TextFormatFlags GetTextRendererFlags()
			{
				return TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PreserveGraphicsTranslateTransform;
			}
		}
	}
}

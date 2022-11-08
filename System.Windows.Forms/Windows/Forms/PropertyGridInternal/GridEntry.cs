using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Design;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007A4 RID: 1956
	internal abstract class GridEntry : GridItem, ITypeDescriptorContext, IServiceProvider
	{
		// Token: 0x06006762 RID: 26466 RVA: 0x0017A9E4 File Offset: 0x001799E4
		protected GridEntry(PropertyGrid owner, GridEntry peParent)
		{
			this.parentPE = peParent;
			this.ownerGrid = owner;
			if (peParent != null)
			{
				this.propertyDepth = peParent.PropertyDepth + 1;
				this.PropertySort = peParent.PropertySort;
				if (peParent.ForceReadOnly)
				{
					this.flags |= 1024;
					return;
				}
			}
			else
			{
				this.propertyDepth = -1;
			}
		}

		// Token: 0x170015D1 RID: 5585
		// (get) Token: 0x06006763 RID: 26467 RVA: 0x0017AA65 File Offset: 0x00179A65
		public AccessibleObject AccessibilityObject
		{
			get
			{
				if (this.accessibleObject == null)
				{
					this.accessibleObject = new GridEntry.GridEntryAccessibleObject(this);
				}
				return this.accessibleObject;
			}
		}

		// Token: 0x170015D2 RID: 5586
		// (get) Token: 0x06006764 RID: 26468 RVA: 0x0017AA81 File Offset: 0x00179A81
		public virtual bool AllowMerge
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170015D3 RID: 5587
		// (get) Token: 0x06006765 RID: 26469 RVA: 0x0017AA84 File Offset: 0x00179A84
		internal virtual bool AlwaysAllowExpand
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170015D4 RID: 5588
		// (get) Token: 0x06006766 RID: 26470 RVA: 0x0017AA87 File Offset: 0x00179A87
		internal virtual AttributeCollection Attributes
		{
			get
			{
				return TypeDescriptor.GetAttributes(this.PropertyType);
			}
		}

		// Token: 0x06006767 RID: 26471 RVA: 0x0017AA94 File Offset: 0x00179A94
		protected virtual Brush GetBackgroundBrush(Graphics g)
		{
			return this.GridEntryHost.GetBackgroundBrush(g);
		}

		// Token: 0x170015D5 RID: 5589
		// (get) Token: 0x06006768 RID: 26472 RVA: 0x0017AAA2 File Offset: 0x00179AA2
		protected virtual Color LabelTextColor
		{
			get
			{
				if (this.ShouldRenderReadOnly)
				{
					return this.GridEntryHost.GrayTextColor;
				}
				return this.GridEntryHost.GetTextColor();
			}
		}

		// Token: 0x170015D6 RID: 5590
		// (get) Token: 0x06006769 RID: 26473 RVA: 0x0017AAC3 File Offset: 0x00179AC3
		// (set) Token: 0x0600676A RID: 26474 RVA: 0x0017AADA File Offset: 0x00179ADA
		public virtual AttributeCollection BrowsableAttributes
		{
			get
			{
				if (this.parentPE != null)
				{
					return this.parentPE.BrowsableAttributes;
				}
				return null;
			}
			set
			{
				this.parentPE.BrowsableAttributes = value;
			}
		}

		// Token: 0x170015D7 RID: 5591
		// (get) Token: 0x0600676B RID: 26475 RVA: 0x0017AAE8 File Offset: 0x00179AE8
		public virtual IComponent Component
		{
			get
			{
				object valueOwner = this.GetValueOwner();
				if (valueOwner is IComponent)
				{
					return (IComponent)valueOwner;
				}
				if (this.parentPE != null)
				{
					return this.parentPE.Component;
				}
				return null;
			}
		}

		// Token: 0x170015D8 RID: 5592
		// (get) Token: 0x0600676C RID: 26476 RVA: 0x0017AB20 File Offset: 0x00179B20
		protected virtual IComponentChangeService ComponentChangeService
		{
			get
			{
				return this.parentPE.ComponentChangeService;
			}
		}

		// Token: 0x170015D9 RID: 5593
		// (get) Token: 0x0600676D RID: 26477 RVA: 0x0017AB30 File Offset: 0x00179B30
		public virtual IContainer Container
		{
			get
			{
				IComponent component = this.Component;
				if (component != null)
				{
					ISite site = component.Site;
					if (site != null)
					{
						return site.Container;
					}
				}
				return null;
			}
		}

		// Token: 0x170015DA RID: 5594
		// (get) Token: 0x0600676E RID: 26478 RVA: 0x0017AB59 File Offset: 0x00179B59
		// (set) Token: 0x0600676F RID: 26479 RVA: 0x0017AB76 File Offset: 0x00179B76
		protected GridEntryCollection ChildCollection
		{
			get
			{
				if (this.childCollection == null)
				{
					this.childCollection = new GridEntryCollection(this, null);
				}
				return this.childCollection;
			}
			set
			{
				if (this.childCollection != value)
				{
					if (this.childCollection != null)
					{
						this.childCollection.Dispose();
						this.childCollection = null;
					}
					this.childCollection = value;
				}
			}
		}

		// Token: 0x170015DB RID: 5595
		// (get) Token: 0x06006770 RID: 26480 RVA: 0x0017ABA2 File Offset: 0x00179BA2
		public int ChildCount
		{
			get
			{
				if (this.Children != null)
				{
					return this.Children.Count;
				}
				return 0;
			}
		}

		// Token: 0x170015DC RID: 5596
		// (get) Token: 0x06006771 RID: 26481 RVA: 0x0017ABB9 File Offset: 0x00179BB9
		public virtual GridEntryCollection Children
		{
			get
			{
				if (this.childCollection == null && !this.Disposed)
				{
					this.CreateChildren();
				}
				return this.childCollection;
			}
		}

		// Token: 0x170015DD RID: 5597
		// (get) Token: 0x06006772 RID: 26482 RVA: 0x0017ABD8 File Offset: 0x00179BD8
		// (set) Token: 0x06006773 RID: 26483 RVA: 0x0017ABEF File Offset: 0x00179BEF
		public virtual PropertyTab CurrentTab
		{
			get
			{
				if (this.parentPE != null)
				{
					return this.parentPE.CurrentTab;
				}
				return null;
			}
			set
			{
				if (this.parentPE != null)
				{
					this.parentPE.CurrentTab = value;
				}
			}
		}

		// Token: 0x170015DE RID: 5598
		// (get) Token: 0x06006774 RID: 26484 RVA: 0x0017AC05 File Offset: 0x00179C05
		// (set) Token: 0x06006775 RID: 26485 RVA: 0x0017AC08 File Offset: 0x00179C08
		internal virtual GridEntry DefaultChild
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x170015DF RID: 5599
		// (get) Token: 0x06006776 RID: 26486 RVA: 0x0017AC0A File Offset: 0x00179C0A
		// (set) Token: 0x06006777 RID: 26487 RVA: 0x0017AC21 File Offset: 0x00179C21
		internal virtual IDesignerHost DesignerHost
		{
			get
			{
				if (this.parentPE != null)
				{
					return this.parentPE.DesignerHost;
				}
				return null;
			}
			set
			{
				if (this.parentPE != null)
				{
					this.parentPE.DesignerHost = value;
				}
			}
		}

		// Token: 0x170015E0 RID: 5600
		// (get) Token: 0x06006778 RID: 26488 RVA: 0x0017AC37 File Offset: 0x00179C37
		internal bool Disposed
		{
			get
			{
				return this.GetFlagSet(8192);
			}
		}

		// Token: 0x170015E1 RID: 5601
		// (get) Token: 0x06006779 RID: 26489 RVA: 0x0017AC44 File Offset: 0x00179C44
		internal virtual bool Enumerable
		{
			get
			{
				return (this.Flags & 2) != 0;
			}
		}

		// Token: 0x170015E2 RID: 5602
		// (get) Token: 0x0600677A RID: 26490 RVA: 0x0017AC54 File Offset: 0x00179C54
		public override bool Expandable
		{
			get
			{
				bool flag = this.GetFlagSet(131072);
				if (flag && this.childCollection != null && this.childCollection.Count > 0)
				{
					return true;
				}
				if (this.GetFlagSet(524288))
				{
					return false;
				}
				if (flag && (this.cacheItems == null || this.cacheItems.lastValue == null) && this.PropertyValue == null)
				{
					flag = false;
				}
				return flag;
			}
		}

		// Token: 0x170015E3 RID: 5603
		// (get) Token: 0x0600677B RID: 26491 RVA: 0x0017ACBA File Offset: 0x00179CBA
		// (set) Token: 0x0600677C RID: 26492 RVA: 0x0017ACC2 File Offset: 0x00179CC2
		public override bool Expanded
		{
			get
			{
				return this.InternalExpanded;
			}
			set
			{
				this.GridEntryHost.SetExpand(this, value);
			}
		}

		// Token: 0x170015E4 RID: 5604
		// (get) Token: 0x0600677D RID: 26493 RVA: 0x0017ACD1 File Offset: 0x00179CD1
		internal virtual bool ForceReadOnly
		{
			get
			{
				return (this.flags & 1024) != 0;
			}
		}

		// Token: 0x170015E5 RID: 5605
		// (get) Token: 0x0600677E RID: 26494 RVA: 0x0017ACE5 File Offset: 0x00179CE5
		// (set) Token: 0x0600677F RID: 26495 RVA: 0x0017AD0C File Offset: 0x00179D0C
		internal virtual bool InternalExpanded
		{
			get
			{
				return this.childCollection != null && this.childCollection.Count != 0 && this.GetFlagSet(65536);
			}
			set
			{
				if (!this.Expandable || value == this.InternalExpanded)
				{
					return;
				}
				if (this.childCollection != null && this.childCollection.Count > 0)
				{
					this.SetFlag(65536, value);
					return;
				}
				this.SetFlag(65536, false);
				if (value)
				{
					bool fVal = this.CreateChildren();
					this.SetFlag(65536, fVal);
				}
			}
		}

		// Token: 0x170015E6 RID: 5606
		// (get) Token: 0x06006780 RID: 26496 RVA: 0x0017AD70 File Offset: 0x00179D70
		// (set) Token: 0x06006781 RID: 26497 RVA: 0x0017AF54 File Offset: 0x00179F54
		internal virtual int Flags
		{
			get
			{
				if ((this.flags & -2147483648) != 0)
				{
					return this.flags;
				}
				this.flags |= int.MinValue;
				TypeConverter typeConverter = this.TypeConverter;
				UITypeEditor uitypeEditor = this.UITypeEditor;
				object instance = this.Instance;
				bool flag = this.ForceReadOnly;
				if (instance != null)
				{
					flag |= TypeDescriptor.GetAttributes(instance).Contains(InheritanceAttribute.InheritedReadOnly);
				}
				if (typeConverter.GetStandardValuesSupported(this))
				{
					this.flags |= 2;
				}
				if (!flag && typeConverter.CanConvertFrom(this, typeof(string)) && !typeConverter.GetStandardValuesExclusive(this))
				{
					this.flags |= 1;
				}
				bool flag2 = TypeDescriptor.GetAttributes(this.PropertyType)[typeof(ImmutableObjectAttribute)].Equals(ImmutableObjectAttribute.Yes);
				bool flag3 = flag2 || typeConverter.GetCreateInstanceSupported(this);
				if (flag3)
				{
					this.flags |= 512;
				}
				if (typeConverter.GetPropertiesSupported(this))
				{
					this.flags |= 131072;
					if (!flag && (this.flags & 1) == 0 && !flag2)
					{
						this.flags |= 128;
					}
				}
				if (this.Attributes.Contains(PasswordPropertyTextAttribute.Yes))
				{
					this.flags |= 4096;
				}
				if (uitypeEditor != null)
				{
					if (uitypeEditor.GetPaintValueSupported(this))
					{
						this.flags |= 4;
					}
					bool flag4 = !flag;
					if (flag4)
					{
						switch (uitypeEditor.GetEditStyle(this))
						{
						case UITypeEditorEditStyle.Modal:
							this.flags |= 16;
							if (!flag3 && !this.PropertyType.IsValueType)
							{
								this.flags |= 128;
							}
							break;
						case UITypeEditorEditStyle.DropDown:
							this.flags |= 32;
							break;
						}
					}
				}
				return this.flags;
			}
			set
			{
				this.flags = value;
			}
		}

		// Token: 0x170015E7 RID: 5607
		// (get) Token: 0x06006782 RID: 26498 RVA: 0x0017AF5D File Offset: 0x00179F5D
		// (set) Token: 0x06006783 RID: 26499 RVA: 0x0017AF68 File Offset: 0x00179F68
		public bool Focus
		{
			get
			{
				return this.hasFocus;
			}
			set
			{
				if (this.Disposed)
				{
					return;
				}
				if (this.cacheItems != null)
				{
					this.cacheItems.lastValueString = null;
					this.cacheItems.useValueString = false;
					this.cacheItems.useShouldSerialize = false;
				}
				if (this.hasFocus != value)
				{
					this.hasFocus = value;
					if (value)
					{
						int num = this.GridEntryHost.AccessibilityGetGridEntryChildID(this);
						if (num >= 0)
						{
							PropertyGridView.PropertyGridViewAccessibleObject propertyGridViewAccessibleObject = (PropertyGridView.PropertyGridViewAccessibleObject)this.GridEntryHost.AccessibilityObject;
							propertyGridViewAccessibleObject.NotifyClients(AccessibleEvents.Focus, num);
							propertyGridViewAccessibleObject.NotifyClients(AccessibleEvents.Selection, num);
						}
					}
				}
			}
		}

		// Token: 0x170015E8 RID: 5608
		// (get) Token: 0x06006784 RID: 26500 RVA: 0x0017AFF8 File Offset: 0x00179FF8
		public string FullLabel
		{
			get
			{
				string text = null;
				if (this.parentPE != null)
				{
					text = this.parentPE.FullLabel;
				}
				if (text != null)
				{
					text += ".";
				}
				else
				{
					text = "";
				}
				return text + this.PropertyLabel;
			}
		}

		// Token: 0x170015E9 RID: 5609
		// (get) Token: 0x06006785 RID: 26501 RVA: 0x0017B040 File Offset: 0x0017A040
		public override GridItemCollection GridItems
		{
			get
			{
				if (this.Disposed)
				{
					throw new ObjectDisposedException(SR.GetString("GridItemDisposed"));
				}
				if (this.IsExpandable && this.childCollection != null && this.childCollection.Count == 0)
				{
					this.CreateChildren();
				}
				return this.Children;
			}
		}

		// Token: 0x170015EA RID: 5610
		// (get) Token: 0x06006786 RID: 26502 RVA: 0x0017B08F File Offset: 0x0017A08F
		// (set) Token: 0x06006787 RID: 26503 RVA: 0x0017B0A6 File Offset: 0x0017A0A6
		internal virtual PropertyGridView GridEntryHost
		{
			get
			{
				if (this.parentPE != null)
				{
					return this.parentPE.GridEntryHost;
				}
				return null;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170015EB RID: 5611
		// (get) Token: 0x06006788 RID: 26504 RVA: 0x0017B0AD File Offset: 0x0017A0AD
		public override GridItemType GridItemType
		{
			get
			{
				return GridItemType.Property;
			}
		}

		// Token: 0x170015EC RID: 5612
		// (get) Token: 0x06006789 RID: 26505 RVA: 0x0017B0B0 File Offset: 0x0017A0B0
		internal virtual bool HasValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170015ED RID: 5613
		// (get) Token: 0x0600678A RID: 26506 RVA: 0x0017B0B4 File Offset: 0x0017A0B4
		public virtual string HelpKeyword
		{
			get
			{
				string text = null;
				if (this.parentPE != null)
				{
					text = this.parentPE.HelpKeyword;
				}
				if (text == null)
				{
					text = string.Empty;
				}
				return text;
			}
		}

		// Token: 0x170015EE RID: 5614
		// (get) Token: 0x0600678B RID: 26507 RVA: 0x0017B0E1 File Offset: 0x0017A0E1
		internal virtual string HelpKeywordInternal
		{
			get
			{
				return this.HelpKeyword;
			}
		}

		// Token: 0x170015EF RID: 5615
		// (get) Token: 0x0600678C RID: 26508 RVA: 0x0017B0EC File Offset: 0x0017A0EC
		public virtual bool IsCustomPaint
		{
			get
			{
				if ((this.flags & -2147483648) == 0)
				{
					UITypeEditor uitypeEditor = this.UITypeEditor;
					if (uitypeEditor != null)
					{
						if ((this.flags & 4) != 0 || (this.flags & 1048576) != 0)
						{
							return (this.flags & 4) != 0;
						}
						if (uitypeEditor.GetPaintValueSupported(this))
						{
							this.flags |= 4;
							return true;
						}
						this.flags |= 1048576;
						return false;
					}
				}
				return (this.Flags & 4) != 0;
			}
		}

		// Token: 0x170015F0 RID: 5616
		// (get) Token: 0x0600678D RID: 26509 RVA: 0x0017B173 File Offset: 0x0017A173
		// (set) Token: 0x0600678E RID: 26510 RVA: 0x0017B17B File Offset: 0x0017A17B
		public virtual bool IsExpandable
		{
			get
			{
				return this.Expandable;
			}
			set
			{
				if (value != this.GetFlagSet(131072))
				{
					this.SetFlag(524288, false);
					this.SetFlag(131072, value);
				}
			}
		}

		// Token: 0x170015F1 RID: 5617
		// (get) Token: 0x0600678F RID: 26511 RVA: 0x0017B1A3 File Offset: 0x0017A1A3
		public virtual bool IsTextEditable
		{
			get
			{
				return this.IsValueEditable && (this.Flags & 1) != 0;
			}
		}

		// Token: 0x170015F2 RID: 5618
		// (get) Token: 0x06006790 RID: 26512 RVA: 0x0017B1BD File Offset: 0x0017A1BD
		public virtual bool IsValueEditable
		{
			get
			{
				return !this.ForceReadOnly && 0 != (this.Flags & 51);
			}
		}

		// Token: 0x170015F3 RID: 5619
		// (get) Token: 0x06006791 RID: 26513 RVA: 0x0017B1D8 File Offset: 0x0017A1D8
		public virtual object Instance
		{
			get
			{
				object valueOwner = this.GetValueOwner();
				if (this.parentPE != null && valueOwner == null)
				{
					return this.parentPE.Instance;
				}
				return valueOwner;
			}
		}

		// Token: 0x170015F4 RID: 5620
		// (get) Token: 0x06006792 RID: 26514 RVA: 0x0017B204 File Offset: 0x0017A204
		public override string Label
		{
			get
			{
				return this.PropertyLabel;
			}
		}

		// Token: 0x170015F5 RID: 5621
		// (get) Token: 0x06006793 RID: 26515 RVA: 0x0017B20C File Offset: 0x0017A20C
		public override PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170015F6 RID: 5622
		// (get) Token: 0x06006794 RID: 26516 RVA: 0x0017B210 File Offset: 0x0017A210
		internal virtual int PropertyLabelIndent
		{
			get
			{
				int num = this.GridEntryHost.GetOutlineIconSize() + 5;
				return (this.propertyDepth + 1) * num + 1;
			}
		}

		// Token: 0x06006795 RID: 26517 RVA: 0x0017B237 File Offset: 0x0017A237
		internal virtual Point GetLabelToolTipLocation(int mouseX, int mouseY)
		{
			return this.labelTipPoint;
		}

		// Token: 0x170015F7 RID: 5623
		// (get) Token: 0x06006796 RID: 26518 RVA: 0x0017B23F File Offset: 0x0017A23F
		internal virtual string LabelToolTipText
		{
			get
			{
				return this.PropertyLabel;
			}
		}

		// Token: 0x170015F8 RID: 5624
		// (get) Token: 0x06006797 RID: 26519 RVA: 0x0017B247 File Offset: 0x0017A247
		public virtual bool NeedsDropDownButton
		{
			get
			{
				return (this.Flags & 32) != 0;
			}
		}

		// Token: 0x170015F9 RID: 5625
		// (get) Token: 0x06006798 RID: 26520 RVA: 0x0017B258 File Offset: 0x0017A258
		public virtual bool NeedsCustomEditorButton
		{
			get
			{
				return (this.Flags & 16) != 0 && (this.IsValueEditable || (this.Flags & 128) != 0);
			}
		}

		// Token: 0x170015FA RID: 5626
		// (get) Token: 0x06006799 RID: 26521 RVA: 0x0017B283 File Offset: 0x0017A283
		public PropertyGrid OwnerGrid
		{
			get
			{
				return this.ownerGrid;
			}
		}

		// Token: 0x170015FB RID: 5627
		// (get) Token: 0x0600679A RID: 26522 RVA: 0x0017B28C File Offset: 0x0017A28C
		public Rectangle OutlineRect
		{
			get
			{
				if (!this.outlineRect.IsEmpty)
				{
					return this.outlineRect;
				}
				PropertyGridView gridEntryHost = this.GridEntryHost;
				int outlineIconSize = gridEntryHost.GetOutlineIconSize();
				int num = outlineIconSize + 5;
				int x = this.propertyDepth * num + 2;
				int y = (gridEntryHost.GetGridEntryHeight() - outlineIconSize) / 2;
				this.outlineRect = new Rectangle(x, y, outlineIconSize, outlineIconSize);
				return this.outlineRect;
			}
		}

		// Token: 0x170015FC RID: 5628
		// (get) Token: 0x0600679B RID: 26523 RVA: 0x0017B2EC File Offset: 0x0017A2EC
		// (set) Token: 0x0600679C RID: 26524 RVA: 0x0017B2F4 File Offset: 0x0017A2F4
		public virtual GridEntry ParentGridEntry
		{
			get
			{
				return this.parentPE;
			}
			set
			{
				this.parentPE = value;
				if (value != null)
				{
					this.propertyDepth = value.PropertyDepth + 1;
					if (this.childCollection != null)
					{
						for (int i = 0; i < this.childCollection.Count; i++)
						{
							this.childCollection.GetEntry(i).ParentGridEntry = this;
						}
					}
				}
			}
		}

		// Token: 0x170015FD RID: 5629
		// (get) Token: 0x0600679D RID: 26525 RVA: 0x0017B34C File Offset: 0x0017A34C
		public override GridItem Parent
		{
			get
			{
				if (this.Disposed)
				{
					throw new ObjectDisposedException(SR.GetString("GridItemDisposed"));
				}
				return this.ParentGridEntry;
			}
		}

		// Token: 0x170015FE RID: 5630
		// (get) Token: 0x0600679E RID: 26526 RVA: 0x0017B379 File Offset: 0x0017A379
		public virtual string PropertyCategory
		{
			get
			{
				return CategoryAttribute.Default.Category;
			}
		}

		// Token: 0x170015FF RID: 5631
		// (get) Token: 0x0600679F RID: 26527 RVA: 0x0017B385 File Offset: 0x0017A385
		public virtual int PropertyDepth
		{
			get
			{
				return this.propertyDepth;
			}
		}

		// Token: 0x17001600 RID: 5632
		// (get) Token: 0x060067A0 RID: 26528 RVA: 0x0017B38D File Offset: 0x0017A38D
		public virtual string PropertyDescription
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001601 RID: 5633
		// (get) Token: 0x060067A1 RID: 26529 RVA: 0x0017B390 File Offset: 0x0017A390
		public virtual string PropertyLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001602 RID: 5634
		// (get) Token: 0x060067A2 RID: 26530 RVA: 0x0017B393 File Offset: 0x0017A393
		public virtual string PropertyName
		{
			get
			{
				return this.PropertyLabel;
			}
		}

		// Token: 0x17001603 RID: 5635
		// (get) Token: 0x060067A3 RID: 26531 RVA: 0x0017B39C File Offset: 0x0017A39C
		public virtual Type PropertyType
		{
			get
			{
				object propertyValue = this.PropertyValue;
				if (propertyValue != null)
				{
					return propertyValue.GetType();
				}
				return null;
			}
		}

		// Token: 0x17001604 RID: 5636
		// (get) Token: 0x060067A4 RID: 26532 RVA: 0x0017B3BB File Offset: 0x0017A3BB
		// (set) Token: 0x060067A5 RID: 26533 RVA: 0x0017B3D2 File Offset: 0x0017A3D2
		public virtual object PropertyValue
		{
			get
			{
				if (this.cacheItems != null)
				{
					return this.cacheItems.lastValue;
				}
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17001605 RID: 5637
		// (get) Token: 0x060067A6 RID: 26534 RVA: 0x0017B3D4 File Offset: 0x0017A3D4
		public virtual bool ShouldRenderPassword
		{
			get
			{
				return (this.Flags & 4096) != 0;
			}
		}

		// Token: 0x17001606 RID: 5638
		// (get) Token: 0x060067A7 RID: 26535 RVA: 0x0017B3E8 File Offset: 0x0017A3E8
		public virtual bool ShouldRenderReadOnly
		{
			get
			{
				return this.ForceReadOnly || (this.Flags & 256) != 0 || (!this.IsValueEditable && 0 == (this.Flags & 128));
			}
		}

		// Token: 0x17001607 RID: 5639
		// (get) Token: 0x060067A8 RID: 26536 RVA: 0x0017B420 File Offset: 0x0017A420
		internal virtual TypeConverter TypeConverter
		{
			get
			{
				if (this.converter == null)
				{
					object propertyValue = this.PropertyValue;
					if (propertyValue == null)
					{
						this.converter = TypeDescriptor.GetConverter(this.PropertyType);
					}
					else
					{
						this.converter = TypeDescriptor.GetConverter(propertyValue);
					}
				}
				return this.converter;
			}
		}

		// Token: 0x17001608 RID: 5640
		// (get) Token: 0x060067A9 RID: 26537 RVA: 0x0017B464 File Offset: 0x0017A464
		internal virtual UITypeEditor UITypeEditor
		{
			get
			{
				if (this.editor == null && this.PropertyType != null)
				{
					this.editor = (UITypeEditor)TypeDescriptor.GetEditor(this.PropertyType, typeof(UITypeEditor));
				}
				return this.editor;
			}
		}

		// Token: 0x17001609 RID: 5641
		// (get) Token: 0x060067AA RID: 26538 RVA: 0x0017B49C File Offset: 0x0017A49C
		public override object Value
		{
			get
			{
				return this.PropertyValue;
			}
		}

		// Token: 0x1700160A RID: 5642
		// (get) Token: 0x060067AB RID: 26539 RVA: 0x0017B4A4 File Offset: 0x0017A4A4
		// (set) Token: 0x060067AC RID: 26540 RVA: 0x0017B4BA File Offset: 0x0017A4BA
		internal Point ValueToolTipLocation
		{
			get
			{
				if (!this.ShouldRenderPassword)
				{
					return this.valueTipPoint;
				}
				return GridEntry.InvalidPoint;
			}
			set
			{
				this.valueTipPoint = value;
			}
		}

		// Token: 0x1700160B RID: 5643
		// (get) Token: 0x060067AD RID: 26541 RVA: 0x0017B4C4 File Offset: 0x0017A4C4
		internal int VisibleChildCount
		{
			get
			{
				if (!this.Expanded)
				{
					return 0;
				}
				int childCount = this.ChildCount;
				int num = childCount;
				for (int i = 0; i < childCount; i++)
				{
					num += this.ChildCollection.GetEntry(i).VisibleChildCount;
				}
				return num;
			}
		}

		// Token: 0x060067AE RID: 26542 RVA: 0x0017B505 File Offset: 0x0017A505
		public virtual void AddOnLabelClick(EventHandler h)
		{
			this.AddEventHandler(GridEntry.EVENT_LABEL_CLICK, h);
		}

		// Token: 0x060067AF RID: 26543 RVA: 0x0017B513 File Offset: 0x0017A513
		public virtual void AddOnLabelDoubleClick(EventHandler h)
		{
			this.AddEventHandler(GridEntry.EVENT_LABEL_DBLCLICK, h);
		}

		// Token: 0x060067B0 RID: 26544 RVA: 0x0017B521 File Offset: 0x0017A521
		public virtual void AddOnValueClick(EventHandler h)
		{
			this.AddEventHandler(GridEntry.EVENT_VALUE_CLICK, h);
		}

		// Token: 0x060067B1 RID: 26545 RVA: 0x0017B52F File Offset: 0x0017A52F
		public virtual void AddOnValueDoubleClick(EventHandler h)
		{
			this.AddEventHandler(GridEntry.EVENT_VALUE_DBLCLICK, h);
		}

		// Token: 0x060067B2 RID: 26546 RVA: 0x0017B53D File Offset: 0x0017A53D
		public virtual void AddOnOutlineClick(EventHandler h)
		{
			this.AddEventHandler(GridEntry.EVENT_OUTLINE_CLICK, h);
		}

		// Token: 0x060067B3 RID: 26547 RVA: 0x0017B54B File Offset: 0x0017A54B
		public virtual void AddOnOutlineDoubleClick(EventHandler h)
		{
			this.AddEventHandler(GridEntry.EVENT_OUTLINE_DBLCLICK, h);
		}

		// Token: 0x060067B4 RID: 26548 RVA: 0x0017B559 File Offset: 0x0017A559
		public virtual void AddOnRecreateChildren(GridEntryRecreateChildrenEventHandler h)
		{
			this.AddEventHandler(GridEntry.EVENT_RECREATE_CHILDREN, h);
		}

		// Token: 0x060067B5 RID: 26549 RVA: 0x0017B567 File Offset: 0x0017A567
		internal void ClearCachedValues()
		{
			this.ClearCachedValues(true);
		}

		// Token: 0x060067B6 RID: 26550 RVA: 0x0017B570 File Offset: 0x0017A570
		internal void ClearCachedValues(bool clearChildren)
		{
			if (this.cacheItems != null)
			{
				this.cacheItems.useValueString = false;
				this.cacheItems.lastValue = null;
				this.cacheItems.useShouldSerialize = false;
			}
			if (clearChildren)
			{
				for (int i = 0; i < this.ChildCollection.Count; i++)
				{
					this.ChildCollection.GetEntry(i).ClearCachedValues();
				}
			}
		}

		// Token: 0x060067B7 RID: 26551 RVA: 0x0017B5D3 File Offset: 0x0017A5D3
		public object ConvertTextToValue(string text)
		{
			if (this.TypeConverter.CanConvertFrom(this, typeof(string)))
			{
				return this.TypeConverter.ConvertFromString(this, text);
			}
			return text;
		}

		// Token: 0x060067B8 RID: 26552 RVA: 0x0017B5FC File Offset: 0x0017A5FC
		internal static IRootGridEntry Create(PropertyGridView view, object[] rgobjs, IServiceProvider baseProvider, IDesignerHost currentHost, PropertyTab tab, PropertySort initialSortType)
		{
			IRootGridEntry result = null;
			if (rgobjs == null || rgobjs.Length == 0)
			{
				return null;
			}
			try
			{
				if (rgobjs.Length == 1)
				{
					result = new SingleSelectRootGridEntry(view, rgobjs[0], baseProvider, currentHost, tab, initialSortType);
				}
				else
				{
					result = new MultiSelectRootGridEntry(view, rgobjs, baseProvider, currentHost, tab, initialSortType);
				}
			}
			catch (Exception)
			{
				throw;
			}
			return result;
		}

		// Token: 0x060067B9 RID: 26553 RVA: 0x0017B654 File Offset: 0x0017A654
		protected virtual bool CreateChildren()
		{
			return this.CreateChildren(false);
		}

		// Token: 0x060067BA RID: 26554 RVA: 0x0017B660 File Offset: 0x0017A660
		protected virtual bool CreateChildren(bool diffOldChildren)
		{
			if (!this.GetFlagSet(131072))
			{
				if (this.childCollection != null)
				{
					this.childCollection.Clear();
				}
				else
				{
					this.childCollection = new GridEntryCollection(this, new GridEntry[0]);
				}
				return false;
			}
			if (!diffOldChildren && this.childCollection != null && this.childCollection.Count > 0)
			{
				return true;
			}
			GridEntry[] propEntries = this.GetPropEntries(this, this.PropertyValue, this.PropertyType);
			bool flag = propEntries != null && propEntries.Length > 0;
			if (diffOldChildren && this.childCollection != null && this.childCollection.Count > 0)
			{
				bool flag2 = true;
				if (propEntries.Length == this.childCollection.Count)
				{
					for (int i = 0; i < propEntries.Length; i++)
					{
						if (!propEntries[i].NonParentEquals(this.childCollection[i]))
						{
							flag2 = false;
							break;
						}
					}
				}
				else
				{
					flag2 = false;
				}
				if (flag2)
				{
					return true;
				}
			}
			if (!flag)
			{
				this.SetFlag(524288, true);
				if (this.childCollection != null)
				{
					this.childCollection.Clear();
				}
				else
				{
					this.childCollection = new GridEntryCollection(this, new GridEntry[0]);
				}
				if (this.InternalExpanded)
				{
					this.InternalExpanded = false;
				}
			}
			else if (this.childCollection != null)
			{
				this.childCollection.Clear();
				this.childCollection.AddRange(propEntries);
			}
			else
			{
				this.childCollection = new GridEntryCollection(this, propEntries);
			}
			return flag;
		}

		// Token: 0x060067BB RID: 26555 RVA: 0x0017B7B1 File Offset: 0x0017A7B1
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060067BC RID: 26556 RVA: 0x0017B7C0 File Offset: 0x0017A7C0
		protected virtual void Dispose(bool disposing)
		{
			this.flags |= int.MinValue;
			this.SetFlag(8192, true);
			this.cacheItems = null;
			this.converter = null;
			this.editor = null;
			this.accessibleObject = null;
			if (disposing)
			{
				this.DisposeChildren();
			}
		}

		// Token: 0x060067BD RID: 26557 RVA: 0x0017B810 File Offset: 0x0017A810
		public virtual void DisposeChildren()
		{
			if (this.childCollection != null)
			{
				this.childCollection.Dispose();
				this.childCollection = null;
			}
		}

		// Token: 0x060067BE RID: 26558 RVA: 0x0017B82C File Offset: 0x0017A82C
		~GridEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x060067BF RID: 26559 RVA: 0x0017B85C File Offset: 0x0017A85C
		internal virtual void EditPropertyValue(PropertyGridView iva)
		{
			if (this.UITypeEditor != null)
			{
				try
				{
					object propertyValue = this.PropertyValue;
					object obj = this.UITypeEditor.EditValue(this, this, propertyValue);
					if (!this.Disposed)
					{
						if (obj != propertyValue && this.IsValueEditable)
						{
							iva.CommitValue(this, obj);
						}
						if (this.InternalExpanded)
						{
							PropertyGridView.GridPositionData gridPositionData = this.GridEntryHost.CaptureGridPositionData();
							this.InternalExpanded = false;
							this.RecreateChildren();
							gridPositionData.Restore(this.GridEntryHost);
						}
						else
						{
							this.RecreateChildren();
						}
					}
				}
				catch (Exception ex)
				{
					IUIService iuiservice = (IUIService)this.GetService(typeof(IUIService));
					if (iuiservice != null)
					{
						iuiservice.ShowError(ex);
					}
					else
					{
						RTLAwareMessageBox.Show(this.GridEntryHost, ex.Message, SR.GetString("PBRSErrorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
					}
				}
			}
		}

		// Token: 0x060067C0 RID: 26560 RVA: 0x0017B940 File Offset: 0x0017A940
		public override bool Equals(object obj)
		{
			return this.NonParentEquals(obj) && ((GridEntry)obj).ParentGridEntry == this.ParentGridEntry;
		}

		// Token: 0x060067C1 RID: 26561 RVA: 0x0017B960 File Offset: 0x0017A960
		public virtual object FindPropertyValue(string propertyName, Type propertyType)
		{
			object valueOwner = this.GetValueOwner();
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(valueOwner)[propertyName];
			if (propertyDescriptor != null && propertyDescriptor.PropertyType == propertyType)
			{
				return propertyDescriptor.GetValue(valueOwner);
			}
			if (this.parentPE != null)
			{
				return this.parentPE.FindPropertyValue(propertyName, propertyType);
			}
			return null;
		}

		// Token: 0x060067C2 RID: 26562 RVA: 0x0017B9AC File Offset: 0x0017A9AC
		internal virtual int GetChildIndex(GridEntry pe)
		{
			return this.Children.GetEntry(pe);
		}

		// Token: 0x060067C3 RID: 26563 RVA: 0x0017B9BC File Offset: 0x0017A9BC
		public virtual IComponent[] GetComponents()
		{
			IComponent component = this.Component;
			if (component != null)
			{
				return new IComponent[]
				{
					component
				};
			}
			return null;
		}

		// Token: 0x060067C4 RID: 26564 RVA: 0x0017B9E4 File Offset: 0x0017A9E4
		protected int GetLabelTextWidth(string labelText, Graphics g, Font f)
		{
			if (this.cacheItems == null)
			{
				this.cacheItems = new GridEntry.CacheItems();
			}
			else if (this.cacheItems.useCompatTextRendering == this.ownerGrid.UseCompatibleTextRendering && this.cacheItems.lastLabel == labelText && f.Equals(this.cacheItems.lastLabelFont))
			{
				return this.cacheItems.lastLabelWidth;
			}
			SizeF sizeF = PropertyGrid.MeasureTextHelper.MeasureText(this.ownerGrid, g, labelText, f);
			this.cacheItems.lastLabelWidth = (int)sizeF.Width;
			this.cacheItems.lastLabel = labelText;
			this.cacheItems.lastLabelFont = f;
			this.cacheItems.useCompatTextRendering = this.ownerGrid.UseCompatibleTextRendering;
			return this.cacheItems.lastLabelWidth;
		}

		// Token: 0x060067C5 RID: 26565 RVA: 0x0017BAAC File Offset: 0x0017AAAC
		internal int GetValueTextWidth(string valueString, Graphics g, Font f)
		{
			if (this.cacheItems == null)
			{
				this.cacheItems = new GridEntry.CacheItems();
			}
			else if (this.cacheItems.lastValueTextWidth != -1 && this.cacheItems.lastValueString == valueString && f.Equals(this.cacheItems.lastValueFont))
			{
				return this.cacheItems.lastValueTextWidth;
			}
			this.cacheItems.lastValueTextWidth = (int)g.MeasureString(valueString, f).Width;
			this.cacheItems.lastValueString = valueString;
			this.cacheItems.lastValueFont = f;
			return this.cacheItems.lastValueTextWidth;
		}

		// Token: 0x060067C6 RID: 26566 RVA: 0x0017BB4D File Offset: 0x0017AB4D
		internal bool GetMultipleLines(string valueString)
		{
			return valueString.IndexOf('\n') > 0 || valueString.IndexOf('\r') > 0;
		}

		// Token: 0x060067C7 RID: 26567 RVA: 0x0017BB68 File Offset: 0x0017AB68
		public virtual object GetValueOwner()
		{
			if (this.parentPE == null)
			{
				return this.PropertyValue;
			}
			return this.parentPE.GetChildValueOwner(this);
		}

		// Token: 0x060067C8 RID: 26568 RVA: 0x0017BB88 File Offset: 0x0017AB88
		public virtual object[] GetValueOwners()
		{
			object valueOwner = this.GetValueOwner();
			if (valueOwner != null)
			{
				return new object[]
				{
					valueOwner
				};
			}
			return null;
		}

		// Token: 0x060067C9 RID: 26569 RVA: 0x0017BBAD File Offset: 0x0017ABAD
		public virtual object GetChildValueOwner(GridEntry childEntry)
		{
			return this.PropertyValue;
		}

		// Token: 0x060067CA RID: 26570 RVA: 0x0017BBB8 File Offset: 0x0017ABB8
		public virtual string GetTestingInfo()
		{
			string text = "object = (";
			string text2 = this.GetPropertyTextValue();
			if (text2 == null)
			{
				text2 = "(null)";
			}
			else
			{
				text2 = text2.Replace('\0', ' ');
			}
			Type type = this.PropertyType;
			if (type == null)
			{
				type = typeof(object);
			}
			text += this.FullLabel;
			object obj = text;
			return string.Concat(new object[]
			{
				obj,
				"), property = (",
				this.PropertyLabel,
				",",
				type.AssemblyQualifiedName,
				"), value = [",
				text2,
				"], expandable = ",
				this.Expandable.ToString(),
				", readOnly = ",
				this.ShouldRenderReadOnly
			});
		}

		// Token: 0x060067CB RID: 26571 RVA: 0x0017BC8C File Offset: 0x0017AC8C
		public virtual Type GetValueType()
		{
			return this.PropertyType;
		}

		// Token: 0x060067CC RID: 26572 RVA: 0x0017BC94 File Offset: 0x0017AC94
		protected virtual GridEntry[] GetPropEntries(GridEntry peParent, object obj, Type objType)
		{
			if (obj == null)
			{
				return null;
			}
			GridEntry[] array = null;
			Attribute[] array2 = new Attribute[this.BrowsableAttributes.Count];
			this.BrowsableAttributes.CopyTo(array2, 0);
			PropertyTab currentTab = this.CurrentTab;
			try
			{
				bool flag = this.ForceReadOnly;
				if (!flag)
				{
					ReadOnlyAttribute readOnlyAttribute = (ReadOnlyAttribute)TypeDescriptor.GetAttributes(obj)[typeof(ReadOnlyAttribute)];
					flag = (readOnlyAttribute != null && !readOnlyAttribute.IsDefaultAttribute());
				}
				if (this.TypeConverter.GetPropertiesSupported(this) || this.AlwaysAllowExpand)
				{
					PropertyDescriptor propertyDescriptor = null;
					PropertyDescriptorCollection propertyDescriptorCollection;
					if (currentTab != null)
					{
						propertyDescriptorCollection = currentTab.GetProperties(this, obj, array2);
						propertyDescriptor = currentTab.GetDefaultProperty(obj);
					}
					else
					{
						propertyDescriptorCollection = this.TypeConverter.GetProperties(this, obj, array2);
						propertyDescriptor = TypeDescriptor.GetDefaultProperty(obj);
					}
					if (propertyDescriptorCollection == null)
					{
						return null;
					}
					if ((this.PropertySort & PropertySort.Alphabetical) != PropertySort.NoSort)
					{
						if (objType == null || !objType.IsArray)
						{
							propertyDescriptorCollection = propertyDescriptorCollection.Sort(GridEntry.DisplayNameComparer);
						}
						PropertyDescriptor[] array3 = new PropertyDescriptor[propertyDescriptorCollection.Count];
						propertyDescriptorCollection.CopyTo(array3, 0);
						propertyDescriptorCollection = new PropertyDescriptorCollection(this.SortParenProperties(array3));
					}
					if (propertyDescriptor == null && propertyDescriptorCollection.Count > 0)
					{
						propertyDescriptor = propertyDescriptorCollection[0];
					}
					if ((propertyDescriptorCollection == null || propertyDescriptorCollection.Count == 0) && objType != null && objType.IsArray && obj != null)
					{
						Array array4 = (Array)obj;
						array = new GridEntry[array4.Length];
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = new ArrayElementGridEntry(this.ownerGrid, peParent, i);
						}
					}
					else
					{
						bool createInstanceSupported = this.TypeConverter.GetCreateInstanceSupported(this);
						array = new GridEntry[propertyDescriptorCollection.Count];
						int num = 0;
						foreach (object obj2 in propertyDescriptorCollection)
						{
							PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)obj2;
							bool hide = false;
							try
							{
								object component = obj;
								if (obj is ICustomTypeDescriptor)
								{
									component = ((ICustomTypeDescriptor)obj).GetPropertyOwner(propertyDescriptor2);
								}
								propertyDescriptor2.GetValue(component);
							}
							catch (Exception)
							{
								bool enabled = GridEntry.PbrsAssertPropsSwitch.Enabled;
								hide = true;
							}
							GridEntry gridEntry;
							if (createInstanceSupported)
							{
								gridEntry = new ImmutablePropertyDescriptorGridEntry(this.ownerGrid, peParent, propertyDescriptor2, hide);
							}
							else
							{
								gridEntry = new PropertyDescriptorGridEntry(this.ownerGrid, peParent, propertyDescriptor2, hide);
							}
							if (flag)
							{
								gridEntry.flags |= 1024;
							}
							if (propertyDescriptor2.Equals(propertyDescriptor))
							{
								this.DefaultChild = gridEntry;
							}
							array[num++] = gridEntry;
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return array;
		}

		// Token: 0x060067CD RID: 26573 RVA: 0x0017BF68 File Offset: 0x0017AF68
		public virtual void ResetPropertyValue()
		{
			this.NotifyValue(1);
			this.Refresh();
		}

		// Token: 0x060067CE RID: 26574 RVA: 0x0017BF78 File Offset: 0x0017AF78
		public virtual bool CanResetPropertyValue()
		{
			return this.NotifyValue(2);
		}

		// Token: 0x060067CF RID: 26575 RVA: 0x0017BF81 File Offset: 0x0017AF81
		public virtual bool DoubleClickPropertyValue()
		{
			return this.NotifyValue(3);
		}

		// Token: 0x060067D0 RID: 26576 RVA: 0x0017BF8A File Offset: 0x0017AF8A
		public virtual string GetPropertyTextValue()
		{
			return this.GetPropertyTextValue(this.PropertyValue);
		}

		// Token: 0x060067D1 RID: 26577 RVA: 0x0017BF98 File Offset: 0x0017AF98
		public virtual string GetPropertyTextValue(object value)
		{
			string text = null;
			TypeConverter typeConverter = this.TypeConverter;
			try
			{
				text = typeConverter.ConvertToString(this, value);
			}
			catch (Exception)
			{
			}
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}

		// Token: 0x060067D2 RID: 26578 RVA: 0x0017BFD8 File Offset: 0x0017AFD8
		public virtual object[] GetPropertyValueList()
		{
			ICollection standardValues = this.TypeConverter.GetStandardValues(this);
			if (standardValues != null)
			{
				object[] array = new object[standardValues.Count];
				standardValues.CopyTo(array, 0);
				return array;
			}
			return new object[0];
		}

		// Token: 0x060067D3 RID: 26579 RVA: 0x0017C014 File Offset: 0x0017B014
		public override int GetHashCode()
		{
			object propertyLabel = this.PropertyLabel;
			object propertyType = this.PropertyType;
			uint num = (uint)((propertyLabel == null) ? 0 : propertyLabel.GetHashCode());
			uint num2 = (uint)((propertyType == null) ? 0 : propertyType.GetHashCode());
			uint hashCode = (uint)base.GetType().GetHashCode();
			return (int)(num ^ (num2 << 13 | num2 >> 19) ^ (hashCode << 26 | hashCode >> 6));
		}

		// Token: 0x060067D4 RID: 26580 RVA: 0x0017C06C File Offset: 0x0017B06C
		protected virtual bool GetFlagSet(int flag)
		{
			return (flag & this.Flags) != 0;
		}

		// Token: 0x060067D5 RID: 26581 RVA: 0x0017C07C File Offset: 0x0017B07C
		protected Font GetFont(bool boldFont)
		{
			if (boldFont)
			{
				return this.GridEntryHost.GetBoldFont();
			}
			return this.GridEntryHost.GetBaseFont();
		}

		// Token: 0x060067D6 RID: 26582 RVA: 0x0017C098 File Offset: 0x0017B098
		protected IntPtr GetHfont(bool boldFont)
		{
			if (boldFont)
			{
				return this.GridEntryHost.GetBoldHfont();
			}
			return this.GridEntryHost.GetBaseHfont();
		}

		// Token: 0x060067D7 RID: 26583 RVA: 0x0017C0B4 File Offset: 0x0017B0B4
		public virtual object GetService(Type serviceType)
		{
			if (serviceType == typeof(GridItem))
			{
				return this;
			}
			if (this.parentPE != null)
			{
				return this.parentPE.GetService(serviceType);
			}
			return null;
		}

		// Token: 0x060067D8 RID: 26584 RVA: 0x0017C0DC File Offset: 0x0017B0DC
		internal virtual bool NonParentEquals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			if (obj == null)
			{
				return false;
			}
			if (!(obj is GridEntry))
			{
				return false;
			}
			GridEntry gridEntry = (GridEntry)obj;
			return gridEntry.PropertyLabel.Equals(this.PropertyLabel) && gridEntry.PropertyType.Equals(this.PropertyType) && gridEntry.PropertyDepth == this.PropertyDepth;
		}

		// Token: 0x060067D9 RID: 26585 RVA: 0x0017C13C File Offset: 0x0017B13C
		public virtual void PaintLabel(Graphics g, Rectangle rect, Rectangle clipRect, bool selected, bool paintFullLabel)
		{
			PropertyGridView gridEntryHost = this.GridEntryHost;
			string propertyLabel = this.PropertyLabel;
			int num = gridEntryHost.GetOutlineIconSize() + 5;
			Brush brush = selected ? SystemBrushes.Highlight : this.GetBackgroundBrush(g);
			if (selected && !this.hasFocus)
			{
				brush = gridEntryHost.GetLineBrush(g);
			}
			bool boldFont = (this.Flags & 64) != 0;
			Font font = this.GetFont(boldFont);
			int labelTextWidth = this.GetLabelTextWidth(propertyLabel, g, font);
			int num2 = paintFullLabel ? labelTextWidth : 0;
			int num3 = rect.X + this.PropertyLabelIndent;
			Brush brush2 = brush;
			if (paintFullLabel && num2 >= rect.Width - (num3 + 2))
			{
				int num4 = num3 + num2 + 2;
				g.FillRectangle(brush2, num - 1, rect.Y, num4 - num + 3, rect.Height);
				Pen pen = new Pen(gridEntryHost.GetLineColor());
				g.DrawLine(pen, num4, rect.Y, num4, rect.Height);
				pen.Dispose();
				rect.Width = num4 - rect.X;
			}
			else
			{
				g.FillRectangle(brush2, rect.X, rect.Y, rect.Width, rect.Height);
			}
			Brush lineBrush = gridEntryHost.GetLineBrush(g);
			g.FillRectangle(lineBrush, rect.X, rect.Y, num, rect.Height);
			if (selected && this.hasFocus)
			{
				g.FillRectangle(SystemBrushes.Highlight, num3, rect.Y, rect.Width - num3 - 1, rect.Height);
			}
			int num5 = Math.Min(rect.Width - num3 - 1, labelTextWidth + 2);
			Rectangle rectangle = new Rectangle(num3, rect.Y + 1, num5, rect.Height - 1);
			if (!Rectangle.Intersect(rectangle, clipRect).IsEmpty)
			{
				Region clip = g.Clip;
				g.SetClip(rectangle);
				Color color = (selected && this.hasFocus) ? SystemColors.HighlightText : g.GetNearestColor(this.LabelTextColor);
				if (this.ownerGrid.UseCompatibleTextRendering)
				{
					using (Brush brush3 = new SolidBrush(color))
					{
						StringFormat stringFormat = new StringFormat(StringFormatFlags.NoWrap);
						stringFormat.Trimming = StringTrimming.None;
						g.DrawString(propertyLabel, font, brush3, rectangle, stringFormat);
						goto IL_257;
					}
				}
				TextRenderer.DrawText(g, propertyLabel, font, rectangle, color, PropertyGrid.MeasureTextHelper.GetTextRendererFlags());
				IL_257:
				g.SetClip(clip, CombineMode.Replace);
				clip.Dispose();
				if (num5 <= labelTextWidth)
				{
					this.labelTipPoint = new Point(num3 + 2, rect.Y + 1);
				}
				else
				{
					this.labelTipPoint = GridEntry.InvalidPoint;
				}
			}
			rect.Y--;
			rect.Height += 2;
			this.PaintOutline(g, rect);
		}

		// Token: 0x060067DA RID: 26586 RVA: 0x0017C414 File Offset: 0x0017B414
		public virtual void PaintOutline(Graphics g, Rectangle r)
		{
			if (this.Expandable)
			{
				bool internalExpanded = this.InternalExpanded;
				Rectangle rectangle = this.OutlineRect;
				rectangle = Rectangle.Intersect(r, rectangle);
				if (rectangle.IsEmpty)
				{
					return;
				}
				Brush backgroundBrush = this.GetBackgroundBrush(g);
				Color textColor = this.GridEntryHost.GetTextColor();
				Pen pen;
				if (textColor.IsSystemColor)
				{
					pen = SystemPens.FromSystemColor(textColor);
				}
				else
				{
					pen = new Pen(textColor);
				}
				g.FillRectangle(backgroundBrush, rectangle);
				g.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
				int num = 2;
				g.DrawLine(pen, rectangle.X + num, rectangle.Y + rectangle.Height / 2, rectangle.X + rectangle.Width - num - 1, rectangle.Y + rectangle.Height / 2);
				if (!internalExpanded)
				{
					g.DrawLine(pen, rectangle.X + rectangle.Width / 2, rectangle.Y + num, rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height - num - 1);
				}
				if (!textColor.IsSystemColor)
				{
					pen.Dispose();
				}
			}
		}

		// Token: 0x060067DB RID: 26587 RVA: 0x0017C550 File Offset: 0x0017B550
		public virtual void PaintValue(object val, Graphics g, Rectangle rect, Rectangle clipRect, GridEntry.PaintValueFlags paintFlags)
		{
			PropertyGridView gridEntryHost = this.GridEntryHost;
			int num = 0;
			Color color = gridEntryHost.GetTextColor();
			if (this.ShouldRenderReadOnly)
			{
				color = this.GridEntryHost.GrayTextColor;
			}
			string text;
			if ((paintFlags & GridEntry.PaintValueFlags.FetchValue) != GridEntry.PaintValueFlags.None)
			{
				if (this.cacheItems != null && this.cacheItems.useValueString)
				{
					text = this.cacheItems.lastValueString;
					val = this.cacheItems.lastValue;
				}
				else
				{
					val = this.PropertyValue;
					text = this.GetPropertyTextValue(val);
					if (this.cacheItems == null)
					{
						this.cacheItems = new GridEntry.CacheItems();
					}
					this.cacheItems.lastValueString = text;
					this.cacheItems.useValueString = true;
					this.cacheItems.lastValueTextWidth = -1;
					this.cacheItems.lastValueFont = null;
					this.cacheItems.lastValue = val;
				}
			}
			else
			{
				text = this.GetPropertyTextValue(val);
			}
			Brush brush = this.GetBackgroundBrush(g);
			if ((paintFlags & GridEntry.PaintValueFlags.DrawSelected) != GridEntry.PaintValueFlags.None)
			{
				brush = SystemBrushes.Highlight;
				color = SystemColors.HighlightText;
			}
			Brush brush2 = brush;
			g.FillRectangle(brush2, clipRect);
			if (this.IsCustomPaint)
			{
				num = gridEntryHost.GetValuePaintIndent();
				Rectangle rectangle = new Rectangle(rect.X + 1, rect.Y + 1, gridEntryHost.GetValuePaintWidth(), gridEntryHost.GetGridEntryHeight() - 2);
				if (!Rectangle.Intersect(rectangle, clipRect).IsEmpty)
				{
					UITypeEditor uitypeEditor = this.UITypeEditor;
					if (uitypeEditor != null)
					{
						uitypeEditor.PaintValue(new PaintValueEventArgs(this, val, g, rectangle));
					}
					rectangle.Width--;
					rectangle.Height--;
					g.DrawRectangle(SystemPens.WindowText, rectangle);
				}
			}
			rect.X += num + gridEntryHost.GetValueStringIndent();
			rect.Width -= num + 2 * gridEntryHost.GetValueStringIndent();
			bool boldFont = (paintFlags & GridEntry.PaintValueFlags.CheckShouldSerialize) != GridEntry.PaintValueFlags.None && this.ShouldSerializePropertyValue();
			if (text != null && text.Length > 0)
			{
				Font font = this.GetFont(boldFont);
				if (text.Length > 1000)
				{
					text = text.Substring(0, 1000);
				}
				int valueTextWidth = this.GetValueTextWidth(text, g, font);
				bool flag = false;
				if (valueTextWidth >= rect.Width || this.GetMultipleLines(text))
				{
					flag = true;
				}
				if (Rectangle.Intersect(rect, clipRect).IsEmpty)
				{
					return;
				}
				if ((paintFlags & GridEntry.PaintValueFlags.PaintInPlace) != GridEntry.PaintValueFlags.None)
				{
					rect.Offset(1, 2);
				}
				else
				{
					rect.Offset(1, 1);
				}
				Matrix transform = g.Transform;
				IntPtr hdc = g.GetHdc();
				IntNativeMethods.RECT rect2 = IntNativeMethods.RECT.FromXYWH(rect.X + (int)transform.OffsetX + 2, rect.Y + (int)transform.OffsetY - 1, rect.Width - 4, rect.Height);
				IntPtr handle = this.GetHfont(boldFont);
				int crColor = 0;
				int clr = 0;
				Color color2 = ((paintFlags & GridEntry.PaintValueFlags.DrawSelected) != GridEntry.PaintValueFlags.None) ? SystemColors.Highlight : this.GridEntryHost.BackColor;
				try
				{
					crColor = SafeNativeMethods.SetTextColor(new HandleRef(g, hdc), SafeNativeMethods.RGBToCOLORREF(color.ToArgb()));
					clr = SafeNativeMethods.SetBkColor(new HandleRef(g, hdc), SafeNativeMethods.RGBToCOLORREF(color2.ToArgb()));
					handle = SafeNativeMethods.SelectObject(new HandleRef(g, hdc), new HandleRef(null, handle));
					int num2 = 10592;
					if (gridEntryHost.DrawValuesRightToLeft)
					{
						num2 |= 131074;
					}
					if (this.ShouldRenderPassword)
					{
						if (GridEntry.passwordReplaceChar == '\0')
						{
							if (Environment.OSVersion.Version.Major > 4)
							{
								GridEntry.passwordReplaceChar = '●';
							}
							else
							{
								GridEntry.passwordReplaceChar = '*';
							}
						}
						text = new string(GridEntry.passwordReplaceChar, text.Length);
					}
					IntUnsafeNativeMethods.DrawText(new HandleRef(g, hdc), text, ref rect2, num2);
				}
				finally
				{
					SafeNativeMethods.SetTextColor(new HandleRef(g, hdc), crColor);
					SafeNativeMethods.SetBkColor(new HandleRef(g, hdc), clr);
					handle = SafeNativeMethods.SelectObject(new HandleRef(g, hdc), new HandleRef(null, handle));
					g.ReleaseHdcInternal(hdc);
				}
				if (flag)
				{
					this.ValueToolTipLocation = new Point(rect.X + 2, rect.Y - 1);
					return;
				}
				this.ValueToolTipLocation = GridEntry.InvalidPoint;
			}
		}

		// Token: 0x060067DC RID: 26588 RVA: 0x0017C960 File Offset: 0x0017B960
		public virtual bool OnComponentChanging()
		{
			if (this.ComponentChangeService != null)
			{
				try
				{
					this.ComponentChangeService.OnComponentChanging(this.GetValueOwner(), this.PropertyDescriptor);
				}
				catch (CheckoutException ex)
				{
					if (ex == CheckoutException.Canceled)
					{
						return false;
					}
					throw ex;
				}
				return true;
			}
			return true;
		}

		// Token: 0x060067DD RID: 26589 RVA: 0x0017C9B0 File Offset: 0x0017B9B0
		public virtual void OnComponentChanged()
		{
			if (this.ComponentChangeService != null)
			{
				this.ComponentChangeService.OnComponentChanged(this.GetValueOwner(), this.PropertyDescriptor, null, null);
			}
		}

		// Token: 0x060067DE RID: 26590 RVA: 0x0017C9D3 File Offset: 0x0017B9D3
		protected virtual void OnLabelClick(EventArgs e)
		{
			this.RaiseEvent(GridEntry.EVENT_LABEL_CLICK, e);
		}

		// Token: 0x060067DF RID: 26591 RVA: 0x0017C9E1 File Offset: 0x0017B9E1
		protected virtual void OnLabelDoubleClick(EventArgs e)
		{
			this.RaiseEvent(GridEntry.EVENT_LABEL_DBLCLICK, e);
		}

		// Token: 0x060067E0 RID: 26592 RVA: 0x0017C9F0 File Offset: 0x0017B9F0
		public virtual bool OnMouseClick(int x, int y, int count, MouseButtons button)
		{
			PropertyGridView gridEntryHost = this.GridEntryHost;
			if ((button & MouseButtons.Left) != MouseButtons.Left)
			{
				return false;
			}
			int num = gridEntryHost.GetLabelWidth();
			if (x >= 0 && x <= num)
			{
				if (this.Expandable && this.OutlineRect.Contains(x, y))
				{
					if (count % 2 == 0)
					{
						this.OnOutlineDoubleClick(EventArgs.Empty);
					}
					else
					{
						this.OnOutlineClick(EventArgs.Empty);
					}
					return true;
				}
				if (count % 2 == 0)
				{
					this.OnLabelDoubleClick(EventArgs.Empty);
				}
				else
				{
					this.OnLabelClick(EventArgs.Empty);
				}
				return true;
			}
			else
			{
				num += gridEntryHost.GetSplitterWidth();
				if (x >= num && x <= num + gridEntryHost.GetValueWidth())
				{
					if (count % 2 == 0)
					{
						this.OnValueDoubleClick(EventArgs.Empty);
					}
					else
					{
						this.OnValueClick(EventArgs.Empty);
					}
					return true;
				}
				return false;
			}
		}

		// Token: 0x060067E1 RID: 26593 RVA: 0x0017CAB4 File Offset: 0x0017BAB4
		protected virtual void OnOutlineClick(EventArgs e)
		{
			this.RaiseEvent(GridEntry.EVENT_OUTLINE_CLICK, e);
		}

		// Token: 0x060067E2 RID: 26594 RVA: 0x0017CAC2 File Offset: 0x0017BAC2
		protected virtual void OnOutlineDoubleClick(EventArgs e)
		{
			this.RaiseEvent(GridEntry.EVENT_OUTLINE_DBLCLICK, e);
		}

		// Token: 0x060067E3 RID: 26595 RVA: 0x0017CAD0 File Offset: 0x0017BAD0
		protected virtual void OnRecreateChildren(GridEntryRecreateChildrenEventArgs e)
		{
			Delegate eventHandler = this.GetEventHandler(GridEntry.EVENT_RECREATE_CHILDREN);
			if (eventHandler != null)
			{
				((GridEntryRecreateChildrenEventHandler)eventHandler)(this, e);
			}
		}

		// Token: 0x060067E4 RID: 26596 RVA: 0x0017CAF9 File Offset: 0x0017BAF9
		protected virtual void OnValueClick(EventArgs e)
		{
			this.RaiseEvent(GridEntry.EVENT_VALUE_CLICK, e);
		}

		// Token: 0x060067E5 RID: 26597 RVA: 0x0017CB07 File Offset: 0x0017BB07
		protected virtual void OnValueDoubleClick(EventArgs e)
		{
			this.RaiseEvent(GridEntry.EVENT_VALUE_DBLCLICK, e);
		}

		// Token: 0x060067E6 RID: 26598 RVA: 0x0017CB15 File Offset: 0x0017BB15
		internal bool OnValueReturnKey()
		{
			return this.NotifyValue(5);
		}

		// Token: 0x060067E7 RID: 26599 RVA: 0x0017CB1E File Offset: 0x0017BB1E
		protected virtual void SetFlag(int flag, bool fVal)
		{
			this.SetFlag(flag, fVal ? flag : 0);
		}

		// Token: 0x060067E8 RID: 26600 RVA: 0x0017CB2E File Offset: 0x0017BB2E
		protected virtual void SetFlag(int flag_valid, int flag, bool fVal)
		{
			this.SetFlag(flag_valid | flag, flag_valid | (fVal ? flag : 0));
		}

		// Token: 0x060067E9 RID: 26601 RVA: 0x0017CB42 File Offset: 0x0017BB42
		protected virtual void SetFlag(int flag, int val)
		{
			this.Flags = ((this.Flags & ~flag) | val);
		}

		// Token: 0x060067EA RID: 26602 RVA: 0x0017CB58 File Offset: 0x0017BB58
		public override bool Select()
		{
			if (this.Disposed)
			{
				return false;
			}
			try
			{
				this.GridEntryHost.SelectedGridEntry = this;
				return true;
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x060067EB RID: 26603 RVA: 0x0017CB98 File Offset: 0x0017BB98
		internal virtual bool ShouldSerializePropertyValue()
		{
			if (this.cacheItems != null)
			{
				if (this.cacheItems.useShouldSerialize)
				{
					return this.cacheItems.lastShouldSerialize;
				}
				this.cacheItems.lastShouldSerialize = this.NotifyValue(4);
				this.cacheItems.useShouldSerialize = true;
			}
			else
			{
				this.cacheItems = new GridEntry.CacheItems();
				this.cacheItems.lastShouldSerialize = this.NotifyValue(4);
				this.cacheItems.useShouldSerialize = true;
			}
			return this.cacheItems.lastShouldSerialize;
		}

		// Token: 0x060067EC RID: 26604 RVA: 0x0017CC1C File Offset: 0x0017BC1C
		private PropertyDescriptor[] SortParenProperties(PropertyDescriptor[] props)
		{
			PropertyDescriptor[] array = null;
			int num = 0;
			for (int i = 0; i < props.Length; i++)
			{
				if (((ParenthesizePropertyNameAttribute)props[i].Attributes[typeof(ParenthesizePropertyNameAttribute)]).NeedParenthesis)
				{
					if (array == null)
					{
						array = new PropertyDescriptor[props.Length];
					}
					array[num++] = props[i];
					props[i] = null;
				}
			}
			if (num > 0)
			{
				for (int j = 0; j < props.Length; j++)
				{
					if (props[j] != null)
					{
						array[num++] = props[j];
					}
				}
				props = array;
			}
			return props;
		}

		// Token: 0x060067ED RID: 26605 RVA: 0x0017CC9D File Offset: 0x0017BC9D
		internal virtual bool NotifyValueGivenParent(object obj, int type)
		{
			return false;
		}

		// Token: 0x060067EE RID: 26606 RVA: 0x0017CCA0 File Offset: 0x0017BCA0
		internal virtual bool NotifyChildValue(GridEntry pe, int type)
		{
			return pe.NotifyValueGivenParent(pe.GetValueOwner(), type);
		}

		// Token: 0x060067EF RID: 26607 RVA: 0x0017CCAF File Offset: 0x0017BCAF
		internal virtual bool NotifyValue(int type)
		{
			return this.parentPE == null || this.parentPE.NotifyChildValue(this, type);
		}

		// Token: 0x060067F0 RID: 26608 RVA: 0x0017CCC8 File Offset: 0x0017BCC8
		internal void RecreateChildren()
		{
			this.RecreateChildren(-1);
		}

		// Token: 0x060067F1 RID: 26609 RVA: 0x0017CCD4 File Offset: 0x0017BCD4
		internal void RecreateChildren(int oldCount)
		{
			bool internalExpanded = this.InternalExpanded || oldCount > 0;
			if (oldCount == -1)
			{
				oldCount = this.VisibleChildCount;
			}
			this.ResetState();
			if (oldCount == 0)
			{
				return;
			}
			foreach (object obj in this.ChildCollection)
			{
				GridEntry gridEntry = (GridEntry)obj;
				gridEntry.RecreateChildren();
			}
			this.DisposeChildren();
			this.InternalExpanded = internalExpanded;
			this.OnRecreateChildren(new GridEntryRecreateChildrenEventArgs(oldCount, this.VisibleChildCount));
		}

		// Token: 0x060067F2 RID: 26610 RVA: 0x0017CD74 File Offset: 0x0017BD74
		public virtual void Refresh()
		{
			Type propertyType = this.PropertyType;
			if (propertyType != null && propertyType.IsArray)
			{
				this.CreateChildren(true);
			}
			if (this.childCollection != null)
			{
				if (this.InternalExpanded && this.cacheItems != null && this.cacheItems.lastValue != null && this.cacheItems.lastValue != this.PropertyValue)
				{
					this.ClearCachedValues();
					this.RecreateChildren();
					return;
				}
				if (this.InternalExpanded)
				{
					foreach (object obj in this.childCollection)
					{
						GridEntry gridEntry = (GridEntry)obj;
						gridEntry.Refresh();
					}
				}
				else
				{
					this.DisposeChildren();
				}
			}
			this.ClearCachedValues();
		}

		// Token: 0x060067F3 RID: 26611 RVA: 0x0017CE20 File Offset: 0x0017BE20
		public virtual void RemoveOnLabelClick(EventHandler h)
		{
			this.RemoveEventHandler(GridEntry.EVENT_LABEL_CLICK, h);
		}

		// Token: 0x060067F4 RID: 26612 RVA: 0x0017CE2E File Offset: 0x0017BE2E
		public virtual void RemoveOnLabelDoubleClick(EventHandler h)
		{
			this.RemoveEventHandler(GridEntry.EVENT_LABEL_DBLCLICK, h);
		}

		// Token: 0x060067F5 RID: 26613 RVA: 0x0017CE3C File Offset: 0x0017BE3C
		public virtual void RemoveOnValueClick(EventHandler h)
		{
			this.RemoveEventHandler(GridEntry.EVENT_VALUE_CLICK, h);
		}

		// Token: 0x060067F6 RID: 26614 RVA: 0x0017CE4A File Offset: 0x0017BE4A
		public virtual void RemoveOnValueDoubleClick(EventHandler h)
		{
			this.RemoveEventHandler(GridEntry.EVENT_VALUE_DBLCLICK, h);
		}

		// Token: 0x060067F7 RID: 26615 RVA: 0x0017CE58 File Offset: 0x0017BE58
		public virtual void RemoveOnOutlineClick(EventHandler h)
		{
			this.RemoveEventHandler(GridEntry.EVENT_OUTLINE_CLICK, h);
		}

		// Token: 0x060067F8 RID: 26616 RVA: 0x0017CE66 File Offset: 0x0017BE66
		public virtual void RemoveOnOutlineDoubleClick(EventHandler h)
		{
			this.RemoveEventHandler(GridEntry.EVENT_OUTLINE_DBLCLICK, h);
		}

		// Token: 0x060067F9 RID: 26617 RVA: 0x0017CE74 File Offset: 0x0017BE74
		public virtual void RemoveOnRecreateChildren(GridEntryRecreateChildrenEventHandler h)
		{
			this.RemoveEventHandler(GridEntry.EVENT_RECREATE_CHILDREN, h);
		}

		// Token: 0x060067FA RID: 26618 RVA: 0x0017CE82 File Offset: 0x0017BE82
		protected void ResetState()
		{
			this.Flags = 0;
			this.ClearCachedValues();
		}

		// Token: 0x060067FB RID: 26619 RVA: 0x0017CE94 File Offset: 0x0017BE94
		public virtual bool SetPropertyTextValue(string str)
		{
			bool flag = this.childCollection != null && this.childCollection.Count > 0;
			this.PropertyValue = this.ConvertTextToValue(str);
			this.CreateChildren();
			bool flag2 = this.childCollection != null && this.childCollection.Count > 0;
			return flag != flag2;
		}

		// Token: 0x060067FC RID: 26620 RVA: 0x0017CEF0 File Offset: 0x0017BEF0
		public override string ToString()
		{
			return base.GetType().FullName + " " + this.PropertyLabel;
		}

		// Token: 0x060067FD RID: 26621 RVA: 0x0017CF10 File Offset: 0x0017BF10
		protected virtual void AddEventHandler(object key, Delegate handler)
		{
			lock (this)
			{
				if (handler != null)
				{
					for (GridEntry.EventEntry next = this.eventList; next != null; next = next.next)
					{
						if (next.key == key)
						{
							next.handler = Delegate.Combine(next.handler, handler);
							return;
						}
					}
					this.eventList = new GridEntry.EventEntry(this.eventList, key, handler);
				}
			}
		}

		// Token: 0x060067FE RID: 26622 RVA: 0x0017CF88 File Offset: 0x0017BF88
		protected virtual void RaiseEvent(object key, EventArgs e)
		{
			Delegate eventHandler = this.GetEventHandler(key);
			if (eventHandler != null)
			{
				((EventHandler)eventHandler)(this, e);
			}
		}

		// Token: 0x060067FF RID: 26623 RVA: 0x0017CFB0 File Offset: 0x0017BFB0
		protected virtual Delegate GetEventHandler(object key)
		{
			Delegate result;
			lock (this)
			{
				for (GridEntry.EventEntry next = this.eventList; next != null; next = next.next)
				{
					if (next.key == key)
					{
						return next.handler;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06006800 RID: 26624 RVA: 0x0017D008 File Offset: 0x0017C008
		protected virtual void RemoveEventHandler(object key, Delegate handler)
		{
			lock (this)
			{
				if (handler != null)
				{
					GridEntry.EventEntry next = this.eventList;
					GridEntry.EventEntry eventEntry = null;
					while (next != null)
					{
						if (next.key == key)
						{
							next.handler = Delegate.Remove(next.handler, handler);
							if (next.handler == null)
							{
								if (eventEntry == null)
								{
									this.eventList = next.next;
								}
								else
								{
									eventEntry.next = next.next;
								}
							}
							break;
						}
						eventEntry = next;
						next = next.next;
					}
				}
			}
		}

		// Token: 0x06006801 RID: 26625 RVA: 0x0017D094 File Offset: 0x0017C094
		protected virtual void RemoveEventHandlers()
		{
			this.eventList = null;
		}

		// Token: 0x04003D12 RID: 15634
		internal const int FLAG_TEXT_EDITABLE = 1;

		// Token: 0x04003D13 RID: 15635
		internal const int FLAG_ENUMERABLE = 2;

		// Token: 0x04003D14 RID: 15636
		internal const int FLAG_CUSTOM_PAINT = 4;

		// Token: 0x04003D15 RID: 15637
		internal const int FLAG_IMMEDIATELY_EDITABLE = 8;

		// Token: 0x04003D16 RID: 15638
		internal const int FLAG_CUSTOM_EDITABLE = 16;

		// Token: 0x04003D17 RID: 15639
		internal const int FLAG_DROPDOWN_EDITABLE = 32;

		// Token: 0x04003D18 RID: 15640
		internal const int FLAG_LABEL_BOLD = 64;

		// Token: 0x04003D19 RID: 15641
		internal const int FLAG_READONLY_EDITABLE = 128;

		// Token: 0x04003D1A RID: 15642
		internal const int FLAG_RENDER_READONLY = 256;

		// Token: 0x04003D1B RID: 15643
		internal const int FLAG_IMMUTABLE = 512;

		// Token: 0x04003D1C RID: 15644
		internal const int FLAG_FORCE_READONLY = 1024;

		// Token: 0x04003D1D RID: 15645
		internal const int FLAG_RENDER_PASSWORD = 4096;

		// Token: 0x04003D1E RID: 15646
		internal const int FLAG_DISPOSED = 8192;

		// Token: 0x04003D1F RID: 15647
		internal const int FL_EXPAND = 65536;

		// Token: 0x04003D20 RID: 15648
		internal const int FL_EXPANDABLE = 131072;

		// Token: 0x04003D21 RID: 15649
		internal const int FL_EXPANDABLE_FAILED = 524288;

		// Token: 0x04003D22 RID: 15650
		internal const int FL_NO_CUSTOM_PAINT = 1048576;

		// Token: 0x04003D23 RID: 15651
		internal const int FL_CATEGORIES = 2097152;

		// Token: 0x04003D24 RID: 15652
		internal const int FL_CHECKED = -2147483648;

		// Token: 0x04003D25 RID: 15653
		protected const int NOTIFY_RESET = 1;

		// Token: 0x04003D26 RID: 15654
		protected const int NOTIFY_CAN_RESET = 2;

		// Token: 0x04003D27 RID: 15655
		protected const int NOTIFY_DBL_CLICK = 3;

		// Token: 0x04003D28 RID: 15656
		protected const int NOTIFY_SHOULD_PERSIST = 4;

		// Token: 0x04003D29 RID: 15657
		protected const int NOTIFY_RETURN = 5;

		// Token: 0x04003D2A RID: 15658
		protected const int OUTLINE_ICON_PADDING = 5;

		// Token: 0x04003D2B RID: 15659
		private const int maximumLengthOfPropertyString = 1000;

		// Token: 0x04003D2C RID: 15660
		protected static readonly Point InvalidPoint = new Point(int.MinValue, int.MinValue);

		// Token: 0x04003D2D RID: 15661
		private static BooleanSwitch PbrsAssertPropsSwitch = new BooleanSwitch("PbrsAssertProps", "PropertyBrowser : Assert on broken properties");

		// Token: 0x04003D2E RID: 15662
		internal static AttributeTypeSorter AttributeTypeSorter = new AttributeTypeSorter();

		// Token: 0x04003D2F RID: 15663
		protected static IComparer DisplayNameComparer = new GridEntry.DisplayNameSortComparer();

		// Token: 0x04003D30 RID: 15664
		private static char passwordReplaceChar;

		// Token: 0x04003D31 RID: 15665
		private GridEntry.CacheItems cacheItems;

		// Token: 0x04003D32 RID: 15666
		protected TypeConverter converter;

		// Token: 0x04003D33 RID: 15667
		protected UITypeEditor editor;

		// Token: 0x04003D34 RID: 15668
		internal GridEntry parentPE;

		// Token: 0x04003D35 RID: 15669
		private GridEntryCollection childCollection;

		// Token: 0x04003D36 RID: 15670
		internal int flags;

		// Token: 0x04003D37 RID: 15671
		private int propertyDepth;

		// Token: 0x04003D38 RID: 15672
		protected bool hasFocus;

		// Token: 0x04003D39 RID: 15673
		private Rectangle outlineRect = Rectangle.Empty;

		// Token: 0x04003D3A RID: 15674
		protected PropertySort PropertySort;

		// Token: 0x04003D3B RID: 15675
		protected Point labelTipPoint = GridEntry.InvalidPoint;

		// Token: 0x04003D3C RID: 15676
		protected Point valueTipPoint = GridEntry.InvalidPoint;

		// Token: 0x04003D3D RID: 15677
		protected PropertyGrid ownerGrid;

		// Token: 0x04003D3E RID: 15678
		private static object EVENT_VALUE_CLICK = new object();

		// Token: 0x04003D3F RID: 15679
		private static object EVENT_LABEL_CLICK = new object();

		// Token: 0x04003D40 RID: 15680
		private static object EVENT_OUTLINE_CLICK = new object();

		// Token: 0x04003D41 RID: 15681
		private static object EVENT_VALUE_DBLCLICK = new object();

		// Token: 0x04003D42 RID: 15682
		private static object EVENT_LABEL_DBLCLICK = new object();

		// Token: 0x04003D43 RID: 15683
		private static object EVENT_OUTLINE_DBLCLICK = new object();

		// Token: 0x04003D44 RID: 15684
		private static object EVENT_RECREATE_CHILDREN = new object();

		// Token: 0x04003D45 RID: 15685
		private GridEntry.GridEntryAccessibleObject accessibleObject;

		// Token: 0x04003D46 RID: 15686
		private GridEntry.EventEntry eventList;

		// Token: 0x020007A5 RID: 1957
		[Flags]
		internal enum PaintValueFlags
		{
			// Token: 0x04003D48 RID: 15688
			None = 0,
			// Token: 0x04003D49 RID: 15689
			DrawSelected = 1,
			// Token: 0x04003D4A RID: 15690
			FetchValue = 2,
			// Token: 0x04003D4B RID: 15691
			CheckShouldSerialize = 4,
			// Token: 0x04003D4C RID: 15692
			PaintInPlace = 8
		}

		// Token: 0x020007A6 RID: 1958
		private class CacheItems
		{
			// Token: 0x04003D4D RID: 15693
			public string lastLabel;

			// Token: 0x04003D4E RID: 15694
			public Font lastLabelFont;

			// Token: 0x04003D4F RID: 15695
			public int lastLabelWidth;

			// Token: 0x04003D50 RID: 15696
			public string lastValueString;

			// Token: 0x04003D51 RID: 15697
			public Font lastValueFont;

			// Token: 0x04003D52 RID: 15698
			public int lastValueTextWidth;

			// Token: 0x04003D53 RID: 15699
			public object lastValue;

			// Token: 0x04003D54 RID: 15700
			public bool useValueString;

			// Token: 0x04003D55 RID: 15701
			public bool lastShouldSerialize;

			// Token: 0x04003D56 RID: 15702
			public bool useShouldSerialize;

			// Token: 0x04003D57 RID: 15703
			public bool useCompatTextRendering;
		}

		// Token: 0x020007A7 RID: 1959
		private sealed class EventEntry
		{
			// Token: 0x06006804 RID: 26628 RVA: 0x0017D137 File Offset: 0x0017C137
			internal EventEntry(GridEntry.EventEntry next, object key, Delegate handler)
			{
				this.next = next;
				this.key = key;
				this.handler = handler;
			}

			// Token: 0x04003D58 RID: 15704
			internal GridEntry.EventEntry next;

			// Token: 0x04003D59 RID: 15705
			internal object key;

			// Token: 0x04003D5A RID: 15706
			internal Delegate handler;
		}

		// Token: 0x020007A8 RID: 1960
		[ComVisible(true)]
		public class GridEntryAccessibleObject : AccessibleObject
		{
			// Token: 0x06006805 RID: 26629 RVA: 0x0017D154 File Offset: 0x0017C154
			public GridEntryAccessibleObject(GridEntry owner)
			{
				this.owner = owner;
			}

			// Token: 0x1700160C RID: 5644
			// (get) Token: 0x06006806 RID: 26630 RVA: 0x0017D163 File Offset: 0x0017C163
			public override Rectangle Bounds
			{
				get
				{
					return this.PropertyGridView.AccessibilityGetGridEntryBounds(this.owner);
				}
			}

			// Token: 0x1700160D RID: 5645
			// (get) Token: 0x06006807 RID: 26631 RVA: 0x0017D176 File Offset: 0x0017C176
			public override string DefaultAction
			{
				get
				{
					if (!this.owner.Expandable)
					{
						return base.DefaultAction;
					}
					if (this.owner.Expanded)
					{
						return SR.GetString("AccessibleActionCollapse");
					}
					return SR.GetString("AccessibleActionExpand");
				}
			}

			// Token: 0x1700160E RID: 5646
			// (get) Token: 0x06006808 RID: 26632 RVA: 0x0017D1AE File Offset: 0x0017C1AE
			public override string Description
			{
				get
				{
					return this.owner.PropertyDescription;
				}
			}

			// Token: 0x06006809 RID: 26633 RVA: 0x0017D1BB File Offset: 0x0017C1BB
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				this.owner.OnOutlineClick(EventArgs.Empty);
			}

			// Token: 0x1700160F RID: 5647
			// (get) Token: 0x0600680A RID: 26634 RVA: 0x0017D1CD File Offset: 0x0017C1CD
			public override string Name
			{
				get
				{
					return this.owner.PropertyLabel;
				}
			}

			// Token: 0x17001610 RID: 5648
			// (get) Token: 0x0600680B RID: 26635 RVA: 0x0017D1DA File Offset: 0x0017C1DA
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.owner.GridEntryHost.AccessibilityObject;
				}
			}

			// Token: 0x17001611 RID: 5649
			// (get) Token: 0x0600680C RID: 26636 RVA: 0x0017D1EC File Offset: 0x0017C1EC
			private PropertyGridView PropertyGridView
			{
				get
				{
					return (PropertyGridView)((PropertyGridView.PropertyGridViewAccessibleObject)this.Parent).Owner;
				}
			}

			// Token: 0x17001612 RID: 5650
			// (get) Token: 0x0600680D RID: 26637 RVA: 0x0017D203 File Offset: 0x0017C203
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.Row;
				}
			}

			// Token: 0x17001613 RID: 5651
			// (get) Token: 0x0600680E RID: 26638 RVA: 0x0017D208 File Offset: 0x0017C208
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.Focusable | AccessibleStates.Selectable;
					if (this.owner.Focus)
					{
						accessibleStates |= AccessibleStates.Focused;
					}
					PropertyGridView.PropertyGridViewAccessibleObject propertyGridViewAccessibleObject = (PropertyGridView.PropertyGridViewAccessibleObject)this.Parent;
					if (propertyGridViewAccessibleObject.GetSelected() == this)
					{
						accessibleStates |= AccessibleStates.Selected;
					}
					if (this.owner.Expandable)
					{
						if (this.owner.Expanded)
						{
							accessibleStates |= AccessibleStates.Expanded;
						}
						else
						{
							accessibleStates |= AccessibleStates.Collapsed;
						}
					}
					if (this.owner.ShouldRenderReadOnly)
					{
						accessibleStates |= AccessibleStates.ReadOnly;
					}
					if (this.owner.ShouldRenderPassword)
					{
						accessibleStates |= AccessibleStates.Protected;
					}
					return accessibleStates;
				}
			}

			// Token: 0x17001614 RID: 5652
			// (get) Token: 0x0600680F RID: 26639 RVA: 0x0017D299 File Offset: 0x0017C299
			// (set) Token: 0x06006810 RID: 26640 RVA: 0x0017D2A6 File Offset: 0x0017C2A6
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.owner.GetPropertyTextValue();
				}
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				set
				{
					this.owner.SetPropertyTextValue(value);
				}
			}

			// Token: 0x06006811 RID: 26641 RVA: 0x0017D2B5 File Offset: 0x0017C2B5
			public override AccessibleObject GetFocused()
			{
				if (this.owner.Focus)
				{
					return this;
				}
				return null;
			}

			// Token: 0x06006812 RID: 26642 RVA: 0x0017D2C8 File Offset: 0x0017C2C8
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				PropertyGridView.PropertyGridViewAccessibleObject propertyGridViewAccessibleObject = (PropertyGridView.PropertyGridViewAccessibleObject)this.Parent;
				switch (navdir)
				{
				case AccessibleNavigation.Up:
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
					return propertyGridViewAccessibleObject.Previous(this.owner);
				case AccessibleNavigation.Down:
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
					return propertyGridViewAccessibleObject.Next(this.owner);
				}
				return null;
			}

			// Token: 0x06006813 RID: 26643 RVA: 0x0017D328 File Offset: 0x0017C328
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void Select(AccessibleSelection flags)
			{
				if (this.PropertyGridView.InvokeRequired)
				{
					this.PropertyGridView.Invoke(new GridEntry.GridEntryAccessibleObject.SelectDelegate(this.Select), new object[]
					{
						flags
					});
					return;
				}
				if ((flags & AccessibleSelection.TakeFocus) == AccessibleSelection.TakeFocus)
				{
					this.PropertyGridView.FocusInternal();
				}
				if ((flags & AccessibleSelection.TakeSelection) == AccessibleSelection.TakeSelection)
				{
					this.PropertyGridView.AccessibilitySelect(this.owner);
				}
			}

			// Token: 0x04003D5B RID: 15707
			private GridEntry owner;

			// Token: 0x020007A9 RID: 1961
			// (Invoke) Token: 0x06006815 RID: 26645
			private delegate void SelectDelegate(AccessibleSelection flags);
		}

		// Token: 0x020007AA RID: 1962
		public class DisplayNameSortComparer : IComparer
		{
			// Token: 0x06006818 RID: 26648 RVA: 0x0017D396 File Offset: 0x0017C396
			public int Compare(object left, object right)
			{
				return string.Compare(((PropertyDescriptor)left).DisplayName, ((PropertyDescriptor)right).DisplayName, true, CultureInfo.CurrentCulture);
			}
		}
	}
}

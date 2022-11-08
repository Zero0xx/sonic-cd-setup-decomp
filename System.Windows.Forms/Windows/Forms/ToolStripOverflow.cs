using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020006B8 RID: 1720
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class ToolStripOverflow : ToolStripDropDown, IArrangedElement, IComponent, IDisposable
	{
		// Token: 0x06005A22 RID: 23074 RVA: 0x0014777B File Offset: 0x0014677B
		public ToolStripOverflow(ToolStripItem parentItem) : base(parentItem)
		{
			if (parentItem == null)
			{
				throw new ArgumentNullException("parentItem");
			}
			this.ownerItem = (parentItem as ToolStripOverflowButton);
		}

		// Token: 0x170012B7 RID: 4791
		// (get) Token: 0x06005A23 RID: 23075 RVA: 0x001477A0 File Offset: 0x001467A0
		protected internal override ToolStripItemCollection DisplayedItems
		{
			get
			{
				if (this.ParentToolStrip != null)
				{
					return this.ParentToolStrip.OverflowItems;
				}
				return new ToolStripItemCollection(null, false);
			}
		}

		// Token: 0x170012B8 RID: 4792
		// (get) Token: 0x06005A24 RID: 23076 RVA: 0x001477CA File Offset: 0x001467CA
		public override ToolStripItemCollection Items
		{
			get
			{
				return new ToolStripItemCollection(null, false, true);
			}
		}

		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x06005A25 RID: 23077 RVA: 0x001477D4 File Offset: 0x001467D4
		private ToolStrip ParentToolStrip
		{
			get
			{
				if (this.ownerItem != null)
				{
					return this.ownerItem.ParentToolStrip;
				}
				return null;
			}
		}

		// Token: 0x170012BA RID: 4794
		// (get) Token: 0x06005A26 RID: 23078 RVA: 0x001477EB File Offset: 0x001467EB
		ArrangedElementCollection IArrangedElement.Children
		{
			get
			{
				return this.DisplayedItems;
			}
		}

		// Token: 0x170012BB RID: 4795
		// (get) Token: 0x06005A27 RID: 23079 RVA: 0x001477F3 File Offset: 0x001467F3
		IArrangedElement IArrangedElement.Container
		{
			get
			{
				return this.ParentInternal;
			}
		}

		// Token: 0x170012BC RID: 4796
		// (get) Token: 0x06005A28 RID: 23080 RVA: 0x001477FB File Offset: 0x001467FB
		bool IArrangedElement.ParticipatesInLayout
		{
			get
			{
				return base.GetState(2);
			}
		}

		// Token: 0x170012BD RID: 4797
		// (get) Token: 0x06005A29 RID: 23081 RVA: 0x00147804 File Offset: 0x00146804
		PropertyStore IArrangedElement.Properties
		{
			get
			{
				return base.Properties;
			}
		}

		// Token: 0x06005A2A RID: 23082 RVA: 0x0014780C File Offset: 0x0014680C
		void IArrangedElement.SetBounds(Rectangle bounds, BoundsSpecified specified)
		{
			this.SetBoundsCore(bounds.X, bounds.Y, bounds.Width, bounds.Height, specified);
		}

		// Token: 0x170012BE RID: 4798
		// (get) Token: 0x06005A2B RID: 23083 RVA: 0x00147831 File Offset: 0x00146831
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return FlowLayout.Instance;
			}
		}

		// Token: 0x06005A2C RID: 23084 RVA: 0x00147838 File Offset: 0x00146838
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripOverflow.ToolStripOverflowAccessibleObject(this);
		}

		// Token: 0x06005A2D RID: 23085 RVA: 0x00147840 File Offset: 0x00146840
		public override Size GetPreferredSize(Size constrainingSize)
		{
			constrainingSize.Width = 200;
			return base.GetPreferredSize(constrainingSize);
		}

		// Token: 0x06005A2E RID: 23086 RVA: 0x00147858 File Offset: 0x00146858
		protected override void OnLayout(LayoutEventArgs e)
		{
			if (this.ParentToolStrip != null && this.ParentToolStrip.IsInDesignMode)
			{
				if (FlowLayout.GetFlowDirection(this) != FlowDirection.TopDown)
				{
					FlowLayout.SetFlowDirection(this, FlowDirection.TopDown);
				}
				if (FlowLayout.GetWrapContents(this))
				{
					FlowLayout.SetWrapContents(this, false);
				}
			}
			else
			{
				if (FlowLayout.GetFlowDirection(this) != FlowDirection.LeftToRight)
				{
					FlowLayout.SetFlowDirection(this, FlowDirection.LeftToRight);
				}
				if (!FlowLayout.GetWrapContents(this))
				{
					FlowLayout.SetWrapContents(this, true);
				}
			}
			base.OnLayout(e);
		}

		// Token: 0x06005A2F RID: 23087 RVA: 0x001478C0 File Offset: 0x001468C0
		protected override void SetDisplayedItems()
		{
			Size size = Size.Empty;
			for (int i = 0; i < this.DisplayedItems.Count; i++)
			{
				ToolStripItem toolStripItem = this.DisplayedItems[i];
				if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
				{
					base.HasVisibleItems = true;
					size = LayoutUtils.UnionSizes(size, toolStripItem.Bounds.Size);
				}
			}
			base.SetLargestItemSize(size);
		}

		// Token: 0x040038B1 RID: 14513
		internal static readonly TraceSwitch PopupLayoutDebug;

		// Token: 0x040038B2 RID: 14514
		private ToolStripOverflowButton ownerItem;

		// Token: 0x020006B9 RID: 1721
		private class ToolStripOverflowAccessibleObject : ToolStrip.ToolStripAccessibleObject
		{
			// Token: 0x06005A30 RID: 23088 RVA: 0x00147921 File Offset: 0x00146921
			public ToolStripOverflowAccessibleObject(ToolStripOverflow owner) : base(owner)
			{
			}

			// Token: 0x06005A31 RID: 23089 RVA: 0x0014792A File Offset: 0x0014692A
			public override AccessibleObject GetChild(int index)
			{
				return ((ToolStripOverflow)base.Owner).DisplayedItems[index].AccessibilityObject;
			}

			// Token: 0x06005A32 RID: 23090 RVA: 0x00147947 File Offset: 0x00146947
			public override int GetChildCount()
			{
				return ((ToolStripOverflow)base.Owner).DisplayedItems.Count;
			}
		}
	}
}

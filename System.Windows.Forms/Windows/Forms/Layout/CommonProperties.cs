using System;
using System.Collections.Specialized;
using System.Drawing;

namespace System.Windows.Forms.Layout
{
	// Token: 0x02000785 RID: 1925
	internal class CommonProperties
	{
		// Token: 0x06006514 RID: 25876 RVA: 0x001718C4 File Offset: 0x001708C4
		internal static void ClearMaximumSize(IArrangedElement element)
		{
			if (element.Properties.ContainsObject(CommonProperties._maximumSizeProperty))
			{
				element.Properties.RemoveObject(CommonProperties._maximumSizeProperty);
			}
		}

		// Token: 0x06006515 RID: 25877 RVA: 0x001718E8 File Offset: 0x001708E8
		internal static bool GetAutoSize(IArrangedElement element)
		{
			int num = CommonProperties.GetLayoutState(element)[CommonProperties._autoSizeSection];
			return num != 0;
		}

		// Token: 0x06006516 RID: 25878 RVA: 0x00171910 File Offset: 0x00170910
		internal static Padding GetMargin(IArrangedElement element)
		{
			bool flag;
			Padding padding = element.Properties.GetPadding(CommonProperties._marginProperty, out flag);
			if (flag)
			{
				return padding;
			}
			return CommonProperties.DefaultMargin;
		}

		// Token: 0x06006517 RID: 25879 RVA: 0x0017193C File Offset: 0x0017093C
		internal static Size GetMaximumSize(IArrangedElement element, Size defaultMaximumSize)
		{
			bool flag;
			Size size = element.Properties.GetSize(CommonProperties._maximumSizeProperty, out flag);
			if (flag)
			{
				return size;
			}
			return defaultMaximumSize;
		}

		// Token: 0x06006518 RID: 25880 RVA: 0x00171964 File Offset: 0x00170964
		internal static Size GetMinimumSize(IArrangedElement element, Size defaultMinimumSize)
		{
			bool flag;
			Size size = element.Properties.GetSize(CommonProperties._minimumSizeProperty, out flag);
			if (flag)
			{
				return size;
			}
			return defaultMinimumSize;
		}

		// Token: 0x06006519 RID: 25881 RVA: 0x0017198C File Offset: 0x0017098C
		internal static Padding GetPadding(IArrangedElement element, Padding defaultPadding)
		{
			bool flag;
			Padding padding = element.Properties.GetPadding(CommonProperties._paddingProperty, out flag);
			if (flag)
			{
				return padding;
			}
			return defaultPadding;
		}

		// Token: 0x0600651A RID: 25882 RVA: 0x001719B4 File Offset: 0x001709B4
		internal static Rectangle GetSpecifiedBounds(IArrangedElement element)
		{
			bool flag;
			Rectangle rectangle = element.Properties.GetRectangle(CommonProperties._specifiedBoundsProperty, out flag);
			if (flag && rectangle != LayoutUtils.MaxRectangle)
			{
				return rectangle;
			}
			return element.Bounds;
		}

		// Token: 0x0600651B RID: 25883 RVA: 0x001719EC File Offset: 0x001709EC
		internal static void ResetPadding(IArrangedElement element)
		{
			object @object = element.Properties.GetObject(CommonProperties._paddingProperty);
			if (@object != null)
			{
				element.Properties.RemoveObject(CommonProperties._paddingProperty);
			}
		}

		// Token: 0x0600651C RID: 25884 RVA: 0x00171A20 File Offset: 0x00170A20
		internal static void SetAutoSize(IArrangedElement element, bool value)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			layoutState[CommonProperties._autoSizeSection] = (value ? 1 : 0);
			CommonProperties.SetLayoutState(element, layoutState);
			if (!value)
			{
				element.SetBounds(CommonProperties.GetSpecifiedBounds(element), BoundsSpecified.None);
			}
		}

		// Token: 0x0600651D RID: 25885 RVA: 0x00171A5E File Offset: 0x00170A5E
		internal static void SetMargin(IArrangedElement element, Padding value)
		{
			element.Properties.SetPadding(CommonProperties._marginProperty, value);
			LayoutTransaction.DoLayout(element.Container, element, PropertyNames.Margin);
		}

		// Token: 0x0600651E RID: 25886 RVA: 0x00171A84 File Offset: 0x00170A84
		internal static void SetMaximumSize(IArrangedElement element, Size value)
		{
			element.Properties.SetSize(CommonProperties._maximumSizeProperty, value);
			Rectangle bounds = element.Bounds;
			bounds.Width = Math.Min(bounds.Width, value.Width);
			bounds.Height = Math.Min(bounds.Height, value.Height);
			element.SetBounds(bounds, BoundsSpecified.Size);
			LayoutTransaction.DoLayout(element.Container, element, PropertyNames.MaximumSize);
		}

		// Token: 0x0600651F RID: 25887 RVA: 0x00171AF8 File Offset: 0x00170AF8
		internal static void SetMinimumSize(IArrangedElement element, Size value)
		{
			element.Properties.SetSize(CommonProperties._minimumSizeProperty, value);
			using (new LayoutTransaction(element.Container as Control, element, PropertyNames.MinimumSize))
			{
				Rectangle bounds = element.Bounds;
				bounds.Width = Math.Max(bounds.Width, value.Width);
				bounds.Height = Math.Max(bounds.Height, value.Height);
				element.SetBounds(bounds, BoundsSpecified.Size);
			}
		}

		// Token: 0x06006520 RID: 25888 RVA: 0x00171B90 File Offset: 0x00170B90
		internal static void SetPadding(IArrangedElement element, Padding value)
		{
			value = LayoutUtils.ClampNegativePaddingToZero(value);
			element.Properties.SetPadding(CommonProperties._paddingProperty, value);
		}

		// Token: 0x06006521 RID: 25889 RVA: 0x00171BAC File Offset: 0x00170BAC
		internal static void UpdateSpecifiedBounds(IArrangedElement element, int x, int y, int width, int height, BoundsSpecified specified)
		{
			Rectangle specifiedBounds = CommonProperties.GetSpecifiedBounds(element);
			bool flag = (specified & BoundsSpecified.X) == BoundsSpecified.None & x != specifiedBounds.X;
			bool flag2 = (specified & BoundsSpecified.Y) == BoundsSpecified.None & y != specifiedBounds.Y;
			bool flag3 = (specified & BoundsSpecified.Width) == BoundsSpecified.None & width != specifiedBounds.Width;
			bool flag4 = (specified & BoundsSpecified.Height) == BoundsSpecified.None & height != specifiedBounds.Height;
			if (flag || flag2 || flag3 || flag4)
			{
				if (!flag)
				{
					specifiedBounds.X = x;
				}
				if (!flag2)
				{
					specifiedBounds.Y = y;
				}
				if (!flag3)
				{
					specifiedBounds.Width = width;
				}
				if (!flag4)
				{
					specifiedBounds.Height = height;
				}
				element.Properties.SetRectangle(CommonProperties._specifiedBoundsProperty, specifiedBounds);
				return;
			}
			if (element.Properties.ContainsObject(CommonProperties._specifiedBoundsProperty))
			{
				element.Properties.SetRectangle(CommonProperties._specifiedBoundsProperty, LayoutUtils.MaxRectangle);
			}
		}

		// Token: 0x06006522 RID: 25890 RVA: 0x00171C8C File Offset: 0x00170C8C
		internal static void UpdateSpecifiedBounds(IArrangedElement element, int x, int y, int width, int height)
		{
			Rectangle value = new Rectangle(x, y, width, height);
			element.Properties.SetRectangle(CommonProperties._specifiedBoundsProperty, value);
		}

		// Token: 0x06006523 RID: 25891 RVA: 0x00171CB6 File Offset: 0x00170CB6
		internal static void xClearPreferredSizeCache(IArrangedElement element)
		{
			element.Properties.SetSize(CommonProperties._preferredSizeCacheProperty, LayoutUtils.InvalidSize);
		}

		// Token: 0x06006524 RID: 25892 RVA: 0x00171CD0 File Offset: 0x00170CD0
		internal static void xClearAllPreferredSizeCaches(IArrangedElement start)
		{
			CommonProperties.xClearPreferredSizeCache(start);
			ArrangedElementCollection children = start.Children;
			for (int i = 0; i < children.Count; i++)
			{
				CommonProperties.xClearAllPreferredSizeCaches(children[i]);
			}
		}

		// Token: 0x06006525 RID: 25893 RVA: 0x00171D08 File Offset: 0x00170D08
		internal static Size xGetPreferredSizeCache(IArrangedElement element)
		{
			bool flag;
			Size size = element.Properties.GetSize(CommonProperties._preferredSizeCacheProperty, out flag);
			if (flag && size != LayoutUtils.InvalidSize)
			{
				return size;
			}
			return Size.Empty;
		}

		// Token: 0x06006526 RID: 25894 RVA: 0x00171D3F File Offset: 0x00170D3F
		internal static void xSetPreferredSizeCache(IArrangedElement element, Size value)
		{
			element.Properties.SetSize(CommonProperties._preferredSizeCacheProperty, value);
		}

		// Token: 0x06006527 RID: 25895 RVA: 0x00171D54 File Offset: 0x00170D54
		internal static AutoSizeMode GetAutoSizeMode(IArrangedElement element)
		{
			if (CommonProperties.GetLayoutState(element)[CommonProperties._autoSizeModeSection] != 0)
			{
				return AutoSizeMode.GrowAndShrink;
			}
			return AutoSizeMode.GrowOnly;
		}

		// Token: 0x06006528 RID: 25896 RVA: 0x00171D7C File Offset: 0x00170D7C
		internal static bool GetNeedsDockAndAnchorLayout(IArrangedElement element)
		{
			return CommonProperties.GetLayoutState(element)[CommonProperties._dockAndAnchorNeedsLayoutSection] != 0;
		}

		// Token: 0x06006529 RID: 25897 RVA: 0x00171DA4 File Offset: 0x00170DA4
		internal static bool GetNeedsAnchorLayout(IArrangedElement element)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			return layoutState[CommonProperties._dockAndAnchorNeedsLayoutSection] != 0 && layoutState[CommonProperties._dockModeSection] == 0;
		}

		// Token: 0x0600652A RID: 25898 RVA: 0x00171DDC File Offset: 0x00170DDC
		internal static bool GetNeedsDockLayout(IArrangedElement element)
		{
			return CommonProperties.GetLayoutState(element)[CommonProperties._dockModeSection] == 1 && element.ParticipatesInLayout;
		}

		// Token: 0x0600652B RID: 25899 RVA: 0x00171E0C File Offset: 0x00170E0C
		internal static bool GetSelfAutoSizeInDefaultLayout(IArrangedElement element)
		{
			int num = CommonProperties.GetLayoutState(element)[CommonProperties._selfAutoSizingSection];
			return num == 1;
		}

		// Token: 0x0600652C RID: 25900 RVA: 0x00171E34 File Offset: 0x00170E34
		internal static void SetAutoSizeMode(IArrangedElement element, AutoSizeMode mode)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			layoutState[CommonProperties._autoSizeModeSection] = ((mode == AutoSizeMode.GrowAndShrink) ? 1 : 0);
			CommonProperties.SetLayoutState(element, layoutState);
		}

		// Token: 0x0600652D RID: 25901 RVA: 0x00171E62 File Offset: 0x00170E62
		internal static bool ShouldSelfSize(IArrangedElement element)
		{
			return !CommonProperties.GetAutoSize(element) || (element.Container is Control && ((Control)element.Container).LayoutEngine is DefaultLayout && CommonProperties.GetSelfAutoSizeInDefaultLayout(element));
		}

		// Token: 0x0600652E RID: 25902 RVA: 0x00171E9C File Offset: 0x00170E9C
		internal static void SetSelfAutoSizeInDefaultLayout(IArrangedElement element, bool value)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			layoutState[CommonProperties._selfAutoSizingSection] = (value ? 1 : 0);
			CommonProperties.SetLayoutState(element, layoutState);
		}

		// Token: 0x0600652F RID: 25903 RVA: 0x00171ECC File Offset: 0x00170ECC
		internal static AnchorStyles xGetAnchor(IArrangedElement element)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			AnchorStyles anchor = (AnchorStyles)layoutState[CommonProperties._dockAndAnchorSection];
			CommonProperties.DockAnchorMode dockAnchorMode = (CommonProperties.DockAnchorMode)layoutState[CommonProperties._dockModeSection];
			return (dockAnchorMode == CommonProperties.DockAnchorMode.Anchor) ? CommonProperties.xTranslateAnchorValue(anchor) : (AnchorStyles.Top | AnchorStyles.Left);
		}

		// Token: 0x06006530 RID: 25904 RVA: 0x00171F08 File Offset: 0x00170F08
		internal static bool xGetAutoSizedAndAnchored(IArrangedElement element)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			return layoutState[CommonProperties._selfAutoSizingSection] == 0 && layoutState[CommonProperties._autoSizeSection] != 0 && layoutState[CommonProperties._dockModeSection] == 0;
		}

		// Token: 0x06006531 RID: 25905 RVA: 0x00171F50 File Offset: 0x00170F50
		internal static DockStyle xGetDock(IArrangedElement element)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			DockStyle dockStyle = (DockStyle)layoutState[CommonProperties._dockAndAnchorSection];
			CommonProperties.DockAnchorMode dockAnchorMode = (CommonProperties.DockAnchorMode)layoutState[CommonProperties._dockModeSection];
			return (dockAnchorMode == CommonProperties.DockAnchorMode.Dock) ? dockStyle : DockStyle.None;
		}

		// Token: 0x06006532 RID: 25906 RVA: 0x00171F88 File Offset: 0x00170F88
		internal static void xSetAnchor(IArrangedElement element, AnchorStyles value)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			layoutState[CommonProperties._dockAndAnchorSection] = (int)CommonProperties.xTranslateAnchorValue(value);
			layoutState[CommonProperties._dockModeSection] = 0;
			CommonProperties.SetLayoutState(element, layoutState);
		}

		// Token: 0x06006533 RID: 25907 RVA: 0x00171FC4 File Offset: 0x00170FC4
		internal static void xSetDock(IArrangedElement element, DockStyle value)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			layoutState[CommonProperties._dockAndAnchorSection] = (int)value;
			layoutState[CommonProperties._dockModeSection] = ((value == DockStyle.None) ? 0 : 1);
			CommonProperties.SetLayoutState(element, layoutState);
		}

		// Token: 0x06006534 RID: 25908 RVA: 0x00172000 File Offset: 0x00171000
		private static AnchorStyles xTranslateAnchorValue(AnchorStyles anchor)
		{
			if (anchor == AnchorStyles.None)
			{
				return AnchorStyles.Top | AnchorStyles.Left;
			}
			if (anchor != (AnchorStyles.Top | AnchorStyles.Left))
			{
				return anchor;
			}
			return AnchorStyles.None;
		}

		// Token: 0x06006535 RID: 25909 RVA: 0x00172020 File Offset: 0x00171020
		internal static bool GetFlowBreak(IArrangedElement element)
		{
			int num = CommonProperties.GetLayoutState(element)[CommonProperties._flowBreakSection];
			return num == 1;
		}

		// Token: 0x06006536 RID: 25910 RVA: 0x00172048 File Offset: 0x00171048
		internal static void SetFlowBreak(IArrangedElement element, bool value)
		{
			BitVector32 layoutState = CommonProperties.GetLayoutState(element);
			layoutState[CommonProperties._flowBreakSection] = (value ? 1 : 0);
			CommonProperties.SetLayoutState(element, layoutState);
			LayoutTransaction.DoLayout(element.Container, element, PropertyNames.FlowBreak);
		}

		// Token: 0x06006537 RID: 25911 RVA: 0x00172088 File Offset: 0x00171088
		internal static Size GetLayoutBounds(IArrangedElement element)
		{
			bool flag;
			Size size = element.Properties.GetSize(CommonProperties._layoutBoundsProperty, out flag);
			if (flag)
			{
				return size;
			}
			return Size.Empty;
		}

		// Token: 0x06006538 RID: 25912 RVA: 0x001720B2 File Offset: 0x001710B2
		internal static void SetLayoutBounds(IArrangedElement element, Size value)
		{
			element.Properties.SetSize(CommonProperties._layoutBoundsProperty, value);
		}

		// Token: 0x06006539 RID: 25913 RVA: 0x001720C8 File Offset: 0x001710C8
		internal static bool HasLayoutBounds(IArrangedElement element)
		{
			bool result;
			element.Properties.GetSize(CommonProperties._layoutBoundsProperty, out result);
			return result;
		}

		// Token: 0x0600653A RID: 25914 RVA: 0x001720E9 File Offset: 0x001710E9
		internal static BitVector32 GetLayoutState(IArrangedElement element)
		{
			return new BitVector32(element.Properties.GetInteger(CommonProperties._layoutStateProperty));
		}

		// Token: 0x0600653B RID: 25915 RVA: 0x00172100 File Offset: 0x00171100
		internal static void SetLayoutState(IArrangedElement element, BitVector32 state)
		{
			element.Properties.SetInteger(CommonProperties._layoutStateProperty, state.Data);
		}

		// Token: 0x04003C12 RID: 15378
		internal const ContentAlignment DefaultAlignment = ContentAlignment.TopLeft;

		// Token: 0x04003C13 RID: 15379
		internal const AnchorStyles DefaultAnchor = AnchorStyles.Top | AnchorStyles.Left;

		// Token: 0x04003C14 RID: 15380
		internal const bool DefaultAutoSize = false;

		// Token: 0x04003C15 RID: 15381
		internal const DockStyle DefaultDock = DockStyle.None;

		// Token: 0x04003C16 RID: 15382
		private static readonly int _layoutStateProperty = PropertyStore.CreateKey();

		// Token: 0x04003C17 RID: 15383
		private static readonly int _specifiedBoundsProperty = PropertyStore.CreateKey();

		// Token: 0x04003C18 RID: 15384
		private static readonly int _preferredSizeCacheProperty = PropertyStore.CreateKey();

		// Token: 0x04003C19 RID: 15385
		private static readonly int _paddingProperty = PropertyStore.CreateKey();

		// Token: 0x04003C1A RID: 15386
		private static readonly int _marginProperty = PropertyStore.CreateKey();

		// Token: 0x04003C1B RID: 15387
		private static readonly int _minimumSizeProperty = PropertyStore.CreateKey();

		// Token: 0x04003C1C RID: 15388
		private static readonly int _maximumSizeProperty = PropertyStore.CreateKey();

		// Token: 0x04003C1D RID: 15389
		private static readonly int _layoutBoundsProperty = PropertyStore.CreateKey();

		// Token: 0x04003C1E RID: 15390
		internal static readonly Padding DefaultMargin = new Padding(3);

		// Token: 0x04003C1F RID: 15391
		internal static readonly Size DefaultMinimumSize = new Size(0, 0);

		// Token: 0x04003C20 RID: 15392
		internal static readonly Size DefaultMaximumSize = new Size(0, 0);

		// Token: 0x04003C21 RID: 15393
		private static readonly BitVector32.Section _dockAndAnchorNeedsLayoutSection = BitVector32.CreateSection(127);

		// Token: 0x04003C22 RID: 15394
		private static readonly BitVector32.Section _dockAndAnchorSection = BitVector32.CreateSection(15);

		// Token: 0x04003C23 RID: 15395
		private static readonly BitVector32.Section _dockModeSection = BitVector32.CreateSection(1, CommonProperties._dockAndAnchorSection);

		// Token: 0x04003C24 RID: 15396
		private static readonly BitVector32.Section _autoSizeSection = BitVector32.CreateSection(1, CommonProperties._dockModeSection);

		// Token: 0x04003C25 RID: 15397
		private static readonly BitVector32.Section _BoxStretchInternalSection = BitVector32.CreateSection(3, CommonProperties._autoSizeSection);

		// Token: 0x04003C26 RID: 15398
		private static readonly BitVector32.Section _anchorNeverShrinksSection = BitVector32.CreateSection(1, CommonProperties._BoxStretchInternalSection);

		// Token: 0x04003C27 RID: 15399
		private static readonly BitVector32.Section _flowBreakSection = BitVector32.CreateSection(1, CommonProperties._anchorNeverShrinksSection);

		// Token: 0x04003C28 RID: 15400
		private static readonly BitVector32.Section _selfAutoSizingSection = BitVector32.CreateSection(1, CommonProperties._flowBreakSection);

		// Token: 0x04003C29 RID: 15401
		private static readonly BitVector32.Section _autoSizeModeSection = BitVector32.CreateSection(1, CommonProperties._selfAutoSizingSection);

		// Token: 0x02000786 RID: 1926
		private enum DockAnchorMode
		{
			// Token: 0x04003C2B RID: 15403
			Anchor,
			// Token: 0x04003C2C RID: 15404
			Dock
		}
	}
}

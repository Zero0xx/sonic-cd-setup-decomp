using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Layout
{
	// Token: 0x02000787 RID: 1927
	internal class DefaultLayout : LayoutEngine
	{
		// Token: 0x0600653E RID: 25918 RVA: 0x0017222C File Offset: 0x0017122C
		private static void LayoutAutoSizedControls(IArrangedElement container)
		{
			ArrangedElementCollection children = container.Children;
			for (int i = children.Count - 1; i >= 0; i--)
			{
				IArrangedElement arrangedElement = children[i];
				if (CommonProperties.xGetAutoSizedAndAnchored(arrangedElement))
				{
					Rectangle cachedBounds = DefaultLayout.GetCachedBounds(arrangedElement);
					AnchorStyles anchor = DefaultLayout.GetAnchor(arrangedElement);
					Size maxSize = LayoutUtils.MaxSize;
					if ((anchor & (AnchorStyles.Left | AnchorStyles.Right)) == (AnchorStyles.Left | AnchorStyles.Right))
					{
						maxSize.Width = cachedBounds.Width;
					}
					if ((anchor & (AnchorStyles.Top | AnchorStyles.Bottom)) == (AnchorStyles.Top | AnchorStyles.Bottom))
					{
						maxSize.Height = cachedBounds.Height;
					}
					Size preferredSize = arrangedElement.GetPreferredSize(maxSize);
					Rectangle rectangle = cachedBounds;
					if (CommonProperties.GetAutoSizeMode(arrangedElement) == AutoSizeMode.GrowAndShrink)
					{
						rectangle = DefaultLayout.GetGrowthBounds(arrangedElement, preferredSize);
					}
					else if (cachedBounds.Width < preferredSize.Width || cachedBounds.Height < preferredSize.Height)
					{
						Size newSize = LayoutUtils.UnionSizes(cachedBounds.Size, preferredSize);
						rectangle = DefaultLayout.GetGrowthBounds(arrangedElement, newSize);
					}
					if (rectangle != cachedBounds)
					{
						DefaultLayout.SetCachedBounds(arrangedElement, rectangle);
					}
				}
			}
		}

		// Token: 0x0600653F RID: 25919 RVA: 0x0017231C File Offset: 0x0017131C
		private static Rectangle GetGrowthBounds(IArrangedElement element, Size newSize)
		{
			DefaultLayout.GrowthDirection growthDirection = DefaultLayout.GetGrowthDirection(element);
			Rectangle cachedBounds = DefaultLayout.GetCachedBounds(element);
			Point location = cachedBounds.Location;
			if ((growthDirection & DefaultLayout.GrowthDirection.Left) != DefaultLayout.GrowthDirection.None)
			{
				location.X -= newSize.Width - cachedBounds.Width;
			}
			if ((growthDirection & DefaultLayout.GrowthDirection.Upward) != DefaultLayout.GrowthDirection.None)
			{
				location.Y -= newSize.Height - cachedBounds.Height;
			}
			Rectangle result = new Rectangle(location, newSize);
			return result;
		}

		// Token: 0x06006540 RID: 25920 RVA: 0x00172390 File Offset: 0x00171390
		private static DefaultLayout.GrowthDirection GetGrowthDirection(IArrangedElement element)
		{
			AnchorStyles anchor = DefaultLayout.GetAnchor(element);
			DefaultLayout.GrowthDirection growthDirection = DefaultLayout.GrowthDirection.None;
			if ((anchor & AnchorStyles.Right) != AnchorStyles.None && (anchor & AnchorStyles.Left) == AnchorStyles.None)
			{
				growthDirection |= DefaultLayout.GrowthDirection.Left;
			}
			else
			{
				growthDirection |= DefaultLayout.GrowthDirection.Right;
			}
			if ((anchor & AnchorStyles.Bottom) != AnchorStyles.None && (anchor & AnchorStyles.Top) == AnchorStyles.None)
			{
				growthDirection |= DefaultLayout.GrowthDirection.Upward;
			}
			else
			{
				growthDirection |= DefaultLayout.GrowthDirection.Downward;
			}
			return growthDirection;
		}

		// Token: 0x06006541 RID: 25921 RVA: 0x001723D0 File Offset: 0x001713D0
		private static Rectangle GetAnchorDestination(IArrangedElement element, Rectangle displayRect, bool measureOnly)
		{
			DefaultLayout.AnchorInfo anchorInfo = DefaultLayout.GetAnchorInfo(element);
			int num = anchorInfo.Left + displayRect.X;
			int num2 = anchorInfo.Top + displayRect.Y;
			int num3 = anchorInfo.Right + displayRect.X;
			int num4 = anchorInfo.Bottom + displayRect.Y;
			AnchorStyles anchor = DefaultLayout.GetAnchor(element);
			if (DefaultLayout.IsAnchored(anchor, AnchorStyles.Right))
			{
				num3 += displayRect.Width;
				if (!DefaultLayout.IsAnchored(anchor, AnchorStyles.Left))
				{
					num += displayRect.Width;
				}
			}
			else if (!DefaultLayout.IsAnchored(anchor, AnchorStyles.Left))
			{
				num3 += displayRect.Width / 2;
				num += displayRect.Width / 2;
			}
			if (DefaultLayout.IsAnchored(anchor, AnchorStyles.Bottom))
			{
				num4 += displayRect.Height;
				if (!DefaultLayout.IsAnchored(anchor, AnchorStyles.Top))
				{
					num2 += displayRect.Height;
				}
			}
			else if (!DefaultLayout.IsAnchored(anchor, AnchorStyles.Top))
			{
				num4 += displayRect.Height / 2;
				num2 += displayRect.Height / 2;
			}
			if (!measureOnly)
			{
				if (num3 < num)
				{
					num3 = num;
				}
				if (num4 < num2)
				{
					num4 = num2;
				}
			}
			else
			{
				Rectangle cachedBounds = DefaultLayout.GetCachedBounds(element);
				if (num3 < num || cachedBounds.Width != element.Bounds.Width || cachedBounds.X != element.Bounds.X)
				{
					if (cachedBounds != element.Bounds)
					{
						num = Math.Max(Math.Abs(num), Math.Abs(cachedBounds.Left));
					}
					num3 = num + Math.Max(element.Bounds.Width, cachedBounds.Width) + Math.Abs(num3);
				}
				else
				{
					num = ((num > 0) ? num : element.Bounds.Left);
					num3 = ((num3 > 0) ? num3 : (element.Bounds.Right + Math.Abs(num3)));
				}
				if (num4 < num2 || cachedBounds.Height != element.Bounds.Height || cachedBounds.Y != element.Bounds.Y)
				{
					if (cachedBounds != element.Bounds)
					{
						num2 = Math.Max(Math.Abs(num2), Math.Abs(cachedBounds.Top));
					}
					num4 = num2 + Math.Max(element.Bounds.Height, cachedBounds.Height) + Math.Abs(num4);
				}
				else
				{
					num2 = ((num2 > 0) ? num2 : element.Bounds.Top);
					num4 = ((num4 > 0) ? num4 : (element.Bounds.Bottom + Math.Abs(num4)));
				}
			}
			return new Rectangle(num, num2, num3 - num, num4 - num2);
		}

		// Token: 0x06006542 RID: 25922 RVA: 0x0017266C File Offset: 0x0017166C
		private static void LayoutAnchoredControls(IArrangedElement container)
		{
			Rectangle displayRectangle = container.DisplayRectangle;
			if (CommonProperties.GetAutoSize(container) && (displayRectangle.Width == 0 || displayRectangle.Height == 0))
			{
				return;
			}
			ArrangedElementCollection children = container.Children;
			for (int i = children.Count - 1; i >= 0; i--)
			{
				IArrangedElement element = children[i];
				if (CommonProperties.GetNeedsAnchorLayout(element))
				{
					DefaultLayout.SetCachedBounds(element, DefaultLayout.GetAnchorDestination(element, displayRectangle, false));
				}
			}
		}

		// Token: 0x06006543 RID: 25923 RVA: 0x001726D4 File Offset: 0x001716D4
		private static Size LayoutDockedControls(IArrangedElement container, bool measureOnly)
		{
			Rectangle bounds = measureOnly ? Rectangle.Empty : container.DisplayRectangle;
			Size empty = Size.Empty;
			IArrangedElement arrangedElement = null;
			ArrangedElementCollection children = container.Children;
			for (int i = children.Count - 1; i >= 0; i--)
			{
				IArrangedElement arrangedElement2 = children[i];
				if (CommonProperties.GetNeedsDockLayout(arrangedElement2))
				{
					switch (DefaultLayout.GetDock(arrangedElement2))
					{
					case DockStyle.Top:
					{
						Size verticalDockedSize = DefaultLayout.GetVerticalDockedSize(arrangedElement2, bounds.Size, measureOnly);
						Rectangle newElementBounds = new Rectangle(bounds.X, bounds.Y, verticalDockedSize.Width, verticalDockedSize.Height);
						DefaultLayout.xLayoutDockedControl(arrangedElement2, newElementBounds, measureOnly, ref empty, ref bounds);
						bounds.Y += arrangedElement2.Bounds.Height;
						bounds.Height -= arrangedElement2.Bounds.Height;
						break;
					}
					case DockStyle.Bottom:
					{
						Size verticalDockedSize2 = DefaultLayout.GetVerticalDockedSize(arrangedElement2, bounds.Size, measureOnly);
						Rectangle newElementBounds2 = new Rectangle(bounds.X, bounds.Bottom - verticalDockedSize2.Height, verticalDockedSize2.Width, verticalDockedSize2.Height);
						DefaultLayout.xLayoutDockedControl(arrangedElement2, newElementBounds2, measureOnly, ref empty, ref bounds);
						bounds.Height -= arrangedElement2.Bounds.Height;
						break;
					}
					case DockStyle.Left:
					{
						Size horizontalDockedSize = DefaultLayout.GetHorizontalDockedSize(arrangedElement2, bounds.Size, measureOnly);
						Rectangle newElementBounds3 = new Rectangle(bounds.X, bounds.Y, horizontalDockedSize.Width, horizontalDockedSize.Height);
						DefaultLayout.xLayoutDockedControl(arrangedElement2, newElementBounds3, measureOnly, ref empty, ref bounds);
						bounds.X += arrangedElement2.Bounds.Width;
						bounds.Width -= arrangedElement2.Bounds.Width;
						break;
					}
					case DockStyle.Right:
					{
						Size horizontalDockedSize2 = DefaultLayout.GetHorizontalDockedSize(arrangedElement2, bounds.Size, measureOnly);
						Rectangle newElementBounds4 = new Rectangle(bounds.Right - horizontalDockedSize2.Width, bounds.Y, horizontalDockedSize2.Width, horizontalDockedSize2.Height);
						DefaultLayout.xLayoutDockedControl(arrangedElement2, newElementBounds4, measureOnly, ref empty, ref bounds);
						bounds.Width -= arrangedElement2.Bounds.Width;
						break;
					}
					case DockStyle.Fill:
						if (arrangedElement2 is MdiClient)
						{
							arrangedElement = arrangedElement2;
						}
						else
						{
							Size size = bounds.Size;
							Rectangle newElementBounds5 = new Rectangle(bounds.X, bounds.Y, size.Width, size.Height);
							DefaultLayout.xLayoutDockedControl(arrangedElement2, newElementBounds5, measureOnly, ref empty, ref bounds);
						}
						break;
					}
				}
				if (arrangedElement != null)
				{
					DefaultLayout.SetCachedBounds(arrangedElement, bounds);
				}
			}
			return empty;
		}

		// Token: 0x06006544 RID: 25924 RVA: 0x00172990 File Offset: 0x00171990
		private static void xLayoutDockedControl(IArrangedElement element, Rectangle newElementBounds, bool measureOnly, ref Size preferredSize, ref Rectangle remainingBounds)
		{
			if (measureOnly)
			{
				Size size = new Size(Math.Max(0, newElementBounds.Width - remainingBounds.Width), Math.Max(0, newElementBounds.Height - remainingBounds.Height));
				DockStyle dock = DefaultLayout.GetDock(element);
				if (dock == DockStyle.Top || dock == DockStyle.Bottom)
				{
					size.Width = 0;
				}
				if (dock == DockStyle.Left || dock == DockStyle.Right)
				{
					size.Height = 0;
				}
				if (dock != DockStyle.Fill)
				{
					preferredSize += size;
					remainingBounds.Size += size;
					return;
				}
				if (dock == DockStyle.Fill && CommonProperties.GetAutoSize(element))
				{
					Size preferredSize2 = element.GetPreferredSize(size);
					remainingBounds.Size += preferredSize2;
					preferredSize += preferredSize2;
					return;
				}
			}
			else
			{
				element.SetBounds(newElementBounds, BoundsSpecified.None);
			}
		}

		// Token: 0x06006545 RID: 25925 RVA: 0x00172A68 File Offset: 0x00171A68
		private static Size GetVerticalDockedSize(IArrangedElement element, Size remainingSize, bool measureOnly)
		{
			Size result = DefaultLayout.xGetDockedSize(element, remainingSize, new Size(remainingSize.Width, 1), measureOnly);
			if (!measureOnly)
			{
				result.Width = remainingSize.Width;
			}
			else
			{
				result.Width = Math.Max(result.Width, remainingSize.Width);
			}
			return result;
		}

		// Token: 0x06006546 RID: 25926 RVA: 0x00172ABC File Offset: 0x00171ABC
		private static Size GetHorizontalDockedSize(IArrangedElement element, Size remainingSize, bool measureOnly)
		{
			Size result = DefaultLayout.xGetDockedSize(element, remainingSize, new Size(1, remainingSize.Height), measureOnly);
			if (!measureOnly)
			{
				result.Height = remainingSize.Height;
			}
			else
			{
				result.Height = Math.Max(result.Height, remainingSize.Height);
			}
			return result;
		}

		// Token: 0x06006547 RID: 25927 RVA: 0x00172B10 File Offset: 0x00171B10
		private static Size xGetDockedSize(IArrangedElement element, Size remainingSize, Size constraints, bool measureOnly)
		{
			Size result;
			if (CommonProperties.GetAutoSize(element))
			{
				result = element.GetPreferredSize(constraints);
			}
			else
			{
				result = element.Bounds.Size;
			}
			return result;
		}

		// Token: 0x06006548 RID: 25928 RVA: 0x00172B40 File Offset: 0x00171B40
		internal override bool LayoutCore(IArrangedElement container, LayoutEventArgs args)
		{
			Size size;
			return DefaultLayout.xLayout(container, false, out size);
		}

		// Token: 0x06006549 RID: 25929 RVA: 0x00172B58 File Offset: 0x00171B58
		private static bool xLayout(IArrangedElement container, bool measureOnly, out Size preferredSize)
		{
			ArrangedElementCollection children = container.Children;
			preferredSize = new Size(-7103, -7105);
			if (!measureOnly && children.Count == 0)
			{
				return CommonProperties.GetAutoSize(container);
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int i = children.Count - 1; i >= 0; i--)
			{
				IArrangedElement element = children[i];
				if (CommonProperties.GetNeedsDockAndAnchorLayout(element))
				{
					if (!flag && CommonProperties.GetNeedsDockLayout(element))
					{
						flag = true;
					}
					if (!flag2 && CommonProperties.GetNeedsAnchorLayout(element))
					{
						flag2 = true;
					}
					if (!flag3 && CommonProperties.xGetAutoSizedAndAnchored(element))
					{
						flag3 = true;
					}
				}
			}
			Size a = Size.Empty;
			Size b = Size.Empty;
			if (flag)
			{
				a = DefaultLayout.LayoutDockedControls(container, measureOnly);
			}
			if (flag2 && !measureOnly)
			{
				DefaultLayout.LayoutAnchoredControls(container);
			}
			if (flag3)
			{
				DefaultLayout.LayoutAutoSizedControls(container);
			}
			if (!measureOnly)
			{
				DefaultLayout.ApplyCachedBounds(container);
			}
			else
			{
				b = DefaultLayout.GetAnchorPreferredSize(container);
				Padding padding = Padding.Empty;
				Control control = container as Control;
				if (control != null)
				{
					padding = control.Padding;
				}
				else
				{
					padding = CommonProperties.GetPadding(container, Padding.Empty);
				}
				b.Width -= padding.Left;
				b.Height -= padding.Top;
				DefaultLayout.ClearCachedBounds(container);
				preferredSize = LayoutUtils.UnionSizes(a, b);
			}
			return CommonProperties.GetAutoSize(container);
		}

		// Token: 0x0600654A RID: 25930 RVA: 0x00172CA0 File Offset: 0x00171CA0
		private static void UpdateAnchorInfo(IArrangedElement element)
		{
			DefaultLayout.AnchorInfo anchorInfo = DefaultLayout.GetAnchorInfo(element);
			if (anchorInfo == null)
			{
				anchorInfo = new DefaultLayout.AnchorInfo();
				DefaultLayout.SetAnchorInfo(element, anchorInfo);
			}
			if (CommonProperties.GetNeedsAnchorLayout(element) && element.Container != null)
			{
				DefaultLayout.GetCachedBounds(element);
				anchorInfo.Left = element.Bounds.Left;
				anchorInfo.Top = element.Bounds.Top;
				anchorInfo.Right = element.Bounds.Right;
				anchorInfo.Bottom = element.Bounds.Bottom;
				Rectangle displayRectangle = element.Container.DisplayRectangle;
				int width = displayRectangle.Width;
				int height = displayRectangle.Height;
				anchorInfo.Left -= displayRectangle.X;
				anchorInfo.Top -= displayRectangle.Y;
				anchorInfo.Right -= displayRectangle.X;
				anchorInfo.Bottom -= displayRectangle.Y;
				AnchorStyles anchor = DefaultLayout.GetAnchor(element);
				if (DefaultLayout.IsAnchored(anchor, AnchorStyles.Right))
				{
					anchorInfo.Right -= width;
					if (!DefaultLayout.IsAnchored(anchor, AnchorStyles.Left))
					{
						anchorInfo.Left -= width;
					}
				}
				else if (!DefaultLayout.IsAnchored(anchor, AnchorStyles.Left))
				{
					anchorInfo.Right -= width / 2;
					anchorInfo.Left -= width / 2;
				}
				if (DefaultLayout.IsAnchored(anchor, AnchorStyles.Bottom))
				{
					anchorInfo.Bottom -= height;
					if (!DefaultLayout.IsAnchored(anchor, AnchorStyles.Top))
					{
						anchorInfo.Top -= height;
						return;
					}
				}
				else if (!DefaultLayout.IsAnchored(anchor, AnchorStyles.Top))
				{
					anchorInfo.Bottom -= height / 2;
					anchorInfo.Top -= height / 2;
				}
			}
		}

		// Token: 0x0600654B RID: 25931 RVA: 0x00172E60 File Offset: 0x00171E60
		public static AnchorStyles GetAnchor(IArrangedElement element)
		{
			return CommonProperties.xGetAnchor(element);
		}

		// Token: 0x0600654C RID: 25932 RVA: 0x00172E68 File Offset: 0x00171E68
		public static void SetAnchor(IArrangedElement container, IArrangedElement element, AnchorStyles value)
		{
			AnchorStyles anchor = DefaultLayout.GetAnchor(element);
			if (anchor != value)
			{
				if (CommonProperties.GetNeedsDockLayout(element))
				{
					DefaultLayout.SetDock(element, DockStyle.None);
				}
				CommonProperties.xSetAnchor(element, value);
				if (CommonProperties.GetNeedsAnchorLayout(element))
				{
					DefaultLayout.UpdateAnchorInfo(element);
				}
				else
				{
					DefaultLayout.SetAnchorInfo(element, null);
				}
				if (element.Container != null)
				{
					bool flag = DefaultLayout.IsAnchored(anchor, AnchorStyles.Right) && !DefaultLayout.IsAnchored(value, AnchorStyles.Right);
					bool flag2 = DefaultLayout.IsAnchored(anchor, AnchorStyles.Bottom) && !DefaultLayout.IsAnchored(value, AnchorStyles.Bottom);
					if (element.Container.Container != null && (flag || flag2))
					{
						LayoutTransaction.DoLayout(element.Container.Container, element, PropertyNames.Anchor);
					}
					LayoutTransaction.DoLayout(element.Container, element, PropertyNames.Anchor);
				}
			}
		}

		// Token: 0x0600654D RID: 25933 RVA: 0x00172F20 File Offset: 0x00171F20
		public static DockStyle GetDock(IArrangedElement element)
		{
			return CommonProperties.xGetDock(element);
		}

		// Token: 0x0600654E RID: 25934 RVA: 0x00172F28 File Offset: 0x00171F28
		public static void SetDock(IArrangedElement element, DockStyle value)
		{
			if (DefaultLayout.GetDock(element) != value)
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 5))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DockStyle));
				}
				bool needsDockLayout = CommonProperties.GetNeedsDockLayout(element);
				CommonProperties.xSetDock(element, value);
				using (new LayoutTransaction(element.Container as Control, element, PropertyNames.Dock))
				{
					if (value == DockStyle.None)
					{
						if (needsDockLayout)
						{
							element.SetBounds(CommonProperties.GetSpecifiedBounds(element), BoundsSpecified.None);
							DefaultLayout.UpdateAnchorInfo(element);
						}
					}
					else
					{
						element.SetBounds(CommonProperties.GetSpecifiedBounds(element), BoundsSpecified.All);
					}
				}
			}
		}

		// Token: 0x0600654F RID: 25935 RVA: 0x00172FD4 File Offset: 0x00171FD4
		private static Rectangle GetCachedBounds(IArrangedElement element)
		{
			if (element.Container != null)
			{
				IDictionary dictionary = (IDictionary)element.Container.Properties.GetObject(DefaultLayout._cachedBoundsProperty);
				if (dictionary != null)
				{
					object obj = dictionary[element];
					if (obj != null)
					{
						return (Rectangle)obj;
					}
				}
			}
			return element.Bounds;
		}

		// Token: 0x06006550 RID: 25936 RVA: 0x0017301F File Offset: 0x0017201F
		private static bool HasCachedBounds(IArrangedElement container)
		{
			return container != null && container.Properties.GetObject(DefaultLayout._cachedBoundsProperty) != null;
		}

		// Token: 0x06006551 RID: 25937 RVA: 0x0017303C File Offset: 0x0017203C
		private static void ApplyCachedBounds(IArrangedElement container)
		{
			if (CommonProperties.GetAutoSize(container))
			{
				Rectangle displayRectangle = container.DisplayRectangle;
				if (displayRectangle.Width == 0 || displayRectangle.Height == 0)
				{
					DefaultLayout.ClearCachedBounds(container);
					return;
				}
			}
			IDictionary dictionary = (IDictionary)container.Properties.GetObject(DefaultLayout._cachedBoundsProperty);
			if (dictionary != null)
			{
				foreach (object obj in dictionary)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					IArrangedElement arrangedElement = (IArrangedElement)dictionaryEntry.Key;
					Rectangle bounds = (Rectangle)dictionaryEntry.Value;
					arrangedElement.SetBounds(bounds, BoundsSpecified.None);
				}
				DefaultLayout.ClearCachedBounds(container);
			}
		}

		// Token: 0x06006552 RID: 25938 RVA: 0x001730FC File Offset: 0x001720FC
		private static void ClearCachedBounds(IArrangedElement container)
		{
			container.Properties.SetObject(DefaultLayout._cachedBoundsProperty, null);
		}

		// Token: 0x06006553 RID: 25939 RVA: 0x00173110 File Offset: 0x00172110
		private static void SetCachedBounds(IArrangedElement element, Rectangle bounds)
		{
			if (bounds != DefaultLayout.GetCachedBounds(element))
			{
				IDictionary dictionary = (IDictionary)element.Container.Properties.GetObject(DefaultLayout._cachedBoundsProperty);
				if (dictionary == null)
				{
					dictionary = new HybridDictionary();
					element.Container.Properties.SetObject(DefaultLayout._cachedBoundsProperty, dictionary);
				}
				dictionary[element] = bounds;
			}
		}

		// Token: 0x06006554 RID: 25940 RVA: 0x00173172 File Offset: 0x00172172
		private static DefaultLayout.AnchorInfo GetAnchorInfo(IArrangedElement element)
		{
			return (DefaultLayout.AnchorInfo)element.Properties.GetObject(DefaultLayout._layoutInfoProperty);
		}

		// Token: 0x06006555 RID: 25941 RVA: 0x00173189 File Offset: 0x00172189
		private static void SetAnchorInfo(IArrangedElement element, DefaultLayout.AnchorInfo value)
		{
			element.Properties.SetObject(DefaultLayout._layoutInfoProperty, value);
		}

		// Token: 0x06006556 RID: 25942 RVA: 0x0017319C File Offset: 0x0017219C
		internal override void InitLayoutCore(IArrangedElement element, BoundsSpecified specified)
		{
			if (specified != BoundsSpecified.None && CommonProperties.GetNeedsAnchorLayout(element))
			{
				DefaultLayout.UpdateAnchorInfo(element);
			}
		}

		// Token: 0x06006557 RID: 25943 RVA: 0x001731B0 File Offset: 0x001721B0
		internal override Size GetPreferredSize(IArrangedElement container, Size proposedBounds)
		{
			Size result;
			DefaultLayout.xLayout(container, true, out result);
			return result;
		}

		// Token: 0x06006558 RID: 25944 RVA: 0x001731C8 File Offset: 0x001721C8
		private static Size GetAnchorPreferredSize(IArrangedElement container)
		{
			Size empty = Size.Empty;
			ArrangedElementCollection children = container.Children;
			for (int i = children.Count - 1; i >= 0; i--)
			{
				IArrangedElement arrangedElement = container.Children[i];
				if (!CommonProperties.GetNeedsDockLayout(arrangedElement) && arrangedElement.ParticipatesInLayout)
				{
					AnchorStyles anchor = DefaultLayout.GetAnchor(arrangedElement);
					Padding margin = CommonProperties.GetMargin(arrangedElement);
					Rectangle rectangle = LayoutUtils.InflateRect(DefaultLayout.GetCachedBounds(arrangedElement), margin);
					if (DefaultLayout.IsAnchored(anchor, AnchorStyles.Left) && !DefaultLayout.IsAnchored(anchor, AnchorStyles.Right))
					{
						empty.Width = Math.Max(empty.Width, rectangle.Right);
					}
					if (!DefaultLayout.IsAnchored(anchor, AnchorStyles.Bottom))
					{
						empty.Height = Math.Max(empty.Height, rectangle.Bottom);
					}
					if (DefaultLayout.IsAnchored(anchor, AnchorStyles.Right))
					{
						Rectangle anchorDestination = DefaultLayout.GetAnchorDestination(arrangedElement, Rectangle.Empty, true);
						if (anchorDestination.Width < 0)
						{
							empty.Width = Math.Max(empty.Width, rectangle.Right + anchorDestination.Width);
						}
						else
						{
							empty.Width = Math.Max(empty.Width, anchorDestination.Right);
						}
					}
					if (DefaultLayout.IsAnchored(anchor, AnchorStyles.Bottom))
					{
						Rectangle anchorDestination2 = DefaultLayout.GetAnchorDestination(arrangedElement, Rectangle.Empty, true);
						if (anchorDestination2.Height < 0)
						{
							empty.Height = Math.Max(empty.Height, rectangle.Bottom + anchorDestination2.Height);
						}
						else
						{
							empty.Height = Math.Max(empty.Height, anchorDestination2.Bottom);
						}
					}
				}
			}
			return empty;
		}

		// Token: 0x06006559 RID: 25945 RVA: 0x00173350 File Offset: 0x00172350
		public static bool IsAnchored(AnchorStyles anchor, AnchorStyles desiredAnchor)
		{
			return (anchor & desiredAnchor) == desiredAnchor;
		}

		// Token: 0x04003C2D RID: 15405
		internal static readonly DefaultLayout Instance = new DefaultLayout();

		// Token: 0x04003C2E RID: 15406
		private static readonly int _layoutInfoProperty = PropertyStore.CreateKey();

		// Token: 0x04003C2F RID: 15407
		private static readonly int _cachedBoundsProperty = PropertyStore.CreateKey();

		// Token: 0x02000788 RID: 1928
		[Flags]
		private enum GrowthDirection
		{
			// Token: 0x04003C31 RID: 15409
			None = 0,
			// Token: 0x04003C32 RID: 15410
			Upward = 1,
			// Token: 0x04003C33 RID: 15411
			Downward = 2,
			// Token: 0x04003C34 RID: 15412
			Left = 4,
			// Token: 0x04003C35 RID: 15413
			Right = 8
		}

		// Token: 0x02000789 RID: 1929
		private sealed class AnchorInfo
		{
			// Token: 0x04003C36 RID: 15414
			public int Left;

			// Token: 0x04003C37 RID: 15415
			public int Top;

			// Token: 0x04003C38 RID: 15416
			public int Right;

			// Token: 0x04003C39 RID: 15417
			public int Bottom;
		}
	}
}

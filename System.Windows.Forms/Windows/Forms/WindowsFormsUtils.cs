using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms
{
	// Token: 0x02000617 RID: 1559
	internal sealed class WindowsFormsUtils
	{
		// Token: 0x17001074 RID: 4212
		// (get) Token: 0x06005185 RID: 20869 RVA: 0x0012BB7C File Offset: 0x0012AB7C
		public static Point LastCursorPoint
		{
			get
			{
				int messagePos = SafeNativeMethods.GetMessagePos();
				return new Point(NativeMethods.Util.SignedLOWORD(messagePos), NativeMethods.Util.SignedHIWORD(messagePos));
			}
		}

		// Token: 0x06005186 RID: 20870 RVA: 0x0012BBA0 File Offset: 0x0012ABA0
		public static Graphics CreateMeasurementGraphics()
		{
			return Graphics.FromHdcInternal(WindowsGraphicsCacheManager.MeasurementGraphics.DeviceContext.Hdc);
		}

		// Token: 0x06005187 RID: 20871 RVA: 0x0012BBB8 File Offset: 0x0012ABB8
		public static bool ContainsMnemonic(string text)
		{
			if (text != null)
			{
				int length = text.Length;
				int num = text.IndexOf('&', 0);
				if (num >= 0 && num <= length - 2)
				{
					int num2 = text.IndexOf('&', num + 1);
					if (num2 == -1)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005188 RID: 20872 RVA: 0x0012BBF6 File Offset: 0x0012ABF6
		internal static Rectangle ConstrainToScreenWorkingAreaBounds(Rectangle bounds)
		{
			return WindowsFormsUtils.ConstrainToBounds(Screen.GetWorkingArea(bounds), bounds);
		}

		// Token: 0x06005189 RID: 20873 RVA: 0x0012BC04 File Offset: 0x0012AC04
		internal static Rectangle ConstrainToScreenBounds(Rectangle bounds)
		{
			return WindowsFormsUtils.ConstrainToBounds(Screen.FromRectangle(bounds).Bounds, bounds);
		}

		// Token: 0x0600518A RID: 20874 RVA: 0x0012BC18 File Offset: 0x0012AC18
		internal static Rectangle ConstrainToBounds(Rectangle constrainingBounds, Rectangle bounds)
		{
			if (!constrainingBounds.Contains(bounds))
			{
				bounds.Size = new Size(Math.Min(constrainingBounds.Width - 2, bounds.Width), Math.Min(constrainingBounds.Height - 2, bounds.Height));
				if (bounds.Right > constrainingBounds.Right)
				{
					bounds.X = constrainingBounds.Right - bounds.Width;
				}
				else if (bounds.Left < constrainingBounds.Left)
				{
					bounds.X = constrainingBounds.Left;
				}
				if (bounds.Bottom > constrainingBounds.Bottom)
				{
					bounds.Y = constrainingBounds.Bottom - 1 - bounds.Height;
				}
				else if (bounds.Top < constrainingBounds.Top)
				{
					bounds.Y = constrainingBounds.Top;
				}
			}
			return bounds;
		}

		// Token: 0x0600518B RID: 20875 RVA: 0x0012BCF8 File Offset: 0x0012ACF8
		internal static string EscapeTextWithAmpersands(string text)
		{
			if (text == null)
			{
				return null;
			}
			int i = text.IndexOf('&');
			if (i == -1)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(text.Substring(0, i));
			while (i < text.Length)
			{
				if (text[i] == '&')
				{
					stringBuilder.Append("&");
				}
				if (i < text.Length)
				{
					stringBuilder.Append(text[i]);
				}
				i++;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600518C RID: 20876 RVA: 0x0012BD6C File Offset: 0x0012AD6C
		internal static string GetControlInformation(IntPtr hwnd)
		{
			if (hwnd == IntPtr.Zero)
			{
				return "Handle is IntPtr.Zero";
			}
			return "";
		}

		// Token: 0x0600518D RID: 20877 RVA: 0x0012BD93 File Offset: 0x0012AD93
		internal static string AssertControlInformation(bool condition, Control control)
		{
			if (condition)
			{
				return string.Empty;
			}
			return WindowsFormsUtils.GetControlInformation(control.Handle);
		}

		// Token: 0x0600518E RID: 20878 RVA: 0x0012BDAC File Offset: 0x0012ADAC
		internal static int GetCombinedHashCodes(params int[] args)
		{
			int num = -757577119;
			for (int i = 0; i < args.Length; i++)
			{
				num = (args[i] ^ num) * -1640531535;
			}
			return num;
		}

		// Token: 0x0600518F RID: 20879 RVA: 0x0012BDDC File Offset: 0x0012ADDC
		public static char GetMnemonic(string text, bool bConvertToUpperCase)
		{
			char result = '\0';
			if (text != null)
			{
				int length = text.Length;
				for (int i = 0; i < length - 1; i++)
				{
					if (text[i] == '&')
					{
						if (text[i + 1] == '&')
						{
							i++;
						}
						else
						{
							if (bConvertToUpperCase)
							{
								result = char.ToUpper(text[i + 1], CultureInfo.CurrentCulture);
								break;
							}
							result = char.ToLower(text[i + 1], CultureInfo.CurrentCulture);
							break;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06005190 RID: 20880 RVA: 0x0012BE54 File Offset: 0x0012AE54
		public static HandleRef GetRootHWnd(HandleRef hwnd)
		{
			IntPtr ancestor = UnsafeNativeMethods.GetAncestor(new HandleRef(hwnd, hwnd.Handle), 2);
			return new HandleRef(hwnd.Wrapper, ancestor);
		}

		// Token: 0x06005191 RID: 20881 RVA: 0x0012BE87 File Offset: 0x0012AE87
		public static HandleRef GetRootHWnd(Control control)
		{
			return WindowsFormsUtils.GetRootHWnd(new HandleRef(control, control.Handle));
		}

		// Token: 0x06005192 RID: 20882 RVA: 0x0012BE9C File Offset: 0x0012AE9C
		public static string TextWithoutMnemonics(string text)
		{
			if (text == null)
			{
				return null;
			}
			int i = text.IndexOf('&');
			if (i == -1)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(text.Substring(0, i));
			while (i < text.Length)
			{
				if (text[i] == '&')
				{
					i++;
				}
				if (i < text.Length)
				{
					stringBuilder.Append(text[i]);
				}
				i++;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005193 RID: 20883 RVA: 0x0012BF08 File Offset: 0x0012AF08
		public static Point TranslatePoint(Point point, Control fromControl, Control toControl)
		{
			NativeMethods.POINT point2 = new NativeMethods.POINT(point.X, point.Y);
			UnsafeNativeMethods.MapWindowPoints(new HandleRef(fromControl, fromControl.Handle), new HandleRef(toControl, toControl.Handle), point2, 1);
			return new Point(point2.x, point2.y);
		}

		// Token: 0x06005194 RID: 20884 RVA: 0x0012BF5A File Offset: 0x0012AF5A
		public static bool SafeCompareStrings(string string1, string string2, bool ignoreCase)
		{
			return string1 != null && string2 != null && string1.Length == string2.Length && string.Compare(string1, string2, ignoreCase, CultureInfo.InvariantCulture) == 0;
		}

		// Token: 0x06005195 RID: 20885 RVA: 0x0012BF84 File Offset: 0x0012AF84
		public static int RotateLeft(int value, int nBits)
		{
			nBits %= 32;
			return value << nBits | value >> 32 - nBits;
		}

		// Token: 0x06005196 RID: 20886 RVA: 0x0012BF9C File Offset: 0x0012AF9C
		public static string GetComponentName(IComponent component, string defaultNameValue)
		{
			string text = string.Empty;
			if (string.IsNullOrEmpty(defaultNameValue))
			{
				if (component.Site != null)
				{
					text = component.Site.Name;
				}
				if (text == null)
				{
					text = string.Empty;
				}
			}
			else
			{
				text = defaultNameValue;
			}
			return text;
		}

		// Token: 0x040035F9 RID: 13817
		public static readonly Size UninitializedSize = new Size(-7199369, -5999471);

		// Token: 0x040035FA RID: 13818
		public static readonly ContentAlignment AnyRightAlign = (ContentAlignment)1092;

		// Token: 0x040035FB RID: 13819
		public static readonly ContentAlignment AnyLeftAlign = (ContentAlignment)273;

		// Token: 0x040035FC RID: 13820
		public static readonly ContentAlignment AnyTopAlign = (ContentAlignment)7;

		// Token: 0x040035FD RID: 13821
		public static readonly ContentAlignment AnyBottomAlign = (ContentAlignment)1792;

		// Token: 0x040035FE RID: 13822
		public static readonly ContentAlignment AnyMiddleAlign = (ContentAlignment)112;

		// Token: 0x040035FF RID: 13823
		public static readonly ContentAlignment AnyCenterAlign = (ContentAlignment)546;

		// Token: 0x02000618 RID: 1560
		public static class EnumValidator
		{
			// Token: 0x06005199 RID: 20889 RVA: 0x0012C03C File Offset: 0x0012B03C
			public static bool IsValidContentAlignment(ContentAlignment contentAlign)
			{
				if (ClientUtils.GetBitCount((uint)contentAlign) != 1)
				{
					return false;
				}
				int num = 1911;
				return (num & (int)contentAlign) != 0;
			}

			// Token: 0x0600519A RID: 20890 RVA: 0x0012C064 File Offset: 0x0012B064
			public static bool IsEnumWithinShiftedRange(Enum enumValue, int numBitsToShift, int minValAfterShift, int maxValAfterShift)
			{
				int num = Convert.ToInt32(enumValue, CultureInfo.InvariantCulture);
				int num2 = num >> numBitsToShift;
				return num2 << numBitsToShift == num && num2 >= minValAfterShift && num2 <= maxValAfterShift;
			}

			// Token: 0x0600519B RID: 20891 RVA: 0x0012C09C File Offset: 0x0012B09C
			public static bool IsValidTextImageRelation(TextImageRelation relation)
			{
				return ClientUtils.IsEnumValid(relation, (int)relation, 0, 8, 1);
			}

			// Token: 0x0600519C RID: 20892 RVA: 0x0012C0B0 File Offset: 0x0012B0B0
			public static bool IsValidArrowDirection(ArrowDirection direction)
			{
				switch (direction)
				{
				case ArrowDirection.Left:
				case ArrowDirection.Up:
					break;
				default:
					switch (direction)
					{
					case ArrowDirection.Right:
					case ArrowDirection.Down:
						break;
					default:
						return false;
					}
					break;
				}
				return true;
			}
		}

		// Token: 0x02000619 RID: 1561
		public class ArraySubsetEnumerator : IEnumerator
		{
			// Token: 0x0600519D RID: 20893 RVA: 0x0012C0E3 File Offset: 0x0012B0E3
			public ArraySubsetEnumerator(object[] array, int count)
			{
				this.array = array;
				this.total = count;
				this.current = -1;
			}

			// Token: 0x0600519E RID: 20894 RVA: 0x0012C100 File Offset: 0x0012B100
			public bool MoveNext()
			{
				if (this.current < this.total - 1)
				{
					this.current++;
					return true;
				}
				return false;
			}

			// Token: 0x0600519F RID: 20895 RVA: 0x0012C123 File Offset: 0x0012B123
			public void Reset()
			{
				this.current = -1;
			}

			// Token: 0x17001075 RID: 4213
			// (get) Token: 0x060051A0 RID: 20896 RVA: 0x0012C12C File Offset: 0x0012B12C
			public object Current
			{
				get
				{
					if (this.current == -1)
					{
						return null;
					}
					return this.array[this.current];
				}
			}

			// Token: 0x04003600 RID: 13824
			private object[] array;

			// Token: 0x04003601 RID: 13825
			private int total;

			// Token: 0x04003602 RID: 13826
			private int current;
		}

		// Token: 0x0200061A RID: 1562
		internal class ReadOnlyControlCollection : Control.ControlCollection
		{
			// Token: 0x060051A1 RID: 20897 RVA: 0x0012C146 File Offset: 0x0012B146
			public ReadOnlyControlCollection(Control owner, bool isReadOnly) : base(owner)
			{
				this._isReadOnly = isReadOnly;
			}

			// Token: 0x060051A2 RID: 20898 RVA: 0x0012C156 File Offset: 0x0012B156
			public override void Add(Control value)
			{
				if (this.IsReadOnly)
				{
					throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
				}
				this.AddInternal(value);
			}

			// Token: 0x060051A3 RID: 20899 RVA: 0x0012C177 File Offset: 0x0012B177
			internal virtual void AddInternal(Control value)
			{
				base.Add(value);
			}

			// Token: 0x060051A4 RID: 20900 RVA: 0x0012C180 File Offset: 0x0012B180
			public override void Clear()
			{
				if (this.IsReadOnly)
				{
					throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
				}
				base.Clear();
			}

			// Token: 0x060051A5 RID: 20901 RVA: 0x0012C1A0 File Offset: 0x0012B1A0
			internal virtual void RemoveInternal(Control value)
			{
				base.Remove(value);
			}

			// Token: 0x060051A6 RID: 20902 RVA: 0x0012C1A9 File Offset: 0x0012B1A9
			public override void RemoveByKey(string key)
			{
				if (this.IsReadOnly)
				{
					throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
				}
				base.RemoveByKey(key);
			}

			// Token: 0x17001076 RID: 4214
			// (get) Token: 0x060051A7 RID: 20903 RVA: 0x0012C1CA File Offset: 0x0012B1CA
			public override bool IsReadOnly
			{
				get
				{
					return this._isReadOnly;
				}
			}

			// Token: 0x04003603 RID: 13827
			private readonly bool _isReadOnly;
		}

		// Token: 0x0200061B RID: 1563
		internal class TypedControlCollection : WindowsFormsUtils.ReadOnlyControlCollection
		{
			// Token: 0x060051A8 RID: 20904 RVA: 0x0012C1D2 File Offset: 0x0012B1D2
			public TypedControlCollection(Control owner, Type typeOfControl, bool isReadOnly) : base(owner, isReadOnly)
			{
				this.typeOfControl = typeOfControl;
				this.ownerControl = owner;
			}

			// Token: 0x060051A9 RID: 20905 RVA: 0x0012C1EA File Offset: 0x0012B1EA
			public TypedControlCollection(Control owner, Type typeOfControl) : base(owner, false)
			{
				this.typeOfControl = typeOfControl;
				this.ownerControl = owner;
			}

			// Token: 0x060051AA RID: 20906 RVA: 0x0012C204 File Offset: 0x0012B204
			public override void Add(Control value)
			{
				Control.CheckParentingCycle(this.ownerControl, value);
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.IsReadOnly)
				{
					throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
				}
				if (!this.typeOfControl.IsAssignableFrom(value.GetType()))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("TypedControlCollectionShouldBeOfType", new object[]
					{
						this.typeOfControl.Name
					}), new object[0]), value.GetType().Name);
				}
				base.Add(value);
			}

			// Token: 0x04003604 RID: 13828
			private Type typeOfControl;

			// Token: 0x04003605 RID: 13829
			private Control ownerControl;
		}

		// Token: 0x0200061C RID: 1564
		internal struct DCMapping : IDisposable
		{
			// Token: 0x060051AB RID: 20907 RVA: 0x0012C2A0 File Offset: 0x0012B2A0
			public DCMapping(HandleRef hDC, Rectangle bounds)
			{
				if (hDC.Handle == IntPtr.Zero)
				{
					throw new ArgumentNullException("hDC");
				}
				NativeMethods.POINT point = new NativeMethods.POINT();
				HandleRef handleRef = NativeMethods.NullHandleRef;
				this.translatedBounds = bounds;
				this.graphics = null;
				this.dc = DeviceContext.FromHdc(hDC.Handle);
				this.dc.SaveHdc();
				SafeNativeMethods.GetViewportOrgEx(hDC, point);
				HandleRef handleRef2 = new HandleRef(null, SafeNativeMethods.CreateRectRgn(point.x + bounds.Left, point.y + bounds.Top, point.x + bounds.Right, point.y + bounds.Bottom));
				try
				{
					handleRef = new HandleRef(this, SafeNativeMethods.CreateRectRgn(0, 0, 0, 0));
					int clipRgn = SafeNativeMethods.GetClipRgn(hDC, handleRef);
					NativeMethods.POINT point2 = new NativeMethods.POINT();
					SafeNativeMethods.SetViewportOrgEx(hDC, point.x + bounds.Left, point.y + bounds.Top, point2);
					if (clipRgn != 0)
					{
						NativeMethods.RECT rect = default(NativeMethods.RECT);
						NativeMethods.RegionFlags rgnBox = (NativeMethods.RegionFlags)SafeNativeMethods.GetRgnBox(handleRef, ref rect);
						if (rgnBox == NativeMethods.RegionFlags.SIMPLEREGION)
						{
							SafeNativeMethods.CombineRgn(handleRef2, handleRef2, handleRef, 1);
						}
					}
					else
					{
						SafeNativeMethods.DeleteObject(handleRef);
						handleRef = new HandleRef(null, IntPtr.Zero);
					}
					SafeNativeMethods.SelectClipRgn(hDC, handleRef2);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					this.dc.RestoreHdc();
					this.dc.Dispose();
				}
				finally
				{
					SafeNativeMethods.DeleteObject(handleRef2);
					if (handleRef.Handle != IntPtr.Zero)
					{
						SafeNativeMethods.DeleteObject(handleRef);
					}
				}
			}

			// Token: 0x060051AC RID: 20908 RVA: 0x0012C44C File Offset: 0x0012B44C
			public void Dispose()
			{
				if (this.graphics != null)
				{
					this.graphics.Dispose();
					this.graphics = null;
				}
				if (this.dc != null)
				{
					this.dc.RestoreHdc();
					this.dc.Dispose();
					this.dc = null;
				}
			}

			// Token: 0x17001077 RID: 4215
			// (get) Token: 0x060051AD RID: 20909 RVA: 0x0012C498 File Offset: 0x0012B498
			public Graphics Graphics
			{
				get
				{
					if (this.graphics == null)
					{
						this.graphics = Graphics.FromHdcInternal(this.dc.Hdc);
						this.graphics.SetClip(new Rectangle(Point.Empty, this.translatedBounds.Size));
					}
					return this.graphics;
				}
			}

			// Token: 0x04003606 RID: 13830
			private DeviceContext dc;

			// Token: 0x04003607 RID: 13831
			private Graphics graphics;

			// Token: 0x04003608 RID: 13832
			private Rectangle translatedBounds;
		}
	}
}

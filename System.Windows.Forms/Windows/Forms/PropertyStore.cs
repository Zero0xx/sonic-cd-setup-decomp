using System;
using System.Diagnostics;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020005CD RID: 1485
	internal class PropertyStore
	{
		// Token: 0x06004E15 RID: 19989 RVA: 0x001202D4 File Offset: 0x0011F2D4
		public bool ContainsInteger(int key)
		{
			bool result;
			this.GetInteger(key, out result);
			return result;
		}

		// Token: 0x06004E16 RID: 19990 RVA: 0x001202EC File Offset: 0x0011F2EC
		public bool ContainsObject(int key)
		{
			bool result;
			this.GetObject(key, out result);
			return result;
		}

		// Token: 0x06004E17 RID: 19991 RVA: 0x00120304 File Offset: 0x0011F304
		public static int CreateKey()
		{
			return PropertyStore.currentKey++;
		}

		// Token: 0x06004E18 RID: 19992 RVA: 0x00120314 File Offset: 0x0011F314
		public Color GetColor(int key)
		{
			bool flag;
			return this.GetColor(key, out flag);
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x0012032C File Offset: 0x0011F32C
		public Color GetColor(int key, out bool found)
		{
			object @object = this.GetObject(key, out found);
			if (found)
			{
				PropertyStore.ColorWrapper colorWrapper = @object as PropertyStore.ColorWrapper;
				if (colorWrapper != null)
				{
					return colorWrapper.Color;
				}
			}
			found = false;
			return Color.Empty;
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x00120360 File Offset: 0x0011F360
		public Padding GetPadding(int key)
		{
			bool flag;
			return this.GetPadding(key, out flag);
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x00120378 File Offset: 0x0011F378
		public Padding GetPadding(int key, out bool found)
		{
			object @object = this.GetObject(key, out found);
			if (found)
			{
				PropertyStore.PaddingWrapper paddingWrapper = @object as PropertyStore.PaddingWrapper;
				if (paddingWrapper != null)
				{
					return paddingWrapper.Padding;
				}
			}
			found = false;
			return Padding.Empty;
		}

		// Token: 0x06004E1C RID: 19996 RVA: 0x001203AC File Offset: 0x0011F3AC
		public Size GetSize(int key, out bool found)
		{
			object @object = this.GetObject(key, out found);
			if (found)
			{
				PropertyStore.SizeWrapper sizeWrapper = @object as PropertyStore.SizeWrapper;
				if (sizeWrapper != null)
				{
					return sizeWrapper.Size;
				}
			}
			found = false;
			return Size.Empty;
		}

		// Token: 0x06004E1D RID: 19997 RVA: 0x001203E0 File Offset: 0x0011F3E0
		public Rectangle GetRectangle(int key)
		{
			bool flag;
			return this.GetRectangle(key, out flag);
		}

		// Token: 0x06004E1E RID: 19998 RVA: 0x001203F8 File Offset: 0x0011F3F8
		public Rectangle GetRectangle(int key, out bool found)
		{
			object @object = this.GetObject(key, out found);
			if (found)
			{
				PropertyStore.RectangleWrapper rectangleWrapper = @object as PropertyStore.RectangleWrapper;
				if (rectangleWrapper != null)
				{
					return rectangleWrapper.Rectangle;
				}
			}
			found = false;
			return Rectangle.Empty;
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x0012042C File Offset: 0x0011F42C
		public int GetInteger(int key)
		{
			bool flag;
			return this.GetInteger(key, out flag);
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x00120444 File Offset: 0x0011F444
		public int GetInteger(int key, out bool found)
		{
			int result = 0;
			short num;
			short entryKey = this.SplitKey(key, out num);
			found = false;
			int num2;
			if (this.LocateIntegerEntry(entryKey, out num2) && (1 << (int)num & (int)this.intEntries[num2].Mask) != 0)
			{
				found = true;
				switch (num)
				{
				case 0:
					result = this.intEntries[num2].Value1;
					break;
				case 1:
					result = this.intEntries[num2].Value2;
					break;
				case 2:
					result = this.intEntries[num2].Value3;
					break;
				case 3:
					result = this.intEntries[num2].Value4;
					break;
				}
			}
			return result;
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x001204F8 File Offset: 0x0011F4F8
		public object GetObject(int key)
		{
			bool flag;
			return this.GetObject(key, out flag);
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x00120510 File Offset: 0x0011F510
		public object GetObject(int key, out bool found)
		{
			object result = null;
			short num;
			short entryKey = this.SplitKey(key, out num);
			found = false;
			int num2;
			if (this.LocateObjectEntry(entryKey, out num2) && (1 << (int)num & (int)this.objEntries[num2].Mask) != 0)
			{
				found = true;
				switch (num)
				{
				case 0:
					result = this.objEntries[num2].Value1;
					break;
				case 1:
					result = this.objEntries[num2].Value2;
					break;
				case 2:
					result = this.objEntries[num2].Value3;
					break;
				case 3:
					result = this.objEntries[num2].Value4;
					break;
				}
			}
			return result;
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x001205C4 File Offset: 0x0011F5C4
		private bool LocateIntegerEntry(short entryKey, out int index)
		{
			if (this.intEntries == null)
			{
				index = 0;
				return false;
			}
			int num = this.intEntries.Length;
			if (num > 16)
			{
				int num2 = num - 1;
				int num3 = 0;
				int num4;
				for (;;)
				{
					num4 = (num2 + num3) / 2;
					short key = this.intEntries[num4].Key;
					if (key == entryKey)
					{
						break;
					}
					if (entryKey < key)
					{
						num2 = num4 - 1;
					}
					else
					{
						num3 = num4 + 1;
					}
					if (num2 < num3)
					{
						goto Block_14;
					}
				}
				index = num4;
				return true;
				Block_14:
				index = num4;
				if (entryKey > this.intEntries[num4].Key)
				{
					index++;
				}
				return false;
			}
			index = 0;
			int num5 = num / 2;
			if (this.intEntries[num5].Key <= entryKey)
			{
				index = num5;
			}
			if (this.intEntries[index].Key == entryKey)
			{
				return true;
			}
			num5 = (num + 1) / 4;
			if (this.intEntries[index + num5].Key <= entryKey)
			{
				index += num5;
				if (this.intEntries[index].Key == entryKey)
				{
					return true;
				}
			}
			num5 = (num + 3) / 8;
			if (this.intEntries[index + num5].Key <= entryKey)
			{
				index += num5;
				if (this.intEntries[index].Key == entryKey)
				{
					return true;
				}
			}
			num5 = (num + 7) / 16;
			if (this.intEntries[index + num5].Key <= entryKey)
			{
				index += num5;
				if (this.intEntries[index].Key == entryKey)
				{
					return true;
				}
			}
			if (entryKey > this.intEntries[index].Key)
			{
				index++;
			}
			return false;
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x00120758 File Offset: 0x0011F758
		private bool LocateObjectEntry(short entryKey, out int index)
		{
			if (this.objEntries == null)
			{
				index = 0;
				return false;
			}
			int num = this.objEntries.Length;
			if (num > 16)
			{
				int num2 = num - 1;
				int num3 = 0;
				int num4;
				for (;;)
				{
					num4 = (num2 + num3) / 2;
					short key = this.objEntries[num4].Key;
					if (key == entryKey)
					{
						break;
					}
					if (entryKey < key)
					{
						num2 = num4 - 1;
					}
					else
					{
						num3 = num4 + 1;
					}
					if (num2 < num3)
					{
						goto Block_14;
					}
				}
				index = num4;
				return true;
				Block_14:
				index = num4;
				if (entryKey > this.objEntries[num4].Key)
				{
					index++;
				}
				return false;
			}
			index = 0;
			int num5 = num / 2;
			if (this.objEntries[num5].Key <= entryKey)
			{
				index = num5;
			}
			if (this.objEntries[index].Key == entryKey)
			{
				return true;
			}
			num5 = (num + 1) / 4;
			if (this.objEntries[index + num5].Key <= entryKey)
			{
				index += num5;
				if (this.objEntries[index].Key == entryKey)
				{
					return true;
				}
			}
			num5 = (num + 3) / 8;
			if (this.objEntries[index + num5].Key <= entryKey)
			{
				index += num5;
				if (this.objEntries[index].Key == entryKey)
				{
					return true;
				}
			}
			num5 = (num + 7) / 16;
			if (this.objEntries[index + num5].Key <= entryKey)
			{
				index += num5;
				if (this.objEntries[index].Key == entryKey)
				{
					return true;
				}
			}
			if (entryKey > this.objEntries[index].Key)
			{
				index++;
			}
			return false;
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x001208EC File Offset: 0x0011F8EC
		public void RemoveInteger(int key)
		{
			short num;
			short entryKey = this.SplitKey(key, out num);
			int num2;
			if (this.LocateIntegerEntry(entryKey, out num2))
			{
				if ((1 << (int)num & (int)this.intEntries[num2].Mask) == 0)
				{
					return;
				}
				PropertyStore.IntegerEntry[] array = this.intEntries;
				int num3 = num2;
				array[num3].Mask = (array[num3].Mask & ~(short)(1 << (int)num));
				if (this.intEntries[num2].Mask == 0)
				{
					PropertyStore.IntegerEntry[] array2 = new PropertyStore.IntegerEntry[this.intEntries.Length - 1];
					if (num2 > 0)
					{
						Array.Copy(this.intEntries, 0, array2, 0, num2);
					}
					if (num2 < array2.Length)
					{
						Array.Copy(this.intEntries, num2 + 1, array2, num2, this.intEntries.Length - num2 - 1);
					}
					this.intEntries = array2;
					return;
				}
				switch (num)
				{
				case 0:
					this.intEntries[num2].Value1 = 0;
					return;
				case 1:
					this.intEntries[num2].Value2 = 0;
					return;
				case 2:
					this.intEntries[num2].Value3 = 0;
					return;
				case 3:
					this.intEntries[num2].Value4 = 0;
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x00120A14 File Offset: 0x0011FA14
		public void RemoveObject(int key)
		{
			short num;
			short entryKey = this.SplitKey(key, out num);
			int num2;
			if (this.LocateObjectEntry(entryKey, out num2))
			{
				if ((1 << (int)num & (int)this.objEntries[num2].Mask) == 0)
				{
					return;
				}
				PropertyStore.ObjectEntry[] array = this.objEntries;
				int num3 = num2;
				array[num3].Mask = (array[num3].Mask & ~(short)(1 << (int)num));
				if (this.objEntries[num2].Mask == 0)
				{
					if (this.objEntries.Length == 1)
					{
						this.objEntries = null;
						return;
					}
					PropertyStore.ObjectEntry[] array2 = new PropertyStore.ObjectEntry[this.objEntries.Length - 1];
					if (num2 > 0)
					{
						Array.Copy(this.objEntries, 0, array2, 0, num2);
					}
					if (num2 < array2.Length)
					{
						Array.Copy(this.objEntries, num2 + 1, array2, num2, this.objEntries.Length - num2 - 1);
					}
					this.objEntries = array2;
					return;
				}
				else
				{
					switch (num)
					{
					case 0:
						this.objEntries[num2].Value1 = null;
						return;
					case 1:
						this.objEntries[num2].Value2 = null;
						return;
					case 2:
						this.objEntries[num2].Value3 = null;
						return;
					case 3:
						this.objEntries[num2].Value4 = null;
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x00120B50 File Offset: 0x0011FB50
		public void SetColor(int key, Color value)
		{
			bool flag;
			object @object = this.GetObject(key, out flag);
			if (!flag)
			{
				this.SetObject(key, new PropertyStore.ColorWrapper(value));
				return;
			}
			PropertyStore.ColorWrapper colorWrapper = @object as PropertyStore.ColorWrapper;
			if (colorWrapper != null)
			{
				colorWrapper.Color = value;
				return;
			}
			this.SetObject(key, new PropertyStore.ColorWrapper(value));
		}

		// Token: 0x06004E28 RID: 20008 RVA: 0x00120B98 File Offset: 0x0011FB98
		public void SetPadding(int key, Padding value)
		{
			bool flag;
			object @object = this.GetObject(key, out flag);
			if (!flag)
			{
				this.SetObject(key, new PropertyStore.PaddingWrapper(value));
				return;
			}
			PropertyStore.PaddingWrapper paddingWrapper = @object as PropertyStore.PaddingWrapper;
			if (paddingWrapper != null)
			{
				paddingWrapper.Padding = value;
				return;
			}
			this.SetObject(key, new PropertyStore.PaddingWrapper(value));
		}

		// Token: 0x06004E29 RID: 20009 RVA: 0x00120BE0 File Offset: 0x0011FBE0
		public void SetRectangle(int key, Rectangle value)
		{
			bool flag;
			object @object = this.GetObject(key, out flag);
			if (!flag)
			{
				this.SetObject(key, new PropertyStore.RectangleWrapper(value));
				return;
			}
			PropertyStore.RectangleWrapper rectangleWrapper = @object as PropertyStore.RectangleWrapper;
			if (rectangleWrapper != null)
			{
				rectangleWrapper.Rectangle = value;
				return;
			}
			this.SetObject(key, new PropertyStore.RectangleWrapper(value));
		}

		// Token: 0x06004E2A RID: 20010 RVA: 0x00120C28 File Offset: 0x0011FC28
		public void SetSize(int key, Size value)
		{
			bool flag;
			object @object = this.GetObject(key, out flag);
			if (!flag)
			{
				this.SetObject(key, new PropertyStore.SizeWrapper(value));
				return;
			}
			PropertyStore.SizeWrapper sizeWrapper = @object as PropertyStore.SizeWrapper;
			if (sizeWrapper != null)
			{
				sizeWrapper.Size = value;
				return;
			}
			this.SetObject(key, new PropertyStore.SizeWrapper(value));
		}

		// Token: 0x06004E2B RID: 20011 RVA: 0x00120C70 File Offset: 0x0011FC70
		public void SetInteger(int key, int value)
		{
			short num2;
			short num = this.SplitKey(key, out num2);
			int num3;
			if (!this.LocateIntegerEntry(num, out num3))
			{
				if (this.intEntries != null)
				{
					PropertyStore.IntegerEntry[] destinationArray = new PropertyStore.IntegerEntry[this.intEntries.Length + 1];
					if (num3 > 0)
					{
						Array.Copy(this.intEntries, 0, destinationArray, 0, num3);
					}
					if (this.intEntries.Length - num3 > 0)
					{
						Array.Copy(this.intEntries, num3, destinationArray, num3 + 1, this.intEntries.Length - num3);
					}
					this.intEntries = destinationArray;
				}
				else
				{
					this.intEntries = new PropertyStore.IntegerEntry[1];
				}
				this.intEntries[num3].Key = num;
			}
			switch (num2)
			{
			case 0:
				this.intEntries[num3].Value1 = value;
				break;
			case 1:
				this.intEntries[num3].Value2 = value;
				break;
			case 2:
				this.intEntries[num3].Value3 = value;
				break;
			case 3:
				this.intEntries[num3].Value4 = value;
				break;
			}
			this.intEntries[num3].Mask = (short)(1 << (int)num2 | (int)((ushort)this.intEntries[num3].Mask));
		}

		// Token: 0x06004E2C RID: 20012 RVA: 0x00120DA0 File Offset: 0x0011FDA0
		public void SetObject(int key, object value)
		{
			short num2;
			short num = this.SplitKey(key, out num2);
			int num3;
			if (!this.LocateObjectEntry(num, out num3))
			{
				if (this.objEntries != null)
				{
					PropertyStore.ObjectEntry[] destinationArray = new PropertyStore.ObjectEntry[this.objEntries.Length + 1];
					if (num3 > 0)
					{
						Array.Copy(this.objEntries, 0, destinationArray, 0, num3);
					}
					if (this.objEntries.Length - num3 > 0)
					{
						Array.Copy(this.objEntries, num3, destinationArray, num3 + 1, this.objEntries.Length - num3);
					}
					this.objEntries = destinationArray;
				}
				else
				{
					this.objEntries = new PropertyStore.ObjectEntry[1];
				}
				this.objEntries[num3].Key = num;
			}
			switch (num2)
			{
			case 0:
				this.objEntries[num3].Value1 = value;
				break;
			case 1:
				this.objEntries[num3].Value2 = value;
				break;
			case 2:
				this.objEntries[num3].Value3 = value;
				break;
			case 3:
				this.objEntries[num3].Value4 = value;
				break;
			}
			this.objEntries[num3].Mask = (short)((int)((ushort)this.objEntries[num3].Mask) | 1 << (int)num2);
		}

		// Token: 0x06004E2D RID: 20013 RVA: 0x00120ED0 File Offset: 0x0011FED0
		private short SplitKey(int key, out short element)
		{
			element = (short)(key & 3);
			return (short)((long)key & (long)((ulong)-4));
		}

		// Token: 0x06004E2E RID: 20014 RVA: 0x00120EE0 File Offset: 0x0011FEE0
		[Conditional("DEBUG_PROPERTYSTORE")]
		private void Debug_VerifyLocateIntegerEntry(int index, short entryKey, int length)
		{
			int num = length - 1;
			int num2 = 0;
			int num3;
			do
			{
				num3 = (num + num2) / 2;
				short key = this.intEntries[num3].Key;
				if (key != entryKey)
				{
					if (entryKey < key)
					{
						num = num3 - 1;
					}
					else
					{
						num2 = num3 + 1;
					}
				}
			}
			while (num >= num2);
			if (entryKey > this.intEntries[num3].Key)
			{
				num3++;
			}
		}

		// Token: 0x06004E2F RID: 20015 RVA: 0x00120F3C File Offset: 0x0011FF3C
		[Conditional("DEBUG_PROPERTYSTORE")]
		private void Debug_VerifyLocateObjectEntry(int index, short entryKey, int length)
		{
			int num = length - 1;
			int num2 = 0;
			int num3;
			do
			{
				num3 = (num + num2) / 2;
				short key = this.objEntries[num3].Key;
				if (key != entryKey)
				{
					if (entryKey < key)
					{
						num = num3 - 1;
					}
					else
					{
						num2 = num3 + 1;
					}
				}
			}
			while (num >= num2);
			if (entryKey > this.objEntries[num3].Key)
			{
				num3++;
			}
		}

		// Token: 0x0400329D RID: 12957
		private static int currentKey;

		// Token: 0x0400329E RID: 12958
		private PropertyStore.IntegerEntry[] intEntries;

		// Token: 0x0400329F RID: 12959
		private PropertyStore.ObjectEntry[] objEntries;

		// Token: 0x020005CE RID: 1486
		private struct IntegerEntry
		{
			// Token: 0x040032A0 RID: 12960
			public short Key;

			// Token: 0x040032A1 RID: 12961
			public short Mask;

			// Token: 0x040032A2 RID: 12962
			public int Value1;

			// Token: 0x040032A3 RID: 12963
			public int Value2;

			// Token: 0x040032A4 RID: 12964
			public int Value3;

			// Token: 0x040032A5 RID: 12965
			public int Value4;
		}

		// Token: 0x020005CF RID: 1487
		private struct ObjectEntry
		{
			// Token: 0x040032A6 RID: 12966
			public short Key;

			// Token: 0x040032A7 RID: 12967
			public short Mask;

			// Token: 0x040032A8 RID: 12968
			public object Value1;

			// Token: 0x040032A9 RID: 12969
			public object Value2;

			// Token: 0x040032AA RID: 12970
			public object Value3;

			// Token: 0x040032AB RID: 12971
			public object Value4;
		}

		// Token: 0x020005D0 RID: 1488
		private sealed class ColorWrapper
		{
			// Token: 0x06004E31 RID: 20017 RVA: 0x00120F9F File Offset: 0x0011FF9F
			public ColorWrapper(Color color)
			{
				this.Color = color;
			}

			// Token: 0x040032AC RID: 12972
			public Color Color;
		}

		// Token: 0x020005D1 RID: 1489
		private sealed class PaddingWrapper
		{
			// Token: 0x06004E32 RID: 20018 RVA: 0x00120FAE File Offset: 0x0011FFAE
			public PaddingWrapper(Padding padding)
			{
				this.Padding = padding;
			}

			// Token: 0x040032AD RID: 12973
			public Padding Padding;
		}

		// Token: 0x020005D2 RID: 1490
		private sealed class RectangleWrapper
		{
			// Token: 0x06004E33 RID: 20019 RVA: 0x00120FBD File Offset: 0x0011FFBD
			public RectangleWrapper(Rectangle rectangle)
			{
				this.Rectangle = rectangle;
			}

			// Token: 0x040032AE RID: 12974
			public Rectangle Rectangle;
		}

		// Token: 0x020005D3 RID: 1491
		private sealed class SizeWrapper
		{
			// Token: 0x06004E34 RID: 20020 RVA: 0x00120FCC File Offset: 0x0011FFCC
			public SizeWrapper(Size size)
			{
				this.Size = size;
			}

			// Token: 0x040032AF RID: 12975
			public Size Size;
		}
	}
}

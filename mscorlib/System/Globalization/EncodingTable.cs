using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Globalization
{
	// Token: 0x020003B2 RID: 946
	internal static class EncodingTable
	{
		// Token: 0x060025B6 RID: 9654 RVA: 0x0006918C File Offset: 0x0006818C
		private unsafe static int internalGetCodePageFromName(string name)
		{
			int i = 0;
			int num = EncodingTable.lastEncodingItem;
			while (num - i > 3)
			{
				int num2 = (num - i) / 2 + i;
				bool flag;
				int num3 = string.nativeCompareOrdinalWC(name, EncodingTable.encodingDataPtr[num2].webName, true, out flag);
				if (num3 == 0)
				{
					return EncodingTable.encodingDataPtr[num2].codePage;
				}
				if (num3 < 0)
				{
					num = num2;
				}
				else
				{
					i = num2;
				}
			}
			while (i <= num)
			{
				bool flag2;
				if (string.nativeCompareOrdinalWC(name, EncodingTable.encodingDataPtr[i].webName, true, out flag2) == 0)
				{
					return EncodingTable.encodingDataPtr[i].codePage;
				}
				i++;
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_EncodingNotSupported"), new object[]
			{
				name
			}), "name");
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x0006925C File Offset: 0x0006825C
		internal unsafe static EncodingInfo[] GetEncodings()
		{
			if (EncodingTable.lastCodePageItem == 0)
			{
				int num = 0;
				while (EncodingTable.codePageDataPtr[num].codePage != 0)
				{
					num++;
				}
				EncodingTable.lastCodePageItem = num;
			}
			EncodingInfo[] array = new EncodingInfo[EncodingTable.lastCodePageItem];
			for (int i = 0; i < EncodingTable.lastCodePageItem; i++)
			{
				array[i] = new EncodingInfo(EncodingTable.codePageDataPtr[i].codePage, new string(EncodingTable.codePageDataPtr[i].webName), Environment.GetResourceString("Globalization.cp_" + EncodingTable.codePageDataPtr[i].codePage));
			}
			return array;
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x00069310 File Offset: 0x00068310
		internal static int GetCodePageFromName(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			object obj = EncodingTable.hashByName[name];
			if (obj != null)
			{
				return (int)obj;
			}
			int num = EncodingTable.internalGetCodePageFromName(name);
			EncodingTable.hashByName[name] = num;
			return num;
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x0006935C File Offset: 0x0006835C
		internal unsafe static CodePageDataItem GetCodePageDataItem(int codepage)
		{
			CodePageDataItem codePageDataItem = (CodePageDataItem)EncodingTable.hashByCodePage[codepage];
			if (codePageDataItem != null)
			{
				return codePageDataItem;
			}
			int num = 0;
			int codePage;
			while ((codePage = EncodingTable.codePageDataPtr[num].codePage) != 0)
			{
				if (codePage == codepage)
				{
					codePageDataItem = new CodePageDataItem(num);
					EncodingTable.hashByCodePage[codepage] = codePageDataItem;
					return codePageDataItem;
				}
				num++;
			}
			return null;
		}

		// Token: 0x060025BA RID: 9658
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern InternalEncodingDataItem* GetEncodingData();

		// Token: 0x060025BB RID: 9659
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetNumEncodingItems();

		// Token: 0x060025BC RID: 9660
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern InternalCodePageDataItem* GetCodePageData();

		// Token: 0x060025BD RID: 9661
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern byte* nativeCreateOpenFileMapping(string inSectionName, int inBytesToAllocate, out IntPtr mappedFileHandle);

		// Token: 0x0400111A RID: 4378
		private static int lastEncodingItem = EncodingTable.GetNumEncodingItems() - 1;

		// Token: 0x0400111B RID: 4379
		private static int lastCodePageItem;

		// Token: 0x0400111C RID: 4380
		internal unsafe static InternalEncodingDataItem* encodingDataPtr = EncodingTable.GetEncodingData();

		// Token: 0x0400111D RID: 4381
		internal unsafe static InternalCodePageDataItem* codePageDataPtr = EncodingTable.GetCodePageData();

		// Token: 0x0400111E RID: 4382
		internal static Hashtable hashByName = Hashtable.Synchronized(new Hashtable(StringComparer.OrdinalIgnoreCase));

		// Token: 0x0400111F RID: 4383
		internal static Hashtable hashByCodePage = Hashtable.Synchronized(new Hashtable());
	}
}

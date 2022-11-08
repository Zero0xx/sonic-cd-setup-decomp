using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Text
{
	// Token: 0x020003F3 RID: 1011
	[Serializable]
	internal abstract class BaseCodePageEncoding : EncodingNLS, ISerializable
	{
		// Token: 0x060029CC RID: 10700 RVA: 0x0008261F File Offset: 0x0008161F
		internal BaseCodePageEncoding(int codepage) : this(codepage, codepage)
		{
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x00082629 File Offset: 0x00081629
		internal BaseCodePageEncoding(int codepage, int dataCodePage)
		{
			this.bFlagDataTable = true;
			this.pCodePage = null;
			base..ctor((codepage == 0) ? Win32Native.GetACP() : codepage);
			this.dataTableCodePage = dataCodePage;
			this.LoadCodePageTables();
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x00082658 File Offset: 0x00081658
		internal BaseCodePageEncoding(SerializationInfo info, StreamingContext context)
		{
			this.bFlagDataTable = true;
			this.pCodePage = null;
			base..ctor(0);
			throw new ArgumentNullException("this");
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x0008267C File Offset: 0x0008167C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.SerializeEncoding(info, context);
			info.AddValue(this.m_bUseMlangTypeForSerialization ? "m_maxByteSize" : "maxCharSize", this.IsSingleByte ? 1 : 2);
			info.SetType(this.m_bUseMlangTypeForSerialization ? typeof(MLangCodePageEncoding) : typeof(CodePageEncoding));
		}

		// Token: 0x060029D0 RID: 10704 RVA: 0x000826DC File Offset: 0x000816DC
		private unsafe void LoadCodePageTables()
		{
			BaseCodePageEncoding.CodePageHeader* ptr = BaseCodePageEncoding.FindCodePage(this.dataTableCodePage);
			if (ptr == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_NoCodepageData", new object[]
				{
					this.CodePage
				}));
			}
			this.pCodePage = ptr;
			this.LoadManagedCodePage();
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x00082730 File Offset: 0x00081730
		private unsafe static BaseCodePageEncoding.CodePageHeader* FindCodePage(int codePage)
		{
			for (int i = 0; i < (int)BaseCodePageEncoding.m_pCodePageFileHeader->CodePageCount; i++)
			{
				BaseCodePageEncoding.CodePageIndex* ptr = &BaseCodePageEncoding.m_pCodePageFileHeader->CodePages + i;
				if ((int)ptr->CodePage == codePage)
				{
					return (BaseCodePageEncoding.CodePageHeader*)(BaseCodePageEncoding.m_pCodePageFileHeader + ptr->Offset / sizeof(BaseCodePageEncoding.CodePageDataFileHeader));
				}
			}
			return null;
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x00082784 File Offset: 0x00081784
		internal unsafe static int GetCodePageByteSize(int codePage)
		{
			BaseCodePageEncoding.CodePageHeader* ptr = BaseCodePageEncoding.FindCodePage(codePage);
			if (ptr == null)
			{
				return 0;
			}
			return (int)ptr->ByteCount;
		}

		// Token: 0x060029D3 RID: 10707
		protected abstract void LoadManagedCodePage();

		// Token: 0x060029D4 RID: 10708 RVA: 0x000827A8 File Offset: 0x000817A8
		protected unsafe byte* GetSharedMemory(int iSize)
		{
			string memorySectionName = this.GetMemorySectionName();
			IntPtr intPtr;
			byte* ptr = EncodingTable.nativeCreateOpenFileMapping(memorySectionName, iSize, out intPtr);
			if (ptr == null)
			{
				throw new OutOfMemoryException(Environment.GetResourceString("Arg_OutOfMemoryException"));
			}
			if (intPtr != IntPtr.Zero)
			{
				this.safeMemorySectionHandle = new SafeViewOfFileHandle((IntPtr)((void*)ptr), true);
				this.safeFileMappingHandle = new SafeFileMappingHandle(intPtr, true);
			}
			return ptr;
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x00082808 File Offset: 0x00081808
		protected unsafe virtual string GetMemorySectionName()
		{
			int num = this.bFlagDataTable ? this.dataTableCodePage : this.CodePage;
			return string.Format(CultureInfo.InvariantCulture, "NLS_CodePage_{0}_{1}_{2}_{3}_{4}", new object[]
			{
				num,
				this.pCodePage->VersionMajor,
				this.pCodePage->VersionMinor,
				this.pCodePage->VersionRevision,
				this.pCodePage->VersionBuild
			});
		}

		// Token: 0x060029D6 RID: 10710
		protected abstract void ReadBestFitTable();

		// Token: 0x060029D7 RID: 10711 RVA: 0x0008289A File Offset: 0x0008189A
		internal override char[] GetBestFitUnicodeToBytesData()
		{
			if (this.arrayUnicodeBestFit == null)
			{
				this.ReadBestFitTable();
			}
			return this.arrayUnicodeBestFit;
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x000828B0 File Offset: 0x000818B0
		internal override char[] GetBestFitBytesToUnicodeData()
		{
			if (this.arrayUnicodeBestFit == null)
			{
				this.ReadBestFitTable();
			}
			return this.arrayBytesBestFit;
		}

		// Token: 0x060029D9 RID: 10713 RVA: 0x000828C6 File Offset: 0x000818C6
		internal void CheckMemorySection()
		{
			if (this.safeMemorySectionHandle != null && this.safeMemorySectionHandle.DangerousGetHandle() == IntPtr.Zero)
			{
				this.LoadManagedCodePage();
			}
		}

		// Token: 0x04001452 RID: 5202
		internal const string CODE_PAGE_DATA_FILE_NAME = "codepages.nlp";

		// Token: 0x04001453 RID: 5203
		[NonSerialized]
		protected int dataTableCodePage;

		// Token: 0x04001454 RID: 5204
		[NonSerialized]
		protected bool bFlagDataTable;

		// Token: 0x04001455 RID: 5205
		[NonSerialized]
		protected int iExtraBytes;

		// Token: 0x04001456 RID: 5206
		[NonSerialized]
		protected char[] arrayUnicodeBestFit;

		// Token: 0x04001457 RID: 5207
		[NonSerialized]
		protected char[] arrayBytesBestFit;

		// Token: 0x04001458 RID: 5208
		[NonSerialized]
		protected bool m_bUseMlangTypeForSerialization;

		// Token: 0x04001459 RID: 5209
		private unsafe static BaseCodePageEncoding.CodePageDataFileHeader* m_pCodePageFileHeader = (BaseCodePageEncoding.CodePageDataFileHeader*)GlobalizationAssembly.GetGlobalizationResourceBytePtr(typeof(CharUnicodeInfo).Assembly, "codepages.nlp");

		// Token: 0x0400145A RID: 5210
		[NonSerialized]
		protected unsafe BaseCodePageEncoding.CodePageHeader* pCodePage;

		// Token: 0x0400145B RID: 5211
		[NonSerialized]
		protected SafeViewOfFileHandle safeMemorySectionHandle;

		// Token: 0x0400145C RID: 5212
		[NonSerialized]
		protected SafeFileMappingHandle safeFileMappingHandle;

		// Token: 0x020003F4 RID: 1012
		[StructLayout(LayoutKind.Explicit)]
		internal struct CodePageDataFileHeader
		{
			// Token: 0x0400145D RID: 5213
			[FieldOffset(0)]
			internal char TableName;

			// Token: 0x0400145E RID: 5214
			[FieldOffset(32)]
			internal ushort Version;

			// Token: 0x0400145F RID: 5215
			[FieldOffset(40)]
			internal short CodePageCount;

			// Token: 0x04001460 RID: 5216
			[FieldOffset(42)]
			internal short unused1;

			// Token: 0x04001461 RID: 5217
			[FieldOffset(44)]
			internal BaseCodePageEncoding.CodePageIndex CodePages;
		}

		// Token: 0x020003F5 RID: 1013
		[StructLayout(LayoutKind.Explicit, Pack = 2)]
		internal struct CodePageIndex
		{
			// Token: 0x04001462 RID: 5218
			[FieldOffset(0)]
			internal char CodePageName;

			// Token: 0x04001463 RID: 5219
			[FieldOffset(32)]
			internal short CodePage;

			// Token: 0x04001464 RID: 5220
			[FieldOffset(34)]
			internal short ByteCount;

			// Token: 0x04001465 RID: 5221
			[FieldOffset(36)]
			internal int Offset;
		}

		// Token: 0x020003F6 RID: 1014
		[StructLayout(LayoutKind.Explicit)]
		internal struct CodePageHeader
		{
			// Token: 0x04001466 RID: 5222
			[FieldOffset(0)]
			internal char CodePageName;

			// Token: 0x04001467 RID: 5223
			[FieldOffset(32)]
			internal ushort VersionMajor;

			// Token: 0x04001468 RID: 5224
			[FieldOffset(34)]
			internal ushort VersionMinor;

			// Token: 0x04001469 RID: 5225
			[FieldOffset(36)]
			internal ushort VersionRevision;

			// Token: 0x0400146A RID: 5226
			[FieldOffset(38)]
			internal ushort VersionBuild;

			// Token: 0x0400146B RID: 5227
			[FieldOffset(40)]
			internal short CodePage;

			// Token: 0x0400146C RID: 5228
			[FieldOffset(42)]
			internal short ByteCount;

			// Token: 0x0400146D RID: 5229
			[FieldOffset(44)]
			internal char UnicodeReplace;

			// Token: 0x0400146E RID: 5230
			[FieldOffset(46)]
			internal ushort ByteReplace;

			// Token: 0x0400146F RID: 5231
			[FieldOffset(48)]
			internal short FirstDataWord;
		}
	}
}

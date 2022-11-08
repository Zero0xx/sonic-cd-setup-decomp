using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x0200075A RID: 1882
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class FileVersionInfo
	{
		// Token: 0x060039B6 RID: 14774 RVA: 0x000F4864 File Offset: 0x000F3864
		private FileVersionInfo(string fileName)
		{
			this.fileName = fileName;
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x060039B7 RID: 14775 RVA: 0x000F4873 File Offset: 0x000F3873
		public string Comments
		{
			get
			{
				return this.comments;
			}
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x060039B8 RID: 14776 RVA: 0x000F487B File Offset: 0x000F387B
		public string CompanyName
		{
			get
			{
				return this.companyName;
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x060039B9 RID: 14777 RVA: 0x000F4883 File Offset: 0x000F3883
		public int FileBuildPart
		{
			get
			{
				return this.fileBuild;
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x060039BA RID: 14778 RVA: 0x000F488B File Offset: 0x000F388B
		public string FileDescription
		{
			get
			{
				return this.fileDescription;
			}
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x060039BB RID: 14779 RVA: 0x000F4893 File Offset: 0x000F3893
		public int FileMajorPart
		{
			get
			{
				return this.fileMajor;
			}
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x060039BC RID: 14780 RVA: 0x000F489B File Offset: 0x000F389B
		public int FileMinorPart
		{
			get
			{
				return this.fileMinor;
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x060039BD RID: 14781 RVA: 0x000F48A3 File Offset: 0x000F38A3
		public string FileName
		{
			get
			{
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this.fileName).Demand();
				return this.fileName;
			}
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x060039BE RID: 14782 RVA: 0x000F48BC File Offset: 0x000F38BC
		public int FilePrivatePart
		{
			get
			{
				return this.filePrivate;
			}
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x060039BF RID: 14783 RVA: 0x000F48C4 File Offset: 0x000F38C4
		public string FileVersion
		{
			get
			{
				return this.fileVersion;
			}
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x060039C0 RID: 14784 RVA: 0x000F48CC File Offset: 0x000F38CC
		public string InternalName
		{
			get
			{
				return this.internalName;
			}
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x060039C1 RID: 14785 RVA: 0x000F48D4 File Offset: 0x000F38D4
		public bool IsDebug
		{
			get
			{
				return (this.fileFlags & 1) != 0;
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x060039C2 RID: 14786 RVA: 0x000F48E4 File Offset: 0x000F38E4
		public bool IsPatched
		{
			get
			{
				return (this.fileFlags & 4) != 0;
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x060039C3 RID: 14787 RVA: 0x000F48F4 File Offset: 0x000F38F4
		public bool IsPrivateBuild
		{
			get
			{
				return (this.fileFlags & 8) != 0;
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x060039C4 RID: 14788 RVA: 0x000F4904 File Offset: 0x000F3904
		public bool IsPreRelease
		{
			get
			{
				return (this.fileFlags & 2) != 0;
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x060039C5 RID: 14789 RVA: 0x000F4914 File Offset: 0x000F3914
		public bool IsSpecialBuild
		{
			get
			{
				return (this.fileFlags & 32) != 0;
			}
		}

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x060039C6 RID: 14790 RVA: 0x000F4925 File Offset: 0x000F3925
		public string Language
		{
			get
			{
				return this.language;
			}
		}

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x060039C7 RID: 14791 RVA: 0x000F492D File Offset: 0x000F392D
		public string LegalCopyright
		{
			get
			{
				return this.legalCopyright;
			}
		}

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x060039C8 RID: 14792 RVA: 0x000F4935 File Offset: 0x000F3935
		public string LegalTrademarks
		{
			get
			{
				return this.legalTrademarks;
			}
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x060039C9 RID: 14793 RVA: 0x000F493D File Offset: 0x000F393D
		public string OriginalFilename
		{
			get
			{
				return this.originalFilename;
			}
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x060039CA RID: 14794 RVA: 0x000F4945 File Offset: 0x000F3945
		public string PrivateBuild
		{
			get
			{
				return this.privateBuild;
			}
		}

		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x060039CB RID: 14795 RVA: 0x000F494D File Offset: 0x000F394D
		public int ProductBuildPart
		{
			get
			{
				return this.productBuild;
			}
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x060039CC RID: 14796 RVA: 0x000F4955 File Offset: 0x000F3955
		public int ProductMajorPart
		{
			get
			{
				return this.productMajor;
			}
		}

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x060039CD RID: 14797 RVA: 0x000F495D File Offset: 0x000F395D
		public int ProductMinorPart
		{
			get
			{
				return this.productMinor;
			}
		}

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x060039CE RID: 14798 RVA: 0x000F4965 File Offset: 0x000F3965
		public string ProductName
		{
			get
			{
				return this.productName;
			}
		}

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x060039CF RID: 14799 RVA: 0x000F496D File Offset: 0x000F396D
		public int ProductPrivatePart
		{
			get
			{
				return this.productPrivate;
			}
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x060039D0 RID: 14800 RVA: 0x000F4975 File Offset: 0x000F3975
		public string ProductVersion
		{
			get
			{
				return this.productVersion;
			}
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x060039D1 RID: 14801 RVA: 0x000F497D File Offset: 0x000F397D
		public string SpecialBuild
		{
			get
			{
				return this.specialBuild;
			}
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x000F4988 File Offset: 0x000F3988
		private static string ConvertTo8DigitHex(int value)
		{
			string text = Convert.ToString(value, 16);
			text = text.ToUpper(CultureInfo.InvariantCulture);
			if (text.Length == 8)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(8);
			for (int i = text.Length; i < 8; i++)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}

		// Token: 0x060039D3 RID: 14803 RVA: 0x000F49E8 File Offset: 0x000F39E8
		private static NativeMethods.VS_FIXEDFILEINFO GetFixedFileInfo(IntPtr memPtr)
		{
			IntPtr zero = IntPtr.Zero;
			int num;
			if (UnsafeNativeMethods.VerQueryValue(new HandleRef(null, memPtr), "\\", ref zero, out num))
			{
				NativeMethods.VS_FIXEDFILEINFO vs_FIXEDFILEINFO = new NativeMethods.VS_FIXEDFILEINFO();
				Marshal.PtrToStructure(zero, vs_FIXEDFILEINFO);
				return vs_FIXEDFILEINFO;
			}
			return new NativeMethods.VS_FIXEDFILEINFO();
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x000F4A28 File Offset: 0x000F3A28
		private static string GetFileVersionLanguage(IntPtr memPtr)
		{
			int langID = FileVersionInfo.GetVarEntry(memPtr) >> 16;
			StringBuilder stringBuilder = new StringBuilder(256);
			UnsafeNativeMethods.VerLanguageName(langID, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x000F4A60 File Offset: 0x000F3A60
		private static string GetFileVersionString(IntPtr memPtr, string name)
		{
			string result = "";
			IntPtr zero = IntPtr.Zero;
			int num;
			if (UnsafeNativeMethods.VerQueryValue(new HandleRef(null, memPtr), name, ref zero, out num) && zero != IntPtr.Zero)
			{
				result = Marshal.PtrToStringAuto(zero);
			}
			return result;
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x000F4AA4 File Offset: 0x000F3AA4
		private static int GetVarEntry(IntPtr memPtr)
		{
			IntPtr zero = IntPtr.Zero;
			int num;
			if (UnsafeNativeMethods.VerQueryValue(new HandleRef(null, memPtr), "\\VarFileInfo\\Translation", ref zero, out num))
			{
				return ((int)Marshal.ReadInt16(zero) << 16) + (int)Marshal.ReadInt16((IntPtr)((long)zero + 2L));
			}
			return 67699940;
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x000F4AF4 File Offset: 0x000F3AF4
		private bool GetVersionInfoForCodePage(IntPtr memIntPtr, string codepage)
		{
			string format = "\\\\StringFileInfo\\\\{0}\\\\{1}";
			this.companyName = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				codepage,
				"CompanyName"
			}));
			this.fileDescription = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				codepage,
				"FileDescription"
			}));
			this.fileVersion = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				codepage,
				"FileVersion"
			}));
			this.internalName = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				codepage,
				"InternalName"
			}));
			this.legalCopyright = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				codepage,
				"LegalCopyright"
			}));
			this.originalFilename = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				codepage,
				"OriginalFilename"
			}));
			this.productName = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				codepage,
				"ProductName"
			}));
			this.productVersion = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				codepage,
				"ProductVersion"
			}));
			this.comments = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				codepage,
				"Comments"
			}));
			this.legalTrademarks = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				codepage,
				"LegalTrademarks"
			}));
			this.privateBuild = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				codepage,
				"PrivateBuild"
			}));
			this.specialBuild = FileVersionInfo.GetFileVersionString(memIntPtr, string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				codepage,
				"SpecialBuild"
			}));
			this.language = FileVersionInfo.GetFileVersionLanguage(memIntPtr);
			NativeMethods.VS_FIXEDFILEINFO fixedFileInfo = FileVersionInfo.GetFixedFileInfo(memIntPtr);
			this.fileMajor = FileVersionInfo.HIWORD(fixedFileInfo.dwFileVersionMS);
			this.fileMinor = FileVersionInfo.LOWORD(fixedFileInfo.dwFileVersionMS);
			this.fileBuild = FileVersionInfo.HIWORD(fixedFileInfo.dwFileVersionLS);
			this.filePrivate = FileVersionInfo.LOWORD(fixedFileInfo.dwFileVersionLS);
			this.productMajor = FileVersionInfo.HIWORD(fixedFileInfo.dwProductVersionMS);
			this.productMinor = FileVersionInfo.LOWORD(fixedFileInfo.dwProductVersionMS);
			this.productBuild = FileVersionInfo.HIWORD(fixedFileInfo.dwProductVersionLS);
			this.productPrivate = FileVersionInfo.LOWORD(fixedFileInfo.dwProductVersionLS);
			this.fileFlags = fixedFileInfo.dwFileFlags;
			return this.fileVersion != string.Empty;
		}

		// Token: 0x060039D8 RID: 14808 RVA: 0x000F4DEA File Offset: 0x000F3DEA
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
		private static string GetFullPathWithAssert(string fileName)
		{
			return Path.GetFullPath(fileName);
		}

		// Token: 0x060039D9 RID: 14809 RVA: 0x000F4E04 File Offset: 0x000F3E04
		public unsafe static FileVersionInfo GetVersionInfo(string fileName)
		{
			if (!File.Exists(fileName))
			{
				string fullPathWithAssert = FileVersionInfo.GetFullPathWithAssert(fileName);
				new FileIOPermission(FileIOPermissionAccess.Read, fullPathWithAssert).Demand();
				throw new FileNotFoundException(fileName);
			}
			int num;
			int fileVersionInfoSize = UnsafeNativeMethods.GetFileVersionInfoSize(fileName, out num);
			FileVersionInfo fileVersionInfo = new FileVersionInfo(fileName);
			if (fileVersionInfoSize != 0)
			{
				byte[] array = new byte[fileVersionInfoSize];
				fixed (byte* ptr = array)
				{
					IntPtr intPtr = new IntPtr((void*)ptr);
					if (UnsafeNativeMethods.GetFileVersionInfo(fileName, 0, fileVersionInfoSize, new HandleRef(null, intPtr)))
					{
						int varEntry = FileVersionInfo.GetVarEntry(intPtr);
						if (!fileVersionInfo.GetVersionInfoForCodePage(intPtr, FileVersionInfo.ConvertTo8DigitHex(varEntry)))
						{
							int[] array2 = new int[]
							{
								67699888,
								67699940,
								67698688
							};
							foreach (int num2 in array2)
							{
								if (num2 != varEntry && fileVersionInfo.GetVersionInfoForCodePage(intPtr, FileVersionInfo.ConvertTo8DigitHex(num2)))
								{
									break;
								}
							}
						}
					}
				}
			}
			return fileVersionInfo;
		}

		// Token: 0x060039DA RID: 14810 RVA: 0x000F4EF1 File Offset: 0x000F3EF1
		private static int HIWORD(int dword)
		{
			return NativeMethods.Util.HIWORD(dword);
		}

		// Token: 0x060039DB RID: 14811 RVA: 0x000F4EF9 File Offset: 0x000F3EF9
		private static int LOWORD(int dword)
		{
			return NativeMethods.Util.LOWORD(dword);
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x000F4F04 File Offset: 0x000F3F04
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			string value = "\r\n";
			stringBuilder.Append("File:             ");
			stringBuilder.Append(this.FileName);
			stringBuilder.Append(value);
			stringBuilder.Append("InternalName:     ");
			stringBuilder.Append(this.InternalName);
			stringBuilder.Append(value);
			stringBuilder.Append("OriginalFilename: ");
			stringBuilder.Append(this.OriginalFilename);
			stringBuilder.Append(value);
			stringBuilder.Append("FileVersion:      ");
			stringBuilder.Append(this.FileVersion);
			stringBuilder.Append(value);
			stringBuilder.Append("FileDescription:  ");
			stringBuilder.Append(this.FileDescription);
			stringBuilder.Append(value);
			stringBuilder.Append("Product:          ");
			stringBuilder.Append(this.ProductName);
			stringBuilder.Append(value);
			stringBuilder.Append("ProductVersion:   ");
			stringBuilder.Append(this.ProductVersion);
			stringBuilder.Append(value);
			stringBuilder.Append("Debug:            ");
			stringBuilder.Append(this.IsDebug.ToString());
			stringBuilder.Append(value);
			stringBuilder.Append("Patched:          ");
			stringBuilder.Append(this.IsPatched.ToString());
			stringBuilder.Append(value);
			stringBuilder.Append("PreRelease:       ");
			stringBuilder.Append(this.IsPreRelease.ToString());
			stringBuilder.Append(value);
			stringBuilder.Append("PrivateBuild:     ");
			stringBuilder.Append(this.IsPrivateBuild.ToString());
			stringBuilder.Append(value);
			stringBuilder.Append("SpecialBuild:     ");
			stringBuilder.Append(this.IsSpecialBuild.ToString());
			stringBuilder.Append(value);
			stringBuilder.Append("Language:         ");
			stringBuilder.Append(this.Language);
			stringBuilder.Append(value);
			return stringBuilder.ToString();
		}

		// Token: 0x040032C7 RID: 12999
		private string fileName;

		// Token: 0x040032C8 RID: 13000
		private string companyName;

		// Token: 0x040032C9 RID: 13001
		private string fileDescription;

		// Token: 0x040032CA RID: 13002
		private string fileVersion;

		// Token: 0x040032CB RID: 13003
		private string internalName;

		// Token: 0x040032CC RID: 13004
		private string legalCopyright;

		// Token: 0x040032CD RID: 13005
		private string originalFilename;

		// Token: 0x040032CE RID: 13006
		private string productName;

		// Token: 0x040032CF RID: 13007
		private string productVersion;

		// Token: 0x040032D0 RID: 13008
		private string comments;

		// Token: 0x040032D1 RID: 13009
		private string legalTrademarks;

		// Token: 0x040032D2 RID: 13010
		private string privateBuild;

		// Token: 0x040032D3 RID: 13011
		private string specialBuild;

		// Token: 0x040032D4 RID: 13012
		private string language;

		// Token: 0x040032D5 RID: 13013
		private int fileMajor;

		// Token: 0x040032D6 RID: 13014
		private int fileMinor;

		// Token: 0x040032D7 RID: 13015
		private int fileBuild;

		// Token: 0x040032D8 RID: 13016
		private int filePrivate;

		// Token: 0x040032D9 RID: 13017
		private int productMajor;

		// Token: 0x040032DA RID: 13018
		private int productMinor;

		// Token: 0x040032DB RID: 13019
		private int productBuild;

		// Token: 0x040032DC RID: 13020
		private int productPrivate;

		// Token: 0x040032DD RID: 13021
		private int fileFlags;
	}
}

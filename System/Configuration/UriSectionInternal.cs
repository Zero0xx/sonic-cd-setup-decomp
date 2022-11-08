using System;
using System.Threading;

namespace System.Configuration
{
	// Token: 0x02000670 RID: 1648
	internal sealed class UriSectionInternal
	{
		// Token: 0x060032E7 RID: 13031 RVA: 0x000D78FE File Offset: 0x000D68FE
		internal UriSectionInternal(UriSection section)
		{
			this.idn = section.Idn.Enabled;
			this.iriParsing = section.IriParsing.Enabled;
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x060032E8 RID: 13032 RVA: 0x000D7928 File Offset: 0x000D6928
		internal UriIdnScope Idn
		{
			get
			{
				return this.idn;
			}
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x060032E9 RID: 13033 RVA: 0x000D7930 File Offset: 0x000D6930
		internal bool IriParsing
		{
			get
			{
				return this.iriParsing;
			}
		}

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x060032EA RID: 13034 RVA: 0x000D7938 File Offset: 0x000D6938
		internal static object ClassSyncObject
		{
			get
			{
				if (UriSectionInternal.classSyncObject == null)
				{
					Interlocked.CompareExchange(ref UriSectionInternal.classSyncObject, new object(), null);
				}
				return UriSectionInternal.classSyncObject;
			}
		}

		// Token: 0x060032EB RID: 13035 RVA: 0x000D7958 File Offset: 0x000D6958
		internal static UriSectionInternal GetSection()
		{
			UriSectionInternal result;
			lock (UriSectionInternal.ClassSyncObject)
			{
				UriSection uriSection = PrivilegedConfigurationManager.GetSection(CommonConfigurationStrings.UriSectionPath) as UriSection;
				if (uriSection == null)
				{
					result = null;
				}
				else
				{
					result = new UriSectionInternal(uriSection);
				}
			}
			return result;
		}

		// Token: 0x04002F70 RID: 12144
		private bool iriParsing;

		// Token: 0x04002F71 RID: 12145
		private UriIdnScope idn;

		// Token: 0x04002F72 RID: 12146
		private static object classSyncObject;
	}
}

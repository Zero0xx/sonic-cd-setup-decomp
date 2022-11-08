using System;

namespace System.Net
{
	// Token: 0x02000543 RID: 1347
	[Flags]
	internal enum Alg
	{
		// Token: 0x040027F8 RID: 10232
		Any = 0,
		// Token: 0x040027F9 RID: 10233
		ClassSignture = 8192,
		// Token: 0x040027FA RID: 10234
		ClassEncrypt = 24576,
		// Token: 0x040027FB RID: 10235
		ClassHash = 32768,
		// Token: 0x040027FC RID: 10236
		ClassKeyXch = 40960,
		// Token: 0x040027FD RID: 10237
		TypeRSA = 1024,
		// Token: 0x040027FE RID: 10238
		TypeBlock = 1536,
		// Token: 0x040027FF RID: 10239
		TypeStream = 2048,
		// Token: 0x04002800 RID: 10240
		TypeDH = 2560,
		// Token: 0x04002801 RID: 10241
		NameDES = 1,
		// Token: 0x04002802 RID: 10242
		NameRC2 = 2,
		// Token: 0x04002803 RID: 10243
		Name3DES = 3,
		// Token: 0x04002804 RID: 10244
		NameAES_128 = 14,
		// Token: 0x04002805 RID: 10245
		NameAES_192 = 15,
		// Token: 0x04002806 RID: 10246
		NameAES_256 = 16,
		// Token: 0x04002807 RID: 10247
		NameAES = 17,
		// Token: 0x04002808 RID: 10248
		NameRC4 = 1,
		// Token: 0x04002809 RID: 10249
		NameMD5 = 3,
		// Token: 0x0400280A RID: 10250
		NameSHA = 4,
		// Token: 0x0400280B RID: 10251
		NameDH_Ephem = 2
	}
}

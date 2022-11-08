using System;

namespace System.Security.AccessControl
{
	// Token: 0x020008F3 RID: 2291
	[Flags]
	public enum FileSystemRights
	{
		// Token: 0x04002AF5 RID: 10997
		ReadData = 1,
		// Token: 0x04002AF6 RID: 10998
		ListDirectory = 1,
		// Token: 0x04002AF7 RID: 10999
		WriteData = 2,
		// Token: 0x04002AF8 RID: 11000
		CreateFiles = 2,
		// Token: 0x04002AF9 RID: 11001
		AppendData = 4,
		// Token: 0x04002AFA RID: 11002
		CreateDirectories = 4,
		// Token: 0x04002AFB RID: 11003
		ReadExtendedAttributes = 8,
		// Token: 0x04002AFC RID: 11004
		WriteExtendedAttributes = 16,
		// Token: 0x04002AFD RID: 11005
		ExecuteFile = 32,
		// Token: 0x04002AFE RID: 11006
		Traverse = 32,
		// Token: 0x04002AFF RID: 11007
		DeleteSubdirectoriesAndFiles = 64,
		// Token: 0x04002B00 RID: 11008
		ReadAttributes = 128,
		// Token: 0x04002B01 RID: 11009
		WriteAttributes = 256,
		// Token: 0x04002B02 RID: 11010
		Delete = 65536,
		// Token: 0x04002B03 RID: 11011
		ReadPermissions = 131072,
		// Token: 0x04002B04 RID: 11012
		ChangePermissions = 262144,
		// Token: 0x04002B05 RID: 11013
		TakeOwnership = 524288,
		// Token: 0x04002B06 RID: 11014
		Synchronize = 1048576,
		// Token: 0x04002B07 RID: 11015
		FullControl = 2032127,
		// Token: 0x04002B08 RID: 11016
		Read = 131209,
		// Token: 0x04002B09 RID: 11017
		ReadAndExecute = 131241,
		// Token: 0x04002B0A RID: 11018
		Write = 278,
		// Token: 0x04002B0B RID: 11019
		Modify = 197055
	}
}

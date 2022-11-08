using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000545 RID: 1349
	[Guid("0000000f-0000-0000-C000-000000000046")]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IMoniker instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface UCOMIMoniker
	{
		// Token: 0x0600336C RID: 13164
		void GetClassID(out Guid pClassID);

		// Token: 0x0600336D RID: 13165
		[PreserveSig]
		int IsDirty();

		// Token: 0x0600336E RID: 13166
		void Load(UCOMIStream pStm);

		// Token: 0x0600336F RID: 13167
		void Save(UCOMIStream pStm, [MarshalAs(UnmanagedType.Bool)] bool fClearDirty);

		// Token: 0x06003370 RID: 13168
		void GetSizeMax(out long pcbSize);

		// Token: 0x06003371 RID: 13169
		void BindToObject(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, [In] ref Guid riidResult, [MarshalAs(UnmanagedType.Interface)] out object ppvResult);

		// Token: 0x06003372 RID: 13170
		void BindToStorage(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObj);

		// Token: 0x06003373 RID: 13171
		void Reduce(UCOMIBindCtx pbc, int dwReduceHowFar, ref UCOMIMoniker ppmkToLeft, out UCOMIMoniker ppmkReduced);

		// Token: 0x06003374 RID: 13172
		void ComposeWith(UCOMIMoniker pmkRight, [MarshalAs(UnmanagedType.Bool)] bool fOnlyIfNotGeneric, out UCOMIMoniker ppmkComposite);

		// Token: 0x06003375 RID: 13173
		void Enum([MarshalAs(UnmanagedType.Bool)] bool fForward, out UCOMIEnumMoniker ppenumMoniker);

		// Token: 0x06003376 RID: 13174
		void IsEqual(UCOMIMoniker pmkOtherMoniker);

		// Token: 0x06003377 RID: 13175
		void Hash(out int pdwHash);

		// Token: 0x06003378 RID: 13176
		void IsRunning(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, UCOMIMoniker pmkNewlyRunning);

		// Token: 0x06003379 RID: 13177
		void GetTimeOfLastChange(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, out FILETIME pFileTime);

		// Token: 0x0600337A RID: 13178
		void Inverse(out UCOMIMoniker ppmk);

		// Token: 0x0600337B RID: 13179
		void CommonPrefixWith(UCOMIMoniker pmkOther, out UCOMIMoniker ppmkPrefix);

		// Token: 0x0600337C RID: 13180
		void RelativePathTo(UCOMIMoniker pmkOther, out UCOMIMoniker ppmkRelPath);

		// Token: 0x0600337D RID: 13181
		void GetDisplayName(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplayName);

		// Token: 0x0600337E RID: 13182
		void ParseDisplayName(UCOMIBindCtx pbc, UCOMIMoniker pmkToLeft, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, out int pchEaten, out UCOMIMoniker ppmkOut);

		// Token: 0x0600337F RID: 13183
		void IsSystemMoniker(out int pdwMksys);
	}
}

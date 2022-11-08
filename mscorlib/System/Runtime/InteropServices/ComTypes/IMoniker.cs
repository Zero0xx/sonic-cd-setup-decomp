using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000578 RID: 1400
	[Guid("0000000f-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IMoniker
	{
		// Token: 0x060033F4 RID: 13300
		void GetClassID(out Guid pClassID);

		// Token: 0x060033F5 RID: 13301
		[PreserveSig]
		int IsDirty();

		// Token: 0x060033F6 RID: 13302
		void Load(IStream pStm);

		// Token: 0x060033F7 RID: 13303
		void Save(IStream pStm, [MarshalAs(UnmanagedType.Bool)] bool fClearDirty);

		// Token: 0x060033F8 RID: 13304
		void GetSizeMax(out long pcbSize);

		// Token: 0x060033F9 RID: 13305
		void BindToObject(IBindCtx pbc, IMoniker pmkToLeft, [In] ref Guid riidResult, [MarshalAs(UnmanagedType.Interface)] out object ppvResult);

		// Token: 0x060033FA RID: 13306
		void BindToStorage(IBindCtx pbc, IMoniker pmkToLeft, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObj);

		// Token: 0x060033FB RID: 13307
		void Reduce(IBindCtx pbc, int dwReduceHowFar, ref IMoniker ppmkToLeft, out IMoniker ppmkReduced);

		// Token: 0x060033FC RID: 13308
		void ComposeWith(IMoniker pmkRight, [MarshalAs(UnmanagedType.Bool)] bool fOnlyIfNotGeneric, out IMoniker ppmkComposite);

		// Token: 0x060033FD RID: 13309
		void Enum([MarshalAs(UnmanagedType.Bool)] bool fForward, out IEnumMoniker ppenumMoniker);

		// Token: 0x060033FE RID: 13310
		[PreserveSig]
		int IsEqual(IMoniker pmkOtherMoniker);

		// Token: 0x060033FF RID: 13311
		void Hash(out int pdwHash);

		// Token: 0x06003400 RID: 13312
		[PreserveSig]
		int IsRunning(IBindCtx pbc, IMoniker pmkToLeft, IMoniker pmkNewlyRunning);

		// Token: 0x06003401 RID: 13313
		void GetTimeOfLastChange(IBindCtx pbc, IMoniker pmkToLeft, out FILETIME pFileTime);

		// Token: 0x06003402 RID: 13314
		void Inverse(out IMoniker ppmk);

		// Token: 0x06003403 RID: 13315
		void CommonPrefixWith(IMoniker pmkOther, out IMoniker ppmkPrefix);

		// Token: 0x06003404 RID: 13316
		void RelativePathTo(IMoniker pmkOther, out IMoniker ppmkRelPath);

		// Token: 0x06003405 RID: 13317
		void GetDisplayName(IBindCtx pbc, IMoniker pmkToLeft, [MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplayName);

		// Token: 0x06003406 RID: 13318
		void ParseDisplayName(IBindCtx pbc, IMoniker pmkToLeft, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, out int pchEaten, out IMoniker ppmkOut);

		// Token: 0x06003407 RID: 13319
		[PreserveSig]
		int IsSystemMoniker(out int pdwMksys);
	}
}

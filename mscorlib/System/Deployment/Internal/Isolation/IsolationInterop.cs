using System;
using System.Deployment.Internal.Isolation.Manifest;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000242 RID: 578
	internal static class IsolationInterop
	{
		// Token: 0x0600160C RID: 5644 RVA: 0x0003820A File Offset: 0x0003720A
		public static Store GetUserStore()
		{
			return new Store(IsolationInterop.GetUserStore(0U, IntPtr.Zero, ref IsolationInterop.IID_IStore) as IStore);
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x0600160D RID: 5645 RVA: 0x00038228 File Offset: 0x00037228
		public static IIdentityAuthority IdentityAuthority
		{
			get
			{
				if (IsolationInterop._idAuth == null)
				{
					lock (IsolationInterop._synchObject)
					{
						if (IsolationInterop._idAuth == null)
						{
							IsolationInterop._idAuth = IsolationInterop.GetIdentityAuthority();
						}
					}
				}
				return IsolationInterop._idAuth;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x0600160E RID: 5646 RVA: 0x00038278 File Offset: 0x00037278
		public static IAppIdAuthority AppIdAuthority
		{
			get
			{
				if (IsolationInterop._appIdAuth == null)
				{
					lock (IsolationInterop._synchObject)
					{
						if (IsolationInterop._appIdAuth == null)
						{
							IsolationInterop._appIdAuth = IsolationInterop.GetAppIdAuthority();
						}
					}
				}
				return IsolationInterop._appIdAuth;
			}
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x000382C8 File Offset: 0x000372C8
		internal static IActContext CreateActContext(IDefinitionAppId AppId)
		{
			IsolationInterop.CreateActContextParameters createActContextParameters;
			createActContextParameters.Size = (uint)Marshal.SizeOf(typeof(IsolationInterop.CreateActContextParameters));
			createActContextParameters.Flags = 16U;
			createActContextParameters.CustomStoreList = IntPtr.Zero;
			createActContextParameters.CultureFallbackList = IntPtr.Zero;
			createActContextParameters.ProcessorArchitectureList = IntPtr.Zero;
			createActContextParameters.Source = IntPtr.Zero;
			createActContextParameters.ProcArch = 0;
			IsolationInterop.CreateActContextParametersSource createActContextParametersSource;
			createActContextParametersSource.Size = (uint)Marshal.SizeOf(typeof(IsolationInterop.CreateActContextParametersSource));
			createActContextParametersSource.Flags = 0U;
			createActContextParametersSource.SourceType = 1U;
			createActContextParametersSource.Data = IntPtr.Zero;
			IsolationInterop.CreateActContextParametersSourceDefinitionAppid createActContextParametersSourceDefinitionAppid;
			createActContextParametersSourceDefinitionAppid.Size = (uint)Marshal.SizeOf(typeof(IsolationInterop.CreateActContextParametersSourceDefinitionAppid));
			createActContextParametersSourceDefinitionAppid.Flags = 0U;
			createActContextParametersSourceDefinitionAppid.AppId = AppId;
			IActContext result;
			try
			{
				createActContextParametersSource.Data = createActContextParametersSourceDefinitionAppid.ToIntPtr();
				createActContextParameters.Source = createActContextParametersSource.ToIntPtr();
				result = (IsolationInterop.CreateActContext(ref createActContextParameters) as IActContext);
			}
			finally
			{
				if (createActContextParametersSource.Data != IntPtr.Zero)
				{
					IsolationInterop.CreateActContextParametersSourceDefinitionAppid.Destroy(createActContextParametersSource.Data);
					createActContextParametersSource.Data = IntPtr.Zero;
				}
				if (createActContextParameters.Source != IntPtr.Zero)
				{
					IsolationInterop.CreateActContextParametersSource.Destroy(createActContextParameters.Source);
					createActContextParameters.Source = IntPtr.Zero;
				}
			}
			return result;
		}

		// Token: 0x06001610 RID: 5648
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object CreateActContext(ref IsolationInterop.CreateActContextParameters Params);

		// Token: 0x06001611 RID: 5649
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object CreateCMSFromXml([In] byte[] buffer, [In] uint bufferSize, [In] IManifestParseErrorCallback Callback, [In] ref Guid riid);

		// Token: 0x06001612 RID: 5650
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object ParseManifest([MarshalAs(UnmanagedType.LPWStr)] [In] string pszManifestPath, [In] IManifestParseErrorCallback pIManifestParseErrorCallback, [In] ref Guid riid);

		// Token: 0x06001613 RID: 5651
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		private static extern object GetUserStore([In] uint Flags, [In] IntPtr hToken, [In] ref Guid riid);

		// Token: 0x06001614 RID: 5652
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Interface)]
		private static extern IIdentityAuthority GetIdentityAuthority();

		// Token: 0x06001615 RID: 5653
		[DllImport("mscorwks.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Interface)]
		private static extern IAppIdAuthority GetAppIdAuthority();

		// Token: 0x06001616 RID: 5654 RVA: 0x00038418 File Offset: 0x00037418
		internal static Guid GetGuidOfType(Type type)
		{
			GuidAttribute guidAttribute = (GuidAttribute)Attribute.GetCustomAttribute(type, typeof(GuidAttribute), false);
			return new Guid(guidAttribute.Value);
		}

		// Token: 0x0400092A RID: 2346
		public const string IsolationDllName = "mscorwks.dll";

		// Token: 0x0400092B RID: 2347
		private static object _synchObject = new object();

		// Token: 0x0400092C RID: 2348
		private static IIdentityAuthority _idAuth = null;

		// Token: 0x0400092D RID: 2349
		private static IAppIdAuthority _appIdAuth = null;

		// Token: 0x0400092E RID: 2350
		public static Guid IID_ICMS = IsolationInterop.GetGuidOfType(typeof(ICMS));

		// Token: 0x0400092F RID: 2351
		public static Guid IID_IDefinitionIdentity = IsolationInterop.GetGuidOfType(typeof(IDefinitionIdentity));

		// Token: 0x04000930 RID: 2352
		public static Guid IID_IManifestInformation = IsolationInterop.GetGuidOfType(typeof(IManifestInformation));

		// Token: 0x04000931 RID: 2353
		public static Guid IID_IEnumSTORE_ASSEMBLY = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY));

		// Token: 0x04000932 RID: 2354
		public static Guid IID_IEnumSTORE_ASSEMBLY_FILE = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_ASSEMBLY_FILE));

		// Token: 0x04000933 RID: 2355
		public static Guid IID_IEnumSTORE_CATEGORY = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY));

		// Token: 0x04000934 RID: 2356
		public static Guid IID_IEnumSTORE_CATEGORY_INSTANCE = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_CATEGORY_INSTANCE));

		// Token: 0x04000935 RID: 2357
		public static Guid IID_IEnumSTORE_DEPLOYMENT_METADATA = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_DEPLOYMENT_METADATA));

		// Token: 0x04000936 RID: 2358
		public static Guid IID_IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY = IsolationInterop.GetGuidOfType(typeof(IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY));

		// Token: 0x04000937 RID: 2359
		public static Guid IID_IStore = IsolationInterop.GetGuidOfType(typeof(IStore));

		// Token: 0x04000938 RID: 2360
		public static Guid GUID_SXS_INSTALL_REFERENCE_SCHEME_OPAQUESTRING = new Guid("2ec93463-b0c3-45e1-8364-327e96aea856");

		// Token: 0x04000939 RID: 2361
		public static Guid SXS_INSTALL_REFERENCE_SCHEME_SXS_STRONGNAME_SIGNED_PRIVATE_ASSEMBLY = new Guid("3ab20ac0-67e8-4512-8385-a487e35df3da");

		// Token: 0x02000243 RID: 579
		internal struct CreateActContextParameters
		{
			// Token: 0x0400093A RID: 2362
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x0400093B RID: 2363
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x0400093C RID: 2364
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr CustomStoreList;

			// Token: 0x0400093D RID: 2365
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr CultureFallbackList;

			// Token: 0x0400093E RID: 2366
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr ProcessorArchitectureList;

			// Token: 0x0400093F RID: 2367
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr Source;

			// Token: 0x04000940 RID: 2368
			[MarshalAs(UnmanagedType.U2)]
			public ushort ProcArch;

			// Token: 0x02000244 RID: 580
			[Flags]
			public enum CreateFlags
			{
				// Token: 0x04000942 RID: 2370
				Nothing = 0,
				// Token: 0x04000943 RID: 2371
				StoreListValid = 1,
				// Token: 0x04000944 RID: 2372
				CultureListValid = 2,
				// Token: 0x04000945 RID: 2373
				ProcessorFallbackListValid = 4,
				// Token: 0x04000946 RID: 2374
				ProcessorValid = 8,
				// Token: 0x04000947 RID: 2375
				SourceValid = 16,
				// Token: 0x04000948 RID: 2376
				IgnoreVisibility = 32
			}
		}

		// Token: 0x02000245 RID: 581
		internal struct CreateActContextParametersSource
		{
			// Token: 0x06001618 RID: 5656 RVA: 0x00038554 File Offset: 0x00037554
			public IntPtr ToIntPtr()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
				Marshal.StructureToPtr(this, intPtr, false);
				return intPtr;
			}

			// Token: 0x06001619 RID: 5657 RVA: 0x0003858A File Offset: 0x0003758A
			public static void Destroy(IntPtr p)
			{
				Marshal.DestroyStructure(p, typeof(IsolationInterop.CreateActContextParametersSource));
				Marshal.FreeCoTaskMem(p);
			}

			// Token: 0x04000949 RID: 2377
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x0400094A RID: 2378
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x0400094B RID: 2379
			[MarshalAs(UnmanagedType.U4)]
			public uint SourceType;

			// Token: 0x0400094C RID: 2380
			[MarshalAs(UnmanagedType.SysInt)]
			public IntPtr Data;

			// Token: 0x02000246 RID: 582
			[Flags]
			public enum SourceFlags
			{
				// Token: 0x0400094E RID: 2382
				Definition = 1,
				// Token: 0x0400094F RID: 2383
				Reference = 2
			}
		}

		// Token: 0x02000247 RID: 583
		internal struct CreateActContextParametersSourceDefinitionAppid
		{
			// Token: 0x0600161A RID: 5658 RVA: 0x000385A4 File Offset: 0x000375A4
			public IntPtr ToIntPtr()
			{
				IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(this));
				Marshal.StructureToPtr(this, intPtr, false);
				return intPtr;
			}

			// Token: 0x0600161B RID: 5659 RVA: 0x000385DA File Offset: 0x000375DA
			public static void Destroy(IntPtr p)
			{
				Marshal.DestroyStructure(p, typeof(IsolationInterop.CreateActContextParametersSourceDefinitionAppid));
				Marshal.FreeCoTaskMem(p);
			}

			// Token: 0x04000950 RID: 2384
			[MarshalAs(UnmanagedType.U4)]
			public uint Size;

			// Token: 0x04000951 RID: 2385
			[MarshalAs(UnmanagedType.U4)]
			public uint Flags;

			// Token: 0x04000952 RID: 2386
			public IDefinitionAppId AppId;
		}
	}
}

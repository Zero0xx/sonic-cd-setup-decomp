using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.PrivateImplementationDetails;

// Token: 0x02000001 RID: 1
internal class <Module>
{
	// Token: 0x06000001 RID: 1 RVA: 0x0005A264 File Offset: 0x00059664
	public unsafe static Microsoft.DirectX.PrivateImplementationDetails.D3DXFLOAT16* __ctor(Microsoft.DirectX.PrivateImplementationDetails.D3DXFLOAT16* A_0, Microsoft.DirectX.PrivateImplementationDetails.D3DXFLOAT16* __unnamed000)
	{
		*A_0 = (short)(*__unnamed000);
		return A_0;
	}

	// Token: 0x06000002 RID: 2 RVA: 0x0005A288 File Offset: 0x00059688
	public unsafe static Microsoft.DirectX.PrivateImplementationDetails.D3DXCOLOR* __ctor(Microsoft.DirectX.PrivateImplementationDetails.D3DXCOLOR* A_0, float r, float g, float b, float a)
	{
		*A_0 = r;
		*(A_0 + 4) = g;
		*(A_0 + 8) = b;
		*(A_0 + 12) = a;
		return A_0;
	}

	// Token: 0x06000003 RID: 3 RVA: 0x0005A2BC File Offset: 0x000596BC
	public unsafe static uint K(Microsoft.DirectX.PrivateImplementationDetails.D3DXCOLOR* A_0)
	{
		uint num;
		if (*A_0 >= 1f)
		{
			num = 255U;
		}
		else if (*A_0 <= 0f)
		{
			num = 0U;
		}
		else
		{
			num = (uint)((double)(*A_0 * 255f + 0.5f));
		}
		uint num2;
		if (*(A_0 + 4) >= 1f)
		{
			num2 = 255U;
		}
		else if (*(A_0 + 4) <= 0f)
		{
			num2 = 0U;
		}
		else
		{
			num2 = (uint)((double)(*(A_0 + 4) * 255f + 0.5f));
		}
		uint num3;
		if (*(A_0 + 8) >= 1f)
		{
			num3 = 255U;
		}
		else if (*(A_0 + 8) <= 0f)
		{
			num3 = 0U;
		}
		else
		{
			num3 = (uint)((double)(*(A_0 + 8) * 255f + 0.5f));
		}
		uint num4;
		if (*(A_0 + 12) >= 1f)
		{
			num4 = 255U;
		}
		else if (*(A_0 + 12) <= 0f)
		{
			num4 = 0U;
		}
		else
		{
			num4 = (uint)((double)(*(A_0 + 12) * 255f + 0.5f));
		}
		return ((num4 << 8 | num) << 8 | num2) << 8 | num3;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000692B0 File Offset: 0x000686B0
	public unsafe static int ?CreateDirect3DDevice@@$$FYGJP$AAVDevice@Direct3D@DirectX@Microsoft@@KW4_D3DDEVTYPE@PrivateImplementationDetails@34@PAUHWND__@634@KP$01AP$AAVPresentParameters@234@PAPAUIDirect3DDevice9@634@@Z(Device pDevice, uint dwAdapterOrdinal, int deviceType, HWND__* windowHandle, uint dwFlags, PresentParameters[] presentationParameters, IDirect3DDevice9** ppRet)
	{
		Debug.Assert(pDevice != null, "Creating Device without parent object created isn't allowed.");
		bool flag = false;
		bool flag2 = (dwFlags >> 2 & 1) != null;
		_D3DPRESENT_PARAMETERS_* ptr;
		if (presentationParameters.Length == 1)
		{
			ptr = presentationParameters[0].RealStruct;
			if (!presentationParameters[0].ForceNoMultiThreadedFlag && !flag2)
			{
				dwFlags |= 4;
			}
		}
		else
		{
			flag = true;
			ptr = <Module>.@new((uint)(presentationParameters.Length * 56));
			int num = 0;
			if (0 < presentationParameters.Length)
			{
				_D3DPRESENT_PARAMETERS_* ptr2 = ptr;
				do
				{
					_D3DPRESENT_PARAMETERS_* realStruct = presentationParameters[num].RealStruct;
					cpblk(ptr2, realStruct, 56);
					if (!presentationParameters[num].ForceNoMultiThreadedFlag && !flag2)
					{
						dwFlags |= 4;
					}
					num++;
					ptr2 += 56 / sizeof(_D3DPRESENT_PARAMETERS_);
				}
				while (num < presentationParameters.Length);
			}
		}
		int num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.HWND__*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),Microsoft.DirectX.PrivateImplementationDetails._D3DPRESENT_PARAMETERS_*,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DDevice9**), Manager.m_lpUM, dwAdapterOrdinal, deviceType, windowHandle, dwFlags, ptr, ppRet, *(*(int*)Manager.m_lpUM + 64));
		if (flag)
		{
			int num3 = 0;
			if (0 < presentationParameters.Length)
			{
				_D3DPRESENT_PARAMETERS_* ptr3 = ptr;
				do
				{
					cpblk(presentationParameters[num3].RealStruct, ptr3, 56);
					num3++;
					ptr3 += 56 / sizeof(_D3DPRESENT_PARAMETERS_);
				}
				while (num3 < presentationParameters.Length);
			}
			if (ptr != null)
			{
				<Module>.delete((void*)ptr);
			}
		}
		if (num2 >= 0)
		{
			pDevice.CreateHelperClasses();
		}
		return num2;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00073702 File Offset: 0x00072B02
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern int memcmp(void*, void*, uint);

	// Token: 0x06000006 RID: 6 RVA: 0x000736D2 File Offset: 0x00072AD2
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public static extern void SetLastError(uint);

	// Token: 0x06000007 RID: 7 RVA: 0x00073792 File Offset: 0x00072B92
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern IDirect3D9* Direct3DCreate9(uint);

	// Token: 0x06000008 RID: 8 RVA: 0x000736DE File Offset: 0x00072ADE
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern int FreeLibrary(HINSTANCE__*);

	// Token: 0x06000009 RID: 9 RVA: 0x000736EA File Offset: 0x00072AEA
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern method GetProcAddress(HINSTANCE__*, sbyte*);

	// Token: 0x0600000A RID: 10 RVA: 0x000736F6 File Offset: 0x00072AF6
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern HINSTANCE__* LoadLibraryA(sbyte*);

	// Token: 0x0600000B RID: 11 RVA: 0x0007370E File Offset: 0x00072B0E
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern void delete(void*);

	// Token: 0x0600000C RID: 12 RVA: 0x0007371A File Offset: 0x00072B1A
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern void* @new(uint);

	// Token: 0x0600000D RID: 13 RVA: 0x0007374A File Offset: 0x00072B4A
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern int DeleteDC(HDC__*);

	// Token: 0x0600000E RID: 14 RVA: 0x00073726 File Offset: 0x00072B26
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern int ReleaseDC(HWND__*, HDC__*);

	// Token: 0x0600000F RID: 15 RVA: 0x00073756 File Offset: 0x00072B56
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern int DeleteObject(void*);

	// Token: 0x06000010 RID: 16 RVA: 0x00073762 File Offset: 0x00072B62
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern int GetDIBits(HDC__*, HBITMAP__*, uint, uint, void*, tagBITMAPINFO*, uint);

	// Token: 0x06000011 RID: 17 RVA: 0x0007376E File Offset: 0x00072B6E
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern void* SelectObject(HDC__*, void*);

	// Token: 0x06000012 RID: 18 RVA: 0x0007377A File Offset: 0x00072B7A
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern HDC__* CreateCompatibleDC(HDC__*);

	// Token: 0x06000013 RID: 19 RVA: 0x00073732 File Offset: 0x00072B32
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern HDC__* GetDC(HWND__*);

	// Token: 0x06000014 RID: 20 RVA: 0x00073786 File Offset: 0x00072B86
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern int GetObjectA(void*, int, void*);

	// Token: 0x06000015 RID: 21 RVA: 0x0007373E File Offset: 0x00072B3E
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern int GetIconInfo(HICON__*, _ICONINFO*);

	// Token: 0x06000016 RID: 22 RVA: 0x000735E6 File Offset: 0x000729E6
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	public unsafe static extern int __DllMainCRTStartupForGS@12(void*, uint, void*);

	// Token: 0x04000001 RID: 1 RVA: 0x00001AAC File Offset: 0x00000EAC
	public static $ArrayType$0x38729158 ??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@;

	// Token: 0x04000002 RID: 2 RVA: 0x000015F0 File Offset: 0x000009F0
	public static uint $ConstGCArrayBound$0x092d518d$7$;

	// Token: 0x04000003 RID: 3 RVA: 0x00001578 File Offset: 0x00000978
	public static uint $ConstGCArrayBound$0x092d518d$37$;

	// Token: 0x04000004 RID: 4 RVA: 0x0000152C File Offset: 0x0000092C
	public static uint $ConstGCArrayBound$0x092d518d$56$;

	// Token: 0x04000005 RID: 5 RVA: 0x00001550 File Offset: 0x00000950
	public static uint $ConstGCArrayBound$0x092d518d$47$;

	// Token: 0x04000006 RID: 6 RVA: 0x000014BC File Offset: 0x000008BC
	public static uint $ConstGCArrayBound$0x092d518d$84$;

	// Token: 0x04000007 RID: 7 RVA: 0x0000149C File Offset: 0x0000089C
	public static uint $ConstGCArrayBound$0x092d518d$92$;

	// Token: 0x04000008 RID: 8 RVA: 0x000014A8 File Offset: 0x000008A8
	public static uint $ConstGCArrayBound$0x092d518d$89$;

	// Token: 0x04000009 RID: 9 RVA: 0x000014F4 File Offset: 0x000008F4
	public static uint $ConstGCArrayBound$0x092d518d$70$;

	// Token: 0x0400000A RID: 10 RVA: 0x000014B0 File Offset: 0x000008B0
	public static uint $ConstGCArrayBound$0x092d518d$87$;

	// Token: 0x0400000B RID: 11 RVA: 0x000015D0 File Offset: 0x000009D0
	public static uint $ConstGCArrayBound$0x092d518d$15$;

	// Token: 0x0400000C RID: 12 RVA: 0x0000140C File Offset: 0x0000080C
	public static uint $ConstGCArrayBound$0x092d518d$128$;

	// Token: 0x0400000D RID: 13 RVA: 0x000014E0 File Offset: 0x000008E0
	public static uint $ConstGCArrayBound$0x092d518d$75$;

	// Token: 0x0400000E RID: 14 RVA: 0x000014D4 File Offset: 0x000008D4
	public static uint $ConstGCArrayBound$0x092d518d$78$;

	// Token: 0x0400000F RID: 15 RVA: 0x00001520 File Offset: 0x00000920
	public static uint $ConstGCArrayBound$0x092d518d$59$;

	// Token: 0x04000010 RID: 16 RVA: 0x0000145C File Offset: 0x0000085C
	public static uint $ConstGCArrayBound$0x092d518d$108$;

	// Token: 0x04000011 RID: 17 RVA: 0x0000158C File Offset: 0x0000098C
	public static uint $ConstGCArrayBound$0x092d518d$32$;

	// Token: 0x04000012 RID: 18 RVA: 0x000014AC File Offset: 0x000008AC
	public static uint $ConstGCArrayBound$0x092d518d$88$;

	// Token: 0x04000013 RID: 19 RVA: 0x000015C4 File Offset: 0x000009C4
	public static uint $ConstGCArrayBound$0x092d518d$18$;

	// Token: 0x04000014 RID: 20 RVA: 0x00001444 File Offset: 0x00000844
	public static uint $ConstGCArrayBound$0x092d518d$114$;

	// Token: 0x04000015 RID: 21 RVA: 0x00001478 File Offset: 0x00000878
	public static uint $ConstGCArrayBound$0x092d518d$101$;

	// Token: 0x04000016 RID: 22 RVA: 0x0000150C File Offset: 0x0000090C
	public static uint $ConstGCArrayBound$0x092d518d$64$;

	// Token: 0x04000017 RID: 23 RVA: 0x000014EC File Offset: 0x000008EC
	public static uint $ConstGCArrayBound$0x092d518d$72$;

	// Token: 0x04000018 RID: 24 RVA: 0x00001600 File Offset: 0x00000A00
	public static uint $ConstGCArrayBound$0x092d518d$3$;

	// Token: 0x04000019 RID: 25 RVA: 0x00001488 File Offset: 0x00000888
	public static uint $ConstGCArrayBound$0x092d518d$97$;

	// Token: 0x0400001A RID: 26 RVA: 0x00001544 File Offset: 0x00000944
	public static uint $ConstGCArrayBound$0x092d518d$50$;

	// Token: 0x0400001B RID: 27 RVA: 0x000015A4 File Offset: 0x000009A4
	public static uint $ConstGCArrayBound$0x092d518d$26$;

	// Token: 0x0400001C RID: 28 RVA: 0x00001458 File Offset: 0x00000858
	public static uint $ConstGCArrayBound$0x092d518d$109$;

	// Token: 0x0400001D RID: 29 RVA: 0x00001490 File Offset: 0x00000890
	public static uint $ConstGCArrayBound$0x092d518d$95$;

	// Token: 0x0400001E RID: 30 RVA: 0x00001540 File Offset: 0x00000940
	public static uint $ConstGCArrayBound$0x092d518d$51$;

	// Token: 0x0400001F RID: 31 RVA: 0x000014E8 File Offset: 0x000008E8
	public static uint $ConstGCArrayBound$0x092d518d$73$;

	// Token: 0x04000020 RID: 32 RVA: 0x00001448 File Offset: 0x00000848
	public static uint $ConstGCArrayBound$0x092d518d$113$;

	// Token: 0x04000021 RID: 33 RVA: 0x000015AC File Offset: 0x000009AC
	public static uint $ConstGCArrayBound$0x092d518d$24$;

	// Token: 0x04000022 RID: 34 RVA: 0x00001440 File Offset: 0x00000840
	public static uint $ConstGCArrayBound$0x092d518d$115$;

	// Token: 0x04000023 RID: 35 RVA: 0x00001410 File Offset: 0x00000810
	public static uint $ConstGCArrayBound$0x092d518d$127$;

	// Token: 0x04000024 RID: 36 RVA: 0x000014D0 File Offset: 0x000008D0
	public static uint $ConstGCArrayBound$0x092d518d$79$;

	// Token: 0x04000025 RID: 37 RVA: 0x00001468 File Offset: 0x00000868
	public static uint $ConstGCArrayBound$0x092d518d$105$;

	// Token: 0x04000026 RID: 38 RVA: 0x00001528 File Offset: 0x00000928
	public static uint $ConstGCArrayBound$0x092d518d$57$;

	// Token: 0x04000027 RID: 39 RVA: 0x00001434 File Offset: 0x00000834
	public static uint $ConstGCArrayBound$0x092d518d$118$;

	// Token: 0x04000028 RID: 40 RVA: 0x00001588 File Offset: 0x00000988
	public static uint $ConstGCArrayBound$0x092d518d$33$;

	// Token: 0x04000029 RID: 41 RVA: 0x00001420 File Offset: 0x00000820
	public static uint $ConstGCArrayBound$0x092d518d$123$;

	// Token: 0x0400002A RID: 42 RVA: 0x00001454 File Offset: 0x00000854
	public static uint $ConstGCArrayBound$0x092d518d$110$;

	// Token: 0x0400002B RID: 43 RVA: 0x00001594 File Offset: 0x00000994
	public static uint $ConstGCArrayBound$0x092d518d$30$;

	// Token: 0x0400002C RID: 44 RVA: 0x000015F4 File Offset: 0x000009F4
	public static uint $ConstGCArrayBound$0x092d518d$6$;

	// Token: 0x0400002D RID: 45 RVA: 0x00001604 File Offset: 0x00000A04
	public static uint $ConstGCArrayBound$0x092d518d$2$;

	// Token: 0x0400002E RID: 46 RVA: 0x000014DC File Offset: 0x000008DC
	public static uint $ConstGCArrayBound$0x092d518d$76$;

	// Token: 0x0400002F RID: 47 RVA: 0x000015B4 File Offset: 0x000009B4
	public static uint $ConstGCArrayBound$0x092d518d$22$;

	// Token: 0x04000030 RID: 48 RVA: 0x000014C4 File Offset: 0x000008C4
	public static uint $ConstGCArrayBound$0x092d518d$82$;

	// Token: 0x04000031 RID: 49 RVA: 0x00001500 File Offset: 0x00000900
	public static uint $ConstGCArrayBound$0x092d518d$67$;

	// Token: 0x04000032 RID: 50 RVA: 0x000014CC File Offset: 0x000008CC
	public static uint $ConstGCArrayBound$0x092d518d$80$;

	// Token: 0x04000033 RID: 51 RVA: 0x000015E8 File Offset: 0x000009E8
	public static uint $ConstGCArrayBound$0x092d518d$9$;

	// Token: 0x04000034 RID: 52 RVA: 0x00001518 File Offset: 0x00000918
	public static uint $ConstGCArrayBound$0x092d518d$61$;

	// Token: 0x04000035 RID: 53 RVA: 0x0000153C File Offset: 0x0000093C
	public static uint $ConstGCArrayBound$0x092d518d$52$;

	// Token: 0x04000036 RID: 54 RVA: 0x00001418 File Offset: 0x00000818
	public static uint $ConstGCArrayBound$0x092d518d$125$;

	// Token: 0x04000037 RID: 55 RVA: 0x000015E4 File Offset: 0x000009E4
	public static uint $ConstGCArrayBound$0x092d518d$10$;

	// Token: 0x04000038 RID: 56 RVA: 0x00001538 File Offset: 0x00000938
	public static uint $ConstGCArrayBound$0x092d518d$53$;

	// Token: 0x04000039 RID: 57 RVA: 0x000015EC File Offset: 0x000009EC
	public static uint $ConstGCArrayBound$0x092d518d$8$;

	// Token: 0x0400003A RID: 58 RVA: 0x00001494 File Offset: 0x00000894
	public static uint $ConstGCArrayBound$0x092d518d$94$;

	// Token: 0x0400003B RID: 59 RVA: 0x00001438 File Offset: 0x00000838
	public static uint $ConstGCArrayBound$0x092d518d$117$;

	// Token: 0x0400003C RID: 60 RVA: 0x00001560 File Offset: 0x00000960
	public static uint $ConstGCArrayBound$0x092d518d$43$;

	// Token: 0x0400003D RID: 61 RVA: 0x00001570 File Offset: 0x00000970
	public static uint $ConstGCArrayBound$0x092d518d$39$;

	// Token: 0x0400003E RID: 62 RVA: 0x00001430 File Offset: 0x00000830
	public static uint $ConstGCArrayBound$0x092d518d$119$;

	// Token: 0x0400003F RID: 63 RVA: 0x000015B0 File Offset: 0x000009B0
	public static uint $ConstGCArrayBound$0x092d518d$23$;

	// Token: 0x04000040 RID: 64 RVA: 0x00001548 File Offset: 0x00000948
	public static uint $ConstGCArrayBound$0x092d518d$49$;

	// Token: 0x04000041 RID: 65 RVA: 0x00001428 File Offset: 0x00000828
	public static uint $ConstGCArrayBound$0x092d518d$121$;

	// Token: 0x04000042 RID: 66 RVA: 0x00001530 File Offset: 0x00000930
	public static uint $ConstGCArrayBound$0x092d518d$55$;

	// Token: 0x04000043 RID: 67 RVA: 0x000015D4 File Offset: 0x000009D4
	public static uint $ConstGCArrayBound$0x092d518d$14$;

	// Token: 0x04000044 RID: 68 RVA: 0x00001464 File Offset: 0x00000864
	public static uint $ConstGCArrayBound$0x092d518d$106$;

	// Token: 0x04000045 RID: 69 RVA: 0x00001480 File Offset: 0x00000880
	public static uint $ConstGCArrayBound$0x092d518d$99$;

	// Token: 0x04000046 RID: 70 RVA: 0x00001460 File Offset: 0x00000860
	public static uint $ConstGCArrayBound$0x092d518d$107$;

	// Token: 0x04000047 RID: 71 RVA: 0x00001590 File Offset: 0x00000990
	public static uint $ConstGCArrayBound$0x092d518d$31$;

	// Token: 0x04000048 RID: 72 RVA: 0x000015DC File Offset: 0x000009DC
	public static uint $ConstGCArrayBound$0x092d518d$12$;

	// Token: 0x04000049 RID: 73 RVA: 0x00001524 File Offset: 0x00000924
	public static uint $ConstGCArrayBound$0x092d518d$58$;

	// Token: 0x0400004A RID: 74 RVA: 0x000014D8 File Offset: 0x000008D8
	public static uint $ConstGCArrayBound$0x092d518d$77$;

	// Token: 0x0400004B RID: 75 RVA: 0x000014A4 File Offset: 0x000008A4
	public static uint $ConstGCArrayBound$0x092d518d$90$;

	// Token: 0x0400004C RID: 76 RVA: 0x000014C8 File Offset: 0x000008C8
	public static uint $ConstGCArrayBound$0x092d518d$81$;

	// Token: 0x0400004D RID: 77 RVA: 0x00001474 File Offset: 0x00000874
	public static uint $ConstGCArrayBound$0x092d518d$102$;

	// Token: 0x0400004E RID: 78 RVA: 0x00001574 File Offset: 0x00000974
	public static uint $ConstGCArrayBound$0x092d518d$38$;

	// Token: 0x0400004F RID: 79 RVA: 0x000015E0 File Offset: 0x000009E0
	public static uint $ConstGCArrayBound$0x092d518d$11$;

	// Token: 0x04000050 RID: 80 RVA: 0x000015F8 File Offset: 0x000009F8
	public static uint $ConstGCArrayBound$0x092d518d$5$;

	// Token: 0x04000051 RID: 81 RVA: 0x00001498 File Offset: 0x00000898
	public static uint $ConstGCArrayBound$0x092d518d$93$;

	// Token: 0x04000052 RID: 82 RVA: 0x000015BC File Offset: 0x000009BC
	public static uint $ConstGCArrayBound$0x092d518d$20$;

	// Token: 0x04000053 RID: 83 RVA: 0x00001414 File Offset: 0x00000814
	public static uint $ConstGCArrayBound$0x092d518d$126$;

	// Token: 0x04000054 RID: 84 RVA: 0x00001534 File Offset: 0x00000934
	public static uint $ConstGCArrayBound$0x092d518d$54$;

	// Token: 0x04000055 RID: 85 RVA: 0x00001558 File Offset: 0x00000958
	public static uint $ConstGCArrayBound$0x092d518d$45$;

	// Token: 0x04000056 RID: 86 RVA: 0x0000159C File Offset: 0x0000099C
	public static uint $ConstGCArrayBound$0x092d518d$28$;

	// Token: 0x04000057 RID: 87 RVA: 0x000015D8 File Offset: 0x000009D8
	public static uint $ConstGCArrayBound$0x092d518d$13$;

	// Token: 0x04000058 RID: 88 RVA: 0x0000155C File Offset: 0x0000095C
	public static uint $ConstGCArrayBound$0x092d518d$44$;

	// Token: 0x04000059 RID: 89 RVA: 0x000014E4 File Offset: 0x000008E4
	public static uint $ConstGCArrayBound$0x092d518d$74$;

	// Token: 0x0400005A RID: 90 RVA: 0x000015FC File Offset: 0x000009FC
	public static uint $ConstGCArrayBound$0x092d518d$4$;

	// Token: 0x0400005B RID: 91 RVA: 0x00001424 File Offset: 0x00000824
	public static uint $ConstGCArrayBound$0x092d518d$122$;

	// Token: 0x0400005C RID: 92 RVA: 0x000015C0 File Offset: 0x000009C0
	public static uint $ConstGCArrayBound$0x092d518d$19$;

	// Token: 0x0400005D RID: 93 RVA: 0x00001508 File Offset: 0x00000908
	public static uint $ConstGCArrayBound$0x092d518d$65$;

	// Token: 0x0400005E RID: 94 RVA: 0x0000147C File Offset: 0x0000087C
	public static uint $ConstGCArrayBound$0x092d518d$100$;

	// Token: 0x0400005F RID: 95 RVA: 0x00001568 File Offset: 0x00000968
	public static uint $ConstGCArrayBound$0x092d518d$41$;

	// Token: 0x04000060 RID: 96 RVA: 0x0000142C File Offset: 0x0000082C
	public static uint $ConstGCArrayBound$0x092d518d$120$;

	// Token: 0x04000061 RID: 97 RVA: 0x0000143C File Offset: 0x0000083C
	public static uint $ConstGCArrayBound$0x092d518d$116$;

	// Token: 0x04000062 RID: 98 RVA: 0x000014F0 File Offset: 0x000008F0
	public static uint $ConstGCArrayBound$0x092d518d$71$;

	// Token: 0x04000063 RID: 99 RVA: 0x00001598 File Offset: 0x00000998
	public static uint $ConstGCArrayBound$0x092d518d$29$;

	// Token: 0x04000064 RID: 100 RVA: 0x00001510 File Offset: 0x00000910
	public static uint $ConstGCArrayBound$0x092d518d$63$;

	// Token: 0x04000065 RID: 101 RVA: 0x00001470 File Offset: 0x00000870
	public static uint $ConstGCArrayBound$0x092d518d$103$;

	// Token: 0x04000066 RID: 102 RVA: 0x00001580 File Offset: 0x00000980
	public static uint $ConstGCArrayBound$0x092d518d$35$;

	// Token: 0x04000067 RID: 103 RVA: 0x000014A0 File Offset: 0x000008A0
	public static uint $ConstGCArrayBound$0x092d518d$91$;

	// Token: 0x04000068 RID: 104 RVA: 0x000014B8 File Offset: 0x000008B8
	public static uint $ConstGCArrayBound$0x092d518d$85$;

	// Token: 0x04000069 RID: 105 RVA: 0x0000148C File Offset: 0x0000088C
	public static uint $ConstGCArrayBound$0x092d518d$96$;

	// Token: 0x0400006A RID: 106 RVA: 0x0000154C File Offset: 0x0000094C
	public static uint $ConstGCArrayBound$0x092d518d$48$;

	// Token: 0x0400006B RID: 107 RVA: 0x000014F8 File Offset: 0x000008F8
	public static uint $ConstGCArrayBound$0x092d518d$69$;

	// Token: 0x0400006C RID: 108 RVA: 0x000015B8 File Offset: 0x000009B8
	public static uint $ConstGCArrayBound$0x092d518d$21$;

	// Token: 0x0400006D RID: 109 RVA: 0x00001504 File Offset: 0x00000904
	public static uint $ConstGCArrayBound$0x092d518d$66$;

	// Token: 0x0400006E RID: 110 RVA: 0x00001608 File Offset: 0x00000A08
	public static uint $ConstGCArrayBound$0x092d518d$1$;

	// Token: 0x0400006F RID: 111 RVA: 0x00001484 File Offset: 0x00000884
	public static uint $ConstGCArrayBound$0x092d518d$98$;

	// Token: 0x04000070 RID: 112 RVA: 0x000015A0 File Offset: 0x000009A0
	public static uint $ConstGCArrayBound$0x092d518d$27$;

	// Token: 0x04000071 RID: 113 RVA: 0x0000141C File Offset: 0x0000081C
	public static uint $ConstGCArrayBound$0x092d518d$124$;

	// Token: 0x04000072 RID: 114 RVA: 0x0000151C File Offset: 0x0000091C
	public static uint $ConstGCArrayBound$0x092d518d$60$;

	// Token: 0x04000073 RID: 115 RVA: 0x00001514 File Offset: 0x00000914
	public static uint $ConstGCArrayBound$0x092d518d$62$;

	// Token: 0x04000074 RID: 116 RVA: 0x000015CC File Offset: 0x000009CC
	public static uint $ConstGCArrayBound$0x092d518d$16$;

	// Token: 0x04000075 RID: 117 RVA: 0x000015C8 File Offset: 0x000009C8
	public static uint $ConstGCArrayBound$0x092d518d$17$;

	// Token: 0x04000076 RID: 118 RVA: 0x00001584 File Offset: 0x00000984
	public static uint $ConstGCArrayBound$0x092d518d$34$;

	// Token: 0x04000077 RID: 119 RVA: 0x0000156C File Offset: 0x0000096C
	public static uint $ConstGCArrayBound$0x092d518d$40$;

	// Token: 0x04000078 RID: 120 RVA: 0x00001450 File Offset: 0x00000850
	public static uint $ConstGCArrayBound$0x092d518d$111$;

	// Token: 0x04000079 RID: 121 RVA: 0x000015A8 File Offset: 0x000009A8
	public static uint $ConstGCArrayBound$0x092d518d$25$;

	// Token: 0x0400007A RID: 122 RVA: 0x0000146C File Offset: 0x0000086C
	public static uint $ConstGCArrayBound$0x092d518d$104$;

	// Token: 0x0400007B RID: 123 RVA: 0x0000157C File Offset: 0x0000097C
	public static uint $ConstGCArrayBound$0x092d518d$36$;

	// Token: 0x0400007C RID: 124 RVA: 0x00001564 File Offset: 0x00000964
	public static uint $ConstGCArrayBound$0x092d518d$42$;

	// Token: 0x0400007D RID: 125 RVA: 0x000014B4 File Offset: 0x000008B4
	public static uint $ConstGCArrayBound$0x092d518d$86$;

	// Token: 0x0400007E RID: 126 RVA: 0x0000144C File Offset: 0x0000084C
	public static uint $ConstGCArrayBound$0x092d518d$112$;

	// Token: 0x0400007F RID: 127 RVA: 0x000014C0 File Offset: 0x000008C0
	public static uint $ConstGCArrayBound$0x092d518d$83$;

	// Token: 0x04000080 RID: 128 RVA: 0x000014FC File Offset: 0x000008FC
	public static uint $ConstGCArrayBound$0x092d518d$68$;

	// Token: 0x04000081 RID: 129 RVA: 0x0000182C File Offset: 0x00000C2C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXPRTEngine;

	// Token: 0x04000082 RID: 130 RVA: 0x0000173C File Offset: 0x00000B3C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_EffectString;

	// Token: 0x04000083 RID: 131 RVA: 0x00001A0C File Offset: 0x00000E0C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DSurface9;

	// Token: 0x04000084 RID: 132 RVA: 0x000018BC File Offset: 0x00000CBC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXBaseMesh;

	// Token: 0x04000085 RID: 133 RVA: 0x0000172C File Offset: 0x00000B2C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_EffectDWord;

	// Token: 0x04000086 RID: 134 RVA: 0x000018CC File Offset: 0x00000CCC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXFileData;

	// Token: 0x04000087 RID: 135 RVA: 0x0000193C File Offset: 0x00000D3C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXRenderToSurface;

	// Token: 0x04000088 RID: 136 RVA: 0x0000160C File Offset: 0x00000A0C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXAnimationController;

	// Token: 0x04000089 RID: 137 RVA: 0x00001A5C File Offset: 0x00000E5C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DTexture9;

	// Token: 0x0400008A RID: 138 RVA: 0x00001A7C File Offset: 0x00000E7C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DResource9;

	// Token: 0x0400008B RID: 139 RVA: 0x000016FC File Offset: 0x00000AFC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_EffectParamDWord;

	// Token: 0x0400008C RID: 140 RVA: 0x000018FC File Offset: 0x00000CFC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXFileSaveObject;

	// Token: 0x0400008D RID: 141 RVA: 0x000017BC File Offset: 0x00000BBC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_PatchMesh9;

	// Token: 0x0400008E RID: 142 RVA: 0x0000191C File Offset: 0x00000D1C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXLine;

	// Token: 0x0400008F RID: 143 RVA: 0x0000176C File Offset: 0x00000B6C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_VertexElement;

	// Token: 0x04000090 RID: 144 RVA: 0x000017DC File Offset: 0x00000BDC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_Patch;

	// Token: 0x04000091 RID: 145 RVA: 0x0000186C File Offset: 0x00000C6C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXPatchMesh;

	// Token: 0x04000092 RID: 146 RVA: 0x0000187C File Offset: 0x00000C7C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXSkinInfo;

	// Token: 0x04000093 RID: 147 RVA: 0x0000162C File Offset: 0x00000A2C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXKeyframedAnimationSet;

	// Token: 0x04000094 RID: 148 RVA: 0x000019BC File Offset: 0x00000DBC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DPixelShader9;

	// Token: 0x04000095 RID: 149 RVA: 0x000016BC File Offset: 0x00000ABC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXConstantTable;

	// Token: 0x04000096 RID: 150 RVA: 0x000016EC File Offset: 0x00000AEC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_EffectInstance;

	// Token: 0x04000097 RID: 151 RVA: 0x000017FC File Offset: 0x00000BFC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_FaceAdjacency;

	// Token: 0x04000098 RID: 152 RVA: 0x0000192C File Offset: 0x00000D2C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXRenderToEnvMap;

	// Token: 0x04000099 RID: 153 RVA: 0x0000161C File Offset: 0x00000A1C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXCompressedAnimationSet;

	// Token: 0x0400009A RID: 154 RVA: 0x0000167C File Offset: 0x00000A7C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXBaseEffect;

	// Token: 0x0400009B RID: 155 RVA: 0x0000178C File Offset: 0x00000B8C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_PMVSplitRecord;

	// Token: 0x0400009C RID: 156 RVA: 0x0000164C File Offset: 0x00000A4C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXEffectCompiler;

	// Token: 0x0400009D RID: 157 RVA: 0x000016CC File Offset: 0x00000ACC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_CompressedAnimationSet;

	// Token: 0x0400009E RID: 158 RVA: 0x000017EC File Offset: 0x00000BEC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_SkinWeights;

	// Token: 0x0400009F RID: 159 RVA: 0x000018AC File Offset: 0x00000CAC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXMesh;

	// Token: 0x040000A0 RID: 160 RVA: 0x0000163C File Offset: 0x00000A3C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXAnimationSet;

	// Token: 0x040000A1 RID: 161 RVA: 0x0000168C File Offset: 0x00000A8C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXEffectPool;

	// Token: 0x040000A2 RID: 162 RVA: 0x000018EC File Offset: 0x00000CEC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXFileSaveData;

	// Token: 0x040000A3 RID: 163 RVA: 0x00001A3C File Offset: 0x00000E3C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DVolumeTexture9;

	// Token: 0x040000A4 RID: 164 RVA: 0x000019CC File Offset: 0x00000DCC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DVertexShader9;

	// Token: 0x040000A5 RID: 165 RVA: 0x00001A1C File Offset: 0x00000E1C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DIndexBuffer9;

	// Token: 0x040000A6 RID: 166 RVA: 0x0000196C File Offset: 0x00000D6C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXBuffer;

	// Token: 0x040000A7 RID: 167 RVA: 0x0000166C File Offset: 0x00000A6C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXEffectStateManager;

	// Token: 0x040000A8 RID: 168 RVA: 0x0000198C File Offset: 0x00000D8C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_HelperName;

	// Token: 0x040000A9 RID: 169 RVA: 0x0000180C File Offset: 0x00000C0C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_VertexDuplicationIndices;

	// Token: 0x040000AA RID: 170 RVA: 0x0000188C File Offset: 0x00000C8C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXSPMesh;

	// Token: 0x040000AB RID: 171 RVA: 0x000018DC File Offset: 0x00000CDC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXFileEnumObject;

	// Token: 0x040000AC RID: 172 RVA: 0x000017CC File Offset: 0x00000BCC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_PatchMesh;

	// Token: 0x040000AD RID: 173 RVA: 0x000019AC File Offset: 0x00000DAC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DStateBlock9;

	// Token: 0x040000AE RID: 174 RVA: 0x0000199C File Offset: 0x00000D9C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DQuery9;

	// Token: 0x040000AF RID: 175 RVA: 0x0000181C File Offset: 0x00000C1C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_XSkinMeshHeader;

	// Token: 0x040000B0 RID: 176 RVA: 0x0000189C File Offset: 0x00000C9C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXPMesh;

	// Token: 0x040000B1 RID: 177 RVA: 0x0000190C File Offset: 0x00000D0C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXFile;

	// Token: 0x040000B2 RID: 178 RVA: 0x0000170C File Offset: 0x00000B0C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_EffectParamString;

	// Token: 0x040000B3 RID: 179 RVA: 0x0000177C File Offset: 0x00000B7C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_FVFData;

	// Token: 0x040000B4 RID: 180 RVA: 0x00001A2C File Offset: 0x00000E2C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DVertexBuffer9;

	// Token: 0x040000B5 RID: 181 RVA: 0x0000185C File Offset: 0x00000C5C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXPRTBuffer;

	// Token: 0x040000B6 RID: 182 RVA: 0x000016AC File Offset: 0x00000AAC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXTextureShader;

	// Token: 0x040000B7 RID: 183 RVA: 0x0000171C File Offset: 0x00000B1C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_EffectParamFloats;

	// Token: 0x040000B8 RID: 184 RVA: 0x000017AC File Offset: 0x00000BAC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_PMInfo;

	// Token: 0x040000B9 RID: 185 RVA: 0x00001A6C File Offset: 0x00000E6C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DBaseTexture9;

	// Token: 0x040000BA RID: 186 RVA: 0x0000179C File Offset: 0x00000B9C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_PMAttributeRange;

	// Token: 0x040000BB RID: 187 RVA: 0x0000183C File Offset: 0x00000C3C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXTextureGutterHelper;

	// Token: 0x040000BC RID: 188 RVA: 0x0000165C File Offset: 0x00000A5C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXEffect;

	// Token: 0x040000BD RID: 189 RVA: 0x00001A9C File Offset: 0x00000E9C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3D9;

	// Token: 0x040000BE RID: 190 RVA: 0x0000197C File Offset: 0x00000D7C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXMatrixStack;

	// Token: 0x040000BF RID: 191 RVA: 0x000019FC File Offset: 0x00000DFC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DVolume9;

	// Token: 0x040000C0 RID: 192 RVA: 0x0000194C File Offset: 0x00000D4C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXFont;

	// Token: 0x040000C1 RID: 193 RVA: 0x0000184C File Offset: 0x00000C4C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXPRTCompBuffer;

	// Token: 0x040000C2 RID: 194 RVA: 0x0000169C File Offset: 0x00000A9C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXFragmentLinker;

	// Token: 0x040000C3 RID: 195 RVA: 0x000016DC File Offset: 0x00000ADC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_AnimTicksPerSecond;

	// Token: 0x040000C4 RID: 196 RVA: 0x000019DC File Offset: 0x00000DDC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DVertexDeclaration9;

	// Token: 0x040000C5 RID: 197 RVA: 0x00001A4C File Offset: 0x00000E4C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DCubeTexture9;

	// Token: 0x040000C6 RID: 198 RVA: 0x00001A8C File Offset: 0x00000E8C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DDevice9;

	// Token: 0x040000C7 RID: 199 RVA: 0x000019EC File Offset: 0x00000DEC
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_IDirect3DSwapChain9;

	// Token: 0x040000C8 RID: 200 RVA: 0x0000174C File Offset: 0x00000B4C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_EffectFloats;

	// Token: 0x040000C9 RID: 201 RVA: 0x0000195C File Offset: 0x00000D5C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID IID_ID3DXSprite;

	// Token: 0x040000CA RID: 202 RVA: 0x0000175C File Offset: 0x00000B5C
	public static Microsoft.DirectX.PrivateImplementationDetails._GUID DXFILEOBJ_DeclData;

	// Token: 0x040000CB RID: 203 RVA: 0x00001554 File Offset: 0x00000954
	public static uint $ConstGCArrayBound$0x092d518d$46$;

	// Token: 0x040000CC RID: 204 RVA: 0x00001E6C File Offset: 0x0000126C
	public static uint $ConstGCArrayBound$0x92e6e72a$93$;

	// Token: 0x040000CD RID: 205 RVA: 0x00001DEC File Offset: 0x000011EC
	public static uint $ConstGCArrayBound$0x92e6e72a$125$;

	// Token: 0x040000CE RID: 206 RVA: 0x00001F84 File Offset: 0x00001384
	public static uint $ConstGCArrayBound$0x92e6e72a$23$;

	// Token: 0x040000CF RID: 207 RVA: 0x00001E7C File Offset: 0x0000127C
	public static uint $ConstGCArrayBound$0x92e6e72a$89$;

	// Token: 0x040000D0 RID: 208 RVA: 0x00001DE4 File Offset: 0x000011E4
	public static uint $ConstGCArrayBound$0x92e6e72a$127$;

	// Token: 0x040000D1 RID: 209 RVA: 0x00001F9C File Offset: 0x0000139C
	public static uint $ConstGCArrayBound$0x92e6e72a$17$;

	// Token: 0x040000D2 RID: 210 RVA: 0x00001F80 File Offset: 0x00001380
	public static uint $ConstGCArrayBound$0x92e6e72a$24$;

	// Token: 0x040000D3 RID: 211 RVA: 0x00001E4C File Offset: 0x0000124C
	public static uint $ConstGCArrayBound$0x92e6e72a$101$;

	// Token: 0x040000D4 RID: 212 RVA: 0x00001F18 File Offset: 0x00001318
	public static uint $ConstGCArrayBound$0x92e6e72a$50$;

	// Token: 0x040000D5 RID: 213 RVA: 0x00001E60 File Offset: 0x00001260
	public static uint $ConstGCArrayBound$0x92e6e72a$96$;

	// Token: 0x040000D6 RID: 214 RVA: 0x00001FB0 File Offset: 0x000013B0
	public static uint $ConstGCArrayBound$0x92e6e72a$12$;

	// Token: 0x040000D7 RID: 215 RVA: 0x00001E14 File Offset: 0x00001214
	public static uint $ConstGCArrayBound$0x92e6e72a$115$;

	// Token: 0x040000D8 RID: 216 RVA: 0x00001E78 File Offset: 0x00001278
	public static uint $ConstGCArrayBound$0x92e6e72a$90$;

	// Token: 0x040000D9 RID: 217 RVA: 0x00001EA8 File Offset: 0x000012A8
	public static uint $ConstGCArrayBound$0x92e6e72a$78$;

	// Token: 0x040000DA RID: 218 RVA: 0x00001F44 File Offset: 0x00001344
	public static uint $ConstGCArrayBound$0x92e6e72a$39$;

	// Token: 0x040000DB RID: 219 RVA: 0x00001F8C File Offset: 0x0000138C
	public static uint $ConstGCArrayBound$0x92e6e72a$21$;

	// Token: 0x040000DC RID: 220 RVA: 0x00001F0C File Offset: 0x0000130C
	public static uint $ConstGCArrayBound$0x92e6e72a$53$;

	// Token: 0x040000DD RID: 221 RVA: 0x00001EF8 File Offset: 0x000012F8
	public static uint $ConstGCArrayBound$0x92e6e72a$58$;

	// Token: 0x040000DE RID: 222 RVA: 0x00001E00 File Offset: 0x00001200
	public static uint $ConstGCArrayBound$0x92e6e72a$120$;

	// Token: 0x040000DF RID: 223 RVA: 0x00001E08 File Offset: 0x00001208
	public static uint $ConstGCArrayBound$0x92e6e72a$118$;

	// Token: 0x040000E0 RID: 224 RVA: 0x00001DFC File Offset: 0x000011FC
	public static uint $ConstGCArrayBound$0x92e6e72a$121$;

	// Token: 0x040000E1 RID: 225 RVA: 0x00001E8C File Offset: 0x0000128C
	public static uint $ConstGCArrayBound$0x92e6e72a$85$;

	// Token: 0x040000E2 RID: 226 RVA: 0x00001EF4 File Offset: 0x000012F4
	public static uint $ConstGCArrayBound$0x92e6e72a$59$;

	// Token: 0x040000E3 RID: 227 RVA: 0x00001EBC File Offset: 0x000012BC
	public static uint $ConstGCArrayBound$0x92e6e72a$73$;

	// Token: 0x040000E4 RID: 228 RVA: 0x00001FA4 File Offset: 0x000013A4
	public static uint $ConstGCArrayBound$0x92e6e72a$15$;

	// Token: 0x040000E5 RID: 229 RVA: 0x00001F50 File Offset: 0x00001350
	public static uint $ConstGCArrayBound$0x92e6e72a$36$;

	// Token: 0x040000E6 RID: 230 RVA: 0x00001E0C File Offset: 0x0000120C
	public static uint $ConstGCArrayBound$0x92e6e72a$117$;

	// Token: 0x040000E7 RID: 231 RVA: 0x00001E68 File Offset: 0x00001268
	public static uint $ConstGCArrayBound$0x92e6e72a$94$;

	// Token: 0x040000E8 RID: 232 RVA: 0x00001E04 File Offset: 0x00001204
	public static uint $ConstGCArrayBound$0x92e6e72a$119$;

	// Token: 0x040000E9 RID: 233 RVA: 0x00001F38 File Offset: 0x00001338
	public static uint $ConstGCArrayBound$0x92e6e72a$42$;

	// Token: 0x040000EA RID: 234 RVA: 0x00001EA0 File Offset: 0x000012A0
	public static uint $ConstGCArrayBound$0x92e6e72a$80$;

	// Token: 0x040000EB RID: 235 RVA: 0x00001F68 File Offset: 0x00001368
	public static uint $ConstGCArrayBound$0x92e6e72a$30$;

	// Token: 0x040000EC RID: 236 RVA: 0x00001FA0 File Offset: 0x000013A0
	public static uint $ConstGCArrayBound$0x92e6e72a$16$;

	// Token: 0x040000ED RID: 237 RVA: 0x00001F98 File Offset: 0x00001398
	public static uint $ConstGCArrayBound$0x92e6e72a$18$;

	// Token: 0x040000EE RID: 238 RVA: 0x00001EAC File Offset: 0x000012AC
	public static uint $ConstGCArrayBound$0x92e6e72a$77$;

	// Token: 0x040000EF RID: 239 RVA: 0x00001F70 File Offset: 0x00001370
	public static uint $ConstGCArrayBound$0x92e6e72a$28$;

	// Token: 0x040000F0 RID: 240 RVA: 0x00001F30 File Offset: 0x00001330
	public static uint $ConstGCArrayBound$0x92e6e72a$44$;

	// Token: 0x040000F1 RID: 241 RVA: 0x00001F24 File Offset: 0x00001324
	public static uint $ConstGCArrayBound$0x92e6e72a$47$;

	// Token: 0x040000F2 RID: 242 RVA: 0x00001EF0 File Offset: 0x000012F0
	public static uint $ConstGCArrayBound$0x92e6e72a$60$;

	// Token: 0x040000F3 RID: 243 RVA: 0x00001F3C File Offset: 0x0000133C
	public static uint $ConstGCArrayBound$0x92e6e72a$41$;

	// Token: 0x040000F4 RID: 244 RVA: 0x00001FD0 File Offset: 0x000013D0
	public static uint $ConstGCArrayBound$0x92e6e72a$4$;

	// Token: 0x040000F5 RID: 245 RVA: 0x00001E5C File Offset: 0x0000125C
	public static uint $ConstGCArrayBound$0x92e6e72a$97$;

	// Token: 0x040000F6 RID: 246 RVA: 0x00001E40 File Offset: 0x00001240
	public static uint $ConstGCArrayBound$0x92e6e72a$104$;

	// Token: 0x040000F7 RID: 247 RVA: 0x00001E80 File Offset: 0x00001280
	public static uint $ConstGCArrayBound$0x92e6e72a$88$;

	// Token: 0x040000F8 RID: 248 RVA: 0x00001EC4 File Offset: 0x000012C4
	public static uint $ConstGCArrayBound$0x92e6e72a$71$;

	// Token: 0x040000F9 RID: 249 RVA: 0x00001F88 File Offset: 0x00001388
	public static uint $ConstGCArrayBound$0x92e6e72a$22$;

	// Token: 0x040000FA RID: 250 RVA: 0x00001E30 File Offset: 0x00001230
	public static uint $ConstGCArrayBound$0x92e6e72a$108$;

	// Token: 0x040000FB RID: 251 RVA: 0x00001DF8 File Offset: 0x000011F8
	public static uint $ConstGCArrayBound$0x92e6e72a$122$;

	// Token: 0x040000FC RID: 252 RVA: 0x00001ED4 File Offset: 0x000012D4
	public static uint $ConstGCArrayBound$0x92e6e72a$67$;

	// Token: 0x040000FD RID: 253 RVA: 0x00001F64 File Offset: 0x00001364
	public static uint $ConstGCArrayBound$0x92e6e72a$31$;

	// Token: 0x040000FE RID: 254 RVA: 0x00001F48 File Offset: 0x00001348
	public static uint $ConstGCArrayBound$0x92e6e72a$38$;

	// Token: 0x040000FF RID: 255 RVA: 0x00001FD4 File Offset: 0x000013D4
	public static uint $ConstGCArrayBound$0x92e6e72a$3$;

	// Token: 0x04000100 RID: 256 RVA: 0x00001EDC File Offset: 0x000012DC
	public static uint $ConstGCArrayBound$0x92e6e72a$65$;

	// Token: 0x04000101 RID: 257 RVA: 0x00001EC8 File Offset: 0x000012C8
	public static uint $ConstGCArrayBound$0x92e6e72a$70$;

	// Token: 0x04000102 RID: 258 RVA: 0x00001F2C File Offset: 0x0000132C
	public static uint $ConstGCArrayBound$0x92e6e72a$45$;

	// Token: 0x04000103 RID: 259 RVA: 0x00001FA8 File Offset: 0x000013A8
	public static uint $ConstGCArrayBound$0x92e6e72a$14$;

	// Token: 0x04000104 RID: 260 RVA: 0x00001DF0 File Offset: 0x000011F0
	public static uint $ConstGCArrayBound$0x92e6e72a$124$;

	// Token: 0x04000105 RID: 261 RVA: 0x00001F28 File Offset: 0x00001328
	public static uint $ConstGCArrayBound$0x92e6e72a$46$;

	// Token: 0x04000106 RID: 262 RVA: 0x00001E64 File Offset: 0x00001264
	public static uint $ConstGCArrayBound$0x92e6e72a$95$;

	// Token: 0x04000107 RID: 263 RVA: 0x00001F58 File Offset: 0x00001358
	public static uint $ConstGCArrayBound$0x92e6e72a$34$;

	// Token: 0x04000108 RID: 264 RVA: 0x00001EE8 File Offset: 0x000012E8
	public static uint $ConstGCArrayBound$0x92e6e72a$62$;

	// Token: 0x04000109 RID: 265 RVA: 0x00001FD8 File Offset: 0x000013D8
	public static uint $ConstGCArrayBound$0x92e6e72a$2$;

	// Token: 0x0400010A RID: 266 RVA: 0x00001E90 File Offset: 0x00001290
	public static uint $ConstGCArrayBound$0x92e6e72a$84$;

	// Token: 0x0400010B RID: 267 RVA: 0x00001EE0 File Offset: 0x000012E0
	public static uint $ConstGCArrayBound$0x92e6e72a$64$;

	// Token: 0x0400010C RID: 268 RVA: 0x00001EEC File Offset: 0x000012EC
	public static uint $ConstGCArrayBound$0x92e6e72a$61$;

	// Token: 0x0400010D RID: 269 RVA: 0x00001E70 File Offset: 0x00001270
	public static uint $ConstGCArrayBound$0x92e6e72a$92$;

	// Token: 0x0400010E RID: 270 RVA: 0x00001EE4 File Offset: 0x000012E4
	public static uint $ConstGCArrayBound$0x92e6e72a$63$;

	// Token: 0x0400010F RID: 271 RVA: 0x00001DE0 File Offset: 0x000011E0
	public static uint $ConstGCArrayBound$0x92e6e72a$128$;

	// Token: 0x04000110 RID: 272 RVA: 0x00001ED0 File Offset: 0x000012D0
	public static uint $ConstGCArrayBound$0x92e6e72a$68$;

	// Token: 0x04000111 RID: 273 RVA: 0x00001F60 File Offset: 0x00001360
	public static uint $ConstGCArrayBound$0x92e6e72a$32$;

	// Token: 0x04000112 RID: 274 RVA: 0x00001E3C File Offset: 0x0000123C
	public static uint $ConstGCArrayBound$0x92e6e72a$105$;

	// Token: 0x04000113 RID: 275 RVA: 0x00001E84 File Offset: 0x00001284
	public static uint $ConstGCArrayBound$0x92e6e72a$87$;

	// Token: 0x04000114 RID: 276 RVA: 0x00001E9C File Offset: 0x0000129C
	public static uint $ConstGCArrayBound$0x92e6e72a$81$;

	// Token: 0x04000115 RID: 277 RVA: 0x00001EA4 File Offset: 0x000012A4
	public static uint $ConstGCArrayBound$0x92e6e72a$79$;

	// Token: 0x04000116 RID: 278 RVA: 0x00001FB8 File Offset: 0x000013B8
	public static uint $ConstGCArrayBound$0x92e6e72a$10$;

	// Token: 0x04000117 RID: 279 RVA: 0x00001ED8 File Offset: 0x000012D8
	public static uint $ConstGCArrayBound$0x92e6e72a$66$;

	// Token: 0x04000118 RID: 280 RVA: 0x00001EFC File Offset: 0x000012FC
	public static uint $ConstGCArrayBound$0x92e6e72a$57$;

	// Token: 0x04000119 RID: 281 RVA: 0x00001F04 File Offset: 0x00001304
	public static uint $ConstGCArrayBound$0x92e6e72a$55$;

	// Token: 0x0400011A RID: 282 RVA: 0x00001DE8 File Offset: 0x000011E8
	public static uint $ConstGCArrayBound$0x92e6e72a$126$;

	// Token: 0x0400011B RID: 283 RVA: 0x00001F34 File Offset: 0x00001334
	public static uint $ConstGCArrayBound$0x92e6e72a$43$;

	// Token: 0x0400011C RID: 284 RVA: 0x00001FAC File Offset: 0x000013AC
	public static uint $ConstGCArrayBound$0x92e6e72a$13$;

	// Token: 0x0400011D RID: 285 RVA: 0x00001EC0 File Offset: 0x000012C0
	public static uint $ConstGCArrayBound$0x92e6e72a$72$;

	// Token: 0x0400011E RID: 286 RVA: 0x00001FDC File Offset: 0x000013DC
	public static uint $ConstGCArrayBound$0x92e6e72a$1$;

	// Token: 0x0400011F RID: 287 RVA: 0x00001E18 File Offset: 0x00001218
	public static uint $ConstGCArrayBound$0x92e6e72a$114$;

	// Token: 0x04000120 RID: 288 RVA: 0x00001E34 File Offset: 0x00001234
	public static uint $ConstGCArrayBound$0x92e6e72a$107$;

	// Token: 0x04000121 RID: 289 RVA: 0x00001F40 File Offset: 0x00001340
	public static uint $ConstGCArrayBound$0x92e6e72a$40$;

	// Token: 0x04000122 RID: 290 RVA: 0x00001E28 File Offset: 0x00001228
	public static uint $ConstGCArrayBound$0x92e6e72a$110$;

	// Token: 0x04000123 RID: 291 RVA: 0x00001FC0 File Offset: 0x000013C0
	public static uint $ConstGCArrayBound$0x92e6e72a$8$;

	// Token: 0x04000124 RID: 292 RVA: 0x00001FC8 File Offset: 0x000013C8
	public static uint $ConstGCArrayBound$0x92e6e72a$6$;

	// Token: 0x04000125 RID: 293 RVA: 0x00001E74 File Offset: 0x00001274
	public static uint $ConstGCArrayBound$0x92e6e72a$91$;

	// Token: 0x04000126 RID: 294 RVA: 0x00001E20 File Offset: 0x00001220
	public static uint $ConstGCArrayBound$0x92e6e72a$112$;

	// Token: 0x04000127 RID: 295 RVA: 0x00001E94 File Offset: 0x00001294
	public static uint $ConstGCArrayBound$0x92e6e72a$83$;

	// Token: 0x04000128 RID: 296 RVA: 0x00001F7C File Offset: 0x0000137C
	public static uint $ConstGCArrayBound$0x92e6e72a$25$;

	// Token: 0x04000129 RID: 297 RVA: 0x00001F78 File Offset: 0x00001378
	public static uint $ConstGCArrayBound$0x92e6e72a$26$;

	// Token: 0x0400012A RID: 298 RVA: 0x00001F94 File Offset: 0x00001394
	public static uint $ConstGCArrayBound$0x92e6e72a$19$;

	// Token: 0x0400012B RID: 299 RVA: 0x00001EB4 File Offset: 0x000012B4
	public static uint $ConstGCArrayBound$0x92e6e72a$75$;

	// Token: 0x0400012C RID: 300 RVA: 0x00001F00 File Offset: 0x00001300
	public static uint $ConstGCArrayBound$0x92e6e72a$56$;

	// Token: 0x0400012D RID: 301 RVA: 0x00001F54 File Offset: 0x00001354
	public static uint $ConstGCArrayBound$0x92e6e72a$35$;

	// Token: 0x0400012E RID: 302 RVA: 0x00001E38 File Offset: 0x00001238
	public static uint $ConstGCArrayBound$0x92e6e72a$106$;

	// Token: 0x0400012F RID: 303 RVA: 0x00001F10 File Offset: 0x00001310
	public static uint $ConstGCArrayBound$0x92e6e72a$52$;

	// Token: 0x04000130 RID: 304 RVA: 0x00001F20 File Offset: 0x00001320
	public static uint $ConstGCArrayBound$0x92e6e72a$48$;

	// Token: 0x04000131 RID: 305 RVA: 0x00001FCC File Offset: 0x000013CC
	public static uint $ConstGCArrayBound$0x92e6e72a$5$;

	// Token: 0x04000132 RID: 306 RVA: 0x00001F08 File Offset: 0x00001308
	public static uint $ConstGCArrayBound$0x92e6e72a$54$;

	// Token: 0x04000133 RID: 307 RVA: 0x00001FC4 File Offset: 0x000013C4
	public static uint $ConstGCArrayBound$0x92e6e72a$7$;

	// Token: 0x04000134 RID: 308 RVA: 0x00001E98 File Offset: 0x00001298
	public static uint $ConstGCArrayBound$0x92e6e72a$82$;

	// Token: 0x04000135 RID: 309 RVA: 0x00001F4C File Offset: 0x0000134C
	public static uint $ConstGCArrayBound$0x92e6e72a$37$;

	// Token: 0x04000136 RID: 310 RVA: 0x00001E48 File Offset: 0x00001248
	public static uint $ConstGCArrayBound$0x92e6e72a$102$;

	// Token: 0x04000137 RID: 311 RVA: 0x00001FB4 File Offset: 0x000013B4
	public static uint $ConstGCArrayBound$0x92e6e72a$11$;

	// Token: 0x04000138 RID: 312 RVA: 0x00001E2C File Offset: 0x0000122C
	public static uint $ConstGCArrayBound$0x92e6e72a$109$;

	// Token: 0x04000139 RID: 313 RVA: 0x00001F74 File Offset: 0x00001374
	public static uint $ConstGCArrayBound$0x92e6e72a$27$;

	// Token: 0x0400013A RID: 314 RVA: 0x00001FBC File Offset: 0x000013BC
	public static uint $ConstGCArrayBound$0x92e6e72a$9$;

	// Token: 0x0400013B RID: 315 RVA: 0x00001F6C File Offset: 0x0000136C
	public static uint $ConstGCArrayBound$0x92e6e72a$29$;

	// Token: 0x0400013C RID: 316 RVA: 0x00001ECC File Offset: 0x000012CC
	public static uint $ConstGCArrayBound$0x92e6e72a$69$;

	// Token: 0x0400013D RID: 317 RVA: 0x00001DF4 File Offset: 0x000011F4
	public static uint $ConstGCArrayBound$0x92e6e72a$123$;

	// Token: 0x0400013E RID: 318 RVA: 0x00001E1C File Offset: 0x0000121C
	public static uint $ConstGCArrayBound$0x92e6e72a$113$;

	// Token: 0x0400013F RID: 319 RVA: 0x00001E24 File Offset: 0x00001224
	public static uint $ConstGCArrayBound$0x92e6e72a$111$;

	// Token: 0x04000140 RID: 320 RVA: 0x00001F90 File Offset: 0x00001390
	public static uint $ConstGCArrayBound$0x92e6e72a$20$;

	// Token: 0x04000141 RID: 321 RVA: 0x00001E10 File Offset: 0x00001210
	public static uint $ConstGCArrayBound$0x92e6e72a$116$;

	// Token: 0x04000142 RID: 322 RVA: 0x00001EB8 File Offset: 0x000012B8
	public static uint $ConstGCArrayBound$0x92e6e72a$74$;

	// Token: 0x04000143 RID: 323 RVA: 0x00001E44 File Offset: 0x00001244
	public static uint $ConstGCArrayBound$0x92e6e72a$103$;

	// Token: 0x04000144 RID: 324 RVA: 0x00001F5C File Offset: 0x0000135C
	public static uint $ConstGCArrayBound$0x92e6e72a$33$;

	// Token: 0x04000145 RID: 325 RVA: 0x00001E88 File Offset: 0x00001288
	public static uint $ConstGCArrayBound$0x92e6e72a$86$;

	// Token: 0x04000146 RID: 326 RVA: 0x00001E50 File Offset: 0x00001250
	public static uint $ConstGCArrayBound$0x92e6e72a$100$;

	// Token: 0x04000147 RID: 327 RVA: 0x00001F1C File Offset: 0x0000131C
	public static uint $ConstGCArrayBound$0x92e6e72a$49$;

	// Token: 0x04000148 RID: 328 RVA: 0x00001E58 File Offset: 0x00001258
	public static uint $ConstGCArrayBound$0x92e6e72a$98$;

	// Token: 0x04000149 RID: 329 RVA: 0x00001E54 File Offset: 0x00001254
	public static uint $ConstGCArrayBound$0x92e6e72a$99$;

	// Token: 0x0400014A RID: 330 RVA: 0x00001F14 File Offset: 0x00001314
	public static uint $ConstGCArrayBound$0x92e6e72a$51$;

	// Token: 0x0400014B RID: 331 RVA: 0x00001EB0 File Offset: 0x000012B0
	public static uint $ConstGCArrayBound$0x92e6e72a$76$;

	// Token: 0x0400014C RID: 332 RVA: 0x0000251C File Offset: 0x0000191C
	public static $ArrayType$0x51890b12 ??_C@_08NIHLGCK@d3d9?4dll?$AA@;

	// Token: 0x0400014D RID: 333 RVA: 0x0000250C File Offset: 0x0000190C
	public static $ArrayType$0x5c1e545c ??_C@_0O@CGJKOMPJ@DisableD3DSpy?$AA@;

	// Token: 0x0400014E RID: 334 RVA: 0x00002528 File Offset: 0x00001928
	public static $ArrayType$0x0a44f3da ??_C@_0M@NIPEKIB@D3DSpyBreak?$AA@;

	// Token: 0x0400014F RID: 335 RVA: 0x0000241C File Offset: 0x0000181C
	public static uint $ConstGCArrayBound$0x868d0ca8$60$;

	// Token: 0x04000150 RID: 336 RVA: 0x00002464 File Offset: 0x00001864
	public static uint $ConstGCArrayBound$0x868d0ca8$42$;

	// Token: 0x04000151 RID: 337 RVA: 0x00002460 File Offset: 0x00001860
	public static uint $ConstGCArrayBound$0x868d0ca8$43$;

	// Token: 0x04000152 RID: 338 RVA: 0x0000233C File Offset: 0x0000173C
	public static uint $ConstGCArrayBound$0x868d0ca8$116$;

	// Token: 0x04000153 RID: 339 RVA: 0x00002444 File Offset: 0x00001844
	public static uint $ConstGCArrayBound$0x868d0ca8$50$;

	// Token: 0x04000154 RID: 340 RVA: 0x00002338 File Offset: 0x00001738
	public static uint $ConstGCArrayBound$0x868d0ca8$117$;

	// Token: 0x04000155 RID: 341 RVA: 0x0000232C File Offset: 0x0000172C
	public static uint $ConstGCArrayBound$0x868d0ca8$120$;

	// Token: 0x04000156 RID: 342 RVA: 0x000024A8 File Offset: 0x000018A8
	public static uint $ConstGCArrayBound$0x868d0ca8$25$;

	// Token: 0x04000157 RID: 343 RVA: 0x000024A0 File Offset: 0x000018A0
	public static uint $ConstGCArrayBound$0x868d0ca8$27$;

	// Token: 0x04000158 RID: 344 RVA: 0x000023B4 File Offset: 0x000017B4
	public static uint $ConstGCArrayBound$0x868d0ca8$86$;

	// Token: 0x04000159 RID: 345 RVA: 0x00002318 File Offset: 0x00001718
	public static uint $ConstGCArrayBound$0x868d0ca8$125$;

	// Token: 0x0400015A RID: 346 RVA: 0x0000234C File Offset: 0x0000174C
	public static uint $ConstGCArrayBound$0x868d0ca8$112$;

	// Token: 0x0400015B RID: 347 RVA: 0x0000236C File Offset: 0x0000176C
	public static uint $ConstGCArrayBound$0x868d0ca8$104$;

	// Token: 0x0400015C RID: 348 RVA: 0x00002360 File Offset: 0x00001760
	public static uint $ConstGCArrayBound$0x868d0ca8$107$;

	// Token: 0x0400015D RID: 349 RVA: 0x000023E4 File Offset: 0x000017E4
	public static uint $ConstGCArrayBound$0x868d0ca8$74$;

	// Token: 0x0400015E RID: 350 RVA: 0x00002334 File Offset: 0x00001734
	public static uint $ConstGCArrayBound$0x868d0ca8$118$;

	// Token: 0x0400015F RID: 351 RVA: 0x0000249C File Offset: 0x0000189C
	public static uint $ConstGCArrayBound$0x868d0ca8$28$;

	// Token: 0x04000160 RID: 352 RVA: 0x00002398 File Offset: 0x00001798
	public static uint $ConstGCArrayBound$0x868d0ca8$93$;

	// Token: 0x04000161 RID: 353 RVA: 0x0000240C File Offset: 0x0000180C
	public static uint $ConstGCArrayBound$0x868d0ca8$64$;

	// Token: 0x04000162 RID: 354 RVA: 0x00002434 File Offset: 0x00001834
	public static uint $ConstGCArrayBound$0x868d0ca8$54$;

	// Token: 0x04000163 RID: 355 RVA: 0x00002458 File Offset: 0x00001858
	public static uint $ConstGCArrayBound$0x868d0ca8$45$;

	// Token: 0x04000164 RID: 356 RVA: 0x00002384 File Offset: 0x00001784
	public static uint $ConstGCArrayBound$0x868d0ca8$98$;

	// Token: 0x04000165 RID: 357 RVA: 0x0000231C File Offset: 0x0000171C
	public static uint $ConstGCArrayBound$0x868d0ca8$124$;

	// Token: 0x04000166 RID: 358 RVA: 0x00002404 File Offset: 0x00001804
	public static uint $ConstGCArrayBound$0x868d0ca8$66$;

	// Token: 0x04000167 RID: 359 RVA: 0x00002350 File Offset: 0x00001750
	public static uint $ConstGCArrayBound$0x868d0ca8$111$;

	// Token: 0x04000168 RID: 360 RVA: 0x0000245C File Offset: 0x0000185C
	public static uint $ConstGCArrayBound$0x868d0ca8$44$;

	// Token: 0x04000169 RID: 361 RVA: 0x00002480 File Offset: 0x00001880
	public static uint $ConstGCArrayBound$0x868d0ca8$35$;

	// Token: 0x0400016A RID: 362 RVA: 0x000024BC File Offset: 0x000018BC
	public static uint $ConstGCArrayBound$0x868d0ca8$20$;

	// Token: 0x0400016B RID: 363 RVA: 0x00002378 File Offset: 0x00001778
	public static uint $ConstGCArrayBound$0x868d0ca8$101$;

	// Token: 0x0400016C RID: 364 RVA: 0x00002504 File Offset: 0x00001904
	public static uint $ConstGCArrayBound$0x868d0ca8$2$;

	// Token: 0x0400016D RID: 365 RVA: 0x0000239C File Offset: 0x0000179C
	public static uint $ConstGCArrayBound$0x868d0ca8$92$;

	// Token: 0x0400016E RID: 366 RVA: 0x00002388 File Offset: 0x00001788
	public static uint $ConstGCArrayBound$0x868d0ca8$97$;

	// Token: 0x0400016F RID: 367 RVA: 0x000023D4 File Offset: 0x000017D4
	public static uint $ConstGCArrayBound$0x868d0ca8$78$;

	// Token: 0x04000170 RID: 368 RVA: 0x00002330 File Offset: 0x00001730
	public static uint $ConstGCArrayBound$0x868d0ca8$119$;

	// Token: 0x04000171 RID: 369 RVA: 0x000023AC File Offset: 0x000017AC
	public static uint $ConstGCArrayBound$0x868d0ca8$88$;

	// Token: 0x04000172 RID: 370 RVA: 0x00002490 File Offset: 0x00001890
	public static uint $ConstGCArrayBound$0x868d0ca8$31$;

	// Token: 0x04000173 RID: 371 RVA: 0x00002424 File Offset: 0x00001824
	public static uint $ConstGCArrayBound$0x868d0ca8$58$;

	// Token: 0x04000174 RID: 372 RVA: 0x000023E8 File Offset: 0x000017E8
	public static uint $ConstGCArrayBound$0x868d0ca8$73$;

	// Token: 0x04000175 RID: 373 RVA: 0x000024F0 File Offset: 0x000018F0
	public static uint $ConstGCArrayBound$0x868d0ca8$7$;

	// Token: 0x04000176 RID: 374 RVA: 0x00002370 File Offset: 0x00001770
	public static uint $ConstGCArrayBound$0x868d0ca8$103$;

	// Token: 0x04000177 RID: 375 RVA: 0x000023A4 File Offset: 0x000017A4
	public static uint $ConstGCArrayBound$0x868d0ca8$90$;

	// Token: 0x04000178 RID: 376 RVA: 0x000024D4 File Offset: 0x000018D4
	public static uint $ConstGCArrayBound$0x868d0ca8$14$;

	// Token: 0x04000179 RID: 377 RVA: 0x00002380 File Offset: 0x00001780
	public static uint $ConstGCArrayBound$0x868d0ca8$99$;

	// Token: 0x0400017A RID: 378 RVA: 0x0000242C File Offset: 0x0000182C
	public static uint $ConstGCArrayBound$0x868d0ca8$56$;

	// Token: 0x0400017B RID: 379 RVA: 0x0000246C File Offset: 0x0000186C
	public static uint $ConstGCArrayBound$0x868d0ca8$40$;

	// Token: 0x0400017C RID: 380 RVA: 0x0000237C File Offset: 0x0000177C
	public static uint $ConstGCArrayBound$0x868d0ca8$100$;

	// Token: 0x0400017D RID: 381 RVA: 0x000024C8 File Offset: 0x000018C8
	public static uint $ConstGCArrayBound$0x868d0ca8$17$;

	// Token: 0x0400017E RID: 382 RVA: 0x00002374 File Offset: 0x00001774
	public static uint $ConstGCArrayBound$0x868d0ca8$102$;

	// Token: 0x0400017F RID: 383 RVA: 0x000023F0 File Offset: 0x000017F0
	public static uint $ConstGCArrayBound$0x868d0ca8$71$;

	// Token: 0x04000180 RID: 384 RVA: 0x000024E0 File Offset: 0x000018E0
	public static uint $ConstGCArrayBound$0x868d0ca8$11$;

	// Token: 0x04000181 RID: 385 RVA: 0x000024A4 File Offset: 0x000018A4
	public static uint $ConstGCArrayBound$0x868d0ca8$26$;

	// Token: 0x04000182 RID: 386 RVA: 0x000023D8 File Offset: 0x000017D8
	public static uint $ConstGCArrayBound$0x868d0ca8$77$;

	// Token: 0x04000183 RID: 387 RVA: 0x00002368 File Offset: 0x00001768
	public static uint $ConstGCArrayBound$0x868d0ca8$105$;

	// Token: 0x04000184 RID: 388 RVA: 0x00002314 File Offset: 0x00001714
	public static uint $ConstGCArrayBound$0x868d0ca8$126$;

	// Token: 0x04000185 RID: 389 RVA: 0x0000248C File Offset: 0x0000188C
	public static uint $ConstGCArrayBound$0x868d0ca8$32$;

	// Token: 0x04000186 RID: 390 RVA: 0x000024E4 File Offset: 0x000018E4
	public static uint $ConstGCArrayBound$0x868d0ca8$10$;

	// Token: 0x04000187 RID: 391 RVA: 0x00002470 File Offset: 0x00001870
	public static uint $ConstGCArrayBound$0x868d0ca8$39$;

	// Token: 0x04000188 RID: 392 RVA: 0x000024B8 File Offset: 0x000018B8
	public static uint $ConstGCArrayBound$0x868d0ca8$21$;

	// Token: 0x04000189 RID: 393 RVA: 0x00002494 File Offset: 0x00001894
	public static uint $ConstGCArrayBound$0x868d0ca8$30$;

	// Token: 0x0400018A RID: 394 RVA: 0x000024DC File Offset: 0x000018DC
	public static uint $ConstGCArrayBound$0x868d0ca8$12$;

	// Token: 0x0400018B RID: 395 RVA: 0x00002418 File Offset: 0x00001818
	public static uint $ConstGCArrayBound$0x868d0ca8$61$;

	// Token: 0x0400018C RID: 396 RVA: 0x00002484 File Offset: 0x00001884
	public static uint $ConstGCArrayBound$0x868d0ca8$34$;

	// Token: 0x0400018D RID: 397 RVA: 0x000024C4 File Offset: 0x000018C4
	public static uint $ConstGCArrayBound$0x868d0ca8$18$;

	// Token: 0x0400018E RID: 398 RVA: 0x00002428 File Offset: 0x00001828
	public static uint $ConstGCArrayBound$0x868d0ca8$57$;

	// Token: 0x0400018F RID: 399 RVA: 0x00002410 File Offset: 0x00001810
	public static uint $ConstGCArrayBound$0x868d0ca8$63$;

	// Token: 0x04000190 RID: 400 RVA: 0x000023CC File Offset: 0x000017CC
	public static uint $ConstGCArrayBound$0x868d0ca8$80$;

	// Token: 0x04000191 RID: 401 RVA: 0x000024C0 File Offset: 0x000018C0
	public static uint $ConstGCArrayBound$0x868d0ca8$19$;

	// Token: 0x04000192 RID: 402 RVA: 0x0000247C File Offset: 0x0000187C
	public static uint $ConstGCArrayBound$0x868d0ca8$36$;

	// Token: 0x04000193 RID: 403 RVA: 0x00002354 File Offset: 0x00001754
	public static uint $ConstGCArrayBound$0x868d0ca8$110$;

	// Token: 0x04000194 RID: 404 RVA: 0x00002394 File Offset: 0x00001794
	public static uint $ConstGCArrayBound$0x868d0ca8$94$;

	// Token: 0x04000195 RID: 405 RVA: 0x00002430 File Offset: 0x00001830
	public static uint $ConstGCArrayBound$0x868d0ca8$55$;

	// Token: 0x04000196 RID: 406 RVA: 0x00002400 File Offset: 0x00001800
	public static uint $ConstGCArrayBound$0x868d0ca8$67$;

	// Token: 0x04000197 RID: 407 RVA: 0x00002440 File Offset: 0x00001840
	public static uint $ConstGCArrayBound$0x868d0ca8$51$;

	// Token: 0x04000198 RID: 408 RVA: 0x000024F8 File Offset: 0x000018F8
	public static uint $ConstGCArrayBound$0x868d0ca8$5$;

	// Token: 0x04000199 RID: 409 RVA: 0x000023A0 File Offset: 0x000017A0
	public static uint $ConstGCArrayBound$0x868d0ca8$91$;

	// Token: 0x0400019A RID: 410 RVA: 0x000023E0 File Offset: 0x000017E0
	public static uint $ConstGCArrayBound$0x868d0ca8$75$;

	// Token: 0x0400019B RID: 411 RVA: 0x00002408 File Offset: 0x00001808
	public static uint $ConstGCArrayBound$0x868d0ca8$65$;

	// Token: 0x0400019C RID: 412 RVA: 0x000024AC File Offset: 0x000018AC
	public static uint $ConstGCArrayBound$0x868d0ca8$24$;

	// Token: 0x0400019D RID: 413 RVA: 0x00002468 File Offset: 0x00001868
	public static uint $ConstGCArrayBound$0x868d0ca8$41$;

	// Token: 0x0400019E RID: 414 RVA: 0x000023BC File Offset: 0x000017BC
	public static uint $ConstGCArrayBound$0x868d0ca8$84$;

	// Token: 0x0400019F RID: 415 RVA: 0x00002340 File Offset: 0x00001740
	public static uint $ConstGCArrayBound$0x868d0ca8$115$;

	// Token: 0x040001A0 RID: 416 RVA: 0x000024E8 File Offset: 0x000018E8
	public static uint $ConstGCArrayBound$0x868d0ca8$9$;

	// Token: 0x040001A1 RID: 417 RVA: 0x00002500 File Offset: 0x00001900
	public static uint $ConstGCArrayBound$0x868d0ca8$3$;

	// Token: 0x040001A2 RID: 418 RVA: 0x0000235C File Offset: 0x0000175C
	public static uint $ConstGCArrayBound$0x868d0ca8$108$;

	// Token: 0x040001A3 RID: 419 RVA: 0x000024FC File Offset: 0x000018FC
	public static uint $ConstGCArrayBound$0x868d0ca8$4$;

	// Token: 0x040001A4 RID: 420 RVA: 0x000023D0 File Offset: 0x000017D0
	public static uint $ConstGCArrayBound$0x868d0ca8$79$;

	// Token: 0x040001A5 RID: 421 RVA: 0x00002508 File Offset: 0x00001908
	public static uint $ConstGCArrayBound$0x868d0ca8$1$;

	// Token: 0x040001A6 RID: 422 RVA: 0x000023F8 File Offset: 0x000017F8
	public static uint $ConstGCArrayBound$0x868d0ca8$69$;

	// Token: 0x040001A7 RID: 423 RVA: 0x0000238C File Offset: 0x0000178C
	public static uint $ConstGCArrayBound$0x868d0ca8$96$;

	// Token: 0x040001A8 RID: 424 RVA: 0x00002324 File Offset: 0x00001724
	public static uint $ConstGCArrayBound$0x868d0ca8$122$;

	// Token: 0x040001A9 RID: 425 RVA: 0x000023B8 File Offset: 0x000017B8
	public static uint $ConstGCArrayBound$0x868d0ca8$85$;

	// Token: 0x040001AA RID: 426 RVA: 0x0000230C File Offset: 0x0000170C
	public static uint $ConstGCArrayBound$0x868d0ca8$128$;

	// Token: 0x040001AB RID: 427 RVA: 0x000024D0 File Offset: 0x000018D0
	public static uint $ConstGCArrayBound$0x868d0ca8$15$;

	// Token: 0x040001AC RID: 428 RVA: 0x000024CC File Offset: 0x000018CC
	public static uint $ConstGCArrayBound$0x868d0ca8$16$;

	// Token: 0x040001AD RID: 429 RVA: 0x00002474 File Offset: 0x00001874
	public static uint $ConstGCArrayBound$0x868d0ca8$38$;

	// Token: 0x040001AE RID: 430 RVA: 0x00002448 File Offset: 0x00001848
	public static uint $ConstGCArrayBound$0x868d0ca8$49$;

	// Token: 0x040001AF RID: 431 RVA: 0x000023C0 File Offset: 0x000017C0
	public static uint $ConstGCArrayBound$0x868d0ca8$83$;

	// Token: 0x040001B0 RID: 432 RVA: 0x000024D8 File Offset: 0x000018D8
	public static uint $ConstGCArrayBound$0x868d0ca8$13$;

	// Token: 0x040001B1 RID: 433 RVA: 0x00002358 File Offset: 0x00001758
	public static uint $ConstGCArrayBound$0x868d0ca8$109$;

	// Token: 0x040001B2 RID: 434 RVA: 0x00002344 File Offset: 0x00001744
	public static uint $ConstGCArrayBound$0x868d0ca8$114$;

	// Token: 0x040001B3 RID: 435 RVA: 0x0000244C File Offset: 0x0000184C
	public static uint $ConstGCArrayBound$0x868d0ca8$48$;

	// Token: 0x040001B4 RID: 436 RVA: 0x00002390 File Offset: 0x00001790
	public static uint $ConstGCArrayBound$0x868d0ca8$95$;

	// Token: 0x040001B5 RID: 437 RVA: 0x00002420 File Offset: 0x00001820
	public static uint $ConstGCArrayBound$0x868d0ca8$59$;

	// Token: 0x040001B6 RID: 438 RVA: 0x000023EC File Offset: 0x000017EC
	public static uint $ConstGCArrayBound$0x868d0ca8$72$;

	// Token: 0x040001B7 RID: 439 RVA: 0x000024F4 File Offset: 0x000018F4
	public static uint $ConstGCArrayBound$0x868d0ca8$6$;

	// Token: 0x040001B8 RID: 440 RVA: 0x000023A8 File Offset: 0x000017A8
	public static uint $ConstGCArrayBound$0x868d0ca8$89$;

	// Token: 0x040001B9 RID: 441 RVA: 0x000023F4 File Offset: 0x000017F4
	public static uint $ConstGCArrayBound$0x868d0ca8$70$;

	// Token: 0x040001BA RID: 442 RVA: 0x00002364 File Offset: 0x00001764
	public static uint $ConstGCArrayBound$0x868d0ca8$106$;

	// Token: 0x040001BB RID: 443 RVA: 0x00002450 File Offset: 0x00001850
	public static uint $ConstGCArrayBound$0x868d0ca8$47$;

	// Token: 0x040001BC RID: 444 RVA: 0x00002438 File Offset: 0x00001838
	public static uint $ConstGCArrayBound$0x868d0ca8$53$;

	// Token: 0x040001BD RID: 445 RVA: 0x000023C8 File Offset: 0x000017C8
	public static uint $ConstGCArrayBound$0x868d0ca8$81$;

	// Token: 0x040001BE RID: 446 RVA: 0x000024B0 File Offset: 0x000018B0
	public static uint $ConstGCArrayBound$0x868d0ca8$23$;

	// Token: 0x040001BF RID: 447 RVA: 0x00002414 File Offset: 0x00001814
	public static uint $ConstGCArrayBound$0x868d0ca8$62$;

	// Token: 0x040001C0 RID: 448 RVA: 0x00002454 File Offset: 0x00001854
	public static uint $ConstGCArrayBound$0x868d0ca8$46$;

	// Token: 0x040001C1 RID: 449 RVA: 0x000024B4 File Offset: 0x000018B4
	public static uint $ConstGCArrayBound$0x868d0ca8$22$;

	// Token: 0x040001C2 RID: 450 RVA: 0x000023B0 File Offset: 0x000017B0
	public static uint $ConstGCArrayBound$0x868d0ca8$87$;

	// Token: 0x040001C3 RID: 451 RVA: 0x00002488 File Offset: 0x00001888
	public static uint $ConstGCArrayBound$0x868d0ca8$33$;

	// Token: 0x040001C4 RID: 452 RVA: 0x00002320 File Offset: 0x00001720
	public static uint $ConstGCArrayBound$0x868d0ca8$123$;

	// Token: 0x040001C5 RID: 453 RVA: 0x00002328 File Offset: 0x00001728
	public static uint $ConstGCArrayBound$0x868d0ca8$121$;

	// Token: 0x040001C6 RID: 454 RVA: 0x000023DC File Offset: 0x000017DC
	public static uint $ConstGCArrayBound$0x868d0ca8$76$;

	// Token: 0x040001C7 RID: 455 RVA: 0x00002310 File Offset: 0x00001710
	public static uint $ConstGCArrayBound$0x868d0ca8$127$;

	// Token: 0x040001C8 RID: 456 RVA: 0x00002498 File Offset: 0x00001898
	public static uint $ConstGCArrayBound$0x868d0ca8$29$;

	// Token: 0x040001C9 RID: 457 RVA: 0x000023C4 File Offset: 0x000017C4
	public static uint $ConstGCArrayBound$0x868d0ca8$82$;

	// Token: 0x040001CA RID: 458 RVA: 0x00002348 File Offset: 0x00001748
	public static uint $ConstGCArrayBound$0x868d0ca8$113$;

	// Token: 0x040001CB RID: 459 RVA: 0x0000243C File Offset: 0x0000183C
	public static uint $ConstGCArrayBound$0x868d0ca8$52$;

	// Token: 0x040001CC RID: 460 RVA: 0x000023FC File Offset: 0x000017FC
	public static uint $ConstGCArrayBound$0x868d0ca8$68$;

	// Token: 0x040001CD RID: 461 RVA: 0x000024EC File Offset: 0x000018EC
	public static uint $ConstGCArrayBound$0x868d0ca8$8$;

	// Token: 0x040001CE RID: 462 RVA: 0x00002478 File Offset: 0x00001878
	public static uint $ConstGCArrayBound$0x868d0ca8$37$;

	// Token: 0x040001CF RID: 463 RVA: 0x00002990 File Offset: 0x00001D90
	public static uint $ConstGCArrayBound$0xfa05a5d2$56$;

	// Token: 0x040001D0 RID: 464 RVA: 0x00002900 File Offset: 0x00001D00
	public static uint $ConstGCArrayBound$0xfa05a5d2$92$;

	// Token: 0x040001D1 RID: 465 RVA: 0x00002940 File Offset: 0x00001D40
	public static uint $ConstGCArrayBound$0xfa05a5d2$76$;

	// Token: 0x040001D2 RID: 466 RVA: 0x0000286C File Offset: 0x00001C6C
	public static uint $ConstGCArrayBound$0xfa05a5d2$129$;

	// Token: 0x040001D3 RID: 467 RVA: 0x000029C8 File Offset: 0x00001DC8
	public static uint $ConstGCArrayBound$0xfa05a5d2$42$;

	// Token: 0x040001D4 RID: 468 RVA: 0x00002A6C File Offset: 0x00001E6C
	public static uint $ConstGCArrayBound$0xfa05a5d2$1$;

	// Token: 0x040001D5 RID: 469 RVA: 0x000029DC File Offset: 0x00001DDC
	public static uint $ConstGCArrayBound$0xfa05a5d2$37$;

	// Token: 0x040001D6 RID: 470 RVA: 0x000028E4 File Offset: 0x00001CE4
	public static uint $ConstGCArrayBound$0xfa05a5d2$99$;

	// Token: 0x040001D7 RID: 471 RVA: 0x000028C8 File Offset: 0x00001CC8
	public static uint $ConstGCArrayBound$0xfa05a5d2$106$;

	// Token: 0x040001D8 RID: 472 RVA: 0x00002A48 File Offset: 0x00001E48
	public static uint $ConstGCArrayBound$0xfa05a5d2$10$;

	// Token: 0x040001D9 RID: 473 RVA: 0x0000297C File Offset: 0x00001D7C
	public static uint $ConstGCArrayBound$0xfa05a5d2$61$;

	// Token: 0x040001DA RID: 474 RVA: 0x000029E4 File Offset: 0x00001DE4
	public static uint $ConstGCArrayBound$0xfa05a5d2$35$;

	// Token: 0x040001DB RID: 475 RVA: 0x0000292C File Offset: 0x00001D2C
	public static uint $ConstGCArrayBound$0xfa05a5d2$81$;

	// Token: 0x040001DC RID: 476 RVA: 0x000028E0 File Offset: 0x00001CE0
	public static uint $ConstGCArrayBound$0xfa05a5d2$100$;

	// Token: 0x040001DD RID: 477 RVA: 0x000028D0 File Offset: 0x00001CD0
	public static uint $ConstGCArrayBound$0xfa05a5d2$104$;

	// Token: 0x040001DE RID: 478 RVA: 0x000028C4 File Offset: 0x00001CC4
	public static uint $ConstGCArrayBound$0xfa05a5d2$107$;

	// Token: 0x040001DF RID: 479 RVA: 0x00002A04 File Offset: 0x00001E04
	public static uint $ConstGCArrayBound$0xfa05a5d2$27$;

	// Token: 0x040001E0 RID: 480 RVA: 0x00002950 File Offset: 0x00001D50
	public static uint $ConstGCArrayBound$0xfa05a5d2$72$;

	// Token: 0x040001E1 RID: 481 RVA: 0x00002944 File Offset: 0x00001D44
	public static uint $ConstGCArrayBound$0xfa05a5d2$75$;

	// Token: 0x040001E2 RID: 482 RVA: 0x0000288C File Offset: 0x00001C8C
	public static uint $ConstGCArrayBound$0xfa05a5d2$121$;

	// Token: 0x040001E3 RID: 483 RVA: 0x00002924 File Offset: 0x00001D24
	public static uint $ConstGCArrayBound$0xfa05a5d2$83$;

	// Token: 0x040001E4 RID: 484 RVA: 0x00002918 File Offset: 0x00001D18
	public static uint $ConstGCArrayBound$0xfa05a5d2$86$;

	// Token: 0x040001E5 RID: 485 RVA: 0x00002980 File Offset: 0x00001D80
	public static uint $ConstGCArrayBound$0xfa05a5d2$60$;

	// Token: 0x040001E6 RID: 486 RVA: 0x0000287C File Offset: 0x00001C7C
	public static uint $ConstGCArrayBound$0xfa05a5d2$125$;

	// Token: 0x040001E7 RID: 487 RVA: 0x000029FC File Offset: 0x00001DFC
	public static uint $ConstGCArrayBound$0xfa05a5d2$29$;

	// Token: 0x040001E8 RID: 488 RVA: 0x000029E0 File Offset: 0x00001DE0
	public static uint $ConstGCArrayBound$0xfa05a5d2$36$;

	// Token: 0x040001E9 RID: 489 RVA: 0x000029CC File Offset: 0x00001DCC
	public static uint $ConstGCArrayBound$0xfa05a5d2$41$;

	// Token: 0x040001EA RID: 490 RVA: 0x00002A08 File Offset: 0x00001E08
	public static uint $ConstGCArrayBound$0xfa05a5d2$26$;

	// Token: 0x040001EB RID: 491 RVA: 0x00002A34 File Offset: 0x00001E34
	public static uint $ConstGCArrayBound$0xfa05a5d2$15$;

	// Token: 0x040001EC RID: 492 RVA: 0x000029A8 File Offset: 0x00001DA8
	public static uint $ConstGCArrayBound$0xfa05a5d2$50$;

	// Token: 0x040001ED RID: 493 RVA: 0x000028BC File Offset: 0x00001CBC
	public static uint $ConstGCArrayBound$0xfa05a5d2$109$;

	// Token: 0x040001EE RID: 494 RVA: 0x000029F0 File Offset: 0x00001DF0
	public static uint $ConstGCArrayBound$0xfa05a5d2$32$;

	// Token: 0x040001EF RID: 495 RVA: 0x00002A3C File Offset: 0x00001E3C
	public static uint $ConstGCArrayBound$0xfa05a5d2$13$;

	// Token: 0x040001F0 RID: 496 RVA: 0x00002878 File Offset: 0x00001C78
	public static uint $ConstGCArrayBound$0xfa05a5d2$126$;

	// Token: 0x040001F1 RID: 497 RVA: 0x00002A00 File Offset: 0x00001E00
	public static uint $ConstGCArrayBound$0xfa05a5d2$28$;

	// Token: 0x040001F2 RID: 498 RVA: 0x000028F0 File Offset: 0x00001CF0
	public static uint $ConstGCArrayBound$0xfa05a5d2$96$;

	// Token: 0x040001F3 RID: 499 RVA: 0x000028DC File Offset: 0x00001CDC
	public static uint $ConstGCArrayBound$0xfa05a5d2$101$;

	// Token: 0x040001F4 RID: 500 RVA: 0x000029D4 File Offset: 0x00001DD4
	public static uint $ConstGCArrayBound$0xfa05a5d2$39$;

	// Token: 0x040001F5 RID: 501 RVA: 0x00002934 File Offset: 0x00001D34
	public static uint $ConstGCArrayBound$0xfa05a5d2$79$;

	// Token: 0x040001F6 RID: 502 RVA: 0x000029B0 File Offset: 0x00001DB0
	public static uint $ConstGCArrayBound$0xfa05a5d2$48$;

	// Token: 0x040001F7 RID: 503 RVA: 0x000029AC File Offset: 0x00001DAC
	public static uint $ConstGCArrayBound$0xfa05a5d2$49$;

	// Token: 0x040001F8 RID: 504 RVA: 0x00002930 File Offset: 0x00001D30
	public static uint $ConstGCArrayBound$0xfa05a5d2$80$;

	// Token: 0x040001F9 RID: 505 RVA: 0x00002A60 File Offset: 0x00001E60
	public static uint $ConstGCArrayBound$0xfa05a5d2$4$;

	// Token: 0x040001FA RID: 506 RVA: 0x0000291C File Offset: 0x00001D1C
	public static uint $ConstGCArrayBound$0xfa05a5d2$85$;

	// Token: 0x040001FB RID: 507 RVA: 0x000029E8 File Offset: 0x00001DE8
	public static uint $ConstGCArrayBound$0xfa05a5d2$34$;

	// Token: 0x040001FC RID: 508 RVA: 0x00002A18 File Offset: 0x00001E18
	public static uint $ConstGCArrayBound$0xfa05a5d2$22$;

	// Token: 0x040001FD RID: 509 RVA: 0x0000294C File Offset: 0x00001D4C
	public static uint $ConstGCArrayBound$0xfa05a5d2$73$;

	// Token: 0x040001FE RID: 510 RVA: 0x0000293C File Offset: 0x00001D3C
	public static uint $ConstGCArrayBound$0xfa05a5d2$77$;

	// Token: 0x040001FF RID: 511 RVA: 0x00002A54 File Offset: 0x00001E54
	public static uint $ConstGCArrayBound$0xfa05a5d2$7$;

	// Token: 0x04000200 RID: 512 RVA: 0x00002904 File Offset: 0x00001D04
	public static uint $ConstGCArrayBound$0xfa05a5d2$91$;

	// Token: 0x04000201 RID: 513 RVA: 0x00002A68 File Offset: 0x00001E68
	public static uint $ConstGCArrayBound$0xfa05a5d2$2$;

	// Token: 0x04000202 RID: 514 RVA: 0x000029F8 File Offset: 0x00001DF8
	public static uint $ConstGCArrayBound$0xfa05a5d2$30$;

	// Token: 0x04000203 RID: 515 RVA: 0x00002888 File Offset: 0x00001C88
	public static uint $ConstGCArrayBound$0xfa05a5d2$122$;

	// Token: 0x04000204 RID: 516 RVA: 0x00002894 File Offset: 0x00001C94
	public static uint $ConstGCArrayBound$0xfa05a5d2$119$;

	// Token: 0x04000205 RID: 517 RVA: 0x00002A14 File Offset: 0x00001E14
	public static uint $ConstGCArrayBound$0xfa05a5d2$23$;

	// Token: 0x04000206 RID: 518 RVA: 0x00002914 File Offset: 0x00001D14
	public static uint $ConstGCArrayBound$0xfa05a5d2$87$;

	// Token: 0x04000207 RID: 519 RVA: 0x00002890 File Offset: 0x00001C90
	public static uint $ConstGCArrayBound$0xfa05a5d2$120$;

	// Token: 0x04000208 RID: 520 RVA: 0x00002A50 File Offset: 0x00001E50
	public static uint $ConstGCArrayBound$0xfa05a5d2$8$;

	// Token: 0x04000209 RID: 521 RVA: 0x00002A2C File Offset: 0x00001E2C
	public static uint $ConstGCArrayBound$0xfa05a5d2$17$;

	// Token: 0x0400020A RID: 522 RVA: 0x00002874 File Offset: 0x00001C74
	public static uint $ConstGCArrayBound$0xfa05a5d2$127$;

	// Token: 0x0400020B RID: 523 RVA: 0x000028E8 File Offset: 0x00001CE8
	public static uint $ConstGCArrayBound$0xfa05a5d2$98$;

	// Token: 0x0400020C RID: 524 RVA: 0x00002A58 File Offset: 0x00001E58
	public static uint $ConstGCArrayBound$0xfa05a5d2$6$;

	// Token: 0x0400020D RID: 525 RVA: 0x000028AC File Offset: 0x00001CAC
	public static uint $ConstGCArrayBound$0xfa05a5d2$113$;

	// Token: 0x0400020E RID: 526 RVA: 0x0000290C File Offset: 0x00001D0C
	public static uint $ConstGCArrayBound$0xfa05a5d2$89$;

	// Token: 0x0400020F RID: 527 RVA: 0x000028D4 File Offset: 0x00001CD4
	public static uint $ConstGCArrayBound$0xfa05a5d2$103$;

	// Token: 0x04000210 RID: 528 RVA: 0x0000289C File Offset: 0x00001C9C
	public static uint $ConstGCArrayBound$0xfa05a5d2$117$;

	// Token: 0x04000211 RID: 529 RVA: 0x0000296C File Offset: 0x00001D6C
	public static uint $ConstGCArrayBound$0xfa05a5d2$65$;

	// Token: 0x04000212 RID: 530 RVA: 0x00002984 File Offset: 0x00001D84
	public static uint $ConstGCArrayBound$0xfa05a5d2$59$;

	// Token: 0x04000213 RID: 531 RVA: 0x0000295C File Offset: 0x00001D5C
	public static uint $ConstGCArrayBound$0xfa05a5d2$69$;

	// Token: 0x04000214 RID: 532 RVA: 0x00002910 File Offset: 0x00001D10
	public static uint $ConstGCArrayBound$0xfa05a5d2$88$;

	// Token: 0x04000215 RID: 533 RVA: 0x00002974 File Offset: 0x00001D74
	public static uint $ConstGCArrayBound$0xfa05a5d2$63$;

	// Token: 0x04000216 RID: 534 RVA: 0x000028F8 File Offset: 0x00001CF8
	public static uint $ConstGCArrayBound$0xfa05a5d2$94$;

	// Token: 0x04000217 RID: 535 RVA: 0x00002920 File Offset: 0x00001D20
	public static uint $ConstGCArrayBound$0xfa05a5d2$84$;

	// Token: 0x04000218 RID: 536 RVA: 0x000029D8 File Offset: 0x00001DD8
	public static uint $ConstGCArrayBound$0xfa05a5d2$38$;

	// Token: 0x04000219 RID: 537 RVA: 0x00002960 File Offset: 0x00001D60
	public static uint $ConstGCArrayBound$0xfa05a5d2$68$;

	// Token: 0x0400021A RID: 538 RVA: 0x00002998 File Offset: 0x00001D98
	public static uint $ConstGCArrayBound$0xfa05a5d2$54$;

	// Token: 0x0400021B RID: 539 RVA: 0x000028FC File Offset: 0x00001CFC
	public static uint $ConstGCArrayBound$0xfa05a5d2$93$;

	// Token: 0x0400021C RID: 540 RVA: 0x00002964 File Offset: 0x00001D64
	public static uint $ConstGCArrayBound$0xfa05a5d2$67$;

	// Token: 0x0400021D RID: 541 RVA: 0x000028C0 File Offset: 0x00001CC0
	public static uint $ConstGCArrayBound$0xfa05a5d2$108$;

	// Token: 0x0400021E RID: 542 RVA: 0x00002870 File Offset: 0x00001C70
	public static uint $ConstGCArrayBound$0xfa05a5d2$128$;

	// Token: 0x0400021F RID: 543 RVA: 0x000029B8 File Offset: 0x00001DB8
	public static uint $ConstGCArrayBound$0xfa05a5d2$46$;

	// Token: 0x04000220 RID: 544 RVA: 0x000029A0 File Offset: 0x00001DA0
	public static uint $ConstGCArrayBound$0xfa05a5d2$52$;

	// Token: 0x04000221 RID: 545 RVA: 0x000028A0 File Offset: 0x00001CA0
	public static uint $ConstGCArrayBound$0xfa05a5d2$116$;

	// Token: 0x04000222 RID: 546 RVA: 0x00002994 File Offset: 0x00001D94
	public static uint $ConstGCArrayBound$0xfa05a5d2$55$;

	// Token: 0x04000223 RID: 547 RVA: 0x000029F4 File Offset: 0x00001DF4
	public static uint $ConstGCArrayBound$0xfa05a5d2$31$;

	// Token: 0x04000224 RID: 548 RVA: 0x00002A0C File Offset: 0x00001E0C
	public static uint $ConstGCArrayBound$0xfa05a5d2$25$;

	// Token: 0x04000225 RID: 549 RVA: 0x00002938 File Offset: 0x00001D38
	public static uint $ConstGCArrayBound$0xfa05a5d2$78$;

	// Token: 0x04000226 RID: 550 RVA: 0x00002A38 File Offset: 0x00001E38
	public static uint $ConstGCArrayBound$0xfa05a5d2$14$;

	// Token: 0x04000227 RID: 551 RVA: 0x00002954 File Offset: 0x00001D54
	public static uint $ConstGCArrayBound$0xfa05a5d2$71$;

	// Token: 0x04000228 RID: 552 RVA: 0x00002A5C File Offset: 0x00001E5C
	public static uint $ConstGCArrayBound$0xfa05a5d2$5$;

	// Token: 0x04000229 RID: 553 RVA: 0x00002A28 File Offset: 0x00001E28
	public static uint $ConstGCArrayBound$0xfa05a5d2$18$;

	// Token: 0x0400022A RID: 554 RVA: 0x00002A10 File Offset: 0x00001E10
	public static uint $ConstGCArrayBound$0xfa05a5d2$24$;

	// Token: 0x0400022B RID: 555 RVA: 0x00002A24 File Offset: 0x00001E24
	public static uint $ConstGCArrayBound$0xfa05a5d2$19$;

	// Token: 0x0400022C RID: 556 RVA: 0x00002978 File Offset: 0x00001D78
	public static uint $ConstGCArrayBound$0xfa05a5d2$62$;

	// Token: 0x0400022D RID: 557 RVA: 0x000028A4 File Offset: 0x00001CA4
	public static uint $ConstGCArrayBound$0xfa05a5d2$115$;

	// Token: 0x0400022E RID: 558 RVA: 0x00002A44 File Offset: 0x00001E44
	public static uint $ConstGCArrayBound$0xfa05a5d2$11$;

	// Token: 0x0400022F RID: 559 RVA: 0x000028CC File Offset: 0x00001CCC
	public static uint $ConstGCArrayBound$0xfa05a5d2$105$;

	// Token: 0x04000230 RID: 560 RVA: 0x00002A40 File Offset: 0x00001E40
	public static uint $ConstGCArrayBound$0xfa05a5d2$12$;

	// Token: 0x04000231 RID: 561 RVA: 0x000028B4 File Offset: 0x00001CB4
	public static uint $ConstGCArrayBound$0xfa05a5d2$111$;

	// Token: 0x04000232 RID: 562 RVA: 0x00002A30 File Offset: 0x00001E30
	public static uint $ConstGCArrayBound$0xfa05a5d2$16$;

	// Token: 0x04000233 RID: 563 RVA: 0x000028B0 File Offset: 0x00001CB0
	public static uint $ConstGCArrayBound$0xfa05a5d2$112$;

	// Token: 0x04000234 RID: 564 RVA: 0x00002A1C File Offset: 0x00001E1C
	public static uint $ConstGCArrayBound$0xfa05a5d2$21$;

	// Token: 0x04000235 RID: 565 RVA: 0x000029C4 File Offset: 0x00001DC4
	public static uint $ConstGCArrayBound$0xfa05a5d2$43$;

	// Token: 0x04000236 RID: 566 RVA: 0x000029A4 File Offset: 0x00001DA4
	public static uint $ConstGCArrayBound$0xfa05a5d2$51$;

	// Token: 0x04000237 RID: 567 RVA: 0x000028B8 File Offset: 0x00001CB8
	public static uint $ConstGCArrayBound$0xfa05a5d2$110$;

	// Token: 0x04000238 RID: 568 RVA: 0x00002880 File Offset: 0x00001C80
	public static uint $ConstGCArrayBound$0xfa05a5d2$124$;

	// Token: 0x04000239 RID: 569 RVA: 0x000029D0 File Offset: 0x00001DD0
	public static uint $ConstGCArrayBound$0xfa05a5d2$40$;

	// Token: 0x0400023A RID: 570 RVA: 0x000029B4 File Offset: 0x00001DB4
	public static uint $ConstGCArrayBound$0xfa05a5d2$47$;

	// Token: 0x0400023B RID: 571 RVA: 0x00002884 File Offset: 0x00001C84
	public static uint $ConstGCArrayBound$0xfa05a5d2$123$;

	// Token: 0x0400023C RID: 572 RVA: 0x000028F4 File Offset: 0x00001CF4
	public static uint $ConstGCArrayBound$0xfa05a5d2$95$;

	// Token: 0x0400023D RID: 573 RVA: 0x00002908 File Offset: 0x00001D08
	public static uint $ConstGCArrayBound$0xfa05a5d2$90$;

	// Token: 0x0400023E RID: 574 RVA: 0x00002A64 File Offset: 0x00001E64
	public static uint $ConstGCArrayBound$0xfa05a5d2$3$;

	// Token: 0x0400023F RID: 575 RVA: 0x000029BC File Offset: 0x00001DBC
	public static uint $ConstGCArrayBound$0xfa05a5d2$45$;

	// Token: 0x04000240 RID: 576 RVA: 0x00002898 File Offset: 0x00001C98
	public static uint $ConstGCArrayBound$0xfa05a5d2$118$;

	// Token: 0x04000241 RID: 577 RVA: 0x00002958 File Offset: 0x00001D58
	public static uint $ConstGCArrayBound$0xfa05a5d2$70$;

	// Token: 0x04000242 RID: 578 RVA: 0x000029EC File Offset: 0x00001DEC
	public static uint $ConstGCArrayBound$0xfa05a5d2$33$;

	// Token: 0x04000243 RID: 579 RVA: 0x000028A8 File Offset: 0x00001CA8
	public static uint $ConstGCArrayBound$0xfa05a5d2$114$;

	// Token: 0x04000244 RID: 580 RVA: 0x0000299C File Offset: 0x00001D9C
	public static uint $ConstGCArrayBound$0xfa05a5d2$53$;

	// Token: 0x04000245 RID: 581 RVA: 0x00002928 File Offset: 0x00001D28
	public static uint $ConstGCArrayBound$0xfa05a5d2$82$;

	// Token: 0x04000246 RID: 582 RVA: 0x00002948 File Offset: 0x00001D48
	public static uint $ConstGCArrayBound$0xfa05a5d2$74$;

	// Token: 0x04000247 RID: 583 RVA: 0x00002968 File Offset: 0x00001D68
	public static uint $ConstGCArrayBound$0xfa05a5d2$66$;

	// Token: 0x04000248 RID: 584 RVA: 0x00002A4C File Offset: 0x00001E4C
	public static uint $ConstGCArrayBound$0xfa05a5d2$9$;

	// Token: 0x04000249 RID: 585 RVA: 0x000028D8 File Offset: 0x00001CD8
	public static uint $ConstGCArrayBound$0xfa05a5d2$102$;

	// Token: 0x0400024A RID: 586 RVA: 0x00002970 File Offset: 0x00001D70
	public static uint $ConstGCArrayBound$0xfa05a5d2$64$;

	// Token: 0x0400024B RID: 587 RVA: 0x000028EC File Offset: 0x00001CEC
	public static uint $ConstGCArrayBound$0xfa05a5d2$97$;

	// Token: 0x0400024C RID: 588 RVA: 0x00002A20 File Offset: 0x00001E20
	public static uint $ConstGCArrayBound$0xfa05a5d2$20$;

	// Token: 0x0400024D RID: 589 RVA: 0x00002988 File Offset: 0x00001D88
	public static uint $ConstGCArrayBound$0xfa05a5d2$58$;

	// Token: 0x0400024E RID: 590 RVA: 0x000029C0 File Offset: 0x00001DC0
	public static uint $ConstGCArrayBound$0xfa05a5d2$44$;

	// Token: 0x0400024F RID: 591 RVA: 0x0000298C File Offset: 0x00001D8C
	public static uint $ConstGCArrayBound$0xfa05a5d2$57$;

	// Token: 0x04000250 RID: 592 RVA: 0x00002DCC File Offset: 0x000021CC
	public static uint $ConstGCArrayBound$0x116630ef$115$;

	// Token: 0x04000251 RID: 593 RVA: 0x00002EDC File Offset: 0x000022DC
	public static uint $ConstGCArrayBound$0x116630ef$47$;

	// Token: 0x04000252 RID: 594 RVA: 0x00002E1C File Offset: 0x0000221C
	public static uint $ConstGCArrayBound$0x116630ef$95$;

	// Token: 0x04000253 RID: 595 RVA: 0x00002DD0 File Offset: 0x000021D0
	public static uint $ConstGCArrayBound$0x116630ef$114$;

	// Token: 0x04000254 RID: 596 RVA: 0x00002E30 File Offset: 0x00002230
	public static uint $ConstGCArrayBound$0x116630ef$90$;

	// Token: 0x04000255 RID: 597 RVA: 0x00002D9C File Offset: 0x0000219C
	public static uint $ConstGCArrayBound$0x116630ef$127$;

	// Token: 0x04000256 RID: 598 RVA: 0x00002E64 File Offset: 0x00002264
	public static uint $ConstGCArrayBound$0x116630ef$77$;

	// Token: 0x04000257 RID: 599 RVA: 0x00002F74 File Offset: 0x00002374
	public static uint $ConstGCArrayBound$0x116630ef$9$;

	// Token: 0x04000258 RID: 600 RVA: 0x00002E08 File Offset: 0x00002208
	public static uint $ConstGCArrayBound$0x116630ef$100$;

	// Token: 0x04000259 RID: 601 RVA: 0x00002EF0 File Offset: 0x000022F0
	public static uint $ConstGCArrayBound$0x116630ef$42$;

	// Token: 0x0400025A RID: 602 RVA: 0x00002E34 File Offset: 0x00002234
	public static uint $ConstGCArrayBound$0x116630ef$89$;

	// Token: 0x0400025B RID: 603 RVA: 0x00002DE4 File Offset: 0x000021E4
	public static uint $ConstGCArrayBound$0x116630ef$109$;

	// Token: 0x0400025C RID: 604 RVA: 0x00002E5C File Offset: 0x0000225C
	public static uint $ConstGCArrayBound$0x116630ef$79$;

	// Token: 0x0400025D RID: 605 RVA: 0x00002EC0 File Offset: 0x000022C0
	public static uint $ConstGCArrayBound$0x116630ef$54$;

	// Token: 0x0400025E RID: 606 RVA: 0x00002DD4 File Offset: 0x000021D4
	public static uint $ConstGCArrayBound$0x116630ef$113$;

	// Token: 0x0400025F RID: 607 RVA: 0x00002E48 File Offset: 0x00002248
	public static uint $ConstGCArrayBound$0x116630ef$84$;

	// Token: 0x04000260 RID: 608 RVA: 0x00002EB4 File Offset: 0x000022B4
	public static uint $ConstGCArrayBound$0x116630ef$57$;

	// Token: 0x04000261 RID: 609 RVA: 0x00002F3C File Offset: 0x0000233C
	public static uint $ConstGCArrayBound$0x116630ef$23$;

	// Token: 0x04000262 RID: 610 RVA: 0x00002F94 File Offset: 0x00002394
	public static uint $ConstGCArrayBound$0x116630ef$1$;

	// Token: 0x04000263 RID: 611 RVA: 0x00002F78 File Offset: 0x00002378
	public static uint $ConstGCArrayBound$0x116630ef$8$;

	// Token: 0x04000264 RID: 612 RVA: 0x00002F60 File Offset: 0x00002360
	public static uint $ConstGCArrayBound$0x116630ef$14$;

	// Token: 0x04000265 RID: 613 RVA: 0x00002F8C File Offset: 0x0000238C
	public static uint $ConstGCArrayBound$0x116630ef$3$;

	// Token: 0x04000266 RID: 614 RVA: 0x00002E3C File Offset: 0x0000223C
	public static uint $ConstGCArrayBound$0x116630ef$87$;

	// Token: 0x04000267 RID: 615 RVA: 0x00002E24 File Offset: 0x00002224
	public static uint $ConstGCArrayBound$0x116630ef$93$;

	// Token: 0x04000268 RID: 616 RVA: 0x00002EE0 File Offset: 0x000022E0
	public static uint $ConstGCArrayBound$0x116630ef$46$;

	// Token: 0x04000269 RID: 617 RVA: 0x00002DE0 File Offset: 0x000021E0
	public static uint $ConstGCArrayBound$0x116630ef$110$;

	// Token: 0x0400026A RID: 618 RVA: 0x00002E10 File Offset: 0x00002210
	public static uint $ConstGCArrayBound$0x116630ef$98$;

	// Token: 0x0400026B RID: 619 RVA: 0x00002F38 File Offset: 0x00002338
	public static uint $ConstGCArrayBound$0x116630ef$24$;

	// Token: 0x0400026C RID: 620 RVA: 0x00002E50 File Offset: 0x00002250
	public static uint $ConstGCArrayBound$0x116630ef$82$;

	// Token: 0x0400026D RID: 621 RVA: 0x00002F14 File Offset: 0x00002314
	public static uint $ConstGCArrayBound$0x116630ef$33$;

	// Token: 0x0400026E RID: 622 RVA: 0x00002F68 File Offset: 0x00002368
	public static uint $ConstGCArrayBound$0x116630ef$12$;

	// Token: 0x0400026F RID: 623 RVA: 0x00002DA8 File Offset: 0x000021A8
	public static uint $ConstGCArrayBound$0x116630ef$124$;

	// Token: 0x04000270 RID: 624 RVA: 0x00002E84 File Offset: 0x00002284
	public static uint $ConstGCArrayBound$0x116630ef$69$;

	// Token: 0x04000271 RID: 625 RVA: 0x00002ED4 File Offset: 0x000022D4
	public static uint $ConstGCArrayBound$0x116630ef$49$;

	// Token: 0x04000272 RID: 626 RVA: 0x00002DB8 File Offset: 0x000021B8
	public static uint $ConstGCArrayBound$0x116630ef$120$;

	// Token: 0x04000273 RID: 627 RVA: 0x00002F64 File Offset: 0x00002364
	public static uint $ConstGCArrayBound$0x116630ef$13$;

	// Token: 0x04000274 RID: 628 RVA: 0x00002F24 File Offset: 0x00002324
	public static uint $ConstGCArrayBound$0x116630ef$29$;

	// Token: 0x04000275 RID: 629 RVA: 0x00002DD8 File Offset: 0x000021D8
	public static uint $ConstGCArrayBound$0x116630ef$112$;

	// Token: 0x04000276 RID: 630 RVA: 0x00002F00 File Offset: 0x00002300
	public static uint $ConstGCArrayBound$0x116630ef$38$;

	// Token: 0x04000277 RID: 631 RVA: 0x00002F7C File Offset: 0x0000237C
	public static uint $ConstGCArrayBound$0x116630ef$7$;

	// Token: 0x04000278 RID: 632 RVA: 0x00002F5C File Offset: 0x0000235C
	public static uint $ConstGCArrayBound$0x116630ef$15$;

	// Token: 0x04000279 RID: 633 RVA: 0x00002F04 File Offset: 0x00002304
	public static uint $ConstGCArrayBound$0x116630ef$37$;

	// Token: 0x0400027A RID: 634 RVA: 0x00002E98 File Offset: 0x00002298
	public static uint $ConstGCArrayBound$0x116630ef$64$;

	// Token: 0x0400027B RID: 635 RVA: 0x00002DC0 File Offset: 0x000021C0
	public static uint $ConstGCArrayBound$0x116630ef$118$;

	// Token: 0x0400027C RID: 636 RVA: 0x00002E60 File Offset: 0x00002260
	public static uint $ConstGCArrayBound$0x116630ef$78$;

	// Token: 0x0400027D RID: 637 RVA: 0x00002F30 File Offset: 0x00002330
	public static uint $ConstGCArrayBound$0x116630ef$26$;

	// Token: 0x0400027E RID: 638 RVA: 0x00002EEC File Offset: 0x000022EC
	public static uint $ConstGCArrayBound$0x116630ef$43$;

	// Token: 0x0400027F RID: 639 RVA: 0x00002E7C File Offset: 0x0000227C
	public static uint $ConstGCArrayBound$0x116630ef$71$;

	// Token: 0x04000280 RID: 640 RVA: 0x00002E94 File Offset: 0x00002294
	public static uint $ConstGCArrayBound$0x116630ef$65$;

	// Token: 0x04000281 RID: 641 RVA: 0x00002F50 File Offset: 0x00002350
	public static uint $ConstGCArrayBound$0x116630ef$18$;

	// Token: 0x04000282 RID: 642 RVA: 0x00002E8C File Offset: 0x0000228C
	public static uint $ConstGCArrayBound$0x116630ef$67$;

	// Token: 0x04000283 RID: 643 RVA: 0x00002EA4 File Offset: 0x000022A4
	public static uint $ConstGCArrayBound$0x116630ef$61$;

	// Token: 0x04000284 RID: 644 RVA: 0x00002DEC File Offset: 0x000021EC
	public static uint $ConstGCArrayBound$0x116630ef$107$;

	// Token: 0x04000285 RID: 645 RVA: 0x00002F2C File Offset: 0x0000232C
	public static uint $ConstGCArrayBound$0x116630ef$27$;

	// Token: 0x04000286 RID: 646 RVA: 0x00002E40 File Offset: 0x00002240
	public static uint $ConstGCArrayBound$0x116630ef$86$;

	// Token: 0x04000287 RID: 647 RVA: 0x00002E68 File Offset: 0x00002268
	public static uint $ConstGCArrayBound$0x116630ef$76$;

	// Token: 0x04000288 RID: 648 RVA: 0x00002EAC File Offset: 0x000022AC
	public static uint $ConstGCArrayBound$0x116630ef$59$;

	// Token: 0x04000289 RID: 649 RVA: 0x00002E20 File Offset: 0x00002220
	public static uint $ConstGCArrayBound$0x116630ef$94$;

	// Token: 0x0400028A RID: 650 RVA: 0x00002DC8 File Offset: 0x000021C8
	public static uint $ConstGCArrayBound$0x116630ef$116$;

	// Token: 0x0400028B RID: 651 RVA: 0x00002E70 File Offset: 0x00002270
	public static uint $ConstGCArrayBound$0x116630ef$74$;

	// Token: 0x0400028C RID: 652 RVA: 0x00002F88 File Offset: 0x00002388
	public static uint $ConstGCArrayBound$0x116630ef$4$;

	// Token: 0x0400028D RID: 653 RVA: 0x00002E58 File Offset: 0x00002258
	public static uint $ConstGCArrayBound$0x116630ef$80$;

	// Token: 0x0400028E RID: 654 RVA: 0x00002F0C File Offset: 0x0000230C
	public static uint $ConstGCArrayBound$0x116630ef$35$;

	// Token: 0x0400028F RID: 655 RVA: 0x00002E38 File Offset: 0x00002238
	public static uint $ConstGCArrayBound$0x116630ef$88$;

	// Token: 0x04000290 RID: 656 RVA: 0x00002F28 File Offset: 0x00002328
	public static uint $ConstGCArrayBound$0x116630ef$28$;

	// Token: 0x04000291 RID: 657 RVA: 0x00002F70 File Offset: 0x00002370
	public static uint $ConstGCArrayBound$0x116630ef$10$;

	// Token: 0x04000292 RID: 658 RVA: 0x00002EBC File Offset: 0x000022BC
	public static uint $ConstGCArrayBound$0x116630ef$55$;

	// Token: 0x04000293 RID: 659 RVA: 0x00002F48 File Offset: 0x00002348
	public static uint $ConstGCArrayBound$0x116630ef$20$;

	// Token: 0x04000294 RID: 660 RVA: 0x00002DBC File Offset: 0x000021BC
	public static uint $ConstGCArrayBound$0x116630ef$119$;

	// Token: 0x04000295 RID: 661 RVA: 0x00002D98 File Offset: 0x00002198
	public static uint $ConstGCArrayBound$0x116630ef$128$;

	// Token: 0x04000296 RID: 662 RVA: 0x00002E80 File Offset: 0x00002280
	public static uint $ConstGCArrayBound$0x116630ef$70$;

	// Token: 0x04000297 RID: 663 RVA: 0x00002DF4 File Offset: 0x000021F4
	public static uint $ConstGCArrayBound$0x116630ef$105$;

	// Token: 0x04000298 RID: 664 RVA: 0x00002E04 File Offset: 0x00002204
	public static uint $ConstGCArrayBound$0x116630ef$101$;

	// Token: 0x04000299 RID: 665 RVA: 0x00002F90 File Offset: 0x00002390
	public static uint $ConstGCArrayBound$0x116630ef$2$;

	// Token: 0x0400029A RID: 666 RVA: 0x00002EA0 File Offset: 0x000022A0
	public static uint $ConstGCArrayBound$0x116630ef$62$;

	// Token: 0x0400029B RID: 667 RVA: 0x00002F54 File Offset: 0x00002354
	public static uint $ConstGCArrayBound$0x116630ef$17$;

	// Token: 0x0400029C RID: 668 RVA: 0x00002EF4 File Offset: 0x000022F4
	public static uint $ConstGCArrayBound$0x116630ef$41$;

	// Token: 0x0400029D RID: 669 RVA: 0x00002F1C File Offset: 0x0000231C
	public static uint $ConstGCArrayBound$0x116630ef$31$;

	// Token: 0x0400029E RID: 670 RVA: 0x00002F44 File Offset: 0x00002344
	public static uint $ConstGCArrayBound$0x116630ef$21$;

	// Token: 0x0400029F RID: 671 RVA: 0x00002F6C File Offset: 0x0000236C
	public static uint $ConstGCArrayBound$0x116630ef$11$;

	// Token: 0x040002A0 RID: 672 RVA: 0x00002E74 File Offset: 0x00002274
	public static uint $ConstGCArrayBound$0x116630ef$73$;

	// Token: 0x040002A1 RID: 673 RVA: 0x00002EC8 File Offset: 0x000022C8
	public static uint $ConstGCArrayBound$0x116630ef$52$;

	// Token: 0x040002A2 RID: 674 RVA: 0x00002E54 File Offset: 0x00002254
	public static uint $ConstGCArrayBound$0x116630ef$81$;

	// Token: 0x040002A3 RID: 675 RVA: 0x00002F58 File Offset: 0x00002358
	public static uint $ConstGCArrayBound$0x116630ef$16$;

	// Token: 0x040002A4 RID: 676 RVA: 0x00002DA0 File Offset: 0x000021A0
	public static uint $ConstGCArrayBound$0x116630ef$126$;

	// Token: 0x040002A5 RID: 677 RVA: 0x00002F80 File Offset: 0x00002380
	public static uint $ConstGCArrayBound$0x116630ef$6$;

	// Token: 0x040002A6 RID: 678 RVA: 0x00002E6C File Offset: 0x0000226C
	public static uint $ConstGCArrayBound$0x116630ef$75$;

	// Token: 0x040002A7 RID: 679 RVA: 0x00002E00 File Offset: 0x00002200
	public static uint $ConstGCArrayBound$0x116630ef$102$;

	// Token: 0x040002A8 RID: 680 RVA: 0x00002EB8 File Offset: 0x000022B8
	public static uint $ConstGCArrayBound$0x116630ef$56$;

	// Token: 0x040002A9 RID: 681 RVA: 0x00002E14 File Offset: 0x00002214
	public static uint $ConstGCArrayBound$0x116630ef$97$;

	// Token: 0x040002AA RID: 682 RVA: 0x00002E2C File Offset: 0x0000222C
	public static uint $ConstGCArrayBound$0x116630ef$91$;

	// Token: 0x040002AB RID: 683 RVA: 0x00002F34 File Offset: 0x00002334
	public static uint $ConstGCArrayBound$0x116630ef$25$;

	// Token: 0x040002AC RID: 684 RVA: 0x00002E0C File Offset: 0x0000220C
	public static uint $ConstGCArrayBound$0x116630ef$99$;

	// Token: 0x040002AD RID: 685 RVA: 0x00002E9C File Offset: 0x0000229C
	public static uint $ConstGCArrayBound$0x116630ef$63$;

	// Token: 0x040002AE RID: 686 RVA: 0x00002E44 File Offset: 0x00002244
	public static uint $ConstGCArrayBound$0x116630ef$85$;

	// Token: 0x040002AF RID: 687 RVA: 0x00002DB4 File Offset: 0x000021B4
	public static uint $ConstGCArrayBound$0x116630ef$121$;

	// Token: 0x040002B0 RID: 688 RVA: 0x00002EA8 File Offset: 0x000022A8
	public static uint $ConstGCArrayBound$0x116630ef$60$;

	// Token: 0x040002B1 RID: 689 RVA: 0x00002DE8 File Offset: 0x000021E8
	public static uint $ConstGCArrayBound$0x116630ef$108$;

	// Token: 0x040002B2 RID: 690 RVA: 0x00002F08 File Offset: 0x00002308
	public static uint $ConstGCArrayBound$0x116630ef$36$;

	// Token: 0x040002B3 RID: 691 RVA: 0x00002EFC File Offset: 0x000022FC
	public static uint $ConstGCArrayBound$0x116630ef$39$;

	// Token: 0x040002B4 RID: 692 RVA: 0x00002EC4 File Offset: 0x000022C4
	public static uint $ConstGCArrayBound$0x116630ef$53$;

	// Token: 0x040002B5 RID: 693 RVA: 0x00002E4C File Offset: 0x0000224C
	public static uint $ConstGCArrayBound$0x116630ef$83$;

	// Token: 0x040002B6 RID: 694 RVA: 0x00002E90 File Offset: 0x00002290
	public static uint $ConstGCArrayBound$0x116630ef$66$;

	// Token: 0x040002B7 RID: 695 RVA: 0x00002EE8 File Offset: 0x000022E8
	public static uint $ConstGCArrayBound$0x116630ef$44$;

	// Token: 0x040002B8 RID: 696 RVA: 0x00002EB0 File Offset: 0x000022B0
	public static uint $ConstGCArrayBound$0x116630ef$58$;

	// Token: 0x040002B9 RID: 697 RVA: 0x00002F10 File Offset: 0x00002310
	public static uint $ConstGCArrayBound$0x116630ef$34$;

	// Token: 0x040002BA RID: 698 RVA: 0x00002ECC File Offset: 0x000022CC
	public static uint $ConstGCArrayBound$0x116630ef$51$;

	// Token: 0x040002BB RID: 699 RVA: 0x00002DA4 File Offset: 0x000021A4
	public static uint $ConstGCArrayBound$0x116630ef$125$;

	// Token: 0x040002BC RID: 700 RVA: 0x00002ED8 File Offset: 0x000022D8
	public static uint $ConstGCArrayBound$0x116630ef$48$;

	// Token: 0x040002BD RID: 701 RVA: 0x00002DAC File Offset: 0x000021AC
	public static uint $ConstGCArrayBound$0x116630ef$123$;

	// Token: 0x040002BE RID: 702 RVA: 0x00002E28 File Offset: 0x00002228
	public static uint $ConstGCArrayBound$0x116630ef$92$;

	// Token: 0x040002BF RID: 703 RVA: 0x00002DF0 File Offset: 0x000021F0
	public static uint $ConstGCArrayBound$0x116630ef$106$;

	// Token: 0x040002C0 RID: 704 RVA: 0x00002E78 File Offset: 0x00002278
	public static uint $ConstGCArrayBound$0x116630ef$72$;

	// Token: 0x040002C1 RID: 705 RVA: 0x00002DFC File Offset: 0x000021FC
	public static uint $ConstGCArrayBound$0x116630ef$103$;

	// Token: 0x040002C2 RID: 706 RVA: 0x00002F18 File Offset: 0x00002318
	public static uint $ConstGCArrayBound$0x116630ef$32$;

	// Token: 0x040002C3 RID: 707 RVA: 0x00002DC4 File Offset: 0x000021C4
	public static uint $ConstGCArrayBound$0x116630ef$117$;

	// Token: 0x040002C4 RID: 708 RVA: 0x00002DF8 File Offset: 0x000021F8
	public static uint $ConstGCArrayBound$0x116630ef$104$;

	// Token: 0x040002C5 RID: 709 RVA: 0x00002ED0 File Offset: 0x000022D0
	public static uint $ConstGCArrayBound$0x116630ef$50$;

	// Token: 0x040002C6 RID: 710 RVA: 0x00002F40 File Offset: 0x00002340
	public static uint $ConstGCArrayBound$0x116630ef$22$;

	// Token: 0x040002C7 RID: 711 RVA: 0x00002F4C File Offset: 0x0000234C
	public static uint $ConstGCArrayBound$0x116630ef$19$;

	// Token: 0x040002C8 RID: 712 RVA: 0x00002DB0 File Offset: 0x000021B0
	public static uint $ConstGCArrayBound$0x116630ef$122$;

	// Token: 0x040002C9 RID: 713 RVA: 0x00002E88 File Offset: 0x00002288
	public static uint $ConstGCArrayBound$0x116630ef$68$;

	// Token: 0x040002CA RID: 714 RVA: 0x00002EE4 File Offset: 0x000022E4
	public static uint $ConstGCArrayBound$0x116630ef$45$;

	// Token: 0x040002CB RID: 715 RVA: 0x00002F84 File Offset: 0x00002384
	public static uint $ConstGCArrayBound$0x116630ef$5$;

	// Token: 0x040002CC RID: 716 RVA: 0x00002EF8 File Offset: 0x000022F8
	public static uint $ConstGCArrayBound$0x116630ef$40$;

	// Token: 0x040002CD RID: 717 RVA: 0x00002E18 File Offset: 0x00002218
	public static uint $ConstGCArrayBound$0x116630ef$96$;

	// Token: 0x040002CE RID: 718 RVA: 0x00002F20 File Offset: 0x00002320
	public static uint $ConstGCArrayBound$0x116630ef$30$;

	// Token: 0x040002CF RID: 719 RVA: 0x00002DDC File Offset: 0x000021DC
	public static uint $ConstGCArrayBound$0x116630ef$111$;

	// Token: 0x040002D0 RID: 720 RVA: 0x000033B8 File Offset: 0x000027B8
	public static uint $ConstGCArrayBound$0x9979c3ed$66$;

	// Token: 0x040002D1 RID: 721 RVA: 0x00003498 File Offset: 0x00002898
	public static uint $ConstGCArrayBound$0x9979c3ed$10$;

	// Token: 0x040002D2 RID: 722 RVA: 0x00003360 File Offset: 0x00002760
	public static uint $ConstGCArrayBound$0x9979c3ed$88$;

	// Token: 0x040002D3 RID: 723 RVA: 0x00003354 File Offset: 0x00002754
	public static uint $ConstGCArrayBound$0x9979c3ed$91$;

	// Token: 0x040002D4 RID: 724 RVA: 0x00003404 File Offset: 0x00002804
	public static uint $ConstGCArrayBound$0x9979c3ed$47$;

	// Token: 0x040002D5 RID: 725 RVA: 0x0000342C File Offset: 0x0000282C
	public static uint $ConstGCArrayBound$0x9979c3ed$37$;

	// Token: 0x040002D6 RID: 726 RVA: 0x00003448 File Offset: 0x00002848
	public static uint $ConstGCArrayBound$0x9979c3ed$30$;

	// Token: 0x040002D7 RID: 727 RVA: 0x0000345C File Offset: 0x0000285C
	public static uint $ConstGCArrayBound$0x9979c3ed$25$;

	// Token: 0x040002D8 RID: 728 RVA: 0x000032F0 File Offset: 0x000026F0
	public static uint $ConstGCArrayBound$0x9979c3ed$116$;

	// Token: 0x040002D9 RID: 729 RVA: 0x000032C4 File Offset: 0x000026C4
	public static uint $ConstGCArrayBound$0x9979c3ed$127$;

	// Token: 0x040002DA RID: 730 RVA: 0x000033C0 File Offset: 0x000027C0
	public static uint $ConstGCArrayBound$0x9979c3ed$64$;

	// Token: 0x040002DB RID: 731 RVA: 0x000032EC File Offset: 0x000026EC
	public static uint $ConstGCArrayBound$0x9979c3ed$117$;

	// Token: 0x040002DC RID: 732 RVA: 0x0000331C File Offset: 0x0000271C
	public static uint $ConstGCArrayBound$0x9979c3ed$105$;

	// Token: 0x040002DD RID: 733 RVA: 0x00003480 File Offset: 0x00002880
	public static uint $ConstGCArrayBound$0x9979c3ed$16$;

	// Token: 0x040002DE RID: 734 RVA: 0x0000332C File Offset: 0x0000272C
	public static uint $ConstGCArrayBound$0x9979c3ed$101$;

	// Token: 0x040002DF RID: 735 RVA: 0x00003390 File Offset: 0x00002790
	public static uint $ConstGCArrayBound$0x9979c3ed$76$;

	// Token: 0x040002E0 RID: 736 RVA: 0x0000341C File Offset: 0x0000281C
	public static uint $ConstGCArrayBound$0x9979c3ed$41$;

	// Token: 0x040002E1 RID: 737 RVA: 0x000032D0 File Offset: 0x000026D0
	public static uint $ConstGCArrayBound$0x9979c3ed$124$;

	// Token: 0x040002E2 RID: 738 RVA: 0x00003368 File Offset: 0x00002768
	public static uint $ConstGCArrayBound$0x9979c3ed$86$;

	// Token: 0x040002E3 RID: 739 RVA: 0x000032F4 File Offset: 0x000026F4
	public static uint $ConstGCArrayBound$0x9979c3ed$115$;

	// Token: 0x040002E4 RID: 740 RVA: 0x0000343C File Offset: 0x0000283C
	public static uint $ConstGCArrayBound$0x9979c3ed$33$;

	// Token: 0x040002E5 RID: 741 RVA: 0x00003384 File Offset: 0x00002784
	public static uint $ConstGCArrayBound$0x9979c3ed$79$;

	// Token: 0x040002E6 RID: 742 RVA: 0x00003438 File Offset: 0x00002838
	public static uint $ConstGCArrayBound$0x9979c3ed$34$;

	// Token: 0x040002E7 RID: 743 RVA: 0x00003450 File Offset: 0x00002850
	public static uint $ConstGCArrayBound$0x9979c3ed$28$;

	// Token: 0x040002E8 RID: 744 RVA: 0x000032E4 File Offset: 0x000026E4
	public static uint $ConstGCArrayBound$0x9979c3ed$119$;

	// Token: 0x040002E9 RID: 745 RVA: 0x00003300 File Offset: 0x00002700
	public static uint $ConstGCArrayBound$0x9979c3ed$112$;

	// Token: 0x040002EA RID: 746 RVA: 0x0000344C File Offset: 0x0000284C
	public static uint $ConstGCArrayBound$0x9979c3ed$29$;

	// Token: 0x040002EB RID: 747 RVA: 0x00003454 File Offset: 0x00002854
	public static uint $ConstGCArrayBound$0x9979c3ed$27$;

	// Token: 0x040002EC RID: 748 RVA: 0x000033A0 File Offset: 0x000027A0
	public static uint $ConstGCArrayBound$0x9979c3ed$72$;

	// Token: 0x040002ED RID: 749 RVA: 0x000032CC File Offset: 0x000026CC
	public static uint $ConstGCArrayBound$0x9979c3ed$125$;

	// Token: 0x040002EE RID: 750 RVA: 0x00003440 File Offset: 0x00002840
	public static uint $ConstGCArrayBound$0x9979c3ed$32$;

	// Token: 0x040002EF RID: 751 RVA: 0x000033E4 File Offset: 0x000027E4
	public static uint $ConstGCArrayBound$0x9979c3ed$55$;

	// Token: 0x040002F0 RID: 752 RVA: 0x0000340C File Offset: 0x0000280C
	public static uint $ConstGCArrayBound$0x9979c3ed$45$;

	// Token: 0x040002F1 RID: 753 RVA: 0x000033B4 File Offset: 0x000027B4
	public static uint $ConstGCArrayBound$0x9979c3ed$67$;

	// Token: 0x040002F2 RID: 754 RVA: 0x000032E0 File Offset: 0x000026E0
	public static uint $ConstGCArrayBound$0x9979c3ed$120$;

	// Token: 0x040002F3 RID: 755 RVA: 0x00003328 File Offset: 0x00002728
	public static uint $ConstGCArrayBound$0x9979c3ed$102$;

	// Token: 0x040002F4 RID: 756 RVA: 0x000034B8 File Offset: 0x000028B8
	public static uint $ConstGCArrayBound$0x9979c3ed$2$;

	// Token: 0x040002F5 RID: 757 RVA: 0x00003344 File Offset: 0x00002744
	public static uint $ConstGCArrayBound$0x9979c3ed$95$;

	// Token: 0x040002F6 RID: 758 RVA: 0x00003394 File Offset: 0x00002794
	public static uint $ConstGCArrayBound$0x9979c3ed$75$;

	// Token: 0x040002F7 RID: 759 RVA: 0x00003380 File Offset: 0x00002780
	public static uint $ConstGCArrayBound$0x9979c3ed$80$;

	// Token: 0x040002F8 RID: 760 RVA: 0x00003460 File Offset: 0x00002860
	public static uint $ConstGCArrayBound$0x9979c3ed$24$;

	// Token: 0x040002F9 RID: 761 RVA: 0x00003474 File Offset: 0x00002874
	public static uint $ConstGCArrayBound$0x9979c3ed$19$;

	// Token: 0x040002FA RID: 762 RVA: 0x00003330 File Offset: 0x00002730
	public static uint $ConstGCArrayBound$0x9979c3ed$100$;

	// Token: 0x040002FB RID: 763 RVA: 0x00003364 File Offset: 0x00002764
	public static uint $ConstGCArrayBound$0x9979c3ed$87$;

	// Token: 0x040002FC RID: 764 RVA: 0x000034B0 File Offset: 0x000028B0
	public static uint $ConstGCArrayBound$0x9979c3ed$4$;

	// Token: 0x040002FD RID: 765 RVA: 0x000034A8 File Offset: 0x000028A8
	public static uint $ConstGCArrayBound$0x9979c3ed$6$;

	// Token: 0x040002FE RID: 766 RVA: 0x00003458 File Offset: 0x00002858
	public static uint $ConstGCArrayBound$0x9979c3ed$26$;

	// Token: 0x040002FF RID: 767 RVA: 0x00003484 File Offset: 0x00002884
	public static uint $ConstGCArrayBound$0x9979c3ed$15$;

	// Token: 0x04000300 RID: 768 RVA: 0x000033FC File Offset: 0x000027FC
	public static uint $ConstGCArrayBound$0x9979c3ed$49$;

	// Token: 0x04000301 RID: 769 RVA: 0x00003414 File Offset: 0x00002814
	public static uint $ConstGCArrayBound$0x9979c3ed$43$;

	// Token: 0x04000302 RID: 770 RVA: 0x000033F8 File Offset: 0x000027F8
	public static uint $ConstGCArrayBound$0x9979c3ed$50$;

	// Token: 0x04000303 RID: 771 RVA: 0x00003444 File Offset: 0x00002844
	public static uint $ConstGCArrayBound$0x9979c3ed$31$;

	// Token: 0x04000304 RID: 772 RVA: 0x000033D0 File Offset: 0x000027D0
	public static uint $ConstGCArrayBound$0x9979c3ed$60$;

	// Token: 0x04000305 RID: 773 RVA: 0x000032F8 File Offset: 0x000026F8
	public static uint $ConstGCArrayBound$0x9979c3ed$114$;

	// Token: 0x04000306 RID: 774 RVA: 0x00003470 File Offset: 0x00002870
	public static uint $ConstGCArrayBound$0x9979c3ed$20$;

	// Token: 0x04000307 RID: 775 RVA: 0x000033D8 File Offset: 0x000027D8
	public static uint $ConstGCArrayBound$0x9979c3ed$58$;

	// Token: 0x04000308 RID: 776 RVA: 0x000034A0 File Offset: 0x000028A0
	public static uint $ConstGCArrayBound$0x9979c3ed$8$;

	// Token: 0x04000309 RID: 777 RVA: 0x000032DC File Offset: 0x000026DC
	public static uint $ConstGCArrayBound$0x9979c3ed$121$;

	// Token: 0x0400030A RID: 778 RVA: 0x000033AC File Offset: 0x000027AC
	public static uint $ConstGCArrayBound$0x9979c3ed$69$;

	// Token: 0x0400030B RID: 779 RVA: 0x0000339C File Offset: 0x0000279C
	public static uint $ConstGCArrayBound$0x9979c3ed$73$;

	// Token: 0x0400030C RID: 780 RVA: 0x00003304 File Offset: 0x00002704
	public static uint $ConstGCArrayBound$0x9979c3ed$111$;

	// Token: 0x0400030D RID: 781 RVA: 0x00003468 File Offset: 0x00002868
	public static uint $ConstGCArrayBound$0x9979c3ed$22$;

	// Token: 0x0400030E RID: 782 RVA: 0x000033E0 File Offset: 0x000027E0
	public static uint $ConstGCArrayBound$0x9979c3ed$56$;

	// Token: 0x0400030F RID: 783 RVA: 0x000033B0 File Offset: 0x000027B0
	public static uint $ConstGCArrayBound$0x9979c3ed$68$;

	// Token: 0x04000310 RID: 784 RVA: 0x00003434 File Offset: 0x00002834
	public static uint $ConstGCArrayBound$0x9979c3ed$35$;

	// Token: 0x04000311 RID: 785 RVA: 0x00003378 File Offset: 0x00002778
	public static uint $ConstGCArrayBound$0x9979c3ed$82$;

	// Token: 0x04000312 RID: 786 RVA: 0x00003324 File Offset: 0x00002724
	public static uint $ConstGCArrayBound$0x9979c3ed$103$;

	// Token: 0x04000313 RID: 787 RVA: 0x000033C4 File Offset: 0x000027C4
	public static uint $ConstGCArrayBound$0x9979c3ed$63$;

	// Token: 0x04000314 RID: 788 RVA: 0x000033F4 File Offset: 0x000027F4
	public static uint $ConstGCArrayBound$0x9979c3ed$51$;

	// Token: 0x04000315 RID: 789 RVA: 0x0000347C File Offset: 0x0000287C
	public static uint $ConstGCArrayBound$0x9979c3ed$17$;

	// Token: 0x04000316 RID: 790 RVA: 0x00003418 File Offset: 0x00002818
	public static uint $ConstGCArrayBound$0x9979c3ed$42$;

	// Token: 0x04000317 RID: 791 RVA: 0x000033A8 File Offset: 0x000027A8
	public static uint $ConstGCArrayBound$0x9979c3ed$70$;

	// Token: 0x04000318 RID: 792 RVA: 0x00003428 File Offset: 0x00002828
	public static uint $ConstGCArrayBound$0x9979c3ed$38$;

	// Token: 0x04000319 RID: 793 RVA: 0x000033EC File Offset: 0x000027EC
	public static uint $ConstGCArrayBound$0x9979c3ed$53$;

	// Token: 0x0400031A RID: 794 RVA: 0x00003490 File Offset: 0x00002890
	public static uint $ConstGCArrayBound$0x9979c3ed$12$;

	// Token: 0x0400031B RID: 795 RVA: 0x0000346C File Offset: 0x0000286C
	public static uint $ConstGCArrayBound$0x9979c3ed$21$;

	// Token: 0x0400031C RID: 796 RVA: 0x00003430 File Offset: 0x00002830
	public static uint $ConstGCArrayBound$0x9979c3ed$36$;

	// Token: 0x0400031D RID: 797 RVA: 0x00003494 File Offset: 0x00002894
	public static uint $ConstGCArrayBound$0x9979c3ed$11$;

	// Token: 0x0400031E RID: 798 RVA: 0x000034BC File Offset: 0x000028BC
	public static uint $ConstGCArrayBound$0x9979c3ed$1$;

	// Token: 0x0400031F RID: 799 RVA: 0x000032C8 File Offset: 0x000026C8
	public static uint $ConstGCArrayBound$0x9979c3ed$126$;

	// Token: 0x04000320 RID: 800 RVA: 0x00003398 File Offset: 0x00002798
	public static uint $ConstGCArrayBound$0x9979c3ed$74$;

	// Token: 0x04000321 RID: 801 RVA: 0x00003408 File Offset: 0x00002808
	public static uint $ConstGCArrayBound$0x9979c3ed$46$;

	// Token: 0x04000322 RID: 802 RVA: 0x000032C0 File Offset: 0x000026C0
	public static uint $ConstGCArrayBound$0x9979c3ed$128$;

	// Token: 0x04000323 RID: 803 RVA: 0x000033C8 File Offset: 0x000027C8
	public static uint $ConstGCArrayBound$0x9979c3ed$62$;

	// Token: 0x04000324 RID: 804 RVA: 0x0000330C File Offset: 0x0000270C
	public static uint $ConstGCArrayBound$0x9979c3ed$109$;

	// Token: 0x04000325 RID: 805 RVA: 0x000033DC File Offset: 0x000027DC
	public static uint $ConstGCArrayBound$0x9979c3ed$57$;

	// Token: 0x04000326 RID: 806 RVA: 0x00003420 File Offset: 0x00002820
	public static uint $ConstGCArrayBound$0x9979c3ed$40$;

	// Token: 0x04000327 RID: 807 RVA: 0x0000349C File Offset: 0x0000289C
	public static uint $ConstGCArrayBound$0x9979c3ed$9$;

	// Token: 0x04000328 RID: 808 RVA: 0x000033CC File Offset: 0x000027CC
	public static uint $ConstGCArrayBound$0x9979c3ed$61$;

	// Token: 0x04000329 RID: 809 RVA: 0x00003374 File Offset: 0x00002774
	public static uint $ConstGCArrayBound$0x9979c3ed$83$;

	// Token: 0x0400032A RID: 810 RVA: 0x0000334C File Offset: 0x0000274C
	public static uint $ConstGCArrayBound$0x9979c3ed$93$;

	// Token: 0x0400032B RID: 811 RVA: 0x0000335C File Offset: 0x0000275C
	public static uint $ConstGCArrayBound$0x9979c3ed$89$;

	// Token: 0x0400032C RID: 812 RVA: 0x00003308 File Offset: 0x00002708
	public static uint $ConstGCArrayBound$0x9979c3ed$110$;

	// Token: 0x0400032D RID: 813 RVA: 0x00003388 File Offset: 0x00002788
	public static uint $ConstGCArrayBound$0x9979c3ed$78$;

	// Token: 0x0400032E RID: 814 RVA: 0x00003478 File Offset: 0x00002878
	public static uint $ConstGCArrayBound$0x9979c3ed$18$;

	// Token: 0x0400032F RID: 815 RVA: 0x00003318 File Offset: 0x00002718
	public static uint $ConstGCArrayBound$0x9979c3ed$106$;

	// Token: 0x04000330 RID: 816 RVA: 0x000032D4 File Offset: 0x000026D4
	public static uint $ConstGCArrayBound$0x9979c3ed$123$;

	// Token: 0x04000331 RID: 817 RVA: 0x000032FC File Offset: 0x000026FC
	public static uint $ConstGCArrayBound$0x9979c3ed$113$;

	// Token: 0x04000332 RID: 818 RVA: 0x000033E8 File Offset: 0x000027E8
	public static uint $ConstGCArrayBound$0x9979c3ed$54$;

	// Token: 0x04000333 RID: 819 RVA: 0x0000336C File Offset: 0x0000276C
	public static uint $ConstGCArrayBound$0x9979c3ed$85$;

	// Token: 0x04000334 RID: 820 RVA: 0x000034B4 File Offset: 0x000028B4
	public static uint $ConstGCArrayBound$0x9979c3ed$3$;

	// Token: 0x04000335 RID: 821 RVA: 0x00003348 File Offset: 0x00002748
	public static uint $ConstGCArrayBound$0x9979c3ed$94$;

	// Token: 0x04000336 RID: 822 RVA: 0x00003464 File Offset: 0x00002864
	public static uint $ConstGCArrayBound$0x9979c3ed$23$;

	// Token: 0x04000337 RID: 823 RVA: 0x0000348C File Offset: 0x0000288C
	public static uint $ConstGCArrayBound$0x9979c3ed$13$;

	// Token: 0x04000338 RID: 824 RVA: 0x00003358 File Offset: 0x00002758
	public static uint $ConstGCArrayBound$0x9979c3ed$90$;

	// Token: 0x04000339 RID: 825 RVA: 0x000033D4 File Offset: 0x000027D4
	public static uint $ConstGCArrayBound$0x9979c3ed$59$;

	// Token: 0x0400033A RID: 826 RVA: 0x000032D8 File Offset: 0x000026D8
	public static uint $ConstGCArrayBound$0x9979c3ed$122$;

	// Token: 0x0400033B RID: 827 RVA: 0x0000333C File Offset: 0x0000273C
	public static uint $ConstGCArrayBound$0x9979c3ed$97$;

	// Token: 0x0400033C RID: 828 RVA: 0x000033F0 File Offset: 0x000027F0
	public static uint $ConstGCArrayBound$0x9979c3ed$52$;

	// Token: 0x0400033D RID: 829 RVA: 0x0000338C File Offset: 0x0000278C
	public static uint $ConstGCArrayBound$0x9979c3ed$77$;

	// Token: 0x0400033E RID: 830 RVA: 0x00003338 File Offset: 0x00002738
	public static uint $ConstGCArrayBound$0x9979c3ed$98$;

	// Token: 0x0400033F RID: 831 RVA: 0x00003424 File Offset: 0x00002824
	public static uint $ConstGCArrayBound$0x9979c3ed$39$;

	// Token: 0x04000340 RID: 832 RVA: 0x000034AC File Offset: 0x000028AC
	public static uint $ConstGCArrayBound$0x9979c3ed$5$;

	// Token: 0x04000341 RID: 833 RVA: 0x000033BC File Offset: 0x000027BC
	public static uint $ConstGCArrayBound$0x9979c3ed$65$;

	// Token: 0x04000342 RID: 834 RVA: 0x0000337C File Offset: 0x0000277C
	public static uint $ConstGCArrayBound$0x9979c3ed$81$;

	// Token: 0x04000343 RID: 835 RVA: 0x000034A4 File Offset: 0x000028A4
	public static uint $ConstGCArrayBound$0x9979c3ed$7$;

	// Token: 0x04000344 RID: 836 RVA: 0x00003350 File Offset: 0x00002750
	public static uint $ConstGCArrayBound$0x9979c3ed$92$;

	// Token: 0x04000345 RID: 837 RVA: 0x000033A4 File Offset: 0x000027A4
	public static uint $ConstGCArrayBound$0x9979c3ed$71$;

	// Token: 0x04000346 RID: 838 RVA: 0x00003334 File Offset: 0x00002734
	public static uint $ConstGCArrayBound$0x9979c3ed$99$;

	// Token: 0x04000347 RID: 839 RVA: 0x00003488 File Offset: 0x00002888
	public static uint $ConstGCArrayBound$0x9979c3ed$14$;

	// Token: 0x04000348 RID: 840 RVA: 0x000032E8 File Offset: 0x000026E8
	public static uint $ConstGCArrayBound$0x9979c3ed$118$;

	// Token: 0x04000349 RID: 841 RVA: 0x00003314 File Offset: 0x00002714
	public static uint $ConstGCArrayBound$0x9979c3ed$107$;

	// Token: 0x0400034A RID: 842 RVA: 0x00003400 File Offset: 0x00002800
	public static uint $ConstGCArrayBound$0x9979c3ed$48$;

	// Token: 0x0400034B RID: 843 RVA: 0x00003370 File Offset: 0x00002770
	public static uint $ConstGCArrayBound$0x9979c3ed$84$;

	// Token: 0x0400034C RID: 844 RVA: 0x00003410 File Offset: 0x00002810
	public static uint $ConstGCArrayBound$0x9979c3ed$44$;

	// Token: 0x0400034D RID: 845 RVA: 0x00003340 File Offset: 0x00002740
	public static uint $ConstGCArrayBound$0x9979c3ed$96$;

	// Token: 0x0400034E RID: 846 RVA: 0x00003320 File Offset: 0x00002720
	public static uint $ConstGCArrayBound$0x9979c3ed$104$;

	// Token: 0x0400034F RID: 847 RVA: 0x00003310 File Offset: 0x00002710
	public static uint $ConstGCArrayBound$0x9979c3ed$108$;

	// Token: 0x04000350 RID: 848 RVA: 0x000039C0 File Offset: 0x00002DC0
	public static uint $ConstGCArrayBound$0xe199ccd9$11$;

	// Token: 0x04000351 RID: 849 RVA: 0x00003828 File Offset: 0x00002C28
	public static uint $ConstGCArrayBound$0xe199ccd9$113$;

	// Token: 0x04000352 RID: 850 RVA: 0x000039E8 File Offset: 0x00002DE8
	public static uint $ConstGCArrayBound$0xe199ccd9$1$;

	// Token: 0x04000353 RID: 851 RVA: 0x000038CC File Offset: 0x00002CCC
	public static uint $ConstGCArrayBound$0xe199ccd9$72$;

	// Token: 0x04000354 RID: 852 RVA: 0x000039B8 File Offset: 0x00002DB8
	public static uint $ConstGCArrayBound$0xe199ccd9$13$;

	// Token: 0x04000355 RID: 853 RVA: 0x0000395C File Offset: 0x00002D5C
	public static uint $ConstGCArrayBound$0xe199ccd9$36$;

	// Token: 0x04000356 RID: 854 RVA: 0x000038B0 File Offset: 0x00002CB0
	public static uint $ConstGCArrayBound$0xe199ccd9$79$;

	// Token: 0x04000357 RID: 855 RVA: 0x0000398C File Offset: 0x00002D8C
	public static uint $ConstGCArrayBound$0xe199ccd9$24$;

	// Token: 0x04000358 RID: 856 RVA: 0x0000394C File Offset: 0x00002D4C
	public static uint $ConstGCArrayBound$0xe199ccd9$40$;

	// Token: 0x04000359 RID: 857 RVA: 0x00003980 File Offset: 0x00002D80
	public static uint $ConstGCArrayBound$0xe199ccd9$27$;

	// Token: 0x0400035A RID: 858 RVA: 0x00003848 File Offset: 0x00002C48
	public static uint $ConstGCArrayBound$0xe199ccd9$105$;

	// Token: 0x0400035B RID: 859 RVA: 0x00003814 File Offset: 0x00002C14
	public static uint $ConstGCArrayBound$0xe199ccd9$118$;

	// Token: 0x0400035C RID: 860 RVA: 0x00003918 File Offset: 0x00002D18
	public static uint $ConstGCArrayBound$0xe199ccd9$53$;

	// Token: 0x0400035D RID: 861 RVA: 0x000039AC File Offset: 0x00002DAC
	public static uint $ConstGCArrayBound$0xe199ccd9$16$;

	// Token: 0x0400035E RID: 862 RVA: 0x00003808 File Offset: 0x00002C08
	public static uint $ConstGCArrayBound$0xe199ccd9$121$;

	// Token: 0x0400035F RID: 863 RVA: 0x00003890 File Offset: 0x00002C90
	public static uint $ConstGCArrayBound$0xe199ccd9$87$;

	// Token: 0x04000360 RID: 864 RVA: 0x00003830 File Offset: 0x00002C30
	public static uint $ConstGCArrayBound$0xe199ccd9$111$;

	// Token: 0x04000361 RID: 865 RVA: 0x0000387C File Offset: 0x00002C7C
	public static uint $ConstGCArrayBound$0xe199ccd9$92$;

	// Token: 0x04000362 RID: 866 RVA: 0x0000384C File Offset: 0x00002C4C
	public static uint $ConstGCArrayBound$0xe199ccd9$104$;

	// Token: 0x04000363 RID: 867 RVA: 0x000037F4 File Offset: 0x00002BF4
	public static uint $ConstGCArrayBound$0xe199ccd9$126$;

	// Token: 0x04000364 RID: 868 RVA: 0x000038A0 File Offset: 0x00002CA0
	public static uint $ConstGCArrayBound$0xe199ccd9$83$;

	// Token: 0x04000365 RID: 869 RVA: 0x00003884 File Offset: 0x00002C84
	public static uint $ConstGCArrayBound$0xe199ccd9$90$;

	// Token: 0x04000366 RID: 870 RVA: 0x000038AC File Offset: 0x00002CAC
	public static uint $ConstGCArrayBound$0xe199ccd9$80$;

	// Token: 0x04000367 RID: 871 RVA: 0x00003990 File Offset: 0x00002D90
	public static uint $ConstGCArrayBound$0xe199ccd9$23$;

	// Token: 0x04000368 RID: 872 RVA: 0x0000388C File Offset: 0x00002C8C
	public static uint $ConstGCArrayBound$0xe199ccd9$88$;

	// Token: 0x04000369 RID: 873 RVA: 0x000039D0 File Offset: 0x00002DD0
	public static uint $ConstGCArrayBound$0xe199ccd9$7$;

	// Token: 0x0400036A RID: 874 RVA: 0x00003858 File Offset: 0x00002C58
	public static uint $ConstGCArrayBound$0xe199ccd9$101$;

	// Token: 0x0400036B RID: 875 RVA: 0x00003910 File Offset: 0x00002D10
	public static uint $ConstGCArrayBound$0xe199ccd9$55$;

	// Token: 0x0400036C RID: 876 RVA: 0x000038A8 File Offset: 0x00002CA8
	public static uint $ConstGCArrayBound$0xe199ccd9$81$;

	// Token: 0x0400036D RID: 877 RVA: 0x00003914 File Offset: 0x00002D14
	public static uint $ConstGCArrayBound$0xe199ccd9$54$;

	// Token: 0x0400036E RID: 878 RVA: 0x000038EC File Offset: 0x00002CEC
	public static uint $ConstGCArrayBound$0xe199ccd9$64$;

	// Token: 0x0400036F RID: 879 RVA: 0x000038BC File Offset: 0x00002CBC
	public static uint $ConstGCArrayBound$0xe199ccd9$76$;

	// Token: 0x04000370 RID: 880 RVA: 0x000039E4 File Offset: 0x00002DE4
	public static uint $ConstGCArrayBound$0xe199ccd9$2$;

	// Token: 0x04000371 RID: 881 RVA: 0x00003944 File Offset: 0x00002D44
	public static uint $ConstGCArrayBound$0xe199ccd9$42$;

	// Token: 0x04000372 RID: 882 RVA: 0x00003998 File Offset: 0x00002D98
	public static uint $ConstGCArrayBound$0xe199ccd9$21$;

	// Token: 0x04000373 RID: 883 RVA: 0x0000382C File Offset: 0x00002C2C
	public static uint $ConstGCArrayBound$0xe199ccd9$112$;

	// Token: 0x04000374 RID: 884 RVA: 0x00003864 File Offset: 0x00002C64
	public static uint $ConstGCArrayBound$0xe199ccd9$98$;

	// Token: 0x04000375 RID: 885 RVA: 0x000038E8 File Offset: 0x00002CE8
	public static uint $ConstGCArrayBound$0xe199ccd9$65$;

	// Token: 0x04000376 RID: 886 RVA: 0x00003974 File Offset: 0x00002D74
	public static uint $ConstGCArrayBound$0xe199ccd9$30$;

	// Token: 0x04000377 RID: 887 RVA: 0x000038F0 File Offset: 0x00002CF0
	public static uint $ConstGCArrayBound$0xe199ccd9$63$;

	// Token: 0x04000378 RID: 888 RVA: 0x000038E4 File Offset: 0x00002CE4
	public static uint $ConstGCArrayBound$0xe199ccd9$66$;

	// Token: 0x04000379 RID: 889 RVA: 0x0000397C File Offset: 0x00002D7C
	public static uint $ConstGCArrayBound$0xe199ccd9$28$;

	// Token: 0x0400037A RID: 890 RVA: 0x000038DC File Offset: 0x00002CDC
	public static uint $ConstGCArrayBound$0xe199ccd9$68$;

	// Token: 0x0400037B RID: 891 RVA: 0x000039B0 File Offset: 0x00002DB0
	public static uint $ConstGCArrayBound$0xe199ccd9$15$;

	// Token: 0x0400037C RID: 892 RVA: 0x000038B8 File Offset: 0x00002CB8
	public static uint $ConstGCArrayBound$0xe199ccd9$77$;

	// Token: 0x0400037D RID: 893 RVA: 0x0000386C File Offset: 0x00002C6C
	public static uint $ConstGCArrayBound$0xe199ccd9$96$;

	// Token: 0x0400037E RID: 894 RVA: 0x000037F8 File Offset: 0x00002BF8
	public static uint $ConstGCArrayBound$0xe199ccd9$125$;

	// Token: 0x0400037F RID: 895 RVA: 0x000039CC File Offset: 0x00002DCC
	public static uint $ConstGCArrayBound$0xe199ccd9$8$;

	// Token: 0x04000380 RID: 896 RVA: 0x000038FC File Offset: 0x00002CFC
	public static uint $ConstGCArrayBound$0xe199ccd9$60$;

	// Token: 0x04000381 RID: 897 RVA: 0x000038B4 File Offset: 0x00002CB4
	public static uint $ConstGCArrayBound$0xe199ccd9$78$;

	// Token: 0x04000382 RID: 898 RVA: 0x00003800 File Offset: 0x00002C00
	public static uint $ConstGCArrayBound$0xe199ccd9$123$;

	// Token: 0x04000383 RID: 899 RVA: 0x00003928 File Offset: 0x00002D28
	public static uint $ConstGCArrayBound$0xe199ccd9$49$;

	// Token: 0x04000384 RID: 900 RVA: 0x00003934 File Offset: 0x00002D34
	public static uint $ConstGCArrayBound$0xe199ccd9$46$;

	// Token: 0x04000385 RID: 901 RVA: 0x000038D4 File Offset: 0x00002CD4
	public static uint $ConstGCArrayBound$0xe199ccd9$70$;

	// Token: 0x04000386 RID: 902 RVA: 0x0000390C File Offset: 0x00002D0C
	public static uint $ConstGCArrayBound$0xe199ccd9$56$;

	// Token: 0x04000387 RID: 903 RVA: 0x0000389C File Offset: 0x00002C9C
	public static uint $ConstGCArrayBound$0xe199ccd9$84$;

	// Token: 0x04000388 RID: 904 RVA: 0x00003804 File Offset: 0x00002C04
	public static uint $ConstGCArrayBound$0xe199ccd9$122$;

	// Token: 0x04000389 RID: 905 RVA: 0x000039A4 File Offset: 0x00002DA4
	public static uint $ConstGCArrayBound$0xe199ccd9$18$;

	// Token: 0x0400038A RID: 906 RVA: 0x0000399C File Offset: 0x00002D9C
	public static uint $ConstGCArrayBound$0xe199ccd9$20$;

	// Token: 0x0400038B RID: 907 RVA: 0x00003970 File Offset: 0x00002D70
	public static uint $ConstGCArrayBound$0xe199ccd9$31$;

	// Token: 0x0400038C RID: 908 RVA: 0x00003930 File Offset: 0x00002D30
	public static uint $ConstGCArrayBound$0xe199ccd9$47$;

	// Token: 0x0400038D RID: 909 RVA: 0x000037EC File Offset: 0x00002BEC
	public static uint $ConstGCArrayBound$0xe199ccd9$128$;

	// Token: 0x0400038E RID: 910 RVA: 0x0000396C File Offset: 0x00002D6C
	public static uint $ConstGCArrayBound$0xe199ccd9$32$;

	// Token: 0x0400038F RID: 911 RVA: 0x00003984 File Offset: 0x00002D84
	public static uint $ConstGCArrayBound$0xe199ccd9$26$;

	// Token: 0x04000390 RID: 912 RVA: 0x00003854 File Offset: 0x00002C54
	public static uint $ConstGCArrayBound$0xe199ccd9$102$;

	// Token: 0x04000391 RID: 913 RVA: 0x000039B4 File Offset: 0x00002DB4
	public static uint $ConstGCArrayBound$0xe199ccd9$14$;

	// Token: 0x04000392 RID: 914 RVA: 0x000039C4 File Offset: 0x00002DC4
	public static uint $ConstGCArrayBound$0xe199ccd9$10$;

	// Token: 0x04000393 RID: 915 RVA: 0x00003860 File Offset: 0x00002C60
	public static uint $ConstGCArrayBound$0xe199ccd9$99$;

	// Token: 0x04000394 RID: 916 RVA: 0x000037FC File Offset: 0x00002BFC
	public static uint $ConstGCArrayBound$0xe199ccd9$124$;

	// Token: 0x04000395 RID: 917 RVA: 0x00003824 File Offset: 0x00002C24
	public static uint $ConstGCArrayBound$0xe199ccd9$114$;

	// Token: 0x04000396 RID: 918 RVA: 0x000039D8 File Offset: 0x00002DD8
	public static uint $ConstGCArrayBound$0xe199ccd9$5$;

	// Token: 0x04000397 RID: 919 RVA: 0x000039C8 File Offset: 0x00002DC8
	public static uint $ConstGCArrayBound$0xe199ccd9$9$;

	// Token: 0x04000398 RID: 920 RVA: 0x00003900 File Offset: 0x00002D00
	public static uint $ConstGCArrayBound$0xe199ccd9$59$;

	// Token: 0x04000399 RID: 921 RVA: 0x00003908 File Offset: 0x00002D08
	public static uint $ConstGCArrayBound$0xe199ccd9$57$;

	// Token: 0x0400039A RID: 922 RVA: 0x00003950 File Offset: 0x00002D50
	public static uint $ConstGCArrayBound$0xe199ccd9$39$;

	// Token: 0x0400039B RID: 923 RVA: 0x00003904 File Offset: 0x00002D04
	public static uint $ConstGCArrayBound$0xe199ccd9$58$;

	// Token: 0x0400039C RID: 924 RVA: 0x0000391C File Offset: 0x00002D1C
	public static uint $ConstGCArrayBound$0xe199ccd9$52$;

	// Token: 0x0400039D RID: 925 RVA: 0x00003964 File Offset: 0x00002D64
	public static uint $ConstGCArrayBound$0xe199ccd9$34$;

	// Token: 0x0400039E RID: 926 RVA: 0x00003844 File Offset: 0x00002C44
	public static uint $ConstGCArrayBound$0xe199ccd9$106$;

	// Token: 0x0400039F RID: 927 RVA: 0x0000381C File Offset: 0x00002C1C
	public static uint $ConstGCArrayBound$0xe199ccd9$116$;

	// Token: 0x040003A0 RID: 928 RVA: 0x00003960 File Offset: 0x00002D60
	public static uint $ConstGCArrayBound$0xe199ccd9$35$;

	// Token: 0x040003A1 RID: 929 RVA: 0x000039D4 File Offset: 0x00002DD4
	public static uint $ConstGCArrayBound$0xe199ccd9$6$;

	// Token: 0x040003A2 RID: 930 RVA: 0x00003820 File Offset: 0x00002C20
	public static uint $ConstGCArrayBound$0xe199ccd9$115$;

	// Token: 0x040003A3 RID: 931 RVA: 0x000039A8 File Offset: 0x00002DA8
	public static uint $ConstGCArrayBound$0xe199ccd9$17$;

	// Token: 0x040003A4 RID: 932 RVA: 0x000039DC File Offset: 0x00002DDC
	public static uint $ConstGCArrayBound$0xe199ccd9$4$;

	// Token: 0x040003A5 RID: 933 RVA: 0x00003920 File Offset: 0x00002D20
	public static uint $ConstGCArrayBound$0xe199ccd9$51$;

	// Token: 0x040003A6 RID: 934 RVA: 0x00003870 File Offset: 0x00002C70
	public static uint $ConstGCArrayBound$0xe199ccd9$95$;

	// Token: 0x040003A7 RID: 935 RVA: 0x00003988 File Offset: 0x00002D88
	public static uint $ConstGCArrayBound$0xe199ccd9$25$;

	// Token: 0x040003A8 RID: 936 RVA: 0x000037F0 File Offset: 0x00002BF0
	public static uint $ConstGCArrayBound$0xe199ccd9$127$;

	// Token: 0x040003A9 RID: 937 RVA: 0x0000383C File Offset: 0x00002C3C
	public static uint $ConstGCArrayBound$0xe199ccd9$108$;

	// Token: 0x040003AA RID: 938 RVA: 0x000039E0 File Offset: 0x00002DE0
	public static uint $ConstGCArrayBound$0xe199ccd9$3$;

	// Token: 0x040003AB RID: 939 RVA: 0x000039A0 File Offset: 0x00002DA0
	public static uint $ConstGCArrayBound$0xe199ccd9$19$;

	// Token: 0x040003AC RID: 940 RVA: 0x00003968 File Offset: 0x00002D68
	public static uint $ConstGCArrayBound$0xe199ccd9$33$;

	// Token: 0x040003AD RID: 941 RVA: 0x00003888 File Offset: 0x00002C88
	public static uint $ConstGCArrayBound$0xe199ccd9$89$;

	// Token: 0x040003AE RID: 942 RVA: 0x00003994 File Offset: 0x00002D94
	public static uint $ConstGCArrayBound$0xe199ccd9$22$;

	// Token: 0x040003AF RID: 943 RVA: 0x00003894 File Offset: 0x00002C94
	public static uint $ConstGCArrayBound$0xe199ccd9$86$;

	// Token: 0x040003B0 RID: 944 RVA: 0x00003948 File Offset: 0x00002D48
	public static uint $ConstGCArrayBound$0xe199ccd9$41$;

	// Token: 0x040003B1 RID: 945 RVA: 0x0000392C File Offset: 0x00002D2C
	public static uint $ConstGCArrayBound$0xe199ccd9$48$;

	// Token: 0x040003B2 RID: 946 RVA: 0x000038C4 File Offset: 0x00002CC4
	public static uint $ConstGCArrayBound$0xe199ccd9$74$;

	// Token: 0x040003B3 RID: 947 RVA: 0x00003810 File Offset: 0x00002C10
	public static uint $ConstGCArrayBound$0xe199ccd9$119$;

	// Token: 0x040003B4 RID: 948 RVA: 0x00003938 File Offset: 0x00002D38
	public static uint $ConstGCArrayBound$0xe199ccd9$45$;

	// Token: 0x040003B5 RID: 949 RVA: 0x00003954 File Offset: 0x00002D54
	public static uint $ConstGCArrayBound$0xe199ccd9$38$;

	// Token: 0x040003B6 RID: 950 RVA: 0x0000380C File Offset: 0x00002C0C
	public static uint $ConstGCArrayBound$0xe199ccd9$120$;

	// Token: 0x040003B7 RID: 951 RVA: 0x00003898 File Offset: 0x00002C98
	public static uint $ConstGCArrayBound$0xe199ccd9$85$;

	// Token: 0x040003B8 RID: 952 RVA: 0x000038E0 File Offset: 0x00002CE0
	public static uint $ConstGCArrayBound$0xe199ccd9$67$;

	// Token: 0x040003B9 RID: 953 RVA: 0x00003958 File Offset: 0x00002D58
	public static uint $ConstGCArrayBound$0xe199ccd9$37$;

	// Token: 0x040003BA RID: 954 RVA: 0x00003834 File Offset: 0x00002C34
	public static uint $ConstGCArrayBound$0xe199ccd9$110$;

	// Token: 0x040003BB RID: 955 RVA: 0x000038D8 File Offset: 0x00002CD8
	public static uint $ConstGCArrayBound$0xe199ccd9$69$;

	// Token: 0x040003BC RID: 956 RVA: 0x0000385C File Offset: 0x00002C5C
	public static uint $ConstGCArrayBound$0xe199ccd9$100$;

	// Token: 0x040003BD RID: 957 RVA: 0x000039BC File Offset: 0x00002DBC
	public static uint $ConstGCArrayBound$0xe199ccd9$12$;

	// Token: 0x040003BE RID: 958 RVA: 0x00003840 File Offset: 0x00002C40
	public static uint $ConstGCArrayBound$0xe199ccd9$107$;

	// Token: 0x040003BF RID: 959 RVA: 0x000038F4 File Offset: 0x00002CF4
	public static uint $ConstGCArrayBound$0xe199ccd9$62$;

	// Token: 0x040003C0 RID: 960 RVA: 0x000038C0 File Offset: 0x00002CC0
	public static uint $ConstGCArrayBound$0xe199ccd9$75$;

	// Token: 0x040003C1 RID: 961 RVA: 0x000038F8 File Offset: 0x00002CF8
	public static uint $ConstGCArrayBound$0xe199ccd9$61$;

	// Token: 0x040003C2 RID: 962 RVA: 0x00003850 File Offset: 0x00002C50
	public static uint $ConstGCArrayBound$0xe199ccd9$103$;

	// Token: 0x040003C3 RID: 963 RVA: 0x000038C8 File Offset: 0x00002CC8
	public static uint $ConstGCArrayBound$0xe199ccd9$73$;

	// Token: 0x040003C4 RID: 964 RVA: 0x00003924 File Offset: 0x00002D24
	public static uint $ConstGCArrayBound$0xe199ccd9$50$;

	// Token: 0x040003C5 RID: 965 RVA: 0x00003838 File Offset: 0x00002C38
	public static uint $ConstGCArrayBound$0xe199ccd9$109$;

	// Token: 0x040003C6 RID: 966 RVA: 0x00003880 File Offset: 0x00002C80
	public static uint $ConstGCArrayBound$0xe199ccd9$91$;

	// Token: 0x040003C7 RID: 967 RVA: 0x00003940 File Offset: 0x00002D40
	public static uint $ConstGCArrayBound$0xe199ccd9$43$;

	// Token: 0x040003C8 RID: 968 RVA: 0x000038A4 File Offset: 0x00002CA4
	public static uint $ConstGCArrayBound$0xe199ccd9$82$;

	// Token: 0x040003C9 RID: 969 RVA: 0x00003868 File Offset: 0x00002C68
	public static uint $ConstGCArrayBound$0xe199ccd9$97$;

	// Token: 0x040003CA RID: 970 RVA: 0x0000393C File Offset: 0x00002D3C
	public static uint $ConstGCArrayBound$0xe199ccd9$44$;

	// Token: 0x040003CB RID: 971 RVA: 0x00003818 File Offset: 0x00002C18
	public static uint $ConstGCArrayBound$0xe199ccd9$117$;

	// Token: 0x040003CC RID: 972 RVA: 0x00003978 File Offset: 0x00002D78
	public static uint $ConstGCArrayBound$0xe199ccd9$29$;

	// Token: 0x040003CD RID: 973 RVA: 0x00003878 File Offset: 0x00002C78
	public static uint $ConstGCArrayBound$0xe199ccd9$93$;

	// Token: 0x040003CE RID: 974 RVA: 0x000038D0 File Offset: 0x00002CD0
	public static uint $ConstGCArrayBound$0xe199ccd9$71$;

	// Token: 0x040003CF RID: 975 RVA: 0x00003874 File Offset: 0x00002C74
	public static uint $ConstGCArrayBound$0xe199ccd9$94$;

	// Token: 0x040003D0 RID: 976 RVA: 0x00003E7C File Offset: 0x0000327C
	public static uint $ConstGCArrayBound$0x1e2815d5$38$;

	// Token: 0x040003D1 RID: 977 RVA: 0x00003E78 File Offset: 0x00003278
	public static uint $ConstGCArrayBound$0x1e2815d5$39$;

	// Token: 0x040003D2 RID: 978 RVA: 0x00003EA4 File Offset: 0x000032A4
	public static uint $ConstGCArrayBound$0x1e2815d5$28$;

	// Token: 0x040003D3 RID: 979 RVA: 0x00003EB0 File Offset: 0x000032B0
	public static uint $ConstGCArrayBound$0x1e2815d5$25$;

	// Token: 0x040003D4 RID: 980 RVA: 0x00003D6C File Offset: 0x0000316C
	public static uint $ConstGCArrayBound$0x1e2815d5$106$;

	// Token: 0x040003D5 RID: 981 RVA: 0x00003D24 File Offset: 0x00003124
	public static uint $ConstGCArrayBound$0x1e2815d5$124$;

	// Token: 0x040003D6 RID: 982 RVA: 0x00003E68 File Offset: 0x00003268
	public static uint $ConstGCArrayBound$0x1e2815d5$43$;

	// Token: 0x040003D7 RID: 983 RVA: 0x00003EAC File Offset: 0x000032AC
	public static uint $ConstGCArrayBound$0x1e2815d5$26$;

	// Token: 0x040003D8 RID: 984 RVA: 0x00003ED0 File Offset: 0x000032D0
	public static uint $ConstGCArrayBound$0x1e2815d5$17$;

	// Token: 0x040003D9 RID: 985 RVA: 0x00003DC0 File Offset: 0x000031C0
	public static uint $ConstGCArrayBound$0x1e2815d5$85$;

	// Token: 0x040003DA RID: 986 RVA: 0x00003DAC File Offset: 0x000031AC
	public static uint $ConstGCArrayBound$0x1e2815d5$90$;

	// Token: 0x040003DB RID: 987 RVA: 0x00003EF8 File Offset: 0x000032F8
	public static uint $ConstGCArrayBound$0x1e2815d5$7$;

	// Token: 0x040003DC RID: 988 RVA: 0x00003D7C File Offset: 0x0000317C
	public static uint $ConstGCArrayBound$0x1e2815d5$102$;

	// Token: 0x040003DD RID: 989 RVA: 0x00003EC4 File Offset: 0x000032C4
	public static uint $ConstGCArrayBound$0x1e2815d5$20$;

	// Token: 0x040003DE RID: 990 RVA: 0x00003EC8 File Offset: 0x000032C8
	public static uint $ConstGCArrayBound$0x1e2815d5$19$;

	// Token: 0x040003DF RID: 991 RVA: 0x00003D78 File Offset: 0x00003178
	public static uint $ConstGCArrayBound$0x1e2815d5$103$;

	// Token: 0x040003E0 RID: 992 RVA: 0x00003EDC File Offset: 0x000032DC
	public static uint $ConstGCArrayBound$0x1e2815d5$14$;

	// Token: 0x040003E1 RID: 993 RVA: 0x00003D98 File Offset: 0x00003198
	public static uint $ConstGCArrayBound$0x1e2815d5$95$;

	// Token: 0x040003E2 RID: 994 RVA: 0x00003E30 File Offset: 0x00003230
	public static uint $ConstGCArrayBound$0x1e2815d5$57$;

	// Token: 0x040003E3 RID: 995 RVA: 0x00003E5C File Offset: 0x0000325C
	public static uint $ConstGCArrayBound$0x1e2815d5$46$;

	// Token: 0x040003E4 RID: 996 RVA: 0x00003D28 File Offset: 0x00003128
	public static uint $ConstGCArrayBound$0x1e2815d5$123$;

	// Token: 0x040003E5 RID: 997 RVA: 0x00003D4C File Offset: 0x0000314C
	public static uint $ConstGCArrayBound$0x1e2815d5$114$;

	// Token: 0x040003E6 RID: 998 RVA: 0x00003E14 File Offset: 0x00003214
	public static uint $ConstGCArrayBound$0x1e2815d5$64$;

	// Token: 0x040003E7 RID: 999 RVA: 0x00003EBC File Offset: 0x000032BC
	public static uint $ConstGCArrayBound$0x1e2815d5$22$;

	// Token: 0x040003E8 RID: 1000 RVA: 0x00003D18 File Offset: 0x00003118
	public static uint $ConstGCArrayBound$0x1e2815d5$127$;

	// Token: 0x040003E9 RID: 1001 RVA: 0x00003D5C File Offset: 0x0000315C
	public static uint $ConstGCArrayBound$0x1e2815d5$110$;

	// Token: 0x040003EA RID: 1002 RVA: 0x00003E54 File Offset: 0x00003254
	public static uint $ConstGCArrayBound$0x1e2815d5$48$;

	// Token: 0x040003EB RID: 1003 RVA: 0x00003DA0 File Offset: 0x000031A0
	public static uint $ConstGCArrayBound$0x1e2815d5$93$;

	// Token: 0x040003EC RID: 1004 RVA: 0x00003DCC File Offset: 0x000031CC
	public static uint $ConstGCArrayBound$0x1e2815d5$82$;

	// Token: 0x040003ED RID: 1005 RVA: 0x00003DF8 File Offset: 0x000031F8
	public static uint $ConstGCArrayBound$0x1e2815d5$71$;

	// Token: 0x040003EE RID: 1006 RVA: 0x00003EF4 File Offset: 0x000032F4
	public static uint $ConstGCArrayBound$0x1e2815d5$8$;

	// Token: 0x040003EF RID: 1007 RVA: 0x00003DDC File Offset: 0x000031DC
	public static uint $ConstGCArrayBound$0x1e2815d5$78$;

	// Token: 0x040003F0 RID: 1008 RVA: 0x00003E60 File Offset: 0x00003260
	public static uint $ConstGCArrayBound$0x1e2815d5$45$;

	// Token: 0x040003F1 RID: 1009 RVA: 0x00003D50 File Offset: 0x00003150
	public static uint $ConstGCArrayBound$0x1e2815d5$113$;

	// Token: 0x040003F2 RID: 1010 RVA: 0x00003E18 File Offset: 0x00003218
	public static uint $ConstGCArrayBound$0x1e2815d5$63$;

	// Token: 0x040003F3 RID: 1011 RVA: 0x00003EA8 File Offset: 0x000032A8
	public static uint $ConstGCArrayBound$0x1e2815d5$27$;

	// Token: 0x040003F4 RID: 1012 RVA: 0x00003DA8 File Offset: 0x000031A8
	public static uint $ConstGCArrayBound$0x1e2815d5$91$;

	// Token: 0x040003F5 RID: 1013 RVA: 0x00003E58 File Offset: 0x00003258
	public static uint $ConstGCArrayBound$0x1e2815d5$47$;

	// Token: 0x040003F6 RID: 1014 RVA: 0x00003DFC File Offset: 0x000031FC
	public static uint $ConstGCArrayBound$0x1e2815d5$70$;

	// Token: 0x040003F7 RID: 1015 RVA: 0x00003EF0 File Offset: 0x000032F0
	public static uint $ConstGCArrayBound$0x1e2815d5$9$;

	// Token: 0x040003F8 RID: 1016 RVA: 0x00003EEC File Offset: 0x000032EC
	public static uint $ConstGCArrayBound$0x1e2815d5$10$;

	// Token: 0x040003F9 RID: 1017 RVA: 0x00003EA0 File Offset: 0x000032A0
	public static uint $ConstGCArrayBound$0x1e2815d5$29$;

	// Token: 0x040003FA RID: 1018 RVA: 0x00003D60 File Offset: 0x00003160
	public static uint $ConstGCArrayBound$0x1e2815d5$109$;

	// Token: 0x040003FB RID: 1019 RVA: 0x00003D48 File Offset: 0x00003148
	public static uint $ConstGCArrayBound$0x1e2815d5$115$;

	// Token: 0x040003FC RID: 1020 RVA: 0x00003D30 File Offset: 0x00003130
	public static uint $ConstGCArrayBound$0x1e2815d5$121$;

	// Token: 0x040003FD RID: 1021 RVA: 0x00003E4C File Offset: 0x0000324C
	public static uint $ConstGCArrayBound$0x1e2815d5$50$;

	// Token: 0x040003FE RID: 1022 RVA: 0x00003DEC File Offset: 0x000031EC
	public static uint $ConstGCArrayBound$0x1e2815d5$74$;

	// Token: 0x040003FF RID: 1023 RVA: 0x00003D74 File Offset: 0x00003174
	public static uint $ConstGCArrayBound$0x1e2815d5$104$;

	// Token: 0x04000400 RID: 1024 RVA: 0x00003E98 File Offset: 0x00003298
	public static uint $ConstGCArrayBound$0x1e2815d5$31$;

	// Token: 0x04000401 RID: 1025 RVA: 0x00003E9C File Offset: 0x0000329C
	public static uint $ConstGCArrayBound$0x1e2815d5$30$;

	// Token: 0x04000402 RID: 1026 RVA: 0x00003D20 File Offset: 0x00003120
	public static uint $ConstGCArrayBound$0x1e2815d5$125$;

	// Token: 0x04000403 RID: 1027 RVA: 0x00003D68 File Offset: 0x00003168
	public static uint $ConstGCArrayBound$0x1e2815d5$107$;

	// Token: 0x04000404 RID: 1028 RVA: 0x00003E74 File Offset: 0x00003274
	public static uint $ConstGCArrayBound$0x1e2815d5$40$;

	// Token: 0x04000405 RID: 1029 RVA: 0x00003E38 File Offset: 0x00003238
	public static uint $ConstGCArrayBound$0x1e2815d5$55$;

	// Token: 0x04000406 RID: 1030 RVA: 0x00003D9C File Offset: 0x0000319C
	public static uint $ConstGCArrayBound$0x1e2815d5$94$;

	// Token: 0x04000407 RID: 1031 RVA: 0x00003DE8 File Offset: 0x000031E8
	public static uint $ConstGCArrayBound$0x1e2815d5$75$;

	// Token: 0x04000408 RID: 1032 RVA: 0x00003E28 File Offset: 0x00003228
	public static uint $ConstGCArrayBound$0x1e2815d5$59$;

	// Token: 0x04000409 RID: 1033 RVA: 0x00003DB8 File Offset: 0x000031B8
	public static uint $ConstGCArrayBound$0x1e2815d5$87$;

	// Token: 0x0400040A RID: 1034 RVA: 0x00003D1C File Offset: 0x0000311C
	public static uint $ConstGCArrayBound$0x1e2815d5$126$;

	// Token: 0x0400040B RID: 1035 RVA: 0x00003E40 File Offset: 0x00003240
	public static uint $ConstGCArrayBound$0x1e2815d5$53$;

	// Token: 0x0400040C RID: 1036 RVA: 0x00003E0C File Offset: 0x0000320C
	public static uint $ConstGCArrayBound$0x1e2815d5$66$;

	// Token: 0x0400040D RID: 1037 RVA: 0x00003D44 File Offset: 0x00003144
	public static uint $ConstGCArrayBound$0x1e2815d5$116$;

	// Token: 0x0400040E RID: 1038 RVA: 0x00003D64 File Offset: 0x00003164
	public static uint $ConstGCArrayBound$0x1e2815d5$108$;

	// Token: 0x0400040F RID: 1039 RVA: 0x00003E6C File Offset: 0x0000326C
	public static uint $ConstGCArrayBound$0x1e2815d5$42$;

	// Token: 0x04000410 RID: 1040 RVA: 0x00003EE4 File Offset: 0x000032E4
	public static uint $ConstGCArrayBound$0x1e2815d5$12$;

	// Token: 0x04000411 RID: 1041 RVA: 0x00003DB0 File Offset: 0x000031B0
	public static uint $ConstGCArrayBound$0x1e2815d5$89$;

	// Token: 0x04000412 RID: 1042 RVA: 0x00003E48 File Offset: 0x00003248
	public static uint $ConstGCArrayBound$0x1e2815d5$51$;

	// Token: 0x04000413 RID: 1043 RVA: 0x00003DA4 File Offset: 0x000031A4
	public static uint $ConstGCArrayBound$0x1e2815d5$92$;

	// Token: 0x04000414 RID: 1044 RVA: 0x00003E24 File Offset: 0x00003224
	public static uint $ConstGCArrayBound$0x1e2815d5$60$;

	// Token: 0x04000415 RID: 1045 RVA: 0x00003DF0 File Offset: 0x000031F0
	public static uint $ConstGCArrayBound$0x1e2815d5$73$;

	// Token: 0x04000416 RID: 1046 RVA: 0x00003E64 File Offset: 0x00003264
	public static uint $ConstGCArrayBound$0x1e2815d5$44$;

	// Token: 0x04000417 RID: 1047 RVA: 0x00003E8C File Offset: 0x0000328C
	public static uint $ConstGCArrayBound$0x1e2815d5$34$;

	// Token: 0x04000418 RID: 1048 RVA: 0x00003D94 File Offset: 0x00003194
	public static uint $ConstGCArrayBound$0x1e2815d5$96$;

	// Token: 0x04000419 RID: 1049 RVA: 0x00003E10 File Offset: 0x00003210
	public static uint $ConstGCArrayBound$0x1e2815d5$65$;

	// Token: 0x0400041A RID: 1050 RVA: 0x00003D88 File Offset: 0x00003188
	public static uint $ConstGCArrayBound$0x1e2815d5$99$;

	// Token: 0x0400041B RID: 1051 RVA: 0x00003DC4 File Offset: 0x000031C4
	public static uint $ConstGCArrayBound$0x1e2815d5$84$;

	// Token: 0x0400041C RID: 1052 RVA: 0x00003D58 File Offset: 0x00003158
	public static uint $ConstGCArrayBound$0x1e2815d5$111$;

	// Token: 0x0400041D RID: 1053 RVA: 0x00003D38 File Offset: 0x00003138
	public static uint $ConstGCArrayBound$0x1e2815d5$119$;

	// Token: 0x0400041E RID: 1054 RVA: 0x00003E90 File Offset: 0x00003290
	public static uint $ConstGCArrayBound$0x1e2815d5$33$;

	// Token: 0x0400041F RID: 1055 RVA: 0x00003E2C File Offset: 0x0000322C
	public static uint $ConstGCArrayBound$0x1e2815d5$58$;

	// Token: 0x04000420 RID: 1056 RVA: 0x00003EC0 File Offset: 0x000032C0
	public static uint $ConstGCArrayBound$0x1e2815d5$21$;

	// Token: 0x04000421 RID: 1057 RVA: 0x00003E94 File Offset: 0x00003294
	public static uint $ConstGCArrayBound$0x1e2815d5$32$;

	// Token: 0x04000422 RID: 1058 RVA: 0x00003D2C File Offset: 0x0000312C
	public static uint $ConstGCArrayBound$0x1e2815d5$122$;

	// Token: 0x04000423 RID: 1059 RVA: 0x00003DD0 File Offset: 0x000031D0
	public static uint $ConstGCArrayBound$0x1e2815d5$81$;

	// Token: 0x04000424 RID: 1060 RVA: 0x00003E88 File Offset: 0x00003288
	public static uint $ConstGCArrayBound$0x1e2815d5$35$;

	// Token: 0x04000425 RID: 1061 RVA: 0x00003ED8 File Offset: 0x000032D8
	public static uint $ConstGCArrayBound$0x1e2815d5$15$;

	// Token: 0x04000426 RID: 1062 RVA: 0x00003D14 File Offset: 0x00003114
	public static uint $ConstGCArrayBound$0x1e2815d5$128$;

	// Token: 0x04000427 RID: 1063 RVA: 0x00003DD4 File Offset: 0x000031D4
	public static uint $ConstGCArrayBound$0x1e2815d5$80$;

	// Token: 0x04000428 RID: 1064 RVA: 0x00003F0C File Offset: 0x0000330C
	public static uint $ConstGCArrayBound$0x1e2815d5$2$;

	// Token: 0x04000429 RID: 1065 RVA: 0x00003E34 File Offset: 0x00003234
	public static uint $ConstGCArrayBound$0x1e2815d5$56$;

	// Token: 0x0400042A RID: 1066 RVA: 0x00003E84 File Offset: 0x00003284
	public static uint $ConstGCArrayBound$0x1e2815d5$36$;

	// Token: 0x0400042B RID: 1067 RVA: 0x00003EE0 File Offset: 0x000032E0
	public static uint $ConstGCArrayBound$0x1e2815d5$13$;

	// Token: 0x0400042C RID: 1068 RVA: 0x00003D80 File Offset: 0x00003180
	public static uint $ConstGCArrayBound$0x1e2815d5$101$;

	// Token: 0x0400042D RID: 1069 RVA: 0x00003DE4 File Offset: 0x000031E4
	public static uint $ConstGCArrayBound$0x1e2815d5$76$;

	// Token: 0x0400042E RID: 1070 RVA: 0x00003D54 File Offset: 0x00003154
	public static uint $ConstGCArrayBound$0x1e2815d5$112$;

	// Token: 0x0400042F RID: 1071 RVA: 0x00003D40 File Offset: 0x00003140
	public static uint $ConstGCArrayBound$0x1e2815d5$117$;

	// Token: 0x04000430 RID: 1072 RVA: 0x00003DF4 File Offset: 0x000031F4
	public static uint $ConstGCArrayBound$0x1e2815d5$72$;

	// Token: 0x04000431 RID: 1073 RVA: 0x00003E3C File Offset: 0x0000323C
	public static uint $ConstGCArrayBound$0x1e2815d5$54$;

	// Token: 0x04000432 RID: 1074 RVA: 0x00003DD8 File Offset: 0x000031D8
	public static uint $ConstGCArrayBound$0x1e2815d5$79$;

	// Token: 0x04000433 RID: 1075 RVA: 0x00003EE8 File Offset: 0x000032E8
	public static uint $ConstGCArrayBound$0x1e2815d5$11$;

	// Token: 0x04000434 RID: 1076 RVA: 0x00003D84 File Offset: 0x00003184
	public static uint $ConstGCArrayBound$0x1e2815d5$100$;

	// Token: 0x04000435 RID: 1077 RVA: 0x00003E50 File Offset: 0x00003250
	public static uint $ConstGCArrayBound$0x1e2815d5$49$;

	// Token: 0x04000436 RID: 1078 RVA: 0x00003E80 File Offset: 0x00003280
	public static uint $ConstGCArrayBound$0x1e2815d5$37$;

	// Token: 0x04000437 RID: 1079 RVA: 0x00003E1C File Offset: 0x0000321C
	public static uint $ConstGCArrayBound$0x1e2815d5$62$;

	// Token: 0x04000438 RID: 1080 RVA: 0x00003F08 File Offset: 0x00003308
	public static uint $ConstGCArrayBound$0x1e2815d5$3$;

	// Token: 0x04000439 RID: 1081 RVA: 0x00003E44 File Offset: 0x00003244
	public static uint $ConstGCArrayBound$0x1e2815d5$52$;

	// Token: 0x0400043A RID: 1082 RVA: 0x00003DBC File Offset: 0x000031BC
	public static uint $ConstGCArrayBound$0x1e2815d5$86$;

	// Token: 0x0400043B RID: 1083 RVA: 0x00003D70 File Offset: 0x00003170
	public static uint $ConstGCArrayBound$0x1e2815d5$105$;

	// Token: 0x0400043C RID: 1084 RVA: 0x00003DE0 File Offset: 0x000031E0
	public static uint $ConstGCArrayBound$0x1e2815d5$77$;

	// Token: 0x0400043D RID: 1085 RVA: 0x00003E08 File Offset: 0x00003208
	public static uint $ConstGCArrayBound$0x1e2815d5$67$;

	// Token: 0x0400043E RID: 1086 RVA: 0x00003F00 File Offset: 0x00003300
	public static uint $ConstGCArrayBound$0x1e2815d5$5$;

	// Token: 0x0400043F RID: 1087 RVA: 0x00003D8C File Offset: 0x0000318C
	public static uint $ConstGCArrayBound$0x1e2815d5$98$;

	// Token: 0x04000440 RID: 1088 RVA: 0x00003ED4 File Offset: 0x000032D4
	public static uint $ConstGCArrayBound$0x1e2815d5$16$;

	// Token: 0x04000441 RID: 1089 RVA: 0x00003D90 File Offset: 0x00003190
	public static uint $ConstGCArrayBound$0x1e2815d5$97$;

	// Token: 0x04000442 RID: 1090 RVA: 0x00003D34 File Offset: 0x00003134
	public static uint $ConstGCArrayBound$0x1e2815d5$120$;

	// Token: 0x04000443 RID: 1091 RVA: 0x00003E04 File Offset: 0x00003204
	public static uint $ConstGCArrayBound$0x1e2815d5$68$;

	// Token: 0x04000444 RID: 1092 RVA: 0x00003ECC File Offset: 0x000032CC
	public static uint $ConstGCArrayBound$0x1e2815d5$18$;

	// Token: 0x04000445 RID: 1093 RVA: 0x00003EB8 File Offset: 0x000032B8
	public static uint $ConstGCArrayBound$0x1e2815d5$23$;

	// Token: 0x04000446 RID: 1094 RVA: 0x00003EFC File Offset: 0x000032FC
	public static uint $ConstGCArrayBound$0x1e2815d5$6$;

	// Token: 0x04000447 RID: 1095 RVA: 0x00003E00 File Offset: 0x00003200
	public static uint $ConstGCArrayBound$0x1e2815d5$69$;

	// Token: 0x04000448 RID: 1096 RVA: 0x00003F10 File Offset: 0x00003310
	public static uint $ConstGCArrayBound$0x1e2815d5$1$;

	// Token: 0x04000449 RID: 1097 RVA: 0x00003E20 File Offset: 0x00003220
	public static uint $ConstGCArrayBound$0x1e2815d5$61$;

	// Token: 0x0400044A RID: 1098 RVA: 0x00003E70 File Offset: 0x00003270
	public static uint $ConstGCArrayBound$0x1e2815d5$41$;

	// Token: 0x0400044B RID: 1099 RVA: 0x00003DB4 File Offset: 0x000031B4
	public static uint $ConstGCArrayBound$0x1e2815d5$88$;

	// Token: 0x0400044C RID: 1100 RVA: 0x00003EB4 File Offset: 0x000032B4
	public static uint $ConstGCArrayBound$0x1e2815d5$24$;

	// Token: 0x0400044D RID: 1101 RVA: 0x00003D3C File Offset: 0x0000313C
	public static uint $ConstGCArrayBound$0x1e2815d5$118$;

	// Token: 0x0400044E RID: 1102 RVA: 0x00003DC8 File Offset: 0x000031C8
	public static uint $ConstGCArrayBound$0x1e2815d5$83$;

	// Token: 0x0400044F RID: 1103 RVA: 0x00003F04 File Offset: 0x00003304
	public static uint $ConstGCArrayBound$0x1e2815d5$4$;

	// Token: 0x04000450 RID: 1104 RVA: 0x000043B0 File Offset: 0x000037B0
	public static uint $ConstGCArrayBound$0x97bbb308$35$;

	// Token: 0x04000451 RID: 1105 RVA: 0x00004314 File Offset: 0x00003714
	public static uint $ConstGCArrayBound$0x97bbb308$74$;

	// Token: 0x04000452 RID: 1106 RVA: 0x00004350 File Offset: 0x00003750
	public static uint $ConstGCArrayBound$0x97bbb308$59$;

	// Token: 0x04000453 RID: 1107 RVA: 0x00004384 File Offset: 0x00003784
	public static uint $ConstGCArrayBound$0x97bbb308$46$;

	// Token: 0x04000454 RID: 1108 RVA: 0x000043A8 File Offset: 0x000037A8
	public static uint $ConstGCArrayBound$0x97bbb308$37$;

	// Token: 0x04000455 RID: 1109 RVA: 0x0000425C File Offset: 0x0000365C
	public static uint $ConstGCArrayBound$0x97bbb308$120$;

	// Token: 0x04000456 RID: 1110 RVA: 0x00004264 File Offset: 0x00003664
	public static uint $ConstGCArrayBound$0x97bbb308$118$;

	// Token: 0x04000457 RID: 1111 RVA: 0x000043B4 File Offset: 0x000037B4
	public static uint $ConstGCArrayBound$0x97bbb308$34$;

	// Token: 0x04000458 RID: 1112 RVA: 0x000043C0 File Offset: 0x000037C0
	public static uint $ConstGCArrayBound$0x97bbb308$31$;

	// Token: 0x04000459 RID: 1113 RVA: 0x000042E4 File Offset: 0x000036E4
	public static uint $ConstGCArrayBound$0x97bbb308$86$;

	// Token: 0x0400045A RID: 1114 RVA: 0x00004388 File Offset: 0x00003788
	public static uint $ConstGCArrayBound$0x97bbb308$45$;

	// Token: 0x0400045B RID: 1115 RVA: 0x00004394 File Offset: 0x00003794
	public static uint $ConstGCArrayBound$0x97bbb308$42$;

	// Token: 0x0400045C RID: 1116 RVA: 0x00004288 File Offset: 0x00003688
	public static uint $ConstGCArrayBound$0x97bbb308$109$;

	// Token: 0x0400045D RID: 1117 RVA: 0x000042A8 File Offset: 0x000036A8
	public static uint $ConstGCArrayBound$0x97bbb308$101$;

	// Token: 0x0400045E RID: 1118 RVA: 0x00004280 File Offset: 0x00003680
	public static uint $ConstGCArrayBound$0x97bbb308$111$;

	// Token: 0x0400045F RID: 1119 RVA: 0x000043B8 File Offset: 0x000037B8
	public static uint $ConstGCArrayBound$0x97bbb308$33$;

	// Token: 0x04000460 RID: 1120 RVA: 0x00004344 File Offset: 0x00003744
	public static uint $ConstGCArrayBound$0x97bbb308$62$;

	// Token: 0x04000461 RID: 1121 RVA: 0x000043CC File Offset: 0x000037CC
	public static uint $ConstGCArrayBound$0x97bbb308$28$;

	// Token: 0x04000462 RID: 1122 RVA: 0x00004338 File Offset: 0x00003738
	public static uint $ConstGCArrayBound$0x97bbb308$65$;

	// Token: 0x04000463 RID: 1123 RVA: 0x0000431C File Offset: 0x0000371C
	public static uint $ConstGCArrayBound$0x97bbb308$72$;

	// Token: 0x04000464 RID: 1124 RVA: 0x000042CC File Offset: 0x000036CC
	public static uint $ConstGCArrayBound$0x97bbb308$92$;

	// Token: 0x04000465 RID: 1125 RVA: 0x00004434 File Offset: 0x00003834
	public static uint $ConstGCArrayBound$0x97bbb308$2$;

	// Token: 0x04000466 RID: 1126 RVA: 0x00004278 File Offset: 0x00003678
	public static uint $ConstGCArrayBound$0x97bbb308$113$;

	// Token: 0x04000467 RID: 1127 RVA: 0x000043C4 File Offset: 0x000037C4
	public static uint $ConstGCArrayBound$0x97bbb308$30$;

	// Token: 0x04000468 RID: 1128 RVA: 0x000043D0 File Offset: 0x000037D0
	public static uint $ConstGCArrayBound$0x97bbb308$27$;

	// Token: 0x04000469 RID: 1129 RVA: 0x0000436C File Offset: 0x0000376C
	public static uint $ConstGCArrayBound$0x97bbb308$52$;

	// Token: 0x0400046A RID: 1130 RVA: 0x00004304 File Offset: 0x00003704
	public static uint $ConstGCArrayBound$0x97bbb308$78$;

	// Token: 0x0400046B RID: 1131 RVA: 0x00004268 File Offset: 0x00003668
	public static uint $ConstGCArrayBound$0x97bbb308$117$;

	// Token: 0x0400046C RID: 1132 RVA: 0x000042D0 File Offset: 0x000036D0
	public static uint $ConstGCArrayBound$0x97bbb308$91$;

	// Token: 0x0400046D RID: 1133 RVA: 0x000042C4 File Offset: 0x000036C4
	public static uint $ConstGCArrayBound$0x97bbb308$94$;

	// Token: 0x0400046E RID: 1134 RVA: 0x000042A0 File Offset: 0x000036A0
	public static uint $ConstGCArrayBound$0x97bbb308$103$;

	// Token: 0x0400046F RID: 1135 RVA: 0x000043EC File Offset: 0x000037EC
	public static uint $ConstGCArrayBound$0x97bbb308$20$;

	// Token: 0x04000470 RID: 1136 RVA: 0x00004318 File Offset: 0x00003718
	public static uint $ConstGCArrayBound$0x97bbb308$73$;

	// Token: 0x04000471 RID: 1137 RVA: 0x000042DC File Offset: 0x000036DC
	public static uint $ConstGCArrayBound$0x97bbb308$88$;

	// Token: 0x04000472 RID: 1138 RVA: 0x00004358 File Offset: 0x00003758
	public static uint $ConstGCArrayBound$0x97bbb308$57$;

	// Token: 0x04000473 RID: 1139 RVA: 0x000043A0 File Offset: 0x000037A0
	public static uint $ConstGCArrayBound$0x97bbb308$39$;

	// Token: 0x04000474 RID: 1140 RVA: 0x00004310 File Offset: 0x00003710
	public static uint $ConstGCArrayBound$0x97bbb308$75$;

	// Token: 0x04000475 RID: 1141 RVA: 0x0000434C File Offset: 0x0000374C
	public static uint $ConstGCArrayBound$0x97bbb308$60$;

	// Token: 0x04000476 RID: 1142 RVA: 0x00004294 File Offset: 0x00003694
	public static uint $ConstGCArrayBound$0x97bbb308$106$;

	// Token: 0x04000477 RID: 1143 RVA: 0x00004374 File Offset: 0x00003774
	public static uint $ConstGCArrayBound$0x97bbb308$50$;

	// Token: 0x04000478 RID: 1144 RVA: 0x00004320 File Offset: 0x00003720
	public static uint $ConstGCArrayBound$0x97bbb308$71$;

	// Token: 0x04000479 RID: 1145 RVA: 0x00004438 File Offset: 0x00003838
	public static uint $ConstGCArrayBound$0x97bbb308$1$;

	// Token: 0x0400047A RID: 1146 RVA: 0x000042D4 File Offset: 0x000036D4
	public static uint $ConstGCArrayBound$0x97bbb308$90$;

	// Token: 0x0400047B RID: 1147 RVA: 0x000043DC File Offset: 0x000037DC
	public static uint $ConstGCArrayBound$0x97bbb308$24$;

	// Token: 0x0400047C RID: 1148 RVA: 0x000042E0 File Offset: 0x000036E0
	public static uint $ConstGCArrayBound$0x97bbb308$87$;

	// Token: 0x0400047D RID: 1149 RVA: 0x0000435C File Offset: 0x0000375C
	public static uint $ConstGCArrayBound$0x97bbb308$56$;

	// Token: 0x0400047E RID: 1150 RVA: 0x00004240 File Offset: 0x00003640
	public static uint $ConstGCArrayBound$0x97bbb308$127$;

	// Token: 0x0400047F RID: 1151 RVA: 0x000042D8 File Offset: 0x000036D8
	public static uint $ConstGCArrayBound$0x97bbb308$89$;

	// Token: 0x04000480 RID: 1152 RVA: 0x00004420 File Offset: 0x00003820
	public static uint $ConstGCArrayBound$0x97bbb308$7$;

	// Token: 0x04000481 RID: 1153 RVA: 0x0000429C File Offset: 0x0000369C
	public static uint $ConstGCArrayBound$0x97bbb308$104$;

	// Token: 0x04000482 RID: 1154 RVA: 0x0000428C File Offset: 0x0000368C
	public static uint $ConstGCArrayBound$0x97bbb308$108$;

	// Token: 0x04000483 RID: 1155 RVA: 0x00004284 File Offset: 0x00003684
	public static uint $ConstGCArrayBound$0x97bbb308$110$;

	// Token: 0x04000484 RID: 1156 RVA: 0x000043E4 File Offset: 0x000037E4
	public static uint $ConstGCArrayBound$0x97bbb308$22$;

	// Token: 0x04000485 RID: 1157 RVA: 0x000043F4 File Offset: 0x000037F4
	public static uint $ConstGCArrayBound$0x97bbb308$18$;

	// Token: 0x04000486 RID: 1158 RVA: 0x00004300 File Offset: 0x00003700
	public static uint $ConstGCArrayBound$0x97bbb308$79$;

	// Token: 0x04000487 RID: 1159 RVA: 0x000043F0 File Offset: 0x000037F0
	public static uint $ConstGCArrayBound$0x97bbb308$19$;

	// Token: 0x04000488 RID: 1160 RVA: 0x00004414 File Offset: 0x00003814
	public static uint $ConstGCArrayBound$0x97bbb308$10$;

	// Token: 0x04000489 RID: 1161 RVA: 0x000042E8 File Offset: 0x000036E8
	public static uint $ConstGCArrayBound$0x97bbb308$85$;

	// Token: 0x0400048A RID: 1162 RVA: 0x00004400 File Offset: 0x00003800
	public static uint $ConstGCArrayBound$0x97bbb308$15$;

	// Token: 0x0400048B RID: 1163 RVA: 0x00004254 File Offset: 0x00003654
	public static uint $ConstGCArrayBound$0x97bbb308$122$;

	// Token: 0x0400048C RID: 1164 RVA: 0x00004370 File Offset: 0x00003770
	public static uint $ConstGCArrayBound$0x97bbb308$51$;

	// Token: 0x0400048D RID: 1165 RVA: 0x00004340 File Offset: 0x00003740
	public static uint $ConstGCArrayBound$0x97bbb308$63$;

	// Token: 0x0400048E RID: 1166 RVA: 0x000042A4 File Offset: 0x000036A4
	public static uint $ConstGCArrayBound$0x97bbb308$102$;

	// Token: 0x0400048F RID: 1167 RVA: 0x00004298 File Offset: 0x00003698
	public static uint $ConstGCArrayBound$0x97bbb308$105$;

	// Token: 0x04000490 RID: 1168 RVA: 0x00004380 File Offset: 0x00003780
	public static uint $ConstGCArrayBound$0x97bbb308$47$;

	// Token: 0x04000491 RID: 1169 RVA: 0x00004250 File Offset: 0x00003650
	public static uint $ConstGCArrayBound$0x97bbb308$123$;

	// Token: 0x04000492 RID: 1170 RVA: 0x00004418 File Offset: 0x00003818
	public static uint $ConstGCArrayBound$0x97bbb308$9$;

	// Token: 0x04000493 RID: 1171 RVA: 0x00004258 File Offset: 0x00003658
	public static uint $ConstGCArrayBound$0x97bbb308$121$;

	// Token: 0x04000494 RID: 1172 RVA: 0x000042F0 File Offset: 0x000036F0
	public static uint $ConstGCArrayBound$0x97bbb308$83$;

	// Token: 0x04000495 RID: 1173 RVA: 0x000042B0 File Offset: 0x000036B0
	public static uint $ConstGCArrayBound$0x97bbb308$99$;

	// Token: 0x04000496 RID: 1174 RVA: 0x0000432C File Offset: 0x0000372C
	public static uint $ConstGCArrayBound$0x97bbb308$68$;

	// Token: 0x04000497 RID: 1175 RVA: 0x00004308 File Offset: 0x00003708
	public static uint $ConstGCArrayBound$0x97bbb308$77$;

	// Token: 0x04000498 RID: 1176 RVA: 0x00004424 File Offset: 0x00003824
	public static uint $ConstGCArrayBound$0x97bbb308$6$;

	// Token: 0x04000499 RID: 1177 RVA: 0x0000430C File Offset: 0x0000370C
	public static uint $ConstGCArrayBound$0x97bbb308$76$;

	// Token: 0x0400049A RID: 1178 RVA: 0x0000438C File Offset: 0x0000378C
	public static uint $ConstGCArrayBound$0x97bbb308$44$;

	// Token: 0x0400049B RID: 1179 RVA: 0x000042FC File Offset: 0x000036FC
	public static uint $ConstGCArrayBound$0x97bbb308$80$;

	// Token: 0x0400049C RID: 1180 RVA: 0x000043F8 File Offset: 0x000037F8
	public static uint $ConstGCArrayBound$0x97bbb308$17$;

	// Token: 0x0400049D RID: 1181 RVA: 0x000043A4 File Offset: 0x000037A4
	public static uint $ConstGCArrayBound$0x97bbb308$38$;

	// Token: 0x0400049E RID: 1182 RVA: 0x00004330 File Offset: 0x00003730
	public static uint $ConstGCArrayBound$0x97bbb308$67$;

	// Token: 0x0400049F RID: 1183 RVA: 0x000042B8 File Offset: 0x000036B8
	public static uint $ConstGCArrayBound$0x97bbb308$97$;

	// Token: 0x040004A0 RID: 1184 RVA: 0x000042C0 File Offset: 0x000036C0
	public static uint $ConstGCArrayBound$0x97bbb308$95$;

	// Token: 0x040004A1 RID: 1185 RVA: 0x0000423C File Offset: 0x0000363C
	public static uint $ConstGCArrayBound$0x97bbb308$128$;

	// Token: 0x040004A2 RID: 1186 RVA: 0x00004334 File Offset: 0x00003734
	public static uint $ConstGCArrayBound$0x97bbb308$66$;

	// Token: 0x040004A3 RID: 1187 RVA: 0x000043FC File Offset: 0x000037FC
	public static uint $ConstGCArrayBound$0x97bbb308$16$;

	// Token: 0x040004A4 RID: 1188 RVA: 0x00004328 File Offset: 0x00003728
	public static uint $ConstGCArrayBound$0x97bbb308$69$;

	// Token: 0x040004A5 RID: 1189 RVA: 0x00004360 File Offset: 0x00003760
	public static uint $ConstGCArrayBound$0x97bbb308$55$;

	// Token: 0x040004A6 RID: 1190 RVA: 0x00004408 File Offset: 0x00003808
	public static uint $ConstGCArrayBound$0x97bbb308$13$;

	// Token: 0x040004A7 RID: 1191 RVA: 0x00004428 File Offset: 0x00003828
	public static uint $ConstGCArrayBound$0x97bbb308$5$;

	// Token: 0x040004A8 RID: 1192 RVA: 0x00004270 File Offset: 0x00003670
	public static uint $ConstGCArrayBound$0x97bbb308$115$;

	// Token: 0x040004A9 RID: 1193 RVA: 0x00004348 File Offset: 0x00003748
	public static uint $ConstGCArrayBound$0x97bbb308$61$;

	// Token: 0x040004AA RID: 1194 RVA: 0x0000441C File Offset: 0x0000381C
	public static uint $ConstGCArrayBound$0x97bbb308$8$;

	// Token: 0x040004AB RID: 1195 RVA: 0x000042BC File Offset: 0x000036BC
	public static uint $ConstGCArrayBound$0x97bbb308$96$;

	// Token: 0x040004AC RID: 1196 RVA: 0x00004248 File Offset: 0x00003648
	public static uint $ConstGCArrayBound$0x97bbb308$125$;

	// Token: 0x040004AD RID: 1197 RVA: 0x00004364 File Offset: 0x00003764
	public static uint $ConstGCArrayBound$0x97bbb308$54$;

	// Token: 0x040004AE RID: 1198 RVA: 0x00004260 File Offset: 0x00003660
	public static uint $ConstGCArrayBound$0x97bbb308$119$;

	// Token: 0x040004AF RID: 1199 RVA: 0x000043D4 File Offset: 0x000037D4
	public static uint $ConstGCArrayBound$0x97bbb308$26$;

	// Token: 0x040004B0 RID: 1200 RVA: 0x000043C8 File Offset: 0x000037C8
	public static uint $ConstGCArrayBound$0x97bbb308$29$;

	// Token: 0x040004B1 RID: 1201 RVA: 0x0000439C File Offset: 0x0000379C
	public static uint $ConstGCArrayBound$0x97bbb308$40$;

	// Token: 0x040004B2 RID: 1202 RVA: 0x00004274 File Offset: 0x00003674
	public static uint $ConstGCArrayBound$0x97bbb308$114$;

	// Token: 0x040004B3 RID: 1203 RVA: 0x000043BC File Offset: 0x000037BC
	public static uint $ConstGCArrayBound$0x97bbb308$32$;

	// Token: 0x040004B4 RID: 1204 RVA: 0x0000427C File Offset: 0x0000367C
	public static uint $ConstGCArrayBound$0x97bbb308$112$;

	// Token: 0x040004B5 RID: 1205 RVA: 0x00004410 File Offset: 0x00003810
	public static uint $ConstGCArrayBound$0x97bbb308$11$;

	// Token: 0x040004B6 RID: 1206 RVA: 0x000042AC File Offset: 0x000036AC
	public static uint $ConstGCArrayBound$0x97bbb308$100$;

	// Token: 0x040004B7 RID: 1207 RVA: 0x0000433C File Offset: 0x0000373C
	public static uint $ConstGCArrayBound$0x97bbb308$64$;

	// Token: 0x040004B8 RID: 1208 RVA: 0x000043AC File Offset: 0x000037AC
	public static uint $ConstGCArrayBound$0x97bbb308$36$;

	// Token: 0x040004B9 RID: 1209 RVA: 0x0000442C File Offset: 0x0000382C
	public static uint $ConstGCArrayBound$0x97bbb308$4$;

	// Token: 0x040004BA RID: 1210 RVA: 0x000042F4 File Offset: 0x000036F4
	public static uint $ConstGCArrayBound$0x97bbb308$82$;

	// Token: 0x040004BB RID: 1211 RVA: 0x00004398 File Offset: 0x00003798
	public static uint $ConstGCArrayBound$0x97bbb308$41$;

	// Token: 0x040004BC RID: 1212 RVA: 0x00004244 File Offset: 0x00003644
	public static uint $ConstGCArrayBound$0x97bbb308$126$;

	// Token: 0x040004BD RID: 1213 RVA: 0x000043E0 File Offset: 0x000037E0
	public static uint $ConstGCArrayBound$0x97bbb308$23$;

	// Token: 0x040004BE RID: 1214 RVA: 0x00004368 File Offset: 0x00003768
	public static uint $ConstGCArrayBound$0x97bbb308$53$;

	// Token: 0x040004BF RID: 1215 RVA: 0x000043E8 File Offset: 0x000037E8
	public static uint $ConstGCArrayBound$0x97bbb308$21$;

	// Token: 0x040004C0 RID: 1216 RVA: 0x0000426C File Offset: 0x0000366C
	public static uint $ConstGCArrayBound$0x97bbb308$116$;

	// Token: 0x040004C1 RID: 1217 RVA: 0x00004354 File Offset: 0x00003754
	public static uint $ConstGCArrayBound$0x97bbb308$58$;

	// Token: 0x040004C2 RID: 1218 RVA: 0x0000440C File Offset: 0x0000380C
	public static uint $ConstGCArrayBound$0x97bbb308$12$;

	// Token: 0x040004C3 RID: 1219 RVA: 0x00004404 File Offset: 0x00003804
	public static uint $ConstGCArrayBound$0x97bbb308$14$;

	// Token: 0x040004C4 RID: 1220 RVA: 0x000042B4 File Offset: 0x000036B4
	public static uint $ConstGCArrayBound$0x97bbb308$98$;

	// Token: 0x040004C5 RID: 1221 RVA: 0x0000424C File Offset: 0x0000364C
	public static uint $ConstGCArrayBound$0x97bbb308$124$;

	// Token: 0x040004C6 RID: 1222 RVA: 0x000042F8 File Offset: 0x000036F8
	public static uint $ConstGCArrayBound$0x97bbb308$81$;

	// Token: 0x040004C7 RID: 1223 RVA: 0x00004290 File Offset: 0x00003690
	public static uint $ConstGCArrayBound$0x97bbb308$107$;

	// Token: 0x040004C8 RID: 1224 RVA: 0x000042EC File Offset: 0x000036EC
	public static uint $ConstGCArrayBound$0x97bbb308$84$;

	// Token: 0x040004C9 RID: 1225 RVA: 0x0000437C File Offset: 0x0000377C
	public static uint $ConstGCArrayBound$0x97bbb308$48$;

	// Token: 0x040004CA RID: 1226 RVA: 0x00004378 File Offset: 0x00003778
	public static uint $ConstGCArrayBound$0x97bbb308$49$;

	// Token: 0x040004CB RID: 1227 RVA: 0x00004390 File Offset: 0x00003790
	public static uint $ConstGCArrayBound$0x97bbb308$43$;

	// Token: 0x040004CC RID: 1228 RVA: 0x000042C8 File Offset: 0x000036C8
	public static uint $ConstGCArrayBound$0x97bbb308$93$;

	// Token: 0x040004CD RID: 1229 RVA: 0x00004324 File Offset: 0x00003724
	public static uint $ConstGCArrayBound$0x97bbb308$70$;

	// Token: 0x040004CE RID: 1230 RVA: 0x000043D8 File Offset: 0x000037D8
	public static uint $ConstGCArrayBound$0x97bbb308$25$;

	// Token: 0x040004CF RID: 1231 RVA: 0x00004430 File Offset: 0x00003830
	public static uint $ConstGCArrayBound$0x97bbb308$3$;

	// Token: 0x040004D0 RID: 1232 RVA: 0x000048E0 File Offset: 0x00003CE0
	public static uint $ConstGCArrayBound$0xe293d6a1$33$;

	// Token: 0x040004D1 RID: 1233 RVA: 0x0000494C File Offset: 0x00003D4C
	public static uint $ConstGCArrayBound$0xe293d6a1$6$;

	// Token: 0x040004D2 RID: 1234 RVA: 0x000048E4 File Offset: 0x00003CE4
	public static uint $ConstGCArrayBound$0xe293d6a1$32$;

	// Token: 0x040004D3 RID: 1235 RVA: 0x00004814 File Offset: 0x00003C14
	public static uint $ConstGCArrayBound$0xe293d6a1$84$;

	// Token: 0x040004D4 RID: 1236 RVA: 0x000048DC File Offset: 0x00003CDC
	public static uint $ConstGCArrayBound$0xe293d6a1$34$;

	// Token: 0x040004D5 RID: 1237 RVA: 0x00004868 File Offset: 0x00003C68
	public static uint $ConstGCArrayBound$0xe293d6a1$63$;

	// Token: 0x040004D6 RID: 1238 RVA: 0x00004840 File Offset: 0x00003C40
	public static uint $ConstGCArrayBound$0xe293d6a1$73$;

	// Token: 0x040004D7 RID: 1239 RVA: 0x000047D4 File Offset: 0x00003BD4
	public static uint $ConstGCArrayBound$0xe293d6a1$100$;

	// Token: 0x040004D8 RID: 1240 RVA: 0x00004910 File Offset: 0x00003D10
	public static uint $ConstGCArrayBound$0xe293d6a1$21$;

	// Token: 0x040004D9 RID: 1241 RVA: 0x00004928 File Offset: 0x00003D28
	public static uint $ConstGCArrayBound$0xe293d6a1$15$;

	// Token: 0x040004DA RID: 1242 RVA: 0x000048F0 File Offset: 0x00003CF0
	public static uint $ConstGCArrayBound$0xe293d6a1$29$;

	// Token: 0x040004DB RID: 1243 RVA: 0x00004944 File Offset: 0x00003D44
	public static uint $ConstGCArrayBound$0xe293d6a1$8$;

	// Token: 0x040004DC RID: 1244 RVA: 0x0000483C File Offset: 0x00003C3C
	public static uint $ConstGCArrayBound$0xe293d6a1$74$;

	// Token: 0x040004DD RID: 1245 RVA: 0x000048F4 File Offset: 0x00003CF4
	public static uint $ConstGCArrayBound$0xe293d6a1$28$;

	// Token: 0x040004DE RID: 1246 RVA: 0x00004828 File Offset: 0x00003C28
	public static uint $ConstGCArrayBound$0xe293d6a1$79$;

	// Token: 0x040004DF RID: 1247 RVA: 0x000047B4 File Offset: 0x00003BB4
	public static uint $ConstGCArrayBound$0xe293d6a1$108$;

	// Token: 0x040004E0 RID: 1248 RVA: 0x000047D0 File Offset: 0x00003BD0
	public static uint $ConstGCArrayBound$0xe293d6a1$101$;

	// Token: 0x040004E1 RID: 1249 RVA: 0x00004778 File Offset: 0x00003B78
	public static uint $ConstGCArrayBound$0xe293d6a1$123$;

	// Token: 0x040004E2 RID: 1250 RVA: 0x00004950 File Offset: 0x00003D50
	public static uint $ConstGCArrayBound$0xe293d6a1$5$;

	// Token: 0x040004E3 RID: 1251 RVA: 0x0000485C File Offset: 0x00003C5C
	public static uint $ConstGCArrayBound$0xe293d6a1$66$;

	// Token: 0x040004E4 RID: 1252 RVA: 0x000047B8 File Offset: 0x00003BB8
	public static uint $ConstGCArrayBound$0xe293d6a1$107$;

	// Token: 0x040004E5 RID: 1253 RVA: 0x0000495C File Offset: 0x00003D5C
	public static uint $ConstGCArrayBound$0xe293d6a1$2$;

	// Token: 0x040004E6 RID: 1254 RVA: 0x00004880 File Offset: 0x00003C80
	public static uint $ConstGCArrayBound$0xe293d6a1$57$;

	// Token: 0x040004E7 RID: 1255 RVA: 0x00004948 File Offset: 0x00003D48
	public static uint $ConstGCArrayBound$0xe293d6a1$7$;

	// Token: 0x040004E8 RID: 1256 RVA: 0x000047E0 File Offset: 0x00003BE0
	public static uint $ConstGCArrayBound$0xe293d6a1$97$;

	// Token: 0x040004E9 RID: 1257 RVA: 0x000048B8 File Offset: 0x00003CB8
	public static uint $ConstGCArrayBound$0xe293d6a1$43$;

	// Token: 0x040004EA RID: 1258 RVA: 0x00004934 File Offset: 0x00003D34
	public static uint $ConstGCArrayBound$0xe293d6a1$12$;

	// Token: 0x040004EB RID: 1259 RVA: 0x00004808 File Offset: 0x00003C08
	public static uint $ConstGCArrayBound$0xe293d6a1$87$;

	// Token: 0x040004EC RID: 1260 RVA: 0x0000489C File Offset: 0x00003C9C
	public static uint $ConstGCArrayBound$0xe293d6a1$50$;

	// Token: 0x040004ED RID: 1261 RVA: 0x000048EC File Offset: 0x00003CEC
	public static uint $ConstGCArrayBound$0xe293d6a1$30$;

	// Token: 0x040004EE RID: 1262 RVA: 0x000047A4 File Offset: 0x00003BA4
	public static uint $ConstGCArrayBound$0xe293d6a1$112$;

	// Token: 0x040004EF RID: 1263 RVA: 0x000048B0 File Offset: 0x00003CB0
	public static uint $ConstGCArrayBound$0xe293d6a1$45$;

	// Token: 0x040004F0 RID: 1264 RVA: 0x00004960 File Offset: 0x00003D60
	public static uint $ConstGCArrayBound$0xe293d6a1$1$;

	// Token: 0x040004F1 RID: 1265 RVA: 0x00004914 File Offset: 0x00003D14
	public static uint $ConstGCArrayBound$0xe293d6a1$20$;

	// Token: 0x040004F2 RID: 1266 RVA: 0x000048C8 File Offset: 0x00003CC8
	public static uint $ConstGCArrayBound$0xe293d6a1$39$;

	// Token: 0x040004F3 RID: 1267 RVA: 0x00004894 File Offset: 0x00003C94
	public static uint $ConstGCArrayBound$0xe293d6a1$52$;

	// Token: 0x040004F4 RID: 1268 RVA: 0x000047C0 File Offset: 0x00003BC0
	public static uint $ConstGCArrayBound$0xe293d6a1$105$;

	// Token: 0x040004F5 RID: 1269 RVA: 0x00004918 File Offset: 0x00003D18
	public static uint $ConstGCArrayBound$0xe293d6a1$19$;

	// Token: 0x040004F6 RID: 1270 RVA: 0x00004860 File Offset: 0x00003C60
	public static uint $ConstGCArrayBound$0xe293d6a1$65$;

	// Token: 0x040004F7 RID: 1271 RVA: 0x00004958 File Offset: 0x00003D58
	public static uint $ConstGCArrayBound$0xe293d6a1$3$;

	// Token: 0x040004F8 RID: 1272 RVA: 0x0000492C File Offset: 0x00003D2C
	public static uint $ConstGCArrayBound$0xe293d6a1$14$;

	// Token: 0x040004F9 RID: 1273 RVA: 0x0000476C File Offset: 0x00003B6C
	public static uint $ConstGCArrayBound$0xe293d6a1$126$;

	// Token: 0x040004FA RID: 1274 RVA: 0x00004890 File Offset: 0x00003C90
	public static uint $ConstGCArrayBound$0xe293d6a1$53$;

	// Token: 0x040004FB RID: 1275 RVA: 0x00004794 File Offset: 0x00003B94
	public static uint $ConstGCArrayBound$0xe293d6a1$116$;

	// Token: 0x040004FC RID: 1276 RVA: 0x000048FC File Offset: 0x00003CFC
	public static uint $ConstGCArrayBound$0xe293d6a1$26$;

	// Token: 0x040004FD RID: 1277 RVA: 0x00004920 File Offset: 0x00003D20
	public static uint $ConstGCArrayBound$0xe293d6a1$17$;

	// Token: 0x040004FE RID: 1278 RVA: 0x000047BC File Offset: 0x00003BBC
	public static uint $ConstGCArrayBound$0xe293d6a1$106$;

	// Token: 0x040004FF RID: 1279 RVA: 0x00004834 File Offset: 0x00003C34
	public static uint $ConstGCArrayBound$0xe293d6a1$76$;

	// Token: 0x04000500 RID: 1280 RVA: 0x000047A0 File Offset: 0x00003BA0
	public static uint $ConstGCArrayBound$0xe293d6a1$113$;

	// Token: 0x04000501 RID: 1281 RVA: 0x00004884 File Offset: 0x00003C84
	public static uint $ConstGCArrayBound$0xe293d6a1$56$;

	// Token: 0x04000502 RID: 1282 RVA: 0x0000490C File Offset: 0x00003D0C
	public static uint $ConstGCArrayBound$0xe293d6a1$22$;

	// Token: 0x04000503 RID: 1283 RVA: 0x000047C4 File Offset: 0x00003BC4
	public static uint $ConstGCArrayBound$0xe293d6a1$104$;

	// Token: 0x04000504 RID: 1284 RVA: 0x000047F0 File Offset: 0x00003BF0
	public static uint $ConstGCArrayBound$0xe293d6a1$93$;

	// Token: 0x04000505 RID: 1285 RVA: 0x00004878 File Offset: 0x00003C78
	public static uint $ConstGCArrayBound$0xe293d6a1$59$;

	// Token: 0x04000506 RID: 1286 RVA: 0x0000484C File Offset: 0x00003C4C
	public static uint $ConstGCArrayBound$0xe293d6a1$70$;

	// Token: 0x04000507 RID: 1287 RVA: 0x000048A8 File Offset: 0x00003CA8
	public static uint $ConstGCArrayBound$0xe293d6a1$47$;

	// Token: 0x04000508 RID: 1288 RVA: 0x000047EC File Offset: 0x00003BEC
	public static uint $ConstGCArrayBound$0xe293d6a1$94$;

	// Token: 0x04000509 RID: 1289 RVA: 0x000047B0 File Offset: 0x00003BB0
	public static uint $ConstGCArrayBound$0xe293d6a1$109$;

	// Token: 0x0400050A RID: 1290 RVA: 0x00004788 File Offset: 0x00003B88
	public static uint $ConstGCArrayBound$0xe293d6a1$119$;

	// Token: 0x0400050B RID: 1291 RVA: 0x0000488C File Offset: 0x00003C8C
	public static uint $ConstGCArrayBound$0xe293d6a1$54$;

	// Token: 0x0400050C RID: 1292 RVA: 0x00004900 File Offset: 0x00003D00
	public static uint $ConstGCArrayBound$0xe293d6a1$25$;

	// Token: 0x0400050D RID: 1293 RVA: 0x0000478C File Offset: 0x00003B8C
	public static uint $ConstGCArrayBound$0xe293d6a1$118$;

	// Token: 0x0400050E RID: 1294 RVA: 0x0000477C File Offset: 0x00003B7C
	public static uint $ConstGCArrayBound$0xe293d6a1$122$;

	// Token: 0x0400050F RID: 1295 RVA: 0x0000481C File Offset: 0x00003C1C
	public static uint $ConstGCArrayBound$0xe293d6a1$82$;

	// Token: 0x04000510 RID: 1296 RVA: 0x000048AC File Offset: 0x00003CAC
	public static uint $ConstGCArrayBound$0xe293d6a1$46$;

	// Token: 0x04000511 RID: 1297 RVA: 0x00004938 File Offset: 0x00003D38
	public static uint $ConstGCArrayBound$0xe293d6a1$11$;

	// Token: 0x04000512 RID: 1298 RVA: 0x000048C0 File Offset: 0x00003CC0
	public static uint $ConstGCArrayBound$0xe293d6a1$41$;

	// Token: 0x04000513 RID: 1299 RVA: 0x00004790 File Offset: 0x00003B90
	public static uint $ConstGCArrayBound$0xe293d6a1$117$;

	// Token: 0x04000514 RID: 1300 RVA: 0x00004770 File Offset: 0x00003B70
	public static uint $ConstGCArrayBound$0xe293d6a1$125$;

	// Token: 0x04000515 RID: 1301 RVA: 0x00004810 File Offset: 0x00003C10
	public static uint $ConstGCArrayBound$0xe293d6a1$85$;

	// Token: 0x04000516 RID: 1302 RVA: 0x00004838 File Offset: 0x00003C38
	public static uint $ConstGCArrayBound$0xe293d6a1$75$;

	// Token: 0x04000517 RID: 1303 RVA: 0x00004780 File Offset: 0x00003B80
	public static uint $ConstGCArrayBound$0xe293d6a1$121$;

	// Token: 0x04000518 RID: 1304 RVA: 0x00004940 File Offset: 0x00003D40
	public static uint $ConstGCArrayBound$0xe293d6a1$9$;

	// Token: 0x04000519 RID: 1305 RVA: 0x00004804 File Offset: 0x00003C04
	public static uint $ConstGCArrayBound$0xe293d6a1$88$;

	// Token: 0x0400051A RID: 1306 RVA: 0x000047F8 File Offset: 0x00003BF8
	public static uint $ConstGCArrayBound$0xe293d6a1$91$;

	// Token: 0x0400051B RID: 1307 RVA: 0x000047FC File Offset: 0x00003BFC
	public static uint $ConstGCArrayBound$0xe293d6a1$90$;

	// Token: 0x0400051C RID: 1308 RVA: 0x0000480C File Offset: 0x00003C0C
	public static uint $ConstGCArrayBound$0xe293d6a1$86$;

	// Token: 0x0400051D RID: 1309 RVA: 0x00004774 File Offset: 0x00003B74
	public static uint $ConstGCArrayBound$0xe293d6a1$124$;

	// Token: 0x0400051E RID: 1310 RVA: 0x0000482C File Offset: 0x00003C2C
	public static uint $ConstGCArrayBound$0xe293d6a1$78$;

	// Token: 0x0400051F RID: 1311 RVA: 0x00004954 File Offset: 0x00003D54
	public static uint $ConstGCArrayBound$0xe293d6a1$4$;

	// Token: 0x04000520 RID: 1312 RVA: 0x000047DC File Offset: 0x00003BDC
	public static uint $ConstGCArrayBound$0xe293d6a1$98$;

	// Token: 0x04000521 RID: 1313 RVA: 0x00004930 File Offset: 0x00003D30
	public static uint $ConstGCArrayBound$0xe293d6a1$13$;

	// Token: 0x04000522 RID: 1314 RVA: 0x000047AC File Offset: 0x00003BAC
	public static uint $ConstGCArrayBound$0xe293d6a1$110$;

	// Token: 0x04000523 RID: 1315 RVA: 0x00004844 File Offset: 0x00003C44
	public static uint $ConstGCArrayBound$0xe293d6a1$72$;

	// Token: 0x04000524 RID: 1316 RVA: 0x000048D8 File Offset: 0x00003CD8
	public static uint $ConstGCArrayBound$0xe293d6a1$35$;

	// Token: 0x04000525 RID: 1317 RVA: 0x0000491C File Offset: 0x00003D1C
	public static uint $ConstGCArrayBound$0xe293d6a1$18$;

	// Token: 0x04000526 RID: 1318 RVA: 0x000047E8 File Offset: 0x00003BE8
	public static uint $ConstGCArrayBound$0xe293d6a1$95$;

	// Token: 0x04000527 RID: 1319 RVA: 0x000048CC File Offset: 0x00003CCC
	public static uint $ConstGCArrayBound$0xe293d6a1$38$;

	// Token: 0x04000528 RID: 1320 RVA: 0x000047D8 File Offset: 0x00003BD8
	public static uint $ConstGCArrayBound$0xe293d6a1$99$;

	// Token: 0x04000529 RID: 1321 RVA: 0x000048BC File Offset: 0x00003CBC
	public static uint $ConstGCArrayBound$0xe293d6a1$42$;

	// Token: 0x0400052A RID: 1322 RVA: 0x000048E8 File Offset: 0x00003CE8
	public static uint $ConstGCArrayBound$0xe293d6a1$31$;

	// Token: 0x0400052B RID: 1323 RVA: 0x000048B4 File Offset: 0x00003CB4
	public static uint $ConstGCArrayBound$0xe293d6a1$44$;

	// Token: 0x0400052C RID: 1324 RVA: 0x00004764 File Offset: 0x00003B64
	public static uint $ConstGCArrayBound$0xe293d6a1$128$;

	// Token: 0x0400052D RID: 1325 RVA: 0x000047F4 File Offset: 0x00003BF4
	public static uint $ConstGCArrayBound$0xe293d6a1$92$;

	// Token: 0x0400052E RID: 1326 RVA: 0x00004800 File Offset: 0x00003C00
	public static uint $ConstGCArrayBound$0xe293d6a1$89$;

	// Token: 0x0400052F RID: 1327 RVA: 0x00004768 File Offset: 0x00003B68
	public static uint $ConstGCArrayBound$0xe293d6a1$127$;

	// Token: 0x04000530 RID: 1328 RVA: 0x000048F8 File Offset: 0x00003CF8
	public static uint $ConstGCArrayBound$0xe293d6a1$27$;

	// Token: 0x04000531 RID: 1329 RVA: 0x000048C4 File Offset: 0x00003CC4
	public static uint $ConstGCArrayBound$0xe293d6a1$40$;

	// Token: 0x04000532 RID: 1330 RVA: 0x00004898 File Offset: 0x00003C98
	public static uint $ConstGCArrayBound$0xe293d6a1$51$;

	// Token: 0x04000533 RID: 1331 RVA: 0x00004818 File Offset: 0x00003C18
	public static uint $ConstGCArrayBound$0xe293d6a1$83$;

	// Token: 0x04000534 RID: 1332 RVA: 0x00004858 File Offset: 0x00003C58
	public static uint $ConstGCArrayBound$0xe293d6a1$67$;

	// Token: 0x04000535 RID: 1333 RVA: 0x000047A8 File Offset: 0x00003BA8
	public static uint $ConstGCArrayBound$0xe293d6a1$111$;

	// Token: 0x04000536 RID: 1334 RVA: 0x000047CC File Offset: 0x00003BCC
	public static uint $ConstGCArrayBound$0xe293d6a1$102$;

	// Token: 0x04000537 RID: 1335 RVA: 0x000048A0 File Offset: 0x00003CA0
	public static uint $ConstGCArrayBound$0xe293d6a1$49$;

	// Token: 0x04000538 RID: 1336 RVA: 0x00004908 File Offset: 0x00003D08
	public static uint $ConstGCArrayBound$0xe293d6a1$23$;

	// Token: 0x04000539 RID: 1337 RVA: 0x000048D0 File Offset: 0x00003CD0
	public static uint $ConstGCArrayBound$0xe293d6a1$37$;

	// Token: 0x0400053A RID: 1338 RVA: 0x00004824 File Offset: 0x00003C24
	public static uint $ConstGCArrayBound$0xe293d6a1$80$;

	// Token: 0x0400053B RID: 1339 RVA: 0x00004798 File Offset: 0x00003B98
	public static uint $ConstGCArrayBound$0xe293d6a1$115$;

	// Token: 0x0400053C RID: 1340 RVA: 0x00004850 File Offset: 0x00003C50
	public static uint $ConstGCArrayBound$0xe293d6a1$69$;

	// Token: 0x0400053D RID: 1341 RVA: 0x00004854 File Offset: 0x00003C54
	public static uint $ConstGCArrayBound$0xe293d6a1$68$;

	// Token: 0x0400053E RID: 1342 RVA: 0x000047E4 File Offset: 0x00003BE4
	public static uint $ConstGCArrayBound$0xe293d6a1$96$;

	// Token: 0x0400053F RID: 1343 RVA: 0x000047C8 File Offset: 0x00003BC8
	public static uint $ConstGCArrayBound$0xe293d6a1$103$;

	// Token: 0x04000540 RID: 1344 RVA: 0x00004904 File Offset: 0x00003D04
	public static uint $ConstGCArrayBound$0xe293d6a1$24$;

	// Token: 0x04000541 RID: 1345 RVA: 0x00004830 File Offset: 0x00003C30
	public static uint $ConstGCArrayBound$0xe293d6a1$77$;

	// Token: 0x04000542 RID: 1346 RVA: 0x000048D4 File Offset: 0x00003CD4
	public static uint $ConstGCArrayBound$0xe293d6a1$36$;

	// Token: 0x04000543 RID: 1347 RVA: 0x00004848 File Offset: 0x00003C48
	public static uint $ConstGCArrayBound$0xe293d6a1$71$;

	// Token: 0x04000544 RID: 1348 RVA: 0x0000479C File Offset: 0x00003B9C
	public static uint $ConstGCArrayBound$0xe293d6a1$114$;

	// Token: 0x04000545 RID: 1349 RVA: 0x00004864 File Offset: 0x00003C64
	public static uint $ConstGCArrayBound$0xe293d6a1$64$;

	// Token: 0x04000546 RID: 1350 RVA: 0x00004924 File Offset: 0x00003D24
	public static uint $ConstGCArrayBound$0xe293d6a1$16$;

	// Token: 0x04000547 RID: 1351 RVA: 0x0000486C File Offset: 0x00003C6C
	public static uint $ConstGCArrayBound$0xe293d6a1$62$;

	// Token: 0x04000548 RID: 1352 RVA: 0x0000487C File Offset: 0x00003C7C
	public static uint $ConstGCArrayBound$0xe293d6a1$58$;

	// Token: 0x04000549 RID: 1353 RVA: 0x00004820 File Offset: 0x00003C20
	public static uint $ConstGCArrayBound$0xe293d6a1$81$;

	// Token: 0x0400054A RID: 1354 RVA: 0x0000493C File Offset: 0x00003D3C
	public static uint $ConstGCArrayBound$0xe293d6a1$10$;

	// Token: 0x0400054B RID: 1355 RVA: 0x00004874 File Offset: 0x00003C74
	public static uint $ConstGCArrayBound$0xe293d6a1$60$;

	// Token: 0x0400054C RID: 1356 RVA: 0x00004784 File Offset: 0x00003B84
	public static uint $ConstGCArrayBound$0xe293d6a1$120$;

	// Token: 0x0400054D RID: 1357 RVA: 0x00004870 File Offset: 0x00003C70
	public static uint $ConstGCArrayBound$0xe293d6a1$61$;

	// Token: 0x0400054E RID: 1358 RVA: 0x00004888 File Offset: 0x00003C88
	public static uint $ConstGCArrayBound$0xe293d6a1$55$;

	// Token: 0x0400054F RID: 1359 RVA: 0x000048A4 File Offset: 0x00003CA4
	public static uint $ConstGCArrayBound$0xe293d6a1$48$;

	// Token: 0x04000550 RID: 1360 RVA: 0x00004D7C File Offset: 0x0000417C
	public static uint $ConstGCArrayBound$0x5978dfc7$69$;

	// Token: 0x04000551 RID: 1361 RVA: 0x00004CB8 File Offset: 0x000040B8
	public static uint $ConstGCArrayBound$0x5978dfc7$118$;

	// Token: 0x04000552 RID: 1362 RVA: 0x00004E48 File Offset: 0x00004248
	public static uint $ConstGCArrayBound$0x5978dfc7$18$;

	// Token: 0x04000553 RID: 1363 RVA: 0x00004DF0 File Offset: 0x000041F0
	public static uint $ConstGCArrayBound$0x5978dfc7$40$;

	// Token: 0x04000554 RID: 1364 RVA: 0x00004E08 File Offset: 0x00004208
	public static uint $ConstGCArrayBound$0x5978dfc7$34$;

	// Token: 0x04000555 RID: 1365 RVA: 0x00004DDC File Offset: 0x000041DC
	public static uint $ConstGCArrayBound$0x5978dfc7$45$;

	// Token: 0x04000556 RID: 1366 RVA: 0x00004C9C File Offset: 0x0000409C
	public static uint $ConstGCArrayBound$0x5978dfc7$125$;

	// Token: 0x04000557 RID: 1367 RVA: 0x00004E4C File Offset: 0x0000424C
	public static uint $ConstGCArrayBound$0x5978dfc7$17$;

	// Token: 0x04000558 RID: 1368 RVA: 0x00004D50 File Offset: 0x00004150
	public static uint $ConstGCArrayBound$0x5978dfc7$80$;

	// Token: 0x04000559 RID: 1369 RVA: 0x00004D74 File Offset: 0x00004174
	public static uint $ConstGCArrayBound$0x5978dfc7$71$;

	// Token: 0x0400055A RID: 1370 RVA: 0x00004E24 File Offset: 0x00004224
	public static uint $ConstGCArrayBound$0x5978dfc7$27$;

	// Token: 0x0400055B RID: 1371 RVA: 0x00004E18 File Offset: 0x00004218
	public static uint $ConstGCArrayBound$0x5978dfc7$30$;

	// Token: 0x0400055C RID: 1372 RVA: 0x00004E60 File Offset: 0x00004260
	public static uint $ConstGCArrayBound$0x5978dfc7$12$;

	// Token: 0x0400055D RID: 1373 RVA: 0x00004CBC File Offset: 0x000040BC
	public static uint $ConstGCArrayBound$0x5978dfc7$117$;

	// Token: 0x0400055E RID: 1374 RVA: 0x00004DD4 File Offset: 0x000041D4
	public static uint $ConstGCArrayBound$0x5978dfc7$47$;

	// Token: 0x0400055F RID: 1375 RVA: 0x00004D20 File Offset: 0x00004120
	public static uint $ConstGCArrayBound$0x5978dfc7$92$;

	// Token: 0x04000560 RID: 1376 RVA: 0x00004D60 File Offset: 0x00004160
	public static uint $ConstGCArrayBound$0x5978dfc7$76$;

	// Token: 0x04000561 RID: 1377 RVA: 0x00004CF8 File Offset: 0x000040F8
	public static uint $ConstGCArrayBound$0x5978dfc7$102$;

	// Token: 0x04000562 RID: 1378 RVA: 0x00004D4C File Offset: 0x0000414C
	public static uint $ConstGCArrayBound$0x5978dfc7$81$;

	// Token: 0x04000563 RID: 1379 RVA: 0x00004D0C File Offset: 0x0000410C
	public static uint $ConstGCArrayBound$0x5978dfc7$97$;

	// Token: 0x04000564 RID: 1380 RVA: 0x00004DE8 File Offset: 0x000041E8
	public static uint $ConstGCArrayBound$0x5978dfc7$42$;

	// Token: 0x04000565 RID: 1381 RVA: 0x00004D54 File Offset: 0x00004154
	public static uint $ConstGCArrayBound$0x5978dfc7$79$;

	// Token: 0x04000566 RID: 1382 RVA: 0x00004CC4 File Offset: 0x000040C4
	public static uint $ConstGCArrayBound$0x5978dfc7$115$;

	// Token: 0x04000567 RID: 1383 RVA: 0x00004DFC File Offset: 0x000041FC
	public static uint $ConstGCArrayBound$0x5978dfc7$37$;

	// Token: 0x04000568 RID: 1384 RVA: 0x00004DC4 File Offset: 0x000041C4
	public static uint $ConstGCArrayBound$0x5978dfc7$51$;

	// Token: 0x04000569 RID: 1385 RVA: 0x00004D90 File Offset: 0x00004190
	public static uint $ConstGCArrayBound$0x5978dfc7$64$;

	// Token: 0x0400056A RID: 1386 RVA: 0x00004E58 File Offset: 0x00004258
	public static uint $ConstGCArrayBound$0x5978dfc7$14$;

	// Token: 0x0400056B RID: 1387 RVA: 0x00004DCC File Offset: 0x000041CC
	public static uint $ConstGCArrayBound$0x5978dfc7$49$;

	// Token: 0x0400056C RID: 1388 RVA: 0x00004DE0 File Offset: 0x000041E0
	public static uint $ConstGCArrayBound$0x5978dfc7$44$;

	// Token: 0x0400056D RID: 1389 RVA: 0x00004E70 File Offset: 0x00004270
	public static uint $ConstGCArrayBound$0x5978dfc7$8$;

	// Token: 0x0400056E RID: 1390 RVA: 0x00004DA8 File Offset: 0x000041A8
	public static uint $ConstGCArrayBound$0x5978dfc7$58$;

	// Token: 0x0400056F RID: 1391 RVA: 0x00004E1C File Offset: 0x0000421C
	public static uint $ConstGCArrayBound$0x5978dfc7$29$;

	// Token: 0x04000570 RID: 1392 RVA: 0x00004DF8 File Offset: 0x000041F8
	public static uint $ConstGCArrayBound$0x5978dfc7$38$;

	// Token: 0x04000571 RID: 1393 RVA: 0x00004CF4 File Offset: 0x000040F4
	public static uint $ConstGCArrayBound$0x5978dfc7$103$;

	// Token: 0x04000572 RID: 1394 RVA: 0x00004E00 File Offset: 0x00004200
	public static uint $ConstGCArrayBound$0x5978dfc7$36$;

	// Token: 0x04000573 RID: 1395 RVA: 0x00004D5C File Offset: 0x0000415C
	public static uint $ConstGCArrayBound$0x5978dfc7$77$;

	// Token: 0x04000574 RID: 1396 RVA: 0x00004D34 File Offset: 0x00004134
	public static uint $ConstGCArrayBound$0x5978dfc7$87$;

	// Token: 0x04000575 RID: 1397 RVA: 0x00004CC0 File Offset: 0x000040C0
	public static uint $ConstGCArrayBound$0x5978dfc7$116$;

	// Token: 0x04000576 RID: 1398 RVA: 0x00004E80 File Offset: 0x00004280
	public static uint $ConstGCArrayBound$0x5978dfc7$4$;

	// Token: 0x04000577 RID: 1399 RVA: 0x00004E04 File Offset: 0x00004204
	public static uint $ConstGCArrayBound$0x5978dfc7$35$;

	// Token: 0x04000578 RID: 1400 RVA: 0x00004E6C File Offset: 0x0000426C
	public static uint $ConstGCArrayBound$0x5978dfc7$9$;

	// Token: 0x04000579 RID: 1401 RVA: 0x00004E84 File Offset: 0x00004284
	public static uint $ConstGCArrayBound$0x5978dfc7$3$;

	// Token: 0x0400057A RID: 1402 RVA: 0x00004DD8 File Offset: 0x000041D8
	public static uint $ConstGCArrayBound$0x5978dfc7$46$;

	// Token: 0x0400057B RID: 1403 RVA: 0x00004D10 File Offset: 0x00004110
	public static uint $ConstGCArrayBound$0x5978dfc7$96$;

	// Token: 0x0400057C RID: 1404 RVA: 0x00004CFC File Offset: 0x000040FC
	public static uint $ConstGCArrayBound$0x5978dfc7$101$;

	// Token: 0x0400057D RID: 1405 RVA: 0x00004E8C File Offset: 0x0000428C
	public static uint $ConstGCArrayBound$0x5978dfc7$1$;

	// Token: 0x0400057E RID: 1406 RVA: 0x00004CE4 File Offset: 0x000040E4
	public static uint $ConstGCArrayBound$0x5978dfc7$107$;

	// Token: 0x0400057F RID: 1407 RVA: 0x00004D24 File Offset: 0x00004124
	public static uint $ConstGCArrayBound$0x5978dfc7$91$;

	// Token: 0x04000580 RID: 1408 RVA: 0x00004D68 File Offset: 0x00004168
	public static uint $ConstGCArrayBound$0x5978dfc7$74$;

	// Token: 0x04000581 RID: 1409 RVA: 0x00004D40 File Offset: 0x00004140
	public static uint $ConstGCArrayBound$0x5978dfc7$84$;

	// Token: 0x04000582 RID: 1410 RVA: 0x00004CB4 File Offset: 0x000040B4
	public static uint $ConstGCArrayBound$0x5978dfc7$119$;

	// Token: 0x04000583 RID: 1411 RVA: 0x00004D8C File Offset: 0x0000418C
	public static uint $ConstGCArrayBound$0x5978dfc7$65$;

	// Token: 0x04000584 RID: 1412 RVA: 0x00004D2C File Offset: 0x0000412C
	public static uint $ConstGCArrayBound$0x5978dfc7$89$;

	// Token: 0x04000585 RID: 1413 RVA: 0x00004D00 File Offset: 0x00004100
	public static uint $ConstGCArrayBound$0x5978dfc7$100$;

	// Token: 0x04000586 RID: 1414 RVA: 0x00004DE4 File Offset: 0x000041E4
	public static uint $ConstGCArrayBound$0x5978dfc7$43$;

	// Token: 0x04000587 RID: 1415 RVA: 0x00004E0C File Offset: 0x0000420C
	public static uint $ConstGCArrayBound$0x5978dfc7$33$;

	// Token: 0x04000588 RID: 1416 RVA: 0x00004D14 File Offset: 0x00004114
	public static uint $ConstGCArrayBound$0x5978dfc7$95$;

	// Token: 0x04000589 RID: 1417 RVA: 0x00004E10 File Offset: 0x00004210
	public static uint $ConstGCArrayBound$0x5978dfc7$32$;

	// Token: 0x0400058A RID: 1418 RVA: 0x00004E38 File Offset: 0x00004238
	public static uint $ConstGCArrayBound$0x5978dfc7$22$;

	// Token: 0x0400058B RID: 1419 RVA: 0x00004C98 File Offset: 0x00004098
	public static uint $ConstGCArrayBound$0x5978dfc7$126$;

	// Token: 0x0400058C RID: 1420 RVA: 0x00004D70 File Offset: 0x00004170
	public static uint $ConstGCArrayBound$0x5978dfc7$72$;

	// Token: 0x0400058D RID: 1421 RVA: 0x00004E64 File Offset: 0x00004264
	public static uint $ConstGCArrayBound$0x5978dfc7$11$;

	// Token: 0x0400058E RID: 1422 RVA: 0x00004DB0 File Offset: 0x000041B0
	public static uint $ConstGCArrayBound$0x5978dfc7$56$;

	// Token: 0x0400058F RID: 1423 RVA: 0x00004D1C File Offset: 0x0000411C
	public static uint $ConstGCArrayBound$0x5978dfc7$93$;

	// Token: 0x04000590 RID: 1424 RVA: 0x00004E88 File Offset: 0x00004288
	public static uint $ConstGCArrayBound$0x5978dfc7$2$;

	// Token: 0x04000591 RID: 1425 RVA: 0x00004D6C File Offset: 0x0000416C
	public static uint $ConstGCArrayBound$0x5978dfc7$73$;

	// Token: 0x04000592 RID: 1426 RVA: 0x00004E50 File Offset: 0x00004250
	public static uint $ConstGCArrayBound$0x5978dfc7$16$;

	// Token: 0x04000593 RID: 1427 RVA: 0x00004D88 File Offset: 0x00004188
	public static uint $ConstGCArrayBound$0x5978dfc7$66$;

	// Token: 0x04000594 RID: 1428 RVA: 0x00004DAC File Offset: 0x000041AC
	public static uint $ConstGCArrayBound$0x5978dfc7$57$;

	// Token: 0x04000595 RID: 1429 RVA: 0x00004D28 File Offset: 0x00004128
	public static uint $ConstGCArrayBound$0x5978dfc7$90$;

	// Token: 0x04000596 RID: 1430 RVA: 0x00004DC8 File Offset: 0x000041C8
	public static uint $ConstGCArrayBound$0x5978dfc7$50$;

	// Token: 0x04000597 RID: 1431 RVA: 0x00004E30 File Offset: 0x00004230
	public static uint $ConstGCArrayBound$0x5978dfc7$24$;

	// Token: 0x04000598 RID: 1432 RVA: 0x00004CB0 File Offset: 0x000040B0
	public static uint $ConstGCArrayBound$0x5978dfc7$120$;

	// Token: 0x04000599 RID: 1433 RVA: 0x00004C94 File Offset: 0x00004094
	public static uint $ConstGCArrayBound$0x5978dfc7$127$;

	// Token: 0x0400059A RID: 1434 RVA: 0x00004E34 File Offset: 0x00004234
	public static uint $ConstGCArrayBound$0x5978dfc7$23$;

	// Token: 0x0400059B RID: 1435 RVA: 0x00004DB4 File Offset: 0x000041B4
	public static uint $ConstGCArrayBound$0x5978dfc7$55$;

	// Token: 0x0400059C RID: 1436 RVA: 0x00004D08 File Offset: 0x00004108
	public static uint $ConstGCArrayBound$0x5978dfc7$98$;

	// Token: 0x0400059D RID: 1437 RVA: 0x00004D44 File Offset: 0x00004144
	public static uint $ConstGCArrayBound$0x5978dfc7$83$;

	// Token: 0x0400059E RID: 1438 RVA: 0x00004DEC File Offset: 0x000041EC
	public static uint $ConstGCArrayBound$0x5978dfc7$41$;

	// Token: 0x0400059F RID: 1439 RVA: 0x00004CEC File Offset: 0x000040EC
	public static uint $ConstGCArrayBound$0x5978dfc7$105$;

	// Token: 0x040005A0 RID: 1440 RVA: 0x00004D94 File Offset: 0x00004194
	public static uint $ConstGCArrayBound$0x5978dfc7$63$;

	// Token: 0x040005A1 RID: 1441 RVA: 0x00004CD4 File Offset: 0x000040D4
	public static uint $ConstGCArrayBound$0x5978dfc7$111$;

	// Token: 0x040005A2 RID: 1442 RVA: 0x00004D58 File Offset: 0x00004158
	public static uint $ConstGCArrayBound$0x5978dfc7$78$;

	// Token: 0x040005A3 RID: 1443 RVA: 0x00004DA4 File Offset: 0x000041A4
	public static uint $ConstGCArrayBound$0x5978dfc7$59$;

	// Token: 0x040005A4 RID: 1444 RVA: 0x00004D48 File Offset: 0x00004148
	public static uint $ConstGCArrayBound$0x5978dfc7$82$;

	// Token: 0x040005A5 RID: 1445 RVA: 0x00004CDC File Offset: 0x000040DC
	public static uint $ConstGCArrayBound$0x5978dfc7$109$;

	// Token: 0x040005A6 RID: 1446 RVA: 0x00004E68 File Offset: 0x00004268
	public static uint $ConstGCArrayBound$0x5978dfc7$10$;

	// Token: 0x040005A7 RID: 1447 RVA: 0x00004CA4 File Offset: 0x000040A4
	public static uint $ConstGCArrayBound$0x5978dfc7$123$;

	// Token: 0x040005A8 RID: 1448 RVA: 0x00004D30 File Offset: 0x00004130
	public static uint $ConstGCArrayBound$0x5978dfc7$88$;

	// Token: 0x040005A9 RID: 1449 RVA: 0x00004D9C File Offset: 0x0000419C
	public static uint $ConstGCArrayBound$0x5978dfc7$61$;

	// Token: 0x040005AA RID: 1450 RVA: 0x00004CCC File Offset: 0x000040CC
	public static uint $ConstGCArrayBound$0x5978dfc7$113$;

	// Token: 0x040005AB RID: 1451 RVA: 0x00004E7C File Offset: 0x0000427C
	public static uint $ConstGCArrayBound$0x5978dfc7$5$;

	// Token: 0x040005AC RID: 1452 RVA: 0x00004D3C File Offset: 0x0000413C
	public static uint $ConstGCArrayBound$0x5978dfc7$85$;

	// Token: 0x040005AD RID: 1453 RVA: 0x00004E20 File Offset: 0x00004220
	public static uint $ConstGCArrayBound$0x5978dfc7$28$;

	// Token: 0x040005AE RID: 1454 RVA: 0x00004DD0 File Offset: 0x000041D0
	public static uint $ConstGCArrayBound$0x5978dfc7$48$;

	// Token: 0x040005AF RID: 1455 RVA: 0x00004D18 File Offset: 0x00004118
	public static uint $ConstGCArrayBound$0x5978dfc7$94$;

	// Token: 0x040005B0 RID: 1456 RVA: 0x00004DF4 File Offset: 0x000041F4
	public static uint $ConstGCArrayBound$0x5978dfc7$39$;

	// Token: 0x040005B1 RID: 1457 RVA: 0x00004E54 File Offset: 0x00004254
	public static uint $ConstGCArrayBound$0x5978dfc7$15$;

	// Token: 0x040005B2 RID: 1458 RVA: 0x00004D78 File Offset: 0x00004178
	public static uint $ConstGCArrayBound$0x5978dfc7$70$;

	// Token: 0x040005B3 RID: 1459 RVA: 0x00004CC8 File Offset: 0x000040C8
	public static uint $ConstGCArrayBound$0x5978dfc7$114$;

	// Token: 0x040005B4 RID: 1460 RVA: 0x00004C90 File Offset: 0x00004090
	public static uint $ConstGCArrayBound$0x5978dfc7$128$;

	// Token: 0x040005B5 RID: 1461 RVA: 0x00004E5C File Offset: 0x0000425C
	public static uint $ConstGCArrayBound$0x5978dfc7$13$;

	// Token: 0x040005B6 RID: 1462 RVA: 0x00004DB8 File Offset: 0x000041B8
	public static uint $ConstGCArrayBound$0x5978dfc7$54$;

	// Token: 0x040005B7 RID: 1463 RVA: 0x00004D04 File Offset: 0x00004104
	public static uint $ConstGCArrayBound$0x5978dfc7$99$;

	// Token: 0x040005B8 RID: 1464 RVA: 0x00004E14 File Offset: 0x00004214
	public static uint $ConstGCArrayBound$0x5978dfc7$31$;

	// Token: 0x040005B9 RID: 1465 RVA: 0x00004E28 File Offset: 0x00004228
	public static uint $ConstGCArrayBound$0x5978dfc7$26$;

	// Token: 0x040005BA RID: 1466 RVA: 0x00004E40 File Offset: 0x00004240
	public static uint $ConstGCArrayBound$0x5978dfc7$20$;

	// Token: 0x040005BB RID: 1467 RVA: 0x00004DA0 File Offset: 0x000041A0
	public static uint $ConstGCArrayBound$0x5978dfc7$60$;

	// Token: 0x040005BC RID: 1468 RVA: 0x00004D84 File Offset: 0x00004184
	public static uint $ConstGCArrayBound$0x5978dfc7$67$;

	// Token: 0x040005BD RID: 1469 RVA: 0x00004DBC File Offset: 0x000041BC
	public static uint $ConstGCArrayBound$0x5978dfc7$53$;

	// Token: 0x040005BE RID: 1470 RVA: 0x00004E2C File Offset: 0x0000422C
	public static uint $ConstGCArrayBound$0x5978dfc7$25$;

	// Token: 0x040005BF RID: 1471 RVA: 0x00004E74 File Offset: 0x00004274
	public static uint $ConstGCArrayBound$0x5978dfc7$7$;

	// Token: 0x040005C0 RID: 1472 RVA: 0x00004CF0 File Offset: 0x000040F0
	public static uint $ConstGCArrayBound$0x5978dfc7$104$;

	// Token: 0x040005C1 RID: 1473 RVA: 0x00004CAC File Offset: 0x000040AC
	public static uint $ConstGCArrayBound$0x5978dfc7$121$;

	// Token: 0x040005C2 RID: 1474 RVA: 0x00004E44 File Offset: 0x00004244
	public static uint $ConstGCArrayBound$0x5978dfc7$19$;

	// Token: 0x040005C3 RID: 1475 RVA: 0x00004E78 File Offset: 0x00004278
	public static uint $ConstGCArrayBound$0x5978dfc7$6$;

	// Token: 0x040005C4 RID: 1476 RVA: 0x00004D80 File Offset: 0x00004180
	public static uint $ConstGCArrayBound$0x5978dfc7$68$;

	// Token: 0x040005C5 RID: 1477 RVA: 0x00004CA8 File Offset: 0x000040A8
	public static uint $ConstGCArrayBound$0x5978dfc7$122$;

	// Token: 0x040005C6 RID: 1478 RVA: 0x00004CA0 File Offset: 0x000040A0
	public static uint $ConstGCArrayBound$0x5978dfc7$124$;

	// Token: 0x040005C7 RID: 1479 RVA: 0x00004CD8 File Offset: 0x000040D8
	public static uint $ConstGCArrayBound$0x5978dfc7$110$;

	// Token: 0x040005C8 RID: 1480 RVA: 0x00004DC0 File Offset: 0x000041C0
	public static uint $ConstGCArrayBound$0x5978dfc7$52$;

	// Token: 0x040005C9 RID: 1481 RVA: 0x00004CE0 File Offset: 0x000040E0
	public static uint $ConstGCArrayBound$0x5978dfc7$108$;

	// Token: 0x040005CA RID: 1482 RVA: 0x00004D64 File Offset: 0x00004164
	public static uint $ConstGCArrayBound$0x5978dfc7$75$;

	// Token: 0x040005CB RID: 1483 RVA: 0x00004D98 File Offset: 0x00004198
	public static uint $ConstGCArrayBound$0x5978dfc7$62$;

	// Token: 0x040005CC RID: 1484 RVA: 0x00004E3C File Offset: 0x0000423C
	public static uint $ConstGCArrayBound$0x5978dfc7$21$;

	// Token: 0x040005CD RID: 1485 RVA: 0x00004CD0 File Offset: 0x000040D0
	public static uint $ConstGCArrayBound$0x5978dfc7$112$;

	// Token: 0x040005CE RID: 1486 RVA: 0x00004CE8 File Offset: 0x000040E8
	public static uint $ConstGCArrayBound$0x5978dfc7$106$;

	// Token: 0x040005CF RID: 1487 RVA: 0x00004D38 File Offset: 0x00004138
	public static uint $ConstGCArrayBound$0x5978dfc7$86$;

	// Token: 0x040005D0 RID: 1488 RVA: 0x000052FC File Offset: 0x000046FC
	public static uint $ConstGCArrayBound$0x0e65953e$47$;

	// Token: 0x040005D1 RID: 1489 RVA: 0x000053B0 File Offset: 0x000047B0
	public static uint $ConstGCArrayBound$0x0e65953e$2$;

	// Token: 0x040005D2 RID: 1490 RVA: 0x00005264 File Offset: 0x00004664
	public static uint $ConstGCArrayBound$0x0e65953e$85$;

	// Token: 0x040005D3 RID: 1491 RVA: 0x000051D0 File Offset: 0x000045D0
	public static uint $ConstGCArrayBound$0x0e65953e$122$;

	// Token: 0x040005D4 RID: 1492 RVA: 0x00005310 File Offset: 0x00004710
	public static uint $ConstGCArrayBound$0x0e65953e$42$;

	// Token: 0x040005D5 RID: 1493 RVA: 0x00005284 File Offset: 0x00004684
	public static uint $ConstGCArrayBound$0x0e65953e$77$;

	// Token: 0x040005D6 RID: 1494 RVA: 0x0000534C File Offset: 0x0000474C
	public static uint $ConstGCArrayBound$0x0e65953e$27$;

	// Token: 0x040005D7 RID: 1495 RVA: 0x00005244 File Offset: 0x00004644
	public static uint $ConstGCArrayBound$0x0e65953e$93$;

	// Token: 0x040005D8 RID: 1496 RVA: 0x00005294 File Offset: 0x00004694
	public static uint $ConstGCArrayBound$0x0e65953e$73$;

	// Token: 0x040005D9 RID: 1497 RVA: 0x0000527C File Offset: 0x0000467C
	public static uint $ConstGCArrayBound$0x0e65953e$79$;

	// Token: 0x040005DA RID: 1498 RVA: 0x00005344 File Offset: 0x00004744
	public static uint $ConstGCArrayBound$0x0e65953e$29$;

	// Token: 0x040005DB RID: 1499 RVA: 0x00005248 File Offset: 0x00004648
	public static uint $ConstGCArrayBound$0x0e65953e$92$;

	// Token: 0x040005DC RID: 1500 RVA: 0x00005218 File Offset: 0x00004618
	public static uint $ConstGCArrayBound$0x0e65953e$104$;

	// Token: 0x040005DD RID: 1501 RVA: 0x000052C8 File Offset: 0x000046C8
	public static uint $ConstGCArrayBound$0x0e65953e$60$;

	// Token: 0x040005DE RID: 1502 RVA: 0x00005260 File Offset: 0x00004660
	public static uint $ConstGCArrayBound$0x0e65953e$86$;

	// Token: 0x040005DF RID: 1503 RVA: 0x000051F8 File Offset: 0x000045F8
	public static uint $ConstGCArrayBound$0x0e65953e$112$;

	// Token: 0x040005E0 RID: 1504 RVA: 0x00005254 File Offset: 0x00004654
	public static uint $ConstGCArrayBound$0x0e65953e$89$;

	// Token: 0x040005E1 RID: 1505 RVA: 0x00005290 File Offset: 0x00004690
	public static uint $ConstGCArrayBound$0x0e65953e$74$;

	// Token: 0x040005E2 RID: 1506 RVA: 0x000053A8 File Offset: 0x000047A8
	public static uint $ConstGCArrayBound$0x0e65953e$4$;

	// Token: 0x040005E3 RID: 1507 RVA: 0x00005388 File Offset: 0x00004788
	public static uint $ConstGCArrayBound$0x0e65953e$12$;

	// Token: 0x040005E4 RID: 1508 RVA: 0x00005384 File Offset: 0x00004784
	public static uint $ConstGCArrayBound$0x0e65953e$13$;

	// Token: 0x040005E5 RID: 1509 RVA: 0x00005258 File Offset: 0x00004658
	public static uint $ConstGCArrayBound$0x0e65953e$88$;

	// Token: 0x040005E6 RID: 1510 RVA: 0x00005200 File Offset: 0x00004600
	public static uint $ConstGCArrayBound$0x0e65953e$110$;

	// Token: 0x040005E7 RID: 1511 RVA: 0x00005314 File Offset: 0x00004714
	public static uint $ConstGCArrayBound$0x0e65953e$41$;

	// Token: 0x040005E8 RID: 1512 RVA: 0x000052AC File Offset: 0x000046AC
	public static uint $ConstGCArrayBound$0x0e65953e$67$;

	// Token: 0x040005E9 RID: 1513 RVA: 0x00005214 File Offset: 0x00004614
	public static uint $ConstGCArrayBound$0x0e65953e$105$;

	// Token: 0x040005EA RID: 1514 RVA: 0x00005210 File Offset: 0x00004610
	public static uint $ConstGCArrayBound$0x0e65953e$106$;

	// Token: 0x040005EB RID: 1515 RVA: 0x0000528C File Offset: 0x0000468C
	public static uint $ConstGCArrayBound$0x0e65953e$75$;

	// Token: 0x040005EC RID: 1516 RVA: 0x0000524C File Offset: 0x0000464C
	public static uint $ConstGCArrayBound$0x0e65953e$91$;

	// Token: 0x040005ED RID: 1517 RVA: 0x000052A0 File Offset: 0x000046A0
	public static uint $ConstGCArrayBound$0x0e65953e$70$;

	// Token: 0x040005EE RID: 1518 RVA: 0x00005300 File Offset: 0x00004700
	public static uint $ConstGCArrayBound$0x0e65953e$46$;

	// Token: 0x040005EF RID: 1519 RVA: 0x00005308 File Offset: 0x00004708
	public static uint $ConstGCArrayBound$0x0e65953e$44$;

	// Token: 0x040005F0 RID: 1520 RVA: 0x00005370 File Offset: 0x00004770
	public static uint $ConstGCArrayBound$0x0e65953e$18$;

	// Token: 0x040005F1 RID: 1521 RVA: 0x00005270 File Offset: 0x00004670
	public static uint $ConstGCArrayBound$0x0e65953e$82$;

	// Token: 0x040005F2 RID: 1522 RVA: 0x000051F0 File Offset: 0x000045F0
	public static uint $ConstGCArrayBound$0x0e65953e$114$;

	// Token: 0x040005F3 RID: 1523 RVA: 0x00005240 File Offset: 0x00004640
	public static uint $ConstGCArrayBound$0x0e65953e$94$;

	// Token: 0x040005F4 RID: 1524 RVA: 0x00005398 File Offset: 0x00004798
	public static uint $ConstGCArrayBound$0x0e65953e$8$;

	// Token: 0x040005F5 RID: 1525 RVA: 0x00005208 File Offset: 0x00004608
	public static uint $ConstGCArrayBound$0x0e65953e$108$;

	// Token: 0x040005F6 RID: 1526 RVA: 0x00005330 File Offset: 0x00004730
	public static uint $ConstGCArrayBound$0x0e65953e$34$;

	// Token: 0x040005F7 RID: 1527 RVA: 0x00005224 File Offset: 0x00004624
	public static uint $ConstGCArrayBound$0x0e65953e$101$;

	// Token: 0x040005F8 RID: 1528 RVA: 0x00005220 File Offset: 0x00004620
	public static uint $ConstGCArrayBound$0x0e65953e$102$;

	// Token: 0x040005F9 RID: 1529 RVA: 0x00005268 File Offset: 0x00004668
	public static uint $ConstGCArrayBound$0x0e65953e$84$;

	// Token: 0x040005FA RID: 1530 RVA: 0x000053A4 File Offset: 0x000047A4
	public static uint $ConstGCArrayBound$0x0e65953e$5$;

	// Token: 0x040005FB RID: 1531 RVA: 0x00005324 File Offset: 0x00004724
	public static uint $ConstGCArrayBound$0x0e65953e$37$;

	// Token: 0x040005FC RID: 1532 RVA: 0x0000538C File Offset: 0x0000478C
	public static uint $ConstGCArrayBound$0x0e65953e$11$;

	// Token: 0x040005FD RID: 1533 RVA: 0x00005230 File Offset: 0x00004630
	public static uint $ConstGCArrayBound$0x0e65953e$98$;

	// Token: 0x040005FE RID: 1534 RVA: 0x00005338 File Offset: 0x00004738
	public static uint $ConstGCArrayBound$0x0e65953e$32$;

	// Token: 0x040005FF RID: 1535 RVA: 0x00005320 File Offset: 0x00004720
	public static uint $ConstGCArrayBound$0x0e65953e$38$;

	// Token: 0x04000600 RID: 1536 RVA: 0x000052A8 File Offset: 0x000046A8
	public static uint $ConstGCArrayBound$0x0e65953e$68$;

	// Token: 0x04000601 RID: 1537 RVA: 0x000052C4 File Offset: 0x000046C4
	public static uint $ConstGCArrayBound$0x0e65953e$61$;

	// Token: 0x04000602 RID: 1538 RVA: 0x000052F0 File Offset: 0x000046F0
	public static uint $ConstGCArrayBound$0x0e65953e$50$;

	// Token: 0x04000603 RID: 1539 RVA: 0x0000521C File Offset: 0x0000461C
	public static uint $ConstGCArrayBound$0x0e65953e$103$;

	// Token: 0x04000604 RID: 1540 RVA: 0x000052EC File Offset: 0x000046EC
	public static uint $ConstGCArrayBound$0x0e65953e$51$;

	// Token: 0x04000605 RID: 1541 RVA: 0x000052B0 File Offset: 0x000046B0
	public static uint $ConstGCArrayBound$0x0e65953e$66$;

	// Token: 0x04000606 RID: 1542 RVA: 0x00005328 File Offset: 0x00004728
	public static uint $ConstGCArrayBound$0x0e65953e$36$;

	// Token: 0x04000607 RID: 1543 RVA: 0x000052D4 File Offset: 0x000046D4
	public static uint $ConstGCArrayBound$0x0e65953e$57$;

	// Token: 0x04000608 RID: 1544 RVA: 0x00005354 File Offset: 0x00004754
	public static uint $ConstGCArrayBound$0x0e65953e$25$;

	// Token: 0x04000609 RID: 1545 RVA: 0x00005374 File Offset: 0x00004774
	public static uint $ConstGCArrayBound$0x0e65953e$17$;

	// Token: 0x0400060A RID: 1546 RVA: 0x000052B8 File Offset: 0x000046B8
	public static uint $ConstGCArrayBound$0x0e65953e$64$;

	// Token: 0x0400060B RID: 1547 RVA: 0x000051C4 File Offset: 0x000045C4
	public static uint $ConstGCArrayBound$0x0e65953e$125$;

	// Token: 0x0400060C RID: 1548 RVA: 0x0000526C File Offset: 0x0000466C
	public static uint $ConstGCArrayBound$0x0e65953e$83$;

	// Token: 0x0400060D RID: 1549 RVA: 0x0000523C File Offset: 0x0000463C
	public static uint $ConstGCArrayBound$0x0e65953e$95$;

	// Token: 0x0400060E RID: 1550 RVA: 0x000051C0 File Offset: 0x000045C0
	public static uint $ConstGCArrayBound$0x0e65953e$126$;

	// Token: 0x0400060F RID: 1551 RVA: 0x0000529C File Offset: 0x0000469C
	public static uint $ConstGCArrayBound$0x0e65953e$71$;

	// Token: 0x04000610 RID: 1552 RVA: 0x00005394 File Offset: 0x00004794
	public static uint $ConstGCArrayBound$0x0e65953e$9$;

	// Token: 0x04000611 RID: 1553 RVA: 0x00005304 File Offset: 0x00004704
	public static uint $ConstGCArrayBound$0x0e65953e$45$;

	// Token: 0x04000612 RID: 1554 RVA: 0x00005234 File Offset: 0x00004634
	public static uint $ConstGCArrayBound$0x0e65953e$97$;

	// Token: 0x04000613 RID: 1555 RVA: 0x000052C0 File Offset: 0x000046C0
	public static uint $ConstGCArrayBound$0x0e65953e$62$;

	// Token: 0x04000614 RID: 1556 RVA: 0x000051DC File Offset: 0x000045DC
	public static uint $ConstGCArrayBound$0x0e65953e$119$;

	// Token: 0x04000615 RID: 1557 RVA: 0x000052F8 File Offset: 0x000046F8
	public static uint $ConstGCArrayBound$0x0e65953e$48$;

	// Token: 0x04000616 RID: 1558 RVA: 0x0000530C File Offset: 0x0000470C
	public static uint $ConstGCArrayBound$0x0e65953e$43$;

	// Token: 0x04000617 RID: 1559 RVA: 0x00005238 File Offset: 0x00004638
	public static uint $ConstGCArrayBound$0x0e65953e$96$;

	// Token: 0x04000618 RID: 1560 RVA: 0x000052A4 File Offset: 0x000046A4
	public static uint $ConstGCArrayBound$0x0e65953e$69$;

	// Token: 0x04000619 RID: 1561 RVA: 0x000053A0 File Offset: 0x000047A0
	public static uint $ConstGCArrayBound$0x0e65953e$6$;

	// Token: 0x0400061A RID: 1562 RVA: 0x000051FC File Offset: 0x000045FC
	public static uint $ConstGCArrayBound$0x0e65953e$111$;

	// Token: 0x0400061B RID: 1563 RVA: 0x0000537C File Offset: 0x0000477C
	public static uint $ConstGCArrayBound$0x0e65953e$15$;

	// Token: 0x0400061C RID: 1564 RVA: 0x000053B4 File Offset: 0x000047B4
	public static uint $ConstGCArrayBound$0x0e65953e$1$;

	// Token: 0x0400061D RID: 1565 RVA: 0x00005368 File Offset: 0x00004768
	public static uint $ConstGCArrayBound$0x0e65953e$20$;

	// Token: 0x0400061E RID: 1566 RVA: 0x00005204 File Offset: 0x00004604
	public static uint $ConstGCArrayBound$0x0e65953e$109$;

	// Token: 0x0400061F RID: 1567 RVA: 0x000052E4 File Offset: 0x000046E4
	public static uint $ConstGCArrayBound$0x0e65953e$53$;

	// Token: 0x04000620 RID: 1568 RVA: 0x000052CC File Offset: 0x000046CC
	public static uint $ConstGCArrayBound$0x0e65953e$59$;

	// Token: 0x04000621 RID: 1569 RVA: 0x00005360 File Offset: 0x00004760
	public static uint $ConstGCArrayBound$0x0e65953e$22$;

	// Token: 0x04000622 RID: 1570 RVA: 0x00005228 File Offset: 0x00004628
	public static uint $ConstGCArrayBound$0x0e65953e$100$;

	// Token: 0x04000623 RID: 1571 RVA: 0x0000520C File Offset: 0x0000460C
	public static uint $ConstGCArrayBound$0x0e65953e$107$;

	// Token: 0x04000624 RID: 1572 RVA: 0x00005288 File Offset: 0x00004688
	public static uint $ConstGCArrayBound$0x0e65953e$76$;

	// Token: 0x04000625 RID: 1573 RVA: 0x000051E8 File Offset: 0x000045E8
	public static uint $ConstGCArrayBound$0x0e65953e$116$;

	// Token: 0x04000626 RID: 1574 RVA: 0x000052E0 File Offset: 0x000046E0
	public static uint $ConstGCArrayBound$0x0e65953e$54$;

	// Token: 0x04000627 RID: 1575 RVA: 0x00005334 File Offset: 0x00004734
	public static uint $ConstGCArrayBound$0x0e65953e$33$;

	// Token: 0x04000628 RID: 1576 RVA: 0x000051C8 File Offset: 0x000045C8
	public static uint $ConstGCArrayBound$0x0e65953e$124$;

	// Token: 0x04000629 RID: 1577 RVA: 0x0000531C File Offset: 0x0000471C
	public static uint $ConstGCArrayBound$0x0e65953e$39$;

	// Token: 0x0400062A RID: 1578 RVA: 0x000052B4 File Offset: 0x000046B4
	public static uint $ConstGCArrayBound$0x0e65953e$65$;

	// Token: 0x0400062B RID: 1579 RVA: 0x000051E0 File Offset: 0x000045E0
	public static uint $ConstGCArrayBound$0x0e65953e$118$;

	// Token: 0x0400062C RID: 1580 RVA: 0x00005250 File Offset: 0x00004650
	public static uint $ConstGCArrayBound$0x0e65953e$90$;

	// Token: 0x0400062D RID: 1581 RVA: 0x0000536C File Offset: 0x0000476C
	public static uint $ConstGCArrayBound$0x0e65953e$19$;

	// Token: 0x0400062E RID: 1582 RVA: 0x00005274 File Offset: 0x00004674
	public static uint $ConstGCArrayBound$0x0e65953e$81$;

	// Token: 0x0400062F RID: 1583 RVA: 0x000053AC File Offset: 0x000047AC
	public static uint $ConstGCArrayBound$0x0e65953e$3$;

	// Token: 0x04000630 RID: 1584 RVA: 0x00005298 File Offset: 0x00004698
	public static uint $ConstGCArrayBound$0x0e65953e$72$;

	// Token: 0x04000631 RID: 1585 RVA: 0x00005390 File Offset: 0x00004790
	public static uint $ConstGCArrayBound$0x0e65953e$10$;

	// Token: 0x04000632 RID: 1586 RVA: 0x00005358 File Offset: 0x00004758
	public static uint $ConstGCArrayBound$0x0e65953e$24$;

	// Token: 0x04000633 RID: 1587 RVA: 0x000051EC File Offset: 0x000045EC
	public static uint $ConstGCArrayBound$0x0e65953e$115$;

	// Token: 0x04000634 RID: 1588 RVA: 0x00005280 File Offset: 0x00004680
	public static uint $ConstGCArrayBound$0x0e65953e$78$;

	// Token: 0x04000635 RID: 1589 RVA: 0x0000533C File Offset: 0x0000473C
	public static uint $ConstGCArrayBound$0x0e65953e$31$;

	// Token: 0x04000636 RID: 1590 RVA: 0x000052BC File Offset: 0x000046BC
	public static uint $ConstGCArrayBound$0x0e65953e$63$;

	// Token: 0x04000637 RID: 1591 RVA: 0x000051D4 File Offset: 0x000045D4
	public static uint $ConstGCArrayBound$0x0e65953e$121$;

	// Token: 0x04000638 RID: 1592 RVA: 0x00005364 File Offset: 0x00004764
	public static uint $ConstGCArrayBound$0x0e65953e$21$;

	// Token: 0x04000639 RID: 1593 RVA: 0x000051BC File Offset: 0x000045BC
	public static uint $ConstGCArrayBound$0x0e65953e$127$;

	// Token: 0x0400063A RID: 1594 RVA: 0x00005318 File Offset: 0x00004718
	public static uint $ConstGCArrayBound$0x0e65953e$40$;

	// Token: 0x0400063B RID: 1595 RVA: 0x00005350 File Offset: 0x00004750
	public static uint $ConstGCArrayBound$0x0e65953e$26$;

	// Token: 0x0400063C RID: 1596 RVA: 0x000051E4 File Offset: 0x000045E4
	public static uint $ConstGCArrayBound$0x0e65953e$117$;

	// Token: 0x0400063D RID: 1597 RVA: 0x000051D8 File Offset: 0x000045D8
	public static uint $ConstGCArrayBound$0x0e65953e$120$;

	// Token: 0x0400063E RID: 1598 RVA: 0x0000525C File Offset: 0x0000465C
	public static uint $ConstGCArrayBound$0x0e65953e$87$;

	// Token: 0x0400063F RID: 1599 RVA: 0x000052DC File Offset: 0x000046DC
	public static uint $ConstGCArrayBound$0x0e65953e$55$;

	// Token: 0x04000640 RID: 1600 RVA: 0x000051B8 File Offset: 0x000045B8
	public static uint $ConstGCArrayBound$0x0e65953e$128$;

	// Token: 0x04000641 RID: 1601 RVA: 0x000052D8 File Offset: 0x000046D8
	public static uint $ConstGCArrayBound$0x0e65953e$56$;

	// Token: 0x04000642 RID: 1602 RVA: 0x0000532C File Offset: 0x0000472C
	public static uint $ConstGCArrayBound$0x0e65953e$35$;

	// Token: 0x04000643 RID: 1603 RVA: 0x0000535C File Offset: 0x0000475C
	public static uint $ConstGCArrayBound$0x0e65953e$23$;

	// Token: 0x04000644 RID: 1604 RVA: 0x00005340 File Offset: 0x00004740
	public static uint $ConstGCArrayBound$0x0e65953e$30$;

	// Token: 0x04000645 RID: 1605 RVA: 0x000051F4 File Offset: 0x000045F4
	public static uint $ConstGCArrayBound$0x0e65953e$113$;

	// Token: 0x04000646 RID: 1606 RVA: 0x0000522C File Offset: 0x0000462C
	public static uint $ConstGCArrayBound$0x0e65953e$99$;

	// Token: 0x04000647 RID: 1607 RVA: 0x000051CC File Offset: 0x000045CC
	public static uint $ConstGCArrayBound$0x0e65953e$123$;

	// Token: 0x04000648 RID: 1608 RVA: 0x00005378 File Offset: 0x00004778
	public static uint $ConstGCArrayBound$0x0e65953e$16$;

	// Token: 0x04000649 RID: 1609 RVA: 0x00005380 File Offset: 0x00004780
	public static uint $ConstGCArrayBound$0x0e65953e$14$;

	// Token: 0x0400064A RID: 1610 RVA: 0x00005348 File Offset: 0x00004748
	public static uint $ConstGCArrayBound$0x0e65953e$28$;

	// Token: 0x0400064B RID: 1611 RVA: 0x000052F4 File Offset: 0x000046F4
	public static uint $ConstGCArrayBound$0x0e65953e$49$;

	// Token: 0x0400064C RID: 1612 RVA: 0x00005278 File Offset: 0x00004678
	public static uint $ConstGCArrayBound$0x0e65953e$80$;

	// Token: 0x0400064D RID: 1613 RVA: 0x000052E8 File Offset: 0x000046E8
	public static uint $ConstGCArrayBound$0x0e65953e$52$;

	// Token: 0x0400064E RID: 1614 RVA: 0x000052D0 File Offset: 0x000046D0
	public static uint $ConstGCArrayBound$0x0e65953e$58$;

	// Token: 0x0400064F RID: 1615 RVA: 0x0000539C File Offset: 0x0000479C
	public static uint $ConstGCArrayBound$0x0e65953e$7$;

	// Token: 0x04000650 RID: 1616 RVA: 0x00005804 File Offset: 0x00004C04
	public static uint $ConstGCArrayBound$0x690e0f90$55$;

	// Token: 0x04000651 RID: 1617 RVA: 0x00005728 File Offset: 0x00004B28
	public static uint $ConstGCArrayBound$0x690e0f90$110$;

	// Token: 0x04000652 RID: 1618 RVA: 0x00005798 File Offset: 0x00004B98
	public static uint $ConstGCArrayBound$0x690e0f90$82$;

	// Token: 0x04000653 RID: 1619 RVA: 0x00005848 File Offset: 0x00004C48
	public static uint $ConstGCArrayBound$0x690e0f90$38$;

	// Token: 0x04000654 RID: 1620 RVA: 0x00005750 File Offset: 0x00004B50
	public static uint $ConstGCArrayBound$0x690e0f90$100$;

	// Token: 0x04000655 RID: 1621 RVA: 0x000057C4 File Offset: 0x00004BC4
	public static uint $ConstGCArrayBound$0x690e0f90$71$;

	// Token: 0x04000656 RID: 1622 RVA: 0x00005858 File Offset: 0x00004C58
	public static uint $ConstGCArrayBound$0x690e0f90$34$;

	// Token: 0x04000657 RID: 1623 RVA: 0x0000575C File Offset: 0x00004B5C
	public static uint $ConstGCArrayBound$0x690e0f90$97$;

	// Token: 0x04000658 RID: 1624 RVA: 0x00005714 File Offset: 0x00004B14
	public static uint $ConstGCArrayBound$0x690e0f90$115$;

	// Token: 0x04000659 RID: 1625 RVA: 0x00005860 File Offset: 0x00004C60
	public static uint $ConstGCArrayBound$0x690e0f90$32$;

	// Token: 0x0400065A RID: 1626 RVA: 0x0000576C File Offset: 0x00004B6C
	public static uint $ConstGCArrayBound$0x690e0f90$93$;

	// Token: 0x0400065B RID: 1627 RVA: 0x000057FC File Offset: 0x00004BFC
	public static uint $ConstGCArrayBound$0x690e0f90$57$;

	// Token: 0x0400065C RID: 1628 RVA: 0x0000581C File Offset: 0x00004C1C
	public static uint $ConstGCArrayBound$0x690e0f90$49$;

	// Token: 0x0400065D RID: 1629 RVA: 0x000057EC File Offset: 0x00004BEC
	public static uint $ConstGCArrayBound$0x690e0f90$61$;

	// Token: 0x0400065E RID: 1630 RVA: 0x000057D4 File Offset: 0x00004BD4
	public static uint $ConstGCArrayBound$0x690e0f90$67$;

	// Token: 0x0400065F RID: 1631 RVA: 0x00005768 File Offset: 0x00004B68
	public static uint $ConstGCArrayBound$0x690e0f90$94$;

	// Token: 0x04000660 RID: 1632 RVA: 0x0000578C File Offset: 0x00004B8C
	public static uint $ConstGCArrayBound$0x690e0f90$85$;

	// Token: 0x04000661 RID: 1633 RVA: 0x00005828 File Offset: 0x00004C28
	public static uint $ConstGCArrayBound$0x690e0f90$46$;

	// Token: 0x04000662 RID: 1634 RVA: 0x00005840 File Offset: 0x00004C40
	public static uint $ConstGCArrayBound$0x690e0f90$40$;

	// Token: 0x04000663 RID: 1635 RVA: 0x000058A4 File Offset: 0x00004CA4
	public static uint $ConstGCArrayBound$0x690e0f90$15$;

	// Token: 0x04000664 RID: 1636 RVA: 0x000057E8 File Offset: 0x00004BE8
	public static uint $ConstGCArrayBound$0x690e0f90$62$;

	// Token: 0x04000665 RID: 1637 RVA: 0x000058D4 File Offset: 0x00004CD4
	public static uint $ConstGCArrayBound$0x690e0f90$3$;

	// Token: 0x04000666 RID: 1638 RVA: 0x00005874 File Offset: 0x00004C74
	public static uint $ConstGCArrayBound$0x690e0f90$27$;

	// Token: 0x04000667 RID: 1639 RVA: 0x00005744 File Offset: 0x00004B44
	public static uint $ConstGCArrayBound$0x690e0f90$103$;

	// Token: 0x04000668 RID: 1640 RVA: 0x0000584C File Offset: 0x00004C4C
	public static uint $ConstGCArrayBound$0x690e0f90$37$;

	// Token: 0x04000669 RID: 1641 RVA: 0x00005718 File Offset: 0x00004B18
	public static uint $ConstGCArrayBound$0x690e0f90$114$;

	// Token: 0x0400066A RID: 1642 RVA: 0x00005844 File Offset: 0x00004C44
	public static uint $ConstGCArrayBound$0x690e0f90$39$;

	// Token: 0x0400066B RID: 1643 RVA: 0x000058B8 File Offset: 0x00004CB8
	public static uint $ConstGCArrayBound$0x690e0f90$10$;

	// Token: 0x0400066C RID: 1644 RVA: 0x000058D0 File Offset: 0x00004CD0
	public static uint $ConstGCArrayBound$0x690e0f90$4$;

	// Token: 0x0400066D RID: 1645 RVA: 0x00005808 File Offset: 0x00004C08
	public static uint $ConstGCArrayBound$0x690e0f90$54$;

	// Token: 0x0400066E RID: 1646 RVA: 0x0000579C File Offset: 0x00004B9C
	public static uint $ConstGCArrayBound$0x690e0f90$81$;

	// Token: 0x0400066F RID: 1647 RVA: 0x000056E4 File Offset: 0x00004AE4
	public static uint $ConstGCArrayBound$0x690e0f90$127$;

	// Token: 0x04000670 RID: 1648 RVA: 0x0000572C File Offset: 0x00004B2C
	public static uint $ConstGCArrayBound$0x690e0f90$109$;

	// Token: 0x04000671 RID: 1649 RVA: 0x000057F8 File Offset: 0x00004BF8
	public static uint $ConstGCArrayBound$0x690e0f90$58$;

	// Token: 0x04000672 RID: 1650 RVA: 0x00005760 File Offset: 0x00004B60
	public static uint $ConstGCArrayBound$0x690e0f90$96$;

	// Token: 0x04000673 RID: 1651 RVA: 0x000058B0 File Offset: 0x00004CB0
	public static uint $ConstGCArrayBound$0x690e0f90$12$;

	// Token: 0x04000674 RID: 1652 RVA: 0x00005778 File Offset: 0x00004B78
	public static uint $ConstGCArrayBound$0x690e0f90$90$;

	// Token: 0x04000675 RID: 1653 RVA: 0x00005898 File Offset: 0x00004C98
	public static uint $ConstGCArrayBound$0x690e0f90$18$;

	// Token: 0x04000676 RID: 1654 RVA: 0x0000586C File Offset: 0x00004C6C
	public static uint $ConstGCArrayBound$0x690e0f90$29$;

	// Token: 0x04000677 RID: 1655 RVA: 0x000056F8 File Offset: 0x00004AF8
	public static uint $ConstGCArrayBound$0x690e0f90$122$;

	// Token: 0x04000678 RID: 1656 RVA: 0x00005890 File Offset: 0x00004C90
	public static uint $ConstGCArrayBound$0x690e0f90$20$;

	// Token: 0x04000679 RID: 1657 RVA: 0x00005738 File Offset: 0x00004B38
	public static uint $ConstGCArrayBound$0x690e0f90$106$;

	// Token: 0x0400067A RID: 1658 RVA: 0x000056FC File Offset: 0x00004AFC
	public static uint $ConstGCArrayBound$0x690e0f90$121$;

	// Token: 0x0400067B RID: 1659 RVA: 0x00005850 File Offset: 0x00004C50
	public static uint $ConstGCArrayBound$0x690e0f90$36$;

	// Token: 0x0400067C RID: 1660 RVA: 0x000058B4 File Offset: 0x00004CB4
	public static uint $ConstGCArrayBound$0x690e0f90$11$;

	// Token: 0x0400067D RID: 1661 RVA: 0x000058CC File Offset: 0x00004CCC
	public static uint $ConstGCArrayBound$0x690e0f90$5$;

	// Token: 0x0400067E RID: 1662 RVA: 0x000058DC File Offset: 0x00004CDC
	public static uint $ConstGCArrayBound$0x690e0f90$1$;

	// Token: 0x0400067F RID: 1663 RVA: 0x00005834 File Offset: 0x00004C34
	public static uint $ConstGCArrayBound$0x690e0f90$43$;

	// Token: 0x04000680 RID: 1664 RVA: 0x00005734 File Offset: 0x00004B34
	public static uint $ConstGCArrayBound$0x690e0f90$107$;

	// Token: 0x04000681 RID: 1665 RVA: 0x00005724 File Offset: 0x00004B24
	public static uint $ConstGCArrayBound$0x690e0f90$111$;

	// Token: 0x04000682 RID: 1666 RVA: 0x00005784 File Offset: 0x00004B84
	public static uint $ConstGCArrayBound$0x690e0f90$87$;

	// Token: 0x04000683 RID: 1667 RVA: 0x00005780 File Offset: 0x00004B80
	public static uint $ConstGCArrayBound$0x690e0f90$88$;

	// Token: 0x04000684 RID: 1668 RVA: 0x00005720 File Offset: 0x00004B20
	public static uint $ConstGCArrayBound$0x690e0f90$112$;

	// Token: 0x04000685 RID: 1669 RVA: 0x000057D8 File Offset: 0x00004BD8
	public static uint $ConstGCArrayBound$0x690e0f90$66$;

	// Token: 0x04000686 RID: 1670 RVA: 0x00005758 File Offset: 0x00004B58
	public static uint $ConstGCArrayBound$0x690e0f90$98$;

	// Token: 0x04000687 RID: 1671 RVA: 0x000058D8 File Offset: 0x00004CD8
	public static uint $ConstGCArrayBound$0x690e0f90$2$;

	// Token: 0x04000688 RID: 1672 RVA: 0x00005774 File Offset: 0x00004B74
	public static uint $ConstGCArrayBound$0x690e0f90$91$;

	// Token: 0x04000689 RID: 1673 RVA: 0x00005854 File Offset: 0x00004C54
	public static uint $ConstGCArrayBound$0x690e0f90$35$;

	// Token: 0x0400068A RID: 1674 RVA: 0x0000588C File Offset: 0x00004C8C
	public static uint $ConstGCArrayBound$0x690e0f90$21$;

	// Token: 0x0400068B RID: 1675 RVA: 0x0000580C File Offset: 0x00004C0C
	public static uint $ConstGCArrayBound$0x690e0f90$53$;

	// Token: 0x0400068C RID: 1676 RVA: 0x000057DC File Offset: 0x00004BDC
	public static uint $ConstGCArrayBound$0x690e0f90$65$;

	// Token: 0x0400068D RID: 1677 RVA: 0x0000589C File Offset: 0x00004C9C
	public static uint $ConstGCArrayBound$0x690e0f90$17$;

	// Token: 0x0400068E RID: 1678 RVA: 0x00005748 File Offset: 0x00004B48
	public static uint $ConstGCArrayBound$0x690e0f90$102$;

	// Token: 0x0400068F RID: 1679 RVA: 0x00005868 File Offset: 0x00004C68
	public static uint $ConstGCArrayBound$0x690e0f90$30$;

	// Token: 0x04000690 RID: 1680 RVA: 0x0000570C File Offset: 0x00004B0C
	public static uint $ConstGCArrayBound$0x690e0f90$117$;

	// Token: 0x04000691 RID: 1681 RVA: 0x00005790 File Offset: 0x00004B90
	public static uint $ConstGCArrayBound$0x690e0f90$84$;

	// Token: 0x04000692 RID: 1682 RVA: 0x00005824 File Offset: 0x00004C24
	public static uint $ConstGCArrayBound$0x690e0f90$47$;

	// Token: 0x04000693 RID: 1683 RVA: 0x00005878 File Offset: 0x00004C78
	public static uint $ConstGCArrayBound$0x690e0f90$26$;

	// Token: 0x04000694 RID: 1684 RVA: 0x00005894 File Offset: 0x00004C94
	public static uint $ConstGCArrayBound$0x690e0f90$19$;

	// Token: 0x04000695 RID: 1685 RVA: 0x000058A0 File Offset: 0x00004CA0
	public static uint $ConstGCArrayBound$0x690e0f90$16$;

	// Token: 0x04000696 RID: 1686 RVA: 0x00005884 File Offset: 0x00004C84
	public static uint $ConstGCArrayBound$0x690e0f90$23$;

	// Token: 0x04000697 RID: 1687 RVA: 0x00005708 File Offset: 0x00004B08
	public static uint $ConstGCArrayBound$0x690e0f90$118$;

	// Token: 0x04000698 RID: 1688 RVA: 0x0000577C File Offset: 0x00004B7C
	public static uint $ConstGCArrayBound$0x690e0f90$89$;

	// Token: 0x04000699 RID: 1689 RVA: 0x00005730 File Offset: 0x00004B30
	public static uint $ConstGCArrayBound$0x690e0f90$108$;

	// Token: 0x0400069A RID: 1690 RVA: 0x000057CC File Offset: 0x00004BCC
	public static uint $ConstGCArrayBound$0x690e0f90$69$;

	// Token: 0x0400069B RID: 1691 RVA: 0x000057E0 File Offset: 0x00004BE0
	public static uint $ConstGCArrayBound$0x690e0f90$64$;

	// Token: 0x0400069C RID: 1692 RVA: 0x000058A8 File Offset: 0x00004CA8
	public static uint $ConstGCArrayBound$0x690e0f90$14$;

	// Token: 0x0400069D RID: 1693 RVA: 0x00005830 File Offset: 0x00004C30
	public static uint $ConstGCArrayBound$0x690e0f90$44$;

	// Token: 0x0400069E RID: 1694 RVA: 0x000057A4 File Offset: 0x00004BA4
	public static uint $ConstGCArrayBound$0x690e0f90$79$;

	// Token: 0x0400069F RID: 1695 RVA: 0x00005704 File Offset: 0x00004B04
	public static uint $ConstGCArrayBound$0x690e0f90$119$;

	// Token: 0x040006A0 RID: 1696 RVA: 0x00005788 File Offset: 0x00004B88
	public static uint $ConstGCArrayBound$0x690e0f90$86$;

	// Token: 0x040006A1 RID: 1697 RVA: 0x00005754 File Offset: 0x00004B54
	public static uint $ConstGCArrayBound$0x690e0f90$99$;

	// Token: 0x040006A2 RID: 1698 RVA: 0x000057C0 File Offset: 0x00004BC0
	public static uint $ConstGCArrayBound$0x690e0f90$72$;

	// Token: 0x040006A3 RID: 1699 RVA: 0x0000574C File Offset: 0x00004B4C
	public static uint $ConstGCArrayBound$0x690e0f90$101$;

	// Token: 0x040006A4 RID: 1700 RVA: 0x0000582C File Offset: 0x00004C2C
	public static uint $ConstGCArrayBound$0x690e0f90$45$;

	// Token: 0x040006A5 RID: 1701 RVA: 0x0000587C File Offset: 0x00004C7C
	public static uint $ConstGCArrayBound$0x690e0f90$25$;

	// Token: 0x040006A6 RID: 1702 RVA: 0x000058C0 File Offset: 0x00004CC0
	public static uint $ConstGCArrayBound$0x690e0f90$8$;

	// Token: 0x040006A7 RID: 1703 RVA: 0x00005818 File Offset: 0x00004C18
	public static uint $ConstGCArrayBound$0x690e0f90$50$;

	// Token: 0x040006A8 RID: 1704 RVA: 0x000057A0 File Offset: 0x00004BA0
	public static uint $ConstGCArrayBound$0x690e0f90$80$;

	// Token: 0x040006A9 RID: 1705 RVA: 0x000057B0 File Offset: 0x00004BB0
	public static uint $ConstGCArrayBound$0x690e0f90$76$;

	// Token: 0x040006AA RID: 1706 RVA: 0x000057B8 File Offset: 0x00004BB8
	public static uint $ConstGCArrayBound$0x690e0f90$74$;

	// Token: 0x040006AB RID: 1707 RVA: 0x00005820 File Offset: 0x00004C20
	public static uint $ConstGCArrayBound$0x690e0f90$48$;

	// Token: 0x040006AC RID: 1708 RVA: 0x000057B4 File Offset: 0x00004BB4
	public static uint $ConstGCArrayBound$0x690e0f90$75$;

	// Token: 0x040006AD RID: 1709 RVA: 0x00005764 File Offset: 0x00004B64
	public static uint $ConstGCArrayBound$0x690e0f90$95$;

	// Token: 0x040006AE RID: 1710 RVA: 0x00005810 File Offset: 0x00004C10
	public static uint $ConstGCArrayBound$0x690e0f90$52$;

	// Token: 0x040006AF RID: 1711 RVA: 0x000058BC File Offset: 0x00004CBC
	public static uint $ConstGCArrayBound$0x690e0f90$9$;

	// Token: 0x040006B0 RID: 1712 RVA: 0x00005814 File Offset: 0x00004C14
	public static uint $ConstGCArrayBound$0x690e0f90$51$;

	// Token: 0x040006B1 RID: 1713 RVA: 0x000057BC File Offset: 0x00004BBC
	public static uint $ConstGCArrayBound$0x690e0f90$73$;

	// Token: 0x040006B2 RID: 1714 RVA: 0x00005794 File Offset: 0x00004B94
	public static uint $ConstGCArrayBound$0x690e0f90$83$;

	// Token: 0x040006B3 RID: 1715 RVA: 0x00005770 File Offset: 0x00004B70
	public static uint $ConstGCArrayBound$0x690e0f90$92$;

	// Token: 0x040006B4 RID: 1716 RVA: 0x000057AC File Offset: 0x00004BAC
	public static uint $ConstGCArrayBound$0x690e0f90$77$;

	// Token: 0x040006B5 RID: 1717 RVA: 0x000056EC File Offset: 0x00004AEC
	public static uint $ConstGCArrayBound$0x690e0f90$125$;

	// Token: 0x040006B6 RID: 1718 RVA: 0x000057D0 File Offset: 0x00004BD0
	public static uint $ConstGCArrayBound$0x690e0f90$68$;

	// Token: 0x040006B7 RID: 1719 RVA: 0x0000573C File Offset: 0x00004B3C
	public static uint $ConstGCArrayBound$0x690e0f90$105$;

	// Token: 0x040006B8 RID: 1720 RVA: 0x000058C8 File Offset: 0x00004CC8
	public static uint $ConstGCArrayBound$0x690e0f90$6$;

	// Token: 0x040006B9 RID: 1721 RVA: 0x000056E0 File Offset: 0x00004AE0
	public static uint $ConstGCArrayBound$0x690e0f90$128$;

	// Token: 0x040006BA RID: 1722 RVA: 0x000057F4 File Offset: 0x00004BF4
	public static uint $ConstGCArrayBound$0x690e0f90$59$;

	// Token: 0x040006BB RID: 1723 RVA: 0x00005838 File Offset: 0x00004C38
	public static uint $ConstGCArrayBound$0x690e0f90$42$;

	// Token: 0x040006BC RID: 1724 RVA: 0x000057C8 File Offset: 0x00004BC8
	public static uint $ConstGCArrayBound$0x690e0f90$70$;

	// Token: 0x040006BD RID: 1725 RVA: 0x000057F0 File Offset: 0x00004BF0
	public static uint $ConstGCArrayBound$0x690e0f90$60$;

	// Token: 0x040006BE RID: 1726 RVA: 0x0000571C File Offset: 0x00004B1C
	public static uint $ConstGCArrayBound$0x690e0f90$113$;

	// Token: 0x040006BF RID: 1727 RVA: 0x0000583C File Offset: 0x00004C3C
	public static uint $ConstGCArrayBound$0x690e0f90$41$;

	// Token: 0x040006C0 RID: 1728 RVA: 0x000058AC File Offset: 0x00004CAC
	public static uint $ConstGCArrayBound$0x690e0f90$13$;

	// Token: 0x040006C1 RID: 1729 RVA: 0x00005740 File Offset: 0x00004B40
	public static uint $ConstGCArrayBound$0x690e0f90$104$;

	// Token: 0x040006C2 RID: 1730 RVA: 0x00005870 File Offset: 0x00004C70
	public static uint $ConstGCArrayBound$0x690e0f90$28$;

	// Token: 0x040006C3 RID: 1731 RVA: 0x00005888 File Offset: 0x00004C88
	public static uint $ConstGCArrayBound$0x690e0f90$22$;

	// Token: 0x040006C4 RID: 1732 RVA: 0x00005880 File Offset: 0x00004C80
	public static uint $ConstGCArrayBound$0x690e0f90$24$;

	// Token: 0x040006C5 RID: 1733 RVA: 0x0000585C File Offset: 0x00004C5C
	public static uint $ConstGCArrayBound$0x690e0f90$33$;

	// Token: 0x040006C6 RID: 1734 RVA: 0x000057E4 File Offset: 0x00004BE4
	public static uint $ConstGCArrayBound$0x690e0f90$63$;

	// Token: 0x040006C7 RID: 1735 RVA: 0x000057A8 File Offset: 0x00004BA8
	public static uint $ConstGCArrayBound$0x690e0f90$78$;

	// Token: 0x040006C8 RID: 1736 RVA: 0x00005710 File Offset: 0x00004B10
	public static uint $ConstGCArrayBound$0x690e0f90$116$;

	// Token: 0x040006C9 RID: 1737 RVA: 0x00005864 File Offset: 0x00004C64
	public static uint $ConstGCArrayBound$0x690e0f90$31$;

	// Token: 0x040006CA RID: 1738 RVA: 0x000056F0 File Offset: 0x00004AF0
	public static uint $ConstGCArrayBound$0x690e0f90$124$;

	// Token: 0x040006CB RID: 1739 RVA: 0x000056F4 File Offset: 0x00004AF4
	public static uint $ConstGCArrayBound$0x690e0f90$123$;

	// Token: 0x040006CC RID: 1740 RVA: 0x000056E8 File Offset: 0x00004AE8
	public static uint $ConstGCArrayBound$0x690e0f90$126$;

	// Token: 0x040006CD RID: 1741 RVA: 0x00005800 File Offset: 0x00004C00
	public static uint $ConstGCArrayBound$0x690e0f90$56$;

	// Token: 0x040006CE RID: 1742 RVA: 0x000058C4 File Offset: 0x00004CC4
	public static uint $ConstGCArrayBound$0x690e0f90$7$;

	// Token: 0x040006CF RID: 1743 RVA: 0x00005700 File Offset: 0x00004B00
	public static uint $ConstGCArrayBound$0x690e0f90$120$;

	// Token: 0x040006D0 RID: 1744 RVA: 0x00005CD0 File Offset: 0x000050D0
	public static uint $ConstGCArrayBound$0x79ffad46$78$;

	// Token: 0x040006D1 RID: 1745 RVA: 0x00005C4C File Offset: 0x0000504C
	public static uint $ConstGCArrayBound$0x79ffad46$111$;

	// Token: 0x040006D2 RID: 1746 RVA: 0x00005C74 File Offset: 0x00005074
	public static uint $ConstGCArrayBound$0x79ffad46$101$;

	// Token: 0x040006D3 RID: 1747 RVA: 0x00005C20 File Offset: 0x00005020
	public static uint $ConstGCArrayBound$0x79ffad46$122$;

	// Token: 0x040006D4 RID: 1748 RVA: 0x00005DF8 File Offset: 0x000051F8
	public static uint $ConstGCArrayBound$0x79ffad46$4$;

	// Token: 0x040006D5 RID: 1749 RVA: 0x00005CA8 File Offset: 0x000050A8
	public static uint $ConstGCArrayBound$0x79ffad46$88$;

	// Token: 0x040006D6 RID: 1750 RVA: 0x00005D44 File Offset: 0x00005144
	public static uint $ConstGCArrayBound$0x79ffad46$49$;

	// Token: 0x040006D7 RID: 1751 RVA: 0x00005DF4 File Offset: 0x000051F4
	public static uint $ConstGCArrayBound$0x79ffad46$5$;

	// Token: 0x040006D8 RID: 1752 RVA: 0x00005C14 File Offset: 0x00005014
	public static uint $ConstGCArrayBound$0x79ffad46$125$;

	// Token: 0x040006D9 RID: 1753 RVA: 0x00005D24 File Offset: 0x00005124
	public static uint $ConstGCArrayBound$0x79ffad46$57$;

	// Token: 0x040006DA RID: 1754 RVA: 0x00005C0C File Offset: 0x0000500C
	public static uint $ConstGCArrayBound$0x79ffad46$127$;

	// Token: 0x040006DB RID: 1755 RVA: 0x00005C34 File Offset: 0x00005034
	public static uint $ConstGCArrayBound$0x79ffad46$117$;

	// Token: 0x040006DC RID: 1756 RVA: 0x00005C3C File Offset: 0x0000503C
	public static uint $ConstGCArrayBound$0x79ffad46$115$;

	// Token: 0x040006DD RID: 1757 RVA: 0x00005C08 File Offset: 0x00005008
	public static uint $ConstGCArrayBound$0x79ffad46$128$;

	// Token: 0x040006DE RID: 1758 RVA: 0x00005C84 File Offset: 0x00005084
	public static uint $ConstGCArrayBound$0x79ffad46$97$;

	// Token: 0x040006DF RID: 1759 RVA: 0x00005CF4 File Offset: 0x000050F4
	public static uint $ConstGCArrayBound$0x79ffad46$69$;

	// Token: 0x040006E0 RID: 1760 RVA: 0x00005C78 File Offset: 0x00005078
	public static uint $ConstGCArrayBound$0x79ffad46$100$;

	// Token: 0x040006E1 RID: 1761 RVA: 0x00005DFC File Offset: 0x000051FC
	public static uint $ConstGCArrayBound$0x79ffad46$3$;

	// Token: 0x040006E2 RID: 1762 RVA: 0x00005CD8 File Offset: 0x000050D8
	public static uint $ConstGCArrayBound$0x79ffad46$76$;

	// Token: 0x040006E3 RID: 1763 RVA: 0x00005CD4 File Offset: 0x000050D4
	public static uint $ConstGCArrayBound$0x79ffad46$77$;

	// Token: 0x040006E4 RID: 1764 RVA: 0x00005C8C File Offset: 0x0000508C
	public static uint $ConstGCArrayBound$0x79ffad46$95$;

	// Token: 0x040006E5 RID: 1765 RVA: 0x00005C2C File Offset: 0x0000502C
	public static uint $ConstGCArrayBound$0x79ffad46$119$;

	// Token: 0x040006E6 RID: 1766 RVA: 0x00005C70 File Offset: 0x00005070
	public static uint $ConstGCArrayBound$0x79ffad46$102$;

	// Token: 0x040006E7 RID: 1767 RVA: 0x00005CC4 File Offset: 0x000050C4
	public static uint $ConstGCArrayBound$0x79ffad46$81$;

	// Token: 0x040006E8 RID: 1768 RVA: 0x00005C30 File Offset: 0x00005030
	public static uint $ConstGCArrayBound$0x79ffad46$118$;

	// Token: 0x040006E9 RID: 1769 RVA: 0x00005DB0 File Offset: 0x000051B0
	public static uint $ConstGCArrayBound$0x79ffad46$22$;

	// Token: 0x040006EA RID: 1770 RVA: 0x00005DA4 File Offset: 0x000051A4
	public static uint $ConstGCArrayBound$0x79ffad46$25$;

	// Token: 0x040006EB RID: 1771 RVA: 0x00005D60 File Offset: 0x00005160
	public static uint $ConstGCArrayBound$0x79ffad46$42$;

	// Token: 0x040006EC RID: 1772 RVA: 0x00005C24 File Offset: 0x00005024
	public static uint $ConstGCArrayBound$0x79ffad46$121$;

	// Token: 0x040006ED RID: 1773 RVA: 0x00005E04 File Offset: 0x00005204
	public static uint $ConstGCArrayBound$0x79ffad46$1$;

	// Token: 0x040006EE RID: 1774 RVA: 0x00005D74 File Offset: 0x00005174
	public static uint $ConstGCArrayBound$0x79ffad46$37$;

	// Token: 0x040006EF RID: 1775 RVA: 0x00005C64 File Offset: 0x00005064
	public static uint $ConstGCArrayBound$0x79ffad46$105$;

	// Token: 0x040006F0 RID: 1776 RVA: 0x00005D58 File Offset: 0x00005158
	public static uint $ConstGCArrayBound$0x79ffad46$44$;

	// Token: 0x040006F1 RID: 1777 RVA: 0x00005DB8 File Offset: 0x000051B8
	public static uint $ConstGCArrayBound$0x79ffad46$20$;

	// Token: 0x040006F2 RID: 1778 RVA: 0x00005D6C File Offset: 0x0000516C
	public static uint $ConstGCArrayBound$0x79ffad46$39$;

	// Token: 0x040006F3 RID: 1779 RVA: 0x00005D48 File Offset: 0x00005148
	public static uint $ConstGCArrayBound$0x79ffad46$48$;

	// Token: 0x040006F4 RID: 1780 RVA: 0x00005D5C File Offset: 0x0000515C
	public static uint $ConstGCArrayBound$0x79ffad46$43$;

	// Token: 0x040006F5 RID: 1781 RVA: 0x00005DC4 File Offset: 0x000051C4
	public static uint $ConstGCArrayBound$0x79ffad46$17$;

	// Token: 0x040006F6 RID: 1782 RVA: 0x00005C98 File Offset: 0x00005098
	public static uint $ConstGCArrayBound$0x79ffad46$92$;

	// Token: 0x040006F7 RID: 1783 RVA: 0x00005CFC File Offset: 0x000050FC
	public static uint $ConstGCArrayBound$0x79ffad46$67$;

	// Token: 0x040006F8 RID: 1784 RVA: 0x00005D08 File Offset: 0x00005108
	public static uint $ConstGCArrayBound$0x79ffad46$64$;

	// Token: 0x040006F9 RID: 1785 RVA: 0x00005CC0 File Offset: 0x000050C0
	public static uint $ConstGCArrayBound$0x79ffad46$82$;

	// Token: 0x040006FA RID: 1786 RVA: 0x00005CDC File Offset: 0x000050DC
	public static uint $ConstGCArrayBound$0x79ffad46$75$;

	// Token: 0x040006FB RID: 1787 RVA: 0x00005DB4 File Offset: 0x000051B4
	public static uint $ConstGCArrayBound$0x79ffad46$21$;

	// Token: 0x040006FC RID: 1788 RVA: 0x00005C58 File Offset: 0x00005058
	public static uint $ConstGCArrayBound$0x79ffad46$108$;

	// Token: 0x040006FD RID: 1789 RVA: 0x00005D70 File Offset: 0x00005170
	public static uint $ConstGCArrayBound$0x79ffad46$38$;

	// Token: 0x040006FE RID: 1790 RVA: 0x00005CE4 File Offset: 0x000050E4
	public static uint $ConstGCArrayBound$0x79ffad46$73$;

	// Token: 0x040006FF RID: 1791 RVA: 0x00005CA4 File Offset: 0x000050A4
	public static uint $ConstGCArrayBound$0x79ffad46$89$;

	// Token: 0x04000700 RID: 1792 RVA: 0x00005DF0 File Offset: 0x000051F0
	public static uint $ConstGCArrayBound$0x79ffad46$6$;

	// Token: 0x04000701 RID: 1793 RVA: 0x00005D40 File Offset: 0x00005140
	public static uint $ConstGCArrayBound$0x79ffad46$50$;

	// Token: 0x04000702 RID: 1794 RVA: 0x00005CF8 File Offset: 0x000050F8
	public static uint $ConstGCArrayBound$0x79ffad46$68$;

	// Token: 0x04000703 RID: 1795 RVA: 0x00005D50 File Offset: 0x00005150
	public static uint $ConstGCArrayBound$0x79ffad46$46$;

	// Token: 0x04000704 RID: 1796 RVA: 0x00005DDC File Offset: 0x000051DC
	public static uint $ConstGCArrayBound$0x79ffad46$11$;

	// Token: 0x04000705 RID: 1797 RVA: 0x00005CB0 File Offset: 0x000050B0
	public static uint $ConstGCArrayBound$0x79ffad46$86$;

	// Token: 0x04000706 RID: 1798 RVA: 0x00005C54 File Offset: 0x00005054
	public static uint $ConstGCArrayBound$0x79ffad46$109$;

	// Token: 0x04000707 RID: 1799 RVA: 0x00005DD8 File Offset: 0x000051D8
	public static uint $ConstGCArrayBound$0x79ffad46$12$;

	// Token: 0x04000708 RID: 1800 RVA: 0x00005CA0 File Offset: 0x000050A0
	public static uint $ConstGCArrayBound$0x79ffad46$90$;

	// Token: 0x04000709 RID: 1801 RVA: 0x00005D9C File Offset: 0x0000519C
	public static uint $ConstGCArrayBound$0x79ffad46$27$;

	// Token: 0x0400070A RID: 1802 RVA: 0x00005CC8 File Offset: 0x000050C8
	public static uint $ConstGCArrayBound$0x79ffad46$80$;

	// Token: 0x0400070B RID: 1803 RVA: 0x00005D14 File Offset: 0x00005114
	public static uint $ConstGCArrayBound$0x79ffad46$61$;

	// Token: 0x0400070C RID: 1804 RVA: 0x00005D64 File Offset: 0x00005164
	public static uint $ConstGCArrayBound$0x79ffad46$41$;

	// Token: 0x0400070D RID: 1805 RVA: 0x00005D38 File Offset: 0x00005138
	public static uint $ConstGCArrayBound$0x79ffad46$52$;

	// Token: 0x0400070E RID: 1806 RVA: 0x00005D30 File Offset: 0x00005130
	public static uint $ConstGCArrayBound$0x79ffad46$54$;

	// Token: 0x0400070F RID: 1807 RVA: 0x00005DA0 File Offset: 0x000051A0
	public static uint $ConstGCArrayBound$0x79ffad46$26$;

	// Token: 0x04000710 RID: 1808 RVA: 0x00005D3C File Offset: 0x0000513C
	public static uint $ConstGCArrayBound$0x79ffad46$51$;

	// Token: 0x04000711 RID: 1809 RVA: 0x00005C94 File Offset: 0x00005094
	public static uint $ConstGCArrayBound$0x79ffad46$93$;

	// Token: 0x04000712 RID: 1810 RVA: 0x00005C88 File Offset: 0x00005088
	public static uint $ConstGCArrayBound$0x79ffad46$96$;

	// Token: 0x04000713 RID: 1811 RVA: 0x00005C38 File Offset: 0x00005038
	public static uint $ConstGCArrayBound$0x79ffad46$116$;

	// Token: 0x04000714 RID: 1812 RVA: 0x00005DD4 File Offset: 0x000051D4
	public static uint $ConstGCArrayBound$0x79ffad46$13$;

	// Token: 0x04000715 RID: 1813 RVA: 0x00005D88 File Offset: 0x00005188
	public static uint $ConstGCArrayBound$0x79ffad46$32$;

	// Token: 0x04000716 RID: 1814 RVA: 0x00005DE0 File Offset: 0x000051E0
	public static uint $ConstGCArrayBound$0x79ffad46$10$;

	// Token: 0x04000717 RID: 1815 RVA: 0x00005DCC File Offset: 0x000051CC
	public static uint $ConstGCArrayBound$0x79ffad46$15$;

	// Token: 0x04000718 RID: 1816 RVA: 0x00005D20 File Offset: 0x00005120
	public static uint $ConstGCArrayBound$0x79ffad46$58$;

	// Token: 0x04000719 RID: 1817 RVA: 0x00005DEC File Offset: 0x000051EC
	public static uint $ConstGCArrayBound$0x79ffad46$7$;

	// Token: 0x0400071A RID: 1818 RVA: 0x00005D84 File Offset: 0x00005184
	public static uint $ConstGCArrayBound$0x79ffad46$33$;

	// Token: 0x0400071B RID: 1819 RVA: 0x00005C48 File Offset: 0x00005048
	public static uint $ConstGCArrayBound$0x79ffad46$112$;

	// Token: 0x0400071C RID: 1820 RVA: 0x00005D10 File Offset: 0x00005110
	public static uint $ConstGCArrayBound$0x79ffad46$62$;

	// Token: 0x0400071D RID: 1821 RVA: 0x00005CB8 File Offset: 0x000050B8
	public static uint $ConstGCArrayBound$0x79ffad46$84$;

	// Token: 0x0400071E RID: 1822 RVA: 0x00005C9C File Offset: 0x0000509C
	public static uint $ConstGCArrayBound$0x79ffad46$91$;

	// Token: 0x0400071F RID: 1823 RVA: 0x00005DBC File Offset: 0x000051BC
	public static uint $ConstGCArrayBound$0x79ffad46$19$;

	// Token: 0x04000720 RID: 1824 RVA: 0x00005D4C File Offset: 0x0000514C
	public static uint $ConstGCArrayBound$0x79ffad46$47$;

	// Token: 0x04000721 RID: 1825 RVA: 0x00005D90 File Offset: 0x00005190
	public static uint $ConstGCArrayBound$0x79ffad46$30$;

	// Token: 0x04000722 RID: 1826 RVA: 0x00005D94 File Offset: 0x00005194
	public static uint $ConstGCArrayBound$0x79ffad46$29$;

	// Token: 0x04000723 RID: 1827 RVA: 0x00005D68 File Offset: 0x00005168
	public static uint $ConstGCArrayBound$0x79ffad46$40$;

	// Token: 0x04000724 RID: 1828 RVA: 0x00005C80 File Offset: 0x00005080
	public static uint $ConstGCArrayBound$0x79ffad46$98$;

	// Token: 0x04000725 RID: 1829 RVA: 0x00005DA8 File Offset: 0x000051A8
	public static uint $ConstGCArrayBound$0x79ffad46$24$;

	// Token: 0x04000726 RID: 1830 RVA: 0x00005DC8 File Offset: 0x000051C8
	public static uint $ConstGCArrayBound$0x79ffad46$16$;

	// Token: 0x04000727 RID: 1831 RVA: 0x00005CE8 File Offset: 0x000050E8
	public static uint $ConstGCArrayBound$0x79ffad46$72$;

	// Token: 0x04000728 RID: 1832 RVA: 0x00005C5C File Offset: 0x0000505C
	public static uint $ConstGCArrayBound$0x79ffad46$107$;

	// Token: 0x04000729 RID: 1833 RVA: 0x00005C40 File Offset: 0x00005040
	public static uint $ConstGCArrayBound$0x79ffad46$114$;

	// Token: 0x0400072A RID: 1834 RVA: 0x00005C1C File Offset: 0x0000501C
	public static uint $ConstGCArrayBound$0x79ffad46$123$;

	// Token: 0x0400072B RID: 1835 RVA: 0x00005D1C File Offset: 0x0000511C
	public static uint $ConstGCArrayBound$0x79ffad46$59$;

	// Token: 0x0400072C RID: 1836 RVA: 0x00005CE0 File Offset: 0x000050E0
	public static uint $ConstGCArrayBound$0x79ffad46$74$;

	// Token: 0x0400072D RID: 1837 RVA: 0x00005C28 File Offset: 0x00005028
	public static uint $ConstGCArrayBound$0x79ffad46$120$;

	// Token: 0x0400072E RID: 1838 RVA: 0x00005C50 File Offset: 0x00005050
	public static uint $ConstGCArrayBound$0x79ffad46$110$;

	// Token: 0x0400072F RID: 1839 RVA: 0x00005CBC File Offset: 0x000050BC
	public static uint $ConstGCArrayBound$0x79ffad46$83$;

	// Token: 0x04000730 RID: 1840 RVA: 0x00005DC0 File Offset: 0x000051C0
	public static uint $ConstGCArrayBound$0x79ffad46$18$;

	// Token: 0x04000731 RID: 1841 RVA: 0x00005D28 File Offset: 0x00005128
	public static uint $ConstGCArrayBound$0x79ffad46$56$;

	// Token: 0x04000732 RID: 1842 RVA: 0x00005C10 File Offset: 0x00005010
	public static uint $ConstGCArrayBound$0x79ffad46$126$;

	// Token: 0x04000733 RID: 1843 RVA: 0x00005DE4 File Offset: 0x000051E4
	public static uint $ConstGCArrayBound$0x79ffad46$9$;

	// Token: 0x04000734 RID: 1844 RVA: 0x00005C6C File Offset: 0x0000506C
	public static uint $ConstGCArrayBound$0x79ffad46$103$;

	// Token: 0x04000735 RID: 1845 RVA: 0x00005D2C File Offset: 0x0000512C
	public static uint $ConstGCArrayBound$0x79ffad46$55$;

	// Token: 0x04000736 RID: 1846 RVA: 0x00005DAC File Offset: 0x000051AC
	public static uint $ConstGCArrayBound$0x79ffad46$23$;

	// Token: 0x04000737 RID: 1847 RVA: 0x00005E00 File Offset: 0x00005200
	public static uint $ConstGCArrayBound$0x79ffad46$2$;

	// Token: 0x04000738 RID: 1848 RVA: 0x00005C68 File Offset: 0x00005068
	public static uint $ConstGCArrayBound$0x79ffad46$104$;

	// Token: 0x04000739 RID: 1849 RVA: 0x00005D18 File Offset: 0x00005118
	public static uint $ConstGCArrayBound$0x79ffad46$60$;

	// Token: 0x0400073A RID: 1850 RVA: 0x00005C44 File Offset: 0x00005044
	public static uint $ConstGCArrayBound$0x79ffad46$113$;

	// Token: 0x0400073B RID: 1851 RVA: 0x00005C60 File Offset: 0x00005060
	public static uint $ConstGCArrayBound$0x79ffad46$106$;

	// Token: 0x0400073C RID: 1852 RVA: 0x00005CEC File Offset: 0x000050EC
	public static uint $ConstGCArrayBound$0x79ffad46$71$;

	// Token: 0x0400073D RID: 1853 RVA: 0x00005D80 File Offset: 0x00005180
	public static uint $ConstGCArrayBound$0x79ffad46$34$;

	// Token: 0x0400073E RID: 1854 RVA: 0x00005D34 File Offset: 0x00005134
	public static uint $ConstGCArrayBound$0x79ffad46$53$;

	// Token: 0x0400073F RID: 1855 RVA: 0x00005DD0 File Offset: 0x000051D0
	public static uint $ConstGCArrayBound$0x79ffad46$14$;

	// Token: 0x04000740 RID: 1856 RVA: 0x00005C7C File Offset: 0x0000507C
	public static uint $ConstGCArrayBound$0x79ffad46$99$;

	// Token: 0x04000741 RID: 1857 RVA: 0x00005D54 File Offset: 0x00005154
	public static uint $ConstGCArrayBound$0x79ffad46$45$;

	// Token: 0x04000742 RID: 1858 RVA: 0x00005D7C File Offset: 0x0000517C
	public static uint $ConstGCArrayBound$0x79ffad46$35$;

	// Token: 0x04000743 RID: 1859 RVA: 0x00005D98 File Offset: 0x00005198
	public static uint $ConstGCArrayBound$0x79ffad46$28$;

	// Token: 0x04000744 RID: 1860 RVA: 0x00005D00 File Offset: 0x00005100
	public static uint $ConstGCArrayBound$0x79ffad46$66$;

	// Token: 0x04000745 RID: 1861 RVA: 0x00005CB4 File Offset: 0x000050B4
	public static uint $ConstGCArrayBound$0x79ffad46$85$;

	// Token: 0x04000746 RID: 1862 RVA: 0x00005CF0 File Offset: 0x000050F0
	public static uint $ConstGCArrayBound$0x79ffad46$70$;

	// Token: 0x04000747 RID: 1863 RVA: 0x00005DE8 File Offset: 0x000051E8
	public static uint $ConstGCArrayBound$0x79ffad46$8$;

	// Token: 0x04000748 RID: 1864 RVA: 0x00005C90 File Offset: 0x00005090
	public static uint $ConstGCArrayBound$0x79ffad46$94$;

	// Token: 0x04000749 RID: 1865 RVA: 0x00005D78 File Offset: 0x00005178
	public static uint $ConstGCArrayBound$0x79ffad46$36$;

	// Token: 0x0400074A RID: 1866 RVA: 0x00005CCC File Offset: 0x000050CC
	public static uint $ConstGCArrayBound$0x79ffad46$79$;

	// Token: 0x0400074B RID: 1867 RVA: 0x00005CAC File Offset: 0x000050AC
	public static uint $ConstGCArrayBound$0x79ffad46$87$;

	// Token: 0x0400074C RID: 1868 RVA: 0x00005C18 File Offset: 0x00005018
	public static uint $ConstGCArrayBound$0x79ffad46$124$;

	// Token: 0x0400074D RID: 1869 RVA: 0x00005D0C File Offset: 0x0000510C
	public static uint $ConstGCArrayBound$0x79ffad46$63$;

	// Token: 0x0400074E RID: 1870 RVA: 0x00005D04 File Offset: 0x00005104
	public static uint $ConstGCArrayBound$0x79ffad46$65$;

	// Token: 0x0400074F RID: 1871 RVA: 0x00005D8C File Offset: 0x0000518C
	public static uint $ConstGCArrayBound$0x79ffad46$31$;

	// Token: 0x04000750 RID: 1872 RVA: 0x0000626C File Offset: 0x0000566C
	public static uint $ConstGCArrayBound$0x9192aa76$49$;

	// Token: 0x04000751 RID: 1873 RVA: 0x00006130 File Offset: 0x00005530
	public static uint $ConstGCArrayBound$0x9192aa76$128$;

	// Token: 0x04000752 RID: 1874 RVA: 0x00006158 File Offset: 0x00005558
	public static uint $ConstGCArrayBound$0x9192aa76$118$;

	// Token: 0x04000753 RID: 1875 RVA: 0x000061D4 File Offset: 0x000055D4
	public static uint $ConstGCArrayBound$0x9192aa76$87$;

	// Token: 0x04000754 RID: 1876 RVA: 0x00006320 File Offset: 0x00005720
	public static uint $ConstGCArrayBound$0x9192aa76$4$;

	// Token: 0x04000755 RID: 1877 RVA: 0x00006138 File Offset: 0x00005538
	public static uint $ConstGCArrayBound$0x9192aa76$126$;

	// Token: 0x04000756 RID: 1878 RVA: 0x000062B4 File Offset: 0x000056B4
	public static uint $ConstGCArrayBound$0x9192aa76$31$;

	// Token: 0x04000757 RID: 1879 RVA: 0x00006300 File Offset: 0x00005700
	public static uint $ConstGCArrayBound$0x9192aa76$12$;

	// Token: 0x04000758 RID: 1880 RVA: 0x000062A8 File Offset: 0x000056A8
	public static uint $ConstGCArrayBound$0x9192aa76$34$;

	// Token: 0x04000759 RID: 1881 RVA: 0x000062E0 File Offset: 0x000056E0
	public static uint $ConstGCArrayBound$0x9192aa76$20$;

	// Token: 0x0400075A RID: 1882 RVA: 0x000061DC File Offset: 0x000055DC
	public static uint $ConstGCArrayBound$0x9192aa76$85$;

	// Token: 0x0400075B RID: 1883 RVA: 0x00006214 File Offset: 0x00005614
	public static uint $ConstGCArrayBound$0x9192aa76$71$;

	// Token: 0x0400075C RID: 1884 RVA: 0x000062C8 File Offset: 0x000056C8
	public static uint $ConstGCArrayBound$0x9192aa76$26$;

	// Token: 0x0400075D RID: 1885 RVA: 0x000061E0 File Offset: 0x000055E0
	public static uint $ConstGCArrayBound$0x9192aa76$84$;

	// Token: 0x0400075E RID: 1886 RVA: 0x0000620C File Offset: 0x0000560C
	public static uint $ConstGCArrayBound$0x9192aa76$73$;

	// Token: 0x0400075F RID: 1887 RVA: 0x000062AC File Offset: 0x000056AC
	public static uint $ConstGCArrayBound$0x9192aa76$33$;

	// Token: 0x04000760 RID: 1888 RVA: 0x00006144 File Offset: 0x00005544
	public static uint $ConstGCArrayBound$0x9192aa76$123$;

	// Token: 0x04000761 RID: 1889 RVA: 0x00006270 File Offset: 0x00005670
	public static uint $ConstGCArrayBound$0x9192aa76$48$;

	// Token: 0x04000762 RID: 1890 RVA: 0x000061C0 File Offset: 0x000055C0
	public static uint $ConstGCArrayBound$0x9192aa76$92$;

	// Token: 0x04000763 RID: 1891 RVA: 0x000061F0 File Offset: 0x000055F0
	public static uint $ConstGCArrayBound$0x9192aa76$80$;

	// Token: 0x04000764 RID: 1892 RVA: 0x0000628C File Offset: 0x0000568C
	public static uint $ConstGCArrayBound$0x9192aa76$41$;

	// Token: 0x04000765 RID: 1893 RVA: 0x00006208 File Offset: 0x00005608
	public static uint $ConstGCArrayBound$0x9192aa76$74$;

	// Token: 0x04000766 RID: 1894 RVA: 0x000062B0 File Offset: 0x000056B0
	public static uint $ConstGCArrayBound$0x9192aa76$32$;

	// Token: 0x04000767 RID: 1895 RVA: 0x00006280 File Offset: 0x00005680
	public static uint $ConstGCArrayBound$0x9192aa76$44$;

	// Token: 0x04000768 RID: 1896 RVA: 0x000061D0 File Offset: 0x000055D0
	public static uint $ConstGCArrayBound$0x9192aa76$88$;

	// Token: 0x04000769 RID: 1897 RVA: 0x00006298 File Offset: 0x00005698
	public static uint $ConstGCArrayBound$0x9192aa76$38$;

	// Token: 0x0400076A RID: 1898 RVA: 0x000061FC File Offset: 0x000055FC
	public static uint $ConstGCArrayBound$0x9192aa76$77$;

	// Token: 0x0400076B RID: 1899 RVA: 0x00006324 File Offset: 0x00005724
	public static uint $ConstGCArrayBound$0x9192aa76$3$;

	// Token: 0x0400076C RID: 1900 RVA: 0x000062CC File Offset: 0x000056CC
	public static uint $ConstGCArrayBound$0x9192aa76$25$;

	// Token: 0x0400076D RID: 1901 RVA: 0x000062C0 File Offset: 0x000056C0
	public static uint $ConstGCArrayBound$0x9192aa76$28$;

	// Token: 0x0400076E RID: 1902 RVA: 0x00006314 File Offset: 0x00005714
	public static uint $ConstGCArrayBound$0x9192aa76$7$;

	// Token: 0x0400076F RID: 1903 RVA: 0x0000618C File Offset: 0x0000558C
	public static uint $ConstGCArrayBound$0x9192aa76$105$;

	// Token: 0x04000770 RID: 1904 RVA: 0x00006174 File Offset: 0x00005574
	public static uint $ConstGCArrayBound$0x9192aa76$111$;

	// Token: 0x04000771 RID: 1905 RVA: 0x00006194 File Offset: 0x00005594
	public static uint $ConstGCArrayBound$0x9192aa76$103$;

	// Token: 0x04000772 RID: 1906 RVA: 0x00006160 File Offset: 0x00005560
	public static uint $ConstGCArrayBound$0x9192aa76$116$;

	// Token: 0x04000773 RID: 1907 RVA: 0x00006284 File Offset: 0x00005684
	public static uint $ConstGCArrayBound$0x9192aa76$43$;

	// Token: 0x04000774 RID: 1908 RVA: 0x00006230 File Offset: 0x00005630
	public static uint $ConstGCArrayBound$0x9192aa76$64$;

	// Token: 0x04000775 RID: 1909 RVA: 0x00006274 File Offset: 0x00005674
	public static uint $ConstGCArrayBound$0x9192aa76$47$;

	// Token: 0x04000776 RID: 1910 RVA: 0x0000624C File Offset: 0x0000564C
	public static uint $ConstGCArrayBound$0x9192aa76$57$;

	// Token: 0x04000777 RID: 1911 RVA: 0x00006290 File Offset: 0x00005690
	public static uint $ConstGCArrayBound$0x9192aa76$40$;

	// Token: 0x04000778 RID: 1912 RVA: 0x00006318 File Offset: 0x00005718
	public static uint $ConstGCArrayBound$0x9192aa76$6$;

	// Token: 0x04000779 RID: 1913 RVA: 0x0000622C File Offset: 0x0000562C
	public static uint $ConstGCArrayBound$0x9192aa76$65$;

	// Token: 0x0400077A RID: 1914 RVA: 0x00006240 File Offset: 0x00005640
	public static uint $ConstGCArrayBound$0x9192aa76$60$;

	// Token: 0x0400077B RID: 1915 RVA: 0x000061B0 File Offset: 0x000055B0
	public static uint $ConstGCArrayBound$0x9192aa76$96$;

	// Token: 0x0400077C RID: 1916 RVA: 0x00006328 File Offset: 0x00005728
	public static uint $ConstGCArrayBound$0x9192aa76$2$;

	// Token: 0x0400077D RID: 1917 RVA: 0x00006248 File Offset: 0x00005648
	public static uint $ConstGCArrayBound$0x9192aa76$58$;

	// Token: 0x0400077E RID: 1918 RVA: 0x00006308 File Offset: 0x00005708
	public static uint $ConstGCArrayBound$0x9192aa76$10$;

	// Token: 0x0400077F RID: 1919 RVA: 0x00006178 File Offset: 0x00005578
	public static uint $ConstGCArrayBound$0x9192aa76$110$;

	// Token: 0x04000780 RID: 1920 RVA: 0x00006264 File Offset: 0x00005664
	public static uint $ConstGCArrayBound$0x9192aa76$51$;

	// Token: 0x04000781 RID: 1921 RVA: 0x00006188 File Offset: 0x00005588
	public static uint $ConstGCArrayBound$0x9192aa76$106$;

	// Token: 0x04000782 RID: 1922 RVA: 0x000061E4 File Offset: 0x000055E4
	public static uint $ConstGCArrayBound$0x9192aa76$83$;

	// Token: 0x04000783 RID: 1923 RVA: 0x000061B4 File Offset: 0x000055B4
	public static uint $ConstGCArrayBound$0x9192aa76$95$;

	// Token: 0x04000784 RID: 1924 RVA: 0x000062C4 File Offset: 0x000056C4
	public static uint $ConstGCArrayBound$0x9192aa76$27$;

	// Token: 0x04000785 RID: 1925 RVA: 0x00006238 File Offset: 0x00005638
	public static uint $ConstGCArrayBound$0x9192aa76$62$;

	// Token: 0x04000786 RID: 1926 RVA: 0x00006244 File Offset: 0x00005644
	public static uint $ConstGCArrayBound$0x9192aa76$59$;

	// Token: 0x04000787 RID: 1927 RVA: 0x00006218 File Offset: 0x00005618
	public static uint $ConstGCArrayBound$0x9192aa76$70$;

	// Token: 0x04000788 RID: 1928 RVA: 0x0000621C File Offset: 0x0000561C
	public static uint $ConstGCArrayBound$0x9192aa76$69$;

	// Token: 0x04000789 RID: 1929 RVA: 0x0000616C File Offset: 0x0000556C
	public static uint $ConstGCArrayBound$0x9192aa76$113$;

	// Token: 0x0400078A RID: 1930 RVA: 0x000061A0 File Offset: 0x000055A0
	public static uint $ConstGCArrayBound$0x9192aa76$100$;

	// Token: 0x0400078B RID: 1931 RVA: 0x000061C8 File Offset: 0x000055C8
	public static uint $ConstGCArrayBound$0x9192aa76$90$;

	// Token: 0x0400078C RID: 1932 RVA: 0x00006170 File Offset: 0x00005570
	public static uint $ConstGCArrayBound$0x9192aa76$112$;

	// Token: 0x0400078D RID: 1933 RVA: 0x0000630C File Offset: 0x0000570C
	public static uint $ConstGCArrayBound$0x9192aa76$9$;

	// Token: 0x0400078E RID: 1934 RVA: 0x0000615C File Offset: 0x0000555C
	public static uint $ConstGCArrayBound$0x9192aa76$117$;

	// Token: 0x0400078F RID: 1935 RVA: 0x00006224 File Offset: 0x00005624
	public static uint $ConstGCArrayBound$0x9192aa76$67$;

	// Token: 0x04000790 RID: 1936 RVA: 0x00006210 File Offset: 0x00005610
	public static uint $ConstGCArrayBound$0x9192aa76$72$;

	// Token: 0x04000791 RID: 1937 RVA: 0x000062D8 File Offset: 0x000056D8
	public static uint $ConstGCArrayBound$0x9192aa76$22$;

	// Token: 0x04000792 RID: 1938 RVA: 0x00006268 File Offset: 0x00005668
	public static uint $ConstGCArrayBound$0x9192aa76$50$;

	// Token: 0x04000793 RID: 1939 RVA: 0x000062F4 File Offset: 0x000056F4
	public static uint $ConstGCArrayBound$0x9192aa76$15$;

	// Token: 0x04000794 RID: 1940 RVA: 0x00006250 File Offset: 0x00005650
	public static uint $ConstGCArrayBound$0x9192aa76$56$;

	// Token: 0x04000795 RID: 1941 RVA: 0x000061B8 File Offset: 0x000055B8
	public static uint $ConstGCArrayBound$0x9192aa76$94$;

	// Token: 0x04000796 RID: 1942 RVA: 0x00006220 File Offset: 0x00005620
	public static uint $ConstGCArrayBound$0x9192aa76$68$;

	// Token: 0x04000797 RID: 1943 RVA: 0x00006154 File Offset: 0x00005554
	public static uint $ConstGCArrayBound$0x9192aa76$119$;

	// Token: 0x04000798 RID: 1944 RVA: 0x00006190 File Offset: 0x00005590
	public static uint $ConstGCArrayBound$0x9192aa76$104$;

	// Token: 0x04000799 RID: 1945 RVA: 0x000062A4 File Offset: 0x000056A4
	public static uint $ConstGCArrayBound$0x9192aa76$35$;

	// Token: 0x0400079A RID: 1946 RVA: 0x00006164 File Offset: 0x00005564
	public static uint $ConstGCArrayBound$0x9192aa76$115$;

	// Token: 0x0400079B RID: 1947 RVA: 0x0000627C File Offset: 0x0000567C
	public static uint $ConstGCArrayBound$0x9192aa76$45$;

	// Token: 0x0400079C RID: 1948 RVA: 0x000061F4 File Offset: 0x000055F4
	public static uint $ConstGCArrayBound$0x9192aa76$79$;

	// Token: 0x0400079D RID: 1949 RVA: 0x00006310 File Offset: 0x00005710
	public static uint $ConstGCArrayBound$0x9192aa76$8$;

	// Token: 0x0400079E RID: 1950 RVA: 0x000062BC File Offset: 0x000056BC
	public static uint $ConstGCArrayBound$0x9192aa76$29$;

	// Token: 0x0400079F RID: 1951 RVA: 0x0000629C File Offset: 0x0000569C
	public static uint $ConstGCArrayBound$0x9192aa76$37$;

	// Token: 0x040007A0 RID: 1952 RVA: 0x000062D4 File Offset: 0x000056D4
	public static uint $ConstGCArrayBound$0x9192aa76$23$;

	// Token: 0x040007A1 RID: 1953 RVA: 0x000062B8 File Offset: 0x000056B8
	public static uint $ConstGCArrayBound$0x9192aa76$30$;

	// Token: 0x040007A2 RID: 1954 RVA: 0x00006258 File Offset: 0x00005658
	public static uint $ConstGCArrayBound$0x9192aa76$54$;

	// Token: 0x040007A3 RID: 1955 RVA: 0x000062EC File Offset: 0x000056EC
	public static uint $ConstGCArrayBound$0x9192aa76$17$;

	// Token: 0x040007A4 RID: 1956 RVA: 0x00006288 File Offset: 0x00005688
	public static uint $ConstGCArrayBound$0x9192aa76$42$;

	// Token: 0x040007A5 RID: 1957 RVA: 0x000061F8 File Offset: 0x000055F8
	public static uint $ConstGCArrayBound$0x9192aa76$78$;

	// Token: 0x040007A6 RID: 1958 RVA: 0x000061CC File Offset: 0x000055CC
	public static uint $ConstGCArrayBound$0x9192aa76$89$;

	// Token: 0x040007A7 RID: 1959 RVA: 0x00006198 File Offset: 0x00005598
	public static uint $ConstGCArrayBound$0x9192aa76$102$;

	// Token: 0x040007A8 RID: 1960 RVA: 0x00006200 File Offset: 0x00005600
	public static uint $ConstGCArrayBound$0x9192aa76$76$;

	// Token: 0x040007A9 RID: 1961 RVA: 0x000062DC File Offset: 0x000056DC
	public static uint $ConstGCArrayBound$0x9192aa76$21$;

	// Token: 0x040007AA RID: 1962 RVA: 0x000062F8 File Offset: 0x000056F8
	public static uint $ConstGCArrayBound$0x9192aa76$14$;

	// Token: 0x040007AB RID: 1963 RVA: 0x00006150 File Offset: 0x00005550
	public static uint $ConstGCArrayBound$0x9192aa76$120$;

	// Token: 0x040007AC RID: 1964 RVA: 0x00006204 File Offset: 0x00005604
	public static uint $ConstGCArrayBound$0x9192aa76$75$;

	// Token: 0x040007AD RID: 1965 RVA: 0x00006254 File Offset: 0x00005654
	public static uint $ConstGCArrayBound$0x9192aa76$55$;

	// Token: 0x040007AE RID: 1966 RVA: 0x00006168 File Offset: 0x00005568
	public static uint $ConstGCArrayBound$0x9192aa76$114$;

	// Token: 0x040007AF RID: 1967 RVA: 0x000061EC File Offset: 0x000055EC
	public static uint $ConstGCArrayBound$0x9192aa76$81$;

	// Token: 0x040007B0 RID: 1968 RVA: 0x00006140 File Offset: 0x00005540
	public static uint $ConstGCArrayBound$0x9192aa76$124$;

	// Token: 0x040007B1 RID: 1969 RVA: 0x000061A4 File Offset: 0x000055A4
	public static uint $ConstGCArrayBound$0x9192aa76$99$;

	// Token: 0x040007B2 RID: 1970 RVA: 0x000061BC File Offset: 0x000055BC
	public static uint $ConstGCArrayBound$0x9192aa76$93$;

	// Token: 0x040007B3 RID: 1971 RVA: 0x000061AC File Offset: 0x000055AC
	public static uint $ConstGCArrayBound$0x9192aa76$97$;

	// Token: 0x040007B4 RID: 1972 RVA: 0x00006180 File Offset: 0x00005580
	public static uint $ConstGCArrayBound$0x9192aa76$108$;

	// Token: 0x040007B5 RID: 1973 RVA: 0x0000613C File Offset: 0x0000553C
	public static uint $ConstGCArrayBound$0x9192aa76$125$;

	// Token: 0x040007B6 RID: 1974 RVA: 0x00006134 File Offset: 0x00005534
	public static uint $ConstGCArrayBound$0x9192aa76$127$;

	// Token: 0x040007B7 RID: 1975 RVA: 0x000061C4 File Offset: 0x000055C4
	public static uint $ConstGCArrayBound$0x9192aa76$91$;

	// Token: 0x040007B8 RID: 1976 RVA: 0x00006184 File Offset: 0x00005584
	public static uint $ConstGCArrayBound$0x9192aa76$107$;

	// Token: 0x040007B9 RID: 1977 RVA: 0x0000632C File Offset: 0x0000572C
	public static uint $ConstGCArrayBound$0x9192aa76$1$;

	// Token: 0x040007BA RID: 1978 RVA: 0x0000625C File Offset: 0x0000565C
	public static uint $ConstGCArrayBound$0x9192aa76$53$;

	// Token: 0x040007BB RID: 1979 RVA: 0x00006228 File Offset: 0x00005628
	public static uint $ConstGCArrayBound$0x9192aa76$66$;

	// Token: 0x040007BC RID: 1980 RVA: 0x00006260 File Offset: 0x00005660
	public static uint $ConstGCArrayBound$0x9192aa76$52$;

	// Token: 0x040007BD RID: 1981 RVA: 0x00006234 File Offset: 0x00005634
	public static uint $ConstGCArrayBound$0x9192aa76$63$;

	// Token: 0x040007BE RID: 1982 RVA: 0x000062FC File Offset: 0x000056FC
	public static uint $ConstGCArrayBound$0x9192aa76$13$;

	// Token: 0x040007BF RID: 1983 RVA: 0x0000631C File Offset: 0x0000571C
	public static uint $ConstGCArrayBound$0x9192aa76$5$;

	// Token: 0x040007C0 RID: 1984 RVA: 0x000061E8 File Offset: 0x000055E8
	public static uint $ConstGCArrayBound$0x9192aa76$82$;

	// Token: 0x040007C1 RID: 1985 RVA: 0x0000623C File Offset: 0x0000563C
	public static uint $ConstGCArrayBound$0x9192aa76$61$;

	// Token: 0x040007C2 RID: 1986 RVA: 0x00006148 File Offset: 0x00005548
	public static uint $ConstGCArrayBound$0x9192aa76$122$;

	// Token: 0x040007C3 RID: 1987 RVA: 0x000062F0 File Offset: 0x000056F0
	public static uint $ConstGCArrayBound$0x9192aa76$16$;

	// Token: 0x040007C4 RID: 1988 RVA: 0x000062E4 File Offset: 0x000056E4
	public static uint $ConstGCArrayBound$0x9192aa76$19$;

	// Token: 0x040007C5 RID: 1989 RVA: 0x000062D0 File Offset: 0x000056D0
	public static uint $ConstGCArrayBound$0x9192aa76$24$;

	// Token: 0x040007C6 RID: 1990 RVA: 0x000061D8 File Offset: 0x000055D8
	public static uint $ConstGCArrayBound$0x9192aa76$86$;

	// Token: 0x040007C7 RID: 1991 RVA: 0x00006304 File Offset: 0x00005704
	public static uint $ConstGCArrayBound$0x9192aa76$11$;

	// Token: 0x040007C8 RID: 1992 RVA: 0x0000619C File Offset: 0x0000559C
	public static uint $ConstGCArrayBound$0x9192aa76$101$;

	// Token: 0x040007C9 RID: 1993 RVA: 0x000061A8 File Offset: 0x000055A8
	public static uint $ConstGCArrayBound$0x9192aa76$98$;

	// Token: 0x040007CA RID: 1994 RVA: 0x00006294 File Offset: 0x00005694
	public static uint $ConstGCArrayBound$0x9192aa76$39$;

	// Token: 0x040007CB RID: 1995 RVA: 0x0000617C File Offset: 0x0000557C
	public static uint $ConstGCArrayBound$0x9192aa76$109$;

	// Token: 0x040007CC RID: 1996 RVA: 0x000062A0 File Offset: 0x000056A0
	public static uint $ConstGCArrayBound$0x9192aa76$36$;

	// Token: 0x040007CD RID: 1997 RVA: 0x00006278 File Offset: 0x00005678
	public static uint $ConstGCArrayBound$0x9192aa76$46$;

	// Token: 0x040007CE RID: 1998 RVA: 0x0000614C File Offset: 0x0000554C
	public static uint $ConstGCArrayBound$0x9192aa76$121$;

	// Token: 0x040007CF RID: 1999 RVA: 0x000062E8 File Offset: 0x000056E8
	public static uint $ConstGCArrayBound$0x9192aa76$18$;

	// Token: 0x040007D0 RID: 2000 RVA: 0x0000681C File Offset: 0x00005C1C
	public static uint $ConstGCArrayBound$0xb2c543d0$15$;

	// Token: 0x040007D1 RID: 2001 RVA: 0x00006680 File Offset: 0x00005A80
	public static uint $ConstGCArrayBound$0xb2c543d0$118$;

	// Token: 0x040007D2 RID: 2002 RVA: 0x000066E4 File Offset: 0x00005AE4
	public static uint $ConstGCArrayBound$0xb2c543d0$93$;

	// Token: 0x040007D3 RID: 2003 RVA: 0x0000676C File Offset: 0x00005B6C
	public static uint $ConstGCArrayBound$0xb2c543d0$59$;

	// Token: 0x040007D4 RID: 2004 RVA: 0x00006810 File Offset: 0x00005C10
	public static uint $ConstGCArrayBound$0xb2c543d0$18$;

	// Token: 0x040007D5 RID: 2005 RVA: 0x00006688 File Offset: 0x00005A88
	public static uint $ConstGCArrayBound$0xb2c543d0$116$;

	// Token: 0x040007D6 RID: 2006 RVA: 0x000067D8 File Offset: 0x00005BD8
	public static uint $ConstGCArrayBound$0xb2c543d0$32$;

	// Token: 0x040007D7 RID: 2007 RVA: 0x00006844 File Offset: 0x00005C44
	public static uint $ConstGCArrayBound$0xb2c543d0$5$;

	// Token: 0x040007D8 RID: 2008 RVA: 0x000066BC File Offset: 0x00005ABC
	public static uint $ConstGCArrayBound$0xb2c543d0$103$;

	// Token: 0x040007D9 RID: 2009 RVA: 0x00006804 File Offset: 0x00005C04
	public static uint $ConstGCArrayBound$0xb2c543d0$21$;

	// Token: 0x040007DA RID: 2010 RVA: 0x00006698 File Offset: 0x00005A98
	public static uint $ConstGCArrayBound$0xb2c543d0$112$;

	// Token: 0x040007DB RID: 2011 RVA: 0x0000677C File Offset: 0x00005B7C
	public static uint $ConstGCArrayBound$0xb2c543d0$55$;

	// Token: 0x040007DC RID: 2012 RVA: 0x000066E8 File Offset: 0x00005AE8
	public static uint $ConstGCArrayBound$0xb2c543d0$92$;

	// Token: 0x040007DD RID: 2013 RVA: 0x000067C4 File Offset: 0x00005BC4
	public static uint $ConstGCArrayBound$0xb2c543d0$37$;

	// Token: 0x040007DE RID: 2014 RVA: 0x000066C0 File Offset: 0x00005AC0
	public static uint $ConstGCArrayBound$0xb2c543d0$102$;

	// Token: 0x040007DF RID: 2015 RVA: 0x00006694 File Offset: 0x00005A94
	public static uint $ConstGCArrayBound$0xb2c543d0$113$;

	// Token: 0x040007E0 RID: 2016 RVA: 0x0000672C File Offset: 0x00005B2C
	public static uint $ConstGCArrayBound$0xb2c543d0$75$;

	// Token: 0x040007E1 RID: 2017 RVA: 0x00006830 File Offset: 0x00005C30
	public static uint $ConstGCArrayBound$0xb2c543d0$10$;

	// Token: 0x040007E2 RID: 2018 RVA: 0x0000680C File Offset: 0x00005C0C
	public static uint $ConstGCArrayBound$0xb2c543d0$19$;

	// Token: 0x040007E3 RID: 2019 RVA: 0x00006824 File Offset: 0x00005C24
	public static uint $ConstGCArrayBound$0xb2c543d0$13$;

	// Token: 0x040007E4 RID: 2020 RVA: 0x000067A4 File Offset: 0x00005BA4
	public static uint $ConstGCArrayBound$0xb2c543d0$45$;

	// Token: 0x040007E5 RID: 2021 RVA: 0x00006724 File Offset: 0x00005B24
	public static uint $ConstGCArrayBound$0xb2c543d0$77$;

	// Token: 0x040007E6 RID: 2022 RVA: 0x000067A0 File Offset: 0x00005BA0
	public static uint $ConstGCArrayBound$0xb2c543d0$46$;

	// Token: 0x040007E7 RID: 2023 RVA: 0x000067F8 File Offset: 0x00005BF8
	public static uint $ConstGCArrayBound$0xb2c543d0$24$;

	// Token: 0x040007E8 RID: 2024 RVA: 0x0000678C File Offset: 0x00005B8C
	public static uint $ConstGCArrayBound$0xb2c543d0$51$;

	// Token: 0x040007E9 RID: 2025 RVA: 0x0000665C File Offset: 0x00005A5C
	public static uint $ConstGCArrayBound$0xb2c543d0$127$;

	// Token: 0x040007EA RID: 2026 RVA: 0x00006748 File Offset: 0x00005B48
	public static uint $ConstGCArrayBound$0xb2c543d0$68$;

	// Token: 0x040007EB RID: 2027 RVA: 0x00006838 File Offset: 0x00005C38
	public static uint $ConstGCArrayBound$0xb2c543d0$8$;

	// Token: 0x040007EC RID: 2028 RVA: 0x000066C4 File Offset: 0x00005AC4
	public static uint $ConstGCArrayBound$0xb2c543d0$101$;

	// Token: 0x040007ED RID: 2029 RVA: 0x000067B4 File Offset: 0x00005BB4
	public static uint $ConstGCArrayBound$0xb2c543d0$41$;

	// Token: 0x040007EE RID: 2030 RVA: 0x000066FC File Offset: 0x00005AFC
	public static uint $ConstGCArrayBound$0xb2c543d0$87$;

	// Token: 0x040007EF RID: 2031 RVA: 0x00006750 File Offset: 0x00005B50
	public static uint $ConstGCArrayBound$0xb2c543d0$66$;

	// Token: 0x040007F0 RID: 2032 RVA: 0x0000683C File Offset: 0x00005C3C
	public static uint $ConstGCArrayBound$0xb2c543d0$7$;

	// Token: 0x040007F1 RID: 2033 RVA: 0x00006778 File Offset: 0x00005B78
	public static uint $ConstGCArrayBound$0xb2c543d0$56$;

	// Token: 0x040007F2 RID: 2034 RVA: 0x000066DC File Offset: 0x00005ADC
	public static uint $ConstGCArrayBound$0xb2c543d0$95$;

	// Token: 0x040007F3 RID: 2035 RVA: 0x00006684 File Offset: 0x00005A84
	public static uint $ConstGCArrayBound$0xb2c543d0$117$;

	// Token: 0x040007F4 RID: 2036 RVA: 0x000067E0 File Offset: 0x00005BE0
	public static uint $ConstGCArrayBound$0xb2c543d0$30$;

	// Token: 0x040007F5 RID: 2037 RVA: 0x000067AC File Offset: 0x00005BAC
	public static uint $ConstGCArrayBound$0xb2c543d0$43$;

	// Token: 0x040007F6 RID: 2038 RVA: 0x00006738 File Offset: 0x00005B38
	public static uint $ConstGCArrayBound$0xb2c543d0$72$;

	// Token: 0x040007F7 RID: 2039 RVA: 0x0000674C File Offset: 0x00005B4C
	public static uint $ConstGCArrayBound$0xb2c543d0$67$;

	// Token: 0x040007F8 RID: 2040 RVA: 0x0000669C File Offset: 0x00005A9C
	public static uint $ConstGCArrayBound$0xb2c543d0$111$;

	// Token: 0x040007F9 RID: 2041 RVA: 0x00006774 File Offset: 0x00005B74
	public static uint $ConstGCArrayBound$0xb2c543d0$57$;

	// Token: 0x040007FA RID: 2042 RVA: 0x00006850 File Offset: 0x00005C50
	public static uint $ConstGCArrayBound$0xb2c543d0$2$;

	// Token: 0x040007FB RID: 2043 RVA: 0x000067B8 File Offset: 0x00005BB8
	public static uint $ConstGCArrayBound$0xb2c543d0$40$;

	// Token: 0x040007FC RID: 2044 RVA: 0x0000671C File Offset: 0x00005B1C
	public static uint $ConstGCArrayBound$0xb2c543d0$79$;

	// Token: 0x040007FD RID: 2045 RVA: 0x00006808 File Offset: 0x00005C08
	public static uint $ConstGCArrayBound$0xb2c543d0$20$;

	// Token: 0x040007FE RID: 2046 RVA: 0x00006710 File Offset: 0x00005B10
	public static uint $ConstGCArrayBound$0xb2c543d0$82$;

	// Token: 0x040007FF RID: 2047 RVA: 0x000067A8 File Offset: 0x00005BA8
	public static uint $ConstGCArrayBound$0xb2c543d0$44$;

	// Token: 0x04000800 RID: 2048 RVA: 0x000067D4 File Offset: 0x00005BD4
	public static uint $ConstGCArrayBound$0xb2c543d0$33$;

	// Token: 0x04000801 RID: 2049 RVA: 0x0000673C File Offset: 0x00005B3C
	public static uint $ConstGCArrayBound$0xb2c543d0$71$;

	// Token: 0x04000802 RID: 2050 RVA: 0x000067BC File Offset: 0x00005BBC
	public static uint $ConstGCArrayBound$0xb2c543d0$39$;

	// Token: 0x04000803 RID: 2051 RVA: 0x000066D8 File Offset: 0x00005AD8
	public static uint $ConstGCArrayBound$0xb2c543d0$96$;

	// Token: 0x04000804 RID: 2052 RVA: 0x000067EC File Offset: 0x00005BEC
	public static uint $ConstGCArrayBound$0xb2c543d0$27$;

	// Token: 0x04000805 RID: 2053 RVA: 0x00006818 File Offset: 0x00005C18
	public static uint $ConstGCArrayBound$0xb2c543d0$16$;

	// Token: 0x04000806 RID: 2054 RVA: 0x000067D0 File Offset: 0x00005BD0
	public static uint $ConstGCArrayBound$0xb2c543d0$34$;

	// Token: 0x04000807 RID: 2055 RVA: 0x00006660 File Offset: 0x00005A60
	public static uint $ConstGCArrayBound$0xb2c543d0$126$;

	// Token: 0x04000808 RID: 2056 RVA: 0x000067CC File Offset: 0x00005BCC
	public static uint $ConstGCArrayBound$0xb2c543d0$35$;

	// Token: 0x04000809 RID: 2057 RVA: 0x00006848 File Offset: 0x00005C48
	public static uint $ConstGCArrayBound$0xb2c543d0$4$;

	// Token: 0x0400080A RID: 2058 RVA: 0x00006780 File Offset: 0x00005B80
	public static uint $ConstGCArrayBound$0xb2c543d0$54$;

	// Token: 0x0400080B RID: 2059 RVA: 0x00006668 File Offset: 0x00005A68
	public static uint $ConstGCArrayBound$0xb2c543d0$124$;

	// Token: 0x0400080C RID: 2060 RVA: 0x00006828 File Offset: 0x00005C28
	public static uint $ConstGCArrayBound$0xb2c543d0$12$;

	// Token: 0x0400080D RID: 2061 RVA: 0x000066AC File Offset: 0x00005AAC
	public static uint $ConstGCArrayBound$0xb2c543d0$107$;

	// Token: 0x0400080E RID: 2062 RVA: 0x0000668C File Offset: 0x00005A8C
	public static uint $ConstGCArrayBound$0xb2c543d0$115$;

	// Token: 0x0400080F RID: 2063 RVA: 0x00006704 File Offset: 0x00005B04
	public static uint $ConstGCArrayBound$0xb2c543d0$85$;

	// Token: 0x04000810 RID: 2064 RVA: 0x00006658 File Offset: 0x00005A58
	public static uint $ConstGCArrayBound$0xb2c543d0$128$;

	// Token: 0x04000811 RID: 2065 RVA: 0x0000679C File Offset: 0x00005B9C
	public static uint $ConstGCArrayBound$0xb2c543d0$47$;

	// Token: 0x04000812 RID: 2066 RVA: 0x00006728 File Offset: 0x00005B28
	public static uint $ConstGCArrayBound$0xb2c543d0$76$;

	// Token: 0x04000813 RID: 2067 RVA: 0x0000682C File Offset: 0x00005C2C
	public static uint $ConstGCArrayBound$0xb2c543d0$11$;

	// Token: 0x04000814 RID: 2068 RVA: 0x000067B0 File Offset: 0x00005BB0
	public static uint $ConstGCArrayBound$0xb2c543d0$42$;

	// Token: 0x04000815 RID: 2069 RVA: 0x000066B8 File Offset: 0x00005AB8
	public static uint $ConstGCArrayBound$0xb2c543d0$104$;

	// Token: 0x04000816 RID: 2070 RVA: 0x00006770 File Offset: 0x00005B70
	public static uint $ConstGCArrayBound$0xb2c543d0$58$;

	// Token: 0x04000817 RID: 2071 RVA: 0x0000666C File Offset: 0x00005A6C
	public static uint $ConstGCArrayBound$0xb2c543d0$123$;

	// Token: 0x04000818 RID: 2072 RVA: 0x000066A8 File Offset: 0x00005AA8
	public static uint $ConstGCArrayBound$0xb2c543d0$108$;

	// Token: 0x04000819 RID: 2073 RVA: 0x00006700 File Offset: 0x00005B00
	public static uint $ConstGCArrayBound$0xb2c543d0$86$;

	// Token: 0x0400081A RID: 2074 RVA: 0x00006854 File Offset: 0x00005C54
	public static uint $ConstGCArrayBound$0xb2c543d0$1$;

	// Token: 0x0400081B RID: 2075 RVA: 0x000067C0 File Offset: 0x00005BC0
	public static uint $ConstGCArrayBound$0xb2c543d0$38$;

	// Token: 0x0400081C RID: 2076 RVA: 0x00006760 File Offset: 0x00005B60
	public static uint $ConstGCArrayBound$0xb2c543d0$62$;

	// Token: 0x0400081D RID: 2077 RVA: 0x00006794 File Offset: 0x00005B94
	public static uint $ConstGCArrayBound$0xb2c543d0$49$;

	// Token: 0x0400081E RID: 2078 RVA: 0x00006800 File Offset: 0x00005C00
	public static uint $ConstGCArrayBound$0xb2c543d0$22$;

	// Token: 0x0400081F RID: 2079 RVA: 0x000066F0 File Offset: 0x00005AF0
	public static uint $ConstGCArrayBound$0xb2c543d0$90$;

	// Token: 0x04000820 RID: 2080 RVA: 0x0000684C File Offset: 0x00005C4C
	public static uint $ConstGCArrayBound$0xb2c543d0$3$;

	// Token: 0x04000821 RID: 2081 RVA: 0x000067DC File Offset: 0x00005BDC
	public static uint $ConstGCArrayBound$0xb2c543d0$31$;

	// Token: 0x04000822 RID: 2082 RVA: 0x000066F8 File Offset: 0x00005AF8
	public static uint $ConstGCArrayBound$0xb2c543d0$88$;

	// Token: 0x04000823 RID: 2083 RVA: 0x00006788 File Offset: 0x00005B88
	public static uint $ConstGCArrayBound$0xb2c543d0$52$;

	// Token: 0x04000824 RID: 2084 RVA: 0x000066B0 File Offset: 0x00005AB0
	public static uint $ConstGCArrayBound$0xb2c543d0$106$;

	// Token: 0x04000825 RID: 2085 RVA: 0x00006720 File Offset: 0x00005B20
	public static uint $ConstGCArrayBound$0xb2c543d0$78$;

	// Token: 0x04000826 RID: 2086 RVA: 0x00006670 File Offset: 0x00005A70
	public static uint $ConstGCArrayBound$0xb2c543d0$122$;

	// Token: 0x04000827 RID: 2087 RVA: 0x0000675C File Offset: 0x00005B5C
	public static uint $ConstGCArrayBound$0xb2c543d0$63$;

	// Token: 0x04000828 RID: 2088 RVA: 0x00006834 File Offset: 0x00005C34
	public static uint $ConstGCArrayBound$0xb2c543d0$9$;

	// Token: 0x04000829 RID: 2089 RVA: 0x00006758 File Offset: 0x00005B58
	public static uint $ConstGCArrayBound$0xb2c543d0$64$;

	// Token: 0x0400082A RID: 2090 RVA: 0x00006714 File Offset: 0x00005B14
	public static uint $ConstGCArrayBound$0xb2c543d0$81$;

	// Token: 0x0400082B RID: 2091 RVA: 0x00006730 File Offset: 0x00005B30
	public static uint $ConstGCArrayBound$0xb2c543d0$74$;

	// Token: 0x0400082C RID: 2092 RVA: 0x000066C8 File Offset: 0x00005AC8
	public static uint $ConstGCArrayBound$0xb2c543d0$100$;

	// Token: 0x0400082D RID: 2093 RVA: 0x00006814 File Offset: 0x00005C14
	public static uint $ConstGCArrayBound$0xb2c543d0$17$;

	// Token: 0x0400082E RID: 2094 RVA: 0x00006678 File Offset: 0x00005A78
	public static uint $ConstGCArrayBound$0xb2c543d0$120$;

	// Token: 0x0400082F RID: 2095 RVA: 0x00006690 File Offset: 0x00005A90
	public static uint $ConstGCArrayBound$0xb2c543d0$114$;

	// Token: 0x04000830 RID: 2096 RVA: 0x000067C8 File Offset: 0x00005BC8
	public static uint $ConstGCArrayBound$0xb2c543d0$36$;

	// Token: 0x04000831 RID: 2097 RVA: 0x000067FC File Offset: 0x00005BFC
	public static uint $ConstGCArrayBound$0xb2c543d0$23$;

	// Token: 0x04000832 RID: 2098 RVA: 0x00006754 File Offset: 0x00005B54
	public static uint $ConstGCArrayBound$0xb2c543d0$65$;

	// Token: 0x04000833 RID: 2099 RVA: 0x00006674 File Offset: 0x00005A74
	public static uint $ConstGCArrayBound$0xb2c543d0$121$;

	// Token: 0x04000834 RID: 2100 RVA: 0x000066A0 File Offset: 0x00005AA0
	public static uint $ConstGCArrayBound$0xb2c543d0$110$;

	// Token: 0x04000835 RID: 2101 RVA: 0x00006798 File Offset: 0x00005B98
	public static uint $ConstGCArrayBound$0xb2c543d0$48$;

	// Token: 0x04000836 RID: 2102 RVA: 0x000067E8 File Offset: 0x00005BE8
	public static uint $ConstGCArrayBound$0xb2c543d0$28$;

	// Token: 0x04000837 RID: 2103 RVA: 0x0000670C File Offset: 0x00005B0C
	public static uint $ConstGCArrayBound$0xb2c543d0$83$;

	// Token: 0x04000838 RID: 2104 RVA: 0x000066D4 File Offset: 0x00005AD4
	public static uint $ConstGCArrayBound$0xb2c543d0$97$;

	// Token: 0x04000839 RID: 2105 RVA: 0x00006790 File Offset: 0x00005B90
	public static uint $ConstGCArrayBound$0xb2c543d0$50$;

	// Token: 0x0400083A RID: 2106 RVA: 0x00006784 File Offset: 0x00005B84
	public static uint $ConstGCArrayBound$0xb2c543d0$53$;

	// Token: 0x0400083B RID: 2107 RVA: 0x000067F0 File Offset: 0x00005BF0
	public static uint $ConstGCArrayBound$0xb2c543d0$26$;

	// Token: 0x0400083C RID: 2108 RVA: 0x000067F4 File Offset: 0x00005BF4
	public static uint $ConstGCArrayBound$0xb2c543d0$25$;

	// Token: 0x0400083D RID: 2109 RVA: 0x000066CC File Offset: 0x00005ACC
	public static uint $ConstGCArrayBound$0xb2c543d0$99$;

	// Token: 0x0400083E RID: 2110 RVA: 0x00006734 File Offset: 0x00005B34
	public static uint $ConstGCArrayBound$0xb2c543d0$73$;

	// Token: 0x0400083F RID: 2111 RVA: 0x000066A4 File Offset: 0x00005AA4
	public static uint $ConstGCArrayBound$0xb2c543d0$109$;

	// Token: 0x04000840 RID: 2112 RVA: 0x000066EC File Offset: 0x00005AEC
	public static uint $ConstGCArrayBound$0xb2c543d0$91$;

	// Token: 0x04000841 RID: 2113 RVA: 0x000066D0 File Offset: 0x00005AD0
	public static uint $ConstGCArrayBound$0xb2c543d0$98$;

	// Token: 0x04000842 RID: 2114 RVA: 0x00006664 File Offset: 0x00005A64
	public static uint $ConstGCArrayBound$0xb2c543d0$125$;

	// Token: 0x04000843 RID: 2115 RVA: 0x000066B4 File Offset: 0x00005AB4
	public static uint $ConstGCArrayBound$0xb2c543d0$105$;

	// Token: 0x04000844 RID: 2116 RVA: 0x00006840 File Offset: 0x00005C40
	public static uint $ConstGCArrayBound$0xb2c543d0$6$;

	// Token: 0x04000845 RID: 2117 RVA: 0x00006740 File Offset: 0x00005B40
	public static uint $ConstGCArrayBound$0xb2c543d0$70$;

	// Token: 0x04000846 RID: 2118 RVA: 0x000066F4 File Offset: 0x00005AF4
	public static uint $ConstGCArrayBound$0xb2c543d0$89$;

	// Token: 0x04000847 RID: 2119 RVA: 0x00006820 File Offset: 0x00005C20
	public static uint $ConstGCArrayBound$0xb2c543d0$14$;

	// Token: 0x04000848 RID: 2120 RVA: 0x00006764 File Offset: 0x00005B64
	public static uint $ConstGCArrayBound$0xb2c543d0$61$;

	// Token: 0x04000849 RID: 2121 RVA: 0x0000667C File Offset: 0x00005A7C
	public static uint $ConstGCArrayBound$0xb2c543d0$119$;

	// Token: 0x0400084A RID: 2122 RVA: 0x00006718 File Offset: 0x00005B18
	public static uint $ConstGCArrayBound$0xb2c543d0$80$;

	// Token: 0x0400084B RID: 2123 RVA: 0x00006744 File Offset: 0x00005B44
	public static uint $ConstGCArrayBound$0xb2c543d0$69$;

	// Token: 0x0400084C RID: 2124 RVA: 0x00006768 File Offset: 0x00005B68
	public static uint $ConstGCArrayBound$0xb2c543d0$60$;

	// Token: 0x0400084D RID: 2125 RVA: 0x000067E4 File Offset: 0x00005BE4
	public static uint $ConstGCArrayBound$0xb2c543d0$29$;

	// Token: 0x0400084E RID: 2126 RVA: 0x000066E0 File Offset: 0x00005AE0
	public static uint $ConstGCArrayBound$0xb2c543d0$94$;

	// Token: 0x0400084F RID: 2127 RVA: 0x00006708 File Offset: 0x00005B08
	public static uint $ConstGCArrayBound$0xb2c543d0$84$;

	// Token: 0x04000850 RID: 2128 RVA: 0x00006C7C File Offset: 0x0000607C
	public static uint $ConstGCArrayBound$0x5a47e36f$65$;

	// Token: 0x04000851 RID: 2129 RVA: 0x00006C44 File Offset: 0x00006044
	public static uint $ConstGCArrayBound$0x5a47e36f$79$;

	// Token: 0x04000852 RID: 2130 RVA: 0x00006BF8 File Offset: 0x00005FF8
	public static uint $ConstGCArrayBound$0x5a47e36f$98$;

	// Token: 0x04000853 RID: 2131 RVA: 0x00006BC8 File Offset: 0x00005FC8
	public static uint $ConstGCArrayBound$0x5a47e36f$110$;

	// Token: 0x04000854 RID: 2132 RVA: 0x00006D6C File Offset: 0x0000616C
	public static uint $ConstGCArrayBound$0x5a47e36f$5$;

	// Token: 0x04000855 RID: 2133 RVA: 0x00006D64 File Offset: 0x00006164
	public static uint $ConstGCArrayBound$0x5a47e36f$7$;

	// Token: 0x04000856 RID: 2134 RVA: 0x00006C60 File Offset: 0x00006060
	public static uint $ConstGCArrayBound$0x5a47e36f$72$;

	// Token: 0x04000857 RID: 2135 RVA: 0x00006D54 File Offset: 0x00006154
	public static uint $ConstGCArrayBound$0x5a47e36f$11$;

	// Token: 0x04000858 RID: 2136 RVA: 0x00006C48 File Offset: 0x00006048
	public static uint $ConstGCArrayBound$0x5a47e36f$78$;

	// Token: 0x04000859 RID: 2137 RVA: 0x00006C78 File Offset: 0x00006078
	public static uint $ConstGCArrayBound$0x5a47e36f$66$;

	// Token: 0x0400085A RID: 2138 RVA: 0x00006CDC File Offset: 0x000060DC
	public static uint $ConstGCArrayBound$0x5a47e36f$41$;

	// Token: 0x0400085B RID: 2139 RVA: 0x00006C18 File Offset: 0x00006018
	public static uint $ConstGCArrayBound$0x5a47e36f$90$;

	// Token: 0x0400085C RID: 2140 RVA: 0x00006C70 File Offset: 0x00006070
	public static uint $ConstGCArrayBound$0x5a47e36f$68$;

	// Token: 0x0400085D RID: 2141 RVA: 0x00006CA4 File Offset: 0x000060A4
	public static uint $ConstGCArrayBound$0x5a47e36f$55$;

	// Token: 0x0400085E RID: 2142 RVA: 0x00006D4C File Offset: 0x0000614C
	public static uint $ConstGCArrayBound$0x5a47e36f$13$;

	// Token: 0x0400085F RID: 2143 RVA: 0x00006D04 File Offset: 0x00006104
	public static uint $ConstGCArrayBound$0x5a47e36f$31$;

	// Token: 0x04000860 RID: 2144 RVA: 0x00006CAC File Offset: 0x000060AC
	public static uint $ConstGCArrayBound$0x5a47e36f$53$;

	// Token: 0x04000861 RID: 2145 RVA: 0x00006BC0 File Offset: 0x00005FC0
	public static uint $ConstGCArrayBound$0x5a47e36f$112$;

	// Token: 0x04000862 RID: 2146 RVA: 0x00006C34 File Offset: 0x00006034
	public static uint $ConstGCArrayBound$0x5a47e36f$83$;

	// Token: 0x04000863 RID: 2147 RVA: 0x00006BD8 File Offset: 0x00005FD8
	public static uint $ConstGCArrayBound$0x5a47e36f$106$;

	// Token: 0x04000864 RID: 2148 RVA: 0x00006D44 File Offset: 0x00006144
	public static uint $ConstGCArrayBound$0x5a47e36f$15$;

	// Token: 0x04000865 RID: 2149 RVA: 0x00006C54 File Offset: 0x00006054
	public static uint $ConstGCArrayBound$0x5a47e36f$75$;

	// Token: 0x04000866 RID: 2150 RVA: 0x00006CCC File Offset: 0x000060CC
	public static uint $ConstGCArrayBound$0x5a47e36f$45$;

	// Token: 0x04000867 RID: 2151 RVA: 0x00006BB8 File Offset: 0x00005FB8
	public static uint $ConstGCArrayBound$0x5a47e36f$114$;

	// Token: 0x04000868 RID: 2152 RVA: 0x00006C3C File Offset: 0x0000603C
	public static uint $ConstGCArrayBound$0x5a47e36f$81$;

	// Token: 0x04000869 RID: 2153 RVA: 0x00006C0C File Offset: 0x0000600C
	public static uint $ConstGCArrayBound$0x5a47e36f$93$;

	// Token: 0x0400086A RID: 2154 RVA: 0x00006CFC File Offset: 0x000060FC
	public static uint $ConstGCArrayBound$0x5a47e36f$33$;

	// Token: 0x0400086B RID: 2155 RVA: 0x00006B90 File Offset: 0x00005F90
	public static uint $ConstGCArrayBound$0x5a47e36f$124$;

	// Token: 0x0400086C RID: 2156 RVA: 0x00006CA8 File Offset: 0x000060A8
	public static uint $ConstGCArrayBound$0x5a47e36f$54$;

	// Token: 0x0400086D RID: 2157 RVA: 0x00006D10 File Offset: 0x00006110
	public static uint $ConstGCArrayBound$0x5a47e36f$28$;

	// Token: 0x0400086E RID: 2158 RVA: 0x00006BFC File Offset: 0x00005FFC
	public static uint $ConstGCArrayBound$0x5a47e36f$97$;

	// Token: 0x0400086F RID: 2159 RVA: 0x00006BE4 File Offset: 0x00005FE4
	public static uint $ConstGCArrayBound$0x5a47e36f$103$;

	// Token: 0x04000870 RID: 2160 RVA: 0x00006D5C File Offset: 0x0000615C
	public static uint $ConstGCArrayBound$0x5a47e36f$9$;

	// Token: 0x04000871 RID: 2161 RVA: 0x00006CD0 File Offset: 0x000060D0
	public static uint $ConstGCArrayBound$0x5a47e36f$44$;

	// Token: 0x04000872 RID: 2162 RVA: 0x00006C74 File Offset: 0x00006074
	public static uint $ConstGCArrayBound$0x5a47e36f$67$;

	// Token: 0x04000873 RID: 2163 RVA: 0x00006D2C File Offset: 0x0000612C
	public static uint $ConstGCArrayBound$0x5a47e36f$21$;

	// Token: 0x04000874 RID: 2164 RVA: 0x00006BB0 File Offset: 0x00005FB0
	public static uint $ConstGCArrayBound$0x5a47e36f$116$;

	// Token: 0x04000875 RID: 2165 RVA: 0x00006C64 File Offset: 0x00006064
	public static uint $ConstGCArrayBound$0x5a47e36f$71$;

	// Token: 0x04000876 RID: 2166 RVA: 0x00006CBC File Offset: 0x000060BC
	public static uint $ConstGCArrayBound$0x5a47e36f$49$;

	// Token: 0x04000877 RID: 2167 RVA: 0x00006C4C File Offset: 0x0000604C
	public static uint $ConstGCArrayBound$0x5a47e36f$77$;

	// Token: 0x04000878 RID: 2168 RVA: 0x00006B9C File Offset: 0x00005F9C
	public static uint $ConstGCArrayBound$0x5a47e36f$121$;

	// Token: 0x04000879 RID: 2169 RVA: 0x00006B98 File Offset: 0x00005F98
	public static uint $ConstGCArrayBound$0x5a47e36f$122$;

	// Token: 0x0400087A RID: 2170 RVA: 0x00006BA4 File Offset: 0x00005FA4
	public static uint $ConstGCArrayBound$0x5a47e36f$119$;

	// Token: 0x0400087B RID: 2171 RVA: 0x00006C68 File Offset: 0x00006068
	public static uint $ConstGCArrayBound$0x5a47e36f$70$;

	// Token: 0x0400087C RID: 2172 RVA: 0x00006B8C File Offset: 0x00005F8C
	public static uint $ConstGCArrayBound$0x5a47e36f$125$;

	// Token: 0x0400087D RID: 2173 RVA: 0x00006CE4 File Offset: 0x000060E4
	public static uint $ConstGCArrayBound$0x5a47e36f$39$;

	// Token: 0x0400087E RID: 2174 RVA: 0x00006D50 File Offset: 0x00006150
	public static uint $ConstGCArrayBound$0x5a47e36f$12$;

	// Token: 0x0400087F RID: 2175 RVA: 0x00006D78 File Offset: 0x00006178
	public static uint $ConstGCArrayBound$0x5a47e36f$2$;

	// Token: 0x04000880 RID: 2176 RVA: 0x00006D68 File Offset: 0x00006168
	public static uint $ConstGCArrayBound$0x5a47e36f$6$;

	// Token: 0x04000881 RID: 2177 RVA: 0x00006C14 File Offset: 0x00006014
	public static uint $ConstGCArrayBound$0x5a47e36f$91$;

	// Token: 0x04000882 RID: 2178 RVA: 0x00006C88 File Offset: 0x00006088
	public static uint $ConstGCArrayBound$0x5a47e36f$62$;

	// Token: 0x04000883 RID: 2179 RVA: 0x00006C94 File Offset: 0x00006094
	public static uint $ConstGCArrayBound$0x5a47e36f$59$;

	// Token: 0x04000884 RID: 2180 RVA: 0x00006D28 File Offset: 0x00006128
	public static uint $ConstGCArrayBound$0x5a47e36f$22$;

	// Token: 0x04000885 RID: 2181 RVA: 0x00006CC4 File Offset: 0x000060C4
	public static uint $ConstGCArrayBound$0x5a47e36f$47$;

	// Token: 0x04000886 RID: 2182 RVA: 0x00006BB4 File Offset: 0x00005FB4
	public static uint $ConstGCArrayBound$0x5a47e36f$115$;

	// Token: 0x04000887 RID: 2183 RVA: 0x00006D0C File Offset: 0x0000610C
	public static uint $ConstGCArrayBound$0x5a47e36f$29$;

	// Token: 0x04000888 RID: 2184 RVA: 0x00006B80 File Offset: 0x00005F80
	public static uint $ConstGCArrayBound$0x5a47e36f$128$;

	// Token: 0x04000889 RID: 2185 RVA: 0x00006C08 File Offset: 0x00006008
	public static uint $ConstGCArrayBound$0x5a47e36f$94$;

	// Token: 0x0400088A RID: 2186 RVA: 0x00006D18 File Offset: 0x00006118
	public static uint $ConstGCArrayBound$0x5a47e36f$26$;

	// Token: 0x0400088B RID: 2187 RVA: 0x00006CEC File Offset: 0x000060EC
	public static uint $ConstGCArrayBound$0x5a47e36f$37$;

	// Token: 0x0400088C RID: 2188 RVA: 0x00006C5C File Offset: 0x0000605C
	public static uint $ConstGCArrayBound$0x5a47e36f$73$;

	// Token: 0x0400088D RID: 2189 RVA: 0x00006D58 File Offset: 0x00006158
	public static uint $ConstGCArrayBound$0x5a47e36f$10$;

	// Token: 0x0400088E RID: 2190 RVA: 0x00006C30 File Offset: 0x00006030
	public static uint $ConstGCArrayBound$0x5a47e36f$84$;

	// Token: 0x0400088F RID: 2191 RVA: 0x00006BD4 File Offset: 0x00005FD4
	public static uint $ConstGCArrayBound$0x5a47e36f$107$;

	// Token: 0x04000890 RID: 2192 RVA: 0x00006CC8 File Offset: 0x000060C8
	public static uint $ConstGCArrayBound$0x5a47e36f$46$;

	// Token: 0x04000891 RID: 2193 RVA: 0x00006D1C File Offset: 0x0000611C
	public static uint $ConstGCArrayBound$0x5a47e36f$25$;

	// Token: 0x04000892 RID: 2194 RVA: 0x00006BE8 File Offset: 0x00005FE8
	public static uint $ConstGCArrayBound$0x5a47e36f$102$;

	// Token: 0x04000893 RID: 2195 RVA: 0x00006BE0 File Offset: 0x00005FE0
	public static uint $ConstGCArrayBound$0x5a47e36f$104$;

	// Token: 0x04000894 RID: 2196 RVA: 0x00006BAC File Offset: 0x00005FAC
	public static uint $ConstGCArrayBound$0x5a47e36f$117$;

	// Token: 0x04000895 RID: 2197 RVA: 0x00006D7C File Offset: 0x0000617C
	public static uint $ConstGCArrayBound$0x5a47e36f$1$;

	// Token: 0x04000896 RID: 2198 RVA: 0x00006D24 File Offset: 0x00006124
	public static uint $ConstGCArrayBound$0x5a47e36f$23$;

	// Token: 0x04000897 RID: 2199 RVA: 0x00006CD4 File Offset: 0x000060D4
	public static uint $ConstGCArrayBound$0x5a47e36f$43$;

	// Token: 0x04000898 RID: 2200 RVA: 0x00006C28 File Offset: 0x00006028
	public static uint $ConstGCArrayBound$0x5a47e36f$86$;

	// Token: 0x04000899 RID: 2201 RVA: 0x00006D34 File Offset: 0x00006134
	public static uint $ConstGCArrayBound$0x5a47e36f$19$;

	// Token: 0x0400089A RID: 2202 RVA: 0x00006C6C File Offset: 0x0000606C
	public static uint $ConstGCArrayBound$0x5a47e36f$69$;

	// Token: 0x0400089B RID: 2203 RVA: 0x00006BEC File Offset: 0x00005FEC
	public static uint $ConstGCArrayBound$0x5a47e36f$101$;

	// Token: 0x0400089C RID: 2204 RVA: 0x00006CB8 File Offset: 0x000060B8
	public static uint $ConstGCArrayBound$0x5a47e36f$50$;

	// Token: 0x0400089D RID: 2205 RVA: 0x00006C10 File Offset: 0x00006010
	public static uint $ConstGCArrayBound$0x5a47e36f$92$;

	// Token: 0x0400089E RID: 2206 RVA: 0x00006D40 File Offset: 0x00006140
	public static uint $ConstGCArrayBound$0x5a47e36f$16$;

	// Token: 0x0400089F RID: 2207 RVA: 0x00006CF4 File Offset: 0x000060F4
	public static uint $ConstGCArrayBound$0x5a47e36f$35$;

	// Token: 0x040008A0 RID: 2208 RVA: 0x00006CE0 File Offset: 0x000060E0
	public static uint $ConstGCArrayBound$0x5a47e36f$40$;

	// Token: 0x040008A1 RID: 2209 RVA: 0x00006B84 File Offset: 0x00005F84
	public static uint $ConstGCArrayBound$0x5a47e36f$127$;

	// Token: 0x040008A2 RID: 2210 RVA: 0x00006D08 File Offset: 0x00006108
	public static uint $ConstGCArrayBound$0x5a47e36f$30$;

	// Token: 0x040008A3 RID: 2211 RVA: 0x00006C2C File Offset: 0x0000602C
	public static uint $ConstGCArrayBound$0x5a47e36f$85$;

	// Token: 0x040008A4 RID: 2212 RVA: 0x00006BCC File Offset: 0x00005FCC
	public static uint $ConstGCArrayBound$0x5a47e36f$109$;

	// Token: 0x040008A5 RID: 2213 RVA: 0x00006CB4 File Offset: 0x000060B4
	public static uint $ConstGCArrayBound$0x5a47e36f$51$;

	// Token: 0x040008A6 RID: 2214 RVA: 0x00006CE8 File Offset: 0x000060E8
	public static uint $ConstGCArrayBound$0x5a47e36f$38$;

	// Token: 0x040008A7 RID: 2215 RVA: 0x00006BA0 File Offset: 0x00005FA0
	public static uint $ConstGCArrayBound$0x5a47e36f$120$;

	// Token: 0x040008A8 RID: 2216 RVA: 0x00006CD8 File Offset: 0x000060D8
	public static uint $ConstGCArrayBound$0x5a47e36f$42$;

	// Token: 0x040008A9 RID: 2217 RVA: 0x00006D60 File Offset: 0x00006160
	public static uint $ConstGCArrayBound$0x5a47e36f$8$;

	// Token: 0x040008AA RID: 2218 RVA: 0x00006D14 File Offset: 0x00006114
	public static uint $ConstGCArrayBound$0x5a47e36f$27$;

	// Token: 0x040008AB RID: 2219 RVA: 0x00006C38 File Offset: 0x00006038
	public static uint $ConstGCArrayBound$0x5a47e36f$82$;

	// Token: 0x040008AC RID: 2220 RVA: 0x00006C24 File Offset: 0x00006024
	public static uint $ConstGCArrayBound$0x5a47e36f$87$;

	// Token: 0x040008AD RID: 2221 RVA: 0x00006C58 File Offset: 0x00006058
	public static uint $ConstGCArrayBound$0x5a47e36f$74$;

	// Token: 0x040008AE RID: 2222 RVA: 0x00006CF0 File Offset: 0x000060F0
	public static uint $ConstGCArrayBound$0x5a47e36f$36$;

	// Token: 0x040008AF RID: 2223 RVA: 0x00006CB0 File Offset: 0x000060B0
	public static uint $ConstGCArrayBound$0x5a47e36f$52$;

	// Token: 0x040008B0 RID: 2224 RVA: 0x00006BBC File Offset: 0x00005FBC
	public static uint $ConstGCArrayBound$0x5a47e36f$113$;

	// Token: 0x040008B1 RID: 2225 RVA: 0x00006C04 File Offset: 0x00006004
	public static uint $ConstGCArrayBound$0x5a47e36f$95$;

	// Token: 0x040008B2 RID: 2226 RVA: 0x00006D70 File Offset: 0x00006170
	public static uint $ConstGCArrayBound$0x5a47e36f$4$;

	// Token: 0x040008B3 RID: 2227 RVA: 0x00006C40 File Offset: 0x00006040
	public static uint $ConstGCArrayBound$0x5a47e36f$80$;

	// Token: 0x040008B4 RID: 2228 RVA: 0x00006BD0 File Offset: 0x00005FD0
	public static uint $ConstGCArrayBound$0x5a47e36f$108$;

	// Token: 0x040008B5 RID: 2229 RVA: 0x00006D30 File Offset: 0x00006130
	public static uint $ConstGCArrayBound$0x5a47e36f$20$;

	// Token: 0x040008B6 RID: 2230 RVA: 0x00006C9C File Offset: 0x0000609C
	public static uint $ConstGCArrayBound$0x5a47e36f$57$;

	// Token: 0x040008B7 RID: 2231 RVA: 0x00006C90 File Offset: 0x00006090
	public static uint $ConstGCArrayBound$0x5a47e36f$60$;

	// Token: 0x040008B8 RID: 2232 RVA: 0x00006C1C File Offset: 0x0000601C
	public static uint $ConstGCArrayBound$0x5a47e36f$89$;

	// Token: 0x040008B9 RID: 2233 RVA: 0x00006BF4 File Offset: 0x00005FF4
	public static uint $ConstGCArrayBound$0x5a47e36f$99$;

	// Token: 0x040008BA RID: 2234 RVA: 0x00006D48 File Offset: 0x00006148
	public static uint $ConstGCArrayBound$0x5a47e36f$14$;

	// Token: 0x040008BB RID: 2235 RVA: 0x00006C50 File Offset: 0x00006050
	public static uint $ConstGCArrayBound$0x5a47e36f$76$;

	// Token: 0x040008BC RID: 2236 RVA: 0x00006B88 File Offset: 0x00005F88
	public static uint $ConstGCArrayBound$0x5a47e36f$126$;

	// Token: 0x040008BD RID: 2237 RVA: 0x00006CC0 File Offset: 0x000060C0
	public static uint $ConstGCArrayBound$0x5a47e36f$48$;

	// Token: 0x040008BE RID: 2238 RVA: 0x00006D74 File Offset: 0x00006174
	public static uint $ConstGCArrayBound$0x5a47e36f$3$;

	// Token: 0x040008BF RID: 2239 RVA: 0x00006BDC File Offset: 0x00005FDC
	public static uint $ConstGCArrayBound$0x5a47e36f$105$;

	// Token: 0x040008C0 RID: 2240 RVA: 0x00006BF0 File Offset: 0x00005FF0
	public static uint $ConstGCArrayBound$0x5a47e36f$100$;

	// Token: 0x040008C1 RID: 2241 RVA: 0x00006CF8 File Offset: 0x000060F8
	public static uint $ConstGCArrayBound$0x5a47e36f$34$;

	// Token: 0x040008C2 RID: 2242 RVA: 0x00006C98 File Offset: 0x00006098
	public static uint $ConstGCArrayBound$0x5a47e36f$58$;

	// Token: 0x040008C3 RID: 2243 RVA: 0x00006BA8 File Offset: 0x00005FA8
	public static uint $ConstGCArrayBound$0x5a47e36f$118$;

	// Token: 0x040008C4 RID: 2244 RVA: 0x00006C84 File Offset: 0x00006084
	public static uint $ConstGCArrayBound$0x5a47e36f$63$;

	// Token: 0x040008C5 RID: 2245 RVA: 0x00006D00 File Offset: 0x00006100
	public static uint $ConstGCArrayBound$0x5a47e36f$32$;

	// Token: 0x040008C6 RID: 2246 RVA: 0x00006C20 File Offset: 0x00006020
	public static uint $ConstGCArrayBound$0x5a47e36f$88$;

	// Token: 0x040008C7 RID: 2247 RVA: 0x00006B94 File Offset: 0x00005F94
	public static uint $ConstGCArrayBound$0x5a47e36f$123$;

	// Token: 0x040008C8 RID: 2248 RVA: 0x00006CA0 File Offset: 0x000060A0
	public static uint $ConstGCArrayBound$0x5a47e36f$56$;

	// Token: 0x040008C9 RID: 2249 RVA: 0x00006BC4 File Offset: 0x00005FC4
	public static uint $ConstGCArrayBound$0x5a47e36f$111$;

	// Token: 0x040008CA RID: 2250 RVA: 0x00006C00 File Offset: 0x00006000
	public static uint $ConstGCArrayBound$0x5a47e36f$96$;

	// Token: 0x040008CB RID: 2251 RVA: 0x00006D3C File Offset: 0x0000613C
	public static uint $ConstGCArrayBound$0x5a47e36f$17$;

	// Token: 0x040008CC RID: 2252 RVA: 0x00006C80 File Offset: 0x00006080
	public static uint $ConstGCArrayBound$0x5a47e36f$64$;

	// Token: 0x040008CD RID: 2253 RVA: 0x00006D20 File Offset: 0x00006120
	public static uint $ConstGCArrayBound$0x5a47e36f$24$;

	// Token: 0x040008CE RID: 2254 RVA: 0x00006C8C File Offset: 0x0000608C
	public static uint $ConstGCArrayBound$0x5a47e36f$61$;

	// Token: 0x040008CF RID: 2255 RVA: 0x00006D38 File Offset: 0x00006138
	public static uint $ConstGCArrayBound$0x5a47e36f$18$;

	// Token: 0x040008D0 RID: 2256 RVA: 0x000071A0 File Offset: 0x000065A0
	public static uint $ConstGCArrayBound$0x1a191e05$66$;

	// Token: 0x040008D1 RID: 2257 RVA: 0x000070E8 File Offset: 0x000064E8
	public static uint $ConstGCArrayBound$0x1a191e05$112$;

	// Token: 0x040008D2 RID: 2258 RVA: 0x00007110 File Offset: 0x00006510
	public static uint $ConstGCArrayBound$0x1a191e05$102$;

	// Token: 0x040008D3 RID: 2259 RVA: 0x00007258 File Offset: 0x00006658
	public static uint $ConstGCArrayBound$0x1a191e05$20$;

	// Token: 0x040008D4 RID: 2260 RVA: 0x00007158 File Offset: 0x00006558
	public static uint $ConstGCArrayBound$0x1a191e05$84$;

	// Token: 0x040008D5 RID: 2261 RVA: 0x00007114 File Offset: 0x00006514
	public static uint $ConstGCArrayBound$0x1a191e05$101$;

	// Token: 0x040008D6 RID: 2262 RVA: 0x000070D8 File Offset: 0x000064D8
	public static uint $ConstGCArrayBound$0x1a191e05$116$;

	// Token: 0x040008D7 RID: 2263 RVA: 0x00007160 File Offset: 0x00006560
	public static uint $ConstGCArrayBound$0x1a191e05$82$;

	// Token: 0x040008D8 RID: 2264 RVA: 0x0000714C File Offset: 0x0000654C
	public static uint $ConstGCArrayBound$0x1a191e05$87$;

	// Token: 0x040008D9 RID: 2265 RVA: 0x000070F4 File Offset: 0x000064F4
	public static uint $ConstGCArrayBound$0x1a191e05$109$;

	// Token: 0x040008DA RID: 2266 RVA: 0x00007190 File Offset: 0x00006590
	public static uint $ConstGCArrayBound$0x1a191e05$70$;

	// Token: 0x040008DB RID: 2267 RVA: 0x00007164 File Offset: 0x00006564
	public static uint $ConstGCArrayBound$0x1a191e05$81$;

	// Token: 0x040008DC RID: 2268 RVA: 0x000071E8 File Offset: 0x000065E8
	public static uint $ConstGCArrayBound$0x1a191e05$48$;

	// Token: 0x040008DD RID: 2269 RVA: 0x00007260 File Offset: 0x00006660
	public static uint $ConstGCArrayBound$0x1a191e05$18$;

	// Token: 0x040008DE RID: 2270 RVA: 0x00007108 File Offset: 0x00006508
	public static uint $ConstGCArrayBound$0x1a191e05$104$;

	// Token: 0x040008DF RID: 2271 RVA: 0x000071D8 File Offset: 0x000065D8
	public static uint $ConstGCArrayBound$0x1a191e05$52$;

	// Token: 0x040008E0 RID: 2272 RVA: 0x000071E4 File Offset: 0x000065E4
	public static uint $ConstGCArrayBound$0x1a191e05$49$;

	// Token: 0x040008E1 RID: 2273 RVA: 0x00007264 File Offset: 0x00006664
	public static uint $ConstGCArrayBound$0x1a191e05$17$;

	// Token: 0x040008E2 RID: 2274 RVA: 0x00007198 File Offset: 0x00006598
	public static uint $ConstGCArrayBound$0x1a191e05$68$;

	// Token: 0x040008E3 RID: 2275 RVA: 0x00007170 File Offset: 0x00006570
	public static uint $ConstGCArrayBound$0x1a191e05$78$;

	// Token: 0x040008E4 RID: 2276 RVA: 0x0000716C File Offset: 0x0000656C
	public static uint $ConstGCArrayBound$0x1a191e05$79$;

	// Token: 0x040008E5 RID: 2277 RVA: 0x0000726C File Offset: 0x0000666C
	public static uint $ConstGCArrayBound$0x1a191e05$15$;

	// Token: 0x040008E6 RID: 2278 RVA: 0x00007100 File Offset: 0x00006500
	public static uint $ConstGCArrayBound$0x1a191e05$106$;

	// Token: 0x040008E7 RID: 2279 RVA: 0x0000723C File Offset: 0x0000663C
	public static uint $ConstGCArrayBound$0x1a191e05$27$;

	// Token: 0x040008E8 RID: 2280 RVA: 0x00007234 File Offset: 0x00006634
	public static uint $ConstGCArrayBound$0x1a191e05$29$;

	// Token: 0x040008E9 RID: 2281 RVA: 0x00007104 File Offset: 0x00006504
	public static uint $ConstGCArrayBound$0x1a191e05$105$;

	// Token: 0x040008EA RID: 2282 RVA: 0x00007174 File Offset: 0x00006574
	public static uint $ConstGCArrayBound$0x1a191e05$77$;

	// Token: 0x040008EB RID: 2283 RVA: 0x00007228 File Offset: 0x00006628
	public static uint $ConstGCArrayBound$0x1a191e05$32$;

	// Token: 0x040008EC RID: 2284 RVA: 0x00007280 File Offset: 0x00006680
	public static uint $ConstGCArrayBound$0x1a191e05$10$;

	// Token: 0x040008ED RID: 2285 RVA: 0x00007298 File Offset: 0x00006698
	public static uint $ConstGCArrayBound$0x1a191e05$4$;

	// Token: 0x040008EE RID: 2286 RVA: 0x00007144 File Offset: 0x00006544
	public static uint $ConstGCArrayBound$0x1a191e05$89$;

	// Token: 0x040008EF RID: 2287 RVA: 0x0000713C File Offset: 0x0000653C
	public static uint $ConstGCArrayBound$0x1a191e05$91$;

	// Token: 0x040008F0 RID: 2288 RVA: 0x000071F8 File Offset: 0x000065F8
	public static uint $ConstGCArrayBound$0x1a191e05$44$;

	// Token: 0x040008F1 RID: 2289 RVA: 0x0000727C File Offset: 0x0000667C
	public static uint $ConstGCArrayBound$0x1a191e05$11$;

	// Token: 0x040008F2 RID: 2290 RVA: 0x000070C0 File Offset: 0x000064C0
	public static uint $ConstGCArrayBound$0x1a191e05$122$;

	// Token: 0x040008F3 RID: 2291 RVA: 0x000071A8 File Offset: 0x000065A8
	public static uint $ConstGCArrayBound$0x1a191e05$64$;

	// Token: 0x040008F4 RID: 2292 RVA: 0x0000717C File Offset: 0x0000657C
	public static uint $ConstGCArrayBound$0x1a191e05$75$;

	// Token: 0x040008F5 RID: 2293 RVA: 0x0000711C File Offset: 0x0000651C
	public static uint $ConstGCArrayBound$0x1a191e05$99$;

	// Token: 0x040008F6 RID: 2294 RVA: 0x00007278 File Offset: 0x00006678
	public static uint $ConstGCArrayBound$0x1a191e05$12$;

	// Token: 0x040008F7 RID: 2295 RVA: 0x000071E0 File Offset: 0x000065E0
	public static uint $ConstGCArrayBound$0x1a191e05$50$;

	// Token: 0x040008F8 RID: 2296 RVA: 0x00007288 File Offset: 0x00006688
	public static uint $ConstGCArrayBound$0x1a191e05$8$;

	// Token: 0x040008F9 RID: 2297 RVA: 0x000070FC File Offset: 0x000064FC
	public static uint $ConstGCArrayBound$0x1a191e05$107$;

	// Token: 0x040008FA RID: 2298 RVA: 0x000070B0 File Offset: 0x000064B0
	public static uint $ConstGCArrayBound$0x1a191e05$126$;

	// Token: 0x040008FB RID: 2299 RVA: 0x000070EC File Offset: 0x000064EC
	public static uint $ConstGCArrayBound$0x1a191e05$111$;

	// Token: 0x040008FC RID: 2300 RVA: 0x00007130 File Offset: 0x00006530
	public static uint $ConstGCArrayBound$0x1a191e05$94$;

	// Token: 0x040008FD RID: 2301 RVA: 0x00007274 File Offset: 0x00006674
	public static uint $ConstGCArrayBound$0x1a191e05$13$;

	// Token: 0x040008FE RID: 2302 RVA: 0x00007244 File Offset: 0x00006644
	public static uint $ConstGCArrayBound$0x1a191e05$25$;

	// Token: 0x040008FF RID: 2303 RVA: 0x00007168 File Offset: 0x00006568
	public static uint $ConstGCArrayBound$0x1a191e05$80$;

	// Token: 0x04000900 RID: 2304 RVA: 0x000071FC File Offset: 0x000065FC
	public static uint $ConstGCArrayBound$0x1a191e05$43$;

	// Token: 0x04000901 RID: 2305 RVA: 0x00007120 File Offset: 0x00006520
	public static uint $ConstGCArrayBound$0x1a191e05$98$;

	// Token: 0x04000902 RID: 2306 RVA: 0x000071C8 File Offset: 0x000065C8
	public static uint $ConstGCArrayBound$0x1a191e05$56$;

	// Token: 0x04000903 RID: 2307 RVA: 0x000070B8 File Offset: 0x000064B8
	public static uint $ConstGCArrayBound$0x1a191e05$124$;

	// Token: 0x04000904 RID: 2308 RVA: 0x0000715C File Offset: 0x0000655C
	public static uint $ConstGCArrayBound$0x1a191e05$83$;

	// Token: 0x04000905 RID: 2309 RVA: 0x00007138 File Offset: 0x00006538
	public static uint $ConstGCArrayBound$0x1a191e05$92$;

	// Token: 0x04000906 RID: 2310 RVA: 0x00007180 File Offset: 0x00006580
	public static uint $ConstGCArrayBound$0x1a191e05$74$;

	// Token: 0x04000907 RID: 2311 RVA: 0x00007284 File Offset: 0x00006684
	public static uint $ConstGCArrayBound$0x1a191e05$9$;

	// Token: 0x04000908 RID: 2312 RVA: 0x000070AC File Offset: 0x000064AC
	public static uint $ConstGCArrayBound$0x1a191e05$127$;

	// Token: 0x04000909 RID: 2313 RVA: 0x000071D4 File Offset: 0x000065D4
	public static uint $ConstGCArrayBound$0x1a191e05$53$;

	// Token: 0x0400090A RID: 2314 RVA: 0x000070D0 File Offset: 0x000064D0
	public static uint $ConstGCArrayBound$0x1a191e05$118$;

	// Token: 0x0400090B RID: 2315 RVA: 0x000071B4 File Offset: 0x000065B4
	public static uint $ConstGCArrayBound$0x1a191e05$61$;

	// Token: 0x0400090C RID: 2316 RVA: 0x000070E0 File Offset: 0x000064E0
	public static uint $ConstGCArrayBound$0x1a191e05$114$;

	// Token: 0x0400090D RID: 2317 RVA: 0x00007214 File Offset: 0x00006614
	public static uint $ConstGCArrayBound$0x1a191e05$37$;

	// Token: 0x0400090E RID: 2318 RVA: 0x000070BC File Offset: 0x000064BC
	public static uint $ConstGCArrayBound$0x1a191e05$123$;

	// Token: 0x0400090F RID: 2319 RVA: 0x000071AC File Offset: 0x000065AC
	public static uint $ConstGCArrayBound$0x1a191e05$63$;

	// Token: 0x04000910 RID: 2320 RVA: 0x000070C8 File Offset: 0x000064C8
	public static uint $ConstGCArrayBound$0x1a191e05$120$;

	// Token: 0x04000911 RID: 2321 RVA: 0x0000712C File Offset: 0x0000652C
	public static uint $ConstGCArrayBound$0x1a191e05$95$;

	// Token: 0x04000912 RID: 2322 RVA: 0x00007218 File Offset: 0x00006618
	public static uint $ConstGCArrayBound$0x1a191e05$36$;

	// Token: 0x04000913 RID: 2323 RVA: 0x00007224 File Offset: 0x00006624
	public static uint $ConstGCArrayBound$0x1a191e05$33$;

	// Token: 0x04000914 RID: 2324 RVA: 0x00007250 File Offset: 0x00006650
	public static uint $ConstGCArrayBound$0x1a191e05$22$;

	// Token: 0x04000915 RID: 2325 RVA: 0x00007184 File Offset: 0x00006584
	public static uint $ConstGCArrayBound$0x1a191e05$73$;

	// Token: 0x04000916 RID: 2326 RVA: 0x000071B0 File Offset: 0x000065B0
	public static uint $ConstGCArrayBound$0x1a191e05$62$;

	// Token: 0x04000917 RID: 2327 RVA: 0x0000720C File Offset: 0x0000660C
	public static uint $ConstGCArrayBound$0x1a191e05$39$;

	// Token: 0x04000918 RID: 2328 RVA: 0x00007178 File Offset: 0x00006578
	public static uint $ConstGCArrayBound$0x1a191e05$76$;

	// Token: 0x04000919 RID: 2329 RVA: 0x000071F0 File Offset: 0x000065F0
	public static uint $ConstGCArrayBound$0x1a191e05$46$;

	// Token: 0x0400091A RID: 2330 RVA: 0x00007254 File Offset: 0x00006654
	public static uint $ConstGCArrayBound$0x1a191e05$21$;

	// Token: 0x0400091B RID: 2331 RVA: 0x00007238 File Offset: 0x00006638
	public static uint $ConstGCArrayBound$0x1a191e05$28$;

	// Token: 0x0400091C RID: 2332 RVA: 0x00007148 File Offset: 0x00006548
	public static uint $ConstGCArrayBound$0x1a191e05$88$;

	// Token: 0x0400091D RID: 2333 RVA: 0x00007268 File Offset: 0x00006668
	public static uint $ConstGCArrayBound$0x1a191e05$16$;

	// Token: 0x0400091E RID: 2334 RVA: 0x000070C4 File Offset: 0x000064C4
	public static uint $ConstGCArrayBound$0x1a191e05$121$;

	// Token: 0x0400091F RID: 2335 RVA: 0x000071F4 File Offset: 0x000065F4
	public static uint $ConstGCArrayBound$0x1a191e05$45$;

	// Token: 0x04000920 RID: 2336 RVA: 0x000071D0 File Offset: 0x000065D0
	public static uint $ConstGCArrayBound$0x1a191e05$54$;

	// Token: 0x04000921 RID: 2337 RVA: 0x000070E4 File Offset: 0x000064E4
	public static uint $ConstGCArrayBound$0x1a191e05$113$;

	// Token: 0x04000922 RID: 2338 RVA: 0x0000729C File Offset: 0x0000669C
	public static uint $ConstGCArrayBound$0x1a191e05$3$;

	// Token: 0x04000923 RID: 2339 RVA: 0x00007240 File Offset: 0x00006640
	public static uint $ConstGCArrayBound$0x1a191e05$26$;

	// Token: 0x04000924 RID: 2340 RVA: 0x00007150 File Offset: 0x00006550
	public static uint $ConstGCArrayBound$0x1a191e05$86$;

	// Token: 0x04000925 RID: 2341 RVA: 0x00007140 File Offset: 0x00006540
	public static uint $ConstGCArrayBound$0x1a191e05$90$;

	// Token: 0x04000926 RID: 2342 RVA: 0x000070D4 File Offset: 0x000064D4
	public static uint $ConstGCArrayBound$0x1a191e05$117$;

	// Token: 0x04000927 RID: 2343 RVA: 0x0000721C File Offset: 0x0000661C
	public static uint $ConstGCArrayBound$0x1a191e05$35$;

	// Token: 0x04000928 RID: 2344 RVA: 0x00007188 File Offset: 0x00006588
	public static uint $ConstGCArrayBound$0x1a191e05$72$;

	// Token: 0x04000929 RID: 2345 RVA: 0x00007154 File Offset: 0x00006554
	public static uint $ConstGCArrayBound$0x1a191e05$85$;

	// Token: 0x0400092A RID: 2346 RVA: 0x000071B8 File Offset: 0x000065B8
	public static uint $ConstGCArrayBound$0x1a191e05$60$;

	// Token: 0x0400092B RID: 2347 RVA: 0x00007134 File Offset: 0x00006534
	public static uint $ConstGCArrayBound$0x1a191e05$93$;

	// Token: 0x0400092C RID: 2348 RVA: 0x00007248 File Offset: 0x00006648
	public static uint $ConstGCArrayBound$0x1a191e05$24$;

	// Token: 0x0400092D RID: 2349 RVA: 0x00007294 File Offset: 0x00006694
	public static uint $ConstGCArrayBound$0x1a191e05$5$;

	// Token: 0x0400092E RID: 2350 RVA: 0x000071A4 File Offset: 0x000065A4
	public static uint $ConstGCArrayBound$0x1a191e05$65$;

	// Token: 0x0400092F RID: 2351 RVA: 0x000071EC File Offset: 0x000065EC
	public static uint $ConstGCArrayBound$0x1a191e05$47$;

	// Token: 0x04000930 RID: 2352 RVA: 0x00007200 File Offset: 0x00006600
	public static uint $ConstGCArrayBound$0x1a191e05$42$;

	// Token: 0x04000931 RID: 2353 RVA: 0x00007220 File Offset: 0x00006620
	public static uint $ConstGCArrayBound$0x1a191e05$34$;

	// Token: 0x04000932 RID: 2354 RVA: 0x0000724C File Offset: 0x0000664C
	public static uint $ConstGCArrayBound$0x1a191e05$23$;

	// Token: 0x04000933 RID: 2355 RVA: 0x0000725C File Offset: 0x0000665C
	public static uint $ConstGCArrayBound$0x1a191e05$19$;

	// Token: 0x04000934 RID: 2356 RVA: 0x000071C4 File Offset: 0x000065C4
	public static uint $ConstGCArrayBound$0x1a191e05$57$;

	// Token: 0x04000935 RID: 2357 RVA: 0x000070B4 File Offset: 0x000064B4
	public static uint $ConstGCArrayBound$0x1a191e05$125$;

	// Token: 0x04000936 RID: 2358 RVA: 0x00007128 File Offset: 0x00006528
	public static uint $ConstGCArrayBound$0x1a191e05$96$;

	// Token: 0x04000937 RID: 2359 RVA: 0x0000728C File Offset: 0x0000668C
	public static uint $ConstGCArrayBound$0x1a191e05$7$;

	// Token: 0x04000938 RID: 2360 RVA: 0x000070CC File Offset: 0x000064CC
	public static uint $ConstGCArrayBound$0x1a191e05$119$;

	// Token: 0x04000939 RID: 2361 RVA: 0x00007270 File Offset: 0x00006670
	public static uint $ConstGCArrayBound$0x1a191e05$14$;

	// Token: 0x0400093A RID: 2362 RVA: 0x000071DC File Offset: 0x000065DC
	public static uint $ConstGCArrayBound$0x1a191e05$51$;

	// Token: 0x0400093B RID: 2363 RVA: 0x000071BC File Offset: 0x000065BC
	public static uint $ConstGCArrayBound$0x1a191e05$59$;

	// Token: 0x0400093C RID: 2364 RVA: 0x000070DC File Offset: 0x000064DC
	public static uint $ConstGCArrayBound$0x1a191e05$115$;

	// Token: 0x0400093D RID: 2365 RVA: 0x000072A0 File Offset: 0x000066A0
	public static uint $ConstGCArrayBound$0x1a191e05$2$;

	// Token: 0x0400093E RID: 2366 RVA: 0x00007208 File Offset: 0x00006608
	public static uint $ConstGCArrayBound$0x1a191e05$40$;

	// Token: 0x0400093F RID: 2367 RVA: 0x0000722C File Offset: 0x0000662C
	public static uint $ConstGCArrayBound$0x1a191e05$31$;

	// Token: 0x04000940 RID: 2368 RVA: 0x00007210 File Offset: 0x00006610
	public static uint $ConstGCArrayBound$0x1a191e05$38$;

	// Token: 0x04000941 RID: 2369 RVA: 0x0000710C File Offset: 0x0000650C
	public static uint $ConstGCArrayBound$0x1a191e05$103$;

	// Token: 0x04000942 RID: 2370 RVA: 0x00007124 File Offset: 0x00006524
	public static uint $ConstGCArrayBound$0x1a191e05$97$;

	// Token: 0x04000943 RID: 2371 RVA: 0x00007290 File Offset: 0x00006690
	public static uint $ConstGCArrayBound$0x1a191e05$6$;

	// Token: 0x04000944 RID: 2372 RVA: 0x0000718C File Offset: 0x0000658C
	public static uint $ConstGCArrayBound$0x1a191e05$71$;

	// Token: 0x04000945 RID: 2373 RVA: 0x000071C0 File Offset: 0x000065C0
	public static uint $ConstGCArrayBound$0x1a191e05$58$;

	// Token: 0x04000946 RID: 2374 RVA: 0x00007230 File Offset: 0x00006630
	public static uint $ConstGCArrayBound$0x1a191e05$30$;

	// Token: 0x04000947 RID: 2375 RVA: 0x00007204 File Offset: 0x00006604
	public static uint $ConstGCArrayBound$0x1a191e05$41$;

	// Token: 0x04000948 RID: 2376 RVA: 0x00007118 File Offset: 0x00006518
	public static uint $ConstGCArrayBound$0x1a191e05$100$;

	// Token: 0x04000949 RID: 2377 RVA: 0x0000719C File Offset: 0x0000659C
	public static uint $ConstGCArrayBound$0x1a191e05$67$;

	// Token: 0x0400094A RID: 2378 RVA: 0x000070F0 File Offset: 0x000064F0
	public static uint $ConstGCArrayBound$0x1a191e05$110$;

	// Token: 0x0400094B RID: 2379 RVA: 0x000070F8 File Offset: 0x000064F8
	public static uint $ConstGCArrayBound$0x1a191e05$108$;

	// Token: 0x0400094C RID: 2380 RVA: 0x000071CC File Offset: 0x000065CC
	public static uint $ConstGCArrayBound$0x1a191e05$55$;

	// Token: 0x0400094D RID: 2381 RVA: 0x000070A8 File Offset: 0x000064A8
	public static uint $ConstGCArrayBound$0x1a191e05$128$;

	// Token: 0x0400094E RID: 2382 RVA: 0x000072A4 File Offset: 0x000066A4
	public static uint $ConstGCArrayBound$0x1a191e05$1$;

	// Token: 0x0400094F RID: 2383 RVA: 0x00007194 File Offset: 0x00006594
	public static uint $ConstGCArrayBound$0x1a191e05$69$;

	// Token: 0x04000950 RID: 2384 RVA: 0x000076F4 File Offset: 0x00006AF4
	public static uint $ConstGCArrayBound$0x7c3cd6b8$56$;

	// Token: 0x04000951 RID: 2385 RVA: 0x000077CC File Offset: 0x00006BCC
	public static uint $ConstGCArrayBound$0x7c3cd6b8$2$;

	// Token: 0x04000952 RID: 2386 RVA: 0x00007768 File Offset: 0x00006B68
	public static uint $ConstGCArrayBound$0x7c3cd6b8$27$;

	// Token: 0x04000953 RID: 2387 RVA: 0x0000771C File Offset: 0x00006B1C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$46$;

	// Token: 0x04000954 RID: 2388 RVA: 0x00007688 File Offset: 0x00006A88
	public static uint $ConstGCArrayBound$0x7c3cd6b8$83$;

	// Token: 0x04000955 RID: 2389 RVA: 0x0000779C File Offset: 0x00006B9C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$14$;

	// Token: 0x04000956 RID: 2390 RVA: 0x00007608 File Offset: 0x00006A08
	public static uint $ConstGCArrayBound$0x7c3cd6b8$115$;

	// Token: 0x04000957 RID: 2391 RVA: 0x00007654 File Offset: 0x00006A54
	public static uint $ConstGCArrayBound$0x7c3cd6b8$96$;

	// Token: 0x04000958 RID: 2392 RVA: 0x000075FC File Offset: 0x000069FC
	public static uint $ConstGCArrayBound$0x7c3cd6b8$118$;

	// Token: 0x04000959 RID: 2393 RVA: 0x00007738 File Offset: 0x00006B38
	public static uint $ConstGCArrayBound$0x7c3cd6b8$39$;

	// Token: 0x0400095A RID: 2394 RVA: 0x000076D0 File Offset: 0x00006AD0
	public static uint $ConstGCArrayBound$0x7c3cd6b8$65$;

	// Token: 0x0400095B RID: 2395 RVA: 0x000076EC File Offset: 0x00006AEC
	public static uint $ConstGCArrayBound$0x7c3cd6b8$58$;

	// Token: 0x0400095C RID: 2396 RVA: 0x00007600 File Offset: 0x00006A00
	public static uint $ConstGCArrayBound$0x7c3cd6b8$117$;

	// Token: 0x0400095D RID: 2397 RVA: 0x00007650 File Offset: 0x00006A50
	public static uint $ConstGCArrayBound$0x7c3cd6b8$97$;

	// Token: 0x0400095E RID: 2398 RVA: 0x00007760 File Offset: 0x00006B60
	public static uint $ConstGCArrayBound$0x7c3cd6b8$29$;

	// Token: 0x0400095F RID: 2399 RVA: 0x00007628 File Offset: 0x00006A28
	public static uint $ConstGCArrayBound$0x7c3cd6b8$107$;

	// Token: 0x04000960 RID: 2400 RVA: 0x000076A4 File Offset: 0x00006AA4
	public static uint $ConstGCArrayBound$0x7c3cd6b8$76$;

	// Token: 0x04000961 RID: 2401 RVA: 0x0000765C File Offset: 0x00006A5C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$94$;

	// Token: 0x04000962 RID: 2402 RVA: 0x000077B8 File Offset: 0x00006BB8
	public static uint $ConstGCArrayBound$0x7c3cd6b8$7$;

	// Token: 0x04000963 RID: 2403 RVA: 0x00007630 File Offset: 0x00006A30
	public static uint $ConstGCArrayBound$0x7c3cd6b8$105$;

	// Token: 0x04000964 RID: 2404 RVA: 0x0000772C File Offset: 0x00006B2C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$42$;

	// Token: 0x04000965 RID: 2405 RVA: 0x00007614 File Offset: 0x00006A14
	public static uint $ConstGCArrayBound$0x7c3cd6b8$112$;

	// Token: 0x04000966 RID: 2406 RVA: 0x00007670 File Offset: 0x00006A70
	public static uint $ConstGCArrayBound$0x7c3cd6b8$89$;

	// Token: 0x04000967 RID: 2407 RVA: 0x000075DC File Offset: 0x000069DC
	public static uint $ConstGCArrayBound$0x7c3cd6b8$126$;

	// Token: 0x04000968 RID: 2408 RVA: 0x000076B8 File Offset: 0x00006AB8
	public static uint $ConstGCArrayBound$0x7c3cd6b8$71$;

	// Token: 0x04000969 RID: 2409 RVA: 0x0000762C File Offset: 0x00006A2C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$106$;

	// Token: 0x0400096A RID: 2410 RVA: 0x00007730 File Offset: 0x00006B30
	public static uint $ConstGCArrayBound$0x7c3cd6b8$41$;

	// Token: 0x0400096B RID: 2411 RVA: 0x000076FC File Offset: 0x00006AFC
	public static uint $ConstGCArrayBound$0x7c3cd6b8$54$;

	// Token: 0x0400096C RID: 2412 RVA: 0x000076F0 File Offset: 0x00006AF0
	public static uint $ConstGCArrayBound$0x7c3cd6b8$57$;

	// Token: 0x0400096D RID: 2413 RVA: 0x00007678 File Offset: 0x00006A78
	public static uint $ConstGCArrayBound$0x7c3cd6b8$87$;

	// Token: 0x0400096E RID: 2414 RVA: 0x000076F8 File Offset: 0x00006AF8
	public static uint $ConstGCArrayBound$0x7c3cd6b8$55$;

	// Token: 0x0400096F RID: 2415 RVA: 0x00007690 File Offset: 0x00006A90
	public static uint $ConstGCArrayBound$0x7c3cd6b8$81$;

	// Token: 0x04000970 RID: 2416 RVA: 0x00007684 File Offset: 0x00006A84
	public static uint $ConstGCArrayBound$0x7c3cd6b8$84$;

	// Token: 0x04000971 RID: 2417 RVA: 0x000077B4 File Offset: 0x00006BB4
	public static uint $ConstGCArrayBound$0x7c3cd6b8$8$;

	// Token: 0x04000972 RID: 2418 RVA: 0x00007740 File Offset: 0x00006B40
	public static uint $ConstGCArrayBound$0x7c3cd6b8$37$;

	// Token: 0x04000973 RID: 2419 RVA: 0x00007778 File Offset: 0x00006B78
	public static uint $ConstGCArrayBound$0x7c3cd6b8$23$;

	// Token: 0x04000974 RID: 2420 RVA: 0x0000773C File Offset: 0x00006B3C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$38$;

	// Token: 0x04000975 RID: 2421 RVA: 0x00007748 File Offset: 0x00006B48
	public static uint $ConstGCArrayBound$0x7c3cd6b8$35$;

	// Token: 0x04000976 RID: 2422 RVA: 0x000076E4 File Offset: 0x00006AE4
	public static uint $ConstGCArrayBound$0x7c3cd6b8$60$;

	// Token: 0x04000977 RID: 2423 RVA: 0x000077AC File Offset: 0x00006BAC
	public static uint $ConstGCArrayBound$0x7c3cd6b8$10$;

	// Token: 0x04000978 RID: 2424 RVA: 0x00007610 File Offset: 0x00006A10
	public static uint $ConstGCArrayBound$0x7c3cd6b8$113$;

	// Token: 0x04000979 RID: 2425 RVA: 0x00007794 File Offset: 0x00006B94
	public static uint $ConstGCArrayBound$0x7c3cd6b8$16$;

	// Token: 0x0400097A RID: 2426 RVA: 0x00007604 File Offset: 0x00006A04
	public static uint $ConstGCArrayBound$0x7c3cd6b8$116$;

	// Token: 0x0400097B RID: 2427 RVA: 0x00007668 File Offset: 0x00006A68
	public static uint $ConstGCArrayBound$0x7c3cd6b8$91$;

	// Token: 0x0400097C RID: 2428 RVA: 0x000077C8 File Offset: 0x00006BC8
	public static uint $ConstGCArrayBound$0x7c3cd6b8$3$;

	// Token: 0x0400097D RID: 2429 RVA: 0x000076A8 File Offset: 0x00006AA8
	public static uint $ConstGCArrayBound$0x7c3cd6b8$75$;

	// Token: 0x0400097E RID: 2430 RVA: 0x00007720 File Offset: 0x00006B20
	public static uint $ConstGCArrayBound$0x7c3cd6b8$45$;

	// Token: 0x0400097F RID: 2431 RVA: 0x000075F4 File Offset: 0x000069F4
	public static uint $ConstGCArrayBound$0x7c3cd6b8$120$;

	// Token: 0x04000980 RID: 2432 RVA: 0x0000776C File Offset: 0x00006B6C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$26$;

	// Token: 0x04000981 RID: 2433 RVA: 0x00007734 File Offset: 0x00006B34
	public static uint $ConstGCArrayBound$0x7c3cd6b8$40$;

	// Token: 0x04000982 RID: 2434 RVA: 0x00007660 File Offset: 0x00006A60
	public static uint $ConstGCArrayBound$0x7c3cd6b8$93$;

	// Token: 0x04000983 RID: 2435 RVA: 0x00007780 File Offset: 0x00006B80
	public static uint $ConstGCArrayBound$0x7c3cd6b8$21$;

	// Token: 0x04000984 RID: 2436 RVA: 0x00007698 File Offset: 0x00006A98
	public static uint $ConstGCArrayBound$0x7c3cd6b8$79$;

	// Token: 0x04000985 RID: 2437 RVA: 0x000077A4 File Offset: 0x00006BA4
	public static uint $ConstGCArrayBound$0x7c3cd6b8$12$;

	// Token: 0x04000986 RID: 2438 RVA: 0x00007640 File Offset: 0x00006A40
	public static uint $ConstGCArrayBound$0x7c3cd6b8$101$;

	// Token: 0x04000987 RID: 2439 RVA: 0x000077C4 File Offset: 0x00006BC4
	public static uint $ConstGCArrayBound$0x7c3cd6b8$4$;

	// Token: 0x04000988 RID: 2440 RVA: 0x000077B0 File Offset: 0x00006BB0
	public static uint $ConstGCArrayBound$0x7c3cd6b8$9$;

	// Token: 0x04000989 RID: 2441 RVA: 0x00007754 File Offset: 0x00006B54
	public static uint $ConstGCArrayBound$0x7c3cd6b8$32$;

	// Token: 0x0400098A RID: 2442 RVA: 0x0000777C File Offset: 0x00006B7C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$22$;

	// Token: 0x0400098B RID: 2443 RVA: 0x00007790 File Offset: 0x00006B90
	public static uint $ConstGCArrayBound$0x7c3cd6b8$17$;

	// Token: 0x0400098C RID: 2444 RVA: 0x00007798 File Offset: 0x00006B98
	public static uint $ConstGCArrayBound$0x7c3cd6b8$15$;

	// Token: 0x0400098D RID: 2445 RVA: 0x00007664 File Offset: 0x00006A64
	public static uint $ConstGCArrayBound$0x7c3cd6b8$92$;

	// Token: 0x0400098E RID: 2446 RVA: 0x000075E4 File Offset: 0x000069E4
	public static uint $ConstGCArrayBound$0x7c3cd6b8$124$;

	// Token: 0x0400098F RID: 2447 RVA: 0x000075F0 File Offset: 0x000069F0
	public static uint $ConstGCArrayBound$0x7c3cd6b8$121$;

	// Token: 0x04000990 RID: 2448 RVA: 0x000076BC File Offset: 0x00006ABC
	public static uint $ConstGCArrayBound$0x7c3cd6b8$70$;

	// Token: 0x04000991 RID: 2449 RVA: 0x0000760C File Offset: 0x00006A0C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$114$;

	// Token: 0x04000992 RID: 2450 RVA: 0x000075F8 File Offset: 0x000069F8
	public static uint $ConstGCArrayBound$0x7c3cd6b8$119$;

	// Token: 0x04000993 RID: 2451 RVA: 0x00007694 File Offset: 0x00006A94
	public static uint $ConstGCArrayBound$0x7c3cd6b8$80$;

	// Token: 0x04000994 RID: 2452 RVA: 0x00007728 File Offset: 0x00006B28
	public static uint $ConstGCArrayBound$0x7c3cd6b8$43$;

	// Token: 0x04000995 RID: 2453 RVA: 0x00007788 File Offset: 0x00006B88
	public static uint $ConstGCArrayBound$0x7c3cd6b8$19$;

	// Token: 0x04000996 RID: 2454 RVA: 0x00007638 File Offset: 0x00006A38
	public static uint $ConstGCArrayBound$0x7c3cd6b8$103$;

	// Token: 0x04000997 RID: 2455 RVA: 0x0000770C File Offset: 0x00006B0C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$50$;

	// Token: 0x04000998 RID: 2456 RVA: 0x00007718 File Offset: 0x00006B18
	public static uint $ConstGCArrayBound$0x7c3cd6b8$47$;

	// Token: 0x04000999 RID: 2457 RVA: 0x0000767C File Offset: 0x00006A7C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$86$;

	// Token: 0x0400099A RID: 2458 RVA: 0x00007724 File Offset: 0x00006B24
	public static uint $ConstGCArrayBound$0x7c3cd6b8$44$;

	// Token: 0x0400099B RID: 2459 RVA: 0x000076B4 File Offset: 0x00006AB4
	public static uint $ConstGCArrayBound$0x7c3cd6b8$72$;

	// Token: 0x0400099C RID: 2460 RVA: 0x000076C4 File Offset: 0x00006AC4
	public static uint $ConstGCArrayBound$0x7c3cd6b8$68$;

	// Token: 0x0400099D RID: 2461 RVA: 0x00007618 File Offset: 0x00006A18
	public static uint $ConstGCArrayBound$0x7c3cd6b8$111$;

	// Token: 0x0400099E RID: 2462 RVA: 0x0000761C File Offset: 0x00006A1C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$110$;

	// Token: 0x0400099F RID: 2463 RVA: 0x00007704 File Offset: 0x00006B04
	public static uint $ConstGCArrayBound$0x7c3cd6b8$52$;

	// Token: 0x040009A0 RID: 2464 RVA: 0x000076AC File Offset: 0x00006AAC
	public static uint $ConstGCArrayBound$0x7c3cd6b8$74$;

	// Token: 0x040009A1 RID: 2465 RVA: 0x000075D4 File Offset: 0x000069D4
	public static uint $ConstGCArrayBound$0x7c3cd6b8$128$;

	// Token: 0x040009A2 RID: 2466 RVA: 0x000077A0 File Offset: 0x00006BA0
	public static uint $ConstGCArrayBound$0x7c3cd6b8$13$;

	// Token: 0x040009A3 RID: 2467 RVA: 0x0000775C File Offset: 0x00006B5C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$30$;

	// Token: 0x040009A4 RID: 2468 RVA: 0x000076B0 File Offset: 0x00006AB0
	public static uint $ConstGCArrayBound$0x7c3cd6b8$73$;

	// Token: 0x040009A5 RID: 2469 RVA: 0x00007744 File Offset: 0x00006B44
	public static uint $ConstGCArrayBound$0x7c3cd6b8$36$;

	// Token: 0x040009A6 RID: 2470 RVA: 0x00007620 File Offset: 0x00006A20
	public static uint $ConstGCArrayBound$0x7c3cd6b8$109$;

	// Token: 0x040009A7 RID: 2471 RVA: 0x000075E8 File Offset: 0x000069E8
	public static uint $ConstGCArrayBound$0x7c3cd6b8$123$;

	// Token: 0x040009A8 RID: 2472 RVA: 0x0000778C File Offset: 0x00006B8C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$18$;

	// Token: 0x040009A9 RID: 2473 RVA: 0x00007784 File Offset: 0x00006B84
	public static uint $ConstGCArrayBound$0x7c3cd6b8$20$;

	// Token: 0x040009AA RID: 2474 RVA: 0x0000774C File Offset: 0x00006B4C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$34$;

	// Token: 0x040009AB RID: 2475 RVA: 0x00007714 File Offset: 0x00006B14
	public static uint $ConstGCArrayBound$0x7c3cd6b8$48$;

	// Token: 0x040009AC RID: 2476 RVA: 0x00007674 File Offset: 0x00006A74
	public static uint $ConstGCArrayBound$0x7c3cd6b8$88$;

	// Token: 0x040009AD RID: 2477 RVA: 0x00007708 File Offset: 0x00006B08
	public static uint $ConstGCArrayBound$0x7c3cd6b8$51$;

	// Token: 0x040009AE RID: 2478 RVA: 0x000076E0 File Offset: 0x00006AE0
	public static uint $ConstGCArrayBound$0x7c3cd6b8$61$;

	// Token: 0x040009AF RID: 2479 RVA: 0x000076C8 File Offset: 0x00006AC8
	public static uint $ConstGCArrayBound$0x7c3cd6b8$67$;

	// Token: 0x040009B0 RID: 2480 RVA: 0x00007758 File Offset: 0x00006B58
	public static uint $ConstGCArrayBound$0x7c3cd6b8$31$;

	// Token: 0x040009B1 RID: 2481 RVA: 0x000077C0 File Offset: 0x00006BC0
	public static uint $ConstGCArrayBound$0x7c3cd6b8$5$;

	// Token: 0x040009B2 RID: 2482 RVA: 0x0000763C File Offset: 0x00006A3C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$102$;

	// Token: 0x040009B3 RID: 2483 RVA: 0x000076D8 File Offset: 0x00006AD8
	public static uint $ConstGCArrayBound$0x7c3cd6b8$63$;

	// Token: 0x040009B4 RID: 2484 RVA: 0x00007658 File Offset: 0x00006A58
	public static uint $ConstGCArrayBound$0x7c3cd6b8$95$;

	// Token: 0x040009B5 RID: 2485 RVA: 0x0000768C File Offset: 0x00006A8C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$82$;

	// Token: 0x040009B6 RID: 2486 RVA: 0x000075EC File Offset: 0x000069EC
	public static uint $ConstGCArrayBound$0x7c3cd6b8$122$;

	// Token: 0x040009B7 RID: 2487 RVA: 0x00007648 File Offset: 0x00006A48
	public static uint $ConstGCArrayBound$0x7c3cd6b8$99$;

	// Token: 0x040009B8 RID: 2488 RVA: 0x000076E8 File Offset: 0x00006AE8
	public static uint $ConstGCArrayBound$0x7c3cd6b8$59$;

	// Token: 0x040009B9 RID: 2489 RVA: 0x00007634 File Offset: 0x00006A34
	public static uint $ConstGCArrayBound$0x7c3cd6b8$104$;

	// Token: 0x040009BA RID: 2490 RVA: 0x00007680 File Offset: 0x00006A80
	public static uint $ConstGCArrayBound$0x7c3cd6b8$85$;

	// Token: 0x040009BB RID: 2491 RVA: 0x0000766C File Offset: 0x00006A6C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$90$;

	// Token: 0x040009BC RID: 2492 RVA: 0x000077BC File Offset: 0x00006BBC
	public static uint $ConstGCArrayBound$0x7c3cd6b8$6$;

	// Token: 0x040009BD RID: 2493 RVA: 0x000075E0 File Offset: 0x000069E0
	public static uint $ConstGCArrayBound$0x7c3cd6b8$125$;

	// Token: 0x040009BE RID: 2494 RVA: 0x000077A8 File Offset: 0x00006BA8
	public static uint $ConstGCArrayBound$0x7c3cd6b8$11$;

	// Token: 0x040009BF RID: 2495 RVA: 0x000076CC File Offset: 0x00006ACC
	public static uint $ConstGCArrayBound$0x7c3cd6b8$66$;

	// Token: 0x040009C0 RID: 2496 RVA: 0x000076A0 File Offset: 0x00006AA0
	public static uint $ConstGCArrayBound$0x7c3cd6b8$77$;

	// Token: 0x040009C1 RID: 2497 RVA: 0x0000764C File Offset: 0x00006A4C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$98$;

	// Token: 0x040009C2 RID: 2498 RVA: 0x000076D4 File Offset: 0x00006AD4
	public static uint $ConstGCArrayBound$0x7c3cd6b8$64$;

	// Token: 0x040009C3 RID: 2499 RVA: 0x00007764 File Offset: 0x00006B64
	public static uint $ConstGCArrayBound$0x7c3cd6b8$28$;

	// Token: 0x040009C4 RID: 2500 RVA: 0x000076C0 File Offset: 0x00006AC0
	public static uint $ConstGCArrayBound$0x7c3cd6b8$69$;

	// Token: 0x040009C5 RID: 2501 RVA: 0x00007750 File Offset: 0x00006B50
	public static uint $ConstGCArrayBound$0x7c3cd6b8$33$;

	// Token: 0x040009C6 RID: 2502 RVA: 0x00007644 File Offset: 0x00006A44
	public static uint $ConstGCArrayBound$0x7c3cd6b8$100$;

	// Token: 0x040009C7 RID: 2503 RVA: 0x0000769C File Offset: 0x00006A9C
	public static uint $ConstGCArrayBound$0x7c3cd6b8$78$;

	// Token: 0x040009C8 RID: 2504 RVA: 0x00007710 File Offset: 0x00006B10
	public static uint $ConstGCArrayBound$0x7c3cd6b8$49$;

	// Token: 0x040009C9 RID: 2505 RVA: 0x000077D0 File Offset: 0x00006BD0
	public static uint $ConstGCArrayBound$0x7c3cd6b8$1$;

	// Token: 0x040009CA RID: 2506 RVA: 0x00007700 File Offset: 0x00006B00
	public static uint $ConstGCArrayBound$0x7c3cd6b8$53$;

	// Token: 0x040009CB RID: 2507 RVA: 0x000076DC File Offset: 0x00006ADC
	public static uint $ConstGCArrayBound$0x7c3cd6b8$62$;

	// Token: 0x040009CC RID: 2508 RVA: 0x000075D8 File Offset: 0x000069D8
	public static uint $ConstGCArrayBound$0x7c3cd6b8$127$;

	// Token: 0x040009CD RID: 2509 RVA: 0x00007770 File Offset: 0x00006B70
	public static uint $ConstGCArrayBound$0x7c3cd6b8$25$;

	// Token: 0x040009CE RID: 2510 RVA: 0x00007624 File Offset: 0x00006A24
	public static uint $ConstGCArrayBound$0x7c3cd6b8$108$;

	// Token: 0x040009CF RID: 2511 RVA: 0x00007774 File Offset: 0x00006B74
	public static uint $ConstGCArrayBound$0x7c3cd6b8$24$;

	// Token: 0x040009D0 RID: 2512 RVA: 0x00007CEC File Offset: 0x000070EC
	public static uint $ConstGCArrayBound$0x2a0931a2$5$;

	// Token: 0x040009D1 RID: 2513 RVA: 0x00007C04 File Offset: 0x00007004
	public static uint $ConstGCArrayBound$0x2a0931a2$63$;

	// Token: 0x040009D2 RID: 2514 RVA: 0x00007C88 File Offset: 0x00007088
	public static uint $ConstGCArrayBound$0x2a0931a2$30$;

	// Token: 0x040009D3 RID: 2515 RVA: 0x00007CD4 File Offset: 0x000070D4
	public static uint $ConstGCArrayBound$0x2a0931a2$11$;

	// Token: 0x040009D4 RID: 2516 RVA: 0x00007B88 File Offset: 0x00006F88
	public static uint $ConstGCArrayBound$0x2a0931a2$94$;

	// Token: 0x040009D5 RID: 2517 RVA: 0x00007CE4 File Offset: 0x000070E4
	public static uint $ConstGCArrayBound$0x2a0931a2$7$;

	// Token: 0x040009D6 RID: 2518 RVA: 0x00007B70 File Offset: 0x00006F70
	public static uint $ConstGCArrayBound$0x2a0931a2$100$;

	// Token: 0x040009D7 RID: 2519 RVA: 0x00007C14 File Offset: 0x00007014
	public static uint $ConstGCArrayBound$0x2a0931a2$59$;

	// Token: 0x040009D8 RID: 2520 RVA: 0x00007B68 File Offset: 0x00006F68
	public static uint $ConstGCArrayBound$0x2a0931a2$102$;

	// Token: 0x040009D9 RID: 2521 RVA: 0x00007CCC File Offset: 0x000070CC
	public static uint $ConstGCArrayBound$0x2a0931a2$13$;

	// Token: 0x040009DA RID: 2522 RVA: 0x00007C3C File Offset: 0x0000703C
	public static uint $ConstGCArrayBound$0x2a0931a2$49$;

	// Token: 0x040009DB RID: 2523 RVA: 0x00007CF0 File Offset: 0x000070F0
	public static uint $ConstGCArrayBound$0x2a0931a2$4$;

	// Token: 0x040009DC RID: 2524 RVA: 0x00007CE8 File Offset: 0x000070E8
	public static uint $ConstGCArrayBound$0x2a0931a2$6$;

	// Token: 0x040009DD RID: 2525 RVA: 0x00007B54 File Offset: 0x00006F54
	public static uint $ConstGCArrayBound$0x2a0931a2$107$;

	// Token: 0x040009DE RID: 2526 RVA: 0x00007B44 File Offset: 0x00006F44
	public static uint $ConstGCArrayBound$0x2a0931a2$111$;

	// Token: 0x040009DF RID: 2527 RVA: 0x00007CC4 File Offset: 0x000070C4
	public static uint $ConstGCArrayBound$0x2a0931a2$15$;

	// Token: 0x040009E0 RID: 2528 RVA: 0x00007C44 File Offset: 0x00007044
	public static uint $ConstGCArrayBound$0x2a0931a2$47$;

	// Token: 0x040009E1 RID: 2529 RVA: 0x00007C80 File Offset: 0x00007080
	public static uint $ConstGCArrayBound$0x2a0931a2$32$;

	// Token: 0x040009E2 RID: 2530 RVA: 0x00007CD0 File Offset: 0x000070D0
	public static uint $ConstGCArrayBound$0x2a0931a2$12$;

	// Token: 0x040009E3 RID: 2531 RVA: 0x00007CFC File Offset: 0x000070FC
	public static uint $ConstGCArrayBound$0x2a0931a2$1$;

	// Token: 0x040009E4 RID: 2532 RVA: 0x00007B80 File Offset: 0x00006F80
	public static uint $ConstGCArrayBound$0x2a0931a2$96$;

	// Token: 0x040009E5 RID: 2533 RVA: 0x00007C28 File Offset: 0x00007028
	public static uint $ConstGCArrayBound$0x2a0931a2$54$;

	// Token: 0x040009E6 RID: 2534 RVA: 0x00007B60 File Offset: 0x00006F60
	public static uint $ConstGCArrayBound$0x2a0931a2$104$;

	// Token: 0x040009E7 RID: 2535 RVA: 0x00007BD0 File Offset: 0x00006FD0
	public static uint $ConstGCArrayBound$0x2a0931a2$76$;

	// Token: 0x040009E8 RID: 2536 RVA: 0x00007C70 File Offset: 0x00007070
	public static uint $ConstGCArrayBound$0x2a0931a2$36$;

	// Token: 0x040009E9 RID: 2537 RVA: 0x00007CB8 File Offset: 0x000070B8
	public static uint $ConstGCArrayBound$0x2a0931a2$18$;

	// Token: 0x040009EA RID: 2538 RVA: 0x00007BD4 File Offset: 0x00006FD4
	public static uint $ConstGCArrayBound$0x2a0931a2$75$;

	// Token: 0x040009EB RID: 2539 RVA: 0x00007C7C File Offset: 0x0000707C
	public static uint $ConstGCArrayBound$0x2a0931a2$33$;

	// Token: 0x040009EC RID: 2540 RVA: 0x00007C40 File Offset: 0x00007040
	public static uint $ConstGCArrayBound$0x2a0931a2$48$;

	// Token: 0x040009ED RID: 2541 RVA: 0x00007B2C File Offset: 0x00006F2C
	public static uint $ConstGCArrayBound$0x2a0931a2$117$;

	// Token: 0x040009EE RID: 2542 RVA: 0x00007B34 File Offset: 0x00006F34
	public static uint $ConstGCArrayBound$0x2a0931a2$115$;

	// Token: 0x040009EF RID: 2543 RVA: 0x00007B28 File Offset: 0x00006F28
	public static uint $ConstGCArrayBound$0x2a0931a2$118$;

	// Token: 0x040009F0 RID: 2544 RVA: 0x00007CBC File Offset: 0x000070BC
	public static uint $ConstGCArrayBound$0x2a0931a2$17$;

	// Token: 0x040009F1 RID: 2545 RVA: 0x00007BB8 File Offset: 0x00006FB8
	public static uint $ConstGCArrayBound$0x2a0931a2$82$;

	// Token: 0x040009F2 RID: 2546 RVA: 0x00007BEC File Offset: 0x00006FEC
	public static uint $ConstGCArrayBound$0x2a0931a2$69$;

	// Token: 0x040009F3 RID: 2547 RVA: 0x00007CA4 File Offset: 0x000070A4
	public static uint $ConstGCArrayBound$0x2a0931a2$23$;

	// Token: 0x040009F4 RID: 2548 RVA: 0x00007C60 File Offset: 0x00007060
	public static uint $ConstGCArrayBound$0x2a0931a2$40$;

	// Token: 0x040009F5 RID: 2549 RVA: 0x00007CB4 File Offset: 0x000070B4
	public static uint $ConstGCArrayBound$0x2a0931a2$19$;

	// Token: 0x040009F6 RID: 2550 RVA: 0x00007BC4 File Offset: 0x00006FC4
	public static uint $ConstGCArrayBound$0x2a0931a2$79$;

	// Token: 0x040009F7 RID: 2551 RVA: 0x00007C68 File Offset: 0x00007068
	public static uint $ConstGCArrayBound$0x2a0931a2$38$;

	// Token: 0x040009F8 RID: 2552 RVA: 0x00007B50 File Offset: 0x00006F50
	public static uint $ConstGCArrayBound$0x2a0931a2$108$;

	// Token: 0x040009F9 RID: 2553 RVA: 0x00007BA8 File Offset: 0x00006FA8
	public static uint $ConstGCArrayBound$0x2a0931a2$86$;

	// Token: 0x040009FA RID: 2554 RVA: 0x00007CA0 File Offset: 0x000070A0
	public static uint $ConstGCArrayBound$0x2a0931a2$24$;

	// Token: 0x040009FB RID: 2555 RVA: 0x00007B10 File Offset: 0x00006F10
	public static uint $ConstGCArrayBound$0x2a0931a2$124$;

	// Token: 0x040009FC RID: 2556 RVA: 0x00007B14 File Offset: 0x00006F14
	public static uint $ConstGCArrayBound$0x2a0931a2$123$;

	// Token: 0x040009FD RID: 2557 RVA: 0x00007BFC File Offset: 0x00006FFC
	public static uint $ConstGCArrayBound$0x2a0931a2$65$;

	// Token: 0x040009FE RID: 2558 RVA: 0x00007CDC File Offset: 0x000070DC
	public static uint $ConstGCArrayBound$0x2a0931a2$9$;

	// Token: 0x040009FF RID: 2559 RVA: 0x00007BF4 File Offset: 0x00006FF4
	public static uint $ConstGCArrayBound$0x2a0931a2$67$;

	// Token: 0x04000A00 RID: 2560 RVA: 0x00007B64 File Offset: 0x00006F64
	public static uint $ConstGCArrayBound$0x2a0931a2$103$;

	// Token: 0x04000A01 RID: 2561 RVA: 0x00007C1C File Offset: 0x0000701C
	public static uint $ConstGCArrayBound$0x2a0931a2$57$;

	// Token: 0x04000A02 RID: 2562 RVA: 0x00007C4C File Offset: 0x0000704C
	public static uint $ConstGCArrayBound$0x2a0931a2$45$;

	// Token: 0x04000A03 RID: 2563 RVA: 0x00007C34 File Offset: 0x00007034
	public static uint $ConstGCArrayBound$0x2a0931a2$51$;

	// Token: 0x04000A04 RID: 2564 RVA: 0x00007C58 File Offset: 0x00007058
	public static uint $ConstGCArrayBound$0x2a0931a2$42$;

	// Token: 0x04000A05 RID: 2565 RVA: 0x00007B8C File Offset: 0x00006F8C
	public static uint $ConstGCArrayBound$0x2a0931a2$93$;

	// Token: 0x04000A06 RID: 2566 RVA: 0x00007BDC File Offset: 0x00006FDC
	public static uint $ConstGCArrayBound$0x2a0931a2$73$;

	// Token: 0x04000A07 RID: 2567 RVA: 0x00007C84 File Offset: 0x00007084
	public static uint $ConstGCArrayBound$0x2a0931a2$31$;

	// Token: 0x04000A08 RID: 2568 RVA: 0x00007B84 File Offset: 0x00006F84
	public static uint $ConstGCArrayBound$0x2a0931a2$95$;

	// Token: 0x04000A09 RID: 2569 RVA: 0x00007B6C File Offset: 0x00006F6C
	public static uint $ConstGCArrayBound$0x2a0931a2$101$;

	// Token: 0x04000A0A RID: 2570 RVA: 0x00007B04 File Offset: 0x00006F04
	public static uint $ConstGCArrayBound$0x2a0931a2$127$;

	// Token: 0x04000A0B RID: 2571 RVA: 0x00007CE0 File Offset: 0x000070E0
	public static uint $ConstGCArrayBound$0x2a0931a2$8$;

	// Token: 0x04000A0C RID: 2572 RVA: 0x00007BB4 File Offset: 0x00006FB4
	public static uint $ConstGCArrayBound$0x2a0931a2$83$;

	// Token: 0x04000A0D RID: 2573 RVA: 0x00007B30 File Offset: 0x00006F30
	public static uint $ConstGCArrayBound$0x2a0931a2$116$;

	// Token: 0x04000A0E RID: 2574 RVA: 0x00007BF8 File Offset: 0x00006FF8
	public static uint $ConstGCArrayBound$0x2a0931a2$66$;

	// Token: 0x04000A0F RID: 2575 RVA: 0x00007C9C File Offset: 0x0000709C
	public static uint $ConstGCArrayBound$0x2a0931a2$25$;

	// Token: 0x04000A10 RID: 2576 RVA: 0x00007C90 File Offset: 0x00007090
	public static uint $ConstGCArrayBound$0x2a0931a2$28$;

	// Token: 0x04000A11 RID: 2577 RVA: 0x00007C5C File Offset: 0x0000705C
	public static uint $ConstGCArrayBound$0x2a0931a2$41$;

	// Token: 0x04000A12 RID: 2578 RVA: 0x00007B7C File Offset: 0x00006F7C
	public static uint $ConstGCArrayBound$0x2a0931a2$97$;

	// Token: 0x04000A13 RID: 2579 RVA: 0x00007C54 File Offset: 0x00007054
	public static uint $ConstGCArrayBound$0x2a0931a2$43$;

	// Token: 0x04000A14 RID: 2580 RVA: 0x00007B20 File Offset: 0x00006F20
	public static uint $ConstGCArrayBound$0x2a0931a2$120$;

	// Token: 0x04000A15 RID: 2581 RVA: 0x00007B00 File Offset: 0x00006F00
	public static uint $ConstGCArrayBound$0x2a0931a2$128$;

	// Token: 0x04000A16 RID: 2582 RVA: 0x00007B18 File Offset: 0x00006F18
	public static uint $ConstGCArrayBound$0x2a0931a2$122$;

	// Token: 0x04000A17 RID: 2583 RVA: 0x00007B24 File Offset: 0x00006F24
	public static uint $ConstGCArrayBound$0x2a0931a2$119$;

	// Token: 0x04000A18 RID: 2584 RVA: 0x00007CA8 File Offset: 0x000070A8
	public static uint $ConstGCArrayBound$0x2a0931a2$22$;

	// Token: 0x04000A19 RID: 2585 RVA: 0x00007BBC File Offset: 0x00006FBC
	public static uint $ConstGCArrayBound$0x2a0931a2$81$;

	// Token: 0x04000A1A RID: 2586 RVA: 0x00007C38 File Offset: 0x00007038
	public static uint $ConstGCArrayBound$0x2a0931a2$50$;

	// Token: 0x04000A1B RID: 2587 RVA: 0x00007BC8 File Offset: 0x00006FC8
	public static uint $ConstGCArrayBound$0x2a0931a2$78$;

	// Token: 0x04000A1C RID: 2588 RVA: 0x00007CAC File Offset: 0x000070AC
	public static uint $ConstGCArrayBound$0x2a0931a2$21$;

	// Token: 0x04000A1D RID: 2589 RVA: 0x00007C50 File Offset: 0x00007050
	public static uint $ConstGCArrayBound$0x2a0931a2$44$;

	// Token: 0x04000A1E RID: 2590 RVA: 0x00007C2C File Offset: 0x0000702C
	public static uint $ConstGCArrayBound$0x2a0931a2$53$;

	// Token: 0x04000A1F RID: 2591 RVA: 0x00007BF0 File Offset: 0x00006FF0
	public static uint $ConstGCArrayBound$0x2a0931a2$68$;

	// Token: 0x04000A20 RID: 2592 RVA: 0x00007C0C File Offset: 0x0000700C
	public static uint $ConstGCArrayBound$0x2a0931a2$61$;

	// Token: 0x04000A21 RID: 2593 RVA: 0x00007C8C File Offset: 0x0000708C
	public static uint $ConstGCArrayBound$0x2a0931a2$29$;

	// Token: 0x04000A22 RID: 2594 RVA: 0x00007BAC File Offset: 0x00006FAC
	public static uint $ConstGCArrayBound$0x2a0931a2$85$;

	// Token: 0x04000A23 RID: 2595 RVA: 0x00007B3C File Offset: 0x00006F3C
	public static uint $ConstGCArrayBound$0x2a0931a2$113$;

	// Token: 0x04000A24 RID: 2596 RVA: 0x00007CD8 File Offset: 0x000070D8
	public static uint $ConstGCArrayBound$0x2a0931a2$10$;

	// Token: 0x04000A25 RID: 2597 RVA: 0x00007CF4 File Offset: 0x000070F4
	public static uint $ConstGCArrayBound$0x2a0931a2$3$;

	// Token: 0x04000A26 RID: 2598 RVA: 0x00007BD8 File Offset: 0x00006FD8
	public static uint $ConstGCArrayBound$0x2a0931a2$74$;

	// Token: 0x04000A27 RID: 2599 RVA: 0x00007B90 File Offset: 0x00006F90
	public static uint $ConstGCArrayBound$0x2a0931a2$92$;

	// Token: 0x04000A28 RID: 2600 RVA: 0x00007B38 File Offset: 0x00006F38
	public static uint $ConstGCArrayBound$0x2a0931a2$114$;

	// Token: 0x04000A29 RID: 2601 RVA: 0x00007B1C File Offset: 0x00006F1C
	public static uint $ConstGCArrayBound$0x2a0931a2$121$;

	// Token: 0x04000A2A RID: 2602 RVA: 0x00007B4C File Offset: 0x00006F4C
	public static uint $ConstGCArrayBound$0x2a0931a2$109$;

	// Token: 0x04000A2B RID: 2603 RVA: 0x00007B58 File Offset: 0x00006F58
	public static uint $ConstGCArrayBound$0x2a0931a2$106$;

	// Token: 0x04000A2C RID: 2604 RVA: 0x00007C00 File Offset: 0x00007000
	public static uint $ConstGCArrayBound$0x2a0931a2$64$;

	// Token: 0x04000A2D RID: 2605 RVA: 0x00007B9C File Offset: 0x00006F9C
	public static uint $ConstGCArrayBound$0x2a0931a2$89$;

	// Token: 0x04000A2E RID: 2606 RVA: 0x00007C08 File Offset: 0x00007008
	public static uint $ConstGCArrayBound$0x2a0931a2$62$;

	// Token: 0x04000A2F RID: 2607 RVA: 0x00007BE0 File Offset: 0x00006FE0
	public static uint $ConstGCArrayBound$0x2a0931a2$72$;

	// Token: 0x04000A30 RID: 2608 RVA: 0x00007B40 File Offset: 0x00006F40
	public static uint $ConstGCArrayBound$0x2a0931a2$112$;

	// Token: 0x04000A31 RID: 2609 RVA: 0x00007BC0 File Offset: 0x00006FC0
	public static uint $ConstGCArrayBound$0x2a0931a2$80$;

	// Token: 0x04000A32 RID: 2610 RVA: 0x00007CC8 File Offset: 0x000070C8
	public static uint $ConstGCArrayBound$0x2a0931a2$14$;

	// Token: 0x04000A33 RID: 2611 RVA: 0x00007C64 File Offset: 0x00007064
	public static uint $ConstGCArrayBound$0x2a0931a2$39$;

	// Token: 0x04000A34 RID: 2612 RVA: 0x00007B5C File Offset: 0x00006F5C
	public static uint $ConstGCArrayBound$0x2a0931a2$105$;

	// Token: 0x04000A35 RID: 2613 RVA: 0x00007B08 File Offset: 0x00006F08
	public static uint $ConstGCArrayBound$0x2a0931a2$126$;

	// Token: 0x04000A36 RID: 2614 RVA: 0x00007C18 File Offset: 0x00007018
	public static uint $ConstGCArrayBound$0x2a0931a2$58$;

	// Token: 0x04000A37 RID: 2615 RVA: 0x00007CF8 File Offset: 0x000070F8
	public static uint $ConstGCArrayBound$0x2a0931a2$2$;

	// Token: 0x04000A38 RID: 2616 RVA: 0x00007C74 File Offset: 0x00007074
	public static uint $ConstGCArrayBound$0x2a0931a2$35$;

	// Token: 0x04000A39 RID: 2617 RVA: 0x00007BA0 File Offset: 0x00006FA0
	public static uint $ConstGCArrayBound$0x2a0931a2$88$;

	// Token: 0x04000A3A RID: 2618 RVA: 0x00007B94 File Offset: 0x00006F94
	public static uint $ConstGCArrayBound$0x2a0931a2$91$;

	// Token: 0x04000A3B RID: 2619 RVA: 0x00007BE8 File Offset: 0x00006FE8
	public static uint $ConstGCArrayBound$0x2a0931a2$70$;

	// Token: 0x04000A3C RID: 2620 RVA: 0x00007BA4 File Offset: 0x00006FA4
	public static uint $ConstGCArrayBound$0x2a0931a2$87$;

	// Token: 0x04000A3D RID: 2621 RVA: 0x00007CB0 File Offset: 0x000070B0
	public static uint $ConstGCArrayBound$0x2a0931a2$20$;

	// Token: 0x04000A3E RID: 2622 RVA: 0x00007C10 File Offset: 0x00007010
	public static uint $ConstGCArrayBound$0x2a0931a2$60$;

	// Token: 0x04000A3F RID: 2623 RVA: 0x00007CC0 File Offset: 0x000070C0
	public static uint $ConstGCArrayBound$0x2a0931a2$16$;

	// Token: 0x04000A40 RID: 2624 RVA: 0x00007BE4 File Offset: 0x00006FE4
	public static uint $ConstGCArrayBound$0x2a0931a2$71$;

	// Token: 0x04000A41 RID: 2625 RVA: 0x00007B0C File Offset: 0x00006F0C
	public static uint $ConstGCArrayBound$0x2a0931a2$125$;

	// Token: 0x04000A42 RID: 2626 RVA: 0x00007C48 File Offset: 0x00007048
	public static uint $ConstGCArrayBound$0x2a0931a2$46$;

	// Token: 0x04000A43 RID: 2627 RVA: 0x00007C24 File Offset: 0x00007024
	public static uint $ConstGCArrayBound$0x2a0931a2$55$;

	// Token: 0x04000A44 RID: 2628 RVA: 0x00007C94 File Offset: 0x00007094
	public static uint $ConstGCArrayBound$0x2a0931a2$27$;

	// Token: 0x04000A45 RID: 2629 RVA: 0x00007B98 File Offset: 0x00006F98
	public static uint $ConstGCArrayBound$0x2a0931a2$90$;

	// Token: 0x04000A46 RID: 2630 RVA: 0x00007C30 File Offset: 0x00007030
	public static uint $ConstGCArrayBound$0x2a0931a2$52$;

	// Token: 0x04000A47 RID: 2631 RVA: 0x00007BB0 File Offset: 0x00006FB0
	public static uint $ConstGCArrayBound$0x2a0931a2$84$;

	// Token: 0x04000A48 RID: 2632 RVA: 0x00007C20 File Offset: 0x00007020
	public static uint $ConstGCArrayBound$0x2a0931a2$56$;

	// Token: 0x04000A49 RID: 2633 RVA: 0x00007C6C File Offset: 0x0000706C
	public static uint $ConstGCArrayBound$0x2a0931a2$37$;

	// Token: 0x04000A4A RID: 2634 RVA: 0x00007B48 File Offset: 0x00006F48
	public static uint $ConstGCArrayBound$0x2a0931a2$110$;

	// Token: 0x04000A4B RID: 2635 RVA: 0x00007B78 File Offset: 0x00006F78
	public static uint $ConstGCArrayBound$0x2a0931a2$98$;

	// Token: 0x04000A4C RID: 2636 RVA: 0x00007C98 File Offset: 0x00007098
	public static uint $ConstGCArrayBound$0x2a0931a2$26$;

	// Token: 0x04000A4D RID: 2637 RVA: 0x00007BCC File Offset: 0x00006FCC
	public static uint $ConstGCArrayBound$0x2a0931a2$77$;

	// Token: 0x04000A4E RID: 2638 RVA: 0x00007B74 File Offset: 0x00006F74
	public static uint $ConstGCArrayBound$0x2a0931a2$99$;

	// Token: 0x04000A4F RID: 2639 RVA: 0x00007C78 File Offset: 0x00007078
	public static uint $ConstGCArrayBound$0x2a0931a2$34$;

	// Token: 0x04000A50 RID: 2640 RVA: 0x0000815C File Offset: 0x0000755C
	public static uint $ConstGCArrayBound$0x5a1db173$51$;

	// Token: 0x04000A51 RID: 2641 RVA: 0x000080A4 File Offset: 0x000074A4
	public static uint $ConstGCArrayBound$0x5a1db173$97$;

	// Token: 0x04000A52 RID: 2642 RVA: 0x00008070 File Offset: 0x00007470
	public static uint $ConstGCArrayBound$0x5a1db173$110$;

	// Token: 0x04000A53 RID: 2643 RVA: 0x00008154 File Offset: 0x00007554
	public static uint $ConstGCArrayBound$0x5a1db173$53$;

	// Token: 0x04000A54 RID: 2644 RVA: 0x00008200 File Offset: 0x00007600
	public static uint $ConstGCArrayBound$0x5a1db173$10$;

	// Token: 0x04000A55 RID: 2645 RVA: 0x0000813C File Offset: 0x0000753C
	public static uint $ConstGCArrayBound$0x5a1db173$59$;

	// Token: 0x04000A56 RID: 2646 RVA: 0x0000820C File Offset: 0x0000760C
	public static uint $ConstGCArrayBound$0x5a1db173$7$;

	// Token: 0x04000A57 RID: 2647 RVA: 0x000081B4 File Offset: 0x000075B4
	public static uint $ConstGCArrayBound$0x5a1db173$29$;

	// Token: 0x04000A58 RID: 2648 RVA: 0x00008098 File Offset: 0x00007498
	public static uint $ConstGCArrayBound$0x5a1db173$100$;

	// Token: 0x04000A59 RID: 2649 RVA: 0x000080A0 File Offset: 0x000074A0
	public static uint $ConstGCArrayBound$0x5a1db173$98$;

	// Token: 0x04000A5A RID: 2650 RVA: 0x000081AC File Offset: 0x000075AC
	public static uint $ConstGCArrayBound$0x5a1db173$31$;

	// Token: 0x04000A5B RID: 2651 RVA: 0x00008138 File Offset: 0x00007538
	public static uint $ConstGCArrayBound$0x5a1db173$60$;

	// Token: 0x04000A5C RID: 2652 RVA: 0x0000805C File Offset: 0x0000745C
	public static uint $ConstGCArrayBound$0x5a1db173$115$;

	// Token: 0x04000A5D RID: 2653 RVA: 0x000081F4 File Offset: 0x000075F4
	public static uint $ConstGCArrayBound$0x5a1db173$13$;

	// Token: 0x04000A5E RID: 2654 RVA: 0x0000804C File Offset: 0x0000744C
	public static uint $ConstGCArrayBound$0x5a1db173$119$;

	// Token: 0x04000A5F RID: 2655 RVA: 0x00008078 File Offset: 0x00007478
	public static uint $ConstGCArrayBound$0x5a1db173$108$;

	// Token: 0x04000A60 RID: 2656 RVA: 0x00008064 File Offset: 0x00007464
	public static uint $ConstGCArrayBound$0x5a1db173$113$;

	// Token: 0x04000A61 RID: 2657 RVA: 0x000080F0 File Offset: 0x000074F0
	public static uint $ConstGCArrayBound$0x5a1db173$78$;

	// Token: 0x04000A62 RID: 2658 RVA: 0x00008190 File Offset: 0x00007590
	public static uint $ConstGCArrayBound$0x5a1db173$38$;

	// Token: 0x04000A63 RID: 2659 RVA: 0x00008040 File Offset: 0x00007440
	public static uint $ConstGCArrayBound$0x5a1db173$122$;

	// Token: 0x04000A64 RID: 2660 RVA: 0x000080C0 File Offset: 0x000074C0
	public static uint $ConstGCArrayBound$0x5a1db173$90$;

	// Token: 0x04000A65 RID: 2661 RVA: 0x00008178 File Offset: 0x00007578
	public static uint $ConstGCArrayBound$0x5a1db173$44$;

	// Token: 0x04000A66 RID: 2662 RVA: 0x000080B0 File Offset: 0x000074B0
	public static uint $ConstGCArrayBound$0x5a1db173$94$;

	// Token: 0x04000A67 RID: 2663 RVA: 0x00008158 File Offset: 0x00007558
	public static uint $ConstGCArrayBound$0x5a1db173$52$;

	// Token: 0x04000A68 RID: 2664 RVA: 0x00008144 File Offset: 0x00007544
	public static uint $ConstGCArrayBound$0x5a1db173$57$;

	// Token: 0x04000A69 RID: 2665 RVA: 0x00008038 File Offset: 0x00007438
	public static uint $ConstGCArrayBound$0x5a1db173$124$;

	// Token: 0x04000A6A RID: 2666 RVA: 0x00008050 File Offset: 0x00007450
	public static uint $ConstGCArrayBound$0x5a1db173$118$;

	// Token: 0x04000A6B RID: 2667 RVA: 0x000081F8 File Offset: 0x000075F8
	public static uint $ConstGCArrayBound$0x5a1db173$12$;

	// Token: 0x04000A6C RID: 2668 RVA: 0x0000821C File Offset: 0x0000761C
	public static uint $ConstGCArrayBound$0x5a1db173$3$;

	// Token: 0x04000A6D RID: 2669 RVA: 0x000080F4 File Offset: 0x000074F4
	public static uint $ConstGCArrayBound$0x5a1db173$77$;

	// Token: 0x04000A6E RID: 2670 RVA: 0x000081E0 File Offset: 0x000075E0
	public static uint $ConstGCArrayBound$0x5a1db173$18$;

	// Token: 0x04000A6F RID: 2671 RVA: 0x0000812C File Offset: 0x0000752C
	public static uint $ConstGCArrayBound$0x5a1db173$63$;

	// Token: 0x04000A70 RID: 2672 RVA: 0x000081C4 File Offset: 0x000075C4
	public static uint $ConstGCArrayBound$0x5a1db173$25$;

	// Token: 0x04000A71 RID: 2673 RVA: 0x00008028 File Offset: 0x00007428
	public static uint $ConstGCArrayBound$0x5a1db173$128$;

	// Token: 0x04000A72 RID: 2674 RVA: 0x00008170 File Offset: 0x00007570
	public static uint $ConstGCArrayBound$0x5a1db173$46$;

	// Token: 0x04000A73 RID: 2675 RVA: 0x000080E0 File Offset: 0x000074E0
	public static uint $ConstGCArrayBound$0x5a1db173$82$;

	// Token: 0x04000A74 RID: 2676 RVA: 0x000080EC File Offset: 0x000074EC
	public static uint $ConstGCArrayBound$0x5a1db173$79$;

	// Token: 0x04000A75 RID: 2677 RVA: 0x00008160 File Offset: 0x00007560
	public static uint $ConstGCArrayBound$0x5a1db173$50$;

	// Token: 0x04000A76 RID: 2678 RVA: 0x00008104 File Offset: 0x00007504
	public static uint $ConstGCArrayBound$0x5a1db173$73$;

	// Token: 0x04000A77 RID: 2679 RVA: 0x0000803C File Offset: 0x0000743C
	public static uint $ConstGCArrayBound$0x5a1db173$123$;

	// Token: 0x04000A78 RID: 2680 RVA: 0x00008220 File Offset: 0x00007620
	public static uint $ConstGCArrayBound$0x5a1db173$2$;

	// Token: 0x04000A79 RID: 2681 RVA: 0x00008034 File Offset: 0x00007434
	public static uint $ConstGCArrayBound$0x5a1db173$125$;

	// Token: 0x04000A7A RID: 2682 RVA: 0x00008168 File Offset: 0x00007568
	public static uint $ConstGCArrayBound$0x5a1db173$48$;

	// Token: 0x04000A7B RID: 2683 RVA: 0x0000814C File Offset: 0x0000754C
	public static uint $ConstGCArrayBound$0x5a1db173$55$;

	// Token: 0x04000A7C RID: 2684 RVA: 0x00008048 File Offset: 0x00007448
	public static uint $ConstGCArrayBound$0x5a1db173$120$;

	// Token: 0x04000A7D RID: 2685 RVA: 0x00008180 File Offset: 0x00007580
	public static uint $ConstGCArrayBound$0x5a1db173$42$;

	// Token: 0x04000A7E RID: 2686 RVA: 0x000080CC File Offset: 0x000074CC
	public static uint $ConstGCArrayBound$0x5a1db173$87$;

	// Token: 0x04000A7F RID: 2687 RVA: 0x00008088 File Offset: 0x00007488
	public static uint $ConstGCArrayBound$0x5a1db173$104$;

	// Token: 0x04000A80 RID: 2688 RVA: 0x000081D8 File Offset: 0x000075D8
	public static uint $ConstGCArrayBound$0x5a1db173$20$;

	// Token: 0x04000A81 RID: 2689 RVA: 0x0000806C File Offset: 0x0000746C
	public static uint $ConstGCArrayBound$0x5a1db173$111$;

	// Token: 0x04000A82 RID: 2690 RVA: 0x00008184 File Offset: 0x00007584
	public static uint $ConstGCArrayBound$0x5a1db173$41$;

	// Token: 0x04000A83 RID: 2691 RVA: 0x000081E8 File Offset: 0x000075E8
	public static uint $ConstGCArrayBound$0x5a1db173$16$;

	// Token: 0x04000A84 RID: 2692 RVA: 0x00008110 File Offset: 0x00007510
	public static uint $ConstGCArrayBound$0x5a1db173$70$;

	// Token: 0x04000A85 RID: 2693 RVA: 0x00008214 File Offset: 0x00007614
	public static uint $ConstGCArrayBound$0x5a1db173$5$;

	// Token: 0x04000A86 RID: 2694 RVA: 0x000080E4 File Offset: 0x000074E4
	public static uint $ConstGCArrayBound$0x5a1db173$81$;

	// Token: 0x04000A87 RID: 2695 RVA: 0x000080BC File Offset: 0x000074BC
	public static uint $ConstGCArrayBound$0x5a1db173$91$;

	// Token: 0x04000A88 RID: 2696 RVA: 0x000081A4 File Offset: 0x000075A4
	public static uint $ConstGCArrayBound$0x5a1db173$33$;

	// Token: 0x04000A89 RID: 2697 RVA: 0x000080E8 File Offset: 0x000074E8
	public static uint $ConstGCArrayBound$0x5a1db173$80$;

	// Token: 0x04000A8A RID: 2698 RVA: 0x000081FC File Offset: 0x000075FC
	public static uint $ConstGCArrayBound$0x5a1db173$11$;

	// Token: 0x04000A8B RID: 2699 RVA: 0x00008150 File Offset: 0x00007550
	public static uint $ConstGCArrayBound$0x5a1db173$54$;

	// Token: 0x04000A8C RID: 2700 RVA: 0x00008140 File Offset: 0x00007540
	public static uint $ConstGCArrayBound$0x5a1db173$58$;

	// Token: 0x04000A8D RID: 2701 RVA: 0x0000809C File Offset: 0x0000749C
	public static uint $ConstGCArrayBound$0x5a1db173$99$;

	// Token: 0x04000A8E RID: 2702 RVA: 0x00008044 File Offset: 0x00007444
	public static uint $ConstGCArrayBound$0x5a1db173$121$;

	// Token: 0x04000A8F RID: 2703 RVA: 0x000080A8 File Offset: 0x000074A8
	public static uint $ConstGCArrayBound$0x5a1db173$96$;

	// Token: 0x04000A90 RID: 2704 RVA: 0x0000811C File Offset: 0x0000751C
	public static uint $ConstGCArrayBound$0x5a1db173$67$;

	// Token: 0x04000A91 RID: 2705 RVA: 0x000081A0 File Offset: 0x000075A0
	public static uint $ConstGCArrayBound$0x5a1db173$34$;

	// Token: 0x04000A92 RID: 2706 RVA: 0x00008188 File Offset: 0x00007588
	public static uint $ConstGCArrayBound$0x5a1db173$40$;

	// Token: 0x04000A93 RID: 2707 RVA: 0x00008208 File Offset: 0x00007608
	public static uint $ConstGCArrayBound$0x5a1db173$8$;

	// Token: 0x04000A94 RID: 2708 RVA: 0x00008114 File Offset: 0x00007514
	public static uint $ConstGCArrayBound$0x5a1db173$69$;

	// Token: 0x04000A95 RID: 2709 RVA: 0x000080FC File Offset: 0x000074FC
	public static uint $ConstGCArrayBound$0x5a1db173$75$;

	// Token: 0x04000A96 RID: 2710 RVA: 0x0000819C File Offset: 0x0000759C
	public static uint $ConstGCArrayBound$0x5a1db173$35$;

	// Token: 0x04000A97 RID: 2711 RVA: 0x000080AC File Offset: 0x000074AC
	public static uint $ConstGCArrayBound$0x5a1db173$95$;

	// Token: 0x04000A98 RID: 2712 RVA: 0x000081EC File Offset: 0x000075EC
	public static uint $ConstGCArrayBound$0x5a1db173$15$;

	// Token: 0x04000A99 RID: 2713 RVA: 0x000081B8 File Offset: 0x000075B8
	public static uint $ConstGCArrayBound$0x5a1db173$28$;

	// Token: 0x04000A9A RID: 2714 RVA: 0x0000818C File Offset: 0x0000758C
	public static uint $ConstGCArrayBound$0x5a1db173$39$;

	// Token: 0x04000A9B RID: 2715 RVA: 0x000081BC File Offset: 0x000075BC
	public static uint $ConstGCArrayBound$0x5a1db173$27$;

	// Token: 0x04000A9C RID: 2716 RVA: 0x0000817C File Offset: 0x0000757C
	public static uint $ConstGCArrayBound$0x5a1db173$43$;

	// Token: 0x04000A9D RID: 2717 RVA: 0x000081A8 File Offset: 0x000075A8
	public static uint $ConstGCArrayBound$0x5a1db173$32$;

	// Token: 0x04000A9E RID: 2718 RVA: 0x00008210 File Offset: 0x00007610
	public static uint $ConstGCArrayBound$0x5a1db173$6$;

	// Token: 0x04000A9F RID: 2719 RVA: 0x000081C8 File Offset: 0x000075C8
	public static uint $ConstGCArrayBound$0x5a1db173$24$;

	// Token: 0x04000AA0 RID: 2720 RVA: 0x000080D8 File Offset: 0x000074D8
	public static uint $ConstGCArrayBound$0x5a1db173$84$;

	// Token: 0x04000AA1 RID: 2721 RVA: 0x00008084 File Offset: 0x00007484
	public static uint $ConstGCArrayBound$0x5a1db173$105$;

	// Token: 0x04000AA2 RID: 2722 RVA: 0x00008120 File Offset: 0x00007520
	public static uint $ConstGCArrayBound$0x5a1db173$66$;

	// Token: 0x04000AA3 RID: 2723 RVA: 0x00008204 File Offset: 0x00007604
	public static uint $ConstGCArrayBound$0x5a1db173$9$;

	// Token: 0x04000AA4 RID: 2724 RVA: 0x00008194 File Offset: 0x00007594
	public static uint $ConstGCArrayBound$0x5a1db173$37$;

	// Token: 0x04000AA5 RID: 2725 RVA: 0x000080C8 File Offset: 0x000074C8
	public static uint $ConstGCArrayBound$0x5a1db173$88$;

	// Token: 0x04000AA6 RID: 2726 RVA: 0x00008174 File Offset: 0x00007574
	public static uint $ConstGCArrayBound$0x5a1db173$45$;

	// Token: 0x04000AA7 RID: 2727 RVA: 0x000081E4 File Offset: 0x000075E4
	public static uint $ConstGCArrayBound$0x5a1db173$17$;

	// Token: 0x04000AA8 RID: 2728 RVA: 0x00008094 File Offset: 0x00007494
	public static uint $ConstGCArrayBound$0x5a1db173$101$;

	// Token: 0x04000AA9 RID: 2729 RVA: 0x00008124 File Offset: 0x00007524
	public static uint $ConstGCArrayBound$0x5a1db173$65$;

	// Token: 0x04000AAA RID: 2730 RVA: 0x00008080 File Offset: 0x00007480
	public static uint $ConstGCArrayBound$0x5a1db173$106$;

	// Token: 0x04000AAB RID: 2731 RVA: 0x00008060 File Offset: 0x00007460
	public static uint $ConstGCArrayBound$0x5a1db173$114$;

	// Token: 0x04000AAC RID: 2732 RVA: 0x00008218 File Offset: 0x00007618
	public static uint $ConstGCArrayBound$0x5a1db173$4$;

	// Token: 0x04000AAD RID: 2733 RVA: 0x00008068 File Offset: 0x00007468
	public static uint $ConstGCArrayBound$0x5a1db173$112$;

	// Token: 0x04000AAE RID: 2734 RVA: 0x0000808C File Offset: 0x0000748C
	public static uint $ConstGCArrayBound$0x5a1db173$103$;

	// Token: 0x04000AAF RID: 2735 RVA: 0x000081DC File Offset: 0x000075DC
	public static uint $ConstGCArrayBound$0x5a1db173$19$;

	// Token: 0x04000AB0 RID: 2736 RVA: 0x000081F0 File Offset: 0x000075F0
	public static uint $ConstGCArrayBound$0x5a1db173$14$;

	// Token: 0x04000AB1 RID: 2737 RVA: 0x00008030 File Offset: 0x00007430
	public static uint $ConstGCArrayBound$0x5a1db173$126$;

	// Token: 0x04000AB2 RID: 2738 RVA: 0x0000816C File Offset: 0x0000756C
	public static uint $ConstGCArrayBound$0x5a1db173$47$;

	// Token: 0x04000AB3 RID: 2739 RVA: 0x000081CC File Offset: 0x000075CC
	public static uint $ConstGCArrayBound$0x5a1db173$23$;

	// Token: 0x04000AB4 RID: 2740 RVA: 0x0000807C File Offset: 0x0000747C
	public static uint $ConstGCArrayBound$0x5a1db173$107$;

	// Token: 0x04000AB5 RID: 2741 RVA: 0x000080D4 File Offset: 0x000074D4
	public static uint $ConstGCArrayBound$0x5a1db173$85$;

	// Token: 0x04000AB6 RID: 2742 RVA: 0x00008148 File Offset: 0x00007548
	public static uint $ConstGCArrayBound$0x5a1db173$56$;

	// Token: 0x04000AB7 RID: 2743 RVA: 0x00008224 File Offset: 0x00007624
	public static uint $ConstGCArrayBound$0x5a1db173$1$;

	// Token: 0x04000AB8 RID: 2744 RVA: 0x00008100 File Offset: 0x00007500
	public static uint $ConstGCArrayBound$0x5a1db173$74$;

	// Token: 0x04000AB9 RID: 2745 RVA: 0x000080B4 File Offset: 0x000074B4
	public static uint $ConstGCArrayBound$0x5a1db173$93$;

	// Token: 0x04000ABA RID: 2746 RVA: 0x00008130 File Offset: 0x00007530
	public static uint $ConstGCArrayBound$0x5a1db173$62$;

	// Token: 0x04000ABB RID: 2747 RVA: 0x00008164 File Offset: 0x00007564
	public static uint $ConstGCArrayBound$0x5a1db173$49$;

	// Token: 0x04000ABC RID: 2748 RVA: 0x000080DC File Offset: 0x000074DC
	public static uint $ConstGCArrayBound$0x5a1db173$83$;

	// Token: 0x04000ABD RID: 2749 RVA: 0x00008198 File Offset: 0x00007598
	public static uint $ConstGCArrayBound$0x5a1db173$36$;

	// Token: 0x04000ABE RID: 2750 RVA: 0x0000810C File Offset: 0x0000750C
	public static uint $ConstGCArrayBound$0x5a1db173$71$;

	// Token: 0x04000ABF RID: 2751 RVA: 0x000080D0 File Offset: 0x000074D0
	public static uint $ConstGCArrayBound$0x5a1db173$86$;

	// Token: 0x04000AC0 RID: 2752 RVA: 0x00008118 File Offset: 0x00007518
	public static uint $ConstGCArrayBound$0x5a1db173$68$;

	// Token: 0x04000AC1 RID: 2753 RVA: 0x000081B0 File Offset: 0x000075B0
	public static uint $ConstGCArrayBound$0x5a1db173$30$;

	// Token: 0x04000AC2 RID: 2754 RVA: 0x00008134 File Offset: 0x00007534
	public static uint $ConstGCArrayBound$0x5a1db173$61$;

	// Token: 0x04000AC3 RID: 2755 RVA: 0x000080B8 File Offset: 0x000074B8
	public static uint $ConstGCArrayBound$0x5a1db173$92$;

	// Token: 0x04000AC4 RID: 2756 RVA: 0x00008074 File Offset: 0x00007474
	public static uint $ConstGCArrayBound$0x5a1db173$109$;

	// Token: 0x04000AC5 RID: 2757 RVA: 0x00008108 File Offset: 0x00007508
	public static uint $ConstGCArrayBound$0x5a1db173$72$;

	// Token: 0x04000AC6 RID: 2758 RVA: 0x000080C4 File Offset: 0x000074C4
	public static uint $ConstGCArrayBound$0x5a1db173$89$;

	// Token: 0x04000AC7 RID: 2759 RVA: 0x00008128 File Offset: 0x00007528
	public static uint $ConstGCArrayBound$0x5a1db173$64$;

	// Token: 0x04000AC8 RID: 2760 RVA: 0x000081D4 File Offset: 0x000075D4
	public static uint $ConstGCArrayBound$0x5a1db173$21$;

	// Token: 0x04000AC9 RID: 2761 RVA: 0x00008058 File Offset: 0x00007458
	public static uint $ConstGCArrayBound$0x5a1db173$116$;

	// Token: 0x04000ACA RID: 2762 RVA: 0x0000802C File Offset: 0x0000742C
	public static uint $ConstGCArrayBound$0x5a1db173$127$;

	// Token: 0x04000ACB RID: 2763 RVA: 0x000081C0 File Offset: 0x000075C0
	public static uint $ConstGCArrayBound$0x5a1db173$26$;

	// Token: 0x04000ACC RID: 2764 RVA: 0x00008054 File Offset: 0x00007454
	public static uint $ConstGCArrayBound$0x5a1db173$117$;

	// Token: 0x04000ACD RID: 2765 RVA: 0x000080F8 File Offset: 0x000074F8
	public static uint $ConstGCArrayBound$0x5a1db173$76$;

	// Token: 0x04000ACE RID: 2766 RVA: 0x000081D0 File Offset: 0x000075D0
	public static uint $ConstGCArrayBound$0x5a1db173$22$;

	// Token: 0x04000ACF RID: 2767 RVA: 0x00008090 File Offset: 0x00007490
	public static uint $ConstGCArrayBound$0x5a1db173$102$;

	// Token: 0x04000AD0 RID: 2768 RVA: 0x000085DC File Offset: 0x000079DC
	public static uint $ConstGCArrayBound$0x8a911ce8$93$;

	// Token: 0x04000AD1 RID: 2769 RVA: 0x000085BC File Offset: 0x000079BC
	public static uint $ConstGCArrayBound$0x8a911ce8$101$;

	// Token: 0x04000AD2 RID: 2770 RVA: 0x00008578 File Offset: 0x00007978
	public static uint $ConstGCArrayBound$0x8a911ce8$118$;

	// Token: 0x04000AD3 RID: 2771 RVA: 0x00008570 File Offset: 0x00007970
	public static uint $ConstGCArrayBound$0x8a911ce8$120$;

	// Token: 0x04000AD4 RID: 2772 RVA: 0x000086D0 File Offset: 0x00007AD0
	public static uint $ConstGCArrayBound$0x8a911ce8$32$;

	// Token: 0x04000AD5 RID: 2773 RVA: 0x0000872C File Offset: 0x00007B2C
	public static uint $ConstGCArrayBound$0x8a911ce8$9$;

	// Token: 0x04000AD6 RID: 2774 RVA: 0x00008564 File Offset: 0x00007964
	public static uint $ConstGCArrayBound$0x8a911ce8$123$;

	// Token: 0x04000AD7 RID: 2775 RVA: 0x00008728 File Offset: 0x00007B28
	public static uint $ConstGCArrayBound$0x8a911ce8$10$;

	// Token: 0x04000AD8 RID: 2776 RVA: 0x000086BC File Offset: 0x00007ABC
	public static uint $ConstGCArrayBound$0x8a911ce8$37$;

	// Token: 0x04000AD9 RID: 2777 RVA: 0x00008668 File Offset: 0x00007A68
	public static uint $ConstGCArrayBound$0x8a911ce8$58$;

	// Token: 0x04000ADA RID: 2778 RVA: 0x000085F4 File Offset: 0x000079F4
	public static uint $ConstGCArrayBound$0x8a911ce8$87$;

	// Token: 0x04000ADB RID: 2779 RVA: 0x000086E8 File Offset: 0x00007AE8
	public static uint $ConstGCArrayBound$0x8a911ce8$26$;

	// Token: 0x04000ADC RID: 2780 RVA: 0x0000870C File Offset: 0x00007B0C
	public static uint $ConstGCArrayBound$0x8a911ce8$17$;

	// Token: 0x04000ADD RID: 2781 RVA: 0x00008658 File Offset: 0x00007A58
	public static uint $ConstGCArrayBound$0x8a911ce8$62$;

	// Token: 0x04000ADE RID: 2782 RVA: 0x00008644 File Offset: 0x00007A44
	public static uint $ConstGCArrayBound$0x8a911ce8$67$;

	// Token: 0x04000ADF RID: 2783 RVA: 0x000086E4 File Offset: 0x00007AE4
	public static uint $ConstGCArrayBound$0x8a911ce8$27$;

	// Token: 0x04000AE0 RID: 2784 RVA: 0x00008654 File Offset: 0x00007A54
	public static uint $ConstGCArrayBound$0x8a911ce8$63$;

	// Token: 0x04000AE1 RID: 2785 RVA: 0x000086D4 File Offset: 0x00007AD4
	public static uint $ConstGCArrayBound$0x8a911ce8$31$;

	// Token: 0x04000AE2 RID: 2786 RVA: 0x0000862C File Offset: 0x00007A2C
	public static uint $ConstGCArrayBound$0x8a911ce8$73$;

	// Token: 0x04000AE3 RID: 2787 RVA: 0x0000867C File Offset: 0x00007A7C
	public static uint $ConstGCArrayBound$0x8a911ce8$53$;

	// Token: 0x04000AE4 RID: 2788 RVA: 0x00008660 File Offset: 0x00007A60
	public static uint $ConstGCArrayBound$0x8a911ce8$60$;

	// Token: 0x04000AE5 RID: 2789 RVA: 0x000086A4 File Offset: 0x00007AA4
	public static uint $ConstGCArrayBound$0x8a911ce8$43$;

	// Token: 0x04000AE6 RID: 2790 RVA: 0x00008550 File Offset: 0x00007950
	public static uint $ConstGCArrayBound$0x8a911ce8$128$;

	// Token: 0x04000AE7 RID: 2791 RVA: 0x00008730 File Offset: 0x00007B30
	public static uint $ConstGCArrayBound$0x8a911ce8$8$;

	// Token: 0x04000AE8 RID: 2792 RVA: 0x000086C0 File Offset: 0x00007AC0
	public static uint $ConstGCArrayBound$0x8a911ce8$36$;

	// Token: 0x04000AE9 RID: 2793 RVA: 0x00008610 File Offset: 0x00007A10
	public static uint $ConstGCArrayBound$0x8a911ce8$80$;

	// Token: 0x04000AEA RID: 2794 RVA: 0x00008738 File Offset: 0x00007B38
	public static uint $ConstGCArrayBound$0x8a911ce8$6$;

	// Token: 0x04000AEB RID: 2795 RVA: 0x00008604 File Offset: 0x00007A04
	public static uint $ConstGCArrayBound$0x8a911ce8$83$;

	// Token: 0x04000AEC RID: 2796 RVA: 0x000086EC File Offset: 0x00007AEC
	public static uint $ConstGCArrayBound$0x8a911ce8$25$;

	// Token: 0x04000AED RID: 2797 RVA: 0x00008624 File Offset: 0x00007A24
	public static uint $ConstGCArrayBound$0x8a911ce8$75$;

	// Token: 0x04000AEE RID: 2798 RVA: 0x0000863C File Offset: 0x00007A3C
	public static uint $ConstGCArrayBound$0x8a911ce8$69$;

	// Token: 0x04000AEF RID: 2799 RVA: 0x0000868C File Offset: 0x00007A8C
	public static uint $ConstGCArrayBound$0x8a911ce8$49$;

	// Token: 0x04000AF0 RID: 2800 RVA: 0x000086D8 File Offset: 0x00007AD8
	public static uint $ConstGCArrayBound$0x8a911ce8$30$;

	// Token: 0x04000AF1 RID: 2801 RVA: 0x00008590 File Offset: 0x00007990
	public static uint $ConstGCArrayBound$0x8a911ce8$112$;

	// Token: 0x04000AF2 RID: 2802 RVA: 0x0000860C File Offset: 0x00007A0C
	public static uint $ConstGCArrayBound$0x8a911ce8$81$;

	// Token: 0x04000AF3 RID: 2803 RVA: 0x00008734 File Offset: 0x00007B34
	public static uint $ConstGCArrayBound$0x8a911ce8$7$;

	// Token: 0x04000AF4 RID: 2804 RVA: 0x000086F4 File Offset: 0x00007AF4
	public static uint $ConstGCArrayBound$0x8a911ce8$23$;

	// Token: 0x04000AF5 RID: 2805 RVA: 0x00008650 File Offset: 0x00007A50
	public static uint $ConstGCArrayBound$0x8a911ce8$64$;

	// Token: 0x04000AF6 RID: 2806 RVA: 0x00008694 File Offset: 0x00007A94
	public static uint $ConstGCArrayBound$0x8a911ce8$47$;

	// Token: 0x04000AF7 RID: 2807 RVA: 0x00008704 File Offset: 0x00007B04
	public static uint $ConstGCArrayBound$0x8a911ce8$19$;

	// Token: 0x04000AF8 RID: 2808 RVA: 0x00008594 File Offset: 0x00007994
	public static uint $ConstGCArrayBound$0x8a911ce8$111$;

	// Token: 0x04000AF9 RID: 2809 RVA: 0x000085E4 File Offset: 0x000079E4
	public static uint $ConstGCArrayBound$0x8a911ce8$91$;

	// Token: 0x04000AFA RID: 2810 RVA: 0x0000873C File Offset: 0x00007B3C
	public static uint $ConstGCArrayBound$0x8a911ce8$5$;

	// Token: 0x04000AFB RID: 2811 RVA: 0x00008608 File Offset: 0x00007A08
	public static uint $ConstGCArrayBound$0x8a911ce8$82$;

	// Token: 0x04000AFC RID: 2812 RVA: 0x0000871C File Offset: 0x00007B1C
	public static uint $ConstGCArrayBound$0x8a911ce8$13$;

	// Token: 0x04000AFD RID: 2813 RVA: 0x000085EC File Offset: 0x000079EC
	public static uint $ConstGCArrayBound$0x8a911ce8$89$;

	// Token: 0x04000AFE RID: 2814 RVA: 0x000086AC File Offset: 0x00007AAC
	public static uint $ConstGCArrayBound$0x8a911ce8$41$;

	// Token: 0x04000AFF RID: 2815 RVA: 0x000086C8 File Offset: 0x00007AC8
	public static uint $ConstGCArrayBound$0x8a911ce8$34$;

	// Token: 0x04000B00 RID: 2816 RVA: 0x000085C8 File Offset: 0x000079C8
	public static uint $ConstGCArrayBound$0x8a911ce8$98$;

	// Token: 0x04000B01 RID: 2817 RVA: 0x0000856C File Offset: 0x0000796C
	public static uint $ConstGCArrayBound$0x8a911ce8$121$;

	// Token: 0x04000B02 RID: 2818 RVA: 0x00008584 File Offset: 0x00007984
	public static uint $ConstGCArrayBound$0x8a911ce8$115$;

	// Token: 0x04000B03 RID: 2819 RVA: 0x0000858C File Offset: 0x0000798C
	public static uint $ConstGCArrayBound$0x8a911ce8$113$;

	// Token: 0x04000B04 RID: 2820 RVA: 0x000085A8 File Offset: 0x000079A8
	public static uint $ConstGCArrayBound$0x8a911ce8$106$;

	// Token: 0x04000B05 RID: 2821 RVA: 0x00008740 File Offset: 0x00007B40
	public static uint $ConstGCArrayBound$0x8a911ce8$4$;

	// Token: 0x04000B06 RID: 2822 RVA: 0x000086F0 File Offset: 0x00007AF0
	public static uint $ConstGCArrayBound$0x8a911ce8$24$;

	// Token: 0x04000B07 RID: 2823 RVA: 0x000085CC File Offset: 0x000079CC
	public static uint $ConstGCArrayBound$0x8a911ce8$97$;

	// Token: 0x04000B08 RID: 2824 RVA: 0x000085F8 File Offset: 0x000079F8
	public static uint $ConstGCArrayBound$0x8a911ce8$86$;

	// Token: 0x04000B09 RID: 2825 RVA: 0x000085F0 File Offset: 0x000079F0
	public static uint $ConstGCArrayBound$0x8a911ce8$88$;

	// Token: 0x04000B0A RID: 2826 RVA: 0x00008554 File Offset: 0x00007954
	public static uint $ConstGCArrayBound$0x8a911ce8$127$;

	// Token: 0x04000B0B RID: 2827 RVA: 0x00008718 File Offset: 0x00007B18
	public static uint $ConstGCArrayBound$0x8a911ce8$14$;

	// Token: 0x04000B0C RID: 2828 RVA: 0x0000864C File Offset: 0x00007A4C
	public static uint $ConstGCArrayBound$0x8a911ce8$65$;

	// Token: 0x04000B0D RID: 2829 RVA: 0x000086CC File Offset: 0x00007ACC
	public static uint $ConstGCArrayBound$0x8a911ce8$33$;

	// Token: 0x04000B0E RID: 2830 RVA: 0x00008684 File Offset: 0x00007A84
	public static uint $ConstGCArrayBound$0x8a911ce8$51$;

	// Token: 0x04000B0F RID: 2831 RVA: 0x0000855C File Offset: 0x0000795C
	public static uint $ConstGCArrayBound$0x8a911ce8$125$;

	// Token: 0x04000B10 RID: 2832 RVA: 0x000085C4 File Offset: 0x000079C4
	public static uint $ConstGCArrayBound$0x8a911ce8$99$;

	// Token: 0x04000B11 RID: 2833 RVA: 0x000085D8 File Offset: 0x000079D8
	public static uint $ConstGCArrayBound$0x8a911ce8$94$;

	// Token: 0x04000B12 RID: 2834 RVA: 0x00008580 File Offset: 0x00007980
	public static uint $ConstGCArrayBound$0x8a911ce8$116$;

	// Token: 0x04000B13 RID: 2835 RVA: 0x0000866C File Offset: 0x00007A6C
	public static uint $ConstGCArrayBound$0x8a911ce8$57$;

	// Token: 0x04000B14 RID: 2836 RVA: 0x0000861C File Offset: 0x00007A1C
	public static uint $ConstGCArrayBound$0x8a911ce8$77$;

	// Token: 0x04000B15 RID: 2837 RVA: 0x000085D0 File Offset: 0x000079D0
	public static uint $ConstGCArrayBound$0x8a911ce8$96$;

	// Token: 0x04000B16 RID: 2838 RVA: 0x000085C0 File Offset: 0x000079C0
	public static uint $ConstGCArrayBound$0x8a911ce8$100$;

	// Token: 0x04000B17 RID: 2839 RVA: 0x00008744 File Offset: 0x00007B44
	public static uint $ConstGCArrayBound$0x8a911ce8$3$;

	// Token: 0x04000B18 RID: 2840 RVA: 0x00008634 File Offset: 0x00007A34
	public static uint $ConstGCArrayBound$0x8a911ce8$71$;

	// Token: 0x04000B19 RID: 2841 RVA: 0x000085E0 File Offset: 0x000079E0
	public static uint $ConstGCArrayBound$0x8a911ce8$92$;

	// Token: 0x04000B1A RID: 2842 RVA: 0x00008600 File Offset: 0x00007A00
	public static uint $ConstGCArrayBound$0x8a911ce8$84$;

	// Token: 0x04000B1B RID: 2843 RVA: 0x00008568 File Offset: 0x00007968
	public static uint $ConstGCArrayBound$0x8a911ce8$122$;

	// Token: 0x04000B1C RID: 2844 RVA: 0x000086DC File Offset: 0x00007ADC
	public static uint $ConstGCArrayBound$0x8a911ce8$29$;

	// Token: 0x04000B1D RID: 2845 RVA: 0x00008628 File Offset: 0x00007A28
	public static uint $ConstGCArrayBound$0x8a911ce8$74$;

	// Token: 0x04000B1E RID: 2846 RVA: 0x00008614 File Offset: 0x00007A14
	public static uint $ConstGCArrayBound$0x8a911ce8$79$;

	// Token: 0x04000B1F RID: 2847 RVA: 0x00008648 File Offset: 0x00007A48
	public static uint $ConstGCArrayBound$0x8a911ce8$66$;

	// Token: 0x04000B20 RID: 2848 RVA: 0x000086B0 File Offset: 0x00007AB0
	public static uint $ConstGCArrayBound$0x8a911ce8$40$;

	// Token: 0x04000B21 RID: 2849 RVA: 0x00008710 File Offset: 0x00007B10
	public static uint $ConstGCArrayBound$0x8a911ce8$16$;

	// Token: 0x04000B22 RID: 2850 RVA: 0x000086A8 File Offset: 0x00007AA8
	public static uint $ConstGCArrayBound$0x8a911ce8$42$;

	// Token: 0x04000B23 RID: 2851 RVA: 0x00008560 File Offset: 0x00007960
	public static uint $ConstGCArrayBound$0x8a911ce8$124$;

	// Token: 0x04000B24 RID: 2852 RVA: 0x00008640 File Offset: 0x00007A40
	public static uint $ConstGCArrayBound$0x8a911ce8$68$;

	// Token: 0x04000B25 RID: 2853 RVA: 0x00008748 File Offset: 0x00007B48
	public static uint $ConstGCArrayBound$0x8a911ce8$2$;

	// Token: 0x04000B26 RID: 2854 RVA: 0x00008664 File Offset: 0x00007A64
	public static uint $ConstGCArrayBound$0x8a911ce8$59$;

	// Token: 0x04000B27 RID: 2855 RVA: 0x00008558 File Offset: 0x00007958
	public static uint $ConstGCArrayBound$0x8a911ce8$126$;

	// Token: 0x04000B28 RID: 2856 RVA: 0x000085A0 File Offset: 0x000079A0
	public static uint $ConstGCArrayBound$0x8a911ce8$108$;

	// Token: 0x04000B29 RID: 2857 RVA: 0x00008678 File Offset: 0x00007A78
	public static uint $ConstGCArrayBound$0x8a911ce8$54$;

	// Token: 0x04000B2A RID: 2858 RVA: 0x00008670 File Offset: 0x00007A70
	public static uint $ConstGCArrayBound$0x8a911ce8$56$;

	// Token: 0x04000B2B RID: 2859 RVA: 0x000086A0 File Offset: 0x00007AA0
	public static uint $ConstGCArrayBound$0x8a911ce8$44$;

	// Token: 0x04000B2C RID: 2860 RVA: 0x000085B4 File Offset: 0x000079B4
	public static uint $ConstGCArrayBound$0x8a911ce8$103$;

	// Token: 0x04000B2D RID: 2861 RVA: 0x0000874C File Offset: 0x00007B4C
	public static uint $ConstGCArrayBound$0x8a911ce8$1$;

	// Token: 0x04000B2E RID: 2862 RVA: 0x00008720 File Offset: 0x00007B20
	public static uint $ConstGCArrayBound$0x8a911ce8$12$;

	// Token: 0x04000B2F RID: 2863 RVA: 0x000086C4 File Offset: 0x00007AC4
	public static uint $ConstGCArrayBound$0x8a911ce8$35$;

	// Token: 0x04000B30 RID: 2864 RVA: 0x00008620 File Offset: 0x00007A20
	public static uint $ConstGCArrayBound$0x8a911ce8$76$;

	// Token: 0x04000B31 RID: 2865 RVA: 0x00008700 File Offset: 0x00007B00
	public static uint $ConstGCArrayBound$0x8a911ce8$20$;

	// Token: 0x04000B32 RID: 2866 RVA: 0x00008630 File Offset: 0x00007A30
	public static uint $ConstGCArrayBound$0x8a911ce8$72$;

	// Token: 0x04000B33 RID: 2867 RVA: 0x00008680 File Offset: 0x00007A80
	public static uint $ConstGCArrayBound$0x8a911ce8$52$;

	// Token: 0x04000B34 RID: 2868 RVA: 0x000086E0 File Offset: 0x00007AE0
	public static uint $ConstGCArrayBound$0x8a911ce8$28$;

	// Token: 0x04000B35 RID: 2869 RVA: 0x0000865C File Offset: 0x00007A5C
	public static uint $ConstGCArrayBound$0x8a911ce8$61$;

	// Token: 0x04000B36 RID: 2870 RVA: 0x00008688 File Offset: 0x00007A88
	public static uint $ConstGCArrayBound$0x8a911ce8$50$;

	// Token: 0x04000B37 RID: 2871 RVA: 0x00008674 File Offset: 0x00007A74
	public static uint $ConstGCArrayBound$0x8a911ce8$55$;

	// Token: 0x04000B38 RID: 2872 RVA: 0x00008724 File Offset: 0x00007B24
	public static uint $ConstGCArrayBound$0x8a911ce8$11$;

	// Token: 0x04000B39 RID: 2873 RVA: 0x00008708 File Offset: 0x00007B08
	public static uint $ConstGCArrayBound$0x8a911ce8$18$;

	// Token: 0x04000B3A RID: 2874 RVA: 0x0000857C File Offset: 0x0000797C
	public static uint $ConstGCArrayBound$0x8a911ce8$117$;

	// Token: 0x04000B3B RID: 2875 RVA: 0x00008690 File Offset: 0x00007A90
	public static uint $ConstGCArrayBound$0x8a911ce8$48$;

	// Token: 0x04000B3C RID: 2876 RVA: 0x00008638 File Offset: 0x00007A38
	public static uint $ConstGCArrayBound$0x8a911ce8$70$;

	// Token: 0x04000B3D RID: 2877 RVA: 0x000086B4 File Offset: 0x00007AB4
	public static uint $ConstGCArrayBound$0x8a911ce8$39$;

	// Token: 0x04000B3E RID: 2878 RVA: 0x000086FC File Offset: 0x00007AFC
	public static uint $ConstGCArrayBound$0x8a911ce8$21$;

	// Token: 0x04000B3F RID: 2879 RVA: 0x000085E8 File Offset: 0x000079E8
	public static uint $ConstGCArrayBound$0x8a911ce8$90$;

	// Token: 0x04000B40 RID: 2880 RVA: 0x000085AC File Offset: 0x000079AC
	public static uint $ConstGCArrayBound$0x8a911ce8$105$;

	// Token: 0x04000B41 RID: 2881 RVA: 0x000085A4 File Offset: 0x000079A4
	public static uint $ConstGCArrayBound$0x8a911ce8$107$;

	// Token: 0x04000B42 RID: 2882 RVA: 0x00008618 File Offset: 0x00007A18
	public static uint $ConstGCArrayBound$0x8a911ce8$78$;

	// Token: 0x04000B43 RID: 2883 RVA: 0x0000859C File Offset: 0x0000799C
	public static uint $ConstGCArrayBound$0x8a911ce8$109$;

	// Token: 0x04000B44 RID: 2884 RVA: 0x000085D4 File Offset: 0x000079D4
	public static uint $ConstGCArrayBound$0x8a911ce8$95$;

	// Token: 0x04000B45 RID: 2885 RVA: 0x000085B8 File Offset: 0x000079B8
	public static uint $ConstGCArrayBound$0x8a911ce8$102$;

	// Token: 0x04000B46 RID: 2886 RVA: 0x00008588 File Offset: 0x00007988
	public static uint $ConstGCArrayBound$0x8a911ce8$114$;

	// Token: 0x04000B47 RID: 2887 RVA: 0x000085FC File Offset: 0x000079FC
	public static uint $ConstGCArrayBound$0x8a911ce8$85$;

	// Token: 0x04000B48 RID: 2888 RVA: 0x00008598 File Offset: 0x00007998
	public static uint $ConstGCArrayBound$0x8a911ce8$110$;

	// Token: 0x04000B49 RID: 2889 RVA: 0x0000869C File Offset: 0x00007A9C
	public static uint $ConstGCArrayBound$0x8a911ce8$45$;

	// Token: 0x04000B4A RID: 2890 RVA: 0x000086F8 File Offset: 0x00007AF8
	public static uint $ConstGCArrayBound$0x8a911ce8$22$;

	// Token: 0x04000B4B RID: 2891 RVA: 0x00008698 File Offset: 0x00007A98
	public static uint $ConstGCArrayBound$0x8a911ce8$46$;

	// Token: 0x04000B4C RID: 2892 RVA: 0x000086B8 File Offset: 0x00007AB8
	public static uint $ConstGCArrayBound$0x8a911ce8$38$;

	// Token: 0x04000B4D RID: 2893 RVA: 0x00008714 File Offset: 0x00007B14
	public static uint $ConstGCArrayBound$0x8a911ce8$15$;

	// Token: 0x04000B4E RID: 2894 RVA: 0x00008574 File Offset: 0x00007974
	public static uint $ConstGCArrayBound$0x8a911ce8$119$;

	// Token: 0x04000B4F RID: 2895 RVA: 0x000085B0 File Offset: 0x000079B0
	public static uint $ConstGCArrayBound$0x8a911ce8$104$;

	// Token: 0x04000B50 RID: 2896 RVA: 0x00008B74 File Offset: 0x00007F74
	public static uint $ConstGCArrayBound$0x19084f87$65$;

	// Token: 0x04000B51 RID: 2897 RVA: 0x00008C50 File Offset: 0x00008050
	public static uint $ConstGCArrayBound$0x19084f87$10$;

	// Token: 0x04000B52 RID: 2898 RVA: 0x00008C00 File Offset: 0x00008000
	public static uint $ConstGCArrayBound$0x19084f87$30$;

	// Token: 0x04000B53 RID: 2899 RVA: 0x00008BF8 File Offset: 0x00007FF8
	public static uint $ConstGCArrayBound$0x19084f87$32$;

	// Token: 0x04000B54 RID: 2900 RVA: 0x00008B04 File Offset: 0x00007F04
	public static uint $ConstGCArrayBound$0x19084f87$93$;

	// Token: 0x04000B55 RID: 2901 RVA: 0x00008B1C File Offset: 0x00007F1C
	public static uint $ConstGCArrayBound$0x19084f87$87$;

	// Token: 0x04000B56 RID: 2902 RVA: 0x00008B08 File Offset: 0x00007F08
	public static uint $ConstGCArrayBound$0x19084f87$92$;

	// Token: 0x04000B57 RID: 2903 RVA: 0x00008B5C File Offset: 0x00007F5C
	public static uint $ConstGCArrayBound$0x19084f87$71$;

	// Token: 0x04000B58 RID: 2904 RVA: 0x00008C64 File Offset: 0x00008064
	public static uint $ConstGCArrayBound$0x19084f87$5$;

	// Token: 0x04000B59 RID: 2905 RVA: 0x00008A78 File Offset: 0x00007E78
	public static uint $ConstGCArrayBound$0x19084f87$128$;

	// Token: 0x04000B5A RID: 2906 RVA: 0x00008BE8 File Offset: 0x00007FE8
	public static uint $ConstGCArrayBound$0x19084f87$36$;

	// Token: 0x04000B5B RID: 2907 RVA: 0x00008B70 File Offset: 0x00007F70
	public static uint $ConstGCArrayBound$0x19084f87$66$;

	// Token: 0x04000B5C RID: 2908 RVA: 0x00008C4C File Offset: 0x0000804C
	public static uint $ConstGCArrayBound$0x19084f87$11$;

	// Token: 0x04000B5D RID: 2909 RVA: 0x00008C08 File Offset: 0x00008008
	public static uint $ConstGCArrayBound$0x19084f87$28$;

	// Token: 0x04000B5E RID: 2910 RVA: 0x00008C6C File Offset: 0x0000806C
	public static uint $ConstGCArrayBound$0x19084f87$3$;

	// Token: 0x04000B5F RID: 2911 RVA: 0x00008B9C File Offset: 0x00007F9C
	public static uint $ConstGCArrayBound$0x19084f87$55$;

	// Token: 0x04000B60 RID: 2912 RVA: 0x00008A84 File Offset: 0x00007E84
	public static uint $ConstGCArrayBound$0x19084f87$125$;

	// Token: 0x04000B61 RID: 2913 RVA: 0x00008ADC File Offset: 0x00007EDC
	public static uint $ConstGCArrayBound$0x19084f87$103$;

	// Token: 0x04000B62 RID: 2914 RVA: 0x00008B10 File Offset: 0x00007F10
	public static uint $ConstGCArrayBound$0x19084f87$90$;

	// Token: 0x04000B63 RID: 2915 RVA: 0x00008BAC File Offset: 0x00007FAC
	public static uint $ConstGCArrayBound$0x19084f87$51$;

	// Token: 0x04000B64 RID: 2916 RVA: 0x00008AE4 File Offset: 0x00007EE4
	public static uint $ConstGCArrayBound$0x19084f87$101$;

	// Token: 0x04000B65 RID: 2917 RVA: 0x00008BE0 File Offset: 0x00007FE0
	public static uint $ConstGCArrayBound$0x19084f87$38$;

	// Token: 0x04000B66 RID: 2918 RVA: 0x00008B84 File Offset: 0x00007F84
	public static uint $ConstGCArrayBound$0x19084f87$61$;

	// Token: 0x04000B67 RID: 2919 RVA: 0x00008B2C File Offset: 0x00007F2C
	public static uint $ConstGCArrayBound$0x19084f87$83$;

	// Token: 0x04000B68 RID: 2920 RVA: 0x00008C74 File Offset: 0x00008074
	public static uint $ConstGCArrayBound$0x19084f87$1$;

	// Token: 0x04000B69 RID: 2921 RVA: 0x00008B34 File Offset: 0x00007F34
	public static uint $ConstGCArrayBound$0x19084f87$81$;

	// Token: 0x04000B6A RID: 2922 RVA: 0x00008C10 File Offset: 0x00008010
	public static uint $ConstGCArrayBound$0x19084f87$26$;

	// Token: 0x04000B6B RID: 2923 RVA: 0x00008B40 File Offset: 0x00007F40
	public static uint $ConstGCArrayBound$0x19084f87$78$;

	// Token: 0x04000B6C RID: 2924 RVA: 0x00008B88 File Offset: 0x00007F88
	public static uint $ConstGCArrayBound$0x19084f87$60$;

	// Token: 0x04000B6D RID: 2925 RVA: 0x00008B80 File Offset: 0x00007F80
	public static uint $ConstGCArrayBound$0x19084f87$62$;

	// Token: 0x04000B6E RID: 2926 RVA: 0x00008C18 File Offset: 0x00008018
	public static uint $ConstGCArrayBound$0x19084f87$24$;

	// Token: 0x04000B6F RID: 2927 RVA: 0x00008BEC File Offset: 0x00007FEC
	public static uint $ConstGCArrayBound$0x19084f87$35$;

	// Token: 0x04000B70 RID: 2928 RVA: 0x00008B20 File Offset: 0x00007F20
	public static uint $ConstGCArrayBound$0x19084f87$86$;

	// Token: 0x04000B71 RID: 2929 RVA: 0x00008C5C File Offset: 0x0000805C
	public static uint $ConstGCArrayBound$0x19084f87$7$;

	// Token: 0x04000B72 RID: 2930 RVA: 0x00008AE0 File Offset: 0x00007EE0
	public static uint $ConstGCArrayBound$0x19084f87$102$;

	// Token: 0x04000B73 RID: 2931 RVA: 0x00008B48 File Offset: 0x00007F48
	public static uint $ConstGCArrayBound$0x19084f87$76$;

	// Token: 0x04000B74 RID: 2932 RVA: 0x00008BCC File Offset: 0x00007FCC
	public static uint $ConstGCArrayBound$0x19084f87$43$;

	// Token: 0x04000B75 RID: 2933 RVA: 0x00008C48 File Offset: 0x00008048
	public static uint $ConstGCArrayBound$0x19084f87$12$;

	// Token: 0x04000B76 RID: 2934 RVA: 0x00008BFC File Offset: 0x00007FFC
	public static uint $ConstGCArrayBound$0x19084f87$31$;

	// Token: 0x04000B77 RID: 2935 RVA: 0x00008B58 File Offset: 0x00007F58
	public static uint $ConstGCArrayBound$0x19084f87$72$;

	// Token: 0x04000B78 RID: 2936 RVA: 0x00008AF4 File Offset: 0x00007EF4
	public static uint $ConstGCArrayBound$0x19084f87$97$;

	// Token: 0x04000B79 RID: 2937 RVA: 0x00008AEC File Offset: 0x00007EEC
	public static uint $ConstGCArrayBound$0x19084f87$99$;

	// Token: 0x04000B7A RID: 2938 RVA: 0x00008A9C File Offset: 0x00007E9C
	public static uint $ConstGCArrayBound$0x19084f87$119$;

	// Token: 0x04000B7B RID: 2939 RVA: 0x00008C0C File Offset: 0x0000800C
	public static uint $ConstGCArrayBound$0x19084f87$27$;

	// Token: 0x04000B7C RID: 2940 RVA: 0x00008C44 File Offset: 0x00008044
	public static uint $ConstGCArrayBound$0x19084f87$13$;

	// Token: 0x04000B7D RID: 2941 RVA: 0x00008C58 File Offset: 0x00008058
	public static uint $ConstGCArrayBound$0x19084f87$8$;

	// Token: 0x04000B7E RID: 2942 RVA: 0x00008C1C File Offset: 0x0000801C
	public static uint $ConstGCArrayBound$0x19084f87$23$;

	// Token: 0x04000B7F RID: 2943 RVA: 0x00008BD4 File Offset: 0x00007FD4
	public static uint $ConstGCArrayBound$0x19084f87$41$;

	// Token: 0x04000B80 RID: 2944 RVA: 0x00008C2C File Offset: 0x0000802C
	public static uint $ConstGCArrayBound$0x19084f87$19$;

	// Token: 0x04000B81 RID: 2945 RVA: 0x00008AA8 File Offset: 0x00007EA8
	public static uint $ConstGCArrayBound$0x19084f87$116$;

	// Token: 0x04000B82 RID: 2946 RVA: 0x00008BE4 File Offset: 0x00007FE4
	public static uint $ConstGCArrayBound$0x19084f87$37$;

	// Token: 0x04000B83 RID: 2947 RVA: 0x00008C24 File Offset: 0x00008024
	public static uint $ConstGCArrayBound$0x19084f87$21$;

	// Token: 0x04000B84 RID: 2948 RVA: 0x00008C30 File Offset: 0x00008030
	public static uint $ConstGCArrayBound$0x19084f87$18$;

	// Token: 0x04000B85 RID: 2949 RVA: 0x00008BB8 File Offset: 0x00007FB8
	public static uint $ConstGCArrayBound$0x19084f87$48$;

	// Token: 0x04000B86 RID: 2950 RVA: 0x00008B50 File Offset: 0x00007F50
	public static uint $ConstGCArrayBound$0x19084f87$74$;

	// Token: 0x04000B87 RID: 2951 RVA: 0x00008A88 File Offset: 0x00007E88
	public static uint $ConstGCArrayBound$0x19084f87$124$;

	// Token: 0x04000B88 RID: 2952 RVA: 0x00008B90 File Offset: 0x00007F90
	public static uint $ConstGCArrayBound$0x19084f87$58$;

	// Token: 0x04000B89 RID: 2953 RVA: 0x00008B6C File Offset: 0x00007F6C
	public static uint $ConstGCArrayBound$0x19084f87$67$;

	// Token: 0x04000B8A RID: 2954 RVA: 0x00008A98 File Offset: 0x00007E98
	public static uint $ConstGCArrayBound$0x19084f87$120$;

	// Token: 0x04000B8B RID: 2955 RVA: 0x00008AB4 File Offset: 0x00007EB4
	public static uint $ConstGCArrayBound$0x19084f87$113$;

	// Token: 0x04000B8C RID: 2956 RVA: 0x00008AE8 File Offset: 0x00007EE8
	public static uint $ConstGCArrayBound$0x19084f87$100$;

	// Token: 0x04000B8D RID: 2957 RVA: 0x00008A8C File Offset: 0x00007E8C
	public static uint $ConstGCArrayBound$0x19084f87$123$;

	// Token: 0x04000B8E RID: 2958 RVA: 0x00008BC0 File Offset: 0x00007FC0
	public static uint $ConstGCArrayBound$0x19084f87$46$;

	// Token: 0x04000B8F RID: 2959 RVA: 0x00008B3C File Offset: 0x00007F3C
	public static uint $ConstGCArrayBound$0x19084f87$79$;

	// Token: 0x04000B90 RID: 2960 RVA: 0x00008C38 File Offset: 0x00008038
	public static uint $ConstGCArrayBound$0x19084f87$16$;

	// Token: 0x04000B91 RID: 2961 RVA: 0x00008AC4 File Offset: 0x00007EC4
	public static uint $ConstGCArrayBound$0x19084f87$109$;

	// Token: 0x04000B92 RID: 2962 RVA: 0x00008C20 File Offset: 0x00008020
	public static uint $ConstGCArrayBound$0x19084f87$22$;

	// Token: 0x04000B93 RID: 2963 RVA: 0x00008AD4 File Offset: 0x00007ED4
	public static uint $ConstGCArrayBound$0x19084f87$105$;

	// Token: 0x04000B94 RID: 2964 RVA: 0x00008C28 File Offset: 0x00008028
	public static uint $ConstGCArrayBound$0x19084f87$20$;

	// Token: 0x04000B95 RID: 2965 RVA: 0x00008BF0 File Offset: 0x00007FF0
	public static uint $ConstGCArrayBound$0x19084f87$34$;

	// Token: 0x04000B96 RID: 2966 RVA: 0x00008AD0 File Offset: 0x00007ED0
	public static uint $ConstGCArrayBound$0x19084f87$106$;

	// Token: 0x04000B97 RID: 2967 RVA: 0x00008B0C File Offset: 0x00007F0C
	public static uint $ConstGCArrayBound$0x19084f87$91$;

	// Token: 0x04000B98 RID: 2968 RVA: 0x00008A94 File Offset: 0x00007E94
	public static uint $ConstGCArrayBound$0x19084f87$121$;

	// Token: 0x04000B99 RID: 2969 RVA: 0x00008B4C File Offset: 0x00007F4C
	public static uint $ConstGCArrayBound$0x19084f87$75$;

	// Token: 0x04000B9A RID: 2970 RVA: 0x00008B94 File Offset: 0x00007F94
	public static uint $ConstGCArrayBound$0x19084f87$57$;

	// Token: 0x04000B9B RID: 2971 RVA: 0x00008B00 File Offset: 0x00007F00
	public static uint $ConstGCArrayBound$0x19084f87$94$;

	// Token: 0x04000B9C RID: 2972 RVA: 0x00008BDC File Offset: 0x00007FDC
	public static uint $ConstGCArrayBound$0x19084f87$39$;

	// Token: 0x04000B9D RID: 2973 RVA: 0x00008B30 File Offset: 0x00007F30
	public static uint $ConstGCArrayBound$0x19084f87$82$;

	// Token: 0x04000B9E RID: 2974 RVA: 0x00008AA0 File Offset: 0x00007EA0
	public static uint $ConstGCArrayBound$0x19084f87$118$;

	// Token: 0x04000B9F RID: 2975 RVA: 0x00008B14 File Offset: 0x00007F14
	public static uint $ConstGCArrayBound$0x19084f87$89$;

	// Token: 0x04000BA0 RID: 2976 RVA: 0x00008A90 File Offset: 0x00007E90
	public static uint $ConstGCArrayBound$0x19084f87$122$;

	// Token: 0x04000BA1 RID: 2977 RVA: 0x00008B54 File Offset: 0x00007F54
	public static uint $ConstGCArrayBound$0x19084f87$73$;

	// Token: 0x04000BA2 RID: 2978 RVA: 0x00008ACC File Offset: 0x00007ECC
	public static uint $ConstGCArrayBound$0x19084f87$107$;

	// Token: 0x04000BA3 RID: 2979 RVA: 0x00008B68 File Offset: 0x00007F68
	public static uint $ConstGCArrayBound$0x19084f87$68$;

	// Token: 0x04000BA4 RID: 2980 RVA: 0x00008AF8 File Offset: 0x00007EF8
	public static uint $ConstGCArrayBound$0x19084f87$96$;

	// Token: 0x04000BA5 RID: 2981 RVA: 0x00008B18 File Offset: 0x00007F18
	public static uint $ConstGCArrayBound$0x19084f87$88$;

	// Token: 0x04000BA6 RID: 2982 RVA: 0x00008B28 File Offset: 0x00007F28
	public static uint $ConstGCArrayBound$0x19084f87$84$;

	// Token: 0x04000BA7 RID: 2983 RVA: 0x00008AB8 File Offset: 0x00007EB8
	public static uint $ConstGCArrayBound$0x19084f87$112$;

	// Token: 0x04000BA8 RID: 2984 RVA: 0x00008B8C File Offset: 0x00007F8C
	public static uint $ConstGCArrayBound$0x19084f87$59$;

	// Token: 0x04000BA9 RID: 2985 RVA: 0x00008A80 File Offset: 0x00007E80
	public static uint $ConstGCArrayBound$0x19084f87$126$;

	// Token: 0x04000BAA RID: 2986 RVA: 0x00008BC8 File Offset: 0x00007FC8
	public static uint $ConstGCArrayBound$0x19084f87$44$;

	// Token: 0x04000BAB RID: 2987 RVA: 0x00008B44 File Offset: 0x00007F44
	public static uint $ConstGCArrayBound$0x19084f87$77$;

	// Token: 0x04000BAC RID: 2988 RVA: 0x00008AD8 File Offset: 0x00007ED8
	public static uint $ConstGCArrayBound$0x19084f87$104$;

	// Token: 0x04000BAD RID: 2989 RVA: 0x00008ABC File Offset: 0x00007EBC
	public static uint $ConstGCArrayBound$0x19084f87$111$;

	// Token: 0x04000BAE RID: 2990 RVA: 0x00008B7C File Offset: 0x00007F7C
	public static uint $ConstGCArrayBound$0x19084f87$63$;

	// Token: 0x04000BAF RID: 2991 RVA: 0x00008C60 File Offset: 0x00008060
	public static uint $ConstGCArrayBound$0x19084f87$6$;

	// Token: 0x04000BB0 RID: 2992 RVA: 0x00008B38 File Offset: 0x00007F38
	public static uint $ConstGCArrayBound$0x19084f87$80$;

	// Token: 0x04000BB1 RID: 2993 RVA: 0x00008C34 File Offset: 0x00008034
	public static uint $ConstGCArrayBound$0x19084f87$17$;

	// Token: 0x04000BB2 RID: 2994 RVA: 0x00008BB0 File Offset: 0x00007FB0
	public static uint $ConstGCArrayBound$0x19084f87$50$;

	// Token: 0x04000BB3 RID: 2995 RVA: 0x00008B64 File Offset: 0x00007F64
	public static uint $ConstGCArrayBound$0x19084f87$69$;

	// Token: 0x04000BB4 RID: 2996 RVA: 0x00008BF4 File Offset: 0x00007FF4
	public static uint $ConstGCArrayBound$0x19084f87$33$;

	// Token: 0x04000BB5 RID: 2997 RVA: 0x00008B24 File Offset: 0x00007F24
	public static uint $ConstGCArrayBound$0x19084f87$85$;

	// Token: 0x04000BB6 RID: 2998 RVA: 0x00008AA4 File Offset: 0x00007EA4
	public static uint $ConstGCArrayBound$0x19084f87$117$;

	// Token: 0x04000BB7 RID: 2999 RVA: 0x00008BA4 File Offset: 0x00007FA4
	public static uint $ConstGCArrayBound$0x19084f87$53$;

	// Token: 0x04000BB8 RID: 3000 RVA: 0x00008BA8 File Offset: 0x00007FA8
	public static uint $ConstGCArrayBound$0x19084f87$52$;

	// Token: 0x04000BB9 RID: 3001 RVA: 0x00008C70 File Offset: 0x00008070
	public static uint $ConstGCArrayBound$0x19084f87$2$;

	// Token: 0x04000BBA RID: 3002 RVA: 0x00008C04 File Offset: 0x00008004
	public static uint $ConstGCArrayBound$0x19084f87$29$;

	// Token: 0x04000BBB RID: 3003 RVA: 0x00008BB4 File Offset: 0x00007FB4
	public static uint $ConstGCArrayBound$0x19084f87$49$;

	// Token: 0x04000BBC RID: 3004 RVA: 0x00008B98 File Offset: 0x00007F98
	public static uint $ConstGCArrayBound$0x19084f87$56$;

	// Token: 0x04000BBD RID: 3005 RVA: 0x00008AC8 File Offset: 0x00007EC8
	public static uint $ConstGCArrayBound$0x19084f87$108$;

	// Token: 0x04000BBE RID: 3006 RVA: 0x00008AAC File Offset: 0x00007EAC
	public static uint $ConstGCArrayBound$0x19084f87$115$;

	// Token: 0x04000BBF RID: 3007 RVA: 0x00008BBC File Offset: 0x00007FBC
	public static uint $ConstGCArrayBound$0x19084f87$47$;

	// Token: 0x04000BC0 RID: 3008 RVA: 0x00008BD0 File Offset: 0x00007FD0
	public static uint $ConstGCArrayBound$0x19084f87$42$;

	// Token: 0x04000BC1 RID: 3009 RVA: 0x00008BA0 File Offset: 0x00007FA0
	public static uint $ConstGCArrayBound$0x19084f87$54$;

	// Token: 0x04000BC2 RID: 3010 RVA: 0x00008B60 File Offset: 0x00007F60
	public static uint $ConstGCArrayBound$0x19084f87$70$;

	// Token: 0x04000BC3 RID: 3011 RVA: 0x00008C14 File Offset: 0x00008014
	public static uint $ConstGCArrayBound$0x19084f87$25$;

	// Token: 0x04000BC4 RID: 3012 RVA: 0x00008AC0 File Offset: 0x00007EC0
	public static uint $ConstGCArrayBound$0x19084f87$110$;

	// Token: 0x04000BC5 RID: 3013 RVA: 0x00008AFC File Offset: 0x00007EFC
	public static uint $ConstGCArrayBound$0x19084f87$95$;

	// Token: 0x04000BC6 RID: 3014 RVA: 0x00008AB0 File Offset: 0x00007EB0
	public static uint $ConstGCArrayBound$0x19084f87$114$;

	// Token: 0x04000BC7 RID: 3015 RVA: 0x00008B78 File Offset: 0x00007F78
	public static uint $ConstGCArrayBound$0x19084f87$64$;

	// Token: 0x04000BC8 RID: 3016 RVA: 0x00008BC4 File Offset: 0x00007FC4
	public static uint $ConstGCArrayBound$0x19084f87$45$;

	// Token: 0x04000BC9 RID: 3017 RVA: 0x00008C68 File Offset: 0x00008068
	public static uint $ConstGCArrayBound$0x19084f87$4$;

	// Token: 0x04000BCA RID: 3018 RVA: 0x00008A7C File Offset: 0x00007E7C
	public static uint $ConstGCArrayBound$0x19084f87$127$;

	// Token: 0x04000BCB RID: 3019 RVA: 0x00008C3C File Offset: 0x0000803C
	public static uint $ConstGCArrayBound$0x19084f87$15$;

	// Token: 0x04000BCC RID: 3020 RVA: 0x00008BD8 File Offset: 0x00007FD8
	public static uint $ConstGCArrayBound$0x19084f87$40$;

	// Token: 0x04000BCD RID: 3021 RVA: 0x00008AF0 File Offset: 0x00007EF0
	public static uint $ConstGCArrayBound$0x19084f87$98$;

	// Token: 0x04000BCE RID: 3022 RVA: 0x00008C40 File Offset: 0x00008040
	public static uint $ConstGCArrayBound$0x19084f87$14$;

	// Token: 0x04000BCF RID: 3023 RVA: 0x00008C54 File Offset: 0x00008054
	public static uint $ConstGCArrayBound$0x19084f87$9$;

	// Token: 0x04000BD0 RID: 3024 RVA: 0x00009108 File Offset: 0x00008508
	public static uint $ConstGCArrayBound$0xa2eae42d$38$;

	// Token: 0x04000BD1 RID: 3025 RVA: 0x00009160 File Offset: 0x00008560
	public static uint $ConstGCArrayBound$0xa2eae42d$16$;

	// Token: 0x04000BD2 RID: 3026 RVA: 0x00009138 File Offset: 0x00008538
	public static uint $ConstGCArrayBound$0xa2eae42d$26$;

	// Token: 0x04000BD3 RID: 3027 RVA: 0x00009044 File Offset: 0x00008444
	public static uint $ConstGCArrayBound$0xa2eae42d$87$;

	// Token: 0x04000BD4 RID: 3028 RVA: 0x00008FAC File Offset: 0x000083AC
	public static uint $ConstGCArrayBound$0xa2eae42d$125$;

	// Token: 0x04000BD5 RID: 3029 RVA: 0x00009010 File Offset: 0x00008410
	public static uint $ConstGCArrayBound$0xa2eae42d$100$;

	// Token: 0x04000BD6 RID: 3030 RVA: 0x00009078 File Offset: 0x00008478
	public static uint $ConstGCArrayBound$0xa2eae42d$74$;

	// Token: 0x04000BD7 RID: 3031 RVA: 0x00009190 File Offset: 0x00008590
	public static uint $ConstGCArrayBound$0xa2eae42d$4$;

	// Token: 0x04000BD8 RID: 3032 RVA: 0x00009008 File Offset: 0x00008408
	public static uint $ConstGCArrayBound$0xa2eae42d$102$;

	// Token: 0x04000BD9 RID: 3033 RVA: 0x00009034 File Offset: 0x00008434
	public static uint $ConstGCArrayBound$0xa2eae42d$91$;

	// Token: 0x04000BDA RID: 3034 RVA: 0x00008FB0 File Offset: 0x000083B0
	public static uint $ConstGCArrayBound$0xa2eae42d$124$;

	// Token: 0x04000BDB RID: 3035 RVA: 0x00009140 File Offset: 0x00008540
	public static uint $ConstGCArrayBound$0xa2eae42d$24$;

	// Token: 0x04000BDC RID: 3036 RVA: 0x00009168 File Offset: 0x00008568
	public static uint $ConstGCArrayBound$0xa2eae42d$14$;

	// Token: 0x04000BDD RID: 3037 RVA: 0x00009048 File Offset: 0x00008448
	public static uint $ConstGCArrayBound$0xa2eae42d$86$;

	// Token: 0x04000BDE RID: 3038 RVA: 0x00009130 File Offset: 0x00008530
	public static uint $ConstGCArrayBound$0xa2eae42d$28$;

	// Token: 0x04000BDF RID: 3039 RVA: 0x00009184 File Offset: 0x00008584
	public static uint $ConstGCArrayBound$0xa2eae42d$7$;

	// Token: 0x04000BE0 RID: 3040 RVA: 0x00009178 File Offset: 0x00008578
	public static uint $ConstGCArrayBound$0xa2eae42d$10$;

	// Token: 0x04000BE1 RID: 3041 RVA: 0x00009038 File Offset: 0x00008438
	public static uint $ConstGCArrayBound$0xa2eae42d$90$;

	// Token: 0x04000BE2 RID: 3042 RVA: 0x00009134 File Offset: 0x00008534
	public static uint $ConstGCArrayBound$0xa2eae42d$27$;

	// Token: 0x04000BE3 RID: 3043 RVA: 0x000090F4 File Offset: 0x000084F4
	public static uint $ConstGCArrayBound$0xa2eae42d$43$;

	// Token: 0x04000BE4 RID: 3044 RVA: 0x0000919C File Offset: 0x0000859C
	public static uint $ConstGCArrayBound$0xa2eae42d$1$;

	// Token: 0x04000BE5 RID: 3045 RVA: 0x00009040 File Offset: 0x00008440
	public static uint $ConstGCArrayBound$0xa2eae42d$88$;

	// Token: 0x04000BE6 RID: 3046 RVA: 0x000090CC File Offset: 0x000084CC
	public static uint $ConstGCArrayBound$0xa2eae42d$53$;

	// Token: 0x04000BE7 RID: 3047 RVA: 0x000090B0 File Offset: 0x000084B0
	public static uint $ConstGCArrayBound$0xa2eae42d$60$;

	// Token: 0x04000BE8 RID: 3048 RVA: 0x000090E4 File Offset: 0x000084E4
	public static uint $ConstGCArrayBound$0xa2eae42d$47$;

	// Token: 0x04000BE9 RID: 3049 RVA: 0x00009080 File Offset: 0x00008480
	public static uint $ConstGCArrayBound$0xa2eae42d$72$;

	// Token: 0x04000BEA RID: 3050 RVA: 0x00009098 File Offset: 0x00008498
	public static uint $ConstGCArrayBound$0xa2eae42d$66$;

	// Token: 0x04000BEB RID: 3051 RVA: 0x000090B8 File Offset: 0x000084B8
	public static uint $ConstGCArrayBound$0xa2eae42d$58$;

	// Token: 0x04000BEC RID: 3052 RVA: 0x00009144 File Offset: 0x00008544
	public static uint $ConstGCArrayBound$0xa2eae42d$23$;

	// Token: 0x04000BED RID: 3053 RVA: 0x0000909C File Offset: 0x0000849C
	public static uint $ConstGCArrayBound$0xa2eae42d$65$;

	// Token: 0x04000BEE RID: 3054 RVA: 0x00009060 File Offset: 0x00008460
	public static uint $ConstGCArrayBound$0xa2eae42d$80$;

	// Token: 0x04000BEF RID: 3055 RVA: 0x00009090 File Offset: 0x00008490
	public static uint $ConstGCArrayBound$0xa2eae42d$68$;

	// Token: 0x04000BF0 RID: 3056 RVA: 0x00008FB8 File Offset: 0x000083B8
	public static uint $ConstGCArrayBound$0xa2eae42d$122$;

	// Token: 0x04000BF1 RID: 3057 RVA: 0x0000915C File Offset: 0x0000855C
	public static uint $ConstGCArrayBound$0xa2eae42d$17$;

	// Token: 0x04000BF2 RID: 3058 RVA: 0x000090F8 File Offset: 0x000084F8
	public static uint $ConstGCArrayBound$0xa2eae42d$42$;

	// Token: 0x04000BF3 RID: 3059 RVA: 0x00009088 File Offset: 0x00008488
	public static uint $ConstGCArrayBound$0xa2eae42d$70$;

	// Token: 0x04000BF4 RID: 3060 RVA: 0x00008FE8 File Offset: 0x000083E8
	public static uint $ConstGCArrayBound$0xa2eae42d$110$;

	// Token: 0x04000BF5 RID: 3061 RVA: 0x000090C8 File Offset: 0x000084C8
	public static uint $ConstGCArrayBound$0xa2eae42d$54$;

	// Token: 0x04000BF6 RID: 3062 RVA: 0x0000911C File Offset: 0x0000851C
	public static uint $ConstGCArrayBound$0xa2eae42d$33$;

	// Token: 0x04000BF7 RID: 3063 RVA: 0x00009180 File Offset: 0x00008580
	public static uint $ConstGCArrayBound$0xa2eae42d$8$;

	// Token: 0x04000BF8 RID: 3064 RVA: 0x000090D8 File Offset: 0x000084D8
	public static uint $ConstGCArrayBound$0xa2eae42d$50$;

	// Token: 0x04000BF9 RID: 3065 RVA: 0x00009030 File Offset: 0x00008430
	public static uint $ConstGCArrayBound$0xa2eae42d$92$;

	// Token: 0x04000BFA RID: 3066 RVA: 0x00009198 File Offset: 0x00008598
	public static uint $ConstGCArrayBound$0xa2eae42d$2$;

	// Token: 0x04000BFB RID: 3067 RVA: 0x000090AC File Offset: 0x000084AC
	public static uint $ConstGCArrayBound$0xa2eae42d$61$;

	// Token: 0x04000BFC RID: 3068 RVA: 0x0000905C File Offset: 0x0000845C
	public static uint $ConstGCArrayBound$0xa2eae42d$81$;

	// Token: 0x04000BFD RID: 3069 RVA: 0x00009158 File Offset: 0x00008558
	public static uint $ConstGCArrayBound$0xa2eae42d$18$;

	// Token: 0x04000BFE RID: 3070 RVA: 0x00009064 File Offset: 0x00008464
	public static uint $ConstGCArrayBound$0xa2eae42d$79$;

	// Token: 0x04000BFF RID: 3071 RVA: 0x00009174 File Offset: 0x00008574
	public static uint $ConstGCArrayBound$0xa2eae42d$11$;

	// Token: 0x04000C00 RID: 3072 RVA: 0x0000902C File Offset: 0x0000842C
	public static uint $ConstGCArrayBound$0xa2eae42d$93$;

	// Token: 0x04000C01 RID: 3073 RVA: 0x00009054 File Offset: 0x00008454
	public static uint $ConstGCArrayBound$0xa2eae42d$83$;

	// Token: 0x04000C02 RID: 3074 RVA: 0x00009028 File Offset: 0x00008428
	public static uint $ConstGCArrayBound$0xa2eae42d$94$;

	// Token: 0x04000C03 RID: 3075 RVA: 0x00009050 File Offset: 0x00008450
	public static uint $ConstGCArrayBound$0xa2eae42d$84$;

	// Token: 0x04000C04 RID: 3076 RVA: 0x00008FB4 File Offset: 0x000083B4
	public static uint $ConstGCArrayBound$0xa2eae42d$123$;

	// Token: 0x04000C05 RID: 3077 RVA: 0x00009018 File Offset: 0x00008418
	public static uint $ConstGCArrayBound$0xa2eae42d$98$;

	// Token: 0x04000C06 RID: 3078 RVA: 0x00009128 File Offset: 0x00008528
	public static uint $ConstGCArrayBound$0xa2eae42d$30$;

	// Token: 0x04000C07 RID: 3079 RVA: 0x00008FA0 File Offset: 0x000083A0
	public static uint $ConstGCArrayBound$0xa2eae42d$128$;

	// Token: 0x04000C08 RID: 3080 RVA: 0x00008FEC File Offset: 0x000083EC
	public static uint $ConstGCArrayBound$0xa2eae42d$109$;

	// Token: 0x04000C09 RID: 3081 RVA: 0x00009000 File Offset: 0x00008400
	public static uint $ConstGCArrayBound$0xa2eae42d$104$;

	// Token: 0x04000C0A RID: 3082 RVA: 0x00009118 File Offset: 0x00008518
	public static uint $ConstGCArrayBound$0xa2eae42d$34$;

	// Token: 0x04000C0B RID: 3083 RVA: 0x000090A4 File Offset: 0x000084A4
	public static uint $ConstGCArrayBound$0xa2eae42d$63$;

	// Token: 0x04000C0C RID: 3084 RVA: 0x00009024 File Offset: 0x00008424
	public static uint $ConstGCArrayBound$0xa2eae42d$95$;

	// Token: 0x04000C0D RID: 3085 RVA: 0x00008FF0 File Offset: 0x000083F0
	public static uint $ConstGCArrayBound$0xa2eae42d$108$;

	// Token: 0x04000C0E RID: 3086 RVA: 0x00008FE0 File Offset: 0x000083E0
	public static uint $ConstGCArrayBound$0xa2eae42d$112$;

	// Token: 0x04000C0F RID: 3087 RVA: 0x000090C4 File Offset: 0x000084C4
	public static uint $ConstGCArrayBound$0xa2eae42d$55$;

	// Token: 0x04000C10 RID: 3088 RVA: 0x000090C0 File Offset: 0x000084C0
	public static uint $ConstGCArrayBound$0xa2eae42d$56$;

	// Token: 0x04000C11 RID: 3089 RVA: 0x00008FF8 File Offset: 0x000083F8
	public static uint $ConstGCArrayBound$0xa2eae42d$106$;

	// Token: 0x04000C12 RID: 3090 RVA: 0x0000918C File Offset: 0x0000858C
	public static uint $ConstGCArrayBound$0xa2eae42d$5$;

	// Token: 0x04000C13 RID: 3091 RVA: 0x0000914C File Offset: 0x0000854C
	public static uint $ConstGCArrayBound$0xa2eae42d$21$;

	// Token: 0x04000C14 RID: 3092 RVA: 0x00008FE4 File Offset: 0x000083E4
	public static uint $ConstGCArrayBound$0xa2eae42d$111$;

	// Token: 0x04000C15 RID: 3093 RVA: 0x00009120 File Offset: 0x00008520
	public static uint $ConstGCArrayBound$0xa2eae42d$32$;

	// Token: 0x04000C16 RID: 3094 RVA: 0x0000904C File Offset: 0x0000844C
	public static uint $ConstGCArrayBound$0xa2eae42d$85$;

	// Token: 0x04000C17 RID: 3095 RVA: 0x000090A8 File Offset: 0x000084A8
	public static uint $ConstGCArrayBound$0xa2eae42d$62$;

	// Token: 0x04000C18 RID: 3096 RVA: 0x00009074 File Offset: 0x00008474
	public static uint $ConstGCArrayBound$0xa2eae42d$75$;

	// Token: 0x04000C19 RID: 3097 RVA: 0x00009110 File Offset: 0x00008510
	public static uint $ConstGCArrayBound$0xa2eae42d$36$;

	// Token: 0x04000C1A RID: 3098 RVA: 0x00009100 File Offset: 0x00008500
	public static uint $ConstGCArrayBound$0xa2eae42d$40$;

	// Token: 0x04000C1B RID: 3099 RVA: 0x00008FC0 File Offset: 0x000083C0
	public static uint $ConstGCArrayBound$0xa2eae42d$120$;

	// Token: 0x04000C1C RID: 3100 RVA: 0x00009170 File Offset: 0x00008570
	public static uint $ConstGCArrayBound$0xa2eae42d$12$;

	// Token: 0x04000C1D RID: 3101 RVA: 0x00008FC8 File Offset: 0x000083C8
	public static uint $ConstGCArrayBound$0xa2eae42d$118$;

	// Token: 0x04000C1E RID: 3102 RVA: 0x0000912C File Offset: 0x0000852C
	public static uint $ConstGCArrayBound$0xa2eae42d$29$;

	// Token: 0x04000C1F RID: 3103 RVA: 0x0000907C File Offset: 0x0000847C
	public static uint $ConstGCArrayBound$0xa2eae42d$73$;

	// Token: 0x04000C20 RID: 3104 RVA: 0x00009020 File Offset: 0x00008420
	public static uint $ConstGCArrayBound$0xa2eae42d$96$;

	// Token: 0x04000C21 RID: 3105 RVA: 0x00009084 File Offset: 0x00008484
	public static uint $ConstGCArrayBound$0xa2eae42d$71$;

	// Token: 0x04000C22 RID: 3106 RVA: 0x00008FA4 File Offset: 0x000083A4
	public static uint $ConstGCArrayBound$0xa2eae42d$127$;

	// Token: 0x04000C23 RID: 3107 RVA: 0x00009148 File Offset: 0x00008548
	public static uint $ConstGCArrayBound$0xa2eae42d$22$;

	// Token: 0x04000C24 RID: 3108 RVA: 0x00009004 File Offset: 0x00008404
	public static uint $ConstGCArrayBound$0xa2eae42d$103$;

	// Token: 0x04000C25 RID: 3109 RVA: 0x00009124 File Offset: 0x00008524
	public static uint $ConstGCArrayBound$0xa2eae42d$31$;

	// Token: 0x04000C26 RID: 3110 RVA: 0x0000900C File Offset: 0x0000840C
	public static uint $ConstGCArrayBound$0xa2eae42d$101$;

	// Token: 0x04000C27 RID: 3111 RVA: 0x000090DC File Offset: 0x000084DC
	public static uint $ConstGCArrayBound$0xa2eae42d$49$;

	// Token: 0x04000C28 RID: 3112 RVA: 0x0000917C File Offset: 0x0000857C
	public static uint $ConstGCArrayBound$0xa2eae42d$9$;

	// Token: 0x04000C29 RID: 3113 RVA: 0x00009150 File Offset: 0x00008550
	public static uint $ConstGCArrayBound$0xa2eae42d$20$;

	// Token: 0x04000C2A RID: 3114 RVA: 0x00009188 File Offset: 0x00008588
	public static uint $ConstGCArrayBound$0xa2eae42d$6$;

	// Token: 0x04000C2B RID: 3115 RVA: 0x00008FBC File Offset: 0x000083BC
	public static uint $ConstGCArrayBound$0xa2eae42d$121$;

	// Token: 0x04000C2C RID: 3116 RVA: 0x000090FC File Offset: 0x000084FC
	public static uint $ConstGCArrayBound$0xa2eae42d$41$;

	// Token: 0x04000C2D RID: 3117 RVA: 0x00008FD0 File Offset: 0x000083D0
	public static uint $ConstGCArrayBound$0xa2eae42d$116$;

	// Token: 0x04000C2E RID: 3118 RVA: 0x00009094 File Offset: 0x00008494
	public static uint $ConstGCArrayBound$0xa2eae42d$67$;

	// Token: 0x04000C2F RID: 3119 RVA: 0x00009114 File Offset: 0x00008514
	public static uint $ConstGCArrayBound$0xa2eae42d$35$;

	// Token: 0x04000C30 RID: 3120 RVA: 0x0000901C File Offset: 0x0000841C
	public static uint $ConstGCArrayBound$0xa2eae42d$97$;

	// Token: 0x04000C31 RID: 3121 RVA: 0x00008FD4 File Offset: 0x000083D4
	public static uint $ConstGCArrayBound$0xa2eae42d$115$;

	// Token: 0x04000C32 RID: 3122 RVA: 0x0000906C File Offset: 0x0000846C
	public static uint $ConstGCArrayBound$0xa2eae42d$77$;

	// Token: 0x04000C33 RID: 3123 RVA: 0x00008FFC File Offset: 0x000083FC
	public static uint $ConstGCArrayBound$0xa2eae42d$105$;

	// Token: 0x04000C34 RID: 3124 RVA: 0x00009164 File Offset: 0x00008564
	public static uint $ConstGCArrayBound$0xa2eae42d$15$;

	// Token: 0x04000C35 RID: 3125 RVA: 0x0000910C File Offset: 0x0000850C
	public static uint $ConstGCArrayBound$0xa2eae42d$37$;

	// Token: 0x04000C36 RID: 3126 RVA: 0x00008FA8 File Offset: 0x000083A8
	public static uint $ConstGCArrayBound$0xa2eae42d$126$;

	// Token: 0x04000C37 RID: 3127 RVA: 0x000090F0 File Offset: 0x000084F0
	public static uint $ConstGCArrayBound$0xa2eae42d$44$;

	// Token: 0x04000C38 RID: 3128 RVA: 0x000090BC File Offset: 0x000084BC
	public static uint $ConstGCArrayBound$0xa2eae42d$57$;

	// Token: 0x04000C39 RID: 3129 RVA: 0x00009104 File Offset: 0x00008504
	public static uint $ConstGCArrayBound$0xa2eae42d$39$;

	// Token: 0x04000C3A RID: 3130 RVA: 0x000090E0 File Offset: 0x000084E0
	public static uint $ConstGCArrayBound$0xa2eae42d$48$;

	// Token: 0x04000C3B RID: 3131 RVA: 0x000090B4 File Offset: 0x000084B4
	public static uint $ConstGCArrayBound$0xa2eae42d$59$;

	// Token: 0x04000C3C RID: 3132 RVA: 0x0000913C File Offset: 0x0000853C
	public static uint $ConstGCArrayBound$0xa2eae42d$25$;

	// Token: 0x04000C3D RID: 3133 RVA: 0x00009154 File Offset: 0x00008554
	public static uint $ConstGCArrayBound$0xa2eae42d$19$;

	// Token: 0x04000C3E RID: 3134 RVA: 0x00008FD8 File Offset: 0x000083D8
	public static uint $ConstGCArrayBound$0xa2eae42d$114$;

	// Token: 0x04000C3F RID: 3135 RVA: 0x00009014 File Offset: 0x00008414
	public static uint $ConstGCArrayBound$0xa2eae42d$99$;

	// Token: 0x04000C40 RID: 3136 RVA: 0x0000908C File Offset: 0x0000848C
	public static uint $ConstGCArrayBound$0xa2eae42d$69$;

	// Token: 0x04000C41 RID: 3137 RVA: 0x00008FF4 File Offset: 0x000083F4
	public static uint $ConstGCArrayBound$0xa2eae42d$107$;

	// Token: 0x04000C42 RID: 3138 RVA: 0x00009194 File Offset: 0x00008594
	public static uint $ConstGCArrayBound$0xa2eae42d$3$;

	// Token: 0x04000C43 RID: 3139 RVA: 0x000090E8 File Offset: 0x000084E8
	public static uint $ConstGCArrayBound$0xa2eae42d$46$;

	// Token: 0x04000C44 RID: 3140 RVA: 0x000090A0 File Offset: 0x000084A0
	public static uint $ConstGCArrayBound$0xa2eae42d$64$;

	// Token: 0x04000C45 RID: 3141 RVA: 0x0000903C File Offset: 0x0000843C
	public static uint $ConstGCArrayBound$0xa2eae42d$89$;

	// Token: 0x04000C46 RID: 3142 RVA: 0x00008FC4 File Offset: 0x000083C4
	public static uint $ConstGCArrayBound$0xa2eae42d$119$;

	// Token: 0x04000C47 RID: 3143 RVA: 0x00008FCC File Offset: 0x000083CC
	public static uint $ConstGCArrayBound$0xa2eae42d$117$;

	// Token: 0x04000C48 RID: 3144 RVA: 0x00009058 File Offset: 0x00008458
	public static uint $ConstGCArrayBound$0xa2eae42d$82$;

	// Token: 0x04000C49 RID: 3145 RVA: 0x000090D4 File Offset: 0x000084D4
	public static uint $ConstGCArrayBound$0xa2eae42d$51$;

	// Token: 0x04000C4A RID: 3146 RVA: 0x0000916C File Offset: 0x0000856C
	public static uint $ConstGCArrayBound$0xa2eae42d$13$;

	// Token: 0x04000C4B RID: 3147 RVA: 0x00008FDC File Offset: 0x000083DC
	public static uint $ConstGCArrayBound$0xa2eae42d$113$;

	// Token: 0x04000C4C RID: 3148 RVA: 0x000090D0 File Offset: 0x000084D0
	public static uint $ConstGCArrayBound$0xa2eae42d$52$;

	// Token: 0x04000C4D RID: 3149 RVA: 0x00009068 File Offset: 0x00008468
	public static uint $ConstGCArrayBound$0xa2eae42d$78$;

	// Token: 0x04000C4E RID: 3150 RVA: 0x00009070 File Offset: 0x00008470
	public static uint $ConstGCArrayBound$0xa2eae42d$76$;

	// Token: 0x04000C4F RID: 3151 RVA: 0x000090EC File Offset: 0x000084EC
	public static uint $ConstGCArrayBound$0xa2eae42d$45$;
}

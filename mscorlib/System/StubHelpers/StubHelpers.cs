using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace System.StubHelpers
{
	// Token: 0x02000129 RID: 297
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	[SuppressUnmanagedCodeSecurity]
	internal static class StubHelpers
	{
		// Token: 0x06001059 RID: 4185
		[DllImport("oleaut32.dll")]
		internal static extern int VarCyFromDec(ref DECIMAL pdecIn, ref CY cyOut);

		// Token: 0x0600105A RID: 4186
		[DllImport("oleaut32.dll")]
		internal static extern int VarDecFromCy(CY cyIn, ref DECIMAL decOut);

		// Token: 0x0600105B RID: 4187
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DoNDirectCall();

		// Token: 0x0600105C RID: 4188
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DoCLRToCOMCall(object thisPtr);

		// Token: 0x0600105D RID: 4189
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetLastError();

		// Token: 0x0600105E RID: 4190
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr BeginStandalone(IntPtr pFrame, IntPtr pNMD, int dwStubFlags);

		// Token: 0x0600105F RID: 4191
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr BeginStandaloneCleanup(IntPtr pFrame, IntPtr pNMD, int dwStubFlags);

		// Token: 0x06001060 RID: 4192
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr BeginCLRToCOMStandalone(IntPtr pFrame, IntPtr pCPCMD, int dwStubFlags, object pThis);

		// Token: 0x06001061 RID: 4193
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr BeginCLRToCOMStandaloneCleanup(IntPtr pFrame, IntPtr pCPCMD, int dwStubFlags, object pThis);

		// Token: 0x06001062 RID: 4194
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ThrowDeferredException();

		// Token: 0x06001063 RID: 4195
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ThrowInteropParamException(int resID, int paramIdx);

		// Token: 0x06001064 RID: 4196
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void CreateCleanupList(IntPtr pCleanupWorkList);

		// Token: 0x06001065 RID: 4197
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DestroyCleanupList(IntPtr pFrameMem, bool fExceptionThrown);

		// Token: 0x06001066 RID: 4198
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern object GetHRExceptionObject(int hr);

		// Token: 0x06001067 RID: 4199
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern object GetCOMHRExceptionObject(int hr, IntPtr pFrame, object pThis);

		// Token: 0x06001068 RID: 4200
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr CreateCustomMarshalerHelper(IntPtr pMD, int paramToken, IntPtr hndManagedType);

		// Token: 0x06001069 RID: 4201
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr MarshalAsAny(ref object pManagedHome, int dwFlags, IntPtr pvMarshaler, IntPtr pCleanupWorkList);

		// Token: 0x0600106A RID: 4202
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void UnmarshalAsAny(ref object pManagedHome, IntPtr pvMarshaler);

		// Token: 0x0600106B RID: 4203
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr SafeHandleC2NHelper(object pThis, IntPtr pCleanupWorkList);

		// Token: 0x0600106C RID: 4204
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void StubRegisterRCW(object pThis, IntPtr pThread, IntPtr pCleanupWorkList);

		// Token: 0x0600106D RID: 4205
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr ProfilerBeginTransitionCallback(IntPtr pSecretParam, IntPtr pThread, object pThis);

		// Token: 0x0600106E RID: 4206
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ProfilerEndTransitionCallback(IntPtr pMD, IntPtr pThread);

		// Token: 0x0600106F RID: 4207
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DebuggerTraceCall(IntPtr pSecretParam);

		// Token: 0x06001070 RID: 4208 RVA: 0x0002E2A6 File Offset: 0x0002D2A6
		internal static void CheckStringLength(int length)
		{
			if (length > 2147483632)
			{
				throw new MarshalDirectiveException(Environment.GetResourceString("Marshaler_StringTooLong"));
			}
		}

		// Token: 0x06001071 RID: 4209
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern int strlen(sbyte* ptr);

		// Token: 0x06001072 RID: 4210
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern decimal DecimalCanonicalizeInternal(decimal dec);

		// Token: 0x06001073 RID: 4211
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void FmtClassUpdateNativeInternal(object obj, byte* pNative, IntPtr pOptionalCleanupList);

		// Token: 0x06001074 RID: 4212
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void FmtClassUpdateCLRInternal(object obj, byte* pNative);

		// Token: 0x06001075 RID: 4213
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void LayoutDestroyNativeInternal(byte* pNative, IntPtr pMT);

		// Token: 0x06001076 RID: 4214
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern object AllocateInternal(IntPtr typeHandle);

		// Token: 0x06001077 RID: 4215
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr MarshalToUnmanagedVaListInternal(IntPtr pArgIterator);

		// Token: 0x06001078 RID: 4216
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void MarshalToManagedVaListInternal(IntPtr va_list, IntPtr pArgIterator);

		// Token: 0x04000583 RID: 1411
		internal const string OLEAUT32 = "oleaut32.dll";
	}
}

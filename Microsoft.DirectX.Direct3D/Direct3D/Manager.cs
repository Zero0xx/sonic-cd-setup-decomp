using System;
using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;
using Microsoft.DirectX.Security;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000101 RID: 257
	public sealed class Manager : MarshalByRefObject
	{
		// Token: 0x06000597 RID: 1431 RVA: 0x000649A0 File Offset: 0x00063DA0
		static Manager()
		{
			new GraphicsPermission(true).Demand();
			Manager.pHolder = new Manager();
			Manager.m_lpUM = <Module>.Direct3DCreate9(32U);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0006539C File Offset: 0x0006479C
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(Manager);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x000653CC File Offset: 0x000647CC
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator ==(Manager left, Manager right)
		{
			if (left == null)
			{
				if (right == null)
				{
					return true;
				}
			}
			else if (right != null)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00065BD8 File Offset: 0x00064FD8
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator !=(Manager left, Manager right)
		{
			return !(left == right);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x000653EC File Offset: 0x000647EC
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00062698 File Offset: 0x00061A98
		private Manager()
		{
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0006540C File Offset: 0x0006480C
		protected unsafe override void Finalize()
		{
			if (Manager.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), Manager.m_lpUM, *(*(int*)Manager.m_lpUM + 8)))
			{
				Manager.m_lpUM = null;
			}
			base.Finalize();
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x00065530 File Offset: 0x00064930
		internal unsafe static int AdapterCount
		{
			get
			{
				return calli(System.UInt32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), Manager.m_lpUM, *(*(int*)Manager.m_lpUM + 16));
			}
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x000626B0 File Offset: 0x00061AB0
		internal static AdapterDetails GetAdapterInformation(int adapter)
		{
			return Manager.GetAdapterInformation(adapter, 0);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00065558 File Offset: 0x00064958
		internal unsafe static AdapterDetails GetAdapterInformation(int adapter, int flags)
		{
			_D3DADAPTER_IDENTIFIER9 val;
			initblk(ref val, 0, 1100);
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),Microsoft.DirectX.PrivateImplementationDetails._D3DADAPTER_IDENTIFIER9*), Manager.m_lpUM, adapter, flags, ref val, *(*(int*)Manager.m_lpUM + 20));
			if (num < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num);
				}
			}
			return AdapterDetails.op_explicit(val);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x000655C0 File Offset: 0x000649C0
		internal unsafe static int GetAdapterModeCount(int adapter, Format format)
		{
			return calli(System.UInt32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32), Manager.m_lpUM, adapter, format, *(*(int*)Manager.m_lpUM + 24));
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x000655E8 File Offset: 0x000649E8
		internal unsafe static DisplayMode EnumAdapterModes(int adapter, Format format, int mode)
		{
			DisplayMode result = default(DisplayMode);
			result = new DisplayMode();
			initblk(ref result, 0, sizeof(DisplayMode));
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails._D3DDISPLAYMODE*), Manager.m_lpUM, adapter, format, mode, ref result, *(*(int*)Manager.m_lpUM + 28));
			if (num < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num);
				}
			}
			return result;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0006565C File Offset: 0x00064A5C
		internal unsafe static DisplayMode GetAdapterDisplayMode(int adapter)
		{
			DisplayMode result = default(DisplayMode);
			result = new DisplayMode();
			initblk(ref result, 0, sizeof(DisplayMode));
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails._D3DDISPLAYMODE*), Manager.m_lpUM, adapter, ref result, *(*(int*)Manager.m_lpUM + 32));
			if (num < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num);
				}
			}
			return result;
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x000626CC File Offset: 0x00061ACC
		public static AdapterListCollection Adapters
		{
			get
			{
				return new AdapterListCollection();
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00065BF4 File Offset: 0x00064FF4
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool CheckDeviceType(int adapter, DeviceType checkType, Format displayFormat, Format backBufferFormat, [MarshalAs(UnmanagedType.U1)] bool windowed)
		{
			return Manager.CheckDeviceType(adapter, checkType, displayFormat, backBufferFormat, windowed, 0);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x000656D0 File Offset: 0x00064AD0
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool CheckDeviceType(int adapter, DeviceType checkType, Format displayFormat, Format backBufferFormat, [MarshalAs(UnmanagedType.U1)] bool windowed, out int result)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32,System.Int32,System.Int32,System.Int32), Manager.m_lpUM, adapter, checkType, displayFormat, backBufferFormat, windowed, *(*(int*)Manager.m_lpUM + 36));
			if (ref result != null)
			{
				result = num;
			}
			return num >= 0;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00062708 File Offset: 0x00061B08
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool CheckDeviceFormat(int adapter, DeviceType deviceType, Format adapterFormat, Usage usage, ResourceType resourceType, DepthFormat checkFormat)
		{
			return Manager.CheckDeviceFormat(adapter, deviceType, adapterFormat, usage, resourceType, (Format)checkFormat);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x000626E4 File Offset: 0x00061AE4
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool CheckDeviceFormat(int adapter, DeviceType deviceType, Format adapterFormat, Usage usage, ResourceType resourceType, DepthFormat checkFormat, out int result)
		{
			return Manager.CheckDeviceFormat(adapter, deviceType, adapterFormat, usage, resourceType, (Format)checkFormat, out result);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00065C14 File Offset: 0x00065014
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool CheckDeviceFormat(int adapter, DeviceType deviceType, Format adapterFormat, Usage usage, ResourceType resourceType, Format checkFormat)
		{
			return Manager.CheckDeviceFormat(adapter, deviceType, adapterFormat, usage, resourceType, checkFormat, 0);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00065710 File Offset: 0x00064B10
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool CheckDeviceFormat(int adapter, DeviceType deviceType, Format adapterFormat, Usage usage, ResourceType resourceType, Format checkFormat, out int result)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32,System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,System.Int32), Manager.m_lpUM, adapter, deviceType, adapterFormat, usage, resourceType, checkFormat, *(*(int*)Manager.m_lpUM + 40));
			if (ref result != null)
			{
				result = num;
			}
			return num >= 0;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00065750 File Offset: 0x00064B50
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool CheckDeviceMultiSampleType(int adapter, DeviceType deviceType, Format surfaceFormat, [MarshalAs(UnmanagedType.U1)] bool windowed, MultiSampleType multiSampleType, out int result, out int qualityLevels)
		{
			uint num = 0U;
			int num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32,System.Int32,System.Int32,System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)*), Manager.m_lpUM, adapter, deviceType, surfaceFormat, windowed, multiSampleType, ref num, *(*(int*)Manager.m_lpUM + 44));
			if (ref result != null)
			{
				result = num2;
			}
			if (ref qualityLevels != null)
			{
				qualityLevels = (int)num;
			}
			return num2 >= 0;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00065C34 File Offset: 0x00065034
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool CheckDeviceMultiSampleType(int adapter, DeviceType deviceType, Format surfaceFormat, [MarshalAs(UnmanagedType.U1)] bool windowed, MultiSampleType multiSampleType)
		{
			return Manager.CheckDeviceMultiSampleType(adapter, deviceType, surfaceFormat, windowed, multiSampleType, 0, 0);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0006579C File Offset: 0x00064B9C
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool CheckDepthStencilMatch(int adapter, DeviceType deviceType, Format adapterFormat, Format renderTargetFormat, DepthFormat depthStencilFormat, out int result)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32,System.Int32,System.Int32,System.Int32), Manager.m_lpUM, adapter, deviceType, adapterFormat, renderTargetFormat, depthStencilFormat, *(*(int*)Manager.m_lpUM + 48));
			if (ref result != null)
			{
				result = num;
			}
			return num >= 0;
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00065C54 File Offset: 0x00065054
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool CheckDepthStencilMatch(int adapter, DeviceType deviceType, Format adapterFormat, Format renderTargetFormat, DepthFormat depthStencilFormat)
		{
			return Manager.CheckDepthStencilMatch(adapter, deviceType, adapterFormat, renderTargetFormat, depthStencilFormat, 0);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x000657DC File Offset: 0x00064BDC
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool CheckDeviceFormatConversion(int adapter, DeviceType deviceType, Format sourceFormat, Format targetFormat, out int result)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32,System.Int32,System.Int32), Manager.m_lpUM, adapter, deviceType, sourceFormat, targetFormat, *(*(int*)Manager.m_lpUM + 52));
			if (ref result != null)
			{
				result = num;
			}
			return num >= 0;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00065C74 File Offset: 0x00065074
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool CheckDeviceFormatConversion(int adapter, DeviceType deviceType, Format sourceFormat, Format targetFormat)
		{
			return Manager.CheckDeviceFormatConversion(adapter, deviceType, sourceFormat, targetFormat, 0);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00065818 File Offset: 0x00064C18
		public unsafe static Caps GetDeviceCaps(int adapter, DeviceType deviceType)
		{
			Caps result = default(Caps);
			result = new Caps();
			initblk(ref result.m_Internal, 0, 304);
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DCAPS9*), Manager.m_lpUM, adapter, deviceType, ref result.m_Internal, *(*(int*)Manager.m_lpUM + 56));
			if (num < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num);
				}
			}
			return result;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00065894 File Offset: 0x00064C94
		public unsafe static IntPtr GetAdapterMonitor(int adapter)
		{
			IntPtr result = 0;
			result = new IntPtr(calli(Microsoft.DirectX.PrivateImplementationDetails.HMONITOR__* modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32), Manager.m_lpUM, adapter, *(*(int*)Manager.m_lpUM + 60)));
			return result;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00065450 File Offset: 0x00064850
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool DisableD3DSpy()
		{
			bool result = false;
			HINSTANCE__* ptr = <Module>.LoadLibraryA((sbyte*)(&<Module>.??_C@_08NIHLGCK@d3d9?4dll?$AA@));
			if (ptr != null)
			{
				method procAddress = <Module>.GetProcAddress(ptr, (sbyte*)(&<Module>.??_C@_0O@CGJKOMPJ@DisableD3DSpy?$AA@));
				if (procAddress != null)
				{
					calli(System.Void modopt(System.Runtime.CompilerServices.CallConvStdcall)(), procAddress);
					result = true;
				}
				<Module>.FreeLibrary(ptr);
			}
			return result;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00065494 File Offset: 0x00064894
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe static bool GenerateD3DSpyBreak()
		{
			bool result = false;
			HINSTANCE__* ptr = <Module>.LoadLibraryA((sbyte*)(&<Module>.??_C@_08NIHLGCK@d3d9?4dll?$AA@));
			if (ptr != null)
			{
				method procAddress = <Module>.GetProcAddress(ptr, (sbyte*)(&<Module>.??_C@_0M@NIPEKIB@D3DSpyBreak?$AA@));
				if (procAddress != null)
				{
					calli(System.Void modopt(System.Runtime.CompilerServices.CallConvStdcall)(), procAddress);
					result = true;
				}
				<Module>.FreeLibrary(ptr);
			}
			return result;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x000654D8 File Offset: 0x000648D8
		private unsafe static void RegisterSoftwareDevice(Delegate Init)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Void*), Manager.m_lpUM, Init, *(*(int*)Manager.m_lpUM + 12));
			if (num < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num);
				}
			}
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00062728 File Offset: 0x00061B28
		private void __dtor()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x04001040 RID: 4160
		internal static Manager pHolder;

		// Token: 0x04001041 RID: 4161
		internal unsafe static IDirect3D9* m_lpUM;
	}
}

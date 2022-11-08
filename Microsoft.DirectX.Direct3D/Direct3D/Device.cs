using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.DirectX.PrivateImplementationDetails;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000103 RID: 259
	public sealed class Device : MarshalByRefObject, IDisposable
	{
		// Token: 0x060005B7 RID: 1463 RVA: 0x00065C90 File Offset: 0x00065090
		[return: MarshalAs(UnmanagedType.U1)]
		public override bool Equals(object compare)
		{
			return compare != null && compare.GetType() == typeof(Device) && this.m_lpUM == compare.m_lpUM;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00065CCC File Offset: 0x000650CC
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator ==(Device left, Device right)
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
				return left.m_lpUM == right.m_lpUM;
			}
			return false;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x000693C4 File Offset: 0x000687C4
		[return: MarshalAs(UnmanagedType.U1)]
		public static bool operator !=(Device left, Device right)
		{
			return !(left == right);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00065CFC File Offset: 0x000650FC
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x000693E0 File Offset: 0x000687E0
		public unsafe void Dispose()
		{
			this.raise_DeviceLost(this, EventArgs.Empty);
			if (this != null && !this.m_bDisposed)
			{
				this.raise_Disposing(this, EventArgs.Empty);
				this.m_bDisposed = true;
				if (this.parent != null)
				{
					this.parent.Resize -= this.OnParentResized;
					this.parent.SizeChanged -= this.OnParentResized;
				}
				if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
				{
					this.m_lpUM = null;
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x00062760 File Offset: 0x00061B60
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x00062778 File Offset: 0x00061B78
		public static bool IsUsingEventHandlers
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return Device.m_useEventHandlers;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				Device.m_useEventHandlers = value;
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00065E4C File Offset: 0x0006524C
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool CheckCooperativeLevel()
		{
			return calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 12)) >= 0;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00065E7C File Offset: 0x0006527C
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool CheckCooperativeLevel(out int result)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 12));
			result = num;
			return num >= 0;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00065EB0 File Offset: 0x000652B0
		public unsafe void TestCooperativeLevel()
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 12));
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

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x00065F08 File Offset: 0x00065308
		public unsafe int AvailableTextureMemory
		{
			get
			{
				return calli(System.UInt32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 16));
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x00065F30 File Offset: 0x00065330
		public unsafe Caps DeviceCaps
		{
			get
			{
				Caps result = default(Caps);
				result = new Caps();
				initblk(result.Handle, 0, sizeof(Caps));
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DCAPS9*), this.m_lpUM, result.Handle, *(*(int*)this.m_lpUM + 28));
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
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x00069484 File Offset: 0x00068884
		public DisplayMode DisplayMode
		{
			get
			{
				return this.GetDisplayMode(0);
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00065FB0 File Offset: 0x000653B0
		private unsafe DisplayMode GetDisplayMode(int swapChain)
		{
			DisplayMode result = default(DisplayMode);
			result = new DisplayMode();
			initblk(ref result, 0, sizeof(DisplayMode));
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails._D3DDISPLAYMODE*), this.m_lpUM, swapChain, ref result, *(*(int*)this.m_lpUM + 32));
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

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x00066024 File Offset: 0x00065424
		public unsafe DeviceCreationParameters CreationParameters
		{
			get
			{
				DeviceCreationParameters result = default(DeviceCreationParameters);
				result = new DeviceCreationParameters();
				initblk(ref result, 0, sizeof(DeviceCreationParameters));
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DDEVICE_CREATION_PARAMETERS*), this.m_lpUM, ref result, *(*(int*)this.m_lpUM + 36));
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
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00066098 File Offset: 0x00065498
		public unsafe void SetCursorPosition(int positionX, int positionY, [MarshalAs(UnmanagedType.U1)] bool updateImmediate)
		{
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, positionX, positionY, updateImmediate ? 1 : 0, *(*(int*)this.m_lpUM + 44));
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000660CC File Offset: 0x000654CC
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool ShowCursor([MarshalAs(UnmanagedType.U1)] bool canShow)
		{
			return calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32), this.m_lpUM, canShow, *(*(int*)this.m_lpUM + 48)) != 0;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000660FC File Offset: 0x000654FC
		public unsafe void Reset(params PresentParameters[] presentationParameters)
		{
			if (presentationParameters != null && presentationParameters.Length != 0)
			{
				this.canDeviceBeReset = false;
				this.localPresent = presentationParameters[0];
				this.raise_DeviceLost(this, EventArgs.Empty);
				bool flag = false;
				_D3DPRESENT_PARAMETERS_* ptr;
				if (presentationParameters.Length == 1)
				{
					ptr = presentationParameters[0].RealStruct;
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
							num++;
							ptr2 += 56 / sizeof(_D3DPRESENT_PARAMETERS_);
						}
						while (num < presentationParameters.Length);
					}
				}
				int num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DPRESENT_PARAMETERS_*), this.m_lpUM, ptr, *(*(int*)this.m_lpUM + 64));
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
					this.raise_DeviceReset(this, EventArgs.Empty);
					this.canDeviceBeReset = true;
				}
				else if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num2);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num2;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num2);
				}
				return;
			}
			throw new ArgumentNullException("presentationParameters");
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00066228 File Offset: 0x00065628
		[CLSCompliant(false)]
		internal unsafe void PresentInternal(tagRECT* sourceRectangle, tagRECT* destRectangle, IntPtr overrideWindow)
		{
			if (sourceRectangle != null)
			{
				*(int*)(sourceRectangle + 8 / sizeof(tagRECT)) = *(int*)(sourceRectangle + 8 / sizeof(tagRECT)) + *(int*)sourceRectangle;
				*(int*)(sourceRectangle + 12 / sizeof(tagRECT)) = *(int*)(sourceRectangle + 12 / sizeof(tagRECT)) + *(int*)(sourceRectangle + 4 / sizeof(tagRECT));
			}
			if (destRectangle != null)
			{
				*(int*)(destRectangle + 8 / sizeof(tagRECT)) = *(int*)(destRectangle + 8 / sizeof(tagRECT)) + *(int*)destRectangle;
				*(int*)(destRectangle + 12 / sizeof(tagRECT)) = *(int*)(destRectangle + 12 / sizeof(tagRECT)) + *(int*)(destRectangle + 4 / sizeof(tagRECT));
			}
			IntPtr value = 0;
			value = new IntPtr(0);
			int num;
			if (overrideWindow != value)
			{
				num = overrideWindow.ToPointer();
			}
			else
			{
				num = 0;
			}
			int num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails.HWND__*,Microsoft.DirectX.PrivateImplementationDetails._RGNDATA modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, sourceRectangle, destRectangle, num, 0, *(*(int*)this.m_lpUM + 68));
			if (num2 < 0)
			{
				if (num2 == -2005530520)
				{
					this.raise_DeviceLost(this, EventArgs.Empty);
				}
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num2);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num2;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num2);
				}
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00062790 File Offset: 0x00061B90
		[CLSCompliant(false)]
		internal unsafe void PresentInternal(tagRECT* sourceRectangle, tagRECT* destRectangle, Control overrideWindow)
		{
			IntPtr* ptr;
			if (overrideWindow != null)
			{
				IntPtr handle = overrideWindow.Handle;
				ptr = &handle;
			}
			else
			{
				IntPtr intPtr = 0;
				intPtr = new IntPtr(0);
				ptr = &intPtr;
			}
			this.PresentInternal(sourceRectangle, destRectangle, *ptr);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000628C0 File Offset: 0x00061CC0
		public unsafe void Present(Rectangle rectPresent, IntPtr overrideWindowHandle, [MarshalAs(UnmanagedType.U1)] bool sourceRectangle)
		{
			if (sourceRectangle)
			{
				this.PresentInternal((tagRECT*)(&rectPresent), null, overrideWindowHandle);
			}
			else
			{
				this.PresentInternal(null, (tagRECT*)(&rectPresent), overrideWindowHandle);
			}
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000628A4 File Offset: 0x00061CA4
		public void Present(IntPtr overrideWindowHandle)
		{
			this.PresentInternal(null, null, overrideWindowHandle);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00062884 File Offset: 0x00061C84
		public unsafe void Present(Rectangle sourceRectangle, Rectangle destRectangle, IntPtr overrideWindowHandle)
		{
			this.PresentInternal((tagRECT*)(&sourceRectangle), (tagRECT*)(&destRectangle), overrideWindowHandle);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00062858 File Offset: 0x00061C58
		public unsafe void Present(Rectangle rectPresent, [MarshalAs(UnmanagedType.U1)] bool sourceRectangle)
		{
			if (sourceRectangle)
			{
				this.PresentInternal((tagRECT*)(&rectPresent), null, null);
			}
			else
			{
				this.PresentInternal(null, (tagRECT*)(&rectPresent), null);
			}
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0006282C File Offset: 0x00061C2C
		public unsafe void Present(Rectangle rectPresent, Control overrideWindow, [MarshalAs(UnmanagedType.U1)] bool sourceRectangle)
		{
			if (sourceRectangle)
			{
				this.PresentInternal((tagRECT*)(&rectPresent), null, overrideWindow);
			}
			else
			{
				this.PresentInternal(null, (tagRECT*)(&rectPresent), overrideWindow);
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00062810 File Offset: 0x00061C10
		public void Present(Control overrideWindow)
		{
			this.PresentInternal(null, null, overrideWindow);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x000627F4 File Offset: 0x00061BF4
		public void Present()
		{
			this.PresentInternal(null, null, null);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x000627D4 File Offset: 0x00061BD4
		public unsafe void Present(Rectangle sourceRectangle, Rectangle destRectangle, Control overrideWindow)
		{
			this.PresentInternal((tagRECT*)(&sourceRectangle), (tagRECT*)(&destRectangle), overrideWindow);
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x000694A0 File Offset: 0x000688A0
		public RasterStatus RasterStatus
		{
			get
			{
				return this.GetRasterStatus(0);
			}
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x000662F8 File Offset: 0x000656F8
		public unsafe RasterStatus GetRasterStatus(int swapChain)
		{
			RasterStatus result = default(RasterStatus);
			result = new RasterStatus();
			initblk(ref result, 0, sizeof(RasterStatus));
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails._D3DRASTER_STATUS*), this.m_lpUM, swapChain, ref result, *(*(int*)this.m_lpUM + 76));
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

		// Token: 0x060005D6 RID: 1494 RVA: 0x0006636C File Offset: 0x0006576C
		public unsafe GammaRamp GetGammaRamp(int swapChain)
		{
			GammaRamp result = default(GammaRamp);
			result = new GammaRamp();
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails._D3DGAMMARAMP*), this.m_lpUM, swapChain, result.Handle, *(*(int*)this.m_lpUM + 88));
			return result;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x000663AC File Offset: 0x000657AC
		public unsafe void SetGammaRamp(int swapChain, [MarshalAs(UnmanagedType.U1)] bool calibrate, GammaRamp ramp)
		{
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),Microsoft.DirectX.PrivateImplementationDetails._D3DGAMMARAMP modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, swapChain, calibrate ? 1 : 0, ramp.Handle, *(*(int*)this.m_lpUM + 84));
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x000663E4 File Offset: 0x000657E4
		public unsafe void BeginScene()
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 164));
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

		// Token: 0x060005D9 RID: 1497 RVA: 0x00066440 File Offset: 0x00065840
		public unsafe void EndScene()
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 168));
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

		// Token: 0x060005DA RID: 1498 RVA: 0x00062934 File Offset: 0x00061D34
		public void Clear(ClearFlags flags, Color color, float zdepth, int stencil)
		{
			this.Clear(flags, color.ToArgb(), zdepth, stencil, null);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0006290C File Offset: 0x00061D0C
		public void Clear(ClearFlags flags, Color color, float zdepth, int stencil, Rectangle[] rect)
		{
			this.Clear(flags, color.ToArgb(), zdepth, stencil, rect);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x000628EC File Offset: 0x00061CEC
		public void Clear(ClearFlags flags, int color, float zdepth, int stencil)
		{
			this.Clear(flags, color, zdepth, stencil, null);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0006649C File Offset: 0x0006589C
		public unsafe void Clear(ClearFlags flags, int color, float zdepth, int stencil, Rectangle[] regions)
		{
			int num2;
			if (regions != null && regions.Length != 0)
			{
				int num = 0;
				if (0 < regions.Length)
				{
					do
					{
						regions[num].Width = regions[num].Width + regions[num].X;
						regions[num].Height = regions[num].Height + regions[num].Y;
						num++;
					}
					while (num < regions.Length);
				}
				ref Rectangle rectangle& = ref regions[0];
				num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),Microsoft.DirectX.PrivateImplementationDetails._D3DRECT modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Single,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, regions.Length, ref rectangle&, flags, color, zdepth, stencil, *(*(int*)this.m_lpUM + 172));
				int num3 = 0;
				if (0 < regions.Length)
				{
					do
					{
						regions[num3].Width = regions[num3].Width - regions[num3].X;
						regions[num3].Height = regions[num3].Height - regions[num3].Y;
						num3++;
					}
					while (num3 < regions.Length);
				}
			}
			else
			{
				num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),Microsoft.DirectX.PrivateImplementationDetails._D3DRECT modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Single,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, 0, 0, flags, color, zdepth, stencil, *(*(int*)this.m_lpUM + 172));
			}
			if (num2 < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num2);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num2;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num2);
				}
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00066610 File Offset: 0x00065A10
		public unsafe void SetTransform(TransformType state, Matrix matrix)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DMATRIX modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, state, ref matrix, *(*(int*)this.m_lpUM + 176));
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

		// Token: 0x060005DF RID: 1503 RVA: 0x0006666C File Offset: 0x00065A6C
		[CLSCompliant(false)]
		internal unsafe Matrix GetTransform(TransformType state, int* result)
		{
			Matrix result2 = default(Matrix);
			result2 = new Matrix();
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DMATRIX*), this.m_lpUM, state, ref result2, *(*(int*)this.m_lpUM + 180));
			if (result != null)
			{
				*(int*)result = num;
			}
			else if (num < 0)
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
			return result2;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00062958 File Offset: 0x00061D58
		public Matrix GetTransform(TransformType state)
		{
			return this.GetTransform(state, null);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000666E0 File Offset: 0x00065AE0
		public unsafe void MultiplyTransform(TransformType state, Matrix matrix)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DMATRIX modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, state, ref matrix, *(*(int*)this.m_lpUM + 184));
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

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0006673C File Offset: 0x00065B3C
		// (set) Token: 0x060005E3 RID: 1507 RVA: 0x000667B4 File Offset: 0x00065BB4
		public unsafe Viewport Viewport
		{
			get
			{
				Viewport result = default(Viewport);
				result = new Viewport();
				initblk(ref result, 0, sizeof(Viewport));
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DVIEWPORT9*), this.m_lpUM, ref result, *(*(int*)this.m_lpUM + 192));
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
			set
			{
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DVIEWPORT9 modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, ref value, *(*(int*)this.m_lpUM + 188));
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
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x00066810 File Offset: 0x00065C10
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x00066888 File Offset: 0x00065C88
		public unsafe Material Material
		{
			get
			{
				Material result = default(Material);
				result = new Material();
				initblk(ref result, 0, sizeof(Material));
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DMATERIAL9*), this.m_lpUM, ref result, *(*(int*)this.m_lpUM + 200));
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
			set
			{
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DMATERIAL9 modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, ref value, *(*(int*)this.m_lpUM + 196));
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
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00066940 File Offset: 0x00065D40
		[CLSCompliant(false)]
		internal unsafe int GetRenderState(int state, int* pReturnValue)
		{
			uint result = 0U;
			*(int*)pReturnValue = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)*), this.m_lpUM, state, ref result, *(*(int*)this.m_lpUM + 232));
			return (int)result;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00062998 File Offset: 0x00061D98
		public void SetRenderState(RenderStates state, float value)
		{
			this.SetRenderState(state, (int)value);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00062974 File Offset: 0x00061D74
		public void SetRenderState(RenderStates state, [MarshalAs(UnmanagedType.U1)] bool value)
		{
			this.SetRenderState(state, value ? 1 : 0);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x000668E4 File Offset: 0x00065CE4
		public unsafe void SetRenderState(RenderStates state, int value)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, state, value, *(*(int*)this.m_lpUM + 228));
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

		// Token: 0x060005EA RID: 1514 RVA: 0x00066974 File Offset: 0x00065D74
		public unsafe int GetRenderStateInt32(RenderStates state)
		{
			int num = 0;
			int renderState = this.GetRenderState((int)state, (int*)(&num));
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
			return renderState;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x000669C0 File Offset: 0x00065DC0
		public unsafe float GetRenderStateSingle(RenderStates state)
		{
			int num = 0;
			int renderState = this.GetRenderState((int)state, (int*)(&num));
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
			return (float)renderState;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00066A10 File Offset: 0x00065E10
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool GetRenderStateBoolean(RenderStates state)
		{
			int num = 0;
			int renderState = this.GetRenderState((int)state, (int*)(&num));
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
			return renderState != 0;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00066A64 File Offset: 0x00065E64
		public unsafe void BeginStateBlock()
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 240));
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

		// Token: 0x060005EE RID: 1518 RVA: 0x00066AC0 File Offset: 0x00065EC0
		public unsafe StateBlock EndStateBlock()
		{
			IDirect3DStateBlock9* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DStateBlock9**), this.m_lpUM, ref ptr, *(*(int*)this.m_lpUM + 244));
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
			if (ptr != null)
			{
				return new StateBlock(ptr, this);
			}
			return null;
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x00066B2C File Offset: 0x00065F2C
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x00066BA4 File Offset: 0x00065FA4
		public unsafe ClipStatus ClipStatus
		{
			get
			{
				ClipStatus result = default(ClipStatus);
				result = new ClipStatus();
				initblk(ref result, 0, sizeof(ClipStatus));
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DCLIPSTATUS9*), this.m_lpUM, ref result, *(*(int*)this.m_lpUM + 252));
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
			set
			{
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DCLIPSTATUS9 modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, ref value, *(*(int*)this.m_lpUM + 248));
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
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00066C5C File Offset: 0x0006605C
		[CLSCompliant(false)]
		internal unsafe int GetTextureStageState(int stage, TextureStageStates state, int* result)
		{
			uint result2 = 0U;
			*(int*)result = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)*), this.m_lpUM, stage, state, ref result2, *(*(int*)this.m_lpUM + 264));
			return (int)result2;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00066DDC File Offset: 0x000661DC
		[CLSCompliant(false)]
		internal unsafe int GetSamplerState(int stage, SamplerStageStates state, int* result)
		{
			uint result2 = 0U;
			*(int*)result = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)*), this.m_lpUM, stage, state, ref result2, *(*(int*)this.m_lpUM + 272));
			return (int)result2;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x000629D8 File Offset: 0x00061DD8
		public void SetSamplerState(int stage, SamplerStageStates state, float value)
		{
			this.SetSamplerState(stage, state, (int)value);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x000629B4 File Offset: 0x00061DB4
		public void SetSamplerState(int stage, SamplerStageStates state, [MarshalAs(UnmanagedType.U1)] bool value)
		{
			this.SetSamplerState(stage, state, value ? 1 : 0);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00066D80 File Offset: 0x00066180
		public unsafe void SetSamplerState(int stage, SamplerStageStates state, int value)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, stage, state, value, *(*(int*)this.m_lpUM + 276));
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

		// Token: 0x060005F6 RID: 1526 RVA: 0x00066E10 File Offset: 0x00066210
		public unsafe int GetSamplerStageStateInt32(int stage, SamplerStageStates state)
		{
			int num = 0;
			int samplerState = this.GetSamplerState(stage, state, (int*)(&num));
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
			return samplerState;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00066EAC File Offset: 0x000662AC
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool GetSamplerStageStateBoolean(int stage, SamplerStageStates state)
		{
			int num = 0;
			int samplerState = this.GetSamplerState(stage, state, (int*)(&num));
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
			return samplerState != 0;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00066E5C File Offset: 0x0006625C
		public unsafe float GetSamplerStageStateSingle(int stage, SamplerStageStates state)
		{
			int num = 0;
			int samplerState = this.GetSamplerState(stage, state, (int*)(&num));
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
			return (float)samplerState;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00062A1C File Offset: 0x00061E1C
		public void SetTextureStageState(int stage, TextureStageStates state, float value)
		{
			this.SetTextureStageState(stage, state, (int)value);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x000629F8 File Offset: 0x00061DF8
		public void SetTextureStageState(int stage, TextureStageStates state, [MarshalAs(UnmanagedType.U1)] bool value)
		{
			this.SetTextureStageState(stage, state, value ? 1 : 0);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00066C00 File Offset: 0x00066000
		public unsafe void SetTextureStageState(int stage, TextureStageStates state, int value)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, stage, state, value, *(*(int*)this.m_lpUM + 268));
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

		// Token: 0x060005FC RID: 1532 RVA: 0x00066C90 File Offset: 0x00066090
		public unsafe int GetTextureStageStateInt32(int stage, TextureStageStates state)
		{
			int num = 0;
			int textureStageState = this.GetTextureStageState(stage, state, (int*)(&num));
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
			return textureStageState;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00066D2C File Offset: 0x0006612C
		[return: MarshalAs(UnmanagedType.U1)]
		public unsafe bool GetTextureStageStateBoolean(int stage, TextureStageStates state)
		{
			int num = 0;
			int textureStageState = this.GetTextureStageState(stage, state, (int*)(&num));
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
			return textureStageState != 0;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00066CDC File Offset: 0x000660DC
		public unsafe float GetTextureStageStateSingle(int stage, TextureStageStates state)
		{
			int num = 0;
			int textureStageState = this.GetTextureStageState(stage, state, (int*)(&num));
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
			return (float)textureStageState;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00066F00 File Offset: 0x00066300
		public unsafe ValidateDeviceParams ValidateDevice()
		{
			ValidateDeviceParams result = default(ValidateDeviceParams);
			result.mResult = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)*), this.m_lpUM, ref result.dwPasses, *(*(int*)this.m_lpUM + 280));
			return result;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00066F44 File Offset: 0x00066344
		public unsafe void SetPaletteEntries(int paletteNumber, PaletteEntry[] entries)
		{
			if (entries == null)
			{
				throw new ArgumentNullException("entries");
			}
			if (entries.Length == 0)
			{
				throw new ArgumentNullException("entries");
			}
			if (entries.Length != 256)
			{
				throw new ArgumentException(string.Empty, "entries");
			}
			ref PaletteEntry paletteEntry& = ref entries[0];
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails.tagPALETTEENTRY modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, paletteNumber, ref paletteEntry&, *(*(int*)this.m_lpUM + 284));
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

		// Token: 0x06000601 RID: 1537 RVA: 0x00066FE0 File Offset: 0x000663E0
		public unsafe PaletteEntry[] GetPaletteEntries(int paletteNumber)
		{
			PaletteEntry[] array = new PaletteEntry[256];
			array.Initialize();
			ref PaletteEntry paletteEntry& = ref array[0];
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails.tagPALETTEENTRY*), this.m_lpUM, paletteNumber, ref paletteEntry&, *(*(int*)this.m_lpUM + 288));
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
			return array;
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x00067058 File Offset: 0x00066458
		// (set) Token: 0x06000602 RID: 1538 RVA: 0x000670B8 File Offset: 0x000664B8
		public unsafe int CurrentTexturePalette
		{
			get
			{
				uint result = 0U;
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32*), this.m_lpUM, ref result, *(*(int*)this.m_lpUM + 296));
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
				return (int)result;
			}
			set
			{
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32), this.m_lpUM, value, *(*(int*)this.m_lpUM + 292));
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
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00067114 File Offset: 0x00066514
		public unsafe void DrawPrimitives(PrimitiveType primitiveType, int startVertex, int primitiveCount)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,System.UInt32,System.UInt32), this.m_lpUM, primitiveType, startVertex, primitiveCount, *(*(int*)this.m_lpUM + 324));
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

		// Token: 0x06000605 RID: 1541 RVA: 0x00067170 File Offset: 0x00066570
		public unsafe void DrawIndexedPrimitives(PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primCount)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,System.Int32,System.UInt32,System.UInt32,System.UInt32,System.UInt32), this.m_lpUM, primitiveType, baseVertex, minVertexIndex, numVertices, startIndex, primCount, *(*(int*)this.m_lpUM + 328));
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

		// Token: 0x06000606 RID: 1542 RVA: 0x000671D4 File Offset: 0x000665D4
		public unsafe void DrawUserPrimitives(PrimitiveType primitiveType, int primitiveCount, object vertexStreamZeroData)
		{
			int num = DXHelp.GetObjectSize(vertexStreamZeroData);
			if (vertexStreamZeroData.GetType().IsArray)
			{
				num /= vertexStreamZeroData.Length;
			}
			GCHandle gchandle = GCHandle.Alloc(vertexStreamZeroData, GCHandleType.Pinned);
			GCHandle gchandle2 = gchandle;
			IntPtr intPtr = gchandle2.AddrOfPinnedObject();
			int num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,System.UInt32,System.Void modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32), this.m_lpUM, primitiveType, primitiveCount, intPtr.ToPointer(), num, *(*(int*)this.m_lpUM + 332));
			gchandle2.Free();
			if (num2 < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num2);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num2;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num2);
				}
			}
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00067274 File Offset: 0x00066674
		public unsafe void DrawIndexedUserPrimitives(PrimitiveType primitiveType, int minVertexIndex, int numVertexIndices, int primitiveCount, object indexData, [MarshalAs(UnmanagedType.U1)] bool sixteenBitIndices, object vertexStreamZeroData)
		{
			int num = DXHelp.GetObjectSize(vertexStreamZeroData);
			if (vertexStreamZeroData.GetType().IsArray)
			{
				num /= vertexStreamZeroData.Length;
			}
			GCHandle gchandle = GCHandle.Alloc(vertexStreamZeroData, GCHandleType.Pinned);
			GCHandle gchandle2 = gchandle;
			GCHandle gchandle3 = GCHandle.Alloc(indexData, GCHandleType.Pinned);
			GCHandle gchandle4 = gchandle3;
			int num2 = sixteenBitIndices ? 101 : 102;
			IntPtr intPtr = gchandle2.AddrOfPinnedObject();
			IntPtr intPtr2 = gchandle4.AddrOfPinnedObject();
			int num3 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32,System.UInt32,System.UInt32,System.UInt32,System.Void modopt(Microsoft.VisualC.IsConstModifier)*,System.Int32,System.Void modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32), this.m_lpUM, primitiveType, minVertexIndex, numVertexIndices, primitiveCount, intPtr2.ToPointer(), num2, intPtr.ToPointer(), num, *(*(int*)this.m_lpUM + 336));
			gchandle2.Free();
			gchandle4.Free();
			if (num3 < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num3);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num3;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num3);
				}
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000608 RID: 1544 RVA: 0x0006734C File Offset: 0x0006674C
		// (set) Token: 0x06000609 RID: 1545 RVA: 0x000673D8 File Offset: 0x000667D8
		public unsafe VertexShader VertexShader
		{
			get
			{
				IDirect3DVertexShader9* ptr = null;
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVertexShader9**), this.m_lpUM, ref ptr, *(*(int*)this.m_lpUM + 372));
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
				if (ptr != null)
				{
					if (this.pCachedResourcepVertexShader != null)
					{
						this.pCachedResourcepVertexShader.UpdateUnmanagedPointer(ptr);
					}
					else
					{
						this.pCachedResourcepVertexShader = new VertexShader(ptr, this);
					}
					return this.pCachedResourcepVertexShader;
				}
				return null;
			}
			set
			{
				IDirect3DVertexShader9* ptr;
				if (value != null)
				{
					ptr = value.m_lpUM;
				}
				else
				{
					ptr = null;
				}
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVertexShader9*), this.m_lpUM, ptr, *(*(int*)this.m_lpUM + 368));
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
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00067440 File Offset: 0x00066840
		[CLSCompliant(false)]
		private unsafe void SetVertexShaderConstantF(int startRegister, void* pData, int numberRegisters)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Single modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32), this.m_lpUM, startRegister, pData, numberRegisters, *(*(int*)this.m_lpUM + 376));
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

		// Token: 0x0600060B RID: 1547 RVA: 0x000674F8 File Offset: 0x000668F8
		[CLSCompliant(false)]
		private unsafe void SetVertexShaderConstantB(int startRegister, void* pData, int numberRegisters)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32 modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32), this.m_lpUM, startRegister, pData, numberRegisters, *(*(int*)this.m_lpUM + 392));
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

		// Token: 0x0600060C RID: 1548 RVA: 0x0006749C File Offset: 0x0006689C
		[CLSCompliant(false)]
		private unsafe void SetVertexShaderConstantI(int startRegister, void* pData, int numberRegisters)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32 modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32), this.m_lpUM, startRegister, pData, numberRegisters, *(*(int*)this.m_lpUM + 384));
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

		// Token: 0x0600060D RID: 1549 RVA: 0x00062B50 File Offset: 0x00061F50
		[CLSCompliant(false)]
		public unsafe void SetVertexShaderConstant(int startRegister, Vector4* constantData)
		{
			this.SetVertexShaderConstantF(startRegister, (void*)constantData, 1);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00062B34 File Offset: 0x00061F34
		[CLSCompliant(false)]
		public unsafe void SetVertexShaderConstant(int startRegister, Matrix* constantData)
		{
			this.SetVertexShaderConstantF(startRegister, (void*)constantData, 4);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00062B18 File Offset: 0x00061F18
		public unsafe void SetVertexShaderConstant(int startRegister, Vector4 constantData)
		{
			this.SetVertexShaderConstantF(startRegister, (void*)(&constantData), 1);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00062AFC File Offset: 0x00061EFC
		public unsafe void SetVertexShaderConstant(int startRegister, Matrix constantData)
		{
			this.SetVertexShaderConstantF(startRegister, (void*)(&constantData), 4);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00062AD8 File Offset: 0x00061ED8
		public void SetVertexShaderConstant(int startRegister, bool[] constantData)
		{
			ref void pData = ref constantData[0];
			this.SetVertexShaderConstantB(startRegister, ref pData, constantData.Length);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00062AB0 File Offset: 0x00061EB0
		public void SetVertexShaderConstant(int startRegister, int[] constantData)
		{
			ref void pData = ref constantData[0];
			this.SetVertexShaderConstantI(startRegister, ref pData, constantData.Length / 4);
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00062A8C File Offset: 0x00061E8C
		public void SetVertexShaderConstant(int startRegister, Vector4[] constantData)
		{
			ref void pData = ref constantData[0];
			this.SetVertexShaderConstantF(startRegister, ref pData, constantData.Length);
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00062A64 File Offset: 0x00061E64
		public void SetVertexShaderConstant(int startRegister, Matrix[] constantData)
		{
			ref void pData = ref constantData[0];
			this.SetVertexShaderConstantF(startRegister, ref pData, constantData.Length << 2);
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00062A3C File Offset: 0x00061E3C
		public void SetVertexShaderConstant(int startRegister, float[] constantData)
		{
			ref void pData = ref constantData[0];
			this.SetVertexShaderConstantF(startRegister, ref pData, constantData.Length / 4);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00062B6C File Offset: 0x00061F6C
		public void SetVertexShaderConstantSingle(int startRegister, GraphicsStream constantData, int numberRegisters)
		{
			this.SetVertexShaderConstantF(startRegister, constantData.InternalDataPointer, numberRegisters);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00062B8C File Offset: 0x00061F8C
		public void SetVertexShaderConstantInt32(int startRegister, GraphicsStream constantData, int numberRegisters)
		{
			this.SetVertexShaderConstantI(startRegister, constantData.InternalDataPointer, numberRegisters);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00062BAC File Offset: 0x00061FAC
		public void SetVertexShaderConstantBoolean(int startRegister, GraphicsStream constantData, int numberRegisters)
		{
			this.SetVertexShaderConstantB(startRegister, constantData.InternalDataPointer, numberRegisters);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00067554 File Offset: 0x00066954
		public unsafe float[] GetVertexShaderSingleConstant(int startRegister, int constantCount)
		{
			float[] array = new float[constantCount];
			array.Initialize();
			ref float single& = ref array[0];
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Single*,System.UInt32), this.m_lpUM, startRegister, ref single&, constantCount, *(*(int*)this.m_lpUM + 380));
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
			return array;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x000675C8 File Offset: 0x000669C8
		public unsafe int[] GetVertexShaderInt32Constant(int startRegister, int constantCount)
		{
			int[] array = new int[constantCount];
			array.Initialize();
			ref int int32& = ref array[0];
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32*,System.UInt32), this.m_lpUM, startRegister, ref int32&, constantCount, *(*(int*)this.m_lpUM + 388));
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
			return array;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0006763C File Offset: 0x00066A3C
		public unsafe bool[] GetVertexShaderBooleanConstant(int startRegister, int constantCount)
		{
			bool[] array = new bool[constantCount];
			array.Initialize();
			ref bool boolean& = ref array[0];
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32*,System.UInt32), this.m_lpUM, startRegister, ref boolean&, constantCount, *(*(int*)this.m_lpUM + 396));
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
			return array;
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00067840 File Offset: 0x00066C40
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x000678CC File Offset: 0x00066CCC
		public unsafe PixelShader PixelShader
		{
			get
			{
				IDirect3DPixelShader9* ptr = null;
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DPixelShader9**), this.m_lpUM, ref ptr, *(*(int*)this.m_lpUM + 432));
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
				if (ptr != null)
				{
					if (this.pCachedResourcepPixelShader != null)
					{
						this.pCachedResourcepPixelShader.UpdateUnmanagedPointer(ptr);
					}
					else
					{
						this.pCachedResourcepPixelShader = new PixelShader(ptr, this);
					}
					return this.pCachedResourcepPixelShader;
				}
				return null;
			}
			set
			{
				IDirect3DPixelShader9* ptr;
				if (value != null)
				{
					ptr = value.m_lpUM;
				}
				else
				{
					ptr = null;
				}
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DPixelShader9*), this.m_lpUM, ptr, *(*(int*)this.m_lpUM + 428));
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
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00067934 File Offset: 0x00066D34
		[CLSCompliant(false)]
		private unsafe void SetPixelShaderConstantF(int startRegister, void* pData, int numberRegisters)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Single modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32), this.m_lpUM, startRegister, pData, numberRegisters, *(*(int*)this.m_lpUM + 436));
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

		// Token: 0x0600061F RID: 1567 RVA: 0x000679EC File Offset: 0x00066DEC
		[CLSCompliant(false)]
		private unsafe void SetPixelShaderConstantB(int startRegister, void* pData, int numberRegisters)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32 modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32), this.m_lpUM, startRegister, pData, numberRegisters, *(*(int*)this.m_lpUM + 452));
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

		// Token: 0x06000620 RID: 1568 RVA: 0x00067990 File Offset: 0x00066D90
		[CLSCompliant(false)]
		private unsafe void SetPixelShaderConstantI(int startRegister, void* pData, int numberRegisters)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32 modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32), this.m_lpUM, startRegister, pData, numberRegisters, *(*(int*)this.m_lpUM + 444));
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

		// Token: 0x06000621 RID: 1569 RVA: 0x00062CE0 File Offset: 0x000620E0
		[CLSCompliant(false)]
		public unsafe void SetPixelShaderConstant(int startRegister, Vector4* constantData)
		{
			this.SetPixelShaderConstantF(startRegister, (void*)constantData, 1);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00062CC4 File Offset: 0x000620C4
		[CLSCompliant(false)]
		public unsafe void SetPixelShaderConstant(int startRegister, Matrix* constantData)
		{
			this.SetPixelShaderConstantF(startRegister, (void*)constantData, 4);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00062CA8 File Offset: 0x000620A8
		public unsafe void SetPixelShaderConstant(int startRegister, Vector4 constantData)
		{
			this.SetPixelShaderConstantF(startRegister, (void*)(&constantData), 1);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00062C8C File Offset: 0x0006208C
		public unsafe void SetPixelShaderConstant(int startRegister, Matrix constantData)
		{
			this.SetPixelShaderConstantF(startRegister, (void*)(&constantData), 4);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00062C68 File Offset: 0x00062068
		public void SetPixelShaderConstant(int startRegister, bool[] constantData)
		{
			ref void pData = ref constantData[0];
			this.SetPixelShaderConstantB(startRegister, ref pData, constantData.Length);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00062C40 File Offset: 0x00062040
		public void SetPixelShaderConstant(int startRegister, int[] constantData)
		{
			ref void pData = ref constantData[0];
			this.SetPixelShaderConstantI(startRegister, ref pData, constantData.Length / 4);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00062C1C File Offset: 0x0006201C
		public void SetPixelShaderConstant(int startRegister, Vector4[] constantData)
		{
			ref void pData = ref constantData[0];
			this.SetPixelShaderConstantF(startRegister, ref pData, constantData.Length);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00062BF4 File Offset: 0x00061FF4
		public void SetPixelShaderConstant(int startRegister, Matrix[] constantData)
		{
			ref void pData = ref constantData[0];
			this.SetPixelShaderConstantF(startRegister, ref pData, constantData.Length << 2);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00062BCC File Offset: 0x00061FCC
		public void SetPixelShaderConstant(int startRegister, float[] constantData)
		{
			ref void pData = ref constantData[0];
			this.SetPixelShaderConstantF(startRegister, ref pData, constantData.Length / 4);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00062CFC File Offset: 0x000620FC
		public void SetPixelShaderConstantSingle(int startRegister, GraphicsStream constantData, int numberRegisters)
		{
			this.SetPixelShaderConstantF(startRegister, constantData.InternalDataPointer, numberRegisters);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00062D1C File Offset: 0x0006211C
		public void SetPixelShaderConstantInt32(int startRegister, GraphicsStream constantData, int numberRegisters)
		{
			this.SetPixelShaderConstantI(startRegister, constantData.InternalDataPointer, numberRegisters);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00062D3C File Offset: 0x0006213C
		public void SetPixelShaderConstantBoolean(int startRegister, GraphicsStream constantData, int numberRegisters)
		{
			this.SetPixelShaderConstantB(startRegister, constantData.InternalDataPointer, numberRegisters);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00067A48 File Offset: 0x00066E48
		public unsafe float[] GetPixelShaderSingleConstant(int startRegister, int constantCount)
		{
			float[] array = new float[constantCount];
			array.Initialize();
			ref float single& = ref array[0];
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Single*,System.UInt32), this.m_lpUM, startRegister, ref single&, constantCount, *(*(int*)this.m_lpUM + 440));
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
			return array;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00067ABC File Offset: 0x00066EBC
		public unsafe int[] GetPixelShaderInt32Constant(int startRegister, int constantCount)
		{
			int[] array = new int[constantCount];
			array.Initialize();
			ref int int32& = ref array[0];
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32*,System.UInt32), this.m_lpUM, startRegister, ref int32&, constantCount, *(*(int*)this.m_lpUM + 448));
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
			return array;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00067B30 File Offset: 0x00066F30
		public unsafe bool[] GetPixelShaderBooleanConstant(int startRegister, int constantCount)
		{
			bool[] array = new bool[constantCount];
			array.Initialize();
			ref bool boolean& = ref array[0];
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Int32*,System.UInt32), this.m_lpUM, startRegister, ref boolean&, constantCount, *(*(int*)this.m_lpUM + 456));
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
			return array;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00062DB4 File Offset: 0x000621B4
		public unsafe void DrawRectanglePatch(int handle, Plane numSegs, RectanglePatchInformation rectPatchInformation)
		{
			this.DrawRectanglePatch(handle, numSegs, &rectPatchInformation);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00062D98 File Offset: 0x00062198
		public unsafe void DrawRectanglePatch(int handle, float[] numSegs, RectanglePatchInformation rectPatchInformation)
		{
			this.DrawRectanglePatch(handle, numSegs, &rectPatchInformation);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00067C08 File Offset: 0x00067008
		[CLSCompliant(false)]
		public unsafe void DrawRectanglePatch(int handle, Plane numSegs, RectanglePatchInformation* rectPatchInformation)
		{
			ref float single& = ref numSegs;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Single modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails._D3DRECTPATCH_INFO modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, handle, ref single&, rectPatchInformation, *(*(int*)this.m_lpUM + 460));
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

		// Token: 0x06000633 RID: 1587 RVA: 0x00067BA4 File Offset: 0x00066FA4
		[CLSCompliant(false)]
		public unsafe void DrawRectanglePatch(int handle, float[] numSegs, RectanglePatchInformation* rectPatchInformation)
		{
			ref float single& = ref numSegs[0];
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Single modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails._D3DRECTPATCH_INFO modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, handle, ref single&, rectPatchInformation, *(*(int*)this.m_lpUM + 460));
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

		// Token: 0x06000634 RID: 1588 RVA: 0x00062D78 File Offset: 0x00062178
		public void DrawRectanglePatch(int handle, Plane numSegs)
		{
			this.DrawRectanglePatch(handle, numSegs, null);
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x00062D5C File Offset: 0x0006215C
		public void DrawRectanglePatch(int handle, float[] numSegs)
		{
			this.DrawRectanglePatch(handle, numSegs, null);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00062E2C File Offset: 0x0006222C
		public unsafe void DrawTrianglePatch(int handle, Plane numSegs, TrianglePatchInformation triPatchInformation)
		{
			this.DrawTrianglePatch(handle, numSegs, &triPatchInformation);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00062E10 File Offset: 0x00062210
		public unsafe void DrawTrianglePatch(int handle, float[] numSegs, TrianglePatchInformation triPatchInformation)
		{
			this.DrawTrianglePatch(handle, numSegs, &triPatchInformation);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00067CCC File Offset: 0x000670CC
		[CLSCompliant(false)]
		public unsafe void DrawTrianglePatch(int handle, Plane numSegs, TrianglePatchInformation* triPatchInformation)
		{
			ref float single& = ref numSegs;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Single modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails._D3DTRIPATCH_INFO modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, handle, ref single&, triPatchInformation, *(*(int*)this.m_lpUM + 464));
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

		// Token: 0x06000639 RID: 1593 RVA: 0x00067C68 File Offset: 0x00067068
		[CLSCompliant(false)]
		public unsafe void DrawTrianglePatch(int handle, float[] numSegs, TrianglePatchInformation* triPatchInformation)
		{
			ref float single& = ref numSegs[0];
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.Single modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails._D3DTRIPATCH_INFO modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, handle, ref single&, triPatchInformation, *(*(int*)this.m_lpUM + 464));
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

		// Token: 0x0600063A RID: 1594 RVA: 0x00062DF0 File Offset: 0x000621F0
		public void DrawTrianglePatch(int handle, Plane numSegs)
		{
			this.DrawTrianglePatch(handle, numSegs, null);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00062DD4 File Offset: 0x000621D4
		public void DrawTrianglePatch(int handle, float[] numSegs)
		{
			this.DrawTrianglePatch(handle, numSegs, null);
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00067D2C File Offset: 0x0006712C
		public unsafe void DeletePatch(int handle)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32), this.m_lpUM, handle, *(*(int*)this.m_lpUM + 468));
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

		// Token: 0x0600063D RID: 1597 RVA: 0x00067D88 File Offset: 0x00067188
		public unsafe void UpdateTexture(BaseTexture sourceTexture, BaseTexture destinationTexture)
		{
			IDirect3DBaseTexture9* ptr;
			if (sourceTexture != null)
			{
				ptr = sourceTexture.m_lpUM;
			}
			else
			{
				ptr = null;
			}
			IDirect3DBaseTexture9* ptr2;
			if (destinationTexture != null)
			{
				ptr2 = destinationTexture.m_lpUM;
			}
			else
			{
				ptr2 = null;
			}
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DBaseTexture9*,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DBaseTexture9*), this.m_lpUM, ptr, ptr2, *(*(int*)this.m_lpUM + 124));
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

		// Token: 0x0600063E RID: 1598 RVA: 0x00067FE0 File Offset: 0x000673E0
		public unsafe void SetTexture(int stage, BaseTexture texture)
		{
			IDirect3DBaseTexture9* ptr;
			if (texture != null)
			{
				ptr = texture.m_lpUM;
			}
			else
			{
				ptr = null;
			}
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),Microsoft.DirectX.PrivateImplementationDetails.IDirect3DBaseTexture9*), this.m_lpUM, stage, ptr, *(*(int*)this.m_lpUM + 260));
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

		// Token: 0x0600063F RID: 1599 RVA: 0x00067E00 File Offset: 0x00067200
		public unsafe Texture GetTexture(int stage)
		{
			IDirect3DBaseTexture9* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),Microsoft.DirectX.PrivateImplementationDetails.IDirect3DBaseTexture9**), this.m_lpUM, stage, ref ptr, *(*(int*)this.m_lpUM + 256));
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
			if (ptr == null)
			{
				return null;
			}
			IDirect3DTexture9* ptr2 = null;
			if (calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._GUID modopt(Microsoft.VisualC.IsConstModifier)* modopt(Microsoft.VisualC.IsCXXReferenceModifier),System.Void**), ptr, ref <Module>.IID_IDirect3DTexture9, ref ptr2, *(*(int*)ptr)) >= 0)
			{
				if (ptr != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), ptr, *(*(int*)ptr + 8)))
				{
					ptr = null;
				}
				if (ptr2 != null)
				{
					return new Texture(ptr2, this, Pool.SystemMemory);
				}
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00067EA0 File Offset: 0x000672A0
		public unsafe CubeTexture GetCubeTexture(int stage)
		{
			IDirect3DBaseTexture9* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),Microsoft.DirectX.PrivateImplementationDetails.IDirect3DBaseTexture9**), this.m_lpUM, stage, ref ptr, *(*(int*)this.m_lpUM + 256));
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
			if (ptr == null)
			{
				return null;
			}
			IDirect3DCubeTexture9* ptr2 = null;
			if (calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._GUID modopt(Microsoft.VisualC.IsConstModifier)* modopt(Microsoft.VisualC.IsCXXReferenceModifier),System.Void**), ptr, ref <Module>.IID_IDirect3DCubeTexture9, ref ptr2, *(*(int*)ptr)) >= 0)
			{
				if (ptr != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), ptr, *(*(int*)ptr + 8)))
				{
					ptr = null;
				}
				if (ptr2 != null)
				{
					return new CubeTexture(ptr2, this, Pool.SystemMemory);
				}
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00067F40 File Offset: 0x00067340
		public unsafe VolumeTexture GetVolumeTexture(int stage)
		{
			IDirect3DBaseTexture9* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),Microsoft.DirectX.PrivateImplementationDetails.IDirect3DBaseTexture9**), this.m_lpUM, stage, ref ptr, *(*(int*)this.m_lpUM + 256));
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
			if (ptr == null)
			{
				return null;
			}
			IDirect3DVolumeTexture9* ptr2 = null;
			if (calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._GUID modopt(Microsoft.VisualC.IsConstModifier)* modopt(Microsoft.VisualC.IsCXXReferenceModifier),System.Void**), ptr, ref <Module>.IID_IDirect3DVolumeTexture9, ref ptr2, *(*(int*)ptr)) >= 0)
			{
				if (ptr != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), ptr, *(*(int*)ptr + 8)))
				{
					ptr = null;
				}
				if (ptr2 != null)
				{
					return new VolumeTexture(ptr2, this, Pool.SystemMemory);
				}
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0006804C File Offset: 0x0006744C
		public unsafe void SetCursorProperties(int hotSpotX, int hotSpotY, Surface cursorBitmap)
		{
			IDirect3DSurface9* ptr;
			if (cursorBitmap != null)
			{
				ptr = cursorBitmap.m_lpUM;
			}
			else
			{
				ptr = null;
			}
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9*), this.m_lpUM, hotSpotX, hotSpotY, ptr, *(*(int*)this.m_lpUM + 40));
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

		// Token: 0x06000643 RID: 1603 RVA: 0x000680B4 File Offset: 0x000674B4
		public unsafe void SetCursor(Cursor cursor, [MarshalAs(UnmanagedType.U1)] bool addWaterMark)
		{
			int num = -2147467259;
			IDirect3DSurface9* ptr = null;
			HDC__* ptr2 = null;
			HDC__* ptr3 = null;
			HDC__* ptr4 = null;
			uint* ptr5 = null;
			uint* ptr6 = null;
			_ICONINFO iconinfo;
			initblk(ref iconinfo, 0, 20);
			tagBITMAP tagBITMAP;
			if (<Module>.GetIconInfo((HICON__*)((cursor == null) ? null : cursor.Handle.ToPointer()), &iconinfo) != null && 0 != <Module>.GetObjectA(*(ref iconinfo + 12), 24, (void*)(&tagBITMAP)))
			{
				uint num2 = (uint)(*(ref tagBITMAP + 4));
				uint num3 = (uint)(*(ref tagBITMAP + 8));
				int num4;
				uint num5;
				if (*(ref iconinfo + 16) == 0)
				{
					num4 = 1;
					num5 = (uint)(*(ref tagBITMAP + 8)) >> 1;
				}
				else
				{
					num4 = 0;
					num5 = (uint)(*(ref tagBITMAP + 8));
				}
				num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9**,System.Void**), this.m_lpUM, *(ref tagBITMAP + 4), num5, 21, 2, ref ptr, 0, *(*(int*)this.m_lpUM + 144));
				if (num >= 0)
				{
					ptr6 = <Module>.@new(num3 * num2 << 2);
					tagBITMAPINFO tagBITMAPINFO;
					initblk(ref tagBITMAPINFO, 0, 44);
					tagBITMAPINFO = 40;
					*(ref tagBITMAPINFO + 4) = (int)num2;
					*(ref tagBITMAPINFO + 8) = (int)num3;
					*(ref tagBITMAPINFO + 12) = 1;
					*(ref tagBITMAPINFO + 14) = 32;
					*(ref tagBITMAPINFO + 16) = 0;
					ptr4 = <Module>.GetDC(null);
					ptr3 = <Module>.CreateCompatibleDC(ptr4);
					if (ptr3 == null)
					{
						num = -2147467259;
					}
					else
					{
						void* ptr7 = <Module>.SelectObject(ptr3, *(ref iconinfo + 12));
						<Module>.GetDIBits(ptr3, *(ref iconinfo + 12), 0U, num3, (void*)ptr6, &tagBITMAPINFO, 0U);
						<Module>.SelectObject(ptr3, ptr7);
						if (num4 == 0)
						{
							ptr5 = <Module>.@new(num5 * num2 << 2);
							ptr2 = <Module>.CreateCompatibleDC(<Module>.GetDC(null));
							if (ptr2 == null)
							{
								num = -2147467259;
								goto IL_2ED;
							}
							<Module>.SelectObject(ptr2, *(ref iconinfo + 16));
							<Module>.GetDIBits(ptr2, *(ref iconinfo + 16), 0U, num5, (void*)ptr5, &tagBITMAPINFO, 0U);
						}
						_D3DLOCKED_RECT d3DLOCKED_RECT;
						object obj = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails._D3DLOCKED_RECT*,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), ptr, ref d3DLOCKED_RECT, 0, 0, *(*(int*)ptr + 52));
						uint num6 = 0U;
						if (0U < num5)
						{
							uint num7 = num2 << 2;
							uint num8 = -num2;
							int num9 = *(ref d3DLOCKED_RECT + 4);
							uint num10 = num2 * 4294967292U;
							int num11 = (int)((num3 - 1U) * num2);
							int num12 = (num5 * 4U - 4U) * num2 / (uint)sizeof(uint) + ptr6;
							do
							{
								uint num13 = 0U;
								if (0U < num2)
								{
									int num14 = 15;
									int num15 = num9;
									int num16 = num12;
									uint* ptr8 = ptr5 - ptr6;
									do
									{
										uint num17;
										bool flag;
										if (num4 != 0)
										{
											num17 = (uint)(*num16);
											flag = (*(int*)((num11 + (int)num13) * 4 / sizeof(uint) + ptr6) != 0);
										}
										else
										{
											num17 = (uint)(*(int*)(ptr8 + num16 / sizeof(uint)));
											flag = (*num16 != 0);
										}
										if (!flag)
										{
											*num15 = (int)(num17 | 4278190080U);
										}
										else
										{
											*num15 = 0;
										}
										if (addWaterMark && num13 < 12U && num6 < 5U)
										{
											$ArrayType$0x855db7ad $ArrayType$0x855db7ad = 52416;
											*(ref $ArrayType$0x855db7ad + 2) = (short)41632;
											*(ref $ArrayType$0x855db7ad + 4) = (short)42144;
											*(ref $ArrayType$0x855db7ad + 6) = (short)41632;
											*(ref $ArrayType$0x855db7ad + 8) = (short)52416;
											if (((int)(*(num6 * 2U + ref $ArrayType$0x855db7ad)) & 1 << num14) != 0)
											{
												*num15 |= -8355712;
											}
										}
										num13 += 1U;
										num16 += 4;
										num15 += 4;
										num14--;
									}
									while (num13 < num2);
								}
								num6 += 1U;
								num9 = (int)(num7 + (uint)num9);
								num12 = (int)(num10 + (uint)num12);
								num11 = (int)(num8 + (uint)num11);
							}
							while (num6 < num5);
						}
						object obj2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), ptr, *(*(int*)ptr + 56));
						num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9*), this.m_lpUM, *(ref iconinfo + 4), *(ref iconinfo + 8), ptr, *(*(int*)this.m_lpUM + 40));
						if (num >= 0)
						{
							num = 0;
						}
					}
				}
			}
			IL_2ED:
			if (*(ref iconinfo + 12) != 0)
			{
				<Module>.DeleteObject(*(ref iconinfo + 12));
			}
			if (*(ref iconinfo + 16) != 0)
			{
				<Module>.DeleteObject(*(ref iconinfo + 16));
			}
			if (ptr4 != null)
			{
				<Module>.ReleaseDC(null, ptr4);
			}
			if (ptr2 != null)
			{
				<Module>.DeleteDC(ptr2);
			}
			if (ptr3 != null)
			{
				<Module>.DeleteDC(ptr3);
			}
			if (ptr5 != null)
			{
				<Module>.delete((void*)ptr5);
			}
			if (ptr6 != null)
			{
				<Module>.delete((void*)ptr6);
			}
			if (ptr != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), ptr, *(*(int*)ptr + 8)))
			{
				ptr = null;
			}
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

		// Token: 0x06000644 RID: 1604 RVA: 0x0006845C File Offset: 0x0006785C
		public unsafe Surface GetBackBuffer(int swapChain, int backBuffer, BackBufferType backBufferType)
		{
			IDirect3DSurface9* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9**), this.m_lpUM, swapChain, backBuffer, backBufferType, ref ptr, *(*(int*)this.m_lpUM + 72));
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
			if (ptr != null)
			{
				return new Surface(ptr, this);
			}
			return null;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x000684C8 File Offset: 0x000678C8
		public unsafe Surface CreateRenderTarget(int width, int height, Format format, MultiSampleType multiSample, int multiSampleQuality, [MarshalAs(UnmanagedType.U1)] bool lockable)
		{
			IDirect3DSurface9* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.Int32,System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9**,System.Void**), this.m_lpUM, width, height, format, multiSample, multiSampleQuality, lockable, ref ptr, 0, *(*(int*)this.m_lpUM + 112));
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
			if (ptr != null)
			{
				return new Surface(ptr, this);
			}
			return null;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00068538 File Offset: 0x00067938
		public unsafe Surface CreateDepthStencilSurface(int width, int height, DepthFormat format, MultiSampleType multiSample, int multiSampleQuality, [MarshalAs(UnmanagedType.U1)] bool discard)
		{
			IDirect3DSurface9* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.Int32,System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9**,System.Void**), this.m_lpUM, width, height, format, multiSample, multiSampleQuality, discard, ref ptr, 0, *(*(int*)this.m_lpUM + 116));
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
			if (ptr != null)
			{
				return new Surface(ptr, this);
			}
			return null;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x000685A8 File Offset: 0x000679A8
		public unsafe Surface CreateOffscreenPlainSurface(int width, int height, Format format, Pool pool)
		{
			IDirect3DSurface9* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.Int32,System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9**,System.Void**), this.m_lpUM, width, height, format, pool, ref ptr, 0, *(*(int*)this.m_lpUM + 144));
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
			if (ptr != null)
			{
				return new Surface(ptr, this);
			}
			return null;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00068788 File Offset: 0x00067B88
		internal unsafe void UpdateSurfaceInternal(Surface sourceSurface, ref Rectangle sourceRect, Surface destinationSurface, ref Point destPoint)
		{
			ref Rectangle rectangle& = ref sourceRect;
			ref Point point& = ref destPoint;
			void* ptr = null;
			void* ptr2 = null;
			if (ref sourceRect != null)
			{
				ptr = ref rectangle&;
			}
			if (ref destPoint != null)
			{
				ptr2 = ref point&;
			}
			IDirect3DSurface9* ptr3;
			if (sourceSurface != null)
			{
				ptr3 = sourceSurface.m_lpUM;
			}
			else
			{
				ptr3 = null;
			}
			IDirect3DSurface9* ptr4;
			if (destinationSurface != null)
			{
				ptr4 = destinationSurface.m_lpUM;
			}
			else
			{
				ptr4 = null;
			}
			if (ptr != null)
			{
				*(int*)((byte*)ptr + 8) = *(int*)((byte*)ptr + 8) + *(int*)ptr;
				*(int*)((byte*)ptr + 12) = *(int*)((byte*)ptr + 12) + *(int*)((byte*)ptr + 4);
			}
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9*,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9*,Microsoft.DirectX.PrivateImplementationDetails.tagPOINT modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, ptr3, ptr, ptr4, ptr2, *(*(int*)this.m_lpUM + 120));
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

		// Token: 0x06000649 RID: 1609 RVA: 0x00062EAC File Offset: 0x000622AC
		public void UpdateSurface(Surface sourceSurface, Surface destinationSurface)
		{
			this.UpdateSurfaceInternal(sourceSurface, 0, destinationSurface, 0);
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00062E8C File Offset: 0x0006228C
		public void UpdateSurface(Surface sourceSurface, Surface destinationSurface, Point destPoint)
		{
			this.UpdateSurfaceInternal(sourceSurface, 0, destinationSurface, ref destPoint);
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00062E6C File Offset: 0x0006226C
		public void UpdateSurface(Surface sourceSurface, Rectangle sourceRect, Surface destinationSurface)
		{
			this.UpdateSurfaceInternal(sourceSurface, ref sourceRect, destinationSurface, 0);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00062E4C File Offset: 0x0006224C
		public void UpdateSurface(Surface sourceSurface, Rectangle sourceRect, Surface destinationSurface, Point destPoint)
		{
			this.UpdateSurfaceInternal(sourceSurface, ref sourceRect, destinationSurface, ref destPoint);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0006883C File Offset: 0x00067C3C
		public unsafe void GetFrontBufferData(int swapChain, Surface buffer)
		{
			IDirect3DSurface9* ptr;
			if (buffer != null)
			{
				ptr = buffer.m_lpUM;
			}
			else
			{
				ptr = null;
			}
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9*), this.m_lpUM, swapChain, ptr, *(*(int*)this.m_lpUM + 132));
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

		// Token: 0x0600064E RID: 1614 RVA: 0x000688A8 File Offset: 0x00067CA8
		public unsafe void GetRenderTargetData(Surface renderTarget, Surface destSurface)
		{
			IDirect3DSurface9* ptr;
			if (destSurface != null)
			{
				ptr = destSurface.m_lpUM;
			}
			else
			{
				ptr = null;
			}
			IDirect3DSurface9* ptr2;
			if (renderTarget != null)
			{
				ptr2 = renderTarget.m_lpUM;
			}
			else
			{
				ptr2 = null;
			}
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9*,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9*), this.m_lpUM, ptr2, ptr, *(*(int*)this.m_lpUM + 128));
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

		// Token: 0x0600064F RID: 1615 RVA: 0x00068924 File Offset: 0x00067D24
		public unsafe void SetRenderTarget(int renderTargetIndex, Surface renderTarget)
		{
			IDirect3DSurface9* ptr;
			if (renderTarget != null)
			{
				ptr = renderTarget.m_lpUM;
			}
			else
			{
				ptr = null;
			}
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9*), this.m_lpUM, renderTargetIndex, ptr, *(*(int*)this.m_lpUM + 148));
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

		// Token: 0x06000650 RID: 1616 RVA: 0x00068990 File Offset: 0x00067D90
		public unsafe Surface GetRenderTarget(int renderTargetIndex)
		{
			IDirect3DSurface9* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier),Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9**), this.m_lpUM, renderTargetIndex, ref ptr, *(*(int*)this.m_lpUM + 152));
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
			if (ptr != null)
			{
				return new Surface(ptr, this);
			}
			return null;
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x000689FC File Offset: 0x00067DFC
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x00068A68 File Offset: 0x00067E68
		public unsafe Surface DepthStencilSurface
		{
			get
			{
				IDirect3DSurface9* ptr = null;
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9**), this.m_lpUM, ref ptr, *(*(int*)this.m_lpUM + 160));
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
				if (ptr != null)
				{
					return new Surface(ptr, this);
				}
				return null;
			}
			set
			{
				IDirect3DSurface9* ptr;
				if (value != null)
				{
					ptr = value.m_lpUM;
				}
				else
				{
					ptr = null;
				}
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9*), this.m_lpUM, ptr, *(*(int*)this.m_lpUM + 156));
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
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00062EC8 File Offset: 0x000622C8
		public void ProcessVertices(int srcStartIndex, int destIndex, int vertexCount, VertexBuffer destBuffer, VertexDeclaration vertexDeclaration)
		{
			this.ProcessVertices(srcStartIndex, destIndex, vertexCount, destBuffer, vertexDeclaration, true);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00068AD0 File Offset: 0x00067ED0
		public unsafe void ProcessVertices(int srcStartIndex, int destIndex, int vertexCount, VertexBuffer destBuffer, VertexDeclaration vertexDeclaration, [MarshalAs(UnmanagedType.U1)] bool copyData)
		{
			IDirect3DVertexBuffer9* ptr;
			if (destBuffer != null)
			{
				ptr = destBuffer.m_lpUM;
			}
			else
			{
				ptr = null;
			}
			IDirect3DVertexDeclaration9* ptr2;
			if (vertexDeclaration != null)
			{
				ptr2 = vertexDeclaration.m_lpUM;
			}
			else
			{
				ptr2 = null;
			}
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVertexBuffer9*,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVertexDeclaration9*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, srcStartIndex, destIndex, vertexCount, ptr, ptr2, copyData ? 0 : 1, *(*(int*)this.m_lpUM + 340));
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

		// Token: 0x06000655 RID: 1621 RVA: 0x00068BC8 File Offset: 0x00067FC8
		public unsafe void SetStreamSource(int streamNumber, VertexBuffer streamData, int offsetInBytes)
		{
			IDirect3DVertexBuffer9* ptr;
			if (streamData != null)
			{
				ptr = streamData.m_lpUM;
			}
			else
			{
				ptr = null;
			}
			int num = 0;
			if (streamData != null)
			{
				num = streamData.TypeSize;
			}
			int num2 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVertexBuffer9*,System.UInt32,System.UInt32), this.m_lpUM, streamNumber, ptr, offsetInBytes, num, *(*(int*)this.m_lpUM + 400));
			if (num2 < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num2);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num2;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num2);
				}
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00068B5C File Offset: 0x00067F5C
		public unsafe void SetStreamSource(int streamNumber, VertexBuffer streamData, int offsetInBytes, int stride)
		{
			IDirect3DVertexBuffer9* ptr;
			if (streamData != null)
			{
				ptr = streamData.m_lpUM;
			}
			else
			{
				ptr = null;
			}
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVertexBuffer9*,System.UInt32,System.UInt32), this.m_lpUM, streamNumber, ptr, offsetInBytes, stride, *(*(int*)this.m_lpUM + 400));
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

		// Token: 0x06000657 RID: 1623 RVA: 0x00068C44 File Offset: 0x00068044
		public unsafe VertexBuffer GetStreamSource(int streamNumber, out int offsetInBytes, out int stride)
		{
			IDirect3DVertexBuffer9* ptr = null;
			uint num = (uint)stride;
			uint num2 = (uint)offsetInBytes;
			int num3 = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVertexBuffer9**,System.UInt32*,System.UInt32*), this.m_lpUM, streamNumber, ref ptr, ref num2, ref num, *(*(int*)this.m_lpUM + 404));
			stride = (int)num;
			offsetInBytes = (int)num2;
			if (num3 < 0)
			{
				if (!DirectXException.IsExceptionIgnored)
				{
					Exception exceptionFromResultInternal = GraphicsException.GetExceptionFromResultInternal(num3);
					DirectXException ex = exceptionFromResultInternal as DirectXException;
					if (ex != null)
					{
						ex.ErrorCode = num3;
						throw ex;
					}
					throw exceptionFromResultInternal;
				}
				else
				{
					<Module>.SetLastError(num3);
				}
			}
			if (ptr != null)
			{
				return new VertexBuffer(ptr, this, Usage.None, VertexFormats.Texture0, Pool.SystemMemory);
			}
			return null;
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00068DEC File Offset: 0x000681EC
		// (set) Token: 0x06000658 RID: 1624 RVA: 0x00068D84 File Offset: 0x00068184
		public unsafe IndexBuffer Indices
		{
			get
			{
				IDirect3DIndexBuffer9* ptr = null;
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DIndexBuffer9**), this.m_lpUM, ref ptr, *(*(int*)this.m_lpUM + 420));
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
				if (ptr != null)
				{
					return new IndexBuffer(ptr, this, Usage.None, Pool.Default);
				}
				return null;
			}
			set
			{
				IDirect3DIndexBuffer9* ptr;
				if (value != null)
				{
					ptr = value.m_lpUM;
				}
				else
				{
					ptr = null;
				}
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DIndexBuffer9*), this.m_lpUM, ptr, *(*(int*)this.m_lpUM + 416));
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
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00068CC8 File Offset: 0x000680C8
		public unsafe void SetStreamSourceFrequency(int streamNumber, int divider)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32), this.m_lpUM, streamNumber, divider, *(*(int*)this.m_lpUM + 408));
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

		// Token: 0x0600065B RID: 1627 RVA: 0x00068D24 File Offset: 0x00068124
		public unsafe int GetStreamSourceFrequency(int streamNumber)
		{
			uint result = 0U;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,System.UInt32*), this.m_lpUM, streamNumber, ref result, *(*(int*)this.m_lpUM + 412));
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
			return (int)result;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00068618 File Offset: 0x00067A18
		public unsafe void StretchRectangle(Surface sourceSurface, Rectangle sourceRectangle, Surface destSurface, Rectangle destRectangle, TextureFilter filter)
		{
			IDirect3DSurface9* ptr;
			if (sourceSurface != null)
			{
				ptr = sourceSurface.m_lpUM;
			}
			else
			{
				ptr = null;
			}
			IDirect3DSurface9* ptr2;
			if (destSurface != null)
			{
				ptr2 = destSurface.m_lpUM;
			}
			else
			{
				ptr2 = null;
			}
			sourceRectangle.Width += sourceRectangle.X;
			sourceRectangle.Height += sourceRectangle.Y;
			destRectangle.Width += destRectangle.X;
			destRectangle.Height += destRectangle.Y;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9*,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9*,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*,System.Int32), this.m_lpUM, ptr, ref sourceRectangle, ptr2, ref destRectangle, filter, *(*(int*)this.m_lpUM + 136));
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

		// Token: 0x0600065D RID: 1629 RVA: 0x00062EE8 File Offset: 0x000622E8
		public void ColorFill(Surface surface, Rectangle rect, Color color)
		{
			this.ColorFill(surface, rect, color.ToArgb());
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x000686F0 File Offset: 0x00067AF0
		public unsafe void ColorFill(Surface surface, Rectangle rect, int color)
		{
			IDirect3DSurface9* ptr;
			if (surface != null)
			{
				ptr = surface.m_lpUM;
			}
			else
			{
				ptr = null;
			}
			rect.Width += rect.X;
			rect.Height += rect.Y;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSurface9*,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, ptr, ref rect, color, *(*(int*)this.m_lpUM + 140));
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

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x00068E58 File Offset: 0x00068258
		// (set) Token: 0x06000660 RID: 1632 RVA: 0x00068F00 File Offset: 0x00068300
		public unsafe Rectangle ScissorRectangle
		{
			get
			{
				Rectangle rectangle = default(Rectangle);
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.tagRECT*), this.m_lpUM, ref rectangle, *(*(int*)this.m_lpUM + 304));
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
				Rectangle result = default(Rectangle);
				result = new Rectangle(rectangle.Left, rectangle.Top, rectangle.Left + rectangle.Width, rectangle.Top + rectangle.Height);
				return result;
			}
			set
			{
				value.Width += value.X;
				value.Height += value.Y;
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.tagRECT modopt(Microsoft.VisualC.IsConstModifier)*), this.m_lpUM, ref value, *(*(int*)this.m_lpUM + 300));
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
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x000676B0 File Offset: 0x00066AB0
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x0006771C File Offset: 0x00066B1C
		public unsafe VertexDeclaration VertexDeclaration
		{
			get
			{
				IDirect3DVertexDeclaration9* ptr = null;
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVertexDeclaration9**), this.m_lpUM, ref ptr, *(*(int*)this.m_lpUM + 352));
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
				if (ptr != null)
				{
					return new VertexDeclaration(ptr, this);
				}
				return null;
			}
			set
			{
				IDirect3DVertexDeclaration9* ptr;
				if (value != null)
				{
					ptr = value.m_lpUM;
				}
				else
				{
					ptr = null;
				}
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DVertexDeclaration9*), this.m_lpUM, ptr, *(*(int*)this.m_lpUM + 348));
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
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x00067784 File Offset: 0x00066B84
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x000677E4 File Offset: 0x00066BE4
		public unsafe VertexFormats VertexFormat
		{
			get
			{
				uint result = 0U;
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)*), this.m_lpUM, ref result, *(*(int*)this.m_lpUM + 360));
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
				return (VertexFormats)result;
			}
			set
			{
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32 modopt(Microsoft.VisualC.IsLongModifier)), this.m_lpUM, value, *(*(int*)this.m_lpUM + 356));
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
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00068F88 File Offset: 0x00068388
		public unsafe void SetDialogBoxesEnabled([MarshalAs(UnmanagedType.U1)] bool value)
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32), this.m_lpUM, value, *(*(int*)this.m_lpUM + 80));
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

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x00068FE0 File Offset: 0x000683E0
		public unsafe int NumberOfSwapChains
		{
			get
			{
				return calli(System.UInt32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 60));
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00069008 File Offset: 0x00068408
		public unsafe SwapChain GetSwapChain(int swapChain)
		{
			IDirect3DSwapChain9* ptr = null;
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.UInt32,Microsoft.DirectX.PrivateImplementationDetails.IDirect3DSwapChain9**), this.m_lpUM, swapChain, ref ptr, *(*(int*)this.m_lpUM + 56));
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
			if (ptr != null)
			{
				return new SwapChain(ptr, this);
			}
			return null;
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x00069070 File Offset: 0x00068470
		// (set) Token: 0x06000669 RID: 1641 RVA: 0x000690A4 File Offset: 0x000684A4
		public unsafe bool SoftwareVertexProcessing
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 312)) != 0;
			}
			[param: MarshalAs(UnmanagedType.U1)]
			set
			{
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Int32), this.m_lpUM, value, *(*(int*)this.m_lpUM + 308));
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
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00069100 File Offset: 0x00068500
		public unsafe void EvictManagedResources()
		{
			int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 20));
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

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00069158 File Offset: 0x00068558
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x00069184 File Offset: 0x00068584
		public unsafe float NPatchMode
		{
			get
			{
				return calli(System.Single modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 320));
			}
			set
			{
				int num = calli(System.Int32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32,System.Single), this.m_lpUM, value, *(*(int*)this.m_lpUM + 316));
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
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x00065DBC File Offset: 0x000651BC
		public RenderStateManager RenderState
		{
			get
			{
				return this.pRenderStateManager;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x00065DD4 File Offset: 0x000651D4
		public Transforms Transform
		{
			get
			{
				return this.pTransformsManager;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x00065DEC File Offset: 0x000651EC
		public TextureStateManagerCollection TextureState
		{
			get
			{
				return this.pTextureStateManager;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x00065E04 File Offset: 0x00065204
		public SamplerStateManagerCollection SamplerState
		{
			get
			{
				return this.pSamplerStateManager;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x00065E1C File Offset: 0x0006521C
		public LightsCollection Lights
		{
			get
			{
				return this.pLightsCollection;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x00065E34 File Offset: 0x00065234
		public ClipPlanes ClipPlanes
		{
			get
			{
				return this.pClipPlaneManager;
			}
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00069670 File Offset: 0x00068A70
		protected override void Finalize()
		{
			this.raise_DeviceLost(this, EventArgs.Empty);
			if (this != null && !this.m_bDisposed)
			{
				this.raise_Disposing(this, EventArgs.Empty);
				this.m_bDisposed = true;
				this.Dispose();
			}
			base.Finalize();
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x000691E0 File Offset: 0x000685E0
		private void OnParentResized(object sender, EventArgs e)
		{
			if (this.canDeviceBeReset)
			{
				Form form = sender as Form;
				if (form == null || form.WindowState != FormWindowState.Minimized)
				{
					CancelEventArgs cancelEventArgs = new CancelEventArgs();
					this.raise_DeviceResizing(this, cancelEventArgs);
					if (!cancelEventArgs.Cancel)
					{
						Control control = sender as Control;
						if (control != null)
						{
							Size size = control.Size;
							if (this.parentSize != size)
							{
								Rectangle clientRectangle = control.ClientRectangle;
								this.localPresent.BackBufferWidth = clientRectangle.Width;
								Rectangle clientRectangle2 = control.ClientRectangle;
								this.localPresent.BackBufferHeight = clientRectangle2.Height;
								PresentParameters[] array = new PresentParameters[<Module>.$ConstGCArrayBound$0xfa05a5d2$129$];
								array[0] = this.localPresent;
								this.Reset(array);
							}
							Size size2 = control.Size;
							this.parentSize = size2;
						}
					}
				}
			}
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00065D1C File Offset: 0x0006511C
		internal void CreateHelperClasses()
		{
			this.m_bDisposed = false;
			this.pRenderStateManager = new RenderStateManager(this);
			this.pTextureStateManager = new TextureStateManagerCollection(this);
			this.pSamplerStateManager = new SamplerStateManagerCollection(this);
			this.pLightsCollection = new LightsCollection(this);
			this.pTransformsManager = new Transforms(this);
			this.pClipPlaneManager = new ClipPlanes(this);
			this.canDeviceBeReset = true;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x000694BC File Offset: 0x000688BC
		internal void OnParentDisposed(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x00062F0C File Offset: 0x0006230C
		public bool Disposed
		{
			[return: MarshalAs(UnmanagedType.U1)]
			get
			{
				return this.m_bDisposed;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x00062F24 File Offset: 0x00062324
		public PresentParameters PresentationParameters
		{
			get
			{
				return this.localPresent;
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00062FA0 File Offset: 0x000623A0
		[CLSCompliant(false)]
		public unsafe Device(IDirect3DDevice9* pUnk)
		{
			this.DeviceLost = null;
			this.DeviceReset = null;
			this.DeviceResizing = null;
			this.Disposing = null;
			this.m_lpUM = pUnk;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00062F3C File Offset: 0x0006233C
		public unsafe Device(IntPtr unmanagedObject)
		{
			this.DeviceLost = null;
			this.DeviceReset = null;
			this.DeviceResizing = null;
			this.Disposing = null;
			IDirect3DDevice9* lpUM = (IDirect3DDevice9*)unmanagedObject.ToPointer();
			this.m_lpUM = lpUM;
			this.CreateHelperClasses();
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x000695E0 File Offset: 0x000689E0
		public unsafe Device(int adapter, DeviceType deviceType, IntPtr renderWindowHandle, CreateFlags behaviorFlags, params PresentParameters[] presentationParameters)
		{
			this.DeviceLost = null;
			this.DeviceReset = null;
			this.DeviceResizing = null;
			this.Disposing = null;
			ref IDirect3DDevice9* ppRet = ref this.m_lpUM;
			this.parent = null;
			this.localPresent = presentationParameters[0];
			int num = <Module>.?CreateDirect3DDevice@@$$FYGJP$AAVDevice@Direct3D@DirectX@Microsoft@@KW4_D3DDEVTYPE@PrivateImplementationDetails@34@PAUHWND__@634@KP$01AP$AAVPresentParameters@234@PAPAUIDirect3DDevice9@634@@Z(this, adapter, (int)deviceType, (HWND__*)renderWindowHandle.ToPointer(), behaviorFlags, presentationParameters, ref ppRet);
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

		// Token: 0x0600067C RID: 1660 RVA: 0x000694D4 File Offset: 0x000688D4
		public unsafe Device(int adapter, DeviceType deviceType, Control renderWindow, CreateFlags behaviorFlags, params PresentParameters[] presentationParameters)
		{
			this.DeviceLost = null;
			this.DeviceReset = null;
			this.DeviceResizing = null;
			this.Disposing = null;
			ref IDirect3DDevice9* ppRet = ref this.m_lpUM;
			this.localPresent = presentationParameters[0];
			if (Device.IsUsingEventHandlers)
			{
				this.parent = renderWindow;
				if (this.parent != null)
				{
					Size size = this.parent.Size;
					this.parentSize = size;
					this.parent.Disposed += this.OnParentDisposed;
					this.parent.Resize += this.OnParentResized;
					this.parent.SizeChanged += this.OnParentResized;
				}
			}
			int windowHandle;
			if (renderWindow != null)
			{
				windowHandle = renderWindow.Handle.ToPointer();
			}
			else
			{
				windowHandle = 0;
			}
			int num = <Module>.?CreateDirect3DDevice@@$$FYGJP$AAVDevice@Direct3D@DirectX@Microsoft@@KW4_D3DDEVTYPE@PrivateImplementationDetails@34@PAUHWND__@634@KP$01AP$AAVPresentParameters@234@PAPAUIDirect3DDevice9@634@@Z(this, adapter, (int)deviceType, windowHandle, behaviorFlags, presentationParameters, ref ppRet);
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

		// Token: 0x0600067D RID: 1661 RVA: 0x00065D84 File Offset: 0x00065184
		[EditorBrowsable(EditorBrowsableState.Never)]
		public unsafe IntPtr GetObjectByValue(int uniqueKey)
		{
			if (uniqueKey == -759872593)
			{
				IntPtr result = 0;
				result = new IntPtr((void*)this.m_lpUM);
				return result;
			}
			throw new ArgumentException();
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x00062F88 File Offset: 0x00062388
		[CLSCompliant(false)]
		public unsafe IDirect3DDevice9* UnmanagedComPointer
		{
			get
			{
				return this.m_lpUM;
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00062FDC File Offset: 0x000623DC
		[CLSCompliant(false)]
		public unsafe void UpdateUnmanagedPointer(IDirect3DDevice9* pInterface)
		{
			if (this.m_lpUM != null && !calli(System.UInt32 modopt(Microsoft.VisualC.IsLongModifier) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.Int32), this.m_lpUM, *(*(int*)this.m_lpUM + 8)))
			{
				this.m_lpUM = null;
			}
			this.m_lpUM = pInterface;
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000680 RID: 1664 RVA: 0x0006301C File Offset: 0x0006241C
		// (remove) Token: 0x06000681 RID: 1665 RVA: 0x00063040 File Offset: 0x00062440
		public event EventHandler DeviceLost
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.DeviceLost = Delegate.Combine(this.DeviceLost, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.DeviceLost = Delegate.Remove(this.DeviceLost, value);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000683 RID: 1667 RVA: 0x0006308C File Offset: 0x0006248C
		// (remove) Token: 0x06000684 RID: 1668 RVA: 0x000630B0 File Offset: 0x000624B0
		public event EventHandler DeviceReset
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.DeviceReset = Delegate.Combine(this.DeviceReset, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.DeviceReset = Delegate.Remove(this.DeviceReset, value);
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000686 RID: 1670 RVA: 0x000630FC File Offset: 0x000624FC
		// (remove) Token: 0x06000687 RID: 1671 RVA: 0x00063120 File Offset: 0x00062520
		public event CancelEventHandler DeviceResizing
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.DeviceResizing = Delegate.Combine(this.DeviceResizing, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.DeviceResizing = Delegate.Remove(this.DeviceResizing, value);
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000689 RID: 1673 RVA: 0x0006316C File Offset: 0x0006256C
		// (remove) Token: 0x0600068A RID: 1674 RVA: 0x00063190 File Offset: 0x00062590
		public event EventHandler Disposing
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.Disposing = Delegate.Combine(this.Disposing, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.Disposing = Delegate.Remove(this.Disposing, value);
			}
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x000631DC File Offset: 0x000625DC
		private void __dtor()
		{
			GC.SuppressFinalize(this);
			this.Finalize();
		}

		// Token: 0x04001042 RID: 4162
		private static bool m_useEventHandlers = true;

		// Token: 0x04001043 RID: 4163
		internal VertexShader pCachedResourcepVertexShader;

		// Token: 0x04001044 RID: 4164
		internal PixelShader pCachedResourcepPixelShader;

		// Token: 0x04001045 RID: 4165
		private RenderStateManager pRenderStateManager;

		// Token: 0x04001046 RID: 4166
		private TextureStateManagerCollection pTextureStateManager;

		// Token: 0x04001047 RID: 4167
		private SamplerStateManagerCollection pSamplerStateManager;

		// Token: 0x04001048 RID: 4168
		private LightsCollection pLightsCollection;

		// Token: 0x04001049 RID: 4169
		private Transforms pTransformsManager;

		// Token: 0x0400104A RID: 4170
		private ClipPlanes pClipPlaneManager;

		// Token: 0x0400104B RID: 4171
		private Control parent;

		// Token: 0x0400104C RID: 4172
		private PresentParameters localPresent;

		// Token: 0x0400104D RID: 4173
		private Size parentSize;

		// Token: 0x0400104E RID: 4174
		private bool canDeviceBeReset;

		// Token: 0x04001052 RID: 4178
		internal bool m_bDisposed;

		// Token: 0x04001054 RID: 4180
		internal unsafe IDirect3DDevice9* m_lpUM;
	}
}

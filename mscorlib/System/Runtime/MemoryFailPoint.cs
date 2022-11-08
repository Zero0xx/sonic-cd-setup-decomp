using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;

namespace System.Runtime
{
	// Token: 0x0200060E RID: 1550
	public sealed class MemoryFailPoint : CriticalFinalizerObject, IDisposable
	{
		// Token: 0x06003810 RID: 14352 RVA: 0x000BBE19 File Offset: 0x000BAE19
		static MemoryFailPoint()
		{
			MemoryFailPoint.GetMemorySettings(out MemoryFailPoint.GCSegmentSize, out MemoryFailPoint.TopOfMemory);
		}

		// Token: 0x06003811 RID: 14353 RVA: 0x000BBE2C File Offset: 0x000BAE2C
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		public unsafe MemoryFailPoint(int sizeInMegabytes)
		{
			if (sizeInMegabytes <= 0)
			{
				throw new ArgumentOutOfRangeException("sizeInMegabytes", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			ulong num = (ulong)((ulong)((long)sizeInMegabytes) << 20);
			this._reservedMemory = num;
			ulong num2 = (ulong)(Math.Ceiling((double)(num / MemoryFailPoint.GCSegmentSize)) * MemoryFailPoint.GCSegmentSize);
			if (num2 >= MemoryFailPoint.TopOfMemory)
			{
				throw new InsufficientMemoryException(Environment.GetResourceString("InsufficientMemory_MemFailPoint_TooBig"));
			}
			ulong num3 = 0UL;
			ulong num4 = 0UL;
			int i = 0;
			while (i < 3)
			{
				MemoryFailPoint.CheckForAvailableMemory(out num3, out num4);
				ulong memoryFailPointReservedMemory = SharedStatics.MemoryFailPointReservedMemory;
				ulong num5 = num2 + memoryFailPointReservedMemory;
				bool flag = num5 < num2 || num5 < memoryFailPointReservedMemory;
				bool flag2 = num3 < num5 + 16777216UL || flag;
				bool flag3 = num4 < num5 || flag;
				long num6 = (long)Environment.TickCount;
				if (num6 > MemoryFailPoint.LastTimeCheckingAddressSpace + 10000L || num6 < MemoryFailPoint.LastTimeCheckingAddressSpace || MemoryFailPoint.LastKnownFreeAddressSpace < (long)num2)
				{
					MemoryFailPoint.CheckForFreeAddressSpace(num2, false);
				}
				bool flag4 = MemoryFailPoint.LastKnownFreeAddressSpace < (long)num2;
				if (!flag2 && !flag3 && !flag4)
				{
					break;
				}
				switch (i)
				{
				case 0:
					GC.Collect();
					break;
				case 1:
					if (flag2)
					{
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
							break;
						}
						finally
						{
							UIntPtr numBytes = new UIntPtr(num2);
							void* ptr = Win32Native.VirtualAlloc(null, numBytes, 4096, 4);
							if (ptr != null && !Win32Native.VirtualFree(ptr, UIntPtr.Zero, 32768))
							{
								__Error.WinIOError();
							}
						}
						goto IL_166;
					}
					break;
				case 2:
					goto IL_166;
				}
				IL_19A:
				i++;
				continue;
				IL_166:
				if (flag2 || flag3)
				{
					InsufficientMemoryException ex = new InsufficientMemoryException(Environment.GetResourceString("InsufficientMemory_MemFailPoint"));
					throw ex;
				}
				if (flag4)
				{
					InsufficientMemoryException ex2 = new InsufficientMemoryException(Environment.GetResourceString("InsufficientMemory_MemFailPoint_VAFrag"));
					throw ex2;
				}
				goto IL_19A;
			}
			Interlocked.Add(ref MemoryFailPoint.LastKnownFreeAddressSpace, (long)(-(long)num));
			if (MemoryFailPoint.LastKnownFreeAddressSpace < 0L)
			{
				MemoryFailPoint.CheckForFreeAddressSpace(num2, true);
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				SharedStatics.AddMemoryFailPointReservation((long)num);
				this._mustSubtractReservation = true;
			}
		}

		// Token: 0x06003812 RID: 14354 RVA: 0x000BC034 File Offset: 0x000BB034
		private static void CheckForAvailableMemory(out ulong availPageFile, out ulong totalAddressSpaceFree)
		{
			if (Environment.IsWin9X())
			{
				Win32Native.MEMORYSTATUS memorystatus = new Win32Native.MEMORYSTATUS();
				if (!Win32Native.GlobalMemoryStatus(memorystatus))
				{
					__Error.WinIOError();
				}
				availPageFile = (ulong)memorystatus.availPageFile;
				totalAddressSpaceFree = (ulong)memorystatus.availVirtual;
				return;
			}
			Win32Native.MEMORYSTATUSEX memorystatusex = new Win32Native.MEMORYSTATUSEX();
			if (!Win32Native.GlobalMemoryStatusEx(memorystatusex))
			{
				__Error.WinIOError();
			}
			availPageFile = memorystatusex.availPageFile;
			totalAddressSpaceFree = memorystatusex.availVirtual;
		}

		// Token: 0x06003813 RID: 14355 RVA: 0x000BC098 File Offset: 0x000BB098
		private static bool CheckForFreeAddressSpace(ulong size, bool shouldThrow)
		{
			ulong num = MemoryFailPoint.MemFreeAfterAddress(null, size);
			MemoryFailPoint.LastKnownFreeAddressSpace = (long)num;
			MemoryFailPoint.LastTimeCheckingAddressSpace = (long)Environment.TickCount;
			if (num < size && shouldThrow)
			{
				throw new InsufficientMemoryException(Environment.GetResourceString("InsufficientMemory_MemFailPoint_VAFrag"));
			}
			return num >= size;
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x000BC0E0 File Offset: 0x000BB0E0
		private unsafe static ulong MemFreeAfterAddress(void* address, ulong size)
		{
			if (size >= MemoryFailPoint.TopOfMemory)
			{
				return 0UL;
			}
			ulong num = 0UL;
			Win32Native.MEMORY_BASIC_INFORMATION memory_BASIC_INFORMATION = default(Win32Native.MEMORY_BASIC_INFORMATION);
			IntPtr sizeOfBuffer = (IntPtr)Marshal.SizeOf(memory_BASIC_INFORMATION);
			while ((byte*)address + size < MemoryFailPoint.TopOfMemory)
			{
				IntPtr value = Win32Native.VirtualQuery(address, ref memory_BASIC_INFORMATION, sizeOfBuffer);
				if (value == IntPtr.Zero)
				{
					__Error.WinIOError();
				}
				ulong num2 = memory_BASIC_INFORMATION.RegionSize.ToUInt64();
				if (memory_BASIC_INFORMATION.State == 65536U)
				{
					if (num2 >= size)
					{
						return num2;
					}
					num = Math.Max(num, num2);
				}
				address = (void*)((byte*)address + num2);
			}
			return num;
		}

		// Token: 0x06003815 RID: 14357
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetMemorySettings(out uint maxGCSegmentSize, out ulong topOfMemory);

		// Token: 0x06003816 RID: 14358 RVA: 0x000BC174 File Offset: 0x000BB174
		~MemoryFailPoint()
		{
			this.Dispose(false);
		}

		// Token: 0x06003817 RID: 14359 RVA: 0x000BC1A4 File Offset: 0x000BB1A4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003818 RID: 14360 RVA: 0x000BC1B4 File Offset: 0x000BB1B4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private void Dispose(bool disposing)
		{
			if (this._mustSubtractReservation)
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					SharedStatics.AddMemoryFailPointReservation((long)(-(long)this._reservedMemory));
					this._mustSubtractReservation = false;
				}
			}
		}

		// Token: 0x04001D0A RID: 7434
		private const int CheckThreshold = 10000;

		// Token: 0x04001D0B RID: 7435
		private const int LowMemoryFudgeFactor = 16777216;

		// Token: 0x04001D0C RID: 7436
		private static readonly ulong TopOfMemory;

		// Token: 0x04001D0D RID: 7437
		private static long LastKnownFreeAddressSpace;

		// Token: 0x04001D0E RID: 7438
		private static long LastTimeCheckingAddressSpace;

		// Token: 0x04001D0F RID: 7439
		private static readonly uint GCSegmentSize;

		// Token: 0x04001D10 RID: 7440
		private ulong _reservedMemory;

		// Token: 0x04001D11 RID: 7441
		private bool _mustSubtractReservation;
	}
}

using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x0200077E RID: 1918
	internal static class WinProcessManager
	{
		// Token: 0x06003B3B RID: 15163 RVA: 0x000FC09C File Offset: 0x000FB09C
		public static int[] GetProcessIds()
		{
			ProcessInfo[] processInfos = WinProcessManager.GetProcessInfos();
			int[] array = new int[processInfos.Length];
			for (int i = 0; i < processInfos.Length; i++)
			{
				array[i] = processInfos[i].processId;
			}
			return array;
		}

		// Token: 0x06003B3C RID: 15164 RVA: 0x000FC0D4 File Offset: 0x000FB0D4
		public static ProcessInfo[] GetProcessInfos()
		{
			IntPtr intPtr = (IntPtr)(-1);
			GCHandle gchandle = default(GCHandle);
			ArrayList arrayList = new ArrayList();
			Hashtable hashtable = new Hashtable();
			try
			{
				intPtr = NativeMethods.CreateToolhelp32Snapshot(6, 0);
				if (intPtr == (IntPtr)(-1))
				{
					throw new Win32Exception();
				}
				int num = Marshal.SizeOf(typeof(NativeMethods.WinProcessEntry));
				int num2 = num + 260;
				int[] value = new int[num2 / 4];
				gchandle = GCHandle.Alloc(value, GCHandleType.Pinned);
				IntPtr intPtr2 = gchandle.AddrOfPinnedObject();
				Marshal.WriteInt32(intPtr2, num2);
				HandleRef handle = new HandleRef(null, intPtr);
				if (NativeMethods.Process32First(handle, intPtr2))
				{
					do
					{
						NativeMethods.WinProcessEntry winProcessEntry = new NativeMethods.WinProcessEntry();
						Marshal.PtrToStructure(intPtr2, winProcessEntry);
						ProcessInfo processInfo = new ProcessInfo();
						string path = Marshal.PtrToStringAnsi((IntPtr)((long)intPtr2 + (long)num));
						processInfo.processName = Path.ChangeExtension(Path.GetFileName(path), null);
						processInfo.handleCount = winProcessEntry.cntUsage;
						processInfo.processId = winProcessEntry.th32ProcessID;
						processInfo.basePriority = winProcessEntry.pcPriClassBase;
						processInfo.mainModuleId = winProcessEntry.th32ModuleID;
						hashtable.Add(processInfo.processId, processInfo);
						Marshal.WriteInt32(intPtr2, num2);
					}
					while (NativeMethods.Process32Next(handle, intPtr2));
				}
				NativeMethods.WinThreadEntry winThreadEntry = new NativeMethods.WinThreadEntry();
				winThreadEntry.dwSize = Marshal.SizeOf(winThreadEntry);
				if (NativeMethods.Thread32First(handle, winThreadEntry))
				{
					do
					{
						arrayList.Add(new ThreadInfo
						{
							threadId = winThreadEntry.th32ThreadID,
							processId = winThreadEntry.th32OwnerProcessID,
							basePriority = winThreadEntry.tpBasePri,
							currentPriority = winThreadEntry.tpBasePri + winThreadEntry.tpDeltaPri
						});
					}
					while (NativeMethods.Thread32Next(handle, winThreadEntry));
				}
				for (int i = 0; i < arrayList.Count; i++)
				{
					ThreadInfo threadInfo = (ThreadInfo)arrayList[i];
					ProcessInfo processInfo2 = (ProcessInfo)hashtable[threadInfo.processId];
					if (processInfo2 != null)
					{
						processInfo2.threadInfoList.Add(threadInfo);
					}
				}
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
				if (intPtr != (IntPtr)(-1))
				{
					SafeNativeMethods.CloseHandle(new HandleRef(null, intPtr));
				}
			}
			ProcessInfo[] array = new ProcessInfo[hashtable.Values.Count];
			hashtable.Values.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06003B3D RID: 15165 RVA: 0x000FC34C File Offset: 0x000FB34C
		public static ModuleInfo[] GetModuleInfos(int processId)
		{
			IntPtr intPtr = (IntPtr)(-1);
			GCHandle gchandle = default(GCHandle);
			ArrayList arrayList = new ArrayList();
			try
			{
				intPtr = NativeMethods.CreateToolhelp32Snapshot(8, processId);
				if (intPtr == (IntPtr)(-1))
				{
					throw new Win32Exception();
				}
				int num = Marshal.SizeOf(typeof(NativeMethods.WinModuleEntry));
				int num2 = num + 260 + 256;
				int[] value = new int[num2 / 4];
				gchandle = GCHandle.Alloc(value, GCHandleType.Pinned);
				IntPtr intPtr2 = gchandle.AddrOfPinnedObject();
				Marshal.WriteInt32(intPtr2, num2);
				HandleRef handle = new HandleRef(null, intPtr);
				if (NativeMethods.Module32First(handle, intPtr2))
				{
					do
					{
						NativeMethods.WinModuleEntry winModuleEntry = new NativeMethods.WinModuleEntry();
						Marshal.PtrToStructure(intPtr2, winModuleEntry);
						arrayList.Add(new ModuleInfo
						{
							baseName = Marshal.PtrToStringAnsi((IntPtr)((long)intPtr2 + (long)num)),
							fileName = Marshal.PtrToStringAnsi((IntPtr)((long)intPtr2 + (long)num + 256L)),
							baseOfDll = winModuleEntry.modBaseAddr,
							sizeOfImage = winModuleEntry.modBaseSize,
							Id = winModuleEntry.th32ModuleID
						});
						Marshal.WriteInt32(intPtr2, num2);
					}
					while (NativeMethods.Module32Next(handle, intPtr2));
				}
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
				if (intPtr != (IntPtr)(-1))
				{
					SafeNativeMethods.CloseHandle(new HandleRef(null, intPtr));
				}
			}
			ModuleInfo[] array = new ModuleInfo[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}
	}
}

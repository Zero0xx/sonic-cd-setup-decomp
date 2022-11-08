using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000781 RID: 1921
	internal static class NtProcessInfoHelper
	{
		// Token: 0x06003B4E RID: 15182 RVA: 0x000FD1D0 File Offset: 0x000FC1D0
		private static int GetNewBufferSize(int existingBufferSize, int requiredSize)
		{
			if (requiredSize != 0)
			{
				return requiredSize + 10240;
			}
			int num = existingBufferSize * 2;
			if (num < existingBufferSize)
			{
				throw new OutOfMemoryException();
			}
			return num;
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x000FD1F8 File Offset: 0x000FC1F8
		public static ProcessInfo[] GetProcessInfos()
		{
			int num = 131072;
			int requiredSize = 0;
			GCHandle gchandle = default(GCHandle);
			ProcessInfo[] processInfos;
			try
			{
				int num2;
				do
				{
					byte[] value = new byte[num];
					gchandle = GCHandle.Alloc(value, GCHandleType.Pinned);
					num2 = NativeMethods.NtQuerySystemInformation(5, gchandle.AddrOfPinnedObject(), num, out requiredSize);
					if (num2 == -1073741820)
					{
						if (gchandle.IsAllocated)
						{
							gchandle.Free();
						}
						num = NtProcessInfoHelper.GetNewBufferSize(num, requiredSize);
					}
				}
				while (num2 == -1073741820);
				if (num2 < 0)
				{
					throw new InvalidOperationException(SR.GetString("CouldntGetProcessInfos"), new Win32Exception(num2));
				}
				processInfos = NtProcessInfoHelper.GetProcessInfos(gchandle.AddrOfPinnedObject());
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
			}
			return processInfos;
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x000FD2AC File Offset: 0x000FC2AC
		private unsafe static ProcessInfo[] GetProcessInfos(IntPtr dataPtr)
		{
			Hashtable hashtable = new Hashtable(60);
			long num = 0L;
			for (;;)
			{
				IntPtr intPtr = (IntPtr)((long)dataPtr + num);
				NtProcessInfoHelper.SystemProcessInformation systemProcessInformation = new NtProcessInfoHelper.SystemProcessInformation();
				Marshal.PtrToStructure(intPtr, systemProcessInformation);
				ProcessInfo processInfo = new ProcessInfo();
				processInfo.processId = systemProcessInformation.UniqueProcessId.ToInt32();
				processInfo.handleCount = (int)systemProcessInformation.HandleCount;
				processInfo.sessionId = (int)systemProcessInformation.SessionId;
				processInfo.poolPagedBytes = (long)systemProcessInformation.QuotaPagedPoolUsage;
				processInfo.poolNonpagedBytes = (long)systemProcessInformation.QuotaNonPagedPoolUsage;
				processInfo.virtualBytes = (long)systemProcessInformation.VirtualSize;
				processInfo.virtualBytesPeak = (long)systemProcessInformation.PeakVirtualSize;
				processInfo.workingSetPeak = (long)systemProcessInformation.PeakWorkingSetSize;
				processInfo.workingSet = (long)systemProcessInformation.WorkingSetSize;
				processInfo.pageFileBytesPeak = (long)systemProcessInformation.PeakPagefileUsage;
				processInfo.pageFileBytes = (long)systemProcessInformation.PagefileUsage;
				processInfo.privateBytes = (long)systemProcessInformation.PrivatePageCount;
				processInfo.basePriority = systemProcessInformation.BasePriority;
				if (systemProcessInformation.NamePtr == IntPtr.Zero)
				{
					if (processInfo.processId == NtProcessManager.SystemProcessID)
					{
						processInfo.processName = "System";
					}
					else if (processInfo.processId == 0)
					{
						processInfo.processName = "Idle";
					}
					else
					{
						processInfo.processName = processInfo.processId.ToString(CultureInfo.InvariantCulture);
					}
				}
				else
				{
					string text = NtProcessInfoHelper.GetProcessShortName((char*)systemProcessInformation.NamePtr.ToPointer(), (int)(systemProcessInformation.NameLength / 2));
					if (ProcessManager.IsOSOlderThanXP && text.Length == 15)
					{
						if (text.EndsWith(".", StringComparison.OrdinalIgnoreCase))
						{
							text = text.Substring(0, 14);
						}
						else if (text.EndsWith(".e", StringComparison.OrdinalIgnoreCase))
						{
							text = text.Substring(0, 13);
						}
						else if (text.EndsWith(".ex", StringComparison.OrdinalIgnoreCase))
						{
							text = text.Substring(0, 12);
						}
					}
					processInfo.processName = text;
				}
				hashtable[processInfo.processId] = processInfo;
				intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(systemProcessInformation));
				int num2 = 0;
				while ((long)num2 < (long)((ulong)systemProcessInformation.NumberOfThreads))
				{
					NtProcessInfoHelper.SystemThreadInformation systemThreadInformation = new NtProcessInfoHelper.SystemThreadInformation();
					Marshal.PtrToStructure(intPtr, systemThreadInformation);
					ThreadInfo threadInfo = new ThreadInfo();
					threadInfo.processId = (int)systemThreadInformation.UniqueProcess;
					threadInfo.threadId = (int)systemThreadInformation.UniqueThread;
					threadInfo.basePriority = systemThreadInformation.BasePriority;
					threadInfo.currentPriority = systemThreadInformation.Priority;
					threadInfo.startAddress = systemThreadInformation.StartAddress;
					threadInfo.threadState = (ThreadState)systemThreadInformation.ThreadState;
					threadInfo.threadWaitReason = NtProcessManager.GetThreadWaitReason((int)systemThreadInformation.WaitReason);
					processInfo.threadInfoList.Add(threadInfo);
					intPtr = (IntPtr)((long)intPtr + (long)Marshal.SizeOf(systemThreadInformation));
					num2++;
				}
				if (systemProcessInformation.NextEntryOffset == 0)
				{
					break;
				}
				num += (long)systemProcessInformation.NextEntryOffset;
			}
			ProcessInfo[] array = new ProcessInfo[hashtable.Values.Count];
			hashtable.Values.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x000FD5DC File Offset: 0x000FC5DC
		internal unsafe static string GetProcessShortName(char* name, int length)
		{
			char* ptr = name;
			char* ptr2 = name;
			char* ptr3 = name;
			int num = 0;
			while (*ptr3 != '\0')
			{
				if (*ptr3 == '\\')
				{
					ptr = ptr3;
				}
				else if (*ptr3 == '.')
				{
					ptr2 = ptr3;
				}
				ptr3++;
				num++;
				if (num >= length)
				{
					break;
				}
			}
			if (ptr2 == name)
			{
				ptr2 = ptr3;
			}
			else
			{
				string b = new string(ptr2);
				if (!string.Equals(".exe", b, StringComparison.OrdinalIgnoreCase))
				{
					ptr2 = ptr3;
				}
			}
			if (*ptr == '\\')
			{
				ptr++;
			}
			int length2 = (int)((long)(ptr2 - ptr));
			return new string(ptr, 0, length2);
		}

		// Token: 0x02000782 RID: 1922
		[StructLayout(LayoutKind.Sequential)]
		internal class SystemProcessInformation
		{
			// Token: 0x040033EB RID: 13291
			internal int NextEntryOffset;

			// Token: 0x040033EC RID: 13292
			internal uint NumberOfThreads;

			// Token: 0x040033ED RID: 13293
			private long SpareLi1;

			// Token: 0x040033EE RID: 13294
			private long SpareLi2;

			// Token: 0x040033EF RID: 13295
			private long SpareLi3;

			// Token: 0x040033F0 RID: 13296
			private long CreateTime;

			// Token: 0x040033F1 RID: 13297
			private long UserTime;

			// Token: 0x040033F2 RID: 13298
			private long KernelTime;

			// Token: 0x040033F3 RID: 13299
			internal ushort NameLength;

			// Token: 0x040033F4 RID: 13300
			internal ushort MaximumNameLength;

			// Token: 0x040033F5 RID: 13301
			internal IntPtr NamePtr;

			// Token: 0x040033F6 RID: 13302
			internal int BasePriority;

			// Token: 0x040033F7 RID: 13303
			internal IntPtr UniqueProcessId;

			// Token: 0x040033F8 RID: 13304
			internal IntPtr InheritedFromUniqueProcessId;

			// Token: 0x040033F9 RID: 13305
			internal uint HandleCount;

			// Token: 0x040033FA RID: 13306
			internal uint SessionId;

			// Token: 0x040033FB RID: 13307
			internal IntPtr PageDirectoryBase;

			// Token: 0x040033FC RID: 13308
			internal IntPtr PeakVirtualSize;

			// Token: 0x040033FD RID: 13309
			internal IntPtr VirtualSize;

			// Token: 0x040033FE RID: 13310
			internal uint PageFaultCount;

			// Token: 0x040033FF RID: 13311
			internal IntPtr PeakWorkingSetSize;

			// Token: 0x04003400 RID: 13312
			internal IntPtr WorkingSetSize;

			// Token: 0x04003401 RID: 13313
			internal IntPtr QuotaPeakPagedPoolUsage;

			// Token: 0x04003402 RID: 13314
			internal IntPtr QuotaPagedPoolUsage;

			// Token: 0x04003403 RID: 13315
			internal IntPtr QuotaPeakNonPagedPoolUsage;

			// Token: 0x04003404 RID: 13316
			internal IntPtr QuotaNonPagedPoolUsage;

			// Token: 0x04003405 RID: 13317
			internal IntPtr PagefileUsage;

			// Token: 0x04003406 RID: 13318
			internal IntPtr PeakPagefileUsage;

			// Token: 0x04003407 RID: 13319
			internal IntPtr PrivatePageCount;

			// Token: 0x04003408 RID: 13320
			private long ReadOperationCount;

			// Token: 0x04003409 RID: 13321
			private long WriteOperationCount;

			// Token: 0x0400340A RID: 13322
			private long OtherOperationCount;

			// Token: 0x0400340B RID: 13323
			private long ReadTransferCount;

			// Token: 0x0400340C RID: 13324
			private long WriteTransferCount;

			// Token: 0x0400340D RID: 13325
			private long OtherTransferCount;
		}

		// Token: 0x02000783 RID: 1923
		[StructLayout(LayoutKind.Sequential)]
		internal class SystemThreadInformation
		{
			// Token: 0x0400340E RID: 13326
			private long KernelTime;

			// Token: 0x0400340F RID: 13327
			private long UserTime;

			// Token: 0x04003410 RID: 13328
			private long CreateTime;

			// Token: 0x04003411 RID: 13329
			private uint WaitTime;

			// Token: 0x04003412 RID: 13330
			internal IntPtr StartAddress;

			// Token: 0x04003413 RID: 13331
			internal IntPtr UniqueProcess;

			// Token: 0x04003414 RID: 13332
			internal IntPtr UniqueThread;

			// Token: 0x04003415 RID: 13333
			internal int Priority;

			// Token: 0x04003416 RID: 13334
			internal int BasePriority;

			// Token: 0x04003417 RID: 13335
			internal uint ContextSwitches;

			// Token: 0x04003418 RID: 13336
			internal uint ThreadState;

			// Token: 0x04003419 RID: 13337
			internal uint WaitReason;
		}
	}
}

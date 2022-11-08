using System;
using System.Runtime.ConstrainedExecution;

namespace System.StubHelpers
{
	// Token: 0x02000126 RID: 294
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal struct EEFrame
	{
		// Token: 0x06001057 RID: 4183 RVA: 0x0002E264 File Offset: 0x0002D264
		internal unsafe static void Push(void* pThis, void* pThread)
		{
			void** ptr = (void**)((byte*)pThread + 16L);
			((EEFrame*)pThis)->m_Next = *(IntPtr*)ptr;
			*(IntPtr*)ptr = pThis;
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x0002E288 File Offset: 0x0002D288
		internal unsafe static void Pop(void* pThis, void* pThread)
		{
			void** ptr = (void**)((byte*)pThread + 16L);
			*(IntPtr*)ptr = ((EEFrame*)pThis)->m_Next;
		}

		// Token: 0x0400057B RID: 1403
		internal const long OFFSETOF__Thread__m_pFrame = 16L;

		// Token: 0x0400057C RID: 1404
		internal unsafe void* __VFN_table;

		// Token: 0x0400057D RID: 1405
		internal unsafe void* m_Next;
	}
}

using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Util;
using System.Threading;

namespace System
{
	// Token: 0x02000116 RID: 278
	internal sealed class SharedStatics
	{
		// Token: 0x06000FFC RID: 4092 RVA: 0x0002D980 File Offset: 0x0002C980
		private SharedStatics()
		{
			this._Remoting_Identity_IDGuid = null;
			this._Remoting_Identity_IDSeqNum = 64;
			this._maker = null;
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000FFD RID: 4093 RVA: 0x0002D9A0 File Offset: 0x0002C9A0
		public static string Remoting_Identity_IDGuid
		{
			get
			{
				if (SharedStatics._sharedStatics._Remoting_Identity_IDGuid == null)
				{
					bool flag = false;
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						Monitor.ReliableEnter(SharedStatics._sharedStatics, ref flag);
						if (SharedStatics._sharedStatics._Remoting_Identity_IDGuid == null)
						{
							SharedStatics._sharedStatics._Remoting_Identity_IDGuid = Guid.NewGuid().ToString().Replace('-', '_');
						}
					}
					finally
					{
						if (flag)
						{
							Monitor.Exit(SharedStatics._sharedStatics);
						}
					}
				}
				return SharedStatics._sharedStatics._Remoting_Identity_IDGuid;
			}
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0002DA28 File Offset: 0x0002CA28
		public static Tokenizer.StringMaker GetSharedStringMaker()
		{
			Tokenizer.StringMaker stringMaker = null;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(SharedStatics._sharedStatics, ref flag);
				if (SharedStatics._sharedStatics._maker != null)
				{
					stringMaker = SharedStatics._sharedStatics._maker;
					SharedStatics._sharedStatics._maker = null;
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(SharedStatics._sharedStatics);
				}
			}
			if (stringMaker == null)
			{
				stringMaker = new Tokenizer.StringMaker();
			}
			return stringMaker;
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0002DA98 File Offset: 0x0002CA98
		public static void ReleaseSharedStringMaker(ref Tokenizer.StringMaker maker)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.ReliableEnter(SharedStatics._sharedStatics, ref flag);
				SharedStatics._sharedStatics._maker = maker;
				maker = null;
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(SharedStatics._sharedStatics);
				}
			}
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0002DAE8 File Offset: 0x0002CAE8
		internal static int Remoting_Identity_GetNextSeqNum()
		{
			return Interlocked.Increment(ref SharedStatics._sharedStatics._Remoting_Identity_IDSeqNum);
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0002DAF9 File Offset: 0x0002CAF9
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static long AddMemoryFailPointReservation(long size)
		{
			return Interlocked.Add(ref SharedStatics._sharedStatics._memFailPointReservedMemory, size);
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06001002 RID: 4098 RVA: 0x0002DB0B File Offset: 0x0002CB0B
		internal static ulong MemoryFailPointReservedMemory
		{
			get
			{
				return (ulong)SharedStatics._sharedStatics._memFailPointReservedMemory;
			}
		}

		// Token: 0x04000569 RID: 1385
		internal static SharedStatics _sharedStatics;

		// Token: 0x0400056A RID: 1386
		private string _Remoting_Identity_IDGuid;

		// Token: 0x0400056B RID: 1387
		private Tokenizer.StringMaker _maker;

		// Token: 0x0400056C RID: 1388
		private int _Remoting_Identity_IDSeqNum;

		// Token: 0x0400056D RID: 1389
		private long _memFailPointReservedMemory;
	}
}

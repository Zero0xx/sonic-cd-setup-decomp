using System;
using System.Reflection;
using System.Threading;

namespace System
{
	// Token: 0x02000112 RID: 274
	internal abstract class Resolver
	{
		// Token: 0x06000FCA RID: 4042
		internal abstract void GetJitContext(ref int securityControlFlags, ref RuntimeTypeHandle typeOwner);

		// Token: 0x06000FCB RID: 4043
		internal abstract byte[] GetCodeInfo(ref int stackSize, ref int initLocals, ref int EHCount);

		// Token: 0x06000FCC RID: 4044
		internal abstract byte[] GetLocalsSignature();

		// Token: 0x06000FCD RID: 4045
		internal unsafe abstract void GetEHInfo(int EHNumber, void* exception);

		// Token: 0x06000FCE RID: 4046
		internal abstract byte[] GetRawEHInfo();

		// Token: 0x06000FCF RID: 4047
		internal abstract string GetStringLiteral(int token);

		// Token: 0x06000FD0 RID: 4048
		internal unsafe abstract void* ResolveToken(int token);

		// Token: 0x06000FD1 RID: 4049
		internal abstract int ParentToken(int token);

		// Token: 0x06000FD2 RID: 4050
		internal abstract byte[] ResolveSignature(int token, int fromMethod);

		// Token: 0x06000FD3 RID: 4051
		internal abstract int IsValidToken(int token);

		// Token: 0x06000FD4 RID: 4052
		internal abstract MethodInfo GetDynamicMethod();

		// Token: 0x06000FD5 RID: 4053
		internal abstract CompressedStack GetSecurityContext();

		// Token: 0x0400055B RID: 1371
		internal const int COR_ILEXCEPTION_CLAUSE_CACHED_CLASS = 268435456;

		// Token: 0x0400055C RID: 1372
		internal const int COR_ILEXCEPTION_CLAUSE_MUST_CACHE_CLASS = 536870912;

		// Token: 0x0400055D RID: 1373
		internal const int TypeToken = 1;

		// Token: 0x0400055E RID: 1374
		internal const int MethodToken = 2;

		// Token: 0x0400055F RID: 1375
		internal const int FieldToken = 4;

		// Token: 0x02000113 RID: 275
		internal struct CORINFO_EH_CLAUSE
		{
			// Token: 0x04000560 RID: 1376
			internal int Flags;

			// Token: 0x04000561 RID: 1377
			internal int TryOffset;

			// Token: 0x04000562 RID: 1378
			internal int TryLength;

			// Token: 0x04000563 RID: 1379
			internal int HandlerOffset;

			// Token: 0x04000564 RID: 1380
			internal int HandlerLength;

			// Token: 0x04000565 RID: 1381
			internal int ClassTokenOrFilterOffset;
		}
	}
}

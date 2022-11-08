using System;
using System.Reflection;

namespace System
{
	// Token: 0x02000111 RID: 273
	internal struct SignatureStruct
	{
		// Token: 0x06000FC9 RID: 4041 RVA: 0x0002D620 File Offset: 0x0002C620
		public SignatureStruct(RuntimeMethodHandle method, RuntimeTypeHandle[] arguments, RuntimeTypeHandle returnType, CallingConventions callingConvention)
		{
			this.m_pMethod = method;
			this.m_arguments = arguments;
			this.m_returnTypeORfieldType = returnType;
			this.m_managedCallingConvention = callingConvention;
			this.m_sig = null;
			this.m_pCallTarget = null;
			this.m_csig = 0;
			this.m_numVirtualFixedArgs = 0;
			this.m_64bitpad = 0;
			this.m_declaringType = default(RuntimeTypeHandle);
		}

		// Token: 0x04000551 RID: 1361
		internal RuntimeTypeHandle[] m_arguments;

		// Token: 0x04000552 RID: 1362
		internal unsafe void* m_sig;

		// Token: 0x04000553 RID: 1363
		internal unsafe void* m_pCallTarget;

		// Token: 0x04000554 RID: 1364
		internal CallingConventions m_managedCallingConvention;

		// Token: 0x04000555 RID: 1365
		internal int m_csig;

		// Token: 0x04000556 RID: 1366
		internal int m_numVirtualFixedArgs;

		// Token: 0x04000557 RID: 1367
		internal int m_64bitpad;

		// Token: 0x04000558 RID: 1368
		internal RuntimeMethodHandle m_pMethod;

		// Token: 0x04000559 RID: 1369
		internal RuntimeTypeHandle m_declaringType;

		// Token: 0x0400055A RID: 1370
		internal RuntimeTypeHandle m_returnTypeORfieldType;
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000337 RID: 823
	[ComVisible(true)]
	public sealed class MethodBody
	{
		// Token: 0x06001FA4 RID: 8100 RVA: 0x0004F94E File Offset: 0x0004E94E
		private MethodBody()
		{
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x0004F956 File Offset: 0x0004E956
		public int LocalSignatureMetadataToken
		{
			get
			{
				return this.m_localSignatureMetadataToken;
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001FA6 RID: 8102 RVA: 0x0004F95E File Offset: 0x0004E95E
		public IList<LocalVariableInfo> LocalVariables
		{
			get
			{
				return Array.AsReadOnly<LocalVariableInfo>(this.m_localVariables);
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001FA7 RID: 8103 RVA: 0x0004F96B File Offset: 0x0004E96B
		public int MaxStackSize
		{
			get
			{
				return this.m_maxStackSize;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001FA8 RID: 8104 RVA: 0x0004F973 File Offset: 0x0004E973
		public bool InitLocals
		{
			get
			{
				return this.m_initLocals;
			}
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x0004F97B File Offset: 0x0004E97B
		public byte[] GetILAsByteArray()
		{
			return this.m_IL;
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001FAA RID: 8106 RVA: 0x0004F983 File Offset: 0x0004E983
		public IList<ExceptionHandlingClause> ExceptionHandlingClauses
		{
			get
			{
				return Array.AsReadOnly<ExceptionHandlingClause>(this.m_exceptionHandlingClauses);
			}
		}

		// Token: 0x04000D97 RID: 3479
		private byte[] m_IL;

		// Token: 0x04000D98 RID: 3480
		private ExceptionHandlingClause[] m_exceptionHandlingClauses;

		// Token: 0x04000D99 RID: 3481
		private LocalVariableInfo[] m_localVariables;

		// Token: 0x04000D9A RID: 3482
		internal MethodBase m_methodBase;

		// Token: 0x04000D9B RID: 3483
		private int m_localSignatureMetadataToken;

		// Token: 0x04000D9C RID: 3484
		private int m_maxStackSize;

		// Token: 0x04000D9D RID: 3485
		private bool m_initLocals;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200082E RID: 2094
	[ComDefaultInterface(typeof(_LocalBuilder))]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	public sealed class LocalBuilder : LocalVariableInfo, _LocalBuilder
	{
		// Token: 0x06004A7F RID: 19071 RVA: 0x00102C38 File Offset: 0x00101C38
		private LocalBuilder()
		{
		}

		// Token: 0x06004A80 RID: 19072 RVA: 0x00102C40 File Offset: 0x00101C40
		internal LocalBuilder(int localIndex, Type localType, MethodInfo methodBuilder) : this(localIndex, localType, methodBuilder, false)
		{
		}

		// Token: 0x06004A81 RID: 19073 RVA: 0x00102C4C File Offset: 0x00101C4C
		internal LocalBuilder(int localIndex, Type localType, MethodInfo methodBuilder, bool isPinned)
		{
			this.m_isPinned = isPinned;
			this.m_localIndex = localIndex;
			this.m_localType = localType;
			this.m_methodBuilder = methodBuilder;
		}

		// Token: 0x06004A82 RID: 19074 RVA: 0x00102C71 File Offset: 0x00101C71
		internal int GetLocalIndex()
		{
			return this.m_localIndex;
		}

		// Token: 0x06004A83 RID: 19075 RVA: 0x00102C79 File Offset: 0x00101C79
		internal MethodInfo GetMethodBuilder()
		{
			return this.m_methodBuilder;
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x06004A84 RID: 19076 RVA: 0x00102C81 File Offset: 0x00101C81
		public override bool IsPinned
		{
			get
			{
				return this.m_isPinned;
			}
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06004A85 RID: 19077 RVA: 0x00102C89 File Offset: 0x00101C89
		public override Type LocalType
		{
			get
			{
				return this.m_localType;
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06004A86 RID: 19078 RVA: 0x00102C91 File Offset: 0x00101C91
		public override int LocalIndex
		{
			get
			{
				return this.m_localIndex;
			}
		}

		// Token: 0x06004A87 RID: 19079 RVA: 0x00102C99 File Offset: 0x00101C99
		public void SetLocalSymInfo(string name)
		{
			this.SetLocalSymInfo(name, 0, 0);
		}

		// Token: 0x06004A88 RID: 19080 RVA: 0x00102CA4 File Offset: 0x00101CA4
		public void SetLocalSymInfo(string name, int startOffset, int endOffset)
		{
			MethodBuilder methodBuilder = this.m_methodBuilder as MethodBuilder;
			if (methodBuilder == null)
			{
				throw new NotSupportedException();
			}
			ModuleBuilder moduleBuilder = (ModuleBuilder)methodBuilder.Module;
			if (methodBuilder.IsTypeCreated())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TypeHasBeenCreated"));
			}
			if (moduleBuilder.GetSymWriter() == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotADebugModule"));
			}
			SignatureHelper fieldSigHelper = SignatureHelper.GetFieldSigHelper(moduleBuilder);
			fieldSigHelper.AddArgument(this.m_localType);
			int num;
			byte[] sourceArray = fieldSigHelper.InternalGetSignature(out num);
			byte[] array = new byte[num - 1];
			Array.Copy(sourceArray, 1, array, 0, num - 1);
			int currentActiveScopeIndex = methodBuilder.GetILGenerator().m_ScopeTree.GetCurrentActiveScopeIndex();
			if (currentActiveScopeIndex == -1)
			{
				methodBuilder.m_localSymInfo.AddLocalSymInfo(name, array, this.m_localIndex, startOffset, endOffset);
				return;
			}
			methodBuilder.GetILGenerator().m_ScopeTree.AddLocalSymInfoToCurrentScope(name, array, this.m_localIndex, startOffset, endOffset);
		}

		// Token: 0x06004A89 RID: 19081 RVA: 0x00102D85 File Offset: 0x00101D85
		void _LocalBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004A8A RID: 19082 RVA: 0x00102D8C File Offset: 0x00101D8C
		void _LocalBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004A8B RID: 19083 RVA: 0x00102D93 File Offset: 0x00101D93
		void _LocalBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004A8C RID: 19084 RVA: 0x00102D9A File Offset: 0x00101D9A
		void _LocalBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04002620 RID: 9760
		private int m_localIndex;

		// Token: 0x04002621 RID: 9761
		private Type m_localType;

		// Token: 0x04002622 RID: 9762
		private MethodInfo m_methodBuilder;

		// Token: 0x04002623 RID: 9763
		private bool m_isPinned;
	}
}

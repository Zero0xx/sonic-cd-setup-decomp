using System;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000816 RID: 2070
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_ILGenerator))]
	public class ILGenerator : _ILGenerator
	{
		// Token: 0x06004935 RID: 18741 RVA: 0x000FDB38 File Offset: 0x000FCB38
		internal static int[] EnlargeArray(int[] incoming)
		{
			int[] array = new int[incoming.Length * 2];
			Array.Copy(incoming, array, incoming.Length);
			return array;
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x000FDB5C File Offset: 0x000FCB5C
		internal static byte[] EnlargeArray(byte[] incoming)
		{
			byte[] array = new byte[incoming.Length * 2];
			Array.Copy(incoming, array, incoming.Length);
			return array;
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x000FDB80 File Offset: 0x000FCB80
		internal static byte[] EnlargeArray(byte[] incoming, int requiredSize)
		{
			byte[] array = new byte[requiredSize];
			Array.Copy(incoming, array, incoming.Length);
			return array;
		}

		// Token: 0x06004938 RID: 18744 RVA: 0x000FDBA0 File Offset: 0x000FCBA0
		internal static __FixupData[] EnlargeArray(__FixupData[] incoming)
		{
			__FixupData[] array = new __FixupData[incoming.Length * 2];
			Array.Copy(incoming, array, incoming.Length);
			return array;
		}

		// Token: 0x06004939 RID: 18745 RVA: 0x000FDBC4 File Offset: 0x000FCBC4
		internal static Type[] EnlargeArray(Type[] incoming)
		{
			Type[] array = new Type[incoming.Length * 2];
			Array.Copy(incoming, array, incoming.Length);
			return array;
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x000FDBE8 File Offset: 0x000FCBE8
		internal static __ExceptionInfo[] EnlargeArray(__ExceptionInfo[] incoming)
		{
			__ExceptionInfo[] array = new __ExceptionInfo[incoming.Length * 2];
			Array.Copy(incoming, array, incoming.Length);
			return array;
		}

		// Token: 0x0600493B RID: 18747 RVA: 0x000FDC0C File Offset: 0x000FCC0C
		internal static int CalculateNumberOfExceptions(__ExceptionInfo[] excp)
		{
			int num = 0;
			if (excp == null)
			{
				return 0;
			}
			for (int i = 0; i < excp.Length; i++)
			{
				num += excp[i].GetNumberOfCatches();
			}
			return num;
		}

		// Token: 0x0600493C RID: 18748 RVA: 0x000FDC3A File Offset: 0x000FCC3A
		internal ILGenerator(MethodInfo methodBuilder) : this(methodBuilder, 64)
		{
		}

		// Token: 0x0600493D RID: 18749 RVA: 0x000FDC48 File Offset: 0x000FCC48
		internal ILGenerator(MethodInfo methodBuilder, int size)
		{
			if (size < 16)
			{
				this.m_ILStream = new byte[16];
			}
			else
			{
				this.m_ILStream = new byte[size];
			}
			this.m_length = 0;
			this.m_labelCount = 0;
			this.m_fixupCount = 0;
			this.m_labelList = null;
			this.m_fixupData = null;
			this.m_exceptions = null;
			this.m_exceptionCount = 0;
			this.m_currExcStack = null;
			this.m_currExcStackCount = 0;
			this.m_RelocFixupList = new int[64];
			this.m_RelocFixupCount = 0;
			this.m_RVAFixupList = new int[64];
			this.m_RVAFixupCount = 0;
			this.m_ScopeTree = new ScopeTree();
			this.m_LineNumberInfo = new LineNumberInfo();
			this.m_methodBuilder = methodBuilder;
			this.m_localCount = 0;
			MethodBuilder methodBuilder2 = this.m_methodBuilder as MethodBuilder;
			if (methodBuilder2 == null)
			{
				this.m_localSignature = SignatureHelper.GetLocalVarSigHelper(null);
				return;
			}
			this.m_localSignature = SignatureHelper.GetLocalVarSigHelper(methodBuilder2.GetTypeBuilder().Module);
		}

		// Token: 0x0600493E RID: 18750 RVA: 0x000FDD38 File Offset: 0x000FCD38
		internal ILGenerator(int size)
		{
			if (size < 16)
			{
				this.m_ILStream = new byte[16];
			}
			else
			{
				this.m_ILStream = new byte[size];
			}
			this.m_length = 0;
			this.m_labelCount = 0;
			this.m_fixupCount = 0;
			this.m_labelList = null;
			this.m_fixupData = null;
			this.m_exceptions = null;
			this.m_exceptionCount = 0;
			this.m_currExcStack = null;
			this.m_currExcStackCount = 0;
			this.m_RelocFixupList = new int[64];
			this.m_RelocFixupCount = 0;
			this.m_RVAFixupList = new int[64];
			this.m_RVAFixupCount = 0;
			this.m_ScopeTree = new ScopeTree();
			this.m_LineNumberInfo = new LineNumberInfo();
			this.m_methodBuilder = null;
			this.m_localCount = 0;
			this.m_localSignature = SignatureHelper.GetLocalVarSigHelper(null);
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x000FDE04 File Offset: 0x000FCE04
		private void RecordTokenFixup()
		{
			if (this.m_RelocFixupCount >= this.m_RelocFixupList.Length)
			{
				this.m_RelocFixupList = ILGenerator.EnlargeArray(this.m_RelocFixupList);
			}
			this.m_RelocFixupList[this.m_RelocFixupCount++] = this.m_length;
		}

		// Token: 0x06004940 RID: 18752 RVA: 0x000FDE50 File Offset: 0x000FCE50
		internal void InternalEmit(OpCode opcode)
		{
			if (opcode.m_size == 1)
			{
				this.m_ILStream[this.m_length++] = opcode.m_s2;
			}
			else
			{
				this.m_ILStream[this.m_length++] = opcode.m_s1;
				this.m_ILStream[this.m_length++] = opcode.m_s2;
			}
			this.UpdateStackSize(opcode, opcode.StackChange());
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x000FDED4 File Offset: 0x000FCED4
		internal void UpdateStackSize(OpCode opcode, int stackchange)
		{
			this.m_maxMidStackCur += stackchange;
			if (this.m_maxMidStackCur > this.m_maxMidStack)
			{
				this.m_maxMidStack = this.m_maxMidStackCur;
			}
			else if (this.m_maxMidStackCur < 0)
			{
				this.m_maxMidStackCur = 0;
			}
			if (opcode.EndsUncondJmpBlk())
			{
				this.m_maxStackSize += this.m_maxMidStack;
				this.m_maxMidStack = 0;
				this.m_maxMidStackCur = 0;
			}
		}

		// Token: 0x06004942 RID: 18754 RVA: 0x000FDF48 File Offset: 0x000FCF48
		internal virtual int GetMethodToken(MethodBase method, Type[] optionalParameterTypes)
		{
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
			int num;
			if (method.IsGenericMethod)
			{
				MethodInfo methodInfo = method as MethodInfo;
				if (!method.IsGenericMethodDefinition && method is MethodInfo)
				{
					methodInfo = ((MethodInfo)method).GetGenericMethodDefinition();
				}
				if (!methodInfo.Module.Equals(this.m_methodBuilder.Module) || (methodInfo.DeclaringType != null && methodInfo.DeclaringType.IsGenericType))
				{
					num = this.GetMemberRefToken(methodInfo, null);
				}
				else
				{
					num = moduleBuilder.GetMethodTokenInternal(methodInfo).Token;
				}
				int sigLength;
				byte[] signature = SignatureHelper.GetMethodSpecSigHelper(moduleBuilder, method.GetGenericArguments()).InternalGetSignature(out sigLength);
				num = TypeBuilder.InternalDefineMethodSpec(num, signature, sigLength, moduleBuilder);
			}
			else if ((method.CallingConvention & CallingConventions.VarArgs) == (CallingConventions)0 && (method.DeclaringType == null || !method.DeclaringType.IsGenericType))
			{
				if (method is MethodInfo)
				{
					num = moduleBuilder.GetMethodTokenInternal(method as MethodInfo).Token;
				}
				else
				{
					num = moduleBuilder.GetConstructorToken(method as ConstructorInfo).Token;
				}
			}
			else
			{
				num = this.GetMemberRefToken(method, optionalParameterTypes);
			}
			return num;
		}

		// Token: 0x06004943 RID: 18755 RVA: 0x000FE062 File Offset: 0x000FD062
		internal virtual int GetMemberRefToken(MethodBase method, Type[] optionalParameterTypes)
		{
			return ((ModuleBuilder)this.m_methodBuilder.Module).GetMemberRefToken(method, optionalParameterTypes);
		}

		// Token: 0x06004944 RID: 18756 RVA: 0x000FE07B File Offset: 0x000FD07B
		internal virtual SignatureHelper GetMemberRefSignature(CallingConventions call, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			return this.GetMemberRefSignature(call, returnType, parameterTypes, optionalParameterTypes, 0);
		}

		// Token: 0x06004945 RID: 18757 RVA: 0x000FE089 File Offset: 0x000FD089
		internal virtual SignatureHelper GetMemberRefSignature(CallingConventions call, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes, int cGenericParameters)
		{
			return ((ModuleBuilder)this.m_methodBuilder.Module).GetMemberRefSignature(call, returnType, parameterTypes, optionalParameterTypes, cGenericParameters);
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x000FE0A8 File Offset: 0x000FD0A8
		internal virtual byte[] BakeByteArray()
		{
			if (this.m_currExcStackCount != 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_UnclosedExceptionBlock"));
			}
			if (this.m_length == 0)
			{
				return null;
			}
			int length = this.m_length;
			byte[] array = new byte[length];
			Array.Copy(this.m_ILStream, array, length);
			for (int i = 0; i < this.m_fixupCount; i++)
			{
				int num = this.GetLabelPos(this.m_fixupData[i].m_fixupLabel) - (this.m_fixupData[i].m_fixupPos + this.m_fixupData[i].m_fixupInstSize);
				if (this.m_fixupData[i].m_fixupInstSize == 1)
				{
					if (num < -128 || num > 127)
					{
						throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("NotSupported_IllegalOneByteBranch"), new object[]
						{
							this.m_fixupData[i].m_fixupPos,
							num
						}));
					}
					if (num < 0)
					{
						array[this.m_fixupData[i].m_fixupPos] = (byte)(256 + num);
					}
					else
					{
						array[this.m_fixupData[i].m_fixupPos] = (byte)num;
					}
				}
				else
				{
					this.PutInteger4(num, this.m_fixupData[i].m_fixupPos, array);
				}
			}
			return array;
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x000FE204 File Offset: 0x000FD204
		internal virtual __ExceptionInfo[] GetExceptions()
		{
			if (this.m_currExcStackCount != 0)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_UnclosedExceptionBlock"));
			}
			if (this.m_exceptionCount == 0)
			{
				return null;
			}
			__ExceptionInfo[] array = new __ExceptionInfo[this.m_exceptionCount];
			Array.Copy(this.m_exceptions, array, this.m_exceptionCount);
			this.SortExceptions(array);
			return array;
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x000FE25C File Offset: 0x000FD25C
		internal virtual void EnsureCapacity(int size)
		{
			if (this.m_length + size >= this.m_ILStream.Length)
			{
				if (this.m_length + size >= 2 * this.m_ILStream.Length)
				{
					this.m_ILStream = ILGenerator.EnlargeArray(this.m_ILStream, this.m_length + size);
					return;
				}
				this.m_ILStream = ILGenerator.EnlargeArray(this.m_ILStream);
			}
		}

		// Token: 0x06004949 RID: 18761 RVA: 0x000FE2BA File Offset: 0x000FD2BA
		internal virtual int PutInteger4(int value, int startPos, byte[] array)
		{
			array[startPos++] = (byte)value;
			array[startPos++] = (byte)(value >> 8);
			array[startPos++] = (byte)(value >> 16);
			array[startPos++] = (byte)(value >> 24);
			return startPos;
		}

		// Token: 0x0600494A RID: 18762 RVA: 0x000FE2F0 File Offset: 0x000FD2F0
		internal virtual int GetLabelPos(Label lbl)
		{
			int labelValue = lbl.GetLabelValue();
			if (labelValue < 0 || labelValue >= this.m_labelCount)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadLabel"));
			}
			if (this.m_labelList[labelValue] < 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadLabelContent"));
			}
			return this.m_labelList[labelValue];
		}

		// Token: 0x0600494B RID: 18763 RVA: 0x000FE348 File Offset: 0x000FD348
		internal virtual void AddFixup(Label lbl, int pos, int instSize)
		{
			if (this.m_fixupData == null)
			{
				this.m_fixupData = new __FixupData[64];
			}
			if (this.m_fixupCount >= this.m_fixupData.Length)
			{
				this.m_fixupData = ILGenerator.EnlargeArray(this.m_fixupData);
			}
			this.m_fixupData[this.m_fixupCount].m_fixupPos = pos;
			this.m_fixupData[this.m_fixupCount].m_fixupLabel = lbl;
			this.m_fixupData[this.m_fixupCount].m_fixupInstSize = instSize;
			this.m_fixupCount++;
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x000FE3E0 File Offset: 0x000FD3E0
		internal virtual int GetMaxStackSize()
		{
			MethodBuilder methodBuilder = this.m_methodBuilder as MethodBuilder;
			if (methodBuilder == null)
			{
				throw new NotSupportedException();
			}
			return this.m_maxStackSize + methodBuilder.GetNumberOfExceptions();
		}

		// Token: 0x0600494D RID: 18765 RVA: 0x000FE410 File Offset: 0x000FD410
		internal virtual void SortExceptions(__ExceptionInfo[] exceptions)
		{
			int num = exceptions.Length;
			for (int i = 0; i < num; i++)
			{
				int num2 = i;
				for (int j = i + 1; j < num; j++)
				{
					if (exceptions[num2].IsInner(exceptions[j]))
					{
						num2 = j;
					}
				}
				__ExceptionInfo _ExceptionInfo = exceptions[i];
				exceptions[i] = exceptions[num2];
				exceptions[num2] = _ExceptionInfo;
			}
		}

		// Token: 0x0600494E RID: 18766 RVA: 0x000FE460 File Offset: 0x000FD460
		internal virtual int[] GetTokenFixups()
		{
			int[] array = new int[this.m_RelocFixupCount];
			Array.Copy(this.m_RelocFixupList, array, this.m_RelocFixupCount);
			return array;
		}

		// Token: 0x0600494F RID: 18767 RVA: 0x000FE48C File Offset: 0x000FD48C
		internal virtual int[] GetRVAFixups()
		{
			int[] array = new int[this.m_RVAFixupCount];
			Array.Copy(this.m_RVAFixupList, array, this.m_RVAFixupCount);
			return array;
		}

		// Token: 0x06004950 RID: 18768 RVA: 0x000FE4B8 File Offset: 0x000FD4B8
		public virtual void Emit(OpCode opcode)
		{
			this.EnsureCapacity(3);
			this.InternalEmit(opcode);
		}

		// Token: 0x06004951 RID: 18769 RVA: 0x000FE4C8 File Offset: 0x000FD4C8
		public virtual void Emit(OpCode opcode, byte arg)
		{
			this.EnsureCapacity(4);
			this.InternalEmit(opcode);
			this.m_ILStream[this.m_length++] = arg;
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x000FE4FC File Offset: 0x000FD4FC
		[CLSCompliant(false)]
		public void Emit(OpCode opcode, sbyte arg)
		{
			this.EnsureCapacity(4);
			this.InternalEmit(opcode);
			if (arg < 0)
			{
				this.m_ILStream[this.m_length++] = (byte)(256 + (int)arg);
				return;
			}
			this.m_ILStream[this.m_length++] = (byte)arg;
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x000FE558 File Offset: 0x000FD558
		public virtual void Emit(OpCode opcode, short arg)
		{
			this.EnsureCapacity(5);
			this.InternalEmit(opcode);
			this.m_ILStream[this.m_length++] = (byte)arg;
			this.m_ILStream[this.m_length++] = (byte)(arg >> 8);
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x000FE5A9 File Offset: 0x000FD5A9
		public virtual void Emit(OpCode opcode, int arg)
		{
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			this.m_length = this.PutInteger4(arg, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004955 RID: 18773 RVA: 0x000FE5D4 File Offset: 0x000FD5D4
		public virtual void Emit(OpCode opcode, MethodInfo meth)
		{
			if (opcode.Equals(OpCodes.Call) || opcode.Equals(OpCodes.Callvirt) || opcode.Equals(OpCodes.Newobj))
			{
				this.EmitCall(opcode, meth, null);
				return;
			}
			int stackchange = 0;
			if (meth == null)
			{
				throw new ArgumentNullException("meth");
			}
			int methodToken = this.GetMethodToken(meth, null);
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			this.UpdateStackSize(opcode, stackchange);
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(methodToken, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004956 RID: 18774 RVA: 0x000FE664 File Offset: 0x000FD664
		public virtual void EmitCalli(OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
		{
			int num = 0;
			if (optionalParameterTypes != null && (callingConvention & CallingConventions.VarArgs) == (CallingConventions)0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAVarArgCallingConvention"));
			}
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
			SignatureHelper memberRefSignature = this.GetMemberRefSignature(callingConvention, returnType, parameterTypes, optionalParameterTypes);
			this.EnsureCapacity(7);
			this.Emit(OpCodes.Calli);
			if (returnType != typeof(void))
			{
				num++;
			}
			if (parameterTypes != null)
			{
				num -= parameterTypes.Length;
			}
			if (optionalParameterTypes != null)
			{
				num -= optionalParameterTypes.Length;
			}
			if ((callingConvention & CallingConventions.HasThis) == CallingConventions.HasThis)
			{
				num--;
			}
			num--;
			this.UpdateStackSize(opcode, num);
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(moduleBuilder.GetSignatureToken(memberRefSignature).Token, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004957 RID: 18775 RVA: 0x000FE728 File Offset: 0x000FD728
		public virtual void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
		{
			int num = 0;
			int num2 = 0;
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
			if (parameterTypes != null)
			{
				num2 = parameterTypes.Length;
			}
			SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(moduleBuilder, unmanagedCallConv, returnType);
			if (parameterTypes != null)
			{
				for (int i = 0; i < num2; i++)
				{
					methodSigHelper.AddArgument(parameterTypes[i]);
				}
			}
			if (returnType != typeof(void))
			{
				num++;
			}
			if (parameterTypes != null)
			{
				num -= num2;
			}
			num--;
			this.UpdateStackSize(opcode, num);
			this.EnsureCapacity(7);
			this.Emit(OpCodes.Calli);
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(moduleBuilder.GetSignatureToken(methodSigHelper).Token, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004958 RID: 18776 RVA: 0x000FE7E0 File Offset: 0x000FD7E0
		public virtual void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
		{
			int num = 0;
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			int methodToken = this.GetMethodToken(methodInfo, optionalParameterTypes);
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			if (methodInfo.GetReturnType() != typeof(void))
			{
				num++;
			}
			if (methodInfo.GetParameterTypes() != null)
			{
				num -= methodInfo.GetParameterTypes().Length;
			}
			if (!(methodInfo is SymbolMethod) && !methodInfo.IsStatic && !opcode.Equals(OpCodes.Newobj))
			{
				num--;
			}
			if (optionalParameterTypes != null)
			{
				num -= optionalParameterTypes.Length;
			}
			this.UpdateStackSize(opcode, num);
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(methodToken, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004959 RID: 18777 RVA: 0x000FE890 File Offset: 0x000FD890
		public virtual void Emit(OpCode opcode, SignatureHelper signature)
		{
			int num = 0;
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
			if (signature == null)
			{
				throw new ArgumentNullException("signature");
			}
			int token = moduleBuilder.GetSignatureToken(signature).Token;
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			if (opcode.m_pop == StackBehaviour.Varpop)
			{
				num -= signature.ArgumentCount;
				num--;
				this.UpdateStackSize(opcode, num);
			}
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(token, this.m_length, this.m_ILStream);
		}

		// Token: 0x0600495A RID: 18778 RVA: 0x000FE91C File Offset: 0x000FD91C
		[ComVisible(true)]
		public virtual void Emit(OpCode opcode, ConstructorInfo con)
		{
			int num = 0;
			int methodToken = this.GetMethodToken(con, null);
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			if (opcode.m_push == StackBehaviour.Varpush)
			{
				num++;
			}
			if (opcode.m_pop == StackBehaviour.Varpop && con.GetParameterTypes() != null)
			{
				num -= con.GetParameterTypes().Length;
			}
			this.UpdateStackSize(opcode, num);
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(methodToken, this.m_length, this.m_ILStream);
		}

		// Token: 0x0600495B RID: 18779 RVA: 0x000FE998 File Offset: 0x000FD998
		public virtual void Emit(OpCode opcode, Type cls)
		{
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
			int token;
			if (opcode == OpCodes.Ldtoken && cls != null && cls.IsGenericTypeDefinition)
			{
				token = moduleBuilder.GetTypeToken(cls).Token;
			}
			else
			{
				token = moduleBuilder.GetTypeTokenInternal(cls).Token;
			}
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(token, this.m_length, this.m_ILStream);
		}

		// Token: 0x0600495C RID: 18780 RVA: 0x000FEA20 File Offset: 0x000FDA20
		public virtual void Emit(OpCode opcode, long arg)
		{
			this.EnsureCapacity(11);
			this.InternalEmit(opcode);
			this.m_ILStream[this.m_length++] = (byte)arg;
			this.m_ILStream[this.m_length++] = (byte)(arg >> 8);
			this.m_ILStream[this.m_length++] = (byte)(arg >> 16);
			this.m_ILStream[this.m_length++] = (byte)(arg >> 24);
			this.m_ILStream[this.m_length++] = (byte)(arg >> 32);
			this.m_ILStream[this.m_length++] = (byte)(arg >> 40);
			this.m_ILStream[this.m_length++] = (byte)(arg >> 48);
			this.m_ILStream[this.m_length++] = (byte)(arg >> 56);
		}

		// Token: 0x0600495D RID: 18781 RVA: 0x000FEB28 File Offset: 0x000FDB28
		public unsafe virtual void Emit(OpCode opcode, float arg)
		{
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			uint num = *(uint*)(&arg);
			this.m_ILStream[this.m_length++] = (byte)num;
			this.m_ILStream[this.m_length++] = (byte)(num >> 8);
			this.m_ILStream[this.m_length++] = (byte)(num >> 16);
			this.m_ILStream[this.m_length++] = (byte)(num >> 24);
		}

		// Token: 0x0600495E RID: 18782 RVA: 0x000FEBBC File Offset: 0x000FDBBC
		public unsafe virtual void Emit(OpCode opcode, double arg)
		{
			this.EnsureCapacity(11);
			this.InternalEmit(opcode);
			ulong num = (ulong)(*(long*)(&arg));
			this.m_ILStream[this.m_length++] = (byte)num;
			this.m_ILStream[this.m_length++] = (byte)(num >> 8);
			this.m_ILStream[this.m_length++] = (byte)(num >> 16);
			this.m_ILStream[this.m_length++] = (byte)(num >> 24);
			this.m_ILStream[this.m_length++] = (byte)(num >> 32);
			this.m_ILStream[this.m_length++] = (byte)(num >> 40);
			this.m_ILStream[this.m_length++] = (byte)(num >> 48);
			this.m_ILStream[this.m_length++] = (byte)(num >> 56);
		}

		// Token: 0x0600495F RID: 18783 RVA: 0x000FECCC File Offset: 0x000FDCCC
		public virtual void Emit(OpCode opcode, Label label)
		{
			label.GetLabelValue();
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			if (OpCodes.TakesSingleByteArgument(opcode))
			{
				this.AddFixup(label, this.m_length, 1);
				this.m_length++;
				return;
			}
			this.AddFixup(label, this.m_length, 4);
			this.m_length += 4;
		}

		// Token: 0x06004960 RID: 18784 RVA: 0x000FED30 File Offset: 0x000FDD30
		public virtual void Emit(OpCode opcode, Label[] labels)
		{
			int num = labels.Length;
			this.EnsureCapacity(num * 4 + 7);
			this.InternalEmit(opcode);
			this.m_length = this.PutInteger4(num, this.m_length, this.m_ILStream);
			int i = num * 4;
			int num2 = 0;
			while (i > 0)
			{
				this.AddFixup(labels[num2], this.m_length, i);
				this.m_length += 4;
				i -= 4;
				num2++;
			}
		}

		// Token: 0x06004961 RID: 18785 RVA: 0x000FEDA8 File Offset: 0x000FDDA8
		public virtual void Emit(OpCode opcode, FieldInfo field)
		{
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
			int token = moduleBuilder.GetFieldToken(field).Token;
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			this.RecordTokenFixup();
			this.m_length = this.PutInteger4(token, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x000FEE04 File Offset: 0x000FDE04
		public virtual void Emit(OpCode opcode, string str)
		{
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
			int token = moduleBuilder.GetStringConstant(str).Token;
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			this.m_length = this.PutInteger4(token, this.m_length, this.m_ILStream);
		}

		// Token: 0x06004963 RID: 18787 RVA: 0x000FEE5C File Offset: 0x000FDE5C
		public virtual void Emit(OpCode opcode, LocalBuilder local)
		{
			if (local == null)
			{
				throw new ArgumentNullException("local");
			}
			int localIndex = local.GetLocalIndex();
			if (local.GetMethodBuilder() != this.m_methodBuilder)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_UnmatchedMethodForLocal"), "local");
			}
			if (opcode.Equals(OpCodes.Ldloc))
			{
				switch (localIndex)
				{
				case 0:
					opcode = OpCodes.Ldloc_0;
					break;
				case 1:
					opcode = OpCodes.Ldloc_1;
					break;
				case 2:
					opcode = OpCodes.Ldloc_2;
					break;
				case 3:
					opcode = OpCodes.Ldloc_3;
					break;
				default:
					if (localIndex <= 255)
					{
						opcode = OpCodes.Ldloc_S;
					}
					break;
				}
			}
			else if (opcode.Equals(OpCodes.Stloc))
			{
				switch (localIndex)
				{
				case 0:
					opcode = OpCodes.Stloc_0;
					break;
				case 1:
					opcode = OpCodes.Stloc_1;
					break;
				case 2:
					opcode = OpCodes.Stloc_2;
					break;
				case 3:
					opcode = OpCodes.Stloc_3;
					break;
				default:
					if (localIndex <= 255)
					{
						opcode = OpCodes.Stloc_S;
					}
					break;
				}
			}
			else if (opcode.Equals(OpCodes.Ldloca) && localIndex <= 255)
			{
				opcode = OpCodes.Ldloca_S;
			}
			this.EnsureCapacity(7);
			this.InternalEmit(opcode);
			if (opcode.OperandType == OperandType.InlineNone)
			{
				return;
			}
			if (!OpCodes.TakesSingleByteArgument(opcode))
			{
				this.m_ILStream[this.m_length++] = (byte)localIndex;
				this.m_ILStream[this.m_length++] = (byte)(localIndex >> 8);
				return;
			}
			if (localIndex > 255)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadInstructionOrIndexOutOfBound"));
			}
			this.m_ILStream[this.m_length++] = (byte)localIndex;
		}

		// Token: 0x06004964 RID: 18788 RVA: 0x000FF018 File Offset: 0x000FE018
		public virtual Label BeginExceptionBlock()
		{
			if (this.m_exceptions == null)
			{
				this.m_exceptions = new __ExceptionInfo[8];
			}
			if (this.m_currExcStack == null)
			{
				this.m_currExcStack = new __ExceptionInfo[8];
			}
			if (this.m_exceptionCount >= this.m_exceptions.Length)
			{
				this.m_exceptions = ILGenerator.EnlargeArray(this.m_exceptions);
			}
			if (this.m_currExcStackCount >= this.m_currExcStack.Length)
			{
				this.m_currExcStack = ILGenerator.EnlargeArray(this.m_currExcStack);
			}
			Label label = this.DefineLabel();
			__ExceptionInfo _ExceptionInfo = new __ExceptionInfo(this.m_length, label);
			this.m_exceptions[this.m_exceptionCount++] = _ExceptionInfo;
			this.m_currExcStack[this.m_currExcStackCount++] = _ExceptionInfo;
			return label;
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x000FF0D8 File Offset: 0x000FE0D8
		public virtual void EndExceptionBlock()
		{
			if (this.m_currExcStackCount == 0)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
			}
			__ExceptionInfo _ExceptionInfo = this.m_currExcStack[this.m_currExcStackCount - 1];
			this.m_currExcStack[this.m_currExcStackCount - 1] = null;
			this.m_currExcStackCount--;
			Label endLabel = _ExceptionInfo.GetEndLabel();
			int currentState = _ExceptionInfo.GetCurrentState();
			if (currentState == 1 || currentState == 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Argument_BadExceptionCodeGen"));
			}
			if (currentState == 2)
			{
				this.Emit(OpCodes.Leave, endLabel);
			}
			else if (currentState == 3 || currentState == 4)
			{
				this.Emit(OpCodes.Endfinally);
			}
			if (this.m_labelList[endLabel.GetLabelValue()] == -1)
			{
				this.MarkLabel(endLabel);
			}
			else
			{
				this.MarkLabel(_ExceptionInfo.GetFinallyEndLabel());
			}
			_ExceptionInfo.Done(this.m_length);
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x000FF1A8 File Offset: 0x000FE1A8
		public virtual void BeginExceptFilterBlock()
		{
			if (this.m_currExcStackCount == 0)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
			}
			__ExceptionInfo _ExceptionInfo = this.m_currExcStack[this.m_currExcStackCount - 1];
			Label endLabel = _ExceptionInfo.GetEndLabel();
			this.Emit(OpCodes.Leave, endLabel);
			_ExceptionInfo.MarkFilterAddr(this.m_length);
		}

		// Token: 0x06004967 RID: 18791 RVA: 0x000FF1FC File Offset: 0x000FE1FC
		public virtual void BeginCatchBlock(Type exceptionType)
		{
			if (this.m_currExcStackCount == 0)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
			}
			__ExceptionInfo _ExceptionInfo = this.m_currExcStack[this.m_currExcStackCount - 1];
			if (_ExceptionInfo.GetCurrentState() == 1)
			{
				if (exceptionType != null)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_ShouldNotSpecifyExceptionType"));
				}
				this.Emit(OpCodes.Endfilter);
			}
			else
			{
				if (exceptionType == null)
				{
					throw new ArgumentNullException("exceptionType");
				}
				Label endLabel = _ExceptionInfo.GetEndLabel();
				this.Emit(OpCodes.Leave, endLabel);
			}
			_ExceptionInfo.MarkCatchAddr(this.m_length, exceptionType);
		}

		// Token: 0x06004968 RID: 18792 RVA: 0x000FF288 File Offset: 0x000FE288
		public virtual void BeginFaultBlock()
		{
			if (this.m_currExcStackCount == 0)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
			}
			__ExceptionInfo _ExceptionInfo = this.m_currExcStack[this.m_currExcStackCount - 1];
			Label endLabel = _ExceptionInfo.GetEndLabel();
			this.Emit(OpCodes.Leave, endLabel);
			_ExceptionInfo.MarkFaultAddr(this.m_length);
		}

		// Token: 0x06004969 RID: 18793 RVA: 0x000FF2DC File Offset: 0x000FE2DC
		public virtual void BeginFinallyBlock()
		{
			if (this.m_currExcStackCount == 0)
			{
				throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
			}
			__ExceptionInfo _ExceptionInfo = this.m_currExcStack[this.m_currExcStackCount - 1];
			int currentState = _ExceptionInfo.GetCurrentState();
			Label endLabel = _ExceptionInfo.GetEndLabel();
			int num = 0;
			if (currentState != 0)
			{
				this.Emit(OpCodes.Leave, endLabel);
				num = this.m_length;
			}
			this.MarkLabel(endLabel);
			Label label = this.DefineLabel();
			_ExceptionInfo.SetFinallyEndLabel(label);
			this.Emit(OpCodes.Leave, label);
			if (num == 0)
			{
				num = this.m_length;
			}
			_ExceptionInfo.MarkFinallyAddr(this.m_length, num);
		}

		// Token: 0x0600496A RID: 18794 RVA: 0x000FF374 File Offset: 0x000FE374
		public virtual Label DefineLabel()
		{
			if (this.m_labelList == null)
			{
				this.m_labelList = new int[16];
			}
			if (this.m_labelCount >= this.m_labelList.Length)
			{
				this.m_labelList = ILGenerator.EnlargeArray(this.m_labelList);
			}
			this.m_labelList[this.m_labelCount] = -1;
			return new Label(this.m_labelCount++);
		}

		// Token: 0x0600496B RID: 18795 RVA: 0x000FF3DC File Offset: 0x000FE3DC
		public virtual void MarkLabel(Label loc)
		{
			int labelValue = loc.GetLabelValue();
			if (labelValue < 0 || labelValue >= this.m_labelList.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidLabel"));
			}
			if (this.m_labelList[labelValue] != -1)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_RedefinedLabel"));
			}
			this.m_labelList[labelValue] = this.m_length;
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x000FF43C File Offset: 0x000FE43C
		public virtual void ThrowException(Type excType)
		{
			if (excType == null)
			{
				throw new ArgumentNullException("excType");
			}
			ModuleBuilder moduleBuilder = (ModuleBuilder)this.m_methodBuilder.Module;
			if (!excType.IsSubclassOf(typeof(Exception)) && excType != typeof(Exception))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_NotExceptionType"));
			}
			ConstructorInfo constructor = excType.GetConstructor(Type.EmptyTypes);
			if (constructor == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MissingDefaultConstructor"));
			}
			this.Emit(OpCodes.Newobj, constructor);
			this.Emit(OpCodes.Throw);
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x000FF4D0 File Offset: 0x000FE4D0
		public virtual void EmitWriteLine(string value)
		{
			this.Emit(OpCodes.Ldstr, value);
			Type[] types = new Type[]
			{
				typeof(string)
			};
			MethodInfo method = typeof(Console).GetMethod("WriteLine", types);
			this.Emit(OpCodes.Call, method);
		}

		// Token: 0x0600496E RID: 18798 RVA: 0x000FF520 File Offset: 0x000FE520
		public virtual void EmitWriteLine(LocalBuilder localBuilder)
		{
			if (this.m_methodBuilder == null)
			{
				throw new ArgumentException(Environment.GetResourceString("InvalidOperation_BadILGeneratorUsage"));
			}
			MethodInfo method = typeof(Console).GetMethod("get_Out");
			this.Emit(OpCodes.Call, method);
			this.Emit(OpCodes.Ldloc, localBuilder);
			Type[] array = new Type[1];
			object localType = localBuilder.LocalType;
			if (localType is TypeBuilder || localType is EnumBuilder)
			{
				throw new ArgumentException(Environment.GetResourceString("NotSupported_OutputStreamUsingTypeBuilder"));
			}
			array[0] = (Type)localType;
			MethodInfo method2 = typeof(TextWriter).GetMethod("WriteLine", array);
			if (method2 == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmitWriteLineType"), "localBuilder");
			}
			this.Emit(OpCodes.Callvirt, method2);
		}

		// Token: 0x0600496F RID: 18799 RVA: 0x000FF5E4 File Offset: 0x000FE5E4
		public virtual void EmitWriteLine(FieldInfo fld)
		{
			if (fld == null)
			{
				throw new ArgumentNullException("fld");
			}
			MethodInfo method = typeof(Console).GetMethod("get_Out");
			this.Emit(OpCodes.Call, method);
			if ((fld.Attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope)
			{
				this.Emit(OpCodes.Ldsfld, fld);
			}
			else
			{
				this.Emit(OpCodes.Ldarg, 0);
				this.Emit(OpCodes.Ldfld, fld);
			}
			Type[] array = new Type[1];
			object fieldType = fld.FieldType;
			if (fieldType is TypeBuilder || fieldType is EnumBuilder)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_OutputStreamUsingTypeBuilder"));
			}
			array[0] = (Type)fieldType;
			MethodInfo method2 = typeof(TextWriter).GetMethod("WriteLine", array);
			if (method2 == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmitWriteLineType"), "fld");
			}
			this.Emit(OpCodes.Callvirt, method2);
		}

		// Token: 0x06004970 RID: 18800 RVA: 0x000FF6C2 File Offset: 0x000FE6C2
		public virtual LocalBuilder DeclareLocal(Type localType)
		{
			return this.DeclareLocal(localType, false);
		}

		// Token: 0x06004971 RID: 18801 RVA: 0x000FF6CC File Offset: 0x000FE6CC
		public virtual LocalBuilder DeclareLocal(Type localType, bool pinned)
		{
			MethodBuilder methodBuilder = this.m_methodBuilder as MethodBuilder;
			if (methodBuilder == null)
			{
				throw new NotSupportedException();
			}
			if (methodBuilder.IsTypeCreated())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TypeHasBeenCreated"));
			}
			if (localType == null)
			{
				throw new ArgumentNullException("localType");
			}
			if (methodBuilder.m_bIsBaked)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MethodBaked"));
			}
			this.m_localSignature.AddArgument(localType, pinned);
			LocalBuilder result = new LocalBuilder(this.m_localCount, localType, methodBuilder, pinned);
			this.m_localCount++;
			return result;
		}

		// Token: 0x06004972 RID: 18802 RVA: 0x000FF758 File Offset: 0x000FE758
		public virtual void UsingNamespace(string usingNamespace)
		{
			if (usingNamespace == null)
			{
				throw new ArgumentNullException("usingNamespace");
			}
			if (usingNamespace.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "usingNamespace");
			}
			MethodBuilder methodBuilder = this.m_methodBuilder as MethodBuilder;
			if (methodBuilder == null)
			{
				throw new NotSupportedException();
			}
			int currentActiveScopeIndex = methodBuilder.GetILGenerator().m_ScopeTree.GetCurrentActiveScopeIndex();
			if (currentActiveScopeIndex == -1)
			{
				methodBuilder.m_localSymInfo.AddUsingNamespace(usingNamespace);
				return;
			}
			this.m_ScopeTree.AddUsingNamespaceToCurrentScope(usingNamespace);
		}

		// Token: 0x06004973 RID: 18803 RVA: 0x000FF7D3 File Offset: 0x000FE7D3
		public virtual void MarkSequencePoint(ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
		{
			if (startLine == 0 || startLine < 0 || endLine == 0 || endLine < 0)
			{
				throw new ArgumentOutOfRangeException("startLine");
			}
			this.m_LineNumberInfo.AddLineNumberInfo(document, this.m_length, startLine, startColumn, endLine, endColumn);
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x000FF808 File Offset: 0x000FE808
		public virtual void BeginScope()
		{
			this.m_ScopeTree.AddScopeInfo(ScopeAction.Open, this.m_length);
		}

		// Token: 0x06004975 RID: 18805 RVA: 0x000FF81C File Offset: 0x000FE81C
		public virtual void EndScope()
		{
			this.m_ScopeTree.AddScopeInfo(ScopeAction.Close, this.m_length);
		}

		// Token: 0x06004976 RID: 18806 RVA: 0x000FF830 File Offset: 0x000FE830
		void _ILGenerator.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x000FF837 File Offset: 0x000FE837
		void _ILGenerator.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004978 RID: 18808 RVA: 0x000FF83E File Offset: 0x000FE83E
		void _ILGenerator.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x000FF845 File Offset: 0x000FE845
		void _ILGenerator.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04002591 RID: 9617
		internal const byte PrefixInstruction = 255;

		// Token: 0x04002592 RID: 9618
		internal const int defaultSize = 16;

		// Token: 0x04002593 RID: 9619
		internal const int DefaultFixupArraySize = 64;

		// Token: 0x04002594 RID: 9620
		internal const int DefaultLabelArraySize = 16;

		// Token: 0x04002595 RID: 9621
		internal const int DefaultExceptionArraySize = 8;

		// Token: 0x04002596 RID: 9622
		internal int m_length;

		// Token: 0x04002597 RID: 9623
		internal byte[] m_ILStream;

		// Token: 0x04002598 RID: 9624
		internal int[] m_labelList;

		// Token: 0x04002599 RID: 9625
		internal int m_labelCount;

		// Token: 0x0400259A RID: 9626
		internal __FixupData[] m_fixupData;

		// Token: 0x0400259B RID: 9627
		internal int m_fixupCount;

		// Token: 0x0400259C RID: 9628
		internal int[] m_RVAFixupList;

		// Token: 0x0400259D RID: 9629
		internal int m_RVAFixupCount;

		// Token: 0x0400259E RID: 9630
		internal int[] m_RelocFixupList;

		// Token: 0x0400259F RID: 9631
		internal int m_RelocFixupCount;

		// Token: 0x040025A0 RID: 9632
		internal int m_exceptionCount;

		// Token: 0x040025A1 RID: 9633
		internal int m_currExcStackCount;

		// Token: 0x040025A2 RID: 9634
		internal __ExceptionInfo[] m_exceptions;

		// Token: 0x040025A3 RID: 9635
		internal __ExceptionInfo[] m_currExcStack;

		// Token: 0x040025A4 RID: 9636
		internal ScopeTree m_ScopeTree;

		// Token: 0x040025A5 RID: 9637
		internal LineNumberInfo m_LineNumberInfo;

		// Token: 0x040025A6 RID: 9638
		internal MethodInfo m_methodBuilder;

		// Token: 0x040025A7 RID: 9639
		internal int m_localCount;

		// Token: 0x040025A8 RID: 9640
		internal SignatureHelper m_localSignature;

		// Token: 0x040025A9 RID: 9641
		internal int m_maxStackSize;

		// Token: 0x040025AA RID: 9642
		internal int m_maxMidStack;

		// Token: 0x040025AB RID: 9643
		internal int m_maxMidStackCur;
	}
}

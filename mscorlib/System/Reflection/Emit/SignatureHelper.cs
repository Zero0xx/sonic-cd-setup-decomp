using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Reflection.Emit
{
	// Token: 0x02000847 RID: 2119
	[ComDefaultInterface(typeof(_SignatureHelper))]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	public sealed class SignatureHelper : _SignatureHelper
	{
		// Token: 0x06004C26 RID: 19494 RVA: 0x0010A8E0 File Offset: 0x001098E0
		public static SignatureHelper GetMethodSigHelper(Module mod, Type returnType, Type[] parameterTypes)
		{
			return SignatureHelper.GetMethodSigHelper(mod, CallingConventions.Standard, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x06004C27 RID: 19495 RVA: 0x0010A8F0 File Offset: 0x001098F0
		internal static SignatureHelper GetMethodSigHelper(Module mod, CallingConventions callingConvention, Type returnType, int cGenericParam)
		{
			return SignatureHelper.GetMethodSigHelper(mod, callingConvention, cGenericParam, returnType, null, null, null, null, null);
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x0010A90B File Offset: 0x0010990B
		public static SignatureHelper GetMethodSigHelper(Module mod, CallingConventions callingConvention, Type returnType)
		{
			return SignatureHelper.GetMethodSigHelper(mod, callingConvention, returnType, null, null, null, null, null);
		}

		// Token: 0x06004C29 RID: 19497 RVA: 0x0010A91C File Offset: 0x0010991C
		internal static SignatureHelper GetMethodSpecSigHelper(Module scope, Type[] inst)
		{
			SignatureHelper signatureHelper = new SignatureHelper(scope, 10);
			signatureHelper.AddData(inst.Length);
			foreach (Type clsArgument in inst)
			{
				signatureHelper.AddArgument(clsArgument);
			}
			return signatureHelper;
		}

		// Token: 0x06004C2A RID: 19498 RVA: 0x0010A958 File Offset: 0x00109958
		internal static SignatureHelper GetMethodSigHelper(Module scope, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			return SignatureHelper.GetMethodSigHelper(scope, callingConvention, 0, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
		}

		// Token: 0x06004C2B RID: 19499 RVA: 0x0010A978 File Offset: 0x00109978
		internal static SignatureHelper GetMethodSigHelper(Module scope, CallingConventions callingConvention, int cGenericParam, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			if (returnType == null)
			{
				returnType = typeof(void);
			}
			int num = 0;
			if ((callingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs)
			{
				num = 5;
			}
			if (cGenericParam > 0)
			{
				num |= 16;
			}
			if ((callingConvention & CallingConventions.HasThis) == CallingConventions.HasThis)
			{
				num |= 32;
			}
			SignatureHelper signatureHelper = new SignatureHelper(scope, num, cGenericParam, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers);
			signatureHelper.AddArguments(parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
			return signatureHelper;
		}

		// Token: 0x06004C2C RID: 19500 RVA: 0x0010A9D0 File Offset: 0x001099D0
		public static SignatureHelper GetMethodSigHelper(Module mod, CallingConvention unmanagedCallConv, Type returnType)
		{
			if (returnType == null)
			{
				returnType = typeof(void);
			}
			int callingConvention;
			if (unmanagedCallConv == CallingConvention.Cdecl)
			{
				callingConvention = 1;
			}
			else if (unmanagedCallConv == CallingConvention.StdCall || unmanagedCallConv == CallingConvention.Winapi)
			{
				callingConvention = 2;
			}
			else if (unmanagedCallConv == CallingConvention.ThisCall)
			{
				callingConvention = 3;
			}
			else
			{
				if (unmanagedCallConv != CallingConvention.FastCall)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_UnknownUnmanagedCallConv"), "unmanagedCallConv");
				}
				callingConvention = 4;
			}
			return new SignatureHelper(mod, callingConvention, returnType, null, null);
		}

		// Token: 0x06004C2D RID: 19501 RVA: 0x0010AA31 File Offset: 0x00109A31
		public static SignatureHelper GetLocalVarSigHelper()
		{
			return SignatureHelper.GetLocalVarSigHelper(null);
		}

		// Token: 0x06004C2E RID: 19502 RVA: 0x0010AA39 File Offset: 0x00109A39
		public static SignatureHelper GetMethodSigHelper(CallingConventions callingConvention, Type returnType)
		{
			return SignatureHelper.GetMethodSigHelper(null, callingConvention, returnType);
		}

		// Token: 0x06004C2F RID: 19503 RVA: 0x0010AA43 File Offset: 0x00109A43
		public static SignatureHelper GetMethodSigHelper(CallingConvention unmanagedCallingConvention, Type returnType)
		{
			return SignatureHelper.GetMethodSigHelper(null, unmanagedCallingConvention, returnType);
		}

		// Token: 0x06004C30 RID: 19504 RVA: 0x0010AA4D File Offset: 0x00109A4D
		public static SignatureHelper GetLocalVarSigHelper(Module mod)
		{
			return new SignatureHelper(mod, 7);
		}

		// Token: 0x06004C31 RID: 19505 RVA: 0x0010AA56 File Offset: 0x00109A56
		public static SignatureHelper GetFieldSigHelper(Module mod)
		{
			return new SignatureHelper(mod, 6);
		}

		// Token: 0x06004C32 RID: 19506 RVA: 0x0010AA5F File Offset: 0x00109A5F
		public static SignatureHelper GetPropertySigHelper(Module mod, Type returnType, Type[] parameterTypes)
		{
			return SignatureHelper.GetPropertySigHelper(mod, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x06004C33 RID: 19507 RVA: 0x0010AA6D File Offset: 0x00109A6D
		public static SignatureHelper GetPropertySigHelper(Module mod, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			return SignatureHelper.GetPropertySigHelper(mod, (CallingConventions)0, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
		}

		// Token: 0x06004C34 RID: 19508 RVA: 0x0010AA80 File Offset: 0x00109A80
		public static SignatureHelper GetPropertySigHelper(Module mod, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			if (returnType == null)
			{
				returnType = typeof(void);
			}
			int num = 8;
			if ((callingConvention & CallingConventions.HasThis) == CallingConventions.HasThis)
			{
				num |= 32;
			}
			SignatureHelper signatureHelper = new SignatureHelper(mod, num, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers);
			signatureHelper.AddArguments(parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
			return signatureHelper;
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x0010AAC4 File Offset: 0x00109AC4
		internal static SignatureHelper GetTypeSigToken(Module mod, Type type)
		{
			if (mod == null)
			{
				throw new ArgumentNullException("module");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return new SignatureHelper(mod, type);
		}

		// Token: 0x06004C36 RID: 19510 RVA: 0x0010AAE9 File Offset: 0x00109AE9
		private SignatureHelper(Module mod, int callingConvention)
		{
			this.Init(mod, callingConvention);
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x0010AAF9 File Offset: 0x00109AF9
		private SignatureHelper(Module mod, int callingConvention, int cGenericParameters, Type returnType, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers)
		{
			this.Init(mod, callingConvention, cGenericParameters);
			if (callingConvention == 6)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_BadFieldSig"));
			}
			this.AddOneArgTypeHelper(returnType, requiredCustomModifiers, optionalCustomModifiers);
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x0010AB2A File Offset: 0x00109B2A
		private SignatureHelper(Module mod, int callingConvention, Type returnType, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers) : this(mod, callingConvention, 0, returnType, requiredCustomModifiers, optionalCustomModifiers)
		{
		}

		// Token: 0x06004C39 RID: 19513 RVA: 0x0010AB3A File Offset: 0x00109B3A
		private SignatureHelper(Module mod, Type type)
		{
			this.Init(mod);
			this.AddOneArgTypeHelper(type);
		}

		// Token: 0x06004C3A RID: 19514 RVA: 0x0010AB50 File Offset: 0x00109B50
		private void Init(Module mod)
		{
			this.m_signature = new byte[32];
			this.m_currSig = 0;
			this.m_module = (mod as ModuleBuilder);
			this.m_argCount = 0;
			this.m_sigDone = false;
			this.m_sizeLoc = -1;
			if (this.m_module == null && mod != null)
			{
				throw new ArgumentException(Environment.GetResourceString("NotSupported_MustBeModuleBuilder"));
			}
		}

		// Token: 0x06004C3B RID: 19515 RVA: 0x0010ABAD File Offset: 0x00109BAD
		private void Init(Module mod, int callingConvention)
		{
			this.Init(mod, callingConvention, 0);
		}

		// Token: 0x06004C3C RID: 19516 RVA: 0x0010ABB8 File Offset: 0x00109BB8
		private void Init(Module mod, int callingConvention, int cGenericParam)
		{
			this.Init(mod);
			this.AddData(callingConvention);
			if (callingConvention == 6 || callingConvention == 10)
			{
				this.m_sizeLoc = -1;
				return;
			}
			if (cGenericParam > 0)
			{
				this.AddData(cGenericParam);
			}
			this.m_sizeLoc = this.m_currSig++;
		}

		// Token: 0x06004C3D RID: 19517 RVA: 0x0010AC06 File Offset: 0x00109C06
		private void AddOneArgTypeHelper(Type argument, bool pinned)
		{
			if (pinned)
			{
				this.AddElementType(69);
			}
			this.AddOneArgTypeHelper(argument);
		}

		// Token: 0x06004C3E RID: 19518 RVA: 0x0010AC1C File Offset: 0x00109C1C
		private void AddOneArgTypeHelper(Type clsArgument, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers)
		{
			if (optionalCustomModifiers != null)
			{
				for (int i = 0; i < optionalCustomModifiers.Length; i++)
				{
					this.AddElementType(32);
					this.AddToken(this.m_module.GetTypeToken(optionalCustomModifiers[i]).Token);
				}
			}
			if (requiredCustomModifiers != null)
			{
				for (int j = 0; j < requiredCustomModifiers.Length; j++)
				{
					this.AddElementType(31);
					this.AddToken(this.m_module.GetTypeToken(requiredCustomModifiers[j]).Token);
				}
			}
			this.AddOneArgTypeHelper(clsArgument);
		}

		// Token: 0x06004C3F RID: 19519 RVA: 0x0010AC9A File Offset: 0x00109C9A
		private void AddOneArgTypeHelper(Type clsArgument)
		{
			this.AddOneArgTypeHelperWorker(clsArgument, false);
		}

		// Token: 0x06004C40 RID: 19520 RVA: 0x0010ACA4 File Offset: 0x00109CA4
		private void AddOneArgTypeHelperWorker(Type clsArgument, bool lastWasGenericInst)
		{
			if (clsArgument.IsGenericParameter)
			{
				if (clsArgument.DeclaringMethod != null)
				{
					this.AddData(30);
				}
				else
				{
					this.AddData(19);
				}
				this.AddData(clsArgument.GenericParameterPosition);
				return;
			}
			if (clsArgument.IsGenericType && (!clsArgument.IsGenericTypeDefinition || !lastWasGenericInst))
			{
				this.AddElementType(21);
				this.AddOneArgTypeHelperWorker(clsArgument.GetGenericTypeDefinition(), true);
				Type[] genericArguments = clsArgument.GetGenericArguments();
				this.AddData(genericArguments.Length);
				foreach (Type clsArgument2 in genericArguments)
				{
					this.AddOneArgTypeHelper(clsArgument2);
				}
				return;
			}
			if (clsArgument is TypeBuilder)
			{
				TypeBuilder typeBuilder = (TypeBuilder)clsArgument;
				TypeToken typeToken;
				if (typeBuilder.Module.Equals(this.m_module))
				{
					typeToken = typeBuilder.TypeToken;
				}
				else
				{
					typeToken = this.m_module.GetTypeToken(clsArgument);
				}
				if (clsArgument.IsValueType)
				{
					this.InternalAddTypeToken(typeToken, 17);
					return;
				}
				this.InternalAddTypeToken(typeToken, 18);
				return;
			}
			else if (clsArgument is EnumBuilder)
			{
				TypeBuilder typeBuilder2 = ((EnumBuilder)clsArgument).m_typeBuilder;
				TypeToken typeToken2;
				if (typeBuilder2.Module.Equals(this.m_module))
				{
					typeToken2 = typeBuilder2.TypeToken;
				}
				else
				{
					typeToken2 = this.m_module.GetTypeToken(clsArgument);
				}
				if (clsArgument.IsValueType)
				{
					this.InternalAddTypeToken(typeToken2, 17);
					return;
				}
				this.InternalAddTypeToken(typeToken2, 18);
				return;
			}
			else
			{
				if (clsArgument.IsByRef)
				{
					this.AddElementType(16);
					clsArgument = clsArgument.GetElementType();
					this.AddOneArgTypeHelper(clsArgument);
					return;
				}
				if (clsArgument.IsPointer)
				{
					this.AddElementType(15);
					this.AddOneArgTypeHelper(clsArgument.GetElementType());
					return;
				}
				if (clsArgument.IsArray)
				{
					if (clsArgument.IsSzArray)
					{
						this.AddElementType(29);
						this.AddOneArgTypeHelper(clsArgument.GetElementType());
						return;
					}
					this.AddElementType(20);
					this.AddOneArgTypeHelper(clsArgument.GetElementType());
					this.AddData(clsArgument.GetArrayRank());
					this.AddData(0);
					this.AddData(0);
					return;
				}
				else
				{
					RuntimeType runtimeType = clsArgument as RuntimeType;
					int num = (runtimeType != null) ? SignatureHelper.GetCorElementTypeFromClass(runtimeType) : 34;
					if (SignatureHelper.IsSimpleType(num))
					{
						this.AddElementType(num);
						return;
					}
					if (clsArgument == typeof(object))
					{
						this.AddElementType(28);
						return;
					}
					if (clsArgument == typeof(string))
					{
						this.AddElementType(14);
						return;
					}
					if (this.m_module == null)
					{
						this.InternalAddRuntimeType(runtimeType);
						return;
					}
					if (clsArgument.IsValueType)
					{
						this.InternalAddTypeToken(this.m_module.GetTypeToken(clsArgument), 17);
						return;
					}
					this.InternalAddTypeToken(this.m_module.GetTypeToken(clsArgument), 18);
					return;
				}
			}
		}

		// Token: 0x06004C41 RID: 19521 RVA: 0x0010AF1C File Offset: 0x00109F1C
		private void AddData(int data)
		{
			if (this.m_currSig + 4 >= this.m_signature.Length)
			{
				this.m_signature = this.ExpandArray(this.m_signature);
			}
			if (data <= 127)
			{
				this.m_signature[this.m_currSig++] = (byte)(data & 255);
				return;
			}
			if (data <= 16383)
			{
				this.m_signature[this.m_currSig++] = (byte)(data >> 8 | 128);
				this.m_signature[this.m_currSig++] = (byte)(data & 255);
				return;
			}
			if (data <= 536870911)
			{
				this.m_signature[this.m_currSig++] = (byte)(data >> 24 | 192);
				this.m_signature[this.m_currSig++] = (byte)(data >> 16 & 255);
				this.m_signature[this.m_currSig++] = (byte)(data >> 8 & 255);
				this.m_signature[this.m_currSig++] = (byte)(data & 255);
				return;
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_LargeInteger"));
		}

		// Token: 0x06004C42 RID: 19522 RVA: 0x0010B068 File Offset: 0x0010A068
		private void AddData(uint data)
		{
			if (this.m_currSig + 4 >= this.m_signature.Length)
			{
				this.m_signature = this.ExpandArray(this.m_signature);
			}
			this.m_signature[this.m_currSig++] = (byte)(data & 255U);
			this.m_signature[this.m_currSig++] = (byte)(data >> 8 & 255U);
			this.m_signature[this.m_currSig++] = (byte)(data >> 16 & 255U);
			this.m_signature[this.m_currSig++] = (byte)(data >> 24 & 255U);
		}

		// Token: 0x06004C43 RID: 19523 RVA: 0x0010B124 File Offset: 0x0010A124
		private void AddData(ulong data)
		{
			if (this.m_currSig + 8 >= this.m_signature.Length)
			{
				this.m_signature = this.ExpandArray(this.m_signature);
			}
			this.m_signature[this.m_currSig++] = (byte)(data & 255UL);
			this.m_signature[this.m_currSig++] = (byte)(data >> 8 & 255UL);
			this.m_signature[this.m_currSig++] = (byte)(data >> 16 & 255UL);
			this.m_signature[this.m_currSig++] = (byte)(data >> 24 & 255UL);
			this.m_signature[this.m_currSig++] = (byte)(data >> 32 & 255UL);
			this.m_signature[this.m_currSig++] = (byte)(data >> 40 & 255UL);
			this.m_signature[this.m_currSig++] = (byte)(data >> 48 & 255UL);
			this.m_signature[this.m_currSig++] = (byte)(data >> 56 & 255UL);
		}

		// Token: 0x06004C44 RID: 19524 RVA: 0x0010B27C File Offset: 0x0010A27C
		private void AddElementType(int cvt)
		{
			if (this.m_currSig + 1 >= this.m_signature.Length)
			{
				this.m_signature = this.ExpandArray(this.m_signature);
			}
			this.m_signature[this.m_currSig++] = (byte)cvt;
		}

		// Token: 0x06004C45 RID: 19525 RVA: 0x0010B2C8 File Offset: 0x0010A2C8
		private void AddToken(int token)
		{
			int num = token & 16777215;
			int num2 = token & -16777216;
			if (num > 67108863)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_LargeInteger"));
			}
			num <<= 2;
			if (num2 == 16777216)
			{
				num |= 1;
			}
			else if (num2 == 553648128)
			{
				num |= 2;
			}
			this.AddData(num);
		}

		// Token: 0x06004C46 RID: 19526 RVA: 0x0010B322 File Offset: 0x0010A322
		private void InternalAddTypeToken(TypeToken clsToken, int CorType)
		{
			this.AddElementType(CorType);
			this.AddToken(clsToken.Token);
		}

		// Token: 0x06004C47 RID: 19527 RVA: 0x0010B338 File Offset: 0x0010A338
		private unsafe void InternalAddRuntimeType(Type type)
		{
			this.AddElementType(33);
			void* ptr = (void*)type.GetTypeHandleInternal().Value;
			if (sizeof(void*) == 4)
			{
				this.AddData(ptr);
				return;
			}
			this.AddData(ptr);
		}

		// Token: 0x06004C48 RID: 19528 RVA: 0x0010B37B File Offset: 0x0010A37B
		private byte[] ExpandArray(byte[] inArray)
		{
			return this.ExpandArray(inArray, inArray.Length * 2);
		}

		// Token: 0x06004C49 RID: 19529 RVA: 0x0010B38C File Offset: 0x0010A38C
		private byte[] ExpandArray(byte[] inArray, int requiredLength)
		{
			if (requiredLength < inArray.Length)
			{
				requiredLength = inArray.Length * 2;
			}
			byte[] array = new byte[requiredLength];
			Array.Copy(inArray, array, inArray.Length);
			return array;
		}

		// Token: 0x06004C4A RID: 19530 RVA: 0x0010B3B8 File Offset: 0x0010A3B8
		private void IncrementArgCounts()
		{
			if (this.m_sizeLoc == -1)
			{
				return;
			}
			this.m_argCount++;
		}

		// Token: 0x06004C4B RID: 19531 RVA: 0x0010B3D4 File Offset: 0x0010A3D4
		private void SetNumberOfSignatureElements(bool forceCopy)
		{
			int currSig = this.m_currSig;
			if (this.m_sizeLoc == -1)
			{
				return;
			}
			if (this.m_argCount < 128 && !forceCopy)
			{
				this.m_signature[this.m_sizeLoc] = (byte)this.m_argCount;
				return;
			}
			int num;
			if (this.m_argCount < 127)
			{
				num = 1;
			}
			else if (this.m_argCount < 16383)
			{
				num = 2;
			}
			else
			{
				num = 4;
			}
			byte[] array = new byte[this.m_currSig + num - 1];
			array[0] = this.m_signature[0];
			Array.Copy(this.m_signature, this.m_sizeLoc + 1, array, this.m_sizeLoc + num, currSig - (this.m_sizeLoc + 1));
			this.m_signature = array;
			this.m_currSig = this.m_sizeLoc;
			this.AddData(this.m_argCount);
			this.m_currSig = currSig + (num - 1);
		}

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x06004C4C RID: 19532 RVA: 0x0010B4A3 File Offset: 0x0010A4A3
		internal int ArgumentCount
		{
			get
			{
				return this.m_argCount;
			}
		}

		// Token: 0x06004C4D RID: 19533 RVA: 0x0010B4AB File Offset: 0x0010A4AB
		internal static bool IsSimpleType(int type)
		{
			return type <= 14 || (type == 22 || type == 24 || type == 25 || type == 28);
		}

		// Token: 0x06004C4E RID: 19534 RVA: 0x0010B4CB File Offset: 0x0010A4CB
		internal byte[] InternalGetSignature(out int length)
		{
			if (!this.m_sigDone)
			{
				this.m_sigDone = true;
				this.SetNumberOfSignatureElements(false);
			}
			length = this.m_currSig;
			return this.m_signature;
		}

		// Token: 0x06004C4F RID: 19535 RVA: 0x0010B4F4 File Offset: 0x0010A4F4
		internal byte[] InternalGetSignatureArray()
		{
			int argCount = this.m_argCount;
			int currSig = this.m_currSig;
			int num = currSig;
			if (argCount < 127)
			{
				num++;
			}
			else if (argCount < 16383)
			{
				num += 2;
			}
			else
			{
				num += 4;
			}
			byte[] array = new byte[num];
			int destinationIndex = 0;
			array[destinationIndex++] = this.m_signature[0];
			if (argCount <= 127)
			{
				array[destinationIndex++] = (byte)(argCount & 255);
			}
			else if (argCount <= 16383)
			{
				array[destinationIndex++] = (byte)(argCount >> 8 | 128);
				array[destinationIndex++] = (byte)(argCount & 255);
			}
			else
			{
				if (argCount > 536870911)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_LargeInteger"));
				}
				array[destinationIndex++] = (byte)(argCount >> 24 | 192);
				array[destinationIndex++] = (byte)(argCount >> 16 & 255);
				array[destinationIndex++] = (byte)(argCount >> 8 & 255);
				array[destinationIndex++] = (byte)(argCount & 255);
			}
			Array.Copy(this.m_signature, 2, array, destinationIndex, currSig - 2);
			array[num - 1] = 0;
			return array;
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x0010B611 File Offset: 0x0010A611
		public void AddArgument(Type clsArgument)
		{
			this.AddArgument(clsArgument, null, null);
		}

		// Token: 0x06004C51 RID: 19537 RVA: 0x0010B61C File Offset: 0x0010A61C
		public void AddArgument(Type argument, bool pinned)
		{
			if (argument == null)
			{
				throw new ArgumentNullException("argument");
			}
			this.IncrementArgCounts();
			this.AddOneArgTypeHelper(argument, pinned);
		}

		// Token: 0x06004C52 RID: 19538 RVA: 0x0010B63C File Offset: 0x0010A63C
		public void AddArguments(Type[] arguments, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
		{
			if (requiredCustomModifiers != null && (arguments == null || requiredCustomModifiers.Length != arguments.Length))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MismatchedArrays", new object[]
				{
					"requiredCustomModifiers",
					"arguments"
				}));
			}
			if (optionalCustomModifiers != null && (arguments == null || optionalCustomModifiers.Length != arguments.Length))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MismatchedArrays", new object[]
				{
					"optionalCustomModifiers",
					"arguments"
				}));
			}
			if (arguments != null)
			{
				for (int i = 0; i < arguments.Length; i++)
				{
					this.AddArgument(arguments[i], (requiredCustomModifiers == null) ? null : requiredCustomModifiers[i], (optionalCustomModifiers == null) ? null : optionalCustomModifiers[i]);
				}
			}
		}

		// Token: 0x06004C53 RID: 19539 RVA: 0x0010B6E4 File Offset: 0x0010A6E4
		public void AddArgument(Type argument, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers)
		{
			if (this.m_sigDone)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_SigIsFinalized"));
			}
			if (argument == null)
			{
				throw new ArgumentNullException("argument");
			}
			if (requiredCustomModifiers != null)
			{
				foreach (Type type in requiredCustomModifiers)
				{
					if (type == null)
					{
						throw new ArgumentNullException("requiredCustomModifiers");
					}
					if (type.HasElementType)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_ArraysInvalid"), "requiredCustomModifiers");
					}
					if (type.ContainsGenericParameters)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_GenericsInvalid"), "requiredCustomModifiers");
					}
				}
			}
			if (optionalCustomModifiers != null)
			{
				foreach (Type type2 in optionalCustomModifiers)
				{
					if (type2 == null)
					{
						throw new ArgumentNullException("optionalCustomModifiers");
					}
					if (type2.HasElementType)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_ArraysInvalid"), "optionalCustomModifiers");
					}
					if (type2.ContainsGenericParameters)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_GenericsInvalid"), "optionalCustomModifiers");
					}
				}
			}
			this.IncrementArgCounts();
			this.AddOneArgTypeHelper(argument, requiredCustomModifiers, optionalCustomModifiers);
		}

		// Token: 0x06004C54 RID: 19540 RVA: 0x0010B7E0 File Offset: 0x0010A7E0
		public void AddSentinel()
		{
			this.AddElementType(65);
		}

		// Token: 0x06004C55 RID: 19541 RVA: 0x0010B7EC File Offset: 0x0010A7EC
		public override bool Equals(object obj)
		{
			if (!(obj is SignatureHelper))
			{
				return false;
			}
			SignatureHelper signatureHelper = (SignatureHelper)obj;
			if (!signatureHelper.m_module.Equals(this.m_module) || signatureHelper.m_currSig != this.m_currSig || signatureHelper.m_sizeLoc != this.m_sizeLoc || signatureHelper.m_sigDone != this.m_sigDone)
			{
				return false;
			}
			for (int i = 0; i < this.m_currSig; i++)
			{
				if (this.m_signature[i] != signatureHelper.m_signature[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x0010B870 File Offset: 0x0010A870
		public override int GetHashCode()
		{
			int num = this.m_module.GetHashCode() + this.m_currSig + this.m_sizeLoc;
			if (this.m_sigDone)
			{
				num++;
			}
			for (int i = 0; i < this.m_currSig; i++)
			{
				num += this.m_signature[i].GetHashCode();
			}
			return num;
		}

		// Token: 0x06004C57 RID: 19543 RVA: 0x0010B8C9 File Offset: 0x0010A8C9
		public byte[] GetSignature()
		{
			return this.GetSignature(false);
		}

		// Token: 0x06004C58 RID: 19544 RVA: 0x0010B8D4 File Offset: 0x0010A8D4
		internal byte[] GetSignature(bool appendEndOfSig)
		{
			if (!this.m_sigDone)
			{
				if (appendEndOfSig)
				{
					this.AddElementType(0);
				}
				this.SetNumberOfSignatureElements(true);
				this.m_sigDone = true;
			}
			if (this.m_signature.Length > this.m_currSig)
			{
				byte[] array = new byte[this.m_currSig];
				Array.Copy(this.m_signature, array, this.m_currSig);
				this.m_signature = array;
			}
			return this.m_signature;
		}

		// Token: 0x06004C59 RID: 19545 RVA: 0x0010B93C File Offset: 0x0010A93C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Length: " + this.m_currSig + Environment.NewLine);
			if (this.m_sizeLoc != -1)
			{
				stringBuilder.Append("Arguments: " + this.m_signature[this.m_sizeLoc] + Environment.NewLine);
			}
			else
			{
				stringBuilder.Append("Field Signature" + Environment.NewLine);
			}
			stringBuilder.Append("Signature: " + Environment.NewLine);
			for (int i = 0; i <= this.m_currSig; i++)
			{
				stringBuilder.Append(this.m_signature[i] + "  ");
			}
			stringBuilder.Append(Environment.NewLine);
			return stringBuilder.ToString();
		}

		// Token: 0x06004C5A RID: 19546
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetCorElementTypeFromClass(RuntimeType cls);

		// Token: 0x06004C5B RID: 19547 RVA: 0x0010BA10 File Offset: 0x0010AA10
		void _SignatureHelper.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C5C RID: 19548 RVA: 0x0010BA17 File Offset: 0x0010AA17
		void _SignatureHelper.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C5D RID: 19549 RVA: 0x0010BA1E File Offset: 0x0010AA1E
		void _SignatureHelper.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004C5E RID: 19550 RVA: 0x0010BA25 File Offset: 0x0010AA25
		void _SignatureHelper.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040027DC RID: 10204
		internal const int mdtTypeRef = 16777216;

		// Token: 0x040027DD RID: 10205
		internal const int mdtTypeDef = 33554432;

		// Token: 0x040027DE RID: 10206
		internal const int mdtTypeSpec = 553648128;

		// Token: 0x040027DF RID: 10207
		internal const byte ELEMENT_TYPE_END = 0;

		// Token: 0x040027E0 RID: 10208
		internal const byte ELEMENT_TYPE_VOID = 1;

		// Token: 0x040027E1 RID: 10209
		internal const byte ELEMENT_TYPE_BOOLEAN = 2;

		// Token: 0x040027E2 RID: 10210
		internal const byte ELEMENT_TYPE_CHAR = 3;

		// Token: 0x040027E3 RID: 10211
		internal const byte ELEMENT_TYPE_I1 = 4;

		// Token: 0x040027E4 RID: 10212
		internal const byte ELEMENT_TYPE_U1 = 5;

		// Token: 0x040027E5 RID: 10213
		internal const byte ELEMENT_TYPE_I2 = 6;

		// Token: 0x040027E6 RID: 10214
		internal const byte ELEMENT_TYPE_U2 = 7;

		// Token: 0x040027E7 RID: 10215
		internal const byte ELEMENT_TYPE_I4 = 8;

		// Token: 0x040027E8 RID: 10216
		internal const byte ELEMENT_TYPE_U4 = 9;

		// Token: 0x040027E9 RID: 10217
		internal const byte ELEMENT_TYPE_I8 = 10;

		// Token: 0x040027EA RID: 10218
		internal const byte ELEMENT_TYPE_U8 = 11;

		// Token: 0x040027EB RID: 10219
		internal const byte ELEMENT_TYPE_R4 = 12;

		// Token: 0x040027EC RID: 10220
		internal const byte ELEMENT_TYPE_R8 = 13;

		// Token: 0x040027ED RID: 10221
		internal const byte ELEMENT_TYPE_STRING = 14;

		// Token: 0x040027EE RID: 10222
		internal const byte ELEMENT_TYPE_PTR = 15;

		// Token: 0x040027EF RID: 10223
		internal const byte ELEMENT_TYPE_BYREF = 16;

		// Token: 0x040027F0 RID: 10224
		internal const byte ELEMENT_TYPE_VALUETYPE = 17;

		// Token: 0x040027F1 RID: 10225
		internal const byte ELEMENT_TYPE_CLASS = 18;

		// Token: 0x040027F2 RID: 10226
		internal const byte ELEMENT_TYPE_VAR = 19;

		// Token: 0x040027F3 RID: 10227
		internal const byte ELEMENT_TYPE_ARRAY = 20;

		// Token: 0x040027F4 RID: 10228
		internal const byte ELEMENT_TYPE_GENERICINST = 21;

		// Token: 0x040027F5 RID: 10229
		internal const byte ELEMENT_TYPE_TYPEDBYREF = 22;

		// Token: 0x040027F6 RID: 10230
		internal const byte ELEMENT_TYPE_I = 24;

		// Token: 0x040027F7 RID: 10231
		internal const byte ELEMENT_TYPE_U = 25;

		// Token: 0x040027F8 RID: 10232
		internal const byte ELEMENT_TYPE_FNPTR = 27;

		// Token: 0x040027F9 RID: 10233
		internal const byte ELEMENT_TYPE_OBJECT = 28;

		// Token: 0x040027FA RID: 10234
		internal const byte ELEMENT_TYPE_SZARRAY = 29;

		// Token: 0x040027FB RID: 10235
		internal const byte ELEMENT_TYPE_MVAR = 30;

		// Token: 0x040027FC RID: 10236
		internal const byte ELEMENT_TYPE_CMOD_REQD = 31;

		// Token: 0x040027FD RID: 10237
		internal const byte ELEMENT_TYPE_CMOD_OPT = 32;

		// Token: 0x040027FE RID: 10238
		internal const byte ELEMENT_TYPE_INTERNAL = 33;

		// Token: 0x040027FF RID: 10239
		internal const byte ELEMENT_TYPE_MAX = 34;

		// Token: 0x04002800 RID: 10240
		internal const byte ELEMENT_TYPE_SENTINEL = 65;

		// Token: 0x04002801 RID: 10241
		internal const byte ELEMENT_TYPE_PINNED = 69;

		// Token: 0x04002802 RID: 10242
		internal const int IMAGE_CEE_UNMANAGED_CALLCONV_C = 1;

		// Token: 0x04002803 RID: 10243
		internal const int IMAGE_CEE_UNMANAGED_CALLCONV_STDCALL = 2;

		// Token: 0x04002804 RID: 10244
		internal const int IMAGE_CEE_UNMANAGED_CALLCONV_THISCALL = 3;

		// Token: 0x04002805 RID: 10245
		internal const int IMAGE_CEE_UNMANAGED_CALLCONV_FASTCALL = 4;

		// Token: 0x04002806 RID: 10246
		internal const int IMAGE_CEE_CS_CALLCONV_DEFAULT = 0;

		// Token: 0x04002807 RID: 10247
		internal const int IMAGE_CEE_CS_CALLCONV_VARARG = 5;

		// Token: 0x04002808 RID: 10248
		internal const int IMAGE_CEE_CS_CALLCONV_FIELD = 6;

		// Token: 0x04002809 RID: 10249
		internal const int IMAGE_CEE_CS_CALLCONV_LOCAL_SIG = 7;

		// Token: 0x0400280A RID: 10250
		internal const int IMAGE_CEE_CS_CALLCONV_PROPERTY = 8;

		// Token: 0x0400280B RID: 10251
		internal const int IMAGE_CEE_CS_CALLCONV_UNMGD = 9;

		// Token: 0x0400280C RID: 10252
		internal const int IMAGE_CEE_CS_CALLCONV_GENERICINST = 10;

		// Token: 0x0400280D RID: 10253
		internal const int IMAGE_CEE_CS_CALLCONV_MAX = 11;

		// Token: 0x0400280E RID: 10254
		internal const int IMAGE_CEE_CS_CALLCONV_MASK = 15;

		// Token: 0x0400280F RID: 10255
		internal const int IMAGE_CEE_CS_CALLCONV_GENERIC = 16;

		// Token: 0x04002810 RID: 10256
		internal const int IMAGE_CEE_CS_CALLCONV_HASTHIS = 32;

		// Token: 0x04002811 RID: 10257
		internal const int IMAGE_CEE_CS_CALLCONV_RETPARAM = 64;

		// Token: 0x04002812 RID: 10258
		internal const int NO_SIZE_IN_SIG = -1;

		// Token: 0x04002813 RID: 10259
		private byte[] m_signature;

		// Token: 0x04002814 RID: 10260
		private int m_currSig;

		// Token: 0x04002815 RID: 10261
		private int m_sizeLoc;

		// Token: 0x04002816 RID: 10262
		private ModuleBuilder m_module;

		// Token: 0x04002817 RID: 10263
		private bool m_sigDone;

		// Token: 0x04002818 RID: 10264
		private int m_argCount;
	}
}

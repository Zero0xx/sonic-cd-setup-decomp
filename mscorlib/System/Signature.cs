using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x0200010F RID: 271
	internal class Signature
	{
		// Token: 0x06000FB9 RID: 4025 RVA: 0x0002D415 File Offset: 0x0002C415
		public static implicit operator SignatureStruct(Signature pThis)
		{
			return pThis.m_signature;
		}

		// Token: 0x06000FBA RID: 4026
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetSignature(ref SignatureStruct signature, void* pCorSig, int cCorSig, IntPtr fieldHandle, IntPtr methodHandle, IntPtr declaringTypeHandle);

		// Token: 0x06000FBB RID: 4027 RVA: 0x0002D41D File Offset: 0x0002C41D
		private unsafe static void GetSignature(ref SignatureStruct signature, void* pCorSig, int cCorSig, RuntimeFieldHandle fieldHandle, RuntimeMethodHandle methodHandle, RuntimeTypeHandle declaringTypeHandle)
		{
			Signature._GetSignature(ref signature, pCorSig, cCorSig, fieldHandle.Value, methodHandle.Value, declaringTypeHandle.Value);
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0002D43C File Offset: 0x0002C43C
		internal static void GetSignatureForDynamicMethod(ref SignatureStruct signature, RuntimeMethodHandle methodHandle)
		{
			Signature._GetSignature(ref signature, null, 0, (IntPtr)0, methodHandle.Value, (IntPtr)0);
		}

		// Token: 0x06000FBD RID: 4029
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetCustomModifiers(ref SignatureStruct signature, int parameter, out RuntimeTypeHandle[] required, out RuntimeTypeHandle[] optional);

		// Token: 0x06000FBE RID: 4030
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CompareSig(ref SignatureStruct left, RuntimeTypeHandle typeLeft, ref SignatureStruct right, RuntimeTypeHandle typeRight);

		// Token: 0x06000FBF RID: 4031 RVA: 0x0002D45C File Offset: 0x0002C45C
		public Signature(RuntimeMethodHandle method, RuntimeTypeHandle[] arguments, RuntimeTypeHandle returnType, CallingConventions callingConvention)
		{
			SignatureStruct signature = new SignatureStruct(method, arguments, returnType, callingConvention);
			Signature.GetSignatureForDynamicMethod(ref signature, method);
			this.m_signature = signature;
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0002D490 File Offset: 0x0002C490
		public Signature(RuntimeMethodHandle methodHandle, RuntimeTypeHandle declaringTypeHandle)
		{
			SignatureStruct signature = default(SignatureStruct);
			Signature.GetSignature(ref signature, null, 0, new RuntimeFieldHandle(null), methodHandle, declaringTypeHandle);
			this.m_signature = signature;
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0002D4C8 File Offset: 0x0002C4C8
		public Signature(RuntimeFieldHandle fieldHandle, RuntimeTypeHandle declaringTypeHandle)
		{
			SignatureStruct signature = default(SignatureStruct);
			Signature.GetSignature(ref signature, null, 0, fieldHandle, new RuntimeMethodHandle(null), declaringTypeHandle);
			this.m_signature = signature;
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0002D500 File Offset: 0x0002C500
		public unsafe Signature(void* pCorSig, int cCorSig, RuntimeTypeHandle declaringTypeHandle)
		{
			SignatureStruct signature = default(SignatureStruct);
			Signature.GetSignature(ref signature, pCorSig, cCorSig, new RuntimeFieldHandle(null), new RuntimeMethodHandle(null), declaringTypeHandle);
			this.m_signature = signature;
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0002D53A File Offset: 0x0002C53A
		internal CallingConventions CallingConvention
		{
			get
			{
				return this.m_signature.m_managedCallingConvention & (CallingConventions)255;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x0002D54D File Offset: 0x0002C54D
		internal RuntimeTypeHandle[] Arguments
		{
			get
			{
				return this.m_signature.m_arguments;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x0002D55A File Offset: 0x0002C55A
		internal RuntimeTypeHandle ReturnTypeHandle
		{
			get
			{
				return this.m_signature.m_returnTypeORfieldType;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x0002D567 File Offset: 0x0002C567
		internal RuntimeTypeHandle FieldTypeHandle
		{
			get
			{
				return this.m_signature.m_returnTypeORfieldType;
			}
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0002D574 File Offset: 0x0002C574
		internal static bool DiffSigs(Signature sig1, RuntimeTypeHandle typeHandle1, Signature sig2, RuntimeTypeHandle typeHandle2)
		{
			SignatureStruct signatureStruct = sig1;
			SignatureStruct signatureStruct2 = sig2;
			return Signature.CompareSig(ref signatureStruct, typeHandle1, ref signatureStruct2, typeHandle2);
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0002D59C File Offset: 0x0002C59C
		public Type[] GetCustomModifiers(int position, bool required)
		{
			RuntimeTypeHandle[] array = null;
			RuntimeTypeHandle[] array2 = null;
			SignatureStruct signatureStruct = this;
			Signature.GetCustomModifiers(ref signatureStruct, position, out array, out array2);
			Type[] array3 = new Type[required ? array.Length : array2.Length];
			if (required)
			{
				for (int i = 0; i < array3.Length; i++)
				{
					array3[i] = array[i].GetRuntimeType();
				}
			}
			else
			{
				for (int j = 0; j < array3.Length; j++)
				{
					array3[j] = array2[j].GetRuntimeType();
				}
			}
			return array3;
		}

		// Token: 0x0400053F RID: 1343
		internal SignatureStruct m_signature;

		// Token: 0x02000110 RID: 272
		internal enum MdSigCallingConvention : byte
		{
			// Token: 0x04000541 RID: 1345
			Generics = 16,
			// Token: 0x04000542 RID: 1346
			HasThis = 32,
			// Token: 0x04000543 RID: 1347
			ExplicitThis = 64,
			// Token: 0x04000544 RID: 1348
			CallConvMask = 15,
			// Token: 0x04000545 RID: 1349
			Default = 0,
			// Token: 0x04000546 RID: 1350
			C,
			// Token: 0x04000547 RID: 1351
			StdCall,
			// Token: 0x04000548 RID: 1352
			ThisCall,
			// Token: 0x04000549 RID: 1353
			FastCall,
			// Token: 0x0400054A RID: 1354
			Vararg,
			// Token: 0x0400054B RID: 1355
			Field,
			// Token: 0x0400054C RID: 1356
			LocalSig,
			// Token: 0x0400054D RID: 1357
			Property,
			// Token: 0x0400054E RID: 1358
			Unmgd,
			// Token: 0x0400054F RID: 1359
			GenericInst,
			// Token: 0x04000550 RID: 1360
			Max
		}
	}
}

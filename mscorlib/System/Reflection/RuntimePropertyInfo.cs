using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x02000355 RID: 853
	[Serializable]
	internal sealed class RuntimePropertyInfo : PropertyInfo, ISerializable
	{
		// Token: 0x06002142 RID: 8514 RVA: 0x00052E58 File Offset: 0x00051E58
		internal unsafe RuntimePropertyInfo(int tkProperty, RuntimeType declaredType, RuntimeType.RuntimeTypeCache reflectedTypeCache, out bool isPrivate)
		{
			MetadataImport metadataImport = declaredType.Module.MetadataImport;
			this.m_token = tkProperty;
			this.m_reflectedTypeCache = reflectedTypeCache;
			this.m_declaringType = declaredType;
			RuntimeTypeHandle typeHandleInternal = declaredType.GetTypeHandleInternal();
			RuntimeTypeHandle runtimeTypeHandle = reflectedTypeCache.RuntimeTypeHandle;
			metadataImport.GetPropertyProps(tkProperty, out this.m_utf8name, out this.m_flags, out MetadataArgs.Skip.ConstArray);
			int associatesCount = metadataImport.GetAssociatesCount(tkProperty);
			AssociateRecord* ptr = stackalloc AssociateRecord[sizeof(AssociateRecord) * associatesCount];
			metadataImport.GetAssociates(tkProperty, ptr, associatesCount);
			RuntimeMethodInfo runtimeMethodInfo;
			Associates.AssignAssociates(ptr, associatesCount, typeHandleInternal, runtimeTypeHandle, out runtimeMethodInfo, out runtimeMethodInfo, out runtimeMethodInfo, out this.m_getterMethod, out this.m_setterMethod, out this.m_otherMethod, out isPrivate, out this.m_bindingFlags);
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x00052F08 File Offset: 0x00051F08
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override bool CacheEquals(object o)
		{
			RuntimePropertyInfo runtimePropertyInfo = o as RuntimePropertyInfo;
			return runtimePropertyInfo != null && runtimePropertyInfo.m_token == this.m_token && this.m_declaringType.GetTypeHandleInternal().GetModuleHandle().Equals(runtimePropertyInfo.m_declaringType.GetTypeHandleInternal().GetModuleHandle());
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06002144 RID: 8516 RVA: 0x00052F60 File Offset: 0x00051F60
		internal unsafe Signature Signature
		{
			get
			{
				if (this.m_signature == null)
				{
					void* ptr;
					ConstArray constArray;
					this.Module.MetadataImport.GetPropertyProps(this.m_token, out ptr, out MetadataArgs.Skip.PropertyAttributes, out constArray);
					this.m_signature = new Signature(constArray.Signature.ToPointer(), constArray.Length, this.m_declaringType.GetTypeHandleInternal());
				}
				return this.m_signature;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06002145 RID: 8517 RVA: 0x00052FCE File Offset: 0x00051FCE
		internal BindingFlags BindingFlags
		{
			get
			{
				return this.m_bindingFlags;
			}
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x00052FD6 File Offset: 0x00051FD6
		internal bool EqualsSig(RuntimePropertyInfo target)
		{
			return Signature.DiffSigs(this.Signature, this.DeclaringType.GetTypeHandleInternal(), target.Signature, target.DeclaringType.GetTypeHandleInternal());
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x00053000 File Offset: 0x00052000
		public override string ToString()
		{
			string text = this.PropertyType.SigToString() + " " + this.Name;
			RuntimeTypeHandle[] arguments = this.Signature.Arguments;
			if (arguments.Length > 0)
			{
				Type[] array = new Type[arguments.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = arguments[i].GetRuntimeType();
				}
				text = text + " [" + RuntimeMethodInfo.ConstructParameters(array, this.Signature.CallingConvention) + "]";
			}
			return text;
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x00053084 File Offset: 0x00052084
		public override object[] GetCustomAttributes(bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType);
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x0005309C File Offset: 0x0005209C
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.GetCustomAttributes(this, runtimeType);
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x000530E4 File Offset: 0x000520E4
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.IsDefined(this, runtimeType);
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x0600214B RID: 8523 RVA: 0x0005312A File Offset: 0x0005212A
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Property;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x0600214C RID: 8524 RVA: 0x00053130 File Offset: 0x00052130
		public override string Name
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = new Utf8String(this.m_utf8name).ToString();
				}
				return this.m_name;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x0600214D RID: 8525 RVA: 0x0005316A File Offset: 0x0005216A
		public override Type DeclaringType
		{
			get
			{
				return this.m_declaringType;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x0600214E RID: 8526 RVA: 0x00053172 File Offset: 0x00052172
		public override Type ReflectedType
		{
			get
			{
				return this.m_reflectedTypeCache.RuntimeType;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x0600214F RID: 8527 RVA: 0x0005317F File Offset: 0x0005217F
		public override int MetadataToken
		{
			get
			{
				return this.m_token;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06002150 RID: 8528 RVA: 0x00053187 File Offset: 0x00052187
		public override Module Module
		{
			get
			{
				return this.m_declaringType.Module;
			}
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x00053194 File Offset: 0x00052194
		public override Type[] GetRequiredCustomModifiers()
		{
			return this.Signature.GetCustomModifiers(0, true);
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x000531A3 File Offset: 0x000521A3
		public override Type[] GetOptionalCustomModifiers()
		{
			return this.Signature.GetCustomModifiers(0, false);
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x000531B4 File Offset: 0x000521B4
		internal object GetConstantValue(bool raw)
		{
			object value = MdConstant.GetValue(this.Module.MetadataImport, this.m_token, this.PropertyType.GetTypeHandleInternal(), raw);
			if (value == DBNull.Value)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_EnumLitValueNotFound"));
			}
			return value;
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x000531FD File Offset: 0x000521FD
		public override object GetConstantValue()
		{
			return this.GetConstantValue(false);
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x00053206 File Offset: 0x00052206
		public override object GetRawConstantValue()
		{
			return this.GetConstantValue(true);
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x00053210 File Offset: 0x00052210
		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			ArrayList arrayList = new ArrayList();
			if (Associates.IncludeAccessor(this.m_getterMethod, nonPublic))
			{
				arrayList.Add(this.m_getterMethod);
			}
			if (Associates.IncludeAccessor(this.m_setterMethod, nonPublic))
			{
				arrayList.Add(this.m_setterMethod);
			}
			if (this.m_otherMethod != null)
			{
				for (int i = 0; i < this.m_otherMethod.Length; i++)
				{
					if (Associates.IncludeAccessor(this.m_otherMethod[i], nonPublic))
					{
						arrayList.Add(this.m_otherMethod[i]);
					}
				}
			}
			return arrayList.ToArray(typeof(MethodInfo)) as MethodInfo[];
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06002157 RID: 8535 RVA: 0x000532A8 File Offset: 0x000522A8
		public override Type PropertyType
		{
			get
			{
				return this.Signature.ReturnTypeHandle.GetRuntimeType();
			}
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x000532C8 File Offset: 0x000522C8
		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			if (!Associates.IncludeAccessor(this.m_getterMethod, nonPublic))
			{
				return null;
			}
			return this.m_getterMethod;
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x000532E0 File Offset: 0x000522E0
		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			if (!Associates.IncludeAccessor(this.m_setterMethod, nonPublic))
			{
				return null;
			}
			return this.m_setterMethod;
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x000532F8 File Offset: 0x000522F8
		public override ParameterInfo[] GetIndexParameters()
		{
			int num = 0;
			ParameterInfo[] array = null;
			MethodInfo methodInfo = this.GetGetMethod(true);
			if (methodInfo != null)
			{
				array = methodInfo.GetParametersNoCopy();
				num = array.Length;
			}
			else
			{
				methodInfo = this.GetSetMethod(true);
				if (methodInfo != null)
				{
					array = methodInfo.GetParametersNoCopy();
					num = array.Length - 1;
				}
			}
			if (array != null && array.Length == 0)
			{
				return array;
			}
			ParameterInfo[] array2 = new ParameterInfo[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = new ParameterInfo(array[i], this);
			}
			return array2;
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x0600215B RID: 8539 RVA: 0x00053369 File Offset: 0x00052369
		public override PropertyAttributes Attributes
		{
			get
			{
				return this.m_flags;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x0600215C RID: 8540 RVA: 0x00053371 File Offset: 0x00052371
		public override bool CanRead
		{
			get
			{
				return this.m_getterMethod != null;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x0600215D RID: 8541 RVA: 0x0005337F File Offset: 0x0005237F
		public override bool CanWrite
		{
			get
			{
				return this.m_setterMethod != null;
			}
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x0005338D File Offset: 0x0005238D
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object GetValue(object obj, object[] index)
		{
			return this.GetValue(obj, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, index, null);
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x0005339C File Offset: 0x0005239C
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			MethodInfo getMethod = this.GetGetMethod(true);
			if (getMethod == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_GetMethNotFnd"));
			}
			return getMethod.Invoke(obj, invokeAttr, binder, index, null);
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x000533D0 File Offset: 0x000523D0
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override void SetValue(object obj, object value, object[] index)
		{
			this.SetValue(obj, value, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, index, null);
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x000533E0 File Offset: 0x000523E0
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			MethodInfo setMethod = this.GetSetMethod(true);
			if (setMethod == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_SetMethNotFnd"));
			}
			object[] array;
			if (index != null)
			{
				array = new object[index.Length + 1];
				for (int i = 0; i < index.Length; i++)
				{
					array[i] = index[i];
				}
				array[index.Length] = value;
			}
			else
			{
				array = new object[]
				{
					value
				};
			}
			setMethod.Invoke(obj, invokeAttr, binder, array, culture);
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x00053452 File Offset: 0x00052452
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			MemberInfoSerializationHolder.GetSerializationInfo(info, this.Name, this.ReflectedType, this.ToString(), MemberTypes.Property);
		}

		// Token: 0x04000E0F RID: 3599
		private int m_token;

		// Token: 0x04000E10 RID: 3600
		private string m_name;

		// Token: 0x04000E11 RID: 3601
		private unsafe void* m_utf8name;

		// Token: 0x04000E12 RID: 3602
		private PropertyAttributes m_flags;

		// Token: 0x04000E13 RID: 3603
		private RuntimeType.RuntimeTypeCache m_reflectedTypeCache;

		// Token: 0x04000E14 RID: 3604
		private RuntimeMethodInfo m_getterMethod;

		// Token: 0x04000E15 RID: 3605
		private RuntimeMethodInfo m_setterMethod;

		// Token: 0x04000E16 RID: 3606
		private MethodInfo[] m_otherMethod;

		// Token: 0x04000E17 RID: 3607
		private RuntimeType m_declaringType;

		// Token: 0x04000E18 RID: 3608
		private BindingFlags m_bindingFlags;

		// Token: 0x04000E19 RID: 3609
		private Signature m_signature;
	}
}

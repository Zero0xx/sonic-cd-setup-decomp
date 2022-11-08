using System;
using System.Collections.Generic;
using System.Reflection.Cache;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;

namespace System.Reflection
{
	// Token: 0x02000357 RID: 855
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_ParameterInfo))]
	[ComVisible(true)]
	[Serializable]
	public class ParameterInfo : _ParameterInfo, ICustomAttributeProvider
	{
		// Token: 0x06002177 RID: 8567 RVA: 0x000537D8 File Offset: 0x000527D8
		internal static ParameterInfo[] GetParameters(MethodBase method, MemberInfo member, Signature sig)
		{
			ParameterInfo parameterInfo;
			return ParameterInfo.GetParameters(method, member, sig, out parameterInfo, false);
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x000537F0 File Offset: 0x000527F0
		internal static ParameterInfo GetReturnParameter(MethodBase method, MemberInfo member, Signature sig)
		{
			ParameterInfo result;
			ParameterInfo.GetParameters(method, member, sig, out result, true);
			return result;
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x0005380C File Offset: 0x0005280C
		internal unsafe static ParameterInfo[] GetParameters(MethodBase method, MemberInfo member, Signature sig, out ParameterInfo returnParameter, bool fetchReturnParameter)
		{
			RuntimeMethodHandle methodHandle = method.GetMethodHandle();
			returnParameter = null;
			int num = sig.Arguments.Length;
			ParameterInfo[] array = fetchReturnParameter ? null : new ParameterInfo[num];
			int methodDef = methodHandle.GetMethodDef();
			int num2 = 0;
			if (!System.Reflection.MetadataToken.IsNullToken(methodDef))
			{
				MetadataImport metadataImport = methodHandle.GetDeclaringType().GetModuleHandle().GetMetadataImport();
				num2 = metadataImport.EnumParamsCount(methodDef);
				int* ptr = stackalloc int[4 * num2];
				metadataImport.EnumParams(methodDef, ptr, num2);
				uint num3 = 0U;
				while ((ulong)num3 < (ulong)((long)num2))
				{
					int num4 = ptr[num3];
					int num5;
					ParameterAttributes attributes;
					metadataImport.GetParamDefProps(num4, out num5, out attributes);
					num5--;
					if (fetchReturnParameter && num5 == -1)
					{
						returnParameter = new ParameterInfo(sig, metadataImport, num4, num5, attributes, member);
					}
					else if (!fetchReturnParameter && num5 >= 0)
					{
						array[num5] = new ParameterInfo(sig, metadataImport, num4, num5, attributes, member);
					}
					num3 += 1U;
				}
			}
			if (fetchReturnParameter)
			{
				if (returnParameter == null)
				{
					returnParameter = new ParameterInfo(sig, MetadataImport.EmptyImport, 0, -1, ParameterAttributes.None, member);
				}
			}
			else if (num2 < array.Length + 1)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == null)
					{
						array[i] = new ParameterInfo(sig, MetadataImport.EmptyImport, 0, i, ParameterAttributes.None, member);
					}
				}
			}
			return array;
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x00053943 File Offset: 0x00052943
		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			Type parameterType = this.ParameterType;
			string name = this.Name;
			this.DefaultValueImpl = this.DefaultValue;
			this._importer = IntPtr.Zero;
			this._token = this.m_tkParamDef;
			this.bExtraConstChecked = false;
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x00053980 File Offset: 0x00052980
		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (this.MemberImpl == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
			}
			MemberTypes memberType = this.MemberImpl.MemberType;
			ParameterInfo parameterInfo;
			if (memberType != MemberTypes.Constructor && memberType != MemberTypes.Method)
			{
				if (memberType != MemberTypes.Property)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_NoParameterInfo"));
				}
				ParameterInfo[] array = ((PropertyInfo)this.MemberImpl).GetIndexParameters();
				if (array == null || this.PositionImpl <= -1 || this.PositionImpl >= array.Length)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_BadParameterInfo"));
				}
				parameterInfo = array[this.PositionImpl];
			}
			else if (this.PositionImpl == -1)
			{
				if (this.MemberImpl.MemberType != MemberTypes.Method)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_BadParameterInfo"));
				}
				parameterInfo = ((MethodInfo)this.MemberImpl).ReturnParameter;
			}
			else
			{
				ParameterInfo[] array = ((MethodBase)this.MemberImpl).GetParametersNoCopy();
				if (array == null || this.PositionImpl >= array.Length)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_BadParameterInfo"));
				}
				parameterInfo = array[this.PositionImpl];
			}
			this.m_tkParamDef = parameterInfo.m_tkParamDef;
			this.m_scope = parameterInfo.m_scope;
			this.m_signature = parameterInfo.m_signature;
			this.m_nameIsCached = true;
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x00053ABE File Offset: 0x00052ABE
		protected ParameterInfo()
		{
			this.m_nameIsCached = true;
			this.m_noDefaultValue = true;
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x00053AD6 File Offset: 0x00052AD6
		internal ParameterInfo(ParameterInfo accessor, RuntimePropertyInfo property) : this(accessor, property)
		{
			this.m_signature = property.Signature;
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x00053AEC File Offset: 0x00052AEC
		internal ParameterInfo(ParameterInfo accessor, MethodBuilderInstantiation method) : this(accessor, method)
		{
			this.m_signature = accessor.m_signature;
			if (this.ClassImpl.IsGenericParameter)
			{
				this.ClassImpl = method.GetGenericArguments()[this.ClassImpl.GenericParameterPosition];
			}
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x00053B28 File Offset: 0x00052B28
		private ParameterInfo(ParameterInfo accessor, MemberInfo member)
		{
			this.MemberImpl = member;
			this.NameImpl = accessor.Name;
			this.m_nameIsCached = true;
			this.ClassImpl = accessor.ParameterType;
			this.PositionImpl = accessor.Position;
			this.AttrsImpl = accessor.Attributes;
			this.m_tkParamDef = (System.Reflection.MetadataToken.IsNullToken(accessor.MetadataToken) ? 134217728 : accessor.MetadataToken);
			this.m_scope = accessor.m_scope;
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x00053BA8 File Offset: 0x00052BA8
		private ParameterInfo(Signature signature, MetadataImport scope, int tkParamDef, int position, ParameterAttributes attributes, MemberInfo member)
		{
			this.PositionImpl = position;
			this.MemberImpl = member;
			this.m_signature = signature;
			this.m_tkParamDef = (System.Reflection.MetadataToken.IsNullToken(tkParamDef) ? 134217728 : tkParamDef);
			this.m_scope = scope;
			this.AttrsImpl = attributes;
			this.ClassImpl = null;
			this.NameImpl = null;
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x00053C08 File Offset: 0x00052C08
		internal ParameterInfo(MethodInfo owner, string name, RuntimeType parameterType, int position)
		{
			this.MemberImpl = owner;
			this.NameImpl = name;
			this.m_nameIsCached = true;
			this.m_noDefaultValue = true;
			this.ClassImpl = parameterType;
			this.PositionImpl = position;
			this.AttrsImpl = ParameterAttributes.None;
			this.m_tkParamDef = 134217728;
			this.m_scope = MetadataImport.EmptyImport;
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06002182 RID: 8578 RVA: 0x00053C65 File Offset: 0x00052C65
		private bool IsLegacyParameterInfo
		{
			get
			{
				return base.GetType() != typeof(ParameterInfo);
			}
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x00053C7C File Offset: 0x00052C7C
		internal void SetName(string name)
		{
			this.NameImpl = name;
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x00053C85 File Offset: 0x00052C85
		internal void SetAttributes(ParameterAttributes attributes)
		{
			this.AttrsImpl = attributes;
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06002185 RID: 8581 RVA: 0x00053C90 File Offset: 0x00052C90
		public virtual Type ParameterType
		{
			get
			{
				if (this.ClassImpl == null && base.GetType() == typeof(ParameterInfo))
				{
					RuntimeTypeHandle runtimeTypeHandle;
					if (this.PositionImpl == -1)
					{
						runtimeTypeHandle = this.m_signature.ReturnTypeHandle;
					}
					else
					{
						runtimeTypeHandle = this.m_signature.Arguments[this.PositionImpl];
					}
					this.ClassImpl = runtimeTypeHandle.GetRuntimeType();
				}
				return this.ClassImpl;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06002186 RID: 8582 RVA: 0x00053D00 File Offset: 0x00052D00
		public virtual string Name
		{
			get
			{
				if (!this.m_nameIsCached)
				{
					if (!System.Reflection.MetadataToken.IsNullToken(this.m_tkParamDef))
					{
						string nameImpl = this.m_scope.GetName(this.m_tkParamDef).ToString();
						this.NameImpl = nameImpl;
					}
					this.m_nameIsCached = true;
				}
				return this.NameImpl;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06002187 RID: 8583 RVA: 0x00053D5A File Offset: 0x00052D5A
		public virtual object DefaultValue
		{
			get
			{
				return this.GetDefaultValue(false);
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x00053D63 File Offset: 0x00052D63
		public virtual object RawDefaultValue
		{
			get
			{
				return this.GetDefaultValue(true);
			}
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x00053D6C File Offset: 0x00052D6C
		internal object GetDefaultValue(bool raw)
		{
			object obj = null;
			if (!this.m_noDefaultValue)
			{
				if (this.ParameterType == typeof(DateTime))
				{
					if (raw)
					{
						CustomAttributeTypedArgument customAttributeTypedArgument = CustomAttributeData.Filter(CustomAttributeData.GetCustomAttributes(this), typeof(DateTimeConstantAttribute), 0);
						if (customAttributeTypedArgument.ArgumentType != null)
						{
							return new DateTime((long)customAttributeTypedArgument.Value);
						}
					}
					else
					{
						object[] customAttributes = this.GetCustomAttributes(typeof(DateTimeConstantAttribute), false);
						if (customAttributes != null && customAttributes.Length != 0)
						{
							return ((DateTimeConstantAttribute)customAttributes[0]).Value;
						}
					}
				}
				if (!System.Reflection.MetadataToken.IsNullToken(this.m_tkParamDef))
				{
					obj = MdConstant.GetValue(this.m_scope, this.m_tkParamDef, this.ParameterType.GetTypeHandleInternal(), raw);
				}
				if (obj == DBNull.Value)
				{
					if (raw)
					{
						IList<CustomAttributeData> customAttributes2 = CustomAttributeData.GetCustomAttributes(this);
						CustomAttributeTypedArgument customAttributeTypedArgument2 = CustomAttributeData.Filter(customAttributes2, ParameterInfo.s_CustomConstantAttributeType, "Value");
						if (customAttributeTypedArgument2.ArgumentType == null)
						{
							customAttributeTypedArgument2 = CustomAttributeData.Filter(customAttributes2, ParameterInfo.s_DecimalConstantAttributeType, "Value");
							if (customAttributeTypedArgument2.ArgumentType == null)
							{
								for (int i = 0; i < customAttributes2.Count; i++)
								{
									if (customAttributes2[i].Constructor.DeclaringType == ParameterInfo.s_DecimalConstantAttributeType)
									{
										ParameterInfo[] parameters = customAttributes2[i].Constructor.GetParameters();
										if (parameters.Length != 0)
										{
											if (parameters[2].ParameterType == typeof(uint))
											{
												IList<CustomAttributeTypedArgument> constructorArguments = customAttributes2[i].ConstructorArguments;
												int lo = (int)((uint)constructorArguments[4].Value);
												int mid = (int)((uint)constructorArguments[3].Value);
												int hi = (int)((uint)constructorArguments[2].Value);
												byte b = (byte)constructorArguments[1].Value;
												byte scale = (byte)constructorArguments[0].Value;
												customAttributeTypedArgument2 = new CustomAttributeTypedArgument(new decimal(lo, mid, hi, b != 0, scale));
											}
											else
											{
												IList<CustomAttributeTypedArgument> constructorArguments2 = customAttributes2[i].ConstructorArguments;
												int lo2 = (int)constructorArguments2[4].Value;
												int mid2 = (int)constructorArguments2[3].Value;
												int hi2 = (int)constructorArguments2[2].Value;
												byte b2 = (byte)constructorArguments2[1].Value;
												byte scale2 = (byte)constructorArguments2[0].Value;
												customAttributeTypedArgument2 = new CustomAttributeTypedArgument(new decimal(lo2, mid2, hi2, b2 != 0, scale2));
											}
										}
									}
								}
							}
						}
						if (customAttributeTypedArgument2.ArgumentType != null)
						{
							obj = customAttributeTypedArgument2.Value;
						}
					}
					else
					{
						object[] customAttributes3 = this.GetCustomAttributes(ParameterInfo.s_CustomConstantAttributeType, false);
						if (customAttributes3.Length != 0)
						{
							obj = ((CustomConstantAttribute)customAttributes3[0]).Value;
						}
						else
						{
							customAttributes3 = this.GetCustomAttributes(ParameterInfo.s_DecimalConstantAttributeType, false);
							if (customAttributes3.Length != 0)
							{
								obj = ((DecimalConstantAttribute)customAttributes3[0]).Value;
							}
						}
					}
				}
				if (obj == DBNull.Value && this.IsOptional)
				{
					obj = Type.Missing;
				}
			}
			return obj;
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x0600218A RID: 8586 RVA: 0x000540B1 File Offset: 0x000530B1
		public virtual int Position
		{
			get
			{
				return this.PositionImpl;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x0600218B RID: 8587 RVA: 0x000540B9 File Offset: 0x000530B9
		public virtual ParameterAttributes Attributes
		{
			get
			{
				return this.AttrsImpl;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x0600218C RID: 8588 RVA: 0x000540C1 File Offset: 0x000530C1
		public virtual MemberInfo Member
		{
			get
			{
				return this.MemberImpl;
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x0600218D RID: 8589 RVA: 0x000540C9 File Offset: 0x000530C9
		public bool IsIn
		{
			get
			{
				return (this.Attributes & ParameterAttributes.In) != ParameterAttributes.None;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x0600218E RID: 8590 RVA: 0x000540D9 File Offset: 0x000530D9
		public bool IsOut
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Out) != ParameterAttributes.None;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x0600218F RID: 8591 RVA: 0x000540E9 File Offset: 0x000530E9
		public bool IsLcid
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Lcid) != ParameterAttributes.None;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06002190 RID: 8592 RVA: 0x000540F9 File Offset: 0x000530F9
		public bool IsRetval
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Retval) != ParameterAttributes.None;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06002191 RID: 8593 RVA: 0x00054109 File Offset: 0x00053109
		public bool IsOptional
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Optional) != ParameterAttributes.None;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x0005411A File Offset: 0x0005311A
		public int MetadataToken
		{
			get
			{
				return this.m_tkParamDef;
			}
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x00054122 File Offset: 0x00053122
		public virtual Type[] GetRequiredCustomModifiers()
		{
			if (this.IsLegacyParameterInfo)
			{
				return new Type[0];
			}
			return this.m_signature.GetCustomModifiers(this.PositionImpl + 1, true);
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x00054147 File Offset: 0x00053147
		public virtual Type[] GetOptionalCustomModifiers()
		{
			if (this.IsLegacyParameterInfo)
			{
				return new Type[0];
			}
			return this.m_signature.GetCustomModifiers(this.PositionImpl + 1, false);
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x0005416C File Offset: 0x0005316C
		public override string ToString()
		{
			return this.ParameterType.SigToString() + " " + this.Name;
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x00054189 File Offset: 0x00053189
		public virtual object[] GetCustomAttributes(bool inherit)
		{
			if (this.IsLegacyParameterInfo)
			{
				return null;
			}
			if (System.Reflection.MetadataToken.IsNullToken(this.m_tkParamDef))
			{
				return new object[0];
			}
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType);
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x000541C0 File Offset: 0x000531C0
		public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (this.IsLegacyParameterInfo)
			{
				return null;
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (System.Reflection.MetadataToken.IsNullToken(this.m_tkParamDef))
			{
				return new object[0];
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.GetCustomAttributes(this, runtimeType);
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x00054224 File Offset: 0x00053224
		public virtual bool IsDefined(Type attributeType, bool inherit)
		{
			if (this.IsLegacyParameterInfo)
			{
				return false;
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (System.Reflection.MetadataToken.IsNullToken(this.m_tkParamDef))
			{
				return false;
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.IsDefined(this, runtimeType);
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06002199 RID: 8601 RVA: 0x00054284 File Offset: 0x00053284
		internal InternalCache Cache
		{
			get
			{
				InternalCache internalCache = this.m_cachedData;
				if (internalCache == null)
				{
					internalCache = new InternalCache("ParameterInfo");
					InternalCache internalCache2 = Interlocked.CompareExchange<InternalCache>(ref this.m_cachedData, internalCache, null);
					if (internalCache2 != null)
					{
						internalCache = internalCache2;
					}
					GC.ClearCache += this.OnCacheClear;
				}
				return internalCache;
			}
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x000542CB File Offset: 0x000532CB
		internal void OnCacheClear(object sender, ClearCacheEventArgs cacheEventArgs)
		{
			this.m_cachedData = null;
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x000542D4 File Offset: 0x000532D4
		void _ParameterInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x000542DB File Offset: 0x000532DB
		void _ParameterInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x000542E2 File Offset: 0x000532E2
		void _ParameterInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x000542E9 File Offset: 0x000532E9
		void _ParameterInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000E25 RID: 3621
		private static readonly Type s_DecimalConstantAttributeType = typeof(DecimalConstantAttribute);

		// Token: 0x04000E26 RID: 3622
		private static readonly Type s_CustomConstantAttributeType = typeof(CustomConstantAttribute);

		// Token: 0x04000E27 RID: 3623
		private static Type ParameterInfoType = typeof(ParameterInfo);

		// Token: 0x04000E28 RID: 3624
		protected string NameImpl;

		// Token: 0x04000E29 RID: 3625
		protected Type ClassImpl;

		// Token: 0x04000E2A RID: 3626
		protected int PositionImpl;

		// Token: 0x04000E2B RID: 3627
		protected ParameterAttributes AttrsImpl;

		// Token: 0x04000E2C RID: 3628
		protected object DefaultValueImpl;

		// Token: 0x04000E2D RID: 3629
		protected MemberInfo MemberImpl;

		// Token: 0x04000E2E RID: 3630
		private IntPtr _importer;

		// Token: 0x04000E2F RID: 3631
		private int _token;

		// Token: 0x04000E30 RID: 3632
		private bool bExtraConstChecked;

		// Token: 0x04000E31 RID: 3633
		[NonSerialized]
		private int m_tkParamDef;

		// Token: 0x04000E32 RID: 3634
		[NonSerialized]
		private MetadataImport m_scope;

		// Token: 0x04000E33 RID: 3635
		[NonSerialized]
		private Signature m_signature;

		// Token: 0x04000E34 RID: 3636
		[NonSerialized]
		private volatile bool m_nameIsCached;

		// Token: 0x04000E35 RID: 3637
		[NonSerialized]
		private readonly bool m_noDefaultValue;

		// Token: 0x04000E36 RID: 3638
		private InternalCache m_cachedData;

		// Token: 0x02000358 RID: 856
		[Flags]
		private enum WhatIsCached
		{
			// Token: 0x04000E38 RID: 3640
			Nothing = 0,
			// Token: 0x04000E39 RID: 3641
			Name = 1,
			// Token: 0x04000E3A RID: 3642
			ParameterType = 2,
			// Token: 0x04000E3B RID: 3643
			DefaultValue = 4,
			// Token: 0x04000E3C RID: 3644
			All = 7
		}
	}
}

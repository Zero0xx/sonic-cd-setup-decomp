using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000300 RID: 768
	[ComVisible(true)]
	[Serializable]
	public sealed class CustomAttributeData
	{
		// Token: 0x06001E2B RID: 7723 RVA: 0x0004A5F0 File Offset: 0x000495F0
		public static IList<CustomAttributeData> GetCustomAttributes(MemberInfo target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			IList<CustomAttributeData> customAttributes = CustomAttributeData.GetCustomAttributes(target.Module, target.MetadataToken);
			int num = 0;
			Attribute[] array = null;
			if (target is RuntimeType)
			{
				array = PseudoCustomAttribute.GetCustomAttributes((RuntimeType)target, typeof(object), false, out num);
			}
			else if (target is RuntimeMethodInfo)
			{
				array = PseudoCustomAttribute.GetCustomAttributes((RuntimeMethodInfo)target, typeof(object), false, out num);
			}
			else if (target is RuntimeFieldInfo)
			{
				array = PseudoCustomAttribute.GetCustomAttributes((RuntimeFieldInfo)target, typeof(object), out num);
			}
			if (num == 0)
			{
				return customAttributes;
			}
			CustomAttributeData[] array2 = new CustomAttributeData[customAttributes.Count + num];
			customAttributes.CopyTo(array2, num);
			for (int i = 0; i < num; i++)
			{
				if (!PseudoCustomAttribute.IsSecurityAttribute(array[i].GetType()))
				{
					array2[i] = new CustomAttributeData(array[i]);
				}
			}
			return Array.AsReadOnly<CustomAttributeData>(array2);
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x0004A6D5 File Offset: 0x000496D5
		public static IList<CustomAttributeData> GetCustomAttributes(Module target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (target.IsResourceInternal())
			{
				return new List<CustomAttributeData>();
			}
			return CustomAttributeData.GetCustomAttributes(target, target.MetadataToken);
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x0004A700 File Offset: 0x00049700
		public static IList<CustomAttributeData> GetCustomAttributes(Assembly target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			return CustomAttributeData.GetCustomAttributes(target.ManifestModule, target.AssemblyHandle.GetToken());
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x0004A734 File Offset: 0x00049734
		public static IList<CustomAttributeData> GetCustomAttributes(ParameterInfo target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			IList<CustomAttributeData> customAttributes = CustomAttributeData.GetCustomAttributes(target.Member.Module, target.MetadataToken);
			int num = 0;
			Attribute[] customAttributes2 = PseudoCustomAttribute.GetCustomAttributes(target, typeof(object), out num);
			if (num == 0)
			{
				return customAttributes;
			}
			CustomAttributeData[] array = new CustomAttributeData[customAttributes.Count + num];
			customAttributes.CopyTo(array, num);
			for (int i = 0; i < num; i++)
			{
				array[i] = new CustomAttributeData(customAttributes2[i]);
			}
			return Array.AsReadOnly<CustomAttributeData>(array);
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x0004A7BC File Offset: 0x000497BC
		private static CustomAttributeEncoding TypeToCustomAttributeEncoding(Type type)
		{
			if (type == typeof(int))
			{
				return CustomAttributeEncoding.Int32;
			}
			if (type.IsEnum)
			{
				return CustomAttributeEncoding.Enum;
			}
			if (type == typeof(string))
			{
				return CustomAttributeEncoding.String;
			}
			if (type == typeof(Type))
			{
				return CustomAttributeEncoding.Type;
			}
			if (type == typeof(object))
			{
				return CustomAttributeEncoding.Object;
			}
			if (type.IsArray)
			{
				return CustomAttributeEncoding.Array;
			}
			if (type == typeof(char))
			{
				return CustomAttributeEncoding.Char;
			}
			if (type == typeof(bool))
			{
				return CustomAttributeEncoding.Boolean;
			}
			if (type == typeof(byte))
			{
				return CustomAttributeEncoding.Byte;
			}
			if (type == typeof(sbyte))
			{
				return CustomAttributeEncoding.SByte;
			}
			if (type == typeof(short))
			{
				return CustomAttributeEncoding.Int16;
			}
			if (type == typeof(ushort))
			{
				return CustomAttributeEncoding.UInt16;
			}
			if (type == typeof(uint))
			{
				return CustomAttributeEncoding.UInt32;
			}
			if (type == typeof(long))
			{
				return CustomAttributeEncoding.Int64;
			}
			if (type == typeof(ulong))
			{
				return CustomAttributeEncoding.UInt64;
			}
			if (type == typeof(float))
			{
				return CustomAttributeEncoding.Float;
			}
			if (type == typeof(double))
			{
				return CustomAttributeEncoding.Double;
			}
			if (type.IsClass)
			{
				return CustomAttributeEncoding.Object;
			}
			if (type.IsInterface)
			{
				return CustomAttributeEncoding.Object;
			}
			if (type.IsValueType)
			{
				return CustomAttributeEncoding.Undefined;
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_InvalidKindOfTypeForCA"), "type");
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x0004A8FC File Offset: 0x000498FC
		private static CustomAttributeType InitCustomAttributeType(Type parameterType, Module scope)
		{
			CustomAttributeEncoding customAttributeEncoding = CustomAttributeData.TypeToCustomAttributeEncoding(parameterType);
			CustomAttributeEncoding customAttributeEncoding2 = CustomAttributeEncoding.Undefined;
			CustomAttributeEncoding encodedEnumType = CustomAttributeEncoding.Undefined;
			string enumName = null;
			if (customAttributeEncoding == CustomAttributeEncoding.Array)
			{
				parameterType = parameterType.GetElementType();
				customAttributeEncoding2 = CustomAttributeData.TypeToCustomAttributeEncoding(parameterType);
			}
			if (customAttributeEncoding == CustomAttributeEncoding.Enum || customAttributeEncoding2 == CustomAttributeEncoding.Enum)
			{
				encodedEnumType = CustomAttributeData.TypeToCustomAttributeEncoding(Enum.GetUnderlyingType(parameterType));
				if (parameterType.Module == scope)
				{
					enumName = parameterType.FullName;
				}
				else
				{
					enumName = parameterType.AssemblyQualifiedName;
				}
			}
			return new CustomAttributeType(customAttributeEncoding, customAttributeEncoding2, encodedEnumType, enumName);
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x0004A964 File Offset: 0x00049964
		private static IList<CustomAttributeData> GetCustomAttributes(Module module, int tkTarget)
		{
			CustomAttributeRecord[] customAttributeRecords = CustomAttributeData.GetCustomAttributeRecords(module, tkTarget);
			CustomAttributeData[] array = new CustomAttributeData[customAttributeRecords.Length];
			for (int i = 0; i < customAttributeRecords.Length; i++)
			{
				array[i] = new CustomAttributeData(module, customAttributeRecords[i]);
			}
			return Array.AsReadOnly<CustomAttributeData>(array);
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x0004A9AC File Offset: 0x000499AC
		internal unsafe static CustomAttributeRecord[] GetCustomAttributeRecords(Module module, int targetToken)
		{
			MetadataImport metadataImport = module.MetadataImport;
			int num = metadataImport.EnumCustomAttributesCount(targetToken);
			int* ptr = stackalloc int[4 * num];
			metadataImport.EnumCustomAttributes(targetToken, ptr, num);
			CustomAttributeRecord[] array = new CustomAttributeRecord[num];
			for (int i = 0; i < num; i++)
			{
				metadataImport.GetCustomAttributeProps(ptr[i], out array[i].tkCtor.Value, out array[i].blob);
			}
			return array;
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x0004AA20 File Offset: 0x00049A20
		internal static CustomAttributeTypedArgument Filter(IList<CustomAttributeData> attrs, Type caType, string name)
		{
			for (int i = 0; i < attrs.Count; i++)
			{
				if (attrs[i].Constructor.DeclaringType == caType)
				{
					IList<CustomAttributeNamedArgument> namedArguments = attrs[i].NamedArguments;
					for (int j = 0; j < namedArguments.Count; j++)
					{
						if (namedArguments[j].MemberInfo.Name.Equals(name))
						{
							return namedArguments[j].TypedValue;
						}
					}
				}
			}
			return default(CustomAttributeTypedArgument);
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x0004AAA8 File Offset: 0x00049AA8
		internal static CustomAttributeTypedArgument Filter(IList<CustomAttributeData> attrs, Type caType, int parameter)
		{
			for (int i = 0; i < attrs.Count; i++)
			{
				if (attrs[i].Constructor.DeclaringType == caType)
				{
					return attrs[i].ConstructorArguments[parameter];
				}
			}
			return default(CustomAttributeTypedArgument);
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x0004AAF8 File Offset: 0x00049AF8
		internal CustomAttributeData(Module scope, CustomAttributeRecord caRecord)
		{
			this.m_scope = scope;
			this.m_ctor = (ConstructorInfo)RuntimeType.GetMethodBase(scope, caRecord.tkCtor);
			ParameterInfo[] parametersNoCopy = this.m_ctor.GetParametersNoCopy();
			this.m_ctorParams = new CustomAttributeCtorParameter[parametersNoCopy.Length];
			for (int i = 0; i < parametersNoCopy.Length; i++)
			{
				this.m_ctorParams[i] = new CustomAttributeCtorParameter(CustomAttributeData.InitCustomAttributeType(parametersNoCopy[i].ParameterType, scope));
			}
			FieldInfo[] fields = this.m_ctor.DeclaringType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			PropertyInfo[] properties = this.m_ctor.DeclaringType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			this.m_namedParams = new CustomAttributeNamedParameter[properties.Length + fields.Length];
			for (int j = 0; j < fields.Length; j++)
			{
				this.m_namedParams[j] = new CustomAttributeNamedParameter(fields[j].Name, CustomAttributeEncoding.Field, CustomAttributeData.InitCustomAttributeType(fields[j].FieldType, scope));
			}
			for (int k = 0; k < properties.Length; k++)
			{
				this.m_namedParams[k + fields.Length] = new CustomAttributeNamedParameter(properties[k].Name, CustomAttributeEncoding.Property, CustomAttributeData.InitCustomAttributeType(properties[k].PropertyType, scope));
			}
			this.m_members = new MemberInfo[fields.Length + properties.Length];
			fields.CopyTo(this.m_members, 0);
			properties.CopyTo(this.m_members, fields.Length);
			CustomAttributeEncodedArgument.ParseAttributeArguments(caRecord.blob, ref this.m_ctorParams, ref this.m_namedParams, this.m_scope);
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x0004AC8C File Offset: 0x00049C8C
		internal CustomAttributeData(Attribute attribute)
		{
			if (attribute is DllImportAttribute)
			{
				this.Init((DllImportAttribute)attribute);
				return;
			}
			if (attribute is FieldOffsetAttribute)
			{
				this.Init((FieldOffsetAttribute)attribute);
				return;
			}
			if (attribute is MarshalAsAttribute)
			{
				this.Init((MarshalAsAttribute)attribute);
				return;
			}
			this.Init(attribute);
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x0004ACE8 File Offset: 0x00049CE8
		private void Init(DllImportAttribute dllImport)
		{
			Type typeFromHandle = typeof(DllImportAttribute);
			this.m_ctor = typeFromHandle.GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0];
			this.m_typedCtorArgs = Array.AsReadOnly<CustomAttributeTypedArgument>(new CustomAttributeTypedArgument[]
			{
				new CustomAttributeTypedArgument(dllImport.Value)
			});
			this.m_namedArgs = Array.AsReadOnly<CustomAttributeNamedArgument>(new CustomAttributeNamedArgument[]
			{
				new CustomAttributeNamedArgument(typeFromHandle.GetField("EntryPoint"), dllImport.EntryPoint),
				new CustomAttributeNamedArgument(typeFromHandle.GetField("CharSet"), dllImport.CharSet),
				new CustomAttributeNamedArgument(typeFromHandle.GetField("ExactSpelling"), dllImport.ExactSpelling),
				new CustomAttributeNamedArgument(typeFromHandle.GetField("SetLastError"), dllImport.SetLastError),
				new CustomAttributeNamedArgument(typeFromHandle.GetField("PreserveSig"), dllImport.PreserveSig),
				new CustomAttributeNamedArgument(typeFromHandle.GetField("CallingConvention"), dllImport.CallingConvention),
				new CustomAttributeNamedArgument(typeFromHandle.GetField("BestFitMapping"), dllImport.BestFitMapping),
				new CustomAttributeNamedArgument(typeFromHandle.GetField("ThrowOnUnmappableChar"), dllImport.ThrowOnUnmappableChar)
			});
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x0004AE80 File Offset: 0x00049E80
		private void Init(FieldOffsetAttribute fieldOffset)
		{
			this.m_ctor = typeof(FieldOffsetAttribute).GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0];
			this.m_typedCtorArgs = Array.AsReadOnly<CustomAttributeTypedArgument>(new CustomAttributeTypedArgument[]
			{
				new CustomAttributeTypedArgument(fieldOffset.Value)
			});
			this.m_namedArgs = Array.AsReadOnly<CustomAttributeNamedArgument>(new CustomAttributeNamedArgument[0]);
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x0004AEE8 File Offset: 0x00049EE8
		private void Init(MarshalAsAttribute marshalAs)
		{
			Type typeFromHandle = typeof(MarshalAsAttribute);
			this.m_ctor = typeFromHandle.GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0];
			this.m_typedCtorArgs = Array.AsReadOnly<CustomAttributeTypedArgument>(new CustomAttributeTypedArgument[]
			{
				new CustomAttributeTypedArgument(marshalAs.Value)
			});
			int num = 3;
			if (marshalAs.MarshalType != null)
			{
				num++;
			}
			if (marshalAs.MarshalTypeRef != null)
			{
				num++;
			}
			if (marshalAs.MarshalCookie != null)
			{
				num++;
			}
			num++;
			num++;
			if (marshalAs.SafeArrayUserDefinedSubType != null)
			{
				num++;
			}
			CustomAttributeNamedArgument[] array = new CustomAttributeNamedArgument[num];
			num = 0;
			array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("ArraySubType"), marshalAs.ArraySubType);
			array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("SizeParamIndex"), marshalAs.SizeParamIndex);
			array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("SizeConst"), marshalAs.SizeConst);
			array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("IidParameterIndex"), marshalAs.IidParameterIndex);
			array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("SafeArraySubType"), marshalAs.SafeArraySubType);
			if (marshalAs.MarshalType != null)
			{
				array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("MarshalType"), marshalAs.MarshalType);
			}
			if (marshalAs.MarshalTypeRef != null)
			{
				array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("MarshalTypeRef"), marshalAs.MarshalTypeRef);
			}
			if (marshalAs.MarshalCookie != null)
			{
				array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("MarshalCookie"), marshalAs.MarshalCookie);
			}
			if (marshalAs.SafeArrayUserDefinedSubType != null)
			{
				array[num++] = new CustomAttributeNamedArgument(typeFromHandle.GetField("SafeArrayUserDefinedSubType"), marshalAs.SafeArrayUserDefinedSubType);
			}
			this.m_namedArgs = Array.AsReadOnly<CustomAttributeNamedArgument>(array);
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x0004B11D File Offset: 0x0004A11D
		private void Init(object pca)
		{
			this.m_ctor = pca.GetType().GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0];
			this.m_typedCtorArgs = Array.AsReadOnly<CustomAttributeTypedArgument>(new CustomAttributeTypedArgument[0]);
			this.m_namedArgs = Array.AsReadOnly<CustomAttributeNamedArgument>(new CustomAttributeNamedArgument[0]);
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x0004B158 File Offset: 0x0004A158
		public override string ToString()
		{
			string text = "";
			for (int i = 0; i < this.ConstructorArguments.Count; i++)
			{
				text += string.Format(CultureInfo.CurrentCulture, (i == 0) ? "{0}" : ", {0}", new object[]
				{
					this.ConstructorArguments[i]
				});
			}
			string text2 = "";
			for (int j = 0; j < this.NamedArguments.Count; j++)
			{
				text2 += string.Format(CultureInfo.CurrentCulture, (j == 0 && text.Length == 0) ? "{0}" : ", {0}", new object[]
				{
					this.NamedArguments[j]
				});
			}
			return string.Format(CultureInfo.CurrentCulture, "[{0}({1}{2})]", new object[]
			{
				this.Constructor.DeclaringType.FullName,
				text,
				text2
			});
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x0004B25A File Offset: 0x0004A25A
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x0004B262 File Offset: 0x0004A262
		public override bool Equals(object obj)
		{
			return obj == this;
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001E3E RID: 7742 RVA: 0x0004B268 File Offset: 0x0004A268
		[ComVisible(true)]
		public ConstructorInfo Constructor
		{
			get
			{
				return this.m_ctor;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001E3F RID: 7743 RVA: 0x0004B270 File Offset: 0x0004A270
		[ComVisible(true)]
		public IList<CustomAttributeTypedArgument> ConstructorArguments
		{
			get
			{
				if (this.m_typedCtorArgs == null)
				{
					CustomAttributeTypedArgument[] array = new CustomAttributeTypedArgument[this.m_ctorParams.Length];
					for (int i = 0; i < array.Length; i++)
					{
						CustomAttributeEncodedArgument customAttributeEncodedArgument = this.m_ctorParams[i].CustomAttributeEncodedArgument;
						array[i] = new CustomAttributeTypedArgument(this.m_scope, this.m_ctorParams[i].CustomAttributeEncodedArgument);
					}
					this.m_typedCtorArgs = Array.AsReadOnly<CustomAttributeTypedArgument>(array);
				}
				return this.m_typedCtorArgs;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001E40 RID: 7744 RVA: 0x0004B2F0 File Offset: 0x0004A2F0
		public IList<CustomAttributeNamedArgument> NamedArguments
		{
			get
			{
				if (this.m_namedArgs == null)
				{
					if (this.m_namedParams == null)
					{
						return null;
					}
					int num = 0;
					for (int i = 0; i < this.m_namedParams.Length; i++)
					{
						if (this.m_namedParams[i].EncodedArgument.CustomAttributeType.EncodedType != CustomAttributeEncoding.Undefined)
						{
							num++;
						}
					}
					CustomAttributeNamedArgument[] array = new CustomAttributeNamedArgument[num];
					int j = 0;
					int num2 = 0;
					while (j < this.m_namedParams.Length)
					{
						if (this.m_namedParams[j].EncodedArgument.CustomAttributeType.EncodedType != CustomAttributeEncoding.Undefined)
						{
							array[num2++] = new CustomAttributeNamedArgument(this.m_members[j], new CustomAttributeTypedArgument(this.m_scope, this.m_namedParams[j].EncodedArgument));
						}
						j++;
					}
					this.m_namedArgs = Array.AsReadOnly<CustomAttributeNamedArgument>(array);
				}
				return this.m_namedArgs;
			}
		}

		// Token: 0x04000B10 RID: 2832
		private ConstructorInfo m_ctor;

		// Token: 0x04000B11 RID: 2833
		private Module m_scope;

		// Token: 0x04000B12 RID: 2834
		private MemberInfo[] m_members;

		// Token: 0x04000B13 RID: 2835
		private CustomAttributeCtorParameter[] m_ctorParams;

		// Token: 0x04000B14 RID: 2836
		private CustomAttributeNamedParameter[] m_namedParams;

		// Token: 0x04000B15 RID: 2837
		private IList<CustomAttributeTypedArgument> m_typedCtorArgs;

		// Token: 0x04000B16 RID: 2838
		private IList<CustomAttributeNamedArgument> m_namedArgs;
	}
}

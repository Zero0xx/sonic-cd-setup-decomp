using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000136 RID: 310
	[Serializable]
	internal class UnitySerializationHolder : ISerializable, IObjectReference
	{
		// Token: 0x0600113D RID: 4413 RVA: 0x0002F860 File Offset: 0x0002E860
		internal static void GetUnitySerializationInfo(SerializationInfo info, Missing missing)
		{
			info.SetType(typeof(UnitySerializationHolder));
			info.AddValue("UnityType", 3);
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x0002F880 File Offset: 0x0002E880
		internal static Type AddElementTypes(SerializationInfo info, Type type)
		{
			List<int> list = new List<int>();
			while (type.HasElementType)
			{
				if (type.IsSzArray)
				{
					list.Add(3);
				}
				else if (type.IsArray)
				{
					list.Add(type.GetArrayRank());
					list.Add(2);
				}
				else if (type.IsPointer)
				{
					list.Add(1);
				}
				else if (type.IsByRef)
				{
					list.Add(4);
				}
				type = type.GetElementType();
			}
			info.AddValue("ElementTypes", list.ToArray(), typeof(int[]));
			return type;
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x0002F910 File Offset: 0x0002E910
		internal Type MakeElementTypes(Type type)
		{
			for (int i = this.m_elementTypes.Length - 1; i >= 0; i--)
			{
				if (this.m_elementTypes[i] == 3)
				{
					type = type.MakeArrayType();
				}
				else if (this.m_elementTypes[i] == 2)
				{
					type = type.MakeArrayType(this.m_elementTypes[--i]);
				}
				else if (this.m_elementTypes[i] == 1)
				{
					type = type.MakePointerType();
				}
				else if (this.m_elementTypes[i] == 4)
				{
					type = type.MakeByRefType();
				}
			}
			return type;
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x0002F994 File Offset: 0x0002E994
		internal static void GetUnitySerializationInfo(SerializationInfo info, Type type)
		{
			if (type.GetRootElementType().IsGenericParameter)
			{
				type = UnitySerializationHolder.AddElementTypes(info, type);
				info.SetType(typeof(UnitySerializationHolder));
				info.AddValue("UnityType", 7);
				info.AddValue("GenericParameterPosition", type.GenericParameterPosition);
				info.AddValue("DeclaringMethod", type.DeclaringMethod, typeof(MethodBase));
				info.AddValue("DeclaringType", type.DeclaringType, typeof(Type));
				return;
			}
			int unityType = 4;
			if (!type.IsGenericTypeDefinition && type.ContainsGenericParameters)
			{
				unityType = 8;
				type = UnitySerializationHolder.AddElementTypes(info, type);
				info.AddValue("GenericArguments", type.GetGenericArguments(), typeof(Type[]));
				type = type.GetGenericTypeDefinition();
			}
			UnitySerializationHolder.GetUnitySerializationInfo(info, unityType, type.FullName, Assembly.GetAssembly(type));
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x0002FA70 File Offset: 0x0002EA70
		internal static void GetUnitySerializationInfo(SerializationInfo info, int unityType, string data, Assembly assembly)
		{
			info.SetType(typeof(UnitySerializationHolder));
			info.AddValue("Data", data, typeof(string));
			info.AddValue("UnityType", unityType);
			string value;
			if (assembly == null)
			{
				value = string.Empty;
			}
			else
			{
				value = assembly.FullName;
			}
			info.AddValue("AssemblyName", value);
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x0002FAD0 File Offset: 0x0002EAD0
		internal UnitySerializationHolder(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_unityType = info.GetInt32("UnityType");
			if (this.m_unityType == 3)
			{
				return;
			}
			if (this.m_unityType == 7)
			{
				this.m_declaringMethod = (info.GetValue("DeclaringMethod", typeof(MethodBase)) as MethodBase);
				this.m_declaringType = (info.GetValue("DeclaringType", typeof(Type)) as Type);
				this.m_genericParameterPosition = info.GetInt32("GenericParameterPosition");
				this.m_elementTypes = (info.GetValue("ElementTypes", typeof(int[])) as int[]);
				return;
			}
			if (this.m_unityType == 8)
			{
				this.m_instantiation = (info.GetValue("GenericArguments", typeof(Type[])) as Type[]);
				this.m_elementTypes = (info.GetValue("ElementTypes", typeof(int[])) as int[]);
			}
			this.m_data = info.GetString("Data");
			this.m_assemblyName = info.GetString("AssemblyName");
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x0002FBF4 File Offset: 0x0002EBF4
		private void ThrowInsufficientInformation(string field)
		{
			throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_InsufficientDeserializationState"), new object[]
			{
				field
			}));
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x0002FC26 File Offset: 0x0002EC26
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnitySerHolder"));
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x0002FC38 File Offset: 0x0002EC38
		public virtual object GetRealObject(StreamingContext context)
		{
			switch (this.m_unityType)
			{
			case 1:
				return Empty.Value;
			case 2:
				return DBNull.Value;
			case 3:
				return Missing.Value;
			case 4:
			{
				if (this.m_data == null || this.m_data.Length == 0)
				{
					this.ThrowInsufficientInformation("Data");
				}
				if (this.m_assemblyName == null)
				{
					this.ThrowInsufficientInformation("AssemblyName");
				}
				if (this.m_assemblyName.Length == 0)
				{
					return Type.GetType(this.m_data, true, false);
				}
				Assembly assembly = Assembly.Load(this.m_assemblyName);
				return assembly.GetType(this.m_data, true, false);
			}
			case 5:
			{
				if (this.m_data == null || this.m_data.Length == 0)
				{
					this.ThrowInsufficientInformation("Data");
				}
				if (this.m_assemblyName == null)
				{
					this.ThrowInsufficientInformation("AssemblyName");
				}
				Assembly assembly = Assembly.Load(this.m_assemblyName);
				Module module = assembly.GetModule(this.m_data);
				if (module == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_UnableToFindModule"), new object[]
					{
						this.m_data,
						this.m_assemblyName
					}));
				}
				return module;
			}
			case 6:
				if (this.m_data == null || this.m_data.Length == 0)
				{
					this.ThrowInsufficientInformation("Data");
				}
				if (this.m_assemblyName == null)
				{
					this.ThrowInsufficientInformation("AssemblyName");
				}
				return Assembly.Load(this.m_assemblyName);
			case 7:
				if (this.m_declaringMethod == null && this.m_declaringType == null)
				{
					this.ThrowInsufficientInformation("DeclaringMember");
				}
				if (this.m_declaringMethod != null)
				{
					return this.m_declaringMethod.GetGenericArguments()[this.m_genericParameterPosition];
				}
				return this.MakeElementTypes(this.m_declaringType.GetGenericArguments()[this.m_genericParameterPosition]);
			case 8:
			{
				this.m_unityType = 4;
				Type type = this.GetRealObject(context) as Type;
				this.m_unityType = 8;
				if (this.m_instantiation[0] == null)
				{
					return null;
				}
				return this.MakeElementTypes(type.MakeGenericType(this.m_instantiation));
			}
			default:
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidUnity"));
			}
		}

		// Token: 0x040005C5 RID: 1477
		internal const int EmptyUnity = 1;

		// Token: 0x040005C6 RID: 1478
		internal const int NullUnity = 2;

		// Token: 0x040005C7 RID: 1479
		internal const int MissingUnity = 3;

		// Token: 0x040005C8 RID: 1480
		internal const int RuntimeTypeUnity = 4;

		// Token: 0x040005C9 RID: 1481
		internal const int ModuleUnity = 5;

		// Token: 0x040005CA RID: 1482
		internal const int AssemblyUnity = 6;

		// Token: 0x040005CB RID: 1483
		internal const int GenericParameterTypeUnity = 7;

		// Token: 0x040005CC RID: 1484
		internal const int PartialInstantiationTypeUnity = 8;

		// Token: 0x040005CD RID: 1485
		internal const int Pointer = 1;

		// Token: 0x040005CE RID: 1486
		internal const int Array = 2;

		// Token: 0x040005CF RID: 1487
		internal const int SzArray = 3;

		// Token: 0x040005D0 RID: 1488
		internal const int ByRef = 4;

		// Token: 0x040005D1 RID: 1489
		private Type[] m_instantiation;

		// Token: 0x040005D2 RID: 1490
		private int[] m_elementTypes;

		// Token: 0x040005D3 RID: 1491
		private int m_genericParameterPosition;

		// Token: 0x040005D4 RID: 1492
		private Type m_declaringType;

		// Token: 0x040005D5 RID: 1493
		private MethodBase m_declaringMethod;

		// Token: 0x040005D6 RID: 1494
		private string m_data;

		// Token: 0x040005D7 RID: 1495
		private string m_assemblyName;

		// Token: 0x040005D8 RID: 1496
		private int m_unityType;
	}
}

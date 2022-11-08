using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Serialization
{
	// Token: 0x0200035F RID: 863
	[ComVisible(true)]
	public sealed class FormatterServices
	{
		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x060021F1 RID: 8689 RVA: 0x000547B0 File Offset: 0x000537B0
		private static object formatterServicesSyncObject
		{
			get
			{
				if (FormatterServices.s_FormatterServicesSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref FormatterServices.s_FormatterServicesSyncObject, value, null);
				}
				return FormatterServices.s_FormatterServicesSyncObject;
			}
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x000547DC File Offset: 0x000537DC
		private FormatterServices()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x000547EC File Offset: 0x000537EC
		private static MemberInfo[] GetSerializableMembers(RuntimeType type)
		{
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			int num = 0;
			for (int i = 0; i < fields.Length; i++)
			{
				if ((fields[i].Attributes & FieldAttributes.NotSerialized) != FieldAttributes.NotSerialized)
				{
					num++;
				}
			}
			if (num != fields.Length)
			{
				FieldInfo[] array = new FieldInfo[num];
				num = 0;
				for (int j = 0; j < fields.Length; j++)
				{
					if ((fields[j].Attributes & FieldAttributes.NotSerialized) != FieldAttributes.NotSerialized)
					{
						array[num] = fields[j];
						num++;
					}
				}
				return array;
			}
			return fields;
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x00054870 File Offset: 0x00053870
		private static bool CheckSerializable(RuntimeType type)
		{
			return type.IsSerializable;
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x00054880 File Offset: 0x00053880
		private static MemberInfo[] InternalGetSerializableMembers(RuntimeType type)
		{
			if (type.IsInterface)
			{
				return new MemberInfo[0];
			}
			if (!FormatterServices.CheckSerializable(type))
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_NonSerType"), new object[]
				{
					type.FullName,
					type.Module.Assembly.FullName
				}));
			}
			MemberInfo[] array = FormatterServices.GetSerializableMembers(type);
			RuntimeType runtimeType = (RuntimeType)type.BaseType;
			if (runtimeType != null && runtimeType != typeof(object))
			{
				Type[] array2 = null;
				int num = 0;
				bool parentTypes = FormatterServices.GetParentTypes(runtimeType, out array2, out num);
				if (num > 0)
				{
					ArrayList arrayList = new ArrayList();
					for (int i = 0; i < num; i++)
					{
						runtimeType = (RuntimeType)array2[i];
						if (!FormatterServices.CheckSerializable(runtimeType))
						{
							throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_NonSerType"), new object[]
							{
								runtimeType.FullName,
								runtimeType.Module.Assembly.FullName
							}));
						}
						FieldInfo[] fields = runtimeType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
						string namePrefix = parentTypes ? runtimeType.Name : runtimeType.FullName;
						foreach (FieldInfo fieldInfo in fields)
						{
							if (!fieldInfo.IsNotSerialized)
							{
								arrayList.Add(new SerializationFieldInfo((RuntimeFieldInfo)fieldInfo, namePrefix));
							}
						}
					}
					if (arrayList != null && arrayList.Count > 0)
					{
						MemberInfo[] array4 = new MemberInfo[arrayList.Count + array.Length];
						Array.Copy(array, array4, array.Length);
						arrayList.CopyTo(array4, array.Length);
						array = array4;
					}
				}
			}
			return array;
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x00054A2C File Offset: 0x00053A2C
		private static bool GetParentTypes(Type parentType, out Type[] parentTypes, out int parentTypeCount)
		{
			parentTypes = null;
			parentTypeCount = 0;
			bool flag = true;
			for (Type type = parentType; type != typeof(object); type = type.BaseType)
			{
				if (!type.IsInterface)
				{
					string name = type.Name;
					int num = 0;
					while (flag && num < parentTypeCount)
					{
						string name2 = parentTypes[num].Name;
						if (name2.Length == name.Length && name2[0] == name[0] && name == name2)
						{
							flag = false;
							break;
						}
						num++;
					}
					if (parentTypes == null || parentTypeCount == parentTypes.Length)
					{
						Type[] array = new Type[Math.Max(parentTypeCount * 2, 12)];
						if (parentTypes != null)
						{
							Array.Copy(parentTypes, 0, array, 0, parentTypeCount);
						}
						parentTypes = array;
					}
					parentTypes[parentTypeCount++] = type;
				}
			}
			return flag;
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x00054AFD File Offset: 0x00053AFD
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public static MemberInfo[] GetSerializableMembers(Type type)
		{
			return FormatterServices.GetSerializableMembers(type, new StreamingContext(StreamingContextStates.All));
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x00054B10 File Offset: 0x00053B10
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public static MemberInfo[] GetSerializableMembers(Type type, StreamingContext context)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!(type is RuntimeType))
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_InvalidType"), new object[]
				{
					type.ToString()
				}));
			}
			MemberHolder key = new MemberHolder(type, context);
			if (FormatterServices.m_MemberInfoTable.ContainsKey(key))
			{
				return FormatterServices.m_MemberInfoTable[key];
			}
			MemberInfo[] array;
			lock (FormatterServices.formatterServicesSyncObject)
			{
				if (FormatterServices.m_MemberInfoTable.ContainsKey(key))
				{
					return FormatterServices.m_MemberInfoTable[key];
				}
				array = FormatterServices.InternalGetSerializableMembers((RuntimeType)type);
				FormatterServices.m_MemberInfoTable[key] = array;
			}
			return array;
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x00054BDC File Offset: 0x00053BDC
		public static void CheckTypeSecurity(Type t, TypeFilterLevel securityLevel)
		{
			if (securityLevel == TypeFilterLevel.Low)
			{
				for (int i = 0; i < FormatterServices.advancedTypes.Length; i++)
				{
					if (FormatterServices.advancedTypes[i].IsAssignableFrom(t))
					{
						throw new SecurityException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_TypeSecurity"), new object[]
						{
							FormatterServices.advancedTypes[i].FullName,
							t.FullName
						}));
					}
				}
			}
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x00054C48 File Offset: 0x00053C48
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public static object GetUninitializedObject(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!(type is RuntimeType))
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_InvalidType"), new object[]
				{
					type.ToString()
				}));
			}
			return FormatterServices.nativeGetUninitializedObject((RuntimeType)type);
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x00054CA4 File Offset: 0x00053CA4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public static object GetSafeUninitializedObject(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!(type is RuntimeType))
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_InvalidType"), new object[]
				{
					type.ToString()
				}));
			}
			if (type == typeof(ConstructionCall) || type == typeof(LogicalCallContext) || type == typeof(SynchronizationAttribute))
			{
				return FormatterServices.nativeGetUninitializedObject((RuntimeType)type);
			}
			object result;
			try
			{
				result = FormatterServices.nativeGetSafeUninitializedObject((RuntimeType)type);
			}
			catch (SecurityException innerException)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Security"), new object[]
				{
					type.FullName
				}), innerException);
			}
			return result;
		}

		// Token: 0x060021FC RID: 8700
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object nativeGetSafeUninitializedObject(RuntimeType type);

		// Token: 0x060021FD RID: 8701
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object nativeGetUninitializedObject(RuntimeType type);

		// Token: 0x060021FE RID: 8702
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetEnableUnsafeTypeForwarders();

		// Token: 0x060021FF RID: 8703 RVA: 0x00054D74 File Offset: 0x00053D74
		[SecuritySafeCritical]
		internal static bool UnsafeTypeForwardersIsEnabled()
		{
			return FormatterServices.GetEnableUnsafeTypeForwarders();
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x00054D7C File Offset: 0x00053D7C
		internal static void SerializationSetValue(MemberInfo fi, object target, object value)
		{
			RtFieldInfo rtFieldInfo = fi as RtFieldInfo;
			if (rtFieldInfo != null)
			{
				rtFieldInfo.InternalSetValue(target, value, BindingFlags.Default, FormatterServices.s_binder, null, false);
				return;
			}
			((SerializationFieldInfo)fi).InternalSetValue(target, value, BindingFlags.Default, FormatterServices.s_binder, null, false, true);
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x00054DBC File Offset: 0x00053DBC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public static object PopulateObjectMembers(object obj, MemberInfo[] members, object[] data)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (members == null)
			{
				throw new ArgumentNullException("members");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (members.Length != data.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_DataLengthDifferent"));
			}
			for (int i = 0; i < members.Length; i++)
			{
				MemberInfo memberInfo = members[i];
				if (memberInfo == null)
				{
					throw new ArgumentNullException("members", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentNull_NullMember"), new object[]
					{
						i
					}));
				}
				if (data[i] != null)
				{
					if (memberInfo.MemberType != MemberTypes.Field)
					{
						throw new SerializationException(Environment.GetResourceString("Serialization_UnknownMemberInfo"));
					}
					FormatterServices.SerializationSetValue(memberInfo, obj, data[i]);
				}
			}
			return obj;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x00054E7C File Offset: 0x00053E7C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public static object[] GetObjectData(object obj, MemberInfo[] members)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (members == null)
			{
				throw new ArgumentNullException("members");
			}
			int num = members.Length;
			object[] array = new object[num];
			for (int i = 0; i < num; i++)
			{
				MemberInfo memberInfo = members[i];
				if (memberInfo == null)
				{
					throw new ArgumentNullException("members", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentNull_NullMember"), new object[]
					{
						i
					}));
				}
				if (memberInfo.MemberType != MemberTypes.Field)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_UnknownMemberInfo"));
				}
				RtFieldInfo rtFieldInfo = memberInfo as RtFieldInfo;
				if (rtFieldInfo != null)
				{
					array[i] = rtFieldInfo.InternalGetValue(obj, false);
				}
				else
				{
					array[i] = ((SerializationFieldInfo)memberInfo).InternalGetValue(obj, false);
				}
			}
			return array;
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x00054F42 File Offset: 0x00053F42
		public static ISerializationSurrogate GetSurrogateForCyclicalReference(ISerializationSurrogate innerSurrogate)
		{
			if (innerSurrogate == null)
			{
				throw new ArgumentNullException("innerSurrogate");
			}
			return new SurrogateForCyclicalReference(innerSurrogate);
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x00054F58 File Offset: 0x00053F58
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public static Type GetTypeFromAssembly(Assembly assem, string name)
		{
			if (assem == null)
			{
				throw new ArgumentNullException("assem");
			}
			return assem.GetType(name, false, false);
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x00054F74 File Offset: 0x00053F74
		internal static Assembly LoadAssemblyFromString(string assemblyName)
		{
			return Assembly.Load(assemblyName);
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x00054F8C File Offset: 0x00053F8C
		internal static Assembly LoadAssemblyFromStringNoThrow(string assemblyName)
		{
			try
			{
				return FormatterServices.LoadAssemblyFromString(assemblyName);
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x04000E3F RID: 3647
		internal static Dictionary<MemberHolder, MemberInfo[]> m_MemberInfoTable = new Dictionary<MemberHolder, MemberInfo[]>(32);

		// Token: 0x04000E40 RID: 3648
		private static object s_FormatterServicesSyncObject = null;

		// Token: 0x04000E41 RID: 3649
		private static readonly Type[] advancedTypes = new Type[]
		{
			typeof(ObjRef),
			typeof(DelegateSerializationHolder),
			typeof(IEnvoyInfo),
			typeof(ISponsor)
		};

		// Token: 0x04000E42 RID: 3650
		private static Binder s_binder = Type.DefaultBinder;
	}
}

using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000061 RID: 97
	[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_Attribute))]
	[ComVisible(true)]
	[Serializable]
	public abstract class Attribute : _Attribute
	{
		// Token: 0x060005C5 RID: 1477 RVA: 0x000140A4 File Offset: 0x000130A4
		private static Attribute[] InternalGetCustomAttributes(PropertyInfo element, Type type, bool inherit)
		{
			Attribute[] array = (Attribute[])element.GetCustomAttributes(type, inherit);
			if (!inherit)
			{
				return array;
			}
			Hashtable types = new Hashtable(11);
			ArrayList arrayList = new ArrayList();
			Attribute.CopyToArrayList(arrayList, array, types);
			for (PropertyInfo parentDefinition = Attribute.GetParentDefinition(element); parentDefinition != null; parentDefinition = Attribute.GetParentDefinition(parentDefinition))
			{
				array = Attribute.GetCustomAttributes(parentDefinition, type, false);
				Attribute.AddAttributesToList(arrayList, array, types);
			}
			return (Attribute[])arrayList.ToArray(type);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001410C File Offset: 0x0001310C
		private static bool InternalIsDefined(PropertyInfo element, Type attributeType, bool inherit)
		{
			if (element.IsDefined(attributeType, inherit))
			{
				return true;
			}
			if (inherit)
			{
				AttributeUsageAttribute attributeUsageAttribute = Attribute.InternalGetAttributeUsage(attributeType);
				if (!attributeUsageAttribute.Inherited)
				{
					return false;
				}
				for (PropertyInfo parentDefinition = Attribute.GetParentDefinition(element); parentDefinition != null; parentDefinition = Attribute.GetParentDefinition(parentDefinition))
				{
					if (parentDefinition.IsDefined(attributeType, false))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001415C File Offset: 0x0001315C
		private static PropertyInfo GetParentDefinition(PropertyInfo property)
		{
			MethodInfo methodInfo = property.GetGetMethod(true);
			if (methodInfo == null)
			{
				methodInfo = property.GetSetMethod(true);
			}
			if (methodInfo != null)
			{
				methodInfo = methodInfo.GetParentDefinition();
				if (methodInfo != null)
				{
					return methodInfo.DeclaringType.GetProperty(property.Name, property.PropertyType);
				}
			}
			return null;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000141A4 File Offset: 0x000131A4
		private static Attribute[] InternalGetCustomAttributes(EventInfo element, Type type, bool inherit)
		{
			Attribute[] array = (Attribute[])element.GetCustomAttributes(type, inherit);
			if (inherit)
			{
				Hashtable types = new Hashtable(11);
				ArrayList arrayList = new ArrayList();
				Attribute.CopyToArrayList(arrayList, array, types);
				for (EventInfo parentDefinition = Attribute.GetParentDefinition(element); parentDefinition != null; parentDefinition = Attribute.GetParentDefinition(parentDefinition))
				{
					array = Attribute.GetCustomAttributes(parentDefinition, type, false);
					Attribute.AddAttributesToList(arrayList, array, types);
				}
				return (Attribute[])arrayList.ToArray(type);
			}
			return array;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001420C File Offset: 0x0001320C
		private static EventInfo GetParentDefinition(EventInfo ev)
		{
			MethodInfo methodInfo = ev.GetAddMethod(true);
			if (methodInfo != null)
			{
				methodInfo = methodInfo.GetParentDefinition();
				if (methodInfo != null)
				{
					return methodInfo.DeclaringType.GetEvent(ev.Name);
				}
			}
			return null;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00014244 File Offset: 0x00013244
		private static bool InternalIsDefined(EventInfo element, Type attributeType, bool inherit)
		{
			if (element.IsDefined(attributeType, inherit))
			{
				return true;
			}
			if (inherit)
			{
				AttributeUsageAttribute attributeUsageAttribute = Attribute.InternalGetAttributeUsage(attributeType);
				if (!attributeUsageAttribute.Inherited)
				{
					return false;
				}
				for (EventInfo parentDefinition = Attribute.GetParentDefinition(element); parentDefinition != null; parentDefinition = Attribute.GetParentDefinition(parentDefinition))
				{
					if (parentDefinition.IsDefined(attributeType, false))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00014294 File Offset: 0x00013294
		private static Attribute[] InternalParamGetCustomAttributes(MethodInfo method, ParameterInfo param, Type type, bool inherit)
		{
			ArrayList arrayList = new ArrayList();
			if (type == null)
			{
				type = typeof(Attribute);
			}
			object[] customAttributes = param.GetCustomAttributes(type, false);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				Type type2 = customAttributes[i].GetType();
				AttributeUsageAttribute attributeUsageAttribute = Attribute.InternalGetAttributeUsage(type2);
				if (!attributeUsageAttribute.AllowMultiple)
				{
					arrayList.Add(type2);
				}
			}
			Attribute[] array;
			if (customAttributes.Length == 0)
			{
				array = (Attribute[])Array.CreateInstance(type, 0);
			}
			else
			{
				array = (Attribute[])customAttributes;
			}
			if (method.DeclaringType == null)
			{
				return array;
			}
			if (!inherit)
			{
				return array;
			}
			int position = param.Position;
			for (method = method.GetParentDefinition(); method != null; method = method.GetParentDefinition())
			{
				ParameterInfo[] parameters = method.GetParameters();
				param = parameters[position];
				customAttributes = param.GetCustomAttributes(type, false);
				int num = 0;
				for (int j = 0; j < customAttributes.Length; j++)
				{
					Type type3 = customAttributes[j].GetType();
					AttributeUsageAttribute attributeUsageAttribute2 = Attribute.InternalGetAttributeUsage(type3);
					if (attributeUsageAttribute2.Inherited && !arrayList.Contains(type3))
					{
						if (!attributeUsageAttribute2.AllowMultiple)
						{
							arrayList.Add(type3);
						}
						num++;
					}
					else
					{
						customAttributes[j] = null;
					}
				}
				Attribute[] array2 = (Attribute[])Array.CreateInstance(type, num);
				num = 0;
				for (int k = 0; k < customAttributes.Length; k++)
				{
					if (customAttributes[k] != null)
					{
						array2[num] = (Attribute)customAttributes[k];
						num++;
					}
				}
				Attribute[] array3 = array;
				array = (Attribute[])Array.CreateInstance(type, array3.Length + num);
				Array.Copy(array3, array, array3.Length);
				int num2 = array3.Length;
				for (int l = 0; l < array2.Length; l++)
				{
					array[num2 + l] = array2[l];
				}
			}
			return array;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00014444 File Offset: 0x00013444
		private static bool InternalParamIsDefined(MethodInfo method, ParameterInfo param, Type type, bool inherit)
		{
			if (param.IsDefined(type, false))
			{
				return true;
			}
			if (method.DeclaringType == null || !inherit)
			{
				return false;
			}
			int position = param.Position;
			for (method = method.GetParentDefinition(); method != null; method = method.GetParentDefinition())
			{
				ParameterInfo[] parameters = method.GetParameters();
				param = parameters[position];
				object[] customAttributes = param.GetCustomAttributes(type, false);
				for (int i = 0; i < customAttributes.Length; i++)
				{
					Type type2 = customAttributes[i].GetType();
					AttributeUsageAttribute attributeUsageAttribute = Attribute.InternalGetAttributeUsage(type2);
					if (customAttributes[i] is Attribute && attributeUsageAttribute.Inherited)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000144D4 File Offset: 0x000134D4
		private static void CopyToArrayList(ArrayList attributeList, Attribute[] attributes, Hashtable types)
		{
			for (int i = 0; i < attributes.Length; i++)
			{
				attributeList.Add(attributes[i]);
				Type type = attributes[i].GetType();
				if (!types.Contains(type))
				{
					types[type] = Attribute.InternalGetAttributeUsage(type);
				}
			}
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00014518 File Offset: 0x00013518
		private static void AddAttributesToList(ArrayList attributeList, Attribute[] attributes, Hashtable types)
		{
			for (int i = 0; i < attributes.Length; i++)
			{
				Type type = attributes[i].GetType();
				AttributeUsageAttribute attributeUsageAttribute = (AttributeUsageAttribute)types[type];
				if (attributeUsageAttribute == null)
				{
					attributeUsageAttribute = Attribute.InternalGetAttributeUsage(type);
					types[type] = attributeUsageAttribute;
					if (attributeUsageAttribute.Inherited)
					{
						attributeList.Add(attributes[i]);
					}
				}
				else if (attributeUsageAttribute.Inherited && attributeUsageAttribute.AllowMultiple)
				{
					attributeList.Add(attributes[i]);
				}
			}
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001458C File Offset: 0x0001358C
		private static AttributeUsageAttribute InternalGetAttributeUsage(Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(AttributeUsageAttribute), false);
			if (customAttributes.Length == 1)
			{
				return (AttributeUsageAttribute)customAttributes[0];
			}
			if (customAttributes.Length == 0)
			{
				return AttributeUsageAttribute.Default;
			}
			throw new FormatException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Format_AttributeUsage"), new object[]
			{
				type
			}));
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x000145EA File Offset: 0x000135EA
		public static Attribute[] GetCustomAttributes(MemberInfo element, Type type)
		{
			return Attribute.GetCustomAttributes(element, type, true);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x000145F4 File Offset: 0x000135F4
		public static Attribute[] GetCustomAttributes(MemberInfo element, Type type, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!type.IsSubclassOf(typeof(Attribute)) && type != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			MemberTypes memberType = element.MemberType;
			if (memberType == MemberTypes.Event)
			{
				return Attribute.InternalGetCustomAttributes((EventInfo)element, type, inherit);
			}
			if (memberType == MemberTypes.Property)
			{
				return Attribute.InternalGetCustomAttributes((PropertyInfo)element, type, inherit);
			}
			return element.GetCustomAttributes(type, inherit) as Attribute[];
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00014685 File Offset: 0x00013685
		public static Attribute[] GetCustomAttributes(MemberInfo element)
		{
			return Attribute.GetCustomAttributes(element, true);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00014690 File Offset: 0x00013690
		public static Attribute[] GetCustomAttributes(MemberInfo element, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			MemberTypes memberType = element.MemberType;
			if (memberType == MemberTypes.Event)
			{
				return Attribute.InternalGetCustomAttributes((EventInfo)element, typeof(Attribute), inherit);
			}
			if (memberType == MemberTypes.Property)
			{
				return Attribute.InternalGetCustomAttributes((PropertyInfo)element, typeof(Attribute), inherit);
			}
			return element.GetCustomAttributes(typeof(Attribute), inherit) as Attribute[];
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000146FF File Offset: 0x000136FF
		public static bool IsDefined(MemberInfo element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType, true);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001470C File Offset: 0x0001370C
		public static bool IsDefined(MemberInfo element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			MemberTypes memberType = element.MemberType;
			if (memberType == MemberTypes.Event)
			{
				return Attribute.InternalIsDefined((EventInfo)element, attributeType, inherit);
			}
			if (memberType == MemberTypes.Property)
			{
				return Attribute.InternalIsDefined((PropertyInfo)element, attributeType, inherit);
			}
			return element.IsDefined(attributeType, inherit);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00014798 File Offset: 0x00013798
		public static Attribute GetCustomAttribute(MemberInfo element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType, true);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x000147A4 File Offset: 0x000137A4
		public static Attribute GetCustomAttribute(MemberInfo element, Type attributeType, bool inherit)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
			if (customAttributes == null || customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 1)
			{
				return customAttributes[0];
			}
			throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.AmbigCust"));
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x000147DD File Offset: 0x000137DD
		public static Attribute[] GetCustomAttributes(ParameterInfo element)
		{
			return Attribute.GetCustomAttributes(element, true);
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x000147E6 File Offset: 0x000137E6
		public static Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType)
		{
			return Attribute.GetCustomAttributes(element, attributeType, true);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x000147F0 File Offset: 0x000137F0
		public static Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			MemberInfo member = element.Member;
			if (member.MemberType == MemberTypes.Method && inherit)
			{
				return Attribute.InternalParamGetCustomAttributes((MethodInfo)member, element, attributeType, inherit);
			}
			return element.GetCustomAttributes(attributeType, inherit) as Attribute[];
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00014878 File Offset: 0x00013878
		public static Attribute[] GetCustomAttributes(ParameterInfo element, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			MemberInfo member = element.Member;
			if (member.MemberType == MemberTypes.Method && inherit)
			{
				return Attribute.InternalParamGetCustomAttributes((MethodInfo)member, element, null, inherit);
			}
			return element.GetCustomAttributes(typeof(Attribute), inherit) as Attribute[];
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x000148CB File Offset: 0x000138CB
		public static bool IsDefined(ParameterInfo element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType, true);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000148D8 File Offset: 0x000138D8
		public static bool IsDefined(ParameterInfo element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			MemberInfo member = element.Member;
			MemberTypes memberType = member.MemberType;
			if (memberType == MemberTypes.Constructor)
			{
				return element.IsDefined(attributeType, false);
			}
			if (memberType == MemberTypes.Method)
			{
				return Attribute.InternalParamIsDefined((MethodInfo)member, element, attributeType, inherit);
			}
			if (memberType != MemberTypes.Property)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidParamInfo"));
			}
			return element.IsDefined(attributeType, false);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0001497D File Offset: 0x0001397D
		public static Attribute GetCustomAttribute(ParameterInfo element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType, true);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00014988 File Offset: 0x00013988
		public static Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, bool inherit)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
			if (customAttributes == null || customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 1)
			{
				return customAttributes[0];
			}
			throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.AmbigCust"));
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000149C8 File Offset: 0x000139C8
		public static Attribute[] GetCustomAttributes(Module element, Type attributeType)
		{
			return Attribute.GetCustomAttributes(element, attributeType, true);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000149D2 File Offset: 0x000139D2
		public static Attribute[] GetCustomAttributes(Module element)
		{
			return Attribute.GetCustomAttributes(element, true);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x000149DB File Offset: 0x000139DB
		public static Attribute[] GetCustomAttributes(Module element, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Attribute[])element.GetCustomAttributes(typeof(Attribute), inherit);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00014A04 File Offset: 0x00013A04
		public static Attribute[] GetCustomAttributes(Module element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			return (Attribute[])element.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00014A69 File Offset: 0x00013A69
		public static bool IsDefined(Module element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType, false);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00014A74 File Offset: 0x00013A74
		public static bool IsDefined(Module element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			return element.IsDefined(attributeType, false);
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00014AD4 File Offset: 0x00013AD4
		public static Attribute GetCustomAttribute(Module element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType, true);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00014AE0 File Offset: 0x00013AE0
		public static Attribute GetCustomAttribute(Module element, Type attributeType, bool inherit)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
			if (customAttributes == null || customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 1)
			{
				return customAttributes[0];
			}
			throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.AmbigCust"));
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00014B19 File Offset: 0x00013B19
		public static Attribute[] GetCustomAttributes(Assembly element, Type attributeType)
		{
			return Attribute.GetCustomAttributes(element, attributeType, true);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00014B24 File Offset: 0x00013B24
		public static Attribute[] GetCustomAttributes(Assembly element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			return (Attribute[])element.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00014B89 File Offset: 0x00013B89
		public static Attribute[] GetCustomAttributes(Assembly element)
		{
			return Attribute.GetCustomAttributes(element, true);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00014B92 File Offset: 0x00013B92
		public static Attribute[] GetCustomAttributes(Assembly element, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Attribute[])element.GetCustomAttributes(typeof(Attribute), inherit);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00014BB8 File Offset: 0x00013BB8
		public static bool IsDefined(Assembly element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType, true);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00014BC4 File Offset: 0x00013BC4
		public static bool IsDefined(Assembly element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			return element.IsDefined(attributeType, false);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00014C24 File Offset: 0x00013C24
		public static Attribute GetCustomAttribute(Assembly element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType, true);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00014C30 File Offset: 0x00013C30
		public static Attribute GetCustomAttribute(Assembly element, Type attributeType, bool inherit)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
			if (customAttributes == null || customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 1)
			{
				return customAttributes[0];
			}
			throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.AmbigCust"));
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00014C74 File Offset: 0x00013C74
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			RuntimeType runtimeType = (RuntimeType)base.GetType();
			RuntimeType runtimeType2 = (RuntimeType)obj.GetType();
			if (runtimeType2 != runtimeType)
			{
				return false;
			}
			FieldInfo[] fields = runtimeType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < fields.Length; i++)
			{
				object value = ((RuntimeFieldInfo)fields[i]).GetValue(this);
				object value2 = ((RuntimeFieldInfo)fields[i]).GetValue(obj);
				if (value == null)
				{
					if (value2 != null)
					{
						return false;
					}
				}
				else if (!value.Equals(value2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00014CFC File Offset: 0x00013CFC
		public override int GetHashCode()
		{
			Type type = base.GetType();
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			object obj = null;
			foreach (FieldInfo fieldInfo in fields)
			{
				obj = fieldInfo.GetValue(this);
				if (obj != null)
				{
					break;
				}
			}
			if (obj != null)
			{
				return obj.GetHashCode();
			}
			return type.GetHashCode();
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x00014D4A File Offset: 0x00013D4A
		public virtual object TypeId
		{
			get
			{
				return base.GetType();
			}
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00014D52 File Offset: 0x00013D52
		public virtual bool Match(object obj)
		{
			return this.Equals(obj);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00014D5B File Offset: 0x00013D5B
		public virtual bool IsDefaultAttribute()
		{
			return false;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00014D5E File Offset: 0x00013D5E
		void _Attribute.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00014D65 File Offset: 0x00013D65
		void _Attribute.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00014D6C File Offset: 0x00013D6C
		void _Attribute.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00014D73 File Offset: 0x00013D73
		void _Attribute.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}
	}
}

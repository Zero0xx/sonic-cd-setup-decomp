using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x0200030B RID: 779
	internal static class PseudoCustomAttribute
	{
		// Token: 0x06001E88 RID: 7816
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetSecurityAttributes(void* module, int token, out object[] securityAttributes);

		// Token: 0x06001E89 RID: 7817 RVA: 0x0004CBD9 File Offset: 0x0004BBD9
		internal static void GetSecurityAttributes(ModuleHandle module, int token, out object[] securityAttributes)
		{
			PseudoCustomAttribute._GetSecurityAttributes(module.Value, token, out securityAttributes);
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x0004CBEC File Offset: 0x0004BBEC
		static PseudoCustomAttribute()
		{
			Type[] array = new Type[]
			{
				typeof(FieldOffsetAttribute),
				typeof(SerializableAttribute),
				typeof(MarshalAsAttribute),
				typeof(ComImportAttribute),
				typeof(NonSerializedAttribute),
				typeof(InAttribute),
				typeof(OutAttribute),
				typeof(OptionalAttribute),
				typeof(DllImportAttribute),
				typeof(PreserveSigAttribute)
			};
			PseudoCustomAttribute.s_pcasCount = array.Length;
			PseudoCustomAttribute.s_pca = new Hashtable(PseudoCustomAttribute.s_pcasCount);
			for (int i = 0; i < PseudoCustomAttribute.s_pcasCount; i++)
			{
				PseudoCustomAttribute.s_pca[array[i]] = array[i];
			}
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x0004CCBD File Offset: 0x0004BCBD
		[Conditional("_DEBUG")]
		private static void VerifyPseudoCustomAttribute(Type pca)
		{
			CustomAttribute.GetAttributeUsage(pca as RuntimeType);
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x0004CCCB File Offset: 0x0004BCCB
		internal static bool IsSecurityAttribute(Type type)
		{
			return type == typeof(SecurityAttribute) || type.IsSubclassOf(typeof(SecurityAttribute));
		}

		// Token: 0x06001E8D RID: 7821 RVA: 0x0004CCEC File Offset: 0x0004BCEC
		internal static Attribute[] GetCustomAttributes(RuntimeType type, Type caType, bool includeSecCa, out int count)
		{
			count = 0;
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			if (!flag && PseudoCustomAttribute.s_pca[caType] == null && !PseudoCustomAttribute.IsSecurityAttribute(caType))
			{
				return new Attribute[0];
			}
			List<Attribute> list = new List<Attribute>();
			if (flag || caType == typeof(SerializableAttribute))
			{
				Attribute customAttribute = SerializableAttribute.GetCustomAttribute(type);
				if (customAttribute != null)
				{
					list.Add(customAttribute);
				}
			}
			if (flag || caType == typeof(ComImportAttribute))
			{
				Attribute customAttribute = ComImportAttribute.GetCustomAttribute(type);
				if (customAttribute != null)
				{
					list.Add(customAttribute);
				}
			}
			if (includeSecCa && (flag || PseudoCustomAttribute.IsSecurityAttribute(caType)) && !type.IsGenericParameter)
			{
				if (type.IsGenericType)
				{
					type = (RuntimeType)type.GetGenericTypeDefinition();
				}
				object[] array;
				PseudoCustomAttribute.GetSecurityAttributes(type.Module.ModuleHandle, type.MetadataToken, out array);
				if (array != null)
				{
					foreach (object obj in array)
					{
						if (caType == obj.GetType() || obj.GetType().IsSubclassOf(caType))
						{
							list.Add((Attribute)obj);
						}
					}
				}
			}
			count = list.Count;
			return list.ToArray();
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x0004CE20 File Offset: 0x0004BE20
		internal static bool IsDefined(RuntimeType type, Type caType)
		{
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			int num;
			return (flag || PseudoCustomAttribute.s_pca[caType] != null || PseudoCustomAttribute.IsSecurityAttribute(caType)) && (((flag || caType == typeof(SerializableAttribute)) && SerializableAttribute.IsDefined(type)) || ((flag || caType == typeof(ComImportAttribute)) && ComImportAttribute.IsDefined(type)) || ((flag || PseudoCustomAttribute.IsSecurityAttribute(caType)) && PseudoCustomAttribute.GetCustomAttributes(type, caType, true, out num).Length != 0));
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x0004CEB8 File Offset: 0x0004BEB8
		internal static Attribute[] GetCustomAttributes(RuntimeMethodInfo method, Type caType, bool includeSecCa, out int count)
		{
			count = 0;
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			if (!flag && PseudoCustomAttribute.s_pca[caType] == null && !PseudoCustomAttribute.IsSecurityAttribute(caType))
			{
				return new Attribute[0];
			}
			List<Attribute> list = new List<Attribute>();
			if (flag || caType == typeof(DllImportAttribute))
			{
				Attribute customAttribute = DllImportAttribute.GetCustomAttribute(method);
				if (customAttribute != null)
				{
					list.Add(customAttribute);
				}
			}
			if (flag || caType == typeof(PreserveSigAttribute))
			{
				Attribute customAttribute = PreserveSigAttribute.GetCustomAttribute(method);
				if (customAttribute != null)
				{
					list.Add(customAttribute);
				}
			}
			if (includeSecCa && (flag || PseudoCustomAttribute.IsSecurityAttribute(caType)))
			{
				object[] array;
				PseudoCustomAttribute.GetSecurityAttributes(method.Module.ModuleHandle, method.MetadataToken, out array);
				if (array != null)
				{
					foreach (object obj in array)
					{
						if (caType == obj.GetType() || obj.GetType().IsSubclassOf(caType))
						{
							list.Add((Attribute)obj);
						}
					}
				}
			}
			count = list.Count;
			return list.ToArray();
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x0004CFCC File Offset: 0x0004BFCC
		internal static bool IsDefined(RuntimeMethodInfo method, Type caType)
		{
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			int num;
			return (flag || PseudoCustomAttribute.s_pca[caType] != null) && (((flag || caType == typeof(DllImportAttribute)) && DllImportAttribute.IsDefined(method)) || ((flag || caType == typeof(PreserveSigAttribute)) && PreserveSigAttribute.IsDefined(method)) || ((flag || PseudoCustomAttribute.IsSecurityAttribute(caType)) && PseudoCustomAttribute.GetCustomAttributes(method, caType, true, out num).Length != 0));
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x0004D05C File Offset: 0x0004C05C
		internal static Attribute[] GetCustomAttributes(ParameterInfo parameter, Type caType, out int count)
		{
			count = 0;
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			if (!flag && PseudoCustomAttribute.s_pca[caType] == null)
			{
				return null;
			}
			Attribute[] array = new Attribute[PseudoCustomAttribute.s_pcasCount];
			if (flag || caType == typeof(InAttribute))
			{
				Attribute customAttribute = InAttribute.GetCustomAttribute(parameter);
				if (customAttribute != null)
				{
					array[count++] = customAttribute;
				}
			}
			if (flag || caType == typeof(OutAttribute))
			{
				Attribute customAttribute = OutAttribute.GetCustomAttribute(parameter);
				if (customAttribute != null)
				{
					array[count++] = customAttribute;
				}
			}
			if (flag || caType == typeof(OptionalAttribute))
			{
				Attribute customAttribute = OptionalAttribute.GetCustomAttribute(parameter);
				if (customAttribute != null)
				{
					array[count++] = customAttribute;
				}
			}
			if (flag || caType == typeof(MarshalAsAttribute))
			{
				Attribute customAttribute = MarshalAsAttribute.GetCustomAttribute(parameter);
				if (customAttribute != null)
				{
					array[count++] = customAttribute;
				}
			}
			return array;
		}

		// Token: 0x06001E92 RID: 7826 RVA: 0x0004D148 File Offset: 0x0004C148
		internal static bool IsDefined(ParameterInfo parameter, Type caType)
		{
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			return (flag || PseudoCustomAttribute.s_pca[caType] != null) && (((flag || caType == typeof(InAttribute)) && InAttribute.IsDefined(parameter)) || ((flag || caType == typeof(OutAttribute)) && OutAttribute.IsDefined(parameter)) || ((flag || caType == typeof(OptionalAttribute)) && OptionalAttribute.IsDefined(parameter)) || ((flag || caType == typeof(MarshalAsAttribute)) && MarshalAsAttribute.IsDefined(parameter)));
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x0004D1F0 File Offset: 0x0004C1F0
		internal static Attribute[] GetCustomAttributes(Assembly assembly, Type caType, out int count)
		{
			count = 0;
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			if (!flag && PseudoCustomAttribute.s_pca[caType] == null && !PseudoCustomAttribute.IsSecurityAttribute(caType))
			{
				return new Attribute[0];
			}
			List<Attribute> list = new List<Attribute>();
			if (flag || PseudoCustomAttribute.IsSecurityAttribute(caType))
			{
				object[] array;
				PseudoCustomAttribute.GetSecurityAttributes(assembly.ManifestModule.ModuleHandle, assembly.AssemblyHandle.GetToken(), out array);
				if (array != null)
				{
					foreach (object obj in array)
					{
						if (caType == obj.GetType() || obj.GetType().IsSubclassOf(caType))
						{
							list.Add((Attribute)obj);
						}
					}
				}
			}
			count = list.Count;
			return list.ToArray();
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x0004D2C0 File Offset: 0x0004C2C0
		internal static bool IsDefined(Assembly assembly, Type caType)
		{
			int num;
			return PseudoCustomAttribute.GetCustomAttributes(assembly, caType, out num).Length > 0;
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x0004D2DB File Offset: 0x0004C2DB
		internal static Attribute[] GetCustomAttributes(Module module, Type caType, out int count)
		{
			count = 0;
			return null;
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x0004D2E1 File Offset: 0x0004C2E1
		internal static bool IsDefined(Module module, Type caType)
		{
			return false;
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x0004D2E4 File Offset: 0x0004C2E4
		internal static Attribute[] GetCustomAttributes(RuntimeFieldInfo field, Type caType, out int count)
		{
			count = 0;
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			if (!flag && PseudoCustomAttribute.s_pca[caType] == null)
			{
				return null;
			}
			Attribute[] array = new Attribute[PseudoCustomAttribute.s_pcasCount];
			if (flag || caType == typeof(MarshalAsAttribute))
			{
				Attribute customAttribute = MarshalAsAttribute.GetCustomAttribute(field);
				if (customAttribute != null)
				{
					array[count++] = customAttribute;
				}
			}
			if (flag || caType == typeof(FieldOffsetAttribute))
			{
				Attribute customAttribute = FieldOffsetAttribute.GetCustomAttribute(field);
				if (customAttribute != null)
				{
					array[count++] = customAttribute;
				}
			}
			if (flag || caType == typeof(NonSerializedAttribute))
			{
				Attribute customAttribute = NonSerializedAttribute.GetCustomAttribute(field);
				if (customAttribute != null)
				{
					array[count++] = customAttribute;
				}
			}
			return array;
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x0004D3A8 File Offset: 0x0004C3A8
		internal static bool IsDefined(RuntimeFieldInfo field, Type caType)
		{
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			return (flag || PseudoCustomAttribute.s_pca[caType] != null) && (((flag || caType == typeof(MarshalAsAttribute)) && MarshalAsAttribute.IsDefined(field)) || ((flag || caType == typeof(FieldOffsetAttribute)) && FieldOffsetAttribute.IsDefined(field)) || ((flag || caType == typeof(NonSerializedAttribute)) && NonSerializedAttribute.IsDefined(field)));
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x0004D434 File Offset: 0x0004C434
		internal static Attribute[] GetCustomAttributes(RuntimeConstructorInfo ctor, Type caType, bool includeSecCa, out int count)
		{
			count = 0;
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			if (!flag && PseudoCustomAttribute.s_pca[caType] == null && !PseudoCustomAttribute.IsSecurityAttribute(caType))
			{
				return new Attribute[0];
			}
			List<Attribute> list = new List<Attribute>();
			if (includeSecCa && (flag || PseudoCustomAttribute.IsSecurityAttribute(caType)))
			{
				object[] array;
				PseudoCustomAttribute.GetSecurityAttributes(ctor.Module.ModuleHandle, ctor.MetadataToken, out array);
				if (array != null)
				{
					foreach (object obj in array)
					{
						if (caType == obj.GetType() || obj.GetType().IsSubclassOf(caType))
						{
							list.Add((Attribute)obj);
						}
					}
				}
			}
			count = list.Count;
			return list.ToArray();
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x0004D500 File Offset: 0x0004C500
		internal static bool IsDefined(RuntimeConstructorInfo ctor, Type caType)
		{
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			int num;
			return (flag || PseudoCustomAttribute.s_pca[caType] != null) && ((flag || PseudoCustomAttribute.IsSecurityAttribute(caType)) && PseudoCustomAttribute.GetCustomAttributes(ctor, caType, true, out num).Length != 0);
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x0004D559 File Offset: 0x0004C559
		internal static Attribute[] GetCustomAttributes(RuntimePropertyInfo property, Type caType, out int count)
		{
			count = 0;
			return null;
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x0004D55F File Offset: 0x0004C55F
		internal static bool IsDefined(RuntimePropertyInfo property, Type caType)
		{
			return false;
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x0004D562 File Offset: 0x0004C562
		internal static Attribute[] GetCustomAttributes(RuntimeEventInfo e, Type caType, out int count)
		{
			count = 0;
			return null;
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x0004D568 File Offset: 0x0004C568
		internal static bool IsDefined(RuntimeEventInfo e, Type caType)
		{
			return false;
		}

		// Token: 0x04000B46 RID: 2886
		private static Hashtable s_pca;

		// Token: 0x04000B47 RID: 2887
		private static int s_pcasCount;
	}
}

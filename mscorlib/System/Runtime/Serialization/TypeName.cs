using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000381 RID: 897
	internal class TypeName
	{
		// Token: 0x06002307 RID: 8967 RVA: 0x000587EF File Offset: 0x000577EF
		private TypeName()
		{
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x000587F8 File Offset: 0x000577F8
		internal static Type GetType(Assembly initialAssembly, string fullTypeName)
		{
			Type typeFromCLSID = Type.GetTypeFromCLSID(new Guid(3089101169U, 8435, 4562, 141, 204, 0, 160, 201, 176, 5, 37));
			TypeName.ITypeNameFactory typeNameFactory = (TypeName.ITypeNameFactory)Activator.CreateInstance(typeFromCLSID);
			int num;
			TypeName.ITypeName typeNameInfo = typeNameFactory.ParseTypeName(fullTypeName, out num);
			Type result = null;
			if (num == -1)
			{
				result = TypeName.LoadTypeWithPartialName(typeNameInfo, initialAssembly, fullTypeName);
			}
			return result;
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x00058868 File Offset: 0x00057868
		private static Type LoadTypeWithPartialName(TypeName.ITypeName typeNameInfo, Assembly initialAssembly, string fullTypeName)
		{
			uint num = typeNameInfo.GetNameCount();
			uint num2 = typeNameInfo.GetTypeArgumentCount();
			IntPtr[] array = new IntPtr[num];
			IntPtr[] array2 = new IntPtr[num2];
			Type result;
			try
			{
				if (num == 0U)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadType"), new object[]
					{
						fullTypeName
					}));
				}
				GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				num = typeNameInfo.GetNames(num, gchandle.AddrOfPinnedObject());
				gchandle.Free();
				string text = Marshal.PtrToStringBSTR(array[0]);
				string assemblyName = typeNameInfo.GetAssemblyName();
				Type type;
				if (!string.IsNullOrEmpty(assemblyName))
				{
					Assembly assembly = Assembly.LoadWithPartialName(assemblyName);
					if (assembly == null)
					{
						assembly = Assembly.LoadWithPartialName(new AssemblyName(assemblyName).Name);
					}
					type = assembly.GetType(text);
				}
				else if (initialAssembly != null)
				{
					type = initialAssembly.GetType(text);
				}
				else
				{
					type = Type.GetType(text);
				}
				if (type == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadType"), new object[]
					{
						fullTypeName
					}));
				}
				int num3 = 1;
				while ((long)num3 < (long)((ulong)num))
				{
					string name = Marshal.PtrToStringBSTR(array[num3]);
					type = type.GetNestedType(name, BindingFlags.Public | BindingFlags.NonPublic);
					if (type == null)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadType"), new object[]
						{
							fullTypeName
						}));
					}
					num3++;
				}
				if (num2 != 0U)
				{
					GCHandle gchandle2 = GCHandle.Alloc(array2, GCHandleType.Pinned);
					num2 = typeNameInfo.GetTypeArguments(num2, gchandle2.AddrOfPinnedObject());
					gchandle2.Free();
					Type[] array3 = new Type[num2];
					int num4 = 0;
					while ((long)num4 < (long)((ulong)num2))
					{
						array3[num4] = TypeName.LoadTypeWithPartialName((TypeName.ITypeName)Marshal.GetObjectForIUnknown(array2[num4]), null, fullTypeName);
						num4++;
					}
					result = type.MakeGenericType(array3);
				}
				else
				{
					result = type;
				}
			}
			finally
			{
				for (int i = 0; i < array.Length; i++)
				{
					IntPtr intPtr = array[i];
					Marshal.FreeBSTR(array[i]);
				}
				for (int j = 0; j < array2.Length; j++)
				{
					IntPtr intPtr2 = array2[j];
					Marshal.Release(array2[j]);
				}
			}
			return result;
		}

		// Token: 0x02000382 RID: 898
		[Guid("B81FF171-20F3-11D2-8DCC-00A0C9B00522")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[TypeLibType(256)]
		[ComImport]
		internal interface ITypeName
		{
			// Token: 0x0600230A RID: 8970
			uint GetNameCount();

			// Token: 0x0600230B RID: 8971
			uint GetNames([In] uint count, IntPtr rgbszNamesArray);

			// Token: 0x0600230C RID: 8972
			uint GetTypeArgumentCount();

			// Token: 0x0600230D RID: 8973
			uint GetTypeArguments([In] uint count, IntPtr rgpArgumentsArray);

			// Token: 0x0600230E RID: 8974
			uint GetModifierLength();

			// Token: 0x0600230F RID: 8975
			uint GetModifiers([In] uint count, out uint rgModifiers);

			// Token: 0x06002310 RID: 8976
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetAssemblyName();
		}

		// Token: 0x02000383 RID: 899
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("B81FF171-20F3-11D2-8DCC-00A0C9B00521")]
		[TypeLibType(256)]
		[ComImport]
		internal interface ITypeNameFactory
		{
			// Token: 0x06002311 RID: 8977
			[return: MarshalAs(UnmanagedType.Interface)]
			TypeName.ITypeName ParseTypeName([MarshalAs(UnmanagedType.LPWStr)] [In] string szName, out int pError);
		}
	}
}

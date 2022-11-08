using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000805 RID: 2053
	internal class TypeNameBuilder
	{
		// Token: 0x060048B7 RID: 18615
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr _CreateTypeNameBuilder();

		// Token: 0x060048B8 RID: 18616
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _ReleaseTypeNameBuilder(IntPtr pAQN);

		// Token: 0x060048B9 RID: 18617
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _OpenGenericArguments(IntPtr tnb);

		// Token: 0x060048BA RID: 18618
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _CloseGenericArguments(IntPtr tnb);

		// Token: 0x060048BB RID: 18619
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _OpenGenericArgument(IntPtr tnb);

		// Token: 0x060048BC RID: 18620
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _CloseGenericArgument(IntPtr tnb);

		// Token: 0x060048BD RID: 18621
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _AddName(IntPtr tnb, string name);

		// Token: 0x060048BE RID: 18622
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _AddPointer(IntPtr tnb);

		// Token: 0x060048BF RID: 18623
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _AddByRef(IntPtr tnb);

		// Token: 0x060048C0 RID: 18624
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _AddSzArray(IntPtr tnb);

		// Token: 0x060048C1 RID: 18625
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _AddArray(IntPtr tnb, int rank);

		// Token: 0x060048C2 RID: 18626
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _AddAssemblySpec(IntPtr tnb, string assemblySpec);

		// Token: 0x060048C3 RID: 18627
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string _ToString(IntPtr tnb);

		// Token: 0x060048C4 RID: 18628
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _Clear(IntPtr tnb);

		// Token: 0x060048C5 RID: 18629 RVA: 0x000FD538 File Offset: 0x000FC538
		internal static string ToString(Type type, TypeNameBuilder.Format format)
		{
			if ((format == TypeNameBuilder.Format.FullName || format == TypeNameBuilder.Format.AssemblyQualifiedName) && !type.IsGenericTypeDefinition && type.ContainsGenericParameters)
			{
				return null;
			}
			TypeNameBuilder typeNameBuilder = new TypeNameBuilder(TypeNameBuilder._CreateTypeNameBuilder());
			typeNameBuilder.Clear();
			typeNameBuilder.ConstructAssemblyQualifiedNameWorker(type, format);
			string result = typeNameBuilder.ToString();
			typeNameBuilder.Dispose();
			return result;
		}

		// Token: 0x060048C6 RID: 18630 RVA: 0x000FD586 File Offset: 0x000FC586
		private TypeNameBuilder(IntPtr typeNameBuilder)
		{
			this.m_typeNameBuilder = typeNameBuilder;
		}

		// Token: 0x060048C7 RID: 18631 RVA: 0x000FD595 File Offset: 0x000FC595
		internal void Dispose()
		{
			TypeNameBuilder._ReleaseTypeNameBuilder(this.m_typeNameBuilder);
		}

		// Token: 0x060048C8 RID: 18632 RVA: 0x000FD5A4 File Offset: 0x000FC5A4
		private void AddElementType(Type elementType)
		{
			if (elementType.HasElementType)
			{
				this.AddElementType(elementType.GetElementType());
			}
			if (elementType.IsPointer)
			{
				this.AddPointer();
				return;
			}
			if (elementType.IsByRef)
			{
				this.AddByRef();
				return;
			}
			if (elementType.IsSzArray)
			{
				this.AddSzArray();
				return;
			}
			if (elementType.IsArray)
			{
				this.AddArray(elementType.GetArrayRank());
			}
		}

		// Token: 0x060048C9 RID: 18633 RVA: 0x000FD608 File Offset: 0x000FC608
		private void ConstructAssemblyQualifiedNameWorker(Type type, TypeNameBuilder.Format format)
		{
			Type type2 = type;
			while (type2.HasElementType)
			{
				type2 = type2.GetElementType();
			}
			List<Type> list = new List<Type>();
			for (Type type3 = type2; type3 != null; type3 = (type3.IsGenericParameter ? null : type3.DeclaringType))
			{
				list.Add(type3);
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				Type type4 = list[i];
				string text = type4.Name;
				if (i == list.Count - 1 && type4.Namespace != null && type4.Namespace.Length != 0)
				{
					text = type4.Namespace + "." + text;
				}
				this.AddName(text);
			}
			if (type2.IsGenericType && (!type2.IsGenericTypeDefinition || format == TypeNameBuilder.Format.ToString))
			{
				Type[] genericArguments = type2.GetGenericArguments();
				this.OpenGenericArguments();
				for (int j = 0; j < genericArguments.Length; j++)
				{
					TypeNameBuilder.Format format2 = (format == TypeNameBuilder.Format.FullName) ? TypeNameBuilder.Format.AssemblyQualifiedName : format;
					this.OpenGenericArgument();
					this.ConstructAssemblyQualifiedNameWorker(genericArguments[j], format2);
					this.CloseGenericArgument();
				}
				this.CloseGenericArguments();
			}
			this.AddElementType(type);
			if (format == TypeNameBuilder.Format.AssemblyQualifiedName)
			{
				this.AddAssemblySpec(type.Module.Assembly.FullName);
			}
		}

		// Token: 0x060048CA RID: 18634 RVA: 0x000FD730 File Offset: 0x000FC730
		private void OpenGenericArguments()
		{
			TypeNameBuilder._OpenGenericArguments(this.m_typeNameBuilder);
		}

		// Token: 0x060048CB RID: 18635 RVA: 0x000FD73D File Offset: 0x000FC73D
		private void CloseGenericArguments()
		{
			TypeNameBuilder._CloseGenericArguments(this.m_typeNameBuilder);
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x000FD74A File Offset: 0x000FC74A
		private void OpenGenericArgument()
		{
			TypeNameBuilder._OpenGenericArgument(this.m_typeNameBuilder);
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x000FD757 File Offset: 0x000FC757
		private void CloseGenericArgument()
		{
			TypeNameBuilder._CloseGenericArgument(this.m_typeNameBuilder);
		}

		// Token: 0x060048CE RID: 18638 RVA: 0x000FD764 File Offset: 0x000FC764
		private void AddName(string name)
		{
			TypeNameBuilder._AddName(this.m_typeNameBuilder, name);
		}

		// Token: 0x060048CF RID: 18639 RVA: 0x000FD772 File Offset: 0x000FC772
		private void AddPointer()
		{
			TypeNameBuilder._AddPointer(this.m_typeNameBuilder);
		}

		// Token: 0x060048D0 RID: 18640 RVA: 0x000FD77F File Offset: 0x000FC77F
		private void AddByRef()
		{
			TypeNameBuilder._AddByRef(this.m_typeNameBuilder);
		}

		// Token: 0x060048D1 RID: 18641 RVA: 0x000FD78C File Offset: 0x000FC78C
		private void AddSzArray()
		{
			TypeNameBuilder._AddSzArray(this.m_typeNameBuilder);
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x000FD799 File Offset: 0x000FC799
		private void AddArray(int rank)
		{
			TypeNameBuilder._AddArray(this.m_typeNameBuilder, rank);
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x000FD7A7 File Offset: 0x000FC7A7
		private void AddAssemblySpec(string assemblySpec)
		{
			TypeNameBuilder._AddAssemblySpec(this.m_typeNameBuilder, assemblySpec);
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x000FD7B5 File Offset: 0x000FC7B5
		public override string ToString()
		{
			return TypeNameBuilder._ToString(this.m_typeNameBuilder);
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x000FD7C2 File Offset: 0x000FC7C2
		private void Clear()
		{
			TypeNameBuilder._Clear(this.m_typeNameBuilder);
		}

		// Token: 0x0400258A RID: 9610
		private IntPtr m_typeNameBuilder;

		// Token: 0x02000806 RID: 2054
		internal enum Format
		{
			// Token: 0x0400258C RID: 9612
			ToString,
			// Token: 0x0400258D RID: 9613
			FullName,
			// Token: 0x0400258E RID: 9614
			AssemblyQualifiedName
		}
	}
}

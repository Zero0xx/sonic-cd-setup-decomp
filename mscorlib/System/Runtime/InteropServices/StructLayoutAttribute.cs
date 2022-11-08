using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004FD RID: 1277
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public sealed class StructLayoutAttribute : Attribute
	{
		// Token: 0x06003185 RID: 12677 RVA: 0x000A96C8 File Offset: 0x000A86C8
		internal static Attribute GetCustomAttribute(Type type)
		{
			if (!StructLayoutAttribute.IsDefined(type))
			{
				return null;
			}
			int num = 0;
			int size = 0;
			LayoutKind layoutKind = LayoutKind.Auto;
			TypeAttributes typeAttributes = type.Attributes & TypeAttributes.LayoutMask;
			if (typeAttributes != TypeAttributes.NotPublic)
			{
				if (typeAttributes != TypeAttributes.SequentialLayout)
				{
					if (typeAttributes == TypeAttributes.ExplicitLayout)
					{
						layoutKind = LayoutKind.Explicit;
					}
				}
				else
				{
					layoutKind = LayoutKind.Sequential;
				}
			}
			else
			{
				layoutKind = LayoutKind.Auto;
			}
			CharSet charSet = CharSet.None;
			TypeAttributes typeAttributes2 = type.Attributes & TypeAttributes.StringFormatMask;
			if (typeAttributes2 != TypeAttributes.NotPublic)
			{
				if (typeAttributes2 != TypeAttributes.UnicodeClass)
				{
					if (typeAttributes2 == TypeAttributes.AutoClass)
					{
						charSet = CharSet.Auto;
					}
				}
				else
				{
					charSet = CharSet.Unicode;
				}
			}
			else
			{
				charSet = CharSet.Ansi;
			}
			type.Module.MetadataImport.GetClassLayout(type.MetadataToken, out num, out size);
			if (num == 0)
			{
				num = 8;
			}
			return new StructLayoutAttribute(layoutKind, num, size, charSet);
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x000A9769 File Offset: 0x000A8769
		internal static bool IsDefined(Type type)
		{
			return !type.IsInterface && !type.HasElementType && !type.IsGenericParameter;
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x000A9786 File Offset: 0x000A8786
		internal StructLayoutAttribute(LayoutKind layoutKind, int pack, int size, CharSet charSet)
		{
			this._val = layoutKind;
			this.Pack = pack;
			this.Size = size;
			this.CharSet = charSet;
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x000A97AB File Offset: 0x000A87AB
		public StructLayoutAttribute(LayoutKind layoutKind)
		{
			this._val = layoutKind;
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x000A97BA File Offset: 0x000A87BA
		public StructLayoutAttribute(short layoutKind)
		{
			this._val = (LayoutKind)layoutKind;
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x0600318A RID: 12682 RVA: 0x000A97C9 File Offset: 0x000A87C9
		public LayoutKind Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x0400199B RID: 6555
		private const int DEFAULT_PACKING_SIZE = 8;

		// Token: 0x0400199C RID: 6556
		internal LayoutKind _val;

		// Token: 0x0400199D RID: 6557
		public int Pack;

		// Token: 0x0400199E RID: 6558
		public int Size;

		// Token: 0x0400199F RID: 6559
		public CharSet CharSet;
	}
}

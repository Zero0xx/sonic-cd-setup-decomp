using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004FE RID: 1278
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	public sealed class FieldOffsetAttribute : Attribute
	{
		// Token: 0x0600318B RID: 12683 RVA: 0x000A97D4 File Offset: 0x000A87D4
		internal static Attribute GetCustomAttribute(RuntimeFieldInfo field)
		{
			int offset;
			if (field.DeclaringType != null && field.Module.MetadataImport.GetFieldOffset(field.DeclaringType.MetadataToken, field.MetadataToken, out offset))
			{
				return new FieldOffsetAttribute(offset);
			}
			return null;
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x000A9819 File Offset: 0x000A8819
		internal static bool IsDefined(RuntimeFieldInfo field)
		{
			return FieldOffsetAttribute.GetCustomAttribute(field) != null;
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x000A9827 File Offset: 0x000A8827
		public FieldOffsetAttribute(int offset)
		{
			this._val = offset;
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x0600318E RID: 12686 RVA: 0x000A9836 File Offset: 0x000A8836
		public int Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x040019A0 RID: 6560
		internal int _val;
	}
}

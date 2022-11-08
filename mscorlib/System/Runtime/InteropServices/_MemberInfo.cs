using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020000F3 RID: 243
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(MemberInfo))]
	[ComVisible(true)]
	[Guid("f7102fa9-cabb-3a74-a6da-b4567ef1b079")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface _MemberInfo
	{
		// Token: 0x06000CA0 RID: 3232
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06000CA1 RID: 3233
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06000CA2 RID: 3234
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06000CA3 RID: 3235
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06000CA4 RID: 3236
		string ToString();

		// Token: 0x06000CA5 RID: 3237
		bool Equals(object other);

		// Token: 0x06000CA6 RID: 3238
		int GetHashCode();

		// Token: 0x06000CA7 RID: 3239
		Type GetType();

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000CA8 RID: 3240
		MemberTypes MemberType { get; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000CA9 RID: 3241
		string Name { get; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000CAA RID: 3242
		Type DeclaringType { get; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000CAB RID: 3243
		Type ReflectedType { get; }

		// Token: 0x06000CAC RID: 3244
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06000CAD RID: 3245
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06000CAE RID: 3246
		bool IsDefined(Type attributeType, bool inherit);
	}
}

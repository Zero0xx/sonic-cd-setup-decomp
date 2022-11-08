using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002FB RID: 763
	[TypeLibImportClass(typeof(FieldInfo))]
	[Guid("8A7C1442-A9FB-366B-80D8-4939FFA6DBE0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[ComVisible(true)]
	public interface _FieldInfo
	{
		// Token: 0x06001DC6 RID: 7622
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06001DC7 RID: 7623
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06001DC8 RID: 7624
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06001DC9 RID: 7625
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06001DCA RID: 7626
		string ToString();

		// Token: 0x06001DCB RID: 7627
		bool Equals(object other);

		// Token: 0x06001DCC RID: 7628
		int GetHashCode();

		// Token: 0x06001DCD RID: 7629
		Type GetType();

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001DCE RID: 7630
		MemberTypes MemberType { get; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001DCF RID: 7631
		string Name { get; }

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001DD0 RID: 7632
		Type DeclaringType { get; }

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001DD1 RID: 7633
		Type ReflectedType { get; }

		// Token: 0x06001DD2 RID: 7634
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06001DD3 RID: 7635
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06001DD4 RID: 7636
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06001DD5 RID: 7637
		Type FieldType { get; }

		// Token: 0x06001DD6 RID: 7638
		object GetValue(object obj);

		// Token: 0x06001DD7 RID: 7639
		object GetValueDirect(TypedReference obj);

		// Token: 0x06001DD8 RID: 7640
		void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture);

		// Token: 0x06001DD9 RID: 7641
		void SetValueDirect(TypedReference obj, object value);

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06001DDA RID: 7642
		RuntimeFieldHandle FieldHandle { get; }

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06001DDB RID: 7643
		FieldAttributes Attributes { get; }

		// Token: 0x06001DDC RID: 7644
		void SetValue(object obj, object value);

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001DDD RID: 7645
		bool IsPublic { get; }

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001DDE RID: 7646
		bool IsPrivate { get; }

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001DDF RID: 7647
		bool IsFamily { get; }

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001DE0 RID: 7648
		bool IsAssembly { get; }

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001DE1 RID: 7649
		bool IsFamilyAndAssembly { get; }

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001DE2 RID: 7650
		bool IsFamilyOrAssembly { get; }

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001DE3 RID: 7651
		bool IsStatic { get; }

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06001DE4 RID: 7652
		bool IsInitOnly { get; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001DE5 RID: 7653
		bool IsLiteral { get; }

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001DE6 RID: 7654
		bool IsNotSerialized { get; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001DE7 RID: 7655
		bool IsSpecialName { get; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001DE8 RID: 7656
		bool IsPinvokeImpl { get; }
	}
}

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x02000347 RID: 839
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_ConstructorInfo))]
	[ComVisible(true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public abstract class ConstructorInfo : MethodBase, _ConstructorInfo
	{
		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x000506E4 File Offset: 0x0004F6E4
		[ComVisible(true)]
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Constructor;
			}
		}

		// Token: 0x06002044 RID: 8260
		public abstract object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x06002045 RID: 8261 RVA: 0x000506E7 File Offset: 0x0004F6E7
		internal override Type GetReturnType()
		{
			return this.DeclaringType;
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x000506EF File Offset: 0x0004F6EF
		[DebuggerStepThrough]
		[DebuggerHidden]
		public object Invoke(object[] parameters)
		{
			return this.Invoke(BindingFlags.Default, null, parameters, null);
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x000506FB File Offset: 0x0004F6FB
		Type _ConstructorInfo.GetType()
		{
			return base.GetType();
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x00050703 File Offset: 0x0004F703
		object _ConstructorInfo.Invoke_2(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.Invoke(obj, invokeAttr, binder, parameters, culture);
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x00050712 File Offset: 0x0004F712
		object _ConstructorInfo.Invoke_3(object obj, object[] parameters)
		{
			return base.Invoke(obj, parameters);
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x0005071C File Offset: 0x0004F71C
		object _ConstructorInfo.Invoke_4(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.Invoke(invokeAttr, binder, parameters, culture);
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x00050729 File Offset: 0x0004F729
		object _ConstructorInfo.Invoke_5(object[] parameters)
		{
			return this.Invoke(parameters);
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x00050732 File Offset: 0x0004F732
		void _ConstructorInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x00050739 File Offset: 0x0004F739
		void _ConstructorInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x00050740 File Offset: 0x0004F740
		void _ConstructorInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x00050747 File Offset: 0x0004F747
		void _ConstructorInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000DE3 RID: 3555
		[ComVisible(true)]
		public static readonly string ConstructorName = ".ctor";

		// Token: 0x04000DE4 RID: 3556
		[ComVisible(true)]
		public static readonly string TypeConstructorName = ".cctor";
	}
}

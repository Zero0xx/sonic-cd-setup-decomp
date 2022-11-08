using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x0200034B RID: 843
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_PropertyInfo))]
	[ComVisible(true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public abstract class PropertyInfo : MemberInfo, _PropertyInfo
	{
		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x0600209A RID: 8346 RVA: 0x00050B2E File Offset: 0x0004FB2E
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Property;
			}
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x00050B32 File Offset: 0x0004FB32
		public virtual object GetConstantValue()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x00050B39 File Offset: 0x0004FB39
		public virtual object GetRawConstantValue()
		{
			throw new NotImplementedException();
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x0600209D RID: 8349
		public abstract Type PropertyType { get; }

		// Token: 0x0600209E RID: 8350
		public abstract void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		// Token: 0x0600209F RID: 8351
		public abstract MethodInfo[] GetAccessors(bool nonPublic);

		// Token: 0x060020A0 RID: 8352
		public abstract MethodInfo GetGetMethod(bool nonPublic);

		// Token: 0x060020A1 RID: 8353
		public abstract MethodInfo GetSetMethod(bool nonPublic);

		// Token: 0x060020A2 RID: 8354
		public abstract ParameterInfo[] GetIndexParameters();

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x060020A3 RID: 8355
		public abstract PropertyAttributes Attributes { get; }

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x060020A4 RID: 8356
		public abstract bool CanRead { get; }

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x060020A5 RID: 8357
		public abstract bool CanWrite { get; }

		// Token: 0x060020A6 RID: 8358 RVA: 0x00050B40 File Offset: 0x0004FB40
		[DebuggerHidden]
		[DebuggerStepThrough]
		public virtual object GetValue(object obj, object[] index)
		{
			return this.GetValue(obj, BindingFlags.Default, null, index, null);
		}

		// Token: 0x060020A7 RID: 8359
		public abstract object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		// Token: 0x060020A8 RID: 8360 RVA: 0x00050B4D File Offset: 0x0004FB4D
		[DebuggerStepThrough]
		[DebuggerHidden]
		public virtual void SetValue(object obj, object value, object[] index)
		{
			this.SetValue(obj, value, BindingFlags.Default, null, index, null);
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x00050B5B File Offset: 0x0004FB5B
		public virtual Type[] GetRequiredCustomModifiers()
		{
			return new Type[0];
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x00050B63 File Offset: 0x0004FB63
		public virtual Type[] GetOptionalCustomModifiers()
		{
			return new Type[0];
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x00050B6B File Offset: 0x0004FB6B
		public MethodInfo[] GetAccessors()
		{
			return this.GetAccessors(false);
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x00050B74 File Offset: 0x0004FB74
		public MethodInfo GetGetMethod()
		{
			return this.GetGetMethod(false);
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x00050B7D File Offset: 0x0004FB7D
		public MethodInfo GetSetMethod()
		{
			return this.GetSetMethod(false);
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x060020AE RID: 8366 RVA: 0x00050B86 File Offset: 0x0004FB86
		public bool IsSpecialName
		{
			get
			{
				return (this.Attributes & PropertyAttributes.SpecialName) != PropertyAttributes.None;
			}
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x00050B9A File Offset: 0x0004FB9A
		Type _PropertyInfo.GetType()
		{
			return base.GetType();
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x00050BA2 File Offset: 0x0004FBA2
		void _PropertyInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x00050BA9 File Offset: 0x0004FBA9
		void _PropertyInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x00050BB0 File Offset: 0x0004FBB0
		void _PropertyInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x00050BB7 File Offset: 0x0004FBB7
		void _PropertyInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}
	}
}

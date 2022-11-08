using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x02000349 RID: 841
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_FieldInfo))]
	[ComVisible(true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public abstract class FieldInfo : MemberInfo, _FieldInfo
	{
		// Token: 0x06002064 RID: 8292 RVA: 0x000507EC File Offset: 0x0004F7EC
		public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle)
		{
			if (handle.IsNullHandle())
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidHandle"));
			}
			FieldInfo fieldInfo = RuntimeType.GetFieldInfo(handle);
			if (fieldInfo.DeclaringType != null && fieldInfo.DeclaringType.IsGenericType)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_FieldDeclaringTypeGeneric"), new object[]
				{
					fieldInfo.Name,
					fieldInfo.DeclaringType.GetGenericTypeDefinition()
				}));
			}
			return fieldInfo;
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x00050868 File Offset: 0x0004F868
		[ComVisible(false)]
		public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle, RuntimeTypeHandle declaringType)
		{
			if (handle.IsNullHandle())
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidHandle"));
			}
			return RuntimeType.GetFieldInfo(declaringType, handle);
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06002067 RID: 8295 RVA: 0x00050892 File Offset: 0x0004F892
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Field;
			}
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x00050895 File Offset: 0x0004F895
		public virtual Type[] GetRequiredCustomModifiers()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x0005089C File Offset: 0x0004F89C
		public virtual Type[] GetOptionalCustomModifiers()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x000508A3 File Offset: 0x0004F8A3
		[CLSCompliant(false)]
		public virtual void SetValueDirect(TypedReference obj, object value)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_AbstractNonCLS"));
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x000508B4 File Offset: 0x0004F8B4
		[CLSCompliant(false)]
		public virtual object GetValueDirect(TypedReference obj)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_AbstractNonCLS"));
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x0600206C RID: 8300
		public abstract RuntimeFieldHandle FieldHandle { get; }

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x0600206D RID: 8301
		public abstract Type FieldType { get; }

		// Token: 0x0600206E RID: 8302
		public abstract object GetValue(object obj);

		// Token: 0x0600206F RID: 8303 RVA: 0x000508C5 File Offset: 0x0004F8C5
		public virtual object GetRawConstantValue()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_AbstractNonCLS"));
		}

		// Token: 0x06002070 RID: 8304
		public abstract void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture);

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06002071 RID: 8305
		public abstract FieldAttributes Attributes { get; }

		// Token: 0x06002072 RID: 8306 RVA: 0x000508D6 File Offset: 0x0004F8D6
		[DebuggerStepThrough]
		[DebuggerHidden]
		public void SetValue(object obj, object value)
		{
			this.SetValue(obj, value, BindingFlags.Default, Type.DefaultBinder, null);
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06002073 RID: 8307 RVA: 0x000508E7 File Offset: 0x0004F8E7
		public bool IsPublic
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06002074 RID: 8308 RVA: 0x000508F4 File Offset: 0x0004F8F4
		public bool IsPrivate
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Private;
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06002075 RID: 8309 RVA: 0x00050901 File Offset: 0x0004F901
		public bool IsFamily
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Family;
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06002076 RID: 8310 RVA: 0x0005090E File Offset: 0x0004F90E
		public bool IsAssembly
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Assembly;
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06002077 RID: 8311 RVA: 0x0005091B File Offset: 0x0004F91B
		public bool IsFamilyAndAssembly
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamANDAssem;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06002078 RID: 8312 RVA: 0x00050928 File Offset: 0x0004F928
		public bool IsFamilyOrAssembly
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamORAssem;
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06002079 RID: 8313 RVA: 0x00050935 File Offset: 0x0004F935
		public bool IsStatic
		{
			get
			{
				return (this.Attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x0600207A RID: 8314 RVA: 0x00050946 File Offset: 0x0004F946
		public bool IsInitOnly
		{
			get
			{
				return (this.Attributes & FieldAttributes.InitOnly) != FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x0600207B RID: 8315 RVA: 0x00050957 File Offset: 0x0004F957
		public bool IsLiteral
		{
			get
			{
				return (this.Attributes & FieldAttributes.Literal) != FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x0600207C RID: 8316 RVA: 0x00050968 File Offset: 0x0004F968
		public bool IsNotSerialized
		{
			get
			{
				return (this.Attributes & FieldAttributes.NotSerialized) != FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x0600207D RID: 8317 RVA: 0x0005097C File Offset: 0x0004F97C
		public bool IsSpecialName
		{
			get
			{
				return (this.Attributes & FieldAttributes.SpecialName) != FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x0600207E RID: 8318 RVA: 0x00050990 File Offset: 0x0004F990
		public bool IsPinvokeImpl
		{
			get
			{
				return (this.Attributes & FieldAttributes.PinvokeImpl) != FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x000509A4 File Offset: 0x0004F9A4
		Type _FieldInfo.GetType()
		{
			return base.GetType();
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x000509AC File Offset: 0x0004F9AC
		void _FieldInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x000509B3 File Offset: 0x0004F9B3
		void _FieldInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x000509BA File Offset: 0x0004F9BA
		void _FieldInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x000509C1 File Offset: 0x0004F9C1
		void _FieldInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}
	}
}

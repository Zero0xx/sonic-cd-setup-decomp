using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x02000348 RID: 840
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_MethodInfo))]
	[ComVisible(true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public abstract class MethodInfo : MethodBase, _MethodInfo
	{
		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06002052 RID: 8274 RVA: 0x0005076C File Offset: 0x0004F76C
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Method;
			}
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x0005076F File Offset: 0x0004F76F
		internal virtual MethodInfo GetParentDefinition()
		{
			return null;
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06002054 RID: 8276 RVA: 0x00050772 File Offset: 0x0004F772
		public virtual Type ReturnType
		{
			get
			{
				return this.GetReturnType();
			}
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x0005077A File Offset: 0x0004F77A
		internal override Type GetReturnType()
		{
			return this.ReturnType;
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06002056 RID: 8278 RVA: 0x00050782 File Offset: 0x0004F782
		public virtual ParameterInfo ReturnParameter
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06002057 RID: 8279
		public abstract ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

		// Token: 0x06002058 RID: 8280
		public abstract MethodInfo GetBaseDefinition();

		// Token: 0x06002059 RID: 8281 RVA: 0x00050789 File Offset: 0x0004F789
		[ComVisible(true)]
		public override Type[] GetGenericArguments()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x0005079A File Offset: 0x0004F79A
		[ComVisible(true)]
		public virtual MethodInfo GetGenericMethodDefinition()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x0600205B RID: 8283 RVA: 0x000507AB File Offset: 0x0004F7AB
		public override bool IsGenericMethodDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x0600205C RID: 8284 RVA: 0x000507AE File Offset: 0x0004F7AE
		public override bool ContainsGenericParameters
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x000507B1 File Offset: 0x0004F7B1
		public virtual MethodInfo MakeGenericMethod(params Type[] typeArguments)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SubclassOverride"));
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x0600205E RID: 8286 RVA: 0x000507C2 File Offset: 0x0004F7C2
		public override bool IsGenericMethod
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x000507C5 File Offset: 0x0004F7C5
		Type _MethodInfo.GetType()
		{
			return base.GetType();
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x000507CD File Offset: 0x0004F7CD
		void _MethodInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x000507D4 File Offset: 0x0004F7D4
		void _MethodInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x000507DB File Offset: 0x0004F7DB
		void _MethodInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x000507E2 File Offset: 0x0004F7E2
		void _MethodInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}
	}
}

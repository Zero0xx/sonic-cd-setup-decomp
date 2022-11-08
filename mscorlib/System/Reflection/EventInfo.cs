using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x0200034A RID: 842
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_EventInfo))]
	[ComVisible(true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public abstract class EventInfo : MemberInfo, _EventInfo
	{
		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06002085 RID: 8325 RVA: 0x000509D0 File Offset: 0x0004F9D0
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Event;
			}
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x000509D3 File Offset: 0x0004F9D3
		public virtual MethodInfo[] GetOtherMethods(bool nonPublic)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002087 RID: 8327
		public abstract MethodInfo GetAddMethod(bool nonPublic);

		// Token: 0x06002088 RID: 8328
		public abstract MethodInfo GetRemoveMethod(bool nonPublic);

		// Token: 0x06002089 RID: 8329
		public abstract MethodInfo GetRaiseMethod(bool nonPublic);

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x0600208A RID: 8330
		public abstract EventAttributes Attributes { get; }

		// Token: 0x0600208B RID: 8331 RVA: 0x000509DA File Offset: 0x0004F9DA
		public MethodInfo[] GetOtherMethods()
		{
			return this.GetOtherMethods(false);
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x000509E3 File Offset: 0x0004F9E3
		public MethodInfo GetAddMethod()
		{
			return this.GetAddMethod(false);
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x000509EC File Offset: 0x0004F9EC
		public MethodInfo GetRemoveMethod()
		{
			return this.GetRemoveMethod(false);
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x000509F5 File Offset: 0x0004F9F5
		public MethodInfo GetRaiseMethod()
		{
			return this.GetRaiseMethod(false);
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x00050A00 File Offset: 0x0004FA00
		[DebuggerStepThrough]
		[DebuggerHidden]
		public void AddEventHandler(object target, Delegate handler)
		{
			MethodInfo addMethod = this.GetAddMethod();
			if (addMethod == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NoPublicAddMethod"));
			}
			addMethod.Invoke(target, new object[]
			{
				handler
			});
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x00050A3C File Offset: 0x0004FA3C
		[DebuggerHidden]
		[DebuggerStepThrough]
		public void RemoveEventHandler(object target, Delegate handler)
		{
			MethodInfo removeMethod = this.GetRemoveMethod();
			if (removeMethod == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NoPublicRemoveMethod"));
			}
			removeMethod.Invoke(target, new object[]
			{
				handler
			});
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06002091 RID: 8337 RVA: 0x00050A78 File Offset: 0x0004FA78
		public Type EventHandlerType
		{
			get
			{
				MethodInfo addMethod = this.GetAddMethod(true);
				ParameterInfo[] parametersNoCopy = addMethod.GetParametersNoCopy();
				Type typeFromHandle = typeof(Delegate);
				for (int i = 0; i < parametersNoCopy.Length; i++)
				{
					Type parameterType = parametersNoCopy[i].ParameterType;
					if (parameterType.IsSubclassOf(typeFromHandle))
					{
						return parameterType;
					}
				}
				return null;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06002092 RID: 8338 RVA: 0x00050AC5 File Offset: 0x0004FAC5
		public bool IsSpecialName
		{
			get
			{
				return (this.Attributes & EventAttributes.SpecialName) != EventAttributes.None;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06002093 RID: 8339 RVA: 0x00050ADC File Offset: 0x0004FADC
		public bool IsMulticast
		{
			get
			{
				Type eventHandlerType = this.EventHandlerType;
				Type typeFromHandle = typeof(MulticastDelegate);
				return typeFromHandle.IsAssignableFrom(eventHandlerType);
			}
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x00050B02 File Offset: 0x0004FB02
		Type _EventInfo.GetType()
		{
			return base.GetType();
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x00050B0A File Offset: 0x0004FB0A
		void _EventInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x00050B11 File Offset: 0x0004FB11
		void _EventInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x00050B18 File Offset: 0x0004FB18
		void _EventInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x00050B1F File Offset: 0x0004FB1F
		void _EventInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}
	}
}

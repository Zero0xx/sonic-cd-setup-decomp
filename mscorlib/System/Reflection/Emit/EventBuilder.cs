using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	// Token: 0x02000823 RID: 2083
	[ComDefaultInterface(typeof(_EventBuilder))]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class EventBuilder : _EventBuilder
	{
		// Token: 0x06004A1B RID: 18971 RVA: 0x00101B51 File Offset: 0x00100B51
		private EventBuilder()
		{
		}

		// Token: 0x06004A1C RID: 18972 RVA: 0x00101B59 File Offset: 0x00100B59
		internal EventBuilder(Module mod, string name, EventAttributes attr, int eventType, TypeBuilder type, EventToken evToken)
		{
			this.m_name = name;
			this.m_module = mod;
			this.m_attributes = attr;
			this.m_evToken = evToken;
			this.m_type = type;
		}

		// Token: 0x06004A1D RID: 18973 RVA: 0x00101B86 File Offset: 0x00100B86
		public EventToken GetEventToken()
		{
			return this.m_evToken;
		}

		// Token: 0x06004A1E RID: 18974 RVA: 0x00101B90 File Offset: 0x00100B90
		public void SetAddOnMethod(MethodBuilder mdBuilder)
		{
			if (mdBuilder == null)
			{
				throw new ArgumentNullException("mdBuilder");
			}
			this.m_type.ThrowIfCreated();
			TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_evToken.Token, MethodSemanticsAttributes.AddOn, mdBuilder.GetToken().Token);
		}

		// Token: 0x06004A1F RID: 18975 RVA: 0x00101BDC File Offset: 0x00100BDC
		public void SetRemoveOnMethod(MethodBuilder mdBuilder)
		{
			if (mdBuilder == null)
			{
				throw new ArgumentNullException("mdBuilder");
			}
			this.m_type.ThrowIfCreated();
			TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_evToken.Token, MethodSemanticsAttributes.RemoveOn, mdBuilder.GetToken().Token);
		}

		// Token: 0x06004A20 RID: 18976 RVA: 0x00101C28 File Offset: 0x00100C28
		public void SetRaiseMethod(MethodBuilder mdBuilder)
		{
			if (mdBuilder == null)
			{
				throw new ArgumentNullException("mdBuilder");
			}
			this.m_type.ThrowIfCreated();
			TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_evToken.Token, MethodSemanticsAttributes.Fire, mdBuilder.GetToken().Token);
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x00101C74 File Offset: 0x00100C74
		public void AddOtherMethod(MethodBuilder mdBuilder)
		{
			if (mdBuilder == null)
			{
				throw new ArgumentNullException("mdBuilder");
			}
			this.m_type.ThrowIfCreated();
			TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_evToken.Token, MethodSemanticsAttributes.Other, mdBuilder.GetToken().Token);
		}

		// Token: 0x06004A22 RID: 18978 RVA: 0x00101CC0 File Offset: 0x00100CC0
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			this.m_type.ThrowIfCreated();
			TypeBuilder.InternalCreateCustomAttribute(this.m_evToken.Token, ((ModuleBuilder)this.m_module).GetConstructorToken(con).Token, binaryAttribute, this.m_module, false);
		}

		// Token: 0x06004A23 RID: 18979 RVA: 0x00101D25 File Offset: 0x00100D25
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			this.m_type.ThrowIfCreated();
			customBuilder.CreateCustomAttribute((ModuleBuilder)this.m_module, this.m_evToken.Token);
		}

		// Token: 0x06004A24 RID: 18980 RVA: 0x00101D5C File Offset: 0x00100D5C
		void _EventBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004A25 RID: 18981 RVA: 0x00101D63 File Offset: 0x00100D63
		void _EventBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004A26 RID: 18982 RVA: 0x00101D6A File Offset: 0x00100D6A
		void _EventBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004A27 RID: 18983 RVA: 0x00101D71 File Offset: 0x00100D71
		void _EventBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040025DF RID: 9695
		private string m_name;

		// Token: 0x040025E0 RID: 9696
		private EventToken m_evToken;

		// Token: 0x040025E1 RID: 9697
		private Module m_module;

		// Token: 0x040025E2 RID: 9698
		private EventAttributes m_attributes;

		// Token: 0x040025E3 RID: 9699
		private TypeBuilder m_type;
	}
}

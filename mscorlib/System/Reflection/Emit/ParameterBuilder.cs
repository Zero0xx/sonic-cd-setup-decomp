using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000843 RID: 2115
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_ParameterBuilder))]
	[ComVisible(true)]
	public class ParameterBuilder : _ParameterBuilder
	{
		// Token: 0x06004BE3 RID: 19427 RVA: 0x0010A21C File Offset: 0x0010921C
		[Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual void SetMarshal(UnmanagedMarshal unmanagedMarshal)
		{
			if (unmanagedMarshal == null)
			{
				throw new ArgumentNullException("unmanagedMarshal");
			}
			byte[] array = unmanagedMarshal.InternalGetBytes();
			TypeBuilder.InternalSetMarshalInfo(this.m_methodBuilder.GetModule(), this.m_pdToken.Token, array, array.Length);
		}

		// Token: 0x06004BE4 RID: 19428 RVA: 0x0010A260 File Offset: 0x00109260
		public virtual void SetConstant(object defaultValue)
		{
			TypeBuilder.SetConstantValue(this.m_methodBuilder.GetModule(), this.m_pdToken.Token, (this.m_iPosition == 0) ? this.m_methodBuilder.m_returnType : this.m_methodBuilder.m_parameterTypes[this.m_iPosition - 1], defaultValue);
		}

		// Token: 0x06004BE5 RID: 19429 RVA: 0x0010A2B4 File Offset: 0x001092B4
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
			TypeBuilder.InternalCreateCustomAttribute(this.m_pdToken.Token, ((ModuleBuilder)this.m_methodBuilder.GetModule()).GetConstructorToken(con).Token, binaryAttribute, this.m_methodBuilder.GetModule(), false);
		}

		// Token: 0x06004BE6 RID: 19430 RVA: 0x0010A318 File Offset: 0x00109318
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			customBuilder.CreateCustomAttribute((ModuleBuilder)this.m_methodBuilder.GetModule(), this.m_pdToken.Token);
		}

		// Token: 0x06004BE7 RID: 19431 RVA: 0x0010A349 File Offset: 0x00109349
		private ParameterBuilder()
		{
		}

		// Token: 0x06004BE8 RID: 19432 RVA: 0x0010A354 File Offset: 0x00109354
		internal ParameterBuilder(MethodBuilder methodBuilder, int sequence, ParameterAttributes attributes, string strParamName)
		{
			this.m_iPosition = sequence;
			this.m_strParamName = strParamName;
			this.m_methodBuilder = methodBuilder;
			this.m_strParamName = strParamName;
			this.m_attributes = attributes;
			this.m_pdToken = new ParameterToken(TypeBuilder.InternalSetParamInfo(this.m_methodBuilder.GetModule(), this.m_methodBuilder.GetToken().Token, sequence, attributes, strParamName));
		}

		// Token: 0x06004BE9 RID: 19433 RVA: 0x0010A3BE File Offset: 0x001093BE
		public virtual ParameterToken GetToken()
		{
			return this.m_pdToken;
		}

		// Token: 0x06004BEA RID: 19434 RVA: 0x0010A3C6 File Offset: 0x001093C6
		void _ParameterBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BEB RID: 19435 RVA: 0x0010A3CD File Offset: 0x001093CD
		void _ParameterBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BEC RID: 19436 RVA: 0x0010A3D4 File Offset: 0x001093D4
		void _ParameterBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004BED RID: 19437 RVA: 0x0010A3DB File Offset: 0x001093DB
		void _ParameterBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06004BEE RID: 19438 RVA: 0x0010A3E2 File Offset: 0x001093E2
		internal virtual int MetadataTokenInternal
		{
			get
			{
				return this.m_pdToken.Token;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06004BEF RID: 19439 RVA: 0x0010A3EF File Offset: 0x001093EF
		public virtual string Name
		{
			get
			{
				return this.m_strParamName;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x06004BF0 RID: 19440 RVA: 0x0010A3F7 File Offset: 0x001093F7
		public virtual int Position
		{
			get
			{
				return this.m_iPosition;
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06004BF1 RID: 19441 RVA: 0x0010A3FF File Offset: 0x001093FF
		public virtual int Attributes
		{
			get
			{
				return (int)this.m_attributes;
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x06004BF2 RID: 19442 RVA: 0x0010A407 File Offset: 0x00109407
		public bool IsIn
		{
			get
			{
				return (this.m_attributes & ParameterAttributes.In) != ParameterAttributes.None;
			}
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06004BF3 RID: 19443 RVA: 0x0010A417 File Offset: 0x00109417
		public bool IsOut
		{
			get
			{
				return (this.m_attributes & ParameterAttributes.Out) != ParameterAttributes.None;
			}
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06004BF4 RID: 19444 RVA: 0x0010A427 File Offset: 0x00109427
		public bool IsOptional
		{
			get
			{
				return (this.m_attributes & ParameterAttributes.Optional) != ParameterAttributes.None;
			}
		}

		// Token: 0x040027C9 RID: 10185
		private string m_strParamName;

		// Token: 0x040027CA RID: 10186
		private int m_iPosition;

		// Token: 0x040027CB RID: 10187
		private ParameterAttributes m_attributes;

		// Token: 0x040027CC RID: 10188
		private MethodBuilder m_methodBuilder;

		// Token: 0x040027CD RID: 10189
		private ParameterToken m_pdToken;
	}
}

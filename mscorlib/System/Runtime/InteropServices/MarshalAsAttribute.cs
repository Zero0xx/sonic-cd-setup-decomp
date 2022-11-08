using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F5 RID: 1269
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
	[ComVisible(true)]
	public sealed class MarshalAsAttribute : Attribute
	{
		// Token: 0x06003166 RID: 12646 RVA: 0x000A9295 File Offset: 0x000A8295
		internal static Attribute GetCustomAttribute(ParameterInfo parameter)
		{
			return MarshalAsAttribute.GetCustomAttribute(parameter.MetadataToken, parameter.Member.Module);
		}

		// Token: 0x06003167 RID: 12647 RVA: 0x000A92AD File Offset: 0x000A82AD
		internal static bool IsDefined(ParameterInfo parameter)
		{
			return MarshalAsAttribute.GetCustomAttribute(parameter) != null;
		}

		// Token: 0x06003168 RID: 12648 RVA: 0x000A92BB File Offset: 0x000A82BB
		internal static Attribute GetCustomAttribute(RuntimeFieldInfo field)
		{
			return MarshalAsAttribute.GetCustomAttribute(field.MetadataToken, field.Module);
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x000A92CE File Offset: 0x000A82CE
		internal static bool IsDefined(RuntimeFieldInfo field)
		{
			return MarshalAsAttribute.GetCustomAttribute(field) != null;
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x000A92DC File Offset: 0x000A82DC
		internal static Attribute GetCustomAttribute(int token, Module scope)
		{
			int num = 0;
			int sizeConst = 0;
			string text = null;
			string marshalCookie = null;
			string text2 = null;
			int iidParamIndex = 0;
			ConstArray fieldMarshal = scope.ModuleHandle.GetMetadataImport().GetFieldMarshal(token);
			if (fieldMarshal.Length == 0)
			{
				return null;
			}
			UnmanagedType val;
			VarEnum safeArraySubType;
			UnmanagedType arraySubType;
			MetadataImport.GetMarshalAs(fieldMarshal, out val, out safeArraySubType, out text2, out arraySubType, out num, out sizeConst, out text, out marshalCookie, out iidParamIndex);
			Type safeArrayUserDefinedSubType = (text2 == null || text2.Length == 0) ? null : RuntimeTypeHandle.GetTypeByNameUsingCARules(text2, scope);
			Type marshalTypeRef = null;
			try
			{
				marshalTypeRef = ((text == null) ? null : RuntimeTypeHandle.GetTypeByNameUsingCARules(text, scope));
			}
			catch (TypeLoadException)
			{
			}
			return new MarshalAsAttribute(val, safeArraySubType, safeArrayUserDefinedSubType, arraySubType, (short)num, sizeConst, text, marshalTypeRef, marshalCookie, iidParamIndex);
		}

		// Token: 0x0600316B RID: 12651 RVA: 0x000A9394 File Offset: 0x000A8394
		internal MarshalAsAttribute(UnmanagedType val, VarEnum safeArraySubType, Type safeArrayUserDefinedSubType, UnmanagedType arraySubType, short sizeParamIndex, int sizeConst, string marshalType, Type marshalTypeRef, string marshalCookie, int iidParamIndex)
		{
			this._val = val;
			this.SafeArraySubType = safeArraySubType;
			this.SafeArrayUserDefinedSubType = safeArrayUserDefinedSubType;
			this.IidParameterIndex = iidParamIndex;
			this.ArraySubType = arraySubType;
			this.SizeParamIndex = sizeParamIndex;
			this.SizeConst = sizeConst;
			this.MarshalType = marshalType;
			this.MarshalTypeRef = marshalTypeRef;
			this.MarshalCookie = marshalCookie;
		}

		// Token: 0x0600316C RID: 12652 RVA: 0x000A93F4 File Offset: 0x000A83F4
		public MarshalAsAttribute(UnmanagedType unmanagedType)
		{
			this._val = unmanagedType;
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x000A9403 File Offset: 0x000A8403
		public MarshalAsAttribute(short unmanagedType)
		{
			this._val = (UnmanagedType)unmanagedType;
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x0600316E RID: 12654 RVA: 0x000A9412 File Offset: 0x000A8412
		public UnmanagedType Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001987 RID: 6535
		internal UnmanagedType _val;

		// Token: 0x04001988 RID: 6536
		public VarEnum SafeArraySubType;

		// Token: 0x04001989 RID: 6537
		public Type SafeArrayUserDefinedSubType;

		// Token: 0x0400198A RID: 6538
		public int IidParameterIndex;

		// Token: 0x0400198B RID: 6539
		public UnmanagedType ArraySubType;

		// Token: 0x0400198C RID: 6540
		public short SizeParamIndex;

		// Token: 0x0400198D RID: 6541
		public int SizeConst;

		// Token: 0x0400198E RID: 6542
		[ComVisible(true)]
		public string MarshalType;

		// Token: 0x0400198F RID: 6543
		[ComVisible(true)]
		public Type MarshalTypeRef;

		// Token: 0x04001990 RID: 6544
		public string MarshalCookie;
	}
}

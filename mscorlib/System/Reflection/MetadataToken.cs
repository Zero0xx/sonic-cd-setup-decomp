using System;
using System.Globalization;

namespace System.Reflection
{
	// Token: 0x02000322 RID: 802
	[Serializable]
	internal struct MetadataToken
	{
		// Token: 0x06001EAC RID: 7852 RVA: 0x0004D63A File Offset: 0x0004C63A
		public static implicit operator int(MetadataToken token)
		{
			return token.Value;
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x0004D643 File Offset: 0x0004C643
		public static implicit operator MetadataToken(int token)
		{
			return new MetadataToken(token);
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x0004D64C File Offset: 0x0004C64C
		public static bool IsTokenOfType(int token, params MetadataTokenType[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				if ((MetadataTokenType)((long)token & (long)((ulong)-16777216)) == types[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x0004D679 File Offset: 0x0004C679
		public static bool IsNullToken(int token)
		{
			return (token & 16777215) == 0;
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x0004D685 File Offset: 0x0004C685
		public MetadataToken(int token)
		{
			this.Value = token;
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001EB1 RID: 7857 RVA: 0x0004D68E File Offset: 0x0004C68E
		public bool IsGlobalTypeDefToken
		{
			get
			{
				return this.Value == 33554433;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001EB2 RID: 7858 RVA: 0x0004D69D File Offset: 0x0004C69D
		public MetadataTokenType TokenType
		{
			get
			{
				return (MetadataTokenType)((long)this.Value & (long)((ulong)-16777216));
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001EB3 RID: 7859 RVA: 0x0004D6AE File Offset: 0x0004C6AE
		public bool IsTypeRef
		{
			get
			{
				return this.TokenType == MetadataTokenType.TypeRef;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001EB4 RID: 7860 RVA: 0x0004D6BD File Offset: 0x0004C6BD
		public bool IsTypeDef
		{
			get
			{
				return this.TokenType == MetadataTokenType.TypeDef;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x0004D6CC File Offset: 0x0004C6CC
		public bool IsFieldDef
		{
			get
			{
				return this.TokenType == MetadataTokenType.FieldDef;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x0004D6DB File Offset: 0x0004C6DB
		public bool IsMethodDef
		{
			get
			{
				return this.TokenType == MetadataTokenType.MethodDef;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x0004D6EA File Offset: 0x0004C6EA
		public bool IsMemberRef
		{
			get
			{
				return this.TokenType == MetadataTokenType.MemberRef;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x0004D6F9 File Offset: 0x0004C6F9
		public bool IsEvent
		{
			get
			{
				return this.TokenType == MetadataTokenType.Event;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x0004D708 File Offset: 0x0004C708
		public bool IsProperty
		{
			get
			{
				return this.TokenType == MetadataTokenType.Property;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001EBA RID: 7866 RVA: 0x0004D717 File Offset: 0x0004C717
		public bool IsParamDef
		{
			get
			{
				return this.TokenType == MetadataTokenType.ParamDef;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001EBB RID: 7867 RVA: 0x0004D726 File Offset: 0x0004C726
		public bool IsTypeSpec
		{
			get
			{
				return this.TokenType == MetadataTokenType.TypeSpec;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001EBC RID: 7868 RVA: 0x0004D735 File Offset: 0x0004C735
		public bool IsMethodSpec
		{
			get
			{
				return this.TokenType == MetadataTokenType.MethodSpec;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001EBD RID: 7869 RVA: 0x0004D744 File Offset: 0x0004C744
		public bool IsString
		{
			get
			{
				return this.TokenType == MetadataTokenType.String;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001EBE RID: 7870 RVA: 0x0004D753 File Offset: 0x0004C753
		public bool IsSignature
		{
			get
			{
				return this.TokenType == MetadataTokenType.Signature;
			}
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x0004D764 File Offset: 0x0004C764
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "0x{0:x8}", new object[]
			{
				this.Value
			});
		}

		// Token: 0x04000D0C RID: 3340
		public int Value;
	}
}

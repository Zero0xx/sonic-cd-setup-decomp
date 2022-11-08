using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x0200007C RID: 124
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeDeclaration : CodeTypeMember
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000451 RID: 1105 RVA: 0x0001495B File Offset: 0x0001395B
		// (remove) Token: 0x06000452 RID: 1106 RVA: 0x00014974 File Offset: 0x00013974
		public event EventHandler PopulateBaseTypes;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000453 RID: 1107 RVA: 0x0001498D File Offset: 0x0001398D
		// (remove) Token: 0x06000454 RID: 1108 RVA: 0x000149A6 File Offset: 0x000139A6
		public event EventHandler PopulateMembers;

		// Token: 0x06000455 RID: 1109 RVA: 0x000149BF File Offset: 0x000139BF
		public CodeTypeDeclaration()
		{
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x000149E4 File Offset: 0x000139E4
		public CodeTypeDeclaration(string name)
		{
			base.Name = name;
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00014A10 File Offset: 0x00013A10
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x00014A18 File Offset: 0x00013A18
		public TypeAttributes TypeAttributes
		{
			get
			{
				return this.attributes;
			}
			set
			{
				this.attributes = value;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00014A21 File Offset: 0x00013A21
		public CodeTypeReferenceCollection BaseTypes
		{
			get
			{
				if ((this.populated & 1) == 0)
				{
					this.populated |= 1;
					if (this.PopulateBaseTypes != null)
					{
						this.PopulateBaseTypes(this, EventArgs.Empty);
					}
				}
				return this.baseTypes;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x00014A5A File Offset: 0x00013A5A
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x00014A7A File Offset: 0x00013A7A
		public bool IsClass
		{
			get
			{
				return (this.attributes & TypeAttributes.ClassSemanticsMask) == TypeAttributes.NotPublic && !this.isEnum && !this.isStruct;
			}
			set
			{
				if (value)
				{
					this.attributes &= ~TypeAttributes.ClassSemanticsMask;
					this.attributes = this.attributes;
					this.isStruct = false;
					this.isEnum = false;
				}
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x00014AA8 File Offset: 0x00013AA8
		// (set) Token: 0x0600045D RID: 1117 RVA: 0x00014AB0 File Offset: 0x00013AB0
		public bool IsStruct
		{
			get
			{
				return this.isStruct;
			}
			set
			{
				if (value)
				{
					this.attributes &= ~TypeAttributes.ClassSemanticsMask;
					this.isStruct = true;
					this.isEnum = false;
					return;
				}
				this.isStruct = false;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x00014ADA File Offset: 0x00013ADA
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x00014AE2 File Offset: 0x00013AE2
		public bool IsEnum
		{
			get
			{
				return this.isEnum;
			}
			set
			{
				if (value)
				{
					this.attributes &= ~TypeAttributes.ClassSemanticsMask;
					this.isStruct = false;
					this.isEnum = true;
					return;
				}
				this.isEnum = false;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00014B0C File Offset: 0x00013B0C
		// (set) Token: 0x06000461 RID: 1121 RVA: 0x00014B1C File Offset: 0x00013B1C
		public bool IsInterface
		{
			get
			{
				return (this.attributes & TypeAttributes.ClassSemanticsMask) == TypeAttributes.ClassSemanticsMask;
			}
			set
			{
				if (value)
				{
					this.attributes &= ~TypeAttributes.ClassSemanticsMask;
					this.attributes |= TypeAttributes.ClassSemanticsMask;
					this.isStruct = false;
					this.isEnum = false;
					return;
				}
				this.attributes &= ~TypeAttributes.ClassSemanticsMask;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00014B68 File Offset: 0x00013B68
		// (set) Token: 0x06000463 RID: 1123 RVA: 0x00014B70 File Offset: 0x00013B70
		public bool IsPartial
		{
			get
			{
				return this.isPartial;
			}
			set
			{
				this.isPartial = value;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00014B79 File Offset: 0x00013B79
		public CodeTypeMemberCollection Members
		{
			get
			{
				if ((this.populated & 2) == 0)
				{
					this.populated |= 2;
					if (this.PopulateMembers != null)
					{
						this.PopulateMembers(this, EventArgs.Empty);
					}
				}
				return this.members;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00014BB2 File Offset: 0x00013BB2
		[ComVisible(false)]
		public CodeTypeParameterCollection TypeParameters
		{
			get
			{
				if (this.typeParameters == null)
				{
					this.typeParameters = new CodeTypeParameterCollection();
				}
				return this.typeParameters;
			}
		}

		// Token: 0x0400087D RID: 2173
		private const int BaseTypesCollection = 1;

		// Token: 0x0400087E RID: 2174
		private const int MembersCollection = 2;

		// Token: 0x0400087F RID: 2175
		private TypeAttributes attributes = TypeAttributes.Public;

		// Token: 0x04000880 RID: 2176
		private CodeTypeReferenceCollection baseTypes = new CodeTypeReferenceCollection();

		// Token: 0x04000881 RID: 2177
		private CodeTypeMemberCollection members = new CodeTypeMemberCollection();

		// Token: 0x04000882 RID: 2178
		private bool isEnum;

		// Token: 0x04000883 RID: 2179
		private bool isStruct;

		// Token: 0x04000884 RID: 2180
		private int populated;

		// Token: 0x04000885 RID: 2181
		[OptionalField]
		private CodeTypeParameterCollection typeParameters;

		// Token: 0x04000886 RID: 2182
		[OptionalField]
		private bool isPartial;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200006A RID: 106
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeObjectCreateExpression : CodeExpression
	{
		// Token: 0x060003ED RID: 1005 RVA: 0x00014236 File Offset: 0x00013236
		public CodeObjectCreateExpression()
		{
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00014249 File Offset: 0x00013249
		public CodeObjectCreateExpression(CodeTypeReference createType, params CodeExpression[] parameters)
		{
			this.CreateType = createType;
			this.Parameters.AddRange(parameters);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0001426F File Offset: 0x0001326F
		public CodeObjectCreateExpression(string createType, params CodeExpression[] parameters)
		{
			this.CreateType = new CodeTypeReference(createType);
			this.Parameters.AddRange(parameters);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001429A File Offset: 0x0001329A
		public CodeObjectCreateExpression(Type createType, params CodeExpression[] parameters)
		{
			this.CreateType = new CodeTypeReference(createType);
			this.Parameters.AddRange(parameters);
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000142C5 File Offset: 0x000132C5
		// (set) Token: 0x060003F2 RID: 1010 RVA: 0x000142E5 File Offset: 0x000132E5
		public CodeTypeReference CreateType
		{
			get
			{
				if (this.createType == null)
				{
					this.createType = new CodeTypeReference("");
				}
				return this.createType;
			}
			set
			{
				this.createType = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x000142EE File Offset: 0x000132EE
		public CodeExpressionCollection Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04000862 RID: 2146
		private CodeTypeReference createType;

		// Token: 0x04000863 RID: 2147
		private CodeExpressionCollection parameters = new CodeExpressionCollection();
	}
}

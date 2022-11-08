using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200006C RID: 108
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeParameterDeclarationExpressionCollection : CollectionBase
	{
		// Token: 0x06000400 RID: 1024 RVA: 0x000143C7 File Offset: 0x000133C7
		public CodeParameterDeclarationExpressionCollection()
		{
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000143CF File Offset: 0x000133CF
		public CodeParameterDeclarationExpressionCollection(CodeParameterDeclarationExpressionCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x000143DE File Offset: 0x000133DE
		public CodeParameterDeclarationExpressionCollection(CodeParameterDeclarationExpression[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x170000C1 RID: 193
		public CodeParameterDeclarationExpression this[int index]
		{
			get
			{
				return (CodeParameterDeclarationExpression)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0001440F File Offset: 0x0001340F
		public int Add(CodeParameterDeclarationExpression value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00014420 File Offset: 0x00013420
		public void AddRange(CodeParameterDeclarationExpression[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00014454 File Offset: 0x00013454
		public void AddRange(CodeParameterDeclarationExpressionCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int count = value.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00014490 File Offset: 0x00013490
		public bool Contains(CodeParameterDeclarationExpression value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001449E File Offset: 0x0001349E
		public void CopyTo(CodeParameterDeclarationExpression[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x000144AD File Offset: 0x000134AD
		public int IndexOf(CodeParameterDeclarationExpression value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000144BB File Offset: 0x000134BB
		public void Insert(int index, CodeParameterDeclarationExpression value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000144CA File Offset: 0x000134CA
		public void Remove(CodeParameterDeclarationExpression value)
		{
			base.List.Remove(value);
		}
	}
}

using System;
using System.Collections.Generic;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000047 RID: 71
	internal class QilScopedVisitor : QilVisitor
	{
		// Token: 0x0600048E RID: 1166 RVA: 0x0001F3A0 File Offset: 0x0001E3A0
		protected virtual void BeginScope(QilNode node)
		{
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0001F3A2 File Offset: 0x0001E3A2
		protected virtual void EndScope(QilNode node)
		{
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0001F3A4 File Offset: 0x0001E3A4
		protected virtual void BeforeVisit(QilNode node)
		{
			QilNodeType nodeType = node.NodeType;
			if (nodeType != QilNodeType.QilExpression)
			{
				switch (nodeType)
				{
				case QilNodeType.Loop:
				case QilNodeType.Filter:
				case QilNodeType.Sort:
					goto IL_112;
				case QilNodeType.SortKey:
				case QilNodeType.DocOrderDistinct:
					return;
				case QilNodeType.Function:
					break;
				default:
					return;
				}
			}
			else
			{
				QilExpression qilExpression = (QilExpression)node;
				foreach (QilNode node2 in qilExpression.GlobalParameterList)
				{
					this.BeginScope(node2);
				}
				foreach (QilNode node3 in qilExpression.GlobalVariableList)
				{
					this.BeginScope(node3);
				}
				using (IEnumerator<QilNode> enumerator3 = qilExpression.FunctionList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						QilNode node4 = enumerator3.Current;
						this.BeginScope(node4);
					}
					return;
				}
			}
			using (IEnumerator<QilNode> enumerator4 = ((QilFunction)node).Arguments.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					QilNode node5 = enumerator4.Current;
					this.BeginScope(node5);
				}
				return;
			}
			IL_112:
			this.BeginScope(((QilLoop)node).Variable);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0001F508 File Offset: 0x0001E508
		protected virtual void AfterVisit(QilNode node)
		{
			QilNodeType nodeType = node.NodeType;
			if (nodeType != QilNodeType.QilExpression)
			{
				switch (nodeType)
				{
				case QilNodeType.Loop:
				case QilNodeType.Filter:
				case QilNodeType.Sort:
					goto IL_112;
				case QilNodeType.SortKey:
				case QilNodeType.DocOrderDistinct:
					return;
				case QilNodeType.Function:
					break;
				default:
					return;
				}
			}
			else
			{
				QilExpression qilExpression = (QilExpression)node;
				foreach (QilNode node2 in qilExpression.FunctionList)
				{
					this.EndScope(node2);
				}
				foreach (QilNode node3 in qilExpression.GlobalVariableList)
				{
					this.EndScope(node3);
				}
				using (IEnumerator<QilNode> enumerator3 = qilExpression.GlobalParameterList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						QilNode node4 = enumerator3.Current;
						this.EndScope(node4);
					}
					return;
				}
			}
			using (IEnumerator<QilNode> enumerator4 = ((QilFunction)node).Arguments.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					QilNode node5 = enumerator4.Current;
					this.EndScope(node5);
				}
				return;
			}
			IL_112:
			this.EndScope(((QilLoop)node).Variable);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0001F66C File Offset: 0x0001E66C
		protected override QilNode Visit(QilNode n)
		{
			this.BeforeVisit(n);
			QilNode result = base.Visit(n);
			this.AfterVisit(n);
			return result;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000063 RID: 99
	internal class QilXmlWriter : QilScopedVisitor
	{
		// Token: 0x060006A3 RID: 1699 RVA: 0x00023F95 File Offset: 0x00022F95
		public QilXmlWriter(XmlWriter writer) : this(writer, QilXmlWriter.Options.Annotations | QilXmlWriter.Options.TypeInfo | QilXmlWriter.Options.LineInfo | QilXmlWriter.Options.NodeIdentity | QilXmlWriter.Options.NodeLocation)
		{
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00023FA0 File Offset: 0x00022FA0
		public QilXmlWriter(XmlWriter writer, QilXmlWriter.Options options)
		{
			this.writer = writer;
			this.ngen = new QilXmlWriter.NameGenerator();
			this.options = options;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00023FC1 File Offset: 0x00022FC1
		public void ToXml(QilNode node)
		{
			this.VisitAssumeReference(node);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00023FCC File Offset: 0x00022FCC
		protected virtual void WriteAnnotations(object ann)
		{
			string text = null;
			string text2 = null;
			if (ann == null)
			{
				return;
			}
			if (ann is string)
			{
				text = (ann as string);
			}
			else if (ann is IQilAnnotation)
			{
				IQilAnnotation qilAnnotation = ann as IQilAnnotation;
				text2 = qilAnnotation.Name;
				text = ann.ToString();
			}
			else if (ann is IList<object>)
			{
				IList<object> list = (IList<object>)ann;
				foreach (object ann2 in list)
				{
					this.WriteAnnotations(ann2);
				}
				return;
			}
			if (text != null && text.Length != 0)
			{
				this.writer.WriteComment((text2 != null && text2.Length != 0) ? (text2 + ": " + text) : text);
			}
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00024094 File Offset: 0x00023094
		protected virtual void WriteLineInfo(QilNode node)
		{
			this.writer.WriteAttributeString("lineInfo", string.Format(CultureInfo.InvariantCulture, "[{0},{1} -- {2},{3}]", new object[]
			{
				node.SourceLine.StartLine,
				node.SourceLine.StartPos,
				node.SourceLine.EndLine,
				node.SourceLine.EndPos
			}));
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00024114 File Offset: 0x00023114
		protected virtual void WriteXmlType(QilNode node)
		{
			this.writer.WriteAttributeString("xmlType", node.XmlType.ToString(((this.options & QilXmlWriter.Options.RoundTripTypeInfo) != QilXmlWriter.Options.None) ? "S" : "G"));
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00024148 File Offset: 0x00023148
		protected override QilNode VisitChildren(QilNode node)
		{
			if (node is QilLiteral)
			{
				this.writer.WriteValue(Convert.ToString(((QilLiteral)node).Value, CultureInfo.InvariantCulture));
				return node;
			}
			if (node is QilReference)
			{
				QilReference qilReference = (QilReference)node;
				this.writer.WriteAttributeString("id", this.ngen.NameOf(node));
				if (qilReference.DebugName != null)
				{
					this.writer.WriteAttributeString("name", qilReference.DebugName.ToString());
				}
				if (node.NodeType == QilNodeType.Parameter)
				{
					QilParameter qilParameter = (QilParameter)node;
					if (qilParameter.DefaultValue != null)
					{
						this.Visit(qilParameter.DefaultValue);
					}
					return node;
				}
			}
			return base.VisitChildren(node);
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x000241FC File Offset: 0x000231FC
		protected override QilNode VisitReference(QilNode node)
		{
			QilReference qilReference = (QilReference)node;
			string text = this.ngen.NameOf(node);
			if (text == null)
			{
				text = "OUT-OF-SCOPE REFERENCE";
			}
			this.writer.WriteStartElement("RefTo");
			this.writer.WriteAttributeString("id", text);
			if (qilReference.DebugName != null)
			{
				this.writer.WriteAttributeString("name", qilReference.DebugName.ToString());
			}
			this.writer.WriteEndElement();
			return node;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00024278 File Offset: 0x00023278
		protected override QilNode VisitQilExpression(QilExpression qil)
		{
			IList<QilNode> list = new QilXmlWriter.ForwardRefFinder().Find(qil);
			if (list != null && list.Count > 0)
			{
				this.writer.WriteStartElement("ForwardDecls");
				foreach (QilNode qilNode in list)
				{
					this.writer.WriteStartElement(Enum.GetName(typeof(QilNodeType), qilNode.NodeType));
					this.writer.WriteAttributeString("id", this.ngen.NameOf(qilNode));
					this.WriteXmlType(qilNode);
					if (qilNode.NodeType == QilNodeType.Function)
					{
						this.Visit(qilNode[0]);
						this.Visit(qilNode[2]);
					}
					this.writer.WriteEndElement();
				}
				this.writer.WriteEndElement();
			}
			return this.VisitChildren(qil);
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00024378 File Offset: 0x00023378
		protected override QilNode VisitLiteralType(QilLiteral value)
		{
			this.writer.WriteString(value.ToString(((this.options & QilXmlWriter.Options.TypeInfo) != QilXmlWriter.Options.None) ? "G" : "S"));
			return value;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x000243A7 File Offset: 0x000233A7
		protected override QilNode VisitLiteralQName(QilName value)
		{
			this.writer.WriteAttributeString("name", value.ToString());
			return value;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x000243C0 File Offset: 0x000233C0
		protected override void BeginScope(QilNode node)
		{
			this.ngen.NameOf(node);
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x000243CF File Offset: 0x000233CF
		protected override void EndScope(QilNode node)
		{
			this.ngen.ClearName(node);
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000243E0 File Offset: 0x000233E0
		protected override void BeforeVisit(QilNode node)
		{
			base.BeforeVisit(node);
			if ((this.options & QilXmlWriter.Options.Annotations) != QilXmlWriter.Options.None)
			{
				this.WriteAnnotations(node.Annotation);
			}
			this.writer.WriteStartElement("", Enum.GetName(typeof(QilNodeType), node.NodeType), "");
			if ((this.options & (QilXmlWriter.Options.TypeInfo | QilXmlWriter.Options.RoundTripTypeInfo)) != QilXmlWriter.Options.None)
			{
				this.WriteXmlType(node);
			}
			if ((this.options & QilXmlWriter.Options.LineInfo) != QilXmlWriter.Options.None && node.SourceLine != null)
			{
				this.WriteLineInfo(node);
			}
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00024463 File Offset: 0x00023463
		protected override void AfterVisit(QilNode node)
		{
			this.writer.WriteEndElement();
			base.AfterVisit(node);
		}

		// Token: 0x0400041D RID: 1053
		protected XmlWriter writer;

		// Token: 0x0400041E RID: 1054
		protected QilXmlWriter.Options options;

		// Token: 0x0400041F RID: 1055
		private QilXmlWriter.NameGenerator ngen;

		// Token: 0x02000064 RID: 100
		[Flags]
		public enum Options
		{
			// Token: 0x04000421 RID: 1057
			None = 0,
			// Token: 0x04000422 RID: 1058
			Annotations = 1,
			// Token: 0x04000423 RID: 1059
			TypeInfo = 2,
			// Token: 0x04000424 RID: 1060
			RoundTripTypeInfo = 4,
			// Token: 0x04000425 RID: 1061
			LineInfo = 8,
			// Token: 0x04000426 RID: 1062
			NodeIdentity = 16,
			// Token: 0x04000427 RID: 1063
			NodeLocation = 32
		}

		// Token: 0x02000065 RID: 101
		internal class ForwardRefFinder : QilVisitor
		{
			// Token: 0x060006B2 RID: 1714 RVA: 0x00024477 File Offset: 0x00023477
			public IList<QilNode> Find(QilExpression qil)
			{
				this.Visit(qil);
				return this.fwdrefs;
			}

			// Token: 0x060006B3 RID: 1715 RVA: 0x00024487 File Offset: 0x00023487
			protected override QilNode Visit(QilNode node)
			{
				if (node is QilIterator || node is QilFunction)
				{
					this.backrefs.Add(node);
				}
				return base.Visit(node);
			}

			// Token: 0x060006B4 RID: 1716 RVA: 0x000244AC File Offset: 0x000234AC
			protected override QilNode VisitReference(QilNode node)
			{
				if (!this.backrefs.Contains(node) && !this.fwdrefs.Contains(node))
				{
					this.fwdrefs.Add(node);
				}
				return node;
			}

			// Token: 0x04000428 RID: 1064
			private List<QilNode> fwdrefs = new List<QilNode>();

			// Token: 0x04000429 RID: 1065
			private List<QilNode> backrefs = new List<QilNode>();
		}

		// Token: 0x02000066 RID: 102
		private sealed class NameGenerator
		{
			// Token: 0x060006B6 RID: 1718 RVA: 0x000244F8 File Offset: 0x000234F8
			public NameGenerator()
			{
				string text = "$";
				this.len = (this.zero = text.Length);
				this.start = 'a';
				this.end = 'z';
				this.name = new StringBuilder(text, this.len + 2);
				this.name.Append(this.start);
			}

			// Token: 0x060006B7 RID: 1719 RVA: 0x0002455C File Offset: 0x0002355C
			public string NextName()
			{
				string result = this.name.ToString();
				char c = this.name[this.len];
				if (c == this.end)
				{
					this.name[this.len] = this.start;
					int num = this.len;
					while (num-- > this.zero && this.name[num] == this.end)
					{
						this.name[num] = this.start;
					}
					if (num < this.zero)
					{
						this.len++;
						this.name.Append(this.start);
					}
					else
					{
						StringBuilder stringBuilder;
						int index;
						(stringBuilder = this.name)[index = num] = stringBuilder[index] + '\u0001';
					}
				}
				else
				{
					this.name[this.len] = c + '\u0001';
				}
				return result;
			}

			// Token: 0x060006B8 RID: 1720 RVA: 0x00024648 File Offset: 0x00023648
			public string NameOf(QilNode n)
			{
				object annotation = n.Annotation;
				QilXmlWriter.NameGenerator.NameAnnotation nameAnnotation = annotation as QilXmlWriter.NameGenerator.NameAnnotation;
				string text;
				if (nameAnnotation == null)
				{
					text = this.NextName();
					n.Annotation = new QilXmlWriter.NameGenerator.NameAnnotation(text, annotation);
				}
				else
				{
					text = nameAnnotation.Name;
				}
				return text;
			}

			// Token: 0x060006B9 RID: 1721 RVA: 0x00024686 File Offset: 0x00023686
			public void ClearName(QilNode n)
			{
				if (n.Annotation is QilXmlWriter.NameGenerator.NameAnnotation)
				{
					n.Annotation = ((QilXmlWriter.NameGenerator.NameAnnotation)n.Annotation).PriorAnnotation;
				}
			}

			// Token: 0x0400042A RID: 1066
			private StringBuilder name;

			// Token: 0x0400042B RID: 1067
			private int len;

			// Token: 0x0400042C RID: 1068
			private int zero;

			// Token: 0x0400042D RID: 1069
			private char start;

			// Token: 0x0400042E RID: 1070
			private char end;

			// Token: 0x02000067 RID: 103
			private class NameAnnotation : ListBase<object>
			{
				// Token: 0x060006BA RID: 1722 RVA: 0x000246AB File Offset: 0x000236AB
				public NameAnnotation(string s, object a)
				{
					this.Name = s;
					this.PriorAnnotation = a;
				}

				// Token: 0x170000EB RID: 235
				// (get) Token: 0x060006BB RID: 1723 RVA: 0x000246C1 File Offset: 0x000236C1
				public override int Count
				{
					get
					{
						return 1;
					}
				}

				// Token: 0x170000EC RID: 236
				public override object this[int index]
				{
					get
					{
						if (index == 0)
						{
							return this.PriorAnnotation;
						}
						throw new IndexOutOfRangeException();
					}
					set
					{
						throw new NotSupportedException();
					}
				}

				// Token: 0x0400042F RID: 1071
				public string Name;

				// Token: 0x04000430 RID: 1072
				public object PriorAnnotation;
			}
		}
	}
}

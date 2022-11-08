using System;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x02000034 RID: 52
	internal class XmlILStateAnalyzer
	{
		// Token: 0x06000330 RID: 816 RVA: 0x000152AE File Offset: 0x000142AE
		public XmlILStateAnalyzer(QilFactory fac)
		{
			this.fac = fac;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000152C0 File Offset: 0x000142C0
		public virtual QilNode Analyze(QilNode ndConstr, QilNode ndContent)
		{
			if (ndConstr == null)
			{
				this.parentInfo = null;
				this.xstates = PossibleXmlStates.WithinSequence;
				this.withinElem = false;
				ndContent = this.AnalyzeContent(ndContent);
			}
			else
			{
				this.parentInfo = XmlILConstructInfo.Write(ndConstr);
				if (ndConstr.NodeType == QilNodeType.Function)
				{
					this.parentInfo.ConstructMethod = XmlILConstructMethod.Writer;
					PossibleXmlStates possibleXmlStates = PossibleXmlStates.None;
					foreach (object obj in this.parentInfo.CallersInfo)
					{
						XmlILConstructInfo xmlILConstructInfo = (XmlILConstructInfo)obj;
						if (possibleXmlStates == PossibleXmlStates.None)
						{
							possibleXmlStates = xmlILConstructInfo.InitialStates;
						}
						else if (possibleXmlStates != xmlILConstructInfo.InitialStates)
						{
							possibleXmlStates = PossibleXmlStates.Any;
						}
						xmlILConstructInfo.PushToWriterFirst = true;
					}
					this.parentInfo.InitialStates = possibleXmlStates;
				}
				else
				{
					if (ndConstr.NodeType != QilNodeType.Choice)
					{
						this.parentInfo.InitialStates = (this.parentInfo.FinalStates = PossibleXmlStates.WithinSequence);
					}
					if (ndConstr.NodeType != QilNodeType.RtfCtor)
					{
						this.parentInfo.ConstructMethod = XmlILConstructMethod.WriterThenIterator;
					}
				}
				this.withinElem = (ndConstr.NodeType == QilNodeType.ElementCtor);
				QilNodeType nodeType = ndConstr.NodeType;
				if (nodeType <= QilNodeType.Function)
				{
					if (nodeType != QilNodeType.Choice)
					{
						if (nodeType == QilNodeType.Function)
						{
							this.xstates = this.parentInfo.InitialStates;
						}
					}
					else
					{
						this.xstates = PossibleXmlStates.Any;
					}
				}
				else
				{
					switch (nodeType)
					{
					case QilNodeType.ElementCtor:
						this.xstates = PossibleXmlStates.EnumAttrs;
						break;
					case QilNodeType.AttributeCtor:
						this.xstates = PossibleXmlStates.WithinAttr;
						break;
					case QilNodeType.CommentCtor:
						this.xstates = PossibleXmlStates.WithinComment;
						break;
					case QilNodeType.PICtor:
						this.xstates = PossibleXmlStates.WithinPI;
						break;
					case QilNodeType.TextCtor:
					case QilNodeType.RawTextCtor:
					case QilNodeType.NamespaceDecl:
						break;
					case QilNodeType.DocumentCtor:
						this.xstates = PossibleXmlStates.WithinContent;
						break;
					case QilNodeType.RtfCtor:
						this.xstates = PossibleXmlStates.WithinContent;
						break;
					default:
						switch (nodeType)
						{
						case QilNodeType.XsltCopy:
							this.xstates = PossibleXmlStates.Any;
							break;
						}
						break;
					}
				}
				if (ndContent != null)
				{
					ndContent = this.AnalyzeContent(ndContent);
				}
				if (ndConstr.NodeType == QilNodeType.Choice)
				{
					this.AnalyzeChoice(ndConstr as QilChoice, this.parentInfo);
				}
				if (ndConstr.NodeType == QilNodeType.Function)
				{
					this.parentInfo.FinalStates = this.xstates;
				}
			}
			return ndContent;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x000154DC File Offset: 0x000144DC
		protected virtual QilNode AnalyzeContent(QilNode nd)
		{
			switch (nd.NodeType)
			{
			case QilNodeType.For:
			case QilNodeType.Let:
			case QilNodeType.Parameter:
				nd = this.fac.Nop(nd);
				break;
			}
			XmlILConstructInfo xmlILConstructInfo = XmlILConstructInfo.Write(nd);
			xmlILConstructInfo.ParentInfo = this.parentInfo;
			xmlILConstructInfo.PushToWriterLast = true;
			xmlILConstructInfo.InitialStates = this.xstates;
			QilNodeType nodeType = nd.NodeType;
			switch (nodeType)
			{
			case QilNodeType.Nop:
			{
				QilNode child = (nd as QilUnary).Child;
				switch (child.NodeType)
				{
				case QilNodeType.For:
				case QilNodeType.Let:
				case QilNodeType.Parameter:
					this.AnalyzeCopy(nd, xmlILConstructInfo);
					break;
				default:
					xmlILConstructInfo.ConstructMethod = XmlILConstructMethod.Writer;
					this.AnalyzeContent(child);
					break;
				}
				break;
			}
			case QilNodeType.Error:
			case QilNodeType.Warning:
				xmlILConstructInfo.ConstructMethod = XmlILConstructMethod.Writer;
				break;
			default:
				switch (nodeType)
				{
				case QilNodeType.Conditional:
					this.AnalyzeConditional(nd as QilTernary, xmlILConstructInfo);
					goto IL_126;
				case QilNodeType.Choice:
					this.AnalyzeChoice(nd as QilChoice, xmlILConstructInfo);
					goto IL_126;
				case QilNodeType.Length:
					break;
				case QilNodeType.Sequence:
					this.AnalyzeSequence(nd as QilList, xmlILConstructInfo);
					goto IL_126;
				default:
					if (nodeType == QilNodeType.Loop)
					{
						this.AnalyzeLoop(nd as QilLoop, xmlILConstructInfo);
						goto IL_126;
					}
					break;
				}
				this.AnalyzeCopy(nd, xmlILConstructInfo);
				break;
			}
			IL_126:
			xmlILConstructInfo.FinalStates = this.xstates;
			return nd;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0001561C File Offset: 0x0001461C
		protected virtual void AnalyzeLoop(QilLoop ndLoop, XmlILConstructInfo info)
		{
			XmlQueryType xmlType = ndLoop.XmlType;
			info.ConstructMethod = XmlILConstructMethod.Writer;
			if (!xmlType.IsSingleton)
			{
				this.StartLoop(xmlType, info);
			}
			ndLoop.Body = this.AnalyzeContent(ndLoop.Body);
			if (!xmlType.IsSingleton)
			{
				this.EndLoop(xmlType, info);
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0001566C File Offset: 0x0001466C
		protected virtual void AnalyzeSequence(QilList ndSeq, XmlILConstructInfo info)
		{
			info.ConstructMethod = XmlILConstructMethod.Writer;
			for (int i = 0; i < ndSeq.Count; i++)
			{
				ndSeq[i] = this.AnalyzeContent(ndSeq[i]);
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x000156A8 File Offset: 0x000146A8
		protected virtual void AnalyzeConditional(QilTernary ndCond, XmlILConstructInfo info)
		{
			info.ConstructMethod = XmlILConstructMethod.Writer;
			ndCond.Center = this.AnalyzeContent(ndCond.Center);
			PossibleXmlStates possibleXmlStates = this.xstates;
			this.xstates = info.InitialStates;
			ndCond.Right = this.AnalyzeContent(ndCond.Right);
			if (possibleXmlStates != this.xstates)
			{
				this.xstates = PossibleXmlStates.Any;
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00015704 File Offset: 0x00014704
		protected virtual void AnalyzeChoice(QilChoice ndChoice, XmlILConstructInfo info)
		{
			int num = ndChoice.Branches.Count - 1;
			ndChoice.Branches[num] = this.AnalyzeContent(ndChoice.Branches[num]);
			PossibleXmlStates possibleXmlStates = this.xstates;
			while (--num >= 0)
			{
				this.xstates = info.InitialStates;
				ndChoice.Branches[num] = this.AnalyzeContent(ndChoice.Branches[num]);
				if (possibleXmlStates != this.xstates)
				{
					possibleXmlStates = PossibleXmlStates.Any;
				}
			}
			this.xstates = possibleXmlStates;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0001578C File Offset: 0x0001478C
		protected virtual void AnalyzeCopy(QilNode ndCopy, XmlILConstructInfo info)
		{
			XmlQueryType xmlType = ndCopy.XmlType;
			if (!xmlType.IsSingleton)
			{
				this.StartLoop(xmlType, info);
			}
			if (this.MaybeContent(xmlType))
			{
				if (this.MaybeAttrNmsp(xmlType))
				{
					if (this.xstates == PossibleXmlStates.EnumAttrs)
					{
						this.xstates = PossibleXmlStates.Any;
					}
				}
				else if (this.xstates == PossibleXmlStates.EnumAttrs || this.withinElem)
				{
					this.xstates = PossibleXmlStates.WithinContent;
				}
			}
			if (!xmlType.IsSingleton)
			{
				this.EndLoop(xmlType, info);
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x000157FC File Offset: 0x000147FC
		private void StartLoop(XmlQueryType typ, XmlILConstructInfo info)
		{
			info.BeginLoopStates = this.xstates;
			if (typ.MaybeMany && this.xstates == PossibleXmlStates.EnumAttrs && this.MaybeContent(typ))
			{
				info.BeginLoopStates = (this.xstates = PossibleXmlStates.Any);
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0001583F File Offset: 0x0001483F
		private void EndLoop(XmlQueryType typ, XmlILConstructInfo info)
		{
			info.EndLoopStates = this.xstates;
			if (typ.MaybeEmpty && info.InitialStates != this.xstates)
			{
				this.xstates = PossibleXmlStates.Any;
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0001586A File Offset: 0x0001486A
		private bool MaybeAttrNmsp(XmlQueryType typ)
		{
			return (typ.NodeKinds & (XmlNodeKindFlags.Attribute | XmlNodeKindFlags.Namespace)) != XmlNodeKindFlags.None;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0001587B File Offset: 0x0001487B
		private bool MaybeContent(XmlQueryType typ)
		{
			return !typ.IsNode || (typ.NodeKinds & ~(XmlNodeKindFlags.Attribute | XmlNodeKindFlags.Namespace)) != XmlNodeKindFlags.None;
		}

		// Token: 0x040002B7 RID: 695
		protected XmlILConstructInfo parentInfo;

		// Token: 0x040002B8 RID: 696
		protected QilFactory fac;

		// Token: 0x040002B9 RID: 697
		protected PossibleXmlStates xstates;

		// Token: 0x040002BA RID: 698
		protected bool withinElem;
	}
}

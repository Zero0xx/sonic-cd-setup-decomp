using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200013D RID: 317
	internal class ChooseAction : ContainerAction
	{
		// Token: 0x06000DD9 RID: 3545 RVA: 0x000479FF File Offset: 0x000469FF
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			if (compiler.Recurse())
			{
				this.CompileConditions(compiler);
				compiler.ToParent();
			}
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x00047A20 File Offset: 0x00046A20
		private void CompileConditions(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			bool flag = false;
			bool flag2 = false;
			for (;;)
			{
				switch (input.NodeType)
				{
				case XPathNodeType.Element:
				{
					compiler.PushNamespaceScope();
					string namespaceURI = input.NamespaceURI;
					string localName = input.LocalName;
					if (Keywords.Equals(namespaceURI, input.Atoms.XsltNamespace))
					{
						IfAction action;
						if (Keywords.Equals(localName, input.Atoms.When))
						{
							if (flag2)
							{
								goto Block_4;
							}
							action = compiler.CreateIfAction(IfAction.ConditionType.ConditionWhen);
							flag = true;
						}
						else
						{
							if (!Keywords.Equals(localName, input.Atoms.Otherwise))
							{
								goto IL_D6;
							}
							if (flag2)
							{
								goto Block_6;
							}
							action = compiler.CreateIfAction(IfAction.ConditionType.ConditionOtherwise);
							flag2 = true;
						}
						base.AddAction(action);
						compiler.PopScope();
						goto IL_114;
					}
					goto IL_E7;
				}
				case XPathNodeType.SignificantWhitespace:
				case XPathNodeType.Whitespace:
				case XPathNodeType.ProcessingInstruction:
				case XPathNodeType.Comment:
					goto IL_114;
				}
				break;
				IL_114:
				if (!compiler.Advance())
				{
					goto Block_7;
				}
			}
			goto IL_F6;
			Block_4:
			throw XsltException.Create("Xslt_WhenAfterOtherwise", new string[0]);
			Block_6:
			throw XsltException.Create("Xslt_DupOtherwise", new string[0]);
			IL_D6:
			throw compiler.UnexpectedKeyword();
			IL_E7:
			throw compiler.UnexpectedKeyword();
			IL_F6:
			throw XsltException.Create("Xslt_InvalidContents", new string[]
			{
				"choose"
			});
			Block_7:
			if (!flag)
			{
				throw XsltException.Create("Xslt_NoWhen", new string[0]);
			}
		}
	}
}

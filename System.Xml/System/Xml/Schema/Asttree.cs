using System;
using System.Collections;
using System.Xml.XPath;
using MS.Internal.Xml.XPath;

namespace System.Xml.Schema
{
	// Token: 0x02000181 RID: 385
	internal class Asttree
	{
		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001461 RID: 5217 RVA: 0x000572F6 File Offset: 0x000562F6
		internal ArrayList SubtreeArray
		{
			get
			{
				return this.fAxisArray;
			}
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x000572FE File Offset: 0x000562FE
		public Asttree(string xPath, bool isField, XmlNamespaceManager nsmgr)
		{
			this.xpathexpr = xPath;
			this.isField = isField;
			this.nsmgr = nsmgr;
			this.CompileXPath(xPath, isField, nsmgr);
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x00057324 File Offset: 0x00056324
		private static bool IsNameTest(Axis ast)
		{
			return ast.TypeOfAxis == Axis.AxisType.Child && ast.NodeType == XPathNodeType.Element;
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0005733A File Offset: 0x0005633A
		internal static bool IsAttribute(Axis ast)
		{
			return ast.TypeOfAxis == Axis.AxisType.Attribute && ast.NodeType == XPathNodeType.Attribute;
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x00057350 File Offset: 0x00056350
		private static bool IsDescendantOrSelf(Axis ast)
		{
			return ast.TypeOfAxis == Axis.AxisType.DescendantOrSelf && ast.NodeType == XPathNodeType.All && ast.AbbrAxis;
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0005736D File Offset: 0x0005636D
		internal static bool IsSelf(Axis ast)
		{
			return ast.TypeOfAxis == Axis.AxisType.Self && ast.NodeType == XPathNodeType.All && ast.AbbrAxis;
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0005738C File Offset: 0x0005638C
		public void CompileXPath(string xPath, bool isField, XmlNamespaceManager nsmgr)
		{
			if (xPath == null || xPath.Length == 0)
			{
				throw new XmlSchemaException("Sch_EmptyXPath", string.Empty);
			}
			string[] array = xPath.Split(new char[]
			{
				'|'
			});
			ArrayList arrayList = new ArrayList(array.Length);
			this.fAxisArray = new ArrayList(array.Length);
			try
			{
				foreach (string xpathExpresion in array)
				{
					Axis value = (Axis)XPathParser.ParseXPathExpresion(xpathExpresion);
					arrayList.Add(value);
				}
			}
			catch
			{
				throw new XmlSchemaException("Sch_ICXpathError", xPath);
			}
			foreach (object obj in arrayList)
			{
				Axis axis = (Axis)obj;
				Axis axis2;
				if ((axis2 = axis) == null)
				{
					throw new XmlSchemaException("Sch_ICXpathError", xPath);
				}
				Axis axis3 = axis2;
				if (Asttree.IsAttribute(axis2))
				{
					if (!isField)
					{
						throw new XmlSchemaException("Sch_SelectorAttr", xPath);
					}
					this.SetURN(axis2, nsmgr);
					try
					{
						axis2 = (Axis)axis2.Input;
						goto IL_14D;
					}
					catch
					{
						throw new XmlSchemaException("Sch_ICXpathError", xPath);
					}
					goto IL_FB;
				}
				IL_14D:
				if (axis2 == null || (!Asttree.IsNameTest(axis2) && !Asttree.IsSelf(axis2)))
				{
					axis3.Input = null;
					if (axis2 == null)
					{
						if (Asttree.IsSelf(axis) && axis.Input != null)
						{
							this.fAxisArray.Add(new ForwardAxis(DoubleLinkAxis.ConvertTree((Axis)axis.Input), false));
							continue;
						}
						this.fAxisArray.Add(new ForwardAxis(DoubleLinkAxis.ConvertTree(axis), false));
						continue;
					}
					else
					{
						if (!Asttree.IsDescendantOrSelf(axis2))
						{
							throw new XmlSchemaException("Sch_ICXpathError", xPath);
						}
						try
						{
							axis2 = (Axis)axis2.Input;
						}
						catch
						{
							throw new XmlSchemaException("Sch_ICXpathError", xPath);
						}
						if (axis2 == null || !Asttree.IsSelf(axis2) || axis2.Input != null)
						{
							throw new XmlSchemaException("Sch_ICXpathError", xPath);
						}
						if (Asttree.IsSelf(axis) && axis.Input != null)
						{
							this.fAxisArray.Add(new ForwardAxis(DoubleLinkAxis.ConvertTree((Axis)axis.Input), true));
							continue;
						}
						this.fAxisArray.Add(new ForwardAxis(DoubleLinkAxis.ConvertTree(axis), true));
						continue;
					}
				}
				IL_FB:
				if (Asttree.IsSelf(axis2) && axis != axis2)
				{
					axis3.Input = axis2.Input;
				}
				else
				{
					axis3 = axis2;
					if (Asttree.IsNameTest(axis2))
					{
						this.SetURN(axis2, nsmgr);
					}
				}
				try
				{
					axis2 = (Axis)axis2.Input;
				}
				catch
				{
					throw new XmlSchemaException("Sch_ICXpathError", xPath);
				}
				goto IL_14D;
			}
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x000576A4 File Offset: 0x000566A4
		private void SetURN(Axis axis, XmlNamespaceManager nsmgr)
		{
			if (axis.Prefix.Length != 0)
			{
				axis.Urn = nsmgr.LookupNamespace(axis.Prefix);
				if (axis.Urn == null)
				{
					throw new XmlSchemaException("Sch_UnresolvedPrefix", axis.Prefix);
				}
			}
			else
			{
				if (axis.Name.Length != 0)
				{
					axis.Urn = null;
					return;
				}
				axis.Urn = "";
			}
		}

		// Token: 0x04000C65 RID: 3173
		private ArrayList fAxisArray;

		// Token: 0x04000C66 RID: 3174
		private string xpathexpr;

		// Token: 0x04000C67 RID: 3175
		private bool isField;

		// Token: 0x04000C68 RID: 3176
		private XmlNamespaceManager nsmgr;
	}
}

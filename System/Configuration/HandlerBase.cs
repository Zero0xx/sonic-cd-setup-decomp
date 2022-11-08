using System;
using System.Globalization;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006F5 RID: 1781
	internal class HandlerBase
	{
		// Token: 0x060036EC RID: 14060 RVA: 0x000E9D15 File Offset: 0x000E8D15
		private HandlerBase()
		{
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x000E9D20 File Offset: 0x000E8D20
		private static XmlNode GetAndRemoveAttribute(XmlNode node, string attrib, bool fRequired)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(attrib);
			if (fRequired && xmlNode == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_missing_required_attribute", new object[]
				{
					attrib,
					node.Name
				}), node);
			}
			return xmlNode;
		}

		// Token: 0x060036EE RID: 14062 RVA: 0x000E9D68 File Offset: 0x000E8D68
		private static XmlNode GetAndRemoveStringAttributeInternal(XmlNode node, string attrib, bool fRequired, ref string val)
		{
			XmlNode andRemoveAttribute = HandlerBase.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				val = andRemoveAttribute.Value;
			}
			return andRemoveAttribute;
		}

		// Token: 0x060036EF RID: 14063 RVA: 0x000E9D8A File Offset: 0x000E8D8A
		internal static XmlNode GetAndRemoveStringAttribute(XmlNode node, string attrib, ref string val)
		{
			return HandlerBase.GetAndRemoveStringAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x060036F0 RID: 14064 RVA: 0x000E9D98 File Offset: 0x000E8D98
		private static XmlNode GetAndRemoveBooleanAttributeInternal(XmlNode node, string attrib, bool fRequired, ref bool val)
		{
			XmlNode andRemoveAttribute = HandlerBase.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				try
				{
					val = bool.Parse(andRemoveAttribute.Value);
				}
				catch (Exception inner)
				{
					throw new ConfigurationErrorsException(SR.GetString(SR.GetString("Config_invalid_boolean_attribute", new object[]
					{
						andRemoveAttribute.Name
					})), inner, andRemoveAttribute);
				}
			}
			return andRemoveAttribute;
		}

		// Token: 0x060036F1 RID: 14065 RVA: 0x000E9DFC File Offset: 0x000E8DFC
		internal static XmlNode GetAndRemoveBooleanAttribute(XmlNode node, string attrib, ref bool val)
		{
			return HandlerBase.GetAndRemoveBooleanAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x060036F2 RID: 14066 RVA: 0x000E9E08 File Offset: 0x000E8E08
		private static XmlNode GetAndRemoveIntegerAttributeInternal(XmlNode node, string attrib, bool fRequired, ref int val)
		{
			XmlNode andRemoveAttribute = HandlerBase.GetAndRemoveAttribute(node, attrib, fRequired);
			if (andRemoveAttribute != null)
			{
				if (andRemoveAttribute.Value.Trim() != andRemoveAttribute.Value)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_invalid_integer_attribute", new object[]
					{
						andRemoveAttribute.Name
					}), andRemoveAttribute);
				}
				try
				{
					val = int.Parse(andRemoveAttribute.Value, CultureInfo.InvariantCulture);
				}
				catch (Exception inner)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_invalid_integer_attribute", new object[]
					{
						andRemoveAttribute.Name
					}), inner, andRemoveAttribute);
				}
			}
			return andRemoveAttribute;
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x000E9EA4 File Offset: 0x000E8EA4
		internal static XmlNode GetAndRemoveIntegerAttribute(XmlNode node, string attrib, ref int val)
		{
			return HandlerBase.GetAndRemoveIntegerAttributeInternal(node, attrib, false, ref val);
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x000E9EB0 File Offset: 0x000E8EB0
		internal static void CheckForUnrecognizedAttributes(XmlNode node)
		{
			if (node.Attributes.Count != 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_attribute", new object[]
				{
					node.Attributes[0].Name
				}), node);
			}
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x000E9EF8 File Offset: 0x000E8EF8
		internal static string RemoveAttribute(XmlNode node, string name)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(name);
			if (xmlNode != null)
			{
				return xmlNode.Value;
			}
			return null;
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x000E9F1D File Offset: 0x000E8F1D
		internal static string RemoveRequiredAttribute(XmlNode node, string name)
		{
			return HandlerBase.RemoveRequiredAttribute(node, name, false);
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x000E9F28 File Offset: 0x000E8F28
		internal static string RemoveRequiredAttribute(XmlNode node, string name, bool allowEmpty)
		{
			XmlNode xmlNode = node.Attributes.RemoveNamedItem(name);
			if (xmlNode == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_required_attribute_missing", new object[]
				{
					name
				}), node);
			}
			if (string.IsNullOrEmpty(xmlNode.Value) && !allowEmpty)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_required_attribute_empty", new object[]
				{
					name
				}), node);
			}
			return xmlNode.Value;
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x000E9F95 File Offset: 0x000E8F95
		internal static void CheckForNonElement(XmlNode node)
		{
			if (node.NodeType != XmlNodeType.Element)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_elements_only"), node);
			}
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x000E9FB1 File Offset: 0x000E8FB1
		internal static bool IsIgnorableAlsoCheckForNonElement(XmlNode node)
		{
			if (node.NodeType == XmlNodeType.Comment || node.NodeType == XmlNodeType.Whitespace)
			{
				return true;
			}
			HandlerBase.CheckForNonElement(node);
			return false;
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x000E9FCF File Offset: 0x000E8FCF
		internal static void CheckForChildNodes(XmlNode node)
		{
			if (node.HasChildNodes)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_no_child_nodes"), node.FirstChild);
			}
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x000E9FEF File Offset: 0x000E8FEF
		internal static void ThrowUnrecognizedElement(XmlNode node)
		{
			throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_element"), node);
		}
	}
}

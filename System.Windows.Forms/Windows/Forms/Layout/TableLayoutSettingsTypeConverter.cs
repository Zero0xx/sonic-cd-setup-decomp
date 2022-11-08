using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Text;
using System.Xml;

namespace System.Windows.Forms.Layout
{
	// Token: 0x02000654 RID: 1620
	public class TableLayoutSettingsTypeConverter : TypeConverter
	{
		// Token: 0x06005512 RID: 21778 RVA: 0x0013608D File Offset: 0x0013508D
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06005513 RID: 21779 RVA: 0x001360A6 File Offset: 0x001350A6
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06005514 RID: 21780 RVA: 0x001360CC File Offset: 0x001350CC
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(value as string);
				TableLayoutSettings tableLayoutSettings = new TableLayoutSettings();
				this.ParseControls(tableLayoutSettings, xmlDocument.GetElementsByTagName("Control"));
				this.ParseStyles(tableLayoutSettings, xmlDocument.GetElementsByTagName("Columns"), true);
				this.ParseStyles(tableLayoutSettings, xmlDocument.GetElementsByTagName("Rows"), false);
				return tableLayoutSettings;
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06005515 RID: 21781 RVA: 0x0013613C File Offset: 0x0013513C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is TableLayoutSettings && destinationType == typeof(string))
			{
				TableLayoutSettings tableLayoutSettings = value as TableLayoutSettings;
				StringBuilder stringBuilder = new StringBuilder();
				XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);
				xmlWriter.WriteStartElement("TableLayoutSettings");
				xmlWriter.WriteStartElement("Controls");
				foreach (TableLayoutSettings.ControlInformation controlInformation in tableLayoutSettings.GetControlsInformation())
				{
					xmlWriter.WriteStartElement("Control");
					xmlWriter.WriteAttributeString("Name", controlInformation.Name.ToString());
					XmlWriter xmlWriter2 = xmlWriter;
					string localName = "Row";
					int row = controlInformation.Row;
					xmlWriter2.WriteAttributeString(localName, row.ToString(CultureInfo.CurrentCulture));
					XmlWriter xmlWriter3 = xmlWriter;
					string localName2 = "RowSpan";
					int rowSpan = controlInformation.RowSpan;
					xmlWriter3.WriteAttributeString(localName2, rowSpan.ToString(CultureInfo.CurrentCulture));
					XmlWriter xmlWriter4 = xmlWriter;
					string localName3 = "Column";
					int column = controlInformation.Column;
					xmlWriter4.WriteAttributeString(localName3, column.ToString(CultureInfo.CurrentCulture));
					XmlWriter xmlWriter5 = xmlWriter;
					string localName4 = "ColumnSpan";
					int columnSpan = controlInformation.ColumnSpan;
					xmlWriter5.WriteAttributeString(localName4, columnSpan.ToString(CultureInfo.CurrentCulture));
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("Columns");
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (object obj in ((IEnumerable)tableLayoutSettings.ColumnStyles))
				{
					ColumnStyle columnStyle = (ColumnStyle)obj;
					stringBuilder2.AppendFormat("{0},{1},", columnStyle.SizeType, columnStyle.Width);
				}
				if (stringBuilder2.Length > 0)
				{
					stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
				}
				xmlWriter.WriteAttributeString("Styles", stringBuilder2.ToString());
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("Rows");
				StringBuilder stringBuilder3 = new StringBuilder();
				foreach (object obj2 in ((IEnumerable)tableLayoutSettings.RowStyles))
				{
					RowStyle rowStyle = (RowStyle)obj2;
					stringBuilder3.AppendFormat("{0},{1},", rowStyle.SizeType, rowStyle.Height);
				}
				if (stringBuilder3.Length > 0)
				{
					stringBuilder3.Remove(stringBuilder3.Length - 1, 1);
				}
				xmlWriter.WriteAttributeString("Styles", stringBuilder3.ToString());
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.Close();
				return stringBuilder.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06005516 RID: 21782 RVA: 0x00136418 File Offset: 0x00135418
		private string GetAttributeValue(XmlNode node, string attribute)
		{
			XmlAttribute xmlAttribute = node.Attributes[attribute];
			if (xmlAttribute != null)
			{
				return xmlAttribute.Value;
			}
			return null;
		}

		// Token: 0x06005517 RID: 21783 RVA: 0x00136440 File Offset: 0x00135440
		private int GetAttributeValue(XmlNode node, string attribute, int valueIfNotFound)
		{
			string attributeValue = this.GetAttributeValue(node, attribute);
			int result;
			if (!string.IsNullOrEmpty(attributeValue) && int.TryParse(attributeValue, out result))
			{
				return result;
			}
			return valueIfNotFound;
		}

		// Token: 0x06005518 RID: 21784 RVA: 0x0013646C File Offset: 0x0013546C
		private void ParseControls(TableLayoutSettings settings, XmlNodeList controlXmlFragments)
		{
			foreach (object obj in controlXmlFragments)
			{
				XmlNode node = (XmlNode)obj;
				string attributeValue = this.GetAttributeValue(node, "Name");
				if (!string.IsNullOrEmpty(attributeValue))
				{
					int attributeValue2 = this.GetAttributeValue(node, "Row", -1);
					int attributeValue3 = this.GetAttributeValue(node, "RowSpan", 1);
					int attributeValue4 = this.GetAttributeValue(node, "Column", -1);
					int attributeValue5 = this.GetAttributeValue(node, "ColumnSpan", 1);
					settings.SetRow(attributeValue, attributeValue2);
					settings.SetColumn(attributeValue, attributeValue4);
					settings.SetRowSpan(attributeValue, attributeValue3);
					settings.SetColumnSpan(attributeValue, attributeValue5);
				}
			}
		}

		// Token: 0x06005519 RID: 21785 RVA: 0x00136534 File Offset: 0x00135534
		private void ParseStyles(TableLayoutSettings settings, XmlNodeList controlXmlFragments, bool columns)
		{
			foreach (object obj in controlXmlFragments)
			{
				XmlNode node = (XmlNode)obj;
				string attributeValue = this.GetAttributeValue(node, "Styles");
				Type typeFromHandle = typeof(SizeType);
				if (!string.IsNullOrEmpty(attributeValue))
				{
					int num;
					for (int i = 0; i < attributeValue.Length; i = num)
					{
						num = i;
						while (char.IsLetter(attributeValue[num]))
						{
							num++;
						}
						SizeType sizeType = (SizeType)Enum.Parse(typeFromHandle, attributeValue.Substring(i, num - i), true);
						while (!char.IsDigit(attributeValue[num]))
						{
							num++;
						}
						StringBuilder stringBuilder = new StringBuilder();
						while (num < attributeValue.Length && char.IsDigit(attributeValue[num]))
						{
							stringBuilder.Append(attributeValue[num]);
							num++;
						}
						stringBuilder.Append('.');
						while (num < attributeValue.Length && !char.IsLetter(attributeValue[num]))
						{
							if (char.IsDigit(attributeValue[num]))
							{
								stringBuilder.Append(attributeValue[num]);
							}
							num++;
						}
						string s = stringBuilder.ToString();
						float num2;
						if (!float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out num2))
						{
							num2 = 0f;
						}
						if (columns)
						{
							settings.ColumnStyles.Add(new ColumnStyle(sizeType, num2));
						}
						else
						{
							settings.RowStyles.Add(new RowStyle(sizeType, num2));
						}
					}
				}
			}
		}
	}
}

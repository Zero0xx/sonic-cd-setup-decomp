using System;
using System.Diagnostics;

namespace System.Xml
{
	// Token: 0x02000017 RID: 23
	internal static class DiagnosticsSwitches
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000038DC File Offset: 0x000028DC
		public static BooleanSwitch XmlSchemaContentModel
		{
			get
			{
				if (DiagnosticsSwitches.xmlSchemaContentModel == null)
				{
					DiagnosticsSwitches.xmlSchemaContentModel = new BooleanSwitch("XmlSchemaContentModel", "Enable tracing for the XmlSchema content model.");
				}
				return DiagnosticsSwitches.xmlSchemaContentModel;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005D RID: 93 RVA: 0x000038FE File Offset: 0x000028FE
		public static TraceSwitch XmlSchema
		{
			get
			{
				if (DiagnosticsSwitches.xmlSchema == null)
				{
					DiagnosticsSwitches.xmlSchema = new TraceSwitch("XmlSchema", "Enable tracing for the XmlSchema class.");
				}
				return DiagnosticsSwitches.xmlSchema;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00003920 File Offset: 0x00002920
		public static BooleanSwitch KeepTempFiles
		{
			get
			{
				if (DiagnosticsSwitches.keepTempFiles == null)
				{
					DiagnosticsSwitches.keepTempFiles = new BooleanSwitch("XmlSerialization.Compilation", "Keep XmlSerialization generated (temp) files.");
				}
				return DiagnosticsSwitches.keepTempFiles;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00003942 File Offset: 0x00002942
		public static BooleanSwitch PregenEventLog
		{
			get
			{
				if (DiagnosticsSwitches.pregenEventLog == null)
				{
					DiagnosticsSwitches.pregenEventLog = new BooleanSwitch("XmlSerialization.PregenEventLog", "Log failures while loading pre-generated XmlSerialization assembly.");
				}
				return DiagnosticsSwitches.pregenEventLog;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00003964 File Offset: 0x00002964
		public static TraceSwitch XmlSerialization
		{
			get
			{
				if (DiagnosticsSwitches.xmlSerialization == null)
				{
					DiagnosticsSwitches.xmlSerialization = new TraceSwitch("XmlSerialization", "Enable tracing for the System.Xml.Serialization component.");
				}
				return DiagnosticsSwitches.xmlSerialization;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00003986 File Offset: 0x00002986
		public static TraceSwitch XslTypeInference
		{
			get
			{
				if (DiagnosticsSwitches.xslTypeInference == null)
				{
					DiagnosticsSwitches.xslTypeInference = new TraceSwitch("XslTypeInference", "Enable tracing for the XSLT type inference algorithm.");
				}
				return DiagnosticsSwitches.xslTypeInference;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000039A8 File Offset: 0x000029A8
		public static BooleanSwitch NonRecursiveTypeLoading
		{
			get
			{
				if (DiagnosticsSwitches.nonRecursiveTypeLoading == null)
				{
					DiagnosticsSwitches.nonRecursiveTypeLoading = new BooleanSwitch("XmlSerialization.NonRecursiveTypeLoading", "Turn on non-recursive algorithm generating XmlMappings for CLR types.");
				}
				return DiagnosticsSwitches.nonRecursiveTypeLoading;
			}
		}

		// Token: 0x04000472 RID: 1138
		private static BooleanSwitch xmlSchemaContentModel;

		// Token: 0x04000473 RID: 1139
		private static TraceSwitch xmlSchema;

		// Token: 0x04000474 RID: 1140
		private static BooleanSwitch keepTempFiles;

		// Token: 0x04000475 RID: 1141
		private static BooleanSwitch pregenEventLog;

		// Token: 0x04000476 RID: 1142
		private static TraceSwitch xmlSerialization;

		// Token: 0x04000477 RID: 1143
		private static TraceSwitch xslTypeInference;

		// Token: 0x04000478 RID: 1144
		private static BooleanSwitch nonRecursiveTypeLoading;
	}
}

using System;
using System.Configuration.Internal;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x020006E9 RID: 1769
	[Serializable]
	public class ConfigurationException : SystemException
	{
		// Token: 0x060036A6 RID: 13990 RVA: 0x000E9421 File Offset: 0x000E8421
		private void Init(string filename, int line)
		{
			base.HResult = -2146232062;
			this._filename = filename;
			this._line = line;
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x000E943C File Offset: 0x000E843C
		protected ConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.Init(info.GetString("filename"), info.GetInt32("line"));
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x000E9462 File Offset: 0x000E8462
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException() : this(null, null, null, 0)
		{
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x000E946E File Offset: 0x000E846E
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message) : this(message, null, null, 0)
		{
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x000E947A File Offset: 0x000E847A
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message, Exception inner) : this(message, inner, null, 0)
		{
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x000E9486 File Offset: 0x000E8486
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message, XmlNode node) : this(message, null, ConfigurationException.GetUnsafeXmlNodeFilename(node), ConfigurationException.GetXmlNodeLineNumber(node))
		{
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x000E949C File Offset: 0x000E849C
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message, Exception inner, XmlNode node) : this(message, inner, ConfigurationException.GetUnsafeXmlNodeFilename(node), ConfigurationException.GetXmlNodeLineNumber(node))
		{
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x000E94B2 File Offset: 0x000E84B2
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message, string filename, int line) : this(message, null, filename, line)
		{
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x000E94BE File Offset: 0x000E84BE
		[Obsolete("This class is obsolete, to create a new exception create a System.Configuration!System.Configuration.ConfigurationErrorsException")]
		public ConfigurationException(string message, Exception inner, string filename, int line) : base(message, inner)
		{
			this.Init(filename, line);
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x000E94D1 File Offset: 0x000E84D1
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("filename", this._filename);
			info.AddValue("line", this._line);
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x060036B0 RID: 14000 RVA: 0x000E9500 File Offset: 0x000E8500
		public override string Message
		{
			get
			{
				string filename = this.Filename;
				if (!string.IsNullOrEmpty(filename))
				{
					if (this.Line != 0)
					{
						return string.Concat(new string[]
						{
							this.BareMessage,
							" (",
							filename,
							" line ",
							this.Line.ToString(CultureInfo.InvariantCulture),
							")"
						});
					}
					return this.BareMessage + " (" + filename + ")";
				}
				else
				{
					if (this.Line != 0)
					{
						return this.BareMessage + " (line " + this.Line.ToString("G", CultureInfo.InvariantCulture) + ")";
					}
					return this.BareMessage;
				}
			}
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x060036B1 RID: 14001 RVA: 0x000E95C0 File Offset: 0x000E85C0
		public virtual string BareMessage
		{
			get
			{
				return base.Message;
			}
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x060036B2 RID: 14002 RVA: 0x000E95C8 File Offset: 0x000E85C8
		public virtual string Filename
		{
			get
			{
				return ConfigurationException.SafeFilename(this._filename);
			}
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x060036B3 RID: 14003 RVA: 0x000E95D5 File Offset: 0x000E85D5
		public virtual int Line
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x000E95DD File Offset: 0x000E85DD
		[Obsolete("This class is obsolete, use System.Configuration!System.Configuration.ConfigurationErrorsException.GetFilename instead")]
		public static string GetXmlNodeFilename(XmlNode node)
		{
			return ConfigurationException.SafeFilename(ConfigurationException.GetUnsafeXmlNodeFilename(node));
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x000E95EC File Offset: 0x000E85EC
		[Obsolete("This class is obsolete, use System.Configuration!System.Configuration.ConfigurationErrorsException.GetLinenumber instead")]
		public static int GetXmlNodeLineNumber(XmlNode node)
		{
			IConfigErrorInfo configErrorInfo = node as IConfigErrorInfo;
			if (configErrorInfo != null)
			{
				return configErrorInfo.LineNumber;
			}
			return 0;
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x000E960C File Offset: 0x000E860C
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
		private static string FullPathWithAssert(string filename)
		{
			string result = null;
			try
			{
				result = Path.GetFullPath(filename);
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x000E9638 File Offset: 0x000E8638
		internal static string SafeFilename(string filename)
		{
			if (string.IsNullOrEmpty(filename))
			{
				return filename;
			}
			if (filename.StartsWith("http:", StringComparison.OrdinalIgnoreCase))
			{
				return filename;
			}
			try
			{
				Path.GetFullPath(filename);
			}
			catch (SecurityException)
			{
				try
				{
					string path = ConfigurationException.FullPathWithAssert(filename);
					filename = Path.GetFileName(path);
				}
				catch
				{
					filename = null;
				}
			}
			catch
			{
				filename = null;
			}
			return filename;
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x000E96B4 File Offset: 0x000E86B4
		private static string GetUnsafeXmlNodeFilename(XmlNode node)
		{
			IConfigErrorInfo configErrorInfo = node as IConfigErrorInfo;
			if (configErrorInfo != null)
			{
				return configErrorInfo.Filename;
			}
			return string.Empty;
		}

		// Token: 0x0400318D RID: 12685
		private const string HTTP_PREFIX = "http:";

		// Token: 0x0400318E RID: 12686
		private string _filename;

		// Token: 0x0400318F RID: 12687
		private int _line;
	}
}

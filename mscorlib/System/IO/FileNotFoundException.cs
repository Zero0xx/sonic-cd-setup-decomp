using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.IO
{
	// Token: 0x020005B9 RID: 1465
	[ComVisible(true)]
	[Serializable]
	public class FileNotFoundException : IOException
	{
		// Token: 0x06003616 RID: 13846 RVA: 0x000B45E6 File Offset: 0x000B35E6
		public FileNotFoundException() : base(Environment.GetResourceString("IO.FileNotFound"))
		{
			base.SetErrorCode(-2147024894);
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x000B4603 File Offset: 0x000B3603
		public FileNotFoundException(string message) : base(message)
		{
			base.SetErrorCode(-2147024894);
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x000B4617 File Offset: 0x000B3617
		public FileNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147024894);
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x000B462C File Offset: 0x000B362C
		public FileNotFoundException(string message, string fileName) : base(message)
		{
			base.SetErrorCode(-2147024894);
			this._fileName = fileName;
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x000B4647 File Offset: 0x000B3647
		public FileNotFoundException(string message, string fileName, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147024894);
			this._fileName = fileName;
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x0600361B RID: 13851 RVA: 0x000B4663 File Offset: 0x000B3663
		public override string Message
		{
			get
			{
				this.SetMessageField();
				return this._message;
			}
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x000B4674 File Offset: 0x000B3674
		private void SetMessageField()
		{
			if (this._message == null)
			{
				if (this._fileName == null && base.HResult == -2146233088)
				{
					this._message = Environment.GetResourceString("IO.FileNotFound");
					return;
				}
				if (this._fileName != null)
				{
					this._message = FileLoadException.FormatFileLoadExceptionMessage(this._fileName, base.HResult);
				}
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x0600361D RID: 13853 RVA: 0x000B46CE File Offset: 0x000B36CE
		public string FileName
		{
			get
			{
				return this._fileName;
			}
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x000B46D8 File Offset: 0x000B36D8
		public override string ToString()
		{
			string text = base.GetType().FullName + ": " + this.Message;
			if (this._fileName != null && this._fileName.Length != 0)
			{
				text = text + Environment.NewLine + string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.FileName_Name"), new object[]
				{
					this._fileName
				});
			}
			if (base.InnerException != null)
			{
				text = text + " ---> " + base.InnerException.ToString();
			}
			if (this.StackTrace != null)
			{
				text = text + Environment.NewLine + this.StackTrace;
			}
			try
			{
				if (this.FusionLog != null)
				{
					if (text == null)
					{
						text = " ";
					}
					text += Environment.NewLine;
					text += Environment.NewLine;
					text += this.FusionLog;
				}
			}
			catch (SecurityException)
			{
			}
			return text;
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x000B47CC File Offset: 0x000B37CC
		protected FileNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._fileName = info.GetString("FileNotFound_FileName");
			try
			{
				this._fusionLog = info.GetString("FileNotFound_FusionLog");
			}
			catch
			{
				this._fusionLog = null;
			}
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x000B4820 File Offset: 0x000B3820
		private FileNotFoundException(string fileName, string fusionLog, int hResult) : base(null)
		{
			base.SetErrorCode(hResult);
			this._fileName = fileName;
			this._fusionLog = fusionLog;
			this.SetMessageField();
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06003621 RID: 13857 RVA: 0x000B4844 File Offset: 0x000B3844
		public string FusionLog
		{
			[SecurityPermission(SecurityAction.Demand, Flags = (SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy))]
			get
			{
				return this._fusionLog;
			}
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x000B484C File Offset: 0x000B384C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("FileNotFound_FileName", this._fileName, typeof(string));
			try
			{
				info.AddValue("FileNotFound_FusionLog", this.FusionLog, typeof(string));
			}
			catch (SecurityException)
			{
			}
		}

		// Token: 0x04001C40 RID: 7232
		private string _fileName;

		// Token: 0x04001C41 RID: 7233
		private string _fusionLog;
	}
}

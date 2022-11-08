using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;
using System.Security.Permissions;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x020007BC RID: 1980
	[SoapType(Embedded = true)]
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapFault : ISerializable
	{
		// Token: 0x06004689 RID: 18057 RVA: 0x000F0854 File Offset: 0x000EF854
		public SoapFault()
		{
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x000F085C File Offset: 0x000EF85C
		public SoapFault(string faultCode, string faultString, string faultActor, ServerFault serverFault)
		{
			this.faultCode = faultCode;
			this.faultString = faultString;
			this.faultActor = faultActor;
			this.detail = serverFault;
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x000F0884 File Offset: 0x000EF884
		internal SoapFault(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string name = enumerator.Name;
				object value = enumerator.Value;
				if (string.Compare(name, "faultCode", true, CultureInfo.InvariantCulture) == 0)
				{
					int num = ((string)value).IndexOf(':');
					if (num > -1)
					{
						this.faultCode = ((string)value).Substring(num + 1);
					}
					else
					{
						this.faultCode = (string)value;
					}
				}
				else if (string.Compare(name, "faultString", true, CultureInfo.InvariantCulture) == 0)
				{
					this.faultString = (string)value;
				}
				else if (string.Compare(name, "faultActor", true, CultureInfo.InvariantCulture) == 0)
				{
					this.faultActor = (string)value;
				}
				else if (string.Compare(name, "detail", true, CultureInfo.InvariantCulture) == 0)
				{
					this.detail = value;
				}
			}
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x000F0964 File Offset: 0x000EF964
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("faultcode", "SOAP-ENV:" + this.faultCode);
			info.AddValue("faultstring", this.faultString);
			if (this.faultActor != null)
			{
				info.AddValue("faultactor", this.faultActor);
			}
			info.AddValue("detail", this.detail, typeof(object));
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x0600468D RID: 18061 RVA: 0x000F09D1 File Offset: 0x000EF9D1
		// (set) Token: 0x0600468E RID: 18062 RVA: 0x000F09D9 File Offset: 0x000EF9D9
		public string FaultCode
		{
			get
			{
				return this.faultCode;
			}
			set
			{
				this.faultCode = value;
			}
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x0600468F RID: 18063 RVA: 0x000F09E2 File Offset: 0x000EF9E2
		// (set) Token: 0x06004690 RID: 18064 RVA: 0x000F09EA File Offset: 0x000EF9EA
		public string FaultString
		{
			get
			{
				return this.faultString;
			}
			set
			{
				this.faultString = value;
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06004691 RID: 18065 RVA: 0x000F09F3 File Offset: 0x000EF9F3
		// (set) Token: 0x06004692 RID: 18066 RVA: 0x000F09FB File Offset: 0x000EF9FB
		public string FaultActor
		{
			get
			{
				return this.faultActor;
			}
			set
			{
				this.faultActor = value;
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06004693 RID: 18067 RVA: 0x000F0A04 File Offset: 0x000EFA04
		// (set) Token: 0x06004694 RID: 18068 RVA: 0x000F0A0C File Offset: 0x000EFA0C
		public object Detail
		{
			get
			{
				return this.detail;
			}
			set
			{
				this.detail = value;
			}
		}

		// Token: 0x04002314 RID: 8980
		private string faultCode;

		// Token: 0x04002315 RID: 8981
		private string faultString;

		// Token: 0x04002316 RID: 8982
		private string faultActor;

		// Token: 0x04002317 RID: 8983
		[SoapField(Embedded = true)]
		private object detail;
	}
}

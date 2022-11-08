using System;
using System.Reflection;

namespace System.Runtime.Remoting.Metadata
{
	// Token: 0x02000740 RID: 1856
	internal class RemotingCachedData
	{
		// Token: 0x0600426E RID: 17006 RVA: 0x000E214A File Offset: 0x000E114A
		internal RemotingCachedData(object ri)
		{
			this.RI = ri;
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x000E215C File Offset: 0x000E115C
		internal SoapAttribute GetSoapAttribute()
		{
			if (this._soapAttr == null)
			{
				lock (this)
				{
					if (this._soapAttr == null)
					{
						SoapAttribute soapAttribute = null;
						ICustomAttributeProvider customAttributeProvider = (ICustomAttributeProvider)this.RI;
						if (this.RI is Type)
						{
							object[] customAttributes = customAttributeProvider.GetCustomAttributes(typeof(SoapTypeAttribute), true);
							if (customAttributes != null && customAttributes.Length != 0)
							{
								soapAttribute = (SoapAttribute)customAttributes[0];
							}
							else
							{
								soapAttribute = new SoapTypeAttribute();
							}
						}
						else if (this.RI is MethodBase)
						{
							object[] customAttributes2 = customAttributeProvider.GetCustomAttributes(typeof(SoapMethodAttribute), true);
							if (customAttributes2 != null && customAttributes2.Length != 0)
							{
								soapAttribute = (SoapAttribute)customAttributes2[0];
							}
							else
							{
								soapAttribute = new SoapMethodAttribute();
							}
						}
						else if (this.RI is FieldInfo)
						{
							object[] customAttributes3 = customAttributeProvider.GetCustomAttributes(typeof(SoapFieldAttribute), false);
							if (customAttributes3 != null && customAttributes3.Length != 0)
							{
								soapAttribute = (SoapAttribute)customAttributes3[0];
							}
							else
							{
								soapAttribute = new SoapFieldAttribute();
							}
						}
						else if (this.RI is ParameterInfo)
						{
							object[] customAttributes4 = customAttributeProvider.GetCustomAttributes(typeof(SoapParameterAttribute), true);
							if (customAttributes4 != null && customAttributes4.Length != 0)
							{
								soapAttribute = (SoapParameterAttribute)customAttributes4[0];
							}
							else
							{
								soapAttribute = new SoapParameterAttribute();
							}
						}
						soapAttribute.SetReflectInfo(this.RI);
						this._soapAttr = soapAttribute;
					}
				}
			}
			return this._soapAttr;
		}

		// Token: 0x04002145 RID: 8517
		protected object RI;

		// Token: 0x04002146 RID: 8518
		private SoapAttribute _soapAttr;
	}
}

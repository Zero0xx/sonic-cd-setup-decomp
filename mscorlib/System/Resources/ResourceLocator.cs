using System;

namespace System.Resources
{
	// Token: 0x02000436 RID: 1078
	internal struct ResourceLocator
	{
		// Token: 0x06002C04 RID: 11268 RVA: 0x00094183 File Offset: 0x00093183
		internal ResourceLocator(int dataPos, object value)
		{
			this._dataPos = dataPos;
			this._value = value;
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06002C05 RID: 11269 RVA: 0x00094193 File Offset: 0x00093193
		internal int DataPosition
		{
			get
			{
				return this._dataPos;
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06002C06 RID: 11270 RVA: 0x0009419B File Offset: 0x0009319B
		// (set) Token: 0x06002C07 RID: 11271 RVA: 0x000941A3 File Offset: 0x000931A3
		internal object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x000941AC File Offset: 0x000931AC
		internal static bool CanCache(ResourceTypeCode value)
		{
			return value <= ResourceTypeCode.TimeSpan;
		}

		// Token: 0x04001576 RID: 5494
		internal object _value;

		// Token: 0x04001577 RID: 5495
		internal int _dataPos;
	}
}

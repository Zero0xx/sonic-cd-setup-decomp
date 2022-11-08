using System;
using System.Collections;
using System.Globalization;

namespace System.Diagnostics
{
	// Token: 0x0200075D RID: 1885
	public class InstanceDataCollection : DictionaryBase
	{
		// Token: 0x060039E3 RID: 14819 RVA: 0x000F5133 File Offset: 0x000F4133
		[Obsolete("This constructor has been deprecated.  Please use System.Diagnostics.InstanceDataCollectionCollection.get_Item to get an instance of this collection instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public InstanceDataCollection(string counterName)
		{
			if (counterName == null)
			{
				throw new ArgumentNullException("counterName");
			}
			this.counterName = counterName;
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x060039E4 RID: 14820 RVA: 0x000F5150 File Offset: 0x000F4150
		public string CounterName
		{
			get
			{
				return this.counterName;
			}
		}

		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x060039E5 RID: 14821 RVA: 0x000F5158 File Offset: 0x000F4158
		public ICollection Keys
		{
			get
			{
				return base.Dictionary.Keys;
			}
		}

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x060039E6 RID: 14822 RVA: 0x000F5165 File Offset: 0x000F4165
		public ICollection Values
		{
			get
			{
				return base.Dictionary.Values;
			}
		}

		// Token: 0x17000D84 RID: 3460
		public InstanceData this[string instanceName]
		{
			get
			{
				if (instanceName == null)
				{
					throw new ArgumentNullException("instanceName");
				}
				if (instanceName.Length == 0)
				{
					instanceName = "systemdiagnosticsperfcounterlibsingleinstance";
				}
				object key = instanceName.ToLower(CultureInfo.InvariantCulture);
				return (InstanceData)base.Dictionary[key];
			}
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x000F51BC File Offset: 0x000F41BC
		internal void Add(string instanceName, InstanceData value)
		{
			object key = instanceName.ToLower(CultureInfo.InvariantCulture);
			base.Dictionary.Add(key, value);
		}

		// Token: 0x060039E9 RID: 14825 RVA: 0x000F51E4 File Offset: 0x000F41E4
		public bool Contains(string instanceName)
		{
			if (instanceName == null)
			{
				throw new ArgumentNullException("instanceName");
			}
			object key = instanceName.ToLower(CultureInfo.InvariantCulture);
			return base.Dictionary.Contains(key);
		}

		// Token: 0x060039EA RID: 14826 RVA: 0x000F5217 File Offset: 0x000F4217
		public void CopyTo(InstanceData[] instances, int index)
		{
			base.Dictionary.Values.CopyTo(instances, index);
		}

		// Token: 0x040032E0 RID: 13024
		private string counterName;
	}
}

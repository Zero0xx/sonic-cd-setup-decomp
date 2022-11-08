using System;
using System.Collections;
using System.Globalization;

namespace System.Diagnostics
{
	// Token: 0x0200075E RID: 1886
	public class InstanceDataCollectionCollection : DictionaryBase
	{
		// Token: 0x060039EB RID: 14827 RVA: 0x000F522B File Offset: 0x000F422B
		[Obsolete("This constructor has been deprecated.  Please use System.Diagnostics.PerformanceCounterCategory.ReadCategory() to get an instance of this collection instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public InstanceDataCollectionCollection()
		{
		}

		// Token: 0x17000D85 RID: 3461
		public InstanceDataCollection this[string counterName]
		{
			get
			{
				if (counterName == null)
				{
					throw new ArgumentNullException("counterName");
				}
				object key = counterName.ToLower(CultureInfo.InvariantCulture);
				return (InstanceDataCollection)base.Dictionary[key];
			}
		}

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x060039ED RID: 14829 RVA: 0x000F526C File Offset: 0x000F426C
		public ICollection Keys
		{
			get
			{
				return base.Dictionary.Keys;
			}
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x060039EE RID: 14830 RVA: 0x000F5279 File Offset: 0x000F4279
		public ICollection Values
		{
			get
			{
				return base.Dictionary.Values;
			}
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x000F5288 File Offset: 0x000F4288
		internal void Add(string counterName, InstanceDataCollection value)
		{
			object key = counterName.ToLower(CultureInfo.InvariantCulture);
			base.Dictionary.Add(key, value);
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x000F52B0 File Offset: 0x000F42B0
		public bool Contains(string counterName)
		{
			if (counterName == null)
			{
				throw new ArgumentNullException("counterName");
			}
			object key = counterName.ToLower(CultureInfo.InvariantCulture);
			return base.Dictionary.Contains(key);
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x000F52E3 File Offset: 0x000F42E3
		public void CopyTo(InstanceDataCollection[] counters, int index)
		{
			base.Dictionary.Values.CopyTo(counters, index);
		}
	}
}

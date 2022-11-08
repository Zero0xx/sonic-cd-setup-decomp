using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x02000741 RID: 1857
	[Serializable]
	public class CounterCreationDataCollection : CollectionBase
	{
		// Token: 0x0600389A RID: 14490 RVA: 0x000EF057 File Offset: 0x000EE057
		public CounterCreationDataCollection()
		{
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x000EF05F File Offset: 0x000EE05F
		public CounterCreationDataCollection(CounterCreationDataCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x000EF06E File Offset: 0x000EE06E
		public CounterCreationDataCollection(CounterCreationData[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000D1D RID: 3357
		public CounterCreationData this[int index]
		{
			get
			{
				return (CounterCreationData)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x000EF09F File Offset: 0x000EE09F
		public int Add(CounterCreationData value)
		{
			return base.List.Add(value);
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x000EF0B0 File Offset: 0x000EE0B0
		public void AddRange(CounterCreationData[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x060038A1 RID: 14497 RVA: 0x000EF0E4 File Offset: 0x000EE0E4
		public void AddRange(CounterCreationDataCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int count = value.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x000EF120 File Offset: 0x000EE120
		public bool Contains(CounterCreationData value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x000EF12E File Offset: 0x000EE12E
		public void CopyTo(CounterCreationData[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x000EF13D File Offset: 0x000EE13D
		public int IndexOf(CounterCreationData value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060038A5 RID: 14501 RVA: 0x000EF14B File Offset: 0x000EE14B
		public void Insert(int index, CounterCreationData value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x000EF15A File Offset: 0x000EE15A
		public virtual void Remove(CounterCreationData value)
		{
			base.List.Remove(value);
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x000EF168 File Offset: 0x000EE168
		protected override void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is CounterCreationData))
			{
				throw new ArgumentException(SR.GetString("MustAddCounterCreationData"));
			}
		}
	}
}

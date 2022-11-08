using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x0200037A RID: 890
	[ComVisible(true)]
	public sealed class SerializationInfoEnumerator : IEnumerator
	{
		// Token: 0x060022E9 RID: 8937 RVA: 0x000582C5 File Offset: 0x000572C5
		internal SerializationInfoEnumerator(string[] members, object[] info, Type[] types, int numItems)
		{
			this.m_members = members;
			this.m_data = info;
			this.m_types = types;
			this.m_numItems = numItems - 1;
			this.m_currItem = -1;
			this.m_current = false;
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x000582FA File Offset: 0x000572FA
		public bool MoveNext()
		{
			if (this.m_currItem < this.m_numItems)
			{
				this.m_currItem++;
				this.m_current = true;
			}
			else
			{
				this.m_current = false;
			}
			return this.m_current;
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x060022EB RID: 8939 RVA: 0x00058330 File Offset: 0x00057330
		object IEnumerator.Current
		{
			get
			{
				if (!this.m_current)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
				return new SerializationEntry(this.m_members[this.m_currItem], this.m_data[this.m_currItem], this.m_types[this.m_currItem]);
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x060022EC RID: 8940 RVA: 0x00058388 File Offset: 0x00057388
		public SerializationEntry Current
		{
			get
			{
				if (!this.m_current)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
				return new SerializationEntry(this.m_members[this.m_currItem], this.m_data[this.m_currItem], this.m_types[this.m_currItem]);
			}
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x000583D9 File Offset: 0x000573D9
		public void Reset()
		{
			this.m_currItem = -1;
			this.m_current = false;
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x060022EE RID: 8942 RVA: 0x000583E9 File Offset: 0x000573E9
		public string Name
		{
			get
			{
				if (!this.m_current)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
				return this.m_members[this.m_currItem];
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x060022EF RID: 8943 RVA: 0x00058410 File Offset: 0x00057410
		public object Value
		{
			get
			{
				if (!this.m_current)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
				return this.m_data[this.m_currItem];
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x060022F0 RID: 8944 RVA: 0x00058437 File Offset: 0x00057437
		public Type ObjectType
		{
			get
			{
				if (!this.m_current)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumOpCantHappen"));
				}
				return this.m_types[this.m_currItem];
			}
		}

		// Token: 0x04000EA8 RID: 3752
		private string[] m_members;

		// Token: 0x04000EA9 RID: 3753
		private object[] m_data;

		// Token: 0x04000EAA RID: 3754
		private Type[] m_types;

		// Token: 0x04000EAB RID: 3755
		private int m_numItems;

		// Token: 0x04000EAC RID: 3756
		private int m_currItem;

		// Token: 0x04000EAD RID: 3757
		private bool m_current;
	}
}

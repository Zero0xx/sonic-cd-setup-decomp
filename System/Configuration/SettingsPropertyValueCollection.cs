using System;
using System.Collections;

namespace System.Configuration
{
	// Token: 0x02000716 RID: 1814
	public class SettingsPropertyValueCollection : ICloneable, ICollection, IEnumerable
	{
		// Token: 0x06003789 RID: 14217 RVA: 0x000EB7F8 File Offset: 0x000EA7F8
		public SettingsPropertyValueCollection()
		{
			this._Indices = new Hashtable(10, CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
			this._Values = new ArrayList();
		}

		// Token: 0x0600378A RID: 14218 RVA: 0x000EB824 File Offset: 0x000EA824
		public void Add(SettingsPropertyValue property)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			int num = this._Values.Add(property);
			try
			{
				this._Indices.Add(property.Name, num);
			}
			catch (Exception)
			{
				this._Values.RemoveAt(num);
				throw;
			}
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x000EB884 File Offset: 0x000EA884
		public void Remove(string name)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			object obj = this._Indices[name];
			if (obj == null || !(obj is int))
			{
				return;
			}
			int num = (int)obj;
			if (num >= this._Values.Count)
			{
				return;
			}
			this._Values.RemoveAt(num);
			this._Indices.Remove(name);
			ArrayList arrayList = new ArrayList();
			foreach (object obj2 in this._Indices)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
				if ((int)dictionaryEntry.Value > num)
				{
					arrayList.Add(dictionaryEntry.Key);
				}
			}
			foreach (object obj3 in arrayList)
			{
				string key = (string)obj3;
				this._Indices[key] = (int)this._Indices[key] - 1;
			}
		}

		// Token: 0x17000CE2 RID: 3298
		public SettingsPropertyValue this[string name]
		{
			get
			{
				object obj = this._Indices[name];
				if (obj == null || !(obj is int))
				{
					return null;
				}
				int num = (int)obj;
				if (num >= this._Values.Count)
				{
					return null;
				}
				return (SettingsPropertyValue)this._Values[num];
			}
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x000EBA0F File Offset: 0x000EAA0F
		public IEnumerator GetEnumerator()
		{
			return this._Values.GetEnumerator();
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x000EBA1C File Offset: 0x000EAA1C
		public object Clone()
		{
			return new SettingsPropertyValueCollection(this._Indices, this._Values);
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x000EBA2F File Offset: 0x000EAA2F
		public void SetReadOnly()
		{
			if (this._ReadOnly)
			{
				return;
			}
			this._ReadOnly = true;
			this._Values = ArrayList.ReadOnly(this._Values);
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x000EBA52 File Offset: 0x000EAA52
		public void Clear()
		{
			this._Values.Clear();
			this._Indices.Clear();
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06003791 RID: 14225 RVA: 0x000EBA6A File Offset: 0x000EAA6A
		public int Count
		{
			get
			{
				return this._Values.Count;
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06003792 RID: 14226 RVA: 0x000EBA77 File Offset: 0x000EAA77
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06003793 RID: 14227 RVA: 0x000EBA7A File Offset: 0x000EAA7A
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x000EBA7D File Offset: 0x000EAA7D
		public void CopyTo(Array array, int index)
		{
			this._Values.CopyTo(array, index);
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x000EBA8C File Offset: 0x000EAA8C
		private SettingsPropertyValueCollection(Hashtable indices, ArrayList values)
		{
			this._Indices = (Hashtable)indices.Clone();
			this._Values = (ArrayList)values.Clone();
		}

		// Token: 0x040031CB RID: 12747
		private Hashtable _Indices;

		// Token: 0x040031CC RID: 12748
		private ArrayList _Values;

		// Token: 0x040031CD RID: 12749
		private bool _ReadOnly;
	}
}

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007BC RID: 1980
	internal class MergePropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x060068BB RID: 26811 RVA: 0x001809C0 File Offset: 0x0017F9C0
		public MergePropertyDescriptor(PropertyDescriptor[] descriptors) : base(descriptors[0].Name, null)
		{
			this.descriptors = descriptors;
		}

		// Token: 0x1700164A RID: 5706
		// (get) Token: 0x060068BC RID: 26812 RVA: 0x001809D8 File Offset: 0x0017F9D8
		public override Type ComponentType
		{
			get
			{
				return this.descriptors[0].ComponentType;
			}
		}

		// Token: 0x1700164B RID: 5707
		// (get) Token: 0x060068BD RID: 26813 RVA: 0x001809E7 File Offset: 0x0017F9E7
		public override TypeConverter Converter
		{
			get
			{
				return this.descriptors[0].Converter;
			}
		}

		// Token: 0x1700164C RID: 5708
		// (get) Token: 0x060068BE RID: 26814 RVA: 0x001809F6 File Offset: 0x0017F9F6
		public override string DisplayName
		{
			get
			{
				return this.descriptors[0].DisplayName;
			}
		}

		// Token: 0x1700164D RID: 5709
		// (get) Token: 0x060068BF RID: 26815 RVA: 0x00180A08 File Offset: 0x0017FA08
		public override bool IsLocalizable
		{
			get
			{
				if (this.localizable == MergePropertyDescriptor.TriState.Unknown)
				{
					this.localizable = MergePropertyDescriptor.TriState.Yes;
					foreach (PropertyDescriptor propertyDescriptor in this.descriptors)
					{
						if (!propertyDescriptor.IsLocalizable)
						{
							this.localizable = MergePropertyDescriptor.TriState.No;
							break;
						}
					}
				}
				return this.localizable == MergePropertyDescriptor.TriState.Yes;
			}
		}

		// Token: 0x1700164E RID: 5710
		// (get) Token: 0x060068C0 RID: 26816 RVA: 0x00180A58 File Offset: 0x0017FA58
		public override bool IsReadOnly
		{
			get
			{
				if (this.readOnly == MergePropertyDescriptor.TriState.Unknown)
				{
					this.readOnly = MergePropertyDescriptor.TriState.No;
					foreach (PropertyDescriptor propertyDescriptor in this.descriptors)
					{
						if (propertyDescriptor.IsReadOnly)
						{
							this.readOnly = MergePropertyDescriptor.TriState.Yes;
							break;
						}
					}
				}
				return this.readOnly == MergePropertyDescriptor.TriState.Yes;
			}
		}

		// Token: 0x1700164F RID: 5711
		// (get) Token: 0x060068C1 RID: 26817 RVA: 0x00180AA7 File Offset: 0x0017FAA7
		public override Type PropertyType
		{
			get
			{
				return this.descriptors[0].PropertyType;
			}
		}

		// Token: 0x17001650 RID: 5712
		public PropertyDescriptor this[int index]
		{
			get
			{
				return this.descriptors[index];
			}
		}

		// Token: 0x060068C3 RID: 26819 RVA: 0x00180AC0 File Offset: 0x0017FAC0
		public override bool CanResetValue(object component)
		{
			if (this.canReset == MergePropertyDescriptor.TriState.Unknown)
			{
				this.canReset = MergePropertyDescriptor.TriState.Yes;
				Array a = (Array)component;
				for (int i = 0; i < this.descriptors.Length; i++)
				{
					if (!this.descriptors[i].CanResetValue(this.GetPropertyOwnerForComponent(a, i)))
					{
						this.canReset = MergePropertyDescriptor.TriState.No;
						break;
					}
				}
			}
			return this.canReset == MergePropertyDescriptor.TriState.Yes;
		}

		// Token: 0x060068C4 RID: 26820 RVA: 0x00180B20 File Offset: 0x0017FB20
		private object CopyValue(object value)
		{
			if (value == null)
			{
				return value;
			}
			Type type = value.GetType();
			if (type.IsValueType)
			{
				return value;
			}
			object obj = null;
			ICloneable cloneable = value as ICloneable;
			if (cloneable != null)
			{
				obj = cloneable.Clone();
			}
			if (obj == null)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(value);
				if (converter.CanConvertTo(typeof(InstanceDescriptor)))
				{
					InstanceDescriptor instanceDescriptor = (InstanceDescriptor)converter.ConvertTo(null, CultureInfo.InvariantCulture, value, typeof(InstanceDescriptor));
					if (instanceDescriptor != null && instanceDescriptor.IsComplete)
					{
						obj = instanceDescriptor.Invoke();
					}
				}
				if (obj == null && converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
				{
					object obj2 = converter.ConvertToInvariantString(value);
					obj = converter.ConvertFromInvariantString((string)obj2);
				}
			}
			if (obj == null && type.IsSerializable)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				MemoryStream memoryStream = new MemoryStream();
				binaryFormatter.Serialize(memoryStream, value);
				memoryStream.Position = 0L;
				obj = binaryFormatter.Deserialize(memoryStream);
			}
			if (obj != null)
			{
				return obj;
			}
			return value;
		}

		// Token: 0x060068C5 RID: 26821 RVA: 0x00180C22 File Offset: 0x0017FC22
		protected override AttributeCollection CreateAttributeCollection()
		{
			return new MergePropertyDescriptor.MergedAttributeCollection(this);
		}

		// Token: 0x060068C6 RID: 26822 RVA: 0x00180C2C File Offset: 0x0017FC2C
		private object GetPropertyOwnerForComponent(Array a, int i)
		{
			object obj = a.GetValue(i);
			if (obj is ICustomTypeDescriptor)
			{
				obj = ((ICustomTypeDescriptor)obj).GetPropertyOwner(this.descriptors[i]);
			}
			return obj;
		}

		// Token: 0x060068C7 RID: 26823 RVA: 0x00180C5E File Offset: 0x0017FC5E
		public override object GetEditor(Type editorBaseType)
		{
			return this.descriptors[0].GetEditor(editorBaseType);
		}

		// Token: 0x060068C8 RID: 26824 RVA: 0x00180C70 File Offset: 0x0017FC70
		public override object GetValue(object component)
		{
			bool flag;
			return this.GetValue((Array)component, out flag);
		}

		// Token: 0x060068C9 RID: 26825 RVA: 0x00180C8C File Offset: 0x0017FC8C
		public object GetValue(Array components, out bool allEqual)
		{
			allEqual = true;
			object value = this.descriptors[0].GetValue(this.GetPropertyOwnerForComponent(components, 0));
			if (value is ICollection)
			{
				if (this.collection == null)
				{
					this.collection = new MergePropertyDescriptor.MultiMergeCollection((ICollection)value);
				}
				else
				{
					if (this.collection.Locked)
					{
						return this.collection;
					}
					this.collection.SetItems((ICollection)value);
				}
			}
			for (int i = 1; i < this.descriptors.Length; i++)
			{
				object value2 = this.descriptors[i].GetValue(this.GetPropertyOwnerForComponent(components, i));
				if (this.collection != null)
				{
					if (!this.collection.MergeCollection((ICollection)value2))
					{
						allEqual = false;
						return null;
					}
				}
				else if ((value != null || value2 != null) && (value == null || !value.Equals(value2)))
				{
					allEqual = false;
					return null;
				}
			}
			if (allEqual && this.collection != null && this.collection.Count == 0)
			{
				return null;
			}
			if (this.collection == null)
			{
				return value;
			}
			return this.collection;
		}

		// Token: 0x060068CA RID: 26826 RVA: 0x00180D88 File Offset: 0x0017FD88
		internal object[] GetValues(Array components)
		{
			object[] array = new object[components.Length];
			for (int i = 0; i < components.Length; i++)
			{
				array[i] = this.descriptors[i].GetValue(this.GetPropertyOwnerForComponent(components, i));
			}
			return array;
		}

		// Token: 0x060068CB RID: 26827 RVA: 0x00180DCC File Offset: 0x0017FDCC
		public override void ResetValue(object component)
		{
			Array a = (Array)component;
			for (int i = 0; i < this.descriptors.Length; i++)
			{
				this.descriptors[i].ResetValue(this.GetPropertyOwnerForComponent(a, i));
			}
		}

		// Token: 0x060068CC RID: 26828 RVA: 0x00180E08 File Offset: 0x0017FE08
		private void SetCollectionValues(Array a, IList listValue)
		{
			try
			{
				if (this.collection != null)
				{
					this.collection.Locked = true;
				}
				object[] array = new object[listValue.Count];
				listValue.CopyTo(array, 0);
				for (int i = 0; i < this.descriptors.Length; i++)
				{
					IList list = this.descriptors[i].GetValue(this.GetPropertyOwnerForComponent(a, i)) as IList;
					if (list != null)
					{
						list.Clear();
						foreach (object value in array)
						{
							list.Add(value);
						}
					}
				}
			}
			finally
			{
				if (this.collection != null)
				{
					this.collection.Locked = false;
				}
			}
		}

		// Token: 0x060068CD RID: 26829 RVA: 0x00180EC0 File Offset: 0x0017FEC0
		public override void SetValue(object component, object value)
		{
			Array a = (Array)component;
			if (value is IList && typeof(IList).IsAssignableFrom(this.PropertyType))
			{
				this.SetCollectionValues(a, (IList)value);
				return;
			}
			for (int i = 0; i < this.descriptors.Length; i++)
			{
				object value2 = this.CopyValue(value);
				this.descriptors[i].SetValue(this.GetPropertyOwnerForComponent(a, i), value2);
			}
		}

		// Token: 0x060068CE RID: 26830 RVA: 0x00180F34 File Offset: 0x0017FF34
		public override bool ShouldSerializeValue(object component)
		{
			Array a = (Array)component;
			for (int i = 0; i < this.descriptors.Length; i++)
			{
				if (!this.descriptors[i].ShouldSerializeValue(this.GetPropertyOwnerForComponent(a, i)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04003D9D RID: 15773
		private PropertyDescriptor[] descriptors;

		// Token: 0x04003D9E RID: 15774
		private MergePropertyDescriptor.TriState localizable;

		// Token: 0x04003D9F RID: 15775
		private MergePropertyDescriptor.TriState readOnly;

		// Token: 0x04003DA0 RID: 15776
		private MergePropertyDescriptor.TriState canReset;

		// Token: 0x04003DA1 RID: 15777
		private MergePropertyDescriptor.MultiMergeCollection collection;

		// Token: 0x020007BD RID: 1981
		private enum TriState
		{
			// Token: 0x04003DA3 RID: 15779
			Unknown,
			// Token: 0x04003DA4 RID: 15780
			Yes,
			// Token: 0x04003DA5 RID: 15781
			No
		}

		// Token: 0x020007BE RID: 1982
		private class MultiMergeCollection : ICollection, IEnumerable
		{
			// Token: 0x060068CF RID: 26831 RVA: 0x00180F75 File Offset: 0x0017FF75
			public MultiMergeCollection(ICollection original)
			{
				this.SetItems(original);
			}

			// Token: 0x17001651 RID: 5713
			// (get) Token: 0x060068D0 RID: 26832 RVA: 0x00180F84 File Offset: 0x0017FF84
			public int Count
			{
				get
				{
					if (this.items != null)
					{
						return this.items.Length;
					}
					return 0;
				}
			}

			// Token: 0x17001652 RID: 5714
			// (get) Token: 0x060068D1 RID: 26833 RVA: 0x00180F98 File Offset: 0x0017FF98
			// (set) Token: 0x060068D2 RID: 26834 RVA: 0x00180FA0 File Offset: 0x0017FFA0
			public bool Locked
			{
				get
				{
					return this.locked;
				}
				set
				{
					this.locked = value;
				}
			}

			// Token: 0x17001653 RID: 5715
			// (get) Token: 0x060068D3 RID: 26835 RVA: 0x00180FA9 File Offset: 0x0017FFA9
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x17001654 RID: 5716
			// (get) Token: 0x060068D4 RID: 26836 RVA: 0x00180FAC File Offset: 0x0017FFAC
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060068D5 RID: 26837 RVA: 0x00180FAF File Offset: 0x0017FFAF
			public void CopyTo(Array array, int index)
			{
				if (this.items == null)
				{
					return;
				}
				Array.Copy(this.items, 0, array, index, this.items.Length);
			}

			// Token: 0x060068D6 RID: 26838 RVA: 0x00180FD0 File Offset: 0x0017FFD0
			public IEnumerator GetEnumerator()
			{
				if (this.items != null)
				{
					return this.items.GetEnumerator();
				}
				return new object[0].GetEnumerator();
			}

			// Token: 0x060068D7 RID: 26839 RVA: 0x00180FF4 File Offset: 0x0017FFF4
			public bool MergeCollection(ICollection newCollection)
			{
				if (this.locked)
				{
					return true;
				}
				if (this.items.Length != newCollection.Count)
				{
					this.items = new object[0];
					return false;
				}
				object[] array = new object[newCollection.Count];
				newCollection.CopyTo(array, 0);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == null != (this.items[i] == null) || (this.items[i] != null && !this.items[i].Equals(array[i])))
					{
						this.items = new object[0];
						return false;
					}
				}
				return true;
			}

			// Token: 0x060068D8 RID: 26840 RVA: 0x00181089 File Offset: 0x00180089
			public void SetItems(ICollection collection)
			{
				if (this.locked)
				{
					return;
				}
				this.items = new object[collection.Count];
				collection.CopyTo(this.items, 0);
			}

			// Token: 0x04003DA6 RID: 15782
			private object[] items;

			// Token: 0x04003DA7 RID: 15783
			private bool locked;
		}

		// Token: 0x020007BF RID: 1983
		private class MergedAttributeCollection : AttributeCollection
		{
			// Token: 0x060068D9 RID: 26841 RVA: 0x001810B2 File Offset: 0x001800B2
			public MergedAttributeCollection(MergePropertyDescriptor owner) : base(null)
			{
				this.owner = owner;
			}

			// Token: 0x17001655 RID: 5717
			public override Attribute this[Type attributeType]
			{
				get
				{
					return this.GetCommonAttribute(attributeType);
				}
			}

			// Token: 0x060068DB RID: 26843 RVA: 0x001810CC File Offset: 0x001800CC
			private Attribute GetCommonAttribute(Type attributeType)
			{
				if (this.attributeCollections == null)
				{
					this.attributeCollections = new AttributeCollection[this.owner.descriptors.Length];
					for (int i = 0; i < this.owner.descriptors.Length; i++)
					{
						this.attributeCollections[i] = this.owner.descriptors[i].Attributes;
					}
				}
				if (this.attributeCollections.Length == 0)
				{
					return base.GetDefaultAttribute(attributeType);
				}
				Attribute attribute;
				if (this.foundAttributes != null)
				{
					attribute = (this.foundAttributes[attributeType] as Attribute);
					if (attribute != null)
					{
						return attribute;
					}
				}
				attribute = this.attributeCollections[0][attributeType];
				if (attribute == null)
				{
					return null;
				}
				for (int j = 1; j < this.attributeCollections.Length; j++)
				{
					Attribute obj = this.attributeCollections[j][attributeType];
					if (!attribute.Equals(obj))
					{
						attribute = base.GetDefaultAttribute(attributeType);
						break;
					}
				}
				if (this.foundAttributes == null)
				{
					this.foundAttributes = new Hashtable();
				}
				this.foundAttributes[attributeType] = attribute;
				return attribute;
			}

			// Token: 0x04003DA8 RID: 15784
			private MergePropertyDescriptor owner;

			// Token: 0x04003DA9 RID: 15785
			private AttributeCollection[] attributeCollections;

			// Token: 0x04003DAA RID: 15786
			private IDictionary foundAttributes;
		}
	}
}

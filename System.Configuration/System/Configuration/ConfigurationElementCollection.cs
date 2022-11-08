using System;
using System.Collections;
using System.Diagnostics;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000025 RID: 37
	[DebuggerDisplay("Count = {Count}")]
	public abstract class ConfigurationElementCollection : ConfigurationElement, ICollection, IEnumerable
	{
		// Token: 0x060001E0 RID: 480 RVA: 0x0000CB9F File Offset: 0x0000BB9F
		protected ConfigurationElementCollection()
		{
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000CBE0 File Offset: 0x0000BBE0
		protected ConfigurationElementCollection(IComparer comparer)
		{
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}
			this._comparer = comparer;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000CC3F File Offset: 0x0000BC3F
		private ArrayList Items
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000CC47 File Offset: 0x0000BC47
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x0000CC50 File Offset: 0x0000BC50
		protected internal string AddElementName
		{
			get
			{
				return this._addElement;
			}
			set
			{
				this._addElement = value;
				if (BaseConfigurationRecord.IsReservedAttributeName(value))
				{
					throw new ArgumentException(SR.GetString("Item_name_reserved", new object[]
					{
						"add",
						value
					}));
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000CC90 File Offset: 0x0000BC90
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x0000CC98 File Offset: 0x0000BC98
		protected internal string RemoveElementName
		{
			get
			{
				return this._removeElement;
			}
			set
			{
				if (BaseConfigurationRecord.IsReservedAttributeName(value))
				{
					throw new ArgumentException(SR.GetString("Item_name_reserved", new object[]
					{
						"remove",
						value
					}));
				}
				this._removeElement = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000CCD8 File Offset: 0x0000BCD8
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x0000CCE0 File Offset: 0x0000BCE0
		protected internal string ClearElementName
		{
			get
			{
				return this._clearElement;
			}
			set
			{
				if (BaseConfigurationRecord.IsReservedAttributeName(value))
				{
					throw new ArgumentException(SR.GetString("Item_name_reserved", new object[]
					{
						"clear",
						value
					}));
				}
				this._clearElement = value;
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000CD20 File Offset: 0x0000BD20
		internal override void AssociateContext(BaseConfigurationRecord configRecord)
		{
			base.AssociateContext(configRecord);
			foreach (object obj in this._items)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
				if (entry._value != null)
				{
					entry._value.AssociateContext(configRecord);
				}
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000CD90 File Offset: 0x0000BD90
		protected internal override bool IsModified()
		{
			if (this.bModified)
			{
				return true;
			}
			if (base.IsModified())
			{
				return true;
			}
			foreach (object obj in this._items)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
				if (entry._entryType != ConfigurationElementCollection.EntryType.Removed)
				{
					ConfigurationElement value = entry._value;
					if (value.IsModified())
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000CE18 File Offset: 0x0000BE18
		protected internal override void ResetModified()
		{
			this.bModified = false;
			base.ResetModified();
			foreach (object obj in this._items)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
				if (entry._entryType != ConfigurationElementCollection.EntryType.Removed)
				{
					ConfigurationElement value = entry._value;
					value.ResetModified();
				}
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000CE90 File Offset: 0x0000BE90
		public override bool IsReadOnly()
		{
			return this.bReadOnly;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000CE98 File Offset: 0x0000BE98
		protected internal override void SetReadOnly()
		{
			this.bReadOnly = true;
			foreach (object obj in this._items)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
				if (entry._entryType != ConfigurationElementCollection.EntryType.Removed)
				{
					ConfigurationElement value = entry._value;
					value.SetReadOnly();
				}
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000CF08 File Offset: 0x0000BF08
		internal virtual IEnumerator GetEnumeratorImpl()
		{
			return new ConfigurationElementCollection.Enumerator(this._items, this);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000CF16 File Offset: 0x0000BF16
		internal IEnumerator GetElementsEnumerator()
		{
			return new ConfigurationElementCollection.Enumerator(this._items, this);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000CF24 File Offset: 0x0000BF24
		public override bool Equals(object compareTo)
		{
			if (compareTo.GetType() != base.GetType())
			{
				return false;
			}
			ConfigurationElementCollection configurationElementCollection = (ConfigurationElementCollection)compareTo;
			if (this.Count != configurationElementCollection.Count)
			{
				return false;
			}
			foreach (object obj in this.Items)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
				bool flag = false;
				foreach (object obj2 in configurationElementCollection.Items)
				{
					ConfigurationElementCollection.Entry entry2 = (ConfigurationElementCollection.Entry)obj2;
					if (object.Equals(entry._value, entry2._value))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000D014 File Offset: 0x0000C014
		public override int GetHashCode()
		{
			int num = 0;
			foreach (object obj in this.Items)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
				ConfigurationElement value = entry._value;
				num ^= value.GetHashCode();
			}
			return num;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000D07C File Offset: 0x0000C07C
		protected internal override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
			if (sourceElement != null)
			{
				ConfigurationElementCollection configurationElementCollection = parentElement as ConfigurationElementCollection;
				ConfigurationElementCollection configurationElementCollection2 = sourceElement as ConfigurationElementCollection;
				Hashtable hashtable = new Hashtable();
				this._lockedAllExceptAttributesList = sourceElement._lockedAllExceptAttributesList;
				this._lockedAllExceptElementsList = sourceElement._lockedAllExceptElementsList;
				this._fItemLocked = sourceElement._fItemLocked;
				this._lockedAttributesList = sourceElement._lockedAttributesList;
				this._lockedElementsList = sourceElement._lockedElementsList;
				this.AssociateContext(sourceElement._configRecord);
				if (parentElement != null)
				{
					if (parentElement._lockedAttributesList != null)
					{
						this._lockedAttributesList = base.UnMergeLockList(sourceElement._lockedAttributesList, parentElement._lockedAttributesList, saveMode);
					}
					if (parentElement._lockedElementsList != null)
					{
						this._lockedElementsList = base.UnMergeLockList(sourceElement._lockedElementsList, parentElement._lockedElementsList, saveMode);
					}
					if (parentElement._lockedAllExceptAttributesList != null)
					{
						this._lockedAllExceptAttributesList = base.UnMergeLockList(sourceElement._lockedAllExceptAttributesList, parentElement._lockedAllExceptAttributesList, saveMode);
					}
					if (parentElement._lockedAllExceptElementsList != null)
					{
						this._lockedAllExceptElementsList = base.UnMergeLockList(sourceElement._lockedAllExceptElementsList, parentElement._lockedAllExceptElementsList, saveMode);
					}
				}
				if (this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMap || this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate)
				{
					this.bCollectionCleared = configurationElementCollection2.bCollectionCleared;
					this.EmitClear = ((saveMode == ConfigurationSaveMode.Full && this._clearElement.Length != 0) || (saveMode == ConfigurationSaveMode.Modified && this.bCollectionCleared) || configurationElementCollection2.EmitClear);
					if (configurationElementCollection != null && !this.EmitClear)
					{
						foreach (object obj in configurationElementCollection.Items)
						{
							ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
							if (entry._entryType != ConfigurationElementCollection.EntryType.Removed)
							{
								hashtable[entry.GetKey(this)] = ConfigurationElementCollection.InheritedType.inParent;
							}
						}
					}
					foreach (object obj2 in configurationElementCollection2.Items)
					{
						ConfigurationElementCollection.Entry entry2 = (ConfigurationElementCollection.Entry)obj2;
						if (entry2._entryType != ConfigurationElementCollection.EntryType.Removed)
						{
							if (hashtable.Contains(entry2.GetKey(this)))
							{
								ConfigurationElementCollection.Entry entry3 = (ConfigurationElementCollection.Entry)configurationElementCollection.Items[configurationElementCollection.RealIndexOf(entry2._value)];
								ConfigurationElement value = entry2._value;
								if (value.Equals(entry3._value))
								{
									hashtable[entry2.GetKey(this)] = ConfigurationElementCollection.InheritedType.inBothSame;
									if (saveMode == ConfigurationSaveMode.Modified)
									{
										if (value.IsModified())
										{
											hashtable[entry2.GetKey(this)] = ConfigurationElementCollection.InheritedType.inBothDiff;
										}
										else if (value.ElementPresent)
										{
											hashtable[entry2.GetKey(this)] = ConfigurationElementCollection.InheritedType.inBothCopyNoRemove;
										}
									}
								}
								else
								{
									hashtable[entry2.GetKey(this)] = ConfigurationElementCollection.InheritedType.inBothDiff;
									if (this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate && entry2._entryType == ConfigurationElementCollection.EntryType.Added)
									{
										hashtable[entry2.GetKey(this)] = ConfigurationElementCollection.InheritedType.inBothCopyNoRemove;
									}
								}
							}
							else
							{
								hashtable[entry2.GetKey(this)] = ConfigurationElementCollection.InheritedType.inSelf;
							}
						}
					}
					if (configurationElementCollection != null && !this.EmitClear)
					{
						foreach (object obj3 in configurationElementCollection.Items)
						{
							ConfigurationElementCollection.Entry entry4 = (ConfigurationElementCollection.Entry)obj3;
							if (entry4._entryType != ConfigurationElementCollection.EntryType.Removed)
							{
								ConfigurationElementCollection.InheritedType inheritedType = (ConfigurationElementCollection.InheritedType)hashtable[entry4.GetKey(this)];
								if (inheritedType == ConfigurationElementCollection.InheritedType.inParent || inheritedType == ConfigurationElementCollection.InheritedType.inBothDiff)
								{
									ConfigurationElement configurationElement = this.CallCreateNewElement(entry4.GetKey(this).ToString());
									configurationElement.Reset(entry4._value);
									this.BaseAdd(configurationElement, this.ThrowOnDuplicate, true);
									this.BaseRemove(entry4.GetKey(this), false);
								}
							}
						}
					}
					using (IEnumerator enumerator4 = configurationElementCollection2.Items.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							object obj4 = enumerator4.Current;
							ConfigurationElementCollection.Entry entry5 = (ConfigurationElementCollection.Entry)obj4;
							if (entry5._entryType != ConfigurationElementCollection.EntryType.Removed)
							{
								ConfigurationElementCollection.InheritedType inheritedType2 = (ConfigurationElementCollection.InheritedType)hashtable[entry5.GetKey(this)];
								if (inheritedType2 == ConfigurationElementCollection.InheritedType.inSelf || inheritedType2 == ConfigurationElementCollection.InheritedType.inBothDiff || inheritedType2 == ConfigurationElementCollection.InheritedType.inBothCopyNoRemove)
								{
									ConfigurationElement configurationElement2 = this.CallCreateNewElement(entry5.GetKey(this).ToString());
									configurationElement2.Unmerge(entry5._value, null, saveMode);
									if (inheritedType2 == ConfigurationElementCollection.InheritedType.inSelf)
									{
										configurationElement2.RemoveAllInheritedLocks();
									}
									this.BaseAdd(configurationElement2, this.ThrowOnDuplicate, true);
								}
							}
						}
						return;
					}
				}
				if (this.CollectionType == ConfigurationElementCollectionType.BasicMap || this.CollectionType == ConfigurationElementCollectionType.BasicMapAlternate)
				{
					foreach (object obj5 in configurationElementCollection2.Items)
					{
						ConfigurationElementCollection.Entry entry6 = (ConfigurationElementCollection.Entry)obj5;
						bool flag = false;
						ConfigurationElementCollection.Entry entry7 = null;
						if (entry6._entryType == ConfigurationElementCollection.EntryType.Added || entry6._entryType == ConfigurationElementCollection.EntryType.Replaced)
						{
							bool flag2 = false;
							if (configurationElementCollection != null)
							{
								foreach (object obj6 in configurationElementCollection.Items)
								{
									ConfigurationElementCollection.Entry entry8 = (ConfigurationElementCollection.Entry)obj6;
									if (object.Equals(entry6.GetKey(this), entry8.GetKey(this)) && !this.IsElementName(entry6.GetKey(this).ToString()))
									{
										flag = true;
										entry7 = entry8;
									}
									if (object.Equals(entry6._value, entry8._value))
									{
										flag = true;
										flag2 = true;
										entry7 = entry8;
										break;
									}
								}
							}
							ConfigurationElement configurationElement3 = this.CallCreateNewElement(entry6.GetKey(this).ToString());
							if (!flag)
							{
								configurationElement3.Unmerge(entry6._value, null, saveMode);
								this.BaseAdd(-1, configurationElement3, true);
							}
							else
							{
								ConfigurationElement value2 = entry6._value;
								if (!flag2 || (saveMode == ConfigurationSaveMode.Modified && value2.IsModified()) || saveMode == ConfigurationSaveMode.Full)
								{
									configurationElement3.Unmerge(entry6._value, entry7._value, saveMode);
									this.BaseAdd(-1, configurationElement3, true);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000D6F4 File Offset: 0x0000C6F4
		protected internal override void Reset(ConfigurationElement parentElement)
		{
			ConfigurationElementCollection configurationElementCollection = parentElement as ConfigurationElementCollection;
			base.ResetLockLists(parentElement);
			if (configurationElementCollection != null)
			{
				foreach (object obj in configurationElementCollection.Items)
				{
					ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
					ConfigurationElement configurationElement = this.CallCreateNewElement(entry.GetKey(this).ToString());
					configurationElement.Reset(entry._value);
					if ((this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMap || this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate) && (entry._entryType == ConfigurationElementCollection.EntryType.Added || entry._entryType == ConfigurationElementCollection.EntryType.Replaced))
					{
						this.BaseAdd(configurationElement, true, true);
					}
					else if (this.CollectionType == ConfigurationElementCollectionType.BasicMap || this.CollectionType == ConfigurationElementCollectionType.BasicMapAlternate)
					{
						this.BaseAdd(-1, configurationElement, true);
					}
				}
				this._inheritedCount = this.Count;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000D7D4 File Offset: 0x0000C7D4
		public int Count
		{
			get
			{
				return this._items.Count - this._removedItemCount;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000D7E8 File Offset: 0x0000C7E8
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x0000D7F0 File Offset: 0x0000C7F0
		public bool EmitClear
		{
			get
			{
				return this.bEmitClearTag;
			}
			set
			{
				if (this.IsReadOnly())
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_base_read_only"));
				}
				if (value)
				{
					base.CheckLockedElement(this._clearElement, null);
					base.CheckLockedElement(this._removeElement, null);
				}
				this.bModified = true;
				this.bEmitClearTag = value;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000D840 File Offset: 0x0000C840
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000D843 File Offset: 0x0000C843
		public object SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000D846 File Offset: 0x0000C846
		public void CopyTo(ConfigurationElement[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000D850 File Offset: 0x0000C850
		void ICollection.CopyTo(Array arr, int index)
		{
			foreach (object obj in this._items)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
				if (entry._entryType != ConfigurationElementCollection.EntryType.Removed)
				{
					arr.SetValue(entry._value, index++);
				}
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000D8C0 File Offset: 0x0000C8C0
		public IEnumerator GetEnumerator()
		{
			return this.GetEnumeratorImpl();
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000D8C8 File Offset: 0x0000C8C8
		protected virtual void BaseAdd(ConfigurationElement element)
		{
			this.BaseAdd(element, this.ThrowOnDuplicate);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000D8D7 File Offset: 0x0000C8D7
		protected internal void BaseAdd(ConfigurationElement element, bool throwIfExists)
		{
			this.BaseAdd(element, throwIfExists, false);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000D8E4 File Offset: 0x0000C8E4
		private void BaseAdd(ConfigurationElement element, bool throwIfExists, bool ignoreLocks)
		{
			bool flagAsReplaced = false;
			bool flag = this.internalAddToEnd;
			if (this.IsReadOnly())
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_read_only"));
			}
			if (base.LockItem && !ignoreLocks)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_element_locked", new object[]
				{
					this._addElement
				}));
			}
			object elementKeyInternal = this.GetElementKeyInternal(element);
			int num = -1;
			int i = 0;
			while (i < this._items.Count)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)this._items[i];
				if (this.CompareKeys(elementKeyInternal, entry.GetKey(this)))
				{
					if (entry._value != null && entry._value.LockItem && !ignoreLocks)
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_base_collection_item_locked"));
					}
					if (entry._entryType != ConfigurationElementCollection.EntryType.Removed && throwIfExists)
					{
						if (!element.Equals(entry._value))
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_base_collection_entry_already_exists", new object[]
							{
								elementKeyInternal
							}), element.PropertyFileName(""), element.PropertyLineNumber(""));
						}
						entry._value = element;
						return;
					}
					else
					{
						if (entry._entryType != ConfigurationElementCollection.EntryType.Added)
						{
							if ((this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMap || this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate) && entry._entryType == ConfigurationElementCollection.EntryType.Removed && this._removedItemCount > 0)
							{
								this._removedItemCount--;
							}
							entry._entryType = ConfigurationElementCollection.EntryType.Replaced;
							flagAsReplaced = true;
						}
						if (!flag && this.CollectionType != ConfigurationElementCollectionType.AddRemoveClearMapAlternate)
						{
							if (!ignoreLocks)
							{
								element.HandleLockedAttributes(entry._value);
								element.MergeLocks(entry._value);
							}
							entry._value = element;
							this.bModified = true;
							return;
						}
						num = i;
						if (entry._entryType == ConfigurationElementCollection.EntryType.Added)
						{
							flag = true;
							break;
						}
						break;
					}
				}
				else
				{
					i++;
				}
			}
			if (num >= 0)
			{
				this._items.RemoveAt(num);
				if (this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate && num > this.Count + this._removedItemCount - this._inheritedCount)
				{
					this._inheritedCount--;
				}
			}
			this.BaseAddInternal(flag ? -1 : num, element, flagAsReplaced, ignoreLocks);
			this.bModified = true;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000DB00 File Offset: 0x0000CB00
		protected int BaseIndexOf(ConfigurationElement element)
		{
			int num = 0;
			object elementKeyInternal = this.GetElementKeyInternal(element);
			foreach (object obj in this._items)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
				if (entry._entryType != ConfigurationElementCollection.EntryType.Removed)
				{
					if (this.CompareKeys(elementKeyInternal, entry.GetKey(this)))
					{
						return num;
					}
					num++;
				}
			}
			return -1;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000DB88 File Offset: 0x0000CB88
		internal int RealIndexOf(ConfigurationElement element)
		{
			int num = 0;
			object elementKeyInternal = this.GetElementKeyInternal(element);
			foreach (object obj in this._items)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
				if (this.CompareKeys(elementKeyInternal, entry.GetKey(this)))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000DC08 File Offset: 0x0000CC08
		private void BaseAddInternal(int index, ConfigurationElement element, bool flagAsReplaced, bool ignoreLocks)
		{
			element.AssociateContext(this._configRecord);
			if (element != null)
			{
				element.CallInit();
			}
			if (this.IsReadOnly())
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_read_only"));
			}
			if (!ignoreLocks)
			{
				if (this.CollectionType == ConfigurationElementCollectionType.BasicMap || this.CollectionType == ConfigurationElementCollectionType.BasicMapAlternate)
				{
					if (BaseConfigurationRecord.IsReservedAttributeName(this.ElementName))
					{
						throw new ArgumentException(SR.GetString("Basicmap_item_name_reserved", new object[]
						{
							this.ElementName
						}));
					}
					base.CheckLockedElement(this.ElementName, null);
				}
				if (this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMap || this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate)
				{
					base.CheckLockedElement(this._addElement, null);
				}
			}
			if (this.CollectionType == ConfigurationElementCollectionType.BasicMapAlternate || this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate)
			{
				if (index == -1)
				{
					index = this.Count + this._removedItemCount - this._inheritedCount;
				}
				else if (index > this.Count + this._removedItemCount - this._inheritedCount && !flagAsReplaced)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_base_cannot_add_items_below_inherited_items"));
				}
			}
			if (this.CollectionType == ConfigurationElementCollectionType.BasicMap && index >= 0 && index < this._inheritedCount)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_cannot_add_items_above_inherited_items"));
			}
			ConfigurationElementCollection.EntryType type = (!flagAsReplaced) ? ConfigurationElementCollection.EntryType.Added : ConfigurationElementCollection.EntryType.Replaced;
			object elementKeyInternal = this.GetElementKeyInternal(element);
			if (index >= 0)
			{
				if (index > this._items.Count)
				{
					throw new ConfigurationErrorsException(SR.GetString("IndexOutOfRange", new object[]
					{
						index
					}));
				}
				this._items.Insert(index, new ConfigurationElementCollection.Entry(type, elementKeyInternal, element));
			}
			else
			{
				this._items.Add(new ConfigurationElementCollection.Entry(type, elementKeyInternal, element));
			}
			this.bModified = true;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000DDA2 File Offset: 0x0000CDA2
		protected virtual void BaseAdd(int index, ConfigurationElement element)
		{
			this.BaseAdd(index, element, false);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000DDB0 File Offset: 0x0000CDB0
		private void BaseAdd(int index, ConfigurationElement element, bool ignoreLocks)
		{
			if (this.IsReadOnly())
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_read_only"));
			}
			if (index < -1)
			{
				throw new ConfigurationErrorsException(SR.GetString("IndexOutOfRange", new object[]
				{
					index
				}));
			}
			if (index != -1 && (this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMap || this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate))
			{
				int num = 0;
				if (index > 0)
				{
					foreach (object obj in this._items)
					{
						ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
						if (entry._entryType != ConfigurationElementCollection.EntryType.Removed)
						{
							index--;
						}
						if (index == 0)
						{
							break;
						}
						num++;
					}
					num = (index = num + 1);
				}
				object elementKeyInternal = this.GetElementKeyInternal(element);
				foreach (object obj2 in this._items)
				{
					ConfigurationElementCollection.Entry entry2 = (ConfigurationElementCollection.Entry)obj2;
					if (this.CompareKeys(elementKeyInternal, entry2.GetKey(this)) && entry2._entryType != ConfigurationElementCollection.EntryType.Removed)
					{
						if (!element.Equals(entry2._value))
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_base_collection_entry_already_exists", new object[]
							{
								elementKeyInternal
							}), element.PropertyFileName(""), element.PropertyLineNumber(""));
						}
						return;
					}
				}
			}
			this.BaseAddInternal(index, element, false, ignoreLocks);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000DF40 File Offset: 0x0000CF40
		protected internal void BaseRemove(object key)
		{
			this.BaseRemove(key, false);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000DF4C File Offset: 0x0000CF4C
		private void BaseRemove(object key, bool throwIfMissing)
		{
			if (this.IsReadOnly())
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_read_only"));
			}
			int num = 0;
			bool flag = false;
			foreach (object obj in this._items)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
				if (this.CompareKeys(key, entry.GetKey(this)))
				{
					flag = true;
					if (entry._value == null)
					{
						if (throwIfMissing)
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_base_collection_entry_not_found", new object[]
							{
								key
							}));
						}
						return;
					}
					else
					{
						if (entry._value.LockItem)
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_base_attribute_locked", new object[]
							{
								key
							}));
						}
						if (!entry._value.ElementPresent)
						{
							base.CheckLockedElement(this._removeElement, null);
						}
						switch (entry._entryType)
						{
						case ConfigurationElementCollection.EntryType.Removed:
							if (throwIfMissing)
							{
								throw new ConfigurationErrorsException(SR.GetString("Config_base_collection_entry_already_removed"));
							}
							break;
						case ConfigurationElementCollection.EntryType.Added:
							if (this.CollectionType != ConfigurationElementCollectionType.AddRemoveClearMap && this.CollectionType != ConfigurationElementCollectionType.AddRemoveClearMapAlternate)
							{
								if (this.CollectionType == ConfigurationElementCollectionType.BasicMapAlternate && num >= this.Count - this._inheritedCount)
								{
									throw new ConfigurationErrorsException(SR.GetString("Config_base_cannot_remove_inherited_items"));
								}
								if (this.CollectionType == ConfigurationElementCollectionType.BasicMap && num < this._inheritedCount)
								{
									throw new ConfigurationErrorsException(SR.GetString("Config_base_cannot_remove_inherited_items"));
								}
								this._items.RemoveAt(num);
							}
							else
							{
								entry._entryType = ConfigurationElementCollection.EntryType.Removed;
								this._removedItemCount++;
							}
							break;
						default:
							if (this.CollectionType != ConfigurationElementCollectionType.AddRemoveClearMap && this.CollectionType != ConfigurationElementCollectionType.AddRemoveClearMapAlternate)
							{
								throw new ConfigurationErrorsException(SR.GetString("Config_base_collection_elements_may_not_be_removed"));
							}
							entry._entryType = ConfigurationElementCollection.EntryType.Removed;
							this._removedItemCount++;
							break;
						}
						this.bModified = true;
						return;
					}
				}
				else
				{
					num++;
				}
			}
			if (!flag)
			{
				if (throwIfMissing)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_base_collection_entry_not_found", new object[]
					{
						key
					}));
				}
				if (this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMap || this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate)
				{
					if (this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate)
					{
						this._items.Insert(this.Count + this._removedItemCount - this._inheritedCount, new ConfigurationElementCollection.Entry(ConfigurationElementCollection.EntryType.Removed, key, null));
					}
					else
					{
						this._items.Add(new ConfigurationElementCollection.Entry(ConfigurationElementCollection.EntryType.Removed, key, null));
					}
					this._removedItemCount++;
				}
			}
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000E1E0 File Offset: 0x0000D1E0
		protected internal ConfigurationElement BaseGet(object key)
		{
			foreach (object obj in this._items)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
				if (entry._entryType != ConfigurationElementCollection.EntryType.Removed && this.CompareKeys(key, entry.GetKey(this)))
				{
					return entry._value;
				}
			}
			return null;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000E258 File Offset: 0x0000D258
		protected internal bool BaseIsRemoved(object key)
		{
			foreach (object obj in this._items)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
				if (this.CompareKeys(key, entry.GetKey(this)))
				{
					if (entry._entryType == ConfigurationElementCollection.EntryType.Removed)
					{
						return true;
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000E2D0 File Offset: 0x0000D2D0
		protected internal ConfigurationElement BaseGet(int index)
		{
			if (index < 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("IndexOutOfRange", new object[]
				{
					index
				}));
			}
			int num = 0;
			ConfigurationElementCollection.Entry entry = null;
			foreach (object obj in this._items)
			{
				ConfigurationElementCollection.Entry entry2 = (ConfigurationElementCollection.Entry)obj;
				if (num == index && entry2._entryType != ConfigurationElementCollection.EntryType.Removed)
				{
					entry = entry2;
					break;
				}
				if (entry2._entryType != ConfigurationElementCollection.EntryType.Removed)
				{
					num++;
				}
			}
			if (entry != null)
			{
				return entry._value;
			}
			throw new ConfigurationErrorsException(SR.GetString("IndexOutOfRange", new object[]
			{
				index
			}));
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000E3A0 File Offset: 0x0000D3A0
		protected internal object[] BaseGetAllKeys()
		{
			object[] array = new object[this.Count];
			int num = 0;
			foreach (object obj in this._items)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
				if (entry._entryType != ConfigurationElementCollection.EntryType.Removed)
				{
					array[num] = entry.GetKey(this);
					num++;
				}
			}
			return array;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000E41C File Offset: 0x0000D41C
		protected internal object BaseGetKey(int index)
		{
			int num = 0;
			ConfigurationElementCollection.Entry entry = null;
			if (index < 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("IndexOutOfRange", new object[]
				{
					index
				}));
			}
			foreach (object obj in this._items)
			{
				ConfigurationElementCollection.Entry entry2 = (ConfigurationElementCollection.Entry)obj;
				if (num == index && entry2._entryType != ConfigurationElementCollection.EntryType.Removed)
				{
					entry = entry2;
					break;
				}
				if (entry2._entryType != ConfigurationElementCollection.EntryType.Removed)
				{
					num++;
				}
			}
			if (entry != null)
			{
				return entry.GetKey(this);
			}
			throw new ConfigurationErrorsException(SR.GetString("IndexOutOfRange", new object[]
			{
				index
			}));
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000E4F0 File Offset: 0x0000D4F0
		protected internal void BaseClear()
		{
			if (this.IsReadOnly())
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_read_only"));
			}
			base.CheckLockedElement(this._clearElement, null);
			base.CheckLockedElement(this._removeElement, null);
			this.bModified = true;
			this.bCollectionCleared = true;
			if ((this.CollectionType == ConfigurationElementCollectionType.BasicMap || this.CollectionType == ConfigurationElementCollectionType.BasicMapAlternate) && this._inheritedCount > 0)
			{
				int index = 0;
				if (this.CollectionType == ConfigurationElementCollectionType.BasicMapAlternate)
				{
					index = 0;
				}
				if (this.CollectionType == ConfigurationElementCollectionType.BasicMap)
				{
					index = this._inheritedCount;
				}
				while (this.Count - this._inheritedCount > 0)
				{
					this._items.RemoveAt(index);
				}
				return;
			}
			int num = 0;
			int num2 = 0;
			int count = this.Count;
			for (int i = 0; i < this._items.Count; i++)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)this._items[i];
				if (entry._value != null && entry._value.LockItem)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_base_collection_item_locked_cannot_clear"));
				}
			}
			for (int j = this._items.Count - 1; j >= 0; j--)
			{
				ConfigurationElementCollection.Entry entry2 = (ConfigurationElementCollection.Entry)this._items[j];
				if ((this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMap && j < this._inheritedCount) || (this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate && j >= count - this._inheritedCount))
				{
					num++;
				}
				if (entry2._entryType == ConfigurationElementCollection.EntryType.Removed)
				{
					num2++;
				}
				this._items.RemoveAt(j);
			}
			this._inheritedCount -= num;
			this._removedItemCount -= num2;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000E688 File Offset: 0x0000D688
		protected internal void BaseRemoveAt(int index)
		{
			if (this.IsReadOnly())
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_read_only"));
			}
			int num = 0;
			ConfigurationElementCollection.Entry entry = null;
			foreach (object obj in this._items)
			{
				ConfigurationElementCollection.Entry entry2 = (ConfigurationElementCollection.Entry)obj;
				if (num == index && entry2._entryType != ConfigurationElementCollection.EntryType.Removed)
				{
					entry = entry2;
					break;
				}
				if (entry2._entryType != ConfigurationElementCollection.EntryType.Removed)
				{
					num++;
				}
			}
			if (entry == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("IndexOutOfRange", new object[]
				{
					index
				}));
			}
			if (entry._value.LockItem)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_attribute_locked", new object[]
				{
					entry.GetKey(this)
				}));
			}
			if (!entry._value.ElementPresent)
			{
				base.CheckLockedElement(this._removeElement, null);
			}
			switch (entry._entryType)
			{
			case ConfigurationElementCollection.EntryType.Removed:
				throw new ConfigurationErrorsException(SR.GetString("Config_base_collection_entry_already_removed"));
			case ConfigurationElementCollection.EntryType.Added:
				if (this.CollectionType != ConfigurationElementCollectionType.AddRemoveClearMap && this.CollectionType != ConfigurationElementCollectionType.AddRemoveClearMapAlternate)
				{
					if (this.CollectionType == ConfigurationElementCollectionType.BasicMapAlternate && index >= this.Count - this._inheritedCount)
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_base_cannot_remove_inherited_items"));
					}
					if (this.CollectionType == ConfigurationElementCollectionType.BasicMap && index < this._inheritedCount)
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_base_cannot_remove_inherited_items"));
					}
					this._items.RemoveAt(index);
				}
				else
				{
					if (!entry._value.ElementPresent)
					{
						base.CheckLockedElement(this._removeElement, null);
					}
					entry._entryType = ConfigurationElementCollection.EntryType.Removed;
					this._removedItemCount++;
				}
				break;
			default:
				if (this.CollectionType != ConfigurationElementCollectionType.AddRemoveClearMap && this.CollectionType != ConfigurationElementCollectionType.AddRemoveClearMapAlternate)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_base_collection_elements_may_not_be_removed"));
				}
				entry._entryType = ConfigurationElementCollection.EntryType.Removed;
				this._removedItemCount++;
				break;
			}
			this.bModified = true;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000E894 File Offset: 0x0000D894
		protected internal override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
		{
			ConfigurationElementCollectionType collectionType = this.CollectionType;
			bool flag = false;
			flag |= base.SerializeElement(writer, serializeCollectionKey);
			if ((collectionType == ConfigurationElementCollectionType.AddRemoveClearMap || collectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate) && this.bEmitClearTag && this._clearElement.Length != 0)
			{
				if (writer != null)
				{
					writer.WriteStartElement(this._clearElement);
					writer.WriteEndElement();
				}
				flag = true;
			}
			foreach (object obj in this._items)
			{
				ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)obj;
				if (collectionType == ConfigurationElementCollectionType.BasicMap || collectionType == ConfigurationElementCollectionType.BasicMapAlternate)
				{
					if (entry._entryType == ConfigurationElementCollection.EntryType.Added || entry._entryType == ConfigurationElementCollection.EntryType.Replaced)
					{
						if (this.ElementName != null && this.ElementName.Length != 0)
						{
							if (BaseConfigurationRecord.IsReservedAttributeName(this.ElementName))
							{
								throw new ArgumentException(SR.GetString("Basicmap_item_name_reserved", new object[]
								{
									this.ElementName
								}));
							}
							flag |= entry._value.SerializeToXmlElement(writer, this.ElementName);
						}
						else
						{
							flag |= entry._value.SerializeElement(writer, false);
						}
					}
				}
				else if (collectionType == ConfigurationElementCollectionType.AddRemoveClearMap || collectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate)
				{
					if ((entry._entryType == ConfigurationElementCollection.EntryType.Removed || entry._entryType == ConfigurationElementCollection.EntryType.Replaced) && entry._value != null)
					{
						if (writer != null)
						{
							writer.WriteStartElement(this._removeElement);
						}
						flag |= entry._value.SerializeElement(writer, true);
						if (writer != null)
						{
							writer.WriteEndElement();
						}
						flag = true;
					}
					if (entry._entryType == ConfigurationElementCollection.EntryType.Added || entry._entryType == ConfigurationElementCollection.EntryType.Replaced)
					{
						flag |= entry._value.SerializeToXmlElement(writer, this._addElement);
					}
				}
			}
			return flag;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000EA48 File Offset: 0x0000DA48
		protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
		{
			bool result = false;
			if (this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMap || this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate)
			{
				if (elementName == this._addElement)
				{
					ConfigurationElement configurationElement = this.CallCreateNewElement();
					configurationElement.ResetLockLists(this);
					configurationElement.DeserializeElement(reader, false);
					this.BaseAdd(configurationElement);
					result = true;
				}
				else if (elementName == this._removeElement)
				{
					ConfigurationElement configurationElement2 = this.CallCreateNewElement();
					configurationElement2.ResetLockLists(this);
					configurationElement2.DeserializeElement(reader, true);
					if (this.IsElementRemovable(configurationElement2))
					{
						this.BaseRemove(this.GetElementKeyInternal(configurationElement2), false);
					}
					result = true;
				}
				else if (elementName == this._clearElement)
				{
					if (reader.AttributeCount > 0 && reader.MoveToNextAttribute())
					{
						string name = reader.Name;
						throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_attribute", new object[]
						{
							name
						}), reader);
					}
					base.CheckLockedElement(elementName, reader);
					reader.MoveToElement();
					this.BaseClear();
					this.bEmitClearTag = true;
					result = true;
				}
			}
			else if (elementName == this.ElementName)
			{
				if (BaseConfigurationRecord.IsReservedAttributeName(elementName))
				{
					throw new ArgumentException(SR.GetString("Basicmap_item_name_reserved", new object[]
					{
						elementName
					}));
				}
				ConfigurationElement configurationElement3 = this.CallCreateNewElement();
				configurationElement3.ResetLockLists(this);
				configurationElement3.DeserializeElement(reader, false);
				this.BaseAdd(configurationElement3);
				result = true;
			}
			else if (this.IsElementName(elementName))
			{
				if (BaseConfigurationRecord.IsReservedAttributeName(elementName))
				{
					throw new ArgumentException(SR.GetString("Basicmap_item_name_reserved", new object[]
					{
						elementName
					}));
				}
				ConfigurationElement configurationElement4 = this.CallCreateNewElement(elementName);
				configurationElement4.ResetLockLists(this);
				configurationElement4.DeserializeElement(reader, false);
				this.BaseAdd(-1, configurationElement4);
				result = true;
			}
			return result;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000EC00 File Offset: 0x0000DC00
		private ConfigurationElement CallCreateNewElement(string elementName)
		{
			ConfigurationElement configurationElement = this.CreateNewElement(elementName);
			configurationElement.AssociateContext(this._configRecord);
			configurationElement.CallInit();
			return configurationElement;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000EC28 File Offset: 0x0000DC28
		private ConfigurationElement CallCreateNewElement()
		{
			ConfigurationElement configurationElement = this.CreateNewElement();
			configurationElement.AssociateContext(this._configRecord);
			configurationElement.CallInit();
			return configurationElement;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000EC4F File Offset: 0x0000DC4F
		protected virtual ConfigurationElement CreateNewElement(string elementName)
		{
			return this.CreateNewElement();
		}

		// Token: 0x06000212 RID: 530
		protected abstract ConfigurationElement CreateNewElement();

		// Token: 0x06000213 RID: 531
		protected abstract object GetElementKey(ConfigurationElement element);

		// Token: 0x06000214 RID: 532 RVA: 0x0000EC58 File Offset: 0x0000DC58
		internal object GetElementKeyInternal(ConfigurationElement element)
		{
			object elementKey = this.GetElementKey(element);
			if (elementKey == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_invalid_element_key"));
			}
			return elementKey;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000EC81 File Offset: 0x0000DC81
		protected virtual bool IsElementRemovable(ConfigurationElement element)
		{
			return true;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000EC84 File Offset: 0x0000DC84
		private bool CompareKeys(object key1, object key2)
		{
			if (this._comparer != null)
			{
				return this._comparer.Compare(key1, key2) == 0;
			}
			return key1.Equals(key2);
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000ECA6 File Offset: 0x0000DCA6
		protected virtual string ElementName
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000ECAD File Offset: 0x0000DCAD
		protected virtual bool IsElementName(string elementName)
		{
			return false;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000ECB0 File Offset: 0x0000DCB0
		internal bool IsLockableElement(string elementName)
		{
			if (this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMap || this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate)
			{
				return elementName == this.AddElementName || elementName == this.RemoveElementName || elementName == this.ClearElementName;
			}
			return elementName == this.ElementName || this.IsElementName(elementName);
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000ED14 File Offset: 0x0000DD14
		internal string LockableElements
		{
			get
			{
				if (this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMap || this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate)
				{
					string text = "'" + this.AddElementName + "'";
					if (this.RemoveElementName.Length != 0)
					{
						text = text + ", '" + this.RemoveElementName + "'";
					}
					if (this.ClearElementName.Length != 0)
					{
						text = text + ", '" + this.ClearElementName + "'";
					}
					return text;
				}
				if (!string.IsNullOrEmpty(this.ElementName))
				{
					return "'" + this.ElementName + "'";
				}
				return string.Empty;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000EDBB File Offset: 0x0000DDBB
		protected virtual bool ThrowOnDuplicate
		{
			get
			{
				return this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMap || this.CollectionType == ConfigurationElementCollectionType.AddRemoveClearMapAlternate;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000EDD2 File Offset: 0x0000DDD2
		public virtual ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.AddRemoveClearMap;
			}
		}

		// Token: 0x04000228 RID: 552
		internal const string DefaultAddItemName = "add";

		// Token: 0x04000229 RID: 553
		internal const string DefaultRemoveItemName = "remove";

		// Token: 0x0400022A RID: 554
		internal const string DefaultClearItemsName = "clear";

		// Token: 0x0400022B RID: 555
		private int _removedItemCount;

		// Token: 0x0400022C RID: 556
		private int _inheritedCount;

		// Token: 0x0400022D RID: 557
		private ArrayList _items = new ArrayList();

		// Token: 0x0400022E RID: 558
		private string _addElement = "add";

		// Token: 0x0400022F RID: 559
		private string _removeElement = "remove";

		// Token: 0x04000230 RID: 560
		private string _clearElement = "clear";

		// Token: 0x04000231 RID: 561
		private bool bEmitClearTag;

		// Token: 0x04000232 RID: 562
		private bool bCollectionCleared;

		// Token: 0x04000233 RID: 563
		private bool bModified;

		// Token: 0x04000234 RID: 564
		private bool bReadOnly;

		// Token: 0x04000235 RID: 565
		private IComparer _comparer;

		// Token: 0x04000236 RID: 566
		internal bool internalAddToEnd;

		// Token: 0x04000237 RID: 567
		internal string internalElementTagName = string.Empty;

		// Token: 0x02000026 RID: 38
		private enum InheritedType
		{
			// Token: 0x04000239 RID: 569
			inNeither,
			// Token: 0x0400023A RID: 570
			inParent,
			// Token: 0x0400023B RID: 571
			inSelf,
			// Token: 0x0400023C RID: 572
			inBothSame,
			// Token: 0x0400023D RID: 573
			inBothDiff,
			// Token: 0x0400023E RID: 574
			inBothCopyNoRemove
		}

		// Token: 0x02000027 RID: 39
		private enum EntryType
		{
			// Token: 0x04000240 RID: 576
			Inherited,
			// Token: 0x04000241 RID: 577
			Replaced,
			// Token: 0x04000242 RID: 578
			Removed,
			// Token: 0x04000243 RID: 579
			Added
		}

		// Token: 0x02000028 RID: 40
		private class Entry
		{
			// Token: 0x0600021D RID: 541 RVA: 0x0000EDD5 File Offset: 0x0000DDD5
			internal object GetKey(ConfigurationElementCollection ThisCollection)
			{
				if (this._value != null)
				{
					return ThisCollection.GetElementKeyInternal(this._value);
				}
				return this._key;
			}

			// Token: 0x0600021E RID: 542 RVA: 0x0000EDF2 File Offset: 0x0000DDF2
			internal Entry(ConfigurationElementCollection.EntryType type, object key, ConfigurationElement value)
			{
				this._entryType = type;
				this._key = key;
				this._value = value;
			}

			// Token: 0x04000244 RID: 580
			internal ConfigurationElementCollection.EntryType _entryType;

			// Token: 0x04000245 RID: 581
			internal object _key;

			// Token: 0x04000246 RID: 582
			internal ConfigurationElement _value;
		}

		// Token: 0x02000029 RID: 41
		private class Enumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x0600021F RID: 543 RVA: 0x0000EE0F File Offset: 0x0000DE0F
			internal Enumerator(ArrayList items, ConfigurationElementCollection collection)
			{
				this._itemsEnumerator = items.GetEnumerator();
				this.ThisCollection = collection;
			}

			// Token: 0x06000220 RID: 544 RVA: 0x0000EE38 File Offset: 0x0000DE38
			bool IEnumerator.MoveNext()
			{
				while (this._itemsEnumerator.MoveNext())
				{
					ConfigurationElementCollection.Entry entry = (ConfigurationElementCollection.Entry)this._itemsEnumerator.Current;
					if (entry._entryType != ConfigurationElementCollection.EntryType.Removed)
					{
						this._current.Key = ((entry.GetKey(this.ThisCollection) != null) ? entry.GetKey(this.ThisCollection) : "key");
						this._current.Value = entry._value;
						return true;
					}
				}
				return false;
			}

			// Token: 0x06000221 RID: 545 RVA: 0x0000EEAE File Offset: 0x0000DEAE
			void IEnumerator.Reset()
			{
				this._itemsEnumerator.Reset();
			}

			// Token: 0x17000080 RID: 128
			// (get) Token: 0x06000222 RID: 546 RVA: 0x0000EEBB File Offset: 0x0000DEBB
			object IEnumerator.Current
			{
				get
				{
					return this._current.Value;
				}
			}

			// Token: 0x17000081 RID: 129
			// (get) Token: 0x06000223 RID: 547 RVA: 0x0000EEC8 File Offset: 0x0000DEC8
			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x17000082 RID: 130
			// (get) Token: 0x06000224 RID: 548 RVA: 0x0000EED0 File Offset: 0x0000DED0
			object IDictionaryEnumerator.Key
			{
				get
				{
					return this._current.Key;
				}
			}

			// Token: 0x17000083 RID: 131
			// (get) Token: 0x06000225 RID: 549 RVA: 0x0000EEDD File Offset: 0x0000DEDD
			object IDictionaryEnumerator.Value
			{
				get
				{
					return this._current.Value;
				}
			}

			// Token: 0x04000247 RID: 583
			private IEnumerator _itemsEnumerator;

			// Token: 0x04000248 RID: 584
			private DictionaryEntry _current = default(DictionaryEntry);

			// Token: 0x04000249 RID: 585
			private ConfigurationElementCollection ThisCollection;
		}
	}
}

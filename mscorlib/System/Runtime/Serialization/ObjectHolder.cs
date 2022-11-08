using System;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x02000365 RID: 869
	internal sealed class ObjectHolder
	{
		// Token: 0x0600223D RID: 8765 RVA: 0x00056762 File Offset: 0x00055762
		internal ObjectHolder(long objID) : this(null, objID, null, null, 0L, null, null)
		{
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x00056774 File Offset: 0x00055774
		internal ObjectHolder(object obj, long objID, SerializationInfo info, ISerializationSurrogate surrogate, long idOfContainingObj, FieldInfo field, int[] arrayIndex)
		{
			this.m_object = obj;
			this.m_id = objID;
			this.m_flags = 0;
			this.m_missingElementsRemaining = 0;
			this.m_missingDecendents = 0;
			this.m_dependentObjects = null;
			this.m_next = null;
			this.m_serInfo = info;
			this.m_surrogate = surrogate;
			this.m_markForFixupWhenAvailable = false;
			if (obj is TypeLoadExceptionHolder)
			{
				this.m_typeLoad = (TypeLoadExceptionHolder)obj;
			}
			if (idOfContainingObj != 0L && ((field != null && field.FieldType.IsValueType) || arrayIndex != null))
			{
				if (idOfContainingObj == objID)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_ParentChildIdentical"));
				}
				this.m_valueFixup = new ValueTypeFixupInfo(idOfContainingObj, field, arrayIndex);
			}
			this.SetFlags();
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x0005682C File Offset: 0x0005582C
		internal ObjectHolder(string obj, long objID, SerializationInfo info, ISerializationSurrogate surrogate, long idOfContainingObj, FieldInfo field, int[] arrayIndex)
		{
			this.m_object = obj;
			this.m_id = objID;
			this.m_flags = 0;
			this.m_missingElementsRemaining = 0;
			this.m_missingDecendents = 0;
			this.m_dependentObjects = null;
			this.m_next = null;
			this.m_serInfo = info;
			this.m_surrogate = surrogate;
			this.m_markForFixupWhenAvailable = false;
			if (idOfContainingObj != 0L && arrayIndex != null)
			{
				this.m_valueFixup = new ValueTypeFixupInfo(idOfContainingObj, field, arrayIndex);
			}
			if (this.m_valueFixup != null)
			{
				this.m_flags |= 8;
			}
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x000568B7 File Offset: 0x000558B7
		private void IncrementDescendentFixups(int amount)
		{
			this.m_missingDecendents += amount;
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x000568C7 File Offset: 0x000558C7
		internal void DecrementFixupsRemaining(ObjectManager manager)
		{
			this.m_missingElementsRemaining--;
			if (this.RequiresValueTypeFixup)
			{
				this.UpdateDescendentDependencyChain(-1, manager);
			}
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x000568E7 File Offset: 0x000558E7
		internal void RemoveDependency(long id)
		{
			this.m_dependentObjects.RemoveElement(id);
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x000568F6 File Offset: 0x000558F6
		internal void AddFixup(FixupHolder fixup, ObjectManager manager)
		{
			if (this.m_missingElements == null)
			{
				this.m_missingElements = new FixupHolderList();
			}
			this.m_missingElements.Add(fixup);
			this.m_missingElementsRemaining++;
			if (this.RequiresValueTypeFixup)
			{
				this.UpdateDescendentDependencyChain(1, manager);
			}
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x00056938 File Offset: 0x00055938
		private void UpdateDescendentDependencyChain(int amount, ObjectManager manager)
		{
			ObjectHolder objectHolder = this;
			do
			{
				objectHolder = manager.FindOrCreateObjectHolder(objectHolder.ContainerID);
				objectHolder.IncrementDescendentFixups(amount);
			}
			while (objectHolder.RequiresValueTypeFixup);
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x00056963 File Offset: 0x00055963
		internal void AddDependency(long dependentObject)
		{
			if (this.m_dependentObjects == null)
			{
				this.m_dependentObjects = new LongList();
			}
			this.m_dependentObjects.Add(dependentObject);
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x00056984 File Offset: 0x00055984
		internal void UpdateData(object obj, SerializationInfo info, ISerializationSurrogate surrogate, long idOfContainer, FieldInfo field, int[] arrayIndex, ObjectManager manager)
		{
			this.SetObjectValue(obj, manager);
			this.m_serInfo = info;
			this.m_surrogate = surrogate;
			if (idOfContainer != 0L && ((field != null && field.FieldType.IsValueType) || arrayIndex != null))
			{
				if (idOfContainer == this.m_id)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_ParentChildIdentical"));
				}
				this.m_valueFixup = new ValueTypeFixupInfo(idOfContainer, field, arrayIndex);
			}
			this.SetFlags();
			if (this.RequiresValueTypeFixup)
			{
				this.UpdateDescendentDependencyChain(this.m_missingElementsRemaining, manager);
			}
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x00056A0B File Offset: 0x00055A0B
		internal void MarkForCompletionWhenAvailable()
		{
			this.m_markForFixupWhenAvailable = true;
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x00056A14 File Offset: 0x00055A14
		internal void SetFlags()
		{
			if (this.m_object is IObjectReference)
			{
				this.m_flags |= 1;
			}
			this.m_flags &= -7;
			if (this.m_surrogate != null)
			{
				this.m_flags |= 4;
			}
			else if (this.m_object is ISerializable)
			{
				this.m_flags |= 2;
			}
			if (this.m_valueFixup != null)
			{
				this.m_flags |= 8;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06002249 RID: 8777 RVA: 0x00056A94 File Offset: 0x00055A94
		// (set) Token: 0x0600224A RID: 8778 RVA: 0x00056AA4 File Offset: 0x00055AA4
		internal bool IsIncompleteObjectReference
		{
			get
			{
				return (this.m_flags & 1) != 0;
			}
			set
			{
				if (value)
				{
					this.m_flags |= 1;
					return;
				}
				this.m_flags &= -2;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x0600224B RID: 8779 RVA: 0x00056AC7 File Offset: 0x00055AC7
		internal bool RequiresDelayedFixup
		{
			get
			{
				return (this.m_flags & 7) != 0;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x0600224C RID: 8780 RVA: 0x00056AD7 File Offset: 0x00055AD7
		internal bool RequiresValueTypeFixup
		{
			get
			{
				return (this.m_flags & 8) != 0;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x0600224D RID: 8781 RVA: 0x00056AE7 File Offset: 0x00055AE7
		// (set) Token: 0x0600224E RID: 8782 RVA: 0x00056B1B File Offset: 0x00055B1B
		internal bool ValueTypeFixupPerformed
		{
			get
			{
				return (this.m_flags & 32768) != 0 || (this.m_object != null && (this.m_dependentObjects == null || this.m_dependentObjects.Count == 0));
			}
			set
			{
				if (value)
				{
					this.m_flags |= 32768;
				}
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x0600224F RID: 8783 RVA: 0x00056B32 File Offset: 0x00055B32
		internal bool HasISerializable
		{
			get
			{
				return (this.m_flags & 2) != 0;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06002250 RID: 8784 RVA: 0x00056B42 File Offset: 0x00055B42
		internal bool HasSurrogate
		{
			get
			{
				return (this.m_flags & 4) != 0;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06002251 RID: 8785 RVA: 0x00056B52 File Offset: 0x00055B52
		internal bool CanSurrogatedObjectValueChange
		{
			get
			{
				return this.m_surrogate == null || this.m_surrogate.GetType() != typeof(SurrogateForCyclicalReference);
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06002252 RID: 8786 RVA: 0x00056B78 File Offset: 0x00055B78
		internal bool CanObjectValueChange
		{
			get
			{
				return this.IsIncompleteObjectReference || (this.HasSurrogate && this.CanSurrogatedObjectValueChange);
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06002253 RID: 8787 RVA: 0x00056B94 File Offset: 0x00055B94
		internal int DirectlyDependentObjects
		{
			get
			{
				return this.m_missingElementsRemaining;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06002254 RID: 8788 RVA: 0x00056B9C File Offset: 0x00055B9C
		internal int TotalDependentObjects
		{
			get
			{
				return this.m_missingElementsRemaining + this.m_missingDecendents;
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06002255 RID: 8789 RVA: 0x00056BAB File Offset: 0x00055BAB
		// (set) Token: 0x06002256 RID: 8790 RVA: 0x00056BB3 File Offset: 0x00055BB3
		internal bool Reachable
		{
			get
			{
				return this.m_reachable;
			}
			set
			{
				this.m_reachable = value;
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06002257 RID: 8791 RVA: 0x00056BBC File Offset: 0x00055BBC
		internal bool TypeLoadExceptionReachable
		{
			get
			{
				return this.m_typeLoad != null;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06002258 RID: 8792 RVA: 0x00056BCA File Offset: 0x00055BCA
		// (set) Token: 0x06002259 RID: 8793 RVA: 0x00056BD2 File Offset: 0x00055BD2
		internal TypeLoadExceptionHolder TypeLoadException
		{
			get
			{
				return this.m_typeLoad;
			}
			set
			{
				this.m_typeLoad = value;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x0600225A RID: 8794 RVA: 0x00056BDB File Offset: 0x00055BDB
		internal object ObjectValue
		{
			get
			{
				return this.m_object;
			}
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x00056BE3 File Offset: 0x00055BE3
		internal void SetObjectValue(object obj, ObjectManager manager)
		{
			this.m_object = obj;
			if (obj == manager.TopObject)
			{
				this.m_reachable = true;
			}
			if (obj is TypeLoadExceptionHolder)
			{
				this.m_typeLoad = (TypeLoadExceptionHolder)obj;
			}
			if (this.m_markForFixupWhenAvailable)
			{
				manager.CompleteObject(this, true);
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x0600225C RID: 8796 RVA: 0x00056C20 File Offset: 0x00055C20
		// (set) Token: 0x0600225D RID: 8797 RVA: 0x00056C28 File Offset: 0x00055C28
		internal SerializationInfo SerializationInfo
		{
			get
			{
				return this.m_serInfo;
			}
			set
			{
				this.m_serInfo = value;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x0600225E RID: 8798 RVA: 0x00056C31 File Offset: 0x00055C31
		internal ISerializationSurrogate Surrogate
		{
			get
			{
				return this.m_surrogate;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x0600225F RID: 8799 RVA: 0x00056C39 File Offset: 0x00055C39
		// (set) Token: 0x06002260 RID: 8800 RVA: 0x00056C41 File Offset: 0x00055C41
		internal LongList DependentObjects
		{
			get
			{
				return this.m_dependentObjects;
			}
			set
			{
				this.m_dependentObjects = value;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06002261 RID: 8801 RVA: 0x00056C4A File Offset: 0x00055C4A
		// (set) Token: 0x06002262 RID: 8802 RVA: 0x00056C71 File Offset: 0x00055C71
		internal bool RequiresSerInfoFixup
		{
			get
			{
				return ((this.m_flags & 4) != 0 || (this.m_flags & 2) != 0) && (this.m_flags & 16384) == 0;
			}
			set
			{
				if (!value)
				{
					this.m_flags |= 16384;
					return;
				}
				this.m_flags &= -16385;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06002263 RID: 8803 RVA: 0x00056C9B File Offset: 0x00055C9B
		internal ValueTypeFixupInfo ValueFixup
		{
			get
			{
				return this.m_valueFixup;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06002264 RID: 8804 RVA: 0x00056CA3 File Offset: 0x00055CA3
		internal bool CompletelyFixed
		{
			get
			{
				return !this.RequiresSerInfoFixup && !this.IsIncompleteObjectReference;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06002265 RID: 8805 RVA: 0x00056CB8 File Offset: 0x00055CB8
		internal long ContainerID
		{
			get
			{
				if (this.m_valueFixup != null)
				{
					return this.m_valueFixup.ContainerID;
				}
				return 0L;
			}
		}

		// Token: 0x04000E5D RID: 3677
		internal const int INCOMPLETE_OBJECT_REFERENCE = 1;

		// Token: 0x04000E5E RID: 3678
		internal const int HAS_ISERIALIZABLE = 2;

		// Token: 0x04000E5F RID: 3679
		internal const int HAS_SURROGATE = 4;

		// Token: 0x04000E60 RID: 3680
		internal const int REQUIRES_VALUETYPE_FIXUP = 8;

		// Token: 0x04000E61 RID: 3681
		internal const int REQUIRES_DELAYED_FIXUP = 7;

		// Token: 0x04000E62 RID: 3682
		internal const int SER_INFO_FIXED = 16384;

		// Token: 0x04000E63 RID: 3683
		internal const int VALUETYPE_FIXUP_PERFORMED = 32768;

		// Token: 0x04000E64 RID: 3684
		private object m_object;

		// Token: 0x04000E65 RID: 3685
		internal long m_id;

		// Token: 0x04000E66 RID: 3686
		private int m_missingElementsRemaining;

		// Token: 0x04000E67 RID: 3687
		private int m_missingDecendents;

		// Token: 0x04000E68 RID: 3688
		internal SerializationInfo m_serInfo;

		// Token: 0x04000E69 RID: 3689
		internal ISerializationSurrogate m_surrogate;

		// Token: 0x04000E6A RID: 3690
		internal FixupHolderList m_missingElements;

		// Token: 0x04000E6B RID: 3691
		internal LongList m_dependentObjects;

		// Token: 0x04000E6C RID: 3692
		internal ObjectHolder m_next;

		// Token: 0x04000E6D RID: 3693
		internal int m_flags;

		// Token: 0x04000E6E RID: 3694
		private bool m_markForFixupWhenAvailable;

		// Token: 0x04000E6F RID: 3695
		private ValueTypeFixupInfo m_valueFixup;

		// Token: 0x04000E70 RID: 3696
		private TypeLoadExceptionHolder m_typeLoad;

		// Token: 0x04000E71 RID: 3697
		private bool m_reachable;
	}
}

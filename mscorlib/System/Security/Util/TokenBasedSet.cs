using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security.Util
{
	// Token: 0x02000488 RID: 1160
	[Serializable]
	internal class TokenBasedSet
	{
		// Token: 0x06002E0C RID: 11788 RVA: 0x0009A6CB File Offset: 0x000996CB
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			this.OnDeserializedInternal();
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x0009A6D3 File Offset: 0x000996D3
		private void OnDeserializedInternal()
		{
			if (this.m_objSet != null)
			{
				if (this.m_cElt == 1)
				{
					this.m_Obj = this.m_objSet[this.m_maxIndex];
				}
				else
				{
					this.m_Set = this.m_objSet;
				}
				this.m_objSet = null;
			}
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x0009A710 File Offset: 0x00099710
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			if ((ctx.State & ~(StreamingContextStates.Clone | StreamingContextStates.CrossAppDomain)) != (StreamingContextStates)0)
			{
				if (this.m_cElt == 1)
				{
					this.m_objSet = new object[this.m_maxIndex + 1];
					this.m_objSet[this.m_maxIndex] = this.m_Obj;
					return;
				}
				if (this.m_cElt > 0)
				{
					this.m_objSet = this.m_Set;
				}
			}
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x0009A771 File Offset: 0x00099771
		[OnSerialized]
		private void OnSerialized(StreamingContext ctx)
		{
			if ((ctx.State & ~(StreamingContextStates.Clone | StreamingContextStates.CrossAppDomain)) != (StreamingContextStates)0)
			{
				this.m_objSet = null;
			}
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x0009A78C File Offset: 0x0009978C
		internal bool MoveNext(ref TokenBasedSetEnumerator e)
		{
			switch (this.m_cElt)
			{
			case 0:
				return false;
			case 1:
				if (e.Index == -1)
				{
					e.Index = this.m_maxIndex;
					e.Current = this.m_Obj;
					return true;
				}
				e.Index = (int)((short)(this.m_maxIndex + 1));
				e.Current = null;
				return false;
			default:
				while (++e.Index <= this.m_maxIndex)
				{
					e.Current = this.m_Set[e.Index];
					if (e.Current != null)
					{
						return true;
					}
				}
				e.Current = null;
				return false;
			}
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x0009A82B File Offset: 0x0009982B
		internal TokenBasedSet()
		{
			this.Reset();
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x0009A848 File Offset: 0x00099848
		internal TokenBasedSet(TokenBasedSet tbSet)
		{
			if (tbSet == null)
			{
				this.Reset();
				return;
			}
			if (tbSet.m_cElt > 1)
			{
				object[] set = tbSet.m_Set;
				int num = set.Length;
				object[] array = new object[num];
				Array.Copy(set, 0, array, 0, num);
				this.m_Set = array;
			}
			else
			{
				this.m_Obj = tbSet.m_Obj;
			}
			this.m_cElt = tbSet.m_cElt;
			this.m_maxIndex = tbSet.m_maxIndex;
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x0009A8C6 File Offset: 0x000998C6
		internal void Reset()
		{
			this.m_Obj = null;
			this.m_Set = null;
			this.m_cElt = 0;
			this.m_maxIndex = -1;
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x0009A8E4 File Offset: 0x000998E4
		internal void SetItem(int index, object item)
		{
			if (item == null)
			{
				this.RemoveItem(index);
				return;
			}
			switch (this.m_cElt)
			{
			case 0:
				this.m_cElt = 1;
				this.m_maxIndex = (int)((short)index);
				this.m_Obj = item;
				return;
			case 1:
			{
				if (index == this.m_maxIndex)
				{
					this.m_Obj = item;
					return;
				}
				object obj = this.m_Obj;
				int num = Math.Max(this.m_maxIndex, index);
				object[] array = new object[num + 1];
				array[this.m_maxIndex] = obj;
				array[index] = item;
				this.m_maxIndex = (int)((short)num);
				this.m_cElt = 2;
				this.m_Set = array;
				this.m_Obj = null;
				return;
			}
			default:
			{
				object[] array = this.m_Set;
				if (index >= array.Length)
				{
					object[] array2 = new object[index + 1];
					Array.Copy(array, 0, array2, 0, this.m_maxIndex + 1);
					this.m_maxIndex = (int)((short)index);
					array2[index] = item;
					this.m_Set = array2;
					this.m_cElt++;
					return;
				}
				if (array[index] == null)
				{
					this.m_cElt++;
				}
				array[index] = item;
				if (index > this.m_maxIndex)
				{
					this.m_maxIndex = (int)((short)index);
				}
				return;
			}
			}
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x0009A9FC File Offset: 0x000999FC
		internal object GetItem(int index)
		{
			switch (this.m_cElt)
			{
			case 0:
				return null;
			case 1:
				if (index == this.m_maxIndex)
				{
					return this.m_Obj;
				}
				return null;
			default:
				if (index < this.m_Set.Length)
				{
					return this.m_Set[index];
				}
				return null;
			}
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x0009AA4C File Offset: 0x00099A4C
		internal object RemoveItem(int index)
		{
			object result = null;
			switch (this.m_cElt)
			{
			case 0:
				result = null;
				break;
			case 1:
				if (index != this.m_maxIndex)
				{
					result = null;
				}
				else
				{
					result = this.m_Obj;
					this.Reset();
				}
				break;
			default:
				if (index < this.m_Set.Length && this.m_Set[index] != null)
				{
					result = this.m_Set[index];
					this.m_Set[index] = null;
					this.m_cElt--;
					if (index == this.m_maxIndex)
					{
						this.ResetMaxIndex(this.m_Set);
					}
					if (this.m_cElt == 1)
					{
						this.m_Obj = this.m_Set[this.m_maxIndex];
						this.m_Set = null;
					}
				}
				break;
			}
			return result;
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x0009AB04 File Offset: 0x00099B04
		private void ResetMaxIndex(object[] aObj)
		{
			for (int i = aObj.Length - 1; i >= 0; i--)
			{
				if (aObj[i] != null)
				{
					this.m_maxIndex = (int)((short)i);
					return;
				}
			}
			this.m_maxIndex = -1;
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x0009AB36 File Offset: 0x00099B36
		internal int GetStartingIndex()
		{
			if (this.m_cElt <= 1)
			{
				return this.m_maxIndex;
			}
			return 0;
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x0009AB49 File Offset: 0x00099B49
		internal int GetCount()
		{
			return this.m_cElt;
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x0009AB51 File Offset: 0x00099B51
		internal int GetMaxUsedIndex()
		{
			return this.m_maxIndex;
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x0009AB59 File Offset: 0x00099B59
		internal bool FastIsEmpty()
		{
			return this.m_cElt == 0;
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x0009AB64 File Offset: 0x00099B64
		internal TokenBasedSet SpecialUnion(TokenBasedSet other, ref bool canUnrestrictedOverride)
		{
			this.OnDeserializedInternal();
			TokenBasedSet tokenBasedSet = new TokenBasedSet();
			int num;
			if (other != null)
			{
				other.OnDeserializedInternal();
				num = ((this.GetMaxUsedIndex() > other.GetMaxUsedIndex()) ? this.GetMaxUsedIndex() : other.GetMaxUsedIndex());
			}
			else
			{
				num = this.GetMaxUsedIndex();
			}
			for (int i = 0; i <= num; i++)
			{
				object item = this.GetItem(i);
				IPermission permission = item as IPermission;
				ISecurityElementFactory securityElementFactory = item as ISecurityElementFactory;
				object obj = (other != null) ? other.GetItem(i) : null;
				IPermission permission2 = obj as IPermission;
				ISecurityElementFactory securityElementFactory2 = obj as ISecurityElementFactory;
				if (item != null || obj != null)
				{
					if (item == null)
					{
						if (securityElementFactory2 != null)
						{
							permission2 = PermissionSet.CreatePerm(securityElementFactory2, false);
						}
						PermissionToken token = PermissionToken.GetToken(permission2);
						if (token == null)
						{
							throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
						}
						tokenBasedSet.SetItem(token.m_index, permission2);
						if (!CodeAccessPermission.CanUnrestrictedOverride(permission2))
						{
							canUnrestrictedOverride = false;
						}
					}
					else if (obj == null)
					{
						if (securityElementFactory != null)
						{
							permission = PermissionSet.CreatePerm(securityElementFactory, false);
						}
						PermissionToken token2 = PermissionToken.GetToken(permission);
						if (token2 == null)
						{
							throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
						}
						tokenBasedSet.SetItem(token2.m_index, permission);
						if (!CodeAccessPermission.CanUnrestrictedOverride(permission))
						{
							canUnrestrictedOverride = false;
						}
					}
				}
			}
			return tokenBasedSet;
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x0009AC98 File Offset: 0x00099C98
		internal void SpecialSplit(ref TokenBasedSet unrestrictedPermSet, ref TokenBasedSet normalPermSet, bool ignoreTypeLoadFailures)
		{
			int maxUsedIndex = this.GetMaxUsedIndex();
			for (int i = this.GetStartingIndex(); i <= maxUsedIndex; i++)
			{
				object item = this.GetItem(i);
				if (item != null)
				{
					IPermission permission = item as IPermission;
					if (permission == null)
					{
						permission = PermissionSet.CreatePerm(item, ignoreTypeLoadFailures);
					}
					PermissionToken token = PermissionToken.GetToken(permission);
					if (permission != null && token != null)
					{
						if (permission is IUnrestrictedPermission)
						{
							if (unrestrictedPermSet == null)
							{
								unrestrictedPermSet = new TokenBasedSet();
							}
							unrestrictedPermSet.SetItem(token.m_index, permission);
						}
						else
						{
							if (normalPermSet == null)
							{
								normalPermSet = new TokenBasedSet();
							}
							normalPermSet.SetItem(token.m_index, permission);
						}
					}
				}
			}
		}

		// Token: 0x040017B0 RID: 6064
		private int m_initSize = 24;

		// Token: 0x040017B1 RID: 6065
		private int m_increment = 8;

		// Token: 0x040017B2 RID: 6066
		private object[] m_objSet;

		// Token: 0x040017B3 RID: 6067
		[OptionalField(VersionAdded = 2)]
		private object m_Obj;

		// Token: 0x040017B4 RID: 6068
		[OptionalField(VersionAdded = 2)]
		private object[] m_Set;

		// Token: 0x040017B5 RID: 6069
		private int m_cElt;

		// Token: 0x040017B6 RID: 6070
		private int m_maxIndex;
	}
}

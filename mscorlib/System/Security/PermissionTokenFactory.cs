using System;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;

namespace System.Security
{
	// Token: 0x0200067D RID: 1661
	internal class PermissionTokenFactory
	{
		// Token: 0x06003BF7 RID: 15351 RVA: 0x000CC954 File Offset: 0x000CB954
		internal PermissionTokenFactory(int size)
		{
			this.m_builtIn = new PermissionToken[17];
			this.m_size = size;
			this.m_index = 17;
			this.m_tokenTable = null;
			this.m_handleTable = new Hashtable(size);
			this.m_indexTable = new Hashtable(size);
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x000CC9A4 File Offset: 0x000CB9A4
		internal PermissionToken FindToken(Type cls)
		{
			IntPtr value = cls.TypeHandle.Value;
			PermissionToken permissionToken = (PermissionToken)this.m_handleTable[value];
			if (permissionToken != null)
			{
				return permissionToken;
			}
			if (this.m_tokenTable == null)
			{
				return null;
			}
			permissionToken = (PermissionToken)this.m_tokenTable[cls.AssemblyQualifiedName];
			if (permissionToken != null)
			{
				lock (this)
				{
					this.m_handleTable.Add(value, permissionToken);
				}
			}
			return permissionToken;
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x000CCA34 File Offset: 0x000CBA34
		internal PermissionToken FindTokenByIndex(int i)
		{
			PermissionToken result;
			if (i < 17)
			{
				result = this.BuiltInGetToken(i, null, null);
			}
			else
			{
				result = (PermissionToken)this.m_indexTable[i];
			}
			return result;
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x000CCA6C File Offset: 0x000CBA6C
		internal PermissionToken GetToken(Type cls, IPermission perm)
		{
			IntPtr value = cls.TypeHandle.Value;
			object obj = this.m_handleTable[value];
			if (obj == null)
			{
				string assemblyQualifiedName = cls.AssemblyQualifiedName;
				obj = ((this.m_tokenTable != null) ? this.m_tokenTable[assemblyQualifiedName] : null);
				if (obj == null)
				{
					lock (this)
					{
						if (this.m_tokenTable != null)
						{
							obj = this.m_tokenTable[assemblyQualifiedName];
						}
						else
						{
							this.m_tokenTable = new Hashtable(this.m_size, 1f, new PermissionTokenKeyComparer(CultureInfo.InvariantCulture));
						}
						if (obj == null)
						{
							if (perm != null)
							{
								if (CodeAccessPermission.CanUnrestrictedOverride(perm))
								{
									obj = new PermissionToken(this.m_index++, PermissionTokenType.IUnrestricted, assemblyQualifiedName);
								}
								else
								{
									obj = new PermissionToken(this.m_index++, PermissionTokenType.Normal, assemblyQualifiedName);
								}
							}
							else if (cls.GetInterface("System.Security.Permissions.IUnrestrictedPermission") != null)
							{
								obj = new PermissionToken(this.m_index++, PermissionTokenType.IUnrestricted, assemblyQualifiedName);
							}
							else
							{
								obj = new PermissionToken(this.m_index++, PermissionTokenType.Normal, assemblyQualifiedName);
							}
							this.m_tokenTable.Add(assemblyQualifiedName, obj);
							this.m_indexTable.Add(this.m_index - 1, obj);
							PermissionToken.s_tokenSet.SetItem(((PermissionToken)obj).m_index, obj);
						}
						if (!this.m_handleTable.Contains(value))
						{
							this.m_handleTable.Add(value, obj);
						}
						goto IL_1BF;
					}
				}
				lock (this)
				{
					if (!this.m_handleTable.Contains(value))
					{
						this.m_handleTable.Add(value, obj);
					}
				}
			}
			IL_1BF:
			if ((((PermissionToken)obj).m_type & PermissionTokenType.DontKnow) != (PermissionTokenType)0)
			{
				if (perm != null)
				{
					if (CodeAccessPermission.CanUnrestrictedOverride(perm))
					{
						((PermissionToken)obj).m_type = PermissionTokenType.IUnrestricted;
					}
					else
					{
						((PermissionToken)obj).m_type = PermissionTokenType.Normal;
					}
					((PermissionToken)obj).m_strTypeName = perm.GetType().AssemblyQualifiedName;
				}
				else
				{
					if (cls.GetInterface("System.Security.Permissions.IUnrestrictedPermission") != null)
					{
						((PermissionToken)obj).m_type = PermissionTokenType.IUnrestricted;
					}
					else
					{
						((PermissionToken)obj).m_type = PermissionTokenType.Normal;
					}
					((PermissionToken)obj).m_strTypeName = cls.AssemblyQualifiedName;
				}
			}
			return (PermissionToken)obj;
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x000CCCF8 File Offset: 0x000CBCF8
		internal PermissionToken GetToken(string typeStr)
		{
			object obj = null;
			obj = ((this.m_tokenTable != null) ? this.m_tokenTable[typeStr] : null);
			if (obj == null)
			{
				lock (this)
				{
					if (this.m_tokenTable != null)
					{
						obj = this.m_tokenTable[typeStr];
					}
					else
					{
						this.m_tokenTable = new Hashtable(this.m_size, 1f, new PermissionTokenKeyComparer(CultureInfo.InvariantCulture));
					}
					if (obj == null)
					{
						obj = new PermissionToken(this.m_index++, PermissionTokenType.DontKnow, typeStr);
						this.m_tokenTable.Add(typeStr, obj);
						this.m_indexTable.Add(this.m_index - 1, obj);
						PermissionToken.s_tokenSet.SetItem(((PermissionToken)obj).m_index, obj);
					}
				}
			}
			return (PermissionToken)obj;
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x000CCDDC File Offset: 0x000CBDDC
		internal PermissionToken BuiltInGetToken(int index, IPermission perm, Type cls)
		{
			PermissionToken permissionToken = this.m_builtIn[index];
			if (permissionToken == null)
			{
				lock (this)
				{
					permissionToken = this.m_builtIn[index];
					if (permissionToken == null)
					{
						PermissionTokenType permissionTokenType = PermissionTokenType.DontKnow;
						if (perm != null)
						{
							if (CodeAccessSecurityEngine.DoesFullTrustMeanFullTrust() || perm is IUnrestrictedPermission)
							{
								permissionTokenType = PermissionTokenType.IUnrestricted;
							}
							else
							{
								permissionTokenType = PermissionTokenType.Normal;
							}
						}
						else if (cls != null)
						{
							if (CodeAccessSecurityEngine.DoesFullTrustMeanFullTrust() || cls.GetInterface("System.Security.Permissions.IUnrestrictedPermission") != null)
							{
								permissionTokenType = PermissionTokenType.IUnrestricted;
							}
							else
							{
								permissionTokenType = PermissionTokenType.Normal;
							}
						}
						permissionToken = new PermissionToken(index, permissionTokenType | PermissionTokenType.BuiltIn, null);
						this.m_builtIn[index] = permissionToken;
						PermissionToken.s_tokenSet.SetItem(permissionToken.m_index, permissionToken);
					}
				}
			}
			if ((permissionToken.m_type & PermissionTokenType.DontKnow) != (PermissionTokenType)0)
			{
				permissionToken.m_type = PermissionTokenType.BuiltIn;
				if (perm != null)
				{
					if (CodeAccessSecurityEngine.DoesFullTrustMeanFullTrust() || perm is IUnrestrictedPermission)
					{
						permissionToken.m_type |= PermissionTokenType.IUnrestricted;
					}
					else
					{
						permissionToken.m_type |= PermissionTokenType.Normal;
					}
				}
				else if (cls != null)
				{
					if (CodeAccessSecurityEngine.DoesFullTrustMeanFullTrust() || cls.GetInterface("System.Security.Permissions.IUnrestrictedPermission") != null)
					{
						permissionToken.m_type |= PermissionTokenType.IUnrestricted;
					}
					else
					{
						permissionToken.m_type |= PermissionTokenType.Normal;
					}
				}
				else
				{
					permissionToken.m_type |= PermissionTokenType.DontKnow;
				}
			}
			return permissionToken;
		}

		// Token: 0x04001EF3 RID: 7923
		private const string s_unrestrictedPermissionInferfaceName = "System.Security.Permissions.IUnrestrictedPermission";

		// Token: 0x04001EF4 RID: 7924
		private int m_size;

		// Token: 0x04001EF5 RID: 7925
		private int m_index;

		// Token: 0x04001EF6 RID: 7926
		private Hashtable m_tokenTable;

		// Token: 0x04001EF7 RID: 7927
		private Hashtable m_handleTable;

		// Token: 0x04001EF8 RID: 7928
		private Hashtable m_indexTable;

		// Token: 0x04001EF9 RID: 7929
		private PermissionToken[] m_builtIn;
	}
}

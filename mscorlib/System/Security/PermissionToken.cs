using System;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Security.Util;

namespace System.Security
{
	// Token: 0x0200067C RID: 1660
	[Serializable]
	internal sealed class PermissionToken : ISecurityEncodable
	{
		// Token: 0x06003BEA RID: 15338 RVA: 0x000CC5B4 File Offset: 0x000CB5B4
		internal static bool IsMscorlibClassName(string className)
		{
			int num = className.IndexOf(',');
			if (num == -1)
			{
				return true;
			}
			num = className.LastIndexOf(']');
			if (num == -1)
			{
				num = 0;
			}
			for (int i = num; i < className.Length; i++)
			{
				if ((className[i] == 'm' || className[i] == 'M') && string.Compare(className, i, "mscorlib", 0, "mscorlib".Length, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x000CC623 File Offset: 0x000CB623
		static PermissionToken()
		{
			PermissionToken.s_theTokenFactory = new PermissionTokenFactory(4);
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x000CC640 File Offset: 0x000CB640
		internal PermissionToken()
		{
		}

		// Token: 0x06003BED RID: 15341 RVA: 0x000CC648 File Offset: 0x000CB648
		internal PermissionToken(int index, PermissionTokenType type, string strTypeName)
		{
			this.m_index = index;
			this.m_type = type;
			this.m_strTypeName = strTypeName;
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x000CC668 File Offset: 0x000CB668
		public static PermissionToken GetToken(Type cls)
		{
			if (cls == null)
			{
				return null;
			}
			if (cls.GetInterface("System.Security.Permissions.IBuiltInPermission") != null)
			{
				if (PermissionToken.s_reflectPerm == null)
				{
					PermissionToken.s_reflectPerm = new ReflectionPermission(PermissionState.Unrestricted);
				}
				PermissionToken.s_reflectPerm.Assert();
				MethodInfo method = cls.GetMethod("GetTokenIndex", BindingFlags.Static | BindingFlags.NonPublic);
				RuntimeMethodInfo runtimeMethodInfo = method as RuntimeMethodInfo;
				int index = (int)runtimeMethodInfo.Invoke(null, BindingFlags.Default, null, null, null, true);
				return PermissionToken.s_theTokenFactory.BuiltInGetToken(index, null, cls);
			}
			return PermissionToken.s_theTokenFactory.GetToken(cls, null);
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x000CC6E4 File Offset: 0x000CB6E4
		public static PermissionToken GetToken(IPermission perm)
		{
			if (perm == null)
			{
				return null;
			}
			IBuiltInPermission builtInPermission = perm as IBuiltInPermission;
			if (builtInPermission != null)
			{
				return PermissionToken.s_theTokenFactory.BuiltInGetToken(builtInPermission.GetTokenIndex(), perm, null);
			}
			return PermissionToken.s_theTokenFactory.GetToken(perm.GetType(), perm);
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x000CC724 File Offset: 0x000CB724
		public static PermissionToken GetToken(string typeStr)
		{
			return PermissionToken.GetToken(typeStr, false);
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x000CC730 File Offset: 0x000CB730
		public static PermissionToken GetToken(string typeStr, bool bCreateMscorlib)
		{
			if (typeStr == null)
			{
				return null;
			}
			if (!PermissionToken.IsMscorlibClassName(typeStr))
			{
				return PermissionToken.s_theTokenFactory.GetToken(typeStr);
			}
			if (!bCreateMscorlib)
			{
				return null;
			}
			return PermissionToken.FindToken(Type.GetType(typeStr));
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x000CC768 File Offset: 0x000CB768
		public static PermissionToken FindToken(Type cls)
		{
			if (cls == null)
			{
				return null;
			}
			if (cls.GetInterface("System.Security.Permissions.IBuiltInPermission") != null)
			{
				if (PermissionToken.s_reflectPerm == null)
				{
					PermissionToken.s_reflectPerm = new ReflectionPermission(PermissionState.Unrestricted);
				}
				PermissionToken.s_reflectPerm.Assert();
				MethodInfo method = cls.GetMethod("GetTokenIndex", BindingFlags.Static | BindingFlags.NonPublic);
				RuntimeMethodInfo runtimeMethodInfo = method as RuntimeMethodInfo;
				int index = (int)runtimeMethodInfo.Invoke(null, BindingFlags.Default, null, null, null, true);
				return PermissionToken.s_theTokenFactory.BuiltInGetToken(index, null, cls);
			}
			return PermissionToken.s_theTokenFactory.FindToken(cls);
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x000CC7E3 File Offset: 0x000CB7E3
		public static PermissionToken FindTokenByIndex(int i)
		{
			return PermissionToken.s_theTokenFactory.FindTokenByIndex(i);
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x000CC7F0 File Offset: 0x000CB7F0
		public static bool IsTokenProperlyAssigned(IPermission perm, PermissionToken token)
		{
			PermissionToken token2 = PermissionToken.GetToken(perm);
			return token2.m_index == token.m_index && token.m_type == token2.m_type && (perm.GetType().Module.Assembly != Assembly.GetExecutingAssembly() || token2.m_index < 17);
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x000CC848 File Offset: 0x000CB848
		public SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("PermissionToken");
			if ((this.m_type & PermissionTokenType.BuiltIn) != (PermissionTokenType)0)
			{
				securityElement.AddAttribute("Index", "" + this.m_index);
			}
			else
			{
				securityElement.AddAttribute("Name", SecurityElement.Escape(this.m_strTypeName));
			}
			securityElement.AddAttribute("Type", this.m_type.ToString("F"));
			return securityElement;
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x000CC8C4 File Offset: 0x000CB8C4
		public void FromXml(SecurityElement elRoot)
		{
			elRoot.Tag.Equals("PermissionToken");
			string text = elRoot.Attribute("Name");
			PermissionToken permissionToken;
			if (text != null)
			{
				permissionToken = PermissionToken.GetToken(text, true);
			}
			else
			{
				permissionToken = PermissionToken.FindTokenByIndex(int.Parse(elRoot.Attribute("Index"), CultureInfo.InvariantCulture));
			}
			this.m_index = permissionToken.m_index;
			this.m_type = (PermissionTokenType)Enum.Parse(typeof(PermissionTokenType), elRoot.Attribute("Type"));
			this.m_strTypeName = permissionToken.m_strTypeName;
		}

		// Token: 0x04001EEC RID: 7916
		private const string c_mscorlibName = "mscorlib";

		// Token: 0x04001EED RID: 7917
		private static readonly PermissionTokenFactory s_theTokenFactory;

		// Token: 0x04001EEE RID: 7918
		private static ReflectionPermission s_reflectPerm = null;

		// Token: 0x04001EEF RID: 7919
		internal int m_index;

		// Token: 0x04001EF0 RID: 7920
		internal PermissionTokenType m_type;

		// Token: 0x04001EF1 RID: 7921
		internal string m_strTypeName;

		// Token: 0x04001EF2 RID: 7922
		internal static TokenBasedSet s_tokenSet = new TokenBasedSet();
	}
}

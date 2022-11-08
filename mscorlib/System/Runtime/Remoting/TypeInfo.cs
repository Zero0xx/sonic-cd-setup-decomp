using System;

namespace System.Runtime.Remoting
{
	// Token: 0x02000733 RID: 1843
	[Serializable]
	internal class TypeInfo : IRemotingTypeInfo
	{
		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x060041F8 RID: 16888 RVA: 0x000E0695 File Offset: 0x000DF695
		// (set) Token: 0x060041F9 RID: 16889 RVA: 0x000E069D File Offset: 0x000DF69D
		public virtual string TypeName
		{
			get
			{
				return this.serverType;
			}
			set
			{
				this.serverType = value;
			}
		}

		// Token: 0x060041FA RID: 16890 RVA: 0x000E06A8 File Offset: 0x000DF6A8
		public virtual bool CanCastTo(Type castType, object o)
		{
			if (castType != null)
			{
				if (castType == typeof(MarshalByRefObject) || castType == typeof(object))
				{
					return true;
				}
				if (castType.IsInterface)
				{
					return this.interfacesImplemented != null && this.CanCastTo(castType, this.InterfacesImplemented);
				}
				if (castType.IsMarshalByRef)
				{
					if (this.CompareTypes(castType, this.serverType))
					{
						return true;
					}
					if (this.serverHierarchy != null && this.CanCastTo(castType, this.ServerHierarchy))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060041FB RID: 16891 RVA: 0x000E0727 File Offset: 0x000DF727
		internal static string GetQualifiedTypeName(Type type)
		{
			if (type == null)
			{
				return null;
			}
			return RemotingServices.GetDefaultQualifiedTypeName(type);
		}

		// Token: 0x060041FC RID: 16892 RVA: 0x000E0734 File Offset: 0x000DF734
		internal static bool ParseTypeAndAssembly(string typeAndAssembly, out string typeName, out string assemName)
		{
			if (typeAndAssembly == null)
			{
				typeName = null;
				assemName = null;
				return false;
			}
			int num = typeAndAssembly.IndexOf(',');
			if (num == -1)
			{
				typeName = typeAndAssembly;
				assemName = null;
				return true;
			}
			typeName = typeAndAssembly.Substring(0, num);
			assemName = typeAndAssembly.Substring(num + 1).Trim();
			return true;
		}

		// Token: 0x060041FD RID: 16893 RVA: 0x000E077C File Offset: 0x000DF77C
		internal TypeInfo(Type typeOfObj)
		{
			this.ServerType = TypeInfo.GetQualifiedTypeName(typeOfObj);
			Type baseType = typeOfObj.BaseType;
			int num = 0;
			while (baseType != typeof(MarshalByRefObject) && baseType != null)
			{
				baseType = baseType.BaseType;
				num++;
			}
			string[] array = null;
			if (num > 0)
			{
				array = new string[num];
				baseType = typeOfObj.BaseType;
				for (int i = 0; i < num; i++)
				{
					array[i] = TypeInfo.GetQualifiedTypeName(baseType);
					baseType = baseType.BaseType;
				}
			}
			this.ServerHierarchy = array;
			Type[] interfaces = typeOfObj.GetInterfaces();
			string[] array2 = null;
			bool isInterface = typeOfObj.IsInterface;
			if (interfaces.Length > 0 || isInterface)
			{
				array2 = new string[interfaces.Length + (isInterface ? 1 : 0)];
				for (int j = 0; j < interfaces.Length; j++)
				{
					array2[j] = TypeInfo.GetQualifiedTypeName(interfaces[j]);
				}
				if (isInterface)
				{
					array2[array2.Length - 1] = TypeInfo.GetQualifiedTypeName(typeOfObj);
				}
			}
			this.InterfacesImplemented = array2;
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x060041FE RID: 16894 RVA: 0x000E0867 File Offset: 0x000DF867
		// (set) Token: 0x060041FF RID: 16895 RVA: 0x000E086F File Offset: 0x000DF86F
		internal string ServerType
		{
			get
			{
				return this.serverType;
			}
			set
			{
				this.serverType = value;
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06004200 RID: 16896 RVA: 0x000E0878 File Offset: 0x000DF878
		// (set) Token: 0x06004201 RID: 16897 RVA: 0x000E0880 File Offset: 0x000DF880
		private string[] ServerHierarchy
		{
			get
			{
				return this.serverHierarchy;
			}
			set
			{
				this.serverHierarchy = value;
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06004202 RID: 16898 RVA: 0x000E0889 File Offset: 0x000DF889
		// (set) Token: 0x06004203 RID: 16899 RVA: 0x000E0891 File Offset: 0x000DF891
		private string[] InterfacesImplemented
		{
			get
			{
				return this.interfacesImplemented;
			}
			set
			{
				this.interfacesImplemented = value;
			}
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x000E089C File Offset: 0x000DF89C
		private bool CompareTypes(Type type1, string type2)
		{
			Type type3 = RemotingServices.InternalGetTypeFromQualifiedTypeName(type2);
			return type1 == type3;
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x000E08B4 File Offset: 0x000DF8B4
		private bool CanCastTo(Type castType, string[] types)
		{
			bool result = false;
			if (castType != null)
			{
				for (int i = 0; i < types.Length; i++)
				{
					if (this.CompareTypes(castType, types[i]))
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x04002116 RID: 8470
		private string serverType;

		// Token: 0x04002117 RID: 8471
		private string[] serverHierarchy;

		// Token: 0x04002118 RID: 8472
		private string[] interfacesImplemented;
	}
}

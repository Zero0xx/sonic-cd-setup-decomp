using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Proxies;
using System.Security;

namespace System.Runtime.Serialization
{
	// Token: 0x02000378 RID: 888
	[ComVisible(true)]
	public sealed class SerializationInfo
	{
		// Token: 0x060022B0 RID: 8880 RVA: 0x0005786E File Offset: 0x0005686E
		[CLSCompliant(false)]
		public SerializationInfo(Type type, IFormatterConverter converter) : this(type, converter, false)
		{
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x0005787C File Offset: 0x0005687C
		[CLSCompliant(false)]
		public SerializationInfo(Type type, IFormatterConverter converter, bool requireSameTokenInPartialTrust)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (converter == null)
			{
				throw new ArgumentNullException("converter");
			}
			this.objectType = type;
			this.m_fullTypeName = type.FullName;
			this.m_assemName = type.Module.Assembly.FullName;
			this.m_members = new string[4];
			this.m_data = new object[4];
			this.m_types = new Type[4];
			this.m_converter = converter;
			this.m_currMember = 0;
			this.requireSameTokenInPartialTrust = requireSameTokenInPartialTrust;
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x060022B2 RID: 8882 RVA: 0x0005790D File Offset: 0x0005690D
		// (set) Token: 0x060022B3 RID: 8883 RVA: 0x00057915 File Offset: 0x00056915
		public string FullTypeName
		{
			get
			{
				return this.m_fullTypeName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_fullTypeName = value;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x060022B4 RID: 8884 RVA: 0x0005792C File Offset: 0x0005692C
		// (set) Token: 0x060022B5 RID: 8885 RVA: 0x00057934 File Offset: 0x00056934
		public string AssemblyName
		{
			get
			{
				return this.m_assemName;
			}
			[SecuritySafeCritical]
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.requireSameTokenInPartialTrust)
				{
					SerializationInfo.DemandForUnsafeAssemblyNameAssignments(this.m_assemName, value);
				}
				this.m_assemName = value;
			}
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x00057960 File Offset: 0x00056960
		[SecuritySafeCritical]
		public void SetType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (this.requireSameTokenInPartialTrust)
			{
				SerializationInfo.DemandForUnsafeAssemblyNameAssignments(this.objectType.Assembly.FullName, type.Assembly.FullName);
			}
			if (!object.ReferenceEquals(this.objectType, type))
			{
				this.objectType = type;
				this.m_fullTypeName = type.FullName;
				this.m_assemName = type.Module.Assembly.FullName;
			}
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x000579DC File Offset: 0x000569DC
		private static bool Compare(byte[] a, byte[] b)
		{
			if (a == null || b == null || a.Length == 0 || b.Length == 0 || a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x00057A1C File Offset: 0x00056A1C
		[SecuritySafeCritical]
		internal static void DemandForUnsafeAssemblyNameAssignments(string originalAssemblyName, string newAssemblyName)
		{
			if (!SerializationInfo.IsAssemblyNameAssignmentSafe(originalAssemblyName, newAssemblyName))
			{
				CodeAccessPermission.DemandInternal(PermissionType.SecuritySerialization);
			}
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x00057A30 File Offset: 0x00056A30
		internal static bool IsAssemblyNameAssignmentSafe(string originalAssemblyName, string newAssemblyName)
		{
			if (originalAssemblyName == newAssemblyName)
			{
				return true;
			}
			AssemblyName assemblyName = new AssemblyName(originalAssemblyName);
			AssemblyName assemblyName2 = new AssemblyName(newAssemblyName);
			return !string.Equals(assemblyName2.Name, "mscorlib", StringComparison.OrdinalIgnoreCase) && !string.Equals(assemblyName2.Name, "mscorlib.dll", StringComparison.OrdinalIgnoreCase) && SerializationInfo.Compare(assemblyName.GetPublicKeyToken(), assemblyName2.GetPublicKeyToken());
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x060022BA RID: 8890 RVA: 0x00057A8F File Offset: 0x00056A8F
		public int MemberCount
		{
			get
			{
				return this.m_currMember;
			}
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x00057A97 File Offset: 0x00056A97
		public SerializationInfoEnumerator GetEnumerator()
		{
			return new SerializationInfoEnumerator(this.m_members, this.m_data, this.m_types, this.m_currMember);
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x00057AB8 File Offset: 0x00056AB8
		private void ExpandArrays()
		{
			int num = this.m_currMember * 2;
			if (num < this.m_currMember && 2147483647 > this.m_currMember)
			{
				num = int.MaxValue;
			}
			string[] array = new string[num];
			object[] array2 = new object[num];
			Type[] array3 = new Type[num];
			Array.Copy(this.m_members, array, this.m_currMember);
			Array.Copy(this.m_data, array2, this.m_currMember);
			Array.Copy(this.m_types, array3, this.m_currMember);
			this.m_members = array;
			this.m_data = array2;
			this.m_types = array3;
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x00057B4C File Offset: 0x00056B4C
		public void AddValue(string name, object value, Type type)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			for (int i = 0; i < this.m_currMember; i++)
			{
				if (this.m_members[i].Equals(name))
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_SameNameTwice"));
				}
			}
			this.AddValue(name, value, type, this.m_currMember);
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x00057BB5 File Offset: 0x00056BB5
		public void AddValue(string name, object value)
		{
			if (value == null)
			{
				this.AddValue(name, value, typeof(object));
				return;
			}
			this.AddValue(name, value, value.GetType());
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x00057BDB File Offset: 0x00056BDB
		public void AddValue(string name, bool value)
		{
			this.AddValue(name, value, typeof(bool));
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x00057BF4 File Offset: 0x00056BF4
		public void AddValue(string name, char value)
		{
			this.AddValue(name, value, typeof(char));
		}

		// Token: 0x060022C1 RID: 8897 RVA: 0x00057C0D File Offset: 0x00056C0D
		[CLSCompliant(false)]
		public void AddValue(string name, sbyte value)
		{
			this.AddValue(name, value, typeof(sbyte));
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x00057C26 File Offset: 0x00056C26
		public void AddValue(string name, byte value)
		{
			this.AddValue(name, value, typeof(byte));
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x00057C3F File Offset: 0x00056C3F
		public void AddValue(string name, short value)
		{
			this.AddValue(name, value, typeof(short));
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x00057C58 File Offset: 0x00056C58
		[CLSCompliant(false)]
		public void AddValue(string name, ushort value)
		{
			this.AddValue(name, value, typeof(ushort));
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x00057C71 File Offset: 0x00056C71
		public void AddValue(string name, int value)
		{
			this.AddValue(name, value, typeof(int));
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x00057C8A File Offset: 0x00056C8A
		[CLSCompliant(false)]
		public void AddValue(string name, uint value)
		{
			this.AddValue(name, value, typeof(uint));
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x00057CA3 File Offset: 0x00056CA3
		public void AddValue(string name, long value)
		{
			this.AddValue(name, value, typeof(long));
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x00057CBC File Offset: 0x00056CBC
		[CLSCompliant(false)]
		public void AddValue(string name, ulong value)
		{
			this.AddValue(name, value, typeof(ulong));
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x00057CD5 File Offset: 0x00056CD5
		public void AddValue(string name, float value)
		{
			this.AddValue(name, value, typeof(float));
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x00057CEE File Offset: 0x00056CEE
		public void AddValue(string name, double value)
		{
			this.AddValue(name, value, typeof(double));
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x00057D07 File Offset: 0x00056D07
		public void AddValue(string name, decimal value)
		{
			this.AddValue(name, value, typeof(decimal));
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x00057D20 File Offset: 0x00056D20
		public void AddValue(string name, DateTime value)
		{
			this.AddValue(name, value, typeof(DateTime));
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x00057D39 File Offset: 0x00056D39
		internal void AddValue(string name, object value, Type type, int index)
		{
			if (index >= this.m_members.Length)
			{
				this.ExpandArrays();
			}
			this.m_members[index] = name;
			this.m_data[index] = value;
			this.m_types[index] = type;
			this.m_currMember++;
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x00057D7C File Offset: 0x00056D7C
		internal void UpdateValue(string name, object value, Type type)
		{
			int num = this.FindElement(name);
			if (num < 0)
			{
				this.AddValue(name, value, type, this.m_currMember);
				return;
			}
			this.m_members[num] = name;
			this.m_data[num] = value;
			this.m_types[num] = type;
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x00057DC0 File Offset: 0x00056DC0
		private int FindElement(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			for (int i = 0; i < this.m_currMember; i++)
			{
				if (this.m_members[i].Equals(name))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x00057E00 File Offset: 0x00056E00
		private object GetElement(string name, out Type foundType)
		{
			int num = this.FindElement(name);
			if (num == -1)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_NotFound"), new object[]
				{
					name
				}));
			}
			foundType = this.m_types[num];
			return this.m_data[num];
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x00057E54 File Offset: 0x00056E54
		[ComVisible(true)]
		private object GetElementNoThrow(string name, out Type foundType)
		{
			int num = this.FindElement(name);
			if (num == -1)
			{
				foundType = null;
				return null;
			}
			foundType = this.m_types[num];
			return this.m_data[num];
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x00057E84 File Offset: 0x00056E84
		public object GetValue(string name, Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Type type2;
			object element = this.GetElement(name, out type2);
			if (RemotingServices.IsTransparentProxy(element))
			{
				RealProxy realProxy = RemotingServices.GetRealProxy(element);
				if (RemotingServices.ProxyCheckCast(realProxy, type))
				{
					return element;
				}
			}
			else if (type2 == type || type.IsAssignableFrom(type2) || element == null)
			{
				return element;
			}
			return this.m_converter.Convert(element, type);
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x00057EE4 File Offset: 0x00056EE4
		[ComVisible(true)]
		internal object GetValueNoThrow(string name, Type type)
		{
			Type type2;
			object elementNoThrow = this.GetElementNoThrow(name, out type2);
			if (elementNoThrow == null)
			{
				return null;
			}
			if (RemotingServices.IsTransparentProxy(elementNoThrow))
			{
				RealProxy realProxy = RemotingServices.GetRealProxy(elementNoThrow);
				if (RemotingServices.ProxyCheckCast(realProxy, type))
				{
					return elementNoThrow;
				}
			}
			else if (type2 == type || type.IsAssignableFrom(type2) || elementNoThrow == null)
			{
				return elementNoThrow;
			}
			return this.m_converter.Convert(elementNoThrow, type);
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x00057F3C File Offset: 0x00056F3C
		public bool GetBoolean(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(bool))
			{
				return (bool)element;
			}
			return this.m_converter.ToBoolean(element);
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x00057F74 File Offset: 0x00056F74
		public char GetChar(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(char))
			{
				return (char)element;
			}
			return this.m_converter.ToChar(element);
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x00057FAC File Offset: 0x00056FAC
		[CLSCompliant(false)]
		public sbyte GetSByte(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(sbyte))
			{
				return (sbyte)element;
			}
			return this.m_converter.ToSByte(element);
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x00057FE4 File Offset: 0x00056FE4
		public byte GetByte(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(byte))
			{
				return (byte)element;
			}
			return this.m_converter.ToByte(element);
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x0005801C File Offset: 0x0005701C
		public short GetInt16(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(short))
			{
				return (short)element;
			}
			return this.m_converter.ToInt16(element);
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x00058054 File Offset: 0x00057054
		[CLSCompliant(false)]
		public ushort GetUInt16(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(ushort))
			{
				return (ushort)element;
			}
			return this.m_converter.ToUInt16(element);
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x0005808C File Offset: 0x0005708C
		public int GetInt32(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(int))
			{
				return (int)element;
			}
			return this.m_converter.ToInt32(element);
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x000580C4 File Offset: 0x000570C4
		[CLSCompliant(false)]
		public uint GetUInt32(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(uint))
			{
				return (uint)element;
			}
			return this.m_converter.ToUInt32(element);
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x000580FC File Offset: 0x000570FC
		public long GetInt64(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(long))
			{
				return (long)element;
			}
			return this.m_converter.ToInt64(element);
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x00058134 File Offset: 0x00057134
		[CLSCompliant(false)]
		public ulong GetUInt64(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(ulong))
			{
				return (ulong)element;
			}
			return this.m_converter.ToUInt64(element);
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x0005816C File Offset: 0x0005716C
		public float GetSingle(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(float))
			{
				return (float)element;
			}
			return this.m_converter.ToSingle(element);
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x000581A4 File Offset: 0x000571A4
		public double GetDouble(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(double))
			{
				return (double)element;
			}
			return this.m_converter.ToDouble(element);
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x000581DC File Offset: 0x000571DC
		public decimal GetDecimal(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(decimal))
			{
				return (decimal)element;
			}
			return this.m_converter.ToDecimal(element);
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x00058214 File Offset: 0x00057214
		public DateTime GetDateTime(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(DateTime))
			{
				return (DateTime)element;
			}
			return this.m_converter.ToDateTime(element);
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x0005824C File Offset: 0x0005724C
		public string GetString(string name)
		{
			Type type;
			object element = this.GetElement(name, out type);
			if (type == typeof(string) || element == null)
			{
				return (string)element;
			}
			return this.m_converter.ToString(element);
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x060022E3 RID: 8931 RVA: 0x00058286 File Offset: 0x00057286
		internal string[] MemberNames
		{
			get
			{
				return this.m_members;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x060022E4 RID: 8932 RVA: 0x0005828E File Offset: 0x0005728E
		internal object[] MemberValues
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x04000E99 RID: 3737
		private const int defaultSize = 4;

		// Token: 0x04000E9A RID: 3738
		private const string s_mscorlibAssemblySimpleName = "mscorlib";

		// Token: 0x04000E9B RID: 3739
		private const string s_mscorlibFileName = "mscorlib.dll";

		// Token: 0x04000E9C RID: 3740
		internal string[] m_members;

		// Token: 0x04000E9D RID: 3741
		internal object[] m_data;

		// Token: 0x04000E9E RID: 3742
		internal Type[] m_types;

		// Token: 0x04000E9F RID: 3743
		internal string m_fullTypeName;

		// Token: 0x04000EA0 RID: 3744
		internal int m_currMember;

		// Token: 0x04000EA1 RID: 3745
		internal string m_assemName;

		// Token: 0x04000EA2 RID: 3746
		private Type objectType;

		// Token: 0x04000EA3 RID: 3747
		internal IFormatterConverter m_converter;

		// Token: 0x04000EA4 RID: 3748
		private bool requireSameTokenInPartialTrust;
	}
}

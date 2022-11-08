using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System
{
	// Token: 0x020000E6 RID: 230
	[Serializable]
	internal class OleAutBinder : DefaultBinder
	{
		// Token: 0x06000C69 RID: 3177 RVA: 0x000253BC File Offset: 0x000243BC
		public override object ChangeType(object value, Type type, CultureInfo cultureInfo)
		{
			Variant source = new Variant(value);
			if (cultureInfo == null)
			{
				cultureInfo = CultureInfo.CurrentCulture;
			}
			if (type.IsByRef)
			{
				type = type.GetElementType();
			}
			if (!type.IsPrimitive && type.IsInstanceOfType(value))
			{
				return value;
			}
			Type type2 = value.GetType();
			if (type.IsEnum && type2.IsPrimitive)
			{
				return Enum.Parse(type, value.ToString());
			}
			if (type2 == typeof(DBNull))
			{
				if (type == typeof(DBNull))
				{
					return value;
				}
				if ((type.IsClass && type != typeof(object)) || type.IsInterface)
				{
					return null;
				}
			}
			object result;
			try
			{
				object obj = OAVariantLib.ChangeType(source, type, 16, cultureInfo).ToObject();
				result = obj;
			}
			catch (NotSupportedException)
			{
				throw new COMException(Environment.GetResourceString("Interop.COM_TypeMismatch"), -2147352571);
			}
			return result;
		}
	}
}

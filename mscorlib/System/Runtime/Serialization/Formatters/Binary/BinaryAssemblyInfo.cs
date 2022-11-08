using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007D7 RID: 2007
	internal sealed class BinaryAssemblyInfo
	{
		// Token: 0x06004729 RID: 18217 RVA: 0x000F3A27 File Offset: 0x000F2A27
		internal BinaryAssemblyInfo(string assemblyString)
		{
			this.assemblyString = assemblyString;
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x000F3A36 File Offset: 0x000F2A36
		internal BinaryAssemblyInfo(string assemblyString, Assembly assembly)
		{
			this.assemblyString = assemblyString;
			this.assembly = assembly;
		}

		// Token: 0x0600472B RID: 18219 RVA: 0x000F3A4C File Offset: 0x000F2A4C
		internal Assembly GetAssembly()
		{
			if (this.assembly == null)
			{
				this.assembly = FormatterServices.LoadAssemblyFromStringNoThrow(this.assemblyString);
				if (this.assembly == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_AssemblyNotFound"), new object[]
					{
						this.assemblyString
					}));
				}
			}
			return this.assembly;
		}

		// Token: 0x040023EE RID: 9198
		internal string assemblyString;

		// Token: 0x040023EF RID: 9199
		private Assembly assembly;
	}
}

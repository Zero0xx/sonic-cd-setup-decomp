using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Policy;

namespace System.Runtime.InteropServices
{
	// Token: 0x020002DD RID: 733
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[TypeLibImportClass(typeof(Assembly))]
	[ComVisible(true)]
	[Guid("17156360-2f1a-384a-bc52-fde93c215c5b")]
	[CLSCompliant(false)]
	public interface _Assembly
	{
		// Token: 0x06001C13 RID: 7187
		string ToString();

		// Token: 0x06001C14 RID: 7188
		bool Equals(object other);

		// Token: 0x06001C15 RID: 7189
		int GetHashCode();

		// Token: 0x06001C16 RID: 7190
		Type GetType();

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001C17 RID: 7191
		string CodeBase { get; }

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001C18 RID: 7192
		string EscapedCodeBase { get; }

		// Token: 0x06001C19 RID: 7193
		AssemblyName GetName();

		// Token: 0x06001C1A RID: 7194
		AssemblyName GetName(bool copiedName);

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001C1B RID: 7195
		string FullName { get; }

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001C1C RID: 7196
		MethodInfo EntryPoint { get; }

		// Token: 0x06001C1D RID: 7197
		Type GetType(string name);

		// Token: 0x06001C1E RID: 7198
		Type GetType(string name, bool throwOnError);

		// Token: 0x06001C1F RID: 7199
		Type[] GetExportedTypes();

		// Token: 0x06001C20 RID: 7200
		Type[] GetTypes();

		// Token: 0x06001C21 RID: 7201
		Stream GetManifestResourceStream(Type type, string name);

		// Token: 0x06001C22 RID: 7202
		Stream GetManifestResourceStream(string name);

		// Token: 0x06001C23 RID: 7203
		FileStream GetFile(string name);

		// Token: 0x06001C24 RID: 7204
		FileStream[] GetFiles();

		// Token: 0x06001C25 RID: 7205
		FileStream[] GetFiles(bool getResourceModules);

		// Token: 0x06001C26 RID: 7206
		string[] GetManifestResourceNames();

		// Token: 0x06001C27 RID: 7207
		ManifestResourceInfo GetManifestResourceInfo(string resourceName);

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001C28 RID: 7208
		string Location { get; }

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001C29 RID: 7209
		Evidence Evidence { get; }

		// Token: 0x06001C2A RID: 7210
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06001C2B RID: 7211
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06001C2C RID: 7212
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x06001C2D RID: 7213
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void GetObjectData(SerializationInfo info, StreamingContext context);

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06001C2E RID: 7214
		// (remove) Token: 0x06001C2F RID: 7215
		event ModuleResolveEventHandler ModuleResolve;

		// Token: 0x06001C30 RID: 7216
		Type GetType(string name, bool throwOnError, bool ignoreCase);

		// Token: 0x06001C31 RID: 7217
		Assembly GetSatelliteAssembly(CultureInfo culture);

		// Token: 0x06001C32 RID: 7218
		Assembly GetSatelliteAssembly(CultureInfo culture, Version version);

		// Token: 0x06001C33 RID: 7219
		Module LoadModule(string moduleName, byte[] rawModule);

		// Token: 0x06001C34 RID: 7220
		Module LoadModule(string moduleName, byte[] rawModule, byte[] rawSymbolStore);

		// Token: 0x06001C35 RID: 7221
		object CreateInstance(string typeName);

		// Token: 0x06001C36 RID: 7222
		object CreateInstance(string typeName, bool ignoreCase);

		// Token: 0x06001C37 RID: 7223
		object CreateInstance(string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes);

		// Token: 0x06001C38 RID: 7224
		Module[] GetLoadedModules();

		// Token: 0x06001C39 RID: 7225
		Module[] GetLoadedModules(bool getResourceModules);

		// Token: 0x06001C3A RID: 7226
		Module[] GetModules();

		// Token: 0x06001C3B RID: 7227
		Module[] GetModules(bool getResourceModules);

		// Token: 0x06001C3C RID: 7228
		Module GetModule(string name);

		// Token: 0x06001C3D RID: 7229
		AssemblyName[] GetReferencedAssemblies();

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001C3E RID: 7230
		bool GlobalAssemblyCache { get; }
	}
}

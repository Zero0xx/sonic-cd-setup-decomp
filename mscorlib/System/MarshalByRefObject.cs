using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Security.Permissions;
using System.Threading;

namespace System
{
	// Token: 0x02000053 RID: 83
	[ComVisible(true)]
	[Serializable]
	public abstract class MarshalByRefObject
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x00010C4C File Offset: 0x0000FC4C
		// (set) Token: 0x06000439 RID: 1081 RVA: 0x00010C54 File Offset: 0x0000FC54
		private object Identity
		{
			get
			{
				return this.__identity;
			}
			set
			{
				this.__identity = value;
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00010C60 File Offset: 0x0000FC60
		internal IntPtr GetComIUnknown(bool fIsBeingMarshalled)
		{
			IntPtr result;
			if (RemotingServices.IsTransparentProxy(this))
			{
				result = RemotingServices.GetRealProxy(this).GetCOMIUnknown(fIsBeingMarshalled);
			}
			else
			{
				result = Marshal.GetIUnknownForObject(this);
			}
			return result;
		}

		// Token: 0x0600043B RID: 1083
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr GetComIUnknown(MarshalByRefObject o);

		// Token: 0x0600043C RID: 1084 RVA: 0x00010C8C File Offset: 0x0000FC8C
		internal bool IsInstanceOfType(Type T)
		{
			return T.IsInstanceOfType(this);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00010C98 File Offset: 0x0000FC98
		internal object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			Type type = base.GetType();
			if (!type.IsCOMObject)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_InvokeMember"));
			}
			return type.InvokeMember(name, invokeAttr, binder, this, args, modifiers, culture, namedParameters);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00010CD8 File Offset: 0x0000FCD8
		protected MarshalByRefObject MemberwiseClone(bool cloneIdentity)
		{
			MarshalByRefObject marshalByRefObject = (MarshalByRefObject)base.MemberwiseClone();
			if (!cloneIdentity)
			{
				marshalByRefObject.Identity = null;
			}
			return marshalByRefObject;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00010CFC File Offset: 0x0000FCFC
		internal static Identity GetIdentity(MarshalByRefObject obj, out bool fServer)
		{
			fServer = true;
			Identity result = null;
			if (obj != null)
			{
				if (!RemotingServices.IsTransparentProxy(obj))
				{
					result = (Identity)obj.Identity;
				}
				else
				{
					fServer = false;
					result = RemotingServices.GetRealProxy(obj).IdentityObject;
				}
			}
			return result;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00010D38 File Offset: 0x0000FD38
		internal static Identity GetIdentity(MarshalByRefObject obj)
		{
			bool flag;
			return MarshalByRefObject.GetIdentity(obj, out flag);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00010D4D File Offset: 0x0000FD4D
		internal ServerIdentity __RaceSetServerIdentity(ServerIdentity id)
		{
			if (this.__identity == null)
			{
				if (!id.IsContextBound)
				{
					id.RaceSetTransparentProxy(this);
				}
				Interlocked.CompareExchange(ref this.__identity, id, null);
			}
			return (ServerIdentity)this.__identity;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00010D80 File Offset: 0x0000FD80
		internal void __ResetServerIdentity()
		{
			this.__identity = null;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00010D89 File Offset: 0x0000FD89
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public object GetLifetimeService()
		{
			return LifetimeServices.GetLease(this);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00010D91 File Offset: 0x0000FD91
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual object InitializeLifetimeService()
		{
			return LifetimeServices.GetLeaseInitial(this);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00010D99 File Offset: 0x0000FD99
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual ObjRef CreateObjRef(Type requestedType)
		{
			if (this.__identity == null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_NoIdentityEntry"));
			}
			return new ObjRef(this, requestedType);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00010DBC File Offset: 0x0000FDBC
		internal bool CanCastToXmlType(string xmlTypeName, string xmlTypeNamespace)
		{
			Type type = SoapServices.GetInteropTypeFromXmlType(xmlTypeName, xmlTypeNamespace);
			if (type == null)
			{
				string text;
				string assemblyString;
				if (!SoapServices.DecodeXmlNamespaceForClrTypeNamespace(xmlTypeNamespace, out text, out assemblyString))
				{
					return false;
				}
				string name;
				if (text != null && text.Length > 0)
				{
					name = text + "." + xmlTypeName;
				}
				else
				{
					name = xmlTypeName;
				}
				try
				{
					Assembly assembly = Assembly.Load(assemblyString);
					type = assembly.GetType(name, false, false);
				}
				catch
				{
					return false;
				}
			}
			return type != null && type.IsAssignableFrom(base.GetType());
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00010E40 File Offset: 0x0000FE40
		internal static bool CanCastToXmlTypeHelper(Type castType, MarshalByRefObject o)
		{
			if (castType == null)
			{
				throw new ArgumentNullException("castType");
			}
			if (!castType.IsInterface && !castType.IsMarshalByRef)
			{
				return false;
			}
			string xmlTypeName = null;
			string xmlTypeNamespace = null;
			if (!SoapServices.GetXmlTypeForInteropType(castType, out xmlTypeName, out xmlTypeNamespace))
			{
				xmlTypeName = castType.Name;
				xmlTypeNamespace = SoapServices.CodeXmlNamespaceForClrTypeNamespace(castType.Namespace, castType.Module.Assembly.nGetSimpleName());
			}
			return o.CanCastToXmlType(xmlTypeName, xmlTypeNamespace);
		}

		// Token: 0x04000194 RID: 404
		private object __identity;
	}
}

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Services
{
	// Token: 0x020007A8 RID: 1960
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class TrackingServices
	{
		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x060045AE RID: 17838 RVA: 0x000ECF9C File Offset: 0x000EBF9C
		private static object TrackingServicesSyncObject
		{
			get
			{
				if (TrackingServices.s_TrackingServicesSyncObject == null)
				{
					object value = new object();
					Interlocked.CompareExchange(ref TrackingServices.s_TrackingServicesSyncObject, value, null);
				}
				return TrackingServices.s_TrackingServicesSyncObject;
			}
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x000ECFC8 File Offset: 0x000EBFC8
		public static void RegisterTrackingHandler(ITrackingHandler handler)
		{
			lock (TrackingServices.TrackingServicesSyncObject)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				if (-1 != TrackingServices.Match(handler))
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_TrackingHandlerAlreadyRegistered"), new object[]
					{
						"handler"
					}));
				}
				if (TrackingServices._Handlers == null || TrackingServices._Size == TrackingServices._Handlers.Length)
				{
					ITrackingHandler[] array = new ITrackingHandler[TrackingServices._Size * 2 + 4];
					if (TrackingServices._Handlers != null)
					{
						Array.Copy(TrackingServices._Handlers, array, TrackingServices._Size);
					}
					TrackingServices._Handlers = array;
				}
				TrackingServices._Handlers[TrackingServices._Size++] = handler;
			}
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x000ED094 File Offset: 0x000EC094
		public static void UnregisterTrackingHandler(ITrackingHandler handler)
		{
			lock (TrackingServices.TrackingServicesSyncObject)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				int num = TrackingServices.Match(handler);
				if (-1 == num)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_HandlerNotRegistered"), new object[]
					{
						handler
					}));
				}
				Array.Copy(TrackingServices._Handlers, num + 1, TrackingServices._Handlers, num, TrackingServices._Size - num - 1);
				TrackingServices._Size--;
			}
		}

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x060045B1 RID: 17841 RVA: 0x000ED130 File Offset: 0x000EC130
		public static ITrackingHandler[] RegisteredHandlers
		{
			get
			{
				ITrackingHandler[] result;
				lock (TrackingServices.TrackingServicesSyncObject)
				{
					if (TrackingServices._Size == 0)
					{
						result = new ITrackingHandler[0];
					}
					else
					{
						ITrackingHandler[] array = new ITrackingHandler[TrackingServices._Size];
						for (int i = 0; i < TrackingServices._Size; i++)
						{
							array[i] = TrackingServices._Handlers[i];
						}
						result = array;
					}
				}
				return result;
			}
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x000ED19C File Offset: 0x000EC19C
		internal static void MarshaledObject(object obj, ObjRef or)
		{
			try
			{
				ITrackingHandler[] handlers = TrackingServices._Handlers;
				for (int i = 0; i < TrackingServices._Size; i++)
				{
					handlers[i].MarshaledObject(obj, or);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x000ED1E0 File Offset: 0x000EC1E0
		internal static void UnmarshaledObject(object obj, ObjRef or)
		{
			try
			{
				ITrackingHandler[] handlers = TrackingServices._Handlers;
				for (int i = 0; i < TrackingServices._Size; i++)
				{
					handlers[i].UnmarshaledObject(obj, or);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x000ED224 File Offset: 0x000EC224
		internal static void DisconnectedObject(object obj)
		{
			try
			{
				ITrackingHandler[] handlers = TrackingServices._Handlers;
				for (int i = 0; i < TrackingServices._Size; i++)
				{
					handlers[i].DisconnectedObject(obj);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x000ED268 File Offset: 0x000EC268
		private static int Match(ITrackingHandler handler)
		{
			int result = -1;
			for (int i = 0; i < TrackingServices._Size; i++)
			{
				if (TrackingServices._Handlers[i] == handler)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		// Token: 0x040022A1 RID: 8865
		private static ITrackingHandler[] _Handlers = new ITrackingHandler[0];

		// Token: 0x040022A2 RID: 8866
		private static int _Size = 0;

		// Token: 0x040022A3 RID: 8867
		private static object s_TrackingServicesSyncObject = null;
	}
}

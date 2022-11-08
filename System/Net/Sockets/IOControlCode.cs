using System;

namespace System.Net.Sockets
{
	// Token: 0x020005A8 RID: 1448
	public enum IOControlCode : long
	{
		// Token: 0x04002AA0 RID: 10912
		AsyncIO = 2147772029L,
		// Token: 0x04002AA1 RID: 10913
		NonBlockingIO,
		// Token: 0x04002AA2 RID: 10914
		DataToRead = 1074030207L,
		// Token: 0x04002AA3 RID: 10915
		OobDataRead = 1074033415L,
		// Token: 0x04002AA4 RID: 10916
		AssociateHandle = 2281701377L,
		// Token: 0x04002AA5 RID: 10917
		EnableCircularQueuing = 671088642L,
		// Token: 0x04002AA6 RID: 10918
		Flush = 671088644L,
		// Token: 0x04002AA7 RID: 10919
		GetBroadcastAddress = 1207959557L,
		// Token: 0x04002AA8 RID: 10920
		GetExtensionFunctionPointer = 3355443206L,
		// Token: 0x04002AA9 RID: 10921
		GetQos,
		// Token: 0x04002AAA RID: 10922
		GetGroupQos,
		// Token: 0x04002AAB RID: 10923
		MultipointLoopback = 2281701385L,
		// Token: 0x04002AAC RID: 10924
		MulticastScope,
		// Token: 0x04002AAD RID: 10925
		SetQos,
		// Token: 0x04002AAE RID: 10926
		SetGroupQos,
		// Token: 0x04002AAF RID: 10927
		TranslateHandle = 3355443213L,
		// Token: 0x04002AB0 RID: 10928
		RoutingInterfaceQuery = 3355443220L,
		// Token: 0x04002AB1 RID: 10929
		RoutingInterfaceChange = 2281701397L,
		// Token: 0x04002AB2 RID: 10930
		AddressListQuery = 1207959574L,
		// Token: 0x04002AB3 RID: 10931
		AddressListChange = 671088663L,
		// Token: 0x04002AB4 RID: 10932
		QueryTargetPnpHandle = 1207959576L,
		// Token: 0x04002AB5 RID: 10933
		NamespaceChange = 2281701401L,
		// Token: 0x04002AB6 RID: 10934
		AddressListSort = 3355443225L,
		// Token: 0x04002AB7 RID: 10935
		ReceiveAll = 2550136833L,
		// Token: 0x04002AB8 RID: 10936
		ReceiveAllMulticast,
		// Token: 0x04002AB9 RID: 10937
		ReceiveAllIgmpMulticast,
		// Token: 0x04002ABA RID: 10938
		KeepAliveValues,
		// Token: 0x04002ABB RID: 10939
		AbsorbRouterAlert,
		// Token: 0x04002ABC RID: 10940
		UnicastInterface,
		// Token: 0x04002ABD RID: 10941
		LimitBroadcasts,
		// Token: 0x04002ABE RID: 10942
		BindToInterface,
		// Token: 0x04002ABF RID: 10943
		MulticastInterface,
		// Token: 0x04002AC0 RID: 10944
		AddMulticastGroupOnInterface,
		// Token: 0x04002AC1 RID: 10945
		DeleteMulticastGroupFromInterface
	}
}

using System;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000005 RID: 5
	public sealed class InterfaceGuid
	{
		// Token: 0x06000017 RID: 23 RVA: 0x0005629C File Offset: 0x0005569C
		private InterfaceGuid()
		{
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000562B4 File Offset: 0x000556B4
		static InterfaceGuid()
		{
			Guid iid_IDirect3DDevice = <Module>.IID_IDirect3DDevice9;
			InterfaceGuid.Device = iid_IDirect3DDevice;
			Guid iid_IDirect3DResource = <Module>.IID_IDirect3DResource9;
			InterfaceGuid.Resource = iid_IDirect3DResource;
			Guid iid_IDirect3DBaseTexture = <Module>.IID_IDirect3DBaseTexture9;
			InterfaceGuid.BaseTexture = iid_IDirect3DBaseTexture;
			Guid iid_IDirect3DCubeTexture = <Module>.IID_IDirect3DCubeTexture9;
			InterfaceGuid.CubeTexture = iid_IDirect3DCubeTexture;
			Guid iid_IDirect3DVolumeTexture = <Module>.IID_IDirect3DVolumeTexture9;
			InterfaceGuid.VolumeTexture = iid_IDirect3DVolumeTexture;
			Guid iid_IDirect3DVertexBuffer = <Module>.IID_IDirect3DVertexBuffer9;
			InterfaceGuid.VertexBuffer = iid_IDirect3DVertexBuffer;
			Guid iid_IDirect3DIndexBuffer = <Module>.IID_IDirect3DIndexBuffer9;
			InterfaceGuid.IndexBuffer = iid_IDirect3DIndexBuffer;
			Guid iid_IDirect3DSurface = <Module>.IID_IDirect3DSurface9;
			InterfaceGuid.Surface = iid_IDirect3DSurface;
			Guid iid_IDirect3DVolume = <Module>.IID_IDirect3DVolume9;
			InterfaceGuid.Volume = iid_IDirect3DVolume;
			Guid iid_IDirect3DSwapChain = <Module>.IID_IDirect3DSwapChain9;
			InterfaceGuid.SwapChain = iid_IDirect3DSwapChain;
			Guid iid_IDirect3DVertexDeclaration = <Module>.IID_IDirect3DVertexDeclaration9;
			InterfaceGuid.VertexDeclaration = iid_IDirect3DVertexDeclaration;
			Guid iid_IDirect3DVertexShader = <Module>.IID_IDirect3DVertexShader9;
			InterfaceGuid.VertexShader = iid_IDirect3DVertexShader;
			Guid iid_IDirect3DPixelShader = <Module>.IID_IDirect3DPixelShader9;
			InterfaceGuid.PixelShader = iid_IDirect3DPixelShader;
			Guid iid_IDirect3DStateBlock = <Module>.IID_IDirect3DStateBlock9;
			InterfaceGuid.StateBlock = iid_IDirect3DStateBlock;
		}

		// Token: 0x04000C50 RID: 3152
		public static readonly Guid Device;

		// Token: 0x04000C51 RID: 3153
		public static readonly Guid Resource;

		// Token: 0x04000C52 RID: 3154
		public static readonly Guid BaseTexture;

		// Token: 0x04000C53 RID: 3155
		public static readonly Guid CubeTexture;

		// Token: 0x04000C54 RID: 3156
		public static readonly Guid VolumeTexture;

		// Token: 0x04000C55 RID: 3157
		public static readonly Guid VertexBuffer;

		// Token: 0x04000C56 RID: 3158
		public static readonly Guid IndexBuffer;

		// Token: 0x04000C57 RID: 3159
		public static readonly Guid Surface;

		// Token: 0x04000C58 RID: 3160
		public static readonly Guid Volume;

		// Token: 0x04000C59 RID: 3161
		public static readonly Guid SwapChain;

		// Token: 0x04000C5A RID: 3162
		public static readonly Guid VertexDeclaration;

		// Token: 0x04000C5B RID: 3163
		public static readonly Guid VertexShader;

		// Token: 0x04000C5C RID: 3164
		public static readonly Guid PixelShader;

		// Token: 0x04000C5D RID: 3165
		public static readonly Guid StateBlock;
	}
}

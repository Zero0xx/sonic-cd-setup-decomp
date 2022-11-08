using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000006 RID: 6
	[Serializable]
	public class GraphicsException : DirectXException
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00073434 File Offset: 0x00072834
		internal GraphicsException(int resultCode)
		{
			base.HResult = resultCode;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0005641C File Offset: 0x0005581C
		protected GraphicsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00056400 File Offset: 0x00055800
		public GraphicsException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000563E4 File Offset: 0x000557E4
		public GraphicsException(string message) : base(message)
		{
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000563CC File Offset: 0x000557CC
		public GraphicsException()
		{
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00073454 File Offset: 0x00072854
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new static Exception GetExceptionFromResultInternal(int resultCode)
		{
			if (resultCode <= -2005530591)
			{
				if (resultCode == -2005530591)
				{
					return new ConflictingRenderStateException();
				}
				if (resultCode <= -2005530598)
				{
					if (resultCode == -2005530598)
					{
						return new UnsupportedColorArgumentException();
					}
					if (resultCode == -2147024882)
					{
						return new OutOfMemoryException();
					}
					if (resultCode == -2147024809)
					{
						return new ArgumentException();
					}
					if (resultCode == -2005532292)
					{
						return new OutOfVideoMemoryException();
					}
					if (resultCode == -2005532132)
					{
						return new WasStillDrawingException();
					}
					if (resultCode == -2005530600)
					{
						return new WrongTextureFormatException();
					}
					if (resultCode == -2005530599)
					{
						return new UnsupportedColorOperationException();
					}
				}
				else
				{
					if (resultCode == -2005530597)
					{
						return new UnsupportedAlphaOperationException();
					}
					if (resultCode == -2005530596)
					{
						return new UnsupportedAlphaArgumentException();
					}
					if (resultCode == -2005530595)
					{
						return new TooManyOperationsException();
					}
					if (resultCode == -2005530594)
					{
						return new ConflictingTextureFilterException();
					}
					if (resultCode == -2005530593)
					{
						return new UnsupportedFactorValueException();
					}
				}
			}
			else if (resultCode <= -2005530520)
			{
				if (resultCode == -2005530520)
				{
					return new DeviceLostException();
				}
				if (resultCode == -2005530590)
				{
					return new UnsupportedTextureFilterException();
				}
				if (resultCode == -2005530586)
				{
					return new ConflictingTexturePaletteException();
				}
				if (resultCode == -2005530585)
				{
					return new DriverInternalErrorException();
				}
				if (resultCode == -2005530522)
				{
					return new NotFoundException();
				}
				if (resultCode == -2005530521)
				{
					return new MoreDataException();
				}
			}
			else
			{
				if (resultCode == -2005530519)
				{
					return new DeviceNotResetException();
				}
				if (resultCode == -2005530518)
				{
					return new NotAvailableException();
				}
				if (resultCode == -2005530517)
				{
					return new InvalidDeviceException();
				}
				if (resultCode == -2005530516)
				{
					return new InvalidCallException();
				}
				if (resultCode == -2005530515)
				{
					return new DriverInvalidCallException();
				}
			}
			return new GraphicsException(resultCode);
		}
	}
}

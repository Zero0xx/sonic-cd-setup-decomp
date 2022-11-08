using System;
using System.Globalization;
using System.IO;

namespace System.Drawing
{
	// Token: 0x02000068 RID: 104
	[AttributeUsage(AttributeTargets.Class)]
	public class ToolboxBitmapAttribute : Attribute
	{
		// Token: 0x060006B6 RID: 1718 RVA: 0x0001AA94 File Offset: 0x00019A94
		public ToolboxBitmapAttribute(string imageFile) : this(ToolboxBitmapAttribute.GetImageFromFile(imageFile, false), ToolboxBitmapAttribute.GetImageFromFile(imageFile, true))
		{
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001AAAA File Offset: 0x00019AAA
		public ToolboxBitmapAttribute(Type t) : this(ToolboxBitmapAttribute.GetImageFromResource(t, null, false), ToolboxBitmapAttribute.GetImageFromResource(t, null, true))
		{
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001AAC2 File Offset: 0x00019AC2
		public ToolboxBitmapAttribute(Type t, string name) : this(ToolboxBitmapAttribute.GetImageFromResource(t, name, false), ToolboxBitmapAttribute.GetImageFromResource(t, name, true))
		{
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001AADA File Offset: 0x00019ADA
		private ToolboxBitmapAttribute(Image smallImage, Image largeImage)
		{
			this.smallImage = smallImage;
			this.largeImage = largeImage;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001AAF0 File Offset: 0x00019AF0
		public override bool Equals(object value)
		{
			if (value == this)
			{
				return true;
			}
			ToolboxBitmapAttribute toolboxBitmapAttribute = value as ToolboxBitmapAttribute;
			return toolboxBitmapAttribute != null && toolboxBitmapAttribute.smallImage == this.smallImage && toolboxBitmapAttribute.largeImage == this.largeImage;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0001AB2D File Offset: 0x00019B2D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0001AB35 File Offset: 0x00019B35
		public Image GetImage(object component)
		{
			return this.GetImage(component, true);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0001AB3F File Offset: 0x00019B3F
		public Image GetImage(object component, bool large)
		{
			if (component != null)
			{
				return this.GetImage(component.GetType(), large);
			}
			return null;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0001AB53 File Offset: 0x00019B53
		public Image GetImage(Type type)
		{
			return this.GetImage(type, false);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0001AB5D File Offset: 0x00019B5D
		public Image GetImage(Type type, bool large)
		{
			return this.GetImage(type, null, large);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001AB68 File Offset: 0x00019B68
		public Image GetImage(Type type, string imgName, bool large)
		{
			if ((large && this.largeImage == null) || (!large && this.smallImage == null))
			{
				Point point = new Point(32, 32);
				Image image;
				if (large)
				{
					image = this.largeImage;
				}
				else
				{
					image = this.smallImage;
				}
				if (image == null)
				{
					image = ToolboxBitmapAttribute.GetImageFromResource(type, imgName, large);
				}
				if (large && this.largeImage == null && this.smallImage != null)
				{
					image = new Bitmap((Bitmap)this.smallImage, point.X, point.Y);
				}
				Bitmap bitmap = image as Bitmap;
				if (bitmap != null)
				{
					ToolboxBitmapAttribute.MakeBackgroundAlphaZero(bitmap);
				}
				if (image == null)
				{
					image = ToolboxBitmapAttribute.DefaultComponent.GetImage(type, large);
				}
				if (large)
				{
					this.largeImage = image;
				}
				else
				{
					this.smallImage = image;
				}
			}
			Image result = large ? this.largeImage : this.smallImage;
			if (this.Equals(ToolboxBitmapAttribute.Default))
			{
				this.largeImage = null;
				this.smallImage = null;
			}
			return result;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0001AC50 File Offset: 0x00019C50
		private static Image GetIconFromStream(Stream stream, bool large)
		{
			if (stream == null)
			{
				return null;
			}
			Icon original = new Icon(stream);
			Icon icon = new Icon(original, large ? new Size(ToolboxBitmapAttribute.largeDim.X, ToolboxBitmapAttribute.largeDim.Y) : new Size(ToolboxBitmapAttribute.smallDim.X, ToolboxBitmapAttribute.smallDim.Y));
			return icon.ToBitmap();
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001ACBC File Offset: 0x00019CBC
		private static Image GetImageFromFile(string imageFile, bool large)
		{
			Image result = null;
			try
			{
				if (imageFile != null)
				{
					string extension = Path.GetExtension(imageFile);
					if (extension != null && string.Equals(extension, ".ico", StringComparison.OrdinalIgnoreCase))
					{
						FileStream fileStream = File.Open(imageFile, FileMode.Open);
						if (fileStream == null)
						{
							goto IL_43;
						}
						try
						{
							result = ToolboxBitmapAttribute.GetIconFromStream(fileStream, large);
							goto IL_43;
						}
						finally
						{
							fileStream.Close();
						}
					}
					if (!large)
					{
						result = Image.FromFile(imageFile);
					}
				}
				IL_43:;
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsCriticalException(ex))
				{
					throw;
				}
			}
			return result;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0001AD38 File Offset: 0x00019D38
		private static Image GetBitmapFromResource(Type t, string bitmapname, bool large)
		{
			if (bitmapname == null)
			{
				return null;
			}
			Image result = null;
			Stream manifestResourceStream = t.Module.Assembly.GetManifestResourceStream(t, bitmapname);
			if (manifestResourceStream != null)
			{
				Bitmap bitmap = new Bitmap(manifestResourceStream);
				result = bitmap;
				ToolboxBitmapAttribute.MakeBackgroundAlphaZero(bitmap);
				if (large)
				{
					result = new Bitmap(bitmap, ToolboxBitmapAttribute.largeDim.X, ToolboxBitmapAttribute.largeDim.Y);
				}
			}
			return result;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001AD97 File Offset: 0x00019D97
		private static Image GetIconFromResource(Type t, string bitmapname, bool large)
		{
			if (bitmapname == null)
			{
				return null;
			}
			return ToolboxBitmapAttribute.GetIconFromStream(t.Module.Assembly.GetManifestResourceStream(t, bitmapname), large);
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0001ADB8 File Offset: 0x00019DB8
		public static Image GetImageFromResource(Type t, string imageName, bool large)
		{
			Image image = null;
			try
			{
				string bitmapname = null;
				string bitmapname2 = null;
				string bitmapname3 = null;
				if (imageName == null)
				{
					string text = t.FullName;
					int num = text.LastIndexOf('.');
					if (num != -1)
					{
						text = text.Substring(num + 1);
					}
					bitmapname = text + ".ico";
					bitmapname2 = text + ".bmp";
				}
				else if (string.Compare(Path.GetExtension(imageName), ".ico", true, CultureInfo.CurrentCulture) == 0)
				{
					bitmapname = imageName;
				}
				else if (string.Compare(Path.GetExtension(imageName), ".bmp", true, CultureInfo.CurrentCulture) == 0)
				{
					bitmapname2 = imageName;
				}
				else
				{
					bitmapname3 = imageName;
					bitmapname2 = imageName + ".bmp";
					bitmapname = imageName + ".ico";
				}
				image = ToolboxBitmapAttribute.GetBitmapFromResource(t, bitmapname3, large);
				if (image == null)
				{
					image = ToolboxBitmapAttribute.GetBitmapFromResource(t, bitmapname2, large);
				}
				if (image == null)
				{
					image = ToolboxBitmapAttribute.GetIconFromResource(t, bitmapname, large);
				}
			}
			catch (Exception)
			{
			}
			return image;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001AE9C File Offset: 0x00019E9C
		private static void MakeBackgroundAlphaZero(Bitmap img)
		{
			Color pixel = img.GetPixel(0, img.Height - 1);
			img.MakeTransparent();
			Color color = Color.FromArgb(0, pixel);
			img.SetPixel(0, img.Height - 1, color);
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0001AED8 File Offset: 0x00019ED8
		static ToolboxBitmapAttribute()
		{
			Bitmap img = null;
			Stream manifestResourceStream = typeof(ToolboxBitmapAttribute).Module.Assembly.GetManifestResourceStream(typeof(ToolboxBitmapAttribute), "DefaultComponent.bmp");
			if (manifestResourceStream != null)
			{
				img = new Bitmap(manifestResourceStream);
				ToolboxBitmapAttribute.MakeBackgroundAlphaZero(img);
			}
			ToolboxBitmapAttribute.DefaultComponent = new ToolboxBitmapAttribute(img, null);
		}

		// Token: 0x0400048C RID: 1164
		private Image smallImage;

		// Token: 0x0400048D RID: 1165
		private Image largeImage;

		// Token: 0x0400048E RID: 1166
		private static readonly Point largeDim = new Point(32, 32);

		// Token: 0x0400048F RID: 1167
		private static readonly Point smallDim = new Point(16, 16);

		// Token: 0x04000490 RID: 1168
		public static readonly ToolboxBitmapAttribute Default = new ToolboxBitmapAttribute(null, null);

		// Token: 0x04000491 RID: 1169
		private static readonly ToolboxBitmapAttribute DefaultComponent;
	}
}

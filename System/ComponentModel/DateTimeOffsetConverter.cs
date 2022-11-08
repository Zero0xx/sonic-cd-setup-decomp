using System;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000C9 RID: 201
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class DateTimeOffsetConverter : TypeConverter
	{
		// Token: 0x060006DF RID: 1759 RVA: 0x00019DAE File Offset: 0x00018DAE
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00019DC7 File Offset: 0x00018DC7
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00019DE0 File Offset: 0x00018DE0
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				if (text.Length == 0)
				{
					return DateTimeOffset.MinValue;
				}
				try
				{
					DateTimeFormatInfo dateTimeFormatInfo = null;
					if (culture != null)
					{
						dateTimeFormatInfo = (DateTimeFormatInfo)culture.GetFormat(typeof(DateTimeFormatInfo));
					}
					if (dateTimeFormatInfo != null)
					{
						return DateTimeOffset.Parse(text, dateTimeFormatInfo);
					}
					return DateTimeOffset.Parse(text, culture);
				}
				catch (FormatException innerException)
				{
					throw new FormatException(SR.GetString("ConvertInvalidPrimitive", new object[]
					{
						(string)value,
						"DateTimeOffset"
					}), innerException);
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00019EA0 File Offset: 0x00018EA0
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType != typeof(string) || !(value is DateTimeOffset))
			{
				if (destinationType == typeof(InstanceDescriptor) && value is DateTimeOffset)
				{
					DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
					if (dateTimeOffset.Ticks == 0L)
					{
						ConstructorInfo constructor = typeof(DateTimeOffset).GetConstructor(new Type[]
						{
							typeof(long)
						});
						if (constructor != null)
						{
							return new InstanceDescriptor(constructor, new object[]
							{
								dateTimeOffset.Ticks
							});
						}
					}
					ConstructorInfo constructor2 = typeof(DateTimeOffset).GetConstructor(new Type[]
					{
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(TimeSpan)
					});
					if (constructor2 != null)
					{
						return new InstanceDescriptor(constructor2, new object[]
						{
							dateTimeOffset.Year,
							dateTimeOffset.Month,
							dateTimeOffset.Day,
							dateTimeOffset.Hour,
							dateTimeOffset.Minute,
							dateTimeOffset.Second,
							dateTimeOffset.Millisecond,
							dateTimeOffset.Offset
						});
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
			DateTimeOffset left = (DateTimeOffset)value;
			if (left == DateTimeOffset.MinValue)
			{
				return string.Empty;
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			DateTimeFormatInfo dateTimeFormatInfo = (DateTimeFormatInfo)culture.GetFormat(typeof(DateTimeFormatInfo));
			if (culture != CultureInfo.InvariantCulture)
			{
				string format;
				if (left.TimeOfDay.TotalSeconds == 0.0)
				{
					format = dateTimeFormatInfo.ShortDatePattern + " zzz";
				}
				else
				{
					format = dateTimeFormatInfo.ShortDatePattern + " " + dateTimeFormatInfo.ShortTimePattern + " zzz";
				}
				return left.ToString(format, CultureInfo.CurrentCulture);
			}
			if (left.TimeOfDay.TotalSeconds == 0.0)
			{
				return left.ToString("yyyy-MM-dd zzz", culture);
			}
			return left.ToString(culture);
		}
	}
}

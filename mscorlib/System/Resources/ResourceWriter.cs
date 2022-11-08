using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace System.Resources
{
	// Token: 0x0200043C RID: 1084
	[ComVisible(true)]
	public sealed class ResourceWriter : IResourceWriter, IDisposable
	{
		// Token: 0x06002C46 RID: 11334 RVA: 0x00095E4C File Offset: 0x00094E4C
		public ResourceWriter(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			this._output = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
			this._resourceList = new Hashtable(1000, FastResourceComparer.Default);
			this._caseInsensitiveDups = new Hashtable(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x00095EA4 File Offset: 0x00094EA4
		public ResourceWriter(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanWrite)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_StreamNotWritable"));
			}
			this._output = stream;
			this._resourceList = new Hashtable(1000, FastResourceComparer.Default);
			this._caseInsensitiveDups = new Hashtable(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x00095F0C File Offset: 0x00094F0C
		public void AddResource(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this._resourceList == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceWriterSaved"));
			}
			this._caseInsensitiveDups.Add(name, null);
			this._resourceList.Add(name, value);
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x00095F5C File Offset: 0x00094F5C
		public void AddResource(string name, object value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this._resourceList == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceWriterSaved"));
			}
			this._caseInsensitiveDups.Add(name, null);
			this._resourceList.Add(name, value);
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x00095FAC File Offset: 0x00094FAC
		public void AddResource(string name, byte[] value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this._resourceList == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceWriterSaved"));
			}
			this._caseInsensitiveDups.Add(name, null);
			this._resourceList.Add(name, value);
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x00095FFC File Offset: 0x00094FFC
		public void AddResourceData(string name, string typeName, byte[] serializedData)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (serializedData == null)
			{
				throw new ArgumentNullException("serializedData");
			}
			if (this._resourceList == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceWriterSaved"));
			}
			this._caseInsensitiveDups.Add(name, null);
			if (this._preserializedData == null)
			{
				this._preserializedData = new Hashtable(FastResourceComparer.Default);
			}
			this._preserializedData.Add(name, new ResourceWriter.PrecannedResource(typeName, serializedData));
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x00096083 File Offset: 0x00095083
		public void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x0009608C File Offset: 0x0009508C
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._resourceList != null)
				{
					this.Generate();
				}
				if (this._output != null)
				{
					this._output.Close();
				}
			}
			this._output = null;
			this._caseInsensitiveDups = null;
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x000960C0 File Offset: 0x000950C0
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x000960CC File Offset: 0x000950CC
		public void Generate()
		{
			if (this._resourceList == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceWriterSaved"));
			}
			BinaryWriter binaryWriter = new BinaryWriter(this._output, Encoding.UTF8);
			List<string> list = new List<string>();
			binaryWriter.Write(ResourceManager.MagicNumber);
			binaryWriter.Write(ResourceManager.HeaderVersionNumber);
			MemoryStream memoryStream = new MemoryStream(240);
			BinaryWriter binaryWriter2 = new BinaryWriter(memoryStream);
			binaryWriter2.Write(typeof(ResourceReader).AssemblyQualifiedName);
			binaryWriter2.Write(ResourceManager.ResSetTypeName);
			binaryWriter2.Flush();
			binaryWriter.Write((int)memoryStream.Length);
			binaryWriter.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			binaryWriter.Write(2);
			int num = this._resourceList.Count;
			if (this._preserializedData != null)
			{
				num += this._preserializedData.Count;
			}
			binaryWriter.Write(num);
			int[] array = new int[num];
			int[] array2 = new int[num];
			int num2 = 0;
			MemoryStream memoryStream2 = new MemoryStream(num * 40);
			BinaryWriter binaryWriter3 = new BinaryWriter(memoryStream2, Encoding.Unicode);
			MemoryStream memoryStream3 = new MemoryStream(num * 40);
			BinaryWriter binaryWriter4 = new BinaryWriter(memoryStream3, Encoding.UTF8);
			IFormatter objFormatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.File | StreamingContextStates.Persistence));
			SortedList sortedList = new SortedList(this._resourceList, FastResourceComparer.Default);
			if (this._preserializedData != null)
			{
				foreach (object obj in this._preserializedData)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					sortedList.Add(dictionaryEntry.Key, dictionaryEntry.Value);
				}
			}
			IDictionaryEnumerator enumerator2 = sortedList.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				array[num2] = FastResourceComparer.HashFunction((string)enumerator2.Key);
				array2[num2++] = (int)binaryWriter3.Seek(0, SeekOrigin.Current);
				binaryWriter3.Write((string)enumerator2.Key);
				binaryWriter3.Write((int)binaryWriter4.Seek(0, SeekOrigin.Current));
				object value = enumerator2.Value;
				ResourceTypeCode resourceTypeCode = this.FindTypeCode(value, list);
				ResourceWriter.Write7BitEncodedInt(binaryWriter4, (int)resourceTypeCode);
				ResourceWriter.PrecannedResource precannedResource = value as ResourceWriter.PrecannedResource;
				if (precannedResource != null)
				{
					binaryWriter4.Write(precannedResource.Data);
				}
				else
				{
					this.WriteValue(resourceTypeCode, value, binaryWriter4, objFormatter);
				}
			}
			binaryWriter.Write(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				binaryWriter.Write(list[i]);
			}
			Array.Sort<int, int>(array, array2);
			binaryWriter.Flush();
			int num3 = (int)binaryWriter.BaseStream.Position & 7;
			if (num3 > 0)
			{
				for (int j = 0; j < 8 - num3; j++)
				{
					binaryWriter.Write("PAD"[j % 3]);
				}
			}
			foreach (int value2 in array)
			{
				binaryWriter.Write(value2);
			}
			foreach (int value3 in array2)
			{
				binaryWriter.Write(value3);
			}
			binaryWriter.Flush();
			binaryWriter3.Flush();
			binaryWriter4.Flush();
			int num4 = (int)(binaryWriter.Seek(0, SeekOrigin.Current) + memoryStream2.Length);
			num4 += 4;
			binaryWriter.Write(num4);
			binaryWriter.Write(memoryStream2.GetBuffer(), 0, (int)memoryStream2.Length);
			binaryWriter3.Close();
			binaryWriter.Write(memoryStream3.GetBuffer(), 0, (int)memoryStream3.Length);
			binaryWriter4.Close();
			binaryWriter.Flush();
			this._resourceList = null;
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x00096474 File Offset: 0x00095474
		private ResourceTypeCode FindTypeCode(object value, List<string> types)
		{
			if (value == null)
			{
				return ResourceTypeCode.Null;
			}
			Type type = value.GetType();
			if (type == typeof(string))
			{
				return ResourceTypeCode.String;
			}
			if (type == typeof(int))
			{
				return ResourceTypeCode.Int32;
			}
			if (type == typeof(bool))
			{
				return ResourceTypeCode.Boolean;
			}
			if (type == typeof(char))
			{
				return ResourceTypeCode.Char;
			}
			if (type == typeof(byte))
			{
				return ResourceTypeCode.Byte;
			}
			if (type == typeof(sbyte))
			{
				return ResourceTypeCode.SByte;
			}
			if (type == typeof(short))
			{
				return ResourceTypeCode.Int16;
			}
			if (type == typeof(long))
			{
				return ResourceTypeCode.Int64;
			}
			if (type == typeof(ushort))
			{
				return ResourceTypeCode.UInt16;
			}
			if (type == typeof(uint))
			{
				return ResourceTypeCode.UInt32;
			}
			if (type == typeof(ulong))
			{
				return ResourceTypeCode.UInt64;
			}
			if (type == typeof(float))
			{
				return ResourceTypeCode.Single;
			}
			if (type == typeof(double))
			{
				return ResourceTypeCode.Double;
			}
			if (type == typeof(decimal))
			{
				return ResourceTypeCode.Decimal;
			}
			if (type == typeof(DateTime))
			{
				return ResourceTypeCode.DateTime;
			}
			if (type == typeof(TimeSpan))
			{
				return ResourceTypeCode.TimeSpan;
			}
			if (type == typeof(byte[]))
			{
				return ResourceTypeCode.ByteArray;
			}
			if (type == typeof(MemoryStream))
			{
				return ResourceTypeCode.Stream;
			}
			string text;
			if (type == typeof(ResourceWriter.PrecannedResource))
			{
				text = ((ResourceWriter.PrecannedResource)value).TypeName;
				if (text.StartsWith("ResourceTypeCode.", StringComparison.Ordinal))
				{
					text = text.Substring(17);
					return (ResourceTypeCode)Enum.Parse(typeof(ResourceTypeCode), text);
				}
			}
			else
			{
				text = type.AssemblyQualifiedName;
			}
			int num = types.IndexOf(text);
			if (num == -1)
			{
				num = types.Count;
				types.Add(text);
			}
			return num + ResourceTypeCode.StartOfUserTypes;
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x00096614 File Offset: 0x00095614
		private void WriteValue(ResourceTypeCode typeCode, object value, BinaryWriter writer, IFormatter objFormatter)
		{
			switch (typeCode)
			{
			case ResourceTypeCode.Null:
				return;
			case ResourceTypeCode.String:
				writer.Write((string)value);
				return;
			case ResourceTypeCode.Boolean:
				writer.Write((bool)value);
				return;
			case ResourceTypeCode.Char:
				writer.Write((ushort)((char)value));
				return;
			case ResourceTypeCode.Byte:
				writer.Write((byte)value);
				return;
			case ResourceTypeCode.SByte:
				writer.Write((sbyte)value);
				return;
			case ResourceTypeCode.Int16:
				writer.Write((short)value);
				return;
			case ResourceTypeCode.UInt16:
				writer.Write((ushort)value);
				return;
			case ResourceTypeCode.Int32:
				writer.Write((int)value);
				return;
			case ResourceTypeCode.UInt32:
				writer.Write((uint)value);
				return;
			case ResourceTypeCode.Int64:
				writer.Write((long)value);
				return;
			case ResourceTypeCode.UInt64:
				writer.Write((ulong)value);
				return;
			case ResourceTypeCode.Single:
				writer.Write((float)value);
				return;
			case ResourceTypeCode.Double:
				writer.Write((double)value);
				return;
			case ResourceTypeCode.Decimal:
				writer.Write((decimal)value);
				return;
			case ResourceTypeCode.DateTime:
			{
				long value2 = ((DateTime)value).ToBinary();
				writer.Write(value2);
				return;
			}
			case ResourceTypeCode.TimeSpan:
				writer.Write(((TimeSpan)value).Ticks);
				return;
			case ResourceTypeCode.ByteArray:
			{
				byte[] array = (byte[])value;
				writer.Write(array.Length);
				writer.Write(array, 0, array.Length);
				return;
			}
			case ResourceTypeCode.Stream:
			{
				MemoryStream memoryStream = (MemoryStream)value;
				if (memoryStream.Length > 2147483647L)
				{
					throw new ArgumentException(Environment.GetResourceString("ArgumentOutOfRange_MemStreamLength"));
				}
				int index;
				int num;
				memoryStream.InternalGetOriginAndLength(out index, out num);
				byte[] buffer = memoryStream.InternalGetBuffer();
				writer.Write(num);
				writer.Write(buffer, index, num);
				return;
			}
			}
			objFormatter.Serialize(writer.BaseStream, value);
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x00096814 File Offset: 0x00095814
		private static void Write7BitEncodedInt(BinaryWriter store, int value)
		{
			uint num;
			for (num = (uint)value; num >= 128U; num >>= 7)
			{
				store.Write((byte)(num | 128U));
			}
			store.Write((byte)num);
		}

		// Token: 0x040015AA RID: 5546
		private const int _ExpectedNumberOfResources = 1000;

		// Token: 0x040015AB RID: 5547
		private const int AverageNameSize = 40;

		// Token: 0x040015AC RID: 5548
		private const int AverageValueSize = 40;

		// Token: 0x040015AD RID: 5549
		private Hashtable _resourceList;

		// Token: 0x040015AE RID: 5550
		private Stream _output;

		// Token: 0x040015AF RID: 5551
		private Hashtable _caseInsensitiveDups;

		// Token: 0x040015B0 RID: 5552
		private Hashtable _preserializedData;

		// Token: 0x0200043D RID: 1085
		private class PrecannedResource
		{
			// Token: 0x06002C53 RID: 11347 RVA: 0x00096847 File Offset: 0x00095847
			internal PrecannedResource(string typeName, byte[] data)
			{
				this.TypeName = typeName;
				this.Data = data;
			}

			// Token: 0x040015B1 RID: 5553
			internal string TypeName;

			// Token: 0x040015B2 RID: 5554
			internal byte[] Data;
		}
	}
}

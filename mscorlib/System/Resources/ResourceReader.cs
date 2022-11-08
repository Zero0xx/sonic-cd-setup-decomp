using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;

namespace System.Resources
{
	// Token: 0x02000437 RID: 1079
	[ComVisible(true)]
	public sealed class ResourceReader : IResourceReader, IEnumerable, IDisposable
	{
		// Token: 0x06002C09 RID: 11273 RVA: 0x000941B8 File Offset: 0x000931B8
		public ResourceReader(string fileName)
		{
			this._resCache = new Dictionary<string, ResourceLocator>(FastResourceComparer.Default);
			this._store = new BinaryReader(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read), Encoding.UTF8);
			try
			{
				this.ReadResources();
			}
			catch
			{
				this._store.Close();
				throw;
			}
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x0009421C File Offset: 0x0009321C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public ResourceReader(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanRead)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_StreamNotReadable"));
			}
			this._resCache = new Dictionary<string, ResourceLocator>(FastResourceComparer.Default);
			this._store = new BinaryReader(stream, Encoding.UTF8);
			this._ums = (stream as UnmanagedMemoryStream);
			this.ReadResources();
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x00094288 File Offset: 0x00093288
		internal ResourceReader(Stream stream, Dictionary<string, ResourceLocator> resCache)
		{
			this._resCache = resCache;
			this._store = new BinaryReader(stream, Encoding.UTF8);
			this._ums = (stream as UnmanagedMemoryStream);
			this.ReadResources();
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x000942BA File Offset: 0x000932BA
		public void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x000942C3 File Offset: 0x000932C3
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x000942CC File Offset: 0x000932CC
		private void Dispose(bool disposing)
		{
			if (this._store != null)
			{
				this._resCache = null;
				if (disposing)
				{
					BinaryReader store = this._store;
					this._store = null;
					if (store != null)
					{
						store.Close();
					}
				}
				this._store = null;
				this._namePositions = null;
				this._nameHashes = null;
				this._ums = null;
				this._namePositionsPtr = null;
				this._nameHashesPtr = null;
			}
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x00094330 File Offset: 0x00093330
		internal unsafe static int ReadUnalignedI4(int* p)
		{
			return (int)(*(byte*)p) | (int)((byte*)p)[1] << 8 | (int)((byte*)p)[2] << 16 | (int)((byte*)p)[3] << 24;
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x0009435B File Offset: 0x0009335B
		private void SkipInt32()
		{
			this._store.BaseStream.Seek(4L, SeekOrigin.Current);
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x00094374 File Offset: 0x00093374
		private void SkipString()
		{
			int num = this._store.Read7BitEncodedInt();
			this._store.BaseStream.Seek((long)num, SeekOrigin.Current);
		}

		// Token: 0x06002C12 RID: 11282 RVA: 0x000943A1 File Offset: 0x000933A1
		private int GetNameHash(int index)
		{
			if (this._ums == null)
			{
				return this._nameHashes[index];
			}
			return ResourceReader.ReadUnalignedI4(this._nameHashesPtr + index);
		}

		// Token: 0x06002C13 RID: 11283 RVA: 0x000943C8 File Offset: 0x000933C8
		private int GetNamePosition(int index)
		{
			int num;
			if (this._ums == null)
			{
				num = this._namePositions[index];
			}
			else
			{
				num = ResourceReader.ReadUnalignedI4(this._namePositionsPtr + index);
			}
			if (num < 0 || (long)num > this._dataSectionOffset - this._nameSectionOffset)
			{
				throw new FormatException(Environment.GetResourceString("BadImageFormat_ResourcesNameOutOfSection", new object[]
				{
					index,
					num.ToString("x", CultureInfo.InvariantCulture)
				}));
			}
			return num;
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x00094445 File Offset: 0x00093445
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x0009444D File Offset: 0x0009344D
		public IDictionaryEnumerator GetEnumerator()
		{
			if (this._resCache == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("ResourceReaderIsClosed"));
			}
			return new ResourceReader.ResourceEnumerator(this);
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x0009446D File Offset: 0x0009346D
		internal ResourceReader.ResourceEnumerator GetEnumeratorInternal()
		{
			return new ResourceReader.ResourceEnumerator(this);
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x00094478 File Offset: 0x00093478
		internal int FindPosForResource(string name)
		{
			int num = FastResourceComparer.HashFunction(name);
			int i = 0;
			int num2 = this._numResources - 1;
			int num3 = -1;
			bool flag = false;
			while (i <= num2)
			{
				num3 = i + num2 >> 1;
				int nameHash = this.GetNameHash(num3);
				int num4;
				if (nameHash == num)
				{
					num4 = 0;
				}
				else if (nameHash < num)
				{
					num4 = -1;
				}
				else
				{
					num4 = 1;
				}
				if (num4 == 0)
				{
					flag = true;
					break;
				}
				if (num4 < 0)
				{
					i = num3 + 1;
				}
				else
				{
					num2 = num3 - 1;
				}
			}
			if (!flag)
			{
				return -1;
			}
			if (i != num3)
			{
				i = num3;
				while (i > 0 && this.GetNameHash(i - 1) == num)
				{
					i--;
				}
			}
			if (num2 != num3)
			{
				num2 = num3;
				while (num2 < this._numResources && this.GetNameHash(num2 + 1) == num)
				{
					num2++;
				}
			}
			lock (this)
			{
				for (int j = i; j <= num2; j++)
				{
					this._store.BaseStream.Seek(this._nameSectionOffset + (long)this.GetNamePosition(j), SeekOrigin.Begin);
					if (this.CompareStringEqualsName(name))
					{
						return this._store.ReadInt32();
					}
				}
			}
			return -1;
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x00094598 File Offset: 0x00093598
		private unsafe bool CompareStringEqualsName(string name)
		{
			int num = this._store.Read7BitEncodedInt();
			if (this._ums == null)
			{
				byte[] array = new byte[num];
				int num2;
				for (int i = num; i > 0; i -= num2)
				{
					num2 = this._store.Read(array, num - i, i);
					if (num2 == 0)
					{
						throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResourceNameCorrupted"));
					}
				}
				return FastResourceComparer.CompareOrdinal(array, num / 2, name) == 0;
			}
			byte* positionPointer = this._ums.PositionPointer;
			this._ums.Seek((long)num, SeekOrigin.Current);
			if (this._ums.Position > this._ums.Length)
			{
				throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResourcesNameTooLong"));
			}
			return FastResourceComparer.CompareOrdinal(positionPointer, num, name) == 0;
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x00094650 File Offset: 0x00093650
		private unsafe string AllocateStringForNameIndex(int index, out int dataOffset)
		{
			long num = (long)this.GetNamePosition(index);
			int num2;
			byte[] array;
			lock (this)
			{
				this._store.BaseStream.Seek(num + this._nameSectionOffset, SeekOrigin.Begin);
				num2 = this._store.Read7BitEncodedInt();
				if (this._ums != null)
				{
					if (this._ums.Position > this._ums.Length - (long)num2)
					{
						throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResourcesIndexTooLong", new object[]
						{
							index
						}));
					}
					char* positionPointer = (char*)this._ums.PositionPointer;
					string result = new string(positionPointer, 0, num2 / 2);
					this._ums.Position += (long)num2;
					dataOffset = this._store.ReadInt32();
					return result;
				}
				else
				{
					array = new byte[num2];
					int num3;
					for (int i = num2; i > 0; i -= num3)
					{
						num3 = this._store.Read(array, num2 - i, i);
						if (num3 == 0)
						{
							throw new EndOfStreamException(Environment.GetResourceString("BadImageFormat_ResourceNameCorrupted_NameIndex", new object[]
							{
								index
							}));
						}
					}
					dataOffset = this._store.ReadInt32();
				}
			}
			return Encoding.Unicode.GetString(array, 0, num2);
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x000947B4 File Offset: 0x000937B4
		private object GetValueForNameIndex(int index)
		{
			long num = (long)this.GetNamePosition(index);
			object result;
			lock (this)
			{
				this._store.BaseStream.Seek(num + this._nameSectionOffset, SeekOrigin.Begin);
				this.SkipString();
				int pos = this._store.ReadInt32();
				if (this._version == 1)
				{
					result = this.LoadObjectV1(pos);
				}
				else
				{
					ResourceTypeCode resourceTypeCode;
					result = this.LoadObjectV2(pos, out resourceTypeCode);
				}
			}
			return result;
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x00094838 File Offset: 0x00093838
		internal string LoadString(int pos)
		{
			this._store.BaseStream.Seek(this._dataSectionOffset + (long)pos, SeekOrigin.Begin);
			string result = null;
			int num = this._store.Read7BitEncodedInt();
			if (this._version == 1)
			{
				if (num == -1)
				{
					return null;
				}
				if (this.FindType(num) != typeof(string))
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceNotString_Type", new object[]
					{
						this.FindType(num).GetType().FullName
					}));
				}
				result = this._store.ReadString();
			}
			else
			{
				ResourceTypeCode resourceTypeCode = (ResourceTypeCode)num;
				if (resourceTypeCode != ResourceTypeCode.String && resourceTypeCode != ResourceTypeCode.Null)
				{
					string text;
					if (resourceTypeCode < ResourceTypeCode.StartOfUserTypes)
					{
						text = resourceTypeCode.ToString();
					}
					else
					{
						text = this.FindType(resourceTypeCode - ResourceTypeCode.StartOfUserTypes).FullName;
					}
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceNotString_Type", new object[]
					{
						text
					}));
				}
				if (resourceTypeCode == ResourceTypeCode.String)
				{
					result = this._store.ReadString();
				}
			}
			return result;
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x0009492C File Offset: 0x0009392C
		internal object LoadObject(int pos)
		{
			if (this._version == 1)
			{
				return this.LoadObjectV1(pos);
			}
			ResourceTypeCode resourceTypeCode;
			return this.LoadObjectV2(pos, out resourceTypeCode);
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x00094954 File Offset: 0x00093954
		internal object LoadObject(int pos, out ResourceTypeCode typeCode)
		{
			if (this._version == 1)
			{
				object obj = this.LoadObjectV1(pos);
				typeCode = ((obj is string) ? ResourceTypeCode.String : ResourceTypeCode.StartOfUserTypes);
				return obj;
			}
			return this.LoadObjectV2(pos, out typeCode);
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x0009498C File Offset: 0x0009398C
		internal object LoadObjectV1(int pos)
		{
			this._store.BaseStream.Seek(this._dataSectionOffset + (long)pos, SeekOrigin.Begin);
			int num = this._store.Read7BitEncodedInt();
			if (num == -1)
			{
				return null;
			}
			Type type = this.FindType(num);
			if (type == typeof(string))
			{
				return this._store.ReadString();
			}
			if (type == typeof(int))
			{
				return this._store.ReadInt32();
			}
			if (type == typeof(byte))
			{
				return this._store.ReadByte();
			}
			if (type == typeof(sbyte))
			{
				return this._store.ReadSByte();
			}
			if (type == typeof(short))
			{
				return this._store.ReadInt16();
			}
			if (type == typeof(long))
			{
				return this._store.ReadInt64();
			}
			if (type == typeof(ushort))
			{
				return this._store.ReadUInt16();
			}
			if (type == typeof(uint))
			{
				return this._store.ReadUInt32();
			}
			if (type == typeof(ulong))
			{
				return this._store.ReadUInt64();
			}
			if (type == typeof(float))
			{
				return this._store.ReadSingle();
			}
			if (type == typeof(double))
			{
				return this._store.ReadDouble();
			}
			if (type == typeof(DateTime))
			{
				return new DateTime(this._store.ReadInt64());
			}
			if (type == typeof(TimeSpan))
			{
				return new TimeSpan(this._store.ReadInt64());
			}
			if (type == typeof(decimal))
			{
				int[] array = new int[4];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this._store.ReadInt32();
				}
				return new decimal(array);
			}
			return this.DeserializeObject(num);
		}

		// Token: 0x06002C1F RID: 11295 RVA: 0x00094B9C File Offset: 0x00093B9C
		internal object LoadObjectV2(int pos, out ResourceTypeCode typeCode)
		{
			this._store.BaseStream.Seek(this._dataSectionOffset + (long)pos, SeekOrigin.Begin);
			typeCode = (ResourceTypeCode)this._store.Read7BitEncodedInt();
			switch (typeCode)
			{
			case ResourceTypeCode.Null:
				return null;
			case ResourceTypeCode.String:
				return this._store.ReadString();
			case ResourceTypeCode.Boolean:
				return this._store.ReadBoolean();
			case ResourceTypeCode.Char:
				return (char)this._store.ReadUInt16();
			case ResourceTypeCode.Byte:
				return this._store.ReadByte();
			case ResourceTypeCode.SByte:
				return this._store.ReadSByte();
			case ResourceTypeCode.Int16:
				return this._store.ReadInt16();
			case ResourceTypeCode.UInt16:
				return this._store.ReadUInt16();
			case ResourceTypeCode.Int32:
				return this._store.ReadInt32();
			case ResourceTypeCode.UInt32:
				return this._store.ReadUInt32();
			case ResourceTypeCode.Int64:
				return this._store.ReadInt64();
			case ResourceTypeCode.UInt64:
				return this._store.ReadUInt64();
			case ResourceTypeCode.Single:
				return this._store.ReadSingle();
			case ResourceTypeCode.Double:
				return this._store.ReadDouble();
			case ResourceTypeCode.Decimal:
				return this._store.ReadDecimal();
			case ResourceTypeCode.DateTime:
			{
				long dateData = this._store.ReadInt64();
				return DateTime.FromBinary(dateData);
			}
			case ResourceTypeCode.TimeSpan:
			{
				long ticks = this._store.ReadInt64();
				return new TimeSpan(ticks);
			}
			case ResourceTypeCode.ByteArray:
			{
				int num = this._store.ReadInt32();
				if (this._ums == null)
				{
					return this._store.ReadBytes(num);
				}
				if ((long)num > this._ums.Length - this._ums.Position)
				{
					throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResourceDataTooLong"));
				}
				byte[] array = new byte[num];
				this._ums.Read(array, 0, num);
				return array;
			}
			case ResourceTypeCode.Stream:
			{
				int num2 = this._store.ReadInt32();
				if (this._ums == null)
				{
					byte[] array2 = this._store.ReadBytes(num2);
					return new PinnedBufferMemoryStream(array2);
				}
				if ((long)num2 > this._ums.Length - this._ums.Position)
				{
					throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResourceDataTooLong"));
				}
				return new UnmanagedMemoryStream(this._ums.PositionPointer, (long)num2, (long)num2, FileAccess.Read, true);
			}
			}
			int typeIndex = typeCode - ResourceTypeCode.StartOfUserTypes;
			return this.DeserializeObject(typeIndex);
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x00094E68 File Offset: 0x00093E68
		private object DeserializeObject(int typeIndex)
		{
			Type type = this.FindType(typeIndex);
			if (this._safeToDeserialize == null)
			{
				this.InitSafeToDeserializeArray();
			}
			object obj;
			if (this._safeToDeserialize[typeIndex])
			{
				this._objFormatter.Binder = this._typeLimitingBinder;
				this._typeLimitingBinder.ExpectingToDeserialize(type);
				obj = this._objFormatter.UnsafeDeserialize(this._store.BaseStream, null);
			}
			else
			{
				this._objFormatter.Binder = null;
				obj = this._objFormatter.Deserialize(this._store.BaseStream);
			}
			if (obj.GetType() != type)
			{
				throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResType&SerBlobMismatch", new object[]
				{
					type.FullName,
					obj.GetType().FullName
				}));
			}
			return obj;
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x00094F28 File Offset: 0x00093F28
		private unsafe void ReadResources()
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.File | StreamingContextStates.Persistence));
			this._typeLimitingBinder = new ResourceReader.TypeLimitingDeserializationBinder();
			binaryFormatter.Binder = this._typeLimitingBinder;
			this._objFormatter = binaryFormatter;
			try
			{
				int num = this._store.ReadInt32();
				if (num != ResourceManager.MagicNumber)
				{
					throw new ArgumentException(Environment.GetResourceString("Resources_StreamNotValid"));
				}
				int num2 = this._store.ReadInt32();
				if (num2 > 1)
				{
					int num3 = this._store.ReadInt32();
					this._store.BaseStream.Seek((long)num3, SeekOrigin.Current);
				}
				else
				{
					this.SkipInt32();
					string text = this._store.ReadString();
					AssemblyName asmName = new AssemblyName(ResourceManager.MscorlibName);
					if (!ResourceManager.CompareNames(text, ResourceManager.ResReaderTypeName, asmName))
					{
						throw new NotSupportedException(Environment.GetResourceString("NotSupported_WrongResourceReader_Type", new object[]
						{
							text
						}));
					}
					this.SkipString();
				}
				int num4 = this._store.ReadInt32();
				if (num4 != 2 && num4 != 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_ResourceFileUnsupportedVersion", new object[]
					{
						2,
						num4
					}));
				}
				this._version = num4;
				this._numResources = this._store.ReadInt32();
				int num5 = this._store.ReadInt32();
				this._typeTable = new Type[num5];
				this._typeNamePositions = new int[num5];
				for (int i = 0; i < num5; i++)
				{
					this._typeNamePositions[i] = (int)this._store.BaseStream.Position;
					this.SkipString();
				}
				long position = this._store.BaseStream.Position;
				int num6 = (int)position & 7;
				if (num6 != 0)
				{
					for (int j = 0; j < 8 - num6; j++)
					{
						this._store.ReadByte();
					}
				}
				if (this._ums == null)
				{
					this._nameHashes = new int[this._numResources];
					for (int k = 0; k < this._numResources; k++)
					{
						this._nameHashes[k] = this._store.ReadInt32();
					}
				}
				else
				{
					this._nameHashesPtr = (int*)this._ums.PositionPointer;
					this._ums.Seek((long)(4 * this._numResources), SeekOrigin.Current);
					byte* positionPointer = this._ums.PositionPointer;
				}
				if (this._ums == null)
				{
					this._namePositions = new int[this._numResources];
					for (int l = 0; l < this._numResources; l++)
					{
						this._namePositions[l] = this._store.ReadInt32();
					}
				}
				else
				{
					this._namePositionsPtr = (int*)this._ums.PositionPointer;
					this._ums.Seek((long)(4 * this._numResources), SeekOrigin.Current);
					byte* positionPointer2 = this._ums.PositionPointer;
				}
				this._dataSectionOffset = (long)this._store.ReadInt32();
				this._nameSectionOffset = this._store.BaseStream.Position;
			}
			catch (EndOfStreamException inner)
			{
				throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResourcesHeaderCorrupted"), inner);
			}
			catch (IndexOutOfRangeException inner2)
			{
				throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResourcesHeaderCorrupted"), inner2);
			}
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x00095274 File Offset: 0x00094274
		private Type FindType(int typeIndex)
		{
			if (this._typeTable[typeIndex] == null)
			{
				long position = this._store.BaseStream.Position;
				try
				{
					this._store.BaseStream.Position = (long)this._typeNamePositions[typeIndex];
					string typeName = this._store.ReadString();
					this._typeTable[typeIndex] = Type.GetType(typeName, true);
				}
				finally
				{
					this._store.BaseStream.Position = position;
				}
			}
			return this._typeTable[typeIndex];
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x000952FC File Offset: 0x000942FC
		private void InitSafeToDeserializeArray()
		{
			this._safeToDeserialize = new bool[this._typeTable.Length];
			int i = 0;
			while (i < this._typeTable.Length)
			{
				long position = this._store.BaseStream.Position;
				string text;
				try
				{
					this._store.BaseStream.Position = (long)this._typeNamePositions[i];
					text = this._store.ReadString();
				}
				finally
				{
					this._store.BaseStream.Position = position;
				}
				Type type = Type.GetType(text, false);
				if (type == null)
				{
					AssemblyName assemblyName = null;
					string typeName = text;
					goto IL_D0;
				}
				if (type.BaseType != typeof(Enum))
				{
					string typeName = type.FullName;
					AssemblyName assemblyName = new AssemblyName();
					Assembly assembly = type.Assembly;
					assemblyName.Init(assembly.nGetSimpleName(), assembly.nGetPublicKey(), null, null, assembly.GetLocale(), AssemblyHashAlgorithm.None, AssemblyVersionCompatibility.SameMachine, null, AssemblyNameFlags.PublicKey, null);
					goto IL_D0;
				}
				this._safeToDeserialize[i] = true;
				IL_106:
				i++;
				continue;
				IL_D0:
				foreach (string asmTypeName in ResourceReader.TypesSafeForDeserialization)
				{
					AssemblyName assemblyName;
					string typeName;
					if (ResourceManager.CompareNames(asmTypeName, typeName, assemblyName))
					{
						this._safeToDeserialize[i] = true;
					}
				}
				goto IL_106;
			}
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x00095434 File Offset: 0x00094434
		public void GetResourceData(string resourceName, out string resourceType, out byte[] resourceData)
		{
			if (resourceName == null)
			{
				throw new ArgumentNullException("resourceName");
			}
			if (this._resCache == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("ResourceReaderIsClosed"));
			}
			int[] array = new int[this._numResources];
			int num = this.FindPosForResource(resourceName);
			if (num == -1)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ResourceNameNotExist", new object[]
				{
					resourceName
				}));
			}
			lock (this)
			{
				for (int i = 0; i < this._numResources; i++)
				{
					this._store.BaseStream.Position = this._nameSectionOffset + (long)this.GetNamePosition(i);
					int num2 = this._store.Read7BitEncodedInt();
					this._store.BaseStream.Position += (long)num2;
					array[i] = this._store.ReadInt32();
				}
				Array.Sort<int>(array);
				int num3 = Array.BinarySearch<int>(array, num);
				long num4 = (num3 < this._numResources - 1) ? ((long)array[num3 + 1] + this._dataSectionOffset) : this._store.BaseStream.Length;
				int num5 = (int)(num4 - ((long)num + this._dataSectionOffset));
				this._store.BaseStream.Position = this._dataSectionOffset + (long)num;
				ResourceTypeCode typeCode = (ResourceTypeCode)this._store.Read7BitEncodedInt();
				resourceType = this.TypeNameFromTypeCode(typeCode);
				num5 -= (int)(this._store.BaseStream.Position - (this._dataSectionOffset + (long)num));
				byte[] array2 = this._store.ReadBytes(num5);
				if (array2.Length != num5)
				{
					throw new FormatException(Environment.GetResourceString("BadImageFormat_ResourceNameCorrupted"));
				}
				resourceData = array2;
			}
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x000955F8 File Offset: 0x000945F8
		private string TypeNameFromTypeCode(ResourceTypeCode typeCode)
		{
			if (typeCode < ResourceTypeCode.StartOfUserTypes)
			{
				return "ResourceTypeCode." + typeCode.ToString();
			}
			int num = typeCode - ResourceTypeCode.StartOfUserTypes;
			long position = this._store.BaseStream.Position;
			string result;
			try
			{
				this._store.BaseStream.Position = (long)this._typeNamePositions[num];
				result = this._store.ReadString();
			}
			finally
			{
				this._store.BaseStream.Position = position;
			}
			return result;
		}

		// Token: 0x04001578 RID: 5496
		private BinaryReader _store;

		// Token: 0x04001579 RID: 5497
		internal Dictionary<string, ResourceLocator> _resCache;

		// Token: 0x0400157A RID: 5498
		private long _nameSectionOffset;

		// Token: 0x0400157B RID: 5499
		private long _dataSectionOffset;

		// Token: 0x0400157C RID: 5500
		private int[] _nameHashes;

		// Token: 0x0400157D RID: 5501
		private unsafe int* _nameHashesPtr;

		// Token: 0x0400157E RID: 5502
		private int[] _namePositions;

		// Token: 0x0400157F RID: 5503
		private unsafe int* _namePositionsPtr;

		// Token: 0x04001580 RID: 5504
		private Type[] _typeTable;

		// Token: 0x04001581 RID: 5505
		private int[] _typeNamePositions;

		// Token: 0x04001582 RID: 5506
		private BinaryFormatter _objFormatter;

		// Token: 0x04001583 RID: 5507
		private int _numResources;

		// Token: 0x04001584 RID: 5508
		private UnmanagedMemoryStream _ums;

		// Token: 0x04001585 RID: 5509
		private int _version;

		// Token: 0x04001586 RID: 5510
		private bool[] _safeToDeserialize;

		// Token: 0x04001587 RID: 5511
		private ResourceReader.TypeLimitingDeserializationBinder _typeLimitingBinder;

		// Token: 0x04001588 RID: 5512
		private static readonly string[] TypesSafeForDeserialization = new string[]
		{
			"System.String[], mscorlib, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
			"System.DateTime[], mscorlib, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
			"System.Drawing.Bitmap, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
			"System.Drawing.Imaging.Metafile, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
			"System.Drawing.Point, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
			"System.Drawing.PointF, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
			"System.Drawing.Size, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
			"System.Drawing.SizeF, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
			"System.Drawing.Font, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
			"System.Drawing.Icon, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
			"System.Drawing.Color, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
			"System.Windows.Forms.Cursor, System.Windows.Forms, Culture=neutral, PublicKeyToken=b77a5c561934e089",
			"System.Windows.Forms.Padding, System.Windows.Forms, Culture=neutral, PublicKeyToken=b77a5c561934e089",
			"System.Windows.Forms.LinkArea, System.Windows.Forms, Culture=neutral, PublicKeyToken=b77a5c561934e089",
			"System.Windows.Forms.ImageListStreamer, System.Windows.Forms, Culture=neutral, PublicKeyToken=b77a5c561934e089"
		};

		// Token: 0x02000438 RID: 1080
		internal sealed class TypeLimitingDeserializationBinder : SerializationBinder
		{
			// Token: 0x17000823 RID: 2083
			// (get) Token: 0x06002C27 RID: 11303 RVA: 0x00095719 File Offset: 0x00094719
			// (set) Token: 0x06002C28 RID: 11304 RVA: 0x00095721 File Offset: 0x00094721
			internal ObjectReader ObjectReader
			{
				get
				{
					return this._objectReader;
				}
				set
				{
					this._objectReader = value;
				}
			}

			// Token: 0x06002C29 RID: 11305 RVA: 0x0009572A File Offset: 0x0009472A
			internal void ExpectingToDeserialize(Type type)
			{
				this._typeToDeserialize = type;
			}

			// Token: 0x06002C2A RID: 11306 RVA: 0x00095734 File Offset: 0x00094734
			public override Type BindToType(string assemblyName, string typeName)
			{
				AssemblyName assemblyName2 = new AssemblyName();
				Assembly assembly = this._typeToDeserialize.Assembly;
				assemblyName2.Init(assembly.nGetSimpleName(), assembly.nGetPublicKey(), null, null, assembly.GetLocale(), AssemblyHashAlgorithm.None, AssemblyVersionCompatibility.SameMachine, null, AssemblyNameFlags.PublicKey, null);
				bool flag = false;
				foreach (string asmTypeName in ResourceReader.TypesSafeForDeserialization)
				{
					if (ResourceManager.CompareNames(asmTypeName, typeName, assemblyName2))
					{
						flag = true;
						break;
					}
				}
				Type type = this.ObjectReader.FastBindToType(assemblyName, typeName);
				if (type.IsEnum)
				{
					flag = true;
				}
				if (flag)
				{
					return null;
				}
				throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResType&SerBlobMismatch", new object[]
				{
					this._typeToDeserialize.FullName,
					typeName
				}));
			}

			// Token: 0x04001589 RID: 5513
			private Type _typeToDeserialize;

			// Token: 0x0400158A RID: 5514
			private ObjectReader _objectReader;
		}

		// Token: 0x02000439 RID: 1081
		internal sealed class ResourceEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06002C2C RID: 11308 RVA: 0x000957F6 File Offset: 0x000947F6
			internal ResourceEnumerator(ResourceReader reader)
			{
				this._currentName = -1;
				this._reader = reader;
				this._dataPosition = -2;
			}

			// Token: 0x06002C2D RID: 11309 RVA: 0x00095814 File Offset: 0x00094814
			public bool MoveNext()
			{
				if (this._currentName == this._reader._numResources - 1 || this._currentName == -2147483648)
				{
					this._currentIsValid = false;
					this._currentName = int.MinValue;
					return false;
				}
				this._currentIsValid = true;
				this._currentName++;
				return true;
			}

			// Token: 0x17000824 RID: 2084
			// (get) Token: 0x06002C2E RID: 11310 RVA: 0x00095870 File Offset: 0x00094870
			public object Key
			{
				get
				{
					if (this._currentName == -2147483648)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
					}
					if (!this._currentIsValid)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					if (this._reader._resCache == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("ResourceReaderIsClosed"));
					}
					return this._reader.AllocateStringForNameIndex(this._currentName, out this._dataPosition);
				}
			}

			// Token: 0x17000825 RID: 2085
			// (get) Token: 0x06002C2F RID: 11311 RVA: 0x000958E6 File Offset: 0x000948E6
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x17000826 RID: 2086
			// (get) Token: 0x06002C30 RID: 11312 RVA: 0x000958F3 File Offset: 0x000948F3
			internal int DataPosition
			{
				get
				{
					return this._dataPosition;
				}
			}

			// Token: 0x17000827 RID: 2087
			// (get) Token: 0x06002C31 RID: 11313 RVA: 0x000958FC File Offset: 0x000948FC
			public DictionaryEntry Entry
			{
				get
				{
					if (this._currentName == -2147483648)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
					}
					if (!this._currentIsValid)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					if (this._reader._resCache == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("ResourceReaderIsClosed"));
					}
					object obj = null;
					string key;
					lock (this._reader._resCache)
					{
						key = this._reader.AllocateStringForNameIndex(this._currentName, out this._dataPosition);
						ResourceLocator resourceLocator;
						if (this._reader._resCache.TryGetValue(key, out resourceLocator))
						{
							obj = resourceLocator.Value;
						}
						if (obj == null)
						{
							if (this._dataPosition == -1)
							{
								obj = this._reader.GetValueForNameIndex(this._currentName);
							}
							else
							{
								obj = this._reader.LoadObject(this._dataPosition);
							}
						}
					}
					return new DictionaryEntry(key, obj);
				}
			}

			// Token: 0x17000828 RID: 2088
			// (get) Token: 0x06002C32 RID: 11314 RVA: 0x000959F8 File Offset: 0x000949F8
			public object Value
			{
				get
				{
					if (this._currentName == -2147483648)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
					}
					if (!this._currentIsValid)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					if (this._reader._resCache == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("ResourceReaderIsClosed"));
					}
					return this._reader.GetValueForNameIndex(this._currentName);
				}
			}

			// Token: 0x06002C33 RID: 11315 RVA: 0x00095A68 File Offset: 0x00094A68
			public void Reset()
			{
				if (this._reader._resCache == null)
				{
					throw new InvalidOperationException(Environment.GetResourceString("ResourceReaderIsClosed"));
				}
				this._currentIsValid = false;
				this._currentName = -1;
			}

			// Token: 0x0400158B RID: 5515
			private const int ENUM_DONE = -2147483648;

			// Token: 0x0400158C RID: 5516
			private const int ENUM_NOT_STARTED = -1;

			// Token: 0x0400158D RID: 5517
			private ResourceReader _reader;

			// Token: 0x0400158E RID: 5518
			private bool _currentIsValid;

			// Token: 0x0400158F RID: 5519
			private int _currentName;

			// Token: 0x04001590 RID: 5520
			private int _dataPosition;
		}
	}
}

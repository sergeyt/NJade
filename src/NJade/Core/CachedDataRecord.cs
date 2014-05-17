using System;
using System.Data;

namespace NJade.Core
{
	//TODO: implement CachedDataRecord
	internal sealed class CachedDataRecord : IDataRecord
	{
		private readonly IDataRecord _dataRecord;

		public CachedDataRecord(IDataRecord dataRecord)
		{
			_dataRecord = dataRecord;
		}

		public string GetName(int i)
		{
			return _dataRecord.GetName(i);
		}

		public string GetDataTypeName(int i)
		{
			return _dataRecord.GetDataTypeName(i);
		}

		public Type GetFieldType(int i)
		{
			return _dataRecord.GetFieldType(i);
		}

		public object GetValue(int i)
		{
			return _dataRecord.GetValue(i);
		}

		public int GetValues(object[] values)
		{
			return _dataRecord.GetValues(values);
		}

		public int GetOrdinal(string name)
		{
			return _dataRecord.GetOrdinal(name);
		}

		public bool GetBoolean(int i)
		{
			return _dataRecord.GetBoolean(i);
		}

		public byte GetByte(int i)
		{
			return _dataRecord.GetByte(i);
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return _dataRecord.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		}

		public char GetChar(int i)
		{
			return _dataRecord.GetChar(i);
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return _dataRecord.GetChars(i, fieldoffset, buffer, bufferoffset, length);
		}

		public Guid GetGuid(int i)
		{
			return _dataRecord.GetGuid(i);
		}

		public short GetInt16(int i)
		{
			return _dataRecord.GetInt16(i);
		}

		public int GetInt32(int i)
		{
			return _dataRecord.GetInt32(i);
		}

		public long GetInt64(int i)
		{
			return _dataRecord.GetInt64(i);
		}

		public float GetFloat(int i)
		{
			return _dataRecord.GetFloat(i);
		}

		public double GetDouble(int i)
		{
			return _dataRecord.GetDouble(i);
		}

		public string GetString(int i)
		{
			return _dataRecord.GetString(i);
		}

		public decimal GetDecimal(int i)
		{
			return _dataRecord.GetDecimal(i);
		}

		public DateTime GetDateTime(int i)
		{
			return _dataRecord.GetDateTime(i);
		}

		public IDataReader GetData(int i)
		{
			return _dataRecord.GetData(i);
		}

		public bool IsDBNull(int i)
		{
			return _dataRecord.IsDBNull(i);
		}

		public int FieldCount
		{
			get { return _dataRecord.FieldCount; }
		}

		public object this[int i]
		{
			get { return _dataRecord[i]; }
		}

		public object this[string name]
		{
			get { return _dataRecord[name]; }
		}
	}
}
/* Copyright (c) 2023 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

namespace extDebug.Menu
{
	public class DMPlayerStorage : IDMStorage
	{
		#region IDMStorage Methods

		bool IDMStorage.Save(string key, object value)
		{
			if (string.IsNullOrEmpty(key) || value == null)
				return false;
			
			PlayerPrefs.SetString(key, Convert.ToString(value));

			return true;
		}

		object IDMStorage.Load(string key, Type valueType)
		{
			if (string.IsNullOrEmpty(key) || valueType == null)
				return null;

			if (PlayerPrefs.HasKey(key))
			{
				var stringValue = PlayerPrefs.GetString(key);

				if (valueType == typeof(string))
					return stringValue;

				if (valueType == typeof(sbyte))
					return Convert.ToSByte(stringValue);

				if (valueType == typeof(Int16))
					return Convert.ToInt16(stringValue);

				if (valueType == typeof(Int32))
					return Convert.ToInt32(stringValue);

				if (valueType == typeof(Int64))
					return Convert.ToInt64(stringValue);

				if (valueType == typeof(byte))
					return Convert.ToByte(stringValue);

				if (valueType == typeof(UInt16))
					return Convert.ToUInt16(stringValue);

				if (valueType == typeof(UInt32))
					return Convert.ToUInt32(stringValue);

				if (valueType == typeof(UInt64))
					return Convert.ToUInt64(stringValue);

				if (valueType == typeof(float))
					return Convert.ToSingle(stringValue);

				if (valueType == typeof(decimal))
					return Convert.ToDecimal(stringValue);

				if (valueType == typeof(bool))
					return Convert.ToBoolean(stringValue);

				if (valueType.IsEnum && Enum.TryParse(valueType, stringValue, out var enumValue))
					return enumValue;
			}

			return null;
		}

		#endregion
	}
}
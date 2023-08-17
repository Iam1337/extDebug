/* Copyright (c) 2023 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;
using System.Collections.Generic;
using extDebug.Menu;
using Random = UnityEngine.Random;

namespace extDebug.Examples.Menu
{
	public class Example : MonoBehaviour
	{
		#region Internal Types

		private enum ExampleEnums
		{
			One,
			Two,
			Three
		}

		[Flags]
		private enum ExampleFlags
		{
			One = 1 << 0,
			Two = 1 << 1,
			Three = 1 << 2,
		}

        private class ExampleLogsContainer : IDMLogsContainer
        {
            private enum LogType
            {
                Info,
                Warning,
                Error
            }

            private struct LogData
            {
                public LogType Type;
                public string Message;
            }

            private bool _isDirty = false;
            
            private readonly Dictionary<LogType, Color> _tagsColors = new()
            {
                { LogType.Info, Color.white},
                { LogType.Warning, Color.yellow },
                { LogType.Error, Color.red },
            };

            private readonly Dictionary<LogType, String> _tags = new()
            {
                { LogType.Info, "[I]" },
                { LogType.Warning, "[W]" },
                { LogType.Error, "[E]" },
            };
            private readonly List<LogData> _logs = new();

            public ExampleLogsContainer()
            {
                for (var i = 0; i < 100; i++) AddRandom();
            }

            public void AddRandom()
            {
                _logs.Add(new LogData
                {
                    Type = (LogType)Random.Range(0, 3),
                    Message = Guid.NewGuid() + (Random.value > 0.5f ? Guid.NewGuid().ToString() : string.Empty  )
                });

                _isDirty = true;
            }

            public bool IsDirty()
            {
                var isDirty = _isDirty;
                _isDirty = false;
                return isDirty;
            }

            public bool GetLog(int index, out string tag, out Color tagColor, out string message, out Color messageColor)
            {
                tag = string.Empty;
                tagColor = Color.clear;
                message = string.Empty;
                messageColor = Color.white;

                if (index >= _logs.Count)
                    return false;

                var log = _logs[_logs.Count - 1 - index];
                tag = _tags[log.Type];
                tagColor = _tagsColors[log.Type];
                message = log.Message;

                return true;
            }

            public int GetLogsCount()
            {
                return _logs.Count;
            }
        }

		#endregion

		#region Private Vars

		private readonly string _string = "Hello, World!";

		private string _string2 = "Variant 2";
		
		private readonly string[] _stringVariants = new string[] { "Variant 1", "Variant 2", "Variant 3" };

		private byte _uint8;

		private UInt16 _uint16;

		private UInt32 _uint32;

		private UInt64 _uint64;

		private sbyte _int8;

		private Int16 _int16;

		private Int32 _int32;

		private Int64 _int64;

		private float _float;

		private bool _bool;

		private Vector2 _vector2;

		private Vector3 _vector3;

		private Vector4 _vector4;

		private Quaternion _quaternion;

		private Color _color;

		private Vector2Int _vector2Int;

		private Vector3Int _vector3Int;

		private ExampleEnums _enum;

		private ExampleFlags _flags;

		private byte _uint8Storage;

		private UInt16 _uint16Storage;

		private UInt32 _uint32Storage;

		private UInt64 _uint64Storage;

		private sbyte _int8Storage;

		private Int16 _int16Storage;

		private Int32 _int32Storage;

		private Int64 _int64Storage;

		private float _floatStorage;

		private bool _boolStorage;

		private ExampleEnums _enumStorage;

		private ExampleFlags _flagsStorage;

        private ExampleLogsContainer _logsContainer;
        
		#endregion

		#region Unity Methods

		private void Start()
		{
			var storage = new DMPlayerStorage();
			var order = 0;
			
			// Simple Menus
			DM.Add("Simple Menus/Action", action => Debug.Log("Hello, Action!"), "Simple Action", order: -1);
			DM.Add("Simple Menus/String", () => _string, order: 0);
			DM.Add("Simple Menus/String Variants", () => _string2, v => _string2 = v, _stringVariants, order: 1);
			DM.Add("Simple Menus/UInt8", () => _uint8, v => _uint8 = v, order: 2);
			DM.Add("Simple Menus/UInt16", () => _uint16, v => _uint16 = v, order: 3);
			DM.Add("Simple Menus/UInt32", () => _uint32, v => _uint32 = v, order: 4);
			DM.Add("Simple Menus/UInt64", () => _uint64, v => _uint64 = v, order: 5);
			DM.Add("Simple Menus/Int8", () => _int8, v => _int8 = v, order: 6);
			DM.Add("Simple Menus/Int16", () => _int16, v => _int16 = v, order: 7);
			DM.Add("Simple Menus/Int32", () => _int32, v => _int32 = v, order: 8);
			DM.Add("Simple Menus/Int64", () => _int64, v => _int64 = v, order: 9);
			DM.Add("Simple Menus/Float", () => _float, v => _float = v, order: 10).SetPrecision(2);
			DM.Add("Simple Menus/Bool", () => _bool, v => _bool = v, order: 11);
			DM.Add("Simple Menus/Enum", () => _enum, v => _enum = v, order: 12);
			DM.Add("Simple Menus/Flags", () => _flags, v => _flags = v, order: 13);
			DM.Add("Simple Menus/Vector 2", () => _vector2, v => _vector2 = v, order: 14).SetPrecision(2);
			DM.Add("Simple Menus/Vector 3", () => _vector3, v => _vector3 = v, order: 15).SetPrecision(2);
			DM.Add("Simple Menus/Vector 4", () => _vector4, v => _vector4 = v, order: 16).SetPrecision(2);
			DM.Add("Simple Menus/Quaternion", () => _quaternion, v => _quaternion = v, order: 17).SetPrecision(2);
			DM.Add("Simple Menus/Color", () => _color, v => _color = v, order: 18).SetPrecision(2);
			DM.Add("Simple Menus/Vector 2 Int", () => _vector2Int, v => _vector2Int = v, order: 19);
			DM.Add("Simple Menus/Vector 3 Int", () => _vector3Int, v => _vector3Int = v, order: 20);

			// Storage
			DM.Add("Storage Values/UInt8", () => _uint8Storage, v => _uint8Storage = v, order: 1).SetStorage(storage);
			DM.Add("Storage Values/UInt16", () => _uint16Storage, v => _uint16Storage = v, order: 2).SetStorage(storage);
			DM.Add("Storage Values/UInt32", () => _uint32Storage, v => _uint32Storage = v, order: 3).SetStorage(storage);
			DM.Add("Storage Values/UInt64", () => _uint64Storage, v => _uint64Storage = v, order: 4).SetStorage(storage);
			DM.Add("Storage Values/Int8", () => _int8Storage, v => _int8Storage = v, order: 5).SetStorage(storage);
			DM.Add("Storage Values/Int16", () => _int16Storage, v => _int16Storage = v, order: 6).SetStorage(storage);
			DM.Add("Storage Values/Int32", () => _int32Storage, v => _int32Storage = v, order: 7).SetStorage(storage);
			DM.Add("Storage Values/Int64", () => _int64Storage, v => _int64Storage = v, order: 8).SetStorage(storage);
			DM.Add("Storage Values/Float", () => _floatStorage, v => _floatStorage = v, order: 9).SetPrecision(2).SetStorage(storage);
			DM.Add("Storage Values/Bool", () => _boolStorage, v => _boolStorage = v, order: 10).SetStorage(storage);
			DM.Add("Storage Values/Enum", () => _enumStorage, v => _enumStorage = v, order: 11).SetStorage(storage);
			DM.Add("Storage Values/Flags", () => _flagsStorage, v => _flagsStorage = v, order: 12).SetStorage(storage);

            // Logs
            _logsContainer = new ExampleLogsContainer();
            DM.Add("Logs", _logsContainer, "Logs Example", 10);
            
			// Dynamic
			DM.Add("Dynamic Transforms", FindObjectsOfType<Transform>, (branch, transform) =>
			{
				branch.Add("Name", a => { Debug.Log(transform); });
			});

			DM.Open();
		}

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _logsContainer.AddRandom();
        }

        #endregion
	}
}
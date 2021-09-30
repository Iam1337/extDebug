# extDebug - Debug Tools for Unity

Created by [iam1337](https://github.com/iam1337) and [hww](https://github.com/hww)

![](https://img.shields.io/badge/unity-2021.1%20or%20later-green.svg)
[![⚙ Build and Release](https://github.com/Iam1337/extDebug/actions/workflows/ci.yml/badge.svg)](https://github.com/Iam1337/extDebug/actions/workflows/ci.yml)
[![openupm](https://img.shields.io/npm/v/com.iam1337.extdebug?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.iam1337.extdebug/)
[![](https://img.shields.io/github/license/iam1337/extDebug.svg)](https://github.com/Iam1337/extDebug/blob/master/LICENSE)

### Table of Contents
- [Introduction](#introduction)
- [Installation](#installation)
- [extDebug.Menu](#extdebugmenu---debug-menu)
- [extDebug.Notifications](#extdebugnotifications---debug-notifications)
- [Author Contacts](#author-contacts)

## Introduction

extDebug are tools for easy development and testing of games on Unity. Supported platforms are PC, Mac and Linux / iOS / tvOS / Android / Universal Windows Platform (UWP) and other.

### Features:

- **Debug Menu**<br>
Allows you to add a debug menu in game, with many different functions.
- **Debug Notifications**<br>
Allows you to show a debug notification in game.
- **Analytics Heatmaps**<br>
TODO: Description

**And also:**

- TODO

**And much more**

## Installation:
**Old school**

Just copy the [Assets/extDebug](Assets/extDebug) folder into your Assets directory within your Unity project, or [download latest extDebug.unitypackage](https://github.com/iam1337/extDebug/releases).

**OpenUPM**

Via [openupm-cli](https://github.com/openupm/openupm-cli):<br>
```
openupm add com.iam1337.extdebug
```

Or if you don't have it, add the scoped registry to manifest.json with the desired dependency semantic version:
```
"scopedRegistries": [
	{
		"name": "package.openupm.com",
		"url": "https://package.openupm.com",
		"scopes": [
			"com.iam1337.extdebug",
		]
	}
],
"dependencies": {
	"com.iam1337.extdebug": "1.4.0"
}
```

**Package Manager**

Project supports Unity Package Manager. To install the project as a Git package do the following:

1. In Unity, open **Window > Package Manager**.
2. Press the **+** button, choose **"Add package from git URL..."**
3. Enter "https://github.com/iam1337/extDebug.git#upm" and press Add.

## extDebug.Menu - Debug Menu

It is easy to use, lightweight library initially forked from [hww/varp_debug_menu](https://github.com/hww/varp_debug_menu) but deeply modifyed. The library allows you to add a debug menu in game, with many different functions.

### Features:

- Changing values: numeric values, booleans, strings, enums, flags and other
- Store and restore default values
- Invoke actions
- Dynamic generation

### Examples:
**Values**<br>
```C#
byte _uint8;
UInt16 _uint16; // ushort
UInt32 _uint32; // uint
UInt64 _uint64; // ulong
sbyte _int8;
Int16 _int16; // short
Int32 _int32; // int
Int64 _int64; // long
float _float;
bool _bool;

DM.Add("Values/UInt8", () => _uint8, v => _uint8 = v);
DM.Add("Values/UInt16", () => _uint16, v => _uint16 = v);
DM.Add("Values/UInt32", () => _uint32, v => _uint32 = v);
DM.Add("Values/UInt64", () => _uint64, v => _uint64 = v);
DM.Add("Values/Int8", () => _int8, v => _int8 = v);
DM.Add("Values/Int16", () => _int16, v => _int16 = v);
DM.Add("Values/Int32", () => _int32, v => _int32 = v);
DM.Add("Values/Int64", () => _int64, v => _int64 = v);
DM.Add("Values/Float", () => _float, v => _float = v);
DM.Add("Values/Bool", () => _bool, v => _bool = v);
```
TODO: Screenshots

**Enums and Flags**<br>
```C#
enum ExampleEnums
{
	One,
	Two,
	Three
}

ExampleEnums _enum;

[Flags]
enum ExampleFlags
{
	One = 1 << 0,
	Two = 1 << 1,
	Three = 1 << 2,
}

ExampleFlags _flags;

DM.Add("Values/Enum", () => _enum, v => _enum = v);
DM.Add("Values/Flags", () => _flags, v => _flags = v);
```
TODO: Screenshots

**Actions**<br>
```C#
DM.Add("Debug/Action", action => Debug.Log("Hello World"));
DM.Add("Debug/Action 2", action => Debug.Log("Hello World"), "Action description"); // Action with description
```
TODO: Screenshots

**Branches**<br>
```C#
DM.Add("Example/Branch 1");
DM.Add("Example/Branch 2", "Branch description");

// Another way to add menu item in specific branch 
var branch = DM.Add("Example/Branch 3");
DM.Add(branch, "Action", action => Debug.Log("Hello World"));
```
TODO: Screenshots

### Keyboard Shortcuts:

**Shared**
- `Q` - Show or hide menu without closing it

**When the menu is open:**
- `W`, `S` - Moving through the menu
- `A`, `D` - Change value, invoke action, open/close branch
- `R` - Reset value to default

**When the menu is closed:**
- `Shift+A`, `Shift+D` - Change value, invoke action if menu is closed
- `Shift+R` - Reset value to default even if menu is closed

To change the default keyboard shortcuts, you need to create a class inherited from the [IDMInput](https://github.com/Iam1337/extDebug/blob/main/Assets/extDebug/Scripts/Menu/IDMInput.cs) interface, and set its instance to `DM.Input`.

### Rendering

To change the default IMGUI render, you need to create a class inherited from the [IDMRender](https://github.com/Iam1337/extDebug/blob/main/Assets/extDebug/Scripts/Menu/IDMRender.cs) interface, and set its instance to `DM.Render`.

## extDebug.Notifications - Debug Notifications

It is easy to use, in-game notification system. Based on [Garry's Mod notification](https://github.com/Facepunch/garrysmod/blob/master/garrysmod/lua/includes/modules/notification.lua) system.

### Examples:

**Simple notification**<br>
```C#
// Show notification for five seconds
DN.Notify("Simple notification", 5f); 
```

**Context notification**<br>
Calling a method with the same context allows you to declare infinite notifications. Also, the last call to the method with the new parameters and the same context will overwrite the parameters of the previous notification.
```C#
// Create simple context
object _context = new object();

// Show infinity notification
DN.Notify(_context, "Infinity notification", -1f);

// Hide infinity notification
DN.Kill(_context);
```

### Rendering

To change the default IMGUI render, you need to create a class inherited from the [IDNRender](https://github.com/Iam1337/extDebug/blob/main/Assets/extDebug/Scripts/Notifications/IDNRender.cs) interface, and set its instance to `DN.Render`.

## Author Contacts:
\> [telegram.me/iam1337](http://telegram.me/iam1337) <br>
\> [ext@iron-wall.org](mailto:ext@iron-wall.org)

## License
This project is under the MIT License.

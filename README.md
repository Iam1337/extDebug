# Extended Debug Menu

It is easy to use, lightweight library initially forked from [hww/varp_debug_menu](https://github.com/hww/varp_debug_menu) but deeply modifyed.

The library renders as text, in game menu.  

```C#

// The fields to modify by menu
public enum TrafficLight { Red,Green, Blue }
public TrafficLight enumValue;
public bool toggleValue;
public int integerValue;
public float floatValue;
        
// Create new menu
new DebugMenu("Edit/Preferences");
new DebugMenuToggle("Edit/Preferences/Toggle", () => toggleValue, value => toggleValue = value, 1);
new DebugMenuInteger("Edit/Preferences/Integer", () => integerValue, value => integerValue = value, 2);
new DebugMenuFloat("Edit/Preferences/Float", () => floatValue, value => floatValue = value, 3);
new DebugMenuAction("Edit/Preferences/Action", (item,tag) => { Debug.Log("Action"); }, 4);
new DebugMenuEnum<TrafficLight>("Edit/Preferences/TraficLight", () => enumValue, value => enumValue = value, 5);
new DebugMenu("Edit/Preferences/Extra Preferences", 10);
```

![Picture1](Documentation/menu-picture1.png)
![Picture2](Documentation/menu-picture2.png)
![Picture3](Documentation/menu-picture3.png)

Other way is to add items directly to menu.

```C#
var menu, new DebugMenu("Edit/Preferences");
new DebugMenuToggle(menu, "Toggle", () => toggleValue, value => toggleValue = value, 1);
new DebugMenuInteger( menu, "Integer",() => integerValue, value => integerValue = value, 2);
new DebugMenuFloat(menu, "Float", () => floatValue, value => floatValue = value, 3);
new DebugMenuAction(menu, "Action", (item,tag) => { Debug.Log("Action"); }, 4);
new DebugMenuEnum<TrafficLight>(menu, "TraficLight", () => enumValue, value => enumValue = value, 5);
```

## Default Value

For integer, floats and enum types creating new DebugMenuItem will capture current value as defaut. When value is default it displayed as bright green color. Information about other color tags see below.

- **booleans**
  - yellow _color of label for enabed feture_
  - white _color of label for disabled feature_
- **integers**, **floats**, **enums**
  - bright green _color of value for default_ 
  - yellow _color of value and label for not default value_
- **actions**
  - gray _color for inactive action_
  - other _color for active action_ 

## Keyboard Shortcuts

- E show or hide menu without closing it
- ESC close current menu and display previous, or hide menu if there are no more
- W,S move previous and next menu item
- A,D edit menu item, open close submenu
- R reset value to default
- Shift-A,Shift-D edit menu item even if menu is closed
- Shift-R reset value to default even if menu is closed

## Events

Menu manager sends messages to menu items for rendering and modifying items .

```C#
public enum EvenTag
{
    Null,               //< Nothing 
    Render,             //< Render item, update label, value and colors
    Left,               //< Decrease value or call action
    Right,              //< Increase value or call action
    Up,                 //< Go to previous item 
    Down,               //< Go to next item
    Reset,              //< Reset value to default
    OpenMenu,           //< When menu open    
    CloseMenu           //< When menu closed
}
```
## Actions

The action code can update the item's fields, and differently response for events: Inc,Dec and Reset

```C#
new DebugMenuAction("Edit/Preferences/Action", (item,tag) => { 
        switch (tag)
        {
        case EventTag.Right:
           item.value = "Increment";
           break;
        case EventTag.Left:
           item.value = "Decrement";
           break;
        ...
        }
}, 1);
```

## Open and Close Menu Events

Possible to add menu items when menu opens, and remove items when it closes.

```C#
new DebugMenu("Edit/Preferences/Extra Preferences", 30)
    .OnOpen(menu => 
    {
        new DebugMenuToggle("Toggle2", menu, () => toggleValue, value => toggleValue = value);
    })
    .OnClose(menu =>
    {
        menu.Clear();
    });
```

## Refresh and AutoRefresh Menu

If values in menu can be modified by game, to display it the menu should be rendered time to time.

```C#
new DebugMenu("Edit/Preferences").AutoRefresh(1f);
```

Set value 0 will never refresh menu. Alternative way is calling _RequestRefresh_ method.

```C#
menu.RequestRefresh()
```
## Increment Step and Precision

For integers and floats: The _step_ field is a step for _Inc_ and _Dec_ evens.  The _format_ field is argument for ToString(...) method.

**For floats** The _precision_ field is number of digits after period. For example _increment=5_ and _precision=2_ will make increment step 0.05

## Other Syntax Sugar

```C#
// For DebugMenu class
DebugMenu OnOpen(Action<DebugMenu> onOpen)
DebugMenu OnClose(Action<DebugMenu> onClose)
DebugMenu AutoRefresh(float period)

// For MenuItem class
DebugMenuItem Order(int order)
DebugMenuItem AddToMenu(DebugMenu menu)
DebugMenuItem Value(string value)
DebugMenuItem LabelColor(string value)
DebugMenuItem ValueColor(string value)

// For DebugMenuEnum class
DebugMenuEnum<T> Default(T value)

// For DebugMenuInteger class
DebugMenuInteger Default(int value)
DebugMenuInteger Step(int value)
DebugMenuInteger Format(string value)

// For DebugMenuFloat class
DebugMenuFloat Default(float value)
DebugMenuFloat Precision(int value)
DebugMenuFloat Step(int value)
DebugMenuFloat Format(string value)
```







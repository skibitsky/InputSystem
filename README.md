# InputSystem
_InputSystem_ (IS) is a custom wrapper for Unity Engine which allows you to separate all input logic into different handlers (one for character movement, one for car controller etc), rebind keys in runtime and save changed keys into xml file. It even provides class for key changing button to use it in your game’s input settings.

It was inspired by new Unity experimental input system (https://goo.gl/fdjL52). The code is well commented, so it will be easy to understand how the things works. Tested on PC only.
## Features
*	Event based, but can also be used in a common way by asking.
*	Stack based. If key has been already used, it won’t call next handlers’ methods (can be easily disabled).
*	Works with axes. 
*	Keys rebinding in runtime.
*	Settings saving. Simple UI script to help you with making input settings.
*	Well documented code
*	100% free and open.
*	Used by Salday in our own project. We will keep improving and supporting IS because our projects depends on it.

## How does it work?
There are 3 very important classes which handle all core logic: InputListener, InputHandler and InputManager.

### InputListener
InputListener isn’t MonoBehavior class which handles one “action” data: name, keys and delegate which will be called by pressing the keys.
Let’s say you want to move your player forward. To do that you need InputListener called, for example “Movement forward” with Positive key ‘W’ and Alternative key ‘Arrow up”. Then you will be able to add movement logic to the listener’s delegate from anywhere.

### InputHandler
In simple words, InputHandler is a storage for InputListeners. 
First of all, you have to fill your Handler (for example “Player Movement” handler) with default InputListener templates (consider it as default Input settings for this handler). There are 3 lists which can be filled with the listeners: JustPressed (`Input.GetKeyDown`), Pressed (`Input.KetKey`) and JustReleased (`Input.GetKeyUp`).
During initialization of there is no custom Handler settings saved, Handler moves all listener from lists to the dictionaries. Now all system works only with these dictionaries. Lists were made only to fill the default Listeners from the editor.

### InputManager
InputManager is the core of party. The most important part of it is the “InputHandlerStack” stack. Manager loops through all keys from Stack’s Handlers from the top and invokes Listener’s delegate if needed. It’s singleton.

Let’s say, you have two handlers: “Player” and “Car”. To control the player, you should add the handler to the Stack. When our player gets in the car, “Car” handler should also be added to the stack. And there is a problem in this situation, now you have similar active Listeners: player and car movement (both are controlled via WSAD and arrows), but we need to invoke only car movement logic and be able to use other but movement player logic. To do that, you can mark BlockKeys in “Car” handler. If BlockKeys is true, InputManager won’t invoke delegates with the key if it has been already used this frame. This is one of the mane IS features.
You can also block all keys in all other handlers with HardBlockKeys (can be used for UI).

IS can also works with axes in similar way. You have to fill the axes for each Handler (if needed) and then ask InputManger for them 
```
Debug.Log(InputManager.instance.GetAxis("Mouse X"));
```
Axes can be blocked as well.

## Demo
For better understanding, please run demo scene and check out PlayerController and Chat scripts. 
There is a very primitive player controller which moves the cube left-right. Try to change keys from the menu, they will be updated in xml as well.
After enabling chat, “Chat” handler, which hard blocks all the keys, will be added to the top of the Stack. (Input Field on the left bottom do nothing 😊 )

## Support
If you have any questions or suggestions, please contact us: support@salday.com



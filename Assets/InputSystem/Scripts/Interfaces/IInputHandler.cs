using System;
using System.Collections.Generic;
using UnityEngine;

namespace Salday.InputSystem
{
    public interface IInputHandler
    {
        /// <summary>
        /// Name of input handler represents its purpose and can be shown in settings for example
        /// It's also used in AllInputHandlers dic in InptuManager
        /// </summary>
        string Name { get; }
        /// <summary>
        /// If true - each listener action will be invoked only once per frame.
        /// So if both Positive and Alternative keys are pressed at one time, 
        /// Action won't be invoked twice.
        /// </summary>
        bool InvokeOncePerFrame { get; }
        /// <summary>
        /// Unity CursorLockMode for this IInputHandler
        /// </summary>
        CursorLockMode CursorLockMode { get; }


        /// <summary>
        /// If true it InputManager will work only with this handler's keys f it is on the top of stack
        /// </summary>
        bool HardBlockKeys { get; }
        /// <summary>
        /// Should InputManager stop on this handler if it contains called key?
        /// </summary>
        bool BlockKeys { get; }


        /// <summary>
        /// Should InputManager block all axes in handlers which comes after him in the Stack?
        /// </summary>
        bool HardBlockAxes { get; }
        /// <summary>
        /// All Handler's axes
        /// </summary>
        List<InputAxis> Axes { get; }


        /// <summary>
        /// JustPressed (Input.GetKeyDown)
        /// </summary>
        Dictionary<KeyCode, InputListener> JustPressed { get; }
        /// <summary>
        /// Pressed (Input.GetKey)
        /// </summary>
        Dictionary<KeyCode, InputListener> Pressed { get; }
        /// <summary>
        /// JustReleased (Input.GetKeyUp)
        /// </summary>
        Dictionary<KeyCode, InputListener> JustReleased { get; }


        /// <summary>
        /// Fills dictionaries with InputListeners from the saving file.
        /// If file doesn't exist - with default data filled from the Editor.
        /// <remark> Called from InputManager Awake() </remark>
        /// </summary>
        bool Init();

        /// <summary>
        /// Returns all KeyCodes used in all handler's dictionaries.
        /// </summary>
        /// <remarks> 
        /// Used in InputManager to update KeyCodesToListen list in 
        /// InputManager.UpdateKeyCodes()
        /// </remarks>
        List<KeyCode> GetAllKeyCodes();

        /// <summary>
        /// Returns InputListener from the handler by name. 
        /// It can return null if there is no listener with such name.
        /// </summary>
        /// <param name="name">Name of InputListener</param>
        /// <returns></returns>
        InputListener GetListener(string name);

        /// <summary>
        /// Changes key in runtime to access listener in all dictionaries. 
        /// </summary>
        /// <param name="listener">Input listener to be changed</param>
        /// <param name="from">Old key</param>
        /// <param name="to">New key</param>
        void ChangeKey(string listener, KeyCode from, KeyCode to);

        /// <summary>
        /// Save setting to the file.
        /// </summary>
        void SaveHandler();


        /// <summary>
        /// Adds passed action to the InputListiner of specific name in 
        /// JustPressed dictionary.
        /// </summary>
        /// <param name="name">Name of InputListener</param>
        /// <param name="method">Method to be added to the listener</param>
        void AddJustPressedAction(string name, Action method);

        /// <summary>
        /// Adds passed action to the InputListiner of specific key in 
        /// JustPressed dictionary.
        /// </summary>
        /// <param name="key">KeyCode of the key the method to be subscribed to</param>
        /// <param name="method">Method to be subscribed</param>
        void AddJustPressedAction(KeyCode key, Action method);

        /// <summary>
        /// Removes passed action from InputListener based on the name of listener
        /// </summary>
        /// <param name="name">Name of the listener</param>
        /// <param name="method">Method to be removed</param>
        void RemoveJustPressedAction(string name, Action method);

        /// <summary>
        /// Removes passed action from InputListener based on the KeyCode.
        /// </summary>
        /// <param name="key">KeyCode of the key the method was subscribed to</param>
        /// <param name="method">Method to be removed</param>
        void RemoveJustPressedAction(KeyCode key, Action method);

        /// <summary>
        /// Adds passed action to the InputListiner of specific name in 
        /// Pressed dictionary.
        /// </summary>
        /// <param name="name">Name of InputListener</param>
        /// <param name="method">Method to be added to the listener</param>
        void AddPressedAction(string name, Action method);

        /// <summary>
        /// Adds passed action to the InputListiner of specific key in 
        /// Pressed dictionary.
        /// </summary>
        /// <param name="key">KeyCode of the key the method to be subscribed to</param>
        /// <param name="method">Method to be subscribed</param>
        void AddPressedAction(KeyCode key, Action method);

        /// <summary>
        /// Removes passed action from InputListener based on the name of listener
        /// </summary>
        /// <param name="name">Name of the listener</param>
        /// <param name="method">Method to be removed</param>
        void RemovePressedAction(string name, Action method);

        /// <summary>
        /// Removes passed action from InputListener based on the KeyCode.
        /// </summary>
        /// <param name="key">KeyCode of the key the method was subscribed to</param>
        /// <param name="method">Method to be removed</param>
        void RemovePressedAction(KeyCode key, Action method);

        /// <summary>
        /// Adds passed action to the InputListiner of specific name in 
        /// JustReleased dictionary.
        /// </summary>
        /// <param name="name">Name of InputListener</param>
        /// <param name="method">Method to be added to the listener</param>
        void AddJustReleasedAction(string name, Action method);

        /// <summary>
        /// Adds passed action to the InputListiner of specific key in 
        /// JustReleased dictionary.
        /// </summary>
        /// <param name="key">KeyCode of the key the method to be subscribed to</param>
        /// <param name="method">Method to be subscribed</param>
        void AddJustReleasedAction(KeyCode key, Action method);

        /// <summary>
        /// Removes passed action from InputListener based on the name of listener
        /// </summary>
        /// <param name="name">Name of the listener</param>
        /// <param name="method">Method to be removed</param>
        void RemoveJustReleasedAction(string name, Action method);

        /// <summary>
        /// Removes passed action from InputListener based on the KeyCode.
        /// </summary>
        /// <param name="key">KeyCode of the key the method was subscribed to</param>
        /// <param name="method">Method to be removed</param>
        void RemoveJustReleasedAction(KeyCode key, Action method);

        // Is something has change inside of InputHandler, Manager should update his data
        bool isDirty { get; set; }
    }
}
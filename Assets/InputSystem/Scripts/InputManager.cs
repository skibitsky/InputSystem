using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;

namespace Salday.GameFramework.InputSystem
{
    public class InputManager : MonoBehaviour
    {
        // Singleton
        public static InputManager instance;

        // Stack of all active Input Handlers
        Stack<InputHandler> InputHandlersStack = new Stack<InputHandler>();

        // List of all keys used in active Handlers to loop through
        List<KeyCode> KeyCodesToListen = new List<KeyCode>();

        // All axes which should return value from UnityEngine.Input
        // Updated in UpdateStack()
        Dictionary<string, InputAxis> AxesToListen = new Dictionary<string, InputAxis>();

        // Collection of all inited InptuHandlers. Key = InptuHandler.Name
        Dictionary<string, InputHandler> AllInptuHandlers = new Dictionary<string, InputHandler>();

        // Current Cursor lock state/mode
        CursorLockMode CurrentCursorLockMode = CursorLockMode.None;

        // Singleton stuff
        void Awake()
        {
            if (InputManager.instance == null)
            {
                InputManager.instance = this;
            }
            else Destroy(this);
        }

        void Start()
        {
            UpdateStack();
        }

        // Loops through all InputHandlers in the stack (frotm the top to bottom) 
        void Update()
        {
            InputListener il;
            InputHandler ih;
            // Loop through all keys used in handlers from the stack
            foreach (KeyCode key in KeyCodesToListen)
            {
                // If the key was JustPressed
                if (Input.GetKeyDown(key))
                {
                    // Taking top handler
                    ih = InputHandlersStack.Peek();
                    // If this handler HardBlock we have to work only with it
                    if (ih.HardBlockKeys)
                    {
                        // Checking if listener has pressed key
                        if (ih.JustPressed.TryGetValue(key, out il))
                        {
                            // Invokes all listener's actions
                            if (il.Actions != null)
                            {
                                // If it must be invoked only once per frame
                                if (ih.InvokeOncePerFrame)
                                {
                                    // Let's check if it hasn't been already invoked
                                    if (!il.Invoked)
                                    {
                                        il.Actions.Invoke();
                                        il.Invoked = true;
                                    }
                                }
                                else
                                    il.Actions.Invoke();
                            }

                        }
                    }
                    // If it wasn't hard blocked
                    else
                    {
                        // Loop through all handlers (from the top because it's a Stack)
                        foreach (var handler in InputHandlersStack)
                        {
                            // Checking if listener has pressed key
                            if (handler.JustPressed.TryGetValue(key, out il))
                            {
                                // Invokes all listener's actions
                                if (il.Actions != null)
                                {
                                    // If it must be invoked only once per frame
                                    if (handler.InvokeOncePerFrame)
                                    {
                                        // Let's check if it hasn't been already invoked
                                        if (!il.Invoked)
                                        {
                                            il.Actions.Invoke();
                                            il.Invoked = true;
                                        }
                                    }
                                    else
                                        il.Actions.Invoke();
                                }
                                // If it has Block we won't go to the other handlers with this key
                                if (handler.BlockKeys) break;
                            }
                        }
                    }
                }

                // Same here for Pressed
                if (Input.GetKey(key))
                {
                    ih = InputHandlersStack.Peek();
                    if (ih.HardBlockKeys)
                    {
                        if (ih.Pressed.TryGetValue(key, out il))
                        {
                            if (il.Actions != null)
                            {
                                if (ih.InvokeOncePerFrame)
                                {
                                    if (!il.Invoked)
                                    {
                                        il.Actions.Invoke();
                                        il.Invoked = true;
                                    }
                                }
                                else
                                    il.Actions.Invoke();
                            }
                        }
                    }
                    else
                    {
                        foreach (var handler in InputHandlersStack)
                        {
                            if (handler.Pressed.TryGetValue(key, out il))
                            {
                                if (il.Actions != null)
                                {
                                    if (handler.InvokeOncePerFrame)
                                    {
                                        if (!il.Invoked)
                                        {
                                            il.Actions.Invoke();
                                            il.Invoked = true;
                                        }
                                    }
                                    else
                                        il.Actions.Invoke();
                                }
                                if (handler.BlockKeys) break;
                            }
                        }
                    }
                }

                // Same here for JustReleased
                if (Input.GetKeyUp(key))
                {
                    ih = InputHandlersStack.Peek();
                    if (ih.HardBlockKeys)
                    {
                        if (ih.JustReleased.TryGetValue(key, out il))
                        {
                            if (il.Actions != null)
                            {
                                if (ih.InvokeOncePerFrame)
                                {
                                    if (!il.Invoked)
                                    {
                                        il.Actions.Invoke();
                                        il.Invoked = true;
                                    }
                                }
                                else
                                    il.Actions.Invoke();
                            }
                        }
                    }
                    else
                    {
                        foreach (var handler in InputHandlersStack)
                        {
                            if (handler.JustReleased.TryGetValue(key, out il))
                            {
                                if (il.Actions != null)
                                {
                                    if (handler.InvokeOncePerFrame)
                                    {
                                        if (!il.Invoked)
                                        {
                                            il.Actions.Invoke();
                                            il.Invoked = true;
                                        }
                                    }
                                    else
                                        il.Actions.Invoke();
                                }
                                if (handler.BlockKeys) break;
                            }
                        }
                    }
                }

            }


        }

        void FixedUpdate()
        {
            // We assign cursor lock state so many times because it can fuck up
            // in different situation depending on IS usage in your project 
            // (for example when loading new scene with DontSestroyOnLoad handler etc...)
            Cursor.lockState = CurrentCursorLockMode;
        }

        /// <summary>
        /// Updates a list of all used keysand axes.
        /// Sets up Cursor.lockState.
        /// </summary>
        public void UpdateStack()
        {
            var continueAddingAxes = true; 

            KeyCodesToListen.Clear();
            AxesToListen.Clear();
            foreach (var ih in InputHandlersStack)
            {
                // Getting keys
                if (KeyCodesToListen.Count == 0)
                    KeyCodesToListen = ih.GetAllKeyCodes();
                else
                    KeyCodesToListen.Concat(ih.GetAllKeyCodes());

                // Getting axes
                if(continueAddingAxes)
                {
                    foreach(var axis in ih.Axes)
                        AxesToListen.Add(axis.Name, axis);

                    if (ih.HardBlockAxes)
                        continueAddingAxes = false;
                }
            }

            if(InputHandlersStack.Count != 0)
                CurrentCursorLockMode = InputHandlersStack.Peek().CursorLockMode;
        }

        /// <summary>
        /// Inits the handler and adds it to the AllInptuHandlers dictionary.
        /// </summary>
        /// <param name="handler">Handler to init</param>
        public void InitNewInputHandler(InputHandler handler)
        {
            if (!AllInptuHandlers.ContainsKey(handler.Name))
            {
                handler.Init();
                AllInptuHandlers.Add(handler.Name, handler);
            }
        }

        /// <summary>
        /// Returns inited InputHandler by name from AllInputHandlers.
        /// Note that it can return null if asked handler wasn't inited or doesn't exist
        /// </summary>
        /// <param name="name">Name of InputHandler</param>
        public InputHandler GetInputHandler(string name)
        {
            InputHandler h;
            AllInptuHandlers.TryGetValue(name, out h);
            return h;
        }

        /// <summary>
        /// Returns the value of the virtual axis identified by axisName if allowed
        /// </summary>
        public float GetAxis(string axisName)
        {
            if (AxesToListen.ContainsKey(axisName))
                return Input.GetAxis(AxesToListen[axisName].GetAxisName());
            else return 0;
        }

        /// <summary>
        /// Adds passed InputHandler to the top of stack
        /// </summary>
        /// <param name="ih">InputHandler to be pushed</param>
        /// <returns>True if handler was added to the stack</returns>
        public bool AddInputHandlerToStack(InputHandler ih)
        {
            InputHandlersStack.Push(ih);
            UpdateStack();
            return true;
        }

        /// <summary>
        /// Adds inited InputHandler to the top of stack by name from AllInptuHandlers collection
        /// </summary>
        /// <param name="name">Name of inited InputHandler</param>
        /// <returns>True if handler was added to the stack</returns>
        public bool AddInputHandlerToStack(string name)
        {
            InputHandler h;
            AllInptuHandlers.TryGetValue(name, out h);
            if (h != null)
            {
                InputHandlersStack.Push(AllInptuHandlers[name]);
                UpdateStack();
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Removes last added input handler from the top of stack
        /// </summary>
        public void RemoveInputHandlerFromStack()
        {
            if(InputHandlersStack.Count != 0)
            {
                InputHandlersStack.Pop();
                UpdateStack();
            }      
        }

        /// <summary>
        /// Removes passed input handler from the stack
        /// </summary>
        /// <param name="handler">Handler to be removed</param>
        public void RemoveInputHandlerFromStack(InputHandler handler)
        {
            Stack<InputHandler> temp = new Stack<InputHandler>();
            InputHandler x;

            while ((x = InputHandlersStack.Pop()) != handler)
                temp.Push(x);
            while (temp.Count > 0)
                InputHandlersStack.Push(temp.Pop());

            UpdateStack();
        }

        /// <summary>
        /// Removes passed input handler from the stack
        /// </summary>
        /// <param name="handler">Name of the handler to be removed</param>
        public void RemoveInputHandlerFromStack(string handler)
        {
            Stack<InputHandler> temp = new Stack<InputHandler>();
            InputHandler x;

            while ((x = InputHandlersStack.Pop()).Name != handler)
                temp.Push(x);
            while (temp.Count > 0)
                InputHandlersStack.Push(temp.Pop());

            UpdateStack();
        }

        /// <summary>
        /// Returns true while the user holds down the key identified by InputListener name.
        /// Takes into account handler BlockKeys and HardBlockKeys.
        /// <para>It's better to pass InputListener, not a name.</para>
        /// </summary>
        public bool GetPressed(string listenerName)
        {
            // We have to get keys of the listener,
            // that's why it's better to pass listener, 
            // no need for foreach and if
            KeyCode Pos = KeyCode.None;
            KeyCode Alt = KeyCode.None;
            foreach(var h in AllInptuHandlers.Values)
            {
                var l = h.GetListener(listenerName);
                if(l != null && listenerName == l.Name)
                {
                    Pos = l.Positive;
                    Alt = l.Alternative;
                    break;
                }
            }

            bool block = false;
            if (Input.GetKey(Pos) || Input.GetKey(Alt))
            {
                foreach (var h in InputHandlersStack)
                {
                    foreach (var l in h.Pressed.Values)
                    {
                        if (l.Name == listenerName)
                        {
                            return true;
                        }
                        else
                        {
                            if (l.Positive == Pos || l.Alternative == Alt)
                                if (h.BlockKeys)
                                    block = true;
                        }
                    }
                    if (h.HardBlockKeys || block)
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true while the user holds down the key identified by InputListener name.
        /// Takes into account handler BlockKeys and HardBlockKeys. <para />
        /// <para>It's better to pass InputListener, not a name.</para>
        /// </summary>
        public bool GetPressed(InputListener listener)
        {
            bool block = false;
            if (Input.GetKey(listener.Positive) || Input.GetKey(listener.Alternative))
            {
                foreach (var h in InputHandlersStack)
                {
                    foreach (var l in h.Pressed.Values)
                    {
                        if (l.Name == listener.Name)
                        {
                            return true;
                        }
                        else
                        {
                            if (l.Positive == listener.Positive || l.Alternative == listener.Alternative)
                                if (h.BlockKeys)
                                    block = true;
                        }
                    }
                    if (h.HardBlockKeys || block)
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true during the frame the user starts pressing down the key identified by InputListener name.
        /// Takes into account handler BlockKeys and HardBlockKeys.
        /// <para>It's better to pass InputListener, not a name.</para>
        /// </summary>
        public bool GetJustPressed(string listenerName)
        {
            // We have to get keys of the listener,
            // that's why it's better to pass listener, 
            // no need for foreach and if
            KeyCode Pos = KeyCode.None;
            KeyCode Alt = KeyCode.None;
            foreach (var h in AllInptuHandlers.Values)
            {
                var l = h.GetListener(listenerName);
                if (l != null && listenerName == l.Name)
                {
                    Pos = l.Positive;
                    Alt = l.Alternative;
                    break;
                }
            }

            bool block = false;
            if (Input.GetKeyDown(Pos) || Input.GetKeyDown(Alt))
            {
                foreach (var h in InputHandlersStack)
                {
                    foreach (var l in h.JustPressed.Values)
                    {
                        if (l.Name == listenerName)
                        {
                            return true;
                        }
                        else
                        {
                            if (l.Positive == Pos || l.Alternative == Alt)
                                if (h.BlockKeys)
                                    block = true;
                        }
                    }
                    if (h.HardBlockKeys || block)
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true during the frame the user starts pressing down the key identified by InputListener name.
        /// Takes into account handler BlockKeys and HardBlockKeys.
        /// <para>It's better to pass InputListener, not a name.</para>
        /// </summary>
        public bool GetJustPressed(InputListener listener)
        {
            bool block = false;
            if (Input.GetKeyDown(listener.Positive) || Input.GetKeyDown(listener.Alternative))
            {
                foreach (var h in InputHandlersStack)
                {
                    foreach (var l in h.JustPressed.Values)
                    {
                        if (l.Name == listener.Name)
                        {
                            return true;
                        }
                        else
                        {
                            if (l.Positive == listener.Positive || l.Alternative == listener.Alternative)
                                if (h.BlockKeys)
                                    block = true;
                        }
                    }
                    if (h.HardBlockKeys || block)
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true during the frame the user releases the key identified by InputListener name.
        /// Takes into account handler BlockKeys and HardBlockKeys.
        /// <para>It's better to pass InputListener, not a name.</para>
        /// </summary>
        public bool GetJustReleased(string listenerName)
        {
            // We have to get keys of the listener,
            // that's why it's better to pass listener, 
            // no need for foreach and if
            KeyCode Pos = KeyCode.None;
            KeyCode Alt = KeyCode.None;
            foreach (var h in AllInptuHandlers.Values)
            {
                var l = h.GetListener(listenerName);
                if (l != null && listenerName == l.Name)
                {
                    Pos = l.Positive;
                    Alt = l.Alternative;
                    break;
                }
            }

            bool block = false;
            if (Input.GetKeyUp(Pos) || Input.GetKeyUp(Alt))
            {
                foreach (var h in InputHandlersStack)
                {
                    foreach (var l in h.JustReleased.Values)
                    {
                        if (l.Name == listenerName)
                        {
                            return true;
                        }
                        else
                        {
                            if (l.Positive == Pos || l.Alternative == Alt)
                                if (h.BlockKeys)
                                    block = true;
                        }
                    }
                    if (h.HardBlockKeys || block)
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true during the frame the user releases the key identified by InputListener name. name.
        /// Takes into account handler BlockKeys and HardBlockKeys.
        /// <para>It's better to pass InputListener, not a name.</para>
        /// </summary>
        public bool GetJustReleased(InputListener listener)
        {
            bool block = false;
            if (Input.GetKeyUp(listener.Positive) || Input.GetKeyUp(listener.Alternative))
            {
                foreach (var h in InputHandlersStack)
                {
                    foreach (var l in h.JustReleased.Values)
                    {
                        if (l.Name == listener.Name)
                        {
                            return true;
                        }
                        else
                        {
                            if (l.Positive == listener.Positive || l.Alternative == listener.Alternative)
                                if (h.BlockKeys)
                                    block = true;
                        }
                    }
                    if (h.HardBlockKeys || block)
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Changes specific listener key in runtime and calls method to update UI
        /// </summary>
        /// <param name="handler">Name of the handler with the listener</param>
        /// <param name="listener">Listener to be updated</param>
        /// <param name="positive">True if Positive key. False if Alternative</param>
        /// <param name="UIUpdater">Method to be called after key changed (can be null)</param>
        public void ChangeKey(string handler, string listener, bool positive, Action UIUpdater)
        {
            var h = GetInputHandler(handler);
            if (h != null)
            {
                var c = ListenForKeyToChange(h, listener, positive, UIUpdater);
                StartCoroutine(c);
            }

        }

        /// <summary>
        /// Changes specific listener key in runtime and calls method to update UI
        /// </summary>
        /// <param name="handler">Handler with the listener</param>
        /// <param name="listener">Listener to be updated</param>
        /// <param name="positive">True if Positive key. False if Alternative</param>
        /// <param name="UIUpdater">Method to be called after key changed (can be null)</param>
        public void ChangeKey(InputHandler handler, string listener, bool positive, Action UIUpdater)
        {
            if (handler != null)
            {
                var c = ListenForKeyToChange(handler, listener, positive, UIUpdater);
                StartCoroutine(c);
            }

        }

        IEnumerator ListenForKeyToChange(InputHandler handler, string listenerName, bool positive, Action UIUpdater)
        {
            KeyCode newKey = KeyCode.None;
            KeyCode oldKey;

            var listener = handler.GetListener(listenerName);
            if (listener != null)
            {
                while (newKey == KeyCode.None)
                {
                    foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKey(keyCode))
                        {
                            newKey = keyCode;
                            break;
                        }
                    }

                    yield return new WaitForEndOfFrame();
                }

                if (positive)
                {
                    oldKey = listener.Positive;
                    listener.Positive = newKey;
                }
                else
                {
                    oldKey = listener.Alternative;
                    listener.Alternative = newKey;
                }

                if (UIUpdater != null)
                {
                    UIUpdater.Invoke();
                }

                handler.ChangeKey(listenerName, oldKey, newKey);
                UpdateStack();
            }
        }

    }
}

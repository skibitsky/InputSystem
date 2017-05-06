﻿using System.Collections.Generic;
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

        // Collection of all inited InptuHandlers. Key = InptuHandler.Name
        Dictionary<string, InputHandler> AllInptuHandlers = new Dictionary<string, InputHandler>();

        // Singleton and handlers init stuff
        void Awake()
        {
            if (InputManager.instance == null)
            {
                InputManager.instance = this;

                InputHandler[] handlersToBeInited = GameObject.FindObjectsOfType<InputHandler>();
                foreach (var h in handlersToBeInited)
                {
                    InitNewInputHandler(h);
                }
            }
            else Destroy(this);
        }

        void Start()
        {
            UpdateKeyCodes();
        }

        // Loops through all InputHandlers in the stack (frotm the top to bottom) 
        // It's the only method which interacts with Unity Input System
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
                    if (ih.HardBlock)
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
                                if (handler.Block) break;
                            }
                        }
                    }
                }

                // Same here for Pressed
                if (Input.GetKey(key))
                {
                    ih = InputHandlersStack.Peek();
                    if (ih.HardBlock)
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
                                if (handler.Block) break;
                            }
                        }
                    }
                }

                // Same here for JustReleased
                if (Input.GetKeyUp(key))
                {
                    ih = InputHandlersStack.Peek();
                    if (ih.HardBlock)
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
                                if (handler.Block) break;
                            }
                        }
                    }
                }

            }


        }

        /// <summary>
        /// Updates a list of all used keys.
        /// </summary>
        public void UpdateKeyCodes()
        {
            KeyCodesToListen.Clear();
            foreach (var ih in InputHandlersStack)
            {
                if (KeyCodesToListen.Count == 0)
                    KeyCodesToListen = ih.GetAllKeyCodes();
                else
                    KeyCodesToListen.Concat(ih.GetAllKeyCodes());
            }
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
            else
                Debug.LogErrorFormat("Input Handler <b>{0}</b> already exists.", handler.Name);
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
        /// Adds passed InputHandler to the top of stack
        /// </summary>
        /// <param name="ih">InputHandler to be pushed</param>
        /// <returns>True if handler was added to the stack</returns>
        public bool AddInputHandlerToStack(InputHandler ih)
        {
            InputHandlersStack.Push(ih);
            UpdateKeyCodes();
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
                UpdateKeyCodes();
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
                UpdateKeyCodes();
            }      
        }

        /// <summary>
        /// Removes passed input handler from the stack
        /// </summary>
        /// <param name="handler">Handler to be removed</param>
        public void RemoveInputHandlerFromStack(InputHandler handler)
        {
            foreach (var h in InputHandlersStack)
            {
                if(h == handler && InputHandlersStack.Count != 0)
                {
                    InputHandlersStack.Pop();
                    UpdateKeyCodes();
                    break;
                }
            } 
        }

        /// <summary>
        /// Removes passed input handler from the stack
        /// </summary>
        /// <param name="handler">Name of the handler to be removed</param>
        public void RemoveInputHandlerFromStack(string handler)
        {
            foreach (var h in InputHandlersStack)
            {
                if (h.Name == handler && InputHandlersStack.Count != 0)
                {
                    InputHandlersStack.Pop();
                    UpdateKeyCodes();
                    break;
                }
            }
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
                UpdateKeyCodes();
            }
        }

    }
}

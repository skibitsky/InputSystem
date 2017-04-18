using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Salday.GameFramework.InputSystem
{
    public class InputManager : MonoBehaviour
    {
        // Singleton
        public static InputManager instance;

        // Stack of all active Input Handlers
        Stack<InputHandler> InputHandlers = new Stack<InputHandler>();

        // List of all keys used in active Handlers to loop through
        List<KeyCode> KeyCodesToListen = new List<KeyCode>();

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
                    ih = InputHandlers.Peek();
                    // If this handler HardBlock we have to work only with it
                    if (ih.HardBlock)
                    {
                        // Checking if listener has pressed key
                        if (ih.JustPressed.TryGetValue(key, out il))
                        {
                            // Invokes all listener's actions
                            if (il.Actions != null)
                                il.Actions.Invoke();
                        }
                    }
                    // If it wasn't hard blocked
                    else
                    {
                        // Loop through all handlers (from the top because it Stack)
                        foreach (var handler in InputHandlers)
                        {
                            // Checking if listener has pressed key
                            if (handler.JustPressed.TryGetValue(key, out il))
                            {
                                // Invokes all listener's actions
                                if (il.Actions != null)
                                    il.Actions.Invoke();
                                // If it has Block we won't go to the other handlers with this key
                                if (handler.Block) break;
                            }
                        }
                    }
                }

                // Same here for Pressed
                if (Input.GetKey(key))
                {
                    ih = InputHandlers.Peek();
                    if (ih.HardBlock)
                    {
                        if (ih.Pressed.TryGetValue(key, out il))
                        {
                            if (il.Actions != null)
                                il.Actions.Invoke();
                        }
                    }
                    else
                    {
                        foreach (var handler in InputHandlers)
                        {
                            if (handler.Pressed.TryGetValue(key, out il))
                            {
                                if (il.Actions != null)
                                    il.Actions.Invoke();
                                if (handler.Block) break;
                            }
                        }
                    }
                }

                // Same here for JustReleased
                if (Input.GetKeyUp(key))
                {
                    ih = InputHandlers.Peek();
                    if (ih.HardBlock)
                    {
                        if (ih.JustReleased.TryGetValue(key, out il))
                        {
                            if (il.Actions != null)
                                il.Actions.Invoke();
                        }
                    }
                    else
                    {
                        foreach (var handler in InputHandlers)
                        {
                            if (handler.JustReleased.TryGetValue(key, out il))
                            {
                                if (il.Actions != null)
                                    il.Actions.Invoke();
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
            foreach (var ih in InputHandlers)
            {
                if (KeyCodesToListen.Count == 0)
                    KeyCodesToListen = ih.GetAllKeyCodes();
                else
                    KeyCodesToListen.Concat(ih.GetAllKeyCodes());
            }
        }

        /// <summary>
        /// Adds new InputHandler to the top of stack
        /// </summary>
        /// <param name="ih">InputHandler to be pushed</param>
        public void AddInputHandler(InputHandler ih)
        {
            InputHandlers.Push(ih);
            UpdateKeyCodes();
        }

        /// <summary>
        /// Removes last added input handler from the top of stack
        /// </summary>
        public void RemoveInputHandler()
        {
            InputHandlers.Pop();
            UpdateKeyCodes();
        }

    }
}

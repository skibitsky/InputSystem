using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Salday.GameFramework.InputSystem
{
    public class InputHandler : MonoBehaviour
    {
        // Name of input handler represents its purpose and can be shown in settings for example
        public string Name = "Player Movement";
        // If true it InputManager will work only with this handler if it is on the top of stack
        public bool HardBlock = false;
        // Should InputManager stop on this handler if it contains called key
        public bool Block = true;

        #region Dictionaries
        // JustPressed (Input.GetKeyDown)
        public Dictionary<KeyCode, InputListener> JustPressed = new Dictionary<KeyCode, InputListener>();
        // Pressed (Input.GetKey)
        public Dictionary<KeyCode, InputListener> Pressed = new Dictionary<KeyCode, InputListener>();
        // JustReleased (Input.GetKeyUp)
        public Dictionary<KeyCode, InputListener> JustReleased = new Dictionary<KeyCode, InputListener>();
        #endregion

        #region Lists for Editor

        // Lists are used to fill default Handler values tight from Unity Editor
        [SerializeField]
        List<InputListener> JustPressedFromEditor = new List<InputListener>();
        [SerializeField]
        List<InputListener> PressedFromEditor = new List<InputListener>();
        [SerializeField]
        List<InputListener> JusReleasedFromEditor = new List<InputListener>();
        #endregion

        void Awake()
        {
            FillDictionaries();
        }

        /// <summary>
        /// Fills dictionaries with data from lists
        /// </summary>
        public void FillDictionaries()
        {
            foreach (var l in JustPressedFromEditor)
            {
                if (l.Positive != KeyCode.None) JustPressed.Add(l.Positive, l);
                if (l.Alternative != KeyCode.None) JustPressed.Add(l.Alternative, l);
            }

            foreach (var l in PressedFromEditor)
            {
                if (l.Positive != KeyCode.None) Pressed.Add(l.Positive, l);
                if (l.Alternative != KeyCode.None) Pressed.Add(l.Alternative, l);
            }

            foreach (var l in JusReleasedFromEditor)
            {
                if (l.Positive != KeyCode.None) JustReleased.Add(l.Positive, l);
                if (l.Alternative != KeyCode.None) JustReleased.Add(l.Alternative, l);
            }
        }

        /// <summary>
        /// Returns all KeyCodes used in all handler's dictionaries.
        /// </summary>
        /// <remarks> 
        /// Used in InputManager to update KeyCodesToListen list in 
        /// InputManager.UpdateKeyCodes()
        /// </remarks>
        public List<KeyCode> GetAllKeyCodes()
        {
            return new List<KeyCode>().Concat(JustPressed.Keys)
                .Concat(Pressed.Keys)
                .Concat(JustReleased.Keys)
                .ToList();
        }


        #region JustPressed
        /// <summary>
        /// Adds passed action to the InputListiner of specific name in 
        /// JustPressed dictionary.
        /// </summary>
        /// <param name="name">Name of InputListener</param>
        /// <param name="method">Method to be added to the listener</param>
        public void AddJustPressedAction(string name, Action method)
        {
            foreach (var l in JustPressed)
            {
                if (l.Value.Name == name)
                {
                    l.Value.Actions += method;
                    break;
                }
            }
        }

        /// <summary>
        /// Adds passed action to the InputListiner of specific key in 
        /// JustPressed dictionary.
        /// </summary>
        /// <param name="key">KeyCode of the key the method to be subscribed to</param>
        /// <param name="method">Method to be subscribed</param>
        public void AddJustPressedAction(KeyCode key, Action method)
        {
            if (JustPressed.ContainsKey(key))
                JustPressed[key].Actions += method;
            else
            {
                InputListener il = new InputListener(key);
                il.Actions += method;
                JustPressed.Add(key, il);
            }
        }

        /// <summary>
        /// Removes passed action from InputListener based on the name of listener
        /// </summary>
        /// <param name="name">Name of the listener</param>
        /// <param name="method">Method to be removed</param>
        public void RemoveJustPressedAction(string name, Action method)
        {
            foreach (var l in JustPressed)
            {
                if (l.Value.Name == name) l.Value.Actions -= method;
            }
        }

        /// <summary>
        /// Removes passed action from InputListener based on the KeyCode.
        /// </summary>
        /// <param name="key">KeyCode of the key the method was subscribed to</param>
        /// <param name="method">Method to be removed</param>
        public void RemoveJustPressedAction(KeyCode key, Action method)
        {
            if (JustPressed.ContainsKey(key))
                JustPressed[key].Actions -= method;
        }
        #endregion

        #region Pressed
        /// <summary>
        /// Adds passed action to the InputListiner of specific name in 
        /// Pressed dictionary.
        /// </summary>
        /// <param name="name">Name of InputListener</param>
        /// <param name="method">Method to be added to the listener</param>
        public void AddPressedAction(string name, Action method)
        {
            foreach (var l in Pressed)
            {
                if (l.Value.Name == name)
                {
                    l.Value.Actions += method;
                    break;
                }
            }
        }

        /// <summary>
        /// Adds passed action to the InputListiner of specific key in 
        /// Pressed dictionary.
        /// </summary>
        /// <param name="key">KeyCode of the key the method to be subscribed to</param>
        /// <param name="method">Method to be subscribed</param>
        public void AddPressedAction(KeyCode key, Action method)
        {
            if (Pressed.ContainsKey(key))
                Pressed[key].Actions += method;
            else
            {
                InputListener il = new InputListener(key);
                il.Actions += method;
                Pressed.Add(key, il);
            }
        }

        /// <summary>
        /// Removes passed action from InputListener based on the name of listener
        /// </summary>
        /// <param name="name">Name of the listener</param>
        /// <param name="method">Method to be removed</param>
        public void RemovePressedAction(string name, Action method)
        {
            foreach (var l in Pressed)
            {
                if (l.Value.Name == name) l.Value.Actions -= method;
            }
        }

        /// <summary>
        /// Removes passed action from InputListener based on the KeyCode.
        /// </summary>
        /// <param name="key">KeyCode of the key the method was subscribed to</param>
        /// <param name="method">Method to be removed</param>
        public void RemovePressedAction(KeyCode key, Action method)
        {
            if (Pressed.ContainsKey(key))
            {
                Pressed[key].Actions -= method;
            }

        }
        #endregion

        #region JustPressed
        /// <summary>
        /// Adds passed action to the InputListiner of specific name in 
        /// JustReleased dictionary.
        /// </summary>
        /// <param name="name">Name of InputListener</param>
        /// <param name="method">Method to be added to the listener</param>
        public void AddJustReleasedAction(string name, Action method)
        {
            foreach (var l in JustReleased)
            {
                if (l.Value.Name == name)
                {
                    l.Value.Actions += method;
                    break;
                }
            }
        }

        /// <summary>
        /// Adds passed action to the InputListiner of specific key in 
        /// JustReleased dictionary.
        /// </summary>
        /// <param name="key">KeyCode of the key the method to be subscribed to</param>
        /// <param name="method">Method to be subscribed</param>
        public void AddJustReleasedAction(KeyCode key, Action method)
        {
            if (JustReleased.ContainsKey(key))
                JustReleased[key].Actions += method;
            else
            {
                InputListener il = new InputListener(key);
                il.Actions += method;
                JustReleased.Add(key, il);
            }
        }

        /// <summary>
        /// Removes passed action from InputListener based on the name of listener
        /// </summary>
        /// <param name="name">Name of the listener</param>
        /// <param name="method">Method to be removed</param>
        public void RemoveJustReleasedAction(string name, Action method)
        {
            foreach (var l in JustReleased)
            {
                if (l.Value.Name == name) l.Value.Actions -= method;
            }
        }

        /// <summary>
        /// Removes passed action from InputListener based on the KeyCode.
        /// </summary>
        /// <param name="key">KeyCode of the key the method was subscribed to</param>
        /// <param name="method">Method to be removed</param>
        public void RemoveJustReleasedAction(KeyCode key, Action method)
        {
            if (JustReleased.ContainsKey(key))
                JustReleased[key].Actions -= method;
        }
        #endregion
    }
}
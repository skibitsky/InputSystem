using System;
using UnityEngine;

namespace Salday.GameFramework.InputSystem
{
    [Serializable]
    public class InputListener
    {
        public string Name;
        public KeyCode Positive;
        public KeyCode Alternative;
        public Action Actions;

        public InputListener(KeyCode key)
        {
            this.Name = "Untitled";
            this.Positive = key;
        }

        public InputListener(string name, KeyCode key)
        {
            this.Name = name;
            this.Positive = key;
        }
    }
}
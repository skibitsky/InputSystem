using System;
using UnityEngine;
using System.Xml.Serialization;

namespace Salday.GameFramework.InputSystem
{
    [Serializable]
    public class InputListener
    {
        public string Name;
        public KeyCode Positive;
        public KeyCode Alternative;
        [XmlIgnore]
        public bool Invoked = false; // True if it has been already invoked this frame
        [XmlIgnore]
        public Action Actions;

        public InputListener() { }

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
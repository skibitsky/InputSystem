using System;
using UnityEngine;
using System.Xml.Serialization;

namespace Skibitsky.InputSystem
{
    [Serializable]
    public class InputListener
    {
        public string Name = "Untiteled";
        public KeyCode Positive;
        public KeyCode Alternative;
        [XmlIgnore]
        [HideInInspector]
        public bool Invoked = false; // True if it has been already invoked this frame
        [XmlIgnore]
        public Action Actions;

        public InputListener() { }

        public InputListener(KeyCode key)
        {
            this.Positive = key;
        }

        public InputListener(string name, KeyCode key)
        {
            this.Name = name;
            this.Positive = key;
        }
    }
}
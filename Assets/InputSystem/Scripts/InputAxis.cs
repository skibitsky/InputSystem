using System.Xml.Serialization;
using UnityEngine;

namespace Skibitsky.InputSystem
{
    [System.Serializable]
    public class InputAxis
    {
        // Names of axis in Unity InputManager:
        [XmlIgnore]
        const string MOUSE_HORIZONTAL = "Mouse X";
        [XmlIgnore]
        const string MOUSE_VERTICAL = "Mouse Y";
        [XmlIgnore]
        const string MOUSE_SCROLLWHEEL = "Mouse ScrollWheel";

        public string Name = "Untiteled";
        public InputAxisType Type;

        [XmlIgnore]
        [HideInInspector]
        public bool AlreadyUsed = false;

        public InputAxis() { }

        public string GetAxisName()
        {
            switch(Type)
            {
                case InputAxisType.MouseHorizontal:
                    return MOUSE_HORIZONTAL;
                case InputAxisType.MouseVertical:
                    return MOUSE_VERTICAL;
                case InputAxisType.MouseScrollWheel:
                    return MOUSE_SCROLLWHEEL;
                default:
                    return "Wrong Axis Type";
            }
        }
    }

}

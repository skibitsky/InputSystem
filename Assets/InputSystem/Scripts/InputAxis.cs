using System.Xml.Serialization;
using UnityEngine;

namespace Salday.GameFramework.InputSystem
{
    [System.Serializable]
    public class InputAxis
    {
        [XmlIgnore]
        const string MOUSE_HORIZONTAL = "Mouse X";
        [XmlIgnore]
        const string MOUSE_VERTICAL = "Mouse Y";

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
                default:
                    return "Wrong Axis Type";
            }
        }
    }

}

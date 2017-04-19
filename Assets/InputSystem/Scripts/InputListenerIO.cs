using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Salday.GameFramework.InputSystem
{
    //(2017 - 04 - 18)
    //TODO this thing
    public class InputListenerIO : MonoBehaviour
    {
        public static InputListener ReadListener(string name)
        {
            XmlSerializer xs = new XmlSerializer(typeof(InputListener));

            using (var sr = new StreamReader(string.Format(@"c:\temp\{0}.xml", name)))
            {
                return xs.Deserialize(sr) as InputListener;
            }
        }

    }
}

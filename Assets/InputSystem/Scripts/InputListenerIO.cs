using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Salday.GameFramework.InputSystem
{
    public class InputListenerIO
    {
        //(2017-04-26)
        //TODO reading from file

        //public static InputListener ReadHandler(string name)
        //{
        //    XmlSerializer xs = new XmlSerializer(typeof(InputListener));

        //    using (var sr = new StreamReader(string.Format(@"c:\temp\{0}.xml", name)))
        //    {
        //        return xs.Deserialize(sr) as InputListener;
        //    }
        //}

        /// <summary>
        /// Saves gived handler to the XML into _Data/Xml/InputSettings.
        /// </summary>
        /// <param name="handler">Handler to be saved</param>
        public static void WriteHandler(InputHandler handler)
        {
            var folder = @"Xml\InputSettings";

            var path = Path.Combine(Application.dataPath, folder);

            DirectoryInfo folderInf = new DirectoryInfo(path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filePath = Path.Combine(path, handler.Name) + ".xml";

            using (var sw = new StreamWriter(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(InputListener));
                foreach (var il in handler.Pressed.Values)
                {
                    serializer.Serialize(sw, il);
                }

                sw.Close();
            }

        }

    }
}

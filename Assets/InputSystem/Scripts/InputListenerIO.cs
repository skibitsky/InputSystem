using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Salday.GameFramework.InputSystem
{
    public class InputListenerIO
    {
        //(2017-04-26)
        //TODO reading from the file.

        /// <summary>
        /// Saves gived handler to the XML into _Data/Xml/InputSettings.
        /// </summary>
        /// <param name="handler">Handler to be saved</param>
        public static void WriteHandler(InputHandler handler)
        {
            var folder = @"Xml\InputSettings";

            var path = Path.Combine(Application.dataPath, folder);

            var folderInf = new DirectoryInfo(path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filePath = Path.Combine(path, handler.Name) + ".xml";

            using (var sw = new StreamWriter(filePath))
            {
                var serializer = new XmlSerializer(typeof(SavingHandler));

                var sh = new SavingHandler(handler.JustPressed, handler.Pressed, handler.JustReleased);

                serializer.Serialize(sw, sh);

                sw.Close();
            }

        }

    }


    // We need this class to serialize it. 
    // We can't serialize dictionaries from the handler that's why we're converting them to the lists
    [Serializable]
    public class SavingHandler
    {
        [XmlArray("JustPressed")]
        public List<InputListener> JustPressed = new List<InputListener>();
        [XmlArray("Pressed")]
        public List<InputListener> Pressed = new List<InputListener>();
        [XmlArray("JustReleased")]
        public List<InputListener> JustReleased = new List<InputListener>();

        /// <summary>
        /// Dont't use it. It's required by XmlSerializer.
        /// </summary>
        public SavingHandler()
        {

        }

        public SavingHandler(Dictionary<KeyCode, InputListener> justPressed,
            Dictionary<KeyCode, InputListener> pressed,
            Dictionary<KeyCode, InputListener> justReleased
            )
        {
            foreach (var item in justPressed)
                JustPressed.Add(item.Value);

            foreach (var item in pressed)
                Pressed.Add(item.Value);

            foreach (var item in justReleased)
                JustReleased.Add(item.Value);
        }
    }
}

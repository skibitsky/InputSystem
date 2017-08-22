using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Salday.InputSystem
{
    public class InputSaver
    {

        /// <summary>
        /// Saves gived handler to the XML into _Data/Xml/InputSettings.
        /// </summary>
        /// <param name="handler">Handler to be saved</param>
        public static void WriteHandler(IInputHandler handler)
        {
            if (Application.isEditor)
                return;

            var folder = @"Xml\InputSettings";

            var path = Path.Combine(Application.dataPath, folder);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filePath = Path.Combine(path, handler.Name) + ".xml";

            using (var sw = new StreamWriter(filePath))
            {
                var serializer = new XmlSerializer(typeof(SavingHandler));

                var sh = new SavingHandler(handler.JustPressed, 
                    handler.Pressed, 
                    handler.JustReleased, 
                    handler.Axes);

                serializer.Serialize(sw, sh);

                sw.Close();
            }

        }

        /// <summary>
        /// Returns SavingHandler with all InputListener of asked handler.
        /// Null if handler doesn't have a file
        /// </summary>
        /// <param name="name">Handlerto be readed</param>
        public static SavingHandler ReadHandler(string name)
        {
            var folder = @"Xml\InputSettings";

            var path = Path.Combine(Application.dataPath, folder);

            if (!Directory.Exists(path)) return null;

            var filePath = Path.Combine(path, name) + ".xml";
            if (!File.Exists(filePath)) return null;

            using (var sr = new StreamReader(filePath))
            {
                var serializer = new XmlSerializer(typeof(SavingHandler));

                var result = serializer.Deserialize(sr) as SavingHandler;

                return result;
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
        [XmlArray("Axes")]
        public List<InputAxis> Axes = new List<InputAxis>();

        /// <summary>
        /// Dont't use it. It's required by XmlSerializer.
        /// </summary>
        public SavingHandler() { }

        public SavingHandler(Dictionary<KeyCode, InputListener> justPressed,
            Dictionary<KeyCode, InputListener> pressed,
            Dictionary<KeyCode, InputListener> justReleased,
            List<InputAxis> axes
            )
        {
            foreach (var item in justPressed.Values)
                if(!JustPressed.Contains(item))
                    JustPressed.Add(item);

            foreach (var item in pressed.Values)
                if(!Pressed.Contains(item))
                    Pressed.Add(item);

            foreach (var item in justReleased.Values)
                if(!Pressed.Contains(item))
                    JustReleased.Add(item);

            Axes = axes;
        }
    }
}

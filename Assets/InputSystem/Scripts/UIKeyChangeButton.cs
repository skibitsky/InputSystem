using UnityEngine.UI;
using UnityEngine;
using System;

namespace Salday.InputSystem
{
    public class UIKeyChangeButton : MonoBehaviour
    {
        [Header("Button settings")]
        [SerializeField]
        Text ButtonText;
        [SerializeField]
        string EnterKeyText = "Enter new key...";

        [Header("Input settings")]
        [SerializeField]
        string HandlerName;
        [SerializeField]
        string ListenerName;
        [SerializeField]
        PositiveAlternative positiveAlternative;

        IInputHandler _handler;

        void Awake()
        {
            if (ButtonText == null) ButtonText = GetComponentInChildren<Text>();
        }

        void Start()
        {
            _handler = InputManager.instance.GetInputHandler(HandlerName);
            UpdateButtonText();
        }

        void UpdateButtonText()
        {
            if (_handler != null)
            {
                switch (positiveAlternative)
                {
                    case PositiveAlternative.Positive:
                        ButtonText.text = _handler.GetListener(ListenerName).Positive.ToString();
                        break;
                    case PositiveAlternative.Alternative:
                        ButtonText.text = _handler.GetListener(ListenerName).Alternative.ToString();
                        break;
                }
            }
        }

        // Should be subscribed to the button click action
        public void ButtonClick()
        {
            InputManager.instance.ChangeKey(HandlerName, ListenerName, Convert.ToBoolean(positiveAlternative), UpdateButtonText);
            ButtonText.text = EnterKeyText;
        }


        enum PositiveAlternative
        {
            Alternative = 0,
            Positive
        }
    }

}
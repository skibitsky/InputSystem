using UnityEngine;
using Skibitsky.InputSystem;

public class Chat : MonoBehaviour
{

    public void EnableChat(bool value)
    {
        if (value) InputManager.instance.AddInputHandlerToStack("Chat");
        else InputManager.instance.RemoveInputHandlerFromStack();
    }
}
 
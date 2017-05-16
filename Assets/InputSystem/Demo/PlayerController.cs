using UnityEngine;
using Salday.GameFramework.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputHandler inputHandler;

    [SerializeField]
    float speed = 0.2f;

    void Start()
    {
        // Getting InputHandler which controls player movement input
        inputHandler = GetComponent<InputHandler>();

        // It can also be accessed by asking it right from InputManager using a name
        // So you can get any InptuHandler without having ref to GameObject with the handler component
        //inputHandler = InputManager.instance.GetInputHandler("Player Movement");

        // Adding action to the "Movement Right" (seted up from editor)
        inputHandler.AddPressedAction("Movement Right", MoveRight);

        // Addiing our handler to the Input Manager's stack of handlers
        InputManager.instance.AddInputHandlerToStack(inputHandler);

        // Let's try one more action after handler was added to the stack
        inputHandler.AddPressedAction("Movement Left", MoveLeft);

        // It's possible to add new actions by passing KeyCode or Listener name
        // which wasn't seted up on editor. 
        inputHandler.AddJustPressedAction(KeyCode.Mouse0, () => Debug.Log("Click!"));

        // And you can always delete any added action from listener
        // accessing it by KeyCode or Name
        //inputHandler.RemovePressedAction(KeyCode.D, MoveRight);
        
    }

    void MoveLeft()
    {
        transform.position += Vector3.left * speed;
    }

    void MoveRight()
    {
        transform.position += Vector3.right * speed;
    }
}

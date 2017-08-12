using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Salday.InputSystem
{
    // Needed to protect InputManager's InputHandlersStack if Manager was
    // disabled or deleted. Very usefull if you have InputHandler in the stack
    // while DontDestroyOnLoad because each scene has unique InputManager.
    public class StackProtector : MonoBehaviour
    {
        public Stack<InputHandler> ProtectedStack = new Stack<InputHandler>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
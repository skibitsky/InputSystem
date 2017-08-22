using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Salday.InputSystem
{
    // Needed to protect InputManager's InputHandlersStack if Manager was
    // disabled or deleted. Very usefull if you have IInputHandler in the stack
    // while DontDestroyOnLoad because each scene has unique InputManager.
    public class StackProtector : MonoBehaviour
    {
        public Stack<IInputHandler> ProtectedStack = new Stack<IInputHandler>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
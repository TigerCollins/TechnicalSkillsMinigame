using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField]
    InputAction pauseInput;

    void Start()
    {
        SetupInput();
    }

    void SetupInput()
    {
        //Adds function to input. Only calls when performed (pressed, not held or up)
        pauseInput.performed += UIHandler.instance.PauseInput;

        //Input functions enabled (this is needed or the input wont work)
        pauseInput.Enable();
    }
}

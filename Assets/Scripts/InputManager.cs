using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    
    private bool _interactPressed;
    private bool _submitPressed;

    public static InputManager GetInstance() => _instance;
    
    private void Awake()
    {
        if (_instance is not null)
            Debug.LogWarning($"More than one instance of {this} found in the scene");
        _instance = this;
    }

    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.started)
            _interactPressed = true;

        else if (context.canceled)
            _interactPressed = false;
    }


    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.started)
            _submitPressed = true;
        
        else if (context.canceled)
            _submitPressed = false;
    }
    
    public bool GetInteractPressed() 
    {
        var result = _interactPressed;
        _interactPressed = false;
        return result;
    }

    public bool GetSubmitPressed() 
    {
        var result = _submitPressed;
        _submitPressed = false;
        return result;
    }

    public void RegisterSubmitPressed() 
    {
        _submitPressed = false;
    }
}

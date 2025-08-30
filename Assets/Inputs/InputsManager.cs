using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputsManager : MonoBehaviour
{
    public static event Action<Vector2> OnNavigateEvent;
    public static event Action OnSubmitEvent;
    public static event Action OnCancelEvent;
    
    private InputsMap _inputs;
    private void OnEnable()
    {
        _inputs = new InputsMap();
        Subscribe();
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
        Unsubscribe();
    }

    private void Subscribe()
    {
        _inputs.UI.Navigate.performed += OnNavigate;
        _inputs.UI.Submit.performed += OnSubmit;
        _inputs.UI.Cancel.performed += OnCancel;
    }
    
    private void Unsubscribe()
    {
        _inputs = new InputsMap();
                
        _inputs.UI.Navigate.performed -= OnNavigate;
        _inputs.UI.Submit.performed -= OnSubmit;
        _inputs.UI.Cancel.performed -= OnCancel;
    }

    private void OnNavigate(InputAction.CallbackContext context)
    {
        OnNavigateEvent?.Invoke(context.ReadValue<Vector2>());
    }
    
    private void OnSubmit(InputAction.CallbackContext context)
    {
        OnSubmitEvent?.Invoke();
    }
    
    private void OnCancel(InputAction.CallbackContext context)
    {
        OnCancelEvent?.Invoke();
    }
    
}

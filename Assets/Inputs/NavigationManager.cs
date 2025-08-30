using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputsManager.OnCancelEvent += OnCancel;
    }

    private void OnDestroy()
    {
        InputsManager.OnCancelEvent -= OnCancel;
    }

    public void OnNavigate(Vector2 direction)
    {
        var current = EventSystem.current.currentSelectedGameObject;
        if (current == null) return;
    
        var sel = current.GetComponent<Selectable>();
        if (sel == null) return;
    
        Selectable next = null;
    
        if (direction.x > 0) next = sel.FindSelectableOnRight();
        else if (direction.x < 0) next = sel.FindSelectableOnLeft();
        else if (direction.y > 0) next = sel.FindSelectableOnUp();
        else if (direction.y < 0) next = sel.FindSelectableOnDown();
    
        if (next != null)
        {
            next.Select();
        }
    }

    public void SelectGameObject(Selectable selectable)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectable.gameObject);
    }
    
    public void OnSubmit()
    {
        var current = EventSystem.current.currentSelectedGameObject;
        if (current != null)
        {
            var button = current.GetComponent<Button>();
            button?.onClick.Invoke();

            var onSubmit = current.GetComponent<ISubmit>();
            if(onSubmit != null)
                onSubmit.OnSubmit();
        }
    }
    
    public void OnCancel()
    {
        var current = EventSystem.current.currentSelectedGameObject;
        if (current != null)
        {
            var onCancel = current.GetComponent<ICancel>();
            if (onCancel != null)
                onCancel.OnCancel();
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

public class CancelHandler : MonoBehaviour, ICancel
{
    [SerializeField] private UnityEvent Event;
    public void OnCancel()
    {
        Event.Invoke();
    }
}

using UnityEngine;
using UnityEngine.Events;

public class SubmitHandler : MonoBehaviour, ISubmit
{
    [SerializeField] private UnityEvent Event;
    public void OnSubmit()
    {
        Event.Invoke();
    }
}

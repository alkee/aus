using UnityEngine;
using UnityEngine.Events;

public class InitEventTrigger : MonoBehaviour
{
    [TextArea]
    public string JustComment; // because component has no place to comment

    public UnityEvent OnAwake;
    public UnityEvent OnStart;

    void Awake()
    {
        if (OnAwake != null) OnAwake.Invoke();
    }

    // Use this for initialization
    void Start()
    {
        if (OnStart != null) OnStart.Invoke();
    }
}

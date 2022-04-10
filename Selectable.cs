using EasyButtons;
using UnityEngine;
using UnityEngine.Events;


//For selecting UI elements in VR

public enum SELECT_EVENT
{
    IDLE,
    ON_HOVER_START,
    ON_HOVER_STAY,
    ON_HOVER_END,
}

public class Selectable : MonoBehaviour
{
    [SerializeField] UnityEvent onSelectEvent;
    [SerializeField] UnityEvent onHoverBegin;
    [SerializeField] UnityEvent onHoverEnd;

    public SELECT_EVENT currentState = SELECT_EVENT.IDLE;

    void Awake() { }

    void Update() { }

    [Button]
    public void Select()
    {
        if (this.enabled)
            onSelectEvent.Invoke();
    }

    public void OnHoverBegin()
    {
        if (enabled)
        {
            if (currentState == SELECT_EVENT.IDLE || currentState == SELECT_EVENT.ON_HOVER_END)
            {
                currentState = SELECT_EVENT.ON_HOVER_START;
                onHoverBegin.Invoke();
                currentState = SELECT_EVENT.ON_HOVER_STAY;
            }
        }
    }


    public void OnHoverEnd()
    {
        if (this.enabled)
        {
            if (currentState == SELECT_EVENT.ON_HOVER_STAY || currentState == SELECT_EVENT.ON_HOVER_START)
            {
                currentState = SELECT_EVENT.ON_HOVER_END;
                onHoverEnd.Invoke();
                currentState = SELECT_EVENT.IDLE;
            }
        }
    }

    public void Disable()
    {
        onHoverEnd.Invoke();
        //currentState = SELECT_EVENT.IDLE;
        enabled = false;
    }
}

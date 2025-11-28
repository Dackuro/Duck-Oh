using UnityEngine;

[RequireComponent(typeof(Events))]
public class GameEventTimeline : MonoBehaviour
{
    [Header("References")]
    public TimedEvent[] events;

    [Header("Variables")]
    private float elapsed = 0f;
    private int index = 0;


    void Update()
    {
        if (!GameManager.instance.gameStarted)
            return;

        elapsed += Time.deltaTime;
        
        // Trigger events in order
        while (index < events.Length && elapsed >= events[index].triggerTime)
        {
            events[index].onTrigger.Invoke();
            index++;
        }
    }
}

[System.Serializable]
public class TimedEvent
{
    [Header("Evento")]
    public float triggerTime;
    public UnityEngine.Events.UnityEvent onTrigger;
}
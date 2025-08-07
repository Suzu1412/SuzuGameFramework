using Cysharp.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Objects/Events/Event/Void Event", fileName = "Void Event")]
public class VoidGameEvent : ScriptableObject
{
    protected event Action OnEventRaised;

#if UNITY_EDITOR
    [SerializeField] protected bool _logEvent = true;
    private const int MaxHistory = 10;
    protected readonly List<string> _eventHistory = new(MaxHistory);
    protected Dictionary<UnityEngine.Object, int> _senderCounts = new Dictionary<UnityEngine.Object, int>();

    public List<string> EventHistory => _eventHistory;
    public Dictionary<UnityEngine.Object, int> SenderCounts => _senderCounts;
#endif

    /// <summary>
    /// Raise the event and notify all listeners.
    /// Needs to be passed 'This' as parameter to log who activated the event
    /// </summary>
    public void RaiseEvent(UnityEngine.Object sender)
    {
        if (OnEventRaised != null)
        {
            try
            {
                OnEventRaised.Invoke();

#if UNITY_EDITOR
                if (_logEvent)
                {
                    LogEvent(sender);
                }
#endif
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while invoking event {name}: {e}");
            }
        }
        else
        {
            Debug.LogWarning($"Event {name} was raised but has no listeners.");
        }
    }

    /// <summary>
    /// Registers a listener for this event.
    /// </summary>
    internal void Register(Action onEvent)
    {
        if (onEvent == null) return;

        // Ensure the same method isn't added twice
        if (OnEventRaised != null && OnEventRaised.GetInvocationList().Contains(onEvent))
        {
            Debug.LogWarning($"Event already registered: {onEvent.Method.Name} on {onEvent.Target}");
            return;
        }
        OnEventRaised += onEvent;
    }

    /// <summary>
    /// Unregisters a listener from this event.
    /// </summary>
    internal void Unregister(Action onEvent)
    {
        if (onEvent == null) return;

        OnEventRaised -= onEvent;
    }

#if UNITY_EDITOR
    protected void LogEvent(UnityEngine.Object sender)
    {
        if (!_senderCounts.TryGetValue(sender, out var count))
            _senderCounts[sender] = 1;
        else
            _senderCounts[sender] = count + 1;

        // Use ZString.Format instead of string interpolation or StringBuilder
        string log = ZString.Format("{0} triggered event at {1:HH:mm:ss.fff}", sender.name, System.DateTime.Now);

        if (_eventHistory.Count >= MaxHistory)
        {
            _eventHistory.RemoveAt(_eventHistory.Count - 1);
        }

        _eventHistory.Insert(0, log);
    }
#endif

    public void RaiseFromEditor(UnityEngine.Object sender)
    {
        RaiseEvent(sender);
    }
}
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public delegate void EventCallback<T>(T data); 

public class EventManager : MonoBehaviour {

    private Dictionary <Type, CustomEvent> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance {
        get {
            if (!eventManager) {
                eventManager = FindObjectOfType (typeof (EventManager)) as EventManager;
                if (!eventManager) {
                    Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
                } else {
                    eventManager.Init (); 
                }
            }
            return eventManager;
        }
    }

    void Init () {
        if (eventDictionary == null) {
            eventDictionary = new Dictionary<Type, CustomEvent>();
        }
    }

    public static void StartListening <T> (EventCallback<T> listener) {
        CustomEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue (typeof(T), out thisEvent)) {
            thisEvent.AddListener(listener);
        } else {
            thisEvent = new CustomEvent ();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add (typeof(T), thisEvent);
        }
    }

    public static void StopListening  <T> (EventCallback<T> listener) {
        if (eventManager == null) {
            return;
        }
        CustomEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue (typeof(T), out thisEvent)) {
            thisEvent.RemoveListener (listener);
        }
    }

    public static void TriggerEvent <T> (T data) {
        CustomEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue (typeof(T), out thisEvent)) {
            thisEvent.Invoke (data);
        }
    }
}

public class CustomEvent {

    public List<Delegate> listeners = new List<Delegate>();

    public void AddListener (Delegate listener) {
        listeners.Add(listener);
    }

    public void RemoveListener(Delegate listener) {
        listeners.Remove(listener);
    }
    
    public void Invoke <T>(T data) {
        foreach (var listener in listeners) {
            listener.DynamicInvoke(data);
        }
    }
}

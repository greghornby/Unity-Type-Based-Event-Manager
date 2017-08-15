using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public delegate void EventCallback<T>(T data); 

public class EventManager : MonoBehaviour {

    private class GameEvent {
        public List<Delegate> listeners = new List<Delegate>();
        public void AddListener(Delegate listener) {
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

    private static Dictionary <Type, GameEvent> eventDictionary = new Dictionary<Type, GameEvent>();
    private static string eventSceneName;

    void Start() {
        eventSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        eventDictionary = new Dictionary<Type, GameEvent>();
    }

    private static void CheckSceneMatches() {
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentSceneName != eventSceneName) {
            throw new Exception("EventManager has not been initialized for the current scene");
        }
    }

    public static void AddEventListener<T>(EventCallback<T> listener) {
        CheckSceneMatches();
        GameEvent thisEvent = null;
        if (eventDictionary.TryGetValue (typeof(T), out thisEvent)) {
            thisEvent.AddListener(listener);
        } else {
            thisEvent = new GameEvent ();
            thisEvent.AddListener(listener);
            eventDictionary.Add (typeof(T), thisEvent);
        }
    }

    public static List<Delegate> GetListenersOfEvent<T>() {
        CheckSceneMatches();
        GameEvent thisEvent = null;
        if (eventDictionary.TryGetValue (typeof(T), out thisEvent)) {
            return thisEvent.listeners;
        } else {
            return null;
        }
    }

    public static void RemoveEventListener<T>(EventCallback<T> listener) {
        CheckSceneMatches();
        GameEvent thisEvent = null;
        if (eventDictionary.TryGetValue(typeof(T), out thisEvent)) {
            thisEvent.RemoveListener (listener);
        }
    }

    public static void RemoveEvent<T>() {
        CheckSceneMatches();
        eventDictionary.Remove(typeof(T));
    }

    public static void RemoveAllEvents() {
        CheckSceneMatches();
        eventDictionary = new Dictionary<Type, GameEvent>();
    }

    public static void TriggerEvent<T>(T data) {
        CheckSceneMatches();
        GameEvent thisEvent = null;
        if (eventDictionary.TryGetValue (typeof(T), out thisEvent)) {
            thisEvent.Invoke (data);
        }
    }
}

# Unity-Type-Based-Event-Manager
An Event Manager script for unity where you register events with Struct/Class types, then trigger them with instances of those Struct/Class types.

## Initializing the EventManager.

To initialize the EventManager, you must place it on a single object (such as an empty Game Controller object) in the scene. This is so in each scene that relies on the EventManager it can reset the registered event listeners.

## Defining an event

You can define an event as a struct, or a class, or any data type.

`events.cs`
```c#
public struct MoveEvent {
    public int distance;
}
```

## Add Event Listener

Say we have a cube object that we want to move when an event is triggered. To register a listener for it, we'd do like so

`Cube.cs`
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    void Start () {
        EventManager.AddEventListener<MoveEvent>((data) => {
            Move(data.distance);
        });
    }
    
    void Move(int distance) {
        var transform = this.GetComponent<Transform>();
        var position = transform.position;
        position.x += distance;
        transform.position = position;
    }
}
```

## Remove Event Listener

You may also remove an event listener using the Event type, and a reference to the listener you used to attach it. So if you wish to remove an event programatically, do not use an anonymous callback function.

`Cube.cs`
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    void Start () {
        EventManager.AddEventListener<MoveEvent>(DoMoveEvent);
        EventManager.RemoveEventListener<MoveEvent>(DoMoveEvent);
    }

    void DoMoveEvent(MoveEvent data) {
        var transform = this.GetComponent<Transform>();
        var position = transform.position;
        position.x += data.distance;
        transform.position = position;
    }
}
```

## Trigger Event
And finally, to trigger the event, we just pass a new instance of the event type to the Trigger method.

`GameController.cs`
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController> {

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            EventManager.TriggerEvent (new MoveEvent{distance=1});
        }
    }
}
```

## Other Methods

`GetListenersOfEvent<T>()` => `List<Delegate>` -- Get a list of all listeners for an event. Returns a new empty list if a listener was never added to the event.

`RemoveEvent<T>()` => `void` -- Remove all listeners for an event, and remove the event itself from the event dictionary.

`RemoveAllEvents<T>()` => `void` -- Remove all listeners for all events. Completely reset the event dictionary

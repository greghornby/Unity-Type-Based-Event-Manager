# Unity-Type-Based-Event-Manager
An Event Manager script for unity where you register events with Struct/Class types, then trigger them with instances of those Struct/Class types

## Defining an event

You can define an event as a struct, or a class, or any data type.
`events.cs`
```c#
public struct MoveEvent {
    public int distance;
}
```

## Register Listener To An Event

Say we have a cube object that we want to move when an event is triggered. To register a listener for it, we'd do like so
`Cube.cs`
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    // Use this for initialization
    void Start () {
        EventManager.StartListening<MoveEvent>((data) => {
            Move(data.distance);
        });
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    void Move(int distance) {
        var transform = this.GetComponent<Transform>();
        var position = transform.position;
        position.x += distance;
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

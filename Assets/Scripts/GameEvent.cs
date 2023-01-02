using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObjects/GameEventScriptableObject", order = 3)]
public class GameEvent : ScriptableObject
{
    private HashSet<GameEventListener> listeners = new HashSet<GameEventListener>();

    public void Invoke()
    {
        foreach (GameEventListener listener in listeners)
        {
            listener.RaiseEvent();
        }
    }

    public void Register(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void Deregister(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RunManager", menuName = "ScriptableObjects/RunManagerScriptableObject", order = 2)]
public class RunManager : ScriptableObject
{
    public float CurrentTime { get; private set; }
    public bool Paused { get; private set; }

    [SerializeField] private ThrowManager throwManager;
    [SerializeField] private GameEvent scoredEvent;

    private void OnEnable()
    {
        CurrentTime = 0;
        Paused = false;
    }

    public void Scored()
    {
        scoredEvent.Invoke();
        Paused = true;
    }

    public void AddTime(float timeDelta)
    {
        CurrentTime += timeDelta;
    }

    public void Reset()
    {
        CurrentTime = 0;
        Paused = false;
    }

    public void Pause()
    {
        Paused = true;
    }

    public void UnPause()
    {
        Paused = false;
    }
}

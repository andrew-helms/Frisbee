using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrowManager", menuName = "ScriptableObjects/ThrowManagerScriptableObject", order = 1)]
public class ThrowManager : ScriptableObject
{
    public bool AimLock { get; private set; }
    public bool HoldingDisc { get; private set; }
    public bool StartedThrow { get; private set; }

    public Vector3 BankAngle;

    private void OnEnable()
    {
        AimLock = false;
        HoldingDisc = true;
        BankAngle.Set(0, 0, 0);
        StartedThrow = false;
    }

    public void StartThrow()
    {
        AimLock = true;
        StartedThrow = true;
    }

    public void EndThrow()
    {
        AimLock = false;
        HoldingDisc = false;
        StartedThrow = false;
    }

    public void PickUpDisc()
    {
        HoldingDisc = true;
    }

    public void Reset()
    {
        AimLock = false;
        HoldingDisc = true;
        BankAngle.Set(0, 0, 0);
        StartedThrow = false;
    }
}

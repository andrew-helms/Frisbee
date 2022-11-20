using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrowManager", menuName = "ScriptableObjects/ThrowManagerScriptableObject", order = 1)]
public class ThrowManager : ScriptableObject
{
    public bool AimLock { get; private set; }
    public bool HoldingDisc { get; private set; }

    public Vector3 BankAngle;

    private void OnEnable()
    {
        AimLock = false;
        HoldingDisc = true;
        BankAngle.Set(0, 0, 0);
    }

    public void StartThrow()
    {
        AimLock = true;
    }

    public void EndThrow()
    {
        AimLock = false;
        HoldingDisc = false;
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
    }
}

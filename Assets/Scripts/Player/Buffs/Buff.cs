using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Buff : ScriptableObject
{
    [SerializeField] _buffType thisBuff;
    [SerializeField] float _buffMultiplier;

    private enum _buffType
    {
        DAMAGEBOOST, SPEEDBOOST, DOUBLEJUMP
    }
}

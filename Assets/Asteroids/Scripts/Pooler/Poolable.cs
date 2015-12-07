using UnityEngine;
using System;


// Extends MonoBehaviour, so it's a Component. Makes an owner poolable.
[Serializable]
public class Poolable : MonoBehaviour
{
    public bool isPooled;   // Flag for checks.
}
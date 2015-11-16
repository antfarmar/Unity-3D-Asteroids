using UnityEngine;
using System.Collections;


// Extends MonoBehaviour, so it's a Component. Makes an owner poolable.
public class Poolable : MonoBehaviour
{
    public string key;      // For dictionary. Maps to a Pool.
    public bool isPooled;   // Flag for checks.
}
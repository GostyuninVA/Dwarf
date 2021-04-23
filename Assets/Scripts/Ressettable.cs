using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ressettable : MonoBehaviour
{
    public UnityEvent OnReset;

    public void Reset()
    {
        OnReset.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private float _sidewaysMotion = 0.0f;
    [SerializeField] private Vector3 _virtualAccel;//testing
    private bool _keyRopeUp;//test
    private bool _keyRopeDown;//test

    public bool KeyRopeUp
    {
        get
        {
            return _keyRopeUp;
        }
    }

    public bool KeyRopeDown
    {
        get
        {
            return _keyRopeDown;
        }
    }

    public float sidewaysMotion
    {
        get
        {
            return _sidewaysMotion;
        }
    }

    private void Update()
    {
         _virtualAccel = Input.mousePosition.normalized;//testing

        //Vector3 accel = Input.acceleration;

        _sidewaysMotion = _virtualAccel.x * 0.5f;

        _keyRopeUp = Input.GetKey(KeyCode.W);
        _keyRopeDown = Input.GetKey(KeyCode.S);

    }
}

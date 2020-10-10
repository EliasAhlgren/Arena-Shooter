using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public BaseState(GameObject go)
    {
        this.gameObject = go;
        this.transform = go.transform;
    }

    protected GameObject gameObject;
    protected Transform transform;

    public abstract Type Tick();
}

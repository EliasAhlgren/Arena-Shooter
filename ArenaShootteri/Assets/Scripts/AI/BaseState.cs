using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Base state for all states to inherit
/// </summary>
public abstract class BaseState
{/// <summary>
/// Contructor for BaseState
/// </summary>
/// <param name="go">GameObject where <c>Grunt.cs</c> is attached </param>
    public BaseState(GameObject go)
    {
        this.gameObject = go;
        this.transform = go.transform;
    }
    /// <summary>
    /// GameObject where the AI is attached 
    /// </summary>
    protected GameObject gameObject;
    /// <summary>
    /// Transform where the AI is attached
    /// </summary>
    protected Transform transform;

    public abstract Type Tick();
    /// <summary>
    /// Called when AI enters specific state
    /// </summary>
    public abstract void OnStateEnter();
    /// <summary>
    /// Called when AI exits specific state
    /// </summary>
    public abstract void OnStateExit();

}

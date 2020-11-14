using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSetter : MonoBehaviour
{
    public int PoseNumber;

    public ikTargetScript IkTargetScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdatePose()
    {
        IkTargetScript.SetHandToPos(IkTargetScript.states[IkTargetScript.index]);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

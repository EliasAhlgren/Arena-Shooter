using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(HandPoser))]
public class customInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        HandPoser handPoser = (HandPoser) target;
        if (GUILayout.Button("Save Current Hand Pose") && handPoser.currentPose)
        {
            handPoser.SetCurrentPose();
        }
        
        if (GUILayout.Button("Get Current Hand Pose") && handPoser.currentPose)
        {
            handPoser.GetCurrentPose();
        }
        
    }

}


public class HandPoser : MonoBehaviour
{
    public Pose currentPose;
    
    public void SetCurrentPose()
    {
        currentPose.SetPose(gameObject.GetComponentsInChildren<Transform>());
        
    }

    public void GetCurrentPose()
    {
        currentPose.GetPose(gameObject.GetComponentsInChildren<Transform>());
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

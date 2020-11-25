using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "NewHandPose")]
public class Pose : ScriptableObject
{
    public Quaternion[] BoneRotations;

    // Asettaa assetin asetukset
    public void SetPose(Transform[] bones)
    {
        //EditorUtility.SetDirty(this);
        
        BoneRotations = new Quaternion[bones.Length];
        for (int i = 0; i < bones.Length; i++)
        {
            BoneRotations[i] = bones[i].localRotation;
        }
        
    }
    
    // Hakee assetin asetukset
    public void GetPose(Transform[] bones)
    {
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].localRotation = BoneRotations[i];
        }
    }
}

    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class IKsolver : MonoBehaviour
{
    private int _iterationsLeft;

    private bool _iterating;
    
    public bool solve;

    public float marginOfError = 0.1f;
    
    public Transform bone1, bone2, bone3;

    private Vector3 point1, point2, point3;
    
    private float length1, length2;

    //public Transform poleTarget;

    public bool flip;

    private int flipMultiplier;
    
    public Transform target;

    private Vector3 origin;
    
    [SerializeField]private bool isReachable;

    void Start()
    {
        origin = bone1.position;
        
        bone1.parent = transform.parent;
        bone2.parent = transform.parent;
        bone3.parent = transform.parent;
       
        point1 = bone1.position;
        point2 = bone2.position;

        length1 = Vector3.Distance(bone1.position, bone2.position);
        length2 = Vector3.Distance(bone2.position, bone3.position);
        
       // solve = true;
    }

    /// <summary>
    ///  Moves the bones so that endpoint reaches target using the FABRIK algorithm
    /// </summary>
    /// <remarks>Called every frame if endpoint is too far from target but can be called anytime as in most situations the endpoint reaches the target in a single pass</remarks>
    public void Solve(int Iterations)
    {
        if (!_iterating)
        {
            _iterationsLeft = Iterations;
        }
        
        _iterating = true;
        
        
        origin = bone1.position;

        
        //Vector3 poleDir = bone2.position - poleTarget.position;
        //bone1.forward = poleDir;
        //bone2.forward = poleDir;
        //bone3.forward = poleDir;

        
        
        //Forwards
        point3 = target.position;
        point2 = point3 + ((point2 - point3).normalized) * length2;
        point1 = point2 + ((point1 - point2).normalized) * length1;
        
        
        // Set bones
        bone3.position = point3;

        bone2.position = point2;
        
        bone1.position = point1;
        
        //Backwards
        point1 = origin;
        point2 = point1 + ((point2 - point1).normalized) * length1;
        point3 = point2 + ((point3 - point2).normalized) * length2;

        // Set bones
        bone3.position = point3;
        bone2.position = point2;
        bone1.position = point1;


        Vector3 dir = point2 - point3;
        
        bone2.right = dir;
        
        
        dir = point1 - point2;
        bone1.right = dir;

        if (flip)
        {
            Vector3 dir1 = point3 - point2;
        
            bone2.right = dir1;

            dir1 = point2 - point1;
            bone1.right = dir1;
        }
        
        //Draw lines
       
        // Debug.DrawLine(bone1.position,bone2.position,Color.red,Mathf.Infinity);
        // Debug.DrawLine(bone2.position,bone3.position,Color.magenta,Mathf.Infinity);

        if (_iterationsLeft > 0)
        {
            _iterationsLeft--;
            Solve(_iterationsLeft);
        }
        else
        {
            _iterating = false;
        }
        
    }
    
    void Update()
    {
        length1 = Vector3.Distance(bone1.position, bone2.position);
        length2 = Vector3.Distance(bone2.position, bone3.position);

        if (solve && Vector3.Distance(bone3.position, target.position) > marginOfError)
        {
            Solve(1);
        }

        //bone2.up = bone3.up;

    }
}

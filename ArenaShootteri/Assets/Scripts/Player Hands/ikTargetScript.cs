using System;
using UnityEngine;

public class ikTargetScript : MonoBehaviour
{
    public IKsolver ikSolver;
    
    public Transform gun;
    public bool setHand;

    public HandPoser handPoser;

    public Transform hand;
    private Transform[] _hand;
    
    private Transform _originalParent;

    public Vector3 posDiff;
    
    public int index;
    
    [Serializable]
    public struct HandState
    {
        public Pose handPose;
        public Transform targetObject;
        public Vector3 position;
        public string name;
    }

    public HandState[] states;
    
    // Start is called before the first frame update
    void Start()
    {
        _hand = hand.GetComponentsInChildren<Transform>();
        _originalParent = transform.parent;
    }

    public void SetHandToPos(HandState state)
    {
            handPoser.currentPose = state.handPose;
            handPoser.currentPose.GetPose(_hand);
            
            transform.position = state.targetObject.position + posDiff;
            ikSolver.Solve(3);
            transform.parent = state.targetObject;
            //hand.parent = state.targetObject;

    }
    
   
    
    // Update is called once per frame
    void Update()
    {

        //hand.position = States[0].targetObject.position;
        
        if (setHand)
        {
            SetHandToPos(states[index]);
            setHand = false;
        }
    }
}

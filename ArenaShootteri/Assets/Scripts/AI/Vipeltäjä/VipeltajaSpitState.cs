using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// Spit logic for Vipeltaja's AI
/// </summary>
public class VipeltajaSpitState : BaseState
{
    private Vipeltaja vipeltaja;

    /// <summary>
    /// Distance to travel between spits
    /// </summary>
    public float distanceBetweenSpits = 10;
    public float distanceCounter;
    /// <summary>
    /// How fast vipeltaja turns
    /// </summary>
    public float turnSpeed = 2;
    /// <summary>
    /// Track how many balls vipeltaja has spat
    /// </summary>
    private int ballsSpat;
    private float lastSpitCounter = 2;
    private float delay;

    public Vector3 startPosition;

    public VipeltajaSpitState(Vipeltaja _vipeltaja) : base(_vipeltaja.gameObject)
    {
        vipeltaja = _vipeltaja;
    }

    public override void OnStateEnter()
    {
        // Set start position to be vipeltaja's position
        startPosition = vipeltaja.transform.position;

        // reset distance and ball counter to zero
        distanceCounter = 0f;
        ballsSpat = 0;
        lastSpitCounter = 0;
        delay = UnityEngine.Random.Range(2, 4);
        // Play idle animation at start. 
        // Spit animation starts when vipeltaja faces the player
        vipeltaja.animator.Play("Idle");
        
    }

    public override void OnStateExit()
    {
        
    }

    public override Type Tick()
    {
        // Rotate vipeltaja all the time towards the player when spitting
        Vector3 targetDirection = vipeltaja.target.transform.position - vipeltaja.transform.position;
        float step = turnSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(vipeltaja.transform.forward, targetDirection, step, 0.0f);
        vipeltaja.transform.rotation = Quaternion.LookRotation(newDirection);

        // Dot product used to determine if enemy is facing the player. return 1 if facing player, return 0 at 90 degree angle.
        float dot = Vector3.Dot(vipeltaja.transform.forward, (vipeltaja.target.transform.position - vipeltaja.transform.position).normalized);
        
        // Launch spit when almost facing the player
        if (dot > 0.9f)
        {   
            // dont spit if spit animation is playing
            if (!vipeltaja.animator.GetCurrentAnimatorStateInfo(0).IsName("Spit"))
            {
                // First spit
                if (ballsSpat == 0)
                {
                    Debug.Log("Spit");
                    vipeltaja.SpawnSpit();
                    ballsSpat++;
                    vipeltaja.agent.ResetPath();
                }

                // Reset Navigation Path and Find new path to player.          
                vipeltaja.agent.SetDestination(vipeltaja.target.transform.position);

                // Track distance from start position. 
                distanceCounter = Vector3.Distance(startPosition, vipeltaja.transform.position);
                Debug.Log("Distance is " + distanceCounter + " Start position was: " + startPosition + " Current position is: " + vipeltaja.transform.position);
            }

            // Spit next ball when distance is bigger than distanceBetweenSpits.
            if (distanceCounter > distanceBetweenSpits)
            {
                // If 2nd spit. Just Spit another
                if (ballsSpat == 1)
                {
                    
                    vipeltaja.SpawnSpit();
                    ballsSpat++;

                }
            }
            
            // Wait for spit animation to end and proceed back to chase state

            if (ballsSpat == 2)
            {
                lastSpitCounter += Time.deltaTime;
                if (lastSpitCounter > delay)
                {
                    return typeof(VipeltajaChaseState);
                }
            }
            
        }
        return null;
    }


}

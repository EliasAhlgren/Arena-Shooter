using UnityEngine;
using System.Collections;
using System;
using UnityEngine.AI;
/// <summary>
/// Escape logic for Vipeltaja's AI
/// </summary>
public class VipeltajaEscapeState : BaseState
{
    
    private Vipeltaja vipeltaja;
    private float timeToFlee = 4f;
    /// <summary>
    /// how long enemy has fleed
    /// </summary>
    private float timer = 0f;
    /// <summary>
    /// how long each run cycle has lasted. One cycle is approx. 1 second
    /// </summary>
    private float dirTimer = 0f; 
    private float FleeOffset = 5;
    private Vector3 direction;
    /// <summary>
    /// Position where enemy will flee
    /// </summary>
    private Vector3 randomDestination;
    /// <summary>
    /// Where enemy should face
    /// </summary>
    private Vector3 targetDir;


    public VipeltajaEscapeState(Vipeltaja _vipeltaja) : base(_vipeltaja.gameObject)
    {
        vipeltaja = _vipeltaja;
    }

    public override void OnStateEnter()
    {
        vipeltaja.animator.SetBool("feared", true);
        vipeltaja.animator.Play("Walk");
        timer = 0f;
        dirTimer = 0f;
        vipeltaja.agent.isStopped = true;
        

        
        // Test 1
        //RandomPointOnNavMesh(vipeltaja.transform.position, FleeOffset, out randomDestination);
        //Debug.Log("Fleeing to " + randomDestination);

        // Test 2
        targetDir = Quaternion.AngleAxis(UnityEngine.Random.Range(-90, 90), vipeltaja.transform.up) * vipeltaja.transform.forward;
        targetDir = -1 * targetDir.normalized * UnityEngine.Random.Range(10, 20);

        // vipeltaja.transform.rotation = Quaternion.LookRotation(vipeltaja.transform.position - vipeltaja.target.transform.position);
        //  direction = vipeltaja.target.transform.position - vipeltaja.transform.position;

        // Run animations
    }

    public override void OnStateExit()
    {
        vipeltaja.agent.isStopped = false;
        vipeltaja.animator.SetBool("feared", false);

        // Something Something
    }

    public override Type Tick()
    {
        vipeltaja.transform.rotation = Quaternion.LookRotation(targetDir);
        vipeltaja.transform.Translate(Vector3.forward * vipeltaja.speed / 2 * Time.deltaTime);


        if(randomDestination == Vector3.zero)
        {
            // RandomPointOnNavMesh(vipeltaja.transform.position, FleeOffset, out randomDestination);
        }
        // vipeltaja.agent.ResetPath();
        dirTimer += Time.deltaTime;
        if (dirTimer > 1)
        {
            // Test 1
            // RandomPointOnNavMesh(vipeltaja.transform.position, FleeOffset, out randomDestination);
            // vipeltaja.agent.SetDestination(randomDestination);
            
            // Test 2 Finds position behind the enemy and runs there.
            targetDir = Quaternion.AngleAxis(UnityEngine.Random.Range(-90, 90), vipeltaja.transform.up) * -vipeltaja.transform.forward;
            targetDir = targetDir.normalized * UnityEngine.Random.Range(10, 20);           

            dirTimer -= 1;
        }

        
        timer += Time.deltaTime;

        // vipeltaja.transform.Translate(Vector3.forward.normalized * vipeltaja.speed * Time.deltaTime);

        // Go to spit state when Escape time is above maximum flee time
        if (timer >= timeToFlee)
        {
            return typeof(VipeltajaSpitState);
        }
        return null;        
    }
    /// <summary>
    /// Finds random point on NavMesh. Returns Vector3.zero if false
    /// </summary>
    /// <param name="center">Center of search</param>
    /// <param name="range">Range of search</param>
    /// <param name="result">Random point on Nav Mesh</param>
    /// <returns></returns>
    public bool RandomPointOnNavMesh(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }
}

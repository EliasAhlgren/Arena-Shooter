using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ChaseConcept : BaseState
{
    
    private EnemyConcept _enemyConcept;
    private float _chaseSpeed = 1;

    public ChaseConcept(EnemyConcept enemyConcept) : base(enemyConcept.gameObject)
    {
        _enemyConcept = enemyConcept;
    }

    public override Type Tick()
    {
        Debug.Log("Chase state. Target is " + _enemyConcept._target.name);
      
        // Implement what to do when enemy is chasing 
        if(_enemyConcept._target != null)
        {

            Seeker seeker = _enemyConcept.seeker;
            //   seeker.pathCallback += OnPathComplete;
            seeker.StartPath(_enemyConcept.transform.position, _enemyConcept._target.transform.position, OnPathComplete);
            
            if (_enemyConcept.path == null)
            {
                // No path to move on. Return to idle state or something
            }

            _enemyConcept.reachedEndOfPath = false;

            float distanceToWaypoint;
            while(true)
            {
                distanceToWaypoint = Vector3.Distance(_enemyConcept.transform.position, _enemyConcept.path.vectorPath[_enemyConcept.currentWaypoint]);
                if (distanceToWaypoint < _enemyConcept.nextWaypointDistance)
                {
                    if(_enemyConcept.currentWaypoint + 1 < _enemyConcept.path.vectorPath.Count)
                    {
                        _enemyConcept.currentWaypoint++;
                    } 
                    else
                    {
                        _enemyConcept.reachedEndOfPath = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            // Slow down smoothly when approaching end of path
            // Value will go from 1 to 0 when approaching the last waypoint of the path
            var speedFactor = _enemyConcept.reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / _enemyConcept.nextWaypointDistance) : 1f;

            // Direction to next waypoint
            Vector3 dir = (_enemyConcept.path.vectorPath[_enemyConcept.currentWaypoint] - _enemyConcept.transform.position).normalized;
            // Multiply direction with our speed to get a velocity
            Vector3 velocity = dir * _chaseSpeed * speedFactor;

            // Use CharacterController component to move the agent
            // SimpleMove takes velocity in meters/second, so no need to multiply by Time.deltaTime
            _enemyConcept.controller.SimpleMove(velocity);
        }
        return null;
        
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("We have path. Errors? " + p.error);
        if(!p.error)
        {
            _enemyConcept.path = p;
            _enemyConcept.currentWaypoint = 0;
        }
    }
}
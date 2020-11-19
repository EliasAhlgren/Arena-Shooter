using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNodes : MonoBehaviour
{
    public Vector3[] positions;
    private Collider[] _mapColliders;

    private GameObject[] _map;

    private int _nodeCount;
    
    int x, z, indexer;

    public int xCount, zCount;
    
    private bool hasCreated;

    public float scale;
    // Start is called before the first frame update
    void Start()
    {
        _nodeCount = xCount * zCount;
        
        positions = new Vector3[_nodeCount];
        
        _map = GameObject.FindGameObjectsWithTag("Level");
        _mapColliders = new Collider[_map.Length];
        for (int i = 0; i < _mapColliders.Length; i++)
        {
            _mapColliders[i] = _map[i].GetComponent<Collider>();
        }

        CreatePositions();
        hasCreated = true;
    }

    private void CreatePositions()
    {
        if (indexer < _nodeCount)
        {
            positions[indexer] = transform.position + new Vector3(x * scale, 0, z * scale);
            x++;
            if (x == xCount)
            {
                x = 0;
                z++;
            }
            indexer++;
            CreatePositions();
        }
    }

    private void OnDrawGizmos()
    {
        if (hasCreated)
        {
            foreach (var VARIABLE in positions)
                    {
                        Gizmos.DrawSphere(VARIABLE, 1);
                    }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

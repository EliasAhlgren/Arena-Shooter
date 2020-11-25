using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shaderPulsating : MonoBehaviour
{
    private Material _material;
    public float intensity = 1f;
    public float speed = 1f;
    private float _time;

    public bool shakeyShakey;

    private Vector3 transformLocalRotation;
    // Start is called before the first frame update
    void Start()
    {
        if (!shakeyShakey)
        {
            _material = gameObject.GetComponent<MeshRenderer>().material;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!shakeyShakey)
        {
            _time += Time.deltaTime * speed;
                    _material.SetFloat("_NormalScale",1.5f + Mathf.Sin(_time) * intensity);
        }
        

        if (shakeyShakey)
        {
            transformLocalRotation = transform.localEulerAngles;
            
            transformLocalRotation += new Vector3(Random.Range(-0.1f * intensity,0.1f * intensity),Random.Range(-0.1f * intensity,0.1f * intensity),Random.Range(-0.1f * intensity,0.1f * intensity));
            transform.localEulerAngles = transformLocalRotation;
        }
    }
}

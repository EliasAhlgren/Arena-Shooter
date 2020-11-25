using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float roughMod = 1, magnMod = 1;
    float currentFadeTime;
    float tickPos = 0;
    float tickRot = 0;
    bool fadingIn;
    Vector3 pos;
    Vector3 rot;

    public Vector3 positionInfluence = new Vector3(1, 1, 1);
    public Vector3 rotationInfluence = new Vector3(1, 1, 1);

    Vector3 posAddShake, rotAddShake;

    public IEnumerator Shake (float magnitude, float roughness, float fadeInTime, float fadeOutTime, bool useRotation)
    {
        bool isShaking = true;

        if (fadeInTime > 0)
        {
            currentFadeTime = 0;
            fadingIn = true;
        }
        else if (fadeOutTime > 0)
        {
            currentFadeTime = 1;
            fadingIn = false;
        }
        else
        {
            isShaking = false;
        }

        tickPos = Random.Range(-100, 100);
        tickRot = Random.Range(-100, 100);

        while (isShaking)
        {

            if (currentFadeTime < 1 && fadingIn)
            {
                currentFadeTime += Time.deltaTime / fadeInTime;
                if (currentFadeTime >= 1)
                {
                    fadingIn = false;
                }
            }
            else
            {
                currentFadeTime -= Time.deltaTime / fadeOutTime;
                if (currentFadeTime > 0)
                {
                    isShaking = false;
                }
            }

            

            pos.x = Mathf.PerlinNoise(tickPos, 0) - 0.5f;
            pos.y = Mathf.PerlinNoise(0, tickPos) - 0.5f;
            pos.z = Mathf.PerlinNoise(tickPos, tickPos) - 0.5f;

            rot.x = Mathf.PerlinNoise(tickRot, 0) - 0.5f;
            rot.y = Mathf.PerlinNoise(0, tickRot) - 0.5f;
            rot.z = Mathf.PerlinNoise(tickRot, tickRot) - 0.5f;

            if (fadingIn)
            {
                tickPos += Time.deltaTime * roughness * roughMod;
                tickRot += Time.deltaTime * roughness * roughMod;
            }
            else
            {
                tickPos = Time.deltaTime * roughness * roughMod * currentFadeTime;
                tickRot = Time.deltaTime * roughness * roughMod * currentFadeTime;
            }

            
            pos *= magnitude * magnMod * currentFadeTime;
            rot *= magnitude * magnMod * currentFadeTime;


            posAddShake = MultiplyVectors(pos, positionInfluence);
            rotAddShake = MultiplyVectors(rot, rotationInfluence);


            transform.localPosition += posAddShake;
            if (useRotation) transform.localEulerAngles += rotAddShake;


            yield return null;
        }

        transform.localPosition = Vector3.zero;
        if (useRotation) transform.localEulerAngles = Vector3.zero;
    }

    public static Vector3 MultiplyVectors(Vector3 a, Vector3 b)
    {
        a.x *= b.x;
        a.y *= b.y;
        a.z *= b.z;

        return a;
    }

}

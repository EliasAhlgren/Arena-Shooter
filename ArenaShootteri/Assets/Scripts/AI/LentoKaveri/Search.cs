using System.Collections;
using UnityEngine;

namespace AI.LentoKaveri
{
    public class Search : EnemyState
    {
        public override IEnumerator BeginState(LentoSaatana obj)
        {
            obj.gameObject.transform.Translate(obj.gameObject.transform.forward);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(BeginState(obj));
        }
    }
}
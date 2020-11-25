using System.Collections;
using System.Collections.Generic;
using AI.LentoKaveri;
using UnityEngine;

public abstract class EnemyState : LentoSaatana
{
  public virtual IEnumerator BeginState(LentoSaatana obj)
  {
    yield break;
  }
}

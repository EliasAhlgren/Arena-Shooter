using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.LentoKaveri
{
    
        
    public class LentoSaatana : MonoBehaviour
    {
        private EnemyState _enemyState;

        public CreateNodes positionNodes;
        
        public void SetState(EnemyState state)
        {
            _enemyState = state;
            StartCoroutine(_enemyState.BeginState(this));
        }
       
        // Start is called before the first frame update
        void Start()
        {
            SetState(gameObject.AddComponent<Search>());
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
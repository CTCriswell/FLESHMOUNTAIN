using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxProjectile : HitBoxEnemy
{
    private EnemyProjectile ep;
    
    void Start()
    {
        ep = GetComponentInParent<EnemyProjectile>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other) {
        if(DamagePlayerCheck(other)){
            ep.selfDestruct();
        }
    }
}

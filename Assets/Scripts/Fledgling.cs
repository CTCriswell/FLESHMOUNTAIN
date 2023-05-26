using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fledgling : Enemy
{
    protected override void Start() {
        maxHealth = 99;
        base.Start();
        runAccel = 0;
        topSpeed = 2;
        meleeDamage = 3;
        StartCoroutine(Jumping_CR());
        play = player.GetComponent<Player>();
    }

    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(collisionBottom,collisionRadius);
    // }

    private void hop(bool right, float power){
        Velocity.x += power;
        if(!right){Velocity.x *= -1;}
        Velocity.y += power;
    }

    private IEnumerator Jumping_CR(){
        byte t;
        while(true){
            if(isDead){break;}
            t = 80;
            if(System.Math.Abs(playerxDis) < 2){
                hop(playerIsRight,1.5f);
            } else {
                hop(playerIsRight,.75f);
            }
            while(t>0){
                t--;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}

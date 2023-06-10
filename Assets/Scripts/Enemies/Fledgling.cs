using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fledgling : Enemy
{
    protected override void Start() {
        maxHealth = 5;
        base.Start();
        runAccel = 0;
        topSpeed = 12;
        //meleeDamage = 3;
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
        Velocity.y += 2*power;
    }

    private IEnumerator Jumping_CR(){
        while(true){
            if(isDead){break;}
            if(System.Math.Abs(playerxDis) < 2){
                hop(playerIsRight,8);
            } else {
                hop(playerIsRight,7);
            }
            yield return new WaitForSeconds(2);
        }
    }
}

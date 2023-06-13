using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fledgling : Enemy
{
    protected override void Start() {
        maxHealth = 5;
        base.Start();
        runAccel = 1;
        //meleeDamage = 3;
        StartCoroutine(Jumping_CR());
        play = player.GetComponent<Player>();
    }

    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(collisionBottom,collisionRadius);
    // }

    private IEnumerator hop_CR(bool right, float power){
        Velocity.x += power;
        topSpeed = power;
        Move = new Vector2(1,0);
        if(!right){
            Velocity.x *= -1;
            Move *= -1;
        }
        Velocity.y += 2*power;
        yield return new WaitForSeconds(0.1f);
        while(InAir){
            yield return new WaitForFixedUpdate();
        }
        Move = new Vector2(0,0);
    }

    private IEnumerator Jumping_CR(){
        while(!isDead){
            if(System.Math.Abs(playerxDis) < 2){
                StartCoroutine(hop_CR(playerIsRight,8+Random.Range(-1f,1f)));
            } else {
                StartCoroutine(hop_CR(playerIsRight,7+Random.Range(-1f,1f)));
            }
            yield return new WaitForSeconds(2+Random.Range(-0.2f,0.2f));
        }
    }
}

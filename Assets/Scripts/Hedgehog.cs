using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hedgehog : Enemy
{
    public GameObject projectileHedgehog;
    protected override void Start()
    {
        maxHealth = 10;
        base.Start();
        runAccel = 0.2f;
        topSpeed = 0.5f;
        //meleeDamage = 3;
        StartCoroutine(Idle_CR());
        play = player.GetComponent<Player>();
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(collisionBottom,collisionRadius);
    }
    private IEnumerator Idle_CR(){
        while(!isDead){

            Move = (sbyte) Random.Range(-2,2);

            for(int i = 0; i<50; i++){
                yield return new WaitForFixedUpdate();
                if(System.Math.Abs(playerxDis) < 2){
                    yield return StartCoroutine(AttackPlayer_CR());
                }
            }

            Move = (sbyte) 0;

            for(int i = 0; i<100; i++){
                yield return new WaitForFixedUpdate();
                if(System.Math.Abs(playerxDis) < 2){
                    yield return StartCoroutine(AttackPlayer_CR());
                }
            }
        }
    }

    private void Shoot(){
        GameObject Proj = Instantiate(projectileHedgehog,transform.position,Quaternion.identity);
        EnemyProjectile ep = Proj.GetComponent<EnemyProjectile>();
        ep.velocity = new Vector2(3.5f*playerxDis/System.Math.Abs(playerxDis),3f);
        Destroy(Proj,15);
    }

    private IEnumerator AttackPlayer_CR(){
        topSpeed = 2.2f;
        while(!isDead){
            Move = (sbyte)(-(playerxDis/System.Math.Abs(playerxDis)));
            if(System.Math.Abs(playerxDis)>2){
                Move = 0;
                yield return new WaitForSeconds(0.15f);
                if(playerxDis/System.Math.Abs(playerxDis) == 1){
                    isRight = true;
                } else {
                    isRight = false;
                }
                while(System.Math.Abs(playerxDis)>2 && System.Math.Abs(playerxDis)<3.5f){
                    Shoot();
                    yield return new WaitForSeconds(0.75f);
                }
            }
            yield return new WaitForFixedUpdate();
            if(System.Math.Abs(playerxDis)>=3.5f){
                break;
            }
            // Move = 0;
            // yield return new WaitForSeconds(0.15f);
            // if(playerxDis/System.Math.Abs(playerxDis) == 1){
            //     isRight = true;
            // } else {
            //     isRight = false;
            // }
            // yield return new WaitForSeconds(0.05f);
            // Shoot();
            // yield return new WaitForSeconds(0.3f);
            // Move = (sbyte)(-(playerxDis/System.Math.Abs(playerxDis)));
            // yield return new WaitForSeconds(1);
        }
    }
}

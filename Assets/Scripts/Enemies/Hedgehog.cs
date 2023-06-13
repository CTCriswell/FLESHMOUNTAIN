using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Hedgehog : Enemy
{
    public GameObject projectileHedgehog;
    protected override void Start()
    {
        maxHealth = 10;
        base.Start();
        runAccel = 1.2f;
        //meleeDamage = 3;
        StartCoroutine(Idle_CR());
        play = player.GetComponent<Player>();
    }
    private IEnumerator Idle_CR(){
        topSpeed = 3;
        while(!isDead){
            Move.x = (sbyte) Random.Range(-1,2);

            for(int i = 0; i<50; i++){
                yield return new WaitForFixedUpdate();
                if(System.Math.Abs(playerxDis) < 12){
                    yield return StartCoroutine(AttackPlayer_CR());
                }
            }

            Move.x = (sbyte) 0;

            for(int i = 0; i<100; i++){
                yield return new WaitForFixedUpdate();
                if(System.Math.Abs(playerxDis) < 12){
                    yield return StartCoroutine(AttackPlayer_CR());
                }
            }
        }
    }

    private void Shoot(){
        GameObject Proj = Instantiate(projectileHedgehog,transform.position,Quaternion.identity);
        EnemyProjectile ep = Proj.GetComponent<EnemyProjectile>();
        ep.velocity = new Vector2((22f+Random.Range(-1f,1f))*playerxDis/System.Math.Abs(playerxDis),20);
        Destroy(Proj,15);
    }

    private IEnumerator AttackPlayer_CR(){
        topSpeed = 14f;
        while(!isDead){
            Move.x = (sbyte)(-(playerxDis/System.Math.Abs(playerxDis)));
            if(System.Math.Abs(playerxDis)>12){
                Move.x = 0;
                yield return new WaitForSeconds(0.15f);
                if(playerxDis/System.Math.Abs(playerxDis) == 1){
                    isRight = false;
                } else {
                    isRight = true;
                }
                while(System.Math.Abs(playerxDis)>12 && System.Math.Abs(playerxDis)<22){
                    Shoot();
                    yield return new WaitForSeconds(0.75f);
                }
            }
            yield return new WaitForFixedUpdate();
            if(System.Math.Abs(playerxDis)>=22){
                topSpeed = 3;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hedgehog : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        maxHealth = 10;
        base.Start();
        runAccel = 0.2f;
        topSpeed = 1;
        meleeDamage = 3;
        StartCoroutine(Idle_CR());
        play = player.GetComponent<Player>();
    }

    private IEnumerator Idle_CR(){
        float t;
        while(true){
            t = 100;
            while(t != 0){
                //if(System.Math.Abs(playerxDis) < 1.5f){break;}
                if(t == 95){
                    Move = (sbyte) Random.Range(-2,2);
                }
                if(t == 65){
                    Move = 0;
                }
                t--;
                yield return new WaitForFixedUpdate();
            }
            //yield return StartCoroutine(AttackPlayer_CR());
        }
    }

    private IEnumerator AttackPlayer_CR(){
        yield return null;
    }
}

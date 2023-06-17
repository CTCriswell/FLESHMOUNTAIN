using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolyPoly : Enemy
{
    private bool bonked;
    private bool attacking;
    private bool isIdle;
    public Sprite sprite_rolling;
    protected override void Start()
    {
        maxHealth = 10;
        base.Start();
        runAccel = 0.5f;
        StartCoroutine(Idle_CR());
        play = player.GetComponent<Player>();
        bonked = false;
        attacking = false;
        isIdle = true;
    }
    private IEnumerator Idle_CR(){
        topSpeed = 2;
        while(!isDead && !attacking){
            Move.x = (sbyte) Random.Range(-1,2);

            for(int i = 0; i<50; i++){
                yield return new WaitForFixedUpdate();
                if(!attacking && System.Math.Abs(playerxDis) < 8){
                    StartCoroutine(AttackPlayer_CR());
                    isIdle = false;
                    break;
                }
            }

            if(isIdle){Move.x = (sbyte) 0;}

            for(int i = 0; i<100; i++){
                yield return new WaitForFixedUpdate();
                if(!attacking && System.Math.Abs(playerxDis) < 8){
                    StartCoroutine(AttackPlayer_CR());
                    isIdle = false;
                    break;
                }
            }
        }
    }

    private IEnumerator AttackPlayer_CR(){
        attacking = true;
        sr.sprite = sprite_rolling;
        Move.x = (sbyte) (-(playerxDis/System.Math.Abs(playerxDis)));
        if(Move.x == 0){ Move.x = 1;}
        topSpeed = 3;
        yield return new WaitForSeconds(0.5f);
        Move.x *= -1;
        topSpeed = 15;
        while(!isDead){
            yield return new WaitForFixedUpdate();
            if(playerxDis*Move.x < 0){
                // yield return StartCoroutine(turn_CR());
                Move.x*=-1;
            }
            if(!bonked && ((collidersLeft.Length > 0 && Velocity.x < 0) || (collidersRight.Length > 0 && Velocity.x > 0))){
                StartCoroutine(bonk_CR());
                break;
            }
            
        }
    }

    private IEnumerator bonk_CR()
    {
        bonked = true;
        // float tempVel;
        // tempVel = Velocity.x;
        // Velocity.x = 0;
        Velocity = new Vector2(-Velocity.x/System.Math.Abs(Velocity.x)*12,15);
        Move.x = 0;
        sr.sprite = sprite_main;
        sr.color = new Color(1,1,1,0.66f);
        yield return new WaitForSeconds(2);
        sr.color = new Color(1,1,1,1);
        bonked = false;
        if(!isDead){
        StartCoroutine(AttackPlayer_CR());
        }
    }

    // private IEnumerator turn_CR()
    // {
    //     Move.x*= -1;
    //     while(Velocity.x*Move.x < 0 && playerxDis*Move.x < 0){
    //         yield return new WaitForFixedUpdate();
    //     }

    //     Velocity.x = 0;
    // }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : Enemy
{
    private float yPosInit;

    // Start is called before the first frame update
    protected override void Start()
    {
        maxHealth = 200;
        base.Start();
        runAccel = 1;
        topSpeed = 15;
        StartCoroutine(Idle_CR());
        rb.gravityScale = 0;
        yPosInit = rb.position.y;
    }
    
    private IEnumerator Idle_CR(){
        while(Vector3.Distance(transform.position,player.transform.position) <= 1){
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(Fly_CR());
    }

    private IEnumerator Fly_CR(){
        Move.x = -1;
        yield return new WaitForSeconds(0.25f);
        int t = 0;
        while(!isDead){
            yield return new WaitForFixedUpdate();
            t++;
            rb.velocity = new Vector2(rb.velocity.x,0.5f*Mathf.Sin(2*t*2*Mathf.PI/256));
            if(System.Math.Abs(rb.velocity.x) < 0.05 && !turning){
                Flip_CR();
            }
        }
        Velocity = new Vector2 (0,0);
        rb.gravityScale = 20;
        Move.x = 0;
    }

    private IEnumerator Flip_CR(){
        Debug.Log("flip called");
        turning = true;
        Move.x*= -1;
        yield return new WaitForSeconds(0.3f);
        turning = false;
    }

}

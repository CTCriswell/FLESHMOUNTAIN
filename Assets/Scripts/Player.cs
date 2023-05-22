using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public int Move;
    private int Vert;
    public Vector2 shotDir = new Vector2(1,1);
    public float shotAirBoost = 0;
    private float shotVelx, shotVely;
    public sbyte reloadDelay;
    public sbyte reloading;
    private Vector2 Recoil = new Vector2(10,10);
    private BoxCollider2D hurtbox;
    private Vector2 collisionBottom, collisionLeft, collisionRight;
    private Collider2D[] collidersBottom, collidersLeft, collidersRight;
    private float collisionRadius = 0.15f;
    private int jumpVel;
    private byte jumpBuffer;
    private Vector2 shootAccel = new Vector2 (0,0);
    private IEnumerator JumpFunc, ShootFunc;
    private int jumpAccel = 25;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hurtbox = transform.Find("PlayerHurtBox").GetComponent<BoxCollider2D>();

        JumpFunc = JumpFunc_CR();
        ShootFunc = ShootFunc_CR();

        reloading = 0;
        reloadDelay = 16;
    }

    protected override void Update()
    {
        if(Move > 0) isRight = true;
        if(Move < 0) isRight = false;

        if(isRight){sr.flipX = false;}
        else sr.flipX = true;

        if(!isDead){
            Move = (int) Input.GetAxisRaw("Horizontal"); // Inputs
            Vert = (int) Input.GetAxisRaw("Vertical");

            if(Input.GetButtonDown("Jump")){
                //JumpFunc.Reset();
                StartCoroutine(JumpFunc_CR());
            }
            if(Input.GetButtonDown("Fire1") && reloading == 0){
                shotDir = new Vector2(Move,Vert);
                StartCoroutine(ShootFunc_CR());
            }
        } else {
            Velocity = new Vector2 (0,0);
            if(Input.GetButtonDown("Fire1")){
                Respawn();
            }
        }
        
    }
    protected override void FixedUpdate()
    {
        if(!isDead){
            MoveFunc();
            if(System.Math.Abs(Velocity.x) < 0.5 && Move == 0){ // stops you if slow enough
                Velocity.x = 0;
            }
        }
        CollisionUpdate();

        if(!InAir && System.Math.Abs(Velocity.x) > topSpeed){// general friction

        //    vvv           how much faster than topSpeed/2         Reduce      Extract velocity.x sign
            Velocity.x -= (System.Math.Abs(Velocity.x)-topSpeed/2) *0.25f* (Velocity.x/System.Math.Abs(Velocity.x));
        }

        rb.velocity = new Vector2(Velocity.x,rb.velocity.y + Velocity.y);
    }
    protected override void MoveFunc(){
        if(InAir && Move == 0){runAccel = 0;}
        else {runAccel = Move*0.75f;}

        if(System.Math.Abs(rb.velocity.x) >= topSpeed && Velocity.x*runAccel > 0){ // cannot add speed past top speed, but can slow down
            runAccel = 0;
        }

        if(Move == 0 && !InAir && System.Math.Abs(Velocity.x)!=0){ // running friction
            Velocity.x -= Velocity.x/System.Math.Abs(Velocity.x);
        }

        Velocity.x += runAccel;
    }

    private IEnumerator JumpFunc_CR(){
        if(InAir) {
            jumpBuffer = 5;
            while(jumpBuffer > 0){
                yield return new WaitForFixedUpdate();
                if(!InAir){break;}
                jumpBuffer--;
            }
        }
        jumpBuffer = 0;
        if(!InAir){
            Velocity.y += jumpAccel;
            InAir = true;
            yield return new WaitForFixedUpdate();
            Velocity.y -= jumpAccel;
        }

        while(Input.GetButton("Jump") && rb.velocity.y > 10){// hold space bar to increase jump height
            rb.gravityScale=6;
            yield return new WaitForFixedUpdate();
        }
        rb.gravityScale = 12;
    }

    private IEnumerator ShootFunc_CR(){
        reloading = reloadDelay;
        if(shotDir.x == 0 && shotDir.y == 0){// shoot where character is facing if no input
            if(isRight){shotDir.x = 1;}
            else{shotDir.x = -1;}
        }
        // one frame y shot boost
        shootAccel.y = -shotDir.y*Recoil.y*2.5f;

        if(rb.velocity.y < 0){
            shootAccel.y += -rb.velocity.y;// shooting down is like a double jump instead of a slow down
        }
        
        Velocity.y += shootAccel.y; 
        yield return new WaitForFixedUpdate();
        Velocity.y -= shootAccel.y;

        if(InAir){ // allows to quickly change horizontal direction unless going too fast
            if(System.Math.Abs(shotAirBoost) < 32 && (-shotDir.x + shotAirBoost/System.Math.Abs(shotAirBoost)) == 0) {shotAirBoost = -shotDir.x*Recoil.x/10f;}
            else {shotAirBoost += -shotDir.x*Recoil.x/10f;}
        }

        Velocity.x += -Recoil.x*shotDir.x*2;

        // while(reloading >= reloadDelay - 15){
        //     shootAccel.x = Recoil.x*shotDir.x*(1/343)*(-675+70*(reloading-(reloadDelay - 15))-3*(reloading-(reloadDelay - 15))^2);
        //     if(!InAir){
        //         shootAccel.x *= 1f; // reduced recoil on ground
        //     }
        //     Velocity.x += shootAccel.x;
        //     reloading --;
        //     yield return new WaitForFixedUpdate();
        // }
        while(reloading > 0){
            reloading --;
            yield return new WaitForFixedUpdate();
        }

    }

    private void CollisionUpdate(){
        collisionBottom = new Vector2(cc.transform.position.x,cc.transform.position.y-0.4f);
        collisionRight = new Vector2(cc.transform.position.x+0.2f,cc.transform.position.y);
        collisionLeft = new Vector2(cc.transform.position.x-0.2f,cc.transform.position.y);

        collidersBottom = Physics2D.OverlapCircleAll(collisionBottom,collisionRadius,groundLayer);
        collidersLeft = Physics2D.OverlapCircleAll(collisionLeft,collisionRadius,groundLayer);
        collidersRight = Physics2D.OverlapCircleAll(collisionRight,collisionRadius,groundLayer);

        if(collidersBottom.Length > 0){InAir = false;}
        else {InAir = true;}
        
        if(collidersLeft.Length > 0 || collidersRight.Length > 0){
            shotVelx = 0;
            shotAirBoost = 0;
            if(collidersLeft.Length > 0 && Velocity.x < 0){
                Velocity.x = 0;
                runAccel = 0;
                Debug.Log(collidersLeft[0].gameObject.name);
            }
            if(collidersRight.Length > 0 && Velocity.x > 0){
                Velocity.x = 0;
                runAccel = 0;
                Debug.Log(collidersRight[0].gameObject.name);
            }
        }
    }
}

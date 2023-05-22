//using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Most veriables with T at the end are used to time between Update() and FixedUpdate(), which are used for rendering and physics respectively
    public float topSpeed;
    public int Move;
    private int MoveTemp;
    private int MoveT;
    private int Vert;
    public int jumpT;
    private int fallJumpT;
    //private float JumpForce = 1000;
    private Vector2 Recoil = new Vector2(7.5f,10);
    private bool isRight;
    public bool InAir;
    private float jumpVel;
    private float runAccel;
    public float friction = 1;
    public float runVel; //x velocity from running
    private float shotVelx, shotVely; // x and y velocity from gun boost
    //private float shotAccelx;
    public Vector2 shotDir = new Vector2(1,1);
    public Vector2 collisionBottom, collisionRight, collisionLeft;
    private int shotT=0;
    public float shotVelT =0;
    public float shotAirBoost = 0;
    private int runT=0;
    public int reload=0;
    private bool sprint;
    public Sprite character;
    private CapsuleCollider2D cc;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private ParticleSystem ps;
    private Damage dmg;
    public LayerMask groundLayer;
    private float collisionRadius = 0.15f;
    private Collider2D[] collidersBottom;
    private Collider2D[] collidersLeft;
    private Collider2D[] collidersRight;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ps = GetComponent<ParticleSystem>();
        dmg = GetComponent<Damage>();
        var emission = ps.emission;
        emission.rateOverDistance = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if(!dmg.isDead){
            Move = (int) Input.GetAxisRaw("Horizontal"); // Inputs
            //Jump = Input.GetButtonDown("Jump");
            Vert = (int) Input.GetAxisRaw("Vertical");
            
            if(Move > 0){isRight = true;}// used for sprite rendering left and right
            if(Move < 0){isRight = false;}
            sr.flipX = !isRight; 

            if(Move+MoveTemp==0){ //is true when frame perfect move change. puts one frame of zero speed, doesnt allow player to maintain boost after changing directions
                Move = 0;
                MoveT = 15;
                }

            MoveTemp = Move;

            if(Input.GetButtonDown("Jump")){
                jumpT = 5;
            }
            if(Input.GetButtonDown("Fire1") && reload == 0){
                shotT = 15;
                reload = 5;
                shotDir = new Vector2 (Move,Vert);
                //if(InAir){shotAirCount ++;}
            }
            if(Input.GetButton("Fire3")){
                sprint = true;
            } else {sprint = false;}

            //if(jumpT>0){jumpT-=1;}// counting down timers
            if(fallJumpT>0){fallJumpT--;}
            if(shotT>0){jumpT--;}
            if(MoveT>0){MoveT--;}
        } else {
            if(jumpT>0){jumpT-=1;}
            if(fallJumpT>0){fallJumpT--;}
            if(shotT>0){jumpT--;}
            if(MoveT>0){MoveT--;}
            if(Input.GetButtonDown("Fire1")){
                Respawn();
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawCube(new Vector3(collisionBottom.x,collisionBottom.y,0),new Vector3(0.4f,0.1f,1));
        Gizmos.DrawWireSphere(new Vector3(collisionBottom.x,collisionBottom.y,0),collisionRadius);
        Gizmos.DrawWireSphere(new Vector3(collisionLeft.x,collisionLeft.y,0),collisionRadius);
        Gizmos.DrawWireSphere(new Vector3(collisionRight.x,collisionRight.y,0),collisionRadius);

    }

    void FixedUpdate()
    {
        
        if(!dmg.isDead){
            MoveFunc();

            CollisionUpdate();

            JumpFunc();

            if(reload>0){reload-=1;}// is here so shoot speed isnt affected by system hardware (rendering times)

            ShootFunc();

            if(shotAirBoost==0||(shotAirBoost <= 5 && shotAirBoost >= -5) && !InAir){shotAirBoost = 0;}
            else if(shotAirBoost > 5 && !InAir){shotAirBoost -= friction;}
            else if(shotAirBoost < -5 && !InAir){shotAirBoost += friction;}

            if(rb.velocity.y+shotVely+jumpVel >=30){// sets max vertical speed
                rb.velocity = new Vector2(shotVelx + runVel + shotAirBoost, 30f);
            } else {
                rb.velocity = new Vector2(shotVelx + runVel + shotAirBoost, rb.velocity.y + shotVely + jumpVel);// CORE CHAR VELOCITY FUNCTION, adds everything from before. rb.vel.y is there to allow gravity
            }
        }
    }

    public void Respawn() {
        shotVelx = 0;
        shotVely = 0;
        runAccel = 0;
        runVel = 0;
        shotAirBoost = 0;
        jumpVel = 0;
        transform.position = new Vector3(0,0,0);
        dmg.isDead = false;
        dmg.restoreHealth(dmg.maxHealth);
        sr.sprite = character;
    }

    private void MoveFunc(){
        if(MoveT > 0){//reciver for timer variable
                MoveT = 0;
                Move=0;
            }

        if(InAir && Move == 0){runAccel = 0;}
        else {runAccel = Move*0.5f;}
        runVel += runAccel;
        if(System.Math.Abs(runVel) >= topSpeed){
            runVel = topSpeed * runVel/System.Math.Abs(runVel);
        }
        if(Move == 0 && !InAir && System.Math.Abs(runVel)!=0){
            runVel -= friction*runVel/System.Math.Abs(runVel);
            //if(System.Math.Abs(runVel)<0.2){runVel = 0;}
        }
    }    

    private void JumpFunc(){
        if((jumpT > 0)){// jumpman wahoo
            if(!InAir){
                jumpVel = 25;
                jumpT = 0;
            } else {jumpT --;}
        } else {jumpVel = 0;}

        if(Input.GetButton("Jump") && rb.velocity.y > 10){// hold space bar to increase jump height
                rb.gravityScale=6;
        } else {rb.gravityScale = 12;}
    }

    private void ShootFunc(){
        if(shotT > 0 || shotVelT > 0){// shoot to boost character speed
            rb.gravityScale = 12;// used to reset gravity from jump, without this player can mega jump
            if(shotT > 0){shotVelT = 15;}// timer for how much boost
            else if(shotVelT < 0){shotVelT = 0;}
            else{shotVelT --;}

            shotT = 0;

            if(shotDir.x == 0 && shotDir.y == 0){// shoot where character is facing is no input
                if(isRight){shotDir.x = 1;}
                else{shotDir.x = -1;}
            }

            if(shotDir.x != Move && shotDir.y == 0){runT +=5;}// horizontal blast to jumpstart running boost
            else {// reset boost and particles
                runT = 0;
                var emission = ps.emission;
                emission.rateOverDistance = 0;
            }

            if(!InAir){shotVelx = -shotDir.x*Recoil.x*0.2f*((shotVelT/7)*(shotVelT/7)*(shotVelT/7));}// reduced recoil when grounded
            else {shotVelx = -shotDir.x*Recoil.x*((shotVelT/7)*(shotVelT/7)*(shotVelT/7));}// cubic function describing shot boost speed in x direction
            
            if(shotVelT == 15){// one frame y shot boost
                if(rb.velocity.y < 0){shotVely = -shotDir.y*Recoil.y*2.5f-rb.velocity.y;}// shooting down is like a double jump instead of a slow down
                else{shotVely = -shotDir.y*Recoil.y*2.5f;}
            }
            else {shotVely = 0;}

            if(InAir){
                if(System.Math.Abs(shotAirBoost) < 32 && (-shotDir.x + shotAirBoost/System.Math.Abs(shotAirBoost)) == 0) {shotAirBoost = -shotDir.x*Recoil.x/10f;}
                else {shotAirBoost += -shotDir.x*Recoil.x/10f;}
            }

        } else {
            shotVelx= 0;
            shotVely= 0;
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
            if(collidersLeft.Length > 0 && runVel < 0){
                runVel = 0;
                runAccel = 0;
                Debug.Log(collidersLeft[0].gameObject.name);
            }
            if(collidersRight.Length > 0 && runVel > 0){
                runVel = 0;
                runAccel = 0;
                Debug.Log(collidersRight[0].gameObject.name);
            }
        }
    }

}
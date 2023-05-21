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
    public Vector2 collisionBottom, collisionSide;
    private int shotT=0;
    public float shotVelT =0;
    public float shotAirBoost = 0;
    private int runT=0;
    public int reload=0;
    private bool sprint;
    public Sprite character;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private ParticleSystem ps;
    private Damage dmg;
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
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
                Revive();
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawCube(new Vector3(collisionBottom.x,collisionBottom.y,0),new Vector3(0.4f,0.1f,1));
        Gizmos.DrawWireSphere(new Vector3(collisionBottom.x,collisionBottom.y,0),0.2f);
        Gizmos.DrawWireSphere(new Vector3(collisionSide.x,collisionSide.y,0),0.2f);
        //Gizmos.DrawCube(new Vector3(0,0,0),new Vector3(10,10,1));
    }

    void FixedUpdate()
    {
        CollisionUpdate();

        if(!dmg.isDead){
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
                if(System.Math.Abs(runVel)<0.2){runVel = 0;}
            }
            


            if(reload>0){reload-=1;}// is here so shoot speed isnt affected by system hardware (rendering times)

            // if((System.Math.Abs(rb.velocity.x) >= topSpeed-1)){//boost function, ie terraria hermes boots
            //     runT +=1;// counts frames at maxspeed (doesnt communicate with Update())
            //     var emission = ps.emission;// particle system
            //     if(runT >=30){// starts aceeleration at 30 frames
            //         runVel = topSpeed*Move*(1f+(runT-30f)/60f); //parentheses is scaling factor ranging from 1 to 1.5
            //         if(runT>=60){// caps at 60
            //             runT = 60;
            //             if(!InAir){emission.rateOverDistance = 1;}// no particles in air
            //             else{emission.rateOverDistance = 0;}
            //         }                
            //     }
            // } else {
            //     var emission = ps.emission;
            //     runT = 0;
            //     emission.rateOverDistance = 0;
            // }

            

            if((jumpT > 0)){// jumpman wahoo
                if(!InAir){
                    jumpVel = 25;
                    jumpT = 0;
                } else {jumpT --;}
            } else {jumpVel = 0;}

            if(Input.GetButton("Jump") && rb.velocity.y > 10){// hold space bar to increase jump height
                rb.gravityScale=6;
            } else {rb.gravityScale = 12;}

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

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.gameObject.CompareTag("Ground")){
    //         InAir = false;
    //         rb.velocity = new Vector2(shotVelx + runVel, 0);// fixes one frame stop when landing, cancelling running boost
    //     }
    // }
    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     string colTag = other.gameObject.tag;
    //     switch(colTag){
    //         case "Ground":
    //             InAir = true;
    //             break;
    //         case "Wall":
    //             shotVelx = 0;
    //             shotAirBoost = 0;
    //             break;
    //     }            
    // }

    public void Revive() {
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

    private void CollisionUpdate(){
        collisionBottom = new Vector2(transform.Find("GroundCheckBox").transform.position.x,transform.Find("GroundCheckBox").transform.position.y);
        collisionSide = new Vector2(rb.transform.position.x+0.2f,rb.transform.position.y);

        //Collider2D[] colliders = Physics2D.OverlapBoxAll(collisionBottom,new Vector2(0.4f,0.1f),groundLayer);
        Collider2D[] collidersBottomGround = Physics2D.OverlapCircleAll(collisionBottom,0.2f,groundLayer);
        Collider2D[] collidersSideGround = Physics2D.OverlapCapsuleAll(transform.position,new Vector2(0.7f,0.2f),CapsuleDirection2D.Horizontal,0,groundLayer);

        if(collidersBottomGround.Length > 0){InAir = false;}
        else {InAir = true;}
        
        if(collidersSideGround.Length > 0){
            shotVelx = 0;
            shotAirBoost = 0;
            if(Move == 0){runVel = 0;}
        }
    }
}
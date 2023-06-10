//using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementOG : MonoBehaviour
{
    // Most veriables with T at the end are used to time between Update() and FixedUpdate(), which are used for rendering and physics respectively
    public float topSpeed;
    public int Move;
    private int MoveTemp;
    private int MoveT;
    private int Vert;
    private int jumpT;
    private int fallJumpT;
    //private float JumpForce = 1000;
    private float Recoil = 10;
    private bool isRight;
    public bool InAir;
    private float jumpVel;
    public float runVel; //x velocity from running
    private float shotVelx, shotVely; // x and y velocity from gun boost
    public Vector2 shotDir = new Vector2(1,1);
    public int shotT=0;
    public float shotVelT =0;
    public int runT=0;
    public int reload=0;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private ParticleSystem ps;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ps = GetComponent<ParticleSystem>();
        var emission = ps.emission;
        emission.rateOverDistance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Move = (int) Input.GetAxisRaw("Horizontal"); // Inputs
        //Jump = Input.GetButtonDown("Jump");
        Vert = (int) Input.GetAxisRaw("Vertical");
        
        if(Move == 1){isRight = true;}// used for sprite rendering left and right
        if(Move == -1){isRight = false;}
        sr.flipX = !isRight; 

        if(Move+MoveTemp==0){ //is true when frame perfect move change. puts one frame of zero speed, doesnt allow player to maintain boost after changing directions
            Move = 0;
            MoveT = 15;
            }

        MoveTemp = Move;

        if(Input.GetButtonDown("Jump")){
            jumpT = 15;
        }
        if(Input.GetButtonDown("Fire1") && reload == 0){
            shotT = 15;
            reload = 50;
            shotDir = new Vector2 (Move,Vert);
        }

        if(jumpT>0){jumpT-=1;}// counting down timers
        if(fallJumpT>0){fallJumpT-=1;}
        if(shotT>0){jumpT-=1;}
        if(MoveT>0){MoveT-=1;}

        
    }

    void FixedUpdate()
    {
        if(MoveT > 0){//reciver for timer variable
            MoveT = 0;
            Move=0;
        }

        runVel = topSpeed*Move;
        if(reload>0){reload-=1;}// is here so shoot speed isnt affected by system hardware (rendering times)

        if((System.Math.Abs(rb.velocity.x) >= topSpeed-1)){//boost function, ie terraria hermes boots
            runT +=1;// counts frames at maxspeed (doesnt communicate with Update())
            var emission = ps.emission;// particle system
            if(runT >=30){// starts aceeleration at 30 frames
                runVel = topSpeed*Move*(1f+(runT-30f)/60f); //parentheses is scaling factor ranging from 1 to 1.5
                if(runT>=60){// caps at 60
                    runT = 60;
                    if(!InAir){emission.rateOverDistance = 1;}// no particles in air
                    else{emission.rateOverDistance = 0;}
                }                
            }
        } else {
            var emission = ps.emission;
            runT = 0;
            emission.rateOverDistance = 0;
        }

        if((jumpT > 0 || fallJumpT > 0)&& !InAir){// jumpman wahoo
            jumpVel = 25;
            jumpT = 0;
            fallJumpT = 0;
        } else {jumpVel = 0;}

        if(Input.GetButton("Jump") && rb.velocity.y > 10){// hold space bar to increase jump height
            rb.gravityScale=6;
        } else {rb.gravityScale = 12;}

        if(shotT > 0 || shotVelT > 0){// shoot to boost character speed
            rb.gravityScale = 12;// used to reset gravity from jump, without this player can mega jump
            if(shotT > 0){shotVelT = 15;}// timer for how much boost
            else if(shotVelT < 0){shotVelT = 0;}
            else{shotVelT -=1;}

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

            shotVelx = -shotDir.x*Recoil*((shotVelT/7)*(shotVelT/7)*(shotVelT/7));// cubic function describing shot boost speed in x direction
            if(shotVelT == 15){// one frame y shot boost
                if(rb.velocity.y < 0){shotVely = -shotDir.y*Recoil*2.5f-rb.velocity.y;}// shooting down is like a double jump instead of a slow down
                else{shotVely = -shotDir.y*Recoil*2.5f;}
            }
            else {shotVely = 0;}

        } else {
            shotVelx= 0;
            shotVely= 0;
        }

        if(rb.velocity.y+shotVely+jumpVel >=30){// sets max vertical speed
            rb.velocity = new Vector2(shotVelx + runVel, 30f);
        }else{
            rb.velocity = new Vector2(shotVelx + runVel, rb.velocity.y + shotVely + jumpVel);// CORE CHAR VELOCITY FUNCTION, adds everything from before. rb.vel.y is there to allow gravity
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.CompareTag("Ground")){
            InAir = false;
            rb.velocity = new Vector2(shotVelx + runVel, 0);// fixes one frame stop when landing, cancelling running boost
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {

        if(other.gameObject.CompareTag("Ground")){
            InAir = true;
        }
    }
}
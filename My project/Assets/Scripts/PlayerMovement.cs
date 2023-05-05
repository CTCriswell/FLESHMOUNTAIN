//using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
    private float runVel;
    private float shotVelx, shotVely;
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
        Move = (int) Input.GetAxisRaw("Horizontal");
        //Jump = Input.GetButtonDown("Jump");
        Vert = (int) Input.GetAxisRaw("Vertical");
        
        if(Move == 1){isRight = true;}
        if(Move == -1){isRight = false;}
        sr.flipX = !isRight; 

        if(Move+MoveTemp==0){
            Move = 0;
            MoveT = 15;
            }

        MoveTemp = Move;

        if(Input.GetButtonDown("Jump")){
            jumpT = 15;
        }
        if(Input.GetButtonDown("Fire1") && reload == 0){
            shotT = 15;
            reload = 300;
            shotDir = new Vector2 (Move,Vert);
        }

        if(jumpT>0){jumpT-=1;}
        if(fallJumpT>0){fallJumpT-=1;}
        if(shotT>0){jumpT-=1;}
        if(reload>0){reload-=1;}
        if(MoveT>0){MoveT-=1;}

        
    }

    void FixedUpdate()
    {
        if(MoveT > 0){
            MoveT = 0;
            Move=0;
        }

        runVel = topSpeed*Move;

        if((System.Math.Abs(rb.velocity.x) >= topSpeed-1)){
            runT +=1;
            var emission = ps.emission;
            if(runT >=60){
                runT = 61;
                runVel = topSpeed*Move*1.5f;
                if(!InAir){emission.rateOverDistance = 1;}
                else{emission.rateOverDistance = 0;}
            }
        } else {
            var emission = ps.emission;
            runT = 0;
            emission.rateOverDistance = 0;
        }

        if((jumpT > 0 || fallJumpT > 0)&& !InAir){
            jumpVel = 25;
            jumpT = 0;
            fallJumpT = 0;
        } else {jumpVel = 0;}

        if(Input.GetButton("Jump") && rb.velocity.y > 10){
            rb.gravityScale=6;
        } else {rb.gravityScale = 12;}

        if(shotT > 0 || shotVelT > 0){
            rb.gravityScale = 12;
            if(shotT > 0){shotVelT = 15;}
            else if(shotVelT < 0){shotVelT = 0;}
            else{shotVelT -=1;}

            shotT = 0;

            if(shotDir.x == 0 && shotDir.y == 0){
                if(isRight){shotDir.x = 1;}
                else{shotDir.x = -1;}
            }

            if(shotDir.x != Move && shotDir.y == 0){runT +=5;}
            else {
                runT = 0;
                var emission = ps.emission;
                emission.rateOverDistance = 0;
                }

            shotVelx = -shotDir.x*Recoil*((shotVelT/7)*(shotVelT/7)*(shotVelT/7));
            if(shotVelT == 15){
                if(rb.velocity.y < 0){shotVely = -shotDir.y*Recoil*2.5f-rb.velocity.y;}
                else{shotVely = -shotDir.y*Recoil*2.5f;}
            }
            else {shotVely = 0;}

        } else {
            shotVelx= 0;
            shotVely= 0;
        }

        if(rb.velocity.y+shotVely+jumpVel >=30){
            rb.velocity = new Vector2(shotVelx + runVel, 30f);
        }else{
            rb.velocity = new Vector2(shotVelx + runVel, rb.velocity.y + shotVely + jumpVel);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.CompareTag("Ground")){
            InAir = false;
            rb.velocity = new Vector2(shotVelx + runVel, 0);
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {

        if(other.gameObject.CompareTag("Ground")){
            InAir = true;
        }
    }
}
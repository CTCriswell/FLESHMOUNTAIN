using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    public Vector2 shotDir = new Vector2(1,1);
    public float shotAirCarry = 0;
    //private float shotVelx, shotVely;
    public sbyte reloadDelay;
    public byte attackDelay;
    public sbyte reloading;
    public byte attacking;
    public string currentWepaon;
    public GameObject weapon_boomerang;
    private Vector2 Recoil = new Vector2(25,42);
    private BoxCollider2D hurtbox;
    private PolygonCollider2D bc;
    private InputMaster inputs;
    //private BoxCollider2D bc;
    //private float collisionRadius = 0.15f;
    private int jumpVel;
    private byte jumpBuffer;
    private Vector2 shootAccel = new Vector2 (0,0);
    private IEnumerator JumpFunc, ShootFunc;
    private int jumpAccel = 32;
    private float inertia;
    private BlastRendering br;
    public Vector2 Aim;
    private bool jump;

    protected override void Start()
    {
        iFrameMax = 50;
        health = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spawner = GetComponentInParent<Spawner>();
        br = GetComponentInChildren<BlastRendering>();
        sr.sprite = sprite_main;
        transform.position = spawner.transform.position;

        isDead = false;
        hurtbox = transform.Find("PlayerHurtBox").GetComponent<BoxCollider2D>();

        JumpFunc = JumpFunc_CR();
        ShootFunc = ShootFunc_CR();

        reloading = 0;
        reloadDelay = 16;
        attackDelay = 5;
        attacking = 0;
        currentWepaon = "default";

        collisionRadius = 0.32f;
        friction = 2.5f;
        inertia = 0;
    }

    protected override void Update()
    {

        //var gamepad = Gamepad.current;

        if(Move.x < 0) isRight = true;// sprite png imported is facing left
        if(Move.x > 0) isRight = false;

        if(isRight){sr.flipX = false;}
        else sr.flipX = true;

        //Aim = gamepad.rightStick.ReadValue();
        

        if(!isDead){
            CollisionUpdate();
            
            //Move = (sbyte) Input.GetAxisRaw("Horizontal"); // Inputs
            //Vert = (sbyte) Input.GetAxisRaw("Vertical");
            

            // if(Input.GetButtonDown("Jump")){
            //     //JumpFunc.Reset();
            //     StartCoroutine(JumpFunc_CR());
            // }
            // if(Input.GetButtonDown("Fire1") && reloading == 0){
            //     getShotDir();
            //     StartCoroutine(ShootFunc_CR());
            // }
            // if(Input.GetButtonDown("Fire2") && attacking == 0){
            //     getShotDir();
            //     StartCoroutine(Attack_CR());
            // }
        } else {
            Velocity = new Vector2 (0,0);
            // if(Input.GetButtonDown("Fire1")){
            //     Respawn();
            // }
        }
    }
    protected void getShotDir(){
        shotDir = new Vector2(Aim.x,Aim.y);
        if(shotDir.x == 0 && shotDir.y == 0){// shoot where character is facing if no input
            if(!isRight){shotDir.x = 1;}
            else{shotDir.x = -1;}
        }
    }
    protected override void FixedUpdate()
    {
        if(!isDead){
            MoveFunc();
            if(System.Math.Abs(Velocity.x) < friction && Move.x == 0){ // stops you if slow enough
                Velocity.x = 0;
                runAccel = 0;
            }
        }
        

        if(!InAir && System.Math.Abs(Velocity.x) > topSpeed){// non running friction

        //    vvv           how much faster than topSpeed/2         Reduce      Extract velocity.x sign
            Velocity.x -= (System.Math.Abs(Velocity.x)-topSpeed/2) *(friction/20)* (Velocity.x/System.Math.Abs(Velocity.x));
        }
        if(rb.velocity.y+Velocity.y > 50){
            rb.velocity = new Vector2(Velocity.x,50);
        } else {
            rb.velocity = new Vector2(Velocity.x,rb.velocity.y + Velocity.y);
        }
        
    }
    protected override void MoveFunc(){
        if(InAir && System.Math.Abs(Move.x) <= 0.1){runAccel = 0;}
        else {
            runAccel = Move.x*inertia*1.25f;
            if(inertia < 1) {inertia += 0.1f;}
        }

        if(System.Math.Abs(rb.velocity.x) >= topSpeed && Velocity.x*runAccel > 0){ // cannot add speed past top speed, but can slow down
            runAccel = 0;
        }

        if(Move.x == 0 && !InAir && System.Math.Abs(Velocity.x)!=0){ // running friction
            Velocity.x -= friction*Velocity.x/System.Math.Abs(Velocity.x);
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

        rb.gravityScale=10f;
        while(jump && rb.velocity.y > 1){// hold space bar to increase jump height
            yield return new WaitForFixedUpdate();
        }
        rb.gravityScale = 20;
    }

    private IEnumerator Attack_CR(){
        switch (currentWepaon){
            case "Boomerang":
                attacking = 1;
                Instantiate(weapon_boomerang);
                yield return new WaitForSeconds(1);
                attacking = 0;
                break;
            default:
                attacking = attackDelay;
                yield return new WaitForSeconds(0.1f);
                attacking = 0;
                break;
        }
        yield return null;
    }

    private IEnumerator ShootFunc_CR(){
        reloading = reloadDelay;
        rb.gravityScale = 20;
        // one frame y shot boost
        shootAccel.y = -shotDir.y*Recoil.y;

        // if(rb.velocity.y < 0){
        //     shootAccel.y += -rb.velocity.y;// shooting down is like a double jump instead of a slow down
        // }
        
        if(shootAccel.y!=0){
            rb.velocity = new Vector2 (rb.velocity.x,shootAccel.y); 
        } 
        // Velocity.y = shootAccel.y;
        // yield return new WaitForFixedUpdate();
        // Velocity.y -= shootAccel.y;

        Velocity.x += -Recoil.x*shotDir.x;

        // if(InAir){ // allows to quickly change horizontal direction unless going too fast
        //     if(System.Math.Abs(shotAirCarry) <= 2 && System.Math.Abs(shotAirCarry)/shotAirCarry + shotDir.x == 0){
        //         Velocity.x += -Recoil.x*shotDir.x*2;
        //         shotAirCarry += shotDir.x;
        //     }
        //     shotAirCarry += shotDir.x;
        // }

        // while(reloading >= reloadDelay - 15){
        //     shootAccel.x = Recoil.x*shotDir.x*(1/343)*(-675+70*(reloading-(reloadDelay - 15))-3*(reloading-(reloadDelay - 15))^2);
        //     if(!InAir){
        //         shootAccel.x *= 1f; // reduced recoil on ground
        //     }
        //     Velocity.x += shootAccel.x;
        //     reloading --;
        //     yield return new WaitForFixedUpdate();
        // }
        StartCoroutine(br.Shoot_CR());
        while(reloading > 0){
            reloading --;
            yield return new WaitForFixedUpdate();
        }
    }

    private void CollisionUpdate(){
        collisionBottom = new Vector2(bc.transform.position.x,bc.transform.position.y-1.62f/2);
        collisionRight = new Vector2(bc.transform.position.x+0.4f,bc.transform.position.y);
        collisionLeft = new Vector2(bc.transform.position.x-0.4f,bc.transform.position.y);

        collidersBottom = Physics2D.OverlapCircleAll(collisionBottom,collisionRadius,groundLayer);
        collidersLeft = Physics2D.OverlapCircleAll(collisionLeft,collisionRadius,groundLayer);
        collidersRight = Physics2D.OverlapCircleAll(collisionRight,collisionRadius,groundLayer);

        if(collidersBottom.Length > 0){
            InAir = false;
            shotAirCarry = 0;
        }
        else {InAir = true;}
        
        if(collidersLeft.Length > 0 || collidersRight.Length > 0){
            shotAirCarry = 0;
            if(collidersLeft.Length > 0 && Velocity.x < 0){
                Velocity.x = 0;
                runAccel = 0;
                //Debug.Log(collidersLeft[0].gameObject.name);
            }
            if(collidersRight.Length > 0 && Velocity.x > 0){
                Velocity.x = 0;
                runAccel = 0;
                //Debug.Log(collidersRight[0].gameObject.name);
            }
        }
    }

    public void OnAim(InputValue value){
        if(!isDead) {
            Aim = value.Get<Vector2>();
        }
    }
    public void OnMove(InputValue value){
        if(!isDead) {
            Move = value.Get<Vector2>();
        }
    }
    public void OnShoot(InputValue value){
        if(reloading == 0 && !isDead){
            getShotDir();
            StartCoroutine(ShootFunc_CR());
        }

        if(isDead){
            Respawn();
        }
    }
    public void OnJump(InputValue value){
        jump = value.isPressed;
        if(jump && !isDead){
            StartCoroutine(JumpFunc_CR());
        }
    }
    public void OnAttack(InputValue value){
        if(attacking == 0 && !isDead){
            getShotDir();
            StartCoroutine(Attack_CR());
        }
    }
}

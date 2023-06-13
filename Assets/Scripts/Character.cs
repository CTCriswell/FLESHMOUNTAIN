using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator ani;
    protected int health;
    public int maxHealth;
    protected bool isDead;
    public bool isRight;
    public bool InAir;
    public Vector2 Move;
    protected float friction;
    public Vector2 Velocity = new Vector2(0,0);
    protected float runAccel = 0;
    public float topSpeed;
    public int iFrames = 0;
    public Sprite sprite_main;
    public Sprite sprite_dead;
    protected SpriteRenderer sr;
    protected CapsuleCollider2D cc;
    protected Rigidbody2D rb;
    protected Spawner spawner;
    public LayerMask groundLayer;
    protected Vector2 collisionBottom, collisionLeft, collisionRight;
    protected Collider2D[] collidersBottom, collidersLeft, collidersRight;
    protected float collisionRadius;
    protected byte iFrameMax;

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(collisionBottom,collisionRadius);
        Gizmos.DrawWireSphere(collisionLeft,collisionRadius);
        Gizmos.DrawWireSphere(collisionRight,collisionRadius);
    }

    protected virtual void Start()
    {
        health = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        cc = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        TryGetComponent<Animator>(out ani);
        spawner = GetComponentInParent<Spawner>();
        if(spawner != null){
            transform.position = spawner.transform.position;
        }
        sr.sprite = sprite_main;
        Move = new Vector2(0,0);
        friction = 1.9f;

        isDead = false;
    }

    protected virtual void Update() { // checks wether to flip sprite, assuming sprite is facing right
        if(Velocity.x < 0) isRight = true;// sprite png is imported facing left
        if(Velocity.x > 0) isRight = false;

        if(isRight){sr.flipX = false;}
        else sr.flipX = true;
        collisionUpdate();
    }

    protected virtual void FixedUpdate() {
        if(ani != null){
            ani.SetFloat("Velocity.y",rb.velocity.y);
            ani.SetBool("inAir",InAir);
            ani.SetBool("isDead",isDead);
        }
        if(!isDead){
            MoveFunc();
            
        } else {
            Velocity = new Vector2(0,0);
        }
    }

    protected virtual void MoveFunc(){
        Velocity.x += Move.x*runAccel;

        if(System.Math.Abs(Velocity.x) >= topSpeed){
            Velocity.x = topSpeed * Velocity.x/System.Math.Abs(Velocity.x);
        }
        if(Move.x == 0 && !InAir && System.Math.Abs(Velocity.x)!=0){ // running friction
            //Debug.Log("Slowing Down");
            Velocity.x -= friction*Velocity.x/System.Math.Abs(Velocity.x);
            if(System.Math.Abs(Velocity.x)<=friction){
                Velocity.x = 0;
            }
        }
        rb.velocity = new Vector2(Velocity.x,rb.velocity.y + Velocity.y);
        Velocity.y = 0;
    }

    public void Respawn(){
        isDead = false;
        transform.position = spawner.transform.position;
        Velocity = new Vector2 (0,0);
        runAccel = 0;
        sr.sprite = sprite_main;
        health = maxHealth;
    }

    public virtual void takeDamage(int ouch){
        if(iFrames == 0){
            health -= ouch;
            if (health<=0){
                isDead = true;
                health = 0;
                sr.sprite = sprite_dead;
            } else {
                StartCoroutine(Invincible());
            }
        }
    }

    public void restoreHealth(int yum){
        health += yum;
        if(health > maxHealth){health = maxHealth;}
    }

    public int getHealth(){
        return health;
    }

    public bool getDead(){
        return isDead;
    }

    /*
    |   ---=---BUG--------
    |   hudgehog gets assblasted when the boomerang floats ontops, applies damage 
    |   when it shouldnt. has to do with iframes i think
    |
    */
    protected IEnumerator Invincible(){
        sr.color = new Color(1,1,1,0.66f); // becomes slightly transparent when damaged
        iFrames = iFrameMax;
        while(iFrames>0){
            iFrames--; // iFrame -= 1
            yield return new WaitForFixedUpdate();
        }
        sr.color = new Color(1,1,1,1);
    }
    protected void collisionUpdate(){
        collisionBottom = new Vector2(cc.transform.position.x,cc.transform.position.y-(cc.size.y/2)*transform.localScale.y+cc.offset.y);
        collisionRight = new Vector2(cc.transform.position.x+(cc.size.x/2)*transform.localScale.x+cc.offset.x,cc.transform.position.y+cc.offset.y);
        collisionLeft = new Vector2(cc.transform.position.x-(cc.size.x/2)*transform.localScale.x+cc.offset.x,cc.transform.position.y+cc.offset.y);

        collidersBottom = Physics2D.OverlapCircleAll(collisionBottom,collisionRadius,groundLayer);
        collidersLeft = Physics2D.OverlapCircleAll(collisionLeft,collisionRadius,groundLayer);
        collidersRight = Physics2D.OverlapCircleAll(collisionRight,collisionRadius,groundLayer);

        if(collidersBottom.Length > 0){
            InAir = false;
        }
        else {InAir = true;}

        if(collidersLeft.Length > 0 || collidersRight.Length > 0){
            if(collidersLeft.Length > 0 && Velocity.x < 0){
                Velocity.x = 0;
                //Debug.Log(collidersLeft[0].gameObject.name);
            }
            if(collidersRight.Length > 0 && Velocity.x > 0){
                Velocity.x = 0;
                //Debug.Log(collidersRight[0].gameObject.name);
            }
        }
    }
}

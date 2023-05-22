using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected int health;
    public int maxHealth;
    protected bool isDead;
    public bool isRight;
    public bool InAir;
    protected Vector2 Velocity = new Vector2(0,0);
    protected float runAccel = 0;
    public float topSpeed;
    public int iFrames = 0;
    public Sprite sprite_main;
    public Sprite sprite_dead;
    protected SpriteRenderer sr;
    protected CapsuleCollider2D cc;
    protected Rigidbody2D rb;
    public Spawner spawner;
    public LayerMask groundLayer;
    
    protected virtual void Start()
    {
        health = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        cc = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr.sprite = sprite_main;

        isDead = false;
    }

    protected virtual void Update() { // checks wether to flip sprite, assuming sprite is facing right
        if(Velocity.x > 0) isRight = true;
        if(Velocity.x < 0) isRight = false;

        if(isRight){sr.flipX = false;}
        else sr.flipX = true;
        
    }

    protected virtual void FixedUpdate() {
        if(!isDead){
            MoveFunc();
        } else {
            Velocity = new Vector2(0,0);
        }
    }

    protected virtual void MoveFunc(){
        Velocity.x += runAccel;
        if(System.Math.Abs(Velocity.x) >= topSpeed){
            Velocity.x = topSpeed * Velocity.x/System.Math.Abs(Velocity.x);
        }
        rb.velocity = new Vector2(Velocity.x,rb.velocity.y);
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

    protected IEnumerator Invincible(){
        sr.color = new Color(1,1,1,0.66f); // becomes slightly transparent when damaged
        iFrames = 50;
        while(iFrames>0){
            iFrames--;
            yield return null;
        }
        sr.color = new Color(1,1,1,1);
    }

}

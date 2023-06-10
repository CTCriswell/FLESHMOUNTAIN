using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float topSpeed;
    public bool isRight;
    public float runVel;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private EnemyDamage dmg;
    public LayerMask groundLayer;
    public int meleeDamage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        dmg = GetComponent<EnemyDamage>();
        topSpeed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(isRight){sr.flipX = true;}
        else sr.flipX = false;
    }

    void FixedUpdate()
    {
        if(!dmg.isDead)
        {
            runFunc();
            CollisionUpdate();

            rb.velocity = new Vector2(runVel,rb.velocity.y);
        }
        else {
            rb.velocity = new Vector2(0,rb.velocity.y);
        }
    }

    void runFunc(){
        if(isRight) runVel = topSpeed;
        else {runVel = -topSpeed;}
    }

    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(new Vector2(transform.position.x + runVel/5f,transform.position.y),0.5f);
    //     //Gizmos.DrawCube(new Vector3(0,0,0),new Vector3(10,10,1));
    // }
    private void CollisionUpdate(){
        Collider2D[] collidersSideGround = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + runVel/5f,transform.position.y),0.5f,groundLayer);
        
        if(collidersSideGround.Length > 0){
            if(isRight) isRight = false;
            else isRight = true;
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(!dmg.isDead && other.gameObject.CompareTag("Player")){
            Damage dmg = other.gameObject.GetComponent<Damage>();
            dmg.takeDamage(meleeDamage);
        }
    }
}

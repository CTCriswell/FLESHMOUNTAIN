using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int projecileDamage;
    public GameObject self;
    private Rigidbody2D rb;
    public Vector2 velocity;
    public Vector2 accel;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void selfDestruct(){
        Destroy(self);
    }

    void FixedUpdate() {
        velocity += accel;
        rb.velocity = new Vector2(velocity.x,rb.velocity.y + velocity.y);
        if(velocity.y != 0){
            velocity.y = 0;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer == 6){
            selfDestruct();
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Boomerang : Weapon
{
    private float accel;
    public Rigidbody2D rb;
    private float initVelocity;
    private bool comingBack;
    private float maxSpeed;

    protected override void Start() {
        base.Start();
        sr.enabled = true;
        rb = GetComponent<Rigidbody2D>();
        initVelocity = 18*play.shotDir.x;
        rb.gravityScale = 0;
        accel = -0.3f*play.shotDir.x;
        transform.position = play.transform.position;
        sr.enabled = true;
        rb.gravityScale = 0;
        damage = 2;
        rb.angularVelocity = play.shotDir.x*500;
        Destroy(gameObject,4);
        rb.velocity = new Vector2(initVelocity,0);
        maxSpeed = 20;
    }
    private void FixedUpdate() {
        rb.velocity = new Vector2(rb.velocity.x + accel,0);
        if(rb.velocity.x*initVelocity < 0){
            comingBack = true;
        }
        if(System.Math.Abs(rb.velocity.x) > maxSpeed){
            rb.velocity = new Vector2(maxSpeed * System.Math.Abs(rb.velocity.x)/rb.velocity.x,rb.velocity.y);
        }
    }
    protected override void catches()
    {
        if(comingBack){
            Destroy(gameObject);
        }
    }
}

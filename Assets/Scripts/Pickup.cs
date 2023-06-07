using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected AudioSource aux;
    protected BoxCollider2D bc;
    private Rigidbody2D rb;
    protected Player play;
    private byte t;
    private float yPosInit;
    // Start is called before the first frame update
    void Start()
    {
        t = 0;
        sr = GetComponent<SpriteRenderer>();
        aux = GetComponent<AudioSource>();
        play = GameObject.Find("PlayerSpawner").GetComponentInChildren<Player>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        yPosInit = rb.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.position = new Vector3(rb.position.x,yPosInit - 0.25f*Mathf.Cos(4*t*2*Mathf.PI/256),0);
        t++;
    }

    protected abstract void onTouch();

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            onTouch();
        }
    }

}

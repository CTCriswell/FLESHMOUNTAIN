using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected Player play;
    protected SpriteRenderer sr;
    protected BoxCollider2D bc;
    public int damage;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        play = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        sr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void hitCheck(Collider2D other){
        Enemy E = other.gameObject.GetComponentInParent<Enemy>();
        if(E != null && !E.getDead()){
            E.takeDamage(damage);
        }
    }
    protected virtual void catches(){
        // nothing usually
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Enemy")){
            hitCheck(other);
        }
        if(other.CompareTag("Player")){
            catches();
        }
    }
}

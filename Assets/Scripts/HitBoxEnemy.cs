using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxEnemy : MonoBehaviour
{
    private BoxCollider2D hit;
    private Enemy en;
    private Player play;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        hit = GetComponent<BoxCollider2D>();
        en = GetComponentInParent<Enemy>();
    }

    void Update()
    {
        
    }
    protected bool DamagePlayerCheck(Collider2D other){
        if(other.gameObject.layer == 8){
            play = other.gameObject.GetComponentInParent<Player>();
            if(!play.getDead()){
                play.takeDamage(damage);
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(!en.getDead()){
            DamagePlayerCheck(other);
        }
    }
}

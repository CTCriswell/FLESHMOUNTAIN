using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private int health;
    public int maxHealth;
    public bool isDead = false;
    private SpriteRenderer sr;
    public Sprite deadSprite;
    

    private void Start() {
        health = maxHealth;
        sr = GetComponent<SpriteRenderer>();       
    }

    public void takeDamage(int ouch){
        health -= ouch;
        if (health<=0){
            isDead = true;
            health = 0;
            sr.sprite = deadSprite;
            }
    }

    public void restoreHealth(int yum){
        health += yum;
        if(health > maxHealth){health = maxHealth;}
    }
    public int getHealth(){
        return health;
    }
}



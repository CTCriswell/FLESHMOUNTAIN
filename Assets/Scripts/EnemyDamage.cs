using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EnemyDamage : MonoBehaviour
{
    private int health;
    public int maxHealth;
    public bool isDead = false;
    private SpriteRenderer sr;
    public Sprite deadSprite;
    public GameObject DmgText;
    private BoxCollider2D bc;

    private void Start() {
        health = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
    }

    public void takeDamage(int ouch){
        health -= ouch;
        SpawnText(ouch);
        if (health<=0){
            isDead = true;
            health = 0;
            sr.sprite = deadSprite;
            //bc.size = new Vector2(0,0);
            }
    }

    public void restoreHealth(int yum){
        health += yum;
        if(health > maxHealth){health = maxHealth;}
    }
    public int getHealth(){
        return health;
    }
    public void SpawnText(int dmg){
        GameObject newSpawnTxt = Instantiate(DmgText, transform.position, Quaternion.identity);
        TMP_Text dmgObj;
        dmgObj = newSpawnTxt.GetComponent<TMP_Text>();
        dmgObj.text = ""+dmg;
        StartCoroutine(SpawnTextCR(newSpawnTxt));
        Destroy(newSpawnTxt, 1.5f);
    }
    private IEnumerator SpawnTextCR(GameObject text){
        int t = 150;
        //Debug.Log("textStart");
        while(t>0){
            //Debug.Log("Move");
            text.transform.Translate(new Vector3(0.01f,0.01f,0f));
            t--;
            yield return null;
        }
    }
}



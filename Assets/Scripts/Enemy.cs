using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : Character
{
    protected int meleeDamage;
    private BoxCollider2D[] hitbox; 
    public GameObject DmgText;
    public GameObject player;
    protected Player play;
    protected bool playerIsRight;
    protected float playerxDis;
    protected override void Start()
    {
        base.Start();
        hitbox = GetComponentsInChildren<BoxCollider2D>();// gets all of the hitbox children and puts in this array
        collisionRadius=0.03f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        playerxDis = play.transform.position.x - transform.position.x;

        if(playerxDis > 0) playerIsRight = true;
        else playerIsRight = false;
    }


    public override void takeDamage(int ouch){
        if(iFrames==0) StartCoroutine(SpawnText_CR(ouch));
        base.takeDamage(ouch);
    }
    private IEnumerator SpawnText_CR(int dmg){ // subroutine to spit out text when damaged
        GameObject newSpawnTxt = Instantiate(DmgText, transform.position, Quaternion.identity);
        TMP_Text dmgObj;
        dmgObj = newSpawnTxt.GetComponent<TMP_Text>();
        dmgObj.text = ""+dmg;
        int t = 150;
        while(t>0){
            newSpawnTxt.transform.Translate(new Vector3(0.001f,0.001f,0f));
            t--;
            yield return null;
        }
        Destroy(newSpawnTxt, 1.5f);
    }
    public int getDamage(){
        return meleeDamage;
    }

    
}

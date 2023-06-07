using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : Pickup
{
    public byte health;
    protected override void onTouch(){
        StartCoroutine(touch_CR());
    }
    private IEnumerator touch_CR(){
        if(play.getHealth() != play.maxHealth){
            aux.Play();
            Destroy(gameObject,1);
            bc.size = new Vector2(0.001f,0.001f);
            play.restoreHealth(health);
            yield return new WaitForSeconds(0.25f);
            sr.enabled = false;
        }
    }
}

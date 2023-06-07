using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Boomerang : Pickup
{
    protected override void onTouch(){
        StartCoroutine(touch_CR());
    }
    private IEnumerator touch_CR(){
        play.currentWepaon = "Boomerang";
        yield return null;
        Destroy(gameObject);
    }
    
}

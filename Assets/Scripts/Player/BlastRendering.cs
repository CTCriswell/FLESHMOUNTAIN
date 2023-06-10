using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastRendering : Weapon
{
    private Vector3 unitRelativePos;
    public float angle = 0;
    private float distance = 3.2f;

    // Update is called once per frame
    // void Update()
    // {
    //     if(p.reloading > p.reloadDelay - 15){// only renders blast when gun is fired
    //         sr.enabled = true;
    //         bc.size = new Vector2(42.33f,17);
    //         transform.position = new Vector3(target.transform.position.x+p.shotDir.x*distance,target.transform.position.y+p.shotDir.y*distance,0);// sets position to player with an offset based on shot direction

    //         unitRelativePos = (transform.position-target.transform.position);// vector describing relative position
    //         //unitRelativePos = unitRelativePos/(float)(System.Math.Sqrt(System.Math.Pow(unitRelativePos.x,2)+System.Math.Pow(unitRelativePos.y,2)));// inverse square root to normalize vector
    //         // ^ didnt need to normalize vector

    //         angle =360 + (float)System.Math.Atan2(unitRelativePos.y,unitRelativePos.x)*(float)(180/System.Math.PI);// Gets angle of vector, I <3 UnitCircle
    //         if(angle/angle +1 != 2){angle = 0f;}// catches if angle is NaN
    //         transform.eulerAngles = new Vector3(360f,360f,angle);// sets angle
    //     } else {
    //         sr.enabled = false;
    //         bc.size = new Vector2(0,0);
    //     }
    // }

    public IEnumerator Shoot_CR(){
        bc.size = new Vector2(42.33f,17);
        transform.position = new Vector3(play.transform.position.x+play.shotDir.x*distance,play.transform.position.y+play.shotDir.y*distance,0);// sets position to player with an offset based on shot direction

        unitRelativePos = (transform.position-play.transform.position);// vector describing relative position
        //unitRelativePos = unitRelativePos/(float)(System.Math.Sqrt(System.Math.Pow(unitRelativePos.x,2)+System.Math.Pow(unitRelativePos.y,2)));// inverse square root to normalize vector
        // ^ didnt need to normalize vector

        angle =360 + (float)System.Math.Atan2(unitRelativePos.y,unitRelativePos.x)*(float)(180/System.Math.PI);// Gets angle of vector, I <3 UnitCircle
        if(angle/angle +1 != 2){angle = 0f;}// catches if angle is NaN
        transform.eulerAngles = new Vector3(360f,360f,angle);// sets angle
        sr.enabled = true;
        yield return new WaitForSeconds(0.3f);
        sr.enabled = false;
        bc.size = new Vector2(0,0);
    }

    protected override void hitCheck(Collider2D other)
    {
        if (play.reloading> play.reloadDelay - 15 && other.CompareTag("Enemy"))
        {
            Enemy E = other.gameObject.GetComponentInParent<Enemy>();
            if(E != null && !E.getDead()){
                E.takeDamage(damage);
            }
        }
    }
}

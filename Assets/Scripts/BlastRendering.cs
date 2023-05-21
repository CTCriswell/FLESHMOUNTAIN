using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastRendering : MonoBehaviour
{
    public GameObject target;
    private PlayerMovement pm;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private Vector3 unitRelativePos;
    public float angle = 0;
    private float distance = 2;
    public int gunDamage;
    // Start is called before the first frame update
    void Start()
    {
        pm = target.GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        sr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(pm.shotVelT > 0){// only renders blast when gun is fired
            sr.enabled = true;
            bc.size = new Vector2(1,1);
        } else {
            sr.enabled = false;
            bc.size = new Vector2(0,0);
        }

        transform.position = new Vector3(target.transform.position.x+pm.shotDir.x*distance,target.transform.position.y+pm.shotDir.y*distance,0);// sets position to player with an offset based on shot direction

        unitRelativePos = (transform.position-target.transform.position);// vector describing relative position
        //unitRelativePos = unitRelativePos/(float)(System.Math.Sqrt(System.Math.Pow(unitRelativePos.x,2)+System.Math.Pow(unitRelativePos.y,2)));// inverse square root to normalize vector
        // ^ didnt need to normalize vector

        angle =360 + (float)System.Math.Atan2(unitRelativePos.y,unitRelativePos.x)*(float)(180/System.Math.PI);// Gets angle of vector, I <3 UnitCircle
        if(angle/angle +1 != 2){angle = 0f;}// catches if angle is NaN
        transform.eulerAngles = new Vector3(360f,360f,angle);// sets angle

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(pm.shotVelT>0 && other.CompareTag("Enemy") && other.TryGetComponent<EnemyDamage>(out EnemyDamage ED))
        {
            if(!ED.isDead)ED.takeDamage(gunDamage);
        }
    }

    // void OnDrawGizmos()
    // {
    //     bc = GetComponent<BoxCollider2D>();
    //     Gizmos.color = Color.yellow;
    // }
}

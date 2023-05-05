using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastRendering : MonoBehaviour
{
    public GameObject target;
    private PlayerMovement pm;
    private SpriteRenderer sr;
    private Vector3 unitRelativePos;
    public float angle = 0;
    // Start is called before the first frame update
    void Start()
    {
        pm = target.GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();

        sr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(pm.shotVelT > 0){
            sr.enabled = true;
        } else {
            sr.enabled = false;
        }

        transform.position = new Vector3(target.transform.position.x+pm.shotDir.x,target.transform.position.y+pm.shotDir.y,0);

        unitRelativePos = (transform.position-target.transform.position);
        unitRelativePos = unitRelativePos/(float)(System.Math.Sqrt(System.Math.Pow(unitRelativePos.x,2)+System.Math.Pow(unitRelativePos.y,2)));

        angle =360 + (float)System.Math.Atan2(unitRelativePos.y,unitRelativePos.x)*(float)(180/System.Math.PI);
        if(angle/angle +1 != 2){angle = 0f;}
        transform.eulerAngles = new Vector3(360f,360f,angle);

    }
}

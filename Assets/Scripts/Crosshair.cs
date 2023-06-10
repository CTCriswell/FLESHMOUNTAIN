using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private Player play;
    private SpriteRenderer sr;
    private Vector2 cros_aim;
    // Start is called before the first frame update
    void Start()
    {
        cros_aim = new Vector2(0,0);
        play = GetComponentInParent<Player>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        cros_aim = play.Aim;
        if(cros_aim.x == 0 && cros_aim.y == 0){// shoot where character is facing if no input
            if(!play.isRight){cros_aim.x = 1;}
            else{cros_aim.x = -1;}
        }

        transform.position = play.transform.position + new Vector3(3*cros_aim.x,3*cros_aim.y,0);
    }
}

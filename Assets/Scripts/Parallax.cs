using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float len, temp;
    private Vector2 startPos;
    public Vector2 parallaxEff;
    public GameObject cam;
    private Vector2 distance;
    void Start()
    {
        startPos = transform.position;
        len = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        temp = cam.transform.position.x * (1-parallaxEff.x);

        distance = new Vector2 (cam.transform.position.x * parallaxEff.x, cam.transform.position.y * parallaxEff.y);
        transform.position = startPos + distance;

        if(temp > startPos.x + 1.5*len){
            startPos.x += 3*len;
        } else if (temp < startPos.x - 1.5*len){
            startPos.x -= 3*len;
        }
    }
}

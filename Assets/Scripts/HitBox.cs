using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox1 : MonoBehaviour
{
    private BoxCollider2D hit;
    private Enemy en;
    // Start is called before the first frame update
    void Start()
    {
        hit = GetComponent<BoxCollider2D>();
        en = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

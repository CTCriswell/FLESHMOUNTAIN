using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public BoxCollider2D hurt;
    public Player play;
    // Start is called before the first frame update
    void Start()
    {
        hurt = GetComponent<BoxCollider2D>();
        play = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


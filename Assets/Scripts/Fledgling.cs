using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fledgling : Enemy
{
    protected override void Start() {
        base.Start();
        runAccel = -1;
        topSpeed = 5;
        meleeDamage = 3;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        runAccel *= -1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class NormalUI : MonoBehaviour
{
    public GameObject target;
    public GameObject self;
    private Damage dmg;
    public TMP_Text[] healthNum;
    //public GameObject normalUI;
    // Start is called before the first frame update
    void Start()
    {
        healthNum = GetComponentsInChildren<TMP_Text>();
        dmg = target.GetComponent<Damage>();
    }

    // Update is called once per frame
    void Update()
    {
        healthNum[1].text = (""+dmg.getHealth());
        if(dmg.isDead){
            self.transform.Find("UsualPanel").gameObject.SetActive(false);
            self.transform.Find("DeathPanel").gameObject.SetActive(true);
        } else {
            self.transform.Find("UsualPanel").gameObject.SetActive(true);
            self.transform.Find("DeathPanel").gameObject.SetActive(false);
        }
    }
}

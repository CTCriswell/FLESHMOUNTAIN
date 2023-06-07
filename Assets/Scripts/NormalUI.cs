using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class NormalUI : MonoBehaviour
{
    public GameObject target;
    public GameObject self;
    private Player p;
    public TMP_Text[] healthNum;
    //public GameObject normalUI;
    // Start is called before the first frame update
    void Start()
    {
        healthNum = GetComponentsInChildren<TMP_Text>();
        p = target.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        healthNum[1].text = (""+p.getHealth());
        if(p.getDead()){
            self.transform.Find("UsualPanel").gameObject.SetActive(false);
            self.transform.Find("DeathPanel").gameObject.SetActive(true);
        } else {
            self.transform.Find("UsualPanel").gameObject.SetActive(true);
            self.transform.Find("DeathPanel").gameObject.SetActive(false);
        }
    }
}

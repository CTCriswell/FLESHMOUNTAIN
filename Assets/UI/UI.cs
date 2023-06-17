using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    public GameObject Player;
    public Player play;
    private Label healthNum;
    private IMGUIContainer DeathScreen;

    private void Start() 
    {
        Player = GameObject.FindWithTag("Player");
        play = Player.GetComponent<Player>();

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        healthNum = root.Q<Label>("healthNumber");
        DeathScreen = root.Q<IMGUIContainer>("DeathFlash");
    }
    private void Update()
    {
        healthNum.text = (""+play.getHealth());
        if(play.getDead()){
            DeathScreen.visible = true;
        } else {
            DeathScreen.visible = false;
        }
    }
}



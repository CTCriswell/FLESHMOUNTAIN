using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject topLevel;
    public GameObject options;
    public PlayerInput inMas;
    public bool isPasued;
    public int conType;
    public int inType;
    public GameObject controlGM;
    public GameObject inputGM;
    public TMP_Text controlText;
    public TMP_Text inputText;
    // Start is called before the first frame update
    void Start()
    {
        inMas = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        Cursor.visible = false;
        isPasued = false;
        conType = 0;
        controlText = controlGM.GetComponent<TMP_Text>();
        inType = 0;
        inputText = inputGM.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!isPasued){
                pause();
            }
            else if(options.activeSelf){
                back();
            } else {
                resume();
            }
        }
    }
    public void quit(){
        Application.Quit();
    }
    public void pause(){
        isPasued = true;
        Time.timeScale = 0;
        topLevel.SetActive(true);
        Cursor.visible = true;
    }
    public void resume(){
        isPasued = false;
        Time.timeScale = 1;
        topLevel.SetActive(false);
        Cursor.visible = false;
    }
    public void openOptionsMenu(){
        topLevel.SetActive(false);
        options.SetActive(true);
    }
    public void back(){
        options.SetActive(false);
        topLevel.SetActive(true);
    }
    public void cType(){
        conType ++;
        if(conType >= 2){conType = 0;}
        switch(conType){
            case 0:
                inMas.SwitchCurrentActionMap("Simple");
                controlText.text = "Simple";
                break;
            case 1:
                inMas.SwitchCurrentActionMap("DualStick");
                controlText.text = "Dual Stick";
                break;
            default:
                inMas.SwitchCurrentActionMap("Simple");
                controlText.text = "Simple";
                break;
        }
    }
    public void cInput(){
    //     inType ++;
    //     if(inType >= 3){inType = 0;}
    //     switch(inType){
    //         case 0:
    //             inMas.SwitchCurrentActionMap("Simple");
    //             inputText.text = "Simple";
    //             break;
    //         case 1:
    //             inMas.SwitchCurrentActionMap("DualStick");
    //             inputText.text = "Dual Stick";
    //             break;
    //         case 2:
    //             inMas.SwitchCurrentActionMap("DualStick");
    //             inputText.text = "Dual Stick";
    //             break;
    //         default:
    //             inMas.SwitchCurrentActionMap("Simple");
    //             inputText.text = "Simple";
    //             break;
    //     }
    }
}

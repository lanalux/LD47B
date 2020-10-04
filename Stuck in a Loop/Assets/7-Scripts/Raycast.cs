using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Raycast : MonoBehaviour
{
    public static Raycast Instance {get;set;}
    Camera cam;
    [SerializeField] float interactionDistance;
    GameObject currentTarget;
    [SerializeField] CanvasGroup hoverCG;
    [SerializeField] GameObject itemInfo;
    int layerMask = (1<<10);

    [SerializeField] Text hoveringText;


    void Awake(){
        if(Instance==null){
            Instance=this;
        }
    }

    void Start(){
        cam = Camera.main;
    }

    void Update(){
        if(!Static.paused){

            if(Input.GetKeyDown(KeyCode.Escape)){
                HUD.Instance.PauseGame();
            }

            RaycastHit hit;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
            if(Physics.Raycast(ray, out hit, interactionDistance)){
                currentTarget = hit.transform.gameObject;
                ActionItem actionScript = currentTarget.GetComponent<ActionItem>();
                FabricatorItem fabricatorScript = currentTarget.GetComponent<FabricatorItem>();
                if(fabricatorScript){
                    UpdateHover("Open Fabricator");
                    if(Input.GetMouseButtonDown(0)){
                        FabricatorControls.Instance.OpenFabricatorMenu();
                    }
                } else {
                    if(actionScript){
                        if(actionScript.actionNum == 0){
                            string infoText = HUD.Instance.allActions[actionScript.actionNum].description + ". You sleep until 8am.";
                            UpdateHover(HUD.Instance.allActions[actionScript.actionNum].name, infoText);
                            if(Input.GetMouseButtonDown(0)){
                                HUD.Instance.Sleep();
                            }
                        } else {
                            if(!HUD.Instance.allActions[actionScript.actionNum].requireHealthAndHappiness || HUD.Instance.allActions[actionScript.actionNum].requireHealthAndHappiness && HUD.Instance.currentHealth>=50.0f && HUD.Instance.currentSpirit>=50.0f && HUD.Instance.currentShip>=50.0f){
                                if(actionScript){
                                    // Check if stats are within bounds
                                    float timeReq = HUD.Instance.allActions[actionScript.actionNum].timeTaken[HUD.Instance.allActions[actionScript.actionNum].currentLevel]*60.0f;
                                    if(timeReq <= HUD.Instance.timeLeftInDay){
                                        string infoText = HUD.Instance.allActions[actionScript.actionNum].description + ". Takes " + HUD.Instance.allActions[actionScript.actionNum].timeTaken[HUD.Instance.allActions[actionScript.actionNum].currentLevel] + " hours.";
                                        UpdateHover(HUD.Instance.allActions[actionScript.actionNum].name, infoText);
                                        if(Input.GetMouseButtonDown(0)){
                                            HUD.Instance.PerformAction(actionScript.actionNum);
                                        }
                                    } else {
                                        string infoText =  HUD.Instance.allActions[actionScript.actionNum].description   + ". Takes " + HUD.Instance.allActions[actionScript.actionNum].timeTaken[HUD.Instance.allActions[actionScript.actionNum].currentLevel] + " hours."+ "\n There not enough time left to do this.";
                                        UpdateHover(HUD.Instance.allActions[actionScript.actionNum].name, infoText);
                                    }
                                }
                            } else {
                                string infoText = HUD.Instance.allActions[actionScript.actionNum].description + ". Takes " + HUD.Instance.allActions[actionScript.actionNum].timeTaken[HUD.Instance.allActions[actionScript.actionNum].currentLevel] + " hours." + "\n Your health, happiness and ship must be >50% to do this.";
                                UpdateHover(HUD.Instance.allActions[actionScript.actionNum].name , infoText);
                                MakeTextRed();
                            }
                        }
                    }
                }
            } else {
                HideHover();
            }
        } else {
            HideHover();
        }
    }



    void UpdateHover(string title){
        hoveringText.color = Color.white;
        hoverCG.alpha=1.0f;
        hoveringText.text = title.ToUpper();
    }

    void UpdateHover(string title, string infoText){
        hoveringText.color = Color.white;
        hoverCG.alpha=1.0f;
        hoveringText.text = title.ToUpper();
        itemInfo.SetActive(true);
        itemInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = title.ToUpper();
        itemInfo.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = infoText;
    }

    void MakeTextRed(){
        hoveringText.color = Color.grey;
    }

    void HideHover(){
        itemInfo.SetActive(false);
        hoverCG.alpha=0.0f;
        hoveringText.text = "";
    }
}

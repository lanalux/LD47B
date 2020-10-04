using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class FabricatorControls : MonoBehaviour
{
    public static FabricatorControls Instance {get;set;}

    [SerializeField] GameObject fabricatorGO, actionPrefab;
    [SerializeField] CanvasGroup fabricatorCG;
    [SerializeField] Transform actionHolder;
    [SerializeField] Text currentMoney;

    void Awake(){
        if(Instance==null){
            Instance=this;
        }
    }

    public void OpenFabricatorMenu(){
        HUD.Instance.SwitchToMouse();
        fabricatorGO.SetActive(true);
        currentMoney.text = "Current Money: $" + HUD.Instance.currentMoney.ToString();
        for(int i=0; i<HUD.Instance.allActions.Count;i++){
            int _i = i;
            GameObject newUI = Instantiate(actionPrefab, actionHolder);
            // newUI.transform.GetChild(0).GetComponent<Image>().sprite = 
            newUI.transform.GetChild(1).GetComponent<Text>().text = HUD.Instance.allActions[i].name;
            newUI.transform.GetChild(2).GetComponent<Text>().text = "Level " + HUD.Instance.allActions[i].currentLevel.ToString();
            newUI.transform.GetChild(3).GetComponent<Text>().text = HUD.Instance.allActions[i].description;
            
            if(HUD.Instance.allActions[i].currentLevel<HUD.Instance.allActions[i].timeTaken.Count-1){
                if(HUD.Instance.currentMoney>=HUD.Instance.allActions[i].expReq[HUD.Instance.allActions[i].currentLevel]){
                    newUI.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "Upgrade for $"+ HUD.Instance.allActions[i].expReq[HUD.Instance.allActions[i].currentLevel].ToString();
                    newUI.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(()=>{
                        Upgrade(_i);
                    });
                } else {
                    newUI.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "$"+ HUD.Instance.allActions[i].expReq[HUD.Instance.allActions[i].currentLevel].ToString() + " required";
                    
                    newUI.transform.GetChild(4).GetComponent<Button>().interactable=false;
                }
            } else {
                // newUI.transform.GetChild(2).GetComponent<Text>().text = "Max Level";
                newUI.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = "Max Level";
                newUI.transform.GetChild(4).GetComponent<Button>().interactable=false;
                
            }
            
        }
        fabricatorCG.DOFade(1.0f,1.0f);
    }

    public void CloseFabricatorMenu(){
        foreach(Transform child in actionHolder){
            Destroy(child.gameObject);
        }
        fabricatorCG.DOFade(0.0f,0.0f).OnComplete(()=>{
            fabricatorGO.SetActive(false);
            HUD.Instance.UpdateHUD();
            HUD.Instance.SwitchToPlayer();
        });
    }
    
    public void Upgrade(int actionNum){
        if((HUD.Instance.allActions[actionNum].currentLevel) < HUD.Instance.allActions[actionNum].expReq.Count){
            HUD.Instance.currentMoney-=HUD.Instance.allActions[actionNum].expReq[HUD.Instance.allActions[actionNum].currentLevel];
            if(HUD.Instance.allActions[actionNum].GO[HUD.Instance.allActions[actionNum].currentLevel])
                HUD.Instance.allActions[actionNum].GO[HUD.Instance.allActions[actionNum].currentLevel].SetActive(false);
            HUD.Instance.allActions[actionNum].currentLevel++;
            HUD.Instance.allActions[actionNum].GO[HUD.Instance.allActions[actionNum].currentLevel].SetActive(true);
            CloseFabricatorMenu();
            // CloseSuccess();
            //TODO show successful upgrade UI
        }
    }

    public void CloseSuccess(){
        HUD.Instance.SwitchToPlayer();
    }
}

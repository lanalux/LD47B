using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Controls : MonoBehaviour
{
   
   [SerializeField] CanvasGroup introCG, introTextCG;
   [SerializeField] GameObject introGO, introTextGO;
    void Start()
    {
        
        HUD.Instance.SwitchToMouse();
        introGO.SetActive(true);
        introCG.DOFade(1.0f, 2.0f).OnComplete(()=>{
            introTextGO.SetActive(true);
            introTextCG.DOFade(1.0f, 2.0f);
        });
    }

    public void CloseIntro(){
        HUD.Instance.SwitchToPlayer();
        introCG.DOFade(0.0f,1.0f);
        introTextCG.DOFade(0.0f,1.0f).OnComplete(()=>{
            introGO.SetActive(false);
            introGO.SetActive(false);
            
        });
    }

}

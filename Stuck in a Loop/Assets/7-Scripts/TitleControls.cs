using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleControls : MonoBehaviour
{

    [SerializeField] CanvasGroup overlay;
    
   [SerializeField] CanvasGroup introCG, introTextCG, startItemsCG;
   [SerializeField] GameObject introGO, introTextGO, startItemsGO;

    bool canClick=true;

    public void StartGame(){
        if(canClick){
            canClick=false;
            introGO.SetActive(true);
            startItemsCG.DOFade(0.0f,1.0f).OnComplete(()=>{
                startItemsGO.SetActive(false);
                introCG.DOFade(1.0f,2.0f).OnComplete(()=>{
                    introTextGO.SetActive(true);
                    canClick=true;
                    introTextCG.DOFade(1.0f,2.0f);
                });
            });

        }
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void Begin(){
        canClick=false;
        overlay.DOFade(1.0f,1.0f).OnComplete(()=>{
            SceneManager.LoadScene("Main");
        });
    }
}

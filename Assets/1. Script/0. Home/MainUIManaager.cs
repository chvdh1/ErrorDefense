using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIManaager : MonoBehaviour
{
    public GameObject homeCanvas;
    public GameObject singleCanvas;
    public GameObject battleCanvas;
    public GameObject settingPop;

    public MainManager mainManager;

    private void Start()
    {
        mainManager = FindAnyObjectByType<MainManager>();
    }


    public void SingleBt()
    {
        //최고 점수, 플레이 시간, 시너지 알려주는 팝업 창.
        Debug.Log("싱글");
       singleCanvas.SetActive(true);
       homeCanvas.SetActive(false);
       battleCanvas.SetActive(false);
       settingPop.SetActive(false);
    }

    public void BattleBt()
    {
        //최고 점수, 플레이 시간, 시너지 알려주는 팝업 창.
        Debug.Log("배틀");
       battleCanvas.SetActive(true);
       singleCanvas.SetActive(false);
       homeCanvas.SetActive(false);
       settingPop.SetActive(false);
    }
    public void HomeBt()
    {
        //최고 점수, 플레이 시간, 시너지 알려주는 팝업 창.
        Debug.Log("홈");
       homeCanvas.SetActive(true);
       singleCanvas.SetActive(false);
       battleCanvas.SetActive(false);
       settingPop.SetActive(false);
    }
    public void SettingBt()
    {
        Debug.Log("설정");
        if (!settingPop.activeSelf)
           settingPop.SetActive(true);
        else
           settingPop.SetActive(false);
    }

    public void PlaySingleGmae()
    {
        SceneManager.LoadScene(1);
        StartCoroutine(mainManager.StartGame());
    }


    public void PlayBattleGmae()
    {
        SceneManager.LoadScene(2);
    }
}

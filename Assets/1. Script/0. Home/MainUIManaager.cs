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
        //�ְ� ����, �÷��� �ð�, �ó��� �˷��ִ� �˾� â.
        Debug.Log("�̱�");
       singleCanvas.SetActive(true);
       homeCanvas.SetActive(false);
       battleCanvas.SetActive(false);
       settingPop.SetActive(false);
    }

    public void BattleBt()
    {
        //�ְ� ����, �÷��� �ð�, �ó��� �˷��ִ� �˾� â.
        Debug.Log("��Ʋ");
       battleCanvas.SetActive(true);
       singleCanvas.SetActive(false);
       homeCanvas.SetActive(false);
       settingPop.SetActive(false);
    }
    public void HomeBt()
    {
        //�ְ� ����, �÷��� �ð�, �ó��� �˷��ִ� �˾� â.
        Debug.Log("Ȩ");
       homeCanvas.SetActive(true);
       singleCanvas.SetActive(false);
       battleCanvas.SetActive(false);
       settingPop.SetActive(false);
    }
    public void SettingBt()
    {
        Debug.Log("����");
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

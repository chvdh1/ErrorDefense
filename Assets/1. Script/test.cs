using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    private void Awake()
    {
        Application.targetFrameRate = 60; //실행 프레임 속도 60프레임으로 고정 시키기.. 코드
        QualitySettings.vSyncCount = 0;
        //모니터 주사율(플레임율)이 다른 컴퓨터일 경우 캐릭터 조작시 빠르게 움직일 수 있다.
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Time.deltaTime);


    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.deltaTime);
    }
}

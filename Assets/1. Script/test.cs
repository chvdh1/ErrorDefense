using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    private void Awake()
    {
        Application.targetFrameRate = 60; //���� ������ �ӵ� 60���������� ���� ��Ű��.. �ڵ�
        QualitySettings.vSyncCount = 0;
        //����� �ֻ���(�÷�����)�� �ٸ� ��ǻ���� ��� ĳ���� ���۽� ������ ������ �� �ִ�.
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

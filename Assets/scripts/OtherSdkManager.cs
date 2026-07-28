using AdjustSdk;
using SolarEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherSdkManager : MonoBehaviour
{
    public static OtherSdkManager Instance;

    public static bool IsInit = false;
    private void Awake()
    {
        Instance = this;
        Init();
    }


    public void Init()
    {
        Debug.Log("Other SDK初始化");

        IsInit = true;
        AdjustInit();
        SolarEngineInit();
    }

    private void AdjustInit()
    {
        string adjust_AppToken = "cgxrt57p00e8";
        AdjustConfig adjustConfig = new AdjustConfig(adjust_AppToken, AdjustEnvironment.Production);
        // ...
        Adjust.InitSdk(adjustConfig);
    }

    private void SolarEngineInit()
    {
        string AppKey = "abfb896423afdd36";
        SEConfig seConfig = new SEConfig();
        seConfig.initCompletedCallback = (e) =>
        {
            Debug.Log("热力引擎初始化成功");
        };
        SolarEngine.Analytics.initSeSdk(AppKey, seConfig);
    }
}

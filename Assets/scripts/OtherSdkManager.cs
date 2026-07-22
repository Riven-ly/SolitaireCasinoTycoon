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
    }


    public void Init()
    {
        Debug.Log("Other SDK≥ı ºªØ");

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
        SolarEngine.Analytics.initSeSdk(AppKey, seConfig);
    }
}

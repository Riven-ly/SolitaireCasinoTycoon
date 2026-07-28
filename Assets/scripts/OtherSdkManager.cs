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

    public void CustomEvent(string eName,string _key,string _value)
    {
        Dictionary<string, object> customAttributes = new Dictionary<string, object>();
        customAttributes.Add(_key, _value);
        SolarEngine.Analytics.track(eName, customAttributes);
    }
    public void CustomEvent(string eName, string _key, string _value, string _key2, string _value2)
    {
        Dictionary<string, object> customAttributes = new Dictionary<string, object>();
        customAttributes.Add(_key, _value);
        customAttributes.Add(_key2, _value2);
        SolarEngine.Analytics.track(eName, customAttributes);
    }
    public void CustomEvent(string eName, string _key, string _value, string _key2, string _value2, string _key3, string _value3)
    {
        Dictionary<string, object> customAttributes = new Dictionary<string, object>();
        customAttributes.Add(_key, _value);
        customAttributes.Add(_key2, _value2);
        customAttributes.Add(_key3, _value3);
        SolarEngine.Analytics.track(eName, customAttributes);
    }
}

using DG.Tweening;
using System;
using UnityEngine;


public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    public static bool ShowAdIcon = true;
    //-----------------------------------------------------------
    public ApplovinMaxRewardOperator applovinMaxRewardOperator;
    public ApplovinMaxInterstitialOperator applovinMaxInterstitialOperator;
    //private string SDK_key = "PbbJng_h8aD16wZWrSaHN5gtVDExorX-b1ywfx8Gal1WlU7kvbWVDpzsPARTTLwex_cbeU8SGZanUXSoA1WDMx";//测试
    private string SDK_key = "4xI8Wexro+t5Cg53ZFj2t6ML1GMEiPi99oBHbP4pfnxdjOou+X/9faU05XjKHFRTL+/wq9t4rKX99il4AwD0REdQSNVF9rAgymZUi6GipA353uCPvO1ejwgWXKDF+rSOoSOnA07lTT8=";

    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        Debug.Log("Max SDK初始化");

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdk.SdkConfiguration sdkConfiguration) =>
        {
            applovinMaxRewardOperator.Init();
            applovinMaxInterstitialOperator.Init();
        };

        string decryptedSdkKey = EncryptSDKKey.DecryptWithRandomSalt(SDK_key);
        Debug.Log("解密结果（还原原值）：" + decryptedSdkKey);
        MaxSdk.SetSdkKey(decryptedSdkKey);
        MaxSdk.SetUserId(GameApiConfig.ClientUUID);
        MaxSdk.InitializeSdk();
    }

    /// <summary>
    /// 激励广告(有)
    /// </summary>
    public void ShowRewardedAd(string _page_id, Action _rewardCallback, Action _displayErrorCallback)
    {
        applovinMaxRewardOperator.RewardReceivedCallback = _rewardCallback;
        applovinMaxRewardOperator.RewardDisplayErrorCallback = _displayErrorCallback;
        applovinMaxRewardOperator.ShowRewardedAd();
    }

    /// <summary>
    /// 激励广告(无)
    /// </summary>
    public void ShowRewardedAd2(string _page_id, Action _rewardCallback, Action _displayErrorCallback)
    {
        //applovinMaxRewardOperator2.page_id = _page_id;
        //applovinMaxRewardOperator2.new_ad_loading_position = "reward2_loading_" + _page_id;
        //applovinMaxRewardOperator2.ad_display_position = "reward2_display_" + _page_id;
        //applovinMaxRewardOperator2.RewardReceivedCallback = _rewardCallback;
        //applovinMaxRewardOperator2.RewardDisplayErrorCallback = _displayErrorCallback;
        //applovinMaxRewardOperator2.ShowRewardedAd();
        DOTween.Sequence().AppendInterval(0.5F).AppendCallback(() =>
        {
            _rewardCallback?.Invoke();
        });
    }


    /// <summary>
    /// 插屏广告
    /// </summary>
    public void OnClickInterstitialAd(string _page_id, bool isClick = true)
    {
        applovinMaxInterstitialOperator.OnClickInterstitialAd(isClick);
    }


}
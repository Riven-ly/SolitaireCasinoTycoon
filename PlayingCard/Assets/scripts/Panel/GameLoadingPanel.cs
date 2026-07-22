using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoadingPanel : UIBase
{
    public Slider slider;
    public Text loadingText;
    public static bool isCheckRegister = true;//登录检测：东8区/中国
    public static bool isOpenStatic = false;

    // Start is called before the first frame update
    private void OnEnable()
    {
        isOpen = true;
        isOpenStatic = true;
    }
    private void OnDisable()
    {
        isOpen = false;
        isOpenStatic = false;
        this.DOKill();
    }
    // Start is called before the first frame update
    void Start()
    {

        slider.value = 0f;
        loadingText.text = $"{LanguageManager.Instance.GetText("Loading")}...";

        string str = PlayerPrefs.GetString("LoginAgreementPanel");
        if(string.IsNullOrEmpty(str))
        {
            //UIManager.Instance.OpenUI<LoginAgreementPanel>(null, () =>
            //{
            //    StartCoroutine(NetworkIE());
            //    LoadingUI();
            //});

            StartCoroutine(CheckNetworkIE());
            LoadingUI();
        }
        else
        {
            StartCoroutine(CheckNetworkIE());
            LoadingUI();
        }
    }

    private void LoadingUI()
    {
        string str = LanguageManager.Instance.GetText("Loading");
        DOTween.Sequence()
             .AppendCallback(() =>
             {
                 loadingText.text = $"{str}.";
             })
            .AppendInterval(0.3f)
            .AppendCallback(() =>
            {
                loadingText.text = $"{str}..";
            })
            .AppendInterval(0.3f)
            .AppendCallback(() =>
            {
                loadingText.text = $"{str}...";
            })
            .AppendInterval(0.3f)
            .SetLoops(-1, LoopType.Restart)
            .SetTarget(this)
            ;

        slider.value = 0f;
  
    }

    IEnumerator CheckNetworkIE()
    {
        //1.网络检测
        NetworkChecker.Instance.StartCheckNetworkStatus();
        while (!NetworkChecker.Instance.isNetworkAvailable)
        {
            Debug.Log("等待网络连接");
            yield return null;
        }

        //2.各个SDK初始化
        GameManager.appATTtype = 1;//todp
        GameManager.Instance.UpdateAppATT();//todp
        //AdManager.Instance.Init();
        //OtherSdkManager.Instance.Init();

        //3.loading进度条动起来
        slider.DOValue(0.9f, 1.8f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1.8f);

        //4.游戏资源准备
        while (!GameManager.LoadABAsyncOK)
        {
            //Debug.Log("等待异步加载");
            yield return null;
        }

        //5.登录检测：东8区/中国不通过 卡死90%（检测调用BI接口）
        if (isCheckRegister)
        {
            slider.DOValue(1f, 0.2f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.3f);
            GameManager.Instance.Init();
            Hide();
        }
    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);

    }
    public override void Hide()
    {
        base.Hide();
    }
}

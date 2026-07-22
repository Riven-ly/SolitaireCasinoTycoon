using DG.Tweening;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TxInfoData
{
    public int todayPeople;
    public float mCnt;
    public float avgCnt;

    public string todayStr;
}

public class TxElementPanel : UIBase
{
    public Transform root;
    public Button hideBtn;
    public Button selectTypeEnterBtn;
    public Image typeIcon;
    public Text accountText;

    public Transform bobaoTrans;
    public Text bobaoText;
    //----
    public Text goldCnt;
    public Text onlyLeftText;
    public Slider slider;
    public Text progressText;
    public Text ex;
    public Button btn;
    public Text btnStr;
    public CanvasGroup canvasGroup;
    public Text str1;
    public Text cnt1;
    public Text str2;
    public Text cnt2;
    public Text str3;
    public Text cnt3;

    private TxInfoData txInfoData;

    private string bobaoStr ="";
    private string Wh;
    private string wh;
    private string unit;
    private string ppStr;

    private bool isOk;
    private void Awake()
    {
        RectTransform rect = root.GetComponent<RectTransform>();
        float topBlockHeight = Screen.height - Screen.safeArea.yMax;
        rect.offsetMax = new Vector2(0, -topBlockHeight);
    }
    private void OnEnable()
    {
        isOpen = true;
        GameScenePanel.isPause = true;
    }
    private void OnDisable()
    {
        isOpen = false;
        GameScenePanel.isPause = false;
    }

    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();

            if (isOk)
            {
                UIManager.Instance.OpenUI<TxElementFinalStepPanel>();
            }
            else
            {
                callback = () =>
                {
                    UIManager.Instance.GetUI<LobbyScenePanel>().lobbyLevelPanel.EnterGame();
                };
                Hide();
            }
        });
        hideBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            AdManager.Instance.OnClickInterstitialAd("TxElementPanel");
            Hide();
        });
  
        selectTypeEnterBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayBtnMusic();
            UIManager.Instance.OpenUI<TxElementTypeSelectPanel>();
        });

    }

    public override void Refresh(object data = null)
    {
        base.Refresh(data);
   
        if(string.IsNullOrEmpty(bobaoStr))
        {
            ppStr = LanguageManager.Instance.GetText_Encrypt("pp");
            bobaoStr = LanguageManager.Instance.GetText("TxPanel_BoBao");
            Wh = LanguageManager.Instance.GetText_Encrypt("Wh");
            wh = LanguageManager.Instance.GetText_Encrypt("wh");
            unit = LanguageManager.Instance.GetText_Encrypt("Special_Diamond__unit");
            btnStr.text = LanguageManager.Instance.GetText_Encrypt("WD");
        }

        UpdateAccountTypeIcon();
        PlayBoBao();

        goldCnt.text = $"{unit}{GameManager.Instance.playerInfo.gold}";
        int temV = TxElementMananger.Instance.GetJIndu();
        int diifV = 10000 - temV;

        onlyLeftText.text = string.Format(LanguageManager.Instance.GetText("TxPanel_onlyleft"), $"{diifV / 100f}%");
        slider.value = temV / 10000f;
        progressText.text = $"{temV / 100f}%";
        ex.text = string.Format(LanguageManager.Instance.GetText("TxPanel_ex"), wh);
        isOk = temV >= 10000; 
        if (isOk)
        {
            btnStr.text = LanguageManager.Instance.GetText_Encrypt("WD");
            btn.interactable = true;
            canvasGroup.alpha = 1f;
        }
        else
        {
            btnStr.text = LanguageManager.Instance.GetText("PLAY");
            btn.interactable = GameManager.Instance.gameType == GameType.LobbyScene;
            canvasGroup.alpha = GameManager.Instance.gameType == GameType.LobbyScene ? 1f : 0.5f;
        }
  

        txInfoData = GetTxData();
        str1.text = LanguageManager.Instance.GetText("TxPanel_1Str");
        str2.text = string.Format(LanguageManager.Instance.GetText("TxPanel_2Str"), Wh);
        str3.text = LanguageManager.Instance.GetText("TxPanel_3Str");

        cnt1.text = txInfoData.todayPeople.ToString();
        cnt2.text = unit + txInfoData.mCnt.ToString();
        cnt3.text = txInfoData.avgCnt.ToString();

        string firstGame = PlayerPrefs.GetString("Guide_TxPanel", "");
        if (string.IsNullOrEmpty(firstGame))
        {
            DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() =>
            {
                UIManager.Instance.OpenUI<GuidePanel_TxPanel>();
            });
        }
    }

    public override void Hide()
    {
        bobaoTrans.transform.DOKill();
        base.Hide();
    }

    // ========== 生成每日随机数据（你来填充具体逻辑） ==========
    public TxInfoData GenerateDailyData()
    {
        var data = new TxInfoData();
        data.todayPeople = Random.Range(10, 100);
        int rS = Random.Range(5000, 10000);
        data.mCnt = rS / 100f;
        data.avgCnt = Random.Range(1, 20);

        // 记录日期
        data.todayStr = GameManager.Instance.GetNowTime().ToString("yyyy-MM-dd");

        return data;
    }

    public void SaveTxData(TxInfoData data)
    {
        string jsonStr = JsonConvert.SerializeObject(data, Formatting.Indented);
        Debug.Log("TxInfoData 保存:\n" + jsonStr);
        PlayerPrefs.SetString("TxInfoData", jsonStr);
    }

    public TxInfoData GetTxData()
    {
        string jsonStr = PlayerPrefs.GetString("TxInfoData", "");
        string today = GameManager.Instance.GetNowTime().ToString("yyyy-MM-dd");

        // 如果没有数据，直接生成
        if (string.IsNullOrEmpty(jsonStr))
        {
            Debug.Log("未找到 TxInfoData，生成默认数据");
            TxInfoData temp = GenerateDailyData();
            SaveTxData(temp);
            return temp;
        }

        // 有数据，检查日期
        TxInfoData existingData = JsonConvert.DeserializeObject<TxInfoData>(jsonStr);

        // 如果日期不是今天，重新生成
        if (existingData.todayStr != today)
        {
            Debug.Log($"日期已变更 ({existingData.todayStr} -> {today})，更新数据");
            TxInfoData newData = GenerateDailyData();
            SaveTxData(newData);
            return newData;
        }

        // 同一天，直接返回
        Debug.Log($"使用已有数据 (日期: {existingData.todayStr})");
        return existingData;
    }

    private void PlayBoBao()
    {
        bobaoTrans.transform.DOKill();
        string curname = GenerateText();
        int ranV = Random.Range(1000, 10000);
        float targetF = ranV / 100f;

        bobaoText.text =string.Format(bobaoStr, curname, unit + targetF, wh);
        Vector3 curPos = bobaoTrans.transform.localPosition;
        curPos.x = 475f;
        bobaoTrans.transform.localPosition = curPos;
        DOTween.Sequence()
               //.Append(bobaoTrans.transform.DOLocalMoveX(0f, 3f).SetEase(Ease.Linear))
               //.AppendInterval(5f)
               .Append(bobaoTrans.transform.DOLocalMoveX(-2000f, 10f).SetEase(Ease.Linear))
               .AppendInterval(3f)
               .AppendCallback(() =>
               {
                   PlayBoBao();
               })
               .SetTarget(bobaoTrans.transform)
               ;
    }

    public void UpdateAccountTypeIcon()
    {
        typeIcon.sprite = TxElementMananger.Instance.accountTypeSprites[(int)TxElementMananger.Instance.info.accountInfo.type];
        typeIcon.SetNativeSize();

        if(string.IsNullOrEmpty(TxElementMananger.Instance.info.accountInfo.email))
        {
            accountText.text = LanguageManager.Instance.GetText("selectType_explain");
        }
        else
        {
            accountText.text = TxElementMananger.Instance.info.accountInfo.email;
        }
    }

    /// <summary>
    /// 生成固定格式的假邮箱：Wx*****qs
    /// 首字母大写，其余字母小写
    /// </summary>
    public string GenerateText()
    {
        // 1. 生成前缀（2个字母，首字母大写，第二个小写）
        string prefix = GetCapitalizedLetters(2);

        // 2. 固定5个星号
        string stars = "*****";

        // 3. 生成后缀（2个字母，全部小写）
        string suffix = GetLowerLetters(2);

        // 4. 拼接
        return prefix + stars + suffix + $"@{ppStr}.com";
    }

    /// <summary>
    /// 生成指定数量的字母：首字母大写，其余小写
    /// </summary>
    private string GetCapitalizedLetters(int count)
    {
        char[] letters = new char[count];
        for (int i = 0; i < count; i++)
        {
            if (i == 0)
            {
                // 首字母：大写 A-Z
                letters[i] = (char)Random.Range(65, 91);
            }
            else
            {
                // 其余字母：小写 a-z
                letters[i] = (char)Random.Range(97, 123);
            }
        }
        return new string(letters);
    }

    /// <summary>
    /// 生成指定数量的随机小写字母
    /// </summary>
    private string GetLowerLetters(int count)
    {
        char[] letters = new char[count];
        for (int i = 0; i < count; i++)
        {
            letters[i] = (char)Random.Range(97, 123);
        }
        return new string(letters);
    }

}

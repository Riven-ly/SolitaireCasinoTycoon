using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidePanel_TxPanel : UIBase
{
    public Button maskBtn;
    public Button tagetBtn;
    public Text str;

    public Transform mask;
    public Transform trans;

    void Start()
    {
        maskBtn.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("Guide_TxPanel", "yes");

            string firstGame = PlayerPrefs.GetString("Guide_TxPanel2", "");
            if (string.IsNullOrEmpty(firstGame))
            {
                UIManager.Instance.OpenUI<GuidePanel_TxPanel2>();
            }
            Hide();
        });
        tagetBtn.onClick.AddListener(() =>
        {
           
        });
    }


    public override void Refresh(object data = null)
    {
        base.Refresh(data);

        TxElementPanel txPanel = UIManager.Instance.GetUI<TxElementPanel>();
        mask.position = txPanel.slider.transform.position;
        //trans.position = txPanel.slider.transform.position;

        //str.text = string.Format(LanguageManager.Instance.GetText("Guide_TxPanel"), LanguageManager.Instance.GetText_Encrypt("wh"));

    }
    public override void Hide()
    {
        base.Hide();
    }
}

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameBox : MonoBehaviour,IEventListener
{
    public static GameBox Instance;

    public Transform effectPoint;
    public Transform xiangzi;
    public Transform xiangzi_kai;
    public Transform xiangzi_bi;

    public Animation anim;
    public Text progressStr;
    public Slider slider;
    private int progress;
    private int rewardIndex;
    private List<int> targetCnt;
    private List<List<ItemData>> rewards;
    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        EventManager.Instance.RegisterListener(GameEvent.AddGameBox, this);
    }
    private void OnDisable()
    {
        EventManager.Instance.UnregisterListener(GameEvent.AddGameBox, this);
    }

    public void Init()
    {
        if(rewards == null)
        {
            targetCnt = new List<int>() { 15, 30, 50 };
            rewards = new List<List<ItemData>>();
            List<ItemData> itemDatas = new List<ItemData>();
            itemDatas.Add(new ItemData(ItemType.GoldDui, 0.5f));
            rewards.Add(itemDatas);

            List<ItemData> itemDatas2 = new List<ItemData>();
            itemDatas2.Add(new ItemData(ItemType.GoldDui, 1.5f));
            rewards.Add(itemDatas2);

            List<ItemData> itemDatas3 = new List<ItemData>();
            itemDatas3.Add(new ItemData(ItemType.GoldDui, 2f));
            rewards.Add(itemDatas3);
        }

        progress = 0;
        rewardIndex = 0;
        UpdateProgressUI();
        PlayIdleAnim();

        xiangzi_bi.gameObject.SetActive(false);
        xiangzi_kai.gameObject.SetActive(false);
    }

    public void PlayIdleAnim()
    {
        anim.Play("GameBoxAnim");
    }
    public void PlayGetProgressAnim()
    {
        anim.Stop();
        transform.DOKill();
        anim.Play("GameBoxAnim2");
        DOTween.Sequence().AppendInterval(2f).AppendCallback(() =>
        {
            PlayIdleAnim();
        }).SetTarget(transform);
    }

    public void UpdateProgressUI()
    {
        if (rewardIndex >= targetCnt.Count)
        {
            return;
        }

        int curMaxCnt = targetCnt[rewardIndex];

        slider.maxValue = curMaxCnt;
        slider.value = progress;
        progressStr.text = $"{progress}/{curMaxCnt}";
        Debug.Log("箱子进度：" + progress);
    }

    /// <summary>
    /// 检查箱子进度
    /// </summary>
    /// <param name="_callback"></param>
    /// <returns></returns>
    public bool CheckBoxProgress(Action _callback = null)
    {
        // 全部奖励已领完，直接回调退出
        if (rewardIndex >= targetCnt.Count)
        {
            _callback?.Invoke();
            return false;
        }

        bool hasReward = false;
        List<ItemData> allGetRewards = new List<ItemData>();
        // 循环领取所有当前进度已达标的档位
        while (rewardIndex < targetCnt.Count && progress >= targetCnt[rewardIndex])
        {
            hasReward = true;
            Debug.Log("领取档位：" + rewardIndex);
            allGetRewards.AddRange(rewards[rewardIndex]);
            rewardIndex++;
        }

        // 没有可领取奖励，直接回调返回
        if (!hasReward)
        {
            _callback?.Invoke();
            return false;
        }
        GameScenePanel.isPause = true;
        UIManager.Instance.OpenUIMask();

        xiangzi_bi.transform.position = xiangzi.transform.position;
        xiangzi_bi.transform.localScale = Vector3.one * 0.5f;
        xiangzi_bi.gameObject.SetActive(true);

        xiangzi_bi.transform.DOLocalMoveX(-330f, 0.5f);
        xiangzi_bi.transform.DOLocalMoveY(-280f, 0.25f);
        xiangzi_bi.transform.DOScale(1f, 0.5f);

        DOTween.Sequence()
            .AppendInterval(0.5F)
            .AppendCallback(() =>
            {
                xiangzi_bi.transform.DOScale(2f, 0.5f);
                xiangzi_bi.transform.DOLocalMoveX(0f, 0.5f);
                xiangzi_bi.transform.DOLocalMoveY(0f, 0.25f);
            })
            .AppendInterval(0.5F)
            .AppendCallback(() =>
            {
                xiangzi_bi.gameObject.SetActive(false);
                xiangzi_kai.gameObject.SetActive(true);
            })
            .AppendInterval(0.1F)
            .AppendCallback(() =>
            {
         
                UIManager.Instance.HideUIMask();
                UIManager.Instance.OpenUI<GeneralRewardsPanel>(allGetRewards, () =>
                {
                    xiangzi_kai.gameObject.SetActive(false);
                    GameScenePanel.isPause = false;
                    UpdateProgressUI();
                    _callback?.Invoke();
                });
            })
            ;
        return true;
    }



    public void OnEventTriggered(GameEvent eventType, object data = null)
    {
        if(eventType == GameEvent.AddGameBox)
        {
            int cnt = (int)data;
            progress += cnt;
            UpdateProgressUI();
        }
    }
}

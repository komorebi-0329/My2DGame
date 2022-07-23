using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdsButton : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener, IUnityAdsLoadListener
{
#if UNITY_IOS
   private string gameID="4829341";
#elif UNITY_ANDROID
    private string gameID = "4829340";
#endif

    Button adsButton;

    private void Awake()
    {
        //这里第2个参数true代表测试广告，false代表真实广告
        //最后一个参数this表示添加监听，替代旧版本 Advertisement.AddListener(this);
        Advertisement.Initialize(gameID, false, this);
    }

    void Start()
    {
        adsButton = GetComponent<Button>();

        if (adsButton)
        {
            adsButton.onClick.AddListener(ShowRewardAds);
        }
      
        //Reward广告无法跳过
        Advertisement.Load("Rewarded_Android", this);
        //Interstitial广告可跳过
        Advertisement.Load("Interstitial_Android", this);

        //Advertisement.Initialize(gameID, true);
    }

    public void ShowRewardAds()
    {
        Advertisement.Show("Rewarded_Android");
    }


    public void OnInitializationComplete()
    {
        
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
       
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        
    }

    public void OnUnityAdsShowClick(string placementId)
    {
       
    }

    public void OnUnityAdsShowComplete(string Rewarded_Android, UnityAdsShowCompletionState showCompletionState)
    {  
            Debug.Log("AD播放完了，回复血量");
            FindObjectOfType<PlayerController>().HP = 3;
            FindObjectOfType<PlayerController>().isDead = false;
            UIManager.instance.UpdateHealth(FindObjectOfType<PlayerController>().HP);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        
    }
}

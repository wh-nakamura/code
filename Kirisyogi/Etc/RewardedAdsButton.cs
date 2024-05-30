using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] Button Rewarded_button;
    [SerializeField] Button Interstitial_button;
    //[SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _Rewarded_Android = "Rewarded_Android";
    [SerializeField] string _Interstitial_Android = "Interstitial_Android";
    [SerializeField] string _Banner_Android = "ca-app-pub-3940256099942544/6300978111";//"Banner_Android";
    //[SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = "5136684"; // This will remain null for unsupported platforms
    int num = 0;

    void Awake()
    {   
        Advertisement.Initialize(_adUnitId);
        // ボタンに関数付与
        Rewarded_button.onClick.AddListener(Show_ad_rewarded);
        Interstitial_button.onClick.AddListener(Show_ad_interstitial);
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER); //バナーを上部中央にセット
        Advertisement.Banner.Show(_Banner_Android);
        // Get the Ad Unit Id for the current platform:
        //#if UNITY_IOS
        //        _adUnitId = _iOSAdUnitId;
        //#elif UNITY_ANDROId
        //        _adUnitId = _androidAdUnitId;
        //#endif
        //_adUnitId = _androidAdUnitId;

        //Disable the button until the ad is ready to show:
        //_showAdButton.interactable = false;
        //
        ////Debug.Log("Ad Loaded: " + adUnitId);
        //print("読み込まれた");
//
        //if (_adUnitId.Equals(_adUnitId))
        //{
        //    // Configure the button to call the ShowAd() method when clicked:
        //    _showAdButton.onClick.AddListener(ShowAd);
        //    // Enable the button for users to click:
        //    _showAdButton.interactable = true;
        //}
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        //Debug.Log("Ad Loaded: " + adUnitId);
        print("読み込まれた2");

        if (adUnitId.Equals(_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            //_showAdButton.onClick.AddListener(ShowAd);
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void Show_ad_rewarded()
    {
        // 広告ボタンを押せなくする
        Rewarded_button.interactable = false;
        // 広告再生
        Advertisement.Show(_Rewarded_Android, this);
        print("ボタンを押した");
    }

    public void Show_ad_interstitial()
    {
        // 広告ボタンを押せなくする
        Interstitial_button.interactable = false;
        // 広告再生
        Advertisement.Show(_Interstitial_Android, this);
        print("ボタンを押した");
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            //Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            print("広告視聴完了");
            num += 1;

            // Load another ad:
            Advertisement.Load(_adUnitId, this);
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { 
        print("視聴開始");
    }
    public void OnUnityAdsShowClick(string adUnitId) {
        print("どこ？");
    }

    void OnDestroy()
    {
        // Clean up the button listeners:
        Rewarded_button.onClick.RemoveAllListeners();
        Interstitial_button.onClick.RemoveAllListeners();
        print("ゲーム終了時");
        print(num);
    }
}
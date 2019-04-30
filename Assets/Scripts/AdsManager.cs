using System;
using Game;
#if ADMOB
using GoogleMobileAds.Api;
#endif
using UnityEngine;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

// ReSharper disable once HollowTypeName
public partial class AdsManager : Singleton<AdsManager>
{
#if ADMOB
    private static string ADMOB_INTERSTITIAL_ID => Application.platform == RuntimePlatform.Android
        ? GameSettings.Default.AndroidAdmobSetting.interstitialId
        : GameSettings.Default.IosAdmobSetting.interstitialId;

    private static string ADMOB_REWARDED_ID => Application.platform == RuntimePlatform.Android
        ? GameSettings.Default.AndroidAdmobSetting.admobRewardedId 
        : GameSettings.Default.IosAdmobSetting.admobRewardedId;
#endif


    public bool Initialized { get; private set; }

    private Action<bool> _pendingCallback;

    public static int InterstitialCount { get; private set; } = 0;


    private static bool IsAdmobRewardedAvailable
    {
        get
        {
#if ADMOB
            return _rewardBaseVideo.IsLoaded();
#endif
            return false;
        }
    }

    private static bool IsAdmobInterstitialAvailable
    {
        get
        {
#if ADMOB
            return _interstitialAd.IsLoaded();
#endif
            return false;
        }
    }

    public static bool IsAdColonyAdsAvailable
    {
        get
        {
#if ADCLONY
            return adColonyInterstitialAd != null;
#endif

            return false;
        }
    }

#if ADMOB
    private static RewardBasedVideoAd _rewardBaseVideo;
    private static InterstitialAd _interstitialAd;

#endif

    void Start()
    {
        Init();
    }


    private void Init()
    {
        if (Initialized)
            return;


#if ADMOB
        _rewardBaseVideo = RewardBasedVideoAd.Instance;
        _rewardBaseVideo.OnAdRewarded += RewardBaseVideoOnOnAdRewarded;
        _rewardBaseVideo.OnAdFailedToLoad += (sender, args) =>
        {
//            PlatformUtils.ShowToast($"Video Ads Loaded Failed:{args.Message}");
            Invoke(nameof(RequestAdmobRewardVideo), 6f);
        };
//        _rewardBaseVideo.OnAdLoaded += (sender, args) => { PlatformUtils.ShowToast($"Video Ads Loaded"); };
        RequestAdmobRewardVideo();
        RequestAdmobInterstitial();
#endif

#if ADCLONY
       ConfigureAdColonyAds();
#endif

        Initialized = true;
    }



#if ADMOB

// ReSharper disable once TooManyDeclarations
    private void RequestAdmobRewardVideo()
    {
        var request = new AdRequest.Builder().Build();
        _rewardBaseVideo.LoadAd(request, ADMOB_REWARDED_ID);
    }


    private void RequestAdmobInterstitial()
    {
        _interstitialAd = new InterstitialAd(ADMOB_INTERSTITIAL_ID);
        _interstitialAd.OnAdClosed += InterstitialAdOnOnAdClosed;
        _interstitialAd.OnAdFailedToLoad += (sender, args) =>
        {
            Instance.Invoke(nameof(RequestAdmobInterstitial), 10);
        };

        var request = new AdRequest.Builder().Build();
        _interstitialAd.LoadAd(request);
    }

    private static void InterstitialAdOnOnAdClosed(object sender, EventArgs eventArgs)
    {
        Instance.RequestAdmobInterstitial();
    }


    private void RewardBaseVideoOnOnAdRewarded(object sender, Reward reward)
    {
        _pendingCallback?.Invoke(true);
        _pendingCallback = null;
    }
#endif


#if UNITY_ADS
    // ReSharper disable once FlagArgument
    private static void ShowUnityVideoAds(Action<bool> completed = null)
    {
        if (!Advertisement.IsReady())
        {
            completed?.Invoke(false);
            return;
        }
        var options = new ShowOptions
        {
            resultCallback = (result) =>
            {
                switch (result)
                {
                    case ShowResult.Failed:
                        completed?.Invoke(false);
                        break;

                    case ShowResult.Finished:
                        completed?.Invoke(true);
                        break;

                    case ShowResult.Skipped:
                        completed?.Invoke(true);
                        break;
                }
            }
        };

        Advertisement.Show(options);
    }

#endif

    public void OnEnable()
    {
        OnEnableAdsPartial();
    }


    public void OnDisable()
    {
        OnDisableAdsPartial();
    }


    public enum VideoType
    {
        Skip,
        Full
    }
}


// ReSharper disable once HollowTypeName
public partial class AdsManager
{
    public static void ShowInterstitial()
    {
//        PlatformUtils.ShowToast(nameof(ShowInterstitial)+IsAdmobInterstitialAvailable);
        if (IsAdmobInterstitialAvailable)
        {
#if ADMOB
            _interstitialAd.Show();
#endif
        }
    }


//    // ReSharper disable once FlagArgument
//    // ReSharper disable once MethodTooLong
    public static void ShowVideoAds(Action<bool> completed = null)
    {
        Action<bool> onCompleted = (success) =>
        {
            //if (success)
            //{
            //    var val = _videoCountDict.GetOrDefault(type);
            //    _videoCountDict.AddOrUpdate(type, ++val);
            //}
            completed?.Invoke(success);
        };


        if (IsAdmobRewardedAvailable && Application.platform != RuntimePlatform.WindowsEditor)
        {
            Instance._pendingCallback = onCompleted;
#if ADMOB
            _rewardBaseVideo.Show();
#endif
        }
        else if(IsAdColonyAdsAvailable)
        {
            Instance._pendingCallback = onCompleted;
#if ADCLONY
            Ads.ShowAd(adColonyInterstitialAd);
#endif
        }
        else if (IsUnityAdsAvailable)
        {
#if UNITY_ADS
            ShowUnityVideoAds(onCompleted);
#endif
        }
        else
        {
            onCompleted(false);
        }
    }


    public static bool IsVideoAvailable()
    {
        return IsAdmobRewardedAvailable || IsUnityAdsAvailable || IsAdColonyAdsAvailable;
    }

    public static bool IsUnityAdsAvailable
    {
        get
        {
#if UNITY_ADS
            return Advertisement.IsReady();
#endif
            // ReSharper disable once HeuristicUnreachableCode
#pragma warning disable 162
            return false;
#pragma warning restore 162
        }
    }



    public static bool IsInterstitialAvailable()
    {
        var available = //Application.internetReachability != NetworkReachability.NotReachable ||
            IsAdmobInterstitialAvailable; //|| IsUnityAdsAvailable(VideoType.Skip);

        return available;
    }
}

#if ADCLONY
public partial class AdsManager
{
    private static AdColony.InterstitialAd adColonyInterstitialAd;

    

    void RequestAdColonyAd()
    {
        var adOptions = new AdOptions();
        Ads.RequestInterstitialAd(Constants.CurrencyZoneID, adOptions);
    }

    void ConfigureAdColonyAds()
    {

        Ads.OnConfigurationCompleted += (list) =>
        {
            RequestAdColonyAd();
        };
        Ads.OnClosed += ad =>
        {
            _pendingCallback?.Invoke(true);
            RequestAdColonyAd();
        };
        Ads.OnRequestInterstitial += ad => { adColonyInterstitialAd = ad; };

        var appOptions = new AppOptions();
        string[] zoneIDs = { Constants.InterstitialZoneID, Constants.CurrencyZoneID };
        Ads.Configure(Constants.AppID, appOptions, zoneIDs);
    }
}
#endif


public partial class AdsManager
{
    private static int AdsPassLeftCount
    {
        get {

            if (!PlayerPrefs.HasKey(nameof(AdsPassLeftCount)))
            {
                SetForNextAds();
            }
            return PlayerPrefs.GetInt(nameof(AdsPassLeftCount));
        }
        set { PlayerPrefs.SetInt(nameof(AdsPassLeftCount), value); }
    }

    public static bool NextVideoAds { get; private set; }

    public static void ShowOrPassAdsIfCan()
    {
        if (!ResourceManager.EnableAds)
            return;

        if (AdsPassLeftCount <= 0)
        {
            ShowAdsIfPassedIfCan();
        }
        else
        {
            PassAdsIfCan();
        }
    }

    public static void ShowAdsIfPassedIfCan()
    {
        if (!ResourceManager.EnableAds)
            return;
        if (AdsPassLeftCount <= 0)
        {
            if (!NextVideoAds && IsInterstitialAvailable())
            {
                ShowInterstitial();
                SetForNextAds();
            }
            else
            {
                ShowVideoAds();
                SetForNextAds();
            }
        }
    }

    private static void SetForNextAds()
    {
        if (UnityEngine.Random.value < GameSettings.Default.AdsSettings.videoAdsFrequency)
        {
            NextVideoAds = true;
            AdsPassLeftCount = UnityEngine.Random.Range(GameSettings.Default.AdsSettings.minAndMaxGameOversBetweenVideoAds.x,
                GameSettings.Default.AdsSettings.minAndMaxGameOversBetweenVideoAds.y);
        }
        else
        {
            NextVideoAds = false;
            AdsPassLeftCount = UnityEngine.Random.Range(GameSettings.Default.AdsSettings.minAndMaxGameOversBetweenInterstitialAds.x, GameSettings.Default.AdsSettings.minAndMaxGameOversBetweenInterstitialAds.y);
        }
    }

    public static void PassAdsIfCan()
    {
        if (!ResourceManager.EnableAds)
            return;

        AdsPassLeftCount = Mathf.Max(AdsPassLeftCount - 1, 0);
    }
}

// Implement Ads Show Logic
public partial class AdsManager
{
    private void OnEnableAdsPartial()
    {
        LevelManager.GameOver += LevelManagerOnGameOver;
    }

    private void OnDisableAdsPartial()
    {
        LevelManager.GameOver -= LevelManagerOnGameOver;
    }

    // ReSharper disable once FlagArgument
    private void LevelManagerOnGameOver(GameOverData gameOverData)
    {
        if (!ResourceManager.EnableAds)
            return;

        ShowOrPassAdsIfCan();
    }
}
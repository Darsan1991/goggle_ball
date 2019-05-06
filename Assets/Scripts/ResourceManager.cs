using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class ResourceManager : Singleton<ResourceManager>
{
#if IN_APP
    public static event Action<string> ProductPurchased;
    public static event Action<bool> ProductRestored;
    public static event Action<bool> PremiumStateChanged;
#endif

    [SerializeField] private List<SlowBallProduct> _slowBallProducts = new List<SlowBallProduct>();


    public static IEnumerable<SlowBallProduct> SlowBallProducts => Instance._slowBallProducts;

    public static bool EnableAds
    {
        get { return PrefManager.GetBool(nameof(EnableAds), true); }
        set
        {
            PrefManager.SetBool(nameof(EnableAds), value); 
            PremiumStateChanged?.Invoke(!value);
        }
    }

    public static int Coins
    {
        get { return PrefManager.GetInt(nameof(Coins)); }
        set { PrefManager.SetInt(nameof(Coins), value); }
    }

    public static int SlowBalls
    {
        get { return PrefManager.GetInt(nameof(SlowBalls), 5); }
        set { PrefManager.SetInt(nameof(SlowBalls), value); }
    }
#if IN_APP

    public static bool AbleToRestore => EnableAds;

    public Purchaser Purchaser { get; private set; }

    public static string NO_ADS_PRODUCT_ID => GameSettings.Default.InAppSetting.removeAdsId;

    protected override void OnInit()
    {
        base.OnInit();
#if UNITY_IOS
        Purchaser = new Purchaser(
            _slowBallProducts.Select(product => product.ProductId),
            new List<string>{NO_ADS_PRODUCT_ID});
#else
        Purchaser = new Purchaser(
            _slowBallProducts.Select(product => product.ProductId).Append(NO_ADS_PRODUCT_ID),new List<string>());
#endif
        Purchaser.RestorePurchased += PurchaserOnRestorePurchased;
    }

    private void PurchaserOnRestorePurchased(bool success)
    {
        if (EnableAds &&  Purchaser.ItemAlreadyPurchased(NO_ADS_PRODUCT_ID))
        {
            EnableAds = false;
            ProductPurchased?.Invoke(NO_ADS_PRODUCT_ID);
        }

        ProductRestored?.Invoke(success);
    }


    public static string GetPriceForProduct(string productId)
    {
        return Instance.Purchaser.GetPrice(productId);
    }

    public static void RestorePurchase()
    {
        Instance.Purchaser.Restore();
    }

    public static void PurchaseSlowBalls(string id, Action<bool> completed = null)
    {
        var ballProduct = SlowBallProducts.First(product => product.Id == id);

        Instance.Purchaser.BuyProduct(ballProduct.ProductId, success =>
        {
            if (success)
                SlowBalls += ballProduct.Value;

            if (success && ballProduct.IncludeAdsFree)
                EnableAds = false;
            if (success)
                ProductPurchased?.Invoke(ballProduct.ProductId);
            completed?.Invoke(success);
        });
    }

    public static void PurchaseNoAds(Action<bool> completed = null)
    {
        if (!EnableAds)
        {
            return;
        }

        Instance.Purchaser.BuyProduct(NO_ADS_PRODUCT_ID, success =>
        {
            if (success)
            {
                EnableAds = false;
            }

            if (success)
                ProductPurchased?.Invoke(NO_ADS_PRODUCT_ID);
            completed?.Invoke(success);
        });
    }
#endif
    }

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreTileUI : MonoBehaviour
{
    public event Action<StoreTileUI> Clicked;

    [SerializeField] private Text _nameText;
    [SerializeField] private Text _priceText;
    [SerializeField] private SlowBallProduct _slowBall;
    [SerializeField] private bool _isAdsFree;

    public SlowBallProduct SlowBall => _slowBall;
    public bool IsAdsFree => _isAdsFree;


    private void Awake()
    {
        if (_slowBall != null)
        {
            _nameText.text = SlowBall.Name;
            _priceText.text = ResourceManager.GetPriceForProduct(SlowBall.ProductId) ??
                              $"{SlowBall.Price:N}$";
        }
        else
        {
            _priceText.text = ResourceManager.GetPriceForProduct(ResourceManager.NO_ADS_PRODUCT_ID) ??
                              $"{0.99:N}$";
        }

        if(IsAdsFree && !ResourceManager.EnableAds)
            gameObject.SetActive(false);
    }

    public void OnClick()
    {
        Clicked?.Invoke(this);
    }

    private void Update()
    {
        if (IsAdsFree && !ResourceManager.EnableAds)
            gameObject.SetActive(false);
    }
}
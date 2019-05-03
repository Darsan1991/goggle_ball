using UnityEngine;

[CreateAssetMenu]
public class SlowBallProduct : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private float _price;
    [SerializeField] private string _productId;
    [SerializeField] private int _value;
    [SerializeField] private bool _includeAdsFree;

    public float Price => _price;
    public string Name => _name;
    public bool IncludeAdsFree => _includeAdsFree;
    public string Id => ProductId;
    public string ProductId => _productId;
    public int Value => _value;
}
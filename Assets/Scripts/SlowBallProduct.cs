using UnityEngine;

[CreateAssetMenu]
public class SlowBallProduct : ScriptableObject
{
    [SerializeField] private string _productId;
    [SerializeField] private int _value;

    public string Id => ProductId;
    public string ProductId => _productId;
    public int Value => _value;
}
using UnityEngine;

[CreateAssetMenu]
public class SlowBallProduct : ScriptableObject
{
    [SerializeField] private string _productId;
    [SerializeField] private int _value;

    public string ProductId => _productId;
    public int Value => _value;
}
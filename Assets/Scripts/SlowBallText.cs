using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SlowBallText : MonoBehaviour
{
    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
        _text.text = ResourceManager.SlowBalls.ToString();
    }

    private void Update()
    {
        _text.text = ResourceManager.SlowBalls.ToString();
    }
}
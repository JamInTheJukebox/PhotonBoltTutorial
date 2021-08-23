using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UI_Healthbar : MonoBehaviour
{
    [SerializeField]
    private Gradient _gradient = null;

    [SerializeField]
    private Image _bg = null;

    [SerializeField]
    private Image _bar = null;

    [SerializeField]
    private TextMeshProUGUI _text = null;

    public void UpdateLife(int hp, int totalHP)
    {
        float ratio = (float)hp / (float)totalHP;
        _bar.fillAmount = ratio;

        Color c = _gradient.Evaluate(ratio);        // never did it like this before. USE THIS!
        _bg.color = new Color(c.r, c.g, c.b, _bg.color.a);
        _bar.color = c;

        _text.text = hp.ToString();
    }
}

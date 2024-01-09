using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private RectTransform _bar;

    private Image _barImage;
    // Start is called before the first frame update
    void Start()
    {
        _bar = GetComponent<RectTransform>();
        _barImage = GetComponent<Image>();
        if (Health.totalHealth < 0.3f)
        {
            _barImage.color = Color.red;
        }
        SetSize(Health.totalHealth);
    }

    public void Damage(float damage)
    {
        if ((Health.totalHealth -= damage) >= 0f)
        {
            Health.totalHealth -= damage;
        }
        else
        {
            Health.totalHealth = 0f;
        }

        if (Health.totalHealth < 0.3f)
        {
            _barImage.color = Color.red;
        }
        SetSize(Health.totalHealth);
    }

    public void SetSize(float size)
    {
        _bar.localScale = new Vector3(size, 1f);
    }
}

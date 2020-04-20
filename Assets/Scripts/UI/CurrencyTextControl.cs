using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyTextControl : MonoBehaviour
{
    [SerializeField, Tooltip("The text UI component.")]
    public Text text;
    [SerializeField, Tooltip("The prefab for Shop Manager.")]
    public ShopManager shop;
    [SerializeField, Tooltip("The prefab for the floating text.")]
    public FloatingText floatingText;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "0";
        shop.currencyChanged += UpdateCurrencyTextAdd;
        shop.currencySet += UpdateCurrencyTextSet;
    }

    // Update the currency text
    private void UpdateCurrencyTextSet(int currency)
    {
        text.text = currency.ToString();
    }

    // Update the currency text and adds a floating text based on the value added
    private void UpdateCurrencyTextAdd(int amount, int currency)
    {
        FloatingText floatText = Instantiate(floatingText, text.transform.position, Quaternion.identity);
        floatText.Color = Color.yellow;
        floatText.Text = "+" + amount.ToString();

        text.text = currency.ToString();
    }

}

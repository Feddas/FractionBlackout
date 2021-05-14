using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FractionButton : MonoBehaviour
{
    [Tooltip("TODO: opps, should be called ButtonsDenominator")]
    public int ButtonsNumerator;

    [Tooltip("Fillable image that shows the already filled amount of this button")]
    public Image FillBar;

    // void Start() { }
    // void Update() { }

    public void OnClick()
    {
        float multiplier = addCurrentCardToThisButton();
        if (multiplier < 0) // card was invalid to add to this button
        {
            FractionDeck.Instance.BadClick();
            return;
        }

        // card was successfully used
        FractionDeck.Instance.NextCard();

        // extra debug info
        int numeratorCard = FractionDeck.Instance.CurrentCard.x;
        Debug.LogFormat("{0}/{1} => {2}/{3} {4}% => {5}%"
            , numeratorCard, FractionDeck.Instance.CurrentCard.y, numeratorCard * multiplier, ButtonsNumerator
            , FillBar.fillAmount, FillBar.fillAmount + FractionDeck.Instance.CurrentFraction());
    }

    /// <returns> multipler to get the equivalent whole number numerator of the CurrentCard. Returns -1 if whole number numerator is impossible </returns>
    private float addCurrentCardToThisButton()
    {
        float multiplier;

        // determine if fraction is equivalent
        if (false == isEquivalentFraction(out multiplier))
        {
            return -1; // is not an equivelent fraction
        }

        // determine if bar has room
        float addAmount = FractionDeck.Instance.CurrentFraction();
        if ((FillBar.fillAmount + addAmount) > (1 + .01)) // using .01 because float.Epsilon was failing for 1/3 + 2/3
        {
            return -1; // fill bar doesn't have room
        }

        // made it past all guards clauses, apply the CurrentCard to this FractionButton
        FillBar.fillAmount += addAmount;
        return multiplier;
    }

    private bool isEquivalentFraction(out float multiplier)
    {
        int denominatorCard = FractionDeck.Instance.CurrentCard.y;
        int denominatorButton = ButtonsNumerator;
        if (denominatorCard == denominatorButton)
        {
            multiplier = 1;
            return true;
        }

        // determine if denominators factor into one another. i.e. 3/6 => 2/4 is invalid
        if (false == isCommonDivisors(denominatorCard, denominatorButton))
        {
            multiplier = -1;
            return false;
        }

        // determine if numerator would be a whole number. i.e. 3/4 => 1.5/2 is invalid
        int numeratorCard = FractionDeck.Instance.CurrentCard.x;
        multiplier = (float)denominatorButton / denominatorCard;
        float numeratorButton = numeratorCard * multiplier;
        if (numeratorButton % 1 >= float.Epsilon)
        {
            return false; // numerator must be a whole number
        }

        // made it past all guards clauses and easy cases
        return true;
    }

    private bool isCommonDivisors(int denCard, int denButton)
    {
        float commonDivisor;
        if (denCard < denButton)
        {
            commonDivisor = (float)denButton / denCard;
        }
        else // (denCard > denButton)
        {
            commonDivisor = (float)denCard / denButton;
        }
        return (commonDivisor % 1 <= float.Epsilon); // commonDivisor is not a whole number // denominators can't factor into one another
    }
}

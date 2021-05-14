using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Card deck for the game Fractions Blackout.
/// Another example (with slightly different rules) can be found at https://www.nctm.org/Classroom-Resources/Illuminations/Interactives/Fraction-Game/
/// </summary>
public class FractionDeck : MonoBehaviour
{
    public static FractionDeck Instance { get; set; }
    readonly int[] denominators = new int[] { 2, 3, 4, 5, 6, 8, 10 };

    public Text TxtNumerator = null;
    public Text TxtDenominator = null;
    public Vector2Int CurrentCard;
    public Text TxtCardsLeft = null;
    public Text TxtBadClicks = null;

    private Queue<Vector2Int> Deck;
    private int badClicks = 0;
    // Veronica found some cheats on, https://www.nctm.org/Classroom-Resources/Illuminations/Interactives/Fraction-Game/ I couldn't resist doing my own take on it. Only equivelent fractions are allowed.
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception("More than one FractionDeck! " + this.name + " vs " + Instance.name);
        }
        NewDeck();
        NextCard();
    }

    // void Update() { }

    public void NewDeck()
    {
        var deck = new List<Vector2Int>();

        foreach (var denominator in denominators)
        {
            for (int i = 1; i <= denominator; i++)
            {
                deck.Add(new Vector2Int(i, denominator));
            }
        }
        Deck = new Queue<Vector2Int>(deck.OrderBy(x => Random.value));
    }

    [ContextMenu("Next Card")]
    public void NextCard()
    {
        // first check if deck is empty to restart game
        if (this.gameObject.activeInHierarchy == false)
        {
            Reset();
            this.gameObject.SetActive(true);
        }
        if (Deck.Count == 0)
        {
            this.gameObject.SetActive(false);
            return;
        }

        // draw next card
        CurrentCard = Deck.Dequeue();
        if (CurrentCard != null)
        {
            TxtNumerator.text = CurrentCard.x.ToString();
            TxtDenominator.text = CurrentCard.y.ToString();
            TxtCardsLeft.text = "Cards Left: " + Deck.Count;
        }
    }

    public void Reset()
    {
        NewDeck();

        // Reset bad clicks
        badClicks = -1;
        BadClick();

        // Reset Buttons
        foreach (var fractionButton in FindObjectsOfType<FractionButton>())
        {
            fractionButton.FillBar.fillAmount = 0;
        }
    }

    public void BadClick()
    {
        if (TxtBadClicks != null)
        {
            TxtBadClicks.text = "Misses: " + ++badClicks;
        }
    }

    public float CurrentFraction()
    {
        return (float)CurrentCard.x / CurrentCard.y;
    }
}

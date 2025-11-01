using UnityEngine;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    [Header("Deck Settings")]
    public GameObject CardPrefab;

    private Stack<Card> _cards = new Stack<Card>();

    void Start()
    {
        GenerateDeck(52);
    }

    private void GenerateDeck(int count)
    {
        if (CardPrefab == null)
        {
            return;
        }

        float zDepth = 0f;
        for (int i = 0; i < count; i++)
        {
            GameObject cardGO = Instantiate(CardPrefab, transform);
            Card card = cardGO.GetComponent<Card>();

            if (card != null)
            {
                cardGO.transform.localScale = Vector3.one;

                Vector3 cardPosition = transform.position;
                cardPosition.z = zDepth;
                card.transform.position = cardPosition;
                zDepth -= 0.001f;

                _cards.Push(card);
            }
            else
            {
                Destroy(cardGO);
            }
        }
    }

    public Card GetTopCard()
    {
        if (_cards.Count == 0)
        {
            return null;
        }
        return _cards.Peek();
    }

    public Card DealCard()
    {
        if (_cards.Count == 0)
        {
            return null;
        }
        return _cards.Pop();
    }
}
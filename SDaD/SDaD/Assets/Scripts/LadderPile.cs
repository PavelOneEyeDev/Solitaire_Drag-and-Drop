using System.Collections.Generic;
using UnityEngine;

public class LadderPile : CardPile
{
    protected override Vector3 cardOffset => new Vector3(0f, -0.15f, -0.001f);

    private BoxCollider2D _boxCollider; 

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public override bool CanTakeCard(Card card)
    {
        return true; 
    }

    public override void PopCardsAbove(Card card)
    {
        card.ClearDraggedCards();

        int index = cards.IndexOf(card);
        if (index != -1)
        {
            for (int i = index + 1; i < cards.Count; i++)
            {
                Card cardAbove = cards[i];
                card.DraggedCards.Add(cardAbove);
                cards.RemoveAt(i);
                i--; 
            }
        }
    }

    public override void PlaceCard(Card card)
    {
        base.PlaceCard(card);
    }

    public override void PlaceCards(Card firstCard, List<Card> followUpCards)
    {
        base.PlaceCards(firstCard, followUpCards);
        UpdateCollider();
    }

    public override void RemoveCard(Card card) 
    {
        base.RemoveCard(card);
        UpdateCollider();
    }

    public override void RestoreCards(List<Card> cardsToRestore)
    {
        base.RestoreCards(cardsToRestore);
        UpdateCollider();
    }

    private void UpdateCollider()
    {
        _boxCollider.offset = new Vector2(0f, 0f);
        if (_boxCollider == null || cards.Count <= 1)
        {
            return;
        }
        _boxCollider.offset = new Vector2(0f, cards.Count * (-0.1f));
    }
}

using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class CardPile : MonoBehaviour
{
    protected List<Card> cards = new List<Card>();
    protected abstract Vector3 cardOffset { get; }

    public Vector3 GetCardOffset()
    {
        return cardOffset;
    }

    public virtual bool CanPlaceCard(Card card)
    {
        return true;
    }

    public virtual bool CanTakeCard(Card card)
    {
        return false;
    }

    public virtual void PopCardsAbove(Card card)
    {
    }

    public virtual void PlaceCards(Card firstCard, List<Card> followUpCards)
    {
        PlaceCard(firstCard);

        Card previousCard = firstCard;
        foreach (Card card in followUpCards)
        {
            card.transform.SetParent(this.transform);

            card.transform.position = previousCard.transform.position + cardOffset;

            cards.Add(card);
            previousCard = card;
        }

    }

    public virtual void PlaceCard(Card card) 
    {
        CardPile oldPile = card.transform.parent?.GetComponent<CardPile>(); 
        if (oldPile != null && oldPile != this) 
        {
            oldPile.RemoveCard(card); 
        }
        else 
        {
            Deck oldDeck = card.transform.parent?.GetComponent<Deck>(); 
            if (oldDeck != null) 
            {
                oldDeck.DealCard(); 
            }
        }

        card.transform.SetParent(this.transform); 
        PositionCard(card);
        cards.Add(card); 
    }

    protected void PositionCard(Card card)
    {
        if (cards.Contains(card))
        {
            Vector3 targetPosition; 
            int index = cards.IndexOf(card); 

            if (index <= 0)
            {
                targetPosition = this.transform.position; 
            }
            else
            {
                Card cardBelow = cards[index - 1]; 
                targetPosition = cardBelow.transform.position + cardOffset; 
            }
            card.transform.position = targetPosition; 
            return; 
        }

        if (cards.Count == 0) 
        {
            Vector3 targetPos = this.transform.position; 
            targetPos.z = 0f; 
            card.transform.position = targetPos; 
        }
        else 
        {
            Card lastCard = cards[cards.Count - 1]; 
            card.transform.position = lastCard.transform.position + cardOffset; 
        }
    }

    public virtual void RemoveCard(Card card)
    {
        cards.Remove(card);
    }

    public virtual void RestoreCards(List<Card> cardsToRestore)
    {
        cards.AddRange(cardsToRestore);
    }
}

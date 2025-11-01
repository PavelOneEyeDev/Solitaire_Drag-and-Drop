using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public static Card CurrentDraggingCard;
    public List<Card> DraggedCards = new List<Card>();

    private Vector3 _offset;
    private Vector3 _originalPosition;
    private CardPile _originalPile;

    private CardPile _potentialDropPile;

    public void ClearDraggedCards()
    {
        DraggedCards.Clear();
    }

    void OnMouseDown()
    {
        CardPile parentPile = transform.parent?.GetComponent<CardPile>();
        if (parentPile != null)
        {
            if (!parentPile.CanTakeCard(this))
            {
                CurrentDraggingCard = null;
                return;
            }
            parentPile.PopCardsAbove(this);
        }

        Deck parentDeck = transform.parent?.GetComponent<Deck>();
        if (parentDeck != null && parentDeck.GetTopCard() != this)
        {
            CurrentDraggingCard = null;
            return;
        }

        CurrentDraggingCard = this;
        _originalPosition = transform.position;
        _originalPile = transform.parent?.GetComponent<CardPile>();

        Vector3 touchPos = GetTouchWorldPosition();
        _offset = transform.position - touchPos;
        _offset.z = 0;
        Vector3 newPosition = transform.position;
        newPosition.z = -5f;
        transform.position = newPosition;
    }

    void OnMouseDrag()
    {
        if (CurrentDraggingCard == this)
        {
            Vector3 targetPos = GetTouchWorldPosition() + _offset;

            targetPos.z = transform.position.z;

            transform.position = targetPos;

            Vector3 offsetToNextCard = Vector3.zero;
            if (DraggedCards.Count > 0)
            {
                CardPile originalPileScript = _originalPile as LadderPile; 
                if (originalPileScript != null)
                {
                    offsetToNextCard = originalPileScript.GetCardOffset();
                }
            }

            Vector3 previousCardPos = transform.position;
            foreach (Card card in DraggedCards)
            {
                card.transform.position = previousCardPos + offsetToNextCard;
                previousCardPos = card.transform.position;
            }
        }
    }

    void OnMouseUp()
    {
        if (CurrentDraggingCard == this)
        {
            if (_potentialDropPile != null && _potentialDropPile.CanPlaceCard(this))
            {
                _potentialDropPile.PlaceCards(this, DraggedCards); 
            }
            else
            {
                ReturnToOriginalPosition();
            }
            CurrentDraggingCard.ClearDraggedCards();
            CurrentDraggingCard = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CardPile pile = other.GetComponent<CardPile>();
        if (pile != null)
        {
            _potentialDropPile = pile;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CardPile pile = other.GetComponent<CardPile>();
        if (pile != null && _potentialDropPile == pile)
        {
            _potentialDropPile = null;
        }
    }

    private Vector3 GetTouchWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void ReturnToOriginalPosition()
    {
        transform.position = _originalPosition;

        Vector3 previousCardPos = _originalPosition;
        if (_originalPile != null)
        {
            Vector3 offsetToNextCard = _originalPile.GetCardOffset();
            foreach (Card card in DraggedCards)
            {
                previousCardPos += offsetToNextCard;
                card.transform.position = previousCardPos;
                card.transform.SetParent(_originalPile.transform);
            }
            _originalPile.RestoreCards(DraggedCards);
        }
    }
}

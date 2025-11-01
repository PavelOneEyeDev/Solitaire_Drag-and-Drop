using UnityEngine;

public class FoundationPile : CardPile
{
    protected override Vector3 cardOffset => new Vector3(0f, 0f, -0.001f);

    public override bool CanPlaceCard(Card card)
    {
        if (card.DraggedCards.Count > 0)
        {
            return false;
        }
        return true;
    }
}

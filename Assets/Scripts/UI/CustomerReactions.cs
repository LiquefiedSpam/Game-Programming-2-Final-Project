using UnityEngine;

[CreateAssetMenu(fileName = "CustomerReactions", menuName = "Scriptable Objects/Customer Reactions")]
public class CustomerReactions : ScriptableObject
{
    [SerializeField] Sprite angrySprite;
    [SerializeField] Sprite annoyedSprite;
    [SerializeField] Sprite okaySprite;
    [SerializeField] Sprite amazingSprite;

    public Sprite GetReactionSprite(CustomerReaction reaction)
    {
        return reaction switch
        {
            CustomerReaction.ANGRY => angrySprite,
            CustomerReaction.ANNOYED => annoyedSprite,
            CustomerReaction.OKAY => okaySprite,
            _ => amazingSprite,
        };
    }
}

public enum CustomerReaction
{
    ANGRY, ANNOYED, OKAY, AMAZING
}

using UnityEngine;

[CreateAssetMenu(fileName = "CustomerReactions", menuName = "Scriptable Objects/Customer Reactions")]
public class CustomerReactions : ScriptableObject
{
    [SerializeField] Sprite angrySprite;
    [SerializeField] Sprite goodSprite;
    [SerializeField] Sprite greatSprite;

    public Sprite GetReactionSprite(CustomerReaction reaction)
    {
        return reaction switch
        {
            CustomerReaction.CHEAP => greatSprite,
            CustomerReaction.TARGET => goodSprite,
            _ => angrySprite
        };
    }
}

public enum CustomerReaction
{
    CHEAP, TARGET, EXPENSIVE
}

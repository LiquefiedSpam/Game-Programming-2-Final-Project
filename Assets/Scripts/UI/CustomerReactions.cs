using UnityEngine;

[CreateAssetMenu(fileName = "CustomerReactions", menuName = "Scriptable Objects/CustomerReactions")]
public class CustomerReactions : ScriptableObject
{
    [SerializeField] Sprite angrySprite;
    [SerializeField] Sprite annoyedSprite;
    [SerializeField] Sprite okaySprite;
    [SerializeField] Sprite happySprite;

    public Sprite GetSprite(CustomerReaction reaction)
    {
        return reaction switch
        {
            CustomerReaction.ANGRY => angrySprite,
            CustomerReaction.ANNOYED => annoyedSprite,
            CustomerReaction.OKAY => okaySprite,
            _ => happySprite
        };
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "New Speaker", menuName = "Dialogue/New Speaker")]
public class SpeakerScript : ScriptableObject
{
    [SerializeField] private string speakerName;
    // [SerializeField] private Sprite speakerSprite;

    public string GetName()
    {
        return speakerName;
    }

    /* public Sprite GetSprite(){
     *      return speakerSprite;
     * }*/
}

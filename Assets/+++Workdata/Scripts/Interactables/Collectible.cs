using UnityEngine;

public class Collectible : MonoBehaviour
{
    //the linked collectible data, which is a ScriptableObject.
    //each has to be uniquely named, because with that unique name the data is identified within the
    //save game.
    [SerializeField] private CollectibleObjectSO collectibleData;

    private void Awake()
    {
        //When loading the scene, we destroy the collectible, if it was already saved as collected.
        if (GameSaveStateManager.instance.saveGameDataManager.HasCollectible(collectibleData.header))
            Destroy(gameObject);
    }

    //Collect is called from the Player script

    public void Collect()
    {
        //We add the unique name of the collectible to the data, once the collectible is collected.
        //This means that it will be saved as well.
        GameSaveStateManager.instance.saveGameDataManager.AddCollectible(collectibleData.header);
        Destroy(gameObject);
    }

    //OnValidate is only called in editor when something about this script changed.
    //Here, we only change the game object name to represent what pickup is linked, 
    //without us having to change the name by hand
    private void OnValidate()
    {
        if (collectibleData == null)
            name = "[Collectible] -unasigned-";
        else
            name = "[Collectible] " + collectibleData.header;
    }
}
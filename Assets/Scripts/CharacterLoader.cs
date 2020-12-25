using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    public GameObject player { get; set; }

    public void LoadCharacter()
    {
        player = GameObject.Find(Global.characters[UISelectCharacter.characterIndex].Name + "Prefab");
    }

    private void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
        }
    }
}

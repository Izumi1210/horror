using UnityEngine;

public class RemoveDialogueManager : MonoBehaviour
{
    void Start()
    {
        var dm = GameObject.Find("Dialogue Manager");
        if (dm != null)
        {
            Destroy(dm);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement; // �Y��Ȃ��I�I
public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string scene0)
    {
        FadeManager.Instance.LoadScene(scene0, 2.0f);
    }
}
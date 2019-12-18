using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void OnClick()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        Application.LoadLevel("BattlefieldTest");
    }
}
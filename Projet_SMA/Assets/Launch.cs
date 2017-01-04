using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Launch : MonoBehaviour {

    public void loadByIndex(int sceneIndex)
    {
        Debug.Log("Load scene");
        SceneManager.LoadScene(sceneIndex);
    }
}

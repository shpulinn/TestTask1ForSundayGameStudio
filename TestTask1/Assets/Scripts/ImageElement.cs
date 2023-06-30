using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImageElement : MonoBehaviour
{
    private Image _image;

    private const string SCENE_NAME = "ViewScene";

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void Click()
    {
        ImageClass.image = _image.sprite;
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>(true);
        if (sceneLoader != null)
        {
            sceneLoader.gameObject.SetActive(true);
            sceneLoader.LoadSceneAsync(SCENE_NAME);
        } else
        {
            SceneManager.LoadScene(SCENE_NAME);
        }
    }
}
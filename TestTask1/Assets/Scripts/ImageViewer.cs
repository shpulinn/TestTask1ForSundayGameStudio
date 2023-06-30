using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImageViewer : MonoBehaviour
{
    [SerializeField] private Image image;

    private const string SCENE_NAME = "GalleryScene";
    private void Awake()
    {
        image.sprite = ImageClass.image;

        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    private void Update()
    {
        // check for Android "back" button input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadSceneGallery();
        }
    }

    public void LoadSceneGallery()
    {
        SceneLoader sceneLoader = FindObjectOfType<SceneLoader>(true);
        if (sceneLoader != null)
        {
            sceneLoader.gameObject.SetActive(true);
            sceneLoader.LoadSceneAsync(SCENE_NAME);
        }
        else
        {
            SceneManager.LoadScene(SCENE_NAME);
        }
    }
}
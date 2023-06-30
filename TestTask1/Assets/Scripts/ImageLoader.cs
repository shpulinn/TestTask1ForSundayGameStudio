using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float scrollPercentageThreshold = 0.8f;
    [SerializeField] private GameObject imagePrefab; 

    [SerializeField] private string mainURL;
    [SerializeField] private int imagesCountMax = 66; 
    [SerializeField] private int imagesPerPage = 20; // images amount on one page

    private int _currentPage = 1; // current page

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        LoadPage(_currentPage);
    }

    private void LoadPage(int page)
    {
        // Calculate the starting image index for the current page
        int startIndex = (page - 1) * imagesPerPage;
        // Calculate the final image index for the current page
        int endIndex = Mathf.Min(startIndex + imagesPerPage, imagesCountMax); 

        for (int i = startIndex; i < endIndex; i++)
        {
            // create a new image object from image prefab
            GameObject imageObject = Instantiate(imagePrefab, gridLayout.transform);
            Image imageComponent = imageObject.GetComponent<Image>();

            // start loading coroutine
            StartCoroutine(LoadImage(mainURL, i + 1, imageComponent));
        }
    }

    private IEnumerator LoadImage(string url, int index, Image image)
    {
        string filePath = Application.persistentDataPath + "/" + index + ".jpg";
        if (System.IO.File.Exists(filePath))
        {
            // read bytes array from file
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);

            // create a texture from file
            Texture2D texture = new Texture2D(2, 2);

            // load imahe from texture
            texture.LoadImage(bytes);

            // create sprite
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            yield break;
        }
        else {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url + index + ".jpg");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                // compress images for better memory usage
                byte[] bytes = texture.EncodeToJPG(60);

                // save bytes array to file
                System.IO.File.WriteAllBytes(filePath, bytes);

            }
            else
            {
                Debug.Log("Image loading error: " + www.error);
            }
        }
    }

    public void OnScroll()
    {
        // Counting when the user reaches the end of the list of images
        float contentHeight = gridLayout.transform.childCount * (gridLayout.cellSize.y + gridLayout.spacing.y);
        float scrollHeight = scrollRect.viewport.rect.height;
        float scrollPosition = scrollRect.content.anchoredPosition.y;
        float scrollPercentage = (scrollHeight + scrollPosition) / contentHeight;

        // load new images if user reaches the end of the list of images
        if (scrollPercentage > scrollPercentageThreshold && _currentPage < Mathf.CeilToInt((float)imagesCountMax / imagesPerPage))
        {
            _currentPage++;
            LoadPage(_currentPage);
        }
    }
}

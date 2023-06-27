using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Slider progressBar;
    private AsyncOperation operation;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneAsync(string sceneName)
    {
        operation = SceneManager.LoadSceneAsync(sceneName);
    }

    private void Update()
    {
        if (operation == null)
        {
            return;
        }

        //progressBar.value = operation.progress;
        progressBar.value = Mathf.Lerp(progressBar.value, operation.progress, Time.deltaTime * 1.5f);

        if (operation.isDone && progressBar.value >= .95f)
        {
            // Загрузка завершена, скрываем экран загрузки
            gameObject.SetActive(false);
        }
    }
}
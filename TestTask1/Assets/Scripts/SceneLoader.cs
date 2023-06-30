using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    private AsyncOperation _operation;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneAsync(string sceneName)
    {
        _operation = SceneManager.LoadSceneAsync(sceneName);
    }

    private void Update()
    {
        if (_operation == null)
        {
            return;
        }

        // a little delay on loading
        progressBar.value = Mathf.Lerp(progressBar.value, _operation.progress, Time.deltaTime * 1.5f);

        if (_operation.isDone && progressBar.value >= .95f)
        {
            // hide loading screen on scene successfully loaded
            gameObject.SetActive(false);
            progressBar.value = 0f;
            _operation = null;
        }
    }
}
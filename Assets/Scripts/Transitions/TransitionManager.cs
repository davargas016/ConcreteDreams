using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    [Header("Optional: assign a screen fader (see ISceneFader below)")]
    public MonoBehaviour faderBehaviour;
    private ISceneFader _fader;

    private string _nextSpawnId;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _fader = faderBehaviour as ISceneFader;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void GoToScene(string sceneName, string spawnId, float fadeDuration = 0.2f)
    {
        _nextSpawnId = spawnId;
        StartCoroutine(LoadRoutine(sceneName, fadeDuration));
    }

    private IEnumerator LoadRoutine(string sceneName, float fadeDuration)
    {
        if (_fader != null) yield return _fader.FadeOut(fadeDuration);

        var op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone) yield return null;

        if (_fader != null) yield return _fader.FadeIn(fadeDuration);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("No GameObject tagged 'Player' found in the loaded scene.");
            return;
        }

        if (string.IsNullOrEmpty(_nextSpawnId))
            return;

        var spawns = GameObject.FindObjectsOfType<SpawnPoint>();
        foreach (var sp in spawns)
        {
            if (sp.spawnId == _nextSpawnId)
            {
                player.transform.position = sp.transform.position;
                _nextSpawnId = null;
                return;
            }
        }

        Debug.LogWarning($"SpawnPoint with id '{_nextSpawnId}' not found in scene '{scene.name}'.");
        _nextSpawnId = null;
    }
}

public interface ISceneFader
{
    IEnumerator FadeOut(float duration);
    IEnumerator FadeIn(float duration);
}
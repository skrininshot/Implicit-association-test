using System.Collections;
using UnityEngine.SceneManagement;

namespace Services
{

    public class SceneLoader : ISceneLoader
    {
        private const string MainSceneName = "MainScene"; 

        public IEnumerator LoadMainScene()
        {
            var asyncLoad = SceneManager.LoadSceneAsync(MainSceneName);
            while (!asyncLoad.isDone)
                yield return null;
        }
    }
}
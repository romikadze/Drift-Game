using UnityEngine.SceneManagement;

namespace Source.Scripts
{
    public class SceneLoader
    {
        private const string MAIN_MENU_SCENE = "MainMenu";
        
        public void LoadMainMenu()
        {
            SceneManager.LoadScene(MAIN_MENU_SCENE);
        }

        public void LoadGame(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }
    }
}
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene { MainMenuScene, GameScene1, LoadingScene }
    private static Scene targetScene;
    public static void Load(Scene scene)
    {
        targetScene = scene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }
    public static void LoaderCallback() { SceneManager.LoadScene(targetScene.ToString()); }
    public static Scene GetGameSceneNumber(int levelNumber)
    {
        return levelNumber == 1 ? Scene.GameScene1 : Scene.MainMenuScene;
    }
}
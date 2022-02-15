using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausemenu;

    // private void Awake() {
    //     pausemenu.SetActive(false);
    // }

    public void Resume() {
        Debug.Log("Resume");

        Time.timeScale = 1;
        pausemenu.SetActive(false);
    }

    public void GotoMenu() {
        Debug.Log("Go to menu!");

        // Go to the first scene (main menu)
        SceneManager.LoadScene(0);
    }

    public void Quit() {
        Debug.Log("Quit!");

        Application.Quit();
    }

    public void NextLevel() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

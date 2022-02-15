using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject settings;
    public GameObject about;

    private void Awake() {
        menu.SetActive(true);
        settings.SetActive(false);
        about.SetActive(false);
    }

    public void Quit() {
        Debug.Log("Quit!");

        Application.Quit();
    }

    public void NextLevel() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Settings() {
        menu.SetActive(false);
        settings.SetActive(true);
    }
    
    public void About() {
        menu.SetActive(false);
        about.SetActive(true);
    }

    public void Back() {
        menu.SetActive(true);
        settings.SetActive(false);
        about.SetActive(false);
    }
}

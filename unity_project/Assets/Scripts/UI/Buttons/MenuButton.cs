using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Buttons
{
    public class MenuButton : MonoBehaviour
    {
        public void OnMenuButtonClicked()
        {
            // Load the main menu scene
            SceneManager.LoadScene("SettingsScene");
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Buttons
{
    public class StartButtonHandler : MonoBehaviour
    {
        public SettingsUI SettingsUI;
        
        public void OnStartButtonClicked()
        {
            SettingsUI.UpdateSettings();
            SceneManager.LoadScene("MainScene");
        }
    }
}
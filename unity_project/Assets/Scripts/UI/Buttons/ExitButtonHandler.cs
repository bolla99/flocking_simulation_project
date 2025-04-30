using UnityEngine;

namespace UI.Buttons
{
    public class ExitButtonHandler : MonoBehaviour
    {
        public void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}

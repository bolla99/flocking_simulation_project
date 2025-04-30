using UnityEngine;
using Controllers;

namespace UI.Toggles
{
    public class MouseModeToggleHandler : MonoBehaviour
    {
        public TargetController Target;
        public void OnMouseSeekToggleChanged(bool value)
        {
            if (!value) Target.SetRandomPosition();
            GameManager.Instance().MouseSeekingModeEnabled = value;
        }
    }
}

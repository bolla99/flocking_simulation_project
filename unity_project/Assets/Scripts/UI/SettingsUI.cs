using TMPro;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

/// \internal
namespace UI
{
    public class SettingsUI : MonoBehaviour
    {
        public Settings Settings;
        public TMP_InputField BoidsCountInput;
        public TMP_InputField BoidsSpeedInput;
        public TMP_InputField SmoothingMultiplierInput;
        public TMP_InputField ObstaclesDetectionRadiusInput;
        public TMP_InputField CollisionPredictionThresholdInput;
        public TMP_InputField ObstacleMinSpeedInput;
        public TMP_InputField ObstacleMaxSpeedInput;
        public Slider ObstaclesCountSlider;

        private void Start()
        {
            Settings.Load();
            BoidsCountInput.text = Settings.BoidsCount.ToString();
            BoidsSpeedInput.text = Settings.BoidsSpeed.ToString();
            SmoothingMultiplierInput.text = Settings.SmoothingMultiplier.ToString();
            ObstaclesDetectionRadiusInput.text = Settings.ObstaclesDetectionRadius.ToString();
            CollisionPredictionThresholdInput.text = Settings.CollisionPredictionThreshold.ToString();
            ObstacleMinSpeedInput.text = Settings.ObstaclesMinSpeed.ToString();
            ObstacleMaxSpeedInput.text = Settings.ObstaclesMaxSpeed.ToString();
            ObstaclesCountSlider.value = Settings.ObstaclesCount;
        }

        public void UpdateSettings()
        {
            // Update settings with values from input fields
            Settings.BoidsCount = int.Parse(BoidsCountInput.text);
            Settings.BoidsSpeed = float.Parse(BoidsSpeedInput.text);
            Settings.SmoothingMultiplier = float.Parse(SmoothingMultiplierInput.text);
            Settings.ObstaclesDetectionRadius = float.Parse(ObstaclesDetectionRadiusInput.text);
            Settings.CollisionPredictionThreshold = float.Parse(CollisionPredictionThresholdInput.text);
            Settings.ObstaclesMinSpeed = float.Parse(ObstacleMinSpeedInput.text);
            Settings.ObstaclesMaxSpeed = float.Parse(ObstacleMaxSpeedInput.text);
            Settings.ObstaclesCount = (int)ObstaclesCountSlider.value;

            // Save the updated settings
            Settings.Save();
        }
    }
}
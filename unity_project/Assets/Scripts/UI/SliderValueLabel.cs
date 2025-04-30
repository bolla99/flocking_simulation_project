using System.Globalization;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class SliderValueLabel : MonoBehaviour
    {
        public Slider Slider;

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<TMP_Text>().text = Slider.value.ToString(CultureInfo.InvariantCulture);
            Slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            GetComponent<TMP_Text>().text = value.ToString(CultureInfo.InvariantCulture);
        }
    }
}

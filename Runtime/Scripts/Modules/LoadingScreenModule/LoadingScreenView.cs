using UnityEngine;
using UnityEngine.UI;

namespace AppBuilder
{
    public class LoadingScreenView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        public Slider Slider => _slider;
    }
}

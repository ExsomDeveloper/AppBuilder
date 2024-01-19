using System.Collections.Generic;
using UnityEngine;

namespace AppBuilder
{
    [DefaultExecutionOrder(-10000)]
    public class AppBuilderBootstrap : MonoBehaviour
    {
        [SerializeField] private List<Installer> _installers;
        [SerializeField] private List<InstallerAsync> _installersAsync;

        private async void Awake()
        {
            await AppBuilder.Run(_installers, _installersAsync);
        }
    }
}

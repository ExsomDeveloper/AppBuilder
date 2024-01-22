using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PimpochkaGames.AppBuilder
{
    public abstract class InstallerAsync : ScriptableObject
    {
        public abstract UniTask InstallAsync(Context context);
    }
}

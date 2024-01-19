using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class InstallerAsync : ScriptableObject
{
    public abstract UniTask InstallAsync(Context context);
}

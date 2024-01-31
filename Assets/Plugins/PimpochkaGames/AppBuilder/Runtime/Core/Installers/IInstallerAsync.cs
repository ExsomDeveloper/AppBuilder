using Cysharp.Threading.Tasks;

namespace PimpochkaGames.AppBuilder
{
    public interface IInstallerAsync 
    {
        UniTask InstallAsync(Context context);
    }
}
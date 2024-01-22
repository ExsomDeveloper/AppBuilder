using UnityEngine;

namespace PimpochkaGames.AppBuilder
{
    public abstract class Installer : ScriptableObject
    {
        public abstract void Install(Context context);
    }
}

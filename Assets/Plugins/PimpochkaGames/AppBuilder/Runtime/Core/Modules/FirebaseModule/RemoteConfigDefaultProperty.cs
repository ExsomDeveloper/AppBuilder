#if AB_FIREBASE_REMOTE_CONFIG
namespace PimpochkaGames.AppBuilder
{
    [System.Serializable]
    public class RemoteConfigDefaultProperty<T>
    {
        public string Key;
        public T Value;
    }
}
#endif
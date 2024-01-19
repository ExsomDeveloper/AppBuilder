using System.Collections;

namespace AppBuilder
{
    public interface ITimerHandler
    {
        public IEnumerator Execute();
    }
}
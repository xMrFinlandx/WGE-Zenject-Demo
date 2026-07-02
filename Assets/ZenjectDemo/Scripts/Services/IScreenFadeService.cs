using System.Threading;
using System.Threading.Tasks;

namespace ZenjectDemo
{
    public interface IScreenFadeService
    {
        public Task ShowAsync(CancellationToken ctsToken);
        public Task HideAsync(CancellationToken ctsToken);
    }
}
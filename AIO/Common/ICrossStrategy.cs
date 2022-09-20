using System.Threading.Tasks;

namespace AIO.Modules
{

    public interface IGameEngine
    {
        Task<bool> Authorize();
    }


    public interface ICrossStrategy
    {
        void startNewGameEngine(string game, string currency, string stratFile);
    }

}
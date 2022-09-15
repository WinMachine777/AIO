namespace AIO.Modules
{
    public interface ICrossStrategy
    {
        //void OpenFromScript(object source, string game, string apiKey, string mirror, string currency, string stratFile);
        void startNewGameEngine(string game, string currency, string stratFile);
    }
}
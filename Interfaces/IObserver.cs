namespace SofEngeneering_project.Interfaces
{
    public interface IObserver
    {
        void OnNotify(int coinsRemaining, bool hasPowerUp, float powerUpTimeLeft, int lives);
    }
}
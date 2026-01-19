namespace SofEngeneering_project.Interfaces
{
    public interface IObserver
    {
        // AANGEPAST: Coins, PowerUp Actief?, Tijd over
        void OnNotify(int coinsRemaining, bool hasPowerUp, float powerUpTimeLeft);
    }
}
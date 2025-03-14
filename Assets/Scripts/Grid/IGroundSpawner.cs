using Card;

namespace Grid
{
    public interface IGroundSpawner
    {
        void SpawnCardOnRandomPosition(GroundConfig config);
    }
}
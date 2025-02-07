using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum GamePhase
    {
        PhaseOne = 0,
        PhaseTwo = 1
    }
    
    GamePhase currentGamePhase = GamePhase.PhaseOne;
    public static GameManager Instance;

    public GamePhase GetCurrentPhase()
    {
        return currentGamePhase;
    }
}

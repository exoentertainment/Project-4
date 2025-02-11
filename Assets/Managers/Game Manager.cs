using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] PlayerInput playerInput;
    
    [SerializeField] private GameObject[] phaseOneUI;
    [SerializeField] private GameObject[] phaseTwoUI;

    #endregion
    
    public enum GamePhase
    {
        PhaseOne = 0,
        PhaseTwo = 1
    }
    
    GamePhase currentGamePhase = GamePhase.PhaseOne;
    public static GameManager Instance;
    
    private void Start()
    {
        PhaseOneSetup();
    }

    void PhaseOneSetup()
    {
        playerInput.SwitchCurrentActionMap("UI");
        Debug.Log(playerInput.currentActionMap.ToString());
        playerInput.SwitchCurrentActionMap("Player");
        Debug.Log(playerInput.currentActionMap.ToString());
    }
    
    public GamePhase GetCurrentPhase()
    {
        return currentGamePhase;
    }

    public void DeactivatePhaseOneUI()
    {
        foreach (GameObject element in phaseOneUI)
        {
            Destroy(element);
        }
    }
}

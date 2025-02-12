using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] PlayerInput playerInput;
    
    [SerializeField] private GameObject[] phaseOneUI;
    [SerializeField] private GameObject[] phaseTwoUI;
    
    [SerializeField] UnityEvent initPhaseTwo;

    #endregion
    
    public enum GamePhase
    {
        PhaseOne = 0,
        PhaseTwo = 1
    }
    
    GamePhase currentGamePhase = GamePhase.PhaseOne;
    
    public static GameManager Instance;

    //Invokes any event associated with the beginning of Phase 2. Calls all internal functions related to the beginning of Phase 2
    public void PhaseTwoSetup()
    {
        initPhaseTwo?.Invoke();
        
        //DeactivatePhaseOneUI();
        ActivatePhaseTwoUI();
        SwitchInputProfile();
        EnableEnemySpawners();
    }
    
    public GamePhase GetCurrentPhase()
    {
        return currentGamePhase;
    }

    #region --Phase 2 Setup--
    
    //Enable all UI elements associated with player flying their ship
    void ActivatePhaseTwoUI()
    {
        foreach (GameObject element in phaseTwoUI)
        {
            element.SetActive(true);
        }
    }

    //Switch player action map to Player
    void SwitchInputProfile()
    {
        playerInput.SwitchCurrentActionMap("Player");
    }
    
    //Enable the enemy spawners
    private void EnableEnemySpawners()
    {
        
    }

    #endregion
}

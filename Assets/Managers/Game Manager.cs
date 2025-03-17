using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] PlayerInput playerInput;
    
    //[FormerlySerializedAs("phaseOneUI")] [SerializeField] private GameObject[] phaseOneObjects;
    //[FormerlySerializedAs("phaseTwoUI")] [SerializeField] private GameObject[] phaseTwoObjects;
    
    [FormerlySerializedAs("initPhaseTwo")] [SerializeField] UnityEvent eventInitPhaseTwo;
    [SerializeField] private UnityEvent eventInitPhaseOne;

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
        if(Time.timeScale == 0)
            Time.timeScale = 1;
        
        currentGamePhase = GamePhase.PhaseTwo;
        eventInitPhaseTwo?.Invoke();
        
        EnableEnemySpawners();
    }
    
    public void ActivateTurretPlacementMode(InputAction.CallbackContext context)
    {
        if(context.performed)
            if (currentGamePhase == GamePhase.PhaseTwo)
            {
                Time.timeScale = 0;

                currentGamePhase = GamePhase.PhaseOne;
                eventInitPhaseOne?.Invoke();
            }
    }
    
    public GamePhase GetCurrentPhase()
    {
        return currentGamePhase;
    }

    #region --Phase 2 Setup--
    
    //Enable the enemy spawners
    private void EnableEnemySpawners()
    {
        
    }

    #endregion
}

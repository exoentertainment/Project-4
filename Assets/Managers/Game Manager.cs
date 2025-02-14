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
    
    [FormerlySerializedAs("phaseOneUI")] [SerializeField] private GameObject[] phaseOneObjects;
    [FormerlySerializedAs("phaseTwoUI")] [SerializeField] private GameObject[] phaseTwoObjects;
    
    [FormerlySerializedAs("initPhaseTwo")] [SerializeField] UnityEvent eventInitPhaseTwo;

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
        eventInitPhaseTwo?.Invoke();
        
        ActivatePhaseTwoUI();
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
        foreach (GameObject element in phaseTwoObjects)
        {
            element.SetActive(true);
        }
    }
    
    //Enable the enemy spawners
    private void EnableEnemySpawners()
    {
        
    }

    #endregion
}

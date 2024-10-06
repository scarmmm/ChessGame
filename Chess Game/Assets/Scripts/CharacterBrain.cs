using System.Collections;
using System.Collections.Generic;
using Animancer.FSM;
using Animancer;
using UnityEngine;
using TMPro;
[DefaultExecutionOrder(-10000)]// Initialize the StateMachine before anything uses it.
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private AnimancerComponent _animancer;
    public AnimancerComponent Animancer => _animancer;
    [SerializeField]
    private StateMachine<CharacterState>.WithDefault _StateMachine;
    public StateMachine<CharacterState>.WithDefault StateMachine => _StateMachine; //access the state machine from the outside
    [SerializeField] private CharacterState _Move;
    [SerializeField] private CharacterState _Action;

    private CharacterController controller;
    [SerializeField] private InputReader inputReader;
    private Transform cameraMain;
    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraMain = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

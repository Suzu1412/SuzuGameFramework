using System.Collections;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    private IAgent _agent;
    private Coroutine _transitionCoroutine;
    private float _handleTransitionTime = 0.1f;
    [SerializeField] private StateListSO _states; // All Possible States
    [SerializeField] private StateSO _currentState;
    [SerializeReference] private StateContext _context;

    public StateSO CurrentState => _currentState;
    public StateListSO StateList => _states;
    public StateContext Context => _context;

    internal IAgent Agent => _agent = _agent != null ? _agent : _agent = GetComponentInChildren<IAgent>();


    private void OnEnable()
    {
        _transitionCoroutine = StartCoroutine(TransitionCoroutine());
        Agent.Input.OnMovement += HandleMovement;
        Agent.Input.OnAttackPressed += HandleAttack;
    }

    private void OnDisable()
    {

        StopAllCoroutines();
        Agent.Input.OnMovement -= HandleMovement;
        Agent.Input.Attack -= HandleAttack;

        if (_currentState == null || _context == null) return;
        _currentState.OnExit(_context);

    }

    private void Update()
    {
        if (_currentState == null || _context == null) return;
        _currentState.OnUpdate(_context);
    }

    private void FixedUpdate()
    {
        if (_currentState == null || _context == null) return;
        _currentState.OnFixedUpdate(_context);
    }

    public void SetStates(StateListSO states)
    {
        _states = states;
        InitializeContext();
        ChangeState(_states.DefaultState);
    }


    internal void Transition()
    {
        StateSO bestState = null;
        float highestUtility = 0f;

        if (_currentState == null || _context == null) return;

        foreach (var state in _states.AllStates)
        {
            float utility = state.EvaluateUtility(_context);

            if (utility > highestUtility)
            {
                highestUtility = utility;
                bestState = state;
            }
        }

        if (bestState != null && bestState != _currentState)
        {
            ChangeState(bestState);
        }
    }

    internal void ChangeState(StateSO state)
    {
        if (_currentState != null && _context != null)
        {
            _currentState.OnExit(_context);
        }

        _currentState = state;

        if (_currentState != null && _context != null)
        {
            _currentState.OnEnter(_context);
        }
    }

    private IEnumerator TransitionCoroutine()
    {
        while (true)
        {
            Transition();
            yield return Helpers.GetWaitForSeconds(_handleTransitionTime);
        }
    }

    private void InitializeContext()
    {
        _context = _states.DefaultState.CreateContext();
        _context.Initialize(Agent, this);
    }

    private void OnDrawGizmos()
    {
        if (_currentState == null || _context == null) return;
        _currentState.DrawGizmos(_context);
    }

    private void HandleMovement(Vector2 direction)
    {
        if (_currentState == null || _context == null) return;
        _currentState.HandleMovement(_context, direction);
    }

    private void HandleAttack(bool isAttacking)
    {
        if (_currentState == null || _context == null) return;
        _currentState.HandleAttack(_context, isAttacking);
    }
}

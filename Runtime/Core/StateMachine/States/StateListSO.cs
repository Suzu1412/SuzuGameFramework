using System.Collections.Generic;
using UnityEngine;

public class StateListSO : MonoBehaviour
{
    [SerializeField] private StateSO _defaultState;
    [SerializeField] private List<StateSO> _allStates;

    public StateSO DefaultState => _defaultState;
    public List<StateSO> AllStates => _allStates;

    private void OnValidate()
    {
        if (!AllStates.Contains(DefaultState))
        {
            Debug.LogWarning($"{name}: DefaultState is not in AllStates list.");
        }

        var seenUtilities = new Dictionary<float, StateSO>();
        foreach (var state in AllStates)
        {
            if (state == null)
            {
                Debug.LogWarning($"[StateListSO: {name}] Found a null state.");
                continue;
            }

            float utility = state.Utility; // You may need a public property or getter
            if (utility <= 0f)
            {
                Debug.LogWarning($"[StateListSO: {name}] State '{state.name}' has non-positive utility: {utility}");
            }

            if (seenUtilities.ContainsKey(utility))
            {
                Debug.LogWarning($"[StateListSO: {name}] Duplicate utility {utility} found in states: '{state.name}' and '{seenUtilities[utility].name}'");
            }
            else
            {
                seenUtilities.Add(utility, state);
            }
        }
    }
}

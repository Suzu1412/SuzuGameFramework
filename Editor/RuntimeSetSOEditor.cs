#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(RuntimeSetSO<>), true)]
public class RuntimeSetSOEditor : Editor
{
    private IRuntimeSet _runtimeSet;
    private ListView _itemsListView;
    private Label _listLabel;

    public override VisualElement CreateInspectorGUI()
    {
        _runtimeSet = target as IRuntimeSet;
        var root = new VisualElement();

        InspectorElement.FillDefaultInspector(root, serializedObject, this);

        root.Add(new Label("Runtime Set Items:"));

        _listLabel = new Label();
        root.Add(_listLabel);

        _itemsListView = new ListView(
            _runtimeSet.Items,
            20,
            () => new Label(),
            (element, index) =>
            {
                var obj = _runtimeSet.GetItem(index);
                var label = element as Label;
                label.text = obj != null ? obj.name : "<null>";

                label.RegisterCallback<MouseDownEvent>(_ =>
                {
                    if (obj != null)
                        EditorGUIUtility.PingObject(obj);
                });
            });

        root.Add(_itemsListView);
        UpdateLabel();
        return root;
    }

    private void UpdateLabel()
    {
        if (_listLabel != null)
        {
            _listLabel.text = $"Runtime Set Items (Count: {_runtimeSet.Count})";
        }
    }
}
#endif

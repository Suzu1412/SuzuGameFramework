using UnityEngine;
/// <summary>
/// Read Only attribute.
/// Attribute is use only to mark ReadOnly properties.
/// </summary>
public class ReadOnlyAttribute : PropertyAttribute
{
    public string Tooltip { get; }

    public ReadOnlyAttribute(string tooltip = null)
    {
        Tooltip = tooltip;
    }
}
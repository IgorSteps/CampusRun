using Unity.VisualScripting;
using UnityEditor.PackageManager;

/// <summary>
/// Constants provides constant values to the whole system.
/// </summary>
public class Constants
{
    // The length of the section(spans along the z-axis).
    public static readonly float SECTION_LENGTH = 30;
    // Right-most x coordinate for a section.
    public static readonly float SECTION_AREA_START = 18;
    // Left-most x coordinate for a section.
    public static readonly float SECTION_AREA_END = -18;

    // Right-most x coordinate for the player area.
    public static readonly float PLAYER_AREA_START = 4.5f;
    // Left-most x coordinate for the player area.
    public static readonly float PLAYER_AREA_END = -4.5f;

    public static readonly float DEFAULT_ROATION_SPEED = 1.0f;

    // Player speeds.
    public static readonly float DEFAULT_PLAYER_FORWARD_SPEED = 7.0f;
    public static readonly float DEFAULT_PLAYER_SIDE_SPEED = 4.0f;

    public static readonly float DOWNWARD_GRAVITY_FORCE = -9.81f;
}
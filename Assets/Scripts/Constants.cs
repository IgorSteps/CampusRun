using UnityEngine;
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

    // Coin rotation.
    public static readonly float DEFAULT_ROATION_SPEED = 1.0f;

    // Player.
    public static readonly float DEFAULT_PLAYER_FORWARD_SPEED = 7.0f;
    public static readonly float DEFAULT_PLAYER_SIDE_SPEED = 4.0f;
    public static readonly float DOWNWARD_GRAVITY_FORCE = -10.0f;
    public static readonly float DEFAULT_JUMP_HEIGHT = 0.5f;

    // Lanes configurations.
    public static readonly int NUM_OF_LANES = 3;
    public static readonly Vector3 START_POS = new(3.0f, 1.0f, 0.0f);

    // Car placement:
    public static readonly float CAR_Y_OFFSET = 0.9f;
    public static readonly float MIN_CAR_SPACING = 6.0f;
    public static readonly float MAX_CAR_SPACING = 10.0f;
}
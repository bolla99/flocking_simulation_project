using UnityEngine;

/// <summary>
/// Subclass of ScriptableObject that stores settings for the game.
/// The class expose methods for loading and saving the settings to a file.
/// </summary>
[CreateAssetMenu(fileName = "Settings", menuName = "Game/Settings")] 
public class Settings : ScriptableObject
{ 
    /// <summary>
    /// Number of boids that will compose the flock
    /// </summary>
    public int BoidsCount;
    /// <summary>
    /// Constant speed of the boids
    /// </summary>
    public float BoidsSpeed;
    /// <summary>
    /// Smoothing multiplier for the boids movement;
    /// The lower the value, the smoother the movement will be
    /// </summary>
    public float SmoothingMultiplier;
    /// <summary>
    /// The range within obstacles are detected
    /// </summary>
    public float ObstaclesDetectionRadius; 
    /// <summary>
    /// Distance value within which a boid and an obstacle are considered to collide
    /// </summary>
    public float CollisionPredictionThreshold;
    /// <summary>
    /// Obstacle minimum speed value
    /// </summary>
    public float ObstaclesMinSpeed;
    /// <summary>
    /// Obstacle maximum speed value
    /// </summary>
    public float ObstaclesMaxSpeed;
    /// <summary>
    /// Obstacle Count
    /// </summary>
    public int ObstaclesCount;

    /// <summary>
    ///  Separation weight value for the weighted blending of the flocking behaviours
    /// </summary>
    public float SeparationWeight;
    /// <summary>
    /// Cohesion weight value for the weighted blending of the flocking behaviours
    /// </summary>
    public float CohesionWeight;
    /// <summary>
    /// Alignment weight value for the weighted blending of the flocking behaviours
    /// </summary>
    public float AlignmentWeight;
    
    private static string _filePath;
    
    private void OnEnable() 
    { 
        _filePath = Application.persistentDataPath + "/settings.json";
    }
    
    /// <summary>
    /// Loads the settings from a file; if the file is not found, it creates a new one
    /// by saving the current settings.
    /// </summary>
    public void Load() 
    { 
        if (System.IO.File.Exists(_filePath)) 
        { 
            var json = System.IO.File.ReadAllText(_filePath); 
            JsonUtility.FromJsonOverwrite(json, this); 
        }
        else 
        { 
            Save(); 
        } 
    }

    /// <summary>
    /// Save the current settings to a file.
    /// </summary>
    public void Save() 
    { 
        var json = JsonUtility.ToJson(this); 
        System.IO.File.WriteAllText(_filePath, json); 
    } 
}

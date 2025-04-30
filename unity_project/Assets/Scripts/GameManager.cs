/// <summary>
/// Singleton Class that expose non-persistent settings
/// </summary>
public class GameManager
{
    /// <summary>
    /// Weather the target should follow the mouse position or not
    /// </summary>
    public bool MouseSeekingModeEnabled = false;
    
    /// <summary>
    /// Radius of detection of each boid with respect to the other boids;
    /// It affects the flocking behavior.
    /// It is meant to be set by a flocking manager based on the size of the flock
    /// </summary>
    public float BoidsDetectionRadius = 10f;
    
    private static GameManager _gameManager;
    
    /// <summary>
    /// Singleton instance
    /// </summary>
    /// <returns>
    /// The singleton instance of GameManager
    /// </returns>
    public static GameManager Instance()
    {
        if (_gameManager == null)
        {
            _gameManager = new GameManager();
        }
        return _gameManager;
    }
}

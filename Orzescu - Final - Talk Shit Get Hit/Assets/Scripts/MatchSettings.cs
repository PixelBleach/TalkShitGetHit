[System.Serializable]
public class MatchSettings {

    public float TimeForGame;
    public float basePlayerMoveSpeed = 4f;
    public float minimumPlayerMoveSpeed = 1f;
    public float moveSpeedDecay = 1f;
    public int scorePerHit = 50;
    public int scoreMultiplier = 2;
    public float maximumMultiplier = 16f;
    public float throwForceIncrease = 20f;
    public float maximumThrowForce = 30f;

}

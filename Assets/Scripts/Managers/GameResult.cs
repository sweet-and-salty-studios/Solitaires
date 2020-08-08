public struct GameResult
{
    public int MoveAmount
    {
        get;
        private set;
    }

    public float PlayTime
    {
        get;
        private set;
    }

    public GameResult(int movesAmount, float playTime)
    {
        MoveAmount = movesAmount;
        PlayTime = playTime;
    }
}
[System.Flags]
public enum GameState
{
    Default = 0,
    Playing = 1,
    Pause = 2,
    Lose = 4,
    Win = 8,
    AnimationOnPlay = 16,
    PowerUpInUsage = 32
}
public enum CellState
{
    Cross = 0,
    Square = 1
}
public enum InputType
{
    Square = 0,
    Cross = 1
}

public enum PowerUpType
{
    Rocket = 0,
    Bomb = 1,
    Fist = 2
}

public enum PoolItemType
{
    Cell = 0,
    Square = 1,
    Cross = 2,
    LeftText = 3,
    UpperText = 4,
    BeachMap = 5,
    ForestMap = 6,
    SnowMap = 7,
    LevelButton = 8,
    FirstBeachMap= 9,
    Rocket = 10,
    Bomb = 11,
    Fist = 12,
    StarIcon = 13,
    DiamondIcon = 14
    
}

public enum CurrencyItemType
{
    Rocket = 0,
    Bomb = 1,
    Fist = 2,
    Star = 3,
    Diamond = 4
}

public enum SoundEffectType
{
    ButtonClick = 0,
    WrongClick = 1,
    WinSound = 2,
    LoseSound = 3,
    LineCompletedSound = 4,
    RocketPowerUp = 5,
    BombPowerUp = 6,
    FistPowerUp = 7
}



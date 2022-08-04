public class Cell
{
    public const ConsoleColor Alive = ConsoleColor.DarkBlue, Dead = ConsoleColor.White;
    public CellState State;

    public Cell(CellState _state = CellState.Dead)
    {
        State = _state;
    }

    public void Show()
    {
        ConsoleColor cellColor = State == CellState.Alive ? Alive : Dead;
        Console.ForegroundColor = cellColor;

        Console.Write($"██");
        Console.ResetColor();
    }
}

public enum CellState
{
    Alive,
    Dead
}
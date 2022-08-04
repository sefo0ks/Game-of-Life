using System.Diagnostics;

class Program
{
    public const int Width = 80, Height = 54;
    public static Random rand = new Random(); 

    static bool RandomCells = true;

    static int UpdateRate = 350;
    static int Updates = 0;
    static Stopwatch updateStopwatch = new Stopwatch();

    private static void Main(string[] args)
    {
        Console.SetWindowSize(Width * 2, Height + 4);
        Console.SetBufferSize(Width * 2, Height + 4);

        while(true)
        {
            try
            {
                Console.Clear();
                Console.Write("Update rate (ms): ");
                int updateRate = int.Parse(Console.ReadLine());
                
                if (updateRate < 50)
                {
                    updateRate = 50;
                    Console.WriteLine("Update rate set to 50 since its minimum");
                    Console.ReadKey(true);
                }
                UpdateRate = updateRate;

                break;
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input. Type only numbers.");
                Console.ReadKey(true);
            }
        }
        Console.Clear();

        Cell[,] grid = CreateNewGrid();

        Reset(ref grid);
        ShowGrid(grid);

        updateStopwatch.Start();
        bool run = true;
        while (run)
        {
            if (updateStopwatch.ElapsedMilliseconds >= UpdateRate)
            {
                grid = UpdateGrid(grid);
                Updates++;
                //Console.ReadKey(true);
                ShowGrid(grid);
                Console.WriteLine("Press any key to restart...");

                if (CheckForAllDead(grid))
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Reset(ref grid);
                }

                updateStopwatch.Restart();
            }
            
            if (CheckForInput())
            {
                Console.ReadKey();
                Console.Clear();
                Reset(ref grid);
            }
        }
    }

    private static void Reset(ref Cell[,] _grid)
    {
        _grid = CreateNewGrid();
        Updates = 0;

        if (!RandomCells)
            PresetGrid(out _grid);
        
        ShowGrid(_grid);
    }

    private static Cell[,] CreateNewGrid(Cell[,]? basedOn = null)
    {
        Cell[,] _grid = new Cell[Height, Width];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _grid[y, x] = new Cell();

                if (basedOn != null)
                    _grid[y, x].State = basedOn[y, x].State;
                else if (RandomCells)
                {
                    if (y == 0 || x == 0 || y == Height - 1 || x == Width - 1)
                        _grid[y, x].State = CellState.Dead;
                    else
                        _grid[y, x].State = rand.Next(0, 11) <= 3 ? CellState.Alive : CellState.Dead;
                }
            }
        }

        return _grid;
    }

    private static Cell[,] UpdateGrid(Cell[,] _grid)
    {
        Cell[,] _oldGrid = CreateNewGrid(_grid);
        Cell[,] _newGrid = CreateNewGrid(_grid);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int neighbors = CountNeighbors(_oldGrid, y, x);
                
                if (_oldGrid[y, x].State == CellState.Alive &&
                    (neighbors < 2 || neighbors > 3))
                    _newGrid[y, x].State = CellState.Dead;
                else if (_oldGrid[y, x].State == CellState.Dead && neighbors == 3)
                    _newGrid[y, x].State = CellState.Alive;
            }
        }

        return _newGrid;
    }

    private static int CountNeighbors(Cell[,] _grid, int y, int x)
    {
        int neighbors = 0;
        if (y == 0 || x == 0 || y == Height - 1 || x == Width - 1)
            return 0;
        
        for (int yOff = -1; yOff <= 1; yOff++)
        {
            for (int xOff = -1; xOff <= 1; xOff++)
            {
                if (xOff == 0 && yOff == 0)
                    continue;

                if (_grid[y + yOff, x + xOff].State == CellState.Alive)
                    neighbors++;
            }
        }

        return neighbors;
    }

    private static void ShowGrid(Cell[,] _grid)
    {
        Console.SetCursorPosition(0, 0);

        Console.WriteLine("Generations: " + Updates);
        for (int y = 1; y < Height - 1; y++)
        {
            for (int x = 1; x < Width - 1; x++)
            {
                _grid[y, x].Show();
            }
            Console.WriteLine();
        }
    }

    private static bool CheckForAllDead(Cell[,] _grid)
    {
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                if (_grid[y, x].State == CellState.Alive)
                    return false;

        return true;
    }

    private static void PresetGrid(out Cell[,] _grid)
    {
        _grid = CreateNewGrid();

        _grid[1, 1].State = CellState.Alive;
        _grid[1, 3].State = CellState.Alive;
        _grid[2, 2].State = CellState.Alive;
        _grid[2, 3].State = CellState.Alive;
        _grid[3, 2].State = CellState.Alive;
    }

    private static bool CheckForInput()
    {
        if (!Console.KeyAvailable)
            return false;
        
        return true;
    }
}
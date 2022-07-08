var game = new Game();
game.PlayGame();

class Board
{
    private readonly Shape[] _squares = new Shape[9];
    private int _filledSquares;
    private static readonly Dictionary<Shape, string> ShapesToDraw = new()
    {
        {Shape.None, " "},
        {Shape.X, "X"},
        {Shape.O, "O"},
    };
    private static readonly Dictionary<Shape, BoardState> ShapeToWin = new()
    {
        { Shape.X, BoardState.XWin },
        { Shape.O, BoardState.OWin }
    };
    
    public static void DrawDirectionsBoard()
    {
        Console.WriteLine("Please play the game using the numpad based on the sample board provided below.");
        Console.WriteLine($" 7 | 8 | 9 ");
        Console.WriteLine("---+---+---");
        Console.WriteLine($" 4 | 5 | 6 ");
        Console.WriteLine("---+---+---");
        Console.WriteLine($" 1 | 2 | 3 ");
    }
    public void DrawBoard()
    {
        Console.WriteLine($" {ShapesToDraw[_squares[6]]} | {ShapesToDraw[_squares[7]]} | {ShapesToDraw[_squares[8]]} ");
        Console.WriteLine("---+---+---");
        Console.WriteLine($" {ShapesToDraw[_squares[3]]} | {ShapesToDraw[_squares[4]]} | {ShapesToDraw[_squares[5]]} ");
        Console.WriteLine("---+---+---");
        Console.WriteLine($" {ShapesToDraw[_squares[0]]} | {ShapesToDraw[_squares[1]]} | {ShapesToDraw[_squares[2]]} ");
    }

    public void FillSquare(int square, Shape shape)
    {
        while (_squares[square] != Shape.None)
        {
            Console.WriteLine("This square has already been filled");
            square = Game.GetPlayerInput();
        }
        _filledSquares++;
        _squares[square] = shape;
    }
    
    public BoardState CheckBoard(Shape shape)
    {
        if (_squares[0] == shape && _squares[1] == shape && _squares[2] == shape) return ShapeToWin[shape];
        if (_squares[3] == shape && _squares[4] == shape && _squares[5] == shape) return ShapeToWin[shape];
        if (_squares[6] == shape && _squares[7] == shape && _squares[8] == shape) return ShapeToWin[shape];
        if (_squares[0] == shape && _squares[3] == shape && _squares[6] == shape) return ShapeToWin[shape];
        if (_squares[1] == shape && _squares[4] == shape && _squares[7] == shape) return ShapeToWin[shape];
        if (_squares[2] == shape && _squares[5] == shape && _squares[8] == shape) return ShapeToWin[shape];
        if (_squares[0] == shape && _squares[4] == shape && _squares[8] == shape) return ShapeToWin[shape];
        if (_squares[6] == shape && _squares[4] == shape && _squares[2] == shape) return ShapeToWin[shape];
        
        return _filledSquares < 9 ? BoardState.Ongoing : BoardState.Draw;
    }
}

class Player
{
    public Shape Shape { get; }

    public Player(Shape shape)
    {
        Shape = shape;
    }
}

class Game
{
    private readonly Player _playerX;
    private readonly Player _playerO;
    private readonly Board _board;
    private const int BoardSize = 9;

    public Game()
    {
        _playerX = new Player(Shape.X);
        _playerO = new Player(Shape.O);
        _board = new Board();
    }

    public static int GetPlayerInput()
    {
        int playerInput;
        bool playerInputValid;
        do
        {
            Console.Write("Please enter a number between 1 and 9: ");
            playerInputValid = Int32.TryParse(Console.ReadLine(), out playerInput);
        } while (!playerInputValid || playerInput is < 1 or > 9 );
        // Subtract 1 in order to convert player selection to array indices
        return playerInput - 1;
    }

    private void PlayRound(Shape shape)
    {
        Console.WriteLine($"It is {shape}'s turn.");
        var playerInput = GetPlayerInput();
        _board.FillSquare(playerInput, shape);
    }
    
    public void PlayGame()
    {
        Board.DrawDirectionsBoard();
        for (var i = 0; i < BoardSize; i++)
        {
            var shape = i % 2 == 0 ? _playerX.Shape : _playerO.Shape; 
            PlayRound(shape);
            _board.DrawBoard();
            var boardState = _board.CheckBoard(shape);
            switch (boardState)
            {
                case BoardState.XWin:
                    Console.WriteLine("Player X has won");
                    return;
                case BoardState.OWin:
                    Console.WriteLine("Player O has won");
                    return;
                case BoardState.Draw:
                    Console.WriteLine("The game was a draw");
                    return;
            }
        }
    }
}

enum BoardState { Ongoing, XWin, OWin, Draw }

enum Shape { None , X, O}
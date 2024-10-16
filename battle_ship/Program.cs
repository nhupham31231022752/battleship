using System;
using System.Collections.Generic;

namespace BattleshipGame
{
    public class Ship
    {
        public string Name { get; }
        public int Size { get; }
        public char Symbol { get; }
        public List<(int row, int col)> Positions { get; private set; }

        public Ship(string name, int size, char symbol)
        {
            Name = name;
            Size = size;
            Symbol = symbol;
            Positions = new List<(int, int)>();
        }

        public void PlaceShip(List<(int row, int col)> positions)
        {
            Positions = positions;
        }
    }

    public class GridManager
    {
        private const int GridSize = 10;
        private char[,] grid;
        private List<Ship> ships;

        public GridManager()
        {
            grid = new char[GridSize, GridSize];
            ships = new List<Ship>();
            InitializeGrid();
        }

        public void InitializeGrid()
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    grid[i, j] = '~'; // Initialize grid with water
                }
            }
        }

        public void AddShip(Ship ship)
        {
            ships.Add(ship);
            foreach (var pos in ship.Positions)
            {
                grid[pos.row, pos.col] = ship.Symbol; // Mark ship position on the grid with its symbol
            }
        }

        public bool IsPlacementValid(Ship ship, List<(int row, int col)> positions)
        {
            foreach (var pos in positions)
            {
                // Check if the position is valid
                if (pos.row < 0 || pos.row >= GridSize || pos.col < 0 || pos.col >= GridSize || grid[pos.row, pos.col] != '~')
                {
                    return false; // Invalid placement
                }
            }
            return true; // Valid placement
        }

        public void DisplayGrid()
        {
            Console.WriteLine(" A B C D E F G H I J");
            for (int i = 0; i < GridSize; i++)
            {
                Console.Write(i + 1 + " ");
                for (int j = 0; j < GridSize; j++)
                {
                    Console.Write(grid[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            GridManager gridManager = new GridManager();

            // Define ships with their symbols and sizes
            List<Ship> ships = new List<Ship>
            {
                new Ship("Tàu Sân Bay", 5, 'A'),   // Ký hiệu A
                new Ship("Hạm Chiến", 4, 'B'),     // Ký hiệu B
                new Ship("Tàu Khu Trục", 3, 'C'),   // Ký hiệu C
                new Ship("Tàu Ngầm", 2, 'D'),       // Ký hiệu D
                new Ship("Tàu Khu Trục Nhỏ", 1, 'E') // Ký hiệu E
            };

            foreach (var ship in ships)
            {
                bool validPlacement = false;
                while (!validPlacement)
                {
                    Console.WriteLine($"Nhập tọa độ cho {ship.Name} (kích thước {ship.Size}):");
                    Console.Write("Nhập hàng (1-10): ");
                    int row = int.Parse(Console.ReadLine()) - 1; // Convert to 0-based index
                    Console.Write("Nhập cột (A-J): ");
                    char colChar = char.ToUpper(Console.ReadLine()[0]);
                    int col = colChar - 'A'; // Convert to 0-based index

                    Console.Write("Nhập hướng (Dọc [V] hoặc Ngang [H]): ");
                    string direction = Console.ReadLine().ToUpper();

                    List<(int row, int col)> positions = new List<(int, int)>();

                    // Determine positions based on the direction
                    if (direction == "V") // Vertical
                    {
                        for (int i = 0; i < ship.Size; i++)
                        {
                            positions.Add((row + i, col));
                        }
                    }
                    else if (direction == "H") // Horizontal
                    {
                        for (int i = 0; i < ship.Size; i++)
                        {
                            positions.Add((row, col + i));
                        }
                    }

                    // Check if placement is valid
                    if (gridManager.IsPlacementValid(ship, positions))
                    {
                        ship.PlaceShip(positions);
                        gridManager.AddShip(ship);
                        Console.WriteLine($"{ship.Name} đã được đặt thành công.");
                        validPlacement = true; // Break out of the loop on successful placement
                    }
                    else
                    {
                        Console.WriteLine("Vị trí đặt không hợp lệ. Vui lòng thử lại.");
                    }
                }
            }

            // Display the grid with ships placed
            Console.WriteLine("\nLưới hiện tại:");
            gridManager.DisplayGrid();
        }
    }
}

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
                int row = 0;
                int col = 0;
                char colChar = 'A';
                string direction = "";
                Console.WriteLine($"Nhập tọa độ cho {ship.Name} (kích thước {ship.Size}):");

                // Nhập hàng với kiểm tra hợp lệ
                bool validRow = false;
                while (!validRow)
                {
                    Console.Write("Nhập hàng (1-10): ");
                    string rowInput = Console.ReadLine();
                    if (int.TryParse(rowInput, out row))
                    {
                        if (row >= 1 && row <= 10)
                        {
                            row -= 1; // Chuyển đổi sang chỉ số 0-based
                            validRow = true;
                        }
                        else
                        {
                            Console.WriteLine("Số hàng không hợp lệ. Vui lòng nhập lại từ 1 đến 10.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Dữ liệu nhập vào không phải là số. Vui lòng nhập lại.");
                    }
                }

                // Nhập cột với kiểm tra hợp lệ
                bool validCol = false;
                while (!validCol)
                {
                    Console.Write("Nhập cột (A-J): ");
                    string colInput = Console.ReadLine();
                    if (!string.IsNullOrEmpty(colInput))
                    {
                        colChar = char.ToUpper(colInput[0]);
                        if (colChar >= 'A' && colChar <= 'J')
                        {
                            col = colChar - 'A'; // Chuyển đổi sang chỉ số 0-based
                            validCol = true;
                        }
                        else
                        {
                            Console.WriteLine("Cột không hợp lệ. Vui lòng nhập lại từ A đến J.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Dữ liệu nhập vào không hợp lệ. Vui lòng nhập lại.");
                    }
                }

                // Nhập hướng với kiểm tra hợp lệ
                bool validDirection = false;
                while (!validDirection)
                {
                    Console.Write("Nhập hướng (Dọc [V] hoặc Ngang [H]): ");
                    direction = Console.ReadLine().ToUpper();
                    if (direction == "V" || direction == "H")
                    {
                        validDirection = true;
                    }
                    else
                    {
                        Console.WriteLine("Hướng không hợp lệ. Vui lòng nhập lại bằng V hoặc H.");
                    }
                }

                List<(int row, int col)> positions = new List<(int, int)>();

                // Xác định vị trí dựa trên hướng
                if (direction == "V") // Dọc
                {
                    for (int i = 0; i < ship.Size; i++)
                    {
                        positions.Add((row + i, col));
                    }
                }
                else if (direction == "H") // Ngang
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

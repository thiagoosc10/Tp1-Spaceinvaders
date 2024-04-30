using System;
using System.Threading;

namespace SpaceInvaders
{
    class Program
    {
        static int enemigosRestantes = 0;

        static void Main()
        {
            int width = 12;
            int height = 12;

           
            char[,] room = new char[width, height];

           
            Random random = new Random();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        room[x, y] = '#';
                    }
                    else if (random.Next(7) < 3 && enemigosRestantes < 12) // Esto hace que haya una probabilidad del 42% de que haya un enemigo en cada celda
                    {
                        if (random.Next(2) == 0)
                        {
                            room[x, y] = '='; 
                        }
                        else
                        {
                            room[x, y] = '='; 
                        }
                        enemigosRestantes++;
                    }
                    else
                    {
                        room[x, y] = ' ';
                    }
                }
            }

            int playerX = width / 2;
            int playerY = height - 2;
            
            {
                playerX--;
            }
            room[playerX, playerY] = '^'; 

            DibujarMapa(room);

            // Esta lista lo que hace es almacenar la posición de los disparos
            var shots = new List<(int x, int y)>();

            
            while (enemigosRestantes > 0)
            {
                
                MoverDisparos(room, shots);

                
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                
                switch (keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                        MoverJugador(room, ref playerX, ref playerY, -1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        MoverJugador(room, ref playerX, ref playerY, 1, 0);
                        break;
                    case ConsoleKey.UpArrow:
                        MoverJugador(room, ref playerX, ref playerY, 0, -1);
                        break;
                    case ConsoleKey.DownArrow:
                        MoverJugador(room, ref playerX, ref playerY, 0, 1);
                        break;
                    case ConsoleKey.Spacebar:
                        Shoot(room, playerX, playerY, shots);
                        break;
                }

                Console.Clear();
                DibujarMapa(room);

            }

            if (enemigosRestantes == 0)
            {
                Console.WriteLine("¡Victoria!");
            }
        }

        // Esta función sirve para crear la habitación en la consola
        static void DibujarMapa(char[,] room)
        {
            int width = room.GetLength(0);
            int height = room.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(room[x, y] + " ");
                }
                Console.WriteLine();
            }
        }

        // Esta función sirve para mover al jugador dentro de la habitación
        static void MoverJugador(char[,] room, ref int playerX, ref int playerY, int deltaX, int deltaY)
        {
            int newX = playerX + deltaX;
            int newY = playerY + deltaY;

            if (room[newX, newY] != '#')
            {
                if (room[newX, newY] == '=')
                {
                    playerX = room.GetLength(0) / 2;
                    playerY = room.GetLength(1) - 2;
                }
                else
                {
                    room[playerX, playerY] = ' ';
                    playerX = newX;
                    playerY = newY;
                    room[playerX, playerY] = '^';
                }
            }
        }

        // Esta función sirve para que el jugador pueda disparar
        static void Shoot(char[,] room, int playerX, int playerY, List<(int x, int y)> shots)
        {
            shots.Add((playerX, playerY - 1));
        }

        // Esta función sirve para mover los disparos
        static void MoverDisparos(char[,] room, List<(int x, int y)> shots)
        {
            for (int i = 0; i < shots.Count; i++)
            {
                int x = shots[i].x;
                int y = shots[i].y;

                if (room[x, y - 1] == '=')
                {
                   
                    room[x, y - 1] = ' ';
                    enemigosRestantes--; 
                    room[x, y] = ' '; // Con esto se elimina el disparo cuando toca a un enemigo
                    shots.RemoveAt(i);
                    i--;
                }
                else if (y - 1 >= 0 && room[x, y - 1] != '#') // Esto verifica que el disparo está dentro de los límites de la habitación y no choca contra una pared
                {
                    room[x, y] = ' ';
                    shots[i] = (x, y - 1);
                    room[x, y - 1] = '-';
                }

                else // Acá se elimina el disparo cuando toca el techo

                {
                     room[x, y] = ' ';
                     shots.RemoveAt(i);
                     i--;
                }
            }
        }
    }
}
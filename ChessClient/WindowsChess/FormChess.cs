using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chess;
using ChessClient;

namespace WindowsChess
{
    public partial class FormChess : Form
    {
        public const string HOST = "http://localhost:50207/api/games/";
        const int SIZE = 50;
        Panel[,] map;
        Chess.Chess chess;
        ChessClient.ChessClient client;
        bool wait; // true = no move
        int xFrom, yFrom;
        public FormChess()
        {
            InitializeComponent();
            client = new ChessClient.ChessClient(HOST);
            InitPanels();
            wait = true;
            RefreshPosition();
        }

        void RefreshPosition()
        {
            chess = new Chess.Chess(client.GetCurrentGame().FEN);
            ShowPosition();
        }
        void InitPanels()
        {
            map = new Panel[8, 8];
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    map[x, y] = AddPanel(x, y);
        }
        void ShowPosition()
        {
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    ShowFigure(x, y, chess.GetFigureAt(x, y));
            MarkSquares();
        }
        void ShowFigure(int x, int y, char figure)
        {
            map[x, y].BackgroundImage = GetFigureImage(figure);
        }
        Image GetFigureImage(char figure)
        {
            switch (figure)
            {
                case 'R': return Properties.Resources.WhiteRook;
                case 'N': return Properties.Resources.WhiteKnight;
                case 'B': return Properties.Resources.WhiteBishop;
                case 'Q': return Properties.Resources.WhiteQueen;
                case 'K': return Properties.Resources.WhiteKing;
                case 'P': return Properties.Resources.WhitePawn;

                case 'r': return Properties.Resources.BlackRook;
                case 'n': return Properties.Resources.BlackKnight;
                case 'b': return Properties.Resources.BlackBishop;
                case 'q': return Properties.Resources.BlackQueen;
                case 'k': return Properties.Resources.BlackKing;
                case 'p': return Properties.Resources.BlackPawn;

                default: return null;
            }
        }
        Panel AddPanel (int x, int y)
        {
            Panel panel = new System.Windows.Forms.Panel();
            panel.BackColor = GetColor(x, y);
            panel.Location = GetLocation(x, y);
            panel.Name = "p" + x + y;
            panel.Size = new System.Drawing.Size(SIZE, SIZE);
            panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            
            panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_MouseDown);
            board.Controls.Add(panel);
            return panel;
        }

        Color GetColor(int x, int y)
        {
            return (x + y) % 2 == 0 ? Color.DarkGray : Color.LightGray;
        }
        Color GetMarkedColor(int x, int y)
        {
            return (x + y) % 2 == 0 ? Color.Green : Color.LightGreen;
        }

        Point GetLocation (int x, int y)
        {
            return new Point(SIZE / 2 + x * SIZE, SIZE / 2 + (7 - y) * SIZE);
        }

        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            string xy = ((Panel)sender).Name.Substring(1); //01
            int x = xy[0] - '0';
            int y = xy[1] - '0';

           if (wait)
            {
                wait = false;
                xFrom = x;
                yFrom = y;
            }
            else
            {
                wait = true;
                //Pe2e4
                string figure = chess.GetFigureAt(xFrom, yFrom).ToString();
                string move = figure + ToCoord(xFrom, yFrom) + ToCoord(x, y);
                chess = new Chess.Chess(client.SendMove(move).FEN);
            }
            ShowPosition();
        }

        void MarkSquares()
        {
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    map[x, y].BackColor = GetColor(x, y);

            if (wait)
                MarkSquaresFrom();
            else
                MarkSquaresTo();
        }
        void MarkSquaresFrom()
        {
            foreach(string move in chess.GetAllMoves()) //Pe2e4 xy = e2
            {
                int x = move[1] - 'a';
                int y = move[2] - '1';
                map[x, y].BackColor = GetMarkedColor(x, y);
            }
        }

        void MarkSquaresTo()
        {
            string suffix = chess.GetFigureAt(xFrom, yFrom) + ToCoord(xFrom, yFrom);
            foreach (string move in chess.GetAllMoves()) //Pe2e4 xy = e4
                if (move.StartsWith (suffix))
                {
                    int x = move[3] - 'a';
                    int y = move[4] - '1';
                    map[x, y].BackColor = GetMarkedColor(x, y);
                }
        }

        private void FormChess_MouseClick(object sender, MouseEventArgs e)
        {
            RefreshPosition();
        }

        string ToCoord ( int x, int y)
        {
            return ((char)('a' + x)).ToString() + ((char)('1' + y)).ToString(); 
        }
    }
}

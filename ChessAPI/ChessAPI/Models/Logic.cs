using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Chess;

namespace ChessAPI.Models
{
    public class Logic
    {
        private ModelChessDB db;

        public Logic()
        {
            db = new ModelChessDB();
        }
        internal Game GetCurrentGame()
        {

            Game game = db.Games.Where(g => g.Status == "play").OrderBy(g => g.ID).FirstOrDefault();
            if (game == null)
                game = CreateNewGame();
            return game;
        }

        public Game GetGame (int id)
        {
            return db.Games.Find(id);
        }

        private Game CreateNewGame()
        {
            Game game = new Game();

            Chess.Chess chess = new Chess.Chess();

            game.FEN = chess.fen;
            game.Status = "play";

            db.Games.Add(game);
            db.SaveChanges();

            return game;
        }
        public Game MakeMove(int id, string move)
        {
            Game game = GetGame(id);

            if (game == null)
                return game;
            if (game.Status != "play")
                return game;

            Chess.Chess chess = new Chess.Chess(game.FEN);
            Chess.Chess chessNext = chess.Move(move);

            if (chessNext.fen == game.FEN)
                return game;
            game.FEN = chessNext.fen;
           // if (chessNext.IsCheckMate || chess.IsStalemate)
           //     game.Status = "done";
            db.Entry(game).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return game;
        }
    }
}
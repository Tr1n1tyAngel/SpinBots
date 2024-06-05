using System;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxAI
{
    public Move GetBestMove(GameState initialState)
    {
        int depth = 3; // Define the depth of the Minimax search
        bool maximizingPlayer = true; // AI starts as maximizing player

        return Minimax(initialState, depth, maximizingPlayer).Item2;
    }

    private Tuple<int, Move> Minimax(GameState state, int depth, bool maximizingPlayer)
    {
        if (depth == 0 || state.IsTerminal())
        {
            return new Tuple<int, Move>(Evaluate(state), null);
        }

        List<Move> possibleMoves = GetPossibleMoves(state);

        if (maximizingPlayer)
        {
            int maxEval = int.MinValue;
            Move bestMove = null;

            foreach (Move move in possibleMoves)
            {
                GameState newState = ApplyMove(state.Clone(), move);
                int eval = Minimax(newState, depth - 1, false).Item1;

                if (eval > maxEval)
                {
                    maxEval = eval;
                    bestMove = move;
                }
            }

            return new Tuple<int, Move>(maxEval, bestMove);
        }
        else
        {
            int minEval = int.MaxValue;
            Move bestMove = null;

            foreach (Move move in possibleMoves)
            {
                GameState newState = ApplyMove(state.Clone(), move);
                int eval = Minimax(newState, depth - 1, true).Item1;

                if (eval < minEval)
                {
                    minEval = eval;
                    bestMove = move;
                }
            }

            return new Tuple<int, Move>(minEval, bestMove);
        }
    }

    private int Evaluate(GameState state)
    {
        // Evaluate the game state from AI perspective
        // Higher value means better for AI
        return (int)(state.p2AttackStatAI + state.p2DefenseStatAI + state.p2StaminaStatAI -
                     state.p1AttackStatAI - state.p1DefenseStatAI - state.p1StaminaStatAI);
    }

    private List<Move> GetPossibleMoves(GameState state)
    {
        List<Move> moves = new List<Move>();

        if (!state.p2BotSelectedAI)
        {
            moves.Add(new Move("BeybladeSelection", "Attack"));
            moves.Add(new Move("BeybladeSelection", "Defense"));
            moves.Add(new Move("BeybladeSelection", "Stamina"));
        }

        // Add other possible moves based on the current state of the game
        // e.g., obstacle selection, powerup selection, ADS, Buff, etc.

        return moves;
    }

    private GameState ApplyMove(GameState state, Move move)
    {
        switch (move.Type)
        {
            case "BeybladeSelection":
                if (move.Choice == "Attack")
                {
                    state.p2AttackStatAI = 8;
                    state.p2DefenseStatAI = 3;
                    state.p2StaminaStatAI = 4;
                }
                else if (move.Choice == "Defense")
                {
                    state.p2AttackStatAI = 3;
                    state.p2DefenseStatAI = 8;
                    state.p2StaminaStatAI = 4;
                }
                else if (move.Choice == "Stamina")
                {
                    state.p2AttackStatAI = 4;
                    state.p2DefenseStatAI = 3;
                    state.p2StaminaStatAI = 8;
                }
                state.p2BotSelectedAI = true;
                break;

                // Handle other types of moves: obstacle selection, powerup selection, ADS, Buff, etc.
        }

        return state;
    }
}


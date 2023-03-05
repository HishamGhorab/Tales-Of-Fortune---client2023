using System.Collections.Generic;

// The interface for using Minimax
// It has its own namespace to be reusable

namespace Minimax
{
    public interface IState
    {
        List<IState> Expand(IPlayer player);  // Generate a list of the states reachable from the current state by player
        double Score(IPlayer player);            // The heuristic score for player in the current state
    }

    public interface IPlayer  // The IPlayer interface is just for identity, does not require any methods
    {

    }
}
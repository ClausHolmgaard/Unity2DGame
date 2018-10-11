using System.Collections;
using System.Collections.Generic;

public class GameState {

	public enum GameStates {
        Running,
        GameOver
    }

    public enum GameEvents {
        GameStart,
        GameEnd
    }

    private GameStates currentState = GameStates.Running;

    public GameStates getState() {
        return currentState;
    }

    public void newEvent(GameEvents e) {
        switch (currentState) {
            case GameStates.Running:
                switch (e) {
                    case GameEvents.GameStart:
                        // Do nothing
                        break;
                    case GameEvents.GameEnd:
                        currentState = GameStates.GameOver;
                        break;
                }
                break;
            case GameStates.GameOver:
                switch (e) {
                    case GameEvents.GameStart:
                        currentState = GameStates.Running;
                        break;
                    case GameEvents.GameEnd:
                        // Do nothing
                        break;
                }
                break;
        }
    }
}

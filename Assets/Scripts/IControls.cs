using System.Collections;
using System.Collections.Generic;

interface IControls {
    void enable();
    void disable();
    void handleWeapons();
    void setGrounded(bool grounded);
    void handleMoveStates();
    void doMovement();
    void die();
}

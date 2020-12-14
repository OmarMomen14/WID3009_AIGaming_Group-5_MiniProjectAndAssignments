package pacman.entries.pacman.bt.leaf;

import pacman.entries.pacman.bt.Blackboard;
import pacman.entries.pacman.bt.TreeNode;
import pacman.entries.pacman.bt.utils.IControllerActions;

public abstract class ActionNode extends TreeNode {

    protected IControllerActions controllerActions;
    protected Blackboard blackboard;

    protected ActionNode(String name, Blackboard blackboard, IControllerActions controllerActions) {
        super(name);
        this.blackboard = blackboard;
        this.controllerActions = controllerActions;
    }
}

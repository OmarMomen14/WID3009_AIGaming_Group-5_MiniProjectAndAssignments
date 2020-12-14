package pacman.entries.pacman.bt.composite;

import pacman.entries.pacman.bt.Status;
import pacman.entries.pacman.bt.TreeNode;

public class Selector extends CompositeNode {

    public Selector(String name) {
        super(name + " Selector");
    }

    @Override
    protected void initialize() {}

    @Override
    protected Status update() {

        // find the first child which is not a failure.
        for (TreeNode child : children) {
            Status childStatus = child.tick();

            if(childStatus != Status.FAILURE) {
                return childStatus;
            }
        }

        return Status.FAILURE;
    }

    @Override
    protected void postUpdate() {

    }
}
